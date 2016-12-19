using ProtoBuf;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;

    /// <summary>
    /// Specifies export queue collection.
    /// </summary>
    public class ExportQueueCollection : InterfaceContractCollection<IExportQueue, ExportQueue>, IExportQueueCollection
    {
        #region Fields
        #endregion Fields

        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ExportQueueCollection()
            : base()
        {
        }

        /// <summary>
        /// Constructor with XML
        /// </summary>
        /// <param name="valuesAsxml">Indicates XML having xml value</param>
        public ExportQueueCollection(String valuesAsxml)
        {
            LoadExportQueueCollection(valuesAsxml);
        }

        #endregion Constructor

        #region Properties
        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get Xml representation of ExportQueueCollection
        /// </summary>
        /// <returns>Returns Xml representation of ExportQueueCollection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    //ExportQueueCollection node start
                    xmlWriter.WriteStartElement("ExportQueues");

                    #region Write EntityFamilyQueueCollection

                    if (_items != null)
                    {
                        foreach (ExportQueue exportQueue in this._items)
                        {
                            xmlWriter.WriteRaw(exportQueue.ToXml());
                        }
                    }

                    #endregion

                    //ExportQueueCollection node end
                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();

                    //Get the actual XML
                    returnXml = sw.ToString();
                }
            }

            return returnXml;
        }

        /// <summary>
        /// Gets export queue for the given condition
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns>
        /// Returns export queue based on given input parameters
        /// </returns>
        public ExportQueue Get(Func<ExportQueue, Boolean> condition)
        {
            if (this._items.Count > 0)
            {
                foreach (ExportQueue exportQueue in this._items)
                {
                    if (condition(exportQueue))
                    {
                        return exportQueue;
                    }
                }
            }

            return null;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadExportQueueCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExportQueue")
                        {
                            String exportQueueXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(exportQueueXml))
                            {
                                ExportQueue exportQueue = new ExportQueue(exportQueueXml);
                                this.Add(exportQueue);
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

        #endregion Methods
    }
}