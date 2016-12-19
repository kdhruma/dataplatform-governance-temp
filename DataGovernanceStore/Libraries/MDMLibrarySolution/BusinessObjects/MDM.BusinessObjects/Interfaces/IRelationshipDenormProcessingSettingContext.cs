using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get the context for the relationship denorm processing setting.
    /// </summary>
    public interface IRelationshipDenormProcessingSettingContext : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property specifying OrganizationIds
        /// </summary>
        Collection<Int32> OrganizationIds { get; set; }

        /// <summary>
        /// Property specifying ContainerIds
        /// </summary>
        Collection<Int32> ContainerIds { get; set; }

        /// <summary>
        /// Property specifying EntityTypeIds
        /// </summary>
        Collection<Int32> EntityTypeIds { get; set; }

        /// <summary>
        /// Property specifying CategoryIds
        /// </summary>
        Collection<Int64> CategoryIds { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Get Xml representation of Relationship Denorm ProcessingSettingContext
        /// </summary>
        /// <returns>Xml representation of Relationship Denorm ProcessingSettingContext</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of Relationship Denorm ProcessingSettingContext based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Relationship Denorm ProcessingSettingContext</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion Methods
    }
}
