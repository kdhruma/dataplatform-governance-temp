using System;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get MdmRule entity extension context.
    /// </summary>
    public interface IMDMRuleEntityExtensionContext
    {
        #region Methods

        /// <summary>
        /// Get Xml representation of MDMRule entity extension context object
        /// </summary>
        /// <returns>Xml representation of MDMRule entity extension context object</returns>
        String ToXml();

        #endregion
    }
}
