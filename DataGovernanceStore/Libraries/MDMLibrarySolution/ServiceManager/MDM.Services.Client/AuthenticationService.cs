using System;
using System.ServiceModel;
using System.Diagnostics;

namespace MDM.Services
{
    using MDM.Utility;
    using MDM.Core.Exceptions;
    using MDM.BusinessObjects;
    using ASC = MDM.Services.AuthenticationServiceClient;
    using MDM.Interfaces;
    using MDM.Core;
    using MDM.BusinessObjects.Interfaces;

    /// <summary>
    /// Authentication Service facilitates to authenticate an user in MDMCenter and process the user credentials.
    /// </summary>
    public class AuthenticationService
    {
        #region Fields

        /// <summary>
        /// Field denoting Configuration Name of EndPoint that Client consumes
        /// </summary>
        private String _endPointConfigurationName = String.Empty;

        /// <summary>
        /// Field denoting the Address of EndPoint
        /// </summary>
        private EndpointAddress _endpointAddress = null;

        /// <summary>
        /// Field denoting the Current Username 
        /// </summary>
        private String _username = String.Empty;

        /// <summary>
        /// Field denoting the User's Password
        /// </summary>
        private String _password = String.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting the Current Username 
        /// </summary>
        public String UserName
        {
            get { return _username; }
            set { _username = value; }
        }

        /// <summary>
        /// Property denoting the User's Password
        /// </summary>
        public String Password
        {
            get { return _password; }
            set { _password = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default Constructor 
        /// </summary>
        public AuthenticationService()
            : this(string.Empty, null)
        {

        }

        /// <summary>
        ///  Constructor with  EndPoint Configuration Name
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        public AuthenticationService(String endPointConfigurationName)
            : this(endPointConfigurationName, null)
        {
        }

        /// <summary>
        /// Constructor with  EndPoint Configuration Name and End point address
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        /// <param name="endPointAddress">Indicates the End point address</param>
        public AuthenticationService(String endPointConfigurationName, EndpointAddress endPointAddress)
        {
            _endPointConfigurationName = endPointConfigurationName;
            _endpointAddress = endPointAddress;
        }

        #endregion

        #region AuthenticationService Methods


        /// <summary>
        ///  Constructs the SecurityPrincipal
        /// </summary>
        /// <param name="userName">username</param>
        /// <returns>returns SecurityPrincipal</returns>
        ///<exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        public ISecurityPrincipal GetSecurityPrincipal(string userName)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("AuthenticationServiceClient.GetSecurityPrincipal", false);

            ISecurityPrincipal securityPrincipal = null;
            ASC.AuthenticationServiceClient authenticationServiceClient = null;

            try
            {
                authenticationServiceClient = GetClient();

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("SecurityPrincipal requested for user: {0}", userName));
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Calling authenticationServiceClient.GetSecurityPrincipal...");
                }
                securityPrincipal = authenticationServiceClient.GetSecurityPrincipal(userName);
            }
            catch (FaultException<MDMExceptionDetails> ex)
            {
                MDMOperationException exception = null;

                if (ex.Detail != null)
                    exception = new MDMOperationException(ex.Detail.MessageCode, ex.Detail.Message, ex.Detail.Source, ex.Detail.StackTrace, ex.Detail.TargetSite, ex.Detail.MessageArguments);
                else
                    exception = new MDMOperationException(ex.Message);

                throw exception;
            }
            finally
            {
                //Close the client
                DisposeClient(authenticationServiceClient);

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("AuthenticationServiceClient.GetSecurityPrincipal");
            }

            return securityPrincipal;
        }

        ///<summary>
        /// Authenticats user using userLoginName and password (obsolete)
        ///</summary>
        ///<param name="userLoginName">Indicates the User Name</param>
        ///<param name="password">Indicates the Login password</param>
        ///<returns>If it is true means user got authenticated</returns>
        ///<exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        public Boolean AuthenticateUser(String userLoginName, String password)
        {
            AuthenticationData authenticationData = new AuthenticationData(userLoginName, password);
            return AuthenticateUser(authenticationData);
        }

        /// <summary>
        /// Authenticats user using authenticationData
        /// </summary>
        /// <param name="authenticationData"></param>
        /// <returns>If it is true means user got authenticated</returns>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        public Boolean AuthenticateUser(IAuthenticationData authenticationData)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("AuthenticationServiceClient.AuthenticateUser", false);

            Boolean isAuthenticated = false;
            ASC.AuthenticationServiceClient authenticationServiceClient = null;

            try
            {
                authenticationServiceClient = GetClient();

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Authentication requested with the following parameters: {0}", authenticationData));
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Calling authenticationServiceClient.AuthenticateUser...");
                }
                isAuthenticated = authenticationServiceClient.AuthenticateUser(authenticationData.UserName, authenticationData.Password);
            }
            catch (FaultException<MDMExceptionDetails> ex)
            {
                MDMOperationException exception = null;

                if (ex.Detail != null)
                    exception = new MDMOperationException(ex.Detail.MessageCode, ex.Detail.Message, ex.Detail.Source, ex.Detail.StackTrace, ex.Detail.TargetSite, ex.Detail.MessageArguments);
                else
                    exception = new MDMOperationException(ex.Message);

                throw exception;
            }
            finally
            {
                //Close the client
                DisposeClient(authenticationServiceClient);

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("AuthenticationServiceClient.AuthenticateUser");
            }

            return isAuthenticated;
        }

        /// <summary>
        /// Processes the user credential request by sending email to the registered email id based on the requested context
        /// </summary>
        /// <example>
        /// <code>
        /// // Create an instance of authentication service
        /// AuthenticationService getAuthenticationService = GetAuthenticationService();
        /// 
        /// // User Credential request to be requested to be processed.Indicates context for the user credential request
        /// // email address of the user
        /// // login id of the user
        /// // Request type needs to be passed
        /// IUserCredentialRequestContext userCredentialRequestContext =  <![CDATA[MDMObjectFactory.GetIUserCredentialRequestContext()]]>;
        /// userCredentialRequestContext.UserEmailId = "cfadmin@dm.com";
        /// userCredentialRequestContext.UserLoginId = "cfadmin";
        /// userCredentialRequestContext.RequestType = Core.UserCredentialRequestType.LoginId;
        /// 
        /// // Key Note: CallerContext has properties Application and Module which are mandatory to be set
        /// // Indicates name of application and module
        /// ICallerContext callerContext = <![CDATA[MDMObjectFactory.GetICallerContext()]]>;
        /// callerContext.Application = MDMCenterApplication.MDMCenter;
        /// callerContext.Module = MDMCenterModules.Entity;
        /// 
        /// // Below will make WCF call which will process the user credential request based on user credential and caller context
        /// IOperationResult processUserCredentialRequest = getAuthenticationService.ProcessUserCredentialRequest(userCredentialRequestContext, callerContext);
        /// </code>
        /// </example>
        /// <param name="iUserCredentialRequestContext">Indicates context for the user credential request</param>
        /// <param name="iCallerContext">Indicates name of application and module</param>
        /// <returns>Operation result containing detail information of process user credential request</returns>
        public IOperationResult ProcessUserCredentialRequest(IUserCredentialRequestContext iUserCredentialRequestContext, ICallerContext iCallerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("AuthenticationServiceClient.ProcessUserCredentialRequest", false);

            OperationResult operationResult = new OperationResult();
            ASC.AuthenticationServiceClient authenticationServiceClient = null;

            try
            {
                authenticationServiceClient = GetClient();

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Calling AuthenticationServiceClient.ProcessUserCredentialRequest...");
                }

                operationResult = authenticationServiceClient.ProcessUserCredentialRequest(iUserCredentialRequestContext as UserCredentialRequestContext, iCallerContext as CallerContext); ;
            }
            catch (FaultException<MDMExceptionDetails> ex)
            {
                MDMOperationException exception = null;

                if (ex.Detail != null)
                    exception = new MDMOperationException(ex.Detail.MessageCode, ex.Detail.Message, ex.Detail.Source, ex.Detail.StackTrace, ex.Detail.TargetSite, ex.Detail.MessageArguments);
                else
                    exception = new MDMOperationException(ex.Message);

                throw exception;
            }
            finally
            {
                //Close the client
                DisposeClient(authenticationServiceClient);

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("AuthenticationServiceClient.ProcessUserCredentialRequest");
            }

            return operationResult;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Get Authentication Service Client
        /// </summary>
        /// <returns>ServiceClient with UerName and Password</returns>
        private ASC.AuthenticationServiceClient GetClient()
        {
            ASC.AuthenticationServiceClient _client = null;

            if (String.IsNullOrEmpty(_endPointConfigurationName) && _endpointAddress == null)
                _client = new ASC.AuthenticationServiceClient();
            else if (!String.IsNullOrEmpty(_endPointConfigurationName) && _endpointAddress == null)
                _client = new ASC.AuthenticationServiceClient(_endPointConfigurationName);
            else if (!String.IsNullOrEmpty(_endPointConfigurationName) && _endpointAddress != null)
                _client = new ASC.AuthenticationServiceClient(_endPointConfigurationName, _endpointAddress);

            if (!String.IsNullOrEmpty(UserName) && !String.IsNullOrEmpty(Password))
            {
                _client.ClientCredentials.UserName.UserName = UserName;
                _client.ClientCredentials.UserName.Password = Password;
                _client.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.None;
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Client context for this request: EndPointConfigurationName-{0}; UserName-{1};",
            this._endPointConfigurationName, this.UserName));

            return _client;
        }

        private void DisposeClient(ASC.AuthenticationServiceClient client)
        {
            if (client != null)
            {
                if (client.State == CommunicationState.Created || client.State == CommunicationState.Opened || client.State == CommunicationState.Opening)
                {
                    client.Close();
                }
                else if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
            }
        }

        #endregion
    }
}
