using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MDM.BusinessObjects.Caching
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class CacheConfigurationCollection : ICollection<CacheConfiguration>, IEnumerable<CacheConfiguration>
    {
        [DataMember]
        private Collection<CacheConfiguration>  _internalCollection = new Collection<CacheConfiguration>();

        /// <summary>
        /// 
        /// </summary>
        public CacheConfigurationCollection() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        public void LoadFromXml(String xml)
        {
            try
            {
                var xmlRoot = XElement.Parse(xml);

                var cconlist = from rml in xmlRoot.Descendants("MDMCacheObjectGroup") select rml;
                foreach (var con in cconlist)
                {
                    var gname = con.Attribute("Name").Value;

                    foreach (var mo in con.Elements())
                    {
                        var cconfig = new CacheConfiguration();
                        cconfig.GroupName = gname;
                        cconfig.LoadFromXml(mo.ToString());
                        _internalCollection.Add(cconfig);
                    }
                }
            }
            catch (Exception e)
            {
                e.ToString();
                //TODO: Add logging.
            }
        }


        #region ICollection<EntityCacheConfiguration> Members

        /// <summary>
        /// Add AttributeModelMappingProperties object in collection
        /// </summary>
        /// <param name="item">AttributeModelMappingProperties to add in collection</param>
        public void Add(CacheConfiguration item)
        {
            this._internalCollection.Add(item);
        }

        /// <summary>
        /// Removes all entities from collection
        /// </summary>
        public void Clear()
        {
            this._internalCollection.Clear();
        }

        /// <summary>
        /// Get the count of no. of entities in EntityCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._internalCollection.Count;
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
        public bool Remove(CacheConfiguration item)
        {
            return this._internalCollection.Remove(item);
        }

        /// <summary>
        /// Determines whether the ColumnCollection contains a specific column.
        /// </summary>
        /// <param name="item">The column object to locate in the ColumnCollection.</param>
        /// <returns>
        /// <para>true : If column found in columnCollection</para>
        /// <para>false : If column found not in columnCollection</para>
        /// </returns>
        public bool Contains(CacheConfiguration item)
        {
            return this._internalCollection.Contains(item);
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
        public void CopyTo(CacheConfiguration[] array, int arrayIndex)
        {
            this._internalCollection.CopyTo(array, arrayIndex);
        }

        #endregion

        #region IEnumerable<EntityCacheConfiguration> Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntityCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<CacheConfiguration> GetEnumerator()
        {
            return this._internalCollection.GetEnumerator();
        }

        #endregion IEnumerable<EntityCacheConfiguration> Members

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a ColumnCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._internalCollection.GetEnumerator();
        }

        #endregion IEnumerable Members

    }
}
