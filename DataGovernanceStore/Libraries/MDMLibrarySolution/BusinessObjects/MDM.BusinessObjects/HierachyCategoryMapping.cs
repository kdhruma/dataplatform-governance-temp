using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Class describes mapping of category Ids to Hierarchy Id
    /// </summary>
    [DataContract]
    public class HierachyCategoryMapping : MDMObject, IHierachyCategoryMapping, ICloneable
    {
        /// <summary>
        /// Parametrized Constructor 
        /// </summary>
        /// <param name="hierarchyId">Hierarchy Id to be mapped</param>
        /// <param name="categoryIds">Collection of CategoryId's</param>
        public HierachyCategoryMapping(Int32 hierarchyId, Collection<Int64> categoryIds)
        {
            HierarchyId = hierarchyId;
            CategoryIds = categoryIds;
        }

        /// <summary>
        /// Property denotes which hierachy ID is using
        /// </summary>
        [DataMember]
        public Int32 HierarchyId { get; set; }

        /// <summary>
        /// Property denotes which category Ids are using
        /// </summary>
        [DataMember]
        public Collection<Int64> CategoryIds { get; set; }

        /// <summary>
        /// Implementaion of ICloneable Interface
        /// </summary>
        /// <returns>Cloned HierachyCategoryMapping object</returns>
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
