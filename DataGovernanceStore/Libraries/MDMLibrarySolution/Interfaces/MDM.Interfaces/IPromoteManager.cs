using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Indicates Promote manager interface
    /// </summary>
    public interface IPromoteManager
    {
        /// <summary>
        /// Promotes the specified entity group queue item.
        /// </summary>
        /// <param name="promoteQueueItem">The entity queue item.</param>
        /// <param name="callerContext">The caller context.</param>
        /// <param name="groupEntityCollection"></param>
        /// <returns>Operation result collection containing the results</returns>
        EntityOperationResult ProcessPromote(EntityFamilyQueue promoteQueueItem, CallerContext callerContext, ref EntityCollection groupEntityCollection);

         /// <summary>
        /// Enqueues the entity for promote.
        /// </summary>
        /// <param name="entityId">The entity identifier.</param>
        /// <param name="callerContext">The caller context.</param>
        /// <returns>Returns the operation result collection indicating if the entity is successfully enqueued for promote or not.</returns>
        OperationResultCollection EnqueueForPromote(Collection<Int64> entityIdCollection, CallerContext callerContext, String businessRuleName = "", String businessRuleContextName = "");

         /// <summary>
        /// Enqueues the entities for promote.
        /// </summary>
        /// <param name="entityFamilyQueueCollection">The entity family queue collection.</param>
        /// <param name="callerContext">The caller context.</param>
        /// <returns>Returns the operation result collection indicating if the entity is successfully enqueued for promote or not.</returns>
        OperationResultCollection EnqueueForPromote(EntityFamilyQueueCollection entityFamilyQueueCollection, CallerContext callerContext);
    }
}
