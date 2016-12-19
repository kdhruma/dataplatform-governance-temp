using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Transactions;
using System.Linq;

namespace MDM.DataModelManager.Business
{
    using MDM.ActivityLogManager.Business;
    using MDM.Interfaces;
    using MDM.BufferManager;
    using MDM.BusinessObjects;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.DataModelManager.Data;
    using MDM.Utility;
    using MDM.BusinessObjects.DataModel;
    using MDM.MessageManager.Business;
    using MDM.Core.Exceptions;

    /// <summary>
    /// Represents business logic for EntityType Attribute mapping
    /// </summary>
    public class EntityTypeAttributeMappingBL : BusinessLogicBase, IDataModelManager
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
        /// Field denoting the IEntityTypeManager
        /// </summary>
        private IEntityTypeManager _iEntityTypeManager = null;

        /// <summary>
        /// Field denoting the IAttributeModelManager
        /// </summary>
        private IAttributeModelManager _iAttributeModelManager = null;

        /// <summary>
        /// Field denoting the IContainerManager
        /// </summary>
        private IContainerManager _iContainerManager = null;

        /// <summary>
        /// EntityTypeAttributeMapping data access object
        /// </summary>
        private EntityTypeAttributeMappingDA _entityTypeAttributeMappingDA = new EntityTypeAttributeMappingDA();

        #endregion

        #region Constructors
        /// <summary>
        /// Constructor for EntityTypeAttributeMappingBL
        /// </summary>
        public EntityTypeAttributeMappingBL()
        {
            GetSecurityPrincipal();
        }

        /// <summary>
        /// Constructor for EntityTypeAttributeMappingBL
        /// </summary>
        /// <param name="iEntityTypeManager">Indicates instance of entity type BL</param>
        /// <param name="iAttributeModelManager">Indicates instance of attribute model BL</param>
        /// <param name="iContainerManager">Indicates instance of container BL</param>
        public EntityTypeAttributeMappingBL(IEntityTypeManager iEntityTypeManager, IAttributeModelManager iAttributeModelManager, IContainerManager iContainerManager)
        {
            this._iEntityTypeManager = iEntityTypeManager;
            this._iAttributeModelManager = iAttributeModelManager;
            this._iContainerManager = iContainerManager;

            GetSecurityPrincipal();
        }

        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        #region GetMethod

        /// <summary>
        /// Get All Entity Type Attribute Mappings
        /// </summary>
        /// <param name="callerContext">Indicates the context of caller which called this API</param>
        /// <returns>Returns all the entity type attribute mapping collection</returns>
        public EntityTypeAttributeMappingCollection GetAll(CallerContext callerContext)
        {
            EntityTypeAttributeMappingCollection entityTypeAttributeMappingCollection = new EntityTypeAttributeMappingCollection();

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("EntityTypeAttributeMappingBL.GetAll", MDMTraceSource.DataModel, false);

                entityTypeAttributeMappingCollection = Get(-1, -1, -1, true, callerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("EntityTypeAttributeMappingBL.GetAll", MDMTraceSource.DataModel);
            }
            return entityTypeAttributeMappingCollection;
        }

        /// <summary>
        /// Get entity type -attribute mappings by entity type id
        /// </summary>
        /// <param name="entityTypeId">All mappings with this entity type will be fetched.</param>
        /// <param name="callerContext">Indicates the context of caller which called this API</param>
        /// <returns>Returns the entity type attribute mapping collection based on given entity type id</returns>
        public EntityTypeAttributeMappingCollection GetMappingsByEntityTypeId(Int32 entityTypeId, CallerContext callerContext)
        {
            EntityTypeAttributeMappingCollection entityTypeAttributeMappingCollection = new EntityTypeAttributeMappingCollection();

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("EntityTypeAttributeMappingBL.GetMappingsByEntityTypeId", MDMTraceSource.DataModel, false);

                entityTypeAttributeMappingCollection = Get(entityTypeId, -1, -1, true, callerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("EntityTypeAttributeMappingBL.GetMappingsByEntityTypeId", MDMTraceSource.DataModel);
            }
            return entityTypeAttributeMappingCollection;
        }

        /// <summary>
        /// Get entity type - attribute mappings by entity type id and attribute group id
        /// </summary>
        /// <param name="entityTypeId">Indicates the entity type identifier</param>
        /// <param name="attributeGroupId">Indicates the attribute group identifier</param>
        /// <returns>Returns entity type attribute mapping collection based on entity type id and attribute group id</returns>
        public EntityTypeAttributeMappingCollection GetMappingsByEntityTypeIdAndAttributeGroupId(Int32 entityTypeId, Int32 attributeGroupId)
        {
            EntityTypeAttributeMappingCollection entityTypeAttributeMappingCollection = new EntityTypeAttributeMappingCollection();

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("EntityTypeAttributeMappingBL.GetMappingsByEntityTypeIdAndAttributeGroupId", MDMTraceSource.DataModel, false);

                entityTypeAttributeMappingCollection = Get(entityTypeId, attributeGroupId, -1, false);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("EntityTypeAttributeMappingBL.GetMappingsByEntityTypeIdAndAttributeGroupId", MDMTraceSource.DataModel);
            }
            return entityTypeAttributeMappingCollection;
        }

        /// <summary>
        /// Get entity type attribute mapping collection based on entity type id and attribute group id
        /// </summary>
        /// <param name="entityTypeId">Indicates the entity type identifier</param>
        /// <param name="attributeGroupId">Indicates the attribute group identifier</param>
        /// <param name="getLatest">Indicates whether to get the information from cache(if available) or database</param>
        /// <param name="callerContext">Indicates the context of the caller which called this API</param>
        /// <returns>Returns entity type attribute mapping collection based on entity type id and attribute group id</returns>
        public EntityTypeAttributeMappingCollection GetMappingsByEntityTypeIdAndAttributeGroupId(Int32 entityTypeId, Int32 attributeGroupId, Boolean getLatest, CallerContext callerContext)
        {
            EntityTypeAttributeMappingCollection entityTypeAttributeMappingCollection = new EntityTypeAttributeMappingCollection();

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("EntityTypeAttributeMappingBL.GetMappingsByEntityTypeIdAndAttributeGroupId", MDMTraceSource.DataModel, false);

                entityTypeAttributeMappingCollection = Get(entityTypeId, attributeGroupId, -1, true);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("EntityTypeAttributeMappingBL.GetMappingsByEntityTypeIdAndAttributeGroupId", MDMTraceSource.DataModel);
            }
            return entityTypeAttributeMappingCollection;
        }

        /// <summary>
        /// Get entity type - attribute mappings by attribute id
        /// </summary>
        /// <param name="attributeId">Indicates the attribute identifier</param>
        /// <param name="callerContext">Indicates the context of the caller which called this API</param>
        /// <returns>Returns entity type attribute mapping collection based on attribute id</returns>
        public EntityTypeAttributeMappingCollection GetMappingsByAttributeId(Int32 attributeId, CallerContext callerContext)
        {
            EntityTypeAttributeMappingCollection entityTypeAttributeMappingCollection = new EntityTypeAttributeMappingCollection();

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("EntityTypeAttributeMappingBL.GetMappingsByAttributeId", MDMTraceSource.DataModel, false);

                entityTypeAttributeMappingCollection = Get(-1, -1, attributeId, true, callerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("EntityTypeAttributeMappingBL.GetMappingsByAttributeId", MDMTraceSource.DataModel);
            }
            return entityTypeAttributeMappingCollection;
        }

        #endregion

        #region CUD Methods

        /// <summary>
        /// Create, Update or Delete EntityType Attribute Mapping 
        /// </summary>
        /// <param name="entityTypeAttributeMappings">EntityTypeAttributeMappings to process</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public OperationResult Process(EntityTypeAttributeMappingCollection entityTypeAttributeMappings, CallerContext callerContext)
        {
            OperationResult operationResult = new OperationResult();
            operationResult.OperationResultStatus = OperationResultStatusEnum.Failed;

            ValidateInputParameters(entityTypeAttributeMappings, callerContext);

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("EntityTypeAttributeMappingBL.Process", MDMTraceSource.DataModel, false);
                
                IEnumerable<Int32> attributeIds = entityTypeAttributeMappings.Where(mapping => mapping.Action != ObjectAction.Delete).Select(mapping => mapping.AttributeId);
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
                    PopulateProgramName(callerContext);
                    _entityTypeAttributeMappingDA.Process(entityTypeAttributeMappings, _securityPrincipal.CurrentUserName, callerContext.ProgramName);

                    ProcessEntityCacheLoadContextForMappingChange(entityTypeAttributeMappings);

                    transactionScope.Complete();
                    operationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                }

                #region activity log

                if (operationResult.OperationResultStatus == OperationResultStatusEnum.Successful && AppConfigurationHelper.GetAppConfig<Boolean>(Constants.ACTIVITYLOG_DATAMODEL_ENABLED_APPCONFIG_KEY, false))
                {
                    LogDataModelChanges(entityTypeAttributeMappings, callerContext);
                }

                #endregion activity log

                #region Invalidate impacted container entitytype attribute mappings

                Task.Factory.StartNew(() => InvalidateImpactedDataAsync(entityTypeAttributeMappings, _iContainerManager, OperationContext.Current));

                #endregion
            }
            catch (Exception ex)
            {
                operationResult.AddOperationResult(String.Empty, ex.Message, OperationResultType.Error);
            }

            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("EntityTypeAttributeMappingBL.Process", MDMTraceSource.DataModel);
            }

            return operationResult;
        }

        #endregion

        #region IDataModelManager Methods

        /// <summary>
        /// Generate and OperationResult Schema for given DataModelObjectCollection
        /// </summary>
        /// <param name="iDataModelObjects">Indicates the data model object collection</param>
        /// <param name="operationResults">Collection of DataModel OperationResult</param>
        public void PrepareOperationResultsSchema(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults)
        {
            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("EntityTypeAttributeMappingBL.PrepareOperationResultsSchema", MDMTraceSource.DataModel, false);

                String separator = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.RSDataModelExcel.Separator", "//").Trim();
                EntityTypeAttributeMappingCollection entityTypeAttributeMappings = iDataModelObjects as EntityTypeAttributeMappingCollection;

                if (entityTypeAttributeMappings != null && entityTypeAttributeMappings.Count > 0)
                {
                    if (operationResults.Count > 0)
                    {
                        operationResults.Clear();
                    }

                    int entityTypeAttributeMappingIdToBeCreated = -1;

                    foreach (EntityTypeAttributeMapping entityTypeAttributeMapping in entityTypeAttributeMappings)
                    {
                        DataModelOperationResult entityTypeAttributeMappingOperationResult = new DataModelOperationResult(entityTypeAttributeMapping.Id, entityTypeAttributeMapping.LongName, entityTypeAttributeMapping.ExternalId, entityTypeAttributeMapping.ReferenceId);

                        if (String.IsNullOrEmpty(entityTypeAttributeMappingOperationResult.ExternalId))
                        {
                            entityTypeAttributeMappingOperationResult.ExternalId = String.Format("{1} {0} {2} {0} {3}", separator, entityTypeAttributeMapping.EntityTypeName, entityTypeAttributeMapping.AttributeParentName, entityTypeAttributeMapping.AttributeName);
                        }

                        if (entityTypeAttributeMapping.Id < 1)
                        {
                            entityTypeAttributeMapping.Id = entityTypeAttributeMappingIdToBeCreated;
                            entityTypeAttributeMappingOperationResult.Id = entityTypeAttributeMappingIdToBeCreated;
                            entityTypeAttributeMappingIdToBeCreated--;
                        }

                        operationResults.Add(entityTypeAttributeMappingOperationResult);
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
                    MDMTraceHelper.StopTraceActivity("EntityTypeAttributeMappingBL.PrepareOperationResultsSchema", MDMTraceSource.DataModel);
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
                    MDMTraceHelper.StartTraceActivity("EntityTypeAttributeMappingBL.Validate", MDMTraceSource.DataModel, false);
                }

                EntityTypeAttributeMappingCollection entityTypeAttributeMappings = iDataModelObjects as EntityTypeAttributeMappingCollection;
                CallerContext callerContext = iCallerContext as CallerContext;
               
                #region Parameter Validations

                ValidateInputParameters(entityTypeAttributeMappings, operationResults, callerContext);

                #endregion  Parameter Validations

                #region Data Validations

                ValidateData(entityTypeAttributeMappings, operationResults, callerContext);
               
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
                    MDMTraceHelper.StopTraceActivity("EntityTypeAttributeMappingBL.Validate", MDMTraceSource.DataModel);
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
                    MDMTraceHelper.StartTraceActivity("EntityTypeAttributeMappingBL.LoadOriginal", MDMTraceSource.DataModel, false);

                LoadOriginalEntityTypeAttributeMapping(iDataModelObjects as EntityTypeAttributeMappingCollection, iCallerContext as CallerContext);
            }
            catch (Exception ex)
            {
                DataModelHelper.AddOperationResults(operationResults, String.Empty, ex.Message, null, OperationResultType.Error, TraceEventType.Error, iCallerContext as CallerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("EntityTypeAttributeMappingBL.LoadOriginal", MDMTraceSource.DataModel);
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
                    MDMTraceHelper.StartTraceActivity("EntityTypeAttributeMappingBL.FillDataModel", MDMTraceSource.DataModel, false);

                FillEntityTypeAttributeMapping(iDataModelObjects as EntityTypeAttributeMappingCollection, iCallerContext as CallerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("EntityTypeAttributeMappingBL.FillDataModel", MDMTraceSource.DataModel);
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
                    MDMTraceHelper.StartTraceActivity("EntityTypeAttributeMappingBL.CompareAndMerge", MDMTraceSource.DataModel, false);

                CompareAndMerge(iDataModelObjects as EntityTypeAttributeMappingCollection, operationResults, iCallerContext as CallerContext);
            }
            catch (Exception ex)
            {
                DataModelHelper.AddOperationResults(operationResults, String.Empty, ex.Message, null, OperationResultType.Error, TraceEventType.Error, iCallerContext as CallerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("EntityTypeAttributeMappingBL.CompareAndMerge", MDMTraceSource.DataModel);
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
                    MDMTraceHelper.StartTraceActivity("EntityTypeAttributeMappingBL.Process", MDMTraceSource.DataModel, false);

                EntityTypeAttributeMappingCollection entityTypeAttributeMappings = iDataModelObjects as EntityTypeAttributeMappingCollection;
                CallerContext callerContext = iCallerContext as CallerContext;

                if (entityTypeAttributeMappings.Count > 0)
                {
                    PopulateProgramName(callerContext);
                    
                    DBCommandProperties command = DBCommandHelper.Get((CallerContext)iCallerContext, MDMCenterModuleAction.Execute);

                    #region Perform EntityTypeAttributeMapping updates in database

                    using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                    {
                        _entityTypeAttributeMappingDA.Process(entityTypeAttributeMappings, operationResults, _securityPrincipal.CurrentUserName, callerContext.ProgramName);
                        ProcessEntityCacheLoadContextForMappingChange(entityTypeAttributeMappings);

                        transactionScope.Complete();
                    }

                    LocalizeErrors(callerContext, operationResults);

                    #endregion

                    #region activity log

                    if (operationResults.OperationResultStatus == OperationResultStatusEnum.Successful && AppConfigurationHelper.GetAppConfig<Boolean>(Constants.ACTIVITYLOG_DATAMODEL_ENABLED_APPCONFIG_KEY, false))
                    {
                        LogDataModelChanges(entityTypeAttributeMappings, callerContext);
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
                    MDMTraceHelper.StopTraceActivity("EntityTypeAttributeMappingBL.Process", MDMTraceSource.DataModel);
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
                    MDMTraceHelper.StartTraceActivity("EntityTypeAttributeMappingBL.InvalidateCache", MDMTraceSource.DataModel, false);

                EntityTypeAttributeMappingCollection entityTypeAttributeMappings = iDataModelObjects as EntityTypeAttributeMappingCollection;

                foreach (EntityTypeAttributeMapping entityTypeAttributeMapping in entityTypeAttributeMappings)
                {
                    // clear buffer manager
                    _mappingBufferManager.RemoveEntityTypeAttributeMappings(entityTypeAttributeMapping.EntityTypeId, false);
                }

                #region Invalidate impacted container entitytype attribute mappings

                Task.Factory.StartNew(() => InvalidateImpactedDataAsync(entityTypeAttributeMappings, this._iContainerManager, OperationContext.Current));

                #endregion
            }
            catch (Exception ex)
            {
                DataModelHelper.AddOperationResults(operationResults, String.Empty, ex.Message, null, OperationResultType.Error, TraceEventType.Error, iCallerContext as CallerContext);
            }

            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("EntityTypeAttributeMappingBL.InvalidateCache", MDMTraceSource.DataModel);
            }
        }

        #endregion

        #endregion

        #region Private Methods

        #region Validation Methods

        /// <summary>
        /// Validate the input parameters in entity type attribute mapping collection
        /// </summary>
        /// <param name="entityTypeAttributeMappings">Indicates the collection of entity type attribute mapping</param>
        /// <param name="callerContext">Indicates the context of the caller</param>
        private void ValidateInputParameters(EntityTypeAttributeMappingCollection entityTypeAttributeMappings, CallerContext callerContext)
        {
            String errorMessage;

            if (entityTypeAttributeMappings == null || entityTypeAttributeMappings.Count < 1)
            {
                errorMessage = "EntityTypeAttributeMapping Collection cannot be null.";
                throw new MDMOperationException("", errorMessage, "DataModelManager.EntityTypeAttributeMappingBL", String.Empty, "Create");
            }

            if (callerContext == null)
            {
                errorMessage = "CallerContext cannot be null.";
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "DataModelManager.EntityTypeAttributeMappingBL", String.Empty, "Create");
            }
        }

        /// <summary>
        /// Validate the input parameters in entity type attribute mapping collection
        /// </summary>
        /// <param name="entityTypeAttributeMappings">Indicates the collection of entity type attribute mapping</param>
        /// <param name="operationResults">Indicates the result of operation</param>
        /// <param name="callerContext">Indicates the context of the caller</param>
        private void ValidateInputParameters(EntityTypeAttributeMappingCollection entityTypeAttributeMappings, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            if (callerContext == null)
            {
                DataModelHelper.AddOperationResults(operationResults, "111846", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
            else if (entityTypeAttributeMappings == null || entityTypeAttributeMappings.Count < 1)
            {
                DataModelHelper.AddOperationResults(operationResults, "113619", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
        }

        /// <summary>
        /// Validate the data of specified parameters in entity type attribute mapping collection
        /// </summary>
        /// <param name="entityTypeAttributeMappings">Indicates the collection of entity type attribute mapping</param>
        /// <param name="operationResults">Indicates the result of operation</param>
        /// <param name="callerContext">Indicates the context of the caller</param>
        private void ValidateData(EntityTypeAttributeMappingCollection entityTypeAttributeMappings, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            if (entityTypeAttributeMappings != null && entityTypeAttributeMappings.Count > 0)
            {
                Collection<String> entityTypeAttributeMappingList = new Collection<String>();
                AttributeModelCollection attributeModels = new AttributeModelCollection();

                foreach (EntityTypeAttributeMapping entityTypeAttributeMapping in entityTypeAttributeMappings)
                {
                    IDataModelOperationResult dataModelOperationResult =
                        operationResults.GetByReferenceId(entityTypeAttributeMapping.ReferenceId);
                    AttributeModel attributeModel = DataModelHelper.GetAttributeModel(_iAttributeModelManager, entityTypeAttributeMapping.AttributeId, 
                        entityTypeAttributeMapping.AttributeName, entityTypeAttributeMapping.AttributeParentName,
                        ref attributeModels, callerContext);

                    if (attributeModel != null && attributeModel.AttributeModelType != AttributeModelType.Common)
                    {
                        String errorMessage =
                            "Attribute '{0}' under '{1}' group is of the type '{2}', so mapping cannot be performed for entity type '{3}'.";
                        Object[] parameters = new Object[]
                        {
                            attributeModel.Name, attributeModel.AttributeParentName,
                            attributeModel.AttributeModelType.ToString().ToLowerInvariant(),
                            entityTypeAttributeMapping.EntityTypeName
                        };

                        DataModelHelper.AddOperationResult(dataModelOperationResult, "113966",
                            String.Format(errorMessage, parameters), parameters, OperationResultType.Error,
                            TraceEventType.Error, callerContext);
                    }
                    else
                    {
                        Collection<String> uniqueIdentifierList = new Collection<String>()
                        {
                            entityTypeAttributeMapping.EntityTypeName,
                            entityTypeAttributeMapping.AttributeName,
                            entityTypeAttributeMapping.AttributeParentName
                        };

                        String rowUniqueIdentifier = DataModelHelper.GetUniqueIdentifier(uniqueIdentifierList);

                        DataModelHelper.ValidateEntityTypeName(entityTypeAttributeMapping.EntityTypeName,
                            dataModelOperationResult, callerContext);

                        DataModelHelper.ValidateAttributeUniqueIdentifier(entityTypeAttributeMapping.AttributeName,
                            entityTypeAttributeMapping.AttributeParentName,
                            dataModelOperationResult, callerContext);

                        if (entityTypeAttributeMappingList.Contains(rowUniqueIdentifier))
                        {
                            Object[] parameters = null;
                            parameters = new Object[]
                            {
                                entityTypeAttributeMapping.EntityTypeName,
                                entityTypeAttributeMapping.AttributeName,
                                entityTypeAttributeMapping.AttributeParentName
                            };

                            String errorMessage =
                                String.Format(
                                    "Duplicate mappings found for the entity type attribute mapping with entity type: {0}, attribute: {1}, and attribute parent: {2}",
                                    parameters);

                            DataModelHelper.AddOperationResult(dataModelOperationResult, "113970", errorMessage,
                                parameters, OperationResultType.Error, TraceEventType.Error, callerContext);
                        }
                        else
                        {
                            entityTypeAttributeMappingList.Add(rowUniqueIdentifier);
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
        /// <param name="entityTypeId"></param>
        /// <param name="attributeGroupId"></param>
        /// <param name="attributeId"></param>
        /// <param name="getLatest"></param>
        /// <returns></returns>
        private EntityTypeAttributeMappingCollection Get(Int32 entityTypeId, Int32 attributeGroupId, Int32 attributeId, Boolean getLatest)
        {
            EntityTypeAttributeMappingCollection entityTypeAttributeMappings = new EntityTypeAttributeMappingCollection();
            CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity);

            EntityTypeAttributeMappingCollection containerEntityTypeAttributeMappingCollection = Get(entityTypeId, attributeGroupId, attributeId, getLatest, callerContext);

            if (containerEntityTypeAttributeMappingCollection != null && containerEntityTypeAttributeMappingCollection.Count > 0)
            {
                entityTypeAttributeMappings = new EntityTypeAttributeMappingCollection(containerEntityTypeAttributeMappingCollection.ToList());
            }

            return entityTypeAttributeMappings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityTypeId"></param>
        /// <param name="attributeGroupId"></param>
        /// <param name="attributeId"></param>
        /// <param name="getLatest"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private EntityTypeAttributeMappingCollection Get(Int32 entityTypeId, Int32 attributeGroupId, Int32 attributeId, Boolean getLatest, CallerContext callerContext)
        {
            #region Initial Setup

            EntityTypeAttributeMappingCollection entityTypeAttributeMappings = null;
            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);
            //TODO: EntityType attribute mappings are not cached.This flag is required for future used.
            getLatest = true;

            #endregion

            #region Get Mappings from Cache if available

            if (!getLatest)
            {
                entityTypeAttributeMappings = _mappingBufferManager.FindEntityTypeAttributeMappings(entityTypeId);
            }

            #endregion

            #region Get Mappings from Database

            if (entityTypeAttributeMappings == null)
            {
                entityTypeAttributeMappings = _entityTypeAttributeMappingDA.Get(entityTypeId, attributeGroupId, attributeId, command);

                #region Cache Mappings data

                //if (entityTypeAttributeMappings != null && entityTypeAttributeMappings.Count > 0)
                //{
                //    _mappingBufferManager.UpdateEntityTypeAttributeMappings(entityTypeAttributeMappings, entityTypeId, attributeModelType, 3);
                //}

                #endregion
            }

            #endregion

            return entityTypeAttributeMappings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityTypeAttributeMappings"></param>
        /// <param name="callerContext"></param>
        private void LoadOriginalEntityTypeAttributeMapping(EntityTypeAttributeMappingCollection entityTypeAttributeMappings, CallerContext callerContext)
        {
            EntityTypeAttributeMappingCollection orginalEntityTypeAttributeMappingCollection = Get(-1, -1, -1, true, callerContext);

            if (orginalEntityTypeAttributeMappingCollection != null && orginalEntityTypeAttributeMappingCollection.Count > 0)
            {
                foreach (EntityTypeAttributeMapping entityTypeAttributeMapping in entityTypeAttributeMappings)
                {
                    entityTypeAttributeMapping.OriginalEntityTypeAttributeMapping = orginalEntityTypeAttributeMappingCollection.Get(entityTypeAttributeMapping.EntityTypeName, entityTypeAttributeMapping.AttributeName, entityTypeAttributeMapping.AttributeParentName);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityTypeAttributeMappings"></param>
        /// <param name="callerContext"></param>
        private void FillEntityTypeAttributeMapping(EntityTypeAttributeMappingCollection entityTypeAttributeMappings, CallerContext callerContext)
        {
            EntityTypeCollection entityTypes = new EntityTypeCollection();
            AttributeModelCollection attributeModels = new AttributeModelCollection();

            foreach (EntityTypeAttributeMapping entityTypeAttributeMapping in entityTypeAttributeMappings)
            {
                if (entityTypeAttributeMapping.OriginalEntityTypeAttributeMapping != null)
                {
                    entityTypeAttributeMapping.Id = entityTypeAttributeMapping.OriginalEntityTypeAttributeMapping.Id;
                    entityTypeAttributeMapping.EntityTypeId = entityTypeAttributeMapping.OriginalEntityTypeAttributeMapping.EntityTypeId;
                    entityTypeAttributeMapping.AttributeId = entityTypeAttributeMapping.OriginalEntityTypeAttributeMapping.AttributeId;
                    entityTypeAttributeMapping.AttributeParentId = entityTypeAttributeMapping.OriginalEntityTypeAttributeMapping.AttributeParentId;
                }
                else
                {
                    entityTypeAttributeMapping.EntityTypeId = DataModelHelper.GetEntityTypeId(_iEntityTypeManager, entityTypeAttributeMapping.EntityTypeName, ref entityTypes, callerContext);
                    AttributeModel attributeModel = DataModelHelper.GetAttributeModel(_iAttributeModelManager, entityTypeAttributeMapping.AttributeId, entityTypeAttributeMapping.AttributeName, entityTypeAttributeMapping.AttributeParentName, ref attributeModels, callerContext);

                    if (attributeModel != null)
                    {
                        entityTypeAttributeMapping.AttributeId = attributeModel.Id;
                        entityTypeAttributeMapping.AttributeParentId = attributeModel.AttributeParentId;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityTypeAttributeMappings"></param>
        /// <param name="operationResults"></param>
        /// <param name="callerContext"></param>
        private void CompareAndMerge(EntityTypeAttributeMappingCollection entityTypeAttributeMappings, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            foreach (EntityTypeAttributeMapping deltaEntityTypeAttributeMapping in entityTypeAttributeMappings)
            {
                IDataModelOperationResult operationResult = operationResults.GetByReferenceId(deltaEntityTypeAttributeMapping.ReferenceId);

                //If delta object Action is Read/Ignore then skip Merge Delta Process.
                if (deltaEntityTypeAttributeMapping.Action == ObjectAction.Read || deltaEntityTypeAttributeMapping.Action == ObjectAction.Ignore)
                    continue;

                IEntityTypeAttributeMapping originalEntityTypeAttributeMapping = deltaEntityTypeAttributeMapping.OriginalEntityTypeAttributeMapping;

                if (originalEntityTypeAttributeMapping != null)
                {
                    //If delta object Action is Delete then skip Merge Delta process.
                    if (deltaEntityTypeAttributeMapping.Action != ObjectAction.Delete)
                    {
                        originalEntityTypeAttributeMapping.MergeDelta(deltaEntityTypeAttributeMapping, callerContext, false);
                    }
                }
                else
                {
                    if (deltaEntityTypeAttributeMapping.Action == ObjectAction.Delete)
                    {
                        Object[] parameters = { deltaEntityTypeAttributeMapping.EntityTypeName, deltaEntityTypeAttributeMapping.AttributeName, deltaEntityTypeAttributeMapping.AttributeParentName };
                        DataModelHelper.AddOperationResult(operationResult, "113630", String.Empty, parameters, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                    else
                    {
                        List<String> parameters = new List<String>();
                        List<String> errorMessages = new List<String>();

                        if (deltaEntityTypeAttributeMapping.EntityTypeId < 1)
                        {
                            parameters.Add(deltaEntityTypeAttributeMapping.EntityTypeName);
                            errorMessages.Add(DataModelHelper.GetLocaleMessageText("100415", String.Empty, null, callerContext)); // 100415 - Entity Type
                        }
                        if (deltaEntityTypeAttributeMapping.AttributeId < 1)
                        {
                            parameters.Add(deltaEntityTypeAttributeMapping.AttributeName);
                            errorMessages.Add(DataModelHelper.GetLocaleMessageText("100163", String.Empty, null, callerContext)); // 100163 - Attribute Name
                        }
                        if (deltaEntityTypeAttributeMapping.AttributeParentId < 1)
                        {
                            parameters.Add(deltaEntityTypeAttributeMapping.AttributeParentName);
                            errorMessages.Add(DataModelHelper.GetLocaleMessageText("112803", String.Empty, null, callerContext)); // 112803 - Attribute Parent Name
                        }

                        if (errorMessages.Count > 0)
                        {
                            DataModelHelper.AddInvalidNamesErrorsToOperationResult(operationResult, errorMessages, parameters, callerContext);
                        }

                        //If original object is not found then set Action as Create always.

                        deltaEntityTypeAttributeMapping.Action = ObjectAction.Create;
                    }
                }
                operationResult.PerformedAction = deltaEntityTypeAttributeMapping.Action;
            }
        }

        #endregion

        #region EntityCacheLoadContext For EntityType Attribute Mapping

        /// <summary>
        /// Processes the EntityCacheLoadContext for entity type mapping change.
        /// </summary>
        private void ProcessEntityCacheLoadContextForMappingChange(EntityTypeAttributeMappingCollection entityTypeAttributeMappings)
        {
            Dictionary<Int32, Collection<Int32>> entityCacheContextRequest = BuildEntityTypeAttributeMappingList(entityTypeAttributeMappings);
            if (entityCacheContextRequest.Count > 0)
            {
                // Create EntityCacheLoadContext entityCacheContextRequest
                EntityCacheLoadContext entityCacheLoadContext = new EntityCacheLoadContext();
                entityCacheLoadContext.CacheStatus = (Int32)(EntityCacheComponentEnum.InheritedAttributes | EntityCacheComponentEnum.OverriddenAttributes);

                foreach (KeyValuePair<Int32, Collection<Int32>> item in entityCacheContextRequest)
                {
                    EntityCacheLoadContextItemCollection entityCacheLoadContextItemCollection = GetEntityCacheLoadContextItemCollection(item.Key, item.Value);
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
        /// Splits the Collection of EntityTypeAttributeMapping into a dictionary of entity type id and attribute id list.
        /// </summary>
        private Dictionary<Int32, Collection<Int32>> BuildEntityTypeAttributeMappingList(EntityTypeAttributeMappingCollection entityTypeAttributeMappings)
        {
            Dictionary<Int32, Collection<Int32>> entityCacheContextRequest = new Dictionary<Int32, Collection<Int32>>();

            foreach (EntityTypeAttributeMapping entityTypeAttributeMapping in entityTypeAttributeMappings)
            {
                if (entityTypeAttributeMapping.Action == ObjectAction.Create || entityTypeAttributeMapping.Action == ObjectAction.Delete)
                {
                    if (entityCacheContextRequest.ContainsKey(entityTypeAttributeMapping.EntityTypeId))
                    {
                        Collection<Int32> valueList = entityCacheContextRequest[entityTypeAttributeMapping.EntityTypeId];
                        valueList.Add(entityTypeAttributeMapping.AttributeId);
                    }
                    else
                    {
                        entityCacheContextRequest.Add(entityTypeAttributeMapping.EntityTypeId,
                            new Collection<Int32>() { entityTypeAttributeMapping.AttributeId });
                    }
                }
            }

            return entityCacheContextRequest;
        }

        /// <summary>
        /// Creates an EntityCacheLoadContextItemCollection for the specified entity type id and attribute id list.
        /// </summary>
        private EntityCacheLoadContextItemCollection GetEntityCacheLoadContextItemCollection(Int32 entityTypeId, Collection<Int32> attributeIdList)
        {
            // Create EntityCacheLoadContextItemCollection
            EntityCacheLoadContextItemCollection entityCacheLoadContextItemCollection = new EntityCacheLoadContextItemCollection();

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityTypeAttributeMappings"></param>
        /// <param name="containerManager"></param>
        /// <param name="operationContext"></param>
        private static void InvalidateImpactedDataAsync(EntityTypeAttributeMappingCollection entityTypeAttributeMappings, IContainerManager containerManager, OperationContext operationContext)
        {
            try
            {
                if (entityTypeAttributeMappings == null)
                    return;

                OperationContext.Current = operationContext;
                Dictionary<Int32, IEnumerable<Int32>> entityTypeIdContainerIdsMapping = new Dictionary<Int32, IEnumerable<Int32>>();
                ContainerEntityTypeMappingBL containerEntityTypeMappingManager = new ContainerEntityTypeMappingBL();

                foreach (EntityTypeAttributeMapping entityTypeAttributeMapping in entityTypeAttributeMappings)
                {
                    Int32 entityTypeId = entityTypeAttributeMapping.EntityTypeId;
                    IEnumerable<Int32> containerIds = new Collection<Int32>();

                    entityTypeIdContainerIdsMapping.TryGetValue(entityTypeId, out containerIds);

                    if (containerIds == null || containerIds.Count() < 1)
                    {
                        //Get all the containers mapped with given entity type 
                        ContainerEntityTypeMappingCollection containerEntityTypeMappings = containerEntityTypeMappingManager.GetMappingsByEntityTypeId(entityTypeId, new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.UIProcess));

                        if (containerEntityTypeMappings != null && containerEntityTypeMappings.Count > 0)
                        {
                            containerIds = containerEntityTypeMappings.Select(m => m.ContainerId);

                            //Check if any entity type is mapped to all containers.
                            //If yes, ContainerId will be 0
                            if (containerIds.Contains(0))
                            {
                                //Yes.. ET is mapped to all containers.
                                //Get all containers
                                IContainerCollection allContainers = containerManager.GetAll(new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.UIProcess), false);

                                //Now containerIds will have all containers. 
                                containerIds = allContainers.Select(con => con.Id);
                            }

                            entityTypeIdContainerIdsMapping.Add(entityTypeId, containerIds);
                        }
                    }
                }

                #region Invalidate All Common Attributes Mappings for entity type

                if (entityTypeIdContainerIdsMapping != null && entityTypeIdContainerIdsMapping.Count > 0)
                {
                    MappingBufferManager mappingBufferManager = new MappingBufferManager();

                    foreach (KeyValuePair<Int32, IEnumerable<Int32>> keyValuePair in entityTypeIdContainerIdsMapping)
                    {
                        //Invalidate All Common Attributes Mappings
                        mappingBufferManager.InvalidateImpactedMappings(new Collection<Int32>(keyValuePair.Value.ToList()), keyValuePair.Key, 0, AttributeModelType.Common, false);
                    }
                }

                #endregion

                #region Update Attribute Model Version Number

                DataModelVersionManager dataModelVersionManager = new DataModelVersionManager();
                dataModelVersionManager.UpdateAttributeModelsVersionNumber();

                #endregion
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
                callerContext.ProgramName = "EntityTypeAttributeMappingBL";
            }
        }

        /// <summary>
        /// Log changes to tb_datamodelactivitylog if DA call is sucessfull and appconfig key datamodelactivitylog.enable == true
        /// </summary>
        /// <param name="entityTypeAttributeMappingCollection"></param>
        /// <param name="callerContext"></param>
        private void LogDataModelChanges(EntityTypeAttributeMappingCollection entityTypeAttributeMappingCollection, CallerContext callerContext)
        {
            #region step: populate datamodelactivitylog object

            DataModelActivityLogBL dataModelActivityLogManager = new DataModelActivityLogBL();

            DataModelActivityLogCollection activityLogCollection = dataModelActivityLogManager.FillDataModelActivityLogCollection(entityTypeAttributeMappingCollection, entityTypeAttributeMappingCollection.DataModelObjectType, callerContext);

            #endregion step: populate datamodelactivitylog object

            #region step: make api call

            if (activityLogCollection != null && activityLogCollection.Count > 0) // null activitylog collection means there was error in mapping
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