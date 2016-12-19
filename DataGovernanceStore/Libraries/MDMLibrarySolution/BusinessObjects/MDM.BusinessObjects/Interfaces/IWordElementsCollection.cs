using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get collection of word element.
    /// </summary>
    public interface IWordElementsCollection : ICollection<WordElement>, ICloneable
    {
        /// <summary>
        /// Indicates TotalWordsCount
        /// </summary>
        Int32? TotalWordsCount { get; set; }

    }
}
