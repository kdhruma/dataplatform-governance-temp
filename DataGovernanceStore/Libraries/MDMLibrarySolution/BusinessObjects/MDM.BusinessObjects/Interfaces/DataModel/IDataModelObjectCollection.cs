using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get data model object collection.
    /// </summary>
    public interface IDataModelObjectCollection : IEnumerable
    {
        #region Properties

        /// <summary>
        /// Represents no. of data model present into the collection
        /// </summary>
        Int32 Count { get; }

        /// <summary>
        /// Property denoting a ObjectType for an dataModelObject object
        /// </summary>
        ObjectType DataModelObjectType { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Remove IDataModelObjects based on collection of reference id.
        /// </summary>
        /// <param name="referenceIds">Indicates collection of referenceId of data model objects to be fetched.</param>
        /// <returns>true if item was successfully removed from the collection;otherwise, false. This method also returns false if item is not found in the original</returns>
        Boolean Remove(Collection<String> referenceIds);

        /// <summary>
        /// Splits IDataModelObjects based on batchSize
        /// </summary>
        /// <param name="batchSize">Indicates no of objects to be put in each batch</param>
        /// <returns>IDataModelObjects in Batch</returns>
        Collection<IDataModelObjectCollection> Split(Int32 batchSize);

        /// <summary>
        /// Adds item in current collection
        /// </summary>
        /// <param name="item">Indicates item to be added in collection</param>
        void AddDataModelObject(IDataModelObject item);

        #endregion
    }
}