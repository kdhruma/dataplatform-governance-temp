using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Transactions;
using System.Collections.ObjectModel;

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
    using MDM.ConfigurationManager.Business;
    using MDM.BusinessObjects.Interfaces;
    using MDM.CacheManager.Business;

    /// <summary>
    /// 
    /// </summary>
    public class ContainerRelationshipTypeEntityTypeMappingCardinalityBL : BusinessLogicBase, IDataModelManager
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
        /// Field denoting the reference of OrganizationBL.
        /// </summary>
        public IOrganizationManager _iOrganizationManager = null;

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

        /// <summary>
        /// ContainerRelationshipTypeEntityTypeMappingCardinality data access object
        /// </summary>
        private ContainerRelationshipTypeEntityTypeMappingCardinalityDA _containerRelationshipTypeEntityTypeMappingCardinalityDA = new ContainerRelationshipTypeEntityTypeMappingCardinalityDA();

        /// <summary>
        /// Indicates ui locale of a system
        /// </summary>
        private LocaleEnum _systemUILocale = LocaleEnum.UnKnown;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public ContainerRelationshipTypeEntityTypeMappingCardinalityBL()
        {
           InitializeMembers();
        }

        /// <summary>
        /// Initializes a new instance of the ContainerRelationshipTypeEntityTypeMappingCardinalityBL.
        /// </summary>
        /// <param name="iOrganizationManager">Represents the reference of OrganizationBL.</param>
        /// <param name="iContainerManager">Represents the reference of ContainerBL.</param>
        /// <param name="iEntityTypeManager">Represents the reference of EntityTypeBL.</param>
        /// <param name="iRelationshipTypeManager">Represents the reference of RelationshipTypeBL.</param>
        public ContainerRelationshipTypeEntityTypeMappingCardinalityBL(IOrganizationManager iOrganizationManager, IContainerManager iContainerManager, IEntityTypeManager iEntityTypeManager, IRelationshipTypeManager iRelationshipTypeManager)
        {
            this._iOrganizationManager = iOrganizationManager;
            this._iContainerManager = iContainerManager;
            this._iEntityTypeManager = iEntityTypeManager;
            this._iRelationshipTypeManager = iRelationshipTypeManager;

            InitializeMembers();
        }

        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        #region Get Methods

        /// <summary>
        /// Gets all Container RelationshipType EntityType Mappings Cardinality from the system
        /// </summary>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>All Container RelationshipType EntityType Mappings cardinality</returns>
        public ContainerRelationshipTypeEntityTypeMappingCardinalityCollection GetAll(CallerContext callerContext)
        {
            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("ContainerRelationshipTypeEntityTypeMappingCardinalityBL.GetAll", MDMTraceSource.DataModel, false);

            ContainerRelationshipTypeEntityTypeMappingCardinalityCollection containerRelationshipTypeEntityTypeMappingCardinalitys = new ContainerRelationshipTypeEntityTypeMappingCardinalityCollection();

            try
            {
                containerRelationshipTypeEntityTypeMappingCardinalitys = _containerRelationshipTypeEntityTypeMappingCardinalityDA.Get(-1, -1, -1);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerRelationshipTypeEntityTypeMappingCardinalityBL.GetAll", MDMTraceSource.DataModel);
            }

            return containerRelationshipTypeEntityTypeMappingCardinalitys;
        }

        /// <summary>
        /// Gets Container RelationshipType EntityType Mappings Cardinality from the system based on ContainerId,  RelationshipType Id and From EntityType Id
        /// </summary>
        /// <param name="containerId">Indicates the ContainerId for which mappings needs to be fetched</param>
        /// <param name="fromEntityTypeId">Indicates the From EntityType Id for which mappings needs to be fetched</param>
        /// <param name="relationshipTypeId">Indicates the RelationshipType Id for which mappings needs to be fetched</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Container RelationshipType EntityType Mappings cardinality for specified ContainerId, RelationshipType Id and From EntityType Id</returns>
        public ContainerRelationshipTypeEntityTypeMappingCardinalityCollection Get(Int32 containerId, Int32 fromEntityTypeId, Int32 relationshipTypeId, CallerContext callerContext)
        {
            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("ContainerRelationshipTypeEntityTypeMappingCardinalityBL.Get", MDMTraceSource.DataModel, false);

            ContainerRelationshipTypeEntityTypeMappingCardinalityCollection containerRelationshipTypeEntityTypeMappingCardinalitys = new ContainerRelationshipTypeEntityTypeMappingCardinalityCollection();
            try
            {
                containerRelationshipTypeEntityTypeMappingCardinalitys = _containerRelationshipTypeEntityTypeMappingCardinalityDA.Get(containerId, fromEntityTypeId, relationshipTypeId);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerRelationshipTypeEntityTypeMappingCardinalityBL.Get", MDMTraceSource.DataModel);
            }
            return containerRelationshipTypeEntityTypeMappingCardinalitys;
        }

        /// <summary>
        /// Gets RelationshipType EntityType Mappings Cardinality from the system based on RelationshipType Id
        /// </summary>
        /// <param name="containerId">Indicates the ContainerId for which mappings needs to be fetched</param>
        /// <param name="relationshipTypeId">Indicates the RelationshipType Id for which mappings needs to be fetched</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Container RelationshipType EntityType Mappings cardinality for specified RelationshipType Id</returns>
        public ContainerRelationshipTypeEntityTypeMappingCardinalityCollection GetByRelationshipTypeId(Int32 containerId, Int32 relationshipTypeId, CallerContext callerContext)
        {
            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("ContainerRelationshipTypeEntityTypeMappingCardinalityBL.Get", MDMTraceSource.DataModel, false);

            ContainerRelationshipTypeEntityTypeMappingCardinalityCollection containerRelationshipTypeEntityTypeMappingCardinalitys = new ContainerRelationshipTypeEntityTypeMappingCardinalityCollection();
            try
            {
                containerRelationshipTypeEntityTypeMappingCardinalitys = _containerRelationshipTypeEntityTypeMappingCardinalityDA.Get(containerId, -1, relationshipTypeId);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerRelationshipTypeEntityTypeMappingCardinalityBL.Get", MDMTraceSource.DataModel);
            }
            return containerRelationshipTypeEntityTypeMappingCardinalitys;
        }

        /// <summary>
        /// Gets Container RelationshipType EntityType Mappings Cardinality from the system based on containerId and From EntityType Id
        /// </summary>
        /// <param name="containerId">Indicates the ContainerId for which mappings needs to be fetched</param>
        /// <param name="fromEntityTypeId">Indicates the From EntityType Id for which mappings needs to be fetched</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Container RelationshipType EntityType Mappings cardinality for specified Container Id and From EntityType Id</returns>
        public ContainerRelationshipTypeEntityTypeMappingCardinalityCollection GetByFromEntityTypeId(Int32 containerId, Int32 fromEntityTypeId, CallerContext callerContext)
        {
            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("ContainerRelationshipTypeEntityTypeMappingCardinalityBL.Get", MDMTraceSource.DataModel, false);

            ContainerRelationshipTypeEntityTypeMappingCardinalityCollection containerRelationshipTypeEntityTypeMappingCardinalitys = new ContainerRelationshipTypeEntityTypeMappingCardinalityCollection();
            try
            {
                containerRelationshipTypeEntityTypeMappingCardinalitys = _containerRelationshipTypeEntityTypeMappingCardinalityDA.Get(containerId, fromEntityTypeId, -1);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerRelationshipTypeEntityTypeMappingCardinalityBL.Get", MDMTraceSource.DataModel);
            }
            return containerRelationshipTypeEntityTypeMappingCardinalitys;
        }
        #endregion

        #region CUD Methods

        /// <summary>
        /// Add new ContainerRelationshipTypeEntityTypeMappingCardinality
        /// </summary>
        /// <param name="containerRelationshipTypeEntityTypeMappingCardinality"></param>
        /// <param name="callerContext"></param>
        /// <returns>OperationResult</returns>
        public OperationResult Create(ContainerRelationshipTypeEntityTypeMappingCardinality containerRelationshipTypeEntityTypeMappingCardinality, CallerContext callerContext)
        {
            containerRelationshipTypeEntityTypeMappingCardinality.Action = ObjectAction.Create;
            ContainerRelationshipTypeEntityTypeMappingCardinalityCollection containerRelationshipTypeEntityTypeMappingCardinalitys = new ContainerRelationshipTypeEntityTypeMappingCardinalityCollection();
            containerRelationshipTypeEntityTypeMappingCardinalitys.Add(containerRelationshipTypeEntityTypeMappingCardinality);
            return GetOperationResultFromCollection(Process(containerRelationshipTypeEntityTypeMappingCardinalitys, callerContext));
        }

        /// <summary>
        /// Update existing ContainerRelationshipTypeEntityTypeMappingCardinality
        /// </summary>
        /// <param name="containerRelationshipTypeEntityTypeMappingCardinality"></param>
        /// <param name="callerContext"></param>
        /// <returns>OperationResult</returns>
        public OperationResult Update(ContainerRelationshipTypeEntityTypeMappingCardinality containerRelationshipTypeEntityTypeMappingCardinality, CallerContext callerContext)
        {
            containerRelationshipTypeEntityTypeMappingCardinality.Action = ObjectAction.Update;
            ContainerRelationshipTypeEntityTypeMappingCardinalityCollection containerRelationshipTypeEntityTypeMappingCardinalitys = new ContainerRelationshipTypeEntityTypeMappingCardinalityCollection();
            containerRelationshipTypeEntityTypeMappingCardinalitys.Add(containerRelationshipTypeEntityTypeMappingCardinality);
            return GetOperationResultFromCollection(Process(containerRelationshipTypeEntityTypeMappingCardinalitys, callerContext));
        }

        /// <summary>
        /// Remove existing ContainerRelationshipTypeEntityTypeMappingCardinality
        /// </summary>
        /// <param name="containerRelationshipTypeEntityTypeMappingCardinality"></param>
        /// <param name="callerContext"></param>
        /// <returns>OperationResult</returns>
        public OperationResult Delete(ContainerRelationshipTypeEntityTypeMappingCardinality containerRelationshipTypeEntityTypeMappingCardinality, CallerContext callerContext)
        {
            containerRelationshipTypeEntityTypeMappingCardinality.Action = ObjectAction.Delete;
            ContainerRelationshipTypeEntityTypeMappingCardinalityCollection containerRelationshipTypeEntityTypeMappingCardinalitys = new ContainerRelationshipTypeEntityTypeMappingCardinalityCollection();
            containerRelationshipTypeEntityTypeMappingCardinalitys.Add(containerRelationshipTypeEntityTypeMappingCardinality);
            return GetOperationResultFromCollection(Process(containerRelationshipTypeEntityTypeMappingCardinalitys, callerContext));
        }

        /// <summary>
        /// Create, Update or Delete ContainerRelationshipTypeEntityTypeMappingCardinalityCollection
        /// </summary>
        /// <param name="containerRelationshipTypeEntityTypeMappingCardinalitys">Container RelationshipType EntityType mappings to process</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation Collection</returns>
        public OperationResultCollection Process(ContainerRelationshipTypeEntityTypeMappingCardinalityCollection containerRelationshipTypeEntityTypeMappingCardinality, CallerContext callerContext)
        {
            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("ContainerRelationshipTypeEntityTypeMappingCardinalityBL.Process", MDMTraceSource.DataModel, false);

            OperationResultCollection operationResults = null;

            try
            {
                ValidateInputParameters(containerRelationshipTypeEntityTypeMappingCardinality, callerContext);

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    PopulateProgramName(callerContext);
                    operationResults = _containerRelationshipTypeEntityTypeMappingCardinalityDA.Process(containerRelationshipTypeEntityTypeMappingCardinality, _securityPrincipal.CurrentUserName, callerContext.ProgramName);
                    transactionScope.Complete();
                }

                operationResults.RefreshOperationResultStatus();

                if (operationResults.OperationResultStatus != OperationResultStatusEnum.Failed)
                {
                    InvalidateCache(containerRelationshipTypeEntityTypeMappingCardinality);
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerRelationshipTypeEntityTypeMappingCardinalityBL.Process", MDMTraceSource.DataModel);
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
                    MDMTraceHelper.StartTraceActivity("ContainerRelationshipTypeEntityTypeMappingCardinalityBL.PrepareOperationResultsSchema", MDMTraceSource.DataModel, false);

                String separator = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.RSDataModelExcel.Separator", "//").Trim();

                ContainerRelationshipTypeEntityTypeMappingCardinalityCollection containerRelationshipTypeEntityTypeMappingCardinalitys = iDataModelObjects as ContainerRelationshipTypeEntityTypeMappingCardinalityCollection;

                if (containerRelationshipTypeEntityTypeMappingCardinalitys != null && containerRelationshipTypeEntityTypeMappingCardinalitys.Count > 0)
                {
                    if (operationResults.Count > 0)
                    {
                        operationResults.Clear();
                    }

                    Int32 containerRelationshipTypeEntityTypeMappingCardinalityIdToBeCreated = -1;

                    foreach (ContainerRelationshipTypeEntityTypeMappingCardinality containerRelationshipTypeEntityTypeMappingCardinality in containerRelationshipTypeEntityTypeMappingCardinalitys)
                    {
                        DataModelOperationResult containerRelationshipTypeEntityTypeMappingCardinalityOperationResult = new DataModelOperationResult(containerRelationshipTypeEntityTypeMappingCardinality.Id, containerRelationshipTypeEntityTypeMappingCardinality.ExternalId, containerRelationshipTypeEntityTypeMappingCardinality.ExternalId, containerRelationshipTypeEntityTypeMappingCardinality.ReferenceId);

                        if (String.IsNullOrEmpty(containerRelationshipTypeEntityTypeMappingCardinalityOperationResult.ExternalId))
                        {
                            containerRelationshipTypeEntityTypeMappingCardinalityOperationResult.ExternalId = String.Format("{1} {0} {2} {0} {3} {0} {4} {0} (5)", separator, containerRelationshipTypeEntityTypeMappingCardinality.OrganizationName, containerRelationshipTypeEntityTypeMappingCardinality.ContainerName, containerRelationshipTypeEntityTypeMappingCardinality.RelationshipTypeName, containerRelationshipTypeEntityTypeMappingCardinality.EntityTypeName, containerRelationshipTypeEntityTypeMappingCardinality.ToEntityTypeName);
                        }

                        if (containerRelationshipTypeEntityTypeMappingCardinality.Id < 1)
                        {
                            containerRelationshipTypeEntityTypeMappingCardinality.Id = containerRelationshipTypeEntityTypeMappingCardinalityIdToBeCreated;
                            containerRelationshipTypeEntityTypeMappingCardinalityOperationResult.Id = containerRelationshipTypeEntityTypeMappingCardinalityIdToBeCreated;
                            containerRelationshipTypeEntityTypeMappingCardinalityIdToBeCreated--;
                        }

                        operationResults.Add(containerRelationshipTypeEntityTypeMappingCardinalityOperationResult);
                    }
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerRelationshipTypeEntityTypeMappingCardinalityBL.PrepareOperationResultsSchema", MDMTraceSource.DataModel);
            }
        }

        /// <summary>
        /// Validates DataModelObjects and populate OperationResults
        /// </summary>
        /// <param name="iDataModelObjects">Indicates the collection of data model objects to be filled up.</param>
        /// <param name="operationResults">Indicates the collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Indicates the context of the caller</param>
        /// <returns>DataModel Operation Result Collection</returns>
        public void Validate(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            try
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StartTraceActivity("ContainerRelationshipTypeEntityTypeMappingCardinalityBL.Validate", MDMTraceSource.DataModel, false);
                }

                ContainerRelationshipTypeEntityTypeMappingCardinalityCollection containerRelationshipTypeEntityTypeMappingCardinalities = iDataModelObjects as ContainerRelationshipTypeEntityTypeMappingCardinalityCollection;
                CallerContext callerContext = iCallerContext as CallerContext;

                #region Parameter Validations

                ValidateInputParameters(containerRelationshipTypeEntityTypeMappingCardinalities, operationResults, callerContext);

                #endregion

                #region Data Validations

                ValidateData(containerRelationshipTypeEntityTypeMappingCardinalities, operationResults, callerContext);

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
                    MDMTraceHelper.StopTraceActivity("ContainerRelationshipTypeEntityTypeMappingCardinalityBL.Validate", MDMTraceSource.DataModel);
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
                    MDMTraceHelper.StartTraceActivity("ContainerRelationshipTypeEntityTypeMappingCardinalityBL.LoadOriginal", MDMTraceSource.DataModel, false);

                LoadOriginalContainerRelationshipTypeEntityTypeMappingCardinality(iDataModelObjects as ContainerRelationshipTypeEntityTypeMappingCardinalityCollection, iCallerContext as CallerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerRelationshipTypeEntityTypeMappingCardinalityBL.LoadOriginal", MDMTraceSource.DataModel);
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
                    MDMTraceHelper.StartTraceActivity("ContainerRelationshipTypeEntityTypeMappingCardinalityBL.FillDataModel", MDMTraceSource.DataModel, false);

                FillContainerRelationshipTypeEntityTypeMappingCardinality(iDataModelObjects as ContainerRelationshipTypeEntityTypeMappingCardinalityCollection, iCallerContext as CallerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerRelationshipTypeEntityTypeMappingCardinalityBL.FillDataModel", MDMTraceSource.DataModel);
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
                    MDMTraceHelper.StartTraceActivity("ContainerRelationshipTypeEntityTypeMappingCardinalityBL.CompareAndMerge", MDMTraceSource.DataModel, false);

                CompareAndMerge(iDataModelObjects as ContainerRelationshipTypeEntityTypeMappingCardinalityCollection, operationResults, iCallerContext as CallerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerRelationshipTypeEntityTypeMappingCardinalityBL.CompareAndMerge", MDMTraceSource.DataModel);
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
                    MDMTraceHelper.StartTraceActivity("ContainerRelationshipTypeEntityTypeMappingCardinalityBL.Process", MDMTraceSource.DataModel, false);

                ContainerRelationshipTypeEntityTypeMappingCardinalityCollection containerRelationshipTypeEntityTypeMappingCardinalitys = iDataModelObjects as ContainerRelationshipTypeEntityTypeMappingCardinalityCollection;
                CallerContext callerContext = iCallerContext as CallerContext;

                if (containerRelationshipTypeEntityTypeMappingCardinalitys.Count > 0)
                {
                    #region Perform ContainerRelationshipTypeEntityTypeMappingCardinality updates in database

                    using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                    {
                        PopulateProgramName(callerContext);

                        _containerRelationshipTypeEntityTypeMappingCardinalityDA.Process(containerRelationshipTypeEntityTypeMappingCardinalitys, operationResults, _securityPrincipal.CurrentUserName, callerContext.ProgramName);

                        transactionScope.Complete();
                    }

                    LocalizeErrors(callerContext, operationResults);

                    InvalidateCache(containerRelationshipTypeEntityTypeMappingCardinalitys);
                    
                    #endregion
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerRelationshipTypeEntityTypeMappingCardinalityBL.Process", MDMTraceSource.DataModel);
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
                InvalidateCache(iDataModelObjects as ContainerRelationshipTypeEntityTypeMappingCardinalityCollection);
            }
        }

        #endregion

        #endregion

        #region Private Methods

        #region Validation Methods

        /// <summary>
        /// Validate the input parameters in container relationship type entity type mapping cardinality collection
        /// </summary>
        /// <param name="containerRelationshipTypeEntityTypeMappingCardinalitys">Indicates the collection of container relationship type entity type mapping cardinality</param>
        /// <param name="callerContext">Indicates the context of the caller</param>
        private void ValidateInputParameters(ContainerRelationshipTypeEntityTypeMappingCardinalityCollection containerRelationshipTypeEntityTypeMappingCardinalitys, CallerContext callerContext)
        {
            String errorMessage;

            if (containerRelationshipTypeEntityTypeMappingCardinalitys == null || containerRelationshipTypeEntityTypeMappingCardinalitys.Count < 1)
            {
                errorMessage = "ContainerRelationshipTypeEntityTypeMappingCardinality Collection cannot be null.";
                throw new MDMOperationException("", errorMessage, "DataModelManager.ContainerRelationshipTypeEntityTypeMappingCardinalityBL", String.Empty, "Create");
            }

            if (callerContext == null)
            {
                errorMessage = "CallerContext cannot be null.";
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "DataModelManager.ContainerRelationshipTypeEntityTypeMappingCardinalityBL", String.Empty, "Create");
            }
        }

        /// <summary>
        /// Validate the input parameters in container relationship type entity type mapping cardinality collection
        /// </summary>
        /// <param name="containerRelationshipTypeEntityTypeMappingCardinalities">Indicates the collection of container relationship type entity type mapping cardinality</param>
        /// <param name="operationResults">Indicates the result of operation</param>
        /// <param name="callerContext">Indicates the context of the caller</param>
        private void ValidateInputParameters(ContainerRelationshipTypeEntityTypeMappingCardinalityCollection containerRelationshipTypeEntityTypeMappingCardinalities, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            if (callerContext == null)
            {
                DataModelHelper.AddOperationResults(operationResults, "111846", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }

            if (containerRelationshipTypeEntityTypeMappingCardinalities == null || containerRelationshipTypeEntityTypeMappingCardinalities.Count < 1)
            {
                DataModelHelper.AddOperationResults(operationResults, "113606", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
        }

        /// <summary>
        /// Validate the data of specified parameters in container relationship type entity type mapping cardinality collection
        /// </summary>
        /// <param name="containerRelationshipTypeEntityTypeMappingCardinalities">Indicates the collection of container relationship type entity type mapping cardinality</param>
        /// <param name="operationResults">Indicates the result of operation</param>
        /// <param name="callerContext">Indicates the context of the caller</param>
        private void ValidateData(ContainerRelationshipTypeEntityTypeMappingCardinalityCollection containerRelationshipTypeEntityTypeMappingCardinalities, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            if (containerRelationshipTypeEntityTypeMappingCardinalities != null && containerRelationshipTypeEntityTypeMappingCardinalities.Count > 0)
            {
                Collection<String> relationshipTypeEntityTypeMappingList = new Collection<String>();

                foreach (ContainerRelationshipTypeEntityTypeMappingCardinality containerRelationshipTypeEntityTypeMappingCardinality in containerRelationshipTypeEntityTypeMappingCardinalities)
                {
                    Collection<String> uniqueIdentifierList = new Collection<String>() { containerRelationshipTypeEntityTypeMappingCardinality.OrganizationName, 
                                                                                            containerRelationshipTypeEntityTypeMappingCardinality.ContainerName, 
                                                                                            containerRelationshipTypeEntityTypeMappingCardinality.RelationshipTypeName, 
                                                                                            containerRelationshipTypeEntityTypeMappingCardinality.EntityTypeName, 
                                                                                            containerRelationshipTypeEntityTypeMappingCardinality.ToEntityTypeName };

                    String rowUniqueIdentifier = DataModelHelper.GetUniqueIdentifier(uniqueIdentifierList);

                    IDataModelOperationResult dataModelOperationResult = operationResults.GetByReferenceId(containerRelationshipTypeEntityTypeMappingCardinality.ReferenceId);

                    DataModelHelper.ValidateMinMaxRelationships(containerRelationshipTypeEntityTypeMappingCardinality.MinRelationships, 
                                                                containerRelationshipTypeEntityTypeMappingCardinality.MaxRelationships, 
                                                                dataModelOperationResult, callerContext);

                    DataModelHelper.ValidateOrganizationName(containerRelationshipTypeEntityTypeMappingCardinality.OrganizationName, 
                                                                dataModelOperationResult, callerContext);

                    DataModelHelper.ValidateContainerName(containerRelationshipTypeEntityTypeMappingCardinality.ContainerName, 
                                                                dataModelOperationResult, callerContext);

                    DataModelHelper.ValidateRelationshipTypeName(containerRelationshipTypeEntityTypeMappingCardinality.RelationshipTypeName, 
                                                                    dataModelOperationResult, callerContext);

                    DataModelHelper.ValidateEntityTypeName(containerRelationshipTypeEntityTypeMappingCardinality.EntityTypeName, 
                                                                dataModelOperationResult, callerContext);

                    if (relationshipTypeEntityTypeMappingList.Contains(rowUniqueIdentifier))
                    {
                        Object[] parameters = null;
                        parameters = new Object[] { containerRelationshipTypeEntityTypeMappingCardinality.ContainerName, 
                                                        containerRelationshipTypeEntityTypeMappingCardinality.EntityTypeName, 
                                                        containerRelationshipTypeEntityTypeMappingCardinality.RelationshipTypeName };

                        String errorMessage = String.Format("Duplicate mappings found for the container relationship type entity type mapping cardinality with container: {0}, entity type: {1} and relationship type : {2}", parameters);

                        DataModelHelper.AddOperationResult(dataModelOperationResult, "113965", errorMessage, parameters, 
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
        /// <param name="containerRelationshipTypeEntityTypeMappingCardinalitys"></param>
        /// <param name="callerContext"></param>
        private void LoadOriginalContainerRelationshipTypeEntityTypeMappingCardinality(ContainerRelationshipTypeEntityTypeMappingCardinalityCollection containerRelationshipTypeEntityTypeMappingCardinalitys, CallerContext callerContext)
        {
            ContainerRelationshipTypeEntityTypeMappingCardinalityCollection orginalContainerRelationshipTypeEntityTypeMappingCardinalityCollection = GetAll(callerContext);

            if (orginalContainerRelationshipTypeEntityTypeMappingCardinalityCollection != null && orginalContainerRelationshipTypeEntityTypeMappingCardinalityCollection.Count > 0)
            {
                foreach (ContainerRelationshipTypeEntityTypeMappingCardinality containerRelationshipTypeEntityTypeMappingCardinality in containerRelationshipTypeEntityTypeMappingCardinalitys)
                {
                    containerRelationshipTypeEntityTypeMappingCardinality.OriginalContainerRelationshipTypeEntityTypeMappingCardinality = orginalContainerRelationshipTypeEntityTypeMappingCardinalityCollection.Get(containerRelationshipTypeEntityTypeMappingCardinality.OrganizationName, containerRelationshipTypeEntityTypeMappingCardinality.ContainerName, containerRelationshipTypeEntityTypeMappingCardinality.EntityTypeName, containerRelationshipTypeEntityTypeMappingCardinality.RelationshipTypeName, containerRelationshipTypeEntityTypeMappingCardinality.ToEntityTypeName);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerRelationshipTypeEntityTypeMappingCardinalitys"></param>
        /// <param name="callerContext"></param>
        private void FillContainerRelationshipTypeEntityTypeMappingCardinality(ContainerRelationshipTypeEntityTypeMappingCardinalityCollection containerRelationshipTypeEntityTypeMappingCardinalitys, CallerContext callerContext)
        {
            OrganizationCollection organizations = new OrganizationCollection();
            ContainerCollection containers = new ContainerCollection();
            EntityTypeCollection entityTypes = new EntityTypeCollection();
            RelationshipTypeCollection relationshipTypes = new RelationshipTypeCollection();
            ContainerRelationshipTypeEntityTypeMappingCollection containerRelationshipTypeEntityTypeMappings = new ContainerRelationshipTypeEntityTypeMappingCollection();

            foreach (ContainerRelationshipTypeEntityTypeMappingCardinality containerRelationshipTypeEntityTypeMappingCardinality in containerRelationshipTypeEntityTypeMappingCardinalitys)
            {
                if (containerRelationshipTypeEntityTypeMappingCardinality.OriginalContainerRelationshipTypeEntityTypeMappingCardinality != null)
                {
                    containerRelationshipTypeEntityTypeMappingCardinality.Id = containerRelationshipTypeEntityTypeMappingCardinality.OriginalContainerRelationshipTypeEntityTypeMappingCardinality.Id;

                    containerRelationshipTypeEntityTypeMappingCardinality.OrganizationId = containerRelationshipTypeEntityTypeMappingCardinality.OriginalContainerRelationshipTypeEntityTypeMappingCardinality.OrganizationId;
                    containerRelationshipTypeEntityTypeMappingCardinality.ContainerId = containerRelationshipTypeEntityTypeMappingCardinality.OriginalContainerRelationshipTypeEntityTypeMappingCardinality.ContainerId;
                    containerRelationshipTypeEntityTypeMappingCardinality.EntityTypeId = containerRelationshipTypeEntityTypeMappingCardinality.OriginalContainerRelationshipTypeEntityTypeMappingCardinality.EntityTypeId;
                    containerRelationshipTypeEntityTypeMappingCardinality.RelationshipTypeId = containerRelationshipTypeEntityTypeMappingCardinality.OriginalContainerRelationshipTypeEntityTypeMappingCardinality.RelationshipTypeId;

                    containerRelationshipTypeEntityTypeMappingCardinality.ToEntityTypeId = containerRelationshipTypeEntityTypeMappingCardinality.OriginalContainerRelationshipTypeEntityTypeMappingCardinality.ToEntityTypeId;
                    containerRelationshipTypeEntityTypeMappingCardinality.ContainerRelationshipTypeEntityTypeId = containerRelationshipTypeEntityTypeMappingCardinality.OriginalContainerRelationshipTypeEntityTypeMappingCardinality.ContainerRelationshipTypeEntityTypeId;
                }
                else
                {
                    containerRelationshipTypeEntityTypeMappingCardinality.OrganizationId = DataModelHelper.GetOrganizationId(this._iOrganizationManager, containerRelationshipTypeEntityTypeMappingCardinality.OrganizationName, ref organizations, callerContext);
                    containerRelationshipTypeEntityTypeMappingCardinality.ContainerId = DataModelHelper.GetContainerId(this._iContainerManager, containerRelationshipTypeEntityTypeMappingCardinality.ContainerName, containerRelationshipTypeEntityTypeMappingCardinality.OrganizationName, ref containers, callerContext);
                    containerRelationshipTypeEntityTypeMappingCardinality.EntityTypeId = DataModelHelper.GetEntityTypeId(this._iEntityTypeManager, containerRelationshipTypeEntityTypeMappingCardinality.EntityTypeName, ref entityTypes, callerContext);
                    containerRelationshipTypeEntityTypeMappingCardinality.RelationshipTypeId = DataModelHelper.GetRelationshipTypeId(this._iRelationshipTypeManager, containerRelationshipTypeEntityTypeMappingCardinality.RelationshipTypeName, ref relationshipTypes, callerContext);

                    containerRelationshipTypeEntityTypeMappingCardinality.ToEntityTypeId = DataModelHelper.GetEntityTypeId(this._iEntityTypeManager, containerRelationshipTypeEntityTypeMappingCardinality.ToEntityTypeName, ref entityTypes, callerContext);
                    containerRelationshipTypeEntityTypeMappingCardinality.ContainerRelationshipTypeEntityTypeId = DataModelHelper.GetContainerRelationshipTypeEntityTypeId(containerRelationshipTypeEntityTypeMappingCardinality.OrganizationName, containerRelationshipTypeEntityTypeMappingCardinality.ContainerName, containerRelationshipTypeEntityTypeMappingCardinality.EntityTypeName, containerRelationshipTypeEntityTypeMappingCardinality.RelationshipTypeName, ref containerRelationshipTypeEntityTypeMappings, callerContext);
                }
            }
        }

        #endregion

        #region Process Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerRelationshipTypeEntityTypeMappingCardinalitys"></param>
        /// <param name="operationResults"></param>
        /// <param name="callerContext"></param>
        private void CompareAndMerge(ContainerRelationshipTypeEntityTypeMappingCardinalityCollection containerRelationshipTypeEntityTypeMappingCardinalitys, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            foreach (ContainerRelationshipTypeEntityTypeMappingCardinality deltaContainerRelationshipTypeEntityTypeMappingCardinality in containerRelationshipTypeEntityTypeMappingCardinalitys)
            {
                IDataModelOperationResult operationResult = operationResults.GetByReferenceId(deltaContainerRelationshipTypeEntityTypeMappingCardinality.ReferenceId);

                //If delta object Action is Read/Ignore then skip Merge Delta Process.
                if (deltaContainerRelationshipTypeEntityTypeMappingCardinality.Action == ObjectAction.Read || deltaContainerRelationshipTypeEntityTypeMappingCardinality.Action == ObjectAction.Ignore)
                    continue;

                IContainerRelationshipTypeEntityTypeMappingCardinality originalContainerRelationshipTypeEntityTypeMappingCardinality = deltaContainerRelationshipTypeEntityTypeMappingCardinality.OriginalContainerRelationshipTypeEntityTypeMappingCardinality;

                if (originalContainerRelationshipTypeEntityTypeMappingCardinality != null)
                {
                    //If delta object Action is Delete then skip Merge Delta process.
                    if (deltaContainerRelationshipTypeEntityTypeMappingCardinality.Action != ObjectAction.Delete)
                    {
                        originalContainerRelationshipTypeEntityTypeMappingCardinality.MergeDelta(deltaContainerRelationshipTypeEntityTypeMappingCardinality, callerContext, false);
                    }
                }
                else
                {
                    Object[] parameters = null;
                    if (deltaContainerRelationshipTypeEntityTypeMappingCardinality.Action == ObjectAction.Delete)
                    {
                        parameters = new Object[] { "", deltaContainerRelationshipTypeEntityTypeMappingCardinality.ContainerName, deltaContainerRelationshipTypeEntityTypeMappingCardinality.EntityTypeName, deltaContainerRelationshipTypeEntityTypeMappingCardinality.RelationshipTypeName };
                        DataModelHelper.AddOperationResult(operationResult, "113607", String.Empty, parameters, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                    else
                    {
                        //If original object is not found then set Action as Create always.
                        if (deltaContainerRelationshipTypeEntityTypeMappingCardinality.ContainerId < 1 || deltaContainerRelationshipTypeEntityTypeMappingCardinality.EntityTypeId < 1 || deltaContainerRelationshipTypeEntityTypeMappingCardinality.RelationshipTypeId < 1 || deltaContainerRelationshipTypeEntityTypeMappingCardinality.ToEntityTypeId < 1 || deltaContainerRelationshipTypeEntityTypeMappingCardinality.ContainerRelationshipTypeEntityTypeId < 1)
                        {
                            parameters = new Object[] { deltaContainerRelationshipTypeEntityTypeMappingCardinality.ContainerName, deltaContainerRelationshipTypeEntityTypeMappingCardinality.EntityTypeName, deltaContainerRelationshipTypeEntityTypeMappingCardinality.RelationshipTypeName, deltaContainerRelationshipTypeEntityTypeMappingCardinality.ToEntityTypeName };
                            DataModelHelper.AddOperationResult(operationResult, "113687", String.Empty, parameters, OperationResultType.Error, TraceEventType.Error, callerContext);
                        }
                        deltaContainerRelationshipTypeEntityTypeMappingCardinality.Action = ObjectAction.Create;
                    }
                    operationResult.PerformedAction = deltaContainerRelationshipTypeEntityTypeMappingCardinality.Action;
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
                callerContext.ProgramName = "ContainerRelationshipTypeEntityTypeMappingCardinalityBL";
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
        }

        /// <summary>
        /// Invalidate cache for each processed cardinality.
        /// </summary>
        /// <param name="cardinalities"></param>
        private void InvalidateCache(ContainerRelationshipTypeEntityTypeMappingCardinalityCollection cardinalities)
        {
            MappingBufferManager bufferManager = new MappingBufferManager();

            if (cardinalities != null)
            {
                foreach (ContainerRelationshipTypeEntityTypeMappingCardinality cardinality in cardinalities)
                {
                    bufferManager.RemoveRelationshipCardinalities(cardinality.RelationshipTypeId, cardinality.ContainerId, cardinality.EntityTypeId);
                }
            }

            bufferManager.NotifyModelDataChange(CacheKeyContants.RELATIONSHIP_HIERARCHY_CHANGED_KEY);
        }

        #endregion

        #endregion

        #endregion
    }
}