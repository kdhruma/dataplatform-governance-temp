using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Class describes HierachyCategoryMappingCollection
    /// </summary>
    [DataContract]
    public class HierachyCategoryMappingCollection : InterfaceContractCollection<IHierachyCategoryMapping, HierachyCategoryMapping>, IHierachyCategoryMappingCollection
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the hierachyCategoryMapping Collection
        /// </summary>
        public HierachyCategoryMappingCollection()
        { }

        /// <summary>
        /// Initialize hierachyCategoryMappings collection from IList
        /// </summary>
        /// <param name="hierachyCategoryMappings">Source items</param>
        public HierachyCategoryMappingCollection(IList<HierachyCategoryMapping> hierachyCategoryMappings)
        {
            this._items = new Collection<HierachyCategoryMapping>(hierachyCategoryMappings);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Implementaion of ICloneable Interface
        /// </summary>
        /// <returns>Cloned HierachyCategoryMappingCollection object</returns>
        public object Clone()
        {
            HierachyCategoryMappingCollection clonedRules = new HierachyCategoryMappingCollection();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (HierachyCategoryMapping item in this._items)
                {
                    HierachyCategoryMapping clonedItem = item.Clone() as HierachyCategoryMapping;
                    clonedRules.Add(clonedItem);
                }
            }

            return clonedRules;
        }

        /// <summary>
        /// Add HierachyCategoryMapping in collection
        /// </summary>
        /// <param name="item">HierachyCategoryMapping to add in collection</param>
        public new void Add(IHierachyCategoryMapping item)
        {
            this._items.Add((HierachyCategoryMapping)item);
        }

        #endregion
    }
}
