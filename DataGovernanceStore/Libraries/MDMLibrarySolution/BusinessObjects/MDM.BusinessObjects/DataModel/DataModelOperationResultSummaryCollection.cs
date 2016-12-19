using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.BusinessObjects.DataModel;

    /// <summary>
    /// Specifies collection of operation results
    /// </summary>
    [DataContract]
    public class DataModelOperationResultSummaryCollection : InterfaceContractCollection<IDataModelOperationResultSummary, DataModelOperationResultSummary>, IDataModelOperationResultSummaryCollection
    {
        #region Fields

        #endregion

        #region Properties

        /// <summary>
        /// Returns Operation Result object for a given index from the collection
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns>Operation Result object for a given index from the collection</returns>
        public DataModelOperationResultSummary this[Int32 index]
        {
            get { return _items[index]; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public DataModelOperationResultSummaryCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public DataModelOperationResultSummaryCollection(String valueAsXml)
        {
            LoadDataModelOperationResultSummaryCollection(valueAsXml);
        }

        #endregion Constructors

        #region Methods

        #region Public Methods

        /// <summary>
        /// Adds an item to the <see cref="DataModelOperationResultSummaryCollection" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="IDataModelOperationResultSummaryCollection" />.</param>
        public new void Add(IDataModelOperationResultSummary item)
        {
            base.Add(item);
        }

        /// <summary>
        /// Adds an item to the <see cref="DataModelOperationResultSummaryCollection" />.
        /// </summary>
        /// <param name="items">The object to add to the <see cref="IDataModelOperationResultSummaryCollection" />.</param>
        public void AddRange(IDataModelOperationResultSummaryCollection items)
        {
            foreach (IDataModelOperationResultSummary item in items)
            {
                base.Add(item);
            }
        }

        /// <summary>
        /// Xml representation of data model operation summary collection object
        /// </summary>
        /// <returns>Xml representation of data model operation summary collection object</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Operation result node start
            xmlWriter.WriteStartElement("DataModelOperationResultSummaryCollection");

            #region Write DataModelOperationResultSummaryCollection

            foreach (DataModelOperationResultSummary operationResult in this._items)
            {
                xmlWriter.WriteRaw(operationResult.ToXml());
            }

            #endregion

            //Operation result node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            returnXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return returnXml;
        }

        #endregion

        #region Private Methods

        private void LoadDataModelOperationResultSummaryCollection(string valueAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valueAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valueAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "DataModelOperationResultSummaryCollection")
                        {
                            String operationResultXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(operationResultXml))
                            {
                                DataModelOperationResultSummary operationResult = new DataModelOperationResultSummary(operationResultXml);
                                this.Add(operationResult);
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

        #endregion
    }
}