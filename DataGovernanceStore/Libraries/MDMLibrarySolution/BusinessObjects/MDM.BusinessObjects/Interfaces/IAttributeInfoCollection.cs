using System;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties used for providing attributeinfo collection related information.
    /// </summary>
    public interface IAttributeInfoCollection
    {
        #region Properties

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets XML representation of attributeinfo collection object
        /// </summary>
        /// <returns>XML representation of attributeinfo collection object</returns>
        String ToXml();

        #endregion Methods
    }
}
