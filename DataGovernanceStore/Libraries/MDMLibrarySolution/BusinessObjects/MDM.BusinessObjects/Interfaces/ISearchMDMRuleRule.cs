using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDM.BusinessObjects;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get search MDMRule rule.
    /// </summary>
    public interface ISearchMDMRuleRule : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Represents search operator for mdmrule
        /// </summary>
        SearchOperator Operator { get; set; }

        /// <summary>
        /// MDMRule 
        /// </summary>
        MDMRule MDMRule
        {
            get;
            set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of Search MDMRule Rule
        /// </summary>
        /// <returns>Xml representation of Search MDMRule Rule</returns>
        String ToXml();
        

        #endregion
    }
}
