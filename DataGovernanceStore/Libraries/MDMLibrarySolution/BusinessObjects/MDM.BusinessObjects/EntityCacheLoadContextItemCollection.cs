using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;

    /// <summary>
    /// Represents a collection of cache status load context. This is used in activity log table to specify the cache reload request.
    /// </summary>
    public class EntityCacheLoadContextItemCollection : ICollection<EntityCacheLoadContextItem>, IEnumerable<EntityCacheLoadContextItem>
    {
        #region Fields

        /// <summary>
        /// Holds the collection of EntityCacheLoadContextItems, based on which the entity cache status request is processed.
        /// </summary>
        private Collection<EntityCacheLoadContextItem> _entityCacheLoadContextItemCollection = new Collection<EntityCacheLoadContextItem>();

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of the EntityCacheLoadContextItemCollection.
        /// </summary>
        public EntityCacheLoadContextItemCollection()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Specifies the count of the elements in the collection.
        /// </summary>
        public Int32 Count
        {
            get { return _entityCacheLoadContextItemCollection.Count; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the EntityCacheLoadContextItem if it exists in the list, else a new item is created based on the specified type and returned.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public EntityCacheLoadContextItem GetOrCreateEntityCacheLoadContextItem(EntityCacheLoadContextTypeEnum type)
        {
            EntityCacheLoadContextItem entityCacheLoadContextItem = GetEntityCacheLoadContextItem(type);
            if (entityCacheLoadContextItem == null)
            {
                entityCacheLoadContextItem = new EntityCacheLoadContextItem();
                entityCacheLoadContextItem.Type = type;
                this._entityCacheLoadContextItemCollection.Add(entityCacheLoadContextItem);
            }
            return entityCacheLoadContextItem;
        }

        /// <summary>
        /// Gets entity cache load context item based on the object type.
        /// </summary>
        /// <param name="type">Indicates type of entity cache load context</param>
        /// <returns>Returns entity cache load context item</returns>
        public EntityCacheLoadContextItem GetEntityCacheLoadContextItem(EntityCacheLoadContextTypeEnum type)
        {
            foreach (EntityCacheLoadContextItem entityCacheLoadContextItem in this._entityCacheLoadContextItemCollection)
            {
                if (entityCacheLoadContextItem.Type == type)
                {
                    return entityCacheLoadContextItem;
                }
            }
            return null;
        }

        /// <summary>
        /// Get XML representation of Entity Cache Load Context Item collection.
        /// </summary>
        /// <returns>XML representation of Entity Cache Load Context Item object</returns>
        public String ToXml()
        {
            String objectInXML = String.Empty;

            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter))
                {
                    xmlWriter.WriteStartElement("EntityCacheLoadContextItemCollection");

                    if (this._entityCacheLoadContextItemCollection != null && this._entityCacheLoadContextItemCollection.Count > 0)
                    {
                        foreach (EntityCacheLoadContextItem entityCacheLoadContextItem in this._entityCacheLoadContextItemCollection)
                        {
                            xmlWriter.WriteRaw(entityCacheLoadContextItem.ToXml());
                        }
                    }

                    //EntityCacheLoadContextItemCollection node end
                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();

                    //get the actual XML
                    objectInXML = stringWriter.ToString();
                }
            }

            return objectInXML;
        }

        /// <summary>
        /// Add EntityCacheLoadContextItem object in collection
        /// </summary>
        /// <param name="item">EntityCacheLoadContextItem to add in collection</param>
        public void Add(EntityCacheLoadContextItem item)
        {
            this._entityCacheLoadContextItemCollection.Add(item);
        }

        /// <summary>
        /// Removes all EntityCacheLoadContextItem from collection
        /// </summary>
        public void Clear()
        {
            this._entityCacheLoadContextItemCollection.Clear();
        }

        /// <summary>
        /// Determines whether the EntityCacheLoadContextItemCollection contains a specific EntityCacheLoadContextItem.
        /// </summary>
        /// <param name="item">The entityCacheLoadContextItem object to locate in the EntityCacheLoadContextItemCollection .</param>
        /// <returns>
        /// <para>true : If entityCacheLoadContextItem found in EntityCacheLoadContextItemCollection </para>
        /// <para>false : If entityCacheLoadContextItem found not in EntityCacheLoadContextItemCollection </para>
        /// </returns>
        public bool Contains(EntityCacheLoadContextItem item)
        {
            return this._entityCacheLoadContextItemCollection.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the EntityCacheLoadContextItemCollection to an System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements copied from EntityCacheLoadContextItemCollection. 
        ///  The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(EntityCacheLoadContextItem[] array, int arrayIndex)
        {
            this._entityCacheLoadContextItemCollection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Check if EntityCacheLoadContextItemCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the EntityCacheLoadContextItemCollection.
        /// </summary>
        /// <param name="item">The entityCacheLoadContextItem object to remove from the EntityCacheLoadContextItemCollection .</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original EntityCacheLoadContextItemCollection </returns>
        public bool Remove(EntityCacheLoadContextItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("EntityCacheLoadContextItem is null");
            }

            return this._entityCacheLoadContextItemCollection.Remove(item);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a EntityCacheLoadContextItemCollection .
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<EntityCacheLoadContextItem> GetEnumerator()
        {
            return this._entityCacheLoadContextItemCollection.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a EntityCacheLoadContextItemCollection .
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._entityCacheLoadContextItemCollection.GetEnumerator();
        }

        #endregion
    }
}
