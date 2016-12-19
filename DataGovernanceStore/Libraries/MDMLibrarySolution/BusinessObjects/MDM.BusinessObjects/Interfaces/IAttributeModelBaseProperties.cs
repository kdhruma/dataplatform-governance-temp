using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get attribute model base properties.
    /// </summary>
    public interface IAttributeModelBaseProperties : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property denoting AttributeParent Id
        /// </summary>
        Int32 AttributeParentId { get; set; }

        /// <summary>
        /// Property denoting AttributeParent Name
        /// </summary>
        String AttributeParentName { get; set; }

        /// <summary>
        /// Property denoting AttributeParent LongName
        /// </summary>
        String AttributeParentLongName { get; set; }

        /// <summary>
        /// Property denoting AttributeType Id
        /// </summary>
        Int32 AttributeTypeId { get; set; }

        /// <summary>
        /// Property denoting AttributeType name.
        /// </summary>
        String AttributeTypeName { get; set; }

        /// <summary>
        /// Property denoting the AttributeType. This property represents de-normalized version of attribute type information.
        /// For possible values see <see cref="MDM.Core.AttributeTypeEnum"/>
        /// </summary>
        MDM.Core.AttributeTypeEnum AttributeType { get; set; }

        /// <summary>
        /// Property denoting Attribute DataType Id
        /// </summary>
        Int32 AttributeDataTypeId { get; set; }

        /// <summary>
        /// Property denoting Attribute DataType name
        /// </summary>
        String AttributeDataTypeName { get; set; }

        /// <summary>
        /// Field denoting Attribute display type Id
        /// </summary>
        Int32 AttributeDisplayTypeId { get; set; }

        /// <summary>
        /// Property denoting  Attribute display type name
        /// </summary>
        String AttributeDisplayTypeName { get; set; }

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
        String UomType { get; set; }

        /// <summary>
        /// Property denoting precision of attribute value
        /// </summary>
        Int32 Precision { get; set; }

        /// <summary>
        /// Property denoting if attribute value  is collection
        /// </summary>
        Boolean IsCollection { get; set; }

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
        String Definition { get; set; }

        /// <summary>
        /// Property denoting Example of attribute
        /// </summary>
        String AttributeExample { get; set; }

        /// <summary>
        /// Property denoting BusinessRule for attribute
        /// </summary>
        String BusinessRule { get; set; }

        /// <summary>
        /// Property denoting if attribute is read-only
        /// </summary>
        Boolean ReadOnly { get; set; }

        /// <summary>
        /// Property denoting Extension of attribute
        /// </summary>
        String Extension { get; set; }

        /// <summary>
        /// Property denoting AttributeRegExp for attribute value
        /// </summary>
        String AttributeRegEx { get; set; }

        /// <summary>
        /// Property denoting the attribute regular expression error message for the attribute value
        /// </summary>
        String RegExErrorMessage { get; set; }

        /// <summary>
        /// Property denoting LookUpTableName for attribute
        /// </summary>
        String LookUpTableName { get; set; }

        /// <summary>
        /// Property denoting DefaultValue for attribute
        /// </summary>
        String DefaultValue { get; set; }

        /// <summary>
        /// Property denoting complex table name for attribute
        /// </summary>
        String ComplexTableName { get; set; }

        /// <summary>
        /// Property denoting Path of attribute
        /// </summary>
        String Path { get; set; }

        /// <summary>
        /// Property denoting if attribute id searchable
        /// </summary>
        Boolean Searchable { get; set; }
        
        /// <summary>
        /// Property denoting is history is to be kept for attribute value
        /// </summary>
        Boolean EnableHistory { get; set; }

        /// <summary>
        /// Property denoting WebUri for attribute
        /// </summary>
        String WebUri { get; set; }

        /// <summary>
        /// Property denoting list of columns for showing for lookup attribute
        /// </summary>
        String LkDisplayColumns { get; set; }

        /// <summary>
        /// Property denoting sort order for lookup values in lookup attribute
        /// </summary>
        String LkSortOrder { get; set; }

        /// <summary>
        /// Property denoting list of columns to search in for lookup attribute
        /// </summary>
        String LkSearchColumns { get; set; }

        /// <summary>
        /// Property denoting display format of attribute value for lookup attribute
        /// </summary>
        String LkDisplayFormat { get; set; }

        /// <summary>
        /// Property denoting sort order of attribute
        /// </summary>
        Int32 SortOrder { get; set; }

        /// <summary>
        /// Property denoting export mask for attribute value
        /// </summary>
        String ExportMask { get; set; }

        /// <summary>
        /// Property denoting if attribute is inheritable
        /// </summary>
        Boolean Inheritable { get; set; }

        /// <summary>
        /// Property denoting if attribute is hidden
        /// </summary>
        Boolean IsHidden { get; set; }

        /// <summary>
        /// Property denoting if attribute is a complex attribute
        /// </summary>
        Boolean IsComplex { get; set; }

        /// <summary>
        /// Property denoting if attribute is a lookup attribute
        /// </summary>
        Boolean IsLookup { get; set; }

        /// <summary>
        /// Property denoting if attribute is localizable 
        /// </summary>
        Boolean IsLocalizable { get; set; }

        /// <summary>
        /// Property denoting is locale specific format is to be applied to attribute value
        /// </summary>
        Boolean ApplyLocaleFormat { get; set; }

        /// <summary>
        /// Property denoting is time zone conversion is to be applied to attribute value
        /// </summary>
        Boolean ApplyTimeZoneConversion { get; set; }

        /// <summary>
        /// Property denoting if null values are searchable for attribute value
        /// </summary>
        Boolean AllowNullSearch { get; set; }

        /// <summary>
        /// Property denoting if attribute is to be shown at the time of item creation
        /// </summary>
        Boolean ShowAtCreation { get; set; }

        /// <summary>
        /// Property denoting the attribute model type such as common,technical
        /// </summary>
        AttributeModelType AttributeModelType { get; set; }

        /// <summary>
        /// Property denoting is the current attribute model depends on any other attribute model.
        /// if 'true' then current attribute model is dependent on other attribute model.
        /// </summary>
        Boolean IsDependentAttribute { get; set; }

        /// <summary>
        /// Property denoting has any Dependent attributes for the current attribute model.
        /// </summary>
        Boolean HasDependentAttribute { get; set; }

        /// <summary>
        /// Property denoting if arbitrary precision is used for Decimal attributes.
        /// If IsPrecisionArbitrary is  set to true, then decimal value 12.2 with precision 3 will be stored as "12.2"
        /// If IsPrecisionArbitrary is  set to false, then decimal value 12.2 with precision 3 will be stored as "12.200"
        /// </summary>
        Boolean IsPrecisionArbitrary { get; set; }

        #endregion
    }
}