using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get collection of word list.
    /// </summary>
    public interface IWordListsCollection : ICollection<WordList>, ICloneable
    {
    }
}