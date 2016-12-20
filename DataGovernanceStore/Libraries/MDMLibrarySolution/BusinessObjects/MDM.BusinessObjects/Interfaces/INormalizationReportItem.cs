using System;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get normalization report item.
    /// </summary>
    public interface INormalizationReportItem : INormalizationResult, ICloneable
    {
        /// <summary>
        /// Property denoting entity short name
        /// </summary>
        String EntityShortName { get; set; }

        /// <summary>
        /// Property denoting entity long name
        /// </summary>
        String EntityLongName { get; set; }

        /// <summary>
        /// Property denoting normalization rule short name
        /// </summary>
        String RuleShortName { get; set; }

        /// <summary>
        /// Property denoting normalization rule long name
        /// </summary>
        String RuleLongName { get; set; }

        /// <summary>
        /// Property denoting attribute short name
        /// </summary>
        String AttributeShortName { get; set; }

        /// <summary>
        /// Property denoting attribute long name
        /// </summary>
        String AttributeLongName { get; set; }

        /// <summary>
        /// Property denoting organization short name
        /// </summary>
        String OrganizationShortName { get; set; }

        /// <summary>
        /// Property denoting organization long name
        /// </summary>
        String OrganizationLongName { get; set; }

        /// <summary>
        /// Property denoting container short name
        /// </summary>
        String ContainerShortName { get; set; }

        /// <summary>
        /// Property denoting container long name
        /// </summary>
        String ContainerLongName { get; set; }

        /// <summary>
        /// Property denoting entity type short name
        /// </summary>
        String EntityTypeShortName { get; set; }

        /// <summary>
        /// Property denoting entity type long name
        /// </summary>
        String EntityTypeLongName { get; set; }

        /// <summary>
        /// Property denoting category short name
        /// </summary>
        String CategoryShortName { get; set; }

        /// <summary>
        /// Property denoting category long name
        /// </summary>
        String CategoryLongName { get; set; }

        /// <summary>
        /// Property denoting the Organization Id of a Container
        /// </summary>
        Int32 OrganizationId { get; set; }

        /// <summary>
        /// Property denoting the Container Id of an entity
        /// </summary>
        Int32 ContainerId { get; set; }

        /// <summary>
        /// Property denoting the Category Id of an entity
        /// </summary>
        Int64 CategoryId { get; set; }

        /// <summary>
        /// Property denoting the Type Id of an entity
        /// </summary>
        Int32 EntityTypeId { get; set; }
    }
}