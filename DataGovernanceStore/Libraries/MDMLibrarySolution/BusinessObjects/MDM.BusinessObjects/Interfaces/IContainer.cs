using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get container.
    /// </summary>
    public interface IContainer : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property denoting the object type of container
        /// </summary>
        String ObjectType { get; }

        /// <summary>
        /// Property denoting the organization id of container
        /// </summary>
        Int32 OrganizationId { get; set; }

        /// <summary>
        /// Property denoting the organization short name of container
        /// </summary>
        String OrganizationShortName { get; set; }

        /// <summary>
        /// Property denoting the organization long name of container
        /// </summary>
        String OrganizationLongName { get; set; }

        /// <summary>
        /// Property denoting the hierarchy id of container
        /// </summary>
        Int32 HierarchyId { get; set; }

        /// <summary>
        /// Property denoting the hierarchy short name of container
        /// </summary>
        String HierarchyShortName { get; set; }

        /// <summary>
        /// Property denoting the hierarchy long name of container
        /// </summary>
        String HierarchyLongName { get; set; }

        /// <summary>
        /// Property denoting the is default of container
        /// </summary>
        Boolean IsDefault { get; set; }

        /// <summary>
        /// Property denoting security object type id of container
        /// </summary>
        Int32 SecurityObjectTypeId { get; set; }

        /// <summary>
        /// Property denoting is staging.
        /// </summary>
        Boolean IsStaging { get; set; }

        /// <summary>
        /// Property denoting type of container
        /// </summary>
        ContainerType ContainerType { get; set; }

        /// <summary>
        /// Property denoting container qualifier identifier
        /// </summary>
        Int32 ContainerQualifierId { get; set; }

        /// <summary>
        /// Property denoting name of container qualifier
        /// </summary>
        String ContainerQualifierName { get; set; }

        /// <summary>
        /// Property denoting list of secondary container qualifier
        /// </summary>
        Collection<String> ContainerSecondaryQualifiers { get; set; }

        /// <summary>
        /// Property denoting parent container identifier
        /// </summary>
        Int32 ParentContainerId { get; set; }

        /// <summary>
        /// Property denoting name of parent container
        /// </summary>
        String ParentContainerName { get; set; }

        /// <summary>
        /// Property denoting that for this container approved copy is required or not
        /// </summary>
        Boolean NeedsApprovedCopy { get; set; }

        /// <summary>
        /// Property denoting type of workflow
        /// </summary>
        WorkflowType WorkflowType { get; set; }

        /// <summary>
        /// Property denoting cross reference id between approved and collaboration container
        /// </summary>
        Int32 CrossReferenceId { get; set; }

        /// <summary>
        /// Specifies level of container
        /// </summary>
        Int32 Level { get; }

        /// <summary>
        /// Property denoting whether container is approved or not
        /// </summary>
        Boolean IsApproved { get; }

        /// <summary>
        /// Specifies whether the auto extension is enabled or not.
        /// </summary>
        Boolean AutoExtensionEnabled { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of ContainerTempleteCopyContext object
        /// </summary>
        /// <returns>Xml representation of object</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of ContainerTempleteCopyContext object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization serialization);

        /// <summary>
        /// Clone Container object.
        /// </summary>
        /// <returns>cloned container object.</returns>
        IContainer Clone();

        /// <summary>
        /// Gets the attributes belonging to the Container
        /// </summary>
        /// <returns>Attribute Collection Interface</returns>
        IAttributeCollection GetAttributes();

        /// <summary>
        /// Gets attribute with specified attribute Id from current container's attributes
        /// </summary>
        /// <param name="attributeId">Id of an attribute to search in current container's attributes</param>
        /// <returns>Attribute interface</returns>
        /// <exception cref="ArgumentException">Attribute Id must be greater than 0</exception>
        /// <exception cref="NullReferenceException">Attributes for container is null. There are no attributes to search in</exception>
        IAttribute GetAttribute(Int32 attributeId);

        /// <summary>
        /// Delta Merge of container
        /// </summary>
        /// <param name="deltaContainer">Container that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="returnClonedObject">true means return new merged cloned object;otherwise, same object.</param>
        /// <returns>Merged container instance</returns>
        IContainer MergeDelta(IContainer deltaContainer, ICallerContext iCallerContext, Boolean returnClonedObject = true);

        /// <summary>
        /// Gets the supported locales of the container
        /// </summary>
        /// <returns>Supported locale collection</returns>
        ILocaleCollection GetSupportedLocales();

        /// <summary>
        /// Sets the supported locales of the container
        /// </summary>
        /// <param name="supportedLocales">Indicates the supported locale collection</param>
        void SetSupportedLocales(ILocaleCollection supportedLocales);

        #endregion
    }
}