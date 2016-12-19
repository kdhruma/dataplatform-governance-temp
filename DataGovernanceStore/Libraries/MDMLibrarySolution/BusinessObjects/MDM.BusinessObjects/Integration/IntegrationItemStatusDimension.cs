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
    /// </summary>
    [DataContract]
    public class IntegrationItemStatusDimension : ObjectBase, IIntegrationItemStatusDimension
    {
        #region Fields

        /// <summary>
        /// Indicates dimension type name.
        /// </summary>
        private String _integrationItemDimensionTypeName = String.Empty;

        /// <summary>
        /// Indicates dimension value.
        /// </summary>
        private String _integrationItemDimensionValue = String.Empty;

        /// <summary>
        /// Indicates place holder for temporary id assignment, used as reference while processing.
        /// </summary>
        private Int32 _referenceId = -1;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public IntegrationItemStatusDimension()
        {
        }

        /// <summary>
        /// Xml constructor
        /// </summary>
        /// <param name="valuesAsXml">Values of IntegrationItemDimensionStatus in xml format</param>
        public IntegrationItemStatusDimension(String valuesAsXml)
        {
            LoadIntegrationItemStatusDimension(valuesAsXml);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the integration item dimension type identifier.
        /// </summary>
        /// <value>
        /// The integration item dimension type identifier.
        /// </value>
        [DataMember]
        public String IntegrationItemDimensionTypeName
        {
            get { return _integrationItemDimensionTypeName; }
            set { _integrationItemDimensionTypeName = value; }
        }

        /// <summary>
        /// Indicates dimension value.
        /// </summary>
        [DataMember]
        public String IntegrationItemDimensionValue
        {
            get { return _integrationItemDimensionValue; }
            set { _integrationItemDimensionValue = value; }
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

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Represents IntegrationItemDimensionStatus in Xml format
        /// </summary>
        /// <returns></returns>
        public String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);
            String xml = String.Empty;

            xmlWriter.WriteStartElement("IntegrationItemStatusDimension");

            xmlWriter.WriteAttributeString("IntegrationItemDimensionTypeName", this.IntegrationItemDimensionTypeName);
            xmlWriter.WriteAttributeString("IntegrationItemDimensionValue", this.IntegrationItemDimensionValue);
            xmlWriter.WriteAttributeString("ReferenceId", this.ReferenceId.ToString());

            //IntegrationItemDimensionStatus end
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
        public IIntegrationItemStatusDimension Clone()
        {
            IntegrationItemStatusDimension clonedIntegrationItemStatusDimension = new IntegrationItemStatusDimension();

            clonedIntegrationItemStatusDimension.IntegrationItemDimensionTypeName = this.IntegrationItemDimensionTypeName;
            clonedIntegrationItemStatusDimension.IntegrationItemDimensionValue = this.IntegrationItemDimensionValue;
            clonedIntegrationItemStatusDimension.ReferenceId = this.ReferenceId;

            return clonedIntegrationItemStatusDimension;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initialize IntegrationItemDimensionStatus from xml.
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadIntegrationItemStatusDimension(string valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "IntegrationItemStatusDimension" && reader.IsStartElement())
                        {
                            #region Read ConnectorProfile Attributes

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("IntegrationItemDimensionTypeName"))
                                {
                                    this.IntegrationItemDimensionTypeName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("IntegrationItemDimensionValue"))
                                {
                                    this.IntegrationItemDimensionValue = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ReferenceId"))
                                {
                                    this.ReferenceId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), -1);
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