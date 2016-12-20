using System.Collections.Generic;
using System;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get the collection of <see cref="IMDMFeatureConfig"/> elements.
    /// </summary>
    public interface IMDMFeatureConfigCollection : ICollection<MDMFeatureConfig>
    {   
        #region Methods         

        /// <summary>
        /// Get Xml representation of IMDMFeatureConfigCollection
        /// </summary>
        /// <returns>Xml representation of IMDMFeatureConfigCollection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of IMDMFeatureConfigCollection
        /// </summary>
        /// <param name="serialization">The serialization options.</param>
        /// <returns>Xml representation of IMDMFeatureConfigCollection</returns>
        String ToXml(ObjectSerialization serialization);

        /// <summary>
        /// Add MDM Feature Config in collection
        /// </summary>
        /// <param name="iMDMFeatureConfig">MDM Feature Config to add in collection</param>
        void Add(IMDMFeatureConfig iMDMFeatureConfig);


        /// <summary>
        /// Add MDM Feature Config in collection
        /// </summary>
        /// <param name="iMDMFeatureConfigs">MDM Feature Config to add in collection</param>
        void AddRange(IMDMFeatureConfigCollection iMDMFeatureConfigs);

       
        /// <summary>
        /// Removes the first occurrence of a specific object from the MDM Feature Config collection
        /// </summary>
        /// <param name="iMDMFeatureConfig">The object to remove from the MDM Feature Config collection</param>
        /// <returns>true if item is successfully removed; otherwise, false</returns>
        Boolean Remove(IMDMFeatureConfig iMDMFeatureConfig);

        #endregion Methods
    }
}
