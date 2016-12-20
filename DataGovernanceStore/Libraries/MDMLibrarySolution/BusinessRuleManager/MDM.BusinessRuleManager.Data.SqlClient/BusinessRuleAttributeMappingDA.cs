using System;
using System.Text;
using System.Transactions;
using System.Data.SqlClient;
using System.Collections.ObjectModel;


using MDM.Core;
using MDM.Utility;
using MDM.BusinessObjects;

namespace MDM.BusinessRuleManager.Data
{
    public class BusinessRuleAttributeMappingDA : SqlClientDataAccessBase
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Process Business Attribute View mapping.
        /// </summary>
        /// <param name="listMappings">Collection of objects to save</param>
        /// <param name="loginUser"></param>
        /// <param name="programName"></param>
        /// <returns></returns>
        public Int32 Process(Collection<BusinessRuleAttributeMapping> listMappings, String loginUser, String programName, String ViewID)
        {
            Int32 output = 0; //success

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
            {
                SqlParameter[] parameters;
                String connectionString = String.Empty;
                String storedProcedureName = String.Empty;

                connectionString = AppConfigurationHelper.ConnectionString;

                String paramXML = ConvertToXML(listMappings, ViewID);

                SqlParametersGenerator generator = new SqlParametersGenerator("BusinessRuleManager_SqlParameters");

                parameters = generator.GetParameters("BusinessRuleManager_ViewAttributeMapping_Process_ParametersArray");

                parameters[0].Value = paramXML;
                parameters[1].Value = loginUser;
                parameters[2].Value = programName;

                storedProcedureName = "usp_BusinessRuleManager_ViewAttributeMapping_Process";

                output = ExecuteProcedureNonQuery(connectionString, parameters, storedProcedureName);

                transactionScope.Complete();
            }

            return output;
        }

        /// <summary>
        /// Get View attribute mapping based on ViewID (BusinessRule_RuleID)
        /// </summary>
        /// <param name="businessRule_RuleID">All attributes mapped with this ViewID (BusinessRule_RuleID) will be fetched.</param>
        /// <returns></returns>
        public Collection<BusinessRuleAttributeMapping> GetByBusinessRuleID(Int32 businessRule_RuleID)
        {
            Collection<BusinessRuleAttributeMapping> data = null;
            SqlDataReader reader = null;
            
            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("BusinessRuleManager_SqlParameters");

                parameters = generator.GetParameters("BusinessRuleManager_ViewAttributeMapping_Get_ParametersArray");

                parameters[0].Value = businessRule_RuleID;
                
                storedProcedureName = "usp_BusinessRuleManager_ViewAttributeMapping_Get";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                data = new Collection<BusinessRuleAttributeMapping>();
                while (reader.Read())
                {
                    Object[] values = new Object[reader.FieldCount];
                    reader.GetValues(values);
                    BusinessRuleAttributeMapping businessRuleAttributeMapping = new BusinessRuleAttributeMapping(values);
                    data.Add(businessRuleAttributeMapping);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return data;
        }
        #endregion

        #region Private Methods

        private String ConvertToXML(Collection<BusinessRuleAttributeMapping> listMappings, String ViewID)
        {
            String xml = String.Format("<ViewAttributeMappings BusinessRuleId=\"{0}\">", ViewID);

            StringBuilder stringBuilder = new StringBuilder(xml);

            foreach (BusinessRuleAttributeMapping mapping in listMappings)
            {
                stringBuilder.Append(mapping.ToXML());

            }
            stringBuilder.Append("</ViewAttributeMappings>");

            return stringBuilder.ToString();
        }

        #endregion

        #endregion
    }
}
