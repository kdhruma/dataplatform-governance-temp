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
    /// Specifies the Container Relationship Entity Type Attribute Mapping Instance Collection for the Object
    /// </summary>
    [DataContract]
    public class RelationshipTypeAttributeMappingCollection : ICollection<RelationshipTypeAttributeMapping>, IEnumerable<RelationshipTypeAttributeMapping>, IRelationshipTypeAttributeMappingCollection, IDataModelObjectCollection
    {
        #region Fields

        [DataMember]
        private Collection<RelationshipTypeAttributeMapping> _relationshipTypeAttributeMappings = new Collection<RelationshipTypeAttributeMapping>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter-less constructor
        /// </summary>
        public RelationshipTypeAttributeMappingCollection() : base() { }

        /// <summary>
        /// Initialize RelationshipTypeAttributeMappingCollection from IList
        /// </summary>
        /// <param name="RelationshipTypeAttributeMappingList">IList of RelationshipTypeAttributeMappings</param>
        public RelationshipTypeAttributeMappingCollection(IList<RelationshipTypeAttributeMapping> RelationshipTypeAttributeMappingList)
        {
            this._relationshipTypeAttributeMappings = new Collection<RelationshipTypeAttributeMapping>(RelationshipTypeAttributeMappingList);
        }

        /// <summary>
        /// Initializes a new instance of the RelationshipTypeAttributeMappingCollection class with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        public RelationshipTypeAttributeMappingCollection(String valuesAsXml)
        {
            LoadRelationshipTypeAttributeMappingCollection(valuesAsXml);
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
                return ObjectType.RelationshipTypeAttributeMapping;
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
        public override bool Equals(object obj)
        {
            if (obj is RelationshipTypeAttributeMappingCollection)
            {
                RelationshipTypeAttributeMappingCollection objectToBeCompared = obj as RelationshipTypeAttributeMappingCollection;
                Int32 relationshipTypeAttributeMappingsUnion = this._relationshipTypeAttributeMappings.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 relationshipTypeAttributeMappingsIntersect = this._relationshipTypeAttributeMappings.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if (relationshipTypeAttributeMappingsUnion != relationshipTypeAttributeMappingsIntersect)
                    return false;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            Int32 hashCode = 0;
            foreach (RelationshipTypeAttributeMapping relationshipTypeAttributeMapping in this._relationshipTypeAttributeMappings)
            {
                hashCode += relationshipTypeAttributeMapping.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// Gets the RelationshipTypeAttributeMapping for given RelationshipTypeName, AttributeName and AttributeParentName
        /// </summary>
        /// <param name="relationshipTypeName">RelationshipTypeName to be searched in the collection</param>
        /// <param name="attributeName">AttributeName to be searched in the collection</param>
        /// <param name="attributeParentName">attributeParentName to be searched in the collection</param>
        /// <returns>RelationshipTypeAttributeMapping having given RelationshipTypeName, AttributeName and AttributeParentName</returns>
        public RelationshipTypeAttributeMapping Get(String relationshipTypeName, String attributeName, String attributeParentName)
        {
            if (this._relationshipTypeAttributeMappings != null && this._relationshipTypeAttributeMappings.Count > 0)
            {
                relationshipTypeName = relationshipTypeName.ToLowerInvariant();
                attributeName = attributeName.ToLowerInvariant();
                attributeParentName = attributeParentName.ToLowerInvariant();

                foreach (RelationshipTypeAttributeMapping relationshipTypeAttributeMapping in this._relationshipTypeAttributeMappings)
                {
                    if (relationshipTypeAttributeMapping.RelationshipTypeName.ToLowerInvariant().Equals(relationshipTypeName) &&
                        relationshipTypeAttributeMapping.AttributeName.ToLowerInvariant().Equals(attributeName) &&
                        relationshipTypeAttributeMapping.AttributeParentName.ToLowerInvariant().Equals(attributeParentName))
                    {
                        return relationshipTypeAttributeMapping;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Compare RelationshipTypeAttributeMappingCollection. 
        /// </summary>
        /// <param name="subsetRelationshipTypeAttributeMappingCollection">Expected RelationshipTypeAttributeMappingCollection.</param>
        /// <param name="compareIds">Compare Ids or not.</param>
        /// <returns>If actual RelationshipTypeAttributeMappingCollection is match then true else false</returns>
        public Boolean IsSuperSetOf(RelationshipTypeAttributeMappingCollection subsetRelationshipTypeAttributeMappingCollection, Boolean compareIds = false)
        {
            if (subsetRelationshipTypeAttributeMappingCollection != null && subsetRelationshipTypeAttributeMappingCollection.Count > 0)
            {
                foreach (RelationshipTypeAttributeMapping subsetRelationshipTypeAttributeMapping in subsetRelationshipTypeAttributeMappingCollection)
                {
                    RelationshipTypeAttributeMapping relationshipTypeAttributeMapping = this.Get(subsetRelationshipTypeAttributeMapping.RelationshipTypeName, subsetRelationshipTypeAttributeMapping.AttributeName, subsetRelationshipTypeAttributeMapping.AttributeParentName);

                    if (relationshipTypeAttributeMapping == null)
                    {
                        return false;
                    }
                    else if (!relationshipTypeAttributeMapping.IsSuperSetOf(subsetRelationshipTypeAttributeMapping, compareIds))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        #region ICollection<RelationshipTypeAttributeMapping> Members

        /// <summary>
        /// Add RelationshipTypeAttributeMapping object in collection
        /// </summary>
        /// <param name="item">RelationshipTypeAttributeMapping to add in collection</param>
        public void Add(RelationshipTypeAttributeMapping item)
        {
            this._relationshipTypeAttributeMappings.Add(item);
        }

        /// <summary>
        /// Add RelationshipTypeAttributeMapping object in collection
        /// </summary>
        /// <param name="items">Indicates collection of relationship type attribute mapping to add</param>
        public void AddRange(RelationshipTypeAttributeMappingCollection items)
        {
            if (items != null && items.Count > 0)
            {
                foreach (RelationshipTypeAttributeMapping item in items)
                {
                    this._relationshipTypeAttributeMappings.Add(item);
                }
            }
        }

        /// <summary>
        /// Removes all RelationshipTypeAttributeMappings from collection
        /// </summary>
        public void Clear()
        {
            this._relationshipTypeAttributeMappings.Clear();
        }

        /// <summary>
        /// Determines whether the RelationshipTypeAttributeMappingCollection contains a specific RelationshipTypeAttributeMapping.
        /// </summary>
        /// <param name="item">The RelationshipTypeAttributeMapping object to locate in the RelationshipTypeAttributeMappingCollection.</param>
        /// <returns>
        /// <para>true : If RelationshipTypeAttributeMapping found in mappingCollection</para>
        /// <para>false : If RelationshipTypeAttributeMapping found not in mappingCollection</para>
        /// </returns>
        public bool Contains(RelationshipTypeAttributeMapping item)
        {
            return this._relationshipTypeAttributeMappings.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the RelationshipTypeAttributeMappingCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from RelationshipTypeAttributeMappingCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(RelationshipTypeAttributeMapping[] array, int arrayIndex)
        {
            this._relationshipTypeAttributeMappings.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of RelationshipTypeAttributeMappings in RelationshipTypeAttributeMappingCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._relationshipTypeAttributeMappings.Count;
            }
        }

        /// <summary>
        /// Check if RelationshipTypeAttributeMappingCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the RelationshipTypeAttributeMappingCollection.
        /// </summary>
        /// <param name="item">The RelationshipTypeAttributeMapping object to remove from the RelationshipTypeAttributeMappingCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original RelationshipTypeAttributeMappingCollection</returns>
        public bool Remove(RelationshipTypeAttributeMapping item)
        {
            return this._relationshipTypeAttributeMappings.Remove(item);
        }

        #endregion

        #region IEnumerable<RelationshipTypeAttributeMapping> Members

        /// <summary>
        /// Returns an enumerator that iterates through a RelationshipTypeAttributeMappingCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<RelationshipTypeAttributeMapping> GetEnumerator()
        {
            return this._relationshipTypeAttributeMappings.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a RelationshipTypeAttributeMappingCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._relationshipTypeAttributeMappings.GetEnumerator();
        }

        #endregion

        #region IRelationshipTypeAttributeMappingCollection Members

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of RelationshipTypeAttributeMappingCollection object
        /// </summary>
        /// <returns>Xml string representing the RelationshipTypeAttributeMappingCollection</returns>
        public String ToXml()
        {
            String relationshipTypeAttributeMappingsXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (RelationshipTypeAttributeMapping relationshipTypeAttributeMapping in this._relationshipTypeAttributeMappings)
            {
                builder.Append(relationshipTypeAttributeMapping.ToXML());
            }

            relationshipTypeAttributeMappingsXml = String.Format("<RelationshipTypeAttributeMappings>{0}</RelationshipTypeAttributeMappings>", builder.ToString());

            return relationshipTypeAttributeMappingsXml;
        }

        /// <summary>
        /// Get Xml representation of RelationshipTypeAttributeMappingCollection object
        /// </summary>
        /// <param name="objectSerialization"></param>
        /// <returns>Xml string representing the RelationshipTypeAttributeMappingCollection</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String relationshipTypeAttributeMappingsXml = String.Empty;

            switch (objectSerialization)
            {
                case ObjectSerialization.ProcessingOnly:
                    StringBuilder builder = new StringBuilder();

                    foreach (RelationshipTypeAttributeMapping relationshipTypeAttributeMapping in this._relationshipTypeAttributeMappings)
                    {
                        builder.Append(relationshipTypeAttributeMapping.ToXML());
                    }

                    relationshipTypeAttributeMappingsXml = String.Format("<RelationshipTypeAttributeMappings>{0}</RelationshipTypeAttributeMappings>", builder.ToString());
                    break;
                default:
                    relationshipTypeAttributeMappingsXml = this.ToXml();
                    break;
            }

            return relationshipTypeAttributeMappingsXml;
        }

        #endregion ToXml methods

        #endregion IRelationshipTypeAttributeMappingCollection Memebers
       
        #region IDataModelObjectCollection
        /// <summary>
        /// Remove dataModelObject based on reference id.
        /// </summary>
        /// <param name="referenceIds">Indicates collection of referenceId of an RelationshipTypeAttributeMapping which is to be fetched.</param>
        /// <returns>true if item was successfully removed from the collection;otherwise, false. This method also returns false if item is not found in the original</returns>
        public bool Remove(Collection<string> referenceIds)
        {
            Boolean result = true;

            RelationshipTypeAttributeMappingCollection relationshipTypeAttributeMappings = Get(referenceIds);

            if (relationshipTypeAttributeMappings != null && relationshipTypeAttributeMappings.Count > 0)
            {
                foreach (RelationshipTypeAttributeMapping relationshipTypeAttributeMapping in relationshipTypeAttributeMappings)
                {
                    result = result && this.Remove(relationshipTypeAttributeMapping);
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
            Collection<IDataModelObjectCollection> relationshipTypeAttributeMappingsInBatch = null;

            if (this._relationshipTypeAttributeMappings != null)
            {
                relationshipTypeAttributeMappingsInBatch = Utility.Split(this, batchSize);
            }

            return relationshipTypeAttributeMappingsInBatch;
        }

        /// <summary>
        /// Adds item in current collection
        /// </summary>
        /// <param name="item">Indicates item to be added in collection</param>
        public void AddDataModelObject(IDataModelObject item)
        {
            this.Add(item as RelationshipTypeAttributeMapping);
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the RelationshipTypeAttributeMappingCollection for given referenceIds.
        /// </summary>
        /// <param name="referenceIds">referenceIds to be searched in the collection</param>
        /// <returns>RelationshipTypeAttributeMappingCollection having given referenceIds</returns>
        private RelationshipTypeAttributeMappingCollection Get(Collection<String> referenceIds)
        {
            RelationshipTypeAttributeMappingCollection relationshipTypeAttributeMappings = new RelationshipTypeAttributeMappingCollection();
            Int32 counter = 0;

            if (this._relationshipTypeAttributeMappings != null && this._relationshipTypeAttributeMappings.Count > 0 && referenceIds != null && referenceIds.Count > 0)
            {
                foreach (RelationshipTypeAttributeMapping relationshipTypeAttributeMapping in this._relationshipTypeAttributeMappings)
                {
                    if (referenceIds.Contains(relationshipTypeAttributeMapping.ReferenceId))
                    {
                        relationshipTypeAttributeMappings.Add(relationshipTypeAttributeMapping);
                        counter++;
                    }

                    if (referenceIds.Count.Equals(counter))
                        break;
                }
            }
            return relationshipTypeAttributeMappings;
        }

        /// <summary>
        /// Load current RelationshipTypeAttributeMappingCollection from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for current RelationshipTypeAttributeMappingCollection
        /// </param>
        private void LoadRelationshipTypeAttributeMappingCollection(String valuesAsXml)
        {
            /*
             * Sample XML:
                <RelationshipTypeAttributeMappings>
	                <RelationshipTypeAttribute Id="-1" RelationshipTypeId = "1" AttributeId = "4393" FK_InheritanceMode = "2" ShowAtCreation = "False" Required = "N" ReadOnly = "False"  SortOrder = "0" ShowInline = "False" Action="Create" />
	                <RelationshipTypeAttribute Id="-1" RelationshipTypeId = "1" AttributeId = "4394" FK_InheritanceMode = "2" ShowAtCreation = "False" Required = "N" ReadOnly = "False"  SortOrder = "0" ShowInline = "False" Action="Create" />
                </RelationshipTypeAttributeMappings>
            */
            XmlTextReader reader = null;

            if (!String.IsNullOrEmpty(valuesAsXml))
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "RelationshipTypeAttribute")
                    {
                        String relationshipTypeAttributeMappingXml = reader.ReadOuterXml();
                        if (!String.IsNullOrEmpty(relationshipTypeAttributeMappingXml))
                        {
                            RelationshipTypeAttributeMapping relationshipTypeAttributeMapping = new RelationshipTypeAttributeMapping(relationshipTypeAttributeMappingXml);
                            if (relationshipTypeAttributeMapping != null)
                            {
                                this.Add(relationshipTypeAttributeMapping);
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