using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.ComponentModel;
using System.ComponentModel.Design;


namespace RS.MDM.ComponentModel.Design
{
    /// <summary>
    /// Provides functionality for editing an object in propertygrid control
    /// </summary>
    public sealed class PropertiesTypeEditor : UITypeEditor
    {

        #region Overrides

        /// <summary>
        /// Edits the value of the specified object using the editor style indicated
        ///     by the System.Drawing.Design.UITypeEditor.GetEditStyle() method.
        /// </summary>
        /// <param name="context">An System.ComponentModel.ITypeDescriptorContext that can be used to gain
        ///     additional context information.</param>
        /// <param name="provider">An System.IServiceProvider that this editor can use to obtain services.</param>
        /// <param name="value">The object to edit.</param>
        /// <returns>The new value of the object. If the value of the object has not changed,
        ///     this should return the same object it was passed.</returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            object _input = value;
            if (((context != null) && (context.Instance != null)) && (provider != null))
            {
                PropertiesEditor frm;
                try
                {
                    frm = new PropertiesEditor();
                    frm.Parameter = _input;
                    frm.StartPosition = FormStartPosition.CenterParent;
                    frm.ShowDialog((IWin32Window)context.Container);
                    if (frm.DialogResult == DialogResult.OK)
                    {
                        _input = frm.Parameter;
                    }
                }
                catch (Exception exception1)
                {
                    MessageBox.Show(exception1.ToString(), "String Editor", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                finally
                {
                    frm = null;
                }
            }
            return _input;
        }

        /// <summary>
        /// Gets the editor style used by the System.Drawing.Design.UITypeEditor.EditValue(System.IServiceProvider,System.Object) method.
        /// </summary>
        /// <param name="context">An System.ComponentModel.ITypeDescriptorContext that can be used to gain
        ///     additional context information.</param>
        /// <returns>
        ///     A System.Drawing.Design.UITypeEditorEditStyle value that indicates the style
        ///     of editor used by the System.Drawing.Design.UITypeEditor.EditValue(System.IServiceProvider,System.Object)
        ///     method. If the System.Drawing.Design.UITypeEditor does not support this method,
        ///     then System.Drawing.Design.UITypeEditor.GetEditStyle() will return System.Drawing.Design.UITypeEditorEditStyle.None.
        /// </returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
                return UITypeEditorEditStyle.Modal;
        }

        #endregion
    }

}
