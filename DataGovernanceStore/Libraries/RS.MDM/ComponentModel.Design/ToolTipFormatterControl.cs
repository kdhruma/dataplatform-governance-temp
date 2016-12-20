using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RS.MDM.ComponentModel.Design
{
    /// <summary>
    /// Represents the type of actions which can be performed on ToolTipFormat string
    /// </summary>
    public enum Action
    {
        Add,
        Remove,
        None
    }

    /// <summary>
    /// Provides custom UI for Formatting ToolTip for the PanelDataItems 
    /// </summary>
    public partial class ToolTipFormatter : Form
    {
        #region Fields
        
        private string _toolTipFormatText = string.Empty;
        private string _lastActionString = string.Empty;
        private Action _lastAction = Action.None;
        private CheckBox _lastUpdatedCB = null;
        private string _configToolTipFormatText = string.Empty;

        #endregion

        #region Properties

        public string ToolTipFormatText
        {
            get
            {
                return _toolTipFormatText;
            }
            set
            {
                _toolTipFormatText = value;
                tbToolTipFormatText.Text = value;
            }
        }

        #endregion

        #region Constructors

        public ToolTipFormatter()
        {
            InitializeComponent();
        }

        public ToolTipFormatter(string configToolTipFormatText)
        {
            InitializeComponent();

            this._configToolTipFormatText = configToolTipFormatText;

            //Load retrieved Tooltip format into UI
            LoadToolTipFormatText();

            btUndo.Enabled = false;
        }

        #endregion

        #region Methods

        #endregion

        /// <summary>
        /// Loads the retrieved tooltip format from the Config into UI
        /// </summary>
        private void LoadToolTipFormatText()
        {
            if (!string.IsNullOrEmpty(this._configToolTipFormatText))
            {
                this.ToolTipFormatText = this._configToolTipFormatText;

                if (this.ToolTipFormatText.Contains("[Organization]"))
                    cbOrganization.Checked = true;
                else
                    cbOrganization.Checked = false;

                if (this.ToolTipFormatText.Contains("[Catalog-ShortName]"))
                    cbCatalogSN.Checked = true;
                else
                    cbCatalogSN.Checked = false;

                if (this.ToolTipFormatText.Contains("[Catalog-LongName]"))
                    cbCatalogLN.Checked = true;
                else
                    cbCatalogLN.Checked = false;

                if (this.ToolTipFormatText.Contains("[Category]"))
                    cbCategory.Checked = true;
                else
                    cbCategory.Checked = false;

                if (this.ToolTipFormatText.Contains("[NodeType]"))
                    cbNodeType.Checked = true;
                else
                    cbNodeType.Checked = false;

                if (this.ToolTipFormatText.Contains("[RelationshipType]"))
                    cbRelationshipType.Checked = true;
                else
                    cbRelationshipType.Checked = false;

                if (this.ToolTipFormatText.Contains("[Locale]"))
                    cbLocale.Checked = true;
                else
                    cbLocale.Checked = false;
            }
        }

        #region ControlEvents

        private void btOK_Click(object sender, EventArgs e)
        {
            errorToolTip.RemoveAll();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btUndo_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this._lastActionString))
            {
                //If previously performed action is the addition, then remove the last appended string from the ToolTipFormatText
                if (this._lastAction == Action.Add)
                {
                    if (this.ToolTipFormatText.Contains(this._lastActionString))
                    {
                        this.ToolTipFormatText = this.ToolTipFormatText.Replace(this._lastActionString, "");
                        this._lastAction = Action.Remove;

                        if (this._lastUpdatedCB != null)
                            _lastUpdatedCB.Checked = false;
                    }
                }
                //Otherwise if previously performed action is the Removal, then add the last removed string from the ToolTipFormatText
                else if (this._lastAction == Action.Remove)
                {
                    this.ToolTipFormatText = this.ToolTipFormatText + this._lastActionString;
                    this._lastAction = Action.Add;

                    if (this._lastUpdatedCB != null)
                        _lastUpdatedCB.Checked = true;
                }
            }
        }

        private void btClearAll_Click(object sender, EventArgs e)
        {
            this.ToolTipFormatText = string.Empty;

            this._lastActionString = string.Empty;
            btUndo.Enabled = false;

            cbOrganization.Checked = false;
            cbCatalogSN.Checked = false;
            cbCatalogLN.Checked = false;
            cbCategory.Checked = false;
            cbNodeType.Checked = false;
            cbRelationshipType.Checked = false;
            cbLocale.Checked = false;

            tbAttributeId.Text = string.Empty;
            tbText.Text = string.Empty;
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            errorToolTip.RemoveAll();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void cbContext_Click(object sender, EventArgs e)
        {
            //Based on the action performed on the context check boxes, edit the TooTipFormatText
            if (sender != null && sender is CheckBox)
            {
                CheckBox contextCheckBox = sender as CheckBox;

                string contextString = string.Format("[{0}]", contextCheckBox.Text);

                if (contextCheckBox.Checked == true)
                {
                    //Context has been selected. Append the context string to ToolTipFormatText
                    this.ToolTipFormatText = this.ToolTipFormatText + contextString;

                    //Save the appended context string and the action performed
                    this._lastActionString = contextString;
                    this._lastAction = Action.Add;
                    this._lastUpdatedCB = contextCheckBox;
                    btUndo.Enabled = true;
                }
                else
                {
                    //Context has been deselected. Remove the context string from ToolTipFormatText
                    if (this.ToolTipFormatText.Contains(contextString))
                    {
                        this.ToolTipFormatText = this.ToolTipFormatText.Replace(contextString, "");

                        //Save the removed context string and the action performed
                        this._lastActionString = contextString;
                        this._lastAction = Action.Remove;
                        this._lastUpdatedCB = contextCheckBox;
                        btUndo.Enabled = true;
                    }
                }
            }
        }

        private void btAttrIdAppend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbAttributeId.Text))
            {
                errorToolTip.ToolTipTitle = "Attribute Id";
                errorToolTip.Show("Enter Attribute Id.", tbAttributeId, 5, -70);
                return;
            }

            //Append the Attribute Id to the ToolTipFormatText
            string attributeIdString = string.Format("[Attribute(Id={0})]", tbAttributeId.Text);
            this.ToolTipFormatText = this.ToolTipFormatText + attributeIdString;

            //Save the appended text and the action performed
            this._lastActionString = attributeIdString;
            this._lastAction = Action.Add;
            this._lastUpdatedCB = null;
            btUndo.Enabled = true;

            tbAttributeId.Text = string.Empty;
        }

        private void btTextAppend_Click(object sender, EventArgs e)
        {
            string textString = tbText.Text;

            if (string.IsNullOrEmpty(textString))
            {
                errorToolTip.ToolTipTitle = "Text";
                errorToolTip.Show("Enter Text.", tbText, 5, -70);
                return;
            }

            //Append the text to the ToolTipFormatText
            this.ToolTipFormatText = this.ToolTipFormatText + textString;

            //Save the appended text and the action performed
            this._lastActionString = textString;
            this._lastAction = Action.Add;
            this._lastUpdatedCB = null;
            btUndo.Enabled = true;

            tbText.Text = string.Empty;
        }

        private void tbAttributeId_TextChanged(object sender, EventArgs e)
        {
            errorToolTip.RemoveAll();

            try
            {
                if (!string.IsNullOrEmpty(tbAttributeId.Text))
                {
                    Int64 attrId = Convert.ToInt64(tbAttributeId.Text);
                }
            }
            catch
            {
                errorToolTip.ToolTipTitle = "Attribute Id";
                errorToolTip.Show("Id of the Attribute should be numeric.", tbAttributeId, 5, -70);
            }
        }

        private void tbText_TextChanged(object sender, EventArgs e)
        {
            errorToolTip.RemoveAll();
        }

        #endregion
    }
}
