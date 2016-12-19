using System;
using System.Collections.Generic;
using MDM.BusinessObjects;
using MDM.BusinessObjects.Exports;
using MDM.Core;

namespace MDM.Interfaces.Exports
{
    /// <summary>
    /// Exposes methods or properties to set or get the collection of export subscriber. 
    /// </summary>
    public interface IExportSubscriberCollection : ICollection<ExportSubscriber>
    {
        /// <summary>
        /// Check if current collection contains given Id Export Subscriber or not
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>true if exist</returns>
        bool Contains(int Id);
        
        /// <summary>
        /// Compare the given Object with the current object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>True if it is equal</returns>
        bool Equals(object obj);
        
        /// <summary>
        /// Get the Hash Code
        /// </summary>
        /// <returns>Hash Code</returns>
        int GetHashCode();

        /// <summary>
        /// Remove accurence of Export Subscriber by given Id
        /// </summary>
        /// <param name="exportSubscriberId">Id of an export Subscriber</param>
        /// <returns>True if successfully removed</returns>
        bool Remove(Int32 exportSubscriberId);

        /// <summary>
        /// XML presentaion of an Object 
        /// </summary>
        /// <returns>XML presentaion of an Object</returns>
        string ToXml();

        /// <summary>
        /// XML presentaion of an Object
        /// </summary>
        /// <param name="serialization"></param>
        /// <returns>XML presentaion of an Object</returns>
        string ToXml(ObjectSerialization serialization);
    }
}
