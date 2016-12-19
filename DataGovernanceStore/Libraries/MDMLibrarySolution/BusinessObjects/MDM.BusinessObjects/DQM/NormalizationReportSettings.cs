using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Defines corresponding column ids for NormalizationResults database table
    /// It can be used for SortColumns values
    /// </summary>
    public enum NormalizationReportPredefinedColumn
    {
        /// <summary>
        /// Indicates normalization result id column
        /// </summary>
        NormalizationResultsId = -1,

        /// <summary>
        /// Indicates normalization rule id column
        /// </summary>
        NormalizationRuleId = -2,

        /// <summary>
        /// Indicates job queue id column
        /// </summary>
        JobQueueId = -3,

        /// <summary>
        /// Indicates attribute value security id column
        /// </summary>
        AttributeValueSecurityId = -4,

        /// <summary>
        /// Indicates entity id column
        /// </summary>
        CNodeId = -5,

        /// <summary>
        /// Indicates attribute id column
        /// </summary>
        AttributeId = -6,

        /// <summary>
        /// Indicates locale id column
        /// </summary>
        LocaleId = -7,

        /// <summary>
        /// Indicates old attribute value column
        /// </summary>
        OldAttrValue = -8,

        /// <summary>
        /// Indicates new attribute value column
        /// </summary>
        NewAttrValue = -9,

        /// <summary>
        /// Indicates column to represent if normalization is successful
        /// </summary>
        IsNormalizationSuccessful = -10,

        /// <summary>
        /// Indicates last modified date and time
        /// </summary>
        LastModDateTime = -11,

        /// <summary>
        /// Indicates result message column
        /// </summary>
        ResultMessage = -12,

        /// <summary>
        /// Indicates normalization rule short name column
        /// </summary>
        NormalizationRuleShortName = -13,

        /// <summary>
        /// Indicates normalization rule long name column
        /// </summary>
        NormalizationRuleLongName = -14,

        /// <summary>
        /// Indicates attribute short name column
        /// </summary>
        AttributeShortName = -15,

        /// <summary>
        /// Indicates attribute long name column
        /// </summary>
        AttributeLongName = -16,

        /// <summary>
        /// Indicates entity short name column
        /// </summary>
        EntityShortName = -17,

        /// <summary>
        /// Indicates entity long name column
        /// </summary>
        EntityLongName = -18,

        /// <summary>
        /// Indicates entity type id column
        /// </summary>
        NodeTypeId = -19,

        /// <summary>
        /// Indicates entity type short name column
        /// </summary>
        NodeTypeShortName = -20,

        /// <summary>
        /// Indicates entity type long name column
        /// </summary>
        NodeTypeLongName = -21,

        /// <summary>
        /// Indicates container id column
        /// </summary>
        CatalogId = -22,

        /// <summary>
        /// Indicates container short name column
        /// </summary>
        CatalogShortName = -23,

        /// <summary>
        /// Indicates container long name column
        /// </summary>
        CatalogLongName = -24,

        /// <summary>
        /// Indicates category id column
        /// </summary>
        CategoryId = -25,

        /// <summary>
        /// Indicates category short name column
        /// </summary>
        CategoryShortName = -26,

        /// <summary>
        /// Indicates category long name column
        /// </summary>
        CategoryLongName = -27,

        /// <summary>
        /// Indicates organization id column
        /// </summary>
        OrgId = -28,

        /// <summary>
        /// Indicates organization short name column
        /// </summary>
        OrganizationShortName = -29,

        /// <summary>
        /// Indicates organization long name column
        /// </summary>
        OrganizationLongName = -30,
    }

    /// <summary>
    /// Specifies Normalization Report settings
    /// </summary>
    [DataContract]
    public class NormalizationReportSettings : INormalizationReportSettings
    {
        #region Fields

        /// <summary>
        /// Field denotes Ids of parent jobs
        /// </summary>
        private Collection<Int64> _jobIds = null;

        /// <summary>
        /// Field denotes locale for translations of the result column values
        /// </summary>
        private LocaleEnum _localeId = LocaleEnum.UnKnown;

        /// <summary>
        /// Field denotes CountFrom parameter for paged result 
        /// </summary>
        private Int64? _countFrom = null;

        /// <summary>
        /// Field denotes CountTo parameter for paged result
        /// </summary>
        private Int64? _countTo = null;

        /// <summary>
        /// Allows to request total items count including all pages
        /// </summary>
        private Boolean _requestTotalItemsCount = false;

        /// <summary>
        /// Denotes sort columns for paged result. It can have values of MergeResultReportPredefinedColumn (negative ones) or attributeId
        /// </summary>
        private Collection<Int32> _sortColumns = new Collection<Int32>();

        /// <summary>
        /// Denotes sort order for paged result. true for Descending, false for Ascending
        /// </summary>
        private Boolean _sortOrder = false;
        
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

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public NormalizationReportSettings()
        {
        } 

        #endregion

        #region Properties

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
        /// Denotes locale for translations of the result column values
        /// </summary>
        [DataMember]
        public LocaleEnum LocaleId
        {
            get { return _localeId; }
            set { _localeId = value; }
        }

        /// <summary>
        /// Denotes CountFrom parameter for paged result 
        /// </summary>
        [DataMember]
        public Int64? CountFrom
        {
            get { return _countFrom; }
            set { _countFrom = value; }
        }

        /// <summary>
        /// Denotes CountTo parameter for paged result
        /// </summary>
        [DataMember]
        public Int64? CountTo
        {
            get { return _countTo; }
            set { _countTo = value; }
        }

        /// <summary>
        /// Allows to request total items count including all pages
        /// </summary>
        [DataMember]
        public Boolean RequestTotalItemsCount
        {
            get { return _requestTotalItemsCount; }
            set { _requestTotalItemsCount = value; }
        }

        /// <summary>
        /// Denotes sort columns for paged result. It can have values of NormalizationReportPredefinedColumn (negative ones) or attributeId
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

        #endregion
    }
}