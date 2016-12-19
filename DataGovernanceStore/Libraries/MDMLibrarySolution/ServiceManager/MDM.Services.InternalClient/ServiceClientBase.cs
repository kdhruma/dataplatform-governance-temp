using System;
using System.Linq;
using System.ServiceModel;
using System.Security.Principal;
using System.Security.Claims;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace MDM.Services
{
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.BusinessObjects;
    using MDM.Utility;
    using MDM.Interfaces;
    using MDM.CacheManager.Business;
    using MDM.WCFServiceInterfaces;
    using MDM.Services.InternalServiceProxies;

    /// <summary>
    /// Base Class for All Service Client
    /// </summary>
    public class ServiceClientBase
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
        /// Field denoting authentication type
        /// </summary>
        private AuthenticationType _authenticationType = AuthenticationType.Unknown;

        /// <summary>
        /// Field denoting user identity
        /// </summary>
        private IIdentity _userIdentity = null;

        /// <summary>
        /// Field denoting the Current Username 
        /// </summary>
        private String _userName = String.Empty;

        /// <summary>
        /// Field denoting the User's Password
        /// </summary>
        private String _password = String.Empty;

        /// <summary>
        /// Field denoting whether delegation is enabled or not
        /// </summary>
        private Boolean _isDelegationEnabled = true;

        #endregion

        #region Constructors

        /// <summary>
        /// Use this constructor while calling MDM service within the context of MDM Application. 
        /// Example: Service call from the MDM business rule.
        /// This would try to use current execution security context for service authentication purpose. 
        /// </summary>
        /// <param name="serviceType">Indicates Service Type</param>
        public ServiceClientBase(Type serviceType)
        {
            PopulateClientContext(serviceType, String.Empty);
        }

        /// <summary>
        /// Use this constructor while calling MDM service within the context of MDM Application. 
        /// Example: Service call from the MDM business rule.
        /// This would try to use current execution security context for service authentication purpose. 
        /// </summary>
        /// <param name="serviceType">Indicates Service Type</param>
        /// <param name="iSecurityPrincipal">Current security principal</param>
        public ServiceClientBase(Type serviceType, ISecurityPrincipal iSecurityPrincipal)
        {
            PopulateClientContext(serviceType, String.Empty, (SecurityPrincipal)iSecurityPrincipal);
        }

        /// <summary>
        /// Use this constructor while calling MDM service within the context of MDM Application. 
        /// Example: Service call from the MDM business rule.
        /// This would try to use current execution security context for service authentication purpose. 
        /// </summary>
        /// <param name="serviceType">Indicates Service Type</param>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        public ServiceClientBase(Type serviceType, String endPointConfigurationName)
        {
            PopulateClientContext(serviceType, endPointConfigurationName);
        }

        /// <summary>
        /// Use this constructor for form based authentication passing username and password
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        /// <param name="userName">Name of the Login User</param>
        /// <param name="password">Password of the User</param>
        public ServiceClientBase(String endPointConfigurationName, String userName, String password)
            : this(endPointConfigurationName, null, AuthenticationType.Forms, null, userName, password)
        {

        }

        /// <summary>
        /// Use this constructor for window authentication passing current windows identity.
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates EndPoint Configuration</param>
        /// <param name="userIdentity">Indicates User Identity</param>
        public ServiceClientBase(String endPointConfigurationName, IIdentity userIdentity)
            : this(endPointConfigurationName, null, AuthenticationType.Windows, userIdentity, String.Empty, String.Empty)
        {

        }

        /// <summary>
        /// Constructor with client configuration.
        /// </summary>
        /// <param name="wcfClientConfiguration">Indicates WCF Client Configuration</param>
        public ServiceClientBase(IWCFClientConfiguration wcfClientConfiguration)
        {
            this._endPointConfigurationName = wcfClientConfiguration.EndPointConfigurationName;
            this._endpointAddress = wcfClientConfiguration.EndpointAddress;
            this._authenticationType = wcfClientConfiguration.AuthenticationType;
            this._userIdentity = wcfClientConfiguration.UserIdentity;
            this._userName = wcfClientConfiguration.UserName;
            this._password = wcfClientConfiguration.Password;
        }

        /// <summary>
        /// Constructor with endPointConfigurationName, authenticationType, userIdentity, userName and password
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates EndPoint Configuration Name</param>
        /// <param name="authenticationType">Indicates Type of Authentication</param>
        /// <param name="userIdentity">Indicates User Identity</param>
        /// <param name="userName">Indicates User Name</param>
        /// <param name="password">Indicates Password</param>
        public ServiceClientBase(String endPointConfigurationName, AuthenticationType authenticationType, IIdentity userIdentity, String userName, String password)
            : this(endPointConfigurationName, null, authenticationType, userIdentity, userName, password)
        {
        }

        /// <summary>
        /// Constructor with endPointConfigurationName, endPointAddress, authenticationType, userIdentity, userName and password
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates EndPoint Configuration Name</param>
        /// <param name="endPointAddress">Indicates EndPoint Address</param>
        /// <param name="authenticationType">Indicates User Identity</param>
        /// <param name="userIdentity">Indicates User Identity</param>
        /// <param name="userName">Indicates User Name</param>
        /// <param name="password">Indicates Password</param>
        public ServiceClientBase(String endPointConfigurationName, EndpointAddress endPointAddress, AuthenticationType authenticationType, IIdentity userIdentity, String userName, String password)
        {
            this._endPointConfigurationName = endPointConfigurationName;
            this._endpointAddress = endPointAddress;
            this._authenticationType = authenticationType;
            this._userIdentity = userIdentity;
            this._userName = userName;
            this._password = password;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting EndPoint Configuration Name
        /// </summary>
        public String EndPointConfigurationName
        {
            get { return _endPointConfigurationName; }
            set { _endPointConfigurationName = value; }
        }

        /// <summary>
        /// Property denoting EndPoint Address
        /// </summary>
        public EndpointAddress EndpointAddress
        {
            get { return _endpointAddress; }
            set { _endpointAddress = value; }
        }

        /// <summary>
        /// Property denoting authentication type
        /// </summary>
        public AuthenticationType AuthenticationType
        {
            get { return _authenticationType; }
            set { _authenticationType = value; }
        }

        /// <summary>
        /// Property denoting user userIdentity
        /// </summary>
        public IIdentity UserIdentity
        {
            get { return _userIdentity; }
            set { _userIdentity = value; }
        }

        /// <summary>
        /// Property denoting the Current Username 
        /// </summary>
        public String UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        /// <summary>
        /// Property denoting the User's Password
        /// </summary>
        public String Password
        {
            get { return _password; }
            set { _password = value; }
        }

        /// <summary>
        /// Property denoting whether delegation is enabled or not
        /// </summary>
        public Boolean IsDelegationEnabled
        {
            get { return _isDelegationEnabled; }
            set { _isDelegationEnabled = value; }
        }

        #endregion

        #region Methods

        #region Public and Protected Methods

        /// <summary>
        /// Validate the Client Context
        /// </summary>
        /// <param name="mdmTraceSource">Source for MDM Traces</param>
        /// <returns>True if Context is Valid</returns>
        protected Boolean ValidateContext(MDMTraceSource mdmTraceSource = MDMTraceSource.APIFramework)
        {
            Object[] errorParams = null;
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "Starting ValidateContext method...", mdmTraceSource);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "AuthenticationType: " + this.AuthenticationType, mdmTraceSource);
            }
            try
            {
                if (this.AuthenticationType != Core.AuthenticationType.Windows && this.AuthenticationType != Core.AuthenticationType.Forms)
                {
                    errorParams = new Object[] { this.AuthenticationType.ToString() };
                    throw new MDMOperationException("111780", "111780", "Services", String.Empty, "ValidateContext", errorParams); //Provided authentication type '{0}' is not supported.
                }

                if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    if (String.IsNullOrWhiteSpace(this.UserName))
                        throw new MDMOperationException("111781", "111781", "Services", String.Empty, "ValidateContext"); //UserName is null or empty.

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "UserName: " + this.UserName, mdmTraceSource);

                    if (String.IsNullOrWhiteSpace(this.Password))
                        throw new MDMOperationException("111782", "111782", "Services", String.Empty, "ValidateContext"); //Password is null or empty.
                }

                if (this.AuthenticationType == Core.AuthenticationType.Windows)
                {
                    if (this.UserIdentity == null)
                        throw new MDMOperationException("111783", "111783", "Services", String.Empty, "ValidateContext");//UserIdentity is null.

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "Identity.Name: " + this.UserIdentity.Name, mdmTraceSource);

                    if (!(this.UserIdentity is WindowsIdentity))
                    {
                        errorParams = new String[] { this.UserIdentity.GetType().FullName };
                        throw new MDMOperationException("111784", "111784", "Services", String.Empty, "ValidateContext", errorParams);//Provided identity type '{0}' is not supported
                    }
                }
            }
            catch (Exception ex)
            {
                StackTrace stackTrace = new StackTrace(false);
                String callStack = stackTrace.ToString();
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("MDM API service client initialization failed with exception: {0}. UserName is:{1} and AuthType is:{2}. Operation callstack is: {3}", ex.Message, this.UserName, this.AuthenticationType.ToString(), callStack), mdmTraceSource);
                throw ex;
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "Done with ValidateContext", mdmTraceSource);
            }

            return true;
        }

        /// <summary>
        /// Handles exceptions 
        /// </summary>
        /// <param name="ex">Exception to be handled</param>
        /// <param name="mdmTraceSource">Source for MDM Traces</param>
        protected void HandleException(Exception ex, MDMTraceSource mdmTraceSource = MDMTraceSource.APIFramework)
        {
            Object[] errorParams = null;
            String serviceName = this.GetType().Name;
            Type exceptionType = ex.GetType();

            MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Service execution failed with exception:{0}", ex.Message), mdmTraceSource);
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Error call stack is:{0}", ex.StackTrace), mdmTraceSource);

            if (exceptionType == typeof(EndpointNotFoundException))
            {
                errorParams = new Object[] { serviceName };
                throw new MDMOperationException("111776", "111776", "Services", String.Empty, "HandleException", errorParams); //Failed to find the [{0}] endpoint. Please check the service configuration.
            }
            else if (exceptionType == typeof(InvalidOperationException))
            {
                errorParams = new Object[] { serviceName };
                throw new MDMOperationException("111777", "111777", "Services", String.Empty, "HandleException", errorParams);//[{0}] is not configured properly. Please check the service configuration.
            }
            else if (exceptionType == typeof(TimeoutException))
            {
                throw new MDMOperationException("111778", "111778", "Services", String.Empty, "HandleException");//The service operation timed out.
            }
            else if (exceptionType == typeof(CommunicationException))
            {
                throw new MDMOperationException("111779", "111779", "Services", String.Empty, "HandleException");//There was a problem in communication.
            }
            else if (ex.GetType() == typeof(FaultException<MDMExceptionDetails>))
            {
                FaultException<MDMExceptionDetails> faultException = (FaultException<MDMExceptionDetails>)ex;

                MDMOperationException exception = null;

                if (faultException != null && faultException.Detail != null)
                    exception = new MDMOperationException(faultException.Detail.MessageCode, faultException.Detail.Message, faultException.Detail.Source, faultException.Detail.StackTrace, faultException.Detail.TargetSite, faultException.Detail.MessageArguments);
                else
                    exception = new MDMOperationException(faultException.Message);

                throw exception;
            }
            else
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get the Client Context by Service Name and endPoint Configuration Name
        /// </summary>
        /// <param name="serviceType">Indicates Service Type</param>
        /// <param name="endPointConfigurationName">Indicates EndPoint Configuration Name</param>
        /// <param name="securityPrincipal">Represents security principal</param>
        protected void PopulateClientContext(Type serviceType, String endPointConfigurationName, SecurityPrincipal securityPrincipal = null)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("ServiceClientBase.PopulateClientContext", MDMTraceSource.APIFramework, false);

            try
            {
                String serviceName = serviceType.Name;

                if (securityPrincipal == null)
                {
                    securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();

                    if (securityPrincipal == null)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "Security Principal is not found or system is not configured properly. Please check authentication settings.", MDMTraceSource.APIFramework);
                        throw new MDMOperationException("111875", "111875", "Services", String.Empty, "PopulateClientContext"); //Security Principal is not found or system is not configured properly. Please check authentication settings.
                    }
                }

                String strIsDelegationEnabled = this.GetAppConfig("MDMCenter.Authentication.Windows.DelegationEnabled") ?? "false";

                if (!String.IsNullOrWhiteSpace(strIsDelegationEnabled))
                {
                    this.IsDelegationEnabled = ValueTypeHelper.BooleanTryParse(strIsDelegationEnabled, true);
                }

                //Note: SecurityPrincipalHelper.GetCurrentSecurityPrincipal method does all null checks 
                //and validate security principal object, thus no need to check for nulls
                this._authenticationType = securityPrincipal.CurrentAuthenticationType;

                if (this._authenticationType == AuthenticationType.Windows)
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Current authentication type is 'Windows'. Getting WindowsIdentity and Checking delegation enabled or not..", MDMTraceSource.APIFramework);

                    WindowsIdentity windowsIdentity = securityPrincipal.WindowsIdentity;
                    if (windowsIdentity == null)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "WindowsIdentity is not found or system is not configured properly. Please check authentication settings.", MDMTraceSource.APIFramework);
                        throw new MDMOperationException("111872", "111872", "Services", String.Empty, "PopulateClientContext"); //WindowsIdentity is not found or system is not configured properly. Please check authentication settings.
                    }

                    //If we are in local WCF instance then no need to run service call with Windows identity. 
                    //It would unnecessary impersonate windows identity and would make all the database calls using requested user's account instead of WCF service user account
                    Boolean isInLocalWCFInstance = MDM.Utility.WCFServiceInstanceLoader.IsLocalInstancesEnabled();

                    if (this.IsDelegationEnabled && (!isInLocalWCFInstance))
                    {
                        if (Constants.TRACING_ENABLED)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Delegation is enabled. Populating context with NetTcpBinding.", MDMTraceSource.APIFramework);
                        this._endPointConfigurationName = String.IsNullOrWhiteSpace(endPointConfigurationName) ? String.Format("{0}_I{1}", WCFBindingType.NetTcpBinding.ToString(), serviceName) : endPointConfigurationName;
                        this._userIdentity = new WindowsIdentity(windowsIdentity.Token);
                    }
                    else
                    {
                        //This is hybrid authentication as client is in Windows Auth but Delegation is off 
                        //Here, we need to pass common password using WSHttpBinding...
                        //Switch authentication for service to form
                        if (Constants.TRACING_ENABLED)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Delegation is disabled. Populating context with WSHttpBinding.", MDMTraceSource.APIFramework);
                        this._authenticationType = Core.AuthenticationType.Forms;
                        this._endPointConfigurationName = String.Format("{0}_I{1}", WCFBindingType.WSHttpBinding.ToString(), serviceName);
                        this._password = this.GetAppConfig("MDM.WCFServices.MessageSecurity.IdentityKey") ?? "super user with special powers - 1234567890-=+_)(*&^%$#@!"; //Get default message security password
                        this._userName = securityPrincipal.CurrentUserName;
                    }
                }
                else if (this._authenticationType == AuthenticationType.Claims)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Current authentication type is 'Claims'. Getting ClaimsIdentity ..", MDMTraceSource.APIFramework);

                    ClaimsIdentity claimsIdentity = securityPrincipal.ClaimsIdentity;
                    if (claimsIdentity == null)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "ClaimsIdentity is not found or system is not configured properly. Please check authentication settings.", MDMTraceSource.APIFramework);
                        throw new MDMOperationException("112490", "112490", "Services", String.Empty, "PopulateClientContext"); //ClaimsIdentity is not found or system is not configured properly. Please check authentication settings.
                    }

                    //Here, we need to pass common password using WSHttpBinding...
                    //Switch authentication for service to form
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Populating context with WSHttpBinding.", MDMTraceSource.APIFramework);
                    this._authenticationType = Core.AuthenticationType.Forms;
                    this._endPointConfigurationName = String.Format("{0}_I{1}", WCFBindingType.WSHttpBinding.ToString(), serviceName);
                    this._password = this.GetAppConfig("MDM.WCFServices.MessageSecurity.IdentityKey") ?? "super user with special powers - 1234567890-=+_)(*&^%$#@!"; //Get default message security password
                    this._userName = claimsIdentity.Name;
                }
                else if (this._authenticationType == AuthenticationType.Forms)
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Current authentication type is 'Forms'. Populating context with WSHttpBinding.", MDMTraceSource.APIFramework);

                    UserIdentity userIdentity = securityPrincipal.UserIdentity;
                    if (userIdentity == null)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "UserIdentity is not found or system is not configured properly. Please check authentication settings.", MDMTraceSource.APIFramework);
                        throw new MDMOperationException("111876", "111876", "Services", String.Empty, "PopulateClientContext"); //UserIdentity is not found or system is not configured properly. Please check authentication settings.
                    }

                    this._endPointConfigurationName = String.IsNullOrWhiteSpace(endPointConfigurationName) ? String.Format("{0}_I{1}", WCFBindingType.WSHttpBinding.ToString(), serviceName) : endPointConfigurationName;
                    this._userName = userIdentity.Name;
                    this._password = userIdentity.FormsAuthenticationTicket;

                    if (String.IsNullOrWhiteSpace(userIdentity.FormsAuthenticationTicket))
                    {
                        StackTrace stackTrace = new StackTrace(false);
                        String callStack = stackTrace.ToString();
                        if (Constants.TRACING_ENABLED)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Warning, String.Format("Form authentication ticket is null or empty. UserName is:{0} and AuthType is: {1}. Operation callstack is: {2}. System would try to fetch security detail from upper stack and continue.", userIdentity.Name, this.AuthenticationType.ToString(), callStack), MDMTraceSource.APIFramework);
                        this._password = this.GetAppConfig("MDM.WCFServices.MessageSecurity.IdentityKey") ?? "super user with special powers - 1234567890-=+_)(*&^%$#@!";
                    }
                }
                else
                {
                    Object[] errorParams = new Object[] { this.AuthenticationType.ToString() };
                    throw new MDMOperationException("111780", "111780", "Services", String.Empty, "PopulateClientContext", errorParams);  //Provided authentication type '{0}' is not supported.
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ServiceClientBase.PopulateClientContext", MDMTraceSource.APIFramework);
            }
        }

        /// <summary>
        /// dispose client
        /// </summary>
        /// <typeparam name="T">Client Type</typeparam>
        /// <param name="client">Indicates Client to be disposed</param>
        protected void DisposeClient<T>(ClientBase<T> client) where T : class
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

        
        /// <summary>
        /// Gets app config value for the requested key
        /// </summary>
        /// <param name="appConfigKey">Indicates app config key name.</param>
        /// <returns>Value of the requested app config key</returns>
        protected String GetAppConfig(String appConfigKey)
        {
            ICache cache = null;
            String appConfigValue = String.Empty;

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Getting AppConfig value for key '{0}'..", appConfigKey), MDMTraceSource.APIFramework);

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Checking AppConfig into cache..", MDMTraceSource.APIFramework);

                cache = CacheFactory.GetCache();

                //Prepare cache key
                String cacheKey = String.Format("RS_AppConfig:[{0}]", appConfigKey);

                //Try to get configuration value from cache
                Object cachedAppConfigValue = cache[cacheKey];

                if (cachedAppConfigValue != null)
                {
                    appConfigValue = cachedAppConfigValue.ToString();
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "AppConfig is available in cache.", MDMTraceSource.APIFramework);
                }
                else
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "AppConfig is not available in cache. Need to populate appconfig in cache", MDMTraceSource.APIFramework);
                }
            }
            catch (Exception ex)
            {
                String errorMessage = String.Format("Unable to retrieve AppConfig. Key: {0}. Error: {1}", appConfigKey, ex.Message);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, errorMessage, MDMTraceSource.APIFramework);

                Object[] messageParams = new Object[] { appConfigKey };
                throw new MDMOperationException("111871", "111871", "Services", String.Empty, "GetAppConfig", messageParams); //Unable to retrieve AppConfig. Key: {0}
            }

            return appConfigValue;
        }

        /// <summary>
        /// Impersonates the specified action under current user identity 
        /// in case current AuthenticationType is AuthenticationType.Windows.
        /// </summary>
        /// <param name="action">The action to perform in authentication context.</param>
        protected T Impersonate<T>(Func<T> action)
        {
            T result = default(T);
            if (this.AuthenticationType == Core.AuthenticationType.Windows)
            {
                WindowsIdentity windowsIdentity = (WindowsIdentity)this.UserIdentity;

                using (WindowsImpersonationContext WindowsImpersonationContext = windowsIdentity.Impersonate())
                {
                    result = action();
                }
            }
            else if (this.AuthenticationType == Core.AuthenticationType.Forms)
            {
                result = action();
            }
            return result;
        }

        #endregion

        #region Private Methods
        #endregion

        #endregion
    }
}
