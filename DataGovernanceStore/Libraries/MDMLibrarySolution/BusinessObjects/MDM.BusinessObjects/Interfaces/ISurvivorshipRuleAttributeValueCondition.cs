using System;

namespace MDM.BusinessObjects.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Specifies interface for SurvivorshipRuleAttributeValueCondition
    /// </summary>
    public interface ISurvivorshipRuleAttributeValueCondition
    {
        /// <summary>
        /// Property denotes TargetAttributeId
        /// </summary>
        Int64 TargetAttributeId { get; set; }

        /// <summary>
        /// Property denotes Operator
        /// </summary>
        SearchOperator Operator { get; set; }

        /// <summary>
        /// Property denotes search value
        /// </summary>
        String Value { get; set; }
    }
}
