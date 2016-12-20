using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MDM.Workflow.TrackingManager.Business
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Workflow;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.NotificationManager.Business;
    using MDM.Utility;
    using MDM.Workflow.TrackingManager.Data;

    /// <summary>
    /// Business Logic for Escalation processing
    /// </summary>
    public class EscalationBL : BusinessLogicBase
    {
        #region Fields
        /// <summary>
        /// Field denoting the escalation data acess manager
        /// </summary>
        private EscalationDA _escalationDA;

        #endregion 

        #region Constructor

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public EscalationBL()
        {
            _escalationDA = new EscalationDA();
        }

        #endregion 

        /// <summary>
        /// Processes Escalation data
        /// </summary>
        /// <param name="callerContext">Context which tells which application/module called this API</param>
        /// <returns>Returns the records regarding Escalations</returns>
        public Collection<Escalation> Process(CallerContext callerContext)
        {
            //Get Command
            DBCommandProperties command = DBCommandHelper.Get(callerContext, Core.MDMCenterModuleAction.Create);

            Collection<Escalation> escalationData = new Collection<Escalation>();

            escalationData = _escalationDA.Process(command);

            return escalationData;
        }

        /// <summary>
        /// Get the workflow elapsed time details for the requested entities based on the escalation context
        /// </summary>
        /// <param name="escalationContext">Indicates the workflow escalation context</param>
        /// <param name="callerContext">Indicates the callerContext which contains the MDMCenter application and module</param>
        /// <returns>Returns the workflow escalation details as collection. </returns>
        public WorkflowEscalationDataCollection GetWorkflowEscalationDetails(WorkflowEscalationContext escalationContext, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("Workflow.TrackingManager.EscalationBL.GetWorkflowElapsedTimeDetails", MDMTraceSource.AdvancedWorkflow, false);
            }

            WorkflowEscalationDataCollection wfEscalations = null;

            try
            {
                ValidateInputParameters(escalationContext, callerContext);

                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

                wfEscalations = _escalationDA.GetWorkflowEscalationDetails(escalationContext, command);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("Workflow.TrackingManager.EscalationBL.GetWorkflowElapsedTimeDetails", MDMTraceSource.AdvancedWorkflow);
                }
            }
            return wfEscalations;
        }

        /// <summary>
        /// Send an escalation mail to user based on the requested escalation context and email context
        /// </summary>
        /// <param name="escalationContext">Indicates the workflow escalation context</param>
        /// <param name="emailContext">Indicates the email context</param>
        /// <param name="includeAssignedUserAsRecipient">Whether to include assigned user in To Address list in the mail or not.
        /// If value is true then along with the email context's To address list assigned user's email will be added else not.
        /// If no need to notify the assigned user then set the value as false.</param>
        /// <param name="callerContext">Indicates the callerContext which contains the MDMCenter application and module</param>
        /// <returns>Returns operation results based on the result</returns>
        public OperationResultCollection SendMailWithWorkflowEscalationDetails(WorkflowEscalationContext escalationContext, EmailContext emailContext, Boolean includeAssignedUserAsRecipient, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("Workflow.TrackingManager.EscalationBL.SendMailWithWorkflowEscalationDetails", MDMTraceSource.AdvancedWorkflow, false);
            }

            WorkflowEscalationDataCollection wfEscalations = null;
            OperationResultCollection result = new OperationResultCollection();

            try
            {
                wfEscalations = this.GetWorkflowEscalationDetails(escalationContext, callerContext);

                if (wfEscalations != null)
                {
                    if (emailContext != null)   // check why send mail API does not have null check for emailcontext
                    {
                        this.SendMail(emailContext, wfEscalations, includeAssignedUserAsRecipient, result, callerContext);
                    }
                    else
                    {
                        throw new ArgumentNullException("emailcontext parameter is null");
                    }
                }
                else
                {
                    var entityIds = ValueTypeHelper.JoinCollection(escalationContext.EntityIds, ",");
                    var workflowNames = ValueTypeHelper.JoinCollection(escalationContext.GetWorkflowNames(), ",");
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Warning, String.Format("There is no escalation found for the following context.EntityId's {0}, WorkflowNames {1}", entityIds, workflowNames), MDMTraceSource.AdvancedWorkflow);
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("Workflow.TrackingManager.EscalationBL.SendMailWithWorkflowEscalationDetails", MDMTraceSource.AdvancedWorkflow);
                }
                result.RefreshOperationResultStatus();
            }
            return result;
        }

        #region Private Methods

        private void SendMail(EmailContext emailContext, WorkflowEscalationDataCollection wfEscalations, Boolean includeAssignedUserAsRecipient, OperationResultCollection result, CallerContext callerContext)
        {
            Collection<String> actualMailAddress = emailContext.ToEmailIds == null ? new Collection<String>() : emailContext.ToEmailIds;

            foreach (WorkflowEscalationData escalation in wfEscalations)
            {
                if (includeAssignedUserAsRecipient)
                {
                    emailContext.ToEmailIds = actualMailAddress;

                    if (!String.IsNullOrWhiteSpace(escalation.AssignedUserMailAddress))
                    {
                        emailContext.ToEmailIds.Add(escalation.AssignedUserMailAddress);
                    }
                    result.Add(this.SendMailWithWorkflowDetails(escalation, emailContext, callerContext));
                }
                else
                {
                    result.Add(this.SendMailWithWorkflowDetails(escalation, emailContext, callerContext));
                }
            }
        }

        private OperationResult SendMailWithWorkflowDetails(WorkflowEscalationData escalation, EmailContext emailContext, CallerContext callerContext)
        {
            MailNotificationBL mailNotificationBL = new MailNotificationBL();
            return mailNotificationBL.SendMailWithWorkflowEscalationDetails(escalation, emailContext, callerContext);
        }

        private void ValidateInputParameters(WorkflowEscalationContext escalationContext, CallerContext callerContext)
        {
            String errorMsg = String.Empty;
            String errorMsgCode = String.Empty;

            if (callerContext == null)
            {
                errorMsg = "CallerContext cannot be null or empty.";
                errorMsgCode = "111823";
            }

            if (escalationContext == null)
            {
                errorMsg = "EscalationContext cannot be null or empty.";
                errorMsgCode = "113774"; 
            }

            if (escalationContext.EntityIds == null || escalationContext.EntityIds.Count == 0)
            {
                errorMsg = "EntityIdList cannot be null or empty";
                errorMsgCode = "111847";  
            }

            if (escalationContext.GetWorkflowNames() == null || escalationContext.GetWorkflowNames().Count == 0)
            {
                errorMsg = " Workflow name cannot be null or empty";
                errorMsgCode = "113769";
            }

            if (!String.IsNullOrWhiteSpace(errorMsgCode))   //Message code is enough to throw error
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, errorMsg, MDMTraceSource.AdvancedWorkflow);
                throw new MDMOperationException(errorMsgCode, errorMsg, "Workflow.TrackingManager.EscalationBL", String.Empty, String.Empty);
            }
        }

        #endregion 
    }
}
