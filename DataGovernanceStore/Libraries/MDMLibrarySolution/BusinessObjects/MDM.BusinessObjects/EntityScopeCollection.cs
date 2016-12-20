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
    /// Specifies the EntityScope Instance Collection for the Object
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class EntityScopeCollection : ICollection<EntityScope>, IEnumerable<EntityScope>, IEntityScopeCollection
    {
        #region Fields

        [DataMember]
        [ProtoMember(1)]
        private Collection<EntityScope> _entityScopes = new Collection<EntityScope>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public EntityScopeCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">XML having xml values</param>
        public EntityScopeCollection(String valuesAsXml)
        {
            LoadEntityScopeCollection(valuesAsXml);
        }

        /// <summary>
        /// Initialize EntityScopeCollection from IList of value
        /// </summary>
        /// <param name="entityScopeList">List of EntityScope object</param>
        public EntityScopeCollection(IList<EntityScope> entityScopeList)
        {
            this._entityScopes = new Collection<EntityScope>(entityScopeList);
        }

        #endregion
       
        #region Methods

        #region Public Methods

        /// <summary>
        /// Get Xml representation of EntityContextCollection object
        /// </summary>
        /// <returns>Xml string representing the EntityContextCollection</returns>
        public String ToXml()
        {
            String entityScopesXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (EntityScope entityScope in this._entityScopes)
            {
                builder.Append(entityScope.ToXml());
            }

            entityScopesXml = String.Format("<EntityScopes>{0}</EntityScopes>", builder.ToString());
            return entityScopesXml;
        }

        /// <summary>
        /// Gets Unique list of entity ids requested in entity scope collection
        /// </summary>
        /// <returns>Entity Id Collection</returns>
        public Collection<Int64> GetRequestedEntityIdList()
        {
            Collection<Int64> entityIdList = new Collection<Int64>();

            if (this._entityScopes != null)
            {
                foreach (EntityScope scope in this._entityScopes)
                {
                    if (scope.EntityIdList != null && scope.EntityIdList.Count > 0)
                    {
                        foreach (Int64 entityId in scope.EntityIdList)
                        {
                            if (!entityIdList.Contains(entityId))
                            {
                                entityIdList.Add(entityId);
                            }
                        }
                    }
                }
            }

            return entityIdList;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Load current EntityScopeCollection from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for current EntityScopeCollection</param>
        private void LoadEntityScopeCollection(String valuesAsXml)
        {
          
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityScope")
                        {
                            String entityScopeXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(entityScopeXml))
                            {
                                EntityScope entityScope = new EntityScope(entityScopeXml);
                                if (entityScope != null)
                                {
                                    this.Add(entityScope);
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

        #region ICollection<EntityScope> Members

        /// <summary>
        /// Add entityscope object in collection
        /// </summary>
        /// <param name="item">entityscope to add in collection</param>
        public void Add(EntityScope item)
        {
            this._entityScopes.Add(item);
        }


        /// <summary>
        /// Add a list of entityscope objects to the collection.
        /// </summary>
        /// <param name="items">Indicates list of entityscope objects</param>
        public void Add(List<EntityScope> items)
        {
            foreach (EntityScope item in items)
            {
               this.Add(item);
            }
        }

        /// <summary>
        /// Add entityscope in collection
        /// </summary>
        /// <param name="iEntityScope">entityscope to add in collection</param>
        public void Add(IEntityScope iEntityScope)
        {
            if (iEntityScope != null)
            {
                this.Add((EntityScope)iEntityScope);
            }
        }

        /// <summary>
        /// Add EntityScopes in collection
        /// </summary>
        /// <param name="iEntityScopeCollection">EntityScopes to add in collection</param>
        public void AddRange(IEntityScopeCollection iEntityScopeCollection)
        {
            if (iEntityScopeCollection == null)
            {
                throw new ArgumentNullException("iEntityScopeCollection");
            }

            foreach (EntityScope entityScope in iEntityScopeCollection)
            {
                this.Add(entityScope);
            }
        }

        /// <summary>
        /// Add EntityScopes in collection
        /// </summary>
        /// <param name="entityScopes">EntityScopes to add in collection</param>
        public void AddRange(EntityScopeCollection entityScopes)
        {
            this.AddRange((IEntityScopeCollection)entityScopes);
        }

        /// <summary>
        /// Removes all entityscopes from collection
        /// </summary>
        public void Clear()
        {
            this._entityScopes.Clear();
        }

        /// <summary>
        /// Determines whether the EntityScopeCollection contains a specific entityscope.
        /// </summary>
        /// <param name="item">The entityscope object to locate in the EntityScopeCollection.</param>
        /// <returns>
        /// <para>true : If entityscope found in entityScopeCollection</para>
        /// <para>false : If entityscope not found in entityScopeCollection</para>
        /// </returns>
        public bool Contains(EntityScope item)
        {
            return this._entityScopes.Contains(item);
        }

        /// <summary>
        ///  Copies the elements of the EntityScopeCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from EntityScopeCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(EntityScope[] array, int arrayIndex)
        {
            this._entityScopes.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of entityscopes in EntityScopeCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._entityScopes.Count;
            }
        }

        /// <summary>
        /// Check if EntityScopeCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the EntityScopeCollection.
        /// </summary>
        /// <param name="item">The entity object to remove from the EntityScopeCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original EntityScopeCollection</returns>
        public bool Remove(EntityScope item)
        {
            return this._entityScopes.Remove(item);
        }

        #endregion

        #region IEnumerable<EntityScope> Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntityScopeCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<EntityScope> GetEnumerator()
        {
            return this._entityScopes.GetEnumerator();
        }

        #endregion IEnumerable<EntityScope> Members

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntityScopeCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._entityScopes.GetEnumerator();
        }

        #endregion IEnumerable Members

       
        #endregion Methods
    }
}
