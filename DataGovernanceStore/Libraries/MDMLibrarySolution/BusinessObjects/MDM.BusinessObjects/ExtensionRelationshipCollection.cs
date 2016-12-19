using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the collection of extension relationships
    /// </summary>
    [DataContract]
    [ProtoContract]
    [KnownType(typeof(RelationshipBaseCollection))]
    public class ExtensionRelationshipCollection : ICollection<ExtensionRelationship>, IEnumerable<ExtensionRelationship>, IExtensionRelationshipCollection
    {
        #region Fields

        /// <summary>
        /// Field denoting collection of extension relationships
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        private Collection<ExtensionRelationship> _extensionRelationships = new Collection<ExtensionRelationship>();

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        private ObjectAction _action = ObjectAction.Unknown;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(3)]
        private EntityChangeContext _changeContext = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Find extension relationship from collection based on related entity id
        /// </summary>
        /// <param name="relatedEntityId">Id of the related entity using which extension relationship is to be searched from collection</param>
        /// <returns>Extension Relationship object</returns>
        public ExtensionRelationship this[Int64 relatedEntityId]
        {
            get
            {
                ExtensionRelationship extensionRelationship = GetExtensionRelationship(relatedEntityId);

                if (extensionRelationship == null)
                    throw new ArgumentException("No extension relationship found for given related entity id");
                else
                    return extensionRelationship;
            }
        }

        /// <summary>
        /// Initializes a new instance of the extension relationship collection class
        /// </summary>
        public ExtensionRelationshipCollection()
        {

        }

        /// <summary>
        /// Initialize ExtensionRelationshipCollection from IList of value
        /// </summary>
        /// <param name="extensionRelationshipList">List of ExtensionRelationship object</param>
        public ExtensionRelationshipCollection(IList<ExtensionRelationship> extensionRelationshipList)
        {
            this._extensionRelationships = new Collection<ExtensionRelationship>(extensionRelationshipList);
        }

        /// <summary>
        /// Initializes a new instance of the extension relationship collection class with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        public ExtensionRelationshipCollection(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            /*
             * Sample XML:
             *  <Relationships>
                    <Relationship Id="124" ObjectType="ExtensionRelationship" Type="Entity" RelatedEntityId="[GrandChild1]" Direction="Down" Path="[Current]-[Child]-[GrandChild1]" Action="Read">
                        <RelationshipAttributes />
                        <Relationships />
                    </Relationship>
                    <Relationship Id="125" ObjectType="ExtensionRelationship" Type="Entity" RelatedEntityId="[GrandChild2]" Direction="Down" Path="[Current]-[Child]-[GrandChild2]" Action="Read">
                        <RelationshipAttributes />
                        <Relationships />
                    </Relationship>
                </Relationships>
             */
            XmlTextReader reader = null;

            if (!String.IsNullOrEmpty(valuesAsXml))
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Relationship")
                    {
                        String relationshipXml = reader.ReadOuterXml();

                        if (!String.IsNullOrWhiteSpace(relationshipXml))
                        {
                            ExtensionRelationship extensionRelationship = new ExtensionRelationship(relationshipXml, objectSerialization);

                            if (extensionRelationship != null)
                                this._extensionRelationships.Add(extensionRelationship);
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

        /// <summary>
        /// Loads ExtensionRelationshipCollection object from xml
        /// </summary>
        /// <param name="reader">Indicates an xml reader used to read xml format data</param>
        internal void LoadExtensionRelationshipCollectionFromXml(XmlTextReader reader)
        {
            if (reader != null)
            {
                if (reader.IsEmptyElement)
                {
                    return;
                }

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Relationship")
                    {
                        if (reader.HasAttributes)
                        {
                            ExtensionRelationship relationship = new ExtensionRelationship();

                            relationship.LoadExtensionRelationshipFromXml(reader);

                            this.Add(relationship);
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Extensions")
                    {
                        return;
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("reader", "XmlTextReader is null. Can not read ExtensionRelationshipCollection object.");
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public ObjectAction Action
        {
            get { return _action; }
            set { _action = value; }
        }

        /// <summary>
        /// This property defines union of all changes made to related entities
        /// </summary>
        public EntityChangeContext ChangeContext
        {
            get { return _changeContext; }
            set { _changeContext = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Check if ExtensionRelationshipCollection contains relationship with given related entity id
        /// </summary>
        /// <param name="relatedEntityId">Id of the related entity using which extension relationship is to be searched from collection</param>
        /// <returns>
        /// <para>true : If relationship found in ExtensionRelationshipCollection</para>
        /// <para>false : If relationship found not in ExtensionRelationshipCollection</para>
        /// </returns>
        public Boolean Contains(Int64 relatedEntityId)
        {
            if (GetExtensionRelationship(relatedEntityId) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Check if ExtensionRelationshipCollection contains relationship with given category id and container Id.
        /// </summary>
        /// <param name="categoryId">Id of category using which extension relationship is to be searched from collection</param>
        /// <param name="containerId">Id of the container using which extension relationship is to be searched from collection</param>
        /// <para>true : If relationship found in ExtensionRelationshipCollection</para>
        /// <para>false : If relationship found not in ExtensionRelationshipCollection</para>
        public Boolean Contains(Int64 categoryId, Int32 containerId)
        {
            if (GetExtensionRelationship(categoryId, containerId) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Remove extension relationship object from ExtensionRelationshipCollection
        /// </summary>
        /// <param name="relatedEntityId">Id of the related entity using which ExtensionRelationship is to be removed from collection</param>
        /// <returns>true if extension relationship is successfully removed; otherwise, false. This method also returns false if extension relationship was not found in the original collection</returns>
        public Boolean Remove(Int64 relatedEntityId)
        {
            ExtensionRelationship extensionRelationship = GetExtensionRelationship(relatedEntityId);

            if (extensionRelationship == null)
                throw new ArgumentException("No extension relationship found for given related entity id");
            else
                return this.Remove(extensionRelationship);
        }

        /// <summary>
        /// Get Xml representation of Extension Relationship Collection
        /// </summary>
        /// <returns>Xml representation of Extension Relationship Collection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            returnXml = "<Extensions>";

            if (this._extensionRelationships != null && this._extensionRelationships.Count > 0)
            {
                foreach (ExtensionRelationship extensionRelationship in this._extensionRelationships)
                {
                    returnXml = String.Concat(returnXml, extensionRelationship.ToXml());
                }
            }

            returnXml = String.Concat(returnXml, "</Extensions>");

            return returnXml;
        }

        /// <summary>
        /// Get Xml representation of Extension Relationship Collection based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Extension Relationship Collection</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String returnXml = String.Empty;

            returnXml = "<Extensions>";

            if (this._extensionRelationships != null && this._extensionRelationships.Count > 0)
            {
                foreach (ExtensionRelationship extensionRelationship in this._extensionRelationships)
                {
                    returnXml = String.Concat(returnXml, extensionRelationship.ToXml(objectSerialization));
                }
            }

            returnXml = String.Concat(returnXml, "</Extensions>");

            return returnXml;
        }

        /// <summary>
        /// Converts ExtensionRelationshipCollection object into xml format.
        /// In future, this method can be enhanced to accept EntityConversionContext object
        /// </summary>
        /// <param name="xmlWriter">Indicates a writer to generate xml reperesentation of ExtensionRelationshipCollection object</param>
        internal void ConvertExtensionRelationshipCollectionToXml(XmlTextWriter xmlWriter)
        {
            if (xmlWriter != null)
            {
                // Extenstions node start
                xmlWriter.WriteStartElement("Extensions");

                if (this._extensionRelationships != null && this._extensionRelationships.Count > 0)
                {
                    foreach (ExtensionRelationship extensionRelationship in this._extensionRelationships)
                    {
                        extensionRelationship.ConvertExtensionRelationshipToXml(xmlWriter);
                    }
                }

                // Extenstions node end
                xmlWriter.WriteEndElement();
            }
            else
            {
                throw new ArgumentNullException("xmlWriter", "XmlTextWriter is null. Can not write ExtensionRelationshipCollection object.");
            }
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj is ExtensionRelationshipCollection)
            {
                ExtensionRelationshipCollection objectToBeCompared = obj as ExtensionRelationshipCollection;

                Int32 extensionRelationshipsUnion = this._extensionRelationships.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 extensionRelationshipsIntersect = this._extensionRelationships.ToList().Intersect(objectToBeCompared.ToList()).Count();

                if (extensionRelationshipsUnion != extensionRelationshipsIntersect)
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

            foreach (ExtensionRelationship extensionRelationship in this._extensionRelationships)
            {
                hashCode += extensionRelationship.GetHashCode();
            }

            return hashCode;
        }

        /// <summary>
        /// Checks whether current object or its child objects have been changed i.e any object having Action flag as Create, Update or Delete
        /// </summary>
        /// <returns>Return true if object is changed else false</returns>
        public Boolean HasObjectChanged()
        {
            Boolean hasObjectUpdated = false;

            if (this._extensionRelationships != null && this._extensionRelationships.Count > 0)
            {
                IEnumerable<ExtensionRelationship> updatedExtensionRelationships = this._extensionRelationships.Where(h => h.HasObjectChanged());

                if (updatedExtensionRelationships != null && updatedExtensionRelationships.Count() > 0)
                    hasObjectUpdated = true;
            }

            return hasObjectUpdated;
        }

        /// <summary>
        /// Denormalize extension relationships tree into flat structure
        /// </summary>
        /// <returns>Denormalized Extension Relationships</returns>
        public IExtensionRelationshipCollection Denormalize()
        {
            ExtensionRelationshipCollection denormalizedExtensions = new ExtensionRelationshipCollection();

            if (this._extensionRelationships != null && this._extensionRelationships.Count > 0)
            {
                foreach (ExtensionRelationship extensionRelationship in this._extensionRelationships)
                {
                    denormalizedExtensions.Add(extensionRelationship);

                    if (extensionRelationship.RelationshipCollection != null && extensionRelationship.RelationshipCollection.Count > 0)
                    {
                        IExtensionRelationshipCollection childDenormalizedExtensions = extensionRelationship.RelationshipCollection.Denormalize();
                        denormalizedExtensions = new ExtensionRelationshipCollection(denormalizedExtensions.Concat(childDenormalizedExtensions).ToList());
                    }
                }
            }

            return denormalizedExtensions;
        }

        /// <summary>
        /// Find ExtensionRelationship from Collection by ContainerId.
        /// </summary>
        /// <param name="containerId">>This parameter is specifying container Id.</param>
        /// <returns>Extension Relationship</returns>
        public IExtensionRelationship FindByContainerId(Int32 containerId)
        {
            IExtensionRelationship extensionRelationship = null;

            if (this._extensionRelationships != null && this._extensionRelationships.Count > 0)
            {
                foreach (ExtensionRelationship extRel in this._extensionRelationships)
                {
                    if (extRel.ContainerId == containerId)
                        extensionRelationship = extRel;
                    else
                    {
                        if (extRel.RelationshipCollection != null && extRel.RelationshipCollection.Count > 0)
                        {
                            extensionRelationship = extRel.RelationshipCollection.FindByContainerId(containerId);
                        }
                    }

                    if (extensionRelationship != null)
                        break;
                }
            }

            return extensionRelationship;
        }

        /// <summary>
        /// Find ExtensionRelationship from Collection by Related Entity Id
        /// </summary>
        /// <param name="relatedEntityId">Id of Related Entity</param>
        /// <returns>Returns ExtensionRelationship if match found or null if not.</returns>
        public IExtensionRelationship FindByRelatedEntityId(Int64 relatedEntityId)
        {
            if (this._extensionRelationships != null && this._extensionRelationships.Count > 0)
            {
                foreach (ExtensionRelationship relationship in this._extensionRelationships)
                {
                    if (relationship.RelatedEntityId == relatedEntityId)
                    {
                        return relationship;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Find ExtensionRelationship from Collection by ToExternalId ToContainerId and ToCategoryId
        /// </summary>
        /// <param name="toExternalId">To ExternalId</param>
        /// <param name="toContainerId">To ContainerId</param>
        /// <param name="toCategoryId">To CategoryId</param>
        /// <returns>Returns ExtensionRelationship if match found or null if not.</returns>
        public IExtensionRelationship FindByToExternalIdContainerIdCategoryId(String toExternalId, Int32 toContainerId, Int64 toCategoryId)
        {
            if (this._extensionRelationships != null && this._extensionRelationships.Count > 0)
            {
                foreach (ExtensionRelationship relationship in this._extensionRelationships)
                {
                    if (String.Compare(toExternalId, relationship.ExternalId, true) == 0 && toContainerId == relationship.ContainerId && toCategoryId == relationship.CategoryId)
                    {
                        return relationship;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Find ExtensionRelationship from Collection by ToExternalId ContainerName and CategoryPath
        /// </summary>
        /// <param name="toExternalId">To ExternalId</param>
        /// <param name="toContainerName"></param>
        /// <param name="toCategoryPath"></param>
        /// <returns>Returns ExtensionRelationship if match found or null if not.</returns>
        public IExtensionRelationship FindByToExternalIdContainerNameCategoryPath(String toExternalId, String toContainerName, String toCategoryPath)
        {
            if (this._extensionRelationships != null && this._extensionRelationships.Count > 0)
            {
                foreach (ExtensionRelationship relationship in this._extensionRelationships)
                {
                    if (String.Compare(toExternalId, relationship.ExternalId, true) == 0 &&
                        String.Compare(toContainerName, relationship.ContainerName, true) == 0 &&
                        String.Compare(toCategoryPath, relationship.CategoryPath, true) == 0)
                    {
                        return relationship;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Filters extension relationships based on direction
        /// </summary>
        /// <param name="direction">Direction by which relationships needs to be filtered.
        /// <para>
        /// Applicable values are 'Up' and 'Down'
        /// </para>
        /// </param>
        /// <returns>Filtered Extension Relationships</returns>
        public IExtensionRelationshipCollection FilterBy(RelationshipDirection direction)
        {
            ExtensionRelationshipCollection extensionRelationships = new ExtensionRelationshipCollection();

            if (this._extensionRelationships != null && this._extensionRelationships.Count > 0)
            {
                extensionRelationships.Action = this.Action;
                extensionRelationships.ChangeContext = this.ChangeContext;

                foreach (ExtensionRelationship extensionRelationship in this._extensionRelationships)
                {
                    if (extensionRelationship.Direction == direction)
                    {
                        extensionRelationships.Add(extensionRelationship);

                        //Apply direction filter for all the levels..
                        if (extensionRelationship.RelationshipCollection != null && extensionRelationship.RelationshipCollection.Count > 0)
                            extensionRelationship.RelationshipCollection = (ExtensionRelationshipCollection)extensionRelationship.RelationshipCollection.FilterBy(direction);
                    }
                }
            }

            return extensionRelationships;
        }

        /// <summary>
        /// Create a new extension relationship collection object.
        /// </summary>
        /// <returns>New extension relationship collection instance.</returns>
        public ExtensionRelationshipCollection Clone()
        {
            ExtensionRelationshipCollection clonedExtensionRelationships = new ExtensionRelationshipCollection();

            if (this._extensionRelationships != null && this._extensionRelationships.Count > 0)
            {
                foreach (ExtensionRelationship childExtensionRelationship in this._extensionRelationships)
                {
                    ExtensionRelationship clonedChildExtensionRelationship = childExtensionRelationship.Clone();
                    clonedExtensionRelationships.Add(clonedChildExtensionRelationship);
                }
            }

            return clonedExtensionRelationships;
        }

        /// <summary>
        /// compare 2 extensionRelationship objects
        /// </summary>
        /// <param name="subsetHierarchyRelationshipCollection"></param>
        /// <param name="operationResult">the parentOperationresult - all errors here will be passed up to EntityOperationResult level</param>
        /// <param name="compareIds"></param>
        public Boolean GetSuperSetOperationResult(ExtensionRelationshipCollection subsetHierarchyRelationshipCollection, OperationResult operationResult, Boolean compareIds = false)
        {
            #region compare xtensionRelationshipCollection

            if (subsetHierarchyRelationshipCollection != null)
            {
                if (this._extensionRelationships != null && this._extensionRelationships.Count > 0)
                {
                    foreach (var extensionRelationship in subsetHierarchyRelationshipCollection)
                    {
                        var sourceExtensionRelationship = this.FirstOrDefault(a => a.Name == extensionRelationship.Name);
                        if (sourceExtensionRelationship != null)
                        {
                            sourceExtensionRelationship.GetSuperSetOperationResult(extensionRelationship, operationResult, compareIds);
                        }
                    }
                }
            }
            else
            {
                operationResult.AddOperationResult("-1", "subsetHierarchyRelationshipCollection is null - nothing to compare to", OperationResultType.Information);
            }

            #endregion compare xtensionRelationshipCollection

            #region refresh and return

            operationResult.RefreshOperationResultStatus();
            if (operationResult.OperationResultStatus == OperationResultStatusEnum.Successful)
            {
                return true;
            }
            else
            {
                return false;
            }

            #endregion refresh and return

        }

        #endregion

        #region Private Methods

        private ExtensionRelationship GetExtensionRelationship(Int64 relatedEntityId)
        {
            var filteredExtensionRelationships = this._extensionRelationships.Where(e => e.RelatedEntityId == relatedEntityId);

            if (filteredExtensionRelationships.Any())
                return filteredExtensionRelationships.First();
            else
                return null;
        }

        private ExtensionRelationship GetExtensionRelationship(Int64 categoryId, Int32 containerId)
        {
            var filteredExtensionRelationships = this._extensionRelationships.Where(e => e.CategoryId == categoryId && e.ContainerId == containerId);

            if (filteredExtensionRelationships.Any())
                return filteredExtensionRelationships.First();
            else
                return null;
        }

        #endregion

        #region ICollection<ExtensionRelationship> Members

        /// <summary>
        /// Add extension relationship object in collection
        /// </summary>
        /// <param name="extensionRelationship">Extension Relationship to add in collection</param>
        public void Add(ExtensionRelationship extensionRelationship)
        {
            this._extensionRelationships.Add(extensionRelationship);
        }

        /// <summary>
        /// Add extension relationship object in collection
        /// </summary>
        /// <param name="iExtensionRelationship">Extension Relationship to add in collection</param>
        public void Add(IExtensionRelationship iExtensionRelationship)
        {
            if (iExtensionRelationship != null)
            {
                this.Add((ExtensionRelationship)iExtensionRelationship);
            }
        }

        /// <summary>
        /// Removes all relationships from collection
        /// </summary>
        public void Clear()
        {
            this._extensionRelationships.Clear();
        }

        /// <summary>
        /// Determines whether the ExtensionRelationshipCollection contains a specific extension relationship
        /// </summary>
        /// <param name="extensionRelationship">The extension relationship object to locate in the extensionRelationshipCollection.</param>
        /// <returns>
        /// <para>true : If extension relationship found in ExtensionRelationshipCollection</para>
        /// <para>false : If extension relationship found not in ExtensionRelationshipCollection</para>
        /// </returns>
        public Boolean Contains(ExtensionRelationship extensionRelationship)
        {
            return this._extensionRelationships.Contains(extensionRelationship);
        }

        /// <summary>
        /// Copies the elements of the ExtensionRelationshipCollection to a
        /// System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from ExtensionRelationshipCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(ExtensionRelationship[] array, Int32 arrayIndex)
        {
            this._extensionRelationships.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of relationship in ExtensionRelationshipCollection
        /// </summary>
        public Int32 Count
        {
            get
            {
                return this._extensionRelationships.Count;
            }
        }

        /// <summary>
        /// Check if ExtensionRelationshipCollection is read-only.
        /// </summary>
        public Boolean IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific extension relationship from the ExtensionRelationshipCollection.
        /// </summary>
        /// <param name="extensionRelationship">The extension relationship object to remove from the ExtensionRelationshipCollection.</param>
        /// <returns>true if extension relationship is successfully removed; otherwise, false. This method also returns false if extension relationship was not found in the original collection</returns>
        public Boolean Remove(ExtensionRelationship extensionRelationship)
        {
            return this._extensionRelationships.Remove(extensionRelationship);
        }

        #endregion

        #region IEnumerable<ExtensionRelationship> Members

        /// <summary>
        /// Returns an enumerator that iterates through a ExtensionRelationshipCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<ExtensionRelationship> GetEnumerator()
        {
            return this._extensionRelationships.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a ExtensionRelationshipCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._extensionRelationships.GetEnumerator();
        }

        #endregion

        #region Internal methods to get / search related entities

        /// <summary>
        /// Gets all extended entities from extension relationships
        /// </summary>
        /// <returns>Returns IEntityCollection</returns>
        internal IEntityCollection GetAllExtendedEntities()
        {
            EntityCollection allRelatedEntities = null;

            allRelatedEntities = (EntityCollection)FindRelatedEntities(extensionRelationship => extensionRelationship.Direction == RelationshipDirection.Up, true);

            EntityCollection allRelatedChildEntities = (EntityCollection)FindRelatedEntities(extensionRelationship => extensionRelationship.Direction == RelationshipDirection.Down, true);

            if (allRelatedChildEntities != null)
            {
                if (allRelatedEntities == null)
                {
                    allRelatedEntities = new EntityCollection();
                }

                allRelatedEntities.AddRange(allRelatedChildEntities);
            }

            return allRelatedEntities;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="categoryPath"></param>
        /// <returns></returns>
        internal IEntity GetExtendedEntityByContainerNameAndCategoryPath(String containerName, String categoryPath)
        {
            IEntity relatedEntity = FindRelatedEntity(extensionRelationship => extensionRelationship.Direction == RelationshipDirection.Down &&
                                     String.Compare(extensionRelationship.GetRelatedEntity().ContainerName, containerName, StringComparison.InvariantCultureIgnoreCase) == 0 &&
                                     String.Compare(extensionRelationship.GetRelatedEntity().CategoryPath, categoryPath, StringComparison.InvariantCultureIgnoreCase) == 0,
                                     true);

            //if related entity is not found with 'Down' direction then try to find out based on 'Up' direction
            if (relatedEntity == null)
            {
                relatedEntity = FindRelatedEntity(extensionRelationship => extensionRelationship.Direction == RelationshipDirection.Up &&
                                     String.Compare(extensionRelationship.GetRelatedEntity().ContainerName, containerName, StringComparison.InvariantCultureIgnoreCase) == 0 &&
                                     String.Compare(extensionRelationship.GetRelatedEntity().CategoryPath, categoryPath, StringComparison.InvariantCultureIgnoreCase) == 0,
                                     true);
            }

            return relatedEntity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="compareMethod"></param>
        /// <param name="isRecursive"></param>
        /// <returns></returns>
        internal IEntity FindRelatedEntity(Func<IExtensionRelationship, Boolean> compareMethod, Boolean isRecursive = false)
        {
            Entity relatedEntity = null;

            var relatedEntities = FindRelatedEntities(compareMethod, isRecursive);

            if (relatedEntities != null && relatedEntities.Count > 0)
            {
                relatedEntity = relatedEntities.ElementAt(0);
            }

            return relatedEntity;
        }

        /// <summary>
        /// Gets related entities for extension relationship
        /// </summary>
        /// <param name="compareMethod">Indicates a condition based on what extension relationship can be processed further.</param>
        /// <param name="isRecursive">Indicates whether to call this method recursively to get the child entities from child extension relationship or not</param>
        internal IEntityCollection FindRelatedEntities(Func<IExtensionRelationship, Boolean> compareMethod, Boolean isRecursive = false)
        {
            var extensionRelationships = this;
            var allRelatedEntities = new EntityCollection();

            FindRelatedEntitiesRecursive(allRelatedEntities, extensionRelationships, compareMethod, isRecursive);

            return allRelatedEntities;
        }

        /// <summary>
        /// Gets related entities recursively.
        /// </summary>
        /// <param name="allRelatedEntities">Indicates entity collection</param>
        /// <param name="extensionRelationships">Indicates extension relationship collection</param>
        /// <param name="compareMethod">Indicates a condition based on what extension relationship can be processed further.</param>
        /// <param name="isRecursive">Indicates whether to call this method recursively to get the child entities from child extension relationship or not</param>
        internal static void FindRelatedEntitiesRecursive(IEntityCollection allRelatedEntities, ExtensionRelationshipCollection extensionRelationships, Func<ExtensionRelationship, Boolean> compareMethod, Boolean isRecursive = false)
        {
            if (extensionRelationships != null && extensionRelationships.Count > 0)
            {
                foreach (var extensionRelationship in extensionRelationships)
                {
                    var relatedEntity = extensionRelationship.RelatedEntity;

                    if (relatedEntity != null)
                    {
                        if (compareMethod.Invoke(extensionRelationship))
                        {
                            allRelatedEntities.Add(relatedEntity);
                        }

                        if (isRecursive)
                        {
                            FindRelatedEntitiesRecursive(allRelatedEntities, relatedEntity.ExtensionRelationships, compareMethod, isRecursive);
                        }
                    }
                }
            }
        }

        #endregion
    }
}