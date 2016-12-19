namespace RS.MDM.ComponentModel.Design
{
    partial class ToolTipFormatter
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btCancel = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.btUndo = new System.Windows.Forms.Button();
            this.btClearAll = new System.Windows.Forms.Button();
            this.gbSeparator = new System.Windows.Forms.GroupBox();
            this.lblAction = new System.Windows.Forms.Label();
            this.tbToolTipFormatText = new System.Windows.Forms.TextBox();
            this.lblToolTipFormatStringHeader = new System.Windows.Forms.Label();
            this.gbContextParameters = new System.Windows.Forms.GroupBox();
            this.cbCatalogLN = new System.Windows.Forms.CheckBox();
            this.cbLocale = new System.Windows.Forms.CheckBox();
            this.cbRelationshipType = new System.Windows.Forms.CheckBox();
            this.cbNodeType = new System.Windows.Forms.CheckBox();
            this.cbCategory = new System.Windows.Forms.CheckBox();
            this.cbCatalogSN = new System.Windows.Forms.CheckBox();
            this.cbOrganization = new System.Windows.Forms.CheckBox();
            this.gbAttributes = new System.Windows.Forms.GroupBox();
            this.btAttrIdAppend = new System.Windows.Forms.Button();
            this.tbAttributeId = new System.Windows.Forms.TextBox();
            this.lblAttributeId = new System.Windows.Forms.Label();
            this.gbText = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btTextAppend = new System.Windows.Forms.Button();
            this.tbText = new System.Windows.Forms.TextBox();
            this.lblText = new System.Windows.Forms.Label();
            this.errorToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.gbContextParameters.SuspendLayout();
            this.gbAttributes.SuspendLayout();
            this.gbText.SuspendLayout();
            this.SuspendLayout();
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(318, 380);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 6;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btOK
            // 
            this.btOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOK.Location = new System.Drawing.Point(75, 380);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(75, 23);
            this.btOK.TabIndex = 3;
            this.btOK.Text = "OK";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // btUndo
            // 
            this.btUndo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btUndo.Location = new System.Drawing.Point(156, 380);
            this.btUndo.Name = "btUndo";
            this.btUndo.Size = new System.Drawing.Size(75, 23);
            this.btUndo.TabIndex = 4;
            this.btUndo.Text = "Undo";
            this.btUndo.UseVisualStyleBackColor = true;
            this.btUndo.Click += new System.EventHandler(this.btUndo_Click);
            // 
            // btClearAll
            // 
            this.btClearAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btClearAll.Location = new System.Drawing.Point(237, 380);
            this.btClearAll.Name = "btClearAll";
            this.btClearAll.Size = new System.Drawing.Size(75, 23);
            this.btClearAll.TabIndex = 5;
            this.btClearAll.Text = "Clear All";
            this.btClearAll.UseVisualStyleBackColor = true;
            this.btClearAll.Click += new System.EventHandler(this.btClearAll_Click);
            // 
            // gbSeparator
            // 
            this.gbSeparator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbSeparator.Location = new System.Drawing.Point(3, 364);
            this.gbSeparator.Name = "gbSeparator";
            this.gbSeparator.Size = new System.Drawing.Size(399, 10);
            this.gbSeparator.TabIndex = 4;
            this.gbSeparator.TabStop = false;
            // 
            // lblAction
            // 
            this.lblAction.AutoSize = true;
            this.lblAction.Location = new System.Drawing.Point(13, 12);
            this.lblAction.Name = "lblAction";
            this.lblAction.Size = new System.Drawing.Size(348, 13);
            this.lblAction.TabIndex = 5;
            this.lblAction.Text = "Select the parameters which are required to be part of the ToolTip string.";
            // 
            // tbToolTipFormatText
            // 
            this.tbToolTipFormatText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbToolTipFormatText.Location = new System.Drawing.Point(12, 284);
            this.tbToolTipFormatText.Multiline = true;
            this.tbToolTipFormatText.Name = "tbToolTipFormatText";
            this.tbToolTipFormatText.ReadOnly = true;
            this.tbToolTipFormatText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbToolTipFormatText.Size = new System.Drawing.Size(381, 74);
            this.tbToolTipFormatText.TabIndex = 6;
            this.tbToolTipFormatText.TabStop = false;
            // 
            // lblToolTipFormatStringHeader
            // 
            this.lblToolTipFormatStringHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblToolTipFormatStringHeader.AutoSize = true;
            this.lblToolTipFormatStringHeader.Location = new System.Drawing.Point(13, 263);
            this.lblToolTipFormatStringHeader.Name = "lblToolTipFormatStringHeader";
            this.lblToolTipFormatStringHeader.Size = new System.Drawing.Size(114, 13);
            this.lblToolTipFormatStringHeader.TabIndex = 7;
            this.lblToolTipFormatStringHeader.Text = "ToolTip Format String :";
            // 
            // gbContextParameters
            // 
            this.gbContextParameters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbContextParameters.Controls.Add(this.cbCatalogLN);
            this.gbContextParameters.Controls.Add(this.cbLocale);
            this.gbContextParameters.Controls.Add(this.cbRelationshipType);
            this.gbContextParameters.Controls.Add(this.cbNodeType);
            this.gbContextParameters.Controls.Add(this.cbCategory);
            this.gbContextParameters.Controls.Add(this.cbCatalogSN);
            this.gbContextParameters.Controls.Add(this.cbOrganization);
            this.gbContextParameters.Location = new System.Drawing.Point(12, 41);
            this.gbContextParameters.Name = "gbContextParameters";
            this.gbContextParameters.Size = new System.Drawing.Size(184, 207);
            this.gbContextParameters.TabIndex = 0;
            this.gbContextParameters.TabStop = false;
            this.gbContextParameters.Text = "Context Parameters";
            // 
            // cbCatalogLN
            // 
            this.cbCatalogLN.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.cbCatalogLN.AutoSize = true;
            this.cbCatalogLN.Location = new System.Drawing.Point(6, 66);
            this.cbCatalogLN.Name = "cbCatalogLN";
            this.cbCatalogLN.Size = new System.Drawing.Size(117, 17);
            this.cbCatalogLN.TabIndex = 2;
            this.cbCatalogLN.Text = "Catalog-LongName";
            this.cbCatalogLN.UseVisualStyleBackColor = true;
            this.cbCatalogLN.Click += new System.EventHandler(this.cbContext_Click);
            // 
            // cbLocale
            // 
            this.cbLocale.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.cbLocale.AutoSize = true;
            this.cbLocale.Enabled = false;
            this.cbLocale.Location = new System.Drawing.Point(6, 158);
            this.cbLocale.Name = "cbLocale";
            this.cbLocale.Size = new System.Drawing.Size(58, 17);
            this.cbLocale.TabIndex = 6;
            this.cbLocale.Text = "Locale";
            this.cbLocale.UseVisualStyleBackColor = true;
            this.cbLocale.Click += new System.EventHandler(this.cbContext_Click);
            // 
            // cbRelationshipType
            // 
            this.cbRelationshipType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.cbRelationshipType.AutoSize = true;
            this.cbRelationshipType.Enabled = false;
            this.cbRelationshipType.Location = new System.Drawing.Point(6, 135);
            this.cbRelationshipType.Name = "cbRelationshipType";
            this.cbRelationshipType.Size = new System.Drawing.Size(108, 17);
            this.cbRelationshipType.TabIndex = 5;
            this.cbRelationshipType.Text = "RelationshipType";
            this.cbRelationshipType.UseVisualStyleBackColor = true;
            this.cbRelationshipType.Click += new System.EventHandler(this.cbContext_Click);
            // 
            // cbNodeType
            // 
            this.cbNodeType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.cbNodeType.AutoSize = true;
            this.cbNodeType.Location = new System.Drawing.Point(6, 112);
            this.cbNodeType.Name = "cbNodeType";
            this.cbNodeType.Size = new System.Drawing.Size(76, 17);
            this.cbNodeType.TabIndex = 4;
            this.cbNodeType.Text = "NodeType";
            this.cbNodeType.UseVisualStyleBackColor = true;
            this.cbNodeType.Click += new System.EventHandler(this.cbContext_Click);
            // 
            // cbCategory
            // 
            this.cbCategory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.cbCategory.AutoSize = true;
            this.cbCategory.Location = new System.Drawing.Point(6, 89);
            this.cbCategory.Name = "cbCategory";
            this.cbCategory.Size = new System.Drawing.Size(68, 17);
            this.cbCategory.TabIndex = 3;
            this.cbCategory.Text = "Category";
            this.cbCategory.UseVisualStyleBackColor = true;
            this.cbCategory.Click += new System.EventHandler(this.cbContext_Click);
            // 
            // cbCatalogSN
            // 
            this.cbCatalogSN.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.cbCatalogSN.AutoSize = true;
            this.cbCatalogSN.Location = new System.Drawing.Point(7, 43);
            this.cbCatalogSN.Name = "cbCatalogSN";
            this.cbCatalogSN.Size = new System.Drawing.Size(118, 17);
            this.cbCatalogSN.TabIndex = 1;
            this.cbCatalogSN.Text = "Catalog-ShortName";
            this.cbCatalogSN.UseVisualStyleBackColor = true;
            this.cbCatalogSN.Click += new System.EventHandler(this.cbContext_Click);
            // 
            // cbOrganization
            // 
            this.cbOrganization.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.cbOrganization.AutoSize = true;
            this.cbOrganization.Location = new System.Drawing.Point(7, 20);
            this.cbOrganization.Name = "cbOrganization";
            this.cbOrganization.Size = new System.Drawing.Size(85, 17);
            this.cbOrganization.TabIndex = 0;
            this.cbOrganization.Text = "Organization";
            this.cbOrganization.UseVisualStyleBackColor = true;
            this.cbOrganization.Click += new System.EventHandler(this.cbContext_Click);
            // 
            // gbAttributes
            // 
            this.gbAttributes.Controls.Add(this.btAttrIdAppend);
            this.gbAttributes.Controls.Add(this.tbAttributeId);
            this.gbAttributes.Controls.Add(this.lblAttributeId);
            this.gbAttributes.Location = new System.Drawing.Point(211, 41);
            this.gbAttributes.Name = "gbAttributes";
            this.gbAttributes.Size = new System.Drawing.Size(184, 94);
            this.gbAttributes.TabIndex = 1;
            this.gbAttributes.TabStop = false;
            this.gbAttributes.Text = "Attributes";
            // 
            // btAttrIdAppend
            // 
            this.btAttrIdAppend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btAttrIdAppend.Location = new System.Drawing.Point(103, 62);
            this.btAttrIdAppend.Name = "btAttrIdAppend";
            this.btAttrIdAppend.Size = new System.Drawing.Size(75, 23);
            this.btAttrIdAppend.TabIndex = 1;
            this.btAttrIdAppend.Text = "Append";
            this.btAttrIdAppend.UseVisualStyleBackColor = true;
            this.btAttrIdAppend.Click += new System.EventHandler(this.btAttrIdAppend_Click);
            // 
            // tbAttributeId
            // 
            this.tbAttributeId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbAttributeId.Location = new System.Drawing.Point(6, 36);
            this.tbAttributeId.Name = "tbAttributeId";
            this.tbAttributeId.Size = new System.Drawing.Size(172, 20);
            this.tbAttributeId.TabIndex = 0;
            this.tbAttributeId.TextChanged += new System.EventHandler(this.tbAttributeId_TextChanged);
            // 
            // lblAttributeId
            // 
            this.lblAttributeId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAttributeId.AutoSize = true;
            this.lblAttributeId.Location = new System.Drawing.Point(4, 20);
            this.lblAttributeId.Name = "lblAttributeId";
            this.lblAttributeId.Size = new System.Drawing.Size(92, 13);
            this.lblAttributeId.TabIndex = 0;
            this.lblAttributeId.Text = "Enter Attribute Id :";
            // 
            // gbText
            // 
            this.gbText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.gbText.Controls.Add(this.label1);
            this.gbText.Controls.Add(this.btTextAppend);
            this.gbText.Controls.Add(this.tbText);
            this.gbText.Controls.Add(this.lblText);
            this.gbText.Location = new System.Drawing.Point(211, 141);
            this.gbText.Name = "gbText";
            this.gbText.Size = new System.Drawing.Size(184, 107);
            this.gbText.TabIndex = 2;
            this.gbText.TabStop = false;
            this.gbText.Text = "Text";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "(For New Line enter \'\\n\')";
            // 
            // btTextAppend
            // 
            this.btTextAppend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btTextAppend.Location = new System.Drawing.Point(103, 76);
            this.btTextAppend.Name = "btTextAppend";
            this.btTextAppend.Size = new System.Drawing.Size(75, 23);
            this.btTextAppend.TabIndex = 1;
            this.btTextAppend.Text = "Append";
            this.btTextAppend.UseVisualStyleBackColor = true;
            this.btTextAppend.Click += new System.EventHandler(this.btTextAppend_Click);
            // 
            // tbText
            // 
            this.tbText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbText.Location = new System.Drawing.Point(6, 50);
            this.tbText.Name = "tbText";
            this.tbText.Size = new System.Drawing.Size(172, 20);
            this.tbText.TabIndex = 0;
            this.tbText.TextChanged += new System.EventHandler(this.tbText_TextChanged);
            // 
            // lblText
            // 
            this.lblText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblText.AutoSize = true;
            this.lblText.Location = new System.Drawing.Point(6, 16);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(62, 13);
            this.lblText.TabIndex = 1;
            this.lblText.Text = "Enter Text :";
            // 
            // errorToolTip
            // 
            this.errorToolTip.IsBalloon = true;
            this.errorToolTip.OwnerDraw = true;
            this.errorToolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Error;
            // 
            // ToolTipFormatter
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(405, 415);
            this.Controls.Add(this.gbText);
            this.Controls.Add(this.gbAttributes);
            this.Controls.Add(this.gbContextParameters);
            this.Controls.Add(this.lblToolTipFormatStringHeader);
            this.Controls.Add(this.tbToolTipFormatText);
            this.Controls.Add(this.lblAction);
            this.Controls.Add(this.gbSeparator);
            this.Controls.Add(this.btClearAll);
            this.Controls.Add(this.btUndo);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.btCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ToolTipFormatter";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ToolTipFormatter";
            this.gbContextParameters.ResumeLayout(false);
            this.gbContextParameters.PerformLayout();
            this.gbAttributes.ResumeLayout(false);
            this.gbAttributes.PerformLayout();
            this.gbText.ResumeLayout(false);
            this.gbText.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button btUndo;
        private System.Windows.Forms.Button btClearAll;
        private System.Windows.Forms.GroupBox gbSeparator;
        private System.Windows.Forms.Label lblAction;
        private System.Windows.Forms.Label lblToolTipFormatStringHeader;
        private System.Windows.Forms.GroupBox gbContextParameters;
        private System.Windows.Forms.GroupBox gbAttributes;
        private System.Windows.Forms.GroupBox gbText;
        private System.Windows.Forms.CheckBox cbOrganization;
        private System.Windows.Forms.CheckBox cbLocale;
        private System.Windows.Forms.CheckBox cbRelationshipType;
        private System.Windows.Forms.CheckBox cbNodeType;
        private System.Windows.Forms.CheckBox cbCategory;
        private System.Windows.Forms.CheckBox cbCatalogSN;
        private System.Windows.Forms.CheckBox cbCatalogLN;
        private System.Windows.Forms.TextBox tbAttributeId;
        private System.Windows.Forms.Label lblAttributeId;
        private System.Windows.Forms.Button btAttrIdAppend;
        private System.Windows.Forms.Label lblText;
        private System.Windows.Forms.Button btTextAppend;
        private System.Windows.Forms.TextBox tbText;
        private System.Windows.Forms.TextBox tbToolTipFormatText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolTip errorToolTip;
    }
}
