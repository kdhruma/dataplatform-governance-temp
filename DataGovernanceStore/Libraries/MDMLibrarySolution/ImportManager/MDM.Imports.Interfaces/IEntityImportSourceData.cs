using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.ObjectModel;

namespace MDM.Imports.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.BusinessObjects.Jobs;
    using MDM.BusinessObjects.Imports;
    using MDM.Interfaces;
    
    /// <summary>
    /// This interface will be used by the import process to get data from a source.
    /// </summary>
    public interface IEntityImportSourceData : IImportSourceData
    {
        #region Properties
        IJobResultHandler JobResultHandler { get; set; }
        #endregion

        #region Entities

        /// <summary>
        /// Total number of entities available for processing.
        /// </summary>
        /// <returns></returns>
        Int64 GetEntityDataCount();

        /// <summary>
        /// Provides a seed for the caller to start from.
        /// </summary>
        /// <returns></returns>
        Int64 GetEntityDataSeed();

        /// <summary>
        /// If seed is provided, this indicates the endpoint for the caller to stop processing.
        /// </summary>
        /// <returns></returns>
        Int64 GetEntityEndPoint();

        /// <summary>
        /// Indicates the batching mode the proiver supports.
        /// </summary>
        /// <returns></returns>
        ImportProviderBatchingType GetBatchingType();

        /// <summary>
        /// Indicates the batch size for the worker thread.
        /// </summary>
        /// <returns></returns>
        Int32 GetEntityDataBatchSize();

        /// <summary>
        /// Gets all the entity data available to process.
        /// </summary>
        /// <returns></returns>
        EntityCollection GetAllEntityData(MDMCenterApplication application, MDMCenterModules module, IEntityProviderContext entityProviderContext);

        /// <summary>
        /// Gets the entity data for a given batch.
        /// </summary>
        /// <param name="startPK"></param>
        /// <param name="endPK"></param>
        /// <returns></returns>
        EntityCollection GetEntityDataBatch(Int64 startPK, Int64 endPK, MDMCenterApplication application, MDMCenterModules module, IEntityProviderContext entityProviderContext);

        /// <summary>
        /// Gets the next available batch of entity data. This method is used when the source is a forward only reader.
        /// </summary>
        /// <param name="startPK"></param>
        /// <param name="endPK"></param>
        /// <returns></returns>
        EntityCollection GetEntityDataNextBatch(Int32 batchSize, MDMCenterApplication application, MDMCenterModules module, IEntityProviderContext entityProviderContext);

        /// <summary>
        /// Gets the attribute data for a given set of entities and attribute type.
        /// </summary>
        /// <param name="attributeType"></param>
        /// <param name="entityCollection"></param>
        /// <returns></returns>
        AttributeCollection GetAttributeDataforEntities(AttributeModelType attributeType, EntityCollection entityCollection, MDMCenterApplication application, MDMCenterModules module);

        /// <summary>
        /// Gets the attribute data for a given set of entities and attribute type.
        /// </summary>
        /// <param name="attributeType"></param>
        /// <param name="entityList"></param>
        /// <param name="entityCollection"></param>
        /// <returns></returns>
        AttributeCollection GetAttributeDataforEntityList(AttributeModelType attributeType, string entityList, EntityCollection entityCollection, MDMCenterApplication application, MDMCenterModules module);

        /// <summary>
        /// Updates the source with the status as error with the information passed on from the processing method.
        /// </summary>
        /// <param name="errorEntities"></param>
        /// <returns></returns>
        bool UpdateErrorEntities(EntityOperationResultCollection errorEntities, MDMCenterApplication application, MDMCenterModules module);

        /// <summary>
        /// Update the source with the status as error with the informatin passed on from the processing method
        /// </summary>
        /// <param name="errorAttributes"></param>
        /// <returns></returns>
        bool UpdateErrorRelationships(RelationshipOperationResultCollection errorRelationships, MDMCenterApplication application, MDMCenterModules module);

        /// <summary>
        /// Update the source with the status as error with the informatin passed on from the processing method
        /// </summary>
        /// <param name="errorAttributes"></param>
        /// <returns></returns>
        bool UpdateErrorAttributes(AttributeOperationResultCollection errorAttributes, MDMCenterApplication application, MDMCenterModules module);

        /// <summary>
        /// Update the source with the status as error for this attribute type and for all the entities in this batch
        /// </summary>
        /// <param name="attributeType"></param>
        /// <param name="entityList"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        bool UpdateErrorAttributes(AttributeModelType attributeType, string entityList, string errorMessage, MDMCenterApplication application, MDMCenterModules module);

        /// <summary>
        /// Update the source with the status as success for all the processed entities.
        /// </summary>
        /// <param name="errorAttributes"></param>
        /// <returns></returns>
        bool UpdateSuccessEntities(EntityCollection entities, MDMCenterApplication application, MDMCenterModules module);

        /// <summary>
        /// Update the source with the status as success for all the processed entities.
        /// </summary>
        /// <param name="errorAttributes"></param>
        /// <returns></returns>
        bool UpdateSuccessAttributes(AttributeModelType attributeType, EntityCollection entities, MDMCenterApplication application, MDMCenterModules module);

        /// <summary>
        /// Get input specification.
        /// This is mostly used on UI for profile management. It will get list of entity metadata and attributes, which can be used later for changing attribute mapping.
        /// </summary>
        /// <param name="entityCountToRead">No. of entities to read from sample.</param>
        /// <param name="callerContext">Indicates who called this method</param>
        /// <returns>DataSet representing schema available in excel.</returns>
        DataSet GetInputSpecification(Int32 entityCountToRead, CallerContext callerContext);

        #endregion

        #region Relationships
        /// <summary>
        /// Total number of relationships available for processing.
        /// </summary>
        /// <returns></returns>
        Int64 GetRelationshipDataCount();

        /// <summary>
        /// Provides a seed for the caller to start from.
        /// </summary>
        /// <returns></returns>
        Int64 GetRelationshipDataSeed();

        /// <summary>
        /// If seed is provided, this indicates the endpoint for the caller to stop processing.
        /// </summary>
        /// <returns></returns>
        Int64 GetRelationshipEndPoint();

        /// <summary>
        /// Indicates the batch size for the worker thread.
        /// </summary>
        /// <returns></returns>
        Int32 GetRelationshipDataBatchSize();

        /// <summary>
        /// Gets all the relationship data available to process.
        /// </summary>
        /// <returns></returns>
        EntityCollection GetAllRelationshipData(MDMCenterApplication application, MDMCenterModules module);

        /// <summary>
        /// Gets the relationship data for a given batch.
        /// </summary>
        /// <param name="startPK"></param>
        /// <param name="endPK"></param>
        /// <returns></returns>
        EntityCollection GetRelationshipDataBatch(Int64 startPK, Int64 endPK, MDMCenterApplication application, MDMCenterModules module);

        /// <summary>
        /// Gets the next available batch of relationship data. This method is used when the source is a forward only reader.
        /// </summary>
        /// <param name="startPK"></param>
        /// <param name="endPK"></param>
        /// <returns></returns>
        EntityCollection GetRelationshipDataNextBatch(Int32 batchSize, MDMCenterApplication application, MDMCenterModules module);
        #endregion

        #region Extension Relationship

        /// <summary>
        /// Gets the parent extension relationship for a given entity
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        ExtensionRelationship GetParentExtensionRelationShip(Entity entity, MDMCenterApplication application, MDMCenterModules module);

        #endregion

        #region Mapping Related Interface Methods

        Attribute GetAttributeInfoFromInputFieldName(String inputFieldName);
        
        #endregion
    }
}
