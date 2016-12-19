using System;
using System.Transactions;
using System.Collections.ObjectModel;
using MDM.ActivityLogManager.Business;

namespace MDM.DataModelManager.Business
{
    using MDM.Utility;
    using MDM.BusinessObjects;
    using MDM.DataModelManager.Data;
    using MDM.Core;
    using MDM.MessageManager.Business;
    using MDM.BufferManager;
    using MDM.Interfaces;
    using MDM.BusinessObjects.DataModel;
    using MDM.ConfigurationManager.Business;
    using System.Diagnostics;
    using MDM.Core.Exceptions;
    using MDM.CacheManager.Business;

    /// <summary>
    /// Represents business logic for RelationshipType EntityType mapping
    /// </summary>
    public class RelationshipTypeEntityTypeMappingBL : BusinessLogicBase, IDataModelManager
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
        private RelationshipTypeEntityTypeMappingDA _relationshipTypeEntityTypeMappingDA = new RelationshipTypeEntityTypeMappingDA();

        /// <summary>
        /// Field denoting the reference of EntityTypeBL.
        /// </summary>
        private IEntityTypeManager _iEntityTypeManager = null;

        /// <summary>
        /// Field denoting the RelationshipTypeBL.
        /// </summary>
        private IRelationshipTypeManager _iRelationshipTypeManager = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public RelationshipTypeEntityTypeMappingBL()
        {
            GetSecurityPrincipal();
        }

        /// <summary>
        /// Initializes a new instance of the RelationshipTypeEntityTypeMappingBL.
        /// </summary>
        /// <param name="iEntityTypeManager">Represents the reference of EntityTypeBL.</param>
        /// <param name="iRelationshipTypeManager">Represents the RelationshipTypeBL.</param>
        public RelationshipTypeEntityTypeMappingBL(IEntityTypeManager iEntityTypeManager, IRelationshipTypeManager iRelationshipTypeManager)
        {
            this._iEntityTypeManager = iEntityTypeManager;
            this._iRelationshipTypeManager = iRelationshipTypeManager;

            GetSecurityPrincipal();
        }

        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        #region CUD Methods

        /// <summary>
        /// Add new RelationshipTypeEntityTypeMapping
        /// </summary>
        /// <param name="containerEntityTypeMapping"></param>
        /// <returns></returns>
        public Boolean Create(RelationshipTypeEntityTypeMapping relationshipTypeEntityTypeMapping)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update existing RelationshipTypeEntityTypeMapping
        /// </summary>
        /// <param name="containerEntityTypeMapping"></param>
        /// <returns></returns>
        public Boolean Update(RelationshipTypeEntityTypeMapping relationshipTypeEntityTypeMapping)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Remove existing RelationshipTypeEntityTypeMapping
        /// </summary>
        /// <param name="containerEntityTypeMapping"></param>
        /// <returns></returns>
        public Boolean Delete(RelationshipTypeEntityTypeMapping relationshipTypeEntityTypeMapping)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create, Update or Delete RelationshipType EntityType mappings
        /// </summary>
        /// <param name="relationshipTypeEntityTypeMappings">RelationshipType EntityType mappings to process</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public OperationResult Process(RelationshipTypeEntityTypeMappingCollection relationshipTypeEntityTypeMapping, CallerContext callerContext)
        {
            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("RelationshipTypeEntityTypeMappingBL.Process", MDMTraceSource.DataModel, false);

            OperationResult operationResult = new OperationResult();
            operationResult.OperationResultStatus = OperationResultStatusEnum.Failed;
            RelationshipTypeEntityTypeMappingCollection originalRelationshipTypeEntityTypeMappings = null;

            try
            {
                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    PopulateProgramName(callerContext);
                    originalRelationshipTypeEntityTypeMappings = GetAll(callerContext);
                    _relationshipTypeEntityTypeMappingDA.Process(relationshipTypeEntityTypeMapping, _securityPrincipal.CurrentUserName, callerContext.ProgramName);

                    transactionScope.Complete();
                    operationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                    RemoveContainerRelationshipTypeEntityTypeMapping();
                }

                #region activity log

                if (operationResult.OperationResultStatus == OperationResultStatusEnum.Successful && AppConfigurationHelper.GetAppConfig<Boolean>(Constants.ACTIVITYLOG_DATAMODEL_ENABLED_APPCONFIG_KEY, false))
                {
                    LogDataModelChanges(relationshipTypeEntityTypeMapping, callerContext, originalRelationshipTypeEntityTypeMappings);
                }

                #endregion activity log
            }

            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("RelationshipTypeEntityTypeMappingBL.Process", MDMTraceSource.DataModel);
            }
            return operationResult;
        }

        #endregion

        #region Get Methods

        /// <summary>
        /// Gets all RelationshipType EntityType mappings from the system
        /// </summary>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>All RelationshipType EntityType mappings</returns>
        public RelationshipTypeEntityTypeMappingCollection GetAll(CallerContext callerContext)
        {
            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("ContainerEntityTypeAttributeMappingBL.GetAll", MDMTraceSource.DataModel, false);

            RelationshipTypeEntityTypeMappingCollection relationshipTypeEntityTypeMappings = new RelationshipTypeEntityTypeMappingCollection();

            try
            {
                relationshipTypeEntityTypeMappings = _relationshipTypeEntityTypeMappingDA.Get(-1, -1);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerEntityTypeAttributeMappingBL.GetAll", MDMTraceSource.DataModel);
            }
          
            return relationshipTypeEntityTypeMappings;
        }

        /// <summary>
        /// Get RelationshipType EntityType Mapping by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RelationshipTypeEntityTypeMapping GetById(Int32 id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets RelationshipType EntityType mappings from the system based on RelationshipType Id
        /// </summary>
        /// <param name="relationshipTypeId">Indicates the RelationshipType Id for which mappings needs to be fetched</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>RelationshipType EntityType mappings for specified RelationshipType Id</returns>
        public RelationshipTypeEntityTypeMappingCollection GetMappingsByRelationshipTypeId(Int32 relationshipTypeId, CallerContext callerContext)
        {
            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("RelationshipTypeEntityTypeMappingBL.GetMappingsByRelationshipTypeId", MDMTraceSource.DataModel, false);

            RelationshipTypeEntityTypeMappingCollection relationshipTypeEntityTypeMappings = new RelationshipTypeEntityTypeMappingCollection();
            try
            {

                relationshipTypeEntityTypeMappings = _relationshipTypeEntityTypeMappingDA.Get(relationshipTypeId, -1);

            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("RelationshipTypeEntityTypeMappingBL.GetMappingsByRelationshipTypeId", MDMTraceSource.DataModel);
            }
            return relationshipTypeEntityTypeMappings;
        }

        /// <summary>
        /// Gets RelationshipType EntityType mappings from the system from the system based on EntityType Id
        /// </summary>
        /// <param name="entityTypeId">Indicates the EntityType Id for which mappings needs to be fetched</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>RelationshipType EntityType mappings for specified EntityType Id</returns>
        public RelationshipTypeEntityTypeMappingCollection GetMappingsByEntityTypeId(Int32 entityTypeId, CallerContext callerContext)
        {
            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("RelationshipTypeEntityTypeMappingBL.GetMappingsByEntityTypeId", MDMTraceSource.DataModel, false);

            RelationshipTypeEntityTypeMappingCollection relationshipTypeEntityTypeMappings = new RelationshipTypeEntityTypeMappingCollection();
            try
            {
                relationshipTypeEntityTypeMappings = _relationshipTypeEntityTypeMappingDA.Get(-1, entityTypeId);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("RelationshipTypeEntityTypeMappingBL.GetMappingsByEntityTypeId", MDMTraceSource.DataModel);
            }
            return relationshipTypeEntityTypeMappings;
        }

        /// <summary>
        /// Gets mapped EntityTypes from the system based on RelationshipType Id
        /// </summary>
        /// <param name="relationshipTypeId">Indicates the RelationshipType Id for which mapped EntityTypes needs to be fetched</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Mapped EntityTypes for specified RelationshipType Id</returns>
        public EntityTypeCollection GetMappedEntityTypes(Int32 relationshipTypeId, CallerContext callerContext)
        {
            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("RelationshipTypeEntityTypeMappingBL.GetMappedEntityTypes", MDMTraceSource.DataModel, false);

            RelationshipTypeEntityTypeMappingCollection relationshipTypeEntityTypeMappings = new RelationshipTypeEntityTypeMappingCollection();
            EntityTypeCollection entityTypes = new EntityTypeCollection();
            try
            {
                relationshipTypeEntityTypeMappings = _relationshipTypeEntityTypeMappingDA.Get(relationshipTypeId, -1);

                foreach (RelationshipTypeEntityTypeMapping mapping in relationshipTypeEntityTypeMappings)
                {
                    if (mapping.EntityTypeId > 0)
                    {
                        EntityType entityType = new EntityType(mapping.EntityTypeId);
                        entityType.LongName = mapping.EntityTypeName;
                        entityTypes.Add(entityType);
                    }
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("RelationshipTypeEntityTypeMappingBL.GetMappedEntityTypes", MDMTraceSource.DataModel);
            }

            return entityTypes;
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
            try
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StartTraceActivity("RelationshipTypeEntityTypeMappingBL.PrepareOperationResultsSchema", MDMTraceSource.DataModel, false);
                }

                String separator = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.RSDataModelExcel.Separator", "//").Trim();
                RelationshipTypeEntityTypeMappingCollection relationshipTypeEntityTypeMappings = iDataModelObjects as RelationshipTypeEntityTypeMappingCollection;

                if (relationshipTypeEntityTypeMappings != null && relationshipTypeEntityTypeMappings.Count > 0)
                {
                    if (operationResults.Count > 0)
                    {
                        operationResults.Clear();
                    }

                    Int32 relationshipTypeEntityTypeMappingIdToBeCreated = -1;

                    foreach (RelationshipTypeEntityTypeMapping relationshipTypeEntityTypeMapping in relationshipTypeEntityTypeMappings)
                    {
                        DataModelOperationResult relationshipTypeEntityTypeMappingOperationResult = new DataModelOperationResult(relationshipTypeEntityTypeMapping.Id, relationshipTypeEntityTypeMapping.LongName, relationshipTypeEntityTypeMapping.ExternalId, relationshipTypeEntityTypeMapping.ReferenceId);

                        if (String.IsNullOrEmpty(relationshipTypeEntityTypeMappingOperationResult.ExternalId))
                        {
                            relationshipTypeEntityTypeMappingOperationResult.ExternalId = String.Format("{1} {0} {2}", separator, relationshipTypeEntityTypeMapping.RelationshipTypeName, relationshipTypeEntityTypeMapping.EntityTypeName);
                        }

                        if (relationshipTypeEntityTypeMapping.Id < 1)
                        {
                            relationshipTypeEntityTypeMapping.Id = relationshipTypeEntityTypeMappingIdToBeCreated;
                            relationshipTypeEntityTypeMappingOperationResult.Id = relationshipTypeEntityTypeMappingIdToBeCreated;
                            relationshipTypeEntityTypeMappingIdToBeCreated--;
                        }

                        operationResults.Add(relationshipTypeEntityTypeMappingOperationResult);
                    }
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("RelationshipTypeEntityTypeMappingBL.PrepareOperationResultsSchema", MDMTraceSource.DataModel);
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
                    MDMTraceHelper.StartTraceActivity("RelationshipTypeEntityTypeMappingBL.Validate", MDMTraceSource.DataModel, false);
                }

                RelationshipTypeEntityTypeMappingCollection relationshipTypeEntityTypeMappings = iDataModelObjects as RelationshipTypeEntityTypeMappingCollection;
                CallerContext callerContext = iCallerContext as CallerContext;
                
                #region Parameter Validations

                ValidateInputParameters(relationshipTypeEntityTypeMappings, operationResults, callerContext);

                #endregion Parameter Validations

                #region Data Validations

                ValidateData(relationshipTypeEntityTypeMappings, operationResults, callerContext);

                #endregion Data Validations

            }
            catch (Exception ex)
            {
                DataModelHelper.AddOperationResults(operationResults, String.Empty, ex.Message, null, OperationResultType.Error, TraceEventType.Error, iCallerContext as CallerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("RelationshipTypeEntityTypeMappingBL.Validate", MDMTraceSource.DataModel);
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
                    MDMTraceHelper.StartTraceActivity("RelationshipTypeEntityTypeMappingBL.LoadOriginal", MDMTraceSource.DataModel, false);

                LoadOriginalRelationshipTypeEntityTypeMapping(iDataModelObjects as RelationshipTypeEntityTypeMappingCollection, iCallerContext as CallerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("RelationshipTypeEntityTypeMappingBL.LoadOriginal", MDMTraceSource.DataModel);
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
                    MDMTraceHelper.StartTraceActivity("RelationshipTypeEntityTypeMappingBL.FillDataModel", MDMTraceSource.DataModel, false);

                FillRelationshipTypeEntityTypeMapping(iDataModelObjects as RelationshipTypeEntityTypeMappingCollection, iCallerContext as CallerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("RelationshipTypeEntityTypeMappingBL.FillDataModel", MDMTraceSource.DataModel);
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
                    MDMTraceHelper.StartTraceActivity("RelationshipTypeEntityTypeMappingBL.CompareAndMerge", MDMTraceSource.DataModel, false);

                CompareAndMerge(iDataModelObjects as RelationshipTypeEntityTypeMappingCollection, operationResults, iCallerContext as CallerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("RelationshipTypeEntityTypeMappingBL.CompareAndMerge", MDMTraceSource.DataModel);
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
                    MDMTraceHelper.StartTraceActivity("RelationshipTypeEntityTypeMappingBL.Process", MDMTraceSource.DataModel, false);

                RelationshipTypeEntityTypeMappingCollection relationshipTypeEntityTypeMappings = iDataModelObjects as RelationshipTypeEntityTypeMappingCollection;
                CallerContext callerContext = iCallerContext as CallerContext;

                if (relationshipTypeEntityTypeMappings.Count > 0)
                {
                    PopulateProgramName(callerContext);

                    DBCommandProperties command = DBCommandHelper.Get((CallerContext)iCallerContext, MDMCenterModuleAction.Execute);

                    #region Perform RelationshipTypeEntityTypeMapping updates in database

                    using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                    {
                        _relationshipTypeEntityTypeMappingDA.Process(relationshipTypeEntityTypeMappings, operationResults, _securityPrincipal.CurrentUserName, callerContext.ProgramName);

                        transactionScope.Complete();
                    }

                    LocalizeErrors(callerContext, operationResults);

                    #endregion

                    #region activity log

                    if (operationResults.OperationResultStatus == OperationResultStatusEnum.Successful && AppConfigurationHelper.GetAppConfig<Boolean>(Constants.ACTIVITYLOG_DATAMODEL_ENABLED_APPCONFIG_KEY, false))
                    {
                        LogDataModelChanges(relationshipTypeEntityTypeMappings, callerContext);
                    }

                    #endregion activity log

                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("RelationshipTypeEntityTypeMappingBL.Process", MDMTraceSource.DataModel);
            }
        }

        ///<summary>
        /// Processes the Entity cache statuses for data model objects
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be process entity cache load.</param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void InvalidateCache(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            RemoveContainerRelationshipTypeEntityTypeMapping();
        }
        
        #endregion

        #endregion

        #region Private Methods

        #region Validation Methods

        /// <summary>
        /// Validate the input parameters in relationship type entity type mapping collection
        /// </summary>
        /// <param name="relationshipTypeEntityTypeMappings">Indicates the collection of relationship type entity type mapping</param>
        /// <param name="callerContext">Indicates the context of the caller</param>
        private void ValidateInputParameters(RelationshipTypeEntityTypeMappingCollection relationshipTypeEntityTypeMappings, CallerContext callerContext)
        {
            String errorMessage;

            if (relationshipTypeEntityTypeMappings == null || relationshipTypeEntityTypeMappings.Count < 1)
            {
                errorMessage = "RelationshipTypeEntityTypeMapping Collection cannot be null.";
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, errorMessage, MDMTraceSource.DataModel);
                throw new MDMOperationException("", errorMessage, "DataModelManager.RelationshipTypeEntityTypeMappingBL", String.Empty, "Create");
            }

            if (callerContext == null)
            {
                errorMessage = "CallerContext cannot be null.";
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "CallerContext cannot be null.", MDMTraceSource.DataModel);
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "DataModelManager.RelationshipTypeEntityTypeMappingBL", String.Empty, "Create");
            }
        }
      
        /// <summary>
        /// Validate the input parameters in relationship type entity type mapping collection
        /// </summary>
        /// <param name="relationshipTypeEntityTypeMappings">Indicates the collection of relationship type entity type mapping</param>
        /// <param name="operationResults">Indicates the result of operation</param>
        /// <param name="callerContext">Indicates the context of the caller</param>
        private void ValidateInputParameters(RelationshipTypeEntityTypeMappingCollection relationshipTypeEntityTypeMappings, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            if (relationshipTypeEntityTypeMappings == null || relationshipTypeEntityTypeMappings.Count < 1)
            {
                DataModelHelper.AddOperationResults(operationResults, "113623", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }

            if (callerContext == null)
            {
                DataModelHelper.AddOperationResults(operationResults, "111846", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
        }

        /// <summary>
        /// Indicates the data of specified parameters in relationship type entity type mapping collection
        /// </summary>
        /// <param name="relationshipTypeEntityTypeMappings">Indicates the collection of relationship type entity type mapping</param>
        /// <param name="callerContext">Indicates the context of the caller making call to this API</param>
        private void ValidateData(RelationshipTypeEntityTypeMappingCollection relationshipTypeEntityTypeMappings, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            if (relationshipTypeEntityTypeMappings != null && relationshipTypeEntityTypeMappings.Count > 0)
            {
                Collection<String> relationshipTypeEntityTypeMappingList = new Collection<String>();

                foreach (RelationshipTypeEntityTypeMapping relationshipTypeEntityTypeMapping in relationshipTypeEntityTypeMappings)
                {
                    Collection<String> uniqueIdentifierList = new Collection<String>(){ relationshipTypeEntityTypeMapping.RelationshipTypeName, 
                                                                                            relationshipTypeEntityTypeMapping.EntityTypeName};

                    String rowUniqueIdentifier = DataModelHelper.GetUniqueIdentifier(uniqueIdentifierList);

                    IDataModelOperationResult dataModelOperationResult = operationResults.GetByReferenceId(
                                                                            relationshipTypeEntityTypeMapping.ReferenceId);

                    DataModelHelper.ValidateRelationshipTypeName(relationshipTypeEntityTypeMapping.RelationshipTypeName, 
                                                                    dataModelOperationResult, callerContext);

                    DataModelHelper.ValidateEntityTypeName(relationshipTypeEntityTypeMapping.EntityTypeName, 
                                                            dataModelOperationResult, callerContext);

                    if (relationshipTypeEntityTypeMappingList.Contains(rowUniqueIdentifier))
                    {
                        Object[] parameters = null;
                        parameters = new Object[] { relationshipTypeEntityTypeMapping.RelationshipTypeName, 
                                                        relationshipTypeEntityTypeMapping.EntityTypeName };

                        String errorMessage = String.Format("Duplicate mappings found for the relationship type entity type mapping with relationship type: {0} and entity type: {1}", parameters);

                        DataModelHelper.AddOperationResult(dataModelOperationResult, "113963", errorMessage, parameters,
                                                                OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                    else
                    {
                        relationshipTypeEntityTypeMappingList.Add(rowUniqueIdentifier);
                    }
                }
            }
        }

        #endregion

        #region Get Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationshipTypeEntityTypeMappings"></param>
        /// <param name="callerContext"></param>
        private void LoadOriginalRelationshipTypeEntityTypeMapping(RelationshipTypeEntityTypeMappingCollection relationshipTypeEntityTypeMappings, CallerContext callerContext)
        {
            RelationshipTypeEntityTypeMappingCollection orginalRelationshipTypeEntityTypeMappingCollection = GetAll(callerContext);

            if (orginalRelationshipTypeEntityTypeMappingCollection != null && orginalRelationshipTypeEntityTypeMappingCollection.Count > 0)
            {
                foreach (RelationshipTypeEntityTypeMapping relationshipTypeEntityTypeMapping in relationshipTypeEntityTypeMappings)
                {
                    relationshipTypeEntityTypeMapping.OriginalRelationshipTypeEntityTypeMapping = orginalRelationshipTypeEntityTypeMappingCollection.Get(relationshipTypeEntityTypeMapping.EntityTypeName, relationshipTypeEntityTypeMapping.RelationshipTypeName);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RelationshipTypeEntityTypeMappingCollection"></param>
        /// <param name="callerContext"></param>
        private void FillRelationshipTypeEntityTypeMapping(RelationshipTypeEntityTypeMappingCollection relationshipTypeEntityTypeMappings, CallerContext callerContext)
        {
            EntityTypeCollection entityTypes = new EntityTypeCollection();
            RelationshipTypeCollection relationshipTypes = new RelationshipTypeCollection();

            foreach (RelationshipTypeEntityTypeMapping relationshipTypeEntityTypeMapping in relationshipTypeEntityTypeMappings)
            {
                if (relationshipTypeEntityTypeMapping.OriginalRelationshipTypeEntityTypeMapping != null)
                {
                    relationshipTypeEntityTypeMapping.Id = relationshipTypeEntityTypeMapping.OriginalRelationshipTypeEntityTypeMapping.Id;
                    relationshipTypeEntityTypeMapping.EntityTypeId = relationshipTypeEntityTypeMapping.OriginalRelationshipTypeEntityTypeMapping.EntityTypeId;
                    relationshipTypeEntityTypeMapping.RelationshipTypeId = relationshipTypeEntityTypeMapping.OriginalRelationshipTypeEntityTypeMapping.RelationshipTypeId;
                }
                else
                {
                    relationshipTypeEntityTypeMapping.EntityTypeId = DataModelHelper.GetEntityTypeId(_iEntityTypeManager, relationshipTypeEntityTypeMapping.EntityTypeName, ref entityTypes, callerContext);
                    relationshipTypeEntityTypeMapping.RelationshipTypeId = DataModelHelper.GetRelationshipTypeId(_iRelationshipTypeManager, relationshipTypeEntityTypeMapping.RelationshipTypeName, ref relationshipTypes, callerContext);
                }
            }
        }

        #endregion

        #region Process Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationshipTypeEntityTypeMappings"></param>
        /// <param name="callerContext"></param>
        private void CompareAndMerge(RelationshipTypeEntityTypeMappingCollection relationshipTypeEntityTypeMappings, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            foreach (RelationshipTypeEntityTypeMapping deltaRelationshipTypeEntityTypeMapping in relationshipTypeEntityTypeMappings)
            {
                IDataModelOperationResult operationResult = operationResults.GetByReferenceId(deltaRelationshipTypeEntityTypeMapping.ReferenceId);

                //If delta object Action is Read/Ignore then skip Merge Delta Process.
                if (deltaRelationshipTypeEntityTypeMapping.Action == ObjectAction.Read || deltaRelationshipTypeEntityTypeMapping.Action == ObjectAction.Ignore)
                    continue;

                IRelationshipTypeEntityTypeMapping originalRelationshipTypeEntityTypeMapping = deltaRelationshipTypeEntityTypeMapping.OriginalRelationshipTypeEntityTypeMapping;

                if (originalRelationshipTypeEntityTypeMapping != null)
                {
                    //If delta object Action is Delete then skip Merge Delta process.
                    if (deltaRelationshipTypeEntityTypeMapping.Action != ObjectAction.Delete)
                    {
                        originalRelationshipTypeEntityTypeMapping.MergeDelta(deltaRelationshipTypeEntityTypeMapping, callerContext, false);
                    }
                }
                else
                {
                    Object[] parameters = null;
                    if (deltaRelationshipTypeEntityTypeMapping.Action == ObjectAction.Delete)
                    {
                        parameters = new Object[] { deltaRelationshipTypeEntityTypeMapping.RelationshipTypeName, deltaRelationshipTypeEntityTypeMapping.EntityTypeName };
                        DataModelHelper.AddOperationResult(operationResult, "113624", String.Empty, parameters, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                    else
                    {
                        if (deltaRelationshipTypeEntityTypeMapping.EntityTypeId < 1 || deltaRelationshipTypeEntityTypeMapping.RelationshipTypeId < 1)
                        {
                            parameters = new Object[] { deltaRelationshipTypeEntityTypeMapping.EntityTypeName, deltaRelationshipTypeEntityTypeMapping.RelationshipTypeName };
                            DataModelHelper.AddOperationResult(operationResult, "113627", String.Empty, parameters, OperationResultType.Error, TraceEventType.Error, callerContext);
                        }
                        //If original object is not found then set Action as Create always.
                        deltaRelationshipTypeEntityTypeMapping.Action = ObjectAction.Create;
                    }
                    operationResult.PerformedAction = deltaRelationshipTypeEntityTypeMapping.Action;
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
                callerContext.ProgramName = "RelationshipTypeEntityTypeMappingBL";
            }
        }

        /// <summary>
        /// This method removes container relationship type entity type mappings from local cache.
        /// </summary>
        private void RemoveContainerRelationshipTypeEntityTypeMapping()
        {
            _mappingBufferManager.RemoveContainerRelationshipTypeEntityTypeMapping();
            _mappingBufferManager.NotifyModelDataChange(CacheKeyContants.RELATIONSHIP_HIERARCHY_CHANGED_KEY);
        }

        /// <summary>
        /// Log changes to tb_datamodelactivitylog if DA call is successful and appconfig key datamodelactivitylog.enable == true
        /// </summary>
        /// <param name="relTypeEntTypeMappings"></param>
        /// <param name="callerContext"></param>
        private void LogDataModelChanges(RelationshipTypeEntityTypeMappingCollection relTypeEntTypeMappings, CallerContext callerContext, RelationshipTypeEntityTypeMappingCollection originalMappings = null)
        {
            #region Step: Populate datamodelactivitylog object

            DataModelActivityLogBL dataModelActivityLogManager = new DataModelActivityLogBL();
            DataModelActivityLogCollection activityLogCollection = dataModelActivityLogManager.FillDataModelActivityLogCollection(relTypeEntTypeMappings, relTypeEntTypeMappings.DataModelObjectType, callerContext, originalMappings);

            #endregion Step: Populate datamodelactivitylog object

            #region Step: Make api call

            if (activityLogCollection != null && activityLogCollection.Count > 0) // null activity log collection means there was error in mapping
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