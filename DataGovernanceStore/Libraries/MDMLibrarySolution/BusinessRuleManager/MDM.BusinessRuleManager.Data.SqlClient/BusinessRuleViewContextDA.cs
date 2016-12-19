using System;
using System.Text;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Transactions;

using MDM.Core;
using MDM.BusinessObjects;
using MDM.Utility;

namespace MDM.BusinessRuleManager.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class BusinessRuleViewContextDA : SqlClientDataAccessBase
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        public Int32 ProcessContext(String paramXML, String loginUser, String programName)
        {
            Int32 output = 0; //success

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
            {
                SqlParameter[] parameters;
                String connectionString = String.Empty;
                String storedProcedureName = String.Empty;

                connectionString = AppConfigurationHelper.ConnectionString;

                SqlParametersGenerator generator = new SqlParametersGenerator("BusinessRuleManager_SqlParameters");

                parameters = generator.GetParameters("BusinessRuleManager_BusinessRule_Context_Process_ParametersArray");

                parameters[0].Value = paramXML;
                parameters[1].Value = loginUser;
                parameters[2].Value = programName;

                storedProcedureName = "usp_BusinessRuleManager_BusinessRule_Context_Process";

                output = ExecuteProcedureNonQuery(connectionString, parameters, storedProcedureName);

                transactionScope.Complete();
            }
            return output;
        }

        #endregion
    }
}
