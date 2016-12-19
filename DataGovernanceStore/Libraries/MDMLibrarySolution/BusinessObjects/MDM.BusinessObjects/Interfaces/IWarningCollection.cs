using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get the collection of warnings.
    /// </summary>
    public interface IWarningCollection : IEnumerable<Warning>
    {
        /// <summary>
        /// No. of warning under Warning Collection
        /// </summary>
        Int32 Count { get; }

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of WarningCollection object
        /// </summary>
        /// <returns>Xml string representing the WarningCollection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of WarningCollection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization serialization);

        #endregion ToXml methods
    }
}
