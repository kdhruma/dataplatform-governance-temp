using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Integration
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Indicates status for an IntegrationItem.
    /// This class is used for internal purpose. It contains Ids for all the required fields. 
    /// So for external purpose when Id's will not be known use IntegrationItemStatus object.
    /// </summary>
    [DataContract]
    public class IntegrationItemStatusInternal : IntegrationItemStatus, IIntegrationItemStatusInternal
    {
        #region Fields

        /// <summary>
        /// Indicates Id of IntegrationItemStatus
        /// </summary>
        private Int64 _id = -1;

        /// <summary>
        /// Indicates id of Object type
        /// </summary>
        private Int16 _mdmObjectTypeId = -1;

        /// <summary>
        /// Indicates  ID of object type.
        /// </summary>
        private Int16 _externalObjectTypeId = -1;

        /// <summary>
        /// Indicates Id of connector for which status is being recorded.
        /// </summary>
        private Int16 _connectorId = -1;

        /// <summary>
        /// Indicates long name of connector for which status is being recorded.
        /// </summary>
        private String _connectorLongName = String.Empty;

        /// <summary>
        /// Indicates collection of internal status dimension collection.
        /// </summary>
        private IntegrationItemStatusDimensionInternalCollection _statusDimensionInternalCollection = null;

        /// <summary>
        /// Indicates time stamp indicating when status was updated.
        /// </summary>
        private DateTime _auditTimeStamp = DateTime.MinValue;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public IntegrationItemStatusInternal()
        {
        }

        /// <summary>
        /// Xml constructor
        /// </summary>
        /// <param name="valuesAsXml">Values of IntegrationItemStatusInternal in xml format</param>
        public IntegrationItemStatusInternal(String valuesAsXml)
        {
            LoadIntegrationItemStatusInternal(valuesAsXml);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Indicates Id of IntegrationItemStatus
        /// </summary>
        [DataMember]
        public Int64 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Indicates id of Object type
        /// </summary>
        [DataMember]
        public Int16 MDMObjectTypeId
        {
            get { return _mdmObjectTypeId; }
            set { _mdmObjectTypeId = value; }
        }

        /// <summary>
        /// Indicates Id of external object.
        /// </summary>
        [DataMember]
        public Int16 ExternalObjectTypeId
        {
            get { return _externalObjectTypeId; }
            set { _externalObjectTypeId=value ; }
        }

        /// <summary>
        /// Indicates Id of connector for which status is being recorded.
        /// </summary>
        [DataMember]
        public Int16 ConnectorId
        {
            get { return _connectorId; }
            set { _connectorId = value; }
        }

        /// <summary>
        /// Indicates long name of connector for which status is being recorded.
        /// </summary>
        [DataMember]
        public String ConnectorLongName
        {
            get { return _connectorLongName; }
            set { _connectorLongName = value; }
        }

        /// <summary>
        /// Indicates collection of internal status dimension collection.
        /// </summary>
        [DataMember]
        public IntegrationItemStatusDimensionInternalCollection StatusDimensionInternalCollection
        {
            get { return _statusDimensionInternalCollection; }
            set { _statusDimensionInternalCollection = value; }
        }

        /// <summary>
        /// Indicates time stamp indicating when status was updated.
        /// </summary>
        [DataMember]
        public DateTime AuditTimeStamp
        {
            get { return _auditTimeStamp; }
            set { _auditTimeStamp = value; }
        }

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Represents IntegrationItemStatusInternalCollection in Xml format
        /// </summary>
        /// <returns></returns>
        public new String ToXml()
        {

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);
            String xml = String.Empty;

            xmlWriter.WriteStartElement("IntegrationItemStatusInternal");

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("MDMObjectId", this.MDMObjectId.ToString());
            xmlWriter.WriteAttributeString("MDMObjectTypeId", this.MDMObjectTypeId.ToString());
            xmlWriter.WriteAttributeString("MDMObjectTypeName", this.MDMObjectTypeName.ToString());
            xmlWriter.WriteAttributeString("ExternalId", this.ExternalId);
            xmlWriter.WriteAttributeString("ExternalObjectTypeId", this.ExternalObjectTypeId.ToString());
            xmlWriter.WriteAttributeString("ExternalObjectTypeName", this.ExternalObjectTypeName); 
            xmlWriter.WriteAttributeString("ConnectorId", this.ConnectorId.ToString());
            xmlWriter.WriteAttributeString("ConnectorName", this.ConnectorName);
            xmlWriter.WriteAttributeString("ConnectorLongName", this.ConnectorLongName);
            xmlWriter.WriteAttributeString("ReferenceId", this.ReferenceId.ToString());
            xmlWriter.WriteAttributeString("Status", this.Status);
            xmlWriter.WriteAttributeString("Comments", this.Comments);
            xmlWriter.WriteAttributeString("StatusType", this.StatusType.ToString());
            xmlWriter.WriteAttributeString("IsExternalStatus", this.IsExternalStatus.ToString());
            xmlWriter.WriteAttributeString("AuditTimeStamp", this.AuditTimeStamp.ToString());

            if (this.StatusDimensionInternalCollection != null)
            {
                xmlWriter.WriteRaw(StatusDimensionInternalCollection.ToXml());
            }

            //IntegrationItemStatusInternal end
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
        public new IIntegrationItemStatusInternal Clone()
        {
            IntegrationItemStatusInternal ClonedItemStatusInternal = new IntegrationItemStatusInternal();
            ClonedItemStatusInternal.Id = this.Id;
            ClonedItemStatusInternal.MDMObjectId = this.MDMObjectId;
            ClonedItemStatusInternal.MDMObjectTypeId = this.MDMObjectTypeId;
            ClonedItemStatusInternal.MDMObjectTypeName = this.MDMObjectTypeName;
            ClonedItemStatusInternal.ExternalId = this.ExternalId;
            ClonedItemStatusInternal.ExternalObjectTypeId = this.ExternalObjectTypeId;
            ClonedItemStatusInternal.ExternalObjectTypeName = this.ExternalObjectTypeName;
            ClonedItemStatusInternal.ConnectorId = this.ConnectorId;
            ClonedItemStatusInternal.ConnectorName = this.ConnectorName;
            ClonedItemStatusInternal.ConnectorLongName = this.ConnectorLongName;
            ClonedItemStatusInternal.ReferenceId = this.ReferenceId;
            ClonedItemStatusInternal.Status = this.Status;
            ClonedItemStatusInternal.Comments = this.Comments;
            ClonedItemStatusInternal.StatusType = this.StatusType;
            ClonedItemStatusInternal.IsExternalStatus = this.IsExternalStatus;
            ClonedItemStatusInternal.AuditTimeStamp = this.AuditTimeStamp;
            ClonedItemStatusInternal.StatusDimensionInternalCollection = this.StatusDimensionInternalCollection;

            return (IIntegrationItemStatusInternal)ClonedItemStatusInternal;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initialize IntegrationItemStatusInternal from xml.
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadIntegrationItemStatusInternal(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "IntegrationItemStatusInternal" && reader.IsStartElement())
                        {
                            #region Read IntegrationItemStatusInternalCollection Attributes

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                    this.Id = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), -1);

                                if (reader.MoveToAttribute("MDMObjectId"))
                                    this.MDMObjectId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), -1);

                                if (reader.MoveToAttribute("MDMObjectTypeName"))
                                    this.MDMObjectTypeName = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("ExternalId"))
                                    this.ExternalId = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("ExternalObjectTypeId"))
                                    this.ExternalObjectTypeId = ValueTypeHelper.Int16TryParse(reader.ReadContentAsString(), -1);

                                if (reader.MoveToAttribute("ExternalObjectTypeName"))
                                    this.ExternalObjectTypeName = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("ConnectorId"))
                                    this.ConnectorId = ValueTypeHelper.Int16TryParse(reader.ReadContentAsString(), -1);

                                if (reader.MoveToAttribute("ConnectorName"))
                                    this.ConnectorName = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("ConnectorLongName"))
                                    this.ConnectorLongName = reader.ReadContentAsString();

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

                                if (reader.MoveToAttribute("AuditTimeStamp"))
                                {
                                    String val = reader.ReadContentAsString();
                                    if (!String.IsNullOrWhiteSpace(val))
                                    {
                                        this.AuditTimeStamp = ValueTypeHelper.ConvertToDateTime(reader.ReadContentAsString());
                                    }
                                } 
                                reader.Read();
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "IntegrationItemStatusDimensionInternalCollection" && reader.IsStartElement())
                        {
                            String xml = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(xml))
                            {
                                IntegrationItemStatusDimensionInternalCollection integrationItemStatusInternalDimensions = new IntegrationItemStatusDimensionInternalCollection(xml);
                                if (integrationItemStatusInternalDimensions != null)
                                {
                                    this.StatusDimensionInternalCollection = integrationItemStatusInternalDimensions;
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