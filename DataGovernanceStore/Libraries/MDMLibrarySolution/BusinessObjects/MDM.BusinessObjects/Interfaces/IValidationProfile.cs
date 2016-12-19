using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get validation profile related information.
    /// </summary>
    public interface IValidationProfile : IDQMJobProfile
    {
        /// <summary>
        /// Selected for validation DataQualityIndicator ids
        /// </summary>
        Collection<Int16> DataQualityIndicatorIds { get; set; }
    }
}
