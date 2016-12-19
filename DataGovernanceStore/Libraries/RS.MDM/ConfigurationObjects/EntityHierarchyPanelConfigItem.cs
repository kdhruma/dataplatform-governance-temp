using System.Xml.Serialization;
using System;
using RS.MDM.Configuration;
using System.ComponentModel;

namespace RS.MDM.ConfigurationObjects
{
    /// <summary>
    /// Provides configuration for EntityHierarchyPanel
    /// </summary>
    [XmlRoot("EntityHierarchyPanelConfigItem")]
    [Serializable()]
    public class EntityHierarchyPanelConfigItem : Object
    {
        #region Fields

        /// <summary>
        /// field for the title of the EntityHierarchyPanelConfigItem
        /// </summary>
        private string _title = String.Empty;

        /// <summary>
        /// field to denote if the EntityHierarchyPanelConfigItem is visible
        /// </summary>
        private bool _visible = true;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates the title of the EntityHierarchyPanelConfigItem
        /// </summary>
        [XmlAttribute("Title")]
        [Description("EntityHierarchyPanelConfigItem title.")]
        [Category("EntityHierarchyPanelConfigItem")]
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
        /// Indicates if the EntityHierarchyPanelConfigItem is visible
        /// </summary>
        [XmlAttribute("Visible")]
        [Description("EntityHierarchyPanelConfigItem is visible or not")]
        [Category("EntityHierarchyPanelConfigItem")]
        [TrackChanges()]
        public Boolean Visible
        {
            get
            {
                return _visible;
            }
            set
            {
                _visible = value;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public EntityHierarchyPanelConfigItem()
            : base()
        {

        }

        #endregion
    }
}
