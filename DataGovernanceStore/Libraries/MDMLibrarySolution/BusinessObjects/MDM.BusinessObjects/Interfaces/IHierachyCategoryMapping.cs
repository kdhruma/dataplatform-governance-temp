using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.Core;
    /// <summary>
    /// Exposes methods or properties used for mapping of category Ids to hierarchy Id.
    /// </summary>
    public interface IHierachyCategoryMapping : IMDMObject
    {
        /// <summary>
        /// Property denotes which hierachy Id is using
        /// </summary>
        Int32 HierarchyId { get; set; }

        /// <summary>
        /// Property denotes which category Ids are using
        /// </summary>
        Collection<Int64> CategoryIds { get; set; }
    }
}
