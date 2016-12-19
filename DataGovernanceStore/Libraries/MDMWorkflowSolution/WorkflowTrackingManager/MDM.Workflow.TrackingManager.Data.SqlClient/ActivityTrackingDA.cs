using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Transactions;


namespace MDM.Workflow.TrackingManager.Data
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Workflow;
    using Microsoft.SqlServer.Server;

    /// <summary>
    /// Data access logic for Instance Tracking Record
    /// </summary>
    public class ActivityTrackingDA : SqlClientDataAccessBase
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructors

        #endregion

        #region Methods

        /// <summary>
        /// Creates the Activity Tracking Record into the database.
        /// </summary>
        /// <param name="activityRecord">ActivityTrackingRecord</param>
        /// <param name="command">DBCommand containing information about connection string</param>
        /// <returns>The number of records affected by the query</returns>
        public Int32 Create(TrackedActivityInfo activityRecord, DBCommandProperties command)
        {
            Int32 output = 0; //success

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                SqlParameter[] parameters;
                String storedProcedureName = String.Empty;

                List<SqlDataRecord> activityRecordTable;
                List<SqlDataRecord> mdmObjectTable;

                SqlParametersGenerator generator = new SqlParametersGenerator("TrackingManager_SqlParameters");
                parameters = generator.GetParameters("TrackingManager_ActivityTracking_Process_ParametersArray");

                SqlMetaData[] activityRecordMetadata = generator.GetTableValueMetadata("TrackingManager_ActivityTracking_Process_ParametersArray", parameters[0].ParameterName);
                SqlMetaData[] mdmObjectMetadata = generator.GetTableValueMetadata("TrackingManager_ActivityTracking_Process_ParametersArray", parameters[1].ParameterName);

                CreateActivityRecordTable(activityRecord, activityRecordMetadata, mdmObjectMetadata, out activityRecordTable, out mdmObjectTable);

                parameters[0].Value = activityRecordTable;
                parameters[1].Value = mdmObjectTable;
                parameters[2].Value = activityRecord.ProgramName;

                storedProcedureName = "usp_Workflow_TrackingManager_ActivityTracking_Process";

                output = ExecuteProcedureNonQuery(command.ConnectionString, parameters, storedProcedureName);

                transactionScope.Complete();
            }

            return output;
        }

        /// <summary>
        /// Converts the collection of ActivityTrackingRecord objects into XML
        /// </summary>
        /// <param name="listActivityRecord">Collection of ActivityTrackingRecord objects</param>
        /// <returns>Converted XML</returns>
        private String ConvertToXML(Collection<TrackedActivityInfo> listActivityRecord)
        {
            String xml = "<TrackedActivityInfoCollection>";

            StringBuilder stringBuilder = new StringBuilder(xml);

            foreach (TrackedActivityInfo mapping in listActivityRecord)
            {
                stringBuilder.Append(mapping.ToXml());

            }
            stringBuilder.Append("</TrackedActivityInfoCollection>");

            return stringBuilder.ToString();
        }

        private void CreateActivityRecordTable(TrackedActivityInfo activityRecord, SqlMetaData[] activityRecordMetadata,
                                       SqlMetaData[] mdmObjectMetaData, out List<SqlDataRecord> activityRecordTable,
                                       out List<SqlDataRecord> mdmObjectTable)
        {
            activityRecordTable = new List<SqlDataRecord>();
            mdmObjectTable = new List<SqlDataRecord>();

            SqlDataRecord activityDataRecord = new SqlDataRecord(activityRecordMetadata);

            activityDataRecord.SetValue(0, activityRecord.WorkflowVersionId);
            activityDataRecord.SetValue(1, activityRecord.WorkflowDefinitionActivityID);
            activityDataRecord.SetValue(2, activityRecord.ActivityShortName);
            activityDataRecord.SetValue(3, activityRecord.ActivityLongName);
            activityDataRecord.SetValue(4, activityRecord.RuntimeInstanceId);
            activityDataRecord.SetValue(5, activityRecord.ExtendedProperties);
            activityDataRecord.SetValue(6, activityRecord.Status);
            activityDataRecord.SetValue(7, activityRecord.ActingUserId);
            activityDataRecord.SetValue(8, activityRecord.ActedUserId);
            activityDataRecord.SetValue(9, activityRecord.ActivityComments);
            activityDataRecord.SetValue(10, activityRecord.Variables);
            activityDataRecord.SetValue(11, activityRecord.Arguments);
            activityDataRecord.SetValue(12, activityRecord.CustomData);
            activityDataRecord.SetValue(13, activityRecord.AssignedUsers);
            activityDataRecord.SetValue(14, activityRecord.AssignedRoles);
            activityDataRecord.SetValue(15, (Byte)activityRecord.AssignementType);
            activityDataRecord.SetValue(16, activityRecord.SortOrder);
            activityDataRecord.SetValue(17, activityRecord.IsHumanActivity);
            activityDataRecord.SetValue(18, activityRecord.PerformedAction);
            activityDataRecord.SetValue(19, activityRecord.PreviousActivityShortName);
            activityDataRecord.SetValue(20, activityRecord.LastActivityComments);

            if (activityRecord.MDMObjectCollection != null && activityRecord.MDMObjectCollection.Count > 0)
            {
                CreateMDMObjectTable(activityRecord.MDMObjectCollection, mdmObjectTable, mdmObjectMetaData, activityRecord.WorkflowVersionId, activityRecord.RuntimeInstanceId);
            }

            activityRecordTable.Add(activityDataRecord);

            //we cannot pass no records in the SqlDataRecord enumeration. To send a table-valued parameter with no rows, use a null reference for the value instead.
            if (activityRecordTable.Count == 0)
                activityRecordTable = null;

            if (mdmObjectTable.Count == 0)
                mdmObjectTable = null;
        }

        private void CreateMDMObjectTable(WorkflowMDMObjectCollection workflowMDMObjects, List<SqlDataRecord> mdmObjectTable, SqlMetaData[] mdmObjectMetaData, Int32 workflowVersionId, String runtimeInstanceId)
        {
            foreach (WorkflowMDMObject mdmObject in workflowMDMObjects)
            {
                SqlDataRecord mdmObjectRecord = new SqlDataRecord(mdmObjectMetaData);

                mdmObjectRecord.SetValue(0, workflowVersionId);
                mdmObjectRecord.SetValue(1, runtimeInstanceId);
                //mdmObjectRecord.SetValue(2, mdmObject.MDMObjectId);
                mdmObjectRecord.SetValue(2, mdmObject.MDMObjectGUID);
                mdmObjectRecord.SetValue(3, mdmObject.MDMObjectType);

                mdmObjectTable.Add(mdmObjectRecord);
            }
        }

        #endregion
    }
}
