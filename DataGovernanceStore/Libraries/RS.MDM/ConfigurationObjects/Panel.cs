using System;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Configuration;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using RS.MDM.Configuration; 

namespace RS.MDM.ConfigurationObjects
{
    /// <summary>
    /// Provides configuration for PanelItem
    /// </summary>
    [XmlRoot("Panel")]
    [Serializable()]
    public class Panel : RS.MDM.Object
    {
        #region Fields

        /// <summary>
        /// field for the title of the Panel
        /// </summary>
        private string _title = String.Empty;

        /// <summary>
        /// field which indicates Unit Of Measure
        /// </summary>
        private UnitType _unitOfMeasure = UnitType.Percentage;

        /// <summary>
        /// field for the height of the Panel 
        /// </summary>
        private string _minHeight = "100";

        /// <summary>
        /// field to denote if the panel is collapsed
        /// </summary>
        private bool _collapsed = false;

        /// <summary>
        /// field to denote if the panel is visible
        /// </summary>
        private bool _visible = true;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates the title of the panel
        /// </summary>
        [XmlAttribute("Title")]
        [Description("Indicates the title of the panel.")]
        [Category("Panel")]
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
        /// Indicates Unit Of Measure(Pixel or Percentage) to be used for the Panels.
        /// </summary>
        [XmlAttribute("UnitOfMeasure")]
        [Description("Indicates Unit Of Measure(Pixel or Percentage) to calculate Panel width.")]
        [Category("Panel")]
        [TrackChanges()]
        public UnitType UnitOfMeasure
        {
            get
            {
                return this._unitOfMeasure;
            }
        }

        /// <summary>
        /// Indicates the height of the panel item
        /// </summary>
        [XmlAttribute("MinHeight")]
        [Description("Indicates the minimum height of the panel.")]
        [Category("Panel")]
        [TrackChanges()]
        public String MinHeight
        {
            get
            {
                return _minHeight;
            }
            set
            {
                int minHeightValue = 0;
                Int32.TryParse(value, out minHeightValue);

                if (minHeightValue > 0)
                    _minHeight = value;
                else
                    _minHeight = "1";
            }
        }

        /// <summary>
        /// Indicates if the panel needs to be expanded
        /// </summary>
        [XmlAttribute("Collapsed")]
        [Description("Indicates if the PanelBar needs to be expanded on load.")]
        [Category("Panel")]
        [TrackChanges()]
        public bool Collapsed
        {
            get
            {
                return this._collapsed;
            }
            set
            {
                this._collapsed = value;
            }
        }

        /// <summary>
        /// Indicates if the panel is visible
        /// </summary>
        [XmlAttribute("Visible")]
        [Description("Indicates if the Panel is visible.")]
        [Category("Panel")]
        [TrackChanges()]
        public bool Visible
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

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public Panel()
            : base()
        {

        }

        #endregion

        #region Overrides

        /// <summary>
        /// Get a tree node that reprents an object and its aggregated children
        /// </summary>
        /// <returns>A Tree Node that represents an object and its aggregated children</returns>
        public override System.Windows.Forms.TreeNode GetTreeNode()
        {
            System.Windows.Forms.TreeNode _treeNode = base.GetTreeNode();

            if (this.PropertyChanges.ObjectStatus == RS.MDM.Configuration.InheritedObjectStatus.None)
            {
                _treeNode.ImageKey = "NavigationPane";
                _treeNode.SelectedImageKey = _treeNode.ImageKey;
            }

            return _treeNode;
        }

        #endregion
    }
}
