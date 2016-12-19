using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.DQM;

    /// <summary>
    /// Interface for mergePlanItemBL
    /// </summary>
    public interface IMergePlanItemManager
    {
        #region Get Methods

        /// <summary>
        /// Allows to get MergePlanItems by specified MergePlanItems Ids
        /// </summary>
        /// <param name="mergePlanItemIds">Merge Plan Item Ids filter</param>
        /// <param name="callerContext">Context details of caller</param>
        /// <returns>MergePlanItems for the given filter values</returns>
        MergePlanItemCollection GetByMpiIds(Collection<Int64> mergePlanItemIds, CallerContext callerContext);

        /// <summary>
        /// Allows to get all MergePlanItems for Matching or MergePlanning Job Id
        /// Job Id filters (<paramref name="matchingJobId"/> and <paramref name="mergePlanningJobId"/>) will be processed in OR mode.
        /// </summary>
        /// <param name="matchingJobId">Matching Job Id or null</param>
        /// <param name="mergePlanningJobId">MergePlanning Job Id or null</param>
        /// <param name="callerContext">Context details of caller</param>
        /// <returns>MergePlanItems for the given filter values</returns>
        MergePlanItemCollection GetAllForJobs(Int64? matchingJobId, Int64? mergePlanningJobId, CallerContext callerContext);

        /// <summary>
        /// Allows to get MergePlanItems by Job Id for specified Source Entities
        /// Job Id filters (<paramref name="matchingJobId"/> and <paramref name="mergePlanningJobId"/>) will be processed in OR mode.
        /// </summary>
        /// <param name="matchingJobId">Matching Job Id or null</param>
        /// <param name="mergePlanningJobId">MergePlanning Job Id or null</param>
        /// <param name="sourceEntityIds">Source Entity Ids filter</param>
        /// <param name="callerContext">Context details of caller</param>
        /// <returns>MergePlanItems for the given filter values</returns>
        MergePlanItemCollection GetByJobForSourceEntities(Int64? matchingJobId, Int64? mergePlanningJobId,
            Collection<Int64> sourceEntityIds, CallerContext callerContext);

        /// <summary>
        /// Get Merge Plan Items by collection of Ids
        /// </summary>
        /// <param name="sourceEntityIds">Collection of source entities</param>
        /// <param name="mergePlanItemIds">Collections of merge Planning items Ids</param>
        /// <param name="callerContext">Context details of caller</param>
        /// <param name="matchingJobId">Id of matching job</param>
        /// <param name="mergePlanningJobId">Id of merge planing job</param>
        /// <returns>Collection of Merge Plan Items</returns>
        MergePlanItemCollection Get(Int64? matchingJobId, Int64? mergePlanningJobId, Collection<Int64> sourceEntityIds,
            Collection<Int64> mergePlanItemIds, CallerContext callerContext);
        
        #endregion

        #region CUD operations

        /// <summary>
        /// Create new Merge Planning Item
        /// </summary>
        /// <param name="mergePlanItem">Merge Planning Item to create</param>
        /// <param name="callerContext">Context details of caller</param>
        /// <returns>Operation result</returns>
        OperationResult Create(MergePlanItem mergePlanItem, CallerContext callerContext);


        /// <summary>
        /// Update the existing Merge Planning Item
        /// </summary>
        /// <param name="mergePlanItem">Merge Planning Item to update</param>
        /// <param name="callerContext">Context details of caller</param>
        /// <returns>Operation result</returns>
        OperationResult Update(MergePlanItem mergePlanItem, CallerContext callerContext);

        /// <summary>
        /// Delete the existing Merge Planning Item
        /// </summary>
        /// <param name="mergePlanItem">Merge Planning Item to delete</param>
        /// <param name="callerContext">Context details of caller</param>
        /// <returns>Operation result</returns>
        OperationResult Delete(MergePlanItem mergePlanItem, CallerContext callerContext);


        /// <summary>
        /// Supports create, update or delete operations for Merge Plan Items
        /// </summary>
        /// <param name="mergePlanItems">Collection of Merge Plan Items</param>
        /// <param name="callerContext">Context details of caller</param>
        /// <returns>Operation result</returns>
        OperationResult Process(MergePlanItemCollection mergePlanItems, CallerContext callerContext);

        #endregion
    }
}
