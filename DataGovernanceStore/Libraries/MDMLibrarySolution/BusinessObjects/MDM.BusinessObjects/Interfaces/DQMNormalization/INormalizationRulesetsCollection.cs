using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects.DQMNormalization;

    /// <summary>
    /// Exposes methods or properties to set or get the collection of normalization rulesets. 
    /// </summary>
    public interface INormalizationRulesetsCollection : ICollection<NormalizationRuleset>
    {
    }
}