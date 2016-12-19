using System;

namespace MDM.BusinessObjects.Interfaces
{
    /// <summary>
    /// Specifies interface for Match Result
    /// </summary>
    public interface IMatchResult : ICloneable
    {
        /// <summary>
        /// Property denoting TargetEntityId
        /// </summary>
        Int64 TargetEntityId { get; set; }

        /// <summary>
        /// Property denoting MatchingScore
        /// </summary>
        Decimal MatchingScore { get; set; }
    }
}
