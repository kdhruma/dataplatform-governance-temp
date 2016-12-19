using System;
using MDM.Core;

namespace MDM.Interfaces.Exports
{
    /// <summary>
    /// Exposes methods or properties to set or get the data subscriber business object.
    /// </summary>
    public interface IDataSubscriber : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property specifying data subscriber location which defines where to store file
        /// </summary>
        String Location { get; set; }

        /// <summary>
        /// Property specifying data subscriber filename
        /// </summary>
        String FileName { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents datasubscriber in Xml format
        /// </summary>
        /// <returns>datasubscriber in Xml format</returns>
        String ToXml();

        /// <summary>
        /// Represents datasubscriber in Xml format based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of datasubscriber</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion Methods
    }
}
