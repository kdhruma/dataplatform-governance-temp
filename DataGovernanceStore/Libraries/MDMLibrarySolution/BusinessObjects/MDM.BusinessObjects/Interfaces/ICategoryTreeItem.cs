using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get category tree item.
    /// </summary>
    public interface ICategoryTreeItem : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Field for the Id of a Category Tree Item
        /// </summary>
        new Int64 Id { get; set; }

        /// <summary>
        /// Property denoting the Id of a parent Category Tree Item
        /// </summary>
        Int64 ParentId { get; set; }

        /// <summary>
        /// Property denoting the path of the Category Tree Item
        /// </summary>
        String Path { get; set; }

        /// <summary>
        /// Property denoting the path of the Category Tree Item
        /// </summary>
        String IdPath { get; set; }

        /// <summary>
        /// Property denoting the child items count of the Category Tree Item
        /// </summary>
        Int64 ChildCount { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of Category Tree Item
        /// </summary>
        /// <returns>Xml representation of Category Tree Item</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of Category Tree Item based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Category Tree Item</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion
    }
}