using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Defines corresponding column ids for MergeResults database table
    /// It can be used for SortColumns values
    /// </summary>
    public enum MergeResultReportPredefinedColumn
    {
        /// <summary>
        /// 
        /// </summary>
        FilterIdForColumn_PK_MergeResultId = -1,

        /// <summary>
        /// 
        /// </summary>
        FilterIdForColumn_FK_MergeJobId = -2,

        /// <summary>
        /// 
        /// </summary>
        FilterIdForColumn_FK_SourceEntityId = -3,

        /// <summary>
        /// 
        /// </summary>
        FilterIdForColumn_FK_TargetEntityId = -4,

        /// <summary>
        /// 
        /// </summary>
        FilterIdForColumn_MergeAction = -5,

        /// <summary>
        /// 
        /// </summary>
        FilterIdForColumn_MergeResultStatus = -6,

        /// <summary>
        /// 
        /// </summary>
        FilterIdForColumn_MergeDateTime = -7
    }

    /// <summary>
    /// Specifies Merge Results Report Settings
    /// </summary>
    [DataContract]
    public class MergeResultsReportSettings : IMergeResultsReportSettings
    {
        #region Fields

        /// <summary>
        /// Denotes Ids of parent jobs
        /// </summary>
        private Collection<Int64> _jobIds = new Collection<Int64>();

        /// <summary>
        /// Denotes countFrom parameter for paged result
        /// </summary>
        private Int64? _countFrom;

        /// <summary>
        /// Denotes countTo parameter for paged result
        /// </summary>
        private Int64? _countTo;

        /// <summary>
        /// Denotes sort columns for paged result. It can have values of MergeResultReportPredefinedColumn (negative ones) or attributeId
        /// </summary>
        private Collection<Int32> _sortColumns = new Collection<Int32>();

        /// <summary>
        /// Denotes sort columns for paged result. It could be any column of result including attributeId e.x. SortColumn =
        /// </summary>
        private Boolean _sortOrder;

        /// <summary>
        /// Denotes DateFilterFrom
        /// </summary>
        private DateTime? _dateFilterFrom;

        /// <summary>
        /// Denotes DateFilterTo
        /// </summary>
        private DateTime? _dateFilterTo;

        /// <summary>
        /// Denotes search attribute rules
        /// </summary>
        private Collection<SearchAttributeRule> _searchAttributeRules = new Collection<SearchAttributeRule>();

        /// <summary>
        /// Denotes locale for translations of the result column values
        /// </summary>
        private LocaleEnum _localeId = LocaleEnum.UnKnown;

        /// <summary>
        /// Denotes total items count including all pages
        /// </summary>
        private Boolean _requestTotalItemsCount;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public MergeResultsReportSettings() { }

        /// <summary>
        /// Construct Merge Result Report by job id
        /// </summary>
        /// <param name="jobId"></param>
        public MergeResultsReportSettings(Int64? jobId)
        {
            if (jobId.HasValue)
            {
                JobIds = new Collection<Int64> { jobId.Value };
            }
        } 

        #endregion
        
        #region Public properties

        /// <summary>
        /// Denotes Ids of parent jobs
        /// </summary>
        [DataMember]
        public Collection<Int64> JobIds
        {
            get { return _jobIds; }
            set { _jobIds = value; }
        }

        /// <summary>
        /// Denotes countFrom parameter for paged result
        /// </summary>
        [DataMember]
        public Int64? CountFrom
        {
            get { return _countFrom; }
            set { _countFrom = value; }
        }

        /// <summary>
        /// Denotes countTo parameter for paged result
        /// </summary>
        [DataMember]
        public Int64? CountTo
        {
            get { return _countTo; }
            set { _countTo = value; }
        }

        /// <summary>
        /// Denotes sort columns for paged result. It can have values of MergeResultReportPredefinedColumn (negative ones) or attributeId
        /// </summary>
        [DataMember]
        public Collection<Int32> SortColumns
        {
            get { return _sortColumns; }
            set { _sortColumns = value; }
        }

        /// <summary>
        /// Denotes sort order for paged result. true for Descending, false for Ascending
        /// </summary>
        [DataMember]
        public Boolean SortOrder
        {
            get { return _sortOrder; }
            set { _sortOrder = value; }
        }

        /// <summary>
        /// Denotes DateFilterFrom
        /// </summary>
        [DataMember]
        public DateTime? DateFilterFrom
        {
            get { return _dateFilterFrom; }
            set { _dateFilterFrom = value; }
        }

        /// <summary>
        /// Denotes DateFilterTo
        /// </summary>
        [DataMember]
        public DateTime? DateFilterTo
        {
            get { return _dateFilterTo; }
            set { _dateFilterTo = value; }
        }

        /// <summary>
        /// Denotes search attribute rules
        /// </summary>
        [DataMember]
        public Collection<SearchAttributeRule> SearchAttributeRules
        {
            get { return _searchAttributeRules; }
            set { _searchAttributeRules = value; }
        }

        /// <summary>
        /// Denotes locale for translations of the result column values
        /// </summary>
        [DataMember]
        public LocaleEnum LocaleId
        {
            get { return _localeId; }
            set { _localeId = value; }
        }
        
        /// <summary>
        /// Denotes total items count including all pages
        /// </summary>
        [DataMember]
        public Boolean RequestTotalItemsCount
        {
            get { return _requestTotalItemsCount; }
            set { _requestTotalItemsCount = value; }
        }

        #endregion
    }
}