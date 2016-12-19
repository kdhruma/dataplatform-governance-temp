using System;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get merge operation results.
    /// </summary>
    public interface IMergeOperationResult: IOperationResult
    {
        /// <summary>
        /// Indicates the name of Workflow
        /// </summary>
        String WorkflowName { get; set; }

        /// <summary>
        /// Indicates Entity Id
        /// </summary>
        Int64? FinalEntityId { get; set; }

        /// <summary>
        /// Indicates Relationship Type ID
        /// </summary>
        Int32? RelationshipTypeId { get; set; }
    }
}
