using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.ServiceModel;
using  System.Linq;

namespace MDM.WCFServices
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Denorm;
    using MDM.Core;
    using MDM.EntityManager.Business;
    using MDM.ActivityLogManager.Business;
    using MDM.Interfaces;
    using MDM.MonitoringManager.Business;
    using MDM.ParallelProcessingService;
    using MDM.Utility;
    using MDM.WCFServiceInterfaces;
    using MDM.EntityProcessorManager.Business;

    /// <summary>
    /// The class which implements Denorm Service Contract
    /// </summary>
    [DiagnosticActivity]
    [ServiceBehavior(Namespace = "http://wcfservices.riversand.com", InstanceContextMode = InstanceContextMode.PerCall)]
    public class DenormService : MDMWCFBase, IDenormService
    {
        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public DenormService() 
            : base(true)
        { 
        }

        #endregion 


        #region Get Impacted Entity

        /// <summary>
        /// Get impacted entities by given entityActivityLogId
        /// </summary>
        /// <param name="entityActivityLogId">PK of entityActivityLog table</param>
        /// <param name="callerContext">Context indicating who called the method</param>
        /// <returns>Collection of impacted entities.</returns>
        public ImpactedEntityCollection GetImpactedEntitiesByEntityActivityLogId(Int64 entityActivityLogId, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DenormService.GetImpactedEntitiesByentityActivityLogId", false);

            ImpactedEntityCollection impactedEntityCollection = null;

            try
            {
                 if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DenormService receives 'GetImpactedEntitiesByentityActivityLogId' request message.");

                ImpactedEntityBL impactedEntityBL = new ImpactedEntityBL();
                impactedEntityCollection = impactedEntityBL.GetByEntityActivityLogId(entityActivityLogId, callerContext);

                 if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DenormService sends 'GetImpactedEntitiesByentityActivityLogId' response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("DenormService.GetImpactedEntitiesByentityActivityLogId");

            return impactedEntityCollection;
        }

        /// <summary>
        /// Get impacted entities by given entityActivityLogId
        /// </summary>
        /// <param name="entityActivityLogId">PK of entityActivityLog table</param>
        /// <param name="fromRecordNumber">Starting index no. of record which are to be fetched</param>
        /// <param name="toRecordNumber">End index no. of record which are to be fetched</param>
        /// <param name="callerContext">Context indicating who called the method</param>
        /// <returns>Collection of impacted entities.</returns>
        public ImpactedEntityCollection GetImpactedEntitiesByEntityActivityLogId(Int64 entityActivityLogId, Int64 fromRecordNumber, Int64 toRecordNumber, CallerContext callerContext)
        {
            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DenormService.GetImpactedEntitiesByentityActivityLogId", false);

            ImpactedEntityCollection impactedEntityCollection = null;

            try
            {
                 if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DenormService receives 'GetImpactedEntitiesByentityActivityLogId' request message.");

                ImpactedEntityBL impactedEntityBL = new ImpactedEntityBL();
                impactedEntityCollection = impactedEntityBL.GetByEntityActivityLogId(entityActivityLogId, fromRecordNumber, toRecordNumber, callerContext);

                 if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DenormService sends 'GetImpactedEntitiesByentityActivityLogId' response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("DenormService.GetImpactedEntitiesByentityActivityLogId");

            return impactedEntityCollection;
        }

        /// <summary>
        /// Load impacted entities in Impacted entities table in database. 
        /// </summary>
        /// <param name="entityActivityLogCollection">Collection of entities for which impacted entities are to be loaded</param>
        /// <param name="impactType">Indicates what kind of impacted entities are to be fetched.</param>
        /// <param name="programName">Indicates ProgramName which has made call.</param>
        /// <param name="callerContext">Context indicating who called the method.</param>
        /// <returns>No. of entities loaded for given entities.</returns>
        public Int64 LoadImpactedEntities(EntityActivityLogCollection entityActivityLogCollection, ImpactType impactType, String programName, CallerContext callerContext)
        {
            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DenormService.LoadImpactedEntities", false);

            Int64 result;

            try
            {
                 if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DenormService receives 'LoadImpactedEntities' request message.");

                ImpactedEntityBL impactedEntityBL = new ImpactedEntityBL();
                result = impactedEntityBL.LoadImpactedEntities(entityActivityLogCollection.FirstOrDefault(), impactType, programName, callerContext);

                 if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DenormService sends 'LoadImpactedEntities' response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("DenormService.LoadImpactedEntities");

            return result;
        }

        #endregion Get Impacted Entity

        #region Get Entity Processor Error Log

        /// <summary>
        /// Gets all the details from error log table
        /// </summary>
        /// <param name="callerContext">caller context indicating who called the API</param>
        /// <returns></returns>
        public EntityProcessorErrorLogCollection GetAllEntityProcessorErrorLog(CallerContext callerContext)
        {
            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DenormService.GetAllEntityProcessorErrorLog", false);

            EntityProcessorErrorLogCollection entityProcessorErrorLogCollection = null;

            try
            {
                 if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DenormService receives 'GetAllEntityProcessorErrorLog' request message.");

                EntityProcessorErrorLogBL pMErrorHandlerBL = new EntityProcessorErrorLogBL();
                entityProcessorErrorLogCollection = pMErrorHandlerBL.Get(callerContext);

                 if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DenormService sends 'GetAllEntityProcessorErrorLog' response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("DenormService.GetAllEntityProcessorErrorLog");

            return entityProcessorErrorLogCollection;
        }

        /// <summary>
        /// Gets all the details from error log table
        /// </summary>
        /// <param name="callerContext">caller context indicating who called the API</param>
        /// <param name="fromRecordNumber">Starting index no. of record which are to be fetched</param>
        /// <param name="toRecordNumber">End index no. of record which are to be fetched</param>
        /// <returns>Impacted entity error log collection</returns>
        public EntityProcessorErrorLogCollection GetAllEntityProcessorErrorLog(Int64 fromRecordNumber, Int64 toRecordNumber, CallerContext callerContext)
        {
            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DenormService.GetAllEntityProcessorErrorLog", false);

            EntityProcessorErrorLogCollection entityProcessorErrorLogCollection = null;

            try
            {
                 if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DenormService receives 'GetAllEntityProcessorErrorLog' request message.");

                EntityProcessorErrorLogBL pMErrorHandlerBL = new EntityProcessorErrorLogBL();
                entityProcessorErrorLogCollection = pMErrorHandlerBL.Get(fromRecordNumber, toRecordNumber, callerContext);

                 if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DenormService sends 'GetAllEntityProcessorErrorLog' response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("DenormService.GetAllEntityProcessorErrorLog");

            return entityProcessorErrorLogCollection;
        }

        /// <summary>
        /// Gets the error log details of the impacted entity which failed during update of impacted entities
        /// </summary>
        /// <param name="entityIdList">entity Id collection indicating Ids of entities to be loaded.</param>
        /// <param name="callerContext">caller context indicating who called the API</param>
        /// <returns></returns>
        public EntityProcessorErrorLogCollection GetEntityProcessorErrorLog(Collection<Int64> entityIdList, CallerContext callerContext)
        {
            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DenormService.GetEntityProcessorErrorLog", false);

            EntityProcessorErrorLogCollection entityProcessorErrorLogCollection = null;

            try
            {
                 if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DenormService receives 'GetEntityProcessorErrorLog' request message.");

                EntityProcessorErrorLogBL pMErrorHandlerBL = new EntityProcessorErrorLogBL();
                entityProcessorErrorLogCollection = pMErrorHandlerBL.Get(entityIdList, callerContext);

                 if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DenormService sends 'GetEntityProcessorErrorLog' response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("DenormService.GetEntityProcessorErrorLog");

            return entityProcessorErrorLogCollection;
        }

        /// <summary>
        /// Gets the error log details of the impacted entity which failed during update of impacted entities
        /// </summary>
        /// <param name="entityIdList">entity Id collection indicating Ids of entities to be loaded.</param>
        /// <param name="fromRecordNumber">Starting index no. of record which are to be fetched</param>
        /// <param name="toRecordNumber">End index no. of record which are to be fetched</param>
        /// <param name="callerContext">caller context indicating who called the API</param>
        /// <returns></returns>
        public EntityProcessorErrorLogCollection GetEntityProcessorErrorLog(Collection<Int64> entityIdList, Int64 fromRecordNumber, Int64 toRecordNumber, CallerContext callerContext)
        {
            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DenormService.GetEntityProcessorErrorLog", false);

            EntityProcessorErrorLogCollection entityProcessorErrorLogCollection = null;

            try
            {
                 if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DenormService receives 'GetEntityProcessorErrorLog' request message.");

                EntityProcessorErrorLogBL pMErrorHandlerBL = new EntityProcessorErrorLogBL();
                entityProcessorErrorLogCollection = pMErrorHandlerBL.Get(entityIdList, fromRecordNumber, toRecordNumber, callerContext);

                 if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DenormService sends 'GetEntityProcessorErrorLog' response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("DenormService.GetEntityProcessorErrorLog");

            return entityProcessorErrorLogCollection;
        }

        /// <summary>
        /// Pushes impacted entity from Error log to Impacted table or entity Queue
        /// </summary>
        /// <param name="impactedEntityId">impacted entityId</param>
        /// <param name="containerId">containerId for the impacted entity</param>
        /// <param name="processorName">Name of the Processor which picked up this entity for processing</param>
        /// <param name="entityActivityLogId">unique identifier of entity activity log</param>
        /// <param name="performedAction">action performed on this entity</param>
        /// <param name="callerContext">caller context indicating who called the API</param>
        /// <returns>If success return true else false</returns>
        public Boolean RefreshEntityProcessorErrorLog(Int64 impactedEntityId, Int32 containerId, String processorName, Int64 entityActivityLogId, CallerContext callerContext, Collection<Int32> attributeIdList = null, Collection<Int32> localeIdList = null)
        {
            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DenormService.RefreshEntityProcessorErrorLog", false);

            Boolean result = false;

            try
            {
                 if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DenormService receives 'RefreshEntityProcessorErrorLog' request message.");

                EntityProcessorErrorLogBL pMErrorHandlerBL = new EntityProcessorErrorLogBL();
                result = pMErrorHandlerBL.Refresh(impactedEntityId, containerId, processorName, entityActivityLogId, callerContext, attributeIdList, localeIdList);

                 if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DenormService sends 'RefreshEntityProcessorErrorLog' response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("DenormService.RefreshEntityProcessorErrorLog");

            return result;
        }

        #endregion Get Entity Processor Error Log

        #region Process Entity Activity Logs

        /// <summary>
        /// Processes the entity activity log 
        /// </summary>
        /// <param name="entityActivityLogCollection">Collection of entities for which impacted entities are to be processed</param>
        /// <param name="programName">Name of the program who invoked this operation</param>
        /// <param name="callerContext">Context indicating who called the API</param>
        /// <returns>true if processed successful</returns>
        public Boolean ProcessEntityActivityLogs(EntityActivityLogCollection entityActivityLogCollection, CallerContext callerContext)
        {
            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DenormService.ProcessEntityActivityLog", false);

            Boolean result = false;

            try
            {
                 if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DenormService receives 'ProcessEntityActivityLog' request message.");

                EntityActivityLogBL entityActivityLogManager = new EntityActivityLogBL();
                result = entityActivityLogManager.Process(entityActivityLogCollection, callerContext);

                 if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DenormService sends 'ProcessEntityActivityLog' response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("DenormService.ProcessEntityActivityLog");

            return result;
        }

        #endregion Process Impacted EntityLog

        #region ParallelProcessing Engine monitoring

        /// <summary>
        /// Get parallelization engine status.
        /// </summary>
        /// <returns>Status result of parallelization engine</returns>
        public ParallelizationEngineStatus GetParallelizationEngineStatus()
        {
            MDMTraceHelper.InitializeTraceSource();
            ParallelizationEngineStatus parallelizationEngineStatus = null;
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DenormService.GetParallelizationEngineStatus", false);

            try
            {
                 if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DenormService receives 'GetParallelizationEngineStatus' request message.");

                parallelizationEngineStatus = ParallelProcessingEngine.GetStatus();
                 if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DenormService sends 'GetParallelizationEngineStatus' response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("DenormService.GetParallelizationEngineStatus");

            return parallelizationEngineStatus;
        }

        /// <summary>
        /// Start Parallel ProcessingEngine
        /// </summary>
        /// <returns>True if started successfully.</returns>
        public Boolean StartParallelProcessingEngine()
        {
            MDMTraceHelper.InitializeTraceSource();
            Boolean result = false;
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DenormService.StartParallelProcessingEngine", false);

            try
            {
                 if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DenormService receives 'StartParallelProcessingEngine' request message.");

                ParallelProcessingEngine engine = ParallelProcessingEngine.GetSingleton();
                result = engine.Start();

                 if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DenormService sends 'StartParallelProcessingEngine' response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("DenormService.StartParallelProcessingEngine");

            return result;
        }

        /// <summary>
        /// Stop Parallel ProcessingEngine
        /// </summary>
        /// <returns>True if stopped successfully.</returns>
        public Boolean StopParallelProcessingEngine()
        {
            MDMTraceHelper.InitializeTraceSource();
            Boolean result = false;
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DenormService.StopParallelProcessingEngine", false);

            try
            {
                 if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DenormService receives 'StopParallelProcessingEngine' request message.");

                ParallelProcessingEngine engine = ParallelProcessingEngine.GetSingleton();
                result = engine.Stop(true);

                 if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DenormService sends 'StopParallelProcessingEngine' response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("DenormService.StopParallelProcessingEngine");

            return result;
        }

        /// <summary>
        /// Restart Parallel ProcessingEngine
        /// </summary>
        /// <returns>True if restarted successfully.</returns>
        public Boolean RestartParallelProcessingEngine()
        {
            MDMTraceHelper.InitializeTraceSource();
            Boolean result = false;
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DenormService.RestartParallelProcessingEngine", false);

            try
            {
                 if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DenormService receives 'RestartParallelProcessingEngine' request message.");

                ParallelProcessingEngine engine = ParallelProcessingEngine.GetSingleton();
                result = engine.Restart();

                 if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DenormService sends 'RestartParallelProcessingEngine' response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("DenormService.RestartParallelProcessingEngine");

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataProcessor"></param>
        /// <returns></returns>
        public Boolean RestartProcessor(CoreDataProcessorList dataProcessor)
        {
            MDMTraceHelper.InitializeTraceSource();
            Boolean result = false;
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DenormService.RestartProcessor", false);

            try
            {
                 if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DenormService receives 'RestartProcessor' request message.");

                ParallelProcessingEngine engine = ParallelProcessingEngine.GetSingleton();
                result = engine.RestartProcessor(dataProcessor);

                 if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DenormService sends 'RestartProcessor' response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("DenormService.RestartProcessor");

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataProcessor"></param>
        /// <returns></returns>
        public Boolean StopProcessor(CoreDataProcessorList dataProcessor)
        {
            MDMTraceHelper.InitializeTraceSource();
            Boolean result = false;
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DenormService.StopProcessor", false);

            try
            {
                 if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DenormService receives 'StopProcessor' request message.");

                ParallelProcessingEngine engine = ParallelProcessingEngine.GetSingleton();
                result = engine.StopProcessor(dataProcessor);

                 if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DenormService sends 'StopProcessor' response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("DenormService.StopProcessor");

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataProcessor"></param>
        /// <returns></returns>
        public Boolean StartProcessor(CoreDataProcessorList dataProcessor)
        {
            MDMTraceHelper.InitializeTraceSource();
            Boolean result = false;
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DenormService.StartProcessor", false);

            try
            {
                 if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DenormService receives 'StartProcessor' request message.");

                ParallelProcessingEngine engine = ParallelProcessingEngine.GetSingleton();
                result = engine.StartProcessor(dataProcessor);

                 if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DenormService sends 'StartProcessor' response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("DenormService.StartProcessor");

            return result;
        }

        /// <summary>
        /// Get All Service Status
        /// </summary>
        /// <param name="callerContext">Context indicating who called the method</param>
        /// <returns>Status Info of all server</returns>
        public ServiceStatusCollection GetServiceStatus(CallerContext callerContext)
        {
            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DenormService.GetServiceStatus", false);

            ServiceStatusCollection serviceStatusCollection = null;

            try
            {
                 if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DenormService receives 'GetServiceStatus' request message.");

                MonitorBL monitorBL = new MonitorBL();
                serviceStatusCollection = monitorBL.GetServiceStatus(callerContext);

                 if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DenormService sends 'GetServiceStatus' response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("DenormService.GetServiceStatus");

            return serviceStatusCollection;
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
        /// <returns>True if process is successful.</returns>
        public Boolean ProcessServiceStatus(String serverName, MDMServiceType service, MDMServiceSubType serviceSubType, String serviceStatusXml, String serviceConfigXml, CallerContext callerContext)
        {
            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DenormService.ProcessServiceStatus", false);
            Boolean result = false;

            try
            {
                 if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DenormService receives 'ProcessServiceStatus' request message.");

                MonitorBL monitorBL = new MonitorBL();
                result = monitorBL.ProcessServiceStatus(serverName, service, serviceSubType, serviceStatusXml, serviceConfigXml, callerContext);

                 if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DenormService sends 'ProcessServiceStatus' response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("DenormService.ProcessServiceStatus");

            return result;
        }

        #endregion ParallelProcessing Engine monitoring
    }
}
