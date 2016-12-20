using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get the collection of relationship denorm action.
    /// </summary>
    public interface IRelationshipDenormActionCollection : IEnumerable<RelationshipDenormAction>
    {
        #region Methods

        /// <summary>
        /// No. of relationship denorm action under current relationshipDenormAction collection
        /// </summary>
        Int32 Count { get; }

        /// <summary>
        /// Add new item into the collection
        /// </summary>
        /// <param name="item">IRelationship DenormAction</param>
        void Add(IRelationshipDenormAction item);

        /// <summary>
        /// Removes item from collection
        /// </summary>
        /// <param name="item">IRelationship DenormAction</param>
        /// <returns>[True] if removal was successful, [False] otherwise</returns>
        Boolean Remove(IRelationshipDenormAction item);

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of RelationshipDenormActionCollection object
        /// </summary>
        /// <returns>Xml string representing the RelationshipDenormActionCollection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of RelationshipDenormActionCollection object
        /// </summary>
        /// <param name="objectSerialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion ToXml methods

        #endregion Methods
    }
}
