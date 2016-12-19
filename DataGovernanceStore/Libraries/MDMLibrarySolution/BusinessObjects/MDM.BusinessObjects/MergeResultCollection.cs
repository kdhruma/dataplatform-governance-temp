using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;
    using MDM.Core;

    /// <summary>
    /// Specifies MergeResult Collection
    /// </summary>
    [DataContract]
    public class MergeResultCollection : InterfaceContractCollection<IMergeResult, MergeResult>, IMergeResultCollection
    {
        #region Fields

        /// <summary>
        /// Denotes total results count for paging
        /// </summary>
        private Int64? _totalItemsCount = null;

        #endregion

        #region Constructors

		/// <summary>
        /// Initializes a new instance of the MergeResult Collection
        /// </summary>
        public MergeResultCollection()
        { }

        /// <summary>
        /// Initialize MergeResult collection from IList
        /// </summary>
        /// <param name="mergeResultList">Source items</param>
        public MergeResultCollection(IList<MergeResult> mergeResultList)
        {
            this._items = new Collection<MergeResult>(mergeResultList);
        } 

	    #endregion

        #region Properties

        /// <summary>
        /// Denotes total results count for paging
        /// </summary>
        [DataMember]
        public Int64? TotalItemsCount
        {
            get { return _totalItemsCount; }
            set { _totalItemsCount = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get Xml representation of MergeResult Collection
        /// </summary>
        public String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("MergeResults");

            foreach (MergeResult item in _items)
            {
                xmlWriter.WriteRaw(item.ToXml());
            }

            if (TotalItemsCount.HasValue)
            {
                xmlWriter.WriteStartElement("TotalItemsCount");
                xmlWriter.WriteValue(TotalItemsCount.Value.ToString(CultureInfo.InvariantCulture));
                xmlWriter.WriteEndElement();
            }

            //MergeResults node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            String result = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return result;
        }

        /// <summary>
        /// Loads MergeResult Collection from "MergeResults" XML node
        /// </summary>
        public void LoadFromXml(XmlNode node)
        {
            _items.Clear();
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "MergeResult")
                {
                    MergeResult item = new MergeResult();
                    item.LoadFromXml(child);
                    _items.Add(item);
                }
                else if (child.Name == "TotalItemsCount")
                {
                    TotalItemsCount = ValueTypeHelper.ConvertToNullableInt64(child.InnerText);
                }
            }
        }

        /// <summary>
        /// Loads MergeResult Collection from XML
        /// </summary>
        public void LoadFromXml(String xml)
        {
            _items.Clear();
            if (!String.IsNullOrWhiteSpace(xml))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                XmlNode node = doc.SelectSingleNode("MergeResults");
                if (node != null)
                {
                    LoadFromXml(node);
                }
            }
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Clone MergeResult collection
        /// </summary>
        /// <returns>Cloned MergeResult collection object</returns>
        public object Clone()
        {
            MergeResultCollection clonedResults = new MergeResultCollection();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (MergeResult item in this._items)
                {
                    MergeResult clonedItem = item.Clone() as MergeResult;
                    clonedResults.Add(clonedItem);
                }
            }
            clonedResults.TotalItemsCount = this.TotalItemsCount;
            
            return clonedResults;
        }

        #endregion
    }
}