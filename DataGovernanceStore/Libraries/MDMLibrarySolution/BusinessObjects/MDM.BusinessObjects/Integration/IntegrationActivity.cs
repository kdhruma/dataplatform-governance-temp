using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Integration
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Indicates integration activity that happened in integration system (inbound action triggered or outbound action triggered)
    /// </summary>
    [DataContract]
    public class IntegrationActivity : ObjectBase, IIntegrationActivity
    {
        #region Fields

        /// <summary>
        /// Indicates Id of integration activity log
        /// </summary>
        private Int64 _id = -1;

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
        /// Indicates the type of integration. Indicates this activity log is for inbound processing or outbound processing
        /// </summary>
        private IntegrationType _integrationType = IntegrationType.Unknown;

        /// <summary>
        /// Field denoting blob of data for the activity. It can contain file data as well.
        /// </summary>
        private Byte[] _data = null;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public IntegrationActivity()
            : base()
        {
        }

        /// <summary>
        /// Xml constructor
        /// </summary>
        /// <param name="valuesAsXml">IntegrationActivity xml having </param>
        public IntegrationActivity(String valuesAsXml)
        {
            LoadIntegrationActivityFromXml(valuesAsXml);
        }

        /// <summary>
        /// Initialize IntegrationActivity object from IntegrationActivityLog
        /// </summary>
        /// <param name="integrationActivityLog">Indicates integration activity log for integration activity</param>
        public IntegrationActivity(IntegrationActivityLog integrationActivityLog)
        {
            if (integrationActivityLog != null)
            {
                this.ConnectorId = integrationActivityLog.ConnectorId;
                this.ConnectorLongName = integrationActivityLog.ConnectorLongName;
                this.ConnectorName = integrationActivityLog.ConnectorName;
                this.Context = integrationActivityLog.Context;
                this.Id = integrationActivityLog.Id;
                this.IntegrationMessageTypeId = integrationActivityLog.IntegrationMessageTypeId;
                this.IntegrationMessageTypeLongName = integrationActivityLog.IntegrationMessageTypeLongName;
                this.IntegrationMessageTypeName = integrationActivityLog.IntegrationMessageTypeName;
                this.IntegrationType = integrationActivityLog.IntegrationType;
                this.MDMObjectId = integrationActivityLog.MDMObjectId;
                this.MDMObjectTypeId = integrationActivityLog.MDMObjectTypeId;
                this.MDMObjectTypeName = integrationActivityLog.MDMObjectTypeName;
            }
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
        /// Indicates the type of integration. Indicates this activity log is for inbound processing or outbound processing
        /// </summary>
        [DataMember]
        public IntegrationType IntegrationType
        {
            get { return _integrationType; }
            set { _integrationType = value; }
        }

        /// <summary>
        /// Field denoting blob of data for the activity. It can contain file data as well.
        /// </summary>
        [DataMember]
        public Byte[] Data
        {
          get { return _data; }
          set { _data = value; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents IntegrationActivity in Xml format
        /// </summary>
        public String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);
            String xml = String.Empty;

            xmlWriter.WriteStartElement("IntegrationActivity");

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("MDMObjectId", this.MDMObjectId.ToString());
            xmlWriter.WriteAttributeString("MDMObjectTypeId", this.MDMObjectTypeId.ToString());
            xmlWriter.WriteAttributeString("MDMObjectTypeName", this.MDMObjectTypeName);
            xmlWriter.WriteAttributeString("IntegrationMessageTypeId", this.IntegrationMessageTypeId.ToString());
            xmlWriter.WriteAttributeString("IntegrationMessageTypeLongName", this.IntegrationMessageTypeLongName);
            xmlWriter.WriteAttributeString("IntegrationMessageTypeName", this.IntegrationMessageTypeName);
            xmlWriter.WriteAttributeString("ConnectorId", this.ConnectorId.ToString());
            xmlWriter.WriteAttributeString("ConnectorLongName", this.ConnectorLongName);
            xmlWriter.WriteAttributeString("ConnectorName", this.ConnectorName);
            xmlWriter.WriteAttributeString("IntegrationType", this.IntegrationType.ToString());

            xmlWriter.WriteStartElement("Context");
            if (!String.IsNullOrWhiteSpace(this.Context))
            {
                xmlWriter.WriteRaw(this.Context);
            }

            //ContextXml end
            xmlWriter.WriteEndElement();

            //IntegrationActivity end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Clone the integration activity and create a new integration activity
        /// </summary>
        /// <returns>New integration activity having same value as current one.</returns>
        public IIntegrationActivity Clone()
        {
            IntegrationActivity integrationActivity = new IntegrationActivity();
            integrationActivity.ConnectorId = this.ConnectorId;
            integrationActivity.ConnectorLongName = this.ConnectorLongName;
            integrationActivity.Context = this.Context;
            integrationActivity.Id = this.Id;
            integrationActivity.IntegrationMessageTypeId = this.IntegrationMessageTypeId;
            integrationActivity.IntegrationMessageTypeLongName = this.IntegrationMessageTypeLongName;
            integrationActivity.IntegrationType = this.IntegrationType;
            integrationActivity.MDMObjectId = this.MDMObjectId;
            integrationActivity.MDMObjectTypeId = this.MDMObjectTypeId;
            integrationActivity.MDMObjectTypeName = this.MDMObjectTypeName;
            integrationActivity.Data = this.Data;

            return integrationActivity;
        }

        #endregion Methods

        #region Private Methods

        /// <summary>
        /// Load integration activity object from xml
        /// </summary>
        /// <param name="valuesAsXml">XML having xml values</param>
        private void LoadIntegrationActivityFromXml(string valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "IntegrationActivity" && reader.IsStartElement())
                        {
                            #region Read IntegrationActivity Attributes

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                    this.Id = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), -1);

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

                                if (reader.MoveToAttribute("IntegrationMessageTypeName "))
                                    this.IntegrationMessageTypeName = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("ConnectorId"))
                                    this.ConnectorId = ValueTypeHelper.Int16TryParse(reader.ReadContentAsString(), -1);

                                if (reader.MoveToAttribute("ConnectorLongName"))
                                    this.ConnectorLongName = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("ConnectorName"))
                                    this.ConnectorName = reader.ReadContentAsString();

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
