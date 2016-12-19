using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies collection of DataQualityIndicatorSummary
    /// </summary>
    [DataContract]
    public class DataQualityIndicatorSummaryCollection : InterfaceContractCollection<IDataQualityIndicatorSummary, DataQualityIndicatorSummary>, IDataQualityIndicatorSummaryCollection
    {
         #region Constructors

        /// <summary>
        /// Initializes a new instance of the DataQualityIndicatorSummaryCollection Collection
        /// </summary>
        public DataQualityIndicatorSummaryCollection()
        { }

        /// <summary>
        /// Initialize DataQualityIndicatorSummary collection from IList
        /// </summary>
        /// <param name="DataQualityIndicatorSummaryList"></param>
        public DataQualityIndicatorSummaryCollection(IList<DataQualityIndicatorSummary> DataQualityIndicatorSummaryList)
        {
            this._items = new Collection<DataQualityIndicatorSummary>(DataQualityIndicatorSummaryList);
        }

        #endregion

    }
}
