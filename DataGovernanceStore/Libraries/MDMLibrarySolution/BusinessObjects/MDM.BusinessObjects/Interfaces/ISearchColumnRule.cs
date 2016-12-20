using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get search criteria based on column name for which rule is defined.
    /// </summary>
    public interface ISearchColumnRule
    {
        /// <summary>
        /// Specifies the ColumnName for which rule is defined
        /// </summary>
        String ColumnName { get; set; }

        /// <summary>
        /// Represents rule operator for search
        /// </summary>
        SearchOperator Operator { get; set; }

        /// <summary>
        /// Represents rule value for search
        /// </summary>
        String Value { get; set; }
    }
}