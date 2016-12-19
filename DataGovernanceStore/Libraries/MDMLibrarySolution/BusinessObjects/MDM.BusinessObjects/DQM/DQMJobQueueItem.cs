using System;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies DQMJobQueueItem
    /// </summary>
    [DataContract]
    [KnownType(typeof(DQMJobType))]
    public class DQMJobQueueItem : MDMObject, IDQMJobQueueItem
    {
        #region Fields

        /// <summary>
        /// Field indicates unique Id representing table item
        /// </summary>
        private Int64 _id = -1;

        /// <summary>
        /// Field indicates unique Id of the parent Job
        /// </summary>
        private Int64? _parentJobId = null;

        /// <summary>
        /// Field indicates Job type
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
        /// Field indicates whether Job has processed
        /// </summary>
        private Boolean _isProcessed = false;

        /// <summary>
        /// Field indicates whether Job is canceled
        /// </summary>
        private Boolean _isCanceled = false;

        /// <summary>
        /// Field indicates the Created Date time of Job
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
        /// Field indicates the processing start time of the Job
        /// </summary>
        private DateTime? _processStartTime = null;

        /// <summary>
        /// Field indicates the processing end time of the Job
        /// </summary>
        private DateTime? _processEndTime = null;

        /// <summary>
        /// Field indicates number of impacted entities effected by the Job
        /// </summary>
        private Int64? _impactedCount = null;

        /// <summary>
        /// Field indicates server Id who has created this Job
        /// </summary>
        private Int32? _serverId = null;

        /// <summary>
        /// Field indicates server name who has created this Job
        /// </summary>
        private String _serverName = null;
        
        /// <summary>
        /// Job Context
        /// </summary>
        private String _context = null;

        /// <summary>
        /// Field indicates profile Id
        /// </summary>
        private Int32? _profileId = null;

        /// <summary>
        /// Field indicates job profile for current JobQueueItem
        /// </summary>
        private DQMJobProfile _jobProfile = null;

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
        public new Int64 Id
        {
            get
            {
                return this._id;
            }

            set
            {
                this._id = value;
            }
        }

        /// <summary>
        /// Unique Id of the parent Job
        /// </summary>
        [DataMember]
        public Int64? ParentJobId
        {
            get
            {
                return this._parentJobId;
            }

            set
            {
                this._parentJobId = value;
            }
        }

        /// <summary>
        /// Property indicates Job type
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
            get
            {
                return this._isInitializationInProgress;
            }
            set
            {
                this._isInitializationInProgress = value;

            }
        }

        /// <summary>
        /// Property indicates whether job is initialized
        /// </summary>
        [DataMember]
        public Boolean IsInitialized
        {
            get
            {
                return this._isInitialized;
            }
            set
            {
                this._isInitialized = value;

            }
        }

        /// <summary>
        /// Property indicates whether job finalization is in progress
        /// </summary>
        [DataMember]
        public Boolean IsFinalizationInProgress
        {
            get
            {
                return this._isFinalizationInProgress;
            }
            set
            {
                this._isFinalizationInProgress = value;
            }
        }

        /// <summary>
        /// Property indicates whether Job has processed
        /// </summary>
        [DataMember]
        public Boolean IsProcessed
        {
            get
            {
                return this._isProcessed;
            }
            set
            {
                this._isProcessed = value;
            }
        }

        /// <summary>
        /// Property indicates whether Job is canceled
        /// </summary>
        [DataMember]
        public Boolean IsCanceled
        {
            get
            {
                return this._isCanceled;
            }
            set
            {
                this._isCanceled = value;
            }
        }

        /// <summary>
        /// Property indicates the Created Date time of Job
        /// </summary>
        [DataMember]
        public DateTime? CreatedDateTime
        {
            get
            {
                return this._createdDateTime;
            }
            set
            {
                this._createdDateTime = value;
            }
        }

        /// <summary>
        /// Property indicates the time when initialization started
        /// </summary>
        [DataMember]
        public DateTime? InitializationStartTime
        {
            get
            {
                return this._initializationStartTime;
            }
            set
            {
                this._initializationStartTime = value;
            }
        }

        /// <summary>
        /// Property indicates the time when initialization ended
        /// </summary>
        [DataMember]
        public DateTime? InitializationEndTime
        {
            get
            {
                return this._initializationEndTime;
            }
            set
            {
                this._initializationEndTime = value;
            }
        }

        /// <summary>
        /// Property indicates the job finalization process start time
        /// </summary>
        [DataMember]
        public DateTime? FinalizationStartTime
        {
            get
            {
                return this._finalizationStartTime;
            }
            set
            {
                this._finalizationStartTime = value;
            }
        }

        /// <summary>
        /// Property indicates the processing start time of the Job
        /// </summary>
        [DataMember]
        public DateTime? ProcessStartTime
        {
            get
            {
                return this._processStartTime;
            }
            set
            {
                this._processStartTime = value;
            }
        }

        /// <summary>
        /// Property indicates the processing end time of the Job
        /// </summary>
        [DataMember]
        public DateTime? ProcessEndTime
        {
            get
            {
                return this._processEndTime;
            }
            set
            {
                this._processEndTime = value;
            }
        }

        /// <summary>
        /// Property indicates number of impacted entities effected by the Job
        /// </summary>
        [DataMember]
        public Int64? ImpactedCount
        {
            get
            {
                return this._impactedCount;
            }
            set
            {
                this._impactedCount = value;
            }
        }

        /// <summary>
        /// Property indicates server Id who has created this Job
        /// </summary>
        [DataMember]
        public Int32? ServerId
        {
            get
            {
                return this._serverId;
            }
            set
            {
                this._serverId = value;
            }
        }

        /// <summary>
        /// Field indicates server name who has created this Job
        /// </summary>
        [DataMember]
        public String ServerName
        {
            get
            {
                return this._serverName;
            }
            set
            {
                this._serverName = value;
            }
        }

        /// <summary>
        /// Job Context
        /// </summary>
        [DataMember]
        public String Context
        {
            get
            {
                return this._context;
            }
            set
            {
                this._context = value;
                this._jobProfile = null;
            }
        }

        /// <summary>
        /// Property indicates profile Id
        /// </summary>
        [DataMember]
        public Int32? ProfileId
        {
            get
            {
                return this._profileId;
            }
            set
            {
                this._profileId = value;
            }
        }

        /// <summary>
        /// Property indicates job profile for current JobQueueItem
        /// </summary>
        [IgnoreDataMember]
        public DQMJobProfile JobProfile
        {
            get
            {
                if (_jobProfile == null && JobType != DQMJobType.Unknown)
                {
                    #region Parse JobProfile from Context

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(this.Context);
                    _jobProfile = new DQMJobProfile(JobType);
                    XmlNode node = doc.SelectSingleNode(_jobProfile.JobProfileType) ??
                        doc.SelectSingleNode(@"/Context/" + _jobProfile.JobProfileType);

                    if (node != null)
                    {
                        _jobProfile.LoadPropertiesOnlyFromXml(node);

                        if (node.Attributes != null && node.Attributes.Count > 0)
                        {
                            if (node.Attributes["Id"] != null)
                            {
                                _jobProfile.Id = ValueTypeHelper.Int32TryParse(node.Attributes["Id"].Value, -1);
                            }
                            if (node.Attributes["ProfileShortName"] != null)
                            {
                                _jobProfile.Name = node.Attributes["ProfileShortName"].Value;
                            }
                            if (node.Attributes["ProfileLongName"] != null)
                            {
                                _jobProfile.LongName = node.Attributes["ProfileLongName"].Value;
                            }
                        }
                    }

                    #endregion
                }
                return _jobProfile;
            }
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
                if (_isInitialized && !_isFinalizationInProgress && !_isProcessed && ImpactedCount > 0)
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
        /// Property indicates IsSystem job status
        /// </summary>
        [DataMember]
        public Boolean IsSystem
        {
            get
            {
                return this._isSystem;
            }
            set
            {
                this._isSystem = value;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DQMJobQueueItem()
            : base()
        {
        }

        /// <summary>
        /// Constructor with XML
        /// </summary>
        /// <param name="valuesAsxml">XML having xml value</param>
        public DQMJobQueueItem(String valuesAsxml)
        {
            LoadDQMJobQueueItem(valuesAsxml);
        }

        #endregion Constructors

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadDQMJobQueueItem(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "DQMJobQueueItem")
                        {
                            #region Read Attribute Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), -1);
                                }
                                if (reader.MoveToAttribute("ParentJobId"))
                                {
                                    this.ParentJobId = ValueTypeHelper.ConvertToNullableInt64(reader.ReadContentAsString());
                                }
                                if (reader.MoveToAttribute("JobType"))
                                {
                                    DQMJobType action = DQMJobType.Unknown;
                                    Enum.TryParse(reader.ReadContentAsString(), true, out action);
                                    this.JobType = action;
                                }
                                if (reader.MoveToAttribute("IsInitializationInProgress"))
                                {
                                    this.IsInitializationInProgress = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }
                                if (reader.MoveToAttribute("IsInitialized"))
                                {
                                    this.IsInitialized = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }
                                if (reader.MoveToAttribute("IsFinalizationInProgress"))
                                {
                                    this.IsFinalizationInProgress = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }
                                if (reader.MoveToAttribute("IsProcessed"))
                                {
                                    this.IsProcessed = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }
                                if (reader.MoveToAttribute("IsCanceled"))
                                {
                                    this.IsCanceled = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }
                                if (reader.MoveToAttribute("CreatedDateTime"))
                                {
                                    this.CreatedDateTime = ValueTypeHelper.ConvertToNullableDateTime(reader.ReadContentAsString());
                                }
                                if (reader.MoveToAttribute("InitializationStartTime"))
                                {
                                    this.InitializationStartTime = ValueTypeHelper.ConvertToNullableDateTime(reader.ReadContentAsString());
                                }
                                if (reader.MoveToAttribute("InitializationEndTime"))
                                {
                                    this.InitializationEndTime = ValueTypeHelper.ConvertToNullableDateTime(reader.ReadContentAsString());
                                }
                                if (reader.MoveToAttribute("FinalizationStartTime"))
                                {
                                    this.FinalizationStartTime = ValueTypeHelper.ConvertToNullableDateTime(reader.ReadContentAsString());
                                }
                                if (reader.MoveToAttribute("ProcessStartTime"))
                                {
                                    this.ProcessStartTime = ValueTypeHelper.ConvertToNullableDateTime(reader.ReadContentAsString());
                                }
                                if (reader.MoveToAttribute("ProcessEndTime"))
                                {
                                    this.ProcessEndTime = ValueTypeHelper.ConvertToNullableDateTime(reader.ReadContentAsString());
                                }
                                if (reader.MoveToAttribute("ImpactedCount"))
                                {
                                    this.ImpactedCount = ValueTypeHelper.ConvertToNullableInt64(reader.ReadContentAsString());
                                }
                                if (reader.MoveToAttribute("ServerId"))
                                {
                                    this.ServerId = ValueTypeHelper.ConvertToNullableInt32(reader.ReadContentAsString());
                                }
                                if (reader.MoveToAttribute("ServerName"))
                                {
                                    this.ServerName = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("Context"))
                                {
                                    this.Context = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("ProgramName"))
                                {
                                    this.ProgramName = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("UserName"))
                                {
                                    this.UserName = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("ProfileId"))
                                {
                                    this.ProfileId = ValueTypeHelper.ConvertToNullableInt32(reader.ReadContentAsString());
                                }
                                if (reader.MoveToAttribute("IsSystem"))
                                {
                                    this.IsSystem = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }
                            }

                            #endregion
                        }
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }
        }

        #endregion Private Methods

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of DQMJobQueueItem
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public override String ToXml()
        {
            String resultXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //DQMJobQueueItem node start
            xmlWriter.WriteStartElement("DQMJobQueueItem");

            #region Write Properties

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("ParentJobId", this.ParentJobId.ToString());
            xmlWriter.WriteAttributeString("JobType", this.JobType.ToString());
            xmlWriter.WriteAttributeString("IsInitializationInProgress", this.IsInitializationInProgress.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("IsInitialized", this.IsInitialized.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("IsFinalizationInProgress", this.IsFinalizationInProgress.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("IsProcessed", this.IsProcessed.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("IsCanceled", this.IsCanceled.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("CreateDateTime", this.CreatedDateTime.ToString());
            xmlWriter.WriteAttributeString("InitializationStartTime", this.InitializationStartTime.ToString());
            xmlWriter.WriteAttributeString("InitializationEndTime", this.InitializationEndTime.ToString());
            xmlWriter.WriteAttributeString("FinalizationStartTime", this.FinalizationStartTime.ToString());
            xmlWriter.WriteAttributeString("ProcessStartTime", this.ProcessStartTime.ToString());
            xmlWriter.WriteAttributeString("ProcessEndTime", this.ProcessEndTime.ToString());
            xmlWriter.WriteAttributeString("ImpactedCount", this.ImpactedCount.ToString());
            xmlWriter.WriteAttributeString("ServerId", this.ServerId.ToString());
            xmlWriter.WriteAttributeString("ServerName", this.ServerName ?? "");
            xmlWriter.WriteAttributeString("Context", this.Context ?? "");
            xmlWriter.WriteAttributeString("ProgramName", this.ProgramName);
            xmlWriter.WriteAttributeString("UserName", this.UserName);
            xmlWriter.WriteAttributeString("ProfileId", this.ProfileId.ToString());
            xmlWriter.WriteAttributeString("IsSystem", this.IsSystem.ToString());

            #endregion

            //DQMJobQueueItem node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            resultXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return resultXml;
        }

        /// <summary>
        /// Get Xml representation of DQMJobQueueItem
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public override String ToXml(ObjectSerialization serialization)
        {
            // No serialization implemented for now...
            return this.ToXml();
        }

        #endregion
    }
}