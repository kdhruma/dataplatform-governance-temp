using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Text;
using System.Transactions;

namespace MDM.Workflow.Designer.Data.SqlClient
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Utility;
    using BO = MDM.BusinessObjects;
    using WFBO = MDM.BusinessObjects.Workflow;

    /// <summary>
    /// Provodes database related functions for Workflow
    /// </summary>
    public class WorkflowDA : SqlClientDataAccessBase
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Get All Workflow Information available in the system (Latest publish)
        /// Workflow Infomration such as WorkflowName, WorkflowVersion, ActivityName, ActivityAction
        /// </summary>
        /// <param name="command">Indicates the database command properties</param>
        /// <returns>WorkflowInformation</returns>
        public WorkflowInfoCollection GetAllWorkflowInformation(DBCommandProperties command)
        {
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = "usp_WorkflowDesigner_Workflow_Get";
            WorkflowInfoCollection workflowInformation = new WorkflowInfoCollection();

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("WorkflowDesigner_SqlParameters");
                parameters = generator.GetParameters("Workflow_Designer_Workflow_Get_ParametersArray");

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                workflowInformation = PopualateWorkflowInformation(reader);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return workflowInformation;
        }

        /// <summary>
        /// Gets the workflow data by name
        /// </summary>
        /// <param name="workflowName">Short Name or Long Name of the workflow</param>
        /// <param name="command">DBCommand containing information about connection string</param>
        /// <returns>Workflow Object</returns>
        public WFBO.Workflow GetWorkflowByName(String workflowName,BO.DBCommandProperties command )
        {
            WFBO.Workflow workflow = null;
            String workflowXml = String.Empty;

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
            {
                SqlParameter[] parameters;
                String storedProcedureName = String.Empty;

                SqlParametersGenerator generator = new SqlParametersGenerator("WorkflowDesigner_SqlParameters");

                parameters = generator.GetParameters("Workflow_Designer_Workflow_GetByName_ParametersArray");

                parameters[0].Value = workflowName;

                storedProcedureName = "usp_Workflow_Designer_Workflow_GetByName";

                Object returnValue = ExecuteProcedureScalar(command.ConnectionString, parameters, storedProcedureName);

                if (returnValue != null)
                {
                    workflowXml = returnValue.ToString();
                    workflow = new WFBO.Workflow(workflowXml);
                }

                transactionScope.Complete();
            }

            return workflow;
        }

        /// <summary>
        /// Process workflows based on action provided in each object
        /// </summary>
        /// <param name="listWorkflows">Collection of workflow objects to be processed</param>
        /// <param name="loginUser">current logged in user</param>
        /// <param name="programName">program name performing this action</param>
        /// <param name="command">DBCommand containing information about connection string</param>
        /// <returns>Returns the newly created workflow version</returns>
        public WFBO.WorkflowVersion Process(Collection<WFBO.Workflow> listWorkflows, String loginUser, String programName, BO.DBCommandProperties command)
        {
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            WFBO.WorkflowVersion workflowVersion = null;

            try
            {
                String paramXML = ConvertToXML(listWorkflows);

                SqlParametersGenerator generator = new SqlParametersGenerator("WorkflowDesigner_SqlParameters");

                parameters = generator.GetParameters("Workflow_Designer_Workflow_Process_ParametersArray");

                parameters[0].Value = paramXML;
                parameters[1].Value = loginUser;
                parameters[2].Value = programName;

                storedProcedureName = "usp_Workflow_Designer_Workflow_Process";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        Object[] values = new Object[reader.FieldCount];
                        reader.GetValues(values);
                        workflowVersion = new WFBO.WorkflowVersion();

                        Int32 intId = 0;
                        if (values[0] != null)
                            Int32.TryParse(values[0].ToString(), out intId);
                        workflowVersion.WorkflowId = intId;

                        intId = 0;
                        if (values[1] != null)
                            Int32.TryParse(values[1].ToString(), out intId);
                        workflowVersion.Id = intId;

                        Int32 versionNumber = 0;
                        if (values[2] != null)
                            Int32.TryParse(values[2].ToString(), out versionNumber);
                        workflowVersion.VersionNumber = versionNumber;

                        break;
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return workflowVersion;
        }

        private String ConvertToXML(Collection<WFBO.Workflow> listWorkflows)
        {
            String xml = "<Workflows>";

            StringBuilder stringBuilder = new StringBuilder(xml);

            foreach (WFBO.Workflow workflow in listWorkflows)
            {
                stringBuilder.Append(workflow.ToXML());

            }
            stringBuilder.Append("</Workflows>");

            return stringBuilder.ToString();
        }

        private WorkflowInfoCollection PopualateWorkflowInformation(SqlDataReader reader)
        {
            WorkflowInfoCollection workflowInfoCollection = new WorkflowInfoCollection();

            while (reader.Read())
            {
                WorkflowInfo workflowInfo = new WorkflowInfo();

                if (reader["WorkflowId"] != null)
                {
                    workflowInfo.WorkflowId = ValueTypeHelper.Int32TryParse(reader["WorkflowId"].ToString(), workflowInfo.WorkflowId);
                }

                if (reader["WorkflowShortName"] != null)
                {
                    workflowInfo.WorkflowName = reader["WorkflowShortName"].ToString();
                }

                if (reader["WorkflowVersion"] != null)
                {
                    workflowInfo.WorkflowVersionNumber = ValueTypeHelper.Int32TryParse(reader["WorkflowVersion"].ToString(), workflowInfo.WorkflowVersionNumber);
                }

                if (reader["WorkflowActivityId"] != null)
                {
                    workflowInfo.WorkflowActivityId = ValueTypeHelper.Int32TryParse(reader["WorkflowActivityId"].ToString(), workflowInfo.WorkflowActivityId);
                }

                if (reader["WorkflowActivityShortName"] != null)
                {
                    workflowInfo.WorkflowActivityShortName = reader["WorkflowActivityShortName"].ToString();
                }

                if (reader["WorkflowActivityLongName"] != null)
                {
                    workflowInfo.WorkflowActivityLongName = reader["WorkflowActivityLongName"].ToString();
                }

                if (reader["WorkflowActionId"] != null)
                {
                    workflowInfo.WorkflowActivityActionId = ValueTypeHelper.Int32TryParse(reader["WorkflowActionId"].ToString(), workflowInfo.WorkflowActivityActionId);
                }

                if (reader["WorkflowAction"] != null)
                {
                    workflowInfo.WorkflowActivityAction = reader["WorkflowAction"].ToString();
                }

                workflowInfoCollection.Add(workflowInfo);
            }

            return workflowInfoCollection;
        }

        #endregion
    }
}
