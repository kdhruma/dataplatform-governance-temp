using System;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;

namespace MDM.JigsawIntegrationManager.DTO
{
    using MDM.BusinessObjects.Workflow;
    using System.Collections.Generic;

    /// <summary>
    /// Represents class for JigsawWorkflow.
    /// </summary>
    /// <seealso cref="MDM.JigsawIntegrationManager.JigsawHelpers.IJigsawJsonSerializable" />
    public class Workflow
    {
        /// <summary>
        /// Gets or sets the name of the workflow.
        /// </summary>
        public String WorkflowName { get; set; }

        /// <summary>
        /// Gets or sets the workflow version.
        /// </summary>
        public String WorkflowVersion { get; set; }

        /// <summary>
        /// Gets or sets the jigsaw workflow stage.
        /// </summary>
        public ICollection<WorkflowStage> WorkflowStages { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<String> CurrentWorkflows { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<String> CurrentStages { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Workflow()
        {
            WorkflowStages = new Collection<WorkflowStage>();
            CurrentWorkflows = new Collection<String>();
            CurrentStages = new Collection<String>();
        }
    }
}