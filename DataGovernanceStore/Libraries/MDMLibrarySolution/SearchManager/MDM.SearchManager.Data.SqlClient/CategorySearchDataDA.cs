using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;

namespace MDM.SearchManager.Data
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Utility;

    /// <summary>
    /// Specifies the data access operations for search data
    /// </summary>
    public class CategorySearchDataDA : SqlClientDataAccessBase
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
        /// Get CategorySearchData for given entities.
        /// </summary>
        /// <param name="entities">Provides Collection of entities for which searchData needs to be found.</param>
        /// <param name="command">Indicates Sql Command</param>
        /// <returns>EntitySearchData Collection</returns>
        public EntitySearchDataCollection Get(EntityCollection entities, DBCommandProperties command)
        {
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;
            EntitySearchDataCollection entitySearchDataCollection = new EntitySearchDataCollection();
            String parameterConfigName = "SearchManager_CategorySearchData_Get_ParametersArray";

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("SearchManager_SqlParameters");

                parameters = generator.GetParameters(parameterConfigName);

                #region Populate table value parameters

                List<SqlDataRecord> entityList = new List<SqlDataRecord>();
                SqlMetaData[] entityMetadata = generator.GetTableValueMetadata(parameterConfigName, parameters[0].ParameterName);

                SqlDataRecord entityRecord = null;

                foreach (Entity entity in entities)
                {
                    entityRecord = new SqlDataRecord(entityMetadata);
                    entityRecord.SetValues(entity.Id);
                    entityList.Add(entityRecord);
                }

                #endregion

                parameters[0].Value = entityList;

                parameters[1].Value = entities.FirstOrDefault().ContainerId;
                storedProcedureName = "usp_SearchManager_CategorySearchData_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        EntitySearchData entitySearchData = new EntitySearchData();

                        if (reader["PK_DN_CategorySearch"] != null)
                            entitySearchData.Id = ValueTypeHelper.Int64TryParse(reader["PK_DN_CategorySearch"].ToString(), 0);

                        if (reader["FK_CNode"] != null)
                            entitySearchData.EntityId = ValueTypeHelper.Int64TryParse(reader["FK_CNode"].ToString(), 0);

                        if (reader["FK_Catalog"] != null)
                            entitySearchData.ContainerId = ValueTypeHelper.Int32TryParse(reader["FK_Catalog"].ToString(), 0);

                        if (reader["SearchVal"] != null)
                            entitySearchData.SearchValue = reader["SearchVal"].ToString();

                        if (reader["KeyVal"] != null)
                            entitySearchData.KeyValue = reader["KeyVal"].ToString();

                        //Add to collection
                        entitySearchDataCollection.Add(entitySearchData);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return entitySearchDataCollection;
        }

        /// <summary>
        /// Process Entity Search Data Collection.
        /// </summary>
        /// <param name="entitySearchDataCollection">Indicates entity search data to be proccessed.</param>
        /// <param name="entityOperationResults">Indicates entityOperationResult.</param>
        /// <param name="command">Indicates Sql Command</param>
        /// <param name="processingMode">Indicates mode of processing (Sync or Async)</param>
        /// <param name="isCategoryData">Indicates whether it is a category data or not.</param>
        /// <returns>Returns true if entity search data have been processed successfully.</returns>
        public Boolean Process(EntitySearchDataCollection entitySearchDataCollection, EntityOperationResultCollection entityOperationResults, DBCommandProperties command, ProcessingMode processingMode = ProcessingMode.Sync, Boolean isCategoryData = false)
        {
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;
            Boolean result = false;
            String parameterConfigName = "SearchManager_CategorySearchData_Process_ParametersArray";

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, BusinessLogicBase.GetTransactionOptions(processingMode)))
            {
                try
                {
                    SqlParametersGenerator generator = new SqlParametersGenerator("SearchManager_SqlParameters");

                    parameters = generator.GetParameters(parameterConfigName);

                    #region Populate table value parameters

                    List<SqlDataRecord> entitySearchDataList = new List<SqlDataRecord>();
                    SqlMetaData[] entitySearchMetadata = generator.GetTableValueMetadata(parameterConfigName, parameters[0].ParameterName);

                    SqlDataRecord entitySearchDataRecord = null;

                    foreach (EntitySearchData entitySearchData in entitySearchDataCollection)
                    {
                        entitySearchDataRecord = new SqlDataRecord(entitySearchMetadata);
                        entitySearchDataRecord.SetValue(0, entitySearchData.EntityId);
                        entitySearchDataRecord.SetValue(1, entitySearchData.ContainerId);
                        entitySearchDataRecord.SetValue(2, entitySearchData.SearchValue);
                        entitySearchDataRecord.SetValue(3, entitySearchData.KeyValue);
                        entitySearchDataRecord.SetValue(4, entitySearchData.Action.ToString());
                        entitySearchDataList.Add(entitySearchDataRecord);
                    }

                    #endregion

                    parameters[0].Value = entitySearchDataList;

                    storedProcedureName = "usp_SearchManager_CategorySearchData_Process";


                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            if (reader["EntityId"] != null)
                            {
                                if (reader["MessageCode"] != null && reader["IsError"] != null)
                                {
                                    if (reader["IsError"].ToString() == "1")
                                    {
                                        result = false;
                                        entityOperationResults.AddEntityOperationResult(ValueTypeHelper.Int64TryParse(reader["EntityId"].ToString(), 0), reader["MessageCode"].ToString(), "", OperationResultType.Error);
                                    }
                                    else
                                    {
                                        result = true;
                                        entityOperationResults.AddEntityOperationResult(ValueTypeHelper.Int64TryParse(reader["EntityId"].ToString(), 0), reader["MessageCode"].ToString(), "", OperationResultType.Information);
                                    }
                                }
                            }
                        }
                    }
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }

                transactionScope.Complete();
            }

            return result;
        }

        #endregion 

        #region Private Methods

        #endregion 

        #endregion
    }
}
