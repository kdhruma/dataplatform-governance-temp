using System;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Interfaces;
    using MDM.Core;

    /// <summary>
    /// Specifies DQMJobQueueItemStatus
    /// </summary>
    [DataContract]
    [KnownType(typeof(DQMJobType))]
    public class DQMJobQueueItemStatus : ObjectBase, IDQMJobQueueItemStatus
    {
        #region Fields

        /// <summary>
        /// Field indicates unique Id representing table item
        /// </summary>
        private Int64 _jobQueueId = -1;
        
        /// <summary>
        /// Field indicates job type
        /// </summary>
        private DQMJobType _jobType = DQMJobType.Unknown;

        /// <summary>
        /// Field indicates whether job is in initialization progress
        /// </summary>
        private Boolean _isInitializationInProgress = false;

        /// <summary>
        /// Field indicates whether job is initialized
        /// </summary>
        private Boolean _isInitialized = false;

        /// <summary>
        /// Field indicates whether job finalization is in progress
        /// </summary>
        private Boolean _isFinalizationInProgress = false;

        /// <summary>
        /// Field indicates whether entity has processed
        /// </summary>
        private Boolean _isProcessed = false;

        /// <summary>
        /// Field indicates whether Job is canceled
        /// </summary>
        private Boolean _isCanceled = false;

        /// <summary>
        /// Field indicates the created date time of job
        /// </summary>
        private DateTime? _createdDateTime = null;

        /// <summary>
        /// Field indicates the time when initialization started
        /// </summary>
        private DateTime? _initializationStartTime = null;

        /// <summary>
        /// Field indicates the time when initialization ended
        /// </summary>
        private DateTime? _initializationEndTime = null;

        /// <summary>
        /// Field indicates the job finalization process start time
        /// </summary>
        private DateTime? _finalizationStartTime = null;

        /// <summary>
        /// Field indicates the processing start time of the job
        /// </summary>
        private DateTime? _processStartTime = null;

        /// <summary>
        /// Field indicates the processing end time of the job
        /// </summary>
        private DateTime? _processEndTime = null;
        
        /// <summary>
        /// Field indicates the number of processed entity tasks related to JobQueue item
        /// </summary>
        private Int64 _processedEntitiesCount = 0;

        /// <summary>
        /// Field indicates the total number of entity tasks related to JobQueue item
        /// </summary>
        private Int64 _totalEntitiesCount = 0;

        /// <summary>
        /// Field indicates the launcher of job 
        /// </summary>
        private String _userName = String.Empty;

        /// <summary>
        /// Field indicates profile Id
        /// </summary>
        private Int32? _profileId = null;

		/// <summary>
		/// Field indicates profile name
		/// </summary>
		private String _profileName = String.Empty;

        /// <summary>
        /// Field indicates LastActionUserName (Audit Ref data)
        /// </summary>
        private String _lastActionUserName = String.Empty;

        /// <summary>
        /// Field indicates LastActionDateTime (Audit Ref data)
        /// </summary>
        private DateTime? _lastActionDateTime = null;

        /// <summary>
        /// Field indicates LastActionProgramName (Audit Ref data)
        /// </summary>
        private String _lastActionProgramName = String.Empty;

        /// <summary>
        /// Field indicates IsSystem job status
        /// </summary>
        private Boolean _isSystem = false;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property indicates unique Id representing table item
        /// </summary>
        [DataMember]
        public Int64 JobQueueId
        {
            get { return this._jobQueueId; }
            set { this._jobQueueId = value; }
        }

        /// <summary>
        /// Property indicates job type
        /// </summary>
        [DataMember]
        public DQMJobType JobType
        {
            get { return _jobType; }
            set { _jobType = value; }
        }

        /// <summary>
        /// Property indicates whether job is in initialization progress
        /// </summary>
        [DataMember]
        public Boolean IsInitializationInProgress
        {
            get { return this._isInitializationInProgress; }
            set { this._isInitializationInProgress = value; }
        }

        /// <summary>
        /// Property indicates whether job is initialized
        /// </summary>
        [DataMember]
        public Boolean IsInitialized
        {
            get { return this._isInitialized; }
            set { this._isInitialized = value; }
        }

        /// <summary>
        /// Property indicates whether job finalization is in progress
        /// </summary>
        [DataMember]
        public Boolean IsFinalizationInProgress
        {
            get { return this._isFinalizationInProgress; }
            set { this._isFinalizationInProgress = value; }
        }

        /// <summary>
        /// Property indicates whether entity has processed
        /// </summary>
        [DataMember]
        public Boolean IsProcessed
        {
            get { return this._isProcessed; }
            set { this._isProcessed = value; }
        }

        /// <summary>
        /// Property indicates whether Job is canceled
        /// </summary>
        [DataMember]
        public Boolean IsCanceled
        {
            get { return this._isCanceled; }
            set { this._isCanceled = value; }
        }

        /// <summary>
        /// Property indicates the created date time of job
        /// </summary>
        [DataMember]
        public DateTime? CreatedDateTime
        {
            get { return this._createdDateTime; }
            set { this._createdDateTime = value;}
        }

        /// <summary>
        /// Property indicates the time when initialization started
        /// </summary>
        [DataMember]
        public DateTime? InitializationStartTime
        {
            get { return this._initializationStartTime; }
            set { this._initializationStartTime = value; }
        }

        /// <summary>
        /// Property indicates the time when initialization ended
        /// </summary>
        [DataMember]
        public DateTime? InitializationEndTime
        {
            get { return this._initializationEndTime; }
            set { this._initializationEndTime = value; }
        }

        /// <summary>
        /// Property indicates the job finalization process start time
        /// </summary>
        [DataMember]
        public DateTime? FinalizationStartTime
        {
            get { return this._finalizationStartTime; }
            set { this._finalizationStartTime = value; }
        }

        /// <summary>
        /// Property indicates the processing start time of the job
        /// </summary>
        [DataMember]
        public DateTime? ProcessStartTime
        {
            get { return this._processStartTime; }
            set { this._processStartTime = value; }
        }

        /// <summary>
        /// Property indicates the processing end time of the job
        /// </summary>
        [DataMember]
        public DateTime? ProcessEndTime
        {
            get { return this._processEndTime; }
            set { this._processEndTime = value; }
        }

        /// <summary>
        /// Property indicates the number of processed entity tasks related to JobQueue item
        /// </summary>
        [DataMember]
        public Int64 ProcessedEntitiesCount
        {
            get { return this._processedEntitiesCount; }
            set { this._processedEntitiesCount = value; }
        }

        /// <summary>
        /// Property indicates the total number of entity tasks related to JobQueue item
        /// </summary>
        [DataMember]
        public Int64 TotalEntitiesCount
        {
            get { return this._totalEntitiesCount; }
            set { this._totalEntitiesCount = value; }
        }

        /// <summary>
        /// Property indicates the JobQueue item state
        /// </summary>
        [IgnoreDataMember]
        public JobQueueItemState State
        {
            get
            {
                if (_isCanceled)
                {
                    return JobQueueItemState.Canceled;
                }
                if (_isProcessed)
                {
                    return JobQueueItemState.Processed;
                }
                if (_isFinalizationInProgress)
                {
                    return JobQueueItemState.Finalizing;
                }
                if (_isInitialized && !_isFinalizationInProgress && !_isProcessed && _processedEntitiesCount > 0)
                {
                    return JobQueueItemState.Processing;
                }
                if (_isInitialized)
                {
                    return JobQueueItemState.Initialized;
                }
                if (_isInitializationInProgress)
                {
                    return JobQueueItemState.Initializing;
                }
                return JobQueueItemState.Initial;
            }
        }

        /// <summary>
        /// Property indicates the launcher of job 
        /// </summary>
        [DataMember]
        public String UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        /// <summary>
        /// Property indicates profile Id
        /// </summary>
        [DataMember]
        public Int32? ProfileId
        {
            get { return this._profileId; }
            set { this._profileId = value; }
        }

		/// <summary>
		/// Property indicates profile name
		/// </summary>
		[DataMember]
		public String ProfileName
		{
			get { return _profileName; }
			set { _profileName = value; }
		}

        /// <summary>
        /// Property indicates LastActionUserName (Audit Ref data)
        /// </summary>
        [DataMember]
        public String LastActionUserName
        {
            get { return _lastActionUserName; }
            set { _lastActionUserName = value; }
        }

        /// <summary>
        /// Property indicates LastActionDateTime (Audit Ref data)
        /// </summary>
        [DataMember]
        public DateTime? LastActionDateTime
        {
            get { return this._lastActionDateTime; }
            set { this._lastActionDateTime = value; }
        }

        /// <summary>
        /// Property indicates LastActionProgramName (Audit Ref data)
        /// </summary>
        [DataMember]
        public String LastActionProgramName
        {
            get { return _lastActionProgramName; }
            set { _lastActionProgramName = value; }
        }

        /// <summary>
        /// Property indicates IsSystem job status
        /// </summary>
        [DataMember]
        public Boolean IsSystem
        {
            get { return this._isSystem; }
            set { this._isSystem = value; }
        }

        #endregion Properties
    }
}