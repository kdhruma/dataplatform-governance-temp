using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Text;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System.Xml;
using System.Data.SqlClient;

namespace MDM.SearchManager.Business
{
    using BusinessObjects;
    using ConfigurationManager.Business;
    using Core;
    using Data;
    using Utility;
    using Core.Exceptions;
    using PermissionManager.Business;
    using MessageManager.Business;
    using RS.MDM.Configuration;
    using RS.MDM.ConfigurationObjects;
    using RS.MDM.Events;
    using MDM.Services;
    using MDM.AttributeModelManager.Business;
    using MDM.Interfaces;
    using System.Text.RegularExpressions;
    using MDM.BusinessObjects.Diagnostics;

    /// <summary>
    /// EntitySearch Manager
    /// </summary>
    public class EntitySearchBL : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// Constant value that used for replace OR operator in text of search
        /// </summary>
        private const String _orSeperator = "#@@#";

        /// <summary>
        /// Field denoting locale Message
        /// </summary>
        private LocaleMessage _localeMessage = null;

        /// <summary>
        /// Field denoting localeMessageBL
        /// </summary>
        private LocaleMessageBL _localeMessageBL = new LocaleMessageBL();

        /// <summary>
        /// Specifies security principal for user.
        /// </summary>
        private SecurityPrincipal _securityPrincipal = null;

        /// <summary>
        /// Specifies the name currentTimeZone
        /// </summary>
        private String _currentTimeZone = String.Empty;

        /// <summary>
        /// Specifies the name System Time Zone
        /// </summary>
        private String _systemTimeZone = String.Empty;

        /// <summary>
        /// Indicates current trace settings
        /// </summary>
        private TraceSettings _currentTraceSettings = null;

        /// <summary>
        /// Field denoting the Entity Manager
        /// </summary>
        private readonly IEntityManager _entityManager;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public EntitySearchBL()
        {
            _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            GetSecurityPrincipal();
        }

        /// <summary>
        /// Initialize entity search bl instance with input param iEntityManage
        /// </summary>
        /// <param name="iEntityManager">Indicates instance of EntityBL</param>
        public EntitySearchBL(IEntityManager iEntityManager)
        {
            _entityManager = iEntityManager;
            _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            GetSecurityPrincipal();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting CurrentTimeZone
        /// </summary>
        public String CurrentTimeZone
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_currentTimeZone))
                {
                    if (_securityPrincipal != null && _securityPrincipal.UserPreferences != null && !String.IsNullOrWhiteSpace(_securityPrincipal.UserPreferences.DefaultTimeZoneShortName))
                        _currentTimeZone = _securityPrincipal.UserPreferences.DefaultTimeZoneShortName;

                }
                return _currentTimeZone;
            }
        }

        /// <summary>
        /// Property denoting SystemTimeZone
        /// </summary>
        public String SystemTimeZone
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_systemTimeZone))
                {
                    _systemTimeZone = AppConfigurationHelper.GetAppConfig<String>("TimeZone.SystemTimeZone");
                }

                return _systemTimeZone;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Search entities in system for given search criteria and return list of entities with specified context.
        /// </summary>
        /// <param name="searchCriteria">Provides search criteria.</param>
        /// <param name="searchContext">Provides search context. Example: SearchContext.MaxRecordCount indicates max records to be fetched while searching and AttributeIdList indicates List of attributes to load in returned entities.</param>
        /// <param name="totalCount">Indicates count of results fetched</param>
        /// <param name="callerContext">Indicates application and method which called this method</param>
        /// <returns>SearchRead result with entities searched.</returns>
        public SearchReadResult SearchEntities(SearchCriteria searchCriteria, SearchContext searchContext, ref Int32 totalCount, CallerContext callerContext)
        {
            SearchReadResult searchReadResult = SearchEntitiesMain(searchCriteria, searchContext, callerContext);
            OperationResult operationResult = searchReadResult.OperationResult;

            if (operationResult.OperationResultStatus == OperationResultStatusEnum.None || operationResult.OperationResultStatus == OperationResultStatusEnum.Successful)
            {
                DataTable dataTable = EntitySearchHelper.ConvertFromEntityCollectionToDataTable(searchReadResult.Entities, searchContext);

                searchReadResult.Entities = null;
                searchReadResult.DataTable = dataTable;
                totalCount = searchReadResult.TotalCount = dataTable.Rows.Count;
            }

            return searchReadResult;
        }

        /// <summary>
        /// Search Entities for given search criteria and return list of entities with specified context. 
        /// </summary>
        /// <param name="searchCriteria">Provides search criteria.</param>
        /// <param name="searchContext">Provides search context. Example: SearchContext.MaxRecordCount indicates max records to be fetched while searching and AttributeIdList indicates List of attributes to load in returned entities.</param>
        /// <param name="callerContext">Provides search criteria.</param>
        /// <returns>Search results - collection of entities</returns>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameter is null</exception>
        /// <exception cref="ArgumentException">Thrown when Category Ids field of Search Criteria is not been populated</exception>
        public SearchReadResult SearchEntities(SearchCriteria searchCriteria, SearchContext searchContext, CallerContext callerContext)
        {
            return SearchEntitiesMain(searchCriteria, searchContext, callerContext);
        }

        /// <summary>
        /// Search Entities for given search criteria and return list of entities with specified context. 
        /// </summary>
        /// <param name="searchCriteria">Provides search criteria.</param>
        /// <param name="searchContext">Provides search context. Example: SearchContext.MaxRecordCount indicates max records to be fetched while searching and AttributeIdList indicates List of attributes to load in returned entities.</param>
        /// <param name="callerContext">Provides search criteria.</param>
        /// <param name="searchOperationResult"></param>
        /// <returns>Search results - collection of entities</returns>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameter is null</exception>
        /// <exception cref="ArgumentException">Thrown when Category Ids field of Search Criteria is not been populated</exception>
        public EntityMapCollection SearchEntitiesByExtendedSearchCriteria(ExtendedSearchCriteria searchCriteria, SearchContext searchContext, OperationResult searchOperationResult, CallerContext callerContext)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            Boolean isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;

            EntityMapCollection entityMaps = null;

            try
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                #region Step : Data Validations

                ValidateSearchParameters(searchCriteria, searchContext, String.Empty, callerContext, diagnosticActivity, false);

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Data validations completed");
                }

                #endregion Step : Data Validations

                #region Step : Initial Setup

                Boolean getSearchResults = true;
                SecurityPermissionDefinitionCollection securityPermissionDefinitions = null;
                LocaleEnum systemDataLocale = GlobalizationHelper.GetSystemDataLocale();

                if (!searchCriteria.Locales.Contains(systemDataLocale))
                {
                    searchCriteria.Locales.Add(systemDataLocale);
                }

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Initial setup and config reads completed");
                }

                #endregion Step : Initial Setup

                #region Step : Get Security Permission Definition for AVS

                getSearchResults = GetAndVerifySecurityPermissions(searchCriteria, searchContext, searchOperationResult, systemDataLocale, out securityPermissionDefinitions, callerContext, diagnosticActivity);

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("AVS security validated");
                }

                #endregion Step : Get Security Permission Definition for AVS

                if (getSearchResults)
                {
                    #region Step : De-format Search Attribute Values

                    AttributeModelCollection searchAttributeModelCollection = GetSearchAttributeModelCollection(searchContext, searchCriteria.SearchAttributeRules, searchCriteria.Locales[0], diagnosticActivity);

                    //Locale Collection will have 2 locales.
                    //1st locale is User's current data locale
                    searchCriteria.SearchAttributeRules = GetDeformattedSearchAttributeValues(searchCriteria.SearchAttributeRules, searchCriteria.Locales[0], searchAttributeModelCollection, searchOperationResult, ref getSearchResults, diagnosticActivity);

                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo("De-format of search attributes completed");
                    }

                    #endregion Step : De-format Search Attribute Values

                    if (getSearchResults)
                    {
                        #region Step : Inject Configured Data to Search Criteria

                        InjectConfiguredDataToSearchCriteria(searchCriteria, diagnosticActivity);

                        if (isTracingEnabled)
                        {
                            diagnosticActivity.LogDurationInfo("Data injection configuration to search criteria completed");
                        }

                        #endregion Step : Inject Configured Data to Search Criteria

                        #region Step : Load Search Result from Database

                        DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Search);

                        EntitySearchDA entitySearchDA = new EntitySearchDA();
                        entityMaps = entitySearchDA.SearchEntitiesByExtendedSearchCriteria(searchCriteria, searchContext, systemDataLocale, _securityPrincipal.CurrentUserName, command);

                        if (_currentTraceSettings.IsBasicTracingEnabled)
                        {
                            diagnosticActivity.LogDurationInfo("Search entities database call completed");
                        }

                        #endregion Step : Load Search Result from Database
                    }
                }
            }
            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return entityMaps;
        }

        #endregion

        #region Private Methods

        #region Search Entities Methods

        /// <summary>
        /// /// Search entities in system for given search criteria and return list of entities with specified context. 
        /// </summary>
        /// <param name="searchCriteria">Provides search criteria.</param>
        /// <param name="searchContext">Provides search context. Example: SearchContext.MaxRecordCount indicates max records to be fetched while searching and AttributeIdList indicates List of attributes to load in returned entities.</param>
        /// <param name="callerContext">Indicates application and method which called this method</param>
        /// <returns>DataTable with entities searched.</returns>
        private SearchReadResult SearchEntitiesMain(SearchCriteria searchCriteria, SearchContext searchContext, CallerContext callerContext)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            Boolean isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;

            SearchReadResult searchReadResult = new SearchReadResult();
            OperationResult searchOperationResult = searchReadResult.OperationResult;

            try
            {
                #region Diagnostics and tracing

                if (isTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                #endregion Diagnostics and tracing

                #region Step : Data Validations

                String searchConfigurationXml = GetSearchConfiguration(searchCriteria, callerContext, diagnosticActivity);

                ValidateSearchParameters(searchCriteria, searchContext, searchConfigurationXml, callerContext, diagnosticActivity);

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Data validations completed");
                }

                #endregion Step : Data Validations

                #region Step : Initial Setup

                Boolean getSearchResults = true;
                SecurityPermissionDefinitionCollection securityPermissionDefinitions = null;
                LocaleEnum systemDataLocale = GlobalizationHelper.GetSystemDataLocale();

                if (!searchCriteria.Locales.Contains(systemDataLocale))
                {
                    searchCriteria.Locales.Add(systemDataLocale);
                }

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Initial setup and config reads completed");
                }

                #endregion Step : Initial Setup

                #region Step : Calculate Security Permission Definition for AVS

                getSearchResults = GetAndVerifySecurityPermissions(searchCriteria, searchContext, searchOperationResult, systemDataLocale, out securityPermissionDefinitions, callerContext, diagnosticActivity);

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("AVS security validated.");
                }

                #endregion Step : Calculate Security Permission Definition for AVS

                if (getSearchResults)
                {
                    #region Step : Inject Configured Data to Search Criteria

                    InjectConfiguredDataToSearchCriteria(searchCriteria, diagnosticActivity);

                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo("Data injection configuration to search criteria completed");
                    }

                    #endregion Step : Inject Configured Data to Search Criteria

                    #region Step : De-format Search Attribute Values

                    AttributeModelCollection searchAttributeModels = GetSearchAttributeModelCollection(searchContext, searchCriteria.SearchAttributeRules, searchCriteria.Locales[0], diagnosticActivity);

                    //Locale Collection will have 2 locales.
                    //1st locale is User's current data locale
                    searchCriteria.SearchAttributeRules = GetDeformattedSearchAttributeValues(searchCriteria.SearchAttributeRules, searchCriteria.Locales[0], searchAttributeModels, searchOperationResult, ref getSearchResults, diagnosticActivity);

                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo("De-format of search attributes completed");
                    }

                    #endregion Step : De-format Search Attribute Values

                    if (getSearchResults)
                    {
                        #region Step : Update Search Criteria for lookup

                        UpdateSearchCriteriaForLookupWSIDs(searchCriteria, searchAttributeModels, callerContext);

                        if (isTracingEnabled)
                        {
                            diagnosticActivity.LogDurationInfo("Search criteria for lookup updated");
                        }

                        #endregion Step : Update Search Criteria for lookup

                        #region Step : Load Search Result from Database

                        DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Search);

                        EntitySearchDA entitySearchDA = new EntitySearchDA();
                        Collection<Int64> entityIdList = null;

                        if (searchContext.IsRetrunAttributeListConfigured)
                        {
                            entityIdList = entitySearchDA.SearchEntities<Collection<Int64>>(searchCriteria, searchContext, searchConfigurationXml, _securityPrincipal.CurrentUserName, command);
                        }
                        else
                        {
                            searchReadResult.Entities = entitySearchDA.SearchEntities<EntityCollection>(searchCriteria, searchContext, searchConfigurationXml, _securityPrincipal.CurrentUserName, command);
                        }

                        if (_currentTraceSettings.IsBasicTracingEnabled)
                        {
                            diagnosticActivity.LogDurationInfo("Search entities database call completed");
                        }

                        #endregion Step : Load Search Result from Database

                        #region Step : Load Entities with return attributes

                        if (entityIdList != null && entityIdList.Count > 0)
                        {
                            EntityContext entityContext = new EntityContext()
                            {
                                LoadEntityProperties = true,
                                LoadAttributes = true,
                                Locale = searchCriteria.Locales[0],
                                DataLocales = searchContext.GetReturnAttributeLocaleList(),
                                AttributeIdList = searchContext.GetReturnAttributeIdList(),
                                LoadLookupDisplayValues = true
                            };

                            searchReadResult.Entities = GetEntities(entityIdList, entityContext, callerContext, diagnosticActivity);
                        }

                        #endregion Step : Load Entities with return attributes

                        #region Update Permission Set for AVS

                        Boolean isAVSEnabled = securityPermissionDefinitions != null && securityPermissionDefinitions.Count > 0 ? true : false;

                        if (isAVSEnabled && searchReadResult.Entities != null)
                        {
                            foreach (Entity entity in searchReadResult.Entities)
                            {
                                Collection<UserAction> permissionSet = new Collection<UserAction>();

                                if (securityPermissionDefinitions != null && securityPermissionDefinitions.Count > 0)
                                {
                                    foreach (SecurityPermissionDefinition securityPermissionDefiniton in securityPermissionDefinitions)
                                    {
                                        Boolean isMatchFound = false;

                                        if (securityPermissionDefiniton.PermissionValues.Contains("[rsall]"))
                                        {
                                            permissionSet = securityPermissionDefiniton.PermissionSet;
                                            break;
                                        }
                                        else
                                        {
                                            Attribute attribute = (Attribute)entity.GetAttribute(securityPermissionDefiniton.ApplicationContext.AttributeName);

                                            if (attribute != null)
                                            {
                                                ValueCollection values = (ValueCollection)attribute.GetCurrentValues();

                                                if (values != null && values.Count > 0)
                                                {
                                                    foreach (Value value in values)
                                                    {
                                                        String val = (value.InvariantVal != null) ? value.InvariantVal.ToString() : String.Empty;

                                                        if (securityPermissionDefiniton.PermissionValues.Contains(val.Trim().ToLowerInvariant()))
                                                        {
                                                            isMatchFound = true;
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                isMatchFound = false;
                                            }
                                        }

                                        if (isMatchFound)
                                        {
                                            if (securityPermissionDefiniton.PermissionSet != null && securityPermissionDefiniton.PermissionSet.Count > 0)
                                            {
                                                foreach (UserAction userAction in securityPermissionDefiniton.PermissionSet)
                                                {
                                                    if (!permissionSet.Contains(userAction))
                                                    {
                                                        permissionSet.Add(userAction);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    entity.PermissionSet = permissionSet;
                                }
                            }
                        }

                        #endregion
                    }
                }
                else
                {
                    //No security permission definition available..
                    //This means user is not having any permissions.
                    //Log message and skip getting search results.

                    _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111848", false, callerContext); //View/Edit permissions are not available for the requested search criteria results.
                    searchOperationResult.AddOperationResult("111848", _localeMessage.Message, OperationResultType.Error);
                }
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("Incorrect syntax near the keyword 'and'"))
                {
                    //Add an info message..   
                    _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "112633", false, callerContext); //Invalid search criteria
                    searchOperationResult.AddOperationResult("112633", _localeMessage.Message, OperationResultType.Error);
                }
                else
                {
                    searchOperationResult.AddOperationResult(String.Empty, ex.Message, OperationResultType.Error);
                }
            }
            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return searchReadResult;
        }

        /// <summary>
        /// Gets entity collection based on given entity id list
        /// </summary>
        /// <param name="entityIdList">Indicates list of entity id</param>
        /// <param name="entityContext">Indicates entity context</param>
        /// <param name="callerContext">Indicates application ane module name of caller</param>
        /// <param name="diagnosticActivity">Indicates diagnostic activity</param>
        /// <returns>Returns entity collection</returns>
        private EntityCollection GetEntities(Collection<Int64> entityIdList, EntityContext entityContext, CallerContext callerContext, DiagnosticActivity diagnosticActivity)
        {
            Boolean isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;
            EntityCollection entities = null;

            try
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                EntityGetOptions entityGetOptions = new EntityGetOptions()
                {
                    ApplyAVS = false,
                    PublishEvents = false,
                    UpdateCache = false,
                    UpdateCacheStatusInDB = false
                };

                entities = _entityManager.Get(entityIdList, entityContext, entityGetOptions, callerContext);
            }
            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return entities;
        }

        #endregion Search Entities Methods

        #region Attribute Model Get Methods

        /// <summary>
        /// Gets the AttributeModel Collection for the given Search Criteria (gets Both return attributes and search attributes)
        /// </summary>
        /// <param name="searchContext">Search Context</param>
        /// <param name="searchAttributeRules">search attribute list</param>
        /// <param name="currentDataLocale">current data locale</param>
        /// <returns></returns>
        private AttributeModelCollection GetSearchAttributeModelCollection(SearchContext searchContext, Collection<SearchAttributeRule> searchAttributeRules, LocaleEnum currentDataLocale, DiagnosticActivity diagnosticActivity)
        {
            Boolean isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;
            AttributeModelCollection attributeModels = new AttributeModelCollection();

            try
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                Collection<Attribute> returnAttributes = searchContext.ReturnAttributeList;
                Collection<Int32> attributeIdList = new Collection<Int32>();

                #region Get Attribute Id List

                foreach (Attribute attribute in returnAttributes)
                {
                    attributeIdList.Add(attribute.Id);
                }

                //Collect all Attribute Ids from Search Rule
                foreach (SearchAttributeRule searchAttributeRule in searchAttributeRules)
                {
                    if (searchAttributeRule.Operator != SearchOperator.HasNoValue
                        || searchAttributeRule.Operator != SearchOperator.HasValue
                        || searchAttributeRule.Attribute.Id > 0)
                    {
                        attributeIdList.Add(searchAttributeRule.Attribute.Id);
                    }
                }

                #endregion

                if (attributeIdList.Count > 0)
                {
                    #region Create AttributeModelContext

                    AttributeModelContext attributeModelContext = new AttributeModelContext
                    {
                        AttributeModelType = AttributeModelType.AttributeMaster,
                        CategoryId = 0,
                        ContainerId = 0,
                        EntityId = 0,
                        EntityTypeId = 0,
                        GetCompleteDetailsOfAttribute = true,
                        GetOnlyRequiredAttributes = false,
                        GetOnlyShowAtCreationAttributes = false,
                        RelationshipTypeId = 0
                    };

                    Collection<LocaleEnum> locales = new Collection<LocaleEnum> { currentDataLocale };
                    attributeModelContext.Locales = locales;

                    #endregion Create AttributeModelContext

                    Boolean isAttributeModelsAvailableLocally = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.WebEngine.LocalAttributeModelManager.Enabled", true);
                    if (isAttributeModelsAvailableLocally)
                    {
                        AttributeModelBL attributeModelBL = new AttributeModelBL();
                        attributeModels = attributeModelBL.GetByIds(attributeIdList, attributeModelContext);
                    }
                    else
                    {
                        DataModelService dataModelService = new DataModelService();
                        attributeModels = (AttributeModelCollection)dataModelService.GetAttributeModelsByIds(attributeIdList, attributeModelContext);
                    }

                    for (Int32 i = 0; i < returnAttributes.Count; i++)
                    {
                        Attribute returnAttribute = returnAttributes[i];
                        IAttributeModel attributeModel = attributeModels.GetAttributeModel(returnAttribute.Id,
                            returnAttribute.Locale);

                        // populate return attribute properties based on model found.
                        if (attributeModel != null)
                        {
                            var populatedReturnAttribute = (Attribute)MDMObjectFactory.GetIAttribute(attributeModel);

                            populatedReturnAttribute.Name = returnAttribute.Name; // workaround for entity explorer where LongName is passed instead of Name

                            searchContext.ReturnAttributeList[i] = populatedReturnAttribute;
                        }
                    }
                }

            }
            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return attributeModels;
        }

        #endregion

        #region De-formatted Methods

        /// <summary>
        /// Deformats input in Search Attributes
        /// </summary>
        /// <param name="searchAttributeRules"></param>
        /// <param name="currentDataLocale"></param>
        /// <param name="attributeModelCollection"></param>
        /// <param name="operationResult"></param>
        /// <param name="getSearchResults"></param>
        /// <returns></returns>
        private Collection<SearchAttributeRule> GetDeformattedSearchAttributeValues(Collection<SearchAttributeRule> searchAttributeRules, LocaleEnum currentDataLocale, AttributeModelCollection attributeModelCollection, OperationResult operationResult, ref Boolean getSearchResults, DiagnosticActivity diagnosticActivity)
        {
            Boolean isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;
            Collection<SearchAttributeRule> deformattedSearchAttributeRules = new Collection<SearchAttributeRule>();

            try
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                if (searchAttributeRules != null && searchAttributeRules.Count > 0)
                {
                    if (isTracingEnabled)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "De-formatting of Search Attribute Values started...", MDMTraceSource.EntitySearch);

                    //Flag to indicate if all search values are valid.
                    getSearchResults = true;

                    //Loop through search attribute rules and De-format data in case of decimal, date and date time
                    // in is applicable only in case of date time
                    foreach (SearchAttributeRule searchAttributeRule in searchAttributeRules)
                    {
                        //indicates if current search attribute rule needs to be replaced
                        Boolean isFormattedSearchAttributeRule = false;

                        Collection<String> searchValues = new Collection<String>();
                        String searchVal = (String)searchAttributeRule.Attribute.OverriddenValue;

                        if (searchVal != null)
                        {
                            searchVal = searchVal.Trim();
                        }

                        AttributeModel attributeModel = new AttributeModel();

                        //In case of keyword search, attributeId is 0. AttributeModelCollection does not hold a model for this Id. 
                        //Hence not getting attribute model from attribute models.
                        if (searchAttributeRule.Attribute.Id > 0)
                        {
                            attributeModel = attributeModelCollection[searchAttributeRule.Attribute.Id, currentDataLocale];
                        }

                        AttributeDataType attributeDataType = AttributeDataType.Unknown;
                        Enum.TryParse(attributeModel.AttributeDataTypeName, out attributeDataType);

                        Int32 attributeModelId = attributeModel.Id;
                        Boolean isAttributeComplexChild = attributeModel.IsComplexChild;

                        //Remove special characters from attribute value.
                        //Short name,long name,RelatedEntityName,Decimal,Integer,Fraction,Date,DateTime and complex child attributes are core table search,
                        //so for that special characters are required to be searched so it is not removed.
                        //Other search are DN_Search so special characters to be removed.
                        if (!(String.Compare(attributeModel.AttributeDataTypeName, AttributeDataType.Decimal.ToString(), true) == 0 ||
                            String.Compare(attributeModel.AttributeDataTypeName, AttributeDataType.Fraction.ToString(), true) == 0 ||
                            String.Compare(attributeModel.AttributeDataTypeName, AttributeDataType.Date.ToString(), true) == 0 ||
                            String.Compare(attributeModel.AttributeDataTypeName, AttributeDataType.DateTime.ToString(), true) == 0 ||
                            String.Compare(attributeModel.AttributeDataTypeName, AttributeDataType.Integer.ToString(), true) == 0 ||
                            attributeModelId == 22 || attributeModelId == 23 || attributeModelId == 28 || isAttributeComplexChild))//These Ids are for ShortName, LongName & RelatedEntityName
                        {
                            searchVal = CleanseSearchAttributeValue(searchAttributeRule, searchVal);
                        }

                        if (attributeDataType.Equals(AttributeDataType.String))
                        {
                            //In case of search for multiple values, the operator should be "In" and shouldnot be "Equal".
                            if (searchVal != null && searchVal.Contains("' OR '"))
                                searchAttributeRule.Operator = SearchOperator.In;

                            AttributeDisplayType attributeDisplayType = AttributeDisplayType.Unknown;
                            Enum.TryParse(attributeModel.AttributeDisplayTypeName, out attributeDisplayType);

                            if (attributeDisplayType != AttributeDisplayType.LookupTable)
                            {
                                if (attributeModel.Id == 22 || attributeModel.Id == 23 || attributeModel.Id == 28)
                                {
                                    if (searchAttributeRule.Operator == SearchOperator.In)
                                    {
                                        searchVal = searchVal.Replace(" OR ", _orSeperator);
                                    }
                                    else if (searchVal.Contains(',') && searchAttributeRule.Operator == SearchOperator.EqualTo)
                                    {
                                        searchVal = searchVal + _orSeperator;
                                    }
                                }
                                else
                                {
                                    //Before populating search attribute rule, "OR" should be replaced with comma(,).
                                    searchVal = searchVal == null ? "" : searchVal.Replace("' OR '", "','");
                                }
                                isFormattedSearchAttributeRule = true;
                            }
                        }

                        searchValues = ConstructSearchString(searchVal);

                        //by pass conversion in case of Has Value and Has No Value
                        if (searchAttributeRule.Operator == SearchOperator.HasNoValue
                            || searchAttributeRule.Operator == SearchOperator.HasValue
                            || searchAttributeRule.Attribute.Id == 0)
                        {
                            searchVal = GenerateCommaSeparatedSearchValue(searchValues);
                            UpdateSearchAttributeRule(searchAttributeRule, searchVal);
                            deformattedSearchAttributeRules.Add(searchAttributeRule);
                            continue;
                        }

                        //By pass short name, long name and other MetaDataAttribute attributes and lookup attributes
                        if (attributeModel.Context.AttributeModelType == AttributeModelType.MetaDataAttribute)
                        {
                            Boolean isLookup = false;
                            if (searchAttributeRule.Operator == SearchOperator.In && attributeModel.Id >= 96 && attributeModel.Id <= 98)
                            {
                                /* Below workflow attributes are treated as lookup, so making them as isLookup = true so that values are marked with " ' ' "
                                 * e.g., "'HumanWork'"
                                 * 96 - CurrentWorkflowName
                                 * 97 - CurrentActivityName
                                 * 98 - CurrentAssignedUser
                                 */
                                isLookup = true;
                            }

                            searchVal = GenerateCommaSeparatedSearchValue(searchValues, isLookup);
                            UpdateSearchAttributeRule(searchAttributeRule, searchVal);
                            deformattedSearchAttributeRules.Add(searchAttributeRule);
                            continue;
                        }

                        if (attributeModel.IsLookup)
                        {
                            searchValues = UpdateSearchCriteria(searchAttributeRule, searchValues, new CallerContext());
                            searchVal = GenerateCommaSeparatedSearchValue(searchValues, true);
                            UpdateSearchAttributeRule(searchAttributeRule, searchVal);
                            deformattedSearchAttributeRules.Add(searchAttributeRule);
                            continue;
                        }

                        //deformatted search value
                        Collection<String> deformattedSearchValues = new Collection<string>();

                        if (String.Compare(attributeModel.AttributeDataTypeName, AttributeDataType.Fraction.ToString(), true) == 0)
                        {
                            isFormattedSearchAttributeRule = true;
                            deformattedSearchValues = DeformatFraction(searchAttributeRule.Attribute.GetCurrentValues(), currentDataLocale, attributeModel.LongName, operationResult, ref getSearchResults);
                            searchVal = GenerateCommaSeparatedSearchValue(deformattedSearchValues);
                        }
                        else if (String.Compare(attributeModel.AttributeDataTypeName, AttributeDataType.Integer.ToString(), true) == 0)
                        {
                            isFormattedSearchAttributeRule = true;
                            deformattedSearchValues = DeformatInteger(searchAttributeRule, attributeModel.LongName, operationResult, ref getSearchResults);
                            searchVal = GenerateCommaSeparatedSearchValue(deformattedSearchValues);
                        }
                        //deformat input in case of date
                        else if (String.Compare(attributeModel.AttributeDataTypeName, AttributeDataType.Date.ToString(), true) == 0)
                        {
                            isFormattedSearchAttributeRule = true;
                            deformattedSearchValues = DeformatDate(searchValues, currentDataLocale, attributeModel.LongName, operationResult, ref getSearchResults);
                            searchVal = GenerateCommaSeparatedSearchValue(deformattedSearchValues);
                        }
                        //deformat input in case of date time
                        else if (String.Compare(attributeModel.AttributeDataTypeName, AttributeDataType.DateTime.ToString(), true) == 0)
                        {
                            isFormattedSearchAttributeRule = true;
                            deformattedSearchValues = GetDeformattedDateTimeValues(searchValues, currentDataLocale, attributeModel.LongName, attributeModel.ApplyTimeZoneConversion, operationResult, ref getSearchResults);
                            searchVal = GenerateCommaSeparatedSearchValue(deformattedSearchValues);
                        }
                        //deformat input of decimal
                        else if (String.Compare(attributeModel.AttributeDataTypeName, AttributeDataType.Decimal.ToString(), true) == 0)
                        {
                            isFormattedSearchAttributeRule = true;
                            deformattedSearchValues = DeformatDecimal(searchValues, currentDataLocale, attributeModel, operationResult, ref getSearchResults);
                            searchVal = GenerateCommaSeparatedSearchValue(deformattedSearchValues);
                        }

                        //if all inputs are valid
                        if (getSearchResults)
                        {
                            //if current search attribute rule needs to be replaced by deformatted search attribute rule clear and add deformatted values
                            if (isFormattedSearchAttributeRule)
                            {
                                UpdateSearchAttributeRule(searchAttributeRule, searchVal);
                            }
                            //add searchAttributeRule
                            deformattedSearchAttributeRules.Add(searchAttributeRule);
                        }

                        ResolveSpecialKeywords(searchAttributeRule);
                    }
                }
            }
            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return deformattedSearchAttributeRules;
        }

        /// <summary>
        /// Fetches deformatted value in case of date and date time
        /// </summary>
        /// <param name="searchValues"></param>
        /// <param name="currentDataLocale"></param>
        /// <param name="attributeName"></param>
        /// <param name="applyTimeZoneConversion"></param>
        /// <param name="searchOperationResult"></param>
        /// <param name="getSearchResults"></param>
        /// <returns></returns>
        private Collection<String> GetDeformattedDateTimeValues(Collection<String> searchValues, LocaleEnum currentDataLocale, String attributeName, Boolean applyTimeZoneConversion, OperationResult searchOperationResult, ref Boolean getSearchResults)
        {
            Collection<String> deformattedSearchValues = new Collection<string>();

            //for each date time value, get deformatetd value
            foreach (String formattedValue in searchValues)
            {
                String deformattedValue = String.Empty;

                if (!String.IsNullOrWhiteSpace(formattedValue))
                {
                    //get deformatted value
                    deformattedValue = DeformatDateTime(formattedValue, currentDataLocale, attributeName, applyTimeZoneConversion, searchOperationResult, ref getSearchResults);
                    deformattedSearchValues.Add(deformattedValue);
                }
            }

            return deformattedSearchValues;
        }

        /// <summary>
        /// De-format date and time
        /// </summary>
        /// <param name="formattedValue">Indicates formatter value which needs to be deformatted.</param>
        /// <param name="currentDataLocale">Indicates currently selected locale</param>
        /// <param name="attributeName">Indicates name of attribute</param>
        /// <param name="applyTimeZoneConversion">Indicates apply time zone convesion</param>
        /// <param name="searchOperationResult">Indicates operation result for search.</param>
        /// <param name="getSearchResults">Indicates whether to get search result or not.</param>
        /// <returns>Deformatted DateTime value for search.</returns>
        private String DeformatDateTime(String formattedValue, LocaleEnum currentDataLocale, String attributeName, Boolean applyTimeZoneConversion, OperationResult searchOperationResult, ref Boolean getSearchResults)
        {
            String deformattedValue = String.Empty;

            try
            {
                //Possible value is "'12.2.2012 8:00' OR '12.2.2012 9:00'" so we replace "'" with ""
                String correctedformattedValue = formattedValue.Trim().Replace("\'", String.Empty);

                //get deformatted date
                DateTime selectedDateTime = MDM.Core.FormatHelper.DeformatDate(correctedformattedValue, currentDataLocale.GetCultureName());
                if (selectedDateTime != null && selectedDateTime != DateTime.MinValue)
                {
                    if (!String.IsNullOrWhiteSpace(this.CurrentTimeZone) && applyTimeZoneConversion)
                    {
                        //convert time zone to sstem time zone
                        selectedDateTime = MDM.Core.FormatHelper.ConvertToTimeZone(selectedDateTime, this.CurrentTimeZone, this.SystemTimeZone);
                    }
                }

                deformattedValue = MDM.Core.FormatHelper.StoreDateTimeUtc(selectedDateTime);
            }
            catch
            {
                getSearchResults = false;

                //populate errors
                PopulateSearchOperationResultErrors(searchOperationResult, attributeName);

            }
            //return
            return deformattedValue;
        }

        /// <summary>
        /// Deformat Fraction
        /// </summary>
        /// <param name="attributeName">Indicates name of attribute</param>
        /// <param name="currentDataLocale">Indicates currently selected locale</param>
        /// <param name="getSearchResults">Indicates whether to get search result or not.</param>
        /// <param name="searchOperationResult">Indicates operation result for search.</param>
        /// <param name="searchValues">Indicates value of attribute which is to be searched.</param>
        /// <returns>Returns deformatted fraction value for search.</returns>
        private Collection<String> DeformatFraction(IValueCollection searchValues, LocaleEnum currentDataLocale, String attributeName, OperationResult searchOperationResult, ref Boolean getSearchResults)
        {
            Collection<String> deformattedSearchValues = new Collection<string>();
            String deformattedSearchValue = String.Empty;

            try
            {
                foreach (Value val in searchValues)
                {
                    Int32 precision = 4; //Since the precision is not set for fraction data type when creating attribute, so hardcoding it here as 4 for formatting purpose
                    Decimal decimalValue = ValueTypeHelper.ConvertFractionToDecimal(val.AttrVal.ToString());

                    //Format the value to en_US culture with precision of 4.
                    deformattedSearchValue = MDM.Core.FormatHelper.FormatNumber((Double)decimalValue, LocaleEnum.en_US.GetCultureName(), precision, false);

                    deformattedSearchValues.Add(deformattedSearchValue);
                }
            }
            catch
            {
                getSearchResults = false;

                //populate errors
                PopulateSearchOperationResultErrors(searchOperationResult, attributeName);
            }

            return deformattedSearchValues;
        }

        /// <summary>
        /// Deformat Date
        /// </summary>
        /// <param name="attributeName">Indicates name of attribute</param>
        /// <param name="currentDataLocale">Indicates currently selected locale</param>
        /// <param name="getSearchResults">Indicates whether to get search result or not.</param>
        /// <param name="searchOperationResult">Indicates operation result for search.</param>
        /// <param name="searchValues">Indicates value of attribute which is to be searched.</param>
        /// <returns>Returns deformatted date value for search.</returns>
        private Collection<String> DeformatDate(Collection<String> searchValues, LocaleEnum currentDataLocale, String attributeName, OperationResult searchOperationResult, ref Boolean getSearchResults)
        {
            Collection<String> deformattedSearchValues = new Collection<string>();

            try
            {
                foreach (String val in searchValues)
                {
                    String deformattedSearchValue = String.Empty;
                    deformattedSearchValue = MDM.Core.FormatHelper.FormatDateOnly(val, currentDataLocale.GetCultureName(), LocaleEnum.en_US.GetCultureName());
                    deformattedSearchValues.Add(deformattedSearchValue);
                }
            }
            catch
            {
                getSearchResults = false;

                //populate errors
                PopulateSearchOperationResultErrors(searchOperationResult, attributeName);
            }

            return deformattedSearchValues;
        }

        /// <summary>
        /// Deformat Decimal value.
        /// </summary>
        /// <param name="attributeModel">Indicates model for given attribute.</param>
        /// <param name="currentDataLocale">Indicates currently selected locale</param>
        /// <param name="getSearchResults">Indicates whether to get search result or not.</param>
        /// <param name="searchOperationResult">Indicates operation result for search.</param>
        /// <param name="searchValues">Indicates value of attribute which is to be searched.</param>
        /// <returns>Returns deformatted decimal value for search.</returns>
        private Collection<String> DeformatDecimal(Collection<String> searchValues, LocaleEnum currentDataLocale, AttributeModel attributeModel, OperationResult searchOperationResult, ref Boolean getSearchResults)
        {
            Collection<String> deformattedSearchValues = new Collection<string>();
            String deformattedSearchValue = String.Empty;

            try
            {
                foreach (String value in searchValues)
                {
                    Double deformatedValue = MDM.Core.FormatHelper.DeformatNumber(value, currentDataLocale.GetCultureName());
                    deformattedSearchValue = MDM.Core.FormatHelper.FormatNumber(deformatedValue, LocaleEnum.en_US.GetCultureName(), attributeModel.Precision, attributeModel.IsPrecisionArbitrary);
                    deformattedSearchValues.Add(deformattedSearchValue);
                }
            }
            catch
            {
                getSearchResults = false;

                //populate errors
                PopulateSearchOperationResultErrors(searchOperationResult, attributeModel.LongName);
            }
            return deformattedSearchValues;
        }

        #endregion

        #region Misc Methods

        /// <summary>
        /// Escapes special characters in search attribute values
        /// </summary>
        /// <param name="searchAttributeRules">Indicates SearchAttributeRules for escaping</param>
        /// <returns>Returns cleansed search attribute values</returns>
        private String CleanseSearchAttributeValue(SearchAttributeRule searchAttributeRule, String searchVal)
        {
            const String nonSearchableCharactersRegex = "[.-\\/_#()!@+]";
            const String emptySeparatedArgumentsRegex = ",[, ]+";

            // Replaces special characters with whitespaces so that they will be ignored during entity search
            searchVal = new Regex(nonSearchableCharactersRegex).Replace(searchVal, " ");

            // ',' is used as a separator in full-text search so we need to eliminate empty full-text search arguments separated by ','
            searchVal = searchVal.Trim(new[] { ',', ' ' });
            searchVal = new Regex(emptySeparatedArgumentsRegex).Replace(searchVal, ",");

            UpdateSearchAttributeRule(searchAttributeRule, searchVal);

            return searchVal;
        }

        /// <summary>
        /// Deformat Integer
        /// </summary>
        /// <param name="searchAttributeRule"></param>
        /// <param name="attributeName"></param>
        /// <param name="searchOperationResult"></param>
        /// <param name="getSearchResults"></param>
        private Collection<String> DeformatInteger(SearchAttributeRule searchAttributeRule, String attributeName, OperationResult searchOperationResult, ref Boolean getSearchResults)
        {
            Collection<String> stringCollection = ConstructSearchString(searchAttributeRule.Attribute.GetCurrentValue().ToString());
            Int32 validNumber = 0;
            foreach (string value in stringCollection)
            {
                if (!Int32.TryParse(value, out validNumber))
                {
                    getSearchResults = false;

                    //populate errors
                    PopulateSearchOperationResultErrors(searchOperationResult, attributeName);
                }
            }
            return stringCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchAttributeRule"></param>
        private void ResolveSpecialKeywords(SearchAttributeRule searchAttributeRule)
        {
            Attribute attribute = searchAttributeRule.Attribute;

            if (attribute != null)
            {
                ValueCollection values = (ValueCollection)attribute.GetCurrentValuesInvariant();

                if (values != null)
                {
                    foreach (Value value in values)
                    {
                        String stringVal = value.GetStringValue();

                        switch (stringVal.ToUpperInvariant())
                        {
                            case "@ME":
                                String loginUser = MDM.Utility.SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;
                                value.AttrVal = loginUser;
                                value.InvariantVal = loginUser;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// get current security principal
        /// </summary>
        private void GetSecurityPrincipal()
        {
            if (_securityPrincipal == null)
            {
                _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            }
        }

        /// <summary>
        /// Populate Search Operation Results
        /// </summary>
        /// <param name="searchOperationResult"></param>
        /// <param name="attributeName"></param>
        private void PopulateSearchOperationResultErrors(OperationResult searchOperationResult, String attributeName)
        {
            if (searchOperationResult.Errors != null
                && searchOperationResult.Errors.Count > 0
                && searchOperationResult.Errors[0].Params.Count > 0)
            {
                searchOperationResult.Errors[0].Params[0] = searchOperationResult.Errors[0].Params[0] + ", " + attributeName + " ";
            }
            else
            {
                Error error = new Error();
                error.ErrorCode = "111663";
                error.Params = new Collection<Object>();
                error.Params.Add(attributeName + " ");
                searchOperationResult.Errors.Add(error);
            }
        }

        /// <summary>
        /// Returns search configuration xml of search configuration application configuration object
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private String GetSearchConfiguration(SearchCriteria searchCriteria, CallerContext callerContext, DiagnosticActivity diagnosticActivity)
        {
            Boolean isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;
            String searchConfigurationXml = String.Empty;

            try
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                //default event source is ProductCenter w.r.t. PIM
                EventSourceList eventSource = EventSourceList.ProductCenter;

                if (callerContext.MDMSource != EventSource.UnKnown)
                    Enum.TryParse(callerContext.MDMSource.ToString(), true, out eventSource);

                EventSubscriberList eventSubscriber = EventSubscriberList.MDMCenterSearchConfiguration;

                if (callerContext.MDMSource != EventSource.UnKnown)
                    Enum.TryParse(callerContext.MDMSubscriber.ToString(), true, out eventSubscriber);

                //Create applicationn configuration context
                ApplicationConfiguration applicationConfiguration = new ApplicationConfiguration((int)eventSource, (int)eventSubscriber, 0, 0, searchCriteria.OrganizationId, searchCriteria.ContainerIds.FirstOrDefault());

                //Get Configurations.
                applicationConfiguration.GetConfigurations();

                //Get SearchConfiguration.
                SearchConfiguration searchConfiguration = (SearchConfiguration)applicationConfiguration.GetObject("SearchConfiguration");

                //Get SearchConfiguration Xml
                if (searchConfiguration != null)
                {
                    searchConfigurationXml = searchConfiguration.ToXml();
                }
            }
            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return searchConfigurationXml;
        }

        #endregion

        #region AVS Calculation Methods

        /// <summary>
        /// This method is used for determine Attribute value based permission for search result.
        /// </summary>
        /// <param name="searchCriteria">This parameter is specifying search criteria.</param>
        /// <param name="searchContext">This parameter is specifying search context.</param>
        /// <param name="operationResult">This parameter is specifying search operation result.</param>
        /// <param name="securityPermissionDefinitions">This parameter is specifying security permission definitions.</param>
        /// <param name="callerContext">This parameter is specifying caller context.</param>
        /// <param name="systemDataLocale">This parameter is specifying system data locales.</param>
        /// <returns>returns true if application able to determine attribute value based permission otherwise false.</returns>
        private Boolean DetermineAttributeValueBasedPermission(SearchCriteria searchCriteria, SearchContext searchContext, OperationResult operationResult, LocaleEnum systemDataLocale, SecurityPermissionDefinitionCollection securityPermissionDefinitions, CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Determining Attribute Value Based Permission..", MDMTraceSource.EntitySearch);

            Boolean hasPermission = false;

            SecurityPermissionDefinitionBL securityPermissionDefinitionBL = new SecurityPermissionDefinitionBL();

            if (securityPermissionDefinitions != null && securityPermissionDefinitions.Count > 0)
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Security Permission definition is not empty.", MDMTraceSource.EntitySearch);

                #region Get Config values

                //Get Application config key to decide whether view only entities should be included in workflow panel count or not
                Boolean includeViewOnlyEntities = true;
                String strIncludeViewOnlyEntities = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.Workflow.WorkflowPanel.IncludeViewOnlyEntities");

                try
                {
                    includeViewOnlyEntities = ValueTypeHelper.ConvertToBoolean(strIncludeViewOnlyEntities);
                }
                catch
                {
                    //Ignore error..
                    //Set includeViewOnlyEntities to true
                    includeViewOnlyEntities = true;
                }

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("AppConfig MDMCenter.Workflow.WorkflowPanel.IncludeViewOnlyEntities: {0}", strIncludeViewOnlyEntities), MDMTraceSource.EntitySearch);

                #endregion

                Collection<SearchAttributeRule> searchAttributeRules = new Collection<SearchAttributeRule>();

                foreach (SecurityPermissionDefinition securityPermissionDefinition in securityPermissionDefinitions)
                {
                    if (includeViewOnlyEntities || !(securityPermissionDefinition.PermissionSet.Count == 1 && securityPermissionDefinition.PermissionSet.Contains(UserAction.View) && !String.IsNullOrWhiteSpace(searchCriteria.WorkflowName)))
                    {
                        #region Prepare/Construct Search Attribute Rule

                        #region Initial Setup

                        //Get definition values..
                        String definitionValues = String.Empty;
                        Attribute attribute = null;

                        //Get attribute Id and attribute name
                        Int32 attributeId = securityPermissionDefinition.ApplicationContext.AttributeId;
                        String attributeName = securityPermissionDefinition.ApplicationContext.AttributeName;

                        #endregion

                        #region Read Definition Values from Permission Definition Object

                        if (securityPermissionDefinition.PermissionValues != null && securityPermissionDefinition.PermissionValues.Count > 0)
                        {
                            definitionValues = ValueTypeHelper.JoinCollection(securityPermissionDefinition.PermissionValues, ",");
                        }

                        if (attributeId < 1 && definitionValues.Contains("[rsall]"))
                        {
                            //Having all permissions..
                            //No need put security attribute rule..
                            //Clear rule collection and come out.

                            hasPermission = true;
                            searchAttributeRules.Clear();
                            break;
                        }
                        #endregion

                        #region Check whether the rule for the current definition attribute has already been added
                        //Check whether the rule for the current definition attribute has already been added
                        SearchAttributeRule searchAttributeRule = searchAttributeRules.FirstOrDefault(s => s.Attribute.Id == attributeId);

                        if (searchAttributeRule == null)
                        {
                            //The rule has not been added..
                            //Populate the rule and add to the rule collection
                            searchAttributeRule = new SearchAttributeRule();
                            searchAttributeRule.Attribute = new Attribute();
                            searchAttributeRule.Attribute.Id = attributeId;
                            searchAttributeRule.Attribute.Name = attributeName;
                            searchAttributeRule.Operator = SearchOperator.In;

                            searchAttributeRules.Add(searchAttributeRule);
                        }
                        else
                        {
                            //Append definition value to the existing rule value
                            Object attributeRuleValue = searchAttributeRule.Attribute.GetCurrentValueInvariant();

                            if (attributeRuleValue != null)
                            {
                                definitionValues = String.Concat(attributeRuleValue.ToString(), ",", definitionValues);
                            }
                        }



                        //Set values
                        searchAttributeRule.Attribute.SetValueInvariant(new Value((Object)definitionValues));

                        #endregion

                        #region Check whether the configured security attribute is there or not in return attribute list for search

                        attribute = searchContext.ReturnAttributeList.FirstOrDefault(a => a.Id == attributeId && a.Locale == systemDataLocale);

                        if (attribute == null)
                        {
                            //Not having configured security attribute in search attribute list.
                            //Add into return attribute list for calculating permission set for each entity.

                            attribute = new Attribute();

                            attribute.Id = attributeId;
                            attribute.Name = attributeName;
                            attribute.Locale = systemDataLocale;

                            searchContext.ReturnAttributeList.Add(attribute);
                        }

                        #endregion

                        #endregion
                    }
                    else
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Only view permission is there for entities with Security Permission Definition : {0} and request is for workflow search. Ignoring this definition considering the value of AppConfig 'MDMCenter.Workflow.WorkflowPanel.IncludeViewOnlyEntities'", securityPermissionDefinition.Name), MDMTraceSource.EntitySearch);
                    }
                }

                //Add all search attribute rule to search criteria
                if (searchAttributeRules.Count > 0)
                {
                    hasPermission = true;

                    //Add all search attribute rule to search criteria.
                    foreach (SearchAttributeRule searchAttributeRule in searchAttributeRules)
                    {
                        searchCriteria.SearchAttributeRules.Add(searchAttributeRule);
                    }
                }
            }
            else
            {
                //No security permission definition available..
                //This means user is not having any permissions.
                //Log message and skip getting search results.
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111848", false, callerContext); //View/Edit permissions are not available for the requested search criteria results.

                operationResult.AddOperationResult("111848", _localeMessage.Message, OperationResultType.Error);

                String strContainerIds = String.Empty;
                Collection<int> containerIds = searchCriteria.ContainerIds;

                if (containerIds != null && containerIds.Count > 0)
                {
                    strContainerIds = ValueTypeHelper.JoinCollection(containerIds, ",");
                }
                else
                {
                    strContainerIds = "0";
                }

                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Permissions are not available for the context Org: {0}, Catalog: {1} and User: {2}. Cannot return search results.", searchCriteria.OrganizationId, strContainerIds, _securityPrincipal.CurrentUserName), MDMTraceSource.EntitySearch);

                hasPermission = false;
            }

            return hasPermission;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <param name="searchContext"></param>
        /// <param name="searchOperationResult"></param>
        /// <param name="systemDataLocale"></param>
        /// <param name="securityPermissionDefinitions"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private Boolean GetAndVerifySecurityPermissions(SearchCriteria searchCriteria, SearchContext searchContext, OperationResult searchOperationResult, LocaleEnum systemDataLocale, out SecurityPermissionDefinitionCollection securityPermissionDefinitions, CallerContext callerContext, DiagnosticActivity diagnosticActivity)
        {
            Boolean isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;
            Boolean result = false;

            try
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                SecurityPermissionDefinitionBL securityPermissionDefinitionBL = new SecurityPermissionDefinitionBL();
                ApplicationContext applicationContext = new ApplicationContext();

                applicationContext.OrganizationId = searchCriteria.OrganizationId;
                applicationContext.ContainerId = searchCriteria.ContainerIds.FirstOrDefault();
                applicationContext.UserId = _securityPrincipal.CurrentUserId;
                applicationContext.UserName = _securityPrincipal.CurrentUserName;

                securityPermissionDefinitions = securityPermissionDefinitionBL.Get(applicationContext, callerContext);

                //Load Attribute Value based security search criteria
                result = DetermineAttributeValueBasedPermission(searchCriteria, searchContext, searchOperationResult, systemDataLocale, securityPermissionDefinitions, callerContext);
            }
            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return result;
        }

        #endregion

        #region Injection Methods for Search Criteria

        /// <summary>
        /// Injects any additional data to be added in the search criteria specified in the Application Config
        /// </summary>
        /// <param name="searchCriteria"></param>
        private void InjectConfiguredDataToSearchCriteria(SearchCriteria searchCriteria, DiagnosticActivity diagnosticActivity)
        {
            Boolean isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;

            try
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                Int32 containerId = 0;
                Int64 categoryId = 0;
                Int32 entityTypeId = 0;

                if (searchCriteria != null)
                {
                    if (searchCriteria.ContainerIds != null && searchCriteria.ContainerIds.Count == 1)
                    {
                        containerId = searchCriteria.ContainerIds.FirstOrDefault();
                    }
                    if (searchCriteria.CategoryIds != null && searchCriteria.CategoryIds.Count == 1)
                    {
                        categoryId = searchCriteria.CategoryIds.FirstOrDefault();
                    }
                    if (searchCriteria.EntityTypeIds != null && searchCriteria.EntityTypeIds.Count == 1)
                    {
                        entityTypeId = searchCriteria.EntityTypeIds.FirstOrDefault();
                    }
                }

                ApplicationConfiguration applicationConfiguration = new ApplicationConfiguration((Int32)EventSource.EntityExplorer, (Int32)EventSubscriber.SearchManager, _securityPrincipal.UserPreferences.DefaultRoleId, _securityPrincipal.UserPreferences.LoginId, searchCriteria.OrganizationId, containerId, categoryId, 0, 0, entityTypeId, 0, 0, 0);
                applicationConfiguration.GetConfigurations();

                InjectionSearchCriteria injectionSearchCriteria = (InjectionSearchCriteria)applicationConfiguration.GetObject("SearchCriteria");

                if (injectionSearchCriteria != null)
                {
                    InjectConfiguredFilterToSearchCriteria(injectionSearchCriteria, searchCriteria);
                }

                SearchWeightage searchWeightageAttributes = (SearchWeightage)applicationConfiguration.GetObject("SearchWeightage");

                if (searchWeightageAttributes != null)
                {
                    InjectConfiguredWeightageToSearchCriteria(searchWeightageAttributes, searchCriteria);
                }

                WorkflowSearchConfiguration workflowSearchConfiguration = (WorkflowSearchConfiguration)applicationConfiguration.GetObject("SearchWorkflow");

                if (workflowSearchConfiguration != null)
                {
                    InjectConfiguredWorkflowDetailsToSearchCriteria(workflowSearchConfiguration, searchCriteria);
                }
            }
            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }
        }

        /// <summary>
        /// Injects the configured workflowname and enable/disable workflow result in Grid
        /// </summary>
        /// <param name="workflowSearchConfiguration"></param>
        /// <param name="searchCriteria"></param>
        private void InjectConfiguredWorkflowDetailsToSearchCriteria(WorkflowSearchConfiguration workflowSearchConfiguration, SearchCriteria searchCriteria)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
            {
                MDMTraceHelper.StartTraceActivity(MDMTraceSource.EntitySearch);
            }

            try
            {
                searchCriteria.ConfiguredWorkflowForSearch = workflowSearchConfiguration.WorkflowName;
                searchCriteria.ReturnWorkflowResult = workflowSearchConfiguration.ReturnWorkflowResult;
            }
            finally
            {
                if (isTracingEnabled)
                {
                    MDMTraceHelper.StopTraceActivity();
                }
            }
        }

        /// <summary>
        /// Injects the additional search filter specified in the Application Configuration
        /// </summary>
        /// <param name="injectionSearchCriteria"></param>
        /// <param name="searchCriteria"></param>
        private void InjectConfiguredFilterToSearchCriteria(InjectionSearchCriteria injectionSearchCriteria, SearchCriteria searchCriteria)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
            {
                MDMTraceHelper.StartTraceActivity(MDMTraceSource.EntitySearch);
            }

            try
            {
                if (injectionSearchCriteria != null && injectionSearchCriteria.InjectionSearchAttributeRules != null && injectionSearchCriteria.InjectionSearchAttributeRules.Count > 0)
                {
                    foreach (InjectionSearchAttributeRule injectionSearchAttributeRule in injectionSearchCriteria.InjectionSearchAttributeRules)
                    {
                        InjectionAttribute injectionAttribute = injectionSearchAttributeRule.InjectionAttribute;
                        if (injectionAttribute != null && injectionAttribute.AttributeID > 0)
                        {
                            SearchAttributeRule searchAttributeRule = new SearchAttributeRule(injectionAttribute.AttributeID, injectionAttribute.AttributeModelType, injectionAttribute.CurrentValue, injectionAttribute.Operator);
                            searchCriteria.SearchAttributeRules.Add(searchAttributeRule);
                        }
                    }
                }
            }
            finally
            {
                if (isTracingEnabled)
                {
                    MDMTraceHelper.StopTraceActivity();
                }
            }
        }

        /// <summary>
        /// Injects configured Weigtage specifed for partiular attribute in application config
        /// </summary>
        /// <param name="searchWeightageAttributes"></param>
        /// <param name="searchCriteria"></param>
        private void InjectConfiguredWeightageToSearchCriteria(SearchWeightage searchWeightageAttributes, SearchCriteria searchCriteria)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
            {
                MDMTraceHelper.StartTraceActivity(MDMTraceSource.EntitySearch);
            }

            try
            {
                Collection<BusinessObjects.SearchWeightageAttribute> attributes = new Collection<BusinessObjects.SearchWeightageAttribute>();
                if (searchWeightageAttributes != null && searchWeightageAttributes.SearchWeightageAttributeCollection.Count > 0)
                {
                    foreach (RS.MDM.ConfigurationObjects.SearchWeightageAttribute searchWeightageAttribute in searchWeightageAttributes.SearchWeightageAttributeCollection)
                    {
                        if (searchWeightageAttribute.WeightageValues.Count > 0)
                        {
                            foreach (WeightageValue weightageValue in searchWeightageAttribute.WeightageValues)
                            {
                                BusinessObjects.SearchWeightageAttribute attribute = new BusinessObjects.SearchWeightageAttribute(searchWeightageAttribute.Id, searchWeightageAttribute.LocaleId, weightageValue.Value, ValueTypeHelper.DecimalTryParse(weightageValue.Weightage, default(Decimal), LocaleEnum.en_US));
                                attributes.Add(attribute);
                            }
                        }
                    }

                    searchCriteria.SearchWeightageAttributes = attributes;
                }
            }
            finally
            {
                if (isTracingEnabled)
                {
                    MDMTraceHelper.StopTraceActivity();
                }
            }
        }

        #endregion

        #region Update Search Criteria Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchAttributeRule"></param>
        /// <param name="searchValues"></param>
        /// <param name="callerContext"></param>
        private Collection<String> UpdateSearchCriteria(SearchAttributeRule searchAttributeRule, Collection<String> searchValues, CallerContext callerContext)
        {
            Collection<Int32> attributeIds = new Collection<Int32>();
            Collection<LocaleEnum> locales = new Collection<LocaleEnum>();
            Collection<String> lookupRefIds = new Collection<string>();

            attributeIds.Add(searchAttributeRule.Attribute.Id);
            if (!locales.Contains(searchAttributeRule.Locale))
            {
                locales.Add(searchAttributeRule.Locale);
            }

            #region Find WSID By DisplayValue from lookup and update the searchCriteria with WSID

            DataModelService dataModelService = new DataModelService();

            Lookup lookup = (Lookup)dataModelService.GetAttributeLookupData(searchAttributeRule.Attribute.Id, searchAttributeRule.Locale, -1, callerContext);

            if (lookup != null)
            {
                foreach (String lookupDisplayValue in searchValues)
                {
                    Row row = (Row)lookup.GetRecordByDisplayFormat(lookupDisplayValue);

                    if (row != null)
                    {
                        lookupRefIds.Add(row["Id"].ToString());
                    }
                    else
                    {
                        lookupRefIds.Add(lookupDisplayValue);
                    }
                }
            }

            #endregion

            return lookupRefIds;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchVal"></param>
        /// <returns></returns>
        private Collection<String> ConstructSearchString(String searchVal)
        {
            Collection<String> splitValues = new Collection<String>();

            if (searchVal != null)
            {

                if (searchVal.Contains("\'"))
                {
                    searchVal = DenormalizeApostrophes(searchVal);
                    searchVal = searchVal.Replace("\'", String.Empty).Trim();
                }
                if (searchVal.IndexOf("(") > 0 && searchVal.IndexOf(")") > 0 && (searchVal.IndexOf(" OR ") > 0 || searchVal.IndexOf(" AND ") > 0))
                {
                    splitValues.Add(NormalizeApostrophes(searchVal));
                }
                else if (searchVal.IndexOf(" OR ") > 0)
                {
                    String[] splitValuesByOR = searchVal.Split(new String[] { "OR" }, StringSplitOptions.None);

                    Collection<String> cleansedSearchValues = new Collection<String>();

                    foreach (String value in splitValuesByOR)
                    {
                        String cleansedSearchValue = value.Trim();
                        if (value.EndsWith("\'"))
                        {
                            cleansedSearchValue = value.TrimEnd('\'');
                        }
                        if (value.StartsWith("\'"))
                        {
                            cleansedSearchValue = value.TrimStart('\'');
                        }
                        splitValues.Add(NormalizeApostrophes(cleansedSearchValue));
                    }
                }
                else
                {
                    splitValues.Add(NormalizeApostrophes(searchVal));
                }
            }

            return splitValues;
        }

        /// <summary>
        /// Replace char ' to "$apos;" when it use as apostrophe
        /// </summary>
        /// <param name="searchVal">String wich contain apostrophis</param>
        private String DenormalizeApostrophes(String searchVal)
        {
            String[] searchSubValues = searchVal.Split(new String[] { _orSeperator }, StringSplitOptions.RemoveEmptyEntries);
            for (Int32 index = 0; index < searchSubValues.Length; index++)
            {
                //finding apostrophes in string. Note: before and after apostrophes cannot be "space" or "(" or")" chars
                Regex t = new Regex(@"(?<before>[^\s\(])'(?<after>[^\s\)])");
                MatchCollection allMatches = t.Matches(searchSubValues[index]);
                foreach (Match match in allMatches)
                {
                    String before = match.Groups["before"].Value;
                    String after = match.Groups["after"].Value;
                    searchSubValues[index] = searchSubValues[index].Replace(match.Value, String.Concat(before, "&apos;", after));
                }
            }
            return searchSubValues.Length > 1 ? String.Join(_orSeperator, searchSubValues) : searchSubValues[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchVal"></param>
        /// <returns></returns>
        private String NormalizeApostrophes(String searchVal)
        {
            return searchVal.Replace("&apos;", "'");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <param name="isLookup"></param>
        /// <returns></returns>
        private String GenerateCommaSeparatedSearchValue(Collection<String> values, Boolean isLookup = false)
        {
            StringBuilder sb = new StringBuilder();

            Boolean isRuleLookup = false;
            String ruleLookupValue = String.Empty;

            //To find out if this is Add Rule lookup search. In this case, there will be only 1 value having comma separated WSIDs ('Id#1, Id#2').
            if (isLookup && values != null && values.Count == 1 && values[0].Contains(","))
            {
                isRuleLookup = true;
                ruleLookupValue = values[0];
            }

            if (!isRuleLookup)
            {
                foreach (String value in values)
                {
                    if (isLookup)
                    {
                        sb.Append("'");
                        sb.Append(value);
                        sb.Append("'");
                        sb.Append(",");
                    }
                    else
                    {
                        sb.Append(value);
                        sb.Append(",");
                    }
                }

                String commaSeparatedSearchValues = String.Empty;
                commaSeparatedSearchValues = sb.ToString();
                commaSeparatedSearchValues = commaSeparatedSearchValues.TrimEnd(',');
                return commaSeparatedSearchValues;
            }
            else
            {
                String[] commaSeparatedlookupWSIDs = (!String.IsNullOrWhiteSpace(ruleLookupValue)) ? ruleLookupValue.Split(',') : null;
                Collection<String> lookupWSIds = (commaSeparatedlookupWSIDs != null) ? new Collection<String>(commaSeparatedlookupWSIDs) : new Collection<String>();

                return (GenerateCommaSeparatedSearchValue(lookupWSIds, true));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchAttributeRule"></param>
        /// <param name="searchVal"></param>
        private void UpdateSearchAttributeRule(SearchAttributeRule searchAttributeRule, String searchVal)
        {
            searchAttributeRule.Attribute.CurrentValues.Clear();
            Value value = new Value();
            value.AttrVal = searchVal;
            searchAttributeRule.Attribute.CurrentValues.Add(value);
        }

        /// <summary>
        /// Updates the LookUp WSIDS for the given Display name in the search criteria
        /// </summary>
        /// <param name="searchCriteria">Search Criteria</param>
        /// <param name="attributeModelCollection">Attribute model collection</param>
        /// <param name="callerContext">Caller Context</param>
        private void UpdateSearchCriteriaForLookupWSIDs(SearchCriteria searchCriteria, AttributeModelCollection attributeModelCollection, CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
            {
                MDMTraceHelper.StartTraceActivity(MDMTraceSource.EntitySearch);
            }

            try
            {
                if (searchCriteria.SearchAttributeRules != null && searchCriteria.SearchAttributeRules.Count > 0)
                {
                    DataModelService dataModelService = new DataModelService();

                    if (attributeModelCollection != null && attributeModelCollection.Count > 0)
                    {
                        foreach (SearchAttributeRule attrRule in searchCriteria.SearchAttributeRules)
                        {
                            AttributeModel attributeModel = null;

                            if (attrRule.Attribute.Id != 0)
                            {
                                attributeModel = attributeModelCollection[attrRule.Attribute.Id, attrRule.Attribute.Locale];
                            }

                            if (attributeModel != null)
                            {
                                if (attributeModel.IsLookup == true)
                                {
                                    #region Find WSID By DisplayValue from lookup and update the searchCriteria with WSID

                                    Lookup lookup = (Lookup)dataModelService.GetAttributeLookupData(attrRule.Attribute.Id, attrRule.Locale, -1, callerContext);

                                    if (lookup != null)
                                    {
                                        Value val = (Value)attrRule.Attribute.GetCurrentValueInstance();

                                        if (val != null)
                                        {
                                            if (val.AttrVal != null)
                                            {
                                                Row row = (Row)lookup.GetRecordByDisplayFormat(val.AttrVal.ToString());

                                                if (row != null)
                                                {
                                                    val.AttrVal = row["Id"].ToString();
                                                    val.InvariantVal = row["Id"].ToString();
                                                }
                                            }
                                        }
                                    }

                                    #endregion
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                if (isTracingEnabled)
                {
                    MDMTraceHelper.StopTraceActivity();
                }
            }
        }

        #endregion

        #region Validation Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <param name="searchContext"></param>
        /// <param name="searchConfiguration"></param>
        /// <param name="callerContext"></param>
        /// <param name="validateSearchConfiguration"></param>
        private void ValidateSearchParameters(SearchCriteria searchCriteria, SearchContext searchContext, String searchConfiguration, CallerContext callerContext, DiagnosticActivity diagnosticActivity, Boolean validateSearchConfiguration = true)
        {
            Boolean isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;

            if (isTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            try
            {
                if (searchCriteria == null)
                    throw new ArgumentException("SearchCriteria");

                if (searchCriteria.Locales == null
                    || (searchCriteria.Locales != null && searchCriteria.Locales.Count <= 0)
                    || (searchCriteria.Locales != null && searchCriteria.Locales.Count > 0
                        && searchCriteria.Locales[0] == LocaleEnum.UnKnown))
                {
                    throw new MDMOperationException("111650", "Locale is not populated in SearchCriteria.", "EntitySearchManager", String.Empty, "SearchEntities");
                }

                if (searchContext == null)
                {
                    throw new MDMOperationException("111651", "SearchContext cannot be null.", "EntitySearchManager", String.Empty, "SearchEntities");
                }

                if (searchCriteria.ContainerIds == null || searchCriteria.ContainerIds.Count < 1)
                {
                    throw new MDMOperationException("111652", "Entities cannot be searched. Please provide Container Ids.", "EntitySearchManager", String.Empty, "SearchEntities");
                }

                if (validateSearchConfiguration && String.IsNullOrWhiteSpace(searchConfiguration))
                {
                    throw new MDMOperationException("111653", "SearchConfiguration cannot be null.", "EntitySearchManager", String.Empty, "SearchEntities");
                }
            }
            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }
        }

        #endregion

        #endregion Private Methods

        #endregion
    }
}