using System;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get base synonymization rules.
    /// </summary>
    public interface IBaseSynonymizationRuleParameters
    {
        /// <summary>
        /// Property for the WordLшstName
        /// </summary>
        String WordListName { get; set; }
    }
}