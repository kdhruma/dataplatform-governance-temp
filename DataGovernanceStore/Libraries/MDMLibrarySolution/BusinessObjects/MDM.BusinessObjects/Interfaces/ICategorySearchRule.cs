using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get category search rule instance.
    /// </summary>
    public interface ICategorySearchRule
    {
        #region Properties

        /// <summary>
        /// Field denoting the Property of Category on which search should happened
        /// </summary>
        CategoryField CategoryField { get; set; }

        /// <summary>
        /// Field denoting the Search Operator 
        /// </summary>
        SearchOperator SearchOperator { get; set; }

        /// <summary>
        /// Field denoting value that need to be searched.
        /// </summary>
        String SearchValue { get; set; }

        /// <summary>
        /// Field denoting The value separator
        /// </summary>
        String ValueSeparator { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of CategorySearchRule
        /// </summary>
        /// <returns>Xml representation of CategorySearchRule</returns>
        String ToXml();

        #endregion
    }
}
