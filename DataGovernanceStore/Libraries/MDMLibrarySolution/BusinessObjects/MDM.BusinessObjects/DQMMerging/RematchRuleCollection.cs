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
    /// Specifies RematchRule Collection
    /// </summary>
    [DataContract]
    public class RematchRuleCollection : InterfaceContractCollection<IRematchRule, RematchRule>, IRematchRuleCollection
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the RematchRule Collection
        /// </summary>
        public RematchRuleCollection()
        { }

        /// <summary>
        /// Initialize RematchRule collection from IList
        /// </summary>
        /// <param name="rematchRuleList">Source items</param>
        public RematchRuleCollection(IList<RematchRule> rematchRuleList)
        {
            this._items = new Collection<RematchRule>(rematchRuleList);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get Xml representation of RematchRule Collection
        /// </summary>
        public String ToXml(Boolean withOuterNode)
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            if (withOuterNode)
            {
                //RematchRules node start
                xmlWriter.WriteStartElement("RematchRules");
            }

            foreach (RematchRule item in _items)
            {
                xmlWriter.WriteRaw(item.ToXml());
            }

            if (withOuterNode)
            {
                //RematchRules node end
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
        /// Loads RematchRule Collection from "RematchRules" XML node
        /// </summary>
        public void LoadFromXml(XmlNode node)
        {
            _items.Clear();
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "RematchRule")
                {
                    RematchRule item = new RematchRule();
                    item.LoadFromXml(child);
                    _items.Add(item);
                }
            }
        }

        /// <summary>
        /// Loads RematchRule Collection from XML
        /// </summary>
        public void LoadFromXml(String xml)
        {
            _items.Clear();
            if (!String.IsNullOrWhiteSpace(xml))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                XmlNode node = doc.SelectSingleNode("RematchRules");
                if (node != null)
                {
                    LoadFromXml(node);
                }
            }
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Clone RematchRule collection
        /// </summary>
        /// <returns>Cloned RematchRule collection object</returns>
        public object Clone()
        {
            RematchRuleCollection clonedRules = new RematchRuleCollection();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (RematchRule item in this._items)
                {
                    RematchRule clonedItem = item.Clone() as RematchRule;
                    clonedRules.Add(clonedItem);
                }
            }

            return clonedRules;
        }

        #endregion
    }
}