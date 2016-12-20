using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ServiceModel;

namespace MDM.WCFServiceInterfaces
{
    using Core;
    using BusinessObjects;
    using BusinessObjects.Imports;
    
    /// <summary>
    /// 
    /// </summary>
    public interface IMDMWCFBase
    {

    }

    /// <summary>
    /// Defines operation contracts for MDM Data operations
    /// </summary>
    [ServiceContract(Namespace = "http://wcfservices.riversand.com")]
    public interface IDataService : IMDMWCFBase
    {
        #region Entity Contracts

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Entity GetEntityModel(EntityContext entityContext, MDMCenterApplication application, MDMCenterModules module);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Entity GetEntity(Int64 entityId, EntityContext entityContext, MDMCenterApplication application, MDMCenterModules module, Boolean publishEvents, Boolean applyAVS);

        [OperationContract(Name = "GetEntityWithGetOptions")]
        [FaultContract(typeof(MDMExceptionDetails))]
        Entity GetEntity(Int64 entityId, EntityContext entityContext, EntityGetOptions entityGetOptions, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityCollection GetEntities(Collection<Int64> entityIdList, EntityContext entityContext, MDMCenterApplication application, MDMCenterModules module, Boolean publishEvents, Boolean applyAVS);

        [OperationContract(Name = "GetEntitiesWithGetOptions")]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityCollection GetEntities(Collection<Int64> entityIdList, EntityContext entityContext, EntityGetOptions entityGetOptions, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityOperationResult CreateEntity(Entity entity, EntityProcessingOptions entityProcessingOptions, String programName, MDMCenterApplication application, MDMCenterModules module);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityOperationResultCollection CreateEntities(EntityCollection entities, EntityProcessingOptions entityProcessingOptions, String programName, MDMCenterApplication application, MDMCenterModules module);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityOperationResult UpdateEntity(Entity entity, EntityProcessingOptions entityProcessingOptions, String programName, MDMCenterApplication application, MDMCenterModules module);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityOperationResult UpdateEntityByContext(Entity entity, CallerContext entityContext, EntityProcessingOptions entityProcessingOptions);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityOperationResultCollection UpdateEntities(EntityCollection entities, EntityProcessingOptions entityProcessingOptions, String programName, MDMCenterApplication application, MDMCenterModules module);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityOperationResult DeleteEntity(Entity entity, String programName, MDMCenterApplication application, MDMCenterModules module);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityOperationResultCollection DeleteEntities(EntityCollection entities, String programName, MDMCenterApplication application, MDMCenterModules module);

        [OperationContract]
        [Obsolete("This method has been obsoleted. Please use ProcessEntitiesWithCallerContext method instead of this")]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityOperationResultCollection ProcessEntities(EntityCollection entities, EntityProcessingOptions entityProcessingOptions, String programName, MDMCenterApplication application, MDMCenterModules module);

        [OperationContract(Name = "ProcessEntitiesWithCallerContext")]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityOperationResultCollection ProcessEntities(EntityCollection entities, EntityProcessingOptions entityProcessingOptions, CallerContext callerContext);
        
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Entity GetEntityByExternalId(String externalId, EntityContext entityContext, CallerContext context);

        [OperationContract(Name = "GetEntityByExtenalIdWithPublishEventAndApplyAVSOption")]
        [FaultContract(typeof(MDMExceptionDetails))]
        Entity GetEntityByExternalId(String externalId, EntityContext entityContext, CallerContext context, Boolean publishEvents, Boolean applyAVS);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityCollection GetEntitiesByExternalIds(Collection<String> externalIdList, EntityContext entityContext, CallerContext callerContext, Boolean publishEvents, Boolean applyAVS);
        
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityOperationResultCollection BulkUpdateEntityAttributes(EntityCollection templateEntities, Collection<Int64> entityIdsToProcess, String actionPerformed, CallerContext callerContext);
        
        #endregion

        #region Entity Locale Contracts

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityOperationResultCollection ProcessEntityLocale(String dataXml, String systemDataLocale, Boolean returnResult, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityCollection GetEntityLocale(Int64 entityId, Collection<Locale> datalocales, CallerContext callerContext);

        #endregion
        
        #region Entity Hierarchy Contracts

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Entity GetEntityHierarchy(EntityUniqueIdentifier entityUniqueIdentifier, EntityContext entityContext, EntityGetOptions entityGetOptions, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Table CalculateEntityHierarchyDimensions(EntityVariantLevel entityHierarchyLevel, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Boolean ProcessEntityHierarchyGenerationRules(Int64 entityId, EntityVariantDefinition entityHierarchyDefinition, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityVariantDefinition GetEntityHierarchyDimensionValues(Int64 entityId, EntityVariantDefinition entityHierarchyDefinition, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Table GetEntityHierarchyMatrix(Int64 entityId, EntityVariantDefinition entityHierarchyDefinition, OperationResult operationResult, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult ProcessEntityHierarchyMatrix(Int64 entityId, Table matrixTable, EntityVariantDefinition entityHierarchyDefinition, CallerContext callerContext);

        [OperationContract(Name = "GenerateEntityHierarchyWithDimensionValues")]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult GenerateEntityHierarchy(Entity entity, EntityVariantDefinition entityHierarchyDefinition, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult GenerateEntityHierarchy(Entity entity, Table matrixTable, EntityVariantDefinition entityHierarchyDefinition, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityCollection GetChildEntitiesByEntityType(Int64 entityId, Int32 entityTypeId, Collection<KeyValuePair<Int32, LocaleEnum>> attributeInfoList, MDMCenterApplication application, MDMCenterModules module);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Collection<Int64> GetChildEntitiesIdsByEntityType(Collection<Int64> entityIds, Collection<Int32> childEntityTypeIds);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityCollection GetChildEntities(Int64 parentEntityId, Int32 childEntityTypeId, LocaleEnum locale, Collection<Int32> returnAttributeIds, Boolean getCompleteDetailsOfEntity, Int32 maxRecordsToReturn, CallerContext callerContext, Boolean getRecursiveChildren = false);

        #endregion Entity Hierarchy Contracts

        #region File Contracts

        /// <summary>
        /// Gets a file object based on given file id
        /// </summary>
        /// <param name="fileId">file id</param>
        /// <param name="getOnlyFileDetails">a flag to indicate if only file details are required(true) or file data is also required along with details(false)</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        File GetFile(Int32 fileId, Boolean getOnlyFileDetails);

        /// <summary>
        /// Returns files using specified filter <paramref name="fileIdsFilter"/>.
        /// </summary>
        /// <param name="fileIdsFilter">Indicates ids of files. All files will be returned if filter is empty.</param>
        /// <param name="getOnlyFileDetails">Indicates file content requesting status. Please set to True if you want only files metadata information except file content.</param>
        /// <returns>Returns collection of files</returns>
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        FileCollection GetFiles(Collection<Int32> fileIdsFilter, Boolean getOnlyFileDetails);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Int32 ProcessFile(File file);
        
        #endregion File Contracts

        #region EntityMap Contracts

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityMap GetEntityMap(String systemId, String externalId, Int32 containerId, Int32 entityTypeId, Int64 categoryId, EntityIdentificationMap entityIdentificationMap, MDMCenterApplication application, MDMCenterModules module);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityMapCollection LoadInternalDetails(EntityMapCollection entityMapCollection, EntityIdentificationMap entityIdentificationMap, MDMCenterApplication application, MDMCenterModules module);

        #endregion

        #region Cache Management Methods

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Boolean RemoveEntityCache(EntityCacheInvalidateContextCollection entityCacheInvalidateContexts, CallerContext callerContext);

        #endregion
        
    }
}