using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Xml;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Microsoft.SqlServer.Server;
using System.Transactions;
using System.Linq;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace MDM.AttributeModelManager.Data
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.DataModel;
    using MDM.BusinessObjects.Diagnostics;

    /// <summary>
    /// Specifies the data access operations for attribute model
    /// </summary>
    public class AttributeModelDA : SqlClientDataAccessBase
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
        /// Gets the ids of attributes coming under requested attribute groups, custom views or state views 
        /// </summary>
        /// <param name="attributeGroupIdsList"> Comma separated list of attribute group ids</param>
        /// <param name="attributeIdList"> Comma separated list of attribute ids</param>
        /// <param name="customViewId">Custom view id</param>
        /// <param name="stateViewId">State view id</param>
        /// <param name="containerId">Container id context parameter</param>
        /// <param name="entityTypeId">Entity type id context parameter</param>
        /// <param name="categoryId">Category id context parameter</param>
        /// <param name="locale">Locale context parameter</param>
        /// <param name="attributeModelType">Type of the attribute model</param>
        /// <returns>Comma separated list of attribute ids</returns>
        public AttributeModelMappingPropertiesCollection GetMappedAttributesForContext(String attributeGroupIdsList, Int32 customViewId, Int32 stateViewId, Int32 containerId, Int32 entityTypeId, Int32 relationshipTypeId, Int64 categoryId, Collection<LocaleEnum> locales, Int64 entityId, Boolean showAtCreationAttributesOnly, Boolean requiredAttributesOnly, Boolean ignoreMapping, AttributeModelType attributeModelType)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("AttributeModelDA.GetMappedAttributesForContext", MDMTraceSource.AttributeModelGet, false);

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;

            AttributeModelMappingPropertiesCollection attributeModelMappingProperties = new AttributeModelMappingPropertiesCollection();

            try
            {
                connectionString = AppConfigurationHelper.ConnectionString;

                SqlParametersGenerator generator = new SqlParametersGenerator("AttributeModelManager_SqlParameters");

                parameters = generator.GetParameters("AttributeModelManager_AttributeModel_AttributeIds_Get_ParametersArray");

                parameters[0].Value = attributeGroupIdsList;
                parameters[1].Value = customViewId;
                parameters[2].Value = stateViewId;
                parameters[3].Value = showAtCreationAttributesOnly;
                parameters[4].Value = requiredAttributesOnly;
                parameters[5].Value = entityId;
                parameters[6].Value = containerId;
                parameters[7].Value = entityTypeId;
                parameters[8].Value = relationshipTypeId;
                parameters[9].Value = categoryId;
                //TODO :: AttributeModelContext.Locales :: Create Table and send multiple locale to Get SP
                parameters[10].Value = locales.FirstOrDefault();
                parameters[11].Value = ignoreMapping;
                parameters[12].Value = attributeModelType.ToString();

                storedProcedureName = "usp_AttributeModelManager_AttributeModel_AttributeIds_Get";

                if (!String.IsNullOrWhiteSpace(AppConfigurationHelper.StoredProcedureSuffix))
                    storedProcedureName = String.Format("{0}_{1}", storedProcedureName, AppConfigurationHelper.StoredProcedureSuffix);

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        Int32 attributeId = 0;
                        AttributeModelType attrModelType = AttributeModelType.Unknown;
                        Int32 attributeParentId = 0;

                        if (reader["AttributeId"] != null)
                            attributeId = ValueTypeHelper.Int32TryParse(reader["AttributeId"].ToString(), attributeId);

                        if (reader["AttributeTypeName"] != null)
                            Enum.TryParse(reader["AttributeTypeName"].ToString(), out attrModelType);

                        if (reader["AttributeParentId"] != null)
                            attributeParentId = ValueTypeHelper.Int32TryParse(reader["AttributeParentId"].ToString(), attributeParentId);

                        AttributeModelMappingProperties attrModelMappingProperties = new AttributeModelMappingProperties(attributeId, attributeParentId, null, null, null, null, null, null, attrModelType, null);

                        attributeModelMappingProperties.Add(attrModelMappingProperties);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("AttributeModelDA.GetMappedAttributesForContext", MDMTraceSource.AttributeModelGet);
            }

            return attributeModelMappingProperties;
        }

        /// <summary>
        /// Get all base attribute models
        /// </summary>
        /// <param name="attributeUniqueIdentifierToAttributeIdMaps">Indicates dictionary of unique attributes ids based on short name</param>
        /// <param name="attributeNameToAttributeIdMaps">Indicates dictionary of unique attributes ids based on short name</param>
        /// <returns>Returns dictionary of attribute model base properties.</returns>
        public Dictionary<Int32, AttributeModelBaseProperties> GetAllBaseAttributeModels(out Dictionary<String, Int32> attributeUniqueIdentifierToAttributeIdMaps, out Dictionary<String, Int32> attributeNameToAttributeIdMaps)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("AttributeModelDA.GetAllBaseAttributeModels", MDMTraceSource.AttributeModelGet, false);

            String connectionString;
            String storedProcedureName;
            SqlDataReader reader = null;

            Dictionary<Int32, AttributeModelBaseProperties> attributeIdToBaseAttributeModelPropertiesDictionary;

            try
            {
                connectionString = AppConfigurationHelper.ConnectionString;

                storedProcedureName = "usp_AttributeModelManager_AttributeModel_Get";

                reader = ExecuteProcedureReader(connectionString, null, storedProcedureName);

                attributeIdToBaseAttributeModelPropertiesDictionary = PopulateAttributeModelCollection(reader, out attributeUniqueIdentifierToAttributeIdMaps, out attributeNameToAttributeIdMaps);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("AttributeModelDA.GetAllBaseAttributeModels", MDMTraceSource.AttributeModelGet);
            }

            return attributeIdToBaseAttributeModelPropertiesDictionary;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locale"></param>
        /// <returns></returns>
        public Dictionary<String, AttributeModelLocaleProperties> GetAttributeModelLocaleProperties(LocaleEnum locale)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("AttributeModelDA.GetAttributeModelLocaleProperties", MDMTraceSource.AttributeModelGet, false);

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;

            Dictionary<String, AttributeModelLocaleProperties> attributeModelLocalePropertiesCollection = new Dictionary<String, AttributeModelLocaleProperties>();

            try
            {
                connectionString = AppConfigurationHelper.ConnectionString;

                SqlParametersGenerator generator = new SqlParametersGenerator("AttributeModelManager_SqlParameters");

                parameters = generator.GetParameters("AttributeModelManager_AttributeModelLocale_Get_ParametersArray");

                storedProcedureName = "usp_AttributeModelManager_AttributeModelLocale_Get";

                #region Populate table value parameters and also populate return result collection

                List<SqlDataRecord> localeList = new List<SqlDataRecord>();

                SqlMetaData[] sqllocales = generator.GetTableValueMetadata("AttributeModelManager_AttributeModelLocale_Get_ParametersArray", parameters[0].ParameterName);

                SqlDataRecord localeRecord = new SqlDataRecord(sqllocales);
                localeRecord.SetValue(0, (Int32)locale);
                localeList.Add(localeRecord);

                #endregion

                parameters[0].Value = localeList;

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                attributeModelLocalePropertiesCollection = PopulateAttributeModelLocaleProperties(reader);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("AttributeModelDA.GetAttributeModelLocaleProperties", MDMTraceSource.AttributeModelGet);
            }

            return attributeModelLocalePropertiesCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeIdList"></param>
        /// <returns></returns>
        public Collection<KeyValuePair<Int32, Collection<AttributeModelContext>>> GetWhereUsed(Collection<Int32> attributeIdList)
        {
            SqlDataReader reader = null;
            String connectionString = String.Empty;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;

            Collection<KeyValuePair<Int32, Collection<AttributeModelContext>>> result = new Collection<KeyValuePair<int, Collection<AttributeModelContext>>>();

            try
            {
                connectionString = AppConfigurationHelper.ConnectionString;

                SqlParametersGenerator generator = new SqlParametersGenerator("AttributeModelManager_SqlParameters");

                parameters = generator.GetParameters("AttributeModelManager_AttributeModel_WhereUsed_Get_ParametersArray");

                #region Populate table value parameters

                List<SqlDataRecord> attributeList = null;
                SqlMetaData[] attributeMetadata = generator.GetTableValueMetadata("AttributeModelManager_AttributeModel_WhereUsed_Get_ParametersArray", parameters[0].ParameterName);
                SqlDataRecord attributeRecord = null;

                if (attributeIdList != null && attributeIdList.Count > 0)
                {
                    attributeList = new List<SqlDataRecord>();

                foreach (Int32 attributeId in attributeIdList)
                {
                    attributeRecord = new SqlDataRecord(attributeMetadata);
                    attributeRecord.SetValues(attributeId);
                    attributeList.Add(attributeRecord);
                }
                }

                #endregion

                parameters[0].Value = attributeList;

                storedProcedureName = "usp_AttributeModelManager_AttributeModel_WhereUsed_Get";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        AttributeModel attributeModel = new AttributeModel();
                        Int32 attributeId = 0;
                        AttributeModelType attributeModelType = AttributeModelType.All;
                        Int32 containerId = 0;

                        Int32 entityTypeId = 0;
                        Int32 categoryId = 0;
                        Int32 relationshipTypeId = 0;

                        if (reader["AttributeId"] != null)
                            Int32.TryParse(reader["AttributeId"].ToString(), out attributeId);

                        if (reader["AttributeType"] != null)
                            Enum.TryParse<AttributeModelType>(reader["AttributeType"].ToString(), out attributeModelType);

                        if (reader["ContainerId"] != null)
                            Int32.TryParse(reader["ContainerId"].ToString(), out containerId);

                        if (reader["EntityTypeId"] != null)
                            Int32.TryParse(reader["EntityTypeId"].ToString(), out entityTypeId);

                        if (reader["CategoryId"] != null)
                            Int32.TryParse(reader["CategoryId"].ToString(), out categoryId);

                        if (reader["RelationshipTypeId"] != null)
                            Int32.TryParse(reader["RelationshipTypeId"].ToString(), out relationshipTypeId);

                        AttributeModelContext attributeModelContext = new AttributeModelContext(containerId, entityTypeId, relationshipTypeId, categoryId, new Collection<LocaleEnum>() { LocaleEnum.UnKnown }, 0, attributeModelType, false, false, false);

                        bool attrFound = false;
                        foreach (KeyValuePair<Int32, Collection<AttributeModelContext>> pair in result)
                        {
                            if (pair.Key == attributeId)
                            {
                                pair.Value.Add(attributeModelContext);
                                attrFound = true;
                                break;
                            }
                        }

                        if (!attrFound)
                        {
                            KeyValuePair<Int32, Collection<AttributeModelContext>> pair = new KeyValuePair<int, Collection<AttributeModelContext>>(attributeId, new Collection<AttributeModelContext>());
                            pair.Value.Add(attributeModelContext);
                            result.Add(pair);
                        }
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return result;
        }

        /// <summary>
        /// Gets mapped attributes based on given container ids
        /// </summary>
        /// <param name="containerIdList">Indicates container ids based on that needs to get mapped attributes</param>
        /// <returns></returns>
        public Collection<Int32> GetMappedAttributesIdsForContainers(Collection<Int32> containerIdList)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            var currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;

            Collection<Int32> attributeIdList = new Collection<Int32>();

            try
            {
                if (currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                connectionString = AppConfigurationHelper.ConnectionString;

                SqlParametersGenerator generator = new SqlParametersGenerator("AttributeModelManager_SqlParameters");

                String parameterName = "AttributeModelManager_AttributeModel_GetMappedAttributeIdsForContainers_ParametersArray";
                parameters = generator.GetParameters(parameterName);
                SqlMetaData[] containersMetaData = generator.GetTableValueMetadata(parameterName, parameters[0].ParameterName);

                storedProcedureName = "usp_DataModelManager_MappedAttribute_Get";

                #region Populate table value parameters and also populate return result collection

                //Create list of Container Ids
                List<SqlDataRecord> containerIdTable = null;
                if (containerIdList != null)
                {
                    containerIdTable = new List<SqlDataRecord>();
                    containerIdTable = CreateContainerIdTable(containerIdList, containersMetaData);

                    if (currentTraceSettings.IsBasicTracingEnabled)
                    {
                        diagnosticActivity.LogInformation("Completed preparing TVP from Container Ids");
                        DiagnosticsUtility.LogSqlTVPInformation(diagnosticActivity, storedProcedureName, "ContainerIdTable", containersMetaData, containerIdTable);
                    }
                }

                #endregion

                parameters[0].Value = containerIdTable;

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        Int32 attributeId = 0;
                        Int32 attributeParentId = 0;

                        if (reader["AttributeId"] != null)
                        {
                            attributeId = ValueTypeHelper.Int32TryParse(reader["AttributeId"].ToString(), attributeId);
                        }

                        if (reader["AttributeParentId"] != null)
                        {
                            attributeParentId = ValueTypeHelper.Int32TryParse(reader["AttributeParentId"].ToString(), attributeParentId);
                        }

                        attributeIdList.Add(attributeId);

                        if (!attributeIdList.Contains(attributeParentId))
                        {
                            attributeIdList.Add(attributeParentId);
                        }
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

            return attributeIdList;
        }


        /// <summary>
        /// Process AttributeModels (Add/ Edit/ Delete Atribute)
        /// </summary>
        /// <param name="attributeModelCollection">Collection of attributes</param>
        /// <param name="systemDataLocaleId">System Data Locale Id</param>
        /// <param name="userName">logged in user name</param>
        /// <param name="attributeOperationResultCollection">collection of attribute operation results</param>
        /// <param name="programName">indicates which module is processing attributes</param>
        /// <param name="command">Indicates sql command</param>
        public void ProcessAttributeModels(AttributeModelCollection attributeModelCollection, Int32 systemDataLocaleId, String userName, AttributeOperationResultCollection attributeOperationResultCollection, String programName, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("AttributeModelDA.ProcessAttributeModels", MDMTraceSource.AttributeModelGet, false);

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
            {
                SqlDataReader reader = null;
                SqlParameter[] parameters;
                String storedProcedureName = String.Empty;

                try
                {
                    SqlParametersGenerator generator = new SqlParametersGenerator("AttributeModelManager_SqlParameters");

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Generate parameters for Attribute Process starting...");

                    parameters = generator.GetParameters("AttributeModelManager_AttributeModel_Process_ParametersArray");

                    if (Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Generate parameters for Attribute Process completed...");
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Generate table valued parameters for Attribute Process starting...");
                    }

                    #region Populate table value parameters and also populate return result collection

                    List<SqlDataRecord> attributeModelList = new List<SqlDataRecord>();

                    SqlMetaData[] attributeModelMetadata = generator.GetTableValueMetadata("AttributeModelManager_AttributeModel_Process_ParametersArray", parameters[0].ParameterName);

                    foreach (AttributeModel attributeModel in attributeModelCollection)
                    {
                        if (attributeModel.IsComplex == true)
                        {
                            if (Constants.TRACING_ENABLED)
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Add sql data record for parent complex attribute starting...");

                            SqlDataRecord attributeModelRecord = new SqlDataRecord(attributeModelMetadata);
                            SqlDataRecord complexAttributeModelRecord = AttributeModelUtility.FillAttributeModelSqlDataRecord(attributeModelRecord, attributeModel);

                            attributeModelList.Add(complexAttributeModelRecord);

                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Add sql data record for parent complex attribute completing...");
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Get complex child attributes starting...");
                            }
                            AttributeModelCollection ComplexChildren = (AttributeModelCollection)attributeModel.GetChildAttributeModels();// check with return

                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Get complex child attributes completing...");
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Attribute is complex hence generate sql data record for all child attributes starting...");
                            }
                            foreach (AttributeModel complexChild in ComplexChildren)
                            {
                                SqlDataRecord childComplexattributeModelRecord = new SqlDataRecord(attributeModelMetadata);
                                childComplexattributeModelRecord = AttributeModelUtility.FillAttributeModelSqlDataRecord(childComplexattributeModelRecord, complexChild);
                                attributeModelList.Add(childComplexattributeModelRecord);
                            }

                            if (Constants.TRACING_ENABLED)
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Attribute is complex hence generate sql data record for all child attributes completed...");
                        }
                        else
                        {
                            if (Constants.TRACING_ENABLED)
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Add sql data record for simple attribute starting...");

                            SqlDataRecord attributeModelRecord = new SqlDataRecord(attributeModelMetadata);
                            attributeModelRecord = AttributeModelUtility.FillAttributeModelSqlDataRecord(attributeModelRecord, attributeModel);
                            attributeModelList.Add(attributeModelRecord);

                            if (Constants.TRACING_ENABLED)
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Add sql data record for simple attribute completed...");
                        }
                    }

                    #endregion

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Generate table valued parameters for Attribute Process completed...");

                    parameters[0].Value = attributeModelList;
                    parameters[1].Value = systemDataLocaleId;
                    parameters[2].Value = userName;
                    parameters[3].Value = programName;

                    storedProcedureName = "usp_AttributeModelManager_AttributeModel_Process";

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Database process starting...");

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Database process completed...");

                    #region Populate Attribute Operation Result Collection

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Update AttributeOperation Results starting...");

                    UpdateAttributeOperationResults(reader, attributeModelCollection, attributeOperationResultCollection);

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Update AttributeOperation Results completed...");

                    #endregion Populate Attribute Operation Result Collection
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.StopTraceActivity("AttributeModelDA.ProcessAttributeModels", MDMTraceSource.AttributeModelGet);
                }

                //return false;
                transactionScope.Complete();
            }
        }

        /// <summary>
        /// Process AttributeModels (Add/ Edit/ Delete Atribute)
        /// </summary>
        /// <param name="attributeModels">Collection of attribute models</param>
        /// <param name="systemDataLocaleId">System Data Locale Id</param>
        /// <param name="userName">logged in user name</param>
        /// <param name="operationResults">collection of operation results</param>
        /// <param name="programName">indicates which module is processing attributes</param>
        /// <param name="command">Indicates sql command</param>
        public void Process(AttributeModelCollection attributeModels, Int32 systemDataLocaleId, String userName, DataModelOperationResultCollection operationResults, String programName, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("AttributeModelDA.ProcessAttributeModels", MDMTraceSource.AttributeModelGet, false);
            }

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
            {
                SqlDataReader reader = null;
                SqlParameter[] parameters;
                String storedProcedureName = String.Empty;

                try
                {
                    SqlParametersGenerator generator = new SqlParametersGenerator("AttributeModelManager_SqlParameters");

                    if (Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Generate parameters for Attribute Process starting...");
                    }

                    parameters = generator.GetParameters("AttributeModelManager_AttributeModel_Process_ParametersArray");

                    if (Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Generate parameters for Attribute Process completed...");
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Generate table valued parameters for Attribute Process starting...");
                    }

                    #region Populate table value parameters and also populate return result collection

                    List<SqlDataRecord> attributeModelList = new List<SqlDataRecord>();

                    SqlMetaData[] attributeModelMetadata = generator.GetTableValueMetadata("AttributeModelManager_AttributeModel_Process_ParametersArray", parameters[0].ParameterName);

                    foreach (AttributeModel attributeModel in attributeModels)
                    {
                        if (attributeModel.Action == ObjectAction.Read || attributeModel.Action == ObjectAction.Ignore)
                            continue;

                        if (attributeModel.IsComplex == true)
                        {
                            if (Constants.TRACING_ENABLED)
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Add sql data record for parent complex attribute starting...");

                            SqlDataRecord attributeModelRecord = new SqlDataRecord(attributeModelMetadata);
                            SqlDataRecord complexAttributeModelRecord = AttributeModelUtility.FillAttributeModelSqlDataRecord(attributeModelRecord, attributeModel);

                            attributeModelList.Add(complexAttributeModelRecord);

                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Add sql data record for parent complex attribute completing...");
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Get complex child attributes starting...");
                            }
                            AttributeModelCollection complexChildren = (AttributeModelCollection)attributeModel.GetChildAttributeModels();// check with return

                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Get complex child attributes completing...");
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Attribute is complex hence generate sql data record for all child attributes starting...");
                            }
                            foreach (AttributeModel complexChild in complexChildren)
                            {
                                SqlDataRecord childComplexattributeModelRecord = new SqlDataRecord(attributeModelMetadata);
                                childComplexattributeModelRecord = AttributeModelUtility.FillAttributeModelSqlDataRecord(childComplexattributeModelRecord, complexChild);
                                attributeModelList.Add(childComplexattributeModelRecord);
                            }

                            if (Constants.TRACING_ENABLED)
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Attribute is complex hence generate sql data record for all child attributes completed...");
                        }
                        else
                        {
                            if (Constants.TRACING_ENABLED)
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Add sql data record for simple attribute starting...");

                            SqlDataRecord attributeModelRecord = new SqlDataRecord(attributeModelMetadata);
                            attributeModelRecord = AttributeModelUtility.FillAttributeModelSqlDataRecord(attributeModelRecord, attributeModel);
                            attributeModelList.Add(attributeModelRecord);

                            if (Constants.TRACING_ENABLED)
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Add sql data record for simple attribute completed...");
                        }
                    }

                    #endregion

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Generate table valued parameters for Attribute Process completed...");

                    if (attributeModelList.Count == 0)
                        attributeModelList = null;

                    parameters[0].Value = attributeModelList;
                    parameters[1].Value = systemDataLocaleId;
                    parameters[2].Value = userName;
                    parameters[3].Value = programName;

                    storedProcedureName = "Usp_AttributeModelManager_AttributeModel_Process";

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Database process starting...");

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Database process completed...");

                    #region Populate Attribute Operation Result Collection

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Update AttributeOperation Results starting...");

                    PopulateOperationResult(reader, attributeModels, operationResults);

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Update AttributeOperation Results completed...");

                    #endregion Populate Attribute Operation Result Collection
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.StopTraceActivity("AttributeModelDA.ProcessAttributeModels", MDMTraceSource.AttributeModelGet);
                }

                transactionScope.Complete();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="attributeUniqueIdentifierToAttributeIdMaps"></param>
        /// <param name="attributeNameToAttributeIdMaps"></param>
        /// <param name="duplicateAttributeNameToAttributeIdsMaps"></param>
        /// <returns></returns>
        private Dictionary<Int32, AttributeModelBaseProperties> PopulateAttributeModelCollection(SqlDataReader reader, out Dictionary<String, Int32> attributeUniqueIdentifierToAttributeIdMaps, out Dictionary<String, Int32> attributeNameToAttributeIdMaps)
        {
            var attributeIdToBaseAttributeModelPropertiesDictionary = new Dictionary<Int32, AttributeModelBaseProperties>();

            attributeUniqueIdentifierToAttributeIdMaps = new Dictionary<String, Int32>();
            attributeNameToAttributeIdMaps = new Dictionary<String, Int32>();

            LocaleEnum systemDataLocale = GlobalizationHelper.GetSystemDataLocale();

            if (reader != null)
            {
                while (reader.Read())
                {
                    var attributeModelBaseProperties = new AttributeModelBaseProperties();

                    #region Reading Properties

                    if (reader["Id"] != null)
                    {
                        Int32 attributeId = 0;
                        Int32.TryParse(reader["Id"].ToString(), out attributeId);
                        attributeModelBaseProperties.Id = attributeId;
                    }

                    if (reader["Name"] != null)
                    {
                        attributeModelBaseProperties.Name = reader["Name"].ToString();
                    }

                    if (reader["LongName"] != null)
                    {
                        attributeModelBaseProperties.LongName = reader["LongName"].ToString();
                    }

                    if (reader["AttributeParentId"] != null)
                    {
                        Int32 attributeParentId = 0;
                        Int32.TryParse(reader["AttributeParentId"].ToString(), out attributeParentId);
                        attributeModelBaseProperties.AttributeParentId = attributeParentId;
                    }

                    if (reader["AttributeParentName"] != null)
                    {
                        attributeModelBaseProperties.AttributeParentName = reader["AttributeParentName"].ToString();
                    }

                    if (reader["AttributeParentLongName"] != null)
                    {
                        attributeModelBaseProperties.AttributeParentLongName = reader["AttributeParentLongName"].ToString();
                    }

                    if (reader["AttributeTypeId"] != null)
                    {
                        Int32 attributeTypeId = 0;
                        Int32.TryParse(reader["AttributeTypeId"].ToString(), out attributeTypeId);
                        attributeModelBaseProperties.AttributeTypeId = attributeTypeId;
                    }

                    if (reader["AttributeTypeName"] != null)
                    {
                        attributeModelBaseProperties.AttributeTypeName = reader["AttributeTypeName"].ToString();
                    }

                    if (reader["AttributeDataTypeId"] != null)
                    {
                        Int32 attributeDataTypeId = 0;
                        Int32.TryParse(reader["AttributeDataTypeId"].ToString(), out attributeDataTypeId);
                        attributeModelBaseProperties.AttributeDataTypeId = attributeDataTypeId;
                    }

                    if (reader["AttributeDataTypeName"] != null)
                        attributeModelBaseProperties.AttributeDataTypeName = reader["AttributeDataTypeName"].ToString();

                    if (reader["AttributeDisplayTypeId"] != null)
                    {
                        Int32 attributeDisplayTypeId = 0;
                        Int32.TryParse(reader["AttributeDisplayTypeId"].ToString(), out attributeDisplayTypeId);
                        attributeModelBaseProperties.AttributeDisplayTypeId = attributeDisplayTypeId;
                    }

                    if (reader["AttributeDisplayTypeName"] != null)
                    {
                        attributeModelBaseProperties.AttributeDisplayTypeName = reader["AttributeDisplayTypeName"].ToString();
                    }

                    if (reader["IsCollection"] != null)
                    {
                        attributeModelBaseProperties.IsCollection = ValueTypeHelper.ConvertToBoolean(reader["IsCollection"].ToString());
                    }

                    if (reader["IsLocalizable"] != null)
                    {
                        attributeModelBaseProperties.IsLocalizable = ValueTypeHelper.ConvertToBoolean(reader["IsLocalizable"].ToString());
                    }

                    if (reader["ApplyLocaleFormat"] != null)
                    {
                        attributeModelBaseProperties.ApplyLocaleFormat = ValueTypeHelper.ConvertToBoolean(reader["ApplyLocaleFormat"].ToString());
                    }

                    if (reader["ApplyTimeZoneConversion"] != null)
                    {
                        attributeModelBaseProperties.ApplyTimeZoneConversion = ValueTypeHelper.ConvertToBoolean(reader["ApplyTimeZoneConversion"].ToString());
                    }

                    if (reader["AllowNullSearch"] != null)
                    {
                        attributeModelBaseProperties.AllowNullSearch = ValueTypeHelper.ConvertToBoolean(reader["AllowNullSearch"].ToString());
                    }

                    if (reader["AllowVal"] != null)
                    {
                        attributeModelBaseProperties.AllowableValues = reader["AllowVal"].ToString();
                    }

                    Int32 maxLength = 0;
                    if (reader["MaxLength"] != null)
                    {
                        Int32.TryParse(reader["MaxLength"].ToString(), out maxLength);
                        attributeModelBaseProperties.MaxLength = maxLength;
                    }

                    Int32 minLength = 0;
                    if (reader["MinLength"] != null)
                    {
                        Int32.TryParse(reader["MinLength"].ToString(), out minLength);
                        attributeModelBaseProperties.MinLength = minLength;
                    }

                    if (reader["Required"] != null)
                    {
                        Int32 required = ValueTypeHelper.Int32TryParse(reader["Required"].ToString(), 0);
                        attributeModelBaseProperties.Required = ValueTypeHelper.ConvertToBooleanFromInteger(required);
                    }

                    if (reader["AllowableUOM"] != null)
                    {
                        attributeModelBaseProperties.AllowableUOM = reader["AllowableUOM"].ToString();
                    }

                    if (reader["DefaultUOM"] != null)
                    {
                        attributeModelBaseProperties.DefaultUOM = reader["DefaultUOM"].ToString();
                    }

                    if (reader["UOMType"] != null)
                    {
                        attributeModelBaseProperties.UomType = reader["UOMType"].ToString();
                    }

                    Int32 precision = 0;
                    if (reader["Precision"] != null)
                    {
                        Int32.TryParse(reader["Precision"].ToString(), out precision);
                        attributeModelBaseProperties.Precision = precision;
                    }

                    if (reader["MinInclusive"] != null)
                    {
                        attributeModelBaseProperties.MinInclusive = reader["MinInclusive"].ToString();
                    }

                    if (reader["MinExclusive"] != null)
                    {
                        attributeModelBaseProperties.MinExclusive = reader["MinExclusive"].ToString();
                    }

                    if (reader["MaxInclusive"] != null)
                    {
                        attributeModelBaseProperties.MaxInclusive = reader["MaxInclusive"].ToString();
                    }

                    if (reader["MaxExclusive"] != null)
                    {
                        attributeModelBaseProperties.MaxExclusive = reader["MaxExclusive"].ToString();
                    }

                    if (!String.IsNullOrEmpty(attributeModelBaseProperties.MinInclusive))
                    {
                        attributeModelBaseProperties.RangeFrom = attributeModelBaseProperties.MinInclusive;
                    }
                    if (!String.IsNullOrEmpty(attributeModelBaseProperties.MinExclusive))
                    {
                        attributeModelBaseProperties.RangeFrom = attributeModelBaseProperties.MinExclusive;
                    }

                    if (!String.IsNullOrEmpty(attributeModelBaseProperties.MaxInclusive))
                    {
                        attributeModelBaseProperties.RangeTo = attributeModelBaseProperties.MaxInclusive;
                    }
                    if (!String.IsNullOrEmpty(attributeModelBaseProperties.MaxExclusive))
                    {
                        attributeModelBaseProperties.RangeTo = attributeModelBaseProperties.MaxExclusive;
                    }

                    if (reader["Label"] != null)
                    {
                        attributeModelBaseProperties.Label = reader["Label"].ToString();
                    }

                    if (reader["Definition"] != null)
                    {
                        attributeModelBaseProperties.Definition = reader["Definition"].ToString();
                    }

                    if (reader["Example"] != null)
                    {
                        attributeModelBaseProperties.AttributeExample = reader["Example"].ToString();
                    }

                    if (reader["BusinessRule"] != null)
                    {
                        attributeModelBaseProperties.BusinessRule = reader["BusinessRule"].ToString();
                    }

                    if (reader["ReadOnly"] != null)
                    {
                        Boolean isReadOnly = false;
                        Boolean.TryParse(reader["ReadOnly"].ToString(), out isReadOnly);
                        attributeModelBaseProperties.ReadOnly = isReadOnly;
                    }

                    if (reader["Extension"] != null)
                    {
                        attributeModelBaseProperties.Extension = reader["Extension"].ToString();
                    }

                    if (reader["AttributeRegex"] != null)
                    {
                        attributeModelBaseProperties.AttributeRegEx = reader["AttributeRegEx"].ToString();
                    }

                    if (reader["AttributeRegExp_ErrorMessage"] != null)
                    {
                        attributeModelBaseProperties.RegExErrorMessage = reader["AttributeRegExp_ErrorMessage"].ToString();
                    }

                    if (reader["LookUpTableName"] != null)
                    {
                        attributeModelBaseProperties.LookUpTableName = reader["LookUpTableName"].ToString();
                    }

                    if (reader["DefaultValue"] != null)
                    {
                        attributeModelBaseProperties.DefaultValue = reader["DefaultValue"].ToString();
                    }

                    if (reader["ComplexTableName"] != null)
                    {
                        attributeModelBaseProperties.ComplexTableName = reader["ComplexTableName"].ToString();
                    }

                    if (reader["IsComplex"] != null)
                    {
                        Boolean isComplex = ValueTypeHelper.ConvertToBoolean(reader["IsComplex"].ToString());

                        attributeModelBaseProperties.IsComplex = isComplex 
                            && (!(attributeModelBaseProperties.ComplexTableName.Equals("tb_SystemAttrVal", StringComparison.InvariantCultureIgnoreCase) || attributeModelBaseProperties.ComplexTableName.Equals("tb_System_Workflow", StringComparison.InvariantCultureIgnoreCase)));
                    }

                    if (reader["Path"] != null)
                    {
                        attributeModelBaseProperties.Path = reader["Path"].ToString();
                    }

                    if (reader["Searchable"] != null)
                    {
                        attributeModelBaseProperties.Searchable = ValueTypeHelper.ConvertToBoolean(reader["Searchable"].ToString());
                    }
                    
                    if (reader["EnableHistory"] != null)
                    {
                        attributeModelBaseProperties.EnableHistory = ValueTypeHelper.ConvertToBoolean(reader["EnableHistory"].ToString());
                    }

                    if (reader["ShowAtCreation"] != null)
                    {
                        attributeModelBaseProperties.ShowAtCreation = ValueTypeHelper.ConvertToBoolean(reader["ShowAtCreation"].ToString());
                    }
                    
                    if (reader["WebUri"] != null)
                    {
                        attributeModelBaseProperties.WebUri = reader["WebUri"].ToString();
                    }

                    if (reader["LKDisplayColumns"] != null)
                    {
                        attributeModelBaseProperties.LkDisplayColumns = reader["LKDisplayColumns"].ToString();
                    }

                    if (reader["LKSortOrder"] != null)
                    {
                        attributeModelBaseProperties.LkSortOrder = reader["LKSortOrder"].ToString();
                    }

                    if (reader["LKSearchColumns"] != null)
                    {
                        attributeModelBaseProperties.LkSearchColumns = reader["LKSearchColumns"].ToString();
                    }

                    if (reader["LKDisplayFormat"] != null)
                    {
                        attributeModelBaseProperties.LkDisplayFormat = reader["LKDisplayFormat"].ToString();
                    }

                    if (reader["SortOrder"] != null)
                    {
                        attributeModelBaseProperties.SortOrder = ValueTypeHelper.Int32TryParse(reader["SortOrder"].ToString(), 0);
                    }

                    if (reader["ExportMask"] != null)
                    {
                        attributeModelBaseProperties.ExportMask = reader["ExportMask"].ToString();
                    }

                    if (reader["Inheritable"] != null)
                    {
                        attributeModelBaseProperties.Inheritable = ValueTypeHelper.ConvertToBoolean(reader["Inheritable"].ToString());
                    }

                    if (reader["IsHidden"] != null)
                    {
                        attributeModelBaseProperties.IsHidden = ValueTypeHelper.ConvertToBoolean(reader["IsHidden"].ToString());
                    }

                    if (reader["IsDependent"] != null)
                    {
                        attributeModelBaseProperties.IsDependentAttribute = ValueTypeHelper.ConvertToBoolean(reader["IsDependent"].ToString());
                    }

                    if (reader["HasDependency"] != null)
                    {
                        attributeModelBaseProperties.HasDependentAttribute = ValueTypeHelper.ConvertToBoolean(reader["HasDependency"].ToString());
                    }

                    if (reader["IsPrecisionArbitrary"] != null)
                    {
                        attributeModelBaseProperties.IsPrecisionArbitrary = ValueTypeHelper.ConvertToBoolean(reader["IsPrecisionArbitrary"].ToString());
                    }

                    attributeModelBaseProperties.Locale = systemDataLocale;


                    #endregion

                    #region Check if attribute read is complex child and add it accordingly

                    String attrTypeNameInLowerCase = attributeModelBaseProperties.AttributeTypeName.ToLowerInvariant();

                    if (attrTypeNameInLowerCase != "metadataattribute" && attrTypeNameInLowerCase != "attributegroup"
                        && attrTypeNameInLowerCase != "techgroup" && attrTypeNameInLowerCase != "relationshipgroup")
                    {
                        Int32 attributeParentId = attributeModelBaseProperties.AttributeParentId;

                        if (attributeIdToBaseAttributeModelPropertiesDictionary.ContainsKey(attributeParentId))
                        {
                            AttributeModelBaseProperties attributeModelBasePropertiesParent = attributeIdToBaseAttributeModelPropertiesDictionary[attributeParentId];
                            String attrGroupType = attributeModelBasePropertiesParent.AttributeTypeName.ToLowerInvariant();

                            if (attrGroupType != "attributegroup" && attrGroupType != "techgroup" && attrGroupType != "relationshipgroup")
                            {
                                if (!attributeModelBasePropertiesParent.IsHierarchical)
                                {

                                    attributeModelBaseProperties.IsComplexChild = true;

                                    String complexChildColumnName = Regex.Replace(attributeModelBaseProperties.Name, Constants.ALLOWED_CHAR_REGEX_PATTERN, "_");

                                    if (!attributeModelBasePropertiesParent.ComplexTableColumnNameList.Contains(complexChildColumnName))
                                    {
                                        attributeModelBasePropertiesParent.ComplexTableColumnNameList.Add(complexChildColumnName);
                                        if (!String.IsNullOrWhiteSpace(attributeModelBaseProperties.AllowableUOM))
                                        {
                                            attributeModelBasePropertiesParent.ComplexTableColumnNameList.Add(String.Format(Constants.UOM_COLUMN_ID_FORMAT, complexChildColumnName));
                                        }
                                    }
                                }
                                else
                                {
                                    attributeModelBaseProperties.IsHierarchicalChild = true;
                                }

                                attributeModelBasePropertiesParent.ChildAttributeModelBaseProperties.Add(attributeModelBaseProperties);
                            }
                        }
                    }

                    #endregion

                    #region Create attribute Id to model object map

                    if (!attributeIdToBaseAttributeModelPropertiesDictionary.ContainsKey(attributeModelBaseProperties.Id))
                    {
                        attributeIdToBaseAttributeModelPropertiesDictionary.Add(attributeModelBaseProperties.Id, attributeModelBaseProperties);
                    }
                    else
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Multiple attribute models found for attribute id: {0}", attributeModelBaseProperties.Id), MDMTraceSource.AttributeModelGet);
                    }

                    #endregion

                    #region Create attribute name to attribute id map

                    String attributeNameKey = attributeModelBaseProperties.Name.ToLowerInvariant();

                    if (!String.IsNullOrWhiteSpace(attributeNameKey))
                    {
                        if (!attributeNameToAttributeIdMaps.ContainsKey(attributeNameKey))
                        {
                            attributeNameToAttributeIdMaps.Add(attributeNameKey, attributeModelBaseProperties.Id);
                        }
                    }

                    #endregion

                    #region Create attribute name + parent name to attribute id map
                    
                    String attributeUniqueIdentier = GetKey(attributeModelBaseProperties.Name, attributeModelBaseProperties.AttributeParentName);

                    if (!attributeUniqueIdentifierToAttributeIdMaps.ContainsKey(attributeUniqueIdentier))
                    {
                        attributeUniqueIdentifierToAttributeIdMaps.Add(attributeUniqueIdentier, attributeModelBaseProperties.Id);
                    }
                    else
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Multiple attribute models found for attribute name: {0} and attribute parent name: {1}", attributeModelBaseProperties.Name, attributeModelBaseProperties.AttributeParentName), MDMTraceSource.AttributeModelGet);
                    }

                    #endregion
                }
            }

            return attributeIdToBaseAttributeModelPropertiesDictionary;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="attributeModelCollection"></param>
        /// <param name="attributeOperationResultCollection"></param>
        private void UpdateAttributeOperationResults(SqlDataReader reader, AttributeModelCollection attributeModelCollection, AttributeOperationResultCollection attributeOperationResultCollection)
        {
            while (reader.Read())
            {
                Int64 id = 0;
                Int32 attributeId = 0;
                Boolean hasError = false;
                String errorCode = String.Empty;
                String idPath = String.Empty;
                //LocaleEnum locale = LocaleEnum.UnKnown;

                if (reader["Id"] != null)
                    Int64.TryParse(reader["Id"].ToString(), out id);
                if (reader["AttributeId"] != null)
                    Int32.TryParse(reader["AttributeId"].ToString(), out attributeId);
                //if (reader["Locale"] != null)
                //    Enum.TryParse<LocaleEnum>(reader["Locale"].ToString(), out locale);
                if (reader["HasError"] != null)
                    Boolean.TryParse(reader["HasError"].ToString(), out hasError);
                if (reader["ErrorCode"] != null)
                    errorCode = reader["ErrorCode"].ToString();

                //Get AttributeModel
                AttributeModel attributeModel = attributeModelCollection.SingleOrDefault(a => a.Id == id);
                //AttributeModel attributeModel = attributeModelCollection.SingleOrDefault(a => a.Id == id && a.Locale == locale);

                //Get AttributeOperationResult
                AttributeOperationResult attributeOperationResult = attributeOperationResultCollection.SingleOrDefault(aor => aor.AttributeId == id);

                if (attributeModel != null && attributeOperationResult != null)
                {
                    if (id < 0)
                    {
                        //Update the id with the new entityId
                        attributeModel.Id = attributeId;
                        attributeOperationResult.AttributeId = attributeId;
                    }

                    if (hasError)
                    {
                        attributeOperationResult.AddOperationResult(errorCode, String.Empty, OperationResultType.Error);

                    }
                    else
                    {
                        //No errors.. update status as Successful.
                        attributeOperationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                    }
                }
            }

            attributeOperationResultCollection.RefreshOperationResultStatus();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="attributeModels"></param>
        /// <param name="operationResults"></param>
        private void PopulateOperationResult(SqlDataReader reader, AttributeModelCollection attributeModels, DataModelOperationResultCollection operationResults)
        {
            while (reader.Read())
            {
                Int64 id = 0;
                Int32 attributeId = 0;
                Boolean hasError = false;
                String errorCode = String.Empty;
                LocaleEnum locale = LocaleEnum.UnKnown;

                if (reader["Id"] != null)
                    Int64.TryParse(reader["Id"].ToString(), out id);
                if (reader["AttributeId"] != null)
                    Int32.TryParse(reader["AttributeId"].ToString(), out attributeId);
                if (reader["LocaleId"] != null)
                    Enum.TryParse<LocaleEnum>(reader["LocaleId"].ToString(), out locale);
                if (reader["HasError"] != null)
                    Boolean.TryParse(reader["HasError"].ToString(), out hasError);
                if (reader["ErrorCode"] != null)
                    errorCode = reader["ErrorCode"].ToString();

                //Get AttributeModel
                AttributeModel attributeModel = attributeModels.SingleOrDefault(a => a.Id == id && a.Locale == locale);

                //Get data model operation result
                DataModelOperationResult operationResult = operationResults.SingleOrDefault(aor => aor.ReferenceId == attributeModel.ReferenceId);

                if (attributeModel != null && operationResult != null)
                {
                    if (id < 0)
                    {
                        //Update the id with the new attribute id
                        attributeModel.Id = attributeId;
                    }

                    if (hasError)
                    {
                        operationResult.AddOperationResult(errorCode, String.Empty, OperationResultType.Error);
                    }
                    else
                    {
                        //No errors.. update status as Successful.
                        operationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private Dictionary<String, AttributeModelLocaleProperties> PopulateAttributeModelLocaleProperties(SqlDataReader reader)
        {
            Dictionary<String, AttributeModelLocaleProperties> attributeModelLocalePropertiesCollection = new Dictionary<String, AttributeModelLocaleProperties>();

            if (reader != null)
            {
                while (reader.Read())
                {
                    AttributeModelLocaleProperties attributeModelLocaleProperties = new AttributeModelLocaleProperties();
                    Int32 attributeParentId = 0;
                    String attributeTypeName = String.Empty;

                    #region Reading Properties

                    if (reader["Id"] != null)
                    {
                        Int32 attributeId = 0;
                        Int32.TryParse(reader["Id"].ToString(), out attributeId);
                        attributeModelLocaleProperties.Id = attributeId;
                    }

                    if (reader["LongName"] != null)
                    {
                        attributeModelLocaleProperties.LongName = reader["LongName"].ToString();
                    }

                    if (reader["AttributeParentId"] != null)
                    {
                        attributeParentId = ValueTypeHelper.Int32TryParse(reader["AttributeParentId"].ToString(), attributeParentId);
                    }

                    if (reader["AttributeTypeName"] != null)
                    {
                        attributeTypeName = reader["AttributeTypeName"].ToString();
                    }

                    attributeModelLocaleProperties.ExtendedProperties = String.Concat(attributeParentId, "@#@", attributeTypeName);

                    if (reader["AttributeParentLongName"] != null)
                    {
                        attributeModelLocaleProperties.AttributeParentLongName = reader["AttributeParentLongName"].ToString();
                    }

                    if (reader["AllowVal"] != null)
                    {
                        attributeModelLocaleProperties.AllowableValues = reader["AllowVal"].ToString();
                    }

                    if (reader["MaxLength"] != null)
                    {
                        Int32 maxLength = 0;
                        Int32.TryParse(reader["MaxLength"].ToString(), out maxLength);
                        attributeModelLocaleProperties.MaxLength = maxLength;
                    }

                    if (reader["MinLength"] != null)
                    {
                        Int32 minLength = 0;
                        Int32.TryParse(reader["MinLength"].ToString(), out minLength);
                        attributeModelLocaleProperties.MinLength = minLength;
                    }

                    if (reader["AllowableUOM"] != null)
                    {
                        attributeModelLocaleProperties.AllowableUOM = reader["AllowableUOM"].ToString();
                    }

                    if (reader["DefaultUOM"] != null)
                    {
                        attributeModelLocaleProperties.DefaultUOM = reader["DefaultUOM"].ToString();
                    }

                    if (reader["MinInclusive"] != null)
                    {
                        attributeModelLocaleProperties.MinInclusive = reader["MinInclusive"].ToString();
                    }

                    if (reader["MinExclusive"] != null)
                    {
                        attributeModelLocaleProperties.MinExclusive = reader["MinExclusive"].ToString();
                    }

                    if (reader["MaxInclusive"] != null)
                    {
                        attributeModelLocaleProperties.MaxInclusive = reader["MaxInclusive"].ToString();
                    }

                    if (reader["MaxExclusive"] != null)
                    {
                        attributeModelLocaleProperties.MaxExclusive = reader["MaxExclusive"].ToString();
                    }

                    if (reader["DefaultValue"] != null)
                    {
                        attributeModelLocaleProperties.DefaultValue = reader["DefaultValue"].ToString();
                    }

                    if (reader["ExportMask"] != null)
                    {
                        attributeModelLocaleProperties.ExportMask = reader["ExportMask"].ToString();
                    }

                    if (reader["FK_Locale"] != null)
                    {
                        Int32 localeId = 0;
                        Int32.TryParse(reader["FK_Locale"].ToString(), out localeId);
                        attributeModelLocaleProperties.Locale = (LocaleEnum)localeId;
                    }

                    if (reader["Definition"] != null)
                    {
                        attributeModelLocaleProperties.Definition = reader["Definition"].ToString();
                    }

                    if (reader["Example"] != null)
                    {
                        attributeModelLocaleProperties.AttributeExample = reader["Example"].ToString();
                    }

                    if (reader["BusinessRule"] != null)
                    {
                        attributeModelLocaleProperties.BusinessRule = reader["BusinessRule"].ToString();
                    }

                    #endregion

                    String key = String.Empty;
                    String attrTypeNameInLowerCase = attributeTypeName.ToLowerInvariant();

                    if (attrTypeNameInLowerCase != "metadataattribute" && attrTypeNameInLowerCase != "attributegroup"
                        && attrTypeNameInLowerCase != "techgroup" && attrTypeNameInLowerCase != "relationshipgroup")
                    {
                        key = GetKey(attributeParentId, attributeModelLocaleProperties.Locale);

                        if (attributeModelLocalePropertiesCollection.ContainsKey(key))
                        {
                            AttributeModelLocaleProperties attributeModelLocalePropertiesParent = attributeModelLocalePropertiesCollection[key];

                            String[] values = attributeModelLocalePropertiesParent.ExtendedProperties.Split(new String[] { "@#@" }, StringSplitOptions.None);

                            String attrGroupType = values[1].ToLowerInvariant();

                            if (attrGroupType != "attributegroup" && attrGroupType != "techgroup" && attrGroupType != "relationshipgroup")
                            {
                                attributeModelLocalePropertiesParent.ChildAttributeModelLocaleProperties.Add(attributeModelLocaleProperties);
                            }
                        }
                    }

                    key = GetKey(attributeModelLocaleProperties.Id, attributeModelLocaleProperties.Locale);

                    if (!attributeModelLocalePropertiesCollection.ContainsKey(key))
                        attributeModelLocalePropertiesCollection.Add(key, attributeModelLocaleProperties);
                    else
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Multiple attribute models found for attribute id: {0} and locale:{1}", attributeModelLocaleProperties.Id, attributeModelLocaleProperties.Locale), MDMTraceSource.AttributeModelGet);
                }
            }

            return attributeModelLocalePropertiesCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeId"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        private String GetKey(Int32 attributeId, LocaleEnum locale)
        {
            return String.Concat(attributeId, "_", (Int32)locale);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeParentName"></param>
        /// <returns></returns>
        private String GetKey(String attributeName, String attributeParentName)
        {
            return String.Concat(attributeName, Constants.STRING_PATH_SEPARATOR, attributeParentName);
        }

        /// <summary>
        /// Generates Container Id Table from Collection of ContainerIds
        /// </summary>
        /// <param name="containerIdList">Indicates containerId List in SearchCriteria</param>
        /// <param name="containersMetadata">Indicates containers sql metadata</param>
        /// <returns>List of Container Ids</returns>
        private List<SqlDataRecord> CreateContainerIdTable(Collection<Int32> containerIdList, SqlMetaData[] containersMetadata)
        {
            List<SqlDataRecord> sqlContainerIdList = null;

            if (containerIdList.Count > 0)
            {
                sqlContainerIdList = new List<SqlDataRecord>();
                foreach (Int32 containerId in containerIdList)
                {
                    SqlDataRecord containerIdRecord = new SqlDataRecord(containersMetadata);
                    containerIdRecord.SetValue(0, containerId);
                    sqlContainerIdList.Add(containerIdRecord);
                }
            }

            return sqlContainerIdList;
        }

        #endregion

        #endregion
    }
}