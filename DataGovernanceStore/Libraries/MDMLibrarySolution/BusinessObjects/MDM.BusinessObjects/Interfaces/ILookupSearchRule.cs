using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get lookup search rule related information.
    /// </summary>
    public interface ILookupSearchRule
    {
        #region Properties
        /// <summary>
        /// Unique identifier for each rule
        /// </summary>
        Int32 RuleId { get; set; }

        /// <summary>
        /// Column name on which search needs to be performed
        /// </summary>
        String SearchColumnName { get; set; }
        
        /// <summary>
        /// operator to search the value from a column
        /// </summary>
        LookupSearchOperatorEnum LookupSearchOperator { get; set; }

        /// <summary>
        /// value that needs to be searched from a column
        /// </summary>
        String SearchValue { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of LookupSearchRule object
        /// </summary>
        /// <returns>Xml representation of LookupSearchRule object</returns>
        String ToXml();

        #endregion
    }
}
