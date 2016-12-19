using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace MDM.Core
{
    /// <summary>
    /// Base class for collections of business objects that could contain additional data members in it.
    /// Implements a basic collection operations.
    /// </summary>
    /// <typeparam name="TBusinessObject">The type of the business object.</typeparam>
    [DataContract]
    [ProtoBuf.ProtoContract]
    public class ItemContractCollection<TBusinessObject> : ICollection<TBusinessObject>
    {
        /// <summary>
        /// Field denoting instance of item contract collection
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(1)]
        protected Collection<TBusinessObject> _items = new Collection<TBusinessObject>();

        #region ICollection<TBusinessObject> implementation

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<TBusinessObject> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        public void Add(TBusinessObject item)
        {
            _items.Add(item);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        public void Clear()
        {
            _items.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        /// true if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false.
        /// </returns>
        public bool Contains(TBusinessObject item)
        {
            return _items.Contains(item);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(TBusinessObject[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        /// true if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false. This method also returns false if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </returns>
        public bool Remove(TBusinessObject item)
        {
            return _items.Remove(item);
        }

        /// <summary>
        /// Inserts item at the specified index
        /// </summary>
        /// <param name="index">The zero based index at which item needs to be inserted</param>
        /// <param name="item">Item to insert</param>
        /// <returns>Boolean flag saying whether insertion is successful or not</returns>
        public bool Insert(int index, TBusinessObject item)
        {
            bool isInsertSuccessful = true;

            try
            {
                _items.Insert(index, item);
            }
            catch
            {
                isInsertSuccessful = false;
            }

            return isInsertSuccessful;
        }

        /// <summary>
        /// Gets the element at the specified index
        /// </summary>
        /// <param name="index">The zero based index of the element to get</param>
        /// <returns>Element at the specified index</returns>
        public TBusinessObject ElementAt(int index)
        {
            return _items[index];
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        ///   </returns>
        public int Count { get { return _items.Count; } }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </summary>
        /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only; otherwise, false.
        ///   </returns>
        public bool IsReadOnly { get { return false; } }

        #endregion

        #region IEnumerable implementation

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}