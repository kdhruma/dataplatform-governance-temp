using System;
using System.Transactions;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Text;

using MDM.Core;
using MDM.BusinessObjects;
using MDM.Utility;

namespace MDM.AdminManager.Data
{
    public class UserConfigDA : SqlClientDataAccessBase
    {

        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        public void Process(UserConfig userConfig, String userName, String programName)
        {
            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            connectionString = AppConfigurationHelper.ConnectionString;

            SqlParametersGenerator generator = new SqlParametersGenerator("AdminManager_SqlParameters");

            parameters = generator.GetParameters("AdminManager_UserConfig_Process_ParametersArray");

            parameters[0].Value = userConfig.Id;
            parameters[1].Value = userConfig.UserConfigTypeId;
            parameters[2].Value = userConfig.SecurityUserId;
            parameters[3].Value = userConfig.OrgId;
            parameters[4].Value = userConfig.ConfigXml;
            parameters[5].Value = userName;
            parameters[6].Value = programName;
            parameters[7].Value = userConfig.Name;
            parameters[8].Value = userConfig.Action.ToString();

            storedProcedureName = "usp_AdminManager_UserConfig_Process";

            ExecuteProcedureNonQuery(connectionString, parameters, storedProcedureName);
        }
        
        #endregion
    }
}
