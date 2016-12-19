using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Xml;
using System.Transactions;

namespace MDM.Workflow.PersistenceManager.Data
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects;
    using Microsoft.SqlServer.Server;
    using MDMBOW = MDM.BusinessObjects.Workflow;
    using MDM.BusinessObjects.Diagnostics;

    public class WorkflowInstanceDA : SqlClientDataAccessBase
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region Public methods

        public Int32 Process(Collection<MDMBOW.WorkflowInstance> workflowInstances, Int32 processingType, Int32 serviceId, String serviceType, String userName, String programName, DBCommandProperties command)
        {
            Int32 output = 0; //success

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                SqlParameter[] parameters;
                String storedProcedureName = String.Empty;
                String instancesXml = String.Empty;

                List<SqlDataRecord> instanceTable;
                List<SqlDataRecord> mdmObjectTable;
                
                SqlParametersGenerator generator = new SqlParametersGenerator("WorkflowPersistence_SqlParameters");
                parameters = generator.GetParameters("Workflow_PersistenceManager_WorkflowInstance_Process_ParametersArray");

                SqlMetaData[] instanceMetaData = generator.GetTableValueMetadata("Workflow_PersistenceManager_WorkflowInstance_Process_ParametersArray", parameters[0].ParameterName);
                SqlMetaData[] mdmObjectMetaData = generator.GetTableValueMetadata("Workflow_PersistenceManager_WorkflowInstance_Process_ParametersArray", parameters[1].ParameterName);

                CreateInstanceTable(workflowInstances, instanceMetaData, mdmObjectMetaData, out instanceTable, out mdmObjectTable);

                parameters[0].Value = instanceTable;
                parameters[1].Value = mdmObjectTable;
                parameters[2].Value = processingType;
                parameters[3].Value = serviceId;
                parameters[4].Value = serviceType;
                parameters[5].Value = String.Empty; // "cfadmin"
                parameters[6].Value = programName;

                storedProcedureName = "usp_Workflow_PersistenceManager_WorkflowInstance_Process";

                output = ExecuteProcedureNonQuery(command.ConnectionString, parameters, storedProcedureName);

                transactionScope.Complete();
            }

            return output;
        }

        public Collection<MDMBOW.WorkflowInstance> GetByWorkflow(Collection<MDMBOW.Workflow> workflowCollection)
        {
            Collection<MDMBOW.WorkflowInstance> workflowInstances = null;
            StringBuilder sbInstancesXml = new StringBuilder();
            String strInstancesXml = String.Empty;
            String workflowCollectionXml = String.Empty;

            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            try
            {
                connectionString = AppConfigurationHelper.ConnectionString;

                workflowCollectionXml = ConvertToXmlForWorkflow(workflowCollection);

                //TODO:: correct reference
                SqlParametersGenerator generator = new SqlParametersGenerator("WorkflowPersistence_SqlParameters");

                parameters = generator.GetParameters("Workflow_PersistenceManager_WorkflowInstance_Get_ParametersArray");
                parameters[0].Value = workflowCollectionXml;

                storedProcedureName = "usp_Workflow_PersistenceManager_WorkflowInstance_Get";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            sbInstancesXml.Append(reader[0]);
                        }
                    }

                    strInstancesXml = sbInstancesXml.ToString();

                    if (!String.IsNullOrWhiteSpace(strInstancesXml))
                    {
                        workflowInstances = GetObjectCollectionFromXml(strInstancesXml);
                    }
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return workflowInstances;
        }

        public Collection<MDMBOW.WorkflowInstance> GetByMDMObject(MDMBOW.WorkflowMDMObject mdmObj, String activityName, DBCommandProperties command)
        {
            Collection<MDMBOW.WorkflowInstance> workflowInstances = null;
            StringBuilder sbInstancesXml = new StringBuilder();
            String strInstancesXml = String.Empty;

            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;

            try
            {
                //TODO:: correct reference
                SqlParametersGenerator generator = new SqlParametersGenerator("WorkflowPersistence_SqlParameters");

                parameters = generator.GetParameters("Workflow_PersistenceManager_WorkflowInstance_GetByMDMObject_ParametersArray");
                //parameters[0].Value = mdmObj.MDMObjectId;
                parameters[0].Value = mdmObj.MDMObjectGUID;
                parameters[1].Value = mdmObj.MDMObjectType;
                parameters[2].Value = activityName;

                storedProcedureName = "usp_Workflow_PersistenceManager_WorkflowInstance_GetByMDMObject";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            sbInstancesXml.Append(reader[0]);
                        }
                    }

                    strInstancesXml = sbInstancesXml.ToString();

                    if (!String.IsNullOrWhiteSpace(strInstancesXml))
                    {
                        workflowInstances = GetObjectCollectionFromXml(strInstancesXml);
                    }
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return workflowInstances;
        }

        public Collection<MDMBOW.WorkflowInstance> GetByMDMObjectInWorkflow(Int32 workflowId, MDMBOW.WorkflowMDMObject mdmObj, DBCommandProperties command)
        {
            Collection<MDMBOW.WorkflowInstance> workflowInstances = null;
            StringBuilder sbInstancesXml = new StringBuilder();
            String strInstancesXml = String.Empty;

            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;

            try
            {
                //TODO:: correct reference
                SqlParametersGenerator generator = new SqlParametersGenerator("WorkflowPersistence_SqlParameters");

                parameters = generator.GetParameters("Workflow_PersistenceManager_WorkflowInstance_GetByMDMObjectInWorkflow_ParametersArray");
                parameters[0].Value = mdmObj.MDMObjectId;
                parameters[1].Value = mdmObj.MDMObjectType;
                parameters[2].Value = workflowId;

                storedProcedureName = "usp_Workflow_PersistenceManager_WorkflowInstance_GetByMDMObjectInWorkflow";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            sbInstancesXml.Append(reader[0]);
                        }
                    }

                    strInstancesXml = sbInstancesXml.ToString();

                    if (!String.IsNullOrWhiteSpace(strInstancesXml))
                    {
                        workflowInstances = GetObjectCollectionFromXml(strInstancesXml);
                    }
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return workflowInstances;
        }

        public Collection<MDMBOW.WorkflowInstance> GetByRuntimeInstanceIds(String commaSeparatedRuntimeInstanceIds, DBCommandProperties command)
        {
            Collection<MDMBOW.WorkflowInstance> workflowInstances = null;
            StringBuilder sbInstancesXml = new StringBuilder();
            String strInstancesXml = String.Empty;

            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("WorkflowPersistence_SqlParameters");

                parameters = generator.GetParameters("Workflow_PersistenceManager_WorkflowInstance_GetByRunningInstanceIds_ParametersArray");

                parameters[0].Value = commaSeparatedRuntimeInstanceIds;

                storedProcedureName = "usp_Workflow_PersistenceManager_WorkflowInstance_GetByRunningInstanceIds";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            sbInstancesXml.Append(reader[0]);
                        }
                    }

                    strInstancesXml = sbInstancesXml.ToString();

                    if (!String.IsNullOrWhiteSpace(strInstancesXml))
                    {
                        workflowInstances = GetObjectCollectionFromXml(strInstancesXml);
                    }
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return workflowInstances;
        }

        public Int32 CheckForRunningInstances(String mdmObjectIDs, String mdmObjectType, Int32 workflowID, DBCommandProperties command)
        {
            Collection<Int64> runningInstances = this.GetRunningInstanceDetails(mdmObjectIDs, mdmObjectType, workflowID, command);
            return runningInstances.Count;
        }

        public Collection<Int64> GetRunningInstanceDetails(String mdmObjectIDs, String mdmObjectType, Int32 workflowID, DBCommandProperties command)
        {
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            Collection<Int64> runningInstanceDetails = new Collection<Int64>();

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("WorkflowPersistence_SqlParameters");

                parameters = generator.GetParameters("Workflow_PersistenceManager_WorkflowInstance_CheckForRunningInstances_ParametersArray");

                parameters[0].Value = mdmObjectIDs;
                parameters[1].Value = mdmObjectType;
                parameters[2].Value = workflowID;

                storedProcedureName = "usp_Workflow_PersistenceManager_WorkflowInstance_CheckForRunningInstances";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                runningInstanceDetails = PopulateRunningInstanceDetails(reader);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return runningInstanceDetails;
        }

        #region Workflow UI Methods

        public void GetAllWorkflowDetails(ref Collection<MDMBOW.Workflow> workflowCollection, ref Collection<MDMBOW.WorkflowVersion> workflowVersionCollection, ref Collection<MDMBOW.WorkflowActivity> workflowActivityCollection, DBCommandProperties command)
        {
            SqlDataReader reader = null;
            MDMBOW.Workflow workflow = null;
            MDMBOW.WorkflowVersion workflowVersion = null;
            MDMBOW.WorkflowActivity workflowActivity = null;
            workflowCollection = new Collection<MDMBOW.Workflow>();
            workflowVersionCollection = new Collection<MDMBOW.WorkflowVersion>();
            workflowActivityCollection = new Collection<MDMBOW.WorkflowActivity>();

            String storedProcedureName = String.Empty;

            try
            {
                storedProcedureName = "usp_Workflow_Dashboard_GetAllWorkflows";

                reader = ExecuteProcedureReader(command.ConnectionString, null, storedProcedureName);

                while (reader.Read())
                {
                    Object[] values = new Object[reader.FieldCount];
                    reader.GetValues(values);
                    workflow = new MDMBOW.Workflow();

                    int intId = -1;
                    if (values[0] != null)
                        Int32.TryParse(values[0].ToString(), out intId);

                    workflow.Id = intId;

                    if (values[1] != null)
                        workflow.LongName = values[1].ToString();

                    if (values[2] != null)
                        workflow.WorkflowType = values[2].ToString();

                    intId = -1;
                    if (values[3] != null)
                        Int32.TryParse(values[3].ToString(), out intId);

                    workflow.LatestVersion = intId;

                    workflowCollection.Add(workflow);
                }

                reader.NextResult();

                while (reader.Read())
                {
                    Object[] values = new Object[reader.FieldCount];
                    reader.GetValues(values);
                    workflowVersion = new MDMBOW.WorkflowVersion();

                    int intId = -1;
                    if (values[0] != null)
                        Int32.TryParse(values[0].ToString(), out intId);
                    workflowVersion.WorkflowId = intId;

                    intId = -1;
                    if (values[1] != null)
                        Int32.TryParse(values[1].ToString(), out intId);
                    workflowVersion.Id = intId;

                    if (values[2] != null)
                        workflowVersion.VersionName = values[2].ToString();

                    intId = -1;
                    if (values[3] != null)
                        Int32.TryParse(values[3].ToString(), out intId);
                    workflowVersion.VersionNumber = intId;

                    Boolean isDraft = false;
                    if (values[4] != null)
                    {
                        Boolean.TryParse(values[4].ToString(), out isDraft);
                        workflowVersion.IsDraft = isDraft;
                    }

                    if (values[5] != null)
                        workflowVersion.VersionType = values[5].ToString();

                    workflowVersionCollection.Add(workflowVersion);
                }

                reader.NextResult();

                while (reader.Read())
                {
                    Object[] values = new Object[reader.FieldCount];
                    reader.GetValues(values);
                    workflowActivity = new MDMBOW.WorkflowActivity();

                    int intId = -1;
                    if (values[0] != null)
                        Int32.TryParse(values[0].ToString(), out intId);
                    workflowActivity.WorkflowId = intId;

                    intId = -1;
                    if (values[1] != null)
                        Int32.TryParse(values[1].ToString(), out intId);
                    workflowActivity.WorkflowVersionId = intId;

                    intId = -1;
                    if (values[2] != null)
                        Int32.TryParse(values[2].ToString(), out intId);
                    workflowActivity.Id = intId;

                    if (values[3] != null)
                        workflowActivity.Name = values[3].ToString();

                    if (values[4] != null)
                        workflowActivity.LongName = values[4].ToString();

                    if (values[5] != null)
                        workflowActivity.WorkflowType = values[5].ToString();

                    workflowActivityCollection.Add(workflowActivity);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        public Collection<MDMBOW.WorkflowInstance> GetInstanceSummary(String workflowType, Int32 workflowId, Int32 workflowVersionId, String workflowStatus, String activityShortName, String roleIds, String userIds, String instanceId, String mdmObjectIds, Boolean? hasEscalation, Int32 returnSize, DBCommandProperties command)
        {
            SqlDataReader reader = null;
            MDMBOW.WorkflowInstance workflowInstance = null;
            Collection<MDMBOW.WorkflowInstance> workflowInstanceCollection = new Collection<MDMBOW.WorkflowInstance>();

            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("WorkflowPersistence_SqlParameters");

                parameters = generator.GetParameters("Workflow_Dashboard_GetInstanceSummary_ParametersArray");

                parameters[0].Value = workflowType;
                parameters[1].Value = workflowId;
                parameters[2].Value = workflowVersionId;
                parameters[3].Value = workflowStatus;
                parameters[4].Value = activityShortName;
                parameters[5].Value = roleIds;
                parameters[6].Value = userIds;
                parameters[7].Value = instanceId;
                parameters[8].Value = mdmObjectIds;
                parameters[9].Value = hasEscalation;
                parameters[10].Value = returnSize;

                storedProcedureName = "usp_Workflow_Dashboard_GetInstanceSummary";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    Object[] values = new Object[reader.FieldCount];
                    reader.GetValues(values);
                    workflowInstance = new MDMBOW.WorkflowInstance(values, "WorkflowDashboard");

                    workflowInstanceCollection.Add(workflowInstance);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return workflowInstanceCollection;
        }

        public void GetInstanceDetails(String workflowType, String instanceId, ref Collection<MDMBOW.WorkflowActivity> runningActivityCollection, ref Collection<MDMBOW.WorkflowMDMObject> mdmObjectCollection, ref Collection<MDMBOW.Escalation> escalationCollection, DBCommandProperties command)
        {
            SqlDataReader reader = null;
            MDMBOW.WorkflowActivity workflowActivity = null;
            MDMBOW.WorkflowMDMObject workflowMDMObject = null;
            MDMBOW.Escalation escalation = null;
            runningActivityCollection = new Collection<MDMBOW.WorkflowActivity>();
            mdmObjectCollection = new Collection<MDMBOW.WorkflowMDMObject>();
            escalationCollection = new Collection<MDMBOW.Escalation>();

            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("WorkflowPersistence_SqlParameters");

                parameters = generator.GetParameters("Workflow_Dashboard_GetInstanceDetails_ParametersArray");

                parameters[0].Value = workflowType;
                parameters[1].Value = instanceId;

                storedProcedureName = "usp_Workflow_Dashboard_GetInstanceDetails";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    Object[] values = new Object[reader.FieldCount];
                    reader.GetValues(values);
                    workflowActivity = new MDMBOW.WorkflowActivity();

                    if (values[0] != null)
                        workflowActivity.Name = values[0].ToString();

                    if (values[1] != null)
                        workflowActivity.LongName = values[1].ToString();

                    if (values[2] != null)
                        workflowActivity.ActingUser = values[2].ToString();

                    if (values[3] != null)
                        workflowActivity.AllowedRoles = values[3].ToString();

                    if (values[4] != null)
                        workflowActivity.AllowedUsers = values[4].ToString();

                    if (values[5] != null)
                        workflowActivity.StartDate = values[5].ToString();

                    if (values[6] != null)
                        workflowActivity.EscalationLevel = values[6].ToString();

                    if (values[7] != null)
                    {
                        Boolean delegationAllowed = true;
                        Boolean.TryParse(values[7].ToString(), out delegationAllowed);
                        workflowActivity.DelegationAllowed = delegationAllowed;
                    }

                    runningActivityCollection.Add(workflowActivity);
                }

                reader.NextResult();

                while (reader.Read())
                {
                    Object[] values = new Object[reader.FieldCount];
                    reader.GetValues(values);
                    workflowMDMObject = new MDMBOW.WorkflowMDMObject(values);

                    mdmObjectCollection.Add(workflowMDMObject);
                }

                reader.NextResult();

                while (reader.Read())
                {
                    Object[] values = new Object[reader.FieldCount];
                    reader.GetValues(values);
                    escalation = new MDMBOW.Escalation(values);

                    escalationCollection.Add(escalation);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        public String GetWorkflowStatistics(String workflowType, Int32 workflowId, Int32 workflowVersionId, DBCommandProperties command)
        {
            StringBuilder strStatistics = new StringBuilder();
            SqlDataReader reader = null;

            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("WorkflowPersistence_SqlParameters");

                parameters = generator.GetParameters("Workflow_Dashboard_GetStatistics_ParametersArray");

                parameters[0].Value = workflowType;
                parameters[1].Value = workflowId;
                parameters[2].Value = workflowVersionId;

                storedProcedureName = "usp_Workflow_Dashboard_GetStatistics";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    Object[] values = new Object[reader.FieldCount];
                    reader.GetValues(values);

                    strStatistics.Append("<b>");

                    if (values[0] != null)
                        strStatistics.Append(values[0].ToString());

                    strStatistics.Append(":</b>&nbsp;&nbsp;&nbsp;");

                    if (values[1] != null)
                        strStatistics.Append(values[1].ToString());

                    strStatistics.Append("<br />");
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return strStatistics.ToString();
        }

        public Int32 UpdateWorkflowInstances(String instanceGUIDs, String mdmObjectID, String mdmObjectType, Int32 workflowID, String activityShortName, String instanceStatus, String loginUser, String modifiedUser, String programName, DBCommandProperties command)
        {
            Int32 output = 0; //success

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                SqlParameter[] parameters;
                String storedProcedureName = String.Empty;

                SqlParametersGenerator generator = new SqlParametersGenerator("WorkflowPersistence_SqlParameters");

                parameters = generator.GetParameters("Workflow_Management_Instance_Update_ParametersArray");

                parameters[0].Value = instanceGUIDs;
                parameters[1].Value = mdmObjectID;
                parameters[2].Value = mdmObjectType;
                parameters[3].Value = workflowID;
                parameters[4].Value = activityShortName;
                parameters[5].Value = instanceStatus;
                parameters[6].Value = loginUser;
                parameters[7].Value = programName;
                parameters[8].Value = modifiedUser;

                storedProcedureName = "usp_Workflow_Management_Instance_Update";

                output = (Int32)ExecuteProcedureScalar(command.ConnectionString, parameters, storedProcedureName);

                transactionScope.Complete();
            }

            return output;
        }

        public DataTable GetActionButtons(Int32 activityId, String toolBarButtonXML, String loginUser, Boolean? checkAllowedUsersAndRoles, DBCommandProperties command)
        {
            SqlDataReader reader = null;

            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;

            DataTable toolBarButtonTable = new DataTable();
            toolBarButtonTable.TableName = "ActionButtons";
            toolBarButtonTable.Columns.Add("Button", typeof(String));
            toolBarButtonTable.Columns.Add("Visible", typeof(String));
            toolBarButtonTable.Columns.Add("CommentsRequired", typeof(String));
            toolBarButtonTable.Columns.Add("TransitionMessageCode", typeof(String));

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("WorkflowPersistence_SqlParameters");

                parameters = generator.GetParameters("Workflow_Management_ActionButtons_Get_ParametersArray");

                parameters[0].Value = activityId;
                parameters[1].Value = toolBarButtonXML;
                if (loginUser != null)
                {
                    parameters[2].Value = loginUser;
                }
                if (checkAllowedUsersAndRoles.HasValue)
                {
                    parameters[3].Value = checkAllowedUsersAndRoles.Value;
                }

                storedProcedureName = "usp_Workflow_Management_ActionButtons_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    Object[] values = new Object[reader.FieldCount];
                    reader.GetValues(values);

                    DataRow toolBarButtonRow = toolBarButtonTable.NewRow();

                    toolBarButtonRow["Button"] = values[0].ToString();
                    toolBarButtonRow["Visible"] = values[1].ToString();
                    toolBarButtonRow["CommentsRequired"] = values[2].ToString();
                    toolBarButtonRow["TransitionMessageCode"] = values[3].ToString();

                    toolBarButtonTable.Rows.Add(toolBarButtonRow);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return toolBarButtonTable;
        }

        /// <summary>
        /// Gets the action buttons for the requested activity
        /// </summary>
        /// <param name="activityId">Id of an activity</param>
        /// <param name="command">Object having command properties</param>
        /// <param name="loginUser">User login. Will be used during allowed users and roles check if enabled</param>
        /// <param name="checkAllowedUsersAndRoles">Set to True if you want to check activity AllowedUsers and AllowedRoles information or set to Null if you want default behavior</param>
        /// <returns>Returns action buttons with comments details</returns>
        public Table GetActionButtons(Int32 activityId, String loginUser, Boolean? checkAllowedUsersAndRoles, DBCommandProperties command)
        {
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;

            #region Prepare the Schema for return Table object

            Table actionButtonsDetails = new Table();

            AddColumnToTable(actionButtonsDetails, 0, "Button");
            AddColumnToTable(actionButtonsDetails, 1, "Visible");
            AddColumnToTable(actionButtonsDetails, 2, "CommentsRequired");
            AddColumnToTable(actionButtonsDetails, 3, "TransitionMessageCode");

            #endregion

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("WorkflowPersistence_SqlParameters");

                parameters = generator.GetParameters("Workflow_Management_ActionButtons_Get_ParametersArray");

                parameters[0].Value = activityId;
                parameters[1].Value = String.Empty;
                if (loginUser != null)
                {
                    parameters[2].Value = loginUser;
                }
                if (checkAllowedUsersAndRoles.HasValue)
                {
                    parameters[3].Value = checkAllowedUsersAndRoles.Value;
                }

                storedProcedureName = "usp_Workflow_Management_ActionButtons_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    Object[] values = new Object[reader.FieldCount];
                    reader.GetValues(values);

                    Row wfDetailRow = actionButtonsDetails.NewRow();
                    wfDetailRow.SetValue(actionButtonsDetails.Columns[0], values[0]);
                    wfDetailRow.SetValue(actionButtonsDetails.Columns[1], values[1]);
                    wfDetailRow.SetValue(actionButtonsDetails.Columns[3], values[3]);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return actionButtonsDetails;
        }

        public DataTable GetAssignmentButtons(Int32 activityId, String assignmentStatus, String toolBarButtonXML, String loginUser, Boolean? checkAllowedUsersAndRoles, DBCommandProperties command)
        {
            SqlDataReader reader = null;

            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;

            DataTable toolBarButtonTable = new DataTable();
            toolBarButtonTable.TableName = "AssignmentButtons";
            toolBarButtonTable.Columns.Add("Button", typeof(String));
            toolBarButtonTable.Columns.Add("Visible", typeof(String));

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("WorkflowPersistence_SqlParameters");

                parameters = generator.GetParameters("Workflow_Management_AssignmentButtons_Get_ParametersArray");

                parameters[0].Value = activityId;
                parameters[1].Value = assignmentStatus;
                parameters[2].Value = toolBarButtonXML;
                parameters[3].Value = loginUser;
                if (checkAllowedUsersAndRoles.HasValue)
                {
                    parameters[4].Value = checkAllowedUsersAndRoles.Value;
                }

                storedProcedureName = "usp_Workflow_Management_AssignmentButtons_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    Object[] values = new Object[reader.FieldCount];
                    reader.GetValues(values);

                    DataRow toolBarButtonRow = toolBarButtonTable.NewRow();

                    toolBarButtonRow["Button"] = values[0].ToString();
                    toolBarButtonRow["Visible"] = values[1].ToString();

                    toolBarButtonTable.Rows.Add(toolBarButtonRow);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return toolBarButtonTable;
        }

        /// <summary>
        /// Gets the details required for the Workflow UI Panel
        /// </summary>
        /// <param name="catalogId">Id of the catalog for which details are required</param>
        /// <param name="userId">User Id for which details needs to be get</param>
        /// <param name="command">Object having command properties</param>
        /// <param name="searchAttributeRules"></param>
        /// <param name="showItemsAssignedToOtherUsers">Include data related to workflows assigned to other users</param>
        /// <param name="showEmptyItems">Do not hide empty nodes</param>
        /// <returns>Returns details in Table format</returns>
        public Table GetWorkflowPanelDetails(Int32 catalogId, Int32 userId, Collection<SearchAttributeRule> searchAttributeRules, Boolean showEmptyItems, Boolean showItemsAssignedToOtherUsers, DBCommandProperties command)
        {
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;

            #region Prepare the Schema for return Table object

            Table wfPanelDetailsTable = new Table();

            AddColumnToTable(wfPanelDetailsTable, 0, "Id");
            AddColumnToTable(wfPanelDetailsTable, 1, "ParentId");
            AddColumnToTable(wfPanelDetailsTable, 2, "WorkflowAttributeId");
            AddColumnToTable(wfPanelDetailsTable, 3, "Name");
            AddColumnToTable(wfPanelDetailsTable, 4, "LongName");
            AddColumnToTable(wfPanelDetailsTable, 5, "Value");
            AddColumnToTable(wfPanelDetailsTable, 6, "WorkflowVersionNumber");
            AddColumnToTable(wfPanelDetailsTable, 7, "WorkflowVersionName");
            AddColumnToTable(wfPanelDetailsTable, 8, "EntityCount");
            AddColumnToTable(wfPanelDetailsTable, 9, "SortOrder");
            AddColumnToTable(wfPanelDetailsTable, 10, "DisplayUnassignedEntities");
            AddColumnToTable(wfPanelDetailsTable, 11, "DisplayOtherUsersEntities");

            #endregion

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("WorkflowPersistence_SqlParameters");
                parameters = generator.GetParameters("Catalog_GetWorkflowPanel_ParametersArray");

                List<SqlDataRecord> attributeDetailsList = new List<SqlDataRecord>();
                SqlMetaData[] sqlAttributeDetailsList = generator.GetTableValueMetadata("Catalog_GetWorkflowPanel_ParametersArray", parameters[2].ParameterName);
                attributeDetailsList = EntityDataReaderUtility.CreateSearchAttributeTable(searchAttributeRules,  sqlAttributeDetailsList);

                parameters[0].Value = catalogId;
                parameters[1].Value = userId;
                parameters[2].Value = attributeDetailsList;
                parameters[3].Value = showEmptyItems;
                parameters[4].Value = showItemsAssignedToOtherUsers;
                parameters[5].Value = false;


                storedProcedureName = "usp_Workflow_PersistenceManager_WorkflowInstance_WorkflowPanel_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    Object[] values = new Object[reader.FieldCount];
                    reader.GetValues(values);

                    Row wfDetailRow = wfPanelDetailsTable.NewRow();
                    wfDetailRow.SetValue(wfPanelDetailsTable.Columns[0], values[0]);
                    wfDetailRow.SetValue(wfPanelDetailsTable.Columns[1], values[1]);
                    wfDetailRow.SetValue(wfPanelDetailsTable.Columns[2], values[2]);
                    wfDetailRow.SetValue(wfPanelDetailsTable.Columns[3], values[3]);
                    wfDetailRow.SetValue(wfPanelDetailsTable.Columns[4], values[4]);
                    wfDetailRow.SetValue(wfPanelDetailsTable.Columns[5], values[5]);
                    wfDetailRow.SetValue(wfPanelDetailsTable.Columns[6], values[6]);
                    wfDetailRow.SetValue(wfPanelDetailsTable.Columns[7], values[7]);
                    wfDetailRow.SetValue(wfPanelDetailsTable.Columns[8], values[8]);
                    wfDetailRow.SetValue(wfPanelDetailsTable.Columns[9], values[9]);
                    wfDetailRow.SetValue(wfPanelDetailsTable.Columns[10], values[10]);
                    wfDetailRow.SetValue(wfPanelDetailsTable.Columns[11], values[11]);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return wfPanelDetailsTable;
        }

        /// <summary>
        /// Gets the details required for the Workflow UI Panel
        /// </summary>
        /// <param name="catalogId">Id of the catalog for which details are required</param>
        /// <param name="userId">User Id for which details needs to be get</param>
        /// <param name="showEmptyItems">Do not hide empty nodes</param>
        /// <param name="showItemsAssignedToOtherUsers">Include data related to workflows assigned to other users</param>
        /// <param name="command">Object having command properties</param>
        /// <returns>Returns details in Table format</returns>
        public Table GetWorkflowPanelDetails(Int32 catalogId, Int32 userId, Boolean showEmptyItems, Boolean showItemsAssignedToOtherUsers, DBCommandProperties command)
        {
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;

            #region Prepare the Schema for return Table object

            Table wfPanelDetailsTable = new Table();

            AddColumnToTable(wfPanelDetailsTable, 0, "Id");
            AddColumnToTable(wfPanelDetailsTable, 1, "ParentId");
            AddColumnToTable(wfPanelDetailsTable, 2, "WorkflowAttributeId");
            AddColumnToTable(wfPanelDetailsTable, 3, "Name");
            AddColumnToTable(wfPanelDetailsTable, 4, "LongName");
            AddColumnToTable(wfPanelDetailsTable, 5, "Value");
            AddColumnToTable(wfPanelDetailsTable, 6, "WorkflowVersionNumber");
            AddColumnToTable(wfPanelDetailsTable, 7, "WorkflowVersionName");
            AddColumnToTable(wfPanelDetailsTable, 8, "EntityCount");
            AddColumnToTable(wfPanelDetailsTable, 9, "SortOrder");

            #endregion

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("WorkflowPersistence_SqlParameters");

                parameters = generator.GetParameters("Workflow_PersistenceManager_WorkflowInstance_GetWorkflowPanel_ParametersArray");

                parameters[0].Value = catalogId;
                parameters[1].Value = userId;
                parameters[2].Value = showEmptyItems;
                parameters[3].Value = showItemsAssignedToOtherUsers;
                parameters[4].Value = false;

                storedProcedureName = "usp_Workflow_PersistenceManager_WorkflowInstance_WorkflowPanel_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    Object[] values = new Object[reader.FieldCount];
                    reader.GetValues(values);

                    Row wfDetailRow = wfPanelDetailsTable.NewRow();
                    wfDetailRow.SetValue(wfPanelDetailsTable.Columns[0], values[0]);
                    wfDetailRow.SetValue(wfPanelDetailsTable.Columns[1], values[1]);
                    wfDetailRow.SetValue(wfPanelDetailsTable.Columns[2], values[2]);
                    wfDetailRow.SetValue(wfPanelDetailsTable.Columns[3], values[3]);
                    wfDetailRow.SetValue(wfPanelDetailsTable.Columns[4], values[4]);
                    wfDetailRow.SetValue(wfPanelDetailsTable.Columns[5], values[5]);
                    wfDetailRow.SetValue(wfPanelDetailsTable.Columns[6], values[6]);
                    wfDetailRow.SetValue(wfPanelDetailsTable.Columns[7], values[7]);
                    wfDetailRow.SetValue(wfPanelDetailsTable.Columns[8], values[8]);
                    wfDetailRow.SetValue(wfPanelDetailsTable.Columns[9], values[9]);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return wfPanelDetailsTable;
        }

        /// <summary>
        /// Returns users list allowed for specified workflow activity
        /// </summary>
        /// <param name="workflowActivityId">Activity primary key value (optional)</param>
        /// <param name="activityShortName">Activity short name. Will be used if <paramref name="workflowActivityId"/> not specified</param>
        /// <param name="workflowVersionId">Workflow version Id. Will be used if <paramref name="workflowActivityId"/> not specified. Optional. If Null, then exist activity with maximal version will be used.</param>
        public Collection<SecurityUser> GetUsersAllowedForActivity(Int32? workflowActivityId, String activityShortName, Int32? workflowVersionId)
        {
            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;

            Collection<SecurityUser> userCollection = new Collection<SecurityUser>();

            try
            {
                connectionString = AppConfigurationHelper.ConnectionString;

                SqlParametersGenerator generator = new SqlParametersGenerator("WorkflowPersistence_SqlParameters");

                parameters = generator.GetParameters("PersistenceManager_WorkflowInstance_UsersAllowedForActivity_Get_ParametersArray");

                parameters[0].Value = workflowActivityId;
                parameters[1].Value = activityShortName;
                parameters[2].Value = workflowVersionId;

                storedProcedureName = "usp_PersistenceManager_WorkflowInstance_UsersAllowedForActivity_Get";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        Object[] values = new Object[reader.FieldCount];
                        reader.GetValues(values);

                        userCollection.Add(new SecurityUser(values));
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return userCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="catalogId"></param>
        /// <param name="userId"></param>
        /// <param name="showEmptyItems"></param>
        /// <param name="showItemsAssignedToOtherUsers"></param>
        /// <param name="showBusinessCondition"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public Table GetWorkItemsPanelDetails(Int32 catalogId, Int32 userId, Boolean showEmptyItems, Boolean showItemsAssignedToOtherUsers, Boolean showBusinessCondition, DBCommandProperties command)
        {
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;

            #region Prepare the Schema for return Table object

            Table wfItemsDetailsTable = new Table();

            AddColumnToTable(wfItemsDetailsTable, 0, "Id");
            AddColumnToTable(wfItemsDetailsTable, 1, "ParentId");
            AddColumnToTable(wfItemsDetailsTable, 2, "AttributeId");
            AddColumnToTable(wfItemsDetailsTable, 3, "Name");
            AddColumnToTable(wfItemsDetailsTable, 4, "LongName");
            AddColumnToTable(wfItemsDetailsTable, 5, "Value");
            AddColumnToTable(wfItemsDetailsTable, 6, "WorkflowVersionNumber");
            AddColumnToTable(wfItemsDetailsTable, 7, "WorkflowVersionName");
            AddColumnToTable(wfItemsDetailsTable, 8, "EntityCount");
            AddColumnToTable(wfItemsDetailsTable, 9, "SortOrder");
            AddColumnToTable(wfItemsDetailsTable, 10, "IsBusinessCondition");

            #endregion

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("WorkflowPersistence_SqlParameters");

                parameters = generator.GetParameters("Workflow_PersistenceManager_WorkflowInstance_GetWorkflowPanel_ParametersArray");

                parameters[0].Value = catalogId;
                parameters[1].Value = userId;
                parameters[2].Value = showEmptyItems;
                parameters[3].Value = showItemsAssignedToOtherUsers;
                parameters[4].Value = showBusinessCondition;

                storedProcedureName = "usp_Workflow_PersistenceManager_WorkflowInstance_WorkflowPanel_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    Object[] values = new Object[reader.FieldCount];
                    reader.GetValues(values);

                    Row wfDetailRow = wfItemsDetailsTable.NewRow();

                    if (values[0].ToString() == values[1].ToString())
                    {
                        wfDetailRow.SetValue(wfItemsDetailsTable.Columns[0], values[0]);
                        wfDetailRow.SetValue(wfItemsDetailsTable.Columns[1], 0);
                    }
                    else
                    {
                        wfDetailRow.SetValue(wfItemsDetailsTable.Columns[0], values[0]);
                        wfDetailRow.SetValue(wfItemsDetailsTable.Columns[1], values[1]);
                    }

                    wfDetailRow.SetValue(wfItemsDetailsTable.Columns[2], values[2]);
                    wfDetailRow.SetValue(wfItemsDetailsTable.Columns[3], values[3]);
                    wfDetailRow.SetValue(wfItemsDetailsTable.Columns[4], values[4]);
                    wfDetailRow.SetValue(wfItemsDetailsTable.Columns[5], values[5]);
                    wfDetailRow.SetValue(wfItemsDetailsTable.Columns[6], values[6]);
                    wfDetailRow.SetValue(wfItemsDetailsTable.Columns[7], values[7]);
                    wfDetailRow.SetValue(wfItemsDetailsTable.Columns[8], values[8]);
                    wfDetailRow.SetValue(wfItemsDetailsTable.Columns[9], values[9]);
                    wfDetailRow.SetValue(wfItemsDetailsTable.Columns[10], values[10]);
                }

                Row wfDetailGlobalRow = wfItemsDetailsTable.NewRow();

                wfDetailGlobalRow.SetValue(wfItemsDetailsTable.Columns[0], 0);
                wfDetailGlobalRow.SetValue(wfItemsDetailsTable.Columns[1], System.DBNull.Value);
                wfDetailGlobalRow.SetValue(wfItemsDetailsTable.Columns[2], 0);
                wfDetailGlobalRow.SetValue(wfItemsDetailsTable.Columns[4], "Global"); //TODO Needs to localize
                wfDetailGlobalRow.SetValue(wfItemsDetailsTable.Columns[10], 1);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return wfItemsDetailsTable;
        }

        /// <summary>
        /// Gets role based work items details including workflow activities and business conditions for given entity id and user id
        /// </summary>
        /// <param name="entityId">Indicates entityId</param>
        /// <param name="userId">Indicates user id</param>
        /// <param name="command">Indicates DBCommandProperties</param>
        /// <returns>Returns a Table with all required data</returns>
        public Table GetWorkItemDetails(Int64 entityId, Int32 userId, DBCommandProperties command)
        {
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = "usp_EntityManager_Entity_BusinessCondition_UserRole_Get";

            #region Diagnostics & tracing

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            var currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            Boolean isTracingEnabled = currentTraceSettings.IsBasicTracingEnabled;

            if (isTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            #endregion Diagnostics & tracing

            #region Prepare columns for to-be returned Table object

            Table workItemsDetailsTable = new Table();
            AddColumnToTable(workItemsDetailsTable, 0, "Id");
            AddColumnToTable(workItemsDetailsTable, 1, "ParentID");
            AddColumnToTable(workItemsDetailsTable, 2, "ObjectId");
            AddColumnToTable(workItemsDetailsTable, 3, "ShortName");
            AddColumnToTable(workItemsDetailsTable, 4, "LongName");
            AddColumnToTable(workItemsDetailsTable, 5, "NodeType");
            AddColumnToTable(workItemsDetailsTable, 6, "Status");
            AddColumnToTable(workItemsDetailsTable, 7, "ContainerId");
            AddColumnToTable(workItemsDetailsTable, 8, "EntityIdList");
            
            #endregion Prepare columns for to-be returned Table object

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("WorkflowPersistence_SqlParameters");
                parameters = generator.GetParameters("Workflow_PersistenceManager_WorkflowInstance_GetWorkItems_ParametersArray");

                parameters[0].Value = entityId;
                parameters[1].Value = userId;
               
                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                #region Fill values into Table

                while (reader.Read())
                {
                    Object[] values = new Object[reader.FieldCount];
                    reader.GetValues(values);

                    Row workItemDetailRow = workItemsDetailsTable.NewRow();
                    workItemDetailRow.SetValue(workItemsDetailsTable.Columns[0], values[0]);
                    workItemDetailRow.SetValue(workItemsDetailsTable.Columns[1], values[1]);
                    workItemDetailRow.SetValue(workItemsDetailsTable.Columns[2], values[2]);
                    workItemDetailRow.SetValue(workItemsDetailsTable.Columns[3], values[3]);
                    workItemDetailRow.SetValue(workItemsDetailsTable.Columns[4], values[4]);
                    workItemDetailRow.SetValue(workItemsDetailsTable.Columns[5], values[5]);
                    workItemDetailRow.SetValue(workItemsDetailsTable.Columns[6], values[6]);
                    workItemDetailRow.SetValue(workItemsDetailsTable.Columns[7], values[7]);
                    workItemDetailRow.SetValue(workItemsDetailsTable.Columns[8], values[8]);
                }

                #endregion Fill values into Table
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("WorkItemDetails Get is completed.");
                    diagnosticActivity.Stop();
                }
            }

            return workItemsDetailsTable;
        }
        
        #endregion

        #region Workflow Execution View Details Methods

        /// <summary>
        /// Get the Workflow Execution Details based on the Entity Id and the Workflow Long Name
        /// </summary>
        /// <param name="entityId">Specifies Id of Entity</param>
        /// <param name="workflowName">Specifies long name of Workflow</param>
        /// <param name="getAll">Specifies whether all activity details or only current activity details are to be retrieved."
        /// <param name="command">Object having command properties</param>
        /// <returns>Returns the TrackedActivityInfoCollection object</returns>
        public MDMBOW.TrackedActivityInfoCollection GetWorkflowExecutionDetails(Int64 entityId, String workflowName, DBCommandProperties command, Boolean getAll = false)
        {
            MDMBOW.TrackedActivityInfoCollection trackedActivityInfoCollection = null;
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;

            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowInstanceDA.GetWorkflowExecutionDetails", false);

            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("WorkflowPersistence_SqlParameters");

                parameters = generator.GetParameters("PersistenceManager_WorkflowInstance_ExecutionDetails_Get_ParametersArray");

                parameters[0].Value = entityId;
                parameters[1].Value = workflowName;
                parameters[2].Value = getAll;

                storedProcedureName = "usp_PersistenceManager_WorkflowInstance_ExecutionDetails_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    trackedActivityInfoCollection = ReadWorkflowExecutionDetails(reader, trackedActivityInfoCollection);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("WorkflowInstanceDA.GetWorkflowExecutionDetails");
            }

            return trackedActivityInfoCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityGUID"></param>
        /// <param name="workflowName"></param>
        /// <param name="command"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public MDMBOW.TrackedActivityInfoCollection GetWorkfloExecutionDetails(String entityGUID, String workflowName, DBCommandProperties command, Boolean getAll = false)
        {
            MDMBOW.TrackedActivityInfoCollection trackedActivityInfoCollection = null;
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;

            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowInstanceDA.GetWorkflowExecutionDetails", false);

            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            String mdmObjectType = "MDM.BusinessObjects.Entity";

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("WorkflowPersistence_SqlParameters");

                parameters = generator.GetParameters("PersistenceManager_WorkflowInstance_ExecutionDetails_Get_ParametersArray");

                parameters[0].Value = mdmObjectType;
                parameters[1].Value = entityGUID;
                parameters[2].Value = workflowName;
                parameters[3].Value = getAll;

                storedProcedureName = "usp_PersistenceManager_WorkflowInstance_ExecutionDetails_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    trackedActivityInfoCollection = ReadWorkflowExecutionDetails(reader, trackedActivityInfoCollection);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("WorkflowInstanceDA.GetWorkflowExecutionDetails");
            }

            return trackedActivityInfoCollection;
        }

        public Collection<DoneReportItem> GetWorkflowDoneReport(Int32 userId, String userParticipation, Collection<Int32> catalogIds, Collection<Int64> entityTypeIds, Collection<String> workflowNames, Collection<String> currentWorkflowActivity, Collection<Int32> attributeIds, Int32 localeId, Int64 countFrom, Int64 countTo, String attributesDataSource, out Int64 totalCount, DBCommandProperties command)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            Collection<DoneReportItem> resultItems = new Collection<DoneReportItem>();

            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("WorkflowInstanceDA.GetWorkflowDoneReport", false);

            SqlDataReader reader = null;
            SqlParameter[] parameters = null;
            String storedProcedureName = String.Empty;
            totalCount = 0;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("WorkflowPersistence_SqlParameters");

                const String paramsKey = "PersistenceManager_WorkflowInstance_WorkflowDoneReport_Get_ParametersArray";
                parameters = generator.GetParameters(paramsKey);

                parameters[0].Value = userId;
                parameters[1].Value = userParticipation;
                parameters[2].Value = generator.PrepareCollectionTypedParameterValue(catalogIds, paramsKey, parameters[2].ParameterName);
                parameters[3].Value = generator.PrepareCollectionTypedParameterValue(entityTypeIds, paramsKey, parameters[3].ParameterName);
                parameters[4].Value = generator.PrepareCollectionTypedParameterValue(workflowNames, paramsKey, parameters[4].ParameterName);
                parameters[5].Value = generator.PrepareCollectionTypedParameterValue(currentWorkflowActivity, paramsKey, parameters[5].ParameterName);

                parameters[6].Value = generator.PrepareCollectionTypedParameterValue(attributeIds, paramsKey, parameters[6].ParameterName);
                parameters[7].Value = localeId;
                parameters[8].Value = countFrom;
                parameters[9].Value = countTo;
                //parameters[10] is TotalCount ouput parameter
                if (String.IsNullOrWhiteSpace(attributesDataSource))
                {
                    parameters[11].Value = DBNull.Value;
                }
                else
                {
                    parameters[11].Value = attributesDataSource;
                }

                storedProcedureName = "usp_PersistenceManager_WorkflowInstance_WorkflowDoneReport_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    GetDoneReportItemsData(reader, resultItems);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (parameters != null)
                {
                    totalCount = (Int64)parameters[10].Value;
                }

                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("WorkflowInstanceDA.GetWorkflowDoneReport");
            }

            return resultItems;
        }

        #endregion Workflow Execution View Details Methods

        #endregion Public methods

        #region Private methods

        private Collection<Int64> PopulateRunningInstanceDetails(SqlDataReader reader)
        {
            Collection<Int64> runningInstanceDetails = new Collection<Int64>();

            if (reader != null)
            {
                while (reader.Read())
                {
                    if (reader["EntityId"] != null)
                    {
                        Int64 entityId = ValueTypeHelper.Int64TryParse(reader["EntityId"].ToString(), 0);
                        runningInstanceDetails.Add(entityId);
                    }
                }

                reader.Close();
            }

            return runningInstanceDetails;
        }

        private static void GetDoneReportItemsData(SqlDataReader reader, Collection<DoneReportItem> reportItems)
        {
            while (reader.Read())
            {
                Int64 cNodeId = 0;
                String name = String.Empty;
                String lName = String.Empty;
                Int64 parentId = 0;
                String parentName = String.Empty;
                String parentPath = String.Empty;
                Int32 categoryId = 0;
                String categoryName = String.Empty;
                String categoryLongName = String.Empty;
                Int32 catalogId = 0;
                String catalogName = String.Empty;
                String catalogLongName = String.Empty;
                Int32 orgId = -1;
                String orgName = String.Empty;
                String orgLongName = String.Empty;
                Int32 entityTypeId = -1;
                String entityTypeDesc = String.Empty;
                DateTime? workflowFirstActivityTime = null;
                DateTime? workflowLastActivityTime = null;

                if (reader["CNodeID"] != null)
                    Int64.TryParse(reader["CNodeID"].ToString(), out cNodeId);
                if (reader["ShortName"] != null)
                    name = reader["ShortName"].ToString();
                if (reader["LongName"] != null)
                    lName = reader["LongName"].ToString();
                if (reader["CategoryId"] != null)
                    Int32.TryParse(reader["CategoryId"].ToString(), out categoryId);
                if (reader["CategoryName"] != null)
                    categoryName = reader["CategoryName"].ToString();
                if (reader["CategoryLongName"] != null)
                    categoryLongName = reader["CategoryLongName"].ToString();
                if (reader["ParentName"] != null)
                    parentName = reader["ParentName"].ToString();
                if (reader["ParentPath"] != null)
                    parentPath = reader["ParentPath"].ToString();
                if (reader["ParentID"] != null)
                    Int64.TryParse(reader["ParentID"].ToString(), out parentId);
                if (reader["CatalogID"] != null)
                    Int32.TryParse(reader["CatalogID"].ToString(), out catalogId);
                if (reader["CatalogName"] != null)
                    catalogName = reader["CatalogName"].ToString();
                if (reader["CatalogLongName"] != null)
                    catalogLongName = reader["CatalogLongName"].ToString();
                if (reader["OrgID"] != null)
                    Int32.TryParse(reader["OrgID"].ToString(), out orgId);
                if (reader["OrgName"] != null)
                    orgName = reader["OrgName"].ToString();
                if (reader["OrgLongName"] != null)
                    orgLongName = reader["OrgLongName"].ToString();
                if (reader["FK_NodeType"] != null)
                    Int32.TryParse(reader["FK_NodeType"].ToString(), out entityTypeId);
                if (reader["FK_NodeTypeDesc"] != null)
                    entityTypeDesc = reader["FK_NodeTypeDesc"].ToString();
                if (!Convert.IsDBNull(reader["WorkflowFirstActivityTime"]))
                    workflowFirstActivityTime = (DateTime)reader["WorkflowFirstActivityTime"];
                if (!Convert.IsDBNull(reader["WorkflowLastActivityTime"]))
                    workflowLastActivityTime = (DateTime)reader["WorkflowLastActivityTime"];

                //Prepare DoneReportItem object
                DoneReportItem item = new DoneReportItem();

                item.Id = cNodeId;
                item.Name = name;
                item.LongName = lName;
                item.ParentId = parentId;
                item.ParentName = parentName;
                item.ParentPath = parentPath;
                item.CategoryId = categoryId;
                item.CategoryName = categoryName;
                item.CategoryLongName = categoryLongName;
                item.CatalogId = catalogId;
                item.CatalogName = catalogName;
                item.CatalogLongName = catalogLongName;
                item.OrganizationId = orgId;
                item.OrganizationName = orgName;
                item.OrganizationLongName = orgLongName;
                item.NodeTypeId = entityTypeId;
                item.NodeTypeDescription = entityTypeDesc;
                item.WorkflowFirstActivityTime = workflowFirstActivityTime;
                item.WorkflowLastActivityTime = workflowLastActivityTime;

                //Add to collection
                reportItems.Add(item);
            }
            reader.NextResult();
            while (reader.Read())
            {
                Int64 cNodeId = 0;
                Int32 attrId = 0;
                Int32 localeId = 0;
                String attrValue = String.Empty;
                LocaleEnum locale = LocaleEnum.UnKnown;
                String uom = String.Empty;
                if (reader["CNodeID"] != null)
                    Int64.TryParse(reader["CNodeID"].ToString(), out cNodeId);
                if (reader["AttrId"] != null)
                    Int32.TryParse(reader["AttrId"].ToString(), out attrId);
                if (reader["AttrVal"] != null)
                    attrValue = reader["AttrVal"].ToString();
                if (reader["LocaleId"] != null)
                {
                    Int32.TryParse(reader["LocaleId"].ToString(), out localeId);
                    locale = (LocaleEnum)localeId;
                }
                if (reader["UOM"] != null)
                    uom = reader["UOM"].ToString();

                foreach (var reportItem in reportItems)
                {
                    if (reportItem.Id == cNodeId)
                    {
                        ValueCollection values = null;
                        if (!reportItem.AttributesValues.TryGetValue(attrId, out values))
                        {
                            values = new ValueCollection();
                            reportItem.AttributesValues.Add(attrId, values);
                        }
                        Value value = new Value((object)attrValue);
                        value.Locale = locale;
                        value.Uom = uom;
                        values.Add(value);
                    }
                }
            }

            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    Int64 cNodeId = 0;
                    String runtimeInstanceId = String.Empty;
                    Int32 workflowId = 0;
                    String workflowShortName = String.Empty;
                    String workflowLongName = String.Empty;
                    Int32 workflowVersionId = 0;
                    Int32 activityId = 0;
                    String activityShortName = String.Empty;
                    String activityLongName = String.Empty;
                    Int32? actingUserId = null;
                    String actingUserLogin = String.Empty;
                    String actingUserFirstName = String.Empty;
                    String actingUserLastName = String.Empty;

                    if (reader["CNodeID"] != null)
                        Int64.TryParse(reader["CNodeID"].ToString(), out cNodeId);
                    if (reader["Runtime_Instance_Id"] != null)
                        runtimeInstanceId = reader["Runtime_Instance_Id"].ToString();
                    if (reader["WorkflowId"] != null)
                        Int32.TryParse(reader["WorkflowId"].ToString(), out workflowId);
                    if (reader["WorkflowShortName"] != null)
                        workflowShortName = reader["WorkflowShortName"].ToString();
                    if (reader["WorkflowLongName"] != null)
                        workflowLongName = reader["WorkflowLongName"].ToString();
                    if (reader["WorkflowVersionId"] != null)
                        Int32.TryParse(reader["WorkflowVersionId"].ToString(), out workflowVersionId);
                    if (reader["ActivityId"] != null)
                        Int32.TryParse(reader["ActivityId"].ToString(), out activityId);
                    if (reader["ActivityShortName"] != null)
                        activityShortName = reader["ActivityShortName"].ToString();
                    if (reader["ActivityLongName"] != null)
                        activityLongName = reader["ActivityLongName"].ToString();
                    if (reader["UserId"] != null)
                    {
                        Int32 userIdTmp;
                        Int32.TryParse(reader["UserId"].ToString(), out userIdTmp);
                        actingUserId = userIdTmp;

                        if (reader["UserLogin"] != null)
                            actingUserLogin = reader["UserLogin"].ToString();
                        if (reader["UserFirstName"] != null)
                            actingUserFirstName = reader["UserFirstName"].ToString();
                        if (reader["UserLastName"] != null)
                            actingUserLastName = reader["UserLastName"].ToString();
                    }

                    foreach (var reportItem in reportItems)
                    {
                        if (reportItem.Id == cNodeId)
                        {
                            reportItem.WorkflowInfo.Add(new DoneReportItemWorkflowInfo
                                {
                                    Id = cNodeId,
                                    RuntimeInstanceId = runtimeInstanceId,
                                    WorkflowId = workflowId,
                                    WorkflowShortName = workflowShortName,
                                    WorkflowLongName = workflowLongName,
                                    WorkflowVersionId = workflowVersionId,
                                    ActivityId = activityId,
                                    ActivityShortName = activityShortName,
                                    ActivityLongName = activityLongName,
                                    ActingUserId = actingUserId,
                                    ActingUserLogin = actingUserLogin,
                                    ActingUserFirstName = actingUserFirstName,
                                    ActingUserLastName = actingUserLastName
                                });
                        }
                    }
                }
            }
        }

        private MDMBOW.TrackedActivityInfoCollection ReadWorkflowExecutionDetails(SqlDataReader reader, MDMBOW.TrackedActivityInfoCollection trackedActivityInfoCollection)
        {
            trackedActivityInfoCollection = new MDMBOW.TrackedActivityInfoCollection();

            while (reader.Read())
            {
                MDMBOW.TrackedActivityInfo trackedActivityInfo = new MDMBOW.TrackedActivityInfo();

                if (reader["WorkflowId"] != null)
                {
                    trackedActivityInfo.WorkflowId = ValueTypeHelper.Int32TryParse(reader["WorkflowId"].ToString(), -1);
                }

                if (reader["WorkflowName"] != null)
                {
                    trackedActivityInfo.WorkflowName = reader["WorkflowName"].ToString();
                }

                if (reader["WorkflowLongName"] != null)
                {
                    trackedActivityInfo.WorkflowLongName = reader["WorkflowLongName"].ToString();
                }

                if (reader["WorkflowVersionId"] != null)
                {
                    trackedActivityInfo.WorkflowVersionId = ValueTypeHelper.Int32TryParse(reader["WorkflowVersionId"].ToString(), -1);
                }

                if (reader["WorkflowVersionName"] != null)
                {
                    trackedActivityInfo.WorkflowVersionName = reader["WorkflowVersionName"].ToString();
                }

                if (reader["WorkflowInstanceId"] != null)
                {
                    trackedActivityInfo.RuntimeInstanceId = reader["WorkflowInstanceId"].ToString();
                }

                if (reader["ActivityShortName"] != null)
                {
                    trackedActivityInfo.ActivityShortName = reader["ActivityShortName"].ToString();
                }

                if (reader["ActivityLongName"] != null)
                {
                    trackedActivityInfo.ActivityLongName = reader["ActivityLongName"].ToString();
                }

                if (reader["IsExecuting"] != null)
                {
                    trackedActivityInfo.IsExecuting = ValueTypeHelper.ConvertToBoolean(reader["IsExecuting"].ToString());
                }

                if (reader["Status"] != null)
                {
                    trackedActivityInfo.Status = reader["Status"].ToString();
                }

                if (reader["PerformedAction"] != null)
                {
                    trackedActivityInfo.PerformedAction = reader["PerformedAction"].ToString();
                }

                if (reader["ActingUserId"] != null)
                {
                    trackedActivityInfo.ActingUserId = ValueTypeHelper.Int32TryParse(reader["ActingUserId"].ToString(), -1);
                }

                if (reader["AllowedRoles"] != null)
                {
                    trackedActivityInfo.AssignedRoles = reader["AllowedRoles"].ToString();
                }

                if (reader["ActingUserName"] != null)
                {
                    trackedActivityInfo.ActingUser = reader["ActingUserName"].ToString();
                }

                if (reader["ActedUserId"] != null)
                {
                    trackedActivityInfo.ActedUserId = ValueTypeHelper.Int32TryParse(reader["ActedUserId"].ToString(), -1);
                }

                if (reader["ActedUserName"] != null)
                {
                    trackedActivityInfo.ActedUser = reader["ActedUserName"].ToString();
                }

                if (reader["SMTP"] != null)
                {
                    trackedActivityInfo.UserMailAddress = reader["SMTP"].ToString();
                }

                if (reader["ActivityComments"] != null)
                {
                    trackedActivityInfo.ActivityComments = reader["ActivityComments"].ToString();
                }

                if (reader["WorkflowComments"] != null)
                {
                    trackedActivityInfo.WorkflowComments = reader["WorkflowComments"].ToString();
                }

                if (reader["EventDate"] != null)
                {
                    trackedActivityInfo.EventDate = reader["EventDate"].ToString();
                }

                if (reader["ActivityId"] != null)
                {
                    trackedActivityInfo.WorkflowDefinitionActivityID = reader["ActivityId"].ToString();
                }

                if (reader["PreviousActivityShortName"] != null)
                {
                    trackedActivityInfo.PreviousActivityShortName = reader["PreviousActivityShortName"].ToString();
                }

                if (reader["PreviousActivityStartDateTime"] != null)
                {
                    trackedActivityInfo.PreviousActivityStartDateTime = reader["PreviousActivityStartDateTime"].ToString();
                }

                if (reader["LastActivityComments"] != null)
                {
                    trackedActivityInfo.LastActivityComments = reader["LastActivityComments"].ToString();
                }

                trackedActivityInfoCollection.Add(trackedActivityInfo);
            }

            return trackedActivityInfoCollection;
        }

        private String ConvertToXmlForWorkflow(Collection<MDMBOW.Workflow> workflowCollection)
        {
            String workflowXml = "<Workflows>";

            if (workflowCollection != null && workflowCollection.Count > 0)
            {
                foreach (MDMBOW.Workflow workflow in workflowCollection)
                {
                    workflowXml = String.Concat(workflowXml, workflow.ToXML());
                }
            }

            workflowXml = String.Concat(workflowXml, "</Workflows>");
            return workflowXml;
        }

        private String ConvertToXmlForWorkflowInstance(Collection<MDMBOW.WorkflowInstance> workflowInstanceCollection)
        {
            String workflowInstanceXml = "<WorkflowInstances>";

            if (workflowInstanceCollection != null && workflowInstanceCollection.Count > 0)
            {
                foreach (MDMBOW.WorkflowInstance instance in workflowInstanceCollection)
                {
                    workflowInstanceXml = String.Concat(workflowInstanceXml, instance.ToXML());
                }
            }

            workflowInstanceXml = String.Concat(workflowInstanceXml, "</WorkflowInstances>");
            return workflowInstanceXml;
        }

        private void CreateInstanceTable(Collection<MDMBOW.WorkflowInstance> workflowInstances, SqlMetaData[] instanceMetadata,
                                       SqlMetaData[] mdmObjectMetaData, out List<SqlDataRecord> instanceTable,
                                       out List<SqlDataRecord> mdmObjectTable)
        {
            instanceTable = new List<SqlDataRecord>();
            mdmObjectTable = new List<SqlDataRecord>();

            if (workflowInstances != null && workflowInstances.Count > 0)
            {
                foreach (MDMBOW.WorkflowInstance instance in workflowInstances)
                {
                    SqlDataRecord instanceRecord = new SqlDataRecord(instanceMetadata);

                    instanceRecord.SetValue(0, instance.Id);
                    instanceRecord.SetValue(1, instance.WorkflowVersionId);
                    instanceRecord.SetValue(2, instance.RuntimeInstanceId);
                    instanceRecord.SetValue(3, instance.Status);
                    instanceRecord.SetValue(4, instance.WorkflowComments);

                    if (instance.WorkflowMDMObjects != null && instance.WorkflowMDMObjects.Count > 0)
                    {
                        CreateMDMObjectTable(instance.WorkflowMDMObjects, mdmObjectTable, mdmObjectMetaData, instance.Id, instance.RuntimeInstanceId);
                    }

                    instanceTable.Add(instanceRecord);
                }
            }

            //we cannot pass no records in the SqlDataRecord enumeration. To send a table-valued parameter with no rows, use a null reference for the value instead.
            if (instanceTable.Count == 0)
                instanceTable = null;

            if (mdmObjectTable.Count == 0)
                mdmObjectTable = null;
        }

        private void CreateMDMObjectTable(MDMBOW.WorkflowMDMObjectCollection workflowMDMObjects, List<SqlDataRecord> mdmObjectTable, SqlMetaData[] mdmObjectMetaData, Int32 instanceId, String runtimeInstanceId)
        {
            foreach (MDMBOW.WorkflowMDMObject mdmObject in workflowMDMObjects)
            {
                SqlDataRecord mdmObjectRecord = new SqlDataRecord(mdmObjectMetaData);

                mdmObjectRecord.SetValue(0, instanceId);
                mdmObjectRecord.SetValue(1, runtimeInstanceId);
                mdmObjectRecord.SetValue(2, mdmObject.MDMObjectGUID);
                mdmObjectRecord.SetValue(3, mdmObject.MDMObjectType);

                mdmObjectTable.Add(mdmObjectRecord);
            }
        }

        private Collection<MDMBOW.WorkflowInstance> GetObjectCollectionFromXml(String valuesAsXml)
        {
            /*
             * Sample
             * <WorkflowInstances>
                  <WorkflowInstance RuntimeInstanceId="34kjthek-8853-4ff8-9197-3e644436d4bc" MDMObjectId="3" MDMObjectType="Entity" WorkflowVersionId="2" STATUS="Idle" />
                  <WorkflowInstance RuntimeInstanceId="4dd68c6b-8853-4ff8-9197-3e644436d4bc" MDMObjectId="2" MDMObjectType="Entity" WorkflowVersionId="2" STATUS="Idle" />
               </WorkflowInstances>
             */
            Collection<MDMBOW.WorkflowInstance> workflowInstanceCollection = null;

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                workflowInstanceCollection = new Collection<MDMBOW.WorkflowInstance>();
                XmlTextReader reader = null;
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "WorkflowInstance")
                    {
                        String workflowInstanceXml = reader.ReadOuterXml();
                        if (!String.IsNullOrEmpty(workflowInstanceXml))
                        {
                            MDMBOW.WorkflowInstance workflowInstance = new MDMBOW.WorkflowInstance(workflowInstanceXml);
                            if (workflowInstance != null)
                            {
                                workflowInstanceCollection.Add(workflowInstance);
                            }
                        }

                    }
                    else
                    {
                        reader.Read();
                    }
                }

                if (reader != null)
                {
                    reader.Close();
                }
            }

            return workflowInstanceCollection;
        }

        private void AddColumnToTable(Table table, Int32 columnId, String columnName)
        {
            Column column = new Column(columnId, columnName, columnName, null);
            table.Columns.Add(column);
        }

        #endregion Private methods

        #endregion Methods
    }
}
