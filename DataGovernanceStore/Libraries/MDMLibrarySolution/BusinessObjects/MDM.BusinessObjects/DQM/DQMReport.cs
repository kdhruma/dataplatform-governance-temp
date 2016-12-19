using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Interfaces;
    using MDM.Core;

    /// <summary>
    /// Specifies DQM Report parameters
    /// </summary>
    [DataContract]
    public class DQMReport : IDQMReport
    {
        #region Constructors
        
        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public DQMReport()
        {
            SortColumns = new Collection<String>();
            EntityIds = new Collection<Int64>();
            AttributeIds = new Collection<Int32>();
            Filter = new DQMFilter();
        } 

        #endregion

        #region Public properties

        /// <summary>
        /// Denotes filter parameters. There are should be only one DataQualityClass in associated collection
        /// </summary>
        [DataMember]
        public DQMFilter Filter { get; set; }

        /// <summary>
        /// Denotes locale for translations of the result column values
        /// </summary>
        [DataMember]
        public LocaleEnum LocaleId { get; set; }

        /// <summary>
        /// Denotes countFrom parameter for paged result 
        /// </summary>
        [DataMember]
        public Int64 CountFrom { get; set; }

        /// <summary>
        /// Denotes countTo parameter for paged result
        /// </summary>
        [DataMember]
        public Int64 CountTo { get; set; }

        /// <summary>
        /// Denotes sort columns for paged result. It could be any column of result including attributeId e.x. SortColumn = "Category" or SortColumn = "4037"
        /// Support multiple sort columns. The order of columns defines presedence for sorting
        /// </summary>        
        [DataMember]
        public ICollection<String> SortColumns { get; set; }

        /// <summary>
        /// Denotes sort order for paged result. true for Descending, false for Ascending 
        /// </summary>
        [DataMember]
        public Boolean SortOrder { get; set; }

        /// <summary>
        /// Denotes collection of result entities
        /// </summary>
        [DataMember]
        public ICollection<Int64> EntityIds { get; set; }

        /// <summary>
        /// Denotes collection of result attributes
        /// </summary>
        [DataMember]
        public ICollection<Int32> AttributeIds { get; set; }

        /// <summary>
        /// Denotes data source for getting attributes
        /// </summary>
        [DataMember]
        public String AttributeDataSource { get; set; }
    
        #endregion
    }
}