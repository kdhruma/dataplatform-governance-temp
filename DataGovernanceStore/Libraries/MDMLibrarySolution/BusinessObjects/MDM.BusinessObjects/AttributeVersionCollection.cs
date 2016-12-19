using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Linq;
using System;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represent Collection of AttributrVersion Object
    /// </summary>
    [DataContract]
    public class AttributeVersionCollection : ICollection<AttributeVersion>, IEnumerable<AttributeVersion>, IAttributeVersionCollection
    {
        #region Fields

        /// <summary>
        /// Field denoting collection of AttributeVersion
        /// </summary>
        [DataMember]
        private Collection<AttributeVersion> _attributeVersion = new Collection<AttributeVersion>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the AttributeVersion Collection
        /// </summary>
        public AttributeVersionCollection() : base() { }

        #endregion

        #region Public Methods

        /// <summary>
        /// Check if AttributeVersionCollection contains AttributeVersion with given Id
        /// </summary>
        /// <param name="Id">Id using which AttributeVersion is to be searched from collection</param>
        /// <returns>
        /// <para>true : If AttributeVersion found in AttributeVersionCollection</para>
        /// <para>false : If AttributeVersion found not in AttributeVersionCollection</para>
        /// </returns>
        public bool Contains(Int64 Id)
        {
            if (GetAttributeVersion(Id) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Remove entity type object from AttributeVersionCollection
        /// </summary>
        /// <param name="typeId">typeId of entity type which is to be removed from collection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool Remove(Int32 typeId)
        {
            AttributeVersion attributeVersion = GetAttributeVersion(typeId);

            if (attributeVersion == null)
                throw new ArgumentException("No AttributeVersion found for given Id");
            else
                return this.Remove(attributeVersion);
        }

        /// <summary>
        /// Get Xml representation of AttributeVersionCollection
        /// </summary>
        /// <returns>Xml representation of AttributeVersionCollection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            returnXml = "<AttributeVersions>";

            if (this._attributeVersion != null && this._attributeVersion.Count > 0)
            {
                foreach (AttributeVersion version in this._attributeVersion)
                {
                    returnXml = String.Concat(returnXml, version.ToXml());
                }
            }

            returnXml = String.Concat(returnXml, "</AttributeVersions>");

            return returnXml;
        }

        /// <summary>
        /// Get Xml representation of AttributeVersionCollection
        /// </summary>
        /// <returns>Xml representation of AttributeVersionCollection</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String returnXml = String.Empty;

            returnXml = "<AttributeVersions>";

            if (this._attributeVersion != null && this._attributeVersion.Count > 0)
            {
                foreach (AttributeVersion version in this._attributeVersion)
                {
                    returnXml = String.Concat(returnXml, version.ToXml(serialization));
                }
            }

            returnXml = String.Concat(returnXml, "</AttributeVersions>");

            return returnXml;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is AttributeVersionCollection)
            {
                AttributeVersionCollection objectToBeCompared = obj as AttributeVersionCollection;

                Int32 attributeVersionsUnion = this._attributeVersion.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 attributeVersionsIntersect = this._attributeVersion.ToList().Intersect(objectToBeCompared.ToList()).Count();

                if (attributeVersionsUnion != attributeVersionsIntersect)
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

            foreach (AttributeVersion AttributeVersion in this._attributeVersion)
            {
                hashCode += AttributeVersion.GetHashCode();
            }

            return hashCode;
        }

        #endregion

        #region Private Methods

        private AttributeVersion GetAttributeVersion(Int64 Id)
        {
            var filteredVersion = from version in this._attributeVersion
                                where version.Id == Id
                                select version;

            if (filteredVersion.Any())
                return filteredVersion.First();
            else
                return null;
        }

        #endregion

        #region ICollection<AttributeVersion> Members

        /// <summary>
        /// Add AttributeVersion object in collection
        /// </summary>
        /// <param name="version">AttributeVersion to add in collection</param>
        public void Add(AttributeVersion version)
        {
            this._attributeVersion.Add(version);
        }

        /// <summary>
        /// Removes all AttributeVersion from collection
        /// </summary>
        public void Clear()
        {
            this._attributeVersion.Clear();
        }

        /// <summary>
        /// Determines whether the AttributeVersionCollection contains a specific AttributeVersion
        /// </summary>
        /// <param name="version">The AttributeVersion object to locate in the AttributeVersionCollection.</param>
        /// <returns>
        /// <para>true : If AttributeVersion found in AttributeVersionCollection</para>
        /// <para>false : If AttributeVersion found not in AttributeVersionCollection</para>
        /// </returns>
        public bool Contains(AttributeVersion version)
        {
            return this._attributeVersion.Contains(version);
        }

        /// <summary>
        /// Copies the elements of the AttributeVersionCollection to a
        /// System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(AttributeVersion[] array, int arrayIndex)
        {
            this._attributeVersion.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of AttributeVersion in AttributeVersionCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._attributeVersion.Count;
            }
        }

        /// <summary>
        /// Check if AttributeVersionCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific AttributeVersion from the AttributeVersionCollection.
        /// </summary>
        /// <param name="version">The AttributeVersion object to remove from the AttributeVersionCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool Remove(AttributeVersion version)
        {
            return this._attributeVersion.Remove(version);
        }

        #endregion

        #region IEnumerable<AttributeVersion> Members

        /// <summary>
        /// Returns an enumerator that iterates through a AttribureVersionCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<AttributeVersion> GetEnumerator()
        {
            return this._attributeVersion.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntityTypeCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._attributeVersion.GetEnumerator();
        }

        #endregion
    }
}
