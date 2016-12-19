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
    /// Specifies RematchRuleCondition Collection
    /// </summary>
    [DataContract]
    public class RematchRuleConditionCollection : InterfaceContractCollection<IRematchRuleCondition, RematchRuleCondition>, IRematchRuleConditionCollection
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the RematchRuleCondition Collection
        /// </summary>
        public RematchRuleConditionCollection()
        { }

        /// <summary>
        /// Initialize RematchRuleCondition collection from IList
        /// </summary>
        /// <param name="rematchRuleConditionList">Source items</param>
        public RematchRuleConditionCollection(IList<RematchRuleCondition> rematchRuleConditionList)
        {
            this._items = new Collection<RematchRuleCondition>(rematchRuleConditionList);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get Xml representation of RematchRuleCondition Collection
        /// </summary>
        public String ToXml(Boolean withOuterNode)
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            if (withOuterNode)
            {
                //RematchRuleConditions node start
                xmlWriter.WriteStartElement("RematchRuleConditions");
            }

            foreach (RematchRuleCondition item in _items)
            {
                xmlWriter.WriteRaw(item.ToXml());
            }

            if (withOuterNode)
            {
                //RematchRuleConditions node end
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
        /// Loads RematchRuleCondition Collection from "RematchRuleConditions" XML node
        /// </summary>
        public void LoadFromXml(XmlNode node)
        {
            _items.Clear();
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "RematchRuleCondition")
                {
                    RematchRuleCondition item = new RematchRuleCondition();
                    item.LoadFromXml(child);
                    _items.Add(item);
                }
            }
        }

        /// <summary>
        /// Loads RematchRuleCondition Collection from XML
        /// </summary>
        public void LoadFromXml(String xml)
        {
            _items.Clear();
            if (!String.IsNullOrWhiteSpace(xml))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                XmlNode node = doc.SelectSingleNode("RematchRuleConditions");
                if (node != null)
                {
                    LoadFromXml(node);
                }
            }
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Clone RematchRuleCondition collection
        /// </summary>
        /// <returns>Cloned RematchRuleCondition collection object</returns>
        public object Clone()
        {
            RematchRuleConditionCollection clonedConditions = new RematchRuleConditionCollection();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (RematchRuleCondition item in this._items)
                {
                    RematchRuleCondition clonedItem = item.Clone() as RematchRuleCondition;
                    clonedConditions.Add(clonedItem);
                }
            }

            return clonedConditions;
        }

        #endregion
    }
}