using System;
using System.Collections.Generic;
using System.Linq;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get hierarchy relationship collection.
    /// </summary>
    public interface IHierarchyRelationshipCollection : IEnumerable<HierarchyRelationship>
    {
        #region Properties

        /// <summary>
        /// Property denoting action
        /// </summary>
        ObjectAction Action { get; set; }

        /// <summary>
        /// Property denoting EntityChangeContext
        /// </summary>
        EntityChangeContext ChangeContext { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of Hierarchy Relationship Collection
        /// </summary>
        /// <returns>Xml representation of Hierarchy Relationship Collection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of Hierarchy Relationship Collection based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Hierarchy Relationship Collection</returns>
        String ToXml(ObjectSerialization objectSerialization);

        /// <summary>
        /// Gets only entity hierarchy relationships from the hierarchy relationships
        /// </summary>
        /// <returns>Entity hierarchy relationships</returns>
        IHierarchyRelationshipCollection GetEntityHierarchyRelationships();

        /// <summary>
        /// Filters hierarchy relationships based on direction
        /// </summary>
        /// <param name="direction">Direction by which relationships needs to be filtered.
        /// <para>
        /// Applicable values are 'Up' and 'Down'
        /// </para>
        /// </param>
        /// <returns>Filtered Hierarchy Relationships</returns>
        IHierarchyRelationshipCollection FilterBy(RelationshipDirection direction);

        #endregion
    }
}
