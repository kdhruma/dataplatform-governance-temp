using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get value segment.
    /// </summary>
    public interface IValueSegment : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property denoting Left Bound
        /// </summary>
        Decimal? LeftBound { get; set; }

        /// <summary>
        /// Property denoting Left Bound Operator
        /// </summary>
        SearchOperator LeftBoundOperator { get; set; }

        /// <summary>
        /// Property denoting Right Bound
        /// </summary>
        Decimal? RightBound { get; set; }

        /// <summary>
        /// Property denoting Right Bound Operator
        /// </summary>
        SearchOperator RightBoundOperator { get; set; }

        #endregion
    }
}