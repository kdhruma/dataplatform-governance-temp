using System;
using System.Collections.Generic;
using System.Linq;
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
    public class BusinessRuleDA : SqlClientDataAccessBase
    {
        #region Fields

        private readonly SqlParametersGenerator _paramGenerator;

        #endregion

        #region Constructors

        public BusinessRuleDA()
        {
            _paramGenerator = new SqlParametersGenerator( "BusinessRuleManager_SqlParameters" );
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        public Collection<BusinessRule> Get(String LoginUser)
        {
            Collection<BusinessRule> Data = null;
            SqlDataReader reader = null;

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            connectionString = AppConfigurationHelper.ConnectionString;
            try
            {
                parameters = _paramGenerator.GetParameters("BusinessRuleManager_BusinessRule_Get_ParametersArray");

                parameters[0].Value = LoginUser;
                storedProcedureName = "usp_BusinessRuleManager_BusinessRule_GetByUser";
                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);
                Data = new Collection<BusinessRule>();
                while (reader.Read())
                {
                    Object[] Values = new Object[reader.FieldCount];
                    reader.GetValues(Values);
                    BusinessRule businessRule = new BusinessRule(Values);
                    Data.Add(businessRule);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return Data;
        }

        public Collection<BusinessRule> Get(Int32 EventSourceID, Int32 EventSubscriberID, Int32 LoginUserID, Int32 LoginUserRole, Int32 OrgID, Int32 ContainerID, Int32 EntityTypeID, String BusinessRuleTypeIDs)
        {
            Collection<BusinessRule> Data = null;
            SqlDataReader reader = null;

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            connectionString = AppConfigurationHelper.ConnectionString;
            try
            {
                parameters = _paramGenerator.GetParameters("BusinessRuleManager_BusinessRule_GetByContext_ParametersArray");
                parameters[0].Value = EventSourceID;
                parameters[1].Value = EventSubscriberID;
                parameters[2].Value = LoginUserRole;
                parameters[3].Value = LoginUserID;
                parameters[4].Value = OrgID;
                parameters[5].Value = ContainerID;
                parameters[6].Value = EntityTypeID;
                parameters[7].Value = BusinessRuleTypeIDs;
                storedProcedureName = "usp_BusinessRuleManager_BusinessRule_GetByContext";
                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);
                Data = new Collection<BusinessRule>();
                while (reader.Read())
                {
                    Object[] Values = new Object[reader.FieldCount];
                    reader.GetValues(Values);
                    BusinessRule businessRule = new BusinessRule(Values);
                    Data.Add(businessRule);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return Data;
        }

        public Collection<BusinessRule> Get(Int32 ruleTypeId, Int32 ruleSetID, String xml )
        {
            var parameters = _paramGenerator.AssignParameters(
                "BusinessRuleManager_BusinessRule_GetCustomByContext_ParametersArray",
                xml, ruleTypeId, ruleSetID);

            return GetBusinessRules("usp_BusinessRuleManager_BusinessRule_GetCustomByContext", parameters);
        }

        public Collection<BusinessRule> Get( IEnumerable<Int32> ruleIds )
        {
            String ruleIdList = String.Join(",", ruleIds.Select(id => id.ToString()).ToArray());

            var parameters = _paramGenerator.AssignParameters(
                "BusinessRuleManager_BusinessRule_GetById_ParametersArray",
                ruleIdList );

            return GetBusinessRules("usp_BusinessRuleManager_BusinessRule_GetById", parameters);
        }

        public Int32 Process( Collection<BusinessRule> listBusinessRules, String loginUser, String programName, String action )
        {
            Int32 output = 0; //success

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
            {
                SqlParameter[] parameters;
                String connectionString = String.Empty;
                String storedProcedureName = String.Empty;

                connectionString = AppConfigurationHelper.ConnectionString;

                String paramXML = ConvertToXML(listBusinessRules,action);

                parameters = _paramGenerator.GetParameters("BusinessRuleManager_BusinessRule_Process_ParametersArray");

                parameters[0].Value = paramXML;
                parameters[1].Value = loginUser;
                parameters[2].Value = programName;

                storedProcedureName = "usp_BusinessRuleManager_BusinessRule_Process";

                output = ExecuteProcedureNonQuery(connectionString, parameters, storedProcedureName);

                transactionScope.Complete();
            }
            return output;
        }

        private Collection<BusinessRule> GetBusinessRules(String procedure, SqlParameter[] parameters)
        {
            return ExecuteProcedureCollection(
                AppConfigurationHelper.ConnectionString,
                parameters,
                procedure,
                values => new BusinessRule( values ) );
        }

        private String ConvertToXML( Collection<BusinessRule> listBusinessRules, String action )
        {
            String xml="";
            if(action.ToLower()=="add")
             xml= "<BusinessRule Action=\"ADD\">";
            else if(action.ToLower()=="update")
                xml= "<BusinessRule Action=\"UPDATE\">";
            else if(action.ToLower()=="delete")
                xml = "<BusinessRule Action=\"DELETE\">";

            StringBuilder stringBuilder = new StringBuilder(xml);
            foreach (BusinessRule businessRule in listBusinessRules)
            {
                stringBuilder.Append(businessRule.ToXML());
            }
            stringBuilder.Append("</BusinessRule>");
            return stringBuilder.ToString();
        }

        #endregion
    }
}
