using System;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get MDM feature configuration.
    /// </summary>
    public interface IMDMFeatureConfig 
    {
        #region Properties

        /// <summary>
        /// Property denoting application under MDMCenter
        /// </summary>
        MDMCenterApplication Application { get; set; }

        /// <summary>
        /// Property denoting Module short name
        /// </summary>
        String ModuleName { get; set; }

        /// <summary>
        /// Property denoting Module Path of MDMCenterApplication
        /// </summary>
        String ModulePath { get; set; }

        /// <summary>
        /// Property denoting Module Id Path of MDMCenterApplication
        /// </summary>
        String ModuleIdPath { get; set; }

        /// <summary>
        /// Property denoting Version of feature
        /// </summary>
        String Version { get; set; }

        /// <summary>
        /// Property denoting feature is enable or not
        /// </summary>
        Boolean IsEnabled { get; set; }

        /// <summary>
        /// Property denoting details about feature
        /// </summary>
        MDMFeatureConfigDetailCollection FeatureDetails { get; set; } 

        #endregion 

        #region Methods

        /// <summary>
        /// Xml representation of MDMFeatureConfig object
        /// </summary>
        /// <returns>Xml format of MDMFeatureConfig</returns>
        String ToXml();

        #endregion Methods
    }
}
