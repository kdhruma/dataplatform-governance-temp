using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get workflow activity related information.
    /// </summary>
    public interface IWorkflowActivity : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Description of the workflow Actvity
        /// </summary>
        String Description
        {
            get;
            set;
        }
    
        /// <summary>
        /// Property denoting currently acting user id on this activity
        /// </summary>
        Int32 ActingUserId { get; set; }

        /// <summary>
        /// Property denoting currently acting user on this activity
        /// </summary>
        String ActingUser { get; set; }

        /// <summary>
        /// Property denoting state of the activity
        /// </summary>
        String State { get; set; }

		/// <summary>
		/// Specifies if its  Human Activity
		/// </summary>
	    Boolean IsHumanActivity
	    {
		    get;
		    set;
	    }

	    #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of Workflow Activity
        /// </summary>
        /// <returns>Xml representation of Workflow Activity</returns>
        String ToXML();

        /// <summary>
        /// Get Xml representation of Workflow Activity based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Workflow Activity</returns>
        String ToXML(ObjectSerialization objectSerialization);

        #endregion
    }
}
