using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get the collection of search attribute rules.
    /// </summary>
    public interface ISearchAttributeRuleCollection : IEnumerable<SearchAttributeRule>
    {
        #region Properties
        #endregion

        #region Methods

        /// <summary>
        /// Get the XML presentation of an SearchAttributeRuleCollection
        /// </summary>
        /// <returns>Xml representation of Search Attribute Rule</returns>
        String ToXml();

        #endregion
    }
}
