using System;
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
    /// 
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class AttributeModelLocaleProperties : MDMObject
    {
        #region Fields

        /// <summary>
        /// Field denoting AttributeParent LongName
        /// </summary>
        private String _attributeParentLongName = String.Empty;

        /// <summary>
        /// Field denoting  Attribute allowable values for attribute
        /// </summary>
        private String _allowableValues = null;

        /// <summary>
        /// Field denoting MaxLength of attribute value
        /// </summary>
        private Int32? _maxLength = null;

        /// <summary>
        /// Field denoting MinLength of attribute value
        /// </summary>
        private Int32? _minLength = null;

        /// <summary>
        /// Field denoting AllowableUOM for attribute value
        /// </summary>
        private String _allowableUOM = null;

        /// <summary>
        /// Field denoting DefaultUOM of attribute value
        /// </summary>
        private String _defaultUOM = null;

        /// <summary>
        /// Field denoting DefaultValue for attribute
        /// </summary>
        private String _defaultValue = null;

        /// <summary>
        /// Field denoting export mask for attribute value
        /// </summary>
        private String _exportMask = null;

        /// <summary>
        /// Field denoting child attribute models
        /// </summary>
        private AttributeModelLocalePropertiesCollection _childAttributeModelLocaleProperties = new AttributeModelLocalePropertiesCollection();

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

        #endregion

        #region Constructors
        #endregion

        #region Properties

        /// <summary>
        /// Property denoting AttributeParent LongName
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
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
        /// Property denoting  Attribute allowable values for attribute
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
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
        [ProtoMember(3)]
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
        [ProtoMember(4)]
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
        [ProtoMember(5)]
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
        [ProtoMember(6)]
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
        /// Property denoting DefaultValue for attribute
        /// </summary>
        [DataMember]
        [ProtoMember(7)]
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
        [ProtoMember(8)]
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
        /// Property denoting if the min value is inclusive
        /// </summary>
        [DataMember]
        [ProtoMember(9)]
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
        [ProtoMember(10)]
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
        [ProtoMember(11)]
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
        /// Property denoting if the max value is exclusive
        /// </summary>
        [DataMember]
        [ProtoMember(12)]
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
        /// Property denoting if the max value is exclusive
        /// </summary>
        [DataMember]
        [ProtoMember(13)]
        public AttributeModelLocalePropertiesCollection ChildAttributeModelLocaleProperties
        {
            get
            {
                return _childAttributeModelLocaleProperties;
            }
            set
            {
                _childAttributeModelLocaleProperties = value;
            }
        }

        /// <summary>
        /// Property denoting Definition of attribute
        /// </summary>
        [DataMember]
        [ProtoMember(14)]
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
        [ProtoMember(15)]
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
        [ProtoMember(16)]
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

        #endregion

        #region Methods

        #region Public Methods
        #endregion

        #region Private Methods
        #endregion

        #endregion
    }
}