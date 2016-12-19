using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get collection of information. 
    /// </summary>
    public interface IInformationCollection : IEnumerable<Information>
    {
        #region ToXml methods

        /// <summary>
        /// Get Xml representation of InformationCollection object
        /// </summary>
        /// <returns>Xml string representing the InformationCollection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of InformationCollection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization serialization);

        #endregion ToXml methods
    }
}
