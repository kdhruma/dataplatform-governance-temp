using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.DQMMerging
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies RangeRule Collection
    /// </summary>
    [DataContract]
    public class RangeRuleCollection : InterfaceContractCollection<IRangeRule, RangeRule>, IRangeRuleCollection
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the RangeRule Collection
        /// </summary>
        public RangeRuleCollection()
        { }

        /// <summary>
        /// Initialize RangeRule collection from IList
        /// </summary>
        /// <param name="rangeRuleList">Source items</param>
        public RangeRuleCollection(IList<RangeRule> rangeRuleList)
        {
            this._items = new Collection<RangeRule>(rangeRuleList);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get Xml representation of RangeRule Collection
        /// </summary>
        public String ToXml(Boolean withOuterNode)
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            if (withOuterNode)
            {
                //RangeRules node start
                xmlWriter.WriteStartElement("RangeRules");
            }

            foreach (RangeRule item in _items)
            {
                xmlWriter.WriteRaw(item.ToXml());
            }

            if (withOuterNode)
            {
                //RangeRules node end
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
        /// Loads RangeRule Collection from "RangeRules" XML node
        /// </summary>
        public void LoadFromXml(XmlNode node)
        {
            _items.Clear();
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "RangeRule")
                {
                    RangeRule item = new RangeRule();
                    item.LoadFromXml(child);
                    _items.Add(item);
                }
            }
        }

        /// <summary>
        /// Loads RangeRule Collection from XML
        /// </summary>
        public void LoadFromXml(String xml)
        {
            _items.Clear();
            if (!String.IsNullOrWhiteSpace(xml))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                XmlNode node = doc.SelectSingleNode("RangeRules");
                if (node != null)
                {
                    LoadFromXml(node);
                }
            }
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Clone RangeRule collection
        /// </summary>
        /// <returns>Cloned RangeRule collection object</returns>
        public object Clone()
        {
            RangeRuleCollection clonedRules = new RangeRuleCollection();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (RangeRule item in this._items)
                {
                    RangeRule clonedItem = item.Clone() as RangeRule;
                    clonedRules.Add(clonedItem);
                }
            }

            return clonedRules;
        }

        #endregion
    }
}
