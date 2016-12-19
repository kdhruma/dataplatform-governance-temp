using System;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get dependent attribute data map related information.
    /// </summary>
    public interface IDependentAttributeDataMap : ITable
    {
        #region Method

        /// <summary>
        /// Get Xml representation of DependentAttributeDataMap object
        /// </summary>
        /// <returns>Xml representation of object</returns>
        new String ToXml();

        /// <summary>
        /// Get Xml representation of DependentAttributeDataMap object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        new String ToXml(ObjectSerialization serialization);

        #endregion
    }
}
