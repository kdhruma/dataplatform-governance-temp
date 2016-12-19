using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies collection of NormalizationReportItem
    /// </summary>
    [DataContract]
    public class NormalizationReportItemsCollection : InterfaceContractCollection<INormalizationReportItem, NormalizationReportItem>, INormalizationReportItemsCollection
    {
        #region Fields

        /// <summary>
        /// Field denoting total items count for all pages of normalization report
        /// </summary>
        private Int64? _totalItemsCount = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the NormalizationReportItem collection
        /// </summary>
        public NormalizationReportItemsCollection()
        { }

        /// <summary>
        /// Initialize NormalizationReportItem collection from IList
        /// </summary>
        /// <param name="dqmNormalizationReportItemsList">Source items</param>
        public NormalizationReportItemsCollection(IList<NormalizationReportItem> dqmNormalizationReportItemsList)
        {
            this._items = new Collection<NormalizationReportItem>(dqmNormalizationReportItemsList);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting total items count for all pages of normalization report
        /// </summary>
        [DataMember]
        public Int64? TotalItemsCount
        {
            get { return _totalItemsCount; }
            set { _totalItemsCount = value; }
        }

        #endregion
    }
}