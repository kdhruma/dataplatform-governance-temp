using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;

namespace MDM.BusinessObjects.Denorm
{
    using MDM.Interfaces;

    /// <summary>
    /// 
    /// </summary>
    [DataContract] 
    public class EntityProcessorErrorLogCollection : ICollection<EntityProcessorErrorLog>, IEnumerable<EntityProcessorErrorLog>, IEntityProcessorErrorLogCollection
    {
        #region Fields

        [DataMember]
        private Collection<EntityProcessorErrorLog> _entityProcessorErrorLogs = new Collection<EntityProcessorErrorLog>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public EntityProcessorErrorLogCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public EntityProcessorErrorLogCollection(String valueAsXml)
        {
            LoadEntityProcessorErrorLog(valueAsXml);
        }

        /// <summary>
        /// Initialize EntityProcessorErrorLogCollection from IList
        /// </summary>
        /// <param name="ImpactedEntityErrorLogList">IList of EntityProcessorErrorLog</param>
        public EntityProcessorErrorLogCollection(IList<EntityProcessorErrorLog> ImpactedEntityErrorLogList)
        {
            this._entityProcessorErrorLogs = new Collection<EntityProcessorErrorLog>(ImpactedEntityErrorLogList);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Find EntityProcessorErrorLog from EntityProcessorErrorLogCollection based on Id
        /// </summary>
        /// <param name="Id">Id to search in result collection</param>
        /// <returns>EntityProcessorErrorLog object having given Id</returns>
        public EntityProcessorErrorLog this[Int64 Id]
        {
            get
            {
                EntityProcessorErrorLog entityProcessorErrorLog = GetEntityProcessorErrorLog(Id);
                if (entityProcessorErrorLog == null)
                    throw new ArgumentException(String.Format("No result found for id: {0}", Id), "Id");
                else
                    return entityProcessorErrorLog;
            }
            set
            {
                EntityProcessorErrorLog entityProcessorErrorLog = GetEntityProcessorErrorLog(Id);
                if (entityProcessorErrorLog == null)
                    throw new ArgumentException(String.Format("No result found for  id: {0}", Id), "Id");

                entityProcessorErrorLog = value;
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
            if (obj is EntityProcessorErrorLogCollection)
            {
                EntityProcessorErrorLogCollection objectToBeCompared = obj as EntityProcessorErrorLogCollection;
                Int32 impactedEntityErrorLogUnion = this._entityProcessorErrorLogs.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 impactedEntityErrorLogIntesect = this._entityProcessorErrorLogs.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if (impactedEntityErrorLogUnion != impactedEntityErrorLogIntesect)
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
            foreach (EntityProcessorErrorLog attr in this._entityProcessorErrorLogs)
            {
                hashCode += attr.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        public void LoadEntityProcessorErrorLog(String valuesAsXml)
        {
            #region Sample Xml
            /*
             * <DenormResults></DenormResults>
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityProcessorErrorLog")
                        {
                            String entityProcessorErrorLogXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(entityProcessorErrorLogXml))
                            {
                                EntityProcessorErrorLog entityProcessorErrorLog = new EntityProcessorErrorLog(entityProcessorErrorLogXml);
                                this.Add(entityProcessorErrorLog);
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

        #endregion

        #region Private Methods

        private EntityProcessorErrorLog GetEntityProcessorErrorLog(Int64 Id)
        {
            var filteredEntityProcessorErrorLog = from entityProcessorErrorLog in this._entityProcessorErrorLogs
                                       where entityProcessorErrorLog.Id == Id
                                       select entityProcessorErrorLog;

            if (filteredEntityProcessorErrorLog.Any())
                return filteredEntityProcessorErrorLog.First();
            else
                return null;
        }

        #endregion

        #region ICollection<EntityProcessorErrorLog> Members

        /// <summary>
        /// Add EntityProcessorErrorLog object in collection
        /// </summary>
        /// <param name="item">EntityProcessorErrorLog to add in collection</param>
        public void Add(EntityProcessorErrorLog item)
        {
            this._entityProcessorErrorLogs.Add(item);
        }

        /// <summary>
        /// Removes all EntityProcessorErrorLog from collection
        /// </summary>
        public void Clear()
        {
            this._entityProcessorErrorLogs.Clear();
        }

        /// <summary>
        /// Determines whether the EntityProcessorErrorLogCollection contains a specific EntityProcessorErrorLog.
        /// </summary>
        /// <param name="item">The EntityProcessorErrorLog object to locate in the EntityProcessorErrorLogCollection.</param>
        /// <returns>
        /// <para>true : If EntityProcessorErrorLog found in mappingCollection</para>
        /// <para>false : If EntityProcessorErrorLog found not in mappingCollection</para>
        /// </returns>
        public bool Contains(EntityProcessorErrorLog item)
        {
            return this._entityProcessorErrorLogs.Contains(item);
        }

        /// <summary>
        /// Determines whether the EntityProcessorErrorLogCollection contains a specific entityProcessorErrorLog based on Id.
        /// </summary>
        /// <param name="id">The Id locate in the EntityProcessorErrorLogCollection.</param>
        /// <returns>
        /// <para>true : If Id found in mappingCollection</para>
        /// <para>false : If Id found not in mappingCollection</para>
        /// </returns>
        public bool Contains(Int64 id)
        {
            if (GetEntityProcessorErrorLog(id) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Copies the elements of the EntityProcessorErrorLogCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from EntityProcessorErrorLogCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(EntityProcessorErrorLog[] array, int arrayIndex)
        {
            this._entityProcessorErrorLogs.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of EntityProcessorErrorLog in EntityProcessorErrorLogCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._entityProcessorErrorLogs.Count;
            }
        }

        /// <summary>
        /// Check if EntityProcessorErrorLogCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the EntityProcessorErrorLogCollection.
        /// </summary>
        /// <param name="item">The EntityProcessorErrorLog object to remove from the EntityProcessorErrorLogCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original EntityProcessorErrorLogCollection</returns>
        public bool Remove(EntityProcessorErrorLog item)
        {
            return this._entityProcessorErrorLogs.Remove(item);
        }

        #endregion

        #region IEnumerable<EntityProcessorErrorLog> Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntityProcessorErrorLogCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<EntityProcessorErrorLog> GetEnumerator()
        {
            return this._entityProcessorErrorLogs.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntityProcessorErrorLogCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._entityProcessorErrorLogs.GetEnumerator();
        }

        #endregion

        #region XML Methods

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of EntityProcessorErrorLogCollection object
        /// </summary>
        /// <returns>Xml string representing the EntityProcessorErrorLogCollection</returns>
        public String ToXml()
        {
            String ImpactedEntityErrorLogCollectionXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (EntityProcessorErrorLog entityProcessorErrorLog in this._entityProcessorErrorLogs)
            {
                builder.Append(entityProcessorErrorLog.ToXml());
            }

            ImpactedEntityErrorLogCollectionXml = String.Format("<EntityProcessorErrorLogCollection>{0}</EntityProcessorErrorLogCollection>", builder.ToString());
            return ImpactedEntityErrorLogCollectionXml;
        }

        #endregion ToXml methods

        #endregion
    }
}
