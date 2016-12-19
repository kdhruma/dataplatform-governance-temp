using System;
using MDM.Core;

namespace MDM.Interfaces.Exports
{
    /// <summary>
    /// Exposes methods or properties to set or get the execution setting business object.
    /// </summary>
    public interface IDataFormatterSetting
    {
        #region Properties

        /// <summary>
        /// Property specifying execution setting name
        /// </summary>
        String Name { get; set; }

        /// <summary>
        /// Property specifying execution setting value
        /// </summary>
        String Value { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents execution setting in Xml format
        /// </summary>
        /// <returns>execution setting in Xml format</returns>
        String ToXml();

        /// <summary>
        /// Represents execution setting in Xml format based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of execution setting</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion Methods
    }
}
