using System.Collections.Generic;
using MDM.BusinessObjects;
using MDM.BusinessObjects.Exports;

namespace MDM.Interfaces.Exports
{
    /// <summary>
    /// Exposes methods or properties to set or get the collection of enitity export profiles. 
    /// </summary>
    public interface IEntityExportProfileCollection : ICollection<EntityExportProfile>, IEnumerable<EntityExportProfile>
    {
        /// <summary>
        /// Add export profiles in collection
        /// </summary>
        /// <param name="items">export profiles to add in collection</param>
        void AddRange(ICollection<EntityExportProfile> items);
    }
}
