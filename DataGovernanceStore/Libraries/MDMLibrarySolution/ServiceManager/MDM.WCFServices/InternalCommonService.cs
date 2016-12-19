using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.ServiceModel;

namespace MDM.WCFServices
{
    using MDM.AdminManager.Business;
    using MDM.AttributeManager.Business;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.DataModel;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.BusinessRuleManagement.Business;
    using MDM.CategoryManager.Business;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.DataModelExport.Business;
    using MDM.DataModelManager.Business;
    using MDM.EntityManager.Business;
    using MDM.HierarchyManager.Business;
    using MDM.Interfaces;
    using MDM.MessageManager.Business;
    using MDM.BusinessObjects.Exports;
    using MDM.AttributeModelManager.Business;
    using MDM.Utility;
    using MDM.WCFServiceInterfaces;
    using MDM.ActivityLogManager.Business;
    using MDM.EntityWorkflowManager.Business;

    [DiagnosticActivity]
    [ServiceBehavior(Namespace = "http://wcfservices.riversand.com", InstanceContextMode = InstanceContextMode.PerCall)]
    public class InternalCommonService : MDMWCFBase, IInternalCommonService
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public InternalCommonService()
            : base(true)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loadSecurityPrincipal"></param>
        public InternalCommonService(Boolean loadSecurityPrincipal)
            : base(loadSecurityPrincipal)
        {

        }

        #endregion Constructors

        #region Methods

        #region Public Methods

        #region Entity Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityIdList"></param>
        /// <param name="entityContext"></param>
        /// <param name="pagingCriteria"></param>
        /// <param name="callerContext"></param>
        /// <param name="publishEvents"></param>
        /// <param name="applyAVS"></param>
        /// <returns></returns>
        public EntityPaginationResult GetEntitiesByIdsAndPagination(Collection<long> entityIdList, EntityContext entityContext, PagingCriteria pagingCriteria, CallerContext callerContext, bool publishEvents, bool applyAVS)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
            {
                MDMTraceHelper.StartTraceActivity("InternalCommonService.GetEntitiesByIdsAndPagination", MDMTraceSource.EntityGet, false);
            }

            EntityCollection entityCollection = null;

            try
            {
                if (isTracingEnabled) MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "InternalCommonService receives 'GetEntitiesByIdsAndPagination' request message.", MDMTraceSource.EntityGet);

                EntityBL entityManager = new EntityBL();

                Boolean loadLatest = false;
                entityCollection = entityManager.Get(entityIdList, entityContext, loadLatest,
                    callerContext != null ? callerContext.Application : MDMCenterApplication.MDMCenter,
                    callerContext != null ? callerContext.Module : MDMCenterModules.Unknown, publishEvents, applyAVS);

                if (isTracingEnabled) MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "InternalCommonService sends 'GetEntitiesByIdsAndPagination' response message.", MDMTraceSource.EntityGet);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            finally 
            {
                if (isTracingEnabled)
                {
                    MDMTraceHelper.StopTraceActivity("InternalCommonService.GetEntitiesByIdsAndPagination", MDMTraceSource.EntityGet);
                }
            }

            return new EntityPaginationResult { EntityList = entityCollection };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityContext"></param>
        /// <param name="pagingCriteria"></param>
        /// <param name="callerContext"></param>
        /// <param name="publishEvents"></param>
        /// <param name="applyAVS"></param>
        /// <returns></returns>
        public EntityPaginationResult GetEntitiesByPagination(EntityContext entityContext, PagingCriteria pagingCriteria, CallerContext callerContext, bool publishEvents, bool applyAVS)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
            {
                MDMTraceHelper.StartTraceActivity("InternalCommonService.GetEntitiesByPagination", MDMTraceSource.EntityGet, false);
            }

            EntityCollection entityCollection = null;

            try
            {
                if (isTracingEnabled) MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "InternalCommonService receives 'GetEntitiesByPagination' request message.", MDMTraceSource.EntityGet);

                EntityBL entityManager = new EntityBL();

                Boolean loadLatest = false;
                entityCollection = entityManager.Get(null, entityContext, loadLatest,
                    callerContext != null ? callerContext.Application : MDMCenterApplication.MDMCenter,
                    callerContext != null ? callerContext.Module : MDMCenterModules.Unknown, publishEvents, applyAVS);

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "InternalCommonService sends 'GetEntitiesByPagination' response message.", MDMTraceSource.EntityGet);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            finally 
            {
                if (isTracingEnabled)
                {
                    MDMTraceHelper.StopTraceActivity("InternalCommonService.GetEntitiesByPagination", MDMTraceSource.EntityGet);
                }
            }

            return new EntityPaginationResult { EntityList = entityCollection };
        }

        /// <summary>
        /// Gets entity objects for the requested entity scopes
        /// </summary>
        /// <param name="entityScopeCollection">EntityScopes for which entity objects are required</param>
        /// <param name="entityGetOptions">Options to retrieve entities</param>
        /// <param name="callerContext">Specifies the caller context, which contains the application and module that has invoked the API</param>
        /// <returns>EntityReadResult containing EntityCollection for given Entity Ids based on EntityScope and failed operationresult collection</returns>
        /// <exception cref="MDMOperationException">Thrown if EntityId is less than 0</exception>
        /// <exception cref="MDMOperationException">Thrown if EntityContext is null</exception>
        /// <exception cref="MDMOperationException">Thrown if none of the EntityContext's properties like LoadEntityProperties, LoadAttributes or LoadRelationships is set to true</exception>
        public EntityReadResult GetEntitiesByEntityScopes(EntityScopeCollection entityScopeCollection, EntityGetOptions entityGetOptions, CallerContext callerContext)
        {
            EntityReadResult entityReadResult = null;

            TraceSettings traceSettings = null;

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            try
            {
                ValidateCallerContext(callerContext, "InternalCommonService", "GetEntitiesByEntityScopes");
                traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

                if (traceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                if (traceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogInformation("DataService receives 'GetEntitiesInParallel' request message.");
                }

                EntityBL entityManager = new EntityBL();

                entityReadResult = entityManager.Get(entityScopeCollection, entityGetOptions, callerContext);

            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            finally
            {
                if (traceSettings != null && traceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return entityReadResult;
        }

        /// <summary>
        /// Creates entity with images
        /// </summary>
        /// <param name="entity">entity object with attribute value</param>
        /// <param name="imageDetails">dictionary with image details </param>
        /// <param name="programName"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public EntityOperationResult CreateEntityHavingImageAttributes(Entity entity, Dictionary<KeyValuePair<Int32, LocaleEnum>, MDM.BusinessObjects.File> imageDetails, String programName, MDMCenterApplication application, MDMCenterModules module)
        {
            return MakeBusinessLogicCall<EntityBL, EntityOperationResult>("CreateEntityHavingImageAttributes", businessLogic => businessLogic.Create(entity, imageDetails, programName, application, module), MDMTraceSource.EntityProcess);
        }

        /// <summary>
        /// Updates entity with Images
        /// </summary>
        /// <param name="entity">entity object with attributes</param>
        /// <param name="imageDetails">image details in dictionary</param>
        /// <param name="programName"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public EntityOperationResult UpdateEntityHavingImageAttributes(Entity entity, Dictionary<KeyValuePair<Int32, LocaleEnum>, MDM.BusinessObjects.File> imageDetails, String programName, MDMCenterApplication application, MDMCenterModules module)
        {
            return MakeBusinessLogicCall<EntityBL, EntityOperationResult>("UpdateEntityHavingImageAttributes", businessLogic => businessLogic.Update(entity, imageDetails, programName, application, module), MDMTraceSource.EntityProcess);
        }

        /// <summary>
        /// Gets the completion status for entity views based on criterion defined in the entity view xml
        /// </summary>
        /// <param name="entityId">Id of the entity for which status needs to be determined</param>
        /// <param name="userId">User Id for which status needs to be determined</param>
        /// <param name="entityViewXml">Entity editor left panel config xml containing views and the completion criteria</param>
        /// <param name="entityContext">Context parameters of the entity</param>
        /// <param name="isRecalculationRequired">Flag which says whether the completion status needs to be recalculated or needs to be get from the cache</param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns>Collection of entity views with the completion status</returns>
        public EntityViewCollection GetEntityViewCompletionStatus(Int64 entityId, Int32 userId, String entityViewXml, EntityContext entityContext, Boolean isRecalculationRequired, MDMCenterApplication application, MDMCenterModules module)
        {
            return MakeBusinessLogicCall<EntityViewBL, EntityViewCollection>("GetEntityViewCompletionStatus", businessLogic => businessLogic.GetEntityViewCompletionStatus(entityId, userId, entityViewXml, entityContext, isRecalculationRequired, application, module), MDMTraceSource.EntityGet);
        }

        /// <summary>
        /// Check the if Entity exists or not.
        /// </summary>
        /// <param name="entity">Entity object which needs to be checked for existence.</param>
        /// <param name="entityId">Indicates the Component Id </param>
        /// <param name="catalogId">Indicates the Catalog Id where to check for the Entity.</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>1 if given entity exists in given context. 0 otherwise</returns>
        public Boolean IsEntityExists(Entity entity, Int64 entityId, Int32 catalogId, MDMCenterApplication application, MDMCenterModules module)
        {
            return MakeBusinessLogicCall<EntityBL, Boolean>("IsEntityExists", businessLogic => businessLogic.Exists(entity, entityId, catalogId, application, module), MDMTraceSource.EntityGet);
        }

        /// <summary>
        /// Gets entity validation scores
        /// </summary>
        /// <param name="entityIds">Indicates entity ids to get their validation scores</param>
        /// <param name="callerContext">Indicates application and module name which is performing action</param>
        /// <returns></returns>
        public EntityStateValidationScoreCollection GetEntityScores(Collection<Int64> entityIds, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<EntityStateValidationBL, EntityStateValidationScoreCollection>("GetEntityScores", businessLogic => businessLogic.GetEntityStateValidationScores(entityIds, callerContext), MDMTraceSource.Entity);
        }

        #endregion Entity Methods

        #region Entity Hierarchy Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataXml"></param>
        /// <param name="userLogin"></param>
        /// <param name="isCategoryReclassify"></param>
        /// <returns></returns>
        public Table ReclassifyLegacy(String dataXml, String userLogin, Boolean isCategoryReclassify)
        {
            Table table = null;

            DataTable dataTable = MakeBusinessLogicCall<EntityOperationsBL, DataTable>("ReclassifyLegacy", businessLogic => businessLogic.Reclassify(dataXml, userLogin, isCategoryReclassify), MDMTraceSource.EntityProcess);

            if (dataTable != null)
            {
                table = new Table(dataTable);
            }

            return table;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityHierarchyDefinitionId"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public Boolean IsEntityHierarchyMatrixLatest(Int64 entityId, Int32 entityHierarchyDefinitionId, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<EntityHierarchyBL, Boolean>("IsEntityHierarchyMatrixLatest", businessLogic => businessLogic.IsLatestMatrix(entityId, entityHierarchyDefinitionId, callerContext), MDMTraceSource.EntityGet);
        }

        /// <summary>
        /// Gets entity variants level and entity types based on given entity id
        /// </summary>
        /// <param name="entityId">Indicates entity id for which entity variant level and entity type to be fetched</param>
        /// <param name="callerContext">Indicates the caller context</param>
        /// <returns>Returns collection of key value pair with key as variant level and value as entity type</returns>
        public Dictionary<Int32, EntityType> GetEntityVariantLevels(Int64 entityId, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<EntityOperationsBL, Dictionary<Int32, EntityType>>("GetEntityVariantLevels", businessLogic => businessLogic.GetEntityVariantLevels(entityId, callerContext), MDMTraceSource.EntityHierarchyGet);
        }

        #endregion

        #region DataModel Methods

        /// <summary>
        /// Generates an excel file with all data models present in the system.
        /// </summary>
        /// <param name="dataModelExportContext">Indicates data model export context</param>
        /// <param name="callerContext">Indicates context of the Caller</param>
        /// <returns>A file containing the data model</returns>
        public File ExportDataModelAsExcel(DataModelExportContext dataModelExportContext, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<DataModelExportBL, File>("ExportDataModelAsExcel", businessLogic => businessLogic.ExportDataModelAsExcel(dataModelExportContext, callerContext), MDMTraceSource.DataModelExport);
        }

        #endregion

        #region Category Methods

        /// <summary>
        /// Search Categories for requested context
        /// </summary>
        /// <param name="containerNames">Collection of container names in which the category belongs</param>
        /// <param name="categoryLongNames">Collection of category long names in which result has to be returned</param>
        /// <param name="dataLocales">DataLocales in which result has to be returned.</param>
        /// <param name="callerContext">Context indicating the caller of the API</param>
        /// <returns>Collection of Category</returns>
        public CategoryCollection SearchCategories(Collection<String> containerNames, Collection<String> categoryLongNames, Collection<LocaleEnum> dataLocales, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<CategoryBL, CategoryCollection>("SearchCategories", businessLogic => businessLogic.Search(containerNames, categoryLongNames, dataLocales, callerContext), MDMTraceSource.CategoryGet);
        }

        #endregion

        #region Attribute Version

        /// <summary>
        /// Get Version History of Attribute
        /// </summary>
        /// <param name="entityId">Specifies Id of Entity</param>
        /// <param name="entityParentId">Specifies ParentId of entity</param>
        /// <param name="attributeId">Specifies Id of Attribute</param>
        /// <param name="catalogId">Specifies Id of Catalog</param>
        /// <param name="locales">Specifies the data Locales</param>
        /// <param name="sequence">Specifies a sequence</param>
        /// <param name="callerContext">Specifies Caller context</param>
        /// <returns>AttributeVersionCollection</returns>
        public AttributeVersionCollection GetComplexAttributeVersions(Int64 entityId, Int64 entityParentId, Int32 attributeId, Int32 catalogId, Collection<LocaleEnum> locales, Int32 sequence, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<AttributeVersionBL, AttributeVersionCollection>("GetComplexAttributeVersions", businessLogic => businessLogic.GetComplexAttributeVersions(entityId, entityParentId, attributeId, catalogId, locales, sequence, callerContext), MDMTraceSource.EntityGet);
        }

        /// <summary>
        /// Get Complex Data for Complex Attribute's version history for requested attribute id
        /// </summary>
        /// <param name="entityId">EntityId for which attribute history is needed</param>
        /// <param name="containerId">Container Id under which Entity is created</param>
        /// <param name="attributeId">Attribute id for which we needs data</param>
        /// <param name="auditRefId">AuditRefId for which we needs data</param>
        /// <param name="locale">locale details</param>
        /// <param name="callerContext">Indicates the Caller Context</param>
        /// <returns>Attribute object with complex attribute's data</returns>
        public Attribute GetComplexDataByAuditRefId(Int64 entityId, Int32 containerId, Int32 attributeId, Int64 auditRefId, LocaleEnum locale, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<AttributeBL, Attribute>("GetComplexDataByAuditRefId", businessLogic => businessLogic.GetComplexDataByAuditRefId(entityId, containerId, attributeId, auditRefId, locale, callerContext), MDMTraceSource.EntityGet);
        }

        /// <summary>
        /// Get Hierarchical attribute at specific version of version history for requested auditRefId, attribute id, entity id
        /// </summary>
        /// <param name="entityId">EntityId for which attribute history is needed</param>
        /// <param name="containerId">Container Id under which Entity is created</param>
        /// <param name="attributeId">Attribute id for which we needs data</param>
        /// <param name="auditRefId">AuditRefId for which we needs data</param>
        /// <param name="locale">locale details</param>
        /// <param name="callerContext">Indicates the Caller Context</param>
        /// <returns>Hierarchical attribute at some specific point of history</returns>
        public Attribute GetHierarchicalAttributeByAuditRefId(long entityId, int containerId, int attributeId, long auditRefId,
            LocaleEnum locale, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<AttributeBL, Attribute>("GetHierarchicalAttributeByAuditRefId", businessLogic => businessLogic.GetHierarchicalAttributeByAuditRefId(entityId, containerId, attributeId, auditRefId, locale, callerContext), MDMTraceSource.EntityGet);
        }

        #endregion

        #region File Methods

        /// <summary>
        /// Gets Import templete file
        /// </summary>
        /// <param name="importProfileName">Import TemplateName</param>
        /// <param name="callerContext">Specifies Caller context</param>
        /// <returns>Returns file related to specified Import template</returns>
        public File GetImportTemplateFileByImportProfileName(String importProfileName, CallerContext callerContext)
        {
            File file = null;

            Template template = MakeBusinessLogicCall<ImportTemplateBL, Template>("GetImportTemplateFileByName", businessLogic => businessLogic.GetImportTemplateByName(importProfileName, callerContext), MDMTraceSource.APIFramework);

            if (template != null)
            {
                file = new File(template.Name, template.FileType, false, template.FileData);
            }

            return file;
        }

        #endregion

        #region Queued Entity

        /// <summary>
        /// Get queued entities by given entityActivityLogId
        /// </summary>
        /// <param name="entityActivity">Type of Activity performed</param>
        /// <param name="entityActivityLogId">PK of entityActivityLog table</param>
        /// <param name="callerContext">Context indicating who called the method</param>
        /// <returns>Collection of queued entities.</returns>
        public QueuedEntityCollection GetQueuedEntities(EntityActivityList entityActivity, Int64 entityActivityLogId, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<EntityQueueBL, QueuedEntityCollection>("GetQueuedEntities", businessLogic => businessLogic.Get(entityActivity, entityActivityLogId, callerContext), MDMTraceSource.DenormProcess);
        }

        public Boolean ProcessEntityActivityLog(EntityActivityLogCollection entityActivityLogCollection, CallerContext callerContext, ProcessingMode processingMode = ProcessingMode.Sync)
        {
            return MakeBusinessLogicCall<EntityActivityLogBL, Boolean>("ProcessEntityActivityLog", businessLogic => businessLogic.Process(entityActivityLogCollection, callerContext, processingMode), MDMTraceSource.DenormProcess);
        }

        #endregion Queued Entity

        #region MDMEvents and MDMEvent Handlers Methods

        /// <summary>
        /// Get all the MDMEvents from the system
        /// </summary>
        /// <param name="callerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Returns the list of MDMEvents</returns>
        public MDMEventInfoCollection GetMDMEventInformation(CallerContext callerContext)
        {
            return MakeBusinessLogicCall<MDMEventInfoBL, MDMEventInfoCollection>("GetMDMEventInformation", businessLogic => businessLogic.Get(callerContext), MDMTraceSource.Configuration);
        }

        /// <summary>
        /// Get all the MDMEvent Handlers from the system
        /// </summary>
        /// <param name="eventHandlerIdList">Indicates the list of event Handler Ids</param>
        /// <param name="callerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Returns the list of MDMEvent Handlers</returns>
        public MDMEventHandlerCollection GetMDMEventHandlers(Collection<Int32> eventHandlerIdList, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<MDMEventHandlerBL, MDMEventHandlerCollection>("Get", businessLogic => businessLogic.Get(eventHandlerIdList, callerContext), MDMTraceSource.Configuration);
        }

        /// <summary>
        /// Process the requested MDMEvent Handler based on their actions.
        /// </summary>
        /// <param name="mdmEventHandlerCollection">Indicates the list of MDMEvnet Handlers</param>
        /// <param name="callerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Returns the operation results</returns>
        public OperationResultCollection ProcessMDMEventHandlers(MDMEventHandlerCollection mdmEventHandlerCollection, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<MDMEventHandlerBL, OperationResultCollection>("Process", businessLogic => businessLogic.Process(mdmEventHandlerCollection, callerContext), MDMTraceSource.Configuration);
        }

        #endregion MDMEvents and MDMEvent Handlers Methods

        #region Relationship type mapping related Interfaces
       
        /// <summary>
        /// Get container relationship type entity type mapping based on given context
        /// </summary>
        /// <param name="entityModelContext">Indicates the entity model context based on which mappings to be returned.</param>
        /// <param name="callerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Returns the container relationship type entity type mappings based on entity model context.</returns>
        public ContainerRelationshipTypeEntityTypeMappingCollection GetContainerRelationshipTypeEntityTypeMappings(EntityModelContext entityModelContext, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<ContainerRelationshipTypeEntityTypeMappingBL, ContainerRelationshipTypeEntityTypeMappingCollection>("GetContainerRelationshipTypeEntityTypeMappings", businessLogic => businessLogic.GetMappingsByContext(entityModelContext, callerContext), MDMTraceSource.DataModel);
        }

        #endregion

        #region Promote Methods

        /// <summary>
        /// Enqueues for promote.
        /// </summary>
        /// <param name="entityFamilyQueueCollection">The entity family queue collection.</param>
        /// <param name="callerContext">The caller context.</param>
        /// <returns></returns>
        public OperationResultCollection EnqueueForPromote(EntityFamilyQueueCollection entityFamilyQueueCollection, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<PromoteBL, OperationResultCollection>("EnqueueForPromote", businessLogic => businessLogic.EnqueueForPromote(entityFamilyQueueCollection, callerContext), MDMTraceSource.Entity);
        }

        #endregion Promote Methods

        #region Entity State Validation Methods

        /// <summary>
        /// Gets the state of the entity validation for given entity id list and its family members
        /// </summary>
        /// <param name="entityIds">Indicates the entity ids</param>
        /// <param name="needGlobalFamilyErrors">Indicates whether to return states for global family members or not</param>
        /// <param name="needVariantFamilyErrors">Indicates whether to return states for variant family members or not</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Returns collection of entity state validation based on provided entity id list</returns>
        public EntityStateValidationCollection GetEntitiesStateValidation(Collection<Int64> entityIds, Boolean needGlobalFamilyErrors, Boolean needVariantFamilyErrors, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<EntityStateValidationBL, EntityStateValidationCollection>("GetEntitiesStateValidation", businessLogic => businessLogic.Get(entityIds, callerContext, needGlobalFamilyErrors, needVariantFamilyErrors), MDMTraceSource.Entity);
        }

        /// <summary>
        /// Enqueue for re-validate process for given entity family queue
        /// </summary>
        /// <param name="entityFamilyQueue">Indicates entity family queue for which re-validation process is required</param>
        /// <param name="callerContext">Indicates application and module name of caller</param>
        /// <returns>Returns operation result of current operation</returns>
        public OperationResult EnqueueForRevalidate(EntityFamilyQueue entityFamilyQueue, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<EntityStateValidationBL, OperationResult>("EnqueueForRevalidate", businessLogic => businessLogic.EnqueueForRevalidate(entityFamilyQueue, callerContext), MDMTraceSource.Entity);
        }

        /// <summary>
        /// Gets business conditions for given entity id list
        /// </summary>
        /// <param name="entityIds">Indicates the entity ids</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Returns collection of entity business conditions based on provided entity id list</returns>
        public EntityBusinessConditionCollection GetEntityBusinessConditions(Collection<Int64> entityIds, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<EntityStateValidationBL, EntityBusinessConditionCollection>("GetEntityBusinessConditions", businessLogic => businessLogic.GetEntityBusinessConditions(entityIds, callerContext), MDMTraceSource.Entity);
        }

        #endregion Entity State Validation Methods

        #region AttributeModel Methods

        /// <summary>
        /// Returns attribute models based on container identifiers list
        /// Note: Method returns base attribute model properties and will not be having property changes done at Container Mapping level
        /// </summary>
        /// <param name="containerIdList">Indicates container ids based on which needs to get attribute models</param>
        /// <param name="callerContext">Indicates context of the caller</param>
        /// <returns>Attribute models based on container ids</returns>
        public AttributeModelCollection GetMappedAttributeModelsByContainers(Collection<Int32> containerIdList, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<AttributeModelBL, AttributeModelCollection>("GetMappedAttributeModelsByContainers",
                                businessLogic => businessLogic.GetMappedAttributeModelsForContainers(containerIdList, callerContext), MDMTraceSource.AttributeModelGet);
        }

        #endregion AttributeModel Methods

        #region Activity Log Methods

        /// <summary>
        /// Get impacted entity log for given status.
        /// </summary>
        /// <param name="entityActivityList">Indicates the entity activity list type</param>
        /// <param name="processingStatus">Indicates which record is to be fetched (current, past or pending)</param>
        /// <param name="fromRecordNumber">Indicates the starting index of record to be fetched</param>
        /// <param name="toRecordNumber">Indicates the end index of record which are to be fetched</param>
        /// <param name="callerContext">Indicates the caller context having information of who called the API</param>
        /// <returns>Returns the impacted entity log collection</returns>
        public EntityActivityLogCollection GetEntityActivityLogs(EntityActivityList entityActivityList, ProcessingStatus processingStatus, Int64 fromRecordNumber, Int64 toRecordNumber, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<EntityActivityLogBL, EntityActivityLogCollection>("GetEntityActivityLogs", 
                businessLogic => businessLogic.Get(entityActivityList, processingStatus, fromRecordNumber, toRecordNumber, callerContext), MDMTraceSource.UI);
        }

        #endregion

        public EntityOperationResultCollection PerformBulkWorkflowOperation(Collection<Int64> entityIdList, String activityLongName, String workflowAction, String comments, String operationType, SecurityUser newlyAssignedUser, CallerContext callerContext)
        {
            EntityOperationResultCollection entityORC = null;
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
            {
                MDMTraceHelper.StartTraceActivity("InternalCommonService.PerformBulkWorkflowOperation", false);
            }

            try
            {
                if (isTracingEnabled)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "InternalCommonService receives 'PerformBulkWorkflowOperation' request message.");
                }

                EntityWorkflowBL entityWorkflowBL = new EntityWorkflowBL(new EntityBL());
                entityORC = entityWorkflowBL.PerformBulkWorkflowOperation(entityIdList, activityLongName, workflowAction, comments, operationType, newlyAssignedUser, callerContext);

                if (isTracingEnabled)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "InternalCommonService sends 'PerformBulkWorkflowOperation' response message.");
                }
            }
            catch (Exception ex)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, ex.Message);
                this.LogException(ex);
            }

            if (isTracingEnabled)
            {
                MDMTraceHelper.StopTraceActivity("InternalCommonService.PerformBulkWorkflowOperation");
            }

            return entityORC;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TBusinessLogic"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="methodName"></param>
        /// <param name="call"></param>
        /// <param name="traceSource"></param>
        /// <returns></returns>
        private TResult MakeBusinessLogicCall<TBusinessLogic, TResult>(String methodName, Func<TBusinessLogic, TResult> call, MDMTraceSource traceSource) where TBusinessLogic : BusinessLogicBase, new()
        {
            #region Diagnostics & Tracing

            ExecutionContext executionContext = new ExecutionContext(traceSource);
            DiagnosticActivity activity = new DiagnosticActivity(executionContext, String.Format("InternalCommonService.{0}", methodName));
            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            Boolean isTraceOn = traceSettings != null && traceSettings.IsBasicTracingEnabled ? true : false;

            //Start trace activity
            if (isTraceOn)
            {
                activity.Start();
            }

            #endregion Diagnostics & Tracing

            TResult operationResult;

            try
            {
                if (isTraceOn)
                {
                    activity.LogInformation(String.Format("InternalCommonService receives '{0}' request message.", methodName));
                }

                operationResult = call(new TBusinessLogic());

                if (isTraceOn)
                {
                    activity.LogInformation(String.Format("InternalCommonService receives'{0}' response message.", methodName));
                }
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            finally
            {
                if (isTraceOn)
                {
                    activity.Stop();
                }
            }

            return operationResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="methodName"></param>
        /// <param name="call"></param>
        /// <param name="traceSource"></param>
        /// <returns></returns>
        private TResult MakeBusinessLogicCall<TResult>(String methodName, Func<TResult> call, MDMTraceSource traceSource)
        {
            #region Diagnostics & Tracing

            ExecutionContext executionContext = new ExecutionContext(traceSource);
            DiagnosticActivity activity = new DiagnosticActivity(executionContext, String.Format("InternalCommonService.{0}", methodName));
            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            Boolean isTraceOn = traceSettings != null && traceSettings.IsBasicTracingEnabled ? true : false;

            //Start trace activity
            if (isTraceOn)
            {
                activity.Start();
            }

            #endregion Diagnostics & Tracing

            TResult operationResult;

            try
            {
                if (isTraceOn)
                {
                    activity.LogInformation(String.Format("InternalCommonService receives '{0}' request message.", methodName));
                }

                operationResult = call();

                if (isTraceOn)
                {
                    activity.LogInformation(String.Format("InternalCommonService receives'{0}' response message.", methodName));
                }
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            finally
            {
                if (isTraceOn)
                {
                    activity.Stop();
                }
            }

            return operationResult;
        }

        /// <summary>
        /// Update categories with target taxonomy and organize them by level
        /// </summary>
        /// <param name="categories">Collection of categories</param>
        /// <param name="targetHierarchy">Target Hierarchy</param>
        /// <returns>Dictionary where [Key] is Category Level and [Value] is collection of categories</returns>
        private Dictionary<Int32, CategoryCollection> GetUpdatedCategoriesForLevelMap(CategoryCollection categories, Hierarchy targetHierarchy)
        {
            Dictionary<Int32, CategoryCollection> categoriesByLevelMap = new Dictionary<Int32, CategoryCollection>();
            foreach (Category category in categories)
            {
                CategoryBaseProperties categoryBaseProperties = new CategoryBaseProperties()
                {
                    Name = category.Name,
                    LongName = category.LongName,
                    HierarchyName = targetHierarchy.Name,
                    Path = category.Path,
                    Locale = category.Locale,
                    Level = category.Level,
                    HierarchyLongName = targetHierarchy.LongName,
                    IsLeaf = category.IsLeaf,
                    // impotant overrides
                    ParentCategoryId = 0,
                    ParentCategoryName = category.ParentCategoryName,
                    Action = ObjectAction.Create,
                    HierarchyId = targetHierarchy.Id,
                    Id = 0
                };

                Category updatedCategory = new Category(categoryBaseProperties, null);
                
                updatedCategory.Action = ObjectAction.Create;

                Int32 categoryLevel = category.Level;
                if (categoriesByLevelMap.ContainsKey(categoryLevel))
                {
                    categoriesByLevelMap[categoryLevel].Add(updatedCategory);
                }
                else
                {
                    categoriesByLevelMap.Add(categoryLevel, new CategoryCollection { updatedCategory });
                }
            }

            return categoriesByLevelMap;
        }

        /// <summary>
        /// Throws argument null exception if caller context object is null.
        /// </summary>
        /// <param name="callerContext">Indicates caller context, which contains the application and module that has invoked the API</param>
        /// <param name="source">Indicates application name or the object that causes the error</param>
        /// <param name="target">Indicates the method name that throws current exception</param>
        private void ValidateCallerContext(CallerContext callerContext, String source, String target)
        {
            if (callerContext == null)
            {
                LocaleMessageBL _localeMessageBL = new LocaleMessageBL();
                DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
                
                LocaleMessage _localeMessage = _localeMessageBL.Get(locale: GlobalizationHelper.GetSystemUILocale(), messageCode: "111823", loadLastest: false, callerContext: callerContext);
                diagnosticActivity.LogMessage(messageClass: MessageClassEnum.Error, message: _localeMessage.Message);
                throw new MDMOperationException(messageCode: "111823", message: _localeMessage.Message, source: source, stackTrace: String.Empty, targetSite: target);
            }
        }

        #endregion

        #endregion
    }
}