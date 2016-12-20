using MDM.BusinessObjects.Interfaces;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MDM.BusinessObjects
{
    /// <summary>
    /// Paging criteria to be used to fetch large items according to page number, page size and sort param
    /// </summary>
    [DataContract]
    [ProtoContract]
    [KnownType(typeof(SortCriteriaCollection))]
    [KnownType(typeof(FilterCriteriaCollection))]
    public class PagingCriteria : IPagingCriteria
    {

        private Int32 _pageSize;
        private Int32 _pageNumber;
        private ISortCriteriaCollection _sortParameters;
        private ICollection<String> _groupByParameters;
        private IFilterCriteriaCollection _filterCriteriaList;

        /// <summary>
        /// Number of records required in the page
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        public Int32 PageSize 
        {
            get { return _pageSize; }
            set { _pageSize = value; } 
        }

        /// <summary>
        /// The page number of the records to retrieve
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        public Int32 PageNumber 
        {
            get { return _pageNumber; }
            set { _pageNumber = value; }
        }

        /// <summary>
        /// The sort parameters for the paginated records
        /// </summary>
        [DataMember]
        [ProtoMember(3)]
        public ISortCriteriaCollection SortParameters 
        {
            get { return _sortParameters; }
            set { _sortParameters = value; }
        }

        /// <summary>
        /// The groupby parameter to group the paginated records
        /// </summary>
        [DataMember]
        [ProtoMember(4)]
        public ICollection<String> GroupByParameters 
        {
            get { return _groupByParameters; }
            set { _groupByParameters = value; }
        }

        /// <summary>
        /// Property to filter the records based on provided filter criteria
        /// </summary>
        [DataMember]
        [ProtoMember(5)]
        public IFilterCriteriaCollection FilterCriteriaList
        {
            get { return _filterCriteriaList; }
            set { _filterCriteriaList = value; }
        }
    }
}
