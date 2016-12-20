using System;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects.DQMMerging;

    /// <summary>
    /// Exposes methods or properties to set or get rematch-rule.
    /// </summary>
    public interface IRematchRule : ICloneable
    {
        /// <summary>
        /// Property denoting Order
        /// </summary>
        Int32 Order { get; set; }

        /// <summary>
        /// Property denoting RematchRule Conditions
        /// </summary>
        RematchRuleConditionCollection Conditions { get; set; }

        /// <summary>
        /// Property denoting MatchingProfileId
        /// </summary>
        Int32 MatchingProfileId { get; set; }
    }
}