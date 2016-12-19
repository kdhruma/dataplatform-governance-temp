using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Interfaces for DDGOperationResult Summary
    /// </summary>
    public interface IDDGOperationResultSummary
    {
        #region Properties

        /// <summary>
        /// Property denoting DDG ObjectType
        /// </summary>
        String ObjectType { get; set; }

        /// <summary>
        /// Property denoting sheet name
        /// </summary>
        String ObjectName { get; set; }

        /// <summary>
        /// Property denoting total object count
        /// </summary>
        Int32 ToatalCount { get; set; }

        /// <summary>
        /// Property denoting pending object count
        /// </summary>
        Int32 PendingCount { get; set; }

        /// <summary>
        /// Property denoting success object count
        /// </summary>
        Int32 SuccessCount { get; set; }

        /// <summary>
        /// Property denoting failed object count
        /// </summary>
        Int32 FailedCount { get; set; }

        /// <summary>
        /// Property denoting completedwitherrors object count
        /// </summary>
        Int32 CompletedWithErrorsCount { get; set; }

        /// <summary>
        /// Property denoting completedwithwarnings object count
        /// </summary>
        Int32 CompletedWithWarningsCount { get; set; }

        /// <summary>
        /// Determines overall status of Operation
        /// </summary>
        OperationResultStatusEnum SummaryStatus { get; }

        #endregion Properties
    }
}
