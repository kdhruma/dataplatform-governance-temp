using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Transactions;
using MDM.ActivityLogManager.Business;

namespace MDM.DataModelManager.Business
{
    using MDM.Utility;
    using MDM.BusinessObjects;
    using MDM.DataModelManager.Data;
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.BufferManager;
    using MDM.MessageManager.Business;
    using MDM.BusinessObjects.DataModel;
    using MDM.ConfigurationManager.Business;
    using System.Diagnostics;
    using MDM.Core.Exceptions;
    using MDM.BusinessObjects.Interfaces;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.CacheManager.Business;

    /// <summary>
    /// Represents business logic for Container RelationshipType EntityType mapping
    /// </summary>
    public class ContainerRelationshipTypeEntityTypeMappingBL : BusinessLogicBase, IDataModelManager
    {
        #region Fields

        /// <summary>
        /// Field denoting the security principal
        /// </summary>
        private SecurityPrincipal _securityPrincipal;

        /// <summary>
        /// Field denoting locale Message
        /// </summary>
        private LocaleMessage _localeMessage = new LocaleMessage();

        /// <summary>
        /// Field denoting localeMessageBL
        /// </summary>
        private LocaleMessageBL _localeMessageBL = new LocaleMessageBL();

        /// <summary>
        /// Field denoting utility object
        /// </summary>
        private Utility _utility = new Utility();

        /// <summary>
        /// Specifies mapping buffer manager
        /// </summary>
        private MappingBufferManager _mappingBufferManager = new MappingBufferManager();

        /// <summary>
        /// RelationshipTypeEntityTypeMapping data access object
        /// </summary>
        private ContainerRelationshipTypeEntityTypeMappingDA _containerRelationshipTypeEntityTypeMappingDA = new ContainerRelationshipTypeEntityTypeMappingDA();

        /// <summary>
        /// Field denoting the reference of OrganizationBL.
        /// </summary>
        private IOrganizationManager _iOrganizationManager = null;

        /// <summary>
        /// Field denoting the reference of ContainerBL. 
        /// </summary>
        private IContainerManager _iContainerManager = null;

        /// <summary>
        /// Field denoting the reference of EntityTypeBL.
        /// </summary>
        private IEntityTypeManager _iEntityTypeManager = null;

        /// <summary>
        /// Field denoting the reference of RelationshipTypeBL.
        /// </summary>
        private IRelationshipTypeManager _iRelationshipTypeManager = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public ContainerRelationshipTypeEntityTypeMappingBL()
        {
            GetSecurityPrincipal();
        }

        /// <summary>
        /// Initializes a new instance of the ContainerRelationshipTypeEntityTypeMappingBL.
        /// </summary>
        /// <param name="iOrganizationManager">Represents the reference of OrganizationBL.</param>
        /// <param name="iContainerManager">Represents the reference of ContainerBL.</param>
        /// <param name="iEntityTypeManager">Represents the reference of EntityTypeBL.</param>
        /// <param name="iRelationshipTypeManager">Represents the reference of RelationshipTypeBL.</param>
        public ContainerRelationshipTypeEntityTypeMappingBL(IOrganizationManager iOrganizationManager , IContainerManager iContainerManager , IEntityTypeManager iEntityTypeManager , IRelationshipTypeManager iRelationshipTypeManager)
        {
            this._iOrganizationManager = iOrganizationManager;
            this._iContainerManager = iContainerManager;
            this._iEntityTypeManager = iEntityTypeManager;
            this._iRelationshipTypeManager = iRelationshipTypeManager;

            GetSecurityPrincipal();
        }

        #endregion

        #region Properties
        #endregion

        #region Method

        #region Public Methods

        #region CUD Methods

        /// <summary>
        /// Add new ContainerRelationshipTypeEntityTypeMapping
        /// </summary>
        /// <param name="containerRelationshipTypeEntityTypeMapping"></param>
        /// <returns></returns>
        public Boolean Create(ContainerRelationshipTypeEntityTypeMapping containerRelationshipTypeEntityTypeMapping)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update existing ContainerRelationshipTypeEntityTypeMapping
        /// </summary>
        /// <param name="containerRelationshipTypeEntityTypeMapping"></param>
        /// <returns></returns>
        public Boolean Update(ContainerRelationshipTypeEntityTypeMapping containerRelationshipTypeEntityTypeMapping)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Remove existing ContainerRelationshipTypeEntityTypeMapping
        /// </summary>
        /// <param name="containerRelationshipTypeEntityTypeMapping"></param>
        /// <returns></returns>
        public Boolean Delete(ContainerRelationshipTypeEntityTypeMapping containerRelationshipTypeEntityTypeMapping)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create, Update or Delete Container RelationshipType EntityType Mappings
        /// </summary>
        /// <param name="ContainerRelationshipTypeEntityTypeMappingCollection">Container RelationshipType EntityType Mappings to process</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public OperationResult Process(ContainerRelationshipTypeEntityTypeMappingCollection containerRelationshipTypeEntityTypeMapping, CallerContext callerContext)
        {
            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("ContainerRelationshipTypeEntityTypeMappingBL.Process", MDMTraceSource.DataModel, false);

            OperationResult operationResult = new OperationResult();
            operationResult.OperationResultStatus = OperationResultStatusEnum.Failed;

            ContainerRelationshipTypeEntityTypeMappingCollection originalMapings = null;

            try
            {

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    PopulateProgramName(callerContext);
                    _containerRelationshipTypeEntityTypeMappingDA.Process(containerRelationshipTypeEntityTypeMapping, _securityPrincipal.CurrentUserName, callerContext.ProgramName);

                    transactionScope.Complete();
                    operationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                }

                #region activity log

                if (operationResult.OperationResultStatus == OperationResultStatusEnum.Successful && AppConfigurationHelper.GetAppConfig<Boolean>(Constants.ACTIVITYLOG_DATAMODEL_ENABLED_APPCONFIG_KEY, false))
                {
                    originalMapings = GetAll(callerContext);
                    LogDataModelChanges(containerRelationshipTypeEntityTypeMapping, callerContext, originalMapings);
                }

                #endregion activity log

                _mappingBufferManager.RemoveContainerRelationshipTypeEntityTypeMapping();
                _mappingBufferManager.NotifyModelDataChange(CacheKeyContants.RELATIONSHIP_HIERARCHY_CHANGED_KEY);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerRelationshipTypeEntityTypeMappingBL.Process", MDMTraceSource.DataModel);
            }

            return operationResult;
        }

        #endregion

        #region Get Methods

        /// <summary>
        /// Gets all container RelationshipType EntityType mappings from the system
        /// </summary>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>All container RelationshipType EntityType mappings</returns>
        public ContainerRelationshipTypeEntityTypeMappingCollection GetAll(CallerContext callerContext)
        {
            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("ContainerRelationshipTypeEntityTypeMappingBL.GetAll", MDMTraceSource.DataModel, false);

            ContainerRelationshipTypeEntityTypeMappingCollection containerRelationshipTypeEntityTypeMappings = new ContainerRelationshipTypeEntityTypeMappingCollection();

            try
            {
                #region Get from cache

                MappingBufferManager bufferManager = new MappingBufferManager();
                containerRelationshipTypeEntityTypeMappings = bufferManager.FindContainerRelationshipTypeEntityTypeMapping();

                #endregion Get from cache

                #region Get from DB

                if (containerRelationshipTypeEntityTypeMappings == null || containerRelationshipTypeEntityTypeMappings.Count < 1)
                {
                    containerRelationshipTypeEntityTypeMappings = _containerRelationshipTypeEntityTypeMappingDA.GetMappings(-1, -1, -1);

                    if (containerRelationshipTypeEntityTypeMappings != null && containerRelationshipTypeEntityTypeMappings.Count > 0)
                    {
                        bufferManager.UpdateContainerRelationshipTypeEntityTypeMapping(containerRelationshipTypeEntityTypeMappings);
                    }
                }

                #endregion Get from DB

            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerRelationshipTypeEntityTypeMappingBL.GetAll", MDMTraceSource.DataModel);
            }
            return containerRelationshipTypeEntityTypeMappings;
        }

        /// <summary>
        /// Get Container RelationshipType EntityType Mapping by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ContainerRelationshipTypeEntityTypeMapping GetById(Int32 id)
        {
            throw new NotImplementedException();
        }

        // TODO:: Remove Mappings from the method name

        /// <summary>
        /// Gets container RelationshipType EntityType mappings based on containerId and relationshipId
        /// </summary>
        /// <param name="containerId">Indicates the container Id for which mappings needs to be fetched</param>
        /// <param name="relationshipTypeId">Indicates the relationship type Id for which mappings needs to be fetched</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Container RelationshipType EntityType mappings  for specified containerId and RelationshipTypeId</returns>
        public ContainerRelationshipTypeEntityTypeMappingCollection Get(Int32 containerId, Int32 relationshipTypeId, CallerContext callerContext)
        {
            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("ContainerRelationshipTypeEntityTypeMappingBL.Get", MDMTraceSource.DataModel, false);

            ContainerRelationshipTypeEntityTypeMappingCollection allContainerRelationshipTypeEntityTypeMappings = null;
            ContainerRelationshipTypeEntityTypeMappingCollection requestedMapping = new ContainerRelationshipTypeEntityTypeMappingCollection();

            try
            {
                allContainerRelationshipTypeEntityTypeMappings = GetAll(callerContext);
                if (allContainerRelationshipTypeEntityTypeMappings != null && allContainerRelationshipTypeEntityTypeMappings.Count > 0)
                {
                    foreach (ContainerRelationshipTypeEntityTypeMapping mapping in allContainerRelationshipTypeEntityTypeMappings)
                    {
                        if (mapping.ContainerId == containerId && mapping.RelationshipTypeId == relationshipTypeId)
                        {
                            requestedMapping.Add(mapping);
                        }
                    }
                }

            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerRelationshipTypeEntityTypeMappingBL.Get", MDMTraceSource.DataModel);
            }
            return requestedMapping;
        }

        /// <summary>
        /// Gets container RelationshipType EntityType mappings based on containerId and entityTypeId
        /// </summary>
        /// <param name="containerId">Indicates the container Id for which mappings needs to be fetched</param>
        /// <param name="entityTypeId">Indicates the entity type Id for which mappings needs to be fetched</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Container RelationshipType EntityType mappings  for specified containerId and entityTypeId</returns>
        public ContainerRelationshipTypeEntityTypeMappingCollection GetMappingsByContext(EntityModelContext entityModelContext, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("ContainerRelationshipTypeEntityTypeMappingBL.GetMappingsByContext", MDMTraceSource.DataModel, false);
            }
           
            ContainerRelationshipTypeEntityTypeMappingCollection requestedMapping = new ContainerRelationshipTypeEntityTypeMappingCollection();

            try
            {
                ContainerRelationshipTypeEntityTypeMappingCollection allContainerRelationshipTypeEntityTypeMappings = GetAll(callerContext);
               
                if (allContainerRelationshipTypeEntityTypeMappings != null && allContainerRelationshipTypeEntityTypeMappings.Count > 0)
                {
                    Int32 containerId = entityModelContext.ContainerId; ;
                    Int32 entityTypeId = entityModelContext.EntityTypeId;
                    Collection<Int32> relationshipTypeIds = entityModelContext.RelationshipTypeIds;
                    
                    //TODO: All existing related APIs need to made obsolete and all referenced page to be wired to use this
                
                    foreach (ContainerRelationshipTypeEntityTypeMapping mapping in allContainerRelationshipTypeEntityTypeMappings)
                    {
                        Boolean isValidContext = true;

                        if (containerId > 0 && mapping.ContainerId != containerId)
                        {
                            isValidContext = false;
                        }
                        if (entityTypeId > 0 && mapping.EntityTypeId != entityTypeId)
                        {
                            isValidContext = false;
                        }
                        if (relationshipTypeIds != null && relationshipTypeIds.Count > 0 && !relationshipTypeIds.Contains(mapping.RelationshipTypeId))
                        {
                            isValidContext = false;
                        }

                        if (isValidContext)
                        {
                            requestedMapping.Add(mapping);
                        }
                    }
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("ContainerRelationshipTypeEntityTypeMappingBL.GetMappingsByContext", MDMTraceSource.DataModel);
                }
            }
            return requestedMapping;
        }

        /// <summary>
        /// Gets mapped EntityTypes based on ContainerId and RelationshipTypeId
        /// </summary>
        /// <param name="containerId">Indicates the container Id for which mapped EntityTypes needs to be fetched</param>
        /// <param name="relationshipTypeId">Indicates the relationshipType Id for which mapped EntityTypes needs to be fetched</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Mapped EntityTypes for specified containerId and relationshipTypeId</returns>
        public EntityTypeCollection GetMappedEntityTypes(Int32 containerId, Int32 relationshipTypeId, CallerContext callerContext)
        {
            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("ContainerRelationshipTypeEntityTypeMappingBL.GetMappedEntityTypes", MDMTraceSource.DataModel, false);

            ContainerRelationshipTypeEntityTypeMappingCollection containerRelationshipTypeEntityTypeMappings = new ContainerRelationshipTypeEntityTypeMappingCollection();
            EntityTypeCollection entityTypes = new EntityTypeCollection();
            Collection<Int32> entityTypeIds = new Collection<Int32>();
            EntityTypeBL entityTypeBL = new EntityTypeBL();

            try
            {
                containerRelationshipTypeEntityTypeMappings = _containerRelationshipTypeEntityTypeMappingDA.GetMappings(containerId, relationshipTypeId, -1);
                if (containerRelationshipTypeEntityTypeMappings.Count > 0)
                {
                    foreach (ContainerRelationshipTypeEntityTypeMapping mapping in containerRelationshipTypeEntityTypeMappings)
                    {
                        if (mapping.EntityTypeId > 0)
                        {
                            entityTypeIds.Add(mapping.EntityTypeId);
                        }
                    }

                    entityTypes = entityTypeBL.GetEntityTypesByIds(entityTypeIds);
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerRelationshipTypeEntityTypeMappingBL.GetMappedEntityTypes", MDMTraceSource.DataModel);
            }

            return entityTypes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cnodeGroupIds"></param>
        /// <param name="catalogId"></param>
        /// <returns></returns>
        public Collection<ContainerRelationshipTypeEntityTypeMapping> GetMappingsByCnodes(String user, String cnodeGroupIds, Int32 catalogId)
        {
            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("ContainerRelationshipTypeEntityTypeMappingBL.GetMappingsByCnodes", MDMTraceSource.DataModel, false);

            Collection<ContainerRelationshipTypeEntityTypeMapping> containerRelationshipTypeEntityTypeMappings = new Collection<ContainerRelationshipTypeEntityTypeMapping>();
            try
            {
                containerRelationshipTypeEntityTypeMappings = _containerRelationshipTypeEntityTypeMappingDA.GetMappingsByCnodes(user, cnodeGroupIds, catalogId);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerRelationshipTypeEntityTypeMappingBL.GetMappingsByCnodes", MDMTraceSource.DataModel);
            }
            return containerRelationshipTypeEntityTypeMappings;
        }

        #endregion

        #region IDataModelManager Methods

        /// <summary>
        /// Generate and OperationResult Schema for given DataModelObjectCollection
        /// </summary>
        /// <param name="iDataModelObjects"></param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        public void PrepareOperationResultsSchema(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults)
        {            
            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("ContainerRelationshipTypeEntityTypeMappingBL.PrepareOperationResultsSchema", MDMTraceSource.DataModel, false);

                String separator = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.RSDataModelExcel.Separator", "//").Trim();
                ContainerRelationshipTypeEntityTypeMappingCollection containerRelationshipTypeEntityTypeMappings = iDataModelObjects as ContainerRelationshipTypeEntityTypeMappingCollection;

                if (containerRelationshipTypeEntityTypeMappings != null && containerRelationshipTypeEntityTypeMappings.Count > 0)
                {
                    if (operationResults.Count > 0)
                    {
                        operationResults.Clear();
                    }

                    Int32 containerRelationshipTypeEntityTypeMappingIdToBeCreated = -1;

                    foreach (ContainerRelationshipTypeEntityTypeMapping containerRelationshipTypeEntityTypeMapping in containerRelationshipTypeEntityTypeMappings)
                    {
                        DataModelOperationResult containerRelationshipTypeEntityTypeMappingOperationResult = new DataModelOperationResult(containerRelationshipTypeEntityTypeMapping.Id, containerRelationshipTypeEntityTypeMapping.LongName, containerRelationshipTypeEntityTypeMapping.ExternalId, containerRelationshipTypeEntityTypeMapping.ReferenceId);

                        if (String.IsNullOrEmpty(containerRelationshipTypeEntityTypeMappingOperationResult.ExternalId))
                        {
                            containerRelationshipTypeEntityTypeMappingOperationResult.ExternalId = String.Format("{1} {0} {2} {0} {3} {0} {4}", separator, containerRelationshipTypeEntityTypeMapping.OrganizationName, containerRelationshipTypeEntityTypeMapping.ContainerName, containerRelationshipTypeEntityTypeMapping.RelationshipTypeName, containerRelationshipTypeEntityTypeMapping.EntityTypeName);
                        }

                        if (containerRelationshipTypeEntityTypeMapping.Id < 1)
                        {
                            containerRelationshipTypeEntityTypeMapping.Id = containerRelationshipTypeEntityTypeMappingIdToBeCreated;
                            containerRelationshipTypeEntityTypeMappingOperationResult.Id = containerRelationshipTypeEntityTypeMappingIdToBeCreated;
                            containerRelationshipTypeEntityTypeMappingIdToBeCreated--;
                        }

                        operationResults.Add(containerRelationshipTypeEntityTypeMappingOperationResult);
                    }
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerRelationshipTypeEntityTypeMappingBL.PrepareOperationResultsSchema", MDMTraceSource.DataModel);
            }
        }

        /// <summary>
        /// Validates DataModelObjects and populate OperationResults
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be filled up.</param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        /// <returns>DataModel Operation Result Collection</returns>
        public void Validate(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            try
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StartTraceActivity("ContainerRelationshipTypeEntityTypeMappingBL.Validate", MDMTraceSource.DataModel, false);
                }

                ContainerRelationshipTypeEntityTypeMappingCollection containerRelationshipTypeEntityTypeMappings = iDataModelObjects as ContainerRelationshipTypeEntityTypeMappingCollection;
                CallerContext callerContext = iCallerContext as CallerContext;

                #region Parameter Validation

                ValidateInputParameters(containerRelationshipTypeEntityTypeMappings, operationResults, callerContext);

                #endregion

                #region Parameter Validation

                #region Data Validation

                ValidateData(containerRelationshipTypeEntityTypeMappings, operationResults, callerContext);

                #endregion Data Validation

                #endregion
            }
            catch (Exception ex)
            {
                DataModelHelper.AddOperationResults(operationResults, String.Empty, ex.Message, null, OperationResultType.Error, TraceEventType.Error, iCallerContext as CallerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("ContainerRelationshipTypeEntityTypeMappingBL.Validate", MDMTraceSource.DataModel);
                }
            }
        }

        /// <summary>
        /// Load original data model objects
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to load original object.</param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void LoadOriginal(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("ContainerRelationshipTypeEntityTypeMappingBL.LoadOriginal", MDMTraceSource.DataModel, false);

                LoadOriginalContainerRelationshipTypeEntityTypeMapping(iDataModelObjects as ContainerRelationshipTypeEntityTypeMappingCollection, iCallerContext as CallerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerRelationshipTypeEntityTypeMappingBL.LoadOriginal", MDMTraceSource.DataModel);
            }

        }

        /// <summary>
        /// Fills missing information to data model objects.
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be filled up.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void FillDataModel(IDataModelObjectCollection iDataModelObjects, ICallerContext iCallerContext)
        {
            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("ContainerRelationshipTypeEntityTypeMappingBL.FillDataModel", MDMTraceSource.DataModel, false);

                FillContainerRelationshipTypeEntityTypeMapping(iDataModelObjects as ContainerRelationshipTypeEntityTypeMappingCollection, iCallerContext as CallerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerRelationshipTypeEntityTypeMappingBL.FillDataModel", MDMTraceSource.DataModel);
            }
        }
        
        /// <summary>
        /// Compare and merge data model object collection with current collection
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be compare and merge.</param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>Returns merged data model objects.</returns>
        public void CompareAndMerge(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {            
            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("ContainerRelationshipTypeEntityTypeMappingBL.CompareAndMerge", MDMTraceSource.DataModel, false);

                CompareAndMerge(iDataModelObjects as ContainerRelationshipTypeEntityTypeMappingCollection, operationResults, iCallerContext as CallerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerRelationshipTypeEntityTypeMappingBL.CompareAndMerge", MDMTraceSource.DataModel);
            }
        }

        /// <summary>
        /// Process data model collection
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be processed.</param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>Result of operation</returns>
        public void Process(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("ContainerRelationshipTypeEntityTypeMappingBL.Process", MDMTraceSource.DataModel, false);

                ContainerRelationshipTypeEntityTypeMappingCollection containerRelationshipTypeEntityTypeMappings = iDataModelObjects as ContainerRelationshipTypeEntityTypeMappingCollection;
                CallerContext callerContext = iCallerContext as CallerContext;

                if (containerRelationshipTypeEntityTypeMappings.Count > 0)
                {
                    PopulateProgramName(callerContext);

                    DBCommandProperties command = DBCommandHelper.Get((CallerContext)iCallerContext, MDMCenterModuleAction.Execute);

                    #region Perform ContainerRelationshipTypeEntityTypeMapping updates in database

                    using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                    {
                        _containerRelationshipTypeEntityTypeMappingDA.Process(containerRelationshipTypeEntityTypeMappings, operationResults, _securityPrincipal.CurrentUserName, callerContext.ProgramName);

                        transactionScope.Complete();
                    }

                    LocalizeErrors(callerContext, operationResults);

                    #endregion

                    #region activity log

                    if (operationResults.OperationResultStatus == OperationResultStatusEnum.Successful && AppConfigurationHelper.GetAppConfig<Boolean>(Constants.ACTIVITYLOG_DATAMODEL_ENABLED_APPCONFIG_KEY, false))
                    {
                        LogDataModelChanges(containerRelationshipTypeEntityTypeMappings, callerContext);
                    }

                    #endregion activity log

                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerRelationshipTypeEntityTypeMappingBL.Process", MDMTraceSource.DataModel);
            }
        }

        ///<summary>
        /// Removes existing ContainerRelationshipTypeEntityTypeMappings from cache
        /// </summary>
        /// <param name="iDataModelObjects">Represents a collection of data model objects to be process entity cache load</param>
        /// <param name="operationResults">Represents a collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Represents a context of caller making call to this API</param>
        public void InvalidateCache(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            DiagnosticActivity activity = new DiagnosticActivity();
            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            try
            {
                if (traceSettings.IsBasicTracingEnabled)
                {
                    activity.Start();
                }

                // Clear all existing ContainerRelationshipTypeEntityTypeMappings from cache
                _mappingBufferManager.RemoveContainerRelationshipTypeEntityTypeMapping();
                _mappingBufferManager.NotifyModelDataChange(CacheKeyContants.RELATIONSHIP_HIERARCHY_CHANGED_KEY);
            }
            catch (Exception ex)
            {
                DataModelHelper.AddOperationResults(operationResults, String.Empty, ex.Message, null, OperationResultType.Error, TraceEventType.Error, iCallerContext as CallerContext);

                if (traceSettings.IsBasicTracingEnabled)
                {
                    activity.LogError(ex.Message);
                }
            }
            finally
            {
                if (traceSettings.IsBasicTracingEnabled)
                {
                    activity.Stop();
                }
            }
        }
        
        #endregion

        #endregion

        #region Private Methods

        #region Validation Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerRelationshipTypeEntityTypeMappings"></param>
        /// <param name="callerContext"></param>
        private void ValidateInputParameters(ContainerRelationshipTypeEntityTypeMappingCollection containerRelationshipTypeEntityTypeMappings, CallerContext callerContext)
        {
            String errorMessage;

            if (containerRelationshipTypeEntityTypeMappings == null || containerRelationshipTypeEntityTypeMappings.Count < 1)
            {
                errorMessage = "ContainerRelationshipTypeEntityTypeMapping Collection cannot be null.";
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, errorMessage, MDMTraceSource.DataModel);
                throw new MDMOperationException("", errorMessage, "DataModelManager.ContainerRelationshipTypeEntityTypeMappingBL", String.Empty, "Create");
            }

            if (callerContext == null)
            {
                errorMessage = "CallerContext cannot be null.";
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "CallerContext cannot be null.", MDMTraceSource.DataModel);
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "DataModelManager.ContainerRelationshipTypeEntityTypeMappingBL", String.Empty, "Create");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerRelationshipTypeEntityTypeMappings"></param>
        /// <param name="operationResults"></param>
        /// <param name="callerContext"></param>
        private void ValidateInputParameters(ContainerRelationshipTypeEntityTypeMappingCollection containerRelationshipTypeEntityTypeMappings, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            if (containerRelationshipTypeEntityTypeMappings == null || containerRelationshipTypeEntityTypeMappings.Count < 1)
            {
                DataModelHelper.AddOperationResults(operationResults, "113631", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }

            if (callerContext == null)
            {
                DataModelHelper.AddOperationResults(operationResults, "111846", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
        }

        /// <summary>
        /// Validate the data of specified parameters in container relationship type entity type mapping collection
        /// </summary>
        /// <param name="containerRelationshipTypeEntityTypeMappings">Indicates the collection of container relationship type entity type mapping</param>
        /// <param name="operationResults">Indicates the result of the operation</param>
        /// <param name="callerContext">Indicates the context of the caller</param>
        private void ValidateData(ContainerRelationshipTypeEntityTypeMappingCollection containerRelationshipTypeEntityTypeMappings, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            if (containerRelationshipTypeEntityTypeMappings != null && containerRelationshipTypeEntityTypeMappings.Count > 0)
            {
                Collection<String> containerRelationshipTypeEntityTypeMappingList = new Collection<String>();

                foreach (ContainerRelationshipTypeEntityTypeMapping containerRelationshipTypeEntityTypeMapping in containerRelationshipTypeEntityTypeMappings)
                {
                    Collection<String> uniqueIdentifierList = new Collection<String>(){ containerRelationshipTypeEntityTypeMapping.ContainerName, 
                                                                                            containerRelationshipTypeEntityTypeMapping.RelationshipTypeName, 
                                                                                            containerRelationshipTypeEntityTypeMapping.EntityTypeName};

                    String rowUniqueIdentifier = DataModelHelper.GetUniqueIdentifier(uniqueIdentifierList);

                    IDataModelOperationResult dataModelOperationResult = operationResults.GetByReferenceId(containerRelationshipTypeEntityTypeMapping.ReferenceId);

                    DataModelHelper.ValidateOrganizationName(containerRelationshipTypeEntityTypeMapping.OrganizationName,
                                                          dataModelOperationResult, callerContext);

                    DataModelHelper.ValidateContainerName(containerRelationshipTypeEntityTypeMapping.ContainerName, 
                                                            dataModelOperationResult, callerContext);

                    DataModelHelper.ValidateRelationshipTypeName(containerRelationshipTypeEntityTypeMapping.RelationshipTypeName, 
                                                                    dataModelOperationResult, callerContext);

                    DataModelHelper.ValidateEntityTypeName(containerRelationshipTypeEntityTypeMapping.EntityTypeName, 
                                                            dataModelOperationResult, callerContext);

                    if (containerRelationshipTypeEntityTypeMappingList.Contains(rowUniqueIdentifier))
                    {
                        Object[] parameters = null;
                        parameters = new Object[] { containerRelationshipTypeEntityTypeMapping.OrganizationName, 
                                                        containerRelationshipTypeEntityTypeMapping.ContainerName, 
                                                        containerRelationshipTypeEntityTypeMapping.EntityTypeName, 
                                                        containerRelationshipTypeEntityTypeMapping.RelationshipTypeName };

                        String errorMessage = String.Format("Duplicate mappings found for the container relationship type entity type mapping with organization: {0}, container: {1}, entity type: {2}, and relationship type: {3}", parameters);

                        DataModelHelper.AddOperationResult(dataModelOperationResult, "113964", errorMessage, parameters,
                                                            OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                    else
                    {
                        containerRelationshipTypeEntityTypeMappingList.Add(rowUniqueIdentifier);
                    }
                }
            }
        }

        #endregion

        #region Get Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ContainerRelationshipTypeEntityTypeMappings"></param>
        /// <param name="callerContext"></param>
        private void LoadOriginalContainerRelationshipTypeEntityTypeMapping(ContainerRelationshipTypeEntityTypeMappingCollection ContainerRelationshipTypeEntityTypeMappings, CallerContext callerContext)
        {
            ContainerRelationshipTypeEntityTypeMappingCollection orginalContainerRelationshipTypeEntityTypeMappingCollection = GetAll(callerContext);

            if (orginalContainerRelationshipTypeEntityTypeMappingCollection != null && orginalContainerRelationshipTypeEntityTypeMappingCollection.Count > 0)
            {
                foreach (ContainerRelationshipTypeEntityTypeMapping ContainerRelationshipTypeEntityTypeMapping in ContainerRelationshipTypeEntityTypeMappings)
                {
                    ContainerRelationshipTypeEntityTypeMapping.OriginalContainerRelationshipTypeEntityTypeMapping = orginalContainerRelationshipTypeEntityTypeMappingCollection.Get(ContainerRelationshipTypeEntityTypeMapping.OrganizationName, ContainerRelationshipTypeEntityTypeMapping.ContainerName, ContainerRelationshipTypeEntityTypeMapping.EntityTypeName, ContainerRelationshipTypeEntityTypeMapping.RelationshipTypeName);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ContainerRelationshipTypeEntityTypeMappingCollection"></param>
        /// <param name="callerContext"></param>
        private void FillContainerRelationshipTypeEntityTypeMapping(ContainerRelationshipTypeEntityTypeMappingCollection containerRelationshipTypeEntityTypeMappings, CallerContext callerContext)
        {
            OrganizationCollection organizations = new OrganizationCollection();
            ContainerCollection containers = new ContainerCollection();
            EntityTypeCollection entityTypes = new EntityTypeCollection();
            RelationshipTypeCollection relationshipTypes = new RelationshipTypeCollection();

            foreach (ContainerRelationshipTypeEntityTypeMapping containerRelationshipTypeEntityTypeMapping in containerRelationshipTypeEntityTypeMappings)
            {
                if (containerRelationshipTypeEntityTypeMapping.OriginalContainerRelationshipTypeEntityTypeMapping != null)
                {
                    containerRelationshipTypeEntityTypeMapping.Id = containerRelationshipTypeEntityTypeMapping.OriginalContainerRelationshipTypeEntityTypeMapping.Id;
                    containerRelationshipTypeEntityTypeMapping.OrganizationId = containerRelationshipTypeEntityTypeMapping.OriginalContainerRelationshipTypeEntityTypeMapping.OrganizationId;
                    containerRelationshipTypeEntityTypeMapping.ContainerId = containerRelationshipTypeEntityTypeMapping.OriginalContainerRelationshipTypeEntityTypeMapping.ContainerId;
                    containerRelationshipTypeEntityTypeMapping.EntityTypeId = containerRelationshipTypeEntityTypeMapping.OriginalContainerRelationshipTypeEntityTypeMapping.EntityTypeId;
                    containerRelationshipTypeEntityTypeMapping.RelationshipTypeId = containerRelationshipTypeEntityTypeMapping.OriginalContainerRelationshipTypeEntityTypeMapping.RelationshipTypeId;
                }
                else
                {
                    containerRelationshipTypeEntityTypeMapping.OrganizationId = DataModelHelper.GetOrganizationId(_iOrganizationManager, containerRelationshipTypeEntityTypeMapping.OrganizationName, ref organizations, callerContext);
                    containerRelationshipTypeEntityTypeMapping.ContainerId = DataModelHelper.GetContainerId(_iContainerManager, containerRelationshipTypeEntityTypeMapping.ContainerName, containerRelationshipTypeEntityTypeMapping.OrganizationName, ref containers, callerContext);
                    containerRelationshipTypeEntityTypeMapping.EntityTypeId = DataModelHelper.GetEntityTypeId(_iEntityTypeManager, containerRelationshipTypeEntityTypeMapping.EntityTypeName, ref entityTypes, callerContext);
                    containerRelationshipTypeEntityTypeMapping.RelationshipTypeId = DataModelHelper.GetRelationshipTypeId(_iRelationshipTypeManager, containerRelationshipTypeEntityTypeMapping.RelationshipTypeName, ref relationshipTypes, callerContext);
                }
            }
        }

        #endregion

        #region Process Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ContainerRelationshipTypeEntityTypeMappings"></param>
        /// <param name="callerContext"></param>
        private void CompareAndMerge(ContainerRelationshipTypeEntityTypeMappingCollection ContainerRelationshipTypeEntityTypeMappings, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            foreach (ContainerRelationshipTypeEntityTypeMapping deltaContainerRelationshipTypeEntityTypeMapping in ContainerRelationshipTypeEntityTypeMappings)
            {
                IDataModelOperationResult operationResult = operationResults.GetByReferenceId(deltaContainerRelationshipTypeEntityTypeMapping.ReferenceId);

                //If delta object Action is Read/Ignore then skip Merge Delta Process.
                if (deltaContainerRelationshipTypeEntityTypeMapping.Action == ObjectAction.Read || deltaContainerRelationshipTypeEntityTypeMapping.Action == ObjectAction.Ignore)
                    continue;

                IContainerRelationshipTypeEntityTypeMapping originalContainerRelationshipTypeEntityTypeMapping = deltaContainerRelationshipTypeEntityTypeMapping.OriginalContainerRelationshipTypeEntityTypeMapping;

                if (originalContainerRelationshipTypeEntityTypeMapping != null)
                {
                    //If delta object Action is Delete then skip Merge Delta process.
                    if (deltaContainerRelationshipTypeEntityTypeMapping.Action != ObjectAction.Delete)
                    {
                        originalContainerRelationshipTypeEntityTypeMapping.MergeDelta(deltaContainerRelationshipTypeEntityTypeMapping, callerContext, false);
                    }
                }
                else
                {
                    if (deltaContainerRelationshipTypeEntityTypeMapping.Action == ObjectAction.Delete)
                    {
                        Object[] parameters = new Object[] { deltaContainerRelationshipTypeEntityTypeMapping.OrganizationName, deltaContainerRelationshipTypeEntityTypeMapping.ContainerName, deltaContainerRelationshipTypeEntityTypeMapping.EntityTypeName, deltaContainerRelationshipTypeEntityTypeMapping.RelationshipTypeName};
                        DataModelHelper.AddOperationResult(operationResult, "113629", String.Empty, parameters, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                    else
                    {
                        List<String> parameters = new List<String>();
                        List<String> errorMessages = new List<String>();

                        if (deltaContainerRelationshipTypeEntityTypeMapping.OrganizationId < 1)
                        {
                            parameters.Add(deltaContainerRelationshipTypeEntityTypeMapping.OrganizationName);
                            errorMessages.Add(DataModelHelper.GetLocaleMessageText("100754", String.Empty, null, callerContext)); // 100754 - Organization
                        }
                        if (deltaContainerRelationshipTypeEntityTypeMapping.ContainerId < 1)
                        {
                            parameters.Add(deltaContainerRelationshipTypeEntityTypeMapping.ContainerName);
                            errorMessages.Add(DataModelHelper.GetLocaleMessageText("113958", String.Empty, null, callerContext)); // 113958 - Container Name
                        }
                        if (deltaContainerRelationshipTypeEntityTypeMapping.EntityTypeId < 1)
                        {
                            parameters.Add(deltaContainerRelationshipTypeEntityTypeMapping.EntityTypeName);
                            errorMessages.Add(DataModelHelper.GetLocaleMessageText("100415", String.Empty, null, callerContext)); // 100415 - Entity Type
                        }
                        if (deltaContainerRelationshipTypeEntityTypeMapping.RelationshipTypeId < 1)
                        {
                            parameters.Add(deltaContainerRelationshipTypeEntityTypeMapping.RelationshipTypeName);
                            errorMessages.Add(DataModelHelper.GetLocaleMessageText("112801", String.Empty, null, callerContext)); // 112801 - Relationship Type Name
                        }

                        if (errorMessages.Count > 0)
                        {
                            DataModelHelper.AddInvalidNamesErrorsToOperationResult(operationResult, errorMessages, parameters, callerContext);
                        }

                        deltaContainerRelationshipTypeEntityTypeMapping.Action = ObjectAction.Create;
                    }
                }
            }
        }

        #endregion

        #region Misc. Methods

        /// <summary>
        /// if there are any error codes coming in OR from db, populate error messages for them
        /// </summary>
        /// <param name="callerContext">CallerContext</param>
        /// <param name="operationResults">DataModelOperationResultCollection</param>
        private void LocalizeErrors(CallerContext callerContext, DataModelOperationResultCollection operationResults)
        {
            foreach (DataModelOperationResult operationResult in operationResults)
            {
                foreach (Error error in operationResult.Errors)
                {
                    if (!String.IsNullOrEmpty(error.ErrorCode) && String.IsNullOrEmpty(error.ErrorMessage))
                    {
                        _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), error.ErrorCode, false, callerContext);

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
        /// <param name="callerContext"></param>
        private void PopulateProgramName(CallerContext callerContext)
        {
            if (String.IsNullOrWhiteSpace(callerContext.ProgramName))
        {
                callerContext.ProgramName = "ContainerRelationshipTypeEntityTypeMappingBL";
            }
        }

        /// <summary>
        /// Log changes to tb_datamodelactivitylog if DA call is sucessfull and appconfig key datamodelactivitylog.enable == true
        /// </summary>
        /// <param name="conRTETMappings"></param>
        /// <param name="callerContext"></param>
        /// <param name="originalMapings"></param>
        private void LogDataModelChanges(ContainerRelationshipTypeEntityTypeMappingCollection conRTETMappings, CallerContext callerContext, ContainerRelationshipTypeEntityTypeMappingCollection originalMapings = null)
        {
            #region Step: Populate datamodelactivitylog object

            DataModelActivityLogBL dataModelActivityLogManager = new DataModelActivityLogBL();
            DataModelActivityLogCollection activityLogCollection = dataModelActivityLogManager.FillDataModelActivityLogCollection(conRTETMappings, conRTETMappings.DataModelObjectType, callerContext, originalMapings);

            #endregion Step: Populate datamodelactivitylog object

            #region Step: Make api call

            if (activityLogCollection != null && activityLogCollection.Count > 0) // null activitylog collection means there was error in mapping
            {
                dataModelActivityLogManager.Process(activityLogCollection, callerContext);
            }

            #endregion Step: Make api call
        }

        #endregion

        #endregion

        #endregion
    }
}