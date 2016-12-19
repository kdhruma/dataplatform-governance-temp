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

    /// <summary>
    /// Specifies the collection of messagePackage objects
    /// </summary>
    [DataContract]
    public class MDMMessagePackageCollection : ICollection<MDMMessagePackage>, IEnumerable<MDMMessagePackage>
    {
        #region Fields

        //This is the most funny part of the WCF which is able to serialize even a private member.. needs to do further RnD to find out the internals
        [DataMember]
        private Collection<MDMMessagePackage> _messagePackages = new Collection<MDMMessagePackage>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the MDMMessagePackage collection class
        /// </summary>
        public MDMMessagePackageCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public MDMMessagePackageCollection(String valueAsXml)
        {
            LoadMDMMessagePackageCollection(valueAsXml);
        }

        #endregion

        #region Properties

        #endregion

        #region Public Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is MDMMessagePackageCollection)
            {
                MDMMessagePackageCollection objectToBeCompared = obj as MDMMessagePackageCollection;
                Int32 attributesUnion = this._messagePackages.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 attributesIntersect = this._messagePackages.ToList().Intersect(objectToBeCompared.ToList()).Count();

                if (attributesUnion != attributesIntersect)
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

            foreach (MDMMessagePackage messagePackage in this._messagePackages)
            {
                hashCode += messagePackage.GetHashCode();
            }
            return hashCode;
        }

        #endregion

        #region Private Methods

        private void LoadMDMMessagePackageCollection(String valuesAsXml)
        {
           
        }

        #endregion

        #region ICollection<MDMMessagePackage> Members

        /// <summary>
        /// Add messagePackage object in collection
        /// </summary>
        /// <param name="item">messagePackage to add in collection</param>
        public void Add(MDMMessagePackage item)
        {
            this._messagePackages.Add(item);
        }

        /// <summary>
        /// Removes all messagePackages from collection
        /// </summary>
        public void Clear()
        {
            this._messagePackages.Clear();
        }

        /// <summary>
        /// Determines whether the MDMMessagePackageCollection contains a specific messagePackage.
        /// </summary>
        /// <param name="item">The messagePackage object to locate in the MDMMessagePackageCollection.</param>
        /// <returns>
        /// <para>true : If messagePackage found in MDMMessagePackageCollection</para>
        /// <para>false : If messagePackage found not in MDMMessagePackageCollection</para>
        /// </returns>
        public bool Contains(MDMMessagePackage item)
        {
            return this._messagePackages.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the MDMMessagePackageCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from MDMMessagePackageCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(MDMMessagePackage[] array, int arrayIndex)
        {
            this._messagePackages.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of messagePackages in MDMMessagePackageCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._messagePackages.Count;
            }
        }

        /// <summary>
        /// Check if MDMMessagePackageCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the MDMMessagePackageCollection.
        /// </summary>
        /// <param name="item">The messagePackage object to remove from the MDMMessagePackageCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original MDMMessagePackageCollection</returns>
        public bool Remove(MDMMessagePackage item)
        {
            return this._messagePackages.Remove(item);
        }

        #endregion

        #region IEnumerable<MDMMessagePackage> Members

        /// <summary>
        /// Returns an enumerator that iterates through a MDMMessagePackageCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<MDMMessagePackage> GetEnumerator()
        {
            return this._messagePackages.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a MDMMessagePackageCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._messagePackages.GetEnumerator();
        }

        #endregion

        #region IMDMMessagePackageCollection Members

        #endregion IMDMMessagePackageCollection Memebers
    }
}
