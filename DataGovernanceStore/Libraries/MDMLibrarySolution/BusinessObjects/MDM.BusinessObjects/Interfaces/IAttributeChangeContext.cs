using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties used for providing attribute change context related information.
    /// </summary>
    public interface IAttributeChangeContext
    {
        #region Properties

        /// <summary>
        /// Specifies the action for based on attribute change context.
        /// </summary>
        ObjectAction Action { get; set; }

        /// <summary>
        /// Returns the attributeinfo collection available in attribute change context
        /// </summary>
        /// <returns>AttributeInfo collection</returns>
        IAttributeInfoCollection GetAttributeInfoCollection();

        /// <summary>
        /// Sets the attributeinfo collection in attribute change context
        /// </summary>
        /// <param name="iAttributeInfoCollection">Indicates the attributeinfo collection</param>
        void SetAttributeInfoCollection(IAttributeInfoCollection iAttributeInfoCollection);

        #endregion Properties

        #region Methods

        #region ToXml Methods

        /// <summary>
        /// Gets XML representation of attribute change context object
        /// </summary>
        /// <returns>XML representation of attribute change context object</returns>
        String ToXml();

        #endregion ToXml Methods

        #endregion Methods
    }
}