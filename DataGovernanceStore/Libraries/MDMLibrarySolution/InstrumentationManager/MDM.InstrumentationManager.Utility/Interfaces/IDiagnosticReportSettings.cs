using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces.Diagnostics
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get diagnostic report filter
    /// </summary>
    public interface IDiagnosticReportSettings
    {
        #region Properties

        /// <summary>
        /// Property denoting DataRequestType for diagnostic
        /// </summary>
        Int32 DataRequestType { get; set; }

        /// <summary>
        /// Property denoting level of the diagnostic Activity
        /// </summary>
        Int32 Level { get; set; }

        /// <summary>
        /// Property denoting collection of message class
        /// </summary>
        Collection<MessageClassEnum> MessageClasses { get; set; }

        /// <summary>
        /// Property denoting duration of an activity
        /// </summary>
        Int32? Duration { get; set; }

        /// <summary>
        /// Property denoting search operator for an Activity duration
        /// </summary>
        SearchOperator? DurationOperator { get; set; }

        /// <summary>
        /// Property denoting keywords to search in diagnostic activities. Please also set <see cref="IDiagnosticReportSettings.SearchColumns"/> list as scope specification for this kind of search (keyword based).
        /// </summary>
        Collection<String> SearchKeywords { get; set; }

        /// <summary>
        /// Property denoting search columns to search in during keyword based search. Please also set <see cref="IDiagnosticReportSettings.SearchKeywords"/>.
        /// </summary>
        Collection<SearchColumn> SearchColumns { get; set; }

        /// <summary>
        /// Property denoting maximum record to return 
        /// </summary>
        Int32? MaxRecordsToReturn { get; set; }

        /// <summary>
        /// Property denoting whether to include/not include extended Data column in diagnostic record 
        /// </summary>
        Boolean IncludeActivityExtendedData { get; set; }

        /// <summary>
        /// Property denoting whether to include/not include execution context columns (except of AdditionalContextData column, please use <see cref="IncludeExecutionContextExtendedData"/> property for this column requesting) in diagnostic record
        /// </summary>
        Boolean IncludeContextData { get; set; }

        /// <summary>
        /// Property denoting whether to include/not include AdditionalContextData column in diagnostic record
        /// </summary>
        Boolean IncludeExecutionContextExtendedData { get; set; }

        /// <summary>
        /// Property denoting filter based on Activity Extended Data existance. Please set to <c>True</c> if you want to see only items with extended data.
        /// Please set to <c>False</c> if you want to see only items without extended data.
        /// Please set to <c>null</c> if you want to ignore extended data existance.
        /// </summary>
        Boolean? HasActivityExtendedData { get; set; }

        /// <summary>
        /// Property denoting CallerContext filter
        /// </summary>
        CallerContextFilter CallerContextFilter { get; set; }

        /// <summary>
        /// Property denoting SecurityContext filter
        /// </summary>
        SecurityContextFilter SecurityContextFilter { get; set; }

        /// <summary>
        /// Property denoting CallDataContext filter
        /// </summary>
        CallDataContext CallDataContext { get; set; }

        /// <summary>
        /// Property denoting legacy MDM trace source filter
        /// </summary>
        Collection<MDMTraceSource> LegacyMDMTraceSources { get; set; }

        /// <summary>
        /// Property denoting FromDateTime filter
        /// </summary>
        DateTime? FromDateTime { get; set; }

        /// <summary>
        /// Property denoting ToDateTime filter
        /// </summary>
        DateTime? ToDateTime { get; set; }

        /// <summary>
        /// Property denoting substrings to search in diagnostic record message properties
        /// </summary>
        Collection<String> Messages { get; set; }

        /// <summary>
        /// Property denoting thread Ids filter
        /// </summary>
        Collection<Int32> ThreadIds { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get XML representation of DiagnosticReportSettings
        /// </summary>
        /// <returns>XML representation of DiagnosticReportSettings</returns>
        String ToXml();

        #endregion
    }
}