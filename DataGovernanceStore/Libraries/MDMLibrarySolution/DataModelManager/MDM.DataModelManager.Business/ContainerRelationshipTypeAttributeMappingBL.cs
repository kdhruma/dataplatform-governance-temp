using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Transactions;
using System.Text;

namespace MDM.DataModelManager.Business
{
    using MDM.ActivityLogManager.Business;
    using MDM.Utility;
    using MDM.BusinessObjects;
    using MDM.DataModelManager.Data;
    using MDM.Core;
    using MDM.BufferManager;
    using MDM.ConfigurationManager.Business;
    using MDM.BusinessObjects.DataModel;
    using MDM.Interfaces;
    using MDM.MessageManager.Business;
    using System.Diagnostics;
    using MDM.Core.Exceptions;

    /// <summary>
    /// Represents business logic for Container RelationshipType Attribute mapping
    /// </summary>
    public class ContainerRelationshipTypeAttributeMappingBL : BusinessLogicBase, IDataModelManager
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
        /// Field denoting the IOrganizationManager
        /// </summary>
        private IOrganizationManager _iOrganizationManager = null;

        /// <summary>
        /// Field denoting the IContainerManager
        /// </summary>
        private IContainerManager _iContainerManager = null;

        /// <summary>
        /// Field denoting the IRelationshipTypeManager
        /// </summary>
        private IRelationshipTypeManager _iRelationshipTypeManager = null;

        /// <summary>
        /// Field denoting the IAttributeModelManager
        /// </summary>
        private IAttributeModelManager _iAttributeModelManager = null;

        /// <summary>
        /// Specifies mapping buffer manager
        /// </summary>
        private MappingBufferManager _mappingBufferManager = new MappingBufferManager();

        /// <summary>
        /// Lookup data access object
        /// </summary>
        private ContainerRelationshipTypeAttributeMappingDA _containerRelationshipTypeAttributeMappingDA = new ContainerRelationshipTypeAttributeMappingDA();

        #endregion

        #region Constructors

        /// <summary>
        ///Constructor for ContainerRelationshipTypeAttributeMappingBL
        /// </summary>
        public ContainerRelationshipTypeAttributeMappingBL()
        {
            GetSecurityPrincipal();
        }

        /// <summary>
        ///Constructor for ContainerRelationshipTypeAttributeMappingBL
        /// </summary>
        /// <param name="iOrganizationManager"></param>
        /// <param name="iContainerManager"></param>
        /// <param name="iRelationshipTypeManager"></param>
        /// <param name="iAttributeModelManager"></param>
        public ContainerRelationshipTypeAttributeMappingBL(IOrganizationManager iOrganizationManager, IContainerManager iContainerManager, IRelationshipTypeManager iRelationshipTypeManager, IAttributeModelManager iAttributeModelManager)
        {
            this._iOrganizationManager = iOrganizationManager;
            this._iContainerManager = iContainerManager;
            this._iRelationshipTypeManager = iRelationshipTypeManager;
            this._iAttributeModelManager = iAttributeModelManager;
            GetSecurityPrincipal();
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region Public Methods

        #region CUD Methods

        /// <summary>
        /// Add new container - relationshipType - attribute mapping.
        /// </summary>
        /// <param name="containerRelationshipTypeAttributeMapping"></param>
        /// <returns></returns>
        public Boolean Create(ContainerRelationshipTypeAttributeMapping containerRelationshipTypeAttributeMapping)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update container - relationshipType - attribute mapping
        /// </summary>
        /// <param name="containerRelationshipTypeAttributeMapping"></param>
        /// <returns></returns>
        public Boolean Update(ContainerRelationshipTypeAttributeMapping containerRelationshipTypeAttributeMapping)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Remove existing container - relationshipType - attribute mapping
        /// </summary>
        /// <param name="containerRelationshipTypeAttributeMapping"></param>
        /// <returns></returns>
        public Boolean Delete(ContainerRelationshipTypeAttributeMapping containerRelationshipTypeAttributeMapping)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create, Update or Delete Container RelationshipType Attribute Mappings
        /// </summary>
        /// <param name="containerRelationshipTypeAttributeMappings">Container RelationshipType Attribute Mappings to process</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public OperationResult Process(ContainerRelationshipTypeAttributeMappingCollection containerRelationshipTypeAttributeMappings, CallerContext callerContext)
        {
            OperationResult operationResult = new OperationResult();
            operationResult.OperationResultStatus = OperationResultStatusEnum.Failed;

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("ContainerRelationshipTypeAttributeMappingBL.Process", MDMTraceSource.DataModel, false);

                if (containerRelationshipTypeAttributeMappings != null && containerRelationshipTypeAttributeMappings.Count > 0)
                {
                    IEnumerable<Int32> attributeIds = containerRelationshipTypeAttributeMappings.Where(mapping => mapping.Action != ObjectAction.Delete).Select(mapping => mapping.AttributeId);
                    AttributeModelCollection baseAttributeModels = _iAttributeModelManager.GetAllBaseAttributeModels();
                    AttributeModelCollection attributeModelsForMapping = new AttributeModelCollection();
                    foreach (AttributeModel baseAttributeModel in baseAttributeModels)
                    {
                        if (attributeIds.Contains(baseAttributeModel.Id))
                        {
                            attributeModelsForMapping.Add(baseAttributeModel);
                        }
                    }

                    DataModelHelper.ValidateComplexAttributes(attributeModelsForMapping, operationResult);
                    if (operationResult.HasError)
                    {
                        foreach (Error error in operationResult.Errors)
                        {
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, error.ErrorMessage);
                        }
                        return operationResult;
                    }

                    using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, BusinessLogicBase.GetTransactionOptions(ProcessingMode.Sync)))
                    {
                        String userName = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;

                        ContainerRelationshipTypeAttributeMappingDA containerRelationshipTypeAttributeMappingsDA = new ContainerRelationshipTypeAttributeMappingDA();
                        containerRelationshipTypeAttributeMappingsDA.Process(containerRelationshipTypeAttributeMappings, userName, "ContainerRelationshipTypeAttributeMappingBL");

                        ProcessEntityCacheLoadContextForMappingChange(containerRelationshipTypeAttributeMappings);

                        transactionScope.Complete();

                        operationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                    }

                    #region activity log

                    if (operationResult.OperationResultStatus == OperationResultStatusEnum.Successful && AppConfigurationHelper.GetAppConfig<Boolean>(Constants.ACTIVITYLOG_DATAMODEL_ENABLED_APPCONFIG_KEY, false))
                    {
                        callerContext.ProgramName = "ContainerRelationshipTypeAttributeMappingBL";
                        LogDataModelChanges(containerRelationshipTypeAttributeMappings, callerContext);
                    }

                    #endregion activity log

                    #region Invalidate Container-RelationshipType Attribute Mappings

                    //Collect all container Ids and relationship type ids.
                    Collection<Int32> containerIds = new Collection<Int32>();
                    Collection<Int32> relationshipTypeIds = new Collection<Int32>();

                    var enumerator = containerRelationshipTypeAttributeMappings.GetEnumerator();

                    while (enumerator.MoveNext())
                    {
                        Int32 containerId = enumerator.Current.ContainerId;
                        Int32 relationshipTypeId = enumerator.Current.RelationshipTypeId;

                        if (!containerIds.Contains(containerId))
                            containerIds.Add(containerId);

                        if (!relationshipTypeIds.Contains(relationshipTypeId))
                            relationshipTypeIds.Add(relationshipTypeId);
                    }

                    _mappingBufferManager.RemoveContainerRelationshipAttributeMappings(containerIds, relationshipTypeIds, true);

                    #endregion

                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerRelationshipTypeAttributeMappingBL.Process", MDMTraceSource.DataModel);
            }

            return operationResult;
        }

        #endregion

        #region Get Methods

        /// <summary>
        /// Get container - relationshipType - attribute mapping by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ContainerRelationshipTypeAttributeMapping GetById(Int32 id)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Gets Container RelationshipType Attribute mappings from the system based on containerId, relationshipId and attributeGroupId
        /// </summary>
        /// <param name="containerId">Indicates the container Id for which mappings needs to be fetched</param>
        /// <param name="relationshipTypeId">Indicates the RelationshipType Id for which mappings needs to be fetched</param>
        /// <param name="attributeGroupId">Indicates the AttributeGroup Id for which mappings needs to be fetched</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Container RelationshipType Attribute mappings for specified containerId, relationshipTypeId and attributeGroupId</returns>
        public ContainerRelationshipTypeAttributeMappingCollection Get(Int32 containerId, Int32 relationshipTypeId, Int32 attributeGroupId, CallerContext callerContext)
        {
            return Get(containerId, relationshipTypeId, attributeGroupId, false, callerContext); 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="relationshipTypeId"></param>
        /// <param name="attributeGroupId"></param>
        /// <param name="getLatest"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public ContainerRelationshipTypeAttributeMappingCollection Get(Int32 containerId, Int32 relationshipTypeId, Int32 attributeGroupId, Boolean getLatest, CallerContext callerContext)
        {
            ContainerRelationshipTypeAttributeMappingCollection filteredContainerRelationshipTypeAttributeMappings = null;

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("ContainerRelationshipTypeAttributeMappingBL.Get", MDMTraceSource.DataModel, false);

                #region Initial Setup

                ContainerRelationshipTypeAttributeMappingCollection containerRelationshipTypeAttributeMappings = null;

                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

                #endregion

                #region Get Mappings from Cache if available

                if (!getLatest)
                {
                    containerRelationshipTypeAttributeMappings = _mappingBufferManager.FindContainerRelationshipAttributeMappingsCompleteDetails(containerId, relationshipTypeId);
                }

                #endregion

                #region Get Mappings from Database

                if (containerRelationshipTypeAttributeMappings == null)
                {
                    //Since there is no mechanism for holding Relatioinship attributes per group in cache, 
                    //We always make a DB call with attributegroupId as -1 and get all mapped attribues.
                    //This copy is maintaied for both Partial and Complete Cache keys.
                    //Later if AttributeGroup is > 0, the data is filtered by AttributeGroupId.
                    containerRelationshipTypeAttributeMappings = _containerRelationshipTypeAttributeMappingDA.Get(containerId, relationshipTypeId, -1, -1, command);

                    #region Cache Mappings data

                    if (containerRelationshipTypeAttributeMappings != null)
                    {
                        _mappingBufferManager.UpdateContainerRelationshipAttributeMappings(containerRelationshipTypeAttributeMappings, containerId, relationshipTypeId, 3);
                    }

                    #endregion
                }

                #endregion

                if (attributeGroupId > 0)
                {
                    filteredContainerRelationshipTypeAttributeMappings = containerRelationshipTypeAttributeMappings.GetByAttributeGroupId(attributeGroupId);
                }
                else
                {
                    filteredContainerRelationshipTypeAttributeMappings = containerRelationshipTypeAttributeMappings;
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerRelationshipTypeAttributeMappingBL.Get", MDMTraceSource.DataModel);
            }

            return filteredContainerRelationshipTypeAttributeMappings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public ContainerRelationshipTypeAttributeMappingCollection GetAll(CallerContext callerContext)
        {
            ContainerRelationshipTypeAttributeMappingCollection containerRelationshipTypeAttributeMappings = new ContainerRelationshipTypeAttributeMappingCollection();
            ContainerRelationshipTypeAttributeMappingBL mappingBL = new ContainerRelationshipTypeAttributeMappingBL();

            IEnumerable<KeyValuePair<Int32, Int32>> valueCollection = GetUniqueContainerRelationshipTypeMappings(callerContext);

            foreach (KeyValuePair<Int32, Int32> keyValue in valueCollection)
            {
                ContainerRelationshipTypeAttributeMappingCollection containerRelTypeAttributeMappings = mappingBL.Get(keyValue.Key, keyValue.Value, -1, callerContext);

                if (containerRelTypeAttributeMappings != null && containerRelTypeAttributeMappings.Count > 0)
                {
                    foreach (ContainerRelationshipTypeAttributeMapping containerRelationshipTypeAttributeMapping in containerRelTypeAttributeMappings)
                    {
                        containerRelationshipTypeAttributeMappings.Add(containerRelationshipTypeAttributeMapping);
                    }
                }
            }

            return containerRelationshipTypeAttributeMappings;
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
                    MDMTraceHelper.StartTraceActivity("ContainerRelationshipTypeAttributeMappingBL.PrepareOperationResultsSchema", MDMTraceSource.DataModel, false);

                String separator = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.RSDataModelExcel.Separator", "//").Trim();
                ContainerRelationshipTypeAttributeMappingCollection containerRelationshipTypeAttributeMappings = iDataModelObjects as ContainerRelationshipTypeAttributeMappingCollection;

                if (containerRelationshipTypeAttributeMappings != null && containerRelationshipTypeAttributeMappings.Count > 0)
                {
                    if (operationResults.Count > 0)
                    {
                        operationResults.Clear();
                    }

                    int containerRelationshipTypeAttributeMappingIdToBeCreated = -1;

                    foreach (ContainerRelationshipTypeAttributeMapping containerRelationshipTypeAttributeMapping in containerRelationshipTypeAttributeMappings)
                    {
                        DataModelOperationResult containerRelationshipTypeAttributeMappingOperationResult = new DataModelOperationResult(containerRelationshipTypeAttributeMapping.Id, containerRelationshipTypeAttributeMapping.LongName, containerRelationshipTypeAttributeMapping.ExternalId, containerRelationshipTypeAttributeMapping.ReferenceId);

                        if (String.IsNullOrEmpty(containerRelationshipTypeAttributeMappingOperationResult.ExternalId))
                        {
                            containerRelationshipTypeAttributeMappingOperationResult.ExternalId = String.Format("{1} {0} {2} {0} {3} {0} {4} {0} {5}", separator, containerRelationshipTypeAttributeMapping.OrganizationName, containerRelationshipTypeAttributeMapping.ContainerName, containerRelationshipTypeAttributeMapping.RelationshipTypeName, containerRelationshipTypeAttributeMapping.AttributeParentName, containerRelationshipTypeAttributeMapping.AttributeName);
                        }

                        if (containerRelationshipTypeAttributeMapping.Id < 1)
                        {
                            containerRelationshipTypeAttributeMapping.Id = containerRelationshipTypeAttributeMappingIdToBeCreated;
                            containerRelationshipTypeAttributeMappingOperationResult.Id = containerRelationshipTypeAttributeMappingIdToBeCreated;
                            containerRelationshipTypeAttributeMappingIdToBeCreated--;
                        }

                        operationResults.Add(containerRelationshipTypeAttributeMappingOperationResult);
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
                    MDMTraceHelper.StopTraceActivity("ContainerRelationshipTypeAttributeMappingBL.PrepareOperationResultsSchema", MDMTraceSource.DataModel);
            }
        }

        /// <summary>
        /// Validates DataModelObjects and populate OperationResults
        /// </summary>
        /// <param name="iDataModelObjects">Indicates collection of data model objects to be filled up.</param>
        /// <param name="operationResults">Indicates collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Indicates the context of the caller</param>
        /// <returns>DataModel Operation Result Collection</returns>
        public void Validate(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            ContainerRelationshipTypeAttributeMappingCollection containerRelationshipTypeAttributeMappings = iDataModelObjects as ContainerRelationshipTypeAttributeMappingCollection;
            CallerContext callerContext = iCallerContext as CallerContext;

            try
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StartTraceActivity("ContainerRelationshipTypeAttributeMappingBL.Validate", MDMTraceSource.DataModel, false);
                }

                #region Parameter Validations

                ValidateInputParameters(containerRelationshipTypeAttributeMappings, operationResults, callerContext);

                #endregion

                #region Parameter Validations

                ValidateData(containerRelationshipTypeAttributeMappings, operationResults, callerContext);

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
                    MDMTraceHelper.StopTraceActivity("ContainerRelationshipTypeAttributeMappingBL.Validate", MDMTraceSource.DataModel);
                }
            }
        }

        /// <summary>
        /// Load original data model objects
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to load original object.</param>
        /// <param name="operationResults">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void LoadOriginal(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
             try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("ContainerRelationshipTypeAttributeMappingBL.LoadOriginal", MDMTraceSource.DataModel, false);

                LoadOriginalContainerRelationshipTypeAttributeMapping(iDataModelObjects as ContainerRelationshipTypeAttributeMappingCollection, iCallerContext as CallerContext);
            }
             catch (Exception ex)
             {
                 DataModelHelper.AddOperationResults(operationResults, String.Empty, ex.Message, null, OperationResultType.Error, TraceEventType.Error, iCallerContext as CallerContext);
             }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerRelationshipTypeAttributeMappingBL.LoadOriginal", MDMTraceSource.DataModel);
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
                    MDMTraceHelper.StartTraceActivity("ContainerRelationshipTypeAttributeMappingBL.FillDataModel", MDMTraceSource.DataModel, false);

                FillContainerRelationshipTypeAttributeMapping(iDataModelObjects as ContainerRelationshipTypeAttributeMappingCollection, iCallerContext as CallerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerRelationshipTypeAttributeMappingBL.FillDataModel", MDMTraceSource.DataModel);
            }
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
            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("ContainerRelationshipTypeAttributeMappingBL.CompareAndMerge", MDMTraceSource.DataModel, false);

                CompareAndMerge(iDataModelObjects as ContainerRelationshipTypeAttributeMappingCollection, operationResults, iCallerContext as CallerContext);
            }
            catch (Exception ex)
            {
                DataModelHelper.AddOperationResults(operationResults, String.Empty, ex.Message, null, OperationResultType.Error, TraceEventType.Error, iCallerContext as CallerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerRelationshipTypeAttributeMappingBL.CompareAndMerge", MDMTraceSource.DataModel);
            }
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
            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("ContainerRelationshipTypeAttributeMappingBL.Process", MDMTraceSource.DataModel, false);

                ContainerRelationshipTypeAttributeMappingCollection containerRelationshipTypeAttributeMappings = iDataModelObjects as ContainerRelationshipTypeAttributeMappingCollection;
                CallerContext callerContext = iCallerContext as CallerContext;

                if (containerRelationshipTypeAttributeMappings.Count > 0)
                {
                    PopulateProgramName(callerContext);    
                    
                    #region Perform ContainerRelationshipTypeAttributeMapping updates in database

                    using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                    {
                        _containerRelationshipTypeAttributeMappingDA.Process(containerRelationshipTypeAttributeMappings, operationResults, _securityPrincipal.CurrentUserName, callerContext.ProgramName);

                        transactionScope.Complete();
                    }

                    LocalizeErrors(callerContext, operationResults);

                    #endregion

                    #region activity log

                    if (operationResults.OperationResultStatus == OperationResultStatusEnum.Successful && AppConfigurationHelper.GetAppConfig<Boolean>(Constants.ACTIVITYLOG_DATAMODEL_ENABLED_APPCONFIG_KEY, false))
                    {
                        callerContext.ProgramName = "ContainerRelationshipTypeAttributeMappingBL";
                        LogDataModelChanges(containerRelationshipTypeAttributeMappings, callerContext);
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
                    MDMTraceHelper.StopTraceActivity("ContainerRelationshipTypeAttributeMappingBL.Process", MDMTraceSource.DataModel);
            }
        }

        ///<summary>
        /// Processes the Entity cache statuses for data model objects
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be process entity cache load.</param>
        /// <param name="operationResults">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void InvalidateCache(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("ContainerRelationshipTypeAttributeMappingBL.InvalidateCache", MDMTraceSource.DataModel, false);

                ContainerRelationshipTypeAttributeMappingCollection containerRelationshipTypeAttributeMappings = iDataModelObjects as ContainerRelationshipTypeAttributeMappingCollection;

                foreach (ContainerRelationshipTypeAttributeMapping containerRelationshipTypeAttributeMapping in containerRelationshipTypeAttributeMappings)
                {
                    // clear buffer manager
                    _mappingBufferManager.RemoveContainerRelationshipAttributeMappings(containerRelationshipTypeAttributeMapping.ContainerId, containerRelationshipTypeAttributeMapping.RelationshipTypeId, false);
                }
            }
            catch (Exception ex)
            {
                DataModelHelper.AddOperationResults(operationResults, String.Empty, ex.Message, null, OperationResultType.Error, TraceEventType.Error, iCallerContext as CallerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerRelationshipTypeAttributeMappingBL.InvalidateCache", MDMTraceSource.DataModel);
            }
        }

        #endregion

        #endregion

        #region Private Methods

        #region Validation Methods

        /// <summary>
        /// Validate the input parameters in container relationship type attribute mapping collection
        /// </summary>
        /// <param name="containerRelationshipTypeAttributeMappings">Indicates the collection of container relationship type attribute mapping</param>
        /// <param name="callerContext">Indicates the context of the caller</param>
        private void ValidateInputParameters(ContainerRelationshipTypeAttributeMappingCollection containerRelationshipTypeAttributeMappings, CallerContext callerContext)
        {
            String errorMessage;

            if (containerRelationshipTypeAttributeMappings == null || containerRelationshipTypeAttributeMappings.Count < 1)
            {
                errorMessage = "ContainerRelationshipTypeAttributeMapping Collection cannot be null.";
                throw new MDMOperationException("", errorMessage, "DataModelManager.ContainerRelationshipTypeAttributeMappingBL", String.Empty, "Create");
            }

            if (callerContext == null)
            {
                errorMessage = "CallerContext cannot be null.";
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "DataModelManager.ContainerRelationshipTypeAttributeMappingBL", String.Empty, "Create");
            }
        }

        /// <summary>
        /// Validate the input parameters in container relationship type attribute mapping collection
        /// </summary>
        /// <param name="containerRelationshipTypeAttributeMappings">Indicates the collection of container relationship type attribute mapping</param>
        /// <param name="operationResults">Indicates the result of operation</param>
        /// <param name="callerContext">Indicates the context of the caller</param>
        private void ValidateInputParameters(ContainerRelationshipTypeAttributeMappingCollection containerRelationshipTypeAttributeMappings, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            if (containerRelationshipTypeAttributeMappings == null || containerRelationshipTypeAttributeMappings.Count < 1)
            {
                DataModelHelper.AddOperationResults(operationResults, "113617", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }

            if (callerContext == null)
            {
                DataModelHelper.AddOperationResults(operationResults, "111846", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
        }

        /// <summary>
        /// Validate the data of specified parameters in container relationship type attribute mapping collection
        /// </summary>
        /// <param name="containerRelationshipTypeAttributeMappings">Indicates the collection of container relationship type attribute mapping</param>
        /// <param name="operationResults">Indicates the result of operation</param>
        /// <param name="callerContext">Indicates the context of the caller</param>
        private void ValidateData(ContainerRelationshipTypeAttributeMappingCollection containerRelationshipTypeAttributeMappings, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            if (containerRelationshipTypeAttributeMappings != null && containerRelationshipTypeAttributeMappings.Count > 0)
            {
                Collection<String> containerRelationshipTypeAttributeMappingList = new Collection<String>();
                AttributeModelCollection attributeModels = new AttributeModelCollection();

                foreach (ContainerRelationshipTypeAttributeMapping containerRelationshipTypeAttributeMapping in containerRelationshipTypeAttributeMappings)
                {
                    IDataModelOperationResult dataModelOperationResult = operationResults.GetByReferenceId(containerRelationshipTypeAttributeMapping.ReferenceId);
                    AttributeModel attributeModel = DataModelHelper.GetAttributeModel(_iAttributeModelManager, containerRelationshipTypeAttributeMapping.AttributeId, containerRelationshipTypeAttributeMapping.AttributeName, containerRelationshipTypeAttributeMapping.AttributeParentName, ref attributeModels, callerContext);

                    if (attributeModel != null && attributeModel.AttributeModelType != AttributeModelType.Relationship)
                    {
                        String errorMessage =
                            "Attribute '{0}' under '{1}' group is of the type '{2}', so mapping cannot be performed for organization name '{3}', container name '{4}' and relationship type '{5}'.";
                        Object[] parameters = new Object[]
                        {
                            containerRelationshipTypeAttributeMapping.AttributeName,
                            containerRelationshipTypeAttributeMapping.AttributeParentName,
                            attributeModel.AttributeModelType.ToString().ToLowerInvariant(),
                            containerRelationshipTypeAttributeMapping.OrganizationName,
                            containerRelationshipTypeAttributeMapping.ContainerName,
                            containerRelationshipTypeAttributeMapping.RelationshipTypeName
                        };

                        DataModelHelper.AddOperationResult(dataModelOperationResult, "114130",
                            String.Format(errorMessage, parameters), parameters, OperationResultType.Error,
                            TraceEventType.Error, callerContext);
                    }
                    else
                    {
                        Collection<String> uniqueIdentifierList = new Collection<String>()
                        {
                            containerRelationshipTypeAttributeMapping.OrganizationName,
                            containerRelationshipTypeAttributeMapping.ContainerName,
                            containerRelationshipTypeAttributeMapping.RelationshipTypeName,
                            containerRelationshipTypeAttributeMapping.AttributeName,
                            containerRelationshipTypeAttributeMapping.AttributeParentName
                        };

                        String rowUniqueIdentifier = DataModelHelper.GetUniqueIdentifier(uniqueIdentifierList);

                        DataModelHelper.ValidateOrganizationName(
                            containerRelationshipTypeAttributeMapping.OrganizationName,
                            dataModelOperationResult, callerContext);

                        DataModelHelper.ValidateContainerName(containerRelationshipTypeAttributeMapping.ContainerName,
                            dataModelOperationResult, callerContext);

                        DataModelHelper.ValidateRelationshipTypeName(
                            containerRelationshipTypeAttributeMapping.RelationshipTypeName,
                            dataModelOperationResult, callerContext);

                        DataModelHelper.ValidateAttributeUniqueIdentifier(
                            containerRelationshipTypeAttributeMapping.AttributeName,
                            containerRelationshipTypeAttributeMapping.AttributeParentName,
                            dataModelOperationResult, callerContext);

                        if (containerRelationshipTypeAttributeMappingList.Contains(rowUniqueIdentifier))
                        {
                            Object[] parameters = null;
                            parameters = new Object[]
                            {
                                containerRelationshipTypeAttributeMapping.ContainerName,
                                containerRelationshipTypeAttributeMapping.RelationshipTypeName,
                                containerRelationshipTypeAttributeMapping.AttributeName,
                                containerRelationshipTypeAttributeMapping.AttributeParentName
                            };

                            String errorMessage =
                                String.Format(
                                    "Duplicate mappings found for the container relationship type attribute mapping with container: {0}, relationship type: {1}, attribute: {2}, and attribute parent: {3}",
                                    parameters);

                            DataModelHelper.AddOperationResult(dataModelOperationResult, "113971", errorMessage,
                                parameters,
                                OperationResultType.Error, TraceEventType.Error, callerContext);
                        }
                        else
                        {
                            containerRelationshipTypeAttributeMappingList.Add(rowUniqueIdentifier);
                        }
                    }
                }
            }
        }
        #endregion

        #region Get Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerRelationshipTypeAttributeMappings"></param>
        /// <param name="callerContext"></param>
        private void LoadOriginalContainerRelationshipTypeAttributeMapping(ContainerRelationshipTypeAttributeMappingCollection containerRelationshipTypeAttributeMappings, CallerContext callerContext)
        {
            ContainerRelationshipTypeAttributeMappingCollection orginalContainerRelationshipTypeAttributeMappingCollection = GetAll(callerContext);

            if (orginalContainerRelationshipTypeAttributeMappingCollection != null && orginalContainerRelationshipTypeAttributeMappingCollection.Count > 0)
            {
                foreach (ContainerRelationshipTypeAttributeMapping containerRelationshipTypeAttributeMapping in containerRelationshipTypeAttributeMappings)
                {
                    containerRelationshipTypeAttributeMapping.OriginalContainerRelationshipTypeAttributeMapping = orginalContainerRelationshipTypeAttributeMappingCollection.Get(containerRelationshipTypeAttributeMapping.OrganizationName, containerRelationshipTypeAttributeMapping.ContainerName, containerRelationshipTypeAttributeMapping.RelationshipTypeName, containerRelationshipTypeAttributeMapping.AttributeName, containerRelationshipTypeAttributeMapping.AttributeParentName);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerRelationshipTypeAttributeMappings"></param>
        /// <param name="callerContext"></param>
        private void FillContainerRelationshipTypeAttributeMapping(ContainerRelationshipTypeAttributeMappingCollection containerRelationshipTypeAttributeMappings, CallerContext callerContext)
        {
            OrganizationCollection organizations = new OrganizationCollection();
            ContainerCollection containers = new ContainerCollection();
            RelationshipTypeCollection relationshipTypes = new RelationshipTypeCollection();
            AttributeModelCollection attributeModels = new AttributeModelCollection();

            foreach (ContainerRelationshipTypeAttributeMapping containerRelationshipTypeAttributeMapping in containerRelationshipTypeAttributeMappings)
            {
                if (containerRelationshipTypeAttributeMapping.OriginalContainerRelationshipTypeAttributeMapping != null)
                {
                    containerRelationshipTypeAttributeMapping.Id = containerRelationshipTypeAttributeMapping.OriginalContainerRelationshipTypeAttributeMapping.Id;
                    containerRelationshipTypeAttributeMapping.OrganizationId = containerRelationshipTypeAttributeMapping.OriginalContainerRelationshipTypeAttributeMapping.OrganizationId;
                    containerRelationshipTypeAttributeMapping.ContainerId = containerRelationshipTypeAttributeMapping.OriginalContainerRelationshipTypeAttributeMapping.ContainerId;
                    containerRelationshipTypeAttributeMapping.RelationshipTypeId = containerRelationshipTypeAttributeMapping.OriginalContainerRelationshipTypeAttributeMapping.RelationshipTypeId;
                    containerRelationshipTypeAttributeMapping.AttributeId = containerRelationshipTypeAttributeMapping.OriginalContainerRelationshipTypeAttributeMapping.AttributeId;
                    containerRelationshipTypeAttributeMapping.AttributeParentId = containerRelationshipTypeAttributeMapping.OriginalContainerRelationshipTypeAttributeMapping.AttributeParentId;
                }
                else
                {
                    containerRelationshipTypeAttributeMapping.OrganizationId = DataModelHelper.GetOrganizationId(_iOrganizationManager, containerRelationshipTypeAttributeMapping.OrganizationName, ref organizations, callerContext);
                    containerRelationshipTypeAttributeMapping.ContainerId = DataModelHelper.GetContainerId(_iContainerManager, containerRelationshipTypeAttributeMapping.ContainerName, containerRelationshipTypeAttributeMapping.OrganizationName, ref containers, callerContext);
                    containerRelationshipTypeAttributeMapping.RelationshipTypeId = DataModelHelper.GetRelationshipTypeId(_iRelationshipTypeManager, containerRelationshipTypeAttributeMapping.RelationshipTypeName, ref relationshipTypes, callerContext);
                    AttributeModel attributeModel = DataModelHelper.GetAttributeModel(_iAttributeModelManager, containerRelationshipTypeAttributeMapping.AttributeId, containerRelationshipTypeAttributeMapping.AttributeName, containerRelationshipTypeAttributeMapping.AttributeParentName, ref attributeModels, callerContext);

                    if (attributeModel != null)
                    {
                        containerRelationshipTypeAttributeMapping.AttributeId = attributeModel.Id;
                        containerRelationshipTypeAttributeMapping.AttributeParentId = attributeModel.AttributeParentId;
                    }
                }
            }
        }

        #endregion

        #region Process Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerRelationshipTypeAttributeMappings"></param>
        /// <param name="operationResults"></param>
        /// <param name="callerContext"></param>
        private void CompareAndMerge(ContainerRelationshipTypeAttributeMappingCollection containerRelationshipTypeAttributeMappings, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            foreach (ContainerRelationshipTypeAttributeMapping deltaContainerRelationshipTypeAttributeMapping in containerRelationshipTypeAttributeMappings)
            {
                IDataModelOperationResult operationResult = operationResults.GetByReferenceId(deltaContainerRelationshipTypeAttributeMapping.ReferenceId);

                //If delta object Action is Read/Ignore then skip Merge Delta Process.
                if (deltaContainerRelationshipTypeAttributeMapping.Action == ObjectAction.Read || deltaContainerRelationshipTypeAttributeMapping.Action == ObjectAction.Ignore)
                    continue;

                IContainerRelationshipTypeAttributeMapping originalContainerRelationshipTypeAttributeMapping = deltaContainerRelationshipTypeAttributeMapping.OriginalContainerRelationshipTypeAttributeMapping;

                if (originalContainerRelationshipTypeAttributeMapping != null)
                {
                    //If delta object Action is Delete then skip Merge Delta process.
                    if (deltaContainerRelationshipTypeAttributeMapping.Action != ObjectAction.Delete)
                    {
                        originalContainerRelationshipTypeAttributeMapping.MergeDelta(deltaContainerRelationshipTypeAttributeMapping, callerContext, false);
                    }
                }
                else
                {
                    Object[] parameters = null;
                    if (deltaContainerRelationshipTypeAttributeMapping.Action == ObjectAction.Delete)
                    {
                        parameters = new Object[] { "", deltaContainerRelationshipTypeAttributeMapping.ContainerName, deltaContainerRelationshipTypeAttributeMapping.RelationshipTypeName, deltaContainerRelationshipTypeAttributeMapping.AttributeName, deltaContainerRelationshipTypeAttributeMapping.AttributeParentName };
                        DataModelHelper.AddOperationResult(operationResult, "113616", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                    else
                    {
                        if (deltaContainerRelationshipTypeAttributeMapping.ContainerId < 1 || deltaContainerRelationshipTypeAttributeMapping.RelationshipTypeId < 1 || deltaContainerRelationshipTypeAttributeMapping.AttributeId < 1 || deltaContainerRelationshipTypeAttributeMapping.AttributeParentId < 1)
                        {
                            parameters = new Object[] { deltaContainerRelationshipTypeAttributeMapping.ContainerName, deltaContainerRelationshipTypeAttributeMapping.RelationshipTypeName, deltaContainerRelationshipTypeAttributeMapping.AttributeName, deltaContainerRelationshipTypeAttributeMapping.AttributeParentName };
                            DataModelHelper.AddOperationResult(operationResult, "113686", String.Empty, parameters, OperationResultType.Error, TraceEventType.Error, callerContext);
                        }
                        deltaContainerRelationshipTypeAttributeMapping.Action = ObjectAction.Create;
                    }
                }
            }
        }

        #endregion

        #region EntityCacheLoadContext For Container RelationshipType Attribute Mapping

        /// <summary>
        /// Processes the EntityCacheLoadContext for Container Relationship type Attribute mapping change.
        /// </summary>
        private void ProcessEntityCacheLoadContextForMappingChange(ContainerRelationshipTypeAttributeMappingCollection containerRelationshipTypeAttributeMappings)
        {
            Collection<Tuple<Int32, Int32, Collection<Int32>>> entityCacheContextRequest =
                BuildContainerContainerRelationshipTypeAttributeMappingList(containerRelationshipTypeAttributeMappings);

            if (entityCacheContextRequest.Count > 0)
            {
                // Create EntityCacheLoadContext entityCacheContextRequest
                EntityCacheLoadContext entityCacheLoadContext = new EntityCacheLoadContext();
                entityCacheLoadContext.CacheStatus = (Int32)(EntityCacheComponentEnum.Relationships);

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
        /// 
        /// </summary>
        /// <param name="entityCacheLoadContext"></param>
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
        /// Splits the ContainerRelationshipTypeAttributeMappingCollection into a collection of tuple containing the container id, relationship type id and attribute id list.
        /// </summary>
        private Collection<Tuple<Int32, Int32, Collection<Int32>>> BuildContainerContainerRelationshipTypeAttributeMappingList(ContainerRelationshipTypeAttributeMappingCollection containerRelationshipTypeAttributeMappings)
        {
            Collection<Tuple<Int32, Int32, Collection<Int32>>> entityCacheContextRequest = new Collection<Tuple<Int32, Int32, Collection<Int32>>>();

            foreach (ContainerRelationshipTypeAttributeMapping containerRelationshipTypeAttributeMapping in containerRelationshipTypeAttributeMappings)
            {
                if (containerRelationshipTypeAttributeMapping.Action == ObjectAction.Create || containerRelationshipTypeAttributeMapping.Action == ObjectAction.Delete)
                {
                    Collection<Int32> attributeIdList = GetAttributeIdList(entityCacheContextRequest, containerRelationshipTypeAttributeMapping.ContainerId,
                        containerRelationshipTypeAttributeMapping.RelationshipTypeId);
                    if (attributeIdList != null)
                    {
                        attributeIdList.Add(containerRelationshipTypeAttributeMapping.AttributeId);
                    }
                    else
                    {
                        Tuple<Int32, Int32, Collection<Int32>> entityCacheContextRequestItem = new Tuple<Int32, Int32, Collection<Int32>>(
                            containerRelationshipTypeAttributeMapping.ContainerId, containerRelationshipTypeAttributeMapping.RelationshipTypeId,
                            new Collection<Int32>() { containerRelationshipTypeAttributeMapping.AttributeId });
                        entityCacheContextRequest.Add(entityCacheContextRequestItem);
                    }
                }
            }

            return entityCacheContextRequest;
        }

        /// <summary>
        /// Returns the Attribute id list for the specified Container Id and Relationship type id.
        /// </summary>
        private Collection<Int32> GetAttributeIdList(Collection<Tuple<Int32, Int32, Collection<Int32>>> entityCacheContextRequest, Int32 containerId, Int32 relationshipTypeId)
        {
            foreach (Tuple<Int32, Int32, Collection<Int32>> entityCacheContextRequestItem in entityCacheContextRequest)
            {
                if (entityCacheContextRequestItem.Item1 == containerId && entityCacheContextRequestItem.Item2 == relationshipTypeId)
                {
                    return entityCacheContextRequestItem.Item3;
                }
            }
            return null;
        }

        /// <summary>
        /// Creates an EntityCacheLoadContextItemCollection for the specified container id, relationship type id and attribute id list.
        /// </summary>
        private EntityCacheLoadContextItemCollection GetEntityCacheLoadContextItemCollection(Int32 containerId, Int32 relationshipTypeId, Collection<Int32> attributeIdList)
        {
            // Create EntityCacheLoadContextItemCollection
            EntityCacheLoadContextItemCollection entityCacheLoadContextItemCollection = new EntityCacheLoadContextItemCollection();

            // Create EntityCacheLoadContextItem for Container
            EntityCacheLoadContextItem entityCacheLoadContextItemForContainer =
                entityCacheLoadContextItemCollection.GetOrCreateEntityCacheLoadContextItem(EntityCacheLoadContextTypeEnum.Container);
            entityCacheLoadContextItemForContainer.AddValues(containerId);

            // Create EntityCacheLoadContextItem for Relationship Type
            EntityCacheLoadContextItem entityCacheLoadContextItemForEntityType =
                entityCacheLoadContextItemCollection.GetOrCreateEntityCacheLoadContextItem(EntityCacheLoadContextTypeEnum.RelationshipType);
            entityCacheLoadContextItemForEntityType.AddValues(relationshipTypeId);

            // Create EntityCacheLoadContextItem for Attributes
            EntityCacheLoadContextItem entityCacheLoadContextItemForAttributes =
                entityCacheLoadContextItemCollection.GetOrCreateEntityCacheLoadContextItem(EntityCacheLoadContextTypeEnum.Attribute);
            entityCacheLoadContextItemForAttributes.AddValues(attributeIdList);

            return entityCacheLoadContextItemCollection;
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
                callerContext.ProgramName = "ContainerRelationshipTypeAttributeMappingBL";
            }
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private IEnumerable<KeyValuePair<Int32, Int32>> GetUniqueContainerRelationshipTypeMappings(CallerContext callerContext)
        {
            Dictionary<String, KeyValuePair<Int32, Int32>> uniqueContainerRelationshipTypeMappings = new Dictionary<String, KeyValuePair<Int32, Int32>>();

            Int32 containerId = -1;
            Int32 relationshipTypeId = -1;
            String key = String.Empty;

            ContainerRelationshipTypeEntityTypeMappingCollection mappingCollection = new ContainerRelationshipTypeEntityTypeMappingBL().GetAll(callerContext);

            foreach (ContainerRelationshipTypeEntityTypeMapping mapping in mappingCollection)
            {
                containerId = mapping.ContainerId;
                relationshipTypeId = mapping.RelationshipTypeId;

                key = String.Format("{0}_{1}", containerId, relationshipTypeId);

                if (!uniqueContainerRelationshipTypeMappings.ContainsKey(key))
                {
                    KeyValuePair<Int32, Int32> value = new KeyValuePair<Int32, Int32>(containerId, relationshipTypeId);
                    uniqueContainerRelationshipTypeMappings.Add(key, value);
                }
            }

            return uniqueContainerRelationshipTypeMappings.Values;
        }

        /// <summary>
        /// Log changes to tb_datamodelactivitylog if DA call is sucessfull and appconfig key datamodelactivitylog.enable == true
        /// </summary>
        /// <param name="containerRelationshipTypeAttributeMappingCollection"></param>
        /// <param name="callerContext"></param>
        private void LogDataModelChanges(ContainerRelationshipTypeAttributeMappingCollection containerRelationshipTypeAttributeMappingCollection, CallerContext callerContext)
        {
            #region step: populate datamodelactivitylog object

            DataModelActivityLogBL dataModelActivityLogManager = new DataModelActivityLogBL();
            DataModelActivityLogCollection activityLogCollection = dataModelActivityLogManager.FillDataModelActivityLogCollection(containerRelationshipTypeAttributeMappingCollection);

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