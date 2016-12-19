using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects.DQM;

    /// <summary>
    /// Exposes methods or properties to set or get the collection of normalization results.
    /// </summary>
    public interface INormalizationResultsCollection : ICollection<NormalizationResult>
    {
        /// <summary>
        /// Property denoting name for NormalizationProfile
        /// </summary>
        String ProfileName { get; set; }
    }
}