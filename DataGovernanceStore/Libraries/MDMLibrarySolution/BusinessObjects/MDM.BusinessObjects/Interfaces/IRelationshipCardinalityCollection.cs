using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get the collection of relationship cardinality.
    /// </summary>
    public interface IRelationshipCardinalityCollection : IEnumerable<RelationshipCardinality>
    {
        /// <summary>
        /// No. of relationship under current relationshipCardinality collection
        /// </summary>
        Int32 Count { get; }

        /// <summary>
        /// Add new item into the collection
        /// </summary>
        /// <param name="item">IRelationship Cardinality</param>
        void Add(IRelationshipCardinality item);

        /// <summary>
        /// Verify whether or not sequence contain required item
        /// </summary>
        /// <param name="Id">Id of relationshipCardinality</param>
        /// <returns>[True] if item exists, [False] otherwise</returns>
        Boolean Contains(Int32 Id);

        /// <summary>
        /// Compare collection with another collection
        /// </summary>
        /// <param name="obj">Collection to compare</param>
        /// <returns>[True] if collections equal and [False] otherwise</returns>
        Boolean Equals(Object obj);

        /// <summary>
        /// Gets hash code of the item
        /// </summary>
        /// <returns>Gets hash code of the item</returns>
        Int32 GetHashCode();

        /// <summary>
        /// Get By Id
        /// </summary>
        /// <param name="Id">RelationCardinality Id</param>
        /// <returns>IRelationshipCardinality</returns>
        IRelationshipCardinality GetRelationshipCardinality(int Id);

        /// <summary>
        /// Removes item by id
        /// </summary>
        /// <param name="relationshipCardinalityId">Id of relationship Cardinality</param>
        /// <returns>[True] if removal was successful, [False] otherwise</returns>
        Boolean Remove(Int32 relationshipCardinalityId);

        /// <summary>
        /// Convert item to XML
        /// </summary>
        /// <returns>Convert item to XML</returns>
        String ToXml();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromEntityTypeId"></param>
        /// <returns></returns>
        IRelationshipCardinalityCollection GetRelationshipCardinalityByEntityTypeId(Int32 fromEntityTypeId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationshipTypeId"></param>
        /// <returns></returns>
        IRelationshipCardinalityCollection GetRelationshipCardinalityByRelationshipTypeId(Int32 relationshipTypeId);
    }
}
