using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.Core.Extensions;

    /// <summary>
    /// Specifies Container Collection.
    /// </summary>
    [DataContract]
    public class ContainerCollection : ICollection<Container>, IEnumerable<Container>, IContainerCollection , IDataModelObjectCollection
    {
        #region Fields

        //This is the most funny part of the WCF which is able to serialize even a private member.. needs to do further RnD to find out the internals
        [DataMember]
        private Collection<Container> _containers = new Collection<Container>();

        /// <summary>
        /// Field for allowed user actions on the object
        /// </summary>
        private Collection<UserAction> _permissionSet = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public ContainerCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public ContainerCollection(String valueAsXml)
        {
            LoadContainerCollection(valueAsXml);
        }

        /// <summary>
        /// Constructor which takes List of type Container
        /// </summary>
        /// <param name="containers"></param>
        public ContainerCollection(IList<Container> containers)
        {
            this._containers = new Collection<Container>(containers);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Indicates allowed user actions on the object
        /// </summary>
        [DataMember]
        public Collection<UserAction> PermissionSet
        {
            get { return _permissionSet; }
            set { _permissionSet = value; }
        }

        /// <summary>
        /// Find container from ContainerCollection based on containerId
        /// </summary>
        /// <param name="containerId">containerId to search</param>
        /// <returns>Container object having given containerId</returns>
        public Container this[Int32 containerId]
        {
            get
            {
                Container container = GetContainer(containerId);
                if (container == null)
                    throw new ArgumentException(String.Format("No container found for container id: {0}", containerId), "containerId");
                else
                    return container;
            }
            set
            {
                Container container = GetContainer(containerId);

                if (container == null)
                    throw new ArgumentException(String.Format("No container found for container id: {0}", containerId), "containerId");

                container = value;
            }
        }

        /// <summary>
        /// Find container from ContainerCollection based on containerName without considering organization name.
        /// </summary>
        /// <param name="containerName">containerName to search</param>
        /// <returns>Container collection having given containerName</returns>
        public ContainerCollection this[String containerName]
        {
            get
            {
                ContainerCollection containers = GetContainers(containerName);

                if (containers == null || (containers != null && containers.Count < 1))
                    throw new ArgumentException(String.Format("No container found for container Name: {0}", containerName), "ContainerName");
                else
                    return containers;
            }
            set
            {
                ContainerCollection containers = GetContainers(containerName);

                if (containers == null || (containers != null && containers.Count < 0))
                    throw new ArgumentException(String.Format("No container found for container Name: {0}", containerName), "ContainerName");

                containers = value;
            }
        }

        /// <summary>
        /// Find container from ContainerCollection based on container and organization short name
        /// </summary>
        /// <param name="containerName">Container Name to search</param>
        /// <param name="organizationName">Organization Name to search</param>
        /// <returns>Container object having given containerName</returns>
        public Container this[String containerName, String organizationName]
        {
            get
            {
                Container container = GetContainer(containerName, organizationName);

                if (container == null)
                    throw new ArgumentException(String.Format("No container found for container Name: {0} and Organization Name: {1}", containerName, organizationName), "ContainerName");
                else
                    return container;
            }
            set
            {
                Container container = GetContainer(containerName, organizationName);

                if (container == null)
                    throw new ArgumentException(String.Format("No container found for container Name: {0} and Organization Name: {1}", containerName, organizationName), "ContainerName");

                container = value;
            }
        }

        /// <summary>
        /// <returns>Container collection wrapped  by the ContainerCollection instance </returns>
        /// </summary>
        public Collection<Container> ContainerCollectionValue
        {
            get { return this._containers; }
        }

        /// <summary>
        /// Property denoting a ObjectType for an dataModelObject object
        /// </summary>
        public ObjectType DataModelObjectType
        {
            get
            {
                return MDM.Core.ObjectType.Catalog;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Remove container object from ContainerCollection
        /// </summary>
        /// <param name="containerId">Id of container which is to be removed from collection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool Remove(Int32 containerId)
        {
            Container container = GetContainer(containerId);

            if (container == null)
                throw new ArgumentException("No container found for given container id");
            else
                return this.Remove(container);
        }

        /// <summary>
        /// Remove container object from ContainerCollection.This method removes container from across organization.
        /// </summary>
        /// <param name="containerName">Name of container which is to be removed from collection</param>
        /// <returns>true if item is successfully removed; otherwise, false. 
        /// This method also returns false if item was not found in the original collection.
        /// There would be possibility same containers are available in different organization.This method returns true if at least one container removed successfully.</returns>
        public bool Remove(String containerName)
        {
            Boolean atLeastOneRemoved = false;
            Boolean result = false;

            ContainerCollection containers = GetContainers(containerName);

            if (containers == null || (containers != null && containers.Count < 1))
                throw new ArgumentNullException("No container found for given container name");
            else
            {
                foreach (Container container in containers)
                {
                    atLeastOneRemoved = this.Remove(container);

                    if (atLeastOneRemoved)
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Remove container object from ContainerCollection.
        /// </summary>
        /// <param name="containerName">Name of container which is to be removed from collection</param>
        /// <param name="organizationName">Name of organization.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool Remove(String containerName, String organizationName)
        {
            Boolean result = false;

            Container container = GetContainer(containerName, organizationName);

            if (container == null)
                throw new ArgumentNullException("No container found for given Container Name and Organization Name");
            else
                result = this.Remove(container);

            return result;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(object obj)
        {
            if (obj is ContainerCollection)
            {
                ContainerCollection objectToBeCompared = obj as ContainerCollection;
                Int32 containerUnion = this._containers.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 containerIntersect = this._containers.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if (containerUnion != containerIntersect)
                    return false;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override Int32 GetHashCode()
        {
            Int32 hashCode = 0;
            foreach (Container container in this._containers)
            {
                hashCode += container.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// Populate containers from Xml
        /// </summary>
        /// <param name="valuesAsXml">
        /// Xml having containers detail
        /// </param>
        public void LoadContainerCollection(String valuesAsXml)
        {

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Container")
                        {
                            String containerXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(containerXml))
                            {
                                Container container = new Container(containerXml);
                                if (container != null)
                                {
                                    this.Add(container);
                                }
                            }
                        }
                        else
                        {
                            reader.Read();
                        }
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Find a container from current container collection with given container Name.
        /// </summary>
        /// <param name="containerName">Name of container which is to be searched in current collection</param>
        /// <returns>Container with given name</returns>
        public ContainerCollection GetContainers(String containerName)
        {
            containerName = containerName.ToLowerInvariant();

            ContainerCollection containers = new ContainerCollection((from container in this._containers
                                                                      where container.Name.ToLowerInvariant() == containerName
                                                                      select container).ToList<Container>());

            return containers;
        }

        /// <summary>
        /// Find a container from current container collection with given container and organization name.
        /// </summary>
        /// <param name="containerName">Name of container which is to be searched in current collection</param>
        /// <param name="organizationName">Name of container which is to be searched in current collection</param>
        /// <returns>Container with given name</returns>
        public Container GetContainer(String containerName, String organizationName)
        {
            if (_containers != null && _containers.Count > 0)
            {
                Func<Container, Boolean> compareMethod;

                if (String.IsNullOrWhiteSpace(organizationName))
                {
                    compareMethod = (container => String.Compare(container.Name, containerName, StringComparison.InvariantCultureIgnoreCase) == 0);
                }
                else
                {
                    compareMethod =  container => ((String.Compare(container.Name, containerName, StringComparison.InvariantCultureIgnoreCase) == 0 &&
                                     String.Compare(container.OrganizationShortName, organizationName, StringComparison.InvariantCultureIgnoreCase) == 0) ||
                                     (String.Compare(container.Name, containerName, StringComparison.InvariantCultureIgnoreCase) == 0));
                }

                Container currentContainer = null;
                Int32 containerCount = _containers.Count;

                for (Int32 index = 0; index < containerCount; index++)
                {
                    currentContainer = _containers[index];

                    if (compareMethod(currentContainer))
                    {
                        return currentContainer;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Clone container collection.
        /// </summary>
        /// <returns>cloned container collection object.</returns>
        public IContainerCollection Clone()
        {
            ContainerCollection clonedContainers = new ContainerCollection();

            if (this._containers != null && this._containers.Count > 0)
            {
                foreach (Container container in this._containers)
                {
                    IContainer clonedIContainer = container.Clone();
                    clonedContainers.Add(clonedIContainer as Container, true);
                }
            }

            return clonedContainers;
        }

        /// <summary>
        /// Returns the Container based on the containerId.
        /// </summary>
        /// <param name="containerId"></param>
        /// <returns></returns>
        public Container GetContainer(Int32 containerId)
        {
            Int32 containerCount = _containers.Count;
            Container container = null;

            for (Int32 index = 0; index < containerCount; index++)
            {
                container = _containers[index];
                if (container.Id == containerId)
                    return container;
            }
            return null;
        }

        /// <summary>
        /// Returns the Container based on the container cross reference identifier.
        /// </summary>
        /// <param name="containerCrossReferenceId"></param>
        /// <returns></returns>
        public Container GetContainerByCrossReferenceId(Int32 containerCrossReferenceId)
        {
            Int32 containerCount = _containers.Count;

            if (_containers.Count > 0)
            {
                foreach(Container container in _containers)
                {
                    if (container.CrossReferenceId == containerCrossReferenceId)
                    {
                        return container;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerId"></param>
        /// <returns></returns>
        public IContainer GetIContainer(Int32 containerId)
        {
            Container container = GetContainer(containerId);
            IContainer iContainer = null;
            if (container != null)
            {
                iContainer = MDMObjectFactory.GetIContainer(container);
            }
            return iContainer;
        }

        /// <summary>
        /// Remove dataModelObject based on reference id.
        /// </summary>
        /// <param name="referenceIds">Indicates collection of referenceId of an entity type which is to be fetched.</param>
        /// <returns>true if item was successfully removed from the collection;otherwise, false. This method also returns false if item is not found in the original</returns>
        public Boolean Remove(Collection<String> referenceIds)
        {
            Boolean result = true;

            ContainerCollection containers = GetContainers(referenceIds);

            if (containers != null && containers.Count > 0)
            {
                foreach (Container container in containers)
                {
                    result = result && this.Remove(container);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets unique hierarchy id list from the current container collection
        /// </summary>
        /// <returns>Collection of unique hierarchy ids in current collection</returns>
        public HashSet<Int32> GetHierarchyIds()
        {
            HashSet<Int32> hierarchyIds = null;

            if (this._containers != null && this._containers.Count > 0)
            {
                hierarchyIds = new HashSet<Int32>();

                foreach (Container container in this._containers)
                {
                    Int32 hierarchyId = container.HierarchyId;

                    if (hierarchyId > 0)
                    {
                        hierarchyIds.Add(hierarchyId);
                    }
                }
            }

            return hierarchyIds;
        }

        /// <summary>
        /// Gets container id list from the current container collection
        /// </summary>
        /// <returns>Collection of container ids in current collection</returns>
        public Collection<Int32> GetContainerIds()
        {
            Collection<Int32> containerIds = null;

            if (this._containers != null && this._containers.Count > 0)
            {
                containerIds = new Collection<Int32>();

                foreach (Container container in this._containers)
                {
                    Int32 containerId = container.Id;

                    if (containerId > 0)
                    {
                        containerIds.Add(containerId);
                    }
                }
            }

            return containerIds;
        }


        /// <summary>
        /// Get container based on given container name
        /// </summary>
        /// <param name="containerName">Indicates container short name</param>
        /// <returns>Returns container based on given container short name.</returns>
        public IContainer GetContainerByName(String containerName)
        {
            IContainer container = null;

            if (!String.IsNullOrWhiteSpace(containerName))
            {
                foreach (Container item in this._containers)
                {
                    if (String.Compare(item.Name, containerName, true) == 0)
                    {
                        container = item;
                    }
                }
            }

            return container;
        }

        /// <summary>
        /// Compare containerCollection with current collection.
        /// This method will compare container. If current collection has more containers than object to be compared, extra containers will be ignored.
        /// </summary>
        /// <param name="subsetContainerCollection">Indicates ContainerCollection to be compared with current collection</param>
        /// <returns>Returns True : Is both are same. False : otherwise</returns>
        public bool IsSuperSetOf(ContainerCollection subsetContainerCollection)
        {
            if (subsetContainerCollection != null)
            {
                foreach (Container container in subsetContainerCollection)
                {
                    IContainer iContainer = this.GetContainerByName(container.Name);

                    if (iContainer != null)
                    {
                        Container sourceContainer = (Container)iContainer;

                        if (!sourceContainer.IsSuperSetOf(container))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///  Gets the containers using the reference id.
        /// </summary>
        /// <param name="referenceIds">Indicates collection of referenceId of container which is to be fetched.</param>
        /// <returns>Returns filtered containers</returns>
        private ContainerCollection GetContainers(Collection<String> referenceIds)
        {
            ContainerCollection containers = new ContainerCollection();
            Int32 counter = 0;

            if (this._containers != null && this._containers.Count > 0 && referenceIds != null && referenceIds.Count > 0)
            {
                foreach (Container container in this._containers)
                {
                    if (referenceIds.Contains(container.ReferenceId))
                    {
                        containers.Add(container);
                        counter++;
                    }

                    if (referenceIds.Count.Equals(counter))
                        break;
                }
            }

            return containers;
        }

        #endregion

        #endregion

        #region IDataModelObjectCollection Members

        /// <summary>
        /// Splits current collection based on batchSize
        /// </summary>
        /// <param name="batchSize">Indicates no of objects to be put in each batch</param>
        /// <returns>IDataModelObjectCollection in Batch</returns>
        public Collection<IDataModelObjectCollection> Split(Int32 batchSize)
        {
            Collection<IDataModelObjectCollection> containersInBatch = new Collection<IDataModelObjectCollection>();

            if (this._containers != null)
            {
                containersInBatch = Utility.Split(this, batchSize);
            }

            return containersInBatch;
        }

        /// <summary>
        /// Adds item in current collection
        /// </summary>
        /// <param name="item">Indicates item to be added in collection</param>
        public void AddDataModelObject(IDataModelObject item)
        {
            this.Add(item as Container);
        }

        #endregion

        #region ICollection<Container> Members

        /// <summary>
        /// Add container object in collection
        /// </summary>
        /// <param name="container">container to add in collection</param>
        /// <param name="ignoreDuplicateCheck">Flag to indicate if we need to skip duplicate check in the current collection</param>
        /// <exception cref="Exception">Thrown if container and organization having same name is being added if ignoreDuplicateCheck is True</exception>
        public void Add(Container container, Boolean ignoreDuplicateCheck)
        {
            if (!ignoreDuplicateCheck)
            {
                this.Add((Container)container);
            }
            else
            {
                this._containers.Add(container);
            }
        }

        /// <summary>
        /// Add container object in collection
        /// Usage of this method should be avoided to ensure row's consistency for added containers.
        /// </summary>
        /// <param name="container">container to add in collection</param>
        /// <exception cref="Exception">Thrown if container and organization having same name is being added</exception>
        public void Add(Container container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            Container duplicateContainer = GetContainer(container.Name, container.OrganizationShortName);

            if (duplicateContainer != null)
            {
                throw new DuplicateObjectException("A Container with same Name already exist in table. Please give unique container name");
            }

            this._containers.Add(container);
        }

        /// <summary>
        /// Add container object in collection
        /// Usage of this method should be avoided to ensure row's consistency for added containers.
        /// Instead Table.AddColumn(Container newColumn) method should be used.
        /// </summary>
        /// <param name="container">container to add in collection</param>
        /// <exception cref="Exception">Thrown if container and organization  having same name is being added</exception>
        public void Add(IContainer container)
        {
            if (container != null)
            {
                this.Add((Container)container);
            }
        }

        /// <summary>
        /// Removes all containers from collection
        /// </summary>
        public void Clear()
        {
            this._containers.Clear();
        }

        /// <summary>
        /// Determines whether the ContainerCollection contains a specific container.
        /// </summary>
        /// <param name="item">The container object to locate in the ContainerCollection.</param>
        /// <returns>
        /// <para>true : If container found in containerCollection</para>
        /// <para>false : If container found not in container collection</para>
        /// </returns>
        public bool Contains(Container item)
        {
            return this._containers.Contains(item);
        }

        /// <summary>
        /// Determines whether the ContainerCollection contains a container with specific container name.
        /// </summary>
        /// <param name="containerName">The container object with given Container Name to locate in the ContainerCollection.</param>
        /// <returns>
        /// <para>true : If container found in containerCollection across organization.</para>
        /// <para>false : If container found not in containerCollection across organization.</para>
        /// </returns>
        public bool Contains(String containerName)
        {
            ContainerCollection containers = GetContainers(containerName);

            if (containers != null && (containers != null && containers.Count > 0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether the ContainerCollection contains a container with specific container name and organization name.
        /// </summary>
        /// <param name="containerName">The container object with given Container Name to locate in the ContainerCollection.</param>
        /// <param name="organizationName">The container object with given Organization Name to locate in the ContainerCollection.</param>
        /// <returns>
        /// <para>true : If container found in containerCollection with given container and organization name.</para>
        /// <para>false : If container found not in containerCollection with given container and organization name.</para>
        /// </returns>
        public bool Contains(String containerName, String organizationName)
        {
            Container container = GetContainer(containerName, organizationName);

            if (container != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Copies the elements of the ContainerCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from ContainerCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(Container[] array, int arrayIndex)
        {
            this._containers.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of containers in ContainerCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._containers.Count;
            }
        }

        /// <summary>
        /// Check if ContainerCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the ContainerCollection.
        /// </summary>
        /// <param name="item">The container object to remove from the ContainerCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original ContainerCollection</returns>
        public bool Remove(Container item)
        {
            return this._containers.Remove(item);
        }

        /// <summary>
        /// Get containers based on container type or qualifier name
        /// </summary>
        /// <param name="containerType">Indicates type of container</param>
        /// <param name="containerQualifierName">Indicates name of container qualifier</param>
        /// <returns>Returns collection of containers based on given container type and container qualifier name. If container type is null or unknown it will returns collection of 
        /// containers based on container qualifier name and if container qualifier name is blank it will returns collection of containers based on container type.</returns>
        public IContainerCollection GetByContainerTypeAndQualifier(ContainerType? containerType, String containerQualifierName) 
        {
            ContainerCollection containers = new ContainerCollection();

            if ((containerType != null && containerType != ContainerType.Unknown) && !String.IsNullOrWhiteSpace(containerQualifierName))
            {
                foreach (Container container in this._containers)
                {
                    if (container.ContainerType == containerType && String.Compare(container.ContainerQualifierName, containerQualifierName, true) == 0)
                    {
                        containers.Add(container);
                    }
                }
            }
            else if (containerType != null && containerType != ContainerType.Unknown)
            {
                foreach (Container container in this._containers)
                {
                    if (container.ContainerType == containerType)
                    {
                        containers.Add(container);
                    }
                }
            }
            else if (!String.IsNullOrWhiteSpace(containerQualifierName))
            {
                foreach (Container container in this._containers)
                {
                    if (String.Compare(container.ContainerQualifierName, containerQualifierName, true) == 0)
                    {
                        containers.Add(container);
                    }
                }
            }

            return containers;
        }

        /// <summary>
        /// Get child containers hierarchy based on given container identifier and flag denoting that load immediate child only or full hierarchy
        /// </summary>
        /// <param name="containerId">Indicates container identifier</param>
        /// <param name="loadRecursive">Indicates whether load immediate child only or full hierarchy</param>
        /// <returns>Returns collection of child containers hierarchy based on given container identifier and flag denoting that load immediate child only or full hierarchy.</returns>
        public IContainerCollection GetChildContainers(Int32 containerId, Boolean loadRecursive)
        {
            ContainerCollection childContainers = new ContainerCollection();

            GetChildContainersHierarchy(this._containers, childContainers, containerId, loadRecursive, false);

            return childContainers;
        }

        /// <summary>
        /// Get container hierarchy based on given container identifier
        /// </summary>
        /// <param name="containerId">Indicates container identifier</param>
        /// <returns>Returns collection of container hierarchy based on given container identifier.</returns>
        public IContainerCollection GetContainerHierarchy(Int32 containerId)
        {
            ContainerCollection childContainers = new ContainerCollection();

            GetChildContainersHierarchy(this._containers, childContainers, containerId, true, true);

            return childContainers;
        }

        /// <summary>
        /// Gets the container hierarchy by levels.
        /// </summary>
        /// <param name="containerId">The container identifier.</param>
        /// <returns>Dictionary containing the containers split by level of the container hierarchy</returns>
        public Dictionary<Int32, ContainerCollection> GetContainerHierarchyByLevels(Int32 containerId)
        {
            Dictionary<Int32, ContainerCollection> containerHierarchy = new Dictionary<Int32, ContainerCollection>();
            Int32 level = 1;

            ContainerCollection allRelatedContainers = (ContainerCollection)GetChildContainers(containerId, true);

            //Current entity container has to be the first in the level
            containerHierarchy.Add(level++, new ContainerCollection { GetContainer(containerId) });

            if (allRelatedContainers != null && allRelatedContainers.Count > 0)
            {
                ContainerCollection childContainers = (ContainerCollection)allRelatedContainers.GetChildContainers(containerId, false);
                if (childContainers != null && childContainers.Count > 0)
                {
                    PopulateChildContainerHierarchyRecursive(childContainers, containerHierarchy, ref level, allRelatedContainers);
                }
            }
            
            return containerHierarchy;
        }

        #endregion

        #region IEnumerable<Container> Members

        /// <summary>
        /// Returns an enumerator that iterates through a ContainerCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<Container> GetEnumerator()
        {
            return this._containers.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a ContainerCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._containers.GetEnumerator();
        }

        #endregion

        #region Private Method


        /// <summary>
        /// Populates the child container hierarchy recursively.
        /// </summary>
        /// <param name="childContainers">The child containers.</param>
        /// <param name="containerHierarchy">The container hierarchy.</param>
        /// <param name="level">The level.</param>
        /// <param name="allRelatedContainers">All related containers.</param>
        private void PopulateChildContainerHierarchyRecursive(ContainerCollection childContainers, Dictionary<int, ContainerCollection> containerHierarchy, ref Int32 level, ContainerCollection allRelatedContainers)
        {
            if (childContainers.Count > 0)
            {
                containerHierarchy.Add(level++, childContainers);

                ContainerCollection nextLevelContainers = new ContainerCollection();
                foreach (Container childContainer in childContainers)
                {
                    ContainerCollection nextLevelChildContainers = (ContainerCollection)allRelatedContainers.GetChildContainers(childContainer.Id, false);
                    if (nextLevelChildContainers != null && nextLevelChildContainers.Count > 0)
                    {
                        nextLevelContainers.AddRange(nextLevelChildContainers, false);
                    }
                }

                if (nextLevelContainers.Count > 0)
                {
                    PopulateChildContainerHierarchyRecursive(nextLevelContainers, containerHierarchy, ref level, allRelatedContainers);
                }
            }
        }


        /// <summary>
        /// Get hierarchy of child containers
        /// </summary>
        /// <param name="containers">Indicates collection of containers</param>
        /// <param name="containerId">Indicates container identifier</param>
        /// <param name="childContainer">Indicates collection of child containers</param>
        /// <param name="loadRecursive">Indicates whether load immediate child containers only or full container hierarchy </param>
        /// <param name="includeSelf">Indicates whether to load container it self of requested container identifier or not</param>
        private void GetChildContainersHierarchy(Collection<Container> containers, ContainerCollection childContainer, Int32 containerId, Boolean loadRecursive, Boolean includeSelf)
        {
            ContainerCollection containerCollection = new ContainerCollection();

            foreach (Container container in containers)
            {
                if (container.Id == containerId && includeSelf)
                {
                    childContainer.Add(container);
                }

                if (container.ParentContainerId == containerId)
                {
                    containerCollection.Add(container);
                    childContainer.Add(container);
                }
            }

            if (loadRecursive)
            {
                if (containerCollection.Count > 0)
                {
                    foreach (Container container in containerCollection)
                    {
                        GetChildContainersHierarchy(containers, childContainer, container.Id, loadRecursive, false);
                    }
                }
            }
        }

        #endregion Private Method

        #region ToXml Methods

        /// <summary>
        /// Get Xml representation of ContainerCollection object
        /// </summary>
        /// <returns>Xml string representing the ContainerCollection</returns>
        public String ToXml()
        {
            String xml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("Containers");

            if (this._containers != null)
            {
                foreach (Container container in this._containers)
                {
                    xmlWriter.WriteRaw(container.ToXml());
                }
            }

            //Container node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Get Xml representation of ContainerCollection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String xml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("Containers");

            if (this._containers != null)
            {
                foreach (Container container in this._containers)
                {
                    xmlWriter.WriteRaw(container.ToXml(serialization));
                }
            }

            //Container node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        #endregion ToXml methods
    }
}