using System;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get the container relationship type attribute mapping.
    /// </summary>
    public interface IContainerRelationshipTypeAttributeMapping : IRelationshipTypeAttributeMapping
    {
        #region Properties

        /// <summary>
        /// Property denoting Organization Id.
        /// </summary>
        Int32 OrganizationId { get; set; }

        /// <summary>
        /// Property denoting Organization Name.
        /// </summary>
        String OrganizationName { get; set; }

        /// <summary>
        /// Property denoting Organization Long Name.
        /// </summary>
        String OrganizationLongName { get; set; }

        /// <summary>
        /// Property denoting the Container Id.
        /// </summary>
        Int32 ContainerId { get; set; }

        /// <summary>
        /// Property denoting Container Short Name.
        /// </summary>
        String ContainerName { get; set; }

        /// <summary>
        /// Property denoting the Container Name.
        /// </summary>
        String ContainerLongName { get; set; }

        /// <summary>
        /// Property denoting Is Specialized.
        /// </summary>
        Boolean IsSpecialized { get; set; }

        /// <summary>
        /// Property denoting Auto Promotable only.
        /// </summary>
        Boolean AutoPromotable { get; set; }

        #endregion

        #region Methods

        #region ToXml methods

        /// <summary>
        /// Get XML representation of ContainerRelationshipTypeAttributeMapping object
        /// </summary>
        /// <returns>XML representation of ContainerRelationshipTypeAttributeMapping object</returns>
        new String ToXML();

        #endregion

        /// <summary>
        /// Clone ContainerRelationshipTypeAttributeMapping object
        /// </summary>
        /// <returns>cloned copy of IContainerRelationshipTypeAttributeMapping object.</returns>
        new IContainerRelationshipTypeAttributeMapping Clone();

        /// <summary>
        /// Delta Merge of ContainerRelationshipTypeAttributeMapping
        /// </summary>
        /// <param name="deltaContainerRelationshipTypeAttributeMapping">ContainerRelationshipTypeAttributeMapping that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="returnClonedObject">true means return new merged cloned object;otherwise, same object.</param>
        /// <returns>Merged IContainerRelationshipTypeAttributeMapping instance</returns>
        IContainerRelationshipTypeAttributeMapping MergeDelta(IContainerRelationshipTypeAttributeMapping deltaContainerRelationshipTypeAttributeMapping, ICallerContext iCallerContext, Boolean returnClonedObject = true);

        #endregion
    }
}