using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get the collection of relationship denorm processing setting.
    /// </summary>
    public interface IRelationshipDenormProcessingSettingCollection : ICollection<RelationshipDenormProcessingSetting>
    {
        #region Methods

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of Relationship Denorm Processing Setting
        /// </summary>
        /// <returns>Xml representation of Relationship Denorm Processing Setting</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of Relationship Denorm Processing Setting based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Relationship Denorm Processing Setting</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion ToXml methods

        #endregion Methods
    }
}
