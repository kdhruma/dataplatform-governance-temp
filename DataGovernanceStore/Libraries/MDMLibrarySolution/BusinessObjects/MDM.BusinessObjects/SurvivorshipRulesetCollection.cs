using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies SurvivorshipRuleset Collection
    /// </summary>
    [DataContract]
    public class SurvivorshipRulesetCollection : InterfaceContractCollection<ISurvivorshipRuleset, SurvivorshipRuleset>, ISurvivorshipRulesetCollection
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the SurvivorshipRuleset Collection
        /// </summary>
        public SurvivorshipRulesetCollection()
        { }

        /// <summary>
        /// Initialize SurvivorshipRuleset collection from IList
        /// </summary>
        /// <param name="survivorshipRulesetList">Source items</param>
        public SurvivorshipRulesetCollection(IList<SurvivorshipRuleset> survivorshipRulesetList)
        {
            this._items = new Collection<SurvivorshipRuleset>(survivorshipRulesetList);
        }

        /// <summary>
        /// Initialize SurvivorshipRuleset collection from XML
        /// </summary>
        /// <param name="xml">Serialized to xml Source items</param>
        public SurvivorshipRulesetCollection(String xml) : this()
        {
            this.LoadFromXml(xml);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get Xml representation of SurvivorshipRuleset Collection
        /// </summary>
        /// <param name="withOuterNode">Specifies outer node status</param>
        public String ToXml(Boolean withOuterNode)
        {
            return ToXml(withOuterNode, ObjectSerialization.Full);
        }

        /// <summary>
        /// Get Xml representation of SurvivorshipRuleset Collection
        /// </summary>
        /// <param name="withOuterNode">Specifies outer node status</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        public String ToXml(Boolean withOuterNode, ObjectSerialization objectSerialization)
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            if (withOuterNode)
            {
                //SurvivorshipRulesets node start
                xmlWriter.WriteStartElement("SurvivorshipRulesets");
            }

            foreach (SurvivorshipRuleset item in _items)
            {
                xmlWriter.WriteRaw(item.ToXml(objectSerialization));
            }

            if (withOuterNode)
            {
                //SurvivorshipRulesets node end
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
        /// Loads SurvivorshipRuleset Collection from "SurvivorshipRulesets" XML node
        /// </summary>
        public void LoadFromXml(XmlNode node)
        {
            _items.Clear();
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "SurvivorshipRuleset")
                {
                    SurvivorshipRuleset item = new SurvivorshipRuleset();
                    item.LoadFromXml(child);
                    _items.Add(item);
                }
            }
        }

        /// <summary>
        /// Loads SurvivorshipRuleset Collection from XML
        /// </summary>
        public void LoadFromXml(String xml)
        {
            _items.Clear();
            if (!String.IsNullOrWhiteSpace(xml))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                XmlNode node = doc.SelectSingleNode("SurvivorshipRulesets");
                if (node != null)
                {
                    LoadFromXml(node);
                }
            }
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Clone SurvivorshipRuleset collection
        /// </summary>
        /// <returns>Cloned SurvivorshipRuleset collection object</returns>
        public object Clone()
        {
            SurvivorshipRulesetCollection clonedRules = new SurvivorshipRulesetCollection();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (SurvivorshipRuleset item in this._items)
                {
                    SurvivorshipRuleset clonedItem = item.Clone() as SurvivorshipRuleset;
                    clonedRules.Add(clonedItem);
                }
            }

            return clonedRules;
        }

        #endregion
    }
}