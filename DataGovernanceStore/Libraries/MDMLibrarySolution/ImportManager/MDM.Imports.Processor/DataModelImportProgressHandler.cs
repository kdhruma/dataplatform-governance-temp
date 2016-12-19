using System;
using System.Collections.ObjectModel;

namespace MDM.Imports.Processor
{
    using MDM.Core;
    using MDM.Imports.Interfaces;

    /// <summary>
    /// Specifies the DataModel import progress handler class.
    /// This track the DataModel import progress
    /// </summary>
    public class DataModelImportProgressHandler : IDataModelImportProgressHandler
    {
        #region Fields

        internal static Object lockObject = new Object();
        private Int64 _totalDataModelRecords = 0;
        private Int64 _completedDataModelrecords = 0;
        private Int64 _successDataModelRecords = 0;
        private Int64 _failedDataModelRecords = 0;
        private JobStatus _jobStatus = JobStatus.UnKnown;
        private Collection<LocaleEnum> _localeList = new Collection<LocaleEnum>();
        private Collection<LocaleEnum> _processedLocaleList = new Collection<LocaleEnum>();
        private Collection<String> _referenceCodes = new Collection<String>();

        #endregion

        #region Methods

        /// <summary>
        /// Get the total number of DataModel records processed so far.
        /// </summary>
        /// <returns>Returns the total number of DataModel record count</returns>
        public Int64 GetTotalDataModelRecords()
        {
            return _totalDataModelRecords;
        }

        /// <summary>
        /// Set the total number of DataModel records yet to process
        /// </summary>
        public void SetTotalDataModelRecords(Int64 count)
        {
            lock (lockObject)
            {
                _totalDataModelRecords = count;
                _jobStatus = JobStatus.Running;
            }
        }

        /// <summary>
        /// Gets the completed DataModel records at a given point of time
        /// </summary>
        /// <returns>Returns the completed DataModel record count</returns>
        public Int64 GetCompletedDataModelRecords()
        {
            return _completedDataModelrecords;
        }

        /// <summary>
        /// Gets the number of successfully completed DataModel records.
        /// </summary>
        /// <returns>Return the successful DataModel record count </returns>
        public Int64 GetSuccessfulDataModelRecords()
        {
            return _successDataModelRecords / this._localeList.Count;
        }

        /// <summary>
        /// Clear the count of all entities
        /// </summary>
        public void ResetDataModelRecordCounts()
        {
            lock (lockObject)
            {
                _totalDataModelRecords = 0;
                _completedDataModelrecords = 0;
                _successDataModelRecords = 0;
                _failedDataModelRecords = 0;
            }
        }

        /// <summary>
        /// Gets the number of failed DataModel records
        /// </summary>
        /// <returns>Returns the failed DataModel record count</returns>
        public Int64 GetFailedDataModelRecords()
        {
            return _failedDataModelRecords;
        }

        /// <summary>
        /// Updates the DataModel record count for success and failure at the end of a given batch.
        /// </summary>
        ///<param name="successfulDataModelRecords">Indicates the successful DataModel records count</param>
        ///<param name="failedDataModelRecords">Indicates the failed DataModel Record count</param>
        ///<param name="lastBatchCompletedRecords">Indicates the last batch completed record count</param>
        public void UpdateCompletedDataModelBatch(Int64 successfulDataModelRecords, Int64 failedDataModelRecords, Int64 lastBatchCompletedRecords)
        {
            lock (lockObject)
            {
                _successDataModelRecords += successfulDataModelRecords;
                _failedDataModelRecords += failedDataModelRecords;
                _completedDataModelrecords += (successfulDataModelRecords + failedDataModelRecords + lastBatchCompletedRecords);
            }
        }

        /// <summary>
        /// Updates the DataModel record count for success and failure at the end of a given batch.
        /// </summary>
        ///<param name="successfulDataModelRecords">Indicates the successful DataModel records count</param>
        ///<param name="failedDataModelRecords">Indicates the failed DataModel Record count</param>
        public void UpdateCompletedDataModelBatch(Int64 successfulDataModelRecords, Int64 failedDataModelRecords)
        {
            this.UpdateCompletedDataModelBatch(successfulDataModelRecords, failedDataModelRecords, 0);
        }

        /// <summary>
        /// Updates the DataModel record count for success and failure at the end of a given batch.
        /// </summary>
        ///<param name="successfulDataModelRecords">Indicates the successful DataModel records count</param>
        public void UpdateCompletedDataModelBatch(Int64 successfulDataModelRecords)
        {
            this.UpdateCompletedDataModelBatch(successfulDataModelRecords, 0, 0);
        }

        /// <summary>
        /// Add the DataModel locale into the current DataModel import progress handler.
        /// </summary>
        public void AddLocale(LocaleEnum locale)
        {
            lock (lockObject)
            {
                this._localeList.Add(locale);
            }
        }

        /// <summary>
        /// Add the processed/completed DataModel locale into the current DataModel import progress handler.
        /// </summary>
        public void AddProcessedDataModelLocale(LocaleEnum locale)
        {
            lock (lockObject)
            {
                this._processedLocaleList.Add(locale);
                this._completedDataModelrecords = 0;

                //Update the Status of the job
                this.RefreshJobStatus();
            }
        }

        /// <summary>
        /// Add the error reference code to the current DataModel import progress handler.
        /// </summary>
        /// <param name="referenceCode">Indicates the reference code</param>
        public void AddReferenceCode(String referenceCode)
        {
            lock (lockObject)
            {
                if (!String.IsNullOrWhiteSpace(referenceCode))
                {
                    this._referenceCodes.Add(referenceCode);
                    //Adding new error code means additional error we are adding to the DataModel operation result
                    this.UpdateCompletedDataModelBatch(0, 1, 0);
                }
            }
        }

        /// <summary>
        /// Get all the reference error code available in the DataModel import progress handler.
        /// </summary>
        /// <returns></returns>
        public Collection<String> GetReferenceCodes()
        {
            return this._referenceCodes;
        }

        /// <summary>
        /// Get the Job status for the DataModel import progress handler.
        /// </summary>
        /// <returns></returns>
        public JobStatus GetJobStatus()
        {
            return this._jobStatus;
        }

        private void RefreshJobStatus()
        {
            lock (lockObject)
            {
                if (this._localeList.Count == this._processedLocaleList.Count)
                {
                    if (_failedDataModelRecords > 0)
                    {
                        this._jobStatus = JobStatus.CompletedWithErrors;
                    }
                    else
                    {
                        this._jobStatus = JobStatus.Completed;
                    }
                }
            }
        }

        #endregion
    }
}
