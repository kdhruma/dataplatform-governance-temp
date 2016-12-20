using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

using MDM.Core;
using MDM.Utility;
using MDMBO = MDM.BusinessObjects;

namespace MDM.KnowledgeManager.Data
{
    public class TimeZoneDA : SqlClientDataAccessBase
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        public Collection<MDMBO.TimeZone> GetAll()
        {
            Collection<MDMBO.TimeZone> timeZones = null;
            SqlDataReader reader = null;

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("KnowledgeManager_SqlParameters");

                parameters = generator.GetParameters("KnowledgeManager_TimeZone_GetAll_ParametersArray");

                storedProcedureName = "usp_KnowledgeManager_TimeZone_GetAll";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                timeZones = new Collection<MDMBO.TimeZone>();
                while (reader.Read())
                {
                    Object[] values = new Object[reader.FieldCount];
                    reader.GetValues(values);
                    MDMBO.TimeZone timeZone = new MDMBO.TimeZone(values);
                    timeZones.Add(timeZone);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return timeZones;
        }

        #endregion
    }
}
