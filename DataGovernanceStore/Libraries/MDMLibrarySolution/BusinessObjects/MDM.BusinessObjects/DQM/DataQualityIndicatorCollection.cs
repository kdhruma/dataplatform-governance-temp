using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the DataQualityIndicator Collection
    /// </summary>
    [DataContract]
    public class DataQualityIndicatorCollection : IDataQualityIndicatorCollection,  ICloneable
    {
        #region Fields

        [DataMember]
        private Collection<DataQualityIndicator> _DataQualityIndicators = new Collection<DataQualityIndicator>();

        private const String XmlNodeName = "DataQualityIndicatorCollection";
        private const String ChildXmlNodeName = "DataQualityIndicator";

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public DataQualityIndicatorCollection()
        {
            this._DataQualityIndicators = new Collection<DataQualityIndicator>();
        }

        /// <summary>
        /// Initialize DataQualityIndicatorCollection from IList
        /// </summary>
        /// <param name="dataQualityIndicators">IList of DataQualityIndicators</param>
        public DataQualityIndicatorCollection(IList<DataQualityIndicator> dataQualityIndicators)
        {
            this._DataQualityIndicators = new Collection<DataQualityIndicator>(dataQualityIndicators);
        }

        /// <summary>
        /// Initialize DataQualityIndicatorCollection from IList
        /// </summary>
        /// <param name="xml">Xml String</param>
        public DataQualityIndicatorCollection(String xml)
            : this()
        {
            LoadFromXmlWithOuterNode(xml);
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region Public Method

        /// <summary>
        /// Clone DataQualityIndicator collection
        /// </summary>
        /// <returns>Cloned DataQualityIndicator collection object</returns>
        public object Clone()
        {
            DataQualityIndicatorCollection clonedCollection = new DataQualityIndicatorCollection();

            if (this._DataQualityIndicators != null && this._DataQualityIndicators.Count > 0)
            {
                foreach (DataQualityIndicator dataQualityIndicator in this._DataQualityIndicators)
                {
                    DataQualityIndicator clonedDataQualityIndicator = dataQualityIndicator.Clone() as DataQualityIndicator;
                    clonedCollection.Add(clonedDataQualityIndicator);
                }
            }

            return clonedCollection;
        }

        /// <summary>
        /// Remove DataQualityIndicator object from DataQualityIndicatorCollection
        /// </summary>
        /// <param name="dataQualityIndicatorId">Id of DataQualityIndicator which is to be removed from collection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public Boolean Remove(Int16 dataQualityIndicatorId)
        {
            DataQualityIndicator dataQualityIndicator = Get(dataQualityIndicatorId);

            return dataQualityIndicator != null && this.Remove(dataQualityIndicator);
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
                DataQualityIndicatorCollection objectToBeCompared = obj as DataQualityIndicatorCollection;
                if (objectToBeCompared != null)
                {
                    if (ReferenceEquals(this, objectToBeCompared))
                    {
                        return true;
                    }

                    Int32 dataQualityIndicatorUnion = this._DataQualityIndicators.ToList().Union(objectToBeCompared.ToList()).Count();
                    Int32 dataQualityIndicatorIntersect = this._DataQualityIndicators.ToList().Intersect(objectToBeCompared.ToList()).Count();

                    return (dataQualityIndicatorUnion == dataQualityIndicatorIntersect);
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
            return this._DataQualityIndicators.Sum(item => item.GetHashCode());
        }

        /// <summary>
        /// Get Xml representation of DataQualityIndicator Collection
        /// </summary>
        public String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement(XmlNodeName);

            foreach (DataQualityIndicator item in _DataQualityIndicators)
            {
                xmlWriter.WriteRaw(item.ToXml());
            }

            // DataQualityIndicatorCollection node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            String result = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return result;
        }

        /// <summary>
        /// Denotes method for xml deserialization
        /// </summary>
        public void LoadFromXmlWithOuterNode(String xml)
        {
            if (!String.IsNullOrWhiteSpace(xml))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                XmlNode node = doc.SelectSingleNode(XmlNodeName);
                if (node != null)
                {
                    LoadFromXml(node);
                }
            }
        }

        /// <summary>
        /// Denotes method for xml deserialization
        /// </summary>
        public void LoadFromXml(XmlNode node)
        {
            _DataQualityIndicators.Clear();
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == ChildXmlNodeName)
                {
                    DataQualityIndicator dataQualityIndicator = new DataQualityIndicator();
                    dataQualityIndicator.LoadFromXml(child);
                    _DataQualityIndicators.Add(dataQualityIndicator);
                }
            }
        }

        #endregion

        #region Private Method

        /// <summary>
        /// Get DataQualityIndicator from current DataQualityIndicator collection based on DataQualityIndicator Id
        /// </summary>
        /// <param name="dataQualityIndicatorId">Id of DataQualityIndicator which is to be searched</param>
        /// <returns>DataQualityIndicator having given DataQualityIndicatorId messageCode</returns>
        private DataQualityIndicator Get(Int16 dataQualityIndicatorId)
        {
            IEnumerable<DataQualityIndicator> filteredDataQualityIndicator = null;

            filteredDataQualityIndicator = this._DataQualityIndicators.Where(a => a.Id == dataQualityIndicatorId);

            return filteredDataQualityIndicator != null && filteredDataQualityIndicator.Any() ? filteredDataQualityIndicator.First() : null;
        }

        #endregion

        #endregion

        #region ICollection<DataQualityIndicator> Members

        /// <summary>
        /// Add DataQualityIndicator object in collection
        /// </summary>
        /// <param name="item">DataQualityIndicator to add in collection</param>
        public void Add(DataQualityIndicator item)
        {
            this._DataQualityIndicators.Add(item);
        }

        /// <summary>
        /// Add IDataQualityIndicator object in collection
        /// </summary>
        /// <param name="item">IDataQualityIndicator to add in collection</param>
        public void Add(IDataQualityIndicator item)
        {
            this._DataQualityIndicators.Add((DataQualityIndicator)item);
        }

        /// <summary>
        /// Removes all DataQualityIndicator from collection
        /// </summary>
        public void Clear()
        {
            this._DataQualityIndicators.Clear();
        }

        /// <summary>
        /// Determines whether the DataQualityIndicatorCollection contains a specific DataQualityIndicator
        /// </summary>
        /// <param name="item">The DataQualityIndicator object to locate in the DataQualityIndicatorCollection</param>
        /// <returns>
        /// <para>true : If DataQualityIndicator found in DataQualityIndicatorCollection</para>
        /// <para>false : If DataQualityIndicator found not in DataQualityIndicatorCollection</para>
        /// </returns>
        public Boolean Contains(DataQualityIndicator item)
        {
            return this._DataQualityIndicators.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the DataQualityIndicatorCollection to an
        /// System.Array, starting at a particular System.Array index
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements copied from DataQualityIndicatorCollection. The System.Array must have zero-based indexing
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins</param>
        public void CopyTo(DataQualityIndicator[] array, Int32 arrayIndex)
        {
            this._DataQualityIndicators.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of DataQualityIndicator in DataQualityIndicatorCollection
        /// </summary>
        public Int32 Count
        {
            get
            {
                return this._DataQualityIndicators.Count;
            }
        }

        /// <summary>
        /// Check if DataQualityIndicatorCollection is read-only
        /// </summary>
        public Boolean IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the DataQualityIndicatorCollection
        /// </summary>
        /// <param name="item">The DataQualityIndicator object to remove from the DataQualityIndicatorCollection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original DataQualityIndicatorCollection</returns>
        public Boolean Remove(DataQualityIndicator item)
        {
            return this._DataQualityIndicators.Remove(item);
        }

        #endregion

        #region IEnumerable<DataQualityIndicator> Members

        /// <summary>
        /// Returns an enumerator that iterates through a DataQualityIndicatorCollection
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection</returns>
        public IEnumerator<DataQualityIndicator> GetEnumerator()
        {
            return this._DataQualityIndicators.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a DataQualityIndicatorCollection
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._DataQualityIndicators.GetEnumerator();
        }

        #endregion
    }
}