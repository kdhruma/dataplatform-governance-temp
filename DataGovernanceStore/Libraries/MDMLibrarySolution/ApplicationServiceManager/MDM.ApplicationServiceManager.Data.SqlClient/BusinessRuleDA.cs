using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDM.Core;
using System.Data.SqlClient;
using System.Collections.ObjectModel;

namespace MDM.ApplicationServiceManager.Data
{
    using MDM.Utility;

    /// <summary>
    /// Class having data access methods for core Business rule
    /// </summary>
    public class BusinessRuleDA : SqlClientDataAccessBase
    {
        /// <summary>
        /// Get unique id based on configuration
        /// </summary>
        /// <param name="objectType">Object type for which we want Unique ID</param>
        /// <param name="organizationId">Org Id : Used for context</param>
        /// <param name="containerId">Container Id : Used for context</param>
        /// <param name="categoryId">Category Id : Used for context</param>
        /// <param name="entityTypeId">Entity type Id : Used for context</param>
        /// <param name="relationshipTypeId">Relationship type Id : Used for context</param>
        /// <param name="locale">Locale : Used for context</param>
        /// <param name="roleId">Role of current user Id : Used for context</param>
        /// <param name="userId">User Id : Used for context</param>
        /// <param name="noOfUIdsToGenerate">Indicates how many unique ids are to be generated</param>
        /// <returns>collection of string having auto ids generated from DB</returns>
        public Collection<String> GetUniqueId( ObjectType objectType, Int32 organizationId, Int32 containerId, Int64 categoryId, Int32 entityTypeId, 
            Int32 relationshipTypeId, String locale, Int32 roleId, Int32 userId, Int32 noOfUIdsToGenerate)
        {
            SqlDataReader reader = null;
            Collection<String> returnValue = new Collection<string>();

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("ApplicationServiceManager_SqlParameters");

                parameters = generator.GetParameters("ApplicationServiceManager_BusinessRule_UniqueId_Get_ParametersArray");

                parameters[0].Value = objectType.ToString();
                parameters[1].Value = organizationId;
                parameters[2].Value = containerId;
                parameters[3].Value = categoryId;
                parameters[4].Value = entityTypeId;
                parameters[5].Value = relationshipTypeId;
                parameters[6].Value = locale;
                parameters[7].Value = roleId;
                parameters[8].Value = userId;
                parameters[9].Value = noOfUIdsToGenerate;
                

                storedProcedureName = "usp_Utility_UniqueID_Get";
                if ( !String.IsNullOrWhiteSpace(AppConfigurationHelper.StoredProcedureSuffix) )
                    storedProcedureName = String.Format("{0}_{1}", storedProcedureName, AppConfigurationHelper.StoredProcedureSuffix);

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                if(reader != null)
                {
                    while ( reader.Read() )
                    {
                        if ( !reader.IsDBNull(0) )
                        {
                            if ( reader["UniqueId"] != null )
                            {
                                returnValue.Add(reader["UniqueId"].ToString());
                            }
                        }
                    }
                }
            }
            finally
            {
                if ( reader != null )
                    reader.Close();
            }
            return returnValue;
        }
    }
}
