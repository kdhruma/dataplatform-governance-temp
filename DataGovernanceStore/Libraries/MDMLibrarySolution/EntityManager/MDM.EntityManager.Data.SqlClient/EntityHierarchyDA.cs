using System;
using System.Linq;
using System.Data.SqlClient;
using System.Xml;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.SqlServer.Server;

namespace MDM.EntityManager.Data
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.Utility;
    using MDM.BusinessObjects.Diagnostics;

    /// <summary>
    /// Specifies data access operations for Entity Hierarchy
    /// </summary>
    public class EntityHierarchyDA : SqlClientDataAccessBase
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
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityHierarchyDefinitionId"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public Table GetDimensionValues(Int64 entityId, Int32 entityHierarchyDefinitionId, LocaleEnum locale)
        {
            var currentSettings  = MDMOperationContextHelper.GetCurrentTraceSettings();
            var currentActivity = new DiagnosticActivity();

            if (currentSettings != null && currentSettings.IsBasicTracingEnabled)
                currentActivity.Start();

            Table table = new Table();

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                parameters = generator.GetParameters("EntityManager_EntityHierarchy_GetDimensionValues_ParametersArray");

                parameters[0].Value = entityId;
                parameters[1].Value = entityHierarchyDefinitionId;

                storedProcedureName = "usp_EntityManager_EntityVariant_DimensionValues_Get";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);


                Column _columnEntityHierarchy_Rule_DimAttrValId = new Column();
                _columnEntityHierarchy_Rule_DimAttrValId.Name = "EntityHierarchy_AttrValId";
                table.AddColumn(_columnEntityHierarchy_Rule_DimAttrValId);

                Column _columnEntityId = new Column();
                _columnEntityId.Name = "EntityId";
                table.AddColumn(_columnEntityId);

                Column _columnEntityHierarchy_AttributeId = new Column();
                _columnEntityHierarchy_AttributeId.Name = "EntityHierarchy_AttributeId";
                table.AddColumn(_columnEntityHierarchy_AttributeId);

                Column _columnAttrVal = new Column();
                _columnAttrVal.Name = "AttrVal";
                table.AddColumn(_columnAttrVal);

                Column _columnLookupRefId = new Column();
                _columnLookupRefId.Name = "LookupRefId";
                table.AddColumn(_columnLookupRefId);

                Column _columnGroupId = new Column();
                _columnGroupId.Name = "GroupId";
                table.AddColumn(_columnGroupId);

                Column _columnFK_Locale = new Column();
                _columnFK_Locale.Name = "FK_Locale";
                table.AddColumn(_columnFK_Locale);

                Column _columnFK_AuditRef = new Column();
                _columnFK_AuditRef.Name = "FK_AuditRef";
                table.AddColumn(_columnFK_AuditRef);

                Column _columnEntityHierarchy_LevelId = new Column();
                _columnEntityHierarchy_LevelId.Name = "EntityHierarchy_LevelId";
                table.AddColumn(_columnEntityHierarchy_LevelId);


                while (reader.Read())
                {
                    Row _row = table.NewRow();
                    _row["EntityHierarchy_AttrValId"] = reader["PK_EntityVariant_AttrVal"].ToString();
                    _row["EntityId"] = reader["FK_Entity"].ToString();
                    _row["EntityHierarchy_AttributeId"] = reader["FK_Target_Attribute"].ToString();
                    _row["AttrVal"] = reader["AttrVal"].ToString();
                    _row["LookupRefId"] = reader["LookupRefId"].ToString();
                    _row["GroupId"] = reader["GroupId"].ToString();
                    _row["FK_Locale"] = reader["FK_Locale"].ToString();
                    _row["FK_AuditRef"] = reader["FK_AuditRef"].ToString();

                    _row["EntityHierarchy_LevelId"] = reader["PK_EntityVariant_Level"].ToString();

                }

                if (currentSettings.IsBasicTracingEnabled)
                    currentActivity.LogInformation("Fetched the dimension values using SP : " + storedProcedureName + ", row count : " + table.Rows.Count());
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (currentSettings != null && currentSettings.IsBasicTracingEnabled)
                    currentActivity.Stop();
            }

            return table;
        }

        /// <summary>
        /// Saves dimension values for a definition for an entity
        /// </summary>
        /// <param name="entityId">Id of Entity for which dimension values will be saved</param>
        /// <param name="entityHierarchyDefinition">EntityHierarchyDefinition which holds dimension values for each level within</param>
        /// <param name="loginUser">logged in user name.</param>
        /// <param name="programName">This parameter is specifying program name.</param>
        /// <param name="command">Name of application and module</param>
        /// <returns>'0' if operation is successful else '1' incase of failure.</returns>
        public Int32 ProcessDimensionValues(Int64 entityId, EntityVariantDefinition entityHierarchyDefinition, String loginUser, String programName, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityManager.EntityHierarchyDA.ProcessHierarchyGenerationRules", MDMTraceSource.EntityHierarchyProcess, false);

            Int32 output = 0; //success
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                parameters = generator.GetParameters("EntityManager_EntityHierarchy_ProcessDimensionValues_ParametersArray");
                SqlMetaData[] entityHierarchyDefinitionMetadata = generator.GetTableValueMetadata("EntityManager_EntityHierarchy_ProcessDimensionValues_ParametersArray", parameters[1].ParameterName);

                #region Populate Entity Hierarchy Definition TVP

                List<SqlDataRecord> entityHierarchyDefinitonList = null;

                if (entityHierarchyDefinition != null && entityHierarchyDefinition.EntityVariantLevels != null && entityHierarchyDefinition.EntityVariantLevels.Count > 0)
                {
                    entityHierarchyDefinitonList = new List<SqlDataRecord>();

                    foreach (EntityVariantLevel entityHierarchyLevel in entityHierarchyDefinition.EntityVariantLevels)
                    {
                        foreach (Row row in entityHierarchyLevel.DimensionValues.Rows)
                        {
                            foreach (Column column in entityHierarchyLevel.DimensionValues.Columns)
                            {
                                if (!column.Name.Contains("RefId") && !column.Name.Contains("GroupId") && !column.Name.Contains("FK_Locale")
                                    && !column.Name.Contains("EntityHierarchy_AttributeId") && row[column.Name] != null && row[column.Name].ToString().Trim() != "")
                                {
                                    SqlDataRecord entityHierarchyLevelRecord = new SqlDataRecord(entityHierarchyDefinitionMetadata);

                                    entityHierarchyLevelRecord.SetValue(0, ValueTypeHelper.Int32TryParse(column.Name, 0));
                                    entityHierarchyLevelRecord.SetValue(1, ValueTypeHelper.Int32TryParse(row["GroupId"].ToString(), 0));
                                    entityHierarchyLevelRecord.SetValue(2, ValueTypeHelper.Int32TryParse(row[column.Name + "RefId"].ToString(), 0));
                                    entityHierarchyLevelRecord.SetValue(3, row[column.Name].ToString());
                                    entityHierarchyLevelRecord.SetValue(4, (Int32)GlobalizationHelper.GetSystemDataLocale());
                                    entityHierarchyLevelRecord.SetValue(5, entityHierarchyLevel.Id);

                                    entityHierarchyDefinitonList.Add(entityHierarchyLevelRecord);
                                }
                            }
                        }
                    }
                }

                #endregion

                if (entityHierarchyDefinitonList != null && entityHierarchyDefinitonList.Count > 0)
                {
                    parameters[0].Value = entityId;
                    parameters[1].Value = entityHierarchyDefinitonList;
                    parameters[2].Value = loginUser;
                    parameters[3].Value = programName;

                    storedProcedureName = "usp_EntityManager_EntityVariant_DimensionValues_Process";

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Database call started...", MDMTraceSource.EntityHierarchyProcess);

                    output = ExecuteProcedureNonQuery(command.ConnectionString, parameters, storedProcedureName);

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Database call completed.", MDMTraceSource.EntityHierarchyProcess);
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("EntityManager.EntityHierarchyDA.ProcessHierarchyGenerationRules", MDMTraceSource.EntityHierarchyProcess);
            }

            return output;
        }

        /// <summary>
        /// Get Hierarchy Matrix
        /// </summary>
        /// <param name="entityId">Indicates entity id.</param>
        /// <param name="entityHierarchyMatrixId">Indicates entity hierarchy matrix id.</param>
        /// <param name="entityHierarchyDefinition">Indicates entity hierarchy definition for requested entity id.</param>
        /// <returns>table for get hierarchy matrix.</returns>
        public Table GetHierarchyMatrix(Int64 entityId, Int32 entityHierarchyMatrixId, EntityVariantDefinition entityHierarchyDefinition)
        {
            Table table = new Table();

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                parameters = generator.GetParameters("EntityManager_EntityHierarchy_GetHierarchyMatrix_ParametersArray");

                parameters[0].Value = entityId;
                parameters[1].Value = entityHierarchyMatrixId;

                storedProcedureName = "usp_EntityManager_EntityHierarchy_GetHierarchyMatrix";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                String itemMatrixXml = String.Empty;

                while (reader.Read())
                {
                    entityId = Convert.ToInt32(reader["FK_Entity"].ToString());
                    itemMatrixXml = reader["Matrix_XML"].ToString();
                }
                table = ParseDefinition(itemMatrixXml, entityHierarchyDefinition);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return table;
        }

        /// <summary>
        /// Process Hierarchy Matrix
        /// </summary>
        /// <param name="entityId">Indicates entity id.</param>
        /// <param name="matrixTable">Indicates matrix table.</param>
        /// <param name="entityHierarchyDefinition">Indicates entity hierarchy definition for requested entity id.</param>
        /// <param name="loginUser">name of logged in user.</param>
        /// <param name="programName">Indicates program name.s</param>
        /// <returns>'0' if operation is successful else '1' incase of failure.</returns>
        public Int32 ProcessHierarchyMatrix(Int64 entityId, Table matrixTable, EntityVariantDefinition entityHierarchyDefinition, String loginUser, String programName)
        {
            Int32 output = 0; //success
            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            connectionString = AppConfigurationHelper.ConnectionString;
            String itemMatrixXml = ConvertToItemMatrixXML(matrixTable, entityHierarchyDefinition);
            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                parameters = generator.GetParameters("EntityManager_EntityHierarchy_ProcessHierarchyMatrix_ParametersArray");

                parameters[0].Value = entityId;
                parameters[1].Value = itemMatrixXml;
                parameters[2].Value = loginUser;
                parameters[3].Value = programName;


                storedProcedureName = "usp_EntityManager_EntityHierarchy_ProcessHierarchyMatrix";

                output = ExecuteProcedureNonQuery(connectionString, parameters, storedProcedureName);
            }
            finally
            {

            }

            return output;
        }

        /// <summary>
        /// Checks if HierarchyDefinition is modified or not
        /// </summary>
        /// <param name="entityId">Id of an Entity</param>
        /// <param name="hierarchyDefinationId">Id of HierarchyDefinition</param>
        /// <param name="command">command having connection properties</param>
        /// <returns>True if HierarchyDefinition is latest</returns>
        public Boolean IsLatestMatrix(Int64 entityId, Int32 hierarchyDefinationId, DBCommandProperties command)
        {
            Boolean isLatest = true;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            Object output = 0;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                parameters = generator.GetParameters("EntityManager_EntityHierarchy_IsLatestMatrix_ParametersArray");

                parameters[0].Value = entityId;
                parameters[1].Value = hierarchyDefinationId;

                storedProcedureName = "usp_EntityManager_EntityHierarchy_Matrix_IsLatest";

                output = ExecuteProcedureScalar(command.ConnectionString, parameters, storedProcedureName);

                if (ValueTypeHelper.Int32TryParse(output.ToString(), 1) == 0)
                    isLatest = false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return isLatest;
        }

        /// <summary>
        /// Gets child entities for the requested parent entity Id.
        /// </summary>
        /// <param name="parentEntityId">parent entity id for which child entities are required.</param>
        /// <param name="childEntityTypeId">Indicates child entity type id.</param>
        /// <param name="locale">Indicates locale</param>
        /// <param name="attributeLocaleList">Indicates return attribute id along with child entities.</param>
        /// <param name="getCompleteDetailsOfEntity">Flag saying whether to load complete details of entity.
        /// If flag is true then it returns CategoryId, CategoryName/LongName , Parent EntityId , Parent EntityName/LongName etc. else EntityId,EntityName and EntityLongName.</param>
        /// <param name="getRecursiveChildren">Get all child entities recursively</param>
        /// <param name="command">Object having command properties</param>
        /// <returns>collection of child entities.</returns>
        public EntityCollection GetChildEntitiesLegacy(Int64 parentEntityId, Int32 childEntityTypeId, LocaleEnum locale, Dictionary<Int32, LocaleEnum> attributeLocaleList, Boolean getCompleteDetailsOfEntity, Boolean getRecursiveChildren, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("HierarchyRelationshipDA.GetChildEntities", MDMTraceSource.HierarchyRelationshipGet, false);

            SqlDataReader reader = null;
            Entity entity = null;
            Attribute attribute = null;
            AttributeCollection attributes = null;
            EntityCollection childEntities = new EntityCollection();

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                SqlParameter[] parameters = generator.GetParameters("EntityManager_HierarchyRelationship_GetChildEntities_ParametersArray");

                List<SqlDataRecord> attributeTable = null;

                SqlMetaData[] attributeListMetaData = generator.GetTableValueMetadata("EntityManager_HierarchyRelationship_GetChildEntities_ParametersArray", parameters[2].ParameterName);

                if (attributeLocaleList != null && attributeLocaleList.Count > 0)
                {
                    attributeTable = new List<SqlDataRecord>();

                    foreach (KeyValuePair<Int32, LocaleEnum> attributeLocale in attributeLocaleList)
                    {
                        SqlDataRecord attributeRecord = new SqlDataRecord(attributeListMetaData);
                        attributeRecord.SetValue(0, attributeLocale.Key);
                        attributeRecord.SetValue(1, attributeLocale.Value);
                        attributeTable.Add(attributeRecord);
                    }
                }

                parameters[0].Value = parentEntityId;
                parameters[1].Value = childEntityTypeId;
                parameters[2].Value = attributeTable;
                parameters[3].Value = getCompleteDetailsOfEntity;
                parameters[4].Value = getRecursiveChildren;

                const String storedProcedureName = "usp_EntityManager_Entity_Child_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Reading data from Reader...", MDMTraceSource.HierarchyRelationshipGet);

                    while (reader.Read())
                    {
                        entity = new Entity();
                        attributes = new AttributeCollection();

                        if (reader["EntityId"] != null)
                        {
                            entity.Id = ValueTypeHelper.Int64TryParse(reader["EntityId"].ToString(), entity.Id);
                        }

                        entity.EntityTypeId = childEntityTypeId;

                        if (reader["EntityName"] != null)
                        {
                            entity.Name = reader["EntityName"].ToString();
                        }

                        if (reader["EntityLongName"] != null)
                        {
                            entity.LongName = reader["EntityLongName"].ToString();
                        }

                        if (reader["EntityTypeId"] != null)
                        {
                            entity.EntityTypeId = ValueTypeHelper.Int32TryParse(reader["EntityTypeId"].ToString(), entity.EntityTypeId);
                        }

                        if (reader["ContainerId"] != null)
                        {
                            entity.ContainerId = ValueTypeHelper.Int32TryParse(reader["ContainerId"].ToString(), entity.ContainerId);
                        }

                        if (reader["CategoryId"] != null)
                        {
                            entity.CategoryId = ValueTypeHelper.Int64TryParse(reader["CategoryId"].ToString(), entity.CategoryId);
                        }

                        if (reader["CategoryName"] != null)
                        {
                            entity.CategoryName = reader["CategoryName"].ToString();
                        }

                        if (reader["CategoryLongName"] != null)
                        {
                            entity.CategoryLongName = reader["CategoryLongName"].ToString();
                        }

                        if (reader["ParentEntityId"] != null)
                        {
                            entity.ParentEntityId = ValueTypeHelper.Int64TryParse(reader["ParentEntityId"].ToString(), entity.ParentEntityId);
                        }

                        if (reader["ParentEntityName"] != null)
                        {
                            entity.ParentEntityName = reader["ParentEntityName"].ToString();
                        }

                        if (reader["ParentEntityLongName"] != null)
                        {
                            entity.ParentEntityLongName = reader["ParentEntityLongName"].ToString();
                        }

                        if (reader["ParentExtensionEntityId"] != null)
                        {
                            entity.ParentExtensionEntityId = ValueTypeHelper.Int64TryParse(reader["ParentExtensionEntityId"].ToString(), 0);
                        }

                        if (attributeLocaleList != null && attributeLocaleList.Count > 0)
                        {
                            foreach (KeyValuePair<Int32, LocaleEnum> attributeId in attributeLocaleList)
                            {
                                attribute = new Attribute();

                                if (reader[attributeId.Key.ToString()] != null)
                                {
                                    attribute.Id = attributeId.Key;
                                    attribute.Locale = locale;

                                    Value value = new Value();
                                    value.AttrVal = reader[attributeId.Key.ToString()];
                                    value.Locale = locale;

                                    attribute.SetValueInvariant(value);
                                }

                                attributes.Add(attribute);
                            }
                        }

                        entity.Locale = locale;
                        entity.SetAttributes(attributes);
                        childEntities.Add(entity);
                    }

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Reading is completed and prepared collection of entity object also.", MDMTraceSource.HierarchyRelationshipGet);
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("HierarchyRelationshipDA.GetChildEntities", MDMTraceSource.HierarchyRelationshipGet);

                if (reader != null)
                    reader.Close();
            }

            return childEntities;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemMatrix"></param>
        /// <param name="definition"></param>
        /// <returns></returns>
        private String ConvertToItemMatrixXML(Table itemMatrix, EntityVariantDefinition definition)
        {
            String xml = "";
            xml += "<Items>";
            foreach (Row _row in itemMatrix.Rows)
            {
                xml += "<Item Id=\"" + _row.Id.ToString() + "\" Excluded=\"" + _row["Excluded"].ToString() + "\" Status=\"" + XmlSerializationHelper.XmlEncode(_row["Status"].ToString()) + "\">";
                foreach (EntityVariantLevel level in definition.EntityVariantLevels)
                {
                    foreach (EntityVariantRuleAttribute ruleAttr in level.RuleAttributes)
                    {
                        Column _column = new Column();
                        MDM.BusinessObjects.Attribute attribute = new MDM.BusinessObjects.Attribute();
                        attribute = (from dimAttr in level.DimensionAttributes
                                     where dimAttr.Id == ruleAttr.TargetAttributeId
                                     select dimAttr).FirstOrDefault();//getDimensionAttributeById(attr.Value,entityHierarchyLevel);

                        if (attribute == null)
                        {
                            if (ruleAttr.TargetAttributeId < 1)
                            {
                                throw new Exception("Can not find Dimension Attribute with Id=" + ruleAttr.TargetAttributeId);
                            }
                            else
                            {
                                attribute = new Attribute();
                                attribute.Id = ruleAttr.TargetAttributeId;
                                attribute.Name = ruleAttr.TargetAttributeName;
                                attribute.LongName = ruleAttr.TargetAttributeLongName;

                                level.DimensionAttributes.Add(attribute);
                            }
                        }

                        xml += "<Dimension Id=\"" + _row["Level" + level.Id.ToString() + "GroupId"] + "\" Level=\"" + level.Id + "\" AttributeId=\"" + attribute.Id.ToString() + "\" Val=\"" + _row[attribute.Id.ToString()].ToString() + "\" RefId=\"" + _row[attribute.Id + "RefId"].ToString() + "\"/>";
                    }
                }
                xml += "</Item>";
            }
            xml += "</Items>";
            return xml;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemMatrixXml"></param>
        /// <param name="entityHierarchyDefinition"></param>
        /// <returns></returns>
        private Table ParseDefinition(String itemMatrixXml, EntityVariantDefinition entityHierarchyDefinition)
        {
            Table ItemMatrix = new Table();

            foreach (EntityVariantLevel level in entityHierarchyDefinition.EntityVariantLevels)
            {
                Table dimensionValue = level.DimensionValues;

                foreach (EntityVariantRuleAttribute ruleAttr in level.RuleAttributes)
                {
                    Column _column = new Column();
                    MDM.BusinessObjects.Attribute attribute = new MDM.BusinessObjects.Attribute();
                    attribute = (from dimAttr in level.DimensionAttributes
                                 where dimAttr.Id == ruleAttr.TargetAttribute
                                 select dimAttr).FirstOrDefault();//getDimensionAttributeById(attr.Value,entityHierarchyLevel);

                    if (attribute == null)
                    {
                        throw new Exception("Can not find Dimension Attribute with Id=" + ruleAttr.TargetAttribute);
                    }

                    _column.Id = attribute.Id;
                    _column.Name = attribute.Id.ToString();
                    _column.LongName = attribute.LongName;
                    ItemMatrix.Columns.Add(_column);

                    Column _columnLookupRef = new Column();
                    _columnLookupRef.Name = attribute.Id.ToString() + "RefId";
                    _columnLookupRef.LongName = attribute.Id.ToString() + "RefId";
                    ItemMatrix.Columns.Add(_columnLookupRef);
                }
                Column column = new Column();
                column.Name = level.Id.ToString() + "GroupId";
                column.LongName = level.Id.ToString() + "GroupId";
                ItemMatrix.Columns.Add(column);

                Column columnlevel = new Column();
                columnlevel.Name = level.Id.ToString() + "LevelID";
                columnlevel.LongName = level.Id.ToString() + "LevelID";
                ItemMatrix.Columns.Add(columnlevel);
            }

            Column Excludedcolumn = new Column();
            Excludedcolumn.Name = "Excluded";
            Excludedcolumn.LongName = "Excluded";
            ItemMatrix.Columns.Add(Excludedcolumn);

            Column Statuscolumn = new Column();
            Statuscolumn.Name = "Status";
            Statuscolumn.LongName = "Status";
            ItemMatrix.Columns.Add(Statuscolumn);

            Column Idcolumn = new Column();
            Idcolumn.Name = "Id";
            Idcolumn.LongName = "Id";
            ItemMatrix.Columns.Add(Idcolumn);

            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(itemMatrixXml, XmlNodeType.Element, null);
                Row _row = null;
                String levelID = "";
                String AttributeID = "";
                while (!reader.EOF)
                {

                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Item")
                    {
                        #region Read Item Properties
                        _row = ItemMatrix.NewRow();
                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("Id"))
                            {
                                _row["Id"] = reader.ReadContentAsString();
                            }
                            if (reader.MoveToAttribute("Excluded"))
                            {
                                _row["Excluded"] = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("Status"))
                            {
                                _row["Status"] = reader.ReadContentAsString();
                            }
                        }
                        #endregion
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Dimension")
                    {
                        #region Read EntityHierarchyDimensions

                        if (reader.MoveToAttribute("Level"))
                        {
                            levelID = reader.ReadContentAsString();
                            //str += "ID" + reader.ReadContentAsString();
                            _row[levelID + "LevelID"] = levelID;
                        }
                        if (reader.MoveToAttribute("Id"))
                        {
                            _row[levelID + "GroupId"] = reader.ReadContentAsString();
                        }

                        if (reader.MoveToAttribute("AttributeId"))
                        {
                            AttributeID = reader.ReadContentAsString();
                        }
                        if (reader.MoveToAttribute("Val"))
                        {
                            _row[AttributeID] = reader.ReadContentAsString();
                        }
                        if (reader.MoveToAttribute("RefId"))
                        {
                            _row[AttributeID + "RefId"] = reader.ReadContentAsString();
                        }
                        #endregion
                    }
                    else
                    {
                        //Keep on reading the xml until we reach expected node.
                        reader.Read();
                    }
                }

                /*
                //foreach (Row row in ItemMatrix.Rows)
                //{
                foreach (EntityHierarchyLevel level in entityHierarchyDefinition.EntityHierarchyLevels)
                {
                    Table dimensionValue = level.DimensionValues;

                    foreach (KeyValuePair<MDM.BusinessObjects.Attribute, int> attr in level.RuleAttributes)
                    {
                        Column _column = new Column();
                        MDM.BusinessObjects.Attribute attribute = new MDM.BusinessObjects.Attribute();
                        attribute = (from dimAttr in level.DimensionAttributes
                                     where dimAttr.Id == attr.Value
                                     select dimAttr).FirstOrDefault();//getDimensionAttributeById(attr.Value,entityHierarchyLevel);

                        if (attribute == null)
                        {
                            throw new Exception("Can not find Dimension Attribute with Id=" + attr.Value);
                        }

                        foreach (Row row in ItemMatrix.Rows)
                        {
                            String GroupId = row[level.Id.ToString() + "GroupId"].ToString();
                            Row row_ = new Row();
                            row_ = (from lrow in dimensionValue.Rows
                                    where lrow["GroupId"].ToString() == GroupId
                                    select lrow).FirstOrDefault();
                            row[attribute.Id.ToString()] = row_[attribute.Id.ToString()];
                            row[attribute.Id.ToString() + "RefId"] = row_[attribute.Id.ToString() + "RefId"];
                        }
                    }
                }
                //}*/
            }
            catch
            {
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return ItemMatrix;
        }

        #endregion

        #endregion
    }
}