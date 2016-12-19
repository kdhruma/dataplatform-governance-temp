using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Transactions;
using MDM.ActivityLogManager.Business;

namespace MDM.DataModelManager.Business
{
    using BufferManager;
    using BusinessObjects;
    using ConfigurationManager.Business;
    using Core;
    using Core.Exceptions;
    using Data;
    using MessageManager.Business;
    using Utility;
    using Interfaces;
    using BusinessObjects.DataModel;
    using JigsawIntegrationManager;
    using JigsawIntegrationManager.DataPackages;
    using MessageBrokerManager;
    using Newtonsoft.Json.Linq;
    using Core.Extensions;

    /// <summary>
    /// Specifies operations for Entity type
    /// </summary>
    public class EntityTypeBL : BusinessLogicBase, IEntityTypeManager, IDataModelManager
    {
        #region Fields

        /// <summary>
        /// Field denoting locale Message
        /// </summary>
        private LocaleMessage _localeMessage;

        /// <summary>
        /// Field denoting locale Message
        /// </summary>
        private LocaleEnum _systemUILocale = LocaleEnum.UnKnown;

        /// <summary>
        /// Field denoting localeMessageBL
        /// </summary>
        private LocaleMessageBL _localeMessageBL = new LocaleMessageBL();

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public EntityTypeBL()
        {
            _systemUILocale = GlobalizationHelper.GetSystemUILocale();
        }

        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        #region CUD Methods

        /// <summary>
        /// Create new entity type
        /// </summary>
        /// <param name="entityType">EntityType which is to be created</param>
        /// <returns>1 if entity type created successfully, otherwise 0</returns>
        public OperationResult Create(EntityType entityType, CallerContext callerContext)
        {
            ValidateEntityType(entityType, "Create");

            entityType.Action = ObjectAction.Create;
            return this.Process(entityType, callerContext);
        }

        /// <summary>
        /// Update existing entity type
        /// </summary>
        /// <param name="entityType">Entity type to update</param>
        /// <returns>1 if entity type updated successfully, otherwise 0</returns>
        public OperationResult Update(EntityType entityType, CallerContext callerContext)
        {
            ValidateEntityType(entityType, "Update");

            entityType.Action = ObjectAction.Update;
            return this.Process(entityType, callerContext);
        }

        /// <summary>
        /// Delete entity type
        /// </summary>
        /// <param name="entityType">Entity type to delete</param>
        /// <returns>1 if entity type deleted successfully, otherwise 0</returns>
        public OperationResult Delete(EntityType entityType, CallerContext callerContext)
        {
            ValidateEntityType(entityType, "Delete");

            entityType.Action = ObjectAction.Delete;
            return this.Process(entityType, callerContext);
        }

        /// <summary>
        /// Process entityTypes
        /// </summary>
        /// <param name="EntityTypes">EntityTypes to process</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of EntityType process</returns>
        public OperationResultCollection Process(EntityTypeCollection entityTypes, CallerContext callerContext)
        {
            #region Parameter Validation

            ValidateEntityTypes(entityTypes);

            #endregion Parameter Validation

            OperationResultCollection operationResults = new OperationResultCollection();

            foreach (EntityType entityType in entityTypes)
            {
                OperationResult entityTypeOR = this.Process(entityType, callerContext, false);
                if (entityTypeOR != null)
                {
                    operationResults.Add(entityTypeOR);
                }
            }

            if (JigsawConstants.IsJigsawIntegrationEnabled)
            {
                SendToJigsaw(callerContext);
            }

            #region activity log

            if ((operationResults.OperationResultStatus == OperationResultStatusEnum.Successful || operationResults.OperationResultStatus == OperationResultStatusEnum.None) && AppConfigurationHelper.GetAppConfig<Boolean>(Constants.ACTIVITYLOG_DATAMODEL_ENABLED_APPCONFIG_KEY, false))
            {
                callerContext.ProgramName = "EntityTypeBL";
                LogDataModelChanges(entityTypes, callerContext);
            }

            #endregion activity log

            return operationResults;
        }

        #endregion CUD Methods

        #region Get Methods

        /// <summary>
        /// Get all entity types in the system
        /// </summary>
        /// <param name="callerContext">Indicates name of application and module.</param>
        /// <param name="getLatest">Boolean flag which says whether to get from DB or cache. True means always get from DB</param>
        /// <returns>All entity types</returns>
        public EntityTypeCollection GetAll(CallerContext callerContext, Boolean getLatest = false)
        {
            EntityTypeCollection entityTypes = new EntityTypeCollection();

            Dictionary<Int32, EntityType> allEntityTypesDictionary = GetAllEntityTypes(callerContext, getLatest);

            if (allEntityTypesDictionary != null)
            {
                foreach (EntityType entityType in allEntityTypesDictionary.Values)
                {
                    entityTypes.Add(entityType);
                }
            }

            return entityTypes;
        }

        /// <summary>
        /// Get entity type by id
        /// </summary>
        /// <param name="id">Id using which Entity type is to be fetched</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>EntityType with Id specified. Otherwise null</returns>
        public EntityType GetById(Int32 id, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityTypeBL.GetById", MDMTraceSource.DataModel, false);

            EntityType entityType = null;

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested entity type id :{0} ", id), MDMTraceSource.DataModel);

                Dictionary<Int32, EntityType> entityTypes = GetAllEntityTypes(callerContext);

                if (entityTypes != null && entityTypes.Count > 0)
                {
                    if (entityTypes.ContainsKey(id))
                        entityType = entityTypes[id];
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("EntityTypeBL.GetById", MDMTraceSource.DataModel);
            }

            return entityType;
        }

        /// <summary>
        /// Get entity type based on its unique identifier - i.e entity type name and parent entity type name
        /// </summary>
        /// <param name="entityTypeUniqueIdentifier">Unique identifier which identifies entity type uniquely in system</param>
        /// <param name="callerContext">caller context indicating who called the API</param>
        /// <returns></returns>
        [Obsolete("This method has been obsoleted. Please use GetByName method instead of this")]
        public EntityType GetByUniqueIdentifier(EntityTypeUniqueIdentifier entityTypeUniqueIdentifier, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityTypeBL.GetByUniqueIdentifier", MDMTraceSource.DataModel, false);

            #region Parameter Validation

            ValidateCallerContext(callerContext);
            ValidateEntityTypeUniqueIdentifier(entityTypeUniqueIdentifier, callerContext);

            #endregion

            EntityType entityType = null;

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested entity type name :{0} , parent entity type name:{1} ", entityTypeUniqueIdentifier.EntityTypeName, entityTypeUniqueIdentifier.ParentEntityTypeName), MDMTraceSource.DataModel);

                entityType = GetEntityTypeByShortName(entityTypeUniqueIdentifier.EntityTypeName, callerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("EntityTypeBL.GetByUniqueIdentifier", MDMTraceSource.DataModel);
            }

            return entityType;
        }

        /// <summary>
        /// Get entity type collection based on its unique identifier - i.e entity type name and parent entity type name
        /// </summary>
        /// <param name="entityTypeUniqueIdentifiers">Unique identifier which identifies entity type uniquely in system</param>
        /// <param name="callerContext">caller context indicating who called the API</param>
        /// <returns></returns>
        [Obsolete("This method has been obsoleted. Please use GetByNames method instead of this")]
        public EntityTypeCollection GetByUniqueIdentifiers(EntityTypeUniqueIdentifierCollection entityTypeUniqueIdentifiers, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityTypeBL.GetByUniqueIdentifiers", MDMTraceSource.DataModel, false);

            #region Parameter validation

            ValidateCallerContext(callerContext);

            if (entityTypeUniqueIdentifiers == null)
            {
                String errorMessage = this.GetSystemLocaleMessage("112644", callerContext).Message;
                throw new MDMOperationException("112644", errorMessage, "DataModelManager.EntityTypeBL", String.Empty, "GetByUniqueIdentifiers");
            }

            #endregion

            EntityTypeCollection entityTypes = null;

            try
            {
                if (entityTypeUniqueIdentifiers != null && entityTypeUniqueIdentifiers.Count > 0)
                {
                    Collection<String> entityTypeNames = new Collection<String>();

                    foreach (EntityTypeUniqueIdentifier entityTypeUniqueIdentifier in entityTypeUniqueIdentifiers)
                    {
                        ValidateEntityTypeUniqueIdentifier(entityTypeUniqueIdentifier, callerContext);
                        entityTypeNames.Add(entityTypeUniqueIdentifier.EntityTypeName);
                    }

                    entityTypes = GetEntityTypesByShortNames(entityTypeNames, callerContext);
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("EntityTypeBL.GetByUniqueIdentifiers", MDMTraceSource.DataModel);
            }

            return entityTypes;
        }

        /// <summary>
        /// Get entity type based on its name
        /// </summary>
        /// <param name="entityTypeShortName">Unique identifier which identifies entity type uniquly in system</param>
        /// <param name="callerContext">caller context indicating who called the API</param>
        /// <returns></returns>
        [Obsolete("This method has been obsoleted. Please use GetByNames method instead of this")]
        public EntityTypeCollection GetByName(String entityTypeShortName, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityTypeBL.GetByName", MDMTraceSource.DataModel, false);

            #region Parameter validation

            ValidateCallerContext(callerContext);

            #endregion

            EntityTypeCollection entityTypes = null;
            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested entity type name :{0} ", entityTypeShortName), MDMTraceSource.DataModel);

                EntityType entityType = GetEntityTypeByShortName(entityTypeShortName, callerContext);
                if (entityType != null)
                {
                    entityTypes.Add(entityType);
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("EntityTypeBL.GetByName", MDMTraceSource.DataModel);
            }

            return entityTypes;
        }

        /// <summary>
        /// Get entity type collection based on its unique short name 
        /// </summary>
        /// <param name="entityTypeShortNames">Unique identifier which identifies entity type uniquly in system</param>
        /// <param name="callerContext">caller context indicating who called the API</param>
        /// <returns></returns>
        public EntityTypeCollection GetByNames(Collection<String> entityTypeShortNames, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityTypeBL.GetByNames", MDMTraceSource.DataModel, false);

            EntityTypeCollection entityTypes = null;

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested for entity types based on short names provided."), MDMTraceSource.DataModel);

                entityTypes = GetEntityTypesByShortNames(entityTypeShortNames, callerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("EntityTypeBL.GetByNames", MDMTraceSource.DataModel);
            }

            return entityTypes;
        }

        /// <summary>
        /// Get entity type based on its name
        /// </summary>
        /// <param name="entityTypeShortName">Unique identifier which identifies entity type uniquly in system</param>
        /// <param name="callerContext">caller context indicating who called the API</param>
        /// <returns></returns>
        public EntityType GetByShortName(String entityTypeShortName, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityTypeBL.GetByShortName", MDMTraceSource.DataModel, false);

            EntityType entityType = null;

            try
            {
                entityType = GetEntityTypeByShortName(entityTypeShortName, callerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("EntityTypeBL.GetByShortName", MDMTraceSource.DataModel);
            }

            return entityType;
        }

        /// <summary>
        /// Get all entity types by list of ids
        /// </summary>
        /// <param name="entityTypeIds">Collection of EntityType Ids to search in the system</param>
        /// <returns>Collection of EntityTypes with specified Ids in the Id list</returns>
        public EntityTypeCollection GetEntityTypesByIds(Collection<Int32> entityTypeIds)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityTypeBL.GetEntityTypesByIds", MDMTraceSource.DataModel, false);

            EntityTypeCollection matchedEntityTypes = new EntityTypeCollection();

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested entity type ids :{0} ", ValueTypeHelper.JoinCollection(entityTypeIds, ",")), MDMTraceSource.DataModel);

                Dictionary<Int32, EntityType> allEntityTypes = GetAllEntityTypes(new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.Modeling));

                if (allEntityTypes != null && allEntityTypes.Count > 0)
                {
                    foreach (Int32 id in entityTypeIds)
                    {
                        if(allEntityTypes.ContainsKey(id))
                            matchedEntityTypes.Add(allEntityTypes[id]);
                    }
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("EntityTypeBL.GetEntityTypesByIds", MDMTraceSource.DataModel);
            }

            return matchedEntityTypes;
        }

        #endregion

        #region IDataModelManager Methods

        /// <summary>
        /// Generate and OperationResult Schema for given DataModelObjectCollection
        /// </summary>
        /// <param name="iDataModelObjects"></param>
        /// <param name="operationResults">Collection of DataModel OperationResult</param>
        public void PrepareOperationResultsSchema(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults)
        {
            EntityTypeCollection entityTypes = iDataModelObjects as EntityTypeCollection;

            if (entityTypes != null && entityTypes.Count > 0)
            {
                if (operationResults == null)
                {
                    operationResults = new DataModelOperationResultCollection();
                }
                else if (operationResults.Count > 0)
                {
                    operationResults.Clear();
                }

                Int32 entityTypeToBeCreated = -1;

                foreach (EntityType entityType in entityTypes)
                {
                    DataModelOperationResult entityTypeOperationResult = new DataModelOperationResult(entityType.Id, entityType.LongName, entityType.ExternalId, entityType.ReferenceId);

                    entityTypeOperationResult.DataModelObjectType = ObjectType.EntityType;

                    if (String.IsNullOrEmpty(entityTypeOperationResult.ExternalId))
                    {
                        entityTypeOperationResult.ExternalId = entityType.Name;
                    }

                    if (entityType.Id < 1)
                    {
                        entityType.Id = entityTypeToBeCreated;
                        entityTypeOperationResult.Id = entityTypeToBeCreated;
                        entityTypeToBeCreated--;
                    }

                    operationResults.Add(entityTypeOperationResult);
                }
            }
        }

        /// <summary>
        /// Validates DataModelObjects and populate OperationResults
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be filled up.</param>
        /// <param name="operationResults">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void Validate(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            ValidateInputParameters(iDataModelObjects as EntityTypeCollection, operationResults, iCallerContext as CallerContext);
        }

        /// <summary>
        /// Load original data model objects
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to load original object.</param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void LoadOriginal(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResult, ICallerContext iCallerContext)
        {
            EntityTypeCollection entityTypes = iDataModelObjects as EntityTypeCollection;

            if (entityTypes != null)
            {
                LoadOriginalEntityTypes(entityTypes, iCallerContext as CallerContext);
            }
        }

        /// <summary>
        /// Fills missing information to data model objects.
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be filled up.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void FillDataModel(IDataModelObjectCollection iDataModelObjects, ICallerContext iCallerContext)
        {
            FillEntityTypes(iDataModelObjects as EntityTypeCollection, iCallerContext as CallerContext);
        }

        /// <summary>
        /// Compare and merge data model object collection with current collection
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be compare and merge.</param>
        /// <param name="operationResults">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>Returns merged data model objects.</returns>
        public void CompareAndMerge(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            CompareAndMerge(iDataModelObjects as EntityTypeCollection, operationResults, iCallerContext as CallerContext);
        }

        /// <summary>
        /// Process data model collection
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be processed.</param>
        /// <param name="operationResults">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>Result of operation</returns>
        public void Process(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            EntityTypeCollection entityTypes = iDataModelObjects as EntityTypeCollection;
            CallerContext callerContext = (CallerContext)iCallerContext;

            if (entityTypes.Count > 0)
            {
                String userName = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;
                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Execute);

                #region Perform entity type updates in database

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    new EntityTypeDA().Process(entityTypes, operationResults, callerContext.ProgramName, userName, command);
                    transactionScope.Complete();
                }

                LocalizeErrors(operationResults, callerContext);

                #region activity log

                if (operationResults.OperationResultStatus == OperationResultStatusEnum.Successful && AppConfigurationHelper.GetAppConfig<Boolean>(Constants.ACTIVITYLOG_DATAMODEL_ENABLED_APPCONFIG_KEY, false))
                {
                    callerContext.ProfileName = "EntityTypeBL";
                    LogDataModelChanges(entityTypes, callerContext);
                }

                #endregion activity log

                #endregion
            }
        }

        /// <summary>
        /// Processes the Entity cache statuses for data model objects
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be process entity cache load.</param>
        /// <param name="operationResults">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void InvalidateCache(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            new EntityTypeBufferManager().RemoveEntityTypes(false);
        }

        #endregion

        #endregion Public Methods

        #region Private Methods

        #region Get Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callerContext"></param>
        /// <param name="loadLatestFromDB"></param>
        /// <returns></returns>
        private Dictionary<Int32, EntityType> GetAllEntityTypes(CallerContext callerContext, Boolean loadLatestFromDB = false)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityTypeBL.GetAll", MDMTraceSource.DataModel, false);

            EntityTypeCollection entityTypes = new EntityTypeCollection();
            Dictionary<Int32, EntityType> allEntityTypes = new Dictionary<Int32, EntityType>();

            try
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "CallerContext : Application - " + callerContext.Application + " Module - " + callerContext.Module);
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Finding Entity Type in cache...", MDMTraceSource.DataModel);
                }

                if (!loadLatestFromDB)
                    allEntityTypes = new EntityTypeBufferManager().FindAllEntityTypes();

                if (allEntityTypes == null || allEntityTypes.Count < 1)
                {
                    if (Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "No Entity Types cache found.Now all entity Types would be loaded from database.", MDMTraceSource.DataModel);
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Loading entity types from database...", MDMTraceSource.DataModel);
                    }

                    DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

                    allEntityTypes = new EntityTypeDA().GetAll(command);

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Loaded entity types from database.", MDMTraceSource.DataModel);

                    if (allEntityTypes != null && allEntityTypes.Count > 0)
                    {
                        if (Constants.TRACING_ENABLED)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Caching {0} entity types...", entityTypes.Count, MDMTraceSource.DataModel));

                        new EntityTypeBufferManager().UpdateEntityTypes(allEntityTypes, 3);

                        if (Constants.TRACING_ENABLED)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Done with caching for EntityTypes.", MDMTraceSource.DataModel);
                    }
                }
                else
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Found {0} entity types in cache.", entityTypes.Count), MDMTraceSource.DataModel);
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("EntityTypeBL.GetAll", MDMTraceSource.DataModel);
            }

            return allEntityTypes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityTypeShortName"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private EntityType GetEntityTypeByShortName(String entityTypeShortName, CallerContext callerContext)
        {
            #region Parameter Validation

            ValidateCallerContext(callerContext);
            ValidateEntityTypeShortName(entityTypeShortName, callerContext);

            #endregion

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested entity type name :{0} ", entityTypeShortName), MDMTraceSource.DataModel);

            EntityType entityType = null;

            Dictionary<Int32, EntityType> entityTypes = GetAllEntityTypes(callerContext);

            if (entityTypes != null && entityTypes.Count > 0)
            {
                foreach (EntityType eType in entityTypes.Values)
                {
                    if (eType.Name.Equals(entityTypeShortName))
                    {
                        entityType = eType;
                        break;
                    }
                }
            }

            return entityType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityTypeShortNames"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private EntityTypeCollection GetEntityTypesByShortNames(Collection<String> entityTypeShortNames, CallerContext callerContext)
        {
            EntityTypeCollection entityTypes = new EntityTypeCollection();

            Dictionary<Int32, EntityType> allEntityTypes = GetAllEntityTypes(callerContext);

            if (entityTypeShortNames != null && entityTypeShortNames.Count > 0)
            {
                if (allEntityTypes != null && allEntityTypes.Count > 0)
                {
                    foreach (String entityTypeName in entityTypeShortNames)
                    {
                        EntityType foundEntityType = null;

                        foreach (EntityType eType in allEntityTypes.Values)
                        {
                            if (eType.Name.ToLowerInvariant().Equals(entityTypeName.ToLowerInvariant()))
                            {
                                foundEntityType = eType;
                                break;
                            }
                        }

                        if (foundEntityType != null)
                        {
                            entityTypes.Add(foundEntityType);
                        }
                    }
                }
            }

            return entityTypes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityTypes"></param>
        /// <param name="callerContext"></param>
        private void LoadOriginalEntityTypes(EntityTypeCollection entityTypes, CallerContext callerContext)
        {
            EntityTypeCollection originalEntityTypes = GetAll(callerContext);

            if (originalEntityTypes != null && originalEntityTypes.Count > 0)
            {
                foreach (EntityType entityType in entityTypes)
                {
                    entityType.OriginalEntityType = originalEntityTypes.Get(entityType.Name);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityTypes"></param>
        /// <param name="callerContext"></param>
        private void FillEntityTypes(EntityTypeCollection entityTypes, CallerContext callerContext)
        {
            foreach (EntityType entityType in entityTypes)
            {
                if (entityType.OriginalEntityType != null)
                {
                    entityType.Id = entityType.OriginalEntityType.Id;
                }
            }
        }

        #endregion

        #region Process Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private OperationResult Process(EntityType entityType, CallerContext callerContext, Boolean sendToJigsaw = true)
        {
            OperationResult entityTypeProcessOperationResult = new OperationResult();

            #region Validations

            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "EntityTypeBL.Process", String.Empty, "Process");
            }

            if (entityType == null)
            {
                throw new MDMOperationException("112129", "Entity Type(s) cannot be null.", "EntityTypeBL.Process", String.Empty, "Process");
            }

            if (String.IsNullOrWhiteSpace(callerContext.ProgramName))
            {
                callerContext.ProgramName = "MDM.DataModelManager.Business.EntityTypeBL.Process";
            }

            #region Validate if entity type is used in entity variant definitions

            if (entityType.Action == ObjectAction.Delete)
            {
                Collection<Int32> entityTypeIds = new Collection<Int32>();
                entityTypeIds = PrepareEntityTypeIdsInUse(callerContext);
             
                if (entityTypeIds.Contains(entityType.Id))
                {
                    String errorMessage = "Entity Type(s) cannot be deleted as it is used in entity variant definition.";

                    throw new MDMOperationException("114166", errorMessage, "EntityTypeBL.Process", String.Empty, "Process");
                }
            }

            #endregion 

            #endregion Validations

            String userName = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;
            
            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Update);

            try
            {
                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    entityTypeProcessOperationResult = new EntityTypeDA().Process(entityType, callerContext.ProgramName, userName, command);

                    LocalizeErrors(callerContext, entityTypeProcessOperationResult);

                    new EntityTypeBufferManager().RemoveEntityTypes(false);

                    transactionScope.Complete();
                }

                if (sendToJigsaw && JigsawConstants.IsJigsawIntegrationEnabled)
                {
                    SendToJigsaw(callerContext);
                }
            }
            catch (Exception ex)
            {
                throw new MDMOperationException(ex.Message, ex);
            }
            return entityTypeProcessOperationResult;
        }

        /// <summary>
        /// Sends the entity type messages to jigsaw.
        /// </summary>
        /// <param name="callerContext">The caller context.</param>
        private void SendToJigsaw(CallerContext callerContext)
        {
            JArray typesArray = new JArray();
            
            foreach (var entityType in this.GetAll(callerContext))
            {
                if (entityType!= null)
                {
                    typesArray.Add(entityType.Name.ToJsCompliant());
                }
            }

            AppConfigManageDataPackage appManageDataPackage = new AppConfigManageDataPackage()
            {
                AppName = JigsawIntegrationAppName.manageGovernApp,
                Action = "entityTypeUpdate",
                Name = "entityTypes",
                MessageData = typesArray
            };
            
            if (typesArray.Count > 0)
            {
                MessageBrokerHelper.SendAppConfigManageMessage(new List<AppConfigManageDataPackage> { appManageDataPackage }, callerContext, JigsawCallerProcessType.AppConfigManageMessage, JigsawConstants.IntegrationBrokerType);
            }
        }

        private Collection<Int32> PrepareEntityTypeIdsInUse(CallerContext callerContext)
        {
            Collection<Int32> entityTypeIds = new Collection<Int32>();
     
            EntityVariantDefinitionBL entityVariantDefinitionBL = new EntityVariantDefinitionBL();
            EntityVariantDefinitionCollection entityVariantDefinitions = entityVariantDefinitionBL.GetAll(callerContext);

            if (entityVariantDefinitions != null)
            {
                foreach (EntityVariantDefinition entityVariantDefinition in entityVariantDefinitions)
                {
                    Int32 rootEntityTypeId = entityVariantDefinition.RootEntityTypeId;

                    if (rootEntityTypeId > 0 && !entityTypeIds.Contains(rootEntityTypeId))
                    {
                        entityTypeIds.Add(rootEntityTypeId);
                    }

                    foreach (EntityVariantLevel entityVariantLevel in entityVariantDefinition.EntityVariantLevels)
                    {
                        Int32 childEntityTypeId = entityVariantLevel.EntityTypeId;

                        if (childEntityTypeId > 0 && !entityTypeIds.Contains(childEntityTypeId))
                        {
                            entityTypeIds.Add(childEntityTypeId);
                        }
                    }

                    if (rootEntityTypeId > 0 && !entityTypeIds.Contains(rootEntityTypeId))
                    {
                        entityTypeIds.Add(rootEntityTypeId);
                    }
                }
            }

            return entityTypeIds;
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityTypes"></param>
        /// <param name="operationResults"></param>
        /// <param name="callerContext"></param>
        private void CompareAndMerge(EntityTypeCollection entityTypes, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            foreach (EntityType deltaEntityType in entityTypes)
            {
                IDataModelOperationResult operationResult = operationResults.GetByReferenceId(deltaEntityType.ReferenceId);

                //If delta object Action is Read/Ignore then skip Merge Delta Process.
                if (deltaEntityType.Action == ObjectAction.Read || deltaEntityType.Action == ObjectAction.Ignore)
                    continue;

                IEntityType origEntityType = deltaEntityType.OriginalEntityType;

                if (origEntityType != null)
                {
                    //If delta object Action is Delete then skip Merge Delta process.
                    if (deltaEntityType.Action != ObjectAction.Delete)
                    {
                        origEntityType.MergeDelta(deltaEntityType, callerContext, false);
                    }
                }
                else
                {
                    String errorMessage = String.Empty;
                    if (deltaEntityType.Action == ObjectAction.Delete)
                    {
                        DataModelHelper.AddOperationResult(operationResult, "113600", String.Empty, new Object[] { deltaEntityType.Name }, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                    else
                    {
                        //If original object is not found then set Action as Create always.
                        deltaEntityType.Action = ObjectAction.Create;
                    }
                }
                operationResult.PerformedAction = deltaEntityType.Action;
            }
        }

        #endregion

        #region Validation Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="methodName"></param>
        private void ValidateEntityType(EntityType entityType, String methodName)
        {
            if (entityType == null)
            {
                throw new MDMOperationException("112129", "Entity Type cannot be null.", "MDM.DataModelManager.Business.EntityTypeBL." + methodName, String.Empty, methodName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityTypes"></param>
        private void ValidateEntityTypes(EntityTypeCollection entityTypes)
        {
            if (entityTypes == null || entityTypes.Count < 1)
            {
                throw new MDMOperationException("112129", "Entity Type(s) cannot be null.", "EntityTypeBL.Process", String.Empty, "Process");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callerContext"></param>
        private void ValidateCallerContext(CallerContext callerContext)
        {
            String errorMessage = String.Empty;

            if (callerContext == null)
            {
                errorMessage = "CallerContext cannot be null.";
                throw new MDMOperationException("111846", errorMessage, "DataModelManager.EntityTypeBL", String.Empty, String.Empty, MDMTraceSource.DataModel);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityTypeUniqueIdentifier"></param>
        /// <param name="callerContext"></param>

        #pragma warning disable 0618

        private void ValidateEntityTypeUniqueIdentifier(EntityTypeUniqueIdentifier entityTypeUniqueIdentifier, CallerContext callerContext)
        {
            if (entityTypeUniqueIdentifier == null)
            {
                String errorMessage = this.GetSystemLocaleMessage("112643", callerContext).Message;
                throw new MDMOperationException("112643", errorMessage, "DataModelManager.EntityTypeBL", String.Empty, String.Empty);
            }

            if (entityTypeUniqueIdentifier.EntityTypeName == String.Empty)
            {
                String errorMessage = this.GetSystemLocaleMessage("112648", callerContext).Message;
                throw new MDMOperationException("112648", errorMessage, "DataModelManager.EntityTypeBL", String.Empty, String.Empty);
            }
        }

        #pragma warning restore 0618

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityTypeShortName"></param>
        /// <param name="callerContext"></param>
        private void ValidateEntityTypeShortName(String entityTypeShortName, CallerContext callerContext)
        {
            if (String.IsNullOrEmpty(entityTypeShortName))
            {
                String errorMessage = this.GetSystemLocaleMessage("112648", callerContext).Message;
                throw new MDMOperationException("112648", errorMessage, "DataModelManager.EntityTypeBL", String.Empty, String.Empty);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityTypes"></param>
        /// <param name="operationResults"></param>
        /// <param name="callerContext"></param>
        private void ValidateInputParameters(EntityTypeCollection entityTypes, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            String errorMessage = String.Empty;
            Collection<String> shortNames = new Collection<String>();

            if (callerContext == null)
            {
                DataModelHelper.AddOperationResults(operationResults, "111846", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
            if (entityTypes == null || entityTypes.Count < 1)
            {
                DataModelHelper.AddOperationResults(operationResults, "113589", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
            else
            {
                Collection<String> internalentityTypeNames = MDM.Core.DataModel.InternalObjectCollection.EntityTypeNames;
                foreach (EntityType entityType in entityTypes)
                {
                    IDataModelOperationResult operationResult = operationResults.GetByReferenceId(entityType.ReferenceId);  
                    if (String.IsNullOrWhiteSpace(entityType.Name))
                    {
                        DataModelHelper.AddOperationResult(operationResult, "112648", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                    else if (internalentityTypeNames.Contains(entityType.Name.ToLowerInvariant()))
                    {
                        DataModelHelper.AddOperationResult(operationResult, "113703", String.Format("'{0}' is an internal entity type. Hence will not be processed further.", entityType.Name), new object[] { entityType.Name }, OperationResultType.Error, TraceEventType.Warning, callerContext);
                    }
                    else
                    {
                        if (shortNames.Contains(entityType.Name))
                        {
                            DataModelHelper.AddOperationResult(operationResult, "110199", String.Empty, new Object[] { entityType.Name }, OperationResultType.Error, TraceEventType.Error, callerContext);
                        }
                        else
                        {
                            shortNames.Add(entityType.Name);
                        }
                    }
                    if (String.IsNullOrWhiteSpace(entityType.LongName))
                    {
                        DataModelHelper.AddOperationResult(operationResult, "113673", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }

                    if (entityType.Action == ObjectAction.Delete)
                    {
                        Collection<Int32> entityTypeIds = new Collection<Int32>();
                        entityTypeIds = PrepareEntityTypeIdsInUse(callerContext);

                        if (entityTypeIds.Contains(entityType.Id))
                        {
                            String entityTypeErrorMessage = String.Format("Failed to delete Entity Type '{0}' as it is used in entity variant definition.", entityType.LongName);
                            DataModelHelper.AddOperationResult(operationResult, "114167", entityTypeErrorMessage, new Object[] { entityType.LongName }, OperationResultType.Error, TraceEventType.Error, callerContext);
                        }
                    }

                }
            }
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callerContext"></param>
        /// <param name="entityTypeProcessOperationResult"></param>
        private void LocalizeErrors(CallerContext callerContext, OperationResult entityTypeProcessOperationResult)
        {
            foreach (Error error in entityTypeProcessOperationResult.Errors)
            {
                if (!String.IsNullOrEmpty(error.ErrorCode))
                {
                    LocaleMessage localeMessage = new LocaleMessageBL().Get(_systemUILocale, error.ErrorCode, false, callerContext);

                    if (localeMessage != null)
                    {
                        error.ErrorMessage = localeMessage.Message;
                    }
                }
            }
        }

        /// <summary>
        /// if there are any error codes coming in OR from db, populate error messages for them
        /// </summary>
        /// <param name="operationResults">DataModelOperationResultCollection</param>
        /// <param name="callerContext">CallerContext</param>
        private void LocalizeErrors(DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            foreach (DataModelOperationResult operationResult in operationResults)
            {
                foreach (Error error in operationResult.Errors)
                {
                    if (!String.IsNullOrEmpty(error.ErrorCode) && String.IsNullOrEmpty(error.ErrorMessage))
                    {
                        _localeMessage = _localeMessageBL.Get(_systemUILocale, error.ErrorCode, false, callerContext);

                        if (_localeMessage != null)
                        {
                            error.ErrorMessage = _localeMessage.Message;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageCode"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private LocaleMessage GetSystemLocaleMessage(String messageCode, CallerContext callerContext)
        {
            LocaleMessageBL localeMessageBL = new LocaleMessageBL();
            return localeMessageBL.Get(_systemUILocale, messageCode, false, callerContext);
        }

        /// <summary>
        /// Log changes to tb_datamodelactivitylog if DA call is sucessfull and appconfig key datamodelactivitylog.enable == true
        /// </summary>
        /// <param name="entityTypeCollection"></param>
        /// <param name="callerContext"></param>
        private void LogDataModelChanges(EntityTypeCollection entityTypeCollection, CallerContext callerContext)
        {
            #region step: populate datamodelactivitylog object

            DataModelActivityLogBL dataModelActivityLogManager = new DataModelActivityLogBL();
            DataModelActivityLogCollection activityLogCollection = dataModelActivityLogManager.FillDataModelActivityLogCollection(entityTypeCollection);

            #endregion step: populate datamodelactivitylog object

            #region step: make api call

            if (activityLogCollection != null) // null activitylog collection means there was error in mapping
            {
                dataModelActivityLogManager.Process(activityLogCollection, callerContext);
            }

            #endregion step: make api call
        }

        #endregion

        #endregion Private Methods

        #endregion
    }
}