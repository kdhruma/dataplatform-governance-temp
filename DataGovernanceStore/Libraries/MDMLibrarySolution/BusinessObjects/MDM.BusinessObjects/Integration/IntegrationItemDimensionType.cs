using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Integration
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Class indicating a dimension type for a status of an item for a connector.
    /// </summary>
    [DataContract]
    public class IntegrationItemDimensionType : MDMObject, IIntegrationItemDimensionType
    {
        #region Fields

        /// <summary>
        /// Indicates Id of IntegrationItemDimensionType
        /// </summary>
        private Int32 _id = -1;

        /// <summary>
        /// Indicates id of connector for the dimension
        /// </summary>
        private Int16 _connectorId = -1;

        /// <summary>
        /// Indicates name of connector for the dimension
        /// </summary>
        private String _connectorName = String.Empty;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public IntegrationItemDimensionType()
            : base()
        {
        }

        /// <summary>
        /// Xml constructor
        /// </summary>
        public IntegrationItemDimensionType(String valuesAsXml)
            : base()
        {
            LoadIntegrationItemDimensionType(valuesAsXml);
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Indicates Id of IntegrationItemDimensionType
        /// </summary>
        [DataMember]
        new public Int32 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Indicates id of connector for the status
        /// </summary>
        [DataMember]
        public Int16 ConnectorId
        {
            get { return _connectorId; }
            set { _connectorId = value; }
        }

        /// <summary>
        /// Indicates name of connector for the status
        /// </summary>
        [DataMember]
        public String ConnectorName
        {
            get { return _connectorName; }
            set { _connectorName = value; }
        }

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Represent IntegrationItemDimensionType in Xml format
        /// </summary>
        /// <returns>IntegrationItemDimensionType object in Xml format</returns>
        public override String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);
            String xml = String.Empty;

            xmlWriter.WriteStartElement("IntegrationItemDimensionType");

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("LongName", this.LongName);
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("ConnectorId", this.ConnectorId.ToString());
            xmlWriter.WriteAttributeString("ConnectorName", this.ConnectorName);
            xmlWriter.WriteAttributeString("Action", this.Action.ToString());

            //IntegrationItemDimensionType end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Create new instance of IIntegrationItemDimensionType with same values as current one
        /// </summary>
        /// <returns></returns>
        public IIntegrationItemDimensionType Clone()
        {
            IntegrationItemDimensionType clonedItem = new IntegrationItemDimensionType();
            clonedItem.Action = this.Action;
            clonedItem.ConnectorId = this.ConnectorId;
            clonedItem.ConnectorName = this.ConnectorName;
            clonedItem.ExtendedProperties = this.ExtendedProperties;
            clonedItem.AuditRefId = this.AuditRefId;
            clonedItem.Id = this.Id;
            clonedItem.Locale = this.Locale;
            clonedItem.LongName = this.LongName;
            clonedItem.Name = this.Name;
            clonedItem.PermissionSet = this.PermissionSet;
            clonedItem.ProgramName = this.ProgramName;
            clonedItem.ReferenceId = this.ReferenceId;
            clonedItem.UserName = this.UserName;

            return clonedItem;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subsetIntegrationItemDimensionType"></param>
        /// <param name="compareIds"></param>
        /// <returns></returns>
        public Boolean IsSuperSetOf(IntegrationItemDimensionType subsetIntegrationItemDimensionType, Boolean compareIds = false)
        {
            if (base.IsSuperSetOf(subsetIntegrationItemDimensionType, compareIds))
            {
                if (compareIds == true)
                {
                    if (this.Id != subsetIntegrationItemDimensionType.Id)
                        return false;

                    if (this.ConnectorId != subsetIntegrationItemDimensionType.ConnectorId)
                        return false;
                }

                if (this.ConnectorName != subsetIntegrationItemDimensionType.ConnectorName)
                    return false;
            }
            else
            {
                return false;
            }
            return true;
        }


        #endregion

        #region Private Methods

        private void LoadIntegrationItemDimensionType(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "IntegrationItemDimensionType" && reader.IsStartElement())
                        {
                            #region Read ConnectorProfile Attributes

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                    this.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), -1);

                                if (reader.MoveToAttribute("Name"))
                                    this.Name = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("LongName"))
                                    this.LongName = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("Action"))
                                {
                                    ObjectAction action = ObjectAction.Read;
                                    if (Enum.TryParse(reader.ReadContentAsString(), out action))
                                    {
                                        this.Action = action;
                                    }
                                }

                                if (reader.MoveToAttribute("ConnectorId"))
                                    this.ConnectorId = ValueTypeHelper.Int16TryParse(reader.ReadContentAsString(), -1);

                                if (reader.MoveToAttribute("ConnectorName"))
                                    this.ConnectorName = reader.ReadContentAsString();

                                reader.Read();
                            }

                            #endregion
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