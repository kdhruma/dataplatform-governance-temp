using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace MDM.NotificationManager.Business
{
    using AdminManager.Business;
    using BusinessObjects;
    using BusinessObjects.Workflow;
    using Core;
    using Core.Exceptions;
    using Core.Extensions;
    using Data;
    using EntityManager.Business;
    using ExceptionManager;
    using Interfaces;
    using LookupManager.Business;
    using MDM.BusinessObjects.Jobs;
    using MDM.Workflow.PersistenceManager.Business;
    using MessageManager.Business;
    using Utility;

    /// <summary>
    /// Specifies the business operations for Mail Notifications
    /// </summary>
    public class MailNotificationBL : BusinessLogicBase
    {
        #region Fields

        Dictionary<Int32, Lookup> _attributeLookupDataMap = new Dictionary<Int32, Lookup>();

        /// <summary>
        /// Field denoting locale message.
        /// </summary>
        LocaleMessageBL _localeMessageBL = null;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Default Constructor 
        /// </summary>
        public MailNotificationBL()
        {
            _localeMessageBL = new LocaleMessageBL();
        }

        #endregion Constructors

        #region Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        #region Send Mail Methods

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
        public OperationResult SendMail(Collection<Int64> entityIdList, String mailTemplateName, Collection<String> toMDMUsers, Collection<String> toMailIds, 
                                        Collection<String> ccMDMUsers, Collection<String> ccMailIds, CallerContext callerContext, Boolean consolidateAllEntitiesMessages = false)
        {
            OperationResult operationResult = null;

            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("MailNotificationBL.SendMail", MDMTraceSource.General, false);

            operationResult = GenerateDataAndSendMail(entityIdList, mailTemplateName, null, 
                                                      toMDMUsers, toMailIds, null, ccMDMUsers, ccMailIds, null,
                                                      String.Empty, String.Empty, String.Empty, false, callerContext, consolidateAllEntitiesMessages, false);

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("MailNotificationBL.SendMail", MDMTraceSource.General);

            return operationResult;
        }

        /// <summary>
        /// Send the mail using template.
        /// </summary>
        /// <param name="emailContext">Indicates the email context details</param>
        /// <param name="callerContext">Indicates the Caller context details</param>
        /// <returns>OperationResult</returns>
        public OperationResult SendMail(EmailContext emailContext, CallerContext callerContext)
        {
            OperationResult operationResult = null;

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("MailNotificationBL.SendMail", MDMTraceSource.General, false);

            ValidateEmailContext(emailContext);

            operationResult = SendGenericEmail(emailContext, callerContext);

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("MailNotificationBL.SendMail", MDMTraceSource.General);

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
        public OperationResult SendMail(Collection<Int64> entityIdList, EmailContext emailContext, CallerContext callerContext, 
                                        Boolean consolidateAllEntitiesMessages = false)
        {
            OperationResult operationResult = null;

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("MailNotificationBL.SendMail", MDMTraceSource.General, false);

            ValidateEmailContext(emailContext);
            
            operationResult = GenerateDataAndSendMail(entityIdList, emailContext, String.Empty, String.Empty, String.Empty, false, callerContext, consolidateAllEntitiesMessages);
            
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("MailNotificationBL.SendMail", MDMTraceSource.General);

            return operationResult;
        }

        #endregion 

        #region Send Mail with Workflow Details

        /// <summary>
        /// Send the mail using template with workflow details.
        /// </summary>
        /// <param name="entityIdList">Indicates the Collection of Entities</param>
        /// <param name="mailTemplateName">Indicates the Name of the mail template</param>
        /// <param name="toMDMUsers">Indicates the list of MDM User login Names[To send Email]</param>
        /// <param name="toMailIds">Indicates the list of receiver Email addresses</param>
        /// <param name="ccMDMUsers">Indicates the list of MDM User login Names[To send as a Carbon Copy Email]</param>
        /// <param name="ccMailIds">Indicates the list of Carbon Copy Email addresses</param>
        /// <param name="workflowName">Indicates workflow Name to consider for mail</param>
        /// <param name="callerContext">Indicates the Caller context details</param>
        /// <param name="consolidateAllEntitiesMessages">Indicates whether to consolidate messages for all entities.
        /// <para>
        /// If set as 'true', sends single mail including all entities else sends separate mail for each entity. 
        /// </para>
        /// </param>
        /// <param name="activityShortName">Indicates short name of the activity to consider for mail
        /// <para>
        /// It is optional and when not supplied considers the current running activity.
        /// </para>
        /// </param>
        /// <param name="activityLongName">Indicates long name of the activity to consider for mail
        /// <para>
        /// It is optional and when not supplied considers the current running activity.
        /// </para>
        /// </param>
        /// <returns>OperationResult</returns>
        public OperationResult SendMailWithWorkflowDetails(Collection<Int64> entityIdList, String mailTemplateName, Collection<String> toMDMUsers, Collection<String> toMailIds,
                                                           Collection<String> ccMDMUsers, Collection<String> ccMailIds, String workflowName, CallerContext callerContext, 
                                                           Boolean consolidateAllEntitiesMessages = false, String activityShortName = "", String activityLongName = "")
        {
            OperationResult operationResult = null;

            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("MailNotificationBL.SendMailWithWorkflowDetails", MDMTraceSource.General, false);

            operationResult = GenerateDataAndSendMail(entityIdList, mailTemplateName, null,
                                                      toMDMUsers, toMailIds, null,
                                                      ccMDMUsers, ccMailIds, null,
                                                      workflowName, activityShortName, activityLongName, true, callerContext, consolidateAllEntitiesMessages);

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("MailNotificationBL.SendMailWithWorkflowDetails", MDMTraceSource.General);

            return operationResult;
        }

        /// <summary>
        /// Send the mail using template with workflow details.
        /// </summary>
        /// <param name="entityIdList">Indicates the Collection of Entities</param>
        /// <param name="emailContext">Indicates the email context details</param>
        /// <param name="workflowName">Indicates workflow Name to consider for mail</param>
        /// <param name="callerContext">Indicates the Caller context details</param>
        /// <param name="consolidateAllEntitiesMessages">Indicates whether to consolidate messages for all entities.
        /// <para>
        /// If set as 'true', sends single mail including all entities else sends separate mail for each entity. 
        /// </para>
        /// </param>
        /// <param name="activityShortName">Indicates short name of the activity to consider for mail
        /// <para>
        /// It is optional and when not supplied considers the current running activity.
        /// </para>
        /// </param>
        /// <param name="activityLongName">Indicates long name of the activity to consider for mail
        /// <para>
        /// It is optional and when not supplied considers the current running activity.
        /// </para>
        /// </param>
        /// <returns>OperationResult</returns>
        public OperationResult SendMailWithWorkflowDetails(Collection<Int64> entityIdList, EmailContext emailContext,
                                                           String workflowName, CallerContext callerContext, Boolean consolidateAllEntitiesMessages = false, 
                                                           String activityShortName = "", String activityLongName = "")
        {
            OperationResult operationResult = null;

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("MailNotificationBL.SendMailWithWorkflowDetails", MDMTraceSource.General, false);

            ValidateEmailContext(emailContext);

            operationResult = GenerateDataAndSendMail(entityIdList, emailContext.TemplateName, emailContext.TemplateData,
                                                  emailContext.ToMDMUserLoginIds, emailContext.ToEmailIds, emailContext.ToMDMRoleNames,
                                                  emailContext.CCMDMUserLoginIds, emailContext.CCEmailIds, emailContext.CCMDMRoleNames,
                                                  workflowName, activityShortName, activityLongName, true, callerContext, consolidateAllEntitiesMessages,
                                                  emailContext.SendMailPerEmailId);
            
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("MailNotificationBL.SendMailWithWorkflowDetails", MDMTraceSource.General);

            return operationResult;
        }

        /// <summary>
        /// Send the mail using template with workflow details as per the BR context
        /// </summary>
        /// <param name="workflowBusinessRuleContext"></param>
        /// <param name="mailTemplateName"></param>
        /// <param name="assignedUserOfActivityInContext"></param>
        /// <param name="toMDMUsers"></param>
        /// <param name="toMailIds"></param>
        /// <param name="ccMDMUsers"></param>
        /// <param name="ccMailIds"></param>
        /// <param name="callerContext"></param>
        /// <param name="consolidateAllEntitiesMessages"></param>
        /// <returns></returns>
        public OperationResult SendMailWithWorkflowDetails(WorkflowBusinessRuleContext workflowBusinessRuleContext, String mailTemplateName, SecurityUser assignedUserOfActivityInContext, Collection<String> toMDMUsers, Collection<String> toMailIds, Collection<String> ccMDMUsers, Collection<String> ccMailIds, CallerContext callerContext, Boolean consolidateAllEntitiesMessages = false)
        {
            return SendMailWithWorkflowDetails(workflowBusinessRuleContext, assignedUserOfActivityInContext,
                                               mailTemplateName, null, 
                                               toMDMUsers, toMailIds, null,
                                               ccMDMUsers, ccMailIds, null,
                                               callerContext, consolidateAllEntitiesMessages);
        }

        /// <summary>
        /// Send the mail using template with workflow details as per the BR context
        /// </summary>
        /// <param name="workflowBusinessRuleContext"></param>
        /// <param name="assignedUserOfActivityInContext"></param>
        /// <param name="emailContext">Indicates the email context details</param>
        /// <param name="callerContext"></param>
        /// <param name="consolidateAllEntitiesMessages"></param>
        /// <returns></returns>
        public OperationResult SendMailWithWorkflowDetails(WorkflowBusinessRuleContext workflowBusinessRuleContext, SecurityUser assignedUserOfActivityInContext,
                                                           EmailContext emailContext, CallerContext callerContext, Boolean consolidateAllEntitiesMessages)
        {
            OperationResult operationResult = new OperationResult();

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("MailNotificationBL.SendMailWithWorkflowDetails", MDMTraceSource.General, false);

            ValidateEmailContext(emailContext);

            operationResult = SendMailWithWorkflowDetails(workflowBusinessRuleContext, assignedUserOfActivityInContext,
                                           emailContext.TemplateName, emailContext.TemplateData,
                                           emailContext.ToMDMUserLoginIds, emailContext.ToEmailIds, emailContext.ToMDMRoleNames,
                                           emailContext.CCMDMUserLoginIds, emailContext.CCEmailIds, emailContext.CCMDMRoleNames,
                                           callerContext, consolidateAllEntitiesMessages, emailContext.SendMailPerEmailId);
            
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("MailNotificationBL.SendMailWithWorkflowDetails", MDMTraceSource.General);

            return operationResult;
        }

        #endregion 

        #region Send Mail on Workflow Assignment

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workflowDataContext"></param>
        /// <param name="currentActivityShortName"></param>
        /// <param name="currentActivityLongName"></param>
        /// <param name="assignedUser"></param>
        /// <param name="toUsers"></param>
        /// <param name="previousWorkflowActionContext"></param>
        /// <param name="mailTemplateName"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public OperationResult SendMailOnWorkflowAssignment(WorkflowDataContext workflowDataContext, String currentActivityShortName, String currentActivityLongName, 
                                                            SecurityUser assignedUser, SecurityUserCollection toUsers, WorkflowActionContext previousWorkflowActionContext, 
                                                            String mailTemplateName, CallerContext callerContext)
        {
            OperationResult operationResult = new OperationResult();

            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("MailNotificationBL.SendMailOnWorkflowAssignment", MDMTraceSource.General, false);

            try
            {
                Collection<Int64> mdmObjectIds = null;
                Dictionary<Int64, String> entityCreatedUserDictionary = null;
                SecurityUserCollection usersInContext = new SecurityUserCollection();
                SecurityUser firstAvailableValidToUser = null;
                String workflowName = String.Empty;
                String workflowLongName = String.Empty;
                String workflowComments = String.Empty;

                //Get message template
                MessageTemplate messageTemplate = GetMessageTemplate(mailTemplateName, callerContext);

                //Get MDM Object Ids
                if (workflowDataContext != null)
                {
                    workflowName = workflowDataContext.WorkflowName;
                    workflowLongName = workflowDataContext.WorkflowLongName;
                    workflowComments = workflowDataContext.WorkflowComments;

                    if (workflowDataContext.MDMObjectCollection != null)
                    {
                        mdmObjectIds = workflowDataContext.MDMObjectCollection.GetMDMObjectIds();
                    }
                }

                if (mdmObjectIds != null && mdmObjectIds.Count > 0)
                {
                    #region Get Entity List

                    Collection<Int32> attributeIdList = null;
                    if (!String.IsNullOrWhiteSpace(messageTemplate.ReturnAttributes))
                    {
                        attributeIdList = ValueTypeHelper.SplitStringToIntCollection(messageTemplate.ReturnAttributes, ',');
                    }

                    EntityCollection entities = GetEntities(mdmObjectIds, attributeIdList, String.Empty, String.Empty, String.Empty, workflowComments, false, callerContext);

                    #endregion

                    if (entities != null && entities.Count > 0)
                    {
                        //Get Entity Created User details
                        entityCreatedUserDictionary = GetEntityCreatedUsers(mdmObjectIds, messageTemplate);

                        PopulateEntitiesWithWorkflowDetails(entities, workflowDataContext, currentActivityShortName, currentActivityLongName, assignedUser, previousWorkflowActionContext, callerContext);

                        //Prepare 'ToUsers' Mail Ids
                        Collection<String> toMailIds = new Collection<String>();
                        foreach (SecurityUser user in toUsers)
                        {
                            if (firstAvailableValidToUser == null)
                            {
                                firstAvailableValidToUser = user;
                            }

                            if (!String.IsNullOrWhiteSpace(user.Smtp))
                            {
                                toMailIds.Add(user.Smtp);
                            }
                        }

                        //Get users in context
                        usersInContext = GetAllUsersInContext(entities, entityCreatedUserDictionary, null, null);

                        PopulateDataAndSendMail(entities, messageTemplate, null, toMailIds, null, entityCreatedUserDictionary, 
                                                firstAvailableValidToUser, usersInContext, operationResult, false, false, callerContext);

                        //Set operation result status to successful
                        operationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                    }
                }
            }
            catch (MDMOperationException ex)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, ex.Message, MDMTraceSource.General);
                operationResult.AddOperationResult(ex.MessageCode, ex.Message, OperationResultType.Error);
            }
            catch (Exception ex)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, ex.Message, MDMTraceSource.General);
                operationResult.AddOperationResult("111995", ex.Message, OperationResultType.Error);
                LogException(ex);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("MailNotificationBL.SendMailOnWorkflowAssignment", MDMTraceSource.General);
            }

            return operationResult;
        }

        #endregion 

        #region Send Mail when forgot the password

        /// <summary>
        /// Send email with URL to reset password using predefined template
        /// </summary>
        /// <param name="passwordResetUrl">Indicates URL for password reset to be sent in email</param>
        /// <param name="toMailId">Indicates the receiver Email address</param>
        /// <param name="callerContext">Indicates the Caller context details</param>
        /// <returns>OperationResult</returns>
        public OperationResult SendMailOnPasswordForgotRequest(String passwordResetUrl, String currentTime, String validity, String toMailId, CallerContext callerContext)
        {
            OperationResult operationResult = new OperationResult();

            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("MailNotificationBL.SendMailOnPasswordForgotRequest", MDMTraceSource.General, false);

            String mailTemplateName = "PasswordRequestMessageTemplate";
            MessageTemplate messageTemplate = null;
            String messageSubject = String.Empty;
            String messageBody = String.Empty;

            try
            {
                messageTemplate = GetMessageTemplate(mailTemplateName, callerContext);
                messageSubject = messageTemplate.MessageSubject;
                messageBody = messageTemplate.MessageBody;

                messageBody = messageBody.Replace("##PasswordUrl##", passwordResetUrl);
                messageBody = messageBody.Replace("##WCFServerTime##", currentTime);
                messageBody = messageBody.Replace("##ValidityTime##", validity);

                SendMail(callerContext, new Collection<String>() { toMailId }, null, messageSubject, messageBody);
            }
            catch (Exception ex)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, ex.Message, MDMTraceSource.General);
                operationResult.Errors.Add(new Error(String.Empty, ex.Message));
                LogException(ex);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("MailNotificationBL.SendMailOnPasswordForgotRequest", MDMTraceSource.General);
            }

            return operationResult;
        }

        /// <summary>
        /// Send email with user name to recover user name using predefined template 
        /// </summary>
        /// <param name="loginId">Indicates the login Id to be sent in email</param>
        /// <param name="toMailId">Indicates the receiver Email address</param>
        /// <param name="callerContext">Indicates the Caller context details</param>
        /// <returns>OperationResult</returns>
        public OperationResult SendMailOnLoginForgotRequest(String loginId, String resetPasswordUrl, String toMailId, CallerContext callerContext)
        {
            OperationResult operationResult = new OperationResult();

            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("MailNotificationBL.SendMailOnLoginForgotRequest", MDMTraceSource.General, false);

            String mailTemplateName = "LoginRequestMessageTemplate";
            MessageTemplate messageTemplate = null;
            String messageSubject = String.Empty;
            String messageBody = String.Empty;

            try
            {
                messageTemplate = GetMessageTemplate(mailTemplateName, callerContext);
                messageSubject = messageTemplate.MessageSubject;
                messageBody = messageTemplate.MessageBody;

                messageBody = messageBody.Replace("##LoginId##", loginId);
                messageBody = messageBody.Replace("##ResetUrl##", resetPasswordUrl);

                SendMail(callerContext, new Collection<String>() { toMailId }, null, messageSubject, messageBody);
            }
            catch (Exception ex)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, ex.Message, MDMTraceSource.General);
                operationResult.Errors.Add(new Error(String.Empty, ex.Message));
                LogException(ex);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("MailNotificationBL.SendMailOnLoginForgotRequest", MDMTraceSource.General);
            }

            return operationResult;
        }

        #endregion 

        #region Send Mail during jobs

        /// <summary>
        /// Send email for lookup job process using predefined template 
        /// </summary>
        /// <param name="mailTemplateName">Indicates the predefined message template</param>
        /// <param name="job">Indicates the job details</param>        
        /// <param name="toMailIds">Indicates the to mail address</param>
        /// <param name="callerContext">Indicates the Caller context details</param>
        /// <returns>Return as operationResult</returns>
        public OperationResult SendMailForJob(String mailTemplateName, Job job, Collection<String> toMailIds, CallerContext callerContext, ExportJobStatus exportJobStatus = ExportJobStatus.Unknown)
        {
            OperationResult operationResult = new OperationResult();
            DurationHelper overallDurationHelper = new DurationHelper(DateTime.Now);
            try
            {
                MDMTraceHelper.InitializeTraceSource();
                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StartTraceActivity("MailNotificationBL.SendMailForJob", MDMTraceSource.LookupExport, false);
                }

                #region Initialization

                MessageTemplate messageTemplate = null;
                String messageSubject = String.Empty;
                String messageBody = String.Empty;
                LocaleMessage localeMessage = new LocaleMessage();
                String jobDetailsPageURL = String.Empty;
                String webServer = String.Empty;
                String profileType = String.Empty;

                #endregion

                if (job == null)
                {
                    localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "112262", false, callerContext);
                    throw new MDMOperationException("112262", localeMessage.Message, "MDM.NotificationManager.Business.MailNotificationBL", String.Empty, "SendMailForJob");
                }

                if (job.JobData == null)
                {
                    localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "113227", false, callerContext);
                    throw new MDMOperationException("113227", localeMessage.Message, "MDM.NotificationManager.Business.MailNotificationBL", String.Empty, "SendMailForJob");
                }

                if (String.IsNullOrWhiteSpace(mailTemplateName))
                {
                    localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "113226", false, callerContext);
                    throw new MDMOperationException("113226", localeMessage.Message, "MDM.NotificationManager.Business.MailNotificationBL", String.Empty, "SendMailForJob");
                }

                if (toMailIds == null || toMailIds.Count < 1)
                {
                    localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111967", false, callerContext);
                    throw new MDMOperationException("111967", localeMessage.Message, "MDM.NotificationManager.Business.MailNotificationBL", String.Empty, "SendMailForJob");
                }

                if (callerContext == null)
                {
                    localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111846", false, callerContext);
                    throw new MDMOperationException("111846", localeMessage.Message, "MDM.NotificationManager.Business.MailNotificationBL", String.Empty, "SendMailForJob");
                }

                #region Initialization

                OperationResult jobDataOperationResult = job.JobData.OperationResult;

                ErrorCollection errorCollection = jobDataOperationResult.Errors;
                String errors = String.Empty;
                if (errorCollection != null && errorCollection.Count > 0)
                {
                    foreach (Error error in errorCollection)
                    {
                        errors += (errors == "") ? error.ErrorMessage : ", " + error.ErrorMessage;
                    }
                }

                profileType = jobDataOperationResult.ExtendedProperties.Contains("ProfileType") ? jobDataOperationResult.ExtendedProperties["ProfileType"].ToString() : String.Empty;
                webServer = GetWebServerDetail();

                #endregion

                #region Prepare message body

                messageTemplate = GetMessageTemplate(mailTemplateName, callerContext);
                messageSubject = messageTemplate.MessageSubject;
                messageBody = messageTemplate.MessageBody;

                if (!String.IsNullOrWhiteSpace(webServer) && !String.IsNullOrWhiteSpace(profileType))
                {
                    jobDetailsPageURL = String.Format("<a href='http://{0}/Main/views/view.aspx?view=LookUpExportJobDetail&jobId={1}&Type={2}'>Click here</a>", webServer, job.Id.ToString(), profileType);
                }

                messageSubject = messageSubject.Replace("##JobId##", job.Id.ToString());
                messageSubject = messageSubject.Replace("##UserName##", job.CreatedUser);

                messageBody = messageBody.Replace("##JobId##", job.Id.ToString());
                messageBody = messageBody.Replace("##JobName##", job.Name);
                messageBody = messageBody.Replace("##ProfileName##", job.ProfileName);
                messageBody = messageBody.Replace("##ProfileLongName##", jobDataOperationResult.ExtendedProperties.Contains("ProfileLongName") ? jobDataOperationResult.ExtendedProperties["ProfileLongName"].ToString() : String.Empty);
                messageBody = messageBody.Replace("##ProfileType##", job.ProfileType);
                messageBody = messageBody.Replace("##OperationResultStatus##", jobDataOperationResult.OperationResultStatus.ToString());
                messageBody = messageBody.Replace("##Error Details here##", errors);
                messageBody = messageBody.Replace("##JobBeginTime##", job.JobData.ExecutionStatus.StartTime);
                messageBody = messageBody.Replace("##JobCompleteTime##", job.JobData.ExecutionStatus.EndTime);
                messageBody = messageBody.Replace("##Username##", job.CreatedUser);
                messageBody = messageBody.Replace("##JobDetailsPageURL##", jobDetailsPageURL);

                #endregion

                SendMail(callerContext, toMailIds, null, messageSubject, messageBody);
                operationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
            }
            catch (Exception ex)
            {
                operationResult.OperationResultStatus = OperationResultStatusEnum.Failed;
                
                String errorMessage = String.Format("Error occurred while sending mail for job id: {0} with export job status: {1}. Error details: {2}", job.Id, exportJobStatus, ex.Message);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, errorMessage, MDMTraceSource.LookupExport);

                String localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "113931", new object[] { job.Id, exportJobStatus, ex.Message }, false, callerContext).Message;
                operationResult.Errors.Add(new Error(String.Empty, localeMessage));

                LogException(ex);
            }
            finally
            {
                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Overall time to send mail ", overallDurationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.LookupExport);
                    MDMTraceHelper.StopTraceActivity("MailNotificationBL.SendMailForJob", MDMTraceSource.LookupExport);
                }
            }

            return operationResult;
        }

        #endregion 

        #region Send Mail for Translation Export

        /// <summary>
        ///  Send email for translation export process using predefined template 
        /// </summary>
        /// <param name="messageTemplateName">Indicates predefined name for message template </param>
        /// <param name="subscriberType">Indicates type of subscriber</param>
        /// <param name="integrationData">Indicates details about translation export</param>
        /// <param name="exportJobStatus">Indicates status of the translation export mail (i.e - complete and failure)</param>
        /// <param name="errorMessage">Indicates error message in case of failure</param>
        /// <param name="toMailIds">Indicates list of to email ids</param>
        /// <param name="callerContext">Indicates name of the application and module performing action</param>
        public void SendMailForTranslationExport(String messageTemplateName, String subscriberType, String integrationData, String SourceLocale, String TargetLocale, ExportJobStatus exportJobStatus, String errorMessage, Collection<String> toMailIds, CallerContext callerContext)
        {
            String methodName = "SendMailForTranslationExport";
            DurationHelper overallDurationHelper = new DurationHelper(DateTime.Now);
            try
            {
                MDMTraceHelper.InitializeTraceSource();
                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StartTraceActivity("MailNotificationBL.SendMailForTranslationExport", MDMTraceSource.TMSConnector, false);
                }

                #region Initialization

                MessageTemplate messageTemplate = null;
                String messageSubject = String.Empty;
                String messageBody = String.Empty;
                LocaleMessage localeMessage = new LocaleMessage();
                String profileType = String.Empty;
                String fileName = integrationData.ToString();
                String[] values = fileName.Split('_');
                String objectType = values[1].ToString();
                //String SourceLocale = values[4] + "_" + values[5];
                //String targetLocale = values[7] + "_" + values[8];

                #endregion

                if (String.IsNullOrWhiteSpace(messageTemplateName))
                {
                    localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "113226", false, callerContext);
                    throw new MDMOperationException("113226", localeMessage.Message, "MDM.NotificationManager.Business.MailNotificationBL", String.Empty, methodName);
                }

                if (toMailIds == null || toMailIds.Count < 1)
                {
                    localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111967", false, callerContext);
                    throw new MDMOperationException("111967", localeMessage.Message, "MDM.NotificationManager.Business.MailNotificationBL", String.Empty, methodName);
                }

                if (callerContext == null)
                {
                    localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111846", false, callerContext);
                    throw new MDMOperationException("111846", localeMessage.Message, "MDM.NotificationManager.Business.MailNotificationBL", String.Empty, methodName);
                }

                #region Initialization

                #endregion

                #region Prepaire message body

                messageTemplate = GetMessageTemplate(messageTemplateName, callerContext);
                messageSubject = messageTemplate.MessageSubject;
                messageBody = messageTemplate.MessageBody;

                messageBody = messageBody.Replace("##SourceLocale##", SourceLocale);
                messageBody = messageBody.Replace("##TargetLocale##", TargetLocale);
                messageBody = messageBody.Replace("##ObjectType##", objectType);
                messageBody = messageBody.Replace("##SubscriberType##", subscriberType);

                //If exporttype is Failure then add two additional property i.e - JobStatus and ErrorMessage
                if (exportJobStatus == ExportJobStatus.Failure)
                {
                    messageBody = messageBody.Replace("##Status##", "Completed With Errors");
                    messageBody = messageBody.Replace("##ErrorMessage##", errorMessage);
                }
                else
                {
                    messageBody = messageBody.Replace("##FileName##", fileName);
                }

                #endregion

                SendMail(callerContext, toMailIds, null, messageSubject, messageBody);
            }
            catch (Exception ex)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, ex.Message, MDMTraceSource.TMSConnector);
                LogException(ex);
            }
            finally
            {
                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Overall time to send mail ", overallDurationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.TMSConnector);
                    MDMTraceHelper.StopTraceActivity("MailNotificationBL.SendMailForJob", MDMTraceSource.TMSConnector);
                }
            }
        }

        #endregion 

        #region Send Mail with Workflow Escalations

        /// <summary>
        /// Send mail for workflow escalation details based on email context
        /// </summary>
        /// <param name="escalationData">Indicates the workflow escalation data</param>
        /// <param name="emailContext">Indicates the email context</param>
        /// <param name="callerContext">Indicates the caller context which contains the MDM application and MMD module</param>
        /// <returns>Returns the operation result object based on the result </returns>
        public OperationResult SendMailWithWorkflowEscalationDetails(WorkflowEscalationData escalationData, EmailContext emailContext, CallerContext callerContext)
        {
            OperationResult result = new OperationResult();

            ValidateEmailContext(emailContext);

            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null", "MDM.NotificationManager.Business.MailNotificationBL", String.Empty, "SendMailWithWorkflowEscalationDetails");
            }
            else if (escalationData != null)
            {
                result = this.GenerateDataAndSendMail(new Collection<Int64>() { escalationData.EntityId }, emailContext, escalationData.WorkflowName, escalationData.ActivityName,
                        escalationData.ActivityLongName, true, callerContext, false, escalationData);
            }

            result.RefreshOperationResultStatus();
            return result;
        }

        #endregion 

        #endregion Public Methods

        #region Private Methods

        #region Send Mail Methods

        private OperationResult SendMailWithWorkflowDetails(WorkflowBusinessRuleContext workflowBusinessRuleContext, SecurityUser assignedUserOfActivityInContext,
                                                            String mailTemplateName, Dictionary<String, String> templateData,                                                            
                                                            Collection<String> toMDMUsers, Collection<String> toMailIds, Collection<String> toMDMRoleNames,
                                                            Collection<String> ccMDMUsers, Collection<String> ccMailIds, Collection<String> ccMDMRoleNames,
                                                            CallerContext callerContext, Boolean consolidateAllEntitiesMessages = false, Boolean sendMailPerEmailId = false)
        {
            OperationResult operationResult = new OperationResult();

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("MailNotificationBL.SendMailWithWorkflowDetails", MDMTraceSource.General, false);

            try
            {
                Collection<Int64> mdmObjectIds = null;
                Dictionary<Int64, String> entityCreatedUserDictionary = null;
                SecurityUserCollection usersInContext = new SecurityUserCollection();
                SecurityUser firstAvailableValidToUser = null;
                String currentActivityShortName = String.Empty;
                String currentActivityLongName = String.Empty;
                WorkflowDataContext workflowDataContext = null;
                WorkflowActionContext previousActivityActionContext = null;
                String workflowComments = String.Empty;
                MessageTemplate messageTemplate = null;
                EntityCollection entities = null;
                String errorMessage = String.Empty;

                if (workflowBusinessRuleContext != null)
                {
                    workflowDataContext = workflowBusinessRuleContext.WorkflowDataContext;
                    previousActivityActionContext = workflowBusinessRuleContext.PreviousActivityActionContext;
                    currentActivityShortName = workflowBusinessRuleContext.CurrentActivityName;
                    currentActivityLongName = workflowBusinessRuleContext.CurrentActivityLongName;

                    if (workflowDataContext != null)
                    {
                        workflowComments = workflowDataContext.WorkflowComments;

                        if (workflowDataContext.MDMObjectCollection != null && workflowDataContext.MDMObjectCollection.Count > 0)
                        {
                            mdmObjectIds = workflowDataContext.MDMObjectCollection.GetMDMObjectIds();
                        }
                    }
                }

                if (toMDMUsers == null)
                {
                    toMDMUsers = new Collection<String>();
                }

                //Add assigned user to toMDMUsers if not available as mail has to be sent to assigned user also.
                if (assignedUserOfActivityInContext != null && !toMDMUsers.Contains(assignedUserOfActivityInContext.SecurityUserLogin))
                {
                    toMDMUsers.Add(assignedUserOfActivityInContext.SecurityUserLogin);
                }

                //To Mail Address
                Collection<String> filteredToMailIds = ValidateAndFilterMailAddresses(toMailIds, operationResult, callerContext, "toMailIds");

                //Convert toMDMRoleNames => toMailIds
                GetUserEmailIdsInRole(toMDMRoleNames, filteredToMailIds, callerContext);

                //Mandatory Input validations...
                ValidateParameters(mdmObjectIds, mailTemplateName, toMDMUsers, toMailIds, callerContext);

                //Get message template
                messageTemplate = GetMessageTemplate(mailTemplateName, callerContext);

                #region Get Entity List

                Collection<Int32> attributeIdList = null;
                if (!String.IsNullOrWhiteSpace(messageTemplate.ReturnAttributes))
                {
                    attributeIdList = ValueTypeHelper.SplitStringToIntCollection(messageTemplate.ReturnAttributes, ',');
                }

                entities = GetEntities(mdmObjectIds, attributeIdList, String.Empty, String.Empty, String.Empty, workflowComments, false, callerContext);

                #endregion

                if (entities != null && entities.Count > 0)
                {
                    //Get Entity Created User details
                    entityCreatedUserDictionary = GetEntityCreatedUsers(mdmObjectIds, messageTemplate);

                    PopulateEntitiesWithWorkflowDetails(entities, workflowDataContext, currentActivityShortName, currentActivityLongName,
                                                        assignedUserOfActivityInContext, previousActivityActionContext, callerContext);

                    //Get all security users details required for this mail template
                    usersInContext = GetAllUsersInContext(entities, entityCreatedUserDictionary, toMDMUsers, ccMDMUsers);

                    #region Prepare TO and CC Addresses

                    //To Mail Address                    
                    firstAvailableValidToUser = PrepareMDMUsersMailAddresses(toMDMUsers, filteredToMailIds, usersInContext, "toMailIds");

                    if (filteredToMailIds.Count < 1) //If there is no 'TO' mail address return it.
                    {
                        errorMessage = this.GetSystemLocaleMessage("111993", callerContext).Message;
                        throw new MDMOperationException("111993", errorMessage, "MailNotificationBL", String.Empty, "SendMail");
                    }

                    //CC Mail Address
                    Collection<String> filteredCcMailIds = ValidateAndFilterMailAddresses(ccMailIds, operationResult, callerContext, "ccMailIds");

                    //Convert ccMDMRoleNames to ccMailIds
                    GetUserEmailIdsInRole(ccMDMRoleNames, filteredCcMailIds, callerContext);

                    PrepareMDMUsersMailAddresses(ccMDMUsers, filteredCcMailIds, usersInContext, "ccMailIds");

                    #endregion

                    PopulateDataAndSendMail(entities, messageTemplate, templateData, filteredToMailIds, filteredCcMailIds, entityCreatedUserDictionary,
                                            firstAvailableValidToUser, usersInContext, operationResult, consolidateAllEntitiesMessages, sendMailPerEmailId, callerContext);

                    //Set operation result status to successful
                    operationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                }
                else
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Failed to get Entity details for requested Entity Ids.", MDMTraceSource.General);

                    errorMessage = this.GetSystemLocaleMessage("111996", callerContext).Message;
                    throw new MDMOperationException("111996", errorMessage, "MailNotificationManager", String.Empty, "SendMail");   //Failed to get Entity details for requested Entity Ids.
                }
            }
            catch (MDMOperationException ex)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, ex.Message, MDMTraceSource.General);
                operationResult.AddOperationResult(ex.MessageCode, ex.Message, OperationResultType.Error);
            }
            catch (Exception ex)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, ex.Message, MDMTraceSource.General);
                operationResult.AddOperationResult("111995", ex.Message, OperationResultType.Error);
                LogException(ex);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("MailNotificationBL.SendMailWithWorkflowDetails", MDMTraceSource.General);
            }

            return operationResult;
        }

        private OperationResult GenerateDataAndSendMail(Collection<Int64> entityIdList, EmailContext emailContext, String workflowName, String activityShortName, String activityLongName, 
                                                        Boolean loadWorkflowDetails, CallerContext callerContext, Boolean consolidateAllEntitiesMessages, WorkflowEscalationData wfEscalationData = null)
        {
            return GenerateDataAndSendMail(entityIdList, emailContext.TemplateName, emailContext.TemplateData,
                                           emailContext.ToMDMUserLoginIds, emailContext.ToEmailIds, emailContext.ToMDMRoleNames,
                                           emailContext.CCMDMUserLoginIds, emailContext.CCEmailIds, emailContext.CCMDMRoleNames,
                                           workflowName, activityShortName, activityLongName, loadWorkflowDetails,
                                           callerContext, consolidateAllEntitiesMessages, emailContext.SendMailPerEmailId, wfEscalationData);
 
        }

        private OperationResult SendGenericEmail(EmailContext emailContext, CallerContext callerContext)
        {
            String errorMessage = String.Empty;
            MessageTemplate messageTemplate = null;
            SecurityUser firstAvailableValidToUser = null;
            OperationResult operationResult = new OperationResult();
            SecurityUserCollection usersInContext = new SecurityUserCollection();

            try
            {
                Collection<String> filteredToMailIds = GetFilteredToEmailIds(emailContext.TemplateName, emailContext.ToMDMUserLoginIds, emailContext.ToEmailIds, 
                                                                          emailContext.ToMDMRoleNames, callerContext, operationResult);

                firstAvailableValidToUser = PrepareMDMUsersMailAddresses(emailContext.ToMDMUserLoginIds, filteredToMailIds, usersInContext, "toMailIds");

                if (filteredToMailIds.Count < 1) //If there is no 'TO' mail address return it.
                {
                    errorMessage = this.GetSystemLocaleMessage("111993", callerContext).Message;
                    throw new MDMOperationException("111993", errorMessage, "MailNotificationBL", String.Empty, "SendMail");
                }

                messageTemplate = GetMessageTemplate(emailContext.TemplateName, callerContext);
                
                Collection<String> filteredCcMailIds = GetFilteredCCEmailIds(emailContext.TemplateName, emailContext.CCMDMUserLoginIds, emailContext.CCEmailIds, 
                                                                          emailContext.CCMDMRoleNames, callerContext, operationResult, usersInContext);

                // construct the email message and send email
                String messageSubject = messageTemplate.MessageSubject;
                String messageBody = messageTemplate.MessageBody;
                String contentTemplate = messageTemplate.MDMObjectContentTemplate;

                messageSubject = PopulateData(messageSubject, emailContext.TemplateData, null, null, null, firstAvailableValidToUser, usersInContext, callerContext);
                messageBody = PopulateData(messageBody, emailContext.TemplateData, null, null, null, firstAvailableValidToUser, usersInContext, callerContext);
                contentTemplate = PopulateData(contentTemplate, emailContext.TemplateData, null, null, null, firstAvailableValidToUser, usersInContext, callerContext);

                messageBody = messageBody.Replace("##MDMObjectContentTemplate##", contentTemplate);

                messageSubject = Regex.Replace(messageSubject, "##[0-9a-zA-Z]+##", "");
                messageBody = Regex.Replace(messageBody, "##[0-9a-zA-Z]+##", "");

                SendMail(callerContext, filteredToMailIds, filteredCcMailIds, messageSubject, messageBody, sendMailPerEmailId: emailContext.SendMailPerEmailId);
                
                //Set operation result status to successful
                operationResult.OperationResultStatus = OperationResultStatusEnum.Successful;                
            }
            catch (MDMOperationException ex)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, ex.Message, MDMTraceSource.General);
                operationResult.AddOperationResult(ex.MessageCode, ex.Message, OperationResultType.Error);
            }
            catch (Exception ex)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, ex.Message, MDMTraceSource.General);
                operationResult.AddOperationResult("111995", ex.Message, OperationResultType.Error);
                LogException(ex);
            }

            return operationResult;
        }

        private OperationResult GenerateDataAndSendMail(Collection<Int64> entityIdList, String mailTemplateName, Dictionary<String, String> templateData, 
                                                        Collection<String> toMDMUsers, Collection<String> toMailIds, Collection<String> toMDMRoleNames,
                                                        Collection<String> ccMDMUsers, Collection<String> ccMailIds, Collection<String> ccMDMRoleNames, 
                                                        String workflowName, String activityShortName, String activityLongName, Boolean loadWorkflowDetails, 
                                                        CallerContext callerContext, Boolean consolidateAllEntitiesMessages, Boolean sendMailPerEmailId = false, WorkflowEscalationData wfEscalationData = null)
        {
            EntityCollection entities = null;
            String errorMessage = String.Empty;
            MessageTemplate messageTemplate = null;
            SecurityUser firstAvailableValidToUser = null;
            Dictionary<Int64, String> entityCreatedUserDictionary = null;
            Collection<Int32> attributeIdList = null;
            OperationResult operationResult = new OperationResult();
            SecurityUserCollection usersInContext = new SecurityUserCollection();

            try
            {
                //To Mail Address
                Collection<String> filteredToMailIds = GetFilteredToEmailIds(mailTemplateName, toMDMUsers, toMailIds, toMDMRoleNames, callerContext, operationResult);

                //Get message template
                messageTemplate = GetMessageTemplate(mailTemplateName, callerContext);

                #region Get Entity List

                if (!String.IsNullOrWhiteSpace(messageTemplate.ReturnAttributes))
                {
                    attributeIdList = ValueTypeHelper.SplitStringToIntCollection(messageTemplate.ReturnAttributes, ',');
                }

                entities = GetEntities(entityIdList, attributeIdList, workflowName, activityShortName, activityLongName, String.Empty, loadWorkflowDetails, callerContext);

                #endregion

                if (entities != null && entities.Count > 0)
                {
                    //Get Entity Created User details
                    entityCreatedUserDictionary = GetEntityCreatedUsers(entityIdList, messageTemplate);

                    //Get all security users details required for this mail template
                    usersInContext = GetAllUsersInContext(entities, entityCreatedUserDictionary, toMDMUsers, ccMDMUsers);

                    #region Prepare TO and CC Addresses                    

                    firstAvailableValidToUser = PrepareMDMUsersMailAddresses(toMDMUsers, filteredToMailIds, usersInContext, "toMailIds");

                    if (filteredToMailIds.Count < 1) //If there is no 'TO' mail address return it.
                    {
                        errorMessage = this.GetSystemLocaleMessage("111993", callerContext).Message;
                        throw new MDMOperationException("111993", errorMessage, "MailNotificationBL", String.Empty, "SendMail");
                    }

                    Collection<String> filteredCcMailIds = GetFilteredCCEmailIds(mailTemplateName, ccMDMUsers, ccMailIds, ccMDMRoleNames, 
                                                                                 callerContext, operationResult, usersInContext);
                    #endregion

                    PopulateDataAndSendMail(entities, messageTemplate, templateData, filteredToMailIds, filteredCcMailIds, entityCreatedUserDictionary, 
                                            firstAvailableValidToUser, usersInContext, operationResult, consolidateAllEntitiesMessages, sendMailPerEmailId, callerContext, wfEscalationData);

                    //Set operation result status to successful
                    operationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                }
                else
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Failed to get Entity details for requested Entity Ids.", MDMTraceSource.General);

                    errorMessage = this.GetSystemLocaleMessage("111996", callerContext).Message;
                    throw new MDMOperationException("111996", errorMessage, "MailNotificationManager", String.Empty, "SendMail");   //Failed to get Entity details for requested Entity Ids.
                }
            }
            catch (MDMOperationException ex)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, ex.Message, MDMTraceSource.General);
                operationResult.AddOperationResult(ex.MessageCode, ex.Message, OperationResultType.Error);
            }
            catch (Exception ex)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, ex.Message, MDMTraceSource.General);
                operationResult.AddOperationResult("111995", ex.Message, OperationResultType.Error);
                LogException(ex);
            }

            return operationResult;
        }

        private void PopulateDataAndSendMail(EntityCollection entities, MessageTemplate messageTemplate, Dictionary<String, String> templateData, 
                                             Collection<String> toMailIds, Collection<String> ccMailIds, Dictionary<Int64, String> entityCreatedUserDictionary, 
                                             SecurityUser firstAvailableValidToUser, SecurityUserCollection usersInContext, OperationResult operationResult,
                                             Boolean consolidateAllEntitiesMessages, Boolean sendMailPerEmailId, CallerContext callerContext, WorkflowEscalationData wfEscalationData = null)
        {
            IWorkflowState workflowState = null;
            String messageSubject = String.Empty;
            String messageBody = String.Empty;
            String mdmObjectContent = String.Empty;

            messageSubject = messageTemplate.MessageSubject;
            messageBody = messageTemplate.MessageBody;

            if (messageTemplate.ConsolidateMessages || consolidateAllEntitiesMessages)
            {
                //When consolidated messages is True..
                //We need to assume that all entities will be at same workflow stage
                //Get the first entity and its workflow details
                Entity commonEntity = entities.FirstOrDefault();
                IWorkflowStateCollection iWFstates = commonEntity.GetWorkflowDetails();

                if (iWFstates != null && iWFstates.Count() > 0)
                {
                    workflowState = iWFstates.OrderByDescending(wf => ValueTypeHelper.ConvertToDateTime(wf.EventDate)).FirstOrDefault();
                }

                messageSubject = PopulateData(messageSubject, templateData, commonEntity, workflowState, entityCreatedUserDictionary, firstAvailableValidToUser, usersInContext, callerContext);
                messageBody = PopulateData(messageBody, templateData, commonEntity, workflowState, entityCreatedUserDictionary, firstAvailableValidToUser, usersInContext, callerContext);

                foreach (Entity entity in entities)
                {
                    String contentTemplate = messageTemplate.MDMObjectContentTemplate;

                    contentTemplate = PopulateData(contentTemplate, templateData, entity, workflowState, entityCreatedUserDictionary, firstAvailableValidToUser, usersInContext, callerContext);

                    mdmObjectContent = String.Concat(mdmObjectContent, contentTemplate);
                }

                messageBody = messageBody.Replace("##MDMObjectContentTemplate##", mdmObjectContent);

                messageSubject = Regex.Replace(messageSubject, "##[0-9a-zA-Z]+##", "");
                messageBody = Regex.Replace(messageBody, "##[0-9a-zA-Z]+##", "");

                SendMail(callerContext, toMailIds, ccMailIds, messageSubject, messageBody, sendMailPerEmailId: sendMailPerEmailId);
            }
            else
            {
                foreach (Entity entity in entities)
                {
                    IWorkflowStateCollection iWFstates = entity.GetWorkflowDetails();

                    String contentTemplate = messageTemplate.MDMObjectContentTemplate;

                    if (iWFstates != null && iWFstates.Count() > 0)
                    {
                        workflowState = iWFstates.OrderByDescending(wf => ValueTypeHelper.ConvertToDateTime(wf.EventDate)).FirstOrDefault();
                    }

                    messageSubject = PopulateData(messageSubject, templateData, entity, workflowState, entityCreatedUserDictionary, firstAvailableValidToUser, usersInContext, callerContext);
                    messageBody = PopulateData(messageBody, templateData, entity, workflowState, entityCreatedUserDictionary, firstAvailableValidToUser, usersInContext, callerContext);
                    contentTemplate = PopulateData(contentTemplate, templateData, entity, workflowState, entityCreatedUserDictionary, firstAvailableValidToUser, usersInContext, callerContext);

                    messageBody = messageBody.Replace("##MDMObjectContentTemplate##", contentTemplate);

                    messageSubject = Regex.Replace(messageSubject, "##[0-9a-zA-Z]+##", "");
                    messageBody = Regex.Replace(messageBody, "##[0-9a-zA-Z]+##", "");

                    SendMail(callerContext, toMailIds, ccMailIds, messageSubject, messageBody, sendMailPerEmailId: sendMailPerEmailId);
                }
            }
        }

        public void SendMail(CallerContext callerContext, Collection<String> toAddresses, Collection<String> ccAddresses, String messageSubject, String messageBody,
                              MailPriority mailPriority = MailPriority.Normal, Boolean sendMailPerEmailId = false)
        {

            MailConfigBL mailConfigBL = new MailConfigBL();
            MailConfig mailConfig = mailConfigBL.Get(callerContext);
                MailMessage message = new MailMessage();
                if (ccAddresses != null && ccAddresses.Count > 0)
                {
                    String ccAddressesString = ValueTypeHelper.JoinCollection(ccAddresses, ",");

                    message.CC.Add(ccAddressesString);
                }

                message.IsBodyHtml = true;
                message.Subject = messageSubject;
                message.Body = messageBody;
                message.Priority = mailPriority;
                
                if (sendMailPerEmailId)
                {
                    foreach (var toAdd in toAddresses)
                    {
                        message.To.Add(toAdd);
                        SendEmail(message, mailConfig);
                    } 
                }
                else
                {
                    String toAddressesString = ValueTypeHelper.JoinCollection(toAddresses, ",");
                    message.To.Add(toAddressesString);
                    SendEmail(message, mailConfig);
                }
            }

        private void SendEmail(MailMessage message, MailConfig mailConfig)
        {
            using (SmtpClient smtpClient = new SmtpClient())
            {
                if (mailConfig != null && !String.IsNullOrWhiteSpace(mailConfig.From) && !String.IsNullOrWhiteSpace(mailConfig.UserName))
                {
                message.From = new MailAddress(mailConfig.From);

                smtpClient.Host = mailConfig.Host;
                smtpClient.Port = mailConfig.Port;
                smtpClient.EnableSsl = mailConfig.EnableSSL;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(mailConfig.UserName, mailConfig.Password);
                }
                smtpClient.Send(message);
            }
        }

        #endregion

        #region Validation Methods

        private void ValidateParameters(Collection<Int64> entityIdList, String mailTemplateName,
                                        Collection<String> toMDMUsers, Collection<String> toMailIds,
                                        CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Input Parameter validations process starts...", MDMTraceSource.General);

            String errorMessage = String.Empty;

            if (entityIdList == null || entityIdList.Count < 1)
            {
                errorMessage = this.GetSystemLocaleMessage("111785", callerContext).Message;
                throw new MDMOperationException("111785", errorMessage, "MailNotificationManager", String.Empty, "SendMail");   //Entity Ids are not available
            }

            ValidateMailParameters(mailTemplateName, toMDMUsers, toMailIds, callerContext);
                        
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Input Parameter validations process completed", MDMTraceSource.General);
        }

        private void ValidateMailParameters(String mailTemplateName, Collection<String> toMDMUsers, Collection<String> toMailIds,
                                            CallerContext callerContext)
        {
            String errorMessage = String.Empty;

            if (String.IsNullOrWhiteSpace(mailTemplateName))
            {
                errorMessage = this.GetSystemLocaleMessage("111966", callerContext).Message;
                throw new MDMOperationException("111966", errorMessage, "MailNotificationManager", String.Empty, "SendMail");   //Mail Template is not provided
            }

            if ((toMDMUsers == null || toMDMUsers.Count < 1) && (toMailIds == null || toMailIds.Count < 1))
            {
                errorMessage = this.GetSystemLocaleMessage("111967", callerContext).Message;
                throw new MDMOperationException("111967", errorMessage, "MailNotificationManager", String.Empty, "SendMail");   //'To' mail addresses are not provided. Please provide either ‘toMDMUsers' or ‘'toMailIds'
            }
        }

        private Collection<String> ValidateAndFilterMailAddresses(Collection<String> emailIdLists, OperationResult operationresult, CallerContext callerContext, String addressParameterName)
        {
            //Logic:
            //If email address is valid then will add to result mail list.
            //

            Collection<String> validMailIds = new Collection<String>();
            String errorMessage = String.Empty;
            Collection<Object> parameter = null;

            if (emailIdLists != null && emailIdLists.Count > 0)
            {
                foreach (String email in emailIdLists)
                {
                    try
                    {
                        MailAddress address = new MailAddress(email);   // Validate Email address

                        validMailIds.Add(email);
                    }
                    catch (FormatException) //If there is a format exception then email address is not a valid format.
                    {
                        parameter = new Collection<Object>();
                        parameter.Add(email);
                        parameter.Add(addressParameterName);

                        errorMessage = this.GetSystemLocaleMessage("111968", callerContext).Message;

                        operationresult.AddOperationResult("111968", errorMessage, parameter, OperationResultType.Warning); //Mail Id '{0}' provided in '{1}' list is not valid and hence ignored.
                    }
                }
            }

            return validMailIds;
        }

        private void ValidateEmailContext(EmailContext emailContext)
        {
            if (emailContext == null)
            {
                String errorMessage = "Email context is not provided";
                throw new MDMOperationException("113796", errorMessage, "MailNotificationManager", String.Empty, "SendMail");
            }
        }

        #endregion Validation Methods

        #region Helper Methods for Mail Message

        private Collection<String> GetFilteredToEmailIds(String mailTemplateName, Collection<String> toMDMUserLoginIds, Collection<String> toEmailIds,
                                                      Collection<String> toMDMRoleNames, CallerContext callerContext, OperationResult operationResult)
        {
            //To Mail Address
            Collection<String> filteredToMailIds = ValidateAndFilterMailAddresses(toEmailIds, operationResult, callerContext, "toMailIds");

            //Convert toMDMRoleNames => toMailIds
            GetUserEmailIdsInRole(toMDMRoleNames, filteredToMailIds, callerContext);

            //Mandatory Input validations...
            ValidateMailParameters(mailTemplateName, toMDMUserLoginIds, filteredToMailIds, callerContext);
            
            return filteredToMailIds;
        }

        private Collection<String> GetFilteredCCEmailIds(String mailTemplateName, Collection<String> ccMDMUserLoginIds, Collection<String> ccEmailIds,
                                                      Collection<String> ccMDMRoleNames, CallerContext callerContext, OperationResult operationResult, SecurityUserCollection usersInContext)
        {
            //To Mail Address
            Collection<String> filteredCCMailIds = ValidateAndFilterMailAddresses(ccEmailIds, operationResult, callerContext, "ccMailIds");

            //Convert ccMDMRoleNames to ccMailIds
            GetUserEmailIdsInRole(ccMDMRoleNames, filteredCCMailIds, callerContext);

            PrepareMDMUsersMailAddresses(ccMDMUserLoginIds, filteredCCMailIds, usersInContext, "ccMailIds");

            return filteredCCMailIds;
        }

        private SecurityUser PrepareMDMUsersMailAddresses(Collection<String> mdmUserList, Collection<String> mailIdList, SecurityUserCollection usersInContext, String addressParameterName)
        {
            SecurityUser firstAvailableValidMDMUser = null;

            if (mdmUserList != null && mdmUserList.Count > 0)
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Starting get users list details for '{0}'...", addressParameterName), MDMTraceSource.General);

                foreach (String mdmUser in mdmUserList)
                {
                    //Get this user details
                    SecurityUser user = usersInContext.FirstOrDefault(u => u.SecurityUserLogin.ToLowerInvariant() == mdmUser.ToLowerInvariant());

                    if (user != null && !String.IsNullOrWhiteSpace(user.Smtp))
                    {
                        if (firstAvailableValidMDMUser == null)
                        {
                            firstAvailableValidMDMUser = user;
                        }

                        if (!mailIdList.Contains(user.Smtp))
                        {
                            mailIdList.Add(user.Smtp);
                        }
                    }
                }

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Done with Get User Details operations for {0}", addressParameterName), MDMTraceSource.General);
            }

            if (firstAvailableValidMDMUser == null && mailIdList.Count > 0)
            {
                //Get from mailId list
                String firstAvailableMailId = mailIdList.FirstOrDefault();
                String firstAvailableMailName = firstAvailableMailId.Remove(firstAvailableMailId.IndexOf('@'));

                firstAvailableValidMDMUser = new SecurityUser();
                firstAvailableValidMDMUser.SecurityUserLogin = firstAvailableMailName;
                firstAvailableValidMDMUser.FirstName = firstAvailableMailName;
                firstAvailableValidMDMUser.LastName = firstAvailableMailName;
            }

            return firstAvailableValidMDMUser;
        }

        private String PopulateData(String mailMessage, Dictionary<String, String> templateData, Entity entity, IWorkflowState iWorkflowState, 
                                    Dictionary<Int64, String> entityCreatedUserDictionary, SecurityUser firstAvailableValidToMDMUser, 
                                    SecurityUserCollection usersInContext, CallerContext callerContext, WorkflowEscalationData wfEscalationData = null)
        {
            if (wfEscalationData != null)
        {
                mailMessage = mailMessage.Replace("##ElapsedTime##", wfEscalationData.ElapsedTime.ToString());
                mailMessage = mailMessage.Replace("##ToUserLogin##", wfEscalationData.AssignedUserLogin);
                mailMessage = mailMessage.Replace("##UserName##", wfEscalationData.AssignedUserLogin);
            }

            if (firstAvailableValidToMDMUser != null)
            {
                mailMessage = mailMessage.Replace("##ToUserLogin##", firstAvailableValidToMDMUser.SecurityUserLogin);
                mailMessage = mailMessage.Replace("##ToUserFirstName##", firstAvailableValidToMDMUser.FirstName);
                mailMessage = mailMessage.Replace("##ToUserLastName##", firstAvailableValidToMDMUser.LastName);
                mailMessage = mailMessage.Replace("##ToUserFullName##", String.Format("{0} {1}", firstAvailableValidToMDMUser.FirstName, firstAvailableValidToMDMUser.LastName));
            }

            if (entity != null)
            {
                mailMessage = mailMessage.Replace("##CNodeId##", entity.Id.ToString());
                mailMessage = mailMessage.Replace("##EntityId##", entity.Id.ToString());
                mailMessage = mailMessage.Replace("##MDMObjectType##", entity.ObjectType);
                mailMessage = mailMessage.Replace("##EntityName##", entity.Name);
                mailMessage = mailMessage.Replace("##EntityLongName##", entity.LongName);
                mailMessage = mailMessage.Replace("##CategoryName##", entity.CategoryName);
                mailMessage = mailMessage.Replace("##CategoryLongName##", entity.CategoryLongName);

                //Populate entity created user
                if (entityCreatedUserDictionary != null && entityCreatedUserDictionary.ContainsKey(entity.Id))
                {
                    //Get entity created user
                    String entityCreatedUserLogin = entityCreatedUserDictionary[entity.Id];
                    SecurityUser entityCreatedUser = null;
                    if (usersInContext != null && usersInContext.Count() > 0)
                    {
                        entityCreatedUser = usersInContext.FirstOrDefault(u => u.SecurityUserLogin == entityCreatedUserLogin);
                    }
                    
                    if (entityCreatedUser != null)
                    {
                        mailMessage = mailMessage.Replace("##EntityCreatedUserLogin##", entityCreatedUser.SecurityUserLogin);
                        mailMessage = mailMessage.Replace("##EntityCreatedUserFirstName##", entityCreatedUser.FirstName);
                        mailMessage = mailMessage.Replace("##EntityCreatedUserLastName##", entityCreatedUser.LastName);
                        mailMessage = mailMessage.Replace("##EntityCreatedUserFullName##", String.Format("{0} {1}", entityCreatedUser.FirstName, entityCreatedUser.LastName));
                    }
                    else
                    {
                        mailMessage = mailMessage.Replace("##EntityCreatedUserLogin##", entityCreatedUserLogin);
                        mailMessage = mailMessage.Replace("##EntityCreatedUserFirstName##", entityCreatedUserLogin);
                        mailMessage = mailMessage.Replace("##EntityCreatedUserLastName##", entityCreatedUserLogin);
                        mailMessage = mailMessage.Replace("##EntityCreatedUserFullName##", entityCreatedUserLogin);
                    }
                }

                if (entity != null && entity.Attributes != null && entity.Attributes.Count > 0)
                {
                    foreach (Attribute attr in entity.Attributes)
                    {
                        String attrVal = String.Empty;
                        String collectionValues = String.Empty;
                        if (!attr.IsComplex && attr.HasAnyValue())
                        {
                            if (!attr.IsCollection)
                            {
                                attrVal = attr.GetCurrentValue().ToString();

                                if (attr.IsLookup)
                                {
                                    Int32 lookupKey = ValueTypeHelper.Int32TryParse(attrVal, 0);
                                    attrVal = GetLookupKeyDisplayFormat(attr.Id, lookupKey, callerContext);
                                }
                            }
                            else
                            {
                                IValueCollection valueCollection = attr.GetCurrentValues();
                                if (valueCollection != null && valueCollection.Count > 0)
                                {
                                    foreach (IValue valueItem in valueCollection)
                                    {
                                        attrVal = valueItem.AttrVal.ToString();

                                        if (attr.IsLookup)
                                        {
                                            Int32 lookupKey = ValueTypeHelper.Int32TryParse(attrVal, 0);
                                            if (!String.IsNullOrWhiteSpace(collectionValues))
                                            {
                                                collectionValues += " |";
                                            }
                                            if (lookupKey > 0)
                                            {
                                                collectionValues += GetLookupKeyDisplayFormat(attr.Id, lookupKey, callerContext);
                                            }
                                        }
                                        else
                                        {

                                            if (!String.IsNullOrWhiteSpace(collectionValues))
                                                collectionValues += " |";
                                            collectionValues += attrVal;
                                        }
                                    }
                                    attrVal = collectionValues;
                                }
                            }
                        }
                        mailMessage = mailMessage.Replace("##" + attr.Id + "##", attrVal);
                    }
                }
            }

            if (iWorkflowState != null)
            {
                mailMessage = mailMessage.Replace("##workflowActivityID##", iWorkflowState.ActivityId.ToString());
                mailMessage = mailMessage.Replace("##WorkflowActivityId##", iWorkflowState.ActivityId.ToString());
                mailMessage = mailMessage.Replace("##WorkflowName##", iWorkflowState.WorkflowName);
                mailMessage = mailMessage.Replace("##WorkflowLongName##", iWorkflowState.WorkflowLongName);
                mailMessage = mailMessage.Replace("##ActivityLongName##", iWorkflowState.ActivityLongName);
                mailMessage = mailMessage.Replace("##WorkflowComments##", iWorkflowState.WorkflowComments.HtmlEncode().Replace("\r\n", "<br>"));

                //Get assigned user from UsersInContext
                SecurityUser assignedUser = null;

                if (usersInContext != null && usersInContext.Count() > 0)
                {
                    assignedUser = usersInContext.FirstOrDefault(u => u.SecurityUserLogin == iWorkflowState.AssignedUser);
                }

                mailMessage = mailMessage.Replace("##UserName##", iWorkflowState.AssignedUser);

                if (assignedUser != null)
                {
                    mailMessage = mailMessage.Replace("##AssignedUserLogin##", assignedUser.SecurityUserLogin);
                    mailMessage = mailMessage.Replace("##AssignedUserFirstName##", assignedUser.FirstName);
                    mailMessage = mailMessage.Replace("##AssignedUserLastName##", assignedUser.LastName);
                    mailMessage = mailMessage.Replace("##AssignedUserFullName##", String.Format("{0} {1}", assignedUser.FirstName, assignedUser.LastName));
                }
                else
                {
                    mailMessage = mailMessage.Replace("##AssignedUserLogin##", iWorkflowState.AssignedUser);
                    mailMessage = mailMessage.Replace("##AssignedUserFirstName##", iWorkflowState.AssignedUser);
                    mailMessage = mailMessage.Replace("##AssignedUserLastName##", iWorkflowState.AssignedUser);
                    mailMessage = mailMessage.Replace("##AssignedUserFullName##", iWorkflowState.AssignedUser);
                }

                //Get previous activity user from UsersInContext
                SecurityUser previousActivityUser = null;
                if (usersInContext != null && usersInContext.Count() > 0)
                {
                    previousActivityUser = usersInContext.FirstOrDefault(u => u.SecurityUserLogin == iWorkflowState.PreviousActivityUser);
                }
                if (previousActivityUser != null)
                {
                    mailMessage = mailMessage.Replace("##PreviousActorLogin##", previousActivityUser.SecurityUserLogin);
                    mailMessage = mailMessage.Replace("##PreviousActorFirstName##", previousActivityUser.FirstName);
                    mailMessage = mailMessage.Replace("##PreviousActorLastName##", previousActivityUser.LastName);
                    mailMessage = mailMessage.Replace("##PreviousActorFullName##", String.Format("{0} {1}", previousActivityUser.FirstName, previousActivityUser.LastName));
                }
                else
                {
                    mailMessage = mailMessage.Replace("##PreviousActorLogin##", iWorkflowState.PreviousActivityUser);
                    mailMessage = mailMessage.Replace("##PreviousActorFirstName##", iWorkflowState.PreviousActivityUser);
                    mailMessage = mailMessage.Replace("##PreviousActorLastName##", iWorkflowState.PreviousActivityUser);
                    mailMessage = mailMessage.Replace("##PreviousActorFullName##", iWorkflowState.PreviousActivityUser);
                }

                mailMessage = mailMessage.Replace("##PreviousActivityLongName##", iWorkflowState.PreviousActivityLongName);
                mailMessage = mailMessage.Replace("##PreviousAction##", iWorkflowState.PreviousActivityAction);
                mailMessage = mailMessage.Replace("##Comments##", iWorkflowState.PreviousActivityComments.HtmlEncode().Replace("\r\n", "<br>"));
                mailMessage = mailMessage.Replace("##ActivityComments##", iWorkflowState.PreviousActivityComments.HtmlEncode().Replace("\r\n", "<br>"));
            }

            if (templateData != null)
            {
                foreach (var kvp in templateData)
                {
                    mailMessage = mailMessage.Replace(kvp.Key, kvp.Value);
                }
            }
            return mailMessage;
        }

        private void PopulateEntitiesWithWorkflowDetails(EntityCollection entities, WorkflowDataContext workflowDataContext, String currentActivityShortName, String currentActivityLongName, SecurityUser assignedUser, WorkflowActionContext previousWorkflowActionContext, CallerContext callerContext)
        {
            Collection<WorkflowActivity> availableWFActivities = GetAvailableActivityDetails(callerContext);
            WorkflowState workflowState = new WorkflowState();

            //Get current workflow activity Id from available activities
            Int32 workflowActivityId = 0;
            if (availableWFActivities != null)
            {
                WorkflowActivity workflowActivity = availableWFActivities.FirstOrDefault(a => a.WorkflowVersionId == workflowDataContext.WorkflowVersionId && a.Name == currentActivityShortName);

                if (workflowActivity != null)
                {
                    workflowActivityId = workflowActivity.Id;
                }
            }

            workflowState.WorkflowName = workflowDataContext.WorkflowName;
            workflowState.WorkflowLongName = workflowDataContext.WorkflowLongName;
            workflowState.ActivityId = workflowActivityId;
            workflowState.ActivityShortName = currentActivityShortName;
            workflowState.ActivityLongName = currentActivityLongName;
            workflowState.EventDate = DateTime.Now.ToString();
            workflowState.WorkflowComments = workflowDataContext.WorkflowComments;

            if (assignedUser != null)
            {
                workflowState.AssignedUser = assignedUser.SecurityUserLogin;
            }

            if (previousWorkflowActionContext != null)
            {
                workflowState.PreviousActivityAction = previousWorkflowActionContext.UserAction.ToString();
                workflowState.PreviousActivityComments = previousWorkflowActionContext.Comments;
                workflowState.PreviousActivityShortName = previousWorkflowActionContext.CurrentActivityName;
                workflowState.PreviousActivityUser = previousWorkflowActionContext.ActingUserName;

                //Get previous activity Long Name from available workflow activities
                if (availableWFActivities != null)
                {
                    WorkflowActivity workflowActivity = availableWFActivities.FirstOrDefault(a => a.WorkflowVersionId == workflowDataContext.WorkflowVersionId && a.Name == previousWorkflowActionContext.CurrentActivityName);

                    if (workflowActivity != null)
                    {
                        workflowState.PreviousActivityLongName = workflowActivity.LongName;
                    }
                }
            }

            foreach (Entity entity in entities)
            {
                if (entity.WorkflowStates == null)
                {
                    entity.WorkflowStates = new WorkflowStateCollection();
                }

                entity.WorkflowStates.Add(workflowState);
            }
        }

        #endregion Helper Methods for Mail Message

        #region Other Methods

        /// <summary>
        /// Get the message template based on template name
        /// If there is no template available will return null.
        /// and check whether the template is disable or not.
        /// </summary>
        /// <param name="templateName">Indicate the template name</param>
        /// <param name="callerContext"></param>
        /// <returns>MessageTemplate</returns>
        private MessageTemplate GetMessageTemplate(String templateName, CallerContext callerContext)
        {
            MessageTemplate messageTemplate = null;
            MDMMessageNotificationDA messageNotificationDA = new MDMMessageNotificationDA();
            messageTemplate = messageNotificationDA.GetMessageTemplateByName(templateName);

            if (messageTemplate == null || messageTemplate.Disabled)
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, " Message template is disable or not available.", MDMTraceSource.General);

                String errorMessage = this.GetSystemLocaleMessage("111994", callerContext).Message;
                throw new MDMOperationException("111994", errorMessage, "MailNotificationManager", String.Empty, "SendMail");   //Message template is disable or not available.
            }

            return messageTemplate;
        }

        private LocaleMessage GetSystemLocaleMessage(String messageCode, CallerContext callerContext)
        {
            LocaleMessageBL localeMessageBL = new LocaleMessageBL();

            return localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), messageCode, false, callerContext);
        }

        private EntityCollection GetEntities(Collection<Int64> entityIdList, Collection<Int32> attributeIdList, String workflowName, String activityShortName, String activityLongName, String workflowComments, Boolean loadWorkflowDetails, CallerContext callerContext)
        {
            EntityCollection entities = null;

            if (entityIdList != null && entityIdList.Count > 0)
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Starting Get Entities operation...", MDMTraceSource.General);

                EntityContext entityContext = new EntityContext();
                entityContext.AttributeIdList = attributeIdList;
                entityContext.LoadAttributes = true;
                entityContext.LoadEntityProperties = true;

                //Check for activity name..
                //If provided then the request is for that particular activity.
                //So, set LoadWorkflowInfomation as false when particular activity has been requested as setting it to true always returns currently running activity details
                entityContext.LoadWorkflowInformation = (String.IsNullOrWhiteSpace(activityShortName)) ? loadWorkflowDetails : false;

                if (!String.IsNullOrWhiteSpace(workflowName))
                {
                    entityContext.WorkflowName = workflowName;
                }

                EntityBL entityBL = new EntityBL();
                entities = entityBL.Get(entityIdList, entityContext, callerContext.Application, callerContext.Module);

                //Load details for requested activity
                if (entities != null && entities.Count > 0 && !String.IsNullOrWhiteSpace(activityShortName))
                {
                    //Since a particular activity details are requested, it means that same activity is applicable for all entities..
                    //So, consider first entity to fetch that activity details
                    Entity firstAvailableEntity = entities.FirstOrDefault();

                    WorkflowInstanceBL workflowInstanceBL = new WorkflowInstanceBL();
                    TrackedActivityInfoCollection trackedActivityInfoCollection = workflowInstanceBL.GetWorkflowExecutionDetails(firstAvailableEntity.Id, workflowName, callerContext, true);

                    if (trackedActivityInfoCollection != null && trackedActivityInfoCollection.Count > 0)
                    {
                        List<TrackedActivityInfo> sortedTrackedActivityInfoList = trackedActivityInfoCollection.OrderByDescending(a => ValueTypeHelper.ConvertToDateTime(a.EventDate)).ToList();
                        TrackedActivityInfo trackedActivityInfo = sortedTrackedActivityInfoList.FirstOrDefault(a => a.ActivityShortName == activityShortName);

                        String requestedActivityLongName = String.Empty;
                        String eventDate = String.Empty;
                        TrackedActivityInfo previousActivityInfo = null;

                        if (trackedActivityInfo == null)
                        {
                            //Requested activity info is not available..
                            //This is possible only if the current activity is CodedActivity provided the activity name passed by the user is part of the workflow
                            requestedActivityLongName = activityLongName;
                            eventDate = DateTime.Now.ToString();

                            //Consider latest completed activity as previous activity.
                            previousActivityInfo = sortedTrackedActivityInfoList.FirstOrDefault(a => a.Status.ToLowerInvariant() == "closed");
                        }
                        else
                        {
                            requestedActivityLongName = trackedActivityInfo.ActivityLongName;
                            eventDate = trackedActivityInfo.EventDate;
                            workflowComments = trackedActivityInfo.WorkflowComments;
                            previousActivityInfo = sortedTrackedActivityInfoList.FirstOrDefault(a => a.ActivityShortName == trackedActivityInfo.PreviousActivityShortName);
                        }

                        //Populate requested state details
                        WorkflowState requestedStateDetails = new WorkflowState();
                        requestedStateDetails.WorkflowName = workflowName;
                        requestedStateDetails.ActivityShortName = activityShortName;
                        requestedStateDetails.ActivityLongName = requestedActivityLongName;
                        requestedStateDetails.EventDate = eventDate;
                        requestedStateDetails.WorkflowComments = workflowComments;

                        if (previousActivityInfo != null)
                        {
                            requestedStateDetails.WorkflowLongName = previousActivityInfo.WorkflowLongName;
                            requestedStateDetails.PreviousActivityShortName = previousActivityInfo.ActivityShortName;
                            requestedStateDetails.PreviousActivityLongName = previousActivityInfo.ActivityLongName;
                            requestedStateDetails.PreviousActivityUserId = previousActivityInfo.ActedUserId;
                            requestedStateDetails.PreviousActivityUser = previousActivityInfo.ActedUser;
                            requestedStateDetails.PreviousActivityComments = previousActivityInfo.ActivityComments;
                            requestedStateDetails.PreviousActivityAction = previousActivityInfo.PerformedAction;
                        }

                        //Add thus populated workflow states to all entities
                        foreach (Entity entity in entities)
                        {
                            if (entity.WorkflowStates == null)
                            {
                                entity.WorkflowStates = new WorkflowStateCollection();
                            }

                            entity.WorkflowStates.Add(requestedStateDetails);
                        }
                    }
                }

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Done with Get Entities operation.", MDMTraceSource.General);
            }

            return entities;
        }

        private String GetLookupKeyDisplayFormat(Int32 attributeId, Int32 lookupKey, CallerContext callerContext)
        {
            String displayFormat = String.Empty;
            Lookup lookup = null;
            LookupBL lookupBL = new LookupBL();

            //Check whether lookup has been loaded
            if (_attributeLookupDataMap.ContainsKey(attributeId))
            {
                lookup = _attributeLookupDataMap[attributeId];
            }
            else
            {
                lookup = lookupBL.Get(attributeId, GlobalizationHelper.GetSystemUILocale(), -1, callerContext);

                if (lookup != null)
                {
                    _attributeLookupDataMap.Add(attributeId, lookup);
                }
            }

            if (lookup != null)
            {
                displayFormat = lookup.GetDisplayFormatById(lookupKey);
            }

            if (String.IsNullOrWhiteSpace(displayFormat))
                displayFormat = lookupKey.ToString();

            return displayFormat;
        }

        private Dictionary<Int64, String> GetEntityCreatedUsers(Collection<Int64> entityIds, MessageTemplate messageTemplate)
        {
            Dictionary<Int64, String> entityCreatedUserDictionary = new Dictionary<Int64, String>();

            if (messageTemplate.MessageBody.Contains("##EntityCreatedUser") || messageTemplate.MessageSubject.Contains("##EntityCreatedUser") || messageTemplate.MDMObjectContentTemplate.Contains("##EntityCreatedUser"))
            {
                AuditInfoBL auditInfoManager = new AuditInfoBL();
                EntityAuditInfoCollection entityAuditInfo = auditInfoManager.Get(entityIds, null, GlobalizationHelper.GetSystemDataLocale(), 0, true, false, false);

                if (entityAuditInfo != null && entityAuditInfo.Count > 0)
                {
                    foreach (Int64 entityId in entityIds)
                    {
                        //Get audit info for this entity where action is add..
                        EntityAuditInfo entityCreateAuditInfo = entityAuditInfo.FirstOrDefault(a => a.EntityId == entityId && a.Action == ObjectAction.Create);

                        if (entityCreateAuditInfo != null && !entityCreatedUserDictionary.ContainsKey(entityId))
                        {
                            entityCreatedUserDictionary.Add(entityId, entityCreateAuditInfo.UserLogin);
                        }
                    }
                }
            }

            return entityCreatedUserDictionary;
        }

        private SecurityUserCollection GetAllUsersInContext(EntityCollection entities, Dictionary<Int64, String> entityCreatedUserDictionary, Collection<String> toMDMUsers, Collection<String> ccMDMUsers)
        {
            SecurityUserCollection securityUserCollection = null;

            //Prepare a list of required MDM users
            List<String> requiredUsers = new List<String>();

            //Add current workflow assigned user and previous acted user
            foreach (Entity entity in entities)
            {
                IWorkflowStateCollection iWFstates = entity.GetWorkflowDetails();

                if (iWFstates != null && iWFstates.Count() > 0)
                {
                    WorkflowState workflowState = iWFstates.OrderByDescending(wf => ValueTypeHelper.ConvertToDateTime(wf.EventDate)).FirstOrDefault();

                    if (workflowState != null)
                    {
                        if (!String.IsNullOrWhiteSpace(workflowState.AssignedUser))
                        {
                            requiredUsers.Add(workflowState.AssignedUser);
                        }

                        if (!String.IsNullOrWhiteSpace(workflowState.PreviousActivityUser))
                        {
                            requiredUsers.Add(workflowState.PreviousActivityUser);
                        }
                    }
                }
            }

            //Add entity created user
            if (entityCreatedUserDictionary != null && entityCreatedUserDictionary.Count > 0)
            {
                requiredUsers.AddRange(entityCreatedUserDictionary.Values.ToList());
            }

            //Add to MDM users
            if (toMDMUsers != null)
            {
                requiredUsers.AddRange(toMDMUsers);
            }

            //Add cc MDM Users
            if (ccMDMUsers != null)
            {
                requiredUsers.AddRange(ccMDMUsers);
            }

            //Get thus concatenated users details from DB
            SecurityUserBL securityUserManager = new SecurityUserBL();
            securityUserCollection = securityUserManager.GetUsersByLogins(new Collection<String>(requiredUsers.Distinct().ToList()));

            return securityUserCollection;
        }

        private Collection<WorkflowActivity> GetAvailableActivityDetails(CallerContext callerContext)
        {
            Collection<Workflow> workflowCollection = null;
            Collection<WorkflowVersion> workflowVersionCollection = null;
            Collection<WorkflowActivity> workflowActivityCollection = null;

            WorkflowInstanceBL workflowInstanceManager = new WorkflowInstanceBL();
            workflowInstanceManager.GetAllWorkflowDetails(ref workflowCollection, ref workflowVersionCollection, ref workflowActivityCollection, callerContext);

            return workflowActivityCollection;
        }

        private void LogException(Exception ex)
        {
            try
            {
                ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
            }
            catch
            {
                //Do not throw
            }
        }

        private String GetWebServerDetail()
        {
            String server = String.Empty;

            String serverXml = AppConfigurationHelper.GetAppConfig("MDMCenter.Diagnostics.ServerConfiguration", "");

            if (!String.IsNullOrWhiteSpace(serverXml))
            {
                XDocument xDoc = XDocument.Parse(serverXml);
                // May have more server so we will take first.
                server = (from f in xDoc.Descendants("WebServers")
                          from f1 in f.Elements("WebServer")
                          select String.Format("{0}/{1}", f1.Attribute("ServerName").Value, f1.Attribute("VirtualDirectory").Value)).FirstOrDefault();
            }

            return server;
        }

        private void GetUserEmailIdsInRole(Collection<String> roleNames, Collection<String> emailIds, CallerContext callerContext)
        {
            SecurityUserBL securityUserManager = new SecurityUserBL();
            SecurityUserCollection securityUserCollection = new SecurityUserCollection();

            if (roleNames != null)
            {
                foreach (var roleName in roleNames)
                {
                    securityUserCollection.AddRange(securityUserManager.GetUsersInRole(roleName, callerContext));
                }
            }

            foreach (SecurityUser user in securityUserCollection)
            {
                if (!emailIds.Contains(user.Smtp))
                    emailIds.Add(user.Smtp);
            }
        }

        #endregion Other Methods

        #endregion Private Methods

        #endregion Methods
    }
}
