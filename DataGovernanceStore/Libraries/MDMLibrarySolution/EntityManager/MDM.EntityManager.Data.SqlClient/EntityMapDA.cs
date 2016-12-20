using System;
using System.Data;
using Microsoft.SqlServer.Server;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Transactions;
using System.Diagnostics;

namespace MDM.EntityManager.Data
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Imports;

    /// <summary>
    /// Specifies data access operations for Entity Maps
    /// </summary>
    public class EntityMapDA : SqlClientDataAccessBase
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
        /// Gets entity mappings for requested external Ids
        /// </summary>
        /// <param name="entityMapCollection">Collection of entity maps for which internals needs to be loaded</param>
        /// <param name="entityIdentificationMappingXml">Entity Identification Mappings in XML format</param>
        /// <param name="command">Object having command properties</param>
        /// <returns>Result of the operation saying whether it is successful or not</returns>
        public Boolean LoadInternalDetails(EntityMapCollection entityMapCollection, EntityIdentificationMap entityIdentificationMap, DBCommandProperties command)
        {
            Boolean result = false;
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                parameters = generator.GetParameters("EntityManager_EntityMap_Get_ParametersArray");
                SqlMetaData[] entityIdentificationMapMetadata = generator.GetTableValueMetadata("EntityManager_EntityMap_Get_ParametersArray", parameters[1].ParameterName);

                #region Populate table value parameters

                #region Populate EntityMap TVP

                List<SqlDataRecord> entityMapList = new List<SqlDataRecord>();
                SqlMetaData[] entityMapMetadata = generator.GetTableValueMetadata("EntityManager_EntityMap_Get_ParametersArray", parameters[0].ParameterName);

                SqlDataRecord entityMapRecord = null;

                foreach (EntityMap entityMap in entityMapCollection)
                {
                    entityMapRecord = new SqlDataRecord(entityMapMetadata);

                    Int32? contId = (entityMap.ContainerId == 0) ? null : (Int32?)entityMap.ContainerId;
                    Int64? catId = (entityMap.CategoryId == 0) ? null : (Int64?)entityMap.CategoryId;
                    Int32? etId = (entityMap.EntityTypeId == 0) ? null : (Int32?)entityMap.EntityTypeId;

                    String externalID = (String.IsNullOrEmpty(entityMap.ExternalId)) ? entityMap.CustomData : entityMap.ExternalId;

                    entityMapRecord.SetValues(entityMap.Id, entityMap.SystemId, externalID, contId, catId, etId);
                    entityMapList.Add(entityMapRecord);
                }

                #endregion

                #region Populate EntityIdentificationMap TVP

                List<SqlDataRecord> entityIdentificationMapList = CreateEntityIdentificationMapTable(entityIdentificationMap, entityIdentificationMapMetadata);

                #endregion

                #endregion

                parameters[0].Value = entityMapList;
                parameters[1].Value = entityIdentificationMapList;

                storedProcedureName = "usp_EntityManager_EntityMap_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    result = UpdateEntityMapCollection(reader, entityMapCollection);
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
        /// Gets entity map for requested external Id or custom data
        /// </summary>
        /// <param name="systemId">External system Id</param>
        /// <param name="externalId">Entity external Id</param>
        /// <param name="containerId">Container Id of the entity</param>
        /// <param name="entityTypeId">Entity Type Id of the entity</param>
        /// <param name="categoryId">Category Id of the entity</param>
        /// <param name="entityIdentificationMap">Entity Identification Mappings</param>
        /// <param name="application">Name of application which is performing action</param>
        /// <param name="module">Name of module which is performing action</param>
        /// <returns>Entity Map object</returns>
        /// <exception cref="ArgumentException">Thrown when either 'systemId' or 'externalId' or 'containerId' or 'categoryId' is not available</exception>
        public EntityMapCollection Get(String systemId, String externalId, Int32? containerId, Int64? categoryId, Int32? entityTypeId, EntityIdentificationMap entityIdentificationMap, DBCommandProperties command)
        {
            return Get(systemId, externalId, containerId, categoryId, entityTypeId, String.Empty, entityIdentificationMap, command);
        }

        /// <summary>
        /// Get All EntityMap for given ExternalId in entityMapCollection
        /// </summary>
        /// <param name="entityMapCollection">Collection of entity maps for which EntityMap needs to be loaded</param>
        /// <param name="entityIdentificationMappingXml">Entity Identification Mappings in XML format</param>
        /// <param name="command">Object having command properties</param>
        /// <returns>Collection of EntityMap</returns>
        public EntityMapCollection Get(String systemId, String externalId, Int32? containerId, Int64? categoryId, Int32? entityTypeId, String customData, EntityIdentificationMap entityIdentificationMap, DBCommandProperties command)
        {
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            EntityMapCollection entityMaps = null;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                Int64 entityMapRefId = -1;

                parameters = generator.GetParameters("EntityManager_EntityMap_Get_ParametersArray");
                SqlMetaData[] entityIdentificationMapMetadata = generator.GetTableValueMetadata("EntityManager_EntityMap_Get_ParametersArray", parameters[1].ParameterName);

                #region Populate table value parameters

                #region Populate EntityMap TVP

                List<SqlDataRecord> entityMapList = new List<SqlDataRecord>();
                SqlMetaData[] entityMapMetadata = generator.GetTableValueMetadata("EntityManager_EntityMap_Get_ParametersArray", parameters[0].ParameterName);

                Int32? contId = (containerId == 0) ? null : (Int32?)containerId;
                Int64? catId = (categoryId == 0) ? null : (Int64?)categoryId;
                Int32? etId = (entityTypeId == 0) ? null : (Int32?)entityTypeId;

                SqlDataRecord entityMapRecord = new SqlDataRecord(entityMapMetadata);
                externalId = (String.IsNullOrEmpty(externalId)) ? customData : externalId;

                entityMapRecord.SetValues(entityMapRefId, systemId, externalId, contId, catId, etId);
                entityMapList.Add(entityMapRecord);

                #endregion

                #region Populate EntityIdentificationMap TVP

                List<SqlDataRecord> entityIdentificationMapList = CreateEntityIdentificationMapTable(entityIdentificationMap, entityIdentificationMapMetadata);

                #endregion

                #endregion

                parameters[0].Value = entityMapList;
                parameters[1].Value = entityIdentificationMapList;

                storedProcedureName = "usp_EntityManager_EntityMap_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    entityMaps = PopulateEntityMapCollection(reader);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return entityMaps;
        }

        #endregion

        #region Private Methods

        private EntityMapCollection PopulateEntityMapCollection(SqlDataReader reader)
        {
            EntityMap entityMap = null;
            EntityMapCollection entityMapCollection = new EntityMapCollection();

            Int64 entityMapRefId = -1;

            while (reader.Read())
            {
                String systemId = String.Empty;
                Int32 objectTypeId = 0;
                String objectType = String.Empty;
                String externalId = String.Empty;
                Int64 internalId = 0;
                Int32 containerId = 0;
                Int64 categoryId = 0;
                Int32 entityTypeId = 0;
                Int64 parentExtensionEntityId = 0;

                if (reader["SystemId"] != null)
                    systemId = reader["SystemId"].ToString();

                if (reader["ObjectTypeId"] != null)
                    Int32.TryParse(reader["ObjectTypeId"].ToString(), out objectTypeId);

                if (reader["ObjectType"] != null)
                    objectType = reader["ObjectType"].ToString();

                if (reader["ExternalId"] != null)
                    externalId = reader["ExternalId"].ToString();

                if (reader["InternalId"] != null)
                    Int64.TryParse(reader["InternalId"].ToString(), out internalId);

                if (reader["ContainerId"] != null)
                    Int32.TryParse(reader["ContainerId"].ToString(), out containerId);

                if (reader["CategoryId"] != null)
                    Int64.TryParse(reader["CategoryId"].ToString(), out categoryId);

                if (reader["EntityTypeId"] != null)
                    Int32.TryParse(reader["EntityTypeId"].ToString(), out entityTypeId);

                if (reader["ParentExtensionEntityId"] != null)
                     parentExtensionEntityId = ValueTypeHelper.Int64TryParse(reader["ParentExtensionEntityId"].ToString() , parentExtensionEntityId);

                entityMap = new EntityMap(entityMapRefId--, systemId, objectTypeId, objectType, externalId, internalId, containerId, categoryId, entityTypeId, parentExtensionEntityId);

                entityMapCollection.Add(entityMap);
            }

            return entityMapCollection;
        }

        private Boolean UpdateEntityMapCollection(SqlDataReader reader, EntityMapCollection entityMapCollection)
        {
            Boolean result = false;
            Boolean hasError = false;

            while (reader.Read())
            {
                Int64 refId = -1;

                String systemId = String.Empty;
                Int32 objectTypeId = 0;
                String objectType = String.Empty;
                String externalId = String.Empty;
                Int64 internalId = 0;
                Int32 containerId = 0;
                Int64 categoryId = 0;
                Int32 entityTypeId = 0;
                Int64 parentExtensionEntityId = 0;
                Int64 entityFamilyId = 0;
                Int64 entityGlobalFamilyId = 0;

                if (reader["RefId"] != null)
                {
                    refId = ValueTypeHelper.Int64TryParse(reader["RefId"].ToString(), refId);
                }

                if (reader["SystemId"] != null)
                {
                    systemId = reader["SystemId"].ToString();
                }

                if (reader["ObjectTypeId"] != null)
                {
                    objectTypeId = ValueTypeHelper.Int32TryParse(reader["ObjectTypeId"].ToString(), objectTypeId);
                }

                if (reader["ObjectType"] != null)
                {
                    objectType = reader["ObjectType"].ToString();
                }

                if (reader["ExternalId"] != null)
                {
                    externalId = reader["ExternalId"].ToString();
                }

                if (reader["InternalId"] != null)
                {
                    internalId = ValueTypeHelper.Int64TryParse(reader["InternalId"].ToString(), internalId);
                }

                if (reader["ContainerId"] != null)
                {
                    containerId = ValueTypeHelper.Int32TryParse(reader["ContainerId"].ToString(), containerId);
                }

                if (reader["CategoryId"] != null)
                {
                    categoryId = ValueTypeHelper.Int64TryParse(reader["CategoryId"].ToString(), categoryId);
                }

                if (reader["EntityTypeId"] != null)
                {
                    entityTypeId = ValueTypeHelper.Int32TryParse(reader["EntityTypeId"].ToString(), entityTypeId);
                }

                if (reader["ParentExtensionEntityId"] != null)
                {
                    parentExtensionEntityId = ValueTypeHelper.Int64TryParse(reader["ParentExtensionEntityId"].ToString(), parentExtensionEntityId);
                }

                if (reader["EntityFamilyId"] != null)
                {
                    entityFamilyId = ValueTypeHelper.Int64TryParse(reader["EntityFamilyId"].ToString(), entityFamilyId);
                }

                if (reader["EntityGlobalFamilyId"] != null)
                {
                    entityGlobalFamilyId = ValueTypeHelper.Int64TryParse(reader["EntityGlobalFamilyId"].ToString(), entityGlobalFamilyId);
                }

                // A given reference might return one or more entity maps..
                EntityMapCollection referenceMapCollection = (EntityMapCollection)entityMapCollection.GetEntityMapCollection(refId);

                //EntityMap entityMap = entityMapCollection.SingleOrDefault(em => em.SystemId == systemId && em.ExternalId == externalId && em.ContainerId == containerId && em.CategoryId == categoryId && em.EntityTypeId == entityTypeId);

                // we need to update each one and then return..
                if (referenceMapCollection != null)
                {
                    foreach (EntityMap entityMap in referenceMapCollection)
                    {
                        entityMap.ExternalId = externalId;
                        entityMap.ObjectTypeId = objectTypeId;
                        entityMap.ObjectType = objectType;
                        entityMap.EntityTypeId = entityTypeId;
                        entityMap.ContainerId = containerId;
                        entityMap.CategoryId = categoryId;
                        entityMap.ParentExtensionEntityId = parentExtensionEntityId;
                        entityMap.EntityFamilyId = entityFamilyId;
                        entityMap.EntityGlobalFamilyId = entityGlobalFamilyId;

                        //In the case of multiple entities found in same container and different category. then entity map internal id set as -100.
                        //This will avoid wrong entity map identification.
                        if (entityMap.InternalId > 0 || entityMap.InternalId == -100)
                        {
                            entityMap.InternalId = -100;

                            if (Constants.TRACING_ENABLED)
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Warning, String.Format("Multiple entities found in container '{0}'. Entity refid '{1}' and category id: '{2}'", containerId, refId, categoryId));
                        }
                        else
                        {
                            entityMap.InternalId = internalId;
                        }
                    }

                    if (!hasError)
                        result = true;
                }
                else
                {
                    hasError = true;
                    result = false;
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "Failed to find entity map object for reference Id:" + refId);
                    //TODO:: Failed to find entity map object which is a rare case.. Need to think about what action to take when it happens
                }
            }

            return result;
        }

        private List<SqlDataRecord> CreateEntityIdentificationMapTable(EntityIdentificationMap entityIdentificationMap, SqlMetaData[] entityIdentificationMapMetaData)
        {
            List<SqlDataRecord> entityIdentificationMapList = null;

            if (entityIdentificationMap != null && entityIdentificationMap.Mappings != null && entityIdentificationMap.Mappings.Count > 0)
            {
                entityIdentificationMapList = new List<SqlDataRecord>();

                foreach (Mapping mapping in entityIdentificationMap.Mappings)
                {
                    SqlDataRecord entityIdentificationMapRecord = new SqlDataRecord(entityIdentificationMapMetaData);

                    entityIdentificationMapRecord.SetValue(0, mapping.SourceType.ToString());
                    entityIdentificationMapRecord.SetValue(1, mapping.SourceName);
                    entityIdentificationMapRecord.SetValue(2, mapping.TargetType.ToString());
                    entityIdentificationMapRecord.SetValue(3, mapping.TargetName);

                    entityIdentificationMapList.Add(entityIdentificationMapRecord);
                }
            }

            return entityIdentificationMapList;
        }

        #endregion

        #endregion
    }
}