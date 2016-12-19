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
    /// Represents class for attribute model locale properties collection
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class AttributeModelLocalePropertiesCollection : ICollection<AttributeModelLocaleProperties>, IEnumerable<AttributeModelLocaleProperties>
    {
        #region Fields

        [DataMember]
        [ProtoMember(1)]
        private Collection<AttributeModelLocaleProperties> _attributeModelLocaleProperties = new Collection<AttributeModelLocaleProperties>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public AttributeModelLocalePropertiesCollection() : base() { }

        /// <summary>
        /// Constructor which takes attribute model locale properties list as input parameter
        /// </summary>
        /// <param name="attributeModelLocaleProperties">List of attribute model locale properties</param>
        public AttributeModelLocalePropertiesCollection(IList<AttributeModelLocaleProperties> attributeModelLocaleProperties)
        {
            this._attributeModelLocaleProperties = new Collection<AttributeModelLocaleProperties>(attributeModelLocaleProperties);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property which return attribute model locale properties object based on attribute id
        /// </summary>
        /// <param name="attributeId">Indicates the attribute identifier</param>
        /// <returns>Returns attribute model locale properties object based on attribute id</returns>
        public AttributeModelLocaleProperties this[Int32 attributeId]
        {
            get
            {
                return GetAttributeModelLocaleProperties(attributeId);
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get attribute model locale properties based on attribute identifier
        /// </summary>
        /// <param name="attributeId">Indicates the attribute identifier</param>
        /// <returns>Return attribute model locale properties based on attribute id</returns>
        public AttributeModelLocaleProperties GetAttributeModelLocaleProperties(Int32 attributeId)
        {
            AttributeModelLocaleProperties attributeModelLocaleProperties = (from attrModelLocaleProperties in this._attributeModelLocaleProperties
                                                                             where attrModelLocaleProperties.Id == attributeId
                                                                             select attrModelLocaleProperties).ToList().FirstOrDefault();
            return attributeModelLocaleProperties;
        }

        #endregion Public Methods

        #region Private Methods
        #endregion Private Methods

        #region ICollection<AttributeModelMappingProperties> Members

        /// <summary>
        /// Add AttributeModelMappingProperties object in collection
        /// </summary>
        /// <param name="item">AttributeModelMappingProperties to add in collection</param>
        public void Add(AttributeModelLocaleProperties item)
        {
            this._attributeModelLocaleProperties.Add(item);
        }

        /// <summary>
        /// Add AttributeModelMappingProperties object in collection
        /// </summary>
        /// <param name="items">AttributeModelMappingProperties to add in collection</param>
        public void AddRange(AttributeModelLocalePropertiesCollection items)
        {
            if (items != null && items.Count > 0)
            {
                foreach (AttributeModelLocaleProperties item in items)
                {
                    this.Add(item);
                }
            }
        }

        /// <summary>
        /// Removes all entities from collection
        /// </summary>
        public void Clear()
        {
            this._attributeModelLocaleProperties.Clear();
        }

        /// <summary>
        /// Get the count of no. of entities in EntityCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._attributeModelLocaleProperties.Count;
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
        public bool Remove(AttributeModelLocaleProperties item)
        {
            return this._attributeModelLocaleProperties.Remove(item);
        }

        /// <summary>
        /// Determines whether the ColumnCollection contains a specific column.
        /// </summary>
        /// <param name="item">The column object to locate in the ColumnCollection.</param>
        /// <returns>
        /// <para>true : If column found in columnCollection</para>
        /// <para>false : If column found not in columnCollection</para>
        /// </returns>
        public bool Contains(AttributeModelLocaleProperties item)
        {
            return this._attributeModelLocaleProperties.Contains(item);
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
        public void CopyTo(AttributeModelLocaleProperties[] array, int arrayIndex)
        {
            this._attributeModelLocaleProperties.CopyTo(array, arrayIndex);
        }

        #endregion

        #region IEnumerable<AttributeModelMappingProperties> Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntityCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<AttributeModelLocaleProperties> GetEnumerator()
        {
            return this._attributeModelLocaleProperties.GetEnumerator();
        }

        #endregion IEnumerable<AttributeModelMappingProperties> Members

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a ColumnCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._attributeModelLocaleProperties.GetEnumerator();
        }

        #endregion IEnumerable Members

        #endregion
    }
}