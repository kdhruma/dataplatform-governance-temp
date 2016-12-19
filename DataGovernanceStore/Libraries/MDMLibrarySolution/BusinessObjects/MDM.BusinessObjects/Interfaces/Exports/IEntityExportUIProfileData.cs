using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using MDM.Core;

namespace MDM.Interfaces.Exports
{
    /// <summary>
    /// Exposes methods or properties to set or get the export profile related information.
    /// </summary>
    public interface IEntityExportUIProfileData : IEntityExportProfileData
    {
        #region Properties

        /// <summary>
        /// Property denoting the common attribute ids.
        /// </summary>
        String CommonAttributeIds { get; set; }

        /// <summary>
        /// Property denoting to whether include all common attributes or not.
        /// </summary>
        Boolean IncludeAllCommonAttributeIds { get; set; }

        /// <summary>
        /// Property denoting to whether include all category attributes or not.
        /// </summary>
        Boolean IncludeAllCategoryAttributeIds { get; set; }

        /// <summary>
        /// Property denoting the category attribute ids.
        /// </summary>
        String CategoryAttributeIds { get; set; }

        /// <summary>
        /// Property denoting the locale id list.
        /// </summary>
        String LocaleIds { get; set; }

        /// <summary>
        /// Property denoting to whether include all relationship types or not.
        /// </summary>
        Boolean IncludeAllRelationshipTypeIds { get; set; }

        /// <summary>
        /// Property denoting the relationship type ids.
        /// </summary>
        String RelationshipTypeIds { get; set; }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// represents ExportProfileData in Xml format 
        /// </summary>
        /// <returns>The result string</returns>
        new String ToXml();

        /// <summary>
        /// represents ExportProfileData in Xml format 
        /// </summary>
        /// <returns>The result string</returns>
        new String ToXml(ObjectSerialization objectSerialization);

        #endregion

        #endregion
    }
}
