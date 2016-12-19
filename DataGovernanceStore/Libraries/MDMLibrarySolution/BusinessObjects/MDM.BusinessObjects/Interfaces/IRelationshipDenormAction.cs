using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get relationship denorm actions.
    /// </summary>
    public interface IRelationshipDenormAction : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Specifies Activity performed on Entity like AttributeUpdate etc
        /// </summary>
        new EntityActivityList Action { get; set; }

        /// <summary>
        /// Specifies processing mode of Extension of an entity
        /// </summary>
        ProcessingMode ExtensionProcessingMode { get; set; }

        /// <summary>
        /// Specifies processing mode of Hierarchy in Entity 
        /// </summary>
        ProcessingMode HierarchyProcessingMode { get; set; }

        /// <summary>
        /// Specifies processing mode of WhereUsed relationships in Entity 
        /// </summary>
        ProcessingMode WhereUsedProcessingMode { get; set; }

        /// <summary>
        /// Specifies processing mode of RelationshipTree in Entity 
        /// </summary>
        ProcessingMode RelationshipTreeProcessingMode { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Get Xml representation of Relationship Denorm Action
        /// </summary>
        /// <returns>Xml representation of Relationship Denorm Action</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of Relationship Denorm Action based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Relationship Denorm Action</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion Methods
    }
}
