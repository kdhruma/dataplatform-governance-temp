using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using System.Diagnostics;

namespace MDM.KnowledgeManager.Business
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.KnowledgeManager.Data;
    using MDM.CacheManager.Business;
    using MDM.Utility;
    using MDM.BufferManager;

    /// <summary>
    /// Specifies Locale class
    /// </summary>
    public class LocaleBL : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// Field denoting locale data access
        /// </summary>
        private LocaleDA localeDA = new LocaleDA();

        /// <summary>
        /// Field denoting locale buffer manager class
        /// </summary>
        private LocaleBufferManager _localeBufferManager = new LocaleBufferManager();

        #endregion

        #region Constructors
        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        public Collection<LocaleEnum> GetAvailableLocaleValues()
        {
            
            Collection<LocaleEnum> localeValues = new Collection<LocaleEnum>();

            LocaleCollection locales = this.GetAvailableLocales();

            foreach (Locale locale in locales)
                localeValues.Add(locale.Locale);

            return localeValues;
        }

        /// <summary>
        /// Get all the organization mapped locales
        /// </summary>
        /// <returns>collection of locales </returns>
        public LocaleCollection GetAvailableLocales()
        {
            LocaleCollection locales = null;

            try
            {

                #region Get Available Locales from Cache if available

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "Finding available locales data into cache.");

                locales = _localeBufferManager.FindAvailableLocales();

                #endregion

                if (locales == null)
                {
                    if (Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "Requested available locales data is not available in cache.");
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "Getting available locales  data from database...");
                    }

                    locales = localeDA.GetAvailableLocales();

                    if (locales != null && locales.Count > 0)
                    {
                        //Order by locale long name only if collection have more than one locale.
                        if (locales.Count > 1)
                        {
                            locales = new LocaleCollection(locales.OrderBy(l => l.LongName).ToList());
                        }

                        #region Cache locales data

                        if (Constants.TRACING_ENABLED)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "Caching available locales data..");

                        //Cache locales
                        _localeBufferManager.UpdateAvailableLocales(locales, 3);

                        if (Constants.TRACING_ENABLED)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "Caching completed.");

                        #endregion
                    }
                }
                else
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "Requested available locales data loaded from cache.");
                }
            }
            finally
            {
                //if (Constants.TRACING_ENABLED)
               // MDMTraceHelper.StopTraceActivity("KnowledgeManager.GetAvailableLocales");
            }

            return locales;
        }

        /// <summary>
        /// Get all the locales in system
        /// </summary>
        /// <returns>collection of locales </returns>
        public LocaleCollection GetAll(Boolean loadFresh = false)
        {
            // Note: localeId = 0 means get all locales in system
            Int32 localeId = 0;
            LocaleCollection localeList = null;

            if (!loadFresh)
            {
                localeList = _localeBufferManager.GetAllLocales();
            }

            if (localeList == null)
            {
                LocaleDA localeDA = new LocaleDA();
                localeList = localeDA.Get(localeId);
                localeList = new LocaleCollection(localeList.OrderBy(l => l.LongName).ToList());
                
                // set locale collection into cache
                if (localeList != null && localeList.Count > 0)
                {
                    _localeBufferManager.UpdateAllLocales(localeList, 3);
                }
            }

            return localeList;
        }

        /// <summary>
        /// Get locale for given locale id
        /// </summary>
        /// <param name="localeId">This parameter is specifying locale id.</param>
        /// <returns>locale object by locale id</returns>
        public Locale Get(int localeId)
        {
            Locale locale = null;

            // Get all locales(GetAll uses cache internally)
            LocaleCollection locales = GetAll();

            var requestedLocales = from l in locales
                                   where l.Id == localeId
                                   select l;

            if (requestedLocales != null && requestedLocales.Any())
                locale = requestedLocales.First();

            return locale;
        }

        /// <summary>
        /// Get locale for given locale name
        /// </summary>
        /// <param name="localeId">This parameter is specifying locale name.</param>
        /// <returns>locale object by locale name</returns>
        public Locale Get(String localeName)
        {
            Locale locale = null;

            // Get all locales(GetAll uses cache internally)
            LocaleCollection locales = GetAll();

            var requestedLocales = from l in locales
                                   where l.Name == localeName
                                   select l;

            if (requestedLocales != null && requestedLocales.Any())
                locale = requestedLocales.First();

            return locale;
        }

        /// <summary>
        /// Loads all locales into cache
        /// </summary>
        public Boolean LoadLocales()
        {
            Boolean loaded = true;

            LocaleCollection locales = GetAll();

            return loaded;
        }

        #endregion

        #region Private Methods
        #endregion

        #endregion
    }
}