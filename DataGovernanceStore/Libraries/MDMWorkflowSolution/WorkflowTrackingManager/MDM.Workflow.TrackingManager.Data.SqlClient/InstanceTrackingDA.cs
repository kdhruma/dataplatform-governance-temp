using System;
using System.CodeDom;
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
    using MDM.BusinessObjects.Diagnostics;
    using MDM.BusinessObjects.Workflow;

    /// <summary>
    /// Data access logic for Instance Tracking Record
    /// </summary>
    public class InstanceTrackingDA : SqlClientDataAccessBase
    {
        #region Fields
        private TraceSettings _currentTraceSettings = null;
        #endregion

        #region Properties

        #endregion

        #region Constructors

        public InstanceTrackingDA()
        {
            _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Creates the Instance Tracking Record into the database.
        /// </summary>
        /// <param name="instanceRecord">InstanceTrackingRecord</param>
        /// <param name="command">DBCommand containing information about connection string</param>
        /// <returns>The number of records affected by the query</returns>
        public Int32 Create(InstanceTracking instanceRecord, DBCommandProperties command)
        {
            DiagnosticActivity diagnosticActivity = null;

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity = MDMTraceHelper.CreateTraceActivity(MDMTraceSource.AdvancedWorkflow, "InstanceTrackingDA.Create");
                diagnosticActivity.Start();
            }

            Int32 output = 0; //success

            try
            {
                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    SqlParameter[] parameters;
                    String storedProcedureName = String.Empty;

                    SqlParametersGenerator generator = new SqlParametersGenerator("TrackingManager_SqlParameters");

                    parameters = generator.GetParameters("TrackingManager_InstanceTracking_Process_ParametersArray");

                    parameters[0].Value = instanceRecord.RuntimeInstanceId;
                    parameters[1].Value = instanceRecord.Status;
                    parameters[2].Value = instanceRecord.ProgramName;

                    storedProcedureName = "usp_Workflow_TrackingManager_InstanceTracking_Process";

                    output = ExecuteProcedureNonQuery(command.ConnectionString, parameters, storedProcedureName);

                    transactionScope.Complete();
                }

            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    if (diagnosticActivity != null)
                    {
                        diagnosticActivity.Stop();
                    }
                }
            }

            return output;
        }

        /// <summary>
        /// Gets instances for which waiting timer has been elapsed
        /// </summary>
        /// <param name="command">Object having command properties</param>
        /// <returns>Collection of Instance Ids for which waiting timer has been elapsed</returns>
        public Collection<String> GetElapsedTimerInstances(DBCommandProperties command)
        {
            Collection<String> elapsedTimerInstances = null;
            SqlDataReader reader = null;
            String storedProcedureName = String.Empty;

            DiagnosticActivity diagnosticActivity = null;

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity = MDMTraceHelper.CreateTraceActivity(MDMTraceSource.AdvancedWorkflow, "InstanceTrackingDA.GetElapsedTimerInstances");
                diagnosticActivity.Start();
            }

            try
            {
                storedProcedureName = "usp_Workflow_TrackingManager_ElapsedTimerInstances_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, null, storedProcedureName);

                if (reader != null)
                {
                    elapsedTimerInstances = new Collection<String>();

                    while (reader.Read())
                    {
                        if (reader["ID"] != null)
                        {
                            elapsedTimerInstances.Add(reader["ID"].ToString());
                        }
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    if (diagnosticActivity != null)
                    {
                        diagnosticActivity.Stop();
                    }
                }
            }

            return elapsedTimerInstances;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Converts the collection of InstanceTrackingRecord objects into XML
        /// </summary>
        /// <param name="listInstanceRecord">Collection of InstanceTrackingRecord objects</param>
        /// <returns>Converted XML</returns>
        private String ConvertToXML(Collection<InstanceTracking> listInstanceRecord)
        {
            String xml = "<InstanceTrackingRecords>";

            StringBuilder stringBuilder = new StringBuilder(xml);

            foreach (InstanceTracking mapping in listInstanceRecord)
            {
                stringBuilder.Append(mapping.ToXML());

            }
            stringBuilder.Append("</InstanceTrackingRecords>");

            return stringBuilder.ToString();
        }

        #endregion

        #endregion
    }
}
