using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Transactions;

namespace MDM.DataModelManager.Business
{
    using MDM.ActivityLogManager.Business;
    using MDM.BufferManager;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.DataModel;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.DataModelManager.Data;
    using MDM.Interfaces;
    using MDM.MessageManager.Business;
    using MDM.Utility;
   

    /// <summary>
    /// Represents business logic for Container EntityType mapping
    /// </summary>
    public class ContainerEntityTypeMappingBL : BusinessLogicBase, IDataModelManager, IContainerEntityTypeMappingManager
    {
        #region Fields

        /// <summary>
        /// Field denoting the security principal
        /// </summary>
        private SecurityPrincipal _securityPrincipal;

        /// <summary>
        /// Data access layer for Container EntityType Mapping
        /// </summary>
        private ContainerEntityTypeMappingDA _containerEntityTypeMappingDA = new ContainerEntityTypeMappingDA();

        /// <summary>
        /// Field denoting locale Message
        /// </summary>
        private LocaleMessage _localeMessage = new LocaleMessage();

        /// <summary>
        /// Field denoting localeMessageBL
        /// </summary>
        private LocaleMessageBL _localeMessageBL = new LocaleMessageBL();

        /// <summary>
        /// Field denoting Mapping Buffer Manager
        /// </summary>
        private MappingBufferManager _mappingBufferManager = new MappingBufferManager();

        /// <summary>
        /// Field denoting utility object
        /// </summary>
        private Utility _utility = new Utility();

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

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ContainerEntityTypeMappingBL.
        /// </summary>
        /// <param name="iOrganizationManager">Represents the reference of OrganizationBL.</param>
        /// <param name="iContainerManager">Represents the reference of ContainerBL.</param>
        /// <param name="iEntityTypeManager">Represents the reference of EntityTypeBL.</param>
        public ContainerEntityTypeMappingBL()
        {
            this._iOrganizationManager = ServiceLocator.Current.GetInstance(typeof(IOrganizationManager)) as IOrganizationManager;
            this._iContainerManager = ServiceLocator.Current.GetInstance(typeof(IContainerManager)) as IContainerManager;
            this._iEntityTypeManager = ServiceLocator.Current.GetInstance(typeof(IEntityTypeManager)) as IEntityTypeManager;
            GetSecurityPrincipal();
        }

        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        #region Get Methods

        /// <summary>
        /// Gets all Container EntityType mappings from the system
        /// </summary>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>All Container Entity type mappings</returns>
        public ContainerEntityTypeMappingCollection GetAll(CallerContext callerContext)
        {
            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("ContainerEntityTypeMappingBL.GetAll", MDMTraceSource.DataModel, false);

            ContainerEntityTypeMappingCollection containerEntityTypeMappings = null;
            try
            {
                containerEntityTypeMappings = Get(-1, -1, -1);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerEntityTypeMappingBL.GetAll", MDMTraceSource.DataModel);

            }
            return containerEntityTypeMappings;
        }

        /// <summary>
        /// Get container - entity type mapping by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ContainerEntityTypeMapping GetById(Int32 id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets Container EntityType mappings from the system based on containerId
        /// </summary>
        /// <param name="containerId">Indicates the container Id for which Container EntityType mappings needs to be fetched</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Container EntityType mappings for a specified container Id</returns>
        public ContainerEntityTypeMappingCollection GetMappingsByContainerId(Int32 containerId, CallerContext callerContext)
        {
            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("ContainerEntityTypeMappingBL.GetMappingsByContainerId", MDMTraceSource.DataModel, false);

            ContainerEntityTypeMappingCollection containerEntityTypeMappings = null;
            try
            {
                containerEntityTypeMappings = Get(-1, containerId, -1);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerEntityTypeMappingBL.GetMappingsByContainerId", MDMTraceSource.DataModel);

            }
            return containerEntityTypeMappings;
        }

        /// <summary>
        /// Gets Container EntityType mappings from the system based on EntityTypeId
        /// </summary>
        /// <param name="entityTypeId">Indicates the EntityType Id for which Container EntityType mappings needs to be fetched</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Container EntityType mappings for a specified EntityType Id</returns>
        public ContainerEntityTypeMappingCollection GetMappingsByEntityTypeId(Int32 entityTypeId, CallerContext callerContext)
        {
            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("ContainerEntityTypeMappingBL.GetMappingsByEntityTypeId", MDMTraceSource.DataModel, false);

            ContainerEntityTypeMappingCollection containerEntityTypeMappings = null;
            try
            {
                containerEntityTypeMappings = Get(-1, -1, entityTypeId);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerEntityTypeMappingBL.GetMappingsByEntityTypeId", MDMTraceSource.DataModel);

            }
            return containerEntityTypeMappings;
        }

        /// <summary>
        /// Gets mapped EntityTypes from the system based on containerId
        /// </summary>
        /// <param name="containerId">Indicates the container Id for which mapped EntityTypes needs to be fetched</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Mapped EntityTypes for a specified container Id</returns>
        public EntityTypeCollection GetMappedEntityTypes(Int32 containerId, CallerContext callerContext)
        {
            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("ContainerEntityTypeMappingBL.GetMappedEntityTypes", MDMTraceSource.DataModel, false);

            ContainerEntityTypeMappingCollection containerEntityTypeMappings = new ContainerEntityTypeMappingCollection();
            EntityTypeCollection entityTypes = new EntityTypeCollection();
            Collection<Int32> entityTypeIds = new Collection<Int32>();
            EntityTypeBL entityTypeBL = new EntityTypeBL();
            try
            {
                containerEntityTypeMappings = Get(-1, containerId, -1);

                if (containerEntityTypeMappings.Count > 0)
                {
                    foreach (ContainerEntityTypeMapping mapping in containerEntityTypeMappings)
                    {
                        Int32 entityTypeId = mapping.EntityTypeId;

                        if (entityTypeId > 0 && !entityTypeIds.Contains(entityTypeId))
                        {
                            entityTypeIds.Add(entityTypeId);
                        }
                    }

                    entityTypes = entityTypeBL.GetEntityTypesByIds(entityTypeIds);
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerEntityTypeMappingBL.GetMappedEntityTypes", MDMTraceSource.DataModel);
            }

            return entityTypes;
        }

        #endregion

        #region CUD Methods

        /// <summary>
        /// Create, Update or Delete Container EntityType Mappings
        /// </summary>
        /// <param name="ContainerEntityTypeMappingCollection">ContainerEntityTypeMappings to process</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public OperationResult Process(ContainerEntityTypeMappingCollection containerEntityTypeMappings, CallerContext callerContext)
        {
            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("ContainerEntityTypeMappingBL.Process", MDMTraceSource.DataModel, false);

            OperationResult operationResult = new OperationResult();
            operationResult.OperationResultStatus = OperationResultStatusEnum.Failed;

            ValidateInputParameters(containerEntityTypeMappings, callerContext);

            ValidateData(containerEntityTypeMappings, callerContext);
            String userName = _securityPrincipal.CurrentUserName;

            try
            {
                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
                {
                   
                    PopulateProgramName(callerContext);
                    _containerEntityTypeMappingDA.Process(containerEntityTypeMappings, userName, callerContext.ProgramName);

                    transactionScope.Complete();
                    operationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                }

                #region Remove from cache
                var containerEntityTypeBufferManager = new ContainerEntityTypeMappingBufferManager();
                containerEntityTypeBufferManager.RemoveContainerEntityTypeMappings();
                #endregion

                #region activity log

                if (operationResult.OperationResultStatus == OperationResultStatusEnum.Successful && AppConfigurationHelper.GetAppConfig<Boolean>(Constants.ACTIVITYLOG_DATAMODEL_ENABLED_APPCONFIG_KEY, false))
                {
                    LogDataModelChanges(containerEntityTypeMappings, callerContext);
                }

                #endregion activity log

                InvalidateImpactedCache();
            }
            catch(Exception ex)
            {
                String errorMessage = String.Format("Fatal error occurred while processing container entity type mappings. Please retry the operation. Contact system administrator, if error persists. Internal Error: {0}", ex.Message);
                operationResult.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerEntityTypeMappingBL.Process", MDMTraceSource.DataModel);
            }

            return operationResult;
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
                    MDMTraceHelper.StartTraceActivity("ContainerEntityTypeMappingBL.PrepareOperationResultsSchema", MDMTraceSource.DataModel, false);

                String separator = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.RSDataModelExcel.Separator", "//").Trim();
                ContainerEntityTypeMappingCollection containerEntityTypeMappings = iDataModelObjects as ContainerEntityTypeMappingCollection;

                if (containerEntityTypeMappings != null && containerEntityTypeMappings.Count > 0)
                {
                    if (operationResults.Count > 0)
                    {
                        operationResults.Clear();
                    }

                    Int32 containerEntityTypeMappingIdToBeCreated = -1;

                    foreach (ContainerEntityTypeMapping containerEntityTypeMapping in containerEntityTypeMappings)
                    {
                        DataModelOperationResult containerEntityTypeMappingOperationResult = new DataModelOperationResult(containerEntityTypeMapping.Id, containerEntityTypeMapping.LongName, containerEntityTypeMapping.ExternalId, containerEntityTypeMapping.ReferenceId);

                        if (String.IsNullOrEmpty(containerEntityTypeMappingOperationResult.ExternalId))
                        {
                            containerEntityTypeMappingOperationResult.ExternalId = String.Format("{1} {0} {2} {0} {3}", separator, containerEntityTypeMapping.OrganizationName, containerEntityTypeMapping.ContainerName, containerEntityTypeMapping.EntityTypeName);
                        }

                        if (containerEntityTypeMapping.Id < 1)
                        {
                            containerEntityTypeMapping.Id = containerEntityTypeMappingIdToBeCreated;
                            containerEntityTypeMappingOperationResult.Id = containerEntityTypeMappingIdToBeCreated;
                            containerEntityTypeMappingIdToBeCreated--;
                        }

                        operationResults.Add(containerEntityTypeMappingOperationResult);
                    }
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerEntityTypeMappingBL.PrepareOperationResultsSchema", MDMTraceSource.DataModel);
            }
        }

        /// <summary>
        /// Validates DataModelObjects and populate OperationResults
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be filled up.</param>
        /// <param name="operationResults">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Indicates the context of the caller.</param>
        /// <returns>DataModel Operation Result Collection</returns>
        public void Validate(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            try
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StartTraceActivity("ContainerEntityTypeMappingBL.Validate", MDMTraceSource.DataModel, false);
                }

                ContainerEntityTypeMappingCollection containerEntityTypeMappings = iDataModelObjects as ContainerEntityTypeMappingCollection;
                CallerContext callerContext = iCallerContext as CallerContext;
                
                #region Input Parameters Validations

                ValidateInputParameters(containerEntityTypeMappings, operationResults, callerContext);

                #endregion Input Parameter Validations

                #region Data Validations

                ValidateData(containerEntityTypeMappings, operationResults, callerContext);
               
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
                    MDMTraceHelper.StopTraceActivity("ContainerEntityTypeMappingBL.Validate", MDMTraceSource.DataModel);
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
                    MDMTraceHelper.StartTraceActivity("ContainerEntityTypeMappingBL.LoadOriginal", MDMTraceSource.DataModel, false);

                LoadOriginalContainerEntityTypeMappings(iDataModelObjects as ContainerEntityTypeMappingCollection, iCallerContext as CallerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerEntityTypeMappingBL.LoadOriginal", MDMTraceSource.DataModel);
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
                    MDMTraceHelper.StartTraceActivity("ContainerEntityTypeMappingBL.FillDataModel", MDMTraceSource.DataModel, false);

                FillContainerEntityTypeMappings(iDataModelObjects as ContainerEntityTypeMappingCollection, iCallerContext as CallerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerEntityTypeMappingBL.FillDataModel", MDMTraceSource.DataModel);
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
                    MDMTraceHelper.StartTraceActivity("ContainerEntityTypeMappingBL.CompareAndMerge", MDMTraceSource.DataModel, false);

                CompareAndMerge(iDataModelObjects as ContainerEntityTypeMappingCollection, operationResults, iCallerContext as CallerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerEntityTypeMappingBL.CompareAndMerge", MDMTraceSource.DataModel);
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
                    MDMTraceHelper.StartTraceActivity("ContainerEntityTypeMappingBL.Process", MDMTraceSource.DataModel, false);

                ContainerEntityTypeMappingCollection containerEntityTypeMappings = iDataModelObjects as ContainerEntityTypeMappingCollection;
                CallerContext callerContext = iCallerContext as CallerContext;

                if (containerEntityTypeMappings.Count > 0)
                {
                    PopulateProgramName(callerContext);

                    DBCommandProperties command = DBCommandHelper.Get((CallerContext)iCallerContext, MDMCenterModuleAction.Execute);

                    #region Perform containerEntityTypeMapping updates in database

                    using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                    {
                        _containerEntityTypeMappingDA.Process(containerEntityTypeMappings, operationResults, _securityPrincipal.CurrentUserName, callerContext.ProgramName);

                        transactionScope.Complete();
                    }

                    LocalizeErrors(callerContext, operationResults);

                    #endregion
                    #region Remove from cache
                    var containerEntityTypeBufferManager = new ContainerEntityTypeMappingBufferManager();
                    containerEntityTypeBufferManager.RemoveContainerEntityTypeMappings();
                    #endregion

                    #region activity log

                    if (operationResults.OperationResultStatus == OperationResultStatusEnum.Successful && AppConfigurationHelper.GetAppConfig<Boolean>(Constants.ACTIVITYLOG_DATAMODEL_ENABLED_APPCONFIG_KEY, false))
                    {
                        LogDataModelChanges(containerEntityTypeMappings, callerContext);
                    }

                    #endregion activity log

                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerEntityTypeMappingBL.Process", MDMTraceSource.DataModel);
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
            InvalidateImpactedCache();
        }

        #endregion

        #endregion

        #region Private Methods

        #region Validation Methods

        /// <summary>
        /// Validate the input parameters in container entity type mapping collection
        /// </summary>
        /// <param name="containerEntityTypeMappings">Indicates the collection of container entity type mapping</param>
        /// <param name="callerContext">Indicates the context of the caller</param>
        private void ValidateInputParameters(ContainerEntityTypeMappingCollection containerEntityTypeMappings, CallerContext callerContext)
        {
            String errorMessage;

            if (containerEntityTypeMappings == null || containerEntityTypeMappings.Count < 1)
            {
                errorMessage = "Container entity type mapping collection is not available or empty";
                throw new MDMOperationException("113643", errorMessage, "DataModelManager.ContainerEntityTypeMappingBL", String.Empty, "Create");
            }

            if (callerContext == null)
            {
                errorMessage = "CallerContext cannot be null.";
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "DataModelManager.ContainerEntityTypeMappingBL", String.Empty, "Create");
            }
        }

        /// <summary>
        /// Validate the input parameters in container entity type mapping collection
        /// </summary>
        /// <param name="containerEntityTypeMappings">Indicates the collection of container entity type mapping</param>
        /// <param name="operationResults">Indicates the result of operation</param>
        /// <param name="callerContext">Indicates the context of the caller</param>
        private void ValidateInputParameters(ContainerEntityTypeMappingCollection containerEntityTypeMappings, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            if (containerEntityTypeMappings == null || containerEntityTypeMappings.Count < 1)
            {
                DataModelHelper.AddOperationResults(operationResults, "113643", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }

            if (callerContext == null)
            {
                DataModelHelper.AddOperationResults(operationResults, "111846", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
        }

        /// <summary>
        /// Validate the data value in container entity type mapping collection.
        /// </summary>
        /// <param name="containerEntityTypeMappings">Indicates the collection of container entity type mapping</param>
        /// <param name="callerContext">Indicates the caller context containing the application and module specifying the caller.</param>
        private void ValidateData(ContainerEntityTypeMappingCollection containerEntityTypeMappings, CallerContext callerContext)
        {
            if (containerEntityTypeMappings != null && containerEntityTypeMappings.Count > 0)
            {
                ContainerCollection containers = (ContainerCollection)_iContainerManager.GetAll(callerContext, true);
                foreach (ContainerEntityTypeMapping containerEntityTypeMapping in containerEntityTypeMappings)
                {
                    String errorMessage = String.Empty;

                    Container container = containers.GetContainer(containerEntityTypeMapping.ContainerName, containerEntityTypeMapping.OrganizationName);

                    //This can be null in case of all container scenario.
                    if (container != null && (container.ContainerType == ContainerType.MasterCollaboration || container.ContainerType == ContainerType.Upstream))
                    {
                        if (containerEntityTypeMapping.MinimumExtensions != 0 || containerEntityTypeMapping.MaximumExtensions != 0)
                        {
                            errorMessage = "Minimum and Maximum Extensions cannot be set for upstream and master collaboration container type.";
                            throw new MDMOperationException("114278", errorMessage, "DataModelManager.ContainerEntityTypeMappingBL", String.Empty, "Create");
                        }
                    }

                    if (containerEntityTypeMapping.MinimumExtensions < 0)
                    {
                        errorMessage = "Minimum Extensions must be an integer value greater than or equal to 0.";
                        throw new MDMOperationException("114146", errorMessage, "DataModelManager.ContainerEntityTypeMappingBL", String.Empty, "Create");
                    }

                    if (containerEntityTypeMapping.MaximumExtensions < 0)
                    {
                        errorMessage = "Maximum Extensions must be an integer value greater than or equal to 0.";
                        throw new MDMOperationException("114149", errorMessage, "DataModelManager.ContainerEntityTypeMappingBL", String.Empty, "Create");
                    }

                    if (containerEntityTypeMapping.MaximumExtensions > 0 && containerEntityTypeMapping.MaximumExtensions < containerEntityTypeMapping.MinimumExtensions)
                    {
                        errorMessage = "Maximum Extensions value must be greater than or equal to Minimum Extensions value.";
                        throw new MDMOperationException("114147", errorMessage, "DataModelManager.ContainerEntityTypeMappingBL", String.Empty, "Create");
                    }
                }
            }
        }

        /// <summary>
        /// Validate the data of specified parameters in container entity type mapping collection
        /// </summary>
        /// <param name="containerEntityTypeMappings">Indicates the collection of container entity type mapping</param>
        /// <param name="operationResults">Indicates the result of operation</param>
        /// <param name="callerContext">Indicates the context of the caller</param>
        private void ValidateData(ContainerEntityTypeMappingCollection containerEntityTypeMappings, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            if (containerEntityTypeMappings != null && containerEntityTypeMappings.Count > 0)
            {
                Collection<String> containerEntityTypeMappingList = new Collection<String>();

                foreach (ContainerEntityTypeMapping containerEntityTypeMapping in containerEntityTypeMappings)
                {
                    Collection<String> uniqueIdentifierList = new Collection<String>() {containerEntityTypeMapping.OrganizationName, 
                                                                                        containerEntityTypeMapping.ContainerName, 
                                                                                        containerEntityTypeMapping.EntityTypeName};

                    String rowUniqueIdentifier = DataModelHelper.GetUniqueIdentifier(uniqueIdentifierList);

                    IDataModelOperationResult dataModelOperationResult = operationResults.GetByReferenceId(containerEntityTypeMapping.ReferenceId);

                    DataModelHelper.ValidateOrganizationName(containerEntityTypeMapping.OrganizationName, dataModelOperationResult, callerContext);
                    DataModelHelper.ValidateContainerName(containerEntityTypeMapping.ContainerName, dataModelOperationResult, callerContext);
                    DataModelHelper.ValidateEntityTypeName(containerEntityTypeMapping.EntityTypeName, dataModelOperationResult, callerContext);

                    if (containerEntityTypeMappingList.Contains(rowUniqueIdentifier))
                    {
                        Object[] parameters = null;
                        parameters = new Object[] { containerEntityTypeMapping.OrganizationName, 
                                                        containerEntityTypeMapping.ContainerName, 
                                                        containerEntityTypeMapping.EntityTypeName };

                        String errorMessage = String.Format("Duplicate mappings found for the container entity type mapping with organization: {0}, container:{1}, and entity type: {2}", parameters);

                        DataModelHelper.AddOperationResult(dataModelOperationResult, "113967", errorMessage, parameters, OperationResultType.Error, 
                                                            TraceEventType.Error, callerContext);
                    }
                    else
                    {
                        containerEntityTypeMappingList.Add(rowUniqueIdentifier);
                    }

                    ContainerCollection containers = this._iContainerManager.GetAll(callerContext, true);
                    Container container = containers.GetContainer(containerEntityTypeMapping.ContainerName, containerEntityTypeMapping.OrganizationName);

                    if (container != null && (container.ContainerType == ContainerType.MasterCollaboration || container.ContainerType == ContainerType.Upstream))
                    {
                        if (containerEntityTypeMapping.MinimumExtensions > 0 || containerEntityTypeMapping.MaximumExtensions > 0)
                        {
                            containerEntityTypeMapping.MinimumExtensions = 0;
                            containerEntityTypeMapping.MaximumExtensions = 0;
                            String errorMessage = "Minimum and Maximum Extensions cannot be set for upstream and master collaboration container type.";
                            DataModelHelper.AddOperationResult(dataModelOperationResult, "114278", errorMessage, null, OperationResultType.Warning, TraceEventType.Error, callerContext);
                        }
                    }

                    if (containerEntityTypeMapping.MinimumExtensions < 0)
                    {
                        Object[] parameters = new Object[] { containerEntityTypeMapping.OrganizationName, 
                                                        containerEntityTypeMapping.ContainerName, 
                                                        containerEntityTypeMapping.EntityTypeName };

                        String errorMessage = String.Format("Minimum Extensions must be an integer value greater than or equal to 0 for the container entity type mapping with organization: {0}, container: {1} and entity type: {2}.", parameters);
                        DataModelHelper.AddOperationResult(dataModelOperationResult, "114148", errorMessage, parameters, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }

                    if (containerEntityTypeMapping.MaximumExtensions < 0)
                    {
                        Object[] parameters = new Object[] { containerEntityTypeMapping.OrganizationName, 
                                                        containerEntityTypeMapping.ContainerName, 
                                                        containerEntityTypeMapping.EntityTypeName };

                        String errorMessage = String.Format("Maximum Extensions must be an integer value greater than or equal to 0 for the container entity type mapping with organization: {0}, container: {1} and entity type: {2}.", parameters);
                        DataModelHelper.AddOperationResult(dataModelOperationResult, "114143", errorMessage, parameters, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }

                    if (containerEntityTypeMapping.MaximumExtensions > 0 && containerEntityTypeMapping.MaximumExtensions < containerEntityTypeMapping.MinimumExtensions)
                    {
                        Object[] parameters = new Object[] { containerEntityTypeMapping.OrganizationName, 
                                                        containerEntityTypeMapping.ContainerName, 
                                                        containerEntityTypeMapping.EntityTypeName };

                        String errorMessage = String.Format("Maximum Extensions value must be greater than or equal to Minimum Extensions value for the container entity type mapping with organization: {0}, container: {1} and entity type: {2}.", parameters);
                        DataModelHelper.AddOperationResult(dataModelOperationResult, "114144", errorMessage, parameters, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                }
            }
        }

        #endregion

        #region Get Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        private ContainerEntityTypeMappingCollection Get(Int32 organizationId, Int32 containerId, Int32 entityTypeId)
        {
            ContainerEntityTypeMappingCollection allMappings = GetAllMappings();

            // Container entity type mapping at All container and All Organization level will have organization and container id as 0
            if (organizationId >= 0 || containerId >= 0 || entityTypeId > 0)
            {
                ContainerEntityTypeMappingCollection filteredContainerEntityTypeMappings = new ContainerEntityTypeMappingCollection();

                foreach (ContainerEntityTypeMapping mapping in allMappings)
                {
                    if (organizationId >= 0  && (mapping.OrganizationId != organizationId && mapping.OrganizationId != 0))
                    {
                        continue;
                    }

                    if (containerId >= 0 && (mapping.ContainerId != containerId && mapping.ContainerId != 0))
                    {
                        continue;
                    }

                    if (entityTypeId > 0 && mapping.EntityTypeId != entityTypeId)
                    {
                        continue;
                    }

                    filteredContainerEntityTypeMappings.Add(mapping);
                }

                return filteredContainerEntityTypeMappings;
            }
            else
            {
                return allMappings;
            }
        }

        private ContainerEntityTypeMappingCollection GetAllMappings()
        {
            ContainerEntityTypeMappingCollection containerEntityTypeMappings = null;

            var containerBufferManager = new ContainerEntityTypeMappingBufferManager();
            containerEntityTypeMappings = containerBufferManager.FindAllContainerEntityTypeMappings();

            // if the cache is empty
            if (containerEntityTypeMappings == null || containerEntityTypeMappings.Count == 0)
            {
                containerEntityTypeMappings = _containerEntityTypeMappingDA.Get(-1, -1, -1);

                // Store it back in cache
                if (containerEntityTypeMappings != null && containerEntityTypeMappings.Count > 0)
                {
                    containerBufferManager.UpdateContainerEntityTypeMappings(containerEntityTypeMappings, 3);
                }
            }

            return containerEntityTypeMappings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerEntityTypeMappingCollection"></param>
        /// <param name="callerContext"></param>
        private void LoadOriginalContainerEntityTypeMappings(ContainerEntityTypeMappingCollection containerEntityTypeMappings, CallerContext callerContext)
        {
            ContainerEntityTypeMappingCollection originalContainerEntityTypeMappings = GetAll(callerContext);

            if (originalContainerEntityTypeMappings != null && originalContainerEntityTypeMappings.Count > 0)
            {
                foreach (ContainerEntityTypeMapping containerEntityTypeMapping in containerEntityTypeMappings)
                {
                    containerEntityTypeMapping.OriginalContainerEntityTypeMapping = originalContainerEntityTypeMappings.Get(containerEntityTypeMapping.OrganizationName, containerEntityTypeMapping.ContainerName, containerEntityTypeMapping.EntityTypeName);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerEntityTypeMappingCollection"></param>
        /// <param name="callerContext"></param>
        private void FillContainerEntityTypeMappings(ContainerEntityTypeMappingCollection containerEntityTypeMappings, CallerContext callerContext)
        {
            OrganizationCollection organizations = new OrganizationCollection();
            ContainerCollection containers = new ContainerCollection();
            EntityTypeCollection entityTypes = new EntityTypeCollection();

            foreach (ContainerEntityTypeMapping containerEntityTypeMapping in containerEntityTypeMappings)
            {
                if (containerEntityTypeMapping.OriginalContainerEntityTypeMapping != null)
                {
                    containerEntityTypeMapping.Id = containerEntityTypeMapping.OriginalContainerEntityTypeMapping.Id;
                    containerEntityTypeMapping.OrganizationId = containerEntityTypeMapping.OriginalContainerEntityTypeMapping.OrganizationId;
                    containerEntityTypeMapping.ContainerId = containerEntityTypeMapping.OriginalContainerEntityTypeMapping.ContainerId;
                    containerEntityTypeMapping.EntityTypeId = containerEntityTypeMapping.OriginalContainerEntityTypeMapping.EntityTypeId;
                }
                else
                {
                    containerEntityTypeMapping.OrganizationId = DataModelHelper.GetOrganizationId(this._iOrganizationManager, containerEntityTypeMapping.OrganizationName, ref organizations, callerContext);
                    containerEntityTypeMapping.ContainerId = DataModelHelper.GetContainerId(this._iContainerManager, containerEntityTypeMapping.ContainerName, containerEntityTypeMapping.OrganizationName, ref containers, callerContext);
                    containerEntityTypeMapping.EntityTypeId = DataModelHelper.GetEntityTypeId(this._iEntityTypeManager, containerEntityTypeMapping.EntityTypeName, ref entityTypes, callerContext);
                }
            }
        }

        #endregion

        #region Process Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerEntityTypeMappings"></param>
        /// <param name="callerContext"></param>
        private void CompareAndMerge(ContainerEntityTypeMappingCollection containerEntityTypeMappings, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            foreach (ContainerEntityTypeMapping deltaContainerEntityTypeMapping in containerEntityTypeMappings)
            {
                IDataModelOperationResult operationResult = operationResults.GetByReferenceId(deltaContainerEntityTypeMapping.ReferenceId);

                //If delta object Action is Read/Ignore then skip Merge Delta Process.
                if (deltaContainerEntityTypeMapping.Action == ObjectAction.Read || deltaContainerEntityTypeMapping.Action == ObjectAction.Ignore)
                    continue;

                IContainerEntityTypeMapping originalContainerEntityTypeMapping = deltaContainerEntityTypeMapping.OriginalContainerEntityTypeMapping;

                if (originalContainerEntityTypeMapping != null)
                {
                    //If delta object Action is Delete then skip Merge Delta process.
                    if (deltaContainerEntityTypeMapping.Action != ObjectAction.Delete)
                    {
                        originalContainerEntityTypeMapping.MergeDelta(deltaContainerEntityTypeMapping, callerContext, false);
                    }
                }
                else
                {
                    if (deltaContainerEntityTypeMapping.Action == ObjectAction.Delete)
                    {
                        DataModelHelper.AddOperationResult(operationResult, "113652", String.Empty, new Object[] { deltaContainerEntityTypeMapping.OrganizationName, deltaContainerEntityTypeMapping.ContainerName, deltaContainerEntityTypeMapping.EntityTypeName }, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                    else
                    {
                        List<String> parameters = new List<String>();
                        List<String> errorMessages = new List<String>();

                        if (deltaContainerEntityTypeMapping.OrganizationId < 1)
                        {
                            parameters.Add(deltaContainerEntityTypeMapping.OrganizationName);
                            errorMessages.Add(DataModelHelper.GetLocaleMessageText("100754", String.Empty, null, callerContext)); // 100754 - Organization
                        }

                        if (deltaContainerEntityTypeMapping.ContainerId < 1)
                        {
                            parameters.Add(deltaContainerEntityTypeMapping.ContainerName);
                            errorMessages.Add(DataModelHelper.GetLocaleMessageText("113958", String.Empty, null, callerContext)); // 113958 - Container Name
                        }

                        if (deltaContainerEntityTypeMapping.EntityTypeId < 1)
                        {
                            parameters.Add(deltaContainerEntityTypeMapping.EntityTypeName);
                            errorMessages.Add(DataModelHelper.GetLocaleMessageText("100415", String.Empty, null, callerContext)); // 100415 - Entity Type
                        }

                        if (errorMessages.Count > 0)
                        {
                            DataModelHelper.AddInvalidNamesErrorsToOperationResult(operationResult, errorMessages, parameters, callerContext);
                        }

                        //If original object is not found then set Action as Create always.
                        deltaContainerEntityTypeMapping.Action = ObjectAction.Create;
                    }
                }
                operationResult.PerformedAction = deltaContainerEntityTypeMapping.Action;
            }
        }

        #endregion

        #region Misc. Methods

        private void InvalidateImpactedCache()
        {
            _mappingBufferManager.NotifyModelDataChange(CacheKeyContants.ATTRIBUTE_MODELS_CHANGED_KEY);
            _mappingBufferManager.NotifyModelDataChange(CacheKeyContants.COMMON_ATTRIBUTE_CHANGED_KEY);
            _mappingBufferManager.NotifyModelDataChange(CacheKeyContants.CONTAINER_ENTITYTYPE_MAPPING_CHANGED_KEY);
        }

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
                callerContext.ProgramName = "ContainerEntityTypeMappingBL";
            }
        }

        /// <summary>
        /// Log changes to tb_datamodelactivitylog if DA call is sucessfull and appconfig key datamodelactivitylog.enable == true
        /// </summary>
        /// <param name="containerEntityTypeMappingCollection"></param>
        /// <param name="callerContext"></param>
        private void LogDataModelChanges( ContainerEntityTypeMappingCollection containerEntityTypeMappingCollection, CallerContext callerContext)
        {
                #region step: populate datamodelactivitylog object

                DataModelActivityLogBL dataModelActivityLogManager = new DataModelActivityLogBL();
                DataModelActivityLogCollection activityLogCollection = dataModelActivityLogManager.FillDataModelActivityLogCollection(containerEntityTypeMappingCollection);

                #endregion step: populate datamodelactivitylog object

                #region step: make api call

                if (activityLogCollection != null) // null activitylog collection means there was error in mapping
                {
                    dataModelActivityLogManager.Process(activityLogCollection, callerContext);
                }

                #endregion step: make api call
        }


        #endregion

        #endregion

        #endregion
    }
}