using System;

namespace MDM.JigsawIntegrationManager.DTO
{
    /// <summary>
    /// Class ChangeContext.
    /// </summary>
    public class ChangeContext
    {
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        public String User { get; set; }

        /// <summary>
        /// Gets or sets the change agent.
        /// </summary>
        public String ChangeAgent { get; set; }

        /// <summary>
        /// Gets or sets the type of the change agent.
        /// </summary>
        public String ChangeAgentType { get; set; }

        /// <summary>
        /// Gets or sets the change interface.
        /// </summary>
        public String ChangeInterface { get; set; }

        /// <summary>
        /// Gets or sets the source timestamp.
        /// </summary>
        public String SourceTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the ingest timestamp.
        /// </summary>
        public String IngestTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the job id.
        /// </summary>
        public String JobId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JigsawChangeContext"/> class.
        /// </summary>
        public ChangeContext()
        {
            
        }
    }
}