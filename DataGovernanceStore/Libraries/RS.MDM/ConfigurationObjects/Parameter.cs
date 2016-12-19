using System;
using System.IO;
using System.Xml;
using System.Data;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace RS.MDM.ConfigurationObjects
{
    using RS.MDM.Validations;
    using RS.MDM.Configuration;

    /// <summary>
    /// Represents a Parameter Object
    /// </summary>
    [XmlRoot("Param")]
    [Serializable()]
    public sealed class Parameter : Object
    {
        #region Fields

        /// <summary>
        /// Represents the value of the parameter
        /// </summary>
        private String _value = String.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// Represents the value of the parameter
        /// </summary>
        [XmlAttribute("Value")]
        [Category("Properties")]
        [Description("Represents the value of the parameter")]
        [TrackChanges()]
        public String Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the Parameter class.
        /// </summary>
        public Parameter()
            : base()
        {
        }

        #endregion

        #region Methods

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

        #endregion

        #region Overrides

        /// <summary>
        /// Get a tree node that represents an object and its aggregated children
        /// </summary>
        /// <returns>A Tree Node that represents an object and its aggregated children</returns>
        public override System.Windows.Forms.TreeNode GetTreeNode()
        {
            System.Windows.Forms.TreeNode _treeNode = base.GetTreeNode();

            if (this.PropertyChanges.ObjectStatus == RS.MDM.Configuration.InheritedObjectStatus.None)
            {
                _treeNode.ImageKey = "UIColumn";
                _treeNode.SelectedImageKey = _treeNode.ImageKey;
            }

            return _treeNode;
        }

        /// <summary>
        /// Get XML representation of the object
        /// </summary>
        /// <returns>XML representation of Parameter</returns>
        public override String ToXml()
        {
            String parameterXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Parameter node start
            xmlWriter.WriteStartElement("Param");

            xmlWriter.WriteAttributeString("ClassName", this.ClassName);
            xmlWriter.WriteAttributeString("AssemblyName", this.AssemblyName);
            xmlWriter.WriteAttributeString("Name", this.Name);

            //Write Parameter value as value of node. Not as attribute.
            xmlWriter.WriteCData(this.Value);

            //Param node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            parameterXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return parameterXml;
        }

        /// <summary>
        /// Load the Parameter object from the input xml
        /// </summary>
        /// <param name="parameterAsXml">Parameter as xml</param>
        public void LoadParameter(String parameterAsXml)
        {
            if (!String.IsNullOrWhiteSpace(parameterAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(parameterAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Param")
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("AssemblyName"))
                                {
                                    this.AssemblyName = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("ClassName"))
                                {
                                    this.ClassName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Name"))
                                {
                                    this.Name = reader.ReadContentAsString();
                                }
                                if (reader.MoveToElement())
                                {
                                    this.Value = reader.ReadElementContentAsString();
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
