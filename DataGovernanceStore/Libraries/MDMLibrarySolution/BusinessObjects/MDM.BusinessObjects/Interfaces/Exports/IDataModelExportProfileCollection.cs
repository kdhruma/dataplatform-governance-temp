using System.Collections.Generic;

namespace MDM.Interfaces.Exports
{
    using MDM.BusinessObjects.Exports;

    /// <summary>
    /// Exposes methods or properties to set or get the collection of data model export profiles. 
    /// </summary>
    public interface IDataModelExportProfileCollection : ICollection<DataModelExportProfile>, IEnumerable<DataModelExportProfile>
    {
        /// <summary>
        /// Add data model export profiles in collection
        /// </summary>
        /// <param name="items">Indicates data model export profiles to add in collection</param>
        void AddRange(ICollection<DataModelExportProfile> items);
    }
}
