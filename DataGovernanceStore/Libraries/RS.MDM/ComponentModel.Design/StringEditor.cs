using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Xml.Xsl;
using System.IO;

namespace RS.MDM.ComponentModel.Design
{
    /// <summary>
    /// Provides functionality for editing a string
    /// </summary>
    public partial class StringEditor : Form
    {
        private String loadedText = String.Empty;

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public StringEditor()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a string that needs to be edited or viewed
        /// </summary>
        public string Data
        {
            get
            {
                return this.rtbData.Text;
            }
            set
            {
                loadedText = value;
                this.rtbData.Text = value;
            }
        }

        #endregion

        #region Event Handlers

        private void rtbData_TextChanged(object sender, EventArgs e)
        {
            Configuration.ConfigurationObject configObject = null;


            if (this.Tag != null && this.Tag is Configuration.ConfigurationObject)
            {
                configObject = this.Tag as Configuration.ConfigurationObject;
                configObject._isConfigDirty = true;
            }
        }

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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnView_Click(object sender, EventArgs e)
        {
            if (btnView.Text == "&Browser")
            {
                this.wbXML.Visible = true;
                this.rtbData.Visible = false;
                this.btnView.Text = "&Text";
                this.wbXML.DocumentText = this.GetHtml();
            }
            else
            {
                this.wbXML.Visible = false;
                this.rtbData.Visible = true;
                this.btnView.Text = "&Browser";
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetHtml()
        {
            try
            {
                String result = String.Empty;
                XslCompiledTransform _xslCompiledTransform = new XslCompiledTransform();
                StringWriter _stringWriter = new StringWriter();
                XmlReader _xslReader = XmlReader.Create(new System.IO.StringReader(RS.MDM.Properties.Resources.DefaultStyleSheet));
                XmlReader _xmlReader = XmlReader.Create(new System.IO.StringReader(this.Data));

                _xslCompiledTransform.Load(_xslReader);
                _xslCompiledTransform.Transform(_xmlReader, null, _stringWriter);
                result = _stringWriter.ToString();

                _xmlReader.Close();
                _stringWriter.Close();
                _xslReader.Close();

                return result;
            }
            catch
            {
            }

            return this.Data;
        }

        #endregion
    }
}
