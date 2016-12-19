using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get attribute model instance.
    /// </summary>
    public interface IAttributeModel : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property denoting AttributeParent Id
        /// </summary>
        Int32 AttributeParentId { get; }

        /// <summary>
        /// Property denoting AttributeParent Name
        /// </summary>
        String AttributeParentName { get; }

        /// <summary>
        /// Property denoting AttributeParent LongName
        /// </summary>
        String AttributeParentLongName { get; }

        /// <summary>
        /// Property denoting AttributeType Id
        /// </summary>
        Int32 AttributeTypeId { get; }

        /// <summary>
        /// Property denoting the AttributeType. This property represents de-normalized version of attribute type information.
        /// For possible values see <see cref="MDM.Core.AttributeTypeEnum"/>
        /// </summary>
        MDM.Core.AttributeTypeEnum AttributeType { get; }

        /// <summary>
        /// Property denoting Attribute DataType Id
        /// </summary>
        Int32 AttributeDataTypeId { get; }

        /// <summary>
        /// Property denoting Attribute DataType name
        /// </summary>
        String AttributeDataTypeName { get; }

        /// <summary>
        /// Field denoting Attribute display type Id
        /// </summary>
        Int32 AttributeDisplayTypeId { get; }

        /// <summary>
        /// Property denoting  Attribute display type name
        /// </summary>
        String AttributeDisplayTypeName { get; }

        /// <summary>
        /// Property denoting  Attribute allowed values for attribute
        /// </summary>
        String AllowableValues { get; set; }

        /// <summary>
        /// Property denoting MaxLength of attribute value
        /// </summary>
        Int32 MaxLength { get; set; }

        /// <summary>
        /// Property denoting MinLength of attribute value
        /// </summary>
        Int32 MinLength { get; set; }

        /// <summary>
        /// Property denoting if attribute value is Required
        /// </summary>
        Boolean Required { get; set; }

        /// <summary>
        /// Property denoting AllowableUOM for attribute value
        /// </summary>
        String AllowableUOM { get; set; }

        /// <summary>
        /// Property denoting DefaultUOM of attribute value
        /// </summary>
        String DefaultUOM { get; set; }

        /// <summary>
        /// Property denoting UOMType of attribute value
        /// </summary>
        String UomType { get; }

        /// <summary>
        /// Property denoting precision of attribute value
        /// </summary>
        Int32 Precision { get; set; }

        /// <summary>
        /// Property denoting if attribute value  is collection
        /// </summary>
        Boolean IsCollection { get; }

        /// <summary>
        /// Property denoting RangeTo for attribute value
        /// </summary>
        String RangeTo { get; set; }

        /// <summary>
        /// Property denoting RangeFrom for attribute value
        /// </summary>
        String RangeFrom { get; set; }

        /// <summary>
        /// Property denoting Label of attribute
        /// </summary>
        String Label { get; set; }

        /// <summary>
        /// Property denoting Definition of attribute
        /// </summary>
        String Definition { get; }

        /// <summary>
        /// Property denoting Example of attribute
        /// </summary>
        String AttributeExample { get; }

        /// <summary>
        /// Property denoting BusinessRule for attribute
        /// </summary>
        String BusinessRule { get; }

        /// <summary>
        /// Property denoting if attribute is read-only
        /// </summary>
        Boolean ReadOnly { get; set; }

        /// <summary>
        /// Property denoting Extension of attribute
        /// </summary>
        String Extension { get; }

        /// <summary>
        /// Property denoting AssemblyName for attribute business rule
        /// </summary>
        [Obsolete("This property has been depreciated. This would be removed from object in next version")]
        String AttributeAssemblyName { get; }

        /// <summary>
        /// Property denoting Method for attribute business rule
        /// </summary>
        [Obsolete("This property has been depreciated. This would be removed from object in next version")]
        String AssemblyMethod { get; }

        /// <summary>
        /// Property denoting AttributeRegExp for attribute value
        /// </summary>
        String AttributeRegEx { get; }

        /// <summary>
        /// Property denoting the attribute regular expression error message for the attribute value
        /// </summary>
        String RegExErrorMessage { get; }

        /// <summary>
        /// Property denoting AssemblyClass for attribute business rule
        /// </summary>
        [Obsolete("This property has been depreciated. This would be removed from object in next version")]
        String AssemblyClass { get; }

        /// <summary>
        /// Property denoting LookUpTableName for attribute
        /// </summary>
        String LookUpTableName { get; }

        /// <summary>
        /// Property denoting DefaultValue for attribute
        /// </summary>
        String DefaultValue { get; }

        /// <summary>
        /// Property denoting isClassification
        /// </summary>
        [Obsolete("This property has been depreciated. This would be removed from object in next version")]
        Boolean Classification { get; }

        /// <summary>
        /// Property denoting complex table name for attribute
        /// </summary>
        String ComplexTableName { get; }

        /// <summary>
        /// Property denoting rule lookup table for attribute
        /// </summary>
        [Obsolete("This property has been depreciated. This would be removed from object in next version")]
        String RuleLookupTable { get; }

        /// <summary>
        /// Property denoting RuleSP for attribute
        /// </summary>
        [Obsolete("This property has been depreciated. This would be removed from object in next version")]
        String RuleSP { get; }

        /// <summary>
        /// Property denoting Path of attribute
        /// </summary>
        String Path { get; }

        /// <summary>
        /// Property denoting if attribute id searchable
        /// </summary>
        Boolean Searchable { get; }

        /// <summary>
        /// Property denoting is history is to be kept for attribute value
        /// </summary>
        Boolean EnableHistory { get; }

        /// <summary>
        /// Property denoting if attribute is to be persisted
        /// </summary>
        [Obsolete("This property has been depreciated. This would be removed from object in next version")]
        Boolean Persists { get; }

        /// <summary>
        /// Property denoting WebUri for attribute
        /// </summary>
        String WebUri { get; }

        /// <summary>
        /// Property denoting custom action for attribute
        /// </summary>
        [Obsolete("This property has been depreciated. This would be removed from object in next version")]
        String CustomAction { get; }

        /// <summary>
        /// Property denoting initial population method for attribute value
        /// </summary>
        [Obsolete("This property has been depreciated. This would be removed from object in next version")]
        String InitialPopulationMethod { get; }

        /// <summary>
        /// Property denoting JavaScript to be executed on click for an attribute
        /// </summary>
        [Obsolete("This property has been depreciated. This would be removed from object in next version")]
        String OnClickJavaScript { get; }

        /// <summary>
        /// Property denoting JavaScript to be executed on load for an attribute
        /// </summary>
        [Obsolete("This property has been depreciated. This would be removed from object in next version")]
        String OnLoadJavaScript { get; }

        /// <summary>
        /// Property denoting storage format of attribute value for lookup attribute
        /// </summary>
        [Obsolete("This property has been depreciated. This would be removed from object in next version")]
        String LkStorageFormat { get; }

        /// <summary>
        /// Property denoting list of columns for showing for lookup attribute
        /// </summary>
        String LkDisplayColumns { get; }

        /// <summary>
        /// Property denoting sort order for lookup values in lookup attribute
        /// </summary>
        String LkSortOrder { get; }

        /// <summary>
        /// Property denoting list of columns to search in for lookup attribute
        /// </summary>
        String LkSearchColumns { get; }

        /// <summary>
        /// Property denoting display format of attribute value for lookup attribute
        /// </summary>
        String LkDisplayFormat { get; }

        /// <summary>
        /// Property denoting list of columns for showing for lookup attribute
        /// </summary>
        [Obsolete("This property has been depreciated. This would be removed from object in next version")]
        String LookUpDisplayColumns { get; }

        /// <summary>
        /// Property denoting if duplicates are allowed for lookup attribute
        /// </summary>
        [Obsolete("This property has been depreciated. This would be removed from object in next version")]
        Boolean LkupDuplicateAllowed { get; }

        /// <summary>
        /// Property denoting whether to store lookup reference
        /// </summary>
        [Obsolete("This property has been depreciated. This would be removed from object in next version")]
        Boolean StoreLookupReference { get; }

        /// <summary>
        /// Property denoting sort order of attribute
        /// </summary>
        Int32 SortOrder { get; set; }

        /// <summary>
        /// Property denoting export mask for attribute value
        /// </summary>
        String ExportMask { get; }

        /// <summary>
        /// Property denoting if attribute is inheritable
        /// </summary>
        Boolean Inheritable { get; }

        /// <summary>
        /// Property denoting if attribute is hidden
        /// </summary>
        Boolean IsHidden { get; set; }

        /// <summary>
        /// Property denoting if attribute is a complex attribute
        /// </summary>
        Boolean IsComplex { get; }

        /// <summary>
        /// Property denoting if attribute is a lookup attribute
        /// </summary>
        Boolean IsLookup { get; }

        /// <summary>
        /// Property denoting if attribute is localizable 
        /// </summary>
        Boolean IsLocalizable { get; }

        /// <summary>
        /// Property denoting is locale specific format is to be applied to attribute value
        /// </summary>
        Boolean ApplyLocaleFormat { get; }

        /// <summary>
        /// Property denoting is time zone conversion is to be applied to attribute value
        /// </summary>
        Boolean ApplyTimeZoneConversion { get; }

        /// <summary>
        /// Property denoting if null values are searchable for attribute value
        /// </summary>
        Boolean AllowNullSearch { get; }

        /// <summary>
        /// Property denoting if attribute is to be shown at the time of item creation
        /// </summary>
        Boolean ShowAtCreation { get; }

        /// <summary>
        /// Property denoting the attribute model type such as common,technical
        /// </summary>
        AttributeModelType AttributeModelType { get; }

        /// <summary>
        /// Property denoting is the current attribute model depends on any other attribute model.
        /// if 'true' then current attribute model is dependent on other attribute model.
        /// </summary>
        Boolean IsDependentAttribute { get; }

        /// <summary>
        /// Property denoting has any Dependent attributes for the current attribute model.
        /// </summary>
        Boolean HasDependentAttribute { get; }

        /// <summary>
        /// Property denoting Dependent attributes for this current attribute model.
        /// Collection of dependent attributes which contains the details of children dependent link mapping details for this attribute model.
        /// </summary>
        DependentAttributeCollection DependentChildAttributes { get; set; }

        /// <summary>
        /// Property denoting Dependent on attributes for this current attribute model.
        /// Collection of dependent attributes which contains the details of Parent dependent link mapping details for this attribute model.
        /// </summary>
        DependentAttributeCollection DependentParentAttributes { get; set; }

        /// <summary>
        /// Property denoting if arbitrary precision is used for Decimal attributes.
        /// If value of IsPrecisionArbitrary is true, then decimal value 12.2 with precision 3 will be stored as "12.2"
        /// If value of IsPrecisionArbitrary is false, then decimal value 12.2 with precision 3 will be stored as "12.200"
        /// </summary>
        Boolean IsPrecisionArbitrary { get; }

        /// <summary>
        /// Property denoting if the attribute value is inheritable only and not overridden
        /// </summary>
        Boolean InheritableOnly { get; }

        #endregion

        #region Methods

        #region Xml Methods

        /// <summary>
        /// Get Xml representation of Attribute Model
        /// </summary>
        /// <returns>Xml representation of Attribute Model</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of Attribute Model based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Attribute Model</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion Xml Methods

        #region Get Models

        /// <summary>
        /// Get child attribute models for current attribute model
        /// </summary>
        /// <returns>AttributeModelCollection interface</returns>
        /// <exception cref="NullReferenceException">Raised when child attribute models are null</exception>
        IAttributeModelCollection GetChildAttributeModels();

        /// <summary>
        /// Get context for current attribute model
        /// </summary>
        /// <returns>AttributeModelContext interface</returns>
        /// <exception cref="NullReferenceException">Raised when context for current model is null</exception>
        IAttributeModelContext GetContext();

        #endregion Get Models

        /// <summary>
        /// Set the locale of an attributeModel
        /// </summary>
        /// <param name="locale">Locale</param>
        void SetLocale(LocaleEnum locale);

        /// <summary>
        /// Clone attribute model object
        /// </summary>
        /// <returns>cloned copy of attribute model object.</returns>
        IAttributeModel Clone();

        /// <summary>
        /// Delta Merge of attribute model
        /// </summary>
        /// <param name="deltaAttributeModel">AttributeModel that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="returnClonedObject">true means return new merged cloned object;otherwise, same object.</param>
        /// <returns>Merged attribute model instance</returns>
        IAttributeModel MergeDelta(IAttributeModel deltaAttributeModel, ICallerContext iCallerContext, Boolean returnClonedObject = true);

        #endregion Methods
    }
}