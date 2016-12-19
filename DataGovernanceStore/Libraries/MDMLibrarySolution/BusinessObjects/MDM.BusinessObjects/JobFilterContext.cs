using System;
using System.Runtime.Serialization;
using MDM.BusinessObjects.Interfaces;
using MDM.Core;

namespace MDM.BusinessObjects
{
    /// <summary>
    /// Defines Filter Context for Get Job from database
    /// </summary>
    [DataContract]
    public class JobFilterContext : ObjectBase, IJobFilterContext
    {
        #region Fields

        private Int32 _key;
        private Int32 _jobId;
        private JobType _jobType;
        private JobSubType _jobSubType;
        private JobStatus _jobStatus;
        private Boolean _skipJobDataLoading;
        private DateTime? _dateTimeFrom;
        private DateTime? _dateTimeTo;
        private LocaleEnum _locale = LocaleEnum.UnKnown;

        /// <summary>
        /// Field denoting needs to get only specific user jobs or not
        /// </summary>
        private Boolean _getOnlyUserJobs = false;

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Int32 JobId
        {
            get { return _jobId; }
            set { _jobId = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public JobType JobType
        {
            get { return _jobType; }
            set { _jobType = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public JobSubType JobSubType
        {
            get { return _jobSubType; }
            set { _jobSubType = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public JobStatus JobStatus
        {
            get { return _jobStatus; }
            set { _jobStatus = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Boolean SkipJobDataLoading
        {
            get { return _skipJobDataLoading; }
            set { _skipJobDataLoading = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public DateTime? DateTimeFrom
        {
            get { return _dateTimeFrom; }
            set { _dateTimeFrom = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public DateTime? DateTimeTo
        {
            get { return _dateTimeTo; }
            set { _dateTimeTo = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Int32 Key
        {
            private set
            {
                _key = value;
            }
            get
            {
                _key = GetKey(JobType, JobStatus);
                return _key;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Boolean GetAll
        {
            get
            {
                return Key == 0 && JobId == 0;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public LocaleEnum Locale
        {
            get { return _locale; }
            set { _locale = value; }
        }

        /// <summary>
        /// Indicates needs to get only specific user jobs or not
        /// </summary>
        [DataMember]
        public Boolean GetOnlyUserJobs
        {
            get { return _getOnlyUserJobs; }
            set { _getOnlyUserJobs = value; }
        }

        #endregion

        #region Constructors

         /// <summary>
         /// 
         /// </summary>
         /// <param name="id"></param>
         /// <param name="jobType"></param>
         /// <param name="jobSubType"></param>
         /// <param name="jobStatus"></param>
         /// <param name="dateFrom"></param>
         /// <param name="dateTo"></param>
         /// <param name="skipJobDataLoading"></param>
        public JobFilterContext(Int32 id, JobType jobType, JobSubType jobSubType, JobStatus jobStatus, DateTime? dateFrom,
            DateTime? dateTo, Boolean skipJobDataLoading)
        {
            JobId = id;
            JobType = jobType;
            JobSubType = jobSubType;
            JobStatus = jobStatus;
            DateTimeFrom = dateFrom;
            DateTimeTo = dateTo;
            SkipJobDataLoading = skipJobDataLoading;
            Key = GetKey(JobType, JobStatus);
        }

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="id">Indicates identifier of job</param>
        /// <param name="jobType">Indicates type of job</param>
        /// <param name="jobSubType">Indicates sub type of job</param>
        /// <param name="jobStatus">Indicates status of job</param>
        /// <param name="dateFrom">Indicates date from where needs to get data</param>
        /// <param name="dateTo">Indicates date till which needs to get data</param>
        /// <param name="skipJobDataLoading">Indicates needs to skip job data loading or not</param>
        /// <param name="getOnlyUserJobs">Indicates needs to get only user jobs or not</param>
        public JobFilterContext(Int32 id, JobType jobType, JobSubType jobSubType, JobStatus jobStatus, DateTime? dateFrom,
            DateTime? dateTo, Boolean skipJobDataLoading, Boolean getOnlyUserJobs)
        {
            JobId = id;
            JobType = jobType;
            JobSubType = jobSubType;
            JobStatus = jobStatus;
            DateTimeFrom = dateFrom;
            DateTimeTo = dateTo;
            SkipJobDataLoading = skipJobDataLoading;
            Key = GetKey(JobType, JobStatus);
            GetOnlyUserJobs = getOnlyUserJobs;
        }

        #endregion

        #region Methods

        #region Private Methods

        private Int32 GetKey(JobType jobType, JobStatus jobStatus)
        {
            if (jobType == JobType.UnKnown)
                return jobStatus == JobStatus.UnKnown ? 0 : 2;
            return jobStatus == JobStatus.UnKnown ? 1 : 3;
        }

        #endregion

        #endregion
    }
}
