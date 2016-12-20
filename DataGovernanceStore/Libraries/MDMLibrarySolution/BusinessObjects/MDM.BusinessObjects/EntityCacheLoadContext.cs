using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;

    /// <summary>
    /// Represents the cache status load context of an entity. This is used in activity log table to specify the cache reload request.
    /// </summary>
    public class EntityCacheLoadContext : MDMObject
    {
        #region Fields

        /// <summary>
        /// Field holds the EntityCacheLoadContextItemCollection, based on which the entities are to picked up for cache reload.
        /// </summary>
        private Collection<EntityCacheLoadContextItemCollection> _entityCacheLoadContextItemCollectionList = new Collection<EntityCacheLoadContextItemCollection>();

        /// <summary>
        /// Field holds the Cache status which is to be updated in the entities cache status table. 
        /// </summary>
        private Int32 _cacheStatus = 0;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of the EntityCacheStatusLoadContext.
        /// </summary>
        public EntityCacheLoadContext() : base()
        {            
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Field holds the Cache status which is to be updated in the entities cache status table. 
        /// </summary>
        public Int32 CacheStatus
        {
            get
            {
                return this._cacheStatus;
            }
            set
            {
                this._cacheStatus = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the EntityCacheLoadContextItemCollection into the EntityCacheLoadContext.
        /// </summary>
        /// <param name="entityCacheLoadContextItemCollection"></param>
        public void Add(EntityCacheLoadContextItemCollection entityCacheLoadContextItemCollection)
        {
            if (entityCacheLoadContextItemCollection == null)
            {
                throw new ArgumentNullException("EntityCacheLoadContextItemCollection is null");
            }

            this._entityCacheLoadContextItemCollectionList.Add(entityCacheLoadContextItemCollection);
        }

        /// <summary>
        /// Get XML representation of Entity Cache Load Context object
        /// </summary>
        /// <returns>XML representation of Entity Cache Load Context object</returns>
        public override String ToXml()
        {
            String entityCacheLoadContext = String.Empty;

            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter))
                {
                    xmlWriter.WriteStartElement("EntityCacheLoadContext");

                    xmlWriter.WriteAttributeString("CacheStatus", this.CacheStatus.ToString());

                    if (this._entityCacheLoadContextItemCollectionList != null)
                    {
                        foreach (EntityCacheLoadContextItemCollection entityCacheLoadContextItemCollection in this._entityCacheLoadContextItemCollectionList)
                        {
                            xmlWriter.WriteRaw(entityCacheLoadContextItemCollection.ToXml());
                        }
                    }

                    //EntityCacheStatusLoadContext node end
                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();

                    //get the actual XML
                    entityCacheLoadContext = stringWriter.ToString();
                }
            }

            return entityCacheLoadContext;
        }

        #endregion
    }    
}
