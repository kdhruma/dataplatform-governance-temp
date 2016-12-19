using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Represents the interface which contains collection of MDMRuleMaps
    /// </summary>
    public interface IMDMRuleMapCollection : ICollection<MDMRuleMap>
    {
        /// <summary>
        /// Get Xml representation of MDMRuleMap Collection
        /// </summary>
        /// <returns>Returns xml representation of MDMRuleMapCollection</returns>
        String ToXml();

        /// <summary>
        /// Compares MDMRuleMapCollection with current collection
        /// </summary>
        /// <param name="subSetMDMRuleMapCollection">Indicates MDMRuleMapCollection to be compare with the current collection</param>
        /// <returns>Returns True : If both are same. False : otherwise</returns>
        Boolean IsSuperSetOf(MDMRuleMapCollection subSetMDMRuleMapCollection);

        /// <summary>
        /// Determines whether the current MDMRuleMapCollection contains mdmRuleMap with the specified name.
        /// </summary>
        /// <param name="mdmRuleMapName">RuleMapName to be verified</param>
        /// <returns>True if found in collection else false</returns>
        Boolean Contains(String mdmRuleMapName);
    }
}