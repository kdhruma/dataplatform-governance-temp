using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties used for providing container entity type attribute mapping related information.
    /// </summary>
    public interface IContainerEntityTypeAttributeMapping : IMDMObject , IEntityTypeAttributeMapping
    {
        #region Properties

        /// <summary>
        /// Property denoting Organization Id.
        /// </summary>
        Int32 OrganizationId { get; set; }

        /// <summary>
        /// Property denoting Organization Short Name.
        /// </summary>
        String OrganizationName { get; set; }

        /// <summary>
        /// Property denoting Organization Long Name.
        /// </summary>
        String OrganizationLongName { get; set; }

        /// <summary>
        ///  Property denoting Container Id.
        /// </summary>
        Int32 ContainerId { get; set; }

        /// <summary>
        /// Property denoting Container Short Name.
        /// </summary>
        String ContainerName { get; set; }

        /// <summary>
        /// Property denoting Container Long Name.
        /// </summary>
        String ContainerLongName { get; set; }

        /// <summary>
        /// Property denoting Is Specialized.
        /// </summary>
        Boolean IsSpecialized { get; set; }

        /// <summary>
        /// Property denoting Inheritable only.
        /// </summary>
        Boolean InheritableOnly { get; set; }

        /// <summary>
        /// Property denoting Auto Promotable only.
        /// </summary>
        Boolean AutoPromotable { get; set; }

        #endregion

        #region Methods

        #region ToXml methods

        /// <summary>
        /// Get XML representation of ContainerEntityTypeAttributeMapping object
        /// </summary>
        /// <returns>XML representation of ContainerEntityTypeAttributeMapping object</returns>
        new String ToXML();

        #endregion

        /// <summary>
        /// Clone ContainerEntityTypeAttributeMapping object
        /// </summary>
        /// <returns>cloned copy of IContainerEntityTypeAttributeMapping object.</returns>
        new IContainerEntityTypeAttributeMapping Clone();

        /// <summary>
        /// Delta Merge of ContainerEntityTypeAttributeMapping
        /// </summary>
        /// <param name="deltaContainerEntityTypeAttributeMapping">ContainerEntityTypeAttributeMapping that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="returnClonedObject">true means return new merged cloned object;otherwise, same object.</param>
        /// <returns>Merged IContainerEntityTypeAttributeMapping instance</returns>
        IContainerEntityTypeAttributeMapping MergeDelta(IContainerEntityTypeAttributeMapping deltaContainerEntityTypeAttributeMapping, ICallerContext iCallerContext, Boolean returnClonedObject = true);

        #endregion
    }
}