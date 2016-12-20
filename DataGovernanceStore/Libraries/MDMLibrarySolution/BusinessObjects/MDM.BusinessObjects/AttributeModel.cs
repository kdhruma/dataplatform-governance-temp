using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

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
    [KnownType(typeof(MDMObject))]
    [KnownType(typeof(ObjectAction))]
    [KnownType(typeof(AttributeModelBaseProperties))]
    [KnownType(typeof(AttributeModelBasePropertiesCollection))]
    [KnownType(typeof(AttributeModelMappingProperties))]
    [KnownType(typeof(AttributeModelMappingPropertiesCollection))]
    [KnownType(typeof(AttributeModelLocaleProperties))]
    [KnownType(typeof(AttributeModelLocalePropertiesCollection))]
    public class AttributeModel : MDMObject, IAttributeModel, IDataModelObject
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Order = 0)]
        [ProtoMember(1)]
        private AttributeModelBaseProperties _attributeModelBaseProperties = new AttributeModelBaseProperties();

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Order = 1)]
        [ProtoMember(2)]
        private AttributeModelMappingProperties _attributeModelMappingProperties = new AttributeModelMappingProperties();

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Order = 2)]
        [ProtoMember(3)]
        private AttributeModelLocaleProperties _attributeModelLocaleProperties = new AttributeModelLocaleProperties();

        /// <summary>
        /// Field denoting permission set for the current attribute model.
        /// </summary>
        [DataMember(Order = 100)]
        [ProtoMember(4)]
        private Collection<UserAction> _permissionSet = new Collection<UserAction>();

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Order = 100)]
        [ProtoMember(5)]
        private Dictionary<String, String> _overridenProperties = null;

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Order = 100)]
        [ProtoMember(6)]
        private AttributeModelContext _context = new AttributeModelContext();

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Order = 100)]
        [ProtoMember(7)]
        private AttributeModelCollection _attributeModels = new AttributeModelCollection();

        /// <summary>
        /// Field denoting 'dependent on attributes' for the current attribute model.
        /// Collection of parent dependent attributes.
        /// </summary>
        [DataMember(Order = 100)]
        [ProtoMember(8)]
        private DependentAttributeCollection _dependentParentAttributes = new DependentAttributeCollection();

        /// <summary>
        /// Field denoting 'dependent attributes' for the current attribute model.
        /// Collection for children dependent attributes.
        /// </summary>
        [DataMember(Order = 100)]
        [ProtoMember(9)]
        private DependentAttributeCollection _dependentChildAttributes = new DependentAttributeCollection();

        /// <summary>
        /// Field Denoting the original attribute model
        /// </summary>
        private AttributeModel _originalAttributeModel = null;

        /// <summary>
        /// Field denoting if attribute has locale properties.
        /// </summary>
        private Boolean _hasLocaleProperties = false;

        #region IDataModelObject Fields

        /// <summary>
        /// Field denoting attribute model key External Id from external source.
        /// </summary>
        private String _externalId = String.Empty;

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter-less Constructor
        /// </summary>
        public AttributeModel()
            : base()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Identity of an Attribute Model</param>
        public AttributeModel(Int32 id)
        {
            this._attributeModelBaseProperties.Id = id;
        }

        /// <summary>
        /// Constructor with Id, Name and Description of an Attribute Model as input parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of an Attribute Model</param>
        /// <param name="name">Indicates the Name of an Attribute Model</param>
        /// <param name="longName">Indicates the Description of an Attribute Model</param>
        public AttributeModel(Int32 id, String name, String longName)
        {
            this._attributeModelBaseProperties.Id = id;
            this._attributeModelBaseProperties.Name = name;
            this._attributeModelBaseProperties.LongName = longName;
        }

        /// <summary>
        /// Constructor with Id, Name, LongName and Locale of an Attribute Model as input parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of an Attribute Model</param>
        /// <param name="name">Indicates the Name of an Attribute Model</param>
        /// <param name="longName">Indicates the LongName of an Attribute Model</param>
        /// <param name="locale">Indicates the Locale of an Attribute Model</param>
        public AttributeModel(Int32 id, String name, String longName, LocaleEnum locale)
        {
            this._attributeModelBaseProperties.Id = id;
            this._attributeModelBaseProperties.Name = name;
            this._attributeModelBaseProperties.LongName = longName;
            this.Locale = locale;
        }

        /// <summary>
        ///  Constructor with Id, Name, LongName,Attribute Parent Name , ShowAtCreation,Required,ReadOnly and SortOrder of an Attribute Model as input parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of an Attribute Model</param>
        /// <param name="name">Indicates the Name of an Attribute Model</param>
        /// <param name="longName">Indicates the LongName of an Attribute Model</param>
        /// <param name="attributeParentName">Indicates the Attribute Parent Name of an Attribute Model</param>
        /// <param name="showAtCreation">Indicates the Show at creation of an Attribute Model</param>
        /// <param name="required">Indicates the if attribute value is required for an Attribute Model</param>
        /// <param name="isReadOnly">Indicates the if attribute value is read-only of an Attribute Model</param>
        /// <param name="sortorder">Indicates the SortOrder of an Attribute Model</param>
        public AttributeModel(Int32 id, String name, String longName, String attributeParentName, Boolean showAtCreation, Boolean required, Boolean isReadOnly, Int32 sortorder)
        {
            this._attributeModelBaseProperties.Id = id;
            this._attributeModelBaseProperties.Name = name;
            this._attributeModelBaseProperties.LongName = longName;
            this._attributeModelBaseProperties.AttributeParentName = attributeParentName;
            this._attributeModelBaseProperties.ShowAtCreation = showAtCreation;
            this._attributeModelBaseProperties.Required = required;
            this._attributeModelBaseProperties.ReadOnly = isReadOnly;
            this._attributeModelBaseProperties.SortOrder = sortorder;
        }

        /// <summary>
        /// Constructor with Id, AttributeName,AttributeParent , AttributeDataTypeName and AttributeDisplay TypeName of an Attribute Model as input parameters
        /// </summary>
        /// <param name="Id">Indicates the Identity of an Attribute Model</param>
        /// <param name="attributeLongName">Indicates the Attribute long name of an Attribute Model</param>
        /// <param name="attributeParent">Indicates the Attribute Parent name of an Attribute Model</param>
        /// <param name="attributeDataTypeName">Indicates the Attribute data type name of an Attribute Model</param>
        /// <param name="attributeDisplayTypeName">Indicates the attribute display type name of an Attribute Model</param>
        public AttributeModel(Int32 Id, String attributeLongName, String attributeParent, String attributeDataTypeName, String attributeDisplayTypeName)
        {
            this._attributeModelBaseProperties.Id = Id;
            this._attributeModelBaseProperties.LongName = attributeLongName;
            this._attributeModelBaseProperties.AttributeParentName = attributeParent;
            this._attributeModelBaseProperties.AttributeDataTypeName = attributeDataTypeName;
            this._attributeModelBaseProperties.AttributeDisplayTypeName = attributeDisplayTypeName;
        }

        /// <summary>
        /// Constructor with context and attribute model xml
        /// </summary>
        /// <param name="containerId">Container id for the attribute model</param>
        /// <param name="entityTypeId">Entity type id for the attribute model</param>
        /// <param name="categoryId">Category id for the attribute model</param>
        /// <param name="locale">Locale for the attribute model</param>
        /// <param name="attributeModelType">Type of the attribute model</param>
        /// <param name="attributeModelXml">Attribute Model XML</param>
        /// <param name="loadCompleteDetailsOfAttribute">Flag denoting whether to load complete details or only the main details of the attribute model</param>
        public AttributeModel(Int32 containerId, Int32 entityTypeId, Int64 categoryId, LocaleEnum locale, AttributeModelType attributeModelType, String attributeModelXml, Boolean loadCompleteDetailsOfAttribute)
        {
            if (this.Context == null)
                this.Context = new AttributeModelContext();

            LoadAttributeModelFromXml(attributeModelXml, loadCompleteDetailsOfAttribute);

            this.Context.ContainerId = containerId;
            this.Context.EntityTypeId = entityTypeId;
            this.Context.CategoryId = categoryId;
            this.Context.Locales.Add(locale);

            if (attributeModelType == AttributeModelType.All)
            {
                this.Context.AttributeModelType = Utility.GetAttributeModelTypeFromAttributeTypeName(this.AttributeTypeName);
            }
            else
            {
                this.Context.AttributeModelType = attributeModelType;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeModel" /> class.
        /// </summary>
        /// <param name="valuesAsXml">The values as XML.</param>
        /// <param name="loadCompleteDetailsOfAttribute">if set to <c>true</c> [load complete details of attribute].</param>
        public AttributeModel(String valuesAsXml, Boolean loadCompleteDetailsOfAttribute = true)
        {
            this.LoadAttributeModelFromXml(valuesAsXml, loadCompleteDetailsOfAttribute);
        }

        /// <summary>
        /// </summary>
        /// <param name="attributeModelBaseProperties"></param>
        /// <param name="attributeModelMappingProperties"></param>
        /// <param name="attributeModelLocaleProperties"></param>
        public AttributeModel(AttributeModelBaseProperties attributeModelBaseProperties, AttributeModelMappingProperties attributeModelMappingProperties, AttributeModelLocaleProperties attributeModelLocaleProperties)
            : this(attributeModelBaseProperties, attributeModelMappingProperties, attributeModelLocaleProperties, LocaleEnum.UnKnown)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeModelBaseProperties"></param>
        /// <param name="attributeModelMappingProperties"></param>
        /// <param name="attributeModelLocaleProperties"></param>
        /// <param name="locale"></param>
        public AttributeModel(AttributeModelBaseProperties attributeModelBaseProperties, AttributeModelMappingProperties attributeModelMappingProperties, AttributeModelLocaleProperties attributeModelLocaleProperties, LocaleEnum locale)
        {
            AttributeModelBasePropertiesCollection childAttributeModelBaseProperties = null;
            AttributeModelLocalePropertiesCollection childAttributeModelLocaleProperties = null;

            if (attributeModelBaseProperties != null)
            {
                this._attributeModelBaseProperties = attributeModelBaseProperties;
                childAttributeModelBaseProperties = attributeModelBaseProperties.ChildAttributeModelBaseProperties;

                if (locale == LocaleEnum.UnKnown)
                {
                    locale = attributeModelBaseProperties.Locale;
                }

                base.Locale = locale;

                if (this.Context == null)
                {
                    this.Context = new AttributeModelContext();
                }

                this.Context.AttributeModelType = Utility.GetAttributeModelTypeFromAttributeTypeName(this._attributeModelBaseProperties.AttributeTypeName);
            }

            if (attributeModelMappingProperties != null)
            {
                this._attributeModelMappingProperties = attributeModelMappingProperties;
            }

            if (attributeModelLocaleProperties != null)
            {
                this._attributeModelLocaleProperties = attributeModelLocaleProperties;
                base.Locale = attributeModelLocaleProperties.Locale;
                _hasLocaleProperties = true;

                childAttributeModelLocaleProperties = attributeModelLocaleProperties.ChildAttributeModelLocaleProperties;
            }

            if (childAttributeModelBaseProperties != null && childAttributeModelBaseProperties.Count > 0)
            {
                this._attributeModels = ConstructChildAttributeModels(childAttributeModelBaseProperties, childAttributeModelLocaleProperties, locale);
            }

            this.Context.AttributeModelType = Utility.GetAttributeModelTypeFromAttributeTypeName(this.AttributeTypeName);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Indicates the Id of an object
        /// </summary>
        public new Int32 Id
        {
            get
            {
                if (this._overridenProperties != null && this._overridenProperties.ContainsKey("Id"))
                    return GetOverridenProperties<Int32>("Id");
                else
                    return this._attributeModelBaseProperties.Id;
            }
            set
            {
                SetOverridenProperties<Int32>("Id", value);
            }
        }

        /// <summary>
        /// Indicates the Name of an object. Name often refers to the ShortName
        /// </summary>
        public new String Name
        {
            get
            {
                return this._attributeModelBaseProperties.Name;
            }
            set { }
        }

        /// <summary>
        /// Indicates the Long Name of an object
        /// </summary>
        public new String LongName
        {
            get
            {
                if (this._attributeModelLocaleProperties != null && !String.IsNullOrWhiteSpace(this._attributeModelLocaleProperties.LongName))
                    return this._attributeModelLocaleProperties.LongName;
                else
                    return this._attributeModelBaseProperties.LongName;
            }
            set { }
        }

        /// <summary>
        /// Indicates the Name of an object in lower case.
        /// </summary>
        public new String NameInLowerCase
        {
            get
            {
                return this._attributeModelBaseProperties.NameInLowerCase;
            }
        }

        /// <summary>
        /// Property for AuditrefId of an Object
        /// </summary>
        public new Int64 AuditRefId
        {
            get { return this._attributeModelBaseProperties.AuditRefId; }
            set { }
        }

        /// <summary>
        /// Property denoting AttributeParent Id
        /// </summary>
        public Int32 AttributeParentId
        {
            get
            {
                if (this._overridenProperties != null && this._overridenProperties.ContainsKey("AttributeParentId"))
                    return GetOverridenProperties<Int32>("AttributeParentId");
                else
                    return this._attributeModelBaseProperties.AttributeParentId;
            }
            set
            {
                SetOverridenProperties<Int32>("AttributeParentId", value);
            }
        }

        /// <summary>
        /// Property denoting AttributeParent Name
        /// </summary>
        public String AttributeParentName
        {
            get
            {
                return this._attributeModelBaseProperties.AttributeParentName;
            }
        }

        /// <summary>
        /// Property denoting AttributeParent LongName
        /// </summary>
        public String AttributeParentLongName
        {
            get
            {
                if (this._overridenProperties != null && this._overridenProperties.ContainsKey("AttributeParentLongName"))
                    return GetOverridenProperties<String>("AttributeParentLongName");
                else if (this._attributeModelLocaleProperties != null && !String.IsNullOrWhiteSpace(this._attributeModelLocaleProperties.AttributeParentLongName))
                    return this._attributeModelLocaleProperties.AttributeParentLongName;
                else
                    return this._attributeModelBaseProperties.AttributeParentLongName;
            }
            set
            {
                SetOverridenProperties<String>("AttributeParentLongName", value);
            }
        }

        /// <summary>
        /// Property denoting AttributeType Id
        /// </summary>
        public Int32 AttributeTypeId
        {
            get
            {
                return this._attributeModelBaseProperties.AttributeTypeId;
            }
        }

        /// <summary>
        /// Property denoting AttributeType name. This value is same as there in Tb_Attribute.AttributeTypeName which is coming from tb_AttributeType
        /// Using this property, we load proper values in <see cref="AttributeTypeEnum"/> and <see cref="AttributeModelType"/>
        /// </summary>
        public String AttributeTypeName
        {
            get
            {
                return this._attributeModelBaseProperties.AttributeTypeName;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public AttributeModelType AttributeModelType
        {
            get
            {
                return this._attributeModelBaseProperties.AttributeModelType;
            }
        }

        /// <summary>
        /// Property denoting the AttributeType. This property represents de-normalized version of attribute type information.
        /// For possible values see <see cref="MDM.Core.AttributeTypeEnum"/>
        /// </summary>
        public AttributeTypeEnum AttributeType
        {
            get
            {
                return this._attributeModelBaseProperties.AttributeType;
            }
        }

        /// <summary>
        /// Property denoting Attribute DataType Id
        /// </summary>
        public Int32 AttributeDataTypeId
        {
            get
            {
                return this._attributeModelBaseProperties.AttributeDataTypeId;
            }
        }

        /// <summary>
        /// Property denoting Attribute DataType name
        /// </summary>
        public String AttributeDataTypeName
        {
            get
            {
                return this._attributeModelBaseProperties.AttributeDataTypeName;
            }
        }

        /// <summary>
        /// Field denoting Attribute display type Id
        /// </summary>
        public Int32 AttributeDisplayTypeId
        {
            get
            {
                return this._attributeModelBaseProperties.AttributeDisplayTypeId;
            }
        }

        /// <summary>
        /// Property denoting  Attribute display type name
        /// </summary>
        public String AttributeDisplayTypeName
        {
            get
            {
                return this._attributeModelBaseProperties.AttributeDisplayTypeName;
            }
        }

        /// <summary>
        /// Property denoting  Attribute allowable values for attribute
        /// </summary>
        public String AllowableValues
        {
            get
            {
                if (this._overridenProperties != null && this._overridenProperties.ContainsKey("AllowableValues"))
                    return GetOverridenProperties<String>("AllowableValues");
                else if (this._attributeModelMappingProperties != null && !String.IsNullOrWhiteSpace(this._attributeModelMappingProperties.AllowableValues))
                    return this._attributeModelMappingProperties.AllowableValues;
                else if (this._attributeModelLocaleProperties != null && !String.IsNullOrWhiteSpace(this._attributeModelLocaleProperties.AllowableValues))
                    return this._attributeModelLocaleProperties.AllowableValues;
                else
                    return this._attributeModelBaseProperties.AllowableValues;
            }
            set
            {
                SetOverridenProperties<String>("AllowableValues", value);
            }
        }

        /// <summary>
        /// Property denoting MaxLength of attribute value
        /// </summary>
        public Int32 MaxLength
        {
            get
            {
                if (this._overridenProperties != null && this._overridenProperties.ContainsKey("MaxLength"))
                    return GetOverridenProperties<Int32>("MaxLength");
                else if (this._attributeModelMappingProperties != null && this._attributeModelMappingProperties.MaxLength != null)
                    return (Int32)this._attributeModelMappingProperties.MaxLength;
                else if (this._attributeModelLocaleProperties != null && this._attributeModelLocaleProperties.MaxLength != null)
                    return (Int32)this._attributeModelLocaleProperties.MaxLength;
                else
                    return this._attributeModelBaseProperties.MaxLength;
            }
            set
            {
                SetOverridenProperties<Int32>("MaxLength", value);
            }
        }

        /// <summary>
        /// Property denoting MinLength of attribute value
        /// </summary>
        public Int32 MinLength
        {
            get
            {
                if (this._overridenProperties != null && this._overridenProperties.ContainsKey("MinLength"))
                    return GetOverridenProperties<Int32>("MinLength");
                else if (this._attributeModelMappingProperties != null && this._attributeModelMappingProperties.MinLength != null)
                    return (Int32)this._attributeModelMappingProperties.MinLength;
                else if (this._attributeModelLocaleProperties != null && this._attributeModelLocaleProperties.MinLength != null)
                    return (Int32)this._attributeModelLocaleProperties.MinLength;
                else
                    return this._attributeModelBaseProperties.MinLength;
            }
            set
            {
                SetOverridenProperties<Int32>("MinLength", value);
            }
        }

        /// <summary>
        /// Property denoting if attribute value is Required
        /// </summary>
        public Boolean Required
        {
            get
            {
                if (this._overridenProperties != null && this._overridenProperties.ContainsKey("Required"))
                    return GetOverridenProperties<Boolean>("Required");
                else if (this._attributeModelMappingProperties != null && this._attributeModelMappingProperties.Required != null)
                    return (Boolean)this._attributeModelMappingProperties.Required;
                else
                    return this._attributeModelBaseProperties.Required;
            }
            set
            {
                SetOverridenProperties<Boolean>("Required", value);
            }
        }

        /// <summary>
        /// Property denoting AllowableUOM for attribute value
        /// </summary>
        public String AllowableUOM
        {
            get
            {
                if (this._overridenProperties != null && this._overridenProperties.ContainsKey("AllowableUOM"))
                    return GetOverridenProperties<String>("AllowableUOM");
                else if (this._attributeModelMappingProperties != null && !String.IsNullOrWhiteSpace(this._attributeModelMappingProperties.AllowableUOM))
                    return this._attributeModelMappingProperties.AllowableUOM;
                else if (this._attributeModelLocaleProperties != null && !String.IsNullOrWhiteSpace(this._attributeModelLocaleProperties.AllowableUOM))
                    return this._attributeModelLocaleProperties.AllowableUOM;
                else
                    return this._attributeModelBaseProperties.AllowableUOM;
            }
            set
            {
                SetOverridenProperties<String>("AllowableUOM", value);
            }
        }

        /// <summary>
        /// Property denoting DefaultUOM of attribute value
        /// </summary>
        public String DefaultUOM
        {
            get
            {
                if (this._overridenProperties != null && this._overridenProperties.ContainsKey("DefaultUOM"))
                    return GetOverridenProperties<String>("DefaultUOM");
                else if (this._attributeModelMappingProperties != null && !String.IsNullOrWhiteSpace(this._attributeModelMappingProperties.DefaultUOM))
                    return this._attributeModelMappingProperties.DefaultUOM;
                else if (this._attributeModelLocaleProperties != null && !String.IsNullOrWhiteSpace(this._attributeModelLocaleProperties.DefaultUOM))
                    return this._attributeModelLocaleProperties.DefaultUOM;
                else
                    return this._attributeModelBaseProperties.DefaultUOM;
            }
            set
            {
                SetOverridenProperties<String>("DefaultUOM", value);
            }
        }

        /// <summary>
        /// Property denoting UOMType of attribute value
        /// </summary>
        public String UomType
        {
            get
            {
                return this._attributeModelBaseProperties.UomType;
            }
        }

        /// <summary>
        /// Property denoting precision of attribute value
        /// </summary>
        public Int32 Precision
        {
            get
            {
                if (this._overridenProperties != null && this._overridenProperties.ContainsKey("Precision"))
                    return GetOverridenProperties<Int32>("Precision");
                else if (this._attributeModelMappingProperties != null && this._attributeModelMappingProperties.Precision != null && this._attributeModelMappingProperties.Precision >= 0)
                    return (Int32)this._attributeModelMappingProperties.Precision;
                else
                    return this._attributeModelBaseProperties.Precision;
            }
            set
            {
                SetOverridenProperties<Int32>("Precision", value);
            }
        }

        /// <summary>
        /// Property denoting if attribute value  is collection
        /// </summary>
        public Boolean IsCollection
        {
            get
            {
                return this._attributeModelBaseProperties.IsCollection;
            }
        }

        /// <summary>
        /// Property denoting RangeTo for attribute value
        /// </summary>
        public String RangeTo
        {
            get
            {
                if (this._overridenProperties != null && this._overridenProperties.ContainsKey("RangeTo"))
                    return GetOverridenProperties<String>("RangeTo");
                else if (this._attributeModelMappingProperties != null && (this._attributeModelMappingProperties.MaxInclusive != null
                        || this._attributeModelMappingProperties.MaxExclusive != null))
                {
                    Int32 precision = (this._attributeModelMappingProperties.Precision != null) ? (Int32)this._attributeModelMappingProperties.Precision : -1;
                    return Utility.DetermineMaxValues(this._attributeModelMappingProperties.MaxInclusive, this._attributeModelMappingProperties.MaxExclusive, this.AttributeDataTypeName, precision);
                }
                else
                    return Utility.DetermineMaxValues(this.MaxInclusive, this.MaxExclusive, this.AttributeDataTypeName, this.Precision);
            }
            set
            {
                SetOverridenProperties<String>("RangeTo", value);
            }
        }

        /// <summary>
        /// Property denoting RangeFrom for attribute value
        /// </summary>
        public String RangeFrom
        {
            get
            {
                if (this._overridenProperties != null && this._overridenProperties.ContainsKey("RangeFrom"))
                    return GetOverridenProperties<String>("RangeFrom");
                else if (this._attributeModelMappingProperties != null && (this._attributeModelMappingProperties.MinInclusive != null
                         || this._attributeModelMappingProperties.MinExclusive != null))
                {
                    Int32 precision = (this._attributeModelMappingProperties.Precision != null) ? (Int32)this._attributeModelMappingProperties.Precision : -1;
                    return Utility.DetermineMinValues(this._attributeModelMappingProperties.MinInclusive, this._attributeModelMappingProperties.MinExclusive, this.AttributeDataTypeName, precision);
                }
                else
                    return Utility.DetermineMinValues(this.MinInclusive, this.MinExclusive, this.AttributeDataTypeName, this.Precision);
            }
            set
            {
                SetOverridenProperties<String>("RangeFrom", value);
            }
        }

        /// <summary>
        /// Property denoting Label of attribute
        /// </summary>
        public String Label
        {
            get
            {
                if (this._overridenProperties != null && this._overridenProperties.ContainsKey("Label"))
                    return GetOverridenProperties<String>("Label");
                else
                    return this._attributeModelBaseProperties.Label;
            }
            set
            {
                SetOverridenProperties<String>("Label", value);
            }
        }

        /// <summary>
        /// Property denoting Definition of attribute
        /// </summary>
        public String Definition
        {
            get
            {
                if (this._attributeModelMappingProperties != null && !String.IsNullOrWhiteSpace(this._attributeModelMappingProperties.Definition))
                    return this._attributeModelMappingProperties.Definition;
                else if (this._attributeModelLocaleProperties != null && !String.IsNullOrWhiteSpace(this._attributeModelLocaleProperties.Definition))
                    return this._attributeModelLocaleProperties.Definition;
                else
                    return this._attributeModelBaseProperties.Definition;
            }
        }

        /// <summary>
        /// Property denoting Example of attribute
        /// </summary>
        public String AttributeExample
        {
            get
            {
                if (this._attributeModelMappingProperties != null && !String.IsNullOrWhiteSpace(this._attributeModelMappingProperties.AttributeExample))
                    return this._attributeModelMappingProperties.AttributeExample;
                else if (this._attributeModelLocaleProperties != null && !String.IsNullOrWhiteSpace(this._attributeModelLocaleProperties.AttributeExample))
                    return this._attributeModelLocaleProperties.AttributeExample;
                else
                    return this._attributeModelBaseProperties.AttributeExample;
            }
        }

        /// <summary>
        /// Property denoting BusinessRule for attribute
        /// </summary>
        public String BusinessRule
        {
            get
            {
                if (this._attributeModelMappingProperties != null && !String.IsNullOrWhiteSpace(this._attributeModelMappingProperties.BusinessRule))
                    return this._attributeModelMappingProperties.BusinessRule;
                else if (this._attributeModelLocaleProperties != null && !String.IsNullOrWhiteSpace(this._attributeModelLocaleProperties.BusinessRule))
                    return this._attributeModelLocaleProperties.BusinessRule;
                else
                    return this._attributeModelBaseProperties.BusinessRule;
            }
        }

        /// <summary>
        /// Property denoting if attribute is read-only
        /// </summary>
        public Boolean ReadOnly
        {
            get
            {
                if (this._overridenProperties != null && this._overridenProperties.ContainsKey("ReadOnly"))
                    return GetOverridenProperties<Boolean>("ReadOnly");
                else if (this._attributeModelMappingProperties != null && this._attributeModelMappingProperties.ReadOnly != null)
                    return (Boolean)this._attributeModelMappingProperties.ReadOnly;
                else
                    return this._attributeModelBaseProperties.ReadOnly;
            }
            set
            {
                SetOverridenProperties<Boolean>("ReadOnly", value);
            }
        }

        /// <summary>
        /// Property denoting Extension of attribute
        /// </summary>
        public String Extension
        {
            get
            {
                return this._attributeModelBaseProperties.Extension;
            }
        }

        /// <summary>
        /// Property denoting AssemblyName for attribute business rule
        /// </summary>
        public String AttributeAssemblyName
        {
            get
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Property denoting Method for attribute business rule
        /// </summary>
        public String AssemblyMethod
        {
            get
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Property denoting AttributeRegExp for attribute value
        /// </summary>
        public String AttributeRegEx
        {
            get
            {
                return this._attributeModelBaseProperties.AttributeRegEx;
            }
        }

        /// <summary>
        /// Property denoting the attribute regular expression error message for the attribute value
        /// </summary>
        public String RegExErrorMessage
        {
            get
            {
                return this._attributeModelBaseProperties.RegExErrorMessage;
            }
        }

        /// <summary>
        /// Property denoting AssemblyClass for attribute business rule
        /// </summary>
        public String AssemblyClass
        {
            get
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Property denoting LookUpTableName for attribute
        /// </summary>
        public String LookUpTableName
        {
            get
            {
                return this._attributeModelBaseProperties.LookUpTableName;
            }
        }

        /// <summary>
        /// Property denoting DefaultValue for attribute
        /// </summary>
        public String DefaultValue
        {
            get
            {
                if (this._attributeModelMappingProperties != null && !String.IsNullOrWhiteSpace(this._attributeModelMappingProperties.DefaultValue))
                    return this._attributeModelMappingProperties.DefaultValue;
                else if (this._attributeModelLocaleProperties != null && !String.IsNullOrWhiteSpace(this._attributeModelLocaleProperties.DefaultValue))
                    return this._attributeModelLocaleProperties.DefaultValue;
                else
                    return this._attributeModelBaseProperties.DefaultValue;
            }
        }

        /// <summary>
        /// Property denoting isClassification
        /// </summary>
        public Boolean Classification
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Property denoting complex table name for attribute
        /// </summary>
        public String ComplexTableName
        {
            get
            {
                return this._attributeModelBaseProperties.ComplexTableName;
            }
        }

        /// <summary>
        /// Property denoting complex table column names for the complex attribute
        /// </summary>
        public Collection<String> ComplexTableColumnNameList
        {
            get
            {
                return this._attributeModelBaseProperties.ComplexTableColumnNameList;
            }
        }

        /// <summary>
        /// Property denoting rule lookup table for attribute
        /// </summary>
        public String RuleLookupTable
        {
            get
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Property denoting RuleSP for attribute
        /// </summary>
        public String RuleSP
        {
            get
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Property denoting Path of attribute
        /// </summary>
        public String Path
        {
            get
            {
                if (this._overridenProperties != null && this._overridenProperties.ContainsKey("Path"))
                    return GetOverridenProperties<String>("Path");
                else
                    return this._attributeModelBaseProperties.Path;
            }
            set
            {
                SetOverridenProperties<String>("Path", value);
            }
        }

        /// <summary>
        /// Property denoting if attribute id searchable
        /// </summary>
        public Boolean Searchable
        {
            get
            {
                if (this._overridenProperties != null && this._overridenProperties.ContainsKey("Searchable"))
                {
                    return GetOverridenProperties<Boolean>("Searchable");
                }
                else
                {
                    return this._attributeModelBaseProperties.Searchable;
                }
            }
            set
            {
                SetOverridenProperties<Boolean>("Searchable", value);
            }
        }
        
        /// <summary>
        /// Property denoting is history is to be kept for attribute value
        /// </summary>
        public Boolean EnableHistory
        {
            get
            {
                if (this._overridenProperties != null && this._overridenProperties.ContainsKey("EnableHistory"))
                {
                    return GetOverridenProperties<Boolean>("EnableHistory");
                }
                else
                {
                    return this._attributeModelBaseProperties.EnableHistory;
                }
            }
            set
            {
                SetOverridenProperties<Boolean>("EnableHistory", value);
            }
        }

        /// <summary>
        /// Property denoting if attribute is to be persisted
        /// </summary>
        public Boolean Persists
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Property denoting WebUri for attribute
        /// </summary>
        public String WebUri
        {
            get
            {
                if (this._overridenProperties != null && this._overridenProperties.ContainsKey("WebUri"))
                    return GetOverridenProperties<String>("WebUri");
                else
                    return this._attributeModelBaseProperties.WebUri;
            }
            set
            {
                SetOverridenProperties<String>("WebUri", value);
            }
        }

        /// <summary>
        /// Property denoting custom action for attribute
        /// </summary>
        public String CustomAction
        {
            get
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Property denoting initial population method for attribute value
        /// </summary>
        public String InitialPopulationMethod
        {
            get { return String.Empty; }
        }

        /// <summary>
        /// Property denoting JavaScript to be executed on click for an attribute
        /// </summary>
        public String OnClickJavaScript
        {
            get
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Property denoting JavaScript to be executed on load for an attribute
        /// </summary>
        public String OnLoadJavaScript
        {
            get
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Property denoting storage format of attribute value for lookup attribute
        /// </summary>
        public String LkStorageFormat
        {
            get
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Property denoting list of columns for showing for lookup attribute
        /// </summary>
        public String LkDisplayColumns
        {
            get
            {
                return this._attributeModelBaseProperties.LkDisplayColumns;
            }
        }

        /// <summary>
        /// Property denoting sort order for lookup values in lookup attribute
        /// </summary>
        public String LkSortOrder
        {
            get
            {
                return this._attributeModelBaseProperties.LkSortOrder;
            }
        }

        /// <summary>
        /// Property denoting list of columns to search in for lookup attribute
        /// </summary>
        public String LkSearchColumns
        {
            get
            {
                return this._attributeModelBaseProperties.LkSearchColumns;
            }
        }

        /// <summary>
        /// Property denoting display format of attribute value for lookup attribute
        /// </summary>
        public String LkDisplayFormat
        {
            get
            {
                return this._attributeModelBaseProperties.LkDisplayFormat;
            }
        }

        /// <summary>
        /// Property denoting list of columns for showing for lookup attribute
        /// </summary>
        public String LookUpDisplayColumns
        {
            get
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Property denoting if duplicates are allowed for lookup attribute
        /// </summary>
        public Boolean LkupDuplicateAllowed
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Property denoting whether to store lookup reference
        /// </summary>
        public Boolean StoreLookupReference
        {
            get { return false; }
        }

        /// <summary>
        /// Property denoting sort order of attribute
        /// </summary>
        public Int32 SortOrder
        {
            get
            {
                if (this._overridenProperties != null && this._overridenProperties.ContainsKey("SortOrder"))
                    return GetOverridenProperties<Int32>("SortOrder");
                else if (this._attributeModelMappingProperties != null && this._attributeModelMappingProperties.SortOrder != null)
                    return (Int32)this._attributeModelMappingProperties.SortOrder;
                else
                    return this._attributeModelBaseProperties.SortOrder;
            }
            set
            {
                SetOverridenProperties<Int32>("SortOrder", value);
            }
        }

        /// <summary>
        /// Property denoting export mask for attribute value
        /// </summary>
        public String ExportMask
        {
            get
            {
                if (this._attributeModelMappingProperties != null && !String.IsNullOrWhiteSpace(this._attributeModelMappingProperties.ExportMask))
                    return this._attributeModelMappingProperties.ExportMask;
                else if (this._attributeModelLocaleProperties != null && !String.IsNullOrWhiteSpace(this._attributeModelLocaleProperties.ExportMask))
                    return this._attributeModelLocaleProperties.ExportMask;
                else
                    return this._attributeModelBaseProperties.ExportMask;
            }
        }

        /// <summary>
        /// Property denoting if attribute is inheritable
        /// </summary>
        public Boolean Inheritable
        {
            get
            {
                return this._attributeModelBaseProperties.Inheritable;
            }
        }

        /// <summary>
        /// Property denoting if attribute is hidden
        /// </summary>
        public Boolean IsHidden
        {
            get
            {
                if (this._overridenProperties != null && this._overridenProperties.ContainsKey("IsHidden"))
                    return GetOverridenProperties<Boolean>("IsHidden");
                else
                    return this._attributeModelBaseProperties.IsHidden;
            }
            set
            {
                SetOverridenProperties<Boolean>("IsHidden", value);
            }
        }

        /// <summary>
        /// Property denoting if attribute is a complex attribute
        /// </summary>
        public Boolean IsComplex
        {
            get
            {
                return this._attributeModelBaseProperties.IsComplex;
            }
        }

        /// <summary>
        /// Property denoting if attribute is a Hierarchical attribute
        /// </summary>
        public Boolean IsHierarchical
        {
            get
            {
                return this._attributeModelBaseProperties.IsHierarchical;
            }
        }

        /// <summary>
        /// Property denoting if attribute is complex child
        /// </summary>
        public Boolean IsComplexChild
        {
            get
            {
                if (this._overridenProperties != null && this._overridenProperties.ContainsKey("IsComplexChild"))
                    return GetOverridenProperties<Boolean>("IsComplexChild");
                else
                    return this._attributeModelBaseProperties.IsComplexChild;
            }
            set
            {
                SetOverridenProperties<Boolean>("IsComplexChild", value);
            }
        }

        /// <summary>
        /// Property denoting if attribute is Hierarchical child
        /// </summary>
        public Boolean IsHierarchicalChild
        {
            get
            {
                if (this._overridenProperties != null && this._overridenProperties.ContainsKey("IsHierarchicalChild"))
                    return GetOverridenProperties<Boolean>("IsHierarchicalChild");
                else
                    return this._attributeModelBaseProperties.IsHierarchicalChild;
            }
            set
            {
                SetOverridenProperties<Boolean>("IsHierarchicalChild", value);
            }
        }

        /// <summary>
        /// Property denoting if attribute is a lookup attribute
        /// </summary>
        public Boolean IsLookup
        {
            get
            {
                return this._attributeModelBaseProperties.IsLookup;
            }
        }

        /// <summary>
        /// Property denoting if attribute is localizable 
        /// </summary>
        public Boolean IsLocalizable
        {
            get
            {
                return this._attributeModelBaseProperties.IsLocalizable;
            }
        }

        /// <summary>
        /// Property denoting is locale specific format is to be applied to attribute value
        /// </summary>
        public Boolean ApplyLocaleFormat
        {
            get
            {
                return this._attributeModelBaseProperties.ApplyLocaleFormat;
            }
        }

        /// <summary>
        /// Property denoting is time zone conversion is to be applied to attribute value
        /// </summary>
        public Boolean ApplyTimeZoneConversion
        {
            get
            {
                return this._attributeModelBaseProperties.ApplyTimeZoneConversion;
            }
        }

        /// <summary>
        /// Property denoting if null values are searchable for attribute value
        /// </summary>
        public Boolean AllowNullSearch
        {
            get
            {
                return this._attributeModelBaseProperties.AllowNullSearch;
            }
        }

        /// <summary>
        /// Property denoting if attribute is to be shown at the time of item creation
        /// </summary>
        public Boolean ShowAtCreation
        {
            get
            {
                if (this._attributeModelMappingProperties != null && this._attributeModelMappingProperties.ShowAtCreation != null)
                    return (Boolean)this._attributeModelMappingProperties.ShowAtCreation;
                else
                    return this._attributeModelBaseProperties.ShowAtCreation;
            }
        }

        /// <summary>
        /// Property denoting if the min value is inclusive
        /// </summary>
        public String MinInclusive
        {
            get
            {
                if (this._attributeModelMappingProperties != null && !String.IsNullOrWhiteSpace(this._attributeModelMappingProperties.MinInclusive))
                    return this._attributeModelMappingProperties.MinInclusive;
                else if (this._attributeModelLocaleProperties != null && !String.IsNullOrWhiteSpace(this._attributeModelLocaleProperties.MinInclusive))
                    return this._attributeModelLocaleProperties.MinInclusive;
                else
                    return this._attributeModelBaseProperties.MinInclusive;
            }
        }

        /// <summary>
        /// Property denoting if the min value is exclusive
        /// </summary>
        public String MinExclusive
        {
            get
            {
                if (this._attributeModelMappingProperties != null && !String.IsNullOrWhiteSpace(this._attributeModelMappingProperties.MinExclusive))
                    return this._attributeModelMappingProperties.MinExclusive;
                else if (this._attributeModelLocaleProperties != null && !String.IsNullOrWhiteSpace(this._attributeModelLocaleProperties.MinExclusive))
                    return this._attributeModelLocaleProperties.MinExclusive;
                else
                    return this._attributeModelBaseProperties.MinExclusive;
            }
        }

        /// <summary>
        /// Property denoting if the max value is inclusive
        /// </summary>
        public String MaxInclusive
        {
            get
            {
                if (this._attributeModelMappingProperties != null && !String.IsNullOrWhiteSpace(this._attributeModelMappingProperties.MaxInclusive))
                    return this._attributeModelMappingProperties.MaxInclusive;
                else if (this._attributeModelLocaleProperties != null && !String.IsNullOrWhiteSpace(this._attributeModelLocaleProperties.MaxInclusive))
                    return this._attributeModelLocaleProperties.MaxInclusive;
                else
                    return this._attributeModelBaseProperties.MaxInclusive;
            }
        }

        /// <summary>
        /// Property denoting if the max value is exclusive
        /// </summary>
        public String MaxExclusive
        {
            get
            {
                if (this._attributeModelMappingProperties != null && !String.IsNullOrWhiteSpace(this._attributeModelMappingProperties.MaxExclusive))
                    return this._attributeModelMappingProperties.MaxExclusive;
                else if (this._attributeModelLocaleProperties != null && !String.IsNullOrWhiteSpace(this._attributeModelLocaleProperties.MaxExclusive))
                    return this._attributeModelLocaleProperties.MaxExclusive;
                else
                    return this._attributeModelBaseProperties.MaxExclusive;
            }
        }

        /// <summary>
        /// Property denoting if the attribute value is inheritable only and not overridden
        /// </summary>
        public Boolean InheritableOnly
        {
            get
            {
                if (this._attributeModelMappingProperties != null && this._attributeModelMappingProperties.InheritableOnly != null)
                {
                    return (Boolean)this._attributeModelMappingProperties.InheritableOnly;
                }
                else
                {
                    return this._attributeModelBaseProperties.InheritableOnly;
                }
            }
        }

        /// <summary>
        /// Property denoting if the attribute value is auto promotable
        /// </summary>
        public Boolean AutoPromotable
        {
            get
            {
                if (this._attributeModelMappingProperties != null && this._attributeModelMappingProperties.AutoPromotable != null)
                {
                    return (Boolean)this._attributeModelMappingProperties.AutoPromotable;
                }
                else
                {
                    return this._attributeModelBaseProperties.AutoPromotable;
                }
            }
        }

        /// <summary>
        /// Property showing permission set for the current attribute model
        /// </summary>
        public new Collection<UserAction> PermissionSet
        {
            get
            {
                return this._permissionSet;
            }
            set
            {
                this._permissionSet = value;
            }
        }

        /// <summary>
        /// Property denoting is the current attribute model depends on any other attribute model.
        /// if 'true' then current attribute model is dependent on other attribute model.
        /// </summary>
        public Boolean IsDependentAttribute
        {
            get
            {
                return this._attributeModelBaseProperties.IsDependentAttribute;
            }
        }

        /// <summary>
        /// Property denoting has any Dependent attributes for the current attribute model.
        /// </summary>
        public Boolean HasDependentAttribute
        {
            get
            {
                return this._attributeModelBaseProperties.HasDependentAttribute;
            }
        }

        /// <summary>
        /// Property denoting Dependent attributes for this current attribute model.
        /// Collection of dependent attributes which contains the details of children dependent link mapping details for this attribute model.
        /// </summary>
        [DataMember]
        public DependentAttributeCollection DependentChildAttributes
        {
            get
            {
                return this._dependentChildAttributes;
            }
            set
            {
                this._dependentChildAttributes = value;
            }
        }

        /// <summary>
        /// Property denoting Dependent on attributes for this current attribute model.
        /// Collection of dependent attributes which contains the details of Parent dependent link mapping details for this attribute model.
        /// </summary>
        [DataMember]
        public DependentAttributeCollection DependentParentAttributes
        {
            get
            {
                return this._dependentParentAttributes;
            }
            set
            {
                this._dependentParentAttributes = value;
            }
        }

        /// <summary>
        /// Property denoting child attribute models
        /// </summary>
        public AttributeModelCollection AttributeModels
        {
            get
            {
                return this._attributeModels;
            }
            set
            {
                this._attributeModels = value;
            }
        }

        /// <summary>
        /// Property denoting data context for an attribute model
        /// </summary>
        public AttributeModelContext Context
        {
            get
            {
                return this._context;
            }
            set
            {
                this._context = value;
            }
        }

        /// <summary>
        /// Property denoting child attribute models
        /// </summary>
        public Dictionary<String, String> OverridenProperties
        {
            get
            {
                return this._overridenProperties;
            }
            set
            {
                this._overridenProperties = value;
            }
        }

        /// <summary>
        /// Property denoting the original attribute model
        /// </summary>
        public AttributeModel OriginalAttributeModel
        {
            get
            {
                return _originalAttributeModel;
            }
            set
            {
                this._originalAttributeModel = value;
            }
        }

        /// <summary>
        /// Property denoting attribute has locale properties.
        /// </summary>
        public Boolean HasLocaleProperties
        {
            get
            {
                return _hasLocaleProperties;
            }
        }

        #region IDataModelObject Properties

        /// <summary>
        /// Property denoting a ObjectType for an dataModelObject object
        /// </summary>
        public ObjectType DataModelObjectType
        {
            get
            {
                return MDM.Core.ObjectType.AttributeModel;
            }
        }

        /// <summary>
        /// Property denoting the external id for an dataModelObject object
        /// </summary>
        public String ExternalId
        {
            get
            {
                return _externalId;
            }
            set
            {
                _externalId = value;
            }
        }

        /// <summary>
        /// Property denoting if arbitrary precision is used for Decimal attributes.
        /// If value of IsPrecisionArbitrary is true, then decimal value 12.2 with precision 3 will be stored as "12.2"
        /// If value of IsPrecisionArbitrary is false, then decimal value 12.2 with precision 3 will be stored as "12.200"
        /// </summary>
        public Boolean IsPrecisionArbitrary
        {
            get
            {
                return this._attributeModelBaseProperties.IsPrecisionArbitrary;
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(Object obj)
        {
            if (base.Equals(obj))
            {
                if (obj is AttributeModel)
                {
                    AttributeModel objectToBeCompared = obj as AttributeModel;

                    if (this.Id != objectToBeCompared.Id)
                        return false;

                    if (this.Name != objectToBeCompared.Name)
                        return false;

                    if (this.LongName != objectToBeCompared.LongName)
                        return false;

                    if (this.AuditRefId != objectToBeCompared.AuditRefId)
                        return false;

                    if (this.AttributeParentId != objectToBeCompared.AttributeParentId)
                        return false;

                    if (this.AttributeParentName != objectToBeCompared.AttributeParentName)
                        return false;

                    if (this.AttributeParentLongName != objectToBeCompared.AttributeParentLongName)
                        return false;

                    if (this.AttributeDataTypeId != objectToBeCompared.AttributeDataTypeId)
                        return false;

                    if (this.AttributeDataTypeName != objectToBeCompared.AttributeDataTypeName)
                        return false;

                    if (this.AttributeDisplayTypeId != objectToBeCompared.AttributeDisplayTypeId)
                        return false;

                    if (this.AttributeDisplayTypeName != objectToBeCompared.AttributeDisplayTypeName)
                        return false;

                    if (this.AttributeDataTypeId != objectToBeCompared.AttributeDataTypeId)
                        return false;

                    if (this.AllowableValues != objectToBeCompared.AllowableValues)
                        return false;

                    if (this.MaxLength != objectToBeCompared.MaxLength)
                        return false;

                    if (this.MinLength != objectToBeCompared.MinLength)
                        return false;

                    if (this.Required != objectToBeCompared.Required)
                        return false;

                    if (this.AllowableUOM != objectToBeCompared.AllowableUOM)
                        return false;

                    if (this.DefaultUOM != objectToBeCompared.DefaultUOM)
                        return false;

                    if (this.UomType != objectToBeCompared.UomType)
                        return false;

                    if (this.Precision != objectToBeCompared.Precision)
                        return false;

                    if (this.IsCollection != objectToBeCompared.IsCollection)
                        return false;

                    if (this.RangeTo != objectToBeCompared.RangeTo)
                        return false;

                    if (this.RangeFrom != objectToBeCompared.RangeFrom)
                        return false;

                    if (this.Label != objectToBeCompared.Label)
                        return false;

                    if (this.Definition != objectToBeCompared.Definition)
                        return false;

                    if (this.AttributeExample != objectToBeCompared.AttributeExample)
                        return false;

                    if (this.BusinessRule != objectToBeCompared.BusinessRule)
                        return false;

                    if (this.ReadOnly != objectToBeCompared.ReadOnly)
                        return false;

                    if (this.Extension != objectToBeCompared.Extension)
                        return false;

                    if (this.AttributeAssemblyName != objectToBeCompared.AttributeAssemblyName)
                        return false;

                    if (this.AssemblyMethod != objectToBeCompared.AssemblyMethod)
                        return false;

                    if (this.AttributeRegEx != objectToBeCompared.AttributeRegEx)
                        return false;

                    if (this.RegExErrorMessage != objectToBeCompared.RegExErrorMessage)
                    {
                        return false;
                    }

                    if (this.AssemblyClass != objectToBeCompared.AssemblyClass)
                        return false;

                    if (this.LookUpTableName != objectToBeCompared.LookUpTableName)
                        return false;

                    if (this.DefaultValue != objectToBeCompared.DefaultValue)
                        return false;

                    if (this.Classification != objectToBeCompared.Classification)
                        return false;

                    if (this.ComplexTableName != objectToBeCompared.ComplexTableName)
                        return false;

                    if (this.RuleLookupTable != objectToBeCompared.RuleLookupTable)
                        return false;

                    if (this.RuleSP != objectToBeCompared.RuleSP)
                        return false;

                    if (this.Path != objectToBeCompared.Path)
                        return false;

                    if (this.Searchable != objectToBeCompared.Searchable)
                        return false;
                    
                    if (this.EnableHistory != objectToBeCompared.EnableHistory)
                        return false;

                    if (this.Persists != objectToBeCompared.Persists)
                        return false;

                    if (this.WebUri != objectToBeCompared.WebUri)
                        return false;

                    if (this.CustomAction != objectToBeCompared.CustomAction)
                        return false;

                    if (this.InitialPopulationMethod != objectToBeCompared.InitialPopulationMethod)
                        return false;

                    if (this.OnClickJavaScript != objectToBeCompared.OnClickJavaScript)
                        return false;

                    if (this.OnLoadJavaScript != objectToBeCompared.OnLoadJavaScript)
                        return false;

                    if (this.LkStorageFormat != objectToBeCompared.LkStorageFormat)
                        return false;

                    if (this.LkDisplayColumns != objectToBeCompared.LkDisplayColumns)
                        return false;

                    if (this.LkSortOrder != objectToBeCompared.LkSortOrder)
                        return false;

                    if (this.LkSearchColumns != objectToBeCompared.LkSearchColumns)
                        return false;

                    if (this.LkDisplayFormat != objectToBeCompared.LkDisplayFormat)
                        return false;

                    if (this.LookUpDisplayColumns != objectToBeCompared.LookUpDisplayColumns)
                        return false;

                    if (this.LkupDuplicateAllowed != objectToBeCompared.LkupDuplicateAllowed)
                        return false;

                    if (this.StoreLookupReference != objectToBeCompared.StoreLookupReference)
                        return false;

                    if (this.SortOrder != objectToBeCompared.SortOrder)
                        return false;

                    if (this.ExportMask != objectToBeCompared.ExportMask)
                        return false;

                    if (this.Inheritable != objectToBeCompared.Inheritable)
                        return false;

                    if (this.IsHidden != objectToBeCompared.IsHidden)
                        return false;

                    if (this.IsComplex != objectToBeCompared.IsComplex)
                        return false;

                    if (this.IsHierarchical != objectToBeCompared.IsHierarchical)
                        return false;

                    if (this.IsLookup != objectToBeCompared.IsLookup)
                        return false;

                    if (this.IsLocalizable != objectToBeCompared.IsLocalizable)
                        return false;

                    if (this.ApplyLocaleFormat != objectToBeCompared.ApplyLocaleFormat)
                        return false;

                    if (this.ApplyTimeZoneConversion != objectToBeCompared.ApplyTimeZoneConversion)
                        return false;

                    if (this.AllowNullSearch != objectToBeCompared.AllowNullSearch)
                        return false;

                    if (this.ShowAtCreation != objectToBeCompared.ShowAtCreation)
                        return false;

                    if (!this.Context.Equals(objectToBeCompared.Context))
                        return false;

                    if (!this.AttributeModels.Equals(objectToBeCompared.AttributeModels))
                        return false;

                    if (!this.HasDependentAttribute.Equals(objectToBeCompared.HasDependentAttribute))
                        return false;

                    if (!this.IsDependentAttribute.Equals(objectToBeCompared.IsDependentAttribute))
                        return false;

                    Int32 dependentParentAttributesUnion = this.DependentParentAttributes.ToList().Union(objectToBeCompared.DependentParentAttributes.ToList()).Count();
                    Int32 dependentParentAttributesIntersect = this.DependentParentAttributes.ToList().Intersect(objectToBeCompared.DependentParentAttributes.ToList()).Count();

                    if (dependentParentAttributesUnion != dependentParentAttributesIntersect)
                        return false;

                    Int32 dependentChildAttributesUnion = this.DependentChildAttributes.ToList().Union(objectToBeCompared.DependentChildAttributes.ToList()).Count();
                    Int32 dependentChildAttributesIntersect = this.DependentChildAttributes.ToList().Intersect(objectToBeCompared.DependentChildAttributes.ToList()).Count();

                    if (dependentChildAttributesUnion != dependentChildAttributesIntersect)
                        return false;

                    if (!this.IsPrecisionArbitrary.Equals(objectToBeCompared.IsPrecisionArbitrary))
                        return false;

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
            int hashCode = base.GetHashCode() ^ this.AttributeParentId.GetHashCode() ^ this.AttributeParentName.GetHashCode() ^ this.AttributeParentLongName.GetHashCode() ^ this.AttributeTypeId.GetHashCode() ^
                this.AttributeTypeName.GetHashCode() ^ this.AttributeDataTypeId.GetHashCode() ^ this.AttributeDataTypeName.GetHashCode() ^ this.AttributeDisplayTypeId.GetHashCode() ^ this.AttributeDisplayTypeName.GetHashCode() ^
                this.AllowableValues.GetHashCode() ^ this.MaxLength.GetHashCode() ^ this.MinLength.GetHashCode() ^ this.Required.GetHashCode() ^ this.AllowableUOM.GetHashCode() ^ this.DefaultUOM.GetHashCode() ^ this.UomType.GetHashCode() ^
                this.Precision.GetHashCode() ^ this.IsCollection.GetHashCode() ^ this.RangeTo.GetHashCode() ^ this.RangeFrom.GetHashCode() ^ this.Label.GetHashCode() ^
                this.Definition.GetHashCode() ^ this.AttributeExample.GetHashCode() ^ this.BusinessRule.GetHashCode() ^ this.ReadOnly.GetHashCode() ^ this.Extension.GetHashCode() ^ this.AttributeAssemblyName.GetHashCode() ^ this.AssemblyMethod.GetHashCode() ^
                this.AttributeRegEx.GetHashCode() ^ this.LookUpTableName.GetHashCode() ^ this.DefaultValue.GetHashCode() ^ this.Classification.GetHashCode() ^ this.ComplexTableName.GetHashCode() ^ this.RuleLookupTable.GetHashCode() ^ this.RuleSP.GetHashCode() ^
                this.Path.GetHashCode() ^ this.Searchable.GetHashCode() ^ this.EnableHistory.GetHashCode() ^ this.Persists.GetHashCode() ^ this.WebUri.GetHashCode() ^ this.CustomAction.GetHashCode() ^ this.InitialPopulationMethod.GetHashCode() ^
                this.OnClickJavaScript.GetHashCode() ^ this.OnLoadJavaScript.GetHashCode() ^ this.LkStorageFormat.GetHashCode() ^ this.LkDisplayColumns.GetHashCode() ^ this.LkSortOrder.GetHashCode() ^ this.LkSearchColumns.GetHashCode() ^ this.LkDisplayFormat.GetHashCode() ^
                this.LookUpDisplayColumns.GetHashCode() ^ this.LkupDuplicateAllowed.GetHashCode() ^ this.StoreLookupReference.GetHashCode() ^ this.SortOrder.GetHashCode() ^ this.ExportMask.GetHashCode() ^ this.Inheritable.GetHashCode() ^ this.IsHidden.GetHashCode() ^
                this.IsComplex.GetHashCode() ^ this.IsHierarchical.GetHashCode() ^ this.IsLookup.GetHashCode() ^ this.IsLocalizable.GetHashCode() ^ this.ApplyLocaleFormat.GetHashCode() ^ this.ApplyTimeZoneConversion.GetHashCode() ^ this.AllowNullSearch.GetHashCode() ^ this.ShowAtCreation.GetHashCode() ^
                this.AttributeModels.GetHashCode() ^ this.PermissionSet.GetHashCode() ^ this.HasDependentAttribute.GetHashCode() ^ this.IsDependentAttribute.GetHashCode() ^ this.DependentChildAttributes.GetHashCode() ^ this.DependentParentAttributes.GetHashCode() ^
                this.IsPrecisionArbitrary.GetHashCode() ^ this.RegExErrorMessage.GetHashCode() ^ this.InheritableOnly.GetHashCode() ^ this.AutoPromotable.GetHashCode();

            return hashCode;
        }

        /// <summary>
        /// Get Xml representation of Attribute Model
        /// </summary>
        /// <returns>Xml representation of Attribute Model</returns>
        public override String ToXml()
        {
            String returnXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);
            //xmlWriter.WriteStartDocument();

            //AttributeModel node start
            xmlWriter.WriteStartElement("AttributeModel");

            #region write Attribute Model meta data for full attribute xml

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("LongName", this.LongName);
            xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
            xmlWriter.WriteAttributeString("AttributeParentId", this.AttributeParentId.ToString());
            xmlWriter.WriteAttributeString("AttributeParentName", this.AttributeParentName);
            xmlWriter.WriteAttributeString("AttributeParentLongName", this.AttributeParentLongName);
            xmlWriter.WriteAttributeString("AttributeTypeId", this.AttributeTypeId.ToString());
            xmlWriter.WriteAttributeString("AttributeTypeName", this.AttributeTypeName);
            xmlWriter.WriteAttributeString("AttributeDataTypeId", this.AttributeDataTypeId.ToString());
            xmlWriter.WriteAttributeString("AttributeDataTypeName", this.AttributeDataTypeName);
            xmlWriter.WriteAttributeString("AttributeDisplayTypeId", this.AttributeDisplayTypeId.ToString());
            xmlWriter.WriteAttributeString("AttributeDisplayTypeName", this.AttributeDisplayTypeName);
            xmlWriter.WriteAttributeString("AllowValues", this.AllowableValues);
            xmlWriter.WriteAttributeString("MaxLength", this.MaxLength.ToString());
            xmlWriter.WriteAttributeString("MinLength", this.MinLength.ToString());
            xmlWriter.WriteAttributeString("Required", this.Required.ToString());
            xmlWriter.WriteAttributeString("AllowableUOM", this.AllowableUOM);
            xmlWriter.WriteAttributeString("DefaultUOM", this.DefaultUOM);
            xmlWriter.WriteAttributeString("UomType", this.UomType);
            xmlWriter.WriteAttributeString("Precision", this.Precision.ToString());
            xmlWriter.WriteAttributeString("IsCollection", this.IsCollection.ToString());
            xmlWriter.WriteAttributeString("RangeTo", this.RangeTo);
            xmlWriter.WriteAttributeString("RangeFrom", this.RangeFrom.ToString());
            xmlWriter.WriteAttributeString("Label", this.Label);
            xmlWriter.WriteAttributeString("Definition", this.Definition);
            xmlWriter.WriteAttributeString("AttributeExample", this.AttributeExample);
            xmlWriter.WriteAttributeString("BusinessRule", this.BusinessRule);
            xmlWriter.WriteAttributeString("ReadOnly", this.ReadOnly.ToString());
            xmlWriter.WriteAttributeString("Extension", this.Extension);
            xmlWriter.WriteAttributeString("AttributeAssemblyName", this.AttributeAssemblyName);
            xmlWriter.WriteAttributeString("AssemblyMethod", this.AssemblyMethod);
            xmlWriter.WriteAttributeString("AttributeRegEx", this.AttributeRegEx);
            xmlWriter.WriteAttributeString("RegExErrorMessage", this.RegExErrorMessage);
            xmlWriter.WriteAttributeString("AssemblyClass", this.AssemblyClass);
            xmlWriter.WriteAttributeString("LookUpTableName", this.LookUpTableName);
            xmlWriter.WriteAttributeString("DefaultValue", this.DefaultValue);
            xmlWriter.WriteAttributeString("Classification", this.Classification.ToString());
            xmlWriter.WriteAttributeString("ComplexTableName", this.ComplexTableName);
            xmlWriter.WriteAttributeString("RuleLookupTable", this.RuleLookupTable);
            xmlWriter.WriteAttributeString("RuleSP", this.RuleSP);
            xmlWriter.WriteAttributeString("Path", this.Path);
            xmlWriter.WriteAttributeString("Searchable", this.Searchable.ToString());
            xmlWriter.WriteAttributeString("EnableHistory", this.EnableHistory.ToString());
            xmlWriter.WriteAttributeString("Persists", this.Persists.ToString());
            xmlWriter.WriteAttributeString("WebUri", this.WebUri);
            xmlWriter.WriteAttributeString("CustomAction", this.CustomAction);
            xmlWriter.WriteAttributeString("InitialPopulationMethod", this.InitialPopulationMethod);
            xmlWriter.WriteAttributeString("OnClickJavaScript", this.OnClickJavaScript);
            xmlWriter.WriteAttributeString("OnLoadJavaScript", this.OnLoadJavaScript);
            xmlWriter.WriteAttributeString("LkStorageFormat", this.LkStorageFormat);
            xmlWriter.WriteAttributeString("LkDisplayColumns", this.LkDisplayColumns);
            xmlWriter.WriteAttributeString("LkSortOrder", this.LkSortOrder);
            xmlWriter.WriteAttributeString("LkSearchColumns", this.LkSearchColumns);
            xmlWriter.WriteAttributeString("LkDisplayFormat", this.LkDisplayFormat);
            xmlWriter.WriteAttributeString("LookUpDisplayColumns", this.LookUpDisplayColumns);
            xmlWriter.WriteAttributeString("LkupDuplicateAllowed", this.LkupDuplicateAllowed.ToString());
            xmlWriter.WriteAttributeString("StoreLookupReference", this.StoreLookupReference.ToString());
            xmlWriter.WriteAttributeString("SortOrder", this.SortOrder.ToString());
            xmlWriter.WriteAttributeString("ExportMask", this.ExportMask);
            xmlWriter.WriteAttributeString("Inheritable", this.Inheritable.ToString());
            xmlWriter.WriteAttributeString("IsHidden", this.IsHidden.ToString());
            xmlWriter.WriteAttributeString("IsComplex", this.IsComplex.ToString());
            xmlWriter.WriteAttributeString("IsHierarchical", this.IsHierarchical.ToString());
            xmlWriter.WriteAttributeString("IsLookup", this.IsLookup.ToString());
            xmlWriter.WriteAttributeString("IsLocalizable", this.IsLocalizable.ToString());
            xmlWriter.WriteAttributeString("ApplyLocaleFormat", this.ApplyLocaleFormat.ToString());
            xmlWriter.WriteAttributeString("ApplyTimeZoneConversion", this.ApplyTimeZoneConversion.ToString());
            xmlWriter.WriteAttributeString("AllowNullSearch", this.AllowNullSearch.ToString());
            xmlWriter.WriteAttributeString("ShowAtCreation", this.ShowAtCreation.ToString());
            xmlWriter.WriteAttributeString("Action", this.Action.ToString());
            xmlWriter.WriteAttributeString("MinInclusive", this.MinInclusive);
            xmlWriter.WriteAttributeString("MinExclusive", this.MinExclusive);
            xmlWriter.WriteAttributeString("MaxInclusive", this.MaxInclusive);
            xmlWriter.WriteAttributeString("MaxExclusive", this.MaxExclusive);
            xmlWriter.WriteAttributeString("InheritableOnly", this.InheritableOnly.ToString());
            xmlWriter.WriteAttributeString("AutoPromotable", this.AutoPromotable.ToString());
            xmlWriter.WriteAttributeString("IsPrecisionArbitrary", this.IsPrecisionArbitrary.ToString().ToLowerInvariant());

            String permissionSetAsString = String.Empty;

            if (this.PermissionSet != null)
            {
                permissionSetAsString = ValueTypeHelper.JoinCollection<UserAction>(this.PermissionSet, ",");
            }

            xmlWriter.WriteAttributeString("PermissionSet", permissionSetAsString);

            #region Dependency Attribute Details

            AppendDependencyDetails(xmlWriter);

            #endregion

            #endregion write Attribute Model meta data for full attribute xml

            #region write child attribute models

            //xmlWriter.WriteStartElement("AttributeModels");

            if (AttributeModels != null)
            {
                xmlWriter.WriteRaw(this.AttributeModels.ToXml());
            }

            //Child AttributeModel node end
            //xmlWriter.WriteEndElement();

            #endregion write child attribute models

            //AttributeModel node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            returnXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return returnXml;
        }

        /// <summary>
        /// Get Xml representation of Attribute Model based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Attribute Model</returns>
        public override String ToXml(ObjectSerialization objectSerialization)
        {
            String attributeModelsXml = String.Empty;

            if (objectSerialization == ObjectSerialization.Full)
            {
                attributeModelsXml = this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);
                //xmlWriter.WriteStartDocument();

                //AttributeModel node start
                xmlWriter.WriteStartElement("AttributeModel");

                if (objectSerialization == ObjectSerialization.ProcessingOnly)
                {
                    #region write AttributeModel meta data for ProcessingOnly Xml

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("Name", this.Name);
                    xmlWriter.WriteAttributeString("LongName", this.LongName);
                    xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
                    xmlWriter.WriteAttributeString("AttributeParentId", this.AttributeParentId.ToString());
                    xmlWriter.WriteAttributeString("AttributeParentName", this.AttributeParentName);
                    xmlWriter.WriteAttributeString("AttributeParentLongName", this.AttributeParentLongName);
                    xmlWriter.WriteAttributeString("AttributeTypeId", this.AttributeTypeId.ToString());
                    xmlWriter.WriteAttributeString("AttributeTypeName", this.AttributeTypeName);
                    xmlWriter.WriteAttributeString("AttributeDataTypeId", this.AttributeDataTypeId.ToString());
                    xmlWriter.WriteAttributeString("AttributeDataTypeName", this.AttributeDataTypeName);
                    xmlWriter.WriteAttributeString("AttributeDisplayTypeId", this.AttributeDisplayTypeId.ToString());
                    xmlWriter.WriteAttributeString("AttributeDisplayTypeName", this.AttributeDisplayTypeName);
                    xmlWriter.WriteAttributeString("Action", this.Action.ToString());

                    #endregion write AttributeModel meta data for ProcessingOnly Xml
                }
                else if (objectSerialization == ObjectSerialization.UIRender)
                {
                    //Determine Max and Min value for the attribute
                    String minValue = String.Empty;
                    String maxValue = String.Empty;

                    #region write AttributeModel meta data for UIRender Xml

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("Name", this.Name);
                    xmlWriter.WriteAttributeString("LN", this.LongName);
                    xmlWriter.WriteAttributeString("PId", this.AttributeParentId.ToString());
                    xmlWriter.WriteAttributeString("PLN", this.AttributeParentLongName);
                    xmlWriter.WriteAttributeString("DTId", this.AttributeDataTypeId.ToString());
                    xmlWriter.WriteAttributeString("DTName", this.AttributeDataTypeName);
                    xmlWriter.WriteAttributeString("DSTId", this.AttributeDisplayTypeId.ToString());
                    xmlWriter.WriteAttributeString("DSTName", this.AttributeDisplayTypeName);
                    xmlWriter.WriteAttributeString("AllowVal", this.AllowableValues);
                    xmlWriter.WriteAttributeString("MaxLen", this.MaxLength.ToString());
                    xmlWriter.WriteAttributeString("MinLen", this.MinLength.ToString());
                    xmlWriter.WriteAttributeString("Req", this.Required.ToString());
                    xmlWriter.WriteAttributeString("AUOM", this.AllowableUOM.ToString());
                    xmlWriter.WriteAttributeString("DefUOM", this.DefaultUOM.ToString());
                    xmlWriter.WriteAttributeString("UOMType", this.UomType);
                    xmlWriter.WriteAttributeString("Prec", this.Precision.ToString());
                    xmlWriter.WriteAttributeString("IsCol", this.IsCollection.ToString());
                    xmlWriter.WriteAttributeString("Min", this.RangeFrom);
                    xmlWriter.WriteAttributeString("Max", this.RangeTo);
                    xmlWriter.WriteAttributeString("Exm", this.AttributeExample);
                    xmlWriter.WriteAttributeString("RO", this.ReadOnly.ToString());
                    xmlWriter.WriteAttributeString("Def", this.Definition);
                    xmlWriter.WriteAttributeString("Ext", this.Extension);
                    xmlWriter.WriteAttributeString("DefVal", this.DefaultValue);
                    xmlWriter.WriteAttributeString("Path", this.Path);
                    xmlWriter.WriteAttributeString("SortOrder", this.SortOrder.ToString());
                    xmlWriter.WriteAttributeString("Inheritable", this.Inheritable.ToString());
                    xmlWriter.WriteAttributeString("IsHidden", this.IsHidden.ToString());
                    xmlWriter.WriteAttributeString("IsCMP", this.IsComplex.ToString());
                    xmlWriter.WriteAttributeString("IsHIR", this.IsHierarchical.ToString());
                    xmlWriter.WriteAttributeString("IsLoc", this.IsLocalizable.ToString());
                    xmlWriter.WriteAttributeString("LocFormat", this.ApplyLocaleFormat.ToString());
                    xmlWriter.WriteAttributeString("TZConv", this.ApplyTimeZoneConversion.ToString());

                    #endregion write AttributeModel meta data for UIRender Xml

                    AppendDependencyDetails(xmlWriter);
                }
                else if (objectSerialization == ObjectSerialization.Compact)
                {
                    #region write AttributeModel meta data for Compact Xml

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("Name", this.Name);
                    xmlWriter.WriteAttributeString("LN", this.LongName);
                    xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
                    xmlWriter.WriteAttributeString("PId", this.AttributeParentId.ToString());
                    xmlWriter.WriteAttributeString("PName", this.AttributeParentName);
                    xmlWriter.WriteAttributeString("PLN", AttributeParentLongName);
                    xmlWriter.WriteAttributeString("TypeId", this.AttributeTypeId.ToString());
                    xmlWriter.WriteAttributeString("TypeName", this.AttributeTypeName);
                    xmlWriter.WriteAttributeString("DTId", this.AttributeDataTypeId.ToString());
                    xmlWriter.WriteAttributeString("DTName", this.AttributeDataTypeName);
                    xmlWriter.WriteAttributeString("DSTId", this.AttributeDisplayTypeId.ToString());
                    xmlWriter.WriteAttributeString("DSTName", this.AttributeDisplayTypeName);
                    xmlWriter.WriteAttributeString("AllowVal", this.AllowableValues);
                    xmlWriter.WriteAttributeString("MaxLen", this.MaxLength.ToString());
                    xmlWriter.WriteAttributeString("MinLen", this.MinLength.ToString());
                    xmlWriter.WriteAttributeString("Req", this.Required.ToString());
                    xmlWriter.WriteAttributeString("AUOM", this.AllowableUOM);
                    xmlWriter.WriteAttributeString("DefUOM", this.DefaultUOM);
                    xmlWriter.WriteAttributeString("UOMType", this.UomType);
                    xmlWriter.WriteAttributeString("Prec", this.Precision.ToString());
                    xmlWriter.WriteAttributeString("IsCol", this.IsCollection.ToString());
                    xmlWriter.WriteAttributeString("RangeTo", this.RangeTo);
                    xmlWriter.WriteAttributeString("RangeFrom", this.RangeFrom);
                    xmlWriter.WriteAttributeString("Label", this.Label);
                    xmlWriter.WriteAttributeString("Def", this.Definition);
                    xmlWriter.WriteAttributeString("Exm", this.AttributeExample);
                    xmlWriter.WriteAttributeString("BR", this.BusinessRule);
                    xmlWriter.WriteAttributeString("RO", this.ReadOnly.ToString());
                    xmlWriter.WriteAttributeString("Ext", this.Extension);
                    xmlWriter.WriteAttributeString("AssemblyName", this.AttributeAssemblyName);
                    xmlWriter.WriteAttributeString("AssemblyMethod", this.AssemblyMethod);
                    xmlWriter.WriteAttributeString("AttrRegEx", this.AttributeRegEx);
                    xmlWriter.WriteAttributeString("RegExErrMsg", this.RegExErrorMessage);
                    xmlWriter.WriteAttributeString("AssemblyClass", this.AssemblyClass);
                    xmlWriter.WriteAttributeString("LkUpTableName", this.LookUpTableName);
                    xmlWriter.WriteAttributeString("DefVal", this.DefaultValue);
                    xmlWriter.WriteAttributeString("Classification", this.Classification.ToString());
                    xmlWriter.WriteAttributeString("ComplexTableName", this.ComplexTableName);
                    xmlWriter.WriteAttributeString("RuleLkUpTable", this.RuleLookupTable);
                    xmlWriter.WriteAttributeString("RuleSP", this.RuleSP);
                    xmlWriter.WriteAttributeString("Path", this.Path);
                    xmlWriter.WriteAttributeString("Searchable", this.Searchable.ToString());
                    xmlWriter.WriteAttributeString("EnableHistory", this.EnableHistory.ToString());
                    xmlWriter.WriteAttributeString("Persists", this.Persists.ToString());
                    xmlWriter.WriteAttributeString("WebUri", this.WebUri);
                    xmlWriter.WriteAttributeString("CustomAction", this.CustomAction);
                    xmlWriter.WriteAttributeString("InitialPopulationMethod", this.InitialPopulationMethod);
                    xmlWriter.WriteAttributeString("OnClickJS", this.OnClickJavaScript);
                    xmlWriter.WriteAttributeString("OnLoadJS", this.OnLoadJavaScript);
                    xmlWriter.WriteAttributeString("LkStorageFormat", this.LkStorageFormat);
                    xmlWriter.WriteAttributeString("LkDisplayColumns", this.LookUpDisplayColumns);
                    xmlWriter.WriteAttributeString("LkSortOrder", this.LkSortOrder);
                    xmlWriter.WriteAttributeString("LkSearchColumns", this.LkSearchColumns);
                    xmlWriter.WriteAttributeString("LkDisplayFormat", this.LkDisplayFormat);
                    xmlWriter.WriteAttributeString("LkUpDisplayColumns", this.LkDisplayColumns);
                    xmlWriter.WriteAttributeString("LkupDuplicateAllowed", this.LkupDuplicateAllowed.ToString());
                    xmlWriter.WriteAttributeString("StoreLkUpReference", this.StoreLookupReference.ToString());
                    xmlWriter.WriteAttributeString("SortOrder", this.SortOrder.ToString());
                    xmlWriter.WriteAttributeString("ExportMask", this.ExportMask);
                    xmlWriter.WriteAttributeString("Inheritable", this.Inheritable.ToString());
                    xmlWriter.WriteAttributeString("IsHidden", this.IsHidden.ToString());
                    xmlWriter.WriteAttributeString("IsCMP", this.IsComplex.ToString());
                    xmlWriter.WriteAttributeString("IsHIR", this.IsHierarchical.ToString());
                    xmlWriter.WriteAttributeString("IsLookup", this.IsLookup.ToString());
                    xmlWriter.WriteAttributeString("IsLoc", this.IsLocalizable.ToString());
                    xmlWriter.WriteAttributeString("LocFormat", this.ApplyLocaleFormat.ToString());
                    xmlWriter.WriteAttributeString("TZConv", this.ApplyTimeZoneConversion.ToString());
                    xmlWriter.WriteAttributeString("AllowNullSearch", this.AllowNullSearch.ToString());
                    xmlWriter.WriteAttributeString("ShowAtCreation", this.ShowAtCreation.ToString());
                    xmlWriter.WriteAttributeString("Action", this.Action.ToString());
                    xmlWriter.WriteAttributeString("IsPrecisionArbitrary", this.IsPrecisionArbitrary.ToString().ToLowerInvariant());

                    #endregion write AttributeModel meta data for Compact Xml
                }

                #region write child attribute models

                if (AttributeModels != null)
                {
                    xmlWriter.WriteRaw(this.AttributeModels.ToXml(objectSerialization));
                }

                #endregion write child attribute models

                //AttributeModel node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //get the actual XML
                attributeModelsXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();

            }
            return attributeModelsXml;

        }

        /// <summary>
        /// Loads the object from XML
        /// </summary>
        /// <param name="attributeModelXml">Attribute Model XML</param>
        /// <param name="loadCompleteDetailsOfAttribute">Flag denoting whether to load complete details or only the main details of the attribute model</param>
        public void LoadAttributeModelFromXml(String attributeModelXml, Boolean loadCompleteDetailsOfAttribute)
        {
            /*
             * Sample - 1 When 'loadCompleteDetailsOfAttribute=false'
             * <AttributeModel Id="4003" Name="ProductID" LongName="ProductID" AttributeParentId="4002" AttributeParentName="Core Attributes" AttributeParentLongName="Core Attributes" AttributeTypeId="1" AttributeTypeName="Attribute" AttributeDataTypeName="String" AttributeDisplayTypeId="3" AttributeDisplayTypeName="TextBox" IsCollection="false" IsComplex="false" IsLocalizable="true" ApplyLocaleFormat="false" ApplyTimeZoneConversion="false" AllowNullSearch="false" />
             * 
             * Sample - 2 When 'loadCompleteDetailsOfAttribute=true'
             * <AttributeModel Id="4003" Name="ProductID" LongName="ProductID" AttributeParentId="4002" AttributeParentName="Core Attributes" AttributeParentLongName="Core Attributes" AttributeTypeId="1" AttributeTypeName="Attribute" AttributeDataTypeName="String" AttributeDisplayTypeId="3" AttributeDisplayTypeName="TextBox" AllowVal="" MaxLength="0" MinLength="0" Required="0" ReadOnlyatNode="1" AllowableUOM="" DefaultUOM="" UOMType="" Precision="0" IsCollection="false" MinInclusive="" MaxInclusive="" MinExclusive="" MaxExclusive="" Label="" Definition="" Example="" BusinessRule="" ReadOnly="1" Extension="" AssemblyName="" AssemblyMethod="" AttributeRegEx="" AssemblyClass="" LookUpTableName="" DefaultValue="" isClassification="0" ComplexTableName="" RuleLookupTable="" RuleSP="" path="Data Attributes#@#Common Attributes#@#Core Attributes#@#ProductID" Searchable="1" EnableHistory="1" ShowAtCreation="1" Persists="0" WebUri="" CustomAction="0" InitialPopulationMethod="" OnClickJavaScript="" OnLoadJavaScript="" LKStorageFormat="" LKDisplayColumns="" LKSortOrder="" LKSearchColumns="" LookUpDisplayColumns="" LkupDuplicateAllowed="0" StoreLookupReference="0" LKDisplayFormat="" SortOrder="" ExportMask="" Inheritable="1" isHidden="0" IsComplex="false" IsLocalizable="true" ApplyLocaleFormat="false" ApplyTimeZoneConversion="false" AllowNullSearch="false" />
             */

            XmlTextReader reader = null;

            try
            {
                reader = new XmlTextReader(attributeModelXml, XmlNodeType.Element, null);

                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        if (reader.HasAttributes)
                        {
                            LoadAttributeModelMetadataFromXml(reader);
                        }
                    }

                    if (reader.ReadToFollowing("AttributeModels"))
                    {
                        //TODO :: AttributeModelContext.Locales : AttributeModelCollection to take locale collection in constructor
                        this.AttributeModels = new AttributeModelCollection(this.Context.ContainerId, this.Context.EntityTypeId, this.Context.CategoryId, this.Context.Locales.FirstOrDefault(), this.Context.AttributeModelType, reader.ReadOuterXml(), loadCompleteDetailsOfAttribute);
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IAttributeModel Clone()
        {
            AttributeModel attributeModel = new AttributeModel();

            attributeModel._attributeModelBaseProperties.Id = this.Id;
            attributeModel._attributeModelBaseProperties.Name = this.Name;
            attributeModel._attributeModelBaseProperties.LongName = this.LongName;
            attributeModel.Locale = this.Locale;
            attributeModel.Action = this.Action;
            attributeModel._attributeModelBaseProperties.AuditRefId = this.AuditRefId;
            attributeModel.ExtendedProperties = this.ExtendedProperties;

            attributeModel._attributeModelBaseProperties.AllowableUOM = this.AllowableUOM;
            attributeModel.AllowableValues = this.AllowableValues;
            attributeModel._attributeModelBaseProperties.AllowNullSearch = this.AllowNullSearch;
            attributeModel._attributeModelBaseProperties.ApplyLocaleFormat = this.ApplyLocaleFormat;
            attributeModel._attributeModelBaseProperties.ApplyTimeZoneConversion = this.ApplyTimeZoneConversion;
            attributeModel._attributeModelBaseProperties.AttributeDataTypeId = this.AttributeDataTypeId;
            attributeModel._attributeModelBaseProperties.AttributeDataTypeName = this.AttributeDataTypeName;
            attributeModel._attributeModelBaseProperties.AttributeDisplayTypeId = this.AttributeDisplayTypeId;
            attributeModel._attributeModelBaseProperties.AttributeDisplayTypeName = this.AttributeDisplayTypeName;
            attributeModel._attributeModelBaseProperties.AttributeExample = this.AttributeExample;
            attributeModel._attributeModelBaseProperties.AttributeModelType = this.AttributeModelType;
            attributeModel._attributeModelBaseProperties.AttributeParentId = this.AttributeParentId;
            attributeModel._attributeModelBaseProperties.AttributeParentLongName = this.AttributeParentLongName;
            attributeModel._attributeModelBaseProperties.AttributeParentName = this.AttributeParentName;
            attributeModel._attributeModelBaseProperties.AttributeRegEx = this.AttributeRegEx;
            attributeModel._attributeModelBaseProperties.RegExErrorMessage = this.RegExErrorMessage;
            attributeModel._attributeModelBaseProperties.AttributeType = this.AttributeType;
            attributeModel._attributeModelBaseProperties.AttributeTypeId = this.AttributeTypeId;
            attributeModel._attributeModelBaseProperties.AttributeTypeName = this.AttributeTypeName;
            attributeModel._attributeModelBaseProperties.BusinessRule = this.BusinessRule;
            attributeModel._attributeModelBaseProperties.ComplexTableName = this.ComplexTableName;
            attributeModel._attributeModelBaseProperties.ComplexTableColumnNameList = ValueTypeHelper.CloneCollection(this.ComplexTableColumnNameList);
            attributeModel.Context = this.Context;
            attributeModel._attributeModelBaseProperties.DefaultUOM = this.DefaultUOM;
            attributeModel._attributeModelBaseProperties.DefaultValue = this.DefaultValue;
            attributeModel._attributeModelBaseProperties.Definition = this.Definition;
            attributeModel._attributeModelBaseProperties.EnableHistory = this.EnableHistory;
            attributeModel._attributeModelBaseProperties.ExportMask = this.ExportMask;
            attributeModel._attributeModelBaseProperties.Extension = this.Extension;
            attributeModel._attributeModelBaseProperties.Inheritable = this.Inheritable;
            attributeModel._attributeModelBaseProperties.IsCollection = this.IsCollection;
            attributeModel._attributeModelBaseProperties.IsComplex = this.IsComplex;
            attributeModel._attributeModelBaseProperties.IsComplexChild = this.IsComplexChild;
            attributeModel._attributeModelBaseProperties.IsHierarchical = this.IsHierarchical;
            attributeModel._attributeModelBaseProperties.IsHierarchicalChild = this.IsHierarchicalChild;
            attributeModel.IsHidden = this.IsHidden;
            attributeModel._attributeModelBaseProperties.IsLocalizable = this.IsLocalizable;
            attributeModel._attributeModelBaseProperties.IsLookup = this.IsLookup;
            attributeModel._attributeModelBaseProperties.Label = this.Label;
            attributeModel._attributeModelBaseProperties.LkDisplayColumns = this.LkDisplayColumns;
            attributeModel._attributeModelBaseProperties.LkDisplayFormat = this.LkDisplayFormat;
            attributeModel._attributeModelBaseProperties.LkSearchColumns = this.LkSearchColumns;
            attributeModel._attributeModelBaseProperties.LkSortOrder = this.LkSortOrder;
            attributeModel._attributeModelBaseProperties.LookUpTableName = this.LookUpTableName;
            attributeModel._attributeModelBaseProperties.MaxExclusive = this.MaxExclusive;
            attributeModel._attributeModelBaseProperties.MaxInclusive = this.MaxInclusive;
            attributeModel.MaxLength = this.MaxLength;
            attributeModel._attributeModelBaseProperties.MinExclusive = this.MinExclusive;
            attributeModel._attributeModelBaseProperties.MinInclusive = this.MinInclusive;
            attributeModel.MinLength = this.MinLength;
            attributeModel._attributeModelBaseProperties.Path = this.Path;
            attributeModel.Precision = this.Precision;
            attributeModel._attributeModelBaseProperties.RangeFrom = this.RangeFrom;
            attributeModel._attributeModelBaseProperties.RangeTo = this.RangeTo;
            attributeModel.ReadOnly = this.ReadOnly;
            attributeModel.Required = this.Required;
            attributeModel._attributeModelBaseProperties.Searchable = this.Searchable;
            attributeModel._attributeModelBaseProperties.ShowAtCreation = this.ShowAtCreation;
            attributeModel.SortOrder = this.SortOrder;
            attributeModel._attributeModelBaseProperties.UomType = this.UomType;
            attributeModel._attributeModelBaseProperties.WebUri = this.WebUri;
            attributeModel._attributeModelBaseProperties.IsPrecisionArbitrary = this.IsPrecisionArbitrary;

            #region Clone PermissionSet

            Collection<UserAction> permissionSetClone = new Collection<UserAction>();

            if (this._permissionSet != null)
            {
                foreach (UserAction userAction in this._permissionSet)
                {
                    permissionSetClone.Add(userAction);
                }
            }

            attributeModel._permissionSet = permissionSetClone;

            #endregion Clone PermissionSet

            //clone children recursive
            if (this._attributeModels != null)
            {
                foreach (AttributeModel childAttributeModel in this._attributeModels)
                {
                    AttributeModel clonedChildAttributeModel = (AttributeModel)childAttributeModel.Clone();
                    attributeModel._attributeModels.Add(clonedChildAttributeModel);
                }
            }

            #region Clone Dependency Details

            attributeModel._attributeModelBaseProperties.IsDependentAttribute = this.IsDependentAttribute;
            attributeModel._attributeModelBaseProperties.HasDependentAttribute = this.HasDependentAttribute;
            attributeModel.DependentChildAttributes = this.DependentChildAttributes;
            attributeModel.DependentParentAttributes = this.DependentParentAttributes;

            #endregion

            return attributeModel;
        }

        /// <summary>
        /// Loads AttributeModel object from xml
        /// </summary>
        /// <param name="reader">Indicates an xml reader used to read xml format data</param>
        internal void LoadAttributeModelDetailFromXml(XmlTextReader reader)
        {
            if (reader != null)
            {
                LoadAttributeModelMetadataFromXml(reader);

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "AttributeModels") // childs attribute models
                    {
                        this._attributeModels.LoadAttributeModelCollectionFromXml(reader);
                    }
                    else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "AttributeModel") // </AttributeModel>
                    {
                        return;
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("reader", "XmlTextReader is null. Can not read AttributeModel object.");
            }
        }

        /// <summary>
        /// Get the list of dependent Attributes
        /// </summary> 
        /// <returns>DependentAttributeCollection</returns>
        public DependentAttributeCollection GetDependentAttributes()
        {
            DependentAttributeCollection dAttrs = new DependentAttributeCollection();

            if (this.DependentParentAttributes != null && this.DependentParentAttributes.Count > 0)
            {
                dAttrs.AddRange(this.DependentParentAttributes);
            }

            if (this.DependentChildAttributes != null && this.DependentChildAttributes.Count > 0)
            {
                dAttrs.AddRange(this.DependentChildAttributes);
            }

            return dAttrs;
        }

        /// <summary>
        /// Get the list of Dependency attribute Id list.
        /// </summary>
        /// <returns>Returns list of dependency attribute Ids</returns>
        public List<Int32> GetDepencyAttributeIdList()
        {
            List<Int32> dependencyAttributeIdlist = new List<Int32>();

            if (this.IsDependentAttribute || this.HasDependentAttribute)
            {
                foreach (DependentAttribute attr in GetDependentAttributes())
                {
                    dependencyAttributeIdlist.Add(attr.AttributeId);
                }
            }

            return dependencyAttributeIdlist;
        }

        /// <summary>
        /// Get the collection of dependent parent Attributes
        /// </summary> 
        /// <returns>DependentAttributeCollection</returns>
        public DependentAttributeCollection GetDependentParentAttributes()
        {
            return this.DependentParentAttributes;
        }

        /// <summary>
        /// Get the collection of dependent Children Attributes
        /// </summary> 
        /// <returns>DependentAttributeCollection</returns>
        public DependentAttributeCollection GetDependentChildAttributes()
        {
            return this.DependentChildAttributes;
        }

        /// <summary>
        /// Get the range from values of an attribute based on the applyInclusiveFlag parameter
        /// </summary>
        /// <param name="applyInclusiveFlag">Flag to determine whether to include the specified value to get the range from value of attribute</param>
        /// <returns>Retuns the range from values of attribute</returns>
        public String GetRangeFrom(Boolean applyInclusiveFlag)
        {
            String rangeFrom = String.Empty;

            if (applyInclusiveFlag)
            {
                rangeFrom = this.RangeFrom;
            }
            else
            {
                if (!String.IsNullOrEmpty(this._attributeModelBaseProperties.MinInclusive))
                {
                    rangeFrom = this._attributeModelBaseProperties.MinInclusive;
                }
                if (!String.IsNullOrEmpty(this._attributeModelBaseProperties.MinExclusive))
                {
                    rangeFrom = this._attributeModelBaseProperties.MinExclusive;
                }
            }

            return rangeFrom;
        }

        /// <summary>
        /// Get the range to values of an attribute based on the applyInclusiveFlag parameter
        /// </summary>
        /// <param name="applyInclusiveFlag">Flag to determine whether to include the specified value to get the range to values of attribute</param>
        /// <returns>Returns the range to values of attribute</returns>
        public String GetRangeTo(Boolean applyInclusiveFlag)
        {
            String rangeTo = String.Empty;

            if (applyInclusiveFlag)
            {
                rangeTo = this.RangeTo;
            }
            else
            {
                if (!String.IsNullOrEmpty(this._attributeModelBaseProperties.MaxInclusive))
                {
                    rangeTo = this._attributeModelBaseProperties.MaxInclusive;
                }
                if (!String.IsNullOrEmpty(this._attributeModelBaseProperties.MaxExclusive))
                {
                    rangeTo = this._attributeModelBaseProperties.MaxExclusive;
                }
            }

            return rangeTo;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="subsetAttributeModel">The Object to compare with the current Object.</param>
        /// <param name="compareIds">Flag to determine whether id based comparison is true or not</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public Boolean IsSuperSetOf(AttributeModel subsetAttributeModel, Boolean compareIds = false)
        {
            if (subsetAttributeModel != null)
            {
                if (base.IsSuperSetOf(subsetAttributeModel, compareIds))
                {
                    if (compareIds)
                    {
                        if (this.AttributeTypeId != subsetAttributeModel.AttributeTypeId)
                            return false;

                        if (this.AttributeDataTypeId != subsetAttributeModel.AttributeDataTypeId)
                            return false;

                        if (this.AttributeDisplayTypeId != subsetAttributeModel.AttributeDisplayTypeId)
                            return false;
                    }

                    if (this.Name != subsetAttributeModel.Name)
                        return false;

                    if (this.LongName != subsetAttributeModel.LongName)
                        return false;

                    if (this.Locale != subsetAttributeModel.Locale)
                        return false;

                    if (this.AttributeParentName != subsetAttributeModel.AttributeParentName)
                        return false;

                    if (this.AttributeParentLongName != subsetAttributeModel.AttributeParentLongName)
                        return false;

                    if (this.AttributeTypeName != subsetAttributeModel.AttributeTypeName)
                        return false;

                    if (this.AttributeDataTypeName != subsetAttributeModel.AttributeDataTypeName)
                        return false;

                    if (this.AttributeDisplayTypeName != subsetAttributeModel.AttributeDisplayTypeName)
                        return false;

                    if (this.AllowableValues != subsetAttributeModel.AllowableValues)
                        return false;

                    if (this.MaxLength != subsetAttributeModel.MaxLength)
                        return false;

                    if (this.MinLength != subsetAttributeModel.MinLength)
                        return false;

                    if (this.Required != subsetAttributeModel.Required)
                        return false;

                    if (this.AllowableUOM != subsetAttributeModel.AllowableUOM)
                        return false;

                    if (this.DefaultUOM != subsetAttributeModel.DefaultUOM)
                        return false;

                    if (this.UomType != subsetAttributeModel.UomType)
                        return false;

                    if (this.Precision != subsetAttributeModel.Precision)
                        return false;

                    if (this.IsCollection != subsetAttributeModel.IsCollection)
                        return false;

                    if (this.RangeTo != subsetAttributeModel.RangeTo)
                        return false;

                    if (this.RangeFrom != subsetAttributeModel.RangeFrom)
                        return false;

                    if (this.Label != subsetAttributeModel.Label)
                        return false;

                    if (this.Definition != subsetAttributeModel.Definition)
                        return false;

                    if (this.AttributeExample != subsetAttributeModel.AttributeExample)
                        return false;

                    if (this.BusinessRule != subsetAttributeModel.BusinessRule)
                        return false;

                    if (this.ReadOnly != subsetAttributeModel.ReadOnly)
                        return false;

                    if (this.Extension != subsetAttributeModel.Extension)
                        return false;

                    if (this.AttributeAssemblyName != subsetAttributeModel.AttributeAssemblyName)
                        return false;

                    if (this.AssemblyMethod != subsetAttributeModel.AssemblyMethod)
                        return false;

                    if (this.AttributeRegEx != subsetAttributeModel.AttributeRegEx)
                        return false;

                    if (this.RegExErrorMessage != subsetAttributeModel.RegExErrorMessage)
                    {
                        return false;
                    }

                    if (this.AssemblyClass != subsetAttributeModel.AssemblyClass)
                        return false;

                    if (this.LookUpTableName != subsetAttributeModel.LookUpTableName)
                        return false;

                    if (this.DefaultValue != subsetAttributeModel.DefaultValue)
                        return false;

                    if (this.Classification != subsetAttributeModel.Classification)
                        return false;

                    if (this.ComplexTableName != subsetAttributeModel.ComplexTableName)
                        return false;

                    //if (this.ComplexTableColumnNameList != subsetAttributeModel.ComplexTableColumnNameList)
                    //    return false;

                    if (this.RuleLookupTable != subsetAttributeModel.RuleLookupTable)
                        return false;

                    if (this.RuleSP != subsetAttributeModel.RuleSP)
                        return false;

                    if (this.Path != subsetAttributeModel.Path)
                        return false;

                    if (this.Searchable != subsetAttributeModel.Searchable)
                        return false;
                    
                    if (this.EnableHistory != subsetAttributeModel.EnableHistory)
                        return false;

                    if (this.Persists != subsetAttributeModel.Persists)
                        return false;

                    if (this.WebUri != subsetAttributeModel.WebUri)
                        return false;

                    if (this.CustomAction != subsetAttributeModel.CustomAction)
                        return false;

                    if (this.InitialPopulationMethod != subsetAttributeModel.InitialPopulationMethod)
                        return false;

                    if (this.OnClickJavaScript != subsetAttributeModel.OnClickJavaScript)
                        return false;

                    if (this.OnLoadJavaScript != subsetAttributeModel.OnLoadJavaScript)
                        return false;

                    if (this.LkStorageFormat != subsetAttributeModel.LkStorageFormat)
                        return false;

                    if (this.LkDisplayColumns != subsetAttributeModel.LkDisplayColumns)
                        return false;

                    if (this.LkSortOrder != subsetAttributeModel.LkSortOrder)
                        return false;

                    if (this.LkSearchColumns != subsetAttributeModel.LkSearchColumns)
                        return false;

                    if (this.LkDisplayFormat != subsetAttributeModel.LkDisplayFormat)
                        return false;

                    if (this.LookUpDisplayColumns != subsetAttributeModel.LookUpDisplayColumns)
                        return false;

                    if (this.LkupDuplicateAllowed != subsetAttributeModel.LkupDuplicateAllowed)
                        return false;

                    if (this.StoreLookupReference != subsetAttributeModel.StoreLookupReference)
                        return false;

                    if (this.SortOrder != subsetAttributeModel.SortOrder)
                        return false;

                    if (this.ExportMask != subsetAttributeModel.ExportMask)
                        return false;

                    if (this.Inheritable != subsetAttributeModel.Inheritable)
                        return false;

                    if (this.IsHidden != subsetAttributeModel.IsHidden)
                        return false;

                    if (this.IsComplex != subsetAttributeModel.IsComplex)
                        return false;

                    if (this.IsComplexChild != subsetAttributeModel.IsComplexChild)
                        return false;

                    if (this.IsHierarchical != subsetAttributeModel.IsHierarchical)
                        return false;

                    if (this.IsHierarchicalChild != subsetAttributeModel.IsHierarchicalChild)
                        return false;

                    if (this.IsLookup != subsetAttributeModel.IsLookup)
                        return false;

                    if (this.IsLocalizable != subsetAttributeModel.IsLocalizable)
                        return false;

                    if (this.ApplyLocaleFormat != subsetAttributeModel.ApplyLocaleFormat)
                        return false;

                    if (this.ApplyTimeZoneConversion != subsetAttributeModel.ApplyTimeZoneConversion)
                        return false;

                    if (this.AllowNullSearch != subsetAttributeModel.AllowNullSearch)
                        return false;

                    if (this.ShowAtCreation != subsetAttributeModel.ShowAtCreation)
                        return false;

                    if (this.InheritableOnly != subsetAttributeModel.InheritableOnly)
                        return false;

                    if (!this.AttributeModels.IsSuperSetOf(subsetAttributeModel.AttributeModels))
                        return false;

                    if (this.IsPrecisionArbitrary != subsetAttributeModel.IsPrecisionArbitrary)
                        return false;
                             
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subsetAttributeModel"></param>
        /// <param name="compareIds"></param>
        /// <returns></returns>
        public OperationResult GetSuperSetOfOperationResult(AttributeModel subsetAttributeModel, Boolean compareIds = false)
        {
            var operationResult = new OperationResult(); 

            if (subsetAttributeModel != null)
            {
               
                if (base.IsSuperSetOf(subsetAttributeModel, compareIds))
                {
                    #region compare Ids

                    if (compareIds)
                    {
                        Utility.BusinessObjectPropertyCompare("AttributeTypeId", this.AttributeTypeId, subsetAttributeModel.AttributeTypeId, operationResult);

                        Utility.BusinessObjectPropertyCompare("AttributeDataTypeId", this.AttributeDataTypeId, subsetAttributeModel.AttributeDataTypeId, operationResult);

                        Utility.BusinessObjectPropertyCompare("AttributeDisplayTypeId", this.AttributeDisplayTypeId, subsetAttributeModel.AttributeDisplayTypeId, operationResult);
                    }

                    #endregion compare Ids

                    #region compare properties

                    Utility.BusinessObjectPropertyCompare("Name", this.Name, subsetAttributeModel.Name, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("LongName", this.LongName, subsetAttributeModel.LongName, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("Locale", (Int32) this.Locale,(Int32) subsetAttributeModel.Locale, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("AttributeParentName", this.AttributeParentName, subsetAttributeModel.AttributeParentName, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("AttributeParentLongName", this.AttributeParentLongName, subsetAttributeModel.AttributeParentLongName, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("AttributeTypeName", this.AttributeTypeName, subsetAttributeModel.AttributeTypeName, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("AttributeDataTypeName", this.AttributeDataTypeName, subsetAttributeModel.AttributeDataTypeName, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("AttributeDisplayTypeName", this.AttributeDisplayTypeName, subsetAttributeModel.AttributeDisplayTypeName, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("AllowableValues", this.AllowableValues, subsetAttributeModel.AllowableValues, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("MaxLength", (Int32)this.MaxLength, (Int32)subsetAttributeModel.MaxLength, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("MinLength", (Int32)this.MinLength, (Int32)subsetAttributeModel.MinLength, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("Required", this.Required.ToString(), subsetAttributeModel.Required.ToString(), operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("LongName", this.LongName, subsetAttributeModel.LongName, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("DefaultUOM", this.DefaultUOM, subsetAttributeModel.DefaultUOM, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("UomType", this.UomType, subsetAttributeModel.UomType, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("Precision", (Int32)this.Precision, (Int32)subsetAttributeModel.Precision, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("IsCollection", this.IsCollection.ToString(), subsetAttributeModel.IsCollection.ToString(), operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("RangeTo", this.RangeTo, subsetAttributeModel.RangeTo, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("RangeFrom", this.RangeFrom, subsetAttributeModel.RangeFrom, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("Label", this.Label, subsetAttributeModel.Label, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("Definition", this.Definition, subsetAttributeModel.Definition, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("AttributeExample", this.AttributeExample, subsetAttributeModel.AttributeExample, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("BusinessRule", this.BusinessRule, subsetAttributeModel.BusinessRule, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("ReadOnly", this.ReadOnly.ToString(), subsetAttributeModel.ReadOnly.ToString(), operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("Extension", this.Extension, subsetAttributeModel.Extension, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("AttributeAssemblyName", this.AttributeAssemblyName, subsetAttributeModel.AttributeAssemblyName, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("AssemblyMethod", this.AssemblyMethod, subsetAttributeModel.AssemblyMethod, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("AttributeRegEx", this.AttributeRegEx, subsetAttributeModel.AttributeRegEx, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("AssemblyClass", this.AssemblyClass, subsetAttributeModel.AssemblyClass, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("LookUpTableName", this.LookUpTableName, subsetAttributeModel.LookUpTableName, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("DefaultValue", this.DefaultValue, subsetAttributeModel.DefaultValue, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("Classification", this.Classification.ToString(), subsetAttributeModel.Classification.ToString(), operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("ComplexTableName", this.ComplexTableName, subsetAttributeModel.ComplexTableName, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("RuleLookupTable", this.RuleLookupTable, subsetAttributeModel.RuleLookupTable, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("RuleSP", this.RuleSP, subsetAttributeModel.RuleSP, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("Path", this.Path, subsetAttributeModel.Path, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("Searchable", this.Searchable.ToString(), subsetAttributeModel.Searchable.ToString(), operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("EnableHistory", this.EnableHistory.ToString(), subsetAttributeModel.EnableHistory.ToString(), operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("Persists", this.Persists.ToString(), subsetAttributeModel.Persists.ToString(), operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("WebUri", this.WebUri, subsetAttributeModel.Path, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("CustomAction", this.CustomAction, subsetAttributeModel.CustomAction, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("InitialPopulationMethod", this.InitialPopulationMethod, subsetAttributeModel.InitialPopulationMethod, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("OnClickJavaScript", this.OnClickJavaScript, subsetAttributeModel.OnClickJavaScript, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("OnLoadJavaScript", this.OnLoadJavaScript, subsetAttributeModel.OnLoadJavaScript, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("LkStorageFormat", this.LkStorageFormat, subsetAttributeModel.LkStorageFormat, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("LkDisplayColumns", this.LkDisplayColumns, subsetAttributeModel.LkDisplayColumns, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("LkSortOrder", this.LkSortOrder, subsetAttributeModel.LkSortOrder, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("LkSearchColumns", this.LkSearchColumns, subsetAttributeModel.LkSearchColumns, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("LkDisplayFormat", this.LkDisplayFormat, subsetAttributeModel.LkDisplayFormat, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("LookUpDisplayColumns", this.LookUpDisplayColumns, subsetAttributeModel.LookUpDisplayColumns, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("LkupDuplicateAllowed", this.LkupDuplicateAllowed.ToString(), subsetAttributeModel.LkupDuplicateAllowed.ToString(), operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("StoreLookupReference", this.StoreLookupReference.ToString(), subsetAttributeModel.StoreLookupReference.ToString(), operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("LongName", this.SortOrder, subsetAttributeModel.SortOrder, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("ExportMask", this.ExportMask, subsetAttributeModel.ExportMask, operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("Inheritable", this.Inheritable.ToString(), subsetAttributeModel.Inheritable.ToString(), operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("IsHidden", this.IsHidden.ToString(), subsetAttributeModel.IsHidden.ToString(), operationResult);

                    Utility.BusinessObjectPropertyCompare("IsComplex", this.IsComplex.ToString(), subsetAttributeModel.IsComplex.ToString(), operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("IsComplexChild", this.IsComplexChild.ToString(), subsetAttributeModel.IsComplexChild.ToString(), operationResult);

                    Utility.BusinessObjectPropertyCompare("IsHierarchical", this.IsHierarchical.ToString(), subsetAttributeModel.IsHierarchical.ToString(), operationResult);

                    Utility.BusinessObjectPropertyCompare("IsHierarchicalChild", this.IsHierarchicalChild.ToString(), subsetAttributeModel.IsHierarchicalChild.ToString(), operationResult);

                    Utility.BusinessObjectPropertyCompare("IsLookup", this.IsLookup.ToString(), subsetAttributeModel.IsLookup.ToString(), operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("IsLocalizable", this.IsLocalizable.ToString(), subsetAttributeModel.IsLocalizable.ToString(), operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("ApplyLocaleFormat", this.ApplyLocaleFormat.ToString(), subsetAttributeModel.ApplyLocaleFormat.ToString(), operationResult);

                    Utility.BusinessObjectPropertyCompare("ApplyTimeZoneConversion", this.ApplyTimeZoneConversion.ToString(), subsetAttributeModel.ApplyTimeZoneConversion.ToString(), operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("AllowNullSearch", this.AllowNullSearch.ToString(), subsetAttributeModel.AllowNullSearch.ToString(), operationResult);
                    
                    Utility.BusinessObjectPropertyCompare("ShowAtCreation", this.ShowAtCreation.ToString(), subsetAttributeModel.ShowAtCreation.ToString(), operationResult);

                    Utility.BusinessObjectPropertyCompare("InheritableOnly", this.InheritableOnly.ToString(), subsetAttributeModel.InheritableOnly.ToString(), operationResult);

                    //Utility.BusinessObjectPropertyCompare("IsComplex", this.IsComplex.ToString(), subsetAttributeModel.IsComplex.ToString(), operationResult);
                    
                    #endregion compare properties

                    #region compare sub attributemodels

                    this.AttributeModels.GetSuperSetOperationResult(subsetAttributeModel.AttributeModels, operationResult, compareIds);

                    #endregion compare sub attributemodels

                    return operationResult;
                }
            }
            else
            {
                operationResult.AddOperationResult("-1", string.Format("SubsetAttributeModel {0} is null", subsetAttributeModel.Name), OperationResultType.Error);
                return operationResult;
            }

            operationResult.RefreshOperationResultStatus();

            return operationResult;
        }

        /// <summary>
        /// Delta Merge of attribute model
        /// </summary>
        /// <param name="deltaAttributeModel">Attribute Model that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="returnClonedObject">true means return new merged cloned object;otherwise, same object.</param>
        /// <returns>Merged attribute model instance</returns>
        public IAttributeModel MergeDelta(IAttributeModel deltaAttributeModel, ICallerContext iCallerContext, Boolean returnClonedObject = true)
        {
            IAttributeModel mergedAttributeModel = (returnClonedObject == true) ? deltaAttributeModel.Clone() : deltaAttributeModel;

            mergedAttributeModel.Action = (mergedAttributeModel.Equals(this)) ? ObjectAction.Read : ObjectAction.Update;

            return mergedAttributeModel;
        }

        /// <summary>
        /// Get attribute model type for data model.
        /// </summary>
        /// <returns></returns>
        public String GetAttributeModelTypeForDataModel()
        {
            String attributeModelType = String.Empty;

            switch (this.AttributeModelType)
            {
                case AttributeModelType.AttributeGroup:
                case AttributeModelType.CommonAttributeGroup:
                case AttributeModelType.CategoryAttributeGroup:
                case AttributeModelType.RelationshipAttributeGroup:
                    if (this.AttributeParentName.Equals("Common Attributes"))
                        attributeModelType = "Common Attribute Group";
                    else if (this.AttributeParentName.Equals("Category Specific"))
                        attributeModelType = "Category Attribute Group";
                    else if (this.AttributeParentName.Equals("Relationship Attributes"))
                        attributeModelType = "Relationship Attribute Group";
                    break;

                default:
                    attributeModelType = this.AttributeModelType.ToString();
                    break;
            }

            return attributeModelType;
        }

        #region IDataModelObject Methods

        /// <summary>
        /// Get IDataModelObject for current object.
        /// </summary>
        /// <returns>IDataModelObject</returns>
        public IDataModelObject GetDataModelObject()
        {
            return this;
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlWriter"></param>
        private void AppendDependencyDetails(XmlTextWriter xmlWriter)
        {
            xmlWriter.WriteAttributeString("IsDependentAttribute", this.IsDependentAttribute.ToString());
            xmlWriter.WriteAttributeString("HasDependentAttribute", this.HasDependentAttribute.ToString());

            if (this.HasDependentAttribute)
            {
                xmlWriter.WriteStartElement("DependentChildAttributes");

                if (this.DependentChildAttributes != null)
                {
                    xmlWriter.WriteRaw(this.DependentChildAttributes.ToXml());
                }

                xmlWriter.WriteEndElement();
            }

            if (this.IsDependentAttribute)
            {
                xmlWriter.WriteStartElement("DependentParentAttributes");

                if (this.DependentParentAttributes != null)
                {
                    xmlWriter.WriteRaw(this.DependentParentAttributes.ToXml());
                }

                xmlWriter.WriteEndElement();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="childAttributeModelBaseProperties"></param>
        /// <param name="childAttributeModelLocaleProperties"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        private AttributeModelCollection ConstructChildAttributeModels(AttributeModelBasePropertiesCollection childAttributeModelBaseProperties, AttributeModelLocalePropertiesCollection childAttributeModelLocaleProperties, LocaleEnum locale)
        {
            AttributeModelCollection childAttributeModels = new AttributeModelCollection();

            if (childAttributeModelBaseProperties != null && childAttributeModelBaseProperties.Count > 0)
            {
                for (Int32 index = 0; index < childAttributeModelBaseProperties.Count; index++)
                {
                    AttributeModel childAttributeModel = new AttributeModel();
                    childAttributeModel._attributeModelBaseProperties = childAttributeModelBaseProperties.ElementAt(index);
                    childAttributeModel.Locale = locale;

                    AttributeModelBasePropertiesCollection grandchildAttributeModelBaseProperties = null;
                    AttributeModelLocalePropertiesCollection grandchildAttributeModelLocaleProperties = null;

                    if (childAttributeModel._attributeModelBaseProperties != null)
                    {
                        grandchildAttributeModelBaseProperties = childAttributeModel._attributeModelBaseProperties.ChildAttributeModelBaseProperties;
                    }

                    Int32 attributeId = childAttributeModel._attributeModelBaseProperties.Id;

                    if (childAttributeModelLocaleProperties != null && childAttributeModelLocaleProperties.Count > 0)
                    {
                        childAttributeModel._attributeModelLocaleProperties = childAttributeModelLocaleProperties[attributeId];
                        childAttributeModel._hasLocaleProperties = true;

                        if (childAttributeModel._attributeModelLocaleProperties != null)
                        {
                            grandchildAttributeModelLocaleProperties = childAttributeModel._attributeModelLocaleProperties.ChildAttributeModelLocaleProperties;
                        }
                    }

                    childAttributeModels.Add(childAttributeModel);

                    if ((grandchildAttributeModelBaseProperties != null && grandchildAttributeModelBaseProperties.Count > 0) ||
                        (grandchildAttributeModelLocaleProperties != null && grandchildAttributeModelLocaleProperties.Count > 0))
                    {
                        childAttributeModel._attributeModels = ConstructChildAttributeModels(grandchildAttributeModelBaseProperties, grandchildAttributeModelLocaleProperties, locale);
                    }
                }
            }

            return childAttributeModels;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void SetOverridenProperties<T>(String key, T value)
        {
            if (!String.IsNullOrWhiteSpace(key) && value != null)
            {
                if (this._overridenProperties == null)
                    this._overridenProperties = new Dictionary<String, String>();

                if (this._overridenProperties.ContainsKey(key))
                {
                    //If user is trying to set overridden properties again then remove from dictionary and add it again.
                    this._overridenProperties.Remove(key);
                }

                this._overridenProperties.Add(key, value.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        private T GetOverridenProperties<T>(String key)
        {
            T returnVal = default(T);

            if (!String.IsNullOrWhiteSpace(key))
            {
                returnVal = (T)Convert.ChangeType(this._overridenProperties[key], typeof(T));
            }

            return returnVal;
        }

        /// <summary>
        /// Loads properties of AttributeModel from xml
        /// </summary>
        /// <param name="reader">Indicates an xml reader used to read xml format data</param>
        private void LoadAttributeModelMetadataFromXml(XmlTextReader reader)
        {
            if (reader.MoveToAttribute("Id"))
            {
                this._attributeModelBaseProperties.Id = reader.ReadContentAsInt();
            }

            if (reader.MoveToAttribute("Name"))
            {
                this._attributeModelBaseProperties.Name = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("LongName"))
            {
                this._attributeModelBaseProperties.LongName = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("Locale"))
            {
                LocaleEnum locale = LocaleEnum.UnKnown;
                Enum.TryParse<LocaleEnum>(reader.ReadContentAsString(), true, out locale);
                this.Locale = locale;
            }

            if (reader.MoveToAttribute("AttributeParentId"))
            {
                this._attributeModelBaseProperties.AttributeParentId = reader.ReadContentAsInt();
            }

            if (reader.MoveToAttribute("AttributeParentName"))
            {
                this._attributeModelBaseProperties.AttributeParentName = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("AttributeParentLongName"))
            {
                this._attributeModelBaseProperties.AttributeParentLongName = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("AttributeTypeId"))
            {
                this._attributeModelBaseProperties.AttributeTypeId = reader.ReadContentAsInt();
            }

            if (reader.MoveToAttribute("IsCollection"))
            {
                this._attributeModelBaseProperties.IsCollection = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
            }

            if (reader.MoveToAttribute("AttributeTypeName"))
            {
                this._attributeModelBaseProperties.AttributeTypeName = reader.ReadContentAsString();
                this._attributeModelBaseProperties.AttributeModelType = Utility.GetAttributeModelTypeFromAttributeTypeName(this.AttributeTypeName);
            }

            if (reader.MoveToAttribute("RegExErrorMessage"))
            {
                this._attributeModelBaseProperties.RegExErrorMessage = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("AttributeDataTypeId"))
            {
                this._attributeModelBaseProperties.AttributeDataTypeId = reader.ReadContentAsInt();
            }

            if (reader.MoveToAttribute("AttributeDataTypeName"))
            {
                this._attributeModelBaseProperties.AttributeDataTypeName = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("AttributeDisplayTypeId"))
            {
                this._attributeModelBaseProperties.AttributeDisplayTypeId = reader.ReadContentAsInt();
            }

            if (reader.MoveToAttribute("AttributeDisplayTypeName"))
            {
                this._attributeModelBaseProperties.AttributeDisplayTypeName = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("IsComplex"))
            {
                this._attributeModelBaseProperties.IsComplex = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
            }

            if (reader.MoveToAttribute("IsComplexChild"))
            {
                this._attributeModelBaseProperties.IsComplexChild = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
            }

            if (reader.MoveToAttribute("IsHierarchical"))
            {
                this._attributeModelBaseProperties.IsHierarchical = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
            }

            if (reader.MoveToAttribute("IsHierarchicalChild"))
            {
                this._attributeModelBaseProperties.IsHierarchicalChild = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
            }

            if (reader.MoveToAttribute("IsLocalizable"))
            {
                this._attributeModelBaseProperties.IsLocalizable = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
            }

            if (reader.MoveToAttribute("ApplyLocaleFormat"))
            {
                this._attributeModelBaseProperties.ApplyLocaleFormat = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
            }

            if (reader.MoveToAttribute("ApplyTimeZoneConversion"))
            {
                this._attributeModelBaseProperties.ApplyTimeZoneConversion = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
            }

            if (reader.MoveToAttribute("AllowNullSearch"))
            {
                this._attributeModelBaseProperties.AllowNullSearch = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
            }

            if (reader.MoveToAttribute("AllowVal") || reader.MoveToAttribute("AllowValues"))
            {
                this.AllowableValues = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("MaxLength"))
            {
                this.MaxLength = reader.ReadContentAsInt();
            }

            if (reader.MoveToAttribute("MinLength"))
            {
                this.MinLength = reader.ReadContentAsInt();
            }

            if (reader.MoveToAttribute("Required"))
            {
                this.Required = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
            }

            if (reader.MoveToAttribute("AllowableUOM"))
            {
                this._attributeModelBaseProperties.AllowableUOM = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("DefaultUOM"))
            {
                this._attributeModelBaseProperties.DefaultUOM = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("UomType"))
            {
                this._attributeModelBaseProperties.UomType = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("Precision"))
            {
                this.Precision = reader.ReadContentAsInt();
            }

            if (reader.MoveToAttribute("MaxExclusive"))
            {
                this._attributeModelBaseProperties.MaxExclusive = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("MaxInclusive"))
            {
                this._attributeModelBaseProperties.MaxInclusive = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("MinInclusive"))
            {
                this._attributeModelBaseProperties.MinInclusive = reader.ReadContentAsString();
            }
            if (reader.MoveToAttribute("MinExclusive"))
            {
                this._attributeModelBaseProperties.MinExclusive = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("Label"))
            {
                this._attributeModelBaseProperties.Label = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("Definition"))
            {
                this._attributeModelBaseProperties.Definition = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("BusinessRule"))
            {
                this._attributeModelBaseProperties.BusinessRule = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("ReadOnly"))
            {
                this.ReadOnly = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
            }

            if (reader.MoveToAttribute("Extension"))
            {
                this._attributeModelBaseProperties.Extension = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("AttributeRegEx"))
            {
                this._attributeModelBaseProperties.AttributeRegEx = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("LookUpTableName"))
            {
                this._attributeModelBaseProperties.LookUpTableName = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("DefaultValue"))
            {
                this._attributeModelBaseProperties.DefaultValue = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("ComplexTableName"))
            {
                this._attributeModelBaseProperties.ComplexTableName = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("ComplexTableColumnNameList"))
            {
                this._attributeModelBaseProperties.ComplexTableColumnNameList = ValueTypeHelper.SplitStringToStringCollection(reader.ReadContentAsString(), ',');
            }

            if (reader.MoveToAttribute("Path"))
            {
                this._attributeModelBaseProperties.Path = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("Searchable"))
            {
                this._attributeModelBaseProperties.Searchable = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
            }
            
            if (reader.MoveToAttribute("EnableHistory"))
            {
                this._attributeModelBaseProperties.EnableHistory = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
            }

            if (reader.MoveToAttribute("ShowAtCreation"))
            {
                this._attributeModelBaseProperties.ShowAtCreation = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
            }

            if (reader.MoveToAttribute("WebUri"))
            {
                this._attributeModelBaseProperties.WebUri = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("LKDisplayColumns") || reader.MoveToAttribute("LkDisplayColumns"))
            {
                this._attributeModelBaseProperties.LkDisplayColumns = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("LKSortOrder") || reader.MoveToAttribute("LkSortOrder"))
            {
                this._attributeModelBaseProperties.LkSortOrder = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("LKSearchColumns") || reader.MoveToAttribute("LkSearchColumns"))
            {
                this._attributeModelBaseProperties.LkSearchColumns = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("LKDisplayFormat") || reader.MoveToAttribute("LkDisplayFormat"))
            {
                this._attributeModelBaseProperties.LkDisplayFormat = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("SortOrder"))
            {
                this.SortOrder = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0); //sort order may comes as empty
            }

            if (reader.MoveToAttribute("ExportMask"))
            {
                this._attributeModelBaseProperties.ExportMask = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("Inheritable"))
            {
                this._attributeModelBaseProperties.Inheritable = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
            }

            if (reader.MoveToAttribute("IsHidden"))
            {
                this.IsHidden = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
            }

            if (reader.MoveToAttribute("PermissionSet"))
            {
                this.PermissionSet = ValueTypeHelper.SplitStringToUserActionCollection(reader.ReadContentAsString(), ',');
            }

            if (this.AttributeType == AttributeTypeEnum.Complex || this.AttributeType == AttributeTypeEnum.ComplexCollection)
            {
                this._attributeModelBaseProperties.IsComplex = true;
            }

            if (this.AttributeDisplayTypeName.ToLower() == "lookuptable")
            {
                this._attributeModelBaseProperties.IsLookup = true;
            }

            if (reader.MoveToAttribute("HasDependentAttribute"))
            {
                this._attributeModelBaseProperties.HasDependentAttribute = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
            }

            if (reader.MoveToAttribute("IsDependentAttribute"))
            {
                this._attributeModelBaseProperties.IsDependentAttribute = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
            }

            if (reader.MoveToAttribute("DependentChildAttributes"))
            {
                this._dependentChildAttributes = new DependentAttributeCollection(reader.ReadContentAsString());
            }

            if (reader.MoveToAttribute("DependentParentAttributes"))
            {
                this._dependentParentAttributes = new DependentAttributeCollection(reader.ReadContentAsString());
            }

            if (reader.MoveToAttribute("IsPrecisionArbitrary"))
            {
                this._attributeModelBaseProperties.IsPrecisionArbitrary = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
            }

            if (reader.MoveToAttribute("Action"))
            {
                this.Action = ValueTypeHelper.GetAction(reader.ReadContentAsString());
            }

        }

        #endregion

        #region IAttributeModel Methods

        /// <summary>
        /// Get child attribute models for current attribute model
        /// </summary>
        /// <returns>AttributeModelCollection interface</returns>
        /// <exception cref="NullReferenceException">Raised when child attribute models are null</exception>
        public IAttributeModelCollection GetChildAttributeModels()
        {
            if (this._attributeModels == null)
            {
                throw new NullReferenceException("Child Attribute Models are null");
            }

            return this._attributeModels;
        }

        /// <summary>
        /// Get context for current attribute model
        /// </summary>
        /// <returns>AttributeModelContext interface</returns>
        /// <exception cref="NullReferenceException">Raised when context for current model is null</exception>
        public IAttributeModelContext GetContext()
        {
            if (this.Context == null)
            {
                throw new NullReferenceException("Context is null.");
            }
            return (IAttributeModelContext)this.Context;
        }

        /// <summary>
        /// Get attribute id and its locale pair
        /// </summary>
        /// <returns>KeyValuePair of Attribute id and its respective Locale</returns>
        public KeyValuePair<Int32, LocaleEnum> GetAttributeModelIdLocalePair()
        {
            return new KeyValuePair<int, LocaleEnum>(this.Id, this.Locale);
        }

        /// <summary>
        /// Set locale of current attribute model and all its child attribute models
        /// </summary>
        /// <param name="locale"></param>
        public void SetLocale(LocaleEnum locale)
        {
            if (locale != LocaleEnum.UnKnown && locale != LocaleEnum.Neutral)
            {
                this.Locale = locale;
                if (this.AttributeModels != null)
                {
                    foreach (AttributeModel childModel in this.AttributeModels)
                    {
                        childModel.SetLocale(locale);
                    }
                }
            }
        }

        #endregion IAttributeMethods

        #endregion
    }
}