using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the CategoryAttributeMapping
    /// </summary>
    [DataContract]
    public class CategoryAttributeMapping : MDMObject, ICategoryAttributeMapping, IDataModelObject
    {
        #region Fields

        /// <summary>
        /// Indicates attribute model mapping properties.
        /// </summary>
        [DataMember(Order = 0)]
        private AttributeModelMappingProperties _attributeModelMappingProperties = new AttributeModelMappingProperties();

        /// <summary>
        /// Indicates attribute id.
        /// </summary>
        private Int32 _attributeId = 0;

        /// <summary>
        /// Indicates attribute id.
        /// </summary>
        private Int32 _attributeParentId = 0;

        /// <summary>
        /// Indicates attribute parent name.
        /// </summary>
        private String _attributeParentName = String.Empty;

        /// <summary>
        /// Indicates attribute parent long name.
        /// </summary>
        private String _attributeParentLongName = String.Empty;

        /// <summary>
        /// Field denoting Attribute DataType name
        /// </summary>
        private String _attributeDataTypeName = String.Empty;

        /// <summary>
        /// The _hierarchy identifier
        /// </summary>
        private Int32 _hierarchyId;

        /// <summary>
        /// The _hierarchy short name.
        /// </summary>
        private String _hierarchyName = String.Empty;

        /// <summary>
        /// The _hierarchy long name.
        /// </summary>
        private String _hierarchyLongName = String.Empty;

        /// <summary>
        /// The _catalog identifier
        /// </summary>
        private Int32 _catalogId;

        /// <summary>
        /// The category identifier
        /// </summary>
        private Int64 _categoryId;

        /// <summary>
        /// The _category short name
        /// </summary>
        private String _categoryName = String.Empty;

        /// <summary>
        /// The _category long name
        /// </summary>
        private String _categoryLongName = String.Empty;

        /// <summary>
        /// The _parent entity identifier
        /// </summary>
        private Int32 _parentEntityId;

        /// <summary>
        /// The Category Path
        /// </summary>
        private String _path = String.Empty;

        /// <summary>
        /// The _is inheritable
        /// </summary>
        private Boolean _isInheritable;

        /// <summary>
        /// The _dni node characteristic template identifier
        /// </summary>
        private Int32 _dniNodeCharacteristicTemplateId;

        /// <summary>
        /// Field for the is mandatory
        /// </summary>
        private String _mandatoryFlag = String.Empty;

        /// <summary>
        /// Field for the source flag
        /// </summary>
        private String _sourceFlag = String.Empty;

        /// <summary>
        /// The _is draft
        /// </summary>
        private Boolean _isDraft;

        /// <summary>
        /// Field denoting the sort order property of mapping
        /// </summary>
        private CategoryAttributeMapping _originalCategoryAttributeMapping = null;

        /// <summary>
        /// Field denoting the sort order property of mapping
        /// </summary>
        private String _externalId = String.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public CategoryAttributeMapping()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Identity of a Category - Attribute Mapping</param>
        public CategoryAttributeMapping(Int32 id) :
            base(id)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryAttributeMapping"/> class from XML.
        /// </summary>
        /// <param name="valueAsXml">The value as XML.</param>
        public CategoryAttributeMapping(String valueAsXml)
            : this()
        {
            LoadHierarchyAttributeMappingFromXml(valueAsXml);
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
                return "CategoryAttributeMapping";
            }
        }

        /// <summary>
        /// Gets or sets the id of the attribute.
        /// </summary>
        /// <value>
        /// The id of the attribute.
        /// </value>
        [DataMember]
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
        /// Gets or sets the short name of the attribute.
        /// </summary>
        /// <value>
        /// The name of the attribute.
        /// </value>
        public String AttributeName
        {
            get
            {
                return this.Name;
            }
            set
            {
                this.Name = value;
            }
        }

        /// <summary>
        /// Gets or sets the long name of the attribute.
        /// </summary>
        /// <value>
        /// The name of the attribute.
        /// </value>
        public String AttributeLongName
        {
            get
            {
                return this.LongName;
            }
            set
            {
                this.LongName = value;
            }
        }

        /// <summary>
        /// Gets or sets the parent id of the attribute.
        /// </summary>
        /// <value>
        /// The parent id of the attribute.
        /// </value>
        [DataMember]
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
        /// Gets or sets the parent name of the attribute.
        /// </summary>
        /// <value>
        /// The parent name of the attribute.
        /// </value>
        [DataMember]
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
        /// Gets or sets the parent long name of the attribute.
        /// </summary>
        /// <value>
        /// The parent long name of the attribute.
        /// </value>
        [DataMember]
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
        /// Property denoting Attribute DataType name
        /// </summary>
        [DataMember]
        public String AttributeDataTypeName
        {
            get
            {
                return this._attributeDataTypeName;
            }
            set
            {
                this._attributeDataTypeName = value;
            }
        }

        /// <summary>
        /// Gets or sets the hierarchy identifier.
        /// </summary>
        /// <value>
        /// The hierarchy identifier.
        /// </value>
        [DataMember]
        public Int32 HierarchyId
        {
            get { return _hierarchyId; }
            set { _hierarchyId = value; }
        }

        /// <summary>
        /// Gets or sets the Short name of the hierarchy.
        /// </summary>
        /// <value>
        /// The name of the hierarchy.
        /// </value>
        [DataMember]
        public String HierarchyName
        {
            get { return _hierarchyName; }
            set { _hierarchyName = value; }
        }

        /// <summary>
        /// Gets or sets the Long name of the hierarchy.
        /// </summary>
        /// <value>
        /// The name of the long hierarchy.
        /// </value>
        [DataMember]
        public String HierarchyLongName
        {
            get { return _hierarchyLongName; }
            set { _hierarchyLongName = value; }
        }

        /// <summary>
        /// Gets or sets the catalog identifier.
        /// </summary>
        /// <value>
        /// The catalog identifier.
        /// </value>
        [DataMember]
        public Int32 CatalogId
        {
            get { return _catalogId; }
            set { _catalogId = value; }
        }

        /// <summary>
        /// Gets or sets the category id.
        /// </summary>
        /// <value>
        /// The category id.
        /// </value>
        [DataMember]
        public Int64 CategoryId
        {
            get { return _categoryId; }
            set { _categoryId = value; }
        }

        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        /// <value>
        /// The name of the category.
        /// </value>
        [DataMember]
        public String CategoryName
        {
            get { return _categoryName; }
            set { _categoryName = value; }
        }

        /// <summary>
        /// Gets or sets the Long name of the category.
        /// </summary>
        /// <value>
        /// The name of the category.
        /// </value>
        [DataMember]
        public String CategoryLongName
        {
            get { return _categoryLongName; }
            set { _categoryLongName = value; }
        }

        /// <summary>
        /// Gets or sets the c node parent identifier.
        /// </summary>
        /// <value>
        /// The c node parent identifier.
        /// </value>
        [DataMember]
        public Int32 ParentEntityId
        {
            get { return _parentEntityId; }
            set { _parentEntityId = value; }
        }

        /// <summary>
        /// Field specifying the Category Path
        /// </summary>
        [DataMember]
        public String Path
        {
            get { return _path; }
            set { _path = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [is inheritable].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is inheritable]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public Boolean IsInheritable
        {
            get { return _isInheritable; }
            set { _isInheritable = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [is mandatory].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is mandatory]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public String MandatoryFlag
        {
            get { return _mandatoryFlag; }
            set { _mandatoryFlag = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [is draft].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is draft]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public Boolean IsDraft
        {
            get { return _isDraft; }
            set { _isDraft = value; }
        }

        /// <summary>
        /// Gets or sets the source flag.
        /// </summary>
        /// <value>
        /// The source flag.
        /// </value>
        [DataMember]
        public String SourceFlag
        {
            get { return _sourceFlag; }
            set { _sourceFlag = value; }
        }

        /// <summary>
        /// Property denoting sort order of attribute
        /// </summary>
        public Int32? SortOrder
        {
            get
            {
                return this._attributeModelMappingProperties.SortOrder;
            }
            set
            {
                this._attributeModelMappingProperties.SortOrder = value;
            }
        }

        /// <summary>
        /// Property denoting if attribute value is Required
        /// </summary>
        public Boolean? Required
        {
            get
            {
                return this._attributeModelMappingProperties.Required;
            }
            set
            {
                this._attributeModelMappingProperties.Required = value;
            }
        }

        /// <summary>
        /// Property denoting if attribute is read-only
        /// </summary>
        public Boolean? ReadOnly
        {
            get
            {
                return this._attributeModelMappingProperties.ReadOnly;
            }
            set
            {
                this._attributeModelMappingProperties.ReadOnly = value;
            }
        }

        /// <summary>
        /// Property denoting if attribute is to be shown at the time of item creation
        /// </summary>
        public Boolean? ShowAtCreation
        {
            get
            {
                return this._attributeModelMappingProperties.ShowAtCreation;
            }
            set
            {
                this._attributeModelMappingProperties.ShowAtCreation = value;
            }
        }

        /// <summary>
        /// Property denoting  Attribute allowable values for attribute
        /// </summary>
        public String AllowableValues
        {
            get
            {
                return this._attributeModelMappingProperties.AllowableValues;
            }
            set
            {
                this._attributeModelMappingProperties.AllowableValues = value;
            }
        }

        /// <summary>
        /// Property denoting MaxLength of attribute value
        /// </summary>
        public Int32? MaxLength
        {
            get
            {
                return this._attributeModelMappingProperties.MaxLength;
            }
            set
            {
                this._attributeModelMappingProperties.MaxLength = value;
            }
        }

        /// <summary>
        /// Property denoting MinLength of attribute value
        /// </summary>
        public Int32? MinLength
        {
            get
            {
                return this._attributeModelMappingProperties.MinLength;
            }
            set
            {
                this._attributeModelMappingProperties.MinLength = value;
            }
        }

        /// <summary>
        /// Property denoting AllowableUOM for attribute value
        /// </summary>
        public String AllowableUOM
        {
            get
            {
                return this._attributeModelMappingProperties.AllowableUOM;
            }
            set
            {
                this._attributeModelMappingProperties.AllowableUOM = value;
            }
        }

        /// <summary>
        /// Property denoting DefaultUOM of attribute value
        /// </summary>
        public String DefaultUOM
        {
            get
            {
                return this._attributeModelMappingProperties.DefaultUOM;
            }
            set
            {
                this._attributeModelMappingProperties.DefaultUOM = value;
            }
        }

        /// <summary>
        /// Property denoting precision of attribute value
        /// </summary>
        public Int32? Precision
        {
            get
            {
                return this._attributeModelMappingProperties.Precision;
            }
            set
            {
                this._attributeModelMappingProperties.Precision = value;
            }
        }

        /// <summary>
        /// Property denoting if the min value is inclusive
        /// </summary>
        public String MinInclusive
        {
            get
            {
                return this._attributeModelMappingProperties.MinInclusive;
            }
            set
            {
                this._attributeModelMappingProperties.MinInclusive = value;
            }
        }

        /// <summary>
        /// Property denoting if the min value is exclusive
        /// </summary>
        public String MinExclusive
        {
            get
            {
                return this._attributeModelMappingProperties.MinExclusive;
            }
            set
            {
                this._attributeModelMappingProperties.MinExclusive = value;
            }
        }

        /// <summary>
        /// Property denoting if the max value is inclusive
        /// </summary>
        public String MaxInclusive
        {
            get
            {
                return this._attributeModelMappingProperties.MaxInclusive;
            }
            set
            {
                this._attributeModelMappingProperties.MaxInclusive = value;
            }
        }

        /// <summary>
        /// Property denoting if the max value is exclusive
        /// </summary>
        public String MaxExclusive
        {
            get
            {
                return this._attributeModelMappingProperties.MaxExclusive;
            }
            set
            {
                this._attributeModelMappingProperties.MaxExclusive = value;
            }
        }

        /// <summary>
        /// Property denoting RangeTo for attribute value
        /// </summary>
        public String RangeTo
        {
            get
            {
                return Utility.DetermineMaxValues(this._attributeModelMappingProperties.MaxInclusive, this._attributeModelMappingProperties.MaxExclusive, this.AttributeDataTypeName, ValueTypeHelper.GetValue<Int32>(this.Precision));
            }
        }

        /// <summary>
        /// Property denoting RangeFrom for attribute value
        /// </summary>
        public String RangeFrom
        {
            get
            {
                return Utility.DetermineMinValues(this._attributeModelMappingProperties.MinInclusive, this._attributeModelMappingProperties.MinExclusive, this.AttributeDataTypeName, ValueTypeHelper.GetValue<Int32>(this.Precision));
            }
        }

        /// <summary>
        /// Property denoting Definition of attribute
        /// </summary>
        public String Definition
        {
            get
            {
                return this._attributeModelMappingProperties.Definition;
            }
            set
            {
                this._attributeModelMappingProperties.Definition = value;
            }
        }

        /// <summary>
        /// Property denoting Example of attribute
        /// </summary>
        public String AttributeExample
        {
            get
            {
                return this._attributeModelMappingProperties.AttributeExample;
            }
            set
            {
                this._attributeModelMappingProperties.AttributeExample = value;
            }
        }

        /// <summary>
        /// Property denoting BusinessRule for attribute
        /// </summary>
        public String BusinessRule
        {
            get
            {
                return this._attributeModelMappingProperties.BusinessRule;
            }
            set
            {
                this._attributeModelMappingProperties.BusinessRule = value;
            }
        }

        /// <summary>
        /// Property denoting DefaultValue for attribute
        /// </summary>
        public String DefaultValue
        {
            get
            {
                return this._attributeModelMappingProperties.DefaultValue;
            }
            set
            {
                this._attributeModelMappingProperties.DefaultValue = value;
            }
        }

        /// <summary>
        /// Property denoting export mask for attribute value
        /// </summary>
        public String ExportMask
        {
            get
            {
                return this._attributeModelMappingProperties.ExportMask;
            }
            set
            {
                this._attributeModelMappingProperties.ExportMask = value;
            }
        }

        /// <summary>
        /// Property denoting IsSpecialized for attribute
        /// </summary>
        public Boolean? IsSpecialized
        {
            get
            {
                return this._attributeModelMappingProperties.IsSpecialized;
            }
            set
            {
                this._attributeModelMappingProperties.IsSpecialized = value;
            }
        }

        /// <summary>
        /// Gets or sets the dni node characteristic template identifier.
        /// </summary>
        /// <value>
        /// The dni node characteristic template identifier.
        /// </value>
        [DataMember]
        public Int32 DNINodeCharacteristicTemplateId
        {
            get
            {
                return _dniNodeCharacteristicTemplateId;
            }
            set
            {
                _dniNodeCharacteristicTemplateId = value;
            }
        }

        /// <summary>
        /// Property denoting if the attribute is inheritable only and not overridden for the current mapping
        /// </summary>
        public Boolean? InheritableOnly
        {
            get { return _attributeModelMappingProperties.InheritableOnly; }
            set { _attributeModelMappingProperties.InheritableOnly = value; }
        }

        /// <summary>
        /// Property denoting if the attribute is auto promotable for the current mapping
        /// </summary>
        public Boolean? AutoPromotable
        {
            get { return _attributeModelMappingProperties.AutoPromotable ; }
            set { _attributeModelMappingProperties.AutoPromotable = value; }
        }

        /// <summary>
        /// Property denoting the OriginalCategoryAttributeMapping
        /// </summary>
        public CategoryAttributeMapping OriginalCategoryAttributeMapping
        {
            get
            {
                return _originalCategoryAttributeMapping;
            }
            set
            {
                _originalCategoryAttributeMapping = value;
            }
        }

        #region IDataModelObject Properties

        /// <summary>
        /// Property denoting the ExternalId property for this Mapping
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
        /// Property denoting a ObjectType for an dataModelObject object
        /// </summary>
        public ObjectType DataModelObjectType
        {
            get
            {
                return MDM.Core.ObjectType.CategoryAttributeMapping;
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Clone categoryAttributeMapping object
        /// </summary>
        /// <returns>
        /// Cloned copy of categoryAttributeMapping object.
        /// </returns>
        public ICategoryAttributeMapping Clone()
        {
            CategoryAttributeMapping clonedCategoryAttributeMapping = new CategoryAttributeMapping();

            clonedCategoryAttributeMapping.ReferenceId = this.ReferenceId;
            clonedCategoryAttributeMapping.ExternalId = this.ExternalId;

            clonedCategoryAttributeMapping.Id = this.Id;
            clonedCategoryAttributeMapping.Name = this.Name;
            clonedCategoryAttributeMapping.LongName = this.LongName;
            clonedCategoryAttributeMapping.Locale = this.Locale;
            clonedCategoryAttributeMapping.Action = this.Action;
            clonedCategoryAttributeMapping.AuditRefId = this.AuditRefId;
            clonedCategoryAttributeMapping.ExtendedProperties = this.ExtendedProperties;

            clonedCategoryAttributeMapping.AttributeId = this.AttributeId;
            clonedCategoryAttributeMapping.AttributeName = this.AttributeName;
            clonedCategoryAttributeMapping.AttributeLongName = this.AttributeLongName;
            clonedCategoryAttributeMapping.AttributeParentId = this.AttributeParentId;
            clonedCategoryAttributeMapping.AttributeParentName = this.AttributeParentName;
            clonedCategoryAttributeMapping.AttributeParentLongName = this.AttributeParentLongName;
            clonedCategoryAttributeMapping.AttributeDataTypeName = this.AttributeDataTypeName;
            clonedCategoryAttributeMapping.HierarchyId = this.HierarchyId;
            clonedCategoryAttributeMapping.HierarchyName = this.HierarchyName;
            clonedCategoryAttributeMapping.HierarchyLongName = this.HierarchyName;
            clonedCategoryAttributeMapping.CatalogId = this.CatalogId;
            clonedCategoryAttributeMapping.CategoryId = this.CategoryId;
            clonedCategoryAttributeMapping.CategoryName = this.CategoryName;
            clonedCategoryAttributeMapping.CategoryLongName = this.CategoryName;
            clonedCategoryAttributeMapping.ParentEntityId = this.ParentEntityId;
            clonedCategoryAttributeMapping.Path = this.Path;
            clonedCategoryAttributeMapping.IsInheritable = this.IsInheritable;
            clonedCategoryAttributeMapping.MandatoryFlag = this.MandatoryFlag;
            clonedCategoryAttributeMapping.IsDraft = this.IsDraft;
            clonedCategoryAttributeMapping.SourceFlag = this.SourceFlag;
            clonedCategoryAttributeMapping.DNINodeCharacteristicTemplateId = this.DNINodeCharacteristicTemplateId;
            clonedCategoryAttributeMapping.InheritableOnly = this.InheritableOnly;
            clonedCategoryAttributeMapping.AutoPromotable = this.AutoPromotable;

            clonedCategoryAttributeMapping._attributeModelMappingProperties = this._attributeModelMappingProperties.Clone();

            return clonedCategoryAttributeMapping;
        }

        /// <summary>
        /// Marks this instance to be made inherited upon deletion.
        /// </summary>
        public void Inherit()
        {
            Action = ObjectAction.Delete;
            IsInheritable = true;
        }

        /// <summary>
        /// Get Xml representation of categoryAttributeMapping
        /// </summary>
        /// <returns>
        /// Xml representation of categoryAttributeMapping
        /// </returns>
        public override String ToXml()
        {
            String returnXml = String.Empty;
            CultureInfo culture = CultureInfo.InvariantCulture;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    //CategoryAttributeMapping node start
                    xmlWriter.WriteStartElement("CategoryAttributeMapping");

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString(culture));
                    xmlWriter.WriteAttributeString("AttributeId", this.AttributeId.ToString(culture));
                    xmlWriter.WriteAttributeString("AttributeName", this.AttributeName);
                    xmlWriter.WriteAttributeString("AttributeLongName", this.AttributeLongName);
                    xmlWriter.WriteAttributeString("AttributeParentId", this.AttributeParentId.ToString(culture));
                    xmlWriter.WriteAttributeString("AttributeParentName", this.AttributeParentName);
                    xmlWriter.WriteAttributeString("AttributeParentLongName", this.AttributeParentLongName);
                    xmlWriter.WriteAttributeString("AttributeDataTypeName", this.AttributeDataTypeName);
                    xmlWriter.WriteAttributeString("CategoryId", this.CategoryId.ToString(culture));
                    xmlWriter.WriteAttributeString("CategoryName", this.CategoryName);
                    xmlWriter.WriteAttributeString("CatalogId", this.CatalogId.ToString(culture));
                    xmlWriter.WriteAttributeString("HierarchyId", this.HierarchyId.ToString(culture));
                    xmlWriter.WriteAttributeString("ParentEntityId", this.ParentEntityId.ToString(culture));
                    xmlWriter.WriteAttributeString("MandatoryFlag", this.MandatoryFlag);
                    xmlWriter.WriteAttributeString("IsDraft", this.IsDraft.ToString(culture));
                    xmlWriter.WriteAttributeString("SourceFlag", this.SourceFlag);
                    xmlWriter.WriteAttributeString("SortOrder", ValueTypeHelper.GetValue<Int32>(this.SortOrder).ToString());
                    xmlWriter.WriteAttributeString("Required", ValueTypeHelper.GetValue<Boolean>(this.Required).ToString());
                    xmlWriter.WriteAttributeString("ReadOnly", ValueTypeHelper.GetValue<Boolean>(this.ReadOnly).ToString());
                    xmlWriter.WriteAttributeString("ShowAtCreation", ValueTypeHelper.GetValue<Boolean>(this.ShowAtCreation).ToString());
                    xmlWriter.WriteAttributeString("AllowableValues", this.AllowableValues);
                    xmlWriter.WriteAttributeString("MaxLength", ValueTypeHelper.GetValue<Int32>(this.MaxLength).ToString());
                    xmlWriter.WriteAttributeString("MinLength", ValueTypeHelper.GetValue<Int32>(this.MinLength).ToString());
                    xmlWriter.WriteAttributeString("AllowableUOM", this.AllowableUOM);
                    xmlWriter.WriteAttributeString("DefaultUOM", this.DefaultUOM);
                    xmlWriter.WriteAttributeString("Precision", ValueTypeHelper.GetValue<Int32>(this.Precision).ToString());
                    xmlWriter.WriteAttributeString("MinInclusive", this.MinInclusive);
                    xmlWriter.WriteAttributeString("MinExclusive", this.MinExclusive);
                    xmlWriter.WriteAttributeString("MaxInclusive", this.MaxInclusive);
                    xmlWriter.WriteAttributeString("MaxExclusive", this.MaxExclusive);
                    xmlWriter.WriteAttributeString("Definition", this.Definition);
                    xmlWriter.WriteAttributeString("AttributeExample", this.AttributeExample);
                    xmlWriter.WriteAttributeString("BusinessRule", this.BusinessRule);
                    xmlWriter.WriteAttributeString("DefaultValue", this.DefaultValue);
                    xmlWriter.WriteAttributeString("ExportMask", this.ExportMask);
                    xmlWriter.WriteAttributeString("InheritableOnly", this.InheritableOnly.ToString());
                    xmlWriter.WriteAttributeString("AutoPromotable", this.AutoPromotable.ToString());
                    xmlWriter.WriteAttributeString("DNINodeCharacteristicTemplateId", this.DNINodeCharacteristicTemplateId.ToString(culture));

                    xmlWriter.WriteAttributeString("Action", this.Action.ToString());

                    //ExportSubscriber node end
                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();

                    returnXml = sw.ToString();
                }
            }

            return returnXml;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryAttributeMapping"></param>
        /// <param name="compareIds"></param>
        /// <returns></returns>
        public Boolean IsSuperSetOf(CategoryAttributeMapping categoryAttributeMapping, Boolean compareIds = false)
        {
            if (compareIds)
            {
                if (this.Id != categoryAttributeMapping.Id)
                {
                    return false;
                }
                if (this.AttributeId != categoryAttributeMapping.AttributeId)
                {
                    return false;
                }
                if (this.AttributeParentId != categoryAttributeMapping.AttributeParentId)
                {
                    return false;
                }
                if (this.CatalogId != categoryAttributeMapping.CatalogId)
                {
                    return false;
                }
                if (this.HierarchyId != categoryAttributeMapping.HierarchyId)
                {
                    return false;
                }
                if (this.CategoryId != categoryAttributeMapping.CategoryId)
                {
                    return false;
                }
                if (this.ParentEntityId != categoryAttributeMapping.ParentEntityId)
                {
                    return false;
                }
            }
            if (this.AttributeName != categoryAttributeMapping.AttributeName)
            {
                return false;
            }
            if (this.AttributeLongName != categoryAttributeMapping.AttributeLongName)
            {
                return false;
            }
            if (this.AttributeParentName != categoryAttributeMapping.AttributeParentName)
            {
                return false;
            }
            if (this.AttributeParentLongName != categoryAttributeMapping.AttributeParentLongName)
            {
                return false;
            }
            if (this.CategoryName != categoryAttributeMapping.CategoryName)
            {
                return false;
            }

            if (this.InheritableOnly != categoryAttributeMapping.InheritableOnly)
            {
                return false;
            }

            if (this.AutoPromotable != categoryAttributeMapping.AutoPromotable)
            {
                return false;
            }

            if (!this._attributeModelMappingProperties.IsSuperSetOf(categoryAttributeMapping._attributeModelMappingProperties))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Delta Merge of CategoryAttributeMapping
        /// </summary>
        /// <param name="deltaCategoryAttributeMapping">CategoryAttributeMapping that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="returnClonedObject">true means return new merged cloned object;otherwise, same object.</param>
        /// <returns>Merged CategoryAttributeMapping instance</returns>
        public ICategoryAttributeMapping MergeDelta(ICategoryAttributeMapping deltaCategoryAttributeMapping, ICallerContext iCallerContext, bool returnClonedObject = true)
        {
            ICategoryAttributeMapping mergedCategoryAttributeMapping = (returnClonedObject == true) ? deltaCategoryAttributeMapping.Clone() : deltaCategoryAttributeMapping;
            mergedCategoryAttributeMapping.Action = (mergedCategoryAttributeMapping.Equals(this)) ? ObjectAction.Read : ObjectAction.Update;

            return mergedCategoryAttributeMapping;
        }

        /// <summary>
        /// Gets or prepares the attribute model by reading mapping properties.
        /// </summary>
        /// <returns></returns>
        public AttributeModel GetAttributeModel()
        {
            AttributeModelBaseProperties attributeModelBaseProperties = new AttributeModelBaseProperties()
            {
                Id = this.AttributeId,
                Name = this.AttributeName,
                LongName = this.AttributeLongName,
                AttributeParentId = this.AttributeParentId,
                AttributeParentName = this.AttributeParentName,
                AttributeParentLongName = this.AttributeParentLongName,
                AttributeDataTypeName = this.AttributeDataTypeName
            };

            return new AttributeModel(attributeModelBaseProperties, this._attributeModelMappingProperties, null);
        }

        /// <summary>
        /// Sets attribute model object related properties to mapping properties.
        /// </summary>
        /// <param name="attributeModel">Indicates the attribute model object.</param>
        public void SetAttributeModel(AttributeModel attributeModel)
        {
            this._attributeModelMappingProperties = new AttributeModelMappingProperties(this.AttributeId, attributeModel, AttributeModelType.Category);
        }

        #endregion

        #region Overridden Methods

        /// <summary>
        /// Determines whether specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">CategoryAttributeMapping object which needs to be compared.</param>
        /// <returns>Result of the comparison in Boolean.</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj is CategoryAttributeMapping)
            {
                CategoryAttributeMapping objectToBeCompared = obj as CategoryAttributeMapping;

                if (!base.Equals(objectToBeCompared))
                {
                    return false;
                }
                if (this.AttributeId != objectToBeCompared.AttributeId)
                {
                    return false;
                }
                if (this.AttributeName != objectToBeCompared.AttributeName)
                {
                    return false;
                }
                if (this.AttributeLongName != objectToBeCompared.AttributeLongName)
                {
                    return false;
                }
                if (this.AttributeParentId != objectToBeCompared.AttributeParentId)
                {
                    return false;
                }
                if (this.AttributeParentName != objectToBeCompared.AttributeParentName)
                {
                    return false;
                }
                if (this.AttributeParentLongName != objectToBeCompared.AttributeParentLongName)
                {
                    return false;
                }
                if (this.CatalogId != objectToBeCompared.CatalogId)
                {
                    return false;
                }
                if (this.HierarchyId != objectToBeCompared.HierarchyId)
                {
                    return false;
                }
                if (this.HierarchyName != objectToBeCompared.HierarchyName)
                {
                    return false;
                }
                if (this.HierarchyLongName != objectToBeCompared.HierarchyLongName)
                {
                    return false;
                }
                if (this.CategoryId != objectToBeCompared.CategoryId)
                {
                    return false;
                }
                if (this.CategoryName != objectToBeCompared.CategoryName)
                {
                    return false;
                }
                if (this.CategoryLongName != objectToBeCompared.CategoryLongName)
                {
                    return false;
                }
                if (this.ParentEntityId != objectToBeCompared.ParentEntityId)
                {
                    return false;
                }
                if (this.Path != objectToBeCompared.Path)
                {
                    return false;
                }
                if (this.IsDraft != objectToBeCompared.IsDraft)
                {
                    return false;
                }
                if (this.IsInheritable != objectToBeCompared.IsInheritable)
                {
                    return false;
                }
                if (this.MandatoryFlag != objectToBeCompared.MandatoryFlag)
                {
                    return false;
                }
                if (this.SourceFlag != objectToBeCompared.SourceFlag)
                {
                    return false;
                }
                if (this.DNINodeCharacteristicTemplateId != objectToBeCompared.DNINodeCharacteristicTemplateId)
                {
                    return false;
                }

                if (this.InheritableOnly != objectToBeCompared.InheritableOnly)
                {
                    return false;
                }

                if (this.AutoPromotable != objectToBeCompared.AutoPromotable)
                {
                    return false;
                }

                if (this._attributeModelMappingProperties != objectToBeCompared._attributeModelMappingProperties)
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
            return base.GetHashCode() ^ this.AttributeId.GetHashCode() ^ this.AttributeName.GetHashCode() ^ this.AttributeLongName.GetHashCode() ^
                   this.AttributeParentId.GetHashCode() ^ this.AttributeParentName.GetHashCode() ^ this.AttributeParentLongName.GetHashCode() ^
                   this.AttributeDataTypeName.GetHashCode() ^ this.HierarchyId.GetHashCode() ^ this.HierarchyName.GetHashCode() ^ this.HierarchyLongName.GetHashCode() ^
                   this.CatalogId.GetHashCode() ^ this.CategoryId.GetHashCode() ^ this.CategoryName.GetHashCode() ^ this.CategoryLongName.GetHashCode() ^
                   this.ParentEntityId.GetHashCode() ^ this.Path.GetHashCode() ^ this.DNINodeCharacteristicTemplateId.GetHashCode() ^
                   this.MandatoryFlag.GetHashCode() ^ this.IsDraft.GetHashCode() ^ this.IsInheritable.GetHashCode() ^ this.SourceFlag.GetHashCode() ^
                   this.InheritableOnly.GetHashCode() ^ this.AutoPromotable.GetHashCode() ^ this._attributeModelMappingProperties.GetHashCode();
        }

        #endregion

        #region Implementation of IClonable

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion Implementation of IClonable

        #region IDataModelObject

        /// <summary>
        /// Get IDataModelObject for current object.
        /// </summary>
        /// <returns>IDataModelObject</returns>
        public IDataModelObject GetDataModelObject()
        {
            return this;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadHierarchyAttributeMappingFromXml(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "CategoryAttributeMapping")
                        {
                            #region Read Category Attribute Mapping

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.Id);
                                }
                                if (reader.MoveToAttribute("AttributeId"))
                                {
                                    this.AttributeId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.AttributeId);
                                }
                                if (reader.MoveToAttribute("Name") || reader.MoveToAttribute("AttributeName"))
                                {
                                    this.Name = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("LongName") || reader.MoveToAttribute("AttributeLongName"))
                                {
                                    this.LongName = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("AttributeParentName"))
                                {
                                    this.AttributeParentName = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("AttributeParentLongName"))
                                {
                                    this.AttributeParentLongName = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("AttributeDataTypeName"))
                                {
                                    this.AttributeDataTypeName = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("Locale"))
                                {
                                    LocaleEnum locale = LocaleEnum.UnKnown;
                                    Enum.TryParse(reader.ReadContentAsString(), out locale);
                                    this.Locale = locale;
                                }
                                if (reader.MoveToAttribute("CatalogId"))
                                {
                                    this.CatalogId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.CatalogId);
                                }
                                if (reader.MoveToAttribute("HierarchyId"))
                                {
                                    this.HierarchyId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.HierarchyId);
                                }
                                if (reader.MoveToAttribute("HierarchyName"))
                                {
                                    this.HierarchyName = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("CategoryId"))
                                {
                                    this.CategoryId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this.CategoryId);
                                }
                                if (reader.MoveToAttribute("CategoryName"))
                                {
                                    this.CategoryName = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("ParentEntityId"))
                                {
                                    this.ParentEntityId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.ParentEntityId);
                                }
                                if (reader.MoveToAttribute("MandatoryFlag"))
                                {
                                    this.MandatoryFlag = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("IsDraft"))
                                {
                                    this.IsDraft = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this.IsDraft);
                                }
                                if (reader.MoveToAttribute("SourceFlag"))
                                {
                                    this.SourceFlag = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("Path"))
                                {
                                    this.Path = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("SortOrder"))
                                {
                                    this.SortOrder = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }
                                if (reader.MoveToAttribute("Required"))
                                {
                                    this.Required = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }
                                if (reader.MoveToAttribute("ReadOnly"))
                                {
                                    this.ReadOnly = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }
                                if (reader.MoveToAttribute("ShowAtCreation"))
                                {
                                    this.ShowAtCreation = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }
                                if (reader.MoveToAttribute("AllowableValues"))
                                {
                                    this.AllowableValues = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("MaxLength"))
                                {
                                    this.MaxLength = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }
                                if (reader.MoveToAttribute("MinLength"))
                                {
                                    this.MinLength = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }
                                if (reader.MoveToAttribute("AllowableUOM"))
                                {
                                    this.AllowableUOM = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("DefaultUOM"))
                                {
                                    this.DefaultUOM = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("Precision"))
                                {
                                    this.Precision = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }
                                if (reader.MoveToAttribute("MinInclusive"))
                                {
                                    this.MinInclusive = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("MinExclusive"))
                                {
                                    this.MinExclusive = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("MaxInclusive"))
                                {
                                    this.MaxInclusive = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("MaxExclusive"))
                                {
                                    this.MaxExclusive = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("Definition"))
                                {
                                    this.Definition = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("AttributeExample"))
                                {
                                    this.AttributeExample = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("BusinessRule"))
                                {
                                    this.BusinessRule = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("DefaultValue"))
                                {
                                    this.DefaultValue = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("ExportMask"))
                                {
                                    this.ExportMask = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("DNINodeCharacteristicTemplateId"))
                                {
                                    this.DNINodeCharacteristicTemplateId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.DNINodeCharacteristicTemplateId);
                                }
                                if (reader.MoveToAttribute("InheritableOnly"))
                                {
                                    this.InheritableOnly = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("AutoPromotable"))
                                {
                                    this.AutoPromotable = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }
                                if (reader.MoveToAttribute("Action"))
                                {
                                    ObjectAction action = ObjectAction.Unknown;
                                    Enum.TryParse(reader.ReadContentAsString(), out action);
                                    this.Action = action;
                                }
                            }

                            #endregion Read Category Attribute Mapping
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

        #endregion Private Methods

        #endregion Methods
    }
}