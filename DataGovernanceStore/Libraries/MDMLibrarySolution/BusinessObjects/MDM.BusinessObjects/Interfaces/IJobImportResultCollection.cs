using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Jobs;

    /// <summary>
    /// Exposes methods or properties to set or get job import result collection.
    /// </summary>
    public interface IJobImportResultCollection : IEnumerable<JobImportResult>
    {
        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of JobImportResultCollection object
        /// </summary>
        /// <returns>Xml string representing the JobImportResultCollection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of JobImportResultCollection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization serialization);

        #endregion
    }
}
