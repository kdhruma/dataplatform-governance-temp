using System;

namespace MDM.ConfigurationManager.Business
{
    using MDM.BusinessObjects;
    using MDM.CacheManager.Business;
    using MDM.ConfigurationManager.Data;
    using MDM.Core;
    using MDM.Utility;
    using MDM.ExceptionManager;

    /// <summary>
    /// Class to manage connection string related configuration. (Used for multiple physical servers)
    /// </summary>
    public class ConnectionStringBL : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// Specifies the name of the culture in the context
        /// </summary>
        private String _cultureName = String.Empty;


        /// <summary>
        /// Specifies security principal for user.
        /// </summary>
        private SecurityPrincipal _securityPrincipal = null;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property denoting name of the culture in the context
        /// </summary>
        private String CultureName
        {
            get
            {
                if ( String.IsNullOrWhiteSpace(_cultureName) )
                {
                    _cultureName = "en-US";

                    if ( _securityPrincipal != null && _securityPrincipal.UserPreferences != null && !String.IsNullOrWhiteSpace(_securityPrincipal.UserPreferences.UICultureName) )
                        _cultureName = _securityPrincipal.UserPreferences.UICultureName;
                }

                return _cultureName;
            }
        }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public ConnectionStringBL()
        {
            GetSecurityPrincipal();
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get connection string for given module and action.
        /// </summary>
        /// <param name="application">Application for which connection string is to be fetched.</param>
        /// <param name="module">Module for which connection string is to be fetched.</param>
        /// <param name="action">Action to be performed.</param>
        /// <returns>Connection string</returns>
        public String Get(MDMCenterApplication application, MDMCenterModules module, MDMCenterModuleAction action)
        {
            String connectionString = String.Empty;
            String cacheKey = this.GetCacheKey(application, module, action);

            #region Get connection string from cache

            ICache cache = CacheFactory.GetCache();
            if ( cache != null )
            {
                connectionString = cache.Get<String>(cacheKey);
            }

            #endregion Get connection string from cache.

            //Get connection string from DB if not found in cache.
            if ( String.IsNullOrWhiteSpace(connectionString) )
            {
                //Get command
                DBCommandProperties command = new DBCommandProperties();
                command.ConnectionString = AppConfigurationHelper.ConnectionString;

                //In case connection string is not found based on configuration, fall back to connection string from web.config or other configuration file
                connectionString = command.ConnectionString; 

                ConnectionProperties connectionProp = null;
                ConnectionStringDA configDA = new ConnectionStringDA();
                connectionProp = configDA.Get(application, module, action, command);

                //Get connection string from connectionProperties object
                if ( connectionProp != null )
                {
                    //Read AppConfig to check if database windows authentication is enabled
                    try
                    {
                        String windowsAuthEnabledStr = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.PhysicalServerSplit.Database.WindowsAuthentication.Enabled");
                        connectionProp.IsWindowsAuthentication = ValueTypeHelper.ConvertToBoolean(windowsAuthEnabledStr);
                    }
                    catch ( Exception ex )
                    {
                        LogException(ex);
                    }
                    connectionString = connectionProp.GetConnectionString();
                }

                #region Put connection string back in cache

                if ( cache != null && !String.IsNullOrWhiteSpace(connectionString) )
                {
                    cache.Set(cacheKey, connectionString, DateTime.Now.AddHours(24.0));
                }

                #endregion Put connection string back in cache
            }
            return connectionString;
        }

        #region Private Methods

        private void GetSecurityPrincipal()
        {
            if ( _securityPrincipal == null )
            {
                _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            }
        }

        private String GetCacheKey(MDMCenterApplication application, MDMCenterModules module, MDMCenterModuleAction action)
        {
            String cacheKey = "RS_ConnectionString_Application:{0}_Module:{1}_Action:{2}";
            cacheKey = String.Format(cacheKey, application.ToString(), module.ToString(), action.ToString());
            return cacheKey;
        }

        private static void LogException(Exception ex)
        {
            ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
        }

        #endregion Private Methods

        #endregion Methods
    }
}
