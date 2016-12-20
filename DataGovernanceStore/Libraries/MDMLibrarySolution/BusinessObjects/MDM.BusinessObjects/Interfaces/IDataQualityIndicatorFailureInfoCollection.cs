using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects.DQM;

    /// <summary>
    /// Exposes methods or properties to set or get collection of  DataQualityIndicator failure information.
    /// </summary>
    public interface IDataQualityIndicatorFailureInfoCollection : ICollection<DataQualityIndicatorFailureInfo>
    {
    }
}