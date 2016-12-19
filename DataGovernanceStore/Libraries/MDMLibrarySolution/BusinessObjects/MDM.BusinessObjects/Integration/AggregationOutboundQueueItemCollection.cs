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
    /// Represents collection of AggregationOutboundQueueItem
    /// </summary>
    [DataContract]
    public class AggregationOutboundQueueItemCollection : InterfaceContractCollection<IAggregationOutboundQueueItem, AggregationOutboundQueueItem>, IAggregationOutboundQueueItemCollection
    {
        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public AggregationOutboundQueueItemCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public AggregationOutboundQueueItemCollection(String valueAsXml)
        {
            LoadAggregationOutboundQueueItemCollection(valueAsXml);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Determines whether the AggregationOutboundQueueItemCollection contains a specific item.
        /// </summary>
        /// <param name="aggregationOutboundQueueItemId">The aggregation outbound queue object to locate in the AggregationOutboundQueueItemCollection.</param>
        /// <returns>
        /// <para>true : If aggregation outbound queue found in AggregationOutboundQueueItemCollection</para>
        /// <para>false : If aggregation outbound queue found not in AggregationOutboundQueueItemCollection</para>
        /// </returns>
        public Boolean Contains(Int64 aggregationOutboundQueueItemId)
        {
            return this.Get(aggregationOutboundQueueItemId) != null;
        }

        /// <summary>
        /// Remove aggregation outbound queue object from AggregationOutboundQueueItemCollection
        /// </summary>
        /// <param name="aggregationOutboundQueueItemId">AggregationOutboundQueueItemId of outbound queue which is to be removed from collection</param>
        /// <returns>true if aggregation outbound queue is successfully removed; otherwise, false. This method also returns false if outbound queue was not found in the original collection</returns>
        public Boolean Remove(Int64 aggregationOutboundQueueItemId)
        {
            IAggregationOutboundQueueItem item = Get(aggregationOutboundQueueItemId);

            if (item == null)
                throw new ArgumentException("No AggregationOutboundQueueItem found for given Id :" + aggregationOutboundQueueItemId);

            return this.Remove(item);
        }


        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(object obj)
        {
            if (obj is AggregationOutboundQueueItemCollection)
            {
                AggregationOutboundQueueItemCollection objectToBeCompared = obj as AggregationOutboundQueueItemCollection;
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
        /// Get Xml representation of AggregationOutboundQueueItem object
        /// </summary>
        /// <returns>Xml String representing the AggregationOutboundQueueItemCollection</returns>
        public String ToXml()
        {
            String xml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (AggregationOutboundQueueItem hierarchy in this._items)
            {
                builder.Append(hierarchy.ToXml());
            }

            xml = String.Format("<AggregationOutboundQueueItemCollection>{0}</AggregationOutboundQueueItemCollection>", builder);
            return xml;
        }

        /// <summary>
        /// Get Xml representation of Aggregation Outbound Messages collection object
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
        /// <returns>cloned aggregation outbound queue collection object.</returns>
        public IAggregationOutboundQueueItemCollection Clone()
        {
            AggregationOutboundQueueItemCollection clonedItems = new AggregationOutboundQueueItemCollection();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (AggregationOutboundQueueItem item in this._items)
                {
                    IAggregationOutboundQueueItem clonedItem = (IAggregationOutboundQueueItem)item.Clone();
                    clonedItems.Add(clonedItem);
                }
            }

            return clonedItems;

        }

        /// <summary>
        /// Gets OutboundQueueItem item by id
        /// </summary>
        /// <param name="aggregationOutboundQueueItemId">Id of the AggregationOutboundQueueItem</param>
        /// <returns>AggregationOutboundQueueItem with specified Id</returns>
        public IAggregationOutboundQueueItem Get(Int64 aggregationOutboundQueueItemId)
        {
            return this._items.FirstOrDefault(item => item.Id == aggregationOutboundQueueItemId);
        }

        #endregion Public Methods

        #region Private Methods

        private void LoadAggregationOutboundQueueItemCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "AggregationOutboundQueueItem")
                        {
                            String xml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(xml))
                            {
                                AggregationOutboundQueueItem item = new AggregationOutboundQueueItem(xml);
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
