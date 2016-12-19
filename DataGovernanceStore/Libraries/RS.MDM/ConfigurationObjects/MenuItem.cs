using System;
using System.IO;
using System.Xml;
using System.Data;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace RS.MDM.ConfigurationObjects
{
    using RS.MDM.Validations;
    using RS.MDM.Configuration;

    /// <summary>
    /// Represents an Item of Menu
    /// </summary>
    [XmlRoot("MenuItem")]
    [Serializable()]
    public sealed class MenuItem : Object
    {
        #region Fields

        /// <summary>
        /// Represents the title of the Menu Item
        /// </summary>
        private String _title = String.Empty;

        /// <summary>
        /// Represents the link of the Menu Item
        /// </summary>
        private String _link = String.Empty;

        /// <summary>
        /// Represents on Allow Multiple Tab of menu Item
        /// </summary>
        private String _allowMultipleTab = String.Empty;

        /// <summary>
        /// Represents the Icon of menu item
        /// </summary>
        private String _icon = String.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// Represents the title of the menu item
        /// </summary>
        [XmlAttribute("Title")]
        [Category("Properties")]
        [Description("Represents the title of the menu item")]
        [TrackChanges()]
        public String Title
        {
            get
            {
                return this._title;
            }
            set
            {
                this._title = value;
            }
        }

        /// <summary>
        /// Represents the link of the menu item
        /// </summary>
        [XmlAttribute("Link")]
        [Category("Properties")]
        [Description("Represents the link of the menu item")]
        [TrackChanges()]
        public String Link
        {
            get
            {
                return this._link;
            }
            set
            {
                this._link = value;
            }
        }

        /// <summary>
        /// Represents Allow Multiple Tab of menu item
        /// </summary>
        [XmlAttribute("AllowMultipleTab")]
        [Category("Properties")]
        [Description("Represents Allow Multiple Tab of menu item")]
        [TrackChanges()]
        public String AllowMultipleTab
        {
            get
            {
                return this._allowMultipleTab;
            }
            set
            {
                this._allowMultipleTab = value;
            }
        }

        /// <summary>
        /// Represents the Icon of menu item
        /// </summary>
        [XmlAttribute("Icon")]
        [Category("Properties")]
        [Description("Represents the Icon of menu item")]
        [TrackChanges()]
        public String Icon
        {
            get
            {
                return this._icon;
            }
            set
            {
                this._icon = value;
            }
        }

        #endregion

        #region Constructors

        #endregion

        #region Serialization & Deserialization

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        [System.Runtime.Serialization.OnDeserialized()]
        private void OnDeserialized(System.Runtime.Serialization.StreamingContext context)
        {
            this.SetParent();
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Get XML representation of the object
        /// </summary>
        /// <returns>XML representation of Menu Item</returns>
        public override String ToXml()
        {
            String menuItemXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            #region Menu Item Node

            //MenuItem node start
            xmlWriter.WriteStartElement("MenuItem");

            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("Title", this.Title);
            xmlWriter.WriteAttributeString("Link", this.Link);
            xmlWriter.WriteAttributeString("AllowMultipleTab", this.AllowMultipleTab);
            xmlWriter.WriteAttributeString("Icon", this.Icon);
            
            //Value node end
            xmlWriter.WriteEndElement();

            #endregion Menu Item Node

            xmlWriter.Flush();

            //Get the actual XML
            menuItemXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return menuItemXml;
        }

        #endregion
    }
}
