using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;

    /// <summary>
    /// Represents Collection of data model exclusion context instance
    /// </summary>
    [DataContract]
    public class DataModelExclusionContextCollection : InterfaceContractCollection<IDataModelExclusionContext, DataModelExclusionContext>, IDataModelExclusionContextCollection
    {
        #region Fields

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Parameter-less constructor
        /// </summary>
        public DataModelExclusionContextCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">XML having xml values</param>
        public DataModelExclusionContextCollection(String valuesAsXml)
        {
            LoadDataModelExclusionContextCollection(valuesAsXml);
        }

        /// <summary>
        /// Converts list into current instance
        /// </summary>
        /// <param name="dataModelExclusionContextList">List of data model exclusion contexts</param>
        public DataModelExclusionContextCollection(IList<DataModelExclusionContext> dataModelExclusionContextList)
        {
            if (dataModelExclusionContextList != null)
            {
                this._items = new Collection<DataModelExclusionContext>(dataModelExclusionContextList);
            }
        }

        #endregion Constructors

        #region Properties

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Adds given items into current collection
        /// </summary>
        /// <param name="items">Specifies items to be added</param>
        public void AddRange(DataModelExclusionContextCollection items)
        {
            if (items != null && items.Count > 0)
            {
                foreach (DataModelExclusionContext dataModelExclusionContext in items)
                {
                    if (!this.Contains(dataModelExclusionContext))
                    {
                        this._items.Add(dataModelExclusionContext);
                    }
                }
            }
        }

        #endregion Public Methods

        #region IDataModelExclusionContextCollection Methods

        /// <summary>
        /// Get Xml representation of DataModelExclusionContext Collection
        /// </summary>
        /// <returns>Xml representation of DataModelExclusionContext Collection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    //DataModelExclusionContextCollection node start
                    xmlWriter.WriteStartElement("DataModelExclusionContexts");

                    #region Write DataModelExclusionContextCollection

                    if (_items != null)
                    {
                        foreach (DataModelExclusionContext dataModelExclusionContext in this._items)
                        {
                            xmlWriter.WriteRaw(dataModelExclusionContext.ToXml());
                        }
                    }

                    #endregion

                    //DataModelExclusionContextCollection node end
                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();
                    
                    //Get the actual XML
                    returnXml = sw.ToString();
                }
            }

            return returnXml;
        }

        /// <summary>
        /// Add DataModelExclusionContext in collection
        /// </summary>
        /// <param name="iDataModelExclusionContext">DataModelExclusionContext to add in collection</param>
        public new void Add(IDataModelExclusionContext iDataModelExclusionContext)
        {
            this._items.Add((DataModelExclusionContext)iDataModelExclusionContext);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the DataModelExclusionContext collection
        /// </summary>
        /// <param name="iDataModelExclusionContext">The object to remove from the DataModelExclusionContext collection</param>
        /// <returns>true if item is successfully removed; otherwise, false</returns>
        public new Boolean Remove(IDataModelExclusionContext iDataModelExclusionContext)
        {
            return this._items.Remove((DataModelExclusionContext)iDataModelExclusionContext);
        }

        #endregion IDataModelExclusionContextCollection Methods
        
        #region Private Methods

        private void LoadDataModelExclusionContextCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "DataModelExclusionContext")
                        {
                            String dataModelExclusionContextXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(dataModelExclusionContextXml))
                            {
                                DataModelExclusionContext dataModelExclusionContext = new DataModelExclusionContext(dataModelExclusionContextXml);

                                if (dataModelExclusionContext != null)
                                {
                                    this.Add(dataModelExclusionContext);
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

        #endregion Private Methods
    }
}
