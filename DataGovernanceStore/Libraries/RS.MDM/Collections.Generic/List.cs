using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;

using RS.MDM.ComponentModel.Design;

namespace RS.MDM.Collections.Generic
{
    /// <summary>
    /// Provides functionality in creating and maintaining lists with the framework
    /// </summary>
    /// <typeparam name="T">The type of items in the List</typeparam>
    /// <exclude/>
    [Serializable()]
    public class List<T> : System.Collections.Generic.List<T>, RS.MDM.ComponentModel.Design.IMenuCommandService
    {

        #region Fields

        /// <summary>
        /// list of supported verbs
        /// </summary>
        private System.ComponentModel.Design.DesignerVerbCollection _designerVerbs = new System.ComponentModel.Design.DesignerVerbCollection();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public List() : base()
        {
            this.AddVerb("Edit"); 
        }

        /// <summary>
        /// Constructor with capacity as input parameter
        /// </summary>
        /// <param name="capacity">The capacity of the list</param>
        public List(int capacity)
            : base(capacity)
        {
            this.AddVerb("Edit"); 
        }

        /// <summary>
        /// Constructor with enumeration as input parameter
        /// </summary>
        /// <param name="collection">Enumeration that needs to used to initialize the list</param>
        public List(IEnumerable<T> collection)
            : base(collection)
        {
            this.AddVerb("Edit");
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the total number of elements the internal data structure can hold without resizing.
        /// </summary>
        [Browsable(false)]
        public new int Capacity
        {
            get
            {
                return base.Capacity;
            }
            set
            {
                base.Capacity = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Removes an item from the list indicated by an unique identifier
        /// </summary>
        /// <param name="uniqueIdentifier">Indicates the unique identifier of an item that needs to be removed from the list</param>
        public void Remove(string uniqueIdentifier)
        {
            T _itemToRemove = default(T);
            foreach (T _item in this)
            {
                if ((_item as RS.MDM.Object).UniqueIdentifier == uniqueIdentifier)
                {
                    _itemToRemove = _item;
                    break;
                }
            }
            if (_itemToRemove != null)
            {
                this.Remove(_itemToRemove);
            }
        }

        /// <summary>
        /// Inserts an item into the list after an item identified by provided unique identifier
        /// </summary>
        /// <param name="uniqueIdentifier">An unique identifier of an item after which the new item needs to be inserted</param>
        /// <param name="item">Indicates an item that needs to be inserted</param>
        public void InsertAfter(string uniqueIdentifier, T item)
        {
            if (string.IsNullOrEmpty(uniqueIdentifier))
            {
                this.Insert(0, item);
            }
            else
            {
                T _previousSibling = this.GetItem(uniqueIdentifier);
                if (_previousSibling as object != null)
                {
                    this.Insert(IndexOf(_previousSibling) + 1, item);
                }
                else
                {
                    this.Insert(0, item);
                }
            }
        }

        /// <summary>
        /// Gets an item identified by an unique identifier
        /// </summary>
        /// <param name="uniqueIdentifier">Indicates the unique identifier of an item that needs to be fetched</param>
        /// <returns>Return an item identified by the given unique identifier</returns>
        public T GetItem(string uniqueIdentifier)
        {
            foreach (T _item in this)
            {
                if ((_item as object != null) && ((_item as RS.MDM.Object).UniqueIdentifier == uniqueIdentifier))
                {
                    return _item;
                }
            }
            return default(T);
        }


        /// <summary>
        /// Gets an item identified by name
        /// </summary>
        /// <param name="name">Indicates the name of an item that needs to be fetched</param>
        /// <returns>Return an item identified by the given name</returns>
        public T GetItemByName(string name)
        {
            foreach (T _item in this)
            {
                if ((_item as object != null) && ((_item as RS.MDM.Object).Name == name))
                {
                    return _item;
                }
            }
            return default(T);
        }
        #endregion

        #region IMenuCommandService Members

        /// <summary>
        /// Adds the specified standard menu command to the menu.
        /// </summary>
        /// <param name="command">The System.ComponentModel.Design.MenuCommand to add.</param>
        public void AddCommand(MenuCommand command)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds the specified designer verb to the set of global designer verbs.
        /// </summary>
        /// <param name="verb">The System.ComponentModel.Design.DesignerVerb to add.</param>
        public void AddVerb(DesignerVerb verb)
        {
            if (!this._designerVerbs.Contains(verb))
            {
                this._designerVerbs.Add(verb);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        public void AddVerb(string text)
        {
            foreach (DesignerVerb _designerVerb in this._designerVerbs)
            {
                if (_designerVerb.Text == text)
                {
                    return;
                }
            }
            this._designerVerbs.Add(new System.ComponentModel.Design.DesignerVerb(text, new EventHandler(DesignerVerb_Click)));
        }

        /// <summary>
        /// Searches for the specified command ID and returns the menu command associated
        ///     with it.
        /// </summary>
        /// <param name="commandID">The System.ComponentModel.Design.CommandID to search for.</param>
        /// <returns>The System.ComponentModel.Design.MenuCommand associated with the command
        ///     ID, or null if no command is found.</returns>
        public MenuCommand FindCommand(CommandID commandID)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Invokes a menu or designer verb command matching the specified command ID.
        /// </summary>
        /// <param name="commandID">The System.ComponentModel.Design.CommandID of the command to search for and
        ///     execute.</param>
        /// <returns>true if the command was found and invoked successfully; otherwise, false.</returns>
        public bool GlobalInvoke(CommandID commandID)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes the specified standard menu command from the menu.
        /// </summary>
        /// <param name="command">The System.ComponentModel.Design.MenuCommand to remove.</param>
        public void RemoveCommand(MenuCommand command)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes the specified designer verb from the collection of global designer
        ///    verbs.
        /// </summary>
        /// <param name="verb">The System.ComponentModel.Design.DesignerVerb to remove.</param>
        public void RemoveVerb(DesignerVerb verb)
        {
            if (this._designerVerbs.Contains(verb))
            {
                this._designerVerbs.Remove(verb);
            }
        }

        /// <summary>
        /// Removes the specified designer verb from the collection of global designer
        /// </summary>
        /// <param name="text">The text of the verb to remove.</param>
        public void RemoveVerb(string text)
        {
            DesignerVerb _verbToRemove = null;
            foreach (DesignerVerb _designerVerb in this._designerVerbs)
            {
                if (_designerVerb.Text == text)
                {
                    _verbToRemove = _designerVerb;
                    break;
                }
            }
            if (_verbToRemove != null)
            {
                this._designerVerbs.Remove(_verbToRemove);
            }
        }

        /// <summary>
        /// Shows the specified shortcut menu at the specified location.
        /// </summary>
        /// <param name="menuID">The System.ComponentModel.Design.CommandID for the shortcut menu to show.</param>
        /// <param name="x">The x-coordinate at which to display the menu, in screen coordinates.</param>
        /// <param name="y">The y-coordinate at which to display the menu, in screen coordinates.</param>
        public void ShowContextMenu(CommandID menuID, int x, int y)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets or sets an array of the designer verbs that are currently available.
        /// </summary>
        [Browsable(false)]
        public DesignerVerbCollection Verbs
        {
            get { return this._designerVerbs; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DesignerVerb_Click(object sender, EventArgs e)
        {
            System.ComponentModel.Design.DesignerVerb _verb = (System.ComponentModel.Design.DesignerVerb)sender;
            this.OnDesignerVerbClick(_verb.Text, null, null);
        }

        /// <summary>
        /// Executes a verb by a given verb text
        /// </summary>
        /// <param name="text">The text of the verb that needs to be executed</param>
        /// <param name="configObject">Indicates configuration object on which action is being performed</param>
        /// <param name="treeView">Indicates tree view of main form</param>
        public void OnDesignerVerbClick(string text, Object configObject, object treeView)
        {
            Object columnObject = null;
            TreeView explorerTreeView = null;
            
            switch (text)
            {
                case "Edit":
                    ListEditor _listEditor = new ListEditor();
                    _listEditor.Tag = configObject;
                    _listEditor.List = this;
                    _listEditor.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;

                    if (DialogResult.OK == _listEditor.ShowDialog())
                    {
                        System.ComponentModel.TypeDescriptor.Refresh(this);

                        if (treeView != null && treeView is TreeView)
                        {
                            try
                            {
                                explorerTreeView = treeView as TreeView;
                                explorerTreeView.SuspendLayout();

                                explorerTreeView.SelectedNode.Nodes.Clear();
                                foreach (T obj in this)
                                {
                                    if (obj is Object)
                                    {
                                        columnObject = obj as Object;
                                        explorerTreeView.SelectedNode.Nodes.Add(columnObject.GetTreeNode());
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Trace.WriteLine(ex.ToString(), RS.MDM.Logging.LogLevel.ERROR.ToString());
                            }
                            finally
                            {
                                explorerTreeView.ResumeLayout();
                            }
                        }
                    }
                    break;
            }
        }

        #endregion
    }
}
