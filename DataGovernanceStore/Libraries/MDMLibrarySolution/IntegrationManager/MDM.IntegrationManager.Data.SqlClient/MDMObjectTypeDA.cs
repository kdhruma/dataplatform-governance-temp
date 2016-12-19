using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using Microsoft.SqlServer.Server;

namespace MDM.IntegrationManager.Data
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Integration;
    using MDM.Core;
    using MDM.Utility;

    /// <summary>
    /// Specifies integration message type type data access
    /// </summary>
    public class MDMObjectTypeDA : SqlClientDataAccessBase
    {
        #region Fields

        private const String _processMethodName = "MDM.IntegrationManager.Data.MDMObjectTypeDA.Process";
        private const String _getMethodName = "MDM.IntegrationManager.Data.MDMObjectTypeDA.Get";

        #endregion Fields

        // Uncomment this code for Process API

        /*
        /// <summary>
        /// Process integration message type
        /// </summary>
        /// <param name="mdmObjectTypes">MDMObjectTypes to process</param>
        /// <param name="programName">Name of program making the change</param>
        /// <param name="userName">Name of user making the change</param>
        /// <param name="command">Connection related properties</param>
        /// <returns>Operation result of processing of integration message type</returns>
        public OperationResultCollection Process(MDMObjectTypeCollection mdmObjectTypes, String programName, String userName, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_processMethodName, MDMTraceSource.Integration, false);
            }

            SqlDataReader reader = null;
            OperationResultCollection result = new OperationResultCollection();
            const String storedProcedureName = "usp_IntegrationManager_MDMObjectType_Process";

            SqlParametersGenerator generator = new SqlParametersGenerator("IntegrationManager_SqlParameters");
            SqlParameter[] parameters = generator.GetParameters("IntegrationManager_MDMObjectType_Process_ParametersArray");
            SqlMetaData[] sqlMetadata = generator.GetTableValueMetadata("IntegrationManager_MDMObjectType_Process_ParametersArray", parameters[0].ParameterName);

            List<SqlDataRecord> mdmObjectTypeRecords = GetSqlRecords(mdmObjectTypes, sqlMetadata);

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                reader = null;
                try
                {

                    parameters[0].Value = mdmObjectTypeRecords;
                    parameters[1].Value = programName;
                    parameters[2].Value = userName;

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    //Update integration message type object with Actual Id in case of create
                    UpdateMDMObjectTypes(reader, mdmObjectTypes, result);

                    transactionScope.Complete();
                }
                finally
                {
                    if (reader != null)
                        reader.Close();

                    if (Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.StartTraceActivity(_processMethodName, MDMTraceSource.Integration, false);
                    }
                }

            }

            return result;
        }
        */

        /// <summary>
        /// Get MDMObjectTypes
        /// </summary>
        /// <param name="command">Connection related properties</param>
        /// <returns>Qualifying items</returns>
        public MDMObjectTypeCollection Get(Int16 mdmObjectTypeId, String mdmObjectTypeName, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_getMethodName, MDMTraceSource.Integration, false);
            }

            SqlDataReader reader = null;
            SqlParameter[] parameters;
            MDMObjectTypeCollection mdmObjectTypes = new MDMObjectTypeCollection();
            String storedProcedureName = "usp_IntegrationManager_MDMObjectType_Get";

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("IntegrationManager_SqlParameters");

                parameters = generator.GetParameters("IntegrationManager_MDMObjectType_Get_ParametersArray");

                parameters[0].Value = mdmObjectTypeId;
                parameters[1].Value = mdmObjectTypeName;

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                mdmObjectTypes = ReadMDMObjectTypes(reader);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity(_getMethodName, MDMTraceSource.Integration);
                }
            }
            return mdmObjectTypes;
        }

        #region Private method

        // Uncomment this code for Process API

        /*
        private void UpdateMDMObjectTypes(SqlDataReader reader, MDMObjectTypeCollection mdmObjectTypes, OperationResultCollection result)
        {
            while (reader.Read())
            {
                MDMObjectType message = null;
                Int64 id = -1;
                Boolean hasError = false;
                String errorMessage = String.Empty;
                String referenceId = String.Empty;

                #region Read values

                if (reader["Id"] != null)
                {
                    id = ValueTypeHelper.Int64TryParse(reader["Id"].ToString(), -1);
                }

                //if (reader["HasError"] != null)
                //{
                //    hasError = ValueTypeHelper.BooleanTryParse(reader["HasError"].ToString(), false);
                //}

                if (reader["Error"] != null)
                {
                    errorMessage = reader["Error"].ToString();
                }

                if (reader["ReferenceId"] != null)
                {
                    referenceId = reader["ReferenceId"].ToString();
                }

                #endregion Read values

                if (!String.IsNullOrWhiteSpace(referenceId))
                {
                    message = mdmObjectTypes.Where(log => log.ReferenceId == referenceId).FirstOrDefault();
                    message.Id = id;
                }

                OperationResult or = new OperationResult();
                or.ReferenceId = referenceId;
                //or.Id = id; //TODO :: need to change OR.Id to Int64

                if (!String.IsNullOrWhiteSpace(errorMessage) && errorMessage.Trim().Length > 0)
                {
                    or.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);
                }

                result.Add(or);
            }
        }

        private List<SqlDataRecord> GetSqlRecords(MDMObjectTypeCollection mdmObjectTypeCollection, SqlMetaData[] sqlMetadata)
        {
            List<SqlDataRecord> integrationActivityLogSqlRecords = new List<SqlDataRecord>();
            Int32 counter = 0;

            foreach (MDMObjectType mdmObjectType in mdmObjectTypeCollection)
            {
                SqlDataRecord mdmObjectTypeRecord = new SqlDataRecord(sqlMetadata);
                if (String.IsNullOrWhiteSpace(mdmObjectType.ReferenceId))
                {
                    mdmObjectType.ReferenceId = ( counter-- ).ToString();
                }

                mdmObjectTypeRecord.SetValue(0, mdmObjectType.ReferenceId);
                mdmObjectTypeRecord.SetValue(1, mdmObjectType.Id);
                mdmObjectTypeRecord.SetValue(2, mdmObjectType.MDMObjectTypeId);
                mdmObjectTypeRecord.SetValue(3, mdmObjectType.ToXml());
                mdmObjectTypeRecord.SetValue(4, mdmObjectType.Action.ToString());

                integrationActivityLogSqlRecords.Add(mdmObjectTypeRecord);
            }

            return integrationActivityLogSqlRecords;
        }

        */
        private MDMObjectTypeCollection ReadMDMObjectTypes(SqlDataReader reader)
        {
            MDMObjectTypeCollection objectTypeCollection = new MDMObjectTypeCollection();

            if (reader != null)
            {
                while (reader.Read())
                {
                    #region Declaration

                    Int16 id = -1;
                    String name = String.Empty;
                    String className = String.Empty;
                    String assemblyName = String.Empty;

                    #endregion Declaration

                    #region Read properties

                    if (reader["Id"] != null)
                        id = ValueTypeHelper.Int16TryParse(reader["Id"].ToString(), -1);

                    if (reader["Name"] != null)
                        name = reader["Name"].ToString();

                    if (reader["ClassName"] != null)
                        className = reader["ClassName"].ToString();

                    if (reader["AssemblyName"] != null)
                        assemblyName = reader["AssemblyName"].ToString();

                    #endregion Read properties

                    #region Initialize object

                    MDMObjectType objecType = new MDMObjectType(assemblyName);
                    objecType.Id = id;
                    objecType.Name = name;
                    objecType.ClassName = className;
                    objecType.AssemblyName = assemblyName;


                    #endregion Initialize object

                    objectTypeCollection.Add(objecType);
                }
            }
            return objectTypeCollection;
        }

        #endregion Private method

    }
}
