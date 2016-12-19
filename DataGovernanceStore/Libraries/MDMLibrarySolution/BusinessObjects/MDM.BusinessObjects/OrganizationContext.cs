using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;
    using MDM.Core;

    /// <summary>
    /// Specifies Interface which indicates what all information is to be loaded in Organization object
    /// </summary>
    public class OrganizationContext: ObjectBase, IOrganizationContext
    {
        #region Fields

        private Boolean _loadAttributes;

        #endregion

        #region Constructors
        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public OrganizationContext()
            : base()
        {
           
        }

        /// <summary>
        /// 
        /// </summary>
        public OrganizationContext(Boolean loadAttributes)
            : base()
        {
            this.LoadAttributes = loadAttributes;
        }

        
        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">XML having xml values</param>
        public OrganizationContext(String valuesAsXml)
        {
            LoadOrganizationContext(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting a boolean value to determine whether to load attributes or not
        /// </summary>
        [DataMember]
        public Boolean LoadAttributes
        {
            get
            {
                return _loadAttributes;
            }
            set
            {
                _loadAttributes = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Convert to xml
        /// </summary>
        /// <returns></returns>
        public String ToXml()
        {
            String xml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);
           
            //Attribute node start
            xmlWriter.WriteStartElement("OrganizationContext");

            xmlWriter.WriteAttributeString("LoadAttributes", this.LoadAttributes.ToString());

            //OrganizationContext end node
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();
            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        private void LoadOrganizationContext(String valuesAsXml)
        {
            #region Sample Xml

            #endregion Sample Xml

            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "OrganizationContext")
                    {
                        #region Read OrganizationContext Properties

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("LoadAttributes"))
                            {
                                this.LoadAttributes = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                            }

                        }

                        #endregion
                    }
                    else
                    {
                        //Keep on reading the xml until we reach expected node.
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

        #endregion
    }
}
