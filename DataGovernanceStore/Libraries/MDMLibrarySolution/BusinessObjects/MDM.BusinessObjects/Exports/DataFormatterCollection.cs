using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Exports
{
    using MDM.Core;
    using MDM.Interfaces.Exports;

    /// <summary>
    /// Specifies the data formatter collection object
    /// </summary>
    [DataContract]
    public class DataFormatterCollection : InterfaceContractCollection<IDataFormatter, DataFormatter>, IDataFormatterCollection
    {
        #region Fields

        #endregion Fields

        #region Constructors

		/// <summary>
		/// Parameterless constructor
		/// </summary>
		public DataFormatterCollection() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valueAsXml">Values Which needs to be added in the collection in XMl format</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        public DataFormatterCollection(String valueAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadDataFormatterCollection(valueAsXml, objectSerialization);
        }

		/// <summary>
        /// Constructor which takes list of data formatter as input parameter
		/// </summary>
        /// <param name="dataFormatterList">Indicates list of data formatter</param>
        public DataFormatterCollection(IList<DataFormatter> dataFormatterList)
		{
            this._items = new Collection<DataFormatter>(dataFormatterList);
		}

        #endregion Constructors

        #region Properties

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Get Xml representation of data formatter collection object
        /// </summary>
        /// <returns>Xml string representing the data formatter collection</returns>
        public String ToXml()
        {
            String dataFormattersXml = String.Empty;

            dataFormattersXml = "<DataFormatters>";

            if (this._items != null && this._items.Count > 0)
            {
                foreach (DataFormatter dataFormatter in this._items)
                {
                    dataFormattersXml = String.Concat(dataFormattersXml, dataFormatter.ToXml());
                }
            }

            dataFormattersXml = String.Concat(dataFormattersXml, "</DataFormatters>");

            return dataFormattersXml;
        }

        /// <summary>
        /// Get Xml representation of data formatter collection object
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>Xml string representing the data formatter collection</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String dataFormattersXml = String.Empty;

            //TODO:: Write logic as per the different object serialization type whenever needed..
            dataFormattersXml = this.ToXml();

            return dataFormattersXml;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Loads the data formatter collection from given XMl and add into this
        /// </summary>
        /// <param name="valuesAsXml">Values Which needs to be added in the collection in XMl format</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        private void LoadDataFormatterCollection(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            #region Sample Xml

            /*
                 <DataFormatters>
			        <DataFormatter Id="" Name="" Type="" AttributeColumnHeaderFormat="" ApplyExportMaskToLookupAttribute="" CategoryPathType=""></DataFormatter>
		        </DataFormatters>
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "DataFormatter")
                        {
                            #region Read DataFormatters Collection

                            String dataFormattersXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(dataFormattersXml))
                            {
                                DataFormatter dataFormatter = new DataFormatter(dataFormattersXml, objectSerialization);
                                if (dataFormatter != null)
                                {
                                    this.Add(dataFormatter);
                                }
                            }

                            #endregion Read DataFormatters Collection
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
