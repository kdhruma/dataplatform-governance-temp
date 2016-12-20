using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
    using MDM.CacheManager.Business;

    /// <summary>
    /// Represents business logic for Container EntityType Attribute mapping
    /// </summary>
    public class ContainerEntityTypeAttributeMappingBL : BusinessLogicBase, IDataModelManager
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
        /// Specifies mapping buffer manager
        /// </summary>
        private MappingBufferManager _mappingBufferManager = new MappingBufferManager();

        /// <summary>
        /// Field denoting the IOrganizationManager
        /// </summary>
        private IOrganizationManager _iOrganizationManager = null;

        /// <summary>
        /// Field denoting the IContainerManager
        /// </summary>
        private IContainerManager _iContainerManager = null;

        /// <summary>
        /// Field denoting the IEntityTypeManager
        /// </summary>
        private IEntityTypeManager _iEntityTypeManager = null;

        /// <summary>
        /// Field denoting the IAttributeModelManager
        /// </summary>
        private IAttributeModelManager _iAttributeModelManager = null;

        /// <summary>
        /// ContainerEntityTypeAttributeMapping data access object
        /// </summary>
        private ContainerEntityTypeAttributeMappingDA _containerEntityTypeAttributeMappingDA = new ContainerEntityTypeAttributeMappingDA();

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for ContainerEntityTypeAttributeMappingBL
        /// </summary>
        public ContainerEntityTypeAttributeMappingBL()
        {
            GetSecurityPrincipal();
        }

        /// <summary>
        /// Constructor for ContainerEntityTypeAttributeMappingBL
        /// </summary>
        /// <param name="iOrganizationManager"></param>
        /// <param name="iContainerManager"></param>
        /// <param name="iEntityTypeManager"></param>
        /// <param name="iAttributeModelManager"></param>
        public ContainerEntityTypeAttributeMappingBL(IOrganizationManager iOrganizationManager, IContainerManager iContainerManager, IEntityTypeManager iEntityTypeManager, IAttributeModelManager iAttributeModelManager)
        {
            this._iOrganizationManager = iOrganizationManager;
            this._iContainerManager = iContainerManager;
            this._iEntityTypeManager = iEntityTypeManager;
            this._iAttributeModelManager = iAttributeModelManager;
            GetSecurityPrincipal();
        }

        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        #region Get Method

        /// <summary>
        /// Gets Container Entity Type Attribute Mappings from the system based on container Id, entityType Id , attributeGroup Id and attribute Id
        /// </summary>
        /// <param name="containerId">Indicates the container Id for which attribute mappings needs to be fetched</param>
        /// <param name="entityTypeId">Indicates the EntityType Id for which attribute mappings needs to be fetched</param>
        /// <param name="attributeGroupId">Indicates the container Id for which attribute mappings needs to be fetched</param>
        /// <param name="attributeId">Indicates the attribute Id for which attribute mappings needs to be fetched</param>
        /// <param name="CallerContext">Context which called the application</param>
        /// <returns>Container EntityType Attribute Mappings for a specified ContainerId, EntityTypeId, AttributeId and AttributeGroupId</returns>
        public ContainerEntityTypeAttributeMappingCollection Get(Int32 containerId, Int32 entityTypeId, Int32 attributeGroupId, Int32 attributeId, CallerContext callerContext)
        {
            ContainerEntityTypeAttributeMappingCollection containerEntityTypeAttributeMappings = new ContainerEntityTypeAttributeMappingCollection();

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("ContainerEntityTypeAttributeMappingBL.Get", MDMTraceSource.DataModel, false);

                containerEntityTypeAttributeMappings = GetContainerEntityTypeAttributeMappings(containerId, entityTypeId, attributeGroupId, attributeId, false, callerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerEntityTypeAttributeMappingBL.Get", MDMTraceSource.DataModel);
            }
            return containerEntityTypeAttributeMappings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="attributeGroupId"></param>
        /// <param name="attributeId"></param>
        /// <returns></returns>
        public ContainerEntityTypeAttributeMappingCollection GetContainerEntityTypeAttributeMappings(Int32 containerId, Int32 entityTypeId, Int32 attributeGroupId, Int32 attributeId, Boolean getLatest, CallerContext callerContext)
        {
            ContainerEntityTypeAttributeMappingCollection filteredContainerEntityTypeAttributeMappings = null;

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("ContainerEntityTypeAttributeMappingBL.GetContainerEntityTypeAttributeMappings", MDMTraceSource.DataModel, false);

                #region Initial Setup

                ContainerEntityTypeAttributeMappingCollection containerEntityTypeAttributeMappings = null;
                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

                #endregion

                #region Get Mappings from Cache if available

                if (!getLatest)
                {
                    containerEntityTypeAttributeMappings = _mappingBufferManager.FindContainerEntityTypeAttributeMappingsCompleteDetails(containerId, entityTypeId);
                }

                #endregion

                #region Get Mappings from Database

                if (containerEntityTypeAttributeMappings == null)
                {
                    containerEntityTypeAttributeMappings = _containerEntityTypeAttributeMappingDA.Get(containerId, entityTypeId, -1, -1, command);

                    #region Cache Mappings data

                    if (containerEntityTypeAttributeMappings != null && containerId > 0 && entityTypeId > 0)
                    {
                        _mappingBufferManager.UpdateContainerEntityTypeAttributeMappings(containerEntityTypeAttributeMappings, containerId, entityTypeId, 3);
                    }

                    #endregion
                }

                if (attributeGroupId > 0 && attributeId > 0)
                {
                    filteredContainerEntityTypeAttributeMappings = new ContainerEntityTypeAttributeMappingCollection();
                    filteredContainerEntityTypeAttributeMappings.Add(containerEntityTypeAttributeMappings.GetByAttributeIdAndGroupId(attributeId, attributeGroupId));
                }
                else if (attributeGroupId > 0)
                {
                    filteredContainerEntityTypeAttributeMappings = containerEntityTypeAttributeMappings.GetByAttributeGroupId(attributeGroupId);
                }
                else if (attributeId > 0)
                {
                    filteredContainerEntityTypeAttributeMappings = new ContainerEntityTypeAttributeMappingCollection();
                    filteredContainerEntityTypeAttributeMappings.Add(containerEntityTypeAttributeMappings.GetByAttributeId(attributeId));
                }
                else
                {
                    filteredContainerEntityTypeAttributeMappings = containerEntityTypeAttributeMappings;
                }

                #endregion
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerEntityTypeAttributeMappingBL.GetContainerEntityTypeAttributeMappings", MDMTraceSource.DataModel);
            }

            return filteredContainerEntityTypeAttributeMappings;
        }

        #endregion

        #region CUD Method

        /// <summary>
        /// Create, Update or delete Container EntityType Attribute Mappings
        /// </summary>
        /// <param name="containerEntityTypeAttributeMappings">ContainerEntityTypeAttributeMappings to process</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public OperationResult Process(ContainerEntityTypeAttributeMappingCollection containerEntityTypeAttributeMappings, CallerContext callerContext)
        {
            OperationResult operationResult = new OperationResult();
            operationResult.OperationResultStatus = OperationResultStatusEnum.Failed;

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("ContainerEntityTypeAttributeMappingBL.Process", MDMTraceSource.DataModel, false);

                if (containerEntityTypeAttributeMappings != null && containerEntityTypeAttributeMappings.Count > 0)
                {
                    IEnumerable<Int32> attributeIds = containerEntityTypeAttributeMappings.Where(mapping => mapping.Action != ObjectAction.Delete).Select(mapping => mapping.AttributeId);
                    AttributeModelCollection baseAttributeModels = _iAttributeModelManager.GetAllBaseAttributeModels();
                    AttributeModelCollection attributeModelsForMapping = new AttributeModelCollection();
                    foreach (AttributeModel baseAttributeModel in baseAttributeModels)
                    {
                        if (attributeIds.Contains(baseAttributeModel.Id))
                        {
                            attributeModelsForMapping.Add(baseAttributeModel);
                        }
                    }

                    ValidateMappingProperties(containerEntityTypeAttributeMappings, attributeModelsForMapping, operationResult);
                    DataModelHelper.ValidateComplexAttributes(attributeModelsForMapping, operationResult);
                    if (operationResult.HasError)
                    {
                        foreach (Error error in operationResult.Errors)
                        {
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, error.ErrorMessage);
                        }
                        return operationResult;
                    }

                    using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                    {
                        PopulateProgramName(callerContext);

                        _containerEntityTypeAttributeMappingDA.Process(containerEntityTypeAttributeMappings, _securityPrincipal.CurrentUserName, callerContext.ProgramName);

                        ProcessEntityCacheLoadContextForMappingChange(containerEntityTypeAttributeMappings);

                        transactionScope.Complete();

                        operationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                    }

                    #region activitylog

                    if (operationResult.OperationResultStatus == OperationResultStatusEnum.Successful && AppConfigurationHelper.GetAppConfig<Boolean>(Constants.ACTIVITYLOG_DATAMODEL_ENABLED_APPCONFIG_KEY, false))
                    {
                        LogDataModelChanges(containerEntityTypeAttributeMappings, callerContext);
                    }

                    #endregion activity log

                    #region Invalidate Container-EntityType Attribute Mappings

                    //Collect all container Ids and entity type ids.
                    Collection<Int32> containerIds = new Collection<Int32>();
                    Collection<Int32> entityTypeIds = new Collection<Int32>();

                    var enumerator = containerEntityTypeAttributeMappings.GetEnumerator();

                    while (enumerator.MoveNext())
                    {
                        Int32 containerId = enumerator.Current.ContainerId;
                        Int32 entityTypeId = enumerator.Current.EntityTypeId;

                        if (!containerIds.Contains(containerId))
                            containerIds.Add(containerId);

                        if (!entityTypeIds.Contains(entityTypeId))
                            entityTypeIds.Add(entityTypeId);
                    }

                    _mappingBufferManager.RemoveContainerEntityTypeAttributeMappings(containerIds, entityTypeIds, true);

                    #endregion

                    _mappingBufferManager.NotifyModelDataChange(CacheKeyContants.COMMON_ATTRIBUTE_CHANGED_KEY);
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerEntityTypeAttributeMappingBL.Process", MDMTraceSource.DataModel);
            }

            return operationResult;
        }

        #endregion

        #region IDataModelManager Methods

        /// <summary>
        /// Generate and OperationResult Schema for given DataModelObjectCollection
        /// </summary>
        /// <param name="iDataModelObjects">Container entity type attribute mapping object</param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        public void PrepareOperationResultsSchema(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults)
        {
            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("ContainerEntityTypeAttributeMappingBL.PrepareOperationResultsSchema", MDMTraceSource.DataModel, false);

                String separator = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.RSDataModelExcel.Separator", "//").Trim();
                ContainerEntityTypeAttributeMappingCollection containerEntityTypeAttributeMappings = iDataModelObjects as ContainerEntityTypeAttributeMappingCollection;

                if (containerEntityTypeAttributeMappings != null && containerEntityTypeAttributeMappings.Count > 0)
                {
                    if (operationResults.Count > 0)
                    {
                        operationResults.Clear();
                    }

                    int containerEntityTypeAttributeMappingIdToBeCreated = -1;

                    foreach (ContainerEntityTypeAttributeMapping containerEntityTypeAttributeMapping in containerEntityTypeAttributeMappings)
                    {
                        DataModelOperationResult containerEntityTypeAttributeMappingOperationResult = new DataModelOperationResult(containerEntityTypeAttributeMapping.Id, containerEntityTypeAttributeMapping.LongName, containerEntityTypeAttributeMapping.ExternalId, containerEntityTypeAttributeMapping.ReferenceId);

                        if (String.IsNullOrEmpty(containerEntityTypeAttributeMappingOperationResult.ExternalId))
                        {
                            containerEntityTypeAttributeMappingOperationResult.ExternalId = String.Format("{1} {0} {2} {0} {3} {0} {4} {0} {5}", separator, containerEntityTypeAttributeMapping.OrganizationName, containerEntityTypeAttributeMapping.ContainerName, containerEntityTypeAttributeMapping.EntityTypeName, containerEntityTypeAttributeMapping.AttributeParentName, containerEntityTypeAttributeMapping.AttributeName);
                        }

                        if (containerEntityTypeAttributeMapping.Id < 1)
                        {
                            containerEntityTypeAttributeMapping.Id = containerEntityTypeAttributeMappingIdToBeCreated;
                            containerEntityTypeAttributeMappingOperationResult.Id = containerEntityTypeAttributeMappingIdToBeCreated;
                            containerEntityTypeAttributeMappingIdToBeCreated--;
                        }

                        operationResults.Add(containerEntityTypeAttributeMappingOperationResult);
                    }
                }
            }
            catch (Exception ex)
            {
                DataModelHelper.AddOperationResults(operationResults, String.Empty, ex.Message, null, OperationResultType.Error, TraceEventType.Error, new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.DataModelImport));
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerEntityTypeAttributeMappingBL.PrepareOperationResultsSchema", MDMTraceSource.DataModel);
            }
        }

        /// <summary>
        /// Validates DataModelObjects and populate OperationResults
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be filled up.</param>
        /// <param name="operationResults">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Indicates the context of the caller</param>
        /// <returns>DataModel Operation Result Collection</returns>
        public void Validate(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            try
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StartTraceActivity("ContainerEntityTypeAttributeMappingBL.Validate", MDMTraceSource.DataModel, false);
                }

                ContainerEntityTypeAttributeMappingCollection containerEntityTypeAttributeMappings = iDataModelObjects as ContainerEntityTypeAttributeMappingCollection;
                CallerContext callerContext = iCallerContext as CallerContext;

                #region Parameter Validations

                ValidateInputParameters(containerEntityTypeAttributeMappings, operationResults, callerContext);

                #endregion

                #region Data Validations

                ValidateData(containerEntityTypeAttributeMappings, operationResults, callerContext);

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
                    MDMTraceHelper.StopTraceActivity("ContainerEntityTypeAttributeMappingBL.Validate", MDMTraceSource.DataModel);
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
                    MDMTraceHelper.StartTraceActivity("ContainerEntityTypeAttributeMappingBL.LoadOriginal", MDMTraceSource.DataModel, false);

                LoadOriginalContainerEntityTypeAttributeMapping(iDataModelObjects as ContainerEntityTypeAttributeMappingCollection, iCallerContext as CallerContext);
            }
            catch (Exception ex)
            {
                DataModelHelper.AddOperationResults(operationResults, String.Empty, ex.Message, null, OperationResultType.Error, TraceEventType.Error, iCallerContext as CallerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerEntityTypeAttributeMappingBL.LoadOriginal", MDMTraceSource.DataModel);
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
                    MDMTraceHelper.StartTraceActivity("ContainerEntityTypeAttributeMappingBL.FillDataModel", MDMTraceSource.DataModel, false);

                FillContainerEntityTypeAttributeMapping(iDataModelObjects as ContainerEntityTypeAttributeMappingCollection, iCallerContext as CallerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerEntityTypeAttributeMappingBL.FillDataModel", MDMTraceSource.DataModel);
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
                    MDMTraceHelper.StartTraceActivity("ContainerEntityTypeAttributeMappingBL.CompareAndMerge", MDMTraceSource.DataModel, false);

                CompareAndMerge(iDataModelObjects as ContainerEntityTypeAttributeMappingCollection, operationResults, iCallerContext as CallerContext);
            }
            catch (Exception ex)
            {
                DataModelHelper.AddOperationResults(operationResults, String.Empty, ex.Message, null, OperationResultType.Error, TraceEventType.Error, iCallerContext as CallerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerEntityTypeAttributeMappingBL.CompareAndMerge", MDMTraceSource.DataModel);
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
                    MDMTraceHelper.StartTraceActivity("ContainerEntityTypeAttributeMappingBL.Process", MDMTraceSource.DataModel, false);

                ContainerEntityTypeAttributeMappingCollection containerEntityTypeAttributeMappings = iDataModelObjects as ContainerEntityTypeAttributeMappingCollection;
                CallerContext callerContext = iCallerContext as CallerContext;

                if (containerEntityTypeAttributeMappings.Count > 0)
                {
                    PopulateProgramName(callerContext);

                    DBCommandProperties command = DBCommandHelper.Get((CallerContext)iCallerContext, MDMCenterModuleAction.Execute);

                    #region Perform ContainerEntityTypeAttributeMapping updates in database

                    using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                    {
                        _containerEntityTypeAttributeMappingDA.Process(containerEntityTypeAttributeMappings, operationResults, _securityPrincipal.CurrentUserName, callerContext.ProgramName);

                        transactionScope.Complete();
                    }

                    LocalizeErrors(callerContext, operationResults);

                    #endregion

                    #region activity log

                    if (operationResults.OperationResultStatus == OperationResultStatusEnum.Successful && AppConfigurationHelper.GetAppConfig<Boolean>(Constants.ACTIVITYLOG_DATAMODEL_ENABLED_APPCONFIG_KEY, false))
                    {
                        LogDataModelChanges(containerEntityTypeAttributeMappings, callerContext);
                    }

                    #endregion activity log

                }
            }
            catch (Exception ex)
            {
                DataModelHelper.AddOperationResults(operationResults, String.Empty, ex.Message, null, OperationResultType.Error, TraceEventType.Error, iCallerContext as CallerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerEntityTypeAttributeMappingBL.Process", MDMTraceSource.DataModel);
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
            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("ContainerEntityTypeAttributeMappingBL.InvalidateCache", MDMTraceSource.DataModel, false);

                ContainerEntityTypeAttributeMappingCollection containerEntityTypeAttributeMappings = iDataModelObjects as ContainerEntityTypeAttributeMappingCollection;

                foreach (ContainerEntityTypeAttributeMapping containerEntityTypeAttributeMapping in containerEntityTypeAttributeMappings)
                {
                    // clear buffer manager
                    _mappingBufferManager.RemoveContainerEntityTypeAttributeMappings(containerEntityTypeAttributeMapping.ContainerId, containerEntityTypeAttributeMapping.EntityTypeId, false);
                }

                _mappingBufferManager.NotifyModelDataChange(CacheKeyContants.COMMON_ATTRIBUTE_CHANGED_KEY);
            }
            catch (Exception ex)
            {
                DataModelHelper.AddOperationResults(operationResults, String.Empty, ex.Message, null, OperationResultType.Error, TraceEventType.Error, iCallerContext as CallerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerEntityTypeAttributeMappingBL.InvalidateCache", MDMTraceSource.DataModel);
            }
        }

        #endregion

        #endregion

        #region Private Methods

        #region Validation Methods

        /// <summary>
        /// Validate the input parameters in container entity type attribute mapping collection
        /// </summary>
        /// <param name="containerEntityTypeAttributeMappings">Indicates the collection of container entity type attribute mapping</param>
        /// <param name="callerContext">Indicates the context of the caller</param>
        private void ValidateInputParameters(ContainerEntityTypeAttributeMappingCollection containerEntityTypeAttributeMappings, CallerContext callerContext)
        {
            String errorMessage;

            if (containerEntityTypeAttributeMappings == null || containerEntityTypeAttributeMappings.Count < 1)
            {
                errorMessage = "ContainerEntityTypeAttributeMapping Collection cannot be null.";
                throw new MDMOperationException("", errorMessage, "DataModelManager.ContainerEntityTypeAttributeMappingBL", String.Empty, "Create");
            }

            if (callerContext == null)
            {
                errorMessage = "CallerContext cannot be null.";
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "DataModelManager.ContainerEntityTypeAttributeMappingBL", String.Empty, "Create");
            }
        }

        /// <summary>
        /// Validate the input parameters in container entity type attribute mapping collection
        /// </summary>
        /// <param name="containerEntityTypeAttributeMappings">Indicates the collection of container entity type attribute mapping</param>
        /// <param name="operationResults">Indicates the result of operation</param>
        /// <param name="callerContext">Indicates the context of the caller</param>
        private void ValidateInputParameters(ContainerEntityTypeAttributeMappingCollection containerEntityTypeAttributeMappings, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            if (containerEntityTypeAttributeMappings == null || containerEntityTypeAttributeMappings.Count < 1)
            {
                DataModelHelper.AddOperationResults(operationResults, "113650", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }

            if (callerContext == null)
            {
                DataModelHelper.AddOperationResults(operationResults, "111846", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
        }

        /// <summary>
        /// Validate the data of specified parameters in container entity type attribute mapping collection
        /// </summary>
        /// <param name="containerEntityTypeAttributeMappings">Indicates the collection of container entity type attribute mapping</param>
        /// <param name="operationResults">Indicates the result of operation</param>
        /// <param name="callerContext">Indicates the context of the caller</param>
        private void ValidateData(ContainerEntityTypeAttributeMappingCollection containerEntityTypeAttributeMappings, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            if (containerEntityTypeAttributeMappings != null && containerEntityTypeAttributeMappings.Count > 0)
            {
                Collection<String> containerEntityTypeAttributeMappingList = new Collection<String>();
                AttributeModelCollection attributeModels = new AttributeModelCollection();

                foreach (ContainerEntityTypeAttributeMapping containerEntityTypeAttributeMapping in containerEntityTypeAttributeMappings)
                {
                    AttributeModel attributeModel = DataModelHelper.GetAttributeModel(_iAttributeModelManager, containerEntityTypeAttributeMapping.AttributeId, containerEntityTypeAttributeMapping.AttributeName, containerEntityTypeAttributeMapping.AttributeParentName, ref attributeModels, callerContext);
                    IDataModelOperationResult dataModelOperationResult = operationResults.GetByReferenceId(containerEntityTypeAttributeMapping.ReferenceId);

                    if (attributeModel != null && attributeModel.AttributeModelType != AttributeModelType.Common)
                    {
                        Object[] parameters = new Object[]
                        {
                            attributeModel.Name, attributeModel.AttributeParentName,
                            attributeModel.AttributeModelType.ToString().ToLowerInvariant(),
                            containerEntityTypeAttributeMapping.OrganizationName,
                            containerEntityTypeAttributeMapping.ContainerName,
                            containerEntityTypeAttributeMapping.EntityTypeName
                        };

                        String errorMessage =
                            "Attribute '{0}' under '{1}' group is of the type {2}, so mapping cannot be performed for organization name '{3}' , container name '{4}' and entity type '{5}'.";
                        DataModelHelper.AddOperationResult(dataModelOperationResult, "114129",
                            String.Format(errorMessage, parameters), parameters, OperationResultType.Error,
                            TraceEventType.Error, callerContext);
                    }
                    else
                    {
                        Collection<String> uniqueIdentifierList = new Collection<String>()
                        {
                            containerEntityTypeAttributeMapping.OrganizationName,
                            containerEntityTypeAttributeMapping.ContainerName,
                            containerEntityTypeAttributeMapping.EntityTypeName,
                            containerEntityTypeAttributeMapping.AttributeName,
                            containerEntityTypeAttributeMapping.AttributeParentName
                        };

                        String rowUniqueIdentifier = DataModelHelper.GetUniqueIdentifier(uniqueIdentifierList);


                        DataModelHelper.ValidateOrganizationName(containerEntityTypeAttributeMapping.OrganizationName,
                            dataModelOperationResult,
                            callerContext);

                        DataModelHelper.ValidateContainerName(containerEntityTypeAttributeMapping.ContainerName,
                            dataModelOperationResult,
                            callerContext);

                        DataModelHelper.ValidateEntityTypeName(containerEntityTypeAttributeMapping.EntityTypeName,
                            dataModelOperationResult,
                            callerContext);

                        DataModelHelper.ValidateAttributeUniqueIdentifier(
                            containerEntityTypeAttributeMapping.AttributeName,
                            containerEntityTypeAttributeMapping.AttributeParentName,
                            dataModelOperationResult, callerContext);

                        if (containerEntityTypeAttributeMappingList.Contains(rowUniqueIdentifier))
                        {
                            Object[] parameters = null;
                            parameters = new Object[]
                            {
                                containerEntityTypeAttributeMapping.OrganizationName,
                                containerEntityTypeAttributeMapping.ContainerName,
                                containerEntityTypeAttributeMapping.EntityTypeName,
                                containerEntityTypeAttributeMapping.AttributeName,
                                containerEntityTypeAttributeMapping.AttributeParentName
                            };

                            String errorMessage =
                                String.Format(
                                    "Duplicate mappings found for the container entity type attribute mapping with organization: {0}, container: {1}, entity type: {2}, attribute: {3}, and attribute parent: {4}",
                                    parameters);

                            DataModelHelper.AddOperationResult(dataModelOperationResult, "113962", errorMessage,
                                parameters,
                                OperationResultType.Error, TraceEventType.Error, callerContext);
                        }
                        else
                        {
                            containerEntityTypeAttributeMappingList.Add(rowUniqueIdentifier);
                        }

                        ValidateEntityTypeMappingDataModelProperties(containerEntityTypeAttributeMapping, attributeModel.Inheritable,
                            dataModelOperationResult, callerContext);
                    }
                }
            }
        }

        #endregion

        #region Get Methods


        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerEntityTypeAttributeMappings"></param>
        /// <param name="callerContext"></param>
        private void LoadOriginalContainerEntityTypeAttributeMapping(ContainerEntityTypeAttributeMappingCollection containerEntityTypeAttributeMappings, CallerContext callerContext)
        {
            ContainerEntityTypeAttributeMappingCollection orginalContainerEntityTypeAttributeMappingCollection = GetContainerEntityTypeAttributeMappings(-1, -1, -1, -1, true, callerContext);

            if (orginalContainerEntityTypeAttributeMappingCollection != null && orginalContainerEntityTypeAttributeMappingCollection.Count > 0)
            {
                foreach (ContainerEntityTypeAttributeMapping containerEntityTypeAttributeMapping in containerEntityTypeAttributeMappings)
                {
                    containerEntityTypeAttributeMapping.OriginalContainerEntityTypeAttributeMapping = orginalContainerEntityTypeAttributeMappingCollection.Get(containerEntityTypeAttributeMapping.OrganizationName, containerEntityTypeAttributeMapping.ContainerName, containerEntityTypeAttributeMapping.EntityTypeName, containerEntityTypeAttributeMapping.AttributeName, containerEntityTypeAttributeMapping.AttributeParentName);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerEntityTypeAttributeMappings"></param>
        /// <param name="callerContext"></param>
        private void FillContainerEntityTypeAttributeMapping(ContainerEntityTypeAttributeMappingCollection containerEntityTypeAttributeMappings, CallerContext callerContext)
        {
            OrganizationCollection organizations = new OrganizationCollection();
            ContainerCollection containers = new ContainerCollection();
            EntityTypeCollection entityTypes = new EntityTypeCollection();
            AttributeModelCollection attributeModels = new AttributeModelCollection();

            foreach (ContainerEntityTypeAttributeMapping containerEntityTypeAttributeMapping in containerEntityTypeAttributeMappings)
            {
                if (containerEntityTypeAttributeMapping.OriginalContainerEntityTypeAttributeMapping != null)
                {
                    containerEntityTypeAttributeMapping.Id = containerEntityTypeAttributeMapping.OriginalContainerEntityTypeAttributeMapping.Id;
                    containerEntityTypeAttributeMapping.OrganizationId = containerEntityTypeAttributeMapping.OriginalContainerEntityTypeAttributeMapping.OrganizationId;
                    containerEntityTypeAttributeMapping.ContainerId = containerEntityTypeAttributeMapping.OriginalContainerEntityTypeAttributeMapping.ContainerId;
                    containerEntityTypeAttributeMapping.EntityTypeId = containerEntityTypeAttributeMapping.OriginalContainerEntityTypeAttributeMapping.EntityTypeId;
                    containerEntityTypeAttributeMapping.AttributeId = containerEntityTypeAttributeMapping.OriginalContainerEntityTypeAttributeMapping.AttributeId;
                    containerEntityTypeAttributeMapping.AttributeParentId = containerEntityTypeAttributeMapping.OriginalContainerEntityTypeAttributeMapping.AttributeParentId;
                }
                else
                {
                    containerEntityTypeAttributeMapping.OrganizationId = DataModelHelper.GetOrganizationId(_iOrganizationManager, containerEntityTypeAttributeMapping.OrganizationName, ref organizations, callerContext);
                    containerEntityTypeAttributeMapping.ContainerId = DataModelHelper.GetContainerId(_iContainerManager, containerEntityTypeAttributeMapping.ContainerName, containerEntityTypeAttributeMapping.OrganizationName, ref containers, callerContext);
                    containerEntityTypeAttributeMapping.EntityTypeId = DataModelHelper.GetEntityTypeId(_iEntityTypeManager, containerEntityTypeAttributeMapping.EntityTypeName, ref entityTypes, callerContext);
                    AttributeModel attributeModel = DataModelHelper.GetAttributeModel(_iAttributeModelManager, containerEntityTypeAttributeMapping.AttributeId, containerEntityTypeAttributeMapping.AttributeName, containerEntityTypeAttributeMapping.AttributeParentName, ref attributeModels, callerContext);
                    if (attributeModel != null)
                    {
                        containerEntityTypeAttributeMapping.AttributeId = attributeModel.Id;
                        containerEntityTypeAttributeMapping.AttributeParentId = attributeModel.AttributeParentId;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerEntityTypeAttributeMappings"></param>
        /// <param name="operationResults"></param>
        /// <param name="callerContext"></param>
        private void CompareAndMerge(ContainerEntityTypeAttributeMappingCollection containerEntityTypeAttributeMappings, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            foreach (ContainerEntityTypeAttributeMapping deltaContainerEntityTypeAttributeMapping in containerEntityTypeAttributeMappings)
            {
                IDataModelOperationResult operationResult = operationResults.GetByReferenceId(deltaContainerEntityTypeAttributeMapping.ReferenceId);

                //If delta object Action is Read/Ignore then skip Merge Delta Process.
                if (deltaContainerEntityTypeAttributeMapping.Action == ObjectAction.Read || deltaContainerEntityTypeAttributeMapping.Action == ObjectAction.Ignore)
                    continue;

                IContainerEntityTypeAttributeMapping originalContainerEntityTypeAttributeMapping = deltaContainerEntityTypeAttributeMapping.OriginalContainerEntityTypeAttributeMapping;

                if (originalContainerEntityTypeAttributeMapping != null)
                {
                    //If delta object Action is Delete then skip Merge Delta process.
                    if (deltaContainerEntityTypeAttributeMapping.Action != ObjectAction.Delete)
                    {
                        originalContainerEntityTypeAttributeMapping.MergeDelta(deltaContainerEntityTypeAttributeMapping, callerContext, false);
                    }
                }
                else
                {
                    if (deltaContainerEntityTypeAttributeMapping.Action == ObjectAction.Delete)
                    {
                        Object[] parameters = new Object[] { deltaContainerEntityTypeAttributeMapping.OrganizationName, deltaContainerEntityTypeAttributeMapping.ContainerName, deltaContainerEntityTypeAttributeMapping.EntityTypeName, deltaContainerEntityTypeAttributeMapping.AttributeName, deltaContainerEntityTypeAttributeMapping.AttributeParentName };
                        DataModelHelper.AddOperationResult(operationResult, "113625", String.Empty, parameters, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                    else
                    {
                        List<String> parameters = new List<String>();
                        List<String> errorMessage = new List<String>();

                        if (deltaContainerEntityTypeAttributeMapping.OrganizationId < 1)
                        {
                            parameters.Add(deltaContainerEntityTypeAttributeMapping.OrganizationName);
                            errorMessage.Add(DataModelHelper.GetLocaleMessageText("113975", String.Empty, null, callerContext)); // 113975 - Organization Name
                        }
                        else if (deltaContainerEntityTypeAttributeMapping.ContainerId < 1)
                        {
                            parameters.Add(deltaContainerEntityTypeAttributeMapping.ContainerName);
                            errorMessage.Add(DataModelHelper.GetLocaleMessageText("113958", String.Empty, null, callerContext)); // 113958 - Container Name
                        }
                        if (deltaContainerEntityTypeAttributeMapping.EntityTypeId < 1)
                        {
                            parameters.Add(deltaContainerEntityTypeAttributeMapping.EntityTypeName);
                            errorMessage.Add(DataModelHelper.GetLocaleMessageText("100415", String.Empty, null, callerContext)); // 100415 - Entity Type
                        }
                        if (deltaContainerEntityTypeAttributeMapping.AttributeId < 1)
                        {
                            parameters.Add(deltaContainerEntityTypeAttributeMapping.AttributeName);
                            errorMessage.Add(DataModelHelper.GetLocaleMessageText("100163", String.Empty, null, callerContext)); // 100163 - Attribute Name
                        }
                        if (deltaContainerEntityTypeAttributeMapping.AttributeParentId < 1)
                        {
                            parameters.Add(deltaContainerEntityTypeAttributeMapping.AttributeParentName);
                            errorMessage.Add(DataModelHelper.GetLocaleMessageText("112803", String.Empty, null, callerContext)); // 112803 - Attribute Parent Name
                        }

                        if (errorMessage.Count > 0)
                        {
                            StringBuilder error = new StringBuilder();
                            error.Append(DataModelHelper.GetLocaleMessageText("112635", String.Empty, null, callerContext)); // 112635 - Invalid

                            for (Int32 i = 0; i < errorMessage.Count; i++)
                            {
                                error.Append(String.Format(" {0} : {{{1}}},", errorMessage[i], i));
                            }

                            DataModelHelper.AddOperationResult(operationResult, String.Empty, error.ToString().Trim(','), parameters.ToArray(), OperationResultType.Error, TraceEventType.Error, callerContext);
                        }

                        deltaContainerEntityTypeAttributeMapping.Action = ObjectAction.Create;
                    }
                }
                operationResult.PerformedAction = deltaContainerEntityTypeAttributeMapping.Action;
            }
        }

        #endregion

        #region EntityCacheLoadContext For Container EntityType Attribute Mapping

        /// <summary>
        /// Processes the EntityCacheLoadContext for entity type mapping change.
        /// </summary>
        private void ProcessEntityCacheLoadContextForMappingChange(ContainerEntityTypeAttributeMappingCollection containerEntityTypeAttributeMappings)
        {
            Collection<Tuple<Int32, Int32, Collection<Int32>>> entityCacheContextRequest =
                BuildContainerEntityTypeAttributeMappingList(containerEntityTypeAttributeMappings);

            if (entityCacheContextRequest.Count > 0)
            {
                // Create EntityCacheLoadContext Container entityCacheContextRequest
                EntityCacheLoadContext entityCacheLoadContext = new EntityCacheLoadContext();
                entityCacheLoadContext.CacheStatus = (Int32)(EntityCacheComponentEnum.InheritedAttributes | EntityCacheComponentEnum.OverriddenAttributes);

                foreach (Tuple<Int32, Int32, Collection<Int32>> item in entityCacheContextRequest)
                {
                    EntityCacheLoadContextItemCollection entityCacheLoadContextItemCollection = GetEntityCacheLoadContextItemCollection(item.Item1, item.Item2,
                        item.Item3);
                    entityCacheLoadContext.Add(entityCacheLoadContextItemCollection);
                }

                UpdateEntityCacheLoadContextInActivityLog(entityCacheLoadContext.ToXml());
            }
        }

        /// <summary>
        /// Processes the EntityCacheLoadContext for Container entity type mapping change.
        /// </summary>
        private void UpdateEntityCacheLoadContextInActivityLog(String entityCacheLoadContext)
        {
            EntityActivityLog entityActivityLog = new EntityActivityLog()
            {
                PerformedAction = EntityActivityList.EntityCacheLoad,
                Context = entityCacheLoadContext
            };

            EntityActivityLogCollection entityActivityLogCollection = new EntityActivityLogCollection() { entityActivityLog };

            EntityActivityLogBL entityActivityLogBL = new EntityActivityLogBL();
            entityActivityLogBL.Process(entityActivityLogCollection, new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.Modeling));
        }

        /// <summary>
        /// Splits the ContainerEntityTypeAttributeMappingCollection into a collection of tuple containing the container id, entity type id and attribute id list.
        /// </summary>
        private Collection<Tuple<Int32, Int32, Collection<Int32>>> BuildContainerEntityTypeAttributeMappingList(ContainerEntityTypeAttributeMappingCollection containerEntityTypeAttributeMappings)
        {
            Collection<Tuple<Int32, Int32, Collection<Int32>>> entityCacheContextRequest = new Collection<Tuple<Int32, Int32, Collection<Int32>>>();

            foreach (ContainerEntityTypeAttributeMapping containerEntityTypeAttributeMapping in containerEntityTypeAttributeMappings)
            {
                if (containerEntityTypeAttributeMapping.Action == ObjectAction.Create || containerEntityTypeAttributeMapping.Action == ObjectAction.Delete)
                {
                    Collection<Int32> attributeIdList = GetAttributeIdList(entityCacheContextRequest, containerEntityTypeAttributeMapping.ContainerId,
                        containerEntityTypeAttributeMapping.EntityTypeId);
                    if (attributeIdList != null)
                    {
                        attributeIdList.Add(containerEntityTypeAttributeMapping.AttributeId);
                    }
                    else
                    {
                        Tuple<Int32, Int32, Collection<Int32>> entityCacheContextRequestItem = new Tuple<Int32, Int32, Collection<Int32>>(
                            containerEntityTypeAttributeMapping.ContainerId, containerEntityTypeAttributeMapping.EntityTypeId,
                            new Collection<Int32>() { containerEntityTypeAttributeMapping.AttributeId });
                        entityCacheContextRequest.Add(entityCacheContextRequestItem);
                    }
                }
            }

            return entityCacheContextRequest;
        }

        /// <summary>
        /// Returns the Attribute id list for the specified Container Id and Entity type id.
        /// </summary>
        private Collection<Int32> GetAttributeIdList(Collection<Tuple<Int32, Int32, Collection<Int32>>> entityCacheContextRequest, Int32 containerId, Int32 entityTypeId)
        {
            foreach (Tuple<Int32, Int32, Collection<Int32>> entityCacheContextRequestItem in entityCacheContextRequest)
            {
                if (entityCacheContextRequestItem.Item1 == containerId && entityCacheContextRequestItem.Item2 == entityTypeId)
                {
                    return entityCacheContextRequestItem.Item3;
                }
            }
            return null;
        }

        /// <summary>
        /// Creates an EntityCacheLoadContextItemCollection for the specified container id, entity type id and attribute id list.
        /// </summary>
        private EntityCacheLoadContextItemCollection GetEntityCacheLoadContextItemCollection(Int32 containerId, Int32 entityTypeId, Collection<Int32> attributeIdList)
        {
            // Create EntityCacheLoadContextItemCollection
            EntityCacheLoadContextItemCollection entityCacheLoadContextItemCollection = new EntityCacheLoadContextItemCollection();

            // Create EntityCacheLoadContextItem for Container
            EntityCacheLoadContextItem entityCacheLoadContextItemForContainer =
                entityCacheLoadContextItemCollection.GetOrCreateEntityCacheLoadContextItem(EntityCacheLoadContextTypeEnum.Container);
            entityCacheLoadContextItemForContainer.AddValues(containerId);

            // Create EntityCacheLoadContextItem for Entity Type
            EntityCacheLoadContextItem entityCacheLoadContextItemForEntityType =
                entityCacheLoadContextItemCollection.GetOrCreateEntityCacheLoadContextItem(EntityCacheLoadContextTypeEnum.EntityType);
            entityCacheLoadContextItemForEntityType.AddValues(entityTypeId);

            // Create EntityCacheLoadContextItem for Attributes
            EntityCacheLoadContextItem entityCacheLoadContextItemForAttributes =
                entityCacheLoadContextItemCollection.GetOrCreateEntityCacheLoadContextItem(EntityCacheLoadContextTypeEnum.Attribute);
            entityCacheLoadContextItemForAttributes.AddValues(attributeIdList);

            return entityCacheLoadContextItemCollection;
        }

        #endregion

        #region Misc. Methods

        /// <summary>
        /// Validates container entity type attribute mapping properties.
        /// </summary>
        /// <param name="attributeModels">Specifies attribute models for validation</param>
        /// <param name="operationResult">Specifies validation operation result</param>
        public static void ValidateMappingProperties(ContainerEntityTypeAttributeMappingCollection mappings, AttributeModelCollection attributeModels, IOperationResult operationResult)
        {

            String errorMessage = String.Empty;
            String attributeNameList = String.Empty;

            List<String> inheritableAttrName = new List<String>();
            List<String> requiredAttrName = new List<String>();
            AttributeModel attributeModel = null;

            foreach (ContainerEntityTypeAttributeMapping mapping in mappings)
            {
                attributeModel = attributeModels.GetAttributeModel(mapping.AttributeId).FirstOrDefault();

                if (attributeModel != null && attributeModel.Id == mapping.AttributeId)
                {
                    if (!attributeModel.Inheritable && mapping.InheritableOnly)
                    {
                        inheritableAttrName.Add(String.Format("'{0}'", attributeModel.LongName));
                    }

                    if (mapping.Required && mapping.InheritableOnly)
                    {
                        requiredAttrName.Add(String.Format("'{0}'", attributeModel.LongName));
                    }
                }
            }

            if (inheritableAttrName.Count > 0)
            {
                String inheritableAttrNameString = String.Join(",", inheritableAttrName);

                object[] parameters = new Object[] { inheritableAttrNameString };

                errorMessage = String.Format("Failed to set attribute {0} to 'Inheritable Only', as it is a non-inheritable attribute.",
                                parameters);

                operationResult.AddOperationResult("114241", errorMessage, parameters, OperationResultType.Error);
            }

            if (requiredAttrName.Count > 0)
            {
                String requiredAttrNameString = String.Join(",", inheritableAttrName);

                object[] parameters = new Object[] { requiredAttrNameString };

                errorMessage = String.Format("Failed to set attribute {0} as both 'Inheritable Only' and 'Required' attribute. Select either 'Inheritable Only' or 'Required'.",
                                parameters);

                operationResult.AddOperationResult("114242", errorMessage, parameters, OperationResultType.Error);
            }
        }

        /// <summary>
        /// Validates container entity type attribute mapping properties.
        /// </summary>
        /// <param name="attributeModels">Specifies attribute models for validation</param>
        /// <param name="operationResult">Specifies validation operation result</param>
        public static void ValidateEntityTypeMappingDataModelProperties(ContainerEntityTypeAttributeMapping mapping, Boolean isInheritable,
                                                                           IDataModelOperationResult iDataModelOperationResult, CallerContext callerContext)
        {
            String errorMessage = String.Empty;
            Object[] parameters = new Object[] { mapping.ContainerName, mapping.EntityTypeName, mapping.AttributeName }; ;

            if (mapping.InheritableOnly && !isInheritable)
            {
                errorMessage = String.Format("Failed to set container entity type attribute mapping with container '{0}', entity type '{1}', and attribute '{2}' to 'Inheritable Only', as it is non-inheritable attribute.",
                                                parameters);

                DataModelHelper.AddOperationResult(iDataModelOperationResult, "114238", errorMessage, parameters, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
            if (mapping.InheritableOnly && mapping.Required)
            {
                errorMessage = String.Format("Failed to set container entity type attribute mapping with container '{0}', entity type '{1}', and attribute '{2}' as both 'Inheritable Only' and 'Required'. Select either 'Inheritable Only' or 'Required'.",
                                                parameters);

                DataModelHelper.AddOperationResult(iDataModelOperationResult, "114240", errorMessage, parameters, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
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
                callerContext.ProgramName = "ContainerEntityTypeAttributeMappingBL";
            }
        }

        /// <summary>
        /// Log changes to tb_datamodelactivitylog if DA call is successful and appconfig key datamodelactivitylog.enable == true
        /// </summary>
        /// <param name="containerEntityTypeAttributeMappings">Indicates container entity type attribute mappings to be logged</param>
        /// <param name="callerContext">Indicates caller context</param>
        private void LogDataModelChanges(ContainerEntityTypeAttributeMappingCollection containerEntityTypeAttributeMappings, CallerContext callerContext)
        {
            #region Step: Populate datamodelactivitylog object

            DataModelActivityLogBL dataModelActivityLogManager = new DataModelActivityLogBL();
            DataModelActivityLogCollection activityLogCollection = dataModelActivityLogManager.FillDataModelActivityLogCollection(containerEntityTypeAttributeMappings, containerEntityTypeAttributeMappings.DataModelObjectType, callerContext);

            #endregion Step: Populate datamodelactivitylog object

            #region Step: Make api call

            if (activityLogCollection != null && activityLogCollection.Count > 0)
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