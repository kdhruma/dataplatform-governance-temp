using System;
using MDM.Core;

namespace MDM.Interfaces.Exports
{
    /// <summary>
    /// Exposes methods or properties to set or get the data formatter business object.
    /// </summary>
    public interface IDataFormatter : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property specifying data formatter type like RsXml, RsExcel etc.
        /// </summary>
        String Type { get; set; }


        /// <summary>
        /// Property Specifying the formatter file extension like .xlsx or .xml etc
        /// </summary>
        String FileExtension { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents dataformatter in Xml format
        /// </summary>
        /// <returns>dataformatter in Xml format</returns>
        String ToXml();

        /// <summary>
        /// Represents dataformatter in Xml format based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of dataformatter</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion Methods
    }
}
