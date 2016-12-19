using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get collection of entity variant rule attribute.
    /// </summary>
    public interface IEntityVariantRuleAttributeCollection : IEnumerable<EntityVariantRuleAttribute>
    {
        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of entity variant rule attribute collection
        /// </summary>
        /// <param name="needValues">Represents a boolean value indicating if attribute value details are required in xml representation.</param>
        /// <returns>Xml representation of entity variant rule attribute collection</returns>
        String ToXml(bool needValues);

        /// <summary>
        /// Get Xml representation of Entity variant rule attribute Collection based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Indicates type of the serialization option</param>
        /// <param name="needValues">Represents a boolean value indicating if attribute value details are required in xml representation.</param>
        /// <returns>Returns Xml representation of entity variant rule attribute collection</returns>
        String ToXml(ObjectSerialization objectSerialization, bool needValues);

        #endregion
    }
}
