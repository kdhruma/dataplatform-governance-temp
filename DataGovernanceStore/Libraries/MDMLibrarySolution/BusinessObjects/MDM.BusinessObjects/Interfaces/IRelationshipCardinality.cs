using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get relationship cardinality.
    /// </summary>
    public interface IRelationshipCardinality : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property denoting the RelationshipTypeEntityTypeMapping Id
        /// </summary>
        Int32 RelationshipTypeEntityTypeMappingId { get; set; }

        /// <summary>
        /// Property denoting the ToEntityTypeId
        /// </summary>
        Int32 ToEntityTypeId { get; set; }        

        /// <summary>
        /// Property denoting the To EntityType LongName
        /// </summary>
        String ToEntityTypeName { get; set; }
        
        /// <summary>
        /// Property denoting the To EntityType LongName
        /// </summary>
        String ToEntityTypeLongName { get; set; }
        
        /// <summary>
        /// Property denoting the Minimum Relationships allowed 
        /// </summary>
        Int32 MinRelationships { get; set; }
        
        /// <summary>
        /// Property denoting the Maximum Relationships allowed 
        /// </summary>
        Int32 MaxRelationships { get; set; }
        
        /// <summary>
        /// Property denoting the RelationshipTypeId
        /// </summary>
        Int32 RelationshipTypeId { get; set; }
        
        /// <summary>
        /// Property denoting the container id
        /// </summary>
        Int32 ContainerId { get; set; }
        
        /// <summary>
        /// Property denoting the From EntityTypeId
        /// </summary>
        Int32 FromEntityTypeId { get; set; }

#endregion

        #region Methods

        /// <summary>
        /// Represents Relationshipcardinality  in XML format
        /// </summary>
        /// <returns>String representation of current Relationshipcardinality object</returns>
        String ToXml();

        #endregion 
    }
}
