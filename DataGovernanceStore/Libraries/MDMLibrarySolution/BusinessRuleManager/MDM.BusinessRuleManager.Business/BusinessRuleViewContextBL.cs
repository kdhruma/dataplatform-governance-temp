using System;
using System.Collections.ObjectModel;
using System.Transactions;

using MDM.Core;
using MDM.BusinessObjects;
using MDM.BusinessRuleManager.Data;
 
namespace MDM.BusinessRuleManager.Business
{
    public class BusinessRuleViewContextBL :  BusinessLogicBase
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        public void ProcessContext(Collection<BusinessRuleSetRule> businessRuleSetRules, String loginUser, String programName, String action)
        {
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
            {
                string ContextXML = "";
                BusinessRuleViewContextDA businessRuleViewContextDA = new BusinessRuleViewContextDA();
                //BusinessRuleDA businessRuleDA = new BusinessRuleDA();
                ContextXML = GenerateContextXML(businessRuleSetRules, action);
                businessRuleViewContextDA.ProcessContext(ContextXML, loginUser, programName);
                //businessRuleDA.ProcessContext(ContextXML, loginUser, programName);

                transactionScope.Complete();
            }
        }

        private String GenerateContextXML(Collection<BusinessRuleSetRule> businessRuleSetRules, String action)
        {
            int count = businessRuleSetRules.Count;
            String xml = "";
            String Context = "";
            String Permission = "";
            string Permissions = "";
            if (action.ToLower() == "add")
                xml = "<BusinessRuleContext Action=\"ADD\"";
            else if (action.ToLower() == "update")
                xml = "<BusinessRuleContext Action=\"UPDATE\"";
            else if (action.ToLower() == "delete")
                xml = "<BusinessRuleContext Action=\"DELETE\"";
            Context = "ID=\"{0}\" Name=\"{1}\" FK_Org=\"{2}\" FK_Catalog=\"{3}\" FK_NodeType=\"{4}\" RuleSetID=\"{5}\" RuleID=\"{6}\" >";

            Context = string.Format(Context, businessRuleSetRules[0].BusinessRuleViewContextId, businessRuleSetRules[0].BusinessRuleViewContextName, businessRuleSetRules[0].OrgId, businessRuleSetRules[0].ContainerId, businessRuleSetRules[0].EntityTypeId, businessRuleSetRules[0].BusinessRuleSetId, businessRuleSetRules[0].BusinessRuleId);

            //return retXML;

            for (int i = 0; i < count; i++)
            {
                Permission = "<Permission FK_SecurityUser=\"{0}\" FK_SecurityRole=\"{1}\" />";
                Permission = string.Format(Permission, businessRuleSetRules[i].SecurityUserId, businessRuleSetRules[i].SecurityRoleId);
                Permissions = Permissions + Permission;
            }

            xml = xml + " " + Context + Permissions + "</BusinessRuleContext>";
            return xml;
        }


        #endregion
    }
}
