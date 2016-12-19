using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get job scope filtering criteria.
    /// </summary>
    public interface IJobScopeFilter
    {
        /// <summary>
        /// Property for container ids
        /// </summary>
        Collection<Int32> ContainerIds { get; set; }

        /// <summary>
        /// Property for category ids
        /// </summary>
        Collection<Int64> CategoryIds { get; set; }

        /// <summary>
        /// Property for entity type ids
        /// </summary>
        Collection<Int32> EntityTypeIds { get; set; }
        
        /// <summary>
        /// Property for attribute rules
        /// </summary>
        SearchAttributeRuleCollection SearchAttributeRules { get; set; }
    }
}
