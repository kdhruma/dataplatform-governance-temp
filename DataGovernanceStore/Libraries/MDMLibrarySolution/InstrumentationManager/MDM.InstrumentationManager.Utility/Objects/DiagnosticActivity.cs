using System;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Threading;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace MDM.BusinessObjects.Diagnostics
{
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.Interfaces.Diagnostics;
    using MDM.InstrumentationManager.Utility;
    using MDM.OperationContextManager.Business;

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    [ProtoBuf.ProtoContract]
    [KnownType(typeof(DiagnosticRecordCollection))]
    [KnownType(typeof(DiagnosticActivityCollection))]
    [Serializable()]
    public class DiagnosticActivity : IDiagnosticActivity, IDisposable
    {
        #region Constants

        /// <summary>
        /// Date Time format which is used for serialization and deserialization
        /// </summary>
        private const String SERIALIZATION_DATETIME_FORMAT = "MM/dd/yyyy hh:mm:ss.fff tt";

        #endregion

        #region Fields

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(1)]
        private Int64 _id = -1;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(2)]
        private DateTime _startDateTime = DateTime.Now;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(3)]
        private DateTime _endDateTime = DateTime.Now;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(4)]
        private DiagnosticRecordCollection _diagnosticRecords = new DiagnosticRecordCollection();

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(5)]
        private DiagnosticActivityCollection _diagnosticActivities = new DiagnosticActivityCollection();

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(6)]
        private Int64 _referenceId = -1;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(7)]
        private Guid _activityId = Guid.NewGuid();

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(8)]
        private Guid _parentActivityId = Guid.Empty;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(9)]
        private Guid _operationId = Guid.Empty;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(10)]
        private String _activityName = String.Empty;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(11)]
        private Double _durationInMilliseconds = 0;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(12)]
        private Int32 _threadId = 0;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(13)]
        private Int64 _currentSequenceNumber = 0;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(14)]
        private Int64 _executionContextId = -1;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(15)]
        private ExecutionContext _executionContext = null;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(16)]
        private Boolean _computeDurationOnSqlServerLevel = false;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(17)]
        private Boolean _doNotPersist = false;

        /// <summary>
        /// 
        /// </summary>
        private DateTime _lastTimeSpanStart = DateTime.Now;

        /// <summary>
        /// 
        /// </summary>
        private Boolean _isActivityStopped = false;

        /// <summary>
        /// 
        /// </summary>
        private Boolean _isActivityStarted = false;

        /// <summary>
        /// 
        /// </summary>
        private TraceSettings _traceSettings = null;

        /// <summary>
        /// 
        /// </summary>
        private static IDiagnosticDataProcessor _diagnosticDataProcessor = null;

        /// <summary>
        /// 
        /// </summary>
        private static Boolean _isInitialized = false;

        /// <summary>
        /// 
        /// </summary>
        private static DiagnosticActivity _unClassifiedTraceActivity = null;

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        static DiagnosticActivity()
        {
            CreateUnClassifiedTraceActivity();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="activityName"></param>
        /// <param name="referenceId"></param>
        /// <param name="filePath"></param>
        public DiagnosticActivity(ExecutionContext executionContext = null, [CallerMemberName] String activityName = "", Int64 referenceId = 1, [CallerFilePath] String filePath = "")
            : base()
        {
            _activityName = String.Format("{0}::{1}", Path.GetFileName(filePath), activityName);
            _threadId = Thread.CurrentThread.ManagedThreadId;
            _referenceId = referenceId;
            _parentActivityId = Guid.Empty;

            if (executionContext != null)
            {
                _executionContext = executionContext;
            }
        }

        /// <summary>
        /// Allows to create shared (between separate threads created by parallel JSON requests from UI) instance
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="operationId"></param>
        /// <param name="executionContext"></param>
        /// <param name="activityName"></param>
        /// <param name="referenceId"></param>
        /// <param name="filePath"></param>
        public DiagnosticActivity(Guid activityId, Guid operationId, ExecutionContext executionContext = null, [CallerMemberName] String activityName = "", Int64 referenceId = 1, [CallerFilePath] String filePath = "")
            : this(executionContext, activityName, referenceId, filePath)
        {
            _activityId = activityId;
            _operationId = operationId;
        }

        /// <summary>
        /// Allows to create shared (between separate threads created by parallel JSON requests from UI) instance
        /// </summary>
        /// <param name="parentActivityId">Identifier of parent diagnostic activity</param>
        /// <param name="activityId"></param>
        /// <param name="operationId"></param>
        /// <param name="executionContext"></param>
        /// <param name="activityName"></param>
        /// <param name="referenceId"></param>
        /// <param name="filePath"></param>
        public DiagnosticActivity(Guid parentActivityId, Guid activityId, Guid operationId, ExecutionContext executionContext = null, [CallerMemberName] String activityName = "", Int64 referenceId = 1, [CallerFilePath] String filePath = "")
            : this(executionContext, activityName, referenceId, filePath)
        {
            _parentActivityId = parentActivityId;
            _activityId = activityId;
            _operationId = operationId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentActivityId"></param>
        /// <param name="executionContext"></param>
        /// <param name="activityName"></param>
        /// <param name="referenceId"></param>
        /// <param name="filePath"></param>
        public DiagnosticActivity(Guid parentActivityId, ExecutionContext executionContext = null, [CallerMemberName] String activityName = "", Int64 referenceId = 1, [CallerFilePath] String filePath = "")
            : this(executionContext, activityName, referenceId, filePath)
        {
            _parentActivityId = parentActivityId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public Int64 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid ActivityId
        {
            get { return _activityId; }
            set { _activityId = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid ParentActivityId
        {
            get { return _parentActivityId; }
            set { _parentActivityId = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid OperationId
        {
            get
            {
                return (_operationId != Guid.Empty) ? _operationId : (_executionContext != null) ?
                    ((_executionContext.CallerContext != null && _executionContext.CallerContext.OperationId != Guid.Empty) ?
                    _executionContext.CallerContext.OperationId : _operationId) : Guid.Empty;

            }
            set { _operationId = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Boolean ComputeDurationOnSqlServerLevel
        {
            get { return _computeDurationOnSqlServerLevel; }
            set { _computeDurationOnSqlServerLevel = value; }
        }

        /// <summary>
        /// Property allows to avoid activity svaing. For example in case of recreated client activity behind WCF barrier.
        /// </summary>
        public Boolean DoNotPersist
        {
            get { return _doNotPersist; }
            set { _doNotPersist = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public String ActivityName
        {
            get { return _activityName; }
            set { _activityName = value; }
        }

        /// <summary>
        /// Property to get the TimeStamp
        /// </summary>
        public DateTime StartDateTime
        {
            get { return _startDateTime; }
            set { _startDateTime = value; }
        }

        /// <summary>
        /// Managed ThreadId of the Activity
        /// </summary>
        public Int32 ThreadId
        {
            get { return _threadId; }
            set { _threadId = value; }
        }

        /// <summary>
        /// Property to get the TimeStamp
        /// </summary>
        public DateTime EndDateTime
        {
            get { return _endDateTime; }
            set { _endDateTime = value; }
        }

        /// <summary>
        /// Property to get the TimeStamp
        /// </summary>
        public DateTime TimeStamp
        {
            get { return _startDateTime; }
            set { _startDateTime = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Int64 ReferenceId
        {
            get { return _referenceId; }
            set { _referenceId = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Double DurationInMilliSeconds
        {
            get { return _durationInMilliseconds; }
            set { _durationInMilliseconds = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DiagnosticRecordCollection DiagnosticRecords
        {
            get { return _diagnosticRecords; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DiagnosticActivityCollection DiagnosticActivities
        {
            get { return _diagnosticActivities; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Boolean IsActivityStarted
        {
            get { return _isActivityStarted; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Boolean IsActivityStopped
        {
            get { return _isActivityStopped; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Int64 ExecutionContextId
        {
            get { return _executionContextId; }
            set { _executionContextId = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ExecutionContext ExecutionContext
        {
            get { return _executionContext; }
        }

        /// <summary>
        /// 
        /// </summary>
        public TraceSettings TraceSettings
        {
            get { return _traceSettings; }
        }

        #endregion

        #region Methods

        #region Initialization Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Boolean InitializeProcessor(IDiagnosticDataProcessor diagnosticDataProcessor)
        {
            _diagnosticDataProcessor = diagnosticDataProcessor;
            _isInitialized = true;

            return _isInitialized;
        }

        #endregion

        #region Activity Mgt Methods

        /// <summary>
        /// Creates new activity and adds to current collection.
        /// </summary>
        /// <param name="activityName"></param>
        /// <param name="executionContext"></param>
        /// <param name="referenceId"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public DiagnosticActivity CreateChildActivity(String activityName = "", ExecutionContext executionContext = null, Int64 referenceId = 1, [CallerFilePath] String filePath = "")
        {
            DiagnosticActivity childDiagnosticActivity = new DiagnosticActivity(this.ActivityId, executionContext, activityName, referenceId, filePath);

            childDiagnosticActivity.OperationId = this.OperationId;

            AddDiagnosticActivityInternal(childDiagnosticActivity);

            return childDiagnosticActivity;
        }

        /// <summary>
        /// Get Root Activity - Given the trace settings, this method
        /// returns root activity for operations tracing or profile based tracing
        /// </summary>
        public static DiagnosticActivity GetRootActivity(String activityName, TraceSettings traceSettings, Guid operationId, bool showDiagnostics = false, [CallerFilePath] String filePath = "")
        {
            DiagnosticActivity activity = new DiagnosticActivity(null, activityName, filePath: filePath);

            if (!showDiagnostics)
            {
                if (traceSettings.TracingMode == TracingMode.SelectiveComponentTracing)
                {
                    activity.OperationId = Constants.ProfileTracingOperationId;
                }
            }
            else
            {
                traceSettings.UpdateSettings(true, TracingMode.OperationTracing, TracingLevel.Basic);

                if (!operationId.Equals(Guid.Empty))
                    activity.OperationId = operationId;
            }

            return activity;
        }

        /// <summary>
        /// Start Activity
        /// </summary>
        public void Start(ExecutionContext executionContext = null, String activityNameSuffix = "")
        {
            if (!String.IsNullOrWhiteSpace(activityNameSuffix))
            {
                _activityName = String.Concat(_activityName, "::", activityNameSuffix);
            }

            if (executionContext != null)
                _executionContext = executionContext;

            if (_traceSettings == null)
                _traceSettings = MDMOperationContext.Current.RequestContextData.TraceSettings.Clone();

            if (!_isActivityStarted)
            {
                if (_parentActivityId.Equals(Guid.Empty))
                {
                    var parentActivity = (DiagnosticActivity)LogicalCallStackManager.Peek();

                    if (parentActivity != null)
                    {
                        _parentActivityId = parentActivity.ActivityId;
                        _operationId = parentActivity.OperationId;

                        if (_executionContext != null && parentActivity.ExecutionContext != null)
                            _executionContext.Merge(parentActivity.ExecutionContext);
                        else if (parentActivity.ExecutionContext != null)
                            _executionContext = parentActivity.ExecutionContext.Clone();
                    }
                    else
                    {
                        if (_operationId.Equals(Guid.Empty))
                        {
                            _operationId = Guid.NewGuid();
                        }

                        if (_executionContext == null)
                            _executionContext = new ExecutionContext();
                    }
                }

                #region Populating default values of empty context parameters

                //if (_executionContext != null && _executionContext.SecurityContext != null)
                //{
                //    SecurityPrincipal currentSecurityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();

                //    if (String.IsNullOrEmpty(_executionContext.SecurityContext.UserLoginName))
                //    {
                //        _executionContext.SecurityContext.UserLoginName = currentSecurityPrincipal.CurrentUserName;
                //    }

                //    if (_executionContext.SecurityContext.UserId <= 0)
                //    {
                //        _executionContext.SecurityContext.UserId = currentSecurityPrincipal.CurrentUserId;
                //    }
                //}

                #endregion

                _isActivityStarted = true;

                LogicalCallStackManager.PushActivity(this);

                PersistDiagnosticActivity();
            }
        }

        /// <summary>
        /// Stop Activity
        /// </summary>
        public void Stop()
        {
            if (!_isActivityStarted || _isActivityStopped)
            {
                return;
            }

            LogicalCallStackManager.PopActivity();

            _endDateTime = DateTime.Now;
            TimeSpan timeSpan = _endDateTime - _startDateTime;
            _durationInMilliseconds = timeSpan.TotalMilliseconds;

            _isActivityStopped = true;

            PersistDiagnosticActivity();
        }

        /// <summary>
        /// Updates caller context from Diagnostic Activity
        /// </summary>
        /// <param name="callerContext"></param>
        public void UpdateCallerContext(CallerContext callerContext)
        {
            callerContext.OperationId = this.OperationId;
            callerContext.ActivityId = this.ActivityId;
            callerContext.TraceSettings = this.TraceSettings;
        }

        /// <summary>
        /// Update Diagnostic Activity settings from caller Context.
        /// </summary>
        /// <param name="callerContext"></param>
        public void UpdateFromCallerContext(CallerContext callerContext)
        {
            this.OperationId = callerContext.OperationId;
            this.ParentActivityId = callerContext.ActivityId;
        }

        #endregion

        #region Logging Methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public void LogVerbose(String message)
		{
			LogMessage(MessageClassEnum.Verbose, String.Empty, message);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="messageCode"></param>
		/// <param name="message"></param>
		public void LogVerbose(String messageCode, String message)
		{
			LogMessage(MessageClassEnum.Verbose, messageCode, message);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void LogInformation(String message)
        {
            LogMessage(MessageClassEnum.Information, String.Empty, message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageCode"></param>
        /// <param name="message"></param>
        public void LogInformation(String messageCode, String message)
        {
            LogMessage(MessageClassEnum.Information, messageCode, message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void LogWarning(String message)
        {
            LogMessage(MessageClassEnum.Warning, String.Empty, message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageCode"></param>
        /// <param name="message"></param>
        public void LogWarning(String messageCode, String message)
        {
            LogMessage(MessageClassEnum.Warning, messageCode, message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void LogError(String message)
        {
            LogMessage(MessageClassEnum.Error, String.Empty, message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageCode"></param>
        /// <param name="message"></param>
        public void LogError(String messageCode, String message)
        {
            LogMessage(MessageClassEnum.Error, messageCode, message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="dataAsXmlString"></param>
        public void LogData(String message, String dataAsXmlString)
        {
            LogMessageWithData(String.Empty, message, dataAsXmlString);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageCode"></param>
        /// <param name="message"></param>
        /// <param name="dataAsXmlString"></param>
        public void LogData(String messageCode, String message, String dataAsXmlString)
        {
            LogMessageWithData(messageCode, message, dataAsXmlString);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageClass"></param>
        /// <param name="message"></param>
        public void LogMessage(MessageClassEnum messageClass, String message)
        {
            LogMessage(messageClass, String.Empty, message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageClass"></param>
        /// <param name="messageCode"></param>
        /// <param name="message"></param>
        public void LogMessage(MessageClassEnum messageClass, String messageCode, String message)
        {
            this.AddDiagnosticRecord(-1, messageClass, messageCode, message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="dataAsXmlString"></param>
        public void LogMessageWithData(String message, String dataAsXmlString)
        {
            LogMessageWithData(String.Empty, message, dataAsXmlString);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageCode"></param>
        /// <param name="message"></param>
        /// <param name="dataAsXmlString"></param>
        public void LogMessageWithData(String messageCode, String message, String dataAsXmlString)
        {
            AddDiagnosticRecordInternal(-1, String.Empty, MessageClassEnum.Information, messageCode, message, null, 0, dataAsXmlString);
        }

        /// <summary>
        /// Log diagnostic message with data.
        /// </summary>
        /// <param name="message">Indicates actual mesage to log.</param>
        /// <param name="dataAsXmlString">Indicates additional data to be logged.</param>
        /// <param name="messageType">Indicates the type of message.</param>
        public void LogMessageWithData(String message, String dataAsXmlString, MessageClassEnum messageType)
        {
            LogMessageWithData(String.Empty, message, dataAsXmlString, messageType);
        }

        /// <summary>
        /// Log diagnostic message with data.
        /// </summary>
        /// <param name="messageCode">Indicates message code for localization.</param>
        /// <param name="message">Indicates actual mesage to log.</param>
        /// <param name="dataAsXmlString">Indicates additional data to be logged.</param>
        /// <param name="messageType">Indicates the type of message.</param>
        public void LogMessageWithData(String messageCode, String message, String dataAsXmlString, MessageClassEnum messageType)
        {
            AddDiagnosticRecordInternal(-1, String.Empty, messageType, messageCode, message, null, 0, dataAsXmlString);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void LogDurationInfo(String message)
        {
            Double duration = (DateTime.Now - _lastTimeSpanStart).TotalMilliseconds;
            _lastTimeSpanStart = DateTime.Now;

            LogMessageWithDuration(MessageClassEnum.TimeSpan, String.Empty, message, duration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="duration"></param>
        public void LogDurationInfo(String message, Double duration)
        {
            LogMessageWithDuration(MessageClassEnum.TimeSpan, String.Empty, message, duration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageCode"></param>
        /// <param name="message"></param>
        public void LogDurationInfo(String messageCode, String message)
        {
            Double duration = (DateTime.Now - _lastTimeSpanStart).TotalMilliseconds;
            _lastTimeSpanStart = DateTime.Now;

            LogMessageWithDuration(MessageClassEnum.TimeSpan, messageCode, message, duration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageClass"></param>
        /// <param name="messageCode"></param>
        /// <param name="message"></param>
        /// <param name="duration"></param>
        public void LogMessageWithDuration(MessageClassEnum messageClass, String messageCode, String message, Double duration)
        {
            this.AddDiagnosticRecord(-1, "", MessageClassEnum.TimeSpan, messageCode, message, duration);
        }

        #endregion

        #region Diagnostic Records and Activity Add Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="diagnosticActivity"></param>
        public void AddDiagnosticActivity(DiagnosticActivity diagnosticActivity)
        {
            if (diagnosticActivity == null)
            {
                return;
            }

            AddDiagnosticActivityInternal(diagnosticActivity);
        }

        /// <summary>
        /// AddDiagnosticActivity collection to current activity
        /// </summary>
        /// <param name="activityCollection"></param>
        public void AddDiagnosticActivity(Collection<DiagnosticActivity> activityCollection)
        {
            foreach (var childActivity in activityCollection)
            {
                this.AddDiagnosticActivity(childActivity);
            }
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String ToXml()
        {
            String diagnosticActivityXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Parameter node start
            xmlWriter.WriteStartElement("DiagnosticActivity");

            xmlWriter.WriteAttributeString("TimeStamp", this.TimeStamp.ToString(SERIALIZATION_DATETIME_FORMAT, CultureInfo.InvariantCulture));
            xmlWriter.WriteAttributeString("StartTime", this.StartDateTime.ToString(SERIALIZATION_DATETIME_FORMAT, CultureInfo.InvariantCulture));
            xmlWriter.WriteAttributeString("EndTime", this.EndDateTime.ToString(SERIALIZATION_DATETIME_FORMAT, CultureInfo.InvariantCulture));
            xmlWriter.WriteAttributeString("ThreadId", this.ThreadId.ToString());
            xmlWriter.WriteAttributeString("ActivityName", this.ActivityName);
            xmlWriter.WriteAttributeString("DurationInMilliSeconds", this.DurationInMilliSeconds.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteAttributeString("ReferenceId", this.ReferenceId.ToString());

            if (this.ExecutionContext != null)
            {
                xmlWriter.WriteRaw(this.ExecutionContext.ToXml());
            }

            if (this.DiagnosticRecords != null)
            {
                xmlWriter.WriteRaw(this.DiagnosticRecords.ToXml());
            }

            if (this.DiagnosticActivities != null)
            {
                xmlWriter.WriteRaw(this.DiagnosticActivities.ToXml());
            }

            //Parameter node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            diagnosticActivityXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return diagnosticActivityXml;
        }

        /// <summary>
        /// Clone properties of current object, without its children
        /// </summary>
        /// <returns></returns>
        public DiagnosticActivity ClonePropertiesOnly()
        {
            DiagnosticActivity clone = new DiagnosticActivity();

            clone.Id = this.Id;
            clone.ActivityId = this.ActivityId;
            clone.ActivityName = this.ActivityName;
            clone.DurationInMilliSeconds = this.DurationInMilliSeconds;
            clone._endDateTime = this.EndDateTime;
            clone.ExecutionContextId = this.ExecutionContextId;

            if (this._executionContext != null)
            {
                clone._executionContext = this.ExecutionContext.Clone();
            }

            clone._computeDurationOnSqlServerLevel = this._computeDurationOnSqlServerLevel;
            clone._doNotPersist = this._doNotPersist;

            clone._isActivityStarted = this._isActivityStarted;
            clone._isActivityStopped = this._isActivityStopped;
            clone._operationId = this._operationId;
            clone._parentActivityId = this.ParentActivityId;
            clone._referenceId = this._referenceId;
            clone._startDateTime = this.StartDateTime;
            clone._threadId = this._threadId;

            if (this._traceSettings != null)
            {
                clone._traceSettings = this.TraceSettings.Clone();
            }

            return clone;
        }
        
        /// <summary>
        /// Loads Diagnostic Activity from XmlTextReader 
        /// </summary>
        /// <param name="reader"></param>
        public void LoadFromStream(XmlReader reader)
        {
            if (reader != null)
            {
                ReadAttributesFromStream(reader);

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "DiagnosticActivity")
                    {
                        return;
                    }

                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "DiagnosticRecords")
                    {
                        XmlReader diagnosticRecordsSubtreeReader = reader.ReadSubtree();

                        if (diagnosticRecordsSubtreeReader.Read())
                        {
                            String recordsAsXml = diagnosticRecordsSubtreeReader.ReadOuterXml();

                            _diagnosticRecords.LoadFromXml(recordsAsXml);
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExecutionContext")
                    {
                        XmlReader executionContextSubtreeReader = reader.ReadSubtree();

                        if (executionContextSubtreeReader.Read())
                        {
                            String contextAsXml = executionContextSubtreeReader.ReadOuterXml();
                            
                            _executionContext = new ExecutionContext(contextAsXml);
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "DiagnosticActivities")
                    {
                        _diagnosticActivities.LoadFromStream(reader);
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        #region Add Methods for DiagnosticRecord and DiagnosticActivity

        /// <summary>
        /// 
        /// </summary>
        /// <param name="referenceId"></param>
        /// <param name="stepNumber"></param>
        /// <param name="messageClass"></param>
        /// <param name="messageCode"></param>
        /// <param name="message"></param>
        /// <param name="durationInMilliseconds"></param>
        private void AddDiagnosticRecord(Int64 referenceId, String stepNumber, MessageClassEnum messageClass, String messageCode, String message, Double durationInMilliseconds)
        {
            this.AddDiagnosticRecordInternal(referenceId, stepNumber, messageClass, messageCode, message, null, durationInMilliseconds, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="referenceId"></param>
        /// <param name="messageClass"></param>
        /// <param name="messageCode"></param>
        /// <param name="message"></param>
        private void AddDiagnosticRecord(Int64 referenceId, MessageClassEnum messageClass, String messageCode, String message)
        {
            this.AddDiagnosticRecordInternal(referenceId, "", messageClass, messageCode, message, null, 0, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="diagnosticRecord"></param>
        public void AddDiagnosticRecord(DiagnosticRecord diagnosticRecord)
        {
            AddDiagnosticRecordInternal(diagnosticRecord);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="referenceId"></param>
        /// <param name="stepNumber"></param>
        /// <param name="messageClass"></param>
        /// <param name="messageCode"></param>
        /// <param name="message"></param>
        /// <param name="messageParameters"></param>
        /// <param name="durationInMilliseconds"></param>
        /// <param name="dataXml"></param>
        /// <param name="threadNumber"></param>
        /// <param name="threadId"></param>
        private void AddDiagnosticRecordInternal(Int64 referenceId, String stepNumber, MessageClassEnum messageClass, String messageCode, String message, Collection<String> messageParameters, Double durationInMilliseconds, String dataXml, Int32 threadNumber = -1, Int32 threadId = -1)
        {
            var diagnosticRecord = new DiagnosticRecord(referenceId, stepNumber, messageClass, messageCode, message, messageParameters, durationInMilliseconds, dataXml, threadNumber, threadId);
            AddDiagnosticRecordInternal(diagnosticRecord);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="diagnosticRecord"></param>
        private void AddDiagnosticRecordInternal(DiagnosticRecord diagnosticRecord)
        {
            var parentActivity = this;

            if (!this.IsActivityStarted && !this.IsActivityStopped)
            {
                parentActivity = _unClassifiedTraceActivity;
                parentActivity._traceSettings = MDMOperationContext.Current.RequestContextData.TraceSettings.Clone();
                //this.Start();
            }

            diagnosticRecord.ExecutionContext = parentActivity.ExecutionContext;
            diagnosticRecord.ReferenceId = Interlocked.Increment(ref parentActivity._currentSequenceNumber);
            diagnosticRecord.ActivityId = parentActivity.ActivityId;
            diagnosticRecord.OperationId = parentActivity.OperationId;
            diagnosticRecord.ParentActivity = parentActivity;

            if (diagnosticRecord.MessageClass == MessageClassEnum.Error || diagnosticRecord.MessageClass == MessageClassEnum.Warning)
            {
                //TODO: Think about moving the scope to the target frame. Now we get entire stack with AddDiagnosticRecordInternal on top.

                if (diagnosticRecord.ExecutionContext == null)
                {
                    diagnosticRecord.ExecutionContext = new ExecutionContext();
                }

                diagnosticRecord.ExecutionContext.AdditionalContextData = String.Format("{0} stack trace:\n<![CDATA[\n{1}\n{2}\n]]>",
                    diagnosticRecord.MessageClass.ToString(),
                    new StackFrame(true).ToString(),
                    new StackTrace().ToString());
            }

            PersistDiagnosticRecord(diagnosticRecord);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="diagnosticActivity"></param>
        private void AddDiagnosticActivityInternal(DiagnosticActivity diagnosticActivity)
        {
            diagnosticActivity.ReferenceId = Interlocked.Increment(ref _currentSequenceNumber);
        }

        #endregion

        #region Post to processor / Persist methods

        /// <summary>
        /// 
        /// </summary>
        private void PersistDiagnosticActivity()
        {
            var clonedDiagnosticActivityWithOnlyProperties = this.ClonePropertiesOnly();
            PostToDataProcessor(clonedDiagnosticActivityWithOnlyProperties);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="diagnosticRecord"></param>
        private void PersistDiagnosticRecord(DiagnosticRecord diagnosticRecord)
        {
            var clonedDiagnosticRecord = diagnosticRecord.Clone();
            PostToDataProcessor(clonedDiagnosticRecord);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="diagnosticDataElement"></param>
        private void PostToDataProcessor(IDiagnosticDataElement diagnosticDataElement)
        {
            if (_isInitialized && _diagnosticDataProcessor != null)
            {
                _diagnosticDataProcessor.Post(diagnosticDataElement);
            }
        }

        #endregion

        #region Execution Context Mgt Methods

        #endregion

        #region UnClassifiedTraceActivity Mgt Methods

        /// <summary>
        /// 
        /// </summary>
        public static void CreateUnClassifiedTraceActivity()
        {
            _unClassifiedTraceActivity = new DiagnosticActivity(null, "All UnClassified Traces", -100000, "MDM");

            Guid operationId = Constants.ProfileTracingOperationId;
            Guid activityId = new Guid("11119999-9999-9999-9999-999999999991");

            _unClassifiedTraceActivity.ParentActivityId = operationId;
            _unClassifiedTraceActivity.OperationId = operationId;
            _unClassifiedTraceActivity.ActivityId = activityId;

            _unClassifiedTraceActivity._traceSettings = new TraceSettings(true, TracingMode.SelectiveComponentTracing, TracingLevel.Basic);

            _unClassifiedTraceActivity.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static DiagnosticActivity GetUnClassifiedTraceActivity()
        {
            return _unClassifiedTraceActivity;
        }

        #endregion

        #region Utility Private Methods

        private void ReadAttributesFromStream(XmlReader reader)
        {
            if (reader != null && reader.HasAttributes)
            {
                if (reader.MoveToAttribute("ActivityName"))
                {
                    ActivityName = reader.ReadContentAsString();
                }
                if (reader.MoveToAttribute("DurationInMilliSeconds"))
                {
                    Double duration;
                    if (Double.TryParse(reader.ReadContentAsString(), NumberStyles.AllowDecimalPoint, NumberFormatInfo.InvariantInfo, out duration))
                    {
                        DurationInMilliSeconds = duration;
                    }
                }
                if (reader.MoveToAttribute("TimeStamp"))
                {
                    TimeStamp = DateTime.ParseExact(reader.ReadContentAsString(), SERIALIZATION_DATETIME_FORMAT, CultureInfo.InvariantCulture);
                }
                if (reader.MoveToAttribute("StartTime"))
                {
                    StartDateTime = DateTime.ParseExact(reader.ReadContentAsString(), SERIALIZATION_DATETIME_FORMAT, CultureInfo.InvariantCulture);
                }
                if (reader.MoveToAttribute("EndTime"))
                {
                    EndDateTime = DateTime.ParseExact(reader.ReadContentAsString(), SERIALIZATION_DATETIME_FORMAT, CultureInfo.InvariantCulture);
                }
                if (reader.MoveToAttribute("ThreadId"))
                {
                    ThreadId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                }
                if (reader.MoveToAttribute("ReferenceId"))
                {
                    ReferenceId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                }
            }
        }

        #endregion

        #endregion

        #region IDiagnosticDataElement region

        /// <summary>
        /// GetExecutionContext()
        /// </summary>
        /// <returns></returns>
        public IExecutionContext GetExecutionContext()
        {
            return _executionContext;
        }

        #endregion

        #region Idispose implementation

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (this.IsActivityStarted && !this.IsActivityStopped)
                this.Stop();
        }

        #endregion

        #endregion
    }
}
