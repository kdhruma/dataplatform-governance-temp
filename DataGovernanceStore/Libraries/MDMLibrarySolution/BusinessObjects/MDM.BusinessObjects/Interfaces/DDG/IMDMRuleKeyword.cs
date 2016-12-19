using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Interface for MDMRuleKeyword
    /// </summary>
    public interface IMDMRuleKeyword : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property denotes whether the rule is enabled or not
        /// </summary>
        Boolean IsEnabled  { get; set; }

        /// <summary>
        /// Property denotes syntax
        /// </summary>
        String Syntax  { get; set; }
        
        /// <summary>
        /// Property denotes keyword sample
        /// </summary>
        String Sample { get; set; }

        /// <summary>
        /// Property denotes description (or description's localization message code) for Rule Keyword
        /// </summary>
        String Description { get; set; }

        /// <summary>
        /// Property denotes Parent Keyword Group Id
        /// </summary>
        Int32 ParentKeywordGroupId { get; set; }

        /// <summary>
        /// Property denotes Parent Keyword Group name
        /// </summary>
        String ParentKeywordGroupName { get; set; }

        /// <summary>
        /// Property denotes whether the Parent Keyword Group is enabled or not
        /// </summary>
        Boolean IsParentKeywordGroupEnabled { get; set; }
 
        #endregion Properties
    }
}