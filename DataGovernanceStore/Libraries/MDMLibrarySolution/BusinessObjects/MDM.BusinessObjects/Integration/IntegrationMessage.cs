using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Integration
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Class for Integration message.
    /// </summary>
    [DataContract]
    public class IntegrationMessage : ObjectBase, IIntegrationMessage
    {
        #region Fields

        /// <summary>
        /// Indicates Id of integration message
        /// </summary>
        private Int64 _id = -1;

        /// <summary>
        /// Indicates action to be performed on the object
        /// </summary>
        private ObjectAction _action = ObjectAction.Read;

        /// <summary>
        /// Indicates Integration Message for integration system
        /// </summary>
        private IntegrationMessageHeader _messageHeader = new IntegrationMessageHeader();

        /// <summary>
        /// Indicates body of message
        /// </summary>
        private String _messageBody = String.Empty;

        /// <summary>
        /// Indicates Reference field for the object
        /// </summary>
        private String _referenceId = String.Empty;

        /// <summary>
        /// Indicates type of object participating in the message
        /// </summary>
        private Int16 _mdmObjectTypeId = -1;

        /// <summary>
        /// Indicates the Integration Message Id of the message it is aggregated into by aggregation logic
        /// </summary>
        private Int64 _aggregatedMessageId = -1;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public IntegrationMessage()
            : base()
        {
        }

        /// <summary>
        /// Xml constructor
        /// </summary>
        /// <param name="valuesAsXml">Value in xml format</param>
        public IntegrationMessage(String valuesAsXml)
        {
            LoadIntegrationMessage(valuesAsXml);
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Indicates Id of integration message
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
        /// Indicates Integration Message for integration system
        /// </summary>
        [DataMember]
        public IntegrationMessageHeader MessageHeader
        {
            get
            {
                return _messageHeader;
            }
            set
            {
                _messageHeader = value;
            }
        }

        /// <summary>
        /// Indicates body of message
        /// </summary>
        [DataMember]
        public String MessageBody
        {
            get
            {
                return _messageBody;
            }
            set
            {
                _messageBody = value;
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
        /// Indicates type of object participating in the message
        /// </summary>
        [DataMember]
        public Int16 MDMObjectTypeId
        {
            get { return _mdmObjectTypeId; }
            set { _mdmObjectTypeId = value; }
        }

        /// <summary>
        /// Indicates the Integration Message Id of the message it is aggregated into by aggregation logic
        /// </summary>
        [DataMember]
        public Int64 AggregatedMessageId
        {
            get { return _aggregatedMessageId; }
            set { _aggregatedMessageId = value; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents IntegrationMessage in Xml format
        /// </summary>
        public String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);
            String xml = String.Empty;

            xmlWriter.WriteStartElement("IntegrationMessage");

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("ReferenceId", this.ReferenceId);
            xmlWriter.WriteAttributeString("Action", this.Action.ToString());
            xmlWriter.WriteAttributeString("AggregatedMessageId", this.AggregatedMessageId.ToString());

            if (this.MessageHeader != null)
            {
                xmlWriter.WriteRaw(this.MessageHeader.ToXml());
            }

            xmlWriter.WriteStartElement("IntegrationMessageBody");
            if (this.MessageHeader != null)
            {
                xmlWriter.WriteRaw(this.MessageBody);
            }

            //IntegrationMessageBody end
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
        /// Clone the message object and return new instance with the same values.
        /// </summary>
        /// <returns>New cloned IntegrationMessage</returns>
        public IIntegrationMessage Clone()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Indicates Integration Message for integration system
        /// </summary>
        public IIntegrationMessageHeader GetMessageHeader()
        {
            return this.MessageHeader;
        }

        #endregion Methods

        #region Private Methods
        /// <summary>
        /// Initialize MessageHeader and MessageBody from xml.
        /// </summary>
        /// <param name="valuesAsXml">Indicates Xml having values which we want to populate in current object</param>
        private void LoadIntegrationMessage(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "IntegrationMessage" && reader.IsStartElement())
                        {
                            #region Read Integration Message Attributes

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                    this.Id = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), -1);

                                if (reader.MoveToAttribute("ReferenceId"))
                                    this.ReferenceId = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("MDMObjectTypeId"))
                                    this.MDMObjectTypeId = ValueTypeHelper.Int16TryParse(reader.ReadContentAsString(), -1);

                                if (reader.MoveToAttribute("Action"))
                                {
                                    ObjectAction action = ObjectAction.Read;
                                    if (Enum.TryParse(reader.ReadContentAsString(), out action))
                                    {
                                        this.Action = action;
                                    }
                                }

                                if (reader.MoveToAttribute("AggregatedMessageId"))
                                    this.AggregatedMessageId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), -1);

                                reader.Read();
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "IntegrationMessageHeader")
                        {
                            String headerXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(headerXml))
                            {
                                IntegrationMessageHeader header = new IntegrationMessageHeader(headerXml);
                                if (header != null)
                                {
                                    this.MessageHeader = header;
                                }
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "IntegrationMessageBody")
                        {
                            String bodyXml = reader.ReadInnerXml();
                            if (!String.IsNullOrEmpty(bodyXml))
                            {
                                this.MessageBody = bodyXml;
                            }
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
        }

        #endregion Private Methods

    }
}
