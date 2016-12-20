using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;

namespace MDM.EntityManager.Data
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Diagnostics;

    /// <summary>
    /// Specifies data access operations for extension relationships
    /// </summary>
    public class ExtensionRelationshipDA : SqlClientDataAccessBase
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
        /// Gets extension relationship collections for requested entity ids
        /// </summary>
        /// <param name="entityIds">Entity Ids for which extension relationships needs to be fetched</param>
        /// <param name="command">Object having command properties</param>
        /// <returns>Dictionary having entity ids with their respective extension relationships</returns>
        public ExtensionRelationshipCollection Get(Int64 entityId, LocaleEnum locale, DBCommandProperties command)
        {
            SqlDataReader reader = null;
            ExtensionRelationshipCollection extensionRelationships = null;

            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                parameters = generator.GetParameters("EntityManager_ExtensionRelationship_Get_ParametersArray");

                parameters[0].Value = entityId;
                parameters[1].Value = (Int32)locale;

                storedProcedureName = "usp_EntityManager_ExtensionRelationship_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    extensionRelationships = PopulateEntityExtensionRelationships(reader);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return extensionRelationships;
        }

        /// <summary>
        /// Changes parent of the extension and its child during reverse MDL 
        /// </summary>
        /// <param name="fromEntityId">Indicates parent extension Id as per INH path</param>
        /// <param name="toEntityId">Indicates child extension Id as per INH path</param>
        /// <param name="command">Object having command properties</param>
        /// <returns>Returns true if extension ReParent is successful, otherwise false.</returns>
        public Boolean ReParent(Int64 fromEntityId, Int64 toEntityId, DBCommandProperties command)
        {
            Boolean isSuccess = false;
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("ExtensionRelationshipDA.ReParent", false);

            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;
            Int32 affectedRows = 0;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                parameters = generator.GetParameters("EntityManager_ExtensionRelationship_ReParent_ParametersArray");

                parameters[0].Value = fromEntityId;
                parameters[1].Value = toEntityId;

                storedProcedureName = "usp_EntityManager_ExtensionRelationship_ReParent";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    affectedRows = PopulateResult(reader);
                }

                if (affectedRows > 0)
                {
                    isSuccess = true;
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ExtensionRelationshipDA.Reparent");
            }

            return isSuccess;
        }

        /// <summary>
        /// Get the extension count dictionary based on the parent extension entity ids.
        /// </summary>
        /// <param name="entityIds">Indicates the parent extension entity identifiers.</param>
        /// <param name="command">Indicates the object having command properties.</param>
        /// <returns>Return the extension count dictionary based on the parent extension entity ids.</returns>
        public Dictionary<Int64, Dictionary<Int32, Int32>> GetExtensionsRelationshipsCount(Collection<Int64> entityIds, DBCommandProperties command)
        {
            Dictionary<Int64, Dictionary<Int32, Int32>> extensionEntitiesCountDictionary = new Dictionary<Int64, Dictionary<Int32, Int32>>();

            var currentSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            var currentActivity = new DiagnosticActivity();

            if (currentSettings != null && currentSettings.IsBasicTracingEnabled)
            {
                currentActivity.Start();
            }

            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                parameters = generator.GetParameters("EntityManager_EntityExtension_Count_Get_ParametersArray");

                #region Populate Entity table value parameters

                List<SqlDataRecord> entityTable = null;

                if (entityIds != null && entityIds.Count > 0)
                {
                    entityTable = new List<SqlDataRecord>();
                    SqlMetaData[] entityMetadata = generator.GetTableValueMetadata("EntityManager_EntityExtension_Count_Get_ParametersArray", parameters[0].ParameterName);

                    SqlDataRecord entityRecord = null;

                    foreach (Int64 entityId in entityIds)
                    {
                        entityRecord = new SqlDataRecord(entityMetadata);
                        entityRecord.SetValues(entityId);
                        entityTable.Add(entityRecord);
                    }
                }

                #endregion

                parameters[0].Value = entityTable;

                storedProcedureName = "usp_EntityManager_EntityExtension_Count_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    extensionEntitiesCountDictionary = PopulateExtensionEntitiesDictionary(reader); 
                }

                if (currentSettings.IsBasicTracingEnabled)
                {
                    currentActivity.LogInformation(String.Format("Fetched the dimension values using SP : {0}", storedProcedureName));
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                if (currentSettings != null && currentSettings.IsBasicTracingEnabled)
                {
                    currentActivity.Stop();
                }
            }

            return extensionEntitiesCountDictionary;
        }

        #endregion

        #region Private Methods

        private Dictionary<Int64, Dictionary<Int32, Int32>> PopulateExtensionEntitiesDictionary(SqlDataReader reader)
        {
            Dictionary<Int64, Dictionary<Int32, Int32>> extensionEntitiesCountDictionary = new Dictionary<Int64, Dictionary<Int32, Int32>>();

            while (reader.Read())
            {
                Int64 parentEntityExtensionId = 0;
                Int32 containerId = 0;
                Int32 entityTypeId = 0;
                Int32 updatedCount = 0;
                Int32 key = 0;

                if (reader["ExtensionParentEntityId"] != null)
                {
                    parentEntityExtensionId = ValueTypeHelper.Int64TryParse(reader["ExtensionParentEntityId"].ToString(), 0);
                }

                if (reader["ContainerId"] != null)
                {
                    containerId = ValueTypeHelper.Int32TryParse(reader["ContainerId"].ToString(), 0);
                }

                if (reader["EntityTypeId"] != null)
                {
                    entityTypeId = ValueTypeHelper.Int32TryParse(reader["EntityTypeId"].ToString(), 0);
                }

                if (reader["EntityExtensionCount"] != null)
                {
                    updatedCount = ValueTypeHelper.Int32TryParse(reader["EntityExtensionCount"].ToString(), 0);
                }

                key = Utility.GetInternalUniqueKeyBasedOnParam(containerId, entityTypeId);

                Dictionary<Int32, Int32> contextBasedCountDictionary = new Dictionary<Int32, Int32>();

                extensionEntitiesCountDictionary.TryGetValue(parentEntityExtensionId, out contextBasedCountDictionary);

                Int32 count = 0;

                if (contextBasedCountDictionary == null)
                {
                    contextBasedCountDictionary = new Dictionary<Int32, Int32>();
                }

                //If the dictionary already contains the context key(containerid + entity type id), then add the count value in dictionary with 
                //current reader row extension count value
                if(contextBasedCountDictionary.Count > 0)
                {
                    contextBasedCountDictionary.TryGetValue(key, out count);

                    if (count > 0)
                    {
                        updatedCount = updatedCount + count;
                        contextBasedCountDictionary[key] = updatedCount;
                    }
                }
                    //If it does not have dictinary with specific context key, then add a new dictionary.
                else 
                {
                    contextBasedCountDictionary.Add(key, updatedCount);
                }

                if (extensionEntitiesCountDictionary.ContainsKey(parentEntityExtensionId))
                {
                    extensionEntitiesCountDictionary[parentEntityExtensionId] = contextBasedCountDictionary;
                }
                else
                {
                    extensionEntitiesCountDictionary.Add(parentEntityExtensionId, contextBasedCountDictionary);
                }
            }

            return extensionEntitiesCountDictionary;
        }

        private Int32 PopulateResult(SqlDataReader reader)
        {
            Int32 affectedRows = 0;

            while (reader.Read())
            {
                if (reader["Result"] != null)
                {
                    affectedRows = ValueTypeHelper.Int32TryParse(reader["Result"].ToString(), 0);
                }
            }

            return affectedRows;
        }

        private ExtensionRelationshipCollection PopulateEntityExtensionRelationships(SqlDataReader reader)
        {
            ExtensionRelationship extensionRelationship = null;
            ExtensionRelationshipCollection extensionRelationships = new ExtensionRelationshipCollection();

            while (reader.Read())
            {
                Int64 entityId = 0;
                Int32 id = 0;
                Int64 relatedEntityId = 0;
                String type = String.Empty;
                Int32 containerId = 0;
                String containerName = String.Empty;
                String containerLongName = String.Empty;
                Int64 categoryId = 0;
                String categoryName = String.Empty;
                String categoryLongName = String.Empty;
                RelationshipDirection direction = RelationshipDirection.None;
                String path = String.Empty;
                String categoryLongNamePath = String.Empty;
                String categoryPath = String.Empty;
                String relatedEntityName = String.Empty;
                Int64 parentExtensionEntityId = 0;

                if (reader["EntityId"] != null)
                    Int64.TryParse(reader["EntityId"].ToString(), out entityId);

                if (reader["Id"] != null)
                    Int32.TryParse(reader["Id"].ToString(), out id);

                if (reader["RelatedEntityId"] != null)
                    Int64.TryParse(reader["RelatedEntityId"].ToString(), out relatedEntityId);

                if (reader["RelatedEntityName"] != null)
                    relatedEntityName = reader["RelatedEntityName"].ToString();

                if (reader["Type"] != null)
                    type = reader["Type"].ToString();

                if (reader["ContainerId"] != null)
                    Int32.TryParse(reader["ContainerId"].ToString(), out containerId);

                if (reader["ContainerName"] != null)
                    containerName = reader["ContainerName"].ToString();

                if (reader["ContainerLongName"] != null)
                    containerLongName = reader["ContainerLongName"].ToString();

                if (reader["CategoryId"] != null)
                    Int64.TryParse(reader["CategoryId"].ToString(), out categoryId);

                if (reader["CategoryName"] != null)
                    categoryName = reader["CategoryName"].ToString();

                if (reader["CategoryLongName"] != null)
                    categoryLongName = reader["CategoryLongName"].ToString();

                if (reader["CategoryLongNamePath"] != null)
                    categoryLongNamePath = reader["CategoryLongNamePath"].ToString();

                if (reader["CategoryPath"] != null)
                    categoryPath = reader["CategoryPath"].ToString();

                if (reader["Direction"] != null)
                {
                    String strDirection = reader["Direction"].ToString();

                    if (!String.IsNullOrWhiteSpace(strDirection))
                    {
                        Enum.TryParse<RelationshipDirection>(strDirection, out direction);
                    }
                }

                if (reader["ParentExtensionEntityId"] != null)
                    parentExtensionEntityId = ValueTypeHelper.Int64TryParse(reader["ParentExtensionEntityId"].ToString(), parentExtensionEntityId);

                extensionRelationship = new ExtensionRelationship(id, type, relatedEntityId, containerId, containerName, containerLongName, categoryId, categoryName, categoryLongName, direction, path, categoryPath, categoryLongNamePath);
                extensionRelationship.Name = relatedEntityName;
                extensionRelationship.ParentExtensionEntityId = parentExtensionEntityId;

                extensionRelationships.Add(extensionRelationship);
            }

            return extensionRelationships;
        }

        #endregion

        #endregion
    }
}
