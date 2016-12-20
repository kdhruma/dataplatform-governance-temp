using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Core.Exceptions;

    /// <summary>
    /// Exposes methods or properties used for attribute collection object.
    /// </summary>
    public interface IAttributeCollection : IEnumerable<Attribute>
    {
        #region Properties

        /// <summary>
        /// Get the count of no. of attributes in AttributeCollection
        /// </summary>
        Int32 Count { get; }

        #endregion

        #region Methods

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of AttributeCollection object
        /// </summary>
        /// <returns>Xml string representing the AttributeCollection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of AttributeCollection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization serialization);

        #endregion ToXml methods

        #region Attribute Get

        /// <summary>
        /// Gets attribute with specified attribute Id from current entity's attributes
        /// </summary>
        /// <param name="attributeId">Id of an attribute to search in current entity's attributes</param>
        /// <returns>Attribute interface</returns>
        /// <exception cref="ArgumentException">Attribute Id must be greater than 0</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        /// <exception cref="MDMOperationException">Not able to find appropriate attribute as attribute is localized. Please provide locale details.</exception>
        IAttribute GetAttribute(Int32 attributeId);

        /// <summary>
        /// Get attribute based on IAttributeUniqueIdentifier from current entity's attributes
        /// </summary>
        /// <param name="attributeUId">IAttributeUniqueIdentifier which identifies an attribute uniquely</param>
        /// <returns>Attribute Interface</returns>
        /// <exception cref="ArgumentNullException">AttributeUniqueIdentifier cannot be null</exception>
        /// <exception cref="ArgumentException">AttributeUniqueIdentifier.AttributeGroupName and AttributeUniqueIdentifier.AttributeName is either null or empty</exception>
        /// <exception cref="MDMOperationException">Not able to find appropriate attribute as attribute is localized. Please provide locale details.</exception>
        IAttribute GetAttribute(IAttributeUniqueIdentifier attributeUId);

        /// <summary>
        /// Gets attribute with specified attribute Id from current entity's attributes
        /// </summary>
        /// <param name="attributeId">Id of an attribute to search in current entity's attributes</param>
        /// <param name="locale">Locale for which attribute is to be searched</param>
        /// <returns>Attribute interface</returns>
        IAttribute GetAttribute(Int32 attributeId, LocaleEnum locale);

        /// <summary>
        /// Get attribute based on IAttributeUniqueIdentifier from current entity's attributes
        /// </summary>
        /// <param name="attributeUId">IAttributeUniqueIdentifier which identifies an attribute uniquely</param>
        /// <param name="locale">Locale for which attribute is to be searched</param>
        /// <returns>Attribute Interface</returns>
        /// <exception cref="ArgumentNullException">AttributeUniqueIdentifier cannot be null</exception>
        /// <exception cref="ArgumentException">AttributeUniqueIdentifier.AttributeGroupName and AttributeUniqueIdentifier.AttributeName is either null or empty</exception>
        IAttribute GetAttribute(IAttributeUniqueIdentifier attributeUId, LocaleEnum locale);

        /// <summary>
        /// Gets child attributes of given attribute group (parent) id
        /// </summary>
        /// <param name="attributeGroupId">AttributeParentId of which attributes are to be fetched.</param>
        /// <param name="locale">Locale for which attribute is to be searched</param>
        /// <returns>IAttribtueCollection having given parent id </returns>
        /// <exception cref="ArgumentException">AttribtueGroupId must be greater than 0</exception>
        /// <exception cref="NullReferenceException">There are no attributes to search in</exception>
        IAttributeCollection GetAttributes(Int32 attributeGroupId, LocaleEnum locale);

        /// <summary>
        /// Get attributes having specific AttributeModelType from current entity's attributes
        /// </summary>
        /// <param name="attributeModelType">AttributeModelType of which attributes are to be fetched from current entity's attributes</param>
        /// <returns>Attribute collection interface. Attributes in this collection are having specified AttributeModelType</returns>
        IAttributeCollection GetAttributes(AttributeModelType attributeModelType);

        /// <summary>
        /// Get common attributes from current entity's attributes
        /// </summary>
        /// <returns>Attribute collection interface. Attributes in this collection are having AttributeModelType = Common</returns>
        IAttributeCollection GetCommonAttributes();

        /// <summary>
        /// Get Category specific attributes from current entity's attributes
        /// </summary>
        /// <returns>Attribute collection interface. Attributes in this collection are having AttributeModelType = Category</returns>
        IAttributeCollection GetCategorySpecificAttributes();

        /// <summary>
        /// Get System attributes from current entity's attributes
        /// </summary>
        /// <returns>Attribute collection interface. Attributes in this collection are having AttributeModelType = System </returns>
        IAttributeCollection GetSystemAttributes();

        /// <summary>
        /// Gets attribute(s) with specified attribute short Name from current entity's attributes
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute Collection By Attribute Short Name"  source="..\MDM.APISamples\EntityManager\AttributeCollection\AttributeCollectionSamples.cs" region="Get Attribute Collection By Attribute Short Name" />
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeShortName">Name of an attribute to search in current entity's attributes</param>
        /// <returns>Attribute interface</returns>
        /// <exception cref="ArgumentException">Attribute Name cannot be null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        IAttributeCollection GetAttributes(String attributeShortName);

        /// <summary>
        /// Gets attribute with specified attribute short Name from current entity's attributes
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute By Attribute Short Name"  source="..\MDM.APISamples\EntityManager\AttributeCollection\AttributeCollectionSamples.cs" region="Get Attribute By Attribute Short Name" />
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeShortName">Name of an attribute to search in current entity's attributes</param>
        /// <returns>Attribute interface</returns>
        /// <exception cref="ArgumentException">Attribute Name cannot be null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        IAttribute GetAttribute(String attributeShortName);

        /// <summary>
        /// Gets the attribute with the specified attribute short Name and in the specified locale from the current entity's attributes
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute By Attribute Short Name in Locale"  source="..\MDM.APISamples\EntityManager\AttributeCollection\AttributeCollectionSamples.cs" region="Get Attribute By Attribute Short Name in Locale" />
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeShortName">Name of an attribute to search in current entity's attributes</param>
        /// <param name="locale">Specifies Locale in which Attributes should be returned</param>
        /// <returns>Attribute interface</returns>
        /// <exception cref="ArgumentException">Attribute Name cannot be null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        IAttribute GetAttribute(String attributeShortName, LocaleEnum locale);

        /// <summary>
        /// Gets the attribute with specified attribute short Name and attribute parent short name from the current entity's attributes
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute By Attribute Short Name And Attribute Parent Name"  source="..\MDM.APISamples\EntityManager\AttributeCollection\AttributeCollectionSamples.cs" region="Get Attribute By Attribute Short Name And Attribute Parent Name" />
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeShortName">Specifies Name of an attribute to search in current entity's attributes</param>
        /// <param name="attributeParentName">Specifies Parent Name of an attribute to search in current entity's attributes</param>
        /// <returns>Attribute interface</returns>
        /// <exception cref="ArgumentException">Attribute Name cannot be null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        IAttribute GetAttribute(String attributeShortName, String attributeParentName);

        /// <summary>
        /// Gets the attribute with the specified attribute short Name and parent name in the specified locale from the current entity's attributes
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute By Attribute Short Name And Attribute Parent Name in Locale"  source="..\MDM.APISamples\EntityManager\AttributeCollection\AttributeCollectionSamples.cs" region="Get Attribute By Attribute Short Name And Attribute Parent Name in Locale" />
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeShortName">Specifies Name of an attribute to search in current entity's attributes</param>
        /// <param name="attributeParentName">Specifies Parent Name of an attribute to search in current entity's attributes</param>
        /// <param name="locale">Specifies Locale of an attribute to search in current entity's attributes</param>
        /// <returns>Attribute interface</returns>
        /// <exception cref="ArgumentException">Attribute Name cannot be null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        IAttribute GetAttribute(String attributeShortName, String attributeParentName, LocaleEnum locale);

        #endregion Attribute Get

        #region Attribute Value Get

        #region Current value

        /// <summary>
        /// Gets the current attribute value with specified attribute short Name from the current entity's attributes        
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute CurrentValue Instance By Attribute Short Name"  source="..\MDM.APISamples\EntityManager\AttributeCollection\AttributeCollectionSamples.cs" region="Get Attribute CurrentValue Instance By Attribute Short Name" />
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <returns>Returns the current attribute value</returns>
        IValue GetAttributeCurrentValueInstance(String attributeName);

        /// <summary>
        /// Gets the current attribute value with specified attribute short Name from the current entity's attributes in the specified locale        
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute CurrentValue Instance By Attribute Short Name in Locale"  source="..\MDM.APISamples\EntityManager\AttributeCollection\AttributeCollectionSamples.cs" region="Get Attribute CurrentValue Instance By Attribute Short Name in Locale" />
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <param name="locale">Specifies locale in which attribute value should returned </param>
        /// <returns>Returns the current attribute value in the specified locale</returns>
        IValue GetAttributeCurrentValueInstance(String attributeName, LocaleEnum locale);

        /// <summary>
        /// Gets the current invariant attribute value with specified attribute short Name from the current entity's attributes        
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute CurrentValue Instance Invariant By Attribute Short Name"  source="..\MDM.APISamples\EntityManager\AttributeCollection\AttributeCollectionSamples.cs" region="Get Attribute CurrentValue Instance Invariant By Attribute Short Name" />
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <returns>Returns the current invariant attribute value</returns>
        IValue GetAttributeCurrentValueInstanceInvariant(String attributeName);

        /// <summary>
        /// Gets the current invariant attribute value with specified attribute short Name from the current entity's attributes in the specified locale        
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute CurrentValue Instance Invariant By Attribute Short Name in Locale"  source="..\MDM.APISamples\EntityManager\AttributeCollection\AttributeCollectionSamples.cs" region="Get Attribute CurrentValue Instance Invariant By Attribute Short Name in Locale" />
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <param name="locale">Specifies Locale in which value should be returned</param>
        /// <returns>Returns the current invariant attribute value in the specified locale</returns>
        IValue GetAttributeCurrentValueInstanceInvariant(String attributeName, LocaleEnum locale);

        /// <summary>
        /// Gets the current attribute value with specified attribute short Name and specified attribute parent short name from the current entity's attributes        
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute CurrentValue Instance By Attribute Short Name and Attribute Parent Name"  source="..\MDM.APISamples\EntityManager\AttributeCollection\AttributeCollectionSamples.cs" region="Get Attribute CurrentValue Instance By Attribute Short Name and Attribute Parent Name" />
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <param name="attributeParentName">Specifies Parent attribute short name</param>
        /// <returns>Returns the current attribute value</returns>
        IValue GetAttributeCurrentValueInstance(String attributeName, String attributeParentName);

        /// <summary>
        /// Gets the current attribute value with specified attribute short Name and specified attribute parent short name from the current entity's attributes in the spaecified locale       
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute CurrentValue Instance By Attribute Short Name and Attribute Parent Name in Locale"  source="..\MDM.APISamples\EntityManager\AttributeCollection\AttributeCollectionSamples.cs" region="Get Attribute CurrentValue Instance By Attribute Short Name and Attribute Parent Name in Locale" />
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <param name="attributeParentName">Specifies Parent attribute short name</param>
        /// <param name="locale">Specifies Locale in which value should be formatted</param>
        /// <returns>Returns the current attribute value in the specified locale</returns>
        IValue GetAttributeCurrentValueInstance(String attributeName, String attributeParentName, LocaleEnum locale);

        /// <summary>
        /// Gets the current invariant attribute value with specified attribute short Name and specified attribute parent short name from the current entity's attributes        
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute CurrentValue Instance Invariant By Attribute Short Name and Attribute Parent Name"  source="..\MDM.APISamples\EntityManager\AttributeCollection\AttributeCollectionSamples.cs" region="Get Attribute CurrentValue Instance Invariant By Attribute Short Name and Attribute Parent Name" />
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <param name="attributeParentName">Specifies Parent attribute short name</param>
        /// <returns>Returns the current invariant attribute value</returns>
        IValue GetAttributeCurrentValueInstanceInvariant(String attributeName, String attributeParentName);

        /// <summary>
        /// Gets the current attribute value with specified attribute short Name and specified attribute parent short name from the current entity's attributes in the specified locale
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute CurrentValue Instance Invariant By Attribute Short Name and Attribute Parent Name in Locale"  source="..\MDM.APISamples\EntityManager\AttributeCollection\AttributeCollectionSamples.cs" region="Get Attribute CurrentValue Instance Invariant By Attribute Short Name and Attribute Parent Name in Locale" />
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <param name="attributeParentName">Specifies Parent attribute short name</param>
        /// <param name="locale">Specifies Locale in which value should be returned</param>
        /// <returns>Returns the current invariant attribute value in the specified locale</returns>
        IValue GetAttributeCurrentValueInstanceInvariant(String attributeName, String attributeParentName, LocaleEnum locale);

        #endregion

        #region Inherited Value

        /// <summary>
        /// Gets an inherited attribute value with specified attribute short name from the current entity's attributes        
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute Inherited Value Instance By Attribute Short Name"  source="..\MDM.APISamples\EntityManager\AttributeCollection\AttributeCollectionSamples.cs" region="Get Attribute Inherited Value Instance By Attribute Short Name" />
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <returns>Returns an inherited attribute value</returns>
        IValue GetAttributeInheritedValueInstance(String attributeName);

        /// <summary>
        /// Gets an inherited attribute value with specified attribute short name from the current entity's attributes in the specified locale
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute Inherited Value Instance By Attribute Short Name in Locale"  source="..\MDM.APISamples\EntityManager\AttributeCollection\AttributeCollectionSamples.cs" region="Get Attribute Inherited Value Instance By Attribute Short Name in Locale" />
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <param name="locale">Specifies Locale in which value should be formatted</param>
        /// <returns>Returns an inherited attribute value in the specified locale</returns>
        IValue GetAttributeInheritedValueInstance(String attributeName, LocaleEnum locale);

        /// <summary>
        /// Gets an inherited invariant attribute value with specified attribute short name from the current entity's attributes        
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute Inherited Value Instance Invariant By Attribute Short Name"  source="..\MDM.APISamples\EntityManager\AttributeCollection\AttributeCollectionSamples.cs" region="Get Attribute Inherited Value Instance Invariant By Attribute Short Name" />
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <returns>Returns an inherited invariant attribute value</returns>
        IValue GetAttributeInheritedValueInstanceInvariant(String attributeName);

        /// <summary>
        /// Gets an inherited attribute value with specified attribute short name from the current entity's attributes in the specified locale
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute Inherited Value Instance Invariant By Attribute Short Name in Locale"  source="..\MDM.APISamples\EntityManager\AttributeCollection\AttributeCollectionSamples.cs" region="Get Attribute Inherited Value Instance Invariant By Attribute Short Name in Locale" />
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <param name="locale">Specifies Locale in which value should be returned</param>
        /// <returns>Returns an inherited invariant attribute value in the specified locale</returns>
        IValue GetAttributeInheritedValueInstanceInvariant(String attributeName, LocaleEnum locale);

        /// <summary>
        /// Gets an inherited attribute value with specified attribute short name and specified attribute parent short name from the current entity's attributes        
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute Inherited Value Instance By Attribute Short Name and Attribute Parent Name"  source="..\MDM.APISamples\EntityManager\AttributeCollection\AttributeCollectionSamples.cs" region="Get Attribute Inherited Value Instance By Attribute Short Name and Attribute Parent Name" />
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <param name="attributeParentName">Specifies Parent attribute short name</param>
        /// <returns>Returns an inherited attribute value</returns>
        IValue GetAttributeInheritedValueInstance(String attributeName, String attributeParentName);

        /// <summary>
        /// Gets an inherited attribute value with specified attribute short name and specified attribute parent short name from the current entity's attributes in the specified locale        
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute Inherited Value Instance By Attribute Short Name and Attribute Parent Name in Locale"  source="..\MDM.APISamples\EntityManager\AttributeCollection\AttributeCollectionSamples.cs" region="Get Attribute Inherited Value Instance By Attribute Short Name and Attribute Parent Name in Locale" />
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <param name="attributeParentName">Specifies Parent attribute short name</param>
        /// <param name="localeFormate">Specifies Locale in which value should be formatted</param>
        /// <returns>Returns an inherited attribute value in the specified locale</returns>
        IValue GetAttributeInheritedValueInstance(String attributeName, String attributeParentName, LocaleEnum localeFormate);

        /// <summary>
        /// Gets an inherited invariant attribute value with specified attribute short name and specified attribute parent short name from the current entity's attributes        
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute Inherited Value Instance Invariant By Attribute Short Name and Attribute Parent Name"  source="..\MDM.APISamples\EntityManager\AttributeCollection\AttributeCollectionSamples.cs" region="Get Attribute Inherited Value Instance Invariant By Attribute Short Name and Attribute Parent Name" />
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <param name="attributeParentName">Specifies Parent attribute short name</param>
        /// <returns>Returns an inherited invariant attribute value</returns>
        IValue GetAttributeInheritedValueInstanceInvariant(String attributeName, String attributeParentName);

        /// <summary>
        /// Gets an inherited invariant attribute value with specified attribute short name and specified attribute parent short name from the current entity's attributes in the specified locale        
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute Inherited Value Instance Invariant By Attribute Short Name and Attribute Parent Name in Locale"  source="..\MDM.APISamples\EntityManager\AttributeCollection\AttributeCollectionSamples.cs" region="Get Attribute Inherited Value Instance Invariant By Attribute Short Name and Attribute Parent Name in Locale" />
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <param name="attributeParentName">Specifies Parent attribute short name</param>
        /// <param name="locale">Specifies Locale in which value should be returned</param>
        /// <returns>Returns an inherited invariant attribute value in the specified locale</returns>
        IValue GetAttributeInheritedValueInstanceInvariant(String attributeName, String attributeParentName, LocaleEnum locale);

        #endregion

        #endregion

        /// <summary>
        /// Get attribute id array from the current attribute collection
        /// </summary>
        /// <returns>Array of Int32 of attributes ids in current collection</returns>
        /// <exception cref="Exception">Thrown if there are no attributes in current attribute collection</exception>
        Collection<Int32> GetAttributeIdList();

        /// <summary>
        /// Set given locale for all values for all attributes in current collection.
        /// Values of only those attributes which are having overridden values will be set.
        /// </summary>
        /// <param name="localeShortName">Locale name (en_WW) to set for values </param>
        /// <exception cref="ArgumentException">localeShortName cannot be null</exception>
        void SetLocale(String localeShortName);

        /// <summary>
        /// Set given action for all attributes in current collection.
        /// </summary>
        /// <param name="action">Locale name (en_WW) to set for values </param>
        /// <exception cref="ArgumentException">localeShortName cannot be null</exception>
        void SetAction(ObjectAction action);

        /// <summary>
        /// Add attributes object in collection
        /// </summary>
        /// <param name="items">attributes to add in collection</param>
        void AddRange(IAttributeCollection items);

        /// <summary>
        /// Add attribute Instance in collection
        /// </summary>
        /// <param name="item">Attribute to add in collection</param>
        void Add(IAttribute item);

        /// <summary>
        /// Removes the first occurrence of a specific object from the Attribute collection
        /// </summary>
        /// <param name="item">The object to remove from the Attribute collection</param>
        /// <returns>true if item is successfully removed; otherwise, false</returns>
        Boolean Remove(IAttribute item);

        #endregion Methods
    }
}
