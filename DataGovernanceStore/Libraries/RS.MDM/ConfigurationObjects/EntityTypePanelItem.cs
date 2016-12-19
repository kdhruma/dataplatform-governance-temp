using MDM.Core;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace RS.MDM.ConfigurationObjects
{
    using RS.MDM.Configuration;

    /// <summary>
    /// Represents a entity type
    /// </summary>
    [XmlRoot("EntityTypePanelItem")]
    [Serializable()]
    public class EntityTypePanelItem : Object
    {
        #region Fields

        /// <summary>
        /// Represents the list of entity type short names
        /// </summary>
        private String _entityTypeShortNames = String.Empty;

        /// <summary>
        /// Represents the title of entity type panel item
        /// </summary>
        private String _title = String.Empty;

        /// <summary>
        /// Represents the tooltip of entity type panel item
        /// </summary>
        private String _tooltip = String.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// Represents the EntityTypeShortNames of the EntityTypePanelItem
        /// </summary>
        [XmlAttribute("EntityTypeShortNames")]
        [Category("Properties")]
        [Description("Represents the EntityTypeShortNames of the EntityTypePanelItem")]
        [TrackChanges()]
        public String EntityTypeShortNames
        {
            get
            {
                return this._entityTypeShortNames;
            }
            set
            {
                this._entityTypeShortNames = value;
            }
        }

        /// <summary>
        /// Represents the Title of the EntityTypePanelItem
        /// </summary>
        [XmlAttribute("Title")]
        [Category("Properties")]
        [Description("Represents the Title of the EntityTypePanelItem")]
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
        /// Represents the ToolTip of the EntityTypePanelItem
        /// </summary>
        [XmlAttribute("ToolTip")]
        [Category("Properties")]
        [Description("Represents the ToolTip of the EntityTypePanelItem")]
        [TrackChanges()]
        public String ToolTip
        {
            get
            {
                return this._tooltip;
            }
            set
            {
                this._tooltip = value;
            }
        }

        #endregion

        #region Constructors

        #endregion

        #region Overrides

        /// <summary>
        /// Get XML representation of the object
        /// </summary>
        /// <returns>XML representation of EntityTypePanel Item</returns>
        public override String ToXml()
        {
            String entityTypePanelItemXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            #region EntityTypePanelItem Node

            //MenuItem node start
            xmlWriter.WriteStartElement("EntityTypePanelItem");

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("UniqueIdentifier", this.UniqueIdentifier);
            xmlWriter.WriteAttributeString("Name", this.GetLocaleMessage(this.Name));
            xmlWriter.WriteAttributeString("Description", this.Description);
            xmlWriter.WriteAttributeString("EntityTypeShortNames", this.EntityTypeShortNames);
            xmlWriter.WriteAttributeString("Title", this.Title);
            xmlWriter.WriteAttributeString("ToolTip", this.ToolTip);

            //Value node end
            xmlWriter.WriteEndElement();

            #endregion EntityTypePanelItem Node

            xmlWriter.Flush();

            //Get the actual XML
            entityTypePanelItemXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();
            
            return entityTypePanelItemXml;
        }

        #endregion
    }
}
