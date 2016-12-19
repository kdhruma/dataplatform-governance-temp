using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml;
using ProtoBuf;

namespace MDM.BusinessObjects
{
    using Core;
    using MDM.Interfaces;

    /// <summary>
    /// Attribute context
    /// </summary>
    [DataContract]
    [ProtoContract]
    [KnownType(typeof(AttributeUniqueIdentifier))]
    [KnownType(typeof(AttributeUniqueIdentifierCollection))]
    public class AttributeContext : ObjectBase, IAttributeContext
    {
        #region Fields

        /// <summary>
        /// Field denoting state view id.
        /// </summary>
        [DataMember]
        [ProtoMember(1), DefaultValue(0)]
        private Int32 _stateViewId = 0;

        /// <summary>
        /// Field denoting state view name.
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        private String _stateViewName = String.Empty;

        /// <summary>
        /// Field denoting custom View id.
        /// </summary>
        [DataMember]
        [ProtoMember(3), DefaultValue(0)]
        private Int32 _customViewId = 0;

        /// <summary>
        /// Field denoting custom view name.
        /// </summary>
        [DataMember]
        [ProtoMember(4)]
        private String _customViewName = String.Empty;

        /// <summary>
        /// Field denoting whether to load only inherited attributes or not.
        /// </summary>
        [DataMember]
        [ProtoMember(5), DefaultValue(false)]
        private Boolean _loadOnlyInheritedValues = false;

        /// <summary>
        /// Field denoting whether to load only overridden attributes or not.
        /// </summary>
        [DataMember]
        [ProtoMember(6), DefaultValue(false)]
        private Boolean _loadOnlyOverridenValues = false;

        /// <summary>
        /// Field denoting whether to load only current attribute values (Either Inherited or Overridden based on availability) or not.
        /// </summary>
        [DataMember]
        [ProtoMember(7), DefaultValue(false)]
        private Boolean _loadOnlyCurrentValues = false;

        /// <summary>
        /// Field denoting attributes belonging to which group are to be loaded.
        /// This property works in unison with LoadAttributesWithSpecificGroupIdList.
        /// To load attributes from group ids given in AttributeGroupIdList, LoadAttributesWithSpecificGroupIdList should be set to true.
        /// </summary>
        [DataMember]
        [ProtoMember(8)]
        private Collection<Int32> _attributeGroupIdList = null;

        /// <summary>
        /// Field denoting short name of the attribute group name 
        /// </summary>
        [DataMember]
        [ProtoMember(9)]
        private Collection<String> _attributeGroupNames = null;

        /// <summary>
        /// Field denoting which attributes are to be loaded.
        /// This property works in unison with LoadAttributesWithSpecificAttributeIdList
        /// To load attributes from attribute ids given in AttributeIdList, LoadAttributesWithSpecificGroupIdList should be set to true.
        /// </summary>
        [DataMember]
        [ProtoMember(10)]
        private Collection<Int32> _attributeIdList = null;

        /// <summary>
        /// Field denoting short name of the attribute with GroupName
        /// </summary>
        [DataMember]
        [ProtoMember(11)]
        private Collection<String> _attributeNames = null;

        /// <summary>
        /// Field denoting whether to load only those attributes which are marked as ShowAtCreation
        /// </summary>
        [DataMember]
        [ProtoMember(12), DefaultValue(false)]
        private Boolean _loadCreationAttributes = false;

        /// <summary>
        /// Field denoting whether to load only those attributes which are marked as Required
        /// </summary>
        [DataMember]
        [ProtoMember(13), DefaultValue(false)]
        private Boolean _loadRequiredAttributes = false;

        /// <summary>
        /// Field denoting which type of attributes are to be loaded. Possible values <see cref="AttributeModelType"/>
        /// </summary>
        [DataMember]
        [ProtoMember(14), DefaultValue(AttributeModelType.All)]
        private AttributeModelType _attributeModelType = AttributeModelType.All;

        /// <summary>
        /// Field denoting whether to load AttributeModel or not
        /// </summary>
        [DataMember]
        [ProtoMember(15), DefaultValue(false)]
        private Boolean _loadAttributeModels = false;

        /// <summary>
        /// Field denoting whether to load dependent attribute or not
        /// </summary>
        [DataMember]
        [ProtoMember(16), DefaultValue(false)]
        private Boolean _loadDependentAttributes = false;

        /// <summary>
        /// Field denoting whether to load attributes having blank values / no value
        /// When set to true, Entity Get will return attribute object instances having blank / no value
        /// </summary>
        [DataMember]
        [ProtoMember(17), DefaultValue(true)]
        private Boolean _loadBlankAttributes = true;

        /// <summary>
        /// Field denoting whether to load complex child attribute data for complex attributes
        /// When set to true, Entity Get will return complex attribute object populated which complex record instances and child attributes
        /// </summary>
        [DataMember]
        [ProtoMember(18), DefaultValue(true)]
        private Boolean _loadComplexChildAttributes = true;

        /// <summary>
        /// Field denoting whether to load lookup display values or not
        /// </summary>
        [DataMember]
        [ProtoMember(19), DefaultValue(false)]
        private Boolean _loadLookupDisplayValues = false;

        /// <summary>
        /// Property denoting whether to load LookupRow along with Value or not
        /// </summary>
        [DataMember]
        [ProtoMember(20), DefaultValue(false)]
        private Boolean _loadLookupRowWithValues = false;

        /// <summary>
        /// Indicates whether to load state validation attributes or not.
        /// </summary>
        [DataMember]
        [ProtoMember(21), DefaultValue(false)]
        private Boolean _loadStateValidationAttributes = false;

        /// <summary>
        /// Indicates whether to load inheritable only attibutes or not.
        /// </summary>
        [DataMember]
        [ProtoMember(22), DefaultValue(true)]
        private Boolean _loadInheritableOnlyAttributes = true;

        #endregion

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public AttributeContext()
            : base()
        {
        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">XML having xml values</param>
        public AttributeContext(String valuesAsXml)
        {
            LoadAttributeContext(valuesAsXml);
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Property denoting state view id.
        /// </summary>
        public Int32 StateViewId
        {
            get
            {
                return _stateViewId;
            }
            set
            {
                _stateViewId = value;
            }
        }

        /// <summary>
        /// Property denoting state view name.
        /// </summary>
        public String StateViewName
        {
            get
            {
                return _stateViewName;
            }
            set
            {
                _stateViewName = value;
            }
        }

        /// <summary>
        /// Property denoting custom view id.
        /// </summary>
        public Int32 CustomViewId
        {
            get
            {
                return _customViewId;
            }
            set
            {
                _customViewId = value;
            }
        }

        /// <summary>
        /// Property denoting custom view name.
        /// </summary>
        public String CustomViewName
        {
            get
            {
                return _customViewName;
            }
            set
            {
                _customViewName = value;
            }
        }

        /// <summary>
        /// Field denoting whether to load only Inherited attributes or not.
        /// </summary>
        public Boolean LoadOnlyInheritedValues
        {
            get
            {
                return _loadOnlyInheritedValues;
            }
            set
            {
                _loadOnlyInheritedValues = value;
            }
        }

        /// <summary>
        /// Field denoting whether to load only Overridden attributes or not.
        /// </summary>
        public Boolean LoadOnlyOverridenValues
        {
            get
            {
                return _loadOnlyOverridenValues;
            }
            set
            {
                _loadOnlyOverridenValues = value;
            }
        }

        /// <summary>
        /// Property denoting whether to load only current attribute values (Either Inherited or Overridden based on availability) or not.
        /// </summary>
        public Boolean LoadOnlyCurrentValues
        {
            get
            {
                return _loadOnlyCurrentValues;
            }
            set
            {
                _loadOnlyCurrentValues = value;
            }
        }

        /// <summary>
        /// Property denoting attributes belonging to which group are to be loaded.
        /// This property works in unison with LoadAttributesWithSpecificGroupIdList.
        /// To load attributes from group ids given in AttributeGroupIdList, LoadAttributesWithSpecificGroupIdList should be set to true.
        /// </summary>
        public Collection<Int32> AttributeGroupIdList
        {
            get
            {
                if (this._attributeGroupIdList == null)
                {
                    this._attributeGroupIdList = new Collection<Int32>();
                }

                return _attributeGroupIdList;
            }
            set
            {
                _attributeGroupIdList = value;
            }
        }

        /// <summary>
        /// Property denoting short name of the attribute group name with Parent Name
        /// </summary>
        public Collection<String> AttributeGroupNames
        {
            get
            {
                if (this._attributeGroupNames == null)
                {
                    this._attributeGroupNames = new Collection<String>();
                }

                return _attributeGroupNames;
            }
            set
            {
                _attributeGroupNames = value;
            }
        }

        /// <summary>
        /// Property denoting which attributes are to be loaded.
        /// This property works in unison with LoadAttributesWithSpecificAttributeIdList.
        /// To load attributes from attribute ids given in AttributeIdList, LoadAttributesWithSpecificGroupIdList should be set to true.
        /// </summary>
        public Collection<Int32> AttributeIdList
        {
            get
            {
                if (this._attributeIdList == null)
                {
                    this._attributeIdList = new Collection<Int32>();
                }

                return _attributeIdList;
            }
            set
            {
                _attributeIdList = value;
            }
        }

        /// <summary>
        /// Property denoting short name of the attribute with Group Name
        /// </summary>
        public Collection<String> AttributeNames
        {
            get
            {
                if (this._attributeNames == null)
                    this._attributeNames = new Collection<String>();

                return _attributeNames;
            }
            set
            {
                this._attributeNames = value;
            }
        }

        /// <summary>
        /// Property denoting whether to load only those attributes which are marked as ShowAtCreation
        /// </summary>
        public Boolean LoadCreationAttributes
        {
            get
            {
                return _loadCreationAttributes;
            }
            set
            {
                _loadCreationAttributes = value;
            }
        }

        /// <summary>
        /// Property denoting whether to load only those attributes which are marked as Required
        /// </summary>
        public Boolean LoadRequiredAttributes
        {
            get
            {
                return _loadRequiredAttributes;
            }
            set
            {
                _loadRequiredAttributes = value;
            }
        }

        /// <summary>
        /// Property denoting which type of attributes are to be loaded. Possible values <see cref="AttributeModelType"/>
        /// </summary>
        public AttributeModelType AttributeModelType
        {
            get
            {
                return _attributeModelType;
            }
            set
            {
                _attributeModelType = value;
            }
        }

        /// <summary>
        /// Property denoting whether to load AttributeModel or not.
        /// </summary>
        public Boolean LoadAttributeModels
        {
            get
            {
                return _loadAttributeModels;
            }
            set
            {
                _loadAttributeModels = value;
            }
        }

        /// <summary>
        /// Property denoting whether to load dependent attribute or not
        /// </summary>
        public Boolean LoadDependentAttributes
        {
            get
            {
                return _loadDependentAttributes;
            }
            set
            {
                _loadDependentAttributes = value;
            }
        }

        /// <summary>
        /// Property denoting whether to load attributes having blank values / no value
        /// When set to true, Entity Get will return attribute object instances having blank / no value
        /// </summary>
        public Boolean LoadBlankAttributes
        {
            get
            {
                return _loadBlankAttributes;
            }
            set
            {
                _loadBlankAttributes = value;
            }
        }

        /// <summary>
        /// Property denoting whether to load complex child attributes or not
        /// </summary>
        public Boolean LoadComplexChildAttributes
        {
            get
            {
                return _loadComplexChildAttributes;
            }
            set
            {
                _loadComplexChildAttributes = value;
            }
        }

        /// <summary>
        /// Property denoting whether to load lookup display values or not
        /// </summary>
        public Boolean LoadLookupDisplayValues
        {
            get
            {
                return _loadLookupDisplayValues;
            }
            set
            {
                _loadLookupDisplayValues = value;
            }
        }

        /// <summary>
        /// Property denoting whether to load LookupRow along with Value or not
        /// </summary>
        public Boolean LoadLookupRowWithValues
        {
            get
            {
                return _loadLookupRowWithValues;
            }
            set
            {
                _loadLookupRowWithValues = value;
            }
        }

        /// <summary>
        /// Specifies whether to load state validation attributes or not.
        /// </summary>
        public Boolean LoadStateValidationAttributes
        {
            get
            {
                return _loadStateValidationAttributes;
            }
            set
            {
                _loadStateValidationAttributes = value;
            }
        }

        /// <summary>
        /// Specifies whether to load inheritable only attributes  or not.
        /// </summary>
        public Boolean LoadInheritableOnlyAttributes
        {
            get
            {
                return _loadInheritableOnlyAttributes;
            }
            set
            {
                _loadInheritableOnlyAttributes = value;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Create a new entity context object.
        /// </summary>
        /// <returns>New entity context instance</returns>
        public IAttributeContext Clone()
        {
            var clonedAttributeContext = new AttributeContext();

            clonedAttributeContext._stateViewId = this._stateViewId;
            clonedAttributeContext._stateViewName = this._stateViewName;
            clonedAttributeContext._customViewId = this._customViewId;
            clonedAttributeContext._customViewName = this._customViewName;
            clonedAttributeContext._attributeGroupIdList = ValueTypeHelper.CloneCollection(this._attributeGroupIdList);
            clonedAttributeContext._attributeGroupNames = ValueTypeHelper.CloneCollection(this._attributeGroupNames);
            clonedAttributeContext._attributeIdList = ValueTypeHelper.CloneCollection(this._attributeIdList);
            clonedAttributeContext._attributeNames = ValueTypeHelper.CloneCollection(this._attributeNames);
            clonedAttributeContext._loadOnlyCurrentValues = this._loadOnlyCurrentValues;
            clonedAttributeContext._loadOnlyInheritedValues = this._loadOnlyInheritedValues;
            clonedAttributeContext._loadOnlyOverridenValues = this._loadOnlyOverridenValues;
            clonedAttributeContext._loadCreationAttributes = this._loadCreationAttributes;
            clonedAttributeContext._loadRequiredAttributes = this._loadRequiredAttributes;
            clonedAttributeContext._attributeModelType = this._attributeModelType;
            clonedAttributeContext._loadAttributeModels = this._loadAttributeModels;
            clonedAttributeContext._loadLookupDisplayValues = this._loadLookupDisplayValues;
            clonedAttributeContext._loadLookupRowWithValues = this._loadLookupRowWithValues;
            clonedAttributeContext._loadDependentAttributes = this._loadDependentAttributes;
            clonedAttributeContext._loadBlankAttributes = this._loadBlankAttributes;
            clonedAttributeContext._loadComplexChildAttributes = this._loadComplexChildAttributes;
            clonedAttributeContext._loadStateValidationAttributes = this._loadStateValidationAttributes;
            clonedAttributeContext._loadInheritableOnlyAttributes = this._loadInheritableOnlyAttributes;

            return clonedAttributeContext;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Load entity object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        private void LoadAttributeContext(String valuesAsXml)
        {
            #region Sample Xml

            // <AttributeContext StateViewId="1" StateViewName="required Empty StateView" CustomViewId="0" CustomViewName="" LoadOnlyInheritedValues="false" LoadOnlyOverridenValues="false"
            //          LoadOnlyCurrentValues="false" AttributeGroupIdList="1,2" AttributeGroupNames="Merchandising, Vendor" AttributeIdList="4410,5236" 
            //          AttributeNames="status,description" LoadCreationAttributes="true" LoadRequiredAttributes="true" AttributeModelType="Common" LoadAttributeModels="true"
            //          LoadDependentAttributes="false" LoadBlankAttributes="false" LoadComplexChildAttributes="true" LoadLookupDisplayValues="true" LoadLookupRowWithValues="true" /> 

            #endregion Sample Xml

            using (XmlTextReader reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null))
            {
                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "AttributeContext")
                    {
                        #region Read AttributeContext properties

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("StateViewId"))
                            {
                                this.StateViewId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                            }

                            if (reader.MoveToAttribute("StateViewName"))
                            {
                                this.StateViewName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("CustomViewId"))
                            {
                                this.CustomViewId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                            }

                            if (reader.MoveToAttribute("CustomViewName"))
                            {
                                this.CustomViewName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("LoadOnlyInheritedValues"))
                            {
                                this.LoadOnlyInheritedValues = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("LoadOnlyOverridenValues"))
                            {
                                this.LoadOnlyOverridenValues = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("LoadOnlyCurrentValues"))
                            {
                                this.LoadOnlyCurrentValues = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("AttributeGroupIdList"))
                            {
                                this.AttributeGroupIdList = ValueTypeHelper.SplitStringToIntCollection(reader.ReadContentAsString(), ',');
                            }

                            if (reader.MoveToAttribute("AttributeGroupNames"))
                            {
                                this.AttributeGroupNames = ValueTypeHelper.SplitStringToStringCollection(reader.ReadContentAsString(), ',');
                            }

                            if (reader.MoveToAttribute("AttributeIdList"))
                            {
                                this.AttributeIdList = ValueTypeHelper.SplitStringToIntCollection(reader.ReadContentAsString(), ',');
                            }

                            if (reader.MoveToAttribute("AttributeNames"))
                            {
                                this.AttributeNames = ValueTypeHelper.SplitStringToStringCollection(reader.ReadContentAsString(), ',');
                            }

                            if (reader.MoveToAttribute("LoadCreationAttributes"))
                            {
                                this.LoadCreationAttributes = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("LoadRequiredAttributes"))
                            {
                                this.LoadRequiredAttributes = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("AttributeModelType"))
                            {
                                AttributeModelType attributeModeType = AttributeModelType.All;
                                Enum.TryParse(reader.ReadContentAsString(), true, out attributeModeType);
                                this.AttributeModelType = attributeModeType;
                            }

                            if (reader.MoveToAttribute("LoadAttributeModels"))
                            {
                                this.LoadAttributeModels = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("LoadDependentAttributes"))
                            {
                                this.LoadDependentAttributes = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("LoadBlankAttributes"))
                            {
                                this.LoadBlankAttributes = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("LoadComplexChildAttributes"))
                            {
                                this.LoadComplexChildAttributes = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("LoadLookupDisplayValues"))
                            {
                                this.LoadLookupDisplayValues = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("LoadLookupRowWithValues"))
                            {
                                this.LoadLookupRowWithValues = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("LoadStateValidationAttributes"))
                            {
                                this.LoadStateValidationAttributes = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if(reader.MoveToAttribute("LoadInheritableOnlyAttributes"))
                            {
                                this.LoadInheritableOnlyAttributes = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }
                        }

                        #endregion Read AttributeContext properties
                    }
                    else
                    {
                        //Keep on reading the xml until we reach expected node.
                        reader.Read();
                    }
                }
            }
        }

        #endregion Private Methods

        #endregion
    }
}