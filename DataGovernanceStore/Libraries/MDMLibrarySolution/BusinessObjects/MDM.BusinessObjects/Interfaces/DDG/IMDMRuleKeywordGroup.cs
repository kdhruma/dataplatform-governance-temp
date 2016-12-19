using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Interface for MDMRuleKeywordGroup
    /// </summary>
    public interface IMDMRuleKeywordGroup : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property denotes whether the rule is enabled or not
        /// </summary>
        Boolean IsEnabled  { get; set; }

        #endregion Properties
    }
}