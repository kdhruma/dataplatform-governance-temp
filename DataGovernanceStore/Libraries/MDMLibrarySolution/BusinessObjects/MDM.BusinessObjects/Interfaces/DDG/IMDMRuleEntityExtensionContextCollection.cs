using System;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get MdmRule entity extension context collection.
    /// </summary>
    public interface IMDMRuleEntityExtensionContextCollection
    {
        #region Methods

        /// <summary>
        /// Get Xml representation of MDMRule entity extension context collection object
        /// </summary>
        /// <returns>Xml representation of MDMRule entity extension context collection object</returns>
        String ToXml();

        #endregion Methods
    }
}
