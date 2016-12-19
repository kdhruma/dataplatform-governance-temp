using System;
using MDM.Core;

namespace MDM.Interfaces.Exports
{
    /// <summary>
    /// Exposes methods or properties to set or get the export scope business object.
    /// </summary>
    public interface IExportScope : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property specifying export scope objectid
        /// </summary>
        Int64 ObjectId { get; set; }

        /// <summary>
        /// Property specifying object type of export scope
        /// </summary>
        ObjectType ObjectType { get; set; }

        /// <summary>
        /// Property specifying export scope object unique identifier
        /// </summary>
        String ObjectUniqueIdentifier { get; set; }

        /// <summary>
        /// Property specifying export scope include or not
        /// </summary>
        Boolean Include { get; set; }

        /// <summary>
        /// Property specifying export scope recursive or not
        /// </summary>
        Boolean IsRecursive { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents export scope in Xml format
        /// </summary>
        /// <returns>export scope in Xml format</returns>
        String ToXml();

        /// <summary>
        /// Represents export scope in Xml format based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of export scope</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion Methods
    }
}
