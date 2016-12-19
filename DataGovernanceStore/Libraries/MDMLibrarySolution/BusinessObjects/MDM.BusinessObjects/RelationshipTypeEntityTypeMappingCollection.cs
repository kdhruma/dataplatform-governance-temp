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
    /// Specifies the mapping collection object of RelationshipType EntityType mapping
    /// </summary>
    [DataContract]
    public class RelationshipTypeEntityTypeMappingCollection : ICollection<RelationshipTypeEntityTypeMapping>, IEnumerable<RelationshipTypeEntityTypeMapping>, IRelationshipTypeEntityTypeMappingCollection, IDataModelObjectCollection
    {
        #region Fields

        [DataMember]
        private Collection<RelationshipTypeEntityTypeMapping> _relationshipTypeEntityTypeMappings = new Collection<RelationshipTypeEntityTypeMapping>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter-less constructor
        /// </summary>
        public RelationshipTypeEntityTypeMappingCollection() : base() { }

        /// <summary>
        /// Initialize RelationshipTypeEntityTypeMappingCollection from IList
        /// </summary>
        /// <param name="RelationshipTypeEntityTypeMappingList">IList of RelationshipTypeEntityTypeMappings</param>
        public RelationshipTypeEntityTypeMappingCollection(IList<RelationshipTypeEntityTypeMapping> RelationshipTypeEntityTypeMappingList)
        {
            this._relationshipTypeEntityTypeMappings = new Collection<RelationshipTypeEntityTypeMapping>(RelationshipTypeEntityTypeMappingList);
        }

        /// <summary>
        /// Initialize RelationshipTypeEntityTypeMappingCollection from Xml
        /// </summary>
        /// <param name="valuesAsXml">Indicates xml having value for current relationsip type entity type mapping collection</param>
        public RelationshipTypeEntityTypeMappingCollection(String valuesAsXml)
        {
            LoadRelationshipTypeEntityTypeMappingCollection(valuesAsXml);
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
                return ObjectType.RelationshipTypeEntityTypeMapping;
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
            if (obj is RelationshipTypeEntityTypeMappingCollection)
            {
                RelationshipTypeEntityTypeMappingCollection objectToBeCompared = obj as RelationshipTypeEntityTypeMappingCollection;
                Int32 RelationshipTypeEntityTypeMappingsUnion = this._relationshipTypeEntityTypeMappings.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 RelationshipTypeEntityTypeMappingsIntersect = this._relationshipTypeEntityTypeMappings.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if (RelationshipTypeEntityTypeMappingsUnion != RelationshipTypeEntityTypeMappingsIntersect)
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
            foreach (RelationshipTypeEntityTypeMapping RelationshipTypeEntityTypeMapping in this._relationshipTypeEntityTypeMappings)
            {
                hashCode += RelationshipTypeEntityTypeMapping.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// Gets the RelationshipTypeEntityTypeMapping for given ContainerName and EntityTypeName.
        /// </summary>
        /// <param name="entityTypeName">EntityTypeName to be searched in the collection</param>
        /// <param name="relationshipTypeName">RelationshipTypeName to be searched in the collection</param>
        /// <returns>RelationshipTypeEntityTypeMapping having given RelationshipTypeName and EntityTypeName</returns>
        public RelationshipTypeEntityTypeMapping Get(String entityTypeName, String relationshipTypeName)
        {
            entityTypeName = entityTypeName.ToLowerInvariant();
            relationshipTypeName = relationshipTypeName.ToLowerInvariant();

            if (this._relationshipTypeEntityTypeMappings != null && this._relationshipTypeEntityTypeMappings.Count > 0)
            {
                foreach (RelationshipTypeEntityTypeMapping relationshipTypeEntityTypeMapping in this._relationshipTypeEntityTypeMappings)
                {
                    if (relationshipTypeEntityTypeMapping.EntityTypeName.ToLowerInvariant().Equals(entityTypeName) && 
                        relationshipTypeEntityTypeMapping.RelationshipTypeName.ToLowerInvariant().Equals(relationshipTypeName))
                    {
                        return relationshipTypeEntityTypeMapping;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the RelationshipTypeEntityTypeMapping for given EntityTypeId and RelatinshipTypeId.
        /// </summary>
        /// <param name="entityTypeId">EntityTypeId to be searched in the collection</param>
        /// <param name="relationshipTypeId">RelatinshipTypeId to be searched in the collection</param>
        /// <returns>RelationshipTypeEntityTypeMapping having given RelatinshipTypeId and EntityTypeId</returns>
        public RelationshipTypeEntityTypeMapping Get(Int32 entityTypeId, Int32 relationshipTypeId)
        {
            if (this._relationshipTypeEntityTypeMappings != null && this._relationshipTypeEntityTypeMappings.Count > 0)
            {
                foreach (RelationshipTypeEntityTypeMapping relationshipTypeEntityTypeMapping in this._relationshipTypeEntityTypeMappings)
                {
                    if (relationshipTypeEntityTypeMapping.EntityTypeId == entityTypeId &&
                        relationshipTypeEntityTypeMapping.RelationshipTypeId == relationshipTypeId)
                    {
                        return relationshipTypeEntityTypeMapping;
                    }
                }
            }

            return null;
        }

        #region ICollection<RelationshipTypeEntityTypeMapping> Members

        /// <summary>
        /// Add RelationshipTypeEntityTypeMapping object in collection
        /// </summary>
        /// <param name="item">RelationshipTypeEntityTypeMapping to add in collection</param>
        public void Add(RelationshipTypeEntityTypeMapping item)
        {
            this._relationshipTypeEntityTypeMappings.Add(item);
        }

        /// <summary>
        /// Add RelationshipTypeEntityTypeMapping object in collection
        /// </summary>
        /// <param name="items">RelationshipTypeEntityTypeMapping to add in collection</param>
        public void AddRange(RelationshipTypeEntityTypeMappingCollection items)
        {
            if (items != null && items.Count > 0)
            {
                foreach (RelationshipTypeEntityTypeMapping item in items)
                {
                    this._relationshipTypeEntityTypeMappings.Add(item);
                }
            }
        }

        /// <summary>
        /// Add RelationshipTypeEntityTypeMapping in collection
        /// </summary>
        /// <param name="iRelationshipTypeEntityTypeMapping">RelationshipTypeEntityTypeMapping to add in collection</param>
        public void Add(IRelationshipTypeEntityTypeMapping iRelationshipTypeEntityTypeMapping)
        {
            if (iRelationshipTypeEntityTypeMapping != null)
            {
                this.Add((RelationshipTypeEntityTypeMapping)iRelationshipTypeEntityTypeMapping);
            }
        }

        /// <summary>
        /// Removes all RelationshipTypeEntityTypeMappings from collection
        /// </summary>
        public void Clear()
        {
            this._relationshipTypeEntityTypeMappings.Clear();
        }

        /// <summary>
        /// Determines whether the RelationshipTypeEntityTypeMappingCollection contains a specific RelationshipTypeEntityTypeMapping.
        /// </summary>
        /// <param name="item">The RelationshipTypeEntityTypeMapping object to locate in the RelationshipTypeEntityTypeMappingCollection.</param>
        /// <returns>
        /// <para>true : If RelationshipTypeEntityTypeMapping found in mappingCollection</para>
        /// <para>false : If RelationshipTypeEntityTypeMapping found not in mappingCollection</para>
        /// </returns>
        public bool Contains(RelationshipTypeEntityTypeMapping item)
        {
            return this._relationshipTypeEntityTypeMappings.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the RelationshipTypeEntityTypeMappingCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from RelationshipTypeEntityTypeMappingCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(RelationshipTypeEntityTypeMapping[] array, int arrayIndex)
        {
            this._relationshipTypeEntityTypeMappings.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of RelationshipTypeEntityTypeMappings in RelationshipTypeEntityTypeMappingCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._relationshipTypeEntityTypeMappings.Count;
            }
        }

        /// <summary>
        /// Check if RelationshipTypeEntityTypeMappingCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the RelationshipTypeEntityTypeMappingCollection.
        /// </summary>
        /// <param name="item">The RelationshipTypeEntityTypeMapping object to remove from the RelationshipTypeEntityTypeMappingCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original RelationshipTypeEntityTypeMappingCollection</returns>
        public bool Remove(RelationshipTypeEntityTypeMapping item)
        {
            return this._relationshipTypeEntityTypeMappings.Remove(item);
        }

        #endregion

        #region IEnumerable<RelationshipTypeEntityTypeMapping> Members

        /// <summary>
        /// Returns an enumerator that iterates through a RelationshipTypeEntityTypeMappingCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<RelationshipTypeEntityTypeMapping> GetEnumerator()
        {
            return this._relationshipTypeEntityTypeMappings.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a RelationshipTypeEntityTypeMappingCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._relationshipTypeEntityTypeMappings.GetEnumerator();
        }

        #endregion

        #region IRelationshipTypeEntityTypeMappingCollection Members

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of RelationshipTypeEntityTypeMappingCollection object
        /// </summary>
        /// <returns>Xml string representing the RelationshipTypeEntityTypeMappingCollection</returns>
        public String ToXml()
        {
            String relationshipTypeEntityTypeMappingsXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (RelationshipTypeEntityTypeMapping relationshipTypeEntityTypeMapping in this._relationshipTypeEntityTypeMappings)
            {
                builder.Append(relationshipTypeEntityTypeMapping.ToXML());
            }

            relationshipTypeEntityTypeMappingsXml = String.Format("<RelationshipTypeEntityTypeMappings>{0}</RelationshipTypeEntityTypeMappings>", builder.ToString());

            return relationshipTypeEntityTypeMappingsXml;
        }

        #endregion ToXml methods

        #endregion IRelationshipTypeEntityTypeMappingCollection Members

        #region IDataModelObjectCollection
        /// <summary>
        /// Remove dataModelObject based on reference id.
        /// </summary>
        /// <param name="referenceIds">Indicates collection of referenceId of an entity type which is to be fetched.</param>
        /// <returns>true if item was successfully removed from the collection;otherwise, false. This method also returns false if item is not found in the original</returns>
        public bool Remove(Collection<string> referenceIds)
        {
            Boolean result = true;

            RelationshipTypeEntityTypeMappingCollection relationshipTypeEntityTypeMappings = Get(referenceIds);

            if (relationshipTypeEntityTypeMappings != null && relationshipTypeEntityTypeMappings.Count > 0)
            {
                foreach (RelationshipTypeEntityTypeMapping relationshipTypeEntityTypeMapping in relationshipTypeEntityTypeMappings)
                {
                    result = result && this.Remove(relationshipTypeEntityTypeMapping);
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
            Collection<IDataModelObjectCollection> relationshipTypeEntityTypeMappingsInBatch = null;

            if (this._relationshipTypeEntityTypeMappings != null)
            {
               relationshipTypeEntityTypeMappingsInBatch = Utility.Split(this, batchSize);
            }

            return relationshipTypeEntityTypeMappingsInBatch;
        }

        /// <summary>
        /// Adds item in current collection
        /// </summary>
        /// <param name="item">Indicates item to be added in collection</param>
        public void AddDataModelObject(IDataModelObject item)
        {
            this.Add(item as RelationshipTypeEntityTypeMapping);
        }

        #endregion
        
        /// <summary>
        /// Checks whether expected output is a subset of actual output or not.
        /// </summary>
        /// <param name="subsetRelationshipTypeEntityTypeMappings">Indicates expected output result which is compared against actual output result.</param>
        /// <param name="compareIds">Indicates boolean value whether to compare ids or not.</param>
        public Boolean IsSuperSetOf(RelationshipTypeEntityTypeMappingCollection subsetRelationshipTypeEntityTypeMappings, Boolean compareIds = false)
        {
            if (subsetRelationshipTypeEntityTypeMappings != null && subsetRelationshipTypeEntityTypeMappings.Count > 0)
            {
                foreach (RelationshipTypeEntityTypeMapping subsetRelationshipTypeEntityTypeMapping in subsetRelationshipTypeEntityTypeMappings)
                {
                    RelationshipTypeEntityTypeMapping relationshipTypeEntityTypeMapping = this.Get(subsetRelationshipTypeEntityTypeMapping.EntityTypeName, subsetRelationshipTypeEntityTypeMapping.RelationshipTypeName);

                    if (relationshipTypeEntityTypeMapping == null)
                    {
                        return false;
                    }
                    else if (!relationshipTypeEntityTypeMapping.IsSuperSetOf(subsetRelationshipTypeEntityTypeMapping, compareIds))
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
        /// Gets the RelationshipTypeEntityTypeMappingCollection for given referenceIds.
        /// </summary>
        /// <param name="referenceIds">referenceIds to be searched in the collection</param>
        /// <returns>RelationshipTypeEntityTypeMappingCollection having given referenceIds</returns>
        private RelationshipTypeEntityTypeMappingCollection Get(Collection<String> referenceIds)
        {
            RelationshipTypeEntityTypeMappingCollection relationshipTypeEntityTypeMappings = new RelationshipTypeEntityTypeMappingCollection();
            Int32 counter = 0;

            if (this._relationshipTypeEntityTypeMappings != null && this._relationshipTypeEntityTypeMappings.Count > 0 && referenceIds != null && referenceIds.Count > 0)
            {
                foreach (RelationshipTypeEntityTypeMapping relationshipTypeEntityTypeMapping in this._relationshipTypeEntityTypeMappings)
                {
                    if (referenceIds.Contains(relationshipTypeEntityTypeMapping.ReferenceId))
                    {
                        relationshipTypeEntityTypeMappings.Add(relationshipTypeEntityTypeMapping);
                        counter++;
                    }

                    if (referenceIds.Count.Equals(counter))
                        break;
                }
            }
            return relationshipTypeEntityTypeMappings;
        }

        /// <summary>
        /// Load current relationsip type entity type mapping collection from Xml
        /// </summary>
        /// <param name="valuesAsXml">
        /// Indicates xml having value for current relationsip type entity type mapping collection
        /// </param>
        private void LoadRelationshipTypeEntityTypeMappingCollection(String valuesAsXml)
        {
            #region Sample Xml

            #endregion

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "RelationshipTypeEntityType")
                        {
                            String RelationshipTypeEntityTypeXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(RelationshipTypeEntityTypeXml))
                            {
                                RelationshipTypeEntityTypeMapping relationshipTypeEntityTypeMapping = new RelationshipTypeEntityTypeMapping(RelationshipTypeEntityTypeXml);
                                if (relationshipTypeEntityTypeMapping != null)
                                {
                                    this.Add(relationshipTypeEntityTypeMapping);
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

        #endregion

        #endregion
    }
}
