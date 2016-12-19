using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get relationship type.
    /// </summary>
    public interface IRelationshipType : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property to identify if the state of the related entity impacts the state of the source entity
        /// </summary>
        Boolean EnforceRelatedEntityStateOnSourceEntity { get; set; }

        /// <summary>
        /// Property to identify if the promote of the current entity requires the related entity to be promoted already
        /// </summary>
        Boolean CheckRelatedEntityPromoteStatusOnPromote { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// XML presentaion of RelationshipType Object
        /// </summary>
        /// <returns>XML presentaion of RelationshipType Object</returns>
        String ToXml();

        /// <summary>
        /// XML presentaion of RelationshipType Object
        /// </summary>
        /// <param name="serialization">Type of Object Serialization</param>
        /// <returns>XML presentaion of RelationshipType Object</returns>
        String ToXml(MDM.Core.ObjectSerialization serialization);

        /// <summary>
        /// Clone Relationship Type object
        /// </summary>
        /// <returns>cloned copy of RelationshipType object.</returns>
        IRelationshipType Clone();

        /// <summary>
        /// Delta Merge of relationship type
        /// </summary>
        /// <param name="deltaRelationshipType">RelationshipType that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="returnClonedObject">true means return new merged cloned object;otherwise, same object.</param>
        /// <returns>Merged relationship type instance</returns>
        IRelationshipType MergeDelta(IRelationshipType deltaRelationshipType, ICallerContext iCallerContext, Boolean returnClonedObject = true);

        #endregion
    }
}