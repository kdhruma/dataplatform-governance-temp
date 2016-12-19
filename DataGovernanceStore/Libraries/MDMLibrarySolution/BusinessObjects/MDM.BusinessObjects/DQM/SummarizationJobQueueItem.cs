using System;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies summarization job queue item
    /// </summary>
    [DataContract]
    [KnownType(typeof(DQMJobType))]
    public class SummarizationJobQueueItem : MDMObject, ISummarizationJobQueueItem
    {
        #region Fields

        /// <summary>
        /// Indicates the identifier of summarization job queue item
        /// </summary>
        private Int64 _id = -1;

        /// <summary>
        /// Indicates the container identifier
        /// </summary>
        private Int32? _containerId = null;

        /// <summary>
        /// Indicates the DQM Job type
        /// </summary>
        private DQMJobType _jobType = DQMJobType.Unknown;

        /// <summary>
        /// Indicates if the summarization job is in progress
        /// </summary>
        private Boolean _isInProgress;

        /// <summary>
        /// Indicates the weightage
        /// </summary>
        private Int32? _weightage = null;

        /// <summary>
        /// Indicates the parent job identifier
        /// </summary>
        private Int64 _parentJobId;

        /// <summary>
        /// Indicates the server identifier
        /// </summary>
        private Int32? _serverId = null;

        /// <summary>
        /// Indicates the server name
        /// </summary>
        private String _serverName = String.Empty;

        /// <summary>
        /// Indicates the last modified date time
        /// </summary>
        private DateTime? _lastModifiedDateTime;

        /// <summary>
        /// Indicates the entity type id
        /// </summary>
        private Int32? _entityTypeId = null;

        /// <summary>
        /// Indicates the category id
        /// </summary>
        private Int64? _categoryId = null;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property denoting identifier of job queue item
        /// </summary>
        [DataMember]
        public new Int64 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Property denoting container identifier
        /// </summary>
        [DataMember]
        public Int32? ContainerId
        {
            get { return _containerId; }
            set { _containerId = value; }
        }

        /// <summary>
        /// Property denoting entity type identifier
        /// </summary>
        [DataMember]
        public Int32? EntityTypeId
        {
            get { return _entityTypeId; }
            set { _entityTypeId = value; }
        }

        /// <summary>
        /// Property denoting category identifier
        /// </summary>
        [DataMember]
        public Int64? CategoryId
        {
            get { return _categoryId; }
            set { _categoryId = value; }
        }

        /// <summary>
        /// Property denoting job type
        /// </summary>
        [DataMember]
        public DQMJobType JobType
        {
            get { return _jobType; }
            set { _jobType = value; }
        }

        /// <summary>
        /// Property denoting if summarization job queue is in progress
        /// </summary>
        [DataMember]
        public Boolean IsInProgress
        {
            get { return _isInProgress; }
            set { _isInProgress = value; }
        }

        /// <summary>
        /// Property denoting weightage
        /// </summary>
        [DataMember]
        public Int32? Weightage
        {
            get { return _weightage; }
            set { _weightage = value; }
        }

        /// <summary>
        /// Property denoting parent job identifier
        /// </summary>
        [DataMember]
        public Int64 ParentJobId
        {
            get { return _parentJobId; }
            set { _parentJobId = value; }
        }

        /// <summary>
        /// Property denoting server identifier
        /// </summary>
        [DataMember]
        public Int32? ServerId
        {
            get { return _serverId; }
            set { _serverId = value; }
        }

        /// <summary>
        /// Property denoting server name
        /// </summary>
        [DataMember]
        public String ServerName
        {
            get { return _serverName; }
            set { _serverName = value; }
        }

        /// <summary>
        /// Property denoting last modified date time
        /// </summary>
        [DataMember]
        public DateTime? LastModifiedDateTime
        {
            get { return _lastModifiedDateTime; }
            set { _lastModifiedDateTime = value; }
        }

        #endregion
    }
}