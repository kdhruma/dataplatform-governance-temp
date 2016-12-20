using System.Collections.Generic;
using System;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get collection of <see cref="IMDMFeatureConfig"/> elements.
    /// </summary>
    public interface IMDMFeatureConfigDetailCollection : ICollection<MDMFeatureConfigDetail>
    {
        #region Methods

         /// <summary>
        /// Determines whether the IMDMFeatureConfigDetailCollection contains a specific value.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        ///   <c>true</c> if item is found in the IMDMFeatureConfigDetailCollection; otherwise, <c>false</c>.
        /// </returns>
        Boolean Contains(int id);

        /// <summary>
        /// Get Xml representation of IMDMFeatureConfigDetailCollection
        /// </summary>
        /// <returns>Xml representation of IMDMFeatureConfigDetailCollection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of IMDMFeatureConfigDetailCollection
        /// </summary>
        /// <param name="serialization">The serialization options.</param>
        /// <returns>Xml representation of IMDMFeatureConfigDetailCollection</returns>
        String ToXml(ObjectSerialization serialization);

        /// <summary>
        /// Add MDM Feature Config in collection
        /// </summary>
        /// <param name="iMDMFeatureConfigDetails">MDM Feature Config to add in collection</param>
        void Add(IMDMFeatureConfigDetail iMDMFeatureConfigDetails);
         

        /// <summary>
        /// Add MDM Feature Config in collection
        /// </summary>
        /// <param name="iMDMFeatureConfigs">MDM Feature Config to add in collection</param>
        void AddRange(IMDMFeatureConfigDetailCollection iMDMFeatureConfigs);

       
        /// <summary>
        /// Removes the first occurrence of a specific object from the MDM Feature Config collection
        /// </summary>
        /// <param name="iMDMFeatureConfigDetails">The object to remove from the MDM Feature Config collection</param>
        /// <returns>true if item is successfully removed; otherwise, false</returns>
        Boolean Remove(IMDMFeatureConfigDetail iMDMFeatureConfigDetails);

        #endregion Methods
    }
}
