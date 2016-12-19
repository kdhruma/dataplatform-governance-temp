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
    /// Represents collection of OutboundQueueItem
    /// </summary>
    [DataContract]
    public class InboundQueueItemCollection : InterfaceContractCollection<IInboundQueueItem, InboundQueueItem>, IInboundQueueItemCollection
    {
        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public InboundQueueItemCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public InboundQueueItemCollection(String valueAsXml)
        {
            LoadInboundQueueItemCollection(valueAsXml);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Determines whether the InboundQueueItemCollection contains a specific item.
        /// </summary>
        /// <param name="inboundQueueItemId">The inbound queue object to locate in the InboundQueueItemCollection.</param>
        /// <returns>
        /// <para>true : If inbound queue found in InboundQueueItemCollection</para>
        /// <para>false : If inbound queue found not in InboundQueueItemCollection</para>
        /// </returns>
        public Boolean Contains(Int64 inboundQueueItemId)
        {
            return this.GetInboundQueueItem(inboundQueueItemId) != null;
        }

        /// <summary>
        /// Remove inbound queue object from InboundQueueItemCollection
        /// </summary>
        /// <param name="inboundQueueItemId">InboundQueueItemId of inbound queue which is to be removed from collection</param>
        /// <returns>true if inbound queue is successfully removed; otherwise, false. This method also returns false if inbound queue was not found in the original collection</returns>
        public Boolean Remove(Int64 inboundQueueItemId)
        {
            IInboundQueueItem item = GetInboundQueueItem(inboundQueueItemId);

            if (item == null)
                throw new ArgumentException("No InboundQueueItem found for given Id :" + inboundQueueItemId);

            return this.Remove(item);
        }

        /// <summary>
        /// Get specific inbound queue item by Id
        /// </summary>
        /// <param name="inboundQueueItemId">Id of inbound queue</param>
        /// <returns><see cref="InboundQueueItem"/></returns>
        public IInboundQueueItem GetInboundQueueItem(Int64 inboundQueueItemId)
        {
            if (this._items == null)
            {
                throw new NullReferenceException("There are no InboundQueueItems to search in");
            }

            if (inboundQueueItemId <= 0)
            {
                throw new ArgumentException("QualifyinfQueueMessage Id must be greater than 0", inboundQueueItemId.ToString());
            }

            return this.Get(inboundQueueItemId) as InboundQueueItem;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(object obj)
        {
            if (obj is InboundQueueItemCollection)
            {
                InboundQueueItemCollection objectToBeCompared = obj as InboundQueueItemCollection;
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
        /// Get Xml representation of InboundQueueItem object
        /// </summary>
        /// <returns>Xml String representing the InboundQueueItemCollection</returns>
        public String ToXml()
        {
            String xml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (InboundQueueItem hierarchy in this._items)
            {
                builder.Append(hierarchy.ToXml());
            }

            xml = String.Format("<InboundQueueItems>{0}</InboundQueueItems>", builder);
            return xml;
        }

        /// <summary>
        /// Get Xml representation of Inbound Messages collection object
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
        /// <returns>cloned inbound queue collection object.</returns>
        public IInboundQueueItemCollection Clone()
        {
            InboundQueueItemCollection clonedItems = new InboundQueueItemCollection();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (InboundQueueItem item in this._items)
                {
                    IInboundQueueItem clonedItem = (IInboundQueueItem) item.Clone();
                    clonedItems.Add(clonedItem);
                }
            }

            return clonedItems;

        }

        /// <summary>
        /// Gets InboundQueueItem item by id
        /// </summary>
        /// <param name="inboundQueueItemId">Id of the InboundQueueItem</param>
        /// <returns>InboundQueueItem with specified Id</returns>
        public IInboundQueueItem Get(Int64 inboundQueueItemId)
        {
            return this._items.FirstOrDefault(item => item.Id == inboundQueueItemId);
        }

        #endregion Public Methods

        #region Private Methods

        private void LoadInboundQueueItemCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "InboundQueueItem")
                        {
                            String xml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(xml))
                            {
                                InboundQueueItem item = new InboundQueueItem(xml);
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
