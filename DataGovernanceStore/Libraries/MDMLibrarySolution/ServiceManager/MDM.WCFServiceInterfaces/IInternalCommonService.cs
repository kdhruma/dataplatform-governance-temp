using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ServiceModel;

namespace MDM.WCFServiceInterfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.BusinessObjects.DataModel;
    using MDM.BusinessObjects.Exports;
    using MDM.BusinessObjects.MergeCopy;

    /// <summary>
    /// Defines operation contracts for MDM Internal common operations
    /// </summary>
    [ServiceContract(Namespace = "http://wcfservices.riversand.com")]
    public interface IInternalCommonService
    {
        #region Entity Methods

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityPaginationResult GetEntitiesByIdsAndPagination(Collection<Int64> entityIdList, EntityContext entityContext, PagingCriteria pagingCriteria, CallerContext callerContext, Boolean publishEvents, Boolean applyAVS);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityPaginationResult GetEntitiesByPagination(EntityContext entityContext, PagingCriteria pagingCriteria, CallerContext callerContext, Boolean publishEvents, Boolean applyAVS);

        [OperationContract(Name = "GetEntitiesByEntityScopes")]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityReadResult GetEntitiesByEntityScopes(EntityScopeCollection entityScopeCollection, EntityGetOptions entityGetOptions, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityOperationResult CreateEntityHavingImageAttributes(Entity entity, Dictionary<KeyValuePair<Int32, LocaleEnum>, MDM.BusinessObjects.File> imageDetails, String programName, MDMCenterApplication application, MDMCenterModules module);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityOperationResult UpdateEntityHavingImageAttributes(Entity entity, Dictionary<KeyValuePair<Int32, LocaleEnum>, MDM.BusinessObjects.File> imageDetails, String programName, MDMCenterApplication application, MDMCenterModules module);
        
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Boolean IsEntityExists(Entity entity, Int64 entityId, Int32 catalogId, MDMCenterApplication application, MDMCenterModules module);

        #endregion Entity Methods

        #region DataModel Methods

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        File ExportDataModelAsExcel(DataModelExportContext dataModelExportContext, CallerContext callerContext);

        #endregion DataModel Methods

        #region Category Methods

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        CategoryCollection SearchCategories(Collection<String> containerNames, Collection<String> categoryLongNames, Collection<LocaleEnum> dataLocales, CallerContext callerContext);

        #endregion Category Methods

        #region Entity Hierarchy Methods

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Table ReclassifyLegacy(String dataXml, String userLogin, Boolean isCategoryReclassify);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Boolean IsEntityHierarchyMatrixLatest(Int64 entityid, Int32 entityHierarchyDefinitionId, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Dictionary<Int32, EntityType> GetEntityVariantLevels(Int64 entityId, CallerContext callerContext);

        #endregion
        
        #region File Methods

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        File GetImportTemplateFileByImportProfileName(String importProfileName, CallerContext callerContext);

        #endregion

        #region Queued Entity

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        QueuedEntityCollection GetQueuedEntities(EntityActivityList entityActivity, Int64 entityActivityLogId, CallerContext callerContext);

        #endregion Queued Entity

        #region MDMEvents and MDMEvent Handlers Interfaces

        /// <summary>
        /// Get all the MDMEvents from the system
        /// </summary>
        /// <param name="callerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Returns the list of MDMEvents</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        MDMEventInfoCollection GetMDMEventInformation(CallerContext callerContext);

        /// <summary>
        /// Get all the MDMEvent Handlers from the system
        /// </summary>
        /// <param name="eventHandlerIdList">Indicates the list of event Handler Ids</param>
        /// <param name="callerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Returns the list of MDMEvent Handlers</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        MDMEventHandlerCollection GetMDMEventHandlers(Collection<Int32> eventHandlerIdList, CallerContext callerContext);

        /// <summary>
        /// Process the requested MDMEvent Handler based on their actions.
        /// </summary>
        /// <param name="mdmEventHandlerCollection">Indicates the list of MDMEvnet Handlers</param>
        /// <param name="callerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Returns the operation results</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResultCollection ProcessMDMEventHandlers(MDMEventHandlerCollection mdmEventHandlerCollection, CallerContext callerContext);

        #endregion MDMEvents and MDMEvent Handlers Methods

        #region Relationship type mapping related Interfaces

        /// <summary>
        /// Get container relationship type entity type mapping based on given context
        /// </summary>
        /// <param name="entityModelContext">Indicates the entity model context based on which mappings to be returned.</param>
        /// <param name="callerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Returns the container relationship type entity type mappings based on entity model context.</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        ContainerRelationshipTypeEntityTypeMappingCollection GetContainerRelationshipTypeEntityTypeMappings(EntityModelContext entityModelContext, CallerContext callerContext);

        #endregion
        
        #region Entity State Validation Methods

        /// <summary>
        /// Gets the state of the entity validation for given entity id list and its family members
        /// </summary>
        /// <param name="entityIds">Indicates the entity ids</param>
        /// <param name="needGlobalFamilyErrors">Indicates whether to return states for global family members or not</param>
        /// <param name="needVariantFamilyErrors">Indicates whether to return states for variant family members or not</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Returns collection of entity state validation based on provided entity id list</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityStateValidationCollection GetEntitiesStateValidation(Collection<Int64> entityIds, Boolean needGlobalFamilyErrors, Boolean needVariantFamilyErrors, CallerContext callerContext);

        /// <summary>
        /// Enqueue for re-validate process for given entity family queue
        /// </summary>
        /// <param name="entityFamilyQueue">Indicates entity family queue for which re-validation process is required</param>
        /// <param name="callerContext">Indicates application and module name of caller</param>
        /// <returns>Returns operation result of current operation</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult EnqueueForRevalidate(EntityFamilyQueue entityFamilyQueue, CallerContext callerContext);

        /// <summary>
        /// Gets business conditions for given entity id list
        /// </summary>
        /// <param name="entityIds">Indicates the entity ids</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Returns collection of entity business conditions based on provided entity id list</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityBusinessConditionCollection GetEntityBusinessConditions(Collection<Int64> entityIds, CallerContext callerContext);

        #endregion Entity State Validation Methods

        #region Entity Score Methods

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityStateValidationScoreCollection GetEntityScores(Collection<Int64> entityIds, CallerContext callerContext);

        #endregion Entity Score Methods

        #region Entity Activity Log

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityActivityLogCollection GetEntityActivityLogs(EntityActivityList entityActivityList, ProcessingStatus processingStatus, Int64 fromRecordNumber, Int64 toRecordNumber, CallerContext callerContext);

        #endregion

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        AttributeModelCollection GetMappedAttributeModelsByContainers(Collection<Int32> containerIdList, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityOperationResultCollection PerformBulkWorkflowOperation(Collection<Int64> entityIdList, String activityLongName, String workflowAction, String comments, String operationType, SecurityUser newlyAssignedUser, CallerContext callerContext);
    }
}
