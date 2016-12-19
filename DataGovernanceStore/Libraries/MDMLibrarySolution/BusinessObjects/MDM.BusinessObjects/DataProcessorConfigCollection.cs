using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;
    using MDM.Core;

    /// <summary>
    /// Specifies class for data processor configuration collection
    /// </summary>
    [DataContract]
    public class DataProcessorConfigCollection : ICollection<DataProcessorConfig>, IEnumerable<DataProcessorConfig>, IDataProcessorConfigCollection
    {

        #region Fields

        private Collection<DataProcessorConfig> _dataProcessorConfigCollection = new Collection<DataProcessorConfig>();

        /// <summary>
        /// Indicates if engine is started or not
        /// </summary>
        private Boolean _isParallelProcessingEngineRunning = true;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates if engine is started or not
        /// </summary>
        [DataMember]
        public Boolean IsParallelProcessingEngineRunning
        {
            get { return _isParallelProcessingEngineRunning; }
            set { _isParallelProcessingEngineRunning = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public DataProcessorConfigCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public DataProcessorConfigCollection(String valueAsXml)
        {
            LoadDataProcessorConfigCollection(valueAsXml);
        }

        /// <summary>
        /// Initialize DataProcessorConfigCollection from IList
        /// </summary>
        /// <param name="dataProcessorConfigList">IList of data processor configuration list</param>
        public DataProcessorConfigCollection(IList<DataProcessorConfig> dataProcessorConfigList)
        {
            this._dataProcessorConfigCollection = new Collection<DataProcessorConfig>(dataProcessorConfigList);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if(obj is DataProcessorConfigCollection)
            {
                DataProcessorConfigCollection objectToBeCompared = obj as DataProcessorConfigCollection;
                Int32 dataProcessorConfigUnion = this._dataProcessorConfigCollection.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 dataProcessorConfigIntersect = this._dataProcessorConfigCollection.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if (dataProcessorConfigUnion != dataProcessorConfigIntersect)
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
            foreach(DataProcessorConfig attr in this._dataProcessorConfigCollection)
            {
                hashCode += attr.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// Load data processor configuration collection object from XML
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        public void LoadDataProcessorConfigCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name.ToLowerInvariant() == "dataprocessorconfigcollection")
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("IsParallelProcessingEngineRunning"))
                                {
                                    this.IsParallelProcessingEngineRunning = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                            }
                            else
                            {
                                reader.Read();
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name.ToLowerInvariant() == "dataprocessorconfig")
                        {
                            String dataProcessorConfigXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(dataProcessorConfigXml))
                            {
                                DataProcessorConfig dataProcessorConfig = new DataProcessorConfig(dataProcessorConfigXml);
                                this.Add(dataProcessorConfig);
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


        /// <summary>
        /// Get the DataProcessorConfig based on the data processor name
        /// </summary>
        /// <param name="processorName">Name of DataProcessor</param>
        /// <returns></returns>
        public DataProcessorConfig GetDataProcessorConfig(String processorName)
        {
            var filteredDataProcessorConfig = from dataProcessorConfig in this._dataProcessorConfigCollection
                                              where dataProcessorConfig.ProcessorName == processorName
                                              select dataProcessorConfig;

            if (filteredDataProcessorConfig.Any())
                return filteredDataProcessorConfig.First();
            else
                return null;
        }

        /// <summary>
        /// Updates the data processor config collection with all new values
        /// </summary>
        /// <param name="newdataProcessorConfig">New values which has to be inserted in xml format.</param>
        /// <returns>String value which has updated data processor config collection values in xml format.</returns>
        public String UpdateProcessorConfigXml(DataProcessorConfig newdataProcessorConfig)
        {
            if (newdataProcessorConfig != null)
            {
                if (_dataProcessorConfigCollection.Count<1)
                {
                    _dataProcessorConfigCollection.Add(newdataProcessorConfig);
                }
                else
                {
                    var filteredDataProcessorConfig = from dataProcessorConfig in _dataProcessorConfigCollection
                                                      where
                                                          dataProcessorConfig.ProcessorName ==
                                                          newdataProcessorConfig.ProcessorName
                                                      select dataProcessorConfig;
                    if (filteredDataProcessorConfig.Any())
                    {
                        DataProcessorConfig dataProcessorConfig = filteredDataProcessorConfig.FirstOrDefault();
                        _dataProcessorConfigCollection.Remove(dataProcessorConfig);
                        _dataProcessorConfigCollection.Add(newdataProcessorConfig);
                    }
                    else
                    {
                        _dataProcessorConfigCollection.Add(newdataProcessorConfig);
                    }
                }
            }
            return ToXml();
        }

        #endregion

        #region ICollection<DataProcessorConfig> Members

        /// <summary>
        /// Add DataProcessorConfig object in collection
        /// </summary>
        /// <param name="item">DataProcessorConfig to add in collection</param>
        public void Add(DataProcessorConfig item)
        {
            this._dataProcessorConfigCollection.Add(item);
        }

        /// <summary>
        /// Removes all DataProcessorConfig from collection
        /// </summary>
        public void Clear()
        {
            this._dataProcessorConfigCollection.Clear();
        }

        /// <summary>
        /// Determines whether the DataProcessorConfigCollection contains a specific DataProcessorConfig.
        /// </summary>
        /// <param name="item">The DataProcessorConfig object to locate in the DataProcessorConfigCollection.</param>
        /// <returns>
        /// <para>true : If DataProcessorConfig found in mappingCollection</para>
        /// <para>false : If DataProcessorConfig found not in mappingCollection</para>
        /// </returns>
        public bool Contains(DataProcessorConfig item)
        {
            return this._dataProcessorConfigCollection.Contains(item);
        }

        /// <summary>
        /// Determines whether the DataProcessorConfigCollection contains a specific dataProcessorConfig based on processor name
        /// </summary>
        /// <param name="processorName">Indicates the name of processor</param>
        /// <returns>
        /// <para>true : If processorName is found in mappingCollection</para>
        /// <para>false : If processorName is not found in mappingCollection</para>
        /// </returns>
        public bool Contains(String processorName)
        {
            if(GetDataProcessorConfig(processorName) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Copies the elements of the DataProcessorConfigCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from DataProcessorConfigCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(DataProcessorConfig[] array, int arrayIndex)
        {
            this._dataProcessorConfigCollection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of DataProcessorConfig in DataProcessorConfigCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._dataProcessorConfigCollection.Count;
            }
        }

        /// <summary>
        /// Check if DataProcessorConfigCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the DataProcessorConfigCollection.
        /// </summary>
        /// <param name="item">The denorm result object to remove from the DataProcessorConfigCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original DataProcessorConfigCollection</returns>
        public bool Remove(DataProcessorConfig item)
        {
            return this._dataProcessorConfigCollection.Remove(item);
        }

        #endregion

        #region IEnumerable<DataProcessorConfig> Members

        /// <summary>
        /// Returns an enumerator that iterates through a DataProcessorConfigCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<DataProcessorConfig> GetEnumerator()
        {
            return this._dataProcessorConfigCollection.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a DataProcessorConfigCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._dataProcessorConfigCollection.GetEnumerator();
        }

        #endregion

        #region XML Methods

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of DataProcessorConfigCollection object
        /// </summary>
        /// <returns>Xml string representing the DataProcessorConfigCollection</returns>
        public String ToXml()
        {
            String xml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //ParallelizationEngineStatus node start
            xmlWriter.WriteStartElement("DataProcessorConfigCollection");

            #region Write Properties

            xmlWriter.WriteAttributeString("IsParallelProcessingEngineRunning", this.IsParallelProcessingEngineRunning.ToString().ToLowerInvariant());
            StringBuilder stringBuilder = new StringBuilder();

            foreach (DataProcessorConfig dataProcessorConfig in this._dataProcessorConfigCollection)
            {
                stringBuilder.Append(dataProcessorConfig.ToXml());
            }

            xmlWriter.WriteRaw(stringBuilder.ToString());

            #endregion

            //Denorm Result node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        #endregion ToXml methods

        #endregion
    }
}
