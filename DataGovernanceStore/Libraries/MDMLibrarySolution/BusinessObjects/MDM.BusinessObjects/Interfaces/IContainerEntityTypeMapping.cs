using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties used for providing container entity type mapping related information.
    /// </summary>
    public interface IContainerEntityTypeMapping : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property denoting mappedOrganization id
        /// </summary>
        Int32 OrganizationId { get; set; }

        /// <summary>
        /// FProperty denoting mapped Organization Name
        /// </summary>
        String OrganizationName { get; set; }

        /// <summary>
        /// Property denoting mapped Container Id
        /// </summary>
        Int32 ContainerId { get; set; }

        /// <summary>
        /// Property denoting the mapped Container Name
        /// </summary>
        String ContainerName { get; set; }

        /// <summary>
        /// Property denoting mapped Entity Type Id
        /// </summary>
        Int32 EntityTypeId { get; set; }

        /// <summary>
        /// Property denoting the mapped Entity Type Name
        /// </summary>
        String EntityTypeName { get; set; }

        /// <summary>
        /// Property denoting if the selected entity type can be shown at creation time
        /// </summary>
        Boolean ShowAtCreation { get; set; }

        /// <summary>
        /// Property denoting minimum occurance of entity in different categories in a container
        /// </summary>
        Int32 MinimumExtensions { get; set; }

        /// <summary>
        /// Property denoting maximum occurance of entity in different categories in a container
        /// </summary>
        Int32 MaximumExtensions { get; set; }

        #endregion

        #region Methods

        #region ToXml methods

        /// <summary>
        /// Get XML representation of ContainerEntityTypeMapping object
        /// </summary>
        /// <returns>XML representation of ContainerEntityTypeMapping object</returns>
        String ToXml();

        /// <summary>
        /// Clone ContainerEntityTypeMapping object
        /// </summary>
        /// <returns>cloned copy of IContainerEntityTypeMapping object.</returns>
        IContainerEntityTypeMapping Clone();

        /// <summary>
        /// Delta Merge of ContainerEntityTypeMapping
        /// </summary>
        /// <param name="deltaContainerEntityTypeMapping">ContainerEntityTypeMapping that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="returnClonedObject">true means return new merged cloned object;otherwise, same object.</param>
        /// <returns>Merged IContainerEntityTypeMapping instance</returns>
        IContainerEntityTypeMapping MergeDelta(IContainerEntityTypeMapping deltaContainerEntityTypeMapping, ICallerContext iCallerContext, Boolean returnClonedObject = true);

        #endregion

        #endregion
    }
}