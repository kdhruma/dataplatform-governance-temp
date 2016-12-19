using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects.DQMMerging;

    /// <summary>
    /// Exposes methods or properties to set or get the collection of range rules.
    /// </summary>
    public interface IRangeRuleCollection : ICollection<RangeRule>
    {
    }
}