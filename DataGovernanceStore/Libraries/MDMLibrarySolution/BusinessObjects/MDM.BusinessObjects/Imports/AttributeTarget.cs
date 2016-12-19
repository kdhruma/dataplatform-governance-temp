using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Imports
{
    using MDM.Core;

    /// <summary>
    /// AttributeTarget Map
    /// </summary>
    [DataContract]
    public class AttributeTarget : ObjectBase
    {
        #region Fields

        /// <summary>
        ///
        /// </summary>
        private AttributeModelType _modelType = AttributeModelType.Unknown;

        /// <summary>
        ///
        /// </summary>
        private Int32 _id = 0;

        /// <summary>
        ///
        /// </summary>
        private String _name = String.Empty;

        /// <summary>
        ///
        /// </summary>
        private String _parentName = String.Empty;

        /// <summary>
        ///
        /// </summary>
        private LocaleEnum _locale = LocaleEnum.UnKnown;

        /// <summary>
        ///
        /// </summary>
        private Boolean _failEntityOnError = false;

        /// <summary>
        ///
        /// </summary>
        private Boolean _canDeleteAttribute = true;

        /// <summary>
        ///
        /// </summary>
        private Boolean _canUpdateAttribute = true;

        /// <summary>
        ///
        /// </summary>
        private Boolean _canAddAttribute = true;
        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public AttributeTarget()
            : base()
        {
        }

        /// <summary>
        /// Constructor with XML having values of object. 
        /// </summary>
        /// <param name="valuesAsXml">Indicates XML having xml value</param>
        public AttributeTarget(String valuesAsXml)
        {
            LoadAttributeTarget(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property defining the type of the MDM object
        /// </summary>
        public new String ObjectType
        {
            get
            {
                return "AttributeTarget";
            }
        }

        /// <summary>
        /// Property defines target attribute's type(Common, Tech, Relationship, etc)
        /// </summary>
        [DataMember]
        public AttributeModelType ModelType
        {
            get { return _modelType; }
            set { _modelType = value; }
        }

        /// <summary>
        /// Property defines target attribute id
        /// </summary>
        [DataMember]
        public Int32 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Property defines target attribute name
        /// </summary>
        [DataMember]
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Property defines target attribute parent name
        /// </summary>
        [DataMember]
        public String ParentName
        {
            get
            {
                return _parentName;
            }
            set
            {
                _parentName = value;
            }
        }

        /// <summary>
        /// Property defines target attribute's locale
        /// </summary>
        [DataMember]
        public LocaleEnum Locale
        {
            get
            {
                return _locale;
            }
            set
            {
                _locale = value;
            }
        }

        /// <summary>
        /// Property defines if we need to fail entity processing when specified attribute has data error
        /// </summary>
        [DataMember]
        public Boolean FailEntityOnError
        {
            get { return _failEntityOnError; }
            set { _failEntityOnError = value; }
        }

        /// <summary>
        /// Property defines whether an attribute can be added based on Attribute Action flag in Import profile
        /// </summary>
        [DataMember]
        public Boolean CanAddAttribute
        {
            get { return _canAddAttribute; }
            set { _canAddAttribute = value; }
        }

        /// <summary>
        /// Property defines whether an attribute can be deleted based on Attribute Action flag in Import profile
        /// </summary>
        [DataMember]
        public Boolean CanDeleteAttribute
        {
            get { return _canDeleteAttribute; }
            set { _canDeleteAttribute = value; }
        }

        /// <summary>
        /// Property defines whether an attribute can be updated based on Attribute Action flag in Import profile
        /// </summary>
        [DataMember]
        public Boolean CanUpdateAttribute
        {
            get { return _canUpdateAttribute; }
            set { _canUpdateAttribute = value; }
        }
        #endregion

        #region Methods

        #region Load Methods

        ///<summary>
        /// Load AttributeTarget object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        /// </example>
        public void LoadAttributeTarget(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "AttributeTarget")
                        {
                            #region Read AttributeTarget Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("ModelType"))
                                {
                                    AttributeModelType modelType = AttributeModelType.Unknown;
                                    Enum.TryParse<AttributeModelType>(reader.ReadContentAsString(), true, out modelType);
                                    this.ModelType = modelType;
                                }

                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = reader.ReadContentAsInt();
                                }

                                if (reader.MoveToAttribute("Name"))
                                {
                                    this.Name = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ParentName"))
                                {
                                    this.ParentName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Locale"))
                                {
                                    LocaleEnum locale = LocaleEnum.UnKnown;
                                    Enum.TryParse<LocaleEnum>(reader.ReadContentAsString(), out locale);
                                    this.Locale = locale;
                                }

                                if (reader.MoveToAttribute("FailEntityOnError"))
                                {
                                    this.FailEntityOnError = reader.ReadContentAsBoolean();
                                }

                                if (reader.MoveToAttribute("CanAddAttribute"))
                                {
                                    this.CanAddAttribute = reader.ReadContentAsBoolean();
                                }

                                if (reader.MoveToAttribute("CanUpdateAttribute"))
                                {
                                    this.CanUpdateAttribute = reader.ReadContentAsBoolean();
                                }

                                if (reader.MoveToAttribute("CanDeleteAttribute"))
                                {
                                    this.CanDeleteAttribute = reader.ReadContentAsBoolean();
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
        /// Get Xml representation of AttributeTarget
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXml()
        {
            String attributeTargetXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //AttributeTarget node start
            xmlWriter.WriteStartElement("AttributeTarget");

            #region Write AttributeTarget Properties for Full AttributeTarget Xml

            xmlWriter.WriteAttributeString("ModelType", this.ModelType.ToString());
            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("ParentName", this.ParentName);
            xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
            xmlWriter.WriteAttributeString("FailEntityOnError", this.FailEntityOnError.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("CanAddAttribute", this.CanAddAttribute.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("CanUpdateAttribute", this.CanUpdateAttribute.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("CanDeleteAttribute", this.CanDeleteAttribute.ToString().ToLowerInvariant());
            #endregion

            //AttributeTarget node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            attributeTargetXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return attributeTargetXml;
        }

        ///<summary>
        /// Get Xml representation of AttributeTarget
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String attributeTargetXml = String.Empty;

            if (serialization == ObjectSerialization.Full)
            {
                attributeTargetXml = this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //AttributeTarget node start
                xmlWriter.WriteStartElement("AttributeTarget");

                if (serialization == ObjectSerialization.ProcessingOnly)
                {
                    #region Write AttributeTarget Properties for Processing Only

                    xmlWriter.WriteAttributeString("ModelType", this.ModelType.ToString());
                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("Name", this.Name);
                    xmlWriter.WriteAttributeString("ParentName", this.ParentName);
                    xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
                    xmlWriter.WriteAttributeString("FailEntityOnError", this.FailEntityOnError.ToString());
                    xmlWriter.WriteAttributeString("CanAddAttribute", this.CanAddAttribute.ToString().ToLowerInvariant());
                    xmlWriter.WriteAttributeString("CanUpdateAttribute", this.CanUpdateAttribute.ToString().ToLowerInvariant());
                    xmlWriter.WriteAttributeString("CanDeleteAttribute", this.CanDeleteAttribute.ToString().ToLowerInvariant());
                    #endregion
                }
                else if (serialization == ObjectSerialization.UIRender)
                {
                    #region Write AttributeTarget Properties for Rendering

                    xmlWriter.WriteAttributeString("ModelType", this.ModelType.ToString());
                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("Name", this.Name);
                    xmlWriter.WriteAttributeString("ParentName", this.ParentName);
                    xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
                    xmlWriter.WriteAttributeString("FailEntityOnError", this.FailEntityOnError.ToString());
                    xmlWriter.WriteAttributeString("CanAddAttribute", this.CanAddAttribute.ToString().ToLowerInvariant());
                    xmlWriter.WriteAttributeString("CanUpdateAttribute", this.CanUpdateAttribute.ToString().ToLowerInvariant());
                    xmlWriter.WriteAttributeString("CanDeleteAttribute", this.CanDeleteAttribute.ToString().ToLowerInvariant());
                    #endregion
                }

                //mapping node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //get the actual XML
                attributeTargetXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }

            return attributeTargetXml;
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}
