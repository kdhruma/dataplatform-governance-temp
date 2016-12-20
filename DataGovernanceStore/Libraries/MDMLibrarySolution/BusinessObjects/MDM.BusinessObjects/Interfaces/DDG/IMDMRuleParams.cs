using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Represents the interface that contains MDMRule Params information
    /// </summary>
    public interface IMDMRuleParams
    {
        /// <summary>
        /// Property denotes the Entity collection
        /// </summary>
        EntityCollection Entities { get; set; }

        /// <summary>
        /// Property denotes the collection of MDMEvents
        /// </summary>
        Collection<MDMEvent> Events { get; set; }

        /// <summary>
        /// Property denotes the User Security principal
        /// </summary>
        SecurityPrincipal UserSecurityPrincipal { get; set; }

        /// <summary>
        /// Property denotes the Caller context
        /// </summary>
        CallerContext CallerContext { get; set; }

        /// <summary>
        /// Property denotes the QualifiedRuleMaps
        /// </summary>
        Dictionary<Int64, MDMRuleMapDetailCollection> QualifiedRuleMaps { get; set; }

        /// <summary>
        /// Property denotes the Entity operation results
        /// </summary>
        EntityOperationResultCollection EntityOperationResults { get; set; }

        /// <summary>
        /// Property denotes the Entity family change context 
        /// </summary>
        EntityFamilyChangeContext EntityFamilyChangeContext { get; set; }

        /// <summary>
        /// Property denotes the workflow change context
        /// </summary>
        WorkflowChangeContext WorkflowChangeContext { get; set; }

        /// <summary>
        /// DDG Caller module
        /// </summary>
        DDGCallerModule DDGCallerModule { get; set; }

        /// <summary>
        /// Property denotes the Entity processing options
        /// </summary>
        EntityProcessingOptions EntityProcessingOptions { get; set; }
    }
}