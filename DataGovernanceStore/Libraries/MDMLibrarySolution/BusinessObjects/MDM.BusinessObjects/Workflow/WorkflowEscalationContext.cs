using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.Workflow
{
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the workflow escalation context object.
    /// </summary>
    [DataContract]
    public class WorkflowEscalationContext :IWorkflowEscalationContext
    {
        #region Fields        

        /// <summary>
        /// Field denoting the list of entity Ids
        /// </summary>
        private Collection<Int64> _entityIds = null;

        /// <summary>
        /// Field denoting the security user login 
        /// </summary>
        private String _userLogin = String.Empty;

        /// <summary>
        /// Field denoting the elapsed time in hours
        /// </summary>
        private Int32 _elapsedTime = -1;

        /// <summary>
        /// Field denoting the workflow name and the activity names
        /// </summary>
        private Dictionary<String, Collection<String>> _workflowNamesAndActivityNames = null;

        #endregion 

        #region Constructor

        #endregion 

        #region Properties        

        /// <summary>
        /// Property denoting the list of entity Ids.
        /// </summary>
        [DataMember]
        public Collection<Int64> EntityIds
        {
            get
            {
                if (_entityIds == null)
                {
                    _entityIds = new Collection<Int64>();
                }
                return this._entityIds;
            }
            set
            {
                this._entityIds = value;
            }
        }

        /// <summary>
        /// Property denoting the security user login.
        /// This is an optional property.If not specified, all the users acting for the specified workflow and activities are considered.
        /// </summary>
        [DataMember]
        public String UserLogin
        {
            get
            {
                return this._userLogin;
            }
            set
            {
                this._userLogin = value;
            }
        }

        /// <summary>
        /// Property denoting the elapsed time in hours for an Activity.
        /// This is an optional property. If not specified, the elapsed time is 0.
        /// </summary>
        [DataMember]
        public Int32 ElapsedTime
        {
            get
            {
                return this._elapsedTime;
            }
            set
            {
                this._elapsedTime = value;
            }
        }

        /// <summary>
        /// Property denoting the list of workflow short names and the collection of activity long names .
        /// Key is workflow short names and value is activity long names.
        /// </summary>
        [DataMember]
        public Dictionary<String, Collection<String>> WorkflowNamesAndActivityNames
        {
            get
            {
                if (_workflowNamesAndActivityNames == null)
                {
                    _workflowNamesAndActivityNames = new Dictionary<String, Collection<String>>();
                }

                return this._workflowNamesAndActivityNames;
            }
            set
            {
                this._workflowNamesAndActivityNames = value;
            }
        }

        #endregion

        #region IWorkflowEscalationContext Members

        /// <summary>
        /// Sets the workflow short name and an activity long name.
        /// Activity name is an optional parameter. If not specified, all the activities in the specified workflow are considered.
        /// </summary>
        /// <param name="workflowName">Indicates the workflow short name</param>
        /// <param name="activityName">Indicates an activity long name</param>
        public void SetWorkflowAndActivityName(String workflowName, String activityName)
        {
            Collection<String> activityNames = null;
            if (!String.IsNullOrWhiteSpace(activityName))
            {
                activityNames = new Collection<String>();
                activityNames.Add(activityName);
            }

            this.SetWorkflowDetails(workflowName, activityNames);
        }

        /// <summary>
        /// Sets the workflow short name and an activity long names. 
        /// Activity names is an optional parameter. If not specified, all the activities in the specified workflow are considered.
        /// </summary>
        /// <param name="workflowName">Indicates the Workflow short name</param>
        /// <param name="activityNames">Indicates an activity long names</param>
        public void SetWorkflowAndActivityName(String workflowName, Collection<String> activityNames)
        {
            this.SetWorkflowDetails(workflowName, activityNames);
        }

        /// <summary>
        /// Gets the collection of workflow short names.
        /// </summary>
        /// <returns>Returns the collection of workflow short names</returns>
        public Collection<String> GetWorkflowNames()
        {
            Collection<String> result = null;

            if (this.WorkflowNamesAndActivityNames != null && this.WorkflowNamesAndActivityNames.Keys != null && this.WorkflowNamesAndActivityNames.Keys.Count > 0)
            {
                result = new Collection<String>();

                foreach (String wfName in this.WorkflowNamesAndActivityNames.Keys)
                {
                    result.Add(wfName);
                }
            }
            return result;
        }

        /// <summary>
        /// Gets the collection of activity names based on the workflow short name.
        /// </summary>
        /// <param name="workflowName">Indicates the workflow short name</param>
        /// <returns>Returns the collection of activity long names</returns>
        public Collection<String> GetActivityNames(String workflowName)
        {
            Collection<String> result = null;
            this.CheckWorkflowName(workflowName);

            if (this.WorkflowNamesAndActivityNames != null && this.WorkflowNamesAndActivityNames.Keys != null && this.WorkflowNamesAndActivityNames.Keys.Count > 0)
            {
                result = new Collection<String>();

                var activities = this.WorkflowNamesAndActivityNames[workflowName];

                if (activities != null && activities.Count > 0)
                {
                    foreach (String activityName in activities)
                    {
                        result.Add(activityName);
                    }
                }
            }
            return result;
        }

        private void CheckWorkflowName(String workflowName)
        {
            if (String.IsNullOrWhiteSpace(workflowName))
            {
                throw new ArgumentNullException("workflowName", "Workflow name cannot be null or empty");
            }
        }

        /// <summary>
        /// Sets workflow details with specified workflow short name and specified activity long names.
        /// </summary>
        /// <param name="workflowName">Indicates the Workflow short name</param>
        /// <param name="activityNames">Indicates an activity long names</param>
        private void SetWorkflowDetails(String workflowName, Collection<String> activityNames)
        {
            this.CheckWorkflowName(workflowName);

            if (!this.WorkflowNamesAndActivityNames.ContainsKey(workflowName))
            {
                WorkflowNamesAndActivityNames.Add(workflowName, activityNames);
            }
            else
            {
                var existingActivityNames = this.WorkflowNamesAndActivityNames[workflowName];

                if (existingActivityNames != null && existingActivityNames.Count > 0)
                {
                    foreach (String name in activityNames)
                    {
                        if (!existingActivityNames.Contains(name))
                        {
                            existingActivityNames.Add(name);
                        }
                    }

                    WorkflowNamesAndActivityNames[workflowName] = existingActivityNames;
                }
                else
                {
                    WorkflowNamesAndActivityNames[workflowName] = activityNames;
                }
            }
        }
        #endregion
    }
}
