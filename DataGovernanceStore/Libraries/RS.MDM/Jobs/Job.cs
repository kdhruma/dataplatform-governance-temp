using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace RS.MDM.Jobs
{
    /// <summary>
    /// Provides statuses of the jobs
    /// </summary>
    public enum JobStatus
    {
        /// <summary>
        /// Job is queued but not Running
        /// </summary>
        Queued,

        /// <summary>
        /// Job is stopped
        /// </summary>
        Stopping,

        /// <summary>
        /// Job is paused.
        /// </summary>
        Paused,

        /// <summary>
        /// Job is running
        /// </summary>
        Running,

        /// <summary>
        /// Job has completed
        /// </summary>
        Completed,

        /// <summary>
        /// Job has aborted with exceptions
        /// </summary>
        Aborted,

        /// <summary>
        /// Job is deleted
        /// </summary>
        Deleted,

        /// <summary>
        /// Job is waiting to be queued
        /// </summary>
        Waiting
    }

    /// <summary>
    /// Provides the format types for the Data
    /// </summary>
    public enum JobDataFormat
    {
        /// <summary>
        /// Custom Xml format mutually agreed upon
        /// </summary>
        CustomXML,

        /// <summary>
        /// xCBL version 3.5 format 
        /// </summary>
        xCBL3_5,

        /// <summary>
        /// xCBL version 4.0 format
        /// </summary>
        xCBL4_0,

        /// <summary>
        /// Excel format
        /// </summary>
        Excel,

        /// <summary>
        /// Riversand defined XML format
        /// </summary>
        RSXML,

        /// <summary>
        /// CSV format
        /// </summary>
        CSV,

        /// <summary>
        /// Tab separated values format
        /// </summary>
        Tab,

        /// <summary>
        /// Custom format
        /// </summary>
        Custom
    }

    /// <summary>
    /// Provides the content types of data 
    /// </summary>
    public enum JobDataContentType
    {
        /// <summary>
        /// Data in its original form
        /// </summary>
        Data,

        /// <summary>
        /// Data contains the name and location of the file
        /// </summary>
        FileName,

        /// <summary>
        /// Data is encapsulated in Multi-part mime message
        /// </summary>
        Mime
    }

    /// <summary>
    /// Type of Jobs
    /// </summary>
    public enum JobType
    {
        /// <summary>
        /// Exports data to external applications
        /// </summary>
        Export,

        /// <summary>
        /// Imports data into Riversand's Environment
        /// </summary>
        Import,

        /// <summary>
        /// Exports data to external applications
        /// </summary>
        NumberMatching,

        /// <summary>
        /// Exports data to external applications
        /// </summary>
        FuzzyMatching,
        
        /// <summary>
        /// Exports data to external applications
        /// </summary>
        AttributeExtraction,

        /// <summary>
        /// Exports data to external applications
        /// </summary>
        Normalization
    }

    /// <summary>
    /// Provides functionality to create jobs 
    /// </summary>
    [DataContract(Namespace = "http://Riversand.MDM.Services")]
    public class Job : RS.MDM.Object
    {
        #region  Fields

        /// <summary>
        /// field for the data
        /// </summary>
        private string _data = string.Empty;

        /// <summary>
        /// field for the profile name
        /// </summary>
        private string _profileName = string.Empty;

        /// <summary>
        /// field for the rule name
        /// </summary>
        private string _ruleName = string.Empty;

        /// <summary>
        /// field for the status of the job
        /// </summary>
        private JobStatus _jobStatus = JobStatus.Waiting;

        /// <summary>
        /// field for the dataformat
        /// </summary>
        private JobDataFormat _dataFormat = JobDataFormat.RSXML;

        /// <summary>
        /// field for the content type of the data
        /// </summary>
        private JobDataContentType _dataContentType = JobDataContentType.Data;

        /// <summary>
        /// field for the Type of the Job
        /// </summary>
        private JobType _jobTypeImport = JobType.Import;

        /// <summary>
        /// field for the Type of the Job
        /// </summary>
        private JobType _jobTypeExport = JobType.Export;

        /// <summary>
        /// field for the Type of the Job
        /// </summary>
        private JobType _jobTypeAttributeExtraction = JobType.AttributeExtraction;

        /// <summary>
        /// field for the Type of the Job
        /// </summary>
        private JobType _jobTypeFuzzyMatching = JobType.FuzzyMatching;

        /// <summary>
        /// field for the Type of the Job
        /// </summary>
        private JobType _jobTypeNormalization = JobType.Normalization;

        /// <summary>
        /// field for the Type of the Job
        /// </summary>
        private JobType _jobTypeNumberMatching = JobType.NumberMatching;

        /// <summary>
        /// field to denote if data is encrypted
        /// </summary>
        private bool _isDataEncrypted = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public Job()
            : base()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the data for the job
        /// </summary>
        [DataMember]
        public string Data
        {
            get { return _data; }
            set { _data = value; }
        }

        /// <summary>
        /// Gets or sets the format of the Data
        /// </summary>
        [DataMember]
        public JobDataFormat DataFormat
        {
            get { return _dataFormat; }
            set { _dataFormat = value; }
        }

        /// <summary>
        /// Indicates the type of the data
        /// </summary>
        public JobDataContentType DataContentType
        {
            get { return this._dataContentType; }
            set { _dataContentType = value; }
        }

        /// <summary>
        /// Indicates if the data is encrypted
        /// </summary>
        public bool IsDataEncrypted
        {
            get { return this._isDataEncrypted; }
            set { this._isDataEncrypted = value; }
        }

        /// <summary>
        /// Gets or sets the name of the Rule
        /// </summary>
        [DataMember]
        public string RuleName
        {
            get { return _ruleName; }
            set { _ruleName = value; }
        }

        /// <summary>
        /// Gets or sets the status of the job
        /// </summary>
        [DataMember]
        public JobStatus JobStatus
        {
            get { return _jobStatus; }
            set { _jobStatus = value; }
        }

        /// <summary>
        /// Gets or sets the type of the Job
        /// </summary>
        [DataMember]
        public JobType JobTypeImport
        {
            get { return _jobTypeImport; }
            set { _jobTypeImport = value; }
        }

        /// <summary>
        /// Gets or sets the type of the Job
        /// </summary>
        [DataMember]
        public JobType JobTypeExport
        {
            get { return _jobTypeExport; }
            set { _jobTypeExport= value; }
        }

        /// <summary>
        /// Gets or sets the type of the Job
        /// </summary>
        [DataMember]
        public JobType JobTypeAttributeExtraction
        {
            get { return _jobTypeAttributeExtraction; }
            set { _jobTypeAttributeExtraction = value; }
        }

       
        /// <summary>
        /// Gets or sets the type of the Job
        /// </summary>
        [DataMember]
        public JobType JobTypeFuzzyMatching
        {
            get { return _jobTypeFuzzyMatching; }
            set { _jobTypeFuzzyMatching = value; }
        }

        /// <summary>
        /// Gets or sets the type of the Job
        /// </summary>
        [DataMember]
        public JobType JobTypeNormalization
        {
            get { return _jobTypeNormalization; }
            set { _jobTypeNormalization = value; }
        }


        /// <summary>
        /// Gets or sets the type of the Job
        /// </summary>
        [DataMember]
        public JobType JobTypeNumberMatching
        {
            get { return _jobTypeNumberMatching; }
            set { _jobTypeNumberMatching = value; }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Aborts a job indicated by a job id
        /// </summary>
        /// <param name="jobId">Indicates the Id of a job that needs to be Aborted</param>
        public static void Abort(int jobId)
        {
        }

        /// <summary>
        /// Deletes a job indicated by a job id
        /// </summary>
        /// <param name="jobId">Indicates the Id of a job that needs to be Deleted</param>
        public static void Delete(int jobId)
        {
        }

        /// <summary>
        /// Gets the definition of a job indicated by a job id
        /// </summary>
        /// <param name="jobId">Indicates the Id of a job for which the definition needs to be fetched</param>
        /// <returns>A job definition indicated by a job id</returns>
        public static Job Get(int jobId)
        {
            return null;
        }

        /// <summary>
        /// Gets all the jobs of a particular job type
        /// </summary>
        /// <param name="jobType">The type of job</param>
        /// <returns>An array of jobs</returns>
        public static Job[] GetAll(JobType jobType)
        {
            return null;
        }

        /// <summary>
        /// Retrys to run an Aborted Job
        /// </summary>
        /// <param name="jobId">Indicates an Id of the Job</param>
        public static void Retry(int jobId)
        {
        }

        /// <summary>
        /// Starts a new job
        /// </summary>
        /// <param name="newJob">The job that needs to be started</param>
        /// <returns>The Job Id</returns>
        public static int Start(Job newJob)
        {
            return -1;
        }

        #endregion
    }
}