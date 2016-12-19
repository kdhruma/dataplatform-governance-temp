using System;
using System.Collections.Generic;
using MDM.BusinessObjects.Exports;
using MDM.Core;

namespace MDM.Interfaces.Exports
{
    /// <summary>
    /// Exposes methods or properties to set or get the collection of data subscribers.
    /// </summary>
    public interface IDataSubscriberCollection : ICollection<DataSubscriber>
    {
        #region Methods

        /// <summary>
        /// Get Xml representation of data subscriber collection
        /// </summary>
        /// <returns>Xml representation of data subscriber collection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of data subscriber collection based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of data subscriber collection</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion Methods
    }
}
