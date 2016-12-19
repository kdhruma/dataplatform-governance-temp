using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Design;
using System.Windows.Forms;


namespace RS.MDM.ComponentModel.Design
{
    /// <summary>
    /// Provides functionality to make the help and commands visible on a propertygrid control
    /// </summary>
    public sealed class CollectionEditor : System.ComponentModel.Design.CollectionEditor
    {

        #region Constructor

        /// <summary>
        /// Constructor with Type as input parameter
        /// </summary>
        /// <param name="type">Indicates the type of an object being edited</param>
        public CollectionEditor(Type type) : base(type) 
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a Collection form for editing a collection
        /// </summary>
        /// <returns>A Collection Form</returns>
        protected override CollectionForm CreateCollectionForm()
        {
            System.ComponentModel.Design.CollectionEditor.CollectionForm form = base.CreateCollectionForm();
            form.Shown += delegate
            {
                ShowDescriptionAndCommands(form);
            };
            return form;
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Iterated through all the propertygrid controls and makes the Help and Commands visible
        /// </summary>
        /// <param name="control">Indicates the PropertyGrid control.</param>
        static void ShowDescriptionAndCommands(Control control)
        {
            PropertyGrid grid = control as PropertyGrid;
            if (grid != null)
            { 
                grid.HelpVisible = true;
                grid.CommandsVisibleIfAvailable = true;
            }
            foreach (Control child in control.Controls)
            {
                ShowDescriptionAndCommands(child);
            }
        }

        #endregion
    }

}
