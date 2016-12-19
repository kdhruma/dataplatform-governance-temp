using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get rematch rule conditions.
    /// </summary>
    public interface IRematchRuleCondition : ICloneable
    {
        /// <summary>
        /// Property denoting Aggregate Function
        /// </summary>
        MatchResultSetAggregateFunction AggregateFunction { get; set; }

        /// <summary>
        /// Property denoting Operator
        /// </summary>
        SearchOperator Operator { get; set; }

        /// <summary>
        /// Property denoting Value
        /// </summary>
        Double Value { get; set; }
    }
}