using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Interface containing the methods to handle Export Queue logic
    /// </summary>
    public interface IExportQueueManager
    {
        /// <summary>
        /// Processes the specified export queue collection.
        /// </summary>
        /// <param name="exportQueueCollection">The export queue collection.</param>
        /// <param name="hasError">Indicates whether specified export queues has been errored out </param>
        /// <param name="callerContext">The caller context.</param>
        /// <returns>Operation result collection containing the results of the operation</returns>
        OperationResultCollection Process(ExportQueueCollection exportQueueCollection, Boolean hasError, CallerContext callerContext);

        /// <summary>
        /// Processes the specified entity identifier for export queue for collaboration delta profiles.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="profileNames">The profile names.</param>
        /// <param name="callerContext">The caller context.</param>
        /// <returns>
        /// Operation result containing the results of the operation
        /// </returns>
        OperationResult ProcessForCollaborationProfiles(IEntity entity, Collection<String> profileNames, CallerContext callerContext);
    }
}
