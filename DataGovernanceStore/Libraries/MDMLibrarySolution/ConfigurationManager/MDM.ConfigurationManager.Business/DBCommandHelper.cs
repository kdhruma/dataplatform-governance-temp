using System;

namespace MDM.ConfigurationManager.Business
{
    using MDM.BusinessObjects;
    using MDM.CacheManager.Business;
    using MDM.Core;
    using MDM.ExceptionManager;
    using MDM.Utility;

    /// <summary>
    /// A helper class for fetching db command related information like connection string, command time out and other.
    /// </summary>
    public class DBCommandHelper
    {
        #region Methods

        /// <summary>
        /// Get DBCommand based on module
        /// </summary>
        /// <param name="application">MDMSystem application for which command information is to be fetched</param>
        /// <param name="module">MDMSystem module for which command information is to be fetched.</param>
        /// <param name="action">Action to be performed on module</param>
        /// <returns>DBCommand populated with db related information like connection string, command time out etc.</returns>
        public static DBCommandProperties Get( MDMCenterApplication application, MDMCenterModules module , MDMCenterModuleAction action )
        {
            DBCommandProperties command = null;
            String connectionString = String.Empty;
            String cacheKey = "RS_DBCommandProperties_Application:{0}_Module:{1}_Action:{2}";
            cacheKey = String.Format(cacheKey, application.ToString(), module.ToString(), action.ToString());

            #region Read cached command object

            ICache cache = CacheFactory.GetCache();
            if ( cache != null )
            {
                command = cache.Get<DBCommandProperties>(cacheKey);
            }

            #endregion Read cached command object

            //Cached Command object is not found.
            //Check if server split is enabled and based on that get command properties from either db or default values.
            if ( command == null )
            {
                command = new DBCommandProperties();

                #region Read overall key to turn on/off the server split feature or data routing 

                //Read AppConfig and check if server split or data routing is enabled.
                Boolean serverSplitEnabled = false;
                Boolean dataRoutingEnabled = false;

                try
                {
                    String splitEnabled = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.PhysicalServerSplit.Enabled");
                    Boolean.TryParse(splitEnabled, out serverSplitEnabled);
                }
                catch ( Exception ex )
                {
                    LogException(ex);
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error, ex.Message);
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Information, "Value will be considered as false");
                }

                try
                {
                    String strDataRoutingEnabled = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.DataRouting.Enabled");
                    Boolean.TryParse(strDataRoutingEnabled, out dataRoutingEnabled);
                }
                catch ( Exception ex )
                {
                    LogException(ex);
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error, ex.Message);
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Information, "Value will be considered as false");
                }


                #endregion Read overall key to turn on/off the server split feature

                //If server split or data routing is enabled then only read connection string and other information
                if(serverSplitEnabled == true || dataRoutingEnabled == true)
                {
                    #region Read connection string

                    ConnectionStringBL connectionStringBL = new ConnectionStringBL();
                    connectionString = connectionStringBL.Get(application, module, action);

                    #endregion Read connection string
                }

                #region Default value initialization

                //If connection string is not found then read the default connection string from config file.
                if ( String.IsNullOrWhiteSpace(connectionString) )
                {
                    connectionString = AppConfigurationHelper.ConnectionString;
                }
                command.ConnectionString = connectionString;

                #endregion Default value initialization

                #region Put populated cache object back in cache

                if ( cache != null )
                {
                    cache.Set<DBCommandProperties>(cacheKey, command, DateTime.Now.AddHours(24));
                }

                #endregion Put populated cache object back in cache
            }

            return command;
        }

        /// <summary>
        /// Get DBCommand based on module
        /// </summary>
        /// <param name="context">Context which tells which application/module called this API</param>
        /// <param name="action">Action to be performed on module</param>
        /// <returns>DBCommand populated with db related information like connection string, command time out etc.</returns>
        public static DBCommandProperties Get(CallerContext context, MDMCenterModuleAction action)
        {
            return DBCommandHelper.Get(context.Application, context.Module, action);
        }


        #region Private Methods

        private static void LogException( Exception ex )
        {
            ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
        }

        #endregion Private Methods

        #endregion Methods
    }
}
