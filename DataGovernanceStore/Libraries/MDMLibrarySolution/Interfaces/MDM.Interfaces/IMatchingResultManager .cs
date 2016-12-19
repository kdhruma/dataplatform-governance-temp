using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.DQM;
    using MDM.Interfaces;

    /// <summary>
    /// Interface for matching result business logic
    /// </summary>
    public interface IMatchingResultManager
    {
        /// <summary>
        /// Get MatchingProfile by EntityId
        /// </summary>
        /// <param name="profileId">Matching Profile Id</param>
        /// <param name="entityId">Entity Id</param>
        /// <param name="callerContext">Context details of caller</param>
        /// <param name="jobId"></param>
        /// <returns>Matching Result for the given ProfileId and EntityId</returns>
        MatchingResult GetMatchingResultByEntityId(Int32 profileId, Int64 entityId, CallerContext callerContext,
            Int64? jobId = null);

        /// <summary>
        /// Get MatchingResult by EntityIds
        /// </summary>
        /// <param name="profileId">Matching Profile Id</param>
        /// <param name="entityIds">Entity Ids</param>
        /// <param name="callerContext">Context details of caller</param>
        /// <param name="jobId"></param>
        /// <returns>Matching Result for the given ProfileId and EntityId</returns>
        MatchingResultCollection GetMatchingResultByEntityIds(Int32 profileId, Collection<Int64> entityIds,
            CallerContext callerContext, Int64? jobId = null);

        /// <summary>
        /// Get MatchingResult by Job Id
        /// </summary>
        /// <param name="jobId">Id of the Job</param>
        /// <param name="profileId">Matching Profile Id</param>
        /// <param name="entityIds">Collection of entity Id's</param>
        /// <param name="callerContext">Context details of caller</param>
        /// <returns>Matching Result for the given JobId, ProfileId and EntityId collection</returns>
        /// <exception cref="NotImplementedException"></exception>
        MatchingResultCollection GetMatchingResultByJobId(Int64? jobId, Int32? profileId, Collection<Int64> entityIds,
            CallerContext callerContext);

        /// <summary>
        /// Process MatchingResult
        /// </summary>
        /// <param name="profile">Matching Profile</param>
        /// <param name="resultCollection">Matching Result Collection</param>
        /// <param name="callerContext">Context of the caller</param>
        /// <param name="jobId">Id of the Job</param>
        /// <returns>Result of the operation</returns>
        OperationResult Process(MatchingProfile profile, IMatchingResultCollection resultCollection,
            CallerContext callerContext, Int64? jobId = null);
    }
}
