using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects.DQMMerging;

    /// <summary>
    /// Exposes methods or properties to set or get the collection of rematch rules.
    /// </summary>
    public interface IRematchRuleCollection : ICollection<RematchRule>, ICloneable
    {
    }
}