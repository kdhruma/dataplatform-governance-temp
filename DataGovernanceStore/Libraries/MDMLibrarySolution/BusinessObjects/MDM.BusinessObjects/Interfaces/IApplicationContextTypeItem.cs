using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get the application context type item.
    /// </summary>
    public interface IApplicationContextTypeItem : IMDMObject
    {
        /// <summary>
        /// Property indicates OrganizationFilterWeight
        /// </summary>
        Int32? OrganizationFilterWeight { get; set; }

        /// <summary>
        /// Property indicates ContainerFilterWeight
        /// </summary>
        Int32? ContainerFilterWeight { get; set; }

        /// <summary>
        /// Property indicates NodeTypeFilterWeight
        /// </summary>
        Int32? NodeTypeFilterWeight { get; set; }

        /// <summary>
        /// Property indicates RelationshipTypeFilterWeight
        /// </summary>
        Int32? RelationshipTypeFilterWeight { get; set; }

        /// <summary>
        /// Property indicates CategoryFilterWeight
        /// </summary>
        Int32? CategoryFilterWeight { get; set; }

        /// <summary>
        /// Property indicates AttributeFilterWeight
        /// </summary>
        Int32? AttributeFilterWeight { get; set; }

        /// <summary>
        /// Property indicates EntityFilterWeight
        /// </summary>
        Int32? EntityFilterWeight { get; set; }

        /// <summary>
        /// Property indicates RoleFilterWeight
        /// </summary>
        Int32? RoleFilterWeight { get; set; }

        /// <summary>
        /// Property indicates UserFilterWeight
        /// </summary>
        Int32? UserFilterWeight { get; set; }
        
        /// <summary>
        /// Property indicates CreateDateTime
        /// </summary>
        DateTime? CreateDateTime { get; set; }

        /// <summary>
        /// Property indicates ModDateTime
        /// </summary>
        DateTime? ModDateTime { get; set; }

        /// <summary>
        /// Property indicates CreateUser
        /// </summary>
        String CreateUser { get; set; }

        /// <summary>
        /// Property indicates ModUser
        /// </summary>
        String ModUser { get; set; }

        /// <summary>
        /// Property indicates CreateProgram
        /// </summary>
        String CreateProgram { get; set; }

        /// <summary>
        /// Property indicates ModProgram
        /// </summary>
        String ModProgram { get; set; }

        /// <summary>
        /// Property indicates LocaleFilterWeight
        /// </summary>
        Int32? LocaleFilterWeight { get; set; }

        /// <summary>
        /// Property indicates AttributeSourceFilterWeight
        /// </summary>
        Int32? AttributeSourceFilterWeight { get; set; }
    }
}

