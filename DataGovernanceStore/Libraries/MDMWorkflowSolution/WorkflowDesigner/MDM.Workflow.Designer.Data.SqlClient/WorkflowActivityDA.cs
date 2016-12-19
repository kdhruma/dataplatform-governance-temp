using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Data.SqlClient;
using System.Transactions;

using MDM.Core;
using MDM.Utility;
using MDM.BusinessObjects;
using MDM.BusinessObjects.Workflow;

namespace MDM.Workflow.Designer.Data.SqlClient
{
    /// <summary>
    /// Provides database related functions for Workflow Activity
    /// </summary>
    public class WorkflowActivityDA : SqlClientDataAccessBase
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Get workflow activities of a workflow version, or all activities available
        /// </summary>
        /// <param name="workflowVersionId">workflow version id to get activities of it</param>
        /// <returns>collection of workflow activities</returns>
        public Collection<WorkflowActivity> Get(Int32 workflowVersionId)
        {
            Collection<WorkflowActivity> data = new Collection<WorkflowActivity>();
            SqlDataReader reader = null;

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("WorkflowDesigner_SqlParameters");

                parameters = generator.GetParameters("Workflow_Designer_WorkflowActivity_Get_ParametersArray");

                parameters[0].Value = workflowVersionId;

                storedProcedureName = "usp_Workflow_Designer_WorkflowActivity_Get";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                data = new Collection<WorkflowActivity>();
                while (reader.Read())
                {
                    Object[] values = new Object[reader.FieldCount];
                    reader.GetValues(values);
                    WorkflowActivity workflowActivity = new WorkflowActivity(values);
                    data.Add(workflowActivity);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return data;
        }

        /// <summary>
        /// Process workflow activities based on action provided in each object
        /// </summary>
        /// <param name="listActivities">Collection of workflow activities to be processed</param>
        /// <param name="loginUser">current logged in user</param>
        /// <param name="programName">program name performing this action</param>
        /// <param name="command">DBCommand containing information about connection string</param>
        /// <returns></returns>
        public Int32 Process(Collection<WorkflowActivity> listActivities, String loginUser, String programName, DBCommandProperties command)
        {
            Int32 output = 0; //success

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
            {
                SqlParameter[] parameters;
                String storedProcedureName = String.Empty;

                String paramXML = ConvertToXML(listActivities);

                SqlParametersGenerator generator = new SqlParametersGenerator("WorkflowDesigner_SqlParameters");

                parameters = generator.GetParameters("Workflow_Designer_WorkflowActivity_Process_ParametersArray");

                parameters[0].Value = paramXML;
                parameters[1].Value = loginUser;
                parameters[2].Value = programName;

                storedProcedureName = "usp_Workflow_Designer_WorkflowActivity_Process";

                output = ExecuteProcedureNonQuery(command.ConnectionString, parameters, storedProcedureName);

                transactionScope.Complete();
            }

            return output;
        }

        /// <summary>
        /// Assigns user to this activity based on AssignmentType
        /// </summary>
        /// <param name="instanceGuid"></param>
        /// <param name="activityShortName"></param>
        /// <param name="assignmentType"></param>
        /// <param name="allowedUsers"></param>
        /// <param name="allowedRoles"></param>
        /// <param name="assignToPreviousActorOnRerun"></param>
        /// <returns>Returns the Security User object</returns>
        public SecurityUser AssignUser(String instanceGuid, String activityShortName, AssignmentType assignmentType, String allowedUsers, String allowedRoles, Boolean assignToPreviousActorOnRerun)
        {
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            SecurityUser securityUser = null;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("WorkflowDesigner_SqlParameters");

                parameters = generator.GetParameters("Workflow_ActivityManager_Assignment_Process_ParametersArray");

                parameters[0].Value = assignmentType.ToString();
                parameters[1].Value = instanceGuid;
                parameters[2].Value = activityShortName;
                parameters[3].Value = allowedUsers;
                parameters[4].Value = allowedRoles;
                parameters[5].Value = assignToPreviousActorOnRerun;

                storedProcedureName = "usp_Workflow_ActivityManager_Assignment_Process";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        Object[] values = new Object[reader.FieldCount];
                        reader.GetValues(values);
                        securityUser = new SecurityUser(values);

                        break;
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return securityUser;
        }

        private String ConvertToXML(Collection<WorkflowActivity> listActivities)
        {
            String xml = "<WorkflowActivities>";

            StringBuilder stringBuilder = new StringBuilder(xml);

            foreach (WorkflowActivity activity in listActivities)
            {
                stringBuilder.Append(activity.ToXML());

            }
            stringBuilder.Append("</WorkflowActivities>");

            return stringBuilder.ToString();
        }

        #endregion
    }
}
