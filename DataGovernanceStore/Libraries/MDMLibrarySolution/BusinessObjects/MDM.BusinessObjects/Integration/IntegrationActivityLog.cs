using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Integration
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Class for Integration activity log
    /// </summary>
    [DataContract]
    public class IntegrationActivityLog : ObjectBase, IIntegrationActivityLog, IIntegrationItem
    {
        #region Fields

        /// <summary>
        /// Indicates Id of integration activity log
        /// </summary>
        private Int64 _id = -1;

        /// <summary>
        /// Indicates action to be performed on the object
        /// </summary>
        private ObjectAction _action = ObjectAction.Read;

        /// <summary>
        /// Indicates object id for which this activity log has been created
        /// </summary>
        private Int64 _mdmObjectId = -1;

        /// <summary>
        /// Indicates id of Object type
        /// </summary>
        private Int16 _mdmObjectTypeId = -1;

        /// <summary>
        /// Indicates name MDMObject type
        /// </summary>
        private String _mdmObjectTypeName = String.Empty;

        /// <summary>
        /// Indicates type of message
        /// </summary>
        private Int16 _integrationMessageTypeId = -1;

        /// <summary>
        /// Indicates Long name of message type
        /// </summary>
        private String _integrationMessageTypeLongName = String.Empty;

        /// <summary>
        /// Indicates Short name of message type
        /// </summary>
        private String _integrationMessageTypeName = String.Empty;

        /// <summary>
        /// Indicates Id of connector for which this message is sent
        /// </summary>
        private Int16 _connectorId = -1;

        /// <summary>
        /// Indicates Long name of connector for which this message is sent
        /// </summary>
        private String _connectorLongName = String.Empty;

        /// <summary>
        /// Indicates Short name of connector for which this message is sent
        /// </summary>
        private String _connectorName = String.Empty;

        /// <summary>
        /// Indicates context for message. Typically some context information for MDMObjectId
        /// </summary>
        private String _context = String.Empty;

        /// <summary>
        /// Indicates no. of messages created from this update.
        /// </summary>
        private Int32 _messageCount = -1;

        /// <summary>
        /// Indicates if loading the message for this activity is in progress
        /// </summary>
        private Boolean _isLoadingInProgress = false;

        /// <summary>
        /// Indicates if messages for this activity are already loaded
        /// </summary>
        private Boolean _isLoaded = false;

        /// <summary>
        /// Indicates if messages for this activity are already processed
        /// </summary>
        private Boolean _isProcessed = false;

        /// <summary>
        /// Indicates which server initiated this change
        /// </summary>
        private Int32 _serverId = -1;

        /// <summary>
        /// Indicates weightage for this change
        /// </summary>
        private Int32 _weightage = -1;

        /// <summary>
        /// Indicates time when creating message started.
        /// </summary>
        private DateTime? _messageLoadStartTime = null;

        /// <summary>
        /// Indicates time when creating message finished.
        /// </summary>
        private DateTime? _messageLoadEndTime = null;

        /// <summary>
        /// Indicates time when processing the messages started.
        /// </summary>
        private DateTime? _processStartTime = null;

        /// <summary>
        /// Indicates time when processing the messages finished.
        /// </summary>
        private DateTime? _processEndTime = null;

        /// <summary>
        /// Indicates Reference field for the object
        /// </summary>
        private String _referenceId = String.Empty;

        /// <summary>
        /// Indicates the type of integration. Is this activity log for inbound processing or outbound processing
        /// </summary>
        private IntegrationType _integrationType = IntegrationType.Unknown;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public IntegrationActivityLog()
            : base()
        {
        }

        /// <summary>
        /// Xml constructor
        /// </summary>
        /// <param name="valuesAsXml">IntegrationActivityLog xml having </param>
        public IntegrationActivityLog(String valuesAsXml)
        {
            LoadIntegrationActivityLogFromXml(valuesAsXml);
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Indicates Id of integration activity log
        /// </summary>
        [DataMember]
        public Int64 Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        /// <summary>
        /// Indicates action to be performed on the object
        /// </summary>
        [DataMember]
        public ObjectAction Action
        {
            get
            {
                return _action;
            }
            set
            {
                _action = value;
            }
        }

        /// <summary>
        /// Indicates object id for which this activity log has been created
        /// </summary>
        [DataMember]
        public Int64 MDMObjectId
        {
            get
            {
                return _mdmObjectId;
            }
            set
            {
                _mdmObjectId = value;
            }
        }

        /// <summary>
        /// Indicates id of Object type
        /// </summary>
        [DataMember]
        public Int16 MDMObjectTypeId
        {
            get
            {
                return _mdmObjectTypeId;
            }
            set
            {
                _mdmObjectTypeId = value;
            }
        }

        /// <summary>
        /// Indicates name MDMObject type
        /// </summary>
        [DataMember]
        public String MDMObjectTypeName
        {
            get
            {
                return _mdmObjectTypeName;
            }
            set
            {
                _mdmObjectTypeName = value;
            }
        }

        /// <summary>
        /// Indicates type of message
        /// </summary>
        [DataMember]
        public Int16 IntegrationMessageTypeId
        {
            get
            {
                return _integrationMessageTypeId;
            }
            set
            {
                _integrationMessageTypeId = value;
            }
        }

        /// <summary>
        /// Indicates Long name of message type
        /// </summary>
        [DataMember]
        public String IntegrationMessageTypeLongName
        {
            get
            {
                return _integrationMessageTypeLongName;
            }
            set
            {
                _integrationMessageTypeLongName = value;
            }
        }

        /// <summary>
        /// Indicates Short name of message type
        /// </summary>
        [DataMember]
        public String IntegrationMessageTypeName
        {
            get { return _integrationMessageTypeName; }
            set { _integrationMessageTypeName = value; }
        }

        /// <summary>
        /// Indicates Id of connector for which this message is sent
        /// </summary>
        [DataMember]
        public Int16 ConnectorId
        {
            get
            {
                return _connectorId;
            }
            set
            {
                _connectorId = value;
            }
        }

        /// <summary>
        /// Indicates Long name of connector for which this message is sent
        /// </summary>
        [DataMember]
        public String ConnectorLongName
        {
            get
            {
                return _connectorLongName;
            }
            set
            {
                _connectorLongName = value;
            }
        }

        /// <summary>
        /// Indicates Short name of connector for which this message is sent
        /// </summary>
        [DataMember]
        public String ConnectorName
        {
            get { return _connectorName; }
            set { _connectorName = value; }
        }

        /// <summary>
        /// Indicates context for message. Typically some context information for MDMObjectId in xml format.
        /// </summary>
        [DataMember]
        public String Context
        {
            get
            {
                return _context;
            }
            set
            {
                _context = value;
            }
        }

        /// <summary>
        /// Indicates no. of messages created from this update.
        /// </summary>
        [DataMember]
        public Int32 MessageCount
        {
            get
            {
                return _messageCount;
            }
            set
            {
                _messageCount = value;
            }
        }

        /// <summary>
        /// Indicates if loading the message for this activity is in progress
        /// </summary>
        [DataMember]
        public Boolean IsLoadingInProgress
        {
            get
            {
                return _isLoadingInProgress;
            }
            set
            {
                _isLoadingInProgress = value;
            }
        }

        /// <summary>
        /// Indicates if messages for this activity are already loaded
        /// </summary>
        [DataMember]
        public Boolean IsLoaded
        {
            get
            {
                return _isLoaded;
            }
            set
            {
                _isLoaded = value;
            }
        }

        /// <summary>
        /// Indicates if messages for this activity are already processed
        /// </summary>
        [DataMember]
        public Boolean IsProcessed
        {
            get
            {
                return _isProcessed;
            }
            set
            {
                _isProcessed = value;
            }
        }

        /// <summary>
        /// Indicates which server initiated this change
        /// </summary>
        [DataMember]
        public Int32 ServerId
        {
            get
            {
                return _serverId;
            }
            set
            {
                _serverId = value;
            }
        }

        /// <summary>
        /// Indicates weightage for this change
        /// </summary>
        [DataMember]
        public Int32 Weightage
        {
            get
            {
                return _weightage;
            }
            set
            {
                _weightage = value;
            }
        }

        /// <summary>
        /// Indicates time when creating message started.
        /// </summary>
        [DataMember]
        public DateTime? MessageLoadStartTime
        {
            get
            {
                return _messageLoadStartTime;
            }
            set
            {
                _messageLoadStartTime = value;
            }
        }

        /// <summary>
        /// Indicates time when creating message finished.
        /// </summary>
        [DataMember]
        public DateTime? MessageLoadEndTime
        {
            get
            {
                return _messageLoadEndTime;
            }
            set
            {
                _messageLoadEndTime = value;
            }
        }

        /// <summary>
        /// Indicates time when processing the messages started.
        /// </summary>
        [DataMember]
        public DateTime? ProcessStartTime
        {
            get
            {
                return _processStartTime;
            }
            set
            {
                _processStartTime = value;
            }
        }

        /// <summary>
        /// Indicates time when processing the messages finished.
        /// </summary>
        [DataMember]
        public DateTime? ProcessEndTime
        {
            get
            {
                return _processEndTime;
            }
            set
            {
                _processEndTime = value;
            }
        }

        /// <summary>
        /// Indicates Reference field for the object
        /// </summary>
        [DataMember]
        public String ReferenceId
        {
            get { return _referenceId; }
            set { _referenceId = value; }
        }

        /// <summary>
        /// Indicates the type of integration. Is this activity log for inbound processing or outbound processing
        /// </summary>
        [DataMember]
        public IntegrationType IntegrationType
        {
            get { return _integrationType; }
            set { _integrationType = value; }
        }

        /// <summary>
        /// Indicates Integration Id. In case of exception handling for queues, Integration Id indicates the PK of respective table. 
        /// And based on ProcessorName and Integration Id, record will be marked for reprocessing in respective table.
        /// For example, if ProcessorName = "IntegrationQualifyingQueueLoadProcessor" and IntegrationId = 5, then tb_IntegrationActivityLog with PK = 5 will be marked for re-process.
        /// </summary>
        [DataMember]
        public Int64 IntegrationId
        {
            get
            {
                return this.Id;
            }

            private set
            {
                this.Id = value;
            }
        }
        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents IntegrationActivityLog in Xml format
        /// </summary>
        public String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);
            String xml = String.Empty;

            xmlWriter.WriteStartElement("IntegrationActivityLog");

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("ReferenceId", this.ReferenceId);
            xmlWriter.WriteAttributeString("MDMObjectId", this.MDMObjectId.ToString());
            xmlWriter.WriteAttributeString("MDMObjectTypeId", this.MDMObjectTypeId.ToString());
            xmlWriter.WriteAttributeString("MDMObjectTypeName", this.MDMObjectTypeName);
            xmlWriter.WriteAttributeString("IntegrationMessageTypeId", this.IntegrationMessageTypeId.ToString());
            xmlWriter.WriteAttributeString("IntegrationMessageTypeName", this.IntegrationMessageTypeName);
            xmlWriter.WriteAttributeString("IntegrationMessageTypeLongName", this.IntegrationMessageTypeLongName);
            xmlWriter.WriteAttributeString("ConnectorId", this.ConnectorId.ToString());
            xmlWriter.WriteAttributeString("ConnectorLongName", this.ConnectorLongName);
            xmlWriter.WriteAttributeString("ConnectorName", this.ConnectorName);
            xmlWriter.WriteAttributeString("MessageCount", this.MessageCount.ToString());
            xmlWriter.WriteAttributeString("IsLoadingInProgress", this.IsLoadingInProgress.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("IsLoaded", this.IsLoaded.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("IsProcessed", this.IsProcessed.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("ServerId", this.ServerId.ToString());
            xmlWriter.WriteAttributeString("Weightage", this.Weightage.ToString());
            xmlWriter.WriteAttributeString("MessageLoadStartTime", this.MessageLoadStartTime.ToString());
            xmlWriter.WriteAttributeString("MessageLoadEndTime", this.MessageLoadEndTime.ToString());
            xmlWriter.WriteAttributeString("ProcessStartTime", this.ProcessStartTime.ToString());
            xmlWriter.WriteAttributeString("ProcessEndTime", this.ProcessEndTime.ToString());
            xmlWriter.WriteAttributeString("Action", this.Action.ToString());
            xmlWriter.WriteAttributeString("IntegrationType", this.IntegrationType.ToString());

            xmlWriter.WriteStartElement("Context");
            if (!String.IsNullOrWhiteSpace(this.Context))
            {
                xmlWriter.WriteRaw(this.Context);
            }

            //ContextXml end
            xmlWriter.WriteEndElement();

            //IntegrationActivityLog end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Clone the IntegrationActivityLog and create a new IntegrationActivityLog object
        /// </summary>
        /// <returns>New IntegrationActivityLog object having same value as current one.</returns>
        public IIntegrationActivityLog Clone()
        {
            IntegrationActivityLog clonedLog = new IntegrationActivityLog();
            clonedLog.Action = this.Action;
            clonedLog.ConnectorId = this.ConnectorId;
            clonedLog.ConnectorLongName = this.ConnectorLongName;
            clonedLog.Context = this.Context;
            clonedLog.Id = this.Id;
            clonedLog.IntegrationMessageTypeId = this.IntegrationMessageTypeId;
            clonedLog.IntegrationMessageTypeLongName = this.IntegrationMessageTypeLongName;
            clonedLog.IntegrationType = this.IntegrationType;
            clonedLog.IsLoaded = this.IsLoaded;
            clonedLog.IsLoadingInProgress = this.IsLoadingInProgress;
            clonedLog.IsProcessed = this.IsProcessed;
            clonedLog.MDMObjectId = this.MDMObjectId;
            clonedLog.MDMObjectTypeId = this.MDMObjectTypeId;
            clonedLog.MDMObjectTypeName = this.MDMObjectTypeName;
            clonedLog.MessageCount = this.MessageCount;
            clonedLog.MessageLoadEndTime = this.MessageLoadEndTime;
            clonedLog.MessageLoadStartTime = this.MessageLoadStartTime;
            clonedLog.ProcessEndTime = this.ProcessEndTime;
            clonedLog.ProcessStartTime = this.ProcessStartTime;
            clonedLog.ReferenceId = this.ReferenceId;
            clonedLog.ServerId = this.ServerId;
            clonedLog.Weightage = this.Weightage;

            return clonedLog;
        }

        #endregion Methods

        #region Private Methods
        
        /// <summary>
        /// Load integration activity log object from xml
        /// </summary>
        /// <param name="valuesAsXml">XML having xml values</param>
        private void LoadIntegrationActivityLogFromXml(string valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "IntegrationActivityLog" && reader.IsStartElement())
                        {
                            #region Read Integration Message Attributes

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                    this.Id = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), -1);

                                if (reader.MoveToAttribute("ReferenceId"))
                                    this.ReferenceId = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("MDMObjectId"))
                                    this.MDMObjectId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), -1);

                                if (reader.MoveToAttribute("MDMObjectTypeId"))
                                    this.MDMObjectTypeId = ValueTypeHelper.Int16TryParse(reader.ReadContentAsString(), -1);

                                if (reader.MoveToAttribute("MDMObjectTypeName"))
                                    this.MDMObjectTypeName = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("IntegrationMessageTypeId"))
                                    this.IntegrationMessageTypeId = ValueTypeHelper.Int16TryParse(reader.ReadContentAsString(), -1);

                                if (reader.MoveToAttribute("IntegrationMessageTypeLongName"))
                                    this.IntegrationMessageTypeLongName = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("IntegrationMessageTypeShortName "))
                                    this.IntegrationMessageTypeName = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("ConnectorId"))
                                    this.ConnectorId = ValueTypeHelper.Int16TryParse(reader.ReadContentAsString(), -1);

                                if (reader.MoveToAttribute("ConnectorLongName"))
                                    this.ConnectorLongName = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("ConnectorShortName"))
                                    this.ConnectorName = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("MessageCount"))
                                    this.MessageCount = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), -1);

                                if (reader.MoveToAttribute("IsLoadingInProgress"))
                                    this.IsLoadingInProgress = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);

                                if (reader.MoveToAttribute("IsLoaded"))
                                    this.IsLoaded = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);

                                if (reader.MoveToAttribute("IsProcessed"))
                                    this.IsProcessed = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);

                                if (reader.MoveToAttribute("ServerId"))
                                    this.ServerId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), -1);

                                if (reader.MoveToAttribute("Weightage"))
                                    this.Weightage = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), -1);

                                if (reader.MoveToAttribute("MessageLoadStartTime"))
                                    this.MessageLoadStartTime = ValueTypeHelper.ConvertToNullableDateTime (reader.ReadContentAsString());

                                if (reader.MoveToAttribute("MessageLoadEndTime"))
                                    this.MessageLoadEndTime = ValueTypeHelper.ConvertToNullableDateTime(reader.ReadContentAsString());

                                if (reader.MoveToAttribute("ProcessStartTime"))
                                    this.ProcessStartTime = ValueTypeHelper.ConvertToNullableDateTime(reader.ReadContentAsString());

                                if (reader.MoveToAttribute("ProcessEndTime"))
                                    this.ProcessEndTime = ValueTypeHelper.ConvertToNullableDateTime(reader.ReadContentAsString());

                                if (reader.MoveToAttribute("Action"))
                                {
                                    ObjectAction action = ObjectAction.Read;
                                    if (Enum.TryParse(reader.ReadContentAsString(), out action))
                                    {
                                        this.Action = action;
                                    }
                                }

                                if (reader.MoveToAttribute("IntegrationType"))
                                {
                                    IntegrationType iType = Core.IntegrationType.Unknown;
                                    if (Enum.TryParse(reader.ReadContentAsString(), out iType))
                                    {
                                        this.IntegrationType = iType;
                                    }
                                }
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Context")
                        {
                            String contextXml = reader.ReadInnerXml();
                            if (!String.IsNullOrEmpty(contextXml))
                            {
                                this.Context = contextXml;
                            }
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
    }
}
