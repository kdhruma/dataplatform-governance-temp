using System;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Threading; 
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.Diagnostics
{
    using MDM.Core;
    using MDM.Interfaces.Diagnostics;
    using MDM.Interfaces;

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    [ProtoBuf.ProtoContract]
    public class DiagnosticRecord : ObjectBase, IDiagnosticRecord
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
        public DateTime _timeStamp = DateTime.Now;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(3)]
        public Int64 _referenceId = -1;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(4)]
        private Guid _activityId = Guid.NewGuid();

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(5)]
        private Guid _operationId = Guid.Empty;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(6)]
        private String _messageCode = String.Empty;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(7)]
        private MessageClassEnum _messageClass = MessageClassEnum.UnKnown;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(8)]
        private String _message = String.Empty;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(9)]
        private Collection<String> _messageParameters = new Collection<String>();

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(10)]
        private String _dataXml = String.Empty;

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
        private Int32 _threadId = -1;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(13)]
        private Int32 _threadNumber = -1;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(14)]
        private Int64 _executionContextId = -1;

        /// <summary>
        /// Specifies Execution Context of the Record
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(15)]
        private ExecutionContext _executionContext = null;

        /// <summary>
        /// Specifies Extended Data existence status on DB level. Used for lazy loading purposes only.
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(16)]
        private Boolean _hasExtendedDataInDB = false;


        /// <summary>
        /// Reference of parent activity. This field is just for referencing parent activity and it must never be be serialized / transfered anywhere.
        /// </summary>
        private DiagnosticActivity _parentActivity = null;

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public DiagnosticRecord()
        {
            
        }


        /// <summary>
        /// DiagnosticRecord Constructor with ExecutionContext
        /// </summary>
        public DiagnosticRecord(ExecutionContext executionContext, Boolean hasExtendedDataInDB = false)
        {
            if (executionContext != null)
            {
                _executionContext = executionContext;
            }

            _hasExtendedDataInDB = hasExtendedDataInDB;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="referenceId"></param>
        /// <param name="stepNumber"></param>
        /// <param name="messageClass"></param>
        /// <param name="messageCode"></param>
        /// <param name="message"></param>
        public DiagnosticRecord(Int64 referenceId, String stepNumber, MessageClassEnum messageClass, String messageCode, String message)
        {
            _referenceId = referenceId;
            //_stepNumber = stepNumber;
            _messageClass = messageClass;
            _messageCode = messageCode;
            _message = message;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="referenceId"></param>
        /// <param name="stepNumber"></param>
        /// <param name="messageClass"></param>
        /// <param name="messageCode"></param>
        /// <param name="message"></param>
        /// <param name="durationInMilliseconds"></param>
        public DiagnosticRecord(Int64 referenceId, String stepNumber, MessageClassEnum messageClass, String messageCode, String message, Double durationInMilliseconds)
            : this(referenceId, stepNumber, messageClass, messageCode, message)
        {
            _durationInMilliseconds = durationInMilliseconds;
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
        public DiagnosticRecord(Int64 referenceId, String stepNumber, MessageClassEnum messageClass, String messageCode, String message, Collection<String> messageParameters, Double durationInMilliseconds)
            : this(referenceId, stepNumber, messageClass, messageCode, message)
        {
            _messageParameters = messageParameters;
            _durationInMilliseconds = durationInMilliseconds;
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
        /// <param name="dataXml"></param>
        public DiagnosticRecord(Int64 referenceId, String stepNumber, MessageClassEnum messageClass, String messageCode, String message, Collection<String> messageParameters, String dataXml)
            : this(referenceId, stepNumber, messageClass, messageCode, message) 
        {
            _messageParameters = messageParameters;
            _dataXml = dataXml;
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
        public DiagnosticRecord(Int64 referenceId, String stepNumber, MessageClassEnum messageClass, String messageCode, String message, Collection<String> messageParameters, Double durationInMilliseconds, String dataXml, Int32 threadNumber, Int32 threadId)
            : this(referenceId, stepNumber, messageClass, messageCode, message)
        {
            _messageParameters = messageParameters;
            _durationInMilliseconds = durationInMilliseconds;
            _dataXml = dataXml;
            _threadNumber = threadNumber;
            _threadId = threadId > 0 ? threadId : Thread.CurrentThread.ManagedThreadId;
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
        /// Property to get the TimeStamp
        /// </summary>
        public DateTime TimeStamp
        {
            get { return _timeStamp; }
            set { _timeStamp = value; }
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
        public Guid ActivityId
        {
            get { return _activityId; }
            set { _activityId = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid OperationId
        {
            get { return _operationId; }
            set { _operationId = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public String MessageCode
        {
            get { return _messageCode; }
            set { _messageCode = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public MessageClassEnum MessageClass
        {
            get { return _messageClass; }
            set { _messageClass = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public String Message
        {
            get { return _message; }
            set { _message = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Collection<String> MessageParameters
        {
            get { return _messageParameters; }
            set { _messageParameters = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public String DataXml
        {
            get { return _dataXml; }
            set { _dataXml = value; }
        }

        /// <summary>
        /// Specifies Extended Data existence status on DB level. Used for lazy loading purposes only.
        /// </summary>
        public Boolean HasExtendedDataInDB
        {
            get { return _hasExtendedDataInDB; }
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
        public Int32 ThreadId
        {
            get { return _threadId; }
            set { _threadId = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Int32 ThreadNumber
        {
            get { return _threadNumber; }
            set { _threadNumber = value; }
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
        /// Specifies Execution Context of the Record
        /// </summary>
        public ExecutionContext ExecutionContext
        {
            get { return _executionContext; }
            set { _executionContext = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DiagnosticActivity ParentActivity
        {
            get { return _parentActivity; }
            set { _parentActivity = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String ToXml()
        {
            String diagnosticRecordXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Parameter node start
            xmlWriter.WriteStartElement("DiagnosticRecord");

            xmlWriter.WriteAttributeString("TimeStamp", this.TimeStamp.ToString(SERIALIZATION_DATETIME_FORMAT, CultureInfo.InvariantCulture));
            xmlWriter.WriteAttributeString("ReferenceId", this.ReferenceId.ToString());
            xmlWriter.WriteAttributeString("ThreadId", this.ThreadId.ToString());
            xmlWriter.WriteAttributeString("MessageCode", this.MessageCode);
            xmlWriter.WriteAttributeString("MessageClass", this.MessageClass.ToString());
            xmlWriter.WriteAttributeString("Message", this.Message);
            xmlWriter.WriteAttributeString("MessageParameters", this.MessageParameters != null ? ValueTypeHelper.JoinCollection(this.MessageParameters, ",") : String.Empty);
            xmlWriter.WriteAttributeString("DurationInMilliSeconds", this.DurationInMilliSeconds.ToString(CultureInfo.InvariantCulture));

            if (this.ExecutionContext != null)
            {
                xmlWriter.WriteRaw(this.ExecutionContext.ToXml());
            }

            xmlWriter.WriteStartElement("DataXml");
            xmlWriter.WriteRaw(this.DataXml);
            xmlWriter.WriteEndElement();

            //Parameter node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            diagnosticRecordXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return diagnosticRecordXml;
        }

        /// <summary>
        /// Clone properties of current object, without its children
        /// </summary>
        /// <returns></returns>
        public DiagnosticRecord Clone()
        {
            DiagnosticRecord clone = new DiagnosticRecord();

            clone._id = this._id;
            clone._activityId = this._activityId;
            clone._dataXml = this._dataXml;
            clone._durationInMilliseconds = this._durationInMilliseconds;
            clone._message = this._message;
            clone._messageClass = this._messageClass;
            clone._messageCode = this._messageCode;
            clone._messageParameters = this._messageParameters;
            clone._operationId = this.OperationId;
            clone._parentActivity = this._parentActivity.ClonePropertiesOnly();
            clone._referenceId = this._referenceId;
            clone._threadId = this._threadId;
            clone._threadNumber = this._threadNumber;
            clone._timeStamp = this._timeStamp;
            clone.ReferenceId = this.ReferenceId;
            clone.ThreadId = this.ThreadId;
            clone._executionContextId = this._executionContextId;

            if (this._executionContext != null)
            {
                clone._executionContext = this._executionContext.Clone();
            }

            return clone;
        }

        /// <summary>
        /// Loads Diagnostic Record from Xml
        /// </summary>
        /// <param name="valueAsXml">Diagnostic Record serialized to Xml String</param>
        public void LoadFromXml(String valueAsXml)
        {
            if (String.IsNullOrEmpty(valueAsXml))
            {
                return;
            }

            XmlTextReader reader = null;
            
            try
            {
                reader = new XmlTextReader(valueAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "DiagnosticRecord")
                    {
                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("TimeStamp"))
                            {
                                TimeStamp = DateTime.ParseExact(reader.ReadContentAsString(), SERIALIZATION_DATETIME_FORMAT, CultureInfo.InvariantCulture);
                            }
                            if (reader.MoveToAttribute("ReferenceId"))
                            {
                                ReferenceId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
                            }
                            if (reader.MoveToAttribute("ThreadId"))
                            {
                                ThreadId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                            }
                            if (reader.MoveToAttribute("MessageClass"))
                            {
                                MessageClassEnum messageClass = MessageClassEnum.UnKnown;
                                if (Enum.TryParse(reader.ReadContentAsString(), out messageClass))
                                {
                                    if (messageClass == MessageClassEnum.StaticText)
                                    {
                                        // Backward compatibility for deprecated MessageClassEnum.StaticText
                                        messageClass = MessageClassEnum.Information;
                                    }
                                }
                                MessageClass = messageClass;
                            }
                            if (reader.MoveToAttribute("MessageCode"))
                            {
                                MessageCode = reader.ReadContentAsString();
                            }
                            if (reader.MoveToAttribute("Message"))
                            {
                                Message = reader.ReadContentAsString();
                            }
                            if (reader.MoveToAttribute("MessageParameters"))
                            {
                                MessageParameters = ValueTypeHelper.SplitStringToStringCollection(reader.ReadContentAsString(), ',');
                            }
                            if (reader.MoveToAttribute("DurationInMilliSeconds"))
                            {
                                Double duration;
                                if(Double.TryParse(reader.ReadContentAsString(), NumberStyles.AllowDecimalPoint, NumberFormatInfo.InvariantInfo, out duration))
                                {
                                    DurationInMilliSeconds = duration;
                                }
                            }
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExecutionContext")
                    {
                        String contextAsXml = reader.ReadOuterXml();

                        ExecutionContext = new ExecutionContext(contextAsXml);
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "DataXml")
                    {
                        DataXml = reader.ReadInnerXml();
                    }
                    else
                    {
                        reader.Read();
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

        #region IDiagnosticDataElement region

        /// <summary>
        /// GetExecutionContext()
        /// </summary>
        /// <returns></returns>
        public IExecutionContext  GetExecutionContext()
        {
            return _executionContext;
        }

        #endregion

        #endregion
    }
}
