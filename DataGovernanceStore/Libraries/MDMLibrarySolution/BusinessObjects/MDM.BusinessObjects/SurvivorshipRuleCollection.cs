using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.BusinessObjects.Interfaces;

    /// <summary>
    /// Specifies SurvivorshipRuleCollection
    /// </summary>
    [DataContract]
    public class SurvivorshipRuleCollection : InterfaceContractCollection<ISurvivorshipRule, SurvivorshipRule>, ISurvivorshipRuleCollection
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the SurvivorshipRule Collection
        /// </summary>
        public SurvivorshipRuleCollection()
        { }

        /// <summary>
        /// Initialize SurvivorshipRule collection from IList
        /// </summary>
        /// <param name="survivorshipRuleList">Source items</param>
        public SurvivorshipRuleCollection(IList<SurvivorshipRule> survivorshipRuleList)
        {
            this._items = new Collection<SurvivorshipRule>(survivorshipRuleList);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get Xml representation of SurvivorshipRule Collection
        /// </summary>
        public String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            foreach (SurvivorshipRule item in _items)
            {
                xmlWriter.WriteRaw(item.ToXml());
            }

            xmlWriter.Flush();

            //Get the actual XML
            String result = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return result;
        }

        /// <summary>
        /// Loads SurvivorshipRule Collection from "SurvivorshipRules" XML node
        /// </summary>
        public void LoadFromXml(XmlNode node)
        {
            _items.Clear();
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "SurvivorshipRule")
                {
                    SurvivorshipRule item = new SurvivorshipRule();
                    item.LoadFromXml(child);
                    _items.Add(item);
                }
            }
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Clone SurvivorshipRule collection
        /// </summary>
        /// <returns>Cloned SurvivorshipRule collection object</returns>
        public object Clone()
        {
            SurvivorshipRuleCollection clonedRules = new SurvivorshipRuleCollection();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (SurvivorshipRule item in this._items)
                {
                    SurvivorshipRule clonedItem = item.Clone() as SurvivorshipRule;
                    clonedRules.Add(clonedItem);
                }
            }

            return clonedRules;
        }

        #endregion
    }
}
