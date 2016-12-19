using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace MDM.LookupManager.Data
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Utility;

    /// <summary>
    /// Specifies data access operations for Lookup with audit information
    /// </summary>
    public class LookupAuditInfoDA : SqlClientDataAccessBase
    {
        #region Fields

        #endregion Fields

        #region Constructors

        #endregion Constructors

        #region Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gets lookup audit information data for the requested lookup
        /// </summary>
        /// <param name="lookup">Indicates lookup for which needs to export audit info</param>
        /// <param name="command">Indicates object which having command properties</param>
        public void PopulateLookupAuditInfo(Lookup lookup, DBCommandProperties command)
        {
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            StringBuilder xml = new StringBuilder();

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("LookupManager_SqlParameters");

                parameters = generator.GetParameters("LookupManager_LookupAuditInfo_Get_ParametersArray");

                parameters[0].Value = lookup.Name;

                storedProcedureName = "usp_LookupManager_LookupAuditInfo_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    Dictionary<Int64, Dictionary<String, String>> lookupRowsAuditInfo = new Dictionary<Int64, Dictionary<String, String>>();

                    while (reader.Read())
                    {
                        Int32 rowId = 0;
                        Dictionary<String, String> auditInfo = new Dictionary<String, String>();

                        if (reader["RowId"] != null)
                        {
                            rowId = ValueTypeHelper.Int32TryParse(reader["RowId"].ToString(), 0);
                        }

                        if (reader["CreatedDateTime"] != null)
                        {
                            auditInfo.Add("CreatedDateTime", reader["CreatedDateTime"].ToString());
                        }

                        if (reader["CreatedUserName"] != null)
                        {
                            auditInfo.Add("CreatedUserName", reader["CreatedUserName"].ToString());
                        }

                        if (reader["LastModifiedDateTime"] != null)
                        {
                            auditInfo.Add("LastModifiedDateTime", reader["LastModifiedDateTime"].ToString());
                        }

                        if (reader["LastModifiedUserName"] != null)
                        {
                            auditInfo.Add("LastModifiedUserName", reader["LastModifiedUserName"].ToString());
                        }

                        lookupRowsAuditInfo.Add(rowId, auditInfo);
                    }

                    if (lookupRowsAuditInfo != null && lookupRowsAuditInfo.Count > 0)
                    {
                        if (lookup.Rows != null && lookup.Rows.Count > 0)
                        {
                            foreach (Row row in lookup.Rows)
                            {
                                Dictionary<String, String> rowAuditfInfo = null;
                                lookupRowsAuditInfo.TryGetValue(row.Id, out rowAuditfInfo);

                                if (rowAuditfInfo != null)
                                {
                                    row.ExtendedProperties = new Hashtable();

                                    foreach (KeyValuePair<String, String> info in rowAuditfInfo)
                                    {
                                        row.ExtendedProperties.Add(info.Key, info.Value);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        #endregion Public Methods

        #endregion Methods
    }
}
