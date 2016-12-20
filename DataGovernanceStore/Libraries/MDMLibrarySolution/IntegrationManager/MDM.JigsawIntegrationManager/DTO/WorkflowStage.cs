using System;
using Newtonsoft.Json.Linq;

namespace MDM.JigsawIntegrationManager.DTO
{
    /// <summary>
    /// Represents class for JigsawWorkflowStage.
    /// </summary>
    /// <seealso cref="MDM.JigsawIntegrationManager.JigsawHelpers.IJigsawJsonSerializable" />
    public class WorkflowStage
    {
        /// <summary>
        /// Gets or sets the name of the stage.
        /// </summary>
        public String StageName
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        public String AssignedUser
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the group.
        /// </summary>
        public String AssignedRole
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the stage entered time.
        /// </summary>
        public String StageEnteredTime
        {
            get; set;
        }
         
    }
}