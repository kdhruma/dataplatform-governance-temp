using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
namespace MDM.SecurityManager.Business
{
    using MDM.AdminManager.Business;
    using MDM.BusinessObjects;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.MessageManager.Business;
    using MDM.SecurityManager.Data.SqlClient;
    using MDM.Utility;

    /// <summary>
    /// Contains business logic for the password reset request
    /// </summary>
    public class PasswordResetRequestBL : BusinessLogicBase
    {
        #region Fields

        private readonly SecurityPrincipal _securityPrincipal;

        private const String _passwordResetForGivenToken = "112973"; // Password already reset
        private const String _invalidToken = "112974"; //Token is invalid for password reset
        private const String _expiredToken = "112975";  //Token has expired 

        #endregion

        #region Constructors

        /// <summary>
        /// Business Logic class constructor
        /// </summary>
        public PasswordResetRequestBL()
        {
            _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Specifies whether the password request is valid or not.
        /// </summary>
        /// <param name="passwordRequestId">Password request id to be validated</param>
        /// <param name="callerContext">Indicates name of application and module</param>
        /// <returns>Operation result containing boolean return value as indication of validity for the password request id</returns>
        public OperationResult IsPasswordResetRequestValid(String passwordRequestId, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Starting validation of Password Reset Request...", MDMTraceSource.SecurityService);

            OperationResult operationResult = new OperationResult();
            DurationHelper durationHelper = new DurationHelper(DateTime.Now);
            LocaleMessageBL localeManager = new LocaleMessageBL();

            try
            {
                PasswordResetRequestDA passwordResetRequestDA = new PasswordResetRequestDA();
                DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Read);
                PasswordResetRequest passwordResetRequest = passwordResetRequestDA.GetByToken(passwordRequestId, command);
                Boolean isValidRequest = false;

                if (passwordResetRequest != null)
                {
                    if (passwordResetRequest.IsPasswordReset)
                    {
                        String errorMsg = localeManager.Get(GlobalizationHelper.GetSystemUILocale(), _passwordResetForGivenToken, false, new CallerContext { Application = MDMCenterApplication.MDMCenter, Module = MDMCenterModules.Security }).Message;
                        operationResult.AddOperationResult("", errorMsg, OperationResultType.Error);
                        operationResult.OperationResultStatus = OperationResultStatusEnum.Failed;
                    }
                    else
                    {
                        // Validate request against validity period
                        DateTime currentTime = DateTime.Now;
                        TimeSpan difference = currentTime.Subtract(passwordResetRequest.RequestedDateTime);

                        Int32 validityPeriod = AppConfigurationHelper.GetAppConfig<Int32>("MDMCenter.SecurityManager.ResetPasswordToken.ValidityPeriod", 24);
                        if (difference.Hours < validityPeriod)
                        {
                            isValidRequest = true;
                            operationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                        }
                        else
                        {
                            String errorMsg = localeManager.Get(GlobalizationHelper.GetSystemUILocale(), _expiredToken, false, new CallerContext { Application = MDMCenterApplication.MDMCenter, Module = MDMCenterModules.Security }).Message;
                            operationResult.AddOperationResult("", errorMsg, OperationResultType.Error);
                            operationResult.OperationResultStatus = OperationResultStatusEnum.Failed;
                        }

                        operationResult.ReturnValues.Add(isValidRequest);
                    }
                }
                else
                {
                    String errorMsg = localeManager.Get(GlobalizationHelper.GetSystemUILocale(), _invalidToken, false, new CallerContext { Application = MDMCenterApplication.MDMCenter, Module = MDMCenterModules.Security }).Message;
                    operationResult.AddOperationResult("", errorMsg, OperationResultType.Error);
                    operationResult.OperationResultStatus = OperationResultStatusEnum.Failed;
                }
            }
            finally
            {
                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Overall Password Reset Request validation time", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.SecurityService);
            }

            return operationResult;
        }

        /// <summary>
        /// Update/Reset password for a password reset id
        /// </summary>
        /// <param name="passwordRequestId">Password request id</param>
        /// <param name="password">Hashed new password</param>
        /// <param name="callerContext">Indicates name of application and module</param>
        /// <returns>Operation result containing detail information of update password</returns>
        public OperationResult UpdatePassword(String passwordRequestId, String password, CallerContext callerContext)
        {
            OperationResult operationResult = new OperationResult();

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Updating Password...", MDMTraceSource.SecurityService);
            DurationHelper durationHelper = new DurationHelper(DateTime.Now);

            try
            {
                PasswordResetRequestDA passwordResetRequestDA = new PasswordResetRequestDA();
                DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Read);

                passwordResetRequestDA.PasswordReset(passwordRequestId, password, command);

                operationResult.ReturnValues.Add(true);
                operationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
            }
            finally
            {
                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Overall Update Password time", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.SecurityService);
            }

            return operationResult;
        }

        #endregion
    }
}
