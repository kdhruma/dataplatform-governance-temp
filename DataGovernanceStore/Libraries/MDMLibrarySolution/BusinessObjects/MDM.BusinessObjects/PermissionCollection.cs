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
    /// Specifies the collection of permission objects
    /// </summary>
    [DataContract]
    public class PermissionCollection : ICollection<Permission>, IEnumerable<Permission>, ICloneable
    {
        #region Fields

        //This is the most funny part of the WCF which is able to serialize even a private member.. needs to do further RnD to find out the internals
        [DataMember]
        private Collection<Permission> _permissions = new Collection<Permission>();
         
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Permission class
        /// </summary>
        public PermissionCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public PermissionCollection(String valueAsXml)
        {
            LoadPermissionCollection(valueAsXml);
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
            if (obj is PermissionCollection)
            {
                PermissionCollection objectToBeCompared = obj as PermissionCollection;
                Int32 attributesUnion = this._permissions.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 attributesIntersect = this._permissions.ToList().Intersect(objectToBeCompared.ToList()).Count();

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

            foreach (Permission permission in this._permissions)
            {
                hashCode += permission.GetHashCode();
            }
            return hashCode;
        }

        #endregion

        #region Private Methods

        private void LoadPermissionCollection( String valuesAsXml )
        {
            //if ( !String.IsNullOrWhiteSpace(valuesAsXml) )
            //{
            //    XmlTextReader reader = null;
            //    try
            //    {
            //        reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

            //        while ( !reader.EOF )
            //        {
            //            if ( reader.NodeType == XmlNodeType.Element && reader.Name == "Permission" )
            //            {
            //                String attributeXml = reader.ReadOuterXml();
            //                if ( !String.IsNullOrEmpty(attributeXml) )
            //                {
            //                    Permission permission = new Permission(attributeXml);
            //                    if (permission != null)
            //                    {
            //                        this.Add(permission);
            //                    }
            //                }

            //            }
            //            else
            //            {
            //                reader.Read();
            //            }
            //        }
            //    }
            //    finally
            //    {
            //        if ( reader != null )
            //        {
            //            reader.Close();
            //        }
            //    }
            //}
        }

        #endregion

        #region ICollection<Attribute> Members

        /// <summary>
        /// Add permission object in collection
        /// </summary>
        /// <param name="item">permission to add in collection</param>
        public void Add(Permission item)
        {
            this._permissions.Add(item);
        }

        /// <summary>
        /// Removes all permissions from collection
        /// </summary>
        public void Clear()
        {
            this._permissions.Clear();
        }

        /// <summary>
        /// Determines whether the PermissionCollection contains a specific permission.
        /// </summary>
        /// <param name="item">The permission object to locate in the PermissionCollection.</param>
        /// <returns>
        /// <para>true : If permission found in PermissionCollection</para>
        /// <para>false : If permission found not in PermissionCollection</para>
        /// </returns>
        public bool Contains(Permission item)
        {
            return this._permissions.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the PermissionCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from PermissionCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(Permission[] array, int arrayIndex)
        {
            this._permissions.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of permissions in PermissionCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._permissions.Count;
            }
        }

        /// <summary>
        /// Check if PermissionCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the PermissionCollection.
        /// </summary>
        /// <param name="item">The permission object to remove from the PermissionCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original PermissionCollection</returns>
        public bool Remove(Permission item)
        {
            return this._permissions.Remove(item);
        }

        #endregion

        #region IEnumerable<Attribute> Members

        /// <summary>
        /// Returns an enumerator that iterates through a PermissionCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<Permission> GetEnumerator()
        {
            return this._permissions.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a PermissionCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._permissions.GetEnumerator();
        }

        #endregion

        #region IAttributeCollection Members

        #endregion IAttributeCollection Memebers

        #region ICloneable Members

        /// <summary>
        /// Clones instance
        /// </summary>
        /// <returns>Cloned object</returns>
        public object Clone()
        {
            PermissionCollection clonedInstance = new PermissionCollection();
            if (this._permissions != null)
            {
                for (Int32 i = 0; i < this._permissions.Count; i++)
                {
                    clonedInstance.Add((Permission) this._permissions[i].Clone());
                }
            }
            return clonedInstance;
        }

        #endregion
    }
}
