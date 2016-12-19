using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get the menu properties.
    /// </summary>
    public interface IMenu : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property denoting Parent Menu ID
        /// </summary>
        Int32 MenuParentId { get; set; }

        /// <summary>
        /// Property denoting sequence of the Menu
        /// </summary>
        Int32 Sequence { get; set; }

        /// <summary>
        /// Property denoting Link to Menu
        /// </summary>
        String Link { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of Menu object
        /// </summary>  
        /// <returns>Xml representation of object</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of Menu object
        /// </summary>
        /// <param name="objectSerialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion
    }
}
