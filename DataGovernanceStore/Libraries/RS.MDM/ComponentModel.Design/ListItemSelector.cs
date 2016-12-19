using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RS.MDM.ComponentModel.Design
{
    /// <summary>
    /// Provides UI functionality to select an item from a list
    /// </summary>
    public partial class ListItemSelector : Form
    {
        #region Fields

        /// <summary>
        /// field for the list
        /// </summary>
        private List<string> _listItems = new List<string>();

        /// <summary>
        /// field for the seleted item
        /// </summary>
        private string _selectedItem = string.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public ListItemSelector()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the list from which an item needs to be selected
        /// </summary>
        public List<string> ListItems
        {
            get
            {
                return this._listItems;
            }
            set
            {
                this._listItems = value;
            }
        }

        /// <summary>
        /// Gets or sets the selected item
        /// </summary>
        public string SelectedItem
        {
            get
            {
                return this.listItems.SelectedItem.ToString();
            }
            set
            {
                this._selectedItem = value;
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListItemSelector_Load(object sender, EventArgs e)
        {
            if (this._listItems != null)
            {
                this.listItems.Items.AddRange(this._listItems.ToArray());
                if (!string.IsNullOrEmpty(this._selectedItem))
                {
                if (listItems.Items.Count > 0)
                    if (listItems.Items.Contains(this._selectedItem))
                    {
                        listItems.SelectedItem = this._selectedItem;
                    }
                    else
                    {
                        listItems.SelectedIndex = 0;
                    }
                }
                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (this.listItems.SelectedItems.Count > 0)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please select an item and try again", "Select", MessageBoxButtons.OK);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void listItems_DoubleClick(object sender, System.EventArgs e)
        {
            if (this.listItems.SelectedItems.Count > 0)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        #endregion

    }
}
