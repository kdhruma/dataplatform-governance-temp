using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Mail;
using System.Security.Principal;
using System.ServiceModel;

namespace MDM.Services
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Workflow;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.Interfaces;
    using MDM.Utility;
    using MDM.WCFServiceInterfaces;
    using MDM.Services.ServiceProxies;

    ///<summary>
    /// Message Service facilitates to create and send messages in MDMCenter. It includes sending workflow messages.
    ///</summary>
    public class MessageService : ServiceClientBase
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructors

        /// <summary>
        /// Use this default constructor only when calling MDM service within the execution context of an MDM Application. 
        /// Example: Service call from the MDM business rule.
        /// This would try to use current execution security context for service authentication purpose. 
        /// </summary>
        public MessageService()
            : base(typeof(MessageService))
        {
        }

        /// <summary>
        /// Use this default constructor with security principal only when calling MDM service within the execution context of an MDM Application. 
        /// Example: Service call from the MDM business rule.
        /// This would try to use current execution security context for service authentication purpose. 
        /// </summary>
        /// <param name="iSecurityPrincipal">Current security principal</param>
        public MessageService(ISecurityPrincipal iSecurityPrincipal)
            : base(typeof(MessageService), iSecurityPrincipal)
        {
        }

        /// <summary>
        /// Use this default constructor with binding configuration name only when calling MDM service within the execution context of an MDM Application. 
        /// Example: Service call from the MDM business rule.
        /// This would try to use current execution security context for service authentication purpose. 
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        public MessageService(String endPointConfigurationName)
            : base(typeof(MessageService), endPointConfigurationName)
        {
        }

        /// <summary>
        /// Use this constructor for form based authentication passing username and password
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        /// <param name="userName">Name of the Login User</param>
        /// <param name="password">Password of the User</param>
        public MessageService(String endPointConfigurationName, String userName, String password)
            : base(endPointConfigurationName, userName, password)
        {

        }

        /// <summary>
        /// Use this constructor for window authentication passing current windows identity.
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        /// <param name="userIdentity">Indicates User's Identity</param>
        public MessageService(String endPointConfigurationName, IIdentity userIdentity)
            : base(endPointConfigurationName, userIdentity)
        {
        }

        /// <summary>
        /// Constructor with client configuration.
        /// </summary>
        /// <param name="wcfClientConfiguration">Indicates WCF Client Configuration</param>
        public MessageService(IWCFClientConfiguration wcfClientConfiguration)
            : base(wcfClientConfiguration)
        {
        }

        /// <summary>
        /// Constructor with endPointConfigurationName, authenticationType, userIdentity, userName and password
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        /// <param name="authenticationType">Indicates Type of Authentication i.e Windows/Forms</param>
        /// <param name="userIdentity">Indicates INdentity of the Login User</param>
        /// <param name="userName">Name of the Login User</param>
        /// <param name="password">Password of the User</param>
        public MessageService(String endPointConfigurationName, AuthenticationType authenticationType, IIdentity userIdentity, String userName, String password)
            : base(endPointConfigurationName, authenticationType, userIdentity, userName, password)
        {
        }

        /// <summary>
        /// Constructor with endPointConfigurationName, endPointAddress, authenticationType, userIdentity, userName and password
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        /// <param name="endPointAddress"> Provides a unique network address that a client uses to communicate with a service endpoint.</param>
        /// <param name="authenticationType">Indicates Type of Authentication i.e Windows/Forms</param>
        /// <param name="userIdentity">Indicates INdentity of the Login User</param>
        /// <param name="userName">Name of the Login User</param>
        /// <param name="password">Password of the User</param>
        public MessageService(String endPointConfigurationName, EndpointAddress endPointAddress, AuthenticationType authenticationType, IIdentity userIdentity, String userName, String password)
            : base(endPointConfigurationName, endPointAddress, authenticationType, userIdentity, userName, password)
        {
        }
        #endregion

        #region Public methods

        ///<summary>
        /// Create Message
        ///</summary>
        ///<param name="Message">Indicates message</param>
        ///<param name="operationResult">Indicated Operation Result</param>
        public void Create(Message Message, ref OperationResult operationResult)
        {
            try
            {
                IMessageService MessageManager = GetClient();

                MessageManager.Create(Message, ref operationResult);
            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                //LogException(ex);
            }
        }
        
        ///<summary>
        /// Create Message based on type,Subject,Body and Priority
        ///</summary>
        ///<param name="Type">Indicates type of the Message</param>
        ///<param name="From">Indicates from message</param>
        ///<param name="To">Indicates to message</param>
        ///<param name="Subject">Indicates subject of Message</param>
        ///<param name="Body">Indicates Body of the Message</param>
        ///<param name="Priority">Indicates the priority of the Message</param>
        ///<param name="operationResult">Indicates Operation result</param>
        
        public void Create(String Type, String From, String To, String Subject, String Body, String Priority, ref OperationResult operationResult)
        {
            try
            {
                IMessageService MessageManager = GetClient();

                MessageManager.Create(Type, From, To, Subject, Body, Priority, ref operationResult);
            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                //LogException(ex);
            }
        }
        
        ///<summary>
        /// Create Message based on type,Subject,Body and Priority
        ///</summary>
        ///<param name="Type">Indicates type of the Message</param>
        ///<param name="From">Indicates from message</param>
        ///<param name="To">Indicates to message</param>
        ///<param name="Subject">Indicates subject of Message</param>
        ///<param name="Body">Indicates Body of the Message</param>
        ///<param name="Priority">Indicates the priority of the Message</param>
        ///<param name="operationResult">Indicates Operation result</param>
        public void Create(MessageType Type, String From, String To, String Subject, String Body, MailPriority Priority, ref OperationResult operationResult)
        {
            try
            {
                IMessageService MessageManager = GetClient();

                MessageManager.Create(Type, From, To, Subject, Body, Priority, ref operationResult);
            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                //LogException(ex);
            }
        }

        ///<summary>
        /// Sends Workflow message to Single user by MDMObject and Workflow Id 
        ///</summary>
        ///<param name="mdmObjectCollection">Indicates Collection of MDM Object</param>
        ///<param name="workflowID">Indicates Id of workflow</param>
        ///<param name="workflowActivityID">Indicates Activity Id of Workflow</param>
        ///<param name="userID">Indicates User Id</param>
        ///<param name="templateName">Indicates Template Name</param>
        ///<param name="operationResult">Indicates Operation result</param>
        public void SendWorkflowMessages(Collection<WorkflowMDMObject> mdmObjectCollection, Int32 workflowID, Int32 workflowActivityID, Int32 userID, String templateName, ref OperationResult operationResult)
        {
            try
            {
                IMessageService MessageManager = GetClient();

                MessageManager.SendWorkflowMessages(mdmObjectCollection, workflowID, workflowActivityID, userID, templateName, ref operationResult);
            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                //LogException(ex);
            }
        }
        
        ///<summary>
        /// Sends Workflow message to Multiple user by MDMObject and Workflow Id 
        ///</summary>
        ///<param name="mdmObjectCollection">Indicates Collection of MDM Object</param>
        ///<param name="workflowID">Indicates Id of workflow</param>
        ///<param name="workflowActivityID">Indicates Activity Id of Workflow</param>
        ///<param name="userIDs">Indicates Collection of User Ids</param>
        ///<param name="templateName">Indicates Template Name</param>
        ///<param name="operationResult">Indicates Operation result</param>
        public void SendWorkflowMessages(Collection<WorkflowMDMObject> mdmObjectCollection, Int32 workflowID, Int32 workflowActivityID, Collection<Int32> userIDs, String templateName, ref OperationResult operationResult)
        {
            try
            {
                IMessageService MessageManager = GetClient();

                MessageManager.SendWorkflowMessages(mdmObjectCollection, workflowID, workflowActivityID, userIDs, templateName, ref operationResult);
            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                //LogException(ex);
            }
        }

        ///<summary>
        /// Sends Workflow message to Single user by MDMObject and Workflow Version
        ///</summary>
        ///<param name="mdmObjectCollection">Indicates Collection of MDM Object</param>
        ///<param name="workflowVersionID">Indicates Version Id of Workflow</param>
        ///<param name="activityName">Indicates Workflow Name</param>
        ///<param name="userID">Indicates User Id</param>
        ///<param name="templateName">Indicates Template Name</param>
        ///<param name="operationResult">Indicates Operation result</param>
        public void SendWorkflowMessages(Collection<WorkflowMDMObject> mdmObjectCollection, Int32 workflowVersionID, String activityName, Int32 userID, String templateName, ref OperationResult operationResult)
        {
            try
            {
                IMessageService MessageManager = GetClient();

                MessageManager.SendWorkflowMessages(mdmObjectCollection, workflowVersionID, activityName, userID, templateName, ref operationResult);
            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                //LogException(ex);
            }
        }
        
        ///<summary>
        /// Sends Workflow message to Multiple user by MDMObject and Workflow Version
        ///</summary>
        ///<param name="mdmObjectCollection">Indicates Collection of MDM Object</param>
        ///<param name="workflowVersionID">Indicates Version Id of Workflow</param>
        ///<param name="activityName">Indicates Workflow Name</param>
        ///<param name="userIDs">Indicates collection of User Ids</param>
        ///<param name="templateName">Indicates Template Name</param>
        ///<param name="operationResult">Indicates Operation result</param>
        public void SendWorkflowMessages(Collection<WorkflowMDMObject> mdmObjectCollection, Int32 workflowVersionID, String activityName, Collection<Int32> userIDs, String templateName, ref OperationResult operationResult)
        {
            try
            {
                IMessageService MessageManager = GetClient();

                MessageManager.SendWorkflowMessages(mdmObjectCollection, workflowVersionID, activityName, userIDs, templateName, ref operationResult);
            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                //LogException(ex);
            }
        }

        ///<summary>
        /// Sends Workflow Messages By MDMObjectIdList And Workflow ID To SingleUserID
        ///</summary>
        ///<param name="MDMObjectIdList">Indicates List of MDMObject Ids</param>
        ///<param name="MDMObjectType">Indicates Type of MDMObjects</param>
        ///<param name="workflowID">Indicates Workflow ID</param>
        ///<param name="workflowActivityID">Indicates workflow ActivityID</param>
        ///<param name="userID">Indicates User ID</param>
        ///<param name="templateName">Indicates Template Name</param>
        ///<param name="operationResult">Indicates Operation Result</param>
        public void SendWorkflowMessages(String MDMObjectIdList, String MDMObjectType, Int32 workflowID, Int32 workflowActivityID, Int32 userID, String templateName, ref OperationResult operationResult)
        {
            try
            {
                IMessageService MessageManager = GetClient();

                MessageManager.SendWorkflowMessages(MDMObjectIdList, MDMObjectType, workflowID, workflowActivityID, userID, templateName, ref operationResult);
            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                //LogException(ex);
            }
        }
        
        ///<summary>
        /// Sends Workflow Messages By MDMObjectIdList And Workflow ID To MultipleUserID
        ///</summary>
        ///<param name="MDMObjectIdList">Indicates List of MDMObject Ids</param>
        ///<param name="MDMObjectType">Indicates Type of MDMObjects</param>
        ///<param name="workflowID">Indicates Workflow ID</param>
        ///<param name="workflowActivityID">Indicates workflow ActivityID</param>
        ///<param name="userIDs">Indicates collection of User IDs</param>
        ///<param name="templateName">Indicates Template Name</param>
        ///<param name="operationResult">Indicates Operation Result</param>
        public void SendWorkflowMessages(String MDMObjectIdList, String MDMObjectType, Int32 workflowID, Int32 workflowActivityID, Collection<Int32> userIDs, String templateName, ref OperationResult operationResult)
        {
            try
            {
                IMessageService MessageManager = GetClient();

                MessageManager.SendWorkflowMessages(MDMObjectIdList, MDMObjectType, workflowID, workflowActivityID, userIDs, templateName, ref operationResult);
            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                //LogException(ex);
            }
        }

        ///<summary>
        /// Sends Workflow Messages By MDMObjectIdList And Workflow ID To SingleUserLogin
        ///</summary>
        ///<param name="MDMObjectIdList">Indicates List of MDMObject Ids</param>
        ///<param name="MDMObjectType">Indicates Type of MDMObjects</param>
        ///<param name="workflowID">Indicates Workflow ID</param>
        ///<param name="workflowActivityID">Indicates workflow ActivityID</param>
        ///<param name="userLogin">Indicates User login</param>
        ///<param name="templateName">Indicates Template Name</param>
        ///<param name="operationResult">Indicates Operation Result</param>
        public void SendWorkflowMessages(String MDMObjectIdList, String MDMObjectType, Int32 workflowID, Int32 workflowActivityID, String userLogin, String templateName, ref OperationResult operationResult)
        {
            try
            {
                IMessageService MessageManager = GetClient();

                MessageManager.SendWorkflowMessages(MDMObjectIdList, MDMObjectType, workflowID, workflowActivityID, userLogin, templateName, ref operationResult);
            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                //LogException(ex);
            }
        }
        
        ///<summary>
        /// Sends Workflow Messages By MDMObjectIdList And Workflow ID To MultipleUserLogin
        ///</summary>
        ///<param name="MDMObjectIdList">Indicates List of MDMObject Ids</param>
        ///<param name="MDMObjectType">Indicates Type of MDMObjects</param>
        ///<param name="workflowID">Indicates Workflow ID</param>
        ///<param name="workflowActivityID">Indicates workflow ActivityID</param>
        ///<param name="userLogins">Indicates collection of User login</param>
        ///<param name="templateName">Indicates Template Name</param>
        ///<param name="operationResult">Indicates Operation Result</param>
        public void SendWorkflowMessages(String MDMObjectIdList, String MDMObjectType, Int32 workflowID, Int32 workflowActivityID, Collection<String> userLogins, String templateName, ref OperationResult operationResult)
        {
            try
            {
                IMessageService MessageManager = GetClient();

                MessageManager.SendWorkflowMessages(MDMObjectIdList, MDMObjectType, workflowID, workflowActivityID, userLogins, templateName, ref operationResult);
            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                //LogException(ex);
            }
        }

        ///<summary>
        /// Sends Workflow Messages By MDMObjectIdList And Workflow Version To SingleUserID
        ///</summary>
        ///<param name="MDMObjectIdList">Indicates List of MDMObject Ids</param>
        ///<param name="MDMObjectType">Indicates Type of MDMObjects</param>	        
        ///<param name="workflowVersionID">Indicates Version Id of Workflow</param>
        ///<param name="activityName">Indicates Workflow Name</param>
        ///<param name="userID">Indicates User Id</param>
        ///<param name="templateName">Indicates Template Name</param>
        ///<param name="operationResult">Indicates Operation result</param>
        public void SendWorkflowMessages(String MDMObjectIdList, String MDMObjectType, Int32 workflowVersionID, String activityName, Int32 userID, String templateName, ref OperationResult operationResult)
        {
            try
            {
                IMessageService MessageManager = GetClient();

                MessageManager.SendWorkflowMessages(MDMObjectIdList, MDMObjectType, workflowVersionID, activityName, userID, templateName, ref operationResult);
            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                //LogException(ex);
            }
        }
        
        ///<summary>
        /// Sends Workflow Messages By MDMObjectIdList And Workflow Version To MultipleUserID
        ///</summary>
        ///<param name="MDMObjectIdList">Indicates List of MDMObject Ids</param>
        ///<param name="MDMObjectType">Indicates Type of MDMObjects</param>	        
        ///<param name="workflowVersionID">Indicates Version Id of Workflow</param>
        ///<param name="activityName">Indicates Workflow Name</param>
        ///<param name="userIDs">Indicates collection of User Id</param>
        ///<param name="templateName">Indicates Template Name</param>
        ///<param name="operationResult">Indicates Operation result</param>
        public void SendWorkflowMessages(String MDMObjectIdList, String MDMObjectType, Int32 workflowVersionID, String activityName, Collection<Int32> userIDs, String templateName, ref OperationResult operationResult)
        {
            try
            {
                IMessageService MessageManager = GetClient();

                MessageManager.SendWorkflowMessages(MDMObjectIdList, MDMObjectType, workflowVersionID, activityName, userIDs, templateName, ref operationResult);
            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                //LogException(ex);
            }
        }

        ///<summary>
        /// Sends Workflow Messages By MDMObjectIdList And Workflow Version To SingleUserLogin
        ///</summary>
        ///<param name="MDMObjectIdList">Indicates List of MDMObject Ids</param>
        ///<param name="MDMObjectType">Indicates Type of MDMObjects</param>	        
        ///<param name="workflowVersionID">Indicates Version Id of Workflow</param>
        ///<param name="activityName">Indicates Workflow Name</param>
        ///<param name="userLogin">Indicates UserLogin</param>
        ///<param name="templateName">Indicates Template Name</param>
        ///<param name="operationResult">Indicates Operation result</param>
        public void SendWorkflowMessages(String MDMObjectIdList, String MDMObjectType, Int32 workflowVersionID, String activityName, String userLogin, String templateName, ref OperationResult operationResult)
        {
            try
            {
                IMessageService MessageManager = GetClient();

                MessageManager.SendWorkflowMessages(MDMObjectIdList, MDMObjectType, workflowVersionID, activityName, userLogin, templateName, ref operationResult);
            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                //LogException(ex);
            }
        }
        
        ///<summary>
        /// Sends Workflow Messages By MDMObjectIdList And Workflow Version To MultipleUserLogin
        ///</summary>
        ///<param name="MDMObjectIdList">Indicates List of MDMObject Ids</param>
        ///<param name="MDMObjectType">Indicates Type of MDMObjects</param>	        
        ///<param name="workflowVersionID">Indicates Version Id of Workflow</param>
        ///<param name="activityName">Indicates Workflow Name</param>
        ///<param name="userLogins">Indicates collection of UserLogin</param>
        ///<param name="templateName">Indicates Template Name</param>
        ///<param name="operationResult">Indicates Operation result</param>
        public void SendWorkflowMessages(String MDMObjectIdList, String MDMObjectType, Int32 workflowVersionID, String activityName, Collection<String> userLogins, String templateName, ref OperationResult operationResult)
        {
            try
            {
                IMessageService MessageManager = GetClient();

                MessageManager.SendWorkflowMessages(MDMObjectIdList, MDMObjectType, workflowVersionID, activityName, userLogins, templateName, ref operationResult);
            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                //LogException(ex);
            }
        }

        ///<summary>
        /// Sends Workflow Messages By Workflow Instances
        ///</summary>
        ///<param name="workflowInstances">Indicates collection of WorkflowInstance</param>
        ///<param name="templateName">Indicates Template name</param>
        ///<param name="operationResult">Indicates Operation Result</param>
        public void SendWorkflowMessages(Collection<WorkflowInstance> workflowInstances, String templateName, ref OperationResult operationResult)
        {
            try
            {
                IMessageService MessageManager = GetClient();

                MessageManager.SendWorkflowMessages(workflowInstances, templateName, ref operationResult);
            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                //LogException(ex);
            }
        }

        ///<summary>
        /// Sends Workflow Messages By InstanceId List
        ///</summary>
        ///<param name="InstanceIdList">Indicates List of Instance Id</param>
        ///<param name="templateName">Indicates Template Name</param>
        ///<param name="operationResult">Indicates Operation Result</param>
        public void SendWorkflowMessages(String InstanceIdList, String templateName, ref OperationResult operationResult)
        {
            try
            {
                IMessageService MessageManager = GetClient();

                MessageManager.SendWorkflowMessages(InstanceIdList, templateName, ref operationResult);
            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                //LogException(ex);
            }
        }

        ///<summary>
        /// Sends Workflow Messages By EscalationCollection
        ///</summary>
        ///<param name="escalationCollection">Indicates collection of Escalation</param>
        ///<param name="templateName">Indicates Template Name</param>
        ///<param name="operationResult">Indicates Operation Result</param>
        public void SendWorkflowMessages(Collection<Escalation> escalationCollection, String templateName, ref OperationResult operationResult)
        {
            try
            {
                IMessageService MessageManager = GetClient();

                MessageManager.SendWorkflowMessages(escalationCollection, templateName, ref operationResult);
            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.ErrorMessage = ex.Message;
                operationResult.Errors.Add(error);

                //Log exception
                //LogException(ex);
            }
        }

        #region Send Mail

        /// <summary>
        /// Sends the email using template with workflow details
        /// </summary>
        /// <example>
        /// <code title="Send email using template with workflow details">
        /// // Assumption: hard coded variables for the demo purpose
        /// Collection<![CDATA[<Int64>]]> entityIdList = new Collection<![CDATA[<Int64>]]>() { 2001, 2002, 2003};
        /// 
        /// String mailTemplateName = "MailTemplate";
        /// Collection<![CDATA[<String>]]> toMDMUsers = new Collection<![CDATA[<String>]]>() { "User1", "User2" }; // User names to send email to
        /// Collection<![CDATA[<String>]]> toMailIds = new Collection<![CDATA[<String>]]>() { "user1@riversand.com", "user2@riversand.com" }; // User emails to send email to
        /// Collection<![CDATA[<String>]]> ccMDMUsers = new Collection<![CDATA[<String>]]>() { "User3", "User4" }; // User names to be included into CC
        /// Collection<![CDATA[<String>]]> ccMailIds = new Collection<![CDATA[<String>]]>() { "user3@riversand.com", "user4@riversand.com" }; // User emails to be included into CC
        /// String workflowName = "WorkflowName";
        /// Boolean consolidateAllEntitiesMessages = false; // Not to consolidate messages
        /// 
        /// //Set caller context
        /// ICallerContext callerContext = MDMObjectFactory.GetICallerContext();
        /// callerContext.Application = MDMCenterApplication.PIM;
        /// callerContext.Module = MDMCenterModules.Entity;
        /// 
        /// MessageService messageService = InitializeMessageServiceWithUserNameAndPassword();
        /// 
        /// IOperationResult result = messageService.SendMailWithWorkflowDetails(entityIdList, mailTemplateName,
        /// toMDMUsers, toMailIds, ccMDMUsers, ccMailIds, workflowName, callerContext, consolidateAllEntitiesMessages);
        /// 
        /// </code>
        /// </example>
        /// <param name="entityIdList">Indicates the collection of the entities ids</param>
        /// <param name="mailTemplateName">Indicates the name of the email template</param>
        /// <param name="toMDMUsers">Indicates the list of MDMCenter user login names (To send an Email)</param>
        /// <param name="toMailIds">Indicates the list of receiver email addresses</param>
        /// <param name="ccMDMUsers">Indicates the list of MDMCenter user login Names (To send as a CC email)</param>
        /// <param name="ccMailIds">Indicates the list of CC email addresses</param>
        /// <param name="workflowName">Indicates the workflow name</param>
        /// <param name="iCallerContext">Indicates the caller context details such as application name and module name</param>
        /// <param name="consolidateAllEntitiesMessages">Indicates whether to consolidate all messages or not</param>
        /// <returns>Results whether the email has been sent or not</returns>
        /// <exception cref="MDMExceptionDetails">Thrown when operation fails</exception>
        public IOperationResult SendMailWithWorkflowDetails(Collection<Int64> entityIdList, String mailTemplateName, Collection<String> toMDMUsers,
                                                            Collection<String> toMailIds, Collection<String> ccMDMUsers, Collection<String> ccMailIds, 
                                                            String workflowName, ICallerContext iCallerContext, Boolean consolidateAllEntitiesMessages)
        {
            return MakeServiceCall<IOperationResult>("SendMailWithWorkflowDetails", "SendMailWithWorkflowDetails",
                        client => client.SendMailWithWorkflowDetails(entityIdList, mailTemplateName, toMDMUsers, toMailIds, ccMDMUsers, ccMailIds, 
                                                                        workflowName, FillDiagnosticTraces(iCallerContext), consolidateAllEntitiesMessages));
        }

        /// <summary>
        /// Sends the email using template
        /// </summary>
        /// <param name="entityIdList">Indicates the collection of entities</param>
        /// <param name="iEmailContext">Indicates the email context details</param>
        /// <param name="workflowName">Indicates the workflow name</param>
        /// <param name="iCallerContext">Indicates the caller context details</param>
        /// <param name="consolidateAllEntitiesMessages">Indicates whether to consolidate all messages or not</param>
        /// <returns>Results whether the email has been sent or not</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Send email using workflow details" source="..\MDM.APISamples\Workflow\Email\SendEmail.cs" region="SendMailUsingWorkflowDetails" />
        /// <code language="c#" title="Get Multiple Entities"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetMultipleEntities"/>          
        /// </example>
        public IOperationResult SendMailWithWorkflowDetails(Collection<Int64> entityIdList, IEmailContext iEmailContext,
                                                            String workflowName, ICallerContext iCallerContext, Boolean consolidateAllEntitiesMessages)
        {
            return MakeServiceCall<IOperationResult>("SendMailWithWorkflowDetails", "SendMailWithWorkflowDetailsUsingMailContext",
                        client => client.SendMailWithWorkflowDetailsUsingMailContext(entityIdList, (EmailContext)iEmailContext, workflowName,
                                                                                        FillDiagnosticTraces(iCallerContext), consolidateAllEntitiesMessages));
        }

        /// <summary>
        /// Sends the email using email context
        /// </summary>
        /// <param name="iEmailContext">Indicates the email context details</param>
        /// <param name="iCallerContext">Indicates the caller context details</param>
        /// <returns>Results whether the email has been sent or not</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Send email using email context" source="..\MDM.APISamples\Workflow\Email\SendEmail.cs" region="SendMailUsingMailContext" />
        /// </example>
        public IOperationResult SendMail(IEmailContext iEmailContext, ICallerContext iCallerContext)
        {
            return MakeServiceCall<IOperationResult>("SendMail", "SendGenericMail",
                        client => client.SendGenericMail((EmailContext)iEmailContext, FillDiagnosticTraces(iCallerContext)));
        }
		
        /// <summary>
        /// Sends the email using template including entities ids, template name, MDMCenter users, email ids, and the caller context details
        /// </summary>
        /// <param name="entityIdList">Indicates the collection of entities</param>
        /// <param name="mailTemplateName">Indicates the name of the email template</param>
        /// <param name="toMDMUsers">Indicates the list of MDMCenter user login names (To send a email)</param>
        /// <param name="toMailIds">Indicates the list of receiver email addresses</param>
        /// <param name="ccMDMUsers">Indicates the list of MDMCenter user login names (To send as a CC email)</param>
        /// <param name="ccMailIds">Indicates the list of CC email addresses</param>
        /// <param name="iCallerContext">Indicates the caller context details</param>
        /// <param name="consolidateAllEntitiesMessages">Indicates whether to consolidate all messages or not</param>
        /// <returns>Results whether the email has been sent to an recipient or not</returns>
        /// <exception cref="MDMExceptionDetails">Thrown when operation fails</exception>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDMCenter operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IOperationResult SendMail(Collection<Int64> entityIdList, String mailTemplateName, Collection<String> toMDMUsers, Collection<String> toMailIds, 
                                        Collection<String> ccMDMUsers, Collection<String> ccMailIds, ICallerContext iCallerContext, Boolean consolidateAllEntitiesMessages)
        {
            return MakeServiceCall<IOperationResult>("SendMail", "SendMail",
                        client => client.SendMail(entityIdList, mailTemplateName, toMDMUsers, toMailIds, ccMDMUsers, ccMailIds, 
                                                    FillDiagnosticTraces(iCallerContext), consolidateAllEntitiesMessages));
        }

        /// <summary>
        /// Sends the email using template including entities ids, email context, and caller context
        /// </summary>
        /// <param name="entityIdList">Indicates the collection of entities</param>
        /// <param name="iEmailContext">Indicates the email context details</param>
        /// <param name="iCallerContext">Indicates the caller context details such as application name and module name</param>
        /// <param name="consolidateAllEntitiesMessages">Indicates whether to consolidate all messages or not</param>
        /// <returns>Results whether the email has been sent or not</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Send email using email context and entities ids" source="..\MDM.APISamples\Workflow\Email\SendEmail.cs" region="SendMailUsingMailContextAndEntityList" />
        /// <code language="c#" title="Get Multiple Entities"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetMultipleEntities"/>          
        /// </example>
        public IOperationResult SendMail(Collection<Int64> entityIdList, IEmailContext iEmailContext, ICallerContext iCallerContext, Boolean consolidateAllEntitiesMessages)
        {
            return MakeServiceCall<IOperationResult>("SendMail", "SendMailWithContext",
                        client => client.SendMailWithContext(entityIdList, (EmailContext)iEmailContext, FillDiagnosticTraces(iCallerContext), consolidateAllEntitiesMessages));
        }

        /// <summary>
        /// Sends the email using the template with workflow details as per the business rule context
        /// </summary>
        /// <example>
        /// <code title="Send email using the template with workflow details as per the business rule context">
        /// // Create empty Workflow Business Rule Context
        /// IWorkflowBusinessRuleContext iWorkflowBusinessRuleContext = new WorkflowBusinessRuleContext(new WorkflowDataContext());
        /// String mailTemplateName = "MailTemplate";
        /// SecurityUser securityUser = new SecurityUser() {UserName = "Test"}; // Create fake security user
        /// Collection<![CDATA[<String>]]> toMDMUsers = new Collection<![CDATA[<String>]]>() { "User1", "User2" }; // User names to send email to
        /// Collection<![CDATA[<String>]]> toMailIds = new Collection<![CDATA[<String>]]>() { "user1@riversand.com", "user2@riversand.com" }; // User emails to send email to
        /// Collection<![CDATA[<String>]]> ccMDMUsers = new Collection<![CDATA[<String>]]>() { "User3", "User4" }; // User names to be included into CC
        /// Collection<![CDATA[<String>]]> ccMailIds = new Collection<![CDATA[<String>]]>() { "user3@riversand.com", "user4@riversand.com" }; // User emails to be included into CC
        /// Boolean consolidateAllEntitiesMessages = false; // Not to consolidate messages
        /// 
        /// // Set caller context
        /// ICallerContext callerContext = MDMObjectFactory.GetICallerContext();
        /// callerContext.Application = MDMCenterApplication.PIM;
        /// callerContext.Module = MDMCenterModules.Entity;
        /// 
        /// MessageService messageService = InitializeMessageServiceWithUserNameAndPassword();
        /// 
        /// IOperationResult result = messageService.SendMailWithWorkflowDetails(iWorkflowBusinessRuleContext, mailTemplateName, securityUser, toMDMUsers, toMailIds, ccMDMUsers, ccMailIds, callerContext, consolidateAllEntitiesMessages);
        /// </code>
        /// </example>
        /// <param name="iWorkflowBusinessRuleContext">Indicates the workflow business rule context to be used for the template with workflow details</param>
        /// <param name="mailTemplateName">Indicates the name of the mail template</param>
        /// <param name="assignedUserOfActivityInContext">Indicates the user assigned in the activity in context</param>
        /// <param name="toMDMUsers">Indicates MDMCenter user login names to send an email to</param>
        /// <param name="toMailIds">Indicates receiver email addresses</param>
        /// <param name="ccMDMUsers">Indicates MDMCenter user login names to send as a CC email to</param>
        /// <param name="ccMailIds">Indicates CC email addresses</param>
        /// <param name="iCallerContext">Indicates application and module name by which action is being performed</param>
        /// <param name="consolidateAllEntitiesMessages">Indicates whether to consolidate entities messages or not</param>
        /// <returns>Results whether the email has been sent or not</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IOperationResult SendMailWithWorkflowDetails(IWorkflowBusinessRuleContext iWorkflowBusinessRuleContext, String mailTemplateName, 
                                ISecurityUser assignedUserOfActivityInContext, Collection<String> toMDMUsers, Collection<String> toMailIds, 
                                Collection<String> ccMDMUsers, Collection<String> ccMailIds, ICallerContext iCallerContext, Boolean consolidateAllEntitiesMessages)
        {
            return MakeServiceCall<IOperationResult>("SendMailWithWorkflowDetails", "SendMailWithWorkflowDetailsUsingBRContext", 
                        client => client.SendMailWithWorkflowDetailsUsingBRContext((WorkflowBusinessRuleContext)iWorkflowBusinessRuleContext, mailTemplateName, 
                                        (SecurityUser)assignedUserOfActivityInContext, toMDMUsers, toMailIds, ccMDMUsers, ccMailIds, 
                                        FillDiagnosticTraces(iCallerContext), consolidateAllEntitiesMessages));
        }

        
        /// <summary>
        /// Sends the email using template with workflow details as per the business rule context
        /// </summary>
        /// <param name="iWorkflowBusinessRuleContext">Indicates the workflow business rule context details</param>        
        /// <param name="assignedUserOfActivityInContext">Indicates the assigned user of activity</param>
        /// <param name="iEmailContext">Indicates the email context details</param>
        /// <param name="iCallerContext">Indicates the caller context details</param>
        /// <param name="consolidateAllEntitiesMessages">Indicates whether consolidate all messages or not</param>
        /// <example>
        /// <code language="c#" title="Send email as per the business rule context" source="..\MDM.APISamples\Workflow\Email\SendEmail.cs" region="SendMailUsingWorkflowBusinessRuleContext" />
        /// </example>
        /// <returns>Results whether the email has been sent or not</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IOperationResult SendMailWithWorkflowDetails(IWorkflowBusinessRuleContext iWorkflowBusinessRuleContext,
                                                           ISecurityUser assignedUserOfActivityInContext, IEmailContext iEmailContext,
                                                           ICallerContext iCallerContext, Boolean consolidateAllEntitiesMessages)
        {
            return MakeServiceCall<IOperationResult>("SendMailWithWorkflowDetails", "SendMailWithWorkflowDetailsUsingBRContextAndMailContext", 
                        client => client.SendMailWithWorkflowDetailsUsingBRContextAndMailContext((WorkflowBusinessRuleContext)iWorkflowBusinessRuleContext, 
                               (SecurityUser)assignedUserOfActivityInContext, (EmailContext)iEmailContext, FillDiagnosticTraces(iCallerContext), consolidateAllEntitiesMessages));
        }
        
        #endregion

        #endregion

        #region Private methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IMessageService GetClient()
        {
            IMessageService serviceClient = null;

            if (WCFServiceInstanceLoader.IsLocalInstancesEnabled())
            {
                serviceClient = WCFServiceInstanceLoader.GetServiceInstance<IMessageService>();
            }

            if (serviceClient == null) //Means the given type is not implemented for local load..
            {
                MessageServiceProxy messageServiceProxy = null;

                if (String.IsNullOrEmpty(this.EndPointConfigurationName) && this.EndpointAddress == null)
                    messageServiceProxy = new MessageServiceProxy();
                else if (!String.IsNullOrEmpty(this.EndPointConfigurationName) && this.EndpointAddress == null)
                    messageServiceProxy = new MessageServiceProxy(this.EndPointConfigurationName);
                else if (!String.IsNullOrEmpty(this.EndPointConfigurationName) && this.EndpointAddress != null)
                    messageServiceProxy = new MessageServiceProxy(this.EndPointConfigurationName, this.EndpointAddress);

                if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    messageServiceProxy.ClientCredentials.UserName.UserName = this.UserName;
                    messageServiceProxy.ClientCredentials.UserName.Password = this.Password;
                    messageServiceProxy.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.None;
                }

                serviceClient = messageServiceProxy;
            }

            return serviceClient;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        private void DisposeClient(IMessageService client)
        {
            if (client == null)
                return;

            if (client.GetType().Equals(typeof(MessageServiceProxy)))
            {
                MessageServiceProxy serviceClient = (MessageServiceProxy)client;
                if (serviceClient.State == CommunicationState.Created || serviceClient.State == CommunicationState.Opened || serviceClient.State == CommunicationState.Opening)
                {
                    serviceClient.Close();
                }
                else if (serviceClient.State == CommunicationState.Faulted)
                {
                    serviceClient.Abort();
                }
            }
            else
            {
                //Do nothing...
            }
        }

        /// <summary>
        /// Makes the DataServiceClient call. Creates the client instance, executes call delegate against it in
        /// impersonated context (if necessary) and then disposes the client. Also emits traces when necessary.
        /// </summary>
        /// <typeparam name="TResult">Indicates type of the result of service call.</typeparam>
        /// <param name="clientMethodName">Indicates name of the client method to include in traces.</param>
        /// <param name="serverMethodName">Indicates name of the server method to include in traces.</param>
        /// <param name="call">Indicates call delegate to be executed against a client instance.</param>
        /// <param name="traceSource">Indicates name of the trace source</param>
        /// <returns>Returns value returned by service, or default.</returns>
        private TResult MakeServiceCall<TResult>(String clientMethodName, String serverMethodName, Func<IMessageService, TResult> call, 
                                                 MDMTraceSource traceSource = MDMTraceSource.APIFramework)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("MessageServiceClient." + clientMethodName, traceSource, false);

            TResult result = default(TResult);
            IMessageService messageServiceClient = null;

            try
            {
                messageServiceClient = GetClient();
                ValidateContext();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "MessageServiceClient sends '" + serverMethodName + "' request message.", traceSource);

                result = Impersonate(() => call(messageServiceClient));

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "MessageServiceClient receives '" + serverMethodName + "' response message.", traceSource);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                DisposeClient(messageServiceClient);

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("MessageServiceClient." + clientMethodName, traceSource);
            }
            return result;
        }

        #endregion
    }
}
