using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace RS.MDM.ComponentModel.Design
{
    // A custom UITypeEditor which allows PanelDataItem's ToolTip property
    // to be changed at design time using a customized UI element
    // that is invoked by the Properties window. 
    // The UI is provided by the ToolTipFormatterControl class.
    public sealed class ToolTipEditor : UITypeEditor
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructors

        #endregion

        #region Methods

        /// <summary>
        /// Gets the edit style for the Type editor
        /// </summary>
        /// <param name="context">An System.ComponentModel.ITypeDescriptorContext that can be used to gain additional context information.</param>
        /// <returns>A System.Drawing.Design.UITypeEditorEditStyle enumeration value indicating the provided editing style.</returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            //Return Modal editor style
            return UITypeEditorEditStyle.Modal;
        }

        /// <summary>
        /// Edits the value of the specified object using the specified service provider and context.
        /// </summary>
        /// <param name="context">An System.ComponentModel.ITypeDescriptorContext that can be used to gain additional context information.</param>
        /// <param name="provider"> A service provider object through which editing services can be obtained.</param>
        /// <param name="value">The object to edit the value of.</param>
        /// <returns>The new value of the object. If the value of the object has not changed, this should return the same object it was passed.</returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService editorService = null;

            if (provider != null)
            {
                editorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            }

            if (editorService != null)
            {
                ToolTipFormatter frmToolTipFormatter = new ToolTipFormatter(value.ToString());

                if (DialogResult.OK == editorService.ShowDialog(frmToolTipFormatter))
                    value = frmToolTipFormatter.ToolTipFormatText;
            }

            return value;
        }

        #endregion
    }
}
