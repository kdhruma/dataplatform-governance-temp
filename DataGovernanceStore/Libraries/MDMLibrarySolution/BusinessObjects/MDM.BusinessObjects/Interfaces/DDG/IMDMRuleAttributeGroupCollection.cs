using System;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get MdmRule attribute Group collection.
    /// </summary>
    public interface IMDMRuleAttributeGroupCollection
    {
        #region Methods

        /// <summary>
        /// Get Xml representation of MDMRule Attribute Group
        /// </summary>
        /// <returns>Xml representation of MDMRule Attribute Group object</returns>
        String ToXml();

        #endregion Methods
    }
}
