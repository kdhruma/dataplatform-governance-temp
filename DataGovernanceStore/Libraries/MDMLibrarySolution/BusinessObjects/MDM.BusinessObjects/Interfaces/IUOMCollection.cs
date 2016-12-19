using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods used for defining the collection of UOMs.
    /// </summary>
    public interface IUOMCollection
    {
        #region Properties

        #endregion

        #region ToXml Methods

        /// <summary>
        /// Get Xml representation of UOMCollection object
        /// </summary>
        /// <returns>Xml string representing the UOMCollection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of UOMCollection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>   
        /// <returns>Xml string representing the UOMCollection</returns>
        String ToXml(ObjectSerialization serialization);

        /// <summary>
        /// Add UOM object in collection
        /// </summary>
        /// <param name="item">IUOM to add in collection</param>
        void Add(IUOM item);

        #endregion
    }
}
