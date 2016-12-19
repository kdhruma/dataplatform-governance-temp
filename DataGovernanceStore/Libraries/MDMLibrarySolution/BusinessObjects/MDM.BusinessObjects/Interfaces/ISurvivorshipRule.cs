using System;

namespace MDM.BusinessObjects.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Specifies interface  for SurvivorshipRule
    /// </summary>
    public interface ISurvivorshipRule
    {
        /// <summary>
        /// Property denoting Conditions
        /// </summary>
        SurvivorshipRuleAttributeValueConditionCollection Conditions { get; set; }

        /// <summary>
        /// Property denoting Sources
        /// </summary>
        SurvivorshipRuleSourceListItemCollection Sources { get; set; }

        /// <summary>
        /// Property denoting ExternalStrategy
        /// </summary>
        String ExternalStrategy { get; set; }

        /// <summary>
        /// Property denoting CollectionStrategy
        /// </summary>
        CollectionStrategy CollectionStrategy { get; set; }
    }
}
