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
    /// Represents collection of OutboundQueueItem
    /// </summary>
    [DataContract]
    public class OutboundQueueItemCollection : InterfaceContractCollection<IOutboundQueueItem, OutboundQueueItem>, IOutboundQueueItemCollection
    {
        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public OutboundQueueItemCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public OutboundQueueItemCollection(String valueAsXml)
        {
            LoadOutboundQueueItemCollection(valueAsXml);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Determines whether the OutboundQueueItemCollection contains a specific item.
        /// </summary>
        /// <param name="outboundQueueItemId">The outbound queue object to locate in the OutboundQueueItemCollection.</param>
        /// <returns>
        /// <para>true : If outbound queue found in OutboundQueueItemCollection</para>
        /// <para>false : If outbound queue found not in OutboundQueueItemCollection</para>
        /// </returns>
        public Boolean Contains(Int64 outboundQueueItemId)
        {
            return this.GetOutboundQueueItem(outboundQueueItemId) != null;
        }

        /// <summary>
        /// Remove outbound queue object from OutboundQueueItemCollection
        /// </summary>
        /// <param name="outboundQueueItemId">OutboundQueueItemId of outbound queue which is to be removed from collection</param>
        /// <returns>true if outbound queue is successfully removed; otherwise, false. This method also returns false if outbound queue was not found in the original collection</returns>
        public Boolean Remove(Int64 outboundQueueItemId)
        {
            IOutboundQueueItem item = GetOutboundQueueItem(outboundQueueItemId);

            if (item == null)
                throw new ArgumentException("No OutboundQueueItem found for given Id :" + outboundQueueItemId);

            return this.Remove(item);
        }

        /// <summary>
        /// Get specific outbound queue item by Id
        /// </summary>
        /// <param name="outboundQueueItemId">Id of outbound queue</param>
        /// <returns><see cref="OutboundQueueItem"/></returns>
        public IOutboundQueueItem GetOutboundQueueItem(Int64 outboundQueueItemId)
        {
            if (this._items == null)
            {
                throw new NullReferenceException("There are no OutboundQueueItems to search in");
            }

            if (outboundQueueItemId <= 0)
            {
                throw new ArgumentException("QualifyinfQueueMessage Id must be greater than 0", outboundQueueItemId.ToString());
            }

            return this.Get(outboundQueueItemId) as OutboundQueueItem;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(object obj)
        {
            if (obj is OutboundQueueItemCollection)
            {
                OutboundQueueItemCollection objectToBeCompared = obj as OutboundQueueItemCollection;
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
        /// Get Xml representation of OutboundQueueItem object
        /// </summary>
        /// <returns>Xml String representing the OutboundQueueItemCollection</returns>
        public String ToXml()
        {
            String xml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (OutboundQueueItem hierarchy in this._items)
            {
                builder.Append(hierarchy.ToXml());
            }

            xml = String.Format("<OutboundQueueItems>{0}</OutboundQueueItems>", builder);
            return xml;
        }

        /// <summary>
        /// Get Xml representation of Outbound Messages collection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            return this.ToXml();
        }

        /// <summary>
        /// Clone outbound queue collection.
        /// </summary>
        /// <returns>cloned outbound queue collection object.</returns>
        public IOutboundQueueItemCollection Clone()
        {
            OutboundQueueItemCollection clonedItems = new OutboundQueueItemCollection();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (OutboundQueueItem item in this._items)
                {
                    IOutboundQueueItem clonedItem = ( IOutboundQueueItem )item.Clone();
                    clonedItems.Add(clonedItem);
                }
            }

            return clonedItems;

        }

        /// <summary>
        /// Gets OutboundQueueItem item by id
        /// </summary>
        /// <param name="outboundQueueItemId">Id of the OutboundQueueItem</param>
        /// <returns>OutboundQueueItem with specified Id</returns>
        public IOutboundQueueItem Get(Int64 outboundQueueItemId)
        {
            return this._items.FirstOrDefault(item => item.Id == outboundQueueItemId);
        }

        #endregion Public Methods

        #region Private Methods

        private void LoadOutboundQueueItemCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "OutboundQueueItem")
                        {
                            String xml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(xml))
                            {
                                OutboundQueueItem item = new OutboundQueueItem(xml);
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
