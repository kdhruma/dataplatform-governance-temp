using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.JigsawIntegrationManager.DataPackages
{
    /// <summary>
    /// 
    /// </summary>
    public enum EventType
    {
        workflowStarted,
        workflowCompleted,
        workflowTransitioned,
        workflowAssignmentChanged,
        workflowTerminated,
        entityExport,
        entityPromote,
        none
    }

    /// <summary>
    /// 
    /// </summary>
    public enum EventSubType
    {
		unknown,
        promte,
        autoPromote,
        emergencyPromote,
        categoryPromote,
        upstreamPromote,
        entityExportFull,
		entityExportDelta,
		entityExportStart,
		entityExportEnd,
		entityExportFailed,
		none
    }
}
