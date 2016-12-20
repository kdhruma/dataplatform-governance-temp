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
    public class WorkflowInfo
    {
        /// <summary>
        /// Gets or sets the jigsaw workflow stage.
        /// </summary>
        public ICollection<Workflow> Workflows { get; set; }

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
        public WorkflowInfo()
        {
            Workflows = new Collection<Workflow>();
            CurrentWorkflows = new Collection<String>();
            CurrentStages = new Collection<String>();
        }
    }
}