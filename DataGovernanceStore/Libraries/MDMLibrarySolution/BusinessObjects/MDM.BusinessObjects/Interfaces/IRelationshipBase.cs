using System;
using System.Collections.Generic;
using System.Linq;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get the base relationship information.
    /// </summary>
    public interface IRelationshipBase : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property denoting the path of the relationship object
        /// </summary>
        String Path { get; set; }

        /// <summary>
        /// Property denoting the direction of the relationship
        /// </summary>
        RelationshipDirection Direction { get; set; }

        /// <summary>
        /// Property denoting the id of the related entity
        /// </summary>
        Int64 RelatedEntityId { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of Relationship object
        /// </summary>
        /// <returns>Xml representation of Relationship object</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of Relationship object based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Relationship object</returns>
        String ToXml(ObjectSerialization objectSerialization);

        /// <summary>
        /// Gets the relationship type
        /// </summary>
        /// <returns>Relationship type interface</returns>
        IRelationshipType GetRelationshipType();

        /// <summary>
        /// Sets the relationship type
        /// </summary>
        /// <param name="iRelationshipType">Relationship type interface</param>
        /// <exception cref="ArgumentNullException">Raised when passed relationship type is null</exception>
        void SetRelationshipType(IRelationshipType iRelationshipType);

        /// <summary>
        /// Gets relationships
        /// </summary>
        /// <returns>Relationship collection interface</returns>
        IRelationshipBaseCollection GetRelationships();

        /// <summary>
        /// Sets relationships
        /// </summary>
        /// <param name="iRelationshipCollection">Relationship collection interface</param>
        /// <exception cref="ArgumentNullException">Raised when passed relationship collection is null</exception>
        void SetRelationships(IRelationshipBaseCollection iRelationshipCollection);

        /// <summary>
        /// Gets the related entity
        /// </summary>
        /// <returns>Related entity interface</returns>
        IEntity GetRelatedEntity();

        /// <summary>
        /// Sets the related entity
        /// </summary>
        /// <param name="iEntity">Related entity interface</param>
        /// <exception cref="ArgumentNullException">Raised when passed entity is null</exception>
        void SetRelatedEntity(IEntity iEntity);

        #endregion
    }
}
