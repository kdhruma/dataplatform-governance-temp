using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects.DQMMerging;

    /// <summary>
    /// Exposes methods or properties to set or get the collection of match review profiles.
    /// </summary>
    public interface IMatchReviewProfileCollection : ICollection<MatchReviewProfile>, ICloneable
    {
    }
}