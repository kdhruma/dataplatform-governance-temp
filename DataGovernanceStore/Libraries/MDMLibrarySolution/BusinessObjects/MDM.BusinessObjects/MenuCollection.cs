using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;  

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;      

    /// <summary>
    /// Represents Collection of Menu Objects
    /// </summary>
    [DataContract]
    public class MenuCollection : ICollection<Menu>, IEnumerable<Menu>, IMenuCollection
    {
        #region Fields

        /// <summary>
        /// Field denoting Collection of Menu Object
        /// </summary>
        [DataMember]
        private Collection<Menu> _menus = new Collection<Menu>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public MenuCollection() : base() { }

        #endregion

        #region Properties

        #endregion

        #region IMenuCollection

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is MenuCollection)
            {
                MenuCollection objectToBeCompared = obj as MenuCollection;
                Int32 menusUnion = this._menus.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 menuIntersect = this._menus.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if (menusUnion != menuIntersect)
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
            foreach (Menu menu in this._menus)
            {
                hashCode += menu.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// Get Xml representation of MenuCollection object
        /// </summary>
        /// <returns>Xml string representing the MenuCollection</returns>
        public String ToXml()
        {
            String MenuXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (Menu menu in this._menus)
            {
                builder.Append(menu.ToXml());
            }

            MenuXml = String.Format("<Menus>{0}</Menus>", builder.ToString());

            return MenuXml;
        }

        /// <summary>
        /// Get Xml representation of Menu object
        /// </summary>
        /// <param name="objectSerialization">Indicates serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String MenuXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (Menu menu in this._menus)
            {
                builder.Append(menu.ToXml(objectSerialization));
            }

            MenuXml = String.Format("<Menus>{0}</Menus>", builder.ToString());

            return MenuXml;
        }
        
        #endregion

        #region ICollection<Menu>

        /// <summary>
        /// Add Menu object in collection
        /// </summary>
        /// <param name="item">Menu to add in collection</param>
        public void Add(Menu item)
        {
            this._menus.Add(item);
        }

        /// <summary>
        /// Removes all Menus from collection
        /// </summary>
        public void Clear()
        {
            this._menus.Clear();
        }

        /// <summary>
        /// Determines whether the MenuCollection contains a specific Menu.
        /// </summary>
        /// <param name="item">The Menu object to locate in the MenuCollection.</param>
        /// <returns>
        /// <para>true : If Menu found in mappingCollection</para>
        /// <para>false : If Menu found not in mappingCollection</para>
        /// </returns>
        public bool Contains(Menu item)
        {
            return this._menus.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the MenuCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from MenuCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(Menu[] array, int arrayIndex)
        {
            this._menus.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of Menus in MenuCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._menus.Count;
            }
        }

        /// <summary>
        /// Check if MenuCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the MenuCollection.
        /// </summary>
        /// <param name="item">The Locale object to remove from the MenuCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original MenuCollection</returns>
        public bool Remove(Menu item)
        {
            return this._menus.Remove(item);
        }

        #endregion

        #region IEnumerable<Menu>

        /// <summary>
        /// Returns an enumerator that iterates through a MenuCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<Menu> GetEnumerator()
        {
            return this._menus.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a MenuCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._menus.GetEnumerator();
        }

        #endregion
    }
}
