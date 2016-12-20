using System;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties used for specifying the normalization rule description.
    /// </summary>
    public interface INormalizationRule
    {
        /// <summary>
        /// Description for the rule
        /// </summary>
        String Description { get; set; }
    }
}