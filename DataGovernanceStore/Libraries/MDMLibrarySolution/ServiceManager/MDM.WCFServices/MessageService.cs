using System;
using System.Collections.ObjectModel;
using System.Net.Mail;
using System.ServiceModel;
using System.Diagnostics;

namespace MDM.WCFServices
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Workflow;
    using MDM.Core;
    using MDM.MessageManager.Business;
    using MDM.NotificationManager.Business;
    using MDM.WCFServiceInterfaces;
    using MDM.Utility;

    [DiagnosticActivity]
    [ServiceBehavior(Namespace = "http://wcfservices.riversand.com", InstanceContextMode = InstanceContextMode.PerCall)]
    public class MessageService : MDMWCFBase, IMessageService
    {
        #region Constructors

        public MessageService()
            : base(true)
        {

        }

        public MessageService(Boolean loadSecurityPrincipal)
            : base(loadSecurityPrincipal)
        {

        }

        #endregion

        public void Create(MessageType Type, String From, String To, String Subject, String Body, MailPriority Priority, ref OperationResult operationResult)
        {
            Message message = new Message();
            message.MessageFrom = From;
            message.MessageTo = To;
            message.Subject = Subject;
            message.Body = Body;
            message.Priority = Priority;
            message.NonRepliable = true;
            message.MessageType = Type;

            Create(message, ref operationResult);
        }
        public void Create(String Type, String From, String To, String Subject, String Body, String Priority, ref OperationResult operationResult)
        {
            Message message = new Message();
            message.MessageFrom = From;
            message.MessageTo = To;
            message.Subject = Subject;
            message.Body = Body;
            message.Priority = (MailPriority)Enum.Parse(typeof(MailPriority), Priority);
            message.NonRepliable = true;
            message.MessageType = (MessageType)Enum.Parse(typeof(MessageType), Type);

            Create(message, ref operationResult);
        }
        public void Create(Message Message, ref OperationResult operationResult)
        {
            try
            {
                MessageBL MessageManager = new MessageBL();

                MessageManager.Create(Message, operationResult);
            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                throw WrapException(ex);
            }
        }

        public void SendWorkflowMessages(Collection<WorkflowMDMObject> mdmObjectCollection, Int32 workflowID, Int32 workflowActivityID, Int32 userID, String templateName, ref OperationResult operationResult)
        {
            Collection<Int32> userIDs = new Collection<int>();
            userIDs.Add(userID);
            SendWorkflowMessages(mdmObjectCollection, workflowID, workflowActivityID, userIDs, templateName, ref operationResult);
        }
        public void SendWorkflowMessages(Collection<WorkflowMDMObject> mdmObjectCollection, Int32 workflowID, Int32 workflowActivityID, Collection<Int32> userIDs, String templateName, ref OperationResult operationResult)
        {
            try
            {
                MDMMessageNotificationBL messageNotificationBL = new MDMMessageNotificationBL();

                messageNotificationBL.SendWorkflowMessages(mdmObjectCollection, workflowID, workflowActivityID, userIDs, templateName, operationResult);
            }
            catch (Exception ex)
            {
                throw WrapException(ex);
            }
        }

        public void SendWorkflowMessages(Collection<WorkflowMDMObject> mdmObjectCollection, Int32 workflowVersionID, String activityName, Int32 userID, String templateName, ref OperationResult operationResult)
        {
            Collection<Int32> userIDs = new Collection<int>();
            userIDs.Add(userID);
            SendWorkflowMessages(mdmObjectCollection, workflowVersionID, activityName, userIDs, templateName, ref operationResult);
        }
        public void SendWorkflowMessages(Collection<WorkflowMDMObject> mdmObjectCollection, Int32 workflowVersionID, String activityName, Collection<Int32> userIDs, String templateName, ref OperationResult operationResult)
        {
            try
            {
                MDMMessageNotificationBL messageNotificationBL = new MDMMessageNotificationBL();

                messageNotificationBL.SendWorkflowMessages(mdmObjectCollection, workflowVersionID, activityName, userIDs, templateName, operationResult);
            }
            catch (Exception ex)
            {
                throw WrapException(ex);
            }
        }

        public void SendWorkflowMessages(String MDMObjectIdList, String MDMObjectType, Int32 workflowID, Int32 workflowActivityID, Int32 userID, String templateName, ref OperationResult operationResult)
        {
            Collection<Int32> userIDs = new Collection<int>();
            userIDs.Add(userID);
            SendWorkflowMessages(MDMObjectIdList, MDMObjectType, workflowID, workflowActivityID, userIDs, templateName, ref operationResult);
        }
        public void SendWorkflowMessages(String MDMObjectIdList, String MDMObjectType, Int32 workflowID, Int32 workflowActivityID, Collection<Int32> userIDs, String templateName, ref OperationResult operationResult)
        {
            try
            {
                MDMMessageNotificationBL messageNotificationBL = new MDMMessageNotificationBL();

                messageNotificationBL.SendWorkflowMessages(MDMObjectIdList, MDMObjectType, workflowID, workflowActivityID, userIDs, templateName, operationResult);
            }
            catch (Exception ex)
            {
                throw WrapException(ex);
            }
        }

        public void SendWorkflowMessages(String MDMObjectIdList, String MDMObjectType, Int32 workflowID, Int32 workflowActivityID, String userLogin, String templateName, ref OperationResult operationResult)
        {
            Collection<String> userLogins = new Collection<String>();
            userLogins.Add(userLogin);
            SendWorkflowMessages(MDMObjectIdList, MDMObjectType, workflowID, workflowActivityID, userLogins, templateName, ref operationResult);
        }
        public void SendWorkflowMessages(String MDMObjectIdList, String MDMObjectType, Int32 workflowID, Int32 workflowActivityID, Collection<String> userLogins, String templateName, ref OperationResult operationResult)
        {
            try
            {
                MDMMessageNotificationBL messageNotificationBL = new MDMMessageNotificationBL();

                messageNotificationBL.SendWorkflowMessages(MDMObjectIdList, MDMObjectType, workflowID, workflowActivityID, userLogins, templateName, operationResult);
            }
            catch (Exception ex)
            {
                throw WrapException(ex);
            }
        }

        public void SendWorkflowMessages(String MDMObjectIdList, String MDMObjectType, Int32 workflowVersionID, String activityName, Int32 userID, String templateName, ref OperationResult operationResult)
        {
            Collection<Int32> userIDs = new Collection<int>();
            userIDs.Add(userID);
            SendWorkflowMessages(MDMObjectIdList, MDMObjectType, workflowVersionID, activityName, userIDs, templateName, ref operationResult);
        }
        public void SendWorkflowMessages(String MDMObjectIdList, String MDMObjectType, Int32 workflowVersionID, String activityName, Collection<Int32> userIDs, String templateName, ref OperationResult operationResult)
        {
            try
            {
                MDMMessageNotificationBL messageNotificationBL = new MDMMessageNotificationBL();

                messageNotificationBL.SendWorkflowMessages(MDMObjectIdList, MDMObjectType, workflowVersionID, activityName, userIDs, templateName, operationResult);
            }
            catch (Exception ex)
            {
                throw WrapException(ex);
            }
        }

        public void SendWorkflowMessages(String MDMObjectIdList, String MDMObjectType, Int32 workflowVersionID, String activityName, String userLogin, String templateName, ref OperationResult operationResult)
        {
            Collection<String> userLogins = new Collection<String>();
            userLogins.Add(userLogin);
            SendWorkflowMessages(MDMObjectIdList, MDMObjectType, workflowVersionID, activityName, userLogins, templateName, ref operationResult);
        }
        public void SendWorkflowMessages(String MDMObjectIdList, String MDMObjectType, Int32 workflowVersionID, String activityName, Collection<String> userLogins, String templateName, ref OperationResult operationResult)
        {
            try
            {
                MDMMessageNotificationBL messageNotificationBL = new MDMMessageNotificationBL();

                messageNotificationBL.SendWorkflowMessages(MDMObjectIdList, MDMObjectType, workflowVersionID, activityName, userLogins, templateName, operationResult);
            }
            catch (Exception ex)
            {
                throw WrapException(ex);
            }
        }

        public void SendWorkflowMessages(Collection<WorkflowInstance> workflowInstances, String templateName, ref OperationResult operationResult)
        {
            try
            {
                MDMMessageNotificationBL messageNotificationBL = new MDMMessageNotificationBL();

                messageNotificationBL.SendWorkflowMessages(workflowInstances, templateName, operationResult);
            }
            catch (Exception ex)
            {
                throw WrapException(ex);
            }
        }

        public void SendWorkflowMessages(String InstanceIdList, String templateName, ref OperationResult operationResult)
        {
            try
            {
                MDMMessageNotificationBL messageNotificationBL = new MDMMessageNotificationBL();

                messageNotificationBL.SendWorkflowMessages(InstanceIdList, templateName, operationResult);
            }
            catch (Exception ex)
            {
                throw WrapException(ex);
            }
        }

        public void SendWorkflowMessages(Collection<Escalation> escalationCollection, String templateName, ref OperationResult operationResult)
        {
            try
            {
                MDMMessageNotificationBL messageNotificationBL = new MDMMessageNotificationBL();

                messageNotificationBL.SendWorkflowMessages(escalationCollection, templateName, operationResult);
            }
            catch (Exception ex)
            {
                throw WrapException(ex);
            }
        }

        #region Send Mail

        /// <summary>
        /// Sends the email using template.
        /// </summary>
        /// <param name="entityIdList">Indicates the Collection of Entities</param>
        /// <param name="mailTemplateName">Indicates the Name of the mail template</param>
        /// <param name="toMDMUsers">Indicates the list of MDM User login Names[To send a Email]</param>
        /// <param name="toMailIds">Indicates the list of receiver Email addresses</param>
        /// <param name="ccMDMUsers">Indicates the list of MDM User login Names[To send as a Carbon Copy Email]</param>
        /// <param name="ccMailIds">Indicates the list of Carbon Copy Email addresses</param>
        /// <param name="workflowName">Indicates the workflow Name</param>
        /// <param name="callerContext">Indicates the Caller context details</param>
        /// <param name="consolidateAllEntitiesMessages">Indicates whether consolidate all messages or not</param>
        /// <returns>OperationResult</returns>
        public OperationResult SendMailWithWorkflowDetails(Collection<Int64> entityIdList, String mailTemplateName, Collection<String> toMDMUsers, Collection<String> toMailIds, Collection<String> ccMDMUsers, Collection<String> ccMailIds, String workflowName, CallerContext callerContext, Boolean consolidateAllEntitiesMessages)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("MessageService.SendMailWithWorkflowDetails", MDMTraceSource.General, false);

            OperationResult operationResult = null;

            try
            {
                 if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "MessageService receives 'SendMailWithWorkflowDetails' request message.", MDMTraceSource.General);

                MailNotificationBL mailNotificationBL = new MailNotificationBL();

                operationResult = mailNotificationBL.SendMailWithWorkflowDetails(entityIdList, mailTemplateName, toMDMUsers, toMailIds, ccMDMUsers, ccMailIds, workflowName, callerContext, consolidateAllEntitiesMessages);

                 if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "MessageService sends 'SendMailWithWorkflowDetails' response message.", MDMTraceSource.General);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("MessageService.SendMailWithWorkflowDetails", MDMTraceSource.General);

            return operationResult;
        }

        /// <summary>
        /// Sends the email using template.
        /// </summary>
        /// <param name="entityIdList">Indicates the Collection of Entities</param>
        /// <param name="emailContext">Indicates the email context details</param>
        /// <param name="workflowName">Indicates the workflow Name</param>
        /// <param name="callerContext">Indicates the Caller context details</param>
        /// <param name="consolidateAllEntitiesMessages">Indicates whether consolidate all messages or not</param>
        /// <returns>OperationResult</returns>
        public OperationResult SendMailWithWorkflowDetailsUsingMailContext(Collection<Int64> entityIdList, EmailContext emailContext,
                                                           String workflowName, CallerContext callerContext, Boolean consolidateAllEntitiesMessages)
        {
            return MakeBusinessLogicCall<MailNotificationBL, OperationResult>("SendMailWithWorkflowDetailsUsingMailContext",
                                    businessLogic => businessLogic.SendMailWithWorkflowDetails(entityIdList, emailContext, workflowName, 
                                                                                               callerContext, consolidateAllEntitiesMessages));
        }

        /// <summary>
        /// Send the mail using template.
        /// </summary>        
        /// <param name="emailContext">Indicates the email context details</param>
        /// <param name="callerContext">Indicates the Caller context details</param>
        /// <returns>OperationResult</returns>
        public OperationResult SendGenericMail(EmailContext emailContext, CallerContext callerContext)
        {
            return MakeBusinessLogicCall<MailNotificationBL, OperationResult>("SendGenericMail",
                                    businessLogic => businessLogic.SendMail(emailContext, callerContext));
        }
		
        /// <summary>
        /// Send the mail using template.
        /// </summary>
        /// <param name="entityIdList">Indicates the Collection of Entities</param>
        /// <param name="mailTemplateName">Indicates the Name of the mail template</param>
        /// <param name="toMDMUsers">Indicates the list of MDM User login Names[To send a Email]</param>
        /// <param name="toMailIds">Indicates the list of receiver Email addresses</param>
        /// <param name="ccMDMUsers">Indicates the list of MDM User login Names[To send as a Carbon Copy Email]</param>
        /// <param name="ccMailIds">Indicates the list of Carbon Copy Email addresses</param>
        /// <param name="callerContext">Indicates the Caller context details</param>
        /// <param name="consolidateAllEntitiesMessages">Indicates whether consolidate all messages or not</param>
        /// <returns>OperationResult</returns>
        public OperationResult SendMail(Collection<Int64> entityIdList, String mailTemplateName, Collection<String> toMDMUsers, Collection<String> toMailIds, Collection<String> ccMDMUsers, Collection<String> ccMailIds, CallerContext callerContext, Boolean consolidateAllEntitiesMessages)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("MessageService.SendMail", MDMTraceSource.General, false);

            OperationResult operationResult = null;

            try
            {
                 if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "MessageService receives 'SendMail' request message.", MDMTraceSource.General);

                MailNotificationBL mailNotificationBL = new MailNotificationBL();
                operationResult = mailNotificationBL.SendMail(entityIdList, mailTemplateName, toMDMUsers, toMailIds, ccMDMUsers, ccMailIds, callerContext, consolidateAllEntitiesMessages);

                 if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "MessageService sends 'SendMail' response message.", MDMTraceSource.General);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("MessageService.SendMail", MDMTraceSource.General);

            return operationResult;
        }

        /// <summary>
        /// Send the mail using template.
        /// </summary>
        /// <param name="entityIdList">Indicates the Collection of Entities</param>
        /// <param name="emailContext">Indicates the email context details</param>
        /// <param name="callerContext">Indicates the Caller context details</param>
        /// <param name="consolidateAllEntitiesMessages">Indicates whether consolidate all messages or not</param>
        /// <returns>OperationResult</returns>
        public OperationResult SendMailWithContext(Collection<Int64> entityIdList, EmailContext emailContext, CallerContext callerContext, Boolean consolidateAllEntitiesMessages)
        {
                return MakeBusinessLogicCall<MailNotificationBL, OperationResult>("SendMailWithContext",
                                    businessLogic => businessLogic.SendMail(entityIdList, emailContext, callerContext, consolidateAllEntitiesMessages));
        }

        /// <summary>
        /// Send the mail using template with workflow details as per the BR context
        /// </summary>
        /// <param name="workflowBusinessRuleContext">Indicates the workflow business rule context details</param>                
        /// <param name="mailTemplateName">Indicates the Name of the mail template</param>
        /// <param name="assignedUserOfActivityInContext">Indicates the assigned user of activity</param>
        /// <param name="toMDMUsers">Indicates the list of MDM User login Names[To send a Email]</param>
        /// <param name="toMailIds">Indicates the list of receiver Email addresses</param>
        /// <param name="ccMDMUsers">Indicates the list of MDM User login Names[To send as a Carbon Copy Email]</param>
        /// <param name="ccMailIds">Indicates the list of Carbon Copy Email addresses</param>
        /// <param name="callerContext">Indicates the Caller context details</param>
        /// <param name="consolidateAllEntitiesMessages">Indicates whether consolidate all messages or not</param>
        /// <returns></returns>
        public OperationResult SendMailWithWorkflowDetailsUsingBRContext(WorkflowBusinessRuleContext workflowBusinessRuleContext, String mailTemplateName, 
                                                                         SecurityUser assignedUserOfActivityInContext, Collection<String> toMDMUsers, 
                                                                         Collection<String> toMailIds, Collection<String> ccMDMUsers, Collection<String> ccMailIds, 
                                                                         CallerContext callerContext, Boolean consolidateAllEntitiesMessages)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("MessageService.SendMailWithWorkflowDetails", MDMTraceSource.General, false);

            OperationResult operationResult = null;

            try
            {
                 if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "MessageService receives 'SendMailWithWorkflowDetails' request message.", MDMTraceSource.General);

                MailNotificationBL mailNotificationBL = new MailNotificationBL();
                operationResult = mailNotificationBL.SendMailWithWorkflowDetails(workflowBusinessRuleContext, mailTemplateName, assignedUserOfActivityInContext, toMDMUsers, toMailIds, ccMDMUsers, ccMailIds, callerContext, consolidateAllEntitiesMessages);

                 if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "MessageService sends 'SendMailWithWorkflowDetails' response message.", MDMTraceSource.General);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("MessageService.SendMailWithWorkflowDetails", MDMTraceSource.General);

            return operationResult;
        }

        /// <summary>
        /// Send the mail using template with workflow details as per the BR context
        /// </summary>
        /// <param name="workflowBusinessRuleContext">Indicates the workflow business rule context details</param>        
        /// <param name="assignedUserOfActivityInContext">Indicates the assigned user of activity</param>
        /// <param name="emailContext">Indicates the email context details</param>
        /// <param name="callerContext">Indicates the Caller context details</param>
        /// <param name="consolidateAllEntitiesMessages">Indicates whether consolidate all messages or not</param>
        /// <returns></returns>
        public OperationResult SendMailWithWorkflowDetailsUsingBRContextAndMailContext(WorkflowBusinessRuleContext workflowBusinessRuleContext, 
                                                                         SecurityUser assignedUserOfActivityInContext, EmailContext emailContext, 
                                                                         CallerContext callerContext, Boolean consolidateAllEntitiesMessages)
        {
            return MakeBusinessLogicCall<MailNotificationBL, OperationResult>("SendMailWithWorkflowDetailsUsingBRContextAndMailContext",
                                    businessLogic => businessLogic.SendMailWithWorkflowDetails(workflowBusinessRuleContext, assignedUserOfActivityInContext, emailContext,
                                                                                 callerContext, consolidateAllEntitiesMessages));
        }

        #endregion

        /// <summary>
        /// Makes calls of Data Business logic. Also emits traces when necessary.
        /// </summary>
        /// <typeparam name="TResult">The type of the result of method of business logic</typeparam>
        /// <typeparam name="TBusinessLogic">Type of business logic</typeparam>
        /// <param name="methodName">Name of the method for tracing</param>
        /// <param name="call">The call delegate to be executed against business logic instance</param>
        /// <returns>The value returned by business logic or default</returns>
        private TResult MakeBusinessLogicCall<TBusinessLogic, TResult>(String methodName, Func<TBusinessLogic, TResult> call) 
            where TBusinessLogic : BusinessLogicBase, new()
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("MessageService." + methodName, false);

            TResult operationResult;

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "MessageService receives" + methodName + " request message.");

                operationResult = call(new TBusinessLogic());

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "MessageService receives" + methodName + " response message.");
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("MessageService." + methodName);

            return operationResult;
        }
    }
}
