using System;
using MDM.Core;

namespace MDM.Interfaces.Exports
{
    /// <summary>
    /// Exposes methods or properties to set or get the profile setting object.
    /// </summary>
    public interface IProfileSetting
    {
        #region Properties

        /// <summary>
        /// Property specifying profilesetting name
        /// </summary>
        String Name { get; set; }

        /// <summary>
        /// Property specifying profilesetting value
        /// </summary>
        String Value { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents profilesetting in Xml format
        /// </summary>
        /// <returns>profilesetting in Xml format</returns>
        String ToXml();

        /// <summary>
        /// Represents profilesetting in Xml format based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of profilesetting</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion Methods
    }
}
