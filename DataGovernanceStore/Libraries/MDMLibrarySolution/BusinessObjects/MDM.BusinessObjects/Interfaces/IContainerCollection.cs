using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Exposes methods or properties used for providing container collection information.
    /// </summary>
    public interface IContainerCollection : IEnumerable<Container>
    {
        #region Properties

        /// <summary>
        /// Property denoting the count of container collection.
        /// </summary>
        Int32 Count { get; }
        
        /// <summary>
        /// Indicates allowed user actions on the object
        /// </summary>
        Collection<UserAction> PermissionSet { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of ContainerCollection object
        /// </summary>
        /// <returns>Xml string representing the ContainerCollection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of ContainerCollection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization serialization);

        /// <summary>
        /// Add container in collection
        /// </summary>
        /// <param name="iContainer">container to add in collection</param>
        void Add(IContainer iContainer);

        /// <summary>
        /// GetContainer by Id
        /// </summary>
        /// <param name="containerId">Id of an container</param>
        /// <returns>IContainer</returns>
        IContainer GetIContainer(Int32 containerId);

        /// <summary>
        /// Clone Container collection.
        /// </summary>
        /// <returns>cloned Container collection object.</returns>
        IContainerCollection Clone();

        /// <summary>
        /// Get containers based on container type or qualifier name
        /// </summary>
        /// <param name="containerType">Indicates type of container</param>
        /// <param name="containerQualifierName">Indicates name of container qualifier</param>
        /// <returns>Returns collection of containers based on given container type and container qualifier name.</returns>
        IContainerCollection GetByContainerTypeAndQualifier(ContainerType? containerType, String containerQualifierName);

        /// <summary>
        /// Get child containers hierarchy based on given container identifier and flag denoting that load immediate child only or full hierarchy
        /// </summary>
        /// <param name="containerId">Indicates container identifier</param>
        /// <param name="loadRecursive">Indicates whether load immediate child only or full hierarchy</param>
        /// <returns>Returns collection of child containers hierarchy based on given container identifier and flag denoting that load immediate child only or full hierarchy.</returns>
        IContainerCollection GetChildContainers(Int32 containerId, Boolean loadRecursive);

        /// <summary>
        /// Get container hierarchy based on given container identifier
        /// </summary>
        /// <param name="containerId">Indicates container identifier</param>
        /// <returns>Returns collection of container hierarchy based on given container identifier.</returns>
        IContainerCollection GetContainerHierarchy(Int32 containerId);

        #endregion
    }
}