namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.DQM;
    using MDM.BusinessObjects.DQMMerging;

    /// <summary>
    /// Interface for merge planning business logic
    /// </summary>
    public interface IMergePlanningManager
    {
        /// <summary>
        /// Allows to prepare Merge Plan
        /// </summary>
        /// <param name="matchingResults">Collection of matching results</param>
        /// <param name="matchReviewProfile">Match Review profile</param>
        /// <param name="callerContext">Context of the caller</param>
        /// <returns>Collection of merge plan items</returns>
        MergePlanItemCollection BuildMergePlan(MatchingResultCollection matchingResults,
            MatchReviewProfile matchReviewProfile, CallerContext callerContext);

        /// <summary>
        /// Build item of merge plan accrording to result of matching
        /// </summary>
        /// <param name="matchingResult">Result of matching</param>
        /// <param name="matchReviewProfile">Match Review profile</param>
        /// <param name="callerContext">Context of the caller</param>
        /// <returns>Item of merge plan</returns>
        MergePlanItem BuildMergePlanItem(MatchingResult matchingResult,
            MatchReviewProfile matchReviewProfile, CallerContext callerContext);
    }
}
