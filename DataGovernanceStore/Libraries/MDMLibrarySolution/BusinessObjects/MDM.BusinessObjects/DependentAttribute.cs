using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Text;

using ProtoBuf;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the Dependent Attribute. 
    /// </summary>
    [DataContract]
    [ProtoContract]
    [KnownType(typeof(MDMObject))]
    public class DependentAttribute : MDMObject, IDependentAttribute
    {
        #region Fields

        /// <summary>
        /// Field denoting Attribute Name
        /// </summary>
        private String _attributeName = String.Empty;

        /// <summary>
        /// Field denoting Parent Attribute Name
        /// </summary>
        private String _parentAttributeName = String.Empty;

        /// <summary>
        /// Field denoting Attribute Id
        /// </summary>
        private Int32 _attributeId = -1;

        /// <summary>
        /// Field denoting Lookup table name
        /// </summary>
        private String _lookupTableName = String.Empty;

        /// <summary>
        /// Field denoting Link table name
        /// </summary>
        private String _linkTableName = String.Empty;

        /// <summary>
        /// Field denoting Link table column
        /// </summary>
        private String _linkTableColumn = String.Empty;

        /// <summary>
        /// Field denoting Attribute value
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        private String _attributeValue = String.Empty;

        /// <summary>
        /// Field denoting the current dependent attribute type whether is child or parent.
        /// </summary>
        private DependencyType _dependencyType = DependencyType.Unknown;

        /// <summary>
        /// Indicates collection of contexts in which the dependency is valid
        /// </summary>
        private ApplicationContextCollection _applicationContexts = new ApplicationContextCollection();

        /// <summary>
        /// Indicates level of dependency. This is used to identify in multilevel dependency, given dependency is at which level.
        /// </summary>
        private Int32 _dependentLevel = -1;

        /// <summary>
        /// Indicates the parent attribute for given dependency
        /// </summary>
        private Int32 _parentAttributeId = -1;

		/// <summary>
        /// Field denoting the current dependent attribute is collection or not.
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        private Boolean _isCollection = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public DependentAttribute()
            : base()
        {
        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public DependentAttribute(String valueAsXml)
        {
            LoadDependentAttribute(valueAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Field denoting Parent Attribute Name
        /// </summary>
        [DataMember]
        [ProtoMember(3)]
        public String ParentAttributeName
        {
            get { return _parentAttributeName; }
            set { _parentAttributeName = value; }
        }
        
        /// <summary>
        /// Property denoting Attribute Name
        /// </summary>
        [DataMember]
        [ProtoMember(4)]
        public String AttributeName
        {
            get
            {
                return this._attributeName;
            }
            set
            {
                this._attributeName = value;
            }
        }

        /// <summary>
        /// Property denoting Attribute Id
        /// </summary>
        [DataMember]
        [ProtoMember(5)]
        public Int32 AttributeId
        {
            get
            {
                return this._attributeId;
            }
            set
            {
                this._attributeId = value;
            }
        }

        /// <summary>
        /// Property denoting Lookup Table Name
        /// </summary>
        [DataMember]
        [ProtoMember(6)]
        public String LookupTableName
        {
            get
            {
                return this._lookupTableName;
            }
            set
            {
                this._lookupTableName = value;
            }
        }

        /// <summary>
        /// Property denoting Link Table Name
        /// </summary>
        [DataMember]
        [ProtoMember(7)]
        public String LinkTableName
        {
            get
            {
                return this._linkTableName;
            }
            set
            {
                this._linkTableName = value;
            }
        }

        /// <summary>
        /// Property denoting Link Table Column
        /// </summary>
        [DataMember]
        [ProtoMember(8)]
        public String LinkTableColumn
        {
            get
            {
                return this._linkTableColumn;
            }
            set
            {
                this._linkTableColumn = value;
            }
        }

        /// <summary>
        /// Property denoting Attribute Value.
        /// If the attribute is collection then the attribute value as the format of 'Value#$#Value'.
        /// The value separated by '$@$'
        /// </summary>
        public String AttributeValue
        {
            get
            {
                return this._attributeValue;
            }
        }

        /// <summary>
        /// Property denoting whether this dependent attribute is child or Parent.
        /// </summary>
        [DataMember]
        [ProtoMember(9)]
        public DependencyType DependencyType
        {
            get
            {
                return this._dependencyType;
            }
            set
            {
                this._dependencyType = value;
            }
        }

		/// <summary>
        /// Property to decide if attribute is collection 
        /// </summary>
        public Boolean IsCollection
        {
            get
            {
                return this._isCollection;
            }
        }

        /// <summary>
        /// Indicates collection of contexts in which the dependency is valid
        /// </summary>
        [DataMember]
        [ProtoMember(10)]
        public ApplicationContextCollection ApplicationContexts
        {
            get { return _applicationContexts; }
            set { _applicationContexts = value; }
        }

        /// <summary>
        /// Indicates level of dependency. This is used to identify in multilevel dependency, given dependency is at which level.
        /// </summary>
        [DataMember]
        [ProtoMember(11)]
        public Int32 DependentLevel
        {
            get { return _dependentLevel; }
            set { _dependentLevel = value; }
        }

        /// <summary>
        /// Indicates the parent attribute for given dependency
        /// </summary>
        [DataMember]
        [ProtoMember(12)]
        public Int32 ParentAttributeId
        {
            get { return _parentAttributeId; }
            set { _parentAttributeId = value; }
        }

        #endregion

        #region Methods

        #region Public Methods

        #region Dependent Attribute Load and Toxml methods
       
        /// <summary>
        /// Load Dependent Attribute
        /// </summary>
        /// <param name="valuesAsXml">Input xml Dependent Attribute object</param>
        public void LoadDependentAttribute(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "DependentAttribute")
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("AttributeId"))
                                {
                                    this._attributeId = reader.ReadContentAsInt();
                                }

                                if (reader.MoveToAttribute("AttributeName"))
                                {
                                    this._attributeName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("AttributeValue"))
                                {
                                    this._attributeValue = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("LinkTableColumn"))
                                {
                                    this._linkTableColumn = reader.ReadContentAsString(); ;
                                }

                                if (reader.MoveToAttribute("LinkTableName"))
                                {
                                    this._linkTableName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("LookupTableName"))
                                {
                                    this._lookupTableName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("DependencyType"))
                                {
                                    DependencyType dAttrType = DependencyType.Unknown;

                                    Enum.TryParse<DependencyType>(reader.ReadContentAsString(), out dAttrType);
                                    this._dependencyType = dAttrType;
                                }

                                if (reader.MoveToAttribute("ParentAttributeId"))
                                {
                                    this._parentAttributeId = reader.ReadContentAsInt();
                                }

                                if (reader.MoveToAttribute("DependentLevel"))
                                {
                                    this._dependentLevel = reader.ReadContentAsInt();
                                }

                                if (reader.MoveToAttribute("IsCollection"))
                                {
                                    this._isCollection = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._isCollection);
                                }
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ApplicationContexts")
                        {
                            String xml = reader.ReadOuterXml();
                            ApplicationContextCollection contextColl = new ApplicationContextCollection(xml);
                            if (contextColl != null)
                            {
                                this._applicationContexts = contextColl;
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

        /// <summary>
        /// Get Xml representation of DependentAttribute object
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public override String ToXml()
        {
            String dependentAttributeXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //DependentAttribute node start
            xmlWriter.WriteStartElement("DependentAttribute");

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("AttributeId", this.AttributeId.ToString());
            xmlWriter.WriteAttributeString("ParentAttributeName", this.ParentAttributeName);
            xmlWriter.WriteAttributeString("AttributeName", this.AttributeName);
            xmlWriter.WriteAttributeString("AttributeValue", this.AttributeValue);
            xmlWriter.WriteAttributeString("LinkTableName", this.LinkTableName);
            xmlWriter.WriteAttributeString("LinkTableColumn", this.LinkTableColumn);
            xmlWriter.WriteAttributeString("LookupTableName", this.LookupTableName);
            xmlWriter.WriteAttributeString("DependencyType", this.DependencyType.ToString());
            xmlWriter.WriteAttributeString("DependentLevel", this.DependentLevel.ToString());
            xmlWriter.WriteAttributeString("ParentAttributeId", this.ParentAttributeId.ToString());
            xmlWriter.WriteAttributeString("IsCollection", this.IsCollection.ToString());
            xmlWriter.WriteAttributeString("Action", this.Action.ToString());
            xmlWriter.WriteStartElement("ApplicationContexts");

            xmlWriter.WriteRaw(this.ApplicationContexts.ToXml());

            xmlWriter.WriteEndElement();

            //DependentAttribute node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            dependentAttributeXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return dependentAttributeXml;
        }

        /// <summary>
        /// Clones the curent instance to a newer instance. 
        /// </summary>
        /// <param name="includeValue">Specified whether the value part of the object needs to be included in the clone or not</param>
        /// <returns></returns>
        public DependentAttribute Clone(Boolean includeValue)
        {
            var dependentAttribute = new DependentAttribute();

            dependentAttribute.Id = this.Id;
            dependentAttribute.AttributeName = this.AttributeName;
            dependentAttribute.ParentAttributeName = this.ParentAttributeName;
            dependentAttribute.ParentAttributeId = this.ParentAttributeId;
            dependentAttribute.AttributeId = this.AttributeId;
            dependentAttribute.LookupTableName = this.LookupTableName;
            dependentAttribute.LinkTableColumn = this.LinkTableColumn;

            if (includeValue)
            {
                dependentAttribute.SetAttributeValue(this.AttributeValue);
            }

            dependentAttribute.DependencyType = this.DependencyType;
            dependentAttribute.DependentLevel = this.DependentLevel;
            dependentAttribute.ApplicationContexts = (ApplicationContextCollection)this.ApplicationContexts.Clone();

            return dependentAttribute;
        }

        #endregion
       
        #region Set and Set Attribute Values

        /// <summary>
        /// Get the Dependency Attribute Value.
        /// If the attribute is collection then the value as the format of 'Value#$#Value'.
        /// Values are separated by '#$#'
        /// </summary>
        /// <returns>Attribute value as String</returns>
        public String GetAttributeValue()
        {
            return this.AttributeValue;
        }

        /// <summary>
        /// Set the Dependent Attribute Value.
        /// </summary>
        /// <param name="valueCollection">Indicates the attribute value collection object</param>
        public void SetAttributeValue(ValueCollection valueCollection)
        {
            StringBuilder result = new StringBuilder();

            if (valueCollection != null)
            {
                foreach (Value val in valueCollection)
                {
                    if (val.Action != ObjectAction.Delete)  // Parent values may comes as delete. No need to consider else deleted values childs will become valid.
                    {
                        result.Append(val.AttrVal).Append("$@$");
                    }
                }

                if (result.Length > 0)
                {
                    this._attributeValue = result.ToString().Substring(0, result.ToString().Length - 3);
                    this._isCollection = true;
                }
            }
        }

        /// <summary>
        /// Set the Dependent Attribute Value.
        /// </summary>
        /// <param name="iValueCollection">Indicates the attribute value collection interface</param>
        public void SetAttributeValue(IValueCollection iValueCollection)
        {
            if (iValueCollection != null)
            {
                this.SetAttributeValue((ValueCollection)iValueCollection);
            }
        }

        /// <summary>
        /// Set the Dependent Attribute Value.
        /// </summary>
        /// <param name="attribute">Indicates the attribute object</param>
        public void SetAttributeValue(Attribute attribute)
        {
            if (attribute != null)
            {
                //Populate whether the attribute is collection or not. 
                this._isCollection = attribute.IsCollection;

                if (!(attribute.IsCollection || attribute.GetOverriddenValue() == null))
                {
                    this._attributeValue = attribute.GetOverriddenValue().ToString();
                }
                else if (attribute.IsCollection && attribute.GetOverriddenValues() != null)
                {
                    this.SetAttributeValue(attribute.GetOverriddenValues());
                }

                //EH scenario below block of code will be used...
                if ((attribute.Action == ObjectAction.Ignore || attribute.Action == ObjectAction.Read) && attribute.GetInheritedValues() != null && attribute.SourceFlag != AttributeValueSource.Overridden)
                {
                    if (!(attribute.IsCollection || attribute.GetInheritedValue() == null))
                    {
                        this._attributeValue = attribute.GetInheritedValue().ToString();
                    }
                    else if (attribute.IsCollection && attribute.GetInheritedValues() != null)
                    {
                        this.SetAttributeValue(attribute.GetInheritedValues());
                    }
                }
            }
        }

        /// <summary>
        /// Set the Dependent Attribute Value.
        /// </summary>
        /// <param name="attributeValue">Indicates the attribute value</param>
        public void SetAttributeValue(String attributeValue)
        {
            if (!String.IsNullOrWhiteSpace(attributeValue))
            {
                this._attributeValue = attributeValue;
            }
        }

        /// <summary>
        /// Set the Dependent Attribute Value.
        /// </summary>
        /// <param name="iAttribute">Indicates the Attribute Interface</param>
        public void SetAttributeValue(IAttribute iAttribute)
        {
            if (iAttribute != null)
            {
                this.SetAttributeValue((Attribute)iAttribute);
            }
        }

        #endregion

        #region ApplicationContext Methods
        /// <summary>
        /// Add dependent attribute application context
        /// </summary>
        /// <param name="applicationContext">Indicates the application context</param>
        public void AddApplicationContext(IApplicationContext applicationContext)
        {
            if (applicationContext != null)
            {
                this._applicationContexts.Add((ApplicationContext)applicationContext);
            }
        }

        /// <summary>
        /// Get application context for given dependency
        /// </summary>
        /// <returns>ApplicationContext collection for given dependency</returns>
        public IApplicationContextCollection GetApplicationContexts()
        {
            if (this._applicationContexts != null)
            {
                return (IApplicationContextCollection)this._applicationContexts;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Remove application context for given dependency
        /// </summary>
        /// <param name="applicationContext">Indicates application context which needs to be removed</param>
        public void RemoveApplicationContext(IApplicationContext applicationContext)
        {
            if (applicationContext != null)
            {
                this._applicationContexts.Remove((ApplicationContext)applicationContext);
            }
        }
        #endregion
      
        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}
