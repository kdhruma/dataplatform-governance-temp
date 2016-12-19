using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;
    using ProtoBuf;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;
    using System.Xml;

    /// <summary>
    /// Specifies the EntityContext Instance Collection for the Object
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class EntityContextCollection : ICollection<EntityContext>, IEnumerable<EntityContext>, IEntityContextCollection, IEntityDataContextCollection
    {
        #region Fields

        [DataMember]
        [ProtoMember(1)]
        private Collection<EntityContext> _entityContexts = new Collection<EntityContext>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public EntityContextCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">XML having xml values</param>
        public EntityContextCollection(String valuesAsXml)
        {
            LoadEntityContextCollection(valuesAsXml);
        }

        /// <summary>
        /// Initialize EntityContextCollection from IList of value
        /// </summary>
        /// <param name="entityContextList">List of EntityContext object</param>
        public EntityContextCollection(IList<EntityContext> entityContextList)
        {
            this._entityContexts = new Collection<EntityContext>(entityContextList);
        }

        #endregion

        #region Properties
        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get Xml representation of EntityContextCollection object
        /// </summary>
        /// <returns>Xml string representing the EntityContextCollection</returns>
        public String ToXml()
        {
            String entityContextsXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (EntityContext entityContext in this._entityContexts)
            {
                builder.Append(entityContext.ToXml());
            }

            entityContextsXml = String.Format("<EntityContexts>{0}</EntityContexts>", builder.ToString());
            return entityContextsXml;
        }

        /// <summary>
        /// Gets CallDataContext from all entity context
        /// </summary>
        /// <returns>Consolidated Call Data Context</returns>
        public CallDataContext GetCallDataContext()
        {
            CallDataContext callDataContext = null;

            if (_entityContexts != null && _entityContexts.Count > 0)
            {
                callDataContext = new CallDataContext();

                foreach (EntityContext entityContext in _entityContexts)
                {
                    if (entityContext.ContainerId > 0)
                    {
                        if (!callDataContext.ContainerIdList.Contains(entityContext.ContainerId))
                        {
                            callDataContext.ContainerIdList.Add(entityContext.ContainerId);
                        }
                    }

                    if (entityContext.EntityTypeId > 0)
                    {
                        if (!callDataContext.EntityTypeIdList.Contains(entityContext.EntityTypeId))
                        {
                            callDataContext.EntityTypeIdList.Add(entityContext.EntityTypeId);
                        }
                    }

                    if (entityContext.RelationshipContext != null && entityContext.RelationshipContext.RelationshipTypeIdList != null && entityContext.RelationshipContext.RelationshipTypeIdList.Count > 0)
                    {
                        foreach (Int32 relationshipTypeId in entityContext.RelationshipContext.RelationshipTypeIdList)
                        {
                            if (!callDataContext.RelationshipTypeIdList.Contains(relationshipTypeId))
                            {
                                callDataContext.RelationshipTypeIdList.Add(relationshipTypeId);
                            }
                        }
                    }

                    if (entityContext.CategoryId > 0)
                    {
                        if (!callDataContext.CategoryIdList.Contains(entityContext.CategoryId))
                        {
                            callDataContext.CategoryIdList.Add(entityContext.CategoryId);
                        }
                    }

                    if (entityContext.DataLocales.Count > 0)
                    {
                        foreach (LocaleEnum locale in entityContext.DataLocales)
                        {
                            if (!callDataContext.LocaleList.Contains(locale))
                            {
                                callDataContext.LocaleList.Add(locale);
                            }
                        }
                    }
                }
            }

            return callDataContext;
        }

        /// <summary>
        /// Gets all distinct list of entity types ids in this entity context collection
        /// </summary>
        /// <returns>Entity Type Id List</returns>
        public Collection<Int32> GetEntityTypeIdList()
        {
            var entityTypeIdList = new Collection<Int32>();

            if (_entityContexts != null && _entityContexts.Count > 0)
            {
                foreach (EntityContext entityContext in _entityContexts)
                {
                    if (!entityTypeIdList.Contains(entityContext.EntityTypeId))
                    {
                        entityTypeIdList.Add(entityContext.EntityTypeId);
                    }
                }
            }

            return entityTypeIdList;
        }

        /// <summary>
        /// Add entity data context into entity context collection
        /// </summary>
        /// <param name="entityDataContext">Indicates entity data context to be added</param>
        public void Add(IEntityDataContext entityDataContext)
        {
            Add((EntityContext)entityDataContext);
        }

        /// <summary>
        /// Adds entity data context collection
        /// </summary>
        /// <param name="entityDataContextCollection">Indicates entity data context collection to be added</param>
        public void AddRange(IEntityDataContextCollection entityDataContextCollection)
        {
            AddRange((EntityContextCollection)entityDataContextCollection);
        }

        /// <summary>
        /// Create a new entity contexts object.
        /// </summary>
        /// <returns>New entity contexts instance</returns>
        public IEntityContextCollection Clone()
        {
            EntityContextCollection clonedEntityContexts = new EntityContextCollection();

            foreach (EntityContext entityContext in this._entityContexts)
            {
                clonedEntityContexts.Add(entityContext.Clone());
            }

            return clonedEntityContexts;
        }

        #region IEntityContext related methods

        /// <summary>
        /// Gets entity context instance by entity type id from current collection
        /// </summary>
        /// <param name="entityTypeId">Specifies entity type id</param>
        /// <returns>Entity Context</returns>
        public IEntityContext GetByEntityTypeId(Int32 entityTypeId)
        {
            if (_entityContexts != null && _entityContexts.Count > 0)
            {
                foreach (EntityContext entityContext in _entityContexts)
                {
                    if (entityContext.EntityTypeId == entityTypeId)
                    {
                        return entityContext;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets entity context instance by entity type name from current collection
        /// </summary>
        /// <param name="entityTypeName">Specifies entity type name</param>
        /// <returns>Entity Context</returns>
        public IEntityContext GetByEntityTypeName(String entityTypeName)
        {
            if (_entityContexts != null && _entityContexts.Count > 0)
            {
                foreach (EntityContext entityContext in _entityContexts)
                {
                    if (entityContext.EntityTypeName.Equals(entityTypeName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return entityContext;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets entity context instance by container and category id from current collection
        /// </summary>
        /// <param name="containerId">Specifies container id</param>
        /// <param name="categoryId">Specifies category id</param>
        /// <returns>Entity Context</returns>
        public IEntityContext GetByContainerAndCategoryId(Int32 containerId, Int64 categoryId)
        {
            if (_entityContexts.Count > 0)
            {
                foreach (EntityContext entityContext in _entityContexts)
                {
                    if (entityContext.ContainerId == containerId && entityContext.CategoryId == categoryId)
                    {
                        return entityContext;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets entity context instance by container name and category path from current collection
        /// </summary>
        /// <param name="containerName">Specifies container name</param>
        /// <param name="categoryPath">Specifies category path</param>
        /// <returns>Entity Context</returns>
        public IEntityContext GetByContainerNameAndCategoryPath(String containerName, String categoryPath)
        {
            if (_entityContexts.Count > 0)
            {
                foreach (EntityContext entityContext in _entityContexts)
                {
                    if (String.Compare(entityContext.ContainerName, containerName, true) == 0 &&
                        String.Compare(entityContext.CategoryPath, categoryPath, true) == 0)
                    {
                        return entityContext;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets entity context instance by container qualifier id from current collection
        /// </summary>
        /// <param name="containerQualifierId">Specifies container qualifier id</param>
        /// <returns>Entity Context</returns>
        public IEntityContext GetByContainerQualifierId(Int32 containerQualifierId)
        {
            if (this._entityContexts.Count > 0)
            {
                foreach (EntityContext entityContext in this._entityContexts)
                {
                    if (entityContext.ContainerQualifierId == containerQualifierId)
                    {
                        return entityContext;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets entity context instance by container qualifier name from current collection
        /// </summary>
        /// <param name="containerQualifierName">Specifies container qualifier name</param>
        /// <returns>Entity Context</returns>
        public IEntityContext GetByContainerQualifierName(String containerQualifierName)
        {
            if (this._entityContexts.Count > 0)
            {
                foreach (EntityContext entityContext in this._entityContexts)
                {
                    if (String.Compare(entityContext.ContainerQualifierName, containerQualifierName, true) == 0)
                    {
                        return entityContext;
                    }
                }
            }

            return null;
        }

        #endregion IEntityContext related methods

        #region IEntity Data Context related methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityTypeId"></param>
        /// <returns></returns>
        IEntityDataContext IEntityDataContextCollection.GetByEntityTypeId(Int32 entityTypeId)
        {
            return (IEntityDataContext)GetByEntityTypeId(entityTypeId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityTypeName"></param>
        /// <returns></returns>
        IEntityDataContext IEntityDataContextCollection.GetByEntityTypeName(String entityTypeName)
        {
            return (IEntityDataContext)GetByEntityTypeName(entityTypeName);
        }

        /// <summary>
        /// Gets entity context instance by container id and category id from current collection
        /// </summary>
        /// <param name="containerId">Specifies container id</param>
        /// <param name="categoryId">Specifies category id</param>
        /// <returns>Entity Context</returns>
        IEntityDataContext IEntityDataContextCollection.GetByContainerAndCategoryId(Int32 containerId, Int64 categoryId)
        {
            return (IEntityDataContext)GetByContainerAndCategoryId(containerId, categoryId);
        }

        /// <summary>
        /// Gets entity context instance by container name and category path from current collection
        /// </summary>
        /// <param name="containerName">Specifies container name</param>
        /// <param name="categoryPath">Specifies category path</param>
        /// <returns>Entity Context</returns>
        IEntityDataContext IEntityDataContextCollection.GetByContainerNameAndCategoryPath(String containerName, String categoryPath)
        {
            return (IEntityDataContext)GetByContainerNameAndCategoryPath(containerName, categoryPath);
        }

        /// <summary>
        /// Gets entity context instance by container qualifier id from current collection
        /// </summary>
        /// <param name="containerQualifierId">Specifies container qualifier id</param>
        /// <returns>Entity Context</returns>
        IEntityDataContext IEntityDataContextCollection.GetByContainerQualifierId(Int32 containerQualifierId)
        {
            return (IEntityDataContext)GetByContainerQualifierId(containerQualifierId);
        }

        /// <summary>
        /// Gets entity context instance by container qualifier name from current collection
        /// </summary>
        /// <param name="containerQualifierName">Specifies container qualifier name</param>
        /// <returns>Entity Context</returns>
        IEntityDataContext IEntityDataContextCollection.GetByContainerQualifierName(String containerQualifierName)
        {
            return (IEntityDataContext)GetByContainerQualifierName(containerQualifierName);
        }

        #endregion IEntity Data Context related methods

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Load current EntityContextCollection from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for current EntityContextCollection</param>
        private void LoadEntityContextCollection(String valuesAsXml)
        {

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityContext")
                        {
                            String entityContextXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(entityContextXml))
                            {
                                EntityContext entityContext = new EntityContext(entityContextXml);
                                if (entityContext != null)
                                {
                                    this.Add(entityContext);
                                }
                            }
                        }
                        else
                        {
                            reader.Read();
                        }
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }
        }

        #endregion Private Methods

        #region ICollection<EntityContext> Members

        /// <summary>
        /// Add entitycontext object in collection
        /// </summary>
        /// <param name="item">entitycontext to add in collection</param>
        public void Add(EntityContext item)
        {
            this._entityContexts.Add(item);
        }

        /// <summary>
        /// Add a list of entitycontext objects to the collection.
        /// </summary>
        /// <param name="items">Indicates list of entitycontext objects</param>
        public void Add(List<EntityContext> items)
        {
            foreach (EntityContext item in items)
            {
                this.Add(item);
            }
        }

        /// <summary>
        /// Add entitycontext in collection
        /// </summary>
        /// <param name="iEntityContext">entitycontext to add in collection</param>
        public void Add(IEntityContext iEntityContext)
        {
            if (iEntityContext != null)
            {
                this.Add((EntityContext)iEntityContext);
            }
        }

        /// <summary>
        /// Add EntityContexts in collection
        /// </summary>
        /// <param name="iEntityContextCollection">EntityContexts to add in collection</param>
        public void AddRange(IEntityContextCollection iEntityContextCollection)
        {
            if (iEntityContextCollection == null)
            {
                throw new ArgumentNullException("iEntityContextCollection");
            }

            foreach (EntityContext entityContext in iEntityContextCollection)
            {
                this.Add(entityContext);
            }
        }

        /// <summary>
        /// Add EntityContexts in collection
        /// </summary>
        /// <param name="entityContexts">EntityContexts to add in collection</param>
        public void AddRange(EntityContextCollection entityContexts)
        {
            this.AddRange((IEntityContextCollection)entityContexts);
        }

        /// <summary>
        /// Removes all entitycontexts from collection
        /// </summary>
        public void Clear()
        {
            this._entityContexts.Clear();
        }

        /// <summary>
        /// Determines whether the EntityContextCollection contains a specific EntityContext.
        /// </summary>
        /// <param name="item">The entitycontext object to locate in the EntityContextCollection.</param>
        /// <returns>
        /// <para>true : If entitycontext found in entityContextsCollection</para>
        /// <para>false : If entitycontext not found in entityContextCollection</para>
        /// </returns>
        public bool Contains(EntityContext item)
        {
            return this._entityContexts.Contains(item);
        }

        /// <summary>
        ///  Copies the elements of the EntityContextCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from EntityContextCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(EntityContext[] array, int arrayIndex)
        {
            this._entityContexts.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of entitycontexts in EntityContextCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._entityContexts.Count;
            }
        }

        /// <summary>
        /// Check if EntityContextCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the EntityContextCollection.
        /// </summary>
        /// <param name="item">The entity object to remove from the EntityContextCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original EntityContextCollection</returns>
        public bool Remove(EntityContext item)
        {
            return this._entityContexts.Remove(item);
        }

        #endregion

        #region IEnumerable<EntityContext> Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntityContextCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<EntityContext> GetEnumerator()
        {
            return this._entityContexts.GetEnumerator();
        }

        #endregion IEnumerable<EntityContext> Members

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntityContextCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._entityContexts.GetEnumerator();
        }

        #endregion IEnumerable Members

        #endregion Methods
    }
}