using System;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;

    /// <summary>
    /// Exposes methods and properties for survivorship ruleset information
    /// </summary>
    public interface ISurvivorshipRuleset : IMDMObject, ICloneable
    {
        /// <summary>
        /// Property indicates OrganizationId
        /// </summary>
        Int32 OrganizationId { get; set; }

        /// <summary>
        /// Property indicates ContainerId
        /// </summary>
        Int32 ContainerId { get; set; }

        /// <summary>
        /// Property indicates CategoryId
        /// </summary>
        Int64 CategoryId { get; set; }

        /// <summary>
        /// Property indicates CategoryPath
        /// </summary>
        String CategoryPath { get; set; }

        /// <summary>
        /// Property indicates EntityId
        /// </summary>
        Int64 EntityId { get; set; }

        /// <summary>
        /// Property indicates AttributeId
        /// </summary>
        Int32 AttributeId { get; set; }

        /// <summary>
        /// Property indicates NodeTypeId
        /// </summary>
        Int32 NodeTypeId { get; set; }

        /// <summary>
        /// Property indicates RelationshipTypeId
        /// </summary>
        Int32 RelationshipTypeId { get; set; }

        /// <summary>
        /// Property indicates SecurityRoleId
        /// </summary>
        Int32 SecurityRoleId { get; set; }

        /// <summary>
        /// Property indicates SecurityUserId
        /// </summary>
        Int32 SecurityUserId { get; set; }

        /// <summary>
        /// Property indicates Description
        /// </summary>
        String Description { get; set; }

        /// <summary>
        /// Property indicates ModDateTime
        /// </summary>
        DateTime? ModDateTime { get; set; }

        /// <summary>
        /// Property indicates ModUser
        /// </summary>
        String ModUser { get; set; }

        /// <summary>
        /// Property indicates ModProgram
        /// </summary>
        String ModProgram { get; set; }

        /// <summary>
        /// Property indicates SequenceNumber
        /// </summary>
        Int32? SequenceNumber { get; set; }

        /// <summary>
        /// Property denoting SurvivorshipRules
        /// </summary>
        SurvivorshipRuleCollection SurvivorshipRules { get; set; }

        /// <summary>
        /// Property denoting StrategyPriorities
        /// </summary>
        SurvivorshipRuleStrategyPriorityCollection StrategyPriorities { get; set; }
    }
}