using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using System.Collections.ObjectModel;
using Microsoft.SqlServer.Server;

namespace MDM.EntityManager.Data
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects;
    using MDMBOW = MDM.BusinessObjects.Workflow;
    using MDM.BusinessObjects.Diagnostics;

    /// <summary>
    /// 
    /// </summary>
    public class EntityOperationsDA : SqlClientDataAccessBase
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region Entity CUD

        #endregion

        #region Get Entity

        /// <summary>
        /// It returns the Catalog, Organization, and the CNodes impacted by a change in a core attribute
        /// </summary>
        /// <param name="localeId">Indicates the locale Id </param>
        /// <param name="cnodeId">Indicates the Cnode Id </param>
        /// <param name="catalogId">Indicates the Catalog Id </param>
        /// <param name="attributeId">Indicates the Attribute Id </param>
        /// <param name="toTotalImpacted">Returns the Total Impacted Items </param>
        /// <param name="toTalAffected">Returns Total Affected Items </param>
        /// <param name="maxReturnCount">Maximum Return counts </param>
        /// <returns></returns>
        public string GetImpactedEntities(Int32 localeId, Int64 cnodeId, Int32 catalogId, Int32 attributeId, ref Int32 toTotalImpacted, ref Int32 toTalAffected, ref Int32 maxReturnCount)
        {
            StringBuilder impactedEntities = new StringBuilder();

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                parameters = generator.GetParameters("EntityManager_Entity_ImpactedEntities_Get_ParametersArray");

                parameters[0].Value = localeId;
                parameters[1].Value = cnodeId;
                parameters[2].Value = catalogId;
                parameters[3].Value = attributeId;

                storedProcedureName = "usp_EntityManager_Entity_ImpactedEntities_Get";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        impactedEntities.Append(reader[0]);
                    }
                }

                reader.NextResult();
                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        Int32.TryParse(reader["TotalImpacted"].ToString(), out toTotalImpacted);
                        Int32.TryParse(reader["TotalAffected"].ToString(), out toTalAffected);
                        Int32.TryParse(reader["MaxReturnCount"].ToString(), out maxReturnCount);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return impactedEntities.ToString();
        }

        #endregion

        #region MDL

        #region Get MDL

        public Collection<Entity> GetMDLsByIdList(String entityIdList, Int64 categoryId, Int32 containerId)
        {
            Collection<Entity> data = new Collection<Entity>();

            SqlDataReader reader = null;

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                parameters = generator.GetParameters("EntityManager_Entity_GetMDLsByIdList_ParametersArray");

                parameters[0].Value = entityIdList;
                parameters[1].Value = categoryId;
                parameters[2].Value = containerId;

                storedProcedureName = "usp_EntityManager_Entity_GetMDLsByIdList";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        Int64 entityId = -1;
                        String shortName = String.Empty;
                        String longName = String.Empty;

                        //Entity entity = new Entity(reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
                        //Temporary fix. Need to convert Entity.Id from Int32 to Int64.
                        //Till then read data as String and then try to parse it Int32

                        if (reader["PK_CNode"] != null)
                            Int64.TryParse(reader["PK_CNode"].ToString(), out entityId);

                        if (reader["ShortName"] != null)
                            shortName = reader["ShortName"].ToString();

                        if (reader["LongName"] != null)
                            longName = reader["LongName"].ToString();

                        Entity entity = new Entity(entityId, shortName, longName);
                        data.Add(entity);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return data;
        }

        /// <summary>
        /// Get the MDL Containers
        /// </summary>
        /// <param name="cnodeId">Indicates the Cnode Id of an Entity</param>
        /// <param name="catalogIDList">Indicates the Catalog List Id's of an Entity</param>
        /// <param name="delimiter">Indicates the delimiters</param>
        /// <param name="vchrUserLogin">Indicates the UserLogin Id of an Entity</param>
        /// <returns></returns>
        public String GetMDL(Int64 cnodeId, String catalogIDList, String delimiter, String vchrUserLogin, Boolean returnSelf)
        {
            StringBuilder containersDataXML = new StringBuilder();

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                parameters = generator.GetParameters("EntityManager_Entity_MDL_Get_ParametersArray");

                parameters[0].Value = cnodeId;
                parameters[1].Value = catalogIDList;
                parameters[2].Value = delimiter;
                parameters[3].Value = vchrUserLogin;
                parameters[4].Value = returnSelf;

                storedProcedureName = "usp_EntityManager_Entity_MDL_Get";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        containersDataXML.Append(reader[0]);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return containersDataXML.ToString();
        }

        public Collection<Entity> GetMDLBasedOnConfig(Int64 FK_CNode, String dataConfigXML, String catalogIDList, String delimiter, String vchrUserLogin, Int32 currentUserLocalId, Boolean returnSelf)
        {
            SqlDataReader reader = null;
            Collection<Entity> mdlDetails = new Collection<Entity>();

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                parameters = generator.GetParameters("EntityManager_Entity_MDL_GetBasedOnConfig_ParametersArray");

                parameters[0].Value = FK_CNode;
                parameters[1].Value = dataConfigXML;
                parameters[2].Value = catalogIDList;
                parameters[3].Value = delimiter;
                parameters[4].Value = vchrUserLogin;
                parameters[5].Value = currentUserLocalId;
                parameters[6].Value = returnSelf;

                storedProcedureName = "usp_EntityManager_Entity_MDL_GetBasedOnConfig";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    Entity container = new Entity();

                    int intId = -1;
                    if (reader["PK_MDL"] != null)
                        Int32.TryParse(reader["PK_MDL"].ToString(), out intId);
                    container.ExtensionUniqueId = intId;

                    intId = -1;
                    if (reader["PK_CNode"] != null)
                        Int32.TryParse(reader["PK_CNode"].ToString(), out intId);
                    container.Id = intId;

                    if (reader["CatalogName"] != null)
                        container.ContainerLongName = reader["CatalogName"].ToString();

                    if (reader["DisplayText"] != null)
                        container.EntityTypeName = reader["DisplayText"].ToString();

                    if (reader["ToolTipText"] != null)
                        container.EntityTypeLongName = reader["ToolTipText"].ToString();

                    intId = -1;
                    if (reader["ParentId"] != null)
                        Int32.TryParse(reader["ParentId"].ToString(), out intId);
                    container.ParentEntityId = intId;

                    Boolean expanded = false;
                    if (reader["Collapsed"] != null)
                        Boolean.TryParse(reader["Collapsed"].ToString(), out expanded);
                    container.ShowMaster = !expanded;

                    if (reader["Clickable"] != null)
                        container.SKU = reader["Clickable"].ToString();

                    mdlDetails.Add(container);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return mdlDetails;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="extSystemID">Indicates the system Id</param>
        /// <param name="inputXML">Indicates the Input xml</param>
        /// <param name="coreAttrList">Indicates the List of Core Attributes</param>
        /// <param name="techAttrList">Indicates the List of Technical Attributes</param>
        /// <param name="localeID">Indicates the Locale Id </param>
        /// <param name="programName">Indicates the Program Name</param>
        /// <returns></returns>
        public String GetMDLsAttributeValues(Int32 extSystemID, String inputXML, String coreAttrList, String techAttrList, Int32 localeID, String programName, Boolean includeInheritedValue, Boolean computeInheritedValues)
        {
            StringBuilder containersDataXML = new StringBuilder();

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                parameters = generator.GetParameters("EntityManager_Entity_GetMDLsAttributeValues_ParametersArray");

                parameters[0].Value = extSystemID;
                parameters[1].Value = inputXML;
                parameters[2].Value = coreAttrList;
                parameters[3].Value = techAttrList;
                parameters[4].Value = localeID;
                parameters[5].Value = programName;
                parameters[6].Value = includeInheritedValue;
                parameters[7].Value = computeInheritedValues;

                storedProcedureName = "usp_N_getCatalogByID_Local_XML";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        containersDataXML.Append(reader[0]);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return containersDataXML.ToString();
        }

        #endregion

        #region Process MDL

        /// <summary>
        /// Process of MDL
        /// </summary>
        /// <param name="mdlsXml">MDL xml Value</param>
        /// <param name="userLogin">User Login</param>
        public void ProcessMDL(String mdlsXml, String userLogin)
        {
            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                parameters = generator.GetParameters("EntityManager_Entity_MDL_Process_ParametersArray");

                parameters[0].Value = mdlsXml;
                parameters[1].Value = userLogin;

                storedProcedureName = "usp_EntityManager_Entity_MDL_Process";

                ExecuteProcedureNonQuery(connectionString, parameters, storedProcedureName);

            }
            finally
            {
            }
        }

        #endregion

        #endregion

        #region Get Core Attributes

        #endregion

        #region Get Parent

        /// <summary>
        /// Get Parent
        /// </summary>
        /// <param name="cnodeId">Indicates the Cnode Id of an Entity</param>
        /// <param name="cnodeParentId">Indicates the Cnode Parent Id of an Entity</param>
        /// <param name="catalogId">Indicates the Catalog Id of an Entity</param>
        /// <returns></returns>
        public String GetParent(Int64 cnodeId, Int64 cnodeParentId, Int32 catalogId, Boolean bitUseDraftTax, Int32 dataLocale)
        {

            StringBuilder parentDataXML = new StringBuilder();

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                parameters = generator.GetParameters("EntityManager_Entity_Parent_Get_ParametersArray");

                parameters[0].Value = cnodeId;
                parameters[1].Value = cnodeParentId;
                parameters[2].Value = catalogId;
                parameters[3].Value = bitUseDraftTax;
                parameters[4].Value = dataLocale;

                storedProcedureName = "usp_EntityManager_Entity_Parent_Get";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        parentDataXML.Append(reader[0]);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return parentDataXML.ToString();
        }

        #endregion

        #region Get NodeType

        /// <summary>
        /// Get child Node Type based on parentEntityId or ParentEntityTypeId
        /// </summary>
        /// <param name="parentEntityId">Specifies the id of parentEntity</param>
        /// <param name="parentEntityTypeId">Specifies the id of parentEntityType</param>
        /// <returns>Collection of EntityTypes</returns>
        public EntityTypeCollection GetEntityNodeType(Int64 parentEntityId, Int32 parentEntityTypeId, DBCommandProperties command)
        {
            SqlParameter[] parameters;
            SqlDataReader reader = null;
            String storedProcedureName = String.Empty;
            EntityTypeCollection entityTypeCollection = new EntityTypeCollection();

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                parameters = generator.GetParameters("EntityManager_EntityOperations_GetEntityNodeType_ParametersArray");

                parameters[0].Value = parentEntityId;
                parameters[1].Value = parentEntityTypeId;

                storedProcedureName = "usp_EntityManager_EntityOperations_GetChildEntities";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    Int32 id = 0;
                    String shortName = String.Empty;
                    String longname = String.Empty;

                    if (reader["PK_NodeType"] != null)
                        Int32.TryParse(reader["PK_NodeType"].ToString(), out id);
                    if (reader["ShortName"] != null)
                        shortName = reader["ShortName"].ToString();
                    if (reader["LongName"] != null)
                        longname = reader["LongName"].ToString();

                    EntityType entityType = new EntityType(id, shortName, longname);
                    entityTypeCollection.Add(entityType);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return entityTypeCollection;
        }

        #endregion

        #region Move Entity

        /// <summary>
        /// Move Catalog/Category/Entity node to a different Node
        /// </summary>
        /// <param name="fromCatalogId">Specifies from which Catalog,item Should be Moved</param>
        /// <param name="fromEntityId">Specifies from which Entity,item Should be Moved</param>
        /// <param name="toCatalogId">Specifies To which Catalog item Should be Moved</param>
        /// <param name="toEntityId">Specifies To which Entity item Should be Moved</param>
        /// <param name="loginUser">Specifies UserId</param>
        /// <param name="programName">Specifies Program Name</param>
        /// <param name="command">Specifies Command Properties</param>
        /// <returns>1 if Move is successful</returns>
        public Int32 Move(Int32 fromCatalogId, Int64 fromEntityId, Int32 toCatalogId, Int64 toEntityId, String loginUser, String programName, DBCommandProperties command)
        {
            SqlParameter[] parameters;
            Object objectValue = null;
            Int32 result = 0;
            String storedProcedureName = String.Empty;

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                try
                {
                    SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                    parameters = generator.GetParameters("EntityManager_EntityOperations_Move_ParametersArray");

                    parameters[0].Value = fromCatalogId;
                    parameters[1].Value = fromEntityId;
                    parameters[2].Value = toCatalogId;
                    parameters[3].Value = toEntityId;
                    parameters[4].Value = loginUser;
                    parameters[5].Value = programName;

                    storedProcedureName = "usp_EntityManager_Entity_Move";

                    objectValue = ExecuteProcedureScalar(command.ConnectionString, parameters, storedProcedureName);

                    if (objectValue != null)
                    {
                        result = ValueTypeHelper.Int32TryParse(objectValue.ToString(), 0);
                    }

                }
                finally
                {

                }

                transactionScope.Complete();
            }

            return result;
        }

        #endregion

        #region Get Affected Info

        public String GetAffectedInfo(Collection<Int64> entityIds, String attributeIdList, Int32 containerId, Int32 localeId, Int32 systemDataLocaleId)
        {
            String result = String.Empty;

            SqlDataReader reader = null;

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                parameters = generator.GetParameters("EntityManager_Entity_GeAffectedInfo_ParametersArray");

                #region Populate table value parameters

                List<SqlDataRecord> entityList = new List<SqlDataRecord>();
                SqlMetaData[] entityMetadata = generator.GetTableValueMetadata("EntityManager_Entity_GeAffectedInfo_ParametersArray", parameters[0].ParameterName);

                SqlDataRecord entityRecord = null;

                foreach (Int64 entityId in entityIds)
                {
                    entityRecord = new SqlDataRecord(entityMetadata);
                    entityRecord.SetValues(entityId);
                    entityList.Add(entityRecord);
                }

                #endregion

                parameters[0].Value = entityList;
                parameters[1].Value = attributeIdList;
                parameters[2].Value = containerId;
                parameters[3].Value = localeId;
                parameters[4].Value = systemDataLocaleId;

                storedProcedureName = "usp_EntityManager_Entity_AffectedInfo_Get";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        result = reader.GetString(0);
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

        #endregion

        #region Process Attribute Exception

        /// <summary>
        ///  Attribute Exception  process
        /// </summary>
        /// <param name="cNodeId">Indicates the Cnode Id </param>
        /// <param name="attributeId">Indicates the Attribute Id </param>
        /// <param name="srcCatalog">Indicates the source catalog</param>
        /// <param name="startDate">Indicates the Start Date</param>
        /// <param name="endDate">Indicates the End Date</param>
        /// <param name="userLogin">Indicates the User Login</param>
        public void ProcessAttributeException(Int64 cNodeId, Int32 attributeId, Int32 srcCatalog, DateTime startDate, DateTime endDate, String userLogin)
        {
            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                parameters = generator.GetParameters("EntityManager_Entity_AttributeException_Process_ParametersArray");

                parameters[0].Value = cNodeId;
                parameters[1].Value = attributeId;
                parameters[2].Value = srcCatalog;
                parameters[3].Value = startDate;
                parameters[4].Value = endDate;
                parameters[5].Value = userLogin;

                storedProcedureName = "usp_EntityManager_Entity_AttributeException_Process";

                ExecuteProcedureScalar(connectionString, parameters, storedProcedureName);
            }
            finally
            {
            }
        }

        #endregion

        #region Get Lookup Values

        /// <summary>
        /// Get Lookup Values
        /// </summary>
        /// <param name="cnodeId">Indicates the Cnode Id</param>
        /// <param name="attributeId">Indicates the Attribute Id</param>
        /// <param name="returnLookupColumns">Return Lookup Columns</param>
        /// <param name="delimiter">passed delimiter</param>
        /// <returns></returns>
        public String GetLookupValues(Int64 cnodeId, Int32 attributeId, String returnLookupColumns, String delimiter)
        {
            StringBuilder lookupValues = new StringBuilder();

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                parameters = generator.GetParameters("EntityManager_Entity_LookupValues_Get_ParametersArray");

                parameters[0].Value = cnodeId;
                parameters[1].Value = attributeId;
                parameters[2].Value = returnLookupColumns;
                parameters[3].Value = delimiter;

                storedProcedureName = "usp_EntityManager_Entity_LookupValues_Get";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        lookupValues.Append(reader[0]);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return lookupValues.ToString();
        }

        #endregion

        #region Workflow

        public String GetWorkflowStatus(String entityIDList, Int32 activityID, String workflowType, Int32 userID)
        {
            String data = String.Empty;

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                parameters = generator.GetParameters("EntityManager_Entity_GetWorkflowStatus");

                parameters[0].Value = entityIDList;
                parameters[1].Value = activityID;
                parameters[2].Value = workflowType;
                parameters[3].Value = userID;

                storedProcedureName = "usp_Entity_GetWorkflowStatus";

                Object value = ExecuteProcedureScalar(connectionString, parameters, storedProcedureName);

                if (value != null)
                {
                    data = value.ToString();
                }

            }
            finally
            {
            }
            return data;
        }

        public Collection<MDMBOW.WorkflowActivity> GetWorkflowTasks(Int64 cNodeID, Int32 userID)
        {
            SqlDataReader reader = null;
            MDMBOW.WorkflowActivity workflowActivity = null;
            Collection<MDMBOW.WorkflowActivity> workflowActivityCollection = new Collection<MDMBOW.WorkflowActivity>();

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            try
            {
                connectionString = AppConfigurationHelper.ConnectionString;

                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                parameters = generator.GetParameters("EntityManager_Entity_GetWorkflowTasks");

                parameters[0].Value = cNodeID;
                parameters[1].Value = userID;

                storedProcedureName = "usp_EntityManager_Entity_GetWorkflowTasks";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    Object[] values = new Object[reader.FieldCount];
                    reader.GetValues(values);
                    workflowActivity = new MDMBOW.WorkflowActivity();

                    Int32 id = 0;
                    if (values[0] != null)
                    {
                        Int32.TryParse(values[0].ToString(), out id);
                        workflowActivity.Id = id;
                    }

                    id = 0;
                    if (values[1] != null)
                    {
                        Int32.TryParse(values[1].ToString(), out id);
                        workflowActivity.WorkflowId = id;
                    }

                    id = 0;
                    if (values[2] != null)
                    {
                        Int32.TryParse(values[2].ToString(), out id);
                        workflowActivity.MetadataAttributeId = id;
                    }

                    if (values[3] != null)
                        workflowActivity.Name = values[3].ToString();

                    if (values[4] != null)
                        workflowActivity.LongName = values[4].ToString();

                    id = 0;
                    if (values[5] != null)
                    {
                        Int32.TryParse(values[5].ToString(), out id);
                        workflowActivity.NodeValue = id;
                    }

                    if (values[6] != null)
                        workflowActivity.ActingUser = values[6].ToString();

                    workflowActivityCollection.Add(workflowActivity);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return workflowActivityCollection;
        }

        #endregion

        #region Reclassify

        public DataTable Reclassify(String dataXml, String userLogin, String programName)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityManager.Data.EntityOperationsDA.Reclassify", false);

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;
            DataTable returnValue = new DataTable();

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                parameters = generator.GetParameters("EntityManager_Entity_Reclassify_ParametersArray");

                parameters[0].Value = dataXml;
                parameters[1].Value = userLogin;
                parameters[2].Value = programName;

                storedProcedureName = "usp_EntityManager_Entity_Reclassify";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                returnValue.Load(reader);

            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("EntityManager.Data.EntityOperationsDA.Reclassify");
            }
            return returnValue;
        }

        #endregion Reclassify

        #region Get Entity Types with Level

        /// <summary>
        /// Gets entity variants level based on given entity id
        /// </summary>
        /// <param name="entityId">Indicates entity id for which entity variant level to be fetched</param>
        /// <param name="command">Indicates DBCommand object</param>
        /// <returns></returns>
        public Dictionary<Int32, Int32> GetEntityVariantLevel(Int64 entityId, DBCommandProperties command)
        {
            Dictionary<Int32, Int32> entityTypeIdToVariantLevelMappings = null;
            SqlDataReader reader = null;

            #region Diagnostics & Tracing

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            var currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            Boolean isTracingEnabled = currentTraceSettings.IsBasicTracingEnabled;

            if (isTracingEnabled)
            {
                diagnosticActivity.Start();
                diagnosticActivity.LogInformation("GetEntityVariantLevel is started.");
            }

            #endregion Diagnostics & Tracing

            try
            {
                #region Initial Setup

                String storedProcedureName = "usp_EntityManager_EntityVariantDefinition_Get";
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");
                SqlParameter[] parameters = generator.GetParameters("EntityManager_EntityVariantDefinition_Get_ParametersArray");

                #endregion Initial Setup

                #region Populate table value parameters for entityIds

                SqlMetaData[] sqlLocalesMetadata = generator.GetTableValueMetadata("EntityManager_EntityVariantDefinition_Get_ParametersArray", parameters[0].ParameterName);
                List<SqlDataRecord> entityIdRecordList = EntityDataReaderUtility.CreateEntityIdTable(new Collection<Int64>() { entityId }, sqlLocalesMetadata);

                #endregion Populate table value parameters for entityIds

                parameters[0].Value = entityIdRecordList;

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo(String.Format("Execution for procedure : {0} is completed", storedProcedureName));
                }

                entityTypeIdToVariantLevelMappings = PopulateEntityVariantLevel(reader);

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("GetEntityVariantLevel reading is completed.");
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("GetEntityVariantLevel loading is completed.");
                    diagnosticActivity.Stop();
                }
            }

            return entityTypeIdToVariantLevelMappings;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private Dictionary<Int32, Int32> PopulateEntityVariantLevel(SqlDataReader reader)
        {
            Dictionary<Int32, Int32> entityTypeIdToVariantLevelMappings = new Dictionary<Int32, Int32>();

            if (reader != null)
            {
                while (reader.Read())
                {
                    Int32 variantLevel = 0;
                    Int32 entityTypeId = 0;

                    if (reader["VariantLevel"] != null)
                    {
                        variantLevel = ValueTypeHelper.Int32TryParse(reader["VariantLevel"].ToString(), variantLevel);
                    }
                    if (reader["EntityTypeId"] != null)
                    {
                        entityTypeId = ValueTypeHelper.Int32TryParse(reader["EntityTypeId"].ToString(), entityTypeId);
                    }

                    if (entityTypeId > 0 && !entityTypeIdToVariantLevelMappings.ContainsKey(entityTypeId))
                    {
                        entityTypeIdToVariantLevelMappings.Add(entityTypeId, variantLevel);
                    }
                }
            }

            return entityTypeIdToVariantLevelMappings;
        }

        #endregion

        #endregion
    }
}