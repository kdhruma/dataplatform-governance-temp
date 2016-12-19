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
    /// Provides UI functionality for editing a collection in DataGridView
    /// </summary>
    public partial class ListEditor : Form
    {
        #region Fields

        /// <summary>
        /// field for the list
        /// </summary>
        private object _list = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public ListEditor()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or Sets the list that needs to be edited in a DataGridView control
        /// </summary>
        public object List
        {
            get
            {
                return this._list;
            }
            set
            {
                this._list = value;
                this.dgvList.DataSource = new BindingSource(value, "");
                this.dgvList.Refresh();
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
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
        /// Includes the properties of basic types for editing. All other parameters are hidden.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            List<string> _types = new List<string>() { "String", "DateTime", "Boolean", "Int16", "Int32", "Int64", "Decimal", "Double", "Single", "Byte", "UInt16", "UInt32", "UInt64", "ColumnDisplayType" };
            foreach (DataGridViewColumn _dataGridViewColumn in this.dgvList.Columns)
            {
                if (!_types.Contains(_dataGridViewColumn.ValueType.Name))
                {
                    _dataGridViewColumn.Visible = false;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //System.Diagnostics.Trace.WriteLine(e.Exception.ToString(), RS.MDM.Logging.LogLevel.ERROR.ToString());
        }

        private void dgvList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Configuration.ConfigurationObject configObject = null;

            if (this.Tag != null && this.Tag is Configuration.ConfigurationObject)
            {
                configObject = this.Tag as Configuration.ConfigurationObject;
                configObject._isConfigDirty = true;
            }
        }

        #endregion
    }
}
