using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Xml;
using System.Collections;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;
    using MDM.Core;

    /// <summary>
    /// Object for Relationship Cardinality Collection
    /// </summary>
    [DataContract]
    public class RelationshipCardinalityCollection : ICollection<RelationshipCardinality>, IEnumerable<RelationshipCardinality>, IRelationshipCardinalityCollection, IDataModelObjectCollection
    {
        #region Fields

        /// <summary>
        /// Field denoting collection of relationship cardinality
        /// </summary>
        [DataMember]
        Collection<RelationshipCardinality> _relationshipCardinalities = new Collection<RelationshipCardinality>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the RelationshipCardinality Collection
        /// </summary>
        public RelationshipCardinalityCollection() { }

        /// <summary>
        /// Initialize subscriber from Xml value
        /// </summary>
        /// <param name="valuesAsXml">Relationship Cardinality in xml format</param>
        public RelationshipCardinalityCollection(String valuesAsXml)
        {
            LoadRelationshipCardinalities(valuesAsXml);
        }

        /// <summary>
        /// Initializes a new instance from List of relationshipCardinality
        /// </summary>
        /// <param name="relationshipCardinalityList">List of RelationshipCardinalities</param>
        public RelationshipCardinalityCollection(IList<RelationshipCardinality> relationshipCardinalityList)
        {
            if (relationshipCardinalityList != null)
            {
                this._relationshipCardinalities = new Collection<RelationshipCardinality>(relationshipCardinalityList);
            }
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
                return ObjectType.ContainerRelationshipTypeEntityTypeMappingCardinality;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Check if RelationshipCardinalityCollection contains RelationshipCardinality with given Id
        /// </summary>
        /// <param name="Id">Id using which RelationshipCardinality is to be searched from collection</param>
        /// <returns>
        /// <para>true : If RelationshipCardinality found in RelationshipCardinalityCollection</para>
        /// <para>false : If RelationshipCardinality found not in RelationshipCardinalityCollection</para>
        /// </returns>
        public bool Contains(Int32 Id)
        {
            if (GetRelationshipCardinality(Id) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Remove RelationshipCardinality object from RelationshipCardinalityCollection
        /// </summary>
        /// <param name="relationshipCardinalityId">RelationshipCardinalityId of RelationshipCardinality which is to be removed from collection</param>
        /// <returns>true if relationshipCardinality is successfully removed; otherwise, false. This method also returns false if RelationshipCardinality was not found in the original collection</returns>
        public bool Remove(Int32 relationshipCardinalityId)
        {
            IRelationshipCardinality relationshipCardinality = GetRelationshipCardinality(relationshipCardinalityId);

            if (relationshipCardinality == null)
                throw new ArgumentException("No RelationshipCardinality found for given Id :" + relationshipCardinalityId);
            else
                return this.Remove(relationshipCardinalityId);
        }

        /// <summary>
        /// Get relationship type based on given relationship cardinality id
        /// </summary>
        /// <param name="Id">Id of relationship cardinality to search on</param>
        /// <returns>Relationship Cardinality with given id.</returns>
        public IRelationshipCardinality GetRelationshipCardinality(Int32 Id)
        {
            var filteredRelationshipCardinality = from relationshipCardinality in this._relationshipCardinalities
                                           where relationshipCardinality.Id == Id
                                           select relationshipCardinality;

            if (filteredRelationshipCardinality.Any())
                return filteredRelationshipCardinality.First();
            else
                return null;
        }

        /// <summary>
        /// Get relationship type based on given fromEntity type id
        /// </summary>
        /// <param name="fromEntityTypeId">Id of Entity Type to search on</param>
        /// <returns>RelationshipCardinality for given entityType Id</returns>
        public IRelationshipCardinalityCollection GetRelationshipCardinalityByEntityTypeId(Int32 fromEntityTypeId)
        {
            RelationshipCardinalityCollection iRelationshipCardinalityCollection = new RelationshipCardinalityCollection((from relCardinality in this._relationshipCardinalities
                                                                                                                          where relCardinality.FromEntityTypeId == fromEntityTypeId
                                                                                                                          select relCardinality).ToList<RelationshipCardinality>());

            return (IRelationshipCardinalityCollection)iRelationshipCardinalityCollection;
        }

        /// <summary>
        /// Get relationship type based on given relationship type id
        /// </summary>
        /// <param name="relationshipTypeId">Id of relationship Type search on</param>
        /// <returns>Relationship Cardinality</returns>
        public IRelationshipCardinalityCollection GetRelationshipCardinalityByRelationshipTypeId(Int32 relationshipTypeId)
        {
            RelationshipCardinalityCollection iRelationshipCardinalityCollection = new RelationshipCardinalityCollection((from relCardinality in this._relationshipCardinalities
                                                                                                                          where relCardinality.RelationshipTypeId == relationshipTypeId
                                                                                                                          select relCardinality).ToList<RelationshipCardinality>());

            return (IRelationshipCardinalityCollection)iRelationshipCardinalityCollection;
        }

        /// <summary>
        /// Get Xml representation of RelationshipCardinalityCollection
        /// </summary>
        /// <returns>Xml representation of RelationshipCardinalityCollection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            returnXml = "<RelationshipCardinalities>";

            if (this._relationshipCardinalities != null && this._relationshipCardinalities.Count > 0)
            {
                foreach (RelationshipCardinality relationshipCardinality in this._relationshipCardinalities)
                {
                    returnXml = String.Concat(returnXml, relationshipCardinality.ToXml());
                }
            }

            returnXml = String.Concat(returnXml, "</RelationshipCardinalities>");

            return returnXml;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is RelationshipCardinalityCollection)
            {
                RelationshipCardinalityCollection objectToBeCompared = obj as RelationshipCardinalityCollection;

                Int32 relationshipCardinalitiesUnion = this._relationshipCardinalities.ToList().Union(objectToBeCompared._relationshipCardinalities.ToList()).Count();
                Int32 relationshipCardinalitiesIntersect = this._relationshipCardinalities.ToList().Intersect(objectToBeCompared._relationshipCardinalities.ToList()).Count();

                if (relationshipCardinalitiesUnion != relationshipCardinalitiesIntersect)
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

            foreach (RelationshipCardinality RelationshipCardinality in this._relationshipCardinalities)
            {
                hashCode += RelationshipCardinality.GetHashCode();
            }

            return hashCode;
        }

        /// <summary>
        /// Returns total number of insatnce present in current collection
        /// </summary>
        public Int32 Count
        {
            get {  return this._relationshipCardinalities.Count; }
        }

        /// <summary>
        /// Add item into the Collection
        /// </summary>
        /// <param name="item">Item to be added</param>
        public void Add(RelationshipCardinality item)
        {
            this._relationshipCardinalities.Add(item);
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="collection">The collection.</param>
        public IRelationshipCardinalityCollection AddRange(IRelationshipCardinalityCollection collection)
        {
            foreach (RelationshipCardinality relationshipCardinality in collection)
            {
                if (!this._relationshipCardinalities.Contains(relationshipCardinality))
                {
                    this._relationshipCardinalities.Add(relationshipCardinality);
                }
            }
            return this;
        }

        #region IEnumerable<RelationshipCardinality> Members

        /// <summary>
        /// eturns an enumerator that iterates through a RelationshipCardinalityCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection</returns>
        public IEnumerator<RelationshipCardinality> GetEnumerator()
        {
            return this._relationshipCardinalities.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a RelationshipCardinalityCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._relationshipCardinalities.GetEnumerator();
        }

        #endregion

        #region Collection<RelationshipCardinality> Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void Add(IRelationshipCardinality item)
        {
            this._relationshipCardinalities.Add((RelationshipCardinality)item);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            this._relationshipCardinalities.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(RelationshipCardinality item)
        {
            return this._relationshipCardinalities.Contains(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(RelationshipCardinality[] array, int arrayIndex)
        {
            this._relationshipCardinalities.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(RelationshipCardinality item)
        {
            return this._relationshipCardinalities.Remove(item);
        }

        #endregion

        #region IDataModelObjectCollection Members

        /// <summary>
        /// Remove dataModelObject based on reference id.
        /// </summary>
        /// <param name="referenceIds">Indicates collection of referenceId of an RelationshipCardinality which is to be fetched.</param>
        /// <returns>true if item was successfully removed from the collection;otherwise, false. This method also returns false if item is not found in the original</returns>
        public bool Remove(Collection<String> referenceIds)
        {
            //TODO : Need to implement this method.
            throw new NotImplementedException();
        }

        /// <summary>
        /// Splits current collection based on batchSize
        /// </summary>
        /// <param name="batchSize">Indicates no of objects to be put in each batch</param>
        /// <returns>IDataModelObjectCollection in Batch</returns>
        public Collection<IDataModelObjectCollection> Split(Int32 batchSize)
        {
            Collection<IDataModelObjectCollection> relationshipCardinalitiesInBatch = null;

            if (this._relationshipCardinalities != null)
            {
                relationshipCardinalitiesInBatch = Utility.Split(this, batchSize);
            }

            return relationshipCardinalitiesInBatch;
        }

        /// <summary>
        /// Adds item in current collection
        /// </summary>
        /// <param name="item">Indicates item to be added in collection</param>
        public void AddDataModelObject(IDataModelObject item)
        {
            this.Add(item as RelationshipCardinality);
        }

        #endregion

        #endregion

        #region Private Methods

        private void LoadRelationshipCardinalities(string valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "RelationshipCardinality")
                        {
                            String relTypeXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(relTypeXml))
                            {
                                RelationshipCardinality relationshipCardinality = new RelationshipCardinality(relTypeXml);
                                if (relationshipCardinality != null)
                                {
                                    this.Add(relationshipCardinality);
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
