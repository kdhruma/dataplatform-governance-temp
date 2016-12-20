using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties used for providing collection attribute related information.
    /// </summary>
    public interface ICollectionAttribute
    {
        #region Get Methods

        /// <summary>
        /// Gets the current attribute values from the current entity
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Current Value Instances Of Collection Attribute"  source="..\..\Documentation\MDM.APISamples\EntityManager\CollectionAttribute\CollectionAttributeSamples.cs" region="Get Current Value Instances Of Collection Attribute"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <returns>Returns the current attribute values</returns>
        IValueCollection GetCurrentValueInstances();

        /// <summary>
        /// Gets the current attribute values from the current entity in the specified locale
        /// </summary>
        /// <param name="locale">Specifies Locale in which value should be formatted</param>
        /// <example>
        /// <code language="c#" title="Get Current Value Instances of Collection Attribute for Locale"  source="..\..\Documentation\MDM.APISamples\EntityManager\CollectionAttribute\CollectionAttributeSamples.cs" region="Get Current Value Instances Of Collection Attribute For Locale"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <returns>Returns the current attribute values in the specified locale</returns>
        IValueCollection GetCurrentValueInstances(LocaleEnum locale);

        /// <summary>
        /// Gets the current invariant attribute values from the current entity
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Current Value Instances Invariant of Collection Attribute"  source="..\..\Documentation\MDM.APISamples\EntityManager\CollectionAttribute\CollectionAttributeSamples.cs" region="Get Current Value Instances Invariant Of Collection Attribute"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <returns>Returns the current invariant attribute values</returns>
        IValueCollection GetCurrentValueInstancesInvariant();

        /// <summary>
        /// Gets an inherited attribute values from the current entity
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Inherited Value Instances from Collection Attribute"  source="..\..\Documentation\MDM.APISamples\EntityManager\CollectionAttribute\CollectionAttributeSamples.cs" region="Get Inherited Value Instances From Collection Attribute"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <returns>Returns an inherited attribute values</returns>
        IValueCollection GetInheritedValueInstances();

        /// <summary>
        /// Gets an inherited attribute values from current entity in the specified locale
        /// </summary>
        /// <param name="locale">Specifies Locale in which value should be formatted</param>
        /// <example>
        /// <code language="c#" title="Get Inherited Value Instances for Locale from Collection Attribute"  source="..\..\Documentation\MDM.APISamples\EntityManager\CollectionAttribute\CollectionAttributeSamples.cs" region="Get Inherited Value Instances For Locale From Collection Attribute"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <returns>Returns an inherited attribute values</returns>
        IValueCollection GetInheritedValueInstances(LocaleEnum locale);

        /// <summary>
        /// Gets an inherited invariant attribute values from the current entity
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Inherited Value Instances Invariant from Collection Attribute"  source="..\..\Documentation\MDM.APISamples\EntityManager\CollectionAttribute\CollectionAttributeSamples.cs" region="Get Inherited Value Instances Invariant From Collection Attribute"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <returns>Returns an inherited invariant attribute values</returns>
        IValueCollection GetInheritedValueInstancesInvariant();

        /// <summary>
        /// Gets the current attribute values from the current entity.
        /// </summary>
        /// <typeparam name="T">Indicates the data type of the result</typeparam>
        /// <example>
        /// <code language="c#" title="Get Current Values in Specified Type from Collection Attribute"  source="..\..\Documentation\MDM.APISamples\EntityManager\CollectionAttribute\CollectionAttributeSamples.cs" region="Get Current Values In Specified Type From Collection Attribute"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <returns>Returns the current attribute values based on the requested data type if the data type is valid</returns>
        Collection<T> GetCurrentValues<T>();

        /// <summary>
        /// Gets the current invariant attribute values from the current entity
        /// </summary>
        /// <typeparam name="T">Indicates the data type of the result</typeparam>
        /// <example>
        /// <code language="c#" title="Get Current Values Invariant in Specified Type from Collection Attribute"  source="..\..\Documentation\MDM.APISamples\EntityManager\CollectionAttribute\CollectionAttributeSamples.cs" region="Get Current Values Invariant In Specified Type From Collection Attribute"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <returns>Returns the current invariant attribute values based on the requested data type if the data type is valid</returns>
        Collection<T> GetCurrentValuesInvariant<T>();

        /// <summary>
        /// Gets the current attribute values from the current entity in the specified locale
        /// </summary>
        /// <typeparam name="T">Indicates the data type of the result</typeparam>
        /// <param name="locale">Specifies Locale in which value should be formatted</param>
        /// <example>
        /// <code language="c#" title="Get Current Values in Specified Type from Collection Attribute for Locale"  source="..\..\Documentation\MDM.APISamples\EntityManager\CollectionAttribute\CollectionAttributeSamples.cs" region="Get Current Values In Specified Type From Collection Attribute For Locale"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <returns>Returns the current attribute values in the specified locale based on the requested data type if the data type is valid</returns>
        Collection<T> GetCurrentValues<T>(LocaleEnum locale);

        #endregion

        #region Set/Append Attribute Values

        /// <summary>
        /// Sets specified attribute values overrides the existing attribute values
        /// </summary>
        /// <param name="values">Specifies the new values to set in overridden attribute collection</param>
        /// <example>
        /// <code language="c#" title="Set Values as Value Collection to Collection Attribute"  source="..\..\Documentation\MDM.APISamples\EntityManager\CollectionAttribute\CollectionAttributeSamples.cs" region="Set Values as Value Collection to Collection Attribute"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        void SetValues(IValueCollection values);

        /// <summary>
        /// Sets specified attribute values overrides the existing attribute values in the specified locale
        /// </summary>
        /// <param name="valueCollection">Specifies new values to set in overridden attribute collection</param>
        /// <param name="formatLocale">Specifies Locale in which values will be set</param>
        /// <example>
        /// <code language="c#" title="Set Values as Value Collection for Specific Locale to Collection Attribute"  source="..\..\Documentation\MDM.APISamples\EntityManager\CollectionAttribute\CollectionAttributeSamples.cs" region="Set Values As Value Collection For Specific Locale To Collection Attribute"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        void SetValues(IValueCollection valueCollection, LocaleEnum formatLocale);

        /// <summary>
        /// Sets specified invariant attribute values overrides the existing attribute values
        /// </summary>
        /// <param name="values">Specifies new value to set in overridden attribute collection</param>
        /// <example>
        /// <code language="c#" title="Set Values Invariant as Value Collection to Collection Attribute"  source="..\..\Documentation\MDM.APISamples\EntityManager\CollectionAttribute\CollectionAttributeSamples.cs" region="Set Values Invariant As Value Collection To Collection Attribute"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        void SetValuesInvariant(IValueCollection values);

        /// <summary>
        /// Sets specified attribute values overrides the existing attribute values
        /// </summary>
        /// <param name="values">Specifies new value to set in overridden attribute collection</param>
        /// <example>
        /// <code language="c#" title="Set Values as Array Of Objects to Collection Attribute"  source="..\..\Documentation\MDM.APISamples\EntityManager\CollectionAttribute\CollectionAttributeSamples.cs" region="Set Values As Array Of Objects To Collection Attribute"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        void SetValues(Object[] values);

        /// <summary>
        /// Sets specified attribute values overrides the existing attribute values in the specified locale
        /// </summary>
        /// <param name="values">Specifies new value to set in overridden attribute collection</param>
        /// <param name="locale">Specifies Locale in which values will be set</param>
        /// <example>
        /// <code language="c#" title="Set Values as Array Of Objects for Specific Locale to Collection Attribute"  source="..\..\Documentation\MDM.APISamples\EntityManager\CollectionAttribute\CollectionAttributeSamples.cs" region="Set Values As Array Of Objects For Specific Locale To Collection Attribute"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        void SetValues(Object[] values, LocaleEnum locale);

        /// <summary>
        /// Sets specified invariant attribute values overrides the existing attribute values
        /// </summary>
        /// <param name="values">Specifies new value to set in overridden attribute collection</param>
        /// <example>
        /// <code language="c#" title="Set Values Invariant As Array Of Objects To Collection Attribute"  source="..\..\Documentation\MDM.APISamples\EntityManager\CollectionAttribute\CollectionAttributeSamples.cs" region="Set Values Invariant As Array Of Objects To Collection Attribute"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        void SetValuesInvariant(Object[] values);

        /// <summary>
        /// Appends specified attribute values in existing attribute values
        /// </summary>
        /// <param name="valueCollection">Specifies new value to add in overridden attribute collection</param>
        /// <exception cref="ArgumentNullException">ValueCollection cannot be null</exception>
        /// <example>
        /// <code language="c#" title="Append Value Collection to Collection Attribute"  source="..\..\Documentation\MDM.APISamples\EntityManager\CollectionAttribute\CollectionAttributeSamples.cs" region="Append Value Collection To Collection Attribute"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        void AppendValues(IValueCollection valueCollection);

        /// <summary>
        /// Appends specified attribute values for the specified locale in existing attribute values
        /// </summary>
        /// <param name="valueCollection">Specifies new value to add in overridden attribute collection</param>
        /// <param name="formatLocale">Specifies Locale in which values will be set</param>
        /// <example>
        /// <code language="c#" title="Append Value Collection for Locale to Collection Attribute"  source="..\..\Documentation\MDM.APISamples\EntityManager\CollectionAttribute\CollectionAttributeSamples.cs" region="Append Value Collection For Locale To Collection Attribute"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        void AppendValues(IValueCollection valueCollection, LocaleEnum formatLocale);

        /// <summary>
        /// Appends specified invariant attribute values in existing attribute values
        /// </summary>
        /// <param name="valueCollection">Specifies new value to add in overridden attribute collection</param>
        /// <example>
        /// <code language="c#" title="Append Value Collection Invariant to Collection Attribute"  source="..\..\Documentation\MDM.APISamples\EntityManager\CollectionAttribute\CollectionAttributeSamples.cs" region="Append Value Collection Invariant To Collection Attribute"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        void AppendValuesInvariant(IValueCollection valueCollection);

        /// <summary>
        /// Appends specified invariant attribute values in existing attribute values
        /// </summary>
        /// <param name="values">Specifies new value to add in overridden attribute collection</param>
        /// <example>
        /// <code language="c#" title="Append Value Collection for Locale to Collection Attribute"  source="..\..\Documentation\MDM.APISamples\EntityManager\CollectionAttribute\CollectionAttributeSamples.cs" region="Append Value Collection For Locale To Collection Attribute"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        void AppendValuesInvariant(Object[] values);

        /// <summary>
        /// Appends specified attribute values in existing attribute values
        /// </summary>
        /// <param name="values">Specifies new value to add in overridden attribute collection</param>
        /// <example>
        /// <code language="c#" title="Append Values as Collection of Objects"  source="..\..\Documentation\MDM.APISamples\EntityManager\CollectionAttribute\CollectionAttributeSamples.cs" region="Append Values As Collection Of Objects"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        void AppendValues(Object[] values);

        /// <summary>
        /// Appends specified attribute values for the specified locale in existing attribute values
        /// </summary>
        /// <param name="values">Specifies new value to add in overridden attribute collection</param>
        /// <param name="locale">Specifies Locale in which values will be set</param>
        /// <example>
        /// <code language="c#" title="Append Values as Collection of Objects for Locale"  source="..\..\Documentation\MDM.APISamples\EntityManager\CollectionAttribute\CollectionAttributeSamples.cs" region="Append Values As Collection Of Objects For Locale"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        void AppendValues(Object[] values, LocaleEnum locale);

        /// <summary>
        /// Appends specified invariant attribute values in existing attribute values
        /// </summary>
        /// <param name="value">Specifies new value to add in overridden attribute collection</param>
        /// <example>
        /// <code language="c#" title="Append Value Invariant to Collection Attribute"  source="..\..\Documentation\MDM.APISamples\EntityManager\CollectionAttribute\CollectionAttributeSamples.cs" region="Append Value Invariant To Collection Attribute"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        void AppendValueInvariant(Object value);

        /// <summary>
        /// Appends specified attribute value in existing attribute values
        /// </summary>
        /// <param name="value">Values to set</param>
        /// <example>
        /// <code language="c#" title="Append Value as Object to Collection Attribute"  source="..\..\Documentation\MDM.APISamples\EntityManager\CollectionAttribute\CollectionAttributeSamples.cs" region="Append Value As Object To Collection Attribute"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        void AppendValue(Object value);

        /// <summary>
        /// Appends specified attribute value in existing attribute values in the specified locale
        /// </summary>
        /// <param name="value">Specifies new value to add in overridden attribute collection</param>
        /// <param name="locale">Specifies Locale in which values will be set</param>
        /// <example>
        /// <code language="c#" title="Append Value for Locale to Collection Attribute"  source="..\..\Documentation\MDM.APISamples\EntityManager\CollectionAttribute\CollectionAttributeSamples.cs" region="Append Value For Locale To Collection Attribute"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        void AppendValue(Object value, LocaleEnum locale);

        #endregion
    }
}
