using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get attribute version collection.
    /// </summary>
    public interface IAttributeVersionCollection : IEnumerable<AttributeVersion>
    {
        #region Properties

        #endregion

        #region Methods

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of AttributeVersionCollection object
        /// </summary>  
        /// <returns>Xml representation of AttributeVersionCollection object</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of AttributeVersionCollection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>        
        /// <returns>Xml representation of AttributeVersionCollection object</returns>
        String ToXml(ObjectSerialization serialization);

        #endregion ToXml methods

        #endregion
    }
}
