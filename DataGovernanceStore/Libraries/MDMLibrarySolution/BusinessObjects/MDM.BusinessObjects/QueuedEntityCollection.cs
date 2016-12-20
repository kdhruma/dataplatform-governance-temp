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
    /// 
    /// </summary>
    [DataContract]
    public class QueuedEntityCollection : ICollection<QueuedEntity>, IEnumerable<QueuedEntity>, IQueuedEntityCollection
    {
        #region Fields

        [DataMember]
        private Collection<QueuedEntity> _queuedEntityCollection = new Collection<QueuedEntity>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public QueuedEntityCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public QueuedEntityCollection(String valueAsXml)
        {
            LoadQueuedEntityCollection(valueAsXml);
        }

        /// <summary>
        /// Initialize QueuedEntityCollection from IList
        /// </summary>
        /// <param name="queuedEntityList">IList of denorm result</param>
        public QueuedEntityCollection(IList<QueuedEntity> queuedEntityList)
        {
            this._queuedEntityCollection = new Collection<QueuedEntity>(queuedEntityList);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Find QueuedEntity from QueuedEntityCollection based on Id
        /// </summary>
        /// <param name="Id">Id to search in result collection</param>
        /// <returns>QueuedEntity object having given Id</returns>
        public QueuedEntity this[Int64 Id]
        {
            get
            {
                QueuedEntity queuedEntity = GetQueuedEntity(Id);
                if (queuedEntity == null)
                    throw new ArgumentException(String.Format("No result found for id: {0}", Id), "Id");
                else
                    return queuedEntity;
            }
            set
            {
                QueuedEntity queuedEntity = GetQueuedEntity(Id);
                if (queuedEntity == null)
                    throw new ArgumentException(String.Format("No result found for  id: {0}", Id), "Id");

                queuedEntity = value;
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
            if (obj is QueuedEntityCollection)
            {
                QueuedEntityCollection objectToBeCompared = obj as QueuedEntityCollection;
                Int32 queuedEntityUnion = this._queuedEntityCollection.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 queuedEntityIntersect = this._queuedEntityCollection.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if (queuedEntityUnion != queuedEntityIntersect)
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
            foreach (QueuedEntity attr in this._queuedEntityCollection)
            {
                hashCode += attr.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        public void LoadQueuedEntityCollection(String valuesAsXml)
        {
            #region Sample Xml
            /*
             * <QueuedEntity></QueuedEntity>
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "QueuedEntity")
                        {
                            String QueuedEntityXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(QueuedEntityXml))
                            {
                                QueuedEntity queuedEntity = new QueuedEntity(QueuedEntityXml);
                                this.Add(queuedEntity);
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
        /// Get the QueuedEntity based on the EntityId
        /// </summary>
        /// <param name="entityId">entity Id</param>
        /// <returns>list of queuedEntity for the specifed entityId</returns>
        public List<QueuedEntity> GetQueuedEntityByEntityId(Int64 entityId)
        {
            var filteredQueuedEntity = from queuedEntity in this._queuedEntityCollection
                                            where queuedEntity.EntityId == entityId
                                            select queuedEntity;

            if (filteredQueuedEntity.Any())
                return filteredQueuedEntity.ToList<QueuedEntity>();
            else
                return null;

        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Get the QueuedEntity based on the unique Id
        /// </summary>
        /// <param name="Id">pk of QueuedEntity</param>
        /// <returns></returns>
        private QueuedEntity GetQueuedEntity(Int64 Id)
        {
            var filteredQueuedEntity = from queuedEntity in this._queuedEntityCollection
                                            where queuedEntity.Id == Id
                                            select queuedEntity;

            if (filteredQueuedEntity.Any())
                return filteredQueuedEntity.First();
            else
                return null;
        }

        #endregion

        #region ICollection<QueuedEntity> Members

        /// <summary>
        /// Add QueuedEntity object in collection
        /// </summary>
        /// <param name="item">QueuedEntity to add in collection</param>
        public void Add(QueuedEntity item)
        {
            this._queuedEntityCollection.Add(item);
        }

        /// <summary>
        /// Removes all QueuedEntity from collection
        /// </summary>
        public void Clear()
        {
            this._queuedEntityCollection.Clear();
        }

        /// <summary>
        /// Determines whether the QueuedEntityCollection contains a specific QueuedEntity.
        /// </summary>
        /// <param name="item">The QueuedEntity object to locate in the QueuedEntityCollection.</param>
        /// <returns>
        /// <para>true : If QueuedEntity found in mappingCollection</para>
        /// <para>false : If QueuedEntity found not in mappingCollection</para>
        /// </returns>
        public bool Contains(QueuedEntity item)
        {
            return this._queuedEntityCollection.Contains(item);
        }

        /// <summary>
        /// Determines whether the QueuedEntityCollection contains a specific queuedEntity based on Id.
        /// </summary>
        /// <param name="id">The Id locate in the QueuedEntityCollection.</param>
        /// <returns>
        /// <para>true : If Id found in mappingCollection</para>
        /// <para>false : If Id found not in mappingCollection</para>
        /// </returns>
        public bool Contains(Int64 id)
        {
            if (GetQueuedEntityByEntityId(id) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Copies the elements of the QueuedEntityCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from QueuedEntityCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(QueuedEntity[] array, int arrayIndex)
        {
            this._queuedEntityCollection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of QueuedEntity in QueuedEntityCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._queuedEntityCollection.Count;
            }
        }

        /// <summary>
        /// Check if QueuedEntityCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the QueuedEntityCollection.
        /// </summary>
        /// <param name="item">The denorm result object to remove from the QueuedEntityCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original QueuedEntityCollection</returns>
        public bool Remove(QueuedEntity item)
        {
            return this._queuedEntityCollection.Remove(item);
        }

        #endregion

        #region IEnumerable<QueuedEntity> Members

        /// <summary>
        /// Returns an enumerator that iterates through a QueuedEntityCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<QueuedEntity> GetEnumerator()
        {
            return this._queuedEntityCollection.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a QueuedEntityCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._queuedEntityCollection.GetEnumerator();
        }

        #endregion

        #region XML Methods

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of QueuedEntityCollection object
        /// </summary>
        /// <returns>Xml string representing the QueuedEntityCollection</returns>
        public String ToXml()
        {
            String queuedEntitysXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (QueuedEntity queuedEntity in this._queuedEntityCollection)
            {
                builder.Append(queuedEntity.ToXml());
            }

            queuedEntitysXml = String.Format("<QueuedEntities>{0}</QueuedEntities>", builder.ToString());
            return queuedEntitysXml;
        }

        #endregion ToXml methods

        #endregion

    }
}
