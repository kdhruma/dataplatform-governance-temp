using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Transactions;

namespace MDM.EntityManager.Business
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.BusinessObjects.Exports;
    using MDM.BusinessObjects.Workflow;
    using MDM.ConfigurationManager.Business;
    using MDM.ContainerManager.Business;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.Core.Extensions;
    using MDM.DataModelManager.Business;
    using MDM.EntityManager.Business.EntityOperations.Helpers;
    using MDM.EntityManager.Data;
    using MDM.Interfaces;
    using MDM.Utility;

    /// <summary>
    /// Class containing the methods to handle promote logic
    /// </summary>
    public class PromoteBL : BusinessLogicBase, IPromoteManager
    {
        #region Fields

        /// <summary>
        /// Field denoting the entity manager
        /// </summary>
        private EntityBL _entityManager = null;

        /// <summary>
        /// Field denoting the current trace settings
        /// </summary>
        private TraceSettings _currentTraceSettings = null;

        /// <summary>
        /// Field denoting the flag mentioning if the tracing is enabled
        /// </summary>
        private Boolean _isTracingEnabled = false;

        /// <summary>
        /// Field denoting the caller context
        /// </summary>
        private CallerContext _callerContext = null;

        /// <summary>
        /// Field denoting the security principal
        /// </summary>
        private readonly SecurityPrincipal _securityPrincipal;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the PromoteBL class.
        /// </summary>
        public PromoteBL()
        {
            _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            _isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;
            _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            _entityManager = new EntityBL();
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Promotes the specified entity group queue item.
        /// </summary>
        /// <param name="promoteQueueItem">The entity queue item.</param>
        /// <param name="callerContext">The caller context.</param>
        /// <param name="groupEntityCollection"></param>
        /// <returns>Operation result collection containing the results</returns>
        public EntityOperationResult ProcessPromote(EntityFamilyQueue promoteQueueItem, CallerContext callerContext, ref EntityCollection groupEntityCollection)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            EntityOperationResult operationResult = null;
            _callerContext = callerContext ?? new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.Promote, "MDM.EntityManager.Business.PromoteBL");

            if (_isTracingEnabled)
            {
                diagnosticActivity.Start();
            }
            try
            {
                switch (promoteQueueItem.EntityActivityList)
                {
                    case EntityActivityList.Promote:
                        operationResult = HandlePromote(promoteQueueItem, ref groupEntityCollection);
                        break;
                    case EntityActivityList.AutoPromote:
                    case EntityActivityList.EmergencyPromote:
                        operationResult = HandleAttributePromote(promoteQueueItem, ref groupEntityCollection);
                        break;
                    case EntityActivityList.UpstreamPromote:
                    case EntityActivityList.CategoryPromote:
                        operationResult = HandleInheritancePromote(promoteQueueItem, ref groupEntityCollection);
                        break;
                    default:
                        diagnosticActivity.LogWarning(String.Format("Unsupported action {0} encountered for entity group id: {1}.", promoteQueueItem.EntityActivityList.ToString(), promoteQueueItem.EntityFamilyId));
                        break;
                }

                //call the resume workflowInstance irrespective the promote failure
                EntityOperationsCommonUtility.ResumeWorkflowInstance(promoteQueueItem, operationResult, groupEntityCollection, callerContext);
                
                if (operationResult != null && operationResult.HasError)
                {
                    diagnosticActivity.LogError(String.Format("Unable to resume the workflow for Entity Global Family Id: {0}", promoteQueueItem.EntityGlobalFamilyId));
                }
            }
            finally
            {
                if (_isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return operationResult;
        }

        /// <summary>
        /// Enqueues the entity for promote.
        /// </summary>
        /// <param name="entityIdCollection">The entity identifier collection.</param>
        /// <param name="callerContext">The caller context.</param>
        /// <param name="businessRuleName">Name of the business rule.</param>
        /// <param name="businessRuleContextName">Name of the business rule context.</param>
        /// <returns></returns>
        public OperationResultCollection EnqueueForPromote(Collection<Int64> entityIdCollection, CallerContext callerContext, String businessRuleName = "", String businessRuleContextName = "")
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            OperationResultCollection operationResultCollection = new OperationResultCollection();
            _callerContext = callerContext ?? new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.Promote, "MDM.EntityManager.Business.PromoteBL");

            try
            {
                if (_isTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                EntityFamilyQueueCollection entityFamilyQueueCollection = new EntityFamilyQueueCollection();
                EntityFamilyQueue entityFamilyQueueItem = null;

                foreach (Int64 entityId in entityIdCollection)
                {
                    OperationResult operationResult = new OperationResult();
                    operationResult.ReferenceId = entityId.ToString();
                    operationResultCollection.Add(operationResult);
                    Boolean isMasterCollaboration = false;

                    #region Validations

                    Entity entity = null;

                    ValidateParameters(entityId, operationResult, out entity, out isMasterCollaboration, callerContext);

                    if (operationResult.OperationResultStatus == OperationResultStatusEnum.Failed)
                    {
                        continue;
                    }

                    #endregion Validations

                    entityFamilyQueueItem = new EntityFamilyQueue()
                    {
                        EntityActivityList = EntityActivityList.Promote,
                        EntityFamilyId = entity.EntityFamilyId,
                        EntityGlobalFamilyId = entity.EntityGlobalFamilyId,
                        Action = ObjectAction.Create,
                        ContainerId = entity.ContainerId,
                        
                    };

                    EntityFamilyChangeContext entityFamilyChangeContext = new EntityFamilyChangeContext()
                    {
                        BusinessRuleName= businessRuleName,
                        BusinessRuleContextName = businessRuleContextName,
                        IsMasterCollaborationRecord = isMasterCollaboration
                    };

                    entityFamilyQueueItem.EntityFamilyChangeContexts.Add(entityFamilyChangeContext);

                    entityFamilyQueueCollection.Add(entityFamilyQueueItem);
                }

                if (entityFamilyQueueCollection.Count > 0)
                {
                    if (_isTracingEnabled)
                    {
                        diagnosticActivity.LogInformation(String.Format("Completed validation and starting to process {0} entity family queue items.", entityFamilyQueueCollection.Count));
                    }

                    OperationResultCollection queueOperationResults = new EntityFamilyQueueBL().Process(entityFamilyQueueCollection, callerContext);

                    foreach (OperationResult queueResult in queueOperationResults)
                    {
                        OperationResult promoteResult = operationResultCollection.GetOperationResultByReferenceId(queueResult.ReferenceId);
                        promoteResult.CopyErrorInfoAndWarning(queueResult);
                        promoteResult.CopyReturnValues(queueResult);
                    }
                }
                else
                {
                    if (_isTracingEnabled)
                    {
                        diagnosticActivity.LogWarning("Unable to continue with the process of entity family queue items as all of them failed validation.");
                    }
                }

                operationResultCollection.RefreshOperationResultStatus();

                return operationResultCollection;

            }
            finally
            {
                if (_isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }
        }

        /// <summary>
        /// Enqueues the entities for promote.
        /// </summary>
        /// <param name="entityFamilyQueueCollection">The entity family queue collection.</param>
        /// <param name="callerContext">The caller context.</param>
        /// <returns></returns>
        public OperationResultCollection EnqueueForPromote(EntityFamilyQueueCollection entityFamilyQueueCollection, CallerContext callerContext)
        {
            // This method is generally called from internal core classes like workflow activity/Entity Process/Entity Family Processor. 
            // Workflow activity will pass the workflow change context as part of the entity family queue item

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            OperationResultCollection operationResultCollection = new OperationResultCollection();
            _callerContext = callerContext ?? new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.Promote, "MDM.EntityManager.Business.PromoteBL");
            List<EntityActivityList> promoteActivityList = new List<EntityActivityList>
                    {
                        EntityActivityList.EmergencyPromote,
                        EntityActivityList.AutoPromote,
                        EntityActivityList.CategoryPromote,
                        EntityActivityList.UpstreamPromote,
                        EntityActivityList.Promote
                    };
            try
            {
                if (_isTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                foreach (EntityFamilyQueue queueItem in entityFamilyQueueCollection)
                {
                    Int64 entityId = queueItem.EntityFamilyId;

                    OperationResult operationResult = new OperationResult();
                    operationResult.ReferenceId = entityId.ToString();
                    operationResultCollection.Add(operationResult);
                    Boolean isMasterCollaboration = false;

                    #region Validations

                    Entity entity = null;

                    ValidateParameters(entityId, operationResult, out entity, out isMasterCollaboration, callerContext);

                    if (operationResult.OperationResultStatus == OperationResultStatusEnum.Failed)
                    {
                        continue;
                    }

                    if (!promoteActivityList.Contains(queueItem.EntityActivityList))
                    {
                        Object[] parameters = new Object[] { entity.LongName, queueItem.EntityActivityList.ToString() };
                        String message = String.Format("Unable to initiate promote for entity {0}, as the EntityActivityList {1} is not supported.", parameters);
                        operationResult.AddOperationResult("114293", message, parameters, OperationResultType.Error);
                        operationResult.OperationResultStatus = OperationResultStatusEnum.Failed;
                        continue;
                    }

                    #endregion Validations

                    queueItem.EntityGlobalFamilyId = entity.EntityGlobalFamilyId;
                    queueItem.ContainerId = entity.ContainerId;
                    queueItem.EntityFamilyChangeContexts.FirstOrDefault().IsMasterCollaborationRecord = isMasterCollaboration;
                    queueItem.Action = ObjectAction.Create;
                }

                operationResultCollection.RefreshOperationResultStatus();

                if (_isTracingEnabled)
                {
                    diagnosticActivity.LogInformation("Completed validating the entity family queue items.");
                }

                if (!ScanAndFilterEntityFamilyQueueBasedOnResults(entityFamilyQueueCollection, operationResultCollection))
                {
                    if (_isTracingEnabled)
                    {
                        diagnosticActivity.LogWarning("Unable to continue with the process of entity family queue items as all of them failed validation.");
                    }
                    return operationResultCollection;
                }
                else
                {
                    OperationResultCollection queueOperationResults = new EntityFamilyQueueBL().Process(entityFamilyQueueCollection, callerContext);

                    foreach (OperationResult queueResult in queueOperationResults)
                    {
                        OperationResult promoteResult = operationResultCollection.GetOperationResultByReferenceId(queueResult.ReferenceId);
                        promoteResult.CopyErrorInfoAndWarning(queueResult);
                        promoteResult.CopyReturnValues(queueResult);
                    }
                }

                operationResultCollection.RefreshOperationResultStatus();
                return operationResultCollection;
            }
            finally
            {
                if (_isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }
        }

        #endregion Public Methods

        #region Promote Handling Methods

        /// <summary>
        /// Handles the promote process.
        /// </summary>
        /// <param name="promoteQueueItem">The promote Queue Item</param>
        /// <param name="groupEntityCollection"></param>
        /// <returns></returns>
        private EntityOperationResult HandlePromote(EntityFamilyQueue promoteQueueItem, ref EntityCollection groupEntityCollection)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            EntityOperationResultCollection resultCollection = null;
            Int64 entityId = -1;
            Entity entity = null;

            if (_isTracingEnabled)
            {
                diagnosticActivity.Start();
            }
            try
            {
                entityId = promoteQueueItem.EntityFamilyId;
                Int32 containerId = promoteQueueItem.ContainerId;

                EntityFamilyChangeContext entityFamilyChangeContext = promoteQueueItem.EntityFamilyChangeContexts.FirstOrDefault();

                #region Get Entity Hierarchy for Promotable Containers

                groupEntityCollection = GetBaseEntity(entityId);

                MDMRuleParams mdmRuleParams = new MDMRuleParams()
                {
                    Entities = groupEntityCollection,
                    EntityOperationResults = resultCollection,
                    UserSecurityPrincipal = _securityPrincipal,
                    CallerContext = _callerContext,
                    Events = new Collection<MDMEvent>() { MDMEvent.EntityFamilyPromoteQualifying, MDMEvent.EntityFamilyPromoted },
                    DDGCallerModule = DDGCallerModule.PromoteProcess
                };

                Container currentContainer;

                entity = GetEntityHierarchy(entityId, containerId, promoteQueueItem.EntityActivityList, mdmRuleParams, out currentContainer, null, diagnosticActivity);

                groupEntityCollection = new EntityCollection { entity };

                resultCollection = new EntityOperationResultCollection(groupEntityCollection);

                if (_isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Completed loading the promotable entity hierarchy.");
                }

                #endregion Get Entity Hierarchy for Promotable Containers

                #region Fire Pre Event - EntityFamilyPromoteQualifying

                //Fire the EntityFamilyPromoteQualifying event

                if (!EntityOperationsCommonUtility.FireEntityEvent(groupEntityCollection, groupEntityCollection, groupEntityCollection, MDMEvent.EntityFamilyPromoteQualifying, _entityManager, mdmRuleParams))
                {
                    EntityOperationResult entityResult = (EntityOperationResult)resultCollection.GetByEntityId(entityId);
                    if (entityResult != null && entityResult.HasError)
                    {
                        using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                        {

                            Error firstError = entityResult.Errors.FirstOrDefault();

                            if (firstError != null)
                            {
                                String qualificationMessageCode = "114287";
                                String messageParams = String.Format("{0}#@#{1}", entity.EntityFamilyId, firstError.ErrorCode);

                                Dictionary<Int64, Int16> promoteEntities = FetchAllEntitiesForPromote(entity, resultCollection);

                                if (String.IsNullOrWhiteSpace(_callerContext.ProgramName))
                                {
                                    _callerContext.ProgramName = "Promote Qualification";
                                }

                                DBCommandProperties command = DBCommandHelper.Get(_callerContext, MDMCenterModuleAction.Execute);

                                new PromoteDA().ProcessPromote(promoteEntities, resultCollection, false, qualificationMessageCode, messageParams, _securityPrincipal.CurrentUserName, command, _callerContext);
                            }


                            ChangeReasonTypeToSystemException(resultCollection);

                            EntityStateValidationBL validationStateBL = new EntityStateValidationBL();
                            validationStateBL.ProcessForSystemException(groupEntityCollection, resultCollection, _callerContext);

                            transactionScope.Complete();
                        }
                    }

                    return entityResult;
                }

                if (_isTracingEnabled)
                {
                    diagnosticActivity.LogInformation(String.Format("EntityFamilyPromoteQualifying event completed with operation result status {0}", resultCollection.OperationResultStatus.ToString()));
                }

                #endregion Fire Pre Event - EntityFamilyPromoteQualifying

                EntityOperationResult entityOperationResult = (EntityOperationResult)resultCollection.GetByEntityId(entityId);

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, GetTransactionOptions(ProcessingMode.Async)))
                {
                    #region Call DA

                    if (!entityOperationResult.HasError)
                    { 
                        Dictionary<Int64, Int16> allEntitiesToBePromoted = FetchAllEntitiesForPromote(entity, resultCollection);
                        
                        if (_isTracingEnabled)
                        {
                            diagnosticActivity.LogInformation("Preparing the dictionary of all entity ids and hierarchy levels completed and starting to call DA.");
                        }

                        String messageCode = String.Empty;
                        String messageParams = String.Empty;
                        EntityFamilyChangeContext changeContext = promoteQueueItem.EntityFamilyChangeContexts.FirstOrDefault();

                        if (changeContext != null)
                        {
                            if (entityFamilyChangeContext != null && entityFamilyChangeContext.WorkflowChangeContext != null)
                            {
                                WorkflowChangeContext workflowContext = entityFamilyChangeContext.WorkflowChangeContext;
                                WorkflowActionContext workflowActionContext = (WorkflowActionContext)workflowContext.GetWorkflowActionContext();

                                if (workflowActionContext != null)
                                {
                                    String workflowName = workflowActionContext.WorkflowName;
                                    String activityName = workflowActionContext.CurrentActivityLongName;

                                    messageCode = "114280";
                                    messageParams = String.Format("{0}#@#{1}#@#{2}", workflowName, activityName, promoteQueueItem.EntityFamilyId);

                                    _callerContext.ProgramName += String.Format(" - Promote by workflow {0} and activity {1}", workflowName, activityName);
                                }
                            }

                            if (String.IsNullOrWhiteSpace(messageCode)) //This means workflow action context was not available
                            {
                                messageCode = "114281";
                                _callerContext.ProgramName = String.Format("Promote by BR {0}", changeContext.BusinessRuleName);
                                messageParams = String.Format("{0}#@#{1}#@#{2}", changeContext.BusinessRuleName, changeContext.BusinessRuleContextName, promoteQueueItem.EntityFamilyId);
                            }
                        }

                        DBCommandProperties command = DBCommandHelper.Get(_callerContext, MDMCenterModuleAction.Execute);
                        Boolean needsApprovedCopy = (currentContainer != null) ? currentContainer.NeedsApprovedCopy : false;

                        new PromoteDA().ProcessPromote(allEntitiesToBePromoted, resultCollection, needsApprovedCopy, messageCode, messageParams, _securityPrincipal.CurrentUserName, command, _callerContext);

                        if (_isTracingEnabled)
                        {
                            diagnosticActivity.LogDurationInfo("Completed executing DA for Promote process.");
                        }
                    }
                    else
                    {
                        if (_isTracingEnabled)
                        {
                            diagnosticActivity.LogInformation("Exiting promote process has EntityFamilyPromoteQualifying event has failed operation result status.");
                        }

                        return entityOperationResult;
                    }

                    #endregion Call DA

                    #region Fire Post Event - EntityFamilyPromoted

                    if (!EntityOperationsCommonUtility.FireEntityEvent(groupEntityCollection, groupEntityCollection, groupEntityCollection, MDMEvent.EntityFamilyPromoted, _entityManager, mdmRuleParams))
                    {
                        return (EntityOperationResult)resultCollection.GetByEntityId(entityId);
                    }

                    if (_isTracingEnabled)
                    {
                        diagnosticActivity.LogInformation(String.Format("EntityFamilyPromoted event completed with operation result status {0}", resultCollection.OperationResultStatus.ToString()));
                    }

                    #endregion Fire Post Event - EntityFamilyPromoted

                    #region Export Qualification

                    if (entityOperationResult != null && entityOperationResult.OperationResultStatus == OperationResultStatusEnum.Successful)
                    {
                        Int64 promotedFamilyId = -1;
                        Object returnValue = entityOperationResult.ReturnValues.FirstOrDefault();
                        if (returnValue != null)
                        {
                            promotedFamilyId = (Int64)returnValue;
                        }

                        if (promotedFamilyId > 0)
                        {
                            QueueExportQualifiedItems(promotedFamilyId, promoteQueueItem.EntityActivityList, diagnosticActivity);
                        }
                    }

                    #endregion Export Qualification

                    #region Refresh Results and Save to State on failure

                    ChangeReasonTypeToSystemException(resultCollection);
                    resultCollection.RefreshOperationResultStatus();

                    if (resultCollection.OperationResultStatus == OperationResultStatusEnum.Failed)
                    {
                        EntityStateValidationBL validationStateBL = new EntityStateValidationBL();
                        validationStateBL.ProcessForSystemException(groupEntityCollection, resultCollection, _callerContext);
                    }

                    #endregion Refresh Results and Save to State on failure

                    transactionScope.Complete();
                }

                return entityOperationResult;
            }
            catch (Exception ex)
            {
                if (groupEntityCollection != null && groupEntityCollection.Count > 0 && entityId > 0)
                {
                    EntityOperationResult entityResult = (EntityOperationResult)resultCollection.GetByEntityId(entityId);

                    Collection<Object> parameters = new Collection<Object> { entity != null ? entity.LongName : entityId.ToString(), ex.Message };
                    String message = String.Format("Unable to promote entity {0}, as it failed with exception: {1}.", parameters.ToArray());
                    entityResult.AddOperationResult("114288", message, parameters, ReasonType.SystemError, -1, -1, OperationResultType.Error);

                    ChangeReasonTypeToSystemException(entityResult);

                    EntityStateValidationBL validationStateBL = new EntityStateValidationBL();
                    validationStateBL.ProcessForSystemException(groupEntityCollection, resultCollection, _callerContext);
                }

                throw;
            }
            finally
            {
                if (_isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }
        }

        /// <summary>
        /// Fetches all entities for promote.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        private static Dictionary<Int64, Int16> FetchAllEntitiesForPromote(Entity entity, EntityOperationResultCollection operationResults)
        {
            Dictionary<Int64, Int16> allEntitiesToBePromoted = new Dictionary<Int64, Int16>();
            IAttribute lifeCycleAttribute = entity.GetAttribute((Int32)SystemAttributes.LifecycleStatus);

            if (lifeCycleAttribute != null)
            {
                String lifeCycleStatusValue = lifeCycleAttribute.GetCurrentValue().ToString();

                if (String.Compare(lifeCycleStatusValue, LifecycleStatusValues.MarkedForDeletion) != 0)
                {
                    //Add current entity
                    allEntitiesToBePromoted.Add(entity.EntityFamilyId, entity.HierarchyLevel);

                    //Get Variants
                    IEntityCollection allChildEntities = entity.GetAllChildEntities();

                    if (allChildEntities != null && allChildEntities.Count > 0)
                    {
                        foreach (Entity item in allChildEntities)
                        {
                            IAttribute childEntityLifeCycleAttribute = item.GetAttribute((Int32)SystemAttributes.LifecycleStatus);
                            if (childEntityLifeCycleAttribute != null)
                            {
                                String childEntityLifeCycleStatus = childEntityLifeCycleAttribute.GetCurrentValue().ToString();

                                if (String.Compare(childEntityLifeCycleStatus, LifecycleStatusValues.MarkedForDeletion) != 0)
                                {

                                    allEntitiesToBePromoted.Add(item.Id, item.HierarchyLevel);
                                    operationResults.Add(new EntityOperationResult { EntityId = item.Id });
                                }
                            }
                        }
                    }

                    //Get extensions
                    IEntityCollection childExtensions = entity.GetAllExtendedEntities();

                    if (childExtensions != null && childExtensions.Count > 0)
                    {
                        //Get extension variant trees
                        foreach (Entity childExtension in childExtensions)
                        {
                            IAttribute childExtensionLifeCycleAttribute = childExtension.GetAttribute((Int32)SystemAttributes.LifecycleStatus);
                            if (childExtensionLifeCycleAttribute != null)
                            {
                                String childExtensionLifeCycleStatus = childExtensionLifeCycleAttribute.GetCurrentValue().ToString();

                                if (String.Compare(childExtensionLifeCycleStatus, LifecycleStatusValues.MarkedForDeletion) != 0)
                                {

                                    allEntitiesToBePromoted.Add(childExtension.Id, childExtension.HierarchyLevel);
                                    operationResults.Add(new EntityOperationResult { EntityId = childExtension.Id });

                                    IEntityCollection childExtensionVariants = childExtension.GetAllChildEntities();
                                    if (childExtensionVariants != null && childExtensionVariants.Count > 0)
                                    {
                                        foreach (Entity childExtensionVariant in childExtensionVariants)
                                        {
                                            IAttribute childExtensionVariantLifeCycleAttribute = childExtensionVariant.GetAttribute((Int32)SystemAttributes.LifecycleStatus);
                                            if (childExtensionVariantLifeCycleAttribute != null)
                                            {
                                                String childExtensionVariantLifeCycleStatus = childExtensionVariantLifeCycleAttribute.GetCurrentValue().ToString();

                                                if (String.Compare(childExtensionVariantLifeCycleStatus, LifecycleStatusValues.MarkedForDeletion) != 0)
                                                {

                                                    allEntitiesToBePromoted.Add(childExtensionVariant.Id, childExtensionVariant.HierarchyLevel);
                                                    operationResults.Add(new EntityOperationResult { EntityId = childExtensionVariant.Id });
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return allEntitiesToBePromoted;
        }

        /// <summary>
        /// Handles the attribute promote in case of Auto Promotable attribute or Emergency Attribute changes.
        /// </summary>
        /// <param name="promoteQueueItem">The promote Queue Item</param>
        /// <param name="groupEntityCollection"></param>
        /// <returns></returns>
        private EntityOperationResult HandleAttributePromote(EntityFamilyQueue promoteQueueItem, ref EntityCollection groupEntityCollection)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            EntityOperationResultCollection resultCollection = new EntityOperationResultCollection();
            EntityOperationResult operationResult = null;
            Int64 entityId = -1;

            if (_isTracingEnabled)
            {
                diagnosticActivity.Start();
            }
            try
            {
                _callerContext.ProgramName = String.Format("{0} {1}", _callerContext.ProgramName, promoteQueueItem.EntityActivityList.ToString());
                EntityFamilyChangeContext changeContext = promoteQueueItem.EntityFamilyChangeContexts.FirstOrDefault();

                entityId = changeContext.EntityFamilyId;

                groupEntityCollection = GetBaseEntity(entityId);
                operationResult = new EntityOperationResult { EntityId = entityId };

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    DBCommandProperties command = DBCommandHelper.Get(_callerContext, MDMCenterModuleAction.Execute);
                    new PromoteDA().ProcessAttributePromote(changeContext, resultCollection, promoteQueueItem.EntityActivityList, _securityPrincipal.CurrentUserName, command, _callerContext);
                    
                    #region Export Qualification

                    if (resultCollection != null && resultCollection.OperationResultStatus == OperationResultStatusEnum.Successful)
                    {
                        Collection<Int64> promotedEntityIds = new Collection<Int64>();
                        foreach (OperationResult result in resultCollection)
                        {
                            Int64 promotedFamilyId = -1;
                            Object returnValue = result.ReturnValues.FirstOrDefault();
                            if (returnValue != null)
                            {
                                promotedFamilyId = (Int64)returnValue;
                            }

                            if (promotedFamilyId > 0)
                            {
                                promotedEntityIds.Add(promotedFamilyId);
                            }
                        }

                        if (promotedEntityIds.Count > 0)
                        {
                            WorkflowInvokableEntityInfoCollection workflowInvokableEntityInfoColl = _entityManager.GetWorkflowInvokableEntityIds(promotedEntityIds, _callerContext);
                            if (workflowInvokableEntityInfoColl.Count > 0)
                            {
                                Collection<Int64> exportFamilyIds = new Collection<Int64>();

                                foreach (WorkflowInvokableEntityInfo info in workflowInvokableEntityInfoColl)
                                {
                                    if (!exportFamilyIds.Contains(info.WorkflowInvokableEntityId))
                                    {
                                        QueueExportQualifiedItems(info.WorkflowInvokableEntityId, promoteQueueItem.EntityActivityList, diagnosticActivity);
                                        exportFamilyIds.Add(info.WorkflowInvokableEntityId);
                                    }
                                } 
                            }
                        }
                    }

                    #endregion Export Qualification

                    transactionScope.Complete();
                }
            }
            catch (Exception ex)
            {
                if (groupEntityCollection != null && groupEntityCollection.Count > 0 && entityId > 0)
                {
                    IEntity entity = groupEntityCollection.GetEntity(entityId);

                    Collection<Object> parameters = new Collection<Object> { entity != null ? entity.LongName : entityId.ToString(), ex.Message };
                    String message = String.Format("Unable to promote entity {0}, as it failed with exception: {1}.", parameters.ToArray());
                    operationResult.AddOperationResult("114288", message, parameters, ReasonType.SystemError, -1, -1, OperationResultType.Error);

                    ChangeReasonTypeToSystemException(operationResult);

                    EntityStateValidationBL validationStateBL = new EntityStateValidationBL();
                    validationStateBL.ProcessForSystemException(groupEntityCollection, resultCollection, _callerContext);
                }
                throw;
            }
            finally
            {
                if (_isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }
            
            return operationResult;
        }

        /// <summary>
        /// Handles the inheritance promote in case of category or upstream changes.
        /// </summary>
        /// <param name="promoteQueueItem">The promote Queue Item</param>
        /// <param name="groupEntityCollection"></param>
        /// <returns></returns>
        private EntityOperationResult HandleInheritancePromote(EntityFamilyQueue promoteQueueItem, ref EntityCollection groupEntityCollection)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            EntityOperationResult operationResult = null;
            EntityOperationResultCollection resultCollection = null;
            Int64 entityId = -1;

            if (_isTracingEnabled)
            {
                diagnosticActivity.Start();
            }
            try
            {
                entityId = promoteQueueItem.EntityFamilyId;

                groupEntityCollection = GetBaseEntity(entityId);
                resultCollection = new EntityOperationResultCollection(groupEntityCollection);

                Entity familyEntity = (Entity)groupEntityCollection.GetEntity(entityId);
                operationResult = (EntityOperationResult)resultCollection.GetByEntityId(entityId);

                #region Export Qualification

                if (familyEntity != null)
                {
                    Int64 promotedFamilyId = familyEntity.CrossReferenceId;

                    if (promotedFamilyId > 0)
                    {
                        QueueExportQualifiedItems(promotedFamilyId, promoteQueueItem.EntityActivityList, diagnosticActivity);
                    }
                }

                #endregion Export Qualification
            }
            catch (Exception ex)
            {
                if (groupEntityCollection != null && groupEntityCollection.Count > 0 && entityId > 0)
                {
                    IEntity entity = groupEntityCollection.GetEntity(entityId);

                    Collection<Object> parameters = new Collection<Object> { entity != null ? entity.LongName : entityId.ToString(), ex.Message };
                    String message = String.Format("Unable to promote entity {0}, as it failed with exception: {1}.", parameters.ToArray());
                    operationResult.AddOperationResult("114288", message, parameters, ReasonType.SystemError, -1, -1, OperationResultType.Error);

                    ChangeReasonTypeToSystemException(operationResult);

                    EntityStateValidationBL validationStateBL = new EntityStateValidationBL();
                    validationStateBL.ProcessForSystemException(groupEntityCollection, resultCollection, _callerContext);
                }
                throw;
            }
            finally
            {
                if (_isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }
            return operationResult;
        }

        #endregion Promote Handling Methods

        #region Helpers

        /// <summary>
        /// Gets the promotable container list.
        /// </summary>
        /// <param name="containerId">The container identifier.</param>
        /// <param name="promoteType">Indicates the type of promote.</param>
        /// <param name="currentContainer">The current container.</param>
        /// <returns></returns>
        private ContainerCollection GetPromotableContainerList(Int32 containerId, EntityActivityList promoteType, out Container currentContainer)
        {
            ContainerBL containerBL = new ContainerBL();
            ContainerCollection allContainers = containerBL.GetAll(new ContainerContext { ApplySecurity = false, IncludeApproved = true, LoadAttributes = false }, _callerContext);

            currentContainer = allContainers.GetContainer(containerId);

            ContainerCollection promotableContainers = new ContainerCollection();

            if (promoteType != EntityActivityList.Promote)
            {
                promotableContainers = (ContainerCollection)allContainers.GetChildContainers(containerId, true);
            }
            else
            {
                GetChildContainersHierarchy(allContainers, promotableContainers, containerId, false);
            }

            return promotableContainers;
        }

        /// <summary>
        /// Gets the child containers hierarchy.
        /// </summary>
        /// <param name="containers">The containers.</param>
        /// <param name="childContainer">The child container.</param>
        /// <param name="containerId">The container identifier.</param>
        /// <param name="includeSelf">if set to <c>true</c> [include self].</param>
        private void GetChildContainersHierarchy(ContainerCollection containers, ContainerCollection childContainer, Int32 containerId, Boolean includeSelf)
        {
            ContainerCollection containerCollection = new ContainerCollection();

            foreach (Container container in containers)
            {
                if (container.Id == containerId && includeSelf)
                {
                    childContainer.Add(container);
                }

                if (container.ParentContainerId == containerId && container.WorkflowType == WorkflowType.InheritParent)
                {
                    containerCollection.Add(container);
                    childContainer.Add(container);
                }
            }

            if (containerCollection.Count > 0)
            {
                foreach (Container container in containerCollection)
                {
                    GetChildContainersHierarchy(containers, childContainer, container.Id, false);
                }
            }
        }

        /// <summary>
        /// Validates the parameters. Returns the base entity as well based on the entity id passed.
        /// Updates the operation result details with the validations.
        /// </summary>
        /// <param name="entityId">The entity identifier.</param>
        /// <param name="operationResult">The operation result.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="callerContext">The caller context.</param>
        private void ValidateParameters(Int64 entityId, OperationResult operationResult, out Entity entity, out Boolean isMasterCollaboration, CallerContext callerContext)
        {
            entity = null;
            isMasterCollaboration = false;
            Object[] parameters = null;
            String message = String.Empty;

            if (entityId < 0)
            {
                parameters = new Object[] { entityId.ToString() };
                message = String.Format("Unable to initiate promote for entity {0}, as the entity identifier must be greater than zero.", parameters);

                operationResult.AddOperationResult("114292", message, parameters, OperationResultType.Error);
                operationResult.OperationResultStatus = OperationResultStatusEnum.Failed;
                return;
            }

            entity = GetBaseEntity(entityId).FirstOrDefault();

            if (entity == null)
            {
                parameters = new Object[] { entityId.ToString() };
                message = String.Format("Unable to initiate promote for entity identifier {0}, as it does not exist in the system.", parameters);

                operationResult.AddOperationResult("114290 ", message, parameters, OperationResultType.Error);
                operationResult.OperationResultStatus = OperationResultStatusEnum.Failed;
                return;
            }

            IAttribute lifeCycleAttribute = entity.GetAttribute((Int32)SystemAttributes.LifecycleStatus);

            if (lifeCycleAttribute != null)
            {
                String lifeCycleAttributeStatus = lifeCycleAttribute.GetCurrentValue().ToString();
                if (String.Compare(lifeCycleAttributeStatus, LifecycleStatusValues.MarkedForDeletion) == 0)
                {
                    parameters = new Object[] { entityId.ToString(), LifecycleStatusValues.MarkedForDeletion };
                    message = String.Format("Unable to initiate promote for entity identifier {0}, as it is {1}.", parameters);

                    operationResult.AddOperationResult("114581", message, parameters, OperationResultType.Error);
                    operationResult.OperationResultStatus = OperationResultStatusEnum.Failed;
                    return;
                }
            }

            if (entity.HierarchyLevel != Constants.VARIANT_LEVEL_ONE)
            {
                parameters = new Object[] { entity.LongName };
                message = String.Format("Unable to initiate promote for entity {0}, as only the root entity of the container can be promoted.", parameters);

                operationResult.AddOperationResult("114294", message, parameters, OperationResultType.Error);
                operationResult.OperationResultStatus = OperationResultStatusEnum.Failed;
                return;
            }

            Collection<Int32> containerIdList = new Collection<int>() { entity.ContainerId, entity.ParentExtensionEntityContainerId };
            ContainerBL containerBL = new ContainerBL();
            ContainerCollection containers = containerBL.GetByIds(containerIdList, callerContext, false);

            if (entity.ParentExtensionEntityId > 0)
            {
                Entity extensionParentEntity = GetBaseEntity(entity.ParentExtensionEntityId).FirstOrDefault();
                Container extensionParentContainer = containers.GetContainer(extensionParentEntity.ContainerId);

                if ((extensionParentContainer != null && extensionParentContainer.ContainerType != ContainerType.Upstream) && 
                    extensionParentEntity != null && extensionParentEntity.CrossReferenceId < 1)
                {
                    parameters = new Object[] { entity.LongName };
                    message = String.Format("Unable to initiate promote for entity '{0}', as the parent extension entity is not yet promoted.", parameters);

                    operationResult.AddOperationResult("114437", message, parameters, OperationResultType.Error);
                    operationResult.OperationResultStatus = OperationResultStatusEnum.Failed;
                    return;
                }
            }

            Container currentContainer = containers.GetContainer(entity.ContainerId);
            isMasterCollaboration = (currentContainer.ContainerType == ContainerType.MasterCollaboration);

            if (!currentContainer.NeedsApprovedCopy)
            {
                parameters = new Object[] { entity.LongName, currentContainer.LongName };
                message = String.Format("Unable to initiate promote for entity '{0}', as the container '{1}' is not configured to have approved copy.", parameters);

                operationResult.AddOperationResult("114439", message, parameters, OperationResultType.Error);
                operationResult.OperationResultStatus = OperationResultStatusEnum.Failed;
                return;
            }

            if (currentContainer.ContainerType == ContainerType.MasterApproved || currentContainer.ContainerType == ContainerType.ExtensionApproved)
            {
                parameters = new Object[] { entity.LongName };
                message = String.Format("Unable to initiate promote for entity {0}, as the container type of the entity is invalid.", parameters);

                operationResult.AddOperationResult("114289", message, parameters, OperationResultType.Error);
                operationResult.OperationResultStatus = OperationResultStatusEnum.Failed;
                return;
            }

            if (currentContainer.WorkflowType == WorkflowType.InheritParent)
            {
                parameters = new Object[] { entity.LongName };
                message = String.Format("Unable to initiate promote for entity {0}, as the container of the entity has workflow type set as 'Inherit Parent'.", parameters);

                operationResult.AddOperationResult("114291", message, parameters, OperationResultType.Error);
                operationResult.OperationResultStatus = OperationResultStatusEnum.Failed;
                return;
            }
        }

        /// <summary>
        /// Gets the base entity.
        /// </summary>
        /// <param name="entityId">The entity identifier.</param>
        /// <returns></returns>
        private EntityCollection GetBaseEntity(Int64 entityId)
        {
            EntityContext entityContext = new EntityContext();
            entityContext.LoadEntityProperties = true;
            entityContext.LoadAttributes = true;
            entityContext.AttributeIdList = new Collection<Int32>() { (Int32)SystemAttributes.LifecycleStatus };

            EntityGetOptions entityGetOptions = PrepareEntityGetOptions(true);

            return _entityManager.Get(new Collection<Int64> { entityId }, entityContext, entityGetOptions, _callerContext);
        }

        /// <summary>
        /// Prepares the entity get options.
        /// </summary>
        /// <param name="forBasePropertiesOnly">Will set the fill options related to attributes as false when this flag is true</param>
        /// <returns>Entity Get Options</returns>
        private EntityGetOptions PrepareEntityGetOptions(Boolean forBasePropertiesOnly)
        {
            EntityGetOptions groupEntityGetOptions = new EntityGetOptions();
            groupEntityGetOptions.ApplyAVS = false;
            groupEntityGetOptions.ApplySecurity = false;
            groupEntityGetOptions.PublishEvents = false;
            groupEntityGetOptions.UpdateCache = false;
            groupEntityGetOptions.UpdateCacheStatusInDB = false;
            groupEntityGetOptions.LoadLatestFromDB = true;

            if (forBasePropertiesOnly)
            {
                groupEntityGetOptions.FillOptions.FillLookupDisplayValues = false;
                groupEntityGetOptions.FillOptions.FillLookupRowWithValues = false;
                groupEntityGetOptions.FillOptions.FillRelationshipProperties = false;
                groupEntityGetOptions.FillOptions.FillUOMValues = false;
            }

            return groupEntityGetOptions;
        }

        /// <summary>
        ///  Performs scan and filter of entity family queue collection based on results and returns boolean flag to continue process.
        /// </summary>
        /// <param name="entityFamilyQueues">>Specifies all entity family queue collection</param>
        /// <param name="operationResults">Specifies  operation result collection</param>
        /// <returns>'true' if operation is successful;otherwise 'false'</returns>
        private Boolean ScanAndFilterEntityFamilyQueueBasedOnResults(EntityFamilyQueueCollection entityFamilyQueues, OperationResultCollection operationResults)
        {
            Boolean continueProcess = true;

            if (operationResults.OperationResultStatus == OperationResultStatusEnum.Failed)
            {
                //Operation result says 'Failed'. Stop processing
                continueProcess = false;
            }
            else if (operationResults.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors)
            {
                //Behavior says process partial.. Filter out the failed ones and continue with succeeded ones..
                IEnumerable<OperationResult> failedOperationResults = operationResults.Where(eor => eor.OperationResultStatus == OperationResultStatusEnum.Failed);

                foreach (OperationResult operationResult in failedOperationResults)
                {
                    if (entityFamilyQueues != null)
                    {
                        EntityFamilyQueue entityGruopQueueInFilteredCollection = entityFamilyQueues.SingleOrDefault(e => e.EntityFamilyId.ToString() == operationResult.ReferenceId);

                        if (entityGruopQueueInFilteredCollection != null)
                        {
                            entityFamilyQueues.Remove(entityGruopQueueInFilteredCollection);
                        }
                    }
                }

                if (entityFamilyQueues != null)
                {
                    if (entityFamilyQueues.Count < 1)
                    {
                        continueProcess = false;
                    }
                }
            }

            return continueProcess;
        }

        /// <summary>
        /// Gets the entity hierarchy.
        /// </summary>
        /// <param name="entityId">The entity identifier.</param>
        /// <param name="containerId">The container identifier.</param>
        /// <param name="mdmRuleParams">The MDM rule parameters.</param>
        /// <param name="promoteType">Indicates the type of promote.</param>
        /// <param name="currentContainer">The current container.</param>
        /// <param name="diagnosticActivity">The diagnostic activity.</param>
        /// <returns></returns>
        private Entity GetEntityHierarchy(Int64 entityId, Int32 containerId, EntityActivityList promoteType, MDMRuleParams mdmRuleParams, out Container currentContainer, Collection<Int32> attributeIds, DiagnosticActivity diagnosticActivity)
        {
            ContainerCollection promotableContainers = GetPromotableContainerList(containerId, promoteType, out currentContainer);

            if (_isTracingEnabled)
            {
                diagnosticActivity.LogInformation(String.Format("Found {0} promotable containers for entity id {1} and container id {2}", promotableContainers.Count, entityId, containerId));
            }

            EntityContext entityContext = new EntityContext()
            {
                LoadAttributes = false,
                LoadEntityProperties = true,
                EntityHierarchyContext = new EntityHierarchyContext(),
                ContainerId = containerId
            };

            if (attributeIds == null)
            {
                attributeIds = new Collection<Int32>();
                attributeIds.Add((Int32)SystemAttributes.LifecycleStatus);
            }
            else
            {
                attributeIds.Add((Int32)SystemAttributes.LifecycleStatus);
            }

            if (attributeIds != null && attributeIds.Count > 0)
            {
                entityContext.LoadAttributes = true;
                entityContext.AttributeIdList = attributeIds;
            }

            EntityTypeCollection entityTypes = null;
            EntityOperationsBL entityOperationsBL = new EntityOperationsBL();
            Dictionary<Int32, Int32> entityTypeIdToVariantLevelMappings = entityOperationsBL.GetEntityVariantLevel(entityId, _callerContext);

            if (entityTypeIdToVariantLevelMappings != null && entityTypeIdToVariantLevelMappings.Count > 0)
            {
                Collection<Int32> entityTypeIdList = new Collection<Int32>();

                foreach (KeyValuePair<Int32, Int32> keyValuePair in entityTypeIdToVariantLevelMappings)
                {
                    entityTypeIdList.Add(keyValuePair.Key);
                }

                EntityTypeBL entityTypeManager = new EntityTypeBL();
                entityTypes = entityTypeManager.GetEntityTypesByIds(entityTypeIdList);
            }

            EntityContext preLoadEntityContext = null;
            if (mdmRuleParams != null)
            {
                //preLoadEntityContext = PreLoadContextHelper.GetEntityContext(mdmRuleParams, _entityManager);
                //EntityContextHelper.PopulateIdsInEntityContext(preLoadEntityContext, _entityManager, _callerContext);

                //UpdateEntityContext(preLoadEntityContext, entityContext);
            }
            else
            {
                entityContext.LoadStateValidationAttributes = true;
                entityContext.LoadBusinessConditions = true;
            }


            if (entityTypes != null && entityTypes.Count > 1)
            {
                foreach (EntityType entityType in entityTypes)
                {
                    Int32 entityTypeId = entityType.Id;
                    String entityTypeName = entityType.Name;

                    Int32 variantLevel = entityTypeIdToVariantLevelMappings[entityTypeId];

                    if (variantLevel == Constants.VARIANT_LEVEL_ONE)
                    {
                        entityContext.EntityTypeId = entityTypeId;
                        entityContext.EntityTypeName = entityTypeName;
                    }
                    else
                    {
                        EntityContext childEntityContext = new EntityContext();
                        childEntityContext.EntityTypeId = entityTypeId;
                        childEntityContext.EntityTypeName = entityTypeName;
                        childEntityContext.LoadAttributes = entityContext.LoadAttributes;
                        childEntityContext.AttributeIdList = entityContext.AttributeIdList;
                        childEntityContext.LoadEntityProperties = true;

                        if (preLoadEntityContext != null)
                        {
                            UpdateEntityContext(preLoadEntityContext, childEntityContext);
                        }
                        else
                        {
                            childEntityContext.LoadStateValidationAttributes = true;
                            childEntityContext.LoadBusinessConditions = true;
                        }

                        entityContext.EntityHierarchyContext.AddEntityDataContext(entityTypeName, childEntityContext);
                    }
                }
            }

            EntityContextCollection contextCollection = new EntityContextCollection();

            foreach (Container container in promotableContainers)
            {
                EntityContext containerEntityContext = new EntityContext
                {
                    ContainerId = container.Id,
                    ContainerName = container.Name,
                    AttributeIdList = entityContext.AttributeIdList,
                    LoadAttributes = entityContext.LoadAttributes
                };

                foreach (EntityContext childHierarchyContext in entityContext.EntityHierarchyContext.EntityContexts)
                {
                    EntityContext clonedContext = (EntityContext)(childHierarchyContext.Clone());
                    containerEntityContext.EntityHierarchyContext.AddEntityDataContext(clonedContext.EntityTypeName, clonedContext);
                }

                contextCollection.Add(containerEntityContext);
            }

            entityContext.EntityExtensionContext.SetEntityContexts(contextCollection);

            EntityGetOptions entityGetOptions = PrepareEntityGetOptions(true);

            return _entityManager.GetEntityHierarchy(new EntityUniqueIdentifier(entityId), entityContext, entityGetOptions, _callerContext);
        }

        /// <summary>
        /// Changes the reason type to system exception.
        /// </summary>
        /// <param name="entityOperationResultCollection">The entity operation result collection.</param>
        private void ChangeReasonTypeToSystemException(EntityOperationResultCollection entityOperationResultCollection)
        {
            foreach (EntityOperationResult entityOperationResult in entityOperationResultCollection)
            {
                ChangeReasonTypeToSystemException(entityOperationResult);
            }
        }

        /// <summary>
        /// Changes the reason type to system exception.
        /// </summary>
        /// <param name="entityOperationResult">The entity operation result.</param>
        private void ChangeReasonTypeToSystemException(EntityOperationResult entityOperationResult)
        {
            foreach (Error error in entityOperationResult.Errors)
            {
                error.ReasonType = ReasonType.SystemError;
            }
        }

        /// <summary>
        /// Updates the entity context.
        /// </summary>
        /// <param name="preLoadEntityContext">The pre load entity context.</param>
        /// <param name="entityContext">The entity context.</param>
        private void UpdateEntityContext(EntityContext preLoadEntityContext, EntityContext entityContext)
        {
            if (preLoadEntityContext != null)
            {
                entityContext.AttributeIdList.AddRange<Int32>(preLoadEntityContext.AttributeIdList);
                entityContext.DataLocales.AddRange<LocaleEnum>(preLoadEntityContext.DataLocales);

                Collection<Int32> relationshipTypeIdList = preLoadEntityContext.RelationshipContext.RelationshipTypeIdList;

                if (relationshipTypeIdList != null && relationshipTypeIdList.Count > 0)
                {
                    entityContext.RelationshipContext.RelationshipTypeIdList.AddRange<Int32>(relationshipTypeIdList);

                    if (preLoadEntityContext.RelationshipContext.LoadRelationshipAttributes)
                    {
                        entityContext.RelationshipContext.LoadRelationshipAttributes = true;
                        entityContext.RelationshipContext.DataLocales = preLoadEntityContext.RelationshipContext.DataLocales;
                    }
                }

                if (entityContext.AttributeIdList.Count > 0)
                {
                    entityContext.LoadAttributes = true;
                }

                if (entityContext.RelationshipContext.RelationshipTypeIdList.Count > 0)
                {
                    entityContext.LoadRelationships = true;
                }
            }
        }

        /// <summary>
        /// Queues the export qualified items.
        /// </summary>
        /// <param name="familyId">The family identifier.</param>
        /// <param name="promoteType">Type of the promote.</param>
        /// <param name="diagnosticActivity">The diagnostic activity.</param>
        /// <exception cref="MDMOperationException">Failed to save export qualified items.</exception>
        private void QueueExportQualifiedItems(Int64 familyId, EntityActivityList promoteType, DiagnosticActivity diagnosticActivity)
        {
            //ExportQualificationHelper qualificationHelper = new ExportQualificationHelper();
            //Int64 promoteEntityRootId = familyId;
            //DateTime exportStartTime = DateTime.Now; // TODO: get more accurate export start time from outside, if necessary

            //Entity promotedBaseEntity = GetBaseEntity(promoteEntityRootId).FirstOrDefault();
            //Container currentContainer;

            //EntityExportProfileBL exportProfileBL = new EntityExportProfileBL();
            //EntityExportProfileCollection exportProfileCollection = exportProfileBL.Get(ExportProfileType.EntityExportSyndicationProfile, _callerContext);
            //ExportProfileCollection filteredExportProfiles = new ExportProfileCollection();

            //#region Filter Delta Profiles Based on Approved or Collaboration

            //filteredExportProfiles = qualificationHelper.GetFilteredExportProfiles(exportProfileCollection, false);

            //#endregion Filter Delta Profiles Based on Approved or Collaboration

            //if (filteredExportProfiles != null && filteredExportProfiles.Count > 0)
            //{
            //    Collection<Int32> attrIds = filteredExportProfiles.GetAttributeFilterIds();

            //    Entity promotedEntityHierarchy = GetEntityHierarchy(promoteEntityRootId, promotedBaseEntity.ContainerId, promoteType, null, out currentContainer, attrIds, diagnosticActivity);

            //    ExportQueueCollection exportQueueItems = qualificationHelper.GetExportQueueItems((EntityCollection)promotedEntityHierarchy.GetFlattenEntities(), filteredExportProfiles, _callerContext);
            //    if (exportQueueItems != null && exportQueueItems.Count > 0)
            //    {
            //        IExportQueueManager exportQueueManager = ServiceLocator.Current.GetInstance(typeof(IExportQueueManager)) as IExportQueueManager;
            //        OperationResultCollection queueResults = exportQueueManager.Process(exportQueueItems, false, _callerContext);

            //        if (queueResults == null || queueResults.OperationResultStatus != OperationResultStatusEnum.Successful)
            //        {
            //            throw new MDMOperationException("Failed to save export qualified items.");
            //        }
            //    }

            //    //#region Send an queued entities to Jigsaw

            //    //if (JigsawConstants.IsJigsawIntegrationEnabled)
            //    //{
            //    //    JigsawEntityContext jigsawEntityContext = new JigsawEntityContext
            //    //    {
            //    //        User = _securityPrincipal.CurrentUserName,
            //    //        CallerProcessType = JigsawCallerProcessType.PromoteEvent,
            //    //        IntegrationAppType = JigsawIntegrationAppName.manageGovernApp
            //    //    };

            //    //    var exportProfileName = exportProfileCollection.First().Name;
            //    //    jigsawEntityContext.JigsawBatchContext = new JigsawBatchContext
            //    //    {
            //    //        EventInfo = new MDM.JigsawIntegrationManager.JigsawHelpers.EventInfo
            //    //        {
            //    //            ExportStartTime = exportStartTime,
            //    //            EventGroupId = familyId.ToString(),
            //    //            EventType = _callerContext.Module.ToString(),
            //    //            EventSubType = String.Empty,
            //    //            Source = _callerContext.Application.ToString(),
            //    //            ExportProfile = exportProfileName
            //    //        },
                        
            //    //    };

            //    //    try
            //    //    {
            //    //        IEnumerable<Entity> entities = promotedEntityHierarchy.GetFlattenEntities();
            //    //        String batchDataJson = JigsawTransformer.ToJigsawJson(entities, _callerContext, jigsawEntityContext);
            //    //        IIntegrationMessageBroker messageBrokerManager = MessageBrokerFactory.GetMessageBrokerManager(JigsawCallerProcessType.PromoteEvent);

            //    //        messageBrokerManager.SendEntity(batchDataJson);
            //    //    }
            //    //    catch (Exception e)
            //    //    {
            //    //        diagnosticActivity.LogError(String.Format("Failed to send promoted entities to Jigsaw. The following exception has occurred: {0}", e.Message));
            //    //    }
            //    //}

            //    //#endregion
            //}
        }

        #endregion Helpers
    }
}