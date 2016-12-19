using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace MDM.Workflow.Designer.Business
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Workflow;
    using MDM.ConfigurationManager.Business;
    using MDM.Workflow.Designer.Data.SqlClient;

    /// <summary>
    /// Provides business logic related functions for Workflow Activity
    /// </summary>
    public class WorkflowActivityBL : BusinessLogicBase
    {
        #region Fields

        private String _programName = "WorkflowActivityBL";

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        private void ValidateActivities(Collection<WorkflowActivity> workflowActivities, OperationResult operationResult)
        {
            Collection<String> duplicateNames = new Collection<String>();
            HashSet<String> names = new HashSet<String>();
            foreach (WorkflowActivity activity in workflowActivities)
            {
                if (!names.Add(activity.LongName))
                {
                    duplicateNames.Add(activity.LongName);
                }
            }

            if (duplicateNames.Count > 0)
            {
                Object[] parameters = {String.Join(", ", duplicateNames)};
                String message = String.Format("Cannot create duplicate activity(s): {0}. The activity(s) with the specified name(s) already exist in the same workflow.", parameters);
                operationResult.AddOperationResult("", message, parameters, OperationResultType.Error);
            }
        }

        /// <summary>
        /// Get collection of workflow activities
        /// </summary>
        /// <param name="workflowVersionId">all activities which are in this workflow version</param>
        /// <returns>return a collection of activities</returns>
        public Collection<WorkflowActivity> Get(Int32 workflowVersionId)
        {
            WorkflowActivityDA workflowActivityDataManager = new WorkflowActivityDA();
            return workflowActivityDataManager.Get(workflowVersionId);
        }

        /// <summary>
        /// Process workflow activities, based on Action
        /// </summary>
        /// <param name="workflowActivities">workflow activities to process</param>
        /// <param name="loginUser">Logged in user name</param>
        /// <param name="callerContext">Context which tells which application/module called this API</param>
        /// <param name="operationResult">Object which collects results of the operation</param>
        /// <returns>returns status of the process</returns>
        public Int32 Process(Collection<WorkflowActivity> workflowActivities, String loginUser, CallerContext callerContext, OperationResult operationResult)
        {
            ValidateActivities(workflowActivities, operationResult);
            operationResult.RefreshOperationResultStatus();
            if (operationResult.HasError)
            {
                return -1;
            }

            //Get Command
            DBCommandProperties command = DBCommandHelper.Get(callerContext, Core.MDMCenterModuleAction.Create);

            WorkflowActivityDA workflowActivityDataManager = new WorkflowActivityDA();
            return workflowActivityDataManager.Process(workflowActivities, loginUser, this._programName, command);
        }
        
        /// <summary>
        /// Assigns user to this activity based on AssignmentType
        /// </summary>
        /// <param name="instanceGuid"></param>
        /// <param name="activityShortName"></param>
        /// <param name="assignmentType"></param>
        /// <param name="allowedUsers"></param>
        /// <param name="allowedRoles"></param>
        /// <param name="assignToPreviousActorOnRerun"></param>
        /// <returns>Returns the Security User object</returns>
        public SecurityUser AssignUser(String instanceGuid, String activityShortName, AssignmentType assignmentType, String allowedUsers, String allowedRoles, Boolean assignToPreviousActorOnRerun)
        {
            SecurityUser securityUser = null;

            WorkflowActivityDA workflowActivityDataManager = new WorkflowActivityDA();
            securityUser = workflowActivityDataManager.AssignUser(instanceGuid, activityShortName, assignmentType, allowedUsers, allowedRoles, assignToPreviousActorOnRerun);

            return securityUser;
        }
        
        #endregion
    }
}
