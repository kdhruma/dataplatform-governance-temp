using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Integration
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Class for Integration error log
    /// </summary>
    [DataContract]
    public class IntegrationErrorLog : ObjectBase, IIntegrationErrorLog, IIntegrationItem
    {
        #region Fields

        /// <summary>
        /// Indicates Id of IntegrationErrorLog table
        /// </summary>
        private Int64 _id = -1;

        /// <summary>
        /// Indicates Id of queue item for which error occurred. It may contains Id of IntegrationActivityLog/QualifyingQueue/OutboundQueue
        /// </summary>
        private Int64 _integrationId = -1;

        /// <summary>
        /// Indicates type of integration - is it inbound or outbound
        /// </summary>
        private IntegrationType _integrationType = IntegrationType.Unknown;

        /// <summary>
        /// Indicates message type for integration message.
        /// </summary>
        private Int16 _integrationMessageTypeId = -1;

        /// <summary>
        /// Indicates Id of connector for which error occurred.
        /// </summary>
        private Int16 _connectorId = -1;

        /// <summary>
        /// Indicates the (error/info/warning) message.
        /// </summary>
        private String _messageText = String.Empty;

        /// <summary>
        /// Indicates if message is Error or information or warning.
        /// </summary>
        private OperationResultType _messageType = OperationResultType.Ignore;

        /// <summary>
        /// Indicates the name of processor from which error occurred.
        /// </summary>
        private CoreDataProcessorList _coreDataProcessorName = CoreDataProcessorList.Unknown;

        /// <summary>
        /// Indicates action to be performed on current object.
        /// </summary>
        private ObjectAction _action = ObjectAction.Read;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public IntegrationErrorLog()
            : base()
        {
        }

        /// <summary>
        /// Xml constructor
        /// </summary>
        /// <param name="valuesAsXml">IntegrationErrorLog xml having values</param>
        public IntegrationErrorLog(String valuesAsXml)
        {
            LoadIntegrationErrorLogFromXml(valuesAsXml);
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Indicates Id of IntegrationErrorLog table
        /// </summary>
        [DataMember]
        public Int64 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Indicates Id of queue item for which error occurred. It may contains Id of IntegrationActivityLog/QualifyingQueue/OutboundQueue
        /// </summary>
        [DataMember]
        public Int64 IntegrationId
        {
            get { return _integrationId; }
            set { _integrationId = value; }
        }

        /// <summary>
        /// Indicates type of integration - is it inbound or outbound
        /// </summary>
        [DataMember]
        public IntegrationType IntegrationType
        {
            get { return _integrationType; }
            set { _integrationType = value; }
        }

        /// <summary>
        /// Indicates message type for integration message.
        /// </summary>
        [DataMember]
        public Int16 IntegrationMessageTypeId
        {
            get { return _integrationMessageTypeId; }
            set { _integrationMessageTypeId = value; }
        }

        /// <summary>
        /// Indicates Id of connector for which error occurred.
        /// </summary>
        [DataMember]
        public Int16 ConnectorId
        {
            get { return _connectorId; }
            set { _connectorId = value; }
        }

        /// <summary>
        /// Indicates the (error/info/warning) message.
        /// </summary>
        [DataMember]
        public String MessageText
        {
            get { return _messageText; }
            set { _messageText = value; }
        }

        /// <summary>
        /// Indicates if message is Error or information or warning.
        /// </summary>
        [DataMember]
        public OperationResultType MessageType
        {
            get { return _messageType; }
            set { _messageType = value; }
        }

        /// <summary>
        /// Indicates the name of processor from which error occurred.
        /// </summary>
        [DataMember]
        public CoreDataProcessorList CoreDataProcessorName
        {
            get { return _coreDataProcessorName; }
            set { _coreDataProcessorName = value; }
        }

        /// <summary>
        /// Indicates action to be performed on current object.
        /// </summary>
        [DataMember]
        public ObjectAction Action
        {
            get { return _action; }
            set { _action = value; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents IntegrationErrorLog in Xml format
        /// </summary>
        public String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);
            String xml = String.Empty;

            xmlWriter.WriteStartElement("IntegrationErrorLog");

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("Action", this.Action.ToString());
            xmlWriter.WriteAttributeString("ConnectorId", this.ConnectorId.ToString());
            xmlWriter.WriteAttributeString("CoreDataProcessorName", this.CoreDataProcessorName.ToString());
            xmlWriter.WriteAttributeString("IntegrationId", this.IntegrationId.ToString());
            xmlWriter.WriteAttributeString("IntegrationMessageTypeId", this.IntegrationMessageTypeId.ToString());
            xmlWriter.WriteAttributeString("IntegrationType", this.IntegrationType.ToString());
            xmlWriter.WriteAttributeString("MessageType", this.MessageType.ToString());

            xmlWriter.WriteStartElement("MessageText");
            if (!String.IsNullOrWhiteSpace(this.MessageText))
            {
                xmlWriter.WriteCData(this.MessageText);
            }

            //MessageText end
            xmlWriter.WriteEndElement();

            //IntegrationErrorLog end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Clone the integration message and create a new integration message object
        /// </summary>
        /// <returns>New integration message object having same value as current one.</returns>
        public IIntegrationErrorLog Clone()
        {
            IntegrationErrorLog clonedErrorLog = new IntegrationErrorLog();
            clonedErrorLog.Id = this.Id;
            clonedErrorLog.Action=this.Action;
            clonedErrorLog.ConnectorId=this.ConnectorId;
            clonedErrorLog.CoreDataProcessorName = this.CoreDataProcessorName;
            clonedErrorLog.IntegrationId = this.IntegrationId;
            clonedErrorLog.IntegrationMessageTypeId=IntegrationMessageTypeId;
            clonedErrorLog.IntegrationType=this.IntegrationType;
            //clonedErrorLog.MDMObjectId=this.MDMObjectId;
            //clonedErrorLog.MDMObjectTypeId=this.MDMObjectTypeId;
            clonedErrorLog.MessageType = this.MessageType;
            clonedErrorLog.MessageText = this.MessageText;

            return clonedErrorLog;
        }

        #endregion Methods

        #region Private Methods

        private void LoadIntegrationErrorLogFromXml(string valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "IntegrationErrorLog" && reader.IsStartElement())
                        {
                            #region Read IntegrationErrorLog Attributes

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                    this.Id = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), -1);

                                if (reader.MoveToAttribute("ConnectorId"))
                                    this.ConnectorId = ValueTypeHelper.Int16TryParse(reader.ReadContentAsString(), -1);

                                if (reader.MoveToAttribute("IntegrationId"))
                                    this.IntegrationId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), -1);

                                if (reader.MoveToAttribute("IntegrationMessageTypeId"))
                                    this.IntegrationMessageTypeId = ValueTypeHelper.Int16TryParse(reader.ReadContentAsString(), -1);

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

                                if (reader.MoveToAttribute("MessageType"))
                                {
                                    OperationResultType mType = OperationResultType.Ignore;
                                    Enum.TryParse<OperationResultType>(reader.ReadContentAsString(), out mType);
                                    this.MessageType = mType;
                                }

                                if (reader.MoveToAttribute("CoreDataProcessorName"))
                                {
                                    CoreDataProcessorList processorName = CoreDataProcessorList.Unknown;
                                    Enum.TryParse<CoreDataProcessorList>(reader.ReadContentAsString(), out processorName);
                                    this.CoreDataProcessorName = processorName;
                                }
                                    
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "MessageText")
                        {
                            String messageText = reader.ReadInnerXml();
                            if (!String.IsNullOrEmpty(messageText))
                            {
                                this.MessageText = messageText;
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
