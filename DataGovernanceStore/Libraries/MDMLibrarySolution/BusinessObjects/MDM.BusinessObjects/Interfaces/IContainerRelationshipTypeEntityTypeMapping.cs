using System;

namespace MDM.BusinessObjects.Interfaces
{
    using MDM.Interfaces;

    /// <summary>
    /// Exposes methods or properties to set or get the container relationship type entity type mapping related information.
    /// </summary>
    public interface IContainerRelationshipTypeEntityTypeMapping : IRelationshipTypeEntityTypeMapping
    {
        #region Properties

        /// <summary>
        /// Property denoting mapped Organization Id
        /// </summary>
        Int32 OrganizationId { get; set; }

        /// <summary>
        /// Property denoting the mapped Organization Name
        /// </summary>
        String OrganizationName { get; set; }

        /// <summary>
        /// Property denoting the mapped Organization long name
        /// </summary>
        String OrganizationLongName { get; set; }

        /// <summary>
        /// Property denoting mapped Container Id
        /// </summary>
        Int32 ContainerId { get; set; }

        /// <summary>
        /// Property denoting the mapped Container Name
        /// </summary>
        String ContainerName { get; set; }

        /// <summary>
        /// Property denoting the mapped Container long name
        /// </summary>
        String ContainerLongName { get; set; }

        /// <summary>
        /// Property denoting if mapping is specialized for an container or derived for container based on relationshipType-entityType mapping. 
        /// </summary>
        Boolean IsSpecialized { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of IContainerRelationshipTypeEntityTypeMappingCollection object
        /// </summary>
        /// <returns>Xml string representing the IContainerRelationshipTypeEntityTypeMappingCollection</returns>
        new String ToXML();

        
        /// <summary>
        /// Clone ContainerRelationshipTypeEntityTypeMapping object
        /// </summary>
        /// <returns>cloned copy of IContainerRelationshipTypeEntityTypeMapping object.</returns>
        new IContainerRelationshipTypeEntityTypeMapping Clone();

        /// <summary>
        /// Delta Merge of ContainerRelationshipTypeEntityTypeMapping
        /// </summary>
        /// <param name="deltaContainerRelationshipTypeEntityTypeMapping">ContainerRelationshipTypeEntityTypeMapping that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="returnClonedObject">true means return new merged cloned object;otherwise, same object.</param>
        /// <returns>Merged IContainerRelationshipTypeEntityTypeMapping instance</returns>
        IContainerRelationshipTypeEntityTypeMapping MergeDelta(IContainerRelationshipTypeEntityTypeMapping deltaContainerRelationshipTypeEntityTypeMapping, ICallerContext iCallerContext, Boolean returnClonedObject = true);

        #endregion
    }
}
