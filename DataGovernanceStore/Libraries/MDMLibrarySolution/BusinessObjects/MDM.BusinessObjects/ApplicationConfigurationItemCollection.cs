using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies ApplicationConfigurationItem Collection
    /// </summary>
    [DataContract]
    public class ApplicationConfigurationItemCollection : InterfaceContractCollection<IApplicationConfigurationItem, ApplicationConfigurationItem>, IApplicationConfigurationItemCollection
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ApplicationConfigurationItem Collection
        /// </summary>
        public ApplicationConfigurationItemCollection()
        { }

        /// <summary>
        /// Initialize ApplicationConfigurationItem Collection from IList
        /// </summary>
        /// <param name="applicationConfigurationItemList">Source items</param>
        public ApplicationConfigurationItemCollection(IList<ApplicationConfigurationItem> applicationConfigurationItemList)
        {
            this._items = new Collection<ApplicationConfigurationItem>(applicationConfigurationItemList);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get Xml representation of ApplicationConfigurationItem Collection
        /// </summary>
        public String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //ApplicationConfigurationItems node start
            xmlWriter.WriteStartElement("ApplicationConfigurationItems");

            foreach (ApplicationConfigurationItem item in _items)
            {
                xmlWriter.WriteRaw(item.ToXml());
            }

            //ApplicationConfigurationItems node end
            xmlWriter.WriteEndElement();
            
            xmlWriter.Flush();

            //Get the actual XML
            String result = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return result;
        }

        /// <summary>
        /// Loads ApplicationConfigurationItem Collection from "ApplicationConfigurationItems" XML node
        /// </summary>
        public void LoadFromXml(XmlNode node)
        {
            _items.Clear();
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "ApplicationConfigurationItems")
                {
                    ApplicationConfigurationItem item = new ApplicationConfigurationItem();
                    item.LoadFromXml(child);
                    _items.Add(item);
                }
            }
        }

        /// <summary>
        /// Loads ApplicationConfigurationItem Collection from XML
        /// </summary>
        public void LoadFromXml(String xml)
        {
            _items.Clear();
            if (!String.IsNullOrWhiteSpace(xml))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                XmlNode node = doc.SelectSingleNode("ApplicationConfigurationItems");
                if (node != null)
                {
                    LoadFromXml(node);
                }
            }
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Clone ApplicationConfigurationItem Collection
        /// </summary>
        /// <returns>Cloned ApplicationConfigurationItem Collection object</returns>
        public object Clone()
        {
            ApplicationConfigurationItemCollection clonedConditions = new ApplicationConfigurationItemCollection();

            if (this._items != null && this._items.Any())
            {
                foreach (ApplicationConfigurationItem item in this._items)
                {
                    ApplicationConfigurationItem clonedItem = item.Clone() as ApplicationConfigurationItem;
                    clonedConditions.Add(clonedItem);
                }
            }

            return clonedConditions;
        }

        #endregion
    }
}