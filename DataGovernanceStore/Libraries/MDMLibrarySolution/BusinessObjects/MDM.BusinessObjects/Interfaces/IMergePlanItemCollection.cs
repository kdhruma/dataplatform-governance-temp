using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects.DQM;

    /// <summary>
    /// Exposes methods or properties to set or get the collection of merge plan items.
    /// </summary>
    public interface IMergePlanItemCollection : ICollection<MergePlanItem>
    {
    }
}