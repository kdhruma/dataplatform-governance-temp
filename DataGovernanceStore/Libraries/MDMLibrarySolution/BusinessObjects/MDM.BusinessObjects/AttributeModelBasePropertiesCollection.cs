using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.Collections;

using ProtoBuf;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the Entity Instance Collection for the Object
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class AttributeModelBasePropertiesCollection : ICollection<AttributeModelBaseProperties>, IEnumerable<AttributeModelBaseProperties>
    {
        #region Fields

        [DataMember]
        [ProtoMember(1)]
        private Collection<AttributeModelBaseProperties> _attributeModelBaseProperties = new Collection<AttributeModelBaseProperties>();

        #endregion

        #region Constructors
        
        /// <summary>
        /// Parameter-less constructor
        /// </summary>
        public AttributeModelBasePropertiesCollection() : base() { }

        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods
        #endregion Public Methods

        #region Private Methods
        #endregion Private Methods

        #region ICollection<AttributeModelMappingProperties> Members

        /// <summary>
        /// Add AttributeModelMappingProperties object in collection
        /// </summary>
        /// <param name="item">AttributeModelMappingProperties to add in collection</param>
        public void Add(AttributeModelBaseProperties item)
        {
            this._attributeModelBaseProperties.Add(item);
        }

        /// <summary>
        /// Removes all entities from collection
        /// </summary>
        public void Clear()
        {
            this._attributeModelBaseProperties.Clear();
        }

        /// <summary>
        /// Get the count of no. of entities in EntityCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._attributeModelBaseProperties.Count;
            }
        }

        /// <summary>
        /// Check if EntityCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the ColumnCollection.
        /// </summary>
        /// <param name="item">The column object to remove from the ColumnCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original ColumnCollection</returns>
        public bool Remove(AttributeModelBaseProperties item)
        {
            return this._attributeModelBaseProperties.Remove(item);
        }

        /// <summary>
        /// Determines whether the ColumnCollection contains a specific column.
        /// </summary>
        /// <param name="item">The column object to locate in the ColumnCollection.</param>
        /// <returns>
        /// <para>true : If column found in columnCollection</para>
        /// <para>false : If column found not in columnCollection</para>
        /// </returns>
        public bool Contains(AttributeModelBaseProperties item)
        {
            return this._attributeModelBaseProperties.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the ColumnCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from ColumnCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(AttributeModelBaseProperties[] array, int arrayIndex)
        {
            this._attributeModelBaseProperties.CopyTo(array, arrayIndex);
        }

        #endregion

        #region IEnumerable<AttributeModelMappingProperties> Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntityCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<AttributeModelBaseProperties> GetEnumerator()
        {
            return this._attributeModelBaseProperties.GetEnumerator();
        }

        #endregion IEnumerable<AttributeModelMappingProperties> Members

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a ColumnCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._attributeModelBaseProperties.GetEnumerator();
        }

        #endregion IEnumerable Members

        #endregion
    }
}