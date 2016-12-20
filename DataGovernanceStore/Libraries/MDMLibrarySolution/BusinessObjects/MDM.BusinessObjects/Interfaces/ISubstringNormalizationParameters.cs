using System;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get substring normalization parameters.
    /// </summary>
    public interface ISubstringNormalizationParameters
    {   
        /// <summary>
        /// Specifies WordList name
        /// </summary>
        String WordListName { get; set; }

        /// <summary>
        /// Specifies Word Breakers for synonymization
        /// </summary>
        String WordBreakers { get; set; }

        /// <summary>
        /// Specifies if Rule IsCaseSensitive
        /// </summary>
        Boolean IsCaseSensitive { get; set; }
    }
}