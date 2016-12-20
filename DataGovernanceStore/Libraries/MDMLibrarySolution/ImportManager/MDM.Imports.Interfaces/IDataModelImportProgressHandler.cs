using System;

namespace MDM.Imports.Interfaces
{
    /// <summary>
    /// Specifies the DataModel import progress handler Interface.
    /// This track the dataModel import progress
    /// </summary>
    public interface IDataModelImportProgressHandler
    {
        /// <summary>
        /// Get the total number of DataModel records processed so far.
        /// </summary>
        /// <returns>Returns the total number of dataModel record count</returns>
        Int64 GetTotalDataModelRecords();

        /// <summary>
        /// Gets the completed dataModel records at a given point of time
        /// </summary>
        /// <returns>Returns the completed dataModel record count</returns>
        Int64 GetCompletedDataModelRecords();

        /// <summary>
        /// Gets the number of successfully completed dataModel records.
        /// </summary>
        /// <returns>Return the successful DataModel record count </returns>
        Int64 GetSuccessfulDataModelRecords();

        /// <summary>
        /// Clear the count of all entities
        /// </summary>
        void ResetDataModelRecordCounts();

        /// <summary>
        /// Gets the number of failed dataModel records
        /// </summary>
        /// <returns>Returns the failed dataModel record count</returns>
        Int64 GetFailedDataModelRecords();

        /// <summary>
        /// Updates the dataModel record count for success and failure at the end of a given batch.
        /// </summary>
        ///<param name="successfulDataModelRecords">Indicates the successful dataModel records count</param>
        ///<param name="failedDataModelRecords">Indicates the failed dataModel Record count</param>
        ///<param name="lastBatchCompletedRecords">Indicates the last batch completed record count</param>
        void UpdateCompletedDataModelBatch(Int64 successfulDataModelRecords, Int64 failedDataModelRecords, Int64 lastBatchCompletedRecords);

        /// <summary>
        /// Updates the dataModel record count for success and failure at the end of a given batch.
        /// </summary>
        ///<param name="successfulDataModelRecords">Indicates the successful dataModel records count</param>
        ///<param name="failedDataModelRecords">Indicates the failed dataModel Record count</param>
        void UpdateCompletedDataModelBatch(Int64 successfulDataModelRecords, Int64 failedDataModelRecords);
    }
}
