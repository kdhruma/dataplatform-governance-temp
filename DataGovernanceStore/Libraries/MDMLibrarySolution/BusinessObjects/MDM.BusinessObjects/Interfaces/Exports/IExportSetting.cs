using System;
using MDM.Core;

namespace MDM.Interfaces.Exports
{
    /// <summary>
    /// Exposes methods or properties to set or get the export setting business object.
    /// </summary>
    public interface IExportSetting
    {
        #region Properties

        /// <summary>
        /// Property specifying export setting name
        /// </summary>
        String Name { get; set; }

        /// <summary>
        /// Property specifying export setting value
        /// </summary>
        String Value { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents export setting in Xml format
        /// </summary>
        /// <returns>export setting in Xml format</returns>
        String ToXml();

        /// <summary>
        /// Represents export setting in Xml format based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of export setting</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion Methods
    }
}
