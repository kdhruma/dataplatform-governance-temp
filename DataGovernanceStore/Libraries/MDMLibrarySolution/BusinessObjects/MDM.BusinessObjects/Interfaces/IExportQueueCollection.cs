using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get export queue collection.
    /// </summary>
    public interface IExportQueueCollection : IEnumerable<ExportQueue>
    {
        #region Properties

        /// <summary>
        /// Presents no. of export queue present into the collection
        /// </summary>
        Int32 Count { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets Xml representation of the ExportQueueCollection object
        /// </summary>
        /// <returns>Returns Xml string representing the ExportQueueCollection</returns>
        String ToXml();

        #endregion Methods
    }
}