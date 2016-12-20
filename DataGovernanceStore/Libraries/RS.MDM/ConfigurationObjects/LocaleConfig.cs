using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel;
using System.IO;

using MDM.Core;
using RS.MDM.Configuration;
using MDM.Interfaces;

namespace RS.MDM.ConfigurationObjects
{
    /// <summary>
    /// Provides system default configuration for Application Localization
    /// </summary>
    [XmlRoot("LocaleConfig")]
    [Serializable()]
    public sealed class LocaleConfig : Object, ILocaleConfig
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

        /// <summary>
        /// Field denoting the System Data Locale of LocaleConfig
        /// </summary>
        private LocaleEnum _systemDataLocale = LocaleEnum.en_WW;

        /// <summary>
        /// Field denoting the System UI Locale of LocaleConfig
        /// </summary>
        private LocaleEnum _systemUILocale = LocaleEnum.en_WW;

        /// <summary>
        /// Field denoting the Model Display Local Type of LocaleConfig
        /// </summary>
        private LocaleType _modelDisplayLocaleType = LocaleType.DataLocale;

        /// <summary>
        /// Field denoting the Data Formatting Local Type of LocaleConfig
        /// </summary>
        private LocaleType _dataFormattingLocaleType = LocaleType.DataLocale; 

        /// <summary>
        /// Field denoting the allowable UI Locales of LocaleConfig
        /// </summary>
        private String _allowableUILocales = String.Empty;
      
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes new instance of LocaleConfig
        /// </summary>
        public LocaleConfig()
            : base()
        {
        }

        /// <summary>
        /// Initializes new instance of LocaleConfig with the provided values in the format of xml
        /// </summary>
        /// <param name="valuesAsxml">XML having values</param>
        public LocaleConfig(String valuesAsxml)
        {
            LoadLocaleConfig(valuesAsxml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting the System Data Locale
        /// </summary>
        [XmlAttribute("SystemDataLocale")]
        [Description("Represents System Data Locale.")]
        [Category("Properties")]
        [TrackChanges()]
        public LocaleEnum SystemDataLocale
        {
            get
            {
                return this._systemDataLocale;
            }
            set
            {
                this._systemDataLocale = value;
            }
        }

        /// <summary>
        /// Property denoting the System UI Locale
        /// </summary>
        [XmlAttribute("SystemUILocale")]
        [Description("Represents System UI Locale.")]
        [Category("Properties")]
        [TrackChanges()]
        public LocaleEnum SystemUILocale
        {
            get
            {
                return this._systemUILocale;
            }
            set
            {
                this._systemUILocale = value;
            }
        }

        /// <summary>
        /// Property denoting the Model Display Locale Type
        /// </summary>
        [XmlAttribute("ModelDisplayLocaleType")]
        [Description("Represents Model Display Locale Type.")]
        [Category("Properties")]
        [TrackChanges()]
        public LocaleType ModelDisplayLocaleType
        {
            get
            {
                return this._modelDisplayLocaleType;
            }
            set
            {
                this._modelDisplayLocaleType = value;
            }
        }

        /// <summary>
        /// Property denoting the Data Formatting Locale Type
        /// </summary>
        [XmlAttribute("DataFormattingLocaleType")]
        [Description("Represents Data Formatting Locale Type.")]
        [Category("Properties")]
        [TrackChanges()]
        public LocaleType DataFormattingLocaleType
        {
            get
            {
                return this._dataFormattingLocaleType;
            }
            set
            {
                this._dataFormattingLocaleType = value;
            }
        }

        /// <summary>
        /// Property denoting the allowable UI Locales
        /// </summary>
        [XmlAttribute("AllowableUILocales")]
        [Description("Represents allowable UI Locales.")]
        [Category("Properties")]
        [TrackChanges]
        public String AllowableUILocales
        {
            get
            {
                if (String.IsNullOrWhiteSpace(this._allowableUILocales))
                {
                    this._allowableUILocales = this.SystemUILocale.ToString();
                }
                else if (!this._allowableUILocales.Contains(this.SystemUILocale.ToString()))
                {
                    this._allowableUILocales = String.Format("{0},{1}", this.SystemUILocale.ToString(), this._allowableUILocales);
                }

                return this._allowableUILocales;
            }
            set
            {
                this._allowableUILocales = value;
            }
        }
       
        #endregion

        #region Methods

        #region Load Methods

        ///<summary>
        /// Load Locale Config object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     <para>
        ///     <![CDATA[
        ///         <LocaleConfig
        ///             Id="101" 
        ///             UniqueIdentifier="292c728e-099e-4680-a63a-9307f5d55228"
        ///             Name="LocaleConfig"
        ///             Description=""
        ///             Tag=""
        ///             ClassName="RS.MDM.Web.UI.Configuration.LocaleConfig" 
        ///             AssemblyName="PCWebControls.dll"
        ///             InheritedParentUId=""
        ///             SystemDataLocale="en-WW"
        ///             SystemUILocale="en-WW"
        ///             ModelDisplayLocaleType="DataLocale"
        ///             DataFormattingLocaleType="DataLocale"
        ///             AllowableUILocales ="en-WW,de-DE,fr-FR">
        ///         </LocaleConfig>
        ///     ]]>    
        ///     </para>
        /// </example>
        public void LoadLocaleConfig(String valuesAsXml)
        {
            #region Sample Xml
            /**
                <LocaleConfig
                    /// Id="101" 
                    /// UniqueIdentifier="292c728e-099e-4680-a63a-9307f5d55228"
                    /// Name="LocaleConfig"
                    /// Description=""
                    /// Tag=""
                    /// ClassName="RS.MDM.Web.UI.Configuration.LocaleConfig" 
                    /// AssemblyName="PCWebControls.dll"
                    /// InheritedParentUId=""
                    /// SystemDataLocale="en_WW"
                    /// SystemUILocale="en_WW"
                    /// ModelDisplayLocaleType="DataLocale"
                    /// AllowableUILocales ="en_WW,de_DE,fr_FR">
                </LocaleConfig>
            **/
            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "LocaleConfig")
                        {
                            #region Read LocaleConfig Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                    this.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);

                                if (reader.MoveToAttribute("UniqueIdentifier"))
                                    this.UniqueIdentifier = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("Name"))
                                    this.Name = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("Description"))
                                    this.Description = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("Tag"))
                                    this.Tag = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("InheritedParentUId"))
                                    this.InheritedParentUId = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("SystemDataLocale"))
                                {
                                    LocaleEnum localeEnum = LocaleEnum.UnKnown;

                                    String locale = reader.ReadContentAsString();

                                    if (!String.IsNullOrWhiteSpace(locale))
                                        Enum.TryParse(reader.ReadContentAsString(), out localeEnum);

                                    this.SystemDataLocale = localeEnum;
                                }

                                if (reader.MoveToAttribute("SystemUILocale"))
                                {
                                    LocaleEnum localeEnum = LocaleEnum.UnKnown;

                                    String locale = reader.ReadContentAsString();

                                    if (!String.IsNullOrWhiteSpace(locale))
                                        Enum.TryParse(reader.ReadContentAsString(), out localeEnum);

                                    this.SystemUILocale = localeEnum;
                                }

                                if (reader.MoveToAttribute("ModelDisplayLocaleType"))
                                {
                                    LocaleType localeType = LocaleType.Unknown;

                                    String localeTypeString = reader.ReadContentAsString();

                                    if (!String.IsNullOrWhiteSpace(localeTypeString))
                                        Enum.TryParse(reader.ReadContentAsString(), out localeType);

                                    this.ModelDisplayLocaleType = localeType;
                                }

                                if (reader.MoveToAttribute("AllowableUILocales"))
                                {
                                    this.AllowableUILocales = reader.ReadContentAsString();
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
        }

        #endregion

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of Locale Config
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public override String ToXml()
        {
            String localeConfigXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //LocaleConfig node start
            xmlWriter.WriteStartElement("LocaleConfig");

            #region Write Properties

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("UniqueIdentifier", this.UniqueIdentifier);
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("Description", this.Description);
            xmlWriter.WriteAttributeString("Tag", this.Tag);
            xmlWriter.WriteAttributeString("InheritedParentUId", this.InheritedParentUId);
            xmlWriter.WriteAttributeString("SystemDataLocale", this.SystemDataLocale.ToString());
            xmlWriter.WriteAttributeString("SystemUILocale", this.SystemUILocale.ToString());
            xmlWriter.WriteAttributeString("ModelDisplayLocaleType", this.ModelDisplayLocaleType.ToString());
            xmlWriter.WriteAttributeString("DataFormattingLocaleType", this.DataFormattingLocaleType.ToString());
            xmlWriter.WriteAttributeString("AllowableUILocales", this.AllowableUILocales);

            #endregion

            //LocaleConfig node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            localeConfigXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return localeConfigXml;
        }

        /// <summary>
        /// Get Xml representation of Locale Config
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String localeConfigXml = String.Empty;

            if (serialization == ObjectSerialization.Full)
            {
                localeConfigXml = this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //LocaleConfig node start
                xmlWriter.WriteStartElement("LocaleConfig");

                if (serialization == ObjectSerialization.ProcessingOnly)
                {
                    #region Write LocaleConfig Properties

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("UniqueIdentifier", this.UniqueIdentifier);
                    xmlWriter.WriteAttributeString("Name", this.Name);
                    xmlWriter.WriteAttributeString("Description", this.Description);
                    xmlWriter.WriteAttributeString("Tag", this.Tag);
                    xmlWriter.WriteAttributeString("InheritedParentUId", this.InheritedParentUId);
                    xmlWriter.WriteAttributeString("SystemDataLocale", this.SystemDataLocale.ToString());
                    xmlWriter.WriteAttributeString("SystemUILocale", this.SystemUILocale.ToString());
                    xmlWriter.WriteAttributeString("ModelDisplayLocaleType", this.ModelDisplayLocaleType.ToString());
                    xmlWriter.WriteAttributeString("DataFormattingLocaleType", this.DataFormattingLocaleType.ToString());

                    #endregion
                }
                else if (serialization == ObjectSerialization.UIRender)
                {
                    #region Write LocaleConfig Properties

                    xmlWriter.WriteAttributeString("SystemDataLocale", this.SystemDataLocale.ToString());
                    xmlWriter.WriteAttributeString("SystemUILocale", this.SystemUILocale.ToString());
                    xmlWriter.WriteAttributeString("ModelDisplayLocaleType", this.ModelDisplayLocaleType.ToString());
                    xmlWriter.WriteAttributeString("DataFormattingLocaleType", this.DataFormattingLocaleType.ToString());
                    #endregion
                }

                xmlWriter.WriteAttributeString("AllowableUILocales", this.AllowableUILocales);

                //LocaleConfig node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //get the actual XML
                localeConfigXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }

            return localeConfigXml;
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

        #endregion
    }
}
