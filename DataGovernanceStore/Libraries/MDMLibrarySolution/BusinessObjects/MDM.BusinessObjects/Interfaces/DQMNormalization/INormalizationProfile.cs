
namespace MDM.Interfaces
{
    using MDM.BusinessObjects.DQMNormalization;

    /// <summary>
    /// Exposes methods or properties to set or get normalization profile information.
    /// </summary>
    public interface INormalizationProfile : IDQMJobProfile
    {
        /// <summary>
        /// Rule sets collection
        /// </summary>
        NormalizationRulesetsCollection RulesetsCollection { get; set; }
    }
}