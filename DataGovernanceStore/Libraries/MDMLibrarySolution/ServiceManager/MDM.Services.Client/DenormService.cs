using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Xml.Linq;
using MDM.Services.ServiceProxies;

namespace MDM.Services
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.Interfaces;
    using MDM.Utility;
    using MDM.WCFServiceInterfaces;

    /// <summary>
    /// Denorm Service facilitates to work with MDMCenter Denorm.
    /// </summary>
    public class DenormService : ServiceClientBase
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructors

        /// <summary>
        /// Use this default constructor only when calling MDM service within the execution context of an MDM Application. 
        /// Example: Service call from the MDM business rule.
        /// This would try to use current execution security context for service authentication purpose. 
        /// </summary>
        public DenormService()
            : base(typeof(DenormService))
        {
            MDMTraceHelper.InitializeTraceSource();
        }

        /// <summary>
        /// Use this default constructor with binding configuration name only when calling MDM service within the execution context of an MDM Application. 
        /// Example: Service call from the MDM business rule.
        /// This would try to use current execution security context for service authentication purpose. 
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        public DenormService(String endPointConfigurationName)
            : base(typeof(DenormService), endPointConfigurationName)
        {
        }

        /// <summary>
        /// Use this constructor for form based authentication passing username and password
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        /// <param name="userName">Name of the Login User</param>
        /// <param name="password">Password of the User</param>
        public DenormService(String endPointConfigurationName, String userName, String password)
            : base(endPointConfigurationName, userName, password)
        {

        }

        /// <summary>
        /// Use this constructor for window authentication passing current windows identity.
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        /// <param name="userIdentity">Indicates User's Identity</param>
        public DenormService(String endPointConfigurationName, IIdentity userIdentity)
            : base(endPointConfigurationName, userIdentity)
        {
        }

        /// <summary>
        /// Constructor with client configuration.
        /// </summary>
        /// <param name="wcfClientConfiguration">Indicates WCF Client Configuration</param>
        public DenormService(IWCFClientConfiguration wcfClientConfiguration)
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
        public DenormService(String endPointConfigurationName, AuthenticationType authenticationType, IIdentity userIdentity, String userName, String password)
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
        public DenormService(String endPointConfigurationName, EndpointAddress endPointAddress, AuthenticationType authenticationType, IIdentity userIdentity, String userName, String password)
            : base(endPointConfigurationName, endPointAddress, authenticationType, userIdentity, userName, password)
        {
        }

        #endregion



        #region Impacted Entity Contracts

        /// <summary>
        /// Load impacted entities in Impacted entities table in database. 
        /// </summary>
        /// <param name="iEntityActivityLogCollection">Collection of entities for which impacted entities are to be loaded</param>
        /// <param name="impactType">Indicates what kind of impacted entities are to be fetched.</param>
        /// <param name="programName">Indicates ProgramName which has made call.</param>
        /// <param name="iCallerContext">Context indicating who called the method.</param>
        /// <returns>No. of entities loaded for given entities.</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public Int64 LoadImpactedEntities(IEntityActivityLogCollection iEntityActivityLogCollection, ImpactType impactType, String programName, ICallerContext iCallerContext)
        {
            return MakeServiceCall("LoadImpactedEntities",
                                   "LoadImpactedEntities",
                                   service => service.LoadImpactedEntities(
                                       (EntityActivityLogCollection) iEntityActivityLogCollection,
                                       impactType,
                                       programName,
                                       FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Get impacted entities by given entityActivityLogId
        /// </summary>
        /// <param name="entityActivityLogId">PK of entityActivityLog table</param>
        /// <param name="iCallerContext">Context indicating who called the method</param>
        /// <returns>Collection of impacted entities.</returns>
        public IImpactedEntityCollection GetImpactedEntities(Int64 entityActivityLogId, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetImpactedEntities",
                                   "GetImpactedEntitiesByEntityActivityLogId",
                                   service => service.GetImpactedEntitiesByEntityActivityLogId(
                                       entityActivityLogId,
                                       FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Get impacted entities by given entityActivityLogId
        /// </summary>
        /// <param name="entityActivityLogId">PK of entityActivityLog table</param>
        /// <param name="fromRecordNumber">Starting index no. of record which are to be fetched</param>
        /// <param name="toRecordNumber">End index no. of record which are to be fetched</param>
        /// <param name="iCallerContext">Context indicating who called the method</param>
        /// <returns>Collection of impacted entities.</returns>
        public IImpactedEntityCollection GetImpactedEntities(Int64 entityActivityLogId, Int64 fromRecordNumber, Int64 toRecordNumber, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetImpactedEntities",
                                   "GetImpactedEntitiesByEntityActivityLogId",
                                   service => service.GetImpactedEntitiesByEntityActivityLogId(
                                       entityActivityLogId,
                                       fromRecordNumber,
                                       toRecordNumber,
                                       FillDiagnosticTraces(iCallerContext)));
        }

        #endregion Impacted Entity Contracts

        #region Entity Processor Error Log Contracts

        /// <summary>
        /// Gets the error log details of the impacted entity which failed during update of impacted entities
        /// </summary>
        /// <param name="entityIdList">entity Id collection indicating Ids of entities to be loaded.</param>
        /// <param name="iCallerContext">caller context indicating who called the API</param>
        /// <returns>Impacted entity error log collection</returns>
        public IEntityProcessorErrorLogCollection GetEntityProcessorErrorLog(Collection<Int64> entityIdList, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetEntityProcessorErrorLog",
                                   "GetEntityProcessorErrorLog",
                                   service => service.GetEntityProcessorErrorLog(
                                       entityIdList,
                                       FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets the error log details of the impacted entity which failed during update of impacted entities
        /// </summary>
        /// <param name="entityIdList">entity Id collection indicating Ids of entities to be loaded.</param>
        /// <param name="fromRecordNumber">Starting index no. of record which are to be fetched</param>
        /// <param name="toRecordNumber">End index no. of record which are to be fetched</param>
        /// <param name="iCallerContext">caller context indicating who called the API</param>
        /// <returns>Impacted entity error log collection</returns>
        public IEntityProcessorErrorLogCollection GetEntityProcessorErrorLog(Collection<Int64> entityIdList, Int64 fromRecordNumber, Int64 toRecordNumber, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetEntityProcessorErrorLog",
                                   "GetEntityProcessorErrorLog",
                                   service => service.GetEntityProcessorErrorLog(
                                       entityIdList,
                                       fromRecordNumber,
                                       toRecordNumber,
                                       FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets all the details from error log table
        /// </summary>
        /// <param name="iCallerContext">caller context indicating who called the API</param>
        /// <returns>Impacted entity error log collection</returns>
        public IEntityProcessorErrorLogCollection GetEntityProcessorErrorLog(ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetEntityProcessorErrorLog",
                                   "GetAllEntityProcessorErrorLog",
                                   service => service.GetAllEntityProcessorErrorLog(FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets all the details from error log table
        /// </summary>
        /// <param name="fromRecordNumber">Starting index no. of record which are to be fetched</param>
        /// <param name="toRecordNumber">End index no. of record which are to be fetched</param>
        /// <param name="iCallerContext">caller context indicating who called the API</param>
        /// <returns>Impacted entity error log collection</returns>
        public IEntityProcessorErrorLogCollection GetAllEntityProcessorErrorLog(Int64 fromRecordNumber, Int64 toRecordNumber, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetAllEntityProcessorErrorLog",
                                   "GetAllEntityProcessorErrorLog",
                                   service => service.GetAllEntityProcessorErrorLog(
                                       fromRecordNumber,
                                       toRecordNumber,
                                       FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Pushes impacted entity from Error log to Impacted table or entity Queue
        /// </summary>
        /// <param name="impactedEntityId">Indicates impacted entity identifier</param>
        /// <param name="containerId">Indicates container identifier for the impacted entity</param>
        /// <param name="processorName">Indicates name of the processor which picked up this entity for processing</param>
        /// <param name="entityActivityLogId">Indicates unique identifier of entity activity log</param>
        /// <param name="iCallerContext">Indicates caller context indicating who called the API</param>
        /// <param name="attributeIdList">Indicates list of attribute identifiers</param>
        /// <param name="localeIdList">Indicates list of locale identifiers</param>
        /// <returns>If success return true else false</returns>
        public Boolean RefreshEntityProcessorErrorLog(Int64 impactedEntityId, Int32 containerId, String processorName, Int64 entityActivityLogId, ICallerContext iCallerContext, Collection<Int32> attributeIdList = null, Collection<Int32> localeIdList = null)
        {
            return MakeServiceCall("RefreshEntityProcessorErrorLog",
                                   "RefreshEntityProcessorErrorLog",
                                   service => service.RefreshEntityProcessorErrorLog(
                                       impactedEntityId,
                                       containerId,
                                       processorName,
                                       entityActivityLogId,
                                       FillDiagnosticTraces(iCallerContext),
                                       attributeIdList,
                                       localeIdList));
        }

        #endregion Entity Processor Error Log Contracts

        #region Process EntityActivityLog Contratcs

        /// <summary>
        /// Processes the impacted entity log
        /// </summary>
        /// <param name="iEntityActivityLogCollection">Collection of entities for which impacted entities are to be processed</param>
        /// <param name="iCallerContext">Context indicating who called the API</param>
        /// <returns>True if processed successfuly</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public Boolean ProcessEntityActivityLogs(IEntityActivityLogCollection iEntityActivityLogCollection, ICallerContext iCallerContext)
        {
            return MakeServiceCall("ProcessEntityActivityLogs",
                                   "ProcessEntityActivityLogs",
                                   service => service.ProcessEntityActivityLogs(
                                       (EntityActivityLogCollection) iEntityActivityLogCollection,
                                       FillDiagnosticTraces(iCallerContext)));
        }

        #endregion

        #region ParallelProcessing Engine monitoring

        /// <summary>
        /// Get parallelization engine status.
        /// </summary>
        /// <returns>Status result of parallelization engine</returns>
        public IParallelizationEngineStatus GetParallelizationEngineStatus()
        {
            return MakeServiceCall("GetParallelizationEngineStatus",
                                   "GetParallelizationEngineStatus",
                                   service => service.GetParallelizationEngineStatus());
        }

        /// <summary>
        /// Start Parallel ProcessingEngine
        /// </summary>
        /// <returns>True if started successfully.</returns>
        public Boolean StartParallelProcessingEngine(String serverName)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DenormServiceClient.StartParallelProcessingEngine", false);

            Boolean result = false;
            IDenormService denormServiceClient = null;

            try
            {
                denormServiceClient = GetClient(serverName);

                base.ValidateContext();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "DenormServiceClient receives 'StartParallelProcessingEngine' request message.");

                if (this.AuthenticationType == Core.AuthenticationType.Windows)
                {
                    WindowsIdentity windowsIdentity = (WindowsIdentity)UserIdentity;

                    using (windowsIdentity.Impersonate())
                    {
                        result = denormServiceClient.StartParallelProcessingEngine();
                    }
                }
                else if (AuthenticationType == AuthenticationType.Forms)
                    result = denormServiceClient.StartParallelProcessingEngine();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "DenormServiceClient sends 'StartParallelProcessingEngine' response message.");

            }
            catch (Exception ex) //Here we will catch all types of exceptions and then real handling happens in the base class method 'HandleException'
            {
                base.HandleException(ex);
            }
            finally
            {
                //Close the client
                this.DisposeClient(denormServiceClient);
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("DenormServiceClient.StartParallelProcessingEngine");

            return result;
        }

        /// <summary>
        /// Stop Parallel ProcessingEngine
        /// </summary>
        /// <returns>True if stopped successfully.</returns>
        public Boolean StopParallelProcessingEngine(String serverName)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DenormServiceClient.StopParallelProcessingEngine", false);

            Boolean result = false;
            IDenormService denormServiceClient = null;

            try
            {

                denormServiceClient = GetClient(serverName);

                base.ValidateContext();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "DenormServiceClient receives 'StopParallelProcessingEngine' request message.");

                if (this.AuthenticationType == Core.AuthenticationType.Windows)
                {
                    WindowsIdentity windowsIdentity = (WindowsIdentity)UserIdentity;

                    using (windowsIdentity.Impersonate())
                    {
                        result = denormServiceClient.StopParallelProcessingEngine();
                    }
                }
                else if (AuthenticationType == AuthenticationType.Forms)
                    result = denormServiceClient.StopParallelProcessingEngine();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "DenormServiceClient sends 'StopParallelProcessingEngine' response message.");

            }
            catch (Exception ex) //Here we will catch all types of exceptions and then real handling happens in the base class method 'HandleException'
            {
                base.HandleException(ex);
            }
            finally
            {
                //Close the client
                this.DisposeClient(denormServiceClient);
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("DenormServiceClient.StopParallelProcessingEngine");

            return result;
        }

        /// <summary>
        /// Stop Parallel ProcessingEngine
        /// </summary>
        /// <returns>True if stopped successfully.</returns>
        public Boolean RestartParallelProcessingEngine(String serverName)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DenormServiceClient.RestartParallelProcessingEngine", false);

            Boolean result = false;
            IDenormService denormServiceClient = null;

            try
            {
                denormServiceClient = GetClient(serverName);

                base.ValidateContext();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "DenormServiceClient receives 'RestartParallelProcessingEngine' request message.");

                if (this.AuthenticationType == Core.AuthenticationType.Windows)
                {
                    WindowsIdentity windowsIdentity = (WindowsIdentity)UserIdentity;

                    using (windowsIdentity.Impersonate())
                    {
                        result = denormServiceClient.RestartParallelProcessingEngine();
                    }
                }
                else if (AuthenticationType == AuthenticationType.Forms)
                    result = denormServiceClient.RestartParallelProcessingEngine();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "DenormServiceClient sends 'RestartParallelProcessingEngine' response message.");

            }
            catch (Exception ex) //Here we will catch all types of exceptions and then real handling happens in the base class method 'HandleException'
            {
                base.HandleException(ex);
            }
            finally
            {
                //Close the client
                this.DisposeClient(denormServiceClient);
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("DenormServiceClient.RestartParallelProcessingEngine");

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataProcessor"></param>
        /// <param name="serverName"></param>
        /// <returns></returns>
        public Boolean RestartProcessor(CoreDataProcessorList dataProcessor, String serverName)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DenormServiceClient.RestartProcessor", false);

            Boolean result = false;
            IDenormService denormServiceClient = null;

            try
            {
                denormServiceClient = GetClient(serverName);

                base.ValidateContext();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "DenormServiceClient receives 'RestartProcessor' request message.");

                if (this.AuthenticationType == Core.AuthenticationType.Windows)
                {
                    WindowsIdentity windowsIdentity = (WindowsIdentity)UserIdentity;

                    using (windowsIdentity.Impersonate())
                    {
                        result = denormServiceClient.RestartProcessor(dataProcessor);
                    }
                }
                else if (AuthenticationType == AuthenticationType.Forms)
                    result = denormServiceClient.RestartProcessor(dataProcessor);

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "DenormServiceClient sends 'RestartProcessor' response message.");

            }
            catch (Exception ex) //Here we will catch all types of exceptions and then real handling happens in the base class method 'HandleException'
            {
                base.HandleException(ex);
            }
            finally
            {
                //Close the client
                this.DisposeClient(denormServiceClient);
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("DenormServiceClient.RestartProcessor");

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataProcessor"></param>
        /// <param name="serverName"></param>
        /// <returns></returns>
        public Boolean StartProcessor(CoreDataProcessorList dataProcessor, String serverName)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DenormServiceClient.StartProcessor", false);

            Boolean result = false;
            IDenormService denormServiceClient = null;

            try
            {
                denormServiceClient = GetClient(serverName);

                base.ValidateContext();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "DenormServiceClient receives 'StartProcessor' request message.");

                if (this.AuthenticationType == Core.AuthenticationType.Windows)
                {
                    WindowsIdentity windowsIdentity = (WindowsIdentity)UserIdentity;

                    using (windowsIdentity.Impersonate())
                    {
                        result = denormServiceClient.StartProcessor(dataProcessor);
                    }
                }
                else if (AuthenticationType == AuthenticationType.Forms)
                    result = denormServiceClient.StartProcessor(dataProcessor);

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "DenormServiceClient sends 'StartProcessor' response message.");

            }
            catch (Exception ex) //Here we will catch all types of exceptions and then real handling happens in the base class method 'HandleException'
            {
                base.HandleException(ex);
            }
            finally
            {
                //Close the client
                this.DisposeClient(denormServiceClient);
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("DenormServiceClient.StartProcessor");

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataProcessor"></param>
        /// <param name="serverName"></param>
        /// <returns></returns>
        public Boolean StopProcessor(CoreDataProcessorList dataProcessor, String serverName)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DenormServiceClient.StopProcessor", false);

            Boolean result = false;
            IDenormService denormServiceClient = null;

            try
            {
                denormServiceClient = GetClient(serverName);

                base.ValidateContext();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "DenormServiceClient receives 'StopProcessor' request message.");

                if (this.AuthenticationType == Core.AuthenticationType.Windows)
                {
                    WindowsIdentity windowsIdentity = (WindowsIdentity)UserIdentity;

                    using (windowsIdentity.Impersonate())
                    {
                        result = denormServiceClient.StopProcessor(dataProcessor);
                    }
                }
                else if (AuthenticationType == AuthenticationType.Forms)
                    result = denormServiceClient.StopProcessor(dataProcessor);

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "DenormServiceClient sends 'StopProcessor' response message.");

            }
            catch (Exception ex) //Here we will catch all types of exceptions and then real handling happens in the base class method 'HandleException'
            {
                base.HandleException(ex);
            }
            finally
            {
                //Close the client
                this.DisposeClient(denormServiceClient);
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("DenormServiceClient.StopProcessor");

            return result;
        }

        /// <summary>
        /// Get All Service Status
        /// </summary>
        /// <param name="iCallerContext">Context indicating who called the method</param>
        /// <returns>Status Info of all server</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IServiceStatusCollection GetServiceStatus(ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetServiceStatus",
                                   "GetServiceStatus",
                                   service => service.GetServiceStatus(FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Process all Service Status
        /// </summary>
        /// <param name="serverName">Name of the server</param>
        /// <param name="service">Name of the service</param>
        /// <param name="serviceSubType">Name of the service sub type</param>
        /// <param name="serviceStatusXml">Xml as string to det service status.</param>
        /// <param name="serviceConfigXml">Xml as string to set the configuration for all processors.</param>
        /// <param name="iCallerContext">Context indicating who called the method</param>
        /// <returns>True if process is successful</returns>
        public Boolean ProcessServiceStatus(String serverName, MDMServiceType service, MDMServiceSubType serviceSubType, String serviceStatusXml, String serviceConfigXml, ICallerContext iCallerContext)
        {
            return MakeServiceCall("ProcessServiceStatus",
                                   "ProcessServiceStatus",
                                   denormService => denormService.ProcessServiceStatus(
                                       serverName,
                                       service,
                                       serviceSubType,
                                       serviceStatusXml,
                                       serviceConfigXml,
                                       FillDiagnosticTraces(iCallerContext)));
        }

        #endregion ParallelProcessing Engine monitoring

        #region Private Methods

        private IDenormService GetClient(String serverName = "")
        {
            IDenormService denormServiceClient = null;

            if (WCFServiceInstanceLoader.IsLocalInstancesEnabled())
            {
                denormServiceClient = WCFServiceInstanceLoader.GetServiceInstance<IDenormService>();
            }

            if (denormServiceClient == null)
            {
                DenormServiceProxy denormServiceProxyClient = null;

                if (String.IsNullOrEmpty(this.EndPointConfigurationName) && this.EndpointAddress == null)
                    denormServiceProxyClient = new DenormServiceProxy();
                else if (!String.IsNullOrEmpty(this.EndPointConfigurationName) && this.EndpointAddress == null)
                    denormServiceProxyClient = new DenormServiceProxy(this.EndPointConfigurationName);
                else if (!String.IsNullOrEmpty(this.EndPointConfigurationName) && this.EndpointAddress != null)
                    denormServiceProxyClient = new DenormServiceProxy(this.EndPointConfigurationName,
                                                                      this.EndpointAddress);

                if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    denormServiceProxyClient.ClientCredentials.UserName.UserName = this.UserName;
                    denormServiceProxyClient.ClientCredentials.UserName.Password = this.Password;
                    denormServiceProxyClient.ClientCredentials.ServiceCertificate.Authentication
                                            .CertificateValidationMode =
                        System.ServiceModel.Security.X509CertificateValidationMode.None;
                }

                if (!String.IsNullOrWhiteSpace(serverName))
                {
                    serverName = GetServer(serverName);

                    String uriString = (this.AuthenticationType == AuthenticationType.Forms) ? String.Format("http://{0}/DenormService.svc", serverName) : String.Format("net.tcp://{0}/DenormService.svc", serverName);

                    denormServiceProxyClient.Endpoint.Address = new EndpointAddress(new Uri(uriString), denormServiceProxyClient.Endpoint.Address.Identity, denormServiceProxyClient.Endpoint.Address.Headers);
                }

                denormServiceClient = denormServiceProxyClient;

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information,
                                              String.Format(
                                                  "Client context for this request: IsDelegationEnabled-{0}; AuthenticationType-{1}; EndPointConfigurationName-{2}; UserName-{3}; UserIdentityName-{4}",
                                                  this.IsDelegationEnabled, this.AuthenticationType,
                                                  this.EndPointConfigurationName, this.UserName,
                                                  ((this.UserIdentity != null) ? this.UserIdentity.Name : string.Empty)));
            }

            return denormServiceClient;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverName"></param>
        /// <returns></returns>
        private String GetServer(String serverName)
        {
            String server = String.Empty;
            String serverXml = this.GetAppConfig("MDMCenter.Diagnostics.ServerConfiguration");

            if (!String.IsNullOrWhiteSpace(serverXml))
            {
                XDocument xDoc = XDocument.Parse(serverXml);

                server = (from f in xDoc.Descendants("WCFServers")
                          from f1 in f.Elements("WCFServer")
                          where (f1.Attribute("ServerName").Value).ToUpper() == serverName.ToUpper()
                          select f1.Attribute("ServerName").Value + "/" + f1.Attribute("VirtualDirectory").Value).FirstOrDefault();
            }

            if (String.IsNullOrWhiteSpace(server))
            {
                throw new Exception(String.Format("Could not connect to server: {0}", serverName));
            }
            return server;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        private void DisposeClient(IDenormService client)
        {
            if (client == null)
                return;

            if (client.GetType() == typeof(DenormServiceProxy))
            {
                DenormServiceProxy serviceClient = (DenormServiceProxy)client;
                if (serviceClient.State == CommunicationState.Created || serviceClient.State == CommunicationState.Opened || serviceClient.State == CommunicationState.Opening)
                {
                    serviceClient.Close();
                }
                else if (serviceClient.State == CommunicationState.Faulted)
                {
                    serviceClient.Abort();
                }
            }
        }

        /// <summary>
        /// Makes the DataServiceClient call. Creates the client instance, executes call delegate against it in
        /// impersonated context (if necessary) and then disposes the client.
        /// Also emits traces when necessary.
        /// </summary>
        /// <typeparam name="TResult">Indicates type of the result of service call.</typeparam>
        /// <param name="clientMethodName">Indicates name of the client method to include in traces.</param>
        /// <param name="serverMethodName">Indicates name of the server method to include in traces.</param>
        /// <param name="call">Indicates call delegate to be executed against a client instance.</param>
        /// <param name="traceSource">Indicates name of the trace source</param>
        /// <returns>Returns value returned by service, or default.</returns>
        private TResult MakeServiceCall<TResult>(String clientMethodName, String serverMethodName, Func<IDenormService, TResult> call, MDMTraceSource traceSource = MDMTraceSource.APIFramework)
        {
            //Start trace activity
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DenormServiceClient." + clientMethodName, traceSource, false);

            TResult result = default(TResult);
            IDenormService denormServiceClient = null;

            try
            {
                denormServiceClient = GetClient();

                ValidateContext();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "DenormServiceClient sends '" + serverMethodName + "' request message.", traceSource);

                result = Impersonate(() => call(denormServiceClient));

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "DenormServiceClient receives '" + serverMethodName + "' response message.", traceSource);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                //Close the client
                DisposeClient(denormServiceClient);

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DenormServiceClient." + clientMethodName, traceSource);
            }

            return result;
        }

        #endregion Private Methods
    }
}