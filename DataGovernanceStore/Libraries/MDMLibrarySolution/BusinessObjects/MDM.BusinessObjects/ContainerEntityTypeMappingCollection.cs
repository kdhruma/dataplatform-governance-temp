using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the mapping collection object for Container EntityType mapping
    /// </summary>
    [DataContract]
    public class ContainerEntityTypeMappingCollection : ICollection<ContainerEntityTypeMapping>, IEnumerable<ContainerEntityTypeMapping>, IContainerEntityTypeMappingCollection, IDataModelObjectCollection
    {
        #region Fields

        [DataMember]
        private Collection<ContainerEntityTypeMapping> _containerEntityTypeMappings = new Collection<ContainerEntityTypeMapping>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter-less constructor
        /// </summary>
        public ContainerEntityTypeMappingCollection() : base() { }

        /// <summary>
        /// Initialize ContainerEntityTypeMappingCollection from IList
        /// </summary>
        /// <param name="ContainerEntityTypeMappingList">IList of ContainerEntityTypeMappings</param>
        public ContainerEntityTypeMappingCollection(IList<ContainerEntityTypeMapping> ContainerEntityTypeMappingList)
        {
            this._containerEntityTypeMappings = new Collection<ContainerEntityTypeMapping>(ContainerEntityTypeMappingList);
        }

        /// <summary>
        /// Initializes a new instance of the ContainerEntityTypeMappingCollection class with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        public ContainerEntityTypeMappingCollection(String valuesAsXml)
        {
            LoadContainerEntityTypeMappingCollection(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting a ObjectType for an dataModelObject object
        /// </summary>
        public ObjectType DataModelObjectType
        {
            get
            {
                return MDM.Core.ObjectType.ContainerEntityTypeMapping;
            }
        }

        #endregion

        #region Method

        #region Public Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(object obj)
        {
            if (obj is ContainerEntityTypeMappingCollection)
            {
                ContainerEntityTypeMappingCollection objectToBeCompared = obj as ContainerEntityTypeMappingCollection;
                Int32 containerEntityTypeMappingsUnion = this._containerEntityTypeMappings.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 containerEntityTypeMappingsIntersect = this._containerEntityTypeMappings.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if (containerEntityTypeMappingsUnion != containerEntityTypeMappingsIntersect)
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
            foreach (ContainerEntityTypeMapping containerEntityTypeMapping in this._containerEntityTypeMappings)
            {
                hashCode += containerEntityTypeMapping.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// Gets the ContainerEntityTypeMapping for given Id.
        /// </summary>
        /// <param name="Id">Id to be searched in the collection</param>
        /// <returns>ContainerEntityTypeMapping having given Id</returns>
        public ContainerEntityTypeMapping Get(Int32 Id)
        {
            if (this._containerEntityTypeMappings != null && this._containerEntityTypeMappings.Count > 0)
            {
                foreach (ContainerEntityTypeMapping containerEntityTypeMapping in this._containerEntityTypeMappings)
                {
                    if (containerEntityTypeMapping.Id.Equals(Id))
                    {
                        return containerEntityTypeMapping;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the ContainerEntityTypeMapping for given ContainerName and EntityTypeName.
        /// </summary>
        /// <param name="organizationName">OrganizationName to be searched in the collection</param>
        /// <param name="containerName">ContainerName to be searched in the collection</param>
        /// <param name="entityTypeName">EntityTypeName to be searched in the collection</param>
        /// <returns>ContainerEntityTypeMapping having given ContainerName and EntityTypeName</returns>
        public ContainerEntityTypeMapping Get(String organizationName, String containerName, String entityTypeName)
        {
            if (this._containerEntityTypeMappings != null && this._containerEntityTypeMappings.Count > 0)
            {
                organizationName = organizationName.ToLowerInvariant();
                containerName = containerName.ToLowerInvariant();
                entityTypeName = entityTypeName.ToLowerInvariant();

                foreach (ContainerEntityTypeMapping containerEntityTypeMapping in this._containerEntityTypeMappings)
                {
                    if (containerEntityTypeMapping.OrganizationName.ToLowerInvariant().Equals(organizationName) &&
                        containerEntityTypeMapping.ContainerName.ToLowerInvariant().Equals(containerName) && 
                        containerEntityTypeMapping.EntityTypeName.ToLowerInvariant().Equals(entityTypeName))
                    {
                        return containerEntityTypeMapping;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the container entity type mappings for given container id
        /// </summary>
        /// <param name="containerId">Container id to be searched in the collection</param>
        /// <returns>Returns collection of container entity type mapping having given container id</returns>
        public ContainerEntityTypeMappingCollection GetByContainerId(Int32 containerId)
        {
            ContainerEntityTypeMappingCollection filteredContainerEntityTypeMappings = null;

            if (this._containerEntityTypeMappings != null && this._containerEntityTypeMappings.Count > 0)
            {
                filteredContainerEntityTypeMappings = new ContainerEntityTypeMappingCollection();

                foreach (ContainerEntityTypeMapping containerEntityTypeMapping in this._containerEntityTypeMappings)
                {
                    if (containerEntityTypeMapping.ContainerId == containerId)
                    {
                        filteredContainerEntityTypeMappings.Add(containerEntityTypeMapping);
                    }
                }
            }

            return filteredContainerEntityTypeMappings;
        }

        /// <summary>
        /// Compare ContainerEntityTypeMappingCollection. 
        /// </summary>
        /// <param name="subsetContainerEntityTypeMappingCollection">Expected ContainerEntityTypeMappingCollection.</param>
        /// <param name="compareIds">Compare Ids or not.</param>
        /// <returns>If actual ContainerEntityTypeMappingCollection is match then true else false</returns>
        public Boolean IsSuperSetOf(ContainerEntityTypeMappingCollection subsetContainerEntityTypeMappingCollection, Boolean compareIds = false)
        {
            if (subsetContainerEntityTypeMappingCollection != null && subsetContainerEntityTypeMappingCollection.Count > 0)
            {
                foreach (ContainerEntityTypeMapping subsetContainerEntityTypeMapping in subsetContainerEntityTypeMappingCollection)
                {
                    ContainerEntityTypeMapping containerEntityTypeMapping = this.Get(subsetContainerEntityTypeMapping.OrganizationName, subsetContainerEntityTypeMapping.ContainerName, subsetContainerEntityTypeMapping.EntityTypeName);

                    if (containerEntityTypeMapping == null)
                    {
                        return false;
                    }
                    else if (!containerEntityTypeMapping.IsSuperSetOf(subsetContainerEntityTypeMapping, compareIds))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        #region ICollection<ContainerEntityTypeMapping> Members

        /// <summary>
        /// Add ContainerEntityTypeMapping object in collection
        /// </summary>
        /// <param name="item">ContainerEntityTypeMapping to add in collection</param>
        public void Add(ContainerEntityTypeMapping item)
        {
            this._containerEntityTypeMappings.Add(item);
        }

        /// <summary>
        /// Add ContainerEntityTypeMapping object in collection
        /// </summary>
        /// <param name="items">Indicates collection of container entity type mapping to add</param>
        public void AddRange(ContainerEntityTypeMappingCollection items)
        {
            if (items != null && items.Count > 0)
            {
                foreach (ContainerEntityTypeMapping item in items)
                {
                    this._containerEntityTypeMappings.Add(item);
                }
            }
        }

        /// <summary>
        /// Add ContainerEntityTypeMapping in collection
        /// </summary>
        /// <param name="iContainerEntityTypeMapping">ContainerEntityTypeMapping to add in collection</param>
        public void Add(IContainerEntityTypeMapping iContainerEntityTypeMapping)
        {
            if (iContainerEntityTypeMapping != null)
            {
                this.Add((ContainerEntityTypeMapping)iContainerEntityTypeMapping);
            }
        }

        /// <summary>
        /// Removes all ContainerEntityTypeMappings from collection
        /// </summary>
        public void Clear()
        {
            this._containerEntityTypeMappings.Clear();
        }

        /// <summary>
        /// Determines whether the ContainerEntityTypeMappingCollection contains a specific ContainerEntityTypeMapping.
        /// </summary>
        /// <param name="item">The ContainerEntityTypeMapping object to locate in the ContainerEntityTypeMappingCollection.</param>
        /// <returns>
        /// <para>true : If ContainerEntityTypeMapping found in mappingCollection</para>
        /// <para>false : If ContainerEntityTypeMapping found not in mappingCollection</para>
        /// </returns>
        public bool Contains(ContainerEntityTypeMapping item)
        {
            return this._containerEntityTypeMappings.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the ContainerEntityTypeMappingCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from ContainerEntityTypeMappingCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(ContainerEntityTypeMapping[] array, int arrayIndex)
        {
            this._containerEntityTypeMappings.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of ContainerEntityTypeMappings in ContainerEntityTypeMappingCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._containerEntityTypeMappings.Count;
            }
        }

        /// <summary>
        /// Check if ContainerEntityTypeMappingCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the ContainerEntityTypeMappingCollection.
        /// </summary>
        /// <param name="item">The ContainerEntityTypeMapping object to remove from the ContainerEntityTypeMappingCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original ContainerEntityTypeMappingCollection</returns>
        public bool Remove(ContainerEntityTypeMapping item)
        {
            return this._containerEntityTypeMappings.Remove(item);
        }

        /// <summary>
        /// Get ContainerEntityTypeMapping for given container and entity type identifiers
        /// </summary>
        /// <param name="containerId">Indicates container identifier</param>
        /// <param name="entityTypeId">Indicates entity type identifier</param>
        /// <returns>Returns ContainerEntityTypeMapping for given container and entity type identifiers.</returns>
        public IContainerEntityTypeMapping GetByContainerAndEntityType(Int32 containerId, Int32 entityTypeId)
        {
            IContainerEntityTypeMapping containerEntityTypeMapping = null;
            
            if (this != null && this.Count() > 0)
            {
                foreach (ContainerEntityTypeMapping item in this)
                {
                    if (item.ContainerId == containerId && item.EntityTypeId == entityTypeId)
                    {
                        containerEntityTypeMapping = item;
                        break;
                    }
                }
            }

            return containerEntityTypeMapping;
        }

        #endregion

        #region IEnumerable<ContainerEntityTypeMapping> Members

        /// <summary>
        /// Returns an enumerator that iterates through a ContainerEntityTypeMappingCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<ContainerEntityTypeMapping> GetEnumerator()
        {
            return this._containerEntityTypeMappings.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a ContainerEntityTypeMappingCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._containerEntityTypeMappings.GetEnumerator();
        }

        #endregion

        #region IContainerEntityTypeMappingCollection Members

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of ContainerEntityTypeMappingCollection object
        /// </summary>
        /// <returns>Xml string representing the ContainerEntityTypeMappingCollection</returns>
        public String ToXml()
        {
            String containerEntityTypeMappingsXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (ContainerEntityTypeMapping containerEntityTypeMapping in this._containerEntityTypeMappings)
            {
                builder.Append(containerEntityTypeMapping.ToXml());
            }

            containerEntityTypeMappingsXml = String.Format("<ContainerEntityTypes>{0}</ContainerEntityTypes>", builder.ToString());

            return containerEntityTypeMappingsXml;
        }

        #endregion ToXml methods

        #endregion IContainerEntityTypeMappingCollection Members

        #region IDataModelObjectCollection
        
        /// <summary>
        /// Remove dataModelObject based on reference id.
        /// </summary>
        /// <param name="referenceIds">Indicates collection of referenceId of an entity type which is to be fetched.</param>
        /// <returns>true if item was successfully removed from the collection;otherwise, false. This method also returns false if item is not found in the original</returns>
        public bool Remove(Collection<string> referenceIds)
        {
            Boolean result = true;

            ContainerEntityTypeMappingCollection containerEntityTypeMappings = Get(referenceIds);

            if (containerEntityTypeMappings != null && containerEntityTypeMappings.Count > 0)
            {
                foreach (ContainerEntityTypeMapping containerEntityTypeMapping in containerEntityTypeMappings)
                {
                    result = result && this.Remove(containerEntityTypeMapping);
                }
            }

            return result;
        }

        /// <summary>
        /// Splits current collection based on batchSize
        /// </summary>
        /// <param name="batchSize">Indicates no of objects to be put in each batch</param>
        /// <returns>IDataModelObjectCollection in Batch</returns>
        public Collection<IDataModelObjectCollection> Split(Int32 batchSize)
        {
            Collection<IDataModelObjectCollection> containerEntityTypeMappingsInBatch = null;

            if (this._containerEntityTypeMappings != null)
            {
                containerEntityTypeMappingsInBatch = Utility.Split(this, batchSize);
            }

            return containerEntityTypeMappingsInBatch;
        }

        /// <summary>
        /// Adds item in current collection
        /// </summary>
        /// <param name="item">Indicates item to be added in collection</param>
        public void AddDataModelObject(IDataModelObject item)
        {
            this.Add(item as ContainerEntityTypeMapping);
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the ContainerEntityTypeMapping for given referenceIds.
        /// </summary>
        /// <param name="referenceIds">referenceIds to be searched in the collection</param>
        /// <returns>ContainerEntityTypeMapping having given referenceIds</returns>
        private ContainerEntityTypeMappingCollection Get(Collection<String> referenceIds)
        {
            ContainerEntityTypeMappingCollection containerEntityTypeMappings = new ContainerEntityTypeMappingCollection();
            Int32 counter = 0;

            if (this._containerEntityTypeMappings != null && this._containerEntityTypeMappings.Count > 0 && referenceIds != null && referenceIds.Count > 0)
            {
                foreach (ContainerEntityTypeMapping containerEntityTypeMapping in this._containerEntityTypeMappings)
                {
                    if (referenceIds.Contains(containerEntityTypeMapping.ReferenceId))
                    {
                        containerEntityTypeMappings.Add(containerEntityTypeMapping);
                        counter++;
                    }

                    if (referenceIds.Count.Equals(counter))
                        break;
                }
            }

            return containerEntityTypeMappings;
        }

        /// <summary>
        /// Load current ContainerEntityTypeMappingCollection from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for current ContainerEntityTypeMappingCollection
        /// </param>
        private void LoadContainerEntityTypeMappingCollection(String valuesAsXml)
        {
            /*
             * Sample XML:
               <ContainerEntityTypes>
                    <ContainerEntityType Id="2" ContainerId="2" OrgId = "1" EntityTypeId = "16" Action="Create" />
               </ContainerEntityTypes>
             */
            XmlTextReader reader = null;

            if (!String.IsNullOrEmpty(valuesAsXml))
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "ContainerEntityType")
                    {
                        String containerEntityTypeMappingXml = reader.ReadOuterXml();
                        if (!String.IsNullOrEmpty(containerEntityTypeMappingXml))
                        {
                            ContainerEntityTypeMapping containerEntityTypeMapping = new ContainerEntityTypeMapping(containerEntityTypeMappingXml);
                            if (containerEntityTypeMapping != null)
                            {
                                this.Add(containerEntityTypeMapping);
                            }
                        }
                    }
                    else
                    {
                        reader.Read();
                    }
                }

                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        #endregion

        #endregion

    }
}
