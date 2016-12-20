using System;
using System.Diagnostics;
using System.Transactions;
using System.Data.SqlClient;

namespace MDM.AdminManager.Data
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.Utility;

    /// <summary>
    /// Data Access Layer for AppConfig business objects
    /// </summary>
    public class AppConfigDA : SqlClientDataAccessBase
    {
        #region Public Methods

        public AppConfigCollection Get(Int32 appConfigId, String searchValue)
        {
            //MDMTraceHelper.StartTraceActivity("AppConfigDA.GetAll", false);

            SqlDataReader reader = null;
            AppConfigCollection appConfigs;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("AdminManager_SqlParameters");

                SqlParameter[] parameters = generator.GetParameters("AdminManager_AppConfig_Get_ParametersArray");

                parameters[0].Value = "AppConfig";
                parameters[1].Value = appConfigId;
                parameters[2].Value = searchValue;
                parameters[3].Value = 0;

                String storedProcedureName = "usp_Sec_Object_Get";

                reader = ExecuteProcedureReader(AppConfigurationHelper.ConnectionString, parameters, storedProcedureName);

                appConfigs = ReadAppConfigs(reader);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                //MDMTraceHelper.StopTraceActivity("AppConfigDA.GetAll");
            }

            return appConfigs;
        }

        public OperationResult Process(AppConfig appConfig, String programName, String userName)
        {
            //MDMTraceHelper.StartTraceActivity("AppConfigDA.Process", false);

            OperationResult operationResult = new OperationResult();
            SqlParametersGenerator generator = new SqlParametersGenerator("AdminManager_SqlParameters");
            String connectionString = AppConfigurationHelper.ConnectionString;

            const String storedProcedureName = "usp_AdminManager_AppConfig_Process";

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                
                try
                {
                    SqlParameter[] parameters =
                        generator.GetParameters("AdminManager_AppConfig_Process_ParametersArray");

                    parameters[0].Value = appConfig.Id;
                    parameters[1].Value = appConfig.Name;
                    parameters[2].Value = appConfig.Value;
                    parameters[3].Value = appConfig.Description;
                    parameters[4].Value = appConfig.LongDescription;
                    parameters[5].Value = appConfig.ValidationRule;
                    parameters[6].Value = appConfig.ValidationMethod;
                    parameters[7].Value = appConfig.Domain;
                    parameters[8].Value = appConfig.Client;
                    parameters[9].Value = appConfig.RowSourceType;
                    parameters[10].Value = appConfig.RowSource;
                    parameters[11].Value = userName;
                    parameters[12].Value = programName;

                    ExecuteProcedureNonQuery(connectionString, parameters, storedProcedureName);

                    //Need empty information to make sure correct operation result status is calculated.
                    operationResult.AddOperationResult(String.Empty, String.Empty, OperationResultType.Information);

                    transactionScope.Complete();

                }
                catch (Exception exception)
                {
                    //MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "AppConfig Process Failed." + exception.Message);
                    operationResult.AddOperationResult(exception.Message, String.Empty, OperationResultType.Error);
                }

            }
            //MDMTraceHelper.StopTraceActivity("AppConfigDA.Process");

            return operationResult;
        }

        #endregion

        #region Private methods

        private AppConfigCollection ReadAppConfigs(SqlDataReader reader)
        {
            AppConfigCollection result = null;

            if (reader != null)
            {
                result = new AppConfigCollection();
                while (reader.Read())
                {
                    Object[] values = new Object[reader.FieldCount];
                    reader.GetValues(values);
                    AppConfig appConfig = new AppConfig(values);
                    result.Add(appConfig);
                }
            }

            return result;
        }
        
        #endregion
    }
}
