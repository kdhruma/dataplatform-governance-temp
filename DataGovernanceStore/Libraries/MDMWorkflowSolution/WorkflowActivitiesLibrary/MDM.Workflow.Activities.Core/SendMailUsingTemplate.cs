using System;
using System.Drawing;
using System.Activities;
using System.ComponentModel;
using System.Net.Mail;
using System.Collections.ObjectModel;

namespace MDM.Workflow.Activities.Core
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Workflow;
    using MDM.Workflow.Activities.Designer;
    using MDM.NotificationManager.Business;

    /// <summary>
    /// Sends a mail as per the template
    /// </summary>
    [Designer(typeof(SendMailUsingTemplateDesigner))]
    [ToolboxBitmap(typeof(SendMailUsingTemplateDesigner), "Images.SendMailUsingTemplate.bmp")]
    public class SendMailUsingTemplate : MDMCodeActivitiyBase<Boolean>
    {
        #region Fields



        #endregion

        #region Members

        [DisplayName("Mail Template Name")]
        [Category("Mail Properties")]
        [RequiredArgument]
        public InArgument<String> MailTemplateName { get; set; }

        [DisplayName("To (MDM Users)")]
        [Category("Mail Properties")]
        public InArgument<String> ToMDMUsers { get; set; }

        [DisplayName("To (e-mail Ids)")]
        [Category("Mail Properties")]
        public InArgument<String> ToMailIds { get; set; }

        [DisplayName("Cc (MDM Users)")]
        [Category("Mail Properties")]
        public InArgument<String> CcMDMUsers { get; set; }

        [DisplayName("Cc (e-mail Ids)")]
        [Category("Mail Properties")]
        public InArgument<String> CcMailIds { get; set; }

        /// <summary>
        /// Data Context in workflow client, provides input for Activity
        /// </summary>
        [DisplayName("MDM Data Context")]
        [Category("Input Arguments")]
        [RequiredArgument]
        public new InArgument<WorkflowDataContext> MDMDataContext { get; set; }

        /// <summary>
        /// Action Context in workflow client, provides action details for Activity
        /// </summary>
        [Browsable(false)]
        public new OutArgument<WorkflowActionContext> MDMActionContext { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Performs the execution of the activity
        /// </summary>
        /// <param name="context">The execution context under which the activity executes.</param>
        /// <returns>The result of the activity’s execution.</returns>
        protected override Boolean Execute(CodeActivityContext context)
        {
            Boolean isMailSendSuccessful = true;

            String currentWorkflowName = String.Empty;
            Collection<Int64> mdmObjectIds = null;
            Collection<String> toMDMUsersList = null;
            Collection<String> toMailIdsList = null;
            Collection<String> ccMDMUsersList = null;
            Collection<String> ccMailIdsList = null;

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("SendMailUsingTemplate.Execute", MDMTraceSource.AdvancedWorkflow, false);

                #region Get property values

                String currentActivityName = context.GetValue(Name);
                WorkflowDataContext participatingDataContext = context.GetValue(MDMDataContext);
                String mailTemplateName = context.GetValue(MailTemplateName);
                String toMDMUsers = context.GetValue(ToMDMUsers);
                String toMailIds = context.GetValue(ToMailIds);
                String ccMDMUsers = context.GetValue(CcMDMUsers);
                String ccMailIds = context.GetValue(CcMailIds);

                #endregion

                #region Get workflow name and participating MDM objects

                if (participatingDataContext != null)
                {
                    currentWorkflowName = participatingDataContext.WorkflowName;

                    if (participatingDataContext.MDMObjectCollection != null)
                    {
                        mdmObjectIds = participatingDataContext.MDMObjectCollection.GetMDMObjectIds();
                    }
                }

                #endregion

                #region Convert To and Cc users to collection

                if (!String.IsNullOrWhiteSpace(toMDMUsers))
                {
                    toMDMUsersList = ValueTypeHelper.SplitStringToStringCollection(toMDMUsers, ',');
                }

                if (!String.IsNullOrWhiteSpace(toMailIds))
                {
                    toMailIdsList = ValueTypeHelper.SplitStringToStringCollection(toMailIds, ',');
                }

                if (!String.IsNullOrWhiteSpace(ccMDMUsers))
                {
                    ccMDMUsersList = ValueTypeHelper.SplitStringToStringCollection(ccMDMUsers, ',');
                }

                if (!String.IsNullOrWhiteSpace(ccMailIds))
                {
                    ccMailIdsList = ValueTypeHelper.SplitStringToStringCollection(ccMailIds, ',');
                }

                #endregion

                MailNotificationBL mailNotificationManager = new MailNotificationBL();
                OperationResult oprResult = mailNotificationManager.SendMailWithWorkflowDetails(mdmObjectIds, mailTemplateName, toMDMUsersList, toMailIdsList, ccMDMUsersList, ccMailIdsList, currentWorkflowName, new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.MDMAdvanceWorkflow), false, currentActivityName, this.DisplayName);

                if (oprResult.OperationResultStatus == OperationResultStatusEnum.Failed)
                {
                    isMailSendSuccessful = false;

                    //Get the first error for logging..
                    String errorMessage = oprResult.Errors[0].ErrorMessage;
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error, String.Format("Sending Mail using Template failed. Error: {0}", errorMessage), MDMTraceSource.AdvancedWorkflow);
                }
                else
                {
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Information, "Sending Mail using Template is successful.", MDMTraceSource.AdvancedWorkflow);
                }
            }
            catch (Exception)
            {
                //TODO::Log trace
                isMailSendSuccessful = false;
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("SendMailUsingTemplate.Execute", MDMTraceSource.AdvancedWorkflow);
            }

            return isMailSendSuccessful;
        }

        /// <summary>
        /// Creates and validates a description of the activity’s arguments, variables,
        /// child activities, and activity delegates.
        /// </summary>
        /// <param name="metadata">The activity’s metadata that encapsulates the activity’s arguments, variables, child activities, and activity delegates.</param>
        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);

            String toMDMUsers = String.Empty;
            String toMailIds = String.Empty;
            String ccMailIds = String.Empty;

            if (ToMDMUsers != null && ToMDMUsers.Expression != null)
            {
                toMDMUsers = ToMDMUsers.Expression.ToString();
            }

            if (ToMailIds != null && ToMailIds.Expression != null)
            {
                toMailIds = ToMailIds.Expression.ToString();
            }

            if (CcMailIds != null && CcMailIds.Expression != null)
            {
                ccMailIds = CcMailIds.Expression.ToString();
            }

            if (String.IsNullOrWhiteSpace(toMDMUsers) && String.IsNullOrWhiteSpace(toMailIds))
            {
                metadata.AddValidationError("Value for either 'To (MDM Users)' or 'To (e-mail Ids)' is mandatory.");
            }

            if (!String.IsNullOrWhiteSpace(toMailIds))
            {
                try
                {
                    MailAddressCollection mailAddresses = new MailAddressCollection();
                    mailAddresses.Add(toMailIds);
                }
                catch (FormatException)
                {
                    metadata.AddValidationError("One or the more Mail Ids provided in 'To (e-mail Ids)' are not valid.");
                }
            }

            if (!String.IsNullOrWhiteSpace(ccMailIds))
            {
                try
                {
                    MailAddressCollection mailAddresses = new MailAddressCollection();
                    mailAddresses.Add(ccMailIds);
                }
                catch (FormatException)
                {
                    metadata.AddValidationError("One or the more Mail Ids provided in 'Cc (e-mail Ids)' are not valid.");
                }
            }
        }

        #endregion
    }
}
