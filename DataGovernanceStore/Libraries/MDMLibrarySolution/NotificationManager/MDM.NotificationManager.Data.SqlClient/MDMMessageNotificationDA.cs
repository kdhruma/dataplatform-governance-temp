using System;
using System.Text;
using System.Data.SqlClient;
using System.Xml;

namespace MDM.NotificationManager.Data
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Utility;

    public class MDMMessageNotificationDA : SqlClientDataAccessBase
    {
        public XmlDocument GetMDMObjectMessagingInfoByMDMObjectIDs(Int32 workflowID, Int32 workflowVersionID, Int32 workflowActivityID, String activityName, String userIDs, String userLogins, String MDMObjectType, String MDMObjectIdList, String returnAttributeList)
        {
            String connectionString = AppConfigurationHelper.ConnectionString;

            SqlParametersGenerator generator = new SqlParametersGenerator("MessageManager_SqlParameters");

            SqlParameter[] parameters = generator.GetParameters("Workflow_Messaging_MDMObjectData_GetByMDMObjectIDs_ParametersArray");

            parameters[0].Value = workflowID;
            parameters[1].Value = workflowVersionID;
            parameters[2].Value = workflowActivityID;
            parameters[3].Value = activityName;
            parameters[4].Value = userIDs;
            parameters[5].Value = userLogins;
            parameters[6].Value = MDMObjectType;
            parameters[7].Value = MDMObjectIdList;
            parameters[8].Value = returnAttributeList;

            String storedProcedureName = "usp_Workflow_Messaging_MDMObjectData_ByMDMObjectIDs_Get";

            Object returnValue = ExecuteProcedureScalar(connectionString, parameters, storedProcedureName);

            XmlDocument xmlDocument = new XmlDocument();
            if (returnValue != null)
            {
                xmlDocument.LoadXml(returnValue.ToString());
                return xmlDocument;
            }
            return null;
        }

        public XmlDocument GetMDMObjectMessagingInfoByInstanceIDs(String RuntimeInstanceIdList, String returnAttributeList)
        {
            String connectionString = AppConfigurationHelper.ConnectionString;

            SqlParametersGenerator generator = new SqlParametersGenerator("MessageManager_SqlParameters");

            SqlParameter[] parameters = generator.GetParameters("Workflow_Messaging_MDMObjectData_GetByInstanceIDs_ParametersArray");

            parameters[0].Value = RuntimeInstanceIdList;
            parameters[1].Value = returnAttributeList;

            String storedProcedureName = "usp_Workflow_Messaging_MDMObjectData_ByInstanceIDs_Get";

            Object returnValue = ExecuteProcedureScalar(connectionString, parameters, storedProcedureName);

            XmlDocument xmlDocument = new XmlDocument();
            if (returnValue != null)
            {
                xmlDocument.LoadXml(returnValue.ToString());
                return xmlDocument;
            }
            return null;
        }

        public XmlDocument GetMDMObjectMessagingInfoByEscalations(String InstanceActivityList, String returnAttributeList)
        {
            String connectionString = AppConfigurationHelper.ConnectionString;

            SqlParametersGenerator generator = new SqlParametersGenerator("MessageManager_SqlParameters");

            SqlParameter[] parameters = generator.GetParameters("Workflow_Messaging_MDMObjectData_GetByEscalations_ParametersArray");

            parameters[0].Value = InstanceActivityList;
            parameters[1].Value = returnAttributeList;

            String storedProcedureName = "usp_Workflow_Messaging_MDMObjectData_ByEscalations_Get";

            Object returnValue = ExecuteProcedureScalar(connectionString, parameters, storedProcedureName);

            XmlDocument xmlDocument = new XmlDocument();
            if (returnValue != null)
            {
                xmlDocument.LoadXml(returnValue.ToString());
                return xmlDocument;
            }
            return null;
        }

        public MessageTemplate GetMessageTemplateByName(String TemplateName)
        {
            SqlDataReader reader = null;
            MessageTemplate messageTemplate = null;
            StringBuilder messageXML = new StringBuilder();

            String connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("MessageManager_SqlParameters");

                SqlParameter[] parameters = generator.GetParameters("Workflow_Messaging_MessageTemplate_Get_ParametersArray");

                parameters[0].Value = TemplateName;

                String storedProcedureName = "usp_Workflow_Messaging_MessageTemplate_Get";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            messageXML.Append(reader[0]);
                        }
                    }

                    if (!String.IsNullOrWhiteSpace(messageXML.ToString()))
                    {
                        messageTemplate = new MessageTemplate(messageXML.ToString());
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return messageTemplate;
        }
    }
}
