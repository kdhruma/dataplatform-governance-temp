using System;
using System.Text;
using System.Xml;
using System.IO;
using System.ComponentModel;
using System.Reflection;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.ComponentModel.Design;
using System.Runtime.Serialization;
using System.Windows.Forms;
using RS.MDM.Validations;
using RS.MDM.Configuration;
using System.Xml.Xsl;
using System.Collections.ObjectModel;
using MDM.BusinessObjects;
using MDM.Core;
using MDM.MessageManager.Business;
using MDM.Utility;
using ILocalizable = MDM.Interfaces.ILocalizable;
using MDM.Interfaces;

namespace RS.MDM
{
    /// <summary>
    /// Supports all classes in the MDM Framework class hierarchy and provides low-level services to derived classes. This is the ultimate base class of all classes in the MDM Framework; it is the root of the type hierarchy.
    /// </summary>
    [DataContract(Namespace = "http://Riversand.MDM.Services"), Serializable()]
    [DefaultProperty("Name")]
    public abstract class Object : IComponent, ISite, RS.MDM.ComponentModel.Design.IMenuCommandService, ICustomTypeDescriptor, ILocalizable, IObject
    {

        #region Fields

        /// <summary>
        /// Field that denotes the Id of the object
        /// </summary>
        private int _id = -1;

        /// <summary>
        /// Field that denotes the unique identifier of the object in the external system
        /// </summary>
        private string _uniqueIdentifier = System.Guid.NewGuid().ToString();

        /// <summary>
        /// field for name
        /// </summary>
        private string _name = string.Empty;

        /// <summary>
        /// field that denotes the ShortName of the object
        /// </summary>
        private string _shortName = string.Empty;

        /// <summary>
        /// field that denotes the LongName of the object
        /// </summary>
        private string _longName = string.Empty;

        /// <summary>
        /// field that denotes the Path of the object
        /// </summary>
        private string _path = string.Empty;

        /// <summary>
        /// field for tag
        /// </summary>
        private string _tag = string.Empty;

        /// <summary>
        /// field for description
        /// </summary>
        private string _description = string.Empty;

        /// <summary>
        /// field for the verb collection
        /// </summary>
        private System.ComponentModel.Design.DesignerVerbCollection _designerVerbs = new System.ComponentModel.Design.DesignerVerbCollection();

        /// <summary>
        /// field for the parent(Container)
        /// </summary>
        private RS.MDM.Object _parent = null;

        /// <summary>
        /// field for the inherited parent (instance)
        /// </summary>
        private RS.MDM.Object _inheritedParent = null;

        /// <summary>
        /// field for the inherited parent's unique identifier
        /// </summary>
        private string _inheritedParentUId = string.Empty;

        /// <summary>
        /// field for the changes of this object compared to the inherited parent
        /// </summary>
        private PropertyChangeCollection _propertyChanges = new PropertyChangeCollection();

        /// <summary>
        /// field for holding security princiapl
        /// </summary>
        private SecurityPrincipal _securityPrincipal;

        private LocaleEnum _uiLocale = LocaleEnum.UnKnown;

        #endregion

        #region Events

        /// <summary>
        /// field for the NameChangedEventHandler
        /// </summary>
        public event NameChangedEventHandler NameChangedEvent;

        public static void Object_Disposed(object sender, EventArgs args)
        {
            //this is just to avoid warning
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public Object()
            : base()
        {
            this.AddVerb("Generate New GUIDs");
            this.AddVerb("Save File");
            this.AddVerb("Read File");
            this.AddVerb("Show XML");
            this.AddVerb("Show WCF XML");
            this.AddVerb("Load XML");
            this.AddVerb("Inherit");
            this.AddVerb("Find Changes");
            this.AddVerb("Accept Changes"); 
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Id of an Object</param>
        public Object(int id)
            : this()
        {
            this._id = id;
        }

        /// <summary>
        /// Constructor with Id and Name as input parameters
        /// </summary>
        /// <param name="id">Indicates the Id of an Object</param>
        /// <param name="name">Indicates the Name of an Object</param>
        public Object(int id, string name)
            : this(id)
        {
            this._name = name;
        }

        /// <summary>
        /// Constructor with Id, Name and Description as input parameters
        /// </summary>
        /// <param name="id">Indicates the Id of an Object</param>
        /// <param name="name">Indicates the Name of an Object</param>
        /// <param name="description">Indicates the Description of an Object</param>
        public Object(int id, string name, string description)
            : this(id, name)
        {
            this._description = description;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Indicates the Identity of an Object
        /// </summary>
        [DataMember]
        [Description("Indicates the Identity of an Object")]
        [Category("Identity")]
        [XmlAttribute("Id")]
        [TrackChanges()]
        public int Id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }

        /// <summary>
        /// Indicates the Unique Identifier of an object in the external system
        /// </summary>
        [Description("Indicates the Unique Identifier of an object in the external system")]
        [Category("Identity")]
        [XmlAttribute("UniqueIdentifier")]
        [TrackChanges()]
        public string UniqueIdentifier
        {
            get
            {
                return this._uniqueIdentifier;
            }
            set
            {
                this._uniqueIdentifier = value;
            }
        }

        /// <summary>
        /// Indicates the Name of an Object
        /// </summary>
        [DataMember]
        [Description("Indicates the Name of an Object")]
        [Category("Identity")]
        [XmlAttribute("Name")]
        [TrackChanges()]
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                if (this.NameChangedEvent != null)
                {
                    string _oldValue = this.Name;
                    string _newValue = value;
                    this.NameChangedEvent.Invoke(this, new NameChangedEventArgs(_oldValue, _newValue));
                }
                this._name = value;
            }
        }

        /// <summary>
        /// Indicates the ShortName of an Object
        /// </summary>
        [XmlIgnore()]
        [Browsable(false)]
        public string ShortName
        {
            get
            {
                return this._shortName;
            }
            set
            {
                this._shortName = value;
            }
        }

        /// <summary>
        /// Indicates the LongName of an Object
        /// </summary>
        [XmlIgnore()]
        [Browsable(false)]
        public string LongName
        {
            get
            {
                return this._longName;
            }
            set
            {
                this._longName = value;
            }
        }

        /// <summary>
        /// Indicates the UI Locale of an Object
        /// </summary>
        [XmlIgnore()]
        [Browsable(false)]
        public LocaleEnum UILocale
        {
            get
            {
                return this._uiLocale;
            }
            set
            {
                this._uiLocale = value;
            }
        }

        /// <summary>
        /// Indicates the Path of an Object
        /// </summary>
        [XmlIgnore()]
        [Browsable(false)]
        public string Path
        {
            get
            {
                return this._path;
            }
            set
            {
                this._path = value;
            }
        }

        /// <summary>
        /// Indicates the Description of an Object
        /// </summary>
        [DataMember]
        [Description("Indicates the Description of an Object")]
        [Category("Identity")]
        [Editor(typeof(RS.MDM.ComponentModel.Design.StringTypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [TrackChanges()]
        [XmlAttribute("Description")]
        public string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
            }
        }

        /// <summary>
        /// Indicates the Tag of an Object
        /// </summary>
        [Description("Indicates the Tag of an Object")]
        [Category("Identity")]
        [XmlAttribute("Tag")]
        [Browsable(false)]
        public string Tag
        {
            get
            {
                return this._tag;
            }
            set
            {
                this._tag = value;
            }
        }

        /// <summary>
        /// Indicates the Class Name of an Object
        /// </summary>
        [Description("Indicates the Class Name of an Object")]
        [Category("Identity")]
        [XmlAttribute("ClassName")]
        [ReadOnly(true)]
        [Browsable(false)]
        public string ClassName
        {
            get
            {
                return this.GetType().FullName;
            }
            set
            {
            }
        }

        /// <summary>
        /// Indicates the Assembly Name of an Object
        /// </summary>
        [Description("Indicates the Assembly Name of an Object")]
        [Category("Identity")]
        [XmlAttribute("AssemblyName")]
        [ReadOnly(true)]
        [Browsable(false)]
        public string AssemblyName
        {
            get
            {
                return this.GetType().Assembly.Location.Substring(this.GetType().Assembly.Location.LastIndexOf(@"\") + 1);
            }
            set
            {
            }
        }

        /// <summary>
        /// Indicates the Parent (Container) of an Object
        /// </summary>
        [XmlIgnore()]
        [Description("Indicates the container of an Object")]
        [ReadOnly(true)]
        [Category("Container")]
        public RS.MDM.Object Parent
        {
            get
            {
                return this._parent;
            }
            set
            {
                this._parent = value;
            }
        }

        /// <summary>
        /// Indicates the inherited parent (instance) of an Object
        /// </summary>
        [XmlIgnore()]
        [Description("Indicates the inherited parent of an Object")]
        [Category("Inheritance")]
        [ReadOnly(true)]
        public RS.MDM.Object InheritedParent
        {
            get
            {
                return this._inheritedParent;
            }
            set
            {
                this._inheritedParent = value;
            }
        }

        /// <summary>
        /// Indicates the Unique Identifier of inherited parent of an Object
        /// </summary>
        [Description("Indicates the Unique Identifier of inherited parent of an Object")]
        [Category("Inheritance")]
        [XmlAttribute("InheritedParentUId")]
        [ReadOnly(true)]
        [Browsable(false)]
        public string InheritedParentUId
        {
            get
            {
                if (this._inheritedParent != null)
                {
                    return this._inheritedParent.UniqueIdentifier;
                }
                return this._inheritedParentUId;
            }
            set
            {
                this._inheritedParentUId = value;
            }
        }

        /// <summary>
        /// Indicates the changes to the object wrt the Inherited Parent
        /// </summary>
        [Description("Indicates the changes to the object wrt the Inherited Parent")]
        [Category("Inheritance")]
        [XmlElement("PropertyChanges")]
        [TypeConverter(typeof(RS.MDM.ComponentModel.StringExpandableObjectConvertor))]
        public PropertyChangeCollection PropertyChanges
        {
            get
            {
                return this._propertyChanges;
            }
            set
            {
                this._propertyChanges = value;
            }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Gets the Html representation of a given RS.MDM.Object
        /// </summary>
        /// <param name="value">Indicates the Object for which Html representation needs to be computed</param>
        /// <returns>A html string representation of a given RS.MDM.Object</returns>
        public static string GetHtml(RS.MDM.Object value)
        {
            try
            {
                String result = String.Empty;
                XslCompiledTransform _xslCompiledTransform = new XslCompiledTransform();
                RS.MDM.IO.StringWriter _stringWriter = new RS.MDM.IO.StringWriter(System.Text.Encoding.UTF8);
                XmlReader _xslReader = XmlReader.Create(new System.IO.StringReader(RS.MDM.Properties.Resources.Xml2HtmlStyleSheet));
                XmlReader _xmlReader = XmlReader.Create(new System.IO.StringReader(RS.MDM.Object.XMLSerialize(value)));
                _xslCompiledTransform.Load(_xslReader);
                _xslCompiledTransform.Transform(_xmlReader, null, _stringWriter);

                result = _stringWriter.ToString();

                _xslReader.Close();
                _xmlReader.Close();
                _stringWriter.Close();

                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.Write(ex.ToString(), RS.MDM.Logging.LogLevel.ERROR.ToString());
            }

            return null;
        }

        /// <summary>
        /// Serializes an Object into XML using WCF's DataContractSerializer
        /// </summary>
        /// <param name="value">Indicates the object to be serialized</param>
        public static string WCFSerialize(RS.MDM.Object value)
        {
            DataContractSerializer _serializer = new DataContractSerializer(value.GetType());
            System.Text.StringBuilder _sb = new StringBuilder();

            try
            {
                using (System.Xml.XmlTextWriter _writer = new System.Xml.XmlTextWriter(new System.IO.StringWriter(_sb)))
                {
                    _serializer.WriteObject(_writer, value);
                    _writer.Flush();
                    _writer.Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("The following exception has occurred:\n" + ex.Message, "Application Configuration Tool", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return _sb.ToString();
        }

        /// <summary>
        /// Deserializes xml string to an object using WCF's DataContractSerializer
        /// </summary>
        /// <param name="xmlValue">Indicates the serialized object in xml format</param>
        /// <param name="type">Indicates the type of object to be deserialized</param>
        /// <returns>The Deserialized Object</returns>
        public static RS.MDM.Object WCFDeserialize(string xmlValue, Type type)
        {
            RS.MDM.Object _returnValue = null;
            DataContractSerializer _serializer = new DataContractSerializer(type);
            using (System.Xml.XmlTextReader _reader = new System.Xml.XmlTextReader(new System.IO.StringReader(xmlValue)))
            {
                _returnValue = ((RS.MDM.Object)_serializer.ReadObject(_reader));
                _reader.Close();
            }
            return _returnValue;
        }

        /// <summary>
        /// Serializes an Object into XML using XML Serializer
        /// </summary>
        /// <param name="value">Indicates the object to be serialized</param>
        public static string XMLSerialize(RS.MDM.Object value)
        {
            return XMLSerialize(value, Encoding.UTF8);
        }

        /// <summary>
        /// Serializes an Object into XML using XML Serializer
        /// </summary>
        /// <param name="value">Indicates the object to be serialized</param>
        /// <param name="encoding">Indicates the encoding of the returned string</param>
        public static string XMLSerialize(RS.MDM.Object value, Encoding encoding)
        {
            System.Xml.Serialization.XmlSerializer _serializer = new System.Xml.Serialization.XmlSerializer(value.GetType());
            System.Text.StringBuilder _sb = new StringBuilder();
            using (System.Xml.XmlTextWriter _writer = new System.Xml.XmlTextWriter(new RS.MDM.IO.StringWriter(_sb, encoding)))
            {
                _serializer.Serialize(_writer, value);
                _writer.Flush();
                _writer.Close();
            }
            return _sb.ToString();
        }

        /// <summary>
        /// Deserializes xml string to an object using XML Serializer
        /// </summary>
        /// <param name="xmlValue">Indicates the serialized object in xml format</param>
        /// <param name="type">Indicates the type of object to be deserialized</param>
        /// <returns>The Deserialized Object</returns>
        public static RS.MDM.Object XMLDeserialize(string xmlValue, Type type)
        {
            RS.MDM.Object _returnValue = null;
            System.Xml.Serialization.XmlSerializer _serializer = new System.Xml.Serialization.XmlSerializer(type);
            using (System.Xml.XmlTextReader _reader = new System.Xml.XmlTextReader(new System.IO.StringReader(xmlValue)))
            {
                _returnValue = ((RS.MDM.Object)_serializer.Deserialize(_reader));
                _reader.Close();
            }
            return _returnValue;
        }

        /// <summary>
        /// Deserializes an Object from xml string
        /// </summary>
        /// <param name="xmlValue">Indicates xml string that needs to be serialized</param>
        /// <returns>A Deserialzed object of a type that is derived from RS.MDM.Object</returns>
        public static RS.MDM.Object XMLDeserialize(string xmlValue)
        {
            if (string.IsNullOrEmpty(xmlValue))
            {
                throw new ArgumentNullException("xmlValue");
            }
            System.Xml.XmlDocument _xmlDoc = new System.Xml.XmlDocument();
            try
            {
                _xmlDoc.LoadXml(xmlValue);
                string _className = _xmlDoc.DocumentElement.GetAttribute("ClassName");
                string _assemblyName = _xmlDoc.DocumentElement.GetAttribute("AssemblyName");

                if (!String.IsNullOrWhiteSpace(_assemblyName) && !String.IsNullOrWhiteSpace(_className))
                {
                    if (System.Web.HttpContext.Current != null)
                    {
                        _assemblyName = System.Web.HttpContext.Current.Request.ServerVariables["APPL_PHYSICAL_PATH"].ToString() + @"bin\" + _assemblyName;
                    }
                    else
                    {
                        _assemblyName = System.AppDomain.CurrentDomain.BaseDirectory + _assemblyName;
                        if (!System.IO.File.Exists(_assemblyName))
                        {
                            _assemblyName = System.AppDomain.CurrentDomain.BaseDirectory + @"bin\" + _xmlDoc.DocumentElement.GetAttribute("AssemblyName");
                        }
                    }
                    System.Reflection.Assembly _assembly = System.Reflection.Assembly.LoadFrom(_assemblyName);
                    if (_assembly != null)
                    {
                        Type _type = _assembly.GetType(_className);
                        if (_type != null)
                        {
                            return XMLDeserialize(xmlValue, _type);
                        }
                    }
                }
            }
            finally
            {
                _xmlDoc = null;
            }
            return null;
        }

        /// <summary>
        /// Deserialzes an inherited object (instance) from a given DataSet
        /// </summary>
        /// <param name="dataSet">Indicates a DataSet that contains the ConfigXML for each object in the inheritance path</param>
        /// <returns>A Deserialzed object of a type that is derived from RS.MDM.Object</returns>
        public static RS.MDM.Object XMLDeserialize(System.Data.DataSet dataSet)
        {
            List<System.Data.DataRow> _dataRows = new List<System.Data.DataRow>();
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                return XMLDeserialize(dataSet.Tables[0]);
            }
            return null;
        }

        /// <summary>
        /// Deserialzes an inherited object (instance) from a given DataTable
        /// </summary>
        /// <param name="dataTable">Indicates a DataTable that contains the ConfigXML for each object in the inheritance path</param>
        /// <returns>A Deserialzed object of a type that is derived from RS.MDM.Object</returns>
        public static RS.MDM.Object XMLDeserialize(System.Data.DataTable dataTable)
        {
            List<System.Data.DataRow> _dataRows = new List<System.Data.DataRow>();
            if (dataTable != null)
            {
                foreach (System.Data.DataRow _dataRow in dataTable.Rows)
                {
                    _dataRows.Add(_dataRow);
                }
            }
            return XMLDeserialize(_dataRows);
        }

        /// <summary>
        /// Deserialzes an inherited object (instance) from a given set of dataRows
        /// </summary>
        /// <param name="dataRows">Indicates a List of DataRows that contain the ConfigXML for each object in the inheritance path</param>
        /// <returns>A Deserialzed object of a type that is derived from RS.MDM.Object</returns>
        public static RS.MDM.Object XMLDeserialize(List<System.Data.DataRow> dataRows)
        {
            List<string> _configXMLs = new List<string>();
            foreach (System.Data.DataRow _dataRow in dataRows)
            {
                _configXMLs.Add(_dataRow["ConfigXML"].ToString());
            }
            return XMLDeserialize(_configXMLs.ToArray());
        }

        /// <summary>
        /// Deserialzes an inherited object (instance) from a given set of serialized objects
        /// </summary>
        /// <param name="xmlValues">Indicates an array of serialized objects in xml format</param>
        /// <returns>A Deserialzed object of a type that is derived from RS.MDM.Object</returns>
        public static RS.MDM.Object XMLDeserialize(string[] xmlValues)
        {
            Dictionary<string, RS.MDM.Object> _objects = new Dictionary<string, RS.MDM.Object>();
            Dictionary<string, string> _relationships = new Dictionary<string, string>();
            RS.MDM.Object _rootObject = null;
            RS.MDM.Object _object = null;
            if (xmlValues != null && xmlValues.Length > 0)
            {
                foreach (string _xmlValue in xmlValues)
                {
                    if (!string.IsNullOrEmpty(_xmlValue))
                    {
                        try
                        {
                            _object = RS.MDM.Object.XMLDeserialize(_xmlValue);
                            if (_object != null)
                            {
                                if (!_objects.ContainsKey(_object.UniqueIdentifier))
                                {
                                    _object.SetParent();
                                    _objects.Add(_object.UniqueIdentifier, _object);
                                    _relationships.Add(_object.InheritedParentUId, _object.UniqueIdentifier);
                                    if (string.IsNullOrEmpty(_object.InheritedParentUId))
                                    {
                                        if (_rootObject == null)
                                        {
                                            _rootObject = _object;
                                        }
                                        else
                                        {
                                            throw new Exception(string.Format("Too many objects ('{0}','{1}') in the collection are root level parents.", _object.Name, _rootObject.Name));
                                        }
                                    }
                                }
                                else
                                {
                                    throw new Exception(string.Format("The collection contails multiple objects ('{0}','{1}') with the same unique Idendifier", _object.Name, _objects[_object.UniqueIdentifier].Name));
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Trace.WriteLine(ex.ToString(), RS.MDM.Logging.LogLevel.ERROR.ToString());
                        }
                    }
                }
                if (_rootObject != null)
                {
                    RS.MDM.Object _childObject = _rootObject;
                    if (_objects.Count > 1)
                    {
                        RS.MDM.Object _parentObject = _rootObject;
                        // get the child UID from relationships
                        string _inheritedChildUId = _relationships[_childObject.UniqueIdentifier];
                        while (!string.IsNullOrEmpty(_inheritedChildUId))
                        {
                            //clone the existing child so that it can be transformed into grandchild
                            _childObject = RS.MDM.Object.Clone(_childObject, false);
                            //transform the child into grandchild
                            _objects[_inheritedChildUId].InheritParent(_childObject);
                            //set the inherited parent to the grandchild
                            _childObject.InheritedParent = _parentObject;
                            //set all the parents to all the objects aggregated by the grandchild
                            _childObject.SetParent();

                            //If _childObject.UniqueIdentifier equals to parentObject.UniqueIdentifier throw error.
                            if (_childObject.UniqueIdentifier == _parentObject.UniqueIdentifier)
                            {
                                throw new InvalidDataException("Child object missing correct property changes for the UniqueIdentifier.");
                            }

                            //set the parent for the next iteration
                            _parentObject = _childObject;
                            //set the child UID for the next iteration
                            _inheritedChildUId = string.Empty;
                            if (_relationships.ContainsKey(_childObject.UniqueIdentifier))
                            {
                                _inheritedChildUId = _relationships[_childObject.UniqueIdentifier];
                            }
                        }
                    }
                    return _childObject;
                }
                else
                {
                    if (_objects.Count > 0)
                    {
                        Dictionary<string, RS.MDM.Object>.Enumerator _enumerator = _objects.GetEnumerator();
                        _enumerator.MoveNext();
                        return _enumerator.Current.Value;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Serializes an object to xml string
        /// </summary>
        /// <param name="value">Indicates an object that needs to be serialized</param>
        /// <param name="indentation">Indicates indent size for the serialized xml string</param>
        /// <returns>Xml string that represents the serialized object</returns>
        public static string XMLSerialize(RS.MDM.Object value, uint indentation)
        {
            return XMLSerialize(value, Encoding.UTF8, indentation);
        }

        /// <summary>
        /// Serializes an object to xml string
        /// </summary>
        /// <param name="value">Indicates an object that needs to be serialized</param>
        /// <param name="indentation">Indicates indent size for the serialized xml string</param>
        /// <param name="encoding">Indicates the encoding of the returned string</param>
        /// <returns>Xml string that represents the serialized object</returns>
        public static string XMLSerialize(RS.MDM.Object value, Encoding encoding, uint indentation)
        {
            string _xml = XMLSerialize(value, encoding);
            if (!string.IsNullOrEmpty(_xml))
            {
                if (indentation > 0)
                {
                    return IndentXML(_xml, indentation);
                }
            }
            return _xml;
        }

        /// <summary>
        /// Returns a list of ListViewItems that represent validation errors
        /// </summary>
        /// <returns></returns>
        public static System.Windows.Forms.ListViewItem[] GetValidationErrors(ValidationErrorCollection validationErrors)
        {
            return null;
        }

        /// <summary>
        /// Indents xml for a given xml string and indentation
        /// </summary>
        /// <param name="xml">Indicates xml string that needs to be indented</param>
        /// <param name="indentation">Indicates the indentation value that needs to be applied on the xml string</param>
        /// <returns>Xml string that is indented</returns>
        public static string IndentXML(string xml, uint indentation)
        {
            string IndentXML = string.Empty;
            XmlTextReader reader = null;
            StreamReader sr = null;
            XmlTextWriter writer = null;
            try
            {
                MemoryStream mem = new MemoryStream();
                reader = new XmlTextReader(xml, XmlNodeType.Document, null);
                reader.WhitespaceHandling = WhitespaceHandling.None;
                writer = new XmlTextWriter(mem, Encoding.UTF8);
                writer.Formatting = Formatting.Indented;
                writer.Indentation = (int)indentation;
                writer.WriteNode(reader, false);
                writer.Flush();
                mem.Position = 0L;
                sr = new StreamReader(mem);
                IndentXML = sr.ReadToEnd();
            }
            catch
            {
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                    writer = null;
                }
                if (reader != null)
                {
                    reader.Close();
                    reader = null;
                }
                if (sr != null)
                {
                    sr.Close();
                    sr = null;
                }
            }
            return IndentXML;
        }

        /// <summary>
        /// Returns a localized string for a give name
        /// </summary>
        /// <param name="text">Indicates the text that needs to be localized </param>
        /// <returns>A localized string</returns>
        public static string GetLocalizedString(string text)
        {
            return Riversand.Localization.GetString(Riversand.Localization.Modules.Web, text, System.Threading.Thread.CurrentThread.CurrentUICulture);
        }

        /// <summary>
        /// Clones an Object
        /// </summary>
        /// <param name="objectToClone">Indicates an Object that needs to be cloned</param>
        /// <param name="inherit">Indicates if the changes needs to be cleaned after cloning</param>
        /// <returns></returns>
        public static RS.MDM.Object Clone(RS.MDM.Object objectToClone, bool inherit)
        {
            RS.MDM.Object _clonedObject = null;
            string _xmlValue = RS.MDM.Object.XMLSerialize(objectToClone);
            _clonedObject = RS.MDM.Object.XMLDeserialize(_xmlValue);
            if (inherit)
            {
                _clonedObject.InheritedParent = objectToClone;
                _clonedObject.SetParent();
                _clonedObject.UniqueIdentifier = System.Guid.NewGuid().ToString();
            }
            else
            {
                //Set InheritedParent(Not serializable) as of the objectToClone's InheritedParent.
                _clonedObject.InheritedParent = objectToClone.InheritedParent;
                //Set Parent(Not serializable) as of the objectToClone's Parent.
                _clonedObject.Parent = objectToClone.Parent;
                _clonedObject.SetParent();
            }
            return _clonedObject;
        }

        #endregion

        #region IDisposable Interface

        // Track whether Dispose has been called.
        /// <summary>
        /// 
        /// </summary>
        bool disposed;

        // Implement IDisposable.
        // Do not make this method virtual.
        // A derived class should not be able to override this method.
        /// <summary>
        /// Cleans up the object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the
        // runtime from inside the finalizer and you should not reference
        // other objects. Only unmanaged resources can be disposed.
        /// <summary>
        /// Cleans up Managed and Unmanaged resources based on the parameter 'disposing'
        /// </summary>
        /// <param name="disposing">Indicates if the user code is calling it.</param>
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                }

                // Call the appropriate methods to clean up
                // unmanaged resources here.
                // If disposing is false,
                // only the following code is executed.

                // Note disposing has been done.
                disposed = true;
            }
        }

        // Use C# destructor syntax for finalization code.
        // This destructor will run only if the Dispose method
        // does not get called.
        // It gives your base class the opportunity to finalize.
        // Do not provide destructors in types derived from this class.
        /// <summary>
        /// Allows an Object to attempt to free resources and perform other cleanup operations before the Object is reclaimed by garbage collection.
        /// </summary>
        ~Object()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }

        #endregion

        #region IComponent Members

        /// <summary>
        /// An event that is fired after an Object is disposed
        /// </summary>
        public event EventHandler Disposed = new EventHandler(Object_Disposed);

        /// <summary>
        /// Gets or sets the System.ComponentModel.ISite associated with the System.ComponentModel.IComponent.
        /// </summary>
        [Browsable(false)]
        [System.Xml.Serialization.XmlIgnore()]
        public ISite Site
        {
            get
            {
                return this;
            }
            set
            {
            }
        }

        #endregion

        #region ISite Members

        /// <summary>
        /// Gets the component associated with the System.ComponentModel.ISite when implemented by a class.
        /// </summary>
        [Browsable(false)]
        [System.Xml.Serialization.XmlIgnore()]
        public IComponent Component
        {
            get { return this; }
        }

        /// <summary>
        /// Gets the System.ComponentModel.IContainer associated with the System.ComponentModel.ISite
        ///     when implemented by a class.
        /// </summary>
        [Browsable(false)]
        [System.Xml.Serialization.XmlIgnore()]
        public System.ComponentModel.IContainer Container
        {
            get { return null; }
        }

        /// <summary>
        /// Determines whether the component is in design mode when implemented by a class.
        /// </summary>
        [Browsable(false)]
        [System.Xml.Serialization.XmlIgnore()]
        public bool DesignMode
        {
            get { return false; }
        }

        #endregion

        #region IServiceProvider Members

        /// <summary>
        /// Gets a Service denoted by a given ServiceType
        /// </summary>
        /// <param name="serviceType">Indicates the ServiceType of a Service</param>
        /// <returns>A Service denoted by the given ServiceType</returns>
        public object GetService(Type serviceType)
        {
            if (serviceType != null && serviceType.ToString() == "System.ComponentModel.Design.IComponentChangeService")
            {
                return null;
            }
            return this;
        }

        #endregion

        #region IMenuCommandService Members

        /// <summary>
        /// Adds a Command to the command collection
        /// </summary>
        /// <param name="command">Indicates a command that needs to be added to the command collection</param>
        public void AddCommand(MenuCommand command)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds the specified designer verb to the set of global designer verbs.
        /// </summary>
        /// <param name="verb">The System.ComponentModel.Design.DesignerVerb to add.</param>
        public void AddVerb(DesignerVerb verb)
        {
            if (!this._designerVerbs.Contains(verb))
            {
                this._designerVerbs.Add(verb);
            }
        }

        /// <summary>
        /// Adds a verb to the verb collection
        /// </summary>
        /// <param name="text">Indicates the text of a verb that needs to be added to the verb collection</param>
        public void AddVerb(string text)
        {
            foreach (DesignerVerb _designerVerb in this._designerVerbs)
            {
                if (_designerVerb.Text == text)
                {
                    return;
                }
            }
            this._designerVerbs.Add(new System.ComponentModel.Design.DesignerVerb(text, new EventHandler(DesignerVerb_Click)));
        }

        /// <summary>
        /// Searches for the specified command ID and returns the menu command associated with it.
        /// </summary>
        /// <param name="commandID">The System.ComponentModel.Design.CommandID to search for.</param>
        /// <returns>The System.ComponentModel.Design.MenuCommand associated with the command
        ///     ID, or null if no command is found.</returns>
        public MenuCommand FindCommand(CommandID commandID)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Invokes a menu or designer verb command matching the specified command ID.
        /// </summary>
        /// <param name="commandID">The System.ComponentModel.Design.CommandID of the command to search for and execute.</param>
        /// <returns>true if the command was found and invoked successfully; otherwise, false.</returns>
        public bool GlobalInvoke(CommandID commandID)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes the specified standard menu command from the menu.
        /// </summary>
        /// <param name="command">The System.ComponentModel.Design.MenuCommand to remove.</param>
        public void RemoveCommand(MenuCommand command)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes the specified designer verb from the collection of global designer verbs.
        /// </summary>
        /// <param name="verb">The System.ComponentModel.Design.DesignerVerb to remove.</param>
        public void RemoveVerb(DesignerVerb verb)
        {
            if (this._designerVerbs.Contains(verb))
            {
                this._designerVerbs.Remove(verb);
            }
        }

        /// <summary>
        /// Removes the specified designer verb from the collection of global designer verbs.
        /// </summary>
        /// <param name="text">The text of the verb that needs to be removed.</param>
        public void RemoveVerb(string text)
        {
            DesignerVerb _verbToRemove = null;
            foreach (DesignerVerb _designerVerb in this._designerVerbs)
            {
                if (_designerVerb.Text == text)
                {
                    _verbToRemove = _designerVerb;
                    break;
                }
            }
            if (_verbToRemove != null)
            {
                this._designerVerbs.Remove(_verbToRemove);
            }
        }

        /// <summary>
        /// Shows the specified shortcut menu at the specified location.
        /// </summary>
        /// <param name="menuID">The System.ComponentModel.Design.CommandID for the shortcut menu to show.</param>
        /// <param name="x">The x-coordinate at which to display the menu, in screen coordinates.</param>
        /// <param name="y">The y-coordinate at which to display the menu, in screen coordinates.</param>
        public void ShowContextMenu(CommandID menuID, int x, int y)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a list of verbs that are supported for an Object
        /// </summary>
        [Browsable(false)]
        [System.Xml.Serialization.XmlIgnore()]
        public DesignerVerbCollection Verbs
        {
            get { return this._designerVerbs; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DesignerVerb_Click(object sender, EventArgs e)
        {
            System.ComponentModel.Design.DesignerVerb _verb = (System.ComponentModel.Design.DesignerVerb)sender;
            this.OnDesignerVerbClick(_verb.Text, null, null);
        }

        /// <summary>
        /// Execute logic related to a given verb
        /// </summary>
        /// <param name="text">Indicate the text that represents a supported verb</param>
        /// <param name="configObject">Indicates configuration object on which action is being performed</param>
        /// <param name="treeView">Indicates tree view of main form</param>
        public virtual void OnDesignerVerbClick(string text, Object configObject, object treeView)
        {
            string _xml = string.Empty;

            switch (text)
            {
                case "Generate New GUIDs":
                    this.GenerateNewUniqueIdentifier();
                    break;
                case "Save File":
                    try
                    {
                        System.Windows.Forms.SaveFileDialog _fileDialog = new System.Windows.Forms.SaveFileDialog();
                        _fileDialog.Title = "Configuration File";
                        _fileDialog.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
                        _fileDialog.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
                        _fileDialog.FilterIndex = 0;
                        _fileDialog.Title = "Select file to save configuration ";
                        if (_fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            _xml = RS.MDM.Object.XMLSerialize(this);
                            System.IO.StreamWriter _streamWriter = new StreamWriter(_fileDialog.FileName);
                            _streamWriter.Write(_xml);
                            _streamWriter.Flush();
                            _streamWriter.Close();
                            System.Diagnostics.Trace.WriteLine("Configuration saved Successfully", RS.MDM.Logging.LogLevel.INFO.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.WriteLine(ex.ToString(), RS.MDM.Logging.LogLevel.ERROR.ToString());
                    }
                    break;
                case "Read File":
                    break;
                case "Show XML":
                    RS.MDM.ComponentModel.Design.StringEditor _stringEditor = new RS.MDM.ComponentModel.Design.StringEditor();
                    _stringEditor.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
                    _stringEditor.Data = RS.MDM.Object.XMLSerialize(this, 4);
                    if (string.IsNullOrEmpty(this.Name))
                    {
                        _stringEditor.Text = this.GetType().Name;
                    }
                    else
                    {
                        _stringEditor.Text = this.Name;
                    }
                    _stringEditor.Tag = configObject;
                    _stringEditor.ShowDialog();
                    break;
                case "Show WCF XML":
                    _stringEditor = new RS.MDM.ComponentModel.Design.StringEditor();
                    _stringEditor.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
                    _stringEditor.Data = RS.MDM.Object.WCFSerialize(this);
                    if (string.IsNullOrEmpty(this.Name))
                    {
                        _stringEditor.Text = this.GetType().Name;
                    }
                    else
                    {
                        _stringEditor.Text = this.Name;
                    }
                    _stringEditor.Tag = configObject;
                    _stringEditor.ShowDialog();
                    break;
                case "Load XML":
                    break;
                case "Inherit":
                    this.AcceptChanges();
                    RS.MDM.Object _clonedObject = RS.MDM.Object.Clone(this, true);
                    RS.MDM.ComponentModel.Design.PropertiesEditor _editor = new RS.MDM.ComponentModel.Design.PropertiesEditor();
                    _editor.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
                    _editor.Parameter = _clonedObject;
                    if (string.IsNullOrEmpty(_clonedObject.Name))
                    {
                        _editor.Text = _clonedObject.GetType().Name + " (Cloned)";
                    }
                    else
                    {
                        _editor.Text = _clonedObject.Name + " (Cloned)";
                    }
                    _editor.Tag = configObject;
                    _editor.ShowDialog();
                    break;
                case "Find Changes":
                    this.FindChanges();
                    break;
                case "Accept Changes":
                    this.AcceptChanges();
                    break;
            }
            System.ComponentModel.TypeDescriptor.Refresh(this);
        }

        #endregion

        #region ICustomTypeDescriptor Members

        /// <summary>
        /// Returns a collection of custom attributes for this instance of a component.
        /// </summary>
        /// <returns></returns>
        public System.ComponentModel.AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        /// <summary>
        /// Returns the class name of this instance of a component.
        /// </summary>
        /// <returns>The class name of the object, or null if the class does not have a name.</returns>
        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        /// <summary>
        /// Returns the name of this instance of a component.
        /// </summary>
        /// <returns>The name of the object, or null if the object does not have a name.</returns>
        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        /// <summary>
        /// Returns a type converter for this instance of a component.
        /// </summary>
        /// <returns>A System.ComponentModel.TypeConverter that is the converter for this object,
        ///     or null if there is no System.ComponentModel.TypeConverter for this object.
        /// </returns>
        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        /// <summary>
        /// Returns the default event for this instance of a component.
        /// </summary>
        /// <returns>An System.ComponentModel.EventDescriptor that represents the default event
        ///     for this object, or null if this object does not have events.</returns>
        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        /// <summary>
        /// Returns the default property for this instance of a component.
        /// </summary>
        /// <returns>A System.ComponentModel.PropertyDescriptor that represents the default property
        ///     for this object, or null if this object does not have properties.</returns>
        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }

        /// <summary>
        /// Returns an editor of the specified type for this instance of a component.
        /// </summary>
        /// <param name="editorBaseType">A System.Type that represents the editor for this object.</param>
        /// <returns>An System.Object of the specified type that is the editor for this object,
        ///     or null if the editor cannot be found.</returns>
        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }


        /// <summary>
        /// Returns the events for this instance of a component.
        /// </summary>
        /// <param name="attributes">An array of type System.Attribute that is used as a filter.</param>
        /// <returns>An System.ComponentModel.EventDescriptorCollection that represents the events
        ///     for this component instance.</returns>
        public EventDescriptorCollection GetEvents(System.Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        /// <summary>
        /// Returns the events for this instance of a component.
        /// </summary>
        /// <returns>An System.ComponentModel.EventDescriptorCollection that represents the events
        ///     for this component instance.</returns>
        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        /// <summary>
        /// Returns the properties for this instance of a component.
        /// </summary>
        /// <param name="attributes">An array of type System.Attribute that is used as a filter.</param>
        /// <returns>A System.ComponentModel.PropertyDescriptorCollection that represents the
        ///     properties for this component instance.</returns>
        public PropertyDescriptorCollection GetProperties(System.Attribute[] attributes)
        {
            PropertyDescriptorCollection _propCol = TypeDescriptor.GetProperties(this, attributes, true);
            return this.GetProperties(_propCol);
        }

        /// <summary>
        /// Returns the properties for this instance of a component.
        /// </summary>
        /// <returns>A System.ComponentModel.PropertyDescriptorCollection that represents the
        ///     properties for this component instance.</returns>
        public PropertyDescriptorCollection GetProperties()
        {
            PropertyDescriptorCollection _propCol = TypeDescriptor.GetProperties(this, true);
            return this.GetProperties(_propCol);
        }



        /// <summary>
        /// Returns an object that contains the property described by the specified property descriptor.
        /// </summary>
        /// <param name="pd">A System.ComponentModel.PropertyDescriptor that represents the property whose
        ///     owner is to be found.</param>
        /// <returns>An System.Object that represents the owner of the specified property.</returns>
        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        #endregion

        #region Virtual Methods

        /// <summary>
        /// Gets the filtered properties of an object
        /// </summary>
        /// <param name="propertyDescriptorCollection">Indicates the superset of properties collection</param>
        /// <returns>A set of filtered properties of an object that needs to be displayed in the designer</returns>
        protected virtual PropertyDescriptorCollection GetProperties(PropertyDescriptorCollection propertyDescriptorCollection)
        {
            PropertyDescriptorCollection _props = new PropertyDescriptorCollection(null);
            foreach (PropertyDescriptor _prop in propertyDescriptorCollection)
            {
                if (_prop != null)
                {
                    switch (_prop.Name)
                    {
                        case "PropertyChanges":
                            if (this.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                            {
                                _props.Add(_prop);
                            }
                            break;
                        default:
                            _props.Add(_prop);
                            break;
                    }
                }
            }
            return _props;
        }

        /// <summary>
        /// Sets the parent (container) for all the aggregated children
        /// </summary>
        public virtual void SetParent()
        {
        }

        /// <summary>
        /// Generates an unique identifier for an object and all the aggregated children
        /// </summary>
        public virtual void GenerateNewUniqueIdentifier()
        {
            this._uniqueIdentifier = System.Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Inherits a parent object (instance)
        /// </summary>
        /// <param name="inheritedParent">Indicates an instance of an object that needs to be inherited</param>
        public virtual void InheritParent(RS.MDM.Object inheritedParent)
        {
            if (inheritedParent != null)
            {
                foreach (PropertyChange _propertyChange in this._propertyChanges.Items)
                {
                    PropertyInfo _propertyInfo = this.GetType().GetProperty(_propertyChange.Name);

                    object _value = null;

                    if (_propertyInfo.PropertyType.IsEnum)
                    {
                        if (!Enum.IsDefined(_propertyInfo.PropertyType, _propertyChange.ChildValue))
                            throw new FormatException("Undefined value for the enum type '" + _propertyInfo.PropertyType.FullName + "'");

                        _value = Enum.Parse(_propertyInfo.PropertyType, _propertyChange.ChildValue);
                    }
                    else    
                        _value = System.Convert.ChangeType(_propertyChange.ChildValue, _propertyInfo.PropertyType);

                    _propertyInfo.SetValue(inheritedParent, _value, null);
                }
                _propertyChanges.ObjectStatus = InheritedObjectStatus.None;
                _propertyChanges.Items.Clear();
            }
        }

        /// <summary>
        /// Finds deleted children of an inherited child
        /// </summary>
        /// <param name="inheritedChild">Indicates the inherited child</param>
        public virtual void FindDeletes(RS.MDM.Object inheritedChild)
        {
        }

        /// <summary>
        /// Accepts the property changes.
        /// </summary>
        public virtual void AcceptChanges()
        {
            this.PropertyChanges.Items.Clear();
            this.PropertyChanges.ObjectStatus = InheritedObjectStatus.None;
        }

        /// <summary>
        /// Finds the changes of an object wrt an instance of an inherited parent
        /// </summary>
        public virtual void FindChanges()
        {
            this.PropertyChanges.Items.Clear();
            RS.MDM.Object _inheritedParentObject = null;

            if (this.InheritedParent != null)
            {
                if (this.Parent == null)
                {
                    _inheritedParentObject = this._inheritedParent;
                }
                else
                {
                    List<RS.MDM.Object> _inheritedParentObjects = this.InheritedParent.FindChildren(this.UniqueIdentifier, false);
                    if (_inheritedParentObjects.Count > 0)
                    {
                        foreach (RS.MDM.Object _object in _inheritedParentObjects)
                        {
                            if (_object != null)
                            {
                                _inheritedParentObject = _object;
                                break;
                            }
                        }
                    }
                }
                if (_inheritedParentObject != null)
                {
                    if (this.PropertyChanges.ObjectStatus != InheritedObjectStatus.Delete)
                    {
                        foreach (PropertyInfo _propertyInfo in this.GetType().GetProperties())
                        {
                            object[] _attributes = _propertyInfo.GetCustomAttributes(typeof(TrackChangesAttribute), false);
                            if (_attributes != null && _attributes.Length > 0)
                            {
                                TrackChangesAttribute _trackChangesAttribute = (TrackChangesAttribute)_attributes[0];
                                if (_trackChangesAttribute.TrackChanges)
                                {
                                    object _thisValue = _propertyInfo.GetValue(this, null);
                                    object _parentValue = _propertyInfo.GetValue(_inheritedParentObject, null);
                                    string _thisStringValue = string.Empty;
                                    string _parentStringValue = string.Empty;
                                    if (_thisValue != null)
                                    {
                                        _thisStringValue = _thisValue.ToString();
                                    }
                                    if (_parentValue != null)
                                    {
                                        _parentStringValue = _parentValue.ToString();
                                    }
                                    if (_thisStringValue != _parentStringValue)
                                    {
                                        this.PropertyChanges.ObjectStatus = InheritedObjectStatus.Change;
                                        this.PropertyChanges.Add(_propertyInfo.Name, _parentStringValue, _thisStringValue);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    this.PropertyChanges.ObjectStatus = InheritedObjectStatus.Add;
                }
            }
        }

        /// <summary>
        /// Finds and returns a list of child objects for a given unique identifier
        /// </summary>
        /// <param name="uniqueIdentifier">Indicates an unique identifier of an object that needs to be found</param>
        /// <param name="includeDeletedItems">Indicates if deleted items to be included in the search</param>
        /// <returns>A list of Child objects matching the given unique identifier</returns>
        public virtual List<RS.MDM.Object> FindChildren(string uniqueIdentifier, bool includeDeletedItems)
        {
            List<RS.MDM.Object> _list = new List<Object>();
            if (this.UniqueIdentifier == uniqueIdentifier)
            {
                if (this.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                {
                    if (includeDeletedItems)
                    {
                        _list.Add(this);
                    }
                }
                else
                {
                    _list.Add(this);
                }
            }
            return _list;
        }

        /// <summary>
        /// Get a tree node that reprents an object and its aggregated children
        /// </summary>
        /// <returns>A Tree Node that represents an object and its aggregated children</returns>
        public virtual TreeNode GetTreeNode()
        {
            TreeNode _treeNode = new TreeNode();
            _treeNode.Tag = this;
            _treeNode.ImageKey = this.PropertyChanges.ObjectStatus.ToString();
            _treeNode.SelectedImageKey = _treeNode.ImageKey;

            if (!string.IsNullOrEmpty(this.LongName.Trim()) && this.Id > 0)
            {
                _treeNode.Text = ((this.Id < 10) ? "0" + this.Id.ToString() : this.Id.ToString()) + " : " + this.LongName;
                _treeNode.ToolTipText = ((this.Id < 10) ? "0" + this.Id.ToString() : this.Id.ToString()) + " : " + this.LongName;
            }
            else if (!string.IsNullOrEmpty(this.Name.Trim()))
            {
                _treeNode.Text = this.Name;
                _treeNode.ToolTipText = this.Name;
            }
            else
            {
                _treeNode.Text = this.GetType().Name;
                _treeNode.ToolTipText = this.GetType().Name;
            }
            _treeNode.Name = _treeNode.Text;

            if (this.PropertyChanges.ObjectStatus != InheritedObjectStatus.None)
            {
                _treeNode.Nodes.Add(this.PropertyChanges.GetTreeNode());
            }
            return _treeNode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="dataRowListList"></param>
        /// <param name="dataRowList"></param>
        /// <param name="dataRow"></param>
        public virtual void GroupConfigurations(System.Data.DataTable dataTable, List<List<System.Data.DataRow>> dataRowListList, List<System.Data.DataRow> dataRowList, System.Data.DataRow dataRow)
        {
            int _applicationConfigId = System.Convert.ToInt32(dataRow["PK_Application_Config"]);
            System.Data.DataRow[] _childDataRows = dataTable.Select("FK_Application_ConfigParent = " + _applicationConfigId);
            if (_childDataRows != null && _childDataRows.Length > 0)
            {
                if (_childDataRows.Length == 1)
                {
                    dataRowList.Add(_childDataRows[0]);
                    this.GroupConfigurations(dataTable, dataRowListList, dataRowList, _childDataRows[0]);
                }
                else if (_childDataRows.Length > 1)
                {
                    List<System.Data.DataRow> _dataRowListClone = new List<System.Data.DataRow>();
                    _dataRowListClone.AddRange(dataRowList);
                    for (int i = 0; i < _childDataRows.Length; i++)
                    {
                        if (i == 0)
                        {
                            dataRowList.Add(_childDataRows[0]);
                            this.GroupConfigurations(dataTable, dataRowListList, dataRowList, _childDataRows[0]);
                        }
                        else
                        {
                            List<System.Data.DataRow> _dataRowList1 = new List<System.Data.DataRow>();
                            dataRowListList.Add(_dataRowList1);
                            _dataRowList1.AddRange(_dataRowListClone);
                            _dataRowList1.Add(_childDataRows[i]);
                            this.GroupConfigurations(dataTable, dataRowListList, _dataRowList1, _childDataRows[i]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get XML representation of the object
        /// </summary>
        /// <returns>XML representation of object</returns>
        public virtual String ToXml()
        {
            //TODO::Convert properties to XML
            return String.Empty;
        }

        public virtual String ToJSON()
        {
            //This method has to be overridden in each inheriting class
            return String.Empty;
        }

        /// <summary>
        /// This method gets the localized text for given message code. 
        /// This returns message in the user preferred locale for current user.
        /// </summary>
        /// <param name="messageCode">message code for which locale message is needed</param>
        /// <param name="defaultMessage">default message to return if given message code is not found</param>
        /// <returns>Localized message as string</returns>
        public virtual String GetLocaleMessage(String messageCode)
        {
            Collection<String> messageCodeList = new Collection<string>();
            messageCodeList.Add(messageCode);

            if (_uiLocale == LocaleEnum.UnKnown)
            {
                if (_securityPrincipal == null)
                {
                    _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
                }

                _uiLocale = (_securityPrincipal.UserPreferences == null || _securityPrincipal.UserPreferences.UILocale == LocaleEnum.UnKnown)
                    ? GlobalizationHelper.GetSystemUILocale() : _securityPrincipal.UserPreferences.UILocale;
            }

            CallerContext callerContext = new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.UIProcess);

            LocaleMessageBL localeMessageBL = new LocaleMessageBL();
            LocaleMessage localeMessage = localeMessageBL.Get(_uiLocale, messageCode, false, callerContext);

            return localeMessage.Message;
        }

        #endregion

        #region Validations

        /// <summary>
        /// Validates an object and aggregates all the validation exceptions
        /// </summary>
        /// <param name="validationErrors">A container to aggregate all the validation exceptions</param>
        public virtual void Validate(ref ValidationErrorCollection validationErrors)
        {
            if (validationErrors == null)
            {
                validationErrors = new RS.MDM.Validations.ValidationErrorCollection();
            }
            if (this._id == -1)
            {
                validationErrors.Add("The Id is not set", ValidationErrorType.Error, "Id", this);
            }
            if (string.IsNullOrEmpty(this._name))
            {
                validationErrors.Add("The Name is not set", ValidationErrorType.Error, "Name", this);
            }
        }

        #endregion

    }
}
