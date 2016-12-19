using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies DQMFilter
    /// </summary>
    [DataContract]
    public class DQMFilter : MDMObject, IDQMFilter
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the DQMFilter class
        /// </summary>
        /// <param name="containerIds">Collection of container ids</param>
        /// <param name="categoryIds">Collection of category ids</param>
        /// <param name="entityTypeIds">Collection of entity type ids</param>
        /// <param name="dataQualityIndicatorIds">Collection of DataQualityIndicator ids</param>
        /// <param name="dataQualityClassIds">Collection of DataQualityClass ids</param>
        /// <param name="searchAttributeRules">Collection of SearchAttributeRules</param>
        /// <param name="searchDataQualityIndicatorRules">Collection of SearchDataQualityIndicatorRules</param>
        /// <param name="searchColumnRules">Collection of SearchColumnRules</param>
        public DQMFilter(Collection<Int32> containerIds, Collection<Int64> categoryIds, Collection<Int32> entityTypeIds, Collection<Int16> dataQualityIndicatorIds, Collection<Int16> dataQualityClassIds,
            Collection<SearchAttributeRule> searchAttributeRules, Collection<SearchDataQualityIndicatorRule> searchDataQualityIndicatorRules, Collection<SearchColumnRule> searchColumnRules)
        {
            ContainerIds = containerIds;
            CategoryIds = categoryIds;
            EntityTypeIds = entityTypeIds;
            DataQualityIndicatorIds = dataQualityIndicatorIds;
            DataQualityClassIds = dataQualityClassIds;
            SearchAttributeRules = searchAttributeRules;
            SearchDataQualityIndicatorRules = searchDataQualityIndicatorRules;
            SearchColumnRules = searchColumnRules;
        }

        /// <summary>
        /// Initializes a new instance of the DQMFilter class
        /// </summary>
        public DQMFilter()
        {
            ContainerIds = new Collection<Int32>();
            CategoryIds = new Collection<Int64>();
            EntityTypeIds = new Collection<Int32>();
            DataQualityIndicatorIds = new Collection<Int16>();
            DataQualityClassIds = new Collection<Int16>();
            SearchAttributeRules = new Collection<SearchAttributeRule>();
            SearchDataQualityIndicatorRules = new Collection<SearchDataQualityIndicatorRule>();
            SearchColumnRules = new Collection<SearchColumnRule>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting container ids
        /// </summary>
        [DataMember]
        public Collection<Int32> ContainerIds { get; set; }

        /// <summary>
        /// Property denoting category ids
        /// </summary>
        [DataMember]
        public Collection<Int64> CategoryIds { get; set; }

        /// <summary>
        /// Property denoting entity type ids
        /// </summary>
        [DataMember]
        public Collection<Int32> EntityTypeIds { get; set; }

        /// <summary>
        /// Property denoting DataQualityIndicator ids
        /// </summary>
        [DataMember]
        public Collection<Int16> DataQualityIndicatorIds { get; set; }

        /// <summary>
        /// Property denoting data quality class Ids
        /// </summary>
        [DataMember]
        public Collection<Int16> DataQualityClassIds { get; set; }

        /// <summary>
        /// Property denoting filter Start Date time
        /// </summary>
        [DataMember]
        public DateTime? FilterStartDateTime { get; set; }

        /// <summary>
        /// Property denoting filter End Date time
        /// </summary>
        [DataMember]
        public DateTime? FilterEndDateTime { get; set; }

        /// <summary>
        /// Property denoting search attribute rules
        /// </summary>
        [DataMember]
        public Collection<SearchAttributeRule> SearchAttributeRules { get; set; }

        /// <summary>
        /// Property denoting search DataQualityIndicator rules
        /// </summary>
        [DataMember]
        public Collection<SearchDataQualityIndicatorRule> SearchDataQualityIndicatorRules { get; set; }

        /// <summary>
        /// Property denoting search SearchColumn rules
        /// </summary>
        [DataMember]
        public Collection<SearchColumnRule> SearchColumnRules { get; set; }
        
        #endregion
    }
}