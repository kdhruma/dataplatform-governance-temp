using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects.DQM;

    /// <summary>
    /// Exposes methods or properties to set or get data quality class collection.
    /// </summary>
    public interface IDataQualityClassCollection : ICollection<DataQualityClass>
    {
    }
}
