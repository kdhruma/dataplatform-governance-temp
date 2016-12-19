using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    /// <summary>
    /// Exposes methods or properties used for mapping hierachy category collection.
    /// </summary>
    public interface IHierachyCategoryMappingCollection : ICollection<HierachyCategoryMapping>, ICloneable
    {
        /// <summary>
        /// Add HierachyCategoryMapping in collection
        /// </summary>
        ///<param name="item">HierachyCategoryMapping to add in collection</param>
        void Add(IHierachyCategoryMapping item);
    }
}
