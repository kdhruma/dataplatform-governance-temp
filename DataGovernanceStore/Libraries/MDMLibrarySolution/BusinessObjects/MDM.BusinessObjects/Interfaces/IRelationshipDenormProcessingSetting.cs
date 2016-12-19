using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get relationship denorm processing setting class.
    /// </summary>
    public interface IRelationshipDenormProcessingSetting : IMDMObject
    {
        #region Methods

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of Relationship Denorm processing setting
        /// </summary>
        /// <returns>Xml representation of Relationship Denorm processing setting</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of Relationship Denorm processing setting on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of MDM Trace Config</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion ToXml methods

        #endregion Methods
    }
}
