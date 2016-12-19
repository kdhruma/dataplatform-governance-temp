using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Transactions;
using Microsoft.SqlServer.Server;

namespace MDM.HierarchyManager.Data
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects.DataModel;

    /// <summary>
    /// Specifies data access operations for Hierarchy.
    /// </summary>
    public class HierarchyDA : SqlClientDataAccessBase
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Get all taxonomies
        /// </summary>
        /// <returns></returns>
        public HierarchyCollection Get(DBCommandProperties command)
        {
            HierarchyCollection hierarchyCollection = new HierarchyCollection();
            SqlDataReader reader = null;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("ContainerManager_SqlParameters");

                SqlParameter[] parameters = generator.GetParameters("ContainerManager_Container_Get_ParametersArray");

                parameters[0].Value = 0;
                parameters[1].Value = 0;
                parameters[2].Value = 1;

                const String storedProcedureName = "usp_ContainerManager_Container_Get ";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    ReadHierarchyProperties(ref hierarchyCollection, reader);
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return hierarchyCollection;
        }

        /// <summary>
        /// Processes hierarchy in accordance to operation
        /// </summary>
        /// <param name="hierarchy">Collection of taxonomies</param>
        /// <param name="programName">Name of the program</param>
        /// <param name="userName">Name of the User</param>
        /// <param name="systemDataLocale">systems data locale</param>
        /// <param name="command">The command</param>
        /// <returns>The result of the operation </returns>
        public OperationResult Process(Hierarchy hierarchy, String programName, String userName, LocaleEnum systemDataLocale, DBCommandProperties command)
        {
            MDMTraceHelper.StartTraceActivity("HierarchyDA.Process", false);

            SqlDataReader reader = null;
            OperationResult hierarchyProcessOperationResult = new OperationResult();

            SqlParametersGenerator generator = new SqlParametersGenerator("ContainerManager_SqlParameters");

            const String storedProcedureName = "usp_ContainerManager_Container_Process";

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                try
                {
                    List<SqlDataRecord> containerTable;
                    List<SqlDataRecord> attributeTable;
                    SqlParameter[] parameters = generator.GetParameters("ContainerManager_Container_Process_ParametersArray");
                    SqlMetaData[] containerMetaData = generator.GetTableValueMetadata("ContainerManager_Container_Process_ParametersArray", parameters[0].ParameterName);
                    SqlMetaData[] attributeMetaData = generator.GetTableValueMetadata("ContainerManager_Container_Process_ParametersArray", parameters[1].ParameterName);

                    CreateTableParams(hierarchy, containerMetaData, attributeMetaData, out containerTable, out attributeTable);

                    parameters[0].Value = containerTable;
                    parameters[1].Value = attributeTable;
                    parameters[2].Value = null;
                    parameters[3].Value = userName;
                    parameters[4].Value = (Int32)systemDataLocale;
                    parameters[5].Value = programName;

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    //Need empty information to make sure correct operation result status is calculated.
                    hierarchyProcessOperationResult.AddOperationResult(String.Empty, String.Empty, OperationResultType.Information);

                    PopulateOperationResult(reader, hierarchy, hierarchyProcessOperationResult);
                    transactionScope.Complete();

                }
                catch (Exception exception)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "hierarchy Process Failed." + exception.Message);
                    hierarchyProcessOperationResult.AddOperationResult(exception.Message, String.Empty, OperationResultType.Error);
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                    MDMTraceHelper.StopTraceActivity("HierarchyDA.Process");
                }
            }


            return hierarchyProcessOperationResult;
        }

        /// <summary>
        /// Processes hierarchy in accordance to operation
        /// </summary>
        /// <param name="hierarchies">Collection of taxonomies</param>
        /// <param name="operationResults">OperationResult Collection</param>
        /// <param name="programName">Name of the program</param>
        /// <param name="userName">Name of the User</param>
        /// <param name="systemDataLocale">systems data locale</param>
        /// <param name="command">The command</param>
        /// <returns>The result of the operation </returns>
        public void Process(HierarchyCollection hierarchies, DataModelOperationResultCollection operationResults, String programName, String userName, LocaleEnum systemDataLocale, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("HierarchyDA.Process", MDMTraceSource.DataModel, false);
            }

            SqlDataReader reader = null;
            SqlParametersGenerator generator = new SqlParametersGenerator("ContainerManager_SqlParameters");
            const String storedProcedureName = "usp_ContainerManager_Container_Process";

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                foreach (Hierarchy hierarchy in hierarchies)
                {
                    if (hierarchy.Action == ObjectAction.Read || hierarchy.Action == ObjectAction.Ignore)
                    {
                        continue;
                    }

                    OperationResult hierarchyOperationResult = (OperationResult)operationResults.GetByReferenceId(hierarchy.ReferenceId);

                    #region Execute SQL Procedure

                    try
                    {
                        List<SqlDataRecord> containerTable;
                        List<SqlDataRecord> attributeTable;
                        SqlParameter[] parameters = generator.GetParameters("ContainerManager_Container_Process_ParametersArray");
                        SqlMetaData[] containerMetaData = generator.GetTableValueMetadata("ContainerManager_Container_Process_ParametersArray", parameters[0].ParameterName);
                        SqlMetaData[] attributeMetaData = generator.GetTableValueMetadata("ContainerManager_Container_Process_ParametersArray", parameters[1].ParameterName);

                        CreateTableParams(hierarchy, containerMetaData, attributeMetaData, out containerTable, out attributeTable);

                        parameters[0].Value = containerTable;
                        parameters[1].Value = attributeTable;
                        parameters[2].Value = null;
                        parameters[3].Value = userName;
                        parameters[4].Value = (Int32)systemDataLocale;
                        parameters[5].Value = programName;

                        reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                        PopulateOperationResult(reader, hierarchy, hierarchyOperationResult);

                        operationResults.RefreshOperationResultStatus();

                    }
                    catch (Exception exception)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "Hierarchy process failed : " + exception.Message);
                        operationResults.AddOperationResult(String.Empty, exception.Message, OperationResultType.Error);
                    }
                    finally
                    {
                        if (reader != null)
                        {
                            reader.Close();
                        }

                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.StopTraceActivity("HierarchyDA.Process", MDMTraceSource.DataModel);
                        }
                    }

                    #endregion

                }// for all hierarchies

                transactionScope.Complete();
            }
        }

        #region Private Methods

        /// <summary>
        /// Reads Hierarchy Metadata
        /// </summary>
        /// <param name="hierarchyCollection"></param>
        /// <param name="reader">SQL reader</param>
        /// <returns></returns>
        private void ReadHierarchyProperties(ref HierarchyCollection hierarchyCollection, SqlDataReader reader)
        {
            Hierarchy hierarchy = null;

            while (reader.Read())
            {
                Int32 id = 0;
                String name = String.Empty;
                String longName = String.Empty;
                LocaleEnum locale = LocaleEnum.UnKnown;
                Int32 securityObjectTypeId = 0;
                Boolean leafNodeOnly = false;

                if (reader["Id"] != null)
                {
                    id = ValueTypeHelper.Int32TryParse(reader["Id"].ToString(), 0);
                }

                if (reader["Name"] != null)
                {
                    name = reader["Name"].ToString();
                }

                if (reader["LongName"] != null)
                {
                    longName = reader["LongName"].ToString();
                }

                if (reader["Locale"] != null)
                {
                    Enum.TryParse(reader["Locale"].ToString(), out locale);
                }
                
                if (reader["SecurityObjectTypeId"] != null)
                {
                    securityObjectTypeId = ValueTypeHelper.Int32TryParse(reader["SecurityObjectTypeId"].ToString(), 0);
                }

                if (reader["LeafNodeOnly"] != null)
                {
                    leafNodeOnly = ValueTypeHelper.BooleanTryParse(reader["LeafNodeOnly"].ToString(), false);
                }

                hierarchy = new Hierarchy
                {
                    Id = id,
                    Name = name,
                    LongName = longName,
                    LeafNodeOnly = leafNodeOnly,
                    Locale = locale,
                    SecurityObjectTypeId = securityObjectTypeId
                };

                hierarchyCollection.Add(hierarchy);
            }
        }

        /// <summary>
        /// Create Container TVP for process
        /// </summary>
        /// <param name="hierarchy">Hierarchy</param>
        /// <param name="containerMetaData">Metadata of Container TVP</param>
        /// <param name="attributeMetaData">Metadata of Container Attribute TVP</param>
        /// <param name="containerTable">Container SQL Data Record</param>
        /// <param name="attributeTable">Attribute SQL Data Record</param>
        private void CreateTableParams(Hierarchy hierarchy, SqlMetaData[] containerMetaData,
                                         SqlMetaData[] attributeMetaData, out List<SqlDataRecord> containerTable,
                                        out List<SqlDataRecord> attributeTable)
        {
            containerTable = new List<SqlDataRecord>();
            attributeTable = new List<SqlDataRecord>();

            SqlDataRecord containerRecord = new SqlDataRecord(containerMetaData);
            containerRecord.SetValue(0, ValueTypeHelper.Int32TryParse(hierarchy.ReferenceId,0));
            containerRecord.SetValue(1, hierarchy.Id);
            containerRecord.SetValue(2, null);
            containerRecord.SetValue(3, hierarchy.Name);
            containerRecord.SetValue(4, hierarchy.LongName);
            containerRecord.SetValue(5, String.Empty);
            containerRecord.SetValue(6, null); // should be null for hierarchy
            containerRecord.SetValue(7, ObjectType.Taxonomy);
            containerRecord.SetValue(8, false);
            containerRecord.SetValue(9, hierarchy.ProcessorWeightage);
            containerRecord.SetValue(10, hierarchy.Locale);
            containerRecord.SetValue(11, hierarchy.Action.ToString());
            containerRecord.SetValue(12, null);
            containerRecord.SetValue(13, null);
            containerRecord.SetValue(14, "None");
            containerRecord.SetValue(15, null);
            containerRecord.SetValue(16, null);
            containerRecord.SetValue(17, hierarchy.LeafNodeOnly);

            containerTable.Add(containerRecord);

            SqlDataRecord attributeTableRecord = new SqlDataRecord(attributeMetaData);

            AttributeCollection hierarchyAttributes = hierarchy.Attributes;

            for (Int32 i = 0; i < hierarchyAttributes.Count; i++)
            {
                attributeTableRecord.SetValue(i, hierarchyAttributes[i, hierarchy.Locale]);
            }

            attributeTable.Add(attributeTableRecord);

            //we cannot pass no records in the SqlDataRecord enumeration. To send a table-valued parameter with no rows, use a null reference for the value instead.
            if (containerTable.Count == 0)
            {
                containerTable = null;
            }

            if (attributeTable.Count == 0)
            {
                attributeTable = null;
            }
        }

        private void PopulateOperationResult(SqlDataReader reader, Hierarchy hierarchy, OperationResult taxonomyProcessOperationResult)
        {
            while (reader.Read())
            {
                Boolean hasError = false;
                String errorCode = String.Empty;
                String hierarchyId = String.Empty;

                if (reader["ContainerId"] != null)
                {
                    hierarchyId = reader["ContainerId"].ToString();
                }

                if (reader["HasError"] != null)
                {
                    hasError = ValueTypeHelper.BooleanTryParse(reader["HasError"].ToString(), false);
                }

                if (reader["ErrorCode"] != null)
                {
                    errorCode = reader["ErrorCode"].ToString();
                }

                if (hasError & !String.IsNullOrEmpty(errorCode))
                {
                    taxonomyProcessOperationResult.AddOperationResult(errorCode, String.Empty, OperationResultType.Error);
                }
                else
                {
                    //taxonomyProcessOperationResult.AddOperationResult("", "Hierarchy ID: " + hierarchyId, OperationResultType.Information);
                    hierarchy.Id = ValueTypeHelper.Int32TryParse(hierarchyId, -1);
                    taxonomyProcessOperationResult.ReturnValues.Add(hierarchyId);
                    taxonomyProcessOperationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                }
            }
        }

        #endregion

        #endregion Methods
    }
}