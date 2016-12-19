using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get synonymization rule parameters.
    /// </summary>
    public interface ISynonymizationRuleParameters
    {
        /// <summary>
        /// Dictionary of Synonyms for target value
        /// </summary>
        Dictionary<String, String> SynonymDictionary { get; set; }
    }
}
