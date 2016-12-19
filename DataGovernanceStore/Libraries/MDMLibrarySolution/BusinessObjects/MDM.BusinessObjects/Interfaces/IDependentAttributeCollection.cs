using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get dependent attribute collection.
    /// </summary>
    public interface IDependentAttributeCollection : IEnumerable<DependentAttribute>
    {
        #region Properties

        /// <summary>
        /// Get the count of no. of DependentAttribute in DependentAttributeCollection
        /// </summary>
        int Count{get;}
        
        #endregion

        #region Methods

        /// <summary>
        /// Add 'IDependentattribute' object in collection
        /// </summary>
        /// <param name="item">Dependent attribute to add in collection</param>
        void Add(IDependentAttribute item);
        
        /// <summary>
        /// Get Xml representation of Dependent Attribute Collection
        /// </summary>
        /// <returns>Xml representation of Dependent Attribute Collection</returns>
        String ToXml();

        /// <summary>
        /// Filter only 'Dependent Of' Attributes.
        /// Will return only the child dependent attribute details
        /// </summary>
        /// <param name="items">DependentAttributeCollection which contains all(parent and child) details</param>
        /// <param name="dependencyType">Type of the dependency type</param>
        /// <returns>IDependentAttributeCollection</returns>
        IDependentAttributeCollection FilterDependentAttributes(IDependentAttributeCollection items, DependencyType dependencyType);

        /// <summary>
        /// Get dependent attribute based on attribute id.
        /// </summary>
        /// <param name="attributeId">Id to filter on</param>
        /// <returns>Dependent attribute for given Id</returns>
        IDependentAttribute GetDependentAttribute(Int32 attributeId);

        /// <summary>
        /// Get dependent attribute based on dependency id
        /// </summary>
        /// <param name="Id">Indicates identifier of dependency which need to filter on</param>
        /// <returns>returns dependent attribute for given dependency id</returns>
        IDependentAttribute GetDependentAttributeByDependencyId(Int32 Id);

        #endregion
    }
}
