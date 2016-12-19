using System;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get category attribute mapping object.
    /// </summary>
    public interface ICategoryAttributeMapping : IMDMObject, ICloneable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the id of the attribute.
        /// </summary>
        /// <value>
        /// The id of the attribute.
        /// </value>
        Int32 AttributeId { get; set; }

        /// <summary>
        /// Gets or sets the short name of the attribute.
        /// </summary>
        /// <value>
        /// The name of the attribute.
        /// </value>
        String AttributeName { get; set; }

        /// <summary>
        /// Gets or sets the long name of the attribute.
        /// </summary>
        /// <value>
        /// The name of the attribute.
        /// </value>
        String AttributeLongName { get; set; }

        /// <summary>
        /// Gets or sets the parent id of the attribute.
        /// </summary>
        /// <value>
        /// The parent id of the attribute.
        /// </value>
        Int32 AttributeParentId { get; set; }

        /// <summary>
        /// Gets or sets the parent name of the attribute.
        /// </summary>
        /// <value>
        /// The parent name of the attribute.
        /// </value>
        String AttributeParentName { get; set; }

        /// <summary>
        /// Gets or sets the parent long name of the attribute.
        /// </summary>
        /// <value>
        /// The parent long name of the attribute.
        /// </value>
        String AttributeParentLongName { get; set; }

        /// <summary>
        /// Gets or sets the hierarchy identifier.
        /// </summary>
        /// <value>
        /// The hierarchy identifier.
        /// </value>
        Int32 HierarchyId { get; set; }

        /// <summary>
        /// Gets or sets the short name of the hierarchy.
        /// </summary>
        /// <value>
        /// The name of the hierarchy.
        /// </value>
        String HierarchyName { get; set; }

        /// <summary>
        /// Gets or sets the long name of the hierarchy.
        /// </summary>
        /// <value>
        /// The name of the hierarchy.
        /// </value>
        String HierarchyLongName { get; set; }

        /// <summary>
        /// Gets or sets the catalog identifier.
        /// </summary>
        /// <value>
        /// The catalog identifier.
        /// </value>
        Int32 CatalogId { get; set; }

        /// <summary>
        /// Gets or sets the category id.
        /// </summary>
        /// <value>
        /// The category id.
        /// </value>
        Int64 CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the short name of the category.
        /// </summary>
        /// <value>
        /// The name of the category.
        /// </value>
        String CategoryName { get; set; }

        /// <summary>
        /// Gets or sets the long name of the category.
        /// </summary>
        /// <value>
        /// The name of the category.
        /// </value>
        String CategoryLongName { get; set; }

        /// <summary>
        /// Gets or sets the c node parent identifier.
        /// </summary>
        /// <value>
        /// The c node parent identifier.
        /// </value>
        Int32 ParentEntityId { get; set; }

        /// <summary>
        /// Field specifying the Category Path
        /// </summary>
        String Path { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [is inheritable].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is inheritable]; otherwise, <c>false</c>.
        /// </value>
        Boolean IsInheritable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [is mandatory].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is mandatory]; otherwise, <c>false</c>.
        /// </value>
        String MandatoryFlag { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [is draft].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is draft]; otherwise, <c>false</c>.
        /// </value>
        Boolean IsDraft { get; set; }

        /// <summary>
        /// Gets or sets the source flag.
        /// </summary>
        /// <value>
        /// The source flag.
        /// </value>
        String SourceFlag { get; set; }

        /// <summary>
        /// Property denoting sort order of attribute
        /// </summary>
        Int32? SortOrder { get; set; }

        /// <summary>
        /// Property denoting if attribute value is Required
        /// </summary>
        Boolean? Required { get; set; }

        /// <summary>
        /// Property denoting if attribute is read-only
        /// </summary>
        Boolean? ReadOnly { get; set; }

        /// <summary>
        /// Property denoting if attribute is to be shown at the time of item creation
        /// </summary>
        Boolean? ShowAtCreation { get; set; }

        /// <summary>
        /// Property denoting  Attribute allowable values for attribute
        /// </summary>
        String AllowableValues { get; set; }

        /// <summary>
        /// Property denoting MaxLength of attribute value
        /// </summary>
        Int32? MaxLength { get; set; }

        /// <summary>
        /// Property denoting MinLength of attribute value
        /// </summary>
        Int32? MinLength { get; set; }

        /// <summary>
        /// Property denoting AllowableUOM for attribute value
        /// </summary>
        String AllowableUOM { get; set; }

        /// <summary>
        /// Property denoting DefaultUOM of attribute value
        /// </summary>
        String DefaultUOM { get; set; }

        /// <summary>
        /// Property denoting precision of attribute value
        /// </summary>
        Int32? Precision { get; set; }

        /// <summary>
        /// Property denoting if the min value is inclusive
        /// </summary>
        String MinInclusive { get; set; }

        /// <summary>
        /// Property denoting if the min value is exclusive
        /// </summary>
        String MinExclusive { get; set; }

        /// <summary>
        /// Property denoting if the max value is inclusive
        /// </summary>
        String MaxInclusive { get; set; }

        /// <summary>
        /// Property denoting if the max value is exclusive
        /// </summary>
        String MaxExclusive { get; set; }

        /// <summary>
        /// Property denoting RangeTo for attribute value
        /// </summary>
        String RangeTo { get; }

        /// <summary>
        /// Property denoting RangeFrom for attribute value
        /// </summary>
        String RangeFrom { get; }

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
        /// Property denoting DefaultValue for attribute
        /// </summary>
        String DefaultValue { get; set; }

        /// <summary>
        /// Property denoting export mask for attribute value
        /// </summary>
        String ExportMask { get; set; }

        /// <summary>
        /// Property denoting IsSpecialized for attribute
        /// </summary>
        Boolean? IsSpecialized { get; set; }

        /// <summary>
        /// Gets or sets the dni node characteristic template identifier.
        /// </summary>
        /// <value>
        /// The dni node characteristic template identifier.
        /// </value>
        Int32 DNINodeCharacteristicTemplateId { get; set; }

        /// <summary>
        /// Property denoting Inheritable Only.
        /// </summary>
        Boolean? InheritableOnly { get; set; }

        /// <summary>
        /// Property denoting Auto Promotable.
        /// </summary>
        Boolean? AutoPromotable { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Get Xml representation of categoryAttributeMapping
        /// </summary>
        /// <returns>Xml representation of categoryAttributeMapping</returns>
        String ToXml();

        /// <summary>
        /// Clone categoryAttributeMapping object
        /// </summary>
        /// <returns>Cloned copy of categoryAttributeMapping object.</returns>
        new ICategoryAttributeMapping Clone();

        /// <summary>
        /// Delta Merge of CategoryAttributeMapping
        /// </summary>
        /// <param name="deltaCategoryAttributeMapping">CategoryAttributeMapping that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="returnClonedObject">true means return new merged cloned object;otherwise, same object.</param>
        /// <returns>Merged ICategoryAttributeMapping instance</returns>
        ICategoryAttributeMapping MergeDelta(ICategoryAttributeMapping deltaCategoryAttributeMapping, ICallerContext iCallerContext, Boolean returnClonedObject = true);

        /// <summary>
        /// Marks this instance to be made inherited upon deletion.
        /// </summary>
        void Inherit();

        #endregion
    }
}