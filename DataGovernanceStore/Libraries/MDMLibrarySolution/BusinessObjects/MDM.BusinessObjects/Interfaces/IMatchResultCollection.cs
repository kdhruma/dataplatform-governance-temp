using System;
using System.Collections.Generic;

namespace MDM.BusinessObjects.Interfaces
{
    using MDM.BusinessObjects.DQM;

    /// <summary>
    /// Specifies interface for Match Results Collection
    /// </summary>
    public interface IMatchResultCollection : ICollection<MatchResult>, ICloneable
    {
    }
}
