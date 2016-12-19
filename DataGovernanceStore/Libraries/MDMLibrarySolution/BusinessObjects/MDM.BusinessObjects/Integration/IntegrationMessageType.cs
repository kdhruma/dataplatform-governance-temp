using System;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;

namespace MDM.BusinessObjects.Integration
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Class for type of integration message.
    /// </summary>
    [DataContract]
    public class IntegrationMessageType : ObjectBase, IIntegrationMessageType
    {
        #region Fields

        /// <summary>
        /// Indicates Id of type of integration message
        /// </summary>
        private Int16 _id = -1;

        /// <summary>
        /// Indicates action to be performed on the object
        /// </summary>
        private ObjectAction _action = ObjectAction.Read;

        /// <summary>
        /// Indicates Short name for the message type
        /// </summary>
        private String _name = String.Empty;

        /// <summary>
        /// Indicates Long name for the message type. This is typically fully qualified class name
        /// </summary>
        private String _longName = String.Empty;

        /// <summary>
        /// Indicates Id of connector for which this message will be used
        /// </summary>
        private Int16 _connectorId = -1;

        /// <summary>
        /// Indicates name of connector for which this message will be used
        /// </summary>
        private String _connectorLongName = String.Empty;

        /// <summary>
        /// Indicates Reference field for the object
        /// </summary>
        private String _referenceId = String.Empty;

        /// <summary>
        /// Indicates weightage for integration message
        /// </summary>
        private Int32 _weightage = -1;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public IntegrationMessageType()
            : base()
        {
        }

        /// <summary>
        /// Xml constructor
        /// </summary>
        /// <param name="valuesAsXml">Value in xml format</param>
        public IntegrationMessageType(String valuesAsXml)
        {
            LoadIntegrationMessageType(valuesAsXml);
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Indicates Id of type of integration message
        /// </summary>
        [DataMember]
        public Int16 Id
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
        /// Indicates Short name for the message type
        /// </summary>
        [DataMember]
        public String Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        /// <summary>
        /// Indicates Long name for the message type. This is typically fully qualified class name
        /// </summary>
        [DataMember]
        public String LongName
        {
            get
            {
                return _longName;
            }
            set
            {
                _longName = value;
            }
        }

        /// <summary>
        /// Indicates Id of connector for which this message will be used
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
        /// Indicates name of connector for which this message will be used
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
        /// Indicates Reference field for the object
        /// </summary>
        [DataMember]
        public String ReferenceId
        {
            get { return _referenceId; }
            set { _referenceId = value; }
        }

        /// <summary>
        /// Indicates weightage for integration message
        /// </summary>
        [DataMember]
        public Int32 Weightage
        {
            get { return _weightage; }
            set { _weightage = value; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents IntegrationMessageType in Xml format
        /// </summary>
        public String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);
            String xml = String.Empty;

            xmlWriter.WriteStartElement("IntegrationMessageType");

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("LongName", this.LongName);
            xmlWriter.WriteAttributeString("ConnectorId", this.ConnectorId.ToString());
            xmlWriter.WriteAttributeString("ConnectorLongName", this.ConnectorLongName);
            xmlWriter.WriteAttributeString("ReferenceId", this.ReferenceId);
            xmlWriter.WriteAttributeString("Action", this.Action.ToString());
            xmlWriter.WriteAttributeString("Weightage", this.Weightage.ToString());
            //IntegrationMessageType end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Create new instance of IntegrationMessageType with same values.
        /// </summary>
        /// <returns><see cref="IIntegrationMessageType"/></returns>
        public IIntegrationMessageType Clone()
        {
            IntegrationMessageType clonedMessageType = new IntegrationMessageType();
            clonedMessageType.Action = this.Action;
            clonedMessageType.ConnectorId = this.ConnectorId;
            clonedMessageType.ConnectorLongName = this.ConnectorLongName;
            clonedMessageType.Id = this.Id;
            clonedMessageType.LongName = this.LongName;
            clonedMessageType.Name = this.Name;
            clonedMessageType.ReferenceId = this.ReferenceId;

            return clonedMessageType;
        }

        private void LoadIntegrationMessageType(string valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "IntegrationMessageType" && reader.IsStartElement())
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                    this.Id = ValueTypeHelper.Int16TryParse(reader.ReadContentAsString(), -1);
                                if (reader.MoveToAttribute("Name"))
                                    this.Name = reader.ReadContentAsString();
                                if (reader.MoveToAttribute("LongName"))
                                    this.LongName = reader.ReadContentAsString();
                                if (reader.MoveToAttribute("ConnectorId"))
                                    this.ConnectorId = ValueTypeHelper.Int16TryParse(reader.ReadContentAsString(), -1);
                                if (reader.MoveToAttribute("ConnectorLongName"))
                                    this.ConnectorLongName = reader.ReadContentAsString();
                                if (reader.MoveToAttribute("ReferenceId"))
                                    this.ReferenceId = reader.ReadContentAsString();
                                if (reader.MoveToAttribute("Action"))
                                {
                                    ObjectAction action = ObjectAction.Read;
                                    if (Enum.TryParse(reader.ReadContentAsString(), out action))
                                    {
                                        this.Action = action;
                                    }
                                }
                                if (reader.MoveToAttribute("Weightage"))
                                    this.Weightage = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), -1);
                            }
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
        }

        #endregion Methods
    }
}
