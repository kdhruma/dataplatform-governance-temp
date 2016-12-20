using System;
using System.Security.Principal;
using System.ServiceModel;

namespace MDM.Services
{
    using BusinessObjects;
    using BusinessObjects.Diagnostics;
    using Core;
    using Interfaces;
    using ServiceProxies;
    using Utility;
    using WCFServiceInterfaces;
    using Interfaces.Diagnostics;
    using File = MDM.BusinessObjects.File;

    ///<summary>
    /// DiagnosticService provides external program an ability to work with MDM Diagnostic. 
    /// </summary>
    public class DiagnosticService : ServiceClientBase
    {
        #region Fields

        #endregion

        #region Constructors

        /// <summary>
        /// Use this default constructor only when calling MDM service within the execution context of an MDM Application. 
        /// Example: Service call from the MDM business rule.
        /// This would try to use current execution security context for service authentication purpose. 
        /// </summary>
        public DiagnosticService()
            : base(typeof(DiagnosticService))
        {
        }

        /// <summary>
        /// Use this default constructor with binding configuration name only when calling MDM service within the execution context of an MDM Application. 
        /// Example: Service call from the MDM business rule.
        /// This would try to use current execution security context for service authentication purpose. 
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        public DiagnosticService(String endPointConfigurationName)
            : base(typeof(DiagnosticService), endPointConfigurationName)
        {
        }

        /// <summary>
        /// Use this constructor for form based authentication passing username and password
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        /// <param name="userName">Name of the Login User</param>
        /// <param name="password">Password of the User</param>
        public DiagnosticService(String endPointConfigurationName, String userName, String password)
            : base(endPointConfigurationName, userName, password)
        {

        }

        /// <summary>
        /// Use this constructor for window authentication passing current windows identity.
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        /// <param name="userIdentity">Indicates User's Identity</param>
        public DiagnosticService(String endPointConfigurationName, IIdentity userIdentity)
            : base(endPointConfigurationName, userIdentity)
        {
        }

        /// <summary>
        /// Constructor with client configuration.
        /// </summary>
        /// <param name="wcfClientConfiguration">Indicates WCF Client Configuration</param>
        public DiagnosticService(IWCFClientConfiguration wcfClientConfiguration)
            : base(wcfClientConfiguration)
        {
        }

        /// <summary>
        /// Constructor with endPointConfigurationName, authenticationType, userIdentity, userName and password
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        /// <param name="authenticationType">Indicates Type of Authentication i.e Windows/Forms</param>
        /// <param name="userIdentity">Indicates Identity of the Login User</param>
        /// <param name="userName">Name of the Login User</param>
        /// <param name="password">Password of the User</param>
        public DiagnosticService(String endPointConfigurationName, AuthenticationType authenticationType, IIdentity userIdentity, String userName, String password)
            : base(endPointConfigurationName, authenticationType, userIdentity, userName, password)
        {
        }

        /// <summary>
        /// Constructor with endPointConfigurationName, endPointAddress, authenticationType, userIdentity, userName and password
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        /// <param name="endPointAddress"> Provides a unique network address that a client uses to communicate with a service endpoint.</param>
        /// <param name="authenticationType">Indicates Type of Authentication i.e Windows/Forms</param>
        /// <param name="userIdentity">Indicates Identity of the Login User</param>
        /// <param name="userName">Name of the Login User</param>
        /// <param name="password">Password of the User</param>
        public DiagnosticService(String endPointConfigurationName, EndpointAddress endPointAddress, AuthenticationType authenticationType, IIdentity userIdentity, String userName, String password)
            : base(endPointConfigurationName, endPointAddress, authenticationType, userIdentity, userName, password)
        {
        }

        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Retrieves the application related diagnostic information
        /// </summary>
        /// <param name="applicationDiagnosticType">Indicates the application diagnostic type</param>
        /// <param name="startDateTime">Indicates start date time to get information from datetime</param>
        /// <param name="entityId">Indicates the Id of an entity</param>
        /// <param name="count">Indicates how many rows to return</param>
        /// <param name="iCallerContext">Indicates the context of the caller</param>
        /// <returns>Returns JSON String</returns>
        public String GetApplicationDiagnostic(ApplicationDiagnosticType applicationDiagnosticType, DateTime startDateTime, Int64 entityId, Int64 count, ICallerContext iCallerContext)
        {
            return MakeServiceCall<String>("GetApplicationDiagnostic", "GetApplicationDiagnostic",
                                                  client =>
                                                      client.GetApplicationDiagnostic(applicationDiagnosticType, startDateTime, entityId, count, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Retrieves the system related diagnostic information
        /// </summary>
        /// <param name="systemDiagnosticType">Indicates the system diagnostic type</param>
        /// <param name="systemDiagnosticSubType">Indicates the system diagnostic sub type</param>
        /// <param name="count">Indicates how many rows to return</param>
        /// <param name="iCallerContext">Indicates the context of the caller</param>
        /// <returns>Returns JSON String</returns>
        public String GetSystemDiagnostic(SystemDiagnosticType systemDiagnosticType, SystemDiagnosticSubType systemDiagnosticSubType, Int64 count, ICallerContext iCallerContext)
        {
            return MakeServiceCall<String>("GetSystemDiagnostic", "GetSystemDiagnostic",
                                                  client =>
                                                      client.GetSystemDiagnostic(systemDiagnosticType, systemDiagnosticSubType, count, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Process DiagnosticActivity and Records collection
        /// </summary>
        /// <param name="diagnosticActivities"></param>
        /// <param name="diagnosticRecords"></param>
        /// <param name="iCallerContext"></param>
        /// <returns></returns>
        public Boolean ProcessDiagnosticData(DiagnosticActivityCollection diagnosticActivities, DiagnosticRecordCollection diagnosticRecords, ICallerContext iCallerContext)
        {
            // Do not wrap processing of Diagnostic Data to diagnostic activity

            ExecutionContext executionContext = new ExecutionContext();

            if (!executionContext.LegacyMDMTraceSources.Contains(MDMTraceSource.DiagnosticService))
            {
                executionContext.LegacyMDMTraceSources.Add(MDMTraceSource.DiagnosticService);
            }

            Boolean result = false;
            IDiagnosticService diagnosticService = null;

            try
            {
                diagnosticService = GetClient();

                ValidateContext(MDMTraceSource.DiagnosticService);

                result = Impersonate(() => diagnosticService.ProcessDiagnosticData(diagnosticActivities, diagnosticRecords, FillDiagnosticTraces(iCallerContext)));
            }
            catch (Exception ex) //Here we will catch all types of exceptions and then real handling happens in the base class method 'HandleException'
            {
                HandleException(ex);
            }
            finally
            {
                //Close the client
                DisposeClient(diagnosticService);
            }

            return result;
        }

        /// <summary>
        /// Get DiagnosticActivity collection
        /// </summary>
        /// <param name="iDiagnosticReportSettings">Indicates Diagnostic Report settings</param>
        /// <param name="iCallerContext">Indicates the context of the caller</param>
        /// <returns>DiagnosticActivityCollection</returns>
        public DiagnosticActivityCollection GetActivities(IDiagnosticReportSettings iDiagnosticReportSettings, ICallerContext iCallerContext)
        {
            return MakeServiceCall<DiagnosticActivityCollection>("GetActivities", "GetActivities",
                                      client =>
                                          client.GetActivities(iDiagnosticReportSettings as DiagnosticReportSettings, FillDiagnosticTraces(iCallerContext)), MDMTraceSource.DiagnosticService);
        }

        /// <summary>
        /// Get DiagnosticRecord collection
        /// </summary>
        /// <param name="operationId"></param>
        /// <param name="iCallerContext"></param>
        /// <returns>DiagnosticRecordCollection</returns>
        public DiagnosticRecordCollection GetRecords(Guid operationId, ICallerContext iCallerContext)
        {
            return MakeServiceCall<DiagnosticRecordCollection>("GetRecords", "GetRecords",
                                      client =>
                                          client.GetRecords(operationId, FillDiagnosticTraces(iCallerContext)), MDMTraceSource.DiagnosticService);
        }

        /// <summary>
        /// Retrieves the related diagnostic record data based on context.
        /// </summary>
        /// <param name="relativeDataReferanceId">Indicates relative data reference id for diagnostic record.</param>
        /// <param name="diagnosticRelativeDataType">Indicates relative data type for diagnostic record.</param>
        /// <param name="iCallerContext">Indicates the context of the caller</param>
        /// <returns>returns related diagnostic record data as string</returns>
        public String GetRelatedDiagnosticRecordData(Int64 relativeDataReferanceId, DiagnosticRelativeDataType diagnosticRelativeDataType, ICallerContext iCallerContext)
        {
            return MakeServiceCall<String>("GetRelatedDiagnosticRecordData", "GetRelatedDiagnosticRecordData",
                          client =>
                              client.GetRelatedDiagnosticRecordData(relativeDataReferanceId, diagnosticRelativeDataType, FillDiagnosticTraces(iCallerContext)), MDMTraceSource.DiagnosticService);
        }

        /// <summary>
        /// Starts the diagnostic traces.
        /// </summary>
        /// <param name="callerContext">Indicates the caller context which contains the MDM application and MDM module</param>
        /// <returns>Returns the operation result based on the result</returns>
        public IOperationResult StartDiagnosticTraces(ICallerContext callerContext)
        {
            return MakeServiceCall<OperationResult>("StartDiagnosticTraces", "StartDiagnosticTraces",
                                          client => client.StartDiagnosticTraces(FillDiagnosticTraces(callerContext)), MDMTraceSource.DiagnosticService);

        }

        /// <summary>
        /// Stop the diagnostic traces
        /// </summary>
        /// <param name="callerContext">Indicates the caller context which contains the MDM application and MDM module</param>
        /// <returns>Returns the operation result based on the result</returns>
        public IOperationResult StopDiagnosticTraces(ICallerContext callerContext)
        {
            return MakeServiceCall<OperationResult>("StopDiagnosticTraces", "StopDiagnosticTraces",
                                         client => client.StopDiagnosticTraces(FillDiagnosticTraces(callerContext)));
        }


        /// <summary>
        /// get excel report file based on report type  subtype
        /// </summary>
        /// <param name="reportType"></param>
        /// <param name="reportSubtype"></param>
        /// <param name="InputXml"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public DiagnosticToolsReportResultWrapper ProcessDiagnosticToolsReport(DiagnosticToolsReportType reportType, DiagnosticToolsReportSubType reportSubtype, String InputXml, ICallerContext callerContext)
        {
            return MakeServiceCall<DiagnosticToolsReportResultWrapper>("ProcessDiagnosticToolsReport", "ProcessDiagnosticToolsReport", client => client.ProcessDiagnosticToolsReport(reportType, reportSubtype, InputXml, FillDiagnosticTraces(callerContext)), MDMTraceSource.DiagnosticService);
        }


        /// <summary>
        /// get input template for diagnostic report
        /// </summary>
        /// <param name="reportType"></param>
        /// <param name="reportSubtype"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public String GetReportTemplate(DiagnosticToolsReportType reportType, DiagnosticToolsReportSubType reportSubtype, CallerContext callerContext)
        {
            return MakeServiceCall<String>("GetReportTemplate", "GetReportTemplate", client => client.GetReportTemplate(reportType, reportSubtype, FillDiagnosticTraces(callerContext)), MDMTraceSource.DiagnosticService);
        }

        #region CRUD operations on DiagnosticReportProfile

        /// <summary>
        /// 
        /// </summary>
        /// <param name="diagnosticReportProfile"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public IOperationResult CreateDiagnosticReportProfile(DiagnosticReportProfile diagnosticReportProfile, CallerContext callerContext)
        {
            return MakeServiceCall("CreateDiagnosticReportProfile", "CreateDiagnosticReportProfile", client => client.CreateDiagnosticReportProfile(diagnosticReportProfile, FillDiagnosticTraces(callerContext)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="diagnosticReportProfile"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public IOperationResult UpdateDiagnosticReportProfile(DiagnosticReportProfile diagnosticReportProfile, CallerContext callerContext)
        {
            return MakeServiceCall("UpdateDiagnosticReportProfile", "UpdateDiagnosticReportProfile", client => client.UpdateDiagnosticReportProfile(diagnosticReportProfile, FillDiagnosticTraces(callerContext)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profileName"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public DiagnosticReportProfile GetDiagnosticReportProfileByName(String profileName, CallerContext callerContext)
        {
            return MakeServiceCall("GetDiagnosticReportProfileByName", "GetDiagnosticReportProfileByName", client => client.GetDiagnosticReportProfileByName(profileName, FillDiagnosticTraces(callerContext)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="diagnosticReportProfile"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public IOperationResult DeleteDiagnosticReportProfile(DiagnosticReportProfile diagnosticReportProfile, CallerContext callerContext)
        {
            return MakeServiceCall("DeleteDiagnosticReportProfile", "DeleteDiagnosticReportProfile", client => client.DeleteDiagnosticReportProfile(diagnosticReportProfile, FillDiagnosticTraces(callerContext)));
        }

        #endregion CRUD operations on DiagnosticReportProfile

        #endregion

        #region Private Methods

        /// <summary>
        /// Get Diagnostic Service Client
        /// </summary>
        /// <returns>ServiceClient with UerName and Password</returns>
        private IDiagnosticService GetClient()
        {
            IDiagnosticService serviceClient = null;

            if (WCFServiceInstanceLoader.IsLocalInstancesEnabled())
            {
                serviceClient = WCFServiceInstanceLoader.GetServiceInstance<IDiagnosticService>();
            }

            if (serviceClient == null) //Means the given type is not implemented for local load..
            {
                DiagnosticServiceProxy dignosticServiceProxy = null;

                if (String.IsNullOrEmpty(this.EndPointConfigurationName) && this.EndpointAddress == null)
                    dignosticServiceProxy = new DiagnosticServiceProxy();
                else if (!String.IsNullOrEmpty(this.EndPointConfigurationName) && this.EndpointAddress == null)
                    dignosticServiceProxy = new DiagnosticServiceProxy(this.EndPointConfigurationName);
                else if (!String.IsNullOrEmpty(this.EndPointConfigurationName) && this.EndpointAddress != null)
                    dignosticServiceProxy = new DiagnosticServiceProxy(this.EndPointConfigurationName, this.EndpointAddress);

                if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    dignosticServiceProxy.ClientCredentials.UserName.UserName = this.UserName;
                    dignosticServiceProxy.ClientCredentials.UserName.Password = this.Password;
                    dignosticServiceProxy.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.None;
                }

                serviceClient = dignosticServiceProxy;
            }

            return serviceClient;
        }

        /// <summary>
        /// Dispose Diagnostic Service Client
        /// </summary>
        /// <param name="client">Client</param>
        private void DisposeClient(IDiagnosticService client)
        {
            if (client == null)
                return;

            if (client.GetType().Equals(typeof(DiagnosticServiceProxy)))
            {
                DiagnosticServiceProxy serviceClient = (DiagnosticServiceProxy)client;
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

        /// <summary>
        /// Makes the DiagnosticService call. Creates the client instance, executes call delegate against it in
        /// impersonated context (if necessary) and then disposes the client.
        /// Also emits traces when necessary.
        /// </summary>
        /// <typeparam name="TResult">The type of the result of service call.</typeparam>
        /// <param name="clientMethodName">Name of the client method to include in traces.</param>
        /// <param name="serverMethodName">Name of the server method to include in traces.</param>
        /// <param name="call">The call delegate to be executed against a client instance.</param>
        /// <param name="traceSource">Indicate the MDM trace source</param>
        /// <returns>The value returned by service, or default.</returns>
        private TResult MakeServiceCall<TResult>(string clientMethodName, string serverMethodName, Func<IDiagnosticService, TResult> call, MDMTraceSource traceSource = MDMTraceSource.General)
        {
            ExecutionContext executionContext = new ExecutionContext();
            executionContext.LegacyMDMTraceSources.Add(traceSource);

            if (!executionContext.LegacyMDMTraceSources.Contains(MDMTraceSource.DiagnosticService))
            {
                executionContext.LegacyMDMTraceSources.Add(MDMTraceSource.DiagnosticService);
            }

            var activity = new DiagnosticActivity(null, "DiagnosticServiceClient." + clientMethodName);

            //Start trace activity
            if (Constants.TRACING_ENABLED)
            {
                activity.Start();
            }

            TResult result = default(TResult);
            IDiagnosticService diagnosticService = null;

            try
            {
                diagnosticService = GetClient();

                ValidateContext(MDMTraceSource.DiagnosticService);

                if (Constants.TRACING_ENABLED)
                {
                    activity.LogVerbose("DiagnosticServiceClient sends '" + serverMethodName + "' request message.");
                }

                result = Impersonate(() => call(diagnosticService));

                if (Constants.TRACING_ENABLED)
                {
                    activity.LogVerbose("DiagnosticServiceClient receives '" + serverMethodName + "' response message.");
                }
            }
            catch (Exception ex) //Here we will catch all types of exceptions and then real handling happens in the base class method 'HandleException'
            {
                HandleException(ex);
            }
            finally
            {
                //Close the client
                DisposeClient(diagnosticService);

                if (Constants.TRACING_ENABLED)
                {
                    activity.Stop();
                }
            }

            return result;
        }

        #endregion

        #endregion
    }
}