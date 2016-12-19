using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.DQM
{
    using MDM.BusinessObjects;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies MergePlanItem Collection
    /// </summary>
    [DataContract]
    public class MergePlanItemCollection : InterfaceContractCollection<IMergePlanItem, MergePlanItem>, IMergePlanItemCollection
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the MergePlanItem Collection
        /// </summary>
        public MergePlanItemCollection()
        { }

        /// <summary>
        /// Initializes a new instance of the MergePlanItem Collection from xml string
        /// </summary>
        public MergePlanItemCollection(String xml)
        {
            LoadFromXmlWithOuterNode(xml);
        }

        /// <summary>
        /// Initialize MergePlanItem collection from IList
        /// </summary>
        /// <param name="mergePlanItemList">Source items</param>
        public MergePlanItemCollection(IList<MergePlanItem> mergePlanItemList)
        {
            this._items = new Collection<MergePlanItem>(mergePlanItemList);
        }

        #endregion

        #region Public Methods
        
        /// <summary>
        /// Allows to add specified items to collection
        /// </summary>
        /// <param name="collection">Items to add</param>
        public void AddRange(IEnumerable<MergePlanItem> collection)
        {
            if (collection == null)
            {
                return;
            }
            foreach (MergePlanItem item in collection)
            {
                _items.Add(item);
            }
        }

        /// <summary>
        /// Get Xml representation of MergePlanItem Collection
        /// </summary>
        public String ToXml(Boolean withOuterNode)
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            if (withOuterNode)
            {
                //MergePlanItems node start
                xmlWriter.WriteStartElement("MergePlanItems");
            }

            foreach (MergePlanItem item in _items)
            {
                xmlWriter.WriteRaw(item.ToXml());
            }

            if (withOuterNode)
            {
                //MergePlanItems node end
                xmlWriter.WriteEndElement();
            }

            xmlWriter.Flush();

            //Get the actual XML
            String result = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return result;
        }

        /// <summary>
        /// Loads MergePlanItem Collection from "MergePlanItems" XML node
        /// </summary>
        public void LoadFromXml(XmlNode node)
        {
            _items.Clear();
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "MergePlanItem")
                {
                    MergePlanItem item = new MergePlanItem();
                    item.LoadFromXml(child);
                    _items.Add(item);
                }
            }
        }

        /// <summary>
        /// Loads MergePlanItem Collection from XML with "MergePlanItems" outer node
        /// </summary>
        public void LoadFromXmlWithOuterNode(String xml)
        {
            _items.Clear();
            if (!String.IsNullOrWhiteSpace(xml))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                XmlNode node = doc.SelectSingleNode("MergePlanItems");
                if (node != null)
                {
                    LoadFromXml(node);
                }
            }
        }

        #endregion
    }
}