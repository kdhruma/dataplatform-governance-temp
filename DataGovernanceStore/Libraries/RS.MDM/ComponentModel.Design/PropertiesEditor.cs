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
    /// Provides UI functionality to edit an object in propertygrid control
    /// </summary>
    public partial class PropertiesEditor : Form
    {
        #region Fields

        /// <summary>
        /// field for an object that needs to be edited in propertygrid control
        /// </summary>
        private object _parameter = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public PropertiesEditor()
        {
            InitializeComponent();
            this.pgProperties.CommandsVisibleIfAvailable = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets an object that needs to be edited in propertygrid control
        /// </summary>
        public object Parameter
        {
            get
            {
                return this._parameter;
            }
            set
            {
                this._parameter = value;
                this.pgProperties.SelectedObject = value;
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


        private void pgProperties_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
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
