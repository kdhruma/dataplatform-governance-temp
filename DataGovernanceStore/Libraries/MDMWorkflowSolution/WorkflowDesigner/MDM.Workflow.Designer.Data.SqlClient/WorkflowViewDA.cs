using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

using MDM.Core;
using MDM.Utility;
using MDMBOW = MDM.BusinessObjects.Workflow;

namespace MDM.Workflow.Designer.Data
{
    /// <summary>
    /// Provides database related functions
    /// </summary>
    public class WorkflowViewDA : SqlClientDataAccessBase
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructors

        #endregion

        #region Methods

        /// <summary>
        /// Gets the details required for Execution/Definition view
        /// </summary>
        /// <param name="workflowVersionID"> Id of the workflow version</param>
        /// <param name="instanceGuid">GUID of the Instance</param>
        /// <param name="workflowVersion">Workflow version object. Ref param.</param>
        /// <param name="trackedActivityCollection">Tracked activity collection. Ref param.</param>
        public void GetWorkflowViewDetails(Int32 workflowVersionID, String instanceGuid, ref MDMBOW.WorkflowVersion workflowVersion, ref Collection<MDMBOW.TrackedActivityInfo> trackedActivityCollection)
        {
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            MDMBOW.TrackedActivityInfo trackedActivity = null;
            workflowVersion = new MDMBOW.WorkflowVersion();
            trackedActivityCollection = new Collection<MDMBOW.TrackedActivityInfo>();

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("WorkflowDesigner_SqlParameters");

                parameters = generator.GetParameters("Workflow_Designer_WorkflowView_Get_ParametersArray");

                parameters[0].Value = workflowVersionID;
                parameters[1].Value = instanceGuid;

                connectionString = AppConfigurationHelper.ConnectionString;

                storedProcedureName = "usp_Workflow_Designer_WorkflowView_Get";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    Object[] values = new Object[reader.FieldCount];
                    reader.GetValues(values);
                    workflowVersion = new MDMBOW.WorkflowVersion(values);
                }

                reader.NextResult();

                while (reader.Read())
                {
                    Object[] values = new Object[reader.FieldCount];
                    reader.GetValues(values);
                    trackedActivity = new MDMBOW.TrackedActivityInfo(values);

                    trackedActivityCollection.Add(trackedActivity);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        #endregion
    }
}
