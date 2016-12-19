using System;

namespace MDM.BusinessObjects.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Specifies interface for SurvivorshipRuleStrategyPriority
    /// </summary>
    public interface ISurvivorshipRuleStrategyPriority
    {
        /// <summary>
        /// Property denotes Strategy
        /// </summary>
        RulesetStrategy Strategy { get; set; }

        /// <summary>
        /// Property denotes Priority
        /// </summary>
        Int32 Priority { get; set; }
    }
}
