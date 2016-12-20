using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the MDMRule Parameters
    /// </summary>
    public class MDMRuleParams : IMDMRuleParams
    {
        #region Properties

        /// <summary>
        /// Property denotes the Entity collection
        /// </summary>
        public EntityCollection Entities { get; set; }

        /// <summary>
        /// Property denotes the collection of MDMEvents
        /// </summary>
        public Collection<MDMEvent> Events { get; set; }

        /// <summary>
        /// Property denotes the User Security principal
        /// </summary>
        public SecurityPrincipal UserSecurityPrincipal { get; set; }

        /// <summary>
        /// Property denotes the Caller context
        /// </summary>
        public CallerContext CallerContext { get; set; }

        /// <summary>
        /// Property denotes the QualifiedRuleMaps
        /// </summary>
        public Dictionary<Int64, MDMRuleMapDetailCollection> QualifiedRuleMaps { get; set; }

        /// <summary>
        /// Property denotes the Entity operation results
        /// </summary>
        public EntityOperationResultCollection EntityOperationResults { get; set; }

        /// <summary>
        /// Property denotes the Entity family change context 
        /// </summary>
        public EntityFamilyChangeContext EntityFamilyChangeContext { get; set; }

        /// <summary>
        /// Property denotes the workflow change context
        /// </summary>
        public WorkflowChangeContext WorkflowChangeContext { get; set; }

        /// <summary>
        /// DDG Caller module
        /// </summary>
        public DDGCallerModule DDGCallerModule { get; set; }

        /// <summary>
        /// Property denotes the Entity processing options
        /// </summary>
        public EntityProcessingOptions EntityProcessingOptions { get; set; }

        #endregion Properties
    }
}