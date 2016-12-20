using System;
using System.Collections.Generic;
using System.Text;
using System.Activities.Tracking;

namespace MDM.WorkflowRuntimeEngine
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Workflow;
    using MDM.Workflow.TrackingManager.Business;
    using MDM.Utility;
    using MDM.BusinessObjects.Diagnostics;

    /// <summary>
    /// This class helps in receiving and saving the tracking records filtered by tracking profile
    /// into the database.
    /// </summary>
    public class CustomTrackingParticipant : TrackingParticipant
    {
        #region Fields

        /// <summary>
        /// Field denoting CallerContext  : Who called API
        /// </summary>
        private CallerContext _callerContext = null;
        private TraceSettings _currentTraceSettings = null;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property denoting denoting CallerContext  : Who called API
        /// </summary>
        public CallerContext CallerContext
        {
            get { return _callerContext; }
            set { _callerContext = value; }
        }

        #endregion

        #region Constructors

        public CustomTrackingParticipant()
        {
            _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
        }
        #endregion

        #region Methods

        #region Public Methods
        #endregion

        #region Private Methods

        /// <summary>
        /// Processes Tracking Records
        /// </summary>
        /// <param name="record"></param>
        /// <param name="timeout"></param>
        protected override void Track(TrackingRecord record, TimeSpan timeout)
        {
            DiagnosticActivity diagnosticActivity = null;
            DurationHelper durationHelper = null;

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity = MDMTraceHelper.CreateTraceActivity(MDMTraceSource.AdvancedWorkflow, "CustomTrackingParticipant.Track");
                diagnosticActivity.Start();
                durationHelper = new DurationHelper(DateTime.Now);
            }

            try
            {
                //Tracking of Activity related data
                ActivityStateRecord activityStateRecord = record as ActivityStateRecord;
                if (activityStateRecord != null)
                {
                    TrackActivityRecord(activityStateRecord);
                    return;
                }

                //Tracking of workflow instance related data
                WorkflowInstanceRecord instanceTrackingRecord = record as WorkflowInstanceRecord;
                if (instanceTrackingRecord != null)
                {
                    TrackInstanceRecord(instanceTrackingRecord);
                    return;
                }

                //Tracking Custom Records
                CustomTrackingRecord customTrackingRecord = record as CustomTrackingRecord;
                if (customTrackingRecord != null)
                {
                    TrackCustomRecord(customTrackingRecord);
                    return;
                }

                //Tracking of aborted workflow instance
                WorkflowInstanceAbortedRecord instanceAbortedRecord = record as WorkflowInstanceAbortedRecord;
                if (instanceAbortedRecord != null)
                {
                    TrackInstanceAbortedRecord(instanceAbortedRecord);
                    return;
                }

                //Tracking of terminated workflow instance
                WorkflowInstanceTerminatedRecord instanceTerminatedRecord = record as WorkflowInstanceTerminatedRecord;
                if (instanceTerminatedRecord != null)
                {
                    TrackInstanceTerminatedRecord(instanceTerminatedRecord);
                    return;
                }

                //Tracking of faults
                FaultPropagationRecord faultTrackingRecord = record as FaultPropagationRecord;
                if (faultTrackingRecord != null)
                {
                    TrackFaultRecord(faultTrackingRecord);
                    return;
                }
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    if (durationHelper != null)
                    {
                        diagnosticActivity.LogMessageWithDuration(MessageClassEnum.Information, "", "CustomTrackingParticipant.Track", durationHelper.GetCumulativeTimeSpanInMilliseconds());
                    }

                    if (diagnosticActivity != null)
                    {
                        diagnosticActivity.Stop();
                    }
                }
            }
        }

        /// <summary>
        /// Tracks WorkflowInstanceRecord
        /// </summary>
        /// <param name="instanceTrackingRecord">WorkflowInstanceRecord</param>
        private void TrackInstanceRecord(WorkflowInstanceRecord instanceTrackingRecord)
        {
            DiagnosticActivity diagnosticActivity = null;
            DurationHelper durationHelper = null;

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity = MDMTraceHelper.CreateTraceActivity(MDMTraceSource.AdvancedWorkflow, "CustomTrackingParticipant.TrackInstanceRecord");
                diagnosticActivity.Start();
                durationHelper = new DurationHelper(DateTime.Now);
            }

            try
            {
                InstanceTracking instanceTracking = new InstanceTracking();
                InstanceTrackingBL instanceTrackingBL = new InstanceTrackingBL();

                if (instanceTrackingRecord.State != "Deleted")
                {
                    //Populate instance tracking object with the instance tracking record data
                    if (instanceTrackingRecord.InstanceId != null)
                        instanceTracking.RuntimeInstanceId = instanceTrackingRecord.InstanceId.ToString();

                    if (instanceTrackingRecord.State != "Canceled")
                    {
                        instanceTracking.Status = instanceTrackingRecord.State;
                    }
                    else
                    {
                        instanceTracking.Status = "Completed";
                    }

                    //Create the the record in the database
                    instanceTrackingBL.Create(instanceTracking, this._callerContext);
                }
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    if (durationHelper != null)
                    {
                        diagnosticActivity.LogMessageWithDuration(MessageClassEnum.Information, "", "CustomTrackingParticipant.TrackInstanceRecord", durationHelper.GetCumulativeTimeSpanInMilliseconds());
                    }

                    if (diagnosticActivity != null)
                    {
                        diagnosticActivity.Stop();
                    }
                }
            }

        }

        /// <summary>
        /// Tracks ActivityStateRecord and also saves the the data in the Running Instance table
        /// </summary>
        /// <param name="activityStateRecord">ActivityStateRecord</param>
        private void TrackActivityRecord(ActivityStateRecord activityStateRecord)
        {
            DiagnosticActivity diagnosticActivity = null;

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity = MDMTraceHelper.CreateTraceActivity(MDMTraceSource.AdvancedWorkflow, "CustomTrackingParticipant.TrackActivityRecord");
                diagnosticActivity.Start();
            }

            try
            {
                WorkflowDataContext wfDataContext = null;
                WorkflowActionContext wfActionContext = null;
                TrackedActivityInfo activityTracking = new TrackedActivityInfo();
                ActivityTrackingBL activityTrackingBL = new ActivityTrackingBL();

                //Populate activity tracking object with the activity state record data
                if (activityStateRecord.InstanceId != null)
                    activityTracking.RuntimeInstanceId = activityStateRecord.InstanceId.ToString();

                if (activityStateRecord.Activity != null)
                {
                    activityTracking.WorkflowDefinitionActivityID = activityStateRecord.Activity.Id;
                    activityTracking.ActivityLongName = activityStateRecord.Activity.Name;
                }

                activityTracking.Status = activityStateRecord.State;

                // Concatenate all the variables into a XML format
                // Format : <Variables><Variable Key = "" Value = "" /></Variables>
                IDictionary<String, object> activityVariables = activityStateRecord.Variables;

                StringBuilder sbVariables = new StringBuilder();
                sbVariables.AppendLine("<Variables>");

                if (activityVariables != null && activityVariables.Count > 0)
                {
                    foreach (KeyValuePair<string, object> variable in activityVariables)
                    {
                        if (variable.Value != null)
                        {
                            sbVariables.AppendLine(String.Format(@"<Variable Key = ""{0}"" Value = ""{1}"" />",
                            variable.Key, variable.Value.ToString()));
                        }
                    }
                }

                sbVariables.AppendLine("</Variables>");

                activityTracking.Variables = sbVariables.ToString();

                // Concatenate all arguments into a XML Format
                // Format : <Arguments><Argument Key = "" Value = "" /></Arguments>
                IDictionary<String, object> activityArguments = activityStateRecord.Arguments;

                StringBuilder sbArguments = new StringBuilder();
                sbArguments.AppendLine("<Arguments>");

                if (activityArguments != null && activityArguments.Count > 0)
                {
                    foreach (KeyValuePair<string, object> argument in activityArguments)
                    {
                        if (argument.Key.Equals("MDMDataContext") && argument.Value is WorkflowDataContext)
                        {
                            wfDataContext = argument.Value as WorkflowDataContext;
                        }
                        else if (argument.Key.Equals("MDMActionContext") && argument.Value is WorkflowActionContext)
                        {
                            wfActionContext = argument.Value as WorkflowActionContext;
                        }
                        else
                        {
                            if (argument.Value != null)
                            {
                                switch (argument.Key)
                                {
                                    case "Name":
                                        activityTracking.ActivityShortName = argument.Value.ToString();
                                        break;
                                    case "IsHumanActivity":
                                        Boolean isHumanActivity = false;
                                        Boolean.TryParse(argument.Value.ToString(), out isHumanActivity);
                                        activityTracking.IsHumanActivity = isHumanActivity;
                                        break;
                                    case "AssignedUser":
                                        int userId = 0;
                                        Int32.TryParse(argument.Value.ToString(), out userId);
                                        activityTracking.ActingUserId = userId;
                                        break;
                                    case "AllowedUsers":
                                        activityTracking.AssignedUsers = argument.Value.ToString();
                                        break;
                                    case "AllowedRoles":
                                        activityTracking.AssignedRoles = argument.Value.ToString();
                                        break;
                                    case "AssignmentType":
                                        AssignmentType assignmentType = AssignmentType.RoundRobin;
                                        Enum.TryParse<AssignmentType>(argument.Value.ToString(), out assignmentType);
                                        activityTracking.AssignementType = assignmentType;
                                        break;
                                    case "SortOrder":
                                        int sortOrder = 0;
                                        Int32.TryParse(argument.Value.ToString(), out sortOrder);
                                        activityTracking.SortOrder = sortOrder;
                                        break;
                                }

                                sbArguments.AppendLine(String.Format(@"<Argument Key = ""{0}"" Value = ""{1}"" />",
                                            argument.Key, argument.Value.ToString()));
                            }
                        }
                    }
                }

                sbArguments.AppendLine("</Arguments>");

                activityTracking.Arguments = sbArguments.ToString();

                if (wfDataContext != null)
                {
                    activityTracking.MDMObjectCollection = wfDataContext.MDMObjectCollection;
                    activityTracking.ExtendedProperties = wfDataContext.ExtendedProperties;
                    activityTracking.WorkflowVersionId = wfDataContext.WorkflowVersionId;
                    activityTracking.WorkflowName = wfDataContext.WorkflowName;
                    activityTracking.WorkflowLongName = wfDataContext.WorkflowLongName;
                }

                if (wfActionContext != null)
                {
                    if (activityStateRecord.State == ActivityStates.Closed)
                    {
                        if (!String.IsNullOrEmpty(wfActionContext.ExtendedProperties))
                        {
                            activityTracking.ExtendedProperties = wfActionContext.ExtendedProperties;
                        }

                        //At the action context while it was populating User behaves like acting user
                        //Once closed record has been generated, User behaves like acted user
                        //That is why, at the ActionContext level, User is defined as Acting User and at the ActivityRecord level, User is defined as Acted user
                        activityTracking.PerformedAction = wfActionContext.UserAction;
                        activityTracking.ActedUserId = wfActionContext.ActingUserId;
                        activityTracking.ActivityComments = wfActionContext.Comments;
                    }
                    else if (activityStateRecord.State == ActivityStates.Executing)
                    {
                        activityTracking.PreviousActivityShortName = wfActionContext.CurrentActivityName;
                        activityTracking.LastActivityComments = wfActionContext.Comments;
                    }
                }

                //Create the the record in the database
                activityTrackingBL.Create(activityTracking, this._callerContext, wfActionContext);
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    if (diagnosticActivity != null)
                    {
                        diagnosticActivity.Stop();
                    }
                }
            }

        }

        /// <summary>
        /// Tracks Custom Records
        /// </summary>
        /// <param name="customTrackingRecord">CustomTrackingRecord</param>
        private void TrackCustomRecord(CustomTrackingRecord customTrackingRecord)
        {
            DiagnosticActivity diagnosticActivity = null;

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity = MDMTraceHelper.CreateTraceActivity(MDMTraceSource.AdvancedWorkflow, "CustomTrackingParticipant.TrackCustomRecord");
                diagnosticActivity.Start();
            }

            try
            {
                WorkflowDataContext wfDataContext = null;
                TrackedActivityInfo customTracking = new TrackedActivityInfo();
                ActivityTrackingBL customTrackingBL = new ActivityTrackingBL();

                //Populate activity tracking object with the custom tracking record data
                if (customTrackingRecord.InstanceId != null)
                    customTracking.RuntimeInstanceId = customTrackingRecord.InstanceId.ToString();

                if (customTrackingRecord.Activity != null)
                    customTracking.ActivityLongName = customTrackingRecord.Activity.Name;

                // Concatenate all the custom data into a XML Format
                // Format : <CustomData><CustomProperty Key = "" Value = "" /></CustomData>
                IDictionary<String, object> customData = customTrackingRecord.Data;

                StringBuilder sbCustomData = new StringBuilder();
                sbCustomData.AppendLine("<CustomData>");

                if (customData != null && customData.Count > 0)
                {
                    foreach (KeyValuePair<string, object> customProperty in customData)
                    {
                        if (customProperty.Key.Equals("MDMDataContext") && customProperty.Value is WorkflowDataContext)
                        {
                            wfDataContext = customProperty.Value as WorkflowDataContext;
                        }
                        else
                        {
                            sbCustomData.AppendLine(String.Format(@"<CustomProperty Key = ""{0}"" Value = ""{1}"" />",
                            customProperty.Key, customProperty.Value));
                        }
                    }
                }
                sbCustomData.AppendLine("</CustomData>");

                customTracking.CustomData = sbCustomData.ToString();

                if (wfDataContext != null)
                {
                    customTracking.ExtendedProperties = wfDataContext.ExtendedProperties;
                }

                //Create the the record in the database
                customTrackingBL.Create(customTracking, this._callerContext);
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    if (diagnosticActivity != null)
                    {
                        diagnosticActivity.Stop();
                    }
                }
            }

        }

        /// <summary>
        /// Tracks WorkflowInstanceAbortedRecord
        /// </summary>
        /// <param name="instanceAbortedRecord">WorkflowInstanceAbortedRecord</param>
        private void TrackInstanceAbortedRecord(WorkflowInstanceAbortedRecord instanceAbortedRecord)
        {
            DiagnosticActivity diagnosticActivity = null;

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity = MDMTraceHelper.CreateTraceActivity(MDMTraceSource.AdvancedWorkflow, "CustomTrackingParticipant.TrackInstanceAbortedRecord");
                diagnosticActivity.Start();
            }

            try
            {
                FaultTracking faultTracking = new FaultTracking();
                FaultTrackingBL faultTrackingBL = new FaultTrackingBL();

                //Populate fault tracking object with the instance aborted tracking record data
                if (instanceAbortedRecord.InstanceId != null)
                    faultTracking.RuntimeInstanceId = instanceAbortedRecord.InstanceId.ToString();
                faultTracking.FaultMessage = XmlSerializationHelper.XmlEncode(instanceAbortedRecord.Reason);

                //Create the the record in the database
                faultTrackingBL.Create(faultTracking, "Aborted", this._callerContext);
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    if (diagnosticActivity != null)
                    {
                        diagnosticActivity.Stop();
                    }
                }
            }

        }

        /// <summary>
        /// Tracks WorkflowInstanceTerminatedRecord
        /// </summary>
        /// <param name="instanceTerminatedRecord">WorkflowInstanceTerminatedRecord</param>
        private void TrackInstanceTerminatedRecord(WorkflowInstanceTerminatedRecord instanceTerminatedRecord)
        {
            DiagnosticActivity diagnosticActivity = null;

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity = MDMTraceHelper.CreateTraceActivity(MDMTraceSource.AdvancedWorkflow, "CustomTrackingParticipant.TrackInstanceTerminatedRecord");
                diagnosticActivity.Start();
            }

            try
            {
                FaultTracking faultTracking = new FaultTracking();
                FaultTrackingBL faultTrackingBL = new FaultTrackingBL();

                //Populate fault tracking object with the instance aborted tracking record data
                if (instanceTerminatedRecord.InstanceId != null)
                    faultTracking.RuntimeInstanceId = instanceTerminatedRecord.InstanceId.ToString();

                faultTracking.FaultMessage = XmlSerializationHelper.XmlEncode(instanceTerminatedRecord.Reason);

                //Create the the record in the database
                faultTrackingBL.Create(faultTracking, "Terminated", this._callerContext);
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    if (diagnosticActivity != null)
                    {
                        diagnosticActivity.Stop();
                    }
                }
            }

        }

        /// <summary>
        /// Tracks FaultPropagationRecord
        /// </summary>
        /// <param name="faultTrackingRecord">FaultPropagationRecord</param>
        private void TrackFaultRecord(FaultPropagationRecord faultTrackingRecord)
        {
            DiagnosticActivity diagnosticActivity = null;

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity = MDMTraceHelper.CreateTraceActivity(MDMTraceSource.AdvancedWorkflow, "CustomTrackingParticipant.TrackFaultRecord");
                diagnosticActivity.Start();
            }

            try
            {
                FaultTracking faultTracking = new FaultTracking();
                FaultTrackingBL faultTrackingBL = new FaultTrackingBL();

                //Populate fault tracking object with the fault tracking record data
                if (faultTrackingRecord.InstanceId != null)
                    faultTracking.RuntimeInstanceId = faultTrackingRecord.InstanceId.ToString();

                if (faultTrackingRecord.Fault != null)
                {
                    faultTracking.ApplicationSource = XmlSerializationHelper.XmlEncode(faultTrackingRecord.Fault.Source);
                    faultTracking.FaultMessage = XmlSerializationHelper.XmlEncode(faultTrackingRecord.Fault.Message);
                    faultTracking.StackTrace = XmlSerializationHelper.XmlEncode(faultTrackingRecord.Fault.StackTrace);
                }

                if (faultTrackingRecord.FaultSource != null)
                    faultTracking.FaultActivitySourceName = XmlSerializationHelper.XmlEncode(faultTrackingRecord.FaultSource.Name);

                //Create the the record in the database
                faultTrackingBL.Create(faultTracking, String.Empty, this._callerContext);

            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    if (diagnosticActivity != null)
                    {
                        diagnosticActivity.Stop();
                    }
                }
            }

        }

        #endregion

        #endregion
    }
}