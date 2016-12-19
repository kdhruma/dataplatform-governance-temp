using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace MDM.CategoryManager.Business
{
    using MDM.AdminManager.Business;
    using MDM.BufferManager;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.DataModel;
    using MDM.CategoryManager.Data;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.DataModelManager.Business;
    using MDM.HierarchyManager.Business;
    using MDM.Interfaces;
    using MDM.MessageManager.Business;
    using MDM.Utility;
    using MDM.ContainerManager.Business;
    using MDM.Core.Extensions;

    /// <summary>
    /// Specifies the business operations for category
    /// </summary>
    public class CategoryBL : BusinessLogicBase, ICategoryManager, IDataModelManager
    {
        #region Fields

        /// <summary>
        /// Field denoting reference of EntityBL.
        /// </summary>
        private IEntityManager _iEntityManager = null;

        /// <summary>
        /// Field denoting reference of EntityLocaleBL.
        /// </summary>
        private IDataModelManager _iEntityLocaleDataManager = null;
        
        /// <summary>
        /// 
        /// </summary>
        private readonly ICategoryDataProvider _categoryDataProvider;

        /// <summary>
        /// Field denoting system data locale..
        /// </summary>
        private LocaleEnum _systemDataLocale = LocaleEnum.UnKnown;

        /// <summary>
        /// Field denoting system UI locale..
        /// </summary>
        private LocaleEnum _systemUILocale = LocaleEnum.UnKnown;

        /// <summary>
        /// Field denoting locale Message
        /// </summary>
        private LocaleMessage _localeMessage = null;

        /// <summary>
        /// Field denoting localeMessageBL
        /// </summary>
        private LocaleMessageBL _localeMessageBL = new LocaleMessageBL();

        /// <summary>
        /// Field denoting the category path separator
        /// </summary>
        private String _categoryPathSeparator = String.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Category BL.
        /// </summary>
        public CategoryBL()
            : base()
        {
            this._categoryDataProvider = new CategoryDA();
            InitializeMembers();
        }

        /// <summary>
        /// Initializes a new instance of the Category BL.
        /// </summary>
        /// <param name="iEntityManager">reference of EntityBL.</param>
        /// <param name="iEntityLocaleDataManager">reference of EntityLocaleBL.</param>
        public CategoryBL(IEntityManager iEntityManager, IDataModelManager iEntityLocaleDataManager)
        {
            this._iEntityManager = iEntityManager;
            this._iEntityLocaleDataManager = iEntityLocaleDataManager;
            this._categoryDataProvider = new CategoryDA();
            InitializeMembers();
        }

        /// <summary>
        /// 
        /// </summary>
        public CategoryBL(ICategoryDataProvider categoryDataProvider)
        {
            _categoryDataProvider = categoryDataProvider;
            InitializeMembers();
        }

        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Search Categories for requested context
        /// </summary>
        /// <param name="categoryContext">Search Context containing hierarchy id, locale and other criteria. </param>
        /// <param name="callerContext">Context indicating the caller of the API</param>
        /// <returns>Collection of Category</returns>
        public CategoryCollection Search(CategoryContext categoryContext, CallerContext callerContext)
        {
            return SearchCategories(categoryContext, callerContext);
        }

        /// <summary>
        /// Search Categories for requested context
        /// </summary>
        /// <param name="containerNames">Collection of container names in which the category belongs</param>
        /// <param name="categoryLongNames">Collection of category long names in which result has to be returned</param>
        /// <param name="dataLocales">DataLocales in which result has to be returned.</param>
        /// <param name="callerContext">Context indicating the caller of the API</param>
        /// <returns>Collection of Category</returns>
        public CategoryCollection Search(Collection<String> containerNames, Collection<String> categoryLongNames, Collection<LocaleEnum> dataLocales, CallerContext callerContext)
        {
            #region Parameter Validation

            if (callerContext == null)
            {
                _localeMessage = _localeMessageBL.TryGet(_systemUILocale, "111846", "CallerContext cannot be null", false, callerContext);

                if (_localeMessage != null)
                {
                    throw new Exception(_localeMessage.Message);
                }
            }

            #endregion

            #region Initial Setup

            CategoryCollection allCategories = new CategoryCollection();
            ContainerBL containerBL = new ContainerBL();
            HashSet<Int32> allHierarchyIds = new HashSet<Int32>();

            ContainerCollection allContainers = containerBL.GetAll(callerContext);

            if (containerNames != null && containerNames.Count > 0)
            {
                foreach (String containerName in containerNames)
                {
                    ContainerCollection filteredContainers = allContainers.GetContainers(containerName);

                    if (filteredContainers != null)
                    {
                        HashSet<Int32> hierarchyIds = filteredContainers.GetHierarchyIds();

                        if (hierarchyIds != null)
                        {
                            foreach (Int32 hierarchyId in hierarchyIds)
                            {
                                allHierarchyIds.Add(hierarchyId);
                            }
                        }
                    }
                }
            }
            else
            {
                //If container names are not available then get all hierarchy ids and search.
                allHierarchyIds = allContainers.GetHierarchyIds();
            }

            #endregion

            #region Search Categories

            if (allHierarchyIds.Count > 0)
            {
                #region Prepare Category Context

                CategoryContext categoryContext = new CategoryContext();

                CategorySearchRuleCollection rules = new CategorySearchRuleCollection();

                if (categoryLongNames != null && categoryLongNames.Count > 0)
                {
                    foreach (String categoryLongName in categoryLongNames)
                    {
                        rules.Add(new CategorySearchRule(CategoryField.LongName, SearchOperator.EqualTo, categoryLongName));
                    }
                }

                categoryContext.CategorySearchRules = rules;
                categoryContext.MaxRecordsToReturn = -1;
                categoryContext.Locales = dataLocales;

                #endregion

                foreach (Int32 hierarchyId in allHierarchyIds)
                {
                    categoryContext.HierarchyId = hierarchyId;

                    CategoryCollection filteredCategories = SearchCategories(categoryContext, callerContext);

                    if (filteredCategories != null && filteredCategories.Count > 0)
                    {
                        allCategories.AddRange(filteredCategories);
                    }
                }
            }

            #endregion

            return allCategories;
        }

        /// <summary>
        /// Get Categories details By requested Hierarchy and category mapping
        /// </summary>
        /// <param name="mappingCollection">Property denotes which categories for which hierarchy ids need to use</param>
        /// <param name="callerContext">Context indicating the caller of the API</param>
        /// <returns>Collection of Categories</returns>
        public CategoryCollection GetByIds(HierachyCategoryMappingCollection mappingCollection, CallerContext callerContext)
        {
            CategoryCollection collection = new CategoryCollection();
            LocaleEnum systemDataLocale = GlobalizationHelper.GetSystemDataLocale();
            foreach (HierachyCategoryMapping item in mappingCollection)
            {
                foreach (Int64 categoryId in item.CategoryIds)
                {
                    Category category = GetById(item.HierarchyId, categoryId, systemDataLocale, callerContext);
                    if (category != null)
                    {
                        collection.Add(category);
                    }
                }
            }
            return collection;
        }

        /// <summary>
        /// Get Category details By requested HierarchyId and CategoryId
        /// </summary>
        /// <param name="hierarchyId">Hierarchy Id in which category is requested</param>
        /// <param name="categoryId">Id of an Category</param>
        /// <param name="callerContext">Context indicating the caller of the API</param>
        /// <returns>Category Information</returns>
        public Category GetById(Int32 hierarchyId, Int64 categoryId, CallerContext callerContext)
        {
            LocaleEnum systemDataLocale = GlobalizationHelper.GetSystemDataLocale();
            return GetById(hierarchyId, categoryId, systemDataLocale, callerContext);
        }

        /// <summary>
        /// Get Category details By requested HierarchyId and CategoryId
        /// </summary>
        /// <param name="hierarchyId">Hierarchy Id in which category is requested</param>
        /// <param name="categoryId">Id of an Category</param>
        /// <param name="locale">DataLocale</param>
        /// <param name="callerContext">Context indicating the caller of the API</param>
        /// <param name="applySecurity">Context indicating the caller of the API</param>
        /// <returns>Category Information</returns>
        public Category GetById(Int32 hierarchyId, Int64 categoryId, LocaleEnum locale, CallerContext callerContext, Boolean applySecurity = false)
        {
            Category category = null;
            CategoryBaseProperties categoryBaseProperties = null;
            CategoryLocaleProperties categoryLocaleProperties = null;

            Permission permission = null;

            CategoryContext categoryContext = new CategoryContext();

            #region Validations

            if (hierarchyId < 0)
            {
                LocaleMessage localeMessage = new LocaleMessageBL().Get(GlobalizationHelper.GetSystemUILocale(), "112198", false, callerContext);
                throw new MDMOperationException("112198", localeMessage.Message, "CategoryManager", String.Empty, "GetCategories");
            }

            if (categoryId < 0)
            {
                LocaleMessage localeMessage = new LocaleMessageBL().Get(GlobalizationHelper.GetSystemUILocale(), "112198", false, callerContext);
                throw new MDMOperationException("112198", localeMessage.Message, "CategoryManager", String.Empty, "GetCategories");
            }

            #endregion

            #region Get Base Properties

            DurationHelper durationHelper = new DurationHelper(DateTime.Now);

            Dictionary<Int64, CategoryBaseProperties> allCategoryBaseProperties = GetBaseCategories(hierarchyId, categoryContext, callerContext);

            if (Constants.PERFORMANCE_TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - To load category base properties for category id {1} in hierarchy {2}", durationHelper.GetDurationInMilliseconds(DateTime.Now), categoryId, hierarchyId), MDMTraceSource.CategoryGet);

            #endregion

            #region Filter Category by Id using dictionary index

            categoryBaseProperties = GetBaseCategoryByKey(allCategoryBaseProperties, categoryId);

            if (Constants.PERFORMANCE_TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - To filter categories for category id {1} in hierarchy {2}", durationHelper.GetDurationInMilliseconds(DateTime.Now), categoryId, hierarchyId), MDMTraceSource.CategoryGet);

            #endregion

            if (categoryBaseProperties != null)
            {
                #region Apply security

                Boolean isCategoryHavingPermissions = false;

                if (applySecurity)
                {
                    SecurityPrincipal securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
                    DataSecurityBL dataSecurityBL = new DataSecurityBL(securityPrincipal);

                    //Get the permissions for each category and add to the collection only if category is having permissions..
                    //Prepare permission context.. Keep RoleId as '0' and pass the current user Id.. The Load logic determines the permissions by considering all roles of the current user.
                    PermissionContext permissionContext = new PermissionContext(0, 0, categoryId, 6, 0, categoryId, 0, 0, securityPrincipal.CurrentUserId, 0);
                    permission = dataSecurityBL.GetMDMObjectPermission(categoryId, (Int32)ObjectType.Category, "Category", permissionContext);

                    if (permission != null && permission.PermissionSet != null)
                    {
                        isCategoryHavingPermissions = true;
                    }
                }
                else
                {
                    permission = new Permission();
                    permission.PermissionSet = new Collection<UserAction> { UserAction.Add, UserAction.Update, UserAction.Delete, UserAction.View };
                    isCategoryHavingPermissions = true;
                }

                if (Constants.PERFORMANCE_TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - To apply security for category id {1} in hierarchy {2}", durationHelper.GetDurationInMilliseconds(DateTime.Now), categoryId, hierarchyId), MDMTraceSource.CategoryGet);

                #endregion

                if (isCategoryHavingPermissions)
                {
                    #region Get Local Properties

                    String localePropertiesKey = GetKey(categoryId, locale);

                    Dictionary<String, CategoryLocaleProperties> allCategoryLocaleProperties = GetCategoryLocaleProperties(hierarchyId, new Collection<LocaleEnum> { locale }, categoryContext, callerContext);

                    categoryLocaleProperties = GetCategoryLocalePropertiesByKey(allCategoryLocaleProperties, localePropertiesKey);

                    if (Constants.PERFORMANCE_TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - To load category locale properties for category id {1} in hierarchy {2}", durationHelper.GetDurationInMilliseconds(DateTime.Now), categoryId, hierarchyId), MDMTraceSource.CategoryGet);

                    #endregion

                    #region Construct Category object

                    category = new Category(categoryBaseProperties, categoryLocaleProperties);

                    if (categoryBaseProperties != null && !String.IsNullOrWhiteSpace(categoryBaseProperties.ExtendedProperties))
                    {
                        category.ExtendedProperties = categoryBaseProperties.ExtendedProperties;
                    }
                    category.Locale = locale;

                    if (applySecurity)
                    {
                        category.PermissionSet = permission.PermissionSet;
                    }

                    #endregion
                }
            }

            if (Constants.PERFORMANCE_TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - overall time to load category for category id {1} in hierarchy {2}", durationHelper.GetDurationInMilliseconds(DateTime.Now), categoryId, hierarchyId), MDMTraceSource.CategoryGet);

            return category;
        }

        /// <summary>
        /// Get Category details By requested CategoryName
        /// </summary>
        /// <param name="hierarchyId">Hierarchy Id in which category is requested</param>
        /// <param name="categoryName">Name of the category</param>
        /// <param name="callerContext">Context indicating the caller of the API</param>
        /// <returns>Category Information</returns>
        public Category GetByName(Int32 hierarchyId, String categoryName, CallerContext callerContext)
        {
            var context = new CategoryContext();
            context.HierarchyId = hierarchyId;
            CategorySearchRuleCollection rules = new CategorySearchRuleCollection();
            rules.Add(new CategorySearchRule { CategoryField = CategoryField.Name, SearchOperator = SearchOperator.EqualTo, SearchValue = categoryName });
            context.CategorySearchRules = rules;
            context.MaxRecordsToReturn = -1;

            return SearchCategories(context, callerContext).FirstOrDefault();
        }

        /// <summary>
        /// Get Category details By requested ContainerName and CategoryName
        /// </summary>
        /// <param name="containerName">Container Name in which category is requested</param>
        /// <param name="categoryName">Name of the category</param>
        /// <param name="callerContext">Context indicating the caller of the API</param>
        /// <returns>Category Information</returns>
        public Category GetByNameUsingContainerName(String containerName, String categoryName, CallerContext callerContext)
        {
            var context = new CategoryContext();
            context.ContainerName = containerName;
            CategorySearchRuleCollection rules = new CategorySearchRuleCollection();
            rules.Add(new CategorySearchRule { CategoryField = CategoryField.Name, SearchOperator = SearchOperator.EqualTo, SearchValue = categoryName });
            context.CategorySearchRules = rules;
            context.MaxRecordsToReturn = -1;

            return SearchCategories(context, callerContext).FirstOrDefault();
        }

        /// <summary>
        /// Get Category details By requested category path
        /// </summary>
        /// <param name="hierarchyId">Hierarchy Id in which category is requested</param>
        /// <param name="path">Path of the category</param>
        /// <param name="callerContext">Context indicating the caller of the API</param>
        /// <returns>Category Information</returns>
        public Category GetByPath(Int32 hierarchyId, String path, CallerContext callerContext)
        {
            var context = new CategoryContext();
            context.HierarchyId = hierarchyId;
            CategorySearchRuleCollection rules = new CategorySearchRuleCollection();
            rules.Add(new CategorySearchRule { CategoryField = CategoryField.Path, SearchOperator = SearchOperator.EqualTo, SearchValue = path });
            context.CategorySearchRules = rules;
            context.MaxRecordsToReturn = -1;

            return SearchCategories(context, callerContext).FirstOrDefault();
        }

        /// <summary>
        /// Get Category details By requested category path
        /// </summary>
        /// <param name="containerName">Container Name in which category is requested</param>
        /// <param name="path">Path of the category</param>
        /// <param name="callerContext">Context indicating the caller of the API</param>
        /// <returns>Category Information</returns>
        public Category GetByPathUsingContainerName(String containerName, String path, CallerContext callerContext)
        {
            var context = new CategoryContext();
            context.ContainerName = containerName;
            CategorySearchRuleCollection rules = new CategorySearchRuleCollection();
            rules.Add(new CategorySearchRule { CategoryField = CategoryField.Path, SearchOperator = SearchOperator.EqualTo, SearchValue = path });
            context.CategorySearchRules = rules;
            context.MaxRecordsToReturn = -1;

            return SearchCategories(context, callerContext).FirstOrDefault();
        }

        /// <summary>
        /// Get All Categories under requested hierarchy Id
        /// </summary>
        /// <param name="hierarchyId">Hierarchy Id in which category is requested</param>
        /// <param name="callerContext">Context indicating the caller of the API</param>
        /// <param name="locale">Locale in which Category is needed</param>
        /// <returns>Collection of Category</returns>
        public CategoryCollection GetAllCategories(Int32 hierarchyId, CallerContext callerContext, LocaleEnum locale = LocaleEnum.UnKnown)
        {
            var context = new CategoryContext();
            context.HierarchyId = hierarchyId;
            context.Locales = new Collection<LocaleEnum>() { locale };
            context.MaxRecordsToReturn = -1;

            return SearchCategories(context, callerContext);
        }

        /// <summary>
        /// Get All Categories under requested container name and locale
        /// </summary>
        /// <param name="containerName">Container Name for which all categories are requested</param>
        /// <param name="callerContext">Context indicating the caller of the API</param>
        /// <param name="locale">Locale in which Category is needed</param>
        /// <returns>Collection of Category</returns>
        public CategoryCollection GetAllCategoriesUsingContainerName(String containerName, CallerContext callerContext, LocaleEnum locale = LocaleEnum.UnKnown)
        {
            var context = new CategoryContext();
            context.ContainerName = containerName;
            context.Locales = new Collection<LocaleEnum>() { locale };
            context.MaxRecordsToReturn = -1;

            return SearchCategories(context, callerContext);
        }

        /// <summary>
        /// Gets categories from all hierarchies
        /// </summary>
        /// <param name="locale">Indicates data locale</param>
        /// <param name="callerContext">Indicates caller context</param>
        /// <returns></returns>
        public CategoryCollection GetCategoriesAcrossHierarchies(LocaleEnum locale, CallerContext callerContext)
        {
            #region Validation

            if (callerContext == null)
            {
                callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity);
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111823", false, callerContext);
                throw new MDMOperationException("111823", _localeMessage.Message, "CategoryManager", String.Empty, "GetCategoriesAcrossHierarchies");//CallerContext is null or empty
            }

            if (locale == LocaleEnum.UnKnown)
            {
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111793", false, callerContext);
                throw new MDMOperationException("111793", _localeMessage.Message, "CategoryManager", String.Empty, "GetCategoriesAcrossHierarchies"); //Locale value cannot be null or unknown
            }            

            #endregion Validation
            
            CategoryCollection categoryCollection = new CategoryCollection();

            Int32 localeId = (Int32)locale;
            HierarchyBL hierarchyBL = new HierarchyBL();            
            Collection<Hierarchy> hierarchies = hierarchyBL.GetAllHierarchies(localeId);

            foreach (Hierarchy hierarchy in hierarchies)
            {
                CategoryCollection categories = this.GetAllCategories(hierarchy.Id, callerContext, locale);
                if (categories != null)
                {
                    categoryCollection.AddRange(categories);
                }
            }

            return categoryCollection;
        }

        #region IDataModelManager Methods

        /// <summary>
        /// Generate and OperationResult Schema for given DataModelObjectCollection
        /// </summary>
        /// <param name="iDataModelObjects"></param>
        /// <param name="operationResultCollection">Collection of DataModel OperationResult</param>
        public void PrepareOperationResultsSchema(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResultCollection)
        {
            CategoryCollection categories = iDataModelObjects as CategoryCollection;

            if (categories != null && categories.Count > 0)
            {
                if (operationResultCollection.Count > 0)
                {
                    operationResultCollection.Clear();
                }

                Int64 categoryToBeCreated = -1;

                foreach (Category category in categories)
                {
                    DataModelOperationResult categoryOperationResult = new DataModelOperationResult(category.Id, category.LongName, category.ExternalId, category.ReferenceId);

                    if (String.IsNullOrEmpty(categoryOperationResult.ExternalId))
                    {
                        categoryOperationResult.ExternalId = category.Name;
                    }

                    if (category.Id < 1)
                    {
                        category.Id = categoryToBeCreated;
                        categoryOperationResult.Id = categoryToBeCreated;
                        categoryToBeCreated--;
                    }

                    operationResultCollection.Add(categoryOperationResult);
                }
            }
        }

        /// <summary>
        /// Validates DataModelObjects and populate OperationResults
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be filled up.</param>
        /// <param name="operationResults">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void Validate(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            ValidateInputParameters(iDataModelObjects as CategoryCollection, operationResults, iCallerContext as CallerContext);
        }

        /// <summary>
        /// Load original data model objects
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to load original object.</param>
        /// <param name="operationResults">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void LoadOriginal(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            CategoryCollection categories = iDataModelObjects as CategoryCollection;

            if (categories != null)
            {
                LoadOriginalCategories(categories, iCallerContext as CallerContext);
            }
        }

        /// <summary>
        /// Fills missing information to data model objects.
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be filled up.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void FillDataModel(IDataModelObjectCollection iDataModelObjects, ICallerContext iCallerContext)
        {
            FillCategories(iDataModelObjects as CategoryCollection, iCallerContext as CallerContext);
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
            CompareAndMerge(iDataModelObjects as CategoryCollection, operationResults, iCallerContext as CallerContext);
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
            EntityCollection entities = new EntityCollection();
            CategoryCollection categories = iDataModelObjects as CategoryCollection;

            foreach (Category category in categories)
            {
                if (category.Action == ObjectAction.Read || category.Action == ObjectAction.Ignore)
                {
                    continue;
                }

                Entity entity = new Entity()
                {
                    ReferenceId = ValueTypeHelper.Int64TryParse(category.ReferenceId, 0),
                    Id = category.Id,
                    Name = category.Name,
                    LongName = category.LongName,
                    BranchLevel = ContainerBranchLevel.Node,
                    EntityTypeId = Constants.CATEGORY_ENTITYTYPE,
                    ContainerId = category.HierarchyId,
                    ContainerName = category.HierarchyName,
                    ContainerLongName = category.HierarchyLongName,
                    ParentEntityId = category.ParentCategoryId,
                    ParentEntityName = category.ParentCategoryName,
                    Path = category.Path,
                    Locale = category.Locale,
                    Action = category.Action
                };

                entities.Add(entity);
            }

            if (entities.Count > 0)
            {
                EntityProcessingOptions entityProcessingOptions = new EntityProcessingOptions(true,false,true,false);
                EntityOperationResultCollection entityOperationResults = this._iEntityManager.Process(entities, entityProcessingOptions, (CallerContext)iCallerContext);

                if (entityOperationResults != null)
                {
                    InvalidateCache(categories);

                    //This process inserts category related information into entity locale table.
                    ProcessCategoryLocalizedValues(categories, entityOperationResults, iCallerContext as CallerContext);

                    CopyOverResultFromEORToDMOR(entityOperationResults, operationResults, iCallerContext as CallerContext);
                }
            }
        }

        /// <summary>
        /// Processes the Entity cache statuses for data model objects
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be process entity cache load.</param>
        /// <param name="operationResults">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void InvalidateCache(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            InvalidateCache(iDataModelObjects as CategoryCollection);
        }

        #endregion

        #endregion

        #region Private Methods

        #region Validation Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryContext"></param>
        /// <param name="callerContext"></param>
        private void ValidateCategoryContext(CategoryContext categoryContext, CallerContext callerContext)
        {
            LocaleMessage _localeMessage = new LocaleMessage();
            LocaleMessageBL _localeMessageBL = new LocaleMessageBL();

            if (categoryContext == null)
            {
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "113131", false, callerContext);
                throw new MDMOperationException("113131", _localeMessage.Message, "CategoryManager", String.Empty, "GetCategories");
            }

            if (categoryContext.HierarchyId < 1)
            {
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "112198", false, callerContext);
                throw new MDMOperationException("112198", _localeMessage.Message, "CategoryManager", String.Empty, "GetCategories");
            }

            if ((categoryContext.Locales == null || categoryContext.Locales.Count < 1) ||
               (categoryContext.Locales != null && categoryContext.Locales.Count == 1 && categoryContext.Locales.Contains(LocaleEnum.UnKnown)))
            {
                Collection<LocaleEnum> locales = new Collection<LocaleEnum>() { GlobalizationHelper.GetSystemDataLocale() };
                categoryContext.Locales = locales;
            }

            if (categoryContext.MaxRecordsToReturn == 0 && categoryContext.StartIndex == 0 && categoryContext.EndIndex == 0)
            {
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "113132", false, callerContext);
                throw new MDMOperationException("113132", _localeMessage.Message, "CategoryManager", String.Empty, "GetCategories");
            }
        }

        /// <summary>
        /// Validates Input Parameters
        /// </summary>
        /// <param name="categories">Indicates collection of category</param>
        /// <param name="operationResults">Indicates collection of operation results after validating input parameters</param>
        /// <param name="callerContext">Indicates the application and module of the caller who initialized the call.</param>
        private void ValidateInputParameters(CategoryCollection categories, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            String errorMessage = String.Empty;
            CategoryCollection duplicateCategories = new CategoryCollection();

            if (callerContext == null)
            {
                AddOperationResults(operationResults, "111846", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
            if (categories == null || categories.Count < 1)
            {
                AddOperationResults(operationResults, "113594", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
            else
            {
                foreach (Category category in categories)
                {
                    IDataModelOperationResult operationResult = operationResults.GetByReferenceId(category.ReferenceId);

                    if (String.IsNullOrWhiteSpace(category.HierarchyName))
                    {
                        AddOperationResult(operationResult, "112688", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                    if (String.IsNullOrWhiteSpace(category.Name))
                    {
                        AddOperationResult(operationResult, "113685", "Category name is empty or not specified", null, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                    else
                    {
                        if (!String.IsNullOrWhiteSpace(category.HierarchyName))
                        {
                            Category duplicateCategory = duplicateCategories.Get(category.Name, category.Path, category.HierarchyName);
                            if (duplicateCategory != null)
                            {
                                AddOperationResult(operationResult, "112016", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
                            }
                            else
                            {
                                duplicateCategories.Add(category);
                            }
                        }
                    }
                    if (String.IsNullOrWhiteSpace(category.LongName))
                    {
                        AddOperationResult(operationResult, "113681", "Category long name is empty or not specified.", null, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                }
            }
        }

        #endregion

        #region Search Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryContext"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private CategoryCollection SearchCategories(CategoryContext categoryContext, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Searching for categories started....", MDMTraceSource.CategoryGet);
            }

            if (categoryContext.HierarchyId < 1 && (!String.IsNullOrEmpty(categoryContext.ContainerName) || categoryContext.ContainerId > 0))
            {
                ContainerBL containerBL = new ContainerBL();
                Container container = null;

                if (categoryContext.ContainerId > 0)
                {
                    container = containerBL.Get(categoryContext.ContainerId, callerContext);
                }
                else
                {
                    container = containerBL.Get(categoryContext.ContainerName, callerContext);
                }

                if (container != null)
                {
                    categoryContext.HierarchyId = container.HierarchyId;
                }
                else
                {
                    LocaleMessage localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "113775", false, callerContext);
                    throw new MDMOperationException("113775", localeMessage.Message, "CategoryManager", String.Empty, "SearchCategories");
                }
            }

            #region STEP : CategoryContext Validation

            ValidateCategoryContext(categoryContext, callerContext);

            #endregion

            #region STEP : Initial Setup

            CategoryCollection categories = new CategoryCollection();
            CategoryCollection filteredCategories = new CategoryCollection();
            Dictionary<Int64, CategoryBaseProperties> categoryBaseProperties = new Dictionary<Int64, CategoryBaseProperties>();
            Dictionary<String, CategoryLocaleProperties> categoryLocaleProperties = new Dictionary<String, CategoryLocaleProperties>();

            DurationHelper durationHelper = new DurationHelper(DateTime.Now);
            DurationHelper completeCategoryDurationHelper = new DurationHelper(DateTime.Now);

            #endregion

            try
            {
                #region STEP : Get Category Base Properties

                categoryBaseProperties = GetBaseCategories(categoryContext.HierarchyId, categoryContext, callerContext);

                if (categoryBaseProperties == null)
                {
                    //TODO : throw error if locale properties not found.
                    return null;
                }

                if (Constants.PERFORMANCE_TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - To load CategoryBaseProperties for hierarchy {1}", durationHelper.GetDurationInMilliseconds(DateTime.Now), categoryContext.HierarchyId), MDMTraceSource.CategoryGet);

                #endregion

                if (categoryBaseProperties != null && categoryBaseProperties.Count > 0)
                {
                    #region STEP : Get Category Locale Properties

                    categoryLocaleProperties = GetCategoryLocaleProperties(categoryContext.HierarchyId, categoryContext.Locales, categoryContext, callerContext);

                    if (categoryLocaleProperties == null)
                    {
                        //TODO : throw error if locale properties not found.
                        return null;
                    }

                    if (Constants.PERFORMANCE_TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - To load CategoryLocaleProperties for hierarchy {1}", durationHelper.GetDurationInMilliseconds(DateTime.Now), categoryContext.HierarchyId), MDMTraceSource.CategoryGet);

                    #endregion

                    #region STEP : Populate Category Collection from Base and Locale Properties applying security

                    Boolean isCategoryHavingPermissions = false;
                    Category category = null;
                    CategoryBaseProperties baseCategory = null;
                    CategoryLocaleProperties catLocaleProperties = null;

                    #region Initialization of Data Security

                    Permission permission = null;
                    SecurityPrincipal securityPrincipal = null;
                    DataSecurityBL dataSecurityBL = null;
                    Boolean applySecurity = categoryContext.ApplySecurity;

                    if (applySecurity)
                    {
                        securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
                        dataSecurityBL = new DataSecurityBL(securityPrincipal);
                    }

                    #endregion

                    foreach (KeyValuePair<Int64, CategoryBaseProperties> catBaseProperties in categoryBaseProperties)
                    {
                        isCategoryHavingPermissions = false;

                        baseCategory = GetBaseCategoryByKey(categoryBaseProperties, catBaseProperties.Key);

                        if (baseCategory != null)
                        {
                            if (applySecurity)
                            {
                                //Get the permissions for each category and add to the collection only if category is having permissions..
                                //Prepare permission context.. Keep RoleId as '0' and pass the current user Id.. The Load logic determines the permissions by considering all roles of the current user.
                                PermissionContext permissionContext = new PermissionContext(0, 0, catBaseProperties.Key, 6, 0, catBaseProperties.Key, 0, 0, securityPrincipal.CurrentUserId, 0);
                                permission = dataSecurityBL.GetMDMObjectPermission(catBaseProperties.Key, (Int32)ObjectType.Category, "Category", permissionContext);

                                if (permission != null && permission.PermissionSet != null)
                                {
                                    isCategoryHavingPermissions = true;
                                }
                            }
                            else
                            {
                                permission = new Permission();
                                permission.PermissionSet.Add(UserAction.View);
                                isCategoryHavingPermissions = true;
                            }

                            if (isCategoryHavingPermissions || !applySecurity)
                            {
                                foreach (LocaleEnum locale in categoryContext.Locales)
                                {
                                    catLocaleProperties = GetCategoryLocalePropertiesByKey(categoryLocaleProperties, GetKey(catBaseProperties.Key, locale));

                                    if (catLocaleProperties != null)
                                    {
                                        category = new Category(baseCategory, catLocaleProperties);
                                        category.Locale = locale;

                                        if (applySecurity)
                                        {
                                            category.PermissionSet = permission.PermissionSet;
                                        }

                                        categories.Add(category);
                                    }
                                }
                            }
                        }
                    }

                    if (Constants.PERFORMANCE_TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - To Populate Categories for hierarchy {1}", durationHelper.GetDurationInMilliseconds(DateTime.Now), categoryContext.HierarchyId), MDMTraceSource.CategoryGet);

                    #endregion

                    #region STEP : Apply Sorting

                    //Check for OrderByField - Default is id... if not order it.
                    if (categoryContext.OrderByField != CategoryField.Id)
                    {
                        categories = SortCategoriesByOrderByField(categories, categoryContext.OrderByField);
                    }

                    if (Constants.PERFORMANCE_TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - To Filter Categories for hierarchy {1}", durationHelper.GetDurationInMilliseconds(DateTime.Now), categoryContext.HierarchyId), MDMTraceSource.CategoryGet);

                    #endregion

                    #region STEP : Perform Search

                    if (categoryContext.CategorySearchRules != null && categoryContext.CategorySearchRules.Count > 0)
                    {
                        filteredCategories = FilterCategoriesBySearchRules(categoryContext.CategorySearchRules, categories);
                    }
                    else
                    {
                        //filteredCategories.AddRange(categories);
                        filteredCategories = categories;
                    }

                    if (Constants.PERFORMANCE_TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - To Perform Search for hierarchy {1}", durationHelper.GetDurationInMilliseconds(DateTime.Now), categoryContext.HierarchyId), MDMTraceSource.CategoryGet);

                    #endregion

                    #region STEP : Load Recursive

                    //Load All the children recursively
                    if (categoryContext.LoadRecursive)
                    {
                        LoadChildCategoriesRecursive(filteredCategories, categories);
                    }

                    if (categoryContext.LoadParentRecursive)
                    {
                        filteredCategories = LoadParentCategoriesRecursive(filteredCategories, categories);
                    }

                    #endregion

                    #region STEP : Prepare Result to return

                    //NOTE : 
                    //MaxRecordsToReturn : -1 means all Records
                    //MaxRecordsToReturn : 0 means No Records, If EndIndex is available then return till endIndex 

                    if (filteredCategories != null && categoryContext.MaxRecordsToReturn != -1)
                    {
                        CategoryCollection categoriesToReturn = new CategoryCollection();
                        Int32 count = 0;

                        if (categoryContext.MaxRecordsToReturn > 0)
                        {
                            foreach (Category categoryToReturn in filteredCategories)
                            {
                                //If LoadOnlyLeafCategories is True and category is not leaf then dont add in final result
                                if (!categoryContext.LoadOnlyLeafCategories || categoryToReturn.IsLeaf)
                                {
                                categoriesToReturn.Add(categoryToReturn);
                                count++;

                                if (count > categoryContext.MaxRecordsToReturn)
                                    {
                                    break;
                            }
                        }
                            }
                        }
                        else if (categoryContext.MaxRecordsToReturn == 0)
                        {
                            if (categoryContext.EndIndex > 0)
                            {
                                foreach (Category categoryToReturn in categories)
                                {
                                    if (count >= categoryContext.StartIndex)
                                    {
                                        if (count <= categoryContext.EndIndex)
                                        {
                                            //If LoadOnlyLeafCategories is True and category is not leaf then dont add in final result
                                            if (!categoryContext.LoadOnlyLeafCategories || categoryToReturn.IsLeaf)
                                            {
                                            categoriesToReturn.Add(categoryToReturn);
                                        }
                                    }
                                    }
                                    count++;
                                }
                            }
                        }
                        //NOTE : If maxRecordToReturn == -1, LoadOnlyLeafCategories = true
                        else if (categoryContext.LoadOnlyLeafCategories)
                        {
                            foreach (Category categoryToReturn in filteredCategories)
                            {
                                //If LoadOnlyLeafCategories is True and category is not leaf then dont add in final result
                                if (!categoryContext.LoadOnlyLeafCategories || categoryToReturn.IsLeaf)
                                {
                                    categoriesToReturn.Add(categoryToReturn);
                                }
                            }
                        }

                        if (categoriesToReturn != null && categoriesToReturn.Count > 0)
                        {
                            filteredCategories = categoriesToReturn;
                        }
                    }

                    if (Constants.PERFORMANCE_TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Found {1} categories to return", durationHelper.GetDurationInMilliseconds(DateTime.Now), filteredCategories.Count), MDMTraceSource.CategoryGet);

                    #endregion
                }
            }
            finally
            {
                if (Constants.PERFORMANCE_TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - taken for overall load.Total categories loaded are : {1}", completeCategoryDurationHelper.GetDurationInMilliseconds(DateTime.Now), filteredCategories.Count), MDMTraceSource.CategoryGet);
                }
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Searching for categories started completed", MDMTraceSource.CategoryGet);
                }
            }

            return filteredCategories;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hierarchyId"></param>
        /// <param name="categoryContext"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private Dictionary<Int64, CategoryBaseProperties> GetBaseCategories(Int32 hierarchyId, CategoryContext categoryContext, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Base Category load started...", MDMTraceSource.CategoryGet);

            #region Get base categories from Cache if available

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Trying to get base Categories from the cache.", MDMTraceSource.CategoryGet);

            CategoryBufferManager categoryBufferManager = new CategoryBufferManager();
            Dictionary<Int64, CategoryBaseProperties> baseCategories = categoryBufferManager.FindBaseCategories(hierarchyId);

            #endregion

            #region Get base categories from Database

            if (baseCategories == null || baseCategories.Count < 1)
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Trying to load base categories from DB as not found in cache for the cache.", MDMTraceSource.CategoryGet);

                baseCategories = LoadBaseCategoriesFromDB(hierarchyId, categoryContext, callerContext);
            }
            else
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0} base Category (s) found in the cache.", baseCategories.Count), MDMTraceSource.CategoryGet);
            }

            #endregion

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Base category load completed.", MDMTraceSource.CategoryGet);

            return baseCategories;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hierarchyId"></param>
        /// <param name="locales"></param>
        /// <param name="categoryContext"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private Dictionary<String, CategoryLocaleProperties> GetCategoryLocaleProperties(Int32 hierarchyId, Collection<LocaleEnum> locales, CategoryContext categoryContext, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Get categories for locale started...", MDMTraceSource.CategoryGet);

            Dictionary<String, CategoryLocaleProperties> categoryLocalePropertiesCollection = null;

            if (locales != null && locales.Count > 0)
            {
                categoryLocalePropertiesCollection = new Dictionary<String, CategoryLocaleProperties>();

                foreach (LocaleEnum locale in locales)
                {
                    #region Get categories locale properties from Cache if available

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Trying to get categories locale properties from the cache.", MDMTraceSource.CategoryGet);

                    CategoryBufferManager categoryBufferManager = new CategoryBufferManager();
                    Dictionary<String, CategoryLocaleProperties> categoryLocaleProperties = categoryBufferManager.FindCategoryLocaleProperties(locale, hierarchyId);

                    #endregion

                    #region Get locale based Categories from Database

                    //Not found in cache, load from the DB
                    if (categoryLocaleProperties == null)
                    {
                        if (Constants.TRACING_ENABLED)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Trying to get categories locale properties from DB as not found in the cache.", MDMTraceSource.CategoryGet);

                        DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);
                        categoryLocaleProperties = _categoryDataProvider.GetCategoryLocaleProperties(locale, hierarchyId, command, categoryContext, callerContext);

                        #region Cache base categories

                        if (categoryLocaleProperties != null)
                        {
                            categoryBufferManager.UpdateCategoryLocaleProperties(categoryLocaleProperties, locale, hierarchyId, 3);
                        }

                        #endregion
                    }
                    else
                    {
                        if (Constants.TRACING_ENABLED)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0} base categories locale properties  found in cache for the cache.", categoryLocaleProperties.Count), MDMTraceSource.CategoryGet);
                    }

                    #endregion

                    #region Merge Category Locale Properties for all locale

                    if ((categoryLocaleProperties != null && categoryLocaleProperties.Count > 0) && locales.Count > 1)
                    {
                        var enumerator = categoryLocaleProperties.GetEnumerator();

                        while (enumerator.MoveNext())
                        {
                            String key = enumerator.Current.Key;

                            if (!categoryLocalePropertiesCollection.ContainsKey(key))
                            {
                                categoryLocalePropertiesCollection.Add(key, enumerator.Current.Value);
                            }
                        }
                    }
                    else
                    {
                        categoryLocalePropertiesCollection = categoryLocaleProperties;
                    }

                    #endregion
                }
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Get categories for locale completed.", MDMTraceSource.CategoryGet);

            return categoryLocalePropertiesCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hierarchyId"></param>
        /// <param name="categoryContext"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private Dictionary<Int64, CategoryBaseProperties> LoadBaseCategoriesFromDB(Int32 hierarchyId, CategoryContext categoryContext, CallerContext callerContext)
        {
            DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Read);

            HierarchyBL hierarchyManager = new HierarchyBL();
            Hierarchy hierarchy = hierarchyManager.GetById(hierarchyId, callerContext, false);

            Dictionary<Int64, CategoryBaseProperties> baseCategories = _categoryDataProvider.GetAllBaseCategories(hierarchy, command, categoryContext, callerContext);

            #region Cache base categories

            if (baseCategories != null && baseCategories.Count > 0)
            {
                CategoryBufferManager categoryBufferManager = new CategoryBufferManager();
                categoryBufferManager.UpdateBaseCategories(baseCategories, hierarchyId, 3);
            }

            #endregion

            return baseCategories;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseCategories"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        private CategoryBaseProperties GetBaseCategoryByKey(Dictionary<Int64, CategoryBaseProperties> baseCategories, Int64 categoryId)
        {
            CategoryBaseProperties baseCategory = null;

            if (baseCategories != null && baseCategories.ContainsKey(categoryId))
            {
                baseCategory = baseCategories[categoryId];
            }

            return baseCategory;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryLocaleProperties"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private CategoryLocaleProperties GetCategoryLocalePropertiesByKey(Dictionary<String, CategoryLocaleProperties> categoryLocaleProperties, String key)
        {
            CategoryLocaleProperties catLocaleProperties = null;

            if (categoryLocaleProperties != null && categoryLocaleProperties.ContainsKey(key))
            {
                catLocaleProperties = categoryLocaleProperties[key];
            }

            return catLocaleProperties;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        private String GetKey(Int64 categoryId, LocaleEnum locale)
        {
            return String.Format("{0}_{1}", categoryId, (Int32)locale);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categories"></param>
        /// <param name="callerContext"></param>
        private void LoadOriginalCategories(CategoryCollection categories, CallerContext callerContext)
        {
            Dictionary<Int32, CategoryCollection> hierachyIdBaseCategories = new Dictionary<Int32, CategoryCollection>();

            //Since this is internal hierarchy get , pass apply security as false.
            HierarchyCollection hierarchies = new HierarchyBL().GetAll(callerContext, false);

            if (hierarchies != null && hierarchies.Count > 0)
            {
                foreach (Category category in categories)
                {
                    //Find a hierarchy from collection with give hierarchy name.
                    Hierarchy hierarchy = hierarchies.Get(category.HierarchyName);

                    if (hierarchy != null)
                    {
                        CategoryCollection originalCategories = null;
                        Int32 hierarchyId = hierarchy.Id;

                        //Try to get original categories based on hierarchy id from dictionary.
                        hierachyIdBaseCategories.TryGetValue(hierarchyId, out originalCategories);

                        //If original categories are not available then load all categories from cache by passing hierarchy id.
                        if (originalCategories == null)
                        {
                            originalCategories = GetAllBaseCategories(hierarchyId, callerContext);
                            if (originalCategories != null && originalCategories.Count > 0)
                            {
                                hierachyIdBaseCategories.Add(hierarchyId, originalCategories);
                            }
                        }

                        category.OriginalCategory = originalCategories.Get(category.Name, category.Path, category.HierarchyName);
                        category.HierarchyId = hierarchyId;
                        category.HierarchyLongName = hierarchy.LongName;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hierarchyId"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public CategoryCollection GetAllBaseCategories(Int32 hierarchyId, CallerContext callerContext)
        {
            CategoryCollection categories = new CategoryCollection();
            Dictionary<Int64, CategoryBaseProperties> baseCategories = GetBaseCategories(hierarchyId, null, callerContext);

            if (baseCategories != null && baseCategories.Count > 0)
            {
                var enumerator = baseCategories.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    Category category = new Category(enumerator.Current.Value, null);
                    categories.Add(category);
                }
            }

            return categories;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categories"></param>
        /// <param name="callerContext"></param>
        private void FillCategories(CategoryCollection categories, CallerContext callerContext)
        {
            HierarchyCollection hierarchies = null;
            Dictionary<Int32, CategoryCollection> baseCatgoriesByHierarchyId = new Dictionary<Int32, CategoryCollection>();

            foreach (Category category in categories)
            {
                CategoryCollection baseCategories = null;

                #region Fill info from original category

                if (category.OriginalCategory != null)
                {
                    category.Id = category.OriginalCategory.Id;
                    category.IsLeaf = category.OriginalCategory.IsLeaf;
                    category.Level = category.OriginalCategory.Level;

                    if (String.IsNullOrWhiteSpace(category.LongNamePath))
                    {
                        category.LongNamePath = category.OriginalCategory.LongNamePath;
                    }

                    if (String.IsNullOrWhiteSpace(category.Path))
                    {
                        category.Path = category.OriginalCategory.Path;
                    }

                    category.HierarchyId = category.OriginalCategory.HierarchyId;
                    category.HierarchyLongName = category.OriginalCategory.HierarchyLongName;

                    category.ParentCategoryId = category.OriginalCategory.ParentCategoryId;
                }

                #endregion Fill info from original category

                #region Fill Hierarchy Info

                if (category.HierarchyId < 1)
                {
                    if (hierarchies == null)
                    {
                        //Since this is internal hierarchy get , pass apply security as false.
                        hierarchies = new HierarchyBL().GetAll(callerContext, false);
                    }

                    if (!hierarchies.IsNullOrEmpty())
                    {
                        Hierarchy hierarchy = hierarchies.Get(category.HierarchyName);

                        if (hierarchy != null)
                        {
                            category.HierarchyId = hierarchy.Id;
                            category.HierarchyLongName = hierarchy.LongName;
                        }
                    }
                }

                #endregion Fill Hierarchy Info

                #region Fill Parent Category Info

                if (category.ParentCategoryId < 1 && category.HierarchyId > 0 && !String.IsNullOrWhiteSpace(category.ParentCategoryName))
                {
                    String categoryPath = category.Path;

                    List<String> categoryNames = categoryPath.Split(new String[] { _categoryPathSeparator }, StringSplitOptions.None).ToList<String>();
                    Int32 index = categoryNames.Count - 1;

                    if (categoryNames[index] == category.Name)
                    {
                        categoryNames.RemoveAt(index);
                    }

                    categoryPath = String.Join(_categoryPathSeparator, categoryNames);

                    baseCatgoriesByHierarchyId.TryGetValue(category.HierarchyId, out baseCategories);

                    if (baseCategories == null)
                    {
                        baseCategories = GetAllBaseCategories(category.HierarchyId, callerContext);

                        if (!baseCategories.IsNullOrEmpty())
                        {
                            baseCatgoriesByHierarchyId.Add(category.HierarchyId, baseCategories);
                        }
                    }

                    Category parentCategory = baseCategories.Get(category.HierarchyId, categoryPath);

                    if (parentCategory != null)
                    {
                        category.ParentCategoryId = parentCategory.Id;
                        category.ParentCategoryName = parentCategory.Name;
                    }
                }

                #endregion Fill Parent Category Info
            }
        }

        #endregion

        #region Process Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categories"></param>
        /// <param name="operationResults"></param>
        /// <param name="callerContext"></param>
        private void CompareAndMerge(CategoryCollection categories, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            foreach (Category deltaCategory in categories)
            {
                IDataModelOperationResult operationResult = operationResults.GetByReferenceId(deltaCategory.ReferenceId);

                //If delta object Action is Read/Ignore then skip Merge Delta Process.
                if (deltaCategory.Action == ObjectAction.Read || deltaCategory.Action == ObjectAction.Ignore)
                    continue;

                Category origCategory = deltaCategory.OriginalCategory;

                if (origCategory != null)
                {
                    //If delta object Action is Delete then skip Merge Delta process.
                    if (deltaCategory.Action != ObjectAction.Delete)
                    {
                        origCategory.MergeDelta(deltaCategory, callerContext, false);
                    }
                }
                else
                {
                    String errorMessage = String.Empty;

                    if (deltaCategory.Action == ObjectAction.Delete)
                    {
                        AddOperationResult(operationResult, "113605", String.Empty, new Object[] { deltaCategory.Name, deltaCategory.HierarchyName }, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                    else
                    {
                        if (deltaCategory.HierarchyId < 1)
                        {
                            AddOperationResult(operationResult, "113675", "Hierarchy: {0} is invalid.", new Object[] { deltaCategory.HierarchyName }, OperationResultType.Error, TraceEventType.Error, callerContext);
                        }
                        if (!String.IsNullOrWhiteSpace(deltaCategory.ParentCategoryName) && deltaCategory.ParentCategoryId < 1)
                        {
                            AddOperationResult(operationResult, "113682", "Catergory parent: {0} is invalid.", new Object[] { deltaCategory.ParentCategoryName }, OperationResultType.Error, TraceEventType.Error, callerContext);
                        }

                        //If original object is not found then set Action as Create always.
                        deltaCategory.Action = ObjectAction.Create;
                    }
                }
                operationResult.PerformedAction = deltaCategory.Action;
            }
        }

        /// <summary>
        /// This method inserts data into entity locale table.This method works only for SDL category.
        /// Please use EntityLocaleBL.Process method to insert data for other than SDL.
        /// </summary>
        /// <param name="categories"></param>
        /// <param name="entityOperationResults"></param>
        /// <param name="callerContext"></param>
        private void ProcessCategoryLocalizedValues(CategoryCollection categories, EntityOperationResultCollection entityOperationResults, CallerContext callerContext)
        {
            if (categories != null && categories.Count > 0 && entityOperationResults != null)
            {
                CategoryLocalePropertiesCollection categoryLocalePropertiesCollection = new CategoryLocalePropertiesCollection();

                foreach (Category category in categories)
                {
                    //if entity locale is other than SDL then skip processing.
                    if (category.Locale != _systemDataLocale)
                        continue;

                    Int64 referanceId = ValueTypeHelper.Int64TryParse(category.ReferenceId, 0);

                    EntityOperationResult entityOperationResult = entityOperationResults.GetByReferenceId(referanceId) as EntityOperationResult;

                    if (entityOperationResult != null &&
                        (entityOperationResult.OperationResultStatus == OperationResultStatusEnum.None ||
                           entityOperationResult.OperationResultStatus == OperationResultStatusEnum.Successful))
                    {
                        CategoryLocaleProperties categoryLocaleProperties = new CategoryLocaleProperties()
                        {
                            CategoryId = entityOperationResult.EntityId,
                            ReferenceId = category.ReferenceId.ToString(),
                            Name = category.Name,
                            LongName = category.LongName,
                            Locale = category.Locale,
                            HierarchyId = category.HierarchyId,
                            HierarchyName = category.HierarchyName,
                            Path = category.Path,
                            Action = category.Action
                        };

                        categoryLocalePropertiesCollection.Add(categoryLocaleProperties);
                    }
                }

                if (categoryLocalePropertiesCollection.Count > 0)
                {
                    IDataModelOperationResultCollection operationResults = DataModelProcessOrchestrator.Validate(this._iEntityLocaleDataManager, categoryLocalePropertiesCollection, callerContext);
                    DataModelProcessOrchestrator.Process(this._iEntityLocaleDataManager, categoryLocalePropertiesCollection, operationResults, callerContext);
                }
            }
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categories"></param>
        /// <param name="orderByField"></param>
        /// <returns></returns>
        private CategoryCollection SortCategoriesByOrderByField(CategoryCollection categories, CategoryField orderByField)
        {
            CategoryCollection filteredCategories = new CategoryCollection();

            if (categories != null && categories.Count > 0)
            {
                List<Category> orderedCategories = null;

                switch (orderByField)
                {
                    case CategoryField.LongName:
                        orderedCategories = categories.OrderBy(c => c.LongName).ToList();
                        break;
                    case CategoryField.LongNamePath:
                        orderedCategories = categories.OrderBy(c => c.LongNamePath).ToList();
                        break;
                    case CategoryField.Name:
                        orderedCategories = categories.OrderBy(c => c.Name).ToList();
                        break;
                    case CategoryField.ParentCategoryId:
                        orderedCategories = categories.OrderBy(c => c.ParentCategoryId).ToList();
                        break;
                    case CategoryField.Path:
                        orderedCategories = categories.OrderBy(c => c.Path).ToList();
                        break;
                }

                if (orderedCategories != null)
                {
                    filteredCategories = new CategoryCollection(orderedCategories);
                }
            }

            return filteredCategories;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rules"></param>
        /// <param name="categories"></param>
        /// <returns></returns>
        private CategoryCollection FilterCategoriesBySearchRules(CategorySearchRuleCollection rules, CategoryCollection categories)
        {
            CategoryCollection filteredCategories = new CategoryCollection();
            Boolean isMatched = false;

            foreach (Category category in categories)
            {
                foreach (CategorySearchRule rule in rules)
                {
                    switch (rule.CategoryField)
                    {
                        case CategoryField.Id:
                            isMatched = CompareValue(category.Id.ToString(), rule.SearchOperator, rule.SearchValue, rule.ValueSeparator);
                            break;
                        case CategoryField.LongName:
                            isMatched = CompareValue(category.LongName, rule.SearchOperator, rule.SearchValue, rule.ValueSeparator);
                            break;
                        case CategoryField.LongNamePath:
                            isMatched = CompareValue(category.LongNamePath, rule.SearchOperator, rule.SearchValue, rule.ValueSeparator);
                            break;
                        case CategoryField.Name:
                            isMatched = CompareValue(category.Name, rule.SearchOperator, rule.SearchValue, rule.ValueSeparator);
                            break;
                        case CategoryField.Path:
                            isMatched = CompareValue(category.Path, rule.SearchOperator, rule.SearchValue, rule.ValueSeparator);
                            break;
                        case CategoryField.ParentCategoryId:
                            isMatched = CompareValue(category.ParentCategoryId.ToString(), rule.SearchOperator, rule.SearchValue, rule.ValueSeparator);
                            break;
                        default:
                            throw new NotSupportedException(String.Format("Category: Search: Provided CategoryField '{0}' is not supported", rule.CategoryField.ToString()));
                    }

                    if (isMatched)
                    {
                        filteredCategories.Add(category, false);
                    }
                }
            }

            return filteredCategories;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="searchOperator"></param>
        /// <param name="filterValue"></param>
        /// <param name="valueSeparator"></param>
        /// <returns></returns>
        private Boolean CompareValue(String value, SearchOperator searchOperator, String filterValue, String valueSeparator)
        {
            Boolean isMatched = false;
            StringComparison comparisionType = StringComparison.InvariantCultureIgnoreCase;

            switch (searchOperator)
            {
                case SearchOperator.EqualTo:
                    {
                        if (value.Equals(filterValue, comparisionType))
                        {
                            isMatched = true;
                        }
                        break;
                    }
                case SearchOperator.Contains:
                    {
                        if (value.ToLowerInvariant().Contains(filterValue.ToLowerInvariant()))
                        {
                            isMatched = true;
                        }
                        break;
                    }
                case SearchOperator.NotContains:
                    {
                        if (!(value.ToLowerInvariant().Contains(filterValue.ToLowerInvariant())))
                        {
                            isMatched = true;
                        }
                        
                        break;
                    }
                case SearchOperator.SubsetOf:
                    {
                        String[] spiltedValues = null;
                        if (valueSeparator.Length > 0)
                        {
                            spiltedValues = value.Split(new String[] { valueSeparator }, StringSplitOptions.RemoveEmptyEntries);

                            int i = 0;

                            foreach (String s in spiltedValues)
                            {
                                spiltedValues[i] = valueSeparator + spiltedValues[i++];
                            }
                        }
                        else
                        {
                            spiltedValues = new String[] { value };
                        }

                        if (spiltedValues != null && spiltedValues.Length > 0)
                        {
                            String concatenatedFilterValue = String.Format("{0}{1}{0}", valueSeparator, filterValue);

                            foreach (String val in spiltedValues)
                            {
                                if (concatenatedFilterValue.StartsWith(val, comparisionType))
                                {
                                    isMatched = true;
                                }
                            }
                        }
                        break;
                    }
                default:
                    throw new NotSupportedException(String.Format("Category: Search: Provided search operator '{0}' is not supported", searchOperator.ToString()));
            }

            return isMatched;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filteredCategories"></param>
        /// <param name="allCategories"></param>
        private void LoadChildCategoriesRecursive(CategoryCollection filteredCategories, CategoryCollection allCategories)
        {
            CategoryCollection childCategories = new CategoryCollection();

            foreach (Category category in filteredCategories)
            {
                if (!category.IsLeaf)
                {
                    childCategories.AddRange(allCategories.GetChildCategoriesRecursive(category.Id));
                }
            }

            if (childCategories != null && childCategories.Count > 0)
            {
                filteredCategories.AddRange(childCategories);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filteredCategories"></param>
        /// <param name="allCategories"></param>
        /// <returns></returns>
        private CategoryCollection LoadParentCategoriesRecursive(CategoryCollection filteredCategories, CategoryCollection allCategories)
        {
            CategoryCollection parentCategories = new CategoryCollection();
            
            foreach (Category category in filteredCategories)
            {
                parentCategories.AddRange(allCategories.GetParentCategoriesRecursive(category, _categoryPathSeparator));
            }

            parentCategories.AddRange(filteredCategories);

            return parentCategories;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityOperationResults"></param>
        /// <param name="operationResults"></param>
        /// <param name="callerContext"></param>
        private void CopyOverResultFromEORToDMOR(EntityOperationResultCollection entityOperationResults, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            foreach (EntityOperationResult entityOperationResult in entityOperationResults)
            {
                DataModelOperationResult operationResult = (DataModelOperationResult)operationResults.GetByReferenceId(entityOperationResult.ReferenceId.ToString());

                if (operationResult != null)
                {
                    operationResult.Errors = LocalizeErrors(entityOperationResult.Errors, callerContext);
                    operationResult.Warnings = LocalizeWarnings(entityOperationResult.Warnings, callerContext);
                    operationResult.Informations = LocalizeInformations(entityOperationResult.Informations, callerContext);
                    operationResult.OperationResultStatus = entityOperationResult.OperationResultStatus;
                    operationResult.PerformedAction = entityOperationResult.PerformedAction;
                }
            }

            operationResults.OperationResultStatus = entityOperationResults.OperationResultStatus;
        }

        private ErrorCollection LocalizeErrors(ErrorCollection errors, CallerContext callerContext)
        {
            ErrorCollection result = new ErrorCollection();

            if (errors != null && errors.Count > 0)
            {
                HashSet<String> codesToBeLocalized = new HashSet<String>();

                foreach (Error error in errors)
                {
                    if (!codesToBeLocalized.Contains(error.ErrorCode))
                    {
                        codesToBeLocalized.Add(error.ErrorCode);
                    }
                }

                LocaleMessageCollection localeMessageCollection = _localeMessageBL.Get(_systemUILocale, codesToBeLocalized.ToCollection(), false, callerContext);

                if (localeMessageCollection != null && localeMessageCollection.Count > 0)
                {
                    foreach (Error error in errors)
                    {
                        LocaleMessage localeMessage = localeMessageCollection.FirstOrDefault(lm => String.Compare(lm.Code, error.ErrorCode, StringComparison.InvariantCultureIgnoreCase) == 0);
                        if (localeMessage != null)
                        {
                            if (error.Params != null && error.Params.Count > 0)
                            {
                                error.ErrorMessage = String.Format(localeMessage.Message, error.Params.ToArray());
                            }
                            else
                            {
                                error.ErrorMessage = localeMessage.Message;
                            }
                        }
                        result.Add(error);
                    }
                }
                else
                {
                    // if errors were not localized return as it is
                    return errors;
                }
            }

            return result;
        }

        private WarningCollection LocalizeWarnings(WarningCollection warnings, CallerContext callerContext)
        {
            WarningCollection result = new WarningCollection();

            if (warnings != null && warnings.Count > 0)
            {
                HashSet<String> codesToBeLocalized = new HashSet<String>();

                foreach (Warning warning in warnings)
                {
                    if (!codesToBeLocalized.Contains(warning.WarningCode))
                    {
                        codesToBeLocalized.Add(warning.WarningCode);
                    }
                }

                LocaleMessageCollection localeMessageCollection = _localeMessageBL.Get(_systemUILocale, codesToBeLocalized.ToCollection(), false, callerContext);

                if (localeMessageCollection != null && localeMessageCollection.Count > 0)
                {
                    foreach (Warning warning in warnings)
                    {
                        LocaleMessage localeMessage = localeMessageCollection.FirstOrDefault(lm => String.Compare(lm.Code, warning.WarningCode, StringComparison.InvariantCultureIgnoreCase) == 0);
                        if (localeMessage != null)
                        {
                            if (warning.Params != null && warning.Params.Count > 0)
                            {
                                warning.WarningMessage = String.Format(localeMessage.Message, warning.Params.ToArray());
                            }
                            else
                            {
                                warning.WarningMessage = localeMessage.Message;
                            }
                        }
                        result.Add(warning);
                    }
                }
                else
                {
                    // if warnings were not localized return as it is
                    return warnings;
                }
            }

            return result;
        }

        private InformationCollection LocalizeInformations(InformationCollection informations, CallerContext callerContext)
        {
            InformationCollection result = new InformationCollection();

            if (informations != null && informations.Count > 0)
            {
                HashSet<String> codesToBeLocalized = new HashSet<String>();

                foreach (Information information in informations)
                {
                    if (!codesToBeLocalized.Contains(information.InformationCode))
                    {
                        codesToBeLocalized.Add(information.InformationCode);
                    }
                }

                LocaleMessageCollection localeMessageCollection = _localeMessageBL.Get(_systemUILocale, codesToBeLocalized.ToCollection(), false, callerContext);

                if (localeMessageCollection != null && localeMessageCollection.Count > 0)
                {
                    foreach (Information information in informations)
                    {
                        LocaleMessage localeMessage = localeMessageCollection.FirstOrDefault( lm => String.Compare(lm.Code, information.InformationCode, StringComparison.InvariantCultureIgnoreCase) == 0);
                        if (localeMessage != null)
                        {
                            if (information.Params != null && information.Params.Count > 0)
                            {
                                information.InformationMessage = String.Format(localeMessage.Message, information.Params.ToArray());
                            }
                            else
                            {
                                information.InformationMessage = localeMessage.Message;
                            }
                        }
                        result.Add(information);
                    }
                }
                else
                {
                    // if informations were not localized return as it is
                    return informations;
                }
            }
            
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categories"></param>
        /// <returns></returns>
        private Boolean InvalidateCache(CategoryCollection categories)
        {
            Boolean result = false;

            if (categories != null && categories.Count > 0)
            {
                Dictionary<Int32, Collection<LocaleEnum>> hierachyIdBasedDictionary = new Dictionary<Int32, Collection<LocaleEnum>>();
                CategoryBufferManager categoryBufferManager = new CategoryBufferManager();

                result = true;

                foreach (Category category in categories)
                {
                    Collection<LocaleEnum> dataLocales = null;
                    Int32 hierarchyId = category.HierarchyId;

                    hierachyIdBasedDictionary.TryGetValue(hierarchyId, out dataLocales);

                    if (dataLocales == null)
                    {
                        hierachyIdBasedDictionary.Add(hierarchyId, new Collection<LocaleEnum>() { category.Locale });
                    }
                    else
                    {
                        dataLocales = hierachyIdBasedDictionary[hierarchyId];
                        dataLocales.Add(category.Locale);
                    }
                }

                if (hierachyIdBasedDictionary.Count > 0)
                {
                    foreach (Int32 hierarchyId in hierachyIdBasedDictionary.Keys)
                    {
                        result = result && categoryBufferManager.RemoveBaseCategories(hierarchyId);
                        result = result && categoryBufferManager.RemoveCategoryLocaleProperties(hierachyIdBasedDictionary[hierarchyId], hierarchyId);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeMembers()
        {
            _systemDataLocale = GlobalizationHelper.GetSystemDataLocale();
            _systemUILocale = GlobalizationHelper.GetSystemUILocale();
            _categoryPathSeparator = AppConfigurationHelper.GetAppConfig<String>("Catalog.Category.PathSeparator", " >> ");
        }

        /// <summary>
        /// 
        /// </summary>
        private void AddOperationResults(DataModelOperationResultCollection operationResults, String messageCode, String message, Object[] parameters, OperationResultType operationResultType, TraceEventType traceEventType, CallerContext callerContext)
        {
            if (parameters != null && parameters.Count() > 0)
            {
                _localeMessage = _localeMessageBL.TryGet(_systemUILocale, messageCode, message, parameters, false, callerContext);
            }
            else
            {
                _localeMessage = _localeMessageBL.TryGet(_systemUILocale, messageCode, message, false, callerContext);
            }

            if (_localeMessage != null)
            {
                operationResults.AddOperationResult(_localeMessage.Code, _localeMessage.Message, operationResultType);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void AddOperationResult(IDataModelOperationResult operationResult, String messageCode, String message, Object[] parameters, OperationResultType operationResultType, TraceEventType traceEventType, CallerContext callerContext)
        {
            if (parameters != null && parameters.Count() > 0)
            {
                _localeMessage = _localeMessageBL.TryGet(_systemUILocale, messageCode, message, parameters, false, callerContext);
            }
            else
            {
                _localeMessage = _localeMessageBL.TryGet(_systemUILocale, messageCode, message, false, callerContext);
            }

            if (_localeMessage != null)
            {
                operationResult.AddOperationResult(_localeMessage.Code, _localeMessage.Message, operationResultType);
            }
        }

        #endregion

        #endregion

        #endregion
    }
}