using System;
using System.Collections.Generic;
namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get the collection of matching profiles.
    /// </summary>
    public interface IMatchingProfileCollection : ICollection<MDM.BusinessObjects.DQM.MatchingProfile>, ICloneable
    {
    }
}