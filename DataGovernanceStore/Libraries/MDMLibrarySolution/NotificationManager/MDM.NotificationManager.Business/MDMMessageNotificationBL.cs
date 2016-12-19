using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
using System.Xml;

namespace MDM.NotificationManager.Business
{
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Workflow;
    using MDM.NotificationManager.Data;
    using MDM.LookupManager.Business;
    using MDM.MessageManager.Business;
    using MDM.EntityManager.Business;

    /// <summary>
    /// Specifies the business operations for MDM Message Notifications
    /// </summary>
	public class MDMMessageNotificationBL : BusinessLogicBase
    {
        #region Fields

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes MDMMessageNotificationBL instance
        /// </summary>
        public MDMMessageNotificationBL()
        {

        }

        #endregion

        #region Methods

        public void SendWorkflowMessages(Collection<WorkflowMDMObject> mdmObjectCollection, Int32 workflowID, Int32 workflowActivityID, Int32 userID, String templateName, OperationResult operationResult)
		{
			Collection<Int32> userIDs = new Collection<int>();
			userIDs.Add(userID);
			SendWorkflowMessages(mdmObjectCollection, workflowID, workflowActivityID, userIDs, templateName, operationResult);
		}
		public void SendWorkflowMessages(Collection<WorkflowMDMObject> mdmObjectCollection, Int32 workflowID, Int32 workflowActivityID, Collection<Int32> userIDs, String templateName, OperationResult operationResult)
		{
            MDMMessageNotificationDA messageNotificationDA = new MDMMessageNotificationDA();

			MessageTemplate messageTemplate = messageNotificationDA.GetMessageTemplateByName(templateName);

			if (messageTemplate == null || messageTemplate.Disabled)
				return;

			String MDMObjectIdList = mdmObjectCollection.Aggregate(String.Empty, (current, workflowMdmObject) => current + workflowMdmObject.MDMObjectId + ",");

			String userIDList = userIDs.Aggregate(String.Empty, (current, userID) => current + userID + ",");

			XmlDocument xmlDocument = messageNotificationDA.GetMDMObjectMessagingInfoByMDMObjectIDs(workflowID, 0, workflowActivityID, null, userIDList, null, mdmObjectCollection[0].MDMObjectType, MDMObjectIdList, messageTemplate.ReturnAttributes);

			XmlNode MDMObjectDataNode = xmlDocument.SelectNodes("MDMObjectData")[0];

			GenerateAndSendMessages(messageTemplate, MDMObjectDataNode, messageTemplate.MessageType, messageTemplate.Priority, messageTemplate.ConsolidateMessages, operationResult);
		}

		public void SendWorkflowMessages(Collection<WorkflowMDMObject> mdmObjectCollection, Int32 workflowVersionID, String activityName, Int32 userID, String templateName, OperationResult operationResult)
		{
			Collection<Int32> userIDs = new Collection<int>();
			userIDs.Add(userID);
			SendWorkflowMessages(mdmObjectCollection, workflowVersionID, activityName, userIDs, templateName, operationResult);
		}
		public void SendWorkflowMessages(Collection<WorkflowMDMObject> mdmObjectCollection, Int32 workflowVersionID, String activityName, Collection<Int32> userIDs, String templateName, OperationResult operationResult)
		{
            MDMMessageNotificationDA messageNotificationDA = new MDMMessageNotificationDA();

			MessageTemplate messageTemplate = messageNotificationDA.GetMessageTemplateByName(templateName);

			if (messageTemplate == null || messageTemplate.Disabled)
				return;

			String MDMObjectIdList = mdmObjectCollection.Aggregate(String.Empty, (current, workflowMdmObject) => current + workflowMdmObject.MDMObjectId + ",");

			String userIDList = userIDs.Aggregate(String.Empty, (current, userID) => current + userID + ",");

			XmlDocument xmlDocument = messageNotificationDA.GetMDMObjectMessagingInfoByMDMObjectIDs(0, workflowVersionID, 0, activityName, userIDList, null, mdmObjectCollection[0].MDMObjectType, MDMObjectIdList, messageTemplate.ReturnAttributes);

			XmlNode MDMObjectDataNode = xmlDocument.SelectNodes("MDMObjectData")[0];

			GenerateAndSendMessages(messageTemplate, MDMObjectDataNode, messageTemplate.MessageType, messageTemplate.Priority, messageTemplate.ConsolidateMessages, operationResult);
		}

		public void SendWorkflowMessages(String MDMObjectIdList, String MDMObjectType, Int32 workflowID, Int32 workflowActivityID, Int32 userID, String templateName, OperationResult operationResult)
		{
			Collection<Int32> userIDs = new Collection<Int32>();
			userIDs.Add(userID);
			SendWorkflowMessages(MDMObjectIdList, MDMObjectType, workflowID, workflowActivityID, userIDs, templateName, operationResult);
		}
		public void SendWorkflowMessages(String MDMObjectIdList, String MDMObjectType, Int32 workflowID, Int32 workflowActivityID, Collection<Int32> userIDs, String templateName, OperationResult operationResult)
		{
            MDMMessageNotificationDA messageNotificationDA = new MDMMessageNotificationDA();

			MessageTemplate messageTemplate = messageNotificationDA.GetMessageTemplateByName(templateName);

			if (messageTemplate == null || messageTemplate.Disabled)
				return;

			String userIDList = userIDs.Aggregate(String.Empty, (current, userID) => current + userID + ",");

			XmlDocument xmlDocument = messageNotificationDA.GetMDMObjectMessagingInfoByMDMObjectIDs(workflowID, 0, workflowActivityID, null, userIDList, null, MDMObjectType, MDMObjectIdList, messageTemplate.ReturnAttributes);

			XmlNode MDMObjectDataNode = xmlDocument.SelectNodes("MDMObjectData")[0];

			GenerateAndSendMessages(messageTemplate, MDMObjectDataNode, messageTemplate.MessageType, messageTemplate.Priority, messageTemplate.ConsolidateMessages, operationResult);
		}

		public void SendWorkflowMessages(String MDMObjectIdList, String MDMObjectType, Int32 workflowID, Int32 workflowActivityID, String userLogin, String templateName, OperationResult operationResult)
		{
			Collection<String> userIDs = new Collection<String>();
			userIDs.Add(userLogin);
			SendWorkflowMessages(MDMObjectIdList, MDMObjectType, workflowID, workflowActivityID, userIDs, templateName, operationResult);
		}
		public void SendWorkflowMessages(String MDMObjectIdList, String MDMObjectType, Int32 workflowID, Int32 workflowActivityID, Collection<String> userLogins, String templateName, OperationResult operationResult)
		{
            MDMMessageNotificationDA messageNotificationDA = new MDMMessageNotificationDA();

			MessageTemplate messageTemplate = messageNotificationDA.GetMessageTemplateByName(templateName);

			if (messageTemplate == null || messageTemplate.Disabled)
				return;

			String userLoginList = userLogins.Aggregate(String.Empty, (current, userID) => current + userID + ",");

			XmlDocument xmlDocument = messageNotificationDA.GetMDMObjectMessagingInfoByMDMObjectIDs(workflowID, 0, workflowActivityID, null, null, userLoginList, MDMObjectType, MDMObjectIdList, messageTemplate.ReturnAttributes);

			XmlNode MDMObjectDataNode = xmlDocument.SelectNodes("MDMObjectData")[0];

			GenerateAndSendMessages(messageTemplate, MDMObjectDataNode, messageTemplate.MessageType, messageTemplate.Priority, messageTemplate.ConsolidateMessages, operationResult);

			//Log exception
			//ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
		}

		public void SendWorkflowMessages(String MDMObjectIdList, String MDMObjectType, Int32 workflowVersionID, String activityName, Int32 userID, String templateName, OperationResult operationResult)
		{
			Collection<Int32> userIDs = new Collection<Int32>();
			userIDs.Add(userID);
			SendWorkflowMessages(MDMObjectIdList, MDMObjectType, workflowVersionID, activityName, userIDs, templateName, operationResult);
		}
		public void SendWorkflowMessages(String MDMObjectIdList, String MDMObjectType, Int32 workflowVersionID, String activityName, Collection<Int32> userIDs, String templateName, OperationResult operationResult)
		{
            MDMMessageNotificationDA messageNotificationDA = new MDMMessageNotificationDA();

			MessageTemplate messageTemplate = messageNotificationDA.GetMessageTemplateByName(templateName);

			if (messageTemplate == null || messageTemplate.Disabled)
				return;

			String userIDList = userIDs.Aggregate(String.Empty, (current, userID) => current + userID + ",");

			XmlDocument xmlDocument = messageNotificationDA.GetMDMObjectMessagingInfoByMDMObjectIDs(0, workflowVersionID, 0, activityName, userIDList, null, MDMObjectType, MDMObjectIdList, messageTemplate.ReturnAttributes);

			XmlNode MDMObjectDataNode = xmlDocument.SelectNodes("MDMObjectData")[0];

			GenerateAndSendMessages(messageTemplate, MDMObjectDataNode, messageTemplate.MessageType, messageTemplate.Priority, messageTemplate.ConsolidateMessages, operationResult);
		}

		public void SendWorkflowMessages(String MDMObjectIdList, String MDMObjectType, Int32 workflowVersionID, String activityName, String userLogin, String templateName, OperationResult operationResult)
		{
			Collection<String> userIDs = new Collection<String>();
			userIDs.Add(userLogin);
			SendWorkflowMessages(MDMObjectIdList, MDMObjectType, workflowVersionID, activityName, userIDs, templateName, operationResult);
		}
		public void SendWorkflowMessages(String MDMObjectIdList, String MDMObjectType, Int32 workflowVersionID, String activityName, Collection<String> userLogins, String templateName, OperationResult operationResult)
		{
            MDMMessageNotificationDA messageNotificationDA = new MDMMessageNotificationDA();

			MessageTemplate messageTemplate = messageNotificationDA.GetMessageTemplateByName(templateName);

			if (messageTemplate == null || messageTemplate.Disabled)
				return;

			String userLoginList = userLogins.Aggregate(String.Empty, (current, userID) => current + userID + ",");

			XmlDocument xmlDocument = messageNotificationDA.GetMDMObjectMessagingInfoByMDMObjectIDs(0, workflowVersionID, 0, activityName, null, userLoginList, MDMObjectType, MDMObjectIdList, messageTemplate.ReturnAttributes);

			XmlNode MDMObjectDataNode = xmlDocument.SelectNodes("MDMObjectData")[0];

			GenerateAndSendMessages(messageTemplate, MDMObjectDataNode, messageTemplate.MessageType, messageTemplate.Priority, messageTemplate.ConsolidateMessages, operationResult);

			//Log exception
			//ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
		}

		public void SendWorkflowMessages(Collection<WorkflowInstance> workflowInstances, String templateName, OperationResult operationResult)
		{
            MDMMessageNotificationDA messageNotificationDA = new MDMMessageNotificationDA();

			MessageTemplate messageTemplate = messageNotificationDA.GetMessageTemplateByName(templateName);

			if (messageTemplate == null || messageTemplate.Disabled)
				return;

			String RuntimeInstanceIdList = workflowInstances.Aggregate(String.Empty, (current, workflowInstance) => current + workflowInstance.RuntimeInstanceId + ",");

			XmlDocument xmlDocument = messageNotificationDA.GetMDMObjectMessagingInfoByInstanceIDs(RuntimeInstanceIdList, messageTemplate.ReturnAttributes);

			XmlNode MDMObjectDataNode = xmlDocument.SelectNodes("MDMObjectData")[0];

			GenerateAndSendMessages(messageTemplate, MDMObjectDataNode, messageTemplate.MessageType, messageTemplate.Priority, messageTemplate.ConsolidateMessages, operationResult);
		}

		public void SendWorkflowMessages(String InstanceIdList, String templateName, OperationResult operationResult)
		{
            MDMMessageNotificationDA messageNotificationDA = new MDMMessageNotificationDA();

			MessageTemplate messageTemplate = messageNotificationDA.GetMessageTemplateByName(templateName);

			if (messageTemplate == null || messageTemplate.Disabled)
				return;

			XmlDocument xmlDocument = messageNotificationDA.GetMDMObjectMessagingInfoByInstanceIDs(InstanceIdList, messageTemplate.ReturnAttributes);

			XmlNode MDMObjectDataNode = xmlDocument.SelectNodes("MDMObjectData")[0];

			GenerateAndSendMessages(messageTemplate, MDMObjectDataNode, messageTemplate.MessageType, messageTemplate.Priority, messageTemplate.ConsolidateMessages, operationResult);
		}

		public void SendWorkflowMessages(Collection<Escalation> escalationCollection, String templateName, OperationResult operationResult)
		{
            MDMMessageNotificationDA messageNotificationDA = new MDMMessageNotificationDA();

			String InstanceActivityList = escalationCollection.Aggregate("<InstanceActivityList>", (current, escalation) => current + "<InstanceActivity Instance=\"" + escalation.RuntimeInstanceId + "\" Activity=\"" + escalation.WorkflowActivityId + "\" />");
			InstanceActivityList = InstanceActivityList + "</InstanceActivityList>";

			MessageTemplate messageTemplate = messageNotificationDA.GetMessageTemplateByName(templateName);

			if (messageTemplate == null || messageTemplate.Disabled)
				return;

			XmlDocument xmlDocument = messageNotificationDA.GetMDMObjectMessagingInfoByEscalations(InstanceActivityList, messageTemplate.ReturnAttributes);

			XmlNode MDMObjectDataNode = xmlDocument.SelectNodes("MDMObjectData")[0];

			GenerateAndSendMessages(messageTemplate, MDMObjectDataNode, messageTemplate.MessageType, messageTemplate.Priority, messageTemplate.ConsolidateMessages, operationResult);
		}

        #region Private Methods

        private void GenerateAndSendMessages(MessageTemplate messageTemplate, XmlNode MDMObjectDataNodes, MessageType messageType, MailPriority messagePriority, Boolean consolidateMessages, OperationResult operationResult)
		{
			Dictionary<String, XmlNode> allUsers = MDMObjectDataNodes.SelectNodes("Users/User").Cast<XmlNode>().ToDictionary(userNode => userNode.Attributes["Id"].Value);

			Dictionary<String, XmlNode> allMDMObjects = MDMObjectDataNodes.SelectNodes("MDMObjects/MDMObject").Cast<XmlNode>().ToDictionary(MDMObjectNode => MDMObjectNode.Attributes["Id"].Value);

			XmlNodeList workflowNodes = MDMObjectDataNodes.SelectNodes("Workflows/Workflow");
			foreach (XmlNode workflowNode in workflowNodes)
			{
				XmlNodeList userNodes = workflowNode.SelectNodes("User");
				foreach (XmlNode userNode in userNodes)
				{
					XmlNode user;

					if (!allUsers.TryGetValue(userNode.Attributes["Id"].Value, out user))
						continue;

					XmlNodeList activityNodes = userNode.SelectNodes("Activity");
					foreach (XmlNode activityNode in activityNodes)
					{
						String MessageSubject = messageTemplate.MessageSubject;
						MessageSubject = MessageSubject.Replace("##UserName##", user.Attributes["FirstName"].Value + " " + user.Attributes["LastName"].Value);
						MessageSubject = MessageSubject.Replace("##WorkflowName##", workflowNode.Attributes["WorkflowName"].Value);
						MessageSubject = MessageSubject.Replace("##ActivityName##", activityNode.Attributes["ActivityName"].Value);

						String MessageBody = messageTemplate.MessageBody;
						MessageBody = MessageBody.Replace("##UserName##", user.Attributes["FirstName"].Value + " " + user.Attributes["LastName"].Value);
						MessageBody = MessageBody.Replace("##WorkflowName##", workflowNode.Attributes["WorkflowName"].Value);
						MessageBody = MessageBody.Replace("##ActivityName##", activityNode.Attributes["ActivityName"].Value);

						if (activityNode.Attributes["EscalatedFrom"] != null)
						{
							XmlNode escalatedFromUser;
							if (allUsers.TryGetValue(activityNode.Attributes["EscalatedFrom"].Value, out escalatedFromUser))
								MessageBody = MessageBody.Replace("##EscalatedFromUserName##", escalatedFromUser.Attributes["FirstName"].Value + " " + escalatedFromUser.Attributes["LastName"].Value);
						}

						XmlNodeList MDMObjectNodes = activityNode.SelectNodes("MDMObject");

						if (consolidateMessages)
						{
							String MDMObjectContent = String.Empty;

							foreach (XmlNode MDMObjectNode in MDMObjectNodes)
							{
								XmlNode MDMObjNode;

								if (!allMDMObjects.TryGetValue(MDMObjectNode.Attributes["Id"].Value, out MDMObjNode))
									continue;

								String MDMObjectContentTemplate = messageTemplate.MDMObjectContentTemplate;

								XmlNodeList AttrValNodes = MDMObjNode.SelectNodes("AttrVal");
								
								MDMObjectContentTemplate = AttrValNodes.Cast<XmlNode>().Aggregate(MDMObjectContentTemplate, (current, attrValNode) => current.Replace("##" + attrValNode.Attributes["Id"].Value + "##", attrValNode.Attributes["Value"].Value));

								MDMObjectContentTemplate = MDMObjectContentTemplate.Replace("##workflowActivityID##", activityNode.Attributes["Id"].Value);
								MDMObjectContentTemplate = MDMObjectContentTemplate.Replace("##CNodeId##", MDMObjNode.Attributes["Id"].Value);
								MDMObjectContentTemplate = MDMObjectContentTemplate.Replace("##MDMObjectType##", MDMObjNode.Attributes["MDMObjectType"].Value);

								MDMObjectContent = MDMObjectContent + MDMObjectContentTemplate;
							}

							Message message = new Message();
							message.MessageType = messageType;
							message.MessageFrom = "Workflow Messaging";
							message.MessageTo = user.Attributes["Login"].Value;
							message.Subject = MessageSubject;
							message.Body = MessageBody.Replace("##MDMObjectContentTemplate##", MDMObjectContent);
							message.Priority = messagePriority;
							message.NonRepliable = true;

							MessageBL messagebl = new MessageBL();
							messagebl.Create(message, operationResult);
						}
						else
						{
							foreach (XmlNode MDMObjectNode in MDMObjectNodes)
							{
								XmlNode MDMObjNode;

								if (!allMDMObjects.TryGetValue(MDMObjectNode.Attributes["Id"].Value, out MDMObjNode))
									continue;

								String MDMObjectContent = messageTemplate.MDMObjectContentTemplate;

								XmlNodeList AttrValNodes = MDMObjNode.SelectNodes("AttrVal");

								MDMObjectContent = AttrValNodes.Cast<XmlNode>().Aggregate(MDMObjectContent, (current, attrValNode) => current.Replace("##" + attrValNode.Attributes["Id"].Value + "##", attrValNode.Attributes["Value"].Value));

								MDMObjectContent = MDMObjectContent.Replace("##workflowActivityID##", activityNode.Attributes["Id"].Value);
								MDMObjectContent = MDMObjectContent.Replace("##CNodeId##", MDMObjNode.Attributes["Id"].Value);
								MDMObjectContent = MDMObjectContent.Replace("##MDMObjectType##", MDMObjNode.Attributes["MDMObjectType"].Value);

								Message message = new Message();
								message.MessageType = messageType;
								message.MessageFrom = "Workflow Messaging";
								message.MessageTo = user.Attributes["Login"].Value;
								message.Subject = MessageSubject;
								message.Body = MessageBody.Replace("##MDMObjectContentTemplate##", MDMObjectContent);
								message.Priority = messagePriority;
								message.NonRepliable = true;

								MessageBL messagebl = new MessageBL();
								messagebl.Create(message, operationResult);
							}
						}
					}
				}
			}
        }

        #endregion

        #endregion
    }
}
