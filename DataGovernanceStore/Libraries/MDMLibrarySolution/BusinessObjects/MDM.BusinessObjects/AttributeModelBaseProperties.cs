using System;
using System.ComponentModel;
using System.Xml;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using ProtoBuf;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the Attribute Model
    /// </summary>
    [DataContract]
    [ProtoContract]
    [KnownType(typeof(DependentAttributeCollection))]
    [KnownType(typeof(AttributeModelBasePropertiesCollection))]
    [KnownType(typeof(AttributeModelContext))]
    public class AttributeModelBaseProperties : MDMObject, IAttributeModelBaseProperties
    {
        #region Fields

        /// <summary>
        /// Field denoting AttributeParent Id
        /// </summary>
        private Int32 _attributeParentId = -1;

        /// <summary>
        /// Field denoting AttributeParent Name
        /// </summary>
        private String _attributeParentName = String.Empty;

        /// <summary>
        /// Field denoting AttributeParent LongName
        /// </summary>
        private String _attributeParentLongName = String.Empty;

        /// <summary>
        /// Field denoting AttributeType Id
        /// </summary>
        private Int32 _attributeTypeId = -1;

        /// <summary>
        /// Field denoting AttributeType name. This field is directly mapped to tb_AttributeType.
        /// After reading this property, <see cref="AttributeTypeEnum"/> and <see cref="AttributeModelType"/> will be populated (normalized one) with correct values
        /// </summary>
        private String _attributeTypeName = String.Empty;

        /// <summary>
        /// Field denoting the AttributeType. This property represents de-normalized version of attribute type information.
        /// For possible values see <see cref="MDM.Core.AttributeTypeEnum"/>
        /// </summary>
        private AttributeTypeEnum _attributeType = AttributeTypeEnum.Simple;

        /// <summary>
        /// 
        /// </summary>
        private AttributeModelType _attributeModelType = AttributeModelType.Common;

        /// <summary>
        /// Field denoting Attribute DataType Id
        /// </summary>
        private Int32 _attributeDataTypeId = -1;

        /// <summary>
        /// Field denoting Attribute DataType name
        /// </summary>
        private String _attributeDataTypeName = String.Empty;

        /// <summary>
        /// Field denoting Attribute display type Id
        /// </summary>
        private Int32 _attributeDisplayTypeId = -1;

        /// <summary>
        /// Field denoting  Attribute display type name
        /// </summary>
        private String _attributeDisplayTypeName = String.Empty;

        /// <summary>
        /// Field denoting  Attribute allowable values for attribute
        /// </summary>
        private String _allowableValues = String.Empty;

        /// <summary>
        /// Field denoting MaxLength of attribute value
        /// </summary>
        private Int32 _maxLength = 0;

        /// <summary>
        /// Field denoting MinLength of attribute value
        /// </summary>
        private Int32 _minLength = 0;

        /// <summary>
        /// Field denoting if attribute value is Required
        /// </summary>
        private Boolean _required = false;

        /// <summary>
        /// Field denoting AllowableUOM for attribute value
        /// </summary>
        private String _allowableUOM = String.Empty;

        /// <summary>
        /// Field denoting DefaultUOM of attribute value
        /// </summary>
        private String _defaultUOM = String.Empty;

        /// <summary>
        /// Field denoting UOMType of attribute value
        /// </summary>
        private String _uomType = String.Empty;

        /// <summary>
        /// Field denoting precision of attribute value
        /// </summary>
        private Int32 _precision = -1;

        /// <summary>
        /// Field denoting if attribute value  is collection
        /// </summary>
        private Boolean _isCollection = false;

        /// <summary>
        /// Field denoting RangeTo for attribute value
        /// </summary>
        private String _rangeTo = String.Empty;

        /// <summary>
        /// Field denoting RangeFrom for attribute value
        /// </summary>
        private String _rangeFrom = String.Empty;

        /// <summary>
        /// Field denoting Label of attribute
        /// </summary>
        private String _label = String.Empty;

        /// <summary>
        /// Field denoting Definition of attribute
        /// </summary>
        private String _definition = String.Empty;

        /// <summary>
        /// Field denoting Example of attribute
        /// </summary>
        private String _attributeExample = String.Empty;

        /// <summary>
        /// Field denoting BusinessRule for attribute
        /// </summary>
        private String _businessRule = String.Empty;

        /// <summary>
        /// Field denoting if attribute is read-only
        /// </summary>
        private Boolean _readOnly = false;

        /// <summary>
        /// Field denoting Extension of attribute
        /// </summary>
        private String _extension = String.Empty;

        /// <summary>
        /// Field denoting AttributeRegExp for attribute value
        /// </summary>
        private String _attributeRegEx = String.Empty;

        /// <summary>
        /// Field denoting Attribute Regular Expression error message for attribute value
        /// </summary>
        private String _regExErrorMessage = String.Empty;

        /// <summary>
        /// Field denoting LookUpTableName for attribute
        /// </summary>
        private String _lookUpTableName = String.Empty;

        /// <summary>
        /// Field denoting DefaultValue for attribute
        /// </summary>
        private String _defaultValue = String.Empty;

        /// <summary>
        /// Field denoting complex table name for attribute
        /// </summary> 
        private String _complexTableName = String.Empty;

        /// <summary>
        /// Field denoting complex table column names for the complex attribute
        /// </summary> 
        private Collection<String> _complexTableColumnNameList = new Collection<String>();

        /// <summary>
        /// Field denoting if attribute is complex child
        /// </summary> 
        private Boolean _isComplexChild = false;

        /// <summary>
        /// Field denoting Path of attribute
        /// </summary>
        private String _path = String.Empty;

        /// <summary>
        /// Field denoting if attribute id searchable
        /// </summary>
        private Boolean _searchable = true;
        
        /// <summary>
        /// Field denoting is history is to be kept for attribute value
        /// </summary>
        private Boolean _enableHistory = true;

        /// <summary>
        /// Field denoting WebUri for attribute
        /// </summary>
        private String _webUri = String.Empty;

        /// <summary>
        /// Field denoting list of columns for showing for lookup attribute
        /// </summary>
        private String _lkDisplayColumns = String.Empty;

        /// <summary>
        /// Field denoting sort order for lookup values in lookup attribute
        /// </summary>
        private String _lkSortOrder = String.Empty;

        /// <summary>
        /// Field denoting list of columns to search in for lookup attribute
        /// </summary>
        private String _lkSearchColumns = String.Empty;

        /// <summary>
        /// Field denoting display format of attribute value for lookup attribute
        /// </summary>
        private String _lkDisplayFormat = String.Empty;

        /// <summary>
        /// Field denoting sort order of attribute
        /// </summary>
        private Int32 _sortOrder = 0;

        /// <summary>
        /// Field denoting export mask for attribute value
        /// </summary>
        private String _exportMask = String.Empty;

        /// <summary>
        /// Field denoting if attribute is inheritable
        /// </summary>
        private Boolean _inheritable = false;

        /// <summary>
        /// Field denoting if attribute is hidden
        /// </summary>
        private Boolean _isHidden = false;

        /// <summary>
        /// Field denoting if attribute is a complex attribute
        /// </summary>
        private Boolean _isComplex = false;

        /// <summary>
        /// Field denoting if attribute is a Hierarchical attribute
        /// </summary>
        private Boolean _isHierarchical = false;

        /// <summary>
        /// Field denoting if attribute is Hierarchical child
        /// </summary> 
        private Boolean _isHierarchicalChild = false;

        /// <summary>
        /// Field denoting if attribute is inheritable only
        /// </summary> 
        private Boolean _inheritableOnly = false;

        /// <summary>
        /// Field denoting if attribute is auto promotable
        /// </summary> 
        private Boolean _autoPromotable = false;

        /// <summary>
        /// Field denoting if attribute is a lookup attribute
        /// </summary>
        private Boolean _isLookup = false;

        /// <summary>
        /// Field denoting if attribute is localizable 
        /// </summary>
        private Boolean _isLocalizable = true;

        /// <summary>
        /// Field denoting is locale specific format is to be applied to attribute value
        /// </summary>
        private Boolean _applyLocaleFormat = true;

        /// <summary>
        /// Field denoting is time zone conversion is to be applied to attribute value
        /// </summary>
        private Boolean _applyTimeZoneConversion = true;

        /// <summary>
        /// Field denoting if null values are searchable for attribute value
        /// </summary>
        private Boolean _allowNullSearch = true;

        /// <summary>
        /// Field denoting if attribute is to be shown at the time of item creation
        /// </summary>
        private Boolean _showAtCreation = false;

        /// <summary>
        /// Field denoting child attribute models
        /// </summary>
        private AttributeModelBasePropertiesCollection _childAttributeModelBaseProperties = new AttributeModelBasePropertiesCollection();

        /// <summary>
        /// Field denoting if the min value is inclusive
        /// </summary>
        private String _minInclusive;

        /// <summary>
        /// Field denoting if the min value is exclusive
        /// </summary>
        private String _minExclusive;

        /// <summary>
        /// Field denoting if the max value is inclusive
        /// </summary>
        private String _maxInclusive;

        /// <summary>
        /// Field denoting if the max value is exclusive
        /// </summary>
        private String _maxExclusive;

        /// <summary>
        /// Field denoting has dependent Attribute for the current attribute model.
        /// </summary>
        private Boolean _hasDependentAttribute = false;

        /// <summary>
        /// Field denoting Is dependent attribute for the current attribute model.
        /// </summary>
        private Boolean _isDependentAttribute = false;

        /// <summary>
        /// Field denoting if arbitrary precision is used for Decimal attributes.
        /// If IsPrecisionArbitrary is  set to true, then decimal value 12.2 with precision 3 will be stored as "12.2"
        /// If IsPrecisionArbitrary is  set to false, then decimal value 12.2 with precision 3 will be stored as "12.200"
        /// </summary>
        private Boolean _isPrecisionArbitrary = false;

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public AttributeModelBaseProperties()
            : base()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting AttributeParent Id
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        public Int32 AttributeParentId
        {
            get
            {
                return this._attributeParentId;
            }
            set
            {
                this._attributeParentId = value;
            }
        }

        /// <summary>
        /// Property denoting AttributeParent Name
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        public String AttributeParentName
        {
            get
            {
                return this._attributeParentName;
            }
            set
            {
                this._attributeParentName = value;
            }
        }

        /// <summary>
        /// Property denoting AttributeParent LongName
        /// </summary>
        [DataMember]
        [ProtoMember(3)]
        public String AttributeParentLongName
        {
            get
            {
                return this._attributeParentLongName;
            }
            set
            {
                this._attributeParentLongName = value;
            }
        }

        /// <summary>
        /// Property denoting AttributeType Id
        /// </summary>
        [DataMember]
        [ProtoMember(4)]
        public Int32 AttributeTypeId
        {
            get
            {
                return this._attributeTypeId;
            }
            set
            {
                this._attributeTypeId = value;
            }
        }

        /// <summary>
        /// Property denoting AttributeType name. This value is same as there in Tb_Attribute.AttributeTypeName which is coming from tb_AttributeType
        /// Using this property, we load proper values in <see cref="AttributeTypeEnum"/> and <see cref="AttributeModelType"/>
        /// </summary>
        [DataMember]
        [ProtoMember(5)]
        public String AttributeTypeName
        {
            get
            {
                return this._attributeTypeName;
            }
            set
            {
                this._attributeTypeName = value;
                this.AttributeType = Utility.GetAttributeTypeFromAttributeTypeName(this._attributeTypeName, this.IsCollection);
                this.AttributeModelType = Utility.GetAttributeModelTypeFromAttributeTypeName(this.AttributeTypeName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(6)]
        public AttributeModelType AttributeModelType
        {
            get
            {
                return this._attributeModelType;
            }
            set
            {
                this._attributeModelType = value;
                SetAttributeParentIdAndNameBasedOnModelType();
            }
        }

        /// <summary>
        /// Property denoting the AttributeType. This property represents de-normalized version of attribute type information.
        /// For possible values see <see cref="MDM.Core.AttributeTypeEnum"/>
        /// </summary>
        [DataMember]
        [ProtoMember(7)]
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
        /// Property denoting Attribute DataType Id
        /// </summary>
        [DataMember]
        [ProtoMember(8)]
        public Int32 AttributeDataTypeId
        {
            get
            {
                return this._attributeDataTypeId;
            }
            set
            {
                this._attributeDataTypeId = value;

                if (value == (Int32)AttributeDataType.Hierarchical)
                {
                    this._isHierarchical = true;
                }
            }
        }

        /// <summary>
        /// Property denoting Attribute DataType name
        /// </summary>
        [DataMember]
        [ProtoMember(9)]
        public String AttributeDataTypeName
        {
            get
            {
                return this._attributeDataTypeName;
            }
            set
            {
                this._attributeDataTypeName = value;

                SetAttributeDataTypeId();
            }
        }

        /// <summary>
        /// Field denoting Attribute display type Id
        /// </summary>
        [DataMember]
        [ProtoMember(10)]
        public Int32 AttributeDisplayTypeId
        {
            get
            {
                return _attributeDisplayTypeId;
            }
            set
            {
                _attributeDisplayTypeId = value;
            }
        }

        /// <summary>
        /// Property denoting  Attribute display type name
        /// </summary>
        [DataMember]
        [ProtoMember(11)]
        public String AttributeDisplayTypeName
        {
            get
            {
                return this._attributeDisplayTypeName;
            }
            set
            {
                this._attributeDisplayTypeName = value;

                SetAttributeDisplayTypedId();

                if (this._attributeDisplayTypeName.ToLower() == "lookuptable")
                    this._isLookup = true;
            }
        }

        /// <summary>
        /// Property denoting  Attribute allowable values for attribute
        /// </summary>
        [DataMember]
        [ProtoMember(12)]
        public String AllowableValues
        {
            get
            {
                return this._allowableValues;
            }
            set
            {
                this._allowableValues = value;
            }
        }

        /// <summary>
        /// Property denoting MaxLength of attribute value
        /// </summary>
        [DataMember]
        [ProtoMember(13)]
        public Int32 MaxLength
        {
            get
            {
                return this._maxLength;
            }
            set
            {
                this._maxLength = value;
            }
        }

        /// <summary>
        /// Property denoting MinLength of attribute value
        /// </summary>
        [DataMember]
        [ProtoMember(14)]
        public Int32 MinLength
        {
            get
            {
                return this._minLength;
            }
            set
            {
                this._minLength = value;
            }
        }

        /// <summary>
        /// Property denoting if attribute value is Required
        /// </summary>
        [DataMember]
        [ProtoMember(15)]
        public Boolean Required
        {
            get
            {
                return this._required;
            }
            set
            {
                this._required = value;
            }
        }

        /// <summary>
        /// Property denoting AllowableUOM for attribute value
        /// </summary>
        [DataMember]
        [ProtoMember(16)]
        public String AllowableUOM
        {
            get
            {
                return this._allowableUOM;
            }
            set
            {
                this._allowableUOM = value;
            }
        }

        /// <summary>
        /// Property denoting DefaultUOM of attribute value
        /// </summary>
        [DataMember]
        [ProtoMember(17)]
        public String DefaultUOM
        {
            get
            {
                return this._defaultUOM;
            }
            set
            {
                this._defaultUOM = value;
            }
        }

        /// <summary>
        /// Property denoting UOMType of attribute value
        /// </summary>
        [DataMember]
        [ProtoMember(18)]
        public String UomType
        {
            get
            {
                return this._uomType;
            }
            set
            {
                this._uomType = value;
            }
        }

        /// <summary>
        /// Property denoting precision of attribute value
        /// </summary>
        [DataMember]
        [ProtoMember(19), DefaultValue(-1)]
        public Int32 Precision
        {
            get
            {
                return this._precision;
            }
            set
            {
                this._precision = value;
            }
        }

        /// <summary>
        /// Property denoting if attribute value  is collection
        /// </summary>
        [DataMember]
        [ProtoMember(20)]
        public Boolean IsCollection
        {
            get
            {
                return this._isCollection;
            }
            set
            {
                this._isCollection = value;
            }
        }

        /// <summary>
        /// Property denoting RangeTo for attribute value
        /// </summary>
        [DataMember]
        [ProtoMember(21)]
        public String RangeTo
        {
            get
            {
                return this._rangeTo;
            }
            set
            {
                this._rangeTo = value;
            }
        }

        /// <summary>
        /// Property denoting RangeFrom for attribute value
        /// </summary>
        [DataMember]
        [ProtoMember(22)]
        public String RangeFrom
        {
            get
            {
                return this._rangeFrom;
            }
            set
            {
                this._rangeFrom = value;
            }
        }

        /// <summary>
        /// Property denoting Label of attribute
        /// </summary>
        [DataMember]
        [ProtoMember(23)]
        public String Label
        {
            get
            {
                return this._label;
            }
            set
            {
                this._label = value;
            }
        }

        /// <summary>
        /// Property denoting Definition of attribute
        /// </summary>
        [DataMember]
        [ProtoMember(24)]
        public String Definition
        {
            get
            {
                return this._definition;
            }
            set
            {
                this._definition = value;
            }
        }

        /// <summary>
        /// Property denoting Example of attribute
        /// </summary>
        [DataMember]
        [ProtoMember(25)]
        public String AttributeExample
        {
            get
            {
                return this._attributeExample;
            }
            set
            {
                this._attributeExample = value;
            }
        }

        /// <summary>
        /// Property denoting BusinessRule for attribute
        /// </summary>
        [DataMember]
        [ProtoMember(26)]
        public String BusinessRule
        {
            get
            {
                return this._businessRule;
            }
            set
            {
                this._businessRule = value;
            }
        }

        /// <summary>
        /// Property denoting if attribute is read-only
        /// </summary>
        [DataMember]
        [ProtoMember(27)]
        public Boolean ReadOnly
        {
            get
            {
                return this._readOnly;
            }
            set
            {
                this._readOnly = value;
            }
        }

        /// <summary>
        /// Property denoting Extension of attribute
        /// </summary>
        [DataMember]
        [ProtoMember(28)]
        public String Extension
        {
            get
            {
                return this._extension;
            }
            set
            {
                this._extension = value;
            }
        }

        /// <summary>
        /// Property denoting AttributeRegExp for attribute value
        /// </summary>
        [DataMember]
        [ProtoMember(31)]
        public String AttributeRegEx
        {
            get
            {
                return this._attributeRegEx;
            }
            set
            {
                this._attributeRegEx = value;
            }
        }

        /// <summary>
        /// Property denoting LookUpTableName for attribute
        /// </summary>
        [DataMember]
        [ProtoMember(33)]
        public String LookUpTableName
        {
            get
            {
                return this._lookUpTableName;
            }
            set
            {
                this._lookUpTableName = value;
            }
        }

        /// <summary>
        /// Property denoting DefaultValue for attribute
        /// </summary>
        [DataMember]
        [ProtoMember(34)]
        public String DefaultValue
        {
            get
            {
                return this._defaultValue;
            }
            set
            {
                this._defaultValue = value;
            }
        }

        /// <summary>
        /// Property denoting complex table name for attribute
        /// </summary>
        [DataMember]
        [ProtoMember(36)]
        public String ComplexTableName
        {
            get
            {
                return this._complexTableName;
            }
            set
            {
                this._complexTableName = value;
            }
        }

        /// <summary>
        /// Property denoting Path of attribute
        /// </summary>
        [DataMember]
        [ProtoMember(39)]
        public String Path
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
        /// Property denoting if attribute id searchable
        /// </summary>
        [DataMember]
        [ProtoMember(40)]
        public Boolean Searchable
        {
            get
            {
                return this._searchable;
            }
            set
            {
                this._searchable = value;
            }
        }

        /// <summary>
        /// Property denoting is history is to be kept for attribute value
        /// </summary>
        [DataMember]
        [ProtoMember(42)]
        public Boolean EnableHistory
        {
            get
            {
                return this._enableHistory;
            }
            set
            {
                this._enableHistory = value;
            }
        }

        /// <summary>
        /// Property denoting WebUri for attribute
        /// </summary>
        [DataMember]
        [ProtoMember(44)]
        public String WebUri
        {
            get
            {
                return this._webUri;
            }
            set
            {
                this._webUri = value;
            }
        }

        /// <summary>
        /// Property denoting list of columns for showing for lookup attribute
        /// </summary>
        [DataMember]
        [ProtoMember(50)]
        public String LkDisplayColumns
        {
            get
            {
                return this._lkDisplayColumns;
            }
            set
            {
                this._lkDisplayColumns = value;
            }
        }

        /// <summary>
        /// Property denoting sort order for lookup values in lookup attribute
        /// </summary>
        [DataMember]
        [ProtoMember(51)]
        public String LkSortOrder
        {
            get
            {
                return this._lkSortOrder;
            }
            set
            {
                this._lkSortOrder = value;
            }
        }

        /// <summary>
        /// Property denoting list of columns to search in for lookup attribute
        /// </summary>
        [DataMember]
        [ProtoMember(52)]
        public String LkSearchColumns
        {
            get
            {
                return this._lkSearchColumns;
            }
            set
            {
                this._lkSearchColumns = value;
            }
        }

        /// <summary>
        /// Property denoting display format of attribute value for lookup attribute
        /// </summary>
        [DataMember]
        [ProtoMember(53)]
        public String LkDisplayFormat
        {
            get
            {
                return this._lkDisplayFormat;
            }
            set
            {
                this._lkDisplayFormat = value;
            }
        }

        /// <summary>
        /// Property denoting sort order of attribute
        /// </summary>
        [DataMember]
        [ProtoMember(57)]
        public Int32 SortOrder
        {
            get
            {
                return _sortOrder;
            }
            set
            {
                _sortOrder = value;
            }
        }

        /// <summary>
        /// Property denoting export mask for attribute value
        /// </summary>
        [DataMember]
        [ProtoMember(58)]
        public String ExportMask
        {
            get
            {
                return _exportMask;
            }
            set
            {
                _exportMask = value;
            }
        }

        /// <summary>
        /// Property denoting if attribute is inheritable
        /// </summary>
        [DataMember]
        [ProtoMember(59)]
        public Boolean Inheritable
        {
            get
            {
                return _inheritable;
            }
            set
            {
                _inheritable = value;
            }
        }

        /// <summary>
        /// Property denoting if attribute is hidden
        /// </summary>
        [DataMember]
        [ProtoMember(60)]
        public Boolean IsHidden
        {
            get
            {
                return _isHidden;
            }
            set
            {
                _isHidden = value;
            }
        }

        /// <summary>
        /// Property denoting if attribute is a complex attribute
        /// </summary>
        [DataMember]
        [ProtoMember(61)]
        public Boolean IsComplex
        {
            get
            {
                return _isComplex;
            }
            set
            {
                _isComplex = value;
            }
        }

        /// <summary>
        /// Property denoting if attribute is a lookup attribute
        /// </summary>
        [DataMember]
        [ProtoMember(62)]
        public Boolean IsLookup
        {
            get
            {
                return _isLookup;
            }
            set
            {
                _isLookup = value;
            }
        }

        /// <summary>
        /// Property denoting if attribute is localizable 
        /// </summary>
        [DataMember]
        [ProtoMember(63), DefaultValue(true)]
        public Boolean IsLocalizable
        {
            get
            {
                return _isLocalizable;
            }
            set
            {
                _isLocalizable = value;
            }
        }

        /// <summary>
        /// Property denoting is locale specific format is to be applied to attribute value
        /// </summary>
        [DataMember]
        [ProtoMember(64), DefaultValue(true)]
        public Boolean ApplyLocaleFormat
        {
            get
            {
                return _applyLocaleFormat;
            }
            set
            {
                _applyLocaleFormat = value;
            }
        }

        /// <summary>
        /// Property denoting is time zone conversion is to be applied to attribute value
        /// </summary>
        [DataMember]
        [ProtoMember(65), DefaultValue(true)]
        public Boolean ApplyTimeZoneConversion
        {
            get
            {
                return _applyTimeZoneConversion;
            }
            set
            {
                _applyTimeZoneConversion = value;
            }
        }

        /// <summary>
        /// Property denoting if null values are searchable for attribute value
        /// </summary>
        [DataMember]
        [ProtoMember(66), DefaultValue(true)]
        public Boolean AllowNullSearch
        {
            get
            {
                return _allowNullSearch;
            }
            set
            {
                _allowNullSearch = value;
            }
        }

        /// <summary>
        /// Property denoting if attribute is to be shown at the time of item creation
        /// </summary>
        [DataMember]
        [ProtoMember(67)]
        public Boolean ShowAtCreation
        {
            get
            {
                return _showAtCreation;
            }
            set
            {
                _showAtCreation = value;
            }
        }

        /// <summary>
        /// Property denoting child attribute models
        /// </summary>
        [DataMember]
        [ProtoMember(68)]
        public AttributeModelBasePropertiesCollection ChildAttributeModelBaseProperties
        {
            get
            {
                return _childAttributeModelBaseProperties;
            }
            set
            {
                _childAttributeModelBaseProperties = value;
            }
        }

        /// <summary>
        /// Property denoting if the min value is inclusive
        /// </summary>
        [DataMember]
        [ProtoMember(69)]
        public String MinInclusive
        {
            get
            {
                return _minInclusive;
            }
            set
            {
                _minInclusive = value;
            }
        }

        /// <summary>
        /// Property denoting if the min value is exclusive
        /// </summary>
        [DataMember]
        [ProtoMember(70)]
        public String MinExclusive
        {
            get
            {
                return _minExclusive;
            }
            set
            {
                _minExclusive = value;
            }
        }

        /// <summary>
        /// Property denoting if the max value is inclusive
        /// </summary>
        [DataMember]
        [ProtoMember(71)]
        public String MaxInclusive
        {
            get
            {
                return _maxInclusive;
            }
            set
            {
                _maxInclusive = value;
            }
        }

        /// <summary>
        /// Property denotig if the max value is exclusive
        /// </summary>
        [DataMember]
        [ProtoMember(72)]
        public String MaxExclusive
        {
            get
            {
                return _maxExclusive;
            }
            set
            {
                _maxExclusive = value;
            }
        }

        /// <summary>
        /// Property denoting is the current attribute model depends on any other attribute model.
        /// if 'true' then current attribute model is dependent on other attribute model.
        /// </summary>
        [DataMember]
        [ProtoMember(73)]
        public Boolean IsDependentAttribute
        {
            get
            {
                return this._isDependentAttribute;
            }
            set
            {
                this._isDependentAttribute = value;
            }
        }

        /// <summary>
        /// Property denoting has any Dependent attributes for the current attribute model.
        /// </summary>
        [DataMember]
        [ProtoMember(74)]
        public Boolean HasDependentAttribute
        {
            get
            {
                return this._hasDependentAttribute;
            }
            set
            {
                this._hasDependentAttribute = value;
            }
        }

        /// <summary>
        /// Property denoting complex table column names for the complex attribute
        /// </summary>
        [DataMember]
        [ProtoMember(75)]
        public Collection<String> ComplexTableColumnNameList
        {
            get
            {
                return this._complexTableColumnNameList;
            }
            set
            {
                this._complexTableColumnNameList = value;
            }
        }

        /// <summary>
        /// Property denoting if attribute is complex child
        /// </summary>
        [DataMember]
        [ProtoMember(76)]
        public Boolean IsComplexChild
        {
            get
            {
                return this._isComplexChild;
            }
            set
            {
                this._isComplexChild = value;
            }
        }

        /// <summary>
        /// Property denoting if arbitrary precision is used for Decimal attributes.
        /// If IsPrecisionArbitrary is  set to true, then decimal value 12.2 with precision 3 will be stored as "12.2"
        /// If IsPrecisionArbitrary is  set to false, then decimal value 12.2 with precision 3 will be stored as "12.200"
        /// </summary>
        [DataMember]
        [ProtoMember(77)]
        public Boolean IsPrecisionArbitrary
        {
            get { return _isPrecisionArbitrary; }
            set { _isPrecisionArbitrary = value; }
        }

        /// <summary>
        /// Property denoting Attribute Regular Expression error message for attribute value
        /// </summary>
        [DataMember]
        [ProtoMember(78)]
        public String RegExErrorMessage
        {
            get
            {
                return this._regExErrorMessage;
            }
            set
            {
                this._regExErrorMessage = value;
            }
        }

        /// <summary>
        /// Property denoting if attribute is a Hierarchical attribute
        /// </summary>
        [DataMember]
        [ProtoMember(79)]
        public Boolean IsHierarchical
        {
            get
            {
                return _isHierarchical;
            }
            set
            {
                _isHierarchical = value;
            }
        }

        /// <summary>
        /// Property denoting if attribute is Hierarchical child
        /// </summary>
        [DataMember]
        [ProtoMember(80)]
        public Boolean IsHierarchicalChild
        {
            get
            {
                return this._isHierarchicalChild;
            }
            set
            {
                this._isHierarchicalChild = value;
            }
        }

        /// <summary>
        /// Property denoting if attribute is inheritable only
        /// </summary>
        [DataMember]
        [ProtoMember(81)]
        public Boolean InheritableOnly
        {
            get
            {
                return this._inheritableOnly;
            }
            set
            {
                this._inheritableOnly = value;
            }
        }

        /// <summary>
        /// Property denoting if attribute is auto promotable
        /// </summary>
        [DataMember]
        [ProtoMember(82)]
        public Boolean AutoPromotable
        {
            get
            {
                return this._autoPromotable;
            }
            set
            {
                this._autoPromotable = value;
            }
        }
        #endregion

        #region Methods

        #region Public Methods
        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        private void SetAttributeDataTypeId()
        {
            if (!String.IsNullOrWhiteSpace(this._attributeDataTypeName) && this._attributeDataTypeId < 1)
            {
                AttributeDataType attributeDataType = AttributeDataType.Unknown;
                Enum.TryParse(this._attributeDataTypeName, true, out attributeDataType);
                this.AttributeDataTypeId = (Int32)attributeDataType;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetAttributeDisplayTypedId()
        {
            if (!String.IsNullOrWhiteSpace(this._attributeDisplayTypeName) && this._attributeDisplayTypeId < 1)
            {
                AttributeDisplayType attributeDisplayType = AttributeDisplayType.Unknown;
                Enum.TryParse(this._attributeDisplayTypeName, true, out attributeDisplayType);
                this._attributeDisplayTypeId = (Int32)attributeDisplayType;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetAttributeParentIdAndNameBasedOnModelType()
        {
            switch (this._attributeModelType)
            {
                case AttributeModelType.CommonAttributeGroup:
                    if (String.IsNullOrEmpty(this._attributeParentName) || _attributeParentName.Equals("Common Attributes",StringComparison.InvariantCultureIgnoreCase))
                    {
                        this._attributeParentId = 14;
                        this._attributeParentName = "Common Attributes";
                        this._attributeTypeName = AttributeModelType.AttributeGroup.ToString();
                    }
                    break;
                case AttributeModelType.CategoryAttributeGroup:
                    if (String.IsNullOrEmpty(this._attributeParentName) || _attributeParentName.Equals("Category Specific", StringComparison.InvariantCultureIgnoreCase))
                    {
                        this._attributeParentId = 15;
                        this._attributeParentName = "Category Specific";
                        this._attributeTypeName = AttributeModelType.AttributeGroup.ToString();
                    }
                    break;
                case AttributeModelType.RelationshipAttributeGroup:
                    if (String.IsNullOrEmpty(this._attributeParentName) || _attributeParentName.Equals("Relationship Attributes", StringComparison.InvariantCultureIgnoreCase))
                    {
                        this._attributeParentId = 17;
                        this._attributeParentName = "Relationship Attributes";
                        this._attributeTypeName = AttributeModelType.AttributeGroup.ToString();
                    }
                    break;
            }
        }

        #endregion

        #endregion
    }
}