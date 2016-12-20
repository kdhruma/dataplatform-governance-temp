using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get search mdmdrules rule group.
    /// </summary>
    public interface ISearchMDMRuleRuleGroup : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Represents group operator for MDMRule rule
        /// </summary>
        ConditionalOperator GroupOperator { get; set; }

        /// <summary>
        /// Represents rule operator for MDMRule rule
        /// </summary>
        ConditionalOperator RuleOperator { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of Search MDMRule Rule group
        /// </summary>
        /// <returns>Xml representation of Search MDMRule Rule group</returns>
        String ToXml();

        #endregion
    }
}
