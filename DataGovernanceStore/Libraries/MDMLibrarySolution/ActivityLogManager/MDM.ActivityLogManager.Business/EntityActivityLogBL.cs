using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace MDM.ActivityLogManager.Business
{
    using MDM.ActivityLogManager.Data;
    using MDM.BusinessObjects;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.MessageManager.Business;
    using MDM.Utility;

    /// <summary>
    /// Specifies Entity Activity Log
    /// </summary>
    public class EntityActivityLogBL : BusinessLogicBase
    {
        #region Constants

        private const String GetEntityActivityLogStatusProcessName = "EntityActivityLogBL.GetEntityActivityLogStatus";
        private const String GetAllEntityActivityLogStatusProcessName = "EntityActivityLogBL.GetAllEntityActivityLogStatus";

        #endregion

        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private EntityActivityLogDA _entityActivityLogDA = new EntityActivityLogDA();

        /// <summary>
        /// Field denoting localeMessageBL
        /// </summary>
        private LocaleMessageBL _localeMessageBL = new LocaleMessageBL();

        /// <summary>
        /// Field denoting security principal
        /// </summary>
        private SecurityPrincipal _securityPrincipal = null;      

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public EntityActivityLogBL()
        {
            //GetSecurityPrincipal();
        }

        #endregion Constructors

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gets EntityActivityLog Item Status for specified entity activity log id
        /// </summary>
        /// <param name="entityActivityLogId">Entity Activity Log record Id</param>
        /// <param name="callerContext">Context details of caller</param>        
        public EntityActivityLogItemStatus GetEntityActivityLogStatus(Int64 entityActivityLogId, CallerContext callerContext)
        {
            EntityActivityLogItemStatus result = null;            

            MDMTraceHelper.StartTraceActivity(GetEntityActivityLogStatusProcessName, false);

            ValidateContext(callerContext, "GetEntityActivityLogStatus");

            try
            {                                                                  
                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

                EntityActivityLogDA entityActivityLogDA = new EntityActivityLogDA();

                result = entityActivityLogDA.GetEntityActivityLogStatus(entityActivityLogId, command);
            }
            finally
            {
                MDMTraceHelper.StopTraceActivity(GetEntityActivityLogStatusProcessName);
            }

            return result;
        }

        /// <summary>
        /// Gets all entity activity log with DQM validations for the current user
        /// If the user is admin it returns entity activity log status data for all users
        /// If the user is not admin it returns entity activity log status data for that user
        /// </summary>
        /// <param name="callerContext">Context details of caller</param>        
        public EntityActivityLogItemStatusCollection GetAllEntityActivityLogStatus(CallerContext callerContext)
        {
            EntityActivityLogItemStatusCollection result = null;
            
            //MDMTraceHelper.StartTraceActivity(GetAllEntityActivityLogStatusProcessName, MDMTraceHelper.IsEventTypeEnabledForSource(TraceEventType.Information, MDMTraceSource.DQM));

            ValidateContext(callerContext, "GetAllEntityActivityLogStatus");

            try
            {
                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

                EntityActivityLogDA entityActivityLogDA = new EntityActivityLogDA();

                result = entityActivityLogDA.GetAllEntityActivityLogStatus(command, SecurityPrincipal.CurrentUserName);
            }
            finally
            {
                //MDMTraceHelper.StopTraceActivity(GetAllEntityActivityLogStatusProcessName);
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="entityProcessingOptions"></param>
        /// <param name="callerContext"></param>
        /// <param name="writeEntityRelationshipChangeLog"></param>
        /// <returns></returns>
        public Boolean Process(EntityCollection entities, EntityProcessingOptions entityProcessingOptions, CallerContext callerContext, Boolean writeEntityRelationshipChangeLog = true)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
            {
                MDMTraceHelper.StartTraceActivity(MDMTraceSource.EntityProcess);
            }

            try
            {
                Boolean successFlag = true;
                Boolean writeEntityRelationshipCreateLog = false;
                Boolean writeEntityRelationshipUpdateLog = false;
                Boolean writeEntityRelationshipDeleteLog = false;
                Boolean writeEntityRelationshipAttributeUpdateLog = false;

                //If it is initial load, just return from the method
                if (entityProcessingOptions.ImportMode == ImportMode.InitialLoad || entityProcessingOptions.ImportMode == ImportMode.RelationshipInitialLoad)
                    return true;

                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Update);

                #region Read App Configs

                Boolean writeEntityCreateLog = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.EntityActivityLog.WriteEntityCreateLog", true);
                Boolean writeEntityUpdateLog = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.EntityActivityLog.WriteEntityUpdateLog", true);
                Boolean writeEntityAttributeUpdateLog = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.EntityActivityLog.WriteEntityAttributeUpdateLog", true);
                Boolean writeEntityHierarchyUpdateLog = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.EntityActivityLog.WriteEntityHierarchyUpdateLog", true);
                Boolean writeEntityExtensionUpdateLog = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.EntityActivityLog.WriteEntityExtensionUpdateLog", true);
                Boolean writeEntityAsyncCreateLog = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.EntityActivityLog.WriteEntityAsyncCreateLog", false);
                Boolean writeEntityAsyncUpdateLog = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.EntityActivityLog.WriteEntityAsyncUpdateLog", false);
                Boolean writeEntityAsyncDeleteLog = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.EntityActivityLog.WriteEntityAsyncDeleteLog", false);

                if (writeEntityRelationshipChangeLog)
                {
                    writeEntityRelationshipCreateLog = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.EntityActivityLog.WriteEntityRelationshipCreateLog", true);
                    writeEntityRelationshipUpdateLog = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.EntityActivityLog.WriteEntityRelationshipUpdateLog", true);
                    writeEntityRelationshipDeleteLog = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.EntityActivityLog.WriteEntityRelationshipDeleteLog", true);
                    writeEntityRelationshipAttributeUpdateLog = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.EntityActivityLog.WriteEntityRelationshipAttributeUpdateLog", true);
                }

                #endregion

                // If all keys are off then no logic to run...just return
                if (!writeEntityCreateLog && !writeEntityAttributeUpdateLog && !writeEntityHierarchyUpdateLog && !writeEntityExtensionUpdateLog)
                {
                    return true;
                }

                #region  Get containers which have been subscribed for the Async Processor, Entity Hierarchy Processor and Extension processor

                Boolean asyncProcessorFilterByContainerIdList = false;
                Collection<Int32> asyncProcessorContainerIdList = new Collection<Int32>();

                if (writeEntityAsyncCreateLog || writeEntityAsyncUpdateLog || writeEntityAsyncDeleteLog)
                {
                    String strContainerIdListForAsyncProcess = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.ParallelProcessingEngine.EntityAsyncProcessor.ContainerIdList", String.Empty);
                    if (!String.IsNullOrWhiteSpace(strContainerIdListForAsyncProcess))
                    {
                        asyncProcessorContainerIdList = ValueTypeHelper.SplitStringToIntCollection(strContainerIdListForAsyncProcess, '|');
                        asyncProcessorFilterByContainerIdList = asyncProcessorContainerIdList.Count > 0 ? true : false;
                    }
                }

                Boolean asyncProcessorFilterByChangedAttributeIdList = false;
                Collection<Int32> asyncProcessorAttributeIdList = new Collection<Int32>();

                if (writeEntityAsyncCreateLog || writeEntityAsyncUpdateLog || writeEntityAsyncDeleteLog)
                {
                    String strAttributeIdListForAsyncProcess = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.ParallelProcessingEngine.EntityAsyncProcessor.FilterByChangeContext.AttributeIdList", String.Empty);
                    if (!String.IsNullOrWhiteSpace(strAttributeIdListForAsyncProcess))
                    {
                        asyncProcessorAttributeIdList = ValueTypeHelper.SplitStringToIntCollection(strAttributeIdListForAsyncProcess, '|');
                        asyncProcessorFilterByChangedAttributeIdList = asyncProcessorAttributeIdList.Count > 0 ? true : false;
                    }
                }

                Boolean hierarchyProcessorFilterByContainerIdList = false;
                Collection<Int32> hierarchyProcessorContainerIdList = new Collection<Int32>();

                if (writeEntityHierarchyUpdateLog)
                {
                    String strContainerIdListForHierarchyRollup = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.ParallelProcessingEngine.EntityHierarchyProcessor.RollupProcessing.ContainerIdList", String.Empty);

                    if (!String.IsNullOrWhiteSpace(strContainerIdListForHierarchyRollup))
                    {
                        hierarchyProcessorContainerIdList = ValueTypeHelper.SplitStringToIntCollection(strContainerIdListForHierarchyRollup, '|');
                        hierarchyProcessorFilterByContainerIdList = hierarchyProcessorContainerIdList.Count > 0 ? true : false;
                    }
                }

                #endregion  Get containers which have subscribed for the Async Processor

                EntityActivityLogCollection entityActivityLogCollection = new EntityActivityLogCollection();
                EntityActivityLogBL entityActivityLogManager = new EntityActivityLogBL();

                foreach (Entity entity in entities)
                {
                    if (entity.Action == ObjectAction.Create
                        || entity.Action == ObjectAction.Update
                        || entity.Action == ObjectAction.Reclassify
                        || entity.Action == ObjectAction.ReParent
                        || entity.Action == ObjectAction.Rename
                        || entity.Action == ObjectAction.Delete)
                    {
                        EntityChangeContext entityChangeContext = (EntityChangeContext)entity.GetChangeContext();

                        #region Adding Entity Activity Log for Entity create

                        if (writeEntityCreateLog && entity.Action == ObjectAction.Create)
                        {
                            EntityActivityLog entityActivityLog = this.CreateEntityActivityLogInstance(entity, EntityActivityList.EntityCreate);

                            entityActivityLog.AttributeIdList = new Collection<Int32>();
                            entityActivityLog.AttributeLocaleIdList = new Collection<LocaleEnum>();

                            entityActivityLogCollection.Add(entityActivityLog);
                        }

                        if (writeEntityAsyncCreateLog && entity.Action == ObjectAction.Create)
                        {
                            if (!asyncProcessorFilterByContainerIdList || asyncProcessorContainerIdList.Contains(entity.ContainerId))
                            {
                                if (!asyncProcessorFilterByChangedAttributeIdList || entityChangeContext.AttributeIdList.Any(i => asyncProcessorAttributeIdList.Contains(i)))
                                {
                                    EntityActivityLog entityActivityLog = this.CreateEntityActivityLogInstance(entity, EntityActivityList.EntityAsyncCreate);

                                    entityActivityLog.AttributeIdList = new Collection<Int32>();
                                    entityActivityLog.AttributeLocaleIdList = new Collection<LocaleEnum>();

                                    entityActivityLogCollection.Add(entityActivityLog);
                                }
                            }
                        }

                        #endregion

                        #region Adding Entity Activity Log for Attribute Updates

                        if (writeEntityAttributeUpdateLog && entityChangeContext.IsAttributesChanged && entity.Action == ObjectAction.Update)
                        {
                            // if its workflow attribute update it should have to send EntityActivityList.WorkflowAttributeUpdate
                            // then only denorm will happen for workflow attributes.

                            Boolean workflowAttributeUpdate = false;
                            Boolean attributeUpdate = false;

                            if (entityChangeContext.AttributeIdList != null)
                            {
                                foreach (var attributeId in entityChangeContext.AttributeIdList)
                                {
                                    if (attributeId > 90 && attributeId < 101)
                                    {
                                        workflowAttributeUpdate = true;
                                    }
                                    else
                                    {
                                        attributeUpdate = true;
                                    }

                                    if (workflowAttributeUpdate == true && attributeUpdate == true)
                                        break;
                                }
                            }

                            if (workflowAttributeUpdate)
                            {
                                EntityActivityLog entityActivityLog = this.CreateEntityActivityLogInstance(entity, EntityActivityList.WorkflowAttributeUpdate);
                                entityActivityLogCollection.Add(entityActivityLog);
                            }

                            if (attributeUpdate)
                            {
                                EntityActivityLog entityActivityLog = this.CreateEntityActivityLogInstance(entity, EntityActivityList.AttributeUpdate);

                                if (entity.EntityTypeId == Constants.CATEGORY_ENTITYTYPE)
                                {
                                    EntityFamilyChangeContext entityFamilyChangeContext = new EntityFamilyChangeContext(entity.EntityFamilyId, entity.EntityGlobalFamilyId, entity.OrganizationId, entity.ContainerId);
                                    entityFamilyChangeContext.VariantsChangeContext.EntityChangeContexts.Add(entityChangeContext);

                                    entityActivityLog.Context= entityFamilyChangeContext.ToXml();
                                }
                                
                                entityActivityLogCollection.Add(entityActivityLog);
                            }
                        }

                        if (writeEntityAsyncUpdateLog && entity.Action == ObjectAction.Update)
                        {
                            if (!asyncProcessorFilterByContainerIdList || asyncProcessorContainerIdList.Contains(entity.ContainerId))
                            {
                                if (!asyncProcessorFilterByChangedAttributeIdList || entityChangeContext.AttributeIdList.Any(i => asyncProcessorAttributeIdList.Contains(i)))
                                {
                                    if (entityChangeContext.IsAttributesChanged)
                                    {
                                        EntityActivityLog entityActivityLog = this.CreateEntityActivityLogInstance(entity, EntityActivityList.EntityAsyncUpdate, true);
                                        entityActivityLogCollection.Add(entityActivityLog);
                                    }
                                    else if (!entity.IsDirectChange && entity.RelatedEntityChangeContext != null && entity.RelatedEntityChangeContext.AttributeIdList != null && entity.RelatedEntityChangeContext.AttributeIdList.Count > 0)
                                    {
                                        EntityActivityLog entityActivityLog = this.CreateEntityActivityLogInstance(entity, EntityActivityList.EntityAsyncUpdate, true, false);
                                        entityActivityLogCollection.Add(entityActivityLog);
                                    }
                                }
                            }
                        }

                        #endregion

                        #region Adding Entity Activity Log for Relationships Change

                        if ((entity.Action == ObjectAction.Create || entity.Action == ObjectAction.Update) && writeEntityRelationshipChangeLog)
                        {
                            EntityActivityLog entityRelationshipActivityLog = null;

                            if (writeEntityRelationshipCreateLog && entityChangeContext.IsRelationshipsCreated)
                            {
                                entityRelationshipActivityLog = this.CreateEntityActivityLogInstance(entity, EntityActivityList.RelationshipCreate);
                            }
                            else if (writeEntityRelationshipAttributeUpdateLog && entityChangeContext.IsRelationshipAttributesChanged)
                            {
                                entityRelationshipActivityLog = this.CreateEntityActivityLogInstance(entity, EntityActivityList.RelationshipAttributeUpdate);
                            }

                            //Add Activity Log into collection
                            if (entityRelationshipActivityLog != null)
                            {
                                entityActivityLogCollection.Add(entityRelationshipActivityLog);
                            }

                            if (writeEntityRelationshipUpdateLog && entityChangeContext.IsRelationshipsUpdated)
                            {
                                entityRelationshipActivityLog = this.CreateEntityActivityLogInstance(entity, EntityActivityList.RelationshipUpdate);

                                //Add Activity Log into collection
                                if (entityRelationshipActivityLog != null)
                                {
                                    entityActivityLogCollection.Add(entityRelationshipActivityLog);
                                }
                            }

                            if (writeEntityRelationshipDeleteLog && entityChangeContext.IsRelationshipsDeleted)
                            {
                                entityRelationshipActivityLog = this.CreateEntityActivityLogInstance(entity, EntityActivityList.RelationshipDelete);

                                //Add Activity Log into collection
                                if (entityRelationshipActivityLog != null)
                                {
                                    entityActivityLogCollection.Add(entityRelationshipActivityLog);
                                }
                            }
                        }

                        #endregion

                        #region Adding Entity Activity Async Log for Entity Delete

                        if (writeEntityAsyncDeleteLog && entity.Action == ObjectAction.Delete && entityProcessingOptions.ProcessingMode != ProcessingMode.AsyncDelete)
                        {
                            if (!asyncProcessorFilterByContainerIdList || asyncProcessorContainerIdList.Contains(entity.ContainerId))
                            {
                                EntityActivityLog entityActivityLog = this.CreateEntityActivityLogInstance(entity, EntityActivityList.EntityAsyncDelete);

                                entityActivityLog.AttributeIdList = new Collection<int>();
                                entityActivityLog.AttributeLocaleIdList = new Collection<LocaleEnum>();

                                entityActivityLogCollection.Add(entityActivityLog);
                            }
                        }

                        #endregion

                        #region Adding Entity Activity Log for Entity Reclassify

                        if (writeEntityAttributeUpdateLog && entity.Action == ObjectAction.Reclassify)
                        {
                            EntityActivityLog entityActivityLog = this.CreateEntityActivityLogInstance(entity, EntityActivityList.EntityReclassify);

                            entityActivityLog.AttributeIdList = new Collection<int>();
                            entityActivityLog.AttributeLocaleIdList = new Collection<LocaleEnum>();
                            if (entity.GetEntityMoveContext() != null)
                            {
                                entityActivityLog.Context = entity.GetEntityMoveContext().ToXml();
                            }

                            entityActivityLogCollection.Add(entityActivityLog);
                        }

                        #endregion

                        #region Adding Entity Activity Log for ReParent and Rename

                        if (writeEntityUpdateLog)
                        {
                            EntityActivityLog entityActivityLog = null;

                            if (entity.Action == ObjectAction.ReParent)
                            {
                                entityActivityLog = this.CreateEntityActivityLogInstance(entity, EntityActivityList.EntityReParent);
                            }
                            else if (entity.Action == ObjectAction.Rename)
                            {
                                entityActivityLog = this.CreateEntityActivityLogInstance(entity, EntityActivityList.EntityUpdate);
                            }

                            if (entityActivityLog != null)
                            {
                                entityActivityLog.AttributeIdList = new Collection<int>();
                                entityActivityLog.AttributeLocaleIdList = new Collection<LocaleEnum>();

                                entityActivityLogCollection.Add(entityActivityLog);
                            }
                        }

                        if (writeEntityAsyncUpdateLog && (entity.Action == ObjectAction.ReParent || entity.Action == ObjectAction.Rename))
                        {
                            EntityActivityLog entityActivityLog = this.CreateEntityActivityLogInstance(entity, EntityActivityList.EntityAsyncUpdate);

                            entityActivityLog.AttributeIdList = new Collection<int>();
                            entityActivityLog.AttributeLocaleIdList = new Collection<LocaleEnum>();

                            entityActivityLogCollection.Add(entityActivityLog);
                        }

                        #endregion

                        #region Adding Entity Activity Log for Entity Child Hierarchy

                        if (writeEntityHierarchyUpdateLog
                            && (entity.CategoryId != entity.ParentEntityId || (entity.Action == ObjectAction.ReParent && entity.EntityMoveContext.TargetParentEntityId > 0))
                            && entity.EntityTypeId != Constants.CATEGORY_ENTITYTYPE
                            && (entityChangeContext.IsAttributesChanged
                                    || entity.Action == ObjectAction.Create
                                    || entity.Action == ObjectAction.ReParent
                                    || (entity.HierarchyRelationships != null && entity.HierarchyRelationships.Action == ObjectAction.Update)
                                )
                            )
                        {
                            if (!hierarchyProcessorFilterByContainerIdList || hierarchyProcessorContainerIdList.Contains(entity.ContainerId))
                            {
                                EntityActivityLog entityActivityLog = this.CreateEntityActivityLogInstance(entity, EntityActivityList.ChildHierarchyChange);

                                //Append attribute changes happened at child hierarchy level...
                                if (entity.HierarchyRelationships != null && entity.HierarchyRelationships.ChangeContext != null && entity.HierarchyRelationships.ChangeContext.IsAttributesChanged)
                                {
                                    Collection<Int32> changedAttrIdList = entityActivityLog.AttributeIdList;

                                    foreach (Int32 attrId in entity.HierarchyRelationships.ChangeContext.AttributeIdList)
                                    {
                                        if (!changedAttrIdList.Contains(attrId))
                                            changedAttrIdList.Add(attrId);
                                    }

                                    entityActivityLog.AttributeIdList = changedAttrIdList;

                                    foreach (LocaleEnum locale in entity.HierarchyRelationships.ChangeContext.AttributeLocaleList)
                                    {
                                        if (!entityActivityLog.AttributeLocaleIdList.Contains(locale))
                                            entityActivityLog.AttributeLocaleIdList.Add(locale);
                                    }
                                }

                                entityActivityLogCollection.Add(entityActivityLog);
                            }
                        }

                        #endregion

                        #region Adding Entity Activity Log for Entity Child Extensions

                        // Create an activity log for extension change on the following condition.
                        // A re-parenting has happened and the entity got a new MDL parent. This could also mean the entity got the MDL parent first time.
                        // A new MDL child got added the existing entity.
                        if (writeEntityExtensionUpdateLog
                            && entity.EntityTypeId != Constants.CATEGORY_ENTITYTYPE
                            && (entity.ParentExtensionEntityId > 0 || (entity.Action == ObjectAction.ReParent && entity.EntityMoveContext.TargetParentExtensionEntityId > 0))
                            && (entityChangeContext.IsAttributesChanged
                                    || entity.Action == ObjectAction.Create
                                    || entity.Action == ObjectAction.ReParent
                                    || (entity.ExtensionRelationships != null && entity.ExtensionRelationships.Action == ObjectAction.Update)
                                )
                            )
                        {
                            EntityActivityLog entityActivityLog = this.CreateEntityActivityLogInstance(entity, EntityActivityList.ChildExtensionChange);

                            //Append attribute changes happened at child extension level...
                            if (entity.ExtensionRelationships != null && entity.ExtensionRelationships.ChangeContext != null && entity.ExtensionRelationships.ChangeContext.IsAttributesChanged)
                            {
                                Collection<Int32> changedAttrIdList = entityActivityLog.AttributeIdList;

                                foreach (Int32 attrId in entity.ExtensionRelationships.ChangeContext.AttributeIdList)
                                {
                                    if (!changedAttrIdList.Contains(attrId))
                                        changedAttrIdList.Add(attrId);
                                }

                                entityActivityLog.AttributeIdList = changedAttrIdList;

                                foreach (LocaleEnum locale in entity.ExtensionRelationships.ChangeContext.AttributeLocaleList)
                                {
                                    if (!entityActivityLog.AttributeLocaleIdList.Contains(locale))
                                        entityActivityLog.AttributeLocaleIdList.Add(locale);
                                }
                            }

                            entityActivityLogCollection.Add(entityActivityLog);
                        }

                        #endregion
                    }
                }

                if (entityActivityLogCollection.Count > 0)
                {
                    entityActivityLogManager.Process(entityActivityLogCollection, callerContext, entityProcessingOptions.ProcessingMode);
                }

                return successFlag;
            }
            finally
            {
                if (isTracingEnabled)
                {
                    MDMTraceHelper.StopTraceActivity();
                }
            }
        }

        /// <summary>
        ///  Processes the impacted entity log
        /// </summary>
        /// <param name="entityActivityLogCollection">Collection of impacted entity log to be processed</param>
        /// <param name="callerContext">Context indicating who called the API</param>
        /// <param name="processingMode">Mode of processing (Sync or Async)</param>
        /// <returns></returns>
        public Boolean Process(EntityActivityLogCollection entityActivityLogCollection, CallerContext callerContext, ProcessingMode processingMode = ProcessingMode.Sync)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;

            if (isTracingEnabled)
            MDMTraceHelper.StartTraceActivity("EntityManager.ImpactedEntityBL.ProcessEntityActivityLog", false);

            try
            {
            #region validations

            ValidateContext(callerContext, "Process");

            if (entityActivityLogCollection == null || entityActivityLogCollection.Count <= 0)
            {
                throw new MDMOperationException("111877", "EntityActivityLogCollection cannot be null or empty.", "EntityManager.ImpactedEntityBL", String.Empty, "ProcessEntityActivityLog");
            }

            #endregion validations

                if (isTracingEnabled)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Starting Impacted Entity Log Process...");
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "No. of entities for which impacted entities are to be loaded : " + entityActivityLogCollection.Count);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "CallerContext : Application - " + callerContext.Application + " Module - " + callerContext.Module);
            }

            String loginUser = SecurityPrincipal.CurrentUserName;
                        
            // Get command
            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Update);

            Boolean result = _entityActivityLogDA.Process(entityActivityLogCollection, loginUser, callerContext, command, processingMode);

            return result;
        }
            finally
            {
                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("EntityManager.ImpactedEntityBL.ProcessEntityActivityLog");
            }
        }

        /// <summary>
        /// Gets all the entity activity logs based on the log status
        /// * LogType.Current -> get all the records from tb_EntityActivityLog with IsLoaded = true, IsProcessed = 0
        /// * LogType.Past -> get all the records from tb_EntityActivityLog_HS
        /// * LogType.Pending -> get all the records from tb_EntityActivityLog with IsLoaded = false, IsProcessed = 0
        /// </summary>
        /// <param name="entityActivityList">Indicates the entity activity list type</param>
        /// <param name="processingStatus">Indicates the processing status type like LogType.Current,LogType.Past,LogType.Pending</param>
        /// <param name="fromRecordNumber">Indicates the starting index of record to be fetched</param>
        /// <param name="toRecordNumber">Indicates the end index of record to be fetched</param>
        /// <param name="callerContext">Indicates the caller context specifying the caller application and module details.</param>
        /// <returns>Returns collection of entity activity log based on the given context</returns>
        public EntityActivityLogCollection Get(EntityActivityList entityActivityList, ProcessingStatus processingStatus, Int64 fromRecordNumber, Int64 toRecordNumber, CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;

            try
            {
                if (isTracingEnabled)
            MDMTraceHelper.StartTraceActivity("EntityManager.EntityActivityLogBL.Get", false);

            #region validations

            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "EntityManager.ImpactedEntityBL", String.Empty, "GetImpactedLogs");
            }

            #endregion validations

            // Get command
            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);
            EntityActivityLogCollection entityActivityLogCollection = _entityActivityLogDA.Get(entityActivityList, processingStatus, fromRecordNumber, toRecordNumber, command);

                return entityActivityLogCollection;
            }
            finally
            {
                if (isTracingEnabled)
            {
            MDMTraceHelper.StopTraceActivity("EntityManager.EntityActivityLogBL.Get");
            }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static EntityActivityList GetAction(ObjectAction action)
        {
            EntityActivityList activityAction = EntityActivityList.UnKnown;

            switch (action)
            {
                case ObjectAction.Create:
                    activityAction = EntityActivityList.EntityCreate;
                    break;
                case ObjectAction.Rename:
                case ObjectAction.ReParent:
                    activityAction = EntityActivityList.EntityUpdate;
                    break;
                case ObjectAction.Delete:
                    activityAction = EntityActivityList.EntityDelete;
                    break;
                case ObjectAction.Update:
                    activityAction = EntityActivityList.AttributeUpdate;
                    break;
                case ObjectAction.Reclassify:
                    activityAction = EntityActivityList.EntityReclassify;
                    break;
                default:
                    activityAction = EntityActivityList.AttributeUpdate;
                    break;
            }

            return activityAction;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="performedAction"></param>
        /// <returns></returns>
        public static ObjectAction GetAction(EntityActivityList performedAction)
        {
            ObjectAction action = ObjectAction.Update;

            switch (performedAction)
            {
                case EntityActivityList.EntityCreate:
                    action = ObjectAction.Create;
                    break;
                case EntityActivityList.EntityUpdate:
                case EntityActivityList.AttributeUpdate:
                    action = ObjectAction.Update;
                    break;
                case EntityActivityList.EntityDelete:
                    action = ObjectAction.Delete;
                    break;
                case EntityActivityList.EntityReclassify:
                    action = ObjectAction.Reclassify;
                    break;
                default:
                    break;
            }

            return action;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityActivity"></param>
        /// <param name="includeRelatedEntityChangeContext"></param>
        /// <param name="isDirectChange"></param>
        /// <returns></returns>
        private EntityActivityLog CreateEntityActivityLogInstance(Entity entity, EntityActivityList entityActivity, Boolean includeRelatedEntityChangeContext = false, Boolean isDirectChange = true)
        {
            EntityChangeContext entityChangeContext = (EntityChangeContext) entity.GetChangeContext();

            EntityActivityLog entityActivityLog = new EntityActivityLog();

            Collection<Int32> attributeIdList = entityChangeContext.AttributeIdList;
            Collection<LocaleEnum> attributeLocaleList = entityChangeContext.AttributeLocaleList;

            if (includeRelatedEntityChangeContext && entity.RelatedEntityChangeContext != null)
            {
                //Include related entity attribute change context
                EntityChangeContext relatedEntityChangeContext = entity.RelatedEntityChangeContext;

                if (relatedEntityChangeContext.AttributeIdList != null && relatedEntityChangeContext.AttributeIdList.Count > 0)
                {
                    if (attributeIdList == null)
                    {
                        attributeIdList = relatedEntityChangeContext.AttributeIdList;
                    }
                    else
                    {
                        attributeIdList = ValueTypeHelper.MergeCollections(attributeIdList, relatedEntityChangeContext.AttributeIdList);
                    }
                }

                if (relatedEntityChangeContext.AttributeLocaleList != null && relatedEntityChangeContext.AttributeLocaleList.Count > 0)
                {
                    if (attributeLocaleList == null)
                    {
                        attributeLocaleList = relatedEntityChangeContext.AttributeLocaleList;
                    }
                    else
                    {
                        attributeLocaleList = ValueTypeHelper.MergeCollections(attributeLocaleList, relatedEntityChangeContext.AttributeLocaleList);
                    }
                }
            }

            if (attributeIdList == null)
            {
                attributeIdList = new Collection<Int32>();
            }

            if (attributeIdList.Count < 1)
            {
                //Attribute id = 0, means no attributes...
                attributeIdList.Add(0);
                attributeLocaleList = new Collection<LocaleEnum>() {LocaleEnum.UnKnown};
            }

            entityActivityLog.EntityId = entity.Id;
            entityActivityLog.ContainerId = entity.ContainerId;
            entityActivityLog.AttributeIdList = attributeIdList;
            entityActivityLog.AttributeLocaleIdList = attributeLocaleList;
            entityActivityLog.RelationshipIdList = entityChangeContext.RelationshipIdList;
            entityActivityLog.PerformedAction = entityActivity;
            entityActivityLog.AuditRefId = entity.AuditRefId;
            entityActivityLog.ParentEntityActivityLogId = entity.ActivityLogId;
            entityActivityLog.IsDirectChange = isDirectChange;

            return entityActivityLog;
        }       

        private void ValidateContext(CallerContext callerContext, String methodName)
        {
            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "EntityManager.EntityActivityLogBL." + methodName, String.Empty, methodName);
            }
        }

        #region Properties

        private SecurityPrincipal SecurityPrincipal
        {
            get
            {
                try
                {
                    if (_securityPrincipal == null)
                    {
                        _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
                    }

                    return _securityPrincipal;
                }
                catch
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "Unable to fetch user login");
                }
                return null;
            }
        }

        #endregion

        #endregion

        #endregion Methods
    }
}