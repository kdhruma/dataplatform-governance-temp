using System;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies Entity Audit Collection
    /// </summary>
    [DataContract]
    public class EntityAuditInfoCollection : ICollection<EntityAuditInfo>, IEnumerable<EntityAuditInfo>, IEntityAuditInfoCollection
    {
        #region Fields

        /// <summary>
        /// Field denoting collection of Entity Audit
        /// </summary>
        [DataMember]
        private Collection<EntityAuditInfo> _entityAuditInfoCollection = new Collection<EntityAuditInfo>();

        #endregion

        #region Properties

        /// <summary>
        /// Find entity audit from EntityAuditInfoCollection based on entityId
        /// </summary>
        /// <param name="entityAuditId">entityId to search in entity Audit Collection</param>
        /// <returns>EntityAuditInfo object having given entityId</returns>
        public EntityAuditInfo this[Int64 entityAuditId]
        {
            get
            {
                EntityAuditInfo entityAuditInfo = GetEntityAuditInfo(entityAuditId);
                if(entityAuditInfo == null)
                    throw new ArgumentException(String.Format("No entity audit info found for audit id: {0}", entityAuditId), "entityAuditId");

                return entityAuditInfo;
            }
            set
            {
                EntityAuditInfo entityAuditInfo = GetEntityAuditInfo(entityAuditId);
                if(entityAuditInfo == null)
                    throw new ArgumentException(String.Format("No entity audit info found for audit id: {0}", entityAuditId), "entityAuditId");

                entityAuditInfo = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the entity audit collection class
        /// </summary>
        public EntityAuditInfoCollection()
        {

        }

        /// <summary>
        /// Constructor which takes a list of Entity Audit Info as input
        /// </summary>
        /// <param name="entityAuditInfoList"></param>
        public EntityAuditInfoCollection(IList<EntityAuditInfo> entityAuditInfoList)
        {
            if (entityAuditInfoList != null)
            {
                this._entityAuditInfoCollection = new Collection<EntityAuditInfo>(entityAuditInfoList);
            }
        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">XML having xml values</param>
        public EntityAuditInfoCollection(String valuesAsXml)
        {
            LoadEntityAuditInfoCollection(valuesAsXml);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Check if EntityAuditInfoCollection contains entity audit with given entity audit id
        /// </summary>
        /// <param name="entityAuditId">Id of the entity audit using which entity audit is to be searched from collection</param>
        /// <returns>
        /// <para>true : If entity audit found in EntityAuditInfoCollection</para>
        /// <para>false : If entity audit found not in EntityAuditInfoCollection</para>
        /// </returns>
        public Boolean Contains(Int64 entityAuditId)
        {
            if(GetEntityAuditInfo(entityAuditId) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Remove entityAuditInfo object from EntityAuditInfoCollection
        /// </summary>
        /// <param name="entityAuditId">Id of the entity audit using which Entity Audit is to be removed from collection</param>
        /// <returns>true if entity audit is successfully removed; otherwise, false. This method also returns false if entity audit was not found in the original collection</returns>
        public Boolean Remove(Int32 entityAuditId)
        {
            EntityAuditInfo entityAuditInfo = GetEntityAuditInfo(entityAuditId);

            if(entityAuditInfo == null)
                throw new ArgumentException("No entity audit found for given entity audit id");
            else
                return this.Remove(entityAuditInfo);
        }

        /// <summary>
        /// Get Xml representation of Entity audit info Collection
        /// </summary>
        /// <returns>Xml representation of Entity audit info Collection</returns>
        public String ToXml()
        {
            /*
             * Sample XML:
              <EntityAuditInfoCollection>
                <EntityAuditInfo Id="001" EntityId = "1001" AttributeId = "4001" ProgramName="BusinessRule.100.1" UserLogin="cfadmin" ChangeDateTime="" Action="Read" Locale="en-WW">    
                </EntityAuditInfo>
              </EntityAuditInfoCollection>
             */
            String returnXml = String.Empty;

            returnXml = "<EntityAuditInfoCollection>";

            if(this._entityAuditInfoCollection != null && this._entityAuditInfoCollection.Count > 0)
            {
                foreach(EntityAuditInfo entityAuditInfo in this._entityAuditInfoCollection)
                {
                    returnXml = String.Concat(returnXml, entityAuditInfo.ToXml());
                }
            }

            returnXml = String.Concat(returnXml, "</EntityAuditInfoCollection>");

            return returnXml;
        }

        /// <summary>
        /// Get Xml representation of entity audit info collection based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of entity audit info collection</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            /*
             * Sample XML:
              <EntityAuditInfoCollection>
                <EntityAuditInfo Id="001" EntityId = "1001" AttributeId = "4001" ProgramName="BusinessRule.100.1" UserLogin="cfadmin" ChangeDateTime="" Action="Read" Locale="en-WW">    
                </EntityAuditInfo>
              </EntityAuditInfoCollection>
             */
            String returnXml = String.Empty;

            returnXml = "<EntityAuditInfoCollection>";

            if(this._entityAuditInfoCollection != null && this._entityAuditInfoCollection.Count > 0)
            {
                foreach(EntityAuditInfo entityAuditInfo in this._entityAuditInfoCollection)
                {
                    returnXml = String.Concat(returnXml, entityAuditInfo.ToXml(objectSerialization));
                }
            }

            returnXml = String.Concat(returnXml, "</EntityAuditInfoCollection>");

            return returnXml;
        }

        /// <summary>
        /// Method to get the entity audit info based on id
        /// </summary>
        /// <param name="entityAuditId"></param>
        /// <returns></returns>
        public EntityAuditInfo GetEntityAuditInfo(Int64 entityAuditId)
        {
            var filteredEntityAuditInfos = from entityAuditInfo in this._entityAuditInfoCollection
                                                 where entityAuditInfo.Id == entityAuditId
                                                 select entityAuditInfo;

            if(filteredEntityAuditInfos.Any())
                return filteredEntityAuditInfos.First();
            else
                return null;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Load current EntityAuditInfoCollection from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for current EntityAuditInfoCollection 
        /// </param>
        public void LoadEntityAuditInfoCollection(String valuesAsXml)
        {
            #region Sample Xml
            /*
             * Sample XML:
              <EntityAuditInfoCollection>
                <EntityAuditInfo Id="001" EntityId = "1001" AttributeId = "4001" ProgramName="BusinessRule.100.1" UserLogin="cfadmin" ChangeDateTime="" Action="Read" Locale="en-WW">    
                </EntityAuditInfo>
              </EntityAuditInfoCollection>
             */
            #endregion Sample Xml

            if(!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while(!reader.EOF)
                    {
                        if(reader.NodeType == XmlNodeType.Element && reader.Name == "EntityAuditInfoCollection")
                        {
                            String entityAuditInfoXml = reader.ReadOuterXml();
                            if(!String.IsNullOrEmpty(entityAuditInfoXml))
                            {
                                EntityAuditInfo entityAuditInfo = new EntityAuditInfo(entityAuditInfoXml);
                                if(entityAuditInfo != null)
                                {
                                    this.Add(entityAuditInfo);
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
                    if(reader != null)
                    {
                        reader.Close();
                    }
                }
            }
        }

        #endregion

        #region ICollection<EntityAuditInfo> Members

        /// <summary>
        /// Add entity audit object in collection
        /// </summary>
        /// <param name="entityAuditInfo">Entity audit to add in collection</param>
        public void Add(EntityAuditInfo entityAuditInfo)
        {
            this._entityAuditInfoCollection.Add(entityAuditInfo);
        }

        /// <summary>
        /// Removes all entity audit from collection
        /// </summary>
        public void Clear()
        {
            this._entityAuditInfoCollection.Clear();
        }

        /// <summary>
        /// Determines whether the EntityAuditInfoCollection contains a specific entity audit
        /// </summary>
        /// <param name="entityAuditInfo">The entity audit object to locate in the EntityAuditInfoCollection.</param>
        /// <returns>
        /// <para>true : If entity audit found in EntityAuditInfoCollection</para>
        /// <para>false : If entity audit found not in EntityAuditInfoCollection</para>
        /// </returns>
        public Boolean Contains(EntityAuditInfo entityAuditInfo)
        {
            return this._entityAuditInfoCollection.Contains(entityAuditInfo);
        }

        /// <summary>
        /// Copies the elements of the EntityAuditInfoCollection to a
        /// System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from EntityAuditInfoCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(EntityAuditInfo[] array, Int32 arrayIndex)
        {
            this._entityAuditInfoCollection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of entity audit info in EntityAuditInfoCollection
        /// </summary>
        public Int32 Count
        {
            get
            {
                return this._entityAuditInfoCollection.Count;
            }
        }

        /// <summary>
        /// Check if EntityAuditInfoCollection is read-only.
        /// </summary>
        public Boolean IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific entity audit from the EntityAuditInfoCollection.
        /// </summary>
        /// <param name="entityAuditInfo">The entity audit object to remove from the EntityAuditInfoCollection.</param>
        /// <returns>true if entity audit is successfully removed; otherwise, false. This method also returns false if entity audit was not found in the original collection</returns>
        public Boolean Remove(EntityAuditInfo entityAuditInfo)
        {
            return this._entityAuditInfoCollection.Remove(entityAuditInfo);
        }

        #endregion

        #region IEnumerable<EntityAuditInfo> Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntityAuditInfoCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<EntityAuditInfo> GetEnumerator()
        {
            return this._entityAuditInfoCollection.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntityAuditInfoCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ( IEnumerator ) this._entityAuditInfoCollection.GetEnumerator();
        }

        #endregion
    }
}
