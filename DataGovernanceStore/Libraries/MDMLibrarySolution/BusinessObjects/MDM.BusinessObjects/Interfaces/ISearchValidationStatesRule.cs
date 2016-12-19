using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects; 

    /// <summary>
    /// Exposes methods or properties to set or get search validationstates rule.
    /// </summary>
    public interface ISearchValidationStatesRule : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Represents search operator for rule
        /// </summary>
        SearchOperator Operator { get; set; }

        /// <summary>
        /// Validation Attribute
        /// </summary>
        SystemAttributes AttributeId { get; set; }


        /// <summary>
        /// Sate value selected
        /// </summary>
        ValidityStateValue Value { get; set; }
        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of Search validation states Rule
        /// </summary>
        /// <returns>Xml representation of Search Validation States Rule</returns>
        String ToXml();
        

        #endregion
    }
}
