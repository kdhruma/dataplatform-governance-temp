using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RS.MDM
{

    /// <summary>
    /// Represents the method that will handle the RS.MDM.NameChangedEvent event of a RS.MDM.Object.
    /// </summary>
    /// <param name="sender"> The source of the event.</param>
    /// <param name="e">A RS.MDM.NameChangedEventArgs object that contains the event data.</param>
    public delegate void NameChangedEventHandler(Object sender, NameChangedEventArgs e);

    /// <summary>
    /// Provides arguments for NameChangedEvent
    /// </summary>
    public sealed class NameChangedEventArgs : EventArgs
    {

        #region Fields

        /// <summary>
        /// Indicates field for new name
        /// </summary>
        private string _newName;

        /// <summary>
        /// Indicates field for old name
        /// </summary>
        private string _oldName;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor with oldName and newName as input parameters
        /// </summary>
        /// <param name="oldName">Indicates the old name of an Object</param>
        /// <param name="newName">Indicates the new name of an Object</param>
        public NameChangedEventArgs(string oldName, string newName)
        {
            this._oldName = oldName;
            this._newName = newName;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Indicates the new Name of an Object
        /// </summary>
        public string NewName
        {
            get
            {
                return this._newName;
            }
        }

        /// <summary>
        /// Indicates the old Name of an Object
        /// </summary>
        public string OldName
        {
            get
            {
                return this._oldName;
            }
        }

        #endregion

    }



}
