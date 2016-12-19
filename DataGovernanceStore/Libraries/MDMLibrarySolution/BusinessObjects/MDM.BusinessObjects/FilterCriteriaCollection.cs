using System.IO;
using System.Xml;
using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.BusinessObjects.Interfaces;

    /// <summary>
    /// Represents the collection of FilterCriteria
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class FilterCriteriaCollection : InterfaceContractCollection<IFilterCriteria, FilterCriteria>, IFilterCriteriaCollection
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the FilterCriteria Collection
        /// </summary>
        public FilterCriteriaCollection()
        { }

        /// <summary>
        /// Initialize FilterCriteria collection from IList
        /// </summary>
        /// <param name="filterCriteriaList">Source items</param>
        public FilterCriteriaCollection(IList<FilterCriteria> filterCriteriaList)
        {
            this._items = new Collection<FilterCriteria>(filterCriteriaList);
        }

        /// <summary>
        /// Initialize FilterCriteria collection from xml
        /// </summary>
        /// <param name="xml">Xml String</param>
        public FilterCriteriaCollection(String xml)
            : this()
        {
            LoadFromXml(xml);
        }

        #endregion
        
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
                xmlWriter.WriteStartElement("FilterCriteriaCollection");
                foreach (FilterCriteria item in _items)
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
            _items.Clear();
            if (node == null)
            {
                return;
            }
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "FilterCriteria")
                {
                    FilterCriteria item = new FilterCriteria();
                    item.LoadFromXml(child);
                    _items.Add(item);
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
            XmlNode node = doc.SelectSingleNode("FilterCriteriaCollection");
            if (node != null)
            {
                LoadFromXml(node);
            }
        }

        #endregion

    }
}