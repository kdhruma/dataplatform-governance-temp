using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;

    /// <summary>
    /// Specifies collection of DDG operation results summary
    /// </summary>
    [DataContract]
    public class DDGOperationResultSummaryCollection : InterfaceContractCollection<IDDGOperationResultSummary, DDGOperationResultSummary>, IDDGOperationResultSummaryCollection
    {
        #region Fields

        #endregion

        #region Properties

        /// <summary>
        /// Returns DDG operation result object for a given index from the collection
        /// </summary>
        /// <param name="index">Indicates the index for getting the specific operation result from the collection</param>
        /// <returns>Returns the DDG operation result object for a given index from the collection</returns>
        public DDGOperationResultSummary this[Int32 index]
        {
            get { return _items[index]; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public DDGOperationResultSummaryCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public DDGOperationResultSummaryCollection(String valueAsXml)
        {
            LoadDDGOperationResultSummaryCollection(valueAsXml);
        }

        #endregion Constructors

        #region Methods

        #region Public Methods

        /// <summary>
        /// Adds an item to the <see cref="DDGOperationResultSummaryCollection" />.
        /// </summary>
        /// <param name="item">Indicates the object to add to the <see cref="IDDGOperationResultSummaryCollection" />.</param>
        public new void Add(IDDGOperationResultSummary item)
        {
            base.Add(item);
        }

        /// <summary>
        /// Adds an item to the <see cref="DDGOperationResultSummaryCollection" />.
        /// </summary>
        /// <param name="items">The object to add to the <see cref="IDDGOperationResultSummaryCollection" />.</param>
        public void AddRange(IDDGOperationResultSummaryCollection items)
        {
            foreach (IDDGOperationResultSummary item in items)
            {
                base.Add(item);
            }
        }

        /// <summary>
        /// Xml representation of DDG operation summary collection object
        /// </summary>
        /// <returns>Xml representation of DDG operation summary collection object</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {

                    //Operation result node start
                    xmlWriter.WriteStartElement("DDGOperationResultSummaryCollection");

                    #region Write DDGOperationResultSummaryCollection

                    foreach (DDGOperationResultSummary operationResult in this._items)
                    {
                        xmlWriter.WriteRaw(operationResult.ToXml());
                    }

                    #endregion

                    //Operation result node end
                    xmlWriter.WriteEndElement();
                }

                //Get the actual XML
                returnXml = sw.ToString();
            }

            return returnXml;
        }

        #endregion

        #region Private Methods

        private void LoadDDGOperationResultSummaryCollection(String valueAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valueAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valueAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "DDGOperationResultSummaryCollection")
                        {
                            String operationResultXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(operationResultXml))
                            {
                                DDGOperationResultSummary operationResult = new DDGOperationResultSummary(operationResultXml);
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
