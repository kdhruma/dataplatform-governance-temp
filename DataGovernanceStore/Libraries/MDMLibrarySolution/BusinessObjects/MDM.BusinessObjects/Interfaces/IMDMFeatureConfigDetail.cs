using System;


namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get MDM feature configuration related information.
    /// </summary>
    public interface IMDMFeatureConfigDetail : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property denoting application type under MDMCenter
        /// </summary>
        ApplicationType Type { get; set; }

        /// <summary>
        /// Property denoting Version of feature
        /// </summary>
        String Version { get; set; }

        /// <summary>
        /// Property denoting file name
        /// </summary>
        String FileName { get; set; }

        /// <summary>
        /// Property for TechnologyVersion of MDMFeatureConfigDetail
        /// </summary>
        String TechnologyVersion { get; set; }

        /// <summary>
        /// Property for IsDefault of MDMFeatureConfigDetail
        /// </summary>
        Boolean IsDefault { get; set; }

        /// <summary>
        /// Property for IsEnabled of MDMFeatureConfigDetail
        /// </summary>
        Boolean IsEnabled { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Xml representation of MDMFeatureConfig object
        /// </summary>
        /// <returns>Xml format of MDMFeatureConfig</returns>
        String ToXml();

        #endregion Methods
    }
}
