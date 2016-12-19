using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
    /// Specifies category attribute mapping manager
    /// </summary> 
    public class CategoryAttributeMappingBL : BusinessLogicBase, IDataModelManager
    {
        #region Fields

        /// <summary>
        /// Field denoting the security principal
        /// </summary>
        private readonly SecurityPrincipal _securityPrincipal = null;

        /// <summary>
        /// Data access layer for entity types
        /// </summary>
        private CategoryAttributeMappingDA _categoryAttributeMappingDA = new CategoryAttributeMappingDA();

        /// <summary>
        /// Field denoting locale Message
        /// </summary>
        private LocaleMessage _localeMessage;

        /// <summary>
        /// Field denoting localeMessageBL
        /// </summary>
        private readonly LocaleMessageBL _localeMessageBL;

        /// <summary>
        /// Cache manager for category - attribute mapping objects
        /// </summary>
        private MappingBufferManager _mappingBufferManager = null;

        /// <summary>
        /// Field denoting the IHierarchyManager
        /// </summary>
        private IHierarchyManager _iHierarchyManager = null;

        /// <summary>
        /// Field denoting the ICategoryManager
        /// </summary>
        private ICategoryManager _iCategoryManager = null;

        /// <summary>
        /// Field denoting the IAttributeModelManager
        /// </summary>
        private IAttributeModelManager _iAttributeModelManager = null;

        /// <summary>
        /// Field denoting the IEntityManager
        /// </summary>
        private IEntityManager _iEntityManager = null;

        /// <summary>
        /// Field denoting system data locale.
        /// </summary>
        private LocaleEnum _systemDataLocale = LocaleEnum.UnKnown;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for CategoryAttributeMappingBL
        /// </summary>
        public CategoryAttributeMappingBL()
        {
            _mappingBufferManager = new MappingBufferManager();
            _localeMessageBL = new LocaleMessageBL();
            _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            _systemDataLocale = GlobalizationHelper.GetSystemDataLocale();
        }

        /// <summary>
        /// Constructor for CategoryAttributeMappingBL
        /// </summary>
        /// <param name="iHierarchyManager">Indicates hierarchy manager</param>
        /// <param name="iCategoryManager">Indicates category manager</param>
        /// <param name="iAttributeModelManager">Indicates attribute model manager</param>
        /// <param name="iEntityManager">Indicates entity manager</param>
        public CategoryAttributeMappingBL(IHierarchyManager iHierarchyManager, ICategoryManager iCategoryManager, IAttributeModelManager iAttributeModelManager, IEntityManager iEntityManager)
            : this()
        {
            this._iHierarchyManager = iHierarchyManager;
            this._iCategoryManager = iCategoryManager;
            this._iAttributeModelManager = iAttributeModelManager;
            this._iEntityManager = iEntityManager;
        }

        #endregion Constructors

        #region Properties
        #endregion Properties

        #region Methods

        #region Public Methods

        #region Get Methods

        /// <summary>
        /// Gets the by category identifier.
        /// </summary>
        /// <param name="categoryId">The category identifier.</param>
        /// <param name="callerContext">The caller context.</param>
        /// <returns>Collection of category - attribute Mapping</returns>
        public CategoryAttributeMappingCollection GetByCategoryId(Int64 categoryId, CallerContext callerContext)
        {
            if (categoryId < 1)
            {
                throw new MDMOperationException("112227", "Category Id cannot less than 1.", "CategoryAttributeMappingBL.GetByCategoryId", String.Empty, "GetByCategoryId");
            }

            return Get(categoryId, 0, 0, false, callerContext);
        }

        /// <summary>
        /// Gets the by catalog identifier.
        /// </summary>
        /// <param name="catalogId">The catalog identifier.</param>
        /// <param name="callerContext">The caller context.</param>
        /// <returns>Collection of category - attribute Mapping</returns>
        public CategoryAttributeMappingCollection GetByCatalogId(Int32 catalogId, CallerContext callerContext)
        {
            if (catalogId < 1)
            {
                throw new MDMOperationException("112228", "Catalog Id cannot less than 1.", "CategoryAttributeMappingBL.GetByCatalogId", String.Empty, "GetByCatalogId");
            }

            //Setting getLatest parameters as true since we are not supporting caching mechanism if mappings are requested by container id or hierarchy id.
            return Get(0, catalogId, 0, true, callerContext);
        }

        /// <summary>
        /// Gets the by hierarchy identifier.
        /// </summary>
        /// <param name="hierarchyId">The hierarchy identifier.</param>
        /// <param name="callerContext">The caller context.</param>
        /// <returns>Collection of category - attribute Mapping</returns>
        public CategoryAttributeMappingCollection GetByHierarchyId(Int32 hierarchyId, CallerContext callerContext)
        {
            if (hierarchyId < 1)
            {
                throw new MDMOperationException("112198", "Hierarchy Id cannot less than 1.", "CategoryAttributeMappingBL.GetByCatalogId", String.Empty, "GetByHierarchyId");
            }

            //Setting getLatest parameters as true since system is not supporting caching mechanism if mappings are requested by container id or hierarchy id.
            return Get(0, 0, hierarchyId, true, callerContext);
        }

        #endregion Get Methods

        #region Process Methods

        /// <summary>
        /// Creates the categoryAttributeMapping
        /// </summary>
        /// <param name="categoryAttributeMapping">categoryAttributeMapping to be created</param>
        /// <param name="entityManager">Entity Manager</param>
        /// <param name="callerContext">Caller context of application</param>
        /// <returns>Result of the operation</returns>
        public OperationResult Create(CategoryAttributeMapping categoryAttributeMapping, IEntityManager entityManager, CallerContext callerContext, IAttributeModelManager iAttributeModelManager)
        {
            ValidateInputParameters(categoryAttributeMapping, callerContext);

            categoryAttributeMapping.Action = ObjectAction.Create;
            return GetOperationResultFromCollection(this.ProcessCollection(new CategoryAttributeMappingCollection { categoryAttributeMapping }, entityManager, callerContext, iAttributeModelManager));
        }

        /// <summary>
        /// Updates given categoryAttributeMapping
        /// </summary>
        /// <param name="categoryAttributeMapping">categoryAttributeMapping to be updated</param>
        /// <param name="entityManager">Entity Manager</param>
        /// <param name="callerContext">Caller context of application</param>
        /// <returns>Result of the operation</returns>
        public OperationResult Update(CategoryAttributeMapping categoryAttributeMapping, IEntityManager entityManager, CallerContext callerContext, IAttributeModelManager iAttributeModelManager)
        {
            ValidateInputParameters(categoryAttributeMapping, callerContext);

            categoryAttributeMapping.Action = ObjectAction.Update;
            return GetOperationResultFromCollection(this.ProcessCollection(new CategoryAttributeMappingCollection { categoryAttributeMapping }, entityManager, callerContext, iAttributeModelManager));
        }

        /// <summary>
        /// Deletes given categoryAttributeMapping
        /// </summary>
        /// <param name="categoryAttributeMapping">categoryAttributeMapping to be deleted</param>
        /// <param name="entityManager">Entity Manager</param>
        /// <param name="callerContext">Caller context of application</param>
        /// <returns>Result of the operation</returns>
        public OperationResult Delete(CategoryAttributeMapping categoryAttributeMapping, IEntityManager entityManager, CallerContext callerContext, IAttributeModelManager iAttributeModelManager)
        {
            ValidateInputParameters(categoryAttributeMapping, callerContext);

            categoryAttributeMapping.Action = ObjectAction.Delete;
            return GetOperationResultFromCollection(this.ProcessCollection(new CategoryAttributeMappingCollection { categoryAttributeMapping }, entityManager, callerContext, iAttributeModelManager));
        }

        /// <summary>
        /// Inherits given categoryAttributeMapping
        /// </summary>
        /// <param name="categoryAttributeMapping">categoryAttributeMapping to be inherited</param>
        /// <param name="entityManager"></param>
        /// <param name="callerContext">Caller context of application</param>
        /// <returns>Result of the operation</returns>
        public OperationResult Inherit(CategoryAttributeMapping categoryAttributeMapping, IEntityManager entityManager, CallerContext callerContext, IAttributeModelManager iAttributeModelManager)
        {
            ValidateInputParameters(categoryAttributeMapping, callerContext);

            categoryAttributeMapping.Action = ObjectAction.Delete;
            categoryAttributeMapping.IsInheritable = true;

            return GetOperationResultFromCollection(this.ProcessCollection(new CategoryAttributeMappingCollection { categoryAttributeMapping }, entityManager, callerContext, iAttributeModelManager));
        }

        /// <summary>
        /// Create - Update or Delete given attribute-collection mappings
        /// </summary>
        /// <param name="categoryAttributeMappingCollection">Collection of attribute-collection mappings to process</param>
        /// <param name="entityManager">Entity Manager</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public OperationResultCollection Process(CategoryAttributeMappingCollection categoryAttributeMappings, IEntityManager entityManager, CallerContext callerContext, IAttributeModelManager iAttributeModelManager)
        {
            OperationResultCollection categoryAttrMappingProcessOperationResult = new OperationResultCollection();

            if (categoryAttributeMappings == null || categoryAttributeMappings.Count < 1)
            {
                throw new MDMOperationException("112229", "Category - Attribute mapping collection cannot be null..", "CategoryAttributeMappingBL.Process", String.Empty, "Process");
            }

            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "CategoryAttributeMappingBL.Process", String.Empty,
                    "Process");
            }

            categoryAttrMappingProcessOperationResult = this.ProcessCollection(categoryAttributeMappings, entityManager, callerContext, iAttributeModelManager);

            return categoryAttrMappingProcessOperationResult;
        }

        #endregion Process Methods

        #region IDataModelManager Methods

        /// <summary>
        /// Generate and OperationResult Schema for given DataModelObjectCollection
        /// </summary>
        /// <param name="iDataModelObjects">Category attribute mapping object</param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        public void PrepareOperationResultsSchema(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults)
        {
            try
            {
                String separator = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.RSDataModelExcel.Separator", "//").Trim();
                CategoryAttributeMappingCollection categoryAttributeMappings = iDataModelObjects as CategoryAttributeMappingCollection;

                if (categoryAttributeMappings != null && categoryAttributeMappings.Count > 0)
                {
                    if (operationResults.Count > 0)
                    {
                        operationResults.Clear();
                    }

                    Int32 categoryAttributeMappingIdToBeCreated = -1;

                    foreach (CategoryAttributeMapping categoryAttributeMapping in categoryAttributeMappings)
                    {
                        DataModelOperationResult categoryAttributeMappingOperationResult = new DataModelOperationResult(categoryAttributeMapping.Id, categoryAttributeMapping.LongName, categoryAttributeMapping.ExternalId, categoryAttributeMapping.ReferenceId);

                        if (String.IsNullOrEmpty(categoryAttributeMappingOperationResult.ExternalId))
                        {
                            categoryAttributeMappingOperationResult.ExternalId = String.Format("{1} {0} {2} {0} {3} {0} {4}", separator, categoryAttributeMapping.HierarchyName, categoryAttributeMapping.CategoryName, categoryAttributeMapping.AttributeParentName, categoryAttributeMapping.AttributeName);
                        }

                        if (categoryAttributeMapping.Id < 1)
                        {
                            categoryAttributeMapping.Id = categoryAttributeMappingIdToBeCreated;
                            categoryAttributeMappingOperationResult.Id = categoryAttributeMappingIdToBeCreated--;
                        }

                        operationResults.Add(categoryAttributeMappingOperationResult);
                    }
                }
            }
            catch (Exception ex)
            {
                DataModelHelper.AddOperationResults(operationResults, String.Empty, ex.Message, null, OperationResultType.Error, TraceEventType.Error, new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.DataModelImport));
            }
        }

        /// <summary>
        /// Validates DataModelObjects and populate OperationResults
        /// </summary>
        /// <param name="iDataModelObjects">Indicates collection of data model objects to be filled up.</param>
        /// <param name="operationResults">Indicates collection of dataModel operation result</param>
        /// <param name="iCallerContext">Indicates the context of the caller</param>
        /// <returns>DataModel Operation Result Collection</returns>
        public void Validate(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            try
            {
                CategoryAttributeMappingCollection categoryAttributeMappings = iDataModelObjects as CategoryAttributeMappingCollection;
                CallerContext callerContext = iCallerContext as CallerContext;
                
                #region Parameter Validations

                ValidateInputParameters(categoryAttributeMappings, operationResults, callerContext);

                #endregion Parameter Validations

                #region Data Validations

                ValidateData(categoryAttributeMappings, operationResults, callerContext);
               
                #endregion Data Validations
            }
            catch (Exception ex)
            {
                DataModelHelper.AddOperationResults(operationResults, String.Empty, ex.Message, null, OperationResultType.Error, TraceEventType.Error, iCallerContext as CallerContext);
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
                LoadOriginalCategoryAttributeMapping(iDataModelObjects as CategoryAttributeMappingCollection, iCallerContext as CallerContext);
            }
            catch (Exception ex)
            {
                DataModelHelper.AddOperationResults(operationResults, String.Empty, ex.Message, null, OperationResultType.Error, TraceEventType.Error, iCallerContext as CallerContext);
            }
        }

        /// <summary>
        /// Fills missing information to data model objects.
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be filled up.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void FillDataModel(IDataModelObjectCollection iDataModelObjects, ICallerContext iCallerContext)
        {
            FillCategoryAttributeMapping(iDataModelObjects as CategoryAttributeMappingCollection, iCallerContext as CallerContext);
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
                CompareAndMerge(iDataModelObjects as CategoryAttributeMappingCollection, operationResults, iCallerContext as CallerContext);
            }
            catch (Exception ex)
            {
                DataModelHelper.AddOperationResults(operationResults, String.Empty, ex.Message, null, OperationResultType.Error, TraceEventType.Error, iCallerContext as CallerContext);
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
                CategoryAttributeMappingCollection categoryAttributeMappings = iDataModelObjects as CategoryAttributeMappingCollection;
                CallerContext callerContext = iCallerContext as CallerContext;

                if (categoryAttributeMappings.Count > 0)
                {
                    DBCommandProperties command = DBCommandHelper.Get((CallerContext)iCallerContext, MDMCenterModuleAction.Execute);
                    PopulateProgramName(callerContext);
                    Collection<Int32> attributeIdsWithDeleteAction = new Collection<Int32>();
                    Collection<Int64> categoryIds = new Collection<Int64>();

                    foreach (CategoryAttributeMapping categoryAttributeMapping in categoryAttributeMappings)
                    {
                        if (categoryAttributeMapping.Action == ObjectAction.Delete)
                        {
                            attributeIdsWithDeleteAction.Add(categoryAttributeMapping.AttributeId);
                        }

                        if (categoryAttributeMapping.Action != ObjectAction.Read && categoryAttributeMapping.Action != ObjectAction.Ignore && !categoryIds.Contains(categoryAttributeMapping.CategoryId))
                        {
                            categoryIds.Add(categoryAttributeMapping.CategoryId);
                        }
                    }

                    //Clear cache for deleted mapping's attribute.
                    if (attributeIdsWithDeleteAction.Count > 0)
                    {
                        Task.Factory.StartNew(() => InvalidateImpactedAttributeIds(attributeIdsWithDeleteAction, _iAttributeModelManager));
                    }

                    InvalidateImpactedCategories(categoryIds, _iEntityManager);

                    #region Perform CategoryAttributeMapping updates in database

                    using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                    {
                        _categoryAttributeMappingDA.Process(categoryAttributeMappings, operationResults, callerContext.ProgramName, _securityPrincipal.CurrentUserName, _systemDataLocale, command);

                        transactionScope.Complete();
                    }

                    LocalizeErrors(callerContext, operationResults);

                    #endregion
                }
            }
            catch (Exception ex)
            {
                DataModelHelper.AddOperationResults(operationResults, String.Empty, ex.Message, null, OperationResultType.Error, TraceEventType.Error, iCallerContext as CallerContext);
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
                CategoryAttributeMappingCollection categoryAttributeMappings = iDataModelObjects as CategoryAttributeMappingCollection;
                Collection<Int32> attributeIdsWithUpdateAction = new Collection<Int32>();

                ProcessEntityCacheLoadContextForMappingChange(categoryAttributeMappings, iCallerContext as CallerContext);

                // clear buffer manager, bulk call.
                _mappingBufferManager.RemoveCategoryAttributeMappings(categoryAttributeMappings, true);

                foreach (CategoryAttributeMapping categoryAttributeMapping in categoryAttributeMappings)
                {
                    if (categoryAttributeMapping.Action == ObjectAction.Update)
                    {
                        attributeIdsWithUpdateAction.Add(categoryAttributeMapping.AttributeId);
                    }
                }

                if (attributeIdsWithUpdateAction.Count > 0)
                {
                    Task.Factory.StartNew(() => InvalidateImpactedAttributeIds(attributeIdsWithUpdateAction, _iAttributeModelManager));
                }
            }
            catch (Exception ex)
            {
                DataModelHelper.AddOperationResults(operationResults, String.Empty, ex.Message, null, OperationResultType.Error, TraceEventType.Error, iCallerContext as CallerContext);
            }
            finally
            {
            }
        }

        #endregion

        #endregion Public Methods

        #region Private Methods

        #region Validation Methods

        /// <summary>
        /// Validate the input parameters for category attribute mappings
        /// </summary>
        /// <param name="categoryAttributeMapping">Indicates the category attribute mapping object</param>
        /// <param name="callerContext">Indicates the context of the caller</param>
        private static void ValidateInputParameters(CategoryAttributeMapping categoryAttributeMapping, CallerContext callerContext)
        {
            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "CategoryAttributeMappingBL.Process", String.Empty,
                    "Process");
            }

            if (categoryAttributeMapping == null)
            {
                throw new MDMOperationException("112230", "Category - Attribute Mapping cannot be null.", "CategoryAttributeMappingBL.Process", String.Empty,
                    "Process");
            }
        }

        /// <summary>
        /// Validate the input parameters for category attribute mappings
        /// </summary>
        /// <param name="categoryAttributeMappings">Indicates the collection of category attribute mapping object</param>
        /// <param name="operationResults">Indicates the results of operation</param>
        /// <param name="callerContext">Indicates the context of the caller</param>
        private void ValidateInputParameters(CategoryAttributeMappingCollection categoryAttributeMappings, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            if (callerContext == null)
            {
                // If caller context is  null, internally object creation is handled 
                DataModelHelper.AddOperationResults(operationResults, "111846", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
            else if (categoryAttributeMappings == null || categoryAttributeMappings.Count < 1)
            {
                DataModelHelper.AddOperationResults(operationResults, "112229", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
        }

        /// <summary>
        /// Validate the data of specified parameters in category attribute mapping
        /// </summary>
        /// <param name="categoryAttributeMappings">Indicates the collection of category attribute mapping</param>
        /// <param name="operationResults">Indicates the result of operation</param>
        /// <param name="callerContext">Indicates the context of the caller</param>
        private void ValidateData(CategoryAttributeMappingCollection categoryAttributeMappings, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            if (categoryAttributeMappings != null && categoryAttributeMappings.Count > 0)
            {
                Collection<String> categoryAttributeMappingList = new Collection<String>();
                AttributeModelCollection attributeModels = new AttributeModelCollection();

                foreach (CategoryAttributeMapping categoryAttributeMapping in categoryAttributeMappings)
                {
                    AttributeModel attributeModel = DataModelHelper.GetAttributeModel(_iAttributeModelManager, categoryAttributeMapping.AttributeId, categoryAttributeMapping.AttributeName, categoryAttributeMapping.AttributeParentName, ref attributeModels, callerContext);
                    IDataModelOperationResult dataModelOperationResult =
                           operationResults.GetByReferenceId(categoryAttributeMapping.ReferenceId);

                    if (attributeModel != null && attributeModel.AttributeModelType != AttributeModelType.Category)
                    {
                        Object[] parameters = new Object[]
                        {
                            attributeModel.Name, attributeModel.AttributeParentName,
                            attributeModel.AttributeModelType.ToString().ToLowerInvariant(),
                            categoryAttributeMapping.CategoryName
                        };
                        String errorMessage =
                            "Attribute '{0}' under '{1}' group is of the type {2}, so mapping cannot be performed for category '{3}'.";

                        DataModelHelper.AddOperationResult(dataModelOperationResult, "113959",
                            String.Format(errorMessage, parameters), parameters, OperationResultType.Error,
                            TraceEventType.Error, callerContext);
                    }
                    else
                    {
                        Collection<String> uniqueIdentifierList = new Collection<String>()
                        {
                            categoryAttributeMapping.HierarchyName,
                            categoryAttributeMapping.Path,
                            categoryAttributeMapping.AttributeName,
                            categoryAttributeMapping.AttributeParentName
                        };

                        String rowUniqueIdentifier = DataModelHelper.GetUniqueIdentifier(uniqueIdentifierList);
                       
                        DataModelHelper.ValidateHierarchyName(categoryAttributeMapping.HierarchyName,
                            dataModelOperationResult, callerContext);
                        DataModelHelper.ValidateCategoryName(categoryAttributeMapping.CategoryName,
                            dataModelOperationResult, callerContext);

                        DataModelHelper.ValidateAttributeUniqueIdentifier(categoryAttributeMapping.AttributeName,
                            categoryAttributeMapping.AttributeParentName,
                            dataModelOperationResult, callerContext);

                        if (categoryAttributeMappingList.Contains(rowUniqueIdentifier))
                        {
                            Object[] parameters = null;
                            parameters = new Object[]
                            {
                                categoryAttributeMapping.HierarchyName, categoryAttributeMapping.CategoryName,
                                categoryAttributeMapping.AttributeName,
                                categoryAttributeMapping.AttributeParentName
                            };

                            String errorMessage =
                                String.Format(
                                    "Duplicate mappings found for the category attribute mapping with hierarchy: {0}, category: {1}, attribute: {2}, and attribute parent: {3}",
                                    parameters);

                            DataModelHelper.AddOperationResult(dataModelOperationResult, "113969", errorMessage,
                                parameters,
                                OperationResultType.Error, TraceEventType.Error, callerContext);
                        }
                        else
                        {
                            categoryAttributeMappingList.Add(rowUniqueIdentifier);
                        }
                    }
                    ValidateCategoryMappingDataModelProperties(categoryAttributeMapping, attributeModel.Inheritable,
                            dataModelOperationResult, callerContext);
                }
            }
        }

        #endregion

        #region Get Methods

        /// <summary>
        /// Get all category - attribute mappings in the system
        /// </summary>
        /// <param name="getFromCacheCall">The get from cache call.</param>
        /// <param name="dbCall">The database call.</param>
        /// <param name="setToCacheCall">The set to cache call.</param>
        /// <param name="callerContext">Indicates name of application and module.</param>
        /// <returns>
        /// All category - attribute mappings
        /// </returns>
        private CategoryAttributeMappingCollection Get(Int64 categoryId, Int32 containerId, Int32 hierarchyId, Boolean getLatest, CallerContext callerContext)
        {
            CategoryAttributeMappingCollection categoryAttributeMappings = null;

            try
            {
                #region Initial Setup

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StartTraceActivity("CategoryAttributeMappingBL.GetCategoryAttributeMapping", MDMTraceSource.DataModel, false);
                }

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "CallerContext : Application - " + callerContext.Application + " Module - " + callerContext.Module);
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Finding category - attribute mappings in cache...", MDMTraceSource.DataModel);
                }

                #endregion Initial Setup

                #region Get category attribute mappings from cache if available

                if (!getLatest)
                {
                    categoryAttributeMappings = _mappingBufferManager.FindCategoryAttributeMappingsCompleteDetails(categoryId);
                }

                #endregion Get category attribute mappings from cache if available

                #region Get category attribute mappings from Database

                if (categoryAttributeMappings == null)
                {
                    #region Load category attribute mappings from DB

                    if (Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "No category - attribute mapping cache found.Now all category - attribute mappings would be loaded from database.", MDMTraceSource.DataModel);

                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Loading category - attribute mappings from database...", MDMTraceSource.DataModel);
                    }

                    //Get command
                    DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

                    categoryAttributeMappings = _categoryAttributeMappingDA.Get(categoryId, containerId, hierarchyId,_systemDataLocale, command);

                    if (Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Loaded CategoryAttributeMappings from database.", MDMTraceSource.DataModel);
                    }

                    #endregion Load category attribute mappings from DB

                    #region Update category attribute mappings into cache

                    //Category attribute mappings manager supports caching mechanism only if mappings are requested by category id.
                    //We don't have any business scenario where we need all mappings by hierarchy id or container id. 
                    if (categoryAttributeMappings != null && categoryId > 0)
                    {
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Caching {0} category - attribute mappings ...", categoryAttributeMappings.Count, MDMTraceSource.DataModel));
                        }

                        _mappingBufferManager.UpdateCategoryAttributeMappings(categoryAttributeMappings, categoryId, 3);

                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Done with caching for category - attribute mappings.", MDMTraceSource.DataModel);
                        }
                    }

                    #endregion Update category attribute mappings into cache
                }
                else
                {
                    if (Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Found {0} category - attribute mappings in cache.", categoryAttributeMappings.Count), MDMTraceSource.DataModel);
                    }
                }

                #endregion Get category attribute mappings from Database
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("CategoryAttributeMappingBL.GetCategoryAttributeMappings", MDMTraceSource.DataModel);
                }
            }

            return categoryAttributeMappings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryAttributeMappings"></param>
        /// <param name="callerContext"></param>
        private void LoadOriginalCategoryAttributeMapping(CategoryAttributeMappingCollection categoryAttributeMappings, CallerContext callerContext)
        {
            HierarchyCollection hierarchies = new HierarchyCollection();
            Dictionary<Int32, CategoryCollection> hierarchyIdBaseCategories = new Dictionary<Int32, CategoryCollection>();
            Dictionary<Int64, CategoryAttributeMappingCollection> categoryIdBasedOriginalCategoryAttributeMappings = new Dictionary<Int64, CategoryAttributeMappingCollection>();

            foreach (CategoryAttributeMapping categoryAttributeMapping in categoryAttributeMappings)
            {
                if (categoryAttributeMapping.CategoryId < 1)
                {
                    categoryAttributeMapping.CategoryId = DataModelHelper.GetCategoryId(_iCategoryManager, _iHierarchyManager, categoryAttributeMapping.CategoryName, categoryAttributeMapping.Path, categoryAttributeMapping.HierarchyName, ref hierarchyIdBaseCategories, ref hierarchies, callerContext);
                }

                CategoryAttributeMappingCollection originalCategoryAttributeMappings = LoadAndPopulateCategoryIdBasedAttributeMappings(categoryAttributeMapping.CategoryId, callerContext, ref categoryIdBasedOriginalCategoryAttributeMappings);

                if (originalCategoryAttributeMappings != null)
                {
                    categoryAttributeMapping.OriginalCategoryAttributeMapping = originalCategoryAttributeMappings.Get(categoryAttributeMapping.HierarchyName, categoryAttributeMapping.CategoryName, categoryAttributeMapping.Path, categoryAttributeMapping.AttributeName, categoryAttributeMapping.AttributeParentName, categoryAttributeMapping.SourceFlag) as CategoryAttributeMapping;
                }
            }
        }

        private CategoryAttributeMappingCollection LoadAndPopulateCategoryIdBasedAttributeMappings(Int64 categoryId, CallerContext callerContext, ref Dictionary<Int64, CategoryAttributeMappingCollection> categoryIdBasedOriginalCategoryAttributeMappings)
        {
            CategoryAttributeMappingCollection originalCategoryAttributeMappings = null;
            categoryIdBasedOriginalCategoryAttributeMappings.TryGetValue(categoryId, out originalCategoryAttributeMappings);

            if (originalCategoryAttributeMappings == null)
            {
                originalCategoryAttributeMappings = GetByCategoryId(categoryId, callerContext);
                categoryIdBasedOriginalCategoryAttributeMappings.Add(categoryId, originalCategoryAttributeMappings);
            }

            return originalCategoryAttributeMappings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryAttributeMappings"></param>
        /// <param name="callerContext"></param>
        private void FillCategoryAttributeMapping(CategoryAttributeMappingCollection categoryAttributeMappings, CallerContext callerContext)
        {
            HierarchyCollection hierarchies = new HierarchyCollection();
            Dictionary<Int32, CategoryCollection> hierarchyIdBaseCategories = new Dictionary<Int32, CategoryCollection>();
            AttributeModelCollection attributeModels = new AttributeModelCollection();

            foreach (CategoryAttributeMapping categoryAttributeMapping in categoryAttributeMappings)
            {
                if (categoryAttributeMapping.OriginalCategoryAttributeMapping != null)
                {
                    categoryAttributeMapping.Id = categoryAttributeMapping.OriginalCategoryAttributeMapping.Id;
                    categoryAttributeMapping.HierarchyId = categoryAttributeMapping.OriginalCategoryAttributeMapping.HierarchyId;
                    categoryAttributeMapping.CategoryId = categoryAttributeMapping.OriginalCategoryAttributeMapping.CategoryId;
                    categoryAttributeMapping.AttributeId = categoryAttributeMapping.OriginalCategoryAttributeMapping.AttributeId;
                    categoryAttributeMapping.AttributeParentId = categoryAttributeMapping.OriginalCategoryAttributeMapping.AttributeParentId;
                }
                else
                {
                    categoryAttributeMapping.HierarchyId = DataModelHelper.GetHierarchyId(_iHierarchyManager, categoryAttributeMapping.HierarchyName, ref hierarchies, callerContext);
                    categoryAttributeMapping.CategoryId = DataModelHelper.GetCategoryId(_iCategoryManager, _iHierarchyManager, categoryAttributeMapping.CategoryName, categoryAttributeMapping.Path, categoryAttributeMapping.HierarchyName, ref hierarchyIdBaseCategories, ref hierarchies, callerContext);
                    AttributeModel attributeModel = DataModelHelper.GetAttributeModel(_iAttributeModelManager, categoryAttributeMapping.AttributeId, categoryAttributeMapping.AttributeName, categoryAttributeMapping.AttributeParentName, ref attributeModels, callerContext);

                    if (attributeModel != null)
                    {
                        categoryAttributeMapping.AttributeId = attributeModel.Id;
                        categoryAttributeMapping.AttributeParentId = attributeModel.AttributeParentId;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryAttributeMappings"></param>
        /// <param name="operationResults"></param>
        /// <param name="callerContext"></param>
        private void CompareAndMerge(CategoryAttributeMappingCollection categoryAttributeMappings, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            foreach (CategoryAttributeMapping deltaCategoryAttributeMapping in categoryAttributeMappings)
            {
                IDataModelOperationResult operationResult = operationResults.GetByReferenceId(deltaCategoryAttributeMapping.ReferenceId);

                //If delta object Action is Read/Ignore then skip Merge Delta Process.
                if (deltaCategoryAttributeMapping.Action == ObjectAction.Read || deltaCategoryAttributeMapping.Action == ObjectAction.Ignore)
                    continue;

                ICategoryAttributeMapping originalCategoryAttributeMapping = deltaCategoryAttributeMapping.OriginalCategoryAttributeMapping;

                if (originalCategoryAttributeMapping != null)
                {
                    //If delta object Action is Delete then skip Merge Delta process.
                    if (deltaCategoryAttributeMapping.Action != ObjectAction.Delete)
                    {
                        originalCategoryAttributeMapping.MergeDelta(deltaCategoryAttributeMapping, callerContext, false);
                    }
                }
                else
                {
                    if (deltaCategoryAttributeMapping.Action == ObjectAction.Delete)
                    {
                        Object[] parameters = new Object[]
                        {
                            deltaCategoryAttributeMapping.HierarchyName, deltaCategoryAttributeMapping.CategoryName,
                            deltaCategoryAttributeMapping.AttributeName, deltaCategoryAttributeMapping.AttributeParentName
                        };
                        DataModelHelper.AddOperationResult(operationResult, "113622", String.Empty, parameters, OperationResultType.Error,
                            TraceEventType.Error, callerContext);
                    }
                    else
                    {
                        List<String> parameters = new List<String>();
                        List<String> errorMessages = new List<String>();

                        if (deltaCategoryAttributeMapping.HierarchyId < 1)
                        {
                            parameters.Add(deltaCategoryAttributeMapping.HierarchyName);
                            errorMessages.Add(DataModelHelper.GetLocaleMessageText("100494", String.Empty, null, callerContext)); // 100494 - Hierarchy
                        }
                        if (deltaCategoryAttributeMapping.CategoryId < 1)
                        {
                            parameters.Add(deltaCategoryAttributeMapping.CategoryName);
                            errorMessages.Add(DataModelHelper.GetLocaleMessageText("100205", String.Empty, null, callerContext)); // 100205 - Category
                        }
                        if (deltaCategoryAttributeMapping.AttributeParentId < 1)
                        {
                            parameters.Add(deltaCategoryAttributeMapping.AttributeParentName);
                            errorMessages.Add(DataModelHelper.GetLocaleMessageText("112803", String.Empty, null, callerContext)); // 112803 - Attribute Parent Name
                        }
                        if (deltaCategoryAttributeMapping.AttributeId < 1)
                        {
                            parameters.Add(deltaCategoryAttributeMapping.AttributeName);
                            errorMessages.Add(DataModelHelper.GetLocaleMessageText("100163", String.Empty, null, callerContext)); // 100163 - Attribute Name
                        }

                        if (errorMessages.Count > 0)
                        {
                            DataModelHelper.AddInvalidNamesErrorsToOperationResult(operationResult, errorMessages, parameters, callerContext);
                        }

                        deltaCategoryAttributeMapping.Action = ObjectAction.Create;
                    }
                }

                operationResult.PerformedAction = deltaCategoryAttributeMapping.Action;
            }
        }

        #endregion

        #region Process Methods

        /// <summary>
        /// Create - Update or Delete given categoryAttributeMapping
        /// </summary>
        /// <param name="categoryAttributeMappingCollection">The category attribute mapping collection.</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>
        /// Result of the operation
        /// </returns>
        /// <exception cref="MDMOperationException"></exception>
        private OperationResultCollection ProcessCollection(CategoryAttributeMappingCollection categoryAttributeMappings, IEntityManager entityManager, CallerContext callerContext, IAttributeModelManager iAttributeModelManager)
        {
            OperationResultCollection operationResultCollection = new OperationResultCollection();

            try
            {
                #region Initial Setup

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StartTraceActivity("CategoryAttributeMappingBL.ProcessCategoryAttributeMappings", MDMTraceSource.DataModel, false);
                }

                Collection<Int32> attributeIdsWithDeleteAction = new Collection<Int32>();
                Collection<Int32> attributeIdsWithUpdateAction = new Collection<Int32>();
                Collection<Int64> categoryIdList = new Collection<Int64>();
                Dictionary<Int64, CategoryAttributeMappingCollection> categoryIdBasedOriginalCategoryAttributeMappings = new Dictionary<Int64, CategoryAttributeMappingCollection>();

                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Update);
                
                String userName = PopulateUserName();
                PopulateProgramName(callerContext);
                
                AttributeModelCollection baseAttributeModels = iAttributeModelManager.GetAllBaseAttributeModels();

                #endregion Initial Setup

                #region Load Original Category Attribute Mappings

                foreach (CategoryAttributeMapping categoryAttributeMapping in categoryAttributeMappings)
                {
                    CategoryAttributeMappingCollection originalCategoryAttributeMappings = LoadAndPopulateCategoryIdBasedAttributeMappings(categoryAttributeMapping.CategoryId, callerContext, ref categoryIdBasedOriginalCategoryAttributeMappings);

                    if (originalCategoryAttributeMappings != null)
                    {
                        categoryAttributeMapping.OriginalCategoryAttributeMapping = originalCategoryAttributeMappings.Get(categoryAttributeMapping.CategoryId, categoryAttributeMapping.AttributeId, categoryAttributeMapping.SourceFlag) as CategoryAttributeMapping;

                        if (categoryAttributeMapping.HierarchyId < 1)
                        {
                            if (categoryAttributeMapping.OriginalCategoryAttributeMapping != null)
                            {
                                //Fill missing information over here.
                                categoryAttributeMapping.HierarchyId = categoryAttributeMapping.OriginalCategoryAttributeMapping.HierarchyId;
                            }
                            else
                            {
                                //If original category attribute mapping is not found then check inherited attribute is available.
                                //if yes then fill missing information.
                                CategoryAttributeMapping inheritedCategoryAttributeMapping = originalCategoryAttributeMappings.GetByAttributeId(categoryAttributeMapping.AttributeId) as CategoryAttributeMapping;

                                if (inheritedCategoryAttributeMapping != null)
                                {
                                    categoryAttributeMapping.HierarchyId = inheritedCategoryAttributeMapping.HierarchyId;
                                }
                            }
                        }
                    }
                }

                #endregion Load Original Category Attribute Mappings

                #region Fill Category Attribute Mappings

                Int32 categoryAttributeMappingToBeCreated = -1;

                OperationResult operationResult = null;

                //Create 2 separate buckets for attribute Ids. 1 for delete and 1 for create/update.
                //Deleted mapping's attributes cache is cleared before deleting. As after delete, Get won't find value.
                foreach (CategoryAttributeMapping categoryAttributeMapping in categoryAttributeMappings)
                {
                    AttributeModel baseAttributeModel = baseAttributeModels.GetAttributeModel(categoryAttributeMapping.AttributeId, _systemDataLocale) as AttributeModel;
                    categoryAttributeMapping.Id = (categoryAttributeMapping.OriginalCategoryAttributeMapping != null) ? categoryAttributeMapping.OriginalCategoryAttributeMapping.Id : categoryAttributeMappingToBeCreated--;

                    CompareAndClearBasePropertiesValues(categoryAttributeMapping, baseAttributeModel);

                    if (!categoryIdList.Contains(categoryAttributeMapping.CategoryId))
                    {
                        categoryIdList.Add(categoryAttributeMapping.CategoryId);
                    }

                    if (categoryAttributeMapping.SourceFlag == "I" && categoryAttributeMapping.Action == ObjectAction.Update)
                    {
                        categoryAttributeMapping.Id = -1;
                    }

                    if (categoryAttributeMapping.Action == ObjectAction.Delete)
                    {
                        attributeIdsWithDeleteAction.Add(categoryAttributeMapping.AttributeId);
                    }
                    else
                    {
                        attributeIdsWithUpdateAction.Add(categoryAttributeMapping.AttributeId);
                    }

                    operationResult = ValidateMappingProperties(categoryAttributeMapping, baseAttributeModel);

                    operationResultCollection.Add(operationResult);
                }

                #endregion Fill Category Attribute Mappings

                operationResultCollection.RefreshOperationResultStatus();

                if (operationResultCollection.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors ||
                    operationResultCollection.OperationResultStatus == OperationResultStatusEnum.Failed)
                {
                    return operationResultCollection;
                }
                else
                {
                    operationResultCollection = new OperationResultCollection();
                }

                #region Clear cache for deleted mapping's attribute

                if (attributeIdsWithDeleteAction.Count > 0)
                {
                    Task.Factory.StartNew(() => InvalidateImpactedAttributeIds(attributeIdsWithDeleteAction, iAttributeModelManager));
                }

                InvalidateImpactedCategories(categoryIdList, entityManager);

                #endregion Clear cache for deleted mapping's attribute

                #region Process Category Attribute Mappings

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    operationResultCollection = _categoryAttributeMappingDA.Process(categoryAttributeMappings, callerContext.ProgramName, userName, _systemDataLocale, command);

                    ProcessEntityCacheLoadContextForMappingChange(categoryAttributeMappings, callerContext);

                    #region Cache clear

                    foreach (Int32 categoryId in categoryIdList)
                    {
                        _mappingBufferManager.RemoveCategoryAttributeMappings(categoryId, true);
                    }

                    if (attributeIdsWithUpdateAction.Count > 0)
                    {
                        Task.Factory.StartNew(() => InvalidateImpactedAttributeIds(attributeIdsWithUpdateAction, iAttributeModelManager));
                    }

                    #endregion Cache clear

                    transactionScope.Complete();
                }

                #region activity log

                if (operationResult.OperationResultStatus == OperationResultStatusEnum.Successful && AppConfigurationHelper.GetAppConfig<Boolean>(Constants.ACTIVITYLOG_DATAMODEL_ENABLED_APPCONFIG_KEY, false))
                {
                    LogDataModelChanges(categoryAttributeMappings, callerContext);
                }

                #endregion activity log

                #endregion Process Category Attribute Mappings
            }
            catch (Exception ex)
            {
                throw new MDMOperationException(ex.Message, ex);
            }
            finally
            {
                #region Populate Operation Result

                for (Int32 i = 0; i < categoryAttributeMappings.Count; i++)
                {
                    PopulateOperationResult(categoryAttributeMappings.Skip(i).Take(1).First(), operationResultCollection.Skip(i).Take(1).First());
                }

                #endregion
            }

            return operationResultCollection;
        }

        /// <summary>
        /// This method compares delta category attribute mapping object vs. base attribute model.
        /// If delta category attribute mapping object and base attribute model values are same then set it null into mapping tables.
        /// </summary>
        /// <param name="categoryAttributeMapping">Indicates category attribute mapping object.</param>
        /// <param name="baseAttributeModel">Indicate base attribute model object.</param>
        private void CompareAndClearBasePropertiesValues(CategoryAttributeMapping categoryAttributeMapping, AttributeModel baseAttributeModel)
        {
            if (categoryAttributeMapping != null && baseAttributeModel != null)
            {
                if (categoryAttributeMapping.SortOrder == baseAttributeModel.SortOrder)
                {
                    categoryAttributeMapping.SortOrder = null;
                }
                if (categoryAttributeMapping.Required == baseAttributeModel.Required)
                {
                    categoryAttributeMapping.Required = null;
                }
                if (categoryAttributeMapping.ReadOnly == baseAttributeModel.ReadOnly)
                {
                    categoryAttributeMapping.ReadOnly = null;
                }
                if (categoryAttributeMapping.ShowAtCreation == baseAttributeModel.ShowAtCreation)
                {
                    categoryAttributeMapping.ShowAtCreation = null;
                }
                if (categoryAttributeMapping.AllowableValues == baseAttributeModel.AllowableValues)
                {
                    categoryAttributeMapping.AllowableValues = null;
                }
                if (categoryAttributeMapping.MaxLength == baseAttributeModel.MaxLength)
                {
                    categoryAttributeMapping.MaxLength = null;
                }
                if (categoryAttributeMapping.MinLength == baseAttributeModel.MinLength)
                {
                    categoryAttributeMapping.MinLength = null;
                }
                if (categoryAttributeMapping.AllowableUOM == baseAttributeModel.AllowableUOM)
                {
                    categoryAttributeMapping.AllowableUOM = null;
                }
                if (categoryAttributeMapping.DefaultUOM == baseAttributeModel.DefaultUOM)
                {
                    categoryAttributeMapping.DefaultUOM = null;
                }
                if (categoryAttributeMapping.Precision == baseAttributeModel.Precision)
                {
                    categoryAttributeMapping.Precision = null;
                }
                if (categoryAttributeMapping.MinInclusive == baseAttributeModel.MinInclusive)
                {
                    categoryAttributeMapping.MinInclusive = null;
                }
                if (categoryAttributeMapping.MinExclusive == baseAttributeModel.MinExclusive)
                {
                    categoryAttributeMapping.MinExclusive = null;
                }
                if (categoryAttributeMapping.MaxInclusive == baseAttributeModel.MaxInclusive)
                {
                    categoryAttributeMapping.MaxInclusive = null;
                }
                if (categoryAttributeMapping.MaxExclusive == baseAttributeModel.MaxExclusive)
                {
                    categoryAttributeMapping.MaxExclusive = null;
                }
                if (categoryAttributeMapping.Definition == baseAttributeModel.Definition)
                {
                    categoryAttributeMapping.Definition = null;
                }
                if (categoryAttributeMapping.AttributeExample == baseAttributeModel.AttributeExample)
                {
                    categoryAttributeMapping.AttributeExample = null;
                }
                if (categoryAttributeMapping.BusinessRule == baseAttributeModel.BusinessRule)
                {
                    categoryAttributeMapping.BusinessRule = null;
                }
                if (categoryAttributeMapping.DefaultValue == baseAttributeModel.DefaultValue)
                {
                    categoryAttributeMapping.DefaultValue = null;
                }
                if (categoryAttributeMapping.ExportMask == baseAttributeModel.ExportMask)
                {
                    categoryAttributeMapping.ExportMask = null;
                }
            }
        }

        #endregion

        #region EntityCacheLoadContext For Category Attribute Mapping

        /// <summary>
        /// Processes the EntityCacheLoadContext for category mapping change.
        /// </summary>        
        private void ProcessEntityCacheLoadContextForMappingChange(CategoryAttributeMappingCollection categoryAttributeMappingCollection, CallerContext callerContext)
        {
            Dictionary<Int64, Collection<Int32>> entityCacheContextRequest = BuildCategoryAttributeMappingList(categoryAttributeMappingCollection);
            if (entityCacheContextRequest.Count > 0)
            {
                // Create EntityCacheLoadContext entityCacheContextRequest
                EntityCacheLoadContext entityCacheLoadContext = new EntityCacheLoadContext();
                entityCacheLoadContext.CacheStatus = (Int32)(EntityCacheComponentEnum.InheritedAttributes | EntityCacheComponentEnum.OverriddenAttributes);

                foreach (KeyValuePair<Int64, Collection<Int32>> item in entityCacheContextRequest)
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
        /// Splits the CategoryAttributeMappingCollection into a dictionary of category id and attribute id list.
        /// </summary>
        private Dictionary<Int64, Collection<Int32>> BuildCategoryAttributeMappingList(CategoryAttributeMappingCollection categoryAttributeMappingCollection)
        {
            Dictionary<Int64, Collection<Int32>> entityCacheContextRequest = new Dictionary<Int64, Collection<Int32>>();

            foreach (CategoryAttributeMapping categoryAttributeMapping in categoryAttributeMappingCollection)
            {
                if (categoryAttributeMapping.Action == ObjectAction.Create || categoryAttributeMapping.Action == ObjectAction.Delete)
                {
                    if (entityCacheContextRequest.ContainsKey(categoryAttributeMapping.CategoryId))
                    {
                        Collection<Int32> valueList = entityCacheContextRequest[categoryAttributeMapping.CategoryId];
                        valueList.Add(categoryAttributeMapping.AttributeId);
                    }
                    else
                    {
                        entityCacheContextRequest.Add(categoryAttributeMapping.CategoryId,
                            new Collection<Int32>() { categoryAttributeMapping.AttributeId });
                    }
                }
            }

            return entityCacheContextRequest;
        }

        /// <summary>
        /// Creates an EntityCacheLoadContextItemCollection for the specified category id and attribute id list.
        /// </summary>
        private EntityCacheLoadContextItemCollection GetEntityCacheLoadContextItemCollection(Int64 categoryId, Collection<Int32> attributeIdList)
        {
            // Create EntityCacheLoadContextItemCollection
            EntityCacheLoadContextItemCollection entityCacheLoadContextItemCollection = new EntityCacheLoadContextItemCollection();

            // Create EntityCacheLoadContextItem for Category
            EntityCacheLoadContextItem entityCacheLoadContextItemForCategory =
                entityCacheLoadContextItemCollection.GetOrCreateEntityCacheLoadContextItem(EntityCacheLoadContextTypeEnum.Category);
            entityCacheLoadContextItemForCategory.AddValues(categoryId);

            // Create EntityCacheLoadContextItem for Attributes
            EntityCacheLoadContextItem entityCacheLoadContextItemForAttributes =
                entityCacheLoadContextItemCollection.GetOrCreateEntityCacheLoadContextItem(EntityCacheLoadContextTypeEnum.Attribute);
            entityCacheLoadContextItemForAttributes.AddValues(attributeIdList);

            return entityCacheLoadContextItemCollection;
        }

        #endregion

        #region Cache Methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="impactedAttributeIds"></param>
        /// <param name="iAttributeModelManager"></param>
        private void InvalidateImpactedAttributeIds(Collection<Int32> impactedAttributeIds, IAttributeModelManager iAttributeModelManager)
        {
            try
            {
                #region Invalidate attribute model cache

                var attributeModelBufferManager = new AttributeModelBufferManager();
                attributeModelBufferManager.InvalidateImpactedAttributeModels(impactedAttributeIds, iAttributeModelManager, true, true);

                #endregion
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryIdList"></param>
        /// <param name="entityManager"></param>
        private void InvalidateImpactedCategories(IEnumerable<Int64> categoryIdList, IEntityManager entityManager)
        {
            _mappingBufferManager.RemoveImpactedCategoryAttributeMappingFromCache(categoryIdList, entityManager);
            _mappingBufferManager.NotifyModelDataChange(CacheKeyContants.TECHNICAL_ATTRIBUTE_CHANGED_KEY);
        }

        #endregion

        #region Misc. Methods

        /// <summary>
        /// Validates container entity type attribute mapping properties.
        /// </summary>
        /// <param name="attributeModels">Specifies attribute models for validation</param>
        /// <param name="operationResult">Specifies validation operation result</param>
        public static OperationResult ValidateMappingProperties(CategoryAttributeMapping mapping, AttributeModel attributeModel)
        {
            OperationResult operationResult = new OperationResult();
            String errorMessage = String.Empty;
            String attributeNameList = String.Empty;

            List<String> inheritableAttrName = new List<String>();
            List<String> requiredAttrName = new List<String>();
           
            if (attributeModel != null && attributeModel.Id == mapping.AttributeId)
            {
                if (!attributeModel.Inheritable && mapping.InheritableOnly == true)
                {
                    inheritableAttrName.Add(String.Format("'{0}'", attributeModel.LongName));
                }

                if (mapping.Required == true && mapping.InheritableOnly == true)
                {
                    requiredAttrName.Add(String.Format("'{0}'", attributeModel.LongName));
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

            else if (requiredAttrName.Count > 0)
            {
                String requiredAttrNameString = String.Join(",", inheritableAttrName);

                object[] parameters = new Object[] { requiredAttrNameString };

                errorMessage = String.Format("Failed to set attribute {0} as both 'Inheritable Only' and 'Required' attribute. Select either 'Inheritable Only' or 'Required'.",
                                parameters);

                operationResult.AddOperationResult("114242", errorMessage, parameters, OperationResultType.Error);
            }
            else
            {
                operationResult.AddOperationResult("", "Category - Attribute Mapping ID: " + mapping.Id, OperationResultType.Information);
                operationResult.Id = mapping.Id;
                operationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                operationResult.ReturnValues.Add(mapping.Id);
            }

            return operationResult;
        }

        /// <summary>
        /// Validates category attribute mapping properties.
        /// </summary>
        /// <param name="attributeModels">Specifies attribute models for validation</param>
        /// <param name="operationResult">Specifies validation operation result</param>
        public static void ValidateCategoryMappingDataModelProperties(CategoryAttributeMapping mapping, Boolean isInheritable,
                                                                           IDataModelOperationResult iDataModelOperationResult, CallerContext callerContext)
        {
            String errorMessage = String.Empty;
            Object[] parameters = new Object[] { mapping.HierarchyName, mapping.CategoryName, mapping.AttributeName };

            if (mapping.InheritableOnly == true && !isInheritable)
            {
                errorMessage = String.Format("Failed to set category attribute mapping with hierarchy '{0}', category '{1}', and attribute '{2}' to 'Inheritable Only', as it is non-inheritable attribute.",
                                                parameters);

                DataModelHelper.AddOperationResult(iDataModelOperationResult, "114243", errorMessage, parameters, OperationResultType.Error, TraceEventType.Error, callerContext);
            }

            if (mapping.InheritableOnly == true && mapping.Required == true)
            {
                errorMessage = String.Format("Failed to set category attribute mapping with hierarchy '{0}', category '{1}', and attribute '{2}' as both 'Inheritable Only' and 'Required'. Select either 'Inheritable Only' or 'Required'.",
                                                parameters);

                DataModelHelper.AddOperationResult(iDataModelOperationResult, "114244", errorMessage, parameters, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
        }

        /// <summary>
        /// Log changes to tb_datamodelactivitylog if DA call is sucessfull and appconfig key datamodelactivitylog.enable == true
        /// </summary>
        /// <param name="CategoryAttributeMappingCollection">Indicates CategoryAttributeMappingCollection to be logged as a data model activity log</param>
        /// <param name="callerContext">Indicates caller context</param>
        private void LogDataModelChanges(CategoryAttributeMappingCollection CategoryAttributeMappings, CallerContext callerContext)
        {
            #region Step: Populate datamodelactivitylog object

            DataModelActivityLogBL dataModelActivityLogManager = new DataModelActivityLogBL();
            DataModelActivityLogCollection activityLogCollection = dataModelActivityLogManager.FillDataModelActivityLogCollection(CategoryAttributeMappings, CategoryAttributeMappings.DataModelObjectType, callerContext);

            #endregion Step: Populate datamodelactivitylog object

            #region Step: Make api call

            if (activityLogCollection != null && activityLogCollection.Count > 0) // null activitylog collection means there was error in mapping
            {
                dataModelActivityLogManager.Process(activityLogCollection, callerContext);
            }

            #endregion Step: Make api call
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
        /// <param name="categoryAttributeMapping"></param>
        /// <param name="categoryAttrMappingProcessOperationResult"></param>
        private static void PopulateOperationResult(CategoryAttributeMapping categoryAttributeMapping, OperationResult categoryAttrMappingProcessOperationResult)
        {
            if (categoryAttributeMapping.Action == ObjectAction.Create)
            {
                if (categoryAttrMappingProcessOperationResult.ReturnValues.Any())
                {
                    categoryAttrMappingProcessOperationResult.Id =
                        Convert.ToInt32(categoryAttrMappingProcessOperationResult.ReturnValues[0]);
                }
            }
            else
            {
                categoryAttrMappingProcessOperationResult.Id = categoryAttributeMapping.Id;
            }

            categoryAttrMappingProcessOperationResult.ReferenceId = String.IsNullOrEmpty(categoryAttributeMapping.ReferenceId)
                ? categoryAttributeMapping.Name
                : categoryAttributeMapping.ReferenceId;
        }

        /// <summary>
        /// 
        /// </summary>
        ///<param name="callerContext">CallerContext</param>
        private static void PopulateProgramName(CallerContext callerContext)
        {
            if (String.IsNullOrWhiteSpace(callerContext.ProgramName))
            {
                callerContext.ProgramName = "CategoryAttributeMappingBL";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private String PopulateUserName()
        {
            if (_securityPrincipal != null)
            {
                return _securityPrincipal.CurrentUserName;
            }

            return String.Empty;
        }

        #endregion

        #endregion Private Methods

        #endregion
    }
}