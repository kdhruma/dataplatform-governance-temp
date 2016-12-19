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
    /// Represents collection of IntegrationErrorLog
    /// </summary>
    [DataContract]
    public class IntegrationErrorLogCollection : InterfaceContractCollection<IIntegrationErrorLog, IntegrationErrorLog>, IIntegrationErrorLogCollection
    {
        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public IntegrationErrorLogCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public IntegrationErrorLogCollection(String valueAsXml)
        {
            LoadIntegrationErrorLogCollection(valueAsXml);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Determines whether the IntegrationErrorLogCollection contains a specific item.
        /// </summary>
        /// <param name="integrationErrorLogId">The integration error log object to locate in the IntegrationErrorLogCollection.</param>
        /// <returns>
        /// <para>true : If integration error log found in IntegrationErrorLogCollection</para>
        /// <para>false : If integration error log not found in IntegrationErrorLogCollection</para>
        /// </returns>
        public Boolean Contains(Int64 integrationErrorLogId)
        {
            return this.Get(integrationErrorLogId) != null;
        }

        /// <summary>
        /// Remove integration error log object from IntegrationErrorLogCollection
        /// </summary>
        /// <param name="integrationErrorLogId">IntegrationErrorLogId of integration error log which is to be removed from collection</param>
        /// <returns>true if integration error log is successfully removed; otherwise, false. This method also returns false if integration error log was not found in the original collection</returns>
        public Boolean Remove(Int64 integrationErrorLogId)
        {
            IIntegrationErrorLog item = Get(integrationErrorLogId);

            if (item == null)
                throw new ArgumentException("No IntegrationErrorLog found for given Id :" + integrationErrorLogId);

            return this.Remove(item);
        }


        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(object obj)
        {
            if (obj is IntegrationErrorLogCollection)
            {
                IntegrationErrorLogCollection objectToBeCompared = obj as IntegrationErrorLogCollection;
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
        /// Get Xml representation of IntegrationErrorLog object
        /// </summary>
        /// <returns>Xml String representing the IntegrationErrorLogCollection</returns>
        public String ToXml()
        {
            String xml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (IntegrationErrorLog errorLogItem in this._items)
            {
                builder.Append(errorLogItem.ToXml());
            }

            xml = String.Format("<IntegrationErrorLogCollection>{0}</IntegrationErrorLogCollection>", builder);
            return xml;
        }

        /// <summary>
        /// Get Xml representation of integration error log collection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            return this.ToXml();
        }

        /// <summary>
        /// Clone integration error log collection.
        /// </summary>
        /// <returns>cloned integration error log collection object.</returns>
        public IIntegrationErrorLogCollection Clone()
        {
            IntegrationErrorLogCollection clonedItems = new IntegrationErrorLogCollection();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (IntegrationErrorLog item in this._items)
                {
                    IIntegrationErrorLog clonedItem = (IIntegrationErrorLog)item.Clone();
                    clonedItems.Add(clonedItem);
                }
            }

            return clonedItems;

        }

        /// <summary>
        /// Gets OutboundQueueItem item by id
        /// </summary>
        /// <param name="integrationErrorLogId">Id of the IntegrationErrorLog</param>
        /// <returns>IntegrationErrorLog with specified Id</returns>
        public IIntegrationErrorLog Get(Int64 integrationErrorLogId)
        {
            return this._items.FirstOrDefault(item => item.Id == integrationErrorLogId);
        }

        #endregion Public Methods

        #region Private Methods

        private void LoadIntegrationErrorLogCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "IntegrationErrorLog")
                        {
                            String xml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(xml))
                            {
                                IntegrationErrorLog item = new IntegrationErrorLog(xml);
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
