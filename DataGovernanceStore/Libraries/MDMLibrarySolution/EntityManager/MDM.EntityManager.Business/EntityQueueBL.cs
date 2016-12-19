using System;
using System.Collections.ObjectModel;
using System.Transactions;
using System.Diagnostics;
using System.Collections.Generic;

namespace MDM.EntityManager.Business
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.EntityManager.Data;
    using MDM.Core.Exceptions;
    using MDM.MessageManager.Business;
    using MDM.Utility;
    using MDM.ConfigurationManager.Business;
    using MDM.CacheManager.Business;

    /// <summary>
    /// Specifies the business operations for entity queue.
    /// </summary>
    public class EntityQueueBL : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private EntityQueueDA _entityQueueDA = new EntityQueueDA();

        /// <summary>
        /// Field denoting localeMessageBL
        /// </summary>
        private LocaleMessageBL _localeMessageBL = new LocaleMessageBL();

        /// <summary>
        /// 
        /// </summary>
        private SecurityPrincipal _securityPrincipal = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public EntityQueueBL()
        {
            //GetSecurityPrincipal();
        }

        #endregion Ctor

        #region Properties
        #endregion

        #region  Methods

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityActivityLog"></param>
        /// <param name="callerContext">Context details of caller</param>
        /// <returns></returns>
        public Int64 Load(EntityActivityLog entityActivityLog, CallerContext callerContext)
        {
            return Load(new EntityActivityLogCollection() { entityActivityLog }, callerContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityActivityLogCollection"></param>
        /// <param name="callerContext">Context details of caller</param>
        /// <returns></returns>
        public Int64 Load(EntityActivityLogCollection entityActivityLogCollection, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("EntityManager.EntityQueueBL.Load", false);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Load entity queue starting..");
            }

            #region validations

            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "EntityManager.EntityQueueBL", String.Empty, "Get");
            }
            if (entityActivityLogCollection == null || entityActivityLogCollection.Count <= 0)
            {
                throw new MDMOperationException("111816", "EntityActivityLogCollection cannot be null or empty.", "EntityManager.EntityQueueBL", String.Empty, "Get");
            }

            #endregion validations

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "CallerContext : Application - " + callerContext.Application + " Module - " + callerContext.Module);

            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

            Int64 queuedEntityCount = 0;

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                _entityQueueDA.Load(entityActivityLogCollection, callerContext, out queuedEntityCount, command);

                transactionScope.Complete();
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("EntityManager.EntityQueueBL.Load");

            return queuedEntityCount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityActivities"></param>
        /// <param name="batchSize"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public QueuedEntityCollection GetNextBatch(List<EntityActivityList> entityActivities, Int32 batchSize, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityManager.EntityQueueBL.GetNextBatch", false);

            #region validations

            if (batchSize < 1)
            {
                throw new MDMOperationException("111863", "BatchSize for getting next batch of entity queue items cannnot be less than 1", "EntityManager.EntityQueueBL", String.Empty, "Get");
            }

            #endregion validations

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Queued entity Get BatchSize : " + batchSize.ToString());
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "CallerContext : Application - " + callerContext.Application + " Module - " + callerContext.Module);
            }

            // Get command
            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

            QueuedEntityCollection queuedEntities = _entityQueueDA.Get(entityActivities, 0, batchSize, command);

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("EntityManager.EntityQueueBL.GetNextBatch");

            return queuedEntities;
        }

        /// <summary>
        /// Get queued entities by given entityActivityLogId
        /// </summary>
        /// <param name="entityActivity"></param>
        /// <param name="entityActivityLogId">PK of entityActivityLog table</param>
        /// <param name="callerContext">Context indicating who called the method</param>
        /// <returns>Collection of queued entities.</returns>
        public QueuedEntityCollection Get(EntityActivityList entityActivity, Int64 entityActivityLogId, CallerContext callerContext)
        {
            QueuedEntityCollection queuedEntities = this.Get(entityActivity, entityActivityLogId, 0, callerContext);

            return queuedEntities;
        }

        /// <summary>
        /// Get queued entities by given entityActivityLogId
        /// </summary>
        /// <param name="entityActivity">Type of activity we want to get</param>
        /// <param name="entityActivityLogId">PK of entityActivityLog table</param>
        /// <param name="batchSize"></param>
        /// <param name="callerContext">Context indicating who called the method</param>
        /// <returns>Collection of queued entities.</returns>
        public QueuedEntityCollection Get(EntityActivityList entityActivity, Int64 entityActivityLogId, Int32 batchSize, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityManager.EntityQueueBL.GetByEntityActivityLogId", false);

            #region validations

            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "EntityManager.EntityQueueBL", String.Empty, "GetByEntityActivityLogId");
            }

            if (entityActivityLogId <= 0)
            {
                throw new MDMOperationException("111873", "Invalid EntityActivityLog Id.", "EntityManager.EntityQueueBL", String.Empty, "GetByEntityActivityLogId");
            }

            #endregion validations

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "CallerContext : Application - " + callerContext.Application + " Module - " + callerContext.Module);

            // Get command
            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

            List<EntityActivityList> entityActivities = new RS.MDM.Collections.Generic.List<EntityActivityList>();
            entityActivities.Add(entityActivity);

            QueuedEntityCollection queuedEntities = _entityQueueDA.Get(entityActivities, entityActivityLogId, batchSize, command);

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("EntityManager.EntityQueueBL.GetByEntityActivityLogId");

            return queuedEntities;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queueRecordId"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public Boolean Remove(Int64 queueRecordId, CallerContext callerContext)
        {
            Boolean returnFlag = false;
            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                returnFlag = _entityQueueDA.Remove(queueRecordId, callerContext, command);

                transactionScope.Complete();
            }

            return returnFlag;
        }

        /// <summary>
        /// Performs the process operation on Entity Queue based on the PK_EntityQueue specified.
        /// </summary>
        /// <param name="queueRecordId">Entity Queue item primary key value</param>
        /// <param name="callerContext">Context details of caller</param>
        /// <returns></returns>
        public Boolean Process(Int64 queueRecordId, CallerContext callerContext)
        {
            return Process(new Collection<Int64> {queueRecordId}, callerContext);
        }

        /// <summary>
        /// Performs the process operation on Entity Queue based on the PK_EntityQueue list specified
        /// </summary>
        /// <param name="queueRecordIds">Entity Queue item primary key values list</param>
        /// <param name="callerContext">Context details of caller</param>
        /// <returns></returns>
        public Boolean Process(Collection<Int64> queueRecordIds, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityManager.EntityQueueBL.Process", false);

            Boolean returnFlag = false;
            try
            {
                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    returnFlag = _entityQueueDA.Process(queueRecordIds, callerContext, command);

                    transactionScope.Complete();
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("EntityManager.EntityQueueBL.Process");
            }
            return returnFlag;
        }

        /// <summary>
        /// check requested entity Id and container Id are available in EntityQueue or not.
        /// </summary>
        /// <param name="entityId">This parameter is specifying entity id.</param>
        /// <param name="containerId">This parameter is specifying container id.</param>
        /// <param name="entityActivityList">This parameter is specifying entity Activity List.</param>
        /// <param name="callerContext">Name of application and module of caller.</param>
        /// <returns>True : If requested entity id and container id is available EntityQueue else False.</returns>
        public Boolean IsEntityInQueue(Int64 entityId, Int32 containerId, EntityActivityList entityActivityList, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityQueueBL.IsEntityInQueue", MDMTraceSource.General, false);

            Boolean isEntityInQueue = false;

            try
            {
                Boolean isEntityQueueProcessingEnabled = true;

                isEntityQueueProcessingEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.ParallelProcessingEngine.EntityQueueProcessor.Enabled", isEntityQueueProcessingEnabled);

                if (!isEntityQueueProcessingEnabled)
                {
                    isEntityInQueue = false;
                }
                else
                {
                    DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Trying to find requested Entity Id: {0} and Container Id: {1} in Entity Queue...", entityId, containerId), MDMTraceSource.General);

                    isEntityInQueue = _entityQueueDA.IsEntityInQueue(entityId, containerId, entityActivityList, command);

                    if (isEntityInQueue)
                    {
                        if (Constants.TRACING_ENABLED)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested Entity Id: {0} and Container Id: {1} is available Entity Queue.", entityId, containerId), MDMTraceSource.General);
                    }
                    else
                    {
                        if (Constants.TRACING_ENABLED)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested Entity Id: {0} and Container Id: {1} is not available Entity Queue.", entityId, containerId), MDMTraceSource.General);
                    }
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("EntityQueueBL.IsEntityInQueue", MDMTraceSource.General);
            }

            return isEntityInQueue;
        }

        #endregion Public Methods

        #region Private Methods

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

        #endregion

        #endregion
    }
}
