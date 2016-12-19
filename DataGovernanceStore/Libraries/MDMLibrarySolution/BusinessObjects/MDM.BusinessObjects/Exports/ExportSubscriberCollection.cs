using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using MDM.Core;
using MDM.Interfaces.Exports;

namespace MDM.BusinessObjects.Exports
{
    /// <summary>
    /// Represent Collection of ExportSubscriber Object
    /// </summary>
    [DataContract]
    public class ExportSubscriberCollection : InterfaceContractCollection<IExportSubscriber, ExportSubscriber>, IExportSubscriberCollection
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ExportSubscriber Collection
        /// </summary>
        public ExportSubscriberCollection() { }

        /// <summary>
        /// Initialize subscriber from Xml value
        /// </summary>
        /// <param name="valuesAsXml">Export subscriber in xml format</param>
        public ExportSubscriberCollection(String valuesAsXml)
        {
            LoadExportSubscriber(valuesAsXml);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Check if ExportSubscriberCollection contains ExportSubscriber with given Id
        /// </summary>
        /// <param name="Id">Id using which ExportSubscriber is to be searched from collection</param>
        /// <returns>
        /// <para>true : If ExportSubscriber found in ExportSubscriberCollection</para>
        /// <para>false : If ExportSubscriber found not in ExportSubscriberCollection</para>
        /// </returns>
        public bool Contains(Int32 Id)
        {
            if (GetExportSubscriber(Id) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Remove exportSubscriber object from ExportSubscriberCollection
        /// </summary>
        /// <param name="exportSubscriberId">exportSubscriberId of exportSubscriber which is to be removed from collection</param>
        /// <returns>true if exportSubscriber is successfully removed; otherwise, false. This method also returns false if exportSubscriber was not found in the original collection</returns>
        public bool Remove(Int32 exportSubscriberId)
        {
            ExportSubscriber exportSubscriber = GetExportSubscriber(exportSubscriberId);

            if (exportSubscriber == null)
                throw new ArgumentException("No ExportSubscriber found for given Id :" + exportSubscriberId);
            else
                return this.Remove(exportSubscriber);
        }

        /// <summary>
        /// Get Xml representation of ExportSubscriberCollection
        /// </summary>
        /// <returns>Xml representation of ExportSubscriberCollection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            returnXml = "<ExportSubscribers>";

            if (this._items != null && this._items.Count > 0)
            {
                foreach (ExportSubscriber exportSubscriber in this._items)
                {
                    returnXml = String.Concat(returnXml, exportSubscriber.ToXml());
                }
            }

            returnXml = String.Concat(returnXml, "</ExportSubscribers>");

            return returnXml;
        }

        /// <summary>
        /// Get Xml representation of ExportSubscriberCollection
        /// </summary>
        /// <returns>Xml representation of ExportSubscriberCollection</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String returnXml = String.Empty;

            returnXml = "<ExportSubscribers>";

            if (this._items != null && this._items.Count > 0)
            {
                foreach (ExportSubscriber exportSubscriber in this._items)
                {
                    returnXml = String.Concat(returnXml, exportSubscriber.ToXml(serialization));
                }
            }

            returnXml = String.Concat(returnXml, "</ExportSubscribers>");

            return returnXml;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is ExportSubscriberCollection)
            {
                ExportSubscriberCollection objectToBeCompared = obj as ExportSubscriberCollection;

                Int32 exportSubscribersUnion = this._items.ToList().Union(objectToBeCompared._items.ToList()).Count();
                Int32 exportSubscribersIntersect = this._items.ToList().Intersect(objectToBeCompared._items.ToList()).Count();

                if (exportSubscribersUnion != exportSubscribersIntersect)
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

            foreach (ExportSubscriber ExportSubscriber in this._items)
            {
                hashCode += ExportSubscriber.GetHashCode();
            }

            return hashCode;
        }

        #endregion

        #region Private Methods

        private ExportSubscriber GetExportSubscriber(Int32 Id)
        {
            var filteredExportSubscriber = from exportSubscriber in this._items
                               where exportSubscriber.Id == Id
                               select exportSubscriber;

            if (filteredExportSubscriber.Any())
                return filteredExportSubscriber.First();
            else
                return null;
        }

        private void LoadExportSubscriber(String valuesAsXml)
        {
            #region Sample Xml
            /*
			 * <ExportSubscribers>
                <ExportSubscriber Id="1" Name="Export Drop" SubscriberType="Unknown">
                  <ConfigurationParameters>
                    <ConfigurationParameter Key="Directory" Value="\\RST1061\Export Drop" />
                  </ConfigurationParameters>
                </ExportSubscriber>
              </ExportSubscribers>
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExportSubscriber")
                        {
                            String subscriberXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(subscriberXml))
                            {
                                ExportSubscriber exportSubscriber = new ExportSubscriber(subscriberXml);
                                if (exportSubscriber != null)
                                {
                                    this.Add(exportSubscriber);
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
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }
        }
        #endregion
    }
}
