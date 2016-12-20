using System;
using System.Transactions;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Text;

using MDM.Core;
using MDM.BusinessObjects;
using MDM.Utility;

namespace MDM.KnowledgeManager.Data
{
    public class LocaleDA : SqlClientDataAccessBase
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public LocaleCollection GetAvailableLocales()
        {
            LocaleCollection locales = null;
            SqlDataReader reader = null;

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("KnowledgeManager_SqlParameters");

                parameters = generator.GetParameters("KnowledgeManager_Locale_AvailableLocales_Get_ParametersArray");

                storedProcedureName = "usp_KnowledgeManager_Locale_AvailableLocales_Get";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                locales = new LocaleCollection();
                while (reader.Read())
                {
                    Object[] values = new Object[reader.FieldCount];
                    reader.GetValues(values);
                    Locale locale = new Locale(values);
                    locales.Add(locale);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return locales;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localeId"></param>
        /// <returns></returns>
        public LocaleCollection Get(int localeId)
        {
            LocaleCollection locales = new LocaleCollection();

            Locale locale = null;
            SqlDataReader reader = null;

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("KnowledgeManager_SqlParameters");

                parameters = generator.GetParameters("KnowledgeManager_Locale_Get_ParametersArray");
                parameters[0].Value = localeId;

                storedProcedureName = "usp_KnowledgeManager_Locale_Get";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    Object[] values = new Object[reader.FieldCount];
                    reader.GetValues(values);
                    locale = new Locale(values);
                    locales.Add(locale);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return locales;
        }
        #endregion
    }
}
