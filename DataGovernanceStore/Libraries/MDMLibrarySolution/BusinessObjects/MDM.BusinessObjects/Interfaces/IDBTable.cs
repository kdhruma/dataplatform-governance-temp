using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    ///  Exposes methods and properties used for setting the database table.
    /// </summary>
    public interface IDBTable : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Field denoting PopulateRSTObject of DBTable
        /// </summary>
        Boolean PopulateRSTObject { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of DBTable object
        /// </summary>
        /// <returns>Xml representation of object</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of DBTable object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization serialization);

        /// <summary>
        /// Clone DBTable object
        /// </summary>
        /// <returns>cloned copy of IDBTable object.</returns>
        IDBTable Clone();

        /// <summary>
        /// Delta Merge of DBTable
        /// </summary>
        /// <param name="deltaDBTable">IDBTable that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="returnClonedObject">true means return new merged cloned object;otherwise, same object.</param>
        /// <returns>Merged IDBTable instance</returns>
        IDBTable MergeDelta(IDBTable deltaDBTable, ICallerContext iCallerContext, Boolean returnClonedObject = true);

        #endregion
    }
}
