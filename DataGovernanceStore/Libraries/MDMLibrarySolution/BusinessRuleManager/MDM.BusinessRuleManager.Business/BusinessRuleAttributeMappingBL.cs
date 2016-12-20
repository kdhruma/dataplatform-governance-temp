using System;
using System.Transactions;
using System.Collections.ObjectModel;

using MDM.Core;
using MDM.BusinessObjects;
using MDM.BusinessRuleManager.Data;
using MDM.Utility;

namespace MDM.BusinessRuleManager.Business
{
    public class BusinessRuleAttributeMappingBL : BusinessLogicBase
    {
        #region Fields

        private Collection<BusinessRuleAttributeMapping> businessRuleAttributeMapping = null;

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Process Business Attribute View mapping.
        /// </summary>
        /// <param name="businessRuleAttributeMappings"> Collection of objects to save</param>
        public void Process(Collection<BusinessRuleAttributeMapping> businessRuleAttributeMappings, String ViewID)
        {
            String userName = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
            {
                BusinessRuleAttributeMappingDA businessRuleAttributeMappingDA = new BusinessRuleAttributeMappingDA();
                businessRuleAttributeMappingDA.Process(businessRuleAttributeMappings, userName, "BusinessRuleAttributeMappingBL", ViewID);

                transactionScope.Complete();
            }
        }

        /// <summary>
        /// Get View attribute mapping based on ViewID (businessRuleID)
        /// </summary>
        /// <param name="businessRuleID">All attributes mapped with this ViewID (businessRuleID) will be fetched.</param>
        /// <returns></returns>
        public Collection<BusinessRuleAttributeMapping> GetByBusinessRuleID(Int32 businessRuleID)
        {
            businessRuleAttributeMapping = new Collection<BusinessRuleAttributeMapping>();

            BusinessRuleAttributeMappingDA businessRuleAttributeMappingDA = new BusinessRuleAttributeMappingDA();
            businessRuleAttributeMapping = businessRuleAttributeMappingDA.GetByBusinessRuleID(businessRuleID);

            return businessRuleAttributeMapping;
        }
        #endregion
    }
}
