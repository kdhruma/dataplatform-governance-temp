using System;
using System.Data.SqlClient;

namespace MDM.ConfigurationManager.Data
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Utility;

    /// <summary>
    /// Specifies the MDMEvent Information data access class
    /// </summary>
    public class MDMEventInfoDA : SqlClientDataAccessBase
    {
        #region Methods

        /// <summary>
        /// Get all the MDMEvents from the system
        /// </summary>
        /// <param name="command">Indicates the Data base command properties</param>
        /// <returns>Returns the list of MDMEvents</returns>
        public MDMEventInfoCollection Get(DBCommandProperties command)
        {
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            MDMEventInfoCollection mdmEvents = null;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("ConfigurationManager_SqlParameters");
                parameters = generator.GetParameters("ConfigurationManager_MDMEventInfo_Get_ParametersArray");

                String connectionString = command.ConnectionString;
                storedProcedureName = "usp_ConfigurationManager_Event_Get";
                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                mdmEvents = this.FillEventInformations(reader);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return mdmEvents;
        }

        #region Private Methods

        private MDMEventInfoCollection FillEventInformations(SqlDataReader reader)
        { 
            MDMEventInfoCollection mdmEvents = null;

            if (reader != null)
            {
                mdmEvents = new MDMEventInfoCollection();

                while (reader.Read())
                {
                    MDMEventInfo mdmEvent = new MDMEventInfo();

                    if (reader["Id"] != null)
                    {
                        mdmEvent.Id = ValueTypeHelper.Int32TryParse(reader["Id"].ToString(), 0);
                    }

                    if (reader["Name"] != null)
                    {
                        mdmEvent.Name = reader["Name"].ToString();
                    }

                    if (reader["EventManagerClassName"] != null)
                    {
                        mdmEvent.EventManagerClassName = reader["EventManagerClassName"].ToString();
                    }

                    if (reader["Description"] != null)
                    {
                        mdmEvent.Description = reader["Description"].ToString();
                    }

                    if (reader["AlternateEventId"] != null)
                    {
                        mdmEvent.AlternateEventInfoId = ValueTypeHelper.Int32TryParse(reader["AlternateEventId"].ToString(), 0);
                    }

                    if (reader["IsBusinessRuleSupported"] != null)
                    {
                        mdmEvent.HasBusinessRuleSupport = ValueTypeHelper.BooleanTryParse(reader["IsBusinessRuleSupported"].ToString(), false);
                    }

                    if (reader["IsInternal"] != null)
                    {
                        mdmEvent.IsInternal = ValueTypeHelper.BooleanTryParse(reader["IsInternal"].ToString(), false);
                    }

                    if (reader["IsObsolete"] != null)
                    {
                        mdmEvent.IsObsolete = ValueTypeHelper.BooleanTryParse(reader["IsObsolete"].ToString(), false);
                    }

                    if (reader["AssemblyName"] != null)
                    {
                        mdmEvent.AssemblyName = reader["AssemblyName"].ToString();
                    }

                    if (reader["EventName"] != null)
                    {
                        mdmEvent.EventName = reader["EventName"].ToString();
                    }

                    mdmEvents.Add(mdmEvent);
                }
            }

            return mdmEvents;
        }

        #endregion Private Methods

        #endregion Methods
    }
}
