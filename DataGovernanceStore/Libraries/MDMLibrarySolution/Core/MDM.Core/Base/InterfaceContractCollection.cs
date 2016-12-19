using System.Runtime.Serialization;

namespace MDM.Core
{
    /// <summary>
    /// Base class for collections of business objects that should have possibility to
    /// represent both collection of business objects and collection of business object's interfaces.
    /// </summary>
    /// <typeparam name="TBusinessObjectInterface">The interface of business object.</typeparam>
    /// <typeparam name="TBusinessObject">The type of business object.</typeparam>
    [DataContract]
    [ProtoBuf.ProtoContract]
    public class InterfaceContractCollection<TBusinessObjectInterface, TBusinessObject> : ItemContractCollection<TBusinessObject>
        where TBusinessObject : TBusinessObjectInterface
    {
        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        public void Add(TBusinessObjectInterface item)
        {
            _items.Add((TBusinessObject)item);
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        /// true if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool Contains(TBusinessObjectInterface item)
        {
            return _items.Contains((TBusinessObject)item);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void CopyTo(TBusinessObjectInterface[] array, int arrayIndex)
        {
            foreach (TBusinessObject item in _items)
            {
                array[arrayIndex] = item;
                arrayIndex++;
            }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        /// true if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false. This method also returns false if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool Remove(TBusinessObjectInterface item)
        {
            return _items.Remove((TBusinessObject)item);
        }

        /// <summary>
        /// Inserts item at the specified index
        /// </summary>
        /// <param name="index">The zero based index at which item needs to be inserted</param>
        /// <param name="item">Item to insert</param>
        /// <returns>Boolean flag saying whether insertion is successful or not</returns>
        public bool Insert(int index, TBusinessObjectInterface item)
        {
            bool isInsertSuccessful = true;

            try
            {
                _items.Insert(index, (TBusinessObject)item);
            }
            catch
            {
                isInsertSuccessful = false;
            }

            return isInsertSuccessful;
        }
    }
}