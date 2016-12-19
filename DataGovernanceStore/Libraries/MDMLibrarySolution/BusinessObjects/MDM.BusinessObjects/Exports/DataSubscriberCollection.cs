using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml;
using MDM.Interfaces.Exports;

namespace MDM.BusinessObjects.Exports
{
    using MDM.Core;

    /// <summary>
    /// Specifies the data subscriber collection object
    /// </summary>
    [DataContract]
    public class DataSubscriberCollection : InterfaceContractCollection<IDataSubscriber, DataSubscriber>, IDataSubscriberCollection
    {
        #region Fields

        #endregion Fields

        #region Constructors

		/// <summary>
		/// Parameterless constructor
		/// </summary>
		public DataSubscriberCollection() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valueAsXml">Values Which needs to be added in the collection in XMl format</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        public DataSubscriberCollection(String valueAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadDataSubscriberCollection(valueAsXml, objectSerialization);
        }

		/// <summary>
        /// Initialize data subscriber collection from IList
		/// </summary>
        /// <param name="dataSubscribersList">IList of dataSubscribercollection</param>
        public DataSubscriberCollection(IList<DataSubscriber> dataSubscribersList)
		{
            this._items = new Collection<DataSubscriber>(dataSubscribersList);
		}

        #endregion Constructors

        #region Properties

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Get Xml representation of data subscriber collection object
        /// </summary>
        /// <returns>Xml string representing the data subscriber collection</returns>
        public String ToXml()
        {
            String dataSubscribersXml = String.Empty;

            dataSubscribersXml = "<DataSubscribers>";

            if (this._items != null && this._items.Count > 0)
            {
                foreach (DataSubscriber dataSubscriber in this._items)
                {
                    dataSubscribersXml = String.Concat(dataSubscribersXml, dataSubscriber.ToXml());
                }
            }

            dataSubscribersXml = String.Concat(dataSubscribersXml, "</DataSubscribers>");

            return dataSubscribersXml;
        }

        /// <summary>
        /// Get Xml representation of data subscriber collection object
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>Xml string representing the data subscriber collection</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String dataFormattersXml = String.Empty;

            //TODO:: Write logic as per the different object serialization type whenever needed..
            //ObjectSerialization.External is used for Export Profile
            if (objectSerialization == ObjectSerialization.External)
            {
                dataFormattersXml = "<DataSubscribers>";

                if (this._items != null && this._items.Count > 0)
                {
                    foreach (DataSubscriber dataSubscriber in this._items)
                    {
                        dataFormattersXml = String.Concat(dataFormattersXml, dataSubscriber.ToXml(objectSerialization));
                    }
                }

                dataFormattersXml = String.Concat(dataFormattersXml, "</DataSubscribers>");
            }
            else
            {
                dataFormattersXml = this.ToXml();
            }

            return dataFormattersXml;
        }

        /// <summary>
        /// Gets the datasubscriber by subscriber id
        /// </summary>
        /// <param name="subscriberId">Id of subsciber which needs to retrived</param>
        /// <returns>Returns datasubsciber</returns>
        public DataSubscriber Get(Int32 subscriberId)
        {
            if (this._items != null && this._items.Count > 0)
            {
                foreach (DataSubscriber dataSubsciber in this._items)
                {
                    if (dataSubsciber.Id == subscriberId)
                    {
                        return dataSubsciber;
                    }
                }
            }

            return null;
        }
        
        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Loads the data subscriber collection from given XMl and add into this
        /// </summary>
        /// <param name="valuesAsXml">Values Which needs to be added in the collection in XMl format</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        private void LoadDataSubscriberCollection(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            #region Sample Xml

            /*
                <DataSubscribers>
			        <DataSubscriber Id="" Name="" Location="" FileName=""></DataSubscriber>
		        </DataSubscribers>
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "DataSubscriber")
                        {
                            #region Read DataSubscribers Collection

                            String dataSubscribersXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(dataSubscribersXml))
                            {
                                DataSubscriber dataSubscriber = new DataSubscriber(dataSubscribersXml, objectSerialization);
                                if (dataSubscriber != null)
                                {
                                    this.Add(dataSubscriber);
                                }
                            }

                            #endregion Read DataSubscribers Collection
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
