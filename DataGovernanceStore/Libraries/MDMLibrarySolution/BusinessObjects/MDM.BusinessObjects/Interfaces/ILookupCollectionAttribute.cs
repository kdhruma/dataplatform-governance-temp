using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties used for providing lookup attribute collection related information.
    /// </summary>
    public interface ILookupCollectionAttribute
    {
        #region Fields
        #endregion

        #region Methods

        #region GetValues Methods

        /// <summary>
        /// Gets the current attribute values
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Current Value Instances"  source="..\..\Documentation\MDM.APISamples\EntityManager\LookupCollectionAttribute\LookupCollectionAttributeSamples.cs" region="Get Current Value Instances"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <returns>Returns the current attribute values</returns>
        IValueCollection GetCurrentValueInstances();

        /// <summary>
        /// Gets an inherited attribute values
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Inherited Value Instances"  source="..\..\Documentation\MDM.APISamples\EntityManager\LookupCollectionAttribute\LookupCollectionAttributeSamples.cs" region="Get Inherited Value Instances"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <returns>Returns an inherited attribute values</returns>
        IValueCollection GetInheritedValueInstances();

        /// <summary>
        /// Gets the current invariant attribute values
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Current Value Instances Invariant"  source="..\..\Documentation\MDM.APISamples\EntityManager\LookupCollectionAttribute\LookupCollectionAttributeSamples.cs" region="Get Current Value Instances Invariant"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <returns>Returns the current invariant attribute values</returns>
        IValueCollection GetCurrentValueInstancesInvariant();

        /// <summary>
        /// Gets an inherited invariant attribute values
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Inherited Value Instances Invariant"  source="..\..\Documentation\MDM.APISamples\EntityManager\LookupCollectionAttribute\LookupCollectionAttributeSamples.cs" region="Get Inherited Value Instances Invariant"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <returns>Returns an inherited invariant attribute values</returns>
        IValueCollection GetInheritedValueInstancesInvariant();

        /// <summary>
        /// Gets the current lookup row as a dictionary based on the specified value reference id
        /// </summary>
        /// <param name="valueRefId">Specifies the value reference id of the lookup row</param>
        /// <example>
        /// <code language="c#" title="Get Current Lookup Row As Dictionary"  source="..\..\Documentation\MDM.APISamples\EntityManager\LookupCollectionAttribute\LookupCollectionAttributeSamples.cs" region="Get Current Lookup Row As Dictionary"/>
        /// <code language="c#" title="Get Entity"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity"/>
        /// </example>
        /// <returns>Returns the dictionary representing the lookup row</returns>
        Dictionary<String, String> GetCurrentLookupRowAsDictionary(Int32 valueRefId);

        /// <summary>
        /// Gets an inherited lookup row as a dictionary based on the specified value reference id
        /// </summary>
        /// <param name="valueRefId">Specifies the value reference id of the lookup row</param>
        /// <example>
        /// <code language="c#" title="Get Inherited Lookup Row As Dictionary"  source="..\..\Documentation\MDM.APISamples\EntityManager\LookupCollectionAttribute\LookupCollectionAttributeSamples.cs" region="Get Inherited Lookup Row As Dictionary"/>
        /// <code language="c#" title="Get Entity"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity"/>
        /// </example>
        /// <returns>Returns the dictionary representing the lookup row</returns>
        Dictionary<String, String> GetInheritedLookupRowAsDictionary(Int32 valueRefId);

        /// <summary>
        /// Gets the current lookup cell values across all rows based on the column name
        /// </summary>
        /// <param name="columnName">Specifies the lookup column name</param>
        /// <example>
        /// <code language="c#" title="Get Current Lookup Cell Value Collection"  source="..\..\Documentation\MDM.APISamples\EntityManager\LookupCollectionAttribute\LookupCollectionAttributeSamples.cs" region="Get Current Lookup Cell Value Collection"/>
        /// <code language="c#" title="Get Entity"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity"/>
        /// </example>
        /// <returns>Returns the collection of lookup cell values</returns>
        Collection<String> GetCurrentLookupCellValueCollection(String columnName);

        /// <summary>
        /// Gets an inherited lookup cell values across all rows based on the column name
        /// </summary>
        /// <param name="columnName">Specifies the lookup column name</param>
        /// <example>
        /// <code language="c#" title="Get Inherited Lookup Cell Value Collection"  source="..\..\Documentation\MDM.APISamples\EntityManager\LookupCollectionAttribute\LookupCollectionAttributeSamples.cs" region="Get Inherited Lookup Cell Value Collection"/>
        /// <code language="c#" title="Get Entity"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity"/>
        /// </example>
        /// <returns>Returns the collection of lookup cell values</returns>
        Collection<String> GetInheritedLookupCellValueCollection(String columnName);

        /// <summary>
        /// Gets the current lookup cell value based on the specified value reference id and column name
        /// </summary>
        /// <param name="valueRefId">Specifies the value reference id of the lookup row</param>
        /// <param name="columnName">Specifies the lookup column name</param>
        /// <example>
        /// <code language="c#" title="Get Current Lookup Cell Value"  source="..\..\Documentation\MDM.APISamples\EntityManager\LookupCollectionAttribute\LookupCollectionAttributeSamples.cs" region="Get Current Lookup Cell Value"/>
        /// <code language="c#" title="Get Entity"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity"/>
        /// </example>
        /// <returns>Returns the lookup cell value</returns>
        String GetCurrentLookupCellValue(Int32 valueRefId, String columnName);

        /// <summary>
        /// Gets an inherited lookup cell value based on the specified value reference id and column name
        /// </summary>
        /// <param name="valueRefId">Specifies the value reference id of the lookup row</param>
        /// <param name="columnName">Specifies the lookup column name</param>
        /// <example>
        /// <code language="c#" title="Get Inherited Lookup Cell Value"  source="..\..\Documentation\MDM.APISamples\EntityManager\LookupCollectionAttribute\LookupCollectionAttributeSamples.cs" region="Get Inherited Lookup Cell Value"/>
        /// <code language="c#" title="Get Entity"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity"/>
        /// </example>
        /// <returns>Returns the lookup cell value</returns>
        String GetInheritedLookupCellValue(Int32 valueRefId, String columnName);

        #endregion

        #region Set/Append Attribute Overridden Values

        /// <summary>
        /// Sets specified attribute values overrides the existing attribute values
        /// </summary>
        /// <param name="values">Specifies new values to set in overridden attribute collection</param>
        /// <example>
        /// <code language="c#" title="Set Lookup Values"  source="..\..\Documentation\MDM.APISamples\EntityManager\LookupCollectionAttribute\LookupCollectionAttributeSamples.cs" region="Set Lookup Values"/>
        /// <code language="c#" title="Get Entity"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity"/>
        /// </example>
        void SetValues(IValueCollection values);

        /// <summary>
        /// Sets specified attribute values overrides the existing attribute values in the specified locale
        /// </summary>
        /// <param name="valueCollection">Specifies new values to set in overridden attribute collection</param>
        /// <param name="formatLocale">Specifies Locale in which values will be set</param>
        /// <example>
        /// <code language="c#" title="Set Lookup Values For Locale"  source="..\..\Documentation\MDM.APISamples\EntityManager\LookupCollectionAttribute\LookupCollectionAttributeSamples.cs" region="Set Lookup Values For Locale"/>
        /// <code language="c#" title="Get Entity"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity"/>
        /// </example>
        void SetValues(IValueCollection valueCollection, LocaleEnum formatLocale);

        /// <summary>
        /// Sets specified invariant attribute values overrides the existing attribute values
        /// </summary>
        /// <param name="values">Specifies new values to set in overridden attribute collection</param>
        /// <example>
        /// <code language="c#" title="Set Lookup Values Invariant"  source="..\..\Documentation\MDM.APISamples\EntityManager\LookupCollectionAttribute\LookupCollectionAttributeSamples.cs" region="Set Lookup Values Invariant"/>
        /// <code language="c#" title="Get Entity"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity"/>
        /// </example>
        void SetValuesInvariant(IValueCollection values);

        /// <summary>
        /// Sets specified attribute values overrides the existing attribute values
        /// </summary>
        /// <param name="values">Specifies new values to set in overridden attribute collection</param>
        /// <example>
        /// <code language="c#" title="Set Lookup Values From Array Of Objects"  source="..\..\Documentation\MDM.APISamples\EntityManager\LookupCollectionAttribute\LookupCollectionAttributeSamples.cs" region="Set Lookup Values From Array Of Objects"/>
        /// <code language="c#" title="Get Entity"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity"/>
        /// </example>
        void SetValues(Object[] values);

        /// <summary>
        /// Sets specified attribute values overrides the existing attribute values in the specified locale
        /// </summary>
        /// <param name="values">Specifies new values to set in overridden attribute collection</param>
        /// <param name="locale">Specifies Locale in which values will be set</param>
        /// <example>
        /// <code language="c#" title="Set Lookup Values from Array of Objects for Specific Locale"  source="..\..\Documentation\MDM.APISamples\EntityManager\LookupCollectionAttribute\LookupCollectionAttributeSamples.cs" region="Set Lookup Values From Array Of Objects For Specific Locale"/>
        /// <code language="c#" title="Get Entity"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity"/>
        /// </example>
        void SetValues(Object[] values, LocaleEnum locale);

        /// <summary>
        /// Sets specified invariant attribute values overrides the existing attribute values
        /// </summary>
        /// <param name="values">Specifies new values to set in overridden attribute collection</param>
        /// <example>
        /// <code language="c#" title="Set Lookup Values Invariant From Array Of Objects"  source="..\..\Documentation\MDM.APISamples\EntityManager\LookupCollectionAttribute\LookupCollectionAttributeSamples.cs" region="Set Lookup Values Invariant From Array Of Objects"/>
        /// <code language="c#" title="Get Entity"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity"/>
        /// </example>
        void SetValuesInvariant(Object[] values);

        /// <summary>
        /// Appends specified attribute values in existing attribute values
        /// </summary>
        /// <param name="valueCollection">Specifies new values to add in overridden attribute collection</param>
        /// <exception cref="ArgumentNullException">ValueCollection cannot be null</exception>
        /// <example>
        /// <code language="c#" title="Append Values to Lookup"  source="..\..\Documentation\MDM.APISamples\EntityManager\LookupCollectionAttribute\LookupCollectionAttributeSamples.cs" region="Append Values To Lookup"/>
        /// <code language="c#" title="Get Entity"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity"/>
        /// </example>
        void AppendValues(IValueCollection valueCollection);

        /// <summary>
        /// Appends specified attribute values for the specified locale in existing attribute values
        /// </summary>
        /// <param name="valueCollection">Specifies new values to add in overridden attribute collection</param>
        /// <param name="formatLocale">Specifies Locale in which values will be set</param>
        /// <example>
        /// <code language="c#" title="Append Values to Lookup for Specific Locale"  source="..\..\Documentation\MDM.APISamples\EntityManager\LookupCollectionAttribute\LookupCollectionAttributeSamples.cs" region="Append Values To Lookup For Specific Locale"/>
        /// <code language="c#" title="Get Entity"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity"/>
        /// </example>
        void AppendValues(IValueCollection valueCollection, LocaleEnum formatLocale);

        /// <summary>
        /// Appends specified invariant attribute values in existing attribute values
        /// </summary>
        /// <param name="valueCollection">Specifies new values to add in overridden attribute collection</param>
        /// <example>
        /// <code language="c#" title="Append Values Invariant to Lookup"  source="..\..\Documentation\MDM.APISamples\EntityManager\LookupCollectionAttribute\LookupCollectionAttributeSamples.cs" region="Append Values Invariant To Lookup"/>
        /// <code language="c#" title="Get Entity"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity"/>
        /// </example>
        void AppendValuesInvariant(IValueCollection valueCollection);

        /// <summary>
        /// Appends specified invariant attribute values in existing attribute values
        /// </summary>
        /// <param name="values">Specifies new values to add in overridden attribute collection</param>
        /// <example>
        /// <code language="c#" title="Append Lookup Values Invariant from Array of Objects"  source="..\..\Documentation\MDM.APISamples\EntityManager\LookupCollectionAttribute\LookupCollectionAttributeSamples.cs" region="Append Lookup Values Invariant From Array Of Objects"/>
        /// <code language="c#" title="Get Entity"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity"/>
        /// </example>
        void AppendValuesInvariant(Object[] values);

        /// <summary>
        /// Appends specified attribute values in existing attribute values
        /// </summary>
        /// <param name="values">Specifies new values to add in overridden attribute collection</param>
        /// <example>
        /// <code language="c#" title="Append Lookup Values from Array of Objects"  source="..\..\Documentation\MDM.APISamples\EntityManager\LookupCollectionAttribute\LookupCollectionAttributeSamples.cs" region="Append Lookup Values From Array Of Objects"/>
        /// <code language="c#" title="Get Entity"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity"/>
        /// </example>
        void AppendValues(Object[] values);

        /// <summary>
        /// Appends specified attribute values for the specified locale in existing attribute values
        /// </summary>
        /// <param name="values">Specifies new values to add in overridden attribute collection</param>
        /// <param name="locale">Specifies Locale in which values will be set</param>
        /// <example>
        /// <code language="c#" title="Append Lookup Values for Specific Locale from Array of Objects"  source="..\..\Documentation\MDM.APISamples\EntityManager\LookupCollectionAttribute\LookupCollectionAttributeSamples.cs" region="Append Lookup Values For Specific Locale From Array Of Objects"/>
        /// <code language="c#" title="Get Entity"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity"/>
        /// </example>
        void AppendValues(Object[] values, LocaleEnum locale);

        /// <summary>
        /// Appends specified invariant attribute values in existing attribute values
        /// </summary>
        /// <param name="value">Specifies new value to add in overridden attribute collection</param>
        /// <example>
        /// <code language="c#" title="Append Invariant Value from Object"  source="..\..\Documentation\MDM.APISamples\EntityManager\LookupCollectionAttribute\LookupCollectionAttributeSamples.cs" region="Append Lookup Values Invariant From Objects"/>
        /// <code language="c#" title="Get Entity"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity"/>
        /// </example>
        void AppendValuesInvariant(Object value);

        /// <summary>
        /// Appends specified attribute value in existing attribute values
        /// </summary>
        /// <param name="value">Specifies new value to append in overridden attribute collection</param>
        /// <example>
        /// <code language="c#" title="Append Lookup Value from Object"  source="..\..\Documentation\MDM.APISamples\EntityManager\LookupCollectionAttribute\LookupCollectionAttributeSamples.cs" region="Append Lookup Value From Object"/>
        /// <code language="c#" title="Get Entity"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity"/>
        /// </example>
        void AppendValue(Object value);

        /// <summary>
        /// Appends specified attribute value in existing attribute values in the specified locale
        /// </summary>
        /// <param name="value">Specifies new value to add in overridden attribute collection</param>
        /// <param name="locale">Specifies Locale in which values will be set</param>
        /// <example>
        /// <code language="c#" title="Append Lookup Value From Object For Specific Locale"  source="..\..\Documentation\MDM.APISamples\EntityManager\LookupCollectionAttribute\LookupCollectionAttributeSamples.cs" region="Append Lookup Value From Object For Specific Locale"/>
        /// <code language="c#" title="Get Entity"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity"/>
        /// </example>
        void AppendValue(Object value, LocaleEnum locale);

        #endregion

        #endregion
    }
}
