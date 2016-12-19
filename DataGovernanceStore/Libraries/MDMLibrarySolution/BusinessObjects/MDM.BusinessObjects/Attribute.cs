using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the Attribute Value Instance for the Object
    /// </summary>
    [DataContract]
    [ProtoContract]
    [KnownType(typeof(AttributeValueSource))]
    public class Attribute : MDMObject, IAttribute
    {
        #region Fields

        /// <summary>
        /// Field for AttributeParentID (Attribute group ID)
        /// </summary>
        private Int32 _attributeParentId = 0;

        /// <summary>
        /// Field for AttributeParentName (Attribute group short name)
        /// </summary>
        private String _attributeParentName = String.Empty;

        /// <summary>
        /// Field for AttributeParentLongName (Attribute group long name)
        /// </summary>
        private String _attributeParentLongName = String.Empty;

        /// <summary>
        /// Field for attribute data type
        /// </summary>
        private AttributeDataType _attributeDataType = AttributeDataType.String;

        /// <summary>
        /// Field to decide if attribute is collection
        /// </summary>
        private Boolean _isCollection = false;

        /// <summary>
        /// Field to decide if attribute is a complex attribute
        /// </summary>
        private Boolean _isComplex = false;

        /// <summary>
        /// Filed indicating if this attribute is of hierarchical strucutre.
        /// This means, it has other attributes as children of any number of levels.
        /// </summary>
        private Boolean _isHierarchical = false;

        /// <summary>
        /// Field to decide if attribute is lookup
        /// </summary>
        private Boolean _isLookup = false;

        /// <summary>
        /// Field which indicates whether attribute is read only
        /// </summary>
        private Boolean _readOnly = false;

        /// <summary>
        /// Field which indicates whether attribute is required
        /// </summary>
        private Boolean _required = false;

        /// <summary>
        /// Field to decide if attribute is localizable
        /// </summary>
        private Boolean _isLocalizable = true;

        /// <summary>
        /// Field to decide if localization formatting is to be applied to attribute value
        /// </summary>
        private Boolean _applyLocaleFormat = true;

        /// <summary>
        /// Field to decide if timezone conversion is to be applied to attribute value
        /// </summary>
        private Boolean _applyTimeZoneConversion = false;

        /// <summary>
        /// Field for attribute value source ('O' or 'I')
        /// </summary>
        private AttributeValueSource _sourceFlag = AttributeValueSource.Overridden;

        /// <summary>
        /// Field denoting source entity for the overridden values (default is 0)
        /// </summary>
        private Int64 _sourceEntityIdOverridden = 0;

        /// <summary>
        ///  indicates Overridden attribute value for given entity is coming from which level
        /// </summary>
        private Int32 _sourceClassOverridden = 0;

        /// <summary>
        /// Field denoting source entity for the inherited values (default is 0)
        /// </summary>
        private Int64 _sourceEntityIdInherited = 0;

        /// <summary>
        ///  indicates Inherited attribute value for given entity is coming frm which level
        /// </summary>
        private Int32 _sourceClassInherited = 0;

        /// <summary>
        /// Field required for adding data validation to excel file in case of lookup atribute
        /// </summary>
        private String _validationAdditionalValue = String.Empty;

        /// <summary>
        /// Field for attribute value collection
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        private ValueCollection _overriddenValues = new ValueCollection();

        /// <summary>
        /// field for innherit attribute value collection
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        private ValueCollection _inheritedValues = new ValueCollection();

        /// <summary>
        /// Field to indicate how many precision will attribute value have
        /// </summary>
        private Int32 _precision = 0;

        /// <summary>
        /// Field denoting AttributeType name. This field is directly mapped to tb_AttributeType.
        /// After reading this property, <see cref="AttributeTypeEnum"/> and <see cref="AttributeModelType"/> will be populated (normalized one) with correct values
        /// </summary>
        private String _dbAttributeType = String.Empty;

        /// <summary>
        /// Field denoting the model type of an attribute. It indicates whether an attribute is common attribute or technical attribute.
        /// For possible values, see <see cref="AttributeModelType" />
        /// </summary>
        private AttributeModelType _attributeModelType = AttributeModelType.All;

        /// <summary>
        /// Field denoting the type of attribute. for possible values, see <see cref="MDM.Core.AttributeTypeEnum"/>
        /// </summary>
        private MDM.Core.AttributeTypeEnum _attributeType = MDM.Core.AttributeTypeEnum.Simple;

        /// <summary>
        /// Field denoting collection of child attributes.
        /// </summary>
        private AttributeCollection _attributes = new AttributeCollection();

        /// <summary>
        /// Field denoting value reference id.
        /// For complex attributes, InstanceRefId represents the PK of respective complex attribute (tbcx_) table
        /// </summary>
        private Int32 _instanceRefId = -1;

        /// <summary>
        /// Field denoting suffix to be used for identifying complex attribute instance
        /// </summary>
        private String _instanceAttributeSuffix = "Instance Record";

        /// <summary>
        /// Field denoting sequence of current attribute.
        /// This is used in case of complex and complex collection attribute
        /// </summary>
        private Decimal _sequence = -1;

        /// <summary>
        /// Field denoting the entity id this attribute belongs to
        /// </summary>
        private Int64 _entityId = -1;

        /// <summary>
        /// Property denoting if arbitrary precision is used for Decimal attributes.
        /// If value of IsPrecisionArbitrary is true, then decimal value 12.2 with precision 3 will be stored as "12.2"
        /// If value of IsPrecisionArbitrary is false, then decimal value 12.2 with precision 3 will be stored as "12.200"
        /// </summary>
        private Boolean _isPrecisionArbitrary = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public Attribute()
            : base()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Identity of an Attribute Instance</param>
        public Attribute(Int32 id)
            : base(id)
        {

        }

        /// <summary>
        /// Initializes a new instance of the Attribute class
        /// </summary>
        /// <param name="id">Id of the attribute</param>
        /// <param name="name">Name of the attribute</param>
        /// <param name="longName">Long name of the attribute</param>
        /// <param name="attributeModelType">Attribute Model Type</param>
        /// <param name="attributeValue">Value of the attribute</param>
        public Attribute(Int32 id, String name, String longName, AttributeModelType attributeModelType, Object attributeValue)
            : base(id, name, longName)
        {
            this._attributeModelType = attributeModelType;

            //Add value
            Value value = new Value(attributeValue);
            this._sourceFlag = AttributeValueSource.Overridden;

            //this._overriddenValues.Add(value);
            this.SetValue(value);
            this.Action = ObjectAction.Read;
        }

        /// <summary>
        /// Create attribute object with property values xml as input parameter
        /// </summary>
        /// <example>
        /// Sample XML
        /// <para>
        ///  &lt;Attribute
        ///         Id="3097" 
        ///         Name="Size" 
        ///         LongName="Size" 
        ///         AttributeParentId="3010" 
        ///         AttributeParentName="Merchandising" 
        ///         AttributeParentLongName="Merchandising" 
        ///         AttributeType="Attribute" 
        ///         AttributeDataType="String" 
        ///         IsCollection="true" 
        ///         IsLocalizable="true" 
        ///         ApplyLocaleFormat="true" 
        ///         ApplyTimeZoneConversion="true" 
        ///         Precision = "2"
        ///         IsComplex="false" 
        ///         SourceFlag="O" 
        ///         Updateable="Update" 
        ///         Visible="View"&gt;
        ///     &lt;Values&gt;
        ///         &lt;Value AttrVal="02T0" Uom="" ValueRefId="2" Sequence="0" /&gt;
        ///         &lt;Value AttrVal="03T0" Uom="" ValueRefId="3" Sequence="1" /&gt;
        ///     &lt;/Values>
        ///     &lt;InheritedValues&gt;
        ///         &lt;Value AttrVal="02T0" Uom="" ValueRefId="2" Sequence="0" /&gt;
        ///     &lt;/InheritedValues&gt;
        /// &lt;/Attribute&gt;
        /// </para>
        /// </example>
        /// <param name="valuesAsXml">XML representation for attribute from which object is to be created</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        public Attribute(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
          : this(valuesAsXml, false, objectSerialization)
        { 
        }

        /// <summary>
        /// Create attribute object with attribute model object as input parameter
        /// </summary>
        /// <param name="attributeModel">AttributeModel from which attribute object is to be populated</param>

        public Attribute(AttributeModel attributeModel)
            : this(attributeModel, attributeModel.Locale)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        /// <param name="allowDuplicates"></param>
        /// <param name="objectSerialization"></param>
        public Attribute(String valuesAsXml, bool allowDuplicates, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
          LoadAttribute(valuesAsXml, allowDuplicates, objectSerialization);
        }


        /// <summary>
        /// Create attribute object with attribute model object as input parameter
        /// </summary>
        /// <param name="attributeModel">AttributeModel from which attribute object is to be populated</param>
        /// <param name="attributeLocale">Locale of an new attribute being populated</param>
        public Attribute(AttributeModel attributeModel, LocaleEnum attributeLocale)
        {
            if (attributeModel == null)
                throw new ArgumentNullException("AttributeModel", "AttributeModel is null");

            //Read property by property and fill properties
            this.Id = attributeModel.Id;
            this.Name = attributeModel.Name;
            this.LongName = attributeModel.LongName;
            this.Locale = attributeLocale;
            this.AttributeParentId = attributeModel.AttributeParentId;
            this.AttributeParentName = attributeModel.AttributeParentName;
            this.AttributeParentLongName = attributeModel.AttributeParentLongName;
            this.AttributeType = attributeModel.AttributeType;
            this.AttributeModelType = attributeModel.Context.AttributeModelType;

            AttributeDataType attributeDataType;
            
            if (Enum.TryParse<AttributeDataType>(attributeModel.AttributeDataTypeName, out attributeDataType))
            {
                this.AttributeDataType = attributeDataType;
            }
            //else
            //{
            //    // default value is already initialized to string.
            //}


            this.IsCollection = attributeModel.IsCollection;
            this.IsComplex = attributeModel.IsComplex;
            this.IsLookup = attributeModel.IsLookup;
            this.IsLocalizable = attributeModel.IsLocalizable;
            this.ReadOnly = attributeModel.ReadOnly;
            this.Required = attributeModel.Required;
            this.ApplyLocaleFormat = attributeModel.ApplyLocaleFormat;
            this.ApplyTimeZoneConversion = attributeModel.ApplyTimeZoneConversion;
            this.Precision = attributeModel.Precision;
            this.IsPrecisionArbitrary = attributeModel.IsPrecisionArbitrary;

            /*if (this.IsHierarchical)
            {
                // TODO PRASAD - Do we need to create Instance Record for Hierarchical attribute type.
                CreateHierarchyInstanceRecordModel(attributeModel, attributeLocale);
            }
            else*/ if (this.IsComplex || this.IsHierarchical)
            {
                //The attribute is complex in nature
                //Complex attribute is represented by the attribute having the instance record of the complex attribute as a child attribute.
                //And the attributes which forms the complex attribute is represented as child attributes of the instance record
                CreateComplexInstanceRecordModel(attributeModel, attributeLocale);
            }
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
                return "Attribute";
            }
        }

        /// <summary>
        /// Property for AttributeParentID (Attribute group ID)
        /// </summary>
        [DataMember]
        [ProtoMember(3)]
        public Int32 AttributeParentId
        {
            get { return _attributeParentId; }
            set { _attributeParentId = value; }
        }

        /// <summary>
        /// Property for AttributeParentName (Attribute group short name) 
        /// </summary>
        [DataMember]
        [ProtoMember(4)]
        public String AttributeParentName
        {
            get { return _attributeParentName; }
            set { _attributeParentName = value; }
        }

        /// <summary>
        /// Property for AttributeParentLongName (Attribute group long name) 
        /// </summary>
        [DataMember]
        [ProtoMember(5)]
        public String AttributeParentLongName
        {
            get { return _attributeParentLongName; }
            set { _attributeParentLongName = value; }
        }

        /// <summary>
        /// Property to decide if attribute is collection 
        /// </summary>
        [DataMember]
        [ProtoMember(6)]
        public Boolean IsCollection
        {
            get { return _isCollection; }
            set { _isCollection = value; }
        }

        /// <summary>
        /// Property to decide if attribute is a complex attribute 
        /// </summary>
        [DataMember]
        [ProtoMember(7)]
        public Boolean IsComplex
        {
            get { return _isComplex; }
            set { _isComplex = value; }
        }

        /// <summary>
        /// Property to decide if attribute is a lookup
        /// </summary>
        [DataMember]
        [ProtoMember(8)]
        public Boolean IsLookup
        {
            get { return _isLookup; }
            set { _isLookup = value; }
        }

        /// <summary>
        /// Property to decide if attribute is localizable 
        /// </summary>
        [DataMember]
        [ProtoMember(9), DefaultValue(true)]
        public Boolean IsLocalizable
        {
            get { return _isLocalizable; }
            set { _isLocalizable = value; }
        }

        /// <summary>
        /// Property which indicates whether attribute is read only
        /// </summary>
        [DataMember]
        [ProtoMember(10)]
        public Boolean ReadOnly
        {
            get { return _readOnly; }
            set { _readOnly = value; }
        }

        /// <summary>
        /// Property which indicates whether attribute is required
        /// </summary>
        [DataMember]
        [ProtoMember(11)]
        public Boolean Required
        {
            get { return _required; }
            set { _required = value; }
        }

        /// <summary>
        /// Property to decide if localization formatting is to be applied to attribute value 
        /// </summary>
        [DataMember]
        [ProtoMember(12), DefaultValue(true)]
        public Boolean ApplyLocaleFormat
        {
            get { return _applyLocaleFormat; }
            set { _applyLocaleFormat = value; }
        }

        /// <summary>
        /// Property to decide if timzone conversion is to be applied to attribute value 
        /// </summary>
        [DataMember]
        [ProtoMember(13)]
        public Boolean ApplyTimeZoneConversion
        {
            get { return _applyTimeZoneConversion; }
            set { _applyTimeZoneConversion = value; }
        }

        /// <summary>
        /// Property for attribute value source ('O' or 'I') 
        /// </summary>
        [DataMember]
        [ProtoMember(14)]
        public AttributeValueSource SourceFlag
        {
            get { return _sourceFlag; }
            set { _sourceFlag = value; }
        }

        /// <summary>
        /// Property denoting source entity for the values
        /// </summary>
        public Int64 SourceEntityId
        {
            get
            {
                if (this.SourceFlag == AttributeValueSource.Inherited)
                {
                    return SourceEntityIdInherited;
                }
                else
                {
                    return SourceEntityIdOverridden;
                }
            }
        }

        /// <summary>
        ///  indicates attribute value for given entity is coming frm which level
        /// </summary>
        public Int32 SourceClass
        {
            get
            {
                if (this.SourceFlag == AttributeValueSource.Inherited)
                {
                    return SourceClassInherited;
                }
                else
                {
                    return SourceClassOverridden;
                }
            }
        }

        /// <summary>
        /// Property denoting source entity for the overridden values (default is 0)
        /// </summary>
        [DataMember]
        [ProtoMember(15)]
        public Int64 SourceEntityIdOverridden
        {
            get { return _sourceEntityIdOverridden; }
            set { _sourceEntityIdOverridden = value; }
        }

        /// <summary>
        ///  indicates Overridden attribute value for given entity is coming from which level
        /// </summary>
        [DataMember]
        [ProtoMember(16)]
        public Int32 SourceClassOverridden
        {
            get { return _sourceClassOverridden; }
            set { _sourceClassOverridden = value; }
        }

        /// <summary>
        /// Property denoting source entity for the inherited values (default is 0)
        /// </summary>
        [DataMember]
        [ProtoMember(17)]
        public Int64 SourceEntityIdInherited
        {
            get { return _sourceEntityIdInherited; }
            set { _sourceEntityIdInherited = value; }
        }

        /// <summary>
        /// Property denoting source entity for the overridden values (default is 0)
        /// </summary>
        [DataMember]
        [ProtoMember(18)]
        public Int32 SourceClassInherited
        {
            get { return _sourceClassInherited; }
            set { _sourceClassInherited = value; }
        }

        /// <summary>
        /// Property for attribute value collection 
        /// </summary>
        //[DataMember]
        public ValueCollection OverriddenValues
        {
            //NOTE: This is just a ready only property. To set Value, developer need to use SetInheritedValue method.
            //This is not WCF DataMember. Rather the private field _overriddenValue is DataMember.
            get
            {
                ValueCollection values = (ValueCollection)GetValue(AttributeValueSource.Overridden, this.Locale, true);
                return values;
            }
        }

        /// <summary>
        /// Property for inherit attribute value collection 
        /// </summary>
        public ValueCollection InheritedValues
        {
            //NOTE: This is just a ready only property. To set Value, developer need to use SetInheritedValue method.
            //This is not WCF DataMember. Rather the private field _inheritedValues is DataMember.
            get
            {
                ValueCollection values = (ValueCollection)GetValue(AttributeValueSource.Inherited, this.Locale, true);
                return values;
            }
        }

        /// <summary>
        /// Property to indicate how many precision will attribute value have
        /// </summary>
        [DataMember]
        [ProtoMember(19)]
        public Int32 Precision
        {
            get { return _precision; }
            set { _precision = value; }
        }

        /// <summary>
        /// Property denoting collection of child attributes.
        /// </summary>
        [DataMember]
        [ProtoMember(20)]
        public AttributeCollection Attributes
        {
            get
            {
                return this._attributes;
            }
            set
            {
                this._attributes = value;
            }
        }

        /// <summary>
        /// Property denoting the data type of attribute. for possible values, see <see cref="MDM.Core.AttributeDataType"/>
        /// </summary>
        [DataMember]
        [ProtoMember(21)]
        public AttributeDataType AttributeDataType
        {
            get
            {
                return this._attributeDataType;
            }
            set
            {
                this._attributeDataType = value;
                
                if (this._attributeDataType == AttributeDataType.Hierarchical)
                {
                    this._isHierarchical = true;
                }
            }
        }

        /// <summary>
        /// Property denoting the model type of an attribute. It indicates whether an attribute is common attribute or technical attribute.
        /// For possible values, see <see cref="AttributeModelType" />
        /// </summary>
        [DataMember]
        [ProtoMember(22)]
        public AttributeModelType AttributeModelType
        {
            get
            {
                return this._attributeModelType;
            }
            set
            {
                this._attributeModelType = value;
            }
        }

        /// <summary>
        /// Property denoting the type of attribute. for possible values, see <see cref="MDM.Core.AttributeTypeEnum"/>
        /// </summary>
        [DataMember]
        [ProtoMember(23)]
        public MDM.Core.AttributeTypeEnum AttributeType
        {
            get
            {
                return this._attributeType;
            }
            set
            {
                this._attributeType = value;
            }
        }

        /// <summary>
        /// Property denoting value reference id.
        /// For complex attributes, InstanceRefId represents the PK of respective complex attribute (tbcx_) table
        /// </summary>
        [DataMember]
        [ProtoMember(24)]
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
        /// Property denoting sequence of current attribute.
        /// This is used in case of complex and complex collection attribute
        /// </summary>
        [DataMember]
        [ProtoMember(25), DefaultValue(-1)]
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
        /// Property denoting the entity id this attribute belongs to
        /// </summary>
        [DataMember]
        [ProtoMember(26)]
        public Int64 EntityId
        {
            get
            {
                return this._entityId;
            }
            set
            {
                this._entityId = value;
            }
        }

        /// <summary>
        /// Property denoting if arbitrary precision is used for Decimal attributes.
        /// If value of IsPrecisionArbitrary is true, then decimal value 12.2 with precision 3 will be stored as "12.2"
        /// If value of IsPrecisionArbitrary is false, then decimal value 12.2 with precision 3 will be stored as "12.200"
        /// </summary>
        [DataMember]
        [ProtoMember(27)]
        public Boolean IsPrecisionArbitrary
        {
            get
            {
                return this._isPrecisionArbitrary;
            }
            set
            {
                this._isPrecisionArbitrary = value;
            }
        }

        /// <summary>
        /// Property to decide if attribute is a complex attribute 
        /// </summary>
        //[DataMember]
        //[ProtoMember(28)]
        public Boolean IsHierarchical
        {
            get { return _isHierarchical; }
            set { _isHierarchical = value; }
        }

        //Smart properties..

        /// <summary>
        /// Property to get the value of attribute as Object
        /// </summary>
        public Object OverriddenValue
        {
            get
            {
                return this.GetOverriddenValue();
            }
        }

        /// <summary>
        /// Property to check if attribute value has been changed
        /// </summary>
        public bool HasChanged
        {
            get
            {
                return this.CheckHasChanged();
            }
        }

        /// <summary>
        /// Get current value based on Source
        /// If Source = Overridden then returns OverriddenValues
        /// If Source = Inherited then returns InheritedValues
        /// </summary>
        public ValueCollection CurrentValues
        {
            get
            {
                return (ValueCollection)this.GetCurrentValues();
            }
        }

        /// <summary>
        /// Property to get the current (considering source flag) value of attribute as Object
        /// </summary>
        /// <exception cref="NotSupportedException">Value property cannot be accessible when attribute is collection.</exception>
        public Object CurrentValue
        {
            get
            {
                return this.GetCurrentValue();
            }
        }
        
        /// <summary>
        /// Indicates if inherited value for attribute is invalid based on validations
        /// </summary>
        public Boolean HasInvalidInheritedValues
        {
            get
            {
                if (this._inheritedValues != null)
                {
                    return this._inheritedValues.HasInvalidValues;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (this._inheritedValues == null)
                {
                    this._inheritedValues = new ValueCollection();
                }

                this._inheritedValues.HasInvalidValues = value;
            }
        }

        /// <summary>
        /// Indicates if overridden value for attribute is invalid based on validations
        /// </summary>
        public Boolean HasInvalidOverriddenValues
        {
            get
            {
                if (this._overriddenValues != null)
                {
                    return this._overriddenValues.HasInvalidValues;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (this._overriddenValues == null)
                {
                    this._overriddenValues = new ValueCollection();
                }

                this._overriddenValues.HasInvalidValues = value;
            }
        }

        /// <summary>
        /// Indicates if current value for attribute is invalid based on validations
        /// </summary>
        public Boolean HasInvalidValues
        {
            get
            {
                Boolean hasInvalidValues = false;

                if (this.SourceFlag == AttributeValueSource.Inherited)
                {
                    hasInvalidValues = this.HasInvalidInheritedValues;
                }
                else if (this.SourceFlag == AttributeValueSource.Overridden)
                {
                    hasInvalidValues = this.HasInvalidOverriddenValues;
                }

                return hasInvalidValues;
            }
            set
            {
                if (this.SourceFlag == AttributeValueSource.Inherited)
                {
                    this.HasInvalidInheritedValues = value;
                }
                else if (this.SourceFlag == AttributeValueSource.Overridden)
                {
                    this.HasInvalidOverriddenValues = value;
                }
            }
        }

        #region Private properties

        /// <summary>
        /// Property denoting AttributeType name. This value is same as there in Tb_Attribute.AttributeTypeName which is coming from tb_AttributeType
        /// Using this property, we load proper values in <see cref="AttributeTypeEnum"/> and <see cref="AttributeModelType"/>
        /// This property is private because user doesn't know exact string value to be assigned to this property. We will decide this value based on <see cref="AttributeType"/> and <see cref="AttributeModelType"/>
        /// </summary>
        private String DBAttributeType
        {
            get
            {
                //TODO :: This property is not exposed to user. So we need to find proper values (which are there in tb_AttributeType) based on AttributeModelType and AttributeType.
                // But currently we are not having all values in enum for all values of tb_AttributeType. So passing always empty to SP. SP need to populate correct value for it.
                return this._dbAttributeType;
            }
            set
            {
                this._dbAttributeType = value;
                this.AttributeType = Utility.GetAttributeTypeFromAttributeTypeName(this._dbAttributeType, this.IsCollection);
                this.AttributeModelType = Utility.GetAttributeModelTypeFromAttributeTypeName(this._dbAttributeType);
            }
        }

        /// <summary>
        /// Culture used for storing value in database. 
        /// </summary>
        private String DataStorageCultureName
        {
            get
            {
                return LocaleEnum.en_US.GetCultureName();
            }
        }

        #endregion Private properties

        #endregion

        #region Methods

        #region Public Methods

        #region Misc Methods

        /// <summary>
        /// Populate current object from incoming XML
        /// </summary>
        /// <example>
        /// Sample XML
        /// <para>
        ///  <Attribute 
        ///     Id="6588" 
        ///     Name="VentureRetail" 
        ///     LongName="VentureRetail" 
        ///     InstanceRefId="19549" 
        ///     AttributeParentId="3027" 
        ///     AttributeParentName="Vendor" 
        ///     AttributeParentLongName="Vendor" 
        ///     IsCollection="True" 
        ///     IsComplex="True" 
        ///     IsHierarchical="True"
        ///     IsLocalizable="True" 
        ///     ApplyLocaleFormat="True" 
        ///     ApplyTimeZoneConversion="False" 
        ///     Precision="0" 
        ///     AttributeType="ComplexCollection" 
        ///     AttributeModelType="Common" 
        ///     AttributeDataType="Decimal" 
        ///     SourceFlag="O" 
        ///     Action="Read">
        ///     <Values SourceFlag="Overridden">
        ///       <Value Uom="" ValueRefId="-1" Sequence="0" Locale="en_WW"><![CDATA[19549]]></Value>
        ///       <Value Uom="" ValueRefId="-1" Sequence="0" Locale="en_WW"><![CDATA[19550]]></Value>
        ///     </Values>
        ///     <Values SourceFlag="Inherited" />
        ///     <Attributes>
        ///       <Attribute Id="6588" Name="VentureRetail" LongName="VentureRetail" InstanceRefId="19549" AttributeParentId="3027" AttributeParentName="Vendor" 
        ///                  AttributeParentLongName="Vendor" IsCollection="False" IsComplex="True" IsLocalizable="True" ApplyLocaleFormat="True" 
        ///                  ApplyTimeZoneConversion="False" Precision="0" AttributeTypeEnum="ComplexCollection" AttributeModelType="Common" AttributeDataType="Decimal" 
        ///                  SourceFlag="O" Action="Read">
        ///         <Values SourceFlag="Overridden">
        ///           <Value Uom="" ValueRefId="-1" Sequence="0" Locale="en_WW"><![CDATA[19549]]></Value>
        ///         </Values>
        ///         <Values SourceFlag="Inherited" />
        ///         <Attributes>
        ///           <Attribute Id="6589" Name="Brand" LongName="Brand" InstanceRefId="19549" AttributeParentId="6588" AttributeParentName="VentureRetail" 
        ///                      AttributeParentLongName="Venture Retail" IsCollection="False" IsComplex="False" IsLocalizable="True" ApplyLocaleFormat="True" 
        ///                      ApplyTimeZoneConversion="False" Precision="0" AttributeTypeEnum="Simple" AttributeModelType="Common" AttributeDataType="String" 
        ///                      SourceFlag="O" Action="Read">
        ///             <Values SourceFlag="Overridden">
        ///               <Value Uom="" ValueRefId="-1" Sequence="1" Locale="en_WW"><![CDATA[Fingerhut]]></Value>
        ///             </Values>
        ///             <Values SourceFlag="Inherited" />
        ///             <Attributes />
        ///           </Attribute>
        ///           <Attribute Id="6594" Name="Campaign" LongName="Campaign" InstanceRefId="19549" AttributeParentId="6588" AttributeParentName="VentureRetail"
        ///                      AttributeParentLongName="Venture Retail" IsCollection="False" IsComplex="False" IsLocalizable="True" ApplyLocaleFormat="True" 
        ///                      ApplyTimeZoneConversion="False" Precision="0" AttributeTypeEnum="Simple" AttributeModelType="Common" AttributeDataType="String" 
        ///                      SourceFlag="O" Action="Read">
        ///             <Values SourceFlag="Overridden">
        ///               <Value Uom="" ValueRefId="-1" Sequence="1" Locale="en_WW"><![CDATA[WEB]]></Value>
        ///             </Values>
        ///             <Values SourceFlag="Inherited" />
        ///             <Attributes />
        ///           </Attribute>
        ///         </Attributes>
        ///       </Attribute>
        ///     </Attributes>
        ///  </Attribute>
        ///  <Attribute Id="7183" Name="ChemicalInformation" LongName="ChemicalInformation" InstanceRefId="-1" Sequence="-1" AttributeParentId="7182" AttributeParentName="ProductInformation" AttributeType="ComplexCollection" AttributeDataType="String" Locale="en_WW" Action="Add">
        ///    <Attributes>
        ///      <Attribute Id="7183" Name="ChemicalInformation Instance Record" LongName="ChemicalInformation Instance Record" InstanceRefId="3" Sequence="0" AttributeParentId="7182" AttributeParentName="ProductInformation" AttributeType="Complex" AttributeDataType="String" Locale="en_WW" Action="Add">
        ///        <Attributes>
        ///          <Attribute Id="7186" Name="chemicalIngredientOrganisation" LongName="ChemicalIngredientOrganisation" InstanceRefId="-1" Sequence="-1" AttributeParentId="7183" AttributeParentName="ChemicalInformation" AttributeType="Simple" AttributeDataType="String" Locale="en_WW" Action="Add">
        ///            <Values>
        ///              <Value Id="-1" Uom="" ValueRefId="0" Sequence="-1" DisplayValue="" HasInvalidValue="False" Locale="en_WW" Action="Add"><![CDATA[Org2]]></Value>
        ///            </Values>
        ///          </Attribute>
        ///          <Attribute Id="7187" Name="chemicalIngredientScheme" LongName="ChemicalIngredientScheme" InstanceRefId="-1" Sequence="-1" AttributeParentId="7183" AttributeParentName="ChemicalInformation" AttributeType="Simple" AttributeDataType="String" Locale="en_WW" Action="Add">
        ///            <Values>
        ///              <Value Id="-1" Uom="" ValueRefId="0" Sequence="-1" DisplayValue="" HasInvalidValue="False" Locale="en_WW" Action="Add"><![CDATA[Scheme2]]></Value>
        ///            </Values>
        ///          </Attribute>
        ///          <Attribute Id="7184" Name="ChemicalIngredient" LongName="ChemicalIngredient" InstanceRefId="-1" Sequence="-1" AttributeParentId="7182" AttributeParentName="ProductInformation" AttributeType="ComplexCollection" AttributeDataType="String" Locale="en_WW" Action="Add">
        ///            <Attributes>
        ///              <Attribute Id="7184" Name="ChemicalIngredient Instance Record" LongName="ChemicalIngredient Instance Record" InstanceRefId="2" Sequence="0" AttributeParentId="7182" AttributeParentName="ProductInformation" AttributeType="Complex" AttributeDataType="String" Locale="en_WW" Action="Add">
        ///                <Attributes>
        ///                  <Attribute Id="7188" Name="chemicalIngredientConcentration" LongName="ChemicalIngredientConcentration" InstanceRefId="-1" Sequence="-1" AttributeParentId="7184" AttributeParentName="ChemicalIngredient" AttributeType="Simple" AttributeDataType="Decimal" Locale="en_WW" Action="Add">
        ///                    <Values>
        ///                      <Value Id="-1" Uom="ml" ValueRefId="0" Sequence="-1" DisplayValue="" HasInvalidValue="False" Locale="en_WW" Action="Add"><![CDATA[200.00]]></Value>
        ///                    </Values>
        ///                  </Attribute>
        ///                  <Attribute Id="7189" Name="chemicalIngredientConcentrationBasis" LongName="ChemicalIngredientConcentrationBasis" InstanceRefId="-1" Sequence="-1" AttributeParentId="7184" AttributeParentName="ChemicalIngredient" AttributeType="Simple" AttributeDataType="Decimal" Locale="en_WW" Action="Add">
        ///                    <Values>
        ///                      <Value Id="-1" Uom="ml" ValueRefId="0" Sequence="-1" DisplayValue="" HasInvalidValue="False" Locale="en_WW" Action="Add"><![CDATA[210.00]]></Value>
        ///                    </Values>
        ///                  </Attribute>
        ///                  <Attribute Id="7190" Name="chemicalIngredientIdentification" LongName="ChemicalIngredientIdentification" InstanceRefId="-1" Sequence="-1" AttributeParentId="7184" AttributeParentName="ChemicalIngredient" AttributeType="Simple" AttributeDataType="String" Locale="en_WW" Action="Add">
        ///                    <Values>
        ///                      <Value Id="-1" Uom="" ValueRefId="0" Sequence="-1" DisplayValue="" HasInvalidValue="False" Locale="en_WW" Action="Add"><![CDATA[Identification2]]></Value>
        ///                    </Values>
        ///                  </Attribute>
        ///                  <Attribute Id="7191" Name="chemicalIngredientName" LongName="ChemicalIngredientName" InstanceRefId="-1" Sequence="-1" AttributeParentId="7184" AttributeParentName="ChemicalIngredient" AttributeType="Simple" AttributeDataType="String" Locale="en_WW" Action="Add">
        ///                    <Values>
        ///                      <Value Id="-1" Uom="" ValueRefId="0" Sequence="-1" DisplayValue="" HasInvalidValue="False" Locale="en_WW" Action="Add"><![CDATA[Ingredient2]]></Value>
        ///                    </Values>
        ///                  </Attribute>
        ///                  <Attribute Id="7192" Name="reachChemicalRegistrationNumber" LongName="ReachChemicalRegistrationNumber" InstanceRefId="-1" Sequence="-1" AttributeParentId="7184" AttributeParentName="ChemicalIngredient" AttributeType="Simple" AttributeDataType="String" Locale="en_WW" Action="Add">
        ///                    <Values>
        ///                      <Value Id="-1" Uom="" ValueRefId="0" Sequence="-1" DisplayValue="" HasInvalidValue="False" Locale="en_WW" Action="Add"><![CDATA[Reg2]]></Value>
        ///                    </Values>
        ///                  </Attribute>
        ///                  <Attribute Id="7185" Name="LethalDoseConcentrationInformation" LongName="LethalDoseConcentrationInformation" InstanceRefId="-1" Sequence="-1" AttributeParentId="7182" AttributeParentName="ProductInformation" AttributeType="ComplexCollection" AttributeDataType="String" Locale="en_WW" Action="Add">
        ///                    <Attributes>
        ///                      <Attribute Id="7185" Name="LethalDoseConcentrationInformation Instance Record" LongName="LethalDoseConcentrationInformation Instance Record" InstanceRefId="1" Sequence="0" AttributeParentId="7182" AttributeParentName="ProductInformation" AttributeType="Complex" AttributeDataType="String" Locale="en_WW" Action="Add">
        ///                        <Attributes>
        ///                          <Attribute Id="7193" Name="lethalConcentration50" LongName="LethalConcentration50" InstanceRefId="-1" Sequence="-1" AttributeParentId="7185" AttributeParentName="LethalDoseConcentrationInformation" AttributeType="Simple" AttributeDataType="Decimal" Locale="en_WW" Action="Add">
        ///                            <Values>
        ///                              <Value Id="-1" Uom="ml" ValueRefId="0" Sequence="-1" DisplayValue="" HasInvalidValue="False" Locale="en_WW" Action="Add"><![CDATA[200.00]]></Value>
        ///                            </Values>
        ///                          </Attribute>
        ///                          <Attribute Id="7194" Name="lethalConcentration50Basis" LongName="LethalConcentration50Basis" InstanceRefId="-1" Sequence="-1" AttributeParentId="7185" AttributeParentName="LethalDoseConcentrationInformation" AttributeType="Simple" AttributeDataType="Decimal" Locale="en_WW" Action="Add">
        ///                            <Values>
        ///                              <Value Id="-1" Uom="ml" ValueRefId="0" Sequence="-1" DisplayValue="" HasInvalidValue="False" Locale="en_WW" Action="Add"><![CDATA[210.00]]></Value>
        ///                            </Values>
        ///                          </Attribute>
        ///                          <Attribute Id="7195" Name="lethalDose50" LongName="LethalDose50" InstanceRefId="-1" Sequence="-1" AttributeParentId="7185" AttributeParentName="LethalDoseConcentrationInformation" AttributeType="Simple" AttributeDataType="Decimal" Locale="en_WW" Action="Add">
        ///                            <Values>
        ///                              <Value Id="-1" Uom="ml" ValueRefId="0" Sequence="-1" DisplayValue="" HasInvalidValue="False" Locale="en_WW" Action="Add"><![CDATA[210.00]]></Value>
        ///                            </Values>
        ///                          </Attribute>
        ///                          <Attribute Id="7196" Name="lethalDose50Basis" LongName="LethalDose50Basis" InstanceRefId="-1" Sequence="-1" AttributeParentId="7185" AttributeParentName="LethalDoseConcentrationInformation" AttributeType="Simple" AttributeDataType="Decimal" Locale="en_WW" Action="Add">
        ///                            <Values>
        ///                              <Value Id="-1" Uom="ml" ValueRefId="0" Sequence="-1" DisplayValue="" HasInvalidValue="False" Locale="en_WW" Action="Add"><![CDATA[230.00]]></Value>
        ///                            </Values>
        ///                          </Attribute>
        ///                          <Attribute Id="7197" Name="routeOfExposureCode" LongName="RouteOfExposureCode" InstanceRefId="-1" Sequence="-1" AttributeParentId="7185" AttributeParentName="LethalDoseConcentrationInformation" AttributeType="Simple" AttributeDataType="String" Locale="en_WW" Action="Add">
        ///                            <Values>
        ///                              <Value Id="-1" Uom="" ValueRefId="0" Sequence="-1" DisplayValue="" HasInvalidValue="False" Locale="en_WW" Action="Add"><![CDATA[]]></Value>
        ///                            </Values>
        ///                          </Attribute>
        ///                          <Attribute Id="7198" Name="testSpeciesCode" LongName="TestSpeciesCode" InstanceRefId="-1" Sequence="-1" AttributeParentId="7185" AttributeParentName="LethalDoseConcentrationInformation" AttributeType="Simple" AttributeDataType="String" Locale="en_WW" Action="Add">
        ///                            <Values>
        ///                              <Value Id="-1" Uom="" ValueRefId="0" Sequence="-1" DisplayValue="" HasInvalidValue="False" Locale="en_WW" Action="Add"><![CDATA[]]></Value>
        ///                            </Values>
        ///                          </Attribute>
        ///                          <Attribute Id="7199" Name="testSpeciesDescription" LongName="TestSpeciesDescription" InstanceRefId="-1" Sequence="-1" AttributeParentId="7185" AttributeParentName="LethalDoseConcentrationInformation" AttributeType="Simple" AttributeDataType="String" Locale="en_WW" Action="Add">
        ///                            <Values>
        ///                              <Value Id="-1" Uom="" ValueRefId="0" Sequence="-1" DisplayValue="" HasInvalidValue="False" Locale="en_WW" Action="Add"><![CDATA[Description2]]></Value>
        ///                            </Values>
        ///                          </Attribute>
        ///                        </Attributes>
        ///                      </Attribute>
        ///                    </Attributes>
        ///                  </Attribute>
        ///                </Attributes>
        ///              </Attribute>
        ///            </Attributes>
        ///          </Attribute>
        ///        </Attributes>
        ///      </Attribute>
        ///    </Attributes>
        ///  </Attribute>
        /// </para>
        /// </example>
        /// <param name="valuesAsXml">XML representation for attribute from which object is to be created</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        public void LoadAttribute(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
          LoadAttribute(valuesAsXml, false, objectSerialization);
        }

        /// <summary>
        /// Populate current object from incoming XML
        /// </summary>
        /// <example>
        /// Sample XML
        /// <para>
        ///  <Attribute 
        ///     Id="6588" 
        ///     Name="VentureRetail" 
        ///     LongName="VentureRetail" 
        ///     InstanceRefId="19549" 
        ///     AttributeParentId="3027" 
        ///     AttributeParentName="Vendor" 
        ///     AttributeParentLongName="Vendor" 
        ///     IsCollection="True" 
        ///     IsComplex="True" 
        ///     IsHierarchical="True"
        ///     IsLocalizable="True" 
        ///     ApplyLocaleFormat="True" 
        ///     ApplyTimeZoneConversion="False" 
        ///     Precision="0" 
        ///     AttributeType="ComplexCollection" 
        ///     AttributeModelType="Common" 
        ///     AttributeDataType="Decimal" 
        ///     SourceFlag="O" 
        ///     Action="Read">
        ///     <Values SourceFlag="Overridden">
        ///       <Value Uom="" ValueRefId="-1" Sequence="0" Locale="en_WW"><![CDATA[19549]]></Value>
        ///       <Value Uom="" ValueRefId="-1" Sequence="0" Locale="en_WW"><![CDATA[19550]]></Value>
        ///     </Values>
        ///     <Values SourceFlag="Inherited" />
        ///     <Attributes>
        ///       <Attribute Id="6588" Name="VentureRetail" LongName="VentureRetail" InstanceRefId="19549" AttributeParentId="3027" AttributeParentName="Vendor" 
        ///                  AttributeParentLongName="Vendor" IsCollection="False" IsComplex="True" IsLocalizable="True" ApplyLocaleFormat="True" 
        ///                  ApplyTimeZoneConversion="False" Precision="0" AttributeTypeEnum="ComplexCollection" AttributeModelType="Common" AttributeDataType="Decimal" 
        ///                  SourceFlag="O" Action="Read">
        ///         <Values SourceFlag="Overridden">
        ///           <Value Uom="" ValueRefId="-1" Sequence="0" Locale="en_WW"><![CDATA[19549]]></Value>
        ///         </Values>
        ///         <Values SourceFlag="Inherited" />
        ///         <Attributes>
        ///           <Attribute Id="6589" Name="Brand" LongName="Brand" InstanceRefId="19549" AttributeParentId="6588" AttributeParentName="VentureRetail" 
        ///                      AttributeParentLongName="Venture Retail" IsCollection="False" IsComplex="False" IsLocalizable="True" ApplyLocaleFormat="True" 
        ///                      ApplyTimeZoneConversion="False" Precision="0" AttributeTypeEnum="Simple" AttributeModelType="Common" AttributeDataType="String" 
        ///                      SourceFlag="O" Action="Read">
        ///             <Values SourceFlag="Overridden">
        ///               <Value Uom="" ValueRefId="-1" Sequence="1" Locale="en_WW"><![CDATA[Fingerhut]]></Value>
        ///             </Values>
        ///             <Values SourceFlag="Inherited" />
        ///             <Attributes />
        ///           </Attribute>
        ///           <Attribute Id="6594" Name="Campaign" LongName="Campaign" InstanceRefId="19549" AttributeParentId="6588" AttributeParentName="VentureRetail"
        ///                      AttributeParentLongName="Venture Retail" IsCollection="False" IsComplex="False" IsLocalizable="True" ApplyLocaleFormat="True" 
        ///                      ApplyTimeZoneConversion="False" Precision="0" AttributeTypeEnum="Simple" AttributeModelType="Common" AttributeDataType="String" 
        ///                      SourceFlag="O" Action="Read">
        ///             <Values SourceFlag="Overridden">
        ///               <Value Uom="" ValueRefId="-1" Sequence="1" Locale="en_WW"><![CDATA[WEB]]></Value>
        ///             </Values>
        ///             <Values SourceFlag="Inherited" />
        ///             <Attributes />
        ///           </Attribute>
        ///         </Attributes>
        ///       </Attribute>
        ///     </Attributes>
        ///  </Attribute>
        ///  <Attribute Id="7183" Name="ChemicalInformation" LongName="ChemicalInformation" InstanceRefId="-1" Sequence="-1" AttributeParentId="7182" AttributeParentName="ProductInformation" AttributeType="ComplexCollection" AttributeDataType="String" Locale="en_WW" Action="Add">
        ///    <Attributes>
        ///      <Attribute Id="7183" Name="ChemicalInformation Instance Record" LongName="ChemicalInformation Instance Record" InstanceRefId="3" Sequence="0" AttributeParentId="7182" AttributeParentName="ProductInformation" AttributeType="Complex" AttributeDataType="String" Locale="en_WW" Action="Add">
        ///        <Attributes>
        ///          <Attribute Id="7186" Name="chemicalIngredientOrganisation" LongName="ChemicalIngredientOrganisation" InstanceRefId="-1" Sequence="-1" AttributeParentId="7183" AttributeParentName="ChemicalInformation" AttributeType="Simple" AttributeDataType="String" Locale="en_WW" Action="Add">
        ///            <Values>
        ///              <Value Id="-1" Uom="" ValueRefId="0" Sequence="-1" DisplayValue="" HasInvalidValue="False" Locale="en_WW" Action="Add"><![CDATA[Org2]]></Value>
        ///            </Values>
        ///          </Attribute>
        ///          <Attribute Id="7187" Name="chemicalIngredientScheme" LongName="ChemicalIngredientScheme" InstanceRefId="-1" Sequence="-1" AttributeParentId="7183" AttributeParentName="ChemicalInformation" AttributeType="Simple" AttributeDataType="String" Locale="en_WW" Action="Add">
        ///            <Values>
        ///              <Value Id="-1" Uom="" ValueRefId="0" Sequence="-1" DisplayValue="" HasInvalidValue="False" Locale="en_WW" Action="Add"><![CDATA[Scheme2]]></Value>
        ///            </Values>
        ///          </Attribute>
        ///          <Attribute Id="7184" Name="ChemicalIngredient" LongName="ChemicalIngredient" InstanceRefId="-1" Sequence="-1" AttributeParentId="7182" AttributeParentName="ProductInformation" AttributeType="ComplexCollection" AttributeDataType="String" Locale="en_WW" Action="Add">
        ///            <Attributes>
        ///              <Attribute Id="7184" Name="ChemicalIngredient Instance Record" LongName="ChemicalIngredient Instance Record" InstanceRefId="2" Sequence="0" AttributeParentId="7182" AttributeParentName="ProductInformation" AttributeType="Complex" AttributeDataType="String" Locale="en_WW" Action="Add">
        ///                <Attributes>
        ///                  <Attribute Id="7188" Name="chemicalIngredientConcentration" LongName="ChemicalIngredientConcentration" InstanceRefId="-1" Sequence="-1" AttributeParentId="7184" AttributeParentName="ChemicalIngredient" AttributeType="Simple" AttributeDataType="Decimal" Locale="en_WW" Action="Add">
        ///                    <Values>
        ///                      <Value Id="-1" Uom="ml" ValueRefId="0" Sequence="-1" DisplayValue="" HasInvalidValue="False" Locale="en_WW" Action="Add"><![CDATA[200.00]]></Value>
        ///                    </Values>
        ///                  </Attribute>
        ///                  <Attribute Id="7189" Name="chemicalIngredientConcentrationBasis" LongName="ChemicalIngredientConcentrationBasis" InstanceRefId="-1" Sequence="-1" AttributeParentId="7184" AttributeParentName="ChemicalIngredient" AttributeType="Simple" AttributeDataType="Decimal" Locale="en_WW" Action="Add">
        ///                    <Values>
        ///                      <Value Id="-1" Uom="ml" ValueRefId="0" Sequence="-1" DisplayValue="" HasInvalidValue="False" Locale="en_WW" Action="Add"><![CDATA[210.00]]></Value>
        ///                    </Values>
        ///                  </Attribute>
        ///                  <Attribute Id="7190" Name="chemicalIngredientIdentification" LongName="ChemicalIngredientIdentification" InstanceRefId="-1" Sequence="-1" AttributeParentId="7184" AttributeParentName="ChemicalIngredient" AttributeType="Simple" AttributeDataType="String" Locale="en_WW" Action="Add">
        ///                    <Values>
        ///                      <Value Id="-1" Uom="" ValueRefId="0" Sequence="-1" DisplayValue="" HasInvalidValue="False" Locale="en_WW" Action="Add"><![CDATA[Identification2]]></Value>
        ///                    </Values>
        ///                  </Attribute>
        ///                  <Attribute Id="7191" Name="chemicalIngredientName" LongName="ChemicalIngredientName" InstanceRefId="-1" Sequence="-1" AttributeParentId="7184" AttributeParentName="ChemicalIngredient" AttributeType="Simple" AttributeDataType="String" Locale="en_WW" Action="Add">
        ///                    <Values>
        ///                      <Value Id="-1" Uom="" ValueRefId="0" Sequence="-1" DisplayValue="" HasInvalidValue="False" Locale="en_WW" Action="Add"><![CDATA[Ingredient2]]></Value>
        ///                    </Values>
        ///                  </Attribute>
        ///                  <Attribute Id="7192" Name="reachChemicalRegistrationNumber" LongName="ReachChemicalRegistrationNumber" InstanceRefId="-1" Sequence="-1" AttributeParentId="7184" AttributeParentName="ChemicalIngredient" AttributeType="Simple" AttributeDataType="String" Locale="en_WW" Action="Add">
        ///                    <Values>
        ///                      <Value Id="-1" Uom="" ValueRefId="0" Sequence="-1" DisplayValue="" HasInvalidValue="False" Locale="en_WW" Action="Add"><![CDATA[Reg2]]></Value>
        ///                    </Values>
        ///                  </Attribute>
        ///                  <Attribute Id="7185" Name="LethalDoseConcentrationInformation" LongName="LethalDoseConcentrationInformation" InstanceRefId="-1" Sequence="-1" AttributeParentId="7182" AttributeParentName="ProductInformation" AttributeType="ComplexCollection" AttributeDataType="String" Locale="en_WW" Action="Add">
        ///                    <Attributes>
        ///                      <Attribute Id="7185" Name="LethalDoseConcentrationInformation Instance Record" LongName="LethalDoseConcentrationInformation Instance Record" InstanceRefId="1" Sequence="0" AttributeParentId="7182" AttributeParentName="ProductInformation" AttributeType="Complex" AttributeDataType="String" Locale="en_WW" Action="Add">
        ///                        <Attributes>
        ///                          <Attribute Id="7193" Name="lethalConcentration50" LongName="LethalConcentration50" InstanceRefId="-1" Sequence="-1" AttributeParentId="7185" AttributeParentName="LethalDoseConcentrationInformation" AttributeType="Simple" AttributeDataType="Decimal" Locale="en_WW" Action="Add">
        ///                            <Values>
        ///                              <Value Id="-1" Uom="ml" ValueRefId="0" Sequence="-1" DisplayValue="" HasInvalidValue="False" Locale="en_WW" Action="Add"><![CDATA[200.00]]></Value>
        ///                            </Values>
        ///                          </Attribute>
        ///                          <Attribute Id="7194" Name="lethalConcentration50Basis" LongName="LethalConcentration50Basis" InstanceRefId="-1" Sequence="-1" AttributeParentId="7185" AttributeParentName="LethalDoseConcentrationInformation" AttributeType="Simple" AttributeDataType="Decimal" Locale="en_WW" Action="Add">
        ///                            <Values>
        ///                              <Value Id="-1" Uom="ml" ValueRefId="0" Sequence="-1" DisplayValue="" HasInvalidValue="False" Locale="en_WW" Action="Add"><![CDATA[210.00]]></Value>
        ///                            </Values>
        ///                          </Attribute>
        ///                          <Attribute Id="7195" Name="lethalDose50" LongName="LethalDose50" InstanceRefId="-1" Sequence="-1" AttributeParentId="7185" AttributeParentName="LethalDoseConcentrationInformation" AttributeType="Simple" AttributeDataType="Decimal" Locale="en_WW" Action="Add">
        ///                            <Values>
        ///                              <Value Id="-1" Uom="ml" ValueRefId="0" Sequence="-1" DisplayValue="" HasInvalidValue="False" Locale="en_WW" Action="Add"><![CDATA[210.00]]></Value>
        ///                            </Values>
        ///                          </Attribute>
        ///                          <Attribute Id="7196" Name="lethalDose50Basis" LongName="LethalDose50Basis" InstanceRefId="-1" Sequence="-1" AttributeParentId="7185" AttributeParentName="LethalDoseConcentrationInformation" AttributeType="Simple" AttributeDataType="Decimal" Locale="en_WW" Action="Add">
        ///                            <Values>
        ///                              <Value Id="-1" Uom="ml" ValueRefId="0" Sequence="-1" DisplayValue="" HasInvalidValue="False" Locale="en_WW" Action="Add"><![CDATA[230.00]]></Value>
        ///                            </Values>
        ///                          </Attribute>
        ///                          <Attribute Id="7197" Name="routeOfExposureCode" LongName="RouteOfExposureCode" InstanceRefId="-1" Sequence="-1" AttributeParentId="7185" AttributeParentName="LethalDoseConcentrationInformation" AttributeType="Simple" AttributeDataType="String" Locale="en_WW" Action="Add">
        ///                            <Values>
        ///                              <Value Id="-1" Uom="" ValueRefId="0" Sequence="-1" DisplayValue="" HasInvalidValue="False" Locale="en_WW" Action="Add"><![CDATA[]]></Value>
        ///                            </Values>
        ///                          </Attribute>
        ///                          <Attribute Id="7198" Name="testSpeciesCode" LongName="TestSpeciesCode" InstanceRefId="-1" Sequence="-1" AttributeParentId="7185" AttributeParentName="LethalDoseConcentrationInformation" AttributeType="Simple" AttributeDataType="String" Locale="en_WW" Action="Add">
        ///                            <Values>
        ///                              <Value Id="-1" Uom="" ValueRefId="0" Sequence="-1" DisplayValue="" HasInvalidValue="False" Locale="en_WW" Action="Add"><![CDATA[]]></Value>
        ///                            </Values>
        ///                          </Attribute>
        ///                          <Attribute Id="7199" Name="testSpeciesDescription" LongName="TestSpeciesDescription" InstanceRefId="-1" Sequence="-1" AttributeParentId="7185" AttributeParentName="LethalDoseConcentrationInformation" AttributeType="Simple" AttributeDataType="String" Locale="en_WW" Action="Add">
        ///                            <Values>
        ///                              <Value Id="-1" Uom="" ValueRefId="0" Sequence="-1" DisplayValue="" HasInvalidValue="False" Locale="en_WW" Action="Add"><![CDATA[Description2]]></Value>
        ///                            </Values>
        ///                          </Attribute>
        ///                        </Attributes>
        ///                      </Attribute>
        ///                    </Attributes>
        ///                  </Attribute>
        ///                </Attributes>
        ///              </Attribute>
        ///            </Attributes>
        ///          </Attribute>
        ///        </Attributes>
        ///      </Attribute>
        ///    </Attributes>
        ///  </Attribute>
        /// </para>
        /// </example>
        /// <param name="valuesAsXml">XML representation for attribute from which object is to be created</param>
        /// <param name="duplicateAttributeAllowed">Specifies if duplicate attributes are allowed</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        public void LoadAttribute(String valuesAsXml, Boolean duplicateAttributeAllowed, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                if (objectSerialization == ObjectSerialization.DataStorage)
                {
                    LoadAttributeForDataStorage(valuesAsXml);
                }
                else if (objectSerialization == ObjectSerialization.DataTransfer)
                {
                    LoadAttributeForDataTransfer(valuesAsXml, duplicateAttributeAllowed);
                }
                else
                {
                    XmlTextReader reader = null;
                    try
                    {
                        reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                        while (!reader.EOF)
                        {
                            if (reader.NodeType == XmlNodeType.Element && reader.Name == "Attribute")
                            {
                                #region Read Attribute Properties

                                if (reader.HasAttributes)
                                {
                                    LoadAttributeMetadataFromXml(reader);
                                }

                                #endregion
                            }
                            else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Values")
                            {
                                #region Read values for current attribute

                                //Read the Value XML first before reading SourceFlag because "reader.MoveToAttribute" will move the cursor forward and we won't get full XML after that.

                                if (reader.HasAttributes && reader.GetAttribute("SourceFlag") != null)
                                {
                                    AttributeValueSource srcFlag = Utility.GetSourceFlagEnum(reader.GetAttribute("SourceFlag"));

                                    //Get ValueCollection from XML
                                    String valueXml = reader.ReadOuterXml();
                                    if (!String.IsNullOrWhiteSpace(valueXml))
                                    {
                                        ValueCollection valCollection = new ValueCollection(valueXml);

                                        if (valCollection != null)
                                        {
                                            //Based on SourceFlag in Values node, populate ValueCollection in either inherited or overridden value collection
                                            switch (srcFlag)
                                            {
                                                case AttributeValueSource.Overridden:
                                                    PopulateOverriddenAttributeValues(valCollection);
                                                    break;
                                                case AttributeValueSource.Inherited:
                                                    PopulateInheritedAttributeValues(valCollection);
                                                    break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    //There are no attributes for 'values' element or Source Flag is not available..
                                    //In this case, the Input XML format will always be in 'External' serialization and having only overridden values
                                    //Get ValueCollection from XML
                                    String valueXml = reader.ReadOuterXml();
                                    if (!String.IsNullOrWhiteSpace(valueXml))
                                    {
                                        ValueCollection valCollection = new ValueCollection(valueXml);

                                        if (valCollection != null)
                                        {
                                            #region Populate Overridden values in object

                                            PopulateOverriddenAttributeValues(valCollection);

                                            #endregion Populate Overridden values in object
                                        }
                                    }
                                }

                                #endregion Read values for current attribute
                            }
                            else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Attributes")
                            {
                                #region Read child attributes
                                //Child attributes are collection under current attribute.

                                String attributeXml = reader.ReadOuterXml();
                                if (!String.IsNullOrEmpty(attributeXml))
                                {
                                    //Get collection of child attributes and populate it in Attribute collection of current attribute object.
                                    AttributeCollection attributeCollection = new AttributeCollection(attributeXml, duplicateAttributeAllowed || this.IsHierarchical);
                                    if (attributeCollection != null)
                                    {
                                        foreach (Attribute attr in attributeCollection)
                                        {
                                            // No need to use parent attribute locale when allowing multiple attribute objects of same id but their own locales.
                                            if (!duplicateAttributeAllowed)
                                            {
                                                attr.Locale = this.Locale;
                                            }

                                            this.Attributes.Add(attr, duplicateAttributeAllowed || this.IsHierarchical);
                                        }
                                    }
                                }

                                #endregion Read child attributes
                            }
                            else
                            {
                                //Keep on reading the XML until we reach expected node.
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
        /// Populate current object from incoming XML when Enum of ObjectSerialization is DataStorage
        /// </summary>
        /// <example>
        /// Sample XML
        /// <para>
        ///  <Attribute 
        ///     Id="6588" 
        ///     Name="VentureRetail" 
        ///     LName="VentureRetail" 
        ///     InstRefId="19549" 
        ///     AttrParentId="3027" 
        ///     AttrParentName="Vendor" 
        ///     AttrParentLongName="Vendor" 
        ///     IsCollection="True" 
        ///     IsComplex="True" 
        ///     IsLocalizable="True" 
        ///     ApplyLocaleFormat="True" 
        ///     ApplyTimeZoneConversion="False" 
        ///     Precision="0" 
        ///     AttrType="ComplexCollection" 
        ///     AttrModelType="Common" 
        ///     AttrDataType="Decimal" 
        ///     SourceFlag="O" 
        ///     Action="Read">
        ///     <Values SourceFlag="Overridden">
        ///       <Value Uom="" ValRefId="-1" Seq="0" Locale="en_WW"><![CDATA[19549]]></Value>
        ///       <Value Uom="" ValRefId="-1" Seq="0" Locale="en_WW"><![CDATA[19550]]></Value>
        ///     </Values>
        ///     <Values SourceFlag="Inherited" />
        ///     <Attributes>
        ///       <Attribute Id="6588" Name="VentureRetail" LName="VentureRetail" InstRefId="19549" AttrParentId="3027" AttrParentName="Vendor" 
        ///                  AttrParentLName="Vendor" IsCollection="False" IsComplex="True" IsLocalizable="True" ApplyLocaleFormat="True" 
        ///                  ApplyTimeZoneConversion="False" Precision="0" AttributeTypeEnum="ComplexCollection" AttrModelType="Common" AttrDataType="Decimal" 
        ///                  SourceFlag="O" Action="Read">
        ///         <Values SourceFlag="Overridden">
        ///           <Value Uom="" ValRefId="-1" Seq="0" Locale="en_WW"><![CDATA[19549]]></Value>
        ///         </Values>
        ///         <Values SourceFlag="Inherited" />
        ///         <Attributes>
        ///           <Attribute Id="6589" Name="Brand" LName="Brand" InstRefId="19549" AttrParentId="6588" AttrParentName="VentureRetail" 
        ///                      AttrParentLName="Venture Retail" IsCollection="False" IsComplex="False" IsLocalizable="True" ApplyLocaleFormat="True" 
        ///                      ApplyTimeZoneConversion="False" Precision="0" AttributeTypeEnum="Simple" AttrModelType="Common" AttrDataType="String" 
        ///                      SourceFlag="O" Action="Read">
        ///             <Values SourceFlag="Overridden">
        ///               <Value Uom="" ValRefId="-1" Seq="1" Locale="en_WW"><![CDATA[Fingerhut]]></Value>
        ///             </Values>
        ///             <Values SourceFlag="Inherited" />
        ///             <Attributes />
        ///           </Attribute>
        ///           <Attribute Id="6594" Name="Campaign" LName="Campaign" InstRefId="19549" AttrParentId="6588" AttrParentName="VentureRetail"
        ///                      AttrParentLName="Venture Retail" IsCollection="False" IsComplex="False" IsLocalizable="True" ApplyLocaleFormat="True" 
        ///                      ApplyTimeZoneConversion="False" Precision="0" AttributeTypeEnum="Simple" AttrModelType="Common" AttrDataType="String" 
        ///                      SourceFlag="O" Action="Read">
        ///             <Values SourceFlag="Overridden">
        ///               <Value Uom="" ValRefId="-1" Seq="1" Locale="en_WW"><![CDATA[WEB]]></Value>
        ///             </Values>
        ///             <Values SourceFlag="Inherited" />
        ///             <Attributes />
        ///           </Attribute>
        ///         </Attributes>
        ///       </Attribute>
        ///     </Attributes>
        ///  </Attribute>
        /// </para>
        /// </example>
        /// <param name="valuesAsXml">XML representation for attribute from which object is to be created</param>
        public void LoadAttributeForDataStorage(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Attribute")
                        {
                            #region Read Attribute Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = reader.ReadContentAsInt();
                                }

                                if (reader.MoveToAttribute("Name"))
                                {
                                    this.Name = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("LName"))
                                {
                                    this.LongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Action"))
                                {
                                    this.Action = ValueTypeHelper.GetAction(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("Locale"))
                                {
                                    LocaleEnum locale = LocaleEnum.UnKnown;
                                    Enum.TryParse(reader.ReadContentAsString(), out locale);
                                    this.Locale = locale;
                                }

                                if (reader.MoveToAttribute("ARefd"))
                                {
                                    this.AuditRefId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this.AuditRefId);
                                }

                                if (reader.MoveToAttribute("InstRefId"))
                                {
                                    Int32 refId = -1;
                                    Int32.TryParse(reader.ReadContentAsString(), out refId);
                                    this.InstanceRefId = refId;
                                }
                                if (reader.MoveToAttribute("Sew"))
                                {
                                    Decimal seq = -1;
                                    Decimal.TryParse(reader.ReadContentAsString(), out seq);
                                    this.Sequence = seq;
                                }
                                if (reader.MoveToAttribute("AttrParentId"))
                                {
                                    this.AttributeParentId = reader.ReadContentAsInt();
                                }

                                if (reader.MoveToAttribute("AttrParentName"))
                                {
                                    this.AttributeParentName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("AttrParentLName"))
                                {
                                    this.AttributeParentLongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("AttrDataType"))
                                {
                                    String strAttributeDataType = reader.ReadContentAsString();
                                    AttributeDataType attributeDataType = AttributeDataType.String;

                                    if (!String.IsNullOrWhiteSpace(strAttributeDataType))
                                        Enum.TryParse(strAttributeDataType, true, out attributeDataType);

                                    this.AttributeDataType = attributeDataType;
                                }

                                if (reader.MoveToAttribute("IsCollection"))
                                {
                                    this.IsCollection = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("IsComplex"))
                                {
                                    this.IsComplex = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("IsLookup"))
                                {
                                    this.IsLookup = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("ReadOnly"))
                                {
                                    this.ReadOnly = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("Req"))
                                {
                                    this.Required = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("AttributeTypeName"))
                                {
                                    String attributeTypeName = reader.ReadContentAsString();
                                    //Here IsCollection is populated before this call. So be careful while moving the position of reading.
                                    this.AttributeType = Utility.GetAttributeTypeFromAttributeTypeName(attributeTypeName, this.IsCollection);
                                    this.AttributeModelType = Utility.GetAttributeModelTypeFromAttributeTypeName(attributeTypeName);
                                }

                                if (reader.MoveToAttribute("IsLocalizable"))
                                {
                                    this.IsLocalizable = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("ApplyLocaleFormat"))
                                {
                                    this.ApplyLocaleFormat = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("ApplyTimeZoneConversion"))
                                {
                                    this.ApplyTimeZoneConversion = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("Precision"))
                                {
                                    this.Precision = reader.ReadContentAsInt();
                                }

                                if (reader.MoveToAttribute("IsPrecisionArbitrary"))
                                {
                                    this.IsPrecisionArbitrary = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("SourceFlag"))
                                {
                                    this.SourceFlag = Utility.GetSourceFlagEnum(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("SourceEnIdOverridden"))
                                {
                                    SourceEntityIdOverridden = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("SourceClassOverridden"))
                                {
                                    SourceClassOverridden = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("SourceEnIdInherited"))
                                {
                                    SourceEntityIdInherited = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("SourceClassInherited"))
                                {
                                    SourceClassInherited = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("AttrModelType"))
                                {
                                    String strAttributeModelType = reader.ReadContentAsString();
                                    AttributeModelType attributeModelType = AttributeModelType.All;

                                    if (!String.IsNullOrWhiteSpace(strAttributeModelType))
                                        Enum.TryParse<AttributeModelType>(strAttributeModelType, true, out attributeModelType);

                                    this.AttributeModelType = attributeModelType;
                                }

                                if (reader.MoveToAttribute("AttrType"))
                                {
                                    String strAttributeType = reader.ReadContentAsString();
                                    AttributeTypeEnum attributeType = AttributeTypeEnum.Unknown;

                                    if (!String.IsNullOrWhiteSpace(strAttributeType))
                                        Enum.TryParse(strAttributeType, true, out attributeType);

                                    this.AttributeType = attributeType;
                                }

                                if (reader.MoveToAttribute("HasInvalidOverriddenVals"))
                                {
                                    HasInvalidOverriddenValues = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("HasInvalidInheritedVals"))
                                {
                                    HasInvalidInheritedValues = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                //if (reader.MoveToAttribute("ValidationAdditionalVal"))
                                //{
                                //    ValidationAdditionalValue = reader.ReadContentAsString();
                                //}

                                if (reader.MoveToAttribute("HasInvalidVals"))
                                {
                                    HasInvalidValues = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (this.AttributeType == AttributeTypeEnum.Complex || this.AttributeType == AttributeTypeEnum.ComplexCollection)
                                    this.IsComplex = true;
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Values")
                        {
                            #region Read values for current attribute

                            //Read the Value xml first before reading SourceFlag because "reader.MoveToAttribute" will move the curser forward and we won't get full xml after that.

                            if (reader.HasAttributes && reader.GetAttribute("SourceFlag") != null)
                            {
                                AttributeValueSource srcFlag = Utility.GetSourceFlagEnum(reader.GetAttribute("SourceFlag"));

                                //Get ValueCollection from Xml
                                String valueXml = reader.ReadOuterXml();
                                if (!String.IsNullOrWhiteSpace(valueXml))
                                {
                                    ValueCollection valCollection = new ValueCollection(valueXml, ObjectSerialization.DataStorage);

                                    if (valCollection != null)
                                    {
                                        //Based on SourceFlag in Values node, populate ValueCollection in either inherited or overridden value collection
                                        switch (srcFlag)
                                        {
                                            case AttributeValueSource.Overridden:
                                                #region Populate Overridden values in object

                                                ObjectAction attributeAction = this.Action;

                                                foreach (Value val in valCollection)
                                                {
                                                    ObjectAction valueAction = val.Action;

                                                    this.AppendValue(val);

                                                    val.Action = valueAction;
                                                }

                                                this.Action = attributeAction;

                                                #endregion Populate Overridden values in object
                                                break;
                                            case AttributeValueSource.Inherited:
                                                #region Populate Inherited values in object

                                                attributeAction = this.Action;

                                                foreach (Value val in valCollection)
                                                {
                                                    ObjectAction valueAction = val.Action;

                                                    this.AppendInheritedValue(val);

                                                    val.Action = valueAction;
                                                }

                                                this.Action = attributeAction;

                                                #endregion Populate Inherited values in object
                                                break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //There are no attributes for 'values' element or Source Flag is not available..
                                //In this case, the Input Xml format will always be in 'External' serialization and having only overridden values
                                //Get ValueCollection from Xml
                                String valueXml = reader.ReadOuterXml();
                                if (!String.IsNullOrWhiteSpace(valueXml))
                                {
                                    ValueCollection valCollection = new ValueCollection(valueXml, ObjectSerialization.DataStorage);

                                    if (valCollection != null)
                                    {

                                        ObjectAction attributeAction = this.Action;

                                        #region Populate Overridden values in object

                                        foreach (Value val in valCollection)
                                        {
                                            ObjectAction valueAction = val.Action;

                                            this.AppendValue(val);

                                            val.Action = valueAction;
                                        }

                                        this.Action = attributeAction;

                                        #endregion Populate Overridden values in object
                                    }
                                }
                            }

                            #endregion Read values for current attribute
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Attributes")
                        {
                            #region Read child attributes
                            //Child attributes are collection under current attribute.

                            String attributeXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(attributeXml))
                            {
                                //Get collection of child attributes and populate it in Attribute collection of current attribute object.
                                AttributeCollection attributeCollection = new AttributeCollection(attributeXml, false, ObjectSerialization.DataStorage);
                                if (attributeCollection != null)
                                {
                                    foreach (Attribute attr in attributeCollection)
                                    {
                                        attr.Locale = this.Locale;
                                        this.Attributes.Add(attr);
                                    }
                                }
                            }

                            #endregion Read child attributes
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
        /// Loads Attribute object from xml
        /// </summary>
        /// <param name="reader">Indicates an xml reader used to read xml format data</param>
        internal void LoadAttributeFromXml(XmlTextReader reader)
        {
            if (reader != null)
            {
                if (reader.HasAttributes)
                {
                    LoadAttributeMetadataFromXml(reader);
                }

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement && reader.Name == "Values")
                    {
                        if (reader.HasAttributes && reader.GetAttribute("SourceFlag") != null)
                        {
                            AttributeValueSource srcFlag = Utility.GetSourceFlagEnum(reader.GetAttribute("SourceFlag"));

                            ValueCollection values = new ValueCollection();
                            values.LoadValueCollectionFromXml(reader);

                            switch (srcFlag)
                            {
                                case AttributeValueSource.Overridden:
                                    PopulateOverriddenAttributeValues(values);
                                    break;
                                case AttributeValueSource.Inherited:
                                    PopulateInheritedAttributeValues(values);
                                    break;
                            }
                        }
                        else
                        {
                            ValueCollection values = new ValueCollection();
                            values.LoadValueCollectionFromXml(reader);

                            PopulateOverriddenAttributeValues(values);
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Attributes") // child attributes
                    {
                        this._attributes.LoadAttributeCollectionFromXml(reader);
                    }
                    else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Attribute") // </Attribute>
                    {
                        return;
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("reader", "XmlTextReader is null. Can not read Attribute object.");
            }
        }


        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                if (obj is Attribute)
                {
                    Attribute objectToBeCompared = obj as Attribute;

                    if (this.AttributeParentId != objectToBeCompared.AttributeParentId)
                        return false;

                    if (this.AttributeParentName != objectToBeCompared.AttributeParentName)
                        return false;

                    if (this.AttributeParentLongName != objectToBeCompared.AttributeParentLongName)
                        return false;

                    if (this.IsCollection != objectToBeCompared.IsCollection)
                        return false;

                    if (this.IsComplex != objectToBeCompared.IsComplex)
                        return false;

                    if (this.IsLookup != objectToBeCompared.IsLookup)
                        return false;

                    if (this.IsLocalizable != objectToBeCompared.IsLocalizable)
                        return false;

                    if (this.Required != objectToBeCompared.Required)
                        return false;

                    if (this.ReadOnly != objectToBeCompared.ReadOnly)
                        return false;

                    if (this.ApplyLocaleFormat != objectToBeCompared.ApplyLocaleFormat)
                        return false;

                    if (this.ApplyTimeZoneConversion != objectToBeCompared.ApplyTimeZoneConversion)
                        return false;

                    if (this.AttributeType != objectToBeCompared.AttributeType)
                        return false;

                    if (this.AttributeDataType != objectToBeCompared.AttributeDataType)
                        return false;

                    if (this.SourceFlag != objectToBeCompared.SourceFlag)
                        return false;

                    if (this.Precision != objectToBeCompared.Precision)
                        return false;

                    if (this.IsPrecisionArbitrary != objectToBeCompared.IsPrecisionArbitrary)
                        return false;

                    if (!this.IsCollection)
                    {
                        if (this.OverriddenValue != null && objectToBeCompared.OverriddenValue != null)
                        {
                            if (!this.OverriddenValue.Equals(objectToBeCompared.OverriddenValue))
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (!(this.OverriddenValue == null && objectToBeCompared.OverriddenValue == null))
                                return false;
                        }
                    }

                    if (this.SourceFlag == AttributeValueSource.Inherited)
                    {
                        Int32 inheritedValuesUnion = this.InheritedValues.ToList().Union(objectToBeCompared.InheritedValues.ToList()).Count();
                        Int32 inheritedValuesIntersect = this.InheritedValues.ToList().Intersect(objectToBeCompared.InheritedValues.ToList()).Count();
                        if (inheritedValuesUnion != inheritedValuesIntersect)
                            return false;

                    }
                    else
                    {
                        Int32 overriddenValuesUnion = this.OverriddenValues.ToList().Union(objectToBeCompared.OverriddenValues.ToList()).Count();
                        Int32 overriddenValuesIntersect = this.OverriddenValues.ToList().Intersect(objectToBeCompared.OverriddenValues.ToList()).Count();
                        if (overriddenValuesUnion != overriddenValuesIntersect)
                            return false;
                    }

                    if (this.HasChanged != objectToBeCompared.HasChanged)
                        return false;
                    
                    if (this.IsHierarchical)
                    {
                        // As it is flush and fill implementation, sequence values are all present and are different.
                        if (this.Sequence != objectToBeCompared.Sequence)
                        {
                            return false;
                        }
                    }

                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            int hashCode = base.GetHashCode() ^ this.AttributeParentId.GetHashCode() ^ this.IsCollection.GetHashCode() ^ this.IsComplex.GetHashCode() ^ this.IsLookup.GetHashCode() ^ this.IsLocalizable.GetHashCode() ^ this.ReadOnly.GetHashCode() ^ this.Required.GetHashCode() ^ this.ApplyLocaleFormat.GetHashCode() ^ this.ApplyTimeZoneConversion.GetHashCode() ^ this.AttributeType.GetHashCode() ^ this.SourceFlag.GetHashCode() ^ this.Precision.GetHashCode() ^ this.HasChanged.GetHashCode() ^ this.IsPrecisionArbitrary.GetHashCode();
            if (this.AttributeParentName != null)
            {
                hashCode = hashCode ^ this.AttributeParentName.GetHashCode();
            }
            if (this.AttributeParentLongName != null)
            {
                hashCode = hashCode ^ this.AttributeParentLongName.GetHashCode();
            }

            hashCode = hashCode ^ this.AttributeDataType.GetHashCode();

            if (this.OverriddenValues != null)
            {
                hashCode = hashCode ^ this.OverriddenValues.GetHashCode();
            }
            if (this.InheritedValues != null)
            {
                hashCode = hashCode ^ this.InheritedValues.GetHashCode();
            }

            return hashCode;
        }

        /// <summary>
        /// Creates a clone copy of current attribute object
        /// </summary>
        /// <returns>Returns a clone copy of current attribute object</returns>
        public Attribute Clone()
        {
            Attribute clonedAttribute = this.CloneBasicProperties();

            ValueCollection existingInheritedValues = (ValueCollection)this.GetInheritedValuesInvariant();
            ValueCollection existingOverriddenValues = (ValueCollection)this.GetOverriddenValuesInvariant();

            //Create new value collection by cloning all the orig values
            ValueCollection clonedInheritedValues = new ValueCollection();

            if (existingInheritedValues != null && existingInheritedValues.Count > 0)
            {
                foreach (Value val in existingInheritedValues)
                {
                    clonedInheritedValues.Add(val.Clone());
                }
            }

            //Create new value collection by cloning all the orig values
            ValueCollection clonedOverriddenValues = new ValueCollection();

            if (existingOverriddenValues != null && existingOverriddenValues.Count > 0)
            {
                foreach (Value val in existingOverriddenValues)
                {
                    clonedOverriddenValues.Add(val.Clone());
                }
            }

            clonedAttribute._overriddenValues = clonedOverriddenValues;
            clonedAttribute._inheritedValues = clonedInheritedValues;

            if (this._attributes != null && this._attributes.Count > 0)
            {
                AttributeCollection clonedChildAttributes = new AttributeCollection();

                foreach (Attribute childAttribute in this._attributes)
                {
                    Attribute clonedChildAttr = childAttribute.Clone(); // Recurse method
                    clonedChildAttributes.Add(clonedChildAttr);
                }

                clonedAttribute._attributes = clonedChildAttributes;
            }

            return clonedAttribute;
        }

        /// <summary>
        /// Merge set of allowable UOMs
        /// </summary>
        /// <param name="allowableUOM">Indicates a string representing allowing UOMs</param>
        public void MergeSetDefaults(String allowableUOM)
        {

            MergeValueSetDefault(this, allowableUOM);
            if (this._attributes != null && this._attributes.Count > 0)
            {
                foreach (Attribute childAttribute in this._attributes)
                {
                    childAttribute.MergeSetDefaults(allowableUOM); // Recurse method
                    MergeValueSetDefault(childAttribute, allowableUOM);
                }
            }

            return;
        }

        /// <summary>
        /// Merge set of default values to the attribute object
        /// </summary>
        /// <param name="attr">Indicates the attribute instance</param>
        /// <param name="allowableUOM">Indicates string specifying allowable UOMs</param>
        public void MergeValueSetDefault(Attribute attr, String allowableUOM)
        {
            if (attr.HasValue() == true)
            {
                return;
            }

            if (attr.AttributeDataType.Equals(AttributeDataType.Boolean))
            {
                attr.SetValue(false);  // ????
            }
            else if (attr.AttributeDataType.Equals(AttributeDataType.Date))
            {
                attr.SetValue(DateTime.Now);
            }
            else if (attr.AttributeDataType.Equals(AttributeDataType.DateTime))
            {
                attr.SetValue(DateTime.Now);
            }
            else if (attr.AttributeDataType.Equals(AttributeDataType.Decimal))
            {
                if (attr.IsPrecisionArbitrary == true)
                {
                    attr.SetValue(0);
                }
                else
                {
                    attr.SetValue(0.0);
                }
            }
            else if (attr.AttributeDataType.Equals(AttributeDataType.Fraction))
            {
                attr.SetValue(0.0);
            }
            else if (attr.AttributeDataType.Equals(AttributeDataType.String))
            {
                attr.SetValue(" ");
            }
            else if (attr.AttributeDataType.Equals(AttributeDataType.Integer))
            {
                attr.SetValue(0);
            }
            else if (attr.AttributeDataType.Equals(AttributeDataType.ImageURL))
            {
                attr.SetValue(" ");
            }
            else if (attr.AttributeDataType.Equals(AttributeDataType.URL))
            {
                attr.SetValue(" ");
            }
            else
            {
                attr.SetValue("");
            }
        }

        /// <summary>
        /// Used by MergeCopy Engine. Shallow clone of one level only. The MergeCopy TreeWalker does a tail recursion externally
        /// </summary>
        /// <param name="entityId"> the entity that this attribute will belong to</param>
        /// <param name="defaultUOM"> UOM value which has to be set by default in new cloned object </param>
        /// <returns></returns>
        public Attribute MergeClone(Int64 entityId, String defaultUOM)
        {
            Attribute clonedAttribute = this.MergeCloneBasicProperties(entityId);

            ValueCollection existingValues = (ValueCollection)this.GetCurrentValuesInvariant();

            //Create new value collection by cloning all the orig values
            ValueCollection clonedValues = new ValueCollection();

            if (existingValues != null && existingValues.Count > 0)
            {
                foreach (Value val in existingValues)
                {
                    Value newValue = val.MergeCloneValue(defaultUOM);
                    newValue.Id = -1;
                    newValue.Action = ObjectAction.Update;
                    clonedValues.Add(newValue);
                }
            }

            if (clonedAttribute._sourceFlag == AttributeValueSource.Overridden)
                clonedAttribute._overriddenValues = clonedValues;
            else
                clonedAttribute._inheritedValues = clonedValues;

            if (this._attributes != null && this._attributes.Count > 0)
            {
                AttributeCollection clonedChildAttributes = new AttributeCollection();

                foreach (Attribute childAttribute in this._attributes)
                {
                    Attribute clonedChildAttr = childAttribute.MergeClone(entityId, defaultUOM); // Recurse method
                    clonedChildAttributes.Add(clonedChildAttr);
                }

                clonedAttribute._attributes = clonedChildAttributes;
            }

            return clonedAttribute;
        }

        /// <summary>
        /// Clones basic properties from current attribute instance and returns new attribute object instance
        /// </summary>
        /// <returns>New attribute instance object</returns>
        public Attribute CloneBasicProperties()
        {
            Attribute attribute = new Attribute();
            attribute.Id = this.Id;
            attribute.Name = this.Name;
            attribute.LongName = this.LongName;
            attribute.ApplyLocaleFormat = this.ApplyLocaleFormat;
            attribute.ApplyTimeZoneConversion = this.ApplyTimeZoneConversion;
            attribute.AttributeType = this.AttributeType;
            attribute.AttributeDataType = this.AttributeDataType;
            attribute.AttributeModelType = this.AttributeModelType;
            attribute.AttributeParentId = this.AttributeParentId;
            attribute.AttributeParentLongName = this.AttributeParentLongName;
            attribute.AttributeParentName = this.AttributeParentName;
            attribute.InstanceRefId = this.InstanceRefId;
            attribute.IsCollection = this.IsCollection;
            attribute.IsComplex = this.IsComplex;
            attribute.IsLookup = this.IsLookup;
            attribute.IsLocalizable = this.IsLocalizable;
            attribute.ReadOnly = this.ReadOnly;
            attribute.Required = this.Required;
            attribute.Locale = this.Locale;
            attribute.Precision = this.Precision;
            attribute.IsPrecisionArbitrary = this.IsPrecisionArbitrary;
            attribute.Sequence = this.Sequence;
            attribute.SourceFlag = this.SourceFlag;
            attribute.SourceEntityIdOverridden = this.SourceEntityIdOverridden;
            attribute.SourceClassOverridden = this.SourceClassOverridden;
            attribute.SourceEntityIdInherited = this.SourceEntityIdInherited;
            attribute.SourceClassInherited = SourceClassInherited;
            attribute.EntityId = this.EntityId;
            attribute.AuditRefId = this.AuditRefId;
            attribute.UserName = this.UserName;
            attribute.ProgramName = this.ProgramName;
            attribute.HasInvalidOverriddenValues = this.HasInvalidOverriddenValues;
            attribute.HasInvalidInheritedValues = this.HasInvalidInheritedValues;
            attribute.Action = this.Action;
            return attribute;
        }

        /// <summary>
        /// MergeClones basic properties from current attribute instance and returns new attribute object instance
        /// </summary>
        /// <returns>New attribute instance object</returns>
        public Attribute MergeCloneBasicProperties(Int64 entityId)
        {
            Attribute attribute = new Attribute();
            attribute.Id = this.Id;
            attribute.Name = this.Name;
            attribute.LongName = this.LongName;
            attribute.ApplyLocaleFormat = this.ApplyLocaleFormat;
            attribute.ApplyTimeZoneConversion = this.ApplyTimeZoneConversion;
            attribute.AttributeDataType = this.AttributeDataType;
            attribute.AttributeModelType = this.AttributeModelType;
            attribute.AttributeParentId = this.AttributeParentId;
            attribute.AttributeParentLongName = this.AttributeParentLongName;
            attribute.AttributeParentName = this.AttributeParentName;
            attribute.EntityId = entityId;
            attribute.InstanceRefId = this.InstanceRefId;
            attribute.IsCollection = this.IsCollection;
            attribute.IsComplex = this.IsComplex;
            attribute.IsLookup = this.IsLookup;
            attribute.IsLocalizable = this.IsLocalizable;
            attribute.ReadOnly = this.ReadOnly;
            attribute.Required = this.Required;
            attribute.Locale = this.Locale;
            attribute.Precision = this.Precision;
            attribute.IsPrecisionArbitrary = this.IsPrecisionArbitrary;
            attribute.Sequence = this.Sequence;
            //attribute.SourceFlag = this.SourceFlag;
            //attribute.SourceEntityIdOverridden = this.SourceEntityIdOverridden;
            //attribute.SourceClassOverridden = this.SourceClassOverridden;
            //attribute.SourceEntityIdInherited = this.SourceEntityIdInherited;
            //attribute.SourceClassInherited = SourceClassInherited;
            attribute.AuditRefId = this.AuditRefId;
            attribute.UserName = this.UserName;
            attribute.ProgramName = this.ProgramName;

            return attribute;
        }

        /// <summary>
        /// Sets the internal ProgramName property 
        /// </summary>
        /// <param name="modProgram"></param>
        public void SetMergeCopyModProgram(String modProgram)
        {
            this.ProgramName = modProgram;
        }

        /// <summary>
        /// Compare attribute with current attribute.
        /// This method will compare attribute and Values
        /// </summary>
        /// <param name="subSetAttribute">Attribute to be compared with current attribute object</param>
        /// <returns>True : Is both are same. False : otherwise</returns>
        public Boolean IsSuperSetOf(Attribute subSetAttribute)
        {
            //if(this.AttributeParentId != subSetAttribute.AttributeParentId)
            //    return false;

            if (this.AttributeParentName != subSetAttribute.AttributeParentName)
                return false;

            if (this.AttributeParentLongName != subSetAttribute.AttributeParentLongName)
                return false;

            if (this.IsCollection != subSetAttribute.IsCollection)
                return false;

            if (this.IsComplex != subSetAttribute.IsComplex)
                return false;

            if (this.IsLookup != subSetAttribute.IsLookup)
                return false;

            if (this.IsLocalizable != subSetAttribute.IsLocalizable)
                return false;

            if (this.ReadOnly != subSetAttribute.ReadOnly)
                return false;

            if (this.Required != subSetAttribute.Required)
                return false;

            if (this.ApplyLocaleFormat != subSetAttribute.ApplyLocaleFormat)
                return false;

            if (this.ApplyTimeZoneConversion != subSetAttribute.ApplyTimeZoneConversion)
                return false;

            if (this.AttributeType != subSetAttribute.AttributeType)
                return false;

            if (this.AttributeDataType != subSetAttribute.AttributeDataType)
                return false;

            if (this.SourceFlag != subSetAttribute.SourceFlag)
                return false;

            if (this.Precision != subSetAttribute.Precision)
                return false;

            if (this.IsPrecisionArbitrary != subSetAttribute.IsPrecisionArbitrary)
                return false;

            // Check if count of inherited and overridden value objects is same in both attributes
            if (this.InheritedValues.Count != subSetAttribute.InheritedValues.Count)
                return false;

            if (this.OverriddenValues.Count != subSetAttribute.OverriddenValues.Count)
                return false;

            // Check if inherited and overridden value objects are also same
            if (!CompareValues(this.OverriddenValues, subSetAttribute.OverriddenValues))
                return false;

            if (!CompareValues(this.InheritedValues, subSetAttribute.InheritedValues))
                return false;

            //if(this.HasChanged != subSetAttribute.HasChanged)
            //    return false;

            return true;
        }

        /// <summary>
        /// compare 2 sets of attributes and add the mismatch(es) as error if any in attributeOperationResults
        /// </summary>
        /// <param name="subSetAttribute"></param>
        /// <param name="attributeOperationResult"></param>
        /// <returns></returns>
        public Boolean GetSuperSetOperationResult(Attribute subSetAttribute, AttributeOperationResult attributeOperationResult)
        {
            #region compare properties

            String _errorMessage = "{0}:  Mismatch: Actual: [{1}] Expected: [{2}]";

            Utility.BusinessObjectPropertyCompare("AttributeParentName", this.AttributeParentName, subSetAttribute.AttributeParentName, attributeOperationResult);

            Utility.BusinessObjectPropertyCompare("AttributeParentLongName", this.AttributeParentLongName, subSetAttribute.AttributeParentLongName, attributeOperationResult);

            Utility.BusinessObjectPropertyCompare("IsCollection", this.IsCollection.ToString(), subSetAttribute.IsCollection.ToString(), attributeOperationResult);

            Utility.BusinessObjectPropertyCompare("IsComplex", this.IsComplex.ToString(), subSetAttribute.IsComplex.ToString(), attributeOperationResult);

            Utility.BusinessObjectPropertyCompare("IsLookup", this.IsLookup.ToString(), subSetAttribute.IsLookup.ToString(), attributeOperationResult);

            Utility.BusinessObjectPropertyCompare("IsLocalizable", this.IsLocalizable.ToString(), subSetAttribute.IsLocalizable.ToString(), attributeOperationResult);

            Utility.BusinessObjectPropertyCompare("ReadOnly", this.ReadOnly.ToString(), subSetAttribute.ReadOnly.ToString(), attributeOperationResult);

            Utility.BusinessObjectPropertyCompare("Required", this.Required.ToString(), subSetAttribute.Required.ToString(), attributeOperationResult);

            Utility.BusinessObjectPropertyCompare("ApplyLocaleFormat", this.ApplyLocaleFormat.ToString(), subSetAttribute.ApplyLocaleFormat.ToString(), attributeOperationResult);

            Utility.BusinessObjectPropertyCompare("ApplyTimeZoneConversion", this.ApplyTimeZoneConversion.ToString(), subSetAttribute.ApplyTimeZoneConversion.ToString(), attributeOperationResult);

            Utility.BusinessObjectPropertyCompare("AttributeType", (Int32)this.AttributeType, (Int32)subSetAttribute.AttributeType, attributeOperationResult);

            Utility.BusinessObjectPropertyCompare("AttributeDataType", (Int32)this.AttributeDataType, (Int32)subSetAttribute.AttributeDataType, attributeOperationResult);

            Utility.BusinessObjectPropertyCompare("SourceFlag", (Int32)this.SourceFlag, (Int32)subSetAttribute.SourceFlag, attributeOperationResult);

            Utility.BusinessObjectPropertyCompare("Precision", (Int32)this.Precision, (Int32)subSetAttribute.Precision, attributeOperationResult);

            Utility.BusinessObjectPropertyCompare("IsPrecisionArbitrary", this.IsPrecisionArbitrary.ToString(), subSetAttribute.IsPrecisionArbitrary.ToString(), attributeOperationResult);

            Utility.BusinessObjectPropertyCompare("InheritedValues.Count", this.InheritedValues.Count, subSetAttribute.InheritedValues.Count, attributeOperationResult);

            Utility.BusinessObjectPropertyCompare("OverriddenValues.Count", this.OverriddenValues.Count, subSetAttribute.OverriddenValues.Count, attributeOperationResult);

            #endregion compare properties

            // Check if inherited and overridden value objects are also identical
            if (!CompareValues(this.OverriddenValues, subSetAttribute.OverriddenValues))
            {
                attributeOperationResult.AddOperationResult("-1", String.Format(_errorMessage, "OverriddenValues:", this.OverriddenValues, subSetAttribute.OverriddenValues), OperationResultType.Error);
            }

            if (!CompareValues(this.InheritedValues, subSetAttribute.InheritedValues))
            {
                attributeOperationResult.AddOperationResult("-1", String.Format(_errorMessage, "InheritedValues:", this.InheritedValues, subSetAttribute.InheritedValues), OperationResultType.Error);
            }

            attributeOperationResult.RefreshOperationResultStatus();

            if (attributeOperationResult.OperationResultStatus != OperationResultStatusEnum.Successful)
            {
                return false;
            }

            return true;
        }

        #endregion

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of Attribute object
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public override String ToXml()
        {
            return this.ToXml(this.Locale, true);
        }

        /// <summary>
        /// Get Xml representation of Attribute object
        /// </summary>
        /// <param name="valueFormatLocale">Locale value based on which values will be returned</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml(LocaleEnum valueFormatLocale)
        {
            return this.ToXml(valueFormatLocale, true);
        }

        /// <summary>
        /// Get Xml representation of Attribute object
        /// </summary>
        /// <returns></returns>
        public String ToXmlInvariant()
        {
            return this.ToXml(this.Locale, false);
        }

        private String ToXml(LocaleEnum valueFormatLocale, Boolean applyFormat)
        {
            String attributeXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);
            //xmlWriter.WriteStartDocument();

            //Attribute node start
            xmlWriter.WriteStartElement("Attribute");

            ConvertAttributeMetadataToXml(xmlWriter);

            #region write overridden attribute values

            xmlWriter.WriteStartElement("Values");
            xmlWriter.WriteAttributeString("SourceFlag", Utility.GetSourceFlagString(Core.AttributeValueSource.Overridden));

            foreach (Value value in this.GetValue(AttributeValueSource.Overridden, valueFormatLocale, applyFormat))
            {
                xmlWriter.WriteRaw(value.ToXml());
            }

            //Overridden Values node end
            xmlWriter.WriteEndElement();

            #endregion write overridden attribute values

            #region write Inherited attribute values

            xmlWriter.WriteStartElement("Values");
            xmlWriter.WriteAttributeString("SourceFlag", Utility.GetSourceFlagString(Core.AttributeValueSource.Inherited));

            foreach (Value value in this.GetValue(AttributeValueSource.Inherited, valueFormatLocale, applyFormat))
            {
                xmlWriter.WriteRaw(value.ToXml());
            }

            //Inherited Values node end
            xmlWriter.WriteEndElement();

            #endregion write Inherited attribute values

            #region Write child attributes xml

            if (this.Attributes != null && this.Attributes.Count > 0)
            {
                xmlWriter.WriteStartElement("Attributes");

                foreach (Attribute childAttr in this.Attributes)
                {
                    xmlWriter.WriteRaw(childAttr.ToXml(valueFormatLocale, applyFormat));
                }

                //Child Attribute node end
                xmlWriter.WriteEndElement();
            }

            #endregion Get ToXml() for child attributes

            //Attribute node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            attributeXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return attributeXml;
        }

        /// <summary>
        /// Get Xml representation of Attribute object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public override String ToXml(ObjectSerialization serialization)
        {
            String attributeXml = String.Empty;

            if (serialization == ObjectSerialization.Full)
            {
                attributeXml = this.ToXml();
            }
            else
            {
                if (String.Compare(this.AttributeParentName, "EntityState", true) != 0)
                {
                    attributeXml = this.ToXml(serialization, this.Locale, true);
                }
            }

            return attributeXml;
        }

        /// <summary>
        /// Get Xml representation of Attribute object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <param name="valueFormatLocale"></param>
        /// <returns></returns>
        public String ToXml(ObjectSerialization serialization, LocaleEnum valueFormatLocale)
        {
            String attributeXml = String.Empty;

            if (serialization == ObjectSerialization.Full)
            {
                attributeXml = this.ToXml(valueFormatLocale);
            }
            else
            {
                attributeXml = this.ToXml(serialization, valueFormatLocale, true);
            }

            return attributeXml;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serialization"></param>
        /// <returns></returns>
        public String ToXmlInvariant(ObjectSerialization serialization)
        {
            String attributeXml = String.Empty;

            if (serialization == ObjectSerialization.Full)
            {
                attributeXml = this.ToXmlInvariant();
            }
            else
            {
                attributeXml = this.ToXml(serialization, this.Locale, false);
            }

            return attributeXml;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serialization"></param>
        /// <param name="valueFormatLocale"></param>
        /// <param name="applyFormat"></param>
        /// <returns></returns>
        private String ToXml(ObjectSerialization serialization, LocaleEnum valueFormatLocale, Boolean applyFormat)
        {
            String attributeXml = String.Empty;

            if (serialization == ObjectSerialization.Full)
            {
                attributeXml = this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //Attribute node start
                xmlWriter.WriteStartElement("Attribute");

                if (serialization == ObjectSerialization.DataStorage)
                {
                    #region Write attribute properties for ProcessingOnly Xml

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("Name", this.Name);
                    xmlWriter.WriteAttributeString("LName", System.Net.WebUtility.HtmlEncode(this.LongName));
                    xmlWriter.WriteAttributeString("ARefd", this.AuditRefId.ToString());
                    xmlWriter.WriteAttributeString("InstRefId", this.InstanceRefId.ToString());
                    xmlWriter.WriteAttributeString("Seq", this.Sequence.ToString());
                    xmlWriter.WriteAttributeString("AttrParentId", this.AttributeParentId.ToString());
                    xmlWriter.WriteAttributeString("AttrParentName", this.AttributeParentName);
                    xmlWriter.WriteAttributeString("AttrParentLName", this.AttributeParentLongName);
                    xmlWriter.WriteAttributeString("IsCollection", this.IsCollection.ToString());
                    xmlWriter.WriteAttributeString("IsComplex", this.IsComplex.ToString());
                    xmlWriter.WriteAttributeString("IsLookup", this.IsLookup.ToString());
                    xmlWriter.WriteAttributeString("IsLocalizable", this.IsLocalizable.ToString());
                    xmlWriter.WriteAttributeString("ReadOnly", this.ReadOnly.ToString());
                    xmlWriter.WriteAttributeString("Req", this.Required.ToString());
                    xmlWriter.WriteAttributeString("ApplyLocaleFormat", this.ApplyLocaleFormat.ToString());
                    xmlWriter.WriteAttributeString("ApplyTimeZoneConversion", this.ApplyTimeZoneConversion.ToString());
                    xmlWriter.WriteAttributeString("Precision", this.Precision.ToString());
                    xmlWriter.WriteAttributeString("IsPrecisionArbitrary", this.IsPrecisionArbitrary.ToString().ToLowerInvariant());
                    xmlWriter.WriteAttributeString("AttrType", this.AttributeType.ToString());
                    xmlWriter.WriteAttributeString("AttrModelType", this.AttributeModelType.ToString());
                    xmlWriter.WriteAttributeString("AttrDataType", this.AttributeDataType.ToString());
                    xmlWriter.WriteAttributeString("SourceFlag", Utility.GetSourceFlagString(this.SourceFlag));
                    xmlWriter.WriteAttributeString("SourceEnId", this.SourceEntityId.ToString());
                    xmlWriter.WriteAttributeString("SourceClass", this.SourceClass.ToString());
                    xmlWriter.WriteAttributeString("SourceEnIdOverridden", this.SourceEntityIdOverridden.ToString());
                    xmlWriter.WriteAttributeString("SourceClassOverridden", this.SourceClassOverridden.ToString());
                    xmlWriter.WriteAttributeString("SourceEnIdInherited", this.SourceEntityIdInherited.ToString());
                    xmlWriter.WriteAttributeString("SourceClassInherited", this.SourceClassInherited.ToString());
                    xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
                    xmlWriter.WriteAttributeString("Action", this.Action.ToString());
                    xmlWriter.WriteAttributeString("HasInvalidOverriddenVals", this.HasInvalidOverriddenValues.ToString().ToLowerInvariant());
                    xmlWriter.WriteAttributeString("HasInvalidInheritedVals", this.HasInvalidInheritedValues.ToString().ToLowerInvariant());
                    //xmlWriter.WriteAttributeString("ValidationAdditionalVal", this.ValidationAdditionalValue);
                    xmlWriter.WriteAttributeString("HasInvalidVals", this.HasInvalidValues.ToString().ToLowerInvariant());

                    #endregion Write attribute properties for ProcessingOnly Xml
                }
                else if (serialization == ObjectSerialization.ProcessingOnly)
                {
                    #region Write attribute properties for ProcessingOnly Xml

                    // TODO : Need to decide which all properties are needed for processing Xml.
                    // currently returning all properties.
                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("Name", this.Name);
                    xmlWriter.WriteAttributeString("LongName", System.Net.WebUtility.HtmlEncode(this.LongName));
                    xmlWriter.WriteAttributeString("InstanceRefId", this.InstanceRefId.ToString());
                    xmlWriter.WriteAttributeString("Sequence", this.Sequence.ToString());
                    xmlWriter.WriteAttributeString("AttributeParentId", this.AttributeParentId.ToString());
                    //xmlWriter.WriteAttributeString("AttributeParentName", this.AttributeParentName);
                    //xmlWriter.WriteAttributeString("AttributeParentLongName", this.AttributeParentLongName);
                    xmlWriter.WriteAttributeString("IsCollection", this.IsCollection.ToString());
                    xmlWriter.WriteAttributeString("IsComplex", this.IsComplex.ToString());
                    xmlWriter.WriteAttributeString("IsLocalizable", this.IsLocalizable.ToString());
                    //xmlWriter.WriteAttributeString("ApplyLocaleFormat", this.ApplyLocaleFormat.ToString());
                    //xmlWriter.WriteAttributeString("ApplyTimeZoneConversion", this.ApplyTimeZoneConversion.ToString());
                    //xmlWriter.WriteAttributeString("Precision", this.Precision.ToString());
                    xmlWriter.WriteAttributeString("AttributeType", this.AttributeType.ToString());
                    xmlWriter.WriteAttributeString("AttributeModelType", this.AttributeModelType.ToString());
                    xmlWriter.WriteAttributeString("AttributeDataType", this.AttributeDataType.ToString());
                    xmlWriter.WriteAttributeString("SourceFlag", Utility.GetSourceFlagString(this.SourceFlag));
                    xmlWriter.WriteAttributeString("SourceEntityId", this.SourceEntityId.ToString());
                    xmlWriter.WriteAttributeString("SourceClass", this.SourceClass.ToString());
                    xmlWriter.WriteAttributeString("LocaleId", ((Int32)this.Locale).ToString());
                    xmlWriter.WriteAttributeString("Action", this.Action.ToString());

                    #endregion Write attribute properties for ProcessingOnly Xml
                }
                else if (serialization == ObjectSerialization.UIRender)
                {
                    #region Write attribute properties for Rendering Xml

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("Name", this.Name);
                    xmlWriter.WriteAttributeString("LongName", System.Net.WebUtility.HtmlEncode(this.LongName));
                    xmlWriter.WriteAttributeString("InstanceRefId", this.InstanceRefId.ToString());
                    xmlWriter.WriteAttributeString("Sequence", this.Sequence.ToString());
                    xmlWriter.WriteAttributeString("AttributeParentId", this.AttributeParentId.ToString());
                    xmlWriter.WriteAttributeString("AttributeParentName", this.AttributeParentName);
                    //xmlWriter.WriteAttributeString("AttributeParentLongName", this.AttributeParentLongName);
                    xmlWriter.WriteAttributeString("IsCollection", this.IsCollection.ToString());
                    //xmlWriter.WriteAttributeString("IsComplex", this.IsComplex.ToString());
                    xmlWriter.WriteAttributeString("IsLocalizable", this.IsLocalizable.ToString());
                    xmlWriter.WriteAttributeString("IsLookup", this.IsLookup.ToString());
                    //xmlWriter.WriteAttributeString("ApplyLocaleFormat", this.ApplyLocaleFormat.ToString());
                    //xmlWriter.WriteAttributeString("ApplyTimeZoneConversion", this.ApplyTimeZoneConversion.ToString());
                    //xmlWriter.WriteAttributeString("Precision", this.Precision.ToString());
                    //xmlWriter.WriteAttributeString("AttributeType", this.AttributeType.ToString());
                    //xmlWriter.WriteAttributeString("AttributeModelType", this.AttributeModelType.ToString());
                    xmlWriter.WriteAttributeString("AttributeDataType", this.AttributeDataType.ToString());
                    xmlWriter.WriteAttributeString("SourceFlag", Utility.GetSourceFlagString(this.SourceFlag));
                    xmlWriter.WriteAttributeString("SourceEntityId", this.SourceEntityId.ToString());
                    xmlWriter.WriteAttributeString("SourceClass", this.SourceClass.ToString());
                    xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
                    xmlWriter.WriteAttributeString("Action", this.Action.ToString());

                    #endregion Write attribute properties for Rendering Xml
                }
                else if (serialization == ObjectSerialization.External)
                {
                    #region Write attribute properties for External Xml

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("Name", this.Name);
                    xmlWriter.WriteAttributeString("LongName", System.Net.WebUtility.HtmlEncode(this.LongName));
                    xmlWriter.WriteAttributeString("InstanceRefId", this.InstanceRefId.ToString());
                    xmlWriter.WriteAttributeString("Sequence", this.Sequence.ToString());
                    xmlWriter.WriteAttributeString("AttributeParentId", this.AttributeParentId.ToString());
                    xmlWriter.WriteAttributeString("AttributeParentName", this.AttributeParentName);
                    xmlWriter.WriteAttributeString("AttributeType", this.AttributeType.ToString());
                    xmlWriter.WriteAttributeString("AttributeDataType", this.AttributeDataType.ToString());

                    //TODO :: AttributeCollection change : Verify locale is needed for External xml too.
                    xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());

                    xmlWriter.WriteAttributeString("Action", ValueTypeHelper.GetActionString(this.Action));

                    #endregion
                }
                else if (serialization == ObjectSerialization.DataTransfer)
                {
                    #region Write attribute properties for ProcessingOnly Xml

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("N", this.Name);
                    xmlWriter.WriteAttributeString("LN", System.Net.WebUtility.HtmlEncode(this.LongName));
                    xmlWriter.WriteAttributeString("DT", this.AttributeDataType.ToString());
                    xmlWriter.WriteAttributeString("IRId", this.InstanceRefId.ToString());
                    xmlWriter.WriteAttributeString("Seq", this.Sequence.ToString());
                    xmlWriter.WriteAttributeString("PId", this.AttributeParentId.ToString());
                    xmlWriter.WriteAttributeString("PN", this.AttributeParentName);
                    xmlWriter.WriteAttributeString("PLN", this.AttributeParentLongName);
                    xmlWriter.WriteAttributeString("Col", this.IsCollection.ToString());
                    xmlWriter.WriteAttributeString("Com", this.IsComplex.ToString());
                    xmlWriter.WriteAttributeString("LP", this.IsLookup.ToString());
                    xmlWriter.WriteAttributeString("Loc", this.IsLocalizable.ToString());
                    xmlWriter.WriteAttributeString("RO", this.ReadOnly.ToString());
                    xmlWriter.WriteAttributeString("R", this.Required.ToString());
                    xmlWriter.WriteAttributeString("ALF", this.ApplyLocaleFormat.ToString());
                    xmlWriter.WriteAttributeString("ATZC", this.ApplyTimeZoneConversion.ToString());
                    xmlWriter.WriteAttributeString("P", this.Precision.ToString());
                    xmlWriter.WriteAttributeString("SF", Utility.GetSourceFlagString(this.SourceFlag));
                    xmlWriter.WriteAttributeString("L", this.Locale.ToString());
                    xmlWriter.WriteAttributeString("A", this.Action.ToString());
                    xmlWriter.WriteAttributeString("HIOV", this.HasInvalidOverriddenValues.ToString().ToLowerInvariant());
                    xmlWriter.WriteAttributeString("HIIV", this.HasInvalidInheritedValues.ToString().ToLowerInvariant());
                    xmlWriter.WriteAttributeString("HIV", this.HasInvalidValues.ToString().ToLowerInvariant());
                    xmlWriter.WriteAttributeString("IPA",this.IsPrecisionArbitrary.ToString().ToLowerInvariant());

                    #endregion Write attribute properties for ProcessingOnly Xml
                }

                Boolean noOverriddenValue = false;

                #region write overridden attribute values

                if (!(serialization == ObjectSerialization.External
                    && (this._attributeType == AttributeTypeEnum.Complex
                            || this._attributeType == AttributeTypeEnum.ComplexCollection)
                      )
                    )
                {

                    IValueCollection values = null;
                    if (serialization == ObjectSerialization.ProcessingOnly)
                    {
                        values = this.GetValue(AttributeValueSource.Overridden, this.Locale, false);
                    }
                    else
                    {
                        values = this.GetValue(AttributeValueSource.Overridden, valueFormatLocale, applyFormat);
                    }

                    if (values != null && values.Count > 0)
                    {
                        xmlWriter.WriteStartElement("Values");

                        //For 'external' serialization, Values are always overridden.. So not writing SourceFlag
                        String sourceFlag = Utility.GetSourceFlagString(Core.AttributeValueSource.Overridden);
                        if (serialization == ObjectSerialization.DataTransfer)
                        {
                            xmlWriter.WriteAttributeString("SF", sourceFlag);
                        }
                        else if (serialization != ObjectSerialization.External)
                        {
                            xmlWriter.WriteAttributeString("SourceFlag", sourceFlag);
                        }

                        foreach (Value value in values)
                        {
                            xmlWriter.WriteRaw(value.ToXml(serialization, this.AttributeDataType));
                        }
                        //Overridden Values node end
                        xmlWriter.WriteEndElement();
                    }
                    else
                    {
                        noOverriddenValue = true;
                    }
                }

                #endregion write overridden attribute values

                #region write Inherited attribute values

                //For 'external' serialization, Values are always overridden.. So not writing inherited values
                //For processing only serialization, no need to send I values.
                if (!(serialization == ObjectSerialization.External || serialization == ObjectSerialization.ProcessingOnly) || noOverriddenValue == true)
                {
                    xmlWriter.WriteStartElement("Values");

                    String sourceFlag = Utility.GetSourceFlagString(Core.AttributeValueSource.Inherited);
                    if (serialization == ObjectSerialization.DataTransfer)
                    {
                        xmlWriter.WriteAttributeString("SF", sourceFlag);
                    }
                    else if (serialization != ObjectSerialization.External)
                    {
                        xmlWriter.WriteAttributeString("SourceFlag", sourceFlag);
                    }

                    if (this.InheritedValues != null)
                    {
                        foreach (Value value in this.GetValue(AttributeValueSource.Inherited, valueFormatLocale, applyFormat))
                        {
                            xmlWriter.WriteRaw(value.ToXml(serialization, this.AttributeDataType));
                        }
                    }

                    //Inherited Values node end
                    xmlWriter.WriteEndElement();
                }

                #endregion write Inherited attribute values

                #region Write child attributes xml

                if (this.IsComplex && this.Attributes != null)
                {
                    if (serialization == ObjectSerialization.UIRender)
                    {
                        xmlWriter.WriteRaw(this.Attributes.ToXml(serialization, valueFormatLocale));
                    }
                    else if (!(this.SourceFlag == AttributeValueSource.Inherited && serialization == ObjectSerialization.ProcessingOnly))
                    {
                        xmlWriter.WriteRaw(this.Attributes.ToXml(serialization));
                    }
                }

                #endregion Write child attributes xml

                //Attribute node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //get the actual XML
                attributeXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }

            return attributeXml;
        }

        /// <summary>
        /// Converts Attribute object into xml format.
        /// In future, this method can be enhanced to accept EntityConversionContext object
        /// </summary>
        /// <param name="xmlWriter">Indicates a writer to generate xml reperesentation of Attribute object</param>
        internal void ConvertAttributeToXml(XmlTextWriter xmlWriter)
        {
            if (xmlWriter != null)
            {
                //Attribute node start
                xmlWriter.WriteStartElement("Attribute");

                ConvertAttributeMetadataToXml(xmlWriter);

                ConvertOverriddenAttributeValuesToXml(xmlWriter);

                ConvertInheritedAttributeValuesToXml(xmlWriter);

                #region Write child attributes xml

                if (this._attributes != null)
                {
                    this._attributes.ConvertAttributeCollectionToXml(xmlWriter);
                }

                #endregion Write child attributes xml

                //Attribute node end
                xmlWriter.WriteEndElement();
            }
            else
            {
                throw new ArgumentNullException("xmlWriter", "XmlTextWriter is null. Can not write Attribute object.");
            }
        }

        #endregion ToXml methods

        #region Get Attribute Value

        #region Get Overridden Value

        /// <summary>
        /// Get overridden values for current attribute
        /// </summary>
        /// <returns>Collection of overridden values for current attribute object</returns>
        public IValueCollection GetOverriddenValues()
        {
            return this.GetValue(AttributeValueSource.Overridden, this.Locale, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formatLocale"></param>
        /// <returns></returns>
        public IValueCollection GetOverriddenValues(LocaleEnum formatLocale)
        {
            return this.GetValue(AttributeValueSource.Overridden, formatLocale, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IValueCollection GetOverriddenValuesInvariant()
        {
            return this.GetValue(AttributeValueSource.Overridden, this.Locale, false);
        }

        /// <summary>
        /// Get AttrVal of overridden value.
        /// </summary>
        /// <returns>AttrVal of current attribute</returns>
        /// <exception cref="NotSupportedException">Value property cannot be accessible when attribute is collection.</exception>
        public Object GetOverriddenValue()
        {
            Object val = null;

            if (this.IsCollection)
            {
                throw new NotSupportedException(String.Format("Value property cannot be accessible when attribute is collection. Attribute Name={0}, LongName={1} and ParentName={2}", this.Name, this.LongName, this.AttributeParentName));
            }
            else
            {
                IValueCollection values = this.GetValue(AttributeValueSource.Overridden, this.Locale, true);
                if (values != null && values.Count > 0)
                {
                    val = values.ElementAt(0).AttrVal;
                }
            }

            return val;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formatLocale"></param>
        /// <returns></returns>
        public Object GetOverriddenValue(LocaleEnum formatLocale)
        {
            Object val = null;

            if (this.IsCollection)
            {
                throw new NotSupportedException("Value property cannot be accessible when attribute is collection");
            }
            else
            {
                IValueCollection values = this.GetValue(AttributeValueSource.Overridden, formatLocale, true);
                if (values != null && values.Count > 0)
                {
                    val = values.ElementAt(0).AttrVal;
                }
            }

            return val;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Object GetOverriddenValueInvariant()
        {
            Object val = null;

            if (this.IsCollection)
            {
                throw new NotSupportedException("Value property cannot be accessible when attribute is collection");
            }
            else
            {
                IValueCollection values = this.GetValue(AttributeValueSource.Overridden, this.Locale, false);
                if (values != null && values.Count > 0)
                {
                    val = values.ElementAt(0).AttrVal;
                }
            }

            return val;
        }

        /// <summary>
        /// Get IValue  object for overridden value.
        /// </summary>
        /// <returns>IValue object for current attribute</returns>
        /// <exception cref="NotSupportedException">Value property cannot be accessible when attribute is collection.</exception>
        public IValue GetOverriddenValueInstance()
        {
            IValue val = null;

            if (this.IsCollection)
            {
                throw new NotSupportedException("Value property cannot be accessible when attribute is collection");
            }
            else
            {
                IValueCollection values = this.GetValue(AttributeValueSource.Overridden, this.Locale, true);
                if (values != null && values.Count > 0)
                {
                    val = values.ElementAt(0);
                }
            }

            return val;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formatLocale"></param>
        /// <returns></returns>
        public IValue GetOverriddenValueInstance(LocaleEnum formatLocale)
        {
            IValue val = null;

            if (this.IsCollection)
            {
                throw new NotSupportedException("Value property cannot be accessible when attribute is collection");
            }
            else
            {
                IValueCollection values = this.GetValue(AttributeValueSource.Overridden, formatLocale, true);
                if (values != null && values.Count > 0)
                {
                    val = values.ElementAt(0);
                }
            }

            return val;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IValue GetOverriddenValueInstanceInvariant()
        {
            IValue val = null;

            if (this.IsCollection)
            {
                throw new NotSupportedException("Value property cannot be accessible when attribute is collection");
            }
            else
            {
                IValueCollection values = this.GetValue(AttributeValueSource.Overridden, this.Locale, false);
                if (values != null && values.Count > 0)
                {
                    val = values.ElementAt(0);
                }
            }

            return val;
        }

        #endregion Get Overridden Value

        #region Get Inherited Value

        /// <summary>
        /// Get AttrVal of inherited value.
        /// </summary>
        /// <returns>AttrVal of current attribute</returns>
        /// <exception cref="NotSupportedException">Value property cannot be accessible when attribute is collection.</exception>
        public Object GetInheritedValue()
        {
            Object val = null;

            if (this.IsCollection)
            {
                throw new NotSupportedException("Value property cannot be accessible when attribute is collection");
            }
            else
            {
                IValueCollection values = this.GetValue(AttributeValueSource.Inherited, this.Locale, true);
                if (values != null && values.Count > 0)
                {
                    val = values.ElementAt(0).AttrVal;
                }
            }

            return val;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formatLocale"></param>
        /// <returns></returns>
        public Object GetInheritedValue(LocaleEnum formatLocale)
        {
            Object val = null;

            if (this.IsCollection)
            {
                throw new NotSupportedException("Value property cannot be accessible when attribute is collection");
            }
            else
            {
                IValueCollection values = this.GetValue(AttributeValueSource.Inherited, formatLocale, true);
                if (values != null && values.Count > 0)
                {
                    val = values.ElementAt(0).AttrVal;
                }
            }

            return val;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Object GetInheritedValueInvariant()
        {
            Object val = null;

            if (this.IsCollection)
            {
                throw new NotSupportedException("Value property cannot be accessible when attribute is collection");
            }
            else
            {
                IValueCollection values = this.GetValue(AttributeValueSource.Inherited, this.Locale, false);
                if (values != null && values.Count > 0)
                {
                    val = values.ElementAt(0).AttrVal;
                }
            }

            return val;
        }

        /// <summary>
        /// Get IValue object for of inherited value.
        /// </summary>
        /// <returns>IValue for current attribute value</returns>
        /// <exception cref="NotSupportedException">Value property cannot be accessible when attribute is collection.</exception>
        public IValue GetInheritedValueInstance()
        {
            IValue val = null;

            if (this.IsCollection)
            {
                throw new NotSupportedException("Value property cannot be accessible when attribute is collection");
            }
            else
            {
                IValueCollection values = this.GetValue(AttributeValueSource.Inherited, this.Locale, true);
                if (values != null && values.Count > 0)
                {
                    val = values.ElementAt(0);
                }
            }

            return val;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formatLocale"></param>
        /// <returns></returns>
        public IValue GetInheritedValueInstance(LocaleEnum formatLocale)
        {
            IValue val = null;

            if (this.IsCollection)
            {
                throw new NotSupportedException("Value property cannot be accessible when attribute is collection");
            }
            else
            {
                IValueCollection values = this.GetValue(AttributeValueSource.Inherited, formatLocale, true);
                if (values != null && values.Count > 0)
                {
                    val = values.ElementAt(0);
                }
            }

            return val;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IValue GetInheritedValueInstanceInvariant()
        {
            IValue val = null;

            if (this.IsCollection)
            {
                throw new NotSupportedException("Value property cannot be accessible when attribute is collection");
            }
            else
            {
                IValueCollection values = this.GetValue(AttributeValueSource.Inherited, this.Locale, false);
                if (values != null && values.Count > 0)
                {
                    val = values.ElementAt(0);
                }
            }

            return val;
        }

        /// <summary>
        /// Get inherited values for current attribute
        /// </summary>
        /// <returns>Collection of overridden values for current attribute object</returns>
        public IValueCollection GetInheritedValues()
        {
            return this.GetValue(AttributeValueSource.Inherited, this.Locale, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formatLocale"></param>
        /// <returns></returns>
        public IValueCollection GetInheritedValues(LocaleEnum formatLocale)
        {
            return this.GetValue(AttributeValueSource.Inherited, formatLocale, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IValueCollection GetInheritedValuesInvariant()
        {
            return this.GetValue(AttributeValueSource.Inherited, this.Locale, false);
        }

        #endregion Get Inherited Value

        #region Get Current Value

        /// <summary>
        /// Get current value based on Source
        /// If Source = Overridden then returns OverriddenValues
        /// If Source = Inherited then returns InheritedValues
        /// </summary>
        /// <returns>Collection of values based on source flag</returns>
        public IValueCollection GetCurrentValues()
        {
            IValueCollection curValues = null;

            if (this.SourceFlag == AttributeValueSource.Inherited)
            {
                curValues = this.GetInheritedValues();
            }
            else if (this.SourceFlag == AttributeValueSource.Overridden)
            {
                curValues = this.GetOverriddenValues();
            }

            return curValues;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formatLocale"></param>
        /// <returns></returns>
        public IValueCollection GetCurrentValues(LocaleEnum formatLocale)
        {
            IValueCollection curValues = null;

            if (this.SourceFlag == AttributeValueSource.Inherited)
            {
                curValues = this.GetInheritedValues(formatLocale);
            }
            else if (this.SourceFlag == AttributeValueSource.Overridden)
            {
                curValues = this.GetOverriddenValues(formatLocale);
            }

            return curValues;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IValueCollection GetCurrentValuesInvariant()
        {
            IValueCollection curValues = null;

            if (this.SourceFlag == AttributeValueSource.Inherited)
            {
                curValues = this.GetInheritedValuesInvariant();
            }
            else if (this.SourceFlag == AttributeValueSource.Overridden)
            {
                curValues = this.GetOverriddenValuesInvariant();
            }

            return curValues;
        }

        /// <summary>
        /// Returns the current (considering source flag) value of attribute as Object
        /// </summary>
        /// <returns>Current value (considering source flag)</returns>
        /// <exception cref="NotSupportedException">Value property cannot be accessible when attribute is collection.</exception>
        public Object GetCurrentValue()
        {
            Object val = null;

            if (this.IsCollection && !this.IsHierarchical)
            {
                throw new NotSupportedException(String.Format("Value property cannot be accessible '{0}' attribute is collection.", this.Name));
            }
            else
            {
                IValueCollection values = this.GetCurrentValues();
                if (values != null && values.Count > 0)
                {
                    val = values.ElementAt(0).AttrVal;
                }
            }

            return val;
        }

        /// <summary>
        ///  Get the current value for the attribute.
        /// </summary>
        /// <typeparam name="T">Indicates the data type of the attribute</typeparam>
        /// <returns>Returns the current value based on the requested data type if the data type is valid</returns>
        public T GetCurrentValue<T>()
        {
            this.CheckIfComplexOrCollectionAttribute();

            T value = default(T);
            value = GetAttributeValue<T>(this.GetCurrentValueInstance());
            return value;
        }

        /// <summary>
        ///  Get the current invariant value for the attribute.
        /// </summary>
        /// <typeparam name="T">Indicates the data type in which format want the result</typeparam>
        /// <returns>Returns the current value based on the requested data type if the data type is valid</returns>
        public T GetCurrentValueInvariant<T>()
        {
            this.CheckIfComplexOrCollectionAttribute();
            
            T value = default(T);
            value = GetAttributeValue<T>(this.GetCurrentValueInstanceInvariant());
            return value;
        }

        

        /// <summary>
        /// Get the attribute value as requested type from the input value object
        /// </summary>
        /// <typeparam name="T">Indicates the data type in which format want the result</typeparam>
        /// <param name="value">Indicates the value object</param>
        /// <returns>Returns the value as requested data type</returns>
        internal T GetAttributeValue<T>(IValue value)
        {
            T result = default(T);

            if (value != null)
            {
                Object attributeValue = null;

                try
                {
                    if (this.AttributeDataType == AttributeDataType.DateTime && typeof(T) == typeof(DateTime))
                    {
                        attributeValue = value.GetDateTimeValue();
                    }
                    else if (this.AttributeDataType == AttributeDataType.Decimal && typeof(T) == typeof(Decimal))
                    {
                        attributeValue = value.GetNumericValue();
                    }
                    else
                    {
                        attributeValue = value.GetStringValue();
                    }

                    if (attributeValue != null)
                    {
                        result = (T)Convert.ChangeType(attributeValue, typeof(T));
                    }
                }
                catch (InvalidCastException ex)
                {
                    String errorMessage = String.Format("Attribute {0} is of type {1}, but it is expected as {2}", this.Name, this.AttributeDataType, typeof(T).Name);
                    throw new InvalidCastException(errorMessage, ex.InnerException);
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formatLocale"></param>
        /// <returns></returns>
        public Object GetCurrentValue(LocaleEnum formatLocale)
        {
            Object val = null;

            if (this.IsCollection)
            {
                throw new NotSupportedException("Value property cannot be accessible when attribute is collection.");
            }
            else
            {
                IValueCollection values = this.GetCurrentValues(formatLocale);
                if (values != null && values.Count > 0)
                {
                    val = values.ElementAt(0).AttrVal;
                }
            }

            return val;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Object GetCurrentValueInvariant()
        {
            Object val = null;

            if (this.IsCollection)
            {
                throw new NotSupportedException("Value property cannot be accessible when attribute is collection.");
            }
            else
            {
                IValueCollection values = this.GetCurrentValuesInvariant();
                if (values != null && values.Count > 0)
                {
                    val = values.ElementAt(0).AttrVal;
                }
            }

            return val;
        }

        /// <summary>
        /// Returns the current (considering source flag) value object of attribute as Object
        /// </summary>
        /// <returns>Current value object (considering source flag)</returns>
        /// <exception cref="NotSupportedException">Value property cannot be accessible when attribute is collection.</exception>
        public IValue GetCurrentValueInstance()
        {
            IValue val = null;

            if (this.IsCollection)
            {
                throw new NotSupportedException("Value property cannot be accessible when attribute is collection.");
            }
            else
            {
                IValueCollection values = this.GetCurrentValues();
                if (values != null && values.Count > 0)
                {
                    val = values.ElementAt(0);
                }
            }

            return val;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formatLocale"></param>
        /// <returns></returns>
        public IValue GetCurrentValueInstance(LocaleEnum formatLocale)
        {
            IValue val = null;

            if (this.IsCollection)
            {
                throw new NotSupportedException("Value property cannot be accessible when attribute is collection.");
            }
            else
            {
                IValueCollection values = this.GetCurrentValues(formatLocale);
                if (values != null && values.Count > 0)
                {
                    val = values.ElementAt(0);
                }
            }

            return val;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IValue GetCurrentValueInstanceInvariant()
        {
            IValue val = null;

            if (this.IsCollection)
            {
                throw new NotSupportedException("Value property cannot be accessible when attribute is collection.");
            }
            else
            {
                IValueCollection values = this.GetCurrentValuesInvariant();
                if (values != null && values.Count > 0)
                {
                    val = values.ElementAt(0);
                }
            }

            return val;
        }

        #endregion Get Current Value

        #endregion Get Attribute Value

        #region Set Attribute Value

        #region Set/Append Inherited values

        /// <summary>
        /// Sets value as inherited
        /// If current attribute's <see cref="SourceFlag"/> is Inherited then only it will save the value. Otherwise throws exception
        /// </summary>
        /// <param name="value">Value to set</param>
        /// <exception cref="ArgumentNullException">Value cannot be null</exception>
        /// <exception cref="InvalidSourceFlagException">Cannot change value when SourceFlag is Overridden.</exception>
        public void SetInheritedValue(IValue value)
        {
            this.SetInheritedValue(value, this.Locale, true, false);
        }

        /// <summary>
        /// Sets values as inherited
        /// If current attribute's <see cref="SourceFlag"/> is Inherited then only it will save the value. Otherwise throws exception
        /// </summary>
        /// <param name="valueCollection">Values to set</param>
        /// <exception cref="ArgumentNullException">Value cannot be null</exception>
        /// <exception cref="InvalidSourceFlagException">Cannot change value when SourceFlag is Overridden.</exception>
        public void SetInheritedValue(IValueCollection valueCollection)
        {
            this.SetInheritedValue(valueCollection, this.Locale, true, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void SetInheritedValueInvariant(IValue value)
        {
            this.SetInheritedValue(value, this.Locale, false, false);
        }

        /// <summary>
        /// Appends new value into the already existing inherited values
        /// </summary>
        /// <param name="value">Value which needs to be added</param>
        /// <exception cref="ArgumentNullException">Thrown when Value parameter is null</exception>
        /// <exception cref="InvalidSourceFlagException">Cannot append value when SourceFlag is Overridden.</exception>
        public void AppendInheritedValue(IValue value)
        {
            this.SetInheritedValue(value, this.Locale, true, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void AppendInheritedValueInvariant(IValue value)
        {
            this.SetInheritedValue(value, this.Locale, false, true);
        }

        #endregion Set/Append Inherited values

        #region Set/Append Overridden values

        /// <summary>
        /// Overrides already existing value with new value.
        /// If current attribute's <see cref="SourceFlag"/> is Overridden then only it will save the value. Otherwise throws exception
        /// </summary>
        /// <param name="value">New value to set in overridden attribute collection.</param>
        /// <exception cref="ArgumentNullException">Value cannot be null</exception>
        /// <exception cref="InvalidSourceFlagException">Cannot change value when SourceFlag is Inherited.</exception>
        public void SetValue(IValue value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("Value");
            }

            this.SetValue(value, this.Locale, true, false);
        }

        /// <summary>
        /// Set overridden attribute value.
        /// </summary>
        /// <param name="attrVal">Value to set for current attribute</param>
        /// <exception cref="InvalidSourceFlagException">Cannot change value when SourceFlag is Inherited</exception>
        /// <exception cref="NotSupportedException">Value property cannot be accessible when attribute is collection</exception>
        public void SetValue(Object attrVal)
        {
            Value value = new Value(attrVal);
            value.Action = ObjectAction.Update;

            this.SetValue(value, this.Locale, true, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        /// <param name="formatLocale"></param>
        public void SetValue(IValue val, LocaleEnum formatLocale)
        {
            this.SetValue(val, formatLocale, true, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        public void SetValueInvariant(IValue val)
        {
            this.SetValue(val, this.Locale, false, false);
        }

        /// <summary>
        /// Overrides already existing value with new values.
        /// If current attribute's <see cref="SourceFlag"/> is Overridden then only it will save the value. Otherwise throws exception
        /// </summary>
        /// <param name="valueCollection">New value to set in overridden attribute collection.</param>
        /// <exception cref="ArgumentNullException">Value cannot be null</exception>
        /// <exception cref="InvalidSourceFlagException">Cannot change value when SourceFlag is Inherited.</exception>
        /// <exception cref="InvalidOperationException">Cannot add multiple values to collection if IsCollection is false</exception>
        public void SetValue(IValueCollection valueCollection)
        {
            this.SetValue(valueCollection, this.Locale, true, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueCollection"></param>
        /// <param name="formatLocale"></param>
        public void SetValue(IValueCollection valueCollection, LocaleEnum formatLocale)
        {
            this.SetValue(valueCollection, formatLocale, true, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        public void SetValueInvariant(IValueCollection values)
        {
            this.SetValue(values, this.Locale, false, false);
        }

        /// <summary>
        /// Appends new value in already existing values.
        /// If current attribute's <see cref="SourceFlag"/> is Overridden then only it will save the value. Otherwise throws exception
        /// If more than 1 values are being added and attribute is not collection (IsCollection = false) then throws error 
        /// </summary>
        /// <param name="value">New value to add in overridden attribute collection.</param>
        /// <exception cref="InvalidSourceFlagException">Value cannot be null</exception>
        /// <exception cref="InvalidSourceFlagException">Cannot change value when SourceFlag is Inherited.</exception>
        /// <exception cref="InvalidOperationException">Cannot add multiple values to collection if IsCollection is false</exception>
        public void AppendValue(IValue value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("Value");
            }

            this.SetValue(value, this.Locale, true, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="formatLocale"></param>
        public void AppendValue(IValue value, LocaleEnum formatLocale)
        {
            if (value == null)
            {
                throw new ArgumentNullException("Value");
            }

            this.SetValue(value, formatLocale, true, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void AppendValueInvariant(IValue value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("Value");
            }

            this.SetValue(value, this.Locale, false, true);
        }

        /// <summary>
        /// Appends new value in already existing values.
        /// If current attribute's <see cref="SourceFlag"/> is Overridden then only it will save the value. Otherwise throws exception
        /// If more than 1 values are being added and attribute is not collection (IsCollection = false) then throws error 
        /// </summary>
        /// <param name="valueCollection">New value to add in overridden attribute collection.</param>
        /// <exception cref="ArgumentNullException">ValueCollection cannot be null</exception>
        /// <exception cref="InvalidSourceFlagException">Cannot change value when SourceFlag is Inherited.</exception>
        /// <exception cref="InvalidOperationException">Cannot add multiple values to collection if IsCollection is false</exception>
        public void AppendValue(IValueCollection valueCollection)
        {
            this.SetValue(valueCollection, this.Locale, true, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueCollection"></param>
        /// <param name="formatLocale"></param>
        public void AppendValue(IValueCollection valueCollection, LocaleEnum formatLocale)
        {
            this.SetValue(valueCollection, formatLocale, true, true);
        }

        /// <summary>
        ///Appends new value
        /// </summary>
        /// <param name="valueCollection"></param>
        public void AppendValueInvariant(IValueCollection valueCollection)
        {
            this.SetValue(valueCollection, this.Locale, false, true);
        }

        #endregion Set/Append Overridden values

        #endregion Set Attribute Value

        #region Check value

        /// <summary>
        /// Check if attribute has any value (without considering SourceFlag. overridden or inherited)
        /// <param name="ignoreAction">If set to true, action will not be considered, which means the values getting deleted are also counted as values. If set to false, action will be considered and value which are marked for delete are not counted as value</param>
        /// </summary>
        /// <returns>True : If value collection (Inherited or Overridden) has any value. False : otherwise</returns>
        public Boolean HasAnyValue(Boolean ignoreAction)
        {
            //if attribute is set for ignore or delete, there's no value
            if (ignoreAction == false && this.Action == ObjectAction.Delete)
            {
                return false;
            }

            if (this.IsComplex)
            {
                var currentValues = this.GetCurrentValuesInvariant();

                if (currentValues != null && currentValues.Count > 0)
                {
                    return true;
                }
            }
            else
            {
                var overridenValues = _overriddenValues;
                var inheritedValues = _inheritedValues;

                //try finding Overridden values
                if (overridenValues != null && overridenValues.Count > 0)
                {
                    foreach (Value val in overridenValues)
                    {
                        //don't count values which are set for ignore or delete
                        if (ignoreAction == false && val.Action == ObjectAction.Delete)
                            continue;

                        if (val.AttrVal != null && val.AttrVal.ToString().Length > 0)
                        {
                            return true;
                        }
                    }
                }

                //try finding inherited values
                if (inheritedValues != null && inheritedValues.Count > 0)
                {
                    foreach (Value val in inheritedValues)
                    {
                        //don't count values which are set for ignore or delete
                        if (ignoreAction == false && val.Action == ObjectAction.Delete)
                            continue;

                        if (val.AttrVal != null && val.AttrVal.ToString().Length > 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Check if attribute has any value (without considering SourceFlag. overridden or inherited). It will ignore actions by default.
        /// </summary>
        /// <returns>True : If value collection (Inherited or Overridden) has any value. False : otherwise</returns>
        public Boolean HasAnyValue()
        {
            return this.HasAnyValue(true);
        }

        /// <summary>
        /// Check if attribute has any value (considering current source flag)
        /// <param name="ignoreAction">If set to true, action will not be considered, which means the values getting deleted are also counted as values. If set to false, action will be considered and value which are marked for delete are not counted as value</param>
        /// </summary>
        /// <returns>True : Based on current source flag, respective value collection has value. False : otherwise</returns>
        public Boolean HasValue(Boolean ignoreAction)
        {
            //if attribute is set for ignore or delete, there's no value
            if (ignoreAction == false && this.Action == ObjectAction.Delete)
            {
                return false;
            }

            var currentValues = this.GetCurrentValuesInvariant();

            if (this.IsComplex)
            {
                if (currentValues != null && currentValues.Count > 0)
                {
                    return true;
                }
            }
            else
            {
                //try finding value in current values
                if (currentValues != null && currentValues.Count > 0)
                {
                    foreach (Value val in currentValues)
                    {
                        //don't count values which are set for ignore or delete
                        if (ignoreAction == false && val.Action == ObjectAction.Delete)
                            continue;

                        if (val.AttrVal != null && val.AttrVal.ToString().Length > 0)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Check if attribute has any value (considering current source flag). It will ignore actions by default.
        /// </summary>
        /// <returns>True : Based on current source flag, respective value collection has value. False : otherwise</returns>
        public Boolean HasValue()
        {
            return this.HasValue(true);
        }

        /// <summary>
        /// check if attribute value has been changed
        /// </summary>
        /// <returns>True : id current action is Create OR Update OR Delete. False : otherwise</returns>
        public bool CheckHasChanged()
        {
            return Action == ObjectAction.Create
                    || Action == ObjectAction.Update
                    || Action == ObjectAction.Delete;
        }

        #endregion Check value

        #region Clear values

        /// <summary>
        /// Clear values.
        /// If current attribute's <see cref="SourceFlag"/> is Overridden then only it will clear value. Otherwise throws exception
        /// </summary>
        /// <exception cref="InvalidSourceFlagException">Cannot change value when SourceFlag is Inherited.</exception>
        /// <exception cref="ArgumentNullException">OverriddenValues is null. No values to clear</exception>
        public void ClearValue()
        {
            if (this.SourceFlag == AttributeValueSource.Inherited)
            {
                throw new InvalidSourceFlagException("Cannot change value when SourceFlag is Inherited");
            }

            if (this.OverriddenValues == null)
            {
                throw new ArgumentNullException("OverriddenValues", "No values to clear");
            }

            this.OverriddenValues.Clear();
            this.Action = ObjectAction.Delete;
        }

        /// <summary>
        /// Removes the first occurrence of a specific value from the current attribute value collection.
        /// </summary>
        /// <param name="value">The value object to remove from the current attribute value collection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original Value collection</returns>
        /// <exception cref="ArgumentNullException">Value cannot be null</exception>
        /// <exception cref="InvalidSourceFlagException">Cannot change value when SourceFlag is Inherited</exception>
        /// <exception cref="ArgumentNullException">OverriddenValues is null. No values to clear</exception>
        /// <exception cref="InvalidOperationException">Removing value is not supported for Non collection attribute</exception>
        public Boolean RemoveValue(IValue value)
        {
            Boolean result = false;
            if (value == null)
            {
                throw new ArgumentNullException("Value");
            }

            if (this.SourceFlag == AttributeValueSource.Inherited)
            {
                throw new InvalidSourceFlagException("Cannot change value when SourceFlag is Inherited");
            }

            if (this.OverriddenValues == null)
            {
                throw new ArgumentNullException("OverriddenValues", "No values to clear");
            }

            if (this.IsCollection == false)
            {
                throw new InvalidOperationException("Removing value is not supported for Non collection attribute");
            }

            value.Action = ObjectAction.Delete;
            result = this.OverriddenValues.Remove((Value)value);

            if (result)
            {
                if (this.OverriddenValues.Count == 0)
                    this.Action = ObjectAction.Delete;
                else
                    this.Action = ObjectAction.Update;
            }

            return result;
        }

        /// <summary>
        /// Resets the Value id for current values for the attribute
        /// </summary>
        public void ResetValueId()
        {
            ValueCollection valueCollection = (_sourceFlag == AttributeValueSource.Inherited) ? _inheritedValues : _overriddenValues;

            if (valueCollection != null && valueCollection.Count > 0)
            {
                foreach (Value value in valueCollection)
                {
                    if (value.Id > 0)
                    {
                        value.Id = -1;
                    }

                    // Reset for child attributes also
                    if (_attributes != null && _attributes.Count > 0)
                    {
                        foreach (Attribute childAttribute in _attributes)
                        {
                            childAttribute.ResetValueId();
                        }
                    }
                }
            }
        }

        #endregion Clear values

        #region Get complex attribute helper method

        /// <summary>
        /// Get complex child attribute collection from complex attribute.
        /// <para>
        /// If attribute is complex collection then returns collection of child attributes at 1st position
        /// </para>
        /// </summary>
        /// <returns>Collection interface of complex child attributes</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when attribute is not complex attribute.
        /// <para>
        /// Also raised when attribute is not complex attribute. If current attribute is either complex attribute instance or complex child attribute then this error is raised.
        /// </para>
        /// </exception>
        public IAttributeCollection GetComplexChildAttributes()
        {
            #region Validation

            if (this.IsComplex == false && this.IsHierarchical == false)
            {
                throw new InvalidOperationException("Attribute is not a complex attribute. Cannot get Complex child attributes");
            }

            ValidateFirstLevelAttribute("Operation is not supported at this level. Current attribute is either complex attribute instance or a complex child attribute.");

            #endregion Validation

            AttributeCollection complexChildAttributes = null;

            if (this.Attributes != null && this.Attributes.Count > 0)
            {
                //In case current attribute is collection, then return element at 0th position.
                complexChildAttributes = this.Attributes.ElementAt(0).Attributes;
            }

            return complexChildAttributes;
        }

        /// <summary>
        /// Get immediate child attribute collection of hierarchy attribute at given level.
        /// <para>
        /// If attribute is hierarchy collection then returns collection of child attributes at 1st position
        /// </para>
        /// </summary>
        /// <returns>Collection interface of immediate child attributes</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when attribute is not hierarchy attribute.
        /// <para>
        /// Also raised when attribute is not immediate attribute. If current attribute is either immediate attribute instance or immediate child attribute then this error is raised.
        /// </para>
        /// </exception>
        public IAttributeCollection GetHierarchicalChildAttributes()
        {
            #region Validation

            if (this.IsHierarchical == false)
            {
                throw new InvalidOperationException("Attribute is not a hierarchical attribute. Cannot get Hierarchical child attributes");
            }

            #endregion Validation

            AttributeCollection childAttributes = null;

            if (this.Attributes != null && this.Attributes.Count > 0)
            {
                //In case current attribute is collection, then return element at 0th position.
                childAttributes = this.Attributes.ElementAt(0).Attributes;
            }

            return childAttributes;
        }

        /// <summary>
        /// Get complex attribute instance based on InstanceRefId
        /// </summary>
        /// <param name="instanceRefId">InstanceRefId of complex attribute instance which is to be fetched</param>
        /// <returns>Attribute interface having given InstanceRefId.</returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when attribute is not complex attribute.
        ///     <para>
        ///         Also raised when attribute is not complex attribute. If current attribute is either complex attribute instance or complex child attribute then this error is raised.
        ///     </para>
        /// </exception>
        /// <exception cref="DuplicateObjectException">Thrown when multiple complex attributes having same InstanceRefId is found.</exception>
        public IAttribute GetComplexAttributeInstanceByInstanceRefId(Int32 instanceRefId)
        {
            #region Validation

            if (this.IsComplex == false && this.IsHierarchical == false)
            {
                throw new InvalidOperationException("Attribute is not a complex attribute. Cannot get Complex attribute instance");
            }

            ValidateFirstLevelAttribute("Operation is not supported at this level. Current attribute is either complex attribute instance or a complex child attribute.");

            #endregion Validation

            Attribute complexAttribute = null;

            if (this.Attributes != null && this.Attributes.Count > 0)
            {
                IList<Attribute> complexAttrs = null;
                complexAttrs = (from attr in this.Attributes
                                where
                                    attr.InstanceRefId == instanceRefId
                                select attr).ToList<Attribute>();

                if (complexAttrs.Any())
                {
                    //TODO :: Do we  need this validation?
                    //Because if a complex attribute has both O and I value as same InstanceRefId, attribute node will be repeated with same InstanceRefId.
                    if (complexAttrs.Count() > 1)
                    {
                        throw new DuplicateObjectException(String.Concat("Multiple complex attribute instances having InstanceRefId = ", instanceRefId, " found."));
                    }
                    complexAttribute = complexAttrs.FirstOrDefault();
                }
            }

            return complexAttribute;
        }

        /// <summary>
        /// Get complex attribute instance at given sequence.
        /// </summary>
        /// <param name="sequence">Sequence of attribute instance which is to be searched within complex attribute instances</param>
        /// <returns>Attribute interface having at given sequence</returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when attribute is not complex attribute.
        ///     <para>
        ///         Also raised when attribute is not complex attribute. If current attribute is either complex attribute instance or complex child attribute then this error is raised.
        ///     </para>
        /// </exception>
        /// <exception cref="DuplicateObjectException">Thrown when there are values having same sequence.</exception>
        public IAttribute GetComplexAttributeInstanceBySequence(Decimal sequence)
        {
            #region Validation

            if (this.IsComplex == false && this.IsHierarchical == false)
            {
                throw new InvalidOperationException("Attribute is not a complex attribute. Cannot get Complex attribute instance");
            }

            ValidateFirstLevelAttribute("Operation is not supported at this level. Current attribute is either complex attribute instance or a complex child attribute.");

            #endregion Validation

            IAttribute complexAttribute = null;

            if (this.Attributes != null && this.Attributes.Count > 0 && this.CurrentValues != null)
            {
                IValue valueForValueRefId = this.CurrentValues.GetBySequence(sequence);
                if (valueForValueRefId != null)
                {
                    complexAttribute = this.GetComplexAttributeInstanceByInstanceRefId(valueForValueRefId.ValueRefId);
                }

            }

            return complexAttribute;
        }

        /// <summary>
        /// Get complex child attributes for complex attribute instance having given InstanceRefId
        /// </summary>
        /// <param name="instanceRefId">InstanceRefId to be searched</param>
        /// <returns>Collection of attribute interfaces having given InstanceRefId</returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when attribute is not complex attribute.
        ///     <para>
        ///         Also raised when attribute is not complex attribute. If current attribute is either complex attribute instance or complex child attribute then this error is raised.
        ///     </para>
        /// </exception>
        /// <exception cref="DuplicateObjectException">Thrown when multiple complex attribute instance having same InstanceRefId is found.</exception>
        public IAttributeCollection GetComplexChildAttributesByInstanceRefId(Int32 instanceRefId)
        {
            #region Validation

            if (this.IsComplex == false && this.IsHierarchical == false)
            {
                throw new InvalidOperationException("Attribute is not a complex attribute. Cannot get Complex child attributes");
            }

            ValidateFirstLevelAttribute("Operation is not supported at this level. Current attribute is either complex attribute instance or a complex child attribute.");

            #endregion Validation

            IAttributeCollection complexChildAttributes = null;
            IAttribute complexAttribute = this.GetComplexAttributeInstanceByInstanceRefId(instanceRefId);

            if (complexAttribute != null)
            {
                complexChildAttributes = complexAttribute.GetChildAttributes();
            }

            return complexChildAttributes;
        }

        /// <summary>
        /// Get complex child attributes for complex attribute instance having given sequence
        /// </summary>
        /// <param name="sequence">Sequence to be searched</param>
        /// <returns>Collection of attribute interfaces having given InstanceRefId</returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when attribute is not complex attribute.
        ///     <para>
        ///         Also raised when attribute is not complex attribute. If current attribute is either complex attribute instance or complex child attribute then this error is raised.
        ///     </para>
        /// </exception>
        /// <exception cref="DuplicateObjectException">Thrown when multiple complex attribute instance having same Sequence is found.</exception>
        public IAttributeCollection GetComplexChildAttributesBySequence(Decimal sequence)
        {
            #region Validation

            if (this.IsComplex == false && this.IsHierarchical == false)
            {
                throw new InvalidOperationException("Attribute is not a complex attribute. Cannot get Complex child attributes");
            }

            ValidateFirstLevelAttribute("Operation is not supported at this level. Current attribute is either complex attribute instance or a complex child attribute.");

            #endregion Validation

            IAttributeCollection complexChildAttributes = null;
            IAttribute complexAttribute = this.GetComplexAttributeInstanceBySequence(sequence);

            if (complexAttribute != null)
            {
                complexChildAttributes = complexAttribute.GetChildAttributes();
            }

            return complexChildAttributes;
        }

        #endregion Get complex attribute helper method

        #region New complex child record

        /// <summary>
        /// Get new model for complex child record.
        /// </summary>
        /// <returns>Attribute collection interface of complex child attribute</returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when attribute is not complex attribute.
        ///     <para>
        ///         Also raised when attribute is not complex attribute. If current attribute is either complex attribute instance or complex child attribute then this error is raised.
        ///     </para>
        /// </exception>
        /// <exception cref="Exception">Thrown in case there is no child attribute model loaded for complex attribute.
        /// Currently there is no mechanism available through which we can make a DB call and get child attribute model.</exception>
        public IAttributeCollection NewComplexChildRecord()
        {
            #region Validation

            if (this.IsComplex == false && this.IsHierarchical == false)
            {
                throw new InvalidOperationException("Attribute is not a complex attribute. Cannot get Complex child attributes");
            }

            ValidateFirstLevelAttribute("Operation is not supported at this level. Current attribute is either complex attribute instance or a complex child attribute.");
            
            #endregion Validation

            /*
             * Check in cache if complex child attributes are there for current complex attribute.
             * If 
             *  it is there, get it
             * Else 
             *  If at least 1 child attribute record is there, then return those w/o value
             *  Else
             *      make attribute model get for current attribute id, take child attributes and return
             *      
             * Cache is not implemented currently
             */

            IAttributeCollection childAttributes = null;

            AttributeCollection existingChildAttributes = (AttributeCollection)GetComplexOrHierarchyChildAttributes();

            if (existingChildAttributes != null && existingChildAttributes.Any())
            {
                childAttributes = existingChildAttributes.Clone();

                foreach (IAttribute attribute in childAttributes)
                {
                    attribute.InstanceRefId = -1;

                    if (attribute.GetInheritedValues() != null)
                    {
                        attribute.GetInheritedValues().Clear();
                    }

                    if (attribute.GetOverriddenValues() != null)
                    {
                        attribute.GetOverriddenValues().Clear();
                    }
                }
            }
            else
            {
                //TODO :: if existing child attributes are not found then ideally, it should be fetched from DB.
                //TODO :: But currently how do we make DB call from BusinessObject?? So throwing exception currently.
                throw new Exception("Could not get child attributes schema for new complex child record");
            }

            return childAttributes;
        }

        #endregion New complex child record

        #region Add complex record

        /// <summary>
        /// Add new complex attribute instance in complex attribute.
        /// </summary>
        /// <param name="childAttributes">Complex child attributes to add in complex attribute.</param>
        /// <exception cref="InvalidOperationException">
        /// Raised when 
        /// <para>
        ///  - Attribute is not complex attribute.
        /// </para>
        /// <para>
        ///  - Attribute is not level 1 complex attribute. If current attribute is either complex attribute instance or complex child attribute then this error is raised.
        /// </para>
        /// <para>
        /// - Attribute is not collection and more than 1 values are being added.
        /// </para>
        /// </exception>
        /// <exception cref="InvalidSourceFlagException">
        /// Raised when 
        /// <para>
        /// - Source flag is not overridden AttributeValueSource.Overridden and user is trying to add value
        /// </para>
        /// <para>
        /// - Any of childAttributes from argument is not there in complex child attribute model.
        /// </para>
        /// </exception>
        public IAttribute AddComplexChildRecord(IAttributeCollection childAttributes)
        {
            #region Validation

            if (childAttributes == null)
            {
                throw new ArgumentNullException("ChildAttributes");
            }

            if (this.SourceFlag == AttributeValueSource.Inherited)
            {
                throw new InvalidSourceFlagException("Cannot change value when SourceFlag is Inherited");
            }

            if (this.IsComplex == false && this.IsHierarchical == false)
            {
                throw new InvalidOperationException("Attribute is not a complex attribute. Cannot Add Complex child record");
            }

            ValidateFirstLevelAttribute("Operation is not supported at this level. Current attribute is either complex attribute instance or a complex child attribute.");

            if (this._overriddenValues == null)
            {
                this._overriddenValues = new ValueCollection();
            }

            if (this.CurrentValues.Count() > 0 && this.IsCollection == false)
            {
                throw new InvalidOperationException("Attribute is not collection. Cannot add more than 1 value");
            }

            #endregion Validation

            //Get only those values which has Sequence > -1. Because Sequence = -1 is treated as delete of record.
            ValueCollection valuesWithoutDelete = new ValueCollection((from val in this.OverriddenValues where val.Sequence >= 0 select val).ToList<Value>());
            Decimal maxAvailableSeq = -1;
            if (valuesWithoutDelete.Count > 0)
            {
                //So last sequence will be (Count of not deleted values + 1)
                maxAvailableSeq = valuesWithoutDelete.Max(val => val.Sequence);
            }

            IAttribute cxAttributeInstance = null;

            if (!IsCollection)
            {
                cxAttributeInstance = this.AddComplexChildRecordAt(maxAvailableSeq, childAttributes);
            }
            else
            {
                cxAttributeInstance = this.AddComplexChildRecordAt(maxAvailableSeq + 1, childAttributes);
            }

            return cxAttributeInstance;
        }

        /// <summary>
        /// Adds new complex attribute instance in complex attribute with the specified instance reference id.
        /// </summary>
        /// <param name="childAttributes">Complex child attributes to add in complex attribute instance.</param>
        /// <param name="instanceRefId">Reference Id of the instance record</param>
        /// <param name="sourceFlag"></param>
        /// <param name="locale">Reference Id of the instance record</param>
        /// <param name="id">Reference Id of the instance record</param>
        /// <param name="hasInvalidData"></param>
        /// <exception cref="InvalidOperationException">
        /// Raised when 
        /// <para>
        ///  - Attribute is not complex attribute.
        /// </para>
        /// <para>
        ///  - Attribute is not level 1 complex attribute. If current attribute is either complex attribute instance or complex child attribute then this error is raised.
        /// </para>
        /// <para>
        /// - Attribute is not collection and more than 1 values are being added.
        /// </para>
        /// <para>
        /// - Any of childAttributes from argument is not there in complex child attribute model.
        /// </para>
        /// </exception>
        public IAttribute AddComplexChildRecord(IAttributeCollection childAttributes, Int32 instanceRefId, AttributeValueSource sourceFlag, LocaleEnum locale, Int64 id = -1, Boolean hasInvalidData = false)
        {
            #region Validation

            if (childAttributes == null)
            {
                throw new ArgumentNullException("ChildAttributes");
            }

            if (this.IsComplex && !this.IsHierarchical)
            {
                if (instanceRefId < 1 && hasInvalidData == false && instanceRefId != Constants.COMPLEX_ATTRIBUTE_EMPTY_INSTANCE_VALUE_REF_ID)
                {
                    throw new ArgumentException("Instance Reference Id is not available.");
                }
            }

            if (this.IsComplex == false && this.IsHierarchical == false)
            {
                throw new InvalidOperationException("Attribute is not a complex attribute. Cannot Add Complex child record");
            }

            ValidateFirstLevelAttribute("Operation is not supported at this level. Current attribute is either complex attribute instance or a complex child attribute.");

            if (this.CurrentValues.Count() > 0 && this.IsCollection == false)
            {
                throw new InvalidOperationException("Attribute is not collection. Cannot add more than 1 value");
            }

            #endregion Validation

            IAttribute cxAttributeInstance = null;

            //Get only those values which has Sequence > -1. Because Sequence = -1 is treated as delete of record.
            ValueCollection valuesWithoutDelete = null;
            if (sourceFlag == AttributeValueSource.Inherited)
                valuesWithoutDelete = new ValueCollection((from val in this.InheritedValues where val.Sequence >= 0 select val).ToList<Value>());
            else
                valuesWithoutDelete = new ValueCollection((from val in this.OverriddenValues where val.Sequence >= 0 select val).ToList<Value>());

            Decimal maxAvailableSeq = -1;
            if (valuesWithoutDelete.Count > 0)
            {
                //So last sequence will be (Count of not deleted values + 1)
                maxAvailableSeq = valuesWithoutDelete.Max(val => val.Sequence);
            }

            if (!IsCollection)
            {
                cxAttributeInstance = this.AddComplexChildRecordAt(maxAvailableSeq, childAttributes, instanceRefId, sourceFlag, locale, id, hasInvalidData);
            }
            else
            {
                cxAttributeInstance = this.AddComplexChildRecordAt(maxAvailableSeq + 1, childAttributes, instanceRefId, sourceFlag, locale, id, hasInvalidData);
            }

            return cxAttributeInstance;
        }

        /// <summary>
        /// Adds new complex attribute instance in complex attribute at the given sequence.
        /// </summary>
        /// <param name="sequence">The zero-based Sequence at which complex child attribute should be inserted.</param>
        /// <param name="childAttributes">Complex child attributes to add in complex attribute.</param>
        /// <exception cref="InvalidOperationException">
        ///     Raised when 
        ///     <para>
        ///      - Attribute is not complex attribute.
        ///     </para>
        ///     <para>
        ///      - Attribute is not level 1 complex attribute. If current attribute is either complex attribute instance or complex child attribute then this error is raised.
        ///     </para>
        ///     <para>
        ///     - Attribute is not collection and more than 1 values are being added.
        ///     </para>
        /// </exception>
        /// <exception cref="InvalidSourceFlagException">
        ///     Raised when 
        ///     <para>
        ///     - Source flag is not overridden AttributeValueSource.Overridden and user is trying to add value
        ///     </para>
        ///     <para>
        ///     - Any of childAttributes from argument is not there in complex child attribute model.
        ///     </para>
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Raised when ChildAttributes to add is null
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     If sequence is less than 0
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Raised if sequence is greater than no. of values.
        /// </exception>
        public IAttribute AddComplexChildRecordAt(Decimal sequence, IAttributeCollection childAttributes)
        {
            #region Validation

            if (this.SourceFlag == AttributeValueSource.Inherited)
            {
                throw new InvalidSourceFlagException("Cannot change value when SourceFlag is Inherited");
            }

            if (this.IsComplex == false && this.IsHierarchical == false)
            {
                throw new InvalidOperationException("Attribute is not a complex attribute. Cannot Add Complex child record");
            }

            if (childAttributes == null)
            {
                throw new ArgumentNullException("ChildAttributes");
            }

            if (IsCollection)
            {
                if (sequence < 0)
                {
                    throw new ArgumentException("Sequence cannot be less than 0");
                }
            }

            if (this._overriddenValues == null)
            {
                this._overriddenValues = new ValueCollection();
            }

            //Get only those values which has Sequence > -1. Because Sequence = -1 is treated as delete of record.
            ValueCollection valuesWithoutDelete = new ValueCollection((from val in this.OverriddenValues where val.Sequence >= 0 select val).ToList<Value>());
            if (valuesWithoutDelete.Count < sequence)
            {
                throw new ArgumentOutOfRangeException("Sequence", "Sequence must be less than the no. of values. Value count doesn't include complex attribute instances marked as delete.");
            }

            ValidateFirstLevelAttribute("Operation is not supported at this level. Current attribute is either complex attribute instance or a complex child attribute.");

            if (this.OverriddenValues.Count() > 0 && this.IsCollection == false)
            {
                throw new InvalidOperationException("Attribute is not collection. Cannot add more than 1 value");
            }


            #endregion Validation

            #region Validate complex child attribute

            //Get complex child attribute model for current complex attribute.
            //This is to make sure that only actual complex child attributes are being added.
            IAttributeCollection existingChildattributes = this.NewComplexChildRecord();
            if (existingChildattributes != null && existingChildattributes.Count() > 0)
            {
                foreach (Attribute attr in childAttributes)
                {
                    IEnumerable<Attribute> attributeChk = from a in existingChildattributes
                                                          where a.Id == attr.Id
                                                          select attr;

                    //attributeChk == null => meaning we didn't find incoming child attribute in complex child attribute model.
                    //So stop right here... 
                    if (attributeChk == null)
                    {
                        throw new InvalidOperationException(String.Concat("Attribute with Id = ", attr.Id, " Name = ", attr.Name, " does not belong to complex attribute with id = ", this.Id, " and Name = ", this.Name));
                    }
                }
            }

            #endregion Validate complex child attribute to

            //If there are no record found for child attribute, that means there is an empty model. 
            //So when user is adding new record, then delete empty model to avoid duplicate records.
            if (this.InheritedValues.Count < 1 && this.OverriddenValues.Count < 1)
            {
                this.Attributes.Clear();
            }

            Int32 tempRefId = 0;

            //Shift sequences to 1 down when a new complex attribute instance is to be inserted in middle of existing sequence value.
            //Short the values first based on sequence and then loop
            //Loop "valuesWithoutDelete" instead of this.OverriddenValues() because if any value is marked as delete, no need to do Seq correction for those values.
            foreach (Value val in valuesWithoutDelete.OrderByDescending(v => v.Sequence))
            {
                //Change Sequence of only those values which are after the sequence at which we want to add new attribute instance
                if (val.Sequence >= sequence)
                {
                    //newSeq is the new value for sequence which we need to shift by 1 value down when we are inserting new attribute instance in middle
                    Decimal newSeq = val.Sequence + 1;

                    //tempRefId is used to assign temporary value to Value.ValueRefId and Attribute.InstanceRefId
                    //This is required when user is adding multiple attribute instances w/o saving. In this case we will have different sequence values
                    //but for linking between this.Value and this.Attirbutes will we are using InstanceRefId
                    tempRefId = Int32.Parse("-" + newSeq);

                    val.Sequence = newSeq;

                    //Now get the actual attribute from this.Attributes for which we need to update Sequence.
                    IAttribute attr = this.GetComplexAttributeInstanceByInstanceRefId(val.ValueRefId);
                    if (attr != null)
                    {
                        //Update respective values in attribute instance.
                        Attribute attributeInstanceToModify = (Attribute)attr;
                        attributeInstanceToModify.Sequence = newSeq;
                        attributeInstanceToModify.OverriddenValues[0].Sequence = newSeq;
                        if (attributeInstanceToModify.InstanceRefId <= 0)
                        {
                            attributeInstanceToModify.InstanceRefId = tempRefId;
                            attributeInstanceToModify.OverriddenValues[0].ValueRefId = tempRefId;
                        }
                        if (attributeInstanceToModify.Action == ObjectAction.Read)
                        {
                            attributeInstanceToModify.Action = ObjectAction.Update;
                        }
                    }

                    //Change valueRefId only if it is newly added attribute instance. In this case Value.ValueRefId and Attribute.InstanceRefId will not be there.
                    if (val.ValueRefId <= 0)
                    {
                        val.ValueRefId = tempRefId;
                    }
                }
            }

            //Add Value in complex table
            Value cxAttributeValue = new Value();
            cxAttributeValue.Sequence = sequence;
            if (!IsCollection)
            {
                cxAttributeValue.ValueRefId = Int32.Parse(sequence.ToString());
            }
            else
            {
                cxAttributeValue.ValueRefId = Int32.Parse("-" + sequence);
            }

            //De-format value and save it.
            cxAttributeValue.AttrVal = this.DeformatValue(cxAttributeValue.AttrVal, this.Locale);

            this.OverriddenValues.Add(cxAttributeValue);

            #region Create level 2 attribtue (attribute instacne)

            //Level 2 attribute (complex attribute instance) is same as level 1 attribute but following properties are changed in level 2 from level 1
            Attribute cxAttributeInstance = this.CloneBasicProperties();
            cxAttributeInstance.Attributes.Clear();
            if (!IsCollection)
            {
                cxAttributeInstance.InstanceRefId = Int32.Parse(sequence.ToString());
            }
            else
            {
                cxAttributeInstance.InstanceRefId = Int32.Parse("-" + sequence);
            }
            cxAttributeInstance.Name = String.Concat(cxAttributeInstance.Name, " ", this._instanceAttributeSuffix);
            cxAttributeInstance.LongName = String.Concat(cxAttributeInstance.LongName, " ", this._instanceAttributeSuffix);
            cxAttributeInstance.AttributeType = AttributeTypeEnum.Complex;
            cxAttributeInstance.IsCollection = false;
            cxAttributeInstance.Action = ObjectAction.Create;
            cxAttributeInstance.Sequence = sequence;

            //Add value for complex attribute instance
            Value cxInstanceAttributeValue = new Value();
            cxInstanceAttributeValue.Sequence = sequence;
            if (!IsCollection)
            {
                cxInstanceAttributeValue.ValueRefId = Int32.Parse(sequence.ToString());
            }
            else
            {
                cxInstanceAttributeValue.ValueRefId = Int32.Parse("-" + sequence);
            }


            cxAttributeInstance.OverriddenValues.Add(cxInstanceAttributeValue);

            //Add complex attribute instance in actual complex attribute
            this.Attributes.Add(cxAttributeInstance);

            #endregion Create level 2 attribtue (attribute instacne)

            //Add complex child attributes in complex attribute instance.
            foreach (IAttribute cxChild in childAttributes)
            {
                if (cxChild.HasInvalidValues == true)
                {
                    cxAttributeInstance.HasInvalidValues = cxChild.HasInvalidValues;
                    cxAttributeValue.HasInvalidValue = cxChild.HasInvalidValues;
                }

                if (!cxAttributeInstance.Attributes.Contains(new AttributeUniqueIdentifier(cxChild.Name, cxChild.AttributeParentName, cxChild.InstanceRefId), cxChild.Locale))
                {
                    cxChild.Action = ObjectAction.Update;
                    cxAttributeInstance.Attributes.Add(cxChild);
                }
            }

            this.Action = ObjectAction.Update;

            return cxAttributeInstance;
        }

        /// <summary>
        /// Adds new complex attribute instance in complex attribute with the specified instance reference id at given sequence.
        /// </summary>
        /// <param name="sequence">The zero-based Sequence at which complex child attribute should be inserted.</param>
        /// <param name="childAttributes"> Complex child attributes to add in complex attribute instance.</param>
        /// <param name="instanceRefId">Reference Id of the instance record</param>
        /// <param name="sourceFlag"></param>
        /// <param name="locale"></param>
        /// <param name="id"></param>
        /// <param name="hasInvalidData"></param>
        /// <exception cref="InvalidOperationException">
        ///     Raised when 
        ///     <para>
        ///      - Attribute is not complex attribute.
        ///     </para>
        ///     <para>
        ///      - Attribute is not level 1 complex attribute. If current attribute is either complex attribute instance or complex child attribute then this error is raised.
        ///     </para>
        ///     <para>
        ///     - Attribute is not collection and more than 1 values are being added.
        ///     </para>
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Raised when ChildAttributes to add is null
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     If sequence is less than 0 and instanceRefId is less than 1
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Raised if sequence is greater than no. of values.
        /// </exception>
        public IAttribute AddComplexChildRecordAt(Decimal sequence, IAttributeCollection childAttributes, Int32 instanceRefId, AttributeValueSource sourceFlag, LocaleEnum locale, Int64 id = -1, Boolean hasInvalidData = false)
        {
            #region Validation

            if (this.IsComplex == false && this.IsHierarchical == false)
            {
                throw new InvalidOperationException("Attribute is not a complex attribute. Cannot Add Complex child record");
            }

            if (childAttributes == null)
            {
                throw new ArgumentNullException("ChildAttributes");
            }

            if (IsCollection)
            {
                if (sequence < 0)
                {
                    throw new ArgumentException("Sequence cannot be less than 0");
                }
            }

            if (this.IsComplex && !this.IsHierarchical)
            {
                if (instanceRefId < 1 && hasInvalidData == false && instanceRefId != Constants.COMPLEX_ATTRIBUTE_EMPTY_INSTANCE_VALUE_REF_ID)
                {
                    throw new ArgumentException("Instance Reference Id is not available.");
                }
            }

            //Get only those values which has Sequence > -1. Because Sequence = -1 is treated as delete of record.
            ValueCollection valuesWithoutDelete = null;

            if (sourceFlag == AttributeValueSource.Inherited)
                valuesWithoutDelete = new ValueCollection((from val in this.InheritedValues where val.Sequence >= 0 select val).ToList<Value>());
            else
                valuesWithoutDelete = new ValueCollection((from val in this.OverriddenValues where val.Sequence >= 0 select val).ToList<Value>());

            if (valuesWithoutDelete.Count < sequence)
            {
                throw new ArgumentOutOfRangeException("Sequence", "Sequence must be less than the no. of values. Value count doesn't include complex attribute instances marked as delete.");
            }

            ValidateFirstLevelAttribute("Operation is not supported at this level. Current attribute is either complex attribute instance or a complex child attribute.");

            Boolean collectionValidationPassed = true;

            if (sourceFlag == AttributeValueSource.Overridden && this.OverriddenValues.Count() > 0 && this.IsCollection == false)
            {
                collectionValidationPassed = false;
            }
            else if (sourceFlag == AttributeValueSource.Inherited && this.InheritedValues.Count() > 0 && this.IsCollection == false)
            {
                collectionValidationPassed = false;
            }

            if (collectionValidationPassed == false)
            {
                throw new InvalidOperationException("Attribute is not collection. Cannot add more than 1 value");
            }

            #endregion Validation

            #region Validate complex child attribute

            //Get complex child attribute model for current complex attribute.
            //This is to make sure that only actual complex child attributes are being added.
            IAttributeCollection existingChildattributes = this.NewComplexChildRecord();
            if (existingChildattributes != null && existingChildattributes.Count() > 0)
            {
                foreach (Attribute attr in childAttributes)
                {
                    IEnumerable<Attribute> attributeChk = from a in existingChildattributes
                                                          where a.Id == attr.Id
                                                          select attr;

                    //attributeChk == null => meaning we didn't find incoming child attribute in complex child attribute model.
                    //So stop right here... 
                    if (attributeChk == null)
                    {
                        throw new InvalidOperationException(String.Concat("Attribute with Id = ", attr.Id, " Name = ", attr.Name, " does not belong to complex attribute with id = ", this.Id, " and Name = ", this.Name));
                    }
                }
            }

            #endregion Validate complex child attribute to

            //If there are no record found for child attribute, that means there is an empty model. 
            //So when user is adding new record, then delete empty model to avoid duplicate records.
            if (this.InheritedValues.Count < 1 && this.OverriddenValues.Count < 1)
            {
                this.Attributes.Clear();
            }

            Int32 tempRefId = 0;

            //Shift sequences to 1 down when a new complex attribute instance is to be inserted in middle of existing sequence value.
            //Short the values first based on sequence and then loop
            //Loop "valuesWithoutDelete" instead of this.OverriddenValues() because if any value is marked as delete, no need to do Seq correction for those values.
            foreach (Value val in valuesWithoutDelete.OrderByDescending(v => v.Sequence))
            {
                //Change Sequence of only those values which are after the sequence at which we want to add new attribute instance
                if (val.Sequence >= sequence)
                {
                    //newSeq is the new value for sequence which we need to shift by 1 value down when we are inserting new attribute instance in middle
                    Decimal newSeq = val.Sequence + 1;

                    //tempRefId is used to assign temporary value to Value.ValueRefId and Attribute.InstanceRefId
                    //This is required when user is adding multiple attribute instances w/o saving. In this case we will have different sequence values
                    //but for linking between this.Value and this.Attirbutes will we are using InstanceRefId
                    tempRefId = Int32.Parse("-" + newSeq);

                    val.Sequence = newSeq;

                    //Now get the actual attribute from this.Attributes for which we need to update Sequence.
                    IAttribute attr = this.GetComplexAttributeInstanceByInstanceRefId(val.ValueRefId);
                    if (attr != null)
                    {
                        //Update respective values in attribute instance.
                        Attribute attributeInstanceToModify = (Attribute)attr;
                        attributeInstanceToModify.Sequence = newSeq;
                        attributeInstanceToModify.OverriddenValues[0].Sequence = newSeq;
                        if (attributeInstanceToModify.InstanceRefId <= 0)
                        {
                            attributeInstanceToModify.InstanceRefId = tempRefId;
                            attributeInstanceToModify.OverriddenValues[0].ValueRefId = tempRefId;
                        }
                    }

                    //Change valueRefId only if it is newly added attribute instance. In this case Value.ValueRefId and Attribute.InstanceRefId will not be there.
                    if (val.ValueRefId <= 0)
                    {
                        val.ValueRefId = tempRefId;
                    }
                }
            }

            //Add Value in complex table
            Value cxAttributeValue = new Value();
            cxAttributeValue.Sequence = sequence;
            cxAttributeValue.ValueRefId = instanceRefId;
            cxAttributeValue.Locale = locale;
            cxAttributeValue.AttrVal = instanceRefId;
            cxAttributeValue.InvariantVal = instanceRefId;
            cxAttributeValue.Id = id;

            if (sourceFlag == AttributeValueSource.Inherited)
                this.InheritedValues.Add(cxAttributeValue);
            else
                this.OverriddenValues.Add(cxAttributeValue);

            #region Create level 2 attribute (attribute instance)

            //Level 2 attribute (complex attribute instance) is same as level 1 attribute but following properties are changed in level 2 from level 1
            Attribute cxAttributeInstance = this.CloneBasicProperties();
            cxAttributeInstance.Attributes.Clear();
            cxAttributeInstance.InstanceRefId = instanceRefId;
            cxAttributeInstance.Name = String.Concat(cxAttributeInstance.Name, " ", this._instanceAttributeSuffix);
            cxAttributeInstance.LongName = String.Concat(cxAttributeInstance.LongName, " ", this._instanceAttributeSuffix);
            cxAttributeInstance.AttributeType = AttributeTypeEnum.Complex;
            cxAttributeInstance.IsCollection = false;
            cxAttributeInstance.Sequence = sequence;
            cxAttributeInstance.SourceFlag = sourceFlag;

            //Add value for complex attribute instance
            if (sourceFlag == AttributeValueSource.Inherited)
            {
                cxAttributeInstance.InheritedValues.Add(cxAttributeValue);
            }
            else if (sourceFlag == AttributeValueSource.Overridden)
            {
                cxAttributeInstance.OverriddenValues.Add(cxAttributeValue);
            }

            cxAttributeInstance.HasInvalidValues = hasInvalidData;

            //Add complex attribute instance in actual complex attribute or always add if it contains invalid values.
            //Adding this check , just to make sure we don't have duplicate entry of the complex attribute Instance with same WSID
            //Case:When Inherited and Overridden Values have same Values, with different sequence.
            if (this.HasInvalidValues || !ComplexAttributeInstanceExists(cxAttributeInstance))
            {
                this.Attributes.Add(cxAttributeInstance);
            }

            #endregion Create level 2 attribtue (attribute instacne)

            //Add complex child attributes in complex attribute instance.

            cxAttributeInstance.Attributes.AddRange((AttributeCollection)childAttributes);

            //Update source flag of all the child attributes
            foreach (Attribute attribute in childAttributes)
            {
                //At instance level,We set child attributes as Overridden
                attribute.SourceFlag = AttributeValueSource.Overridden;
            }

            //Update source flag of attribute
            if (this.OverriddenValues != null && this.OverriddenValues.Count > 0)
                this.SourceFlag = AttributeValueSource.Overridden;
            else
                this.SourceFlag = AttributeValueSource.Inherited;

            return cxAttributeInstance;
        }

        #endregion Add complex record

        #region Remove complex record

        /// <summary>
        /// Remove value from Non collection complex attribute.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Raised when 
        /// <para>
        ///  - Attribute is not complex attribute.
        /// </para>
        /// <para>
        ///  - Attribute is not level 1 complex attribute. If current attribute is either complex attribute instance or complex child attribute then this error is raised.
        /// </para>
        /// <para>
        ///  - Attribute is collection
        /// </para>
        /// </exception>
        /// <exception cref="InvalidSourceFlagException">
        /// Raised when 
        /// <para>
        /// - Source flag is not overridden AttributeValueSource.Overridden and user is trying to add value
        /// </para>
        /// </exception>
        public void RemoveComplexChildRecord()
        {
            #region Validation

            if (this.SourceFlag == AttributeValueSource.Inherited)
            {
                throw new InvalidSourceFlagException("Cannot change value when SourceFlag is Inherited");
            }

            if (this.IsComplex == false && this.IsHierarchical == false)
            {
                throw new InvalidOperationException("Attribute is not a complex attribute. Cannot Add Complex child record");
            }

            ValidateFirstLevelAttribute("Operation is not supported at this level. Current attribute is either complex attribute instance or a complex child attribute.");

            if (this._overriddenValues == null)
            {
                this._overriddenValues = new ValueCollection();
            }

            if (this.IsCollection == true)
            {
                throw new InvalidOperationException("Attribute is a collection attribute. Remove attribute value using sequence(RemoveComplexChildRecordBySequence) or InstanceRefId (RemoveComplexChildRecordByInstanceRefId)");
            }

            #endregion Validation

            this.Attributes.ElementAt(0).Action = ObjectAction.Delete;
            this.Action = ObjectAction.Update;
        }

        /// <summary>
        /// Remove complex child attribute based on Sequence.
        /// </summary>
        /// <param name="sequence">Sequence to remove</param>
        /// <exception cref="InvalidOperationException">
        /// Raised when 
        /// <para>
        ///  - Attribute is not complex attribute.
        /// </para>
        /// <para>
        ///  - Attribute is not level 1 complex attribute. If current attribute is either complex attribute instance or complex child attribute then this error is raised.
        /// </para>
        /// </exception>
        /// <exception cref="InvalidSourceFlagException">
        /// Raised when 
        /// <para>
        /// - Source flag is not overridden AttributeValueSource.Overridden and user is trying to add value
        /// </para>
        /// </exception>
        /// <exception cref="ArgumentException">Raised when sequence is less than 0</exception>
        /// <exception cref="ArgumentOutOfRangeException">Raised when sequence value is more than no. of values in attribute</exception>
        public void RemoveComplexChildRecordBySequence(Decimal sequence)
        {
            #region Validation

            if (this.SourceFlag == AttributeValueSource.Inherited)
            {
                throw new InvalidSourceFlagException("Cannot change value when SourceFlag is Inherited");
            }

            if (this.IsComplex == false && this.IsHierarchical == false)
            {
                throw new InvalidOperationException("Attribute is not a complex attribute. Cannot Add Complex child record");
            }

            ValidateFirstLevelAttribute("Operation is not supported at this level. Current attribute is either complex attribute instance or a complex child attribute.");

            if (sequence < 0)
            {
                throw new ArgumentException("Sequence cannot be less than 0");
            }

            if (this._overriddenValues == null)
            {
                this._overriddenValues = new ValueCollection();
            }

            if (this.OverriddenValues.Count < sequence)
            {
                throw new ArgumentOutOfRangeException("Sequence", "Sequence must be less than the no. of values");
            }

            #endregion Validation

            if (this.IsCollection == true)
            {
                Decimal newSeq = 0;
                //Int32 tempRefId = 0;

                //Shift sequences to 1 down when a new complex attribute instance is to be inserted in middle of existing sequence value.
                //Short the values first based on sequence and then loop
                foreach (Value val in this.OverriddenValues.OrderBy(v => v.Sequence))
                {
                    if (val.Sequence == sequence)
                    {
                        //We are at proper value which is to be deleted (by sequence)
                        //So set sequence = -1 and action = delete
                        val.Sequence = -1;
                        IAttribute attr = this.GetComplexAttributeInstanceByInstanceRefId(val.ValueRefId);
                        if (attr != null)
                        {
                            Attribute attributeInstanceToModify = (Attribute)attr;
                            attributeInstanceToModify.Sequence = -1;
                            attributeInstanceToModify.OverriddenValues[0].Sequence = -1;
                            attributeInstanceToModify.Action = ObjectAction.Delete;
                        }
                    }
                    else if (val.Sequence > sequence)
                    {
                        //Change Sequence of only those values which are after the sequence at which we want to delete attribute instance

                        newSeq = val.Sequence - 1;

                        //newSeq is the new value for sequence which we need to shift by 1 value up when we are inserting new attribute instance in middle
                        val.Sequence = newSeq;

                        //Now get the actual attribute from this.Attributes for which we need to update Sequence.
                        IAttribute attr = this.GetComplexAttributeInstanceByInstanceRefId(val.ValueRefId);
                        if (attr != null)
                        {
                            //Update respective values in attribute instance.
                            Attribute attributeInstanceToModify = (Attribute)attr;
                            attributeInstanceToModify.Sequence = newSeq;
                            attributeInstanceToModify.OverriddenValues[0].Sequence = newSeq;
                            if (attributeInstanceToModify.Action == ObjectAction.Read)
                            {
                                attributeInstanceToModify.Action = ObjectAction.Update;
                            }
                        }
                    }
                }
            }
            else
            {
                this.Attributes.ElementAt(0).Action = ObjectAction.Delete;
            }
            this.Action = ObjectAction.Update;
        }

        /// <summary>
        /// Remove complex child attribute based on ValueRefId.
        /// </summary>
        /// <param name="instanceRefId">ValueRefId to remove</param>
        /// <exception cref="InvalidOperationException">
        /// Raised when 
        /// <para>
        ///  - Attribute is not complex attribute.
        /// </para>
        /// <para>
        ///  - Attribute is not level 1 complex attribute. If current attribute is either complex attribute instance or complex child attribute then this error is raised.
        /// </para>
        /// </exception>
        /// <exception cref="InvalidSourceFlagException">
        /// Raised when 
        /// <para>
        /// - Source flag is not overridden AttributeValueSource.Overridden and user is trying to add value
        /// </para>
        /// </exception>
        /// <exception cref="ArgumentException">Raised when ValueRefId is less than 0</exception>
        /// <exception cref="ArgumentOutOfRangeException">Raised when sequence value is more than no. of values in attribute</exception>
        /// <exception cref="Exception">Raised when no Value is found with given ValueRefId</exception>
        public void RemoveComplexChildRecordByInstanceRefId(Int32 instanceRefId)
        {
            #region Validation

            if (this.SourceFlag == AttributeValueSource.Inherited)
            {
                throw new InvalidSourceFlagException("Cannot change value when SourceFlag is Inherited");
            }

            if (this.IsComplex == false && this.IsHierarchical == false)
            {
                throw new InvalidOperationException("Attribute is not a complex attribute. Cannot Add Complex child record");
            }

            ValidateFirstLevelAttribute("Operation is not supported at this level. Current attribute is either complex attribute instance or a complex child attribute.");

            if (instanceRefId < 0)
            {
                throw new ArgumentException("ValueRefId cannot be less than 0");
            }

            if (this._overriddenValues == null)
            {
                this._overriddenValues = new ValueCollection();
            }

            #endregion Validation

            IValue valueToDelete = this.OverriddenValues.GetByValueRefId(instanceRefId);
            if (valueToDelete == null)
            {
                throw new Exception(String.Concat("No Attribute with InstanceRefId = ", instanceRefId, " found"));
            }

            if (this.IsCollection == true)
            {
                this.RemoveComplexChildRecordBySequence(valueToDelete.Sequence);
            }
            else
            {
                this.Attributes.ElementAt(0).Action = ObjectAction.Delete;
                this.Action = ObjectAction.Update;
            }

        }

        #endregion Remove complex record

        #region Hierarchy Attribute Methods
        
        #region Hierarchy Attribute Add Methods

        /// <summary>
        /// Adds new complex attribute instance in complex attribute with the specified instance reference id.
        /// </summary>
        /// <param name="childAttributes">Complex child attributes to add in complex attribute instance.</param>
        /// <param name="instanceRefId">Reference Id of the instance record</param>
        /// <param name="sourceFlag"></param>
        /// <param name="locale">Reference Id of the instance record</param>
        /// <param name="id">Reference Id of the instance record</param>
        /// <param name="hasInvalidData"></param>
        /// <exception cref="InvalidOperationException">
        /// Raised when 
        /// <para>
        ///  - Attribute is not hierarchy attribute.
        /// </para>
        /// </exception>
        public IAttribute AddHierarchyChildRecord(IAttributeCollection childAttributes, Int32 instanceRefId, AttributeValueSource sourceFlag, LocaleEnum locale, Int64 id = -1, Boolean hasInvalidData = false)
        {
            #region Validation

            if (childAttributes == null)
            {
                throw new ArgumentNullException("ChildAttributes");
            }

            if (this.IsComplex == false && this.IsHierarchical == false)
            {
                throw new InvalidOperationException("Attribute is not a complex attribute. Cannot Add Complex child record");
            }

            ValidateFirstLevelAttribute("Operation is not supported at this level. Current attribute is either complex attribute instance or a complex child attribute.");

            if (this.CurrentValues.Count() > 0 && this.IsCollection == false)
            {
                throw new InvalidOperationException("Attribute is not collection. Cannot add more than 1 value");
            }

            #endregion Validation

            IAttribute cxAttributeInstance = null;

            //Get only those values which has Sequence > -1. Because Sequence = -1 is treated as delete of record.
            ValueCollection valuesWithoutDelete = null;
            if (sourceFlag == AttributeValueSource.Inherited)
                valuesWithoutDelete = new ValueCollection((from val in this.InheritedValues where val.Sequence >= 0 select val).ToList<Value>());
            else
                valuesWithoutDelete = new ValueCollection((from val in this.OverriddenValues where val.Sequence >= 0 select val).ToList<Value>());

            Decimal maxAvailableSeq = -1;
            if (valuesWithoutDelete.Count > 0)
            {
                //So last sequence will be (Count of not deleted values + 1)
                maxAvailableSeq = valuesWithoutDelete.Max(val => val.Sequence);
            }

            if (!IsCollection)
            {
                cxAttributeInstance = this.AddComplexChildRecordAt(maxAvailableSeq, childAttributes, instanceRefId, sourceFlag, locale, id, hasInvalidData);
            }
            else
            {
                cxAttributeInstance = this.AddComplexChildRecordAt(maxAvailableSeq + 1, childAttributes, instanceRefId, sourceFlag, locale, id, hasInvalidData);
            }

            return cxAttributeInstance;
        }

        #endregion

        #endregion

        #region Misc. Methods

        /// <summary>
        /// Get child attributes for current attribute
        /// </summary>
        /// <returns>Attribute collection interface representing child attributes</returns>
        /// <exception cref="NullReferenceException">Thrown if child attributes is null</exception>
        public IAttributeCollection GetChildAttributes()
        {
            if (this.Attributes == null)
            {
                throw new NullReferenceException("ChildAttributes is null");
            }
            return (IAttributeCollection)this.Attributes;
        }

        /// <summary>
        /// Get child attributes for current attribute
        /// </summary>
        /// <returns>Attribute collection interface representing child attributes</returns>
        /// <exception cref="NullReferenceException">Thrown if child attributes is null</exception>
        public IAttributeCollection GetChildAttributes(AttributeValueSource sourceFlag)
        {
            if (this.Attributes == null)
            {
                throw new NullReferenceException("ChildAttributes is null");
            }
            return new AttributeCollection(this.Attributes.Where(attr => attr.SourceFlag == sourceFlag).ToList());
        }

        /// <summary>
        /// Get Attribute Id and Locale as a key value pair
        /// </summary>
        /// <returns>KeyValuePair of attibute id and its locale</returns>
        public KeyValuePair<Int32, LocaleEnum> GetAttributeIdLocalePair()
        {
            return new KeyValuePair<int, LocaleEnum>(this.Id, this.Locale);
        }

        /// <summary>
        /// Set given locale for all values for current attribute
        /// </summary>
        /// <param name="localeShortName">Locale name (en_WW) to set for values </param>
        /// <exception cref="ArgumentException">localeShortName cannot be null</exception>
        /// <exception cref="InvalidSourceFlagException">Cannot change value's locale when SourceFlag is Inherited.</exception>
        public void SetLocale(String localeShortName)
        {
            if (this.SourceFlag == AttributeValueSource.Inherited)
            {
                throw new InvalidSourceFlagException("Cannot change value when SourceFlag is Inherited");
            }

            if (String.IsNullOrWhiteSpace(localeShortName))
            {
                throw new ArgumentException("localeShortName cannot be null or empty");
            }

            if (this.OverriddenValues.Count > 0)
            {
                LocaleEnum locale = LocaleEnum.UnKnown;
                Enum.TryParse<Core.LocaleEnum>(localeShortName, out locale);

                foreach (Value val in this.OverriddenValues)
                {
                    val.Locale = locale;
                    val.Action = ObjectAction.Update;
                }

                //TODO:: What to do for locale at attribute level...Ideally it should not be at attribute level to support multiple locale values in attribute object...
                this.Locale = locale;
                this.Action = ObjectAction.Update;
            }
        }

        #endregion Misc. Methods

        #region Merge Methods

        /// <summary>
        /// Delta Merge of Attribute Values
        /// </summary>
        /// <param name="deltaAttribute">Attribute that needs to be merged.</param>
        /// <param name="flushExistingValues">True if existing values needs to be flushed else false</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Merged attribute instance</returns>
        public IAttribute MergeDelta(IAttribute deltaAttribute, Boolean flushExistingValues, ICallerContext iCallerContext)
        {
            return MergeDelta(deltaAttribute, flushExistingValues, StringComparison.InvariantCultureIgnoreCase, iCallerContext);
        }

        /// <summary>
        /// Delta Merge of Attribute Values
        /// </summary>
        /// <param name="deltaAttribute">Attribute that needs to be merged.</param>
        /// <param name="flushExistingValues">True if existing values needs to be flushed else false</param>
        /// <param name="stringComparison">Indicates string comparison options to be used</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Merged attribute instance</returns>
        public IAttribute MergeDelta(IAttribute deltaAttribute, Boolean flushExistingValues, StringComparison stringComparison, ICallerContext iCallerContext)
        {
            return MergeDelta(deltaAttribute, flushExistingValues, stringComparison, AttributeCompareAndMergeBehavior.CompareOverriddenValuesOnly, iCallerContext);
        }

        /// <summary>
        /// Delta Merge of Attribute Values based on the defined merge behavior
        /// </summary>
        /// <param name="deltaAttribute">Attribute that needs to be merged</param>
        /// <param name="flushExistingValues">True if existing values needs to be flushed else false</param>
        /// <param name="stringComparison">Comparison options to be used while finding changes</param>
        /// <param name="attributeCompareAndMergeBehavior">merge behavior of the attribute values</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Merged attribute instance</returns>
        public IAttribute MergeDelta(IAttribute deltaAttribute, Boolean flushExistingValues, StringComparison stringComparison, AttributeCompareAndMergeBehavior attributeCompareAndMergeBehavior, ICallerContext iCallerContext)
        {
            //Warning: Please dont call this method for complex attributes as it is not yet supported.
            //TODO:: Write support for complex attributes..

            //Create clone copy of attribute to start with..
            Attribute mergedAttribute = this.Clone(); // This would clone all the properties, values, and child attributes

            //Start with setting action as Read
            mergedAttribute.Action = ObjectAction.Read;

            ValueCollection mergedValues = (ValueCollection)mergedAttribute.GetCurrentValuesInvariant();

            if (deltaAttribute == null)
                return mergedAttribute;

            ValueCollection deltaValues = (ValueCollection)deltaAttribute.GetCurrentValuesInvariant();

            if (deltaValues == null)
                deltaValues = new ValueCollection();

            //If attribute is asked to delete..just mark orig as delete
            if (deltaAttribute.Action == ObjectAction.Delete)
            {
                mergedAttribute.Action = ObjectAction.Delete;
                mergedValues.SetAction(ObjectAction.Delete);

                IValueCollection inheritedValues = (ValueCollection)deltaAttribute.GetInheritedValues();

                if (inheritedValues != null && inheritedValues.Count > 0)
                {
                    mergedAttribute.SourceFlag = AttributeValueSource.Inherited;
                }
            }
            else
            {
                // O to I
                if (mergedAttribute.SourceFlag == AttributeValueSource.Overridden && deltaAttribute.SourceFlag == AttributeValueSource.Inherited)
                {
                    mergedAttribute.SourceFlag = AttributeValueSource.Inherited;
                    mergedAttribute.Action = ObjectAction.Delete;
                    mergedValues.SetAction(ObjectAction.Delete);
                }
                // I to O
                else if (mergedAttribute.SourceFlag == AttributeValueSource.Inherited && deltaAttribute.SourceFlag == AttributeValueSource.Overridden && attributeCompareAndMergeBehavior == AttributeCompareAndMergeBehavior.CompareOverriddenValuesOnly)
                {
                    mergedAttribute.Action = ObjectAction.Create;
                    mergedAttribute.SourceFlag = AttributeValueSource.Overridden;

                    //If Inherited Collection attributes have values A and B and we are adding same overridden values, then mergedAttribute will have duplicate values.
                    //So clear mergedAttributes Values and add only Overridden values.
                    mergedAttribute._overriddenValues.Clear();
                    ValueCollection mergedOverriddenValues = new ValueCollection();
                    Int32 seq = -1;

                    if (mergedAttribute.IsCollection)
                        seq = 0;

                    Int64 id = -1;

                    foreach (Value deltaVal in deltaValues)
                    {
                        deltaVal.Sequence = seq++;
                        deltaVal.Id = id--;
                        deltaVal.Action = ObjectAction.Create;
                        mergedOverriddenValues.Add(deltaVal);
                    }

                    mergedAttribute._overriddenValues = mergedOverriddenValues;
                }
                //I to I
                else if (mergedAttribute.SourceFlag == AttributeValueSource.Inherited && deltaAttribute.SourceFlag == AttributeValueSource.Inherited)
                {
                    if (iCallerContext.Module == MDMCenterModules.Denorm)
                    {
                        //In case of denorm, we need to process inherited values..
                        //Merge Values and send for update..
                        MergeValues(mergedAttribute, (Attribute)deltaAttribute, mergedValues, deltaValues, flushExistingValues, stringComparison);
                    }
                    else
                    {
                        //DO nothing in case of normal Attribute Update..
                        mergedAttribute.Action = ObjectAction.Read;
                        mergedValues.SetAction(ObjectAction.Read);
                    }
                }
                //O to O
                //I to O with comparison ( same as O to O )
                else if ((mergedAttribute.SourceFlag == AttributeValueSource.Overridden && deltaAttribute.SourceFlag == AttributeValueSource.Overridden) ||
                                (attributeCompareAndMergeBehavior == AttributeCompareAndMergeBehavior.CompareOverriddenAndInheritedValues && mergedAttribute.SourceFlag == AttributeValueSource.Inherited && deltaAttribute.SourceFlag == AttributeValueSource.Overridden))
                {
                    MergeValues(mergedAttribute, (Attribute)deltaAttribute, mergedValues, deltaValues, flushExistingValues, stringComparison);
                }
            }

            #region Merge child attributes

            //Write logic here..
            //if (this._attributes != null && this._attributes.Count > 0)
            //{
            //AttributeCollection mergedAttributes = new AttributeCollection();

            //foreach (Attribute childAttribute in this._attributes)
            //{
            //Step 1: Write logic to find instance delta...and define instance level action..
            //Step 2: write logic to find instance attr level delta...for now we dont need this as SP dont care abut this..
            //Write logic here...
            //Attribute mergedChildAttr = childAttribute.MergeDelta(deltaAttribute); // Recurse method
            //mergedAttributes.Add(mergedChildAttr);
            //}

            //mergedAttribute._attributes = mergedAttributes;
            //}

            #endregion

            #region Update Program Name and User Name

            mergedAttribute.UserName = deltaAttribute.UserName;
            mergedAttribute.ProgramName = deltaAttribute.ProgramName;

            #endregion

            return mergedAttribute;
        }

        /// <summary>
        /// Delta Merge of Attribute Values used by lineage-based merge
        /// </summary>
        /// <param name="source">Attribute that needs to be merged.</param>
        /// <param name="collectionStrategy">Strategy of collection merging</param>
        /// <param name="allowInhiritantToOverridenMerging">Flag indicates whether I(source) to O(target) merge allowed</param>
        /// <param name="isSourceProcessingEnabled">Indicates if needed process source info</param>
        /// <returns>Merged attribute instance</returns>
        public IAttribute MergeDelta(IAttribute source, CollectionStrategy collectionStrategy, Boolean allowInhiritantToOverridenMerging, Boolean isSourceProcessingEnabled = true)
        {
            return MergeDelta(source, collectionStrategy, allowInhiritantToOverridenMerging, StringComparison.InvariantCultureIgnoreCase, isSourceProcessingEnabled);
        }

        /// <summary>
        /// Delta Merge of Attribute Values used by lineage-based merge
        /// </summary>
        /// <param name="source">Attribute that needs to be merged.</param>
        /// <param name="collectionStrategy">Strategy of collection merging</param>
        /// <param name="allowInhiritantToOverridenMerging">Flag indicates whether I(source) to O(target) merge allowed</param>
        /// <param name="stringComparison">Indicates string comparison options to be used</param>
        /// <param name="isSourceProcessingEnabled">Indicates if needed process source info</param>
        /// <returns>Merged attribute instance</returns>
        public IAttribute MergeDelta(IAttribute source, CollectionStrategy collectionStrategy, Boolean allowInhiritantToOverridenMerging, StringComparison stringComparison, Boolean isSourceProcessingEnabled = true)
        {
            Attribute target = this.Clone(); // This would clone all the properties, values, and child attributes

            ValueCollection mergedValues = (ValueCollection)target.GetCurrentValuesInvariant();

            if (source == null)
                return target;

            ValueCollection deltaValues = (ValueCollection)source.GetCurrentValuesInvariant() ??
                                          new ValueCollection();

            if ((target.SourceFlag == AttributeValueSource.Overridden &&
                 source.SourceFlag == AttributeValueSource.Overridden) ||
                ((source.SourceFlag == AttributeValueSource.Overridden &&
                 target.SourceFlag == AttributeValueSource.Inherited)))
            {
                MergeValues(target, source, mergedValues, deltaValues, collectionStrategy, stringComparison, isSourceProcessingEnabled);
            }
            else if ((source.SourceFlag == AttributeValueSource.Inherited && target.SourceFlag == AttributeValueSource.Overridden
                       && allowInhiritantToOverridenMerging))
            {
                target.Action = ObjectAction.Create;
                target.SourceFlag = AttributeValueSource.Overridden;
                MergeValues(target, source, mergedValues, deltaValues, collectionStrategy, stringComparison, isSourceProcessingEnabled);
            }
            else
            {
                target.Action = ObjectAction.Read;
                mergedValues.SetAction(ObjectAction.Read);
            }

            return target;
        }

        #endregion

        #region Internal Key related methods

        /// <summary>
        /// Returns internal unique key by combining attr id + locale
        /// </summary>
        public Int32 GetInternalUniqueKey()
        {
            return (this.Id << 7) + (Int16)this.Locale;
        }

        /// <summary>
        /// Returns internal unique key by combining attr id + locale
        /// </summary>
        public static Int32 GetInternalUniqueKey(Int32 attributeId, LocaleEnum locale)
        {
            return (attributeId << 7) + (Int16)locale;
        }

        /// <summary>
        /// Retrives and return attribute id from the internal unique key
        /// </summary>
        public static Int32 RetriveAttributeIdFromInternalKey(Int32 internalUniqueKey)
        {
            return (internalUniqueKey >> 7);
        }

        /// <summary>
        /// Retrives and return attribute id from the internal unique key
        /// </summary>
        public static LocaleEnum RetriveLocaleFromInternalKey(Int32 internalUniqueKey)
        {
            return ((LocaleEnum)(internalUniqueKey & 127));
        }

        #endregion

        #region validate
        
        /// <summary>
        /// Validates the Hierarchial Attribute for a given attribute model.  Removes any attributes that are not defined in attribute model 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>AttributeOperationResultCollection</returns>
        /// <param name="addMissingAttribues"></param>
        /// <returns>All the attribute names that are invalid - doesnt confirm to the given model</returns>
        public AttributeOperationResultCollection ValidateHierarchicalAttribute(AttributeModel model, Boolean addMissingAttribues = true)
        {
            #region Validation

            if (!this.IsHierarchical)
            {
                throw new InvalidOperationException("Attribute is not a hierarchical attribute.");
            }

            if (model == null || !model.IsHierarchical)
            {
                throw new InvalidOperationException("Attribute Model in null or does not represent an not a hierarchical attribute.");
            }

            #endregion Validation

            var attributeOperationResults = new AttributeOperationResultCollection();

            var childInstanceAttributes = this.GetChildAttributes();

            if (childInstanceAttributes != null && childInstanceAttributes.Count > 0)
            {
                if ((!this.IsCollection) && childInstanceAttributes.Count > 1)
                {
                    throw new InvalidDataException(String.Format("Non collection Hierarchial Attribute has more than one instance record: {0}", this.Name));
                }

                foreach (var instanceAttribute in childInstanceAttributes)
                {
                    ValidateHierarchicalAttribute(instanceAttribute.GetChildAttributes(), model.GetChildAttributeModels(), attributeOperationResults, addMissingAttribues);
                }
            }

            return attributeOperationResults;
        }
        #endregion

        #endregion Public Methods

        #region Private methods

        /// <summary>
        /// Validates the hierarchal attributes collection recursively using the attribute model collection.
        /// </summary>
        /// <param name="attributeCollection"></param>
        /// <param name="modelCollection"></param>
        /// <param name="attributeOperationResultCollection"></param>
        /// <param name="addMissingAttribues"></param>
        private void ValidateHierarchicalAttribute(IAttributeCollection attributeCollection, IAttributeModelCollection modelCollection, AttributeOperationResultCollection attributeOperationResultCollection, Boolean addMissingAttribues = true)
        {
            if (attributeOperationResultCollection == null)
            {
                throw new ArgumentNullException("AttributeOperationResultCollection");
            }

            if ((attributeCollection == null) || (modelCollection == null))
            {
                return;
            }

            var attributesToPurge = new List<Attribute>();

            //The attributes collection does not contain attributes not defined in the model 
            if (modelCollection.Count() > attributeCollection.Count())
            {
                var attributesNotPerModel = from m in modelCollection
                                        join a in attributeCollection on m.Name equals a.Name into ma
                                        from modelattribute in ma.DefaultIfEmpty()
                                        select new { m.Name };

                if (attributesNotPerModel != null)
                {
                    foreach (var attributeNotPerModel in attributesNotPerModel)
                    {
                        var operationResult = new AttributeOperationResult { AttributeShortName = attributeNotPerModel.Name };
                        operationResult.Errors.Add(new Error("", "No matching attribute model found for this attribute."));
                        attributeOperationResultCollection.Add(operationResult);
                    }
                }
            }

            // Find all child attributes that are extra per model and add to attribute purge list.
            foreach (var attribute in attributeCollection)
            {
                var currentAttrModel = modelCollection.FirstOrDefault(model => model.Name.Equals(attribute.Name, StringComparison.InvariantCultureIgnoreCase));
                
                if (currentAttrModel == null)
                {
                    var operationResult = new AttributeOperationResult {AttributeShortName = attribute.Name};

                    operationResult.Errors.Add(new Error("", "No matching attribute model found for this attribute."));
                    operationResult.PerformedAction = ObjectAction.Exclude;

                    attributeOperationResultCollection.Add(operationResult);
                    attributesToPurge.Add(attribute);
                }
                else
                {
                    // Model is found. But if this is hierarchical attribute, we need to cotinue validation
                    if (attribute.IsHierarchical)
                    {
                        if (!currentAttrModel.IsHierarchical)
                        {
                            var operationResult = new AttributeOperationResult { AttributeShortName = attribute.Name };

                            operationResult.Errors.Add(new Error("", "Attribute is Hierarchical but current Model says it is not."));
                            operationResult.PerformedAction = ObjectAction.Exclude;

                            attributeOperationResultCollection.Add(operationResult);
                            attributesToPurge.Add(attribute);
                        }
                        else
                        {
                            //Get the Instances of the Hierarchal attribute
                            Dictionary<Int32, IAttributeCollection> sequenceToAttributeCollectionMap = GetHierarchyChildAttributes(attribute);

                            foreach (KeyValuePair<Int32, IAttributeCollection> sequenceToAttributeCollection in sequenceToAttributeCollectionMap)
                            {
                                if (sequenceToAttributeCollection.Value != null)
                                {
                                    ValidateHierarchicalAttribute(sequenceToAttributeCollection.Value, currentAttrModel.GetChildAttributeModels(), attributeOperationResultCollection, addMissingAttribues);
                        }
                    }
                }
            }
                }
            }

            //Remove invalid attributes from the attributes collection if any and return
            foreach (var attributeToPurge in attributesToPurge)
            {
                attributeCollection.Remove(attributeToPurge);
            }

            // Find all child attributes that are in the model but not in the attribute collection and add to it.
            if (addMissingAttribues)
            {
                foreach (var attributeModel in modelCollection)
                {
                    var currentAttribute = attributeCollection.FirstOrDefault(attribute => attribute.Name.Equals(attributeModel.Name, StringComparison.InvariantCultureIgnoreCase));

                    if (currentAttribute == null)
                    {
                        attributeCollection.Add(new Attribute(attributeModel));
                    }
                }
            }
        }

        /// <summary>
        /// Returns hierarchal attribute child instance records.
        /// </summary>
        /// <param name="hierarcyAttribute">The hierarchy attribute.</param>
        /// <returns><!--Dictionary<Int32, IAttributeCollection>--></returns>
        /// <exception cref="System.ArgumentException">Attribute is not a Hierarchal Attribute.</exception>
        /// <exception cref="System.IO.InvalidDataException">Hierarchal Attribute has more than one instance record</exception>
        private Dictionary<Int32, IAttributeCollection> GetHierarchyChildAttributes(Attribute hierarcyAttribute)
        {
          if (!hierarcyAttribute.IsHierarchical)
          {
            throw new ArgumentException("Attribute is not a Hierarchal Attribute.");
          }

            Dictionary<Int32, IAttributeCollection> result = new Dictionary<Int32, IAttributeCollection>();

          var childInstanceAttributes = hierarcyAttribute.GetChildAttributes();

          if (childInstanceAttributes.Any())
          {
                if (!hierarcyAttribute.IsCollection && childInstanceAttributes.Count > 1)
            {
              throw new InvalidDataException("Hierarchal Attribute has more than one instance record");
            }

                Int32 couner = 0;

                foreach (Attribute instanceAttribute in childInstanceAttributes)
            {
              //if child attributes are null the call GetChildAttributes throws the error
                    result.Add(couner++, instanceAttribute.GetChildAttributes());
            }
          }

            return result;
        }

        /// <summary>
        /// Returns Child (Complex or Hierarchical) attributes.
        /// </summary>
        /// <returns></returns>
        private IAttributeCollection GetComplexOrHierarchyChildAttributes()
        {
            if (this.IsHierarchical)
            {
                return GetHierarchicalChildAttributes();
            }

            if (this.IsComplex)
            {
                return GetComplexChildAttributes();
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        /// <param name="duplicateAttributeAllowed"></param>
        private void LoadAttributeForDataTransfer(String valuesAsXml, Boolean duplicateAttributeAllowed = false)
        {
            #region Sample Xml

            //<Attribute 
            //      Id="6588" 
            //      Name="VentureRetail" 
            //      LName="VentureRetail" 
            //      InstRefId="19549" 
            //      AttrParentId="3027" 
            //      AttrParentName="Vendor" 
            //      AttrParentLongName="Vendor" 
            //      IsCollection="True" 
            //      IsComplex="True" 
            //      IsLocalizable="True" 
            //      ApplyLocaleFormat="True" 
            //      ApplyTimeZoneConversion="False" 
            //      Precision="0" 
            //      AttrType="ComplexCollection" 
            //      AttrModelType="Common" 
            //      AttrDataType="Decimal" 
            //      SourceFlag="O" 
            //      Action="Read">
            //      <Values SourceFlag="Overridden">
            //        <Value Uom="" ValRefId="-1" Seq="0" Locale="en_WW"><![CDATA[19549]]></Value>
            //        <Value Uom="" ValRefId="-1" Seq="0" Locale="en_WW"><![CDATA[19550]]></Value>
            //      </Values>
            //      <Values SourceFlag="Inherited" />
            //     <Attributes>
            //       <Attribute Id="6588" Name="VentureRetail" LName="VentureRetail" InstRefId="19549" AttrParentId="3027" AttrParentName="Vendor" 
            //                  AttrParentLName="Vendor" IsCollection="False" IsComplex="True" IsLocalizable="True" ApplyLocaleFormat="True" 
            //                  ApplyTimeZoneConversion="False" Precision="0" AttributeTypeEnum="ComplexCollection" AttrModelType="Common" AttrDataType="Decimal" 
            //                  SourceFlag="O" Action="Read">
            //         <Values SourceFlag="Overridden">
            //           <Value Uom="" ValRefId="-1" Seq="0" Locale="en_WW"><![CDATA[19549]]></Value>
            //         </Values>
            //         <Values SourceFlag="Inherited" />
            //         <Attributes>
            //           <Attribute Id="6589" Name="Brand" LName="Brand" InstRefId="19549" AttrParentId="6588" AttrParentName="VentureRetail" 
            //                      AttrParentLName="Venture Retail" IsCollection="False" IsComplex="False" IsLocalizable="True" ApplyLocaleFormat="True" 
            //                      ApplyTimeZoneConversion="False" Precision="0" AttributeTypeEnum="Simple" AttrModelType="Common" AttrDataType="String" 
            //                      SourceFlag="O" Action="Read">
            //             <Values SourceFlag="Overridden">
            //               <Value Uom="" ValRefId="-1" Seq="1" Locale="en_WW"><![CDATA[Fingerhut]]></Value>
            //             </Values>
            //             <Values SourceFlag="Inherited" />
            //             <Attributes />
            //           </Attribute>
            //           <Attribute Id="6594" Name="Campaign" LName="Campaign" InstRefId="19549" AttrParentId="6588" AttrParentName="VentureRetail"
            //                      AttrParentLName="Venture Retail" IsCollection="False" IsComplex="False" IsLocalizable="True" ApplyLocaleFormat="True" 
            //                      ApplyTimeZoneConversion="False" Precision="0" AttributeTypeEnum="Simple" AttrModelType="Common" AttrDataType="String" 
            //                      SourceFlag="O" Action="Read">
            //             <Values SourceFlag="Overridden">
            //               <Value Uom="" ValRefId="-1" Seq="1" Locale="en_WW"><![CDATA[WEB]]></Value>
            //             </Values>
            //             <Values SourceFlag="Inherited" />
            //             <Attributes />
            //           </Attribute>
            //         </Attributes>
            //       </Attribute>
            //     </Attributes>
            //</Attribute>

            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Attribute")
                        {
                            #region Read Attribute Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = reader.ReadContentAsInt();
                                }

                                if (reader.MoveToAttribute("N"))
                                {
                                    this.Name = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("LN"))
                                {
                                    this.LongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("DT"))
                                {
                                    String strAttributeDataType = reader.ReadContentAsString();
                                    AttributeDataType attributeDataType = AttributeDataType.String;

                                    if (!String.IsNullOrWhiteSpace(strAttributeDataType))
                                        Enum.TryParse(strAttributeDataType, true, out attributeDataType);

                                    this.AttributeDataType = attributeDataType;
                                }

                                if (reader.MoveToAttribute("A"))
                                {
                                    this.Action = ValueTypeHelper.GetAction(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("L"))
                                {
                                    LocaleEnum locale = LocaleEnum.UnKnown;
                                    Enum.TryParse(reader.ReadContentAsString(), out locale);
                                    this.Locale = locale;
                                }

                                if (reader.MoveToAttribute("IRId"))
                                {
                                    Int32 refId = -1;
                                    Int32.TryParse(reader.ReadContentAsString(), out refId);
                                    this.InstanceRefId = refId;
                                }
                                if (reader.MoveToAttribute("Seq"))
                                {
                                    Decimal seq = -1;
                                    Decimal.TryParse(reader.ReadContentAsString(), out seq);
                                    this.Sequence = seq;
                                }
                                if (reader.MoveToAttribute("PId"))
                                {
                                    this.AttributeParentId = reader.ReadContentAsInt();
                                }

                                if (reader.MoveToAttribute("PN"))
                                {
                                    this.AttributeParentName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("PLN"))
                                {
                                    this.AttributeParentLongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Col"))
                                {
                                    this.IsCollection = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("Com"))
                                {
                                    this.IsComplex = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("LP"))
                                {
                                    this.IsLookup = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("RO"))
                                {
                                    this.ReadOnly = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("R"))
                                {
                                    this.Required = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("Loc"))
                                {
                                    this.IsLocalizable = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("ALF"))
                                {
                                    this.ApplyLocaleFormat = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("ATZC"))
                                {
                                    this.ApplyTimeZoneConversion = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("P"))
                                {
                                    this.Precision = reader.ReadContentAsInt();
                                }

                                if (reader.MoveToAttribute("IPA"))
                                {
                                    this.IsPrecisionArbitrary =
                                        ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("SF"))
                                {
                                    this.SourceFlag = Utility.GetSourceFlagEnum(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("HIOV"))
                                {
                                    HasInvalidOverriddenValues = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("HIIV"))
                                {
                                    HasInvalidInheritedValues = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("HIV"))
                                {
                                    HasInvalidValues = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (this.AttributeType == AttributeTypeEnum.Complex || this.AttributeType == AttributeTypeEnum.ComplexCollection)
                                {
                                    this.IsComplex = true;
                                }

                                // For Hierachical attribute and its Instance Record, DataType is Hierarchcal but AttributeType needs to be set.
                                // AttributeType for Hierarchical is always either Complex (or ComplexCollection) 
                                if (this.IsHierarchical)
                                {
                                    if (this.IsCollection)
                                    {
                                        this.AttributeType = AttributeTypeEnum.ComplexCollection;
                                    }
                                    else
                                    {
                                        this.AttributeType = AttributeTypeEnum.Complex;
                                    }
                                }
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Values")
                        {
                            #region Read values for current attribute

                            //Read the Value xml first before reading SourceFlag because "reader.MoveToAttribute" will move the curser forward and we won't get full xml after that.

                            if (reader.HasAttributes && reader.GetAttribute("SF") != null)
                            {
                                AttributeValueSource srcFlag = Utility.GetSourceFlagEnum(reader.GetAttribute("SF"));

                                //Get ValueCollection from Xml
                                String valueXml = reader.ReadOuterXml();
                                if (!String.IsNullOrWhiteSpace(valueXml))
                                {
                                    ValueCollection valCollection = new ValueCollection(valueXml, ObjectSerialization.DataTransfer);

                                    if (valCollection != null)
                                    {
                                        //Based on SourceFlag in Values node, populate ValueCollection in either inherited or overridden value collection
                                        switch (srcFlag)
                                        {
                                            case AttributeValueSource.Overridden:
                                                #region Populate Overridden values in object

                                                ObjectAction attributeAction = this.Action;

                                                foreach (Value val in valCollection)
                                                {
                                                    ObjectAction valueAction = val.Action;

                                                    this.AppendValue(val);

                                                    val.Action = valueAction;
                                                }

                                                this.Action = attributeAction;

                                                #endregion Populate Overridden values in object
                                                break;
                                            case AttributeValueSource.Inherited:
                                                #region Populate Inherited values in object

                                                attributeAction = this.Action;

                                                foreach (Value val in valCollection)
                                                {
                                                    ObjectAction valueAction = val.Action;

                                                    this.AppendInheritedValue(val);

                                                    val.Action = valueAction;
                                                }

                                                this.Action = attributeAction;

                                                #endregion Populate Inherited values in object
                                                break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //There are no attributes for 'values' element or Source Flag is not available..
                                //In this case, the Input Xml format will always be in 'External' serialization and having only overridden values
                                //Get ValueCollection from Xml
                                String valueXml = reader.ReadOuterXml();
                                if (!String.IsNullOrWhiteSpace(valueXml))
                                {
                                    ValueCollection valCollection = new ValueCollection(valueXml, ObjectSerialization.DataTransfer);

                                    if (valCollection != null)
                                    {
                                        ObjectAction attributeAction = this.Action;

                                        #region Populate Overridden values in object

                                        foreach (Value val in valCollection)
                                        {
                                            ObjectAction valueAction = val.Action;

                                            this.AppendValue(val);

                                            val.Action = valueAction;
                                        }

                                        this.Action = attributeAction;

                                        #endregion Populate Overridden values in object
                                    }
                                }
                            }

                            #endregion Read values for current attribute
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Attributes")
                        {
                            #region Read child attributes
                            //Child attributes are collection under current attribute.

                            String attributeXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(attributeXml))
                            {
                                //Get collection of child attributes and populate it in Attribute collection of current attribute object.
                                AttributeCollection attributeCollection = new AttributeCollection(attributeXml, duplicateAttributeAllowed, ObjectSerialization.DataTransfer);

                                if (attributeCollection != null)
                                {
                                    foreach (Attribute attr in attributeCollection)
                                    {
                                        // No need to use parent attribute locale when allowing multiple attribute objects of same id but their own locales.
                                        if (!duplicateAttributeAllowed)
                                        {
                                            attr.Locale = this.Locale;
                                        }

                                        this.Attributes.Add(attr, duplicateAttributeAllowed);
                                    }
                                }
                            }

                            #endregion Read child attributes
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
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void ValidateFirstLevelAttribute(String message)
        {
            if (!IsFirstLevelAttribute())
            {
                throw new InvalidOperationException(message);
            }
        }

        /// <summary>
        /// Check if attribute is level 1 complex attribute.
        /// </summary>
        /// <returns>True : If attribute is level 1 attribute. False otherwise </returns>
        private Boolean IsFirstLevelAttribute()
        {
            Boolean isLevel1Attribute = false;

            if (this.IsHierarchical)
            {
                return true;
            }
            else if (this.IsComplex == true)
            {
                // this => Level 1 attribute
                // this.Attributes => Level 2 attribute
                // this.Attributes[0].Attribtues => Level 3 attribute
                if (this.Attributes != null && this.Attributes.Count > 0 // Level 2
                    && this.Attributes.FirstOrDefault().Attributes != null && this.Attributes.FirstOrDefault().Attributes.Count > 0) // Level 3
                {
                    isLevel1Attribute = true;
                }
            }

            return isLevel1Attribute;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeModel"></param>
        /// <param name="attributeLocale"></param>
        private void CreateComplexInstanceRecordModel(AttributeModel attributeModel, LocaleEnum attributeLocale)
        {
            //Check for child attribute models..
            if (attributeModel.AttributeModels == null || attributeModel.AttributeModels.Count < 1)
            {
                if (attributeModel.IsHierarchical)
                {
                    throw new Exception(String.Format("Attribute Model Id:{0}- Name: {1} is improper. Attribute model type is hierarchical but child attribute models are not found.", attributeModel.Id, attributeModel.LongName));
                }
                else
                {
                    throw new Exception(String.Format("Attribute Model Id:{0}- Name: {1} is improper. Attribute model type is complex but child attribute models are not found.", attributeModel.Id, attributeModel.LongName));
                }
                
            }

            Attribute instanceRecord = new Attribute();

            //Read property by property and fill properties
            instanceRecord.Id = attributeModel.Id;
            instanceRecord.Name = attributeModel.Name + " Instance Record"; //Representation of instance record - '[Attribute Name] Instance Record' 
            instanceRecord.LongName = attributeModel.LongName + " Instance Record";
            instanceRecord.Locale = attributeLocale;
            instanceRecord.AttributeParentId = attributeModel.AttributeParentId;
            instanceRecord.AttributeParentName = attributeModel.AttributeParentName;
            instanceRecord.AttributeParentLongName = attributeModel.AttributeParentLongName;
            instanceRecord.AttributeType = AttributeTypeEnum.Complex; //Since this is an instance record attribute type is complex irrespective of whether attribute is collection or not
            instanceRecord.AttributeModelType = attributeModel.Context.AttributeModelType;
            Enum.TryParse<AttributeDataType>(attributeModel.AttributeDataTypeName, out instanceRecord._attributeDataType);
            instanceRecord.IsCollection = attributeModel.IsCollection;
            instanceRecord.IsComplex = attributeModel.IsComplex;
            instanceRecord.IsLookup = attributeModel.IsLookup;
            instanceRecord.IsLocalizable = attributeModel.IsLocalizable;
            instanceRecord.ApplyLocaleFormat = attributeModel.ApplyLocaleFormat;
            instanceRecord.ApplyTimeZoneConversion = attributeModel.ApplyTimeZoneConversion;
            instanceRecord.Precision = attributeModel.Precision;
            instanceRecord.IsPrecisionArbitrary = attributeModel.IsPrecisionArbitrary;

            instanceRecord.Action = this.Action;

            instanceRecord._attributes = new AttributeCollection();

            foreach (AttributeModel attrModel in attributeModel.AttributeModels)
            {
                Attribute attribute = new Attribute(attrModel, attributeLocale);
                instanceRecord._attributes.Add(attribute);
            }

            this.Attributes.Add(instanceRecord);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeModel"></param>
        /// <param name="attributeLocale"></param>
        private void CreateHierarchyInstanceRecordModel(AttributeModel attributeModel, LocaleEnum attributeLocale)
        {
            //Check for child attribute models..
            if (attributeModel.AttributeModels == null || attributeModel.AttributeModels.Count < 1)
            {
                throw new Exception(String.Format("Attribute Model Id:{0}- Name: {1} is improper. Attribute model type is complex but child attribute models are not found.", attributeModel.Id, attributeModel.LongName));
            }

            Attribute instanceRecord = new Attribute();

            //Read property by property and fill properties
            instanceRecord.Id = attributeModel.Id;
            instanceRecord.Name = attributeModel.Name + " Instance Record"; //Representation of instance record - '[Attribute Name] Instance Record' 
            instanceRecord.LongName = attributeModel.LongName + " Instance Record";
            instanceRecord.Locale = attributeLocale;
            instanceRecord.AttributeParentId = attributeModel.AttributeParentId;
            instanceRecord.AttributeParentName = attributeModel.AttributeParentName;
            instanceRecord.AttributeParentLongName = attributeModel.AttributeParentLongName;
            instanceRecord.AttributeType = AttributeTypeEnum.Complex; //Since this is an instance record attribute type is complex irrespective of whether attribute is collection or not
            instanceRecord.AttributeModelType = attributeModel.Context.AttributeModelType;
            Enum.TryParse<AttributeDataType>(attributeModel.AttributeDataTypeName, out instanceRecord._attributeDataType);
            instanceRecord.IsCollection = attributeModel.IsCollection;
            instanceRecord.IsComplex = attributeModel.IsComplex;
            instanceRecord.IsLookup = attributeModel.IsLookup;
            instanceRecord.IsLocalizable = attributeModel.IsLocalizable;
            instanceRecord.ApplyLocaleFormat = attributeModel.ApplyLocaleFormat;
            instanceRecord.ApplyTimeZoneConversion = attributeModel.ApplyTimeZoneConversion;
            instanceRecord.Precision = attributeModel.Precision;
            instanceRecord.IsPrecisionArbitrary = attributeModel.IsPrecisionArbitrary;

            instanceRecord.Action = this.Action;

            instanceRecord._attributes = new AttributeCollection();

            foreach (AttributeModel attrModel in attributeModel.AttributeModels)
            {
                Attribute attribute = new Attribute(attrModel, attributeLocale);
                instanceRecord._attributes.Add(attribute);
            }

            this.Attributes.Add(instanceRecord);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formattedValue"></param>
        /// <param name="formatLocale"></param>
        /// <returns></returns>
        private Object DeformatValue(Object formattedValue, LocaleEnum formatLocale)
        {
            /* *************************
             * Process flow:
             * *************************
             * 
             * newVal = null
             * 
             * if (this.DataType == Decimal)
             *      newVal = de-format val.AttrVal according to this.Locale
             * else if  (this.DataType == DateTime)
             *      newVal = de-format val.AttrVal according to this.Locale
             *
             * if (newVal != null)
             *      val.AttrVal = newVal
             *
             * val.Locale = this.Locale
             * 
             * this.CurrentValues.Add(val);
             */

            String deFormattedValue = String.Empty;

            if (formattedValue != null)
            {
                String formattedStringValue = formattedValue.ToString();

                if (!String.IsNullOrWhiteSpace(formattedStringValue))
                {
                    String currentCultureName = formatLocale.GetCultureName();

                    //Start with assumption that there is no format needed..
                    deFormattedValue = formattedStringValue;

                    if (this.AttributeDataType == Core.AttributeDataType.Decimal)
                    {
                        if(FormatHelper.IsProperDecimal(formattedStringValue, formatLocale))
                        {
                            Double dblValue = MDM.Core.FormatHelper.DeformatNumber(formattedStringValue, currentCultureName);
                            deFormattedValue = MDM.Core.FormatHelper.FormatNumber(dblValue, this.DataStorageCultureName, this.Precision, this.IsPrecisionArbitrary);
                        }
                        else
                        {
                            //In case of Import, import file reader is always reading in en_US number format even though attribute locale is not en_US.
                            //So, here we need to try one more time formatting the value for en_US locale
                            if (FormatHelper.IsProperDecimal(formattedStringValue, LocaleEnum.en_US))
                            {
                                Double dblValue = MDM.Core.FormatHelper.DeformatNumber(formattedStringValue, LocaleEnum.en_US.GetCultureName());
                                deFormattedValue = MDM.Core.FormatHelper.FormatNumber(dblValue, this.DataStorageCultureName, this.Precision, this.IsPrecisionArbitrary);
                            }
                            else
                            {
                                //TODO:: Commenting this throw as we need to cross check the flow once more...
                                //throw new Exception(String.Concat(this.LongName, " attribute value = ", formattedStringValue, " is not proper Decimal for locale = ", formatLocale));
                            }
                        }
                    }
                    else if (this.AttributeDataType == AttributeDataType.Date)
                    {
                        try
                        {
                            deFormattedValue = MDM.Core.FormatHelper.FormatDateOnly(formattedStringValue, currentCultureName, this.DataStorageCultureName);
                        }
                        catch
                        {
                            //TODO:: Commenting this throw as we need to cross check the flow once more...
                            //throw new Exception(String.Concat(this.LongName, " attribute value = ", formattedStringValue, " is not proper Date for locale = ", formatLocale));
                        }
                    }
                    else if (this.AttributeDataType == AttributeDataType.DateTime)
                    {
                        try
                        {
                            DateTime deformattedDate = DateTime.MinValue;
                            deformattedDate = MDM.Core.FormatHelper.DeformatDate(formattedStringValue, currentCultureName);
                            deFormattedValue = MDM.Core.FormatHelper.StoreDateTimeUtc(deformattedDate);
                        }
                        catch
                        {
                            //TODO:: Commenting this throw as we need to cross check the flow once more...
                            //throw new Exception(String.Concat(this.LongName, " attribute value = ", formattedStringValue, " is not proper DateTime for locale = ", formatLocale));
                        }
                    }
                }
            }
            return deFormattedValue;
        }

        /// <summary>
        ///  Overrides already existing value with new value.
        /// If current attribute's <see cref="SourceFlag"/> is Overridden then only it will save the value. Otherwise throws exception
        /// </summary>
        /// <param name="iValue">New value to set in overridden attribute collection.</param>
        /// <param name="formatLocale">Locale</param>
        /// <param name="applyDeFormatting"></param>
        /// <param name="isAppend"></param>
        /// <exception cref="ArgumentNullException">Value cannot be null</exception>
        /// <exception cref="InvalidSourceFlagException">Cannot change value when SourceFlag is Inherited.</exception>
        private void SetValue(IValue iValue, LocaleEnum formatLocale, Boolean applyDeFormatting, Boolean isAppend)
        {
            /* *************************
             * Process flow:
             * *************************
             * 
             * newVal = null
             * systemDataLocale = GetSystemDataLocale();
             * 
             * if (this.DataType == Decimal)
             *      newVal = de-format val.AttrVal according to systemDataLocale
             * else if  (this.DataType == DateTime)
             *      newVal = de-format val.AttrVal according to systemDataLocale
             *
             * if (newVal != null)
             *      val.AttrVal = newVal
             *
             * val.Locale = this.Locale
             * 
             * this.CurrentValues.Add(val);
             */

            if (iValue == null)
            {
                throw new ArgumentNullException("Value");
            }

            Value value = (Value)iValue;

            //Since the source flag is inherited before, so changing it to overridden and assigning value id as -1 to insert new overridden values.
            if (this.SourceFlag == AttributeValueSource.Inherited)
            {
                this.SourceFlag = AttributeValueSource.Overridden;
                value.Id = -1;
            }
            else
            {
                if (isAppend == false)
                {
                    //If we have existing value count as 1 then replace the value. Don't flush and create a new value object.
                    ValueCollection existingValues = this.OverriddenValues;

                    if (existingValues.Count == 1)
                    {
                        Value existingValue = existingValues.FirstOrDefault();
                        value.Id = existingValue.Id;
                    }
                }
            }

            if (isAppend == false)
            {
                this._overriddenValues.Clear();
            }

            if (value.Action != ObjectAction.Ignore && value.Action != ObjectAction.Delete)
            {
                value.Action = ObjectAction.Update;

                if (this.IsLookup && this.HasInvalidValues == false && value.ValueRefId < 0 && value.AttrVal != null)
                {
                    value.ValueRefId = ValueTypeHelper.Int32TryParse(value.AttrVal.ToString(), -1);
                }
            }

            if (applyDeFormatting == true)
            {
                //De-format value and save it.
                //value.AttrVal = this.DeformatValue(value.AttrVal, formatLocale);
                value.SetInVariantVal(this.DeformatValue(value.AttrVal, formatLocale));
            }
            else
            {
                value.SetInVariantVal(value.AttrVal);
            }

            this._overriddenValues.Add((Value)value);

            // Set attribute action to update.
            if (this.Action != ObjectAction.Ignore && this.Action != ObjectAction.Delete)
            {
                this.Action = ObjectAction.Update;
            }
        }

        /// <summary>
        /// Overrides already existing value with new values.
        /// If current attribute's <see cref="SourceFlag"/> is Overridden then only it will save the value. Otherwise throws exception
        /// </summary>
        /// <param name="valueCollection">New value to set in overridden attribute collection.</param>
        /// <param name="formatLocale"></param>
        /// <param name="applyDeFormatting"></param>
        /// <param name="isAppend"></param>
        /// <exception cref="ArgumentNullException">Value cannot be null</exception>
        /// <exception cref="InvalidSourceFlagException">Cannot change value when SourceFlag is Inherited.</exception>
        /// <exception cref="InvalidOperationException">Cannot add multiple values to collection if IsCollection is false</exception>
        private void SetValue(IValueCollection valueCollection, LocaleEnum formatLocale, Boolean applyDeFormatting, Boolean isAppend)
        {
            if (valueCollection == null)
            {
                throw new ArgumentNullException("ValueCollection");
            }

            Boolean isPreviousValueInherited = false;

            //Since the source flag is inherited before, so changing it to overridden.
            if (this.SourceFlag == AttributeValueSource.Inherited)
            {
                isPreviousValueInherited = true;
                this.SourceFlag = AttributeValueSource.Overridden;
            }

            ValueCollection values = (ValueCollection)valueCollection;

            if (values.Count > 1 && this.IsCollection == false)
            {
                throw new InvalidOperationException(string.Format("Multiple values found for a Non-Collection attribute with Name={0}, LongName={1} and Parent Name={2}", this.Name, this.LongName, this.AttributeParentName));
            }
            else
            {
                if (isAppend == false)
                {
                    this._overriddenValues.Clear();
                }

                isAppend = true;
                //
                foreach (Value value in valueCollection)
                {
                    //Since the source flag is inherited before and now changed to overridden, so assigning value id as -1 to insert new overridden values.
                    if (isPreviousValueInherited)
                    {
                        value.Id = -1;
                    }

                    this.SetValue(value, formatLocale, applyDeFormatting, isAppend);
                }

                //Set attribute action to update..
                if (this.Action != ObjectAction.Ignore && this.Action != ObjectAction.Delete)
                    this.Action = ObjectAction.Update;
            }
        }

        /// <summary>
        /// Overrides already existing value with new value.
        /// If current attribute's <see cref="SourceFlag"/> is Overridden then only it will save the value. Otherwise throws exception
        /// </summary>
        /// <param name="iValue">New value to set in overridden attribute collection.</param>
        /// <param name="formatLocale"></param>
        /// <param name="applyDeFormatting"></param>
        /// <param name="isAppend"></param>
        /// <exception cref="ArgumentNullException">Value cannot be null</exception>
        /// <exception cref="InvalidSourceFlagException">Cannot change value when SourceFlag is Inherited.</exception>
        private void SetInheritedValue(IValue iValue, LocaleEnum formatLocale, Boolean applyDeFormatting, Boolean isAppend)
        {
            if (iValue == null)
            {
                throw new ArgumentNullException("Value");
            }

            Value value = (Value)iValue;

            if (isAppend == false)
            {
                this._inheritedValues.Clear();
            }

            // Set value action to update..
            if (value.Action != ObjectAction.Ignore && value.Action != ObjectAction.Delete)
                value.Action = ObjectAction.Update;

            if (applyDeFormatting == true)
            {
                //De-format value and save it.
                value.SetInVariantVal(this.DeformatValue(value.AttrVal, formatLocale));
            }
            else
            {
                value.SetInVariantVal(value.AttrVal);
            }

            this._inheritedValues.Add(value);

            // Set attribute action to update.
            if (this.Action != ObjectAction.Ignore && this.Action != ObjectAction.Delete)
                this.Action = ObjectAction.Update;
        }

        /// <summary>
        /// Set Inherited values for an attirbute
        /// </summary>
        /// <param name="valueCollection">New values to set in inherited value collection.</param>
        /// <param name="formatLocale"></param>
        /// <param name="applyDeFormatting"></param>
        /// <param name="isAppend"></param>
        /// <exception cref="ArgumentNullException">Value cannot be null</exception>
        /// <exception cref="InvalidSourceFlagException">Cannot change value when SourceFlag is Inherited.</exception>
        private void SetInheritedValue(IValueCollection valueCollection, LocaleEnum formatLocale, Boolean applyDeFormatting, Boolean isAppend)
        {
            if (valueCollection == null)
            {
                throw new ArgumentNullException("ValueCollection");
            }

            if (isAppend == false)
            {
                this._inheritedValues.Clear();
            }

            isAppend = true;

            foreach (Value value in valueCollection)
            {
                this.SetInheritedValue(value, formatLocale, applyDeFormatting, isAppend);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notFormattedValue"></param>
        /// <param name="formatLocale"></param>
        /// <returns></returns>
        private Object FormatValue(Object notFormattedValue, LocaleEnum formatLocale)
        {
            /* *************************
             * Process flow:
             * *************************
             * 
             * newVal = null
             * 
             * if (this.DataType == Decimal)
             *      newVal = de-format val.AttrVal according to this.Locale
             * else if  (this.DataType == DateTime)
             *      newVal = de-format val.AttrVal according to this.Locale
             *
             * if (newVal != null)
             *      val.AttrVal = newVal
             *
             * val.Locale = this.Locale
             * 
             * this.CurrentValues.Add(val);
             */

            String formattedValue = String.Empty;

            if (notFormattedValue != null)
            {
                String notFormattedStringValue = notFormattedValue.ToString();

                if (!String.IsNullOrWhiteSpace(notFormattedStringValue))
                {
                    String currentCultureName = formatLocale.GetCultureName();

                    //Start with assumption that we dont need formatting
                    formattedValue = notFormattedStringValue;

                    if (this.AttributeDataType == Core.AttributeDataType.Decimal)
                    {
                        try
                        {
                            formattedValue = MDM.Core.FormatHelper.FormatNumber(notFormattedStringValue, currentCultureName, this.Precision, this.IsPrecisionArbitrary);
                        }
                        catch
                        {
                            //TODO:: Commenting this throw as we need to cross check the flow once more...
                            //throw new Exception(String.Concat(this.LongName, " attribute value = ", notFormattedStringValue, " is not proper decimal for locale = ", formatLocale));
                        }
                    }
                    else if (this.AttributeDataType == AttributeDataType.Date)
                    {
                        try
                        {
                            formattedValue = MDM.Core.FormatHelper.FormatDateOnly(notFormattedStringValue, this.DataStorageCultureName, currentCultureName);
                        }
                        catch
                        {
                            //TODO:: Commenting this throw as we need to cross check the flow once more...
                            //throw new Exception(String.Concat(this.LongName, " attribute value = ", notFormattedStringValue, " is not proper Date for locale = ", formatLocale));
                        }
                    }
                    else if (this.AttributeDataType == AttributeDataType.DateTime)
                    {
                        try
                        {
                            DateTime dateTime = MDM.Core.FormatHelper.ReadDateTimeUtc(notFormattedStringValue);
                            formattedValue = MDM.Core.FormatHelper.FormatDate(dateTime, currentCultureName);
                        }
                        catch
                        {
                            //TODO:: Commenting this throw as we need to cross check the flow once more...
                            //throw new Exception(String.Concat(this.LongName, " attribute value = ", notFormattedStringValue, " is not proper DateTime for locale = ", formatLocale));
                        }
                    }
                }
            }
            return formattedValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceFlag"></param>
        /// <param name="formatLocale"></param>
        /// <param name="applyFormatting"></param>
        /// <returns></returns>
        private IValueCollection GetValue(AttributeValueSource sourceFlag, LocaleEnum formatLocale, Boolean applyFormatting)
        {
            #region Process flow comments

            /* *************************
             * Process flow:
             * *************************
             * 
             * val = this.GetCurrentValue()
             * 
             * newVal = val.AttrVal
             * 
             * if (this.DataType == Decimal)
             *      newVal = format val.AttrVal according to this.Locale
             * else if  (this.DataType == DateTime)
             *      newVal = format val.AttrVal according to this.Locale
             *
             * if (newVal != null)
             *      val.AttrVal = newVal
             *
             * val.Locale = this.Locale
             * 
             * return val;
             * 
             */

            #endregion

            IValueCollection values = (IValueCollection)new ValueCollection();

            if (sourceFlag == AttributeValueSource.Inherited)
            {
                values = this._inheritedValues;
            }
            else
            {
                values = this._overriddenValues;
            }

            if (values != null)
            {
                foreach (Value val in values)
                {
                    if (applyFormatting == true)
                    {
                        if (val.InvariantVal != null && !String.IsNullOrWhiteSpace(val.InvariantVal.ToString()))
                        {
                            Object oldVal = new object();
                            //oldVal = this.FormatValue(val.OriginalVal, formatLocale);
                            oldVal = this.FormatValue(val.InvariantVal, formatLocale);
                            val.AttrVal = oldVal;
                        }
                    }
                    else
                    {
                        if (val.InvariantVal != null && !String.IsNullOrWhiteSpace(val.InvariantVal.ToString()))
                        {
                            val.AttrVal = val.InvariantVal;
                        }
                    }
                }
            }

            return values;
        }

        /// <summary>
        /// Compare 2 value collections
        /// </summary>
        /// <param name="values">Value collection to be compared with</param>
        /// <param name="valuesToBeCompared">Value collection to compare</param>
        /// <returns>True : If values are same. False : If values doesn't match</returns>
        private Boolean CompareValues(ValueCollection values, ValueCollection valuesToBeCompared)
        {
            foreach (Value val in values)
            {
                IValue iValue = valuesToBeCompared.GetBySequence(val.Sequence);

                if (iValue != null)
                {
                    Value subSetVal = (Value)iValue;

                    if (!val.Equals(subSetVal) || CompareExtendedValues(val.ExtendedValues, subSetVal.ExtendedValues) == false)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mergedAttribute"></param>
        /// <param name="deltaAttribute"></param>
        /// <param name="mergedValues"></param>
        /// <param name="deltaValues"></param>
        /// <param name="collectionStrategy"></param>
        /// <param name="stringComparison"></param>
        /// <param name="isSourceProcessingEnabled"></param>
        private void MergeValues(IAttribute mergedAttribute, IAttribute deltaAttribute, ValueCollection mergedValues, ValueCollection deltaValues, CollectionStrategy collectionStrategy, StringComparison stringComparison, Boolean isSourceProcessingEnabled = true)
        {
            if (mergedAttribute.IsCollection)
            {
                #region Collection attribute processing

                Boolean isDeltaChange = false;

                switch (collectionStrategy)
                {
                    case CollectionStrategy.Union:
                        isDeltaChange = UnionCollectionValues(mergedValues, deltaValues, deltaAttribute, stringComparison, isSourceProcessingEnabled);
                        UpdateAttribute(mergedAttribute, mergedValues, isDeltaChange);
                        break;
                    case CollectionStrategy.FlushAndFill:
                        isDeltaChange = FlushAndFillValueCollection(mergedValues, deltaValues, deltaAttribute, stringComparison, isSourceProcessingEnabled);
                        UpdateAttribute(mergedAttribute, mergedValues, isDeltaChange);
                        break;
                    case CollectionStrategy.Unknown:
                        // Do nothing in this case
                        break;
                }

                if (isDeltaChange && mergedAttribute.IsComplex && deltaAttribute.IsComplex)
                {
                    MergeComplexAttribute((Attribute)mergedAttribute, (Attribute)deltaAttribute);
                }

                #endregion
            }
            else
            {
                MergeValue((Attribute)mergedAttribute, (Attribute)deltaAttribute, stringComparison);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mergedAttribute"></param>
        /// <param name="deltaAttribute"></param>
        /// <param name="stringComparison"></param>
        private void MergeValue(Attribute mergedAttribute, Attribute deltaAttribute, StringComparison stringComparison)
        {
            Value origVal = (Value)mergedAttribute.GetCurrentValueInstanceInvariant();
            Value deltaVal = (Value)deltaAttribute.GetCurrentValueInstanceInvariant();

            if (origVal == null)
                origVal = new Value();

            if (deltaVal == null)
                deltaVal = new Value();

            Value mergedVal = origVal.Clone();

            if (mergedVal.ValueEquals(deltaVal, stringComparison))
            {
                if (mergedVal.SourceInfo == deltaVal.SourceInfo)
                {
                    mergedVal.Action = deltaVal.Action == ObjectAction.Delete ? ObjectAction.Delete : ObjectAction.Read;
                }
                else
                {
                    mergedVal.Action = ObjectAction.Update;
                }

            }
            else
            {
                var mergeValueId = mergedVal.Id;

                mergedVal = deltaVal.Clone();

                if (mergeValueId > 0)
                {
                    mergedVal.Id = mergeValueId;
                    mergedVal.Action = ObjectAction.Update;
                }

                else
                {
                    mergedVal.Id = -1;
                    mergedVal.Action = ObjectAction.Create;
                }
            }

            mergedVal.SourceInfo = (deltaVal.SourceInfo != null) ? (SourceInfo)deltaVal.SourceInfo.Clone() : null;

            if (mergedAttribute.SourceFlag == AttributeValueSource.Overridden)
            {
                ((Attribute)mergedAttribute)._overriddenValues.Clear();
                ((Attribute)mergedAttribute)._overriddenValues.Add(mergedVal);
            }
            else
            {
                ((Attribute)mergedAttribute)._inheritedValues.Clear();
                ((Attribute)mergedAttribute)._overriddenValues.Add(mergedVal);
            }

            MergeComplexAttribute(mergedAttribute, deltaAttribute);

            mergedAttribute.Action = mergedVal.Action;

            if (mergedVal.SourceInfo != null && !mergedVal.SourceInfo.SourceEntityId.HasValue)
            {
                mergedVal.SourceInfo.SourceEntityId = deltaAttribute.SourceEntityId;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mergedAttribute"></param>
        /// <param name="deltaAttribute"></param>
        private static void MergeComplexAttribute(Attribute mergedAttribute, Attribute deltaAttribute)
        {
            IValueCollection values = new ValueCollection(mergedAttribute.GetCurrentValues().Where(v => v.Action == ObjectAction.Delete).ToList());
            
            foreach (Value value in values)
            {
                mergedAttribute.RemoveValue(value);
            }

            if (deltaAttribute.Attributes != null && deltaAttribute.Attributes.Count > 0)
            {
                if (deltaAttribute.IsFirstLevelAttribute() && mergedAttribute.IsFirstLevelAttribute())
                {
                    mergedAttribute.Attributes = new AttributeCollection();
                    foreach (Attribute attribute in deltaAttribute.Attributes)
                    {
                        Attribute clonedAttribut = attribute.Clone();
                        clonedAttribut.Action = ObjectAction.Update;
                        mergedAttribute.Attributes.Add(clonedAttribut);
                    }
                    foreach (Attribute attribute in mergedAttribute.GetComplexChildAttributes())
                    {
                        attribute.Action = ObjectAction.Update;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mergedValueCollection"></param>
        /// <param name="deltaValueCollection"></param>
        /// <param name="deltaAttribute"></param>
        /// <param name="stringComparison"></param>
        /// <param name="isSourceProcessingEnabled"></param>
        /// <returns></returns>
        private Boolean UnionCollectionValues(ValueCollection mergedValueCollection, ValueCollection deltaValueCollection, IAttribute deltaAttribute, StringComparison stringComparison, Boolean isSourceProcessingEnabled)
        {
            return MergeCollections(mergedValueCollection, deltaValueCollection, deltaAttribute, stringComparison, isSourceProcessingEnabled);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mergedValueCollection"></param>
        /// <param name="deltaValueCollection"></param>
        /// <param name="deltaAttribute"></param>
        /// <param name="stringComparison"></param>
        /// <param name="isSourceProcessingEnabled"></param>
        /// <returns></returns>
        private Boolean FlushAndFillValueCollection(ValueCollection mergedValueCollection, ValueCollection deltaValueCollection, IAttribute deltaAttribute, StringComparison stringComparison, Boolean isSourceProcessingEnabled)
        {
            // No need to mark 'Action==Create' items for deleting...
            Value valueToRemoving = null;
            do
            {
                valueToRemoving = mergedValueCollection.FirstOrDefault(x => x.Action == ObjectAction.Create || x.Action == ObjectAction.Read);
                if (valueToRemoving != null)
                {
                    mergedValueCollection.Remove(valueToRemoving);
                }
            } while (valueToRemoving != null);

            mergedValueCollection.SetAction(ObjectAction.Delete);

            return MergeCollections(mergedValueCollection, deltaValueCollection, deltaAttribute, stringComparison, isSourceProcessingEnabled);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        private SourceInfo MergeSourceInfos(SourceInfo source, SourceInfo target)
        {
            // Final source resolving. Priority to target source
            if (source != null && target != null)
            {
                if (source.SourceId.HasValue && target.SourceId.HasValue)
                {
                    return target;
                }
                else
                {
                    if (source.SourceId.HasValue)
                    {
                        return source;
                    }
                    if (target.SourceId.HasValue)
                    {
                        return target;
                    }
                    return source;
                }
            }
            else
            {
                return source ?? target;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mergedValueCollection"></param>
        /// <param name="deltaValueCollection"></param>
        /// <param name="deltaAttribute"></param>
        /// <param name="stringComparison"></param>
        /// <param name="isSourceProcessingEnabled"></param>
        /// <returns></returns>
        private Boolean MergeCollections(ValueCollection mergedValueCollection, ValueCollection deltaValueCollection, IAttribute deltaAttribute, StringComparison stringComparison, Boolean isSourceProcessingEnabled)
        {
            Int64 valueId = -1;
            if (deltaValueCollection.Any())
            {
                valueId = deltaValueCollection.Min(x => x.Id);
                if (valueId >= 0)
                {
                    valueId = -1;
                }
                else
                {
                    valueId--;
                }
            }

            Boolean isDeltaChange = false;

            foreach (Value deltaValue in deltaValueCollection)
            {
                #region SourceInfo-invariant search of already existent value in collection

                Value matchedValue = null;
                SourceInfo deltaValueSrcInfoTmp = deltaValue.SourceInfo;
                try
                {
                    deltaValue.SourceInfo = null;
                    matchedValue = mergedValueCollection.FirstOrDefault(v =>
                    {
                        Boolean result = false;
                        SourceInfo vSrcInfoTmp = v.SourceInfo;
                        try
                        {
                            v.SourceInfo = null;
                            // Comparison without SourceInfo information taking into attention
                            result = v.ValueEquals(deltaValue, stringComparison);
                        }
                        finally
                        {
                            v.SourceInfo = vSrcInfoTmp;
                        }
                        return result;
                    });
                }
                finally
                {
                    deltaValue.SourceInfo = deltaValueSrcInfoTmp;
                }

                #endregion

                if (matchedValue != null)
                {
                    if (isSourceProcessingEnabled)
                    {
                    matchedValue.HasInvalidValue = deltaValue.HasInvalidValue;

                    SourceInfo resolvedSourceInfo = MergeSourceInfos(deltaValue.SourceInfo, matchedValue.SourceInfo);

                    matchedValue.SourceInfo = (resolvedSourceInfo != null) ? (SourceInfo)resolvedSourceInfo.Clone() : null;

                    if (matchedValue.Action != ObjectAction.Create)
                    {
                        matchedValue.Action = ObjectAction.Update;
                    }

                    isDeltaChange = true;
                    if (matchedValue.SourceInfo != null && !matchedValue.SourceInfo.SourceEntityId.HasValue)
                    {
                        matchedValue.SourceInfo.SourceEntityId = ((Attribute)deltaAttribute).SourceEntityId;
                    }
                }
                    else if (matchedValue.Action == ObjectAction.Delete)
                    {
                        matchedValue.Action = ObjectAction.Read;
                    }
                }
                else
                {
                    Value newValue = deltaValue.Clone();
                    newValue.Id = valueId--;
                    newValue.Action = ObjectAction.Create;
                    mergedValueCollection.Add(newValue);

                    if (newValue.SourceInfo != null && !newValue.SourceInfo.SourceEntityId.HasValue)
                    {
                        newValue.SourceInfo.SourceEntityId = ((Attribute)deltaAttribute).SourceEntityId;
                    }
                }
            }

            isDeltaChange |= ResequenceValues(mergedValueCollection);

            return isDeltaChange;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mergedAttribute"></param>
        /// <param name="mergedValues"></param>
        /// <param name="isDeltaChange"></param>
        private void UpdateAttribute(IAttribute mergedAttribute, ValueCollection mergedValues, Boolean isDeltaChange)
        {
            if (isDeltaChange)
            {
                mergedAttribute.Action = ObjectAction.Update;
            }
            else
            {
                // No need to update anithing is nothing is changed
                mergedAttribute.Action = ObjectAction.Read;
                return;
            }

            ((Attribute)mergedAttribute)._overriddenValues = mergedValues;
            
            //Since the source flag is inherited before, so changing it to overridden.
            if (mergedAttribute.SourceFlag == AttributeValueSource.Inherited)
            {
                mergedAttribute.SourceFlag = AttributeValueSource.Overridden;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueCollection"></param>
        /// <returns></returns>
        private Boolean ResequenceValues(ValueCollection valueCollection)
        {
            Boolean isDeltaChange = false;

            //Re-sequence values..
            Int32 seq = 0;
            foreach (Value val in valueCollection)
            {
                if (val.Action != ObjectAction.Delete && val.Action != ObjectAction.Ignore)
                {
                    if (val.Sequence != seq)
                    {
                        val.Sequence = seq++;
                        // Set action to Update as sequence changed..
                        if (val.Action == ObjectAction.Read)
                        {
                            val.Action = ObjectAction.Update;
                            isDeltaChange = true;
                        }
                    }
                    else
                    {
                        seq++;
                    }
                }
                
                if (val.Action == ObjectAction.Delete || val.Action == ObjectAction.Create)
                {
                    isDeltaChange = true;
                }
            }

            return isDeltaChange;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mergedAttribute"></param>
        /// <param name="deltaAttribute"></param>
        /// <param name="mergedValues"></param>
        /// <param name="deltaValues"></param>
        /// <param name="flushExistingValues"></param>
        /// <param name="stringComparison"></param>
        private void MergeValues(Attribute mergedAttribute, Attribute deltaAttribute, ValueCollection mergedValues, ValueCollection deltaValues, Boolean flushExistingValues, StringComparison stringComparison)
        {
            if (mergedAttribute.IsCollection)
            {
                #region Collection attribute

                Boolean isDeltaChange = false;

                //Commenting below line - Attribute Action will be set to update only in case of any changes, which is done in the below logic
                //mergedAttribute.Action = ObjectAction.Update; 

                if (flushExistingValues)
                    mergedValues.SetAction(ObjectAction.Delete);

                ValueCollection newValues = new ValueCollection();

                foreach (Value deltaVal in deltaValues)
                {
                    var matchedVals = mergedValues.Where(v => v.ValueEquals(deltaVal, stringComparison));

                    //If exact value matches..
                    if (matchedVals != null && matchedVals.Count() == 1)
                    {
                        Value matchedVal = matchedVals.First();
                        matchedVal.HasInvalidValue = deltaVal.HasInvalidValue;

                        //value exists but have been instructed to delete
                        if (deltaVal.Action == ObjectAction.Delete)
                        {
                            matchedVal.Action = ObjectAction.Delete;
                            isDeltaChange = true;
                        }
                        else if (deltaVal.Action == ObjectAction.Create ||
                                 deltaVal.Action == ObjectAction.Update ||
                                 flushExistingValues && matchedVal.Action == ObjectAction.Delete) //In case of flushExistingValues, we need to set matched value action as Update in order to avoid flushing of all values
                        {
                            if (flushExistingValues)
                            {
                                matchedVal.Action = ObjectAction.Update;
                                isDeltaChange = true;
                            }
                            else
                            {
                                matchedVal.Action = ObjectAction.Read;
                            }
                        }
                    }
                    else if (matchedVals != null && matchedVals.Count() > 1)
                    {
                        //TODO:: What to do for duplicate matches
                    }
                    else
                    {
                        // Don't add new value to attribute if it's not found in the original attribute values and marked for deletion
                        if (deltaVal.Action != ObjectAction.Delete)
                        {
                            newValues.Add(deltaVal);
                            isDeltaChange = true;
                        }
                    }
                }

                //Add all new values..
                Int32 valId = -1;
                foreach (Value newVal in newValues)
                {
                    newVal.Action = ObjectAction.Create;
                    newVal.Id = valId--; // Set -1, -2. -3 etc..
                    mergedValues.Add(newVal);
                }

                //Re-sequence values..
                IList<Value> sortedValues = mergedValues.OrderBy(v=>v.Sequence).ToList();

                Int32 seq = 0;
                foreach (Value val in sortedValues)
                {
                    if (val.Action != ObjectAction.Delete && val.Action != ObjectAction.Ignore)
                    {
                        if (val.Sequence != seq)
                        {
                            val.Sequence = seq++;
                            // Set action to Update as sequence changed..
                            if (val.Action == ObjectAction.Read)
                            {
                                val.Action = ObjectAction.Update;
                                isDeltaChange = true;
                            }
                        }
                        else
                        {
                            seq++;
                        }
                    }
                }

                if (isDeltaChange)
                {
                    mergedAttribute.Action = ObjectAction.Update;
                }

                #region Fix delete action at attribute level

                Boolean isAllValuesDeleted = true;
                Boolean isAnyValueDeleted = false;

                foreach (Value mergedValue in mergedValues)
                {
                    if (mergedValue.Action != ObjectAction.Delete)
                        isAllValuesDeleted = false;
                    else
                        isAnyValueDeleted = true;
                }

                if (isAnyValueDeleted)
                {
                    mergedAttribute.Action = ObjectAction.Update;
                }

                if (isAnyValueDeleted && isAllValuesDeleted)
                {
                    mergedValues.SetAction(ObjectAction.Delete);
                    mergedAttribute.Action = ObjectAction.Delete;
                }

                #endregion

                if (mergedAttribute.SourceFlag == AttributeValueSource.Overridden)
                {
                    mergedAttribute._overriddenValues = mergedValues;
                }
                else
                {
                    mergedAttribute._inheritedValues = mergedValues;
                }

                #endregion
            }
            else
            {
                #region Non-Collection attribute

                Value origVal = (Value)mergedAttribute.GetCurrentValueInstanceInvariant();
                Value deltaVal = (Value)deltaAttribute.GetCurrentValueInstanceInvariant();

                if (origVal == null)
                    origVal = new Value();

                if (deltaVal == null)
                    deltaVal = new Value();

                Value mergedVal = origVal.Clone();

                if (mergedVal.ValueEquals(deltaVal, stringComparison))
                {
                    if (deltaVal.Action == ObjectAction.Delete)
                        mergedVal.Action = ObjectAction.Delete;
                    else
                        mergedVal.Action = ObjectAction.Read;
                }
                else
                {
                    if (mergedVal.Id > 0)
                        mergedVal.Action = ObjectAction.Update;
                    else
                    {
                        mergedVal.Id = -1;
                        mergedVal.Action = ObjectAction.Create;
                    }

                    mergedVal.AttrVal = deltaVal.AttrVal;
                    mergedVal.InvariantVal = deltaVal.InvariantVal;
                    mergedVal.StringVal = deltaVal.StringVal;
                    mergedVal.DateVal = deltaVal.DateVal;
                    mergedVal.NumericVal = deltaVal.NumericVal;
                    mergedVal.ValueRefId = deltaVal.ValueRefId;
                    mergedVal.Locale = deltaVal.Locale;
                    mergedVal.Sequence = -1;
                    mergedVal.Uom = deltaVal.Uom;
                    mergedVal.UomId = deltaVal.UomId;
                    mergedVal.AuditRefId = deltaVal.AuditRefId;
                    mergedVal.ProgramName = deltaVal.ProgramName;
                    mergedVal.SetDisplayValue(deltaVal.GetDisplayValue());
                    mergedVal.HasInvalidValue = deltaVal.HasInvalidValue;
                    mergedVal.SourceInfo = deltaVal.SourceInfo;
                }

                if (mergedAttribute.SourceFlag == AttributeValueSource.Overridden)
                {
                    mergedAttribute._overriddenValues.Clear();
                    mergedAttribute._overriddenValues.Add(mergedVal);
                }
                else
                {
                    mergedAttribute._inheritedValues.Clear();
                    mergedAttribute._inheritedValues.Add(mergedVal);
                }

                mergedAttribute.Action = mergedVal.Action;

                #endregion
            }
        }

        /// <summary>
        /// Checks whether the attribute exists in the collection with same WSID(instancerefId)
        /// </summary>
        /// <param name="cxAttributeInstance">attribute</param>
        /// <returns>If exists- returns true else false</returns>
        private bool ComplexAttributeInstanceExists(Attribute cxAttributeInstance)
        {
            if (cxAttributeInstance != null && cxAttributeInstance.Id > 0)
            {
                IAttribute attributeExists = this.Attributes.GetAttribute(cxAttributeInstance.Id, cxAttributeInstance.InstanceRefId, cxAttributeInstance.Locale);
                if (attributeExists == null)
                    return false;
                else
                    return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 
        /// </summary>
        private void CheckIfComplexOrCollectionAttribute()
        {
            if (this.IsCollection && this.IsComplex)
            {
                throw new NotSupportedException(String.Format("{0} is complex collection attribute. Use Entity.GetAttributeAsComplexCollection() method to get the complex collection attribute and then use the appropriate method to get the respective value", this.Name));
            }
            else if (this.IsComplex)
            {
                throw new NotSupportedException(String.Format("{0} is complex attribute. Use Entity.GetAttributeAsComplex() method to get the complex attribute and then use the appropriate method to get the respective value", this.Name));
            }
            else if (this.IsCollection)
            {
                throw new NotSupportedException(String.Format("{0} is collection attribute. Use Entity.GetAttributeAsCollection() method to get the collection attribute and then use the appropriate method to get the respective value", this.Name));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="extendedValues"></param>
        /// <param name="extendedValuesToBeCompared"></param>
        /// <returns></returns>
        private Boolean CompareExtendedValues(Dictionary<String, String> extendedValues, Dictionary<String, String> extendedValuesToBeCompared)
        {
            Boolean isEqual = true;

            if (extendedValues != null)
            {
                if (extendedValuesToBeCompared == null)
                {
                    isEqual = false;
                }
                else
                {
                    isEqual = (extendedValues.Count == extendedValuesToBeCompared.Count);
                    if (isEqual)
                    {
                        foreach (KeyValuePair<String, String> extendedValue in extendedValues)
                        {
                            if (!extendedValuesToBeCompared.ContainsKey(extendedValue.Key) || (extendedValuesToBeCompared[extendedValue.Key] != extendedValue.Value))
                            {
                                isEqual = false;
                                break;
                            }
                        }
                    }
                }
            }
            else if (extendedValuesToBeCompared != null)
            {
                isEqual = false;
            }

            return isEqual;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        private void PopulateOverriddenAttributeValues(ValueCollection values)
        {
            ObjectAction attributeAction = this.Action;

            foreach (Value val in values)
            {
                ObjectAction valueAction = val.Action;

                this.AppendValue(val);

                val.Action = valueAction;
            }

            this.Action = attributeAction;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        private void PopulateInheritedAttributeValues(ValueCollection values)
        {
            ObjectAction attributeAction = this.Action;

            foreach (Value val in values)
            {
                ObjectAction valueAction = val.Action;

                this.AppendInheritedValue(val);

                val.Action = valueAction;
            }

            this.Action = attributeAction;
        }

        /// <summary>
        /// Loads properties of Attribute from xml
        /// </summary>
        /// <param name="reader">Indicates an xml reader used to read xml format data</param>
        private void LoadAttributeMetadataFromXml(XmlTextReader reader)
        {
            #region Read Attribute Properties

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

            if (reader.MoveToAttribute("Action"))
            {
                this.Action = ValueTypeHelper.GetAction(reader.ReadContentAsString());
            }

            if (reader.MoveToAttribute("Locale"))
            {
                LocaleEnum locale = LocaleEnum.UnKnown;
                Enum.TryParse(reader.ReadContentAsString(), out locale);
                this.Locale = locale;
            }

            if (reader.MoveToAttribute("InstanceRefId"))
            {
                Int32 refId = -1;
                Int32.TryParse(reader.ReadContentAsString(), out refId);
                this._instanceRefId = refId;
            }
            if (reader.MoveToAttribute("Sequence"))
            {
                Decimal seq = -1;
                Decimal.TryParse(reader.ReadContentAsString(), out seq);
                this._sequence = seq;
            }
            if (reader.MoveToAttribute("AttributeParentId"))
            {
                this._attributeParentId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._attributeParentId);
            }

            if (reader.MoveToAttribute("AttributeParentName"))
            {
                this._attributeParentName = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("AttributeParentLongName"))
            {
                this._attributeParentLongName = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("AttributeDataType"))
            {
                String strAttributeDataType = reader.ReadContentAsString();
                AttributeDataType attributeDataType = AttributeDataType.String;

                if (!String.IsNullOrWhiteSpace(strAttributeDataType))
                    Enum.TryParse(strAttributeDataType, true, out attributeDataType);

                this.AttributeDataType = attributeDataType;
            }

            if (reader.MoveToAttribute("IsCollection"))
            {
                this._isCollection = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
            }

            if (reader.MoveToAttribute("IsComplex"))
            {
                this._isComplex = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
            }

            if (reader.MoveToAttribute("IsLookup"))
            {
                this._isLookup = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
            }

            if (reader.MoveToAttribute("ReadOnly"))
            {
                this._readOnly = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
            }

            if (reader.MoveToAttribute("Required"))
            {
                this._required = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
            }

            if (reader.MoveToAttribute("AttributeTypeName"))
            {
                String attributeTypeName = reader.ReadContentAsString();
                //Here IsCollection is populated before this call. So be careful while moving the position of reading.
                this._attributeType = Utility.GetAttributeTypeFromAttributeTypeName(attributeTypeName, this.IsCollection);
                this._attributeModelType = Utility.GetAttributeModelTypeFromAttributeTypeName(attributeTypeName);
            }

            if (reader.MoveToAttribute("IsLocalizable"))
            {
                this._isLocalizable = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
            }

            if (reader.MoveToAttribute("ApplyLocaleFormat"))
            {
                this._applyLocaleFormat = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
            }

            if (reader.MoveToAttribute("ApplyTimeZoneConversion"))
            {
                this._applyTimeZoneConversion = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
            }

            if (reader.MoveToAttribute("Precision"))
            {
                this._precision = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._precision);
            }

            if (reader.MoveToAttribute("IsPrecisionArbitrary"))
            {
                this._isPrecisionArbitrary = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
            }

            if (reader.MoveToAttribute("SourceFlag"))
            {
                this._sourceFlag = Utility.GetSourceFlagEnum(reader.ReadContentAsString());
            }

            if (reader.MoveToAttribute("SourceEntityIdOverridden"))
            {
                this._sourceEntityIdOverridden = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
            }

            if (reader.MoveToAttribute("SourceClassOverridden"))
            {
                this._sourceClassOverridden = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
            }

            if (reader.MoveToAttribute("SourceEntityIdInherited"))
            {
                this._sourceEntityIdInherited = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
            }

            if (reader.MoveToAttribute("SourceClassInherited"))
            {
                this._sourceClassInherited = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
            }

            if (reader.MoveToAttribute("AttributeModelType"))
            {
                String strAttributeModelType = reader.ReadContentAsString();
                AttributeModelType attributeModelType = AttributeModelType.All;

                if (!String.IsNullOrWhiteSpace(strAttributeModelType))
                    Enum.TryParse<AttributeModelType>(strAttributeModelType, true, out attributeModelType);

                this._attributeModelType = attributeModelType;
            }

            if (reader.MoveToAttribute("AttributeType"))
            {
                String strAttributeType = reader.ReadContentAsString();
                AttributeTypeEnum attributeType = AttributeTypeEnum.Unknown;

                if (!String.IsNullOrWhiteSpace(strAttributeType))
                    Enum.TryParse(strAttributeType, true, out attributeType);

                this._attributeType = attributeType;
            }

            if (this.AttributeType == AttributeTypeEnum.Complex || this.AttributeType == AttributeTypeEnum.ComplexCollection)
            {
                this._isComplex = true;
            }

            #endregion
        }

        /// <summary>
        /// Converts properties (metadata) of Attribute object into xml
        /// </summary>
        /// <param name="xmlWriter">Indicates a writer to generate xml reperesentation of Attribute metadata</param>
        private void ConvertAttributeMetadataToXml(XmlTextWriter xmlWriter)
        {
            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("LongName", System.Net.WebUtility.HtmlEncode(this.LongName));
            xmlWriter.WriteAttributeString("InstanceRefId", this._instanceRefId.ToString());
            xmlWriter.WriteAttributeString("Sequence", this._sequence.ToString());
            xmlWriter.WriteAttributeString("AttributeParentId", this._attributeParentId.ToString());
            xmlWriter.WriteAttributeString("AttributeParentName", this._attributeParentName);
            xmlWriter.WriteAttributeString("AttributeParentLongName", this._attributeParentLongName);
            xmlWriter.WriteAttributeString("IsCollection", this._isCollection.ToString());
            xmlWriter.WriteAttributeString("IsComplex", this._isComplex.ToString());
            xmlWriter.WriteAttributeString("IsLookup", this._isLookup.ToString());
            xmlWriter.WriteAttributeString("IsLocalizable", this._isLocalizable.ToString());
            xmlWriter.WriteAttributeString("ReadOnly", this._readOnly.ToString());
            xmlWriter.WriteAttributeString("Required", this._required.ToString());
            xmlWriter.WriteAttributeString("ApplyLocaleFormat", this._applyLocaleFormat.ToString());
            xmlWriter.WriteAttributeString("ApplyTimeZoneConversion", this._applyTimeZoneConversion.ToString());
            xmlWriter.WriteAttributeString("Precision", this._precision.ToString());
            xmlWriter.WriteAttributeString("AttributeType", this._attributeType.ToString());
            xmlWriter.WriteAttributeString("AttributeModelType", this._attributeModelType.ToString());
            xmlWriter.WriteAttributeString("AttributeDataType", this._attributeDataType.ToString());
            xmlWriter.WriteAttributeString("SourceFlag", Utility.GetSourceFlagString(this._sourceFlag));
            xmlWriter.WriteAttributeString("SourceEntityId", this.SourceEntityId.ToString());
            xmlWriter.WriteAttributeString("SourceClass", this.SourceClass.ToString());
            xmlWriter.WriteAttributeString("SourceEntityIdOverridden", this._sourceEntityIdOverridden.ToString());
            xmlWriter.WriteAttributeString("SourceClassOverridden", this._sourceClassOverridden.ToString());
            xmlWriter.WriteAttributeString("SourceEntityIdInherited", this._sourceEntityIdInherited.ToString());
            xmlWriter.WriteAttributeString("SourceClassInherited", this._sourceClassInherited.ToString());
            xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
            xmlWriter.WriteAttributeString("Action", this.Action.ToString());
            xmlWriter.WriteAttributeString("HasInvalidOverriddenValues", this.HasInvalidOverriddenValues.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("HasInvalidInheritedValues", this.HasInvalidInheritedValues.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("IsPrecisionArbitrary", this._isPrecisionArbitrary.ToString().ToLowerInvariant());
        }

        /// <summary>
        /// Converts overridden attribute values into xml
        /// </summary>
        /// <param name="xmlWriter">Indicates a writer to generate xml reperesentation of values</param>
        private void ConvertOverriddenAttributeValuesToXml(XmlTextWriter xmlWriter)
        {
            //Overridden Values node start
            xmlWriter.WriteStartElement("Values");

            xmlWriter.WriteAttributeString("SourceFlag", Utility.GetSourceFlagString(Core.AttributeValueSource.Overridden));

            foreach (Value value in this.GetValue(AttributeValueSource.Overridden, this.Locale, true))
            {
                value.ConvertValueToXml(xmlWriter);
            }

            //Overridden Values node end
            xmlWriter.WriteEndElement();
        }

        /// <summary>
        /// Converts inherited attribute values into xml
        /// </summary>
        /// <param name="xmlWriter">Indicates a writer to generate xml reperesentation of values</param>
        private void ConvertInheritedAttributeValuesToXml(XmlTextWriter xmlWriter)
        {
            //Inherited Values node start
            xmlWriter.WriteStartElement("Values");

            xmlWriter.WriteAttributeString("SourceFlag", Utility.GetSourceFlagString(Core.AttributeValueSource.Inherited));

            foreach (Value value in this.GetValue(AttributeValueSource.Inherited, this.Locale, true))
            {
                value.ConvertValueToXml(xmlWriter);
            }

            //Inherited Values node end
            xmlWriter.WriteEndElement();
        }

        #endregion Private methods

        #endregion
    }
}