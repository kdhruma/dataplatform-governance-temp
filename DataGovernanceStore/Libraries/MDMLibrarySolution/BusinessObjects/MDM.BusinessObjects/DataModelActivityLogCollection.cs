using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;

    /// <summary>
    /// Represents class for entity activity log collection
    /// </summary>
    [DataContract]
    public class DataModelActivityLogCollection : ICollection<DataModelActivityLog>, IEnumerable<DataModelActivityLog>, IDataModelActivityLogCollection
    {
        #region Fields

        [DataMember]
        private Collection<DataModelActivityLog> _dataModelActivityLogCollection = new Collection<DataModelActivityLog>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public DataModelActivityLogCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public DataModelActivityLogCollection(String valueAsXml)
        {
            LoadDataModelActivityLogCollection(valueAsXml);
        }

        /// <summary>
        /// Initialize DataModelActivityLogCollection from IList
        /// </summary>
        /// <param name="DataModelActivityLogList">IList of denorm result</param>
        public DataModelActivityLogCollection(IList<DataModelActivityLog> DataModelActivityLogList)
        {
            this._dataModelActivityLogCollection = new Collection<DataModelActivityLog>(DataModelActivityLogList);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Find DataModelActivityLog from DataModelActivityLogCollection based on Id
        /// </summary>
        /// <param name="Id">Id to search in result collection</param>
        /// <returns>DataModelActivityLog object having given Id</returns>
        public DataModelActivityLog this[Int64 Id]
        {
            get
            {
                DataModelActivityLog DataModelActivityLog = GetDataModelActivityLog(Id);
                if (DataModelActivityLog == null)
                    throw new ArgumentException(String.Format("No result found for id: {0}", Id), "Id");
                else
                    return DataModelActivityLog;
            }
            set
            {
                DataModelActivityLog DataModelActivityLog = GetDataModelActivityLog(Id);
                if (DataModelActivityLog == null)
                    throw new ArgumentException(String.Format("No result found for  id: {0}", Id), "Id");

                DataModelActivityLog = value;
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
            if (obj is DataModelActivityLogCollection)
            {
                DataModelActivityLogCollection objectToBeCompared = obj as DataModelActivityLogCollection;
                Int32 DataModelActivityLogUnion = this._dataModelActivityLogCollection.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 DataModelActivityLogIntersect = this._dataModelActivityLogCollection.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if (DataModelActivityLogUnion != DataModelActivityLogIntersect)
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
            foreach (DataModelActivityLog attr in this._dataModelActivityLogCollection)
            {
                hashCode += attr.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        public void LoadDataModelActivityLogCollection(String valuesAsXml)
        {
            #region Sample Xml
            /*
             * <DataModelActivityLog></DataModelActivityLog>
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "DataModelActivityLog")
                        {
                            String DataModelActivityLogXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(DataModelActivityLogXml))
                            {
                                DataModelActivityLog DataModelActivityLog = new DataModelActivityLog(DataModelActivityLogXml);
                                this.Add(DataModelActivityLog);
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
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<DataModelActivityLog> GetDataModelActivityLogById(Int64 id)
        {
            var filteredDataModelActivityLog = from DataModelActivityLog in this._dataModelActivityLogCollection
                                               where DataModelActivityLog.Id == id
                                               select DataModelActivityLog;

            if (filteredDataModelActivityLog.Any())
                return filteredDataModelActivityLog.ToList<DataModelActivityLog>();
            else
                return null;

        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Get the DataModelActivityLog based on the unique Id
        /// </summary>
        /// <param name="Id">pk of DataModelActivityLog</param>
        /// <returns></returns>
        private DataModelActivityLog GetDataModelActivityLog(Int64 Id)
        {
            var filteredDataModelActivityLog = from DataModelActivityLog in this._dataModelActivityLogCollection
                                            where DataModelActivityLog.Id == Id
                                            select DataModelActivityLog;

            if (filteredDataModelActivityLog.Any())
                return filteredDataModelActivityLog.First();
            else
                return null;
        }

        #endregion

        #region ICollection<DataModelActivityLog> Members

        /// <summary>
        /// Add DataModelActivityLog object in collection
        /// </summary>
        /// <param name="item">DataModelActivityLog to add in collection</param>
        public void Add(DataModelActivityLog item)
        {
            this._dataModelActivityLogCollection.Add(item);
        }

        /// <summary>
        /// Removes all DataModelActivityLog from collection
        /// </summary>
        public void Clear()
        {
            this._dataModelActivityLogCollection.Clear();
        }

        /// <summary>
        /// Determines whether the DataModelActivityLogCollection contains a specific DataModelActivityLog.
        /// </summary>
        /// <param name="item">The DataModelActivityLog object to locate in the DataModelActivityLogCollection.</param>
        /// <returns>
        /// <para>true : If DataModelActivityLog found in mappingCollection</para>
        /// <para>false : If DataModelActivityLog found not in mappingCollection</para>
        /// </returns>
        public bool Contains(DataModelActivityLog item)
        {
            return this._dataModelActivityLogCollection.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the DataModelActivityLogCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from DataModelActivityLogCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(DataModelActivityLog[] array, int arrayIndex)
        {
            this._dataModelActivityLogCollection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Add DataModelActivityLogCollection object in current collection
        /// </summary>
        /// <param name="items">Indicates data model activity log collection to be added</param>
        public void AddRange(DataModelActivityLogCollection items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("DataModelActivityLogCollection");
            }

            foreach(DataModelActivityLog item in items)
            {
                this._dataModelActivityLogCollection.Add(item);
            }
        }
        
        /// <summary>
        /// Get the count of no. of DataModelActivityLog in DataModelActivityLogCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._dataModelActivityLogCollection.Count;
            }
        }

        /// <summary>
        /// Check if DataModelActivityLogCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the DataModelActivityLogCollection.
        /// </summary>
        /// <param name="item">The denorm result object to remove from the DataModelActivityLogCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original DataModelActivityLogCollection</returns>
        public bool Remove(DataModelActivityLog item)
        {
            return this._dataModelActivityLogCollection.Remove(item);
        }

        #endregion

        #region IEnumerable<DataModelActivityLog> Members

        /// <summary>
        /// Returns an enumerator that iterates through a DataModelActivityLogCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<DataModelActivityLog> GetEnumerator()
        {
            return this._dataModelActivityLogCollection.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a DataModelActivityLogCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._dataModelActivityLogCollection.GetEnumerator();
        }

        #endregion

        #region XML Methods

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of DataModelActivityLogCollection object
        /// </summary>
        /// <returns>Xml string representing the DataModelActivityLogCollection</returns>
        public String ToXml()
        {
            String DataModelActivityLogsXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (DataModelActivityLog DataModelActivityLog in this._dataModelActivityLogCollection)
            {
                builder.Append(DataModelActivityLog.ToXml());
            }

            DataModelActivityLogsXml = String.Format("<DataModelActivityLogs>{0}</DataModelActivityLogs>", builder.ToString());
            return DataModelActivityLogsXml;
        }

        #endregion ToXml methods

        #endregion

    }
}
