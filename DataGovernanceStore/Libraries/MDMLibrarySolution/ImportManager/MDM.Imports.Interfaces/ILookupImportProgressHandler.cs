using System;

namespace MDM.Imports.Interfaces
{
    /// <summary>
    /// Specifies the Lookup import progress handler Interface.
    /// This track the lookup import progress
    /// </summary>
    public interface ILookupImportProgressHandler
    {
        /// <summary>
        /// Get the total number of Lookup records processed so far.
        /// </summary>
        /// <returns>Returns the total number of lookup record count</returns>
        Int64 GetTotalLookupRecords();

        /// <summary>
        /// Gets the completed lookup records at a given point of time
        /// </summary>
        /// <returns>Returns the completed lookup record count</returns>
        Int64 GetCompletedLookupRecords();

        /// <summary>
        /// Gets the number of successfully completed lookup records.
        /// </summary>
        /// <returns>Return the successful Lookup record count </returns>
        Int64 GetSuccessfulLookupRecords();

        /// <summary>
        /// Clear the count of all entities
        /// </summary>
        void ResetLookupRecordCounts();

        /// <summary>
        /// Gets the number of failed lookup records
        /// </summary>
        /// <returns>Returns the failed lookup record count</returns>
        Int64 GetFailedLookupRecords();

        /// <summary>
        /// Updates the lookup record count for success and failure at the end of a given batch.
        /// </summary>
        ///<param name="successfulLookupRecords">Indicates the successful lookup records count</param>
        ///<param name="failedLookupRecords">Indicates the failed lookup Record count</param>
        ///<param name="lastBatchCompletedRecords">Indicates the last batch completed record count</param>
        void UpdateCompletedLookupBatch(Int64 successfulLookupRecords, Int64 failedLookupRecords, Int64 lastBatchCompletedRecords);

        /// <summary>
        /// Updates the lookup record count for success and failure at the end of a given batch.
        /// </summary>
        ///<param name="successfulLookupRecords">Indicates the successful lookup records count</param>
        ///<param name="failedLookupRecords">Indicates the failed lookup Record count</param>
        void UpdateCompletedLookupBatch(Int64 successfulLookupRecords, Int64 failedLookupRecords);
    }
}
