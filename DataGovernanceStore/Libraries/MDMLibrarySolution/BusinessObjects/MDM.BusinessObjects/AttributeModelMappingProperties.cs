using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

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
    public class AttributeModelMappingProperties : MDMObject
    {
        #region Fields

        /// <summary>
        /// Field denoting AttributeParent Id
        /// </summary>
        private Int32 _attributeParentId = -1;

        /// <summary>
        /// Field denoting sort order of attribute
        /// </summary>
        private Int32? _sortOrder = null;

        /// <summary>
        /// Field denoting if attribute value is Required
        /// </summary>
        private Boolean? _required = null;

        /// <summary>
        /// Field denoting if attribute is read-only
        /// </summary>
        private Boolean? _readOnly = null;

        /// <summary>
        /// Field denoting if attribute is to be shown at the time of item creation
        /// </summary>
        private Boolean? _showAtCreation = null;

        /// <summary>
        /// 
        /// </summary>
        private AttributeModelType _attributeModelType = AttributeModelType.Unknown;

        /// <summary>
        /// Field denoting  Attribute allowable values for attribute
        /// </summary>
        private String _allowableValues = null;

        /// <summary>
        /// Field denoting MinLength of attribute value
        /// </summary>
        private Int32? _minLength = null;

        /// <summary>
        /// Field denoting MaxLength of attribute value
        /// </summary>
        private Int32? _maxLength = null;

        /// <summary>
        /// Field denoting AllowableUOM for attribute value
        /// </summary>
        private String _allowableUOM = null;

        /// <summary>
        /// Field denoting DefaultUOM of attribute value
        /// </summary>
        private String _defaultUOM = null;

        /// <summary>
        /// Field denoting UOMType of attribute value
        /// </summary>
        private String _uomType = null;

        /// <summary>
        /// Field denoting precision of attribute value
        /// </summary>
        private Int32? _precision = null;

        /// <summary>
        /// Field denoting if the min value is inclusive
        /// </summary>
        private String _minInclusive = null;

        /// <summary>
        /// Field denoting if the min value is exclusive
        /// </summary>
        private String _minExclusive = null;

        /// <summary>
        /// Field denoting if the max value is inclusive
        /// </summary>
        private String _maxInclusive = null;

        /// <summary>
        /// Field denoting if the max value is exclusive
        /// </summary>
        private String _maxExclusive = null;

        /// <summary>
        /// Field denoting Definition of attribute
        /// </summary>
        private String _definition = null;

        /// <summary>
        /// Field denoting Example of attribute
        /// </summary>
        private String _attributeExample = null;

        /// <summary>
        /// Field denoting BusinessRule for attribute
        /// </summary>
        private String _businessRule = null;

        /// <summary>
        /// Field denoting DefaultValue for attribute
        /// </summary>
        private String _defaultValue = null;

        /// <summary>
        /// Field denoting export mask for attribute value
        /// </summary>
        private String _exportMask = null;

        /// <summary>
        /// Field denoting Extension of attribute
        /// </summary>
        private String _extension = null;

        /// <summary>
        /// Field denoting WebUri for attribute
        /// </summary>
        private String _webUri = null;

        /// <summary>
        /// 
        /// </summary>
        private AttributeModelContext _attributeModelContext = null;

        /// <summary>
        /// 
        /// </summary>
        private Boolean? _isSpecialized = null;

        /// <summary>
        /// Field denoting inheritable only for attribute
        /// </summary>
        private Boolean? _inheritableOnly = null;

        /// <summary>
        /// Field denoting auto promotable for attribute
        /// </summary>
        private Boolean? _autoPromotable = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public AttributeModelMappingProperties()
            : base()
        {
        }

        /// <summary>
        /// Constructor with attributeId, attributeParentId,  required, readOnly, showAtCreation, sortOrder, attributeModelType and isSpecialized 
        /// of attribute model mapping as input parameters
        /// </summary>
        /// <param name="attributeId">Indicates identifier of an attribute model</param>
        /// <param name="attributeParentId">Indicates identifier of attribute parent</param>
        /// <param name="required">Indicates if attribute is required for an attribute model</param>
        /// <param name="readOnly">Indicates if attribute value is read only of an attribute model</param>
        /// <param name="showAtCreation">Indicates show at creation of an attribute model</param>
        /// <param name="sortOrder">Indicates sort order property of an attribute model</param>
        /// <param name="inheritableOnly">Indicates the inheritable only property of an attribute model</param>
        /// <param name="autoPromotable">Indicates the auto promotable property of an attribute model</param>
        /// <param name="attributeModelType">Indicates type of attribute model</param>
        /// <param name="isSpecialized">Indicates if the attribute model mapping is overridded at current mapping level</param>
        public AttributeModelMappingProperties(Int32 attributeId, Int32 attributeParentId, Boolean? required, Boolean? readOnly, Boolean? showAtCreation, Int32? sortOrder, Boolean? inheritableOnly, 
                                                    Boolean? autoPromotable, AttributeModelType attributeModelType, Boolean? isSpecialized)
            : base(attributeId)
        {
            this._attributeParentId = attributeParentId;
            this._required = required;
            this._readOnly = readOnly;
            this._showAtCreation = showAtCreation;
            this._sortOrder = sortOrder;
            this._inheritableOnly = inheritableOnly;
            this._autoPromotable = autoPromotable;
            this._attributeModelType = attributeModelType;
            this._isSpecialized = isSpecialized;
        }

        /// <summary>
        /// Constructor with attributeId, attributeModel, and attributeModelType of attribute model mapping as input parameters
        /// </summary>
        /// <param name="attributeId">Indicates the identifier of an attribute model</param>
        /// <param name="attributeModel">Indicates an instance of attribute model</param>
        /// <param name="attributeModelType">Indicates the type of attribute model</param>
        public AttributeModelMappingProperties(Int32 attributeId, AttributeModel attributeModel, AttributeModelType attributeModelType)
            : base(attributeId)
        {
            //TODO: Need to set the value for inheritableOnly for this.
            this._allowableUOM = attributeModel.AllowableUOM;
            this._allowableValues = attributeModel.AllowableValues;
            this._attributeExample = attributeModel.AttributeExample;
            this._attributeParentId = attributeModel.AttributeParentId;
            this._businessRule = attributeModel.BusinessRule;
            this._defaultUOM = attributeModel.DefaultUOM;
            this._defaultValue = attributeModel.DefaultValue;
            this._definition = attributeModel.Definition;
            this._exportMask = attributeModel.ExportMask;
            this._extension = attributeModel.Extension;
            this._maxExclusive = attributeModel.MaxExclusive;
            this._maxInclusive = attributeModel.MaxInclusive;
            this._maxLength = attributeModel.MaxLength;
            this._minExclusive = attributeModel.MinExclusive;
            this._minInclusive = attributeModel.MinInclusive;
            this._minLength = attributeModel.MinLength;
            this._precision = attributeModel.Precision;
            this._readOnly = attributeModel.ReadOnly;
            this._required = attributeModel.Required;
            this._showAtCreation = attributeModel.ShowAtCreation;
            this._sortOrder = attributeModel.SortOrder;
            this._uomType = attributeModel.UomType;
            this._webUri = attributeModel.WebUri;
            this._inheritableOnly = attributeModel.InheritableOnly;
            this._autoPromotable = attributeModel.AutoPromotable;
            this._attributeModelType = attributeModelType;
        }


        /// <summary>
        /// Constructor with attributeId, attributeModel, and attributeModelType of attribute model mapping as input parameters
        /// </summary>
        /// <param name="attributeId">Indicates the identifier of an attribute model</param>
        /// <param name="categoryAttributeMapping">Indicates an instance of categoryAttributeMapping</param>
        /// <param name="attributeModelType">Indicates the type of attribute model</param>
        public AttributeModelMappingProperties(Int32 attributeId, CategoryAttributeMapping categoryAttributeMapping, AttributeModelType attributeModelType)
            : base(attributeId)
        {
            if (categoryAttributeMapping != null)
            {
                this._allowableUOM = categoryAttributeMapping.AllowableUOM;
                this._allowableValues = categoryAttributeMapping.AllowableValues;
                this._attributeExample = categoryAttributeMapping.AttributeExample;
                this._attributeParentId = categoryAttributeMapping.AttributeParentId;
                this._businessRule = categoryAttributeMapping.BusinessRule;
                this._defaultUOM = categoryAttributeMapping.DefaultUOM;
                this._defaultValue = categoryAttributeMapping.DefaultValue;
                this._definition = categoryAttributeMapping.Definition;
                this._exportMask = categoryAttributeMapping.ExportMask;
                this._maxExclusive = categoryAttributeMapping.MaxExclusive;
                this._maxInclusive = categoryAttributeMapping.MaxInclusive;
                this._maxLength = categoryAttributeMapping.MaxLength;
                this._minExclusive = categoryAttributeMapping.MinExclusive;
                this._minInclusive = categoryAttributeMapping.MinInclusive;
                this._minLength = categoryAttributeMapping.MinLength;
                this._precision = categoryAttributeMapping.Precision;
                this._readOnly = categoryAttributeMapping.ReadOnly;
                this._required = categoryAttributeMapping.Required;
                this._showAtCreation = categoryAttributeMapping.ShowAtCreation;
                this._sortOrder = categoryAttributeMapping.SortOrder;
                this._inheritableOnly = categoryAttributeMapping.InheritableOnly;
                this._autoPromotable = categoryAttributeMapping.AutoPromotable;
                this._attributeModelType = attributeModelType;
            }
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
        /// Property denoting sort order of attribute
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        public Int32? SortOrder
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
        /// Property denoting if attribute value is Required
        /// </summary>
        [DataMember]
        [ProtoMember(3)]
        public Boolean? Required
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
        /// Property denoting if attribute is read-only
        /// </summary>
        [DataMember]
        [ProtoMember(4)]
        public Boolean? ReadOnly
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
        /// Property denoting if attribute is to be shown at the time of item creation
        /// </summary>
        [DataMember]
        [ProtoMember(5)]
        public Boolean? ShowAtCreation
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
            }
        }

        /// <summary>
        /// Property denoting  Attribute allowable values for attribute
        /// </summary>
        [DataMember]
        [ProtoMember(7)]
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
        [ProtoMember(8)]
        public Int32? MaxLength
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
        [ProtoMember(9)]
        public Int32? MinLength
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
        /// Property denoting AllowableUOM for attribute value
        /// </summary>
        [DataMember]
        [ProtoMember(10)]
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
        [ProtoMember(11)]
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
        [ProtoMember(12)]
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
        [ProtoMember(13)]
        public Int32? Precision
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
        /// Property denoting if the min value is inclusive
        /// </summary>
        [DataMember]
        [ProtoMember(14)]
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
        [ProtoMember(15)]
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
        [ProtoMember(16)]
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
        [ProtoMember(17)]
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
        /// Property denoting Definition of attribute
        /// </summary>
        [DataMember]
        [ProtoMember(18)]
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
        [ProtoMember(19)]
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
        [ProtoMember(20)]
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
        /// Property denoting DefaultValue for attribute
        /// </summary>
        [DataMember]
        [ProtoMember(21)]
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
        /// Property denoting export mask for attribute value
        /// </summary>
        [DataMember]
        [ProtoMember(22)]
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
        /// Property denoting Extension of attribute
        /// </summary>
        [DataMember]
        [ProtoMember(23)]
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
        /// Property denoting WebUri for attribute
        /// </summary>
        [DataMember]
        [ProtoMember(24)]
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
        /// 
        /// </summary>
        public AttributeModelContext AttributeModelContext
        {
            get
            {
                return this._attributeModelContext;
            }
            set
            {
                this._attributeModelContext = value;
            }
        }

        /// <summary>
        /// Property denoting if attribute is read-only
        /// </summary>
        [DataMember]
        [ProtoMember(25)]
        public Boolean? IsSpecialized
        {
            get
            {
                return this._isSpecialized;
            }
            set
            {
                this._isSpecialized = value;
            }
        }

        /// <summary>
        /// Property denoting if attribute is inheritable only or not
        /// </summary>
        [DataMember]
        [ProtoMember(26)]
        public Boolean? InheritableOnly
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
        /// Property denoting if attribute is auto promotable or not
        /// </summary>
        [DataMember]
        [ProtoMember(27)]
        public Boolean? AutoPromotable
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

        /// <summary>
        /// Determines whether specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">AttributeModelMappingProperties object which needs to be compared.</param>
        /// <returns>Result of the comparison in Boolean.</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj is AttributeModelMappingProperties)
            {
                AttributeModelMappingProperties objectToBeCompared = obj as AttributeModelMappingProperties;

                if (this.SortOrder != objectToBeCompared.SortOrder)
                {
                    return false;
                }

                if (this.Required != objectToBeCompared.Required)
                {
                    return false;
                }

                if (this.ReadOnly != objectToBeCompared.ReadOnly)
                {
                    return false;
                }

                if (this.ShowAtCreation != objectToBeCompared.ShowAtCreation)
                {
                    return false;
                }

                if (this.AllowableValues != objectToBeCompared.AllowableValues)
                {
                    return false;
                }

                if (this.MaxLength != objectToBeCompared.MaxLength)
                {
                    return false;
                }

                if (this.MinLength != objectToBeCompared.MinLength)
                {
                    return false;
                }

                if (this.AllowableUOM != objectToBeCompared.AllowableUOM)
                {
                    return false;
                }

                if (this.DefaultUOM != objectToBeCompared.DefaultUOM)
                {
                    return false;
                }

                if (this.UomType != objectToBeCompared.UomType)
                {
                    return false;
                }

                if (this.Precision != objectToBeCompared.Precision)
                {
                    return false;
                }

                if (this.MinInclusive != objectToBeCompared.MinInclusive)
                {
                    return false;
                }

                if (this.MinExclusive != objectToBeCompared.MinExclusive)
                {
                    return false;
                }

                if (this.MaxInclusive != objectToBeCompared.MaxInclusive)
                {
                    return false;
                }

                if (this.MaxExclusive != objectToBeCompared.MaxExclusive)
                {
                    return false;
                }

                if (this.Definition != objectToBeCompared.Definition)
                {
                    return false;
                }

                if (this.AttributeExample != objectToBeCompared.AttributeExample)
                {
                    return false;
                }

                if (this.BusinessRule != objectToBeCompared.BusinessRule)
                {
                    return false;
                }

                if (this.DefaultValue != objectToBeCompared.DefaultValue)
                {
                    return false;
                }

                if (this.ExportMask != objectToBeCompared.ExportMask)
                {
                    return false;
                }

                if (this.Extension != objectToBeCompared.Extension)
                {
                    return false;
                }

                if (this.WebUri != objectToBeCompared.WebUri)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the hash code for this instance
        /// </summary>
        /// <returns>Hash code of the instance</returns>
        public override Int32 GetHashCode()
        {
            return this.SortOrder.GetHashCode() ^ this.Required.GetHashCode() ^ this.ReadOnly.GetHashCode() ^ this.ShowAtCreation.GetHashCode() ^
                   this.AllowableValues.GetHashCode() ^ this.MaxLength.GetHashCode() ^ this.MinLength.GetHashCode() ^ this.AllowableUOM.GetHashCode() ^
                   this.DefaultUOM.GetHashCode() ^ this.UomType.GetHashCode() ^ this.Precision.GetHashCode() ^ this.MinInclusive.GetHashCode() ^
                   this.MinExclusive.GetHashCode() ^ this.MaxInclusive.GetHashCode() ^ this.MaxExclusive.GetHashCode() ^ this.Definition.GetHashCode() ^
                   this.AttributeExample.GetHashCode() ^ this.BusinessRule.GetHashCode() ^ this.DefaultValue.GetHashCode() ^ this.ExportMask.GetHashCode() ^
                   this.Extension.GetHashCode() ^ this.WebUri.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeModelMappingProperties"></param>
        /// <param name="compareIds"></param>
        /// <returns></returns>
        public Boolean IsSuperSetOf(AttributeModelMappingProperties attributeModelMappingProperties, Boolean compareIds = false)
        {
            if (this.SortOrder != attributeModelMappingProperties.SortOrder)
            {
                return false;
            }

            if (this.Required != attributeModelMappingProperties.Required)
            {
                return false;
            }

            if (this.ReadOnly != attributeModelMappingProperties.ReadOnly)
            {
                return false;
            }

            if (this.ShowAtCreation != attributeModelMappingProperties.ShowAtCreation)
            {
                return false;
            }

            if (this.AllowableValues != attributeModelMappingProperties.AllowableValues)
            {
                return false;
            }

            if (this.MaxLength != attributeModelMappingProperties.MaxLength)
            {
                return false;
            }

            if (this.MinLength != attributeModelMappingProperties.MinLength)
            {
                return false;
            }

            if (this.AllowableUOM != attributeModelMappingProperties.AllowableUOM)
            {
                return false;
            }

            if (this.DefaultUOM != attributeModelMappingProperties.DefaultUOM)
            {
                return false;
            }

            if (this.UomType != attributeModelMappingProperties.UomType)
            {
                return false;
            }

            if (this.Precision != attributeModelMappingProperties.Precision)
            {
                return false;
            }

            if (this.MinInclusive != attributeModelMappingProperties.MinInclusive)
            {
                return false;
            }

            if (this.MinExclusive != attributeModelMappingProperties.MinExclusive)
            {
                return false;
            }

            if (this.MaxInclusive != attributeModelMappingProperties.MaxInclusive)
            {
                return false;
            }

            if (this.MaxExclusive != attributeModelMappingProperties.MaxExclusive)
            {
                return false;
            }

            if (this.Definition != attributeModelMappingProperties.Definition)
            {
                return false;
            }

            if (this.AttributeExample != attributeModelMappingProperties.AttributeExample)
            {
                return false;
            }

            if (this.BusinessRule != attributeModelMappingProperties.BusinessRule)
            {
                return false;
            }

            if (this.DefaultValue != attributeModelMappingProperties.DefaultValue)
            {
                return false;
            }

            if (this.ExportMask != attributeModelMappingProperties.ExportMask)
            {
                return false;
            }

            if (this.Extension != attributeModelMappingProperties.Extension)
            {
                return false;
            }

            if (this.WebUri != attributeModelMappingProperties.WebUri)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Clone attributeModel mapping properties object
        /// </summary>
        /// <returns>
        /// Cloned copy of attributeModel mapping properties  object.
        /// </returns>
        public AttributeModelMappingProperties Clone()
        {
            AttributeModelMappingProperties clonedAttributeModelProperties = new AttributeModelMappingProperties();

            clonedAttributeModelProperties.SortOrder = this.SortOrder;
            clonedAttributeModelProperties.Required = this.Required;
            clonedAttributeModelProperties.ReadOnly = this.ReadOnly;
            clonedAttributeModelProperties.ShowAtCreation = this.ShowAtCreation;
            clonedAttributeModelProperties.AllowableValues = this.AllowableValues;
            clonedAttributeModelProperties.MaxLength = this.MaxLength;
            clonedAttributeModelProperties.MinLength = this.MinLength;
            clonedAttributeModelProperties.AllowableUOM = this.AllowableUOM;
            clonedAttributeModelProperties.DefaultUOM = this.DefaultUOM;
            clonedAttributeModelProperties.UomType = this.UomType;
            clonedAttributeModelProperties.Precision = this.Precision;
            clonedAttributeModelProperties.MinInclusive = this.MinInclusive;
            clonedAttributeModelProperties.MinExclusive = this.MinExclusive;
            clonedAttributeModelProperties.MaxInclusive = this.MaxInclusive;
            clonedAttributeModelProperties.MaxExclusive = this.MaxExclusive;
            clonedAttributeModelProperties.Definition = this.Definition;
            clonedAttributeModelProperties.AttributeExample = this.AttributeExample;
            clonedAttributeModelProperties.BusinessRule = this.BusinessRule;
            clonedAttributeModelProperties.DefaultValue = this.DefaultValue;
            clonedAttributeModelProperties.ExportMask = this.ExportMask;
            clonedAttributeModelProperties.Extension = this.Extension;
            clonedAttributeModelProperties.WebUri = this.WebUri;

            return clonedAttributeModelProperties;
        }

        #endregion

        #region Private Methods
        #endregion

        #endregion
    }
}