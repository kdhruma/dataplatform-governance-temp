using System;
using System.Collections.ObjectModel;
using System.Transactions;

namespace MDM.EntityManager.Business
{
    using Core;
    using BusinessObjects;
    using Data;
    using Utility;
    using BufferManager;

    /// <summary>
    /// Specifies business operations for entity hierarchy relationships
    /// </summary>
    public class HierarchyRelationshipBL : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// Specifies security principal for user.
        /// </summary>
        private SecurityPrincipal _securityPrincipal = null;

        #endregion

        #region Properties

        #endregion

        #region Constructors

        /// <summary>
        /// Default Constructor that loads the security principal from Cache if present
        /// </summary>
        public HierarchyRelationshipBL()
        {
            GetSecurityPrincipal();
        }

        /// <summary>
        /// Constructor which takes the security principal if its already present in the system 
        /// </summary>
        /// <param name="securityPrincipalInstance">The user specific security principal of this instance</param>
        public HierarchyRelationshipBL(SecurityPrincipal securityPrincipalInstance)
        {
            _securityPrincipal = securityPrincipalInstance;
            SecurityPrincipalHelper.ValidateSecurityPrincipal(_securityPrincipal);
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gets hierarchy relationships for the requested entity id
        /// </summary>
        /// <param name="entityId">Id of the entity for which hierarchies are required</param>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="includeCategories">Boolean flag saying whether to include category relationships or not</param>
        /// <param name="loadLatest"></param>
        /// <param name="entityCacheStatus"></param>
        /// <param name="updateCache"></param>
        /// <param name="updateCacheStatusInDB"></param>
        /// <returns>The collection of hierarchy relationships</returns>
        /// <exception cref="ArgumentException">Thrown when entity id is not available</exception>        
        public HierarchyRelationshipCollection GetHierarchyRelationships(Int64 entityId, Int32 containerId, Int32 entityTypeId, Boolean includeCategories, Boolean loadLatest, EntityCacheStatus entityCacheStatus, Boolean updateCache = false, Boolean updateCacheStatusInDB = true)
        {
            #region Step: Parameters Validation

            if (entityId < 1)
                throw new ArgumentException("Entity Id is not available. Please provide the entity id for which hierarchies are required.");

            #endregion

            #region Step : Initial Setup

            HierarchyRelationshipCollection hierarchyRelationshipCollection = null;
            HierarchyRelationshipDA entityHierarchyRelationshipDA = new HierarchyRelationshipDA();
            EntityQueueBL entityQueueBL = new EntityQueueBL();
            Boolean isEHRCacheDirty = false;

            var entityBufferManager = new EntityBufferManager();

            #endregion

            #region Step : Check if requested entity is available in Entity Hierarchy Queue, if not then only try to find it in cache

            if (!loadLatest)
            {
                #region Logical Flow

                /*
                 *  a.
                        i. Check the requested entity is available in Entity Hierarchy queue.
                        ii.	If it returns true, then do the following,
                            1.	Clear the Hierarchy Relationships from the cache.
                            3.	Continue with the current logic.
                    b.	If false,
                        i.	Continue with the current logic
                */

                #endregion

                isEHRCacheDirty = (entityCacheStatus != null) && entityCacheStatus.IsHierarchyCacheDirty;

                if (!isEHRCacheDirty)
                {
                    hierarchyRelationshipCollection = entityBufferManager.FindHierarchyRelationships(entityId, containerId, entityTypeId);

                    // If Cache status is not dirty and if cache is unavailable, then set for reload.
                    if (hierarchyRelationshipCollection == null && entityCacheStatus != null)
                    {
                        entityCacheStatus.IsHierarchyCacheDirty = true;

                        // If the call comes from EntityBL, the save should not be called.
                        if (updateCacheStatusInDB && entityCacheStatus.IsCacheStatusUpdated)
                        {
                            EntityCacheStatusCollection entityCacheStatusCollection = new EntityCacheStatusCollection();
                            entityCacheStatusCollection.Add(entityCacheStatus);

                            CallerContext callerContext = new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.Entity);
                            EntityCacheStatusBL entityCacheStatusBL = new EntityCacheStatusBL();
                            entityCacheStatusBL.Process(entityCacheStatusCollection, callerContext);
                        }
                    }
                }
            }

            #endregion

            #region Step : if EHR object not found in cache, try to load it from database..

            if (hierarchyRelationshipCollection == null)
            {
                hierarchyRelationshipCollection = entityHierarchyRelationshipDA.GetHierarchyRelationships(entityId, true); //Get CategoryHierarchyRelationships always

                #region Step : Check if EHR found in db and place it into entity buffer cache

                if (updateCache && hierarchyRelationshipCollection != null)
                {
                    entityBufferManager.UpdateHierarchyRelationships(entityId, containerId, entityTypeId, hierarchyRelationshipCollection);
                }

                #endregion
            }

            #endregion

            #region Step : Filter Category Hierarchy Relationships if asked

            if (!includeCategories)
            {
                hierarchyRelationshipCollection = (HierarchyRelationshipCollection)hierarchyRelationshipCollection.GetEntityHierarchyRelationships();
            }

            #endregion

            return hierarchyRelationshipCollection;
        }

        /// <summary>
        /// Loads hierarchy relationships for the requested entity
        /// </summary>
        /// <param name="entity">Entity for which hierarchies needs to be loaded</param>
        /// <param name="includeCategories">Boolean flag saying whether to include category relationships or not</param>
        /// <returns>The Boolean flag which tells whether the load is successful or not</returns>
        /// <exception cref="ArgumentNullException">Thrown when passed entity object is null</exception>
        public Boolean LoadHierarchyRelationships(Entity entity, Boolean includeCategories)
        {
            EntityCacheStatus entityCacheStatus = GetEntityCacheStatus(entity.Id, entity.ContainerId);
            return LoadHierarchyRelationships(entity, includeCategories, false, entityCacheStatus);
        }

        /// <summary>
        /// Loads hierarchy relationships for the requested entity
        /// </summary>
        /// <param name="entity">Entity for which hierarchies needs to be loaded</param>
        /// <param name="includeCategories">Boolean flag saying whether to include category relationships or not</param>
        /// <param name="loadLatest">Get fresh copy of data</param>
        /// <param name="entityCacheStatus">Holds the entity cache status</param>
        /// <param name="updateCache">Specifies whether to update cache</param>
        /// <param name="updateCacheStatusInDB">Specifies whether to update cache</param>
        /// <returns>The Boolean flag which tells whether the load is successful or not</returns>
        /// <exception cref="ArgumentNullException">Thrown when passed entity object is null</exception>
        public Boolean LoadHierarchyRelationships(Entity entity, Boolean includeCategories, Boolean loadLatest, EntityCacheStatus entityCacheStatus, Boolean updateCache = false, Boolean updateCacheStatusInDB = true)
        {
            Boolean result = false;

            if (entity == null)
                throw new ArgumentNullException("entity");

            HierarchyRelationshipCollection hierarchyRelationshipCollection = GetHierarchyRelationships(entity.Id, entity.ContainerId, entity.EntityTypeId, includeCategories, loadLatest, entityCacheStatus, updateCache, updateCacheStatusInDB);

            if (hierarchyRelationshipCollection != null)
            {
                result = true;
                entity.HierarchyRelationships = hierarchyRelationshipCollection;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hierarchyRelationships"></param>
        /// <param name="entityContext"></param>
        /// <param name="loadRecursive"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public OperationResult LoadRelatedEntities(HierarchyRelationshipCollection hierarchyRelationships, EntityContext entityContext, Boolean loadRecursive, CallerContext callerContext)
        {
            return LoadRelatedEntities(hierarchyRelationships, entityContext, loadRecursive, callerContext, false, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hierarchyRelationships"></param>
        /// <param name="entityContext"></param>
        /// <param name="loadRecursive"></param>
        /// <param name="callerContext"></param>
        /// <param name="publishEvents"></param>
        /// <param name="applyAVS"></param>
        /// <returns></returns>
        public OperationResult LoadRelatedEntities(HierarchyRelationshipCollection hierarchyRelationships, EntityContext entityContext, Boolean loadRecursive, CallerContext callerContext, Boolean publishEvents, Boolean applyAVS)
        {
            OperationResult operationResult = new OperationResult();

            Collection<Int64> entityIdList = new Collection<Int64>();

            if (hierarchyRelationships != null && hierarchyRelationships.Count > 0)
            {
                foreach (HierarchyRelationship hierarchyRelationship in hierarchyRelationships)
                {
                    entityIdList.Add(hierarchyRelationship.RelatedEntityId);
                }

                EntityBL entityBL = new EntityBL();
                EntityCollection entityCollection = entityBL.Get(entityIdList, entityContext, false, callerContext.Application, callerContext.Module, publishEvents, applyAVS);

                if (entityCollection != null)
                {
                    foreach (HierarchyRelationship hierarchyRelationship in hierarchyRelationships)
                    {
                        Int64 relatedEntityId = hierarchyRelationship.RelatedEntityId;

                        Entity entity = (Entity)entityCollection.GetEntity(relatedEntityId);

                        if (entity != null)
                            hierarchyRelationship.RelatedEntity = entity;

                        if (loadRecursive
                                && hierarchyRelationship.RelationshipCollection != null
                                && hierarchyRelationship.RelationshipCollection.Count > 0)
                        {
                            OperationResult internalOperationResult = LoadRelatedEntities(hierarchyRelationship.RelationshipCollection, entityContext, loadRecursive, callerContext, publishEvents, applyAVS);
                            //Copy and merge operation results..
                        }
                    }
                }
            }
            return operationResult;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        private void GetSecurityPrincipal()
        {
            if (_securityPrincipal == null)
            {
                _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="containerId"></param>
        /// <returns></returns>
        private EntityCacheStatus GetEntityCacheStatus(Int64 entityId, Int32 containerId)
        {
            EntityCacheStatusLoadRequest entityCacheStatusLoadRequest = new EntityCacheStatusLoadRequest() { EntityId = entityId, ContainerId = containerId };

            EntityCacheStatusBL entityCacheStatusBL = new EntityCacheStatusBL();
            EntityCacheStatusCollection entityCacheStatusCollection = entityCacheStatusBL.GetEntityCacheStatusCollection(new Collection<EntityCacheStatusLoadRequest>() { entityCacheStatusLoadRequest }, new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity));

            return entityCacheStatusCollection.GetEntityCacheStatus(entityId, containerId);
        }

        #endregion

        #endregion
    }
}