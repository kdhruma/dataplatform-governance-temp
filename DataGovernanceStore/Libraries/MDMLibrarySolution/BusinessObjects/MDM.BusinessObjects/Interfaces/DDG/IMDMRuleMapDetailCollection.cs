using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get MDMRule map details.
    /// </summary>
    public interface IMDMRuleMapDetailCollection
    {
        #region Methods

        /// <summary>
        /// Get Xml representation of MDMRule map detail collection
        /// </summary>
        /// <returns>Xml representation of MDMRule map detail collection object</returns>
        String ToXml();

        /// <summary>
        /// Gets MDMRuleMapDetails based on requested event name
        /// </summary>
        /// <param name="eventName">Indicates the event name</param>
        /// <returns>Returns the MDMRuleMapDetailCollection object</returns>
        IMDMRuleMapDetailCollection GetMDMRuleMapDetailsByEventName(String eventName);

        /// <summary>
        /// Gets MDMRuleMapDetails based on requested MDMRule type
        /// </summary>
        /// <param name="ruleType">Indicates the MDMRule type</param>
        /// <returns>Returns the MDMRuleMapDetailCollection object</returns>
        IMDMRuleMapDetailCollection GetMDMRuleMapDetailsByRuleType(MDMRuleType ruleType);

        /// <summary>
        /// Gets MDMRules based on requested MDMRule type
        /// </summary>
        /// <param name="ruleType">Indicates the MDMRule type</param>
        /// <returns>Returns the MDMRules collection object</returns>
        IMDMRuleCollection GetMDMRulesByRuleType(MDMRuleType ruleType);

        #endregion
    }
}
