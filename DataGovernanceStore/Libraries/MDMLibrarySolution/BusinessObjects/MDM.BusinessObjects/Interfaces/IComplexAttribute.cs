using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties used for providing complex attribute related information.
    /// </summary>
    public interface IComplexAttribute
    {
        #region Properties

        /// <summary>
        /// Get and set value for the child attribute
        /// </summary>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <returns>Returns the attribute current value</returns>
        Object this[String attributeName] { get; set; }

        #endregion

        #region Methods

        #region Child Attribute Utilities

        /// <summary>
        /// Get all the child attributes
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get All Complex Child Attributes"  source="..\..\Documentation\MDM.APISamples\EntityManager\ComplexAttribute\ComplexAttributeSamples.cs" region="Get All Complex Child Attributes"/>
        /// <code language="c#" title="Get Entity with Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <returns>Returns the child attributes</returns>
        IAttributeCollection GetChildAttributes();

        /// <summary>
        /// Gets the child attribute based on the attribute short name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Child Complex Attribute"  source="..\..\Documentation\MDM.APISamples\EntityManager\ComplexAttribute\ComplexAttributeSamples.cs" region="Get a Complex Child Attribute"/>
        /// <code language="c#" title="Get Entity with Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Indicates the attribute short Name</param>
        /// <returns>Returns the attribute object</returns>
        IAttribute GetChildAttribute(String attributeName);

        /// <summary>
        /// Gets the child attribute's current value as Dictionary format based on the attribute short name.
        /// Dictionary key is attribute name and the value is attribute current value.
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Child Complex Attribute As Dictionary"  source="..\..\Documentation\MDM.APISamples\EntityManager\ComplexAttribute\ComplexAttributeSamples.cs" region="Get Complex ChildAttribute Value As Dictionary"/>
        /// <code language="c#" title="Get Entity with Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <returns>Returns the Dictionary object.Dictionary key is attribute name and the value is attribute current value.</returns>
        Dictionary<String, Object> GetChildAttributesAsDictionary();

        /// <summary>
        /// Gets the child lookup attribute based on the attribute short name
        /// </summary>
        /// <example>        
        /// <code language="c#" title="Get Child Attribute As Lookup"  source="..\MDM.APISamples\EntityManager\Entity\GetLookupAttributeSamples.cs" region="Lookup Attribute From Complex Attribute"/>
        /// <code language="c#" title="Get Entity with Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Indicates the attribute Name</param>
        /// <returns>Returns the ILookupAttribute object</returns>
        ILookupAttribute GetChildAttributeAsLookup(String attributeName);

        #endregion Child Attribute Utilities

        #region Utilities for getting child attribute values

        /// <summary>
        /// Gets the current value object of the requested child attribute
        /// </summary>
        /// <param name="attributeName">Indicates the complex child attribute short name</param>
        /// <example>
        /// <code language="c#" title="Get Complex Child Attribute Value Instance"  source="..\..\Documentation\MDM.APISamples\EntityManager\ComplexAttribute\ComplexAttributeSamples.cs" region="Get Complex Child Attribute Value Instance"/>
        /// <code language="c#" title="Get Entity with Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <returns>Current value object (considering source flag)</returns>
        IValue GetChildAttributeCurrentValueInstance(String attributeName);

        /// <summary>
        /// Gets the child attribute's current invariant value based on child attribute name
        /// </summary>
        /// <param name="attributeName">Indicates the complex child attribute short name</param>
        /// <example> 
        /// <code language="c#" title="Get Complex Child Attribute Value Instance Invariant"  source="..\..\Documentation\MDM.APISamples\EntityManager\ComplexAttribute\ComplexAttributeSamples.cs" region="Get Complex Child Attribute Value Instance Invariant"/>
        /// <code language="c#" title="Get Entity with Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <returns>Returns the current invariant value for the complex child attribute</returns>
        IValue GetChildAttributeCurrentValueInstanceInvariant(String attributeName);

        /// <summary>
        /// Gets the child attribute's Inherited value based on child attribute name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Child Attribute Inherited Value"  source="..\..\Documentation\MDM.APISamples\EntityManager\ComplexAttribute\ComplexAttributeSamples.cs" region="Get Complex ChildAttribute InheritedValue"/>
        /// <code language="c#" title="Get Entity with Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="attributeName">Indicates the complex child attribute short name</param>
        /// <returns>Returns the Inherited value for the complex child attribute</returns>
        IValue GetChildAttributeInheritedValueInstance(String attributeName);

        /// <summary>
        /// Gets the child attribute's current value based on requested data type
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Child Attribute Current Value"  source="..\..\Documentation\MDM.APISamples\EntityManager\ComplexAttribute\ComplexAttributeSamples.cs" region="Get Complex ChildAttribute Value In RequestedType"/>
        /// <code language="c#" title="Get Entity with Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="shortName">Indicates the child attribute short name</param>
        /// <typeparam name="T">Indicates the data type of the result</typeparam>
        /// <returns>Returns the child attribute's current value as requested data type</returns>
        T GetChildAttributeCurrentValue<T>(String shortName);

        /// <summary>
        /// Gets the child attribute's current invariant value based on requested data type
        /// </summary>
        /// <param name="shortName">Indicates the child attribute short name</param>
        /// <typeparam name="T">Indicates the data type of the result</typeparam>
        /// <exception cref="InvalidCastException">Not able to format the attribute value to the requested data type</exception>
        /// <example>
        /// <code language="c#" title="Get Complex Child Attribute Value Invariant in Requested Type"  source="..\..\Documentation\MDM.APISamples\EntityManager\ComplexAttribute\ComplexAttributeSamples.cs" region="Get Complex ChildAttribute Value Invariant In RequestedType"/>
        /// <code language="c#" title="Get Entity with Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <returns>Returns the child attribute's current invariant value as requested data type</returns>
        T GetChildAttributeCurrentValueInvariant<T>(String shortName);

        #endregion Utilities for getting child attribute Values

        #region Utilities for updating child attribute values

        /// <summary>
        /// Sets the complex child attribute value based on the attribute short name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Update Child Attribute Value"  source="..\..\Documentation\MDM.APISamples\EntityManager\ComplexAttribute\ComplexAttributeSamples.cs" region="Update Complex Child AttributeValue By IValue As Argument"/>
        /// <code language="c#" title="Add Child Attribute Value"  source="..\..\Documentation\MDM.APISamples\EntityManager\ComplexAttribute\ComplexAttributeSamples.cs" region="Add Complex Child AttributeValue By IValue As Argument"/>
        /// </example>
        /// <param name="attributeName">Indicates the child attribute short name</param>
        /// <param name="value">Indicates the value of the child attribute</param>
        void SetValue(String attributeName, IValue value);

        /// <summary>
        /// Sets the complex attribute based on the child attributes
        /// </summary>
        /// <example>
        /// <code language="c#" title="Update Child Attribute Value"  source="..\..\Documentation\MDM.APISamples\EntityManager\ComplexAttribute\ComplexAttributeSamples.cs" region="Update Complex Child AttributeValue By IAttributeCollection As Argument"/>
        /// <code language="c#" title="Add Child Attribute Value"  source="..\..\Documentation\MDM.APISamples\EntityManager\ComplexAttribute\ComplexAttributeSamples.cs" region="Add Complex Child AttributeValue By IAttributeCollection As Argument"/>
        /// </example>
        /// <param name="childAttributes">Indicates the child attribute collections</param>
        void SetValues(IAttributeCollection childAttributes);

        /// <summary>
        /// Sets the complex attribute based on the child attributes.
        /// </summary>
        /// <example>
        /// <code language="c#" title="Update Child Attribute Value"  source="..\..\Documentation\MDM.APISamples\EntityManager\ComplexAttribute\ComplexAttributeSamples.cs" region="Update Complex Child AttributeValue By Dictionary As Argument"/>
        /// <code language="c#" title="Add Child Attribute Value"  source="..\..\Documentation\MDM.APISamples\EntityManager\ComplexAttribute\ComplexAttributeSamples.cs" region="Add Complex Child AttributeValue By Dictionary As Argument"/>
        /// </example>
        /// <param name="values">Indicates the child attribute values. Dictionary key is attribute short name and the value is 
        /// corresponding values for the child.</param>
        void SetValues(Dictionary<String, Object> values);

        /// <summary>
        /// Sets the complex child attribute value based on the attribute short name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Update Child Attribute Value"  source="..\..\Documentation\MDM.APISamples\EntityManager\ComplexAttribute\ComplexAttributeSamples.cs" region="Update Complex Child AttributeValue By Object As Argument"/>
        /// <code language="c#" title="Add Child Attribute Value"  source="..\..\Documentation\MDM.APISamples\EntityManager\ComplexAttribute\ComplexAttributeSamples.cs" region="Add Complex Child AttributeValue By Object As Argument"/>
        /// </example>
        /// <param name="attributeName">Indicates the child attribute short name</param>
        /// <param name="value">Indicates the value of the child attribute</param>
        void SetValue(String attributeName, Object value);

        #endregion Utilities for updating child attribute values

        #region Utilities for clearing child attribute values

        /// <summary>
        /// Removes the complex child record value with the specified attribute short name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Remove Child Attribute Value"  source="..\..\Documentation\MDM.APISamples\EntityManager\ComplexAttribute\ComplexAttributeSamples.cs" region="Remove Child Attribute Value"/>
        /// </example>
        /// <param name="attributeName">Indicates the attribute short name</param>
        void RemoveValue(String attributeName);

        /// <summary>
        /// Remove all the child values
        /// </summary>
        /// <example>
        /// <code language="c#" title="Remove Child Attribute Values"  source="..\..\Documentation\MDM.APISamples\EntityManager\ComplexAttribute\ComplexAttributeSamples.cs" region="Remove All Values of Child Attribute"/>
        /// </example>
        void RemoveValues();

        #endregion Utilities for clearing child attribute values

        #endregion
    }
}
