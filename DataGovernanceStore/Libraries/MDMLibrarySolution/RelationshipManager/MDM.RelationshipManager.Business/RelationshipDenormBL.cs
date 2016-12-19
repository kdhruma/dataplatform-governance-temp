using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace MDM.RelationshipManager.Business
{
    using MDM.AttributeModelManager.Business;
    using MDM.BusinessObjects;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.EntityProcessorManager.Business;
    using MDM.Interfaces;
    using MDM.MessageManager.Business;
    using MDM.RelationshipManager.Data;
    using MDM.Utility;

    /// <summary>
    /// Specifies business logic operations for Relationship Denorm
    /// </summary>
    public class RelationshipDenormBL : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// Specifies security principal for user.
        /// </summary>
        private SecurityPrincipal _securityPrincipal = null;

        /// <summary>
        /// Field denoting locale Message
        /// </summary>
        private LocaleMessage _localeMessage = null;

        /// <summary>
        /// Field denoting localeMessageBL
        /// </summary>
        private LocaleMessageBL _localeMessageBL = null;

        /// <summary>
        /// Field denoting system UI locale.
        /// </summary>
        private LocaleEnum _systemUILocale = LocaleEnum.UnKnown;

        /// <summary>
        /// Field denoting system Data locale.
        /// </summary>
        private LocaleEnum _systemDataLocale = LocaleEnum.UnKnown;

        #endregion

        #region Properties
        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor that loads the security principal from Cache if present
        /// </summary>
        public RelationshipDenormBL()
        {
            GetSecurityPrincipal();
            InitalizeMembers();
        }

        /// <summary>
        /// Instantiates Relationship Business Logic with provided Security Principal
        /// </summary>
        /// <param name="securityPrinicipal">Security Principal</param>
        public RelationshipDenormBL(SecurityPrincipal securityPrinicipal)
        {
            this._securityPrincipal = securityPrinicipal;
            InitalizeMembers();
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gets entities denormalized relationships
        /// </summary>
        /// <param name="entityContainerIds">Entity Ids and Container Ids dictionary for which denormalized relationships are required</param>
        /// <param name="relationshipId">Relationship Id for which denormalized relationships are required</param>
        /// <param name="relationshipTypeId">Relationship Type Id for which denormalized relationships are required</param>
        /// <param name="getWhereUsed">Flag denoting whether to consider where used relationships in this get operation</param>
        /// <param name="getImpactedExtensions">Flag denoting whether to consider impacted extension relationships in this get operation</param>
        /// <param name="getImpactedHierarchies">Flag denoting whether to consider impacted hierarchies relationships in this get operation</param>
        /// <param name="getRelationshipTree">Flag denoting whether to consider relationships tree in this get operation</param>
        /// <param name="getInheritable">Flag denoting whether to consider inheritable relationships in this get operation</param>
        /// <param name="loadRelationshipAttributes">Flag denoting whether to load relationship attributes for denormalized relationships</param>
        /// <param name="callerContext">Indicates Application and Module name by which operation is being performed</param>
        /// <returns>Denormalized relationships</returns>
        public RelationshipCollection GetDenormalizedRelationships(Dictionary<Int64, Int32> entityContainerIds, Int64 relationshipId, Int32 relationshipTypeId, Boolean getWhereUsed, Boolean getImpactedExtensions, Boolean getImpactedHierarchies, Boolean getRelationshipTree, Boolean getInheritable, Boolean loadRelationshipAttributes, CallerContext callerContext)
        {
            RelationshipCollection denormalizedRelationships = null;

            denormalizedRelationships = GetEntitiesDenormalizedRelationships(entityContainerIds, relationshipId, relationshipTypeId, getWhereUsed, getImpactedExtensions, getImpactedHierarchies, getRelationshipTree, getInheritable, loadRelationshipAttributes, callerContext);

            return denormalizedRelationships;
        }

        /// <summary>
        /// Gets entity's denormalized relationships
        /// </summary>
        /// <param name="entityId">Id of the entity</param>
        /// <param name="containerId">Container Id of the entity</param>
        /// <param name="relationshipId">Relationship Id for which denormalized relationships are required</param>
        /// <param name="relationshipTypeId">Relationship Type Id for which denormalized relationships are required</param>
        /// <param name="getWhereUsed">Flag denoting whether to consider where used relationships in this get operation</param>
        /// <param name="getImpactedExtensions">Flag denoting whether to consider impacted extension relationships in this get operation</param>
        /// <param name="getImpactedHierarchies">Flag denoting whether to consider impacted hierarchies relationships in this get operation</param>
        /// <param name="getRelationshipTree">Flag denoting whether to consider relationships tree in this get operation</param>
        /// <param name="getInheritable">Flag denoting whether to consider inheritable relationships in this get operation</param>
        /// <param name="loadRelationshipAttributes">Flag denoting whether to load relationship attributes for denormalized relationships</param>
        /// <param name="callerContext">Indicates Application and Module name by which operation is being performed</param>
        /// <returns>Denormalized relationships</returns>
        public RelationshipCollection GetDenormalizedRelationships(Int64 entityId, Int32 containerId, Int64 relationshipId, Int32 relationshipTypeId, Boolean getWhereUsed, Boolean getImpactedExtensions, Boolean getImpactedHierarchies, Boolean getRelationshipTree, Boolean getInheritable, Boolean loadRelationshipAttributes, CallerContext callerContext)
        {
            RelationshipCollection denormalizedRelationships = null;

            Dictionary<Int64, Int32> entityContainerIds = new Dictionary<Int64, Int32>();
            entityContainerIds.Add(entityId, containerId);

            denormalizedRelationships = GetEntitiesDenormalizedRelationships(entityContainerIds, relationshipId, relationshipTypeId,  getWhereUsed, getImpactedExtensions, getImpactedHierarchies, getRelationshipTree, getInheritable, loadRelationshipAttributes, callerContext);

            return denormalizedRelationships;
        }

        /// <summary>
        /// Processes denormalized relationships for the requested entity
        /// </summary>
        /// <param name="entity">Entity for which relationship needs to be denormalized</param>
        /// <param name="processWhereUsed">Flag denoting whether to consider where used relationships in this process operation</param>
        /// <param name="processImpactedExtensions">Flag denoting whether to consider impacted extension relationships in this process operation</param>
        /// <param name="processImpactedHierarchies">Flag denoting whether to consider impacted hierarchies relationships in this process operation</param>
        /// <param name="processRelationshipTree">Flag denoting whether to consider relationships tree in this process operation</param>
        /// <param name="processInheritable">Flag denoting whether to consider inheritable relationships in this process operation</param>
        /// <param name="processRelationshipAttributes">Flag denoting whether to process relationship attributes for denormalized relationships</param>
        /// <param name="processingMode">Requested processing mode</param>
        /// <param name="iEntityManager">Entity Manager object</param>
        /// <param name="callerContext">Indicates Application and Module name by which operation is being performed</param>
        /// <returns>Entity operation result</returns>
        public EntityOperationResult ProcessDenormalizedRelationships(Entity entity, Boolean processWhereUsed, Boolean processImpactedExtensions, Boolean processImpactedHierarchies, Boolean processRelationshipTree, Boolean processInheritable, Boolean processRelationshipAttributes, ProcessingMode processingMode, IEntityManager iEntityManager, CallerContext callerContext)
        {
            EntityOperationResult entityOperationResult = null;

            EntityOperationResultCollection eorCollection = ProcessDenormalizedRelationships(new EntityCollection() { entity }, processWhereUsed, processImpactedExtensions, processImpactedHierarchies, processRelationshipTree, processInheritable, processRelationshipAttributes, processingMode, iEntityManager, callerContext);

            if (eorCollection != null && eorCollection.Count > 0)
            {
                entityOperationResult = eorCollection[entity.Id];
            }

            return entityOperationResult;
        }

        /// <summary>
        /// Processes denormalized relationships for the requested entities
        /// </summary>
        /// <param name="entities">Entities for which relationship needs to be denormalized</param>
        /// <param name="processWhereUsed">Flag denoting whether to consider where used relationships in this process operation</param>
        /// <param name="processImpactedExtensions">Flag denoting whether to consider impacted extension relationships in this process operation</param>
        /// <param name="processImpactedHierarchies">Flag denoting whether to consider impacted hierarchies relationships in this process operation</param>
        /// <param name="processRelationshipTree">Flag denoting whether to consider relationships tree in this process operation</param>
        /// <param name="processInheritable">Flag denoting whether to consider inheritable relationships in this process operation</param>
        /// <param name="processRelationshipAttributes">Flag denoting whether to process relationship attributes for denormalized relationships</param>
        /// <param name="processingMode">Requested processing mode</param>
        /// <param name="iEntityManager">Entity Manager object</param>
        /// <param name="callerContext">Indicates Application and Module name by which operation is being performed</param>
        /// <returns>Entity operation results</returns>
        public EntityOperationResultCollection ProcessDenormalizedRelationships(EntityCollection entities, Boolean processWhereUsed, Boolean processImpactedExtensions, Boolean processImpactedHierarchies, Boolean processRelationshipTree, Boolean processInheritable, Boolean processRelationshipAttributes, ProcessingMode processingMode, IEntityManager iEntityManager, CallerContext callerContext)
        {
            EntityOperationResultCollection eorCollection = new EntityOperationResultCollection();

            DurationHelper durationHelper = new DurationHelper(DateTime.Now);
            DurationHelper overallDurationHelper = new DurationHelper(DateTime.Now);

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("RelationshipManager.RelationshipDenormBL.ProcessRelationshipDenorm", MDMTraceSource.RelationshipProcess, false);
            }

            try
            {
                #region STEP: Validation and Initial Setup

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Starting parameter validation and initial setup for denorm process...", MDMTraceSource.RelationshipProcess);
                }

                #region Parameters Validation

                if (entities == null || entities.Count < 1)
                {
                    _localeMessage = _localeMessageBL.Get(_systemUILocale, "111816", false, callerContext);
                    throw new MDMOperationException("111816", _localeMessage.Message, "RelationshipManager.RelationshipDenormBL", String.Empty, "ProcessRelationshipDenorm");   //Entities collection is null or empty.
                }

                #endregion

                #region Initial Setup

                //Override caller context module to Denorm..
                callerContext.Module = MDMCenterModules.Denorm;

                //Get Relationship Id filter
                Int64 relationshipIdFilter = 0;

                EntityContext entityContext = entities.First().EntityContext;

                if (entityContext != null && entityContext.RelationshipContext != null)
                {
                    relationshipIdFilter = entityContext.RelationshipContext.RelationshipParentId;
                }

                RelationshipCollection denormRelsToBeProcessed = new RelationshipCollection();

                #endregion

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Done with parameter validation and initial setup for denorm process.", MDMTraceSource.RelationshipProcess);
                }

                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - parameter validation and initial setup for denorm process.", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.RelationshipProcess);
                }

                #endregion

                #region STEP: Get Denormalized relationships

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Getting denormalized relationships...", MDMTraceSource.RelationshipProcess);
                }

                foreach (Entity entity in entities)
                {
                    RelationshipCollection relationships = GetDenormalizedRelationships(entity.Id, entity.ContainerId, relationshipIdFilter, 0, processWhereUsed, processImpactedExtensions, processImpactedHierarchies, processRelationshipTree, processInheritable, processRelationshipAttributes, callerContext);

                    if (relationships != null)
                    {
                        Boolean considerRelationshipsForProcessing = true;

                        if (relationships.Count == 1)
                        {
                            Relationship firstRelationship = relationships.First();

                            if (firstRelationship.RelationshipProcessingContext.EntityId == entity.Id && (firstRelationship.RelationshipCollection == null || firstRelationship.RelationshipCollection.Count < 1))
                            {
                                considerRelationshipsForProcessing = false;
                            }
                        }

                        if (considerRelationshipsForProcessing)
                        {
                            foreach (Relationship rel in relationships)
                            {
                                denormRelsToBeProcessed.Add(rel);
                            }
                        }
                    }
                }

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Done with denormalized relationships get.", MDMTraceSource.RelationshipProcess);
                }

                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Denormalized relationship get for all updated Entity Relationships.", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.RelationshipProcess);
                }

                #endregion

                eorCollection = ProcessDenormalizedRelationships(entities, denormRelsToBeProcessed, processingMode, iEntityManager, durationHelper, callerContext);
            }
            finally
            {
                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Overall relationship denorm processing time", overallDurationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.RelationshipProcess);

                    MDMTraceHelper.StopTraceActivity("RelationshipManager.RelationshipDenormBL.ProcessRelationshipDenorm", MDMTraceSource.RelationshipProcess);
                }
            }

            return eorCollection;
        }

        /// <summary>
        /// Processes denormalized relationships for the requested entity
        /// </summary>
        /// <param name="entity">Entity for which relationship needs to be denormalized</param>
        /// <param name="processingMode">Requested processing mode</param>
        /// <param name="iEntityManager">Entity Manager object</param>
        /// <param name="callerContext">Indicates Application and Module name by which operation is being performed</param>
        /// <param name="performedActionList">Actions performed on Entity because of which denorm is needed</param>
        /// <returns>Entity operation result</returns>
        public EntityOperationResult ProcessDenormalizedRelationships(Entity entity, ProcessingMode processingMode, IEntityManager iEntityManager, CallerContext callerContext, Collection<EntityActivityList> performedActionList = null)
        {
            EntityOperationResult entityOperationResult = null;

            EntityOperationResultCollection eorCollection = ProcessDenormalizedRelationships(new EntityCollection() { entity }, processingMode, iEntityManager, callerContext, performedActionList);

            if (eorCollection != null && eorCollection.Count > 0)
            {
                entityOperationResult = eorCollection[entity.Id];
            }

            return entityOperationResult;
        }

        /// <summary>
        /// Processes denormalized relationships for the requested entities
        /// </summary>
        /// <param name="entities">Entities for which relationship needs to be denormalized</param>
        /// <param name="processingMode">Requested processing mode</param>
        /// <param name="iEntityManager">Entity Manager object</param>
        /// <param name="callerContext">Indicates Application and Module name by which operation is being performed</param>
        /// <param name="performedActionList">Actions performed on Entity because of which denorm is needed</param>
        /// <returns>Entity operation results</returns>
        public EntityOperationResultCollection ProcessDenormalizedRelationships(EntityCollection entities, ProcessingMode processingMode, IEntityManager iEntityManager, CallerContext callerContext, Collection<EntityActivityList> performedActionList = null)
        {
            EntityOperationResultCollection eorCollection = new EntityOperationResultCollection();

            DurationHelper durationHelper = new DurationHelper(DateTime.Now);
            DurationHelper overallDurationHelper = new DurationHelper(DateTime.Now);

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("RelationshipManager.RelationshipDenormBL.ProcessRelationshipDenorm", MDMTraceSource.RelationshipProcess, false);
            }

            try
            {
                #region STEP: Validation and Initial Setup

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Starting parameter validation and initial setup for denorm process...", MDMTraceSource.RelationshipProcess);
                }

                #region Parameters Validation

                if (entities == null || entities.Count < 1)
                {
                    _localeMessage = _localeMessageBL.Get(_systemUILocale, "111816", false, callerContext);
                    throw new MDMOperationException("111816", _localeMessage.Message, "RelationshipManager.RelationshipDenormBL", String.Empty, "ProcessRelationshipDenorm");   //Entities collection is null or empty.
                }

                #endregion

                #region Initial Setup

                //Override caller context module to Denorm..
                callerContext.Module = MDMCenterModules.Denorm;

                RelationshipCollection denormRelsToBeProcessed = null;

                #endregion

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Done with parameter validation and initial setup for denorm process.", MDMTraceSource.RelationshipProcess);
                }

                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - parameter validation and initial setup for denorm process.", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.RelationshipProcess);
                }

                #endregion

                #region STEP: Get Denormalized relationships

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Getting denormalized relationships...", MDMTraceSource.RelationshipProcess);
                }

                denormRelsToBeProcessed = GetEntitiesDenormalizedRelationships(entities, processingMode, performedActionList, callerContext);

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Done with denormalized relationships get.", MDMTraceSource.RelationshipProcess);
                }

                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Denormalized relationship get for all updated Entity Relationships.", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.RelationshipProcess);
                }

                #endregion

                eorCollection = ProcessDenormalizedRelationships(entities, denormRelsToBeProcessed, processingMode, iEntityManager, durationHelper, callerContext);
            }
            finally
            {
                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Overall relationship denorm processing time", overallDurationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.RelationshipProcess);

                    MDMTraceHelper.StopTraceActivity("RelationshipManager.RelationshipDenormBL.ProcessRelationshipDenorm", MDMTraceSource.RelationshipProcess);
                }
            }

            return eorCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="containerId"></param>
        /// <param name="categoryId"></param>
        /// <param name="entityTypeId"></param>
        /// <returns></returns>
        public RelationshipDenormProcessingSetting GetRelationshipDenormProcessingSettingsForContext(Int32 organizationId, Int32 containerId, Int64 categoryId, Int32 entityTypeId)
        {
            RelationshipDenormProcessingSetting relationshipDenormProcessingSetting = null;
            RelationshipDenormProcessingSettingCollection relDenormProcessingSettings = null;

            #region STEP: Get Relationship Denorm Processing Settings Configuration

            relDenormProcessingSettings = AppConfigurationHelper.GetRelationshipDenormProcessingSettings();

            #endregion

            //--------------------------------------------------
            //Step 1: Get the processing setting with the exact context match or with nearest context match
            //Step 2: Among the processing setting in the step1, get the permission with high sequence value
            //--------------------------------------------------

            //Step 1:
            IEnumerable<RelationshipDenormProcessingSetting> nearestMatchedProcessingSettings = null;

            if (relDenormProcessingSettings != null)
            {
                nearestMatchedProcessingSettings = relDenormProcessingSettings.Where(p => (p.SettingContext.OrganizationIds == null || p.SettingContext.OrganizationIds.Count < 1 || p.SettingContext.OrganizationIds.Contains(organizationId)) &&
                    (p.SettingContext.ContainerIds == null || p.SettingContext.ContainerIds.Count < 1 || p.SettingContext.ContainerIds.Contains(containerId)) &&
                    (p.SettingContext.CategoryIds == null || p.SettingContext.CategoryIds.Count < 1 || p.SettingContext.CategoryIds.Contains(categoryId)) &&
                    (p.SettingContext.EntityTypeIds == null || p.SettingContext.EntityTypeIds.Count < 1 || p.SettingContext.EntityTypeIds.Contains(entityTypeId)));
            }

            if (nearestMatchedProcessingSettings != null && nearestMatchedProcessingSettings.Count() > 0)
            {
                relationshipDenormProcessingSetting = nearestMatchedProcessingSettings.OrderByDescending(p => p.SettingContext.Weightage).First();
            }

            return relationshipDenormProcessingSetting;
        }

        /// <summary>
        /// Ensures the relationship tree denorm for the non-denormalized entities.
        /// </summary>
        /// <param name="entityIdList">Entities for which relationship needs to be denormalized</param>
        /// <param name="entityContext">Specifies EntityContext which indicates what all information is to be loaded in Entity object</param>
        /// <param name="entityManager">Entity Manager object</param>
        /// <param name="impactedEntityManager">Impacted Entity Manager object</param>
        /// <param name="callerContext">Indicates Application and Module name by which operation is being performed</param>
        /// <returns></returns>
        public EntityOperationResultCollection EnsureInheritedEntityRelationships(Collection<Int64> entityIdList, EntityContext entityContext, 
            Boolean processWhereUsed, Boolean processImpactedExtensions, Boolean processImpactedHierarchies, Boolean processRelationshipTree, 
            Boolean processInheritable, Boolean processRelationshipAttributes, IEntityManager entityManager, IImpactedEntityManager impactedEntityManager, 
            CallerContext callerContext)
        {
            EntityOperationResultCollection entityOperationResultCollection = new EntityOperationResultCollection();

            //Get Relationship Tree denorm processing key...
            Boolean isOnDemandRelationshipDenormEnabled = 
                AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.RelationshipDenorm.RelationshipTreeDenorm.OnDemand.Enabled", true);

            if (isOnDemandRelationshipDenormEnabled)
            {
                //Check whether the relationship ASync Denorm is still under progress..
                ImpactedEntityCollection impactedEntityCollection = impactedEntityManager.GetByIdList(entityIdList, callerContext);
                if (impactedEntityCollection != null)
                {
                    EntityCollection entityCollection = GetNonDenormalizedEntities(impactedEntityCollection, entityContext);
                    if (entityCollection.Count > 0)
                    {
                        //Relationship denorm is still under progress..
                        //So do forced denorm before getting relationship details
                        entityOperationResultCollection = ProcessDenormalizedRelationships(entityCollection, processWhereUsed, processImpactedExtensions,
                            processImpactedHierarchies, processRelationshipTree, processInheritable, processRelationshipAttributes, ProcessingMode.Sync, 
                            entityManager, callerContext);
                    }
                }
            }

            return entityOperationResultCollection;
        }

        /// <summary>
        /// Indicates whether De-normalized relationships are dirty or not.The return value indicates whether inherited relationships are in denorm process or not.
        /// </summary>
        /// <param name="entityId">Indicates entity id.</param>
        /// <param name="containerId">Indicate container id.</param>
        /// <param name="relationshipTypeId">Indicates relationship type id.
        /// If relationship type id is 0 means All. It means get the status from all relationship types else requested relationship type.</param>
        /// <param name="entityTreeIdList">Indicates entity tree id list.</param>
        /// <param name="callerContext">Indicates Application and Module name by which operation is being performed</param>
        /// <returns>true if the De-normalized/Inherited relationships are in denorm progress; otherwise, false.</returns>
        public Boolean IsDenormalizedRelationshipsDirty(Int64 entityId, Int32 containerId, Int32 relationshipTypeId, String entityTreeIdList, CallerContext callerContext)
        {
            DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Read);
            return new RelationshipDenormDA().IsDenormalizedRelationshipsDirty(entityId, containerId, relationshipTypeId, entityTreeIdList, command);
        }

        #endregion

        #region Private Methods

        private EntityCollection GetNonDenormalizedEntities(ImpactedEntityCollection impactedEntityCollection, EntityContext entityContext)
        {
            EntityCollection entityCollection = new EntityCollection();

            foreach (ImpactedEntity impactedEntity in impactedEntityCollection)
            {
                if (impactedEntity != null && (impactedEntity.IsEntityDenormRequired && !impactedEntity.IsEntityDenormInProcess))
                {
                    //Relationship denorm is still under progress..
                    //So do forced denorm before getting relationship details
                    Entity entity = new Entity();
                    entity.Id = impactedEntity.EntityId;
                    entity.ContainerId = entityContext.ContainerId;
                    entity.EntityContext = entityContext;

                    entityCollection.Add(entity);
                }
            }

            return entityCollection;
        }

        private RelationshipCollection GetEntitiesDenormalizedRelationships(EntityCollection entities, ProcessingMode processingMode, Collection<EntityActivityList> performedActionList, CallerContext callerContext)
        {
            RelationshipCollection denormalizedRelationships = new RelationshipCollection();
            RelationshipDenormProcessingSetting entityRelDenormProcessingSetting = null;

            Boolean getWhereUsed = false;
            Boolean getImpactedExtensions = false;
            Boolean getImpactedHierarchies = false;
            Boolean getRelationshipTree = false;
            Boolean getInheritable = false;
            Boolean getRelationshipAttributes = false;

            if (processingMode == ProcessingMode.Async)
            {
                if (performedActionList != null)
                {
                    //In ASync mode, get all possible Denorm relationships and process.. So, skipping processing mode settings reading here..
                    //And setting all processing types as true and getting denormalized relationships..

                    //Commented below lines: We need to process attributes in case of new relationship creation also.
                    //So, checking only RelationshipAttributesUpdate is not sufficient here. We need to come up with a
                    //way to identify when to process attributes and when to not

                    //Check whether the performed action is Relationship Attributes Update..
                    //If yes, get relationship attribute details for denorm relationships
                    //if (performedActionList.Contains(EntityActivityList.RelationshipAttributeUpdate))
                    //{
                    //    getRelationshipAttributes = true;
                    //}

                    //Prepare entity container Ids dictionary
                    Dictionary<Int64, Int32> entityContainerIds = new Dictionary<Int64, Int32>();

                    foreach (Entity entity in entities)
                    {
                        if (!entityContainerIds.ContainsKey(entity.Id))
                        {
                            entityContainerIds.Add(entity.Id, entity.ContainerId);
                        }
                    }

                    denormalizedRelationships = GetDenormalizedRelationships(entityContainerIds, 0, 0, true, true, true, true, true, true, callerContext);
                }
            }
            else
            {
                foreach (Entity entity in entities)
                {
                    getRelationshipAttributes = false;

                    //Get processing settings for this entity
                    entityRelDenormProcessingSetting = GetRelationshipDenormProcessingSettingsForContext(entity.OrganizationId, entity.ContainerId, entity.CategoryId, entity.EntityTypeId);

                    if (entityRelDenormProcessingSetting != null)
                    {
                        EntityChangeContext entityChangeContext = (EntityChangeContext)entity.GetChangeContext();

                        EntityActivityList relationshipChangeAction = EntityActivityList.UnKnown;

                        //Determine action
                        if (entityChangeContext.IsRelationshipsCreated)
                        {
                            getRelationshipAttributes = true;
                            relationshipChangeAction = EntityActivityList.RelationshipCreate;
                        }
                        else if (entityChangeContext.IsRelationshipAttributesChanged)
                        {
                            getRelationshipAttributes = true;
                            relationshipChangeAction = EntityActivityList.RelationshipAttributeUpdate;
                        }
                        else if (entityChangeContext.IsRelationshipsUpdated)
                        {
                            relationshipChangeAction = EntityActivityList.RelationshipUpdate;
                        }
                        else if (entityChangeContext.IsRelationshipsDeleted)
                        {
                            relationshipChangeAction = EntityActivityList.RelationshipDelete;
                        }

                        RelationshipDenormAction relDenormAction = entityRelDenormProcessingSetting.RelationshipDenormActions.FirstOrDefault(p => p.Action == relationshipChangeAction);

                        if (relDenormAction != null)
                        {
                            getWhereUsed = (relDenormAction.WhereUsedProcessingMode == processingMode) ? true : false;
                            getImpactedExtensions = (relDenormAction.ExtensionProcessingMode == processingMode) ? true : false;
                            getImpactedHierarchies = (relDenormAction.HierarchyProcessingMode == processingMode) ? true : false;
                            getRelationshipTree = (relDenormAction.RelationshipTreeProcessingMode == processingMode) ? true : false;
                        }
                    }

                    RelationshipCollection relationships = GetDenormalizedRelationships(entity.Id, entity.ContainerId, 0, 0, getWhereUsed, getImpactedExtensions, getImpactedHierarchies, getRelationshipTree, getInheritable, getRelationshipAttributes, callerContext);

                    if (relationships != null)
                    {
                        Boolean considerRelationshipsForProcessing = true;

                        if (relationships.Count == 1)
                        {
                            Relationship firstRelationship = relationships.First();

                            if (firstRelationship.RelationshipProcessingContext.EntityId == entity.Id && (firstRelationship.RelationshipCollection == null || firstRelationship.RelationshipCollection.Count < 1))
                            {
                                considerRelationshipsForProcessing = false;
                            }
                        }

                        if (considerRelationshipsForProcessing)
                        {
                            foreach (Relationship rel in relationships)
                            {
                                denormalizedRelationships.Add(rel);
                            }
                        }
                    }
                }
            }

            return denormalizedRelationships;
        }

        private RelationshipCollection GetEntitiesDenormalizedRelationships(Dictionary<Int64, Int32> entityContainerIds, Int64 relationshipId, Int32 relationshipTypeId, Boolean getWhereUsed, Boolean getImpactedExtensions, Boolean getImpactedHierarchies, Boolean getRelationshipTree, Boolean getInheritable, Boolean loadRelationshipAttributes, CallerContext callerContext)
        {
            RelationshipCollection denormalizedRelationships = null;

            DurationHelper durationHelper = new DurationHelper(DateTime.Now);
            DurationHelper overallDurationHelper = new DurationHelper(DateTime.Now);

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("RelationshipManager.RelationshipDenormBL.GetDenormalizedRelationships", MDMTraceSource.RelationshipGet, false);
            }

            try
            {
                if (getWhereUsed || getImpactedHierarchies || getImpactedExtensions || getRelationshipTree || getInheritable)
                {
                    #region Parameter Validations

                    if (entityContainerIds == null || entityContainerIds.Count < 1)
                    {
                        _localeMessage = _localeMessageBL.Get(_systemUILocale, "111785", false, callerContext);
                        throw new MDMOperationException("111785", _localeMessage.Message, "RelationshipManager.RelationshipDenormBL", String.Empty, "GetDenormalizedRelationships");   //Entity Ids are not available.
                    }

                    #endregion

                    #region Initial Setup

                    //Get command
                    DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Update);

                    //Load relationship attribute models if requested..
                    AttributeModelCollection relationshipAttributeModels = null;

                    if (loadRelationshipAttributes)
                    {
                        relationshipAttributeModels = GetBaseRelationshipAttributeModels(callerContext);
                    }

                    #endregion

                    if (Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Getting denormalized relationships...", MDMTraceSource.RelationshipGet);
                    }

                    RelationshipDenormDA relationshipDenormDataManager = new RelationshipDenormDA();
                    denormalizedRelationships = relationshipDenormDataManager.GetEntitiesDenormalizedRelationships(entityContainerIds, relationshipId, relationshipTypeId, getWhereUsed, getImpactedExtensions, getImpactedHierarchies, getRelationshipTree, getInheritable, loadRelationshipAttributes, relationshipAttributeModels, command, callerContext);

                    if (Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Done with Denormalized Relationships Get", MDMTraceSource.RelationshipGet);
                    }

                    if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Denormalized Relationships Get", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.RelationshipGet);
                    }
                }
                else
                {
                    if (Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "None of the options are requested. Skipping Denormalized Relationships Get.", MDMTraceSource.RelationshipGet);
                    }
                }
            }
            finally
            {
                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Overall denormalized relationships get time", overallDurationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.RelationshipGet);

                    MDMTraceHelper.StopTraceActivity("RelationshipManager.RelationshipDenormBL.GetDenormalizedRelationships", MDMTraceSource.RelationshipGet);
                }
            }

            return denormalizedRelationships;
        }

        private AttributeModelCollection GetBaseRelationshipAttributeModels(CallerContext callerContext)
        {
            AttributeModelCollection relationshipAttributeModels = null;

            AttributeModelBL attributeModelManager = new AttributeModelBL();
            relationshipAttributeModels = attributeModelManager.GetAllBaseRelationshipAttributeModels();

            return relationshipAttributeModels;
        }

        private EntityOperationResultCollection ProcessDenormalizedRelationships(EntityCollection entities, RelationshipCollection denormRelsToBeProcessed, ProcessingMode processingMode, IEntityManager iEntityManager, DurationHelper durationHelper, CallerContext callerContext)
        {
            EntityOperationResultCollection eorCollection = new EntityOperationResultCollection();
            EntityCollection entitiesWithRelationshipsToBeProcessed = new EntityCollection();
            EntityProcessorUtility entityProcessorUtility = new EntityProcessorUtility();
            EntityProcessorErrorLogBL entityProcessorErrorLogManager = new EntityProcessorErrorLogBL();
            Dictionary<Int32, AttributeModelCollection> masterAttributeModels = new Dictionary<Int32, AttributeModelCollection>();

            //Get the entity activity log Id
            //ASSUMPTION: In case of multiple entities, we are assuming that the current batch of entities are coming because of a single Activity
            //Hence we are considering activity log Id of first entity
            Int64 activityLogId = entities.First().ActivityLogId;

            if (denormRelsToBeProcessed != null && denormRelsToBeProcessed.Count > 0)
            {
                #region STEP: Prepare Entity details for denormalized processing

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Preparing entities needed for denormalized relationships processing...", MDMTraceSource.RelationshipProcess);
                }

                foreach (Relationship relationship in denormRelsToBeProcessed)
                {
                    if (relationship.RelationshipProcessingContext != null)
                    {
                        Entity entity = null;
                        Int32 relationshipTypeId = relationship.RelationshipTypeId;
                        Int64 entityId = relationship.RelationshipProcessingContext.EntityId;
                        Collection<LocaleEnum> dataLocales = GetLocaleList(relationship);

                        if (processingMode == ProcessingMode.Async && !entities.Contains(entityId))
                            continue;

                        #region Remove Unmapped relationship attributes

                        relationship.SetRelationshipAttributes(RemoveUnMappedRelationshipAttributes(relationship.RelationshipAttributes, masterAttributeModels , relationshipTypeId, relationship.ContainerId , dataLocales));
                        RemoveUnMappedRelationshipAttributesForChildRelationships(relationship.RelationshipCollection, masterAttributeModels);

                        #endregion

                        if (entitiesWithRelationshipsToBeProcessed.Contains(entityId))
                        {
                            entity = entitiesWithRelationshipsToBeProcessed[entityId];
                            entity.Relationships.Add(relationship);
                        }
                        else
                        {
                            entity = new Entity();
                            entity.Id = entityId;
                            entity.EntityTypeId = relationship.RelationshipProcessingContext.EntityTypeId;
                            entity.CategoryId = relationship.RelationshipProcessingContext.CategoryId;
                            entity.ContainerId = relationship.ContainerId;
                            entity.ActivityLogId = activityLogId;
                            entity.Action = ObjectAction.Update;

                            entity.Relationships.Add(relationship);

                            entitiesWithRelationshipsToBeProcessed.Add(entity);
                        }
                    }
                }

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Done with entities preparation needed for denormalized relationships processing.", MDMTraceSource.RelationshipProcess);
                }

                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Preparing Entity details for denormalized processing.", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.RelationshipProcess);
                }

                #endregion

                #region STEP: Process denormalized relationships

                if (entitiesWithRelationshipsToBeProcessed != null && entitiesWithRelationshipsToBeProcessed.Count > 0)
                {
                    if (Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Starting denormalized relationships processing...", MDMTraceSource.RelationshipProcess);
                    }

                    EntityProcessingOptions entityProcessingOptions = new EntityProcessingOptions();
                    entityProcessingOptions.ProcessingMode = processingMode;
                    entityProcessingOptions.ApplyAVS = false;
                    entityProcessingOptions.ApplyDMS = false;

                    eorCollection = iEntityManager.Process(entitiesWithRelationshipsToBeProcessed, entityProcessingOptions, callerContext);

                    if (Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Done with denormalized relationships processing.", MDMTraceSource.RelationshipProcess);
                    }

                    if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Denormalized relationships processing.", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.RelationshipProcess);
                    }
                }

                #endregion
            }
            else
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "There are no relationships to be denormalized. Skipping Relationship Denorm process...", MDMTraceSource.RelationshipProcess);
                }
            }

            return eorCollection;
        }

        private AttributeCollection RemoveUnMappedRelationshipAttributes(AttributeCollection relationshipAttributes, Dictionary<Int32, AttributeModelCollection> masterAttributeModels, Int32 relationshipTypeId, Int32 containerId, Collection<LocaleEnum> dataLocales)
        {
            AttributeModelCollection attributeModels = null;
            AttributeCollection filteredRlationshipAttributes = new AttributeCollection();
            RelationshipBL relationshipBL = new RelationshipBL();

            if (relationshipAttributes != null && relationshipAttributes.Count() > 0)
            {
                if (masterAttributeModels.ContainsKey(relationshipTypeId))
                {
                    attributeModels = masterAttributeModels[relationshipTypeId];
                }
                else
                {
                    RelationshipContext relationshipContext = new RelationshipContext();
                    relationshipContext.RelationshipTypeIdList.Add(relationshipTypeId);
                    relationshipContext.ContainerId = containerId;
                    relationshipContext.DataLocales = dataLocales;

                    Dictionary<Int32, AttributeModelCollection> attributeModelsPerRT = relationshipBL.GetRelationshipAttributeModelsByContext(relationshipContext);

                    if (attributeModelsPerRT != null)
                    {
                        attributeModels = attributeModelsPerRT[relationshipTypeId];

                        if (attributeModels != null && attributeModels.Count > 0)
                        {
                            masterAttributeModels.Add(relationshipTypeId, attributeModels);
                        }
                    }
                }

                if (attributeModels != null && attributeModels.Count > 0)
                {
                    foreach (AttributeModel attributeModel in attributeModels)
                    {
                        IAttribute iRelationshipAttribute = relationshipAttributes.GetAttribute(attributeModel.Id, attributeModel.Locale);

                        if (iRelationshipAttribute != null)
                        {
                            filteredRlationshipAttributes.Add(iRelationshipAttribute);
                        }
                    }
                }
            }

            return filteredRlationshipAttributes;
        }

        private void RemoveUnMappedRelationshipAttributesForChildRelationships(RelationshipCollection childRelationships, Dictionary<Int32, AttributeModelCollection> masterAttributeModels)
        {
            foreach (Relationship childRelationship in childRelationships)
            {
                Collection<LocaleEnum> dataLocales = GetLocaleList(childRelationship);

                childRelationship.SetRelationshipAttributes(RemoveUnMappedRelationshipAttributes(childRelationship.RelationshipAttributes, masterAttributeModels, childRelationship.RelationshipTypeId, childRelationship.ContainerId, dataLocales));

                if (childRelationships != null && childRelationships.Count > 0)
                {
                    RemoveUnMappedRelationshipAttributesForChildRelationships(childRelationship.RelationshipCollection, masterAttributeModels);
                }
            }
        }

        private Collection<LocaleEnum> GetLocaleList(Relationship relationship)
        {
            Collection<LocaleEnum> dataLocales = new Collection<LocaleEnum>();

            if (relationship != null && relationship.RelationshipAttributes != null && relationship.RelationshipAttributes.Count > 0)
            {
                dataLocales = relationship.RelationshipAttributes.GetLocaleList();
            }

            return dataLocales;
        }

        private void GetSecurityPrincipal()
        {
            if (_securityPrincipal == null)
            {
                _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            }
        }

        private void InitalizeMembers()
        {
            _localeMessageBL = new LocaleMessageBL();
            _systemUILocale = GlobalizationHelper.GetSystemUILocale();
            _systemDataLocale = GlobalizationHelper.GetSystemDataLocale();
        }

        #endregion

        #endregion
    }
}