using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Data.SqlClient;
using System.Transactions;

namespace MDM.Workflow.Designer.Data.SqlClient
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Workflow;

    /// <summary>
    /// Provodes database related functions for Workflow Version
    /// </summary>
    public class WorkflowVersionDA : SqlClientDataAccessBase
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region Public methods

        /// <summary>
        /// Get workflow versions of a workflow, or all versions available
        /// </summary>
        /// <param name="workflowId">workflow id to get versions of it</param>
        /// <returns>collection of workflow versions</returns>
        public Collection<WorkflowVersion> Get(Int32 workflowId)
        {
            Collection<WorkflowVersion> data = new Collection<WorkflowVersion>();
            SqlDataReader reader = null;

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("WorkflowManager_SqlParameters");

                parameters = generator.GetParameters("Workflow_Designer_WorkflowVersion_Get_ParametersArray");

                storedProcedureName = "usp_Workflow_Designer_WorkflowVersion_Get";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                data = new Collection<WorkflowVersion>();
                while (reader.Read())
                {
                    Object[] values = new Object[reader.FieldCount];
                    reader.GetValues(values);
                    WorkflowVersion workflowVersion = new WorkflowVersion(values);
                    data.Add(workflowVersion);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return data;
        }

        public WorkflowVersion GetById(Int32 versionId, DBCommandProperties command )
        {
            WorkflowVersion version = null;
            String versionXml = String.Empty;

            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("WorkflowDesigner_SqlParameters");

                parameters = generator.GetParameters("Workflow_Designer_WorkflowVersion_GetById_ParametersArray");

                parameters[0].Value = versionId;

                storedProcedureName = "usp_Workflow_Designer_WorkflowVersion_GetById";

                Object returnValue = ExecuteProcedureScalar(command.ConnectionString, parameters, storedProcedureName);
                if (returnValue != null)
                {
                    versionXml = returnValue.ToString();
                    version = new WorkflowVersion(versionXml);
                }
            }
            finally
            {
            }

            return version;
        }

        /// <summary>
        /// Process workflow versions based on action provided in each object
        /// </summary>
        /// <param name="listWorkflowVersions">Collection of workflow versions to be processed</param>
        /// <param name="loginUser">current logged in user</param>
        /// <param name="programName">program name performing this action</param>
        /// <returns></returns>
        public Int32 Process(Collection<WorkflowVersion> listWorkflowVersions, String loginUser, String programName)
        {
            Int32 output = 0; //success

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
            {
                SqlParameter[] parameters;
                String connectionString = String.Empty;
                String storedProcedureName = String.Empty;

                connectionString = AppConfigurationHelper.ConnectionString;

                String paramXML = ConvertToXML(listWorkflowVersions);

                SqlParametersGenerator generator = new SqlParametersGenerator("WorkflowManager_SqlParameters");

                parameters = generator.GetParameters("Workflow_Designer_WorkflowVersion_Process_ParametersArray");

                parameters[0].Value = paramXML;
                parameters[1].Value = loginUser;
                parameters[2].Value = programName;

                storedProcedureName = "usp_Workflow_Designer_WorkflowVersion_Process";

                output = ExecuteProcedureNonQuery(connectionString, parameters, storedProcedureName);

                transactionScope.Complete();
            }

            return output;
        }

        #endregion Public methods

        #region Private methods

        private String ConvertToXML(Collection<WorkflowVersion> listWorkflowVersions)
        {
            String xml = "<WorkflowVersions>";

            StringBuilder stringBuilder = new StringBuilder(xml);

            foreach (WorkflowVersion workflowVersion in listWorkflowVersions)
            {
                stringBuilder.Append(workflowVersion.ToXML());

            }
            stringBuilder.Append("</WorkflowVersions>");

            return stringBuilder.ToString();
        }

        #endregion Private methods

        #endregion
    }
}
