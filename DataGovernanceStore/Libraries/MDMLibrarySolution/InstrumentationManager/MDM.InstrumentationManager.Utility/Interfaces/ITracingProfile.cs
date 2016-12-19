using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces.Diagnostics
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Diagnostics;

    /// <summary>
    /// Specifies interface for Tracing Profile
    /// </summary>
    public interface ITracingProfile
    {
        /// <summary>
        /// Specifies Start DateTime filter
        /// </summary>
        DateTime StartDateTime { get; set; }

        /// <summary>
        /// Specifies End DateTime filter
        /// </summary>
        DateTime EndDateTime { get; set; }

        /// <summary>
        /// Specifies minimum threshold for diagnostic activities duration in milliseconds
        /// </summary>
        Int32? DurationMinimumThresholdInMilliSeconds { get; set; }

        /// <summary>
        /// Specifies maximum threshold for diagnostic activities duration in milliseconds
        /// </summary>
        Int32? DurationMaximumThresholdInMilliSeconds { get; set; }

        /// <summary>
        /// Specifies CallerContext filter
        /// </summary>
        CallerContextFilter CallerContextFilter { get; set; }

        /// <summary>
        /// Specifies SecurityContext filter
        /// </summary>
        SecurityContextFilter SecurityContextFilter { get; set; }

        /// <summary>
        /// Specifies CallDataContext filter
        /// </summary>
        CallDataContext CallDataContext { get; set; }

        /// <summary>
        /// Specifies the legacy MDM trace source values
        /// </summary>
        Collection<MDMTraceSource> LegacyMDMTraceSources { get; set; }

        /// <summary>
        /// Specifies TraceSettings
        /// </summary>
        TraceSettings TraceSettings { get; set; }

        /// <summary>
        /// Specifies MessageClasses filter
        /// </summary>
        Collection<MessageClassEnum> MessageClasses { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        String ToXml();

        /// <summary>
        /// Check specified activity by profile filters
        /// </summary>
        /// <param name="diagnosticActivity">Activity to check</param>
        Boolean CheckActivity(DiagnosticActivity diagnosticActivity);

        /// <summary>
        /// Check specified CallDataContext by profile filters
        /// </summary>
        /// <param name="callDataContext"><see cref="TracingProfile.CallDataContext"/> to check</param>
        Boolean CheckCallDataContext(CallDataContext callDataContext);

        /// <summary>
        /// Check specified SecurityContext by profile filters
        /// </summary>
        /// <param name="securityContext"><see cref="SecurityContext"/> to check</param>
        Boolean CheckSecurityContext(SecurityContext securityContext);

        /// <summary>
        /// Check specified CallerContext by profile filters
        /// </summary>
        /// <param name="callerContext"><see cref="CallerContext"/> to check</param>
        Boolean CheckCallerContext(CallerContext callerContext);
    }
}