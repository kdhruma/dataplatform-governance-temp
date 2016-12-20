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
    public class ValidationStateDA : SqlClientDataAccessBase
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        public Collection<ValidationState> Get(Int64 EntityID, String AttributeIDList)
        {
            Collection<ValidationState> Data = null;
            SqlDataReader reader = null;

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            connectionString = AppConfigurationHelper.ConnectionString;
            try
            {

                SqlParametersGenerator generator = new SqlParametersGenerator("BusinessRuleManager_SqlParameters");

                parameters = generator.GetParameters("BusinessRuleManager_ValidationState_Get_ParametersArray");

                parameters[0].Value = EntityID;
                parameters[1].Value = AttributeIDList;

                storedProcedureName = "usp_CNode_ValidationState_AttrVal";
                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);
                Data = new Collection<ValidationState>();
                while (reader.Read())
                {
                    Object[] Values = new Object[reader.FieldCount];
                    reader.GetValues(Values);
                    ValidationState validationState = new ValidationState(Values);
                    Data.Add(validationState);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return Data;
        }


        #endregion
    }
}
