using System.Xml.Serialization;
using System;
using RS.MDM.Configuration;
using System.ComponentModel;
using System.IO;
using System.Xml;
using MDM.Core;

namespace RS.MDM.ConfigurationObjects
{
    /// <summary>
    /// Specifies object type for application context configuration item
    /// </summary>
    public enum ObjectTypeEnum
    {
        Unknown = 0,
        Organization = 1,
        Container = 2,
        Category = 3,
        Attribute = 4,
        EntityType= 5,
        RelationshipType = 6,
        SecurityRole = 7,
        SecurityUser = 8,
        Locale = 9
    }

    /// <summary>
    /// Provides configuration for application context configuration item
    /// </summary>
    [XmlRoot("ApplicationContextConfigurationItem")]
    [Serializable()]
    public class ApplicationContextConfigurationItem : Object
    {
        #region Fields

        /// <summary>
        /// field for the title of the ApplicationContextConfigurationItem
        /// </summary>
        private ObjectTypeEnum _objectType = ObjectTypeEnum.Unknown;

        /// <summary>
        /// field to denote if the ApplicationContextConfigurationItem is visible
        /// </summary>
        private Boolean _visible = true;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates the title of the ApplicationContextConfigurationItem
        /// </summary>
        [XmlAttribute("ObjectType")]
        [Description("ApplicationContextConfigurationItem object type.")]
        [Category("ApplicationContextConfigurationItem")]
        [TrackChanges()]
        public ObjectTypeEnum ObjectType
        {
            get
            {
                return this._objectType;
            }
            set
            {
                this._objectType = value;
            }
        }

        /// <summary>
        /// Indicates if the ApplicationContextConfigurationItem is visible
        /// </summary>
        [XmlAttribute("Visible")]
        [Description("ApplicationContextConfigurationItem is visible or not")]
        [Category("ApplicationContextConfigurationItem")]
        [TrackChanges()]
        public Boolean Visible
        {
            get
            {
                return _visible;
            }
            set
            {
                _visible = value;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public ApplicationContextConfigurationItem()
            : base()
        {

        }

        public ApplicationContextConfigurationItem(String valuesAsXml)
        {
            LoadParameter(valuesAsXml);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get XML representation of the ApplicationContextConfigurationItem object
        /// </summary>
        /// <returns>XML representation of ApplicationContextConfigurationItem</returns>
        public override String ToXml()
        {
            String parameterXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //ApplicationContextConfigurationItem node start
            xmlWriter.WriteStartElement("ApplicationContextConfigurationItem");

            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("Description", this.Description);
            xmlWriter.WriteAttributeString("Visible", this.Visible.ToString());
            xmlWriter.WriteAttributeString("ObjectType", this.ObjectType.ToString());

            //ApplicationContextConfigurationItem node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            parameterXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return parameterXml;
        }

        /// <summary>
        /// Load the ApplicationContextConfigurationItem object from the input xml
        /// </summary>
        /// <param name="valueAsXml">ApplicationContextConfigurationItem object representation as xml</param>
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ApplicationContextConfigurationItem")
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Name"))
                                {
                                    this.Name = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("Description"))
                                {
                                    this.Description = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("Visible"))
                                {
                                    this.Visible = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this.Visible);
                                }
                                if (reader.MoveToAttribute("ObjectType"))
                                {
                                    ObjectTypeEnum objectType = ObjectTypeEnum.Unknown;
                                    Enum.TryParse(reader.ReadContentAsString(), out objectType);
                                    this.ObjectType = objectType;
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
                _treeNode.ImageKey = "NavigationPane";
                _treeNode.SelectedImageKey = _treeNode.ImageKey;
            }

            return _treeNode;
        }

        #endregion
    }
}