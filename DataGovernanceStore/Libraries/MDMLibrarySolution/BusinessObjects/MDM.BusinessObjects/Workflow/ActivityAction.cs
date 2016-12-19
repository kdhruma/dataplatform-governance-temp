using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

using MDM.Core;

namespace MDM.BusinessObjects.Workflow
{
    /// <summary>
    /// Specifies the properties related to the particular action of an activity
    /// </summary>
    [DataContract]
    public class ActivityAction : MDMObject
    {
        #region Fields

        /// <summary>
        /// Field for the execution Status of the workflow instance
        /// </summary>
        private CommentsRequired _commentsRequired = CommentsRequired.None;

        /// <summary>
        /// Field indicating the message code for successful transition of given activity
        /// </summary>
        private String _transitionMessageCode = String.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting the Id of an object
        /// </summary>
        [DataMember]
        [Browsable(false)]
        public new Int32 Id
        {
            get { return base.Id; }
            set { base.Id = value; }
        }

        /// <summary>
        /// Property denoting the Name of an action
        /// </summary>
        [DataMember]
        [DisplayName("Action Name")]
        public override String Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }

        /// <summary>
        /// Property denoting the comments property for the action 
        /// </summary>
        [DataMember]
        [DisplayName("Comments Required")]
        public CommentsRequired CommentsRequired
        {
            get
            {
                return this._commentsRequired;
            }
            set
            {
                this._commentsRequired = value;
            }
        }

        /// <summary>
        /// Property indicating the message code for successful transition of given activity
        /// </summary>
        [DataMember]
        [DisplayName("Transition Message Code")]
        public String TransitionMessageCode
        {
            get { return _transitionMessageCode; }
            set { _transitionMessageCode = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public ActivityAction()
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of Activity action
        /// </summary>
        /// <returns>Xml representation of Activity action</returns>
        public String ToXML()
        {
            String xml = String.Empty;

            xml = "<ActivityAction ActionName=\"{0}\" CommentsRequired=\"{1}\" TransitionMessageCode=\"{2}\" />";

            String retXML = String.Format(xml, this.Name, this.CommentsRequired.ToString(), this.TransitionMessageCode);

            return retXML;
        }

        /// <summary>
        /// Determines whether specified object is equal to the current object. 
        /// </summary>
        /// <param name="obj">ActivityTracking object which needs to be compared.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false</returns>
        public override bool Equals(object obj)
        {
            if (obj is InstanceTracking)
            {
                ActivityAction objectToBeCompared = obj as ActivityAction;

                if (!base.Equals(objectToBeCompared))
                    return false;

                if (this.CommentsRequired != objectToBeCompared.CommentsRequired)
                    return false;

                if (this.TransitionMessageCode != objectToBeCompared.TransitionMessageCode)
                    return false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode() ^ this.CommentsRequired.GetHashCode() ^ this.TransitionMessageCode.GetHashCode();
        }

        #endregion
    }
}
