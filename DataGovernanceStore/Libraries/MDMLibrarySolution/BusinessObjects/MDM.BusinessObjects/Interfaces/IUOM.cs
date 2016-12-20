using System;

namespace MDM.Interfaces
{
    using MDM.Core;
    /// <summary>
    /// Exposes methods or properties to set or get the unit of measurement (UOM).
    /// </summary>
    public interface IUOM : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property denoting Key of UOM.
        /// </summary>
        String Key { get; set; }

       

        #endregion

        #region Method

        /// <summary>
        /// Get Xml representation of UOM object
        /// </summary>
        /// <returns>Xml representation of object</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of UOM object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization serialization);

        #endregion
    }
}