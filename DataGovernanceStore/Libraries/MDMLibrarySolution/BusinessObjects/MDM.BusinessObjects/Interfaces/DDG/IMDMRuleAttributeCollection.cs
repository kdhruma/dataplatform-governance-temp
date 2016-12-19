using System;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get MdmRule attributes.
    /// </summary>
    public interface IMDMRuleAttributeCollection
    {
        #region Methods

        /// <summary>
        /// Get Xml representation of MDMRule Attributes
        /// </summary>
        /// <returns>Xml representation of MDMRule Attributes object</returns>
        String ToXml();

        #endregion Methods
    }
}
