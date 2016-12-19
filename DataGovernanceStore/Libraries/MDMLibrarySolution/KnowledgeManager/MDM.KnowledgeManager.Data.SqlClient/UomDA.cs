using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Transactions;
using System.Collections.ObjectModel;

using MDM.Core; 
using MDM.Utility;
using MDM.BusinessObjects;

namespace MDM.KnowledgeManager.Data
{
    public class UomDA : SqlClientDataAccessBase
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods
        public String GetAll()
        {
            StringBuilder returnXml = new StringBuilder();
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;
            SqlParameter[] parameters;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("KnowledgeManager_SqlParameters");

                parameters = generator.GetParameters("KnowledgeManager_UOM_Get_ParametersArray");

                storedProcedureName = "usp_KnowledgeManager_UOM_Get";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        returnXml.Append(reader[0]);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return returnXml.ToString();
        }

        #endregion
    }
}
