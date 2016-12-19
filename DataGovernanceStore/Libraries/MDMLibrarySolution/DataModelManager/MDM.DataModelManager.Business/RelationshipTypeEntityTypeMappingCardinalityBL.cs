using System;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Transactions;
using System.Collections.ObjectModel;
using SM = System.ServiceModel;

namespace MDM.DataModelManager.Business
{
    using MDM.BufferManager;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.DataModel;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.DataModelManager.Data;
    using MDM.Interfaces;
    using MDM.MessageManager.Business;
    using MDM.Utility;
    using MDM.BusinessObjects.Interfaces;
    using MDM.CacheManager.Business;

    /// <summary>
    /// Represents business logic for RelationshipType EntityType mapping cardinality.
    /// </summary>
    public class RelationshipTypeEntityTypeMappingCardinalityBL : BusinessLogicBase, IDataModelManager
    {
        #region Fields

        /// <summary>
        /// Field denoting the security principal
        /// </summary>
        private SecurityPrincipal _securityPrincipal = null;

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
        /// RelationshipTypeEntityTypeMappingCardinality data access object
        /// </summary>
        private RelationshipTypeEntityTypeMappingCardinalityDA _relationshipTypeEntityTypeMappingCardinalityDA = new RelationshipTypeEntityTypeMappingCardinalityDA();

        /// <summary>
        /// Field denoting the reference of EntityTypeBL.
        /// </summary>
        private IEntityTypeManager _iEntityTypeManager = null;

        /// <summary>
        /// Field denoting the reference of RelationshipTypeBL.
        /// </summary>
        private IRelationshipTypeManager _iRelationshipTypeManager = null;

        /// <summary>
        /// Field denoting the reference of ContainerBL.
        /// </summary>
        private IContainerManager _iContainerManager = null;

        /// <summary>
        /// Specifies mapping buffer manager
        /// </summary>
        private MappingBufferManager _mappingBufferManager = null;

        /// <summary>
        /// Indicates ui locale of a system
        /// </summary>
        private LocaleEnum _systemUILocale = LocaleEnum.UnKnown;

        /// <summary>
        /// 
        /// </summary>
        SM.OperationContext _operationContext = SM.OperationContext.Current;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public RelationshipTypeEntityTypeMappingCardinalityBL()
        {
            InitializeMembers();
        }
        
        /// <summary>
        /// Initializes a new instance of the RelationshipTypeEntityTypeMappingCardinalityBL.
        /// </summary>
        /// <param name="iEntityTypeManager">Represents the reference of EntityTypeBL.</param>
        /// <param name="iRelationshipTypeManager">Represents the RelationshipTypeBL.</param>
        /// <param name="iContainerManager">Represents the ContainerBL.</param>
        public RelationshipTypeEntityTypeMappingCardinalityBL(IEntityTypeManager iEntityTypeManager, IRelationshipTypeManager iRelationshipTypeManager, IContainerManager iContainerManager)
        {
            this._iEntityTypeManager = iEntityTypeManager;
            this._iRelationshipTypeManager = iRelationshipTypeManager;
            this._iContainerManager = iContainerManager;

            InitializeMembers();
        }

        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        #region Get Methods

        /// <summary>
        /// Gets all RelationshipType EntityType Mappings Cardinality from the system
        /// </summary>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>All RelationshipType EntityType Mappings cardinality</returns>
        public RelationshipTypeEntityTypeMappingCardinalityCollection GetAll(CallerContext callerContext)
        {
            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("RelationshipTypeEntityTypeMappingCardinalityBL.GetAll", MDMTraceSource.DataModel, false);

            RelationshipTypeEntityTypeMappingCardinalityCollection relationshipTypeEntityTypeMappingCardinalitys = new RelationshipTypeEntityTypeMappingCardinalityCollection();

            try
            {
                relationshipTypeEntityTypeMappingCardinalitys = _relationshipTypeEntityTypeMappingCardinalityDA.Get(-1, -1);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("RelationshipTypeEntityTypeMappingCardinalityBL.GetAll", MDMTraceSource.DataModel);
            }

            return relationshipTypeEntityTypeMappingCardinalitys;
        }

        /// <summary>
        /// Gets RelationshipType EntityType Mappings Cardinality from the system based on RelationshipType Id and From EntityType Id
        /// </summary>
        /// <param name="fromEntityTypeId">Indicates the From EntityType Id for which mappings needs to be fetched</param>
        /// <param name="relationshipTypeId">Indicates the RelationshipType Id for which mappings needs to be fetched</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>RelationshipType EntityType Mappings cardinality for specified RelationshipType Id and From EntityType Id</returns>
        public RelationshipTypeEntityTypeMappingCardinalityCollection Get(Int32 fromEntityTypeId, Int32 relationshipTypeId, CallerContext callerContext)
        {
            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("RelationshipTypeEntityTypeMappingCardinalityBL.Get", MDMTraceSource.DataModel, false);

            RelationshipTypeEntityTypeMappingCardinalityCollection relationshipTypeEntityTypeMappingCardinalities = new RelationshipTypeEntityTypeMappingCardinalityCollection();
            try
            {
                relationshipTypeEntityTypeMappingCardinalities = _relationshipTypeEntityTypeMappingCardinalityDA.Get(fromEntityTypeId, relationshipTypeId);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("RelationshipTypeEntityTypeMappingCardinalityBL.Get", MDMTraceSource.DataModel);
            }
            return relationshipTypeEntityTypeMappingCardinalities;
        }

        /// <summary>
        /// Gets RelationshipType EntityType Mappings Cardinality from the system based on RelationshipType Id
        /// </summary>
        /// <param name="relationshipTypeId">Indicates the RelationshipType Id for which mappings needs to be fetched</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>RRelationshipType EntityType Mappings cardinality for specified RelationshipType Id</returns>
        public RelationshipTypeEntityTypeMappingCardinalityCollection GetByRelationshipTypeId(Int32 relationshipTypeId, CallerContext callerContext)
        {
            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("RelationshipTypeEntityTypeMappingCardinalityBL.Get", MDMTraceSource.DataModel, false);

            RelationshipTypeEntityTypeMappingCardinalityCollection relationshipTypeEntityTypeMappingCardinalitys = new RelationshipTypeEntityTypeMappingCardinalityCollection();
            try
            {
                relationshipTypeEntityTypeMappingCardinalitys = _relationshipTypeEntityTypeMappingCardinalityDA.Get(-1, relationshipTypeId);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("RelationshipTypeEntityTypeMappingCardinalityBL.Get", MDMTraceSource.DataModel);
            }
            return relationshipTypeEntityTypeMappingCardinalitys;
        }

        /// <summary>
        /// Gets RelationshipType EntityType Mappings Cardinality from the system based on From EntityType Id
        /// </summary>
        /// <param name="fromEntityTypeId">Indicates the From EntityType Id for which mappings needs to be fetched</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>RelationshipType EntityType Mappings cardinality for specified From EntityType Id</returns>
        public RelationshipTypeEntityTypeMappingCardinalityCollection GetByFromEntityTypeId(Int32 fromEntityTypeId, CallerContext callerContext)
        {
            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("RelationshipTypeEntityTypeMappingCardinalityBL.Get", MDMTraceSource.DataModel, false);

            RelationshipTypeEntityTypeMappingCardinalityCollection relationshipTypeEntityTypeMappingCardinalitys = new RelationshipTypeEntityTypeMappingCardinalityCollection();
            try
            {
                relationshipTypeEntityTypeMappingCardinalitys = _relationshipTypeEntityTypeMappingCardinalityDA.Get(fromEntityTypeId, -1);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("RelationshipTypeEntityTypeMappingCardinalityBL.Get", MDMTraceSource.DataModel);
            }
            return relationshipTypeEntityTypeMappingCardinalitys;
        }
        #endregion

        #region CUD Methods

        /// <summary>
        /// Add new RelationshipTypeEntityTypeMappingCardinality
        /// </summary>
        /// <param name="relationshipTypeEntityTypeMappingCardinality"></param>
        /// <param name="callerContext"></param>
        /// <param name="iContainerManager">Represents the ContainerBL.</param>
        /// <returns>OperationResult</returns>
        public OperationResult Create(RelationshipTypeEntityTypeMappingCardinality relationshipTypeEntityTypeMappingCardinality, CallerContext callerContext, IContainerManager iContainerManager)
        {
            relationshipTypeEntityTypeMappingCardinality.Action = ObjectAction.Create;
            RelationshipTypeEntityTypeMappingCardinalityCollection relationshipTypeEntityTypeMappingCardinalitys = new RelationshipTypeEntityTypeMappingCardinalityCollection();
            relationshipTypeEntityTypeMappingCardinalitys.Add(relationshipTypeEntityTypeMappingCardinality);
            return GetOperationResultFromCollection(Process(relationshipTypeEntityTypeMappingCardinalitys, callerContext, iContainerManager));
        }

        /// <summary>
        /// Update existing RelationshipTypeEntityTypeMappingCardinality
        /// </summary>
        /// <param name="relationshipTypeEntityTypeMappingCardinality"></param>
        /// <param name="callerContext"></param>
        /// <param name="iContainerManager">Represents the ContainerBL.</param>
        /// <returns>OperationResult</returns>
        public OperationResult Update(RelationshipTypeEntityTypeMappingCardinality relationshipTypeEntityTypeMappingCardinality, CallerContext callerContext , IContainerManager iContainerManager)
        {
            relationshipTypeEntityTypeMappingCardinality.Action = ObjectAction.Update;
            RelationshipTypeEntityTypeMappingCardinalityCollection relationshipTypeEntityTypeMappingCardinalitys = new RelationshipTypeEntityTypeMappingCardinalityCollection();
            relationshipTypeEntityTypeMappingCardinalitys.Add(relationshipTypeEntityTypeMappingCardinality);
            return GetOperationResultFromCollection(Process(relationshipTypeEntityTypeMappingCardinalitys, callerContext , iContainerManager));
        }

        /// <summary>
        /// Remove existing RelationshipTypeEntityTypeMappingCardinality
        /// </summary>
        /// <param name="relationshipTypeEntityTypeMappingCardinality"></param>
        /// <param name="callerContext"></param>
        /// <param name="iContainerManager">Represents the ContainerBL.</param>
        /// <returns>OperationResult</returns>
        public OperationResult Delete(RelationshipTypeEntityTypeMappingCardinality relationshipTypeEntityTypeMappingCardinality, CallerContext callerContext, IContainerManager iContainerManager)
        {
            relationshipTypeEntityTypeMappingCardinality.Action = ObjectAction.Delete;
            RelationshipTypeEntityTypeMappingCardinalityCollection relationshipTypeEntityTypeMappingCardinalitys = new RelationshipTypeEntityTypeMappingCardinalityCollection();
            relationshipTypeEntityTypeMappingCardinalitys.Add(relationshipTypeEntityTypeMappingCardinality);
            return GetOperationResultFromCollection(Process(relationshipTypeEntityTypeMappingCardinalitys, callerContext, iContainerManager));
        }

        /// <summary>
        /// Create, Update or Delete RelationshipTypeEntityTypeMappingCardinalityCollection
        /// </summary>
        /// <param name="relationshipTypeEntityTypeMappingCardinalitys">RelationshipType EntityType mappings to process</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <param name="iContainerManager">Represents the ContainerBL.</param>
        /// <returns>Result of the operation Collection</returns>
        public OperationResultCollection Process(RelationshipTypeEntityTypeMappingCardinalityCollection relationshipTypeEntityTypeMappingCardinality, CallerContext callerContext, IContainerManager iContainerManager)
        {
            MDMTraceHelper.InitializeTraceSource();

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("RelationshipTypeEntityTypeMappingCardinalityBL.Process", MDMTraceSource.DataModel, false);

            OperationResultCollection operationResults = null;

            try
            {
                ValidateInputParameters(relationshipTypeEntityTypeMappingCardinality, callerContext);

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    PopulateProgramName(callerContext);
                    operationResults = _relationshipTypeEntityTypeMappingCardinalityDA.Process(relationshipTypeEntityTypeMappingCardinality, _securityPrincipal.CurrentUserName, callerContext.ProgramName);
                    transactionScope.Complete();
                }

                if (operationResults.OperationResultStatus != OperationResultStatusEnum.Failed)
                {
                    this._iContainerManager = iContainerManager;

                    Task.Factory.StartNew(() => InvalidateImpactedDataAsync(relationshipTypeEntityTypeMappingCardinality, callerContext));
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("RelationshipTypeEntityTypeMappingCardinalityBL.Process", MDMTraceSource.DataModel);
            }

            return operationResults;
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
                    MDMTraceHelper.StartTraceActivity("RelationshipTypeEntityTypeMappingCardinalityBL.PrepareOperationResultsSchema", MDMTraceSource.DataModel, false);

                String separator = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.RSDataModelExcel.Separator", "//").Trim();
                RelationshipTypeEntityTypeMappingCardinalityCollection relationshipTypeEntityTypeMappingCardinalitys = iDataModelObjects as RelationshipTypeEntityTypeMappingCardinalityCollection;

                if (relationshipTypeEntityTypeMappingCardinalitys != null && relationshipTypeEntityTypeMappingCardinalitys.Count > 0)
                {
                    if (operationResults.Count > 0)
                    {
                        operationResults.Clear();
                    }

                    Int32 relationshipTypeEntityTypeMappingCardinalityIdToBeCreated = -1;

                    foreach (RelationshipTypeEntityTypeMappingCardinality relationshipTypeEntityTypeMappingCardinality in relationshipTypeEntityTypeMappingCardinalitys)
                    {
                        DataModelOperationResult relationshipTypeEntityTypeMappingCardinalityOperationResult = new DataModelOperationResult(relationshipTypeEntityTypeMappingCardinality.Id, relationshipTypeEntityTypeMappingCardinality.ExternalId, relationshipTypeEntityTypeMappingCardinality.ExternalId, relationshipTypeEntityTypeMappingCardinality.ReferenceId);

                        if (String.IsNullOrEmpty(relationshipTypeEntityTypeMappingCardinalityOperationResult.ExternalId))
                        {
                            relationshipTypeEntityTypeMappingCardinalityOperationResult.ExternalId = String.Format("{1} {0} {2} {0} {3}", separator, relationshipTypeEntityTypeMappingCardinality.RelationshipTypeName, relationshipTypeEntityTypeMappingCardinality.EntityTypeName, relationshipTypeEntityTypeMappingCardinality.ToEntityTypeName);
                        }

                        if (relationshipTypeEntityTypeMappingCardinality.Id < 1)
                        {
                            relationshipTypeEntityTypeMappingCardinality.Id = relationshipTypeEntityTypeMappingCardinalityIdToBeCreated;
                            relationshipTypeEntityTypeMappingCardinalityOperationResult.Id = relationshipTypeEntityTypeMappingCardinalityIdToBeCreated;
                            relationshipTypeEntityTypeMappingCardinalityIdToBeCreated--;
                        }

                        operationResults.Add(relationshipTypeEntityTypeMappingCardinalityOperationResult);
                    }
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("RelationshipTypeEntityTypeMappingCardinalityBL.PrepareOperationResultsSchema", MDMTraceSource.DataModel);
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
                    MDMTraceHelper.StartTraceActivity("RelationshipTypeEntityTypeMappingCardinalityBL.Validate", MDMTraceSource.DataModel, false);
                }

                RelationshipTypeEntityTypeMappingCardinalityCollection relationshipTypeEntityTypeMappingCardinalities = iDataModelObjects as RelationshipTypeEntityTypeMappingCardinalityCollection;
                CallerContext callerContext = iCallerContext as CallerContext;

                #region Parameter Validations

                ValidateInputParameters(relationshipTypeEntityTypeMappingCardinalities, operationResults, callerContext);

                #endregion

                #region Data Validations

                ValidateData(relationshipTypeEntityTypeMappingCardinalities, operationResults, callerContext);

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
                    MDMTraceHelper.StopTraceActivity("RelationshipTypeEntityTypeMappingCardinalityBL.Validate", MDMTraceSource.DataModel);
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
                    MDMTraceHelper.StartTraceActivity("RelationshipTypeEntityTypeMappingCardinalityBL.LoadOriginal", MDMTraceSource.DataModel, false);

                LoadOriginalRelationshipTypeEntityTypeMappingCardinality(iDataModelObjects as RelationshipTypeEntityTypeMappingCardinalityCollection, iCallerContext as CallerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("RelationshipTypeEntityTypeMappingCardinalityBL.LoadOriginal", MDMTraceSource.DataModel);
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
                    MDMTraceHelper.StartTraceActivity("RelationshipTypeEntityTypeMappingCardinalityBL.FillDataModel", MDMTraceSource.DataModel, false);

                FillRelationshipTypeEntityTypeMappingCardinality(iDataModelObjects as RelationshipTypeEntityTypeMappingCardinalityCollection, iCallerContext as CallerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("RelationshipTypeEntityTypeMappingCardinalityBL.FillDataModel", MDMTraceSource.DataModel);
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
                    MDMTraceHelper.StartTraceActivity("RelationshipTypeEntityTypeMappingCardinalityBL.CompareAndMerge", MDMTraceSource.DataModel, false);

                CompareAndMerge(iDataModelObjects as RelationshipTypeEntityTypeMappingCardinalityCollection, operationResults, iCallerContext as CallerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("RelationshipTypeEntityTypeMappingCardinalityBL.CompareAndMerge", MDMTraceSource.DataModel);
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
                    MDMTraceHelper.StartTraceActivity("RelationshipTypeEntityTypeMappingCardinalityBL.Process", MDMTraceSource.DataModel, false);

                RelationshipTypeEntityTypeMappingCardinalityCollection relationshipTypeEntityTypeMappingCardinalitys = iDataModelObjects as RelationshipTypeEntityTypeMappingCardinalityCollection;
                CallerContext callerContext = iCallerContext as CallerContext;

                if (relationshipTypeEntityTypeMappingCardinalitys.Count > 0)
                {
                    #region Perform RelationshipTypeEntityTypeMappingCardinality updates in database

                    using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                    {
                        PopulateProgramName(callerContext);

                        _relationshipTypeEntityTypeMappingCardinalityDA.Process(relationshipTypeEntityTypeMappingCardinalitys, operationResults, _securityPrincipal.CurrentUserName, callerContext.ProgramName);

                        transactionScope.Complete();
                    }

                    LocalizeErrors(callerContext, operationResults);

                    #endregion
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("RelationshipTypeEntityTypeMappingCardinalityBL.Process", MDMTraceSource.DataModel);
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
            if (operationResults.OperationResultStatus != OperationResultStatusEnum.Failed)
            {
                Task.Factory.StartNew(() => InvalidateImpactedDataAsync(iDataModelObjects as RelationshipTypeEntityTypeMappingCardinalityCollection, iCallerContext as CallerContext));
            }
        }
        
        #endregion

        #endregion

        #region Private Methods

        #region Validation Methods

        /// <summary>
        /// Validate the input parameters in relationship type entity type mapping cardinality collection
        /// </summary>
        /// <param name="relationshipTypeEntityTypeMappingCardinalitys">Indicates the collection of relationship type entity type mapping cardinality</param>
        /// <param name="callerContext">Indicates the context of the caller</param>
        private void ValidateInputParameters(RelationshipTypeEntityTypeMappingCardinalityCollection relationshipTypeEntityTypeMappingCardinalitys, CallerContext callerContext)
        {
            String errorMessage;

            if (relationshipTypeEntityTypeMappingCardinalitys == null || relationshipTypeEntityTypeMappingCardinalitys.Count < 1)
            {
                errorMessage = "RelationshipTypeEntityTypeMappingCardinality Collection cannot be null.";
                throw new MDMOperationException("", errorMessage, "DataModelManager.RelationshipTypeEntityTypeMappingCardinalityBL", String.Empty, "Create");
            }

            if (callerContext == null)
            {
                errorMessage = "CallerContext cannot be null.";
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "DataModelManager.RelationshipTypeEntityTypeMappingCardinalityBL", String.Empty, "Create");
            }
        }

        /// <summary>
        /// Validate the input parameters in relationship type entity type mapping cardinality collection
        /// </summary>
        /// <param name="relationshipTypeEntityTypeMappingCardinalities">Indicates the collection of relationship type entity type mapping cardinality</param>
        /// <param name="operationResults">Indicates the result of operation</param>
        /// <param name="callerContext">Indicates the context of the caller</param>
        private void ValidateInputParameters(RelationshipTypeEntityTypeMappingCardinalityCollection relationshipTypeEntityTypeMappingCardinalities, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            if (callerContext == null)
            {
                DataModelHelper.AddOperationResults(operationResults, "111846", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }

            if (relationshipTypeEntityTypeMappingCardinalities == null || relationshipTypeEntityTypeMappingCardinalities.Count < 1)
            {
                DataModelHelper.AddOperationResults(operationResults, "113646", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
        }

        /// <summary>
        /// Validate the data of specified parameters in relationship type entity type mapping cardinality collection
        /// </summary>
        /// <param name="relationshipTypeEntityTypeMappingCardinalities">Indicates the collection of relationship type entity type mapping cardinality</param>
        /// <param name="operationResults">Indicates the result of operation</param>
        /// <param name="callerContext">Indicates the context of the caller</param>
        private void ValidateData(RelationshipTypeEntityTypeMappingCardinalityCollection relationshipTypeEntityTypeMappingCardinalities, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            if (relationshipTypeEntityTypeMappingCardinalities != null && relationshipTypeEntityTypeMappingCardinalities.Count > 0)
            {
                Collection<String> relationshipTypeEntityTypeMappingList = new Collection<String>();

                foreach (RelationshipTypeEntityTypeMappingCardinality relationshipTypeEntityTypeMappingCardinality in relationshipTypeEntityTypeMappingCardinalities)
                {
                    Collection<String> uniqueIdentifierList = new Collection<String>() { relationshipTypeEntityTypeMappingCardinality.RelationshipTypeName,
                                                                                            relationshipTypeEntityTypeMappingCardinality.EntityTypeName, 
                                                                                            relationshipTypeEntityTypeMappingCardinality.ToEntityTypeName };

                    String rowUniqueIdentifier = DataModelHelper.GetUniqueIdentifier(uniqueIdentifierList);

                    IDataModelOperationResult dataModelOperationResult = operationResults.GetByReferenceId(relationshipTypeEntityTypeMappingCardinality.ReferenceId);

                    DataModelHelper.ValidateMinMaxRelationships(relationshipTypeEntityTypeMappingCardinality.MinRelationships, relationshipTypeEntityTypeMappingCardinality.MaxRelationships, dataModelOperationResult, callerContext);
                    DataModelHelper.ValidateRelationshipTypeName(relationshipTypeEntityTypeMappingCardinality.RelationshipTypeName, dataModelOperationResult, callerContext);
                    DataModelHelper.ValidateEntityTypeName(relationshipTypeEntityTypeMappingCardinality.EntityTypeName, dataModelOperationResult, callerContext);

                    if (relationshipTypeEntityTypeMappingList.Contains(rowUniqueIdentifier))
                    {
                        Object[] parameters = null;
                        parameters = new Object[] { relationshipTypeEntityTypeMappingCardinality.RelationshipTypeName, 
                                                        relationshipTypeEntityTypeMappingCardinality.EntityTypeName, 
                                                        relationshipTypeEntityTypeMappingCardinality.ToEntityTypeName };

                        String errorMessage = String.Format("Duplicate mappings found for the relationship type entity type mapping cardinality with relationship type: {0}, from entity type: {1}, and to entity type: {2}", parameters);

                        DataModelHelper.AddOperationResult(dataModelOperationResult, "113972", errorMessage, parameters,
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
        /// <param name="relationshipTypeEntityTypeMappingCardinalitys"></param>
        /// <param name="callerContext"></param>
        private void LoadOriginalRelationshipTypeEntityTypeMappingCardinality(RelationshipTypeEntityTypeMappingCardinalityCollection relationshipTypeEntityTypeMappingCardinalitys, CallerContext callerContext)
        {
            RelationshipTypeEntityTypeMappingCardinalityCollection orginalRelationshipTypeEntityTypeMappingCardinalityCollection = GetAll(callerContext);

            if (orginalRelationshipTypeEntityTypeMappingCardinalityCollection != null && orginalRelationshipTypeEntityTypeMappingCardinalityCollection.Count > 0)
            {
                foreach (RelationshipTypeEntityTypeMappingCardinality relationshipTypeEntityTypeMappingCardinality in relationshipTypeEntityTypeMappingCardinalitys)
                {
                    relationshipTypeEntityTypeMappingCardinality.OriginalRelationshipTypeEntityTypeMappingCardinality = orginalRelationshipTypeEntityTypeMappingCardinalityCollection.Get(relationshipTypeEntityTypeMappingCardinality.EntityTypeName, relationshipTypeEntityTypeMappingCardinality.RelationshipTypeName, relationshipTypeEntityTypeMappingCardinality.ToEntityTypeName);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationshipTypeEntityTypeMappingCardinalitys"></param>
        /// <param name="callerContext"></param>
        private void FillRelationshipTypeEntityTypeMappingCardinality(RelationshipTypeEntityTypeMappingCardinalityCollection relationshipTypeEntityTypeMappingCardinalitys, CallerContext callerContext)
        {
            EntityTypeCollection entityTypes = new EntityTypeCollection();
            RelationshipTypeCollection relationshipTypes = new RelationshipTypeCollection();
            RelationshipTypeEntityTypeMappingCollection relationshipTypeEntityTypeMappings = new RelationshipTypeEntityTypeMappingCollection();

            foreach (RelationshipTypeEntityTypeMappingCardinality relationshipTypeEntityTypeMappingCardinality in relationshipTypeEntityTypeMappingCardinalitys)
            {
                if (relationshipTypeEntityTypeMappingCardinality.OriginalRelationshipTypeEntityTypeMappingCardinality != null)
                {
                    relationshipTypeEntityTypeMappingCardinality.Id = relationshipTypeEntityTypeMappingCardinality.OriginalRelationshipTypeEntityTypeMappingCardinality.Id;
                    relationshipTypeEntityTypeMappingCardinality.RelationshipTypeId = relationshipTypeEntityTypeMappingCardinality.OriginalRelationshipTypeEntityTypeMappingCardinality.RelationshipTypeId;
                    relationshipTypeEntityTypeMappingCardinality.EntityTypeId = relationshipTypeEntityTypeMappingCardinality.OriginalRelationshipTypeEntityTypeMappingCardinality.EntityTypeId;
                    relationshipTypeEntityTypeMappingCardinality.ToEntityTypeId = relationshipTypeEntityTypeMappingCardinality.OriginalRelationshipTypeEntityTypeMappingCardinality.ToEntityTypeId;
                    relationshipTypeEntityTypeMappingCardinality.RelationshipTypeEntityTypeId = relationshipTypeEntityTypeMappingCardinality.OriginalRelationshipTypeEntityTypeMappingCardinality.RelationshipTypeEntityTypeId;
                }
                else
                {
                    relationshipTypeEntityTypeMappingCardinality.RelationshipTypeId = DataModelHelper.GetRelationshipTypeId(_iRelationshipTypeManager, relationshipTypeEntityTypeMappingCardinality.RelationshipTypeName, ref relationshipTypes, callerContext);
                    relationshipTypeEntityTypeMappingCardinality.EntityTypeId = DataModelHelper.GetEntityTypeId(_iEntityTypeManager, relationshipTypeEntityTypeMappingCardinality.EntityTypeName, ref entityTypes, callerContext);
                    relationshipTypeEntityTypeMappingCardinality.ToEntityTypeId = DataModelHelper.GetEntityTypeId(_iEntityTypeManager, relationshipTypeEntityTypeMappingCardinality.ToEntityTypeName, ref entityTypes, callerContext);
                    relationshipTypeEntityTypeMappingCardinality.RelationshipTypeEntityTypeId = DataModelHelper.GetRelationshipTypeEntityTypeId(relationshipTypeEntityTypeMappingCardinality.RelationshipTypeName, relationshipTypeEntityTypeMappingCardinality.EntityTypeName, ref  relationshipTypeEntityTypeMappings, callerContext);
                }
            }
        }
        
        #endregion

        #region Process Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationshipTypeEntityTypeMappingCardinalitys"></param>
        /// <param name="operationResults"></param>
        /// <param name="callerContext"></param>
        private void CompareAndMerge(RelationshipTypeEntityTypeMappingCardinalityCollection relationshipTypeEntityTypeMappingCardinalitys, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            foreach (RelationshipTypeEntityTypeMappingCardinality deltaRelationshipTypeEntityTypeMappingCardinality in relationshipTypeEntityTypeMappingCardinalitys)
            {
                IDataModelOperationResult operationResult = operationResults.GetByReferenceId(deltaRelationshipTypeEntityTypeMappingCardinality.ReferenceId);

                //If delta object Action is Read/Ignore then skip Merge Delta Process.
                if (deltaRelationshipTypeEntityTypeMappingCardinality.Action == ObjectAction.Read || deltaRelationshipTypeEntityTypeMappingCardinality.Action == ObjectAction.Ignore)
                    continue;

                IRelationshipTypeEntityTypeMappingCardinality originalRelationshipTypeEntityTypeMappingCardinality = deltaRelationshipTypeEntityTypeMappingCardinality.OriginalRelationshipTypeEntityTypeMappingCardinality;

                if (originalRelationshipTypeEntityTypeMappingCardinality != null)
                {
                    //If delta object Action is Delete then skip Merge Delta process.
                    if (deltaRelationshipTypeEntityTypeMappingCardinality.Action != ObjectAction.Delete)
                    {
                        originalRelationshipTypeEntityTypeMappingCardinality.MergeDelta(deltaRelationshipTypeEntityTypeMappingCardinality, callerContext, false);
                    }
                }
                else
                {
                    Object[] parameters = null;
                    if (deltaRelationshipTypeEntityTypeMappingCardinality.Action == ObjectAction.Delete)
                    {
                        parameters = new Object[] { deltaRelationshipTypeEntityTypeMappingCardinality.RelationshipTypeName, deltaRelationshipTypeEntityTypeMappingCardinality.EntityTypeName, deltaRelationshipTypeEntityTypeMappingCardinality.ToEntityTypeName};
                        DataModelHelper.AddOperationResult(operationResult, "113626", String.Empty, parameters, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                    else
                    {
                        if (deltaRelationshipTypeEntityTypeMappingCardinality.EntityTypeId < 1 || deltaRelationshipTypeEntityTypeMappingCardinality.RelationshipTypeId < 1 ||deltaRelationshipTypeEntityTypeMappingCardinality.ToEntityTypeId < 1 || deltaRelationshipTypeEntityTypeMappingCardinality.RelationshipTypeEntityTypeId < 1)
                        {
                            parameters = new Object[] { deltaRelationshipTypeEntityTypeMappingCardinality.EntityTypeName, deltaRelationshipTypeEntityTypeMappingCardinality.RelationshipTypeName, deltaRelationshipTypeEntityTypeMappingCardinality.ToEntityTypeName };
                            DataModelHelper.AddOperationResult(operationResult, "113691", String.Empty, parameters, OperationResultType.Error, TraceEventType.Error, callerContext);
                        }

                        //If original object is not found then set Action as Create always.
                        deltaRelationshipTypeEntityTypeMappingCardinality.Action = ObjectAction.Create;
                    }
                    operationResult.PerformedAction = deltaRelationshipTypeEntityTypeMappingCardinality.Action;
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
        /// <param name="operationResultCollection"></param>
        private OperationResult GetOperationResultFromCollection(OperationResultCollection operationResultCollection)
        {
            OperationResult operationResult = new OperationResult();

            if (operationResultCollection != null && operationResultCollection.Any())
            {
                operationResult = operationResultCollection.First();
            }

            return operationResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callerContext"></param>
        private void PopulateProgramName(CallerContext callerContext)
        {
            if (String.IsNullOrWhiteSpace(callerContext.ProgramName))
            {
                callerContext.ProgramName = "RelationshipTypeEntityTypeMappingCardinalityBL";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeMembers()
        {
            if (_securityPrincipal == null)
            {
                _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            }
            _systemUILocale = GlobalizationHelper.GetSystemUILocale();
            _mappingBufferManager = new MappingBufferManager();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardinalities"></param>
        /// <param name="callerContext"></param>
        private void InvalidateImpactedDataAsync(RelationshipTypeEntityTypeMappingCardinalityCollection cardinalities, CallerContext callerContext)
        {
            _mappingBufferManager.NotifyModelDataChange(CacheKeyContants.RELATIONSHIP_HIERARCHY_CHANGED_KEY);

            if (cardinalities != null)
            {
                SM.OperationContext.Current = _operationContext;

                #region Get All Container Ids

                Collection<Int32> containerIds = null;

                if (this._iContainerManager != null)
                {
                    ContainerCollection containers = this._iContainerManager.GetAll(callerContext, false);

                    if (containers != null && containers.Count > 0)
                    {
                        containerIds = containers.GetContainerIds();
                    }
                }

                #endregion

                #region Invalidate Impacted Data

                if (containerIds != null && containerIds.Count > 0)
                {
                    MappingBufferManager bufferManager = new MappingBufferManager();

                    foreach (RelationshipTypeEntityTypeMappingCardinality cardinality in cardinalities)
                    {
                        bufferManager.RemoveRelationshipCardinalities(containerIds, cardinality.RelationshipTypeId, cardinality.EntityTypeId);
                    }
                }

                #endregion
            }
        }

        #endregion

        #endregion

        #endregion
    }
}