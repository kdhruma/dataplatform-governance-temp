using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Xml;
using System.Threading;
using System.Collections.ObjectModel;

namespace MDM.SecurityManager.Business
{
    using MDM.AdminManager.Business;
    using MDM.BusinessObjects;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.MessageManager.Business;
    using MDM.NotificationManager.Business;
    using MDM.SecurityManager.Data.SqlClient;
    using MDM.Utility;

    /// <summary>
    /// Contains business logic for the user credential requests
    /// </summary>
    public class UserCredentialRequestBL : BusinessLogicBase
    {
        #region Fields

        private readonly SecurityPrincipal _securityPrincipal;

        private const String _noUsersWithEmail = "112976";  // User with email {0} not found
        private const String _userWithoutEmail = "112985"; // Your email address is not on our records.
        private const String _manyUsersWithEmail = "112977";  // There are multiple users with email {0}
        private const String _disbledUser = "112978";  // Password cannot be recovered for disabled user
        private const String _noUserWithLogin = "112979";  // User with login id {0} not found
        private const String _noUserWithEmailNLogin = "112980"; // User with login id {0} and email id {1} not found
        private const String _contactSystemAdmin = "100792"; // Please contact System Administrator

        #endregion

        #region Constructors

        /// <summary>
        /// Business Logic class constructor
        /// </summary>
        public UserCredentialRequestBL()
        {
            _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
        }

        #endregion

        #region Methods

        /// <summary>
        //  Processes the user credential request by sending email to the registered email id based on the requested context
        /// </summary>
        /// <param name="userRequestContext">Indicates context for the user credential request</param>
        /// <param name="callerContext">Indicates name of application and module</param>
        /// <returns>Operation result containing detail information of process user credential request</returns>
        public OperationResult ProcessUserCredentialRequest(UserCredentialRequestContext userRequestContext, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("UserCredentialRequestBL.ProcessUserCredentialRequest", MDMTraceSource.SecurityService, false);

            OperationResult operationResult = new OperationResult();
            SecurityUserCollection securityUsers = new SecurityUserCollection();
            SecurityUser user;
            String errorMessage = String.Empty;

            try
            {
                SecurityUserBL securityUserBL = new SecurityUserBL();
                securityUsers = securityUserBL.SearchUsers(userRequestContext.UserLoginId, userRequestContext.UserEmailId, callerContext);

                if (securityUsers.Count == 1)
                {
                    user = securityUsers.First();

                    if (user.Disabled || user.IsWindowsAuthentication)
                    {
                        operationResult.AddOperationResult("", this.GetMessageWithAdminContactDetails(_disbledUser), OperationResultType.Error);
                    }
                    else if (String.IsNullOrWhiteSpace(user.Smtp))
                    {
                        operationResult.AddOperationResult("", this.GetMessageWithAdminContactDetails(_userWithoutEmail), OperationResultType.Error);
                    }
                    else
                    {
                        switch (userRequestContext.RequestType)
                        {
                            case UserCredentialRequestType.Password:

                                operationResult.CopyErrorInfoAndWarning(this.RegisterPasswordResetRequest(user, callerContext));
                                break;
                            case UserCredentialRequestType.LoginId:

                                String forgotPaswordUrl = this.BuildForgotPaswordUrl();
                                operationResult.CopyErrorInfoAndWarning(new MailNotificationBL().SendMailOnLoginForgotRequest(user.SecurityUserLogin, forgotPaswordUrl, user.Smtp, callerContext));
                                break;
                        }

                        this.RefreshOperationResultStatus(operationResult);

                        if (operationResult.OperationResultStatus == OperationResultStatusEnum.Successful)
                        {
                            String[] emailParts = user.Smtp.Split('@');
                            char[] maskedEmailID = emailParts[0].ToCharArray();

                            for (int i = 0; i < maskedEmailID.Length; i++)
                            {
                                if (i >= 3)
                                    maskedEmailID[i] = '*';
                            }

                            operationResult.ReturnValues.Add(String.Format("Email has been sent to {0}@{1}", new String(maskedEmailID), emailParts[1]));
                        }
                    }
                }
                else
                {
                    if (securityUsers.Count == 0)
                    {
                        if (!String.IsNullOrWhiteSpace(userRequestContext.UserEmailId) && !String.IsNullOrWhiteSpace(userRequestContext.UserLoginId))
                            errorMessage = String.Format(this.GetMessageWithAdminContactDetails(_noUserWithEmailNLogin), userRequestContext.UserLoginId, userRequestContext.UserEmailId);

                        else if (!String.IsNullOrWhiteSpace(userRequestContext.UserEmailId))
                            errorMessage = String.Format(this.GetMessageWithAdminContactDetails(_noUsersWithEmail), userRequestContext.UserEmailId);

                        else if (!String.IsNullOrWhiteSpace(userRequestContext.UserLoginId))
                            errorMessage = String.Format(this.GetMessageWithAdminContactDetails(_noUserWithLogin), userRequestContext.UserEmailId);
                    }
                    else
                    {
                        errorMessage = String.Format(this.GetMessageWithAdminContactDetails(_manyUsersWithEmail), userRequestContext.UserEmailId);
                    }
                }

                if (!String.IsNullOrEmpty(errorMessage))
                {
                    operationResult.Errors.Add(new Error(String.Empty, errorMessage));
                    operationResult.OperationResultStatus = OperationResultStatusEnum.Failed;
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("UserCredentialRequestBL.ProcessUserCredentialRequest", MDMTraceSource.SecurityService);
            }

            return operationResult;
        }

        #region Helper Methods

        private String GetMessageWithAdminContactDetails(String messageCode)
        {
            LocaleMessageBL localeManager = new LocaleMessageBL();
            Collection<String> messageCodes = new Collection<string>() 
            {
                _contactSystemAdmin, messageCode
            };

            LocaleMessageCollection localeMessages = localeManager.Get(GlobalizationHelper.GetSystemUILocale(), messageCodes, false, new CallerContext { Application = MDMCenterApplication.MDMCenter, Module = MDMCenterModules.Security });
            String adminEmailId = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.Administration.AdminEmailId", String.Empty);

            String contactSystemAdmin = String.Format("{0}. {1}", localeMessages[0].Message, adminEmailId);

            return String.Format("{0}. {1}", localeMessages[1].Message, contactSystemAdmin);
        }

        private OperationResult RegisterPasswordResetRequest(SecurityUser user, CallerContext callerContext)
        {
            String token = this.GenerateToken(user);
            PasswordResetRequest passwordResetRequest = new PasswordResetRequest();
            passwordResetRequest.SecurityUserId = user.Id;
            passwordResetRequest.Token = token;
            passwordResetRequest.RequestedDateTime = DateTime.Now;

            String resetPasswordUrl = this.BuildPaswordResetUrl(token);
            OperationResult operationResult = this.Process(passwordResetRequest, callerContext);
            UserPreferences userPreferences = new UserPreferencesBL().GetUserPreferences(user.SecurityUserLogin);
            DateTime currentTime = DateTime.Now;
            String strCurrentTime = currentTime.ToString();

            //Format in user’s default time zone. It should be the Time Zone Id which is of String type.Also the DefaultTimeZoneShortName is one of the Time Zone Ids
            String wcfTimeZone = TimeZoneInfo.Local.Id;
            if (userPreferences != null && !String.IsNullOrWhiteSpace(userPreferences.DefaultTimeZoneShortName) && wcfTimeZone != userPreferences.DefaultTimeZoneShortName)
            {
                currentTime = FormatHelper.ConvertToTimeZone(DateTime.Now, wcfTimeZone, userPreferences.DefaultTimeZoneShortName);
                strCurrentTime = FormatHelper.FormatDate(currentTime.ToString(), Thread.CurrentThread.CurrentCulture.Name, userPreferences.UILocale.GetCultureName());
            }

            Int32 validityPeriod = AppConfigurationHelper.GetAppConfig<Int32>("MDMCenter.SecurityManager.ResetPasswordToken.ValidityPeriod", 24);

            operationResult.CopyErrorInfoAndWarning(new MailNotificationBL().SendMailOnPasswordForgotRequest(resetPasswordUrl, strCurrentTime, validityPeriod.ToString(), user.Smtp, callerContext));

            return operationResult;
        }

        private String GenerateToken(SecurityUser user)
        {
            // PasswordRequestId has to be unique and encrypted, hence a GUID can be used along with a hashed value of the user login id to populate this field.
            String token = BasicEncryptionHelper.Encrypt(user.SecurityUserLogin);
            return WebUtility.UrlEncode(String.Format("{0}_{1}", token, Guid.NewGuid()));
        }

        private OperationResult Process(PasswordResetRequest passwordResetRequest, CallerContext callerContext)
        {
            OperationResult operationResult = new OperationResult();

            PasswordResetRequestDA passwordResetRequestDA = new PasswordResetRequestDA();
            DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Create);

            passwordResetRequestDA.Process(passwordResetRequest, command);
            operationResult.OperationResultStatus = OperationResultStatusEnum.Successful;

            return operationResult;
        }

        private void RefreshOperationResultStatus(OperationResult operationResult)
        {
            if (operationResult.HasError)
                operationResult.OperationResultStatus = OperationResultStatusEnum.Failed;
            else
                operationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
        }

        private String BuildPaswordResetUrl(String token)
        {
            Tuple<String, String> webDeploymentDetails = this.GetWebDeploymentDetails();
            String serverName = webDeploymentDetails.Item1;
            String virtualDirectoryName = webDeploymentDetails.Item2;

            return String.Format("http:\\\\{0}\\{1}\\Login\\ResetPassword.html?Token={2}", serverName, virtualDirectoryName, token);
        }

        private String BuildForgotPaswordUrl()
        {
            Tuple<String, String> webDeploymentDetails = this.GetWebDeploymentDetails();
            String serverName = webDeploymentDetails.Item1;
            String virtualDirectoryName = webDeploymentDetails.Item2;

            return String.Format("http:\\\\{0}\\{1}\\Login\\login.html?requestType=forgotPassword", serverName, virtualDirectoryName);
        }

        private Tuple<String, String> GetWebDeploymentDetails()
        {

            String serverConfig = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.Diagnostics.ServerConfiguration", String.Empty);

            String serverName = String.Empty;
            String virtualDirectoryName = String.Empty;

            XmlTextReader reader = null;

            try
            {
                reader = new XmlTextReader(serverConfig, XmlNodeType.Element, null);
                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "WebServer")
                    {
                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("ServerName"))
                            {
                                serverName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("VirtualDirectory"))
                            {
                                virtualDirectoryName = reader.ReadContentAsString();

                                if (virtualDirectoryName.EndsWith("_Forms"))
                                    break;
                            }
                        }
                        else
                        {
                            //Keep on reading the xml until we reach expected node.
                            reader.Read();
                        }
                    }
                    else
                    {
                        //Keep on reading the xml until we reach expected node.
                        reader.Read();
                    }
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return new Tuple<String, String>(serverName, virtualDirectoryName);
        }

        #endregion

        #endregion
    }
}
