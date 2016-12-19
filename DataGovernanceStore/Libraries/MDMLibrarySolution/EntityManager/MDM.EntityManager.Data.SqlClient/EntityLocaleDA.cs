using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Transactions;
using System.Collections.ObjectModel;
using Microsoft.SqlServer.Server;
using System.IO;
using System.Xml;

namespace MDM.EntityManager.Data
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.DataModel;

    /// <summary>
    /// 
    /// </summary>
    public class EntityLocaleDA : SqlClientDataAccessBase
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
        ///   Process given list of entities based on their actions and data locales
        /// </summary>
        /// <param name="categoryLocaleProperties">Collection of category locale properties.</param>
        /// <param name="loginUser">Indicates the logged in User</param>
        /// <param name="programName">Indicates the Program Name</param>
        /// <param name="systemDataLocale">Indicates the system data locale</param>
        /// <param name="returnResult">Whether need to return result or not </param>
        /// <param name="command">Indicates the DBCommandProperties</param>
        /// <returns>Returns EntityOperationResultCollection</returns>
        public EntityOperationResultCollection Process(CategoryLocalePropertiesCollection categoryLocaleProperties, String loginUser, String programName, String systemDataLocale, Boolean returnResult, DBCommandProperties command)
        {
            EntityOperationResultCollection entityOperationResult = new EntityOperationResultCollection();

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                SqlDataReader reader = null;
                SqlParameter[] parameters;
                String storedProcedureName = String.Empty;

                try
                {
                    SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                    parameters = generator.GetParameters("EntityManager_EntityLocale_Process_ParametersArray");

                    #region Populate table value parameters for Entity Locale

                    SqlMetaData[] sqlCategoryLocalePropertiesMetadata = generator.GetTableValueMetadata("EntityManager_EntityLocale_Process_ParametersArray", parameters[0].ParameterName);
                    List<SqlDataRecord> categoryLocalePropertiesList = CreateCategoryLocalePropertiesTable(categoryLocaleProperties, sqlCategoryLocalePropertiesMetadata);

                    #endregion Populate table value parameters for Locale

                    parameters[0].Value = categoryLocalePropertiesList;
                    parameters[1].Value = loginUser;
                    parameters[2].Value = programName;
                    parameters[3].Value = ValueTypeHelper.Int32TryParse(systemDataLocale, 0);
                    parameters[4].Value = returnResult;

                    storedProcedureName = "usp_EntityManager_EntityLocale_Process";

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    if (reader != null)
                    {
                        UpdateEntityLocaleOperationResults(reader, entityOperationResult);
                    }
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }

                transactionScope.Complete();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("EntityManager.EntityLocaleDA.Process");
            }

            return entityOperationResult;
        }

        /// <summary>
        /// Process given list of categories based on their actions and data locales
        /// </summary>
        /// <param name="categoryLocalePropertiesCollection">Collection of category locale properties.</param>
        /// <param name="operationResults">OperationResult Collection</param>
        /// <param name="loginUser">Indicates the logged in User</param>
        /// <param name="programName">Indicates the Program Name</param>
        /// <param name="systemDataLocale">Indicates the system data locale</param>
        /// <param name="returnResult">Whether need to return result or not </param>
        /// <param name="command">Indicates the DBCommandProperties</param>
        public void Process(CategoryLocalePropertiesCollection categoryLocalePropertiesCollection, DataModelOperationResultCollection operationResults, String loginUser, String programName, String systemDataLocale, Boolean returnResult, DBCommandProperties command)
        {
            SqlDataReader reader = null;
            SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");
            const String storedProcedureName = "usp_EntityManager_EntityLocale_Process";

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                foreach (CategoryLocaleProperties categoryLocaleProperties in categoryLocalePropertiesCollection)
                {
                    if (categoryLocaleProperties.Action == ObjectAction.Read || categoryLocaleProperties.Action == ObjectAction.Ignore)
                        continue;

                    OperationResult categoryLocalePropetiesOperationResult = (OperationResult)operationResults.GetByReferenceId(categoryLocaleProperties.ReferenceId);
                    SqlParameter[] parameters = generator.GetParameters("EntityManager_EntityLocale_Process_ParametersArray");

                    #region Populate table value parameters for Entity Locale

                    SqlMetaData[] sqlCategoryLocalePropertiesMetadata = generator.GetTableValueMetadata("EntityManager_EntityLocale_Process_ParametersArray", parameters[0].ParameterName);
                    List<SqlDataRecord> categoryLocalePropertiesList = CreateCategoryLocalePropertiesTable(new CategoryLocalePropertiesCollection() { categoryLocaleProperties }, sqlCategoryLocalePropertiesMetadata);

                    #endregion Populate table value parameters for Locale

                    #region Execute SQL Procedure

                    try
                    {
                        parameters[0].Value = categoryLocalePropertiesList;
                        parameters[1].Value = loginUser;
                        parameters[2].Value = programName;
                        parameters[3].Value = ValueTypeHelper.Int32TryParse(systemDataLocale, 0);
                        parameters[4].Value = returnResult;

                        reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                        if (reader != null)
                        {
                            PopulateOperationResult(reader, categoryLocaleProperties, categoryLocalePropetiesOperationResult);
                        }
                    }
                    finally
                    {
                        if (reader != null)
                            reader.Close();
                    }

                    #endregion

                } // for all category locale properties

                transactionScope.Complete();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("EntityManager.EntityLocaleDA.Process");
            }
        }

        /// <summary>
        /// Get Entity with different long Name based on the data locales 
        /// </summary>
        /// <param name="entityId">Indicates the Entity Id</param>
        /// <param name="datalocales">Indicates the data locales</param>
        /// <param name="systemDataLocaleId">Indicates the present system data locale id</param>
        /// <param name="command">Indicates the DBCommandProperties</param>
        /// <returns>Returns EntityCollection</returns>
        public EntityCollection Get(Int64 entityId, Collection<Locale> datalocales, Int32 systemDataLocaleId, DBCommandProperties command)
        {
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            EntityCollection entities = new EntityCollection();

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                parameters = generator.GetParameters("EntityManager_EntityLocale_Get_ParametersArray");

                #region Populate table value parameters for Locale

                SqlMetaData[] sqlLocalesMetadata = generator.GetTableValueMetadata("EntityManager_EntityLocale_Get_ParametersArray", parameters[1].ParameterName);
                List<SqlDataRecord> localeList = EntityDataReaderUtility.CreateLocaleTable(datalocales, systemDataLocaleId, sqlLocalesMetadata);

                #endregion Populate table value parameters for Locale

                parameters[0].Value = entityId;
                parameters[1].Value = localeList;

                storedProcedureName = "usp_EntityManager_EntityLocale_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                entities = PopulateEntityLocaleData(reader);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return entities;
        }

        /// <summary>
        /// Get category locale properties with different long Name based on the data locales 
        /// </summary>
        /// <param name="categoryId">Indicates the category Id</param>
        /// <param name="datalocales">Indicates the data locales</param>
        /// <param name="systemDataLocaleId">Indicates the present system data locale id</param>
        /// <param name="command">Indicates the DBCommandProperties</param>
        /// <returns>Returns CategoryLocalePropertiesCollection</returns>
        public CategoryLocalePropertiesCollection GetCategoryLocaleProperties(Int64 categoryId, Collection<LocaleEnum> datalocales, Int32 systemDataLocaleId, DBCommandProperties command)
        {
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            CategoryLocalePropertiesCollection categoryLocalePropertiesCollection = null;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                parameters = generator.GetParameters("EntityManager_EntityLocale_Get_ParametersArray");

                #region Populate table value parameters for Locale

                SqlMetaData[] sqlLocalesMetadata = generator.GetTableValueMetadata("EntityManager_EntityLocale_Get_ParametersArray", parameters[1].ParameterName);
                List<SqlDataRecord> localeList = EntityDataReaderUtility.CreateLocaleTable(datalocales, systemDataLocaleId, sqlLocalesMetadata);

                #endregion Populate table value parameters for Locale

                parameters[0].Value = categoryId;
                parameters[1].Value = localeList;

                storedProcedureName = "usp_EntityManager_EntityLocale_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                categoryLocalePropertiesCollection = PopulateCategoryLocalePropertiesCollection(reader);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return categoryLocalePropertiesCollection;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryLocaleProperties"></param>
        /// <param name="sqlLocalesMetadata"></param>
        /// <returns></returns>
        private List<SqlDataRecord> CreateCategoryLocalePropertiesTable(CategoryLocalePropertiesCollection categoryLocaleProperties, SqlMetaData[] sqlLocalesMetadata)
        {
            List<SqlDataRecord> categoryLocalePropertiesList = null;

            if (categoryLocaleProperties != null && categoryLocaleProperties.Count > 0)
            {
                categoryLocalePropertiesList = new List<SqlDataRecord>();

                foreach (CategoryLocaleProperties categoryLocaleProperty in categoryLocaleProperties)
                {
                    SqlDataRecord categoryLocalePropertiesRecord = new SqlDataRecord(sqlLocalesMetadata);

                    categoryLocalePropertiesRecord.SetValue(0, categoryLocaleProperty.Id);
                    categoryLocalePropertiesRecord.SetValue(1, categoryLocaleProperty.CategoryId);
                    categoryLocalePropertiesRecord.SetValue(2, categoryLocaleProperty.Name);
                    categoryLocalePropertiesRecord.SetValue(3, categoryLocaleProperty.LongName);
                    categoryLocalePropertiesRecord.SetValue(4, (Int32)categoryLocaleProperty.Locale);
                    categoryLocalePropertiesRecord.SetValue(5, categoryLocaleProperty.Action.ToString());

                    categoryLocalePropertiesList.Add(categoryLocalePropertiesRecord);
                }
            }

            return categoryLocalePropertiesList;
        }

        /// <summary>
        /// Updating the entity operation result collection
        /// </summary>
        /// <param name="reader">Indicates the SqlDataReader</param>
        /// <param name="entityOperationResults">Indicates the EntityOperationResultCollection</param>
        private void UpdateEntityLocaleOperationResults(SqlDataReader reader, EntityOperationResultCollection entityOperationResults)
        {
            while (reader.Read())
            {
                Int64 id = 0;
                Int64 entityId = 0;
                Boolean IsError = false;
                String errorMessage = String.Empty;
                String errorMessageCode = String.Empty;

                if (reader["Id"] != null)
                    Int64.TryParse(reader["Id"].ToString(), out id);
                if (reader["EntityId"] != null)
                    Int64.TryParse(reader["EntityId"].ToString(), out entityId);
                if (reader["IsError"] != null)
                    Boolean.TryParse(reader["IsError"].ToString(), out IsError);
                if (reader["ErrorCode"] != null)
                    errorMessageCode = reader["ErrorCode"].ToString();

                if (IsError)
                {
                    //Add error
                    entityOperationResults.AddEntityOperationResult(id, errorMessageCode, String.Empty, OperationResultType.Error);
                }
                else
                {
                    //No errors.. update status as Successful.
                    entityOperationResults.OperationResultStatus = OperationResultStatusEnum.Successful;
                }

            }
        }

        /// <summary>
        /// Updating the operation result
        /// </summary>
        /// <param name="reader">Indicates the SqlDataReader</param>
        /// <param name="categoryLocaleProperties">Indicates the category locale properties</param>
        /// <param name="operationResult">Indicates the operationResult</param>
        private void PopulateOperationResult(SqlDataReader reader, CategoryLocaleProperties categoryLocaleProperties, OperationResult operationResult)
        {
            while (reader.Read())
            {
                Int32 id = 0;
                Int64 categoryId = 0;
                Boolean hasError = false;
                String errorMessage = String.Empty;
                String errorCode = String.Empty;

                if (reader["Id"] != null)
                    Int32.TryParse(reader["Id"].ToString(), out id);
                if (reader["EntityId"] != null)
                    Int64.TryParse(reader["EntityId"].ToString(), out categoryId);
                if (reader["IsError"] != null)
                    Boolean.TryParse(reader["IsError"].ToString(), out hasError);
                if (reader["ErrorCode"] != null)
                    errorCode = reader["ErrorCode"].ToString();

                if (hasError && !String.IsNullOrEmpty(errorCode))
                {
                    operationResult.AddOperationResult(errorCode, String.Empty, OperationResultType.Error);
                }
                else
                {
                    operationResult.AddOperationResult("", "Category Locale properties  Id: " + id, OperationResultType.Information);
                    categoryLocaleProperties.Id = id;
                    operationResult.ReturnValues.Add(operationResult);

                    //No errors.. update status as Successful.
                    operationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                }
            }
        }

        /// <summary>
        ///  Population of entity Locale Details
        /// </summary>
        /// <param name="reader">Indicates the SqlDataReader</param>
        /// <returns>Returns EntityCollection</returns>
        private EntityCollection PopulateEntityLocaleData(SqlDataReader reader)
        {
            EntityCollection entities = new EntityCollection();

            if (reader != null)
            {
                while (reader.Read())
                {
                    Entity entity = new Entity();

                    String id = String.Empty;
                    Int64 entityId = -1;
                    String shortName = String.Empty;
                    String longName = String.Empty;
                    LocaleEnum locale = LocaleEnum.UnKnown;

                    if (reader["Id"] != null)
                        id = reader["Id"].ToString();
                    if (reader["EntityId"] != null)
                        Int64.TryParse(reader["EntityId"].ToString(), out entityId);
                    if (reader["ShortName"] != null)
                        shortName = reader["ShortName"].ToString();
                    if (reader["LongName"] != null)
                        longName = reader["LongName"].ToString();
                    if (reader["Locale"] != null)
                        Enum.TryParse(reader["Locale"].ToString(), out locale);

                    entity.Id = entityId;
                    entity.ExternalId = id;
                    entity.Locale = locale;
                    entity.Name = shortName;
                    entity.LongName = longName;
                    entities.Add(entity);
                }
            }

            return entities;
        }

        /// <summary>
        ///  Population of category locale properties details
        /// </summary>
        /// <param name="reader">Indicates the SqlDataReader</param>
        /// <returns>Returns category locale properties collection</returns>
        private CategoryLocalePropertiesCollection PopulateCategoryLocalePropertiesCollection(SqlDataReader reader)
        {
            CategoryLocalePropertiesCollection categoryLocalePropertiesCollection = null;

            if (reader != null)
            {
                while (reader.Read())
                {
                    CategoryLocaleProperties categoryLocaleProperties = new CategoryLocaleProperties();

                    Int32 id = 0;
                    Int64 categoryId = 0;
                    String shortName = String.Empty;
                    String longName = String.Empty;
                    LocaleEnum locale = LocaleEnum.UnKnown;

                    if (reader["Id"] != null)
                    {
                        id = ValueTypeHelper.Int32TryParse(reader["Id"].ToString(), id);
                    }

                    if (reader["EntityId"] != null)
                    {
                        categoryId = ValueTypeHelper.Int64TryParse(reader["EntityId"].ToString(), categoryId);
                    }

                    if (reader["ShortName"] != null)
                    {
                        shortName = reader["ShortName"].ToString();
                    }

                    if (reader["LongName"] != null)
                    {
                        longName = reader["LongName"].ToString();
                    }

                    if (reader["Locale"] != null)
                    {
                        Enum.TryParse(reader["Locale"].ToString(), out locale);
                    }

                    if (id > 0)
                    {
                        if (categoryLocalePropertiesCollection == null)
                            categoryLocalePropertiesCollection = new CategoryLocalePropertiesCollection();

                        categoryLocaleProperties.Id = id;
                        categoryLocaleProperties.CategoryId = categoryId;
                        categoryLocaleProperties.Locale = locale;
                        categoryLocaleProperties.Name = shortName;
                        categoryLocaleProperties.LongName = longName;

                        categoryLocalePropertiesCollection.Add(categoryLocaleProperties);
                    }
                }
            }

            return categoryLocalePropertiesCollection;
        }

        #endregion

        #endregion
    }
}