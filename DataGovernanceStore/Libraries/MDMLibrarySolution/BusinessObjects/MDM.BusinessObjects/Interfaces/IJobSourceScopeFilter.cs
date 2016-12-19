using System;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get job source scope filtering criteria.
    /// </summary>
    public interface IJobSourceScopeFilter : IJobScopeFilter
    {
        /// <summary>
        /// Delta mode
        /// </summary>
        Boolean IsDeltaMode { get; set; }
    }
}