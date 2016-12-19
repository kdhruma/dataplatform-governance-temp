using System;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get normalization ruleset.
    /// </summary>
    public interface INormalizationRuleset
    {
        /// <summary>
        /// Property denoting Attribute Id
        /// </summary>
        Int32? AttributeId { get; set; }

        /// <summary>
        /// Property denoting value of Ruleset
        /// </summary>
        String Value { get; set; }
        
        /// <summary>
        /// Property denoting Rule Id
        /// </summary>
        Int32 RuleId { get; set; }

        /// <summary>
        /// Property denoting Relationship Type Id
        /// </summary>
        Int32? RelationshipTypeId { get; set; }

    }
}
