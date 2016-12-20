
using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace MDM.BusinessObjects.Integration
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents collection of QualifyingQueueItem
    /// </summary>
    [DataContract]
    public class QualifyingQueueItemCollection : InterfaceContractCollection<IQualifyingQueueItem, QualifyingQueueItem>, IQualifyingQueueItemCollection
    {
        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public QualifyingQueueItemCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public QualifyingQueueItemCollection(String valueAsXml)
        {
            LoadQualifyingQueueItemCollection(valueAsXml);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Determines whether the QualifyingQueueItemCollection contains a specific item.
        /// </summary>
        /// <param name="qualifyingQueueItemId">The qualifying queue object to locate in the QualifyingQueueItemCollection.</param>
        /// <returns>
        /// <para>true : If qualifying queue found in QualifyingQueueItemCollection</para>
        /// <para>false : If qualifying queue found not in QualifyingQueueItemCollection</para>
        /// </returns>
        public Boolean Contains(Int64 qualifyingQueueItemId)
        {
            return this.GetQualifyingQueueItem(qualifyingQueueItemId) != null;
        }

        /// <summary>
        /// Remove qualifying queue object from QualifyingQueueItemCollection
        /// </summary>
        /// <param name="qualifyingQueueItemId">QualifyingQueueItemId of qualifying queue which is to be removed from collection</param>
        /// <returns>true if qualifying queue is successfully removed; otherwise, false. This method also returns false if qualifying queue was not found in the original collection</returns>
        public Boolean Remove(Int64 qualifyingQueueItemId)
        {
            IQualifyingQueueItem item = GetQualifyingQueueItem(qualifyingQueueItemId);

            if (item == null)
                throw new ArgumentException("No QualifyingQueueItem found for given Id :" + qualifyingQueueItemId);

            return this.Remove(item);
        }

        /// <summary>
        /// Get specific qualifying queue item by Id
        /// </summary>
        /// <param name="qualifyingQueueItemId">Id of qualifying queue</param>
        /// <returns><see cref="QualifyingQueueItem"/></returns>
        public IQualifyingQueueItem GetQualifyingQueueItem(Int64 qualifyingQueueItemId)
        {
            if (this._items == null)
            {
                throw new NullReferenceException("There are no QualifyingQueueItems to search in");
            }

            if (qualifyingQueueItemId <= 0)
            {
                throw new ArgumentException("QualifyinfQueueMessage Id must be greater than 0", qualifyingQueueItemId.ToString());
            }

            return this.Get(qualifyingQueueItemId) as QualifyingQueueItem;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(object obj)
        {
            if (obj is QualifyingQueueItemCollection)
            {
                QualifyingQueueItemCollection objectToBeCompared = obj as QualifyingQueueItemCollection;
                Int64 union = this._items.ToList().Union(objectToBeCompared._items.ToList()).Count();
                Int64 intersect = this._items.ToList().Intersect(objectToBeCompared._items.ToList()).Count();
                if (union != intersect)
                    return false;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override Int32 GetHashCode()
        {
            return this._items.Sum(attr => attr.GetHashCode());
        }

        /// <summary>
        /// Get Xml representation of QualifyingQueueItem object
        /// </summary>
        /// <returns>Xml String representing the QualifyingQueueItemCollection</returns>
        public String ToXml()
        {
            String xml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (QualifyingQueueItem hierarchy in this._items)
            {
                builder.Append(hierarchy.ToXml());
            }

            xml = String.Format("<QualifyingQueueItems>{0}</QualifyingQueueItems>", builder);
            return xml;
        }

        /// <summary>
        /// Get Xml representation of Qualifying Messages collection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            return this.ToXml();
        }

        /// <summary>
        /// Clone qualifying queue collection.
        /// </summary>
        /// <returns>cloned qualifying queue collection object.</returns>
        public IQualifyingQueueItemCollection Clone()
        {
            QualifyingQueueItemCollection clonedItems = new QualifyingQueueItemCollection();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (QualifyingQueueItem item in this._items)
                {
                    IQualifyingQueueItem clonedItem = item.Clone();
                    clonedItems.Add(clonedItem);
                }
            }

            return clonedItems;

        }

        /// <summary>
        /// Gets QualifyingQueueItem item by id
        /// </summary>
        /// <param name="qualifyingQueueItemId">Id of the QualifyingQueueItem</param>
        /// <returns>QualifyingQueueItem with specified Id</returns>
        public IQualifyingQueueItem Get(Int64 qualifyingQueueItemId)
        {
            return this._items.FirstOrDefault(item => item.Id == qualifyingQueueItemId);
        }

        #endregion Public Methods

        #region Private Methods

        private void LoadQualifyingQueueItemCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "QualifyingQueueItem")
                        {
                            String xml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(xml))
                            {
                                QualifyingQueueItem item = new QualifyingQueueItem(xml);
                                this.Add(item);
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
    }
}
