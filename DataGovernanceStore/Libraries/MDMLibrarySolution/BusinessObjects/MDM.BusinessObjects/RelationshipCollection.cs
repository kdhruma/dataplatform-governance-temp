using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the collection of relationships
    /// </summary>
    [DataContract]
    [ProtoContract]
    [KnownType(typeof(RelationshipBaseCollection))]
    public class RelationshipCollection : ICollection<Relationship>, IEnumerable<Relationship>, IRelationshipCollection
    {
        #region Fields

        /// <summary>
        /// Field denoting collection of  relationships
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        private Collection<Relationship> _relationships = new Collection<Relationship>();

        /// <summary>
        /// Field which indicates allowed user actions for relationship level
        /// </summary>
        private Collection<UserAction> _permissionSet = null;

        /// <summary>
        /// Field which indicates De-normalized relationships are dirty or not.
        /// </summary>
        private Boolean _isDenormalizedRelationshipsDirty = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the  relationship collection class
        /// </summary>
        public RelationshipCollection()
        {

        }

        /// <summary>
        /// Initializes a new instance of the  relationship collection class with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        public RelationshipCollection(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            /*
             * Sample XML:
             *  <Relationships>
                    <Relationship Id="124" ObjectType="Relationship" Type="Entity" RelatedEntityId="[GrandChild1]" Direction="Down" Path="[Current]-[Child]-[GrandChild1]" Action="Read">
                        <RelationshipAttributes />
                        <Relationships />
                    </Relationship>
                    <Relationship Id="125" ObjectType="Relationship" Type="Entity" RelatedEntityId="[GrandChild2]" Direction="Down" Path="[Current]-[Child]-[GrandChild2]" Action="Read">
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
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Relationships" && reader.HasAttributes)
                    {
                        if (reader.MoveToAttribute("PS"))
                        {
                            this.PermissionSet = Utility.GetPermissionsAsObject(reader.ReadContentAsString());
                        }

                        if (objectSerialization == ObjectSerialization.DataTransfer)
                        {
                            if (reader.MoveToAttribute("IsInhRelsDirty"))
                            {
                                this.IsDenormalizedRelationshipsDirty = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._isDenormalizedRelationshipsDirty);
                            }
                        }

                        reader.Read();
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Relationship")
                    {
                        String relationshipXml = reader.ReadOuterXml();

                        if (!String.IsNullOrWhiteSpace(relationshipXml))
                        {
                            Relationship Relationship = new Relationship(relationshipXml, objectSerialization);

                            if (Relationship != null)
                                this._relationships.Add(Relationship);
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
        /// Initialize RelationshipCollection from relationship list
        /// </summary>
        /// <param name="relationshipList">List of relationship objects</param>
        public RelationshipCollection(IList<Relationship> relationshipList)
        {
            this._relationships = new Collection<Relationship>(relationshipList);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property which indicates allowed user actions for relationship level
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        public Collection<UserAction> PermissionSet
        {
            get { return _permissionSet; }
            set { _permissionSet = value; }
        }

        /// <summary>
        /// Indicates whether De-normalized relationships are dirty or not.
        /// </summary>
        [DataMember]
        [ProtoMember(3)]
        public Boolean IsDenormalizedRelationshipsDirty
        {
            get { return _isDenormalizedRelationshipsDirty; }
            set { _isDenormalizedRelationshipsDirty = value; }
        }

        #endregion

        #region Indexers

        /// <summary>
        /// Find  relationship from collection based on logically related entity id
        /// </summary>
        /// <param name="relatedEntityId">Id of the related entity using which  relationship is to be searched from collection</param>
        /// <returns> Relationship object</returns>
        public Relationship this[Int64 relatedEntityId]
        {
            get
            {
                Relationship Relationship = GetRelationship(relatedEntityId);

                if (Relationship == null)
                    throw new ArgumentException("No  relationship found for given related entity id");
                else
                    return Relationship;
            }
        }

        #endregion Indexers

        #region Public Methods

        /// <summary>
        /// Check if RelationshipCollection contains relationship with given related entity id
        /// </summary>
        /// <param name="relatedEntityId">Id of the related entity using which relationship is to be searched from collection</param>
        /// <returns>
        /// <para>true : If relationship found in RelationshipCollection</para>
        /// <para>false : If relationship found not in RelationshipCollection</para>
        /// </returns>
        public Boolean Contains(Int64 relatedEntityId)
        {
            return (GetRelationship(relatedEntityId) != null) ? true : false;
        }

        /// <summary>
        /// Check if RelationshipCollection contains relationship with given context parameters
        /// </summary>
        /// <param name="fromEntityId">Id of the From Entity using which relationship is to be searched from collection</param>
        /// <param name="relatedEntityId">Id of the related entity using which relationship is to be searched from collection</param>
        /// <param name="containerId">Id of the container using which relationship is to be searched from collection</param>
        /// <param name="relationshipTypeId">Id of the relationship type using which relationship is to be searched from collection</param>
        /// <returns>
        /// <para>true : If relationship found in RelationshipCollection</para>
        /// <para>false : If relationship found not in RelationshipCollection</para>
        /// </returns>
        public Boolean Contains(Int64 fromEntityId, Int64 relatedEntityId, Int32 containerId, Int32 relationshipTypeId)
        {
            return (GetRelationship(fromEntityId, relatedEntityId, containerId, relationshipTypeId) != null) ? true : false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromEntityId"></param>
        /// <param name="relatedEntityId"></param>
        /// <param name="containerId"></param>
        /// <param name="relationshipTypeId"></param>
        /// <returns></returns>
        public Relationship GetRelationship(Int64 fromEntityId, Int64 relatedEntityId, Int32 containerId, Int32 relationshipTypeId)
        {
            Relationship requestedRelationship = null;

            foreach (Relationship relationship in this._relationships)
            {
                if (relationship.FromEntityId == fromEntityId && relationship.RelatedEntityId == relatedEntityId &&
                    relationship.ContainerId == containerId && relationship.RelationshipTypeId == relationshipTypeId)
                {
                    requestedRelationship = relationship;
                    break;
                }
            }

            return requestedRelationship;
        }

        /// <summary>
        /// Get relationships based on the specified parameters.
        /// </summary>
        /// <param name="fromEntityId">Indicates the from entity id based on which relationships to be fetched</param>
        /// <param name="relatedEntityId">Indicates the related entity id based on which relationships to be fetched</param>
        /// <param name="containerId">Indicates the relationship to container name based on which relationships to be fetched</param>
        /// <param name="relationshipTypeId">Indicates the relationship type id based on which relationships to be fetched</param>
        /// <returns>Returns relationships based on the specified parameters</returns>
        public RelationshipCollection GetRelationships(Int64 fromEntityId, Int64 relatedEntityId, Int32 containerId, Int32 relationshipTypeId)
        {
            RelationshipCollection requestedRelationships = new RelationshipCollection();

            if (this._relationships != null && this._relationships.Count > 0)
            {
                foreach (Relationship relationship in this._relationships)
                {
                    if (relationship.FromEntityId == fromEntityId && relationship.RelatedEntityId == relatedEntityId &&
                        relationship.ContainerId == containerId && relationship.RelationshipTypeId == relationshipTypeId)
                    {
                        requestedRelationships.Add(relationship);
                    }
                }
            }

            return requestedRelationships;
        }

        /// <summary>
        /// Remove  relationship object from RelationshipCollection
        /// </summary>
        /// <param name="relatedEntityId">Id of the related entity using which Relationship is to be removed from collection</param>
        /// <returns>true if  relationship is successfully removed; otherwise, false. This method also returns false if  relationship was not found in the original collection</returns>
        public Boolean Remove(Int64 relatedEntityId)
        {
            Relationship Relationship = GetRelationship(relatedEntityId);

            if (Relationship == null)
                throw new ArgumentException("No relationship found for given related entity id");
            else
                return this.Remove(Relationship);
        }

        /// <summary>
        /// Get Xml representation of  Relationship Collection
        /// </summary>
        /// <returns>Xml representation of  Relationship Collection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            returnXml = "<Relationships>";

            if (this._relationships != null && this._relationships.Count > 0)
            {
                foreach (Relationship Relationship in this._relationships)
                {
                    returnXml = String.Concat(returnXml, Relationship.ToXml());
                }
            }

            returnXml = String.Concat(returnXml, "</Relationships>");

            return returnXml;
        }

        /// <summary>
        /// Get Xml representation of  Relationship Collection based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of  Relationship Collection</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String returnXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("Relationships");

            if (objectSerialization == ObjectSerialization.DataTransfer)
            {
                xmlWriter.WriteAttributeString("PS", Utility.GetPermissionsAsString(this.PermissionSet));
            }

            if (this._relationships != null && this._relationships.Count > 0)
            {
                foreach (Relationship relationship in this._relationships)
                {
                    xmlWriter.WriteRaw(relationship.ToXml(objectSerialization));
                }
            }

            xmlWriter.WriteEndElement();
            xmlWriter.Flush();
            returnXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return returnXml;
        }

        /// <summary>
        /// Converts RelationshipCollection object into xml format.
        /// In future, this method can be enhanced to accept EntityConversionContext object
        /// </summary>
        /// <param name="xmlWriter">Indicates a writer to generate xml reperesentation of RelationshipCollection object</param>
        internal void ConvertRelationshipCollectionToXml(XmlTextWriter xmlWriter)
        {
            if (xmlWriter != null)
            {
                //Relationships node start
                xmlWriter.WriteStartElement("Relationships");

                if (this._relationships != null && this._relationships.Count > 0)
                {
                    foreach (Relationship relationship in this._relationships)
                    {
                        relationship.ConvertRelationshipToXml(xmlWriter);
                    }
                }

                // Relatinships node end
                xmlWriter.WriteEndElement();
            }
            else
            {
                throw new ArgumentNullException("xmlWriter", "XmlTextWriter is null. Can not write RelationshipCollection object.");
            }
        }

        /// <summary>
        /// Loads RelationshipCollection object from xml
        /// </summary>
        /// <param name="reader">Indicates an xml reader used to read xml format data</param>	
        /// <param name="context">Indicates context object which specifies what all data to be converted into object from Xml</param>
        internal void LoadRelationshipCollectionFromXml(XmlTextReader reader, EntityConversionContext context)
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
                            Relationship relationship = new Relationship();

                            relationship.LoadRelationshipFromXml(reader, context);

                            this.Add(relationship);
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Relationships")
                    {
                        return;
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("reader", "XmlTextReader is null. Can not read RelationshipCollection object.");
            }
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj is RelationshipCollection)
            {
                RelationshipCollection objectToBeCompared = obj as RelationshipCollection;

                Int32 RelationshipsUnion = this._relationships.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 RelationshipsIntersect = this._relationships.ToList().Intersect(objectToBeCompared.ToList()).Count();

                if (RelationshipsUnion != RelationshipsIntersect)
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

            foreach (Relationship Relationship in this._relationships)
            {
                hashCode += Relationship.GetHashCode();
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

            if (this._relationships != null && this._relationships.Count > 0)
            {
                IEnumerable<Relationship> updatedRelationships = this._relationships.Where(r => r.HasObjectChanged());

                if (updatedRelationships != null && updatedRelationships.Count() > 0)
                    hasObjectUpdated = true;
            }

            return hasObjectUpdated;
        }

        /// <summary>
        /// Get flat list of all the related entity ids.
        /// </summary>
        /// <returns></returns>
        public Collection<Int64> GetRelatedEntityIdList()
        {
            Collection<Int64> entityIdList = GetRelatedEntityIdListRecursive(this);

            entityIdList = new Collection<Int64>(entityIdList.Distinct().ToList());

            return entityIdList;
        }

        /// <summary>
        /// Get flat list of all the source entity ids.
        /// </summary>
        /// <returns></returns>
        public Collection<Int64> GetSourceEntityIdList()
        {
            Collection<Int64> entityIdList = GetSourceEntityIdListRecursive(this);

            entityIdList = new Collection<Int64>(entityIdList.Distinct().ToList());

            return entityIdList;
        }

        /// <summary>
        /// Get list of relationshipType ids.
        /// </summary>
        /// <returns></returns>
        public Collection<Int32> GetRelationshipTypeIds()
        {
            Collection<Int32> relationshipTypeIds = new Collection<Int32>();

            if (this._relationships != null && this._relationships.Count > 0)
            {
                PrepareRelationshipTypeIdList(relationshipTypeIds, this);
            }

            return relationshipTypeIds;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Collection<LocaleEnum> GetRelationshipLocaleList()
        {
            Collection<LocaleEnum> relationshipLocaleList = new Collection<LocaleEnum>();

            if (this._relationships != null && _relationships.Count > 0)
            {
                PrepareRelationshipLocaleList(relationshipLocaleList, this);
            }

            return relationshipLocaleList;
        }

        /// <summary>
        /// Denormalize relationships tree into flat structure
        /// </summary>
        /// <returns>De-normalized Relationships</returns>
        public IRelationshipCollection Denormalize()
        {
            RelationshipCollection denormalizedRelationships = new RelationshipCollection();

            if (this._relationships != null && this._relationships.Count > 0)
            {
                foreach (Relationship relationship in this._relationships)
                {
                    denormalizedRelationships.Add(relationship);

                    if (relationship.RelationshipCollection != null && relationship.RelationshipCollection.Count > 0)
                    {
                        IRelationshipCollection childDenormalizedRelationships = relationship.RelationshipCollection.Denormalize();
                        denormalizedRelationships = new RelationshipCollection(denormalizedRelationships.Concat(childDenormalizedRelationships).ToList());
                    }
                }
            }

            return denormalizedRelationships;
        }

        /// <summary>
        /// Compare relationshipCollection with current collection.
        /// This method will compare relationshipCollection. If current collection has more relationships than object to be compared, extra relationships will be ignored.
        /// If relationship to be compared has relationships which is not there in current collection, it will return false.
        /// </summary>
        /// <param name="subsetRelationshipCollection">RelationshipCollection to be compared with current collection</param>
        /// <param name="compareIds">Flag to determine whether id based comparison is true or not</param>
        /// <returns>True : Is both are same. False : otherwise</returns>
        public Boolean IsSuperSetOf(RelationshipCollection subsetRelationshipCollection, Boolean compareIds = false)
        {
            if (subsetRelationshipCollection != null)
            {
                foreach (Relationship relationship in subsetRelationshipCollection)
                {
                    if (this._relationships != null && this._relationships.Count > 0)
                    {
                        Relationship sourceRelationship = null;

                        sourceRelationship = this.GetRelationship(relationship.RelatedEntityId);

                        if (sourceRelationship == null)
                        {
                            sourceRelationship = this.GetRelationshipByToExternalId(relationship.ToExternalId);
                        }
                        
                        if (sourceRelationship == null || !sourceRelationship.IsSuperSetOf(relationship, compareIds))
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

        /// <summary>
        /// Compare relationshipCollection with current collection.
        /// This method will compare relationshipCollection. If current collection has more relationships than object to be compared, extra relationships will be ignored.
        /// If relationship to be compared has relationships which is not there in current collection, it will return false.
        /// </summary>
        /// <param name="subsetRelationshipCollection">RelationshipCollection to be compared with current collection</param>
        /// <param name="compareIds">Flag to determine whether id based comparison is true or not</param>
        /// <returns>RelationshipOperationResultCollection status is successful if no mismatch found, fail otherwise</returns>
        public RelationshipOperationResultCollection GetSuperSetOfOperationResult(RelationshipCollection subsetRelationshipCollection, Boolean compareIds = false)
        {
            var relationshipOperationResultCollection = new RelationshipOperationResultCollection();
            
            if (subsetRelationshipCollection != null)
            {
                foreach (Relationship relationship in subsetRelationshipCollection)
                {
                    if (this._relationships != null && this._relationships.Count > 0)
                    {
                        Relationship sourceRelationship = null;

                        sourceRelationship =  this.GetRelationship(relationship.RelatedEntityId);

                        if (sourceRelationship == null)
                        {
                            sourceRelationship = this.GetRelationshipByToExternalId(relationship.ToExternalId);
                        }

                        if (sourceRelationship != null)
                        {
                            RelationshipOperationResult relationshipOperationResult = sourceRelationship.GetSuperSetOfOperationResult(relationship);

                            if (relationshipOperationResult.OperationResultStatus != OperationResultStatusEnum.Successful)
                            {
                                relationshipOperationResultCollection.Add(relationshipOperationResult);
                            }
                        }
                        else
                        {
                            relationshipOperationResultCollection.AddRelationshipOperationResult((Int32)relationship.Id, "-1", String.Format("Relationship of type {0} is null", relationship.RelationshipTypeName), OperationResultType.Error);
                        }
                    }
                    else
                    {
                        relationshipOperationResultCollection.AddOperationResult("-1", String.Format("Relationships {0} is not loaded in source entity", relationship.RelationshipTypeName), OperationResultType.Error);
                    }
                }  
            }

            relationshipOperationResultCollection.RefreshOperationResultStatus();

            return relationshipOperationResultCollection;
        }

        /// <summary>
        /// Add child relationship into requested relationship parent id.
        /// </summary>
        /// <param name="relationshipParentId">Specifies relationship parent id.</param>
        /// <param name="childRelationship">Specifies child relationship</param>
        /// <returns>True if child relationship added successfully otherwise false.</returns>
        public Boolean AddChildRelationshipForParent(Int64 relationshipParentId, Relationship childRelationship)
        {
            Boolean result = false;

            if (this._relationships != null && this._relationships.Count > 0)
            {
                Relationship parentRelationship = (Relationship)this.GetRelationshipById(relationshipParentId);

                if (parentRelationship != null)
                {
                    parentRelationship.RelationshipCollection.Add(childRelationship);
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// Create a new relationship collection object.
        /// </summary>
        /// <returns>New relationship collection instance.</returns>
        public RelationshipCollection Clone()
        {
            RelationshipCollection clonedRelationships = new RelationshipCollection();

            if (this._relationships != null && this._relationships.Count > 0)
            {
                foreach (Relationship childRelationship in this._relationships)
                {
                    Relationship clonedChildRel = childRelationship.Clone(); // Recurse method
                    clonedRelationships.Add(clonedChildRel);
                }
            }

            return clonedRelationships;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relatedEntityId"></param>
        /// <returns></returns>
        private Relationship GetRelationship(Int64 relatedEntityId)
        {
            var filteredRelationships = from Relationship in this._relationships
                                        where Relationship.RelatedEntityId == relatedEntityId
                                        select Relationship;

            if (filteredRelationships.Any())
                return filteredRelationships.First();
            else
                return null;
        }

        /// <summary>
        /// Get Relationship By To ExternalId
        /// </summary>
        /// <param name="externalId"></param>
        /// <returns></returns>
        private Relationship GetRelationshipByToExternalId(String externalId)
        {
            foreach (var relationship in this._relationships)
            {
                if (String.Compare(relationship.ToExternalId, externalId, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    return relationship;
                }
            }

            return null;
        }

        private Collection<Int64> GetRelatedEntityIdListRecursive(RelationshipCollection relationshipCollection)
        {
            Collection<Int64> entityIdList = new Collection<Int64>();

            foreach (Relationship relationship in relationshipCollection)
            {
                entityIdList.Add(relationship.RelatedEntityId);

                if (relationship.RelationshipCollection != null && relationship.RelationshipCollection.Count > 0)
                {
                    Collection<Int64> childRelationshipEntityIdList = GetRelatedEntityIdListRecursive(relationship.RelationshipCollection);

                    entityIdList = ValueTypeHelper.MergeCollections(entityIdList, childRelationshipEntityIdList);
                }
            }

            return entityIdList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationships"></param>
        /// <returns></returns>
        private Collection<Int64> GetSourceEntityIdListRecursive(RelationshipCollection relationships)
        {
            Collection<Int64> entityIdList = new Collection<Int64>();

            if (relationships != null && relationships.Count > 0)
            {
                foreach (Relationship relationship in relationships)
                {
                    if (relationship.RelationshipSourceEntityId > 0 && relationship.RelatedEntityId != relationship.RelationshipSourceEntityId)
                    {
                        entityIdList.Add(relationship.RelationshipSourceEntityId);
                    }

                    Collection<Int64> childRelationshipSourceEntityIdList = GetSourceEntityIdListRecursive(relationship.RelationshipCollection);

                    if (childRelationshipSourceEntityIdList != null && childRelationshipSourceEntityIdList.Count > 0)
                    {
                        entityIdList = ValueTypeHelper.MergeCollections(entityIdList, childRelationshipSourceEntityIdList);
                    }
                }
            }

            return entityIdList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationship"></param>
        /// <returns></returns>
        private RelationshipCollection GetUpdatedRelationships(Relationship relationship)
        {
            RelationshipCollection updatedRelationships = new RelationshipCollection();

            if (relationship.Action == ObjectAction.Create || relationship.Action == ObjectAction.Update || relationship.Action == ObjectAction.Delete)
            {
                updatedRelationships.Add(relationship);
            }

            foreach (Relationship rel in relationship.RelationshipCollection)
            {
                updatedRelationships.AddRange(GetUpdatedRelationships(rel));
            }

            return updatedRelationships;
        }

        private void PrepareRelationshipTypeIdList(Collection<Int32> relationshipTypeIds, RelationshipCollection relationships)
        {
            foreach (Relationship relationship in relationships)
            {
                if (!relationshipTypeIds.Contains(relationship.RelationshipTypeId))
                {
                    relationshipTypeIds.Add(relationship.RelationshipTypeId);
                }

                if (relationship.RelationshipCollection != null && relationship.RelationshipCollection.Count > 0)
                {
                    PrepareRelationshipTypeIdList(relationshipTypeIds, relationship.RelationshipCollection);
                }
            }
        }

        private void PrepareRelationshipLocaleList(Collection<LocaleEnum> relationshipLocaleList, RelationshipCollection relationships)
        {
            foreach (Relationship relationship in relationships)
            {
                if (!relationshipLocaleList.Contains(relationship.Locale))
                {
                    relationshipLocaleList.Add(relationship.Locale);
                }

                if (relationship.RelationshipAttributes != null && relationship.RelationshipAttributes.Count > 0)
                {
                    foreach (Attribute relAttribute in relationship.RelationshipAttributes)
                    {
                        if (!relationshipLocaleList.Contains(relAttribute.Locale))
                        {
                            relationshipLocaleList.Add(relAttribute.Locale);
                        }
                    }
                }

                if (relationship.RelationshipCollection != null && relationship.RelationshipCollection.Count > 0)
                {
                    PrepareRelationshipLocaleList(relationshipLocaleList, relationship.RelationshipCollection);
                }
            }
        }

        #endregion

        #region ICollection<Relationship> Members

        /// <summary>
        /// Add  relationship object in collection
        /// </summary>
        /// <param name="relationship"> Relationship to add in collection</param>
        public void Add(Relationship relationship)
        {
            this._relationships.Add(relationship);
        }

        /// <summary>
        /// Add  relationship object in collection
        /// </summary>
        /// <param name="iRelationship"> Relationship to add in collection</param>
        public void Add(IRelationship iRelationship)
        {
            this.Add((Relationship)iRelationship);
        }

        /// <summary>
        /// Add Relationship object in collection
        /// </summary>
        /// <param name="items">Relationship to add in collection</param>
        public void AddRange(IRelationshipCollection items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("Relationships");
            }

            foreach (Relationship relationship in items)
            {
                if (!this.Contains(relationship))
                {
                    this.Add(relationship);
                }
            }
        }

        /// <summary>
        /// Removes all relationships from collection
        /// </summary>
        public void Clear()
        {
            this._relationships.Clear();
        }

        /// <summary>
        /// Determines whether the RelationshipCollection contains a specific  relationship
        /// </summary>
        /// <param name="relationship">The  relationship object to locate in the RelationshipCollection.</param>
        /// <returns>
        /// <para>true : If  relationship found in RelationshipCollection</para>
        /// <para>false : If  relationship found not in RelationshipCollection</para>
        /// </returns>
        public Boolean Contains(Relationship relationship)
        {
            return this._relationships.Contains(relationship);
        }

        /// <summary>
        /// Copies the elements of the RelationshipCollection to a
        /// System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from RelationshipCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(Relationship[] array, Int32 arrayIndex)
        {
            this._relationships.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of relationship in RelationshipCollection
        /// </summary>
        public Int32 Count
        {
            get
            {
                return this._relationships.Count;
            }
        }

        /// <summary>
        /// Check if RelationshipCollection is read-only.
        /// </summary>
        public Boolean IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific  relationship from the RelationshipCollection.
        /// </summary>
        /// <param name="relationship">The  relationship object to remove from the RelationshipCollection.</param>
        /// <returns>true if  relationship is successfully removed; otherwise, false. This method also returns false if  relationship was not found in the original collection</returns>
        public Boolean Remove(Relationship relationship)
        {
            return this._relationships.Remove(relationship);
        }

        #endregion

        #region IEnumerable<Relationship> Members

        /// <summary>
        /// Returns an enumerator that iterates through a RelationshipCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<Relationship> GetEnumerator()
        {
            return this._relationships.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a RelationshipCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._relationships.GetEnumerator();
        }

        #endregion

        #region IRelationshipCollection methods

        /// <summary>
        /// Marks all the relationships as readOnly
        /// </summary>
        public void MarkAsReadOnly()
        {
            if (this._relationships != null)
            {
                this.PermissionSet = new Collection<UserAction>() { UserAction.View };

                foreach (Relationship relationship in this._relationships)
                {
                    relationship.PermissionSet = new Collection<UserAction>() { UserAction.View };
                }
            }
        }

        /// <summary>
        /// Get flat list of updated relationships from current relationship collection
        /// </summary>
        /// <returns>Flat list of updated relationships </returns>
        public IRelationshipCollection GetUpdatedRelationships()
        {
            var flatUpdatedRelationships = new RelationshipCollection();

            foreach (Relationship relationship in this._relationships)
            {
                //DO recursive get..
                //Add range don't add relationship
                flatUpdatedRelationships.AddRange(GetUpdatedRelationships(relationship));
            }

            return flatUpdatedRelationships;
        }

        /// <summary>
        /// Get the relationship based on the Relationship Id
        /// </summary>
        /// <param name="relationshipId">Indicate the relationship type id for the Relationship</param>
        /// <returns>Relationship Interface</returns>
        public IRelationship GetRelationshipById(Int64 relationshipId)
        {
            IRelationship resultedRelationship = null;

            if (this._relationships != null && this._relationships.Count > 0)
            {
                foreach (Relationship relationship in this._relationships)
                {
                    if (relationship.Id == relationshipId)
                    {
                        resultedRelationship = relationship;
                        break;
                    }

                    resultedRelationship = relationship.RelationshipCollection.GetRelationshipById(relationshipId);

                    if (resultedRelationship != null)
                    {
                        break;
                    }
                }
            }

            return resultedRelationship;
        }

        /// <summary>
        /// Get the relationship based on the 'from Entity Id + relationship Entity Id + Relationship type Id' combination
        /// </summary>
        /// <param name="relatedEntityId">Indicate the related Entity Id for the Relationship</param>
        /// <param name="relationshipTypeId">Indicate the relationship type Id for the Relationship</param>
        /// <returns>Relationship Interface</returns>
        public IRelationship GetRelationship(Int64 relatedEntityId, Int32 relationshipTypeId)
        {
            IRelationship resultedRelationship = null;

            if (this._relationships != null && this._relationships.Count > 0)
            {
                foreach (Relationship relationship in this._relationships)
                {
                    if (relationship.RelatedEntityId == relatedEntityId && relationship.RelationshipTypeId == relationshipTypeId)
                    {
                        resultedRelationship = relationship;
                        break;
                    }

                    resultedRelationship = relationship.RelationshipCollection.GetRelationship(relatedEntityId, relationshipTypeId);

                    if (resultedRelationship != null)
                    {
                        break;
                    }
                }
            }

            return resultedRelationship;
        }

        #endregion IRelationshipCollection methods
    }
}
