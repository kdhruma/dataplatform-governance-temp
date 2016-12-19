using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Activities;
using System.Activities.XamlIntegration;
using System.Xml;
using System.IO;
using System.Text;

using MDM.Core;
using MDM.Workflow.Activities.Core;
using MDMBOW = MDM.BusinessObjects.Workflow;

namespace MDM.Workflow.Designer
{
    /// <summary>
    /// This class is used to get list of activities in a workflow out of a xaml code
    /// </summary>
    public class ActivityExtractor
    {
        #region Fields

        private Int32 _workflowVersionID = 0;

        #endregion

        #region Constructor

        public ActivityExtractor(Int32 workflowVersionID)
        {
            this._workflowVersionID = workflowVersionID;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get collection of activities from a workflow definition
        /// </summary>
        /// <param name="workflowVersion">workflow version which containers workflow definition to parsed</param>
        /// <returns></returns>
        public Collection<MDMBOW.WorkflowActivity> GetActivities(MDMBOW.WorkflowVersion workflowVersion)
        {
            this._workflowVersionID = workflowVersion.Id;
            return GetActivities(workflowVersion.WorkflowDefinition);
        }

        /// <summary>
        /// Get collection of activities from a workflow definition
        /// </summary>
        /// <param name="xaml">xaml file to be parsed for activity list</param>
        /// <returns></returns>
        public Collection<MDMBOW.WorkflowActivity> GetActivities(String xaml)
        {
            Collection<MDMBOW.WorkflowActivity> worklflowActivityList = new Collection<MDMBOW.WorkflowActivity>();

            //parse the xaml code and get activity object
            XmlTextReader xmlReader = new XmlTextReader(new StringReader(xaml));

            try
            {
                Activity root = ActivityXamlServices.Load(xmlReader);

                //pass to a recursive function which populates the activity collection
                TraverseActivity(root, worklflowActivityList);
            }
            finally
            {
                if (xmlReader != null)
                    xmlReader.Close();
            }

            return worklflowActivityList;
        }

        private void TraverseActivity(Activity root, Collection<MDMBOW.WorkflowActivity> worklflowActivityList)
        {
            //get the child activities from the current activity
            IEnumerator<Activity> activities =
                WorkflowInspectionServices.GetActivities(root).GetEnumerator();

            //The base class are MDMNativeSystemActivityBase (SystemActivity),  MDMNativeActivitiyBase is subclass of MDMNativeActivitiyBase
            if (root is MDMNativeSystemActivityBase)
            {
				MDMNativeSystemActivityBase mdmActivity = root as MDMNativeSystemActivityBase;

                //Check if the system activity can be persisted to the repository. if this property is set to true
                //the activity is stored in the tb_Workflow_Activity table.
	            if (mdmActivity.PersistToRepository)
	            {
					//populate the BO and add in collection
		            MDMBOW.WorkflowActivity workflowActivity = PrepareWorkflowActivity(mdmActivity);
		            worklflowActivityList.Add(workflowActivity);
	            }
            }

            //add actitivy in list, move ahead and populate child activities of current activity, if any
            while (activities != null && activities.MoveNext())
            {
                //try searching for child activities of current
                TraverseActivity(activities.Current, worklflowActivityList);
            }
        }
		//
		private MDMBOW.WorkflowActivity PrepareWorkflowActivity(MDMNativeSystemActivityBase activity)
        {
            MDMBOW.WorkflowActivity workflowActivity = new MDMBOW.WorkflowActivity();

            String activityName = String.Empty;

            if (activity.Name != null && activity.Name.Expression != null)
            {
                activityName = activity.Name.Expression.ToString();
            }

            //IMDMActivity does not have DisplayName so get it from activity if possible
            if (activity is Activity)
            {
                workflowActivity.LongName = MDM.Utility.XmlSerializationHelper.XmlEncode((activity as Activity).DisplayName);
            }
            else
            {
                workflowActivity.LongName = activityName;
            }

            workflowActivity.Name = activityName;

            workflowActivity.WorkflowVersionId = this._workflowVersionID;

            workflowActivity.Action = Core.ObjectAction.Create;

            Boolean isHumanActivitiy = false;

            if (activity.IsHumanActivity != null && activity.IsHumanActivity.Expression != null)
            {
                Boolean.TryParse(activity.IsHumanActivity.Expression.ToString(), out isHumanActivitiy);
            }

            workflowActivity.IsHumanActivity = isHumanActivitiy;

            if (activity.AllowedRoles != null && activity.AllowedRoles.Expression != null)
            {
                workflowActivity.AllowedRoles = MDM.Utility.XmlSerializationHelper.XmlEncode(activity.AllowedRoles.Expression.ToString());
            }

            if (activity.Actions != null)
            {
                Collection<MDMBOW.ActivityAction> activityActionsCollection = new Collection<MDMBOW.ActivityAction>();

                String[] actionRows = activity.Actions.Split(";".ToCharArray());

                foreach (String actionRow in actionRows)
                {
                    String[] actionColumns = actionRow.Split(",".ToCharArray());

                    MDMBOW.ActivityAction activityAction = new MDMBOW.ActivityAction();
                    activityAction.Name = actionColumns[0].ToString();

                    CommentsRequired commentsRequired = CommentsRequired.None;
                    Enum.TryParse<CommentsRequired>(actionColumns[1].ToString(), out commentsRequired);
                    activityAction.CommentsRequired = commentsRequired;
                    //Read TransitionMessageCode only if it is available.
                    if (actionColumns.Length > 2)
                    {
                        activityAction.TransitionMessageCode = actionColumns[2].ToString();
                    }

                    activityActionsCollection.Add(activityAction);
                }

                workflowActivity.ActivityActionsCollection = activityActionsCollection;
            }

            //Processing to check if the activity is Human activity
            //If it is then information specific to Human activity is stored in the workflowActivity object
			if (activity is MDMNativeActivitiyBase)
			{
				PrepareWorkflowActivity(workflowActivity, activity as MDMNativeActivitiyBase);
			}

            return workflowActivity;
        }


	    private void PrepareWorkflowActivity(MDMBOW.WorkflowActivity workflowActivity, MDMNativeActivitiyBase activity)
	    {
			AssignmentType assignmentType = AssignmentType.RoundRobin;

            if (activity.AssignmentType != null && activity.AssignmentType.Expression != null)
            {
                Enum.TryParse<AssignmentType>(activity.AssignmentType.Expression.ToString(), out assignmentType);
            }

			workflowActivity.AssignmentType = assignmentType;

            if (activity.AllowedUsers != null && activity.AllowedUsers.Expression != null)
            {
                workflowActivity.AllowedUsers = MDM.Utility.XmlSerializationHelper.XmlEncode(activity.AllowedUsers.Expression.ToString());
            }


            if (activity.Description != null && activity.Description.Expression != null)
            {
                workflowActivity.Description = MDM.Utility.XmlSerializationHelper.XmlEncode(activity.Description.Expression.ToString());
            }

			Int32 intSortOrder = 0;
			if (activity.DisplayOrder != null && activity.DisplayOrder.Expression != null)
				Int32.TryParse(activity.DisplayOrder.Expression.ToString(), out intSortOrder);

			workflowActivity.SortOrder = intSortOrder;

			//Prepare EscalationContext XML
			StringBuilder sbEscalationContext = new StringBuilder();
			sbEscalationContext.AppendLine("&lt;EscalationContext&gt;");

			String enableEscalation = "False";
            if (activity.EnableEscalation != null && activity.EnableEscalation.Expression != null)
            {
                enableEscalation = activity.EnableEscalation.Expression.ToString();
            }

			sbEscalationContext.AppendLine(String.Format("&lt;Property Key = &quot;EnableEscalation&quot; Value = &quot;{0}&quot; /&gt;", activity.EnableEscalation.Expression.ToString()));

			Int32 iEscalationDuration = 0;
			if (activity.EscalationDuration != null && activity.EscalationDuration.Expression != null)
			{
				Double dEscalationDuration = 0;
				Double.TryParse(activity.EscalationDuration.Expression.ToString(), out dEscalationDuration);
				iEscalationDuration = (Int32)(dEscalationDuration * 24);
			}
			sbEscalationContext.AppendLine(String.Format("&lt;Property Key = &quot;EscalationDuration&quot; Value = &quot;{0}&quot; /&gt;", iEscalationDuration.ToString()));

			Int32 iAlertUserAfter = 0;
			if (activity.AlertUserAfter != null && activity.AlertUserAfter.Expression != null)
			{
				Double dAlertUserAfter = 0;
				Double.TryParse(activity.AlertUserAfter.Expression.ToString(), out dAlertUserAfter);
				iAlertUserAfter = (Int32)(dAlertUserAfter * 24);
			}
			sbEscalationContext.AppendLine(String.Format("&lt;Property Key = &quot;AlertUserAfter&quot; Value = &quot;{0}&quot; /&gt;", iAlertUserAfter.ToString()));

			Int32 iEscalateToManagerAfter = 0;
			if (activity.EscalateToManagerAfter != null && activity.EscalateToManagerAfter.Expression != null)
			{
				Double dEscalateToManagerAfter = 0;
				Double.TryParse(activity.EscalateToManagerAfter.Expression.ToString(), out dEscalateToManagerAfter);
				iEscalateToManagerAfter = (Int32)(dEscalateToManagerAfter * 24);
			}
			sbEscalationContext.AppendLine(String.Format("&lt;Property Key = &quot;EscalateToManagerAfter&quot; Value = &quot;{0}&quot; /&gt;", iEscalateToManagerAfter.ToString()));

			Int32 iRemoveFromQueueAfter = 0;
			if (activity.MoveToUnassignedPoolAfter != null && activity.MoveToUnassignedPoolAfter.Expression != null)
			{
				Double dRemoveFromQueueAfter = 0;
				Double.TryParse(activity.MoveToUnassignedPoolAfter.Expression.ToString(), out dRemoveFromQueueAfter);
				iRemoveFromQueueAfter = (Int32)(dRemoveFromQueueAfter * 24);
			}
			sbEscalationContext.AppendLine(String.Format("&lt;Property Key = &quot;RemoveFromQueueAfter&quot; Value = &quot;{0}&quot; /&gt;", iRemoveFromQueueAfter.ToString()));

			sbEscalationContext.AppendLine("&lt;/EscalationContext&gt;");

			workflowActivity.EscalationContext = sbEscalationContext.ToString();

			Boolean delegationAllowed = true;
            if (activity.DelegationAllowed != null && activity.DelegationAllowed.Expression != null)
            {
                Boolean.TryParse(activity.DelegationAllowed.Expression.ToString(), out delegationAllowed);
            }

            if (String.IsNullOrEmpty(workflowActivity.AllowedRoles) && !workflowActivity.AllowedUsers.Contains(","))
            {
                delegationAllowed = false;
            }

			//TODO:: When AllowedUsers is empty and AllowedRoles is having one role with single user, set delegationAllowed to false

			workflowActivity.DelegationAllowed = delegationAllowed;

			Boolean displayOtherUsersEntities = true;
            if (activity.DisplayOtherUsersEntities != null && activity.DisplayOtherUsersEntities.Expression != null)
            {
                Boolean.TryParse(activity.DisplayOtherUsersEntities.Expression.ToString(), out displayOtherUsersEntities);
            }

			workflowActivity.DisplayOtherUsersEntities = displayOtherUsersEntities;

			Boolean displayUnassignedEntities = true;
            if (activity.DisplayUnassignedEntities != null && activity.DisplayUnassignedEntities.Expression != null)
            {
                Boolean.TryParse(activity.DisplayUnassignedEntities.Expression.ToString(), out displayUnassignedEntities);
            }
			workflowActivity.DisplayUnassignedEntities = displayUnassignedEntities;
		}

        #endregion
    }
}
