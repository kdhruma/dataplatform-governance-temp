using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Interfaces;
    using MDM.Core.Extensions;

    /// <summary>
    /// Specifies DataQualityIndicatorFailureInfo Collection
    /// </summary>
    [DataContract]
    public class DataQualityIndicatorFailureInfoCollection : InterfaceContractCollection<IDataQualityIndicatorFailureInfo, DataQualityIndicatorFailureInfo>, IDataQualityIndicatorFailureInfoCollection
    {
        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public DataQualityIndicatorFailureInfoCollection()
        {
        }

        /// <summary>
        /// Initializes DataQualityIndicatorFailureInfoCollection from xml
        /// </summary>
        /// <param name="xml"></param>
        public DataQualityIndicatorFailureInfoCollection(String xml)
        {
            LoadFromXmlWithOuterNode(xml);
        }

        /// <summary>
        /// Initializes DataQualityIndicatorFailureInfoCollection from IList
        /// </summary>
        /// <param name="dataQualityIndicatorErrorInfo">IList of DataQualityIndicatorFailureInfo</param>
        public DataQualityIndicatorFailureInfoCollection(IList<DataQualityIndicatorFailureInfo> dataQualityIndicatorErrorInfo)
        {
            this._items = new Collection<DataQualityIndicatorFailureInfo>(dataQualityIndicatorErrorInfo);
        }

        #endregion

        #region Methods

        #region Xml Serialization

        /// <summary>
        /// Denotes method for xml serialization
        /// </summary>
        public String ToXml(Boolean withOuterNode = true)
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            if (withOuterNode)
            {
                xmlWriter.WriteStartElement("DataQualityIndicatorFailureInfoCollection");
            }

            foreach (DataQualityIndicatorFailureInfo dataQualityIndicatorErrorInfo in this)
            {
                xmlWriter.WriteRaw(dataQualityIndicatorErrorInfo.ToXml());
            }

            if (withOuterNode)
            {
                xmlWriter.WriteEndElement();
            }

            xmlWriter.Flush();

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

                XmlNode node = doc.SelectSingleNode("DataQualityIndicatorFailureInfoCollection");
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
            _items.Clear();
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "DataQualityIndicatorFailureInfo")
                {
                    DataQualityIndicatorFailureInfo item = new DataQualityIndicatorFailureInfo();
                    item.LoadFromXml(child);
                    _items.Add(item);
                }
            }
        }

        #endregion
        
        #region Equality Comparision

        /// <summary>
        /// Denotes method for comparing DataQualityIndicatorFailureInfoCollection
        /// </summary>
        public Boolean Equals(DataQualityIndicatorFailureInfoCollection other)
        {
            return this.EqualsIgnoringOrder(other);
        }

        /// <summary>
        /// Denotes method for comparing DataQualityIndicatorFailureInfoCollection
        /// </summary>
        public override Boolean Equals(Object obj)
        {
            return Equals(obj as DataQualityIndicatorFailureInfoCollection);
        }

        /// <summary>
        /// Denotes method for generating hashcode
        /// </summary>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                Int32 hashCode = 397;
                foreach (DataQualityIndicatorFailureInfo wordList in this)
                {
                    hashCode = (hashCode*397) ^ (wordList != null ? wordList.GetHashCode() : 0);
                }
                return hashCode;
            }
        }

        #endregion

        #endregion
    }
}