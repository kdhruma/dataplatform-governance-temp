using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Transactions;

namespace MDM.DataModelManager.Business
{
    using MDM.ActivityLogManager.Business;
    using MDM.BufferManager;
    using MDM.BusinessObjects;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.DataModelManager.Data;
    using MDM.Utility;
    using MDM.Interfaces;
    using MDM.MessageManager.Business;
    using MDM.BusinessObjects.DataModel;
    using MDM.Core.Exceptions;
    using MDM.CacheManager.Business;

    /// <summary>
    /// Represents business logic for RelationshipType Attribute mapping
    /// </summary>
    public class RelationshipTypeAttributeMappingBL : BusinessLogicBase, IDataModelManager
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
        ///  Field denoting theIRelationshipTypeManager
        /// </summary>
        private IRelationshipTypeManager _iRelationshipTypeManager = null;

        /// <summary>
        /// Field denoting the IAttributeModelManager
        /// </summary>
        private IAttributeModelManager _iAttributeModelManager = null;

        /// <summary>
        /// Field denoting the IContainerManager
        /// </summary>
        private IContainerManager _iContainerManager = null;

        /// <summary>
        /// Specifies mapping buffer manager
        /// </summary>
        private MappingBufferManager _mappingBufferManager = new MappingBufferManager();

        /// <summary>
        /// Lookup data access object
        /// </summary>
        private RelationshipTypeAttributeMappingDA _relationshipTypeAttributeMappingDA = new RelationshipTypeAttributeMappingDA();

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for RelationshipTypeAttributeMappingBL
        /// </summary>
        public RelationshipTypeAttributeMappingBL()
        {
            GetSecurityPrincipal();
        }

        /// <summary>
        /// Constructor for RelationshipTypeAttributeMappingBL
        /// </summary>
        /// <param name="iRelationshipTypeManager">Indicates instance of relationship type BL</param>
        /// <param name="iAttributeModelManager">Indicates instance of attribute model BL</param>
        /// <param name="iContainerManager">Indicates instance of container BL</param>
        public RelationshipTypeAttributeMappingBL(IRelationshipTypeManager iRelationshipTypeManager, IAttributeModelManager iAttributeModelManager, IContainerManager iContainerManager)
        {
            this._iRelationshipTypeManager = iRelationshipTypeManager;
            this._iAttributeModelManager = iAttributeModelManager;
            this._iContainerManager = iContainerManager;
            GetSecurityPrincipal();
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region Public Methods

        #region CUD Methods

        /// <summary>
        /// Add new relationshipType - attribute mapping.
        /// </summary>
        /// <param name="relationshipTypeAttributeMapping"></param>
        /// <returns></returns>
        public Boolean Create(RelationshipTypeAttributeMapping relationshipTypeAttributeMapping)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update relationshipType - attribute mapping
        /// </summary>
        /// <param name="relationshipTypeAttributeMapping"></param>
        /// <returns></returns>
        public Boolean Update(RelationshipTypeAttributeMapping relationshipTypeAttributeMapping)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Remove existing relationshipType - attribute mapping
        /// </summary>
        /// <param name="relationshipTypeAttributeMapping"></param>
        /// <returns></returns>
        public Boolean Delete(RelationshipTypeAttributeMapping relationshipTypeAttributeMapping)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create, Update or Delete RelationshipType attribute mappings
        /// </summary>
        /// <param name="relationshipTypeAttributeMappings">RelationshipType attribute mappings to process</param>
        /// <returns>Result of the operation</returns>
        public OperationResult Process(RelationshipTypeAttributeMappingCollection relationshipTypeAttributeMappings)
        {
            OperationResult operationResult = new OperationResult();
            operationResult.OperationResultStatus = OperationResultStatusEnum.Failed;

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("RelationshipTypeAttributeMappingBL.Process", MDMTraceSource.DataModel, false);

                IEnumerable<Int32> attributeIds = relationshipTypeAttributeMappings.Where(mapping => mapping.Action != ObjectAction.Delete).Select(mapping => mapping.AttributeId);
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

                    RelationshipTypeAttributeMappingDA relationshipTypeAttributeMappingDA = new RelationshipTypeAttributeMappingDA();
                    relationshipTypeAttributeMappingDA.Process(relationshipTypeAttributeMappings, userName, "RelationshipTypeAttributeMappingBL");

                    ProcessEntityCacheLoadContextForMappingChange(relationshipTypeAttributeMappings);

                    transactionScope.Complete();
                    operationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                }

                #region activity log

                if (operationResult.OperationResultStatus == OperationResultStatusEnum.Successful && AppConfigurationHelper.GetAppConfig<Boolean>(Constants.ACTIVITYLOG_DATAMODEL_ENABLED_APPCONFIG_KEY, false))
                {
                    LogDataModelChanges(relationshipTypeAttributeMappings, new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.Modeling, "RelationshipTypeAttributeMappingBL.ProcessDataModelActivityLog"));
                }

                #endregion activity log

                #region Invalidate relationshiptype attribute mappings

                Task.Factory.StartNew(() => InvalidateImpactedDataAsync(relationshipTypeAttributeMappings, _iContainerManager, OperationContext.Current));

                #endregion

                 _mappingBufferManager.NotifyModelDataChange(CacheKeyContants.RELATIONSHIP_HIERARCHY_CHANGED_KEY);
                
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("RelationshipTypeAttributeMappingBL.Process", MDMTraceSource.DataModel);
            }

            return operationResult;
        }

        #endregion

        #region Get Methods

        /// <summary>
        /// Get relationshipType - attribute mapping by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RelationshipTypeAttributeMapping GetById(Int32 id)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Gets RelationshipType attribute mappings from the system based on relationshpiypeId and attributeGroupId
        /// </summary>
        /// <param name="relationshipTypeId">Indicates the RelationshipType Id for which mappings needs to be fetched</param>
        /// <param name="attributeGroupId">Indicates the AttributeGroup Id for which mappings needs to be fetched</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>RelationshipType attribute mappings for specified relationshipType Id and attributeGroup Id</returns>
        public RelationshipTypeAttributeMappingCollection Get(Int32 relationshipTypeId, Int32 attributeGroupId, CallerContext callerContext)
        {
            return Get(relationshipTypeId, attributeGroupId, false, callerContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationshipTypeId"></param>
        /// <param name="attributeGroupId"></param>
        /// <param name="getLatest"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public RelationshipTypeAttributeMappingCollection Get(Int32 relationshipTypeId, Int32 attributeGroupId, Boolean getLatest, CallerContext callerContext)
        {
            RelationshipTypeAttributeMappingCollection relationshipTypeAttributeMappings = null;

            try
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StartTraceActivity("RelationshipTypeAttributeMappingBL.Get", MDMTraceSource.DataModel, false);
                }

            #region Initial Setup

            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);
            //TODO: Relationship type attribute mappings are not cached.This flag is required for future used.
            getLatest = true;

            #endregion

            #region Get Mappings from Cache if available

            if (!getLatest)
            {
                relationshipTypeAttributeMappings = _mappingBufferManager.FindRelationshipAttributeMappings(relationshipTypeId);
            }

            #endregion

            #region Get Mappings from Database

            if (relationshipTypeAttributeMappings == null)
            {
                relationshipTypeAttributeMappings = _relationshipTypeAttributeMappingDA.Get(relationshipTypeId, attributeGroupId, -1, command);

                #region Cache Mappings data

                //if (relationshipTypeAttributeMappings != null && relationshipTypeAttributeMappings.Count > 0)
                //{
                //    _mappingBufferManager.UpdateRelationshipAttributeMappings(relationshipTypeAttributeMappings, relationshipTypeId, attributeModelType, systerDataLocale, 3);
                //}

                #endregion
            }

            #endregion
                        
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("RelationshipTypeAttributeMappingBL.Get", MDMTraceSource.DataModel);
            }

            return relationshipTypeAttributeMappings;
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
                {
                    MDMTraceHelper.StartTraceActivity("RelationshipTypeAttributeMappingBL.PrepareOperationResultsSchema", MDMTraceSource.DataModel, false);
                }

                String separator = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.RSDataModelExcel.Separator", "//").Trim();
                RelationshipTypeAttributeMappingCollection relationshipTypeAttributeMappings = iDataModelObjects as RelationshipTypeAttributeMappingCollection;

                if (relationshipTypeAttributeMappings != null && relationshipTypeAttributeMappings.Count > 0)
                {
                    if (operationResults.Count > 0)
                    {
                        operationResults.Clear();
                    }

                    Int32 relationshipTypeAttributeMappingIdToBeCreated = -1;

                    foreach (RelationshipTypeAttributeMapping relationshipTypeAttributeMapping in relationshipTypeAttributeMappings)
                    {
                        DataModelOperationResult relationshipTypeAttributeMappingOperationResult = new DataModelOperationResult(relationshipTypeAttributeMapping.Id, relationshipTypeAttributeMapping.LongName, relationshipTypeAttributeMapping.ExternalId, relationshipTypeAttributeMapping.ReferenceId);

                        if (String.IsNullOrEmpty(relationshipTypeAttributeMappingOperationResult.ExternalId))
                        {
                            relationshipTypeAttributeMappingOperationResult.ExternalId = String.Format("{1} {0} {2} {0} {3}", separator, relationshipTypeAttributeMapping.RelationshipTypeName, relationshipTypeAttributeMapping.AttributeParentName, relationshipTypeAttributeMapping.AttributeName);
                        }

                        if (relationshipTypeAttributeMapping.Id < 1)
                        {
                            relationshipTypeAttributeMapping.Id = relationshipTypeAttributeMappingIdToBeCreated;
                            relationshipTypeAttributeMappingOperationResult.Id = relationshipTypeAttributeMappingIdToBeCreated;
                            relationshipTypeAttributeMappingIdToBeCreated--;
                        }

                        operationResults.Add(relationshipTypeAttributeMappingOperationResult);
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
                    MDMTraceHelper.StopTraceActivity("RelationshipTypeAttributeMappingBL.PrepareOperationResultsSchema", MDMTraceSource.DataModel);
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
            RelationshipTypeAttributeMappingCollection relationshipTypeAttributeMappings = iDataModelObjects as RelationshipTypeAttributeMappingCollection;
            CallerContext callerContext = iCallerContext as CallerContext;

            try
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StartTraceActivity("RelationshipTypeAttributeMappingBL.Validate", MDMTraceSource.DataModel, false);
                }

                #region Parameter Validation

                ValidateInputParameters(relationshipTypeAttributeMappings, operationResults, callerContext);

                #endregion

                #region Data Validation

                ValidateData(relationshipTypeAttributeMappings, operationResults, callerContext);

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
                    MDMTraceHelper.StopTraceActivity("RelationshipTypeAttributeMappingBL.Validate", MDMTraceSource.DataModel);
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
                    MDMTraceHelper.StartTraceActivity("RelationshipTypeAttributeMappingBL.LoadOriginal", MDMTraceSource.DataModel, false);

                LoadOriginalRelationshipTypeAttributeMapping(iDataModelObjects as RelationshipTypeAttributeMappingCollection, iCallerContext as CallerContext);
            }
            catch (Exception ex)
            {
                DataModelHelper.AddOperationResults(operationResults, String.Empty, ex.Message, null, OperationResultType.Error, TraceEventType.Error, iCallerContext as CallerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("RelationshipTypeAttributeMappingBL.LoadOriginal", MDMTraceSource.DataModel);
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
                    MDMTraceHelper.StartTraceActivity("RelationshipTypeAttributeMappingBL.FillDataModel", MDMTraceSource.DataModel, false);

                FillRelationshipTypeAttributeMapping(iDataModelObjects as RelationshipTypeAttributeMappingCollection, iCallerContext as CallerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("RelationshipTypeAttributeMappingBL.FillDataModel", MDMTraceSource.DataModel);
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
                    MDMTraceHelper.StartTraceActivity("RelationshipTypeAttributeMappingBL.CompareAndMerge", MDMTraceSource.DataModel, false);

                CompareAndMerge(iDataModelObjects as RelationshipTypeAttributeMappingCollection, operationResults, iCallerContext as CallerContext);
            }
            catch (Exception ex)
            {
                DataModelHelper.AddOperationResults(operationResults, String.Empty, ex.Message, null, OperationResultType.Error, TraceEventType.Error, iCallerContext as CallerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("RelationshipTypeAttributeMappingBL.CompareAndMerge", MDMTraceSource.DataModel);
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
                    MDMTraceHelper.StartTraceActivity("RelationshipTypeAttributeMappingBL.Process", MDMTraceSource.DataModel, false);

                RelationshipTypeAttributeMappingCollection relationshipTypeAttributeMappings = iDataModelObjects as RelationshipTypeAttributeMappingCollection;
                CallerContext callerContext = iCallerContext as CallerContext;

                if (relationshipTypeAttributeMappings.Count > 0)
                {
                    PopulateProgramName(callerContext);

                    DBCommandProperties command = DBCommandHelper.Get((CallerContext)iCallerContext, MDMCenterModuleAction.Execute);

                    #region Perform RelationshipTypeAttributeMapping updates in database

                    using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                    {
                        _relationshipTypeAttributeMappingDA.Process(relationshipTypeAttributeMappings, operationResults, _securityPrincipal.CurrentUserName, callerContext.ProgramName);
                        ProcessEntityCacheLoadContextForMappingChange(relationshipTypeAttributeMappings);

                        transactionScope.Complete();
                    }

                    LocalizeErrors(callerContext, operationResults);

                    #endregion

                    #region activity log

                    if (operationResults.OperationResultStatus == OperationResultStatusEnum.Successful && AppConfigurationHelper.GetAppConfig<Boolean>(Constants.ACTIVITYLOG_DATAMODEL_ENABLED_APPCONFIG_KEY, false))
                    {
                        LogDataModelChanges(relationshipTypeAttributeMappings, callerContext);
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
                    MDMTraceHelper.StopTraceActivity("RelationshipTypeAttributeMappingBL.Process", MDMTraceSource.DataModel);
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
                    MDMTraceHelper.StartTraceActivity("RelationshipTypeAttributeMappingBL.InvalidateCache", MDMTraceSource.DataModel, false);

                RelationshipTypeAttributeMappingCollection relationshipTypeAttributeMappings = iDataModelObjects as RelationshipTypeAttributeMappingCollection;

                foreach (RelationshipTypeAttributeMapping relationshipTypeAttributeMapping in relationshipTypeAttributeMappings)
                {
                    // clear buffer manager
                    _mappingBufferManager.RemoveRelationshipAttributeMappings(relationshipTypeAttributeMapping.RelationshipTypeId, false);
                }

                #region Invalidate relationshiptype attribute mappings

                Task.Factory.StartNew(() => InvalidateImpactedDataAsync(relationshipTypeAttributeMappings, this._iContainerManager, OperationContext.Current));

                #endregion

                _mappingBufferManager.NotifyModelDataChange(CacheKeyContants.RELATIONSHIP_HIERARCHY_CHANGED_KEY);
            }
            catch (Exception ex)
            {
                DataModelHelper.AddOperationResults(operationResults, String.Empty, ex.Message, null, OperationResultType.Error, TraceEventType.Error, iCallerContext as CallerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("RelationshipTypeAttributeMappingBL.InvalidateCache", MDMTraceSource.DataModel);
            }
        }

        #endregion

        #endregion

        #region Private Methods

        #region Validation Methods

        /// <summary>
        /// Validate the input parameters in relationship type attribute mapping collection
        /// </summary>
        /// <param name="relationshipTypeAttributeMappings">Indicates the collection of relationship type attribute mapping</param>
        /// <param name="callerContext">Indicates the context of the caller making call to this API</param>
        private void ValidateInputParameters(RelationshipTypeAttributeMappingCollection relationshipTypeAttributeMappings, CallerContext callerContext)
        {
            String errorMessage;

            if (relationshipTypeAttributeMappings == null || relationshipTypeAttributeMappings.Count < 1)
            {
                errorMessage = "RelationshipTypeAttributeMapping Collection cannot be null.";
                throw new MDMOperationException("", errorMessage, "DataModelManager.RelationshipTypeAttributeMappingBL", String.Empty, "Create");
            }

            if (callerContext == null)
            {
                errorMessage = "CallerContext cannot be null.";
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "DataModelManager.RelationshipTypeAttributeMappingBL", String.Empty, "Create");
            }
        }

        /// <summary>
        /// Validate the input parameters in relationship type attribute mapping collection
        /// </summary>
        /// <param name="relationshipTypeAttributeMappings">Indicates the collection of relationship type attribute mapping</param>
        /// <param name="operationResults">Indicates the result of operation</param>
        /// <param name="callerContext">Indicates the context of the caller</param>
        private void ValidateInputParameters(RelationshipTypeAttributeMappingCollection relationshipTypeAttributeMappings, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            if (relationshipTypeAttributeMappings == null || relationshipTypeAttributeMappings.Count < 1)
            {
                DataModelHelper.AddOperationResults(operationResults, "113645", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }

            if (callerContext == null)
            {
                DataModelHelper.AddOperationResults(operationResults, "111846", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
        }

        /// <summary>
        /// Validate the data of specified parameters in relationship type attribute mapping collection
        /// </summary>
        /// <param name="relationshipTypeAttributeMappings">Indicates the collection of relationship type attribute mapping</param>
        /// <param name="operationResults">Indicates the result of operation</param>
        /// <param name="callerContext">Indicates the context of the caller</param>
        private void ValidateData(RelationshipTypeAttributeMappingCollection relationshipTypeAttributeMappings, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            if (relationshipTypeAttributeMappings != null && relationshipTypeAttributeMappings.Count > 0)
            {
                Collection<String> relationshipTypeAttributeMappingList = new Collection<String>();
                AttributeModelCollection attributeModels = new AttributeModelCollection();

                foreach (RelationshipTypeAttributeMapping relationshipTypeAttributeMapping in relationshipTypeAttributeMappings)
                {
                    IDataModelOperationResult dataModelOperationResult = operationResults.GetByReferenceId(relationshipTypeAttributeMapping.ReferenceId);
                    AttributeModel attributeModel = DataModelHelper.GetAttributeModel(_iAttributeModelManager, relationshipTypeAttributeMapping.AttributeId, relationshipTypeAttributeMapping.AttributeName, relationshipTypeAttributeMapping.AttributeParentName, ref attributeModels, callerContext);

                    if (attributeModel != null && attributeModel.AttributeModelType != AttributeModelType.Relationship)
                    {
                        String errorMessage =
                            "Attribute '{0}' under '{1}' group is of the type '{2}', so mapping cannot be performed for relationship type '{3}'.";
                        Object[] parameters = new Object[]
                        {
                            relationshipTypeAttributeMapping.AttributeName,
                            relationshipTypeAttributeMapping.AttributeParentName,
                            attributeModel.AttributeModelType.ToString().ToLowerInvariant(),
                            relationshipTypeAttributeMapping.RelationshipTypeName
                        };

                        DataModelHelper.AddOperationResult(dataModelOperationResult, "ZZZZ",
                            String.Format(errorMessage, parameters), parameters, OperationResultType.Error,
                            TraceEventType.Error, callerContext);
                    }
                    else
                    {
                        Collection<String> uniqueIdentifierList = new Collection<String>()
                        {
                            relationshipTypeAttributeMapping.RelationshipTypeName,
                            relationshipTypeAttributeMapping.AttributeName,
                            relationshipTypeAttributeMapping.AttributeParentName
                        };

                        String rowUniqueIdentifier = DataModelHelper.GetUniqueIdentifier(uniqueIdentifierList);

                        DataModelHelper.ValidateRelationshipTypeName(
                            relationshipTypeAttributeMapping.RelationshipTypeName,
                            dataModelOperationResult, callerContext);

                        DataModelHelper.ValidateAttributeUniqueIdentifier(
                            relationshipTypeAttributeMapping.AttributeName,
                            relationshipTypeAttributeMapping.AttributeParentName,
                            dataModelOperationResult, callerContext);

                        if (relationshipTypeAttributeMappingList.Contains(rowUniqueIdentifier))
                        {
                            Object[] parameters = null;
                            parameters = new Object[]
                            {
                                relationshipTypeAttributeMapping.RelationshipTypeName,
                                relationshipTypeAttributeMapping.AttributeName,
                                relationshipTypeAttributeMapping.AttributeParentName
                            };

                            String errorMessage =
                                String.Format(
                                    "Duplicate mappings found for the relationship type attribute mapping with relationship type: {0}, attribute: {1}, and attribute parent: {2}",
                                    parameters);

                            DataModelHelper.AddOperationResult(dataModelOperationResult, "113968", errorMessage,
                                parameters,
                                OperationResultType.Error, TraceEventType.Error, callerContext);
                        }
                        else
                        {
                            relationshipTypeAttributeMappingList.Add(rowUniqueIdentifier);
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
        /// <param name="relationshipTypeAttributeMappings"></param>
        /// <param name="callerContext"></param>
        private void LoadOriginalRelationshipTypeAttributeMapping(RelationshipTypeAttributeMappingCollection relationshipTypeAttributeMappings, CallerContext callerContext)
        {
            RelationshipTypeAttributeMappingCollection orginalRelationshipTypeAttributeMappingCollection = Get(-1, -1, true, callerContext);

            if (orginalRelationshipTypeAttributeMappingCollection != null && orginalRelationshipTypeAttributeMappingCollection.Count > 0)
            {
                foreach (RelationshipTypeAttributeMapping relationshipTypeAttributeMapping in relationshipTypeAttributeMappings)
                {
                    relationshipTypeAttributeMapping.OriginalRelationshipTypeAttributeMapping = orginalRelationshipTypeAttributeMappingCollection.Get(relationshipTypeAttributeMapping.RelationshipTypeName, relationshipTypeAttributeMapping.AttributeName, relationshipTypeAttributeMapping.AttributeParentName);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationshipTypeAttributeMappings"></param>
        /// <param name="callerContext"></param>
        private void FillRelationshipTypeAttributeMapping(RelationshipTypeAttributeMappingCollection relationshipTypeAttributeMappings, CallerContext callerContext)
        {
            RelationshipTypeCollection relationshipTypes = new RelationshipTypeCollection();
            AttributeModelCollection attributeModels = new AttributeModelCollection();
            foreach (RelationshipTypeAttributeMapping relationshipTypeAttributeMapping in relationshipTypeAttributeMappings)
            {
                if (relationshipTypeAttributeMapping.OriginalRelationshipTypeAttributeMapping != null)
                {
                    relationshipTypeAttributeMapping.Id = relationshipTypeAttributeMapping.OriginalRelationshipTypeAttributeMapping.Id;
                    relationshipTypeAttributeMapping.RelationshipTypeId = relationshipTypeAttributeMapping.OriginalRelationshipTypeAttributeMapping.RelationshipTypeId;
                    relationshipTypeAttributeMapping.AttributeId = relationshipTypeAttributeMapping.OriginalRelationshipTypeAttributeMapping.AttributeId;
                    relationshipTypeAttributeMapping.AttributeParentId = relationshipTypeAttributeMapping.OriginalRelationshipTypeAttributeMapping.AttributeParentId;
                }
                else
                {
                    relationshipTypeAttributeMapping.RelationshipTypeId = DataModelHelper.GetRelationshipTypeId(_iRelationshipTypeManager, relationshipTypeAttributeMapping.RelationshipTypeName, ref relationshipTypes, callerContext);
                    AttributeModel attributeModel = DataModelHelper.GetAttributeModel(_iAttributeModelManager, relationshipTypeAttributeMapping.AttributeId, relationshipTypeAttributeMapping.AttributeName, relationshipTypeAttributeMapping.AttributeParentName, ref attributeModels, callerContext);

                    if (attributeModel != null)
                    {
                        relationshipTypeAttributeMapping.AttributeId = attributeModel.Id;
                        relationshipTypeAttributeMapping.AttributeParentId = attributeModel.AttributeParentId;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationshipTypeAttributeMappings"></param>
        /// <param name="operationResults"></param>
        /// <param name="callerContext"></param>
        private void CompareAndMerge(RelationshipTypeAttributeMappingCollection relationshipTypeAttributeMappings, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            foreach (RelationshipTypeAttributeMapping deltaRelationshipTypeAttributeMapping in relationshipTypeAttributeMappings)
            {
                IDataModelOperationResult operationResult = operationResults.GetByReferenceId(deltaRelationshipTypeAttributeMapping.ReferenceId);

                //If delta object Action is Read/Ignore then skip Merge Delta Process.
                if (deltaRelationshipTypeAttributeMapping.Action == ObjectAction.Read || deltaRelationshipTypeAttributeMapping.Action == ObjectAction.Ignore)
                    continue;

                IRelationshipTypeAttributeMapping originalRelationshipTypeAttributeMapping = deltaRelationshipTypeAttributeMapping.OriginalRelationshipTypeAttributeMapping;

                if (originalRelationshipTypeAttributeMapping != null)
                {
                    //If delta object Action is Delete then skip Merge Delta process.
                    if (deltaRelationshipTypeAttributeMapping.Action != ObjectAction.Delete)
                    {
                        originalRelationshipTypeAttributeMapping.MergeDelta(deltaRelationshipTypeAttributeMapping, callerContext, false);
                    }
                }
                else
                {
                    Object[] parameters = null;
                    if (deltaRelationshipTypeAttributeMapping.Action == ObjectAction.Delete)
                    {
                        parameters = new Object[] { "", deltaRelationshipTypeAttributeMapping.RelationshipTypeName, deltaRelationshipTypeAttributeMapping.AttributeName, deltaRelationshipTypeAttributeMapping.AttributeParentName };
                        DataModelHelper.AddOperationResult(operationResult, "113608", String.Empty, parameters, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                    else
                    {
                        
                        if (deltaRelationshipTypeAttributeMapping.RelationshipTypeId < 1 || deltaRelationshipTypeAttributeMapping.AttributeId < 1 || deltaRelationshipTypeAttributeMapping.AttributeParentId < 1)
                        {
                            parameters = new Object[] { deltaRelationshipTypeAttributeMapping.RelationshipTypeName, deltaRelationshipTypeAttributeMapping.AttributeName, deltaRelationshipTypeAttributeMapping.AttributeParentName };
                            DataModelHelper.AddOperationResult(operationResult, "113610", String.Empty, parameters, OperationResultType.Error, TraceEventType.Error, callerContext);
                        }
                        //If original object is not found then set Action as Create always.
                        deltaRelationshipTypeAttributeMapping.Action = ObjectAction.Create;
                    }
                }
                operationResult.PerformedAction = deltaRelationshipTypeAttributeMapping.Action;
            }
        }

        #endregion

        #region EntityCacheLoadContext For RelationshipType Attribute Mapping

        /// <summary>
        /// Processes the EntityCacheLoadContext for relationship type mapping change.
        /// </summary>
        private void ProcessEntityCacheLoadContextForMappingChange(RelationshipTypeAttributeMappingCollection relationshipTypeAttributeMappings)
        {
            Dictionary<Int32, Collection<Int32>> entityCacheContextRequest = BuildRelationshipTypeAttributeMappingList(relationshipTypeAttributeMappings);
            if (entityCacheContextRequest.Count > 0)
            {
                // Create EntityCacheLoadContext entityCacheContextRequest
                EntityCacheLoadContext entityCacheLoadContext = new EntityCacheLoadContext();
                entityCacheLoadContext.CacheStatus = (Int32)(EntityCacheComponentEnum.Relationships);

                foreach (KeyValuePair<Int32, Collection<Int32>> item in entityCacheContextRequest)
                {
                    EntityCacheLoadContextItemCollection entityCacheLoadContextItemCollection = GetEntityCacheLoadContextItemCollection(item.Key, item.Value);
                    entityCacheLoadContext.Add(entityCacheLoadContextItemCollection);
                }

                UpdateEntityCacheLoadContextInActivityLog(entityCacheLoadContext.ToXml());
            }
        }

        /// <summary>
        /// Processes the EntityCacheLoadContext for relationship type mapping change.
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
        /// Splits the Collection<RelationshipTypeAttributeMapping> into a dictionary of relationship type id and attribute id list.
        /// </summary>
        private Dictionary<Int32, Collection<Int32>> BuildRelationshipTypeAttributeMappingList(RelationshipTypeAttributeMappingCollection relationshipTypeAttributeMappings)
        {
            Dictionary<Int32, Collection<Int32>> entityCacheContextRequest = new Dictionary<Int32, Collection<Int32>>();

            foreach (RelationshipTypeAttributeMapping relationshipTypeAttributeMapping in relationshipTypeAttributeMappings)
            {
                if (relationshipTypeAttributeMapping.Action == ObjectAction.Create || relationshipTypeAttributeMapping.Action == ObjectAction.Delete)
                {
                    if (entityCacheContextRequest.ContainsKey(relationshipTypeAttributeMapping.RelationshipTypeId))
                    {
                        Collection<Int32> valueList = entityCacheContextRequest[relationshipTypeAttributeMapping.RelationshipTypeId];
                        valueList.Add(relationshipTypeAttributeMapping.AttributeId);
                    }
                    else
                    {
                        entityCacheContextRequest.Add(relationshipTypeAttributeMapping.RelationshipTypeId,
                            new Collection<Int32>() { relationshipTypeAttributeMapping.AttributeId });
                    }
                }
            }

            return entityCacheContextRequest;
        }

        /// <summary>
        /// Creates an EntityCacheLoadContextItemCollection for the specified relationship type id and attribute id list.
        /// </summary>
        private EntityCacheLoadContextItemCollection GetEntityCacheLoadContextItemCollection(Int32 relationshipTypeId, Collection<Int32> attributeIdList)
        {
            // Create EntityCacheLoadContextItemCollection
            EntityCacheLoadContextItemCollection entityCacheLoadContextItemCollection = new EntityCacheLoadContextItemCollection();

            // Create EntityCacheLoadContextItem for Relationship Type
            EntityCacheLoadContextItem entityCacheLoadContextItemForRelationshipType =
                entityCacheLoadContextItemCollection.GetOrCreateEntityCacheLoadContextItem(EntityCacheLoadContextTypeEnum.RelationshipType);
            entityCacheLoadContextItemForRelationshipType.AddValues(relationshipTypeId);

            // Create EntityCacheLoadContextItem for Attributes
            EntityCacheLoadContextItem entityCacheLoadContextItemForAttributes =
                entityCacheLoadContextItemCollection.GetOrCreateEntityCacheLoadContextItem(EntityCacheLoadContextTypeEnum.Attribute);
            entityCacheLoadContextItemForAttributes.AddValues(attributeIdList);

            return entityCacheLoadContextItemCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationshipTypeAttributeMappings"></param>
        /// <param name="operationContext"></param>
        private static void InvalidateImpactedDataAsync(RelationshipTypeAttributeMappingCollection relationshipTypeAttributeMappings, IContainerManager containerManager, OperationContext operationContext)
        {
            try
            {
                if (relationshipTypeAttributeMappings == null)
                    return;

                OperationContext.Current = operationContext;

                //Relationship type Id will be only 1. So we take 1 relationship type id from all the mappings.
                var relationshipTypeAttributeMapping = relationshipTypeAttributeMappings.FirstOrDefault();

                if (relationshipTypeAttributeMapping != null)
                {
                    Int32 relationshipTypeId = relationshipTypeAttributeMapping.RelationshipTypeId;

                    #region Invalidate All Relationship Type Attributes Mappings

                    IEnumerable<Int32> containerIds = new Collection<Int32>();

                    //var dataModelService = new DataModelService();
                    IContainerCollection allContainers = containerManager.GetAll(new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.UIProcess), false);

                    //Now containerIds will have all containers.
                    containerIds = allContainers.Select(con => con.Id);

                    MappingBufferManager mappingBufferManager = new MappingBufferManager();
                    mappingBufferManager.InvalidateImpactedMappings(new Collection<Int32>(containerIds.ToList()), 0, relationshipTypeId, AttributeModelType.Relationship, false);

                    #endregion

                    #region Update Attribute Model Version Number

                    DataModelVersionManager dataModelVersionManager = new DataModelVersionManager();
                    dataModelVersionManager.UpdateAttributeModelsVersionNumber();

                    #endregion
                }
            }
            catch
            {
                throw;
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
                callerContext.ProgramName = "RelationshipTypeAttributeMappingBL";
            }
        }

        /// <summary>
        /// Log changes to tb_datamodelactivitylog if DA call is sucessfull and appconfig key datamodelactivitylog.enable == true
        /// </summary>
        /// <param name="relationshipTypeAttributeMappingCollection"></param>
        /// <param name="callerContext"></param>
        private void LogDataModelChanges(RelationshipTypeAttributeMappingCollection relationshipTypeAttributeMappingCollection, CallerContext callerContext)
        {
            #region step: populate datamodelactivitylog object

            DataModelActivityLogBL dataModelActivityLogManager = new DataModelActivityLogBL();
            DataModelActivityLogCollection activityLogCollection = dataModelActivityLogManager.FillDataModelActivityLogCollection(relationshipTypeAttributeMappingCollection);

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