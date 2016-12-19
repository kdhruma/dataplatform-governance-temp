using System;
using System.Text;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies collection of vendor object
    /// </summary>
    [DataContract]
    public class VendorCollection : ICollection<Vendor>, IEnumerable<Vendor>, IVendorCollection
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        private Collection<Vendor> _vendorCollection = new Collection<Vendor>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public VendorCollection() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vendor"></param>
        public VendorCollection(Vendor vendor)
        {
            if (vendor != null)
            {
                this._vendorCollection.Add(vendor);
            }
        }

        #endregion

        #region ICollection Members

        /// <summary>
        /// Add Vendor object in collection
        /// </summary>
        /// <param name="item">Vendor to add in collection</param>
        public void Add(Vendor item)
        {
            this._vendorCollection.Add(item);
        }

        /// <summary>
        /// Removes all Vendors from collection
        /// </summary>
        public void Clear()
        {
            this._vendorCollection.Clear();
        }

        /// <summary>
        /// Determines whether the VendorCollection contains a specific Vendor.
        /// </summary>
        /// <param name="item">The Vendor object to locate in the VendorCollection.</param>
        /// <returns>
        /// <para>true : If Vendor found in VendorCollection</para>
        /// <para>false : If Vendor not found in VendorCollection</para>
        /// </returns>
        public bool Contains(Vendor item)
        {
            return this._vendorCollection.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the VendorCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from VendorCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(Vendor[] array, int arrayIndex)
        {
            this._vendorCollection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of vendor in VendorCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._vendorCollection.Count;
            }
        }

        /// <summary>
        /// Check if VendorCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the VendorCollection.
        /// </summary>
        /// <param name="item">The Vendor object to remove from the VendorCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original VendorCollection</returns>
        public bool Remove(Vendor item)
        {
            return this._vendorCollection.Remove(item);
        }

        #endregion

        #region IEnumerable<Vendor> Members

        /// <summary>
        /// Returns an enumerator that iterates through a VendorCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<Vendor> GetEnumerator()
        {
            return this._vendorCollection.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a VendorCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._vendorCollection.GetEnumerator();
        }

        #endregion
    }
}
