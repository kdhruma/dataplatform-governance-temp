using System;
using MDM.BusinessObjects.Exports;
using MDM.Core;

namespace MDM.Interfaces.Exports
{
    /// <summary>
    /// Exposes methods or properties to set or get the output specification business object.
    /// </summary>
    public interface IOutputSpecification : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property specifying data formatter
        /// </summary>
        DataFormatter DataFormatter { get; set; }

        /// <summary>
        /// Property specifying collection of data subscribers
        /// </summary>
        DataSubscriberCollection DataSubscribers { get; set; }

        /// <summary>
        /// Property specifying output data specification
        /// </summary>
        OutputDataSpecification OutputDataSpecification { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents output specification in Xml format
        /// </summary>
        /// <returns>output specification in Xml format</returns>
        String ToXml();

        /// <summary>
        /// Represents output specification in Xml format based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of output specification</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion Methods
    }
}
