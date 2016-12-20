using System;
using System.Diagnostics;
using System.Transactions;
using System.Collections.ObjectModel;

namespace MDM.EntityProcessorManager.Business
{
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.BusinessObjects;
    using MDM.Interfaces;
    using MDM.BusinessObjects.Denorm;
    using MDM.ConfigurationManager.Business;
    using MDM.MessageManager.Business;
    using MDM.Utility;
    using MDM.EntityProcessorManager.Data.SqlClient;

    /// <summary>
    /// 
    /// </summary>
    public class EntityProcessorErrorLogBL : BusinessLogicBase
    {
        /// <summary>
        /// Field denoting locale Message
        /// </summary>
        private LocaleMessage _localeMessage = null;

        /// <summary>
        /// Field denoting localeMessageBL
        /// </summary>
        private LocaleMessageBL _localeMessageBL = new LocaleMessageBL();

        /// <summary>
        /// 
        /// </summary>
        private SecurityPrincipal _securityPrincipal = null;

        #region Entity Processor ErrorLog

        /// <summary>
        /// 
        /// </summary>
        /// <param name="failedMDMMessagePackageCollection"></param>
        /// <param name="exception"></param>
        /// <param name="coreDataProcessor"></param>
        public void HandleException(MDMMessagePackageCollection failedMDMMessagePackageCollection, Exception exception, CoreDataProcessorList coreDataProcessor)
        {
            try
            {
                CallerContext callerContext = new CallerContext { Application = MDMCenterApplication.PIM, Module = MDMCenterModules.Denorm };

                foreach (MDMMessagePackage messagePackage in failedMDMMessagePackageCollection)
                {
                    Object data = messagePackage.Data;
                    IDataProcessorEntity dataProcessorEntity = (IDataProcessorEntity)data;

                    EntityProcessorErrorLogBL entityProcessorErrorLogManager = new EntityProcessorErrorLogBL();
                    entityProcessorErrorLogManager.WriteErrorLog(dataProcessorEntity, coreDataProcessor, dataProcessorEntity.PerformedAction, exception.ToString(), callerContext);
                }
            }
            catch (Exception ex)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("DataProcessor: {0}:ProduceMessages failed. Exception:{1}", CoreDataProcessorList.EntityActivityLogProcessor.ToString(), ex.ToString()), MDMTraceSource.ParallelProcessingEngine);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="impactedEntity"></param>
        /// <param name="currentEntityActivity"></param>
        /// <param name="exception"></param>
        /// <param name="callerContext"></param>
        public void WriteErrorLog(IDataProcessorEntity processorEntity, CoreDataProcessorList coreDataProcessor, EntityActivityList currentActivityAction, String errorMessage, CallerContext callerContext)
        {
            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "ParallelProcessingEngine." + coreDataProcessor.ToString() + " will try to add record in Impacted entity error log.", MDMTraceSource.ParallelProcessingEngine);

                //Create error log table.
                EntityProcessorErrorLog errorLogEntry = new EntityProcessorErrorLog();
                errorLogEntry.ImpactedEntityId = processorEntity.EntityId;
                errorLogEntry.Priority = processorEntity.Weightage;
                errorLogEntry.PerformedAction = currentActivityAction;
                errorLogEntry.ProcessorName = coreDataProcessor.ToString();
                errorLogEntry.ErrorMessage = errorMessage;
                errorLogEntry.ProgramName = processorEntity.ProgramName;
                errorLogEntry.ContainerId = processorEntity.ContainerId;
                errorLogEntry.EntityActivityLogId = processorEntity.EntityActivityLogId;

                //Save error log.
                if (Process(errorLogEntry, callerContext))
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "ParallelProcessingEngine." + coreDataProcessor.ToString() + " errorlog saved successfully", MDMTraceSource.ParallelProcessingEngine);
                }
                else
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "ParallelProcessingEngine." + coreDataProcessor.ToString() + " error log failed to save", MDMTraceSource.ParallelProcessingEngine);
                }
            }
            catch (Exception innerException)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "ParallelProcessingEngine." + coreDataProcessor.ToString() + " failed. Exception : " + innerException.ToString(), MDMTraceSource.ParallelProcessingEngine);
            }
        }

        /// <summary>
        /// Gets the error log details of the impacted entity which failed during update of impacted entities
        /// </summary>
        /// <param name="entityIdList">entity Id collection indicating Ids of entities to be loaded.</param>
        /// <param name="callerContext">caller context indicating who called the API</param>
        /// <returns></returns>
        public EntityProcessorErrorLogCollection Get(Collection<Int64> entityIdList, CallerContext callerContext)
        {
            EntityProcessorErrorLogCollection entityProcessorErrorLogCollection = this.Get(entityIdList, 0, 0, callerContext);

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
        public EntityProcessorErrorLogCollection Get(Collection<Int64> entityIdList, Int64 fromRecordNumber, Int64 toRecordNumber, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityManager.ImpactedEntityBL.GetEntityProcessorErrorLog", false);

            #region validations

            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "EntityManager.ImpactedEntityBL", String.Empty, "GetEntityProcessorErrorLog");
            }
            if (entityIdList == null || entityIdList.Count <= 0)
            {
                throw new MDMOperationException("111847", "EntityIdList cannot be null or empty.", "EntityManager.ImpactedEntityBL", String.Empty, "GetEntityProcessorErrorLog");
            }

            #endregion validations

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "EntityIds to load : " + ValueTypeHelper.JoinCollection(entityIdList, ","));
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "CallerContext : Application - " + callerContext.Application + " Module - " + callerContext.Module);
            }

            DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Read);
            EntityProcessorErrorLogDA pMErrorHandlerDA = new EntityProcessorErrorLogDA();
            EntityProcessorErrorLogCollection entityProcessorErrorLogCollection = pMErrorHandlerDA.Get(entityIdList, fromRecordNumber, toRecordNumber, command);

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("EntityManager.ImpactedEntityBL.GetEntityProcessorErrorLog");

            return entityProcessorErrorLogCollection;
        }

        /// <summary>
        /// Gets all the details from error log table
        /// </summary>
        /// <param name="callerContext">caller context indicating who called the API</param>
        /// <returns></returns>
        public EntityProcessorErrorLogCollection Get(CallerContext callerContext)
        {
            EntityProcessorErrorLogCollection entityProcessorErrorLogCollection = this.Get(0, 0, callerContext);

            return entityProcessorErrorLogCollection;
        }

        /// <summary>
        /// Gets all the details from error log table
        /// </summary>
        /// <param name="fromRecordNumber">Starting index no. of record which are to be fetched</param>
        /// <param name="toRecordNumber">End index no. of record which are to be fetched</param>
        /// <param name="callerContext">caller context indicating who called the API</param>
        /// <returns></returns>
        public EntityProcessorErrorLogCollection Get(Int64 fromRecordNumber, Int64 toRecordNumber, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityManager.ImpactedEntityBL.GetEntityProcessorErrorLog", false);

            #region validations

            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "EntityManager.ImpactedEntityBL", String.Empty, "Get");
            }

            #endregion validations

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "CallerContext : Application - " + callerContext.Application + " Module - " + callerContext.Module);

            DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Read);
            EntityProcessorErrorLogDA pMErrorHandlerDA = new EntityProcessorErrorLogDA();
            EntityProcessorErrorLogCollection entityProcessorErrorLogCollection = pMErrorHandlerDA.Get(null, fromRecordNumber, toRecordNumber, command);

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("EntityManager.ImpactedEntityBL.GetEntityProcessorErrorLog");

            return entityProcessorErrorLogCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityProcessorErrorLog"></param>
        /// <param name="programName"></param>
        /// <param name="callerContext"></param>
        public Boolean Process(EntityProcessorErrorLog entityProcessorErrorLog, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "EntityProcessorErrorLog Process starting..");

            #region validations

            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "EntityManager.ImpactedEntityBL", String.Empty, "ProcessEntityProcessorErrorLog");
            }

            if (entityProcessorErrorLog == null)
            {
                throw new MDMOperationException("111874", "EntityProcessorErrorLog cannot be null.", "EntityManager.ImpactedEntityBL", String.Empty, "ProcessEntityProcessorErrorLog");
            }

            #endregion validations

            Boolean result = false;
            try
            {
                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    //Get current logon user
                    String loginUser = String.Empty;
                    if (_securityPrincipal != null)
                        loginUser = _securityPrincipal.CurrentUserName;

                    DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Update);
                    EntityProcessorErrorLogDA pMErrorHandlerDA = new EntityProcessorErrorLogDA();
                    result = pMErrorHandlerDA.Process(entityProcessorErrorLog, command, loginUser, callerContext.ProgramName);

                    transactionScope.Complete();
                }
            }
            catch (Exception exception)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "ImpactedEntityBL-ProcessEntityActivityLog: " + exception.Message);
                return false;
            }
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "EntityProcessorErrorLog Process completed successfully.");
            return result;

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
        public Boolean Refresh(Int64 impactedEntityId, Int32 containerId, String processorName, Int64 entityActivityLogId, CallerContext callerContext, Collection<Int32> attributeIdList = null, Collection<Int32> localeIdList = null)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "RefreshEntityProcessorErrorLog Process starting..");

            #region validations

            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "EntityManager.ImpactedEntityBL", String.Empty, "ProcessEntityProcessorErrorLog");
            }

            if (impactedEntityId <= 0)
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Warning, "Entity get request is received without entity id. Get operation is being terminated with exception.");
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111795", false, callerContext);
                throw new MDMOperationException("111795", _localeMessage.Message, "EntityManager", String.Empty, "RefreshEntityProcessorErrorLog");//EntityId must be greater than 0
            }

            #endregion validations

            Boolean result;
            String entityId;

            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Update);
            entityId = impactedEntityId.ToString();
            EntityProcessorErrorLogDA pMErrorHandlerDA = new EntityProcessorErrorLogDA();
            result = pMErrorHandlerDA.Refresh(entityId, containerId, processorName, entityActivityLogId, command, attributeIdList, localeIdList);

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "EntityProcessorErrorLog refresh completed successfully.");
            return result;

        }

        /// <summary>
        /// 
        /// </summary>
        private void GetSecurityPrincipal()
        {
            if (_securityPrincipal == null)
            {
                _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            }
        }

        #endregion Entitity Processor ErrorLog
    }
}
