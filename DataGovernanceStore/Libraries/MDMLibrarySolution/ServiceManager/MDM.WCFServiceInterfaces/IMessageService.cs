using System;
using System.Collections.ObjectModel;
using System.Net.Mail;
using System.ServiceModel;
using MDM.BusinessObjects;
using MDM.BusinessObjects.Workflow;
using MDM.Core;

namespace MDM.WCFServiceInterfaces
{
    [ServiceContract(Namespace = "http://wcfservices.riversand.com")]
    public interface IMessageService
    {
        [OperationContract(Name = "CreateMessageUsingSmartTypes")]
        [FaultContract(typeof(MDMExceptionDetails))]
        void Create(MessageType Type, String From, String To, String Subject, String Body, MailPriority Priority, ref OperationResult operationResult);

        [OperationContract(Name = "CreateMessageUsingBasicTypes")]
        [FaultContract(typeof(MDMExceptionDetails))]
        void Create(String Type, String From, String To, String Subject, String Body, String Priority, ref OperationResult operationResult);

        [OperationContract(Name = "CreateMessage")]
        [FaultContract(typeof(MDMExceptionDetails))]
        void Create(Message Message, ref OperationResult operationResult);

        [OperationContract(Name = "SendWorkflowMessagesByWorkflowMDMObjectAndWFIDToSingleUser")]
        [FaultContract(typeof(MDMExceptionDetails))]
        void SendWorkflowMessages(Collection<WorkflowMDMObject> mdmObjectCollection, Int32 workflowID, Int32 workflowActivityID, Int32 userID, String templateName, ref OperationResult operationResult);

        [OperationContract(Name = "SendWorkflowMessagesByWorkflowMDMObjectAndWFIDToMultipleUsers")]
        [FaultContract(typeof(MDMExceptionDetails))]
        void SendWorkflowMessages(Collection<WorkflowMDMObject> mdmObjectCollection, Int32 workflowID, Int32 workflowActivityID, Collection<Int32> userIDs, String templateName, ref OperationResult operationResult);

        [OperationContract(Name = "SendWorkflowMessagesByWorkflowMDMObjectAndWFVersionToSingleUser")]
        [FaultContract(typeof(MDMExceptionDetails))]
        void SendWorkflowMessages(Collection<WorkflowMDMObject> mdmObjectCollection, Int32 workflowVersionID, String activityName, Int32 userID, String templateName, ref OperationResult operationResult);

        [OperationContract(Name = "SendWorkflowMessagesByWorkflowMDMObjectAndWFVersionToMultipleUsers")]
        [FaultContract(typeof(MDMExceptionDetails))]
        void SendWorkflowMessages(Collection<WorkflowMDMObject> mdmObjectCollection, Int32 workflowVersionID, String activityName, Collection<Int32> userIDs, String templateName, ref OperationResult operationResult);

        [OperationContract(Name = "SendWorkflowMessagesByMDMObjectIdListAndWFIDToSingleUserID")]
        [FaultContract(typeof(MDMExceptionDetails))]
        void SendWorkflowMessages(String MDMObjectIdList, String MDMObjectType, Int32 workflowID, Int32 workflowActivityID, Int32 userID, String templateName, ref OperationResult operationResult);

        [OperationContract(Name = "SendWorkflowMessagesByMDMObjectIdListAndWFIDToMultipleUserIDs")]
        [FaultContract(typeof(MDMExceptionDetails))]
        void SendWorkflowMessages(String MDMObjectIdList, String MDMObjectType, Int32 workflowID, Int32 workflowActivityID, Collection<Int32> userIDs, String templateName, ref OperationResult operationResult);

        [OperationContract(Name = "SendWorkflowMessagesByMDMObjectIdListAndWFIDToSingleUserLogin")]
        [FaultContract(typeof(MDMExceptionDetails))]
        void SendWorkflowMessages(String MDMObjectIdList, String MDMObjectType, Int32 workflowID, Int32 workflowActivityID, String userLogin, String templateName, ref OperationResult operationResult);

        [OperationContract(Name = "SendWorkflowMessagesByMDMObjectIdListAndWFIDToMultipleUserLogins")]
        [FaultContract(typeof(MDMExceptionDetails))]
        void SendWorkflowMessages(String MDMObjectIdList, String MDMObjectType, Int32 workflowID, Int32 workflowActivityID, Collection<String> userLogins, String templateName, ref OperationResult operationResult);

        [OperationContract(Name = "SendWorkflowMessagesByMDMObjectIdListAndWFVersionToSingleUserID")]
        [FaultContract(typeof(MDMExceptionDetails))]
        void SendWorkflowMessages(String MDMObjectIdList, String MDMObjectType, Int32 workflowVersionID, String activityName, Int32 userID, String templateName, ref OperationResult operationResult);

        [OperationContract(Name = "SendWorkflowMessagesByMDMObjectIdListAndWFVersionToMultipleUserIDs")]
        [FaultContract(typeof(MDMExceptionDetails))]
        void SendWorkflowMessages(String MDMObjectIdList, String MDMObjectType, Int32 workflowVersionID, String activityName, Collection<Int32> userIDs, String templateName, ref OperationResult operationResult);

        [OperationContract(Name = "SendWorkflowMessagesByMDMObjectIdListAndWFVersionToSingleUserLogin")]
        [FaultContract(typeof(MDMExceptionDetails))]
        void SendWorkflowMessages(String MDMObjectIdList, String MDMObjectType, Int32 workflowVersionID, String activityName, String userLogin, String templateName, ref OperationResult operationResult);

        [OperationContract(Name = "SendWorkflowMessagesByMDMObjectIdListAndWFVersionToMultipleUserLogins")]
        [FaultContract(typeof(MDMExceptionDetails))]
        void SendWorkflowMessages(String MDMObjectIdList, String MDMObjectType, Int32 workflowVersionID, String activityName, Collection<String> userLogins, String templateName, ref OperationResult operationResult);

        [OperationContract(Name = "SendWorkflowMessagesByWorkflowInstances")]
        [FaultContract(typeof(MDMExceptionDetails))]
        void SendWorkflowMessages(Collection<WorkflowInstance> workflowInstances, String templateName, ref OperationResult operationResult);

        [OperationContract(Name = "SendWorkflowMessagesByInstanceIdList")]
        [FaultContract(typeof(MDMExceptionDetails))]
        void SendWorkflowMessages(String InstanceIdList, String templateName, ref OperationResult operationResult);

        [OperationContract(Name = "SendWorkflowMessagesByEscalationCollection")]
        [FaultContract(typeof(MDMExceptionDetails))]
        void SendWorkflowMessages(Collection<Escalation> escalationCollection, String templateName, ref OperationResult operationResult);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult SendMailWithWorkflowDetails(Collection<Int64> entityIdList, String mailTemplateName, Collection<String> toMDMUsers, Collection<String> toMailIds, Collection<String> ccMDMUsers, Collection<String> ccMailIds, String workflowName, CallerContext callerContext, Boolean consolidateAllEntitiesMessages);
        
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult SendMailWithWorkflowDetailsUsingMailContext(Collection<Int64> entityIdList, EmailContext emailContext,
                                                           String workflowName, CallerContext callerContext, Boolean consolidateAllEntitiesMessages);
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult SendGenericMail(EmailContext emailContext, CallerContext callerContext);
		
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult SendMail(Collection<Int64> entityIdList, String mailTemplateName, Collection<String> toMDMUsers, Collection<String> toMailIds, Collection<String> ccMDMUsers, Collection<String> ccMailIds, CallerContext callerContext, Boolean consolidateAllEntitiesMessages);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult SendMailWithContext(Collection<Int64> entityIdList, EmailContext emailContext, CallerContext callerContext, Boolean consolidateAllEntitiesMessages);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult SendMailWithWorkflowDetailsUsingBRContext(WorkflowBusinessRuleContext workflowBusinessRuleContext, String mailTemplateName, SecurityUser assignedUserOfActivityInContext, Collection<String> toMDMUsers, Collection<String> toMailIds, Collection<String> ccMDMUsers, Collection<String> ccMailIds, CallerContext callerContext, Boolean consolidateAllEntitiesMessages);
        
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult SendMailWithWorkflowDetailsUsingBRContextAndMailContext(WorkflowBusinessRuleContext workflowBusinessRuleContext, SecurityUser assignedUserOfActivityInContext,
                                                                         EmailContext emailContext,
                                                                         CallerContext callerContext, Boolean consolidateAllEntitiesMessages);
    }
}
