using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get search DataQualityIndicator rule (where id of DataQualityIndicator for which rule is defined). 
    /// </summary>
    public interface ISearchDataQualityIndicatorRule
    {
        /// <summary>
        /// Specifies the Id of DataQualityIndicator for which rule is defined
        /// </summary>
        Int16 DataQualityIndicatorId { get; set; }

        /// <summary>
        /// Represents rule operator for search
        /// </summary>
        SearchOperator Operator { get; set; }
        
        /// <summary>
        /// Represents rule value for search
        /// </summary>
        Boolean? Value { get; set; }
    }
}
