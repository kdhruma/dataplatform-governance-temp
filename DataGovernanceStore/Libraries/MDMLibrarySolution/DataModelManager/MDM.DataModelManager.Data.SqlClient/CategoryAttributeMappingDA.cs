using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Transactions;

namespace MDM.DataModelManager.Data
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.DataModel;
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.Utility;

    /// <summary>
    /// Data access for Category - Attribute Mapping object
    /// </summary>
    public class CategoryAttributeMappingDA : SqlClientDataAccessBase
    {
        #region Fields

        /// <summary>
        /// Field denoting Category path separator.
        /// </summary>
        private String _categoryPathSeparator = AppConfigurationHelper.GetAppConfig<String>("Catalog.Category.PathSeparator", " » ");

        #endregion

        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get all Category - Attribute mappings
        /// </summary>
        /// <param name="categoryId">The category identifier.</param>
        /// <param name="containerId">The catalog identifier.</param>
        /// <param name="hierarchyId">The hierarchy identifier.</param>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public CategoryAttributeMappingCollection Get(Int64 categoryId, Int32 containerId, Int32 hierarchyId, LocaleEnum systemDataLocale, DBCommandProperties command)
        {
            return Get(categoryId,
                       containerId,
                       hierarchyId,
                       (Int32)GlobalizationHelper.GetSystemDataLocale(),
                       false,
                       command);
        }

        /// <summary>
        /// Get all Category - Attribute mappings
        /// </summary>
        /// <param name="categoryId">The category identifier.</param>
        /// <param name="containerId">The catalog identifier.</param>
        /// <param name="hierarchyId">The hierarchy identifier.</param>
        /// <param name="localeId">Locale identifier.</param>
        /// <param name="returnUniqueAttributes">Value indicates that unique attributes should be returned</param>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public CategoryAttributeMappingCollection Get(Int64 categoryId, Int32 containerId, Int32 hierarchyId, Int32 localeId, Boolean returnUniqueAttributes, DBCommandProperties command)
        {

            CategoryAttributeMappingCollection categoryAttributeMappingCollection = new CategoryAttributeMappingCollection();
            SqlDataReader reader = null;

            try
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StartTraceActivity("CategoryAttributeMappingDA.Get", MDMTraceSource.DataModel, false);
                }

                SqlParametersGenerator generator = new SqlParametersGenerator("DataModelManager_SqlParameters");
                SqlParameter[] parameters = generator.GetParameters("DataModelManager_CategoryAttributeMap_Get_ParametersArray");

                parameters[0].Value = categoryId;
                parameters[1].Value = containerId;
                parameters[2].Value = hierarchyId;
                parameters[3].Value = localeId;
                parameters[4].Value = returnUniqueAttributes;

                const String storedProcedureName = "usp_DataModelManager_CategoryAttributeMap_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    categoryAttributeMappingCollection = PopulateCategoryAttributeMappingProperties(reader);
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("CategoryAttributeMappingDA.Get", MDMTraceSource.DataModel);
                }
            }

            return categoryAttributeMappingCollection;
        }

        /// <summary>
        /// Processes categoryAttributeMapping in accordance to operation
        /// </summary>
        /// <param name="categoryAttributeMappingCollection">Collection of Category - Attribute Mapping</param>
        /// <param name="programName">Name of the program</param>
        /// <param name="userName">Name of the User</param>
        /// <param name="systemDataLocale">systems data locale</param>
        /// <param name="command">The command</param>
        /// <returns>The result of the operation </returns>
        public OperationResultCollection Process(CategoryAttributeMappingCollection categoryAttributeMappingCollection, string programName, string userName, LocaleEnum systemDataLocale, DBCommandProperties command)
        {
            SqlDataReader reader = null;
            OperationResultCollection categoryAttrMappingProcessOperationResultCollection = new OperationResultCollection();
            OperationResult operationResult = new OperationResult();

            try
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StartTraceActivity("CategoryAttributeMappingDA.Process", MDMTraceSource.DataModel, false);
                }

                const String storedProcedureName = "usp_DataModelManager_CategoryAttributeMap_Process";

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    SqlParametersGenerator generator = new SqlParametersGenerator("DataModelManager_SqlParameters");
                    SqlParameter[] parameters = generator.GetParameters("DataModelManager_CategoryAttributeMap_Process_ParametersArray");
                    SqlMetaData[] categoryAttributeMapMetaData = generator.GetTableValueMetadata("DataModelManager_CategoryAttributeMap_Process_ParametersArray", parameters[0].ParameterName);
                    SqlMetaData[] attributeMasterMetaData = generator.GetTableValueMetadata("DataModelManager_CategoryAttributeMap_Process_ParametersArray", parameters[1].ParameterName);

                    List<SqlDataRecord> categoryAttributeMapTable = new List<SqlDataRecord>();
                    List<SqlDataRecord> attributeMasterTable = new List<SqlDataRecord>();

                    FillTableValues(categoryAttributeMappingCollection, categoryAttributeMapMetaData, ref categoryAttributeMapTable, attributeMasterMetaData, ref attributeMasterTable);

                    parameters[0].Value = categoryAttributeMapTable;
                    parameters[1].Value = attributeMasterTable;
                    parameters[2].Value = programName;
                    parameters[3].Value = userName;

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    //Need empty information to make sure correct operation result status is calculated.
                    operationResult.AddOperationResult(String.Empty, String.Empty, OperationResultType.Information);

                    categoryAttrMappingProcessOperationResultCollection.Add(operationResult);

                    PopulateOperationResult(reader, categoryAttrMappingProcessOperationResultCollection);

                    transactionScope.Complete();
                }
            }
            catch (Exception exception)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "CategoryAttributeMapping Process Failed." + exception.Message, MDMTraceSource.DataModel);
                operationResult.AddOperationResult(exception.Message, String.Empty, OperationResultType.Error);
                categoryAttrMappingProcessOperationResultCollection.Add(operationResult);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("CategoryAttributeMappingDA.Process", MDMTraceSource.DataModel);
                }
            }

            return categoryAttrMappingProcessOperationResultCollection;
        }

        /// <summary>
        /// Processes categoryAttributeMapping in accordance to operation
        /// </summary>
        /// <param name="categoryAttributeMappingCollection">Collection of Category - Attribute Mapping</param>
        ///  <param name="operationResults">Collection of DataModelOperationResult</param>
        /// <param name="programName">Name of the program</param>
        /// <param name="userName">Name of the User</param>
        /// <param name="systemDataLocale">systems data locale</param>
        /// <param name="command">The command</param>
        /// <returns></returns>
        public void Process(CategoryAttributeMappingCollection categoryAttributeMappingCollection, DataModelOperationResultCollection operationResults, string programName, string userName, LocaleEnum systemDataLocale, DBCommandProperties command)
        {
            SqlDataReader reader = null;

            try
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StartTraceActivity("CategoryAttributeMappingDA.Process", MDMTraceSource.DataModel, false);
                }

                const String storedProcedureName = "usp_DataModelManager_CategoryAttributeMap_Process";

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    SqlParametersGenerator generator = new SqlParametersGenerator("DataModelManager_SqlParameters");
                    SqlParameter[] parameters = generator.GetParameters("DataModelManager_CategoryAttributeMap_Process_ParametersArray");
                    SqlMetaData[] categoryAttributeMapMetaData = generator.GetTableValueMetadata("DataModelManager_CategoryAttributeMap_Process_ParametersArray", parameters[0].ParameterName);
                    SqlMetaData[] attributeMasterMetaData = generator.GetTableValueMetadata("DataModelManager_CategoryAttributeMap_Process_ParametersArray", parameters[1].ParameterName);

                    List<SqlDataRecord> categoryAttributeMapTable = new List<SqlDataRecord>();
                    List<SqlDataRecord> attributeMasterTable = new List<SqlDataRecord>();

                    FillTableValues(categoryAttributeMappingCollection, categoryAttributeMapMetaData, ref categoryAttributeMapTable, attributeMasterMetaData, ref attributeMasterTable);

                    parameters[0].Value = categoryAttributeMapTable;
                    parameters[1].Value = attributeMasterTable;
                    parameters[2].Value = programName;
                    parameters[3].Value = userName;

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    var dataModelMap = new Dictionary<Int32, IDataModelObject>();

                    foreach (var mapping in categoryAttributeMappingCollection)
                    {
                        if (!dataModelMap.ContainsKey(mapping.Id))
                        {
                            dataModelMap.Add(mapping.Id, mapping);
                        }
                    }

                    DataModelHelper.PopulateOperationResult(reader, operationResults, dataModelMap);

                    transactionScope.Complete();
                }
            }
            catch (Exception exception)
            {
                String errorMessage = String.Format("CategoryAttributeMapping Process Failed with {0}", exception.Message);
                PopulateOperationResult(errorMessage, operationResults, categoryAttributeMappingCollection);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("CategoryAttributeMappingDA.Process", MDMTraceSource.DataModel);
                }
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Copy the data from Reader to CategoryAttributeMappingCollection
        /// </summary>
        /// <param name="reader"></param>
        private CategoryAttributeMappingCollection PopulateCategoryAttributeMappingProperties(SqlDataReader reader)
        {
            CategoryAttributeMappingCollection categoryAttributeMappings = new CategoryAttributeMappingCollection();

            while (reader.Read())
            {
                CategoryAttributeMapping categoryAttributeMapping = new CategoryAttributeMapping();

                if (reader["ID"] != null)
                {
                    categoryAttributeMapping.Id = ValueTypeHelper.Int32TryParse(reader["ID"].ToString(), categoryAttributeMapping.Id);
                }
                if (reader["FK_CNodeParent"] != null)
                {
                    categoryAttributeMapping.ParentEntityId = ValueTypeHelper.Int32TryParse(reader["FK_CNodeParent"].ToString(), categoryAttributeMapping.ParentEntityId);
                }
                if (reader["CatalogID"] != null)
                {
                    categoryAttributeMapping.HierarchyId = ValueTypeHelper.Int32TryParse(reader["CatalogID"].ToString(), categoryAttributeMapping.HierarchyId);
                }
                if (reader["HierarchyName"] != null)
                {
                    categoryAttributeMapping.HierarchyName = reader["HierarchyName"].ToString();
                }
                if (reader["HierarchyLongName"] != null)
                {
                    categoryAttributeMapping.HierarchyLongName = reader["HierarchyLongName"].ToString();
                }
                if (reader["CategoryID"] != null)
                {
                    categoryAttributeMapping.CategoryId = ValueTypeHelper.Int64TryParse(reader["CategoryID"].ToString(), categoryAttributeMapping.CategoryId);
                }
                if (reader["CategoryName"] != null)
                {
                    categoryAttributeMapping.CategoryName = reader["CategoryName"].ToString();
                }
                if (reader["CategoryLongName"] != null)
                {
                    categoryAttributeMapping.CategoryLongName = Convert.ToString(reader["CategoryLongName"]);
                }
                if (reader["CategoryPath"] != null)
                {
                    categoryAttributeMapping.Path = reader["CategoryPath"].ToString().Replace(Constants.STRING_PATH_SEPARATOR, _categoryPathSeparator);
                }
                if (reader["AttributeID"] != null)
                {
                    categoryAttributeMapping.AttributeId = ValueTypeHelper.Int32TryParse(reader["AttributeID"].ToString(), categoryAttributeMapping.AttributeId);
                }
                if (reader["AttributeName"] != null)
                {
                    categoryAttributeMapping.AttributeName = reader["AttributeName"].ToString();
                }
                if (reader["AttributeLongName"] != null)
                {
                    categoryAttributeMapping.AttributeLongName = reader["AttributeLongName"].ToString();
                }
                if (reader["AttributeParentID"] != null)
                {
                    categoryAttributeMapping.AttributeParentId = ValueTypeHelper.Int32TryParse(reader["AttributeParentID"].ToString(), categoryAttributeMapping.AttributeParentId);
                }
                if (reader["AttributeParent"] != null)
                {
                    categoryAttributeMapping.AttributeParentName = reader["AttributeParent"].ToString();
                }
                if (reader["AttributeParentLongName"] != null)
                {
                    categoryAttributeMapping.AttributeParentLongName = reader["AttributeParentLongName"].ToString();
                }
                if (reader["AttributeDataTypeName"] != null)
                {
                    categoryAttributeMapping.AttributeDataTypeName = reader["AttributeDataTypeName"].ToString();
                }
                if (reader["AllowValues"] != null)
                {
                    categoryAttributeMapping.AllowableValues = reader["AllowValues"].ToString();
                }
                if (reader["MandatoryFlag"] != null)
                {
                    categoryAttributeMapping.MandatoryFlag = reader["MandatoryFlag"].ToString();

                    if (!String.IsNullOrWhiteSpace(categoryAttributeMapping.MandatoryFlag))
                    {
                        if (String.Compare(categoryAttributeMapping.MandatoryFlag, "y", true) == 0)
                        {
                            categoryAttributeMapping.Required = true;
                        }
                        else
                        {
                            categoryAttributeMapping.Required = false;
                        }
                    }
                }
                if (reader["IsDraft"] != null)
                {
                    categoryAttributeMapping.IsDraft = ValueTypeHelper.BooleanTryParse(reader["IsDraft"].ToString(), categoryAttributeMapping.IsDraft);
                }
                if (reader["SortOrder"] != null)
                {
                    String sortOrder = reader["SortOrder"].ToString();

                    if (!String.IsNullOrWhiteSpace(sortOrder))
                    {
                        categoryAttributeMapping.SortOrder = ValueTypeHelper.Int32TryParse(sortOrder, 0);
                    }
                }
                if (reader["SourceFlag"] != null)
                {
                    categoryAttributeMapping.SourceFlag = reader["SourceFlag"].ToString();

                    if (!String.IsNullOrWhiteSpace(categoryAttributeMapping.SourceFlag))
                    {
                        if (String.Compare(categoryAttributeMapping.SourceFlag, "I", true) == 0)
                        {
                            categoryAttributeMapping.IsInheritable = true;
                        }
                        else
                        {
                            categoryAttributeMapping.IsInheritable = false;
                        }
                    }
                }
                if (reader["FK_DNI_NodeCharacteristicTemplate"] != null)
                {
                    categoryAttributeMapping.DNINodeCharacteristicTemplateId = ValueTypeHelper.Int32TryParse(reader["FK_DNI_NodeCharacteristicTemplate"].ToString(), categoryAttributeMapping.DNINodeCharacteristicTemplateId);
                }
                if (reader["MaxLength"] != null)
                {
                    String maxLength = reader["MaxLength"].ToString();

                    if (!String.IsNullOrWhiteSpace(maxLength))
                    {
                        categoryAttributeMapping.MaxLength = ValueTypeHelper.Int32TryParse(maxLength, 0);
                    }
                }
                if (reader["MinLength"] != null)
                {
                    String minLength = reader["MinLength"].ToString();

                    if (!String.IsNullOrWhiteSpace(minLength))
                    {
                        categoryAttributeMapping.MinLength = ValueTypeHelper.Int32TryParse(minLength, 0);
                    }
                }
                if (reader["AllowableUOM"] != null)
                {
                    categoryAttributeMapping.AllowableUOM = reader["AllowableUOM"].ToString();
                }
                if (reader["DefaultUOM"] != null)
                {
                    categoryAttributeMapping.DefaultUOM = reader["DefaultUOM"].ToString();
                }
                if (reader["Precision"] != null)
                {
                    String precision = reader["Precision"].ToString();

                    if (!String.IsNullOrWhiteSpace(precision))
                    {
                        categoryAttributeMapping.Precision = ValueTypeHelper.Int32TryParse(precision, 0);
                    }
                }
                if (reader["MinInclusive"] != null)
                {
                    categoryAttributeMapping.MinInclusive = reader["MinInclusive"].ToString();
                }
                if (reader["MaxInclusive"] != null)
                {
                    categoryAttributeMapping.MaxInclusive = reader["MaxInclusive"].ToString();
                }
                if (reader["MinExclusive"] != null)
                {
                    categoryAttributeMapping.MinExclusive = reader["MinExclusive"].ToString();
                }
                if (reader["MaxExclusive"] != null)
                {
                    categoryAttributeMapping.MaxExclusive = reader["MaxExclusive"].ToString();
                }
                if (reader["Definition"] != null)
                {
                    categoryAttributeMapping.Definition = reader["Definition"].ToString();
                }
                if (reader["Example"] != null)
                {
                    categoryAttributeMapping.AttributeExample = reader["Example"].ToString();
                }
                if (reader["BusinessRule"] != null)
                {
                    categoryAttributeMapping.BusinessRule = reader["BusinessRule"].ToString();
                }
                if (reader["IsReadOnly"] != null)
                {
                    String readOnly = reader["IsReadOnly"].ToString();

                    if (!String.IsNullOrWhiteSpace(readOnly))
                    {
                        categoryAttributeMapping.ReadOnly = ValueTypeHelper.BooleanTryParse(readOnly, false);
                    }
                }
                if (reader["DefaultValue"] != null)
                {
                    categoryAttributeMapping.DefaultValue = reader["DefaultValue"].ToString();
                }
                if (reader["ExportMask"] != null)
                {
                    categoryAttributeMapping.ExportMask = reader["ExportMask"].ToString();
                }

                if (reader["InheritableOnly"] != null)
                {
                    categoryAttributeMapping.InheritableOnly = ValueTypeHelper.BooleanTryParse(reader["InheritableOnly"].ToString(), false);
                }

                if (reader["AutoPromotable"] != null)
                {
                    categoryAttributeMapping.AutoPromotable = ValueTypeHelper.BooleanTryParse(reader["AutoPromotable"].ToString(), false);
                }

                categoryAttributeMapping.IsSpecialized = true;

                categoryAttributeMappings.Add(categoryAttributeMapping);
            }

            return categoryAttributeMappings;
        }

        /// <summary>
        /// Fills the table values.
        /// </summary>
        /// <param name="categoryAttributeMappingCollection">The category attribute mapping collection.</param>
        /// <param name="categoryAttributeMapMetaData">The category attribute map meta data.</param>
        /// <param name="categoryAttributeMapTable">The category attribute map table.</param>
        /// <param name="attributeMasterMetaData">The attribute master meta data.</param>
        /// <param name="attributeMasterTable">The attribute master table.</param>
        private void FillTableValues(CategoryAttributeMappingCollection categoryAttributeMappingCollection, SqlMetaData[] categoryAttributeMapMetaData, ref List<SqlDataRecord> categoryAttributeMapTable, SqlMetaData[] attributeMasterMetaData, ref List<SqlDataRecord> attributeMasterTable)
        {
            Collection<Int32> uniqueAttributeIds = new Collection<Int32>();

            foreach (CategoryAttributeMapping categoryAttributeMapping in categoryAttributeMappingCollection)
            {
                var categoryAttributeMapRecord = new SqlDataRecord(categoryAttributeMapMetaData);
                categoryAttributeMapRecord.SetValue(0, categoryAttributeMapping.Id); // FK_NodeCharacteristicTemplate
                categoryAttributeMapRecord.SetValue(1, categoryAttributeMapping.AttributeId); // FK_Attribute
                categoryAttributeMapRecord.SetValue(2, categoryAttributeMapping.HierarchyId); // FK_Taxonomy
                categoryAttributeMapRecord.SetValue(3, categoryAttributeMapping.CategoryId); // FK_Category
                categoryAttributeMapRecord.SetValue(4, categoryAttributeMapping.IsInheritable); // Inheritable
                categoryAttributeMapRecord.SetValue(5, categoryAttributeMapping.InheritableOnly); // Inheritable Only
                categoryAttributeMapRecord.SetValue(6, categoryAttributeMapping.AutoPromotable); // Auto Promotable
                categoryAttributeMapRecord.SetValue(7, categoryAttributeMapping.Action.ToString());

                categoryAttributeMapTable.Add(categoryAttributeMapRecord);

                ConvertAttributeModelToSqlDataRecord(attributeMasterMetaData, ref attributeMasterTable, categoryAttributeMapping, ref uniqueAttributeIds);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="operationResultCollection"></param>
        private void PopulateOperationResult(SqlDataReader reader, OperationResultCollection operationResultCollection)
        {
            while (reader.Read())
            {
                Boolean hasError = false;
                String errorCode = String.Empty;
                String categoryAttributeMappingId = String.Empty;
                OperationResult operationResult = new OperationResult();

                if (reader["Id"] != null)
                {
                    categoryAttributeMappingId = reader["Id"].ToString();
                }

                if (reader["IsError"] != null)
                {
                    hasError = ValueTypeHelper.BooleanTryParse(reader["IsError"].ToString(), false);
                }

                if (reader["ErrorCode"] != null)
                {
                    errorCode = reader["ErrorCode"].ToString();
                }

                if (hasError & !String.IsNullOrEmpty(errorCode))
                {
                    operationResult.AddOperationResult(errorCode, String.Empty, OperationResultType.Error);
                }
                else
                {
                    operationResult.AddOperationResult("", "Category - Attribute Mapping ID: " + categoryAttributeMappingId, OperationResultType.Information);
                    operationResult.Id = ValueTypeHelper.Int32TryParse(categoryAttributeMappingId, 0);
                    operationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                    operationResult.ReturnValues.Add(categoryAttributeMappingId);
                }

                operationResultCollection.Add(operationResult);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="operationResults"></param>
        /// <param name="categoryAttributeMappingCollection"></param>
        /// <returns></returns>
        private void PopulateOperationResult(String errorMessage, DataModelOperationResultCollection operationResults, CategoryAttributeMappingCollection categoryAttributeMappingCollection)
        {
            MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, errorMessage, MDMTraceSource.DataModel);
            operationResults.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);

            foreach (CategoryAttributeMapping categoryAttributeMapping in categoryAttributeMappingCollection)
            {
                DataModelOperationResult operationResult = (DataModelOperationResult)operationResults.GetByReferenceId(categoryAttributeMapping.ReferenceId);
                operationResult.AddOperationResult(String.Empty, errorMessage, OperationResultType.Error);
            }
        }


        /// <summary>
        /// Converts the attribute model to SQL data record.
        /// </summary>
        /// <param name="attributeMasterMetaData">Indicates attribute master meta data.</param>
        /// <param name="attributeMasterTable">Indicates attribute master table.</param>
        /// <param name="categoryAttributeMapping">Indicates categoryAttributeMapping.</param>
        /// <param name="uniqueAttributeIds">Indicates collection of unique attribute ids that have been seen so far</param>
        private void ConvertAttributeModelToSqlDataRecord(SqlMetaData[] attributeMasterMetaData,
                                                                          ref List<SqlDataRecord> attributeMasterTable,
                                                                          CategoryAttributeMapping categoryAttributeMapping,
                                                                          ref Collection<Int32> uniqueAttributeIds)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information,
                                       "Convert attribute model to sql data record technical attribute starting...");
            }

            SqlDataRecord attributeModelRecord = new SqlDataRecord(attributeMasterMetaData);
            attributeModelRecord = FillAttributeModelSqlDataRecord(attributeModelRecord, categoryAttributeMapping);

            if (!uniqueAttributeIds.Contains(categoryAttributeMapping.AttributeId) || attributeMasterTable.Count == 0)
            {
                attributeMasterTable.Add(attributeModelRecord);

                if (!uniqueAttributeIds.Contains(categoryAttributeMapping.AttributeId))
                {
                    uniqueAttributeIds.Add(categoryAttributeMapping.AttributeId);
                }
            }

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information,
                                              "Convert attribute model to sql data record technical attribute completed...");
            }
        }

        /// <summary>
        /// Fills sqldata record with values in all columns like attribute shortname, longname, and other attributemetadata information
        /// </summary>
        /// <param name="attributeModelRecord">The attribute model record.</param>
        /// <param name="categoryAttributeMapping">The category attribute mapping.</param>
        /// <returns></returns>
        private SqlDataRecord FillAttributeModelSqlDataRecord(SqlDataRecord attributeModelRecord, CategoryAttributeMapping categoryAttributeMapping)
        {
            attributeModelRecord.SetValue(0, categoryAttributeMapping.AttributeId);
            attributeModelRecord.SetValue(1, null);//Name
            attributeModelRecord.SetValue(2, null);//LongName
            attributeModelRecord.SetValue(3, null);//AttributeParentId
            attributeModelRecord.SetValue(4, null);//AttributeParentName
            attributeModelRecord.SetValue(5, null);//AttributeParentLongName
            attributeModelRecord.SetValue(6, null);//AttributeTypeId
            attributeModelRecord.SetValue(7, null);//AttributeTypeName
            attributeModelRecord.SetValue(8, null);//AttributeDataTypeId
            attributeModelRecord.SetValue(9, null);//AttributeDataTypeName
            attributeModelRecord.SetValue(10, null);//AttributeDisplayTypeId
            attributeModelRecord.SetValue(11, null);//AttributeDisplayTypeName
            attributeModelRecord.SetValue(12, categoryAttributeMapping.Locale);
            attributeModelRecord.SetValue(13, categoryAttributeMapping.AllowableValues);
            attributeModelRecord.SetValue(14, categoryAttributeMapping.MaxLength);
            attributeModelRecord.SetValue(15, categoryAttributeMapping.MinLength);
            attributeModelRecord.SetValue(16, categoryAttributeMapping.Required);
            attributeModelRecord.SetValue(17, categoryAttributeMapping.AllowableUOM);
            attributeModelRecord.SetValue(18, categoryAttributeMapping.DefaultUOM);
            attributeModelRecord.SetValue(19, null);//UomType
            attributeModelRecord.SetValue(20, categoryAttributeMapping.Precision);
            attributeModelRecord.SetValue(21, null);//IsCollection
            attributeModelRecord.SetValue(22, categoryAttributeMapping.MinInclusive);
            attributeModelRecord.SetValue(23, categoryAttributeMapping.MaxInclusive);
            attributeModelRecord.SetValue(24, categoryAttributeMapping.MinExclusive);
            attributeModelRecord.SetValue(25, categoryAttributeMapping.MaxExclusive);
            attributeModelRecord.SetValue(26, null);//Label
            attributeModelRecord.SetValue(27, categoryAttributeMapping.Definition);
            attributeModelRecord.SetValue(28, categoryAttributeMapping.BusinessRule);
            attributeModelRecord.SetValue(29, categoryAttributeMapping.ReadOnly);
            attributeModelRecord.SetValue(30, null);//Extension
            attributeModelRecord.SetValue(31, null);//AttributeRegEx
            attributeModelRecord.SetValue(32, null);//LookUpTableName
            attributeModelRecord.SetValue(33, categoryAttributeMapping.DefaultValue);
            attributeModelRecord.SetValue(34, null);//ComplexTableName
            attributeModelRecord.SetValue(35, null);//Path
            attributeModelRecord.SetValue(36, null);//Searchable
            attributeModelRecord.SetValue(37, null);//Denormalize
            attributeModelRecord.SetValue(38, null);//EnableHistory
            attributeModelRecord.SetValue(39, null);//ShowAtCreation
            attributeModelRecord.SetValue(40, null);//WebUri
            attributeModelRecord.SetValue(41, null);//LkSortOrder
            attributeModelRecord.SetValue(42, null);//LkSearchColumns
            attributeModelRecord.SetValue(43, null);//LkDisplayColumns
            attributeModelRecord.SetValue(44, null);//LkDisplayFormat
            attributeModelRecord.SetValue(45, categoryAttributeMapping.SortOrder);
            attributeModelRecord.SetValue(46, categoryAttributeMapping.ExportMask);
            attributeModelRecord.SetValue(47, null);//Inheritable
            attributeModelRecord.SetValue(48, null);//IsHidden
            attributeModelRecord.SetValue(49, null);//IsComplex
            attributeModelRecord.SetValue(50, null);//IsLookup
            attributeModelRecord.SetValue(51, null);//IsLocalizable
            attributeModelRecord.SetValue(52, null);//ApplyLocaleFormat
            attributeModelRecord.SetValue(53, null);//ApplyTimeZoneConversion
            attributeModelRecord.SetValue(54, null);//AllowNullSearch
            attributeModelRecord.SetValue(55, categoryAttributeMapping.AttributeExample);
            attributeModelRecord.SetValue(56, null);//IsPrecisionArbitrary
            attributeModelRecord.SetValue(57, categoryAttributeMapping.Action.ToString());
            attributeModelRecord.SetValue(58, null);//RegExErrorMessage

            return attributeModelRecord;
        }

        #endregion Private Methods

        #endregion
    }
}