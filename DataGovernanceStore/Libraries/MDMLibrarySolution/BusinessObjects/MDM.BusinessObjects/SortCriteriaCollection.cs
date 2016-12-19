using MDM.BusinessObjects.Interfaces;
using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MDM.BusinessObjects
{
    using MDM.Core;

    /// <summary>
    /// Represents the collection of SortCriteria
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class SortCriteriaCollection : InterfaceContractCollection<ISortCriteria, SortCriteria>, ISortCriteriaCollection
    {
       #region Constructors

        /// <summary>
        /// Initializes a new instance of the SortCriteria Collection
        /// </summary>
        public SortCriteriaCollection() 
        { }

        /// <summary>
        /// Initialize SortCriteria collection from IList
        /// </summary>
        /// <param name="sortCriteriaList">Source items</param>
        public SortCriteriaCollection(IList<SortCriteria> sortCriteriaList)
        {
            this._items = new Collection<SortCriteria>(sortCriteriaList);
        }

        #endregion
    }
}
