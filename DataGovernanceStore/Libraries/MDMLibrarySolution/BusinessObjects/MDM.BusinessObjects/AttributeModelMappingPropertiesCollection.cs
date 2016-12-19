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
    /// Represents class for a collection of attribute model locale properties
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class AttributeModelMappingPropertiesCollection : ICollection<AttributeModelMappingProperties>, IEnumerable<AttributeModelMappingProperties>
    {
        #region Fields

        [DataMember]
        [ProtoMember(1)]
        private Collection<AttributeModelMappingProperties> _attributeModelMappingProperties = new Collection<AttributeModelMappingProperties>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter-less constructor
        /// </summary>
        public AttributeModelMappingPropertiesCollection() : base() { }

        /// <summary>
        /// Constructor which takes attribute model mapping properties list as input parameter
        /// </summary>
        /// <param name="attributeModelMappingList">Indicates the attribute model mapping list</param>
        public AttributeModelMappingPropertiesCollection(IList<AttributeModelMappingProperties> attributeModelMappingList)
        {
            this._attributeModelMappingProperties = new Collection<AttributeModelMappingProperties>(attributeModelMappingList);
        }

        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Collection<Int32> GetAttributeIds()
        {
            Collection<Int32> attributeIds = null;

            if (this._attributeModelMappingProperties != null && this._attributeModelMappingProperties.Count > 0)
            {
                attributeIds = new Collection<Int32>();

                foreach (AttributeModelMappingProperties attrModelMappingProperties in this._attributeModelMappingProperties)
                {
                    attributeIds.Add(attrModelMappingProperties.Id);
                }
            }

            return attributeIds;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeId"></param>
        /// <returns></returns>
        public AttributeModelMappingProperties GetAttributeModelMappingProperties(Int32 attributeId)
        {
            return GetAttributeModelMappingProperties(new Collection<Int32>() { attributeId }).FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeIds"></param>
        /// <returns></returns>
        public AttributeModelMappingPropertiesCollection GetAttributeModelMappingProperties(Collection<Int32> attributeIds)
        {
            AttributeModelMappingPropertiesCollection attributeModelMappingProperties = null;

            if (this._attributeModelMappingProperties != null && this._attributeModelMappingProperties.Count > 0
                && attributeIds != null && attributeIds.Count > 0)
            {
                attributeModelMappingProperties = new AttributeModelMappingPropertiesCollection();

                foreach (AttributeModelMappingProperties attrModelMappingProperties in this._attributeModelMappingProperties)
                {
                    if (attributeIds.Contains(attrModelMappingProperties.Id))
                    {
                        attributeModelMappingProperties.Add(attrModelMappingProperties);
                    }

                    if (attributeModelMappingProperties.Count.Equals(attributeIds.Count)) break;
                }
            }

            return attributeModelMappingProperties;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeGroupId"></param>
        /// <returns></returns>
        public AttributeModelMappingPropertiesCollection GetAttributeModelMappingPropertiesByAttributeGroupId(Int32 attributeGroupId)
        {
            return GetAttributeModelMappingPropertiesByAttributeGroupId(new Collection<Int32>() { attributeGroupId });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeGroupIds"></param>
        /// <returns></returns>
        public AttributeModelMappingPropertiesCollection GetAttributeModelMappingPropertiesByAttributeGroupId(Collection<Int32> attributeGroupIds)
        {
            AttributeModelMappingPropertiesCollection attributeModelMappingProperties = null;

            if (this._attributeModelMappingProperties != null && this._attributeModelMappingProperties.Count > 0
                && attributeGroupIds != null && attributeGroupIds.Count > 0)
            {
                attributeModelMappingProperties = new AttributeModelMappingPropertiesCollection();

                foreach (AttributeModelMappingProperties attrModelMappingProperties in this._attributeModelMappingProperties)
                {
                    if (attributeGroupIds.Contains(attrModelMappingProperties.AttributeParentId))
                    {
                        attributeModelMappingProperties.Add(attrModelMappingProperties);
                    }
                }
            }

            return attributeModelMappingProperties;
        }

        /// <summary>
        /// This method returns all attribute model mappings if attribute ids or attribute group id are not available
        /// </summary>
        /// <param name="attributeIds">Indicates the attribute Ids based on which attribute model mapping is returned</param>
        /// <param name="attributeGroupIds">Indicates the attribute group Ids based on which attribute model mapping is returned</param>
        /// <returns>Returns the collection of attribute model mapping filter by attribute id and group id</returns>
        public AttributeModelMappingPropertiesCollection FilterByAttributeIdAndGroupId(Collection<Int32> attributeIds, Collection<Int32> attributeGroupIds)
        {
            AttributeModelMappingPropertiesCollection filteredAttributeModelMappingProperties = null;
            AttributeModelMappingPropertiesCollection filteredAttributeModelMappingPropertiesByIds = null;
            AttributeModelMappingPropertiesCollection filteredAttributeModelMappingPropertiesByGroupIds = null; 

            if (this._attributeModelMappingProperties != null && this._attributeModelMappingProperties.Count > 0)
            {
                if (attributeIds != null && attributeIds.Count > 0)
                {
                    filteredAttributeModelMappingPropertiesByIds = GetAttributeModelMappingProperties(attributeIds);
                }
                if (attributeGroupIds != null && attributeGroupIds.Count > 0)
                {
                    filteredAttributeModelMappingPropertiesByGroupIds = GetAttributeModelMappingPropertiesByAttributeGroupId(attributeGroupIds);
                }

                if (filteredAttributeModelMappingPropertiesByIds != null || filteredAttributeModelMappingPropertiesByGroupIds != null)
                {
                    filteredAttributeModelMappingProperties = new AttributeModelMappingPropertiesCollection();

                    filteredAttributeModelMappingProperties.AddRange(filteredAttributeModelMappingPropertiesByIds);

                    if (filteredAttributeModelMappingPropertiesByGroupIds != null)
                    {
                        // Need to check the duplicate first before adding, in case of pre-load attribute for entity loaded business rule this is causing the issue
                        foreach (AttributeModelMappingProperties attributeModelMappingProperties in filteredAttributeModelMappingPropertiesByGroupIds)
                        {
                            if (!filteredAttributeModelMappingProperties.Contains(attributeModelMappingProperties.Id, attributeModelMappingProperties.AttributeParentId))
                            {
                                filteredAttributeModelMappingProperties.Add(attributeModelMappingProperties);
                            }
                        }
                    }
                }
                else
                {
                    filteredAttributeModelMappingProperties = new AttributeModelMappingPropertiesCollection(this._attributeModelMappingProperties);
                }
            }

            return filteredAttributeModelMappingProperties;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Gets the attribute model mapping properties based on attribute id  and attribute group id.
        /// </summary>
        /// <param name="attributeId">Indicates the attribute Id based on which attribute model mapping is returned.</param>
        /// <param name="attributeGroupId">Indicates the attribute group Id based on which attribute model mapping is returned.</param>
        /// <returns>Returns the attribute model mapping filtered by attribute id and group id</returns>
        private AttributeModelMappingProperties Get(Int32 attributeId, Int32 attributeGroupId)
        {
            if (this._attributeModelMappingProperties != null && this._attributeModelMappingProperties.Count > 0)
            {
                foreach (AttributeModelMappingProperties attributeModelMappingProperties in this._attributeModelMappingProperties)
                {
                    if (attributeModelMappingProperties.Id == attributeId && attributeModelMappingProperties.AttributeParentId == attributeGroupId)
                    {
                        return attributeModelMappingProperties;
                    }
                }
            }

            return null;
        }

        #endregion Private Methods

        #region ICollection<AttributeModelMappingProperties> Members

        /// <summary>
        /// Add AttributeModelMappingProperties object in collection
        /// </summary>
        /// <param name="item">AttributeModelMappingProperties to add in collection</param>
        public void Add(AttributeModelMappingProperties item)
        {
            if (item != null)
                this._attributeModelMappingProperties.Add(item);
        }

        /// <summary>
        /// Add AttributeModelMappingProperties object in collection
        /// </summary>
        /// <param name="items">AttributeModelMappingProperties to add in collection</param>
        public void AddRange(AttributeModelMappingPropertiesCollection items)
        {
            if (items != null && items.Count > 0)
            {
                foreach (AttributeModelMappingProperties item in items)
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
            this._attributeModelMappingProperties.Clear();
        }

        /// <summary>
        /// Get the count of no. of entities in EntityCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._attributeModelMappingProperties.Count;
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
        public bool Remove(AttributeModelMappingProperties item)
        {
            return this._attributeModelMappingProperties.Remove(item);
        }

        /// <summary>
        /// Determines whether the AttributeModelMappingPropertiesCollection contains a specific AttributeModelMappingProperties.
        /// </summary>
        /// <param name="item">The AttributeModelMappingProperties object to locate in the AttributeModelMappingPropertiesCollection.</param>
        /// <returns>
        /// <para>true : If AttributeModelMappingProperties found in AttributeModelMappingPropertiesCollection.</para>
        /// <para>false : If AttributeModelMappingProperties found not in AttributeModelMappingPropertiesCollection.</para>
        /// </returns>
        public bool Contains(AttributeModelMappingProperties item)
        {
            return this._attributeModelMappingProperties.Contains(item);
        }

        /// <summary>
        /// Determines whether the AttributeModelMappingPropertiesCollection contains a specific AttributeModelMappingProperties.
        /// </summary>
        /// <param name="attributeId">Indicates attribute id to be find in the AttributeModelMappingPropertiesCollection.</param>
        /// <param name="attributeGroupId">Indicates attribute group id to be find in the AttributeModelMappingPropertiesCollection.</param>
        /// <returns>
        /// <para>true : If AttributeModelMappingProperties found in AttributeModelMappingPropertiesCollection.</para>
        /// <para>false : If AttributeModelMappingProperties found not in AttributeModelMappingPropertiesCollection.</para>
        /// </returns>
        public Boolean Contains(Int32 attributeId, Int32 attributeGroupId)
        {
            if (Get(attributeId, attributeGroupId) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
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
        public void CopyTo(AttributeModelMappingProperties[] array, int arrayIndex)
        {
            this._attributeModelMappingProperties.CopyTo(array, arrayIndex);
        }

        #endregion

        #region IEnumerable<AttributeModelMappingProperties> Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntityCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<AttributeModelMappingProperties> GetEnumerator()
        {
            return this._attributeModelMappingProperties.GetEnumerator();
        }

        #endregion IEnumerable<AttributeModelMappingProperties> Members

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a ColumnCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._attributeModelMappingProperties.GetEnumerator();
        }

        #endregion IEnumerable Members

        #endregion
    }
}