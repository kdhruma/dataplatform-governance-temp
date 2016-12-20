using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Imports
{
    using MDM.Core;
    using MDM.Interfaces;
    
    /// <summary>
    /// Specifies KeywordProcessingOptions which specifies various flags and indications to handle keywords in entity processing
    /// </summary>
    [DataContract]
    public class KeywordProcessingOptions : ObjectBase, IKeywordProcessingOptions
    {
        #region Fields

        /// <summary>
        /// Field indicates if Ignore keyword should be processed
        /// </summary>
        private Boolean _enableIgnoreKeyword = true;

        /// <summary>
        /// Field indicates the value which will be interpreted as Ignore
        /// </summary>
        private String _ignoreKeyword = "[ignore]";

        /// <summary>
        /// Field indicates if Blank keyword should be processed
        /// </summary>
        private Boolean _enableBlankKeyword = true;

        /// <summary>
        /// Field indicates the value which will be interpreted as Blank
        /// </summary>
        private String _blankKeyword = "[blank]";

        /// <summary>
        /// Field indicates if Delete keyword should be processed
        /// </summary>
        private Boolean _enableDeleteKeyword = true;

        /// <summary>
        /// Field indicates the value which will be interpreted as Delete
        /// </summary>
        private String _deleteKeyword = "[delete]";

        /// <summary>
        /// Field indicates the data separator to be used to create multiple valuees for a collection
        /// </summary>
        private String _collectionDataSeparator = String.Empty;

        /// <summary>
        /// Field indicates the data separator to be used to identify uom in the value
        /// </summary>
        private String _uomDataSeparator = String.Empty;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public KeywordProcessingOptions()
            : base()
        { }

        /// <summary>
        /// 
        /// </summary>
        public KeywordProcessingOptions(Boolean enableIgnoreKeyword, String IgnoreKeyword,
                                            Boolean enableBlankKeyword, String BlankKeyword,
                                            Boolean enableDeleteKeyword, String DeleteKeyword)
            : base()
        {
            this._enableIgnoreKeyword = enableIgnoreKeyword;
            this._ignoreKeyword = IgnoreKeyword;
            this._enableBlankKeyword = enableBlankKeyword; 
            this._blankKeyword = BlankKeyword;
            this._enableDeleteKeyword = enableDeleteKeyword;
            this._deleteKeyword = DeleteKeyword;
        }

        /// <summary>
        /// Constructor with XML having values of object. 
        /// </summary>
        /// <param name="valuesAsXml">Indicates XML having xml value</param>
        public KeywordProcessingOptions(String valuesAsXml)
        {
            LoadKeywordProcessingOptions(valuesAsXml);
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Property indicates if process Ignore keyword
        /// </summary>
        [DataMember]
        public Boolean EnableIgnoreKeyword
        {
            get
            {
                return _enableIgnoreKeyword;
            }
            set
            {
                _enableIgnoreKeyword = value;
            }
        }

        /// <summary>
        /// Property indicates the keyword which should be interpreted as Ignore
        /// </summary>
        [DataMember]
        public String IgnoreKeyword
        {
            get
            {
                return _ignoreKeyword;
            }
            set
            {
                _ignoreKeyword = value;
            }
        }

        /// <summary>
        /// Property indicates if process Blank keyword
        /// </summary>
        [DataMember]
        public Boolean EnableBlankKeyword
        {
            get
            {
                return _enableBlankKeyword;
            }
            set
            {
                _enableBlankKeyword = value;
            }
        }

        /// <summary>
        /// Property indicates the keyword which should be interpreted as Blank
        /// </summary>
        [DataMember]
        public String BlankKeyword
        {
            get
            {
                return _blankKeyword;
            }
            set
            {
                _blankKeyword = value;
            }
        }

        /// <summary>
        /// Property indicates if process Delete keyword
        /// </summary>
        [DataMember]
        public Boolean EnableDeleteKeyword
        {
            get
            {
                return _enableDeleteKeyword;
            }
            set
            {
                _enableDeleteKeyword = value;
            }
        }

        /// <summary>
        /// Property indicates the keyword which should be interpreted as Delete
        /// </summary>
        [DataMember]
        public String DeleteKeyword
        {
            get
            {
                return _deleteKeyword;
            }
            set
            {
                _deleteKeyword = value;
            }
        }

        /// <summary>
        /// Property  indicates the data separator to be used to create multiple values from a given value for a collection attribute
        /// </summary>
        [DataMember]
        public String CollectionDataSeparator
        {
            get
            {
                return _collectionDataSeparator;
            }
            set
            {
                _collectionDataSeparator = value;
            }
        }

        /// <summary>
        /// Property indicates the data separator to be used to identify uom in the value
        /// </summary>
        [DataMember]
        public String UomDataSeparator
        {
            get
            {
                return _uomDataSeparator;
            }
            set
            {
                _uomDataSeparator = value;
            }
        }


        #endregion Properties

        #region Methods
         
        #region Public methods

        /// <summary>
        /// Represents KeywordProcessingOptions in Xml format
        /// </summary>
        /// <returns>String representation of current KeywordProcessingOptions object instance</returns>
        public String ToXml()
        {
            String xml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Attribute node start
            xmlWriter.WriteStartElement("KeywordProcessingOptions");

            xmlWriter.WriteAttributeString("EnableIgnoreKeyword", this.EnableIgnoreKeyword.ToString().ToLower());
            xmlWriter.WriteAttributeString("IgnoreKeyword", this.IgnoreKeyword.ToString().ToLower());
            xmlWriter.WriteAttributeString("EnableBlankKeyword", this.EnableBlankKeyword.ToString().ToLower());
            xmlWriter.WriteAttributeString("BlankKeyword", this.BlankKeyword.ToString().ToLower());
            xmlWriter.WriteAttributeString("EnableDeleteKeyword", this.EnableDeleteKeyword.ToString().ToLower());
            xmlWriter.WriteAttributeString("DeleteKeyword", this.DeleteKeyword.ToString());
            xmlWriter.WriteAttributeString("CollectionDataSeparator", this.CollectionDataSeparator.ToString().ToLower());
            xmlWriter.WriteAttributeString("UomDataSeparator", this.UomDataSeparator.ToString());

            //KeywordProcessingOptions end node
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();
            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Represents KeywordProcessingOptions in Xml format
        /// </summary>
        /// <returns>String representation of current KeywordProcessingOptions object instance</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String xml = String.Empty;

            if (serialization == ObjectSerialization.Full)
            {
                xml = this.ToXml();
            }
            else
            {

                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //Attribute node start
                xmlWriter.WriteStartElement("KeywordProcessingOptions");

                if (serialization == ObjectSerialization.ProcessingOnly)
                {
                    xmlWriter.WriteAttributeString("EnableIgnoreKeyword", this.EnableIgnoreKeyword.ToString().ToLower());
                    xmlWriter.WriteAttributeString("IgnoreKeyword", this.IgnoreKeyword.ToString().ToLower());
                    xmlWriter.WriteAttributeString("EnableBlankKeyword", this.EnableBlankKeyword.ToString().ToLower());
                    xmlWriter.WriteAttributeString("BlankKeyword", this.BlankKeyword.ToString().ToLower());
                    xmlWriter.WriteAttributeString("EnableDeleteKeyword", this.EnableDeleteKeyword.ToString().ToLower());
                    xmlWriter.WriteAttributeString("DeleteKeyword", this.DeleteKeyword.ToString());
                    xmlWriter.WriteAttributeString("CollectionDataSeparator", this.CollectionDataSeparator.ToString().ToLower());
                    xmlWriter.WriteAttributeString("UomDataSeparator", this.UomDataSeparator.ToString());
                }

                if (serialization == ObjectSerialization.UIRender)
                {
                    xmlWriter.WriteAttributeString("EnableIgnoreKeyword", this.EnableIgnoreKeyword.ToString().ToLower());
                    xmlWriter.WriteAttributeString("IgnoreKeyword", this.IgnoreKeyword.ToString().ToLower());
                    xmlWriter.WriteAttributeString("EnableBlankKeyword", this.EnableBlankKeyword.ToString().ToLower());
                    xmlWriter.WriteAttributeString("BlankKeyword", this.BlankKeyword.ToString().ToLower());
                    xmlWriter.WriteAttributeString("EnableDeleteKeyword", this.EnableDeleteKeyword.ToString().ToLower());
                    xmlWriter.WriteAttributeString("DeleteKeyword", this.DeleteKeyword.ToString());
                    xmlWriter.WriteAttributeString("CollectionDataSeparator", this.CollectionDataSeparator.ToString().ToLower());
                    xmlWriter.WriteAttributeString("UomDataSeparator", this.UomDataSeparator.ToString());
                }

                //KeywordProcessingOptions end node
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();
                //get the actual XML
                xml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            
            }
            return xml;
        }

        #endregion Public methods

        #region Private methods

        private void LoadKeywordProcessingOptions(String valuesAsXml)
        { 
            #region Sample Xml
            /*
             * <KeywordProcessingOptions EnableIgnoreKeyword="true" IgnoreKeyword="IGNORE"
                                    EnableBlankKeyword="true" BlankKeyword="[BLANK]"
                                    EnableDeleteKeyword="true" DeleteKeyword="DEL" />
             * */
            #endregion Sample Xml

            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "KeywordProcessingOptions")
                    {
                        #region Read KeywordProcessingOptions Properties

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("EnableIgnoreKeyword"))
                            {
                                this.EnableIgnoreKeyword = reader.ReadContentAsBoolean();
                            }

                            if (reader.MoveToAttribute("IgnoreKeyword"))
                            {
                                this.IgnoreKeyword = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("EnableBlankKeyword"))
                            {
                                this.EnableBlankKeyword = reader.ReadContentAsBoolean();
                            }

                            if (reader.MoveToAttribute("BlankKeyword"))
                            {
                                this.BlankKeyword = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("EnableDeleteKeyword"))
                            {
                                this.EnableDeleteKeyword = reader.ReadContentAsBoolean();
                            }

                            if (reader.MoveToAttribute("DeleteKeyword"))
                            {
                                this.DeleteKeyword = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("CollectionDataSeparator"))
                            {
                                this.CollectionDataSeparator = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("UomDataSeparator"))
                            {
                                this.UomDataSeparator = reader.ReadContentAsString();
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

        #endregion Private methods

        #endregion Methods
    }
}
