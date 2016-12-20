using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.Collections;
using System.Xml;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Interfaces;

    /// <summary>
    /// Specifies DataQualityClass collection
    /// </summary>
    [DataContract]
    public class DataQualityClassCollection : IDataQualityClassCollection, ICloneable
    {
        #region Fields

        [DataMember]
        private Collection<DataQualityClass> _dataQualityClasses = new Collection<DataQualityClass>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public DataQualityClassCollection()
        {
            this._dataQualityClasses = new Collection<DataQualityClass>();
        }

        /// <summary>
        /// Initialize DataQualityClassCollection from IList
        /// </summary>
        /// <param name="DataQualityClassList">IList of DataQualityClass</param>
        public DataQualityClassCollection(IList<DataQualityClass> DataQualityClassList)
        {
            this._dataQualityClasses = new Collection<DataQualityClass>(DataQualityClassList);
        }

        /// <summary>
        /// Initialize DataQualityClassCollection from xml
        /// </summary>
        /// <param name="xml">Xml String</param>
        public DataQualityClassCollection(String xml)
            : this()
        {
            LoadFromXml(xml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Indexer to getting DataQualityClass by Id
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public DataQualityClass this[Int32 index]
        {
            get { return _dataQualityClasses[index]; }
        }

        #endregion

        #region Methods

        #region Public Method

        /// <summary>
        /// Clone data quality class collection
        /// </summary>
        /// <returns>Cloned filter criteria settings collection object</returns>
        public object Clone()
        {
            DataQualityClassCollection clonedProfiles = new DataQualityClassCollection();

            if (this._dataQualityClasses != null && this._dataQualityClasses.Count > 0)
            {
                foreach (DataQualityClass dataQualityClass in this._dataQualityClasses)
                {
                    DataQualityClass clonedProfile = dataQualityClass.Clone() as DataQualityClass;
                    clonedProfiles.Add(clonedProfile);
                }
            }

            return clonedProfiles;
        }

        /// <summary>
        /// Remove DataQualityClass object from DataQualityClassCollection
        /// </summary>
        /// <param name="dataQualityClassId">Id of DataQualityClass which information is to be removed from collection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public Boolean Remove(Int32 dataQualityClassId)
        {
            DataQualityClass dataQualityClass = Get(dataQualityClassId);
            if (dataQualityClass == null)
            {
                return false;
            }
            return this.Remove(dataQualityClass);
        }

        /// <summary>
        /// Determines whether two Object instances are equal
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false</returns>
        public override Boolean Equals(object obj)
        {
            if (obj != null)
            {
                DataQualityClassCollection objectToBeCompared = obj as DataQualityClassCollection;
                if (objectToBeCompared != null)
                {
                    if (ReferenceEquals(this, objectToBeCompared))
                    {
                        return true;
                    }

                    List<DataQualityClass> thisItems = this._dataQualityClasses.ToList();
                    List<DataQualityClass> otherItems = objectToBeCompared.ToList();

                    Int32 unionCount = thisItems.Union(otherItems).Count();
                    Int32 intersectCount = thisItems.Intersect(otherItems).Count();

                    return (unionCount == intersectCount);
                }
            }
            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object</returns>
        public override Int32 GetHashCode()
        {
            return this._dataQualityClasses.Sum(item => item.GetHashCode());
        }
        
        #region Xml Serialization

        /// <summary>
        /// Gets Xml representation of current object
        /// </summary>
        /// <returns>Returns Xml representation of current object as string</returns>
        public String ToXml()
        {
            String result = String.Empty;

            using (StringWriter sw = new StringWriter())
            using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
            {
                xmlWriter.WriteStartElement("DataQualityClassCollection");
                foreach (DataQualityClass item in _dataQualityClasses)
                {
                    xmlWriter.WriteRaw(item.ToXml());
                }
                xmlWriter.WriteEndElement();
                xmlWriter.Flush();

                //Get the actual XML
                result = sw.ToString();
            }

            return result;
        }

        /// <summary>
        /// Loads current object from provided XmlNode
        /// </summary>
        /// <param name="node">XmlNode for deserialization</param>
        public void LoadFromXml(XmlNode node)
        {
            _dataQualityClasses.Clear();
            if (node == null)
            {
                return;
            }
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "DataQualityClass")
                {
                    DataQualityClass item = new DataQualityClass();
                    item.LoadFromXml(child);
                    _dataQualityClasses.Add(item);
                }
            }
        }

        /// <summary>
        /// Loads current object from provided Xml
        /// </summary>
        /// <param name="xml">Xml string for deserialization</param>
        public void LoadFromXml(String xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode node = doc.SelectSingleNode("DataQualityClassCollection");
            if (node != null)
            {
                LoadFromXml(node);
            }
        }

        #endregion

        #endregion

        #region Private Method

        /// <summary>
        /// Get DataQualityClass from current data quality classes based on data quality class Id
        /// </summary>
        /// <param name="dataQualityClassId">Id of data quality class which information is to be searched</param>
        /// <returns>DataQualityClass having given DataQualityClassId messageCode</returns>
        private DataQualityClass Get(Int32 dataQualityClassId)
        {
            return this._dataQualityClasses.FirstOrDefault(dataQualityClass => dataQualityClass.Id == dataQualityClassId);
        }

        #endregion

        #endregion

        #region ICollection<DataQualityClass> Members

        /// <summary>
        /// Add DataQualityClass object in collection
        /// </summary>
        /// <param name="item">DataQualityClass to add in collection</param>
        public void Add(DataQualityClass item)
        {
            this._dataQualityClasses.Add(item);
        }

        /// <summary>
        /// Add IDataQualityClass object in collection
        /// </summary>
        /// <param name="item">IDataQualityClass to add in collection</param>
        public void Add(IDataQualityClass item)
        {
            this._dataQualityClasses.Add((DataQualityClass)item);
        }

        /// <summary>
        /// Removes all DataQualityClass from collection
        /// </summary>
        public void Clear()
        {
            this._dataQualityClasses.Clear();
        }

        /// <summary>
        /// Determines whether the DataQualityClassCollection contains a specific DataQualityClass
        /// </summary>
        /// <param name="item">The DataQualityClass object to locate in the DataQualityClassCollection</param>
        /// <returns>
        /// <param>true : If DataQualityClass found in DataQualityClassCollection</param>
        /// <param>false : If DataQualityClass found not in DataQualityClassCollection</param>
        /// </returns>
        public Boolean Contains(DataQualityClass item)
        {
            return this._dataQualityClasses.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the DataQualityClassCollection to an
        /// System.Array, starting at a particular System.Array index
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements copied from DataQualityClassCollection. The System.Array must have zero-based indexing
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins</param>
        public void CopyTo(DataQualityClass[] array, Int32 arrayIndex)
        {
            this._dataQualityClasses.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of DataQualityClass in DataQualityClassCollection
        /// </summary>
        public Int32 Count
        {
            get
            {
                return this._dataQualityClasses.Count;
            }
        }

        /// <summary>
        /// Check if DataQualityClassCollection is read-only
        /// </summary>
        public Boolean IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the DataQualityClassCollection
        /// </summary>
        /// <param name="item">The DataQualityClass object to remove from the DataQualityClassCollection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original DataQualityClassCollection</returns>
        public Boolean Remove(DataQualityClass item)
        {
            return this._dataQualityClasses.Remove(item);
        }

        #endregion

        #region IEnumerable<DataQualityClass> Members

        /// <summary>
        /// Returns an enumerator that iterates through a DataQualityClassCollection
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection</returns>
        public IEnumerator<DataQualityClass> GetEnumerator()
        {
            return this._dataQualityClasses.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a DataQualityClassCollection
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._dataQualityClasses.GetEnumerator();
        }

        #endregion
    }
}
