using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MDM.Services.ServiceProxies
{
    using Core;
    using BusinessObjects;
    using DataServiceClient;

    /// <summary>
    /// Represents class for data service proxy
    /// </summary>
    internal class DataServiceProxy : DataServiceClient, WCFServiceInterfaces.IDataService
    {
        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public DataServiceProxy()
        {

        }

        /// <summary>
        /// Constructor with endpoint configuration name
        /// </summary>
        /// <param name="endpointConfigurationName">Indicates the endpoint configuration name</param>
        public DataServiceProxy(String endpointConfigurationName)
            : base(endpointConfigurationName)
        {
        }

        /// <summary>
        /// Constructor with endpoint configuration name and remote address
        /// </summary>
        /// <param name="endpointConfigurationName">Indicates the endpoint configuration name</param>
        /// <param name="remoteAddress">Indicates the remote address</param>
        public DataServiceProxy(String endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress)
            : base(endpointConfigurationName, remoteAddress)
        {
        }

        #endregion

        /* Note: Below are methods which has different name in the WCF contract so not coming up as part of Service Reference class
         * We need to explicitly divert call for all the mismatched method names.
         */

        #region IDataService Members

        /// <summary>
        /// Processes a given list of entities
        /// </summary>
        /// <param name="entities">Indicates collection of entities which needs to be processed</param>
        /// <param name="entityProcessingOptions">Indicates processing option for the entities</param>
        /// <param name="callerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Returns the collection of entity operation results after processing entities</returns>
        public EntityOperationResultCollection ProcessEntities(EntityCollection entities, EntityProcessingOptions entityProcessingOptions, CallerContext callerContext)
        {
            return ProcessEntitiesWithCallerContext(entities, entityProcessingOptions, callerContext);
        }

        /// <summary>
        /// Loads requested attributes into the entity, if not available, it loads the remaining.
        /// </summary>
        /// <param name="entity">Indicates entity for which attributes needs to be ensured</param>
        /// <param name="attributeIds">Indicates attributes that needs to be ensured</param>
        /// <param name="application">Indicates the name of the application which is performing the action</param>
        /// <param name="module">Indicates the name of the module which is performing the action</param>
        /// <returns>Returns entity for which attributes are ensured</returns>
        public Entity EnsureAttributesValues(Entity entity, IEnumerable<int> attributeIds, MDMCenterApplication application, MDMCenterModules module)
        {
            return this.EnsureAttributesValues(entity, attributeIds, application, module);
        }

        /// <summary>
        /// Loads requested attributes into the Entity, if not available, it loads the remaining.
        /// </summary>
        /// <param name="entity">Indicates entity for which attributes needs to be ensured</param>
        /// <param name="attributeIds">Indicates attributes that needs to be ensured</param>
        /// <param name="loadAttributeModels">Indicates whether attribute models also needs to be ensured</param>
        /// <param name="callerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Returns entity for which attributes are ensured</returns>
        public Entity EnsureAttributes(Entity entity, IEnumerable<int> attributeIds, bool loadAttributeModels, CallerContext callerContext)
        {
            return this.EnsureAttributes(entity, attributeIds, loadAttributeModels, callerContext);
        }

        /// <summary>
        /// Loads requested attributes into the entity, if not available, it loads the remaining.
        /// </summary>
        /// <param name="entity">Indicates entity for which attributes needs to be ensured</param>
        /// <param name="attributeUniqueIdentifier">Indicates attribute UID that needs to be ensured</param>
        /// <param name="loadAttributeModels">Indicates whether attribute models also needs to be ensured</param>
        /// <param name="callerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Returns entity for which attributes are ensured</returns>
        public Entity EnsureAttributes(Entity entity, AttributeUniqueIdentifier attributeUniqueIdentifier, bool loadAttributeModels, CallerContext callerContext)
        {
            return this.EnsureAttributesForEntityAndAttributeUniqueIdentifier(entity, attributeUniqueIdentifier, loadAttributeModels, callerContext);
        }

        /// <summary>
        /// Loads requested attributes into the entity, if not available, it loads the remaining.
        /// </summary>
        /// <param name="entityCollection">Indicates collection of entity for which attributes needs to be ensured</param>
        /// <param name="attributeIds">Indicates attributes that needs to be ensured</param>
        /// <param name="loadAttributeModels">Indicates whether attribute models also needs to be ensured</param>
        /// <param name="callerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Returns collection of entities for which attributes are ensured</returns>
        public EntityCollection EnsureAttributes(EntityCollection entityCollection, IEnumerable<int> attributeIds, bool loadAttributeModels, CallerContext callerContext)
        {
            return this.EnsureAttributesForEntityCollectionAndAttributeIds(entityCollection, new Collection<Int32>(attributeIds.ToList()), loadAttributeModels, callerContext);
        }

        /// <summary>
        /// Loads requested attributes into the entity, if not available, it loads the remaining.
        /// </summary>
        /// <param name="entityCollection">Indicates collection of entity for which attributes needs to be ensured</param>
        /// <param name="attributeUniqueIdentifiers">Indicates attribute UIDs that needs to be ensured</param>
        /// <param name="loadAttributeModels">Indicates whether attribute models also needs to be ensured</param>
        /// <param name="callerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Returns collection of entities for which attributes are ensured</returns>
        public EntityCollection EnsureAttributes(EntityCollection entityCollection, Collection<AttributeUniqueIdentifier> attributeUniqueIdentifiers, bool loadAttributeModels, CallerContext callerContext)
        {
            return this.EnsureAttributesForEntityCollectionAndAttributeUniqueIdentifiers(entityCollection, attributeUniqueIdentifiers, loadAttributeModels, callerContext);
        }

        /// <summary>
        /// Loads requested attributes into the entity, if not available, it loads the remaining.
        /// </summary>
        /// <param name="entityCollection">Indicates collection of entity for which attributes needs to be ensured</param>
        /// <param name="attributeModelContext">Indicates attribute model context</param>
        /// <param name="loadAttributeModels">Indicates whether attribute models also needs to be ensured</param>
        /// <param name="callerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Returns collection of entities for which attributes are ensured</returns>
        public EntityCollection EnsureAttributes(EntityCollection entityCollection, AttributeModelContext attributeModelContext, bool loadAttributeModels, CallerContext callerContext)
        {
            return this.EnsureAttributesForEntityCollectionAndAttributeModelContext(entityCollection, attributeModelContext, loadAttributeModels, callerContext);
        }

        /// <summary>
        /// Generates the child entities for an entity
        /// </summary>
        /// <param name="entity">Indicates the entity under which the hierarchy is created</param>
        /// <param name="entityHierarchyDefinition">Indicates Hierarchy definition for which the matrix table is provided</param>
        /// <param name="callerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Returns operation result object that indicates the status of the operation and the updated matrix table</returns>
        public OperationResult GenerateEntityHierarchy(Entity entity, EntityVariantDefinition entityHierarchyDefinition, CallerContext callerContext)
        {
            return GenerateEntityHierarchyWithDimensionValues(entity, entityHierarchyDefinition, callerContext);
        }

        /// <summary>
        /// Get entity by given external identifier
        /// </summary>
        /// <param name="externalId">Indicates external identifier for which entity needs to be populate</param>
        /// <param name="entityContext">Indicates entity context for which entity needs to be populated</param>
        /// <param name="context">Indicates application and module name by which action is being performed</param>
        /// <param name="publishEvents">Indicates whether to publish events or not. Default is set to 'true'</param>
        /// <param name="applyAVS">Indicates whether to apply AVS or not. Default is set to 'true'</param>
        /// <returns>Returns entity object based on external id</returns>
        public Entity GetEntityByExternalId(String externalId, EntityContext entityContext, CallerContext context, Boolean publishEvents, Boolean applyAVS)
        {
            return base.GetEntityByExtenalIdWithPublishEventAndApplyAVSOption(externalId, entityContext, context, publishEvents, applyAVS);
        }

        /// <summary>
        /// Gets where used relationships
        /// </summary>
        /// <param name="entityId">Indicates collection of entity identifiers</param>
        /// <param name="relationshipTypeId">Indicates the relationship type identifier to filter usage for a given relationship type. 
        /// Provide '0' if you need to load every usage</param>
        /// <param name="attributeIds">Indicates identifier of attributes which needs to be loaded with related entity</param>
        /// <param name="dataLocale">Indicates data locale</param>
        /// <param name="context">Indicates application and module name by which action is being performed</param>
        /// <returns>Returns the relationship collection having relationships in the UP direction</returns>
        public RelationshipCollection GetWhereUsedRelationships(Collection<Int64> entityId, Int32 relationshipTypeId, Collection<Int32> attributeIds, LocaleEnum dataLocale, CallerContext context)
        {
            return base.GetWhereUsedRelationshipsForEntities(entityId, relationshipTypeId, attributeIds, dataLocale, context);
        }

        /// <summary>
        /// Gets where used relationships
        /// </summary>
        /// <param name="entityId">Indicates the entity identifier</param>
        /// <param name="relationshipTypeId">Indicates the relationship type identifier to filter usage for a given relationship type.
        /// Provide '0' if you need to load every usage</param>
        /// <param name="attributeIds">Indicates identifier of attributes which needs to be loaded with related entity</param>
        /// <param name="dataLocale">Indicates data locale</param>
        /// <param name="context">Indicates application and module name by which action is being performed</param>
        /// <returns>Returns the relationship collection having relationships in the UP direction</returns>
        public RelationshipCollection GetWhereUsedRelationships(Int64 entityId, Collection<Int32> relationshipTypeId, Collection<Int32> attributeIds, LocaleEnum dataLocale, CallerContext context)
        {
            return base.GetWhereUsedRelationshipsByRelationshipTypeIds(entityId, relationshipTypeId, attributeIds, dataLocale, context);
        }

        /// <summary>
        /// Gets where used relationships
        /// </summary>
        /// <param name="entityId">Indicates the entity identifier</param>
        /// <param name="relationshipTypeId">Indicates the relationship type identifier to filter usage for a given relationship type.
        /// Provide '0' if you need to load every usage</param>
        /// <param name="attributeIds">Indicates identifier of attributes which needs to be loaded with related entity</param>
        /// <param name="dataLocale">Indicates data locale</param>
        /// <param name="context">Indicates application and module name by which action is being performed</param>
        /// <returns>Returns the relationship collection having relationships in the UP direction</returns>
        public RelationshipCollection GetWhereUsedRelationships(Collection<Int64> entityId, Collection<Int32> relationshipTypeId, Collection<Int32> attributeIds, LocaleEnum dataLocale, CallerContext context)
        {
            return base.GetWhereUsedRelationshipsForEntitiesByRelationshipTypeIds(entityId, relationshipTypeId, attributeIds, dataLocale, context);
        }

        /// <summary>
        /// Obtain an Entity from the MDM system
        /// </summary>
        /// <param name="entityId">The entity identifier</param>
        /// <param name="entityContext">The data context for which entity needs to be fetched</param>
        /// <param name="entityGetOptions">Indicates the options available while retrieving entity data</param>
        /// <param name="callerContext">Specifies caller context</param>
        /// <returns>Entity having given EntityId and attributes, relationship, hierarchy relationships baesd on EntityContext</returns>
        public Entity GetEntity(Int64 entityId, EntityContext entityContext, EntityGetOptions entityGetOptions, CallerContext callerContext)
        {
            //callerContext.AdditionalProperties = null;
            return base.GetEntityWithGetOptions(entityId, entityContext, entityGetOptions, callerContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityIdList"></param>
        /// <param name="entityContext"></param>
        /// <param name="entityGetOptions"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public EntityCollection GetEntities(Collection<long> entityIdList, EntityContext entityContext, EntityGetOptions entityGetOptions, CallerContext callerContext)
        {
            return GetEntitiesWithGetOptions(entityIdList, entityContext, entityGetOptions, callerContext);
        }

        #endregion
    }
}
