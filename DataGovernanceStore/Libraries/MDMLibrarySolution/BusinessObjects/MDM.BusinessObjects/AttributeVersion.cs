using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represent class to indicate version of attribute
    /// </summary>
    [DataContract]
    public class AttributeVersion : MDMObject, IAttributeVersion
    {
        #region Fields

        /// <summary>
        /// Field for the id of an AttributeValue
        /// </summary>
        private Int64 _id = -1;

        /// <summary>
        /// Field Denoting ParentAttribute Id
        /// </summary>
        private Int64 _parentId = -1;

        /// <summary>
        /// Field for attribute value source ('O' or 'I')
        /// </summary>
        private AttributeValueSource _sourceFlag = AttributeValueSource.Overridden;

        /// <summary>
        /// Field for attribute value collection
        /// </summary>
        private Value _value = null;

        /// <summary>
        /// Field denoting value reference id.
        /// For complex attributes, InstanceRefId represents the PK of respective complex attribute (tbcx_) table
        /// </summary>
        private Int32 _instanceRefId = -1;

        /// <summary>
        /// Field denoting sequence of current attribute.
        /// This is used in case of complex and complex collection attribute
        /// </summary>
        private Decimal _sequence = -1;

        /// <summary>
        /// Field denoting the Modification dateTime 
        /// </summary>
        private DateTime _modDateTime = DateTime.Now;

        /// <summary>
        /// Field Denoting the Modification User
        /// </summary>
        private String _modUser = String.Empty;

        /// <summary>
        /// Field denoting the ModProgram
        /// </summary>
        private String _modProgram = String.Empty;

        /// <summary>
        /// Field denoting the Display Value
        /// </summary>
        private String _displayValue = String.Empty;

        /// <summary>
        /// Field denoting the User Action
        /// </summary>
        private String _userAction = String.Empty;

        /// <summary>
        /// Field denoting the GroupId
        /// </summary>
        private Int32 _groupId = -1;
        /// <summary>
        /// Field indicates whether version had invalid value
        /// </summary>
        private Boolean _hasInvalidValue;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public AttributeVersion()
            : base()
        {
        }
        

        /// <summary>
        /// Initializes a new instance of the Attribute Version 
        /// </summary>
        /// <param name="id">Indicates id of attribute</param>
        /// <param name="parentId">Indicates parent id of attribute</param>
        /// <param name="locale">Indicates locale of attribute</param>
        /// <param name="attributeValue">Indicates value of attribute</param>
        /// <param name="sequence">Indicates sequence of attribute</param>
        /// <param name="modDateTime">Indicates modified date and time of attribute</param>
        /// <param name="moduser">Indicates modified user of attribute</param>
        /// <param name="modProgram">Indicates modified program of attribute</param>
        /// <param name="instanceRefId">Indicates instance reference id of attribute</param>
        /// <param name="userAction">Indicates user action on attribute</param>
        public AttributeVersion(Int64 id, Int64 parentId, LocaleEnum locale, Object attributeValue,Decimal sequence, DateTime modDateTime, String moduser, String modProgram, Int32 instanceRefId, String userAction)
        {
            this._id = id;
            this._parentId = parentId;
            this._sequence = sequence;            
            this._modDateTime = modDateTime;
            this._modUser = moduser;
            this._modProgram = modProgram;
            this._instanceRefId = instanceRefId;            
            this.Locale = locale;
            //Add value
            Value value = new Value(attributeValue);
            this._value = value;
            this._sourceFlag = AttributeValueSource.Overridden;
            this._userAction = userAction;
        }

        /// <summary>
        /// Create attribute object with property values xml as input parameter
        /// </summary>
        /// <example>
        /// Sample XML
        /// </example>
        /// <param name="valuesAsXml">XML representation for attribute from which object is to be created</param>
        public AttributeVersion(String valuesAsXml)
        {
            LoadAttributeVersion(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property for the id of an AttributeValue
        /// </summary>
        [DataMember]
        public new Int64 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Property Denoting the ParentAttribute Id
        /// </summary>
        [DataMember]
        public Int64 ParentId
        {
            get { return this._parentId; }
            set { this._parentId = value; }
        }

        /// <summary>
        /// Property denoting value reference id.
        /// Null in case of simple. Will be having WSID in case of complex
        /// </summary>
        [DataMember]
        public Int32 InstanceRefId
        {
            get
            {
                return this._instanceRefId;
            }
            set
            {
                this._instanceRefId = value;
            } 
        }

        /// <summary>
        /// Property for attribute value source ('O' or 'I') 
        /// </summary>
        [DataMember]
        public AttributeValueSource SourceFlag 
        {
            get { return _sourceFlag; }
            set { _sourceFlag = value; }
        }

        /// <summary>
        /// Property denoting sequence of current attribute.
        /// This is used in case of complex and complex collection attribute
        /// </summary>
        [DataMember]
        public Decimal Sequence
        {
            get
            {
                return this._sequence;
            }
            set
            {
                this._sequence = value;
            }
        }        

        /// <summary>
        /// Property denoting DateTime of Modification
        /// </summary>
        [DataMember]
        public DateTime ModDateTime 
        {
            get
            {
                return this._modDateTime;
            }
            set
            {
                this._modDateTime = value;
            }
        }

        /// <summary>
        /// Property Denoting the user that modified the Attribute
        /// </summary>
        [DataMember]
        public String ModUser
        {
            get
            {
                return this._modUser;
            }
            set
            {
                this._modUser = value;
            }
        }

        /// <summary>
        /// Property denoting the ModProgram 
        /// </summary>
        [DataMember]
        public String ModProgram
        {
            get
            {
                return this._modProgram;
            }
            set
            {
                this._modProgram = value;
            }
        }

        /// <summary>
        /// Property to get the value of attribute as Object
        /// </summary>
         [DataMember]
        public Value Value
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

        /// <summary>
        /// Property to get the Display Value
        /// </summary>
         [DataMember]
        public String DisplayValue
        {
            get
            {
                return this._displayValue;
            }
            set
            {
                this._displayValue = value;
            }
        }

         /// <summary>
         /// Property to get the User Action
         /// </summary>
         [DataMember]
         public String UserAction
         {
             get
             {
                 return this._userAction;
             }
             set
             {
                 this._userAction = value;
             }
         }

         /// <summary>
         /// Property to get the GroupId
         /// </summary>
         [DataMember]
         public Int32 GroupId
         {
             get
             {
                 return this._groupId;
             }
             set
             {
                 this._groupId = value;
             }
         }

        /// <summary>
        /// Property indicates whether attribute version has invalid value or not
        /// </summary>
        [DataMember]
        public Boolean HasInvalidValue
        {
            get
            {
                return this._hasInvalidValue;
            }
            set
            {
                this._hasInvalidValue = value;
            }
        }

        #endregion

        #region Methods

        #region Load Methods

        /// <summary>
        /// Populate current object from incoming XML
        /// </summary>        
        public void LoadAttributeVersion(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "AttributeVersion")
                        {
                            #region Read AttributeVersion Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = reader.ReadContentAsLong();
                                }

                                if (reader.MoveToAttribute("InstanceRefId"))
                                {
                                    this.InstanceRefId = reader.ReadContentAsInt();
                                }

                                if (reader.MoveToAttribute("Sequence"))
                                {
                                    this.Sequence = reader.ReadContentAsDecimal();
                                }

                                if (reader.MoveToAttribute("SourceFlag"))
                                {
                                    this.SourceFlag = Utility.GetSourceFlagEnum(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("ModDateTime"))
                                {
                                    this.ModDateTime = reader.ReadContentAsDateTime();
                                }

                                if (reader.MoveToAttribute("ModUser"))
                                {
                                    this.ModUser = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ModProgram"))
                                {
                                    this.ModProgram = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("DisplayValue"))
                                {
                                    this.DisplayValue = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Locale"))
                                {
                                    Int32 localeId = reader.ReadContentAsInt();
                                    LocaleEnum locale = LocaleEnum.UnKnown;
                                    locale = (LocaleEnum)localeId;
                                    this.Locale = locale;
                                }

                                if (reader.MoveToAttribute("UserAction"))
                                {
                                    this.UserAction = reader.ReadContentAsString();
                                }
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Value")
                        {
                            #region Read values for current attribute

                            //Get Value from Xml
                            String valueXml = reader.ReadOuterXml();
                            if (!String.IsNullOrWhiteSpace(valueXml))
                            {
                                Value val = new Value(valueXml);
                                this.Value = val;                               
                            }

                            #endregion Read values for current attribute
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

        #region XML Methods

        /// <summary>
        /// Get Xml representation of AttributeVersion object
        /// </summary>
        /// <returns></returns>
        public override String ToXml()
        {
            String attributeVersionXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Attribute node start
            xmlWriter.WriteStartElement("AttributeVersion");

            #region write Attribute properties for full attribute xml

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());           
            xmlWriter.WriteAttributeString("InstanceRefId", this.InstanceRefId.ToString());
            xmlWriter.WriteAttributeString("Sequence", this.Sequence.ToString());            
            xmlWriter.WriteAttributeString("SourceFlag", Utility.GetSourceFlagString(this.SourceFlag));
            xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
            xmlWriter.WriteAttributeString("ModDateTime", this.ModDateTime.ToString());
            xmlWriter.WriteAttributeString("ModUser", this.ModUser);
            xmlWriter.WriteAttributeString("ModProgram", this.ModProgram);
            xmlWriter.WriteAttributeString("UserAction", this.UserAction);

            #endregion  write Attribute properties for full attribute xml

            #region write overridden attribute value

            if (this.Value != null)
            {
                xmlWriter.WriteRaw(this.Value.ToXml());
            }

            #endregion write overridden attribute value

            return attributeVersionXml;
        }

        /// <summary>
        /// Get Xml representation of AttributeVersion object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public override String ToXml(ObjectSerialization serialization)
        {
            String attributeVersionXml = String.Empty;

            if (serialization == ObjectSerialization.Full)
            {
                attributeVersionXml = this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //AttributeVersion node start
                xmlWriter.WriteStartElement("AttributeVersion");

                if (serialization == ObjectSerialization.ProcessingOnly)
                {
                    #region Write AttributeVersion properties for ProcessingOnly Xml

                    // TODO : Need to decide which all properties are needed for processing Xml.
                    // currently returning all properties.
                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("InstanceRefId", this.InstanceRefId.ToString());
                    xmlWriter.WriteAttributeString("Sequence", this.Sequence.ToString());
                    xmlWriter.WriteAttributeString("SourceFlag", Utility.GetSourceFlagString(this.SourceFlag));
                    xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
                    xmlWriter.WriteAttributeString("ModDateTime", this.ModDateTime.ToString());
                    xmlWriter.WriteAttributeString("ModUser", this.ModUser);
                    xmlWriter.WriteAttributeString("ModProgram", this.ModProgram);
                    xmlWriter.WriteAttributeString("UserAction", this.UserAction);

                    #endregion Write AttributeVersion properties for ProcessingOnly Xml
                }
                else if (serialization == ObjectSerialization.UIRender)
                {
                    #region Write AttributeVersion properties for Rendering Xml

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());                    
                    xmlWriter.WriteAttributeString("InstanceRefId", this.InstanceRefId.ToString());
                    xmlWriter.WriteAttributeString("Sequence", this.Sequence.ToString());                    
                    xmlWriter.WriteAttributeString("SourceFlag", Utility.GetSourceFlagString(this.SourceFlag));
                    xmlWriter.WriteAttributeString("UserAction", this.UserAction);

                    #endregion Write AttributeVersion properties for Rendering Xml
                }
                else if (serialization == ObjectSerialization.External)
                {
                    #region Write AttributeVersion properties for External Xml

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());                 
                    xmlWriter.WriteAttributeString("InstanceRefId", this.InstanceRefId.ToString());                    

                    #endregion
                }

                #region write overridden attribute value

                if (this.Value != null)
                {
                    xmlWriter.WriteRaw(this.Value.ToXml());
                }

                #endregion write overridden attribute value              

                //AttributeVersion node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //get the actual XML
                attributeVersionXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }

            return attributeVersionXml;
        }

        #endregion
        
        #endregion

    }
}
