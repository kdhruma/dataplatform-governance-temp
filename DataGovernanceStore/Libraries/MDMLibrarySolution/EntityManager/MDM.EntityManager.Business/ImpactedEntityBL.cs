using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Transactions;
using System.Diagnostics;

namespace MDM.EntityManager.Business
{
    using Core;
    using BusinessObjects;
    using Utility;
    using Data;
    using ConfigurationManager.Business;
    using ActivityLogManager.Business;
    using Core.Exceptions;
    using MessageManager.Business;
    using RelationshipManager.Business;
    using Interfaces;

    /// <summary>
    /// 
    /// </summary>
    public class ImpactedEntityBL : BusinessLogicBase, IImpactedEntityManager
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private readonly ImpactedEntityDA _impactedEntityDA = new ImpactedEntityDA();

        /// <summary>
        /// Field denoting locale Message
        /// </summary>
        private LocaleMessage _localeMessage;

        /// <summary>
        /// Field denoting localeMessageBL
        /// </summary>
        private readonly LocaleMessageBL _localeMessageBL = new LocaleMessageBL();

        #endregion

        #region Constructors

        #endregion Constructors

        #region Methods

        #region Public Methods

        #region Get and Load Impacted Entities

        /// <summary>
        /// Load impacted entities in Impacted entities table in database. 
        /// </summary>
        /// <param name="entityActivityLog">Entity activity log for which impacted entities are to be loaded into database</param>
        /// <param name="impactType">Indicates what kind of impacted entities are to be fetched.</param>
        /// <param name="programName">Indicates ProgramName which has made call.</param>
        /// <param name="callerContext">Context indicating who called the method.</param>
        /// <returns>No. of entities loaded for given entities.</returns>
        public Int64 LoadImpactedEntities(EntityActivityLog entityActivityLog, ImpactType impactType, String programName, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("EntityManager.ImpactedEntityBL.LoadImpactedEntities", false);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Load impacted Entity Get starting..", MDMTraceSource.Entity);
            }

            #region validations

            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "EntityManager.ImpactedEntityBL", String.Empty, "Get");
            }

            if (entityActivityLog == null)
            {
                throw new MDMOperationException("111816", "EntityActivityLog cannot be null or empty.", "EntityManager.ImpactedEntityBL", String.Empty, "Get");
            }

            #endregion validations

            // Get command
            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

            Int64 impactedEntityCount;

            if (entityActivityLog.PerformedAction == EntityActivityList.RelationshipCreate || entityActivityLog.PerformedAction == EntityActivityList.RelationshipUpdate ||
                entityActivityLog.PerformedAction == EntityActivityList.RelationshipDelete || entityActivityLog.PerformedAction == EntityActivityList.RelationshipAttributeUpdate)
            {
                PopulateRelationshipProcessMode(entityActivityLog, callerContext);
            }

            //Here we don't need the result out. So sending ReturnResult = false
            _impactedEntityDA.LoadImpactedEntities(entityActivityLog, impactType, out impactedEntityCount, command, programName, true, true, true, false, false, false, false, false);

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "No. of impacted entities loaded : " + impactedEntityCount, MDMTraceSource.Entity);
                MDMTraceHelper.StopTraceActivity("EntityManager.ImpactedEntityBL.LoadImpactedEntities");
            }

            return impactedEntityCount;
        }

        /// <summary>
        /// Get collection of impacted entity ids for given entity
        /// Collection of impacted entityIds will be returned by this API. This will not populated impacted entities in impacted entity table in database.
        /// </summary>
        /// <param name="entity">Entity for which impacted entities are to be fetched.</param>
        /// <param name="callerContext">Context indicating who called the API</param>
        /// <param name="impactType">Indicates what kind of impacted entities are to be fetched.</param>
        /// <returns>Collection of impacted entities for given entityId</returns>
        public EntityCollection GetImpactedEntities(Entity entity, ImpactType impactType, CallerContext callerContext)
        {
            //Get command properties
            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

            return GetImpactedEntities(entity, command, ImpactType.All);
        }

        /// <summary>
        /// Get next batch of impacted entities which are to be processed for cache invalidation or Denorm refresh.
        /// </summary>
        /// <param name="batchSize">Indicates how many impacted entities are to be loaded.</param>
        /// <param name="callerContext">Context indicating who called the method</param>
        /// <returns>Collection of impacted entities.</returns>
        public ImpactedEntityCollection GetNextBatch(Int32 batchSize, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("EntityManager.ImpactedEntityBL.GetNextBatch", false);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Get impacted entity next batch starting..", MDMTraceSource.Entity);
            }

            #region validations

            if (batchSize < 1)
            {
                throw new MDMOperationException("111863", "BatchSize for getting next batch of impacted entities cannot be less than 1", "EntityManager.ImpactedEntityBL", String.Empty, "Get");
            }

            #endregion validations

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Impacted entity get BatchSize : " + batchSize, MDMTraceSource.Entity);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "CallerContext : Application - " + callerContext.Application + " Module - " + callerContext.Module, MDMTraceSource.Entity);
            }

            // Get command
            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

            ImpactedEntityDA impactedEntityDA = new ImpactedEntityDA();
            ImpactedEntityCollection impactedEntities = impactedEntityDA.GetImpactedEntities(batchSize, 0, 0, 0, command);

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Get impacted entity next batch end..", MDMTraceSource.Entity);
                MDMTraceHelper.StopTraceActivity("EntityManager.ImpactedEntityBL.GetNextBatch");
            }

            return impactedEntities;
        }

        /// <summary>
        /// Get impacted entity by given entityId
        /// </summary>
        /// <param name="entityId">Impacted entity id to be loaded</param>
        /// <param name="callerContext">Context indicating who called the method</param>
        /// <returns>Impacted entities.</returns>
        public ImpactedEntity GetById(Int64 entityId, CallerContext callerContext)
        {
            #region validations

            if (entityId <= 0)
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Warning, "Entity get request is received without entity id. Get impacted entity operation is being terminated with exception.", MDMTraceSource.Entity);
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111795", false, callerContext);
                throw new MDMOperationException("111795", _localeMessage.Message, "EntityManager.ImpactedEntityBL", String.Empty, "GetById");//EntityId must be greater than 0
            }

            #endregion validations

            ImpactedEntity impactedEntity = null;

            ImpactedEntityCollection impactedEntities = GetByIdList(new Collection<Int64> { entityId }, callerContext);

            if (impactedEntities != null && impactedEntities.Count > 0)
            {
                impactedEntity = impactedEntities.FirstOrDefault();
            }
            return impactedEntity;
        }

        /// <summary>
        /// Get impacted entities by given entityIds
        /// </summary>
        /// <param name="entityIdList">List of impacted entity ids to be loaded</param>
        /// <param name="callerContext">Context indicating who called the method</param>
        /// <returns>Collection of impacted entities.</returns>
        public ImpactedEntityCollection GetByIdList(Collection<Int64> entityIdList, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("EntityManager.ImpactedEntityBL.GetByIdList", false);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Get impacted entities by EntityIdList starting..", MDMTraceSource.Entity);
            }

            #region validations

            if (entityIdList == null || entityIdList.Count < 1)
            {
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111785", false, callerContext);
                throw new MDMOperationException("111785", _localeMessage.Message, "EntityManager", String.Empty, "Get");//Entity Ids are not available
            }


            #endregion validations

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "No. of impacted entities to load : " + entityIdList.Count, MDMTraceSource.Entity);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "CallerContext : Application - " + callerContext.Application + " Module - " + callerContext.Module);
            }

            // Get command
            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

            ImpactedEntityDA impactedEntityDA = new ImpactedEntityDA();
            ImpactedEntityCollection impactedEntities = impactedEntityDA.GetImpactedEntities(entityIdList, command);

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Get impacted entities by EntityIdList end..", MDMTraceSource.Entity);
                MDMTraceHelper.StopTraceActivity("EntityManager.ImpactedEntityBL.GetByIdList");
            }

            return impactedEntities;
        }

        /// <summary>
        /// Get impacted entities by given entityActivityLogId
        /// </summary>
        /// <param name="entityActivityLogId">PK of entityActivityLog table</param>
        /// <param name="callerContext">Context indicating who called the method</param>
        /// <returns>Collection of impacted entities.</returns>
        public ImpactedEntityCollection GetByEntityActivityLogId(Int64 entityActivityLogId, CallerContext callerContext)
        {
            ImpactedEntityCollection impactedEntities = GetByEntityActivityLogId(entityActivityLogId, 0, 0, callerContext);

            return impactedEntities;
        }

        /// <summary>
        /// Get impacted entities by given entityActivityLogId
        /// </summary>
        /// <param name="entityActivityLogId">PK of entityActivityLog table</param>
        /// <param name="fromRecordNumber">Starting index no. of record which are to be fetched</param>
        /// <param name="toRecordNumber">End index no. of record which are to be fetched</param>
        /// <param name="callerContext">Context indicating who called the method</param>
        /// <returns>Collection of impacted entities.</returns>
        public ImpactedEntityCollection GetByEntityActivityLogId(Int64 entityActivityLogId, Int64 fromRecordNumber, Int64 toRecordNumber, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("EntityManager.ImpactedEntityBL.GetByentityActivityLogId", false);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Get impacted entities by entityActivityLogId starting..", MDMTraceSource.Entity);
            }

            #region validations

            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "EntityManager.ImpactedEntityBL", String.Empty, "GetByentityActivityLogId");
            }

            if (entityActivityLogId <= 0)
            {
                throw new MDMOperationException("111873", "Invalid EntityActivityLog Id.", "EntityManager.ImpactedEntityBL", String.Empty, "GetByentityActivityLogId");
            }

            #endregion validations

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "CallerContext : Application - " + callerContext.Application + " Module - " + callerContext.Module, MDMTraceSource.Entity);

            // Get command
            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

            ImpactedEntityDA impactedEntityDA = new ImpactedEntityDA();
            ImpactedEntityCollection impactedEntities = impactedEntityDA.GetImpactedEntities(0, entityActivityLogId, fromRecordNumber, toRecordNumber, command);

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Get impacted entities by entityActivityLogId end..", MDMTraceSource.Entity);
                MDMTraceHelper.StopTraceActivity("EntityManager.ImpactedEntityBL.GetByentityActivityLogId");
            }

            return impactedEntities;
        }

        #endregion

        #region Update impacted entities status

        /// <summary>
        /// Update the status of impacted entity in impacted entity table
        /// </summary>
        /// <param name="impactedEntity">Impacted entity to be updated</param>
        /// <param name="callerContext">Context indicating who called the method</param>
        /// <param name="impactAction"></param>
        /// <returns>Operation result indicating the status of operation</returns>
        public EntityOperationResult UpdateImpactedEntityStatus(ImpactedEntity impactedEntity, CallerContext callerContext, String impactAction)
        {
            EntityOperationResult entityOR = new EntityOperationResult();
            EntityOperationResultCollection entityORC = this.UpdateImpactedEntityStatus(new ImpactedEntityCollection { impactedEntity }, callerContext, impactAction);
            if (entityORC != null)
            {
                entityOR = entityORC.FirstOrDefault();
            }

            return entityOR;
        }

        /// <summary>
        /// Update the status of impacted entity in impacted entity table
        /// </summary>
        /// <param name="impactedEntities">Impacted entities to be updated</param>
        /// <param name="callerContext">Context indicating who called the method</param>
        /// <param name="impactAction">Indicating the impact action</param>
        ///  <param name="programName">Indicating the who called this method</param>
        /// <returns>Operation result collection indicating the status of operation</returns>
        public EntityOperationResultCollection UpdateImpactedEntityStatus(ImpactedEntityCollection impactedEntities, CallerContext callerContext, String impactAction)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("EntityManager.ImpactedEntityBL.UpdateImpactedEntityStatus", false);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Impacted Entity Process starting..", MDMTraceSource.Entity);
            }

            EntityOperationResultCollection entityOperationResults = new EntityOperationResultCollection();

            #region validations

            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "EntityManager.ImpactedEntityBL", String.Empty, "Process");
            }
            if (impactedEntities == null || impactedEntities.Count <= 0)
            {
                throw new MDMOperationException("111861", "ImpactedEntities cannot be null or empty.", "EntityManager.ImpactedEntityBL", String.Empty, "Process");
            }

            #endregion validations

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "No. of impacted entity to process : " + impactedEntities.Count, MDMTraceSource.Entity);

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Update);

                ImpactedEntityDA impactedEntityDA = new ImpactedEntityDA();
                entityOperationResults = impactedEntityDA.UpdateEntityStatus(impactedEntities, command, impactAction);

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "EntityOperationResultCollection.Status = " + entityOperationResults.OperationResultStatus, MDMTraceSource.Entity);

                if (entityOperationResults.OperationResultStatus != OperationResultStatusEnum.Failed)
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Committing Transaction", MDMTraceSource.Entity);
                    transactionScope.Complete();
                }
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Impacted Entity process transaction committed.", MDMTraceSource.Entity);
            }

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Impacted Entity Process completed successfully.", MDMTraceSource.Entity);
                MDMTraceHelper.StopTraceActivity("EntityManager.ImpactedEntityBL.UpdateImpactedEntityStatus");
            }

            return entityOperationResults;
        }

        public Boolean ImpactedEntityBulkProcess(EntityCollection entityCollection, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("EntityManager.ImpactedEntityBL.ImpactedEntityBulkProcess", false);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Bulk Impacted Entity Process starting..", MDMTraceSource.Entity);
            }

            Boolean result = true;

            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Update);

            ImpactedEntityDA impactedEntityDA = new ImpactedEntityDA();
            result = impactedEntityDA.ImpactedEntityBulkProcess(entityCollection, command);

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "ImpactedEntityBulkProcess.Status = " + result.ToString(), MDMTraceSource.Entity);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Impacted Entity Process completed successfully.", MDMTraceSource.Entity);
                MDMTraceHelper.StopTraceActivity("EntityManager.ImpactedEntityBL.ImpactedEntityBulkProcess");
            }

            return result;
        }

        #endregion Update impacted entities status

        #region Cache status methods

        /// <summary>
        /// Get cache status of given entity. 
        /// This will return value based on if entity is available in tb_Impacted_Entity if impacted entities are loaded or based on available parent in tb_EntityActivityLog.
        /// </summary>
        /// <param name="entityId">Entity for which cache status is to be checked</param>
        /// <param name="containerId">Container Id of entity</param>
        /// <param name="entityTreeIdList">Parent entity id list</param>
        /// <param name="callerContext">Context of application making call to this API</param>
        /// <returns>
        /// Tuple indicating cache status. Values in tuple are as following:
        /// Item1 -> CurrentEntityId
        /// Item2 -> CacheStatus enum indicating cache status
        /// ITem3 -> If CacheStatus = DirectDirty then PK_Impacted_Entity. Else 0
        /// </returns>
        public Tuple<Int64, CacheStatus, Int64> GetEntityCacheStatus(Int64 entityId, Int32 containerId, String entityTreeIdList, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("ImpactedEntityBL.CheckEntityCacheStatus", false);

            Tuple<Int64, CacheStatus, Int64> entityCacheStatus = null;

            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

            ImpactedEntityDA impactedEntityDA = new ImpactedEntityDA();
            entityCacheStatus = impactedEntityDA.GetEntityCacheStatus(entityId, containerId, entityTreeIdList, command);

            if (entityCacheStatus != null)
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "EntityId = " + entityId + " : CacheStatus = " + entityCacheStatus.Item2.ToString() + " : ReferenceId = " + entityCacheStatus.Item3.ToString(), MDMTraceSource.Entity);
            }
            else
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "Failed to get cache status for entity id : " + entityId, MDMTraceSource.Entity);
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("ImpactedEntityBL.CheckEntityCacheStatus");

            return entityCacheStatus;
        }

        #endregion Cache status methods

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Get collection of impacted entity ids for given entity Ids.
        /// Collection of impacted entityIds will be returned by this API. This will not populated impacted entities in impacted entity table in database.
        /// </summary>
        /// <param name="entity">Entity for which impacted entities are to be fetched.</param>
        /// <param name="command"></param>
        /// <param name="impactType">Indicates what kind of impacted entities are to be fetched.</param>
        /// <returns>Collection of impacted entities for given entityId</returns>
        private EntityCollection GetImpactedEntities(Entity entity, DBCommandProperties command, ImpactType impactType)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("EntityManager.ImpactedEntityBL.GetImpactedEntities", false);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Get impacted Entity starting..", MDMTraceSource.Entity);
            }

            #region Validation

            if (entity == null)
            {
                throw new MDMOperationException("111816", "Entity cannot be null or empty.", "EntityManager.ImpactedEntityBL", String.Empty, "Get");
            }

            #endregion Validation

            Int64 impactedEntityCount = 0;
            EntityCollection impactedEntities = null;

            var entityChangeContext = (EntityChangeContext)entity.GetChangeContext();

            var entityActivityLog = new EntityActivityLog
            {
                EntityId = entity.Id,
                AttributeIdList = entityChangeContext.AttributeIdList,
                AttributeLocaleIdList = entityChangeContext.AttributeLocaleList,
                PerformedAction = EntityActivityLogBL.GetAction(entity.Action)
            };

            switch (impactType)
            {
                case ImpactType.InheritedAttributes:
                    // Get all children
                    impactedEntities = _impactedEntityDA.LoadImpactedEntities(entityActivityLog, impactType, out impactedEntityCount, command, String.Empty, true, true, true, false, false, false, false, true);
                    break;
                case ImpactType.Relationships:
                    // Get all impacted relationships
                    throw new NotImplementedException("Relationship impacted get is not implemented.");
                case ImpactType.HierarchyRelationships:
                    // Get all parents. Check with sagesh and update. This should ideally be only MDL parents
                    impactedEntities = _impactedEntityDA.LoadImpactedEntities(entityActivityLog, impactType, out impactedEntityCount, command, String.Empty, false, false, false, true, true, true, true, true);
                    break;
                case ImpactType.ExtensionRelationships:
                    // Get all parents. Check with sagesh and update. This should ideally be only EH parents
                    impactedEntities = _impactedEntityDA.LoadImpactedEntities(entityActivityLog, impactType, out impactedEntityCount, command, String.Empty, false, false, false, true, true, true, true, true);
                    break;
                case ImpactType.All:
                    // for all
                    impactedEntities = _impactedEntityDA.LoadImpactedEntities(entityActivityLog, impactType, out impactedEntityCount, command, String.Empty, true, true, true, true, true, true, true, true);
                    break;
            }

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Get impacted Entity end..", MDMTraceSource.Entity);
                MDMTraceHelper.StopTraceActivity("EntityManager.ImpactedEntityBL.GetImpactedEntities");
            }

            return impactedEntities;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityActivityLog"></param>
        /// <param name="callerContext"></param>
        private void PopulateRelationshipProcessMode(EntityActivityLog entityActivityLog, CallerContext callerContext)
        {
            //Get entity details 
            EntityContext entityContext = new EntityContext();
            entityContext.LoadEntityProperties = true;
            entityContext.ContainerId = entityActivityLog.ContainerId;

            EntityBL entityManager = new EntityBL();
            Entity entity = entityManager.Get(entityActivityLog.EntityId, entityContext, callerContext.Application, callerContext.Module, false, false);

            if (entity != null)
            {
                RelationshipDenormBL reationshipDenormManager = new RelationshipDenormBL();
                RelationshipDenormProcessingSetting relDenormProcessingsetting = reationshipDenormManager.GetRelationshipDenormProcessingSettingsForContext(entity.OrganizationId, entity.ContainerId, entity.CategoryId, entity.EntityTypeId);

                if (relDenormProcessingsetting != null)
                {
                    RelationshipDenormAction relDenormAction = relDenormProcessingsetting.RelationshipDenormActions.FirstOrDefault(p => p.Action == entityActivityLog.PerformedAction);

                    if (relDenormAction != null)
                    {
                        Int32 relationshipProcessingMode = 0;

                        if (relDenormAction.WhereUsedProcessingMode == ProcessingMode.Async)
                        {
                            relationshipProcessingMode += 1;    //Adding 0th index weightage(1) in decimal for a Binary representation 
                        }

                        if (relDenormAction.HierarchyProcessingMode == ProcessingMode.Async)
                        {
                            relationshipProcessingMode += 2;    //Adding 1st index weightage(2) in decimal for a Binary representation
                        }

                        if (relDenormAction.ExtensionProcessingMode == ProcessingMode.Async)
                        {
                            relationshipProcessingMode += 4;    //Adding 2nd index weightage(4) in decimal for a Binary representation
                        }

                        if (relDenormAction.RelationshipTreeProcessingMode == ProcessingMode.Async)
                        {
                            relationshipProcessingMode += 8;    //Adding 3rd index weightage(8) in decimal for a Binary representation
                        }

                        //Populate relationship processing mode into activity log
                        entityActivityLog.RelationshipProcessMode = relationshipProcessingMode;
                    }
                }
            }
        }

        #endregion

        #endregion
    }
}