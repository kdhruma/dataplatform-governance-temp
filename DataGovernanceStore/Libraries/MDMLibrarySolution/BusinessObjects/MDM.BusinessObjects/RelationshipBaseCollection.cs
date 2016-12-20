using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

using ProtoBuf;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the collection of relationships
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class RelationshipBaseCollection :  ICollection<RelationshipBase>, IEnumerable<RelationshipBase>, IRelationshipBaseCollection
    {
        #region Fields

        /// <summary>
        /// Field denoting collection of relationships
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        private Collection<RelationshipBase> _relationships = new Collection<RelationshipBase>();

        #endregion

        #region Constructors

        /// <summary>
        /// Find relationship from collection based on related entity id
        /// </summary>
        /// <param name="relatedEntityId">Id of the related entity using which relationship is to be searched from collection</param>
        /// <returns>Relationship object</returns>
        public RelationshipBase this[Int64 relatedEntityId]
        {
            get
            {
                RelationshipBase relationship = GetRelationship(relatedEntityId);

                if (relationship == null)
                    throw new ArgumentException("No relationship found for given related entity id");
                else
                    return relationship;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Check if RelationshipCollection contains relationship with given related entity id
        /// </summary>
        /// <param name="relatedEntityId">Id of the related entity using which relationship is to be searched from collection</param>
        /// <returns>
        /// <para>true : If relationship found in RelationshipCollection</para>
        /// <para>false : If relationship found not in RelationshipCollection</para>
        /// </returns>
        public bool Contains(Int32 relatedEntityId)
        {
            if (GetRelationship(relatedEntityId) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Remove relationship object from RelationshipCollection
        /// </summary>
        /// <param name="relatedEntityId">Id of the related entity using which relationship is to be removed from collection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public Boolean Remove(Int32 relatedEntityId)
        {
            RelationshipBase relationship = GetRelationship(relatedEntityId);

            if (relationship == null)
                throw new ArgumentException("No relationship found for given related entity id");
            else
                return this.Remove(relationship);
        }

        /// <summary>
        /// Get Xml representation of Relationship Collection
        /// </summary>
        /// <returns>Xml representation of Relationship Collection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            returnXml = "<Relationships>";

            if (this._relationships != null && this._relationships.Count > 0)
            {
                foreach (RelationshipBase relationship in this._relationships)
                {
                    returnXml = String.Concat(returnXml, relationship.ToXml());
                }
            }

            returnXml = String.Concat(returnXml, "</Relationships>");

            return returnXml;
        }

        /// <summary>
        /// Get Xml representation of Relationship Collection based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Relationship Collection</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String returnXml = String.Empty;

            returnXml = "<Relationships>";

            if (this._relationships != null && this._relationships.Count > 0)
            {
                foreach (RelationshipBase relationship in this._relationships)
                {
                    returnXml = String.Concat(returnXml, relationship.ToXml(objectSerialization));
                }
            }

            returnXml = String.Concat(returnXml, "</Relationships>");

            return returnXml;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj is RelationshipBaseCollection)
            {
                RelationshipBaseCollection objectToBeCompared = obj as RelationshipBaseCollection;

                Int32 relationshipsUnion = this._relationships.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 relationshipsIntersect = this._relationships.ToList().Intersect(objectToBeCompared.ToList()).Count();

                if (relationshipsUnion != relationshipsIntersect)
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

            foreach (RelationshipBase relationship in this._relationships)
            {
                hashCode += relationship.GetHashCode();
            }

            return hashCode;
        }

        #endregion

        #region Private Methods

        private RelationshipBase GetRelationship(Int64 relatedEntityId)
        {
            var filteredRelationships = from relationship in this._relationships
                                        where relationship.RelatedEntityId == relatedEntityId
                                        select relationship;

            if (filteredRelationships.Any())
                return filteredRelationships.First();
            else
                return null;
        }

        #endregion

        #region ICollection<Relationship> Members

        /// <summary>
        /// Add relationship object in collection
        /// </summary>
        /// <param name="relationship">Relationship to add in collection</param>
        public void Add(RelationshipBase relationship)
        {
            this._relationships.Add(relationship);
        }

        /// <summary>
        /// Removes all relationships from collection
        /// </summary>
        public void Clear()
        {
            this._relationships.Clear();
        }

        /// <summary>
        /// Determines whether the RelationshipCollection contains a specific relationship
        /// </summary>
        /// <param name="relationship">The relationship object to locate in the relationshipCollection.</param>
        /// <returns>
        /// <para>true : If relationship found in RelationshipCollection</para>
        /// <para>false : If relationship found not in RelationshipCollection</para>
        /// </returns>
        public Boolean Contains(RelationshipBase relationship)
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
        public void CopyTo(RelationshipBase[] array, Int32 arrayIndex)
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
        /// Removes the first occurrence of a specific relationship from the RelationshipCollection.
        /// </summary>
        /// <param name="relationship">The relationship object to remove from the RelationshipCollection.</param>
        /// <returns>true if relationship is successfully removed; otherwise, false. This method also returns false if relationship was not found in the original collection</returns>
        public Boolean Remove(RelationshipBase relationship)
        {
            return this._relationships.Remove(relationship);
        }

        #endregion

        #region IEnumerable<Relationship> Members

        /// <summary>
        /// Returns an enumerator that iterates through a RelationshipCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<RelationshipBase> GetEnumerator()
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
    }
}
