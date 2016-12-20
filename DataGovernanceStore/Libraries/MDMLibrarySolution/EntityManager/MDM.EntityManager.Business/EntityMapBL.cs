using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace MDM.EntityManager.Business
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Imports;
    using MDM.EntityManager.Data;
    using MDM.ConfigurationManager.Business;
    using MDM.Core.Exceptions;

    /// <summary>
    /// Specifies business operations for Entity Maps
    /// </summary>
    public class EntityMapBL : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// Separator for the grouped cache key.
        /// </summary>
        private String _cacheSeparator = "#@#";

        /// <summary>
        /// Specifies security principal for user.
        /// </summary>
        private SecurityPrincipal _securityPrincipal = null;

        /// <summary>
        /// 
        /// </summary>
        private EntityMapBufferManager _entityMapBufferManager = new EntityMapBufferManager();

        #endregion

        #region Properties

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiate Entity Map BL
        /// </summary>
        public EntityMapBL()
        {
            GetSecurityPrincipal();
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gets entity map for requested external Id or custom data
        /// </summary>
        /// <param name="systemId">External system Id</param>
        /// <param name="externalId">Entity external Id</param>
        /// <param name="containerId">Container Id of the entity</param>
        /// <param name="entityTypeId">Entity Type Id of the entity</param>
        /// <param name="categoryId">Category Id of the entity</param>
        /// <param name="entityIdentificationMap">Entity Identification Mappings</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Entity Map object</returns>
        /// <exception cref="ArgumentException">Thrown when either 'systemId' or 'externalId' or 'containerId' or 'categoryId' is not available</exception>
        public EntityMap Get(String systemId, String externalId, Int32 containerId, Int32 entityTypeId, Int64 categoryId, EntityIdentificationMap entityIdentificationMap, MDMCenterApplication application, MDMCenterModules module)
        {
            return Get(systemId, externalId, containerId, entityTypeId, categoryId, String.Empty, entityIdentificationMap, application, module);
        }

        /// <summary>
        /// Gets entity map for requested external Id
        /// </summary>
        /// <param name="systemId">External system Id</param>
        /// <param name="externalId">Entity external Id</param>
        /// <param name="containerId">Container Id of the entity</param>
        /// <param name="categoryId">Category Id of the entity</param>
        /// <param name="entityIdentificationMap">Entity Identification Mappings</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Entity Map object</returns>
        /// <exception cref="ArgumentException">Thrown when either 'systemId' or 'externalId' or 'containerId' or 'categoryId' is not available</exception>
        public EntityMap Get(String systemId, String externalId, Int32 containerId, Int32 entityTypeId, Int64 categoryId, String customData, EntityIdentificationMap entityIdentificationMap, MDMCenterApplication application, MDMCenterModules module)
        {
            if (String.IsNullOrWhiteSpace(systemId))
                throw new ArgumentException("Failed to get Entity Map. System Id is not available.");

            if (String.IsNullOrWhiteSpace(externalId) && String.IsNullOrWhiteSpace(customData))
                throw new ArgumentException("Failed to get Entity Map. External Id and Custom Data is not available.");

            EntityMap entityMap = null;

            if (entityIdentificationMap == null)
            {
                entityIdentificationMap = FillEntityIdentificationMap();
            }

            externalId = String.IsNullOrWhiteSpace(customData) ? externalId : customData;

            entityMap = _entityMapBufferManager.FindEntityMap(systemId, externalId, containerId, entityTypeId, categoryId);

            if (entityMap == null)
            {
                //Get command properties
                DBCommandProperties command = DBCommandHelper.Get(application, module, MDMCenterModuleAction.Read);

                //Get entity mapping from DB
                EntityMapDA entityMapDA = new EntityMapDA();
                EntityMapCollection entityMaps = entityMapDA.Get(systemId, externalId, containerId, categoryId, entityTypeId, entityIdentificationMap, command);

                if (entityMaps != null && entityMaps.Count > 0)
                {
                    //if NodeType is 6 (Category), filter maps with container and parent id. 
                    //To filter and minimize maps to 1 matching entity map
                    
                    if (entityTypeId == 6)
                    {
                        entityMaps = new EntityMapCollection(entityMaps.Where(em => em.CategoryId == categoryId).ToList());
                    }

                    if (entityMaps.Count > 1)
                    {
                        String message = String.Format("Multiple match found for the requested External Id: {0}. Please specify Container, Category, Entity Type and Parent Entity information for the exact match.", externalId);
                        throw new MDMOperationException("113896", message, "EntityMapBL", String.Empty, "Get", externalId);
                    }
                    else
                    {
                        if (entityMaps != null && entityMaps.Count > 0)
                        {
                            entityMap = entityMaps.First();

                            try
                            {
                            //Cache object only if map exist.. i.e. there is an entity exist in MDM for the requested external Id
                            if (entityMap.InternalId > 0)
                                _entityMapBufferManager.UpdateEntityMap(entityMap);
                            else
                                entityMap = null; //There is no mapping exists.. return as null
                        }
                            catch
                            {
                                throw;
                            }
                    }
                }
            }
            }
     
            return entityMap;
        }

        /// <summary>
        /// Loads entity maps with the entity internals
        /// </summary>
        /// <param name="entityMapCollection">Collection of entity maps which needs to be loaded</param>
        /// <param name="entityIdentificationMap">Entity Identification Mappings</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Result of the operation saying whether it is successful or not</returns>
        /// <exception cref="ArgumentException">Thrown when Entity Maps which needs to be loaded are not available</exception>
        public Boolean LoadInternalDetails(EntityMapCollection entityMapCollection, EntityIdentificationMap entityIdentificationMap, MDMCenterApplication application, MDMCenterModules module)
        {
            if (entityMapCollection == null || entityMapCollection.Count < 1)
                throw new ArgumentException("Entity Maps are not available");

            if (entityIdentificationMap == null)
            {
                entityIdentificationMap = FillEntityIdentificationMap();   
            }

            Boolean loadSuccessful = LoadInternalDetail(entityMapCollection, entityIdentificationMap, application, module);

            return loadSuccessful;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public Boolean UpdateEntityMaps(EntityCollection entities)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
            {
                MDMTraceHelper.StartTraceActivity(MDMTraceSource.EntityProcess);
            }

            try
            {
                Boolean result = true;

                if (entities != null && entities.Count > 0)
                {
                    foreach (Entity entity in entities)
                    {
                        String systemId = "Internal";
                        String externalId = String.IsNullOrWhiteSpace(entity.ExternalId) ? entity.Name : entity.ExternalId;

                        if (entity.Id > 0 && entity.Action == ObjectAction.Create)
                        {
                            //TODO:: Come up with system id. Currently it is hardcoded to "Internal"..
                            EntityMap entityMap = new EntityMap(entity.Id, systemId, entity.ObjectTypeId, entity.ObjectType, externalId, entity.Id, entity.ContainerId, entity.CategoryId, entity.EntityTypeId, entity.ParentExtensionEntityId);
                        
                            _entityMapBufferManager.UpdateEntityMap(entityMap);
                        }
                        else if (entity.Id > 0 && entity.Action == ObjectAction.Reclassify && entity.EntityMoveContext != null)
                        {
                            EntityMap entityMap = _entityMapBufferManager.FindEntityMap(systemId, externalId, entity.ContainerId, entity.EntityTypeId, entity.EntityMoveContext.FromCategoryId);

                            if (entityMap != null)
                            {
                                _entityMapBufferManager.RemoveEntityMap(entityMap);
                                entityMap.CategoryId = entity.EntityMoveContext.TargetCategoryId;
                            }
                            else
                            {
                                entityMap = new EntityMap(entity.Id, systemId, entity.ObjectTypeId, entity.ObjectType, externalId, entity.Id, entity.ContainerId, entity.CategoryId, entity.EntityTypeId, entity.ParentExtensionEntityId);
                            }

                            _entityMapBufferManager.UpdateEntityMap(entityMap);
                        }
                        else if (entity.Action == ObjectAction.Delete)
                        {
                            EntityMap entityMap = new EntityMap(entity.Id, systemId, entity.ObjectTypeId, entity.ObjectType, externalId, entity.Id, entity.ContainerId, entity.CategoryId, entity.EntityTypeId);
                            _entityMapBufferManager.RemoveEntityMap(entityMap);
                        }
                    }
                }

                return result;
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
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public Boolean RemoveEntityMapCache(EntityCollection entities)
        {
            Boolean returnFlag = true;

            String systemId = "Internal";

            if (entities != null && entities.Count > 0)
            {
                foreach (Entity entity in entities)
                {
                    String externalId = String.IsNullOrWhiteSpace(entity.ExternalId) ? entity.Name : entity.ExternalId;

                    EntityMap entityMap = new EntityMap(entity.Id, systemId, entity.ObjectTypeId, entity.ObjectType, externalId, entity.Id, entity.ContainerId, entity.CategoryId, entity.EntityTypeId);

                    _entityMapBufferManager.RemoveEntityMap(entityMap);
                }
            }

            return returnFlag;
        }

        #endregion

        #region Private Methods

        private void GetSecurityPrincipal()
        {
            if (_securityPrincipal == null)
            {
                _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            }
        }

        private Boolean LoadInternalDetail(EntityMapCollection entityMapsToLoad, EntityIdentificationMap entityIdentificationMap, MDMCenterApplication application, MDMCenterModules module)
        {
            Boolean loadSuccessful = false;

            // Group the Entity Maps across ref ids
            EntityMapCollection groupedEntityMaps = GroupEntityMapCollectionByReferenceId(entityMapsToLoad);

            EntityMapCollection entityMapsToFetch = PrepareEntityMapCollectionToFetch(entityMapsToLoad, groupedEntityMaps);

            if (entityMapsToFetch != null && entityMapsToFetch.Count > 0)
            {
                loadSuccessful = LoadEntitiesFromDBAndPutInEntityMap(entityMapsToFetch, groupedEntityMaps, entityIdentificationMap, application, module);
            }

            return loadSuccessful;
        }

        /// <summary>
        /// New method used currently by MergeCopy
        /// Returns true if an entity exists in entity map( whether in Cache or in DB )
        /// If loaded from db the entities are put into entity map
        /// </summary>
        /// <param name="entityMapsToLoad"></param>
        /// <param name="entityIdentificationMap"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public Boolean DoEntitiesExistInCacheOrDB(EntityMapCollection entityMapsToLoad, EntityIdentificationMap entityIdentificationMap, MDMCenterApplication application, MDMCenterModules module)
        {
            // Group the Entity Maps across ref ids
            EntityMapCollection groupedEntityMaps = GroupEntityMapCollectionByReferenceId(entityMapsToLoad);

            EntityMapCollection entityMapsToFetch = PrepareEntityMapCollectionToFetch(entityMapsToLoad, groupedEntityMaps);

            if (entityMapsToFetch == null)
            {
                return false;
            }
            else if (entityMapsToFetch.Count == 0)
            {
                // the entity was found in map  
                return true;
            }
            else
            {
                // try to load from DB : will return false if not found in db
                return LoadEntitiesFromDBAndPutInEntityMap(entityMapsToFetch, groupedEntityMaps, entityIdentificationMap, application, module);
            }
        }

        /// <summary>
        /// Loads entities from db and puts into entity map.
        /// </summary>
        /// <param name="entityMapsToFetch"></param>
        /// <param name="groupedEntityMaps"></param>
        /// <param name="entityIdentificationMap"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns>false if any entity is not found in db</returns>

        public bool LoadEntitiesFromDBAndPutInEntityMap(EntityMapCollection entityMapsToFetch, EntityMapCollection groupedEntityMaps, EntityIdentificationMap entityIdentificationMap, MDMCenterApplication application, MDMCenterModules module)
        {

            bool loadFromDBSuccessful = false;
            DBCommandProperties command = DBCommandHelper.Get(application, module, MDMCenterModuleAction.Read);

            //Get entity mapping
            EntityMapDA entityMapDA = new EntityMapDA();
            loadFromDBSuccessful = entityMapDA.LoadInternalDetails(entityMapsToFetch, entityIdentificationMap, command);

            //Cache thus loaded entity maps
            if (loadFromDBSuccessful)
            {
                // for saving it to the cache we have to save 
                foreach (EntityMap entityMap in entityMapsToFetch)
                {
                    EntityMap groupMap = (EntityMap)groupedEntityMaps.GetEntityMap(entityMap.Id);

                    if (entityMap.InternalId > 0)
                    {
                        try
                        {
                        if (groupMap != null)
                        {
                            // Update the individual map ONLY if call is for individual entity map. For grouped calls ( attribute based)
                            // update only the grouped key.
                            if ((String.IsNullOrEmpty(entityMap.ExternalId) == false) &&
                                (String.Compare(entityMap.ExternalId, groupMap.ExternalId, true) == 0))
                            {
                                _entityMapBufferManager.UpdateEntityMap(entityMap);
                            }
                            else if ((String.IsNullOrEmpty(entityMap.CustomData) == false) &&
                                (String.Compare(entityMap.CustomData, groupMap.CustomData, true) == 0))
                            {
                                _entityMapBufferManager.UpdateEntityMap(entityMap);
                            }
                            else
                            {
                                if (groupMap.InternalId <= 0)
                                {
                                    groupMap.InternalId = entityMap.InternalId;
                                    groupMap.ExternalId = entityMap.ExternalId;
                                    _entityMapBufferManager.UpdateEntityMap(groupMap);
                                }
                            }
                        }
                        else
                        {
                            throw new Exception(String.Format("Grouped record not found for External Id {0}.", entityMap.ExternalId));
                        }
                    }
                        catch
                        {
                            throw;
                        }
                    }
                }
            }

            return loadFromDBSuccessful;
        }

        /// <summary>
        /// When the entity map collection has more than one key to identify an entity ( using attributes), we need to group them based on the refernce key.
        /// </summary>
        /// <param name="entityMapsToLoad"></param>
        /// <returns></returns>
        private EntityMapCollection GroupEntityMapCollectionByReferenceId(EntityMapCollection entityMapsToLoad)
        {
            EntityMapCollection groupedEntityMaps = new EntityMapCollection();

            foreach (EntityMap entityMap in entityMapsToLoad)
            {
                // find based on the reference id..
                EntityMap groupMap = (EntityMap)groupedEntityMaps.GetEntityMap(entityMap.Id);

                // found one..
                if (groupMap != null)
                {
                    // simply concatenate the ids to for the unique key.
                    if (!String.IsNullOrEmpty(groupMap.ExternalId))
                        groupMap.ExternalId = String.Format("{0}{1}{2}", groupMap.ExternalId, _cacheSeparator, entityMap.ExternalId);

                    if (!String.IsNullOrEmpty(groupMap.CustomData))
                        groupMap.CustomData = String.Format("{0}{1}{2}", groupMap.CustomData, _cacheSeparator, entityMap.CustomData);
                }
                else
                {
                    // create a new object and add it to the collection.
                    groupMap = new EntityMap(entityMap.Id, entityMap.SystemId, entityMap.ObjectTypeId, entityMap.ObjectType, entityMap.ExternalId, entityMap.InternalId, entityMap.ContainerId, entityMap.CategoryId, entityMap.EntityTypeId);
                    groupMap.CustomData = entityMap.CustomData;
                    groupedEntityMaps.Add(groupMap);
                }
            }

            return groupedEntityMaps;
        }

        private EntityMapCollection PrepareEntityMapCollectionToFetch(EntityMapCollection entityMapsToLoad, EntityMapCollection groupedEntityMaps)
        {
            EntityMapCollection entityMapsToFetch = new EntityMapCollection();

            foreach (EntityMap entityMap in entityMapsToLoad)
            {
                // find the grouped map.
                EntityMap groupMap = (EntityMap)groupedEntityMaps.GetEntityMap(entityMap.Id);

                String entityMapKey = (String.IsNullOrEmpty(groupMap.CustomData)) ? groupMap.ExternalId : groupMap.CustomData;

                // Check in cache whether an entity map exists for this external Id
                // Check the 'grouped' cache key for getting the current values..this is true for single or grouped map
                EntityMap cachedEntityMap = _entityMapBufferManager.FindEntityMap(entityMap.SystemId, entityMapKey, entityMap.ContainerId, entityMap.EntityTypeId, entityMap.CategoryId);

                if (cachedEntityMap != null)
                {
                    entityMap.ExternalId = entityMap.ExternalId;
                    entityMap.InternalId = cachedEntityMap.InternalId;
                    entityMap.ObjectTypeId = cachedEntityMap.ObjectTypeId;
                    entityMap.ObjectType = cachedEntityMap.ObjectType;
                    entityMap.EntityTypeId = cachedEntityMap.EntityTypeId;
                    entityMap.CategoryId = cachedEntityMap.CategoryId;
                    entityMap.ContainerId = cachedEntityMap.ContainerId;
                    entityMap.EntityFamilyId = cachedEntityMap.EntityFamilyId;
                    entityMap.EntityGlobalFamilyId = cachedEntityMap.EntityGlobalFamilyId;
                }
                else
                {
                    entityMapsToFetch.Add(entityMap);
                }
            }

            return entityMapsToFetch;
        }

        private EntityIdentificationMap FillEntityIdentificationMap()
        {
            EntityIdentificationMap entityIdentificationMap = new EntityIdentificationMap();
            Mapping mapping = new Mapping();

            mapping.SourceType = MappingDataType.MDMEntityObject;
            mapping.SourceName = "ExternalId";
            mapping.TargetType = MappingDataType.EntityData;
            mapping.TargetName = "ShortName";

            entityIdentificationMap.Mappings.Add(mapping);

            return entityIdentificationMap;
        }
      
        #endregion

        #endregion
    }
}