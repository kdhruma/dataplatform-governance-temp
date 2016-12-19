using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get the collection of relationship.
    /// </summary>
    public interface IRelationshipCollection : IEnumerable<Relationship>
    {
        #region Properties

        /// <summary>
        /// Presents no. of entity present into the collection
        /// </summary>
        Int32 Count { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of  Relationship Collection
        /// </summary>
        /// <returns>Xml representation of  Relationship Collection</returns>
        String ToXml();

        /// <summary>
        /// Add relationship object in collection
        /// </summary>
        /// <param name="iRelationship"> Relationship to add in collection</param>
        void Add(IRelationship iRelationship);

        /// <summary>
        /// Add Relationships in collection
        /// </summary>
        /// <param name="items">Relationships to add in collection</param>
        void AddRange(IRelationshipCollection items);
        
        /// <summary>
        /// Get Xml representation of  Relationship Collection based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of  Relationship Collection</returns>
        String ToXml(ObjectSerialization objectSerialization);

        /// <summary>
        /// Get flat list of updated relationships from current relationship collection
        /// </summary>
        /// <returns>Flat list of updated relationships </returns>
        IRelationshipCollection GetUpdatedRelationships();

        /// <summary>
        /// Get flat list of all the related entity ids.
        /// </summary>
        /// <returns>List of Entity Ids</returns>
        Collection<Int64> GetRelatedEntityIdList();

        /// <summary>
        ///  Get list of relationshipType ids.
        /// </summary>
        /// <returns>List of Relationship Type Ids</returns>
        Collection<Int32> GetRelationshipTypeIds();

        /// <summary>
        /// Get the relationship based on the Relationship Id
        /// </summary>
        /// <param name="relationshipId">Indicate the relationship type id for the Relationship</param>
        /// <returns>Relationship Interface</returns>
        IRelationship GetRelationshipById(Int64 relationshipId);

        /// <summary>
        /// Get the relationship based on the 'from Entity Id + relationship Entity Id + Relationship type Id' combination
        /// </summary>
        /// <param name="relatedEntityId">Indicate the related Entity Id for the Relationship</param>
        /// <param name="relationshipTypeId">Indicate the relationship type Id for the Relationship</param>
        /// <returns>Relationship Interface</returns>
        IRelationship GetRelationship(Int64 relatedEntityId, Int32 relationshipTypeId);

        /// <summary>
        /// Denormalize relationships into flat structure
        /// </summary>
        /// <returns>De-normalized  Relationships</returns>
        IRelationshipCollection Denormalize();

        /// <summary>
        /// Marks all the relationships as readOnly
        /// </summary>
        void MarkAsReadOnly();

        /// <summary>
        /// Check if RelationshipCollection contains relationship with given context parameters
        /// </summary>
        /// <param name="fromEntityId">Id of the From Entity using which relationship is to be searched from collection</param>
        /// <param name="relatedEntityId">Id of the related entity using which relationship is to be searched from collection</param>
        /// <param name="containerId">Id of the container using which relationship is to be searched from collection</param>
        /// <param name="relationshipTypeId">Id of the relationship type using which relationship is to be searched from collection</param>
        /// <returns>
        /// <para>true : If relationship found in RelationshipCollection</para>
        /// <para>false : If relationship found not in RelationshipCollection</para>
        /// </returns>
        Boolean Contains(Int64 fromEntityId, Int64 relatedEntityId, Int32 containerId, Int32 relationshipTypeId);

        /// <summary>
        /// Check if RelationshipCollection contains relationship with given related entity id
        /// </summary>
        /// <param name="relatedEntityId">Id of the related entity using which relationship is to be searched from collection</param>
        /// <returns>
        /// <para>true : If relationship found in RelationshipCollection</para>
        /// <para>false : If relationship found not in RelationshipCollection</para>
        /// </returns>
        Boolean Contains(Int64 relatedEntityId);

        #endregion
    }
}
