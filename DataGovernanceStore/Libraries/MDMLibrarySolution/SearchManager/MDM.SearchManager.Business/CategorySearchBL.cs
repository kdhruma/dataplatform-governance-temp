using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MDM.SearchManager.Business
{
    using AttributeModelManager.Business;
    using BusinessObjects;
    using ConfigurationManager.Business;
    using Core;
    using Core.Exceptions;
    using Data;
    using Interfaces;
    using MessageManager.Business;
    using PermissionManager.Business;
    using RS.MDM.Configuration;
    using RS.MDM.ConfigurationObjects;
    using RS.MDM.Events;
    using Services;
    using System.Data.SqlClient;
    using Utility;

    /// <summary>
    /// Category search manager
    /// </summary>
    public class CategorySearchBL : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// Field denoting locale Message
        /// </summary>
        private LocaleMessage _localeMessage = null;

        /// <summary>
        /// Field denoting localeMessageBL
        /// </summary>
        private LocaleMessageBL _localeMessageBL = null;

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

        #endregion

        #region Constructors

        public CategorySearchBL()
        {
            GetSecurityPrincipal();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting locale message BL class
        /// </summary>
        private LocaleMessageBL LocaleMessageBL
        {
            get
            {
                if (_localeMessageBL == null)
                {
                    _localeMessageBL = new LocaleMessageBL();

                }
                return _localeMessageBL;
            }
        }

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
        /// Search Categories for given search criteria and return list of categories with specified context. 
        /// </summary>
        /// <param name="searchCriteria">Provides search criteria.</param>
        /// <param name="searchContext">Provides search context. Example: SearchContext.MaxRecordCount indicates max records to be fetched while searching and AttributeIdList indicates List of attributes to load in returned categories.</param>
        /// <param name="callerContext">Provides search criteria.</param>
        /// <param name="searchOperationResult"></param>
        /// <param name="iEntityManager">Indicates IEntityManager</param>
        /// <param name="callerContext">Indicates in which context caller is calling the method</param>
        /// <returns>Search results - collection of entities</returns>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameter is null</exception>
        /// <exception cref="ArgumentException">Thrown when Category Ids field of Search Criteria is not been populated</exception>
        public EntityCollection SearchCategories(SearchCriteria searchCriteria, SearchContext searchContext, OperationResult searchOperationResult, IEntityManager iEntityManager, CallerContext callerContext)
        {
            EntityCollection entitySearchResult = new EntityCollection();

            try
            {
                //Start trace
                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("EntitySearchManager.CategorySearchBL.SearchCategories", MDMTraceSource.CategorySearch, true);

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Incoming Application is {0} and Module is {1}.", callerContext.Application, callerContext.Module), MDMTraceSource.CategorySearch);

                DurationHelper durationHelper = new DurationHelper(DateTime.Now);
                DurationHelper searchEntitiesDurationHelper = new DurationHelper(DateTime.Now);

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Starting EntitySearchManager.CategorySearchBL.GetSearchConfiguration", MDMTraceSource.CategorySearch);

                String searchConfigurationXml = GetSearchConfiguration(searchCriteria, callerContext);

                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - EntitySearchManager.CategorySearchBL.GetSearchConfiguration", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.CategorySearch);

                //get total count(Max Result Count)
                String totalCount = searchContext.MaxRecordsToReturn.ToString();

                #region Validation for Search Criteria , Search Context and Search Configuration

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Validation of search parameter started...", MDMTraceSource.CategorySearch);

                ValidateSearchParameters(searchCriteria, searchContext, searchConfigurationXml, callerContext);

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Validation of search parameter completed.", MDMTraceSource.CategorySearch);

                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Validation for Search Criteria,Search Context and Search Configuration", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.CategorySearch);

                #endregion

                #region Initial Setup

                //Flag to indicate if there are no errors in input and search can be executed.
                Boolean isSearchCriteriaValidated = true;

                SecurityPermissionDefinitionCollection securityPermissionDefinitions = null;

                //Get logged in User's details
                String currentUser = "system";
                Int32 roleId = 0;
                Int32 userId = 0;

                if (_securityPrincipal != null)
                {
                    currentUser = _securityPrincipal.CurrentUserName;
                    roleId = _securityPrincipal.UserPreferences.DefaultRoleId;
                    userId = _securityPrincipal.UserPreferences.LoginId;
                }

                //Get System Data Locale.
                LocaleEnum systemDataLocale = GlobalizationHelper.GetSystemDataLocale();

                //If requested locale details does not contain system Data Locale, add system data locale in list
                if (!searchCriteria.Locales.Contains(systemDataLocale))
                {
                    searchCriteria.Locales.Add(systemDataLocale);
                }

                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Search Initial Setup", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.CategorySearch);

                #endregion

                #region Calculate Security Permission Definition for AVS

                isSearchCriteriaValidated = GetAndVerifySecurityPermissions(searchCriteria, searchContext, searchOperationResult, systemDataLocale, out securityPermissionDefinitions, callerContext);

                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Calculate Security Permission Definition for AVS", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.CategorySearch);

                #endregion

                if (isSearchCriteriaValidated)
                {
                    AttributeModelCollection searchAttributeModelCollection = GetSearchAttributeModelCollection(searchContext.ReturnAttributeList, searchCriteria.SearchAttributeRules, searchCriteria.Locales[0]);

                    if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Search Attribute Model Get", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.CategorySearch);

                    #region De-format Search Attribute Values

                    //Locale Collection will have 2 locales.
                    //1st locale is User's current data locale
                    searchCriteria.SearchAttributeRules = GetDeformattedSearchAttributeValues(searchCriteria.SearchAttributeRules, searchCriteria.Locales[0], searchAttributeModelCollection, searchOperationResult, ref isSearchCriteriaValidated);

                    if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - De-format Search Attribute Values", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.CategorySearch);

                    #endregion

                    #region Inject Configured Data to Search Criteria

                    //injects any additional data like search filter or weightage specified in the Application configuration
                    InjectConfiguredDataToSearchCriteria(searchCriteria, roleId, userId);

                    if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Inject Configured Data to Search Criteria", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.CategorySearch);

                    #endregion

                    #region If search Criteria is having Lookup Attribute then populate WSID by DisplayValue

                    UpdateSearchCriteriaForLookupWSIDs(searchCriteria, searchAttributeModelCollection, callerContext);

                    if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Update LookupAttributeValue With WSID In SearchCriteria", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.CategorySearch);

                    #endregion

                    #region Load Search Result from Database
                    //create command
                    DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Search);

                    //Call DA mthod to fetch results
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Starting Load Category Search Result from database...", MDMTraceSource.CategorySearch);

                    CategorySearchDA categorySearchDA = new CategorySearchDA();
                    Collection<Int64> categoryIdList = categorySearchDA.SearchCategories(searchCriteria, searchContext, searchConfigurationXml, systemDataLocale, _securityPrincipal, command);

                    if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Load Category Search Result from Database", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.CategorySearch);

                    #endregion

                    #region Get Categories by using Entity Get

                    EntityContext entityContext = new EntityContext();

                    entityContext.LoadEntityProperties = true;
                    entityContext.LoadAttributes = true;
                    entityContext.DataLocales = searchCriteria.Locales;
                    entityContext.ContainerId = searchCriteria.ContainerIds.FirstOrDefault();
                    entityContext.EntityTypeId = 6;

                    if (searchContext.ReturnAttributeList != null && searchContext.ReturnAttributeList.Count > 0)
                    {
                        foreach (Attribute returnAttribute in searchContext.ReturnAttributeList)
                        {
                            entityContext.AttributeIdList.Add(returnAttribute.Id);
                        }
                    }

                    if (categoryIdList != null && categoryIdList.Count > 0)
                    {
                        entitySearchResult = iEntityManager.Get(categoryIdList, entityContext, callerContext.Application, callerContext.Module);
                    }

                    if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Total time taken to Get Category Entities for total {1} Categories", durationHelper.GetDurationInMilliseconds(DateTime.Now), categoryIdList.Count), MDMTraceSource.CategorySearch);

                    #endregion

                    #region Write Final Traces

                    if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Overall Time taken for Category Search ", searchEntitiesDurationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.CategorySearch);

                    #endregion
                }
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("Incorrect syntax near the keyword 'and'"))
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Invalid search criteria. Error: {0}", ex.Message), MDMTraceSource.CategorySearch);

                    //Add an info message..   
                    _localeMessage = LocaleMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "112633", false, callerContext); //Invalid search criteria

                    throw new MDMOperationException("112633", _localeMessage.Message, "CategorySearchBL", String.Empty, "SearchCategories");
                }
                else
                {
                    throw ex;
                }
            }
            finally
            {
                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("EntitySearchManager.CategorySearchBL.SearchCategories", MDMTraceSource.CategorySearch);
            }

            return entitySearchResult;
        }

        #endregion

        #region Private Methods

        #region Attribute Model Get Methods

        /// <summary>
        /// Gets the AttributeModel Collection for the given Search Criteria (gets Both return attributes and search attributes)
        /// </summary>
        /// <param name="returnAttributes">return attribute list</param>
        /// <param name="searchAttributeRules">search attribute list</param>
        /// <param name="currentDataLocale">current data locale</param>
        /// <returns></returns>
        private AttributeModelCollection GetSearchAttributeModelCollection(Collection<Attribute> returnAttributes, Collection<SearchAttributeRule> searchAttributeRules, LocaleEnum currentDataLocale)
        {
            AttributeModelCollection attributeModels = new AttributeModelCollection();
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

            }
            return attributeModels;
        }

        #endregion

        #region Deformatted Methods

        /// <summary>
        /// Deformats input in Search Attributes
        /// </summary>
        /// <param name="searchAttributeRules"></param>
        /// <param name="currentDataLocale"></param>
        /// <param name="attributeModelCollection"></param>
        /// <returns></returns>
        Collection<SearchAttributeRule> GetDeformattedSearchAttributeValues(Collection<SearchAttributeRule> searchAttributeRules, LocaleEnum currentDataLocale, AttributeModelCollection attributeModelCollection, OperationResult operationResult, ref Boolean getSearchResults)
        {
            Collection<SearchAttributeRule> deformattedSearchAttributeRules = new Collection<SearchAttributeRule>();

            if (searchAttributeRules != null && searchAttributeRules.Count > 0)
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "De-formatting of Search Attribute Values started...", MDMTraceSource.CategorySearch);

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

                    AttributeModel attributeModel = new AttributeModel();

                    //In case of keyword search, attributeId is 0. AttributeModelCollection does not hold a model for this Id. 
                    //Hence not getting attributemodel from attributemodelcollection.
                    if (searchAttributeRule.Attribute.Id > 0)
                    {
                        attributeModel = attributeModelCollection[searchAttributeRule.Attribute.Id, currentDataLocale];
                    }

                    AttributeDataType attributeDataType = AttributeDataType.Unknown;
                    Enum.TryParse(attributeModel.AttributeDataTypeName, out attributeDataType);

                    if (attributeDataType.Equals(AttributeDataType.String))
                    {
                        //In case of search for multiple values, the operator should be "In" and should not be "Equal".
                        if (searchVal != null && searchVal.Contains("' OR '"))
                            searchAttributeRule.Operator = SearchOperator.In;

                        AttributeDisplayType attributeDisplayType = AttributeDisplayType.Unknown;
                        Enum.TryParse(attributeModel.AttributeDisplayTypeName, out attributeDisplayType);

                        if (attributeDisplayType != AttributeDisplayType.LookupTable)
                        {
                            //Before populating search attribute rule, "OR" should be replaced with comma(,).
                            searchVal = searchVal == null ? "" : searchVal.Replace("' OR '", "','");
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
                        searchVal = GenerateCommaSeparatedSearchValue(searchValues);
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

                    ResolveSpecialKeywords(searchAttributeRule);

                    //deformatted search value
                    Collection<String> deformattedSearchValues = new Collection<string>();

                    if (attributeModel.AttributeDataTypeName.ToLowerInvariant().Equals("fraction"))
                    {
                        //deformat
                        isFormattedSearchAttributeRule = true;
                        deformattedSearchValues = DeformatFraction(searchAttributeRule.Attribute.GetCurrentValues(), currentDataLocale, attributeModel.LongName, operationResult, ref getSearchResults);
                        searchVal = GenerateCommaSeparatedSearchValue(deformattedSearchValues);
                    }
                    else if (attributeModel.AttributeDataTypeName.ToLowerInvariant().Equals("integer"))
                    {
                        isFormattedSearchAttributeRule = true;
                        deformattedSearchValues = DeformatInteger(searchAttributeRule, attributeModel.LongName, operationResult, ref getSearchResults);
                        searchVal = GenerateCommaSeparatedSearchValue(deformattedSearchValues);
                    }
                    //deformat input in case of date
                    else if (attributeModel.AttributeDataTypeName.ToLowerInvariant().Equals("date"))
                    {
                        isFormattedSearchAttributeRule = true;
                        deformattedSearchValues = DeformatDate(searchValues, currentDataLocale, attributeModel.LongName, operationResult, ref getSearchResults);
                        searchVal = GenerateCommaSeparatedSearchValue(deformattedSearchValues);
                    }
                    //deformat input in case of date time
                    else if (attributeModel.AttributeDataTypeName.ToLowerInvariant().Equals("datetime"))
                    {
                        isFormattedSearchAttributeRule = true;
                        deformattedSearchValues = GetDeformattedDateTimeValues(searchValues, currentDataLocale, attributeModel.LongName, operationResult, ref getSearchResults);
                        searchVal = GenerateCommaSeparatedSearchValue(deformattedSearchValues);
                    }
                    //deformat input of decimal
                    else if (attributeModel.AttributeDataTypeName.ToLowerInvariant().Equals("decimal"))
                    {
                        isFormattedSearchAttributeRule = true;
                        deformattedSearchValues = DeformatDecimal(searchValues, currentDataLocale, attributeModel.LongName, operationResult, ref getSearchResults, attributeModel.Precision);
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
                }

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "De-formatting of Search Attribute Values completed.", MDMTraceSource.CategorySearch);
            }

            return deformattedSearchAttributeRules;
        }

        /// <summary>
        /// Fetches deformatted value in case of date and date time
        /// </summary>
        /// <param name="searchAttributeRule"></param>
        /// <param name="currentDataLocale"></param>
        /// <param name="attributeDataType"></param>
        /// <returns></returns>
        private Collection<String> GetDeformattedDateTimeValues(Collection<String> searchValues, LocaleEnum currentDataLocale, String attributeName, OperationResult searchOperationResult, ref Boolean getSearchResults)
        {
            Collection<String> deformattedSearchValues = new Collection<string>();

            //for each date time value, get deformatetd value
            foreach (String formattedValue in searchValues)
            {
                String deformattedValue = String.Empty;

                if (!String.IsNullOrWhiteSpace(formattedValue))
                {
                    //get deformatted value
                    deformattedValue = DeformatDateTime(formattedValue, currentDataLocale, attributeName, searchOperationResult, ref getSearchResults);
                    deformattedSearchValues.Add(deformattedValue);
                }
            }

            return deformattedSearchValues;
        }

        /// <summary>
        /// De-format date and time
        /// </summary>
        /// <param name="formattedValue"></param>
        /// <param name="currentDataLocale"></param>
        /// <param name="attributeName"></param>
        /// <param name="searchOperationResult"></param>
        /// <param name="getSearchResults"></param>
        /// <returns></returns>
        private String DeformatDateTime(String formattedValue, LocaleEnum currentDataLocale, String attributeName, OperationResult searchOperationResult, ref Boolean getSearchResults)
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
                    if (!String.IsNullOrWhiteSpace(this.CurrentTimeZone))
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
        /// <param name="searchAttributeRule"></param>
        /// <param name="currentDataLocale"></param>
        /// <param name="attributeName"></param>
        /// <param name="searchOperationResult"></param>
        /// <param name="getSearchResults"></param>
        /// <returns></returns>
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
        /// Deformat Date
        /// </summary>
        /// <param name="searchAttributeRule"></param>
        /// <param name="currentDataLocale"></param>
        /// <param name="attributeName"></param>
        /// <param name="searchOperationResult"></param>
        /// <param name="getSearchResults"></param>
        /// <returns></returns>
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
        /// Deformat Decimal
        /// </summary>
        /// <param name="searchAttributeRule"></param>
        /// <param name="currentDataLocale"></param>
        /// <param name="attributeName"></param>
        /// <param name="searchOperationResult"></param>
        /// <param name="getSearchResults"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        private Collection<String> DeformatDecimal(Collection<String> searchValues, LocaleEnum currentDataLocale, String attributeName, OperationResult searchOperationResult, ref Boolean getSearchResults, Int32 precision)
        {

            Collection<String> deformattedSearchValues = new Collection<string>();
            String deformattedSearchValue = String.Empty;

            try
            {
                foreach (String value in searchValues)
                {
                    Double deformatedValue = MDM.Core.FormatHelper.DeformatNumber(value, currentDataLocale.GetCultureName());
                    deformattedSearchValue = MDM.Core.FormatHelper.FormatNumber(deformatedValue, LocaleEnum.en_US.GetCultureName(), precision, false);
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

        #endregion

        #region Misc Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchAttributeRule"></param>
        /// <param name="callerContext"></param>
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
        private String GetSearchConfiguration(SearchCriteria searchCriteria, CallerContext callerContext)
        {
            String searchConfigurationXml = String.Empty;

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

            //return SearchConfiguration.
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
        /// <param name="callerContext">This parameter is specifying caller context.</param>
        /// <returns>returns true if application able to determine attribute value based permission otherwise false.</returns>
        private Boolean DetermineAttributeValueBasedPermission(SearchCriteria searchCriteria, SearchContext searchContext, OperationResult operationResult, LocaleEnum systemDataLocale, SecurityPermissionDefinitionCollection securityPermissionDefinitions, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Determining Attribute Value Based Permission..", MDMTraceSource.CategorySearch);

            Boolean hasPermission = false;

            SecurityPermissionDefinitionBL securityPermissionDefinitionBL = new SecurityPermissionDefinitionBL();

            if (securityPermissionDefinitions != null && securityPermissionDefinitions.Count > 0)
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Security Permission definition is not empty.", MDMTraceSource.CategorySearch);

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

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("AppConfig MDMCenter.Workflow.WorkflowPanel.IncludeViewOnlyEntities: {0}", strIncludeViewOnlyEntities), MDMTraceSource.CategorySearch);

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
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Only view permission is there for entities with Security Permission Definition : {0} and request is for workflow search. Ignoring this definition considering the value of AppConfig 'MDMCenter.Workflow.WorkflowPanel.IncludeViewOnlyEntities'", securityPermissionDefinition.Name), MDMTraceSource.CategorySearch);
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
                _localeMessage = LocaleMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111848", false, callerContext); //View/Edit permissions are not available for the requested search criteria results.

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

                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Permissions are not available for the context Org: {0}, Catalog: {1} and User: {2}. Cannot return search results.", searchCriteria.OrganizationId, strContainerIds, _securityPrincipal.CurrentUserName), MDMTraceSource.CategorySearch);

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
        private Boolean GetAndVerifySecurityPermissions(SearchCriteria searchCriteria, SearchContext searchContext, OperationResult searchOperationResult, LocaleEnum systemDataLocale, out SecurityPermissionDefinitionCollection securityPermissionDefinitions, CallerContext callerContext)
        {
            SecurityPermissionDefinitionBL securityPermissionDefinitionBL = new SecurityPermissionDefinitionBL();
            BusinessObjects.ApplicationContext applicationContext = new BusinessObjects.ApplicationContext();

            applicationContext.OrganizationId = searchCriteria.OrganizationId;
            applicationContext.ContainerId = searchCriteria.ContainerIds.FirstOrDefault();
            applicationContext.UserId = _securityPrincipal.CurrentUserId;
            applicationContext.UserName = _securityPrincipal.CurrentUserName;

            securityPermissionDefinitions = securityPermissionDefinitionBL.Get(applicationContext, callerContext);

            //Load Attribute Value based security search criteria
            return DetermineAttributeValueBasedPermission(searchCriteria, searchContext, searchOperationResult, systemDataLocale, securityPermissionDefinitions, callerContext);
        }

        #endregion

        #region Injection Methods for Search Criteria

        /// <summary>
        /// Injects any additinal data to be added in the search criteria specified in the Application Config
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <param name="roleId"></param>
        /// <param name="userId"></param>
        private void InjectConfiguredDataToSearchCriteria(SearchCriteria searchCriteria, Int32 roleId, Int32 userId)
        {
            DurationHelper durationHelper = new DurationHelper(DateTime.Now);
            Int32 containerId = 0;
            Int64 categoryId = 0;
            Int32 entityTypeId = 0;
            if (searchCriteria != null)
            {
                if (searchCriteria.ContainerIds != null && searchCriteria.ContainerIds.Count == 1)
                    containerId = searchCriteria.ContainerIds.FirstOrDefault();
                if (searchCriteria.CategoryIds != null && searchCriteria.CategoryIds.Count == 1)
                    categoryId = searchCriteria.CategoryIds.FirstOrDefault();
                if (searchCriteria.EntityTypeIds != null && searchCriteria.EntityTypeIds.Count == 1)
                    entityTypeId = searchCriteria.EntityTypeIds.FirstOrDefault();
            }

            ApplicationConfiguration applicationConfiguration = new ApplicationConfiguration((Int32)EventSource.EntityExplorer, (Int32)EventSubscriber.SearchManager, roleId, userId, searchCriteria.OrganizationId, containerId, categoryId, 0, 0, entityTypeId, 0, 0, 0);
            applicationConfiguration.GetConfigurations();

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Get Application Configuration-Method:InjectConfiguredDataToSearchCriteria", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.CategorySearch);

            InjectionSearchCriteria injectionSearchCriteria = (InjectionSearchCriteria)applicationConfiguration.GetObject("SearchCriteria");
            if (injectionSearchCriteria != null)
                InjectConfiguredFilterToSearchCriteria(injectionSearchCriteria, searchCriteria);

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - InjectConfiguredFilterToSearchCriteria-Method:InjectConfiguredDataToSearchCriteria", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.CategorySearch);

            SearchWeightage searchWeightageAttributes = (SearchWeightage)applicationConfiguration.GetObject("SearchWeightage");
            if (searchWeightageAttributes != null)
                InjectConfiguredWeightageToSearchCriteria(searchWeightageAttributes, searchCriteria);

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - InjectConfiguredWeightageToSearchCriteria-Method:InjectConfiguredDataToSearchCriteria", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.CategorySearch);

            WorkflowSearchConfiguration workflowSearchConfiguration = (WorkflowSearchConfiguration)applicationConfiguration.GetObject("SearchWorkflow");
            if (workflowSearchConfiguration != null)
                InjectConfiguredWorkflowDetailsToSearchCriteria(workflowSearchConfiguration, searchCriteria);

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - InjectConfiguredWorkflowDetailsToSearchCriteria-Method:InjectConfiguredDataToSearchCriteria", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.CategorySearch);

        }

        /// <summary>
        /// Injects the additional search filter specified in the Application Configuration
        /// </summary>
        /// <param name="injectionSearchCriteria"></param>
        /// <param name="searchCriteria"></param>
        private void InjectConfiguredFilterToSearchCriteria(InjectionSearchCriteria injectionSearchCriteria, SearchCriteria searchCriteria)
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

        /// <summary>
        /// Injects configured Weigtage specifed for partiular attribute in application config
        /// </summary>
        /// <param name="searchWeightageAttributes"></param>
        /// <param name="searchCriteria"></param>
        private void InjectConfiguredWeightageToSearchCriteria(SearchWeightage searchWeightageAttributes, SearchCriteria searchCriteria)
        {
            Collection<MDM.BusinessObjects.SearchWeightageAttribute> attributes = new Collection<MDM.BusinessObjects.SearchWeightageAttribute>();
            if (searchWeightageAttributes != null && searchWeightageAttributes.SearchWeightageAttributeCollection.Count > 0)
            {
                foreach (RS.MDM.ConfigurationObjects.SearchWeightageAttribute searchWeightageAttribute in searchWeightageAttributes.SearchWeightageAttributeCollection)
                {
                    if (searchWeightageAttribute.WeightageValues.Count > 0)
                    {
                        foreach (WeightageValue weightageValue in searchWeightageAttribute.WeightageValues)
                        {
                            MDM.BusinessObjects.SearchWeightageAttribute attribute = new MDM.BusinessObjects.SearchWeightageAttribute(searchWeightageAttribute.Id, searchWeightageAttribute.LocaleId, weightageValue.Value, ValueTypeHelper.DecimalTryParse(weightageValue.Weightage, default(Decimal), LocaleEnum.en_US));
                            attributes.Add(attribute);
                        }
                    }
                }

                searchCriteria.SearchWeightageAttributes = attributes;
            }
        }

        /// <summary>
        /// Injects the configured workflowname and enable/disable workflow result in Grid
        /// </summary>
        /// <param name="workflowSearchConfiguration"></param>
        /// <param name="searchCriteria"></param>
        private void InjectConfiguredWorkflowDetailsToSearchCriteria(WorkflowSearchConfiguration workflowSearchConfiguration, SearchCriteria searchCriteria)
        {
            searchCriteria.ConfiguredWorkflowForSearch = workflowSearchConfiguration.WorkflowName;
            searchCriteria.ReturnWorkflowResult = workflowSearchConfiguration.ReturnWorkflowResult;
        }

        #endregion

        #region Updation Search Criteria Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchCriteria"></param>
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
        /// It will construct the search string based on given search value
        /// </summary>
        /// <param name="searchVal">Input for construct search string</param>
        /// <returns>Collection of search string</returns>
        private Collection<String> ConstructSearchString(String searchVal)
        {
            Collection<String> splitValues = new Collection<string>();
            searchVal = searchVal.Replace("\'", String.Empty);

            if (searchVal.IndexOf("(") > 0 && searchVal.IndexOf(")") > 0 && (searchVal.IndexOf(" OR ") > 0 || searchVal.IndexOf(" AND ") > 0))
            {
                splitValues.Add(searchVal);
                return splitValues;

            }
            else if (searchVal.IndexOf(" OR ") > 0)
            {
                string[] splitValuesByOR = searchVal.Split(new string[] { "OR" }, StringSplitOptions.None);
                splitValues = new Collection<string>();

                Collection<String> cleansedSearchValues = new Collection<string>();

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
                    splitValues.Add(cleansedSearchValue);
                }
                return splitValues;

            }
            else
            {
                splitValues.Add(searchVal);
                return splitValues;
            }
        }

        /// <summary>
        /// It will return comma separated string
        /// </summary>
        /// <param name="values">Collection of string which needs to search</param>
        /// <param name="isLookup">Indicated given value is lookup or not</param>
        /// <returns>returns comma separated string from the collection of string</returns>
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
        /// It will update the search attribute rule
        /// </summary>
        /// <param name="searchAttributeRule">Attribute rule which needs to update</param>
        /// <param name="searchVal">search value</param>
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

        #endregion

        #region Validation Methods

        private void ValidateSearchParameters(SearchCriteria searchCriteria, SearchContext searchContext, String searchConfiguration, CallerContext callerContext)
        {
            if (searchCriteria == null)
                throw new ArgumentException("SearchCriteria");

            if (searchCriteria.Locales == null
                || (searchCriteria.Locales != null && searchCriteria.Locales.Count <= 0)
                || (searchCriteria.Locales != null && searchCriteria.Locales.Count > 0
                    && searchCriteria.Locales[0] == LocaleEnum.UnKnown))
            {
                throw new MDMOperationException("111650", "Locale is not populated in SearchCriteria.", "EntitySearchManager", String.Empty, "SearchCategories");
            }

            if (searchContext == null)
            {
                throw new MDMOperationException("111651", "SearchContext cannot be null.", "EntitySearchManager", String.Empty, "SearchCategories");
            }

            if (searchCriteria.ContainerIds == null || searchCriteria.ContainerIds.Count < 1)
            {
                throw new MDMOperationException("111652", "Entities cannot be searched. Please provide Container Ids.", "EntitySearchManager", String.Empty, "SearchCategories");
            }

            if (String.IsNullOrWhiteSpace(searchConfiguration))
            {
                throw new MDMOperationException("111653", "SearchConfiguration cannot be null.", "EntitySearchManager", String.Empty, "SearchCategories");
            }
        }

        #endregion

        #endregion

        #endregion
    }
}
