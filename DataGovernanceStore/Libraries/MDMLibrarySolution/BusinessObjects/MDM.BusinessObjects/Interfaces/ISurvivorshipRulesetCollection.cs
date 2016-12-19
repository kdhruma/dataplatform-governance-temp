using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get the collection of survivorship rulesets.
    /// </summary>
    public interface ISurvivorshipRulesetCollection : ICollection<SurvivorshipRuleset>, ICloneable
    {
         
    }
}