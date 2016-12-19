using System;
using MDM.BusinessObjects.Exports;
using MDM.Core;

namespace MDM.Interfaces.Exports
{
    /// <summary>
    /// Exposes methods or properties to set or get the scope specification business object.
    /// </summary>
    public interface IScopeSpecification :IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property specifying export scope collection
        /// </summary>
        ExportScopeCollection ExportScopes { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents notification in Xml format
        /// </summary>
        /// <returns>notification in Xml format</returns>
        String ToXml();

        /// <summary>
        /// Represents notification in Xml format based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of notification</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion Methods
    }
}
