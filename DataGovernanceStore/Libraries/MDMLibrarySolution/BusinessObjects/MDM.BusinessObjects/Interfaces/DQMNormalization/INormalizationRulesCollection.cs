using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects.DQMNormalization;

    /// <summary>
    /// Exposes methods or properties to set or get the collection of normalization rules.
    /// </summary>
    public interface INormalizationRulesCollection : ICollection<NormalizationRule>
    {
    }
}