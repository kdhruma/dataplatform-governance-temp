using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get the collection of merge profiles.
    /// </summary>
    public interface IMergingProfileCollection: ICollection<MergingProfile>, ICloneable
    {
    }
}
