using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDM.BusinessObjects;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get the relationship information.
    /// </summary>
    public interface IRelationship : IRelationshipBase
    {
        #region Properties

        /// <summary>
        /// Property denoting the Id of an entity
        /// </summary>
        new Int64 Id { get; set; }

        /// <summary>
        /// Property denoting source flag of relationship
        /// </summary>
        AttributeValueSource SourceFlag { get; set; }

        /// <summary>
        /// Property denoting relationship type id
        /// </summary>
        Int32 RelationshipTypeId { get; set; }

        /// <summary>
        /// Property denoting relationship type name
        /// </summary>
        String RelationshipTypeName { get; set; }

        /// <summary>
        /// Property denoting the type Id of To entity
        /// </summary>
        Int32 ToEntityTypeId { get; set; }

        /// <summary>
        /// Property denoting the type name of To entity
        /// </summary>
        String ToEntityTypeName { get; set; }

        /// <summary>
        /// Property denoting the ExternalId of To entity
        /// </summary>
        String ToExternalId { get; set; }

        /// <summary>
        /// Property for external purpose. This can be used for putting temporary value
        /// </summary>
        String ExtendedProperties { get; set; }

        /// <summary>
        /// Gets and Sets source info of changes to relationship collection
        /// </summary>
        SourceInfo SourceInfo { get; set; }

        /// <summary>
        /// Property denoting the reference id of the relationship
        /// </summary>
        new Int64 ReferenceId { get; set; }

        /// <summary>
        /// Property denotes cross reference id between approved and collaboration of relationship
        /// </summary>
        Int64 CrossReferenceId { get; set; }

        /// <summary>
        /// Property denotes cross reference id between approved and collaboration of from entity
        /// </summary>
        Int64 FromCrossReferenceId { get; set; }

        /// <summary>
        /// Property denotes cross reference id between approved and collaboration of related entity
        /// </summary>
        Int64 RelatedCrossReferenceId { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets relationships
        /// </summary>
        /// <returns> Relationship collection interface</returns>
        new IRelationshipCollection GetRelationships();

        /// <summary>
        /// Sets relationships
        /// </summary>
        /// <param name="iRelationshipCollection"> Relationship collection interface</param>
        /// <exception cref="ArgumentNullException">Raised when passed relationship collection is null</exception>
        void SetRelationships(IRelationshipCollection iRelationshipCollection);

        /// <summary>
        /// Gets relationship attributes
        /// </summary>
        /// <returns> Relationship attribute collection</returns>
        IAttributeCollection GetRelationshipAttributes();

        /// <summary>
        /// Checks whether current object or its child objects have been changed i.e any object having Action flag as Create, Update or Delete
        /// </summary>
        /// <returns>Return true if object is changed else false</returns>
        Boolean HasObjectChanged();

        #endregion
    }
}
