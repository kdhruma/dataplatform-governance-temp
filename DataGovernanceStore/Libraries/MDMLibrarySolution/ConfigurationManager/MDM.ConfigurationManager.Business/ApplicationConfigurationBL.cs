using System;
using System.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MDM.ConfigurationManager.Business
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Utility;
    using MDM.ConfigurationManager.Data;
    using ILocalizable = MDM.Interfaces.ILocalizable;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the business operations for Application Configurations
    /// </summary>
    public class ApplicationConfigurationBL : BusinessLogicBase
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gets application configurations for the requested context parameters.
        /// </summary>
        /// <param name="applicationConfiguration">Indicates instance of applicationConfiguration for which key-value to be returned.</param>
        /// <returns>Object containing all the configurations for the requested context in the key-value pair</returns>
        public Dictionary<String, String> GetApplicationConfigurations(IApplicationConfiguration applicationConfiguration)
        {
            Dictionary<String, String> configurationXmlDictionary = null;

            // Get configurations
            applicationConfiguration.GetConfigurations();

            //Prepare dictionary
            configurationXmlDictionary = new Dictionary<String, String>();

            List<IConfigurationObject> items = applicationConfiguration.GetItems();

            if (applicationConfiguration != null && items != null && items.Count > 0)
            {
                foreach (IConfigurationObject configObject in items)
                {
                    IObject innerConfigObject = configObject.GetObject();
                    if (innerConfigObject is ILocalizable && applicationConfiguration is ILocalizable)
                    {
                        ((ILocalizable) innerConfigObject).UILocale = ((ILocalizable) applicationConfiguration).UILocale;
                    }

                    IApplicationContext context = configObject.GetApplicationContext();

                    if (innerConfigObject != null && context != null)
                    {
                        configurationXmlDictionary.Add(context.GetEventSubscriberName(), innerConfigObject.ToXml());
                    }
                }
            }

            return configurationXmlDictionary;
        }

        /// <summary>
        /// Get application configuration Locale key-value pair.
        /// </summary>
        /// <param name="applicationConfiguration">Indicates instance of applicationConfiguration for which key-value to be returned.</param>
        /// <returns>Returns LocaleConfig key Value pair</returns>
        public Dictionary<String, ILocaleConfig> GetLocaleApplicationConfigurations(IApplicationConfiguration applicationConfiguration)
        {
            Dictionary<String, ILocaleConfig> configurationXmlDictionary = null;

            // Get configurations
            applicationConfiguration.GetConfigurations();

            //Prepare dictionary
            configurationXmlDictionary = new Dictionary<String, ILocaleConfig>();

            List<IConfigurationObject> items = applicationConfiguration.GetItems();
            if (applicationConfiguration != null && items != null && items.Count > 0)
            {
                foreach (IConfigurationObject configObject in items)
                {
                    IObject innerConfigObject = configObject.GetObject();

                    IApplicationContext context = configObject.GetApplicationContext();

                    if (innerConfigObject != null && context != null && innerConfigObject is ILocaleConfig)
                    {
                        configurationXmlDictionary.Add(context.GetEventSubscriberName(), (ILocaleConfig)innerConfigObject);
                    }
                }
            }

            return configurationXmlDictionary;
        }

        /// <summary>
        /// Returns Application Configuration scripts as per the provided where clause
        /// </summary>
        /// <param name="whereClause">Where clause having filter data</param>
        /// <returns>Returns datatable containing scripts</returns>
        public DataTable GetApplicationConfigScripts(String whereClause)
        {
            DataTable result = null;

            ApplicationConfigurationDA applicationConfigDA = new ApplicationConfigurationDA();
            result = applicationConfigDA.GetApplicationConfigScripts(whereClause);

            return result;
        }

        /// <summary>
        /// Update the custom application config based on the input application config excel file
        /// </summary>
        /// <param name="applicationConfigFullPath">Indicates the application config full path of excel file</param>
        /// <param name="callerContext">Indicates caller context indicating application and module</param>
        public OperationResult ProcessExtendedApplicationConfig(String applicationConfigFullPath, CallerContext callerContext)
        {
            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Update);
            return ProcessExtendedApplicationConfig(applicationConfigFullPath, command, callerContext);
        }

        /// <summary>
        /// Update the custom application config based on the input application config excel file
        /// </summary>
        /// <param name="applicationConfigFullPath">Indicates the application config full path of excel file</param>
        /// <param name="dbCommandProperties"></param>
        /// <param name="callerContext">Indicates caller context indicating application and module</param>
        public OperationResult ProcessExtendedApplicationConfig(String applicationConfigFullPath, DBCommandProperties dbCommandProperties, CallerContext callerContext)
        {
            OperationResult configProcessOperationResult = new OperationResult();

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("ApplicationConfigBL.ProcessExtendedApplicationConfig", MDMTraceSource.Application, false);
            }

            try
            {
                DataSet dsApplicationConfig = null;
                ApplicationConfigurationDA applicationConfigDA = new ApplicationConfigurationDA();

                dsApplicationConfig = PopulateApplicationConfigDataSet(applicationConfigFullPath);
                configProcessOperationResult = applicationConfigDA.ProcessExtendedApplicationConfig(dsApplicationConfig, dbCommandProperties, callerContext);
            }
            finally
            {
                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("ApplicationConfigBL.ProcessExtendedApplicationConfig", MDMTraceSource.Application);
                }
            }

            return configProcessOperationResult;
        }

        #endregion

        #region Private Methods

        private DataSet PopulateApplicationConfigDataSet(string applicationConfigFullPath)
        {
            DataSet dsConfig = new DataSet("ConfigDataSet");

            //TODO: Need to have configuration of these sheet names
            Collection<String> sheetNames = new Collection<String>();

            sheetNames.Add("Extended Application Configs");
            sheetNames.Add("ToolBar Button Config");
            sheetNames.Add("Grid Columns Config");
            sheetNames.Add("PanelBar Config");
            sheetNames.Add("Explorer Search Attributes");
            
            if(sheetNames.Count > 0)
            {
                foreach(string sheetName in sheetNames)
                {
                    DataSet dataSet = ExternalFileReader.ReadExternalFileSAXParsing(applicationConfigFullPath, sheetName, false);
                    
                    if(dataSet!= null && dataSet.Tables != null && dataSet.Tables.Count > 0)
                    {
                        dataSet.Tables[0].TableName = sheetName;
                        dsConfig.Merge(dataSet.Tables[0]);
                    }
                }
            }

            return dsConfig;
        }

        #endregion

        #endregion
    }
}
