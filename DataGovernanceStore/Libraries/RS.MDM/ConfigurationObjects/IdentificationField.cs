using System;
using System.ComponentModel;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using MDM.Core;

namespace RS.MDM.ConfigurationObjects
{
    using Configuration;

    /// <summary>
    /// Provides ID Field configuration for Entity Identifier config
    /// </summary>
    [XmlRoot("IdentificationField")]
    [Serializable()]
    public sealed class IdentificationField : Object
    {
        #region Fields

        private EntityIdentificationFieldType _identifierType = EntityIdentificationFieldType.Unknown;
        private String _identifiername = String.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// Represents the change type
        /// </summary>
        [Category("Properties")]
        [XmlAttribute("IdentifierType")]
        [Description("Represents attribute identifier field type")]
        [TrackChanges()]
        public EntityIdentificationFieldType IdentifierType
        {
            get
            {
                return this._identifierType;
            }
            set
            {
                this._identifierType = value;
            }
        }

        /// <summary>
        /// Represents the Identifier name
        /// </summary>
        [Category("Properties")]
        [XmlAttribute("IdentifierName")]
        [Description("Represents identifier field name")]
        [TrackChanges()]
        public String IdentifierName
        {
            get
            {
                return this._identifiername;
            }
            set
            {
                this._identifiername = value;
            }
        }


        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the IDField class.
        /// </summary>
        public IdentificationField()
        {
        }

        /// <summary>
        /// Parameterized constructor with values as xml representation of object.
        /// </summary>
        public IdentificationField(String valuesAsXml)
        {

        }

        #endregion

        #region Serialization & Deserialization

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        [System.Runtime.Serialization.OnDeserialized()]
        private void OnDeserialized(System.Runtime.Serialization.StreamingContext context)
        {
            this.SetParent();
        }

        #region Overrides

        /// <summary>
        /// Get a tree node that represents an object and its aggregated children
        /// </summary>
        /// <returns>A Tree Node that represents an object and its aggregated children</returns>
        public override System.Windows.Forms.TreeNode GetTreeNode()
        {
            System.Windows.Forms.TreeNode treeNode = base.GetTreeNode();

            return treeNode;
        }

        #endregion

        /// <summary>
        /// Get XML representation of the Identifier object
        /// </summary>
        /// <returns>XML representation of Identifier</returns>
        public override String ToXml()
        {
            String parameterXml;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //IDField node start
            xmlWriter.WriteStartElement("IdentificationField");

            xmlWriter.WriteAttributeString("IdentifierType", IdentifierType.ToString());
            xmlWriter.WriteAttributeString("IdentifierName", IdentifierName);

            //IDField node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            parameterXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return parameterXml;
        }

        /// <summary>
        /// Load the Identifier object from the input xml
        /// </summary>
        /// <param name="valueAsXml">Identifier object representation as xml</param>
        public void LoadParameter(String valueAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valueAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valueAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "IdentificationField")
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("IdentificationField"))
                                {
                                    EntityIdentificationFieldType identifierType;
                                    Enum.TryParse(reader.ReadContentAsString(), out identifierType);
                                    this.IdentifierType = identifierType;
                                }
                            }
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
        }

        #endregion
    }
}
