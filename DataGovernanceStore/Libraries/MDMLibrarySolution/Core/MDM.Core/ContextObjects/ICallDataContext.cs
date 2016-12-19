using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get call data context.
    /// </summary>
    public interface ICallDataContext
    {
        /// <summary>
        /// 
        /// </summary>
        Collection<Int32> AttributeIdList { get; set; }

        /// <summary>
        /// 
        /// </summary>
        Collection<Int64> CategoryIdList { get; set; }

        /// <summary>
        /// 
        /// </summary>
        Collection<Int32> ContainerIdList { get; set; }

        /// <summary>
        /// 
        /// </summary>
        Collection<Int64> EntityIdList { get; set; }

        /// <summary>
        /// 
        /// </summary>
        Collection<Int32> EntityTypeIdList { get; set; }

        /// <summary>
        /// 
        /// </summary>
        Collection<Int32> RelationshipTypeIdList { get; set; }

        /// <summary>
        /// 
        /// </summary>
        Collection<LocaleEnum> LocaleList { get; set; }

        /// <summary>
        /// 
        /// </summary>
        Collection<String> LookupTableNameList { get; set; }

        /// <summary>
        /// 
        /// </summary>
        Collection<Int32> OrganizationIdList { get; set; }
    }
}
