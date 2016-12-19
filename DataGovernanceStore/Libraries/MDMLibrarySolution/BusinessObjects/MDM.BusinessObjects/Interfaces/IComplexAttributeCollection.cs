using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties used for providing complex attribute collection related information.
    /// </summary>
    public interface IComplexAttributeCollection : IEnumerable
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Removes all the child attribute values
        /// </summary>
        /// <example>
        /// <code language="c#" title="Remove Child Attribute Values"  source="..\..\Documentation\MDM.APISamples\EntityManager\ComplexAttribute\ComplexAttributeSamples.cs" region="Clear All ChildValue For Collection"/>
        /// <code language="c#" title="Get Entity"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity"/>
        /// </example>
        void RemoveValues();

        /// <summary>
        /// Get all the child attribute's current values based on the child attribute short name
        /// </summary>
        /// <param name="shortName">Indicates the child attribute short name</param>
        /// <typeparam name="T">Indicates the data type of the result</typeparam>
        /// <example>
        /// <code language="c#" title="Get Child Attributes Current Values in Requested Type"  source="..\..\Documentation\MDM.APISamples\EntityManager\ComplexAttribute\ComplexAttributeSamples.cs" region="Get Complex Child Attribute Values From Complex Attribute In Requested Type"/>
        /// <code language="c#" title="Get Entity"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity"/>
        /// </example>
        /// <returns>Returns the child attribute's current values as requested data type</returns>
        Collection<T> GetChildAttributeCurrentValues<T>(String shortName);

        /// <summary>
        /// Get all the child attribute's current invariant value based on the child attribute short name
        /// </summary>
        /// <param name="shortName">Indicates the child attribute short name</param>
        /// <typeparam name="T">Indicates the data type of the result</typeparam>
        /// <example>
        /// <code language="c#" title="Get Child Attributes Current Invariant Values in Requested Type"  source="..\..\Documentation\MDM.APISamples\EntityManager\ComplexAttribute\ComplexAttributeSamples.cs" region="Get Complex Child Attribute Values Invariant From Complex Attribute"/>
        /// <code language="c#" title="Get Entity"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity"/>
        /// </example>
        /// <returns>Returns the child attribute's current invariant values as requested data type</returns>
        Collection<T> GetChildAttributeCurrentValuesInvariant<T>(String shortName);
                      
        /// <summary>
        /// Adds a new complex record with specified child attributes
        /// </summary>
        /// <example>
        ///  <code language="c#" title="Add New Row"  source="..\..\Documentation\MDM.APISamples\EntityManager\ComplexAttribute\ComplexAttributeSamples.cs" region="Add NewRow For ComplexAttributeCollection"/>
        ///  <code language="c#" title="Get Entity with Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>          
        /// </example>
        /// <param name="childAttributes">Indicates the child attributes</param>
        void AddRow(IAttributeCollection childAttributes);

        /// <summary>
        /// Adds a new complex record with specified child attributes as key values format
        /// </summary>
        /// <example> 
        /// <code language="c#" title="Add New Row as a Dictionary object for Complex Attribute Collection"  source="..\..\Documentation\MDM.APISamples\EntityManager\ComplexAttribute\ComplexAttributeSamples.cs" region="Add NewRow As A Dictionary For ComplexAttributeCollection"/>
        /// <code language="c#" title="Get Entity with Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>          
        /// </example>
        /// <param name="values">Indicates the child attributes as key values format. Key is child attribute short name and values is child attribute value</param>
        void AddRow(Dictionary<String, Object> values);

        /// <summary>
        /// Adds a new complex record with specified child attributes as key values format
        /// </summary>
        /// <param name="values">Indicates the child attributes as key values format. Key is child attribute short name and values is child attribute value</param>
        void AddRow(Dictionary<String, IValue> values);

        #endregion Methods
    }
}
