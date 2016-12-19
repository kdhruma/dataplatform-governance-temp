using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MDM.LookupManager.Business
{
    using MDM.AttributeDependencyManager.Business;
    using MDM.AttributeModelManager.Business;
    using MDM.BufferManager;
    using MDM.BusinessObjects;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.IntegrationManager.Business;
    using MDM.Interfaces;
    using MDM.KnowledgeManager.Business;
    using MDM.LookupManager.Data;
    using MDM.MessageManager.Business;
    using MDM.Utility;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.InstrumentationManager.Utility;
    using MDM.ActivityLogManager.Business;
    using MDM.AdminManager.Business;

    /// <summary>
    /// Specifies business logic operations for Lookup
    /// </summary>
    public class LookupBL : BusinessLogicBase, ILookupManager
    {
        #region Fields

        // lock object for the load all..singleton
        // this is kept as static to ensure only 
        private static Object lockObj = new Object();

        /// <summary>
        /// Specifies security principal for user.
        /// </summary>
        private SecurityPrincipal _securityPrincipal = null;

        /// <summary>
        /// Lookup data access object
        /// </summary>
        private LookupDA _lookupDA = new LookupDA();

        /// <summary>
        /// Specifies lookup buffer manager
        /// </summary>
        private LookupBufferManager _lookupBufferManager = new LookupBufferManager();

        /// <summary>
        /// 
        /// </summary>
        private OperationContext _operationContext = OperationContext.Current;

        /// <summary>
        /// Field denoting the thread safe dictionary which contains the lookup names as lock key
        /// </summary>
        private static ConcurrentDictionary<String, Object> _lookupLockKeys = new ConcurrentDictionary<String, Object>();

        /// <summary>
        /// Indicates current trace settings
        /// </summary>
        private TraceSettings _currentTraceSettings = null;

        /// <summary>
        /// Indicates the seperator for unique values in a row
        /// </summary>
        private const String _uniqueValueSeperator = "#@#";

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiate Lookup BL
        /// </summary>
        public LookupBL()
        {
            _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            GetSecurityPrincipal();
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Loads all lookup data
        /// </summary>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Boolean flag which says whether the load is success or not</returns>
        public Boolean Load(CallerContext callerContext)
        {
            Boolean isLoadSuccesful = true;
            Collection<String> lookupNames = null;

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("LookupManager.Load", MDMTraceSource.LookupGet, false);

            try
            {
                #region Get defined Lookup Table Names in the system

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Loading lookup table name list...", MDMTraceSource.LookupGet);

                LookupContext lookupContext = new LookupContext();
                lookupContext.TableFilterType = LookupTableFilterType.All;
                lookupNames = this.GetAllLookupTableNamesFromDB(callerContext, lookupContext);

                if (lookupNames != null && lookupNames.Count <= 0)
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Warning, "No lookup tables found in the system. Load method ended with false result.");
                    return false;
                }

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Loaded lookup table name list. Total lookups found are:{0}", lookupNames.Count));

                #endregion

                #region Get Available Locales and Construct locale dictionary

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Loading locale list...", MDMTraceSource.LookupGet);

                Dictionary<LocaleEnum, Boolean> localeDictionary = new Dictionary<LocaleEnum, Boolean>();

                //Get system data locale
                LocaleEnum systemDataLocale = MDM.Utility.GlobalizationHelper.GetSystemDataLocale();

                LocaleBL localeManager = new LocaleBL();
                LocaleCollection availableLocales = localeManager.GetAvailableLocales();

                if (availableLocales == null || availableLocales.Count < 1)
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "No locales found. Load method ended with false result.", MDMTraceSource.LookupGet);
                    return false;
                }
                else
                {
                    StringBuilder localeList = new StringBuilder();

                    Boolean isSystemDataLocaleAdded = false;

                    //Construct locales dictionary
                    foreach (Locale locale in availableLocales)
                    {
                        LocaleEnum localeEnum = (LocaleEnum)locale.Id;

                        if (localeEnum == systemDataLocale)
                        {
                            localeDictionary.Add(localeEnum, true);
                            isSystemDataLocaleAdded = true;
                        }
                        else
                            localeDictionary.Add(localeEnum, false);

                        localeList.Append(locale.Locale.ToString());
                        localeList.Append(", ");
                    }

                    if (!isSystemDataLocaleAdded)
                        localeDictionary.Add(systemDataLocale, true);

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "List of locales to load:" + localeList, MDMTraceSource.LookupGet);
                }

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Loaded locale list. Total locales found are:" + localeDictionary.Count, MDMTraceSource.LookupGet);

                #endregion

                #region Load Lookup Data

                foreach (String lookupName in lookupNames)
                {
                   this.GetLookupDataFromDB(lookupName, localeDictionary, systemDataLocale, callerContext);
                }

                #endregion
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("LookupManager.Load", MDMTraceSource.LookupGet);
            }

            return isLoadSuccesful;
        }

        /// <summary>
        /// Gets model of the requested lookup table 
        /// </summary>
        /// <param name="lookupTableName">Name of the lookup table</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Returns lookup model</returns>
        /// <exception cref="MDMOperationException">Thrown when lookupTableName is not available</exception>
        public Lookup GetModel(String lookupTableName, CallerContext callerContext)
        {
            Lookup lookup = null;
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("LookupManager.GetModel", MDMTraceSource.LookupGet, false);

            try
            {
                //Note: Lookup model is nothing but the data saved for system data locale

                //Get system data locale
                LocaleEnum systemDataLocale = MDM.Utility.GlobalizationHelper.GetSystemDataLocale();

                lookup = Get(lookupTableName, systemDataLocale, -1, false, callerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("LookupManager.GetModel", MDMTraceSource.LookupGet);
            }

            return lookup;
        }

        /// <summary>
        /// Gets the lookup schema based on requested lookup names.
        /// </summary>
        /// <param name="lookupNames">Indicates list of lookup names</param>
        /// <param name="callerContext">Indicates application and module name by which operation is being performed</param>
        /// <returns>Returns the list of lookup schema</returns>
        public LookupCollection GetLookupSchema(Collection<String> lookupNames, CallerContext callerContext)
        {
            LookupCollection lookups = null;

            if (lookupNames != null && lookupNames.Count > 0)
            {
                lookups = new LookupCollection();

                foreach (String lookupName in lookupNames)
                {
                    Lookup lookup = this.GetLookupSchema(lookupName, callerContext);
                    if (lookup != null)
                    {
                        lookups.Add(lookup);
                    }
                }
            }

            return lookups;
        }

        /// <summary>
        /// Gets the lookup schema based requested lookup name.
        /// </summary>
        /// <param name="lookupName">Indicates the lookup name</param>
        /// <param name="callerContext">Indicates application and module name by which operation is being performed</param>
        /// <returns>Returns the lookup schema</returns>
        public Lookup GetLookupSchema(String lookupName, CallerContext callerContext)
        {
            Lookup lookup = this.GetModel(lookupName, callerContext);

            if (lookup != null)
            {
                lookup = lookup.CopyStructure();
            }
            return lookup;
        }

        /// <summary>
        /// Gets model of the requested lookup table 
        /// </summary>
        /// <param name="lookupTableName">Name of the lookup table</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <param name="getLatest">Indicates to get the Latest from DB</param>
        /// <returns>Returns lookup model</returns>
        /// <exception cref="MDMOperationException">Thrown when lookupTableName is not available</exception>
        public Lookup GetModel(String lookupTableName, CallerContext callerContext, Boolean getLatest)
        {
            Lookup lookup = null;
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("LookupManager.GetModel", MDMTraceSource.LookupGet, false);

            try
            {
                //Note: Lookup model is nothing but the data saved for system data locale

                //Get system data locale
                LocaleEnum systemDataLocale = MDM.Utility.GlobalizationHelper.GetSystemDataLocale();

                lookup = Get(lookupTableName, systemDataLocale, -1, getLatest, callerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("LookupManager.GetModel", MDMTraceSource.LookupGet);
            }

            return lookup;
        }

        /// <summary>
        /// Gets the lookup row collection for the requested lookup table, locale and based on the search rule collection.
        /// </summary>
        /// <param name="lookupTableName">Name of the lookup table</param>
        /// <param name="locale">Locale for which data needs to be fetched</param>
        /// <param name="searchRuleCollection">Specifies the search rules for filtering the lookup row collection</param>
        /// <param name="maxRecordsToReturn">Max number of lookup records to return. Setting '-1' returns all record</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>The lookup row collection</returns>
        public RowCollection GetLookupRows(String lookupTableName, LocaleEnum locale, LookupSearchRuleCollection searchRuleCollection, Int32 maxRecordsToReturn, CallerContext callerContext)
        {
            RowCollection rowCollection = null;

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("LookupManager.GetLookupRows", MDMTraceSource.LookupGet, false);

            try
            {
                Lookup lookup = Get(lookupTableName, locale, -1, false, callerContext);
                if (lookup != null)
                {
                    if (searchRuleCollection != null && searchRuleCollection.Count > 0)
                        rowCollection = (RowCollection)lookup.Filter(searchRuleCollection);
                    else
                        rowCollection = (RowCollection)lookup.GetRows();

                     if (maxRecordsToReturn > 0 && rowCollection != null && rowCollection.Count > maxRecordsToReturn)
                         rowCollection = new RowCollection(rowCollection.Take(maxRecordsToReturn).ToList());
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("LookupManager.GetLookupRows", MDMTraceSource.LookupGet);
            }

            return rowCollection;
        }

        /// <summary>
        /// Gets lookup data for the requested lookup table and locale
        /// </summary>
        /// <param name="lookupTableName">Name of the lookup table</param>
        /// <param name="locale">Locale for which data needs to be fetched</param>
        /// <param name="maxRecordsToReturn">Max number of lookup records to return. Setting '-1' returns all record</param>
        /// <param name="getLatest">Boolean flag which says whether to get from DB or cache. True means always get from DB</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Lookup data</returns>
        /// <exception cref="MDMOperationException">Thrown when lookupTableName or locale is not available</exception>
        public Lookup Get(String lookupTableName, LocaleEnum locale, Int32 maxRecordsToReturn, Boolean getLatest, CallerContext callerContext)
        {
            Lookup lookup = null;
            Lookup filteredLookup = null;

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("LookupManager.Get", MDMTraceSource.LookupGet, false);

            try
            {
                #region Validation

                if (String.IsNullOrWhiteSpace(lookupTableName))
                {
                    //Get localized message for system default UI locale
                    LocaleMessageBL localeMessageBL = new LocaleMessageBL();
                    LocaleMessage localeMessage = localeMessageBL.Get(MDM.Utility.GlobalizationHelper.GetSystemUILocale(), "111389", false, callerContext);
                    throw new MDMOperationException("111389", localeMessage.Message, "LookupManager", String.Empty, "Get");
                }

                if (locale == LocaleEnum.Neutral || locale == LocaleEnum.UnKnown)
                {
                    throw new MDMOperationException("111390", "Failed to get lookup data. Locale is not available.", "LookupManager", String.Empty, "Get");
                }

                #endregion

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested lookup table:{0}, locale:{1}, maxRecordsToReturn:{2}, getLatest:{3}, Application:{4} and Service:{5}", lookupTableName, locale.ToString(), maxRecordsToReturn, getLatest, callerContext.Application, callerContext.Module), MDMTraceSource.LookupGet);

                if (!getLatest)
                {
                    #region Get lookup from Cache if available

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Finding lookup table[name:{0}, locale:{1}] in cache...", lookupTableName, locale.ToString()), MDMTraceSource.LookupGet);

                    lookup = _lookupBufferManager.FindLookup(lookupTableName, locale);

                    #endregion
                }

                //Get lookup from DB if not available in cache or requested for the latest value..
                if (lookup == null)
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("No lookup table cache found for table:{0} and locale:{1}. Now Lookup table would be loaded from database.", lookupTableName, locale.ToString()), MDMTraceSource.LookupGet);

                    #region Get lookupdata from DB

                    lookup = GetLookupDataFromDB(lookupTableName, locale, callerContext);

                    #endregion
                }
                else
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Found lookup table[name:{0}, locale:{1}] in cache", lookupTableName, locale.ToString()), MDMTraceSource.LookupGet);
                }

                if (lookup != null)
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Done with lookup table[name:{0} and locale:{1}] load. Total records in table are:{2}", lookupTableName, locale.ToString(), lookup.Rows.Count), MDMTraceSource.LookupGet);

                    #region Filter lookup data

                    if (maxRecordsToReturn > 0 && lookup.Rows != null && lookup.Rows.Count > maxRecordsToReturn)
                    {
                        if (Constants.TRACING_ENABLED)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Filtering lookup table[name:{0} and locale:{1}] for MaxRecordsToReturn:{2}...", lookupTableName, locale.ToString(), maxRecordsToReturn), MDMTraceSource.LookupGet);

                        filteredLookup = lookup.CopyStructure();
                        filteredLookup.Rows = new RowCollection(lookup.Rows.Take(maxRecordsToReturn).ToList());

                        if (Constants.TRACING_ENABLED)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Done with filtering of lookup table[name:{0} and locale:{1}] for MaxRecordsToReturn:{2}", lookupTableName, locale.ToString(), maxRecordsToReturn), MDMTraceSource.LookupGet);
                    }
                    else
                    {
                        filteredLookup = lookup;
                    }

                    #endregion

                    #region Calculate Permissions

                    if (lookupTableName == Constants.LOOKUP_SECURITY_TABLE_NAME)
                    {
                        if (_securityPrincipal.IsInRole(Constants.SYSTEM_ADMIN_USER_ROLE))
                        {
                            filteredLookup.PermissionSet = new Collection<UserAction>() { UserAction.All };
                }
                        else
                        {
                            filteredLookup.PermissionSet = new Collection<UserAction>() { UserAction.None };
            }
                    }
                    else
                    {
                        if (IsLookupSecurityFeatureEnabled())
                        {
                            filteredLookup.PermissionSet = PopulateLookupPermissionSet(lookupTableName, locale, callerContext);
                        }
                    }

                    #endregion
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("LookupManager.Get", MDMTraceSource.LookupGet);
            }

            return filteredLookup;
        }

        /// <summary>
        /// Gets lookup data for the requested attribute and locale
        /// </summary>
        /// <param name="attributeId">Id of the attribute for which data needs to be get</param>
        /// <param name="locale">Locale for which data needs to be fetched</param>
        /// <param name="maxRecordsToReturn">Max number of lookup records to return. Setting '-1' returns all record</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Lookup data</returns>
        public Lookup Get(Int32 attributeId, LocaleEnum locale, Int32 maxRecordsToReturn, CallerContext callerContext)
        {
            return this.Get(attributeId, locale, maxRecordsToReturn, new ApplicationContext(), callerContext, false);
        }

        /// <summary>
        /// Gets lookup data for the requested attribute and locale
        /// </summary>
        /// <param name="attributeId">Id of the attribute for which data needs to be get</param>
        /// <param name="locale">Locale for which data needs to be fetched</param>
        /// <param name="maxRecordsToReturn">Max number of lookup records to return. Setting '-1' returns all record</param>
        /// <param name="applicationContext">Application context</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <param name="publishEvent"></param>
        /// <returns>Lookup data</returns>
        public Lookup Get(Int32 attributeId, LocaleEnum locale, Int32 maxRecordsToReturn, ApplicationContext applicationContext, CallerContext callerContext, Boolean publishEvent)
        {
            return Get(attributeId, locale, maxRecordsToReturn, null, applicationContext, callerContext, publishEvent);
        }

        /// <summary>
        /// Gets lookup data for the requested attribute and locale
        /// </summary>
        /// <param name="attributeId">Id of the attribute for which data needs to be get</param>
        /// <param name="locale">Locale for which data needs to be fetched</param>
        /// <param name="maxRecordsToReturn">Max number of lookup records to return. Setting '-1' returns all record</param>
        /// <param name="lookupValueIdList">List of lookup PK Ids to be returned along with requested max number of records to return.</param>
        /// <param name="applicationContext">Application context</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <param name="publishEvent"></param>
        /// <returns>Lookup data</returns>
        public Lookup Get(Int32 attributeId, LocaleEnum locale, Int32 maxRecordsToReturn, Collection<Int32> lookupValueIdList, ApplicationContext applicationContext, CallerContext callerContext, Boolean publishEvent)
        {
            return this.Get(attributeId, locale, maxRecordsToReturn, lookupValueIdList, applicationContext, callerContext, publishEvent, false, null);
        }

        /// <summary>
        /// Gets lookup data for the requested attribute name, parent name and locale
        /// </summary>
        /// <param name="attributeName">Represents the attribute short name required the identify an attribute</param>
        /// <param name="attributeParentName">Represents the attribute parent name required the identify an attribute</param>
        /// <param name="locale">Locale for which data needs to be fetched</param>
        /// <param name="maxRecordsToReturn">Max number of lookup records to return. Setting '-1' returns all record</param>
        /// <param name="lookupValueIdList">List of lookup PK Ids to be returned along with requested max number of records to return.</param>
        /// <param name="applicationContext">Application context</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <param name="publishEvent">Indicates the publish event flag</param>
        /// <param name="isDependent">Indicates whether the attribute is dependent or not</param>
        /// <param name="dependentAttributes">Indicates dependent attribute collection details</param>
        /// <returns>Lookup data</returns>
        public Lookup GetAttributeLookupData(String attributeName, String attributeParentName, LocaleEnum locale, Int32 maxRecordsToReturn, Collection<Int32> lookupValueIdList, ApplicationContext applicationContext, CallerContext callerContext, Boolean publishEvent, Boolean isDependent, DependentAttributeCollection dependentAttributes)
        {
            Lookup lookup = null;

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("LookupManager.GetAttributeLookupData", MDMTraceSource.LookupGet, false);

            try
            {
                #region Validate Input Parameters

                if (String.IsNullOrWhiteSpace(attributeName))
                {
                    // Get localized message for system default UI locale
                    LocaleMessageBL localeMessageBL = new LocaleMessageBL();
                    LocaleMessage localeMessage = localeMessageBL.Get(MDM.Utility.GlobalizationHelper.GetSystemUILocale(), "113783", false, callerContext);
                    throw new MDMOperationException("113783", localeMessage.Message, "LookupManager", String.Empty, "GetAttributeLookupData"); // AttributeName should not contain null or white spaces
                }

                if (String.IsNullOrWhiteSpace(attributeParentName))
                {
                    // Get localized message for system default UI locale
                    LocaleMessageBL localeMessageBL = new LocaleMessageBL();
                    LocaleMessage localeMessage = localeMessageBL.Get(MDM.Utility.GlobalizationHelper.GetSystemUILocale(), "113784", false, callerContext);
                    throw new MDMOperationException("113784", localeMessage.Message, "LookupManager", String.Empty, "GetAttributeLookupData"); // AttributeParentName should not contain null or white spaces
                }

                #endregion
             
                Int32 attributeId = new AttributeModelBL().GetAttributeId(attributeName, attributeParentName);

                lookup = this.Get(attributeId, locale, maxRecordsToReturn, lookupValueIdList, applicationContext, callerContext, publishEvent, isDependent, dependentAttributes);

                if (lookup == null)
                {
                    //Get localized message for system default UI locale
                    LocaleMessageBL localeMessageBL = new LocaleMessageBL();
                    LocaleMessage localeMessage = localeMessageBL.Get(MDM.Utility.GlobalizationHelper.GetSystemUILocale(), "110657", false, callerContext);
                    throw new MDMOperationException("110657", localeMessage.Message, "LookupManager", String.Empty, "GetAttributeLookupData"); // Lookup object should not contain null value
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("LookupManager.GetAttributeLookupData", MDMTraceSource.LookupGet);
            }

            return lookup;
        }

        /// <summary>
        /// Gets lookup data for the requested attribute and locale
        /// </summary>
        /// <param name="attributeId">Id of the attribute for which data needs to be get</param>
        /// <param name="locale">Locale for which data needs to be fetched</param>
        /// <param name="maxRecordsToReturn">Max number of lookup records to return. Setting '-1' returns all record</param>
        /// <param name="lookupValueIdList">List of lookup PK Ids to be returned along with requested max number of records to return.</param>
        /// <param name="applicationContext">Application context</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <param name="publishEvent"></param>
        /// <returns>Lookup data</returns>
        public Lookup Get(Int32 attributeId, LocaleEnum locale, Int32 maxRecordsToReturn, Collection<Int32> lookupValueIdList, ApplicationContext applicationContext, CallerContext callerContext, Boolean publishEvent, Boolean isDependent, DependentAttributeCollection dependentAttributes)
        {
            Lookup attrLookupInternal = null;
            Lookup attrLookupToReturn = null;
            RowCollection rowsToReturn = new RowCollection();

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("LookupManager.Get", MDMTraceSource.LookupGet, false);

            if (lookupValueIdList == null)
                lookupValueIdList = new Collection<Int32>();

            try
            {
                #region Validation

                if (attributeId < 1)
                {
                    //Get localized message for system default UI locale
                    LocaleMessageBL localeMessageBL = new LocaleMessageBL();
                    LocaleMessage localeMessage = localeMessageBL.Get(MDM.Utility.GlobalizationHelper.GetSystemUILocale(), "111670", false, callerContext);
                    throw new MDMOperationException("111670", localeMessage.Message, "LookupManager", String.Empty, "Get");
                }

                if (locale == LocaleEnum.Neutral || locale == LocaleEnum.UnKnown)
                {
                    //Get localized message for system default UI locale
                    LocaleMessageBL localeMessageBL = new LocaleMessageBL();
                    LocaleMessage localeMessage = localeMessageBL.Get(MDM.Utility.GlobalizationHelper.GetSystemUILocale(), "111390", false, callerContext);
                    throw new MDMOperationException("111390", localeMessage.Message, "LookupManager", String.Empty, "Get");
                }

                #endregion

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested attribute id:{0}, locale:{1}, maxRecordsToReturn:{2}, Application:{3} and Service:{4}", attributeId, locale.ToString(), maxRecordsToReturn, callerContext.Application, callerContext.Module), MDMTraceSource.LookupGet);

                #region Get lookup from Cache if available

                //Try to get from cache..
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Finding lookup table[attribute id:{0}, locale:{1}] in cache...", attributeId, locale.ToString()), MDMTraceSource.LookupGet);

                attrLookupInternal = _lookupBufferManager.FindLookup(attributeId, locale);

                #endregion

                #region If not found in cache, then load it from the database

                AttributeModel attributeModel = null;
                if (attrLookupInternal == null)
                {
                    lock (lockObj)
                    {
                        attrLookupInternal = _lookupBufferManager.FindLookup(attributeId, locale);

                if (attrLookupInternal == null)
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("No lookup table cache found for attribute id:{0} and locale:{1}. Now Lookup table would be loaded from database.", attributeId, locale.ToString()), MDMTraceSource.LookupGet);

                    attributeModel = GetAttributeModel(attributeId, locale, callerContext);

                    attrLookupInternal = GetAttributeLookupDataFromDB(attributeId, attributeModel, locale, callerContext);
                }
                    }
                }
                else
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Found lookup table[attribute id:{0}, locale:{1}] in cache", attributeId, locale.ToString()), MDMTraceSource.LookupGet);
                }

                #endregion

                if (attrLookupInternal != null)
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Done with lookup table[attribute id:{0} and locale:{1}] load. Total records in table are:{2}", attributeId, locale.ToString(), attrLookupInternal.Rows.Count), MDMTraceSource.LookupGet);

                    #region Create output lookup object

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Creating output copy of lookup table[attribute id: {0} and locale: {1}]...", attributeId, locale.ToString()), MDMTraceSource.LookupGet);

                    attrLookupToReturn = attrLookupInternal.CopyStructure();
                    rowsToReturn = new RowCollection(attrLookupInternal.Rows.ToList());

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Done with output copy creation for lookup table[attribute id: {0} and locale: {1}]", attributeId, locale.ToString()), MDMTraceSource.LookupGet);

                    #endregion

                    #region Dependency Attribute

                    if (isDependent && dependentAttributes != null && dependentAttributes.Count > 0)
                    {
                        Collection<Int32> listOfValidIds = new Collection<Int32>();
                        AttributeDependencyBL attributeDependencyBL = new AttributeDependencyBL();

                        Collection<String> listOfMappingIds = attributeDependencyBL.GetDependencyMappings(attributeId, applicationContext, dependentAttributes, callerContext);

                        foreach (String id in listOfMappingIds)
                        {
                            if (!String.IsNullOrWhiteSpace(id))
                            {
                                listOfValidIds.Add(ValueTypeHelper.Int32TryParse(id, -1));
                            }
                        }

                        if (listOfValidIds != null && listOfValidIds.Count > 0)
                        {
                            rowsToReturn = (RowCollection)attrLookupInternal.GetRecordByIdList(listOfValidIds);
                        }
                        else  //If there is no matching record found return empty.
                        {
                            rowsToReturn.Clear();
                        }
                    }

                    #endregion

                    #region Apply application context filter

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Filtering lookup table[attribute id: {0} and locale: {1}] for Application context: {2}...", attributeId, locale.ToString(), applicationContext.ToXml()), MDMTraceSource.LookupGet);

                    rowsToReturn = FilterLookupRowsForApplicationContext(attrLookupInternal, rowsToReturn, applicationContext);

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Done with filtering of lookup table[attribute id: {0} and locale: {1}] for Application context: {2}", attributeId, locale.ToString(), applicationContext.ToXml()), MDMTraceSource.LookupGet);

                    #endregion

                    #region Publish Lookup loaded event

                    if (publishEvent)
                    {
                        if (Constants.TRACING_ENABLED)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Triggering lookup data load event...", MDMTraceSource.LookupGet);
                        attrLookupToReturn.Rows = rowsToReturn;

                        OnLookupDataLoad(attrLookupToReturn, applicationContext);

                        rowsToReturn = attrLookupToReturn.Rows;
                        if (Constants.TRACING_ENABLED)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Lookup data load event completed.", MDMTraceSource.LookupGet);
                    }

                    #endregion

                    #region Populate Extended property for TotalRecords in lookup table

                    String totalRecordsKey = "TotalRecords";

                    if (attrLookupToReturn.ExtendedProperties.ContainsKey(totalRecordsKey))
                        attrLookupToReturn.ExtendedProperties.Remove(totalRecordsKey);

                    if (!attrLookupToReturn.ExtendedProperties.ContainsKey(totalRecordsKey))
                        attrLookupToReturn.ExtendedProperties.Add(totalRecordsKey, rowsToReturn.Count);

                    #endregion

                    #region Apply max records filter

                    if (maxRecordsToReturn > 0 && attrLookupToReturn.Rows != null && rowsToReturn.Count > maxRecordsToReturn)
                    {
                        if (Constants.TRACING_ENABLED)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Filtering lookup table[attribute id:{0} and locale:{1}] for MaxRecordsToReturn:{2}...", attributeId, locale.ToString(), maxRecordsToReturn), MDMTraceSource.LookupGet);

                        rowsToReturn = new RowCollection(rowsToReturn.Take(maxRecordsToReturn).ToList());

                        if (Constants.TRACING_ENABLED)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Done with filtering of lookup table[attribute id:{0} and locale:{1}] for MaxRecordsToReturn:{2}", attributeId, locale.ToString(), maxRecordsToReturn), MDMTraceSource.LookupGet);
                    }
                    else if (maxRecordsToReturn == 0)
                    {
                        rowsToReturn.Clear();
                    }

                    #endregion

                    #region Add requested lookup value id records into rows

                    if (lookupValueIdList.Count > 0)
                    {
                        if (Constants.TRACING_ENABLED)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Adding requested LookupValueIdList rows for lookup table[attribute id: {0} and locale: {1}] for LookupValueIdList: {2}...", attributeId, locale.ToString(), ValueTypeHelper.JoinCollection(lookupValueIdList, ",")), MDMTraceSource.LookupGet);

                        RowCollection valueIdRows = (RowCollection)attrLookupInternal.GetRecordByIdList(lookupValueIdList);

                        if (valueIdRows != null && valueIdRows.Count > 0)
                        {
                            foreach (Row valueRow in valueIdRows)
                            {
                                Boolean isFound = false;
                                foreach (Row row in rowsToReturn)
                                {
                                    if (row.Id == valueRow.Id)
                                    {
                                        isFound = true;
                                        break;
                                    }
                                }

                                if (!isFound)
                                {
                                    rowsToReturn.Add(valueRow);
                                }
                            }
                        }

                        if (Constants.TRACING_ENABLED)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Done with addition of requested LookupValueIdList rows for lookup table[attribute id: {0} and locale: {1}] for LookupValueIdList: {2}...", attributeId, locale.ToString(), ValueTypeHelper.JoinCollection(lookupValueIdList, ",")), MDMTraceSource.LookupGet);
                    }

                    #endregion

                    attrLookupToReturn.Rows = rowsToReturn;
                }

                // Commenting the app config usage and defaulting to false as performance has degraded in 767. Will be corrected in later FP
                Boolean isLookupIndexesEnabled = false;//AppConfigurationHelper.GetAppConfig("MDMCenter.LookupManager.LookupIndexes.Enabled", false);
                if (isLookupIndexesEnabled)
                {
                    // The indexes are applicable only for clients requesting the full lookup table. That means no value list should be passed and records to return count shoould be -1
                    // TODO - The logic to build indexes should be revisited for perfomance
                    if (attrLookupToReturn != null && lookupValueIdList.Count == 0 && maxRecordsToReturn == -1)
                    {
                        attrLookupToReturn.IsIndexEnabled = true;

                        if (attributeModel == null)
                        {
                            attributeModel = GetAttributeModel(attributeId, locale, callerContext);
                        }

                        attrLookupToReturn.UpdateLookupIndexes(!String.IsNullOrEmpty(attributeModel.ExportMask));
                    }
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("LookupManager.Get", MDMTraceSource.LookupGet);
            }

            return attrLookupToReturn;
        }

        /// <summary>
        /// Gets lookup data for the requested attribute and locale
        /// </summary>
        /// <param name="attributeId">Id of the attribute for which data needs to be get</param>
        /// <param name="locale">Locale for which data needs to be fetched</param>
        /// <param name="maxRecordsToReturn">Max number of lookup records to return. Setting '-1' returns all record</param>
        /// <param name="searchValue">Value to be searched for the attribute lookup.</param>
        /// <param name="applicationContext">Application context</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <param name="publishEvent">Indicates the publish event flag</param>
        /// <returns>Lookup data</returns>
        public Lookup Search(Int32 attributeId, LocaleEnum locale, Int32 maxRecordsToReturn, String searchValue, ApplicationContext applicationContext, CallerContext callerContext, Boolean publishEvent)
        {
            return this.Search(attributeId, locale, maxRecordsToReturn, searchValue, applicationContext, callerContext, publishEvent, false, null);
        }

        /// <summary>
        /// Gets lookup data for the requested attribute and locale
        /// </summary>
        /// <param name="attributeId">Id of the attribute for which data needs to be get</param>
        /// <param name="locale">Locale for which data needs to be fetched</param>
        /// <param name="maxRecordsToReturn">Max number of lookup records to return. Setting '-1' returns all record</param>
        /// <param name="searchValue">Value to be searched for the attribute lookup.</param>
        /// <param name="applicationContext">Application context</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <param name="publishEvent">Indicates the publish event flag</param>
        /// <param name="isDependent">Whether requested attribute is dependent attribute or not</param>
        /// <param name="dependentAttributes">Dependent Attribute Collection details</param>
        /// <returns>Lookup data</returns>
        public Lookup Search(Int32 attributeId, LocaleEnum locale, Int32 maxRecordsToReturn, String searchValue, ApplicationContext applicationContext, CallerContext callerContext, Boolean publishEvent, Boolean isDependent, DependentAttributeCollection dependentAttributeCollection)
        {
            Lookup attrLookup = null;
            RowCollection rowsToReturn = new RowCollection();

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("LookupManager.Search", MDMTraceSource.LookupGet, false);

            try
            {
                #region Validation

                if (String.IsNullOrWhiteSpace(searchValue))
                {
                    //TODO:: Create locale message
                    String messageCode = "ZZZZZZZZ";

                    //Get localized message for system default UI locale
                    LocaleMessageBL localeMessageBL = new LocaleMessageBL();
                    LocaleMessage localeMessage = localeMessageBL.Get(MDM.Utility.GlobalizationHelper.GetSystemUILocale(), messageCode, false, callerContext);
                    throw new MDMOperationException(messageCode, localeMessage.Message, "LookupManager", String.Empty, "Get");
                }

                #endregion

                #region Setup

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested attribute id:{0}, locale:{1}, maxRecordsToReturn:{2}, SearchValue:{3}, Application:{4} and Service:{5}", attributeId, locale.ToString(), maxRecordsToReturn, searchValue, callerContext.Application, callerContext.Module), MDMTraceSource.LookupGet);

                #endregion

                #region Get Lookup table

                attrLookup = this.Get(attributeId, locale, -1, null, applicationContext, callerContext, true, isDependent, dependentAttributeCollection);

                #endregion

                #region Apply search

                if (attrLookup != null && attrLookup.Rows.Count > 0)
                {
                    rowsToReturn = (RowCollection)attrLookup.Filter(Lookup.SearchDataColumnName, SearchOperator.Contains, searchValue, StringComparison.InvariantCultureIgnoreCase);

                    const String filteredRecordsCount = "FilteredRecordsCount";
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        if (attrLookup.ExtendedProperties.ContainsKey(filteredRecordsCount))
                        {
                            attrLookup.ExtendedProperties[filteredRecordsCount] = rowsToReturn.Count;
                        }
                        else
                        {
                            attrLookup.ExtendedProperties.Add(filteredRecordsCount, rowsToReturn.Count);
                        }
                    }
                    else if (attrLookup.ExtendedProperties.ContainsKey(filteredRecordsCount))
                    {
                        attrLookup.ExtendedProperties.Remove(filteredRecordsCount);
                    }
                }

                #endregion

                #region Apply max records filter

                if (maxRecordsToReturn > 0 && rowsToReturn != null && rowsToReturn.Count > maxRecordsToReturn)
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Filtering lookup table[attribute id:{0} and locale:{1}] for MaxRecordsToReturn:{2}...", attributeId, locale.ToString(), maxRecordsToReturn), MDMTraceSource.LookupGet);

                    rowsToReturn = new RowCollection(rowsToReturn.Take(maxRecordsToReturn).ToList());

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Done with filtering of lookup table[attribute id:{0} and locale:{1}] for MaxRecordsToReturn:{2}", attributeId, locale.ToString(), maxRecordsToReturn), MDMTraceSource.LookupGet);
                }

                #endregion

                attrLookup.Rows = rowsToReturn;

            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("LookupManager.Search", MDMTraceSource.LookupGet);
            }

            return attrLookup;
        }

        /// <summary>
        /// Gets lookup data for requested attributes and locale
        /// </summary>
        /// <param name="attributeIds">List of attribute Ids for which data needs to be get</param>
        /// <param name="locale">Locale for which data needs to be fetched</param>
        /// <param name="maxRecordsToReturn">Max number of lookup records to return. Setting '-1' returns all record</param>
        /// <param name="applicationContext">Current context of the application</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <param name="publishEvent"></param>
        /// <returns>Lookup data</returns>
        public LookupCollection Get(Collection<Int32> attributeIds, LocaleEnum locale, Int32 maxRecordsToReturn, ApplicationContext applicationContext, CallerContext callerContext, Boolean publishEvent)
        {
            LookupCollection lookups = null;

            #region Validation

            if (attributeIds == null || attributeIds.Count < 1)
            {
                //Get localized message for system default UI locale
                LocaleMessageBL localeMessageBL = new LocaleMessageBL();
                LocaleMessage localeMessage = localeMessageBL.Get(MDM.Utility.GlobalizationHelper.GetSystemUILocale(), "111673", false, callerContext);
                throw new MDMOperationException("111673", localeMessage.Message, "LookupManager", String.Empty, "Get");
            }

            #endregion

            lookups = new LookupCollection();

            foreach (Int32 attributeId in attributeIds)
            {
                //TODO: applicationContext.AttributeId = attributeId override needed?

                Lookup lookup = this.Get(attributeId, locale, maxRecordsToReturn, applicationContext, callerContext, publishEvent);

                if (lookup != null)
                    lookups.Add(lookup);
            }

            return lookups;
        }

        /// <summary>
        /// Gets lookup data for requested attributes and locale
        /// </summary>
        /// <param name="attributeIds">List of attribute Ids for which data needs to be get</param>
        /// <param name="locale">Locale for which data needs to be fetched</param>
        /// <param name="maxRecordsToReturn">Max number of lookup records to return. Setting '-1' returns all record</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Lookup data</returns>
        public LookupCollection Get(Collection<Int32> attributeIds, LocaleEnum locale, Int32 maxRecordsToReturn, CallerContext callerContext)
        {
            LookupCollection lookups = null;

            #region Validation

            if (attributeIds == null || attributeIds.Count < 1)
            {
                //Get localized message for system default UI locale
                LocaleMessageBL localeMessageBL = new LocaleMessageBL();
                LocaleMessage localeMessage = localeMessageBL.Get(MDM.Utility.GlobalizationHelper.GetSystemUILocale(), "111673", false, callerContext);
                throw new MDMOperationException("111673", localeMessage.Message, "LookupManager", String.Empty, "Get");
            }

            #endregion

            lookups = new LookupCollection();

            foreach (Int32 attributeId in attributeIds)
            {
                Lookup lookup = this.Get(attributeId, locale, maxRecordsToReturn, callerContext);

                if (lookup != null)
                    lookups.Add(lookup);
            }

            return lookups;
        }

        /// <summary>
        /// Processes lookup data
        /// </summary>
        /// <param name="lookup">Lookup data which needs to be processed</param>
        /// <param name="programName">Program name which is requesting for process</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <param name="invalidateCache">Specifies whether to invalidate the lookup cache or not</param>
        /// <returns>Result of the process</returns>
        public OperationResult Process(Lookup lookup, String programName, CallerContext callerContext, Boolean invalidateCache = true)
        {
            OperationResult operationResult = new OperationResult();

            #region Parameter Validation

            if (lookup == null)
            {
                //Get localized message for system default UI locale
                LocaleMessageBL localeMessageBL = new LocaleMessageBL();
                LocaleMessage localeMessage = localeMessageBL.Get(MDM.Utility.GlobalizationHelper.GetSystemUILocale(), "111659", false, callerContext);
                throw new MDMOperationException("111659", localeMessage.Message, "LookupManager", String.Empty, "Process");
            }

            #endregion

            LookupCollection lookupCollection = new LookupCollection();
            lookupCollection.Add(lookup);

            operationResult = Process(lookupCollection, programName, callerContext, invalidateCache);

            return operationResult;
        }

        /// <summary>
        /// Processes Multiple lookup data
        /// </summary>
        /// <param name="lookupCollection">Lookup data which needs to be processed</param>
        /// <param name="programName">Program name which is requesting for process</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <param name="invalidateCache">Specifies whether to invalidate the lookup cache or not</param>
        /// <returns>Result of the process</returns>
        /// <exception cref="MDMOperationException">Thrown when no lookup found for process</exception>
        /// <exception cref="MDMOperationException">Thrown when Lookup Table Name not found.</exception>
        /// <exception cref="MDMOperationException">Thrown when Lookup Locale not found.</exception>
        public OperationResult Process(LookupCollection lookupCollection, String programName, CallerContext callerContext, Boolean invalidateCache = true)
        {
            OperationResult operationResult = new OperationResult();
            LocaleEnum systemDataLocale = MDM.Utility.GlobalizationHelper.GetSystemDataLocale();

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("LookupManager.Process", MDMTraceSource.LookupProcess, false);

                #region Parameter Validations

                if (lookupCollection == null || lookupCollection.Count == 0)
                {
                    throw new MDMOperationException("111659", "Failed to process lookup data. No Lookups are there to process", "LookupManager", String.Empty, "Process");
                }

                #endregion

                StringBuilder listOfLookupTablesToProcess = new StringBuilder();
                Int32 lookupId = -1;

                foreach (Lookup lookup in lookupCollection)
                {
                    ValidateParameters(lookup, systemDataLocale);

                    if (lookup.ExtendedProperties != null && lookup.ExtendedProperties.ContainsKey("SheetName"))
                    {
                        operationResult.ExtendedProperties.Add("SheetName", lookup.ExtendedProperties["SheetName"].ToString());
                    }

                    if (listOfLookupTablesToProcess.Length > 0)
                        listOfLookupTablesToProcess.Append(", ");

                    listOfLookupTablesToProcess.Append(lookup.Name);

                    //Decide row actions based on unique ids
                    ModifyRowActionsBasedOnUniqueKeys(lookup, systemDataLocale, callerContext, operationResult);
                    //This populate Ids if the lookup table or particular lookup row doesnot exist in system.
                    //This is for used for create action.
                    PopulateLookupIdAndRowId(lookup, lookupId);
                }

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("List of lookup table names to process:{0}", listOfLookupTablesToProcess.ToString()), MDMTraceSource.LookupProcess);

				foreach (Lookup lookup in lookupCollection)
				{
					OnLookupProcessingEvent(lookup);
				}

                #region DB Processing

                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Update);

                if (String.IsNullOrWhiteSpace(programName))
                    programName = "LookupManager.Lookup.Process";

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Processing Lookup tables in database...", MDMTraceSource.LookupProcess);

                _lookupDA.Process(lookupCollection, _securityPrincipal.CurrentUserName, systemDataLocale, programName, command, operationResult);

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Processed Lookup tables in database.", MDMTraceSource.LookupProcess);

                #endregion

                if (IsTranslationEnabled())
                {
                    EnqueueIntegrationActivity(lookupCollection, callerContext);
                }

				#region Cache Invalidation of Lookup

                //This flag is used to decide whether to invalidate the cache in lookup manager or not.
                //When importing lookup data, all invalidation of cache should be done in lookupImportEngine after processing all the batches.
                //So in case of importing lookup data, this flag is false, otherwise by default, it is true.
                Dictionary<String, Collection<Int32>> impactedAttributeIdList = new Dictionary<String, Collection<Int32>>();

                if (invalidateCache)
                {
					impactedAttributeIdList = InvalidateLookupData(lookupCollection);
                }

				foreach (Lookup lookup in lookupCollection)
				{
					Collection<Int32> attributeIdList = null;

					if (impactedAttributeIdList.ContainsKey(lookup.Name))
					{
						attributeIdList = impactedAttributeIdList[lookup.Name];
					}

					OnLookupProcessedEvent(lookup, attributeIdList);
				}

				#endregion

                //since there is no error or waring update operation result status as successful.
                if (operationResult.OperationResultStatus == OperationResultStatusEnum.None)
                {
                    operationResult.OperationResultStatus = OperationResultStatusEnum.Successful;

                    if (AppConfigurationHelper.GetAppConfig<Boolean>(Constants.ACTIVITYLOG_DATAMODEL_ENABLED_APPCONFIG_KEY, false))
                    {
                        LogDataModelChanges(lookupCollection, callerContext);
                }
            }
            }
            catch (Exception ex)
            {
                foreach (Lookup lookup in lookupCollection)
                {
                    foreach (Row row in lookup.Rows)
                    {
                        if (row.ExtendedProperties != null && row.ExtendedProperties.ContainsKey("ReferenceId"))
                        {
                            operationResult.AddOperationResult(String.Empty, ex.Message, row.ExtendedProperties["ReferenceId"].ToString(), OperationResultType.Error);
                        }
                        else
                        {
                            operationResult.AddOperationResult(String.Empty, ex.Message, row.Id.ToString(), OperationResultType.Error);
                        }
                    }
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("LookupManager.Process", MDMTraceSource.LookupProcess);
            }
            return operationResult;
        }

        /// <summary>
        /// Invalidates lookup cache for the specified locale.
        /// </summary>
        /// <param name="lookupName">Specifies the name of lookup.</param>
        /// <param name="locale">Specifies the locale for which lookup cache to be cleared.</param>
		public Dictionary<String, Collection<Int32>> InvalidateLookupData(String lookupName, LocaleEnum locale)
        {
            Lookup lookup = new Lookup() { Locale = locale, Name = lookupName };
            LookupCollection collection = new LookupCollection();
            collection.AddLookup(lookup);
            return InvalidateLookupData(collection);
        }

        /// <summary>
        /// Invalidate cache for multiple lookups.
        /// </summary>
        /// <param name="lookupCollection">Specifes the collection of lookups for which cache to be cleared.</param>
		public Dictionary<String, Collection<Int32>> InvalidateLookupData(LookupCollection lookupCollection)
        {
			Dictionary<String, Collection<Int32>> impactedAttributeIdList = new Dictionary<String, Collection<Int32>>();

			//Invalidate Lookup Cache for current Locale
            foreach (Lookup lookup in lookupCollection)
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Removing lookup cache for table:{0} and locale:{1}...", lookup.Name, lookup.Locale.ToString()), MDMTraceSource.LookupProcess);

                _lookupBufferManager.RemoveLookup(lookup.Name, lookup.Locale, true);

                //Removing cache for view for lookup also.
                _lookupBufferManager.RemoveLookup("vw_" + lookup.Name.Replace("tblk_", ""), lookup.Locale, true);

                //Remove cache for all attributes which is refering to this lookup table.
				Collection<Int32> attributeIdsUsingLookupTable = RemoveCacheForAttributeLookup(lookup.Name);
				Collection<Int32> attributeIdsUsingLookupView = RemoveCacheForAttributeLookup("vw_" + lookup.Name.Replace("tblk_", ""));

				if (!impactedAttributeIdList.ContainsKey(lookup.Name))
				{
					//The current lookup table is not there in the collection, add it
					impactedAttributeIdList.Add(lookup.Name, ValueTypeHelper.MergeCollections(attributeIdsUsingLookupTable, attributeIdsUsingLookupView));
				}
				else
				{
					//The current lookup table is already available in the collection, because of a different locale scenario
					//Append new attribute ids
					Collection<Int32> mergedImpactedAttributeIdList = impactedAttributeIdList[lookup.Name];
					mergedImpactedAttributeIdList = ValueTypeHelper.MergeCollections(mergedImpactedAttributeIdList, attributeIdsUsingLookupTable);
					mergedImpactedAttributeIdList = ValueTypeHelper.MergeCollections(mergedImpactedAttributeIdList, attributeIdsUsingLookupView);

					impactedAttributeIdList[lookup.Name] = mergedImpactedAttributeIdList;
				}

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Removed lookup cache for table:{0} and locale:{1}", lookup.Name, lookup.Locale.ToString()), MDMTraceSource.LookupProcess);
            }

            _lookupBufferManager.UpdateDirtyLookupObjectWebServerListCache(lookupCollection);

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Starting async process to clear lookup cache for all the attributes configured for lookup tables...", MDMTraceSource.LookupProcess);

            //Invalidate cache for impacted Data
            Task.Factory.StartNew(() => { InvalidateImpactedData(lookupCollection); });

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Done with async process creation to clear lookup cache for all the attributes configured for lookup tables", MDMTraceSource.LookupProcess);

			return impactedAttributeIdList;
		}

        /// <summary>
        /// Gets All Related Lookup Table Names for Current Lookup Table
        /// </summary>
        /// <param name="lookup">Current Lookup Table Name</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Collection of Referrer Lookups </returns>
        public LookupCollection GetRelatedLookups(Lookup lookup, CallerContext callerContext)
        {
            LookupCollection lookupCollection = new LookupCollection();

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("LookupManager.GetRelatedLookups", MDMTraceSource.LookupGet, false);

            try
            {

                Collection<String> referrerLookupTables = _lookupDA.GetRelatedLookups(lookup.Name, DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read));

                if (referrerLookupTables != null && referrerLookupTables.Count > 0)
                {
                    foreach (String item in referrerLookupTables)
                    {
                        lookupCollection.Add(new Lookup { Name = item.Replace("tblk_", ""), Locale = lookup.Locale });
                    }
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("LookupManager.GetRelatedLookups", MDMTraceSource.LookupGet);
            }

            return lookupCollection;
        }

        /// <summary>
        /// Get all the lookup table names from system.
        /// </summary>
        /// <param name="callerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="lookupContext">Provides lookup context</param>
        /// <returns>Returns the collection of lookup table names</returns>
        public Collection<String> GetAllLookupTableNames(CallerContext callerContext, LookupContext lookupContext)
        {
            return this.GetAllLookupTableNamesFromDB(callerContext, lookupContext);
        }

        /// <summary>
        /// Get the lookup table data by table name,row ids, and locale
        /// </summary>
        /// <param name="lookupTableName">Indicates the lookup table name</param>
        /// <param name="localeEnum"></param>
        /// <param name="rowIds">Indicates the row ids in the lookup</param>
        /// <param name="getLatest">Indicates get the lookup data from db. Always hit DB Because by inserting new lookup, cache is not invalidated </param>
        /// <param name="callerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Returns the collection of lookup table names</returns>
        public Lookup GetLookup(String lookupTableName, LocaleEnum localeEnum, Collection<Int32> rowIds, Boolean getLatest, CallerContext callerContext)
        {
            var lookup = this.Get(lookupTableName, localeEnum, -1, getLatest, callerContext);
            lookup.Rows = (RowCollection)lookup.GetRecordByIdList(rowIds);
            return lookup;
        }

        /// <summary>
        /// Get all lookup models based on dynamic table type
        /// </summary>
        /// <param name="dynamicTableType">Indicates dynamic table type.</param>
        /// <param name="filteredLookupTableNames">Indicates list of lookup table name to be filtered.
        ///  If filtered lookup table names are not available then it will all lookup models.</param>
        /// <param name="callerContext">Indicates the name of application and the module that are performing the action.</param>
        /// <param name="includeViewBasedLookup">Flag indicates weather to include view based lookup or not.</param>
        /// <returns>collection of lookup models.</returns>
        public LookupModelCollection GetLookupModels(DynamicTableType dynamicTableType, Collection<String> filteredLookupTableNames, CallerContext callerContext, Boolean includeViewBasedLookup = true)
        {
            var diagnosticActivity = new DiagnosticActivity();

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.LogDurationInfo("LookupManager.LookupBL.GetLookupModels started...");
            }

            LookupModelCollection lookupModels = null;
            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);
            lookupModels = _lookupDA.GetLookupModels(dynamicTableType, filteredLookupTableNames, command);

            if (!includeViewBasedLookup)
            {
                lookupModels = lookupModels.GetNonViewLookupModels();
            }

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.LogDurationInfo("LookupManager.LookupBL.GetLookupModels completed.");
            }

            return lookupModels;
        }

        #endregion

        #region Private Methods

        private Collection<String> GetAllLookupTableNamesFromDB(CallerContext callerContext, LookupContext lookupContext = null)
        {
            Collection<String> tableNames = new Collection<String>();

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Loading lookup table name list...", MDMTraceSource.LookupGet);
            }

            Boolean filterLookupWithUniqueColumns = lookupContext != null ? lookupContext.TableFilterType == LookupTableFilterType.LookupWithUniqueColumn : false;

            //The following code has been added temporarily in order to complete lookup functionality..
            //TODO::Create BL and DA to get list of tables for requested RST object type(Create an Enum for this)..
            //Remove the reference of StoredProcedures project from the LookupManager 
            DataTable dtTableNames = Riversand.StoredProcedures.Lookup.GetTableNames((Int32)DynamicTableType.Lookup, String.Empty, false, false, filterLookupWithUniqueColumns);

            if (dtTableNames == null || dtTableNames.Rows == null || dtTableNames.Rows.Count < 1 || dtTableNames.Columns == null || !dtTableNames.Columns.Contains("DisplayTableName"))
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Warning, "No lookup tables found in the system. Load method ended with false result.");
                return tableNames;
            }

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Loaded lookup table name list. Total lookups found are:{0}", dtTableNames.Rows.Count));
            }

            foreach (DataRow dataRow in dtTableNames.Rows)
            {
                if (dataRow["DisplayTableName"] != null)
                {
                    try
                    {
                        tableNames.Add(dataRow["DisplayTableName"].ToString());
                    }
                    catch (Exception ex)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Warning, ex.Message, MDMTraceSource.LookupGet);
                        //continue reading
                    }
                }
            }

            return tableNames;
        }

        private void ValidateParameters(Lookup lookup, LocaleEnum systemDataLocale)
        {
            if (String.IsNullOrEmpty(lookup.Name))
            {
                throw new MDMOperationException("111660", "Failed to process lookup data. LookupTableName is not available.", "LookupManager", String.Empty, "Process");
            }

            if (lookup.Locale == LocaleEnum.UnKnown || lookup.Locale == LocaleEnum.Neutral)
            {
                throw new MDMOperationException("111661", "Failed to process lookup data. Locale is not available.", "LookupManager", String.Empty, "Process");
            }


        }

        private void PopulateLookupIdAndRowId(Lookup lookup, Int32 lookupId)
        {
            //if Lookup Table Id is not there put Id as -1,-2,-3..
            if (lookup.Id < 1)
                lookup.Id = lookupId--;

            #region populate Ids for each row of lookup Data

            if (lookup.Rows != null && lookup.Rows.Count > 0)
            {
                Int32 rowId = -1;
                foreach (Row row in lookup.Rows)
                {
                    //if any lookup row Id is not there put Id as -1,-2,-3..
                    if (row.Id < 1)
                        row.Id = rowId--;
                }
            }

            #endregion
        }

        private void ModifyRowActionsBasedOnUniqueKeys(Lookup newLookup, LocaleEnum systemDataLocale, CallerContext callerContext, OperationResult operationResult)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "LookupImportEngine.PopulateLookupRowId starting...");
            }

            Lookup existingLookup = Get(newLookup.Name, newLookup.Locale, -1, false, callerContext);

            if (existingLookup == null)
            {
                String message = String.Format("Failed to process lookup data. The lookup name [{0}] does not exist in the system.", newLookup.Name);
                throw new MDMOperationException("112894", message, "LookupManager", String.Empty, "Process");
            }

            RowCollection rowsToBeRemoved = new RowCollection();

            if (!existingLookup.HasUniqueColumns && callerContext.Module == MDMCenterModules.UIProcess)
            {
                // The below code is user to handle the following scenario:
                // When lookup defination does not contain any unique columns and those non-unique columns are marked as non-nullable
                // If user passes null values for the columns marked as non-nullable then database will throw an exception.
                // To avoid throwing of an exception, the below validation code has been added
                // Note:- If user performs imports then no need to do the below check because we already have a validation furthur
                //        in a code that will verify, whether lookup contains any unique columns or not
                //        if lookup does not contains any unique columns or not remove it from processing
                foreach (Row row in newLookup.Rows)
                {
                    // These will check whether the nonunique column which is marked as non-nullable contains value or not
                    // If row does not contain value, then row will be consider as invalid and will be removed from futher processing
                    Boolean isAllNonUniqueKeysOfRowValid = ValidateAllNonUniqueKeysOfRow(newLookup, row, operationResult);

                    if (!isAllNonUniqueKeysOfRowValid)
                    {
                        rowsToBeRemoved.Add(row);
                        continue;
                    }
                }

                this.RemoveInvalidRows(rowsToBeRemoved, newLookup);

                // To support backward compatibility, 
                // Skip the unique key validation in the case of there is no unique key found for the lookup
                //and the process request comes from UI
                return;
            }

            Dictionary<String, Object> defaultValues = GetDefaultColumnValues(existingLookup);

            String uniqueKey = String.Empty;
            StringBuilder uniqueKeyBuilder = new StringBuilder();
            // OperationResult operationResult = new OperationResult();
            Dictionary<String, KeyValuePair<Int64, Boolean>> existingKeys = new Dictionary<String, KeyValuePair<Int64, Boolean>>();

            String uniqueKeysWithNotNull = String.Empty; // It will contain comma seperated name of columns marked as unique and not null
            StringBuilder uniqueKeysBuilderWithNotNull = new StringBuilder();

            //Before modifying the action ignore the invalid columns
            this.IgnoreInvalidColumn(newLookup, existingLookup.GetColumns(), operationResult);

            newLookup.Id = existingLookup.Id;   //Fill the lookup table id.
            if (!operationResult.ExtendedProperties.ContainsKey("LookupTableId"))
            {
                operationResult.ExtendedProperties.Add("LookupTableId", newLookup.Id);
            }

            foreach (Column column in existingLookup.Columns)
            {
                if (column.IsUnique)
                {
                    if (uniqueKeyBuilder.Length > 0)
                    {
                        uniqueKeyBuilder.Append(',');
                    }

                    uniqueKeyBuilder.Append(column.Name.Trim());

                    // String builder that will build comma seperated name of columns marked as unique and not null
                    if (!column.Nullable)
                    {
                        uniqueKeysBuilderWithNotNull.Append(column.Name.Trim() + ",");
                    }
                }

                // Sets the nullable property of newlookup as per the metadata (existing lookup)
                Column newLookupColumn = newLookup.Columns.GetColumn(column.Name);
                if (newLookupColumn != null)
                {
                    newLookupColumn.Nullable = column.Nullable;
                }
            }

            if (uniqueKeyBuilder.Length > 0)
            {
                uniqueKey = uniqueKeyBuilder.ToString();
            existingKeys = CreateUniqueValuesForTable(existingLookup, uniqueKey, operationResult);
            }
            else
            {
                String errorMessage = String.Format("Unique column(s) does not exist for  lookup [{0}]. Add one or more unique column(s).", newLookup.Name);
                throw new MDMOperationException("112895", errorMessage, "LookupManager", String.Empty, "Process");
            }

            if (uniqueKeysBuilderWithNotNull.Length > 0)
            {
                uniqueKeysWithNotNull = uniqueKeysBuilderWithNotNull.ToString();
                uniqueKeysWithNotNull = uniqueKeysWithNotNull.Remove(uniqueKeysWithNotNull.LastIndexOf(","), 1);
            }

            HashSet<String> uniqueValueSet = new HashSet<String>();

            foreach (Row row in newLookup.Rows)
            {
                if (!String.IsNullOrWhiteSpace(uniqueKey))
                {
                    String newUniqueValue = CreateUniqueValueForRow(newLookup, row, uniqueKey, false, operationResult, true);

                    if (!String.IsNullOrWhiteSpace(uniqueKey) && String.IsNullOrWhiteSpace(newUniqueValue) && row.Action != ObjectAction.Delete)
                    {
                        this.LogError("One or more unique row(s) are not having value for lookup : {0}", "112892", row.Id.ToString(), new Collection<Object> { newLookup.Name }, operationResult);
                        rowsToBeRemoved.Add(row);
                        continue;
                    }

                    // Note: These below code is used to check whether user has passed values all mandatory unique columns or not
                    // The below code will generate unique values from the columns which are marked as unique and not null
                    String newUniqueValueOfNotNullColumns = CreateUniqueValueForRow(newLookup, row, uniqueKeysWithNotNull, true, operationResult);

                    // If columns with unique and not null exists in newlookup then,
                    //      Check if user has passed values all mandatory unique columns or not
                    //      If Yes then,
                    //          Proceed
                    //      Else 
                    //          Remove lookup from furthur processing
                    if (!String.IsNullOrWhiteSpace(uniqueKeysWithNotNull) && row.Action != ObjectAction.Delete)
                    {
                        String[] notNullColumnSplit = new String[] { "@#@" };

                        if (String.IsNullOrWhiteSpace(newUniqueValueOfNotNullColumns) || newUniqueValueOfNotNullColumns.Split(notNullColumnSplit, StringSplitOptions.None).Length != uniqueKeysWithNotNull.Split(',').Length)
                        {
                            rowsToBeRemoved.Add(row);
                            continue;
                        }
                    }

                    // These will check whether the nonunique column which is marked as non-nullable contains value or not
                    // If row does not contain value, then row will be consider as invalid and will be removed from futher processing
                    Boolean isAllNonUniqueKeysOfRowValid = ValidateAllNonUniqueKeysOfRow(newLookup, row, operationResult);

                    if (!isAllNonUniqueKeysOfRowValid)
                    {
                        rowsToBeRemoved.Add(row);
                        continue;
                    }

                    if (existingKeys.Keys.Contains(newUniqueValue))
                    {
                        KeyValuePair<Int64, Boolean> map = existingKeys[newUniqueValue];

                        if (callerContext.Module == MDMCenterModules.UIProcess &&
                            ((row.Action == ObjectAction.Create && systemDataLocale == row.Locale) ||    //When the row created for SDL
                            (row.Action == ObjectAction.Create && systemDataLocale != row.Locale && row.Id != map.Key) ||   //When the row created for non SDL
                            (row.Action == ObjectAction.Update && row.Id != map.Key)))  //when the row is updated
                        {
                            throw new MDMOperationException("113229", "Values for unique key cannot be duplicated", "LookupManager", String.Empty, "Process");
                        }

                        if (!uniqueValueSet.Contains(newUniqueValue))
                        {
                        row.Id = map.Key;
                        }

                        if (row.Action != ObjectAction.Delete)
                        {
                            if (!map.Value)
                            {
                                row.Action = ObjectAction.Update;
                            }
                            else
                            {
                                row.Action = ObjectAction.Create;
                            }
                        }
                    }
                    else
                    {
                        if (callerContext.Module == MDMCenterModules.UIProcess &&
                            ((row.Action == ObjectAction.Update ||
                            row.Action == ObjectAction.Create) && systemDataLocale != row.Locale))    //when updating non SDL row user update the unique key with not exist key (since unique key value is not exist, row action comes as create)
                        {
                            throw new MDMOperationException("113230", "Unique key values can be updated only for system data locale", "LookupManager", String.Empty, "Process");
                        }

                        //  isUpdateActionForSDL is true when the user updating the unique key value for the SDL row.In this case no need to change the action.
                        Boolean isUpdateActionForSDL = (callerContext.Module == MDMCenterModules.UIProcess && row.Action == ObjectAction.Update && systemDataLocale == row.Locale);

                        //In case if deleting rows from UI, the newUniqueValue will be empty with Action as Delete.
                        //So in that case action should not be changed, otherwise is should be create action.
                        if (!isUpdateActionForSDL && row.Action != ObjectAction.Delete)
                        {
                            row.Action = ObjectAction.Create;
                        }
                    }

                    if (!uniqueValueSet.Add(newUniqueValue))
                    {
                        if (row.Action != ObjectAction.Delete)
                        {
                            StringBuilder rowUniqueValue = new StringBuilder();

                            String[] rowUniqueValues = newUniqueValue.Split(',');

                            for (Int32 i = 0; i < rowUniqueValues.Length; i++)
                            {
                                String[] columnUniqueValue = rowUniqueValues[i].Split(_uniqueValueSeperator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                if (columnUniqueValue.Length == 2)
                                {
                                    rowUniqueValue.Append(String.Format("{0},", columnUniqueValue[1]));
                                }
                            }

                            newUniqueValue = rowUniqueValue.Remove(rowUniqueValue.ToString().LastIndexOf(","), 1).ToString();

                        this.LogError("The duplicate unique key value [{0}] in input file for lookup [{1}] exists in the same batch.", "112896", row.Id.ToString(), new Collection<Object>() { newUniqueValue, newLookup.Name }, operationResult);
                            rowsToBeRemoved.Add(row);
                            continue;
                        }
                    }

                    //Added unique key to row's extended properties. In the case procedure throws error use this to populate the reference id.
                    if (row.ExtendedProperties == null)
                    {
                        row.ExtendedProperties = new Hashtable();
                    }
                    if (row.ExtendedProperties.ContainsKey("ReferenceId"))
                    {
                        row.ExtendedProperties["ReferenceId"] = newUniqueValue;
                    }
                    else
                    {
                        row.ExtendedProperties.Add("ReferenceId", newUniqueValue);
                    }
                }
                else
                {
                    //If unique key is not there for lookup and trying to delete from UI, then action should not be changed.
                    //Else, change the action to Create.
                    if (row.Action != ObjectAction.Delete)
                        row.Action = ObjectAction.Create;
                }

                if (newLookup.Locale != systemDataLocale)
                {
                    if (row.Action == ObjectAction.Create && row.Id < 0)
                    {
                    String referenceId = row.ExtendedProperties != null && row.ExtendedProperties.ContainsKey("ReferenceId") ? row.ExtendedProperties["ReferenceId"].ToString() : String.Empty;
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "Failed to process lookup data. Lookup record creation is allowed only for system data locale.", MDMTraceSource.LookupProcess);
                    operationResult.AddOperationResult("111662", "Failed to process lookup data. Lookup Record creation is allowed only for system data locale.", referenceId, OperationResultType.Error);
                        rowsToBeRemoved.Add(row);
                    }
                }

                //Fill Default Values.
                if (row.Action == ObjectAction.Create && defaultValues != null && defaultValues.Count > 0 && newLookup.Locale == systemDataLocale)
                {
                    //Default column value will get populated only for system locale.
                    this.PopulateDefaultColumnValues(row, defaultValues);
                }
            }

            //Check permission
            if (IsLookupSecurityFeatureEnabled())
            {
                ValidateLookupPermission(newLookup, existingLookup.PermissionSet, operationResult, rowsToBeRemoved);
            }

            this.RemoveInvalidRows(rowsToBeRemoved, newLookup);

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "LookupImportEngine.PopulateLookupRowId completed...");
            }
        }

        private void RemoveInvalidRows(RowCollection invalidRows, Lookup sourceLookup)
        {
            if (invalidRows != null && invalidRows.Count > 0 && sourceLookup != null && sourceLookup.Rows != null && sourceLookup.Rows.Count > 0)
                {
                foreach (Row row in invalidRows)
                    {
                    sourceLookup.Rows.Remove(row);
                }
            }
        }

        private Dictionary<String, KeyValuePair<Int64, Boolean>> CreateUniqueValuesForTable(Lookup lookup, String uniqueKey, OperationResult operationResult)
        {
            Dictionary<String, KeyValuePair<Int64, Boolean>> uniqueIdentifierMap = new Dictionary<String, KeyValuePair<Int64, Boolean>>();

            foreach (Row row in lookup.Rows)
            {
                String uniqueValue = CreateUniqueValueForRow(lookup, row, uniqueKey, true, operationResult);

                if (!uniqueIdentifierMap.Keys.Contains(uniqueValue))
                {
                    uniqueIdentifierMap.Add(uniqueValue, new KeyValuePair<Int64, Boolean>(row.Id, row.IsSystemLocaleRow));
                }
            }

            return uniqueIdentifierMap;
        }

        private String CreateUniqueValueForRow(Lookup lookup, Row row, String uniqueKey, Boolean isLookupFromDB, OperationResult operationResult, Boolean logErrorForEmptyValue = false)
        {
            String lookupName = lookup.Name;
            Boolean isUniqueColumnNullable = false;

            String uniqueValue = String.Empty;
            String[] uniqueKeyColumnArray = uniqueKey.Split(',');

            foreach (String participatingColumnName in uniqueKeyColumnArray)
            {
                Column participatingColumn = lookup.Columns.GetColumn(participatingColumnName);
                if (participatingColumn != null)
                {
                    isUniqueColumnNullable = participatingColumn.Nullable;
                }

                Cell[] cells = row.Cells.Where(c => c.ColumnName.Equals(participatingColumnName)).ToArray();

                Cell cell = (cells.Length > 0) ? cells[0] : null;
                Object cellVal = (cell == null) ? null : cell.Value;

                if (cells.Length == 1 && cellVal != null && !String.IsNullOrWhiteSpace(cellVal.ToString()))
                {
                    if (!String.IsNullOrEmpty(uniqueValue))
                    {
                        uniqueValue += "@#@";
                    }

                    uniqueValue += String.Format("{0}{1}{2}", participatingColumnName, _uniqueValueSeperator, cell.Value.ToString().Trim());
                }
                else if (!isLookupFromDB)
                {
                    if (cells.Length > 1)
                    {
                        logErrorForEmptyValue = false;
                        this.LogError("The multiple values in lookup table [{1}] exist for the same column name [{0}]. Every row must have single cell for a column.", "112897", row.Id.ToString(), new Collection<Object>() { participatingColumnName, lookupName }, operationResult);
                    }

                    //If any of the unique column(s) do not have value and not nullable then throw the error.
                    else if (cells.Length == 0 || cellVal == null || (cellVal != null && String.IsNullOrWhiteSpace(cellVal.ToString())))
                    {
                        if (!isUniqueColumnNullable)
                        {
                        logErrorForEmptyValue = false;
                        this.LogError("The value is missing for the column [{0}] in lookup table [{1}]. Every row in table must have cell(s) for unique key column(s).", "112898", row.Id.ToString(), new Collection<Object>() { participatingColumnName, lookupName }, operationResult);
                    }
                    }
                }
            }

            if (logErrorForEmptyValue && !String.IsNullOrWhiteSpace(uniqueKey) && String.IsNullOrWhiteSpace(uniqueValue) && row.Action != ObjectAction.Delete)
            {
                if (!isUniqueColumnNullable)
                {
                this.LogError("One or more unique row(s) are not having value for lookup : {0}", "112892", row.Id.ToString(), new Collection<Object> { lookupName }, operationResult);
            }
            }

            return uniqueValue;
        }

        private Boolean ValidateAllNonUniqueKeysOfRow(Lookup newLookup, Row row, OperationResult operationResult)
        {
            String lookupName = newLookup.Name;
            Boolean isValidRow = true;

            foreach (Column newLookupColumn in newLookup.Columns)
            {
                String newLookupColumnName = newLookupColumn.Name;

                // If column name is 'Id' then skip
                if (String.Compare(newLookupColumnName.ToLower(), "id") == 0)
                {
                    continue;
                }

                // If newLookup contains non-unique column and they are marked as non-nullable
                if (!newLookupColumn.IsUnique && !newLookupColumn.Nullable)
                {
                    Object cellVal = row.GetValue(newLookupColumnName);

                    //if any of the non-unique column(s) do not have value and are non-nullable then throw an error.
                    if (cellVal == null || (String.IsNullOrWhiteSpace(cellVal.ToString())))
                    {
                        isValidRow = false;
                        this.LogError("The value is missing for the column [{0}] in lookup table [{1}]. Check for the lookup definition and try again.", "114076", row.Id.ToString(), new Collection<Object>() { newLookupColumnName, lookupName }, operationResult);
                    }
                }
            }

            return isValidRow;
        }

        private void LogError(String message, String messageCode, String referenceId, Collection<Object> parameters, OperationResult operationResult)
        {
            String errorMessage = message;
            if (parameters != null && parameters.Count > 0)
            {
                errorMessage = String.Format(message, parameters.ToArray());
            }
            MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, errorMessage, MDMTraceSource.LookupProcess);
            operationResult.AddOperationResult(messageCode, errorMessage, referenceId, parameters, OperationResultType.Error);
        }

		private Collection<Int32> RemoveCacheForAttributeLookup(String tableName)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Starting RemoveCacheForAttributeLookup.....");

			DataSet ds = new DataSet();
            Int32 attrId = 0;
            LookupBufferManager lookupBufferManager = new LookupBufferManager();
            LocaleCollection locales = new LocaleBL().GetAvailableLocales();
			Collection<Int32> impactedAttributeIds = new Collection<Int32>();

            ds = Riversand.StoredProcedures.Common.GetObject("LookupTableWhereUsed", 0, tableName, 0);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Columns.Contains("attr_id"))
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Int32.TryParse(dr["attr_id"].ToString(), out attrId);

                    if (attrId > 0)
                    {
                        foreach (Locale locale in locales)
                        {
                            lookupBufferManager.RemoveLookup(attrId, locale.Locale, true);
                        }

						if (!impactedAttributeIds.Contains(attrId))
						{
							impactedAttributeIds.Add(attrId);
						}
					}
                }
            }

			if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Ending RemoveCacheForAttributeLookup.....");

			return impactedAttributeIds;
		}

        private void GetSecurityPrincipal()
        {
            if (_securityPrincipal == null)
            {
                _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            }
        }

        private Lookup GetLookupDataFromDB(String lookupTableName, LocaleEnum locale, CallerContext callerContext)
        {
            Lookup lookup = null;

            //Along with requested locale, we need to fetch lookup data for system data locale also..
            //Because DB will not be having merged data.. merging has to be done here.
            //In order to do that we need to have data for system locale also

            #region Locale Dictionary construction

            Dictionary<LocaleEnum, Boolean> localeDictionary = new Dictionary<LocaleEnum, Boolean>();

            //Get system data locale
            LocaleEnum systemDataLocale = MDM.Utility.GlobalizationHelper.GetSystemDataLocale();

            //Make sure that the request is not for the system locale itself
            if (locale == systemDataLocale)
                localeDictionary.Add(systemDataLocale, true);
            else
            {
                localeDictionary.Add(systemDataLocale, true);
                localeDictionary.Add(locale, false);
            }

            #endregion

            LookupCollection lookupsForLocale = this.LoadLookupFromDB(lookupTableName, localeDictionary, systemDataLocale, callerContext);

            //Get the lookup for the requested locale
            lookup = lookupsForLocale.GetLookup(lookupTableName, locale);

            return lookup;
        }

        /// <summary>
        /// Gets the lookup data from cache based on requested lookup table name and the locales.
        /// </summary>
        /// <param name="lookupTableName">Indicates the lookup table name</param>
        /// <param name="localeDictionary">Indicates the locale list</param>
        /// <returns>Returns the list of lookups and list of locales which are not available in cache. </returns>
        private Tuple<LookupCollection, Dictionary<LocaleEnum, Boolean>> LoadLookupFromCache(String lookupTableName, Dictionary<LocaleEnum, Boolean> localeDictionary)
        {
            LookupCollection lookups = new LookupCollection();
            Dictionary<LocaleEnum, Boolean> missingLocales = new Dictionary<LocaleEnum, Boolean>();

            foreach (KeyValuePair<LocaleEnum, Boolean> locale in localeDictionary)
            {
                Lookup lookup = _lookupBufferManager.FindLookup(lookupTableName, locale.Key);

                if (lookup != null)
                {
                    lookups.Add(lookup);
                }
                else
                {
                    missingLocales.Add(locale.Key, locale.Value);
                }
            }

            return new Tuple<LookupCollection, Dictionary<LocaleEnum, Boolean>>(lookups, missingLocales);
        }

        /// <summary>
        /// Load the lookup data from data base and update to cache. And returns the lookups based on the inputs.
        /// If multiple threads are requested for the same lookup to load data from data base, 
        /// Only the first thread will load the data from data base. Other thread will wait to get the data from cache.
        /// </summary>
        /// <param name="lookupTableName">Indicates the lookup table name</param>
        /// <param name="localeDictionary">Indicates the list of locales</param>
        /// <param name="systemDataLocale">Indicates the system locale</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Returns the collection of lookups</returns>
        private LookupCollection LoadLookupFromDB(String lookupTableName, Dictionary<LocaleEnum, Boolean> localeDictionary, LocaleEnum systemDataLocale, CallerContext callerContext)
        {
            LookupCollection result = null;

            if (_lookupBufferManager.IsLookupCacheEnabled)
            {
                Object lockObject = _lookupLockKeys.GetOrAdd(lookupTableName, new Object());
                lock (lockObject)
                {
                    // This is special case when one thread loaded the data from db & removed from from _lookupLockKeys object
                    // By the same time if other thread added into the _lookupLockKeys dictionary, so the actual data is available in cache
                    // so always try to get data from cache if not then get data from db 
                    Tuple<LookupCollection, Dictionary<LocaleEnum, Boolean>> tupleResult = this.LoadLookupFromCache(lookupTableName, localeDictionary);
                    result = tupleResult.Item1; // Lookups which are loaded from cache.

                    if (tupleResult.Item2 != null && tupleResult.Item2.Keys.Count > 0)
                    {
                        if (!tupleResult.Item2.ContainsKey(systemDataLocale))   //System data locale always required to get the model from db.
                        {
                            tupleResult.Item2.Add(systemDataLocale, true);
                        }

                        //Load lookup data from data base
                        LookupCollection dbLookups = this.GetLookupDataFromDB(lookupTableName, tupleResult.Item2, systemDataLocale, callerContext);

                        if (dbLookups != null && dbLookups.Count > 0)
                        {
                            if (result == null || result.Count == 0)
                            {
                                result = dbLookups;
                            }
                            else
                            {
                                //Merge cached lookups and lookups which are loaded from db
                                result.AddLookups(dbLookups);
                            }
                        }
                    }
                }

                Object objToBeRemoved = null;
                _lookupLockKeys.TryRemove(lookupTableName, out objToBeRemoved);
            }
            else
            {
                //When cache is disabled load data from data base always
                result = this.GetLookupDataFromDB(lookupTableName, localeDictionary, systemDataLocale, callerContext);
            }

            return result;
        }

        private LookupCollection GetLookupDataFromDB(String lookupTableName, Dictionary<LocaleEnum, Boolean> localeDictionary, LocaleEnum systemDataLocale, CallerContext callerContext)
        {
            LookupCollection lookupCollection = null;

            //Get command
            DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Read);

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Loading lookup table:'{0}' from database...", lookupTableName, MDMTraceSource.LookupGet));

            //Make DB call..
            //TODO: Add maxRecordsToReturn support here if needed
            lookupCollection = _lookupDA.Get(lookupTableName, localeDictionary, 0, 0, command);

            if (lookupCollection != null && lookupCollection.Count > 0)
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Loaded lookup table:'{0}' from database.", lookupTableName, MDMTraceSource.LookupGet));

                #region Merge lookup data with system locale

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Merging lookup table:'{0}'s current locale values and sytem locale values...", lookupTableName, MDMTraceSource.LookupGet));

                MergeWithSystemData(lookupCollection, lookupTableName, systemDataLocale);

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Done with merge of lookup table:'{0}'. ", lookupTableName, MDMTraceSource.LookupGet));

                #endregion

                #region Cache lookup data

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Caching lookup table:'{0}'...", lookupTableName, MDMTraceSource.LookupGet));

                foreach (Lookup lkup in lookupCollection)
                {
                    //Cache lookup
                    _lookupBufferManager.UpdateLookup(lkup, 3);
                }

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Done with caching lookup table:'{0}'", lookupTableName, MDMTraceSource.LookupGet));

                #endregion
            }

            return lookupCollection;
        }

        private Lookup GetAttributeLookupDataFromDB(Int32 attributeId, AttributeModel attributeModel, LocaleEnum locale, CallerContext callerContext)
        {
            Lookup attrLookup = null;

            #region Lookup Table Get

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Loading lookup master table data for lookup table:{0}...", attributeModel.LookUpTableName, MDMTraceSource.LookupGet));

            Lookup lookupMetadata = this.Get(attributeModel.LookUpTableName, locale, -1, false, callerContext);

            if (lookupMetadata == null)
            {
                //Get localized message for system default UI locale
                LocaleMessageBL localeMessageBL = new LocaleMessageBL();
                LocaleMessage localeMessage = localeMessageBL.Get(MDM.Utility.GlobalizationHelper.GetSystemUILocale(), "111672", false, callerContext);
                throw new MDMOperationException("111672", localeMessage.Message, "LookupManager", String.Empty, "Get");
            }
            else
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Loaded lookup master table data for lookup table:{0}", attributeModel.LookUpTableName, MDMTraceSource.LookupGet));
            }

            #endregion

            #region Construct Attribute Lookup Data

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Starting construction of lookup data for an attribute:{0} and locale:{1} using lookup master table:{2}...", attributeId, locale.ToString(), attributeModel.LookUpTableName), MDMTraceSource.LookupGet);

            attrLookup = LookupUtility.ConstructAttributeLookupData(attributeModel, lookupMetadata);

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Done with lookup data building for an attribute:{0} and locale:{1} using lookup master table:{2}", attributeId, locale.ToString(), attributeModel.LookUpTableName), MDMTraceSource.LookupGet);

            #endregion

            #region Cache Attribute Lookup Data

            if (attrLookup != null)
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Caching attribute level lookup table for attribute id:{0}, locale:{1} and lookup table:{2}...", attributeId, locale.ToString(), attributeModel.LookUpTableName), MDMTraceSource.LookupGet);

                //Cache lookup
                _lookupBufferManager.UpdateLookup(attrLookup, attributeId, locale, 3);

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Done with caching attribute level lookup table for attribute id:{0}, locale:{1} and lookup table:{2}", attributeId, locale.ToString(), attributeModel.LookUpTableName), MDMTraceSource.LookupGet);
            }
            else
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Warning, String.Format("Loaded lookup table data for attribute id:{0} and locale:{1} is NULL. Resyult would be return as NULL only and no caching operation would be performed on this data,", attributeId, locale.ToString()));
            }

            #endregion

            return attrLookup;
        }

        private AttributeModel GetAttributeModel(Int32 attributeId, LocaleEnum locale, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Loading attribute model for attribute:" + attributeId.ToString(), MDMTraceSource.LookupGet);

            Collection<LocaleEnum> locales = new Collection<LocaleEnum>();
            locales.Add(locale);

            AttributeModelContext attributeModelContext = new AttributeModelContext();
            attributeModelContext.AttributeModelType = AttributeModelType.AttributeMaster;
            attributeModelContext.CategoryId = 0;
            attributeModelContext.ContainerId = 0;
            attributeModelContext.EntityId = 0;
            attributeModelContext.EntityTypeId = 0;
            attributeModelContext.GetCompleteDetailsOfAttribute = true;
            attributeModelContext.GetOnlyRequiredAttributes = false;
            attributeModelContext.GetOnlyShowAtCreationAttributes = false;
            attributeModelContext.RelationshipTypeId = 0;
            attributeModelContext.Locales = locales;
            attributeModelContext.ApplySecurity = false;
            attributeModelContext.ApplySorting = false;

            Collection<Int32> attributeIdList = new Collection<Int32>();
            attributeIdList.Add(attributeId);

            AttributeModel attributeModel = null;
            AttributeModelBL attributeModelManager = new AttributeModelBL();

            AttributeModelCollection attributeModelCollection = attributeModelManager.GetByIds(attributeIdList, attributeModelContext);

            if (attributeModelCollection != null && attributeModelCollection.Count > 0)
            {
                attributeModel = attributeModelCollection[attributeId, locale];
            }

            if (attributeModel == null)
            {
                //Get localized message for system default UI locale
                LocaleMessageBL localeMessageBL = new LocaleMessageBL();
                LocaleMessage localeMessage = localeMessageBL.Get(MDM.Utility.GlobalizationHelper.GetSystemUILocale(), "111671", false, callerContext);
                throw new MDMOperationException("111671", localeMessage.Message, "LookupManager", String.Empty, "Get");
            }
            else
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Loaded attribute model for attribute:{0} and locale:{1}", attributeId.ToString(), locale.ToString(), MDMTraceSource.LookupGet));
            }

            return attributeModel;
        }

        private void MergeWithSystemData(LookupCollection lookupsForLocales, String lookupTableName, LocaleEnum systemDataLocale)
        {
            Lookup systemDataLocaleLookup = lookupsForLocales.GetLookup(lookupTableName, systemDataLocale);

            if (systemDataLocaleLookup != null)
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Total records in table:'{0}' and system locale:'{1}' are:{2}", systemDataLocaleLookup.Name, systemDataLocale, systemDataLocaleLookup.Rows.Count), MDMTraceSource.LookupGet);

                foreach (Lookup lookup in lookupsForLocales)
                {
                    if (lookup != systemDataLocaleLookup)
                    {
                        if (systemDataLocaleLookup.Rows != null && systemDataLocaleLookup.Rows.Count > 0)
                        {
                            foreach (Row sysLkupRow in systemDataLocaleLookup.Rows)
                            {
                                Row curLkupRow = null;

                                //Get the corresponding row in current lookup
                                if (lookup.Rows != null)
                                    curLkupRow = lookup.Rows.GetRow(sysLkupRow.Id);

                                //Check whether this row exist in the current lookup..
                                if (curLkupRow == null)
                                {
                                    //Row does not exist..
                                    //Add this row in the current lookup
                                    curLkupRow = lookup.NewRow(false, sysLkupRow.Id);

                                    curLkupRow.IsSystemLocaleRow = true;
                                }

                                foreach (Column sysLkupColumn in systemDataLocaleLookup.Columns)
                                {
                                    //Get value for the same field from system data locale lookup
                                    Object value = sysLkupRow.GetValue(sysLkupColumn);

                                    Cell cellInContext = curLkupRow.GetCell(sysLkupColumn.Id);

                                    if (cellInContext != null)
                                    {
                                        if (curLkupRow.GetValue(sysLkupColumn) == null)
                                        {
                                            curLkupRow.SetValue(sysLkupColumn, value);

                                            //Set flag saying the value is of system locales's data value..
                                            cellInContext.IsSystemLocaleValue = true;
                                        }

                                        cellInContext.SystemLocaleValue = value;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void OnLookupDataLoad(Lookup lookup, ApplicationContext applicationContext)
        {
            var eventArgs = new LookupDataLoadEventArgs(lookup, applicationContext);
            LookupEventManager.Instance.OnLookupDataLoad(eventArgs);
        }

		private void OnLookupProcessingEvent(Lookup lookup)
		{
			var eventArgs = new LookupDataProcessEventArgs(lookup);
			LookupEventManager.Instance.OnLookupDataProcessing(eventArgs);
		}

		private void OnLookupProcessedEvent(Lookup lookup, Collection<Int32> impactedAttributeIds)
		{
			var eventArgs = new LookupDataProcessEventArgs(lookup, impactedAttributeIds);
			LookupEventManager.Instance.OnLookupDataProcessed(eventArgs);
		}
        
        private void InvalidateImpactedData(LookupCollection lookupCollection)
        {
            try
            {
                //Set thread operation context
                OperationContext.Current = _operationContext;

                LocaleBL _localeBL = new LocaleBL();
                LocaleCollection localeCollection = _localeBL.GetAvailableLocales();

                _lookupBufferManager.InvalidateImpactedData(lookupCollection, localeCollection);
            }
            catch
            {
                throw;
            }
        }

        private RowCollection FilterLookupRowsForApplicationContext(Lookup lookup, RowCollection rows, ApplicationContext applicationContext)
        {
            RowCollection rowCollection = new RowCollection();

            String categoryPathSeperator = AppConfigurationHelper.GetAppConfig("Catalog.Category.PathSeparator", "#@#");

            // for any cateogry entities, include CateogryName also in CategoryPath(only if name is NOT already part of category path)
            if (!String.IsNullOrWhiteSpace(applicationContext.CategoryPath) && !String.IsNullOrWhiteSpace(applicationContext.CategoryName))
            {
                if (applicationContext.EntityTypeId == Constants.CATEGORY_ENTITYTYPE && !applicationContext.CategoryPath.EndsWith(applicationContext.CategoryName))
                {
                    applicationContext.CategoryPath = String.Concat(applicationContext.CategoryPath, categoryPathSeperator, applicationContext.CategoryName);
                }
            }

            if (lookup.Columns.Contains(Constants.LOOKUP_CONTEXT_ORGANIZATION_ID_LIST_COLUMN_NAME)
                || lookup.Columns.Contains(Constants.LOOKUP_CONTEXT_CONTAINER_ID_LIST_COLUMN_NAME)
                || lookup.Columns.Contains(Constants.LOOKUP_CONTEXT_CATEGORY_PATH_LIST_COLUMN_NAME))
            {
                foreach (Row row in rows)
                {
                    if (lookup.CompareWithContext(row, applicationContext))
                        rowCollection.Add(row);
                }
            }
            else
                rowCollection = rows;

            return rowCollection;
        }

        private void PopulateDefaultColumnValues(Row row, Dictionary<String, Object> defaultValues)
        {
            // Fill the default value only in the case of object action is create.
            if (defaultValues != null && defaultValues.Count > 0 && row != null && row.Cells != null && row.Cells.Count > 0)
            {
                if (row.Action == ObjectAction.Create)
                {
                    foreach (KeyValuePair<String, Object> defaultValue in defaultValues)
                    {
                        Cell cell = row.Cells.GetCell(defaultValue.Key);

                        if (cell != null && cell.Value != null && String.IsNullOrWhiteSpace(cell.Value.ToString()))
                        {
                            //In the case the cell value is empty then populate default value
                            cell.Value = defaultValue.Value;
                        }
                    }
                }
            }
        }

        private Dictionary<String, Object> GetDefaultColumnValues(Lookup lookup)
        {
            Dictionary<String, Object> result = new Dictionary<String, Object>();

            if (lookup != null)
            {
                IColumnCollection columns = lookup.GetColumns();

                if (columns != null && columns.Count() > 0)
                {
                    foreach (Column column in columns)
                    {
                        Object defaultValue = column.DefaultValue;

                        if (!(defaultValue == null || String.IsNullOrWhiteSpace(defaultValue.ToString())))
                        {
                            if (!result.ContainsKey(column.Name))
                            {
                                result.Add(column.Name, defaultValue);
                            }
                        }
                    }
                }
            }

            return result;
        }

        private void IgnoreInvalidColumn(Lookup lookup, IColumnCollection validColumns, OperationResult operationResult)
        {
            if (lookup != null && lookup.Columns != null && lookup.Columns.Count > 0 && validColumns != null && validColumns.Count() > 0)
            {
                ColumnCollection invalidColumnNames = new ColumnCollection();
                Collection<String> columnNames = new Collection<String>();

                validColumns.ToList().ForEach(coln =>
                {
                    columnNames.Add(coln.Name.ToLowerInvariant());
                });

                HashSet<String> uniqueColumnSet = new HashSet<String>();

                foreach (Column column in lookup.Columns)
                {
                    if (!columnNames.Contains(column.Name.ToLowerInvariant()) || !uniqueColumnSet.Add(column.Name))
                    {
                        invalidColumnNames.Add(column);
                    }
                }

                if (invalidColumnNames != null && invalidColumnNames.Count > 0)
                {
                    foreach (Column invalidColumn in invalidColumnNames)
                    {
                        lookup.Columns.Remove(invalidColumn);

                        foreach (Row row in lookup.Rows)
                        {
                            row.Cells.Remove(row.GetCell(invalidColumn.Name));
                        }

                        if (uniqueColumnSet.Count > 0 && uniqueColumnSet.Contains(invalidColumn.Name))
                        {
                            String errorMessage = String.Format("Duplicate column '{0}' exists in the '{1}' lookup table for '{2}' locale", invalidColumn.Name, lookup.Name, lookup.Locale.ToString());
                            operationResult.AddOperationResult("113011", errorMessage, new Collection<Object>() { invalidColumn.Name, lookup.Name, lookup.Locale.ToString() }, OperationResultType.Error); ////Duplicate column '{0}' exists in the '{1}' lookup table for '{2}' locale

                            if (!operationResult.ExtendedProperties.Contains("ErrorLevel"))
                            {
                            operationResult.ExtendedProperties.Add("ErrorLevel", "Table");
                            }
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, errorMessage, MDMTraceSource.LookupProcess);
                        }
                        else
                        {
                            String warningMessage = String.Format("Column '{0}' does not exist in the '{1}' lookup table for '{2}' locale", invalidColumn.Name, lookup.Name, lookup.Locale.ToString());
                            operationResult.AddOperationResult("112994", warningMessage, new Collection<Object>() { invalidColumn.Name, lookup.Name, lookup.Locale.ToString() }, OperationResultType.Warning); ////Column '{0}' does not exist in the '{1}' lookup table for '{2}' locale

                            if (!operationResult.ExtendedProperties.Contains("ErrorLevel"))
                            {
                            operationResult.ExtendedProperties.Add("ErrorLevel", "Table");
                            }

                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Warning, warningMessage, MDMTraceSource.LookupProcess);
                            }
                        }
                    }
                }
            }
        }
        #region Translation Methods

        private void EnqueueIntegrationActivity(LookupCollection lookups, CallerContext callerContext)
        {
            DurationHelper durationHelper = new DurationHelper(DateTime.Now);
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Starting integration activity log creation...", MDMTraceSource.LookupProcess);
            }

            try
            {
                IntegrationActivityLogBL integrationActivityLogManager = new IntegrationActivityLogBL();
                integrationActivityLogManager.Create(lookups, "MDMTranslationConnector", "TRANSLATION EXPORT", IntegrationType.Outbound, callerContext);   //TODO:: Add AppConfigs for Connector Name and Integration Message Type Name

                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Integration activity log created", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.LookupProcess);
                }

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Done with integration activity log creation", MDMTraceSource.EntityProcess);
                }
            }
            catch (Exception ex)
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Failed to create integration activity log. Error: {0}", ex.Message), MDMTraceSource.EntityProcess);
                }
            }
        }

        #endregion

        #region Lookup Security Methods

        private Collection<UserAction> PopulateLookupPermissionSet(String lookupTableName, LocaleEnum locale, CallerContext callerContext)
        {
            DataSecurityBL dataSecurityBL = new DataSecurityBL();
            Collection<UserAction> permissionSet = null;

            Lookup securityLookup = Get("Lookup_Security", locale, -1, false, callerContext);

            Permission permission = dataSecurityBL.GetLookupPermission(lookupTableName, securityLookup);

            if (permission != null && permission.PermissionSet != null)
            {
                permissionSet = permission.PermissionSet;
            }

            return permissionSet;
        }

        private Boolean ValidateLookupPermission(Lookup lookup, Collection<UserAction> permissionSet, OperationResult operationResult, RowCollection removeRows)
        {
            Boolean hasLookupPermission = true;

            if (permissionSet != null)
            {
                if (permissionSet.Contains(UserAction.None))
                {
                    hasLookupPermission = false;

                    //Clear all rows
                    lookup.Rows.Clear();

                    Collection<Object> messageParams = new Collection<Object>() { lookup.Name, lookup.Locale };
                    String message = String.Format("Permission Denied for lookup {0} and locale {1}.", messageParams);
                    operationResult.AddOperationResult("114431", message, messageParams, OperationResultType.Error);
                }
                else if (!permissionSet.Contains(UserAction.All))
                {
                    Boolean hasCreateErrorMessageAdded = false;
                    Boolean hasUpdateErrorMessageAdded = false;
                    Boolean hasDeleteErrorMessageAdded = false;

                    foreach (Row row in lookup.Rows)
                    {
                        Boolean addMessage = false;
                        UserAction rowAction = UserAction.Unknown;

                        switch (row.Action)
                        {
                            case ObjectAction.Create: rowAction = UserAction.Add;
                                if (!permissionSet.Contains(rowAction))
                                {
                                    removeRows.Add(row);

                                    if (!hasCreateErrorMessageAdded)
                                    {
                                        addMessage = true;
                                        hasCreateErrorMessageAdded = true;
                                    }
                                }
                                break;
                            case ObjectAction.Delete: rowAction = UserAction.Delete;
                                if (!permissionSet.Contains(rowAction))
                                {
                                    removeRows.Add(row);

                                    if (!hasDeleteErrorMessageAdded)
                                    {
                                        addMessage = true;
                                        hasDeleteErrorMessageAdded = true;
                                    }
                                }
                                break;
                            case ObjectAction.Update: rowAction = UserAction.Update;
                                if (!permissionSet.Contains(rowAction))
                                {
                                    removeRows.Add(row);

                                    if (!hasUpdateErrorMessageAdded)
                                    {
                                        addMessage = true;
                                        hasUpdateErrorMessageAdded = true;
                                    }
                                }
                                break;
        }

                        if (addMessage)
                        {
                            Collection<Object> messageParams = new Collection<Object>() { rowAction.ToString(), lookup.Name, lookup.Locale };
                            String message = String.Format("User do not have permission to {0} row for lookup {1} and locale {2}. Rows with this action have been removed from processing.", messageParams);
                            operationResult.AddOperationResult("114430", message, messageParams, OperationResultType.Error);
                        }
                    }
                }
            }

            return hasLookupPermission;
        }

        #endregion

        #region Implementation of Feature Toggle

        private Boolean IsTranslationEnabled()
        {
            MDMFeatureConfig mdmFeatureConfig = MDMFeatureConfigHelper.GetMDMFeatureConfig(MDMCenterApplication.MDMCenter, "TMSConnector", "1");
            return mdmFeatureConfig.IsEnabled;
        }

        /// <summary>
        /// Log changes to tb_datamodelactivitylog if DA call is sucessfull and appconfig key datamodelactivitylog.enable == true
        /// </summary>
        /// <param name="lookupCollection"></param>
        /// <param name="callerContext"></param>
        private void LogDataModelChanges(LookupCollection lookupCollection, CallerContext callerContext)
        {
            #region step: populate datamodelactivitylog object

            DataModelActivityLogBL dataModelActivityLogManager = new DataModelActivityLogBL();
            DataModelActivityLogCollection activityLogCollection = dataModelActivityLogManager.FillDataModelActivityLogCollection(lookupCollection);

            #endregion step: populate datamodelactivitylog object

            #region step: make api call

            if (activityLogCollection != null) // null activitylog collection means there was error in mapping
            {
                dataModelActivityLogManager.Process(activityLogCollection, callerContext);
            }

            #endregion step: make api call
        }

        private Boolean IsLookupSecurityFeatureEnabled()
        {
            MDMFeatureConfig mdmFeatureConfig = MDMFeatureConfigHelper.GetMDMFeatureConfig(MDMCenterApplication.MDMCenter, "LookupSecurity", "1");
            return mdmFeatureConfig == null ? false : mdmFeatureConfig.IsEnabled;
        }

        #endregion

        #endregion

        #endregion
    }
}