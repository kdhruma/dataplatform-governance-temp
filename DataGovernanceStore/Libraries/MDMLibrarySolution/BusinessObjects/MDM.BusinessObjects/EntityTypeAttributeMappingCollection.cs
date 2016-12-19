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
    /// Specifies the Entity Type Attribute Mapping Instance Collection for the Object
    /// </summary>
    [DataContract]
    public class EntityTypeAttributeMappingCollection : ICollection<EntityTypeAttributeMapping>, IEnumerable<EntityTypeAttributeMapping>, IEntityTypeAttributeMappingCollection, IDataModelObjectCollection
    {
        #region Fields

        [DataMember]
        private Collection<EntityTypeAttributeMapping> _entityTypeAttributeMappings = new Collection<EntityTypeAttributeMapping>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter-less constructor
        /// </summary>
        public EntityTypeAttributeMappingCollection() : base() { }

        /// <summary>
        /// Initialize EntityTypeAttributeMappingCollection from IList
        /// </summary>
        /// <param name="EntityTypeAttributeMappingList">IList of EntityTypeAttributeMappings</param>
        public EntityTypeAttributeMappingCollection(IList<EntityTypeAttributeMapping> EntityTypeAttributeMappingList)
        {
            this._entityTypeAttributeMappings = new Collection<EntityTypeAttributeMapping>(EntityTypeAttributeMappingList);
        }

        /// <summary>
        /// Initializes a new instance of the EntityTypeAttributeMappingCollection class with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        public EntityTypeAttributeMappingCollection(String valuesAsXml)
        {
            LoadEntityTypeAttributeMappingCollection(valuesAsXml);
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
                return ObjectType.EntityTypeAttributeMapping;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(object obj)
        {
            if (obj is EntityTypeAttributeMappingCollection)
            {
                EntityTypeAttributeMappingCollection objectToBeCompared = obj as EntityTypeAttributeMappingCollection;
                Int32 entityTypeAttributeMappingsUnion = this._entityTypeAttributeMappings.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 entityTypeAttributeMappingsIntersect = this._entityTypeAttributeMappings.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if (entityTypeAttributeMappingsUnion != entityTypeAttributeMappingsIntersect)
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
            foreach (EntityTypeAttributeMapping entityTypeAttributeMapping in this._entityTypeAttributeMappings)
            {
                hashCode += entityTypeAttributeMapping.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// Gets the EntityTypeAttributeMapping for given EntityTypeName, AttributeName and AttributeParentName.
        /// </summary>
        /// <param name="entityTypeName">EntityTypeName to be searched in the collection</param>
        /// <param name="attributeName">AttributeName to be searched in the collection</param>
        /// <param name="attributeParentName">AttributeParentName to be searched in the collection</param>
        /// <returns>EntityTypeAttributeMapping having given EntityTypeName, AttributeName and AttributeParentName</returns>
        public EntityTypeAttributeMapping Get(String entityTypeName, String attributeName, String attributeParentName)
        {
            if (this._entityTypeAttributeMappings != null && this._entityTypeAttributeMappings.Count > 0)
            {
                entityTypeName = entityTypeName.ToLowerInvariant();
                attributeName = attributeName.ToLowerInvariant();
                attributeParentName = attributeParentName.ToLowerInvariant();

                foreach (EntityTypeAttributeMapping entityTypeAttributeMapping in this._entityTypeAttributeMappings)
                {
                    if (entityTypeAttributeMapping.EntityTypeName.ToLowerInvariant().Equals(entityTypeName) &&
                        entityTypeAttributeMapping.AttributeName.ToLowerInvariant().Equals(attributeName) &&
                        entityTypeAttributeMapping.AttributeParentName.ToLowerInvariant().Equals(attributeParentName))
                    {
                        return entityTypeAttributeMapping;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Compare EntityTypeAttributeMappingCollection. 
        /// </summary>
        /// <param name="subsetEntityTypeAttributeMappingCollection">Expected EntityTypeAttributeMappingCollection.</param>
        /// <param name="compareIds">Compare Ids or not.</param>
        /// <returns>If actual EntityTypeAttributeMappingCollection is match then true else false</returns>
        public Boolean IsSuperSetOf(EntityTypeAttributeMappingCollection subsetEntityTypeAttributeMappingCollection, Boolean compareIds = false)
        {
            if (subsetEntityTypeAttributeMappingCollection != null && subsetEntityTypeAttributeMappingCollection.Count > 0)
            {
                foreach (EntityTypeAttributeMapping subsetEntityTypeAttributeMapping in subsetEntityTypeAttributeMappingCollection)
                {
                    EntityTypeAttributeMapping entityTypeAttributeMapping = this.Get(subsetEntityTypeAttributeMapping.EntityTypeName, subsetEntityTypeAttributeMapping.AttributeName, subsetEntityTypeAttributeMapping.AttributeParentName);

                    if (entityTypeAttributeMapping == null)
                    {
                        return false;
                    }
                    else if (!entityTypeAttributeMapping.IsSuperSetOf(subsetEntityTypeAttributeMapping, compareIds))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
       
        #region ICollection<EntityTypeAttributeMapping> Members

        /// <summary>
        /// Add EntityTypeAttributeMapping object in collection
        /// </summary>
        /// <param name="item">EntityTypeAttributeMapping to add in collection</param>
        public void Add(EntityTypeAttributeMapping item)
        {
            this._entityTypeAttributeMappings.Add(item);
        }

        /// <summary>
        /// Add EntityTypeAttributeMapping object in collection
        /// </summary>
        /// <param name="items">Indicates collection of entity type attribute mapping to add</param>
        public void AddRange(EntityTypeAttributeMappingCollection items)
        {
            if (items != null && items.Count > 0)
            {
                foreach (EntityTypeAttributeMapping item in items)
                {
                    this._entityTypeAttributeMappings.Add(item);
                }
            }
        }

        /// <summary>
        /// Removes all EntityTypeAttributeMappings from collection
        /// </summary>
        public void Clear()
        {
            this._entityTypeAttributeMappings.Clear();
        }

        /// <summary>
        /// Determines whether the EntityTypeAttributeMappingCollection contains a specific EntityTypeAttributeMapping.
        /// </summary>
        /// <param name="item">The EntityTypeAttributeMapping object to locate in the EntityTypeAttributeMappingCollection.</param>
        /// <returns>
        /// <para>true : If EntityTypeAttributeMapping found in mappingCollection</para>
        /// <para>false : If EntityTypeAttributeMapping found not in mappingCollection</para>
        /// </returns>
        public bool Contains(EntityTypeAttributeMapping item)
        {
            return this._entityTypeAttributeMappings.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the EntityTypeAttributeMappingCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from EntityTypeAttributeMappingCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(EntityTypeAttributeMapping[] array, int arrayIndex)
        {
            this._entityTypeAttributeMappings.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of EntityTypeAttributeMappings in EntityTypeAttributeMappingCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._entityTypeAttributeMappings.Count;
            }
        }

        /// <summary>
        /// Check if EntityTypeAttributeMappingCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the EntityTypeAttributeMappingCollection.
        /// </summary>
        /// <param name="item">The EntityTypeAttributeMapping object to remove from the EntityTypeAttributeMappingCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original EntityTypeAttributeMappingCollection</returns>
        public bool Remove(EntityTypeAttributeMapping item)
        {
            return this._entityTypeAttributeMappings.Remove(item);
        }

        #endregion

        #region IEnumerable<EntityTypeAttributeMapping> Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntityTypeAttributeMappingCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<EntityTypeAttributeMapping> GetEnumerator()
        {
            return this._entityTypeAttributeMappings.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntityTypeAttributeMappingCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._entityTypeAttributeMappings.GetEnumerator();
        }

        #endregion

        #region IEntityTypeAttributeMappingCollection Members

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of EntityTypeAttributeMappingCollection object
        /// </summary>
        /// <returns>Xml string representing the EntityTypeAttributeMappingCollection</returns>
        public String ToXml()
        {
            String entityTypeAttributeMappingsXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (EntityTypeAttributeMapping entityTypeAttributeMapping in this._entityTypeAttributeMappings)
            {
                builder.Append(entityTypeAttributeMapping.ToXML());
            }

            entityTypeAttributeMappingsXml = String.Format("<EntityTypeAttributeMappings>{0}</EntityTypeAttributeMappings>", builder.ToString());

            return entityTypeAttributeMappingsXml;
        }

        /// <summary>
        /// Get Xml representation of EntityTypeAttributeMappingCollection object
        /// </summary>
        /// <param name="objectSerialization"></param>
        /// <returns>Xml string representing the EntityTypeAttributeMappingCollection</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String entityTypeAttributeMappingsXml = String.Empty;

            switch (objectSerialization)
            {
                case ObjectSerialization.ProcessingOnly:
                    String xml = "<EntityTypeAttributes>";

                    StringBuilder stringBuilder = new StringBuilder(xml);

                    foreach (EntityTypeAttributeMapping mapping in this._entityTypeAttributeMappings)
                    {
                        stringBuilder.Append(mapping.ToXML());

                    }
                    stringBuilder.Append("</EntityTypeAttributes>");

                    entityTypeAttributeMappingsXml = stringBuilder.ToString();
                    break;
                default:
                    entityTypeAttributeMappingsXml = this.ToXml();
                    break;
            }

            return entityTypeAttributeMappingsXml;
        }

        #endregion ToXml methods

        #endregion IEntityTypeAttributeMappingCollection Memebers

        #region IDataModelObjectCollection

        /// <summary>
        /// Remove dataModelObject based on reference id.
        /// </summary>
        /// <param name="referenceIds">Indicates collection of referenceId of an entity type which is to be fetched.</param>
        /// <returns>true if item was successfully removed from the collection;otherwise, false. This method also returns false if item is not found in the original</returns>
        public bool Remove(Collection<string> referenceIds)
        {
            Boolean result = true;

            EntityTypeAttributeMappingCollection entityTypeAttributeMappings = Get(referenceIds);

            if (entityTypeAttributeMappings != null && entityTypeAttributeMappings.Count > 0)
            {
                foreach (EntityTypeAttributeMapping entityTypeAttributeMapping in entityTypeAttributeMappings)
                {
                    result = result && this.Remove(entityTypeAttributeMapping);
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
            Collection<IDataModelObjectCollection> entityTypeAttributeMappingsInBatch = null;

            if (this._entityTypeAttributeMappings != null)
            {
                entityTypeAttributeMappingsInBatch = Utility.Split(this, batchSize);
            }

            return entityTypeAttributeMappingsInBatch;
        }

        /// <summary>
        /// Adds item in current collection
        /// </summary>
        /// <param name="item">Indicates item to be added in collection</param>
        public void AddDataModelObject(IDataModelObject item)
        {
            this.Add(item as EntityTypeAttributeMapping);
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the EntityTypeAttributeMappingCollection for given referenceIds.
        /// </summary>
        /// <param name="referenceIds">referenceIds to be searched in the collection</param>
        /// <returns>EntityTypeAttributeMappingCollection having given referenceIds</returns>
        private EntityTypeAttributeMappingCollection Get(Collection<String> referenceIds)
        {
            EntityTypeAttributeMappingCollection entityTypeAttributeMappings = new EntityTypeAttributeMappingCollection();
            Int32 counter = 0;

            if (this._entityTypeAttributeMappings != null && this._entityTypeAttributeMappings.Count > 0 && referenceIds != null && referenceIds.Count > 0)
            {
                foreach (EntityTypeAttributeMapping entityTypeAttributeMapping in this._entityTypeAttributeMappings)
                {
                    if (referenceIds.Contains(entityTypeAttributeMapping.ReferenceId))
                    {
                        entityTypeAttributeMappings.Add(entityTypeAttributeMapping);
                        counter++;
                    }

                    if (referenceIds.Count.Equals(counter))
                        break;
                }
            }

            return entityTypeAttributeMappings;
        }

        /// <summary>
        /// Load current EntityTypeAttributeMappingCollection from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for current EntityTypeAttributeMappingCollection
        /// </param>
        private void LoadEntityTypeAttributeMappingCollection(String valuesAsXml)
        {
            /*
             * Sample XML:
                <EntityTypeAttributeMappings>
	                <EntityTypeAttribute Id="-1" EntityTypeId="29" AttributeId = "4079" SortOrder = "0" FK_InheritanceMode = "2" Required = "N" ReadOnly = "False" ShowAtCreation = "False" Extension = "" Action="Create" />
	                <EntityTypeAttribute Id="-1" EntityTypeId="29" AttributeId = "4080" SortOrder = "0" FK_InheritanceMode = "2" Required = "N" ReadOnly = "False" ShowAtCreation = "False" Extension = "" Action="Create" />
                </EntityTypeAttributeMappings>
            */
            XmlTextReader reader = null;

            if (!String.IsNullOrEmpty(valuesAsXml))
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityTypeAttribute")
                    {
                        String entityTypeAttributeMappingXml = reader.ReadOuterXml();
                        if (!String.IsNullOrEmpty(entityTypeAttributeMappingXml))
                        {
                            EntityTypeAttributeMapping entityTypeAttributeMapping = new EntityTypeAttributeMapping(entityTypeAttributeMappingXml);
                            if (entityTypeAttributeMapping != null)
                            {
                                this.Add(entityTypeAttributeMapping);
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
