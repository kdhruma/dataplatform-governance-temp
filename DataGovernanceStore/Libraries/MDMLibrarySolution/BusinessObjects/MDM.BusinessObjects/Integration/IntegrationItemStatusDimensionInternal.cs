using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Integration
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents dimension values for an item's status.
    /// This class is used for internal purpose with all Ids'. For external purpose when Id's are not known and want to update the values, use <see cref="IntegrationItemStatusDimension"/>
    /// </summary>
    [DataContract]
    public class IntegrationItemStatusDimensionInternal : IntegrationItemStatusDimension, IIntegrationItemStatusDimensionInternal
    {
        #region Fields

        /// <summary>
        /// Indicates Id of IntegrationItemStatusDimension
        /// </summary>
        private Int64 _id = -1;

        /// <summary>
        /// Indicates dimension type Id.
        /// </summary>
        private Int32 _integrationItemDimensionTypeId = -1;

        /// <summary>
        /// Indicates dimension type long name.
        /// </summary>
        private String _integrationItemDimensionTypeLongName = String.Empty;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public IntegrationItemStatusDimensionInternal()
        {
        }

        /// <summary>
        /// Xml constructor
        /// </summary>
        /// <param name="valuesAsXml">Values of IntegrationItemDimensionStatus in xml format</param>
        public IntegrationItemStatusDimensionInternal(String valuesAsXml)
        {
            LoadIntegrationItemDimensionStatusInternal(valuesAsXml);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Indicates Id of IntegrationItemStatusDimension
        /// </summary>
        public Int64 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Indicates dimension type long name.
        /// </summary>
        [DataMember]
        public Int32 IntegrationItemDimensionTypeId
        {
            get { return _integrationItemDimensionTypeId; }
            set { _integrationItemDimensionTypeId = value; }
        }

        /// <summary>
        /// Indicates dimension type long name.
        /// </summary>
        [DataMember]
        public String IntegrationItemDimensionTypeLongName
        {
            get { return _integrationItemDimensionTypeLongName; }
            set { _integrationItemDimensionTypeLongName = value; }
        }
        
        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Represents IntegrationItemDimensionStatusInternalCollection in Xml format
        /// </summary>
        /// <returns>Object in xml format</returns>
        public new String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);
            String xml = String.Empty;

            xmlWriter.WriteStartElement("IntegrationItemDimensionStatusInternal");

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("IntegrationItemDimensionTypeId", this.IntegrationItemDimensionTypeId.ToString());
            xmlWriter.WriteAttributeString("IntegrationItemDimensionTypeName", this.IntegrationItemDimensionTypeName);
            xmlWriter.WriteAttributeString("IntegrationItemDimensionTypeLongName", this.IntegrationItemDimensionTypeLongName);
            xmlWriter.WriteAttributeString("IntegrationItemDimensionValue", this.IntegrationItemDimensionValue);
            xmlWriter.WriteAttributeString("ReferenceId", this.ReferenceId.ToString());

            //IntegrationItemDimensionStatusInternal end
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
        public new IIntegrationItemStatusDimensionInternal Clone()
        {
            IntegrationItemStatusDimensionInternal clonedItemDimensionStatusInternal = new IntegrationItemStatusDimensionInternal();

            clonedItemDimensionStatusInternal.Id = this.Id;
            clonedItemDimensionStatusInternal.IntegrationItemDimensionTypeId = this.IntegrationItemDimensionTypeId;
            clonedItemDimensionStatusInternal.IntegrationItemDimensionTypeName = this.IntegrationItemDimensionTypeName;
            clonedItemDimensionStatusInternal.IntegrationItemDimensionTypeLongName = this.IntegrationItemDimensionTypeLongName;
            clonedItemDimensionStatusInternal.IntegrationItemDimensionValue = this.IntegrationItemDimensionValue;
            clonedItemDimensionStatusInternal.ReferenceId = this.ReferenceId;

            return clonedItemDimensionStatusInternal;
        }

        #endregion

        #region Private Methods
        
        /// <summary>
        /// Initialize IntegrationItemDimensionStatusInternal from xml.
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadIntegrationItemDimensionStatusInternal(string valuesAsXml)
        {

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "IntegrationItemDimensionStatusInternal" && reader.IsStartElement())
                        {
                            #region Read IntegrationItemDimensionStatusInternal Attributes

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), -1);
                                }

                                if (reader.MoveToAttribute("IntegrationItemDimensionTypeId"))
                                {
                                    this.IntegrationItemDimensionTypeId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(),-1);
                                }

                                if (reader.MoveToAttribute("IntegrationItemDimensionTypeName"))
                                {
                                    this.IntegrationItemDimensionTypeName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("IntegrationItemDimensionTypeLongName"))
                                {
                                    this.IntegrationItemDimensionTypeLongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("IntegrationItemDimensionValue"))
                                {
                                    this.IntegrationItemDimensionValue = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ReferenceId"))
                                {
                                    this.ReferenceId = ValueTypeHelper.Int32TryParse (reader.ReadContentAsString(),-1);
                                }

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