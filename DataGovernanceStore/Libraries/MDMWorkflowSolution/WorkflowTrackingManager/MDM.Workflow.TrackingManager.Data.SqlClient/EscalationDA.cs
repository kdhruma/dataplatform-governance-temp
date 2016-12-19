using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;

namespace MDM.Workflow.TrackingManager.Data
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Workflow;
    using MDM.Core;
    using MDM.Utility;
   

    /// <summary>
    /// Data processing logic for Escalation
    /// </summary>
    public class EscalationDA : SqlClientDataAccessBase
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructors

        #endregion

        #region Public Methods

        /// <summary>
        /// Processes Escalation data
        /// </summary>
        /// <param name="command">DBCommand containing information about connection string</param>
        /// <returns>Returns the records regarding Escalations</returns>
        public Collection<Escalation> Process(DBCommandProperties command)
        {
            SqlDataReader reader = null;
            String storedProcedureName = String.Empty;
            Collection<Escalation> escalationData = new Collection<Escalation>();

            try
            {
                storedProcedureName = "usp_Workflow_TrackingManager_Escalation_Process";

                reader = ExecuteProcedureReader(command.ConnectionString, null, storedProcedureName);

                escalationData = new Collection<Escalation>();

                while (reader.Read())
                {
                    Object[] values = new Object[reader.FieldCount];
                    reader.GetValues(values);

                    Escalation escalation = new Escalation(values);
                    escalationData.Add(escalation);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return escalationData;
        }

        /// <summary>
        /// Get the workflow elapsed time details for the requested entities based on the escalation context
        /// </summary>
        /// <param name="escalationContext">Indicates the workflow escalation context</param>
        /// <param name="callerContext">Indicates the callerContext which contains the MDMCenter application and module</param>
        /// <returns>Returns the workflow escalation details as collection. </returns>
        public WorkflowEscalationDataCollection GetWorkflowEscalationDetails(WorkflowEscalationContext escalationContext, DBCommandProperties command)
        {
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            WorkflowEscalationDataCollection wfEscalations = null;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("TrackingManager_SqlParameters");
                String parameterName = "TrackingManager_Escalation_ElapsedTime_Get_ParametersArray";
                List<SqlDataRecord> activityTable = null;
                List<SqlDataRecord> workflowTable = null;
                List<SqlDataRecord> entityTable = null;

                parameters = generator.GetParameters(parameterName);
                SqlMetaData[] workflowMetaData = generator.GetTableValueMetadata(parameterName, parameters[0].ParameterName);
                SqlMetaData[] activityMetaData = generator.GetTableValueMetadata(parameterName, parameters[1].ParameterName);
                SqlMetaData[] entityMetaData = generator.GetTableValueMetadata(parameterName, parameters[2].ParameterName);

                if (this.ContainsActivityName(escalationContext))
                {
                    activityTable = this.CreateActivityTable(escalationContext, activityMetaData);
                }
                else
                {
                    workflowTable = this.CreateWorkflowTable(escalationContext, workflowMetaData);
                }
                
                entityTable = this.CreateEntityTable(escalationContext, entityMetaData);

                parameters[0].Value = workflowTable;
                parameters[1].Value = activityTable;
                parameters[2].Value = entityTable;
                parameters[3].Value = escalationContext.UserLogin;
                parameters[4].Value = escalationContext.ElapsedTime;
                storedProcedureName = "usp_Workflow_TrackingManager_workflowElapsedTime_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                wfEscalations = FillWorkflowEscalationDetails(reader);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return wfEscalations;
        }

        #endregion

        #region Private Methods

        private List<SqlDataRecord> CreateActivityTable(WorkflowEscalationContext escalationContext, SqlMetaData[] activityMetaData)
        {
            List<SqlDataRecord> activityTable = new List<SqlDataRecord>();

            var workflowNames = escalationContext.GetWorkflowNames();

            if (workflowNames != null && workflowNames.Count > 0)
            {
                foreach (String workflowName in workflowNames)
                {
                    var activities = escalationContext.GetActivityNames(workflowName);

                    if (activities != null && activities.Count > 0)
                    {
                        foreach (String activityName in activities)
                        {
                            var activityRecord = new SqlDataRecord(activityMetaData);
                            activityRecord.SetValue(0, workflowName);

                            activityRecord.SetValue(1, activityName);
                            activityTable.Add(activityRecord);
                        }
                    }
                }
            }
            return activityTable;
        }

        private List<SqlDataRecord> CreateWorkflowTable(WorkflowEscalationContext escalationContext, SqlMetaData[] workflowMetaData)
        {
            List<SqlDataRecord> workflowTable = null;

            var workflowNames = escalationContext.GetWorkflowNames();

            if (workflowNames != null && workflowNames.Count > 0)
            {
                workflowTable = new List<SqlDataRecord>();

                foreach (String workflowName in workflowNames)
                {
                    var workflowRecord = new SqlDataRecord(workflowMetaData);
                    workflowRecord.SetValue(0, workflowName);

                    workflowTable.Add(workflowRecord);
                }
            }

            return workflowTable;
        }

        private Boolean ContainsActivityName(WorkflowEscalationContext escalationContext)
        {
            Boolean result = false;

            var workflowNames = escalationContext.GetWorkflowNames();

            if (workflowNames != null && workflowNames.Count > 0)
            {
                foreach (String workflowName in workflowNames)
                {
                    var activities = escalationContext.GetActivityNames(workflowName);

                    if (activities != null && activities.Count > 0)
                    {
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }

        private List<SqlDataRecord> CreateEntityTable(WorkflowEscalationContext escalationContext, SqlMetaData[] entityMetaData)
        {
            List<SqlDataRecord> entityTable = null;
            if (escalationContext.EntityIds != null)
            {
                entityTable = new List<SqlDataRecord>();

                foreach (Int64 entityId in escalationContext.EntityIds)
                {
                    var workflowRecord = new SqlDataRecord(entityMetaData);
                    workflowRecord.SetValue(0, entityId);

                    entityTable.Add(workflowRecord);
                }
            }
            return entityTable;
        }

        private WorkflowEscalationDataCollection FillWorkflowEscalationDetails(SqlDataReader reader)
        {
            WorkflowEscalationDataCollection wfEscalations = null;

            if (reader != null)
            {
                wfEscalations = new WorkflowEscalationDataCollection();
                while (reader.Read())
                {
                    WorkflowEscalationData wfEscalation = new WorkflowEscalationData();

                    if (reader["EntityId"] != null)
                    {
                        wfEscalation.EntityId = ValueTypeHelper.Int64TryParse(reader["EntityId"].ToString(), 0);
                    }
                    if (reader["WorkflowName"] != null)
                    {
                        wfEscalation.WorkflowName = reader["WorkflowName"].ToString();
                    }
                    if (reader["WorkflowLongName"] != null)
                    {
                        wfEscalation.WorkflowLongName = reader["WorkflowLongName"].ToString();
                    }
                    if (reader["ActivityName"] != null)
                    {
                        wfEscalation.ActivityName = reader["ActivityName"].ToString();
                    }
                    if (reader["ActivityLongName"] != null)
                    {
                        wfEscalation.ActivityLongName = reader["ActivityLongName"].ToString();
                    }
                    if (reader["UserEmail"] != null)
                    {
                        wfEscalation.AssignedUserMailAddress = reader["UserEmail"].ToString();
                    }
                    if (reader["Login"] != null)
                    {
                        wfEscalation.AssignedUserLogin = reader["Login"].ToString();
                    }
                    if (reader["UserId"] != null)
                    {
                        wfEscalation.AssignedUserId = ValueTypeHelper.Int32TryParse(reader["UserId"].ToString(), 0);
                    }
                    if (reader["ElapsedTime"] != null)
                    {
                        wfEscalation.ElapsedTime = ValueTypeHelper.Int32TryParse(reader["ElapsedTime"].ToString(), 0);
                    }

                    wfEscalations.Add(wfEscalation);
                }
            }
            return wfEscalations;
        }

        #endregion
    }
}
