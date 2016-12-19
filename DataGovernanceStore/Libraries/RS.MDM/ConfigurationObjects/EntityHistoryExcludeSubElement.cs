using MDM.Core;
using System;
using System.ComponentModel;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace RS.MDM.ConfigurationObjects
{
    using RS.MDM.Configuration;

    /// <summary>
    /// Provides system default configuration for EntityHistory Exclude sub element list config
    /// </summary>
    [XmlRoot("EntityHistoryExcludeSubElement")]
    [Serializable()]
    public sealed class EntityHistoryExcludeSubElement : Object
    {
        #region LocaleConfigXml

        //<LocaleConfig xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" Id="-1" 
        //              UniqueIdentifier="292c728e-099e-4680-a63a-9307f5d55228" Name="LocaleConfig" Description="" Tag="" 
        //              ClassName="RS.MDM.Web.UI.Configuration.LocaleConfig" AssemblyName="PCWebControls.dll" 
        //              InheritedParentUId="" SystemDataLocale="en_WW" SystemUILocale="en_WW" AllowableUILocales ="en_WW,de_DE,fr_FR" 
        //              ModelDisplayLocaleType="DataLocale" DataFormattingLocaleType="DataLocale" >
        //<PropertyChanges ObjectStatus="None" />
        //</LocaleConfig>

        #endregion

        #region Fields

        private EntityHistoryExcludeChangeType _entityHistoryExcludeChangeType = EntityHistoryExcludeChangeType.Unknown;

        private String _ids = String.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// Represents the change type
        /// </summary>
        [Category("Properties")]
        [XmlAttribute("ChangeType")]
        [Description("Represents entity history exclude change type")]
        [TrackChanges()]
        public EntityHistoryExcludeChangeType ChangeType
        {
            get
            {
                return this._entityHistoryExcludeChangeType;
            }
            set
            {
                this._entityHistoryExcludeChangeType = value;
            }
        }

        /// <summary>
        /// Represents the change type
        /// </summary>
        [Category("Properties")]
        [XmlAttribute("Ids")]
        [Description("Represents list of ids which are excluded for particular entity history exclude change type")]
        [TrackChanges()]
        public String Ids
        {
            get
            {
                return this._ids;
            }
            set
            {
                this._ids = value;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the EntityHistoryExcludeSubElement class.
        /// </summary>
        public EntityHistoryExcludeSubElement()
            : base()
        {
        }

        /// <summary>
        /// Parameterized constructor with values as xml representation of object.
        /// </summary>
        public EntityHistoryExcludeSubElement(String ValuesAsXml)
            : base()
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
            System.Windows.Forms.TreeNode _treeNode = base.GetTreeNode();

            if (this.PropertyChanges.ObjectStatus == RS.MDM.Configuration.InheritedObjectStatus.None)
            {
                _treeNode.ImageKey = "UIColumn";
                _treeNode.SelectedImageKey = _treeNode.ImageKey;
            }

            return _treeNode;
        }

        #endregion

        /// <summary>
        /// Get XML representation of the EntityHistoryExcludeSubElement object
        /// </summary>
        /// <returns>XML representation of EntityHistoryExcludeSubElement</returns>
        public override String ToXml()
        {
            String parameterXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //EntityHistoryExcludeSubElement node start
            xmlWriter.WriteStartElement("EntityHistoryExcludeSubElement");

            xmlWriter.WriteAttributeString("ChangeType", this.ChangeType.ToString());
            xmlWriter.WriteAttributeString("Ids", Ids);

            //EntityHistoryExcludeSubElement node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            parameterXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return parameterXml;
        }

        /// <summary>
        /// Load the EntityHistoryExcludeSubElement object from the input xml
        /// </summary>
        /// <param name="valueAsXml">EntityHistoryExcludeSubElement object representation as xml</param>
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityHistoryExcludeSubElement")
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("EntityHistoryExcludeSubElement"))
                                {
                                    EntityHistoryExcludeChangeType entityHistoryExcludeChangeType = EntityHistoryExcludeChangeType.Unknown;
                                    Enum.TryParse<EntityHistoryExcludeChangeType>(reader.ReadContentAsString(), out entityHistoryExcludeChangeType);
                                    this.ChangeType = entityHistoryExcludeChangeType;
                                }


                                if (reader.MoveToAttribute("Ids"))
                                {
                                    this.Ids = reader.ReadContentAsString();
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
