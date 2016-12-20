using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects.DQM;

    /// <summary>
    /// Exposes methods or properties to set or get collection of data quality indicators.
    /// </summary>
    public interface IDataQualityIndicatorCollection : ICollection<DataQualityIndicator>
    {

    }
}
