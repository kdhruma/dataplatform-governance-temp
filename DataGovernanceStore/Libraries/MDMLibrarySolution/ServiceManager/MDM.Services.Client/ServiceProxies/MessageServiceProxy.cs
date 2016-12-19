using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace MDM.Services.ServiceProxies
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.ExceptionManager;
    using MDM.WCFServiceInterfaces;
    using MDM.Services.MessageServiceClient;

    /// <summary>
    /// Represents class for message service proxy
    /// </summary>
    public class MessageServiceProxy : MessageServiceClient, MDM.WCFServiceInterfaces.IMessageService
    {
        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public MessageServiceProxy()
        { 
        
        }

        /// <summary>
        /// Constructor with endpoint configuration name
        /// </summary>
        /// <param name="endpointConfigurationName">Indicates the endpoint configuration name</param>
        public MessageServiceProxy(String endpointConfigurationName) 
            : base(endpointConfigurationName) 
        {
        }

        /// <summary>
        /// Constructor with endpoint configuration name and remote address
        /// </summary>
        /// <param name="endpointConfigurationName">Indicates the endpoint configuration name</param>
        /// <param name="remoteAddress">Indicates the remote address</param>
        public MessageServiceProxy(String endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) 
            : base(endpointConfigurationName, remoteAddress)
        {
        }

        #endregion

        #region IMessageService Members

        ///<summary>
        /// Creates message based on type, subject, body and priority
        ///</summary>
        ///<param name="Type">Indicates type of the message</param>
        ///<param name="From">Indicates from message</param>
        ///<param name="To">Indicates to message</param>
        ///<param name="Subject">Indicates subject of message</param>
        ///<param name="Body">Indicates body of the message</param>
        ///<param name="Priority">Indicates the priority of the message</param>
        ///<param name="operationResult">Indicates operation result as output parameter</param>
        public void Create(MessageType Type, string From, string To, string Subject, string Body, System.Net.Mail.MailPriority Priority, ref OperationResult operationResult)
        {
            base.CreateMessageUsingSmartTypes(Type, From, To, Subject, Body, Priority, ref operationResult);    
        }

        ///<summary>
        /// Creates message based on type, subject, body and priority
        ///</summary>
        ///<param name="Type">Indicates type of the message</param>
        ///<param name="From">Indicates from message</param>
        ///<param name="To">Indicates to message</param>
        ///<param name="Subject">Indicates subject of message</param>
        ///<param name="Body">Indicates body of the message</param>
        ///<param name="Priority">Indicates the priority of the message</param>
        ///<param name="operationResult">Indicates operation result as output parameter</param>
        public void Create(string Type, string From, string To, string Subject, string Body, string Priority, ref OperationResult operationResult)
        {
            base.CreateMessageUsingBasicTypes(Type, From, To, Subject, Body, Priority, ref operationResult);
        }

        /// <summary>
        /// Creates message
        /// </summary>
        /// <param name="Message">Indicates message</param>
        /// <param name="operationResult">Indicates operation result as output parameter</param>
        public void Create(Message Message, ref OperationResult operationResult)
        {
            base.CreateMessage(Message, ref operationResult);
        }

        ///<summary>
        /// Send workflow messages to single user by MDMObject and workflow identifier
        ///</summary>
        ///<param name="mdmObjectCollection">Indicates collection of MDMObject</param>
        ///<param name="workflowID">Indicates identifier of workflow</param>
        ///<param name="workflowActivityID">Indicates activity identifier of workflow</param>
        ///<param name="userID">Indicates user identifier</param>
        ///<param name="templateName">Indicates name of the template</param>
        ///<param name="operationResult">Indicates operation result as output parameter</param>
        public void SendWorkflowMessages(Collection<BusinessObjects.Workflow.WorkflowMDMObject> mdmObjectCollection, int workflowID, int workflowActivityID, int userID, string templateName, ref OperationResult operationResult)
        {
            base.SendWorkflowMessagesByWorkflowMDMObjectAndWFIDToSingleUser(mdmObjectCollection, workflowID, workflowActivityID, userID, templateName, ref operationResult);
        }

        ///<summary>
        /// Send workflow messages to multiple user by MDMObject and workflow identifier
        ///</summary>
        ///<param name="mdmObjectCollection">Indicates collection of MDMObject</param>
        ///<param name="workflowID">Indicates identifier of workflow</param>
        ///<param name="workflowActivityID">Indicates activity identifier of workflow</param>
        ///<param name="userIDs">Indicates collection of user identifiers</param>
        ///<param name="templateName">Indicates name of the template</param>
        ///<param name="operationResult">Indicates operation result as output parameter</param>
        public void SendWorkflowMessages(Collection<BusinessObjects.Workflow.WorkflowMDMObject> mdmObjectCollection, int workflowID, int workflowActivityID, Collection<int> userIDs, string templateName, ref OperationResult operationResult)
        {
            base.SendWorkflowMessagesByWorkflowMDMObjectAndWFIDToMultipleUsers(mdmObjectCollection, workflowID, workflowActivityID, userIDs, templateName, ref operationResult);
        }

        ///<summary>
        /// Send workflow messages to single user by MDMObject and workFlow version
        ///</summary>
        ///<param name="mdmObjectCollection">Indicates collection of MDMObject</param>
        ///<param name="workflowVersionID">Indicates version identifier of workflow</param>
        ///<param name="activityName">Indicates activity name of workflow</param>
        ///<param name="userID">Indicates user identifier</param>
        ///<param name="templateName">Indicates name of the template</param>
        ///<param name="operationResult">Indicates operation result as output parameter</param>
        public void SendWorkflowMessages(Collection<BusinessObjects.Workflow.WorkflowMDMObject> mdmObjectCollection, int workflowVersionID, string activityName, int userID, string templateName, ref OperationResult operationResult)
        {
            base.SendWorkflowMessagesByWorkflowMDMObjectAndWFVersionToSingleUser(mdmObjectCollection, workflowVersionID, activityName, userID, templateName, ref operationResult);
        }

        ///<summary>
        /// Send workflow messages to multiple user by MDMObject and workFlow version
        ///</summary>
        ///<param name="mdmObjectCollection">Indicates collection of MDMObject</param>
        ///<param name="workflowVersionID">Indicates version identifier of workflow</param>
        ///<param name="activityName">Indicates activity name of workflow</param>
        ///<param name="userIDs">Indicates collection of user identifiers</param>
        ///<param name="templateName">Indicates name of the template</param>
        ///<param name="operationResult">Indicates operation result as output parameter</param>
        public void SendWorkflowMessages(Collection<BusinessObjects.Workflow.WorkflowMDMObject> mdmObjectCollection, int workflowVersionID, string activityName, Collection<int> userIDs, string templateName, ref OperationResult operationResult)
        {
            base.SendWorkflowMessagesByWorkflowMDMObjectAndWFVersionToMultipleUsers(mdmObjectCollection, workflowVersionID, activityName, userIDs, templateName, ref operationResult);
        }

        ///<summary>
        /// Send workflow messages by MDMObject identifier list and workFlow identifier to single user identifier
        ///</summary>
        ///<param name="MDMObjectIdList">Indicates list of MDMObject identifiers</param>
        ///<param name="MDMObjectType">Indicates type of MDMObject</param>	
        ///<param name="workflowID">Indicates identifier of workflow</param>
        ///<param name="workflowActivityID">Indicates activity identifier of workflow</param>
        ///<param name="userID">Indicates user identifier</param>
        ///<param name="templateName">Indicates name of the template</param>
        ///<param name="operationResult">Indicates operation result as output parameter</param>
        public void SendWorkflowMessages(string MDMObjectIdList, string MDMObjectType, int workflowID, int workflowActivityID, int userID, string templateName, ref OperationResult operationResult)
        {
            base.SendWorkflowMessagesByMDMObjectIdListAndWFIDToSingleUserID(MDMObjectIdList, MDMObjectType, workflowID, workflowActivityID, userID, templateName, ref operationResult);
        }

        ///<summary>
        /// Send workflow messages by MDMObject identifier list and workflow identifier to multiple user identifiers
        ///</summary>
        ///<param name="MDMObjectIdList">Indicates list of MDMObject identifiers</param>
        ///<param name="MDMObjectType">Indicates type of MDMObject</param>	
        ///<param name="workflowID">Indicates identifier of workflow</param>
        ///<param name="workflowActivityID">Indicates activity identifier of workflow</param>
        ///<param name="userIDs">Indicates collection of user identifiers</param>
        ///<param name="templateName">Indicates name of the template</param>
        ///<param name="operationResult">Indicates operation result as output parameter</param>
        public void SendWorkflowMessages(string MDMObjectIdList, string MDMObjectType, int workflowID, int workflowActivityID, Collection<int> userIDs, string templateName, ref OperationResult operationResult)
        {
            base.SendWorkflowMessagesByMDMObjectIdListAndWFIDToMultipleUserIDs(MDMObjectIdList, MDMObjectType, workflowID, workflowActivityID, userIDs, templateName, ref operationResult);
        }

        ///<summary>
        /// Send workflow messages by MDMObject identifier list and workflow identifier to single user login
        ///</summary>
        ///<param name="MDMObjectIdList">Indicates list of MDMObject identifiers</param>
        ///<param name="MDMObjectType">Indicates type of MDMObject</param>	
        ///<param name="workflowID">Indicates identifier of workflow</param>
        ///<param name="workflowActivityID">Indicates activity identifier of workflow</param>
        ///<param name="userLogin">Indicates login name of the user</param>
        ///<param name="templateName">Indicates name of the template</param>
        ///<param name="operationResult">Indicates operation result as output parameter</param>
        public void SendWorkflowMessages(string MDMObjectIdList, string MDMObjectType, int workflowID, int workflowActivityID, string userLogin, string templateName, ref OperationResult operationResult)
        {
            base.SendWorkflowMessagesByMDMObjectIdListAndWFIDToSingleUserLogin(MDMObjectIdList, MDMObjectType, workflowID, workflowActivityID, userLogin, templateName, ref operationResult);
        }

        ///<summary>
        /// Send workflow messages by MDMObject identifier list and workflow identifier to multiple user login
        ///</summary>
        ///<param name="MDMObjectIdList">Indicates list of MDMObject identifiers</param>
        ///<param name="MDMObjectType">Indicates type of MDMObject</param>	
        ///<param name="workflowID">Indicates identifier of workflow</param>
        ///<param name="workflowActivityID">Indicates activity identifier of workflow</param>
        ///<param name="userLogins">Indicates collection of login names of the users</param>
        ///<param name="templateName">Indicates name of the template</param>
        ///<param name="operationResult">Indicates operation result as output parameter</param>
        public void SendWorkflowMessages(string MDMObjectIdList, string MDMObjectType, int workflowID, int workflowActivityID, Collection<string> userLogins, string templateName, ref OperationResult operationResult)
        {
            base.SendWorkflowMessagesByMDMObjectIdListAndWFIDToMultipleUserLogins(MDMObjectIdList, MDMObjectType, workflowID, workflowActivityID, userLogins, templateName, ref operationResult);
        }

        ///<summary>
        /// Send workflow messages by MDMObject identifier list and workFlow version to single user identifier
        ///</summary>
        ///<param name="MDMObjectIdList">Indicates list of MDMObject identifiers</param>
        ///<param name="MDMObjectType">Indicates type of MDMObject</param>	       
        ///<param name="workflowVersionID">Indicates version identifier of workflow</param>
        ///<param name="activityName">Indicates activity name of the workflow</param>
        ///<param name="userID">Indicates user identifier</param>
        ///<param name="templateName">Indicates name of the template</param>
        ///<param name="operationResult">Indicates operation result as output parameter</param>
        public void SendWorkflowMessages(string MDMObjectIdList, string MDMObjectType, int workflowVersionID, string activityName, int userID, string templateName, ref OperationResult operationResult)
        {
            base.SendWorkflowMessagesByMDMObjectIdListAndWFVersionToSingleUserID(MDMObjectIdList, MDMObjectType, workflowVersionID, activityName, userID, templateName, ref operationResult);
        }

        ///<summary>
        /// Send workflow messages by MDMObject identifier list and workFlow version to multiple user identifiers
        ///</summary>
        ///<param name="MDMObjectIdList">Indicates list of MDMObject identifiers</param>
        ///<param name="MDMObjectType">Indicates type of MDMObject</param>	       
        ///<param name="workflowVersionID">Indicates version identifier of workflow</param>
        ///<param name="activityName">Indicates activity name of the workflow</param>
        ///<param name="userIDs">Indicates collection of user identifiers</param>
        ///<param name="templateName">Indicates name of the template</param>
        ///<param name="operationResult">Indicates operation result as output parameter</param>
        public void SendWorkflowMessages(string MDMObjectIdList, string MDMObjectType, int workflowVersionID, string activityName, Collection<int> userIDs, string templateName, ref OperationResult operationResult)
        {
            base.SendWorkflowMessagesByMDMObjectIdListAndWFVersionToMultipleUserIDs(MDMObjectIdList, MDMObjectType, workflowVersionID, activityName, userIDs, templateName, ref operationResult);
        }

        ///<summary>
        /// Send workflow messages by MDMObject identifier list and workFlow version to single user login
        ///</summary>
        ///<param name="MDMObjectIdList">Indicates list of MDMObject identifiers</param>
        ///<param name="MDMObjectType">Indicates type of MDMObject</param>	        
        ///<param name="workflowVersionID">Indicates version identifier of workflow</param>
        ///<param name="activityName">Indicates activity name of the workflow</param>
        ///<param name="userLogin">Indicates login name of the user</param>
        ///<param name="templateName">Indicates name of the template</param>
        ///<param name="operationResult">Indicates operation result as output parameter</param>
        public void SendWorkflowMessages(string MDMObjectIdList, string MDMObjectType, int workflowVersionID, string activityName, string userLogin, string templateName, ref OperationResult operationResult)
        {
            base.SendWorkflowMessagesByMDMObjectIdListAndWFVersionToSingleUserLogin(MDMObjectIdList, MDMObjectType, workflowVersionID, activityName, userLogin, templateName, ref operationResult);
        }

        ///<summary>
        /// Send workflow messages by MDMObject identifier list and workFlow version to multiple user login
        ///</summary>
        ///<param name="MDMObjectIdList">Indicates list of MDMObject identifiers</param>
        ///<param name="MDMObjectType">Indicates type of MDMObject</param>	        
        ///<param name="workflowVersionID">Indicates version identifier of workflow</param>
        ///<param name="activityName">Indicates activity name of the workflow</param>
        ///<param name="userLogins">Indicates collection of login names of the users</param>
        ///<param name="templateName">Indicates name of the template</param>
        ///<param name="operationResult">Indicates operation result as output parameter</param>
        public void SendWorkflowMessages(string MDMObjectIdList, string MDMObjectType, int workflowVersionID, string activityName, Collection<string> userLogins, string templateName, ref OperationResult operationResult)
        {
            base.SendWorkflowMessagesByMDMObjectIdListAndWFVersionToMultipleUserLogins(MDMObjectIdList, MDMObjectType, workflowVersionID, activityName, userLogins, templateName, ref operationResult);
        }

        ///<summary>
        /// Send workflow messages by workflow instances
        ///</summary>
        ///<param name="workflowInstances">Indicates collection of workflow instance</param>
        ///<param name="templateName">Indicates name of the template</param>
        ///<param name="operationResult">Indicates operation result as output parameter</param>
        public void SendWorkflowMessages(Collection<BusinessObjects.Workflow.WorkflowInstance> workflowInstances, string templateName, ref OperationResult operationResult)
        {
            base.SendWorkflowMessagesByWorkflowInstances(workflowInstances, templateName, ref operationResult);
        }

        ///<summary>
        /// Send workflow messages by instance id list
        ///</summary>
        ///<param name="InstanceIdList">Indicates list of instance identifier</param>
        ///<param name="templateName">Indicates name of the template</param>
        ///<param name="operationResult">Indicates operation result as output parameter</param>
        public void SendWorkflowMessages(string InstanceIdList, string templateName, ref OperationResult operationResult)
        {
            base.SendWorkflowMessagesByInstanceIdList(InstanceIdList, templateName, ref operationResult);
        }

        ///<summary>
        /// Send workflow messages by collection of escalation
        ///</summary>
        ///<param name="escalationCollection">Indicates collection of escalation</param>
        ///<param name="templateName">Indicates name of the template</param>
        ///<param name="operationResult">Indicates operation result as output parameter</param>
        public void SendWorkflowMessages(Collection<BusinessObjects.Workflow.Escalation> escalationCollection, string templateName, ref OperationResult operationResult)
        {
            base.SendWorkflowMessagesByEscalationCollection(escalationCollection, templateName, ref operationResult);
        }

        #endregion
    }
}
