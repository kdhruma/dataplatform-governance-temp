using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;

namespace MDM.RelationshipManager.Data
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects;

    /// <summary>
    /// Specifies data access operations for Relationship Denorm
    /// </summary>
    public class RelationshipDenormDA : SqlClientDataAccessBase
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructors

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gets entities denormalized relationships
        /// </summary>
        /// <param name="entityContainerIds">Entity Ids and Container Ids dictionary for which denormalized relationships are required</param>
        /// <param name="relationshipId">Relationship Id for which denormalized relationships are required</param>
        /// <param name="relationshipTypeId">Relationship Type Id for which denormalized relationships are required</param>
        /// <param name="getWhereUsed">Flag denoting whether to consider where used relationships in this get operation</param>
        /// <param name="getImpactedExtensions">Flag denoting whether to consider impacted extension relationships in this get operation</param>
        /// <param name="getImpactedHierarchies">Flag denoting whether to consider impacted hierarchies relationships in this get operation</param>
        /// <param name="getRelationshipTree">Flag denoting whether to consider relationships tree in this get operation</param>
        /// <param name="getInheritable">Flag denoting whether to consider inheritable relationships in this get operation</param>
        /// <param name="loadRelationshipAttributes">Flag denoting whether to load relationship attributes for denormalized relationships</param>
        /// <param name="relationshipAttributeModels">Relationship attribute models</param>
        /// <param name="command"></param>
        /// <param name="callerContext">Indicates Application and Module name by which operation is being performed</param>
        /// <returns>Denormalized relationships</returns>
        public RelationshipCollection GetEntitiesDenormalizedRelationships(Dictionary<Int64, Int32> entityContainerIds, Int64 relationshipId, Int32 relationshipTypeId, Boolean getWhereUsed, Boolean getImpactedExtensions, Boolean getImpactedHierarchies, Boolean getRelationshipTree, Boolean getInheritable, Boolean loadRelationshipAttributes, AttributeModelCollection relationshipAttributeModels, DBCommandProperties command, CallerContext callerContext)
        {
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            RelationshipCollection denormalizedRelationships = null;

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("RelationshipManager.RelationshipDenormDA.GetEntitiesDenormalizedRelationships", MDMTraceSource.RelationshipGet, false);
            }

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("RelationshipManager_SqlParameters");

                parameters = generator.GetParameters("RelationshipManager_DenormalizedRelationship_Get_ParametersArray");

                #region Populate table value parameters for entity tree list table

                List<SqlDataRecord> entityTreeListTable = null;

                SqlMetaData[] entityTreeListMetaData = generator.GetTableValueMetadata("RelationshipManager_DenormalizedRelationship_Get_ParametersArray", parameters[0].ParameterName);

                if (entityContainerIds != null && entityContainerIds.Count > 0)
                {
                    entityTreeListTable = new List<SqlDataRecord>();

                    foreach (KeyValuePair<Int64, Int32> entityContainerIdPair in entityContainerIds)
                    {
                        SqlDataRecord entitytreeListRecord = new SqlDataRecord(entityTreeListMetaData);

                        entitytreeListRecord.SetValue(0, entityContainerIdPair.Key);
                        entitytreeListRecord.SetValue(1, entityContainerIdPair.Value);
                        entitytreeListRecord.SetValue(2, String.Empty);

                        if (relationshipId > 0)
                        {
                            entitytreeListRecord.SetValue(3, relationshipId.ToString());
                        }
                        else
                        {
                            entitytreeListRecord.SetValue(3, String.Empty);
                        }

                        entityTreeListTable.Add(entitytreeListRecord);
                    }
                }

                #endregion 

                parameters[0].Value = entityTreeListTable;
                parameters[1].Value = relationshipTypeId;
                parameters[2].Value = getWhereUsed;
                parameters[3].Value = getImpactedHierarchies;
                parameters[4].Value = getImpactedExtensions;
                parameters[5].Value = getRelationshipTree;
                parameters[6].Value = getInheritable;
                parameters[7].Value = loadRelationshipAttributes;

                storedProcedureName = "usp_RelationshipManager_DenormalizedRelationship_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                denormalizedRelationships = EntityDataReaderUtility.ReadRelationships(reader, null, relationshipAttributeModels, callerContext.Module);
            }
            finally
            {
                // Close the connection...
                if (reader != null)
                    reader.Close();

                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("RelationshipManager.RelationshipDenormDA.GetEntitiesDenormalizedRelationships", MDMTraceSource.RelationshipGet);
                }
            }

            return denormalizedRelationships;
        }

        /// <summary>
        /// Indicates whether De-normalized relationships are dirty or not.The return value indicates whether inherited relationships are in denorm process or not.
        /// </summary>
        /// <param name="entityId">Indicates entity id.</param>
        /// <param name="containerId">Indicate container id.</param>
        /// <param name="relationshipTypeId">Indicates relationship type id.
        /// If relationship type id is 0 means All. It means get the status from all relationship types else requested relationship type.</param>
        /// <param name="entityTreeIdList">Indicates entity tree id list.</param>
        /// <param name="command">Connection string and other command related property</param>
        /// <returns>true if the De-normalized/Inherited relationships are in denorm progress; otherwise, false.</returns>
        public Boolean IsDenormalizedRelationshipsDirty(Int64 entityId, Int32 containerId,Int32 relationshipTypeId, String entityTreeIdList, DBCommandProperties command)
        {
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            Boolean isDenormalizedRelationshipsProcessRequired =  false;
            List<SqlDataRecord> entityIdList = null;

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("RelationshipManager.RelationshipDenormDA.IsDenormalizedRelationshipsDirty", MDMTraceSource.RelationshipGet, false);
            }

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("RelationshipManager_SqlParameters");

                parameters = generator.GetParameters("RelationshipManager_DenormalizedRelationship_Status_Get_ParametersArray");

                #region Populate entity tree id list table value parameters

                if (!String.IsNullOrWhiteSpace(entityTreeIdList))
                {
                    entityIdList = new List<SqlDataRecord>();

                    SqlMetaData[] metaData = generator.GetTableValueMetadata("RelationshipManager_DenormalizedRelationship_Status_Get_ParametersArray", parameters[2].ParameterName);

                    foreach (String id in entityTreeIdList.Split(','))
                    {
                        SqlDataRecord entityRecord = new SqlDataRecord(metaData);

                        entityRecord.SetValue(0, ValueTypeHelper.Int64TryParse(id, -1)); // entityId

                        entityIdList.Add(entityRecord);
                    }
                }

                #endregion Populate entityIdList table value parameters

                parameters[0].Value = entityId;
                parameters[1].Value = containerId;
                parameters[2].Value = entityIdList;
                parameters[3].Value = relationshipTypeId; //If relationship type id is 0 means All. It means get the status from all relationship types else requested relationship type.

                storedProcedureName = "usp_RelationshipManager_DenormalizedRelationship_Status_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    if (reader["RelationshipDenormStatus"] != null)
                    {
                        isDenormalizedRelationshipsProcessRequired = ValueTypeHelper.BooleanTryParse(reader["RelationshipDenormStatus"].ToString(), isDenormalizedRelationshipsProcessRequired);
                    }
                }
            }
            finally
            {
                // Close the connection...
                if (reader != null)
                    reader.Close();

                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("RelationshipManager.RelationshipDenormDA.IsDenormalizedRelationshipsDirty", MDMTraceSource.RelationshipGet);
                }
            }

            return isDenormalizedRelationshipsProcessRequired;
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}