using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get dependent attribute data map collection related information.
    /// </summary>
    public interface IDependentAttributeDataMapCollection : IEnumerable<DependentAttributeDataMap>
    {
        #region ToXml methods

        /// <summary>
        /// Get Xml representation of TableCollection object
        /// </summary>
        /// <returns>Xml string representing the TableCollection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of TableCollection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization serialization);

        /// <summary>
        /// Get Xml string representing DependentAttributeDataMap model for each dependentAttributeDataMap in this collection
        /// </summary>
        /// <returns>Xml string representing DependentAttributeDataMap model</returns>
        String GetDependentAttributeDataMapsModel();

        #endregion ToXml methods

        #region Add DependentAttributeDataMap Methods

        /// <summary>
        /// Add table in ITableCollection
        /// </summary>
        /// <param name="dependentAttributeDataMap">Table to add in ITableCollection</param>
        /// <exception cref="ArgumentNullException">Thrown if table is null</exception>
        void AddDependentAttributeDataMap(IDependentAttributeDataMap dependentAttributeDataMap);

        /// <summary>
        /// Add tables in current table collection
        /// </summary>
        /// <param name="dependentAttributeDataMaps">ITableCollection to add in current collection</param>
        /// <exception cref="ArgumentNullException">Thrown if tableCollection is null</exception>
        void AddDependentAttributeDataMaps(IDependentAttributeDataMapCollection dependentAttributeDataMaps);

        #endregion
    }
}
