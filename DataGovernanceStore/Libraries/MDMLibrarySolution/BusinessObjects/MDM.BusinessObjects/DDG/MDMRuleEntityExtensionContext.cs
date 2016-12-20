using ProtoBuf;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;
   
    /// <summary>
    /// Represents the class that contain MdmRule entity extension context details
    /// </summary>
    [DataContract]
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public class MDMRuleEntityExtensionContext : MDMObject, IMDMRuleEntityExtensionContext
    {
          #region Fields

        /// <summary>
        /// Field denotes  the container name of the extension context.
        /// </summary>
        private String _containerName = String.Empty;

        /// <summary>
        /// Field denotes  the category path of the entity extension context.
        /// </summary>
        private String _categoryPath = String.Empty;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property the container name of the extension context.
        /// </summary>
        [DataMember]
        public String ContainerName
        {
            get { return _containerName; }
            set { _containerName = value; }
        }

        /// <summary>
        /// Property denotes the category path of the entity extension context.
        /// </summary>
        [DataMember]
        public String CategoryPath
        {
            get { return _categoryPath; }
            set { _categoryPath = value; }
        }
        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the MdmRule entity extension context
        /// </summary>
        public MDMRuleEntityExtensionContext()
            : base()
        {
        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">XML having xml values</param>
        public MDMRuleEntityExtensionContext(String valuesAsXml)
        {
            LoadEntityExtensionContext(valuesAsXml);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Initialize current object with Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having values for current MdmRule entity extension context 
        /// </param>
        public void LoadEntityExtensionContext(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                using (XmlTextReader reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMRuleEntityExtensionContext")
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("ContainerName"))
                                {
                                    this._containerName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("CategoryPath"))
                                {
                                    this._categoryPath = reader.ReadContentAsString();
                                }

                                reader.Read();
                            }
                        }
                        else
                        {
                            reader.Read();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get Xml representation of MDMRule entity extension context object
        /// </summary>
        /// <returns>Xml representation of MDMRule entity extension context object</returns>
        public new String ToXml()
        {
            String outputXML = String.Empty;
            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter))
                {
                    xmlWriter.WriteStartElement("MDMRuleEntityExtensionContext");

                    xmlWriter.WriteAttributeString("ContainerName", ContainerName);
                    xmlWriter.WriteAttributeString("CategoryPath", CategoryPath);

                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();

                    //get the actual XML
                    outputXML = stringWriter.ToString();
                }
            }
            return outputXML;
        }

        #endregion Methods
    }
}
