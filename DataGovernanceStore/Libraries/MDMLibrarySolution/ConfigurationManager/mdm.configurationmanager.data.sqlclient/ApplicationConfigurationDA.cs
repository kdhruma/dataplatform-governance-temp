using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Transactions;
using System.Diagnostics;
using Microsoft.SqlServer.Server;

namespace MDM.ConfigurationManager.Data
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Utility;

    /// <summary>
    /// Specifies the data access operations for Application Configurations
    /// </summary>
    public class ApplicationConfigurationDA : SqlClientDataAccessBase
    {
        #region Fields 

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Returns Application Configuration scripts as per the provided where clause
        /// </summary>
        /// <param name="whereClause">Where clause having filter data</param>
        /// <returns>DataTable containing scripts</returns>
        public DataTable GetApplicationConfigScripts(String whereClause)
        {
            DataTable result = null;

            SqlParameter[] parameters;
            SqlDataReader reader = null;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("ConfigurationManager_SqlParameters");

                parameters = generator.GetParameters("ConfigurationManager_ApplicationConfiguration_Scripts_Get_ParametersArray");

                parameters[0].Value = whereClause;

                storedProcedureName = "usp_Tool_ConfigurationManager_ApplicationConfiguration_Scripts_Get";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    result = new DataTable();
                    result.Load(reader);
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
        /// Update the custom application config based on the input application config excel file
        /// </summary>
        /// <param name="dsApplicationConfig">Indicates the application config dataset</param>
        /// <param name="command">Indicates the command having connection properties</param>
        /// <param name="callerContext">Indicates caller context specifying application and module</param>
        public OperationResult ProcessExtendedApplicationConfig(DataSet dsApplicationConfig, DBCommandProperties command, CallerContext callerContext)
        {
            OperationResult configProcessOperationResult = new OperationResult();

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("ApplicationConfigurationDA.ProcessExtendedApplicationConfig", MDMTraceSource.Application, false);
            }

            SqlDataReader reader = null;

            try
            {
                if (dsApplicationConfig != null && dsApplicationConfig.Tables != null && dsApplicationConfig.Tables.Count > 0)
                {
                    TransactionOptions transactionOptions = BusinessLogicBase.GetTransactionOptions(ProcessingMode.Sync);

                    if (callerContext != null && callerContext.Module == MDMCenterModules.CustomApplicationConfig)
                    {
                        //In case of CustomApplicationConfig tool, operation is not insert all application configs which is time consuming...
                        //So set transaction timeout to 30mins.
                        transactionOptions.Timeout = new TimeSpan(0, 30, 0);    
                    }

                    using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                    {
                        try
                        {
                            String storedProcedureName = "usp_ConfigurationManager_CustomizeApplicationConfig_Process";

                            SqlParametersGenerator generator = new SqlParametersGenerator("ConfigurationManager_SqlParameters");
                            SqlParameter[] parameters = generator.GetParameters("ConfigurationManager_ApplicationConfiguration_Process_ParametersArray");

                            if (PrepareTableValueParameters(dsApplicationConfig, generator, ref parameters))
                            {
                                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);
                                PopulateOperationResult(reader, configProcessOperationResult);
                            }
                        }
                        finally
                        {
                            if (reader != null)
                            {
                                reader.Close();
                            }
                        }

                        transactionScope.Complete();
                    }
                }
            }
            finally
            {
                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("ApplicationConfigurationDA.ProcessExtendedApplicationConfig", MDMTraceSource.Application);
                }
            }

            return configProcessOperationResult;
        }

        #endregion

        #region Private Methods

        private Boolean PrepareTableValueParameters(DataSet dsApplicationConfig, SqlParametersGenerator generator, ref SqlParameter[] parameters)
        {
            Boolean success = false;

            DataTable dtApplicationConfigs = dsApplicationConfig.Tables["Extended Application Configs"];

            if (dtApplicationConfigs != null && dtApplicationConfigs.Rows != null && dtApplicationConfigs.Rows.Count > 0)
            {
                SqlMetaData[] applicationConfigMetaData = generator.GetTableValueMetadata("ConfigurationManager_ApplicationConfiguration_Process_ParametersArray", parameters[0].ParameterName);
                SqlMetaData[] toolBarMetadata = generator.GetTableValueMetadata("ConfigurationManager_ApplicationConfiguration_Process_ParametersArray", parameters[1].ParameterName);
                SqlMetaData[] gridMetadata = generator.GetTableValueMetadata("ConfigurationManager_ApplicationConfiguration_Process_ParametersArray", parameters[2].ParameterName);
                SqlMetaData[] panelBarMetadata = generator.GetTableValueMetadata("ConfigurationManager_ApplicationConfiguration_Process_ParametersArray", parameters[3].ParameterName);
                SqlMetaData[] searchAttributeMetadata = generator.GetTableValueMetadata("ConfigurationManager_ApplicationConfiguration_Process_ParametersArray", parameters[4].ParameterName);

                Dictionary<String, String> configNameToConfigTypeMapping = new Dictionary<String, String>();

                foreach (DataRow configRow in dtApplicationConfigs.Rows)
                {
                    String configName = configRow[ApplicationConfigConstants.ApplicationConfigTemplateColumns[ApplicationConfigTemplateFieldEnum.ShortName]].ToString();
                    String configType = configRow[ApplicationConfigConstants.ApplicationConfigTemplateColumns[ApplicationConfigTemplateFieldEnum.ConfigType]].ToString();

                    if (!configNameToConfigTypeMapping.ContainsKey(configName))
                    {
                        configNameToConfigTypeMapping.Add(configName, configType);
                    }
                }

                parameters[0].Value = CreateApplicationConfigTable(dtApplicationConfigs, applicationConfigMetaData);
                parameters[1].Value = CreateToolBarTable(dsApplicationConfig.Tables["ToolBar Button Config"], toolBarMetadata);
                parameters[2].Value = CreateGridTable(dsApplicationConfig.Tables["Grid Columns Config"], gridMetadata, configNameToConfigTypeMapping);
                parameters[3].Value = CreatePanelBarTable(dsApplicationConfig.Tables["PanelBar Config"], panelBarMetadata, configNameToConfigTypeMapping);
                parameters[4].Value = CreateSearchAttributeTable(dsApplicationConfig.Tables["Explorer Search Attributes"], searchAttributeMetadata);

                success = true;
            }

            return success;
        }

        private List<SqlDataRecord> CreateApplicationConfigTable(DataTable dtApplicationConfig, SqlMetaData[] applicationConfigMetadata)
        {
            List<SqlDataRecord> applicationConfigTable = new List<SqlDataRecord>();

            if (dtApplicationConfig != null && dtApplicationConfig.Rows != null && dtApplicationConfig.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dtApplicationConfig.Rows)
                {
                    SqlDataRecord applicationConfigRecord = new SqlDataRecord(applicationConfigMetadata);
                    applicationConfigRecord.SetValue(0, dataRow[ApplicationConfigConstants.ApplicationConfigTemplateColumns[ApplicationConfigTemplateFieldEnum.ConfigType]].ToString());
                    applicationConfigRecord.SetValue(1, dataRow[ApplicationConfigConstants.ApplicationConfigTemplateColumns[ApplicationConfigTemplateFieldEnum.ShortName]].ToString());
                    applicationConfigRecord.SetValue(2, dataRow[ApplicationConfigConstants.ApplicationConfigTemplateColumns[ApplicationConfigTemplateFieldEnum.ShortName]].ToString());
                    applicationConfigRecord.SetValue(3, dataRow[ApplicationConfigConstants.ApplicationConfigTemplateColumns[ApplicationConfigTemplateFieldEnum.ApplicationContextDefinition]].ToString());
                    applicationConfigRecord.SetValue(4, dataRow[ApplicationConfigConstants.ApplicationConfigTemplateColumns[ApplicationConfigTemplateFieldEnum.Organization]].ToString());
                    applicationConfigRecord.SetValue(5, dataRow[ApplicationConfigConstants.ApplicationConfigTemplateColumns[ApplicationConfigTemplateFieldEnum.Container]].ToString());
                    applicationConfigRecord.SetValue(6, dataRow[ApplicationConfigConstants.ApplicationConfigTemplateColumns[ApplicationConfigTemplateFieldEnum.Taxonomy]].ToString());
                    applicationConfigRecord.SetValue(7, dataRow[ApplicationConfigConstants.ApplicationConfigTemplateColumns[ApplicationConfigTemplateFieldEnum.CategoryPath]].ToString());
                    applicationConfigRecord.SetValue(8, dataRow[ApplicationConfigConstants.ApplicationConfigTemplateColumns[ApplicationConfigTemplateFieldEnum.EntityType]].ToString());
                    applicationConfigRecord.SetValue(9, dataRow[ApplicationConfigConstants.ApplicationConfigTemplateColumns[ApplicationConfigTemplateFieldEnum.AttributePath]].ToString());
                    applicationConfigRecord.SetValue(10, dataRow[ApplicationConfigConstants.ApplicationConfigTemplateColumns[ApplicationConfigTemplateFieldEnum.RelationshipType]].ToString());
                    applicationConfigRecord.SetValue(11, dataRow[ApplicationConfigConstants.ApplicationConfigTemplateColumns[ApplicationConfigTemplateFieldEnum.SecurityRole]].ToString());
                    applicationConfigRecord.SetValue(12, dataRow[ApplicationConfigConstants.ApplicationConfigTemplateColumns[ApplicationConfigTemplateFieldEnum.SecurityUserLogin]].ToString());
                    //applicationConfigRecord.SetValue(13, dataRow[ApplicationConfigConstants.ApplicationConfigTemplateColumns[ApplicationConfigTemplateFieldEnum.Locale]].ToString());
                    applicationConfigRecord.SetValue(13, dataRow[ApplicationConfigConstants.ApplicationConfigTemplateColumns[ApplicationConfigTemplateFieldEnum.IsPopulate]].ToString());

                    applicationConfigTable.Add(applicationConfigRecord);
                }
            }

            if (applicationConfigTable.Count < 1)
            {
                applicationConfigTable = null;
            }

            return applicationConfigTable;
        }

        private List<SqlDataRecord> CreateToolBarTable(DataTable dtToolBar, SqlMetaData[] toolBarMetadata)
        {
            List<SqlDataRecord> toolBarTable = new List<SqlDataRecord>();

            #region Prepare list of new pivot toolbar data table columns

            Collection<String> columnNames = new Collection<String>();
            columnNames.Add(ToolBarTemplateFieldEnum.ConfigName.ToString());
            columnNames.Add(ToolBarTemplateFieldEnum.ToolBarItemType.ToString());
            columnNames.Add(ToolBarTemplateFieldEnum.ToolBarItemKey.ToString());
            columnNames.Add(ToolBarTemplateFieldEnum.ToolBarItemParentKey.ToString());
            columnNames.Add(ToolBarTemplateFieldEnum.Event.ToString());
            columnNames.Add(ToolBarTemplateFieldEnum.Property.ToString());
            columnNames.Add(ToolBarTemplateFieldEnum.Value.ToString());
            columnNames.Add(ToolBarTemplateFieldEnum.Action.ToString());

            #endregion

            #region Prepare static column name list

            Dictionary<String, String> staticToolBarColumnDictionary = new Dictionary<String, String>();

            staticToolBarColumnDictionary.Add(ToolBarTemplateFieldEnum.ConfigName.ToString(), ApplicationConfigConstants.ToolBarTemplateColumns[ToolBarTemplateFieldEnum.ConfigName].ToString());
            staticToolBarColumnDictionary.Add(ToolBarTemplateFieldEnum.ToolBarItemType.ToString(), ApplicationConfigConstants.ToolBarTemplateColumns[ToolBarTemplateFieldEnum.ToolBarItemType].ToString());
            staticToolBarColumnDictionary.Add(ToolBarTemplateFieldEnum.ToolBarItemKey.ToString(), ApplicationConfigConstants.ToolBarTemplateColumns[ToolBarTemplateFieldEnum.ToolBarItemKey].ToString());
            staticToolBarColumnDictionary.Add(ToolBarTemplateFieldEnum.ToolBarItemParentKey.ToString(), ApplicationConfigConstants.ToolBarTemplateColumns[ToolBarTemplateFieldEnum.ToolBarItemParentKey].ToString());
            staticToolBarColumnDictionary.Add(ToolBarTemplateFieldEnum.Action.ToString(), ApplicationConfigConstants.ToolBarTemplateColumns[ToolBarTemplateFieldEnum.Action].ToString());

            #endregion

            #region Prepare new pivot toolbar data table

            DataTable pivotDataTable = PreparePivotDataTable(dtToolBar, columnNames, staticToolBarColumnDictionary);

            #endregion

            #region Map the new pivot data table to TVP

            toolBarTable = MapPivotDataTableToTVP(pivotDataTable, toolBarMetadata, columnNames);

            #endregion

            if (toolBarTable.Count < 1)
            {
                toolBarTable = null;
            }

            return toolBarTable;
        }

        private List<SqlDataRecord> CreateGridTable(DataTable dtGrid, SqlMetaData[] searchGridMetadata, Dictionary<String, String> configNameToConfigTypeMapping)
        {
            List<SqlDataRecord> searchGridTable = new List<SqlDataRecord>();

            #region Prepare list of new pivot search grid data table columns

            Collection<String> columnNames = new Collection<String>();

            columnNames.Add(GridTemplateFieldEnum.ConfigName.ToString());
            columnNames.Add(GridTemplateFieldEnum.ColumnType.ToString());
            columnNames.Add(GridTemplateFieldEnum.ColumnName.ToString());
            columnNames.Add(GridTemplateFieldEnum.ParentColumnName.ToString());
            columnNames.Add(GridTemplateFieldEnum.Property.ToString());
            columnNames.Add(GridTemplateFieldEnum.Value.ToString());
            columnNames.Add(GridTemplateFieldEnum.Action.ToString());

            #endregion

            #region Prepare static column name list

            Dictionary<String, String> staticColumnDictionary = new Dictionary<String, String>();

            staticColumnDictionary.Add(GridTemplateFieldEnum.ConfigName.ToString(), ApplicationConfigConstants.GridTemplateColumns[GridTemplateFieldEnum.ConfigName].ToString());
            staticColumnDictionary.Add(GridTemplateFieldEnum.ColumnName.ToString(), ApplicationConfigConstants.GridTemplateColumns[GridTemplateFieldEnum.ColumnName].ToString());
            staticColumnDictionary.Add(GridTemplateFieldEnum.Action.ToString(), ApplicationConfigConstants.GridTemplateColumns[GridTemplateFieldEnum.Action].ToString());

            Collection<String> dbColumns = new Collection<String> 
            {
                staticColumnDictionary[GridTemplateFieldEnum.ConfigName.ToString()], 
                staticColumnDictionary[GridTemplateFieldEnum.ColumnName.ToString()], 
                staticColumnDictionary[GridTemplateFieldEnum.Action.ToString()], 
                "Name", "DataField", "DataType", "DataFieldType"
            };

            #endregion

            #region Prepare new pivot search grid data table

            //Create a mew data table which map the excel column to the TVP columns.
            DataTable pivotDataTable = new DataTable();

            if (dtGrid != null && dtGrid.Rows != null && dtGrid.Rows.Count > 0)
            {   
                #region Create a new data table structure same as TVP

                pivotDataTable.TableName = dtGrid.TableName;

                Collection<DataColumn> dataColumns = new Collection<DataColumn>();

                if (columnNames != null && columnNames.Count > 0)
                {
                    foreach (String columnName in columnNames)
                    {
                        dataColumns.Add(new DataColumn(columnName));
                        pivotDataTable.Columns.Add(columnName);
                    }
                }

                #endregion

                #region Create a dynamic data table columns dictionary

                Dictionary<Int32, String> indexColumnNameMapping = new Dictionary<Int32, String>();
                int index = 0;
                foreach (DataColumn column in dtGrid.Columns)
                {
                    indexColumnNameMapping.Add(index++, column.ColumnName);
                }

                #endregion

                #region Populate the pivoted data table

                Int32 noOfColumns = dtGrid.Columns.Count;
                String propertyName = String.Empty;
                String propertyValue = String.Empty;

                foreach (DataRow dataRow in dtGrid.Rows)
                {
                    String configName = dataRow[ApplicationConfigConstants.GridTemplateColumns[GridTemplateFieldEnum.ConfigName]].ToString();
                    String configType = configNameToConfigTypeMapping.ContainsKey(configName) ? configNameToConfigTypeMapping[configName] : "Grid"; // default is "Grid"

                    for(int counter = 4;counter < noOfColumns;counter++)
                    {
                        propertyName = indexColumnNameMapping[counter];
                        propertyValue = dataRow[propertyName].ToString();

                        if (!String.IsNullOrWhiteSpace(propertyValue) || dataRow["Action"].ToString() == "Delete")
                        {
                            String columnKey = String.Empty;

                            if (configType.Equals("Grid"))
                            {
                                #region UI Column

                                DataRow newUIColumnDataRow = pivotDataTable.NewRow();

                                foreach (String staticColumnKey in staticColumnDictionary.Keys)
                                {
                                    newUIColumnDataRow[staticColumnKey] = dataRow[staticColumnDictionary[staticColumnKey]];

                                    if (staticColumnKey.Equals(GridTemplateFieldEnum.ColumnName.ToString()))
                                    {
                                        columnKey = newUIColumnDataRow[staticColumnKey] != null ? newUIColumnDataRow[staticColumnKey].ToString() : String.Empty;
                                    }
                                }

                                newUIColumnDataRow["ColumnType"] = "UIColumn";
                                newUIColumnDataRow["Property"] = propertyName;

                                if (propertyName.Equals("DataField")) // UIColumn's data field is always name of db column which is column key.
                                {
                                    newUIColumnDataRow["Value"] = columnKey;
                                }
                                else
                                {
                                    newUIColumnDataRow["Value"] = propertyValue;
                                }

                                pivotDataTable.Rows.Add(newUIColumnDataRow);

                                #endregion

                                #region DB Column

                                DataRow newDBColumnDataRow = null;

                                if (dbColumns.Contains(propertyName)) //if this particular property is part of db columns too
                                {
                                    newDBColumnDataRow = pivotDataTable.NewRow();

                                    foreach (String staticColumnKey in staticColumnDictionary.Keys)
                                    {
                                        newDBColumnDataRow[staticColumnKey] = dataRow[staticColumnDictionary[staticColumnKey]];
                                    }

                                    newDBColumnDataRow["ColumnType"] = "DBColumn";
                                    newDBColumnDataRow["Property"] = propertyName;

                                    if (propertyName.Equals("Name")) // DBcolumn's Name field is always column key which would be DataField value of UI Column
                                    {
                                        newDBColumnDataRow["Value"] = columnKey;
                                    }
                                    else
                                    {
                                        newDBColumnDataRow["Value"] = propertyValue;
                                    }

                                    pivotDataTable.Rows.Add(newDBColumnDataRow);
                                }

                                #endregion
                            }
                            else
                            {
                                #region DataColumn

                                DataRow newDataGridDataRow = null;

                                newDataGridDataRow = pivotDataTable.NewRow();

                                foreach (String staticColumnKey in staticColumnDictionary.Keys)
                                {
                                    newDataGridDataRow[staticColumnKey] = dataRow[staticColumnDictionary[staticColumnKey]];

                                    if (staticColumnKey.Equals(GridTemplateFieldEnum.ColumnName.ToString()))
                                    {
                                        columnKey = newDataGridDataRow[staticColumnKey] != null ? newDataGridDataRow[staticColumnKey].ToString() : String.Empty;
                                    }
                                }

                                newDataGridDataRow["ColumnType"] = "DataColumn";

                                if (propertyName.Equals("Visible"))
                                {
                                    propertyName = "Hidden";

                                    if (propertyValue.Equals("false"))
                                    {
                                        propertyValue = "true";
                                    }
                                    else
                                    {
                                        propertyValue = "false";
                                    }
                                }

                                newDataGridDataRow["Property"] = propertyName;
                                newDataGridDataRow["Value"] = propertyValue;

                                pivotDataTable.Rows.Add(newDataGridDataRow);

                                #endregion
                            }
                        }
                    }
                }

                #endregion
            }

            #endregion

            #region Map the new pivot data table to TVP

            searchGridTable = MapPivotDataTableToTVP(pivotDataTable, searchGridMetadata, columnNames);

            #endregion

            if (searchGridTable.Count < 1)
            {
                searchGridTable = null;
            }

            return searchGridTable;
        }

        private List<SqlDataRecord> CreatePanelBarTable(DataTable dtPanelBar, SqlMetaData[] panelBarMetadata, Dictionary<String, String> configNameToConfigTypeMapping)
        {
            List<SqlDataRecord> panelBarTable = new List<SqlDataRecord>();

            #region Prepare list of new pivot panelbar data table columns

            Collection<String> columnNames = new Collection<String>();
            columnNames.Add(PanelBarTemplateFieldEnum.ConfigName.ToString());
            columnNames.Add(PanelBarTemplateFieldEnum.PanelType.ToString());
            columnNames.Add(PanelBarTemplateFieldEnum.PanelName.ToString());
            columnNames.Add(PanelBarTemplateFieldEnum.ParentPanelName.ToString());
            columnNames.Add(PanelBarTemplateFieldEnum.Property.ToString());
            columnNames.Add(PanelBarTemplateFieldEnum.Value.ToString());
            columnNames.Add(PanelBarTemplateFieldEnum.Action.ToString());

            #endregion

            #region Prepare static column name list

            Dictionary<String, String> staticPanelBarColumnDictionary = new Dictionary<String, String>();

            staticPanelBarColumnDictionary.Add(PanelBarTemplateFieldEnum.ConfigName.ToString(), ApplicationConfigConstants.PanelBarTemplateColumns[PanelBarTemplateFieldEnum.ConfigName].ToString());
            //staticPanelBarColumnDictionary.Add(PanelBarTemplateFieldEnum.PanelType.ToString(), ApplicationConfigConstants.PanelBarTemplateColumns[PanelBarTemplateFieldEnum.PanelType].ToString());
            staticPanelBarColumnDictionary.Add(PanelBarTemplateFieldEnum.PanelName.ToString(), ApplicationConfigConstants.PanelBarTemplateColumns[PanelBarTemplateFieldEnum.PanelName].ToString());
            //staticPanelBarColumnDictionary.Add(PanelBarTemplateFieldEnum.ParentPanelName.ToString(), ApplicationConfigConstants.PanelBarTemplateColumns[PanelBarTemplateFieldEnum.ParentPanelName].ToString());
            staticPanelBarColumnDictionary.Add(PanelBarTemplateFieldEnum.Action.ToString(), ApplicationConfigConstants.PanelBarTemplateColumns[PanelBarTemplateFieldEnum.Action].ToString());

            #endregion

            #region Prepare new pivot panel bar dropdown data table

            DataTable pivotDataTable = PreparePivotDataTable(dtPanelBar, columnNames, staticPanelBarColumnDictionary);

            #endregion

            #region Map the new pivot data table to TVP

            panelBarTable = MapPivotDataTableToTVP(pivotDataTable, panelBarMetadata, columnNames);

            #endregion

            if (panelBarTable.Count < 1)
            {
                panelBarTable = null;
            }

            return panelBarTable;
        }

        private List<SqlDataRecord> CreateSearchAttributeTable(DataTable dataTable, SqlMetaData[] searchAttributeMetadata)
        {
            List<SqlDataRecord> searchAttributeTable = new List<SqlDataRecord>();

            #region Prepare list of new pivot search attributes data table columns

            Collection<String> columnNames = new Collection<String>();

            columnNames.Add(SearchAttributeTemplateFieldEnum.ConfigName.ToString());
            columnNames.Add(SearchAttributeTemplateFieldEnum.AttributeRuleType.ToString());
            columnNames.Add(SearchAttributeTemplateFieldEnum.AttributeRule.ToString());
            columnNames.Add(SearchAttributeTemplateFieldEnum.Property.ToString());
            columnNames.Add(SearchAttributeTemplateFieldEnum.Value.ToString());
            columnNames.Add(SearchAttributeTemplateFieldEnum.Action.ToString());

            #endregion

            #region Prepare static column name list

            Dictionary<String, String> staticSearchAttributesColumnDictionary = new Dictionary<String, String>();
            staticSearchAttributesColumnDictionary.Add(SearchAttributeTemplateFieldEnum.ConfigName.ToString(), ApplicationConfigConstants.SearchAttributeTemplateColumns[SearchAttributeTemplateFieldEnum.ConfigName].ToString());
            staticSearchAttributesColumnDictionary.Add(SearchAttributeTemplateFieldEnum.AttributeRuleType.ToString(), ApplicationConfigConstants.SearchAttributeTemplateColumns[SearchAttributeTemplateFieldEnum.AttributeRuleType].ToString());
            staticSearchAttributesColumnDictionary.Add(SearchAttributeTemplateFieldEnum.AttributeRule.ToString(), ApplicationConfigConstants.SearchAttributeTemplateColumns[SearchAttributeTemplateFieldEnum.AttributeRule].ToString());
            staticSearchAttributesColumnDictionary.Add(SearchAttributeTemplateFieldEnum.Action.ToString(), ApplicationConfigConstants.SearchAttributeTemplateColumns[SearchAttributeTemplateFieldEnum.Action].ToString());

            #endregion

            #region Prepare new pivot search attributes data table

            DataTable pivotDataTable = PreparePivotDataTable(dataTable, columnNames, staticSearchAttributesColumnDictionary);

            #endregion

            #region Map the new pivot data table to TVP

            searchAttributeTable = MapPivotDataTableToTVP(pivotDataTable, searchAttributeMetadata, columnNames);

            #endregion

            if (searchAttributeTable.Count < 1)
            {
                searchAttributeTable = null;
            }

            return searchAttributeTable;
        }

        private void PopulateOperationResult(SqlDataReader reader, OperationResult configProcessOperationResult)
        {
            while (reader.Read())
            {
                Boolean hasError = false;
                String errorMessage = String.Empty;
                String configName = String.Empty;

                if (reader["HasError"] != null)
                {
                    Boolean.TryParse(reader["HasError"].ToString(), out hasError);
                }

                if (reader["ErrorMessage"] != null)
                {
                    errorMessage = reader["ErrorMessage"].ToString();
                }

                if (reader["ConfigName"] != null)
                {
                    configName = reader["ConfigName"].ToString();
                }

                if (hasError)
                {
                    if (!String.IsNullOrEmpty(errorMessage))
                    {
                        if (!String.IsNullOrEmpty(configName))
                        {
                            errorMessage = String.Format("{0}: {1}", configName, errorMessage);
                        }

                        configProcessOperationResult.AddOperationResult("", errorMessage, OperationResultType.Error);
                    }

                    configProcessOperationResult.OperationResultStatus = OperationResultStatusEnum.Failed;
                }
                else
                {
                    configProcessOperationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                }

            }
        }

        private DataTable PreparePivotDataTable(DataTable dataTable, Collection<String> columnNames, Dictionary<String, String> staticColumnDictionary)
        {
            //Create a mew data table which map the excel column to the TVP columns.
            DataTable pivotDataTable = new DataTable();

            if (dataTable != null && dataTable.Rows != null && dataTable.Rows.Count > 0)
            {
                #region Create a new data table structure same as TVP

                pivotDataTable.TableName = dataTable.TableName;
                Collection<DataColumn> dataColumns = new Collection<DataColumn>();

                if(columnNames != null && columnNames.Count > 0)
                {
                    foreach(String columnName in columnNames)
                    {
                       dataColumns.Add(new DataColumn(columnName));
                       pivotDataTable.Columns.Add(columnName);
                    }
                }

                #endregion

                #region Create a dynamic data table columns dictionary

                Dictionary<Int32, String> indexColumnNameMapping = new Dictionary<Int32, String>();
                Int32 counter = 0;
                foreach (DataColumn column in dataTable.Columns)
                {
                    //These columns are compulsory static column mapped to the TVP columns except event column
                    if(!staticColumnDictionary.ContainsValue(column.ColumnName))
                    {
                        indexColumnNameMapping.Add(counter, column.ColumnName);
                    }

                    counter++;
                }

                #endregion

                #region Populate the new data table

                counter = staticColumnDictionary.Count; 
                Int32 noOfColumns = dataTable.Columns.Count;
                String propertyName = String.Empty;
                String propertyValue = String.Empty;

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    counter = staticColumnDictionary.Count;
                    String eventName = String.Empty;

                    while (noOfColumns - counter > 0)
                    {
                        propertyName = indexColumnNameMapping[counter];
                        propertyValue = dataRow[propertyName].ToString();

                        if (!String.IsNullOrWhiteSpace(propertyValue) || dataRow["Action"].ToString() == "Delete")
                        {
                            DataRow newDataRow = pivotDataTable.NewRow();

                            foreach(String staticColumnKey in staticColumnDictionary.Keys)
                            {
                                newDataRow[staticColumnKey] = dataRow[staticColumnDictionary[staticColumnKey]];
                            }

                           //TODO: Need to check how to set for event.

                            if (propertyName.Equals("EventName"))
                            {
                                eventName = propertyValue;
                            }

                            if (!String.IsNullOrWhiteSpace(eventName) && (propertyName.Equals("EventName") || propertyName.Equals("EventHandlerName")))
                            {
                                newDataRow["Event"] = eventName;
                            }

                            newDataRow["Property"] = propertyName;
                            newDataRow["Value"] = propertyValue;

                            pivotDataTable.Rows.Add(newDataRow);
                        }

                        counter++;
                    }
                }

                #endregion
            }

            return pivotDataTable;
        }

        private List<SqlDataRecord> MapPivotDataTableToTVP(DataTable pivotDataTable, SqlMetaData[] metadata, Collection<String> columnNames)
        {
            List<SqlDataRecord> sqlDataTable = new List<SqlDataRecord>();

            if (pivotDataTable != null && pivotDataTable.Rows != null && pivotDataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in pivotDataTable.Rows)
                {
                    SqlDataRecord sqlDataRecord = new SqlDataRecord(metadata);

                    Int32 counter = 0;
                    foreach (String columnName in columnNames)
                    {
                        sqlDataRecord.SetValue(counter, dataRow[columnName].ToString());
                        counter++;
                    }

                    sqlDataTable.Add(sqlDataRecord);
                }
            }

            return sqlDataTable;
        }

        #endregion

        #endregion Methods
    }
}
