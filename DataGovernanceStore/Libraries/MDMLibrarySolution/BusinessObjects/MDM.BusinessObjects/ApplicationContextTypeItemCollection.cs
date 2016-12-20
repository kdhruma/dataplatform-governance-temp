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
    /// Specifies ApplicationContextTypeItem Collection
    /// </summary>
    [DataContract]
    public class ApplicationContextTypeItemCollection : InterfaceContractCollection<IApplicationContextTypeItem, ApplicationContextTypeItem>, IApplicationContextTypeItemCollection
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ApplicationContextTypeItem Collection
        /// </summary>
        public ApplicationContextTypeItemCollection()
        { }

        /// <summary>
        /// Initialize ApplicationContextTypeItem Collection from IList
        /// </summary>
        /// <param name="applicationContextTypeList">Source items</param>
        public ApplicationContextTypeItemCollection(IList<ApplicationContextTypeItem> applicationContextTypeList)
        {
            this._items = new Collection<ApplicationContextTypeItem>(applicationContextTypeList);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get Xml representation of ApplicationContextTypeItem Collection
        /// </summary>
        public String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //ApplicationContextTypes node start
            xmlWriter.WriteStartElement("ApplicationContextTypes");

            foreach (ApplicationContextTypeItem item in _items)
            {
                xmlWriter.WriteRaw(item.ToXml());
            }

            //ApplicationContextTypes node end
            xmlWriter.WriteEndElement();
            
            xmlWriter.Flush();

            //Get the actual XML
            String result = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return result;
        }

        /// <summary>
        /// Loads ApplicationContextTypeItem Collection from "ApplicationContextTypes" XML node
        /// </summary>
        public void LoadFromXml(XmlNode node)
        {
            _items.Clear();
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "ApplicationContextTypes")
                {
                    ApplicationContextTypeItem item = new ApplicationContextTypeItem();
                    item.LoadFromXml(child);
                    _items.Add(item);
                }
            }
        }

        /// <summary>
        /// Loads ApplicationContextTypeItem Collection from XML
        /// </summary>
        public void LoadFromXml(String xml)
        {
            _items.Clear();
            if (!String.IsNullOrWhiteSpace(xml))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                XmlNode node = doc.SelectSingleNode("ApplicationContextTypes");
                if (node != null)
                {
                    LoadFromXml(node);
                }
            }
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Clone ApplicationContextTypeItem Collection
        /// </summary>
        /// <returns>Cloned ApplicationContextTypeItem Collection object</returns>
        public object Clone()
        {
            ApplicationContextTypeItemCollection clonedConditions = new ApplicationContextTypeItemCollection();

            if (this._items != null && this._items.Any())
            {
                foreach (ApplicationContextTypeItem item in this._items)
                {
                    ApplicationContextTypeItem clonedItem = item.Clone() as ApplicationContextTypeItem;
                    clonedConditions.Add(clonedItem);
                }
            }

            return clonedConditions;
        }

        #endregion
    }
}