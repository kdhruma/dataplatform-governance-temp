using System;

namespace MDM.BusinessObjects.Interfaces
{
    /// <summary>
    /// Specifies interface  for SurvivorshipRuleSourceListItem
    /// </summary>
    public interface ISurvivorshipRuleSourceListItem
    {
        /// <summary>
        /// Property denotes SourceId
        /// </summary>
        Int32 SourceId { get; set; }

        /// <summary>
        /// Property denotes SourceAttributeId
        /// </summary>
        long? SourceAttributeId { get; set; }

        /// <summary>
        /// Property denotes Priority
        /// </summary>
        Int32 Priority { get; set; }

    }
}
