using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties used for representing the error collection.
    /// </summary>
    public interface IErrorCollection : IEnumerable<Error>
    {
        #region Properties

        /// <summary>
        /// Indicates the count of error in errorCollection Object
        /// </summary>
        Int32 Count { get; }

        #endregion

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of ErrorCollection object
        /// </summary>
        /// <returns>Xml string representing the ErrorCollection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of ErrorCollection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization serialization);

        #endregion ToXml methods

        /// <summary>
        /// Add Item in to the current ErrorCollection
        /// </summary>
        /// <param name="item">Error intance to be added</param>
        void Add(IError item);

        /// <summary>
        /// Add Item in to the current ErrorCollection
        /// </summary>
        /// <param name="items">Error intances to be added</param>
        void AddRange(Collection<IError> items);
    }
}
