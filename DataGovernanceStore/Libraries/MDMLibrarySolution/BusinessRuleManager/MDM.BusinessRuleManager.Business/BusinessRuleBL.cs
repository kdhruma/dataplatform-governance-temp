using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Transactions;

using MDM.Core;
using MDM.BusinessObjects;
using MDM.BusinessRuleManager.Data;

namespace MDM.BusinessRuleManager.Business
{
    public class BusinessRuleBL : BusinessLogicBase
    {
        #region Fields

        private Collection<BusinessRule> businessRules = null;

        #endregion

        #region Constructors

        #endregion

        #region Properties


        #endregion

        #region Methods

        public Collection<BusinessRule> GetAllByUser(String LoginUser)
        {
            businessRules = new Collection<BusinessRule>();

            BusinessRuleDA businessRuleDA = new BusinessRuleDA();
            businessRules = businessRuleDA.Get(LoginUser);
            return businessRules;
        }

        public void Process(Collection<BusinessRule> businessRules,String loginUser,String programName, String action)
        {
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
            {
                BusinessRuleDA businessRuleDA = new BusinessRuleDA();
                businessRuleDA.Process(businessRules, loginUser, programName, action);

                transactionScope.Complete();
            }
        }
        
        /// <summary>
        /// Get collection of BusinessRule based on Context
        /// </summary>
        /// <param name="eventSourceID">Indicates identifier of event source for retrieving business rules</param>
        /// <param name="eventSubscriberID">Indicates identifier of event subscriber for retrieving business rules</param>
        /// <param name="loginUserID">Indicates identifier of login user for retrieving business rules</param>
        /// <param name="loginUserRole">Indicates identifier of login user role for retrieving business rules</param>
        /// <param name="orgID">Indicates identifier of organization for retrieving business rules</param>
        /// <param name="containerID">Indicates identifier of container for retrieving business rules</param>
        /// <param name="entityTypeID">Indicates identifier of entity type for retrieving business rules</param>
        /// <param name="businessRuleTypeIDs">Indicates identifiers of business rule types for retrieving business rules</param>
        /// <returns>Returns collection of BusinessRule</returns>
        public Collection<BusinessRule> GetBusinessRulesByContext(Int32 eventSourceID, Int32 eventSubscriberID, Int32 loginUserID, Int32 loginUserRole, Int32 orgID, Int32 containerID, Int32 entityTypeID, String businessRuleTypeIDs)
        {
            var businessRuleDA = new BusinessRuleDA();
            return businessRuleDA.Get(eventSourceID, eventSubscriberID, loginUserID, loginUserRole, orgID, containerID, entityTypeID, businessRuleTypeIDs);
        }

        public Collection<BusinessRule> GetByContext( Int32 ruleTypeId, Int32 ruleSetId, string xml )
        {
            var businessRuleDA = new BusinessRuleDA();
            return businessRuleDA.Get( ruleTypeId, ruleSetId, xml );
        }

        public Collection<BusinessRule> GetById(IEnumerable<Int32> ruleIds)
        {
            var businessRuleDA = new BusinessRuleDA();
            return businessRuleDA.Get( ruleIds );
        }

        #endregion
    }
}
