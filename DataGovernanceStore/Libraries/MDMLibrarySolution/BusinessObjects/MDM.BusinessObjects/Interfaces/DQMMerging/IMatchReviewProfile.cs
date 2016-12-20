using System;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects.DQMMerging;

    /// <summary>
    /// Exposes methods or properties to set or get match review profiles.
    /// </summary>
    public interface IMatchReviewProfile : IMDMObject, ICloneable
    {
        /// <summary>
        /// Indicates the MatchResultSetRules object
        /// </summary>
        RematchRuleCollection MatchResultSetRules { get; set; }

        /// <summary>
        /// Indicates the MatchResultsRule object
        /// </summary>
        MergePlanningRule MatchResultsRule { get; set; }
    }
}