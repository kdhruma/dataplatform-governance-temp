using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;

namespace RS.MDM.ComponentModel.Design
{
    /// <summary>
    /// Provides functionality that can be used to design value editors that can provide
    ///     a user interface (UI) for representing and editing the values of string type
    /// </summary>
    public sealed class StringTypeEditor : UITypeEditor
    {
        #region Methods

        /// <summary>
        /// Edits the specified object's value using the editor style indicated by the
        ///     System.Drawing.Design.UITypeEditor.GetEditStyle() method.
        /// </summary>
        /// <param name="context">An System.ComponentModel.ITypeDescriptorContext that can be used to gain
        ///     additional context information.</param>
        /// <param name="provider">An System.IServiceProvider that this editor can use to obtain services.</param>
        /// <param name="value">The object to edit.</param>
        /// <returns>The new value of the object. If the value of the object has not changed,
        ///     this should return the same object it was passed.</returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            string _inputString = (string)value;
            if (((context != null) & (context.Instance != null)) & (provider != null))
            {
                StringEditor _stringEditor;
                try
                {
                    _stringEditor = new StringEditor();
                    _stringEditor.Data = _inputString;
                    _stringEditor.StartPosition = FormStartPosition.CenterParent;
                    _stringEditor.ShowDialog((IWin32Window)context.Container);
                    if (_stringEditor.DialogResult == DialogResult.OK)
                    {
                        _inputString = _stringEditor.Data;
                    }
                }
                catch (Exception exception1)
                {
                    MessageBox.Show(exception1.ToString(), "String Editor", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                finally
                {
                    _stringEditor = null;
                }
            }
            return _inputString;
        }

        /// <summary>
        /// Gets the editor style used by the System.Drawing.Design.UITypeEditor.EditValue(System.IServiceProvider,System.Object)
        ///     method.
        /// </summary>
        /// <param name="context">An System.ComponentModel.ITypeDescriptorContext that can be used to gain
        ///     additional context information.</param>
        /// <returns>A System.Drawing.Design.UITypeEditorEditStyle value that indicates the style
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
