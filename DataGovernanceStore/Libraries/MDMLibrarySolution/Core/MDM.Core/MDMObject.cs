using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

using ProtoBuf;
using System.Text;

namespace MDM.Core
{
    /// <summary>
    /// Base class for business objects with locale specific characteristics
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    [ProtoContract]
    [KnownType(typeof(LocaleEnum))]
    public class MDMObject : ObjectBase, IMDMObject
    {
        #region Fields

        /// <summary>
        /// Field for the id of an object
        /// </summary>
        private Int32 _id = -1;

        /// <summary>
        /// Field for the name of an object. Name often refers to the ShortName
        /// </summary>
        private String _name = String.Empty;

        /// <summary>
        /// Field for the name of an object in lower case.
        /// </summary>
        private String _nameInLowerCase = String.Empty;

        /// <summary>
        /// Field for the descriptive name of an object
        /// </summary>
        private String _longName = String.Empty;

        /// <summary>
        /// Field for the locale of an object
        /// </summary>
        private LocaleEnum _locale = LocaleEnum.en_WW;

        /// <summary>
        /// Field for the action of an object.
        /// </summary>
        private ObjectAction _action = ObjectAction.Read;

        /// <summary>
        /// Field for the AuditRefId of an object.
        /// </summary>
        private Int64 _auditRefId = -1;

        /// <summary>
        /// Field for the programName of an object.
        /// </summary>
        private String _programName = String.Empty;

        /// <summary>
        /// Field for the UserName for an object who changed it
        /// </summary>
        private String _userName = String.Empty;

        /// <summary>
        /// Field for reference Id
        /// </summary>
        private String _referenceId = String.Empty;

        /// <summary>
        /// 
        /// </summary>
        private String _extendedProperties = null;

        /// <summary>
        /// Field for allowed user actions on the object
        /// </summary>
        private Collection<UserAction> _permissionSet = null;


        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public MDMObject()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Identity of an Object</param>
        public MDMObject(Int32 id)
            : this()
        {
            this._id = id;
        }

        /// <summary>
        /// Constructor with Id, Name and LongName of an Object as input parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of an Object</param>
        /// <param name="name">Indicates the Name of an Object</param>
        /// <param name="longName">Indicates the LongName of an Object</param>
        public MDMObject(Int32 id, String name, String longName)
            : this(id)
        {
            this._name = name;
            this._nameInLowerCase = name.ToLower();
            this._longName = longName;
        }

        /// <summary>
        /// Constructor with Id, Name and LongName of an Localized value for an object as input parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of an Object</param>
        /// <param name="name">Indicates the Name of an Object</param>
        /// <param name="longName">Indicates the LongName of an Object</param>
        /// <param name="locale">Indicates the Locale of an Object</param>
        public MDMObject(Int32 id, String name, String longName, LocaleEnum locale)
            : this(id, name, longName)
        {
            this._locale = locale;
        }

        /// <summary>
        /// Constructor with Id, Name, LongName, auditRefId and programName of an Localized value for an object as input parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of an Object</param>
        /// <param name="name">Indicates the Name of an Object</param>
        /// <param name="longName">Indicates the LongName of an Object</param>
        /// <param name="locale">Indicates the Locale of an Object</param>
        /// <param name="auditRefId">Indicates the AuditRefId of an Object</param>
        /// <param name="programName">Indicates programName of an Object</param>
        public MDMObject(Int32 id, String name, String longName, LocaleEnum locale, Int64 auditRefId, String programName)
            : this(id, name, longName)
        {
            this._locale = locale;
            this._auditRefId = auditRefId;
            this._programName = programName;
        }

        /// <summary>
        /// Constructor with Id, Name, name in lower case and LongName of an Object as input parameters
        /// </summary>
        /// <param name="objectTypeId">Id of the ObjectType</param>
        /// <param name="objectType">Object Type</param>
        /// <param name="nameInLowerCase">Indicates the lower case name of an Object</param>
        /// <param name="name">Indicates the Name of an Object</param>
        /// <param name="longName">Indicates the LongName of an Object</param>
        public MDMObject(Int32 objectTypeId, String objectType, String name, String nameInLowerCase, String longName)
            : this(objectTypeId, objectType)
        {
            this._name = name;
            this._nameInLowerCase = nameInLowerCase;
            this._longName = longName;
        }


        /// <summary>
        /// Initializes object with the values provided as Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml string having values</param>
        /// <param name="objectSerialization">Specifies Enum type of objectSerialization</param>
        public MDMObject(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadMDMObject(valuesAsXml, objectSerialization);
        }

        /// <summary>
        /// Constructor with objectTypeId and objectType as input parameter
        /// </summary>
        /// <param name="objectTypeId">Id of the ObjectType</param>
        /// <param name="objectType">Object Type</param>
        public MDMObject(Int32 objectTypeId, String objectType)
            : base(objectTypeId, objectType)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Indicates the Id of an object
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        public Int32 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Indicates the Name of an object. Name often refers to the ShortName
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        public virtual String Name
        {
            get { return _name; }
            set
            {
                _name = value;
                _nameInLowerCase = _name.ToLower();
            }
        }

        /// <summary>
        /// Indicates the Name of an object in lower case.
        /// </summary>
        [DataMember]
        [ProtoMember(3)]
        public virtual String NameInLowerCase
        {
            get { return _nameInLowerCase; }
            set { _nameInLowerCase = value; }
        }

        /// <summary>
        /// Indicates the Long Name of an object
        /// </summary>
        [DataMember]
        [ProtoMember(4)]
        public virtual String LongName
        {
            get { return _longName; }
            set { _longName = value; }
        }

        /// <summary>
        /// Indicates the Locale of an object
        /// </summary>
        [DataMember]
        [ProtoMember(5)]
        public LocaleEnum Locale
        {
            get { return _locale; }
            set { _locale = value; }
        }

        /// <summary>
        /// Indicates the Action of an object
        /// </summary>
        [DataMember]
        [ProtoMember(6)]
        public virtual ObjectAction Action
        {
            get { return _action; }
            set { _action = value; }
        }

        /// <summary>
        /// Property for AuditrefId of an Object
        /// </summary>
        [DataMember]
        [ProtoMember(7)]
        public Int64 AuditRefId
        {
            get { return _auditRefId; }
            set { _auditRefId = value; }
        }

        /// <summary>
        /// Property for ProgramName of an Object
        /// </summary>
        [DataMember]
        [ProtoMember(8)]
        public String ProgramName
        {
            get { return _programName; }
            set { _programName = value; }
        }

        /// <summary>
        /// Property for the UserName for an object who changed it
        /// </summary>
        [DataMember]
        [ProtoMember(9)]
        public String UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        /// <summary>
        /// Property for 
        /// </summary>
        [DataMember]
        [ProtoMember(10)]
        public String ExtendedProperties
        {
            get { return _extendedProperties; }
            set { _extendedProperties = value; }
        }

        /// <summary>
        /// Property for reference Id
        /// </summary>
        [DataMember]
        [ProtoMember(11)]
        public String ReferenceId
        {
            get { return _referenceId; }
            set { _referenceId = value; }
        }

        /// <summary>
        /// Indicates allowed user actions on the object
        /// </summary>
        [DataMember]
        [ProtoMember(12)]
        public Collection<UserAction> PermissionSet
        {
            get { return _permissionSet; }
            set { _permissionSet = value; }
        }



        #endregion

        #region Overrides

        /// <summary>
        /// Check the equality of object
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is MDMObject)
            {
                MDMObject objectToBeCompared = obj as MDMObject;

                if (this.Id != objectToBeCompared.Id)
                    return false;

                if (this.Name != objectToBeCompared.Name)
                    return false;

                if (this.LongName != objectToBeCompared.LongName)
                    return false;

                if (this.Locale != objectToBeCompared.Locale)
                    return false;

                if (this.AuditRefId != objectToBeCompared.AuditRefId)
                    return false;

                if (this.ProgramName != objectToBeCompared.ProgramName)
                    return false;

                if (this.UserName != objectToBeCompared.UserName)
                    return false;


                return true;
            }
            return false;
        }

        /// <summary>
        /// Serves as a hash function for a object,
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            //TODO:: How to include this.ExtendedProperties.GetHashCode() into hash code..
            int hashCode = this.Id.GetHashCode() ^ this.Name.GetHashCode() ^ this.LongName.GetHashCode() ^ this.Locale.GetHashCode() ^ this.AuditRefId.GetHashCode() ^ this.ProgramName.GetHashCode() ^ this.UserName.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object</param>
        /// <param name="compareIds">Indicates if Ids to be compared or not</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public virtual Boolean IsSuperSetOf(Object obj, Boolean compareIds = false)
        {
            if (obj is MDMObject)
            {
                MDMObject objectToBeCompared = obj as MDMObject;

                if (compareIds)
                {
                    if (this.Id != objectToBeCompared.Id)
                        return false;
                }

                if (this.Name != objectToBeCompared.Name)
                    return false;

                if (this.LongName != objectToBeCompared.LongName)
                    return false;

                if (this.Locale != objectToBeCompared.Locale)
                    return false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Represents mdmObject in Xml format
        /// </summary>
        /// <returns>String representation of current mdmObject</returns>
        public virtual String ToXml()
        {
            String mdmObjectXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            // mdmObject Item node start
            xmlWriter.WriteStartElement("MDMObject");

            #region write MDMObject for Full MDMObject Xml

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("NameInLowerCase", this.NameInLowerCase);
            xmlWriter.WriteAttributeString("LongName", this.LongName);
            xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
            xmlWriter.WriteAttributeString("Action", this.Action.ToString());
            xmlWriter.WriteAttributeString("AuditRefId", this.AuditRefId.ToString());
            xmlWriter.WriteAttributeString("ProgramName", this.ProgramName);
            xmlWriter.WriteAttributeString("ReferenceId", this.ReferenceId);

            #endregion write MDMObject for Full MDMObject Xml

            // mdmObject Item node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            mdmObjectXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return mdmObjectXml;
        }

        /// <summary>
        /// Represents mdmObject in Xml format
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>String representation of current mdmObject</returns>
        public virtual String ToXml(ObjectSerialization objectSerialization)
        {
            String mdmObjectXml = String.Empty;

            //TODO:: Write logic as per the different object serialization type whenever needed..
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            if (objectSerialization == ObjectSerialization.DataStorage)
            {
                // mdmObject Item node start
                xmlWriter.WriteStartElement("MDMObject");

                #region write MDMObject for Full MDMObject Xml

                xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                xmlWriter.WriteAttributeString("Name", this.Name);
                xmlWriter.WriteAttributeString("LongName", this.LongName);

                xmlWriter.WriteStartElement("ExtendedProperties");

                foreach (String extendedProperty in this.ExtendedProperties.Split('#'))
                {
                    //ExtendedProperty node start
                    xmlWriter.WriteStartElement("ExtendedProperty");

                    String[] extendedPropertyArray = extendedProperty.Split(',');
                    xmlWriter.WriteAttributeString("Name", extendedPropertyArray[0]);
                    xmlWriter.WriteAttributeString("Value", extendedPropertyArray[1]);

                    //ExtendedProperty node end
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();

                #endregion write MDMObject for Full MDMObject Xml

                // mdmObject Item node end
                xmlWriter.WriteEndElement();
            }
            //ObjectSerialization.External is used for Export Profile
            else if (objectSerialization == ObjectSerialization.External)
            {

                // mdmObject Item node start
                xmlWriter.WriteStartElement("MDMObject");

                #region write MDMObject for Full MDMObject Xml

                xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                xmlWriter.WriteAttributeString("Name", this.Name);
                xmlWriter.WriteAttributeString("NameInLowerCase", this.NameInLowerCase);
                xmlWriter.WriteAttributeString("LongName", this.LongName);
                xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
                xmlWriter.WriteAttributeString("Action", this.Action.ToString());
                xmlWriter.WriteAttributeString("AuditRefId", this.AuditRefId.ToString());
                xmlWriter.WriteAttributeString("ProgramName", this.ProgramName);
                xmlWriter.WriteAttributeString("ReferenceId", this.ReferenceId);

                if (this.ExtendedProperties != null)
                {
                    xmlWriter.WriteStartElement("ExtendedProperties");
                    foreach (String extendedProperty in this.ExtendedProperties.Split('#'))
                    {
                        //ExtendedProperty node start
                        xmlWriter.WriteStartElement("ExtendedProperty");

                        String[] extendedPropertyArray = extendedProperty.Split(',');
                        xmlWriter.WriteAttributeString("Name", extendedPropertyArray[0]);
                        xmlWriter.WriteAttributeString("Value", extendedPropertyArray[1]);

                        //ExtendedProperty node end
                        xmlWriter.WriteEndElement();
                    }
                    xmlWriter.WriteEndElement();
                }


                #endregion write MDMObject for Full MDMObject Xml

                // mdmObject Item node end
                xmlWriter.WriteEndElement();
            }
            else
            {
                mdmObjectXml = this.ToXml();
            }

            xmlWriter.Flush();

            //Get the actual XML
            mdmObjectXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return mdmObjectXml;
        }

        #endregion

        #region Methods

        #region Public Methods
        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Loads the mdmObject with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        private void LoadMDMObject(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            #region Sample Xml

            /*
             *  <MDMObject Id="" Name="" NameInLowerCase="" LongName="" Locale="en_WW" Action="" AuditRefId="" ProgramName="" ReferenceId=""/>
             */

            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                if (objectSerialization == ObjectSerialization.DataStorage)
                {
                    LoadMDMObjectDataStorage(valuesAsXml);
                }
                else
                {
                    XmlTextReader reader = null;

                    try
                    {
                        reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                        while (!reader.EOF)
                        {
                            if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMObject")
                            {
                                #region Read MDMObject

                                if (reader.HasAttributes)
                                {
                                    if (reader.MoveToAttribute("Id"))
                                    {
                                        this.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.Id);
                                    }

                                    if (reader.MoveToAttribute("Name"))
                                    {
                                        this.Name = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("NameInLowerCase"))
                                    {
                                        this.NameInLowerCase = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("LongName"))
                                    {
                                        this.LongName = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("Locale"))
                                    {
                                        String strLocale = reader.ReadContentAsString();
                                        LocaleEnum locale = LocaleEnum.UnKnown;
                                        Enum.TryParse<LocaleEnum>(strLocale, true, out locale);

                                        if (locale == LocaleEnum.UnKnown)
                                            throw new ArgumentOutOfRangeException("Locale Id is out of range. Please verify provided locale and try again.");

                                        this.Locale = locale;
                                    }

                                    if (reader.MoveToAttribute("Action"))
                                    {
                                        this.Action = ValueTypeHelper.GetAction(reader.ReadContentAsString());
                                    }

                                    if (reader.MoveToAttribute("AuditRefId"))
                                    {
                                        this.AuditRefId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this.AuditRefId);
                                    }

                                    if (reader.MoveToAttribute("ProgramName"))
                                    {
                                        this.ProgramName = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("ReferenceId"))
                                    {
                                        this.ReferenceId = reader.ReadContentAsString();
                                    }
                                }
                                else
                                {
                                    reader.Read();
                                }

                                #endregion
                            }
                            else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExtendedProperties")
                            {
                                String attributeXml = reader.ReadInnerXml();
                                if (!String.IsNullOrWhiteSpace(attributeXml))
                                {
                                    this.ExtendedProperties = LoadExtendedProperties(attributeXml);
                                }
                            }
                            else
                            {
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
        }

        /// <summary>
        /// Loads the mdmObject with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        private void LoadMDMObjectDataStorage(String valuesAsXml)
        {
            #region Sample Xml

            /*
             *  <MDMObject Id="" Name="" LongName="">
				    <ExtendedProperties>
					    <ExtendedProperty Name="Position" Value="0" />
				    </ExtendedProperties>
			    </MDMObject>
             */

            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMObject")
                        {
                            #region Read MDMObject

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.Id);
                                }

                                if (reader.MoveToAttribute("Name"))
                                {
                                    this.Name = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("LongName"))
                                {
                                    this.LongName = reader.ReadContentAsString();
                                }

                            }
                            else
                            {
                                reader.Read();
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExtendedProperties")
                        {
                            this.ExtendedProperties = reader.ReadOuterXml();
                        }
                        else
                        {
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
        /// <summary>
        /// method used for loading extended properties
        /// </summary>
        /// <param name="valuesAsXml">xml as input </param>
        /// <param name="objectSerialization">objectserialization</param>
        /// <returns>extendedproperty string</returns>
        private String LoadExtendedProperties(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            StringBuilder extendedProperties = new StringBuilder();
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExtendedProperty")
                        {
                            if (reader.HasAttributes)
                            {
                                String name = String.Empty;
                                String value = String.Empty;
                                if (reader.MoveToAttribute("Name"))
                                {
                                    name = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("Value"))
                                {
                                    value = reader.ReadContentAsString();
                                }
                                if (!String.IsNullOrWhiteSpace(extendedProperties.ToString()))
                                {
                                    extendedProperties.Append("#" + name + "," + value);
                                }
                                else
                                {
                                    extendedProperties.Append(name + "," + value);
                                }
                            }
                            else
                            {
                                reader.Read();
                            }
                        }
                        else
                        {
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
            return extendedProperties.ToString();
        }
        #endregion Private Methods

        #endregion
    }
}