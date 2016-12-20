using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Security.Principal;
using System.ServiceModel;

namespace MDM.Services
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.Interfaces;
    using MDM.Services.ServiceProxies;
    using MDM.Utility;
    using MDM.WCFServiceInterfaces;

    /// <summary>
    /// KnowledgeBase Service facilitates to get available locales in MDMCenter.
    /// </summary>
    public class KnowledgeBaseService : ServiceClientBase
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="KnowledgeBaseService"/> class. 
        /// Use this default constructor only when calling MDM service within the execution context of an MDM Application. 
        /// Example: Service call from the MDM business rule.
        /// This would try to use current execution security context for service authentication purpose. 
        /// </summary>
        public KnowledgeBaseService()
            : base(typeof(KnowledgeBaseService))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KnowledgeBaseService"/> class. 
        /// Use this default constructor with binding configuration name only when calling MDM service within the execution context of an MDM Application. 
        /// Example: Service call from the MDM business rule.
        /// This would try to use current execution security context for service authentication purpose. 
        /// </summary>
        /// <param name="endPointConfigurationName">
        /// Indicates the EndPointConfigurationName
        /// </param>
        public KnowledgeBaseService(String endPointConfigurationName)
            : base(typeof(KnowledgeBaseService), endPointConfigurationName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KnowledgeBaseService"/> class. 
        /// Use this constructor for form based authentication passing user name and password
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        /// <param name="userName">Name of the Login User</param>
        /// <param name="password">Password of the User</param>
        public KnowledgeBaseService(String endPointConfigurationName, String userName, String password)
            : base(endPointConfigurationName, userName, password)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KnowledgeBaseService"/> class. 
        /// Use this constructor for window authentication passing current windows identity.
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        /// <param name="userIdentity">Indicates User's Identity</param>
        public KnowledgeBaseService(String endPointConfigurationName, IIdentity userIdentity)
            : base(endPointConfigurationName, userIdentity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KnowledgeBaseService"/> class. 
        /// Constructor with client configuration.
        /// </summary>
        /// <param name="wcfClientConfiguration">Indicates WCF Client Configuration</param>
        public KnowledgeBaseService(IWCFClientConfiguration wcfClientConfiguration)
            : base(wcfClientConfiguration)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KnowledgeBaseService"/> class. 
        /// Constructor with endPointConfigurationName, authenticationType, userIdentity, userName and password
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        /// <param name="authenticationType">Indicates Type of Authentication i.e Windows/Forms</param>
        /// <param name="userIdentity">Indicates Identity of the Login User</param>
        /// <param name="userName">Name of the Login User</param>
        /// <param name="password">Password of the User</param>
        public KnowledgeBaseService(String endPointConfigurationName, AuthenticationType authenticationType, IIdentity userIdentity, String userName, String password)
            : base(endPointConfigurationName, authenticationType, userIdentity, userName, password)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KnowledgeBaseService"/> class. 
        /// Constructor with endPointConfigurationName, endPointAddress, authenticationType, userIdentity, userName and password
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        /// <param name="endPointAddress">Provides a unique network address that a client uses to communicate with a service endpoint.</param>
        /// <param name="authenticationType">Indicates Type of Authentication i.e Windows/Forms</param>
        /// <param name="userIdentity">Indicates Identity of the Login User</param>
        /// <param name="userName">Name of the Login User</param>
        /// <param name="password">Password of the User</param>
        public KnowledgeBaseService(String endPointConfigurationName, EndpointAddress endPointAddress, AuthenticationType authenticationType, IIdentity userIdentity, String userName, String password)
            : base(endPointConfigurationName, endPointAddress, authenticationType, userIdentity, userName, password)
        {
        }

        #endregion

        #region Locale Methods

        /// <summary>
        /// Get Locale based on Locale ID
        /// </summary>
        /// <param name="localeId">Indicates Id of Locale</param>
        /// <returns>Locale Interface Object</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public ILocale GetLocale(Int32 localeId)
        {
            return MakeServiceCall("GetLocale",
                                   "GetLocale",
                                   service =>
                                       {
                                           if (Constants.TRACING_ENABLED)
                                           {
                                               MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested Get Locale for LocaleId: {0}", localeId));
                                           }
                                           return service.GetLocale(localeId);
                                       });
        }

        /// <summary>
        /// Get all Available Locales
        /// </summary>
        /// <returns>Array of locale objects</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public ILocaleCollection GetAvailableLocales()
        {
            return MakeServiceCall("GetAvailableLocales",
                                   "GetAvailableLocales",
                                   service => service.GetAvailableLocales());
        }

        /// <summary>
        /// Get all Locales
        /// </summary>
        /// <returns>Array of locale objects</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public ILocaleCollection GetAllLocales()
        {
            return MakeServiceCall("GetAllLocales",
                                   "GetAllLocales",
                                   service => service.GetAllLocales());
        }
        

        /// <summary>
        /// Get all locales by container id.
        /// </summary>
        /// <example>
        /// <code>
        /// //Create an instance of KnowledgeBase service
        /// KnowledgeBaseService mdmKnowledgeService = GetMDMKnowledgeService();
        /// 
        /// //River Works Product Master as per River Works Data Model
        /// Int32 productMasterId = 5;
        /// 
        /// //Below will make a WCF call to get all locales by container id.
        /// ILocaleCollection getByContainerCollection = mdmKnowledgeService.GetLocalesByContainer(containerId);
        /// </code>
        /// </example>
        /// <param name="containerId">This parameter is specifying container id.</param>
        /// <returns>Collection of locales</returns>
        public ILocaleCollection GetLocalesByContainer(Int32 containerId)
        {
            return MakeServiceCall("GetLocalesByContainer",
                                   "GetLocalesByContainer",
                                   service => service.GetLocalesByContainer(containerId));
        }

        #endregion Locale Methods

        #region Lookup

        /// <summary>
        /// Gets all the lookup table names from system or lookup table names having at least one unique column 
        /// </summary>
        /// <example>
        /// <code>
        /// // Creates an instance of Knowledge base service
        /// KnowledgeBaseService service = new KnowledgeBaseService();
        /// 
        /// // Note: Setting Application and Module properties of the CallerContext is mandatory
        /// ICallerContext callerContext = <![CDATA[MDMObjectFactory.GetICallerContext()]]>;
        /// callerContext.Application = MDMCenterApplication.MDMCenter;
        /// callerContext.Module = MDMCenterModules.Lookup;
        ///
        /// ILookupContext lookupContext =  <![CDATA[MDMObjectFactory.GetILookupContext()]]>;
        /// lookupContext.TableFilterType = LookupTableFilterType.All;
        /// // Note: The other possible option is to get lookup table names having at least one unique column i.e. LookupTableFilterType.LookupWithUniqueColumn
        /// 
        /// // Makes WCF call to get all the lookup table names from the system
        /// var tableNames = service.GetAllLookupTableNames(callerContext, lookupContext);
        /// </code>
        /// </example>
        /// <param name="callerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="lookupContext">Indicates the lookup filtering context</param>
        /// <returns>Returns the collection of lookup table names</returns>
        public Collection<String> GetAllLookupTableNames(ICallerContext callerContext, ILookupContext lookupContext = null)
        {
            return MakeServiceCall("GetAllLookupTableNames", "GetAllLookupTableNames",
               client => client.GetAllLookupTableNames(FillDiagnosticTraces(callerContext), lookupContext as LookupContext));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Get KnowledgeBaseService Service Client
        /// </summary>
        /// <returns>ServiceClient with UerName and Password</returns>
        private IKnowledgeBaseService GetClient()
        {
            IKnowledgeBaseService serviceClient = null;

            if (WCFServiceInstanceLoader.IsLocalInstancesEnabled())
            {
                serviceClient = WCFServiceInstanceLoader.GetServiceInstance<IKnowledgeBaseService>();
            }

            if (serviceClient == null) //Means the given type is not implemented for local load..
            {
                KnowledgeBaseServiceProxy iKnowledgeBaseServiceProxy = null;

                if (String.IsNullOrEmpty(this.EndPointConfigurationName) && this.EndpointAddress == null)
                    iKnowledgeBaseServiceProxy = new KnowledgeBaseServiceProxy();
                else if (!String.IsNullOrEmpty(this.EndPointConfigurationName) && this.EndpointAddress == null)
                    iKnowledgeBaseServiceProxy = new KnowledgeBaseServiceProxy(this.EndPointConfigurationName);
                else if (!String.IsNullOrEmpty(this.EndPointConfigurationName) && this.EndpointAddress != null)
                    iKnowledgeBaseServiceProxy = new KnowledgeBaseServiceProxy(this.EndPointConfigurationName, this.EndpointAddress);

                if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    iKnowledgeBaseServiceProxy.ClientCredentials.UserName.UserName = this.UserName;
                    iKnowledgeBaseServiceProxy.ClientCredentials.UserName.Password = this.Password;
                    iKnowledgeBaseServiceProxy.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.None;
                }

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information,
                        String.Format(
                            "Client context for this request: IsDelegationEnabled-{0}; AuthenticationType-{1}; EndPointConfigurationName-{2}; UserName-{3}; UserIdentityName-{4}",
                            this.IsDelegationEnabled,
                            this.AuthenticationType,
                            this.EndPointConfigurationName,
                            this.UserName,
                            (this.UserIdentity != null) ? this.UserIdentity.Name : String.Empty));
                }

                serviceClient = iKnowledgeBaseServiceProxy;
            }

            return serviceClient;
        }

        /// <summary>
        /// Makes the KnowledgeBase Service call. Creates the client instance, executes call delegate against it in
        /// impersonated context (if necessary) and then disposes the client.
        /// Also emits traces when necessary.
        /// </summary>
        /// <typeparam name="TResult">The type of the result of service call.</typeparam>
        /// <param name="clientMethodName">Name of the client method to include in traces.</param>
        /// <param name="serverMethodName">Name of the server method to include in traces.</param>
        /// <param name="call">The call delegate to be executed against a client instance.</param>
        /// <param name="traceSource">Indicates the MDMTrace Source. By default trace source is General.</param>
        /// <returns>The value returned by service, or default.</returns>
        private TResult MakeServiceCall<TResult>(String clientMethodName, String serverMethodName, Func<IKnowledgeBaseService, TResult> call, MDMTraceSource traceSource = MDMTraceSource.General)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("IKnowledgeBaseService." + clientMethodName, traceSource, false);
            }

            TResult result = default(TResult);
            IKnowledgeBaseService iKnowledgeBaseServiceService = null;

            try
            {
                iKnowledgeBaseServiceService = GetClient();

                ValidateContext();

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "IKnowledgeBaseServiceClient sends '" + serverMethodName + "' request message.", traceSource);
                }

                result = Impersonate(() => call(iKnowledgeBaseServiceService));

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "IKnowledgeBaseServiceClient receives '" + serverMethodName + "' response message.", traceSource);
                }
            }
            catch (Exception ex) //Here we will catch all types of exceptions and then real handling happens in the base class method 'HandleException'
            {
                HandleException(ex);
            }
            finally
            {
                //Close the client
                DisposeClient(iKnowledgeBaseServiceService);

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("IKnowledgeBaseServiceClient." + clientMethodName, traceSource);
                }
            }

            return result;
        }

        private void DisposeClient(IKnowledgeBaseService client)
        {
            if (client == null)
                return;

            if (client.GetType().Equals(typeof(KnowledgeBaseServiceProxy)))
            {
                KnowledgeBaseServiceProxy serviceClient = (KnowledgeBaseServiceProxy)client;
                if (serviceClient.State == CommunicationState.Created || serviceClient.State == CommunicationState.Opened || serviceClient.State == CommunicationState.Opening)
                {
                    serviceClient.Close();
                }
                else if (serviceClient.State == CommunicationState.Faulted)
                {
                    serviceClient.Abort();
                }
            }
            else
            {
                //Do nothing...
            }
        }

        #endregion
    }
}