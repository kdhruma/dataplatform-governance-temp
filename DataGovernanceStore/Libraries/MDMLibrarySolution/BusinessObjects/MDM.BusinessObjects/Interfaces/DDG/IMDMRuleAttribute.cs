using System;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get MdmRule attribute.
    /// </summary>
    public interface IMDMRuleAttribute
    {
        #region Methods

        /// <summary>
        /// Get Xml representation of MDMRule Attribute
        /// </summary>
        /// <returns>Xml representation of MDMRule Attribute object</returns>
        String ToXml();

        #endregion Methods
    }
}
