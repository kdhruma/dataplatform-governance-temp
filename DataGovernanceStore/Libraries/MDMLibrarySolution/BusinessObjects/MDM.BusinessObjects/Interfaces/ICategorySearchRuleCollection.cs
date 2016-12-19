using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get collection of category search rule.
    /// </summary>
    public interface ICategorySearchRuleCollection : IEnumerable<CategorySearchRule>, ICollection<CategorySearchRule>
    {
        #region Properties
        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of CategorySearchRules
        /// </summary>
        /// <returns>Xml representation of CategorySearchRules</returns>
        String ToXml();

        /// <summary>
        /// Add CategorySearchRule in collection
        /// </summary>
        ///<param name="item">CategorySearchRule to add in collection</param>
        void Add(ICategorySearchRule item);

        #endregion
    }
}
