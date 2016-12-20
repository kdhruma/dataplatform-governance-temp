using System;
using Microsoft.SqlServer.Server;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MDM.EntityManager.Data
{
    using Core;
    using Utility;
    using BusinessObjects;
    using BusinessObjects.Diagnostics;
    using BusinessObjects.EntityIdentification;

    /// <summary>
    /// Specifies data access operations for EntityUniqueIdentification Maps
    /// </summary>
    public class EntityUniqueIdentificationMapDA : SqlClientDataAccessBase
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region Public Methods
        /// <summary>
        /// Loading EntityUniqueIdentification Details
        /// </summary>
        /// <param name="entityIdentifierMapCollection">collection of EntityIdentificationMap</param>
        /// <param name="command">command</param>
        public void LoadEntityUniqueIdentificationDetails(EntityUniqueIdentificationMapCollection entityIdentifierMapCollection, DBCommandProperties command)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            var currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;

            try
            {
                #region Diagnostics & Tracing

                if (currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                #endregion

                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");
                parameters = generator.GetParameters("EntityManager_EntityIdentificationMap_Get_ParametersArray");

                if (currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogInformation("Before preparing EntityIdentificationMap parameter");
                }
                #region Populate table value parameters

                #region Populate EntityMap TVP

                List<SqlDataRecord> entityIdentifierMapList = new List<SqlDataRecord>();
                SqlMetaData[] entityIdentifierMapMetadata = generator.GetTableValueMetadata("EntityManager_EntityIdentificationMap_Get_ParametersArray", parameters[0].ParameterName);

                SqlDataRecord entityMapRecord = null;

                foreach (EntityUniqueIdentificationMap entityIdentifierMap in entityIdentifierMapCollection)
                {
                    entityMapRecord = new SqlDataRecord(entityIdentifierMapMetadata);

                    Int32? contId = (entityIdentifierMap.ContainerId == 0) ? null : (Int32?)entityIdentifierMap.ContainerId;
                    Int64? catId = (entityIdentifierMap.CategoryId == 0) ? null : (Int64?)entityIdentifierMap.CategoryId;
                    Int32? etId = (entityIdentifierMap.EntityTypeId == 0) ? null : (Int32?)entityIdentifierMap.EntityTypeId;

                    String externalID = entityIdentifierMap.ExternalId;

                    String entityIdentifier1Value = entityIdentifierMap.EntityIdentifier1Value;
                    String entityIdentifier2Value = entityIdentifierMap.EntityIdentifier2Value;
                    String entityIdentifier3Value = entityIdentifierMap.EntityIdentifier3Value;
                    String entityIdentifier4Value = entityIdentifierMap.EntityIdentifier4Value;
                    String entityIdentifier5Value = entityIdentifierMap.EntityIdentifier5Value;

                    entityMapRecord.SetValues(entityIdentifierMap.Id, externalID, contId, catId, etId, entityIdentifier1Value, entityIdentifier2Value, entityIdentifier3Value, entityIdentifier4Value, entityIdentifier5Value);
                    entityIdentifierMapList.Add(entityMapRecord);
                }

                #endregion

                #endregion

                parameters[0].Value = entityIdentifierMapList;

                if (currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogInformation("After preparing EntityIdentificationMap parameter and assigning to parameter list");
                }

                storedProcedureName = "usp_EntityManager_EntityIdentificationMap_Get";

                if (currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogInformation("Before calling stored procedure");
                }
                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogInformation("After calling stored procedure which returned reader object");
                }

                if (reader != null)
                {
                    UpdateEntityIdentifierMapCollection(reader, entityIdentifierMapCollection);
                    if (currentTraceSettings.IsBasicTracingEnabled)
                    {
                        diagnosticActivity.LogInformation("After updating EntityIdentificationMapCollection object");
                    }
                }
                else
                {
                    if (currentTraceSettings.IsBasicTracingEnabled)
                    {
                        diagnosticActivity.LogInformation("Returned object reader is null or empty");
                    }
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                if (currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }

            }

        }
        #endregion

        #region Private Methods

        private void UpdateEntityIdentifierMapCollection(SqlDataReader reader, EntityUniqueIdentificationMapCollection entityIdentifierMapCollection)
        {
            Int64 refId = 0;
            Int64 entityIdentifier1Result = 0;
            Int64 entityIdentifier2Result = 0;
            Int64 entityIdentifier3Result = 0;
            Int64 entityIdentifier4Result = 0;
            Int64 entityIdentifier5Result = 0;
            while (reader.Read())
            {
                refId = 0;
                entityIdentifier1Result = 0;
                entityIdentifier2Result = 0;
                entityIdentifier3Result = 0;
                entityIdentifier4Result = 0;
                entityIdentifier5Result = 0;

                if (reader["RefId"] != null)
                {
                    Int64.TryParse(reader["RefId"].ToString(), out refId);
                }

                if (reader["EntityIdentifier1Result"] != null)
                {
                    Int64.TryParse(reader["EntityIdentifier1Result"].ToString(), out entityIdentifier1Result);
                }

                if (reader["EntityIdentifier2Result"] != null)
                {
                    Int64.TryParse(reader["EntityIdentifier2Result"].ToString(), out entityIdentifier2Result);
                }

                if (reader["EntityIdentifier3Result"] != null)
                {
                    Int64.TryParse(reader["EntityIdentifier3Result"].ToString(), out entityIdentifier3Result);
                }

                if (reader["EntityIdentifier4Result"] != null)
                {
                    Int64.TryParse(reader["EntityIdentifier4Result"].ToString(), out entityIdentifier4Result);
                }
                if (reader["EntityIdentifier5Result"] != null)
                {
                    Int64.TryParse(reader["EntityIdentifier5Result"].ToString(), out entityIdentifier5Result);
                }

                EntityUniqueIdentificationMap entityIdentifierMap = (EntityUniqueIdentificationMap)entityIdentifierMapCollection.GetEntityIdentifierMap(refId);
                if (entityIdentifierMap != null)
                {
                    entityIdentifierMap.EntityIdentifier1Result = entityIdentifier1Result;
                    entityIdentifierMap.EntityIdentifier2Result = entityIdentifier2Result;
                    entityIdentifierMap.EntityIdentifier3Result = entityIdentifier3Result;
                    entityIdentifierMap.EntityIdentifier4Result = entityIdentifier4Result;
                    entityIdentifierMap.EntityIdentifier5Result = entityIdentifier5Result;
                }
            }
        }


        #endregion

        #endregion Methods
    }
}