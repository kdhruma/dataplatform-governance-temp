using System;

namespace MDM.JigsawIntegrationManager.DataPackages
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.Core.Extensions;
    using MDM.BusinessObjects.Workflow;
    
    /// <summary>
    /// Represents class for EntityMessageDataPackage
    /// </summary>
    public class WorkflowEventData : IEventData
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public String WorkflowName {get ; set; }

        /// <summary>
        /// 
        /// </summary>
        public String WorkflowVersion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime WorkflowStartTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime WorkflowEndTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String WorkflowStageFrom { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String WorkflowStageTo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime WorkflowStageFromDateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime WorkflowStageToDateTime{ get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String WorkflowStageActionTaken { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String WorkflowStageActionComments { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String WorkflowAssignedFrom { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String WorkflowAssignedTo { get; set; }

        /// <summary>
        /// Computes the total time an entity was in stage in seconds
        /// </summary>
        public Double WorkflowStageTotalTime { get; set; }

        #endregion
    }
}