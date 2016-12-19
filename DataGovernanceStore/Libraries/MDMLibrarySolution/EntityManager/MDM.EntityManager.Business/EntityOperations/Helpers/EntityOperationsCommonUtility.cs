using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Practices.ServiceLocation;

namespace MDM.EntityManager.Business.EntityOperations.Helpers
{
    using BusinessObjects;
    using Core;
    using Interfaces;
    using Utility;
    using DataModelManager.Business;
    using IntegrationManager.Business;
    using BusinessObjects.Workflow;

    /// <summary>
    /// 
    /// </summary>
    internal sealed class EntityOperationsCommonUtility
    {
        #region Fields
        #endregion

        #region Constructors
        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Fire entity event based on given input parameters
        /// </summary>
        /// <param name="entitiesFilteredByChangeType">Indicates entities collection filtered by change type</param>
        /// <param name="entitiesFilteredByAction">Indicates filtered entity collection by action</param> 
        /// <param name="allEntities">Indicates collection of all entities.</param>
        /// <param name="mdmEvent">Indicates MDM event to be fire</param>
        /// <param name="entityManager">Indicates instance of entity manager</param>
        /// <param name="mdmRuleParams">Indicates MDM rule params</param>
        /// <returns>Return 'true' if event fires successfully else 'false'</returns>
        public static Boolean FireEntityEvent(EntityCollection entitiesFilteredByChangeType, EntityCollection entitiesFilteredByAction, EntityCollection allEntities, MDMEvent mdmEvent, IEntityManager entityManager, MDMRuleParams mdmRuleParams)
        {
            Boolean continueProcess = true;

            if (mdmRuleParams != null && entitiesFilteredByChangeType != null && entitiesFilteredByChangeType.Count > 0)
            {
                Int32 userId = (mdmRuleParams.UserSecurityPrincipal != null) ? mdmRuleParams.UserSecurityPrincipal.CurrentUserId : -1;

                mdmRuleParams.Events = new Collection<MDMEvent>() { mdmEvent };
                mdmRuleParams.Entities = entitiesFilteredByChangeType;

                //MDMRuleEvaluator.Evaluate(mdmRuleParams);

                var eventArgs = new EntityEventArgs(entitiesFilteredByChangeType, entityManager, mdmRuleParams.EntityOperationResults, userId, mdmRuleParams.CallerContext, mdmRuleParams.EntityProcessingOptions);

                switch (mdmEvent)
                {
                    case MDMEvent.EntityAttributesCreating:
                        EntityEventManager.Instance.OnEntityAttributesCreating(eventArgs);
                        break;
                    case MDMEvent.EntityExtensionsCreating:
                        EntityEventManager.Instance.OnEntityExtensionsCreating(eventArgs);
                        break;
                    case MDMEvent.EntityHierarchyCreating:
                        EntityEventManager.Instance.OnEntityHierarchyCreating(eventArgs);
                        break;
                    case MDMEvent.EntityRelationShipsCreating:
                        EntityEventManager.Instance.OnEntityRelationShipsCreating(eventArgs);
                        break;
                    case MDMEvent.EntityCreating:
                        EntityEventManager.Instance.OnEntityCreating(eventArgs);
                        break;
                    case MDMEvent.EntityAttributesCreated:
                        EntityEventManager.Instance.OnEntityAttributesCreated(eventArgs);
                        break;
                    case MDMEvent.EntityExtensionsCreated:
                        EntityEventManager.Instance.OnEntityExtensionsCreated(eventArgs);
                        break;
                    case MDMEvent.EntityHierarchyCreated:
                        EntityEventManager.Instance.OnEntityHierarchyCreated(eventArgs);
                        break;
                    case MDMEvent.EntityRelationShipsCreated:
                        EntityEventManager.Instance.OnEntityRelationShipsCreated(eventArgs);
                        break;
                    case MDMEvent.EntityCreated:
                        EntityEventManager.Instance.OnEntityCreated(eventArgs);
                        break;
                    case MDMEvent.EntityAttributesUpdating:
                        EntityEventManager.Instance.OnEntityAttributesUpdating(eventArgs);
                        break;
                    case MDMEvent.EntityExtensionsUpdating:
                        EntityEventManager.Instance.OnEntityExtensionsUpdating(eventArgs);
                        break;
                    case MDMEvent.EntityHierarchyUpdating:
                        EntityEventManager.Instance.OnEntityHierarchyUpdating(eventArgs);
                        break;
                    case MDMEvent.EntityRelationShipsUpdating:
                        EntityEventManager.Instance.OnEntityRelationShipsUpdating(eventArgs);
                        break;
                    case MDMEvent.EntityUpdating:
                        EntityEventManager.Instance.OnEntityUpdating(eventArgs);
                        break;
                    case MDMEvent.EntityAttributesUpdated:
                        EntityEventManager.Instance.OnEntityAttributesUpdated(eventArgs);
                        break;
                    case MDMEvent.EntityExtensionsUpdated:
                        EntityEventManager.Instance.OnEntityExtensionsUpdated(eventArgs);
                        break;
                    case MDMEvent.EntityHierarchyUpdated:
                        EntityEventManager.Instance.OnEntityHierarchyUpdated(eventArgs);
                        break;
                    case MDMEvent.EntityRelationShipsUpdated:
                        EntityEventManager.Instance.OnEntityRelationShipsUpdated(eventArgs);
                        break;
                    case MDMEvent.EntityUpdated:
                        EntityEventManager.Instance.OnEntityUpdated(eventArgs);
                        break;
                    case MDMEvent.EntityValidate:
                        EntityEventManager.Instance.OnEntityValidate(eventArgs);
                        break;
                    case MDMEvent.EntityLoaded:
                        EntityEventManager.Instance.OnEntityLoaded(eventArgs);
                        break;
                    case MDMEvent.EntityLoading:
                        EntityEventManager.Instance.OnEntityLoading(eventArgs);
                        break;
                    case MDMEvent.EntityReclassifying:
                        EntityEventManager.Instance.OnEntityReclassifying(eventArgs);
                        break;
                    case MDMEvent.EntityReclassified:
                        EntityEventManager.Instance.OnEntityReclassified(eventArgs);
                        break;
                    case MDMEvent.EntityHierarchyChanged:
                        EntityEventManager.Instance.OnEntityHierarchyChanged(eventArgs);
                        break;
                    case MDMEvent.EntityExtensionsChanged:
                        EntityEventManager.Instance.OnEntityExtensionsChanged(eventArgs);
                        break;
                    case MDMEvent.EntityCreatePostProcessStarting:
                        EntityEventManager.Instance.OnEntityCreatePostProcessStarting(eventArgs);
                        break;
                    case MDMEvent.EntityUpdatePostProcessStarting:
                        EntityEventManager.Instance.OnEntityUpdatePostProcessStarting(eventArgs);
                        break;
                    case MDMEvent.EntityCreatePostProcessCompleted:
                        EntityEventManager.Instance.OnEntityCreatePostProcessCompleted(eventArgs);
                        break;
                    case MDMEvent.EntityUpdatePostProcessCompleted:
                        EntityEventManager.Instance.OnEntityUpdatePostProcessCompleted(eventArgs);
                        break;
                    case MDMEvent.EntityDeletePostProcessStarting:
                        EntityEventManager.Instance.OnEntityDeletePostProcessStarting(eventArgs);
                        break;
                    case MDMEvent.EntityDeletePostProcessCompleted:
                        EntityEventManager.Instance.OnEntityDeletePostProcessCompleted(eventArgs);
                        break;
                }

                mdmRuleParams.EntityOperationResults.RefreshOperationResultStatus();

                continueProcess = EntityOperationsHelper.ScanAndFilterEntitiesBasedOnResults(entitiesFilteredByAction, allEntities, mdmRuleParams.EntityOperationResults, mdmRuleParams.CallerContext);
            }

            return continueProcess;
        }

        /// <summary>
        /// Updates the entity operation results.
        /// </summary>
        /// <param name="entities">Defines the entities.</param>
        /// <param name="entityOperationResults">Defines the entity operation results.</param>
        /// <param name="localeMessage">Defines the locale message.</param>
        /// <param name="exceptionTitle">Defines the exception title.</param>
        /// <param name="reasonType">Defines Type of the reason.</param>
        public static void UpdateEntityOperationResults(EntityCollection entities, EntityOperationResultCollection entityOperationResults, LocaleMessage localeMessage, String exceptionTitle, ReasonType reasonType = ReasonType.NotSpecified)
        {
            var parameters = new Collection<Object> { exceptionTitle };

            if (entities != null && entities.Count > 0)
            {
                foreach (Entity entity in entities)
            {
                    EntityOperationResult entityOperationResult = (EntityOperationResult)entityOperationResults.GetByReferenceId(entity.Id) ?? entityOperationResults.GetEntityOperationResult(entity.Id);

                    if (entityOperationResult != null)
                {
                        OperationResultType oprResultType = OperationResultType.Information;

                        switch (localeMessage.MessageClass)
                    {
                            case MessageClassEnum.Warning:
                                oprResultType = OperationResultType.Warning;
                                break;
                            case MessageClassEnum.Error:
                                oprResultType = OperationResultType.Error;
                                break;
            }

                        entityOperationResult.PerformedAction = entity.Action;
                        entityOperationResult.AddOperationResult(localeMessage.Code, localeMessage.Message, parameters, reasonType, -1, -1, oprResultType);
                                    }
                                }
                            }
                                    else
                                    {
                entityOperationResults.AddOperationResult(localeMessage.Code, localeMessage.Message, OperationResultType.Error);
                }

            entityOperationResults.RefreshOperationResultStatus();
        }

        /// <summary>
        ///     Checks if All entities have the SameAttributes
        /// </summary>
        public static Boolean IsAllSiblingEntitiesWithSameAttributes(EntityCollection entities, MDMTraceSource traceSource)
        {
            var durationHelper = new DurationHelper(DateTime.Now);

            Boolean allSiblingEntitiesWithSameAttributes = true;
            Entity firstEntity = null;
            EntityContext firstEntityContext = null;
            EntityChangeContext firstEntityChangeContext = null;

            if (entities.Count > 0)
            {
                firstEntity = entities.ElementAt(0);
                firstEntityContext = firstEntity.EntityContext;
                firstEntityChangeContext = (EntityChangeContext)firstEntity.GetChangeContext();
            }

            if (entities.Count == 1)
            {
                allSiblingEntitiesWithSameAttributes = false;
            }
            else
            {
                foreach (Entity entity in entities)
                {
                    if (entity == firstEntity)
                        continue;

                    EntityContext entityContext = entity.EntityContext;

                    if (firstEntityChangeContext != null && (firstEntityContext != null && !(firstEntityContext.ContainerId.Equals(entityContext.ContainerId)
                                                                                             && firstEntityContext.EntityTypeId.Equals(entityContext.EntityTypeId)
                                                                                             && firstEntityContext.CategoryId.Equals(entityContext.CategoryId) //Verify if locale check is required..
                                                                                             && ValueTypeHelper.CollectionEquals(firstEntityContext.DataLocales, entityContext.DataLocales)
                                                                                             && ValueTypeHelper.CollectionEquals(firstEntityChangeContext.AttributeIdList, entity.GetChangeContext().AttributeIdList))))
                    {
                        allSiblingEntitiesWithSameAttributes = false;
                        break;
                    }
                }
            }

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Verified if all entities are sibling with same change context", durationHelper.GetDurationInMilliseconds(DateTime.Now)), traceSource);

            return allSiblingEntitiesWithSameAttributes;
        }

        /// <summary>
        ///     Check EntityContext, if no locale value is provided then add locale as System default data locale
        /// </summary>
        /// <param name="entityContext">EntityContext to check</param>
        /// <param name="application">
        ///     Application which called EntityGet(). System default data locale can be different for
        ///     different application.
        /// </param>
        public static void ValidateAndUpdateEntityContextForLocales(EntityContext entityContext, MDMCenterApplication application)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Starting verification of locales provided in the entity get request...");

            //Add system default data  locale in data locale if no locale info is provided by user.
            if (entityContext == null)
            {
                entityContext = new EntityContext();
            }

            if (entityContext.Locale == LocaleEnum.UnKnown || entityContext.Locale == LocaleEnum.Neutral)
            {
                LocaleEnum systemDataLocale = GlobalizationHelper.GetSystemDataLocale();

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("No locale provided for entity. System will use system data locale:{0} for this request.", systemDataLocale));
                }

                entityContext.Locale = systemDataLocale;
            }

            if (entityContext.DataLocales != null && entityContext.DataLocales.Count < 1)
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("No data locales provided in the entity context. System will use entity level locale:{0} for this request.", entityContext.Locale));
                }

                entityContext.DataLocales.Add(entityContext.Locale);
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Done with verification of locales provided in the entity get request");
        }

        /// <summary>
        ///     Builds a comma separated string for the specified entity ids.
        /// </summary>
        public static String GetEntityIdListAsString(Collection<Int64> entityIdList)
        {
            var stringBuilder = new StringBuilder();

            Int32 entityIdListCount = entityIdList.Count;
            for (Int32 index = 0; index < entityIdListCount; index++)
            {
                stringBuilder.Append(entityIdList.ElementAt(index));

                if (index < (entityIdListCount - 1))
                {
                    stringBuilder.Append(",");
                }
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Calculate entities action based on the change context
        /// <para>
        /// Note: Currently it is setting Read action when there are no changes based on change context
        /// </para>
        /// </summary>
        /// <param name="entities">Entities for which Action needs to be calculated</param>
        /// <param name="entityProcessingOptions">Entity processing options being sent</param>
        /// <returns>If calculation is successful returns true; otherwise false.</returns>
        public static Boolean CalculateEntityAction(EntityCollection entities, EntityProcessingOptions entityProcessingOptions)
        {
            Boolean isSuccessful = true;

            foreach (Entity entity in entities)
            {
                //Refresh change context
                IEntityChangeContext changeContext = entity.GetChangeContext(true);

                if (IsAsyncMode(entityProcessingOptions) || changeContext.IsAttributesChanged || changeContext.IsRelationshipsChanged ||
                    changeContext.IsHierarchyChanged || changeContext.IsExtensionsChanged || IsMetadataChanged(entity))
                {
                    continue;
                }

                entity.Action = ObjectAction.Read;
            }

            return isSuccessful;
        }

        /// <summary>
        /// Populate entity operation result status
        /// </summary>
        /// <param name="entities">Indicates entity collection to find out respective entity operation result</param>
        /// <param name="entityoperationResults">Indicates entity operation result collection to be populated.</param>
        /// <param name="operationAction">Indicates operation action</param>
        public static void UpdateEntityOperationResultStatus(EntityCollection entities, EntityOperationResultCollection entityOperationResults, String operationAction)
        {
            if (entityOperationResults.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors)
            {
                //Here, entities would be the only ones which are successful till they come out of transaction.
                foreach (Entity entity in entities)
                {
                    EntityOperationResult entityOperationResult = entityOperationResults.SingleOrDefault(eor => eor.EntityId == entity.Id);

                    if (entityOperationResult != null && entityOperationResult.OperationResultStatus == OperationResultStatusEnum.Failed)
                    {
                        if (String.IsNullOrWhiteSpace(operationAction))
                            operationAction = "Processe"; //The last letter 'e' is required for a proper message

                        //Prepare the info messages
                        String infoMessage = String.Format("{0}d successfully but some errors occurred in {0}d event.", operationAction);

                        entityOperationResult.Informations.Add(new Information("", infoMessage));

                        foreach (Error err in entityOperationResult.Errors)
                        {
                            entityOperationResult.Informations.Add(new Information(err.ErrorCode, err.ErrorMessage, err.Params));
                        }

                        entityOperationResult.Errors.Clear();

                        entityOperationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                    }
                }
            }
            else if (entityOperationResults.OperationResultStatus == OperationResultStatusEnum.None)
            {
                //All are succeeded..
                //So update the status to Successful
                entityOperationResults.OperationResultStatus = OperationResultStatusEnum.Successful;
            }
        }

        #region Cleanse Methods

        /// <summary>
        /// Before process the entity, cleanse for any properties that would have come through and that we do not need.
        /// </summary>
        /// <param name="entity">Indicates entiy object</param>
        public static void CleanseEntity(Entity entity)
        {
            //the id of the value object is not managed internally..If we get it from the caller, we need to reset it. 
            if (entity.Attributes != null)
            {
                entity.Attributes.ResetValueId();
            }

            //do the same for relationships also..
            CleanseRelationships(entity.Relationships);
        }

        /// <summary>
        /// Before process the entity, cleanse for any properties that would have come through and that we do not need.
        /// </summary>
        /// <param name="relationships">Indicates relationship collection object</param>
        public static void CleanseRelationships(RelationshipCollection relationships)
        {
            if (relationships != null)
            {
                foreach (Relationship relationship in relationships)
                {
                    if (relationship.RelationshipAttributes != null)
                    {
                        relationship.RelationshipAttributes.ResetValueId();
                    }

                    CleanseRelationships(relationship.RelationshipCollection);
                }
            }
        }

        #endregion Cleanse Methods

        #region Translation Methods

        /// <summary>
        /// Enqueue integration activity log for TMS.
        /// </summary>
        /// <param name="entities">Indicates collection of entities</param>
        /// <param name="callerContext">Indicates name of application and module name of caller</param>
        public static void EnqueueIntegrationActivity(EntityCollection entities, CallerContext callerContext)
        {
            try
            {
                IntegrationActivityLogBL integrationActivityLogManager = new IntegrationActivityLogBL();
                integrationActivityLogManager.Create(entities, "MDMTranslationConnector", "TRANSLATION EXPORT", IntegrationType.Outbound, callerContext); //TODO:: Add AppConfigs for Connector Name and Integration Message Type Name
            }
            catch (Exception ex)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Failed to create integration activity log. Error: {0}", ex.Message), MDMTraceSource.EntityProcess);
            }
        }

        /// <summary>
        /// Indicates translation module is enabled or not.
        /// </summary>
        /// <returns>Returns 'true' if translation module is enabled else 'false'</returns>
        public static Boolean IsTranslationEnabled()
        {
            MDMFeatureConfig mdmFeatureConfig = MDMFeatureConfigHelper.GetMDMFeatureConfig(MDMCenterApplication.MDMCenter, "TMSConnector", "1");
            return mdmFeatureConfig.IsEnabled;
        }

        #endregion

        #region Workflow Methods

        /// <summary>
        /// Resume the workflow activity
        /// </summary>
        /// <param name="entityFamilyQueueItem">Entity Family Queue Item</param>
        /// <param name="entityOperationResult">Entity Operation Result</param>
        /// <param name="groupEntityCollection">Entity collection</param>
        /// <param name="callerContext">caller context</param>
        public static void ResumeWorkflowInstance(EntityFamilyQueue entityFamilyQueueItem, EntityOperationResult entityOperationResult, EntityCollection groupEntityCollection, CallerContext callerContext)
        {
            if (entityFamilyQueueItem.EntityFamilyChangeContexts != null)
            {
                EntityFamilyChangeContext entityFamilyChangeContext = entityFamilyQueueItem.EntityFamilyChangeContexts.GetByEntityGlobalFamilyId(entityFamilyQueueItem.EntityGlobalFamilyId);

                if (entityFamilyChangeContext != null && entityFamilyChangeContext.WorkflowChangeContext != null)
                {
                    WorkflowChangeContext workflowContext = entityFamilyChangeContext.WorkflowChangeContext;

                    WorkflowActionContext workflowActionContext = (WorkflowActionContext)workflowContext.GetWorkflowActionContext();

                    if (workflowActionContext != null)
                    {

                        if (entityOperationResult != null && entityOperationResult.HasError)
                        {
                            workflowActionContext.FaultMessage = entityOperationResult.ToXml();
                            workflowActionContext.UserAction = "Reject";
                        }
                        else
                        {
                            workflowActionContext.UserAction = "Approve";
                        }

                        IWorkflowRuntimeManager workflowInstanceManager = ServiceLocator.Current.GetInstance(typeof(IWorkflowRuntimeManager)) as IWorkflowRuntimeManager;

                        OperationResult operationResult = entityOperationResult;

                        Int32 success = workflowInstanceManager.ResumeWorkflow(workflowContext.WorkflowRuntimeInstanceId, workflowActionContext, ref operationResult, callerContext);

                        if ((operationResult != null) && operationResult.Errors.Any())
                        {
                            EntityOperationResultCollection resultCollection = new EntityOperationResultCollection();
                            resultCollection.Add(entityOperationResult);

                            if (groupEntityCollection != null)
                            {
                                EntityStateValidationBL validationStateBL = new EntityStateValidationBL();
                                validationStateBL.ProcessForSystemException(groupEntityCollection, resultCollection, callerContext);
                            }
                        }
                    }
                }
            }
        }

        #endregion Workflow Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static Boolean IsMetadataChanged(Entity entity)
        {
            return entity.OriginalEntity == null ||
                   !entity.Name.Equals(entity.OriginalEntity.Name) ||
                   !entity.LongName.Equals(entity.OriginalEntity.LongName) ||
                   !entity.CategoryName.Equals(entity.OriginalEntity.CategoryName) ||
                   !entity.CategoryLongName.Equals(entity.OriginalEntity.CategoryLongName) ||
                   !entity.CategoryPath.Equals(entity.OriginalEntity.CategoryPath) ||
                   entity.ParentEntityId != entity.OriginalEntity.ParentEntityId ||
                   entity.ParentExtensionEntityId != entity.OriginalEntity.ParentExtensionEntityId ||
                   (
                       entity.EntityMoveContext != null &&
                       ((entity.EntityMoveContext.TargetCategoryId != 0 && entity.EntityMoveContext.TargetCategoryId != entity.OriginalEntity.CategoryId) || // Reclassify  scenario
                        (entity.EntityMoveContext.TargetParentEntityId != 0 && entity.EntityMoveContext.TargetParentEntityId != entity.OriginalEntity.ParentEntityId) || //HiearchyReParent scenario
                        (entity.EntityMoveContext.TargetParentExtensionEntityId != 0 && entity.EntityMoveContext.TargetParentExtensionEntityId != entity.OriginalEntity.ParentExtensionEntityId)) //ExtensionReParent scenario
                       );
        }

        /// <summary>
        /// Сheck for async mode because there will not be changes in Entity by design but need to process it anyway 
        /// </summary>
        /// <param name="entityProcessingOptions"></param>
        /// <returns></returns>
        private static bool IsAsyncMode(EntityProcessingOptions entityProcessingOptions)
        {
            return entityProcessingOptions != null
                && (entityProcessingOptions.ProcessingMode == ProcessingMode.AsyncUpdate || entityProcessingOptions.ProcessingMode == ProcessingMode.AsyncCreate
                   || entityProcessingOptions.ProcessingMode == ProcessingMode.AsyncDelete || entityProcessingOptions.ProcessingMode == ProcessingMode.Async);
        }

        #endregion

        #region Private Methods
        #endregion Private Methods

        #endregion
    }
}