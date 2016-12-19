using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Integration
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Indicates status for an IntegrationItem
    /// </summary>
    [DataContract]
    [KnownType(typeof(IntegrationItemStatusDimensionCollection))]
    public class IntegrationItemStatus : ObjectBase, IIntegrationItemStatus
    {
        #region Fields

        /// <summary>
        /// Indicates Id of MDMobject participating as IntegrationItem
        /// </summary>
        private Int64 _mdmObjectId = -1;

        /// <summary>
        /// Indicates id of Object type
        /// </summary>
        private String _mdmObjectTypeName = String.Empty;

        /// <summary>
        /// Indicates Id of entity/message in external system.
        /// </summary>
        private String _externalId = String.Empty;

        /// <summary>
        /// Indicates  name of object type.
        /// </summary>
        private String _externalObjectTypeName = String.Empty;

        /// <summary>
        /// Indicates name of connector for which status is being recorded.
        /// </summary>
        private String _connectorName = String.Empty;

        /// <summary>
        /// Indicates collection of dimension type and value for an item status
        /// </summary>
        private IntegrationItemStatusDimensionCollection _integrationItemStatusDimensionCollection = new IntegrationItemStatusDimensionCollection();

        /// <summary>
        /// Indicates place holder for temporary id assignment, used as reference while processing.
        /// </summary>
        private Int32 _referenceId = -1;

        /// <summary>
        /// Indicates status for a dimension
        /// </summary>
        private String _status = String.Empty;

        /// <summary>
        /// Indicates some additional comments for the dimension value
        /// </summary>
        private String _comments = String.Empty;

        /// <summary>
        /// Indicates type of status. Is it error/info/warning
        /// </summary>
        private OperationResultType _statusType = OperationResultType.Information;

        /// <summary>
        /// Indicates if indicating status is for message in external system or it is indicating status for message in our system. 
        /// </summary>
        private Boolean _isExternalStatus = false;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public IntegrationItemStatus()
            : base()
        {
        }

        /// <summary>
        /// Xml constructor
        /// </summary>
        /// <param name="valuesAsXml">Values in xml format for IntegrationItemStatus</param>
        public IntegrationItemStatus(String valuesAsXml)
        {
            LoadIntegrationItemStatus(valuesAsXml);
        }


        #endregion Constructor

        #region Properties

        /// <summary>
        /// Indicates Id of MDMobject participating as IntegrationItem
        /// </summary>
        [DataMember]
        public Int64 MDMObjectId
        {
            get { return _mdmObjectId; }
            set { _mdmObjectId = value; }
        }

        /// <summary>
        /// Indicates id of Object type
        /// </summary>
        [DataMember]
        public String MDMObjectTypeName
        {
            get { return _mdmObjectTypeName; }
            set { _mdmObjectTypeName = value; }
        }

        /// <summary>
        /// Indicates Id of entity/message in external system.
        /// </summary>
        [DataMember]
        public String ExternalId
        {
            get { return _externalId; }
            set { _externalId = value; }
        }

        /// <summary>
        /// Indicates name of external object.
        /// </summary>
        [DataMember]
        public String ExternalObjectTypeName
        {
            get { return _externalObjectTypeName; }
            set { _externalObjectTypeName = value; }
        }

        /// <summary>
        /// Indicates name of connector for which status is being recorded.
        /// </summary>
        [DataMember]
        public String ConnectorName
        {
            get { return _connectorName; }
            set { _connectorName = value; }
        }

        /// <summary>
        /// Indicates collection of dimension type and value for an item status
        /// </summary>
        [DataMember]
        public IntegrationItemStatusDimensionCollection StatusDimensionCollection
        {
            get { return _integrationItemStatusDimensionCollection; }
            set { _integrationItemStatusDimensionCollection = value; }
        }

        /// <summary>
        /// Indicates place holder for temporary id assignment, used as reference while processing.
        /// </summary>
        [DataMember]
        public Int32 ReferenceId
        {
            get { return _referenceId; }
            set { _referenceId = value; }
        }

        /// <summary>
        /// Indicates status value.
        /// </summary>
        [DataMember]
        public String Status
        {
            get { return _status; }
            set { _status = value; }
        }

        /// <summary>
        /// Indicates some additional comments for the dimension value
        /// </summary>
        [DataMember]
        public String Comments
        {
            get { return _comments; }
            set { _comments = value; }
        }

        /// <summary>
        /// Indicates type of status. Is it error/info/warning
        /// </summary>
        [DataMember]
        public OperationResultType StatusType
        {
            get { return _statusType; }
            set { _statusType = value; }
        }

        /// <summary>
        /// Indicates if indicating status is for message in external system or it is indicating status for message in our system. 
        /// </summary>
        [DataMember]
        public Boolean IsExternalStatus
        {
            get { return _isExternalStatus; }
            set { _isExternalStatus = value; }
        }

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Represents IntegrationItemStatus in Xml format
        /// </summary>
        /// <returns></returns>
        public String ToXml()
        {

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);
            String xml = String.Empty;

            xmlWriter.WriteStartElement("IntegrationItemStatus");

            xmlWriter.WriteAttributeString("MDMObjectId", this.MDMObjectId.ToString());
            xmlWriter.WriteAttributeString("MDMObjectTypeName", this.MDMObjectTypeName);
            xmlWriter.WriteAttributeString("ExternalId", this.ExternalId);
            xmlWriter.WriteAttributeString("ExternalObjectTypeName", this.ExternalObjectTypeName);
            xmlWriter.WriteAttributeString("ConnectorName", this.ConnectorName);
            xmlWriter.WriteAttributeString("ReferenceId", this.ReferenceId.ToString());
            xmlWriter.WriteAttributeString("Status", this.Status);
            xmlWriter.WriteAttributeString("Comments", this.Comments);
            xmlWriter.WriteAttributeString("StatusType", this.StatusType.ToString());
            xmlWriter.WriteAttributeString("IsExternalStatus", this.IsExternalStatus.ToString());

            if (this.StatusDimensionCollection != null)
            {
                xmlWriter.WriteRaw(this.StatusDimensionCollection.ToXml());
            }

            //IntegrationItemStatus end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Return new object with the values same as current one
        /// </summary>
        /// <returns>New object with same values as current one</returns>
        public IIntegrationItemStatus Clone()
        {
            IntegrationItemStatus clonedItemStatus = new IntegrationItemStatus();
            clonedItemStatus.MDMObjectId = this.MDMObjectId;
            clonedItemStatus.MDMObjectTypeName = this.MDMObjectTypeName;
            clonedItemStatus.ExternalId = this.ExternalId;
            clonedItemStatus.ExternalObjectTypeName = this.ExternalObjectTypeName;
            clonedItemStatus.ConnectorName = this.ConnectorName;
            clonedItemStatus.ReferenceId = this.ReferenceId;
            clonedItemStatus.Status = this.Status;
            clonedItemStatus.Comments = this.Comments;
            clonedItemStatus.StatusType = this.StatusType;
            clonedItemStatus.IsExternalStatus = this.IsExternalStatus;


            if (this.StatusDimensionCollection != null)
            {
                clonedItemStatus.StatusDimensionCollection = (IntegrationItemStatusDimensionCollection)this.StatusDimensionCollection.Clone();
            }
            return clonedItemStatus;
        }

        #region IIntegrationItemStatus Members

        /// <summary>
        /// Sets the integration item status dimension for item status
        /// </summary>
        /// <param name="iIntegrationItemStatusDimensions">IntegrationItemStatusDimensions Interface</param>
        public void SetStatusDimensions(IIntegrationItemStatusDimensionCollection iIntegrationItemStatusDimensions)
        {
            IntegrationItemStatusDimensionCollection integrationItemStatusDimensions = (IntegrationItemStatusDimensionCollection)iIntegrationItemStatusDimensions;

            this._integrationItemStatusDimensionCollection = integrationItemStatusDimensions;
        }

        /// <summary>
        /// Gets the attributes belonging to the Entity
        /// </summary>
        /// <returns>Attribute Collection Interface</returns>
        /// <exception cref="NullReferenceException">Attributes for entity is null. There are no attributes to search in</exception>
        public IIntegrationItemStatusDimensionCollection GetStatusDimensions()
        {
            if (this._integrationItemStatusDimensionCollection == null)
            {
                throw new NullReferenceException("Integration item status dimensions are not available.");
            }

            return (IIntegrationItemStatusDimensionCollection)this._integrationItemStatusDimensionCollection;
        }

        #endregion

        #endregion

        #region Private Methods

        private void LoadIntegrationItemStatus(string valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "IntegrationItemStatus" && reader.IsStartElement())
                        {
                            #region Read ConnectorProfile Attributes

                            if (reader.HasAttributes)
                            {

                                if (reader.MoveToAttribute("MDMObjectId"))
                                    this.MDMObjectId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), -1);

                                if (reader.MoveToAttribute("MDMObjectTypeName"))
                                    this.MDMObjectTypeName = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("ExternalId"))
                                    this.ExternalId = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("ExternalObjectTypeName"))
                                    this.ExternalObjectTypeName = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("ConnectorName"))
                                    this.ConnectorName = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("ReferenceId"))
                                    this.ReferenceId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), -1);

                                if (reader.MoveToAttribute("Status"))
                                    this.Status = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("Comments"))
                                    this.Comments = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("StatusType"))
                                {
                                    OperationResultType sType = OperationResultType.Information;
                                    Enum.TryParse<OperationResultType>(reader.ReadContentAsString(), out sType);
                                    this.StatusType = sType;
                                }

                                if (reader.MoveToAttribute("IsExternalStatus"))
                                {
                                    this.IsExternalStatus = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }

                                reader.Read();
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "IntegrationItemStatusDimensionCollection" && reader.IsStartElement())
                        {
                            String xml = reader.ReadOuterXml();
                            if (!String.IsNullOrWhiteSpace(xml))
                            {
                                IntegrationItemStatusDimensionCollection integrationItemStatusDimensions = new IntegrationItemStatusDimensionCollection(xml);
                                if (integrationItemStatusDimensions != null)
                                {
                                    this.StatusDimensionCollection = integrationItemStatusDimensions;
                                }
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

        #endregion

        #endregion
    }
}