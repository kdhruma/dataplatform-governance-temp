using System;
using System.Data.SqlClient;
using System.Diagnostics;

namespace MDM.AdminManager.Data
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.Utility;   

    /// <summary>
    /// Data Access Layer for MDM Feature Config business objects
    /// </summary>
    public class MDMFeatureConfigDA : SqlClientDataAccessBase
    {
        #region Public Methods
        
        /// <summary>
        /// Gets all MDM Feature Config 
        /// </summary>
        /// <param name="callerContext">context indicates the properties of the caller who called the API like program name,Module</param>
        /// <param name="command">sql command</param>
        /// <returns>Returns MDMFeature Config collection</returns>
        public MDMFeatureConfigCollection GetMDMFeatureConfigCollection(CallerContext callerContext, DBCommandProperties command)
        {

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("MDMFeatureConfigDA.GetMDMFeatureConfigCollection", MDMTraceSource.Configuration, false);

            DurationHelper overallDurationHelper = new DurationHelper(DateTime.Now);
            SqlDataReader reader = null;
            MDMFeatureConfigCollection mdmFeatureConfigs = null;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("AdminManager_SqlParameters");
                SqlParameter[] parameters = generator.GetParameters("AdminManager_Feature_Toggle_Get_ParametersArray");               

                const String storedProcedureName = "usp_AdminManager_Feature_Toggle_Get";
                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    mdmFeatureConfigs = ReadMDMFeatureConfigCollection(reader);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Overall time to load requested MDM feature configs", overallDurationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.EntityProcess);
                    MDMTraceHelper.StopTraceActivity("MDMFeatureConfigDA.GetMDMFeatureConfigCollection", MDMTraceSource.Configuration);
                } 
            }

            return mdmFeatureConfigs;
        }

        /// <summary>
        /// Gets MDM Feature Config by application, module name and version
        /// </summary>
        /// <param name="application">Indicates the Application for requested feature config</param>
        /// <param name="moduleName">Indicates module short name for requested feature config</param>             
        /// <param name="version">Indicates the version  for requested feature config</param>   
        /// <param name="callerContext">context indicates the properties of the caller who called the API like program name,Module</param>
        /// <returns>Returns MDM Feature Config</returns>
        public MDMFeatureConfig GetMDMFeatureConfig(MDMCenterApplication application, String moduleName, String version, CallerContext callerContext, DBCommandProperties command)
        {
            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("MDMFeatureConfigDA.GetMDMFeatureConfig", MDMTraceSource.Configuration, false);

            DurationHelper overallDurationHelper = new DurationHelper(DateTime.Now);
            SqlDataReader reader = null;
            MDMFeatureConfig mdmFeatureConfig = null;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("AdminManager_SqlParameters");
                SqlParameter[] parameters = generator.GetParameters("AdminManager_Feature_Toggle_Get_ParametersArray");

                parameters[0].Value = application;
                parameters[1].Value = moduleName;
                parameters[2].Value = version;

                const String storedProcedureName = "usp_AdminManager_Feature_Toggle_Get";
                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    mdmFeatureConfig = ReadMDMFeatureConfig(reader);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Overall time to load requested MDM feature config", overallDurationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.EntityProcess);
                    MDMTraceHelper.StopTraceActivity("MDMFeatureConfigDA.GetMDMFeatureConfig", MDMTraceSource.Configuration);
                }
            }

            return mdmFeatureConfig;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Reads MDM Feature Configs - Wrapper method to call other read methods
        /// </summary>
        /// <param name="reader">sql reader</param>
        /// <returns>Returns collection of  MDM Feature Configs</returns>
        private MDMFeatureConfigCollection ReadMDMFeatureConfigCollection(SqlDataReader reader)
        {
            MDMFeatureConfigCollection mdmFeatureConfigs = new MDMFeatureConfigCollection();
            while (reader.Read())
            {
                MDMFeatureConfig mdmFeatureConfig = ReadMDMFeatureConfigData(reader); 
                mdmFeatureConfigs.Add(mdmFeatureConfig);
            }

            return mdmFeatureConfigs;
        }              

        /// <summary>
        /// Reads MDM Feature Config - Wrapper method to call other read methods
        /// </summary>
        /// <param name="reader">sql reader</param>
        /// <returns>Returns  MDMFeature Config</returns>
        private MDMFeatureConfig ReadMDMFeatureConfig(SqlDataReader reader)
        {
            MDMFeatureConfig mdmFeatureConfig = null;
            if (reader.Read())
            {
                mdmFeatureConfig = ReadMDMFeatureConfigData(reader);
            }

            return mdmFeatureConfig;
        }

        /// <summary>
        /// Reads MDM Feature Config - Wrapper method to call other read methods
        /// </summary>
        /// <param name="reader">sql reader</param>
        /// <returns>Returns  MDMFeature Config</returns>
        private MDMFeatureConfig ReadMDMFeatureConfigData(SqlDataReader reader)
        {
            MDMFeatureConfig mdmFeatureConfig = new MDMFeatureConfig();            
            Boolean isEnabled = false;
            MDMCenterApplication application = MDMCenterApplication.MDMCenter;
            FeatureConfigDisableLevel featureConfigDisableLevel = FeatureConfigDisableLevel.None;

            if (reader["Application"] != null)
            {
                Enum.TryParse(reader["Application"].ToString(), out application);
                mdmFeatureConfig.Application = application;
            }

            if (reader["Module"] != null)
                mdmFeatureConfig.ModuleName = reader["Module"].ToString();

            if (reader["Version"] != null)
                mdmFeatureConfig.Version = reader["Version"].ToString();

            if (reader["IsEnabled"] != null)
                mdmFeatureConfig.OverriddenValue = ValueTypeHelper.BooleanTryParse(reader["IsEnabled"].ToString(), isEnabled);

            if (reader["IsParentEnabled"] != null)
                mdmFeatureConfig.InheritedValue = ValueTypeHelper.BooleanTryParse(reader["IsParentEnabled"].ToString(), isEnabled);

            if (reader["DisableParent"] != null)
                mdmFeatureConfig.DisabledParent = reader["DisableParent"].ToString();

            if (reader["ModulePath"] != null)
                mdmFeatureConfig.ModulePath = reader["ModulePath"].ToString();

            if (reader["ModuleIdPath"] != null)
                mdmFeatureConfig.ModuleIdPath = reader["ModuleIdPath"].ToString();

            if (reader["DisableLevel"] != null)
            {
                Enum.TryParse(reader["DisableLevel"].ToString(), out featureConfigDisableLevel);
                mdmFeatureConfig.DisableLevel = featureConfigDisableLevel;
            }

            return mdmFeatureConfig;
        }      

        #endregion
    }
}
