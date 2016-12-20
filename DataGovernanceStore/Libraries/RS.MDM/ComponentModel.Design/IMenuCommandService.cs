using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RS.MDM.ComponentModel.Design
{
    /// <summary>
    /// Provides methods to manage the global designer verbs and menu commands available
    ///     in design mode, and to show some types of shortcut menus.
    /// </summary>
    public interface IMenuCommandService : System.ComponentModel.Design.IMenuCommandService 
    {
        /// <summary>
        /// Executes an action denoted by a given verb
        /// </summary>
        /// <param name="text">Indicates the action that needs to be executed</param>
        /// <param name="configObject">Indicates configuration object on which action is being performed</param>
        /// <param name="treeView">Indicates tree view of main form</param>
        void OnDesignerVerbClick(string text, Object configObject, object treeView);

        /// <summary>
        /// Adds a verb to the verb collection
        /// </summary>
        /// <param name="text">Indicates the text of a verb that needs to be added to the verb collection</param>
        void AddVerb(string text);

        /// <summary>
        /// Removes a verb from the verb collection
        /// </summary>
        /// <param name="text">Indicates the text of a verb that needs to be removed from the verb collection</param>
        void RemoveVerb(string text);


    }
}
