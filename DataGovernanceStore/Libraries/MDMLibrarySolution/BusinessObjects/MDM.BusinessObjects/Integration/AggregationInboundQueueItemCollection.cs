using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Collections.Generic;

namespace MDM.BusinessObjects.Integration
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents collection of AggregationInboundQueueItem
    /// </summary>
    [DataContract]
    public class AggregationInboundQueueItemCollection : InterfaceContractCollection<IAggregationInboundQueueItem, AggregationInboundQueueItem>, IAggregationInboundQueueItemCollection
    {
        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public AggregationInboundQueueItemCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public AggregationInboundQueueItemCollection(String valueAsXml)
        {
            LoadAggregationInboundQueueItemCollection(valueAsXml);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Determines whether the AggregationInboundQueueItemCollection contains a specific item.
        /// </summary>
        /// <param name="aggregationInboundQueueItemId">The aggregation inbound queue object to locate in the AggregationInboundQueueItemCollection.</param>
        /// <returns>
        /// <para>true : If aggregation inbound queue found in AggregationInboundQueueItemCollection</para>
        /// <para>false : If aggregation inbound queue found not in AggregationInboundQueueItemCollection</para>
        /// </returns>
        public Boolean Contains(Int64 aggregationInboundQueueItemId)
        {
            return this.Get(aggregationInboundQueueItemId) != null;
        }

        /// <summary>
        /// Remove aggregation inbound queue object from AggregationInboundQueueItemCollection
        /// </summary>
        /// <param name="aggregationInboundQueueItemId">AggregationInboundQueueItemId of inbound queue which is to be removed from collection</param>
        /// <returns>true if aggregation inbound queue is successfully removed; otherwise, false. This method also returns false if inbound queue was not found in the original collection</returns>
        public Boolean Remove(Int64 aggregationInboundQueueItemId)
        {
            IAggregationInboundQueueItem item = Get(aggregationInboundQueueItemId);

            if (item == null)
                throw new ArgumentException("No AggregationInboundQueueItem found for given Id :" + aggregationInboundQueueItemId);

            return this.Remove(item);
        }


        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(object obj)
        {
            if (obj is AggregationInboundQueueItemCollection)
            {
                AggregationInboundQueueItemCollection objectToBeCompared = obj as AggregationInboundQueueItemCollection;
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
        /// Get Xml representation of AggregationInboundQueueItem object
        /// </summary>
        /// <returns>Xml String representing the AggregationInboundQueueItemCollection</returns>
        public String ToXml()
        {
            String xml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (AggregationInboundQueueItem aggregationInboundQueueItem in this._items)
            {
                builder.Append(aggregationInboundQueueItem.ToXml());
            }

            xml = String.Format("<AggregationInboundQueueItemCollection>{0}</AggregationInboundQueueItemCollection>", builder);
            return xml;
        }

        /// <summary>
        /// Get Xml representation of Aggregation Inbound Messages collection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            return this.ToXml();
        }

        /// <summary>
        /// Clone inbound queue collection.
        /// </summary>
        /// <returns>cloned aggregation inbound queue collection object.</returns>
        public IAggregationInboundQueueItemCollection Clone()
        {
            AggregationInboundQueueItemCollection clonedItems = new AggregationInboundQueueItemCollection();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (AggregationInboundQueueItem item in this._items)
                {
                    IAggregationInboundQueueItem clonedItem = (IAggregationInboundQueueItem)item.Clone();
                    clonedItems.Add(clonedItem);
                }
            }

            return clonedItems;

        }

        /// <summary>
        /// Gets InboundQueueItem item by id
        /// </summary>
        /// <param name="aggregationInboundQueueItemId">Id of the AggregationInboundQueueItem</param>
        /// <returns>AggregationInboundQueueItem with specified Id</returns>
        public IAggregationInboundQueueItem Get(Int64 aggregationInboundQueueItemId)
        {
            return this._items.FirstOrDefault(item => item.Id == aggregationInboundQueueItemId);
        }

        #endregion Public Methods

        #region Private Methods

        private void LoadAggregationInboundQueueItemCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "AggregationInboundQueueItem")
                        {
                            String xml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(xml))
                            {
                                AggregationInboundQueueItem item = new AggregationInboundQueueItem(xml);
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
