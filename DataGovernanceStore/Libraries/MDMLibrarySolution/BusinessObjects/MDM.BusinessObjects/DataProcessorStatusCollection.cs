using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;

    /// <summary>
    /// Represents class for data processor status collection
    /// </summary>
    [DataContract]
    public class DataProcessorStatusCollection : ICollection<DataProcessorStatus>, IEnumerable<DataProcessorStatus>, IDataProcessorStatusCollection
    {
        #region Fields

        [DataMember]
        private Collection<DataProcessorStatus> _dataProcessorStatusCollection = new Collection<DataProcessorStatus>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public DataProcessorStatusCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public DataProcessorStatusCollection(String valueAsXml)
        {
            LoadDataProcessorStatusCollection(valueAsXml);
        }

        /// <summary>
        /// Initialize DataProcessorStatusCollection from IList
        /// </summary>
        /// <param name="dataProcessorStatusList">IList of data processor status</param>
        public DataProcessorStatusCollection(IList<DataProcessorStatus> dataProcessorStatusList)
        {
            this._dataProcessorStatusCollection = new Collection<DataProcessorStatus>(dataProcessorStatusList);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Find DataProcessorStatus from DataProcessorStatusCollection based on processor name
        /// </summary>
        /// <param name="processorName">Indicates the name of processor</param>
        /// <returns>DataProcessorStatus object having given processor name</returns>
        public DataProcessorStatus this[String processorName]
        {
            get
            {
                DataProcessorStatus dataProcessorStatus = GetDataProcessorStatus(processorName);
                if(dataProcessorStatus == null)
                    throw new ArgumentException(String.Format("No DataProcessor found for Name: {0}", processorName), "processorName");
                else
                    return dataProcessorStatus;
            }
            set
            {
                DataProcessorStatus dataProcessorStatus = GetDataProcessorStatus(processorName);
                if(dataProcessorStatus == null)
                    throw new ArgumentException(String.Format("No DataProcessor found for Name: {0}", processorName), "processorName");

                dataProcessorStatus = value;
            }
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
            if(obj is DataProcessorStatusCollection)
            {
                DataProcessorStatusCollection objectToBeCompared = obj as DataProcessorStatusCollection;
                Int32 dataProcessorStatusUnion = this._dataProcessorStatusCollection.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 dataProcessorStatusIntersect = this._dataProcessorStatusCollection.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if(dataProcessorStatusUnion != dataProcessorStatusIntersect)
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
            foreach(DataProcessorStatus attr in this._dataProcessorStatusCollection)
            {
                hashCode += attr.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// Load data processor status collection object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        ///     <para>
        ///     <![CDATA[
        ///        <DataProcessorStatus>
        ///        </DataProcessorStatus>
        ///     ]]>    
        ///     </para>
        /// </example>
        public void LoadDataProcessorStatusCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name.ToLowerInvariant() == "dataprocessorstatus")
                        {
                            String DataProcessorStatusXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(DataProcessorStatusXml))
                            {
                                DataProcessorStatus dataProcessorStatus = new DataProcessorStatus(DataProcessorStatusXml);
                                this.Add(dataProcessorStatus);
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

        #region Private Methods

        /// <summary>
        /// Get the DataProcessorStatus based on the data processor name
        /// </summary>
        /// <param name="processorName">Name of DataProcessor</param>
        /// <returns></returns>
        private DataProcessorStatus GetDataProcessorStatus(String processorName)
        {
            var filteredDataProcessorStatus = from dataProcessorStatus in this._dataProcessorStatusCollection
                                            where dataProcessorStatus.ProcessorName == processorName
                                            select dataProcessorStatus;

            if(filteredDataProcessorStatus.Any())
                return filteredDataProcessorStatus.First();
            else
                return null;
        }

        #endregion

        #region ICollection<DataProcessorStatus> Members

        /// <summary>
        /// Add DataProcessorStatus object in collection
        /// </summary>
        /// <param name="item">DataProcessorStatus to add in collection</param>
        public void Add(DataProcessorStatus item)
        {
            this._dataProcessorStatusCollection.Add(item);
        }

        /// <summary>
        /// Removes all DataProcessorStatus from collection
        /// </summary>
        public void Clear()
        {
            this._dataProcessorStatusCollection.Clear();
        }

        /// <summary>
        /// Determines whether the DataProcessorStatusCollection contains a specific DataProcessorStatus.
        /// </summary>
        /// <param name="item">The DataProcessorStatus object to locate in the DataProcessorStatusCollection.</param>
        /// <returns>
        /// <para>true : If DataProcessorStatus found in mappingCollection</para>
        /// <para>false : If DataProcessorStatus found not in mappingCollection</para>
        /// </returns>
        public bool Contains(DataProcessorStatus item)
        {
            return this._dataProcessorStatusCollection.Contains(item);
        }

        /// <summary>
        /// Determines whether the data processor status collection contains a specific dataProcessorStatus based on processor name
        /// </summary>
        /// <param name="processorName">Indicates the name of processor</param>
        /// <returns>
        /// <para>true : If processor name is found in mappingCollection</para>
        /// <para>false : If processor name is not found in mappingCollection</para>
        /// </returns>
        public bool Contains(String processorName)
        {
            if(GetDataProcessorStatus(processorName) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Copies the elements of the DataProcessorStatusCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from DataProcessorStatusCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(DataProcessorStatus[] array, int arrayIndex)
        {
            this._dataProcessorStatusCollection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of DataProcessorStatus in DataProcessorStatusCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._dataProcessorStatusCollection.Count;
            }
        }

        /// <summary>
        /// Check if DataProcessorStatusCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the DataProcessorStatusCollection.
        /// </summary>
        /// <param name="item">The denorm result object to remove from the DataProcessorStatusCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original DataProcessorStatusCollection</returns>
        public bool Remove(DataProcessorStatus item)
        {
            return this._dataProcessorStatusCollection.Remove(item);
        }

        /// <summary>
        /// Removes a specific data processor with provided short name from DataProcessorStatusCollection.
        /// </summary>
        /// <param name="processorName">Name of the processor which has to remove from the DataProcessorStatusCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original DataProcessorStatusCollection</returns>
        public bool Remove(String processorName)
        {
            if (this._dataProcessorStatusCollection != null && !String.IsNullOrWhiteSpace(processorName))
            {
                foreach (DataProcessorStatus dataProcessorStatus in this._dataProcessorStatusCollection)
                {
                    if (dataProcessorStatus.ProcessorName.Equals(processorName))
                    {
                        this._dataProcessorStatusCollection.Remove(dataProcessorStatus);
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion

        #region IEnumerable<DataProcessorStatus> Members

        /// <summary>
        /// Returns an enumerator that iterates through a DataProcessorStatusCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<DataProcessorStatus> GetEnumerator()
        {
            return this._dataProcessorStatusCollection.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a DataProcessorStatusCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ( IEnumerator ) this._dataProcessorStatusCollection.GetEnumerator();
        }

        #endregion

        #region XML Methods

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of DataProcessorStatusCollection object
        /// </summary>
        /// <returns>Xml string representing the DataProcessorStatusCollection</returns>
        public String ToXml()
        {
            String dataProcessorStatussXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach(DataProcessorStatus dataProcessorStatus in this._dataProcessorStatusCollection)
            {
                builder.Append(dataProcessorStatus.ToXml());
            }

            dataProcessorStatussXml = String.Format("<DataProcessorStatusCollection>{0}</DataProcessorStatusCollection>", builder.ToString());
            return dataProcessorStatussXml;
        }

        #endregion ToXml methods

        #endregion

    }
}
