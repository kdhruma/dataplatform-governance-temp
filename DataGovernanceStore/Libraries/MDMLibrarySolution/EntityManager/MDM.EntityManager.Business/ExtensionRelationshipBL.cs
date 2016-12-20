using System;
using System.Linq;
using System.Transactions;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Diagnostics;

namespace MDM.EntityManager.Business
{
    using MDM.CategoryManager.Business;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.Utility;
    using MDM.BusinessObjects;
    using MDM.ConfigurationManager.Business;
    using MDM.EntityManager.Data;
    using MDM.ContainerManager.Business;
    using MDM.MessageManager.Business;
    using MDM.BufferManager;
    using MDM.BusinessObjects.Diagnostics;
    using Interfaces;

    /// <summary>
    /// Specifies business operations for extension relationships
    /// </summary>
    public class ExtensionRelationshipBL : BusinessLogicBase, IExtensionRelationshipManager
    {
        #region Fields

        /// <summary>
        /// Specifies security principal for user.
        /// </summary>
        private SecurityPrincipal _securityPrincipal = null;

        /// <summary>
        /// Specifies buffer manager for entity
        /// </summary>
        private EntityBufferManager _entityBufferManager = new EntityBufferManager();

        /// <summary>
        /// Field denoting locale Message
        /// </summary>
        private LocaleMessage _localeMessage = null;

        /// <summary>
        /// Field denoting localeMessageBL
        /// </summary>
        private LocaleMessageBL _localeMessageManager = new LocaleMessageBL();

        /// <summary>
        /// Maintains categories requested for extensions
        /// </summary>
        private Dictionary<Int64, Entity> _categoryDictionary = new Dictionary<Int64, Entity>();

        /// <summary>
        /// Maintains avaliable containers which can be used while extension validation
        /// </summary>
        private ContainerCollection _containers = null;

        /// <summary>
        /// Maintains Category Business logic object which can be used while extension validation
        /// </summary>
        private CategoryBL _categoryBL = null;
        
        #endregion

        #region Properties

        #endregion

        #region Construstors

        /// <summary>
        /// Default Constructor that loads the security principal from Cache if present
        /// </summary>
        public ExtensionRelationshipBL()
        {
            GetSecurityPrincipal();
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gets extension relationships for the requested entity Id
        /// <para>
        /// This method loads the entity object. So use this method only when if entity object is not available.
        /// </para>
        /// </summary>
        /// <param name="entityId">Entity Id for which extensions are required</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Extension relationships</returns>
        /// <exception cref="ArgumentException">Thrown when the entity Id parameter is not available</exception>
        /// <exception cref="Exception">Thrown when entity object is not available for the requested entity id</exception>
        public ExtensionRelationshipCollection Get(Int64 entityId, MDMCenterApplication application, MDMCenterModules module)
        {
            return Get(entityId, false, application, module);
        }

        /// <summary>
        /// Gets extension relationships for the requested entity Id
        /// <para>
        /// This method loads the entity object. So use this method only when if entity object is not available.
        /// </para>
        /// </summary>
        /// <param name="entityId">Entity Id for which extensions are required</param>
        /// <param name="getLatest">Boolean flag which says whether to get from DB or cache. True means always get from DB</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Extension relationships</returns>
        /// <exception cref="ArgumentException">Thrown when the entity Id parameter is not available</exception>
        /// <exception cref="Exception">Thrown when entity object is not available for the requested entity id</exception>
        public ExtensionRelationshipCollection Get(Int64 entityId, Boolean getLatest, MDMCenterApplication application, MDMCenterModules module)
        {
                ExtensionRelationshipCollection extensionRelationships = null;

                #region Step 1: Parameter validation

                if (entityId < 1)
                {
                    throw new ArgumentException("Entity extensions cannot be fetched. Entity Id for which extensions are required is not available");
                }

                #endregion

                #region Step 2: Initial setup

            //Get entity (only entity properties)
            Entity entity = GetEntity(entityId, application, module);

            EntityCollection entityCollection = new EntityCollection();
            entityCollection.Add(entity);

            EntityOperationResult entityOperationResult = new EntityOperationResult(entity.Id, entity.LongName);
            EntityOperationResultCollection entityOperationResultCollection = new EntityOperationResultCollection();
            entityOperationResultCollection.Add(entityOperationResult);

            CallerContext callerContext = new CallerContext(application, module);
            EntityCacheStatusLoadRequest entityCacheStatusLoadRequest = new EntityCacheStatusLoadRequest() { EntityId = entityId };

            EntityCacheStatusBL entityCacheStatusBL = new EntityCacheStatusBL();
            var entityCacheStatusCollection = entityCacheStatusBL.GetEntityCacheStatusCollection(new Collection<EntityCacheStatusLoadRequest>() { entityCacheStatusLoadRequest }, callerContext);

            #endregion

            #region Step 3: Get Entity Extensions

            LoadEntityExtensions(entityCollection, getLatest, entityOperationResultCollection, entityCacheStatusCollection, application, module);

            if (!entityOperationResult.HasError)
            {
                extensionRelationships = entity.ExtensionRelationships;
            }
            else
            {
                throw new Exception(entityOperationResult.Errors[0].ErrorMessage);
            }

            #endregion

            return extensionRelationships;
        }

        /// <summary>
        /// Loads the entity with the extension relationships
        /// </summary>
        /// <param name="entity">Entity for which extension relationships needs to be loaded</param>
        /// <param name="loadLatest">Boolean flag which says whether to load from DB or cache. True means always load from DB</param>
        /// <param name="entityCacheStatus">Specifies the Cache status of the Entity.</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <param name="updateCache">Specifies whether to update cache.</param>
        /// <param name="updateCacheStatusInDB">Specifies whether to update cache status in database.</param>
        /// <returns>The operation result</returns>
        /// <exception cref="ArgumentNullException">Thrown when entity parameter is null</exception>
        /// <exception cref="ArgumentException">Thrown when the entity Id of the entity parameter is not available</exception>
        public EntityOperationResult Load(Entity entity, Boolean loadLatest, EntityCacheStatus entityCacheStatus, MDMCenterApplication application, MDMCenterModules module, Boolean updateCache = false, Boolean updateCacheStatusInDB = true)
        {
            #region Step 1: Parameter validation

            if (entity == null)
                throw new ArgumentNullException("entity");

            if (entity.Id < 1)
                throw new ArgumentException("Entity extensions cannot be fetched. Entity Id for which extensions are required is not available.");

            #endregion

            #region Step 2: Initial setup

            EntityCollection entityCollection = new EntityCollection();
            entityCollection.Add(entity);

            EntityCacheStatusCollection entityCacheStatusCollection = new EntityCacheStatusCollection();
            if (entityCacheStatus != null)
            {
                entityCacheStatusCollection.Add(entityCacheStatus);
            }

            EntityOperationResultCollection entityOperationResultCollection = new EntityOperationResultCollection();
            entityOperationResultCollection.Add(new EntityOperationResult(entity.Id, entity.LongName));

            #endregion

            #region Step 3: Load Entity Extensions

            LoadEntityExtensions(entityCollection, loadLatest, entityOperationResultCollection, entityCacheStatusCollection, application, module, updateCache, updateCacheStatusInDB);

            #endregion

            return entityOperationResultCollection.First();
        }

        /// <summary>
        /// Processes the extensions of the entities
        /// </summary>
        /// <param name="entityCollection">Entities for which extensions needs to be processed</param>
        /// <param name="entityCacheStatusCollection">Entities for which extensions needs to be processed</param>
        /// <param name="programName">Name of program which is performing action</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <param name="processingMode">Mode of processing (Sync or Async)</param>
        /// <param name="saveCacheStatus">Specifies whether to save the entity cache status in database.</param>
        /// <returns>Entity Operation Results having results of the operation</returns>
        /// <exception cref="ArgumentNullException">Thrown when entityCollection is null</exception>
        /// <exception cref="ArgumentException">Thrown when entityCollection is having zero entities</exception>
        public EntityOperationResultCollection Process(EntityCollection entityCollection, EntityCacheStatusCollection entityCacheStatusCollection, String programName, MDMCenterApplication application, MDMCenterModules module, ProcessingMode processingMode = ProcessingMode.Sync, Boolean saveCacheStatus = true)
        {
            EntityOperationResultCollection entityOperationResultCollection = new EntityOperationResultCollection();

            #region Step 1: Parameter Validation

            if (entityCollection == null)
                throw new ArgumentNullException("entityCollection");

            if (entityCollection.Count < 1)
                throw new ArgumentException("Extension process failed. Entities are not available");

            #endregion

            try
            {
                #region Step 2: Initial Setup

                foreach (Entity entity in entityCollection)
                {
                    if (entity.ExtensionRelationships != null && entity.ExtensionRelationships.Count > 0)
                    {
                        entityOperationResultCollection.Add(new EntityOperationResult(entity.Id, entity.LongName));
                    }
                }

                #endregion

                if (entityOperationResultCollection.Count > 0)
                {
                    CallerContext callerContext = new CallerContext(application, module);
                    ContainerBL containerManager = new ContainerBL();
                    ContainerCollection containers = containerManager.GetAll(callerContext, true);

                    if (containers == null || containers.Count < 1)
                        throw new Exception("Container hierarchy is not available");

                    foreach (Entity entity in entityCollection)
                    {
                        if (entity.ExtensionRelationships != null && entity.ExtensionRelationships.Count > 0)
                        {
                            try
                            {
                                #region Step 3: Determine Extensions Action

                                PopulateMissingExtensionsFromCache(entity, entity.ExtensionRelationships, application, module);

                                #endregion

                                #region Step 4: Validate Inheritance path

                                entity.ExtensionRelationships = ValidateExtensionsAndPrepareHierarchy(entity, entity.ExtensionRelationships, containers, "process", application, module);

                                #endregion

                                #region Step 5: Process extensions

                                Dictionary<Int64, ExtensionRelationshipCollection> extensionRelationshipsDictionary = new Dictionary<Int64, ExtensionRelationshipCollection>();
                                extensionRelationshipsDictionary.Add(entity.Id, entity.ExtensionRelationships);

                                EntityOperationResult currentEntityOperationResult = entityOperationResultCollection.FirstOrDefault(eor => eor.EntityId == entity.Id);

                                ProcessExtensions(extensionRelationshipsDictionary, entity, currentEntityOperationResult, entityCacheStatusCollection, containers,
                                    programName, new CallerContext(application, module), processingMode, saveCacheStatus);

                                if (currentEntityOperationResult != null && currentEntityOperationResult.OperationResultStatus == OperationResultStatusEnum.Failed)
                                {
                                    entityOperationResultCollection.Add(currentEntityOperationResult);
                                    entityOperationResultCollection.RefreshOperationResultStatus();
                                }

                                #endregion
                            }
                            catch (MDMOperationException ex)
                            {
                                entityOperationResultCollection.AddEntityOperationResult(entity.Id, ex.MessageCode, ex.Message, OperationResultType.Error);
                            }
                            catch (Exception ex)
                            {
                                entityOperationResultCollection.AddEntityOperationResult(entity.Id, "", ex.Message, OperationResultType.Error);
                            }
                        }
                    }
                }
            }
            catch (MDMOperationException ex)
            {
                entityOperationResultCollection.AddOperationResult(ex.MessageCode, ex.Message, OperationResultType.Error);
            }
            catch (Exception ex)
            {
                entityOperationResultCollection.AddOperationResult("", ex.Message, OperationResultType.Error);
            }

            return entityOperationResultCollection;
        }

        /// <summary>
        /// Changes parent of the extension and its child during reverse MDL 
        /// </summary>
        /// <param name="fromEntityId">Indicates parent extension Id as per INH path</param>
        /// <param name="toEntityId">Indicates child extension Id as per INH path</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <param name="processingMode">Mode of processing (Sync or Async)</param>
        /// <returns>Returns true if extension ReParent is successful, otherwise false.</returns>
        /// <exception cref="MDMOperationException">Thrown when fromEntityId or toEntityId is not available.</exception>
        public Boolean ReParent(Int64 fromEntityId, Int64 toEntityId, CallerContext callerContext, ProcessingMode processingMode = ProcessingMode.Sync)
        {
            Boolean isSuccess = true;
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("ExtensionRelationshipManager.ReParent", MDMTraceSource.ExtensionRelationship, false);

            try
            {
                //Get command
                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Update);

                #region Parameter Validation

                if (fromEntityId < 1)
                {
                    _localeMessage = _localeMessageManager.Get(GlobalizationHelper.GetSystemUILocale(), "111945", false, callerContext);
                    throw new MDMOperationException("111945", _localeMessage.Message, "ExtensionRelationshipManager", String.Empty, "ReParent"); //Extension reparenting failed. fromEntityId is not available.
                }

                if (toEntityId < 1)
                {
                    _localeMessage = _localeMessageManager.Get(GlobalizationHelper.GetSystemUILocale(), "111946", false, callerContext);
                    throw new MDMOperationException("111946", _localeMessage.Message, "ExtensionRelationshipManager", String.Empty, "ReParent"); //Extension reparenting failed. toEntityId is not available.
                }

                #endregion

                ExtensionRelationshipDA extensionRelationshipDA = new ExtensionRelationshipDA();
                isSuccess = extensionRelationshipDA.ReParent(fromEntityId, toEntityId, command);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("ExtensionRelationshipManager.ReParent", MDMTraceSource.ExtensionRelationship);
                }
            }

            return isSuccess;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="extensionRelationships"></param>
        /// <param name="entityContext"></param>
        /// <param name="loadRecursive"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public OperationResult LoadRelatedEntities(ExtensionRelationshipCollection extensionRelationships, EntityContext entityContext, Boolean loadRecursive, CallerContext callerContext)
        {
            #region Initial Setup

            OperationResult operationResult = new OperationResult();
            Collection<Int64> entityIdList = new Collection<Int64>();
            EntityCollection entityCollection = null;

            #endregion

            if (extensionRelationships != null && extensionRelationships.Count > 0)
            {
                foreach (ExtensionRelationship extensionRelationship in extensionRelationships)
                {
                    if (extensionRelationship.RelatedEntityId > 0)
                        entityIdList.Add(extensionRelationship.RelatedEntityId);
                }

                if (entityIdList != null && entityIdList.Count > 0)
                {
                    EntityBL entityBL = new EntityBL();
                    entityCollection = entityBL.Get(entityIdList, entityContext, false, callerContext.Application, callerContext.Module, false, false);
                }

                if (entityCollection != null)
                {
                    foreach (ExtensionRelationship extensionRelationship in extensionRelationships)
                    {
                        Int64 relatedEntityId = extensionRelationship.RelatedEntityId;

                        if (extensionRelationship.RelatedEntityId > 0)
                        {
                            Entity entity = (Entity)entityCollection.GetEntity(relatedEntityId);

                            if (entity != null)
                                extensionRelationship.RelatedEntity = entity;

                            if (loadRecursive
                                    && extensionRelationship.RelationshipCollection != null
                                    && extensionRelationship.RelationshipCollection.Count > 0)
                            {
                                OperationResult internalOperationResult = LoadRelatedEntities(extensionRelationship.RelationshipCollection, entityContext, loadRecursive, callerContext);
                                //Copy and merge operation results..
                            }
                        }
                    }
                }
            }

            return operationResult;
        }

        /// <summary>
        /// Get entension entities count dictionary based on parent extension entity Ids
        /// </summary>
        /// <param name="entityIds">Indicates the parent extensions entity identifiers.</param>
        /// <param name="callerContext">Indicates application and module name by which action is being performed</param>
        /// <returns>Returns entension entities count dictionary based on parent extension entity Ids</returns>
        public Dictionary<Int64, Dictionary<Int32, Int32>> GetExtensionsRelationshipsCount(Collection<Int64> entityIds, CallerContext callerContext)
        {
            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            DiagnosticActivity currentActivity = new DiagnosticActivity();

            if (traceSettings.IsBasicTracingEnabled)
            {
                currentActivity.Start();
            }

            Dictionary<Int64, Dictionary<Int32, Int32>> extensionEntitiesCount = new Dictionary<Int64, Dictionary<Int32, Int32>>();

            try
            {
                //Get command
                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);
                ExtensionRelationshipDA extensionRelationshipDA = new ExtensionRelationshipDA();

                if (traceSettings.IsBasicTracingEnabled)
                {
                    currentActivity.LogInformation("Getting the extension relationships count from database based on parent entity entension ids");
                }

                DurationHelper durHelper = new DurationHelper(DateTime.Now);

                extensionEntitiesCount = extensionRelationshipDA.GetExtensionsRelationshipsCount(entityIds, command);

                if (traceSettings.IsBasicTracingEnabled)
                {
                    currentActivity.LogMessageWithDuration(MessageClassEnum.Information, null, "Fetching extension relationships count from database based on parent entity entension ids : Finished", durHelper.GetCumulativeTimeSpanInMilliseconds());
                }
            }

            catch (Exception e)
            {
                currentActivity.LogError(e.Message);
                throw e;
            }
           
            finally
            {
                if (traceSettings.IsBasicTracingEnabled)
                {
                    currentActivity.Stop();
                }
            }

            return extensionEntitiesCount;
        }

        #endregion

        #region Private Methods

        private void LoadEntityExtensions(EntityCollection entityCollection, Boolean loadLatest, EntityOperationResultCollection entityOperationResultCollection, EntityCacheStatusCollection entityCacheStatusCollection, MDMCenterApplication application, MDMCenterModules module, Boolean updateCache = false, Boolean updateCacheStatusInDB = true)
        {
            try
            {
                #region Step 1: Initial Setup

                Dictionary<Int64, LocaleEnum> entityIdsExtensionsTobeFetched = new Dictionary<Int64, LocaleEnum>();
                EntityQueueBL entityQueueBL = new EntityQueueBL();
                Boolean isCacheStatusDirty = true;

                #endregion

                #region Step 2: Check if requested entity is available in Entity Extension Queue, in not try to find it in cache..

                if (!loadLatest)
                {
                    #region Logical Flow
                    /*
                    a.	Check the Global Cache key to see if Entity Extension Processor is in progress.
                    b.	If true,
                        i. Check the requested entity is available in Entity Extension Queue.
                        ii.	If it returns true, then do the following,
                            1.	Clear the Extension Relationships from the cache.
                            3.	Continue with the current logic.
                    c.	If false,
                        i.	Continue with the current logic
                    */
                    #endregion

                    EntityCacheStatus entityCacheStatus = null;
                    foreach (Entity entity in entityCollection)
                    {
                        entityCacheStatus = entityCacheStatusCollection.GetEntityCacheStatus(entity.Id, entity.ContainerId);
                        isCacheStatusDirty = (entityCacheStatus != null)? entityCacheStatus.IsExtensionsCacheDirty : false;
                        
                        if (!isCacheStatusDirty)
                        {
                            entity.ExtensionRelationships = _entityBufferManager.FindExtensionRelationships(entity.Id);

                            // If Cache status is not dirty and if cache is unavailable, then set for reload.
                            if (entity.ExtensionRelationships == null && entityCacheStatus != null)
                            {
                                entityCacheStatus.IsExtensionsCacheDirty = true;
                            }
                        }
                    }

                    // If the call comes from EntityBL, the save should not be called.
                    if (updateCacheStatusInDB && entityCacheStatusCollection.IsCacheStatusUpdated())
                    {
                        CallerContext callerContext = new CallerContext(application, module);
                        EntityCacheStatusBL entityCacheStatusBL = new EntityCacheStatusBL();
                        entityCacheStatusBL.Process(entityCacheStatusCollection, callerContext);
                    }
                }

                #endregion

                #region Step 3: Populate entities for which ExR has to be loaded

                foreach (Entity entity in entityCollection)
                {
                    if ((entity.ExtensionRelationships == null || entity.ExtensionRelationships.Count < 1) && !entityIdsExtensionsTobeFetched.ContainsKey(entity.Id))
                    {
                        entityIdsExtensionsTobeFetched.Add(entity.Id, entity.Locale);
                    }
                }

                #endregion

                #region Step 4: If EER object not found in cache, try to load it from database..

                if (entityIdsExtensionsTobeFetched.Count > 0)
                {
                    //Get command
                    DBCommandProperties command = DBCommandHelper.Get(application, module, MDMCenterModuleAction.Read);

                    ExtensionRelationshipDA extensionRelationshipDA = new ExtensionRelationshipDA();

                    //Get Containers
                    CallerContext callerContext = new CallerContext(application, module);
                    ContainerBL containerManager = new ContainerBL();
                    ContainerCollection containers = containerManager.GetAll(callerContext, true);

                    foreach (Int64 entityId in entityIdsExtensionsTobeFetched.Keys)
                    {
                        //Get extension relationships
                        ExtensionRelationshipCollection entityExtensionRelationships = extensionRelationshipDA.Get(entityId, entityIdsExtensionsTobeFetched[entityId], command);

                        if (entityExtensionRelationships != null)
                        {
                            if (containers == null || containers.Count < 1)
                                throw new Exception("Entity extensions loading failed. Container hierarchy is not available");

                            //Get entity
                            Entity entity = (Entity)entityCollection.GetEntity(entityId);

                            if (entity != null)
                            {
                                //Get entity operation result
                                EntityOperationResult entityOperationResult = entityOperationResultCollection[entityId];

                                try
                                {
                                    entity.ExtensionRelationships = ValidateExtensionsAndPrepareHierarchy(entity, entityExtensionRelationships, containers, "load", application, module);

                                    //Update entity buffer cache
                                    if (updateCache && entity.ExtensionRelationships != null && entity.ExtensionRelationships.Count > 0)
                                        _entityBufferManager.UpdateExtensionRelationships(entity.Id, entity.ExtensionRelationships);
                                }
                                catch (Exception ex)
                                {
                                    entityOperationResultCollection.AddEntityOperationResult(entity.Id, "", ex.Message, OperationResultType.Error);
                                }
                            }
                        }
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                entityOperationResultCollection.AddOperationResult(String.Empty, ex.Message, OperationResultType.Error);
            }
        }

        private Entity GetEntity(Int64 entityId, MDMCenterApplication application, MDMCenterModules module)
        {
            Entity entity = null;

            EntityContext entityContext = new EntityContext(0, 0, 0, GlobalizationHelper.GetSystemDataLocale(), new Collection<LocaleEnum>() { MDM.Utility.GlobalizationHelper.GetSystemDataLocale() }, true, false, false, false, null, null, false, false, null, false, false, AttributeModelType.Unknown);

            EntityBL entityManager = new EntityBL();
            entity = entityManager.Get(entityId, entityContext, application, module, false, false);

            return entity;
        }

        private ExtensionRelationshipCollection ValidateExtensionsAndPrepareHierarchy(Entity entity, ExtensionRelationshipCollection extensionRelationships, ContainerCollection containers, String operation, MDMCenterApplication application, MDMCenterModules module)
        {
            ExtensionRelationshipCollection extensionHierarchies = new ExtensionRelationshipCollection(extensionRelationships.ToXml());

            if (extensionRelationships != null && extensionRelationships.Count > 0)
            {
                #region Child Entity Extension Validation Intial Setup

                Boolean isChildEntityExtensionProcess = false;
                ExtensionRelationshipCollection parentEntityExtensions = null;

                //TODO::Change the identification logic to determine whether entity is of type SKU
                if (entity.CategoryId != entity.ParentEntityId)
                {
                    EntityBL entityBL = new EntityBL();
                    EntityContext entityContext = new EntityContext(0, 0, 0, entity.Locale, entity.EntityContext.DataLocales, true, false, false, true, null, null, false, false, null, false, false, AttributeModelType.Unknown);
                    Entity parentEntity = entityBL.Get(entity.ParentEntityId, entityContext, application, module, false, false);

                    if (parentEntity == null)
                    {
                        String errorMessage = String.Format("Entity extensions {0}ing failed. Unable to get the parent entity.", operation);
                        throw new Exception(errorMessage);
                    }

                    if (parentEntity.ExtensionRelationships != null && parentEntity.ExtensionRelationships.Count > 0)
                    {
                        parentEntityExtensions = (ExtensionRelationshipCollection)parentEntity.ExtensionRelationships.Denormalize();
                    }

                    isChildEntityExtensionProcess = true;
                }

                #endregion

                foreach (ExtensionRelationship extensionRelationship in extensionRelationships)
                {
                    if (isChildEntityExtensionProcess)
                    {
                        #region Child Entity Extension Validation

                        ValidateChildEntityExtensions(entity, extensionRelationship, parentEntityExtensions, operation, application, module);

                        #endregion
                    }

                    #region Category and INH path validation

                    if (extensionRelationship.Action != ObjectAction.Delete)
                    {
                        ValidateExtensionContainerAndCategory(extensionRelationship, application, module);

                        ValidateINHPath(entity, extensionRelationship, extensionHierarchies, containers, operation, application, module);
                    }

                    #endregion
                }
            }

            return extensionHierarchies;
        }

        private void ValidateChildEntityExtensions(Entity entity, ExtensionRelationship extensionRelationship, ExtensionRelationshipCollection parentEntityExtensions, String operation, MDMCenterApplication application, MDMCenterModules module)
        {
            if (entity.Id != extensionRelationship.RelatedEntityId)
            {
                Boolean isParentExtended = false;

                if (parentEntityExtensions != null && parentEntityExtensions.Count > 0)
                {
                    //Check whether parent has been extended to the SKU's target container and category
                    ExtensionRelationship parentExtension = parentEntityExtensions.FirstOrDefault(e => e.ContainerId == extensionRelationship.ContainerId && e.CategoryId == extensionRelationship.CategoryId);

                    if (parentExtension != null)
                    {
                        isParentExtended = true;
                    }
                }

                if (!isParentExtended)
                {
                    String errorMessage = String.Format("Entity extensions {0}ing failed: This entity cannot be extended to the specified container and category, as the parent for this entity does not exist in that container and category. ", operation);
                    throw new Exception(errorMessage);
                }
            }
        }

        private void ValidateExtensionContainerAndCategory(ExtensionRelationship extensionRelationship, MDMCenterApplication application, MDMCenterModules module)
        {
            CallerContext callerContext = new CallerContext(application, module);

            if (_containers == null || _containers.Count < 1)
            {
                ContainerBL containerBL = new ContainerBL();
                _containers = containerBL.GetAll(callerContext);
            }

            if (_containers == null)
            {
                const String noContainersFoundErrorCode = "110342";
                const String noContainersFoundErrorMessage = "No Containers found";
                
                _localeMessage = _localeMessageManager.Get(GlobalizationHelper.GetSystemUILocale(), noContainersFoundErrorCode, null, false, callerContext);
                String errorMessage = _localeMessage != null && !String.IsNullOrEmpty(_localeMessage.Message) ? _localeMessage.Message : noContainersFoundErrorMessage;
                
                throw new MDMOperationException(errorMessage);
            }

            Container container = null;

            if (extensionRelationship.ContainerId > 0)
            {
                container = _containers.GetContainer(extensionRelationship.ContainerId);
            }
            else if (!String.IsNullOrEmpty(extensionRelationship.ContainerName))
            {
                foreach (Container currentContainer in _containers)
                {
                    if (currentContainer.Name.Equals(extensionRelationship.ContainerName))
                    {
                        container = currentContainer;
                        break;
                    }
                }
            }
            else
            {
                const String containerDetailsAreNotSpecifiedErrorCode = "114013";
                const String containerDetailsAreNotSpecifiedErrorMessage = "Container details are not specified. Specify Container Id or Container Name for Extension";

                _localeMessage = _localeMessageManager.Get(GlobalizationHelper.GetSystemUILocale(), containerDetailsAreNotSpecifiedErrorCode, null, false, callerContext);
                String errorMessage = _localeMessage != null && !String.IsNullOrEmpty(_localeMessage.Message) ? _localeMessage.Message : containerDetailsAreNotSpecifiedErrorMessage;
                
                throw new MDMOperationException(errorMessage);                
            }

            if (container == null)
            {
                const String containerCouldNotBeFoundErrorCode = "114014";
                const String containerCouldNotBeFoundErrorMessage = "Container with Id '{0}' or Name '{1}' could not be found.";

                _localeMessage = _localeMessageManager.Get(GlobalizationHelper.GetSystemUILocale(), containerCouldNotBeFoundErrorCode, new Object[] { extensionRelationship.ContainerId, extensionRelationship.ContainerName }, false, callerContext);
                String errorMessage = _localeMessage != null && !String.IsNullOrEmpty(_localeMessage.Message) ? _localeMessage.Message : String.Format(containerCouldNotBeFoundErrorMessage, extensionRelationship.ContainerId, extensionRelationship.ContainerName);
                
                throw new MDMOperationException(errorMessage);
            }

            if (_categoryBL == null)
            {
                _categoryBL = new CategoryBL();
            }

            //TODO: Preload all categories also when API will be available
            Category categoryToExtendIn;

            if (extensionRelationship.CategoryId > 0)
            {
                categoryToExtendIn = _categoryBL.GetById(container.HierarchyId, extensionRelationship.CategoryId, callerContext);
            }
            else if (!String.IsNullOrEmpty(extensionRelationship.CategoryName))
            {
                categoryToExtendIn = _categoryBL.GetByName(container.HierarchyId, extensionRelationship.CategoryName, callerContext);
            }
            else
            {
                const String categoryDetailsAreNotSpecifiedErrorCode = "114015";
                const String categoryDetailsAreNotSpecifiedErrorMessage = "Category details are not specified. Specify Category Id or Category Name for Extension.";

                _localeMessage = _localeMessageManager.Get(GlobalizationHelper.GetSystemUILocale(), categoryDetailsAreNotSpecifiedErrorCode, null, false, callerContext);
                String errorMessage = _localeMessage != null && !String.IsNullOrEmpty(_localeMessage.Message) ? _localeMessage.Message : categoryDetailsAreNotSpecifiedErrorMessage;

                throw new MDMOperationException(errorMessage);
            }

            if (categoryToExtendIn == null)
            {
                const String couldNotExtendErrorCode = "114016";
                const String couldNotExtendErrorMessage = "Could not extend entity to Category with Id '{0}' or Name '{1}' because it does not exist in the system or it is under Hierarchy which is not mapped to selected Container '{2}'.";

                _localeMessage = _localeMessageManager.Get(GlobalizationHelper.GetSystemUILocale(), couldNotExtendErrorCode, new Object[] { extensionRelationship.CategoryId, extensionRelationship.CategoryName, container.Name }, false, callerContext);
                String errorMessage = _localeMessage != null && !String.IsNullOrEmpty(_localeMessage.Message) ? _localeMessage.Message : String.Format(couldNotExtendErrorMessage, extensionRelationship.CategoryId, extensionRelationship.CategoryName, container.Name);

                throw new MDMOperationException(errorMessage);
            }
        }

        private void ValidateINHPath(Entity entity, ExtensionRelationship extensionRelationship, ExtensionRelationshipCollection extensionHierarchies, ContainerCollection containers, String operation, MDMCenterApplication application, MDMCenterModules module)
        {

            #region INH path validation logic

            //Validate..
            //Loop through ExtensionRelationships..
            //Step:1 Get container and parent container from all containers for extension relationship container
            //Step:2 Set Direction for extensionRelationship as default down 
            //Step:3 Check whether the Extension exists for the parent
            //Step:3.1 If exists, prepare proper hierarchy and return
            //Step:3.2 If not throw exception

            #endregion

            ExtensionRelationship extensionInHierarchy = null;

            try
            {
                extensionInHierarchy = extensionHierarchies.SingleOrDefault(eInh => eInh.ContainerId == extensionRelationship.ContainerId && eInh.CategoryId == extensionRelationship.CategoryId);
            }
            catch
            {
                String errorMessage = String.Format("Entity extensions {0}ing failed for Entity: {1}. Duplicate extensions found for container and category combination", operation, entity.Name);
                throw new Exception(errorMessage);
            }

            if (extensionInHierarchy != null)
            {
                #region Step 1 : Initial get for container and parent container

                Container container = containers.GetContainer(extensionInHierarchy.ContainerId);

                if (container == null)
                {
                    String errorMessage = String.Format("Entity extensions {0}ing failed. Container is not available", operation);
                    throw new Exception(errorMessage);
                }

                Container parentContainer = containers.GetContainer(container.ParentContainerId);

                #endregion Step 1 : Initial get for container and parent container

                #region Step 2: Set Direction for extensionRelationship

                extensionInHierarchy.Direction = RelationshipDirection.Down;

                #endregion

                #region Step 3: Check whether the Extension exists for the parent

                ExtensionRelationship parentExtensionRelationship = null;

                if (parentContainer != null)
                {
                    //Get the parent extension relationship..
                    parentExtensionRelationship = (ExtensionRelationship)extensionHierarchies.FindByContainerId(parentContainer.Id);

                    //Check whether the parent is itself the entity object for which extensions are requested
                    if (parentContainer.Id == entity.ContainerId)
                    {
                        //Yes.. parent is current entity itself..
                        if (parentExtensionRelationship != null && parentExtensionRelationship.Action == ObjectAction.Delete)
                        {
                            String errorMessage = String.Format("Entity extensions {0}ing failed. Extensions cannot be deleted as the resulting Extension(s) after delete would not meet the defined container hierarchy", operation);
                            throw new Exception(errorMessage);
                        }

                        return;
                    }
                }

                if (parentExtensionRelationship == null)
                {
                    //Parent extension is not available..
                    //Check for UP(reverse) extension..

                    Boolean isParentAvailable = false;

                    if (entity.OrganizationId > 0)
                    {
                        container = containers.First(c => c.Id == extensionInHierarchy.ContainerId);
                    }

                    //At down level there is possibility of multiple childs.. So, getting all container records where parent container id is same like current container id
                    IEnumerable<Container> parentContainerCandidates = containers.Where(c => c.ParentContainerId == container.Id);

                    if (parentContainerCandidates != null && parentContainerCandidates.Count() > 0)
                    {
                        //Loop through each containers to find out actual parent container
                        foreach (Container containerCandidate in parentContainerCandidates)
                        {
                            //Get the parent extension relationship..
                            parentExtensionRelationship = (ExtensionRelationship)extensionHierarchies.FindByContainerId(containerCandidate.Id);

                            //Check whether the parent is itself the entity object for which extensions are requested
                            if (containerCandidate.Id == entity.ContainerId)
                            {
                                //Yes.. parent is current entity itself..
                                if (parentExtensionRelationship != null && parentExtensionRelationship.Action == ObjectAction.Delete)
                                {
                                    String errorMessage = String.Format("Entity extensions {0}ing failed. Extensions cannot be deleted as the resulting Extension(s) after delete would not meet the defined container hierarchy", operation);
                                    throw new Exception(errorMessage);
                                }

                                isParentAvailable = true;
                            }
                            else if (parentExtensionRelationship != null)
                            {
                                isParentAvailable = true;
                            }

                            if (isParentAvailable)
                            {
                                //Set relationship direction to Up..
                                extensionInHierarchy.Direction = RelationshipDirection.Up;
                                break;
                            }
                        }
                    }

                    if (!isParentAvailable && extensionRelationship.Action == ObjectAction.Create)
                    {
                        String errorMessage = String.Format("Entity extensions {0}ing failed for Entity: {1}. Extensions are not meeting the defined container inheritance path", operation, entity.Name);
                        throw new Exception(errorMessage);
                    }
                }

                if (parentExtensionRelationship != null)
                {
                    if (parentExtensionRelationship.Action == ObjectAction.Delete && extensionRelationship.ParentExtensionEntityId == parentExtensionRelationship.RelatedEntityId)
                    {
                        String errorMessage = String.Format("Entity extensions {0}ing failed for Entity: {1}. Extensions cannot be deleted as the resulting Extension(s) after delete could not meet the defined container inheritance path", operation, entity.Name);
                        throw new Exception(errorMessage);
                    }
                    else
                    {
                        if (extensionInHierarchy.Direction == RelationshipDirection.Down)
                        {
                            //Check if any extension relationship is available with given category id and container id.If not then add it.
                            if (!parentExtensionRelationship.RelationshipCollection.Contains(extensionInHierarchy.CategoryId, extensionInHierarchy.ContainerId))
                                parentExtensionRelationship.RelationshipCollection.Add(extensionInHierarchy);

                            //Being at second level and trying to extend for 'Down' Direction with virtual catalogs, 
                            //It should not change Direction to 'UP' while validating for Virtual catalogs.

                            //parentExtensionRelationship.Direction = RelationshipDirection.Up;

                            extensionHierarchies.Remove(extensionInHierarchy);
                        }
                        else if (extensionInHierarchy.Direction == RelationshipDirection.Up)
                        {
                            //Check if any extension relationship is available with given category id and container id.If not then add it.
                            if (!extensionInHierarchy.RelationshipCollection.Contains(parentExtensionRelationship.CategoryId, parentExtensionRelationship.ContainerId))
                            {
                                extensionInHierarchy.RelationshipCollection.Add(parentExtensionRelationship);
                                parentExtensionRelationship.Direction = RelationshipDirection.Down;
                            }

                            extensionHierarchies.Remove(parentExtensionRelationship);
                        }
                    }
                }

                #endregion
            }
        }

        private void PopulateMissingExtensionsFromCache(Entity entity, ExtensionRelationshipCollection extensionRelationships, MDMCenterApplication application, MDMCenterModules module)
        {
            ExtensionRelationshipCollection cachedExtensionRelationships = Get(entity.Id, application, module);

            if (cachedExtensionRelationships != null && cachedExtensionRelationships.Count > 0)
            {
                //Vishal..Why we are searching denormalized cache for normalized recursive extension relationship collection
                cachedExtensionRelationships = (ExtensionRelationshipCollection)cachedExtensionRelationships.Denormalize();

                foreach (ExtensionRelationship extension in extensionRelationships)
                {
                    ExtensionRelationship cachedExtension = cachedExtensionRelationships.SingleOrDefault(e => e.ContainerId == extension.ContainerId && e.CategoryId == extension.CategoryId);

                    if (cachedExtension == null)
                    {
                        SetNonCachedExtensionAction(entity, extension);
                    }
                    else if (extension.Action == ObjectAction.Delete)
                        extension.Action = ObjectAction.Delete;
                    else
                        extension.Action = ObjectAction.Read;
                }

                foreach (ExtensionRelationship cachedExtension in cachedExtensionRelationships)
                {
                    IEnumerable<ExtensionRelationship> extensions = extensionRelationships.Where(e => e.ContainerId == cachedExtension.ContainerId && e.CategoryId == cachedExtension.CategoryId);

                    if ((extensions == null || extensions.Count() < 1) && !extensionRelationships.Contains(cachedExtension.RelatedEntityId))
                    {
                        extensionRelationships.Add(cachedExtension);
                    }
                }
            }
            else
            {
                foreach (ExtensionRelationship extension in extensionRelationships)
                {
                    SetNonCachedExtensionAction(entity, extension);
                }
            }
        }

        private void ProcessExtensions(Dictionary<Int64, ExtensionRelationshipCollection> extensionRelationshipsDictionary, Entity entityObject, EntityOperationResult entityOperationResult, EntityCacheStatusCollection entityCacheStatusCollection, ContainerCollection containers, String programName, CallerContext callerContext, ProcessingMode processingMode = ProcessingMode.Sync, Boolean saveCacheStatus = true)
        {
            EntityCollection extendedEntitiesToBeProcessed = new EntityCollection();
            ExtensionRelationshipCollection extensionRelsToBeProcessed = new ExtensionRelationshipCollection();
            Dictionary<Int64, ExtensionRelationshipCollection> extensionRelsDictionaryTobeProcessed = new Dictionary<Int64, ExtensionRelationshipCollection>();

            #region Step : Filter out EER with Read actions

            foreach (KeyValuePair<Int64, ExtensionRelationshipCollection> extensionRelParentIdPair in extensionRelationshipsDictionary)
            {
                ExtensionRelationshipCollection updatedExtensions = new ExtensionRelationshipCollection();

                foreach (ExtensionRelationship extensionRelationship in extensionRelParentIdPair.Value)
                {
                    //ExtensionRelationship.HasObjectChanged will check for child relationship also.
                    //If any of those has Action Update,it returns true.
                    if (extensionRelationship.HasObjectChanged())
                    {
                        updatedExtensions.Add(extensionRelationship);
                        extensionRelsToBeProcessed.Add(extensionRelationship);
                    }
                }

                if (updatedExtensions.Count > 0)
                {
                    extensionRelsDictionaryTobeProcessed.Add(extensionRelParentIdPair.Key, updatedExtensions);
                }
            }

            #endregion

            #region Step : Load related entities

            EntityContext entityContext = new EntityContext();
            entityContext.LoadEntityProperties = true;
            entityContext.LoadAttributes = false;

            Boolean loadRelatedEntitiesRecursive = false;
            OperationResult operationResult = LoadRelatedEntities(extensionRelsToBeProcessed, entityContext, loadRelatedEntitiesRecursive, callerContext);

            #endregion

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, BusinessLogicBase.GetTransactionOptions(processingMode)))
            {
                #region Step: Process each extension

                foreach (KeyValuePair<Int64, ExtensionRelationshipCollection> extensionRelParentIdPair in extensionRelsDictionaryTobeProcessed)
                {
                    foreach (ExtensionRelationship extensionRelationship in extensionRelParentIdPair.Value)
                    {
                        Entity extendedEntity = ConstructExtendedEntity(extensionRelationship, extensionRelParentIdPair.Key, entityObject, containers, callerContext);

                        if (extendedEntity.Action != ObjectAction.Read)
                        {
                            extendedEntitiesToBeProcessed.Add(extendedEntity);
                        }
                    }
                }

                #region Process extended entities

                if (extendedEntitiesToBeProcessed.Count > 0)
                {
                    SaveEntity(extendedEntitiesToBeProcessed, entityOperationResult, extensionRelsToBeProcessed, processingMode, callerContext);
                }

                #endregion

                if (entityOperationResult != null && entityOperationResult.OperationResultStatus != OperationResultStatusEnum.Failed)
                {
                #region Process child extension relationships

                Dictionary<Int64, ExtensionRelationshipCollection> childExtensionRelsDictionaryToBeProcessed = new Dictionary<Int64, ExtensionRelationshipCollection>();

                foreach (ExtensionRelationship extensionRelationship in extensionRelsToBeProcessed)
                {
                    Entity extendedEntity = extensionRelationship.RelatedEntity;

                    if (extendedEntity != null)
                    {
                        #region ReParent current entity if having UP direction relationships

                        if (extensionRelationship.Direction == RelationshipDirection.Up
                                && entityObject.ParentExtensionEntityId != extendedEntity.Id
                                && extendedEntity.Id != entityObject.Id
                                && extensionRelationship.Action != ObjectAction.Read
                                && extendedEntity.Action != ObjectAction.Reclassify)
                        {
                            //entityObject.Action = ObjectAction.ReParent; //Since we are doing reparent right here, dont mess up with entity.Action

                            this.ReParent(extendedEntity.Id, entityObject.Id, callerContext, processingMode);

                            #region Alternate way to do reparent..but this is not required..

                            //EntityMoveContext entityMoveContext = new EntityMoveContext();
                            //entityMoveContext.ReParentType = ReParentTypeEnum.ExtensionReParent;
                            //entityMoveContext.TargetParentEntityId = extendedEntity.Id;

                            //entityObject.EntityMoveContext = entityMoveContext;

                            //SaveEntity(entityObject, extensionRelationship, callerContext);

                            #endregion
                        }

                        #endregion

                        if (extensionRelationship.RelationshipCollection != null && extensionRelationship.RelationshipCollection.Count > 0)
                        {
                            childExtensionRelsDictionaryToBeProcessed.Add(extensionRelationship.RelatedEntity.Id, extensionRelationship.RelationshipCollection);
                        }
                    }
                }

                if (childExtensionRelsDictionaryToBeProcessed.Count > 0)
                {                    
                        ProcessExtensions(childExtensionRelsDictionaryToBeProcessed, entityObject, entityOperationResult, entityCacheStatusCollection, containers,
                        programName, callerContext, processingMode);
                }

                #endregion

                #region Step: Clear extension relationships cache for entity in context           

                ClearExtensionRelationshipCache(entityObject, extensionRelationshipsDictionary, entityCacheStatusCollection);

                #endregion

                #region Save Cache Status

                // Update cache only if the call is from API. For Entity.Process, the save is not performed here.  
                if (saveCacheStatus)
                {
                    EntityCacheStatusBL entityCacheStatusBL = new EntityCacheStatusBL();
                    entityCacheStatusBL.Process(entityCacheStatusCollection, callerContext);
                }

                #endregion

                }
                
                #endregion

                //Commit transaction
                transactionScope.Complete();
            }
        }

        private void ClearExtensionRelationshipCache(Entity entity, Dictionary<Int64, ExtensionRelationshipCollection> extensionRelationshipsDictionary, EntityCacheStatusCollection entityCacheStatusCollection)
        {
            EntityCacheStatus entityCacheStatus = entityCacheStatusCollection.GetOrCreateEntityCacheStatus(entity.Id, entity.ContainerId);
            entityCacheStatus.IsExtensionsCacheDirty = true;
            entityCacheStatus.IsBaseEntityCacheDirty = true;                

            ExtensionRelationshipCollection extensionRelationshipCollection = new ExtensionRelationshipCollection();
            foreach (KeyValuePair<Int64, ExtensionRelationshipCollection> extensionRelParentIdPair in extensionRelationshipsDictionary)
            {
                foreach (ExtensionRelationship extensionRelationship in extensionRelParentIdPair.Value)
                {
                    extensionRelationshipCollection.Add(extensionRelationship);
                }
            }

            ClearExtendedEntitiesExtRelCache(extensionRelationshipCollection, entityCacheStatusCollection, entity.Locale);
        }

        private void ClearExtendedEntitiesExtRelCache(ExtensionRelationshipCollection extensionRelationships, EntityCacheStatusCollection entityCacheStatusCollection, LocaleEnum locale)
        {
            EntityCacheStatus entityCacheStatus = null;

            foreach (ExtensionRelationship extensionRelationship in extensionRelationships)
            {
                if (extensionRelationship.RelatedEntityId > 0)
                {
                    entityCacheStatus = entityCacheStatusCollection.GetOrCreateEntityCacheStatus(extensionRelationship.RelatedEntityId, extensionRelationship.ContainerId);
                    entityCacheStatus.IsExtensionsCacheDirty = true;
                    entityCacheStatus.IsBaseEntityCacheDirty = true;
                }

                ExtensionRelationshipCollection childExtensionRelationships = extensionRelationship.RelationshipCollection;

                if (childExtensionRelationships != null && childExtensionRelationships.Count > 0)
                {
                    ClearExtendedEntitiesExtRelCache(childExtensionRelationships, entityCacheStatusCollection, locale);
                }
            }
        }

        private void GetSecurityPrincipal()
        {
            if (_securityPrincipal == null)
            {
                _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            }
        }

        private Entity GetCategory(Int64 categoryId, Int32 containerId, CallerContext callerContext)
        {
            Entity categoryEntity = null;

            //Check in category dictionary
            if (_categoryDictionary.ContainsKey(categoryId))
            {
                categoryEntity = _categoryDictionary[categoryId];
            }
            else
            {
                //Get system data 
                LocaleEnum systemDataLocale = GlobalizationHelper.GetSystemDataLocale();

                //Prepare category context
                EntityContext categoryContext = new EntityContext();
                categoryContext.LoadEntityProperties = true;
                categoryContext.LoadCreationAttributes = false;
                categoryContext.LoadAttributes = false;
                categoryContext.LoadExtensionRelationships = false;
                categoryContext.LoadHierarchyRelationships = false;
                categoryContext.LoadRelationships = false;
                categoryContext.LoadRequiredAttributes = false;
                categoryContext.EntityTypeId = 6;
                categoryContext.ContainerId = containerId;
                categoryContext.DataLocales.Add(systemDataLocale);
                categoryContext.Locale = systemDataLocale;
                categoryContext.LoadWorkflowInformation = false;

                //Get call for Category
                EntityBL entityManager = new EntityBL();
                categoryEntity = entityManager.Get(categoryId, categoryContext, callerContext.Application, callerContext.Module, false, false);

                //Put into dictionary
                _categoryDictionary.Add(categoryId, categoryEntity);
            }

            return categoryEntity;
        }

        private Entity BuildEntityClone(Entity entity, ExtensionRelationship extensionRelationship, Container container, CallerContext callerContext)
        {
            EntityBL entityBL = new EntityBL();

            //Clone the entity object with the Id as Related Entity Id
            Entity extendedEntity = new Entity();
            extendedEntity.Id = extensionRelationship.RelatedEntityId;

            extendedEntity.ExternalId = entity.ExternalId;
            extendedEntity.Name = entity.Name;
            extendedEntity.LongName = entity.LongName;
            extendedEntity.OrganizationId = container.OrganizationId;
            extendedEntity.OrganizationName = container.OrganizationShortName;
            extendedEntity.ContainerId = extensionRelationship.ContainerId;
            extendedEntity.ContainerName = container.Name;
            extendedEntity.CategoryId = extensionRelationship.CategoryId;
            extendedEntity.CategoryName = extensionRelationship.CategoryName;
            extendedEntity.CategoryPath = extensionRelationship.CategoryPath;
            extendedEntity.EntityTypeId = entity.EntityTypeId;
            extendedEntity.EntityTypeName = entity.EntityTypeName;
            extendedEntity.ParentEntityId = extensionRelationship.CategoryId;
            extendedEntity.Action = extensionRelationship.Action;

            //Get category details
            if (String.IsNullOrWhiteSpace(extendedEntity.CategoryName)
                || String.IsNullOrWhiteSpace(extendedEntity.CategoryPath))
            {
                Entity category = GetCategory(extensionRelationship.CategoryId, extensionRelationship.ContainerId, callerContext);

                if (category != null)
                {
                    extendedEntity.CategoryName = category.Name;
                    extendedEntity.CategoryPath = category.CategoryPath;
                }
            }

            //TODO::Change the identification logic to determine whether entity is of type SKU
            if (entity.CategoryId != entity.ParentEntityId)
            {
                //Entity's parent is of type Entity
                //Get the Entity id of the parent in the container where it is going to be extended using Entity Maps
                //To do this, we need entity type of the parent entity in the target container. This entity type is same as the entity type of the current entity's parent
                //So, get the parent entity
                EntityContext entityContext = new EntityContext(0, 0, 0, entity.Locale, entity.EntityContext.DataLocales, true, false, false, false, null, null, false, false, null, false, false, AttributeModelType.Unknown);
                Entity parentEntity = entityBL.Get(entity.ParentEntityId, entityContext, false, callerContext.Application, callerContext.Module, false, false);

                if (parentEntity != null)
                {
                    EntityMapBL entityMapBL = new EntityMapBL();
                    EntityMap entityMap = entityMapBL.Get("Internal", entity.ParentEntityName, extensionRelationship.ContainerId, parentEntity.EntityTypeId, extensionRelationship.CategoryId, null, callerContext.Application, callerContext.Module);

                    if (entityMap != null && entityMap.InternalId > 0)
                    {
                        extendedEntity.ParentEntityId = entityMap.InternalId;
                    }
                }
            }

            return extendedEntity;
        }

        private Boolean SaveEntity(EntityCollection entities, EntityOperationResult entityOperationResult, ExtensionRelationshipCollection extensionRelationships, ProcessingMode processingMode, CallerContext callerContext)
        {
            Boolean successFlag = true;
            EntityBL entityManager = new EntityBL();
            EntityProcessingOptions entityProcessingOptions = new EntityProcessingOptions(true, true, false, true);
            entityProcessingOptions.ProcessingMode = processingMode;

            if (String.IsNullOrWhiteSpace(callerContext.ProgramName))
            {
                callerContext.ProgramName = "ExtensionRelationshipBL.ProcessExtensions.SaveEntity";
            }

            EntityOperationResultCollection entityOperationResultCollection = entityManager.Process(entities, entityProcessingOptions, callerContext);

            if (entityOperationResultCollection != null && entityOperationResultCollection.Count > 0)
            {
                EntityOperationResult processedEntityOperationResult = entityOperationResultCollection.FirstOrDefault();

                if (processedEntityOperationResult != null && processedEntityOperationResult.Errors != null && processedEntityOperationResult.Errors.Count > 0)
                {
                    String errorMessage;
                    Error processError = processedEntityOperationResult.Errors[0]; 

                    if (_securityPrincipal.UserPreferences != null)
                    {
                        _localeMessage = _localeMessageManager.Get(GlobalizationHelper.GetSystemUILocale(), processError.ErrorCode, processError.Params.ToArray(), false, callerContext);
                        errorMessage = _localeMessage != null && !String.IsNullOrEmpty(_localeMessage.Message) ? _localeMessage.Message : processError.ErrorMessage;
                    }
                    else
                    {
                        errorMessage = processError.ErrorMessage;
                    }

                    entityOperationResult.AddOperationResult(processError.ErrorCode, errorMessage, OperationResultType.Error);
                }
                else
                {
                    foreach (Entity entity in entities)
                    {
                        //Get extension relationship for this entity..
                        ExtensionRelationship extensionRelationship = extensionRelationships.FirstOrDefault(e => e.ContainerId == entity.ContainerId && e.CategoryId == entity.CategoryId);

                        if (extensionRelationship != null)
                        {
                            if (entity.Id < 1 && extensionRelationship.Action == ObjectAction.Create)
                            {
                                String errorMessage = String.Format("Entity extensions processing for Container: {0} and Category: {1} failed.", extensionRelationship.ContainerLongName, extensionRelationship.CategoryLongName);
                                throw new Exception(errorMessage);
                            }
                            else
                            {
                                extensionRelationship.RelatedEntityId = entity.Id;
                                extensionRelationship.RelatedEntity = entity;
                            }
                        }
                        else
                        {
                            //TODO::This is the rare case.. Do we need to throw error here?
                        }
                    }
                }
            }

            return successFlag;
        }

        private void SetNonCachedExtensionAction(Entity entity, ExtensionRelationship extension)
        {
            if (extension.RelatedEntityId > 0)
            {
                if (entity.Id == extension.RelatedEntityId)
                {
                    if (entity.CategoryId == extension.CategoryId)
                    {
                        if (extension.Action != ObjectAction.Delete)
                            extension.Action = ObjectAction.Read;
                    }
                    else
                    {
                        extension.Action = ObjectAction.Update;
                    }
                }
                else
                {
                    extension.Action = ObjectAction.Update;
                }
            }
            else
                extension.Action = ObjectAction.Create;
        }

        private Entity ConstructExtendedEntity(ExtensionRelationship extensionRelationship, Int64 parentEntityId, Entity entityObject, ContainerCollection containers, CallerContext callerContext)
        {
            Entity extendedEntity = null;

            //Considering first INH path assuming that INH path will be defined properly as per organization
            Container container = containers.First(c => c.Id == extensionRelationship.ContainerId);

            if (extensionRelationship.RelatedEntity != null)
            {
                extendedEntity = extensionRelationship.RelatedEntity;

                //If extensionRelationship.Action is coming with Delete Action.
                //It means set extendedEntity.Action also as Delete to delete extension as well as extended entity.
                extendedEntity.Action = extensionRelationship.Action == ObjectAction.Delete ? extensionRelationship.Action : ObjectAction.Update;
            }
            else
            {
                extendedEntity = BuildEntityClone(entityObject, extensionRelationship, container, callerContext);
                extendedEntity.Action = ObjectAction.Create;

                if (extensionRelationship.Direction == RelationshipDirection.Up && parentEntityId == entityObject.Id)
                    extendedEntity.ParentExtensionEntityId = 0;
                else
                    extendedEntity.ParentExtensionEntityId = parentEntityId;

                //Make sure entity is not already existing..
                EntityMapBL entityMapManger = new EntityMapBL();
                EntityMap entityMapForExtendedEntity = entityMapManger.Get("Internal", extendedEntity.Name, extendedEntity.ContainerId, extendedEntity.EntityTypeId, extendedEntity.CategoryId, null, callerContext.Application, callerContext.Module);

                if (entityMapForExtendedEntity != null && entityMapForExtendedEntity.InternalId > 0)
                {
                    extendedEntity.Id = entityMapForExtendedEntity.InternalId;
                    extendedEntity.ParentExtensionEntityId = 0;
                    extendedEntity.Action = ObjectAction.Update;
                }
            }

            if (extendedEntity.Action == ObjectAction.Update && extendedEntity.Id > 0)
            {
                //Que: What if item exists in the target but in different category...should we do reclassify or reparent or both(reparent + reclass)?

                // Set Reclassify action for entity as category id is different
                if (extendedEntity.CategoryId != extensionRelationship.CategoryId
                    && extensionRelationship.Action != ObjectAction.Read)
                {
                    extendedEntity.Action = ObjectAction.Reclassify;

                    if (String.IsNullOrWhiteSpace(extensionRelationship.CategoryPath))
                    {
                        Entity category = GetCategory(extensionRelationship.CategoryId, extendedEntity.ContainerId, callerContext);

                        if (category != null)
                        {
                            extensionRelationship.CategoryName = category.Name;
                            extensionRelationship.CategoryPath = category.CategoryPath;
                        }
                    }

                    EntityMoveContext entityMoveContext = new EntityMoveContext();
                    entityMoveContext.ReParentType = ReParentTypeEnum.CategoryReParent;
                    entityMoveContext.TargetCategoryId = extensionRelationship.CategoryId;
                    entityMoveContext.TargetCategoryName = extensionRelationship.CategoryName;
                    entityMoveContext.TargetCategoryPath = extensionRelationship.CategoryPath;
                    entityMoveContext.FromCategoryId = extendedEntity.CategoryId;

                    extendedEntity.EntityMoveContext = entityMoveContext;
                }
                else if (extendedEntity.Id == parentEntityId)
                {
                    extendedEntity.Action = ObjectAction.Read;
                }
                else if (extendedEntity.ParentExtensionEntityId != parentEntityId
                         && extensionRelationship.Direction == RelationshipDirection.Down)
                {
                    //Set ReParent action for entity as parent extension entity id is different
                    extendedEntity.Action = ObjectAction.ReParent;

                    EntityMoveContext entityMoveContext = new EntityMoveContext();
                    entityMoveContext.ReParentType = ReParentTypeEnum.ExtensionReParent;
                    entityMoveContext.TargetParentExtensionEntityId = parentEntityId;

                    extendedEntity.EntityMoveContext = entityMoveContext;
                }
                else if (extendedEntity.ParentExtensionEntityId != parentEntityId
                        && extensionRelationship.Direction == RelationshipDirection.Up)
                {
                    //Don't do anything.Just make sure else condition won't change extendedEntity.Action as Read.
                    //ReParent region will take of each and everything.
                }
                else
                    extendedEntity.Action = ObjectAction.Read; // Extension relationship processing does not do normal entity update..it has to be either reclassify or extension or reparenting..
            }

            return extendedEntity;
        }

        #endregion

        #endregion
    }
}