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
    using Core;
    using MDM.Interfaces;    

    /// <summary>
    /// Specifies the collection of hierarchy relationships
    /// </summary>
    [DataContract]
    [ProtoContract]
    [KnownType(typeof(RelationshipBaseCollection))]
    public class HierarchyRelationshipCollection : ICollection<HierarchyRelationship>, IEnumerable<HierarchyRelationship>, IHierarchyRelationshipCollection
    {
        #region Fields

        /// <summary>
        /// Field denoting collection of hierarchy relationships
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        private Collection<HierarchyRelationship> _hierarchyRelationships = new Collection<HierarchyRelationship>();

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
        /// Find hierarchy relationship from collection based on related entity id
        /// </summary>
        /// <param name="relatedEntityId">Id of the related entity using which hierarchy relationship is to be searched from collection</param>
        /// <returns>Hierarchy Relationship object</returns>
        public HierarchyRelationship this[Int64 relatedEntityId]
        {
            get
            {
                HierarchyRelationship hierarchyRelationship = GetHierarchyRelationship(relatedEntityId);

                if (hierarchyRelationship == null)
                    throw new ArgumentException("No hierarchy relationship found for given related entity id");
                else
                    return hierarchyRelationship;
            }
        }

        /// <summary>
        /// Initializes a new instance of the hierarchy relationship collection class
        /// </summary>
        public HierarchyRelationshipCollection()
        {

        }

        /// <summary>
        /// Initializes a new instance of the hierarchy relationship collection class with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        public HierarchyRelationshipCollection(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            /*
             * Sample XML:
             *  <Relationships>
                    <Relationship Id="124" ObjectType="HierarchyRelationship" Type="Entity" RelatedEntityId="[GrandChild1]" Direction="Down" Path="[Current]-[Child]-[GrandChild1]" Action="Read">
                        <RelationshipAttributes />
                        <Relationships />
                    </Relationship>
                    <Relationship Id="125" ObjectType="HierarchyRelationship" Type="Entity" RelatedEntityId="[GrandChild2]" Direction="Down" Path="[Current]-[Child]-[GrandChild2]" Action="Read">
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
                            HierarchyRelationship hierarchyRelationship = new HierarchyRelationship(relationshipXml, objectSerialization);

                            if (hierarchyRelationship != null)
                                this._hierarchyRelationships.Add(hierarchyRelationship);
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
        /// Check if HierarchyRelationshipCollection contains relationship with given related entity id
        /// </summary>
        /// <param name="relatedEntityId">Id of the related entity using which hierarchy relationship is to be searched from collection</param>
        /// <returns>
        /// <para>true : If relationship found in HierarchyRelationshipCollection</para>
        /// <para>false : If relationship found not in HierarchyRelationshipCollection</para>
        /// </returns>
        public Boolean Contains(Int64 relatedEntityId)
        {
            if (GetHierarchyRelationship(relatedEntityId) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Remove hierarchy relationship object from HierarchyRelationshipCollection
        /// </summary>
        /// <param name="relatedEntityId">Id of the related entity using which HierarchyRelationship is to be removed from collection</param>
        /// <returns>true if hierarchy relationship is successfully removed; otherwise, false. This method also returns false if hierarchy relationship was not found in the original collection</returns>
        public Boolean Remove(Int64 relatedEntityId)
        {
            HierarchyRelationship hierarchyRelationship = GetHierarchyRelationship(relatedEntityId);

            if (hierarchyRelationship == null)
                throw new ArgumentException("No hierarchy relationship found for given related entity id");
            else
                return this.Remove(hierarchyRelationship);
        }

        /// <summary>
        /// Get Xml representation of Hierarchy Relationship Collection
        /// </summary>
        /// <returns>Xml representation of Hierarchy Relationship Collection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            returnXml = "<Hierarchies>";

            if (this._hierarchyRelationships != null && this._hierarchyRelationships.Count > 0)
            {
                foreach (HierarchyRelationship hierarchyRelationship in this._hierarchyRelationships)
                {
                    returnXml = String.Concat(returnXml, hierarchyRelationship.ToXml());
                }
            }

            returnXml = String.Concat(returnXml, "</Hierarchies>");

            return returnXml;
        }       

        /// <summary>
        /// Get Xml representation of Hierarchy Relationship Collection based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Hierarchy Relationship Collection</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String returnXml = String.Empty;

            returnXml = "<Hierarchies>";

            if (this._hierarchyRelationships != null && this._hierarchyRelationships.Count > 0)
            {
                foreach (HierarchyRelationship hierarchyRelationship in this._hierarchyRelationships)
                {
                    returnXml = String.Concat(returnXml, hierarchyRelationship.ToXml(objectSerialization));
                }
            }

            returnXml = String.Concat(returnXml, "</Hierarchies>");

            return returnXml;
        }

        /// <summary>
        /// Compare hierarchyRelationshipCollection with current collection.
        /// This method will compare hierarchyrelationshipcollection. If current collection has more hierarchyrelationships than object to be compared, extra hierarchyrelationships will be ignored.
        /// If hierarchyrelationship to be compared has hierarchyrelationships which is not there in current collection, it will return false.
        /// </summary>
        /// <param name="subsetHierarchyRelationshipCollection">HierarchyRelationshipCollection to be compared with current collection</param>
        /// <param name="compareIds">Flag to determine whether id based comparison is true or not</param>
        /// <returns>True : Is both are same. False : otherwise</returns>
        public Boolean IsSuperSetOf(HierarchyRelationshipCollection subsetHierarchyRelationshipCollection, Boolean compareIds = false)
        {
            if (subsetHierarchyRelationshipCollection != null)
            {
                if (this._hierarchyRelationships != null && this._hierarchyRelationships.Count > 0)
                {
                    foreach (HierarchyRelationship hierarchyRelationship in subsetHierarchyRelationshipCollection)
                    {
                        HierarchyRelationship sourceHierarchyRelationship = this.Where(a => a.Name == hierarchyRelationship.Name).FirstOrDefault();

                        if (sourceHierarchyRelationship == null || !sourceHierarchyRelationship.IsSuperSetOf(hierarchyRelationship, compareIds))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subsetHierarchyRelationshipCollection"></param>
        /// <param name="operationResult"></param>
        /// <param name="compareIds"></param>
        /// <returns></returns>
        public Boolean GetSuperSetOfOperationResult(HierarchyRelationshipCollection subsetHierarchyRelationshipCollection, OperationResult operationResult, Boolean compareIds = false)
        {
            #region compare hierarchy relationship collection

            if (subsetHierarchyRelationshipCollection != null)
            {
                if (this._hierarchyRelationships != null && this._hierarchyRelationships.Count > 0)
                {
                    foreach (HierarchyRelationship hierarchyRelationship in subsetHierarchyRelationshipCollection)
                    {
                        HierarchyRelationship sourceHierarchyRelationship = this.FirstOrDefault(a => a.Name == hierarchyRelationship.Name);
                        if (sourceHierarchyRelationship != null)
                        {
                            sourceHierarchyRelationship.GetSuperSetOperationResult(hierarchyRelationship, operationResult, compareIds);
                        }
                    }
                }
            }
            else
            {
                operationResult.AddOperationResult("-1", "SubsetHierarchyRelationshipCollection is null - nothing to compare to", OperationResultType.Information);
            }

            #endregion compare hierarchy relationship colleciton

            #region refresh and return

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

        /// <summary>
        /// Converts HierarchyRelationshipCollection object into xml format.
        /// In future, this method can be enhanced to accept EntityConversionContext object
        /// </summary>
        /// <param name="xmlWriter">Indicates a writer to generate xml reperesentation of HierarchyRelationshipCollection object</param>
        /// <param name="context">Indicates context object which specifies what all data of entity to be converted into Xml</param>
        internal void ConvertHierarchyRelationshipCollectionToXml(XmlTextWriter xmlWriter, EntityConversionContext context)
        {
            if (xmlWriter != null)
            {
                // HierarchyRelationships node start
                xmlWriter.WriteStartElement("Hierarchies");

                if (this._hierarchyRelationships != null && this._hierarchyRelationships.Count > 0)
                {
                    foreach (HierarchyRelationship hierarchyRelationship in this._hierarchyRelationships)
                    {
                        hierarchyRelationship.ConvertHierarchyRelationshipToXml(xmlWriter, context);
                    }
                }

                // HierarchyRelationships node end
                xmlWriter.WriteEndElement();
            }
            else
            {
                throw new ArgumentNullException("xmlWriter", "XmlTextWriter is null. Can not write HierarchyRelationshipCollection object.");
            }
        }

        /// <summary>
        /// Find HierarchyRelationship for given related entity recursively.
        /// </summary>
        /// <param name="relatedEntityId">RelatedEntityId which is to be searched in the collection</param>
        /// <returns>HierarchyRelationship having given RelatedEntityId</returns>
        public HierarchyRelationship FindHierarchyRelationshipRecursive(Int64 relatedEntityId)
        {
            HierarchyRelationship hierarchyRelationship = null;

            if (this._hierarchyRelationships != null && this._hierarchyRelationships.Count > 0)
            {
                foreach (HierarchyRelationship hierarchyRel in this._hierarchyRelationships)
                {
                    if (hierarchyRel.RelatedEntityId == relatedEntityId)
                        hierarchyRelationship = hierarchyRel;
                    else
                    {
                        if (hierarchyRel.RelationshipCollection != null && hierarchyRel.RelationshipCollection.Count > 0)
                        {
                            hierarchyRelationship = hierarchyRel.RelationshipCollection.FindHierarchyRelationshipRecursive(relatedEntityId);
                        }
                    }

                    if (hierarchyRelationship != null)
                        break;
                }
            }

            return hierarchyRelationship;
        }

        /// <summary>
        /// Find HierarchyRelationship for given related entity.
        /// </summary>
        /// <param name="relatedEntityId">RelatedEntityId which is to be searched in the collection at current level</param>
        /// <returns>HierarchyRelationship having given RelatedEntityId, if match found. Else, returns null.</returns>
        public HierarchyRelationship FindHierarchyRelationshipByRelatedEntityId(Int64 relatedEntityId)
        {
            HierarchyRelationship hierarchyRelationship = null;

            if (this._hierarchyRelationships != null && this._hierarchyRelationships.Count > 0)
            {
                foreach (HierarchyRelationship relationship in this._hierarchyRelationships)
                {
                    if (relationship.RelatedEntityId == relatedEntityId)
                    {
                        hierarchyRelationship = relationship;
                        break;
                    }
                }
            }

            return hierarchyRelationship;
        }

        /// <summary>
        /// Loads HierarchyRelationshipCollection from xml
        /// </summary>
        /// <param name="reader">Indicates an xml reader used to read xml format data</param>
        /// <param name="context">Indicates context object which specifies what all data to be converted into object from Xml</param>
        internal void LoadHierarchyRelationshipCollectionFromXml(XmlTextReader reader, EntityConversionContext context)
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
                            HierarchyRelationship hierarchyRelatioship = new HierarchyRelationship();

                            hierarchyRelatioship.LoadHierarchyRelationshipFromXml(reader, context);

                            this.Add(hierarchyRelatioship);
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Hierarchies")
                    {
                        return;
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("reader", "XmlTextReader is null. Can not read HierarchyReltionshipCollection object.");
            }
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj is HierarchyRelationshipCollection)
            {
                HierarchyRelationshipCollection objectToBeCompared = obj as HierarchyRelationshipCollection;

                Int32 hierarchyRelationshipsUnion = this._hierarchyRelationships.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 hierarchyRelationshipsIntersect = this._hierarchyRelationships.ToList().Intersect(objectToBeCompared.ToList()).Count();

                if (hierarchyRelationshipsUnion != hierarchyRelationshipsIntersect)
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

            foreach (HierarchyRelationship hierarchyRelationship in this._hierarchyRelationships)
            {
                hashCode += hierarchyRelationship.GetHashCode();
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

            if (this._hierarchyRelationships != null && this._hierarchyRelationships.Count > 0)
            {
                IEnumerable<HierarchyRelationship> updatedHierarchyRelationships = this._hierarchyRelationships.Where(h => h.HasObjectChanged());

                if (updatedHierarchyRelationships != null && updatedHierarchyRelationships.Count() > 0)
                    hasObjectUpdated = true;
            }

            return hasObjectUpdated;
        }

        /// <summary>
        /// Gets hierarchy relationship based on given hierarchy relationship name
        /// </summary>
        /// <param name="name">Indicates hierarchy relationship name</param>
        /// <returns>Returns HierarchyRelationship</returns>
        public HierarchyRelationship GetHierarchyRelationshipByName(String name)
        {
            foreach (HierarchyRelationship item in this._hierarchyRelationships)
            {
                if (String.Compare(item.Name, name, true) == 0)
                {
                    return item;
                }
            }

            return null;
        }

        #endregion

        #region Private Methods

        private HierarchyRelationship GetHierarchyRelationship(Int64 relatedEntityId)
        {
            Int32 hierarchyRelationshipsCount = _hierarchyRelationships.Count;
            HierarchyRelationship hierarchyRelationship = null;

            for (Int32 index = 0; index < hierarchyRelationshipsCount; index++)
            {
                hierarchyRelationship = _hierarchyRelationships[index];
                if (hierarchyRelationship.RelatedEntityId == relatedEntityId)
                    return hierarchyRelationship;
            }
            return null;
        }

        #endregion

        #region ICollection<HierarchyRelationship> Members

        /// <summary>
        /// Add hierarchy relationship object in collection
        /// </summary>
        /// <param name="hierarchyRelationship">Hierarchy Relationship to add in collection</param>
        public void Add(HierarchyRelationship hierarchyRelationship)
        {
            this._hierarchyRelationships.Add(hierarchyRelationship);
        }

        /// <summary>
        /// Removes all relationships from collection
        /// </summary>
        public void Clear()
        {
            this._hierarchyRelationships.Clear();
        }

        /// <summary>
        /// Determines whether the HierarchyRelationshipCollection contains a specific hierarchy relationship
        /// </summary>
        /// <param name="hierarchyRelationship">The hierarchy relationship object to locate in the hierarchyRelationshipCollection.</param>
        /// <returns>
        /// <para>true : If hierarchy relationship found in HierarchyRelationshipCollection</para>
        /// <para>false : If hierarchy relationship found not in HierarchyRelationshipCollection</para>
        /// </returns>
        public Boolean Contains(HierarchyRelationship hierarchyRelationship)
        {
            return this._hierarchyRelationships.Contains(hierarchyRelationship);
        }

        /// <summary>
        /// Copies the elements of the HierarchyRelationshipCollection to a
        /// System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from HierarchyRelationshipCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(HierarchyRelationship[] array, Int32 arrayIndex)
        {
            this._hierarchyRelationships.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of relationship in HierarchyRelationshipCollection
        /// </summary>
        public Int32 Count
        {
            get
            {
                return this._hierarchyRelationships.Count;
            }
        }

        /// <summary>
        /// Check if HierarchyRelationshipCollection is read-only.
        /// </summary>
        public Boolean IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific hierarchy relationship from the HierarchyRelationshipCollection.
        /// </summary>
        /// <param name="hierarchyRelationship">The hierarchy relationship object to remove from the HierarchyRelationshipCollection.</param>
        /// <returns>true if hierarchy relationship is successfully removed; otherwise, false. This method also returns false if hierarchy relationship was not found in the original collection</returns>
        public Boolean Remove(HierarchyRelationship hierarchyRelationship)
        {
            return this._hierarchyRelationships.Remove(hierarchyRelationship);
        }

        #endregion

        #region IEnumerable<HierarchyRelationship> Members

        /// <summary>
        /// Returns an enumerator that iterates through a HierarchyRelationshipCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<HierarchyRelationship> GetEnumerator()
        {
            return this._hierarchyRelationships.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a HierarchyRelationshipCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._hierarchyRelationships.GetEnumerator();
        }

        #endregion

        #region IHierarchyRelationshipCollection Methods

        /// <summary>
        /// Gets only entity hierarchy relationships from the hierarchy relationships
        /// </summary>
        /// <returns>Entity hierarchy relationships</returns>
        public IHierarchyRelationshipCollection GetEntityHierarchyRelationships()
        {
            HierarchyRelationshipCollection entityHierarchyRelationships = new HierarchyRelationshipCollection();

            if (this._hierarchyRelationships != null && this._hierarchyRelationships.Count > 0)
            {
                foreach (HierarchyRelationship hierarchyRelationship in this._hierarchyRelationships)
                {
                    RelationshipType type = hierarchyRelationship.Type;

                    if (type != null)
                    {
                        if (type.Name.ToLowerInvariant() == "entity")
                        {
                            entityHierarchyRelationships.Add(hierarchyRelationship);
                        }
                        else
                        {
                            HierarchyRelationshipCollection childHierarchyRelationships = hierarchyRelationship.RelationshipCollection;

                            if (childHierarchyRelationships != null && childHierarchyRelationships.Count > 0)
                            {
                                HierarchyRelationshipCollection childEntityHierarchyRelationships = (HierarchyRelationshipCollection)childHierarchyRelationships.GetEntityHierarchyRelationships();

                                foreach (HierarchyRelationship hierarchyRel in childEntityHierarchyRelationships)
                                {
                                    entityHierarchyRelationships.Add(hierarchyRel);
                                }
                            }
                        }
                    }
                }
            }

            return entityHierarchyRelationships;
        }

        /// <summary>
        /// Filters hierarchy relationships based on direction
        /// </summary>
        /// <param name="direction">Direction by which relationships needs to be filtered.
        /// <para>
        /// Applicable values are 'Up' and 'Down'
        /// </para>
        /// </param>
        /// <returns>Filtered Hierarchy Relationships</returns>
        public IHierarchyRelationshipCollection FilterBy(RelationshipDirection direction)
        {
            HierarchyRelationshipCollection hierarchyRelationships = new HierarchyRelationshipCollection();

            if (this._hierarchyRelationships != null && this._hierarchyRelationships.Count > 0)
            {
                hierarchyRelationships.Action = this.Action;
                hierarchyRelationships.ChangeContext = this.ChangeContext;

                foreach (HierarchyRelationship hierarchyRelationship in this._hierarchyRelationships)
                {
                    if (hierarchyRelationship.Direction == direction)
                    {
                        hierarchyRelationships.Add(hierarchyRelationship);

                        //Apply direction filter for all the levels..
                        if (hierarchyRelationship.RelationshipCollection != null && hierarchyRelationship.RelationshipCollection.Count > 0)
                            hierarchyRelationship.RelationshipCollection = (HierarchyRelationshipCollection)hierarchyRelationship.RelationshipCollection.FilterBy(direction);
                    }
                }
            }

            return hierarchyRelationships;
        }

        /// <summary>
        /// Create a new hierarchy relationship collection object.
        /// </summary>
        /// <returns>New hierarchy relationship collection instance.</returns>
        public HierarchyRelationshipCollection Clone()
        {
            HierarchyRelationshipCollection clonedHierarchyRelationships = new HierarchyRelationshipCollection();

            if (this._hierarchyRelationships != null && this._hierarchyRelationships.Count > 0)
            {
                foreach (HierarchyRelationship childHierarchyRelationship in this._hierarchyRelationships)
                {
                    HierarchyRelationship clonedChildHierarchyRelationship = childHierarchyRelationship.Clone(); // Recurse method
                    clonedHierarchyRelationships.Add(clonedChildHierarchyRelationship);
                }
            }

            return clonedHierarchyRelationships;
        }

        #endregion

        #region Internal methods to get / search related entities

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal IEntityCollection GetChildEntities()
        {
            return FindRelatedEntities(hierarchyRelationship => hierarchyRelationship.Direction == RelationshipDirection.Down);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal IEntity GetParentEntity()
        {
            return FindRelatedEntity(hierarchyRelationship => hierarchyRelationship.Direction == RelationshipDirection.Up);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal IEntityCollection GetAllParentEntities()
        {
            return FindRelatedEntities(hierarchyRelationship => hierarchyRelationship.Direction == RelationshipDirection.Up, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal IEntityCollection GetAllChildEntities()
        {
            return FindRelatedEntities(hierarchyRelationship => hierarchyRelationship.Direction == RelationshipDirection.Down, true);
        }

        /// <summary>
        /// Gets all variants entities
        /// </summary>
        /// <returns>Returns collection of entities</returns>
        internal IEntityCollection GetAllVariantsEntities()
        {
            EntityCollection allEntities = new EntityCollection();

            EntityCollection parentEntities = (EntityCollection)this.GetAllParentEntities();

            if (parentEntities != null && parentEntities.Count > 0)
            {
                allEntities.AddRange(parentEntities);
            }

            EntityCollection childEntities = (EntityCollection)this.GetAllChildEntities();

            if (childEntities != null && childEntities.Count > 0)
            {
                allEntities.AddRange(childEntities);
            }

            return allEntities;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityTypeId"></param>
        /// <returns></returns>
        internal IEntityCollection GetChildEntitiesByEntityTypeId(Int32 entityTypeId)
        {
            return FindRelatedEntities(hierarchyRelationship => hierarchyRelationship.Direction == RelationshipDirection.Down && hierarchyRelationship.GetRelatedEntity().EntityTypeId == entityTypeId, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityTypeName"></param>
        /// <returns></returns>
        internal IEntityCollection GetChildEntitiesByEntityTypeName(String entityTypeName)
        {
            return FindRelatedEntities(hierarchyRelationship => hierarchyRelationship.Direction == RelationshipDirection.Down && hierarchyRelationship.GetRelatedEntity().EntityTypeName.Equals(entityTypeName, StringComparison.InvariantCultureIgnoreCase), true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        internal IEntity GetChildEntityByEntityId(Int64 entityId)
        {
            return FindRelatedEntity(hierarchyRelationship => hierarchyRelationship.Direction == RelationshipDirection.Down && hierarchyRelationship.GetRelatedEntity().Id == entityId, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        internal IEntity GetChildEntityByEntityName(String entityName)
        {
            return FindRelatedEntity(hierarchyRelationship => hierarchyRelationship.Direction == RelationshipDirection.Down && hierarchyRelationship.GetRelatedEntity().Name.Equals(entityName, StringComparison.InvariantCultureIgnoreCase), true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityTypeId"></param>
        /// <returns></returns>
        internal IEntity GetParentEntityByEntityTypeId(Int32 entityTypeId)
        {
            return FindRelatedEntity(hierarchyRelationship => hierarchyRelationship.Direction == RelationshipDirection.Up && hierarchyRelationship.GetRelatedEntity().EntityTypeId == entityTypeId, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityTypeName"></param>
        /// <returns></returns>
        internal IEntity GetParentEntityByEntityTypeName(String entityTypeName)
        {
            return FindRelatedEntity(hierarchyRelationship => hierarchyRelationship.Direction == RelationshipDirection.Up && hierarchyRelationship.GetRelatedEntity().EntityTypeName.Equals(entityTypeName, StringComparison.InvariantCultureIgnoreCase), true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        internal IEntity GetParentEntityByEntityId(Int64 entityId)
        {
            return FindRelatedEntity(hierarchyRelationship => hierarchyRelationship.Direction == RelationshipDirection.Up && hierarchyRelationship.GetRelatedEntity().Id == entityId, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        internal IEntity GetParentEntityByEntityName(String entityName)
        {
            return FindRelatedEntity(hierarchyRelationship => hierarchyRelationship.Direction == RelationshipDirection.Up && hierarchyRelationship.GetRelatedEntity().Name.Equals(entityName, StringComparison.InvariantCultureIgnoreCase), true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="compareMethod"></param>
        /// <param name="isRecursive"></param>
        internal IEntity FindRelatedEntity(Func<IHierarchyRelationship, Boolean> compareMethod, Boolean isRecursive = false)
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
        /// 
        /// </summary>
        /// <param name="compareMethod"></param>
        /// <param name="isRecursive"></param>
        internal IEntityCollection FindRelatedEntities(Func<IHierarchyRelationship, Boolean> compareMethod, Boolean isRecursive = false)
        {
            var hierarchyRelationships = this;
            var allRelatedEntities = new EntityCollection();

            FindRelatedEntitiesRecursive(allRelatedEntities, hierarchyRelationships, compareMethod, isRecursive);

            return allRelatedEntities;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="allRelatedEntities"></param>
        /// <param name="hierarchyRelationships"></param>
        /// <param name="compareMethod"></param>
        /// <param name="isRecursive"></param>
        internal static void FindRelatedEntitiesRecursive(IEntityCollection allRelatedEntities, HierarchyRelationshipCollection hierarchyRelationships, Func<HierarchyRelationship, Boolean> compareMethod, Boolean isRecursive = false)
        {
            if (hierarchyRelationships != null && hierarchyRelationships.Count > 0)
            {
                foreach (var hierarchyRelationship in hierarchyRelationships)
                {
                    var relatedEntity = hierarchyRelationship.RelatedEntity;

                    if (relatedEntity != null)
                    {
                        if (compareMethod.Invoke(hierarchyRelationship))
                        {
                            allRelatedEntities.Add(relatedEntity);
                        }

                        if (isRecursive)
                        {
                            FindRelatedEntitiesRecursive(allRelatedEntities, relatedEntity.HierarchyRelationships, compareMethod, isRecursive);
                        }
                    }
                }
            }
        }

        #endregion
    }
}
