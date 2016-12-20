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

    /// <summary>
    /// Data access logic for Instance Tracking Record
    /// </summary>
    public class FaultTrackingDA : SqlClientDataAccessBase
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructors

        #endregion

        #region Methods

        /// <summary>
        /// Creates the Fault Tracking Records into the database.
        /// </summary>
        /// <param name="listFaultRecord">Collection of FaultTrackingRecord</param>
        /// <param name="command">DBCommand containing information about connection string</param>
        /// <returns>The number of records affected by the query</returns>
        public Int32 Create(Collection<FaultTracking> listFaultRecord, DBCommandProperties command)
        {
            Int32 output = 0; //success

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                SqlParameter[] parameters;
                String connectionString = String.Empty;
                String storedProcedureName = String.Empty;

                String paramXML = ConvertToXML(listFaultRecord);

                SqlParametersGenerator generator = new SqlParametersGenerator("TrackingManager_SqlParameters");

                parameters = generator.GetParameters("TrackingManager_FaultTracking_Process_ParametersArray");

                parameters[0].Value = paramXML;

                storedProcedureName = "usp_Workflow_TrackingManager_FaultTracking_Process";

                output = ExecuteProcedureNonQuery(command.ConnectionString, parameters, storedProcedureName);

                transactionScope.Complete();
            }

            return output;
        }

        /// <summary>
        /// Converts the collection of FaultTrackingRecord objects into XML
        /// </summary>
        /// <param name="listFaultRecord">Collection of FaultTrackingRecord objects</param>
        /// <returns>Converted XML</returns>
        private String ConvertToXML(Collection<FaultTracking> listFaultRecord)
        {
            String xml = "<FaultTrackingRecords>";

            StringBuilder stringBuilder = new StringBuilder(xml);

            foreach (FaultTracking mapping in listFaultRecord)
            {
                stringBuilder.Append(mapping.ToXML());

            }
            stringBuilder.Append("</FaultTrackingRecords>");

            return stringBuilder.ToString();
        }

        #endregion
    }
}
