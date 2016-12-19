using System;
using System.Collections.ObjectModel;

namespace MDM.Imports.Processor
{
    using MDM.Core;
    using MDM.Imports.Interfaces;

    /// <summary>
    /// Specifies the Lookup import progress handler class.
    /// This track the lookup import progress
    /// </summary>
    public class LookupImportProgressHandler : ILookupImportProgressHandler
    {
        #region Fields

        internal static Object lockObject = new Object();
        private Int64 _totalLookupRecords = 0;
        private Int64 _completedLookuprecords = 0;
        private Int64 _successLookupRecords = 0;
        private Int64 _failedLookupRecords = 0;
        private Int64 _warningLookupRecords = 0;
        private JobStatus _jobStatus = JobStatus.UnKnown;
        private Collection<LocaleEnum> _localeList = new Collection<LocaleEnum>();
        private Collection<LocaleEnum> _processedLocaleList = new Collection<LocaleEnum>();
        private Collection<String> _referenceCodes = new Collection<String>();

        #endregion

        #region Methods

        /// <summary>
        /// Get the total number of Lookup records processed so far.
        /// </summary>
        /// <returns>Returns the total number of lookup record count</returns>
        public Int64 GetTotalLookupRecords()
        {
            return _totalLookupRecords;
        }

        /// <summary>
        /// Set the total number of Lookup records yet to process
        /// </summary>
        public void SetTotalLookupRecords(Int64 count)
        {
            lock (lockObject)
            {
                _totalLookupRecords = count;
                _jobStatus = JobStatus.Running;
            }
        }

        /// <summary>
        /// Gets the completed lookup records at a given point of time
        /// </summary>
        /// <returns>Returns the completed lookup record count</returns>
        public Int64 GetCompletedLookupRecords()
        {
            return _completedLookuprecords;
        }

        /// <summary>
        /// Gets the number of successfully completed lookup records.
        /// </summary>
        /// <returns>Return the successful Lookup record count </returns>
        public Int64 GetSuccessfulLookupRecords()
        {
            Int64 result = 0;
            if (_successLookupRecords > 0)
            {
                result = _successLookupRecords / this._localeList.Count;
            }

            return result;
        }

        /// <summary>
        /// Clear the count of all entities
        /// </summary>
        public void ResetLookupRecordCounts()
        {
            lock (lockObject)
            {
                _totalLookupRecords = 0;
                _completedLookuprecords = 0;
                _successLookupRecords = 0;
                _failedLookupRecords = 0;
                _warningLookupRecords = 0;
            }
        }

        /// <summary>
        /// Gets the number of failed lookup records
        /// </summary>
        /// <returns>Returns the failed lookup record count</returns>
        public Int64 GetFailedLookupRecords()
        {
            Int64 result = 0;
            if (_failedLookupRecords > 0)
            {
                result = _failedLookupRecords / this._localeList.Count;
            }

            return result;
        }

        /// <summary>
        /// Updates the lookup record count for success and failure at the end of a given batch.
        /// </summary>
        ///<param name="successfulLookupRecords">Indicates the successful lookup records count</param>
        ///<param name="failedLookupRecords">Indicates the failed lookup Record count</param>
        ///<param name="lastBatchCompletedRecords">Indicates the last batch completed record count</param>
        public void UpdateCompletedLookupBatch(Int64 successfulLookupRecords, Int64 failedLookupRecords, Int64 lastBatchCompletedRecords)
        {
            lock (lockObject)
            {
                _successLookupRecords += successfulLookupRecords;
                _failedLookupRecords += failedLookupRecords;
                _completedLookuprecords += (successfulLookupRecords + failedLookupRecords + lastBatchCompletedRecords);
            }
        }

        /// <summary>
        /// Updates the lookup record count for success and failure at the end of a given batch.
        /// </summary>
        ///<param name="successfulLookupRecords">Indicates the successful lookup records count</param>
        ///<param name="failedLookupRecords">Indicates the failed lookup Record count</param>
        public void UpdateCompletedLookupBatch(Int64 successfulLookupRecords, Int64 failedLookupRecords)
        {
            this.UpdateCompletedLookupBatch(successfulLookupRecords, failedLookupRecords, 0);
        }

        /// <summary>
        /// Updates the lookup record count for success and failure at the end of a given batch.
        /// </summary>
        ///<param name="successfulLookupRecords">Indicates the successful lookup records count</param>
        public void UpdateCompletedLookupBatch(Int64 successfulLookupRecords)
        {
            this.UpdateCompletedLookupBatch(successfulLookupRecords, 0, 0);
        }

        /// <summary>
        /// Updates the lookup record warning count at the end of a given batch.
        /// </summary>
        ///<param name="warningLookupRecords">Indicates the warning lookup records count</param>
        public void UpdateWarningLookupBatch(Int64 warningLookupRecords)
        {
            lock (lockObject)
            {
                _warningLookupRecords += warningLookupRecords;
            }
        }

        /// <summary>
        /// Add the lookup locale into the current lookup import progress handler.
        /// </summary>
        public void AddLocale(LocaleEnum locale)
        {
            lock (lockObject)
            {
                this._localeList.Add(locale);
            }
        }

        /// <summary>
        /// Add the processed/completed lookup locale into the current lookup import progress handler.
        /// </summary>
        public void AddProcessedLookupLocale(LocaleEnum locale)
        {
            lock (lockObject)
            {
                this._processedLocaleList.Add(locale);
                this._completedLookuprecords = 0;

                //Update the Status of the job
                this.RefreshJobStatus();
            }
        }

        /// <summary>
        /// Add the error reference code to the current lookup import progress handler.
        /// </summary>
        /// <param name="referenceCode">Indicates the reference code</param>
        public void AddReferenceCode(String referenceCode)
        {
            lock (lockObject)
            {
                if (!String.IsNullOrWhiteSpace(referenceCode))
                {
                    this._referenceCodes.Add(referenceCode);
                    //Adding new error code means additional error we are adding to the lookup operation result
                    this.UpdateCompletedLookupBatch(0, 1, 0);
                }
            }
        }

        /// <summary>
        /// Get all the reference error code available in the lookup import progress handler.
        /// </summary>
        /// <returns></returns>
        public Collection<String> GetReferenceCodes()
        {
            return this._referenceCodes;
        }

        /// <summary>
        /// Get the Job status for the lookup import progress handler.
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
                    if (_failedLookupRecords > 0)
                    {
                        this._jobStatus = JobStatus.CompletedWithErrors;
                    }
                    else if (_warningLookupRecords > 0)
                    {
                        this._jobStatus = JobStatus.CompletedWithWarnings;
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
