using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects.DQMNormalization;

    /// <summary>
    /// Exposes methods or properties to set or get the collection of normalization profiles.
    /// </summary>
    public interface INormalizationProfilesCollection : ICollection<NormalizationProfile>
    {
    }
}