using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Integration
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Class for Integration message header. It contains context information for message.
    /// </summary>
    [DataContract]
    public class IntegrationMessageHeader : ObjectBase, IIntegrationMessageHeader
    {
        #region Fields

        /// <summary>
        /// Indicates object id for which this activity log has been created
        /// </summary>
        private Int64 _mdmObjectId = -1;

        /// <summary>
        /// Indicates id of Object type
        /// </summary>
        private Int16 _mdmObjectTypeId = -1;

        /// <summary>
        /// Indicates Long name MDMObject type
        /// </summary>
        private String _mdmObjectTypeLongName = String.Empty;

        /// <summary>
        /// Indicates type of message
        /// </summary>
        private Int16 _integrationMessageTypeId = -1;

        /// <summary>
        /// Indicates Long name of message type
        /// </summary>
        private String _messageTypeLongName = String.Empty;

        /// <summary>
        /// Indicates Id of connector for which this message is sent
        /// </summary>
        private Int16 _connectorId = -1;

        /// <summary>
        /// Indicates Long name of connector for which this message is sent
        /// </summary>
        private String _connectorName = String.Empty;

        /// <summary>
        /// Indicates context for message. Typically some context information for MDMObjectId
        /// </summary>
        private String _context = String.Empty;

        /// <summary>
        /// Indicates the type of integration. Indicates this activity log is for inbound processing or outbound processing
        /// </summary>
        private IntegrationType _integrationType = IntegrationType.Unknown;

        /// <summary>
        /// Indicates the collection of Integration Message Ids the current message is aggregated as.
        /// </summary>
        private Collection<Int64> _parentMessageIds = new Collection<Int64>();

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public IntegrationMessageHeader()
            : base()
        {
        }

        /// <summary>
        /// Xml Constructor
        /// </summary>
        public IntegrationMessageHeader(String valuesAsXml)
            : base()
        {
            LoadMessageHeader(valuesAsXml);
        }

        #endregion Constructor

        #region Properties

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
        /// Indicates Long name MDMObject type
        /// </summary>
        [DataMember]
        public String MDMObjectTypeLongName
        {
            get
            {
                return _mdmObjectTypeLongName;
            }
            set
            {
                _mdmObjectTypeLongName = value;
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
                return _messageTypeLongName;
            }
            set
            {
                _messageTypeLongName = value;
            }
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
        public String ConnectorName
        {
            get
            {
                return _connectorName;
            }
            set
            {
                _connectorName = value;
            }
        }

        /// <summary>
        /// Indicates context for message. Typically some context information for MDMObjectId
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
        /// Indicates the type of integration. Indicates this message is for inbound processing or outbound processing
        /// </summary>
        [DataMember]
        public IntegrationType IntegrationType
        {
            get { return _integrationType; }
            set { _integrationType = value; }
        }
        
        /// <summary>
        /// Indicates the collection of Integration Message Ids the current message is aggregated as.
        /// </summary>
        [DataMember]
        public Collection<Int64> ParentMessageIds
        {
            get
            {
                return _parentMessageIds;
            }
            set
            {
                _parentMessageIds = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents IntegrationMessageHeader in Xml format
        /// </summary>
        public String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);
            String xml = String.Empty;

            xmlWriter.WriteStartElement("IntegrationMessageHeader");

            xmlWriter.WriteAttributeString("MDMObjectId", this.MDMObjectId.ToString());
            xmlWriter.WriteAttributeString("MDMObjectTypeId", this.MDMObjectTypeId.ToString());
            xmlWriter.WriteAttributeString("MDMObjectTypeLongName", this.MDMObjectTypeLongName);
            xmlWriter.WriteAttributeString("IntegrationMessageTypeId", this.IntegrationMessageTypeId.ToString());
            xmlWriter.WriteAttributeString("IntegrationMessageTypeLongName", this.IntegrationMessageTypeLongName);
            xmlWriter.WriteAttributeString("ConnectorId", this.ConnectorId.ToString());
            xmlWriter.WriteAttributeString("ConnectorName", this.ConnectorName.ToString());
            xmlWriter.WriteAttributeString("IntegrationType", this.IntegrationType.ToString());

            String parentMessageIdCollectionString = String.Empty;

            if (this.ParentMessageIds != null && this.ParentMessageIds.Count > 0)
            {
                parentMessageIdCollectionString = ValueTypeHelper.JoinCollection(this.ParentMessageIds, ",");
            }
            xmlWriter.WriteAttributeString("ParentMessageIds", parentMessageIdCollectionString);

            //Write context data
            xmlWriter.WriteStartElement("Context");
            xmlWriter.WriteCData(this.Context);
            xmlWriter.WriteEndElement();
            
            //IntegrationMessageHeader end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        #endregion Methods

        #region Private Methods

        private void LoadMessageHeader(string valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "IntegrationMessageHeader" && reader.IsStartElement())
                        {
                            #region Read Integration Message Attributes

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("MDMObjectId"))
                                    this.MDMObjectId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), -1);

                                if (reader.MoveToAttribute("MDMObjectTypeId"))
                                    this.MDMObjectTypeId = ValueTypeHelper.Int16TryParse(reader.ReadContentAsString(), -1);

                                if (reader.MoveToAttribute("MDMObjectTypeLongName"))
                                    this.MDMObjectTypeLongName = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("IntegrationMessageTypeId"))
                                    this.IntegrationMessageTypeId = ValueTypeHelper.Int16TryParse(reader.ReadContentAsString(), -1);

                                if (reader.MoveToAttribute("IntegrationMessageTypeLongName"))
                                    this.IntegrationMessageTypeLongName = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("ConnectorId"))
                                    this.ConnectorId = ValueTypeHelper.Int16TryParse(reader.ReadContentAsString(), -1);

                                if (reader.MoveToAttribute("ConnectorName"))
                                    this.ConnectorName = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("IntegrationType"))
                                {
                                    IntegrationType iType = Core.IntegrationType.Unknown;
                                    Enum.TryParse<IntegrationType>(reader.ReadContentAsString(), out iType);
                                    this.IntegrationType = iType;
                                }
                                if (reader.MoveToAttribute("ParentMessageIds"))
                                {
                                    Collection<Int64> parentIds = ValueTypeHelper.SplitStringToLongCollection(reader.ReadContentAsString(), ',');
                                    if (parentIds != null && parentIds.Count > 0)
                                    {
                                        this.ParentMessageIds = parentIds;
                                    }
                                }
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Context")
                        {
                            if (!reader.IsEmptyElement)
                            {
                                this.Context = reader.ReadElementContentAsString();
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
