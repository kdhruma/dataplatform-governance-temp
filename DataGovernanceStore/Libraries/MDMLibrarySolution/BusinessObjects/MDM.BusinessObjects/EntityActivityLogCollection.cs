using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;

    /// <summary>
    /// Represents class for entity activity log collection
    /// </summary>
    [DataContract]
    public class EntityActivityLogCollection : ICollection<EntityActivityLog>, IEnumerable<EntityActivityLog>, IEntityActivityLogCollection
    {
        #region Fields

        [DataMember]
        private Collection<EntityActivityLog> _entityActivityLogCollection = new Collection<EntityActivityLog>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public EntityActivityLogCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public EntityActivityLogCollection(String valueAsXml)
        {
            LoadEntityActivityLogCollection(valueAsXml);
        }

        /// <summary>
        /// Initialize EntityActivityLogCollection from IList
        /// </summary>
        /// <param name="entityActivityLogList">IList of denorm result</param>
        public EntityActivityLogCollection(IList<EntityActivityLog> entityActivityLogList)
        {
            this._entityActivityLogCollection = new Collection<EntityActivityLog>(entityActivityLogList);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Find EntityActivityLog from EntityActivityLogCollection based on Id
        /// </summary>
        /// <param name="Id">Id to search in result collection</param>
        /// <returns>EntityActivityLog object having given Id</returns>
        public EntityActivityLog this[Int64 Id]
        {
            get
            {
                EntityActivityLog entityActivityLog = GetEntityActivityLog(Id);
                if (entityActivityLog == null)
                    throw new ArgumentException(String.Format("No result found for id: {0}", Id), "Id");
                else
                    return entityActivityLog;
            }
            set
            {
                EntityActivityLog entityActivityLog = GetEntityActivityLog(Id);
                if (entityActivityLog == null)
                    throw new ArgumentException(String.Format("No result found for  id: {0}", Id), "Id");

                entityActivityLog = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is EntityActivityLogCollection)
            {
                EntityActivityLogCollection objectToBeCompared = obj as EntityActivityLogCollection;
                Int32 entityActivityLogUnion = this._entityActivityLogCollection.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 entityActivityLogIntersect = this._entityActivityLogCollection.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if (entityActivityLogUnion != entityActivityLogIntersect)
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
            foreach (EntityActivityLog attr in this._entityActivityLogCollection)
            {
                hashCode += attr.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        public void LoadEntityActivityLogCollection(String valuesAsXml)
        {
            #region Sample Xml
            /*
             * <EntityActivityLog></EntityActivityLog>
             */
            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityActivityLog")
                        {
                            String EntityActivityLogXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(EntityActivityLogXml))
                            {
                                EntityActivityLog entityActivityLog = new EntityActivityLog(EntityActivityLogXml);
                                this.Add(entityActivityLog);
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

        /// <summary>
        /// Get the EntityActivityLog based on the EntityId
        /// </summary>
        /// <param name="entityId">entity Id</param>
        /// <returns>list of entityActivityLog for the specifed entityId</returns>
        public List<EntityActivityLog> GetEntityActivityLogByEntityId(Int64 entityId)
        {
            var filteredEntityActivityLog = from entityActivityLog in this._entityActivityLogCollection
                                            where entityActivityLog.EntityId == entityId
                                            select entityActivityLog;

            if (filteredEntityActivityLog.Any())
                return filteredEntityActivityLog.ToList<EntityActivityLog>();
            else
                return null;

        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Get the EntityActivityLog based on the unique Id
        /// </summary>
        /// <param name="Id">pk of EntityActivityLog</param>
        /// <returns></returns>
        private EntityActivityLog GetEntityActivityLog(Int64 Id)
        {
            var filteredEntityActivityLog = from entityActivityLog in this._entityActivityLogCollection
                                            where entityActivityLog.Id == Id
                                            select entityActivityLog;

            if (filteredEntityActivityLog.Any())
                return filteredEntityActivityLog.First();
            else
                return null;
        }

        #endregion

        #region ICollection<EntityActivityLog> Members

        /// <summary>
        /// Add EntityActivityLog object in collection
        /// </summary>
        /// <param name="item">EntityActivityLog to add in collection</param>
        public void Add(EntityActivityLog item)
        {
            this._entityActivityLogCollection.Add(item);
        }

        /// <summary>
        /// Removes all EntityActivityLog from collection
        /// </summary>
        public void Clear()
        {
            this._entityActivityLogCollection.Clear();
        }

        /// <summary>
        /// Determines whether the EntityActivityLogCollection contains a specific EntityActivityLog.
        /// </summary>
        /// <param name="item">The EntityActivityLog object to locate in the EntityActivityLogCollection.</param>
        /// <returns>
        /// <para>true : If EntityActivityLog found in mappingCollection</para>
        /// <para>false : If EntityActivityLog found not in mappingCollection</para>
        /// </returns>
        public bool Contains(EntityActivityLog item)
        {
            return this._entityActivityLogCollection.Contains(item);
        }

        /// <summary>
        /// Determines whether the EntityActivityLogCollection contains a specific entityActivityLog based on Id.
        /// </summary>
        /// <param name="id">The Id locate in the EntityActivityLogCollection.</param>
        /// <returns>
        /// <para>true : If Id found in mappingCollection</para>
        /// <para>false : If Id found not in mappingCollection</para>
        /// </returns>
        public bool Contains(Int64 id)
        {
            if (GetEntityActivityLogByEntityId(id) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Copies the elements of the EntityActivityLogCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from EntityActivityLogCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(EntityActivityLog[] array, int arrayIndex)
        {
            this._entityActivityLogCollection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of EntityActivityLog in EntityActivityLogCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._entityActivityLogCollection.Count;
            }
        }

        /// <summary>
        /// Check if EntityActivityLogCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the EntityActivityLogCollection.
        /// </summary>
        /// <param name="item">The denorm result object to remove from the EntityActivityLogCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original EntityActivityLogCollection</returns>
        public bool Remove(EntityActivityLog item)
        {
            return this._entityActivityLogCollection.Remove(item);
        }

        #endregion

        #region IEnumerable<EntityActivityLog> Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntityActivityLogCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<EntityActivityLog> GetEnumerator()
        {
            return this._entityActivityLogCollection.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntityActivityLogCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._entityActivityLogCollection.GetEnumerator();
        }

        #endregion

        #region XML Methods

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of EntityActivityLogCollection object
        /// </summary>
        /// <returns>Xml string representing the EntityActivityLogCollection</returns>
        public String ToXml()
        {
            String entityActivityLogsXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (EntityActivityLog entityActivityLog in this._entityActivityLogCollection)
            {
                builder.Append(entityActivityLog.ToXml());
            }

            entityActivityLogsXml = String.Format("<EntityActivityLogs>{0}</EntityActivityLogs>", builder.ToString());
            return entityActivityLogsXml;
        }

        #endregion ToXml methods

        #endregion

    }
}
