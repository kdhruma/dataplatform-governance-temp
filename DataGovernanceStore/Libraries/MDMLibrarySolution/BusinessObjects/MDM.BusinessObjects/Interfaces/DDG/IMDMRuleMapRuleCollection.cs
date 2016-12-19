using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// 
    /// </summary>
    public interface IMDMRuleMapRuleCollection : ICollection<MDMRuleMapRule>
    {
        #region Properties

        #endregion Properties

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ruleMapName"></param>
        /// <returns></returns>
        IMDMRuleMapRuleCollection FilterMDMRuleMapRulesByRuleMapName(String ruleMapName);

        #endregion Methods
    }
}
