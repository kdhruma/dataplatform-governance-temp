using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MDM.BusinessObjects
{
    using MDM.BusinessObjects.Interfaces;

    /// <summary>
    /// Class for sort criteria
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class SortCriteria : ISortCriteria
    {
        private String _sortParamter;
        private bool _isDescendingOrder;

        /// <summary>
        /// Property for Sort paramter
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        public String SortParameter
        {
            get
            {
                return _sortParamter;
            }
            set
            {
                _sortParamter = value;
            }
        }

        /// <summary>
        /// Indicates whether the sorted result should be ordered on descending order,
        /// default order would be an ascending order
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        public bool IsDescendingOrder
        {
            get
            {
                return _isDescendingOrder;
            }
            set
            {
                _isDescendingOrder = value;
            }
        }
    }
}
