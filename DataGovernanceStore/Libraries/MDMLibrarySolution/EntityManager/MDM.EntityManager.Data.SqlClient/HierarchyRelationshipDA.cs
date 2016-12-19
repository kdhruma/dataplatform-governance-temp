using System;
using System.Data.SqlClient;
using System.Transactions;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.SqlServer.Server;

namespace MDM.EntityManager.Data
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects;

    public class HierarchyRelationshipDA : SqlClientDataAccessBase
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
        /// Gets hierarchy relationships for the requested entity id
        /// </summary>
        /// <param name="entityId">Id of the entity for which hierarchies are required</param>
        /// <param name="includeCategories">Flag saying whether to include categories in Hierarchy</param>
        /// <returns>The collection of hierarchy relationships</returns>
        /// <exception cref="ArgumentException">Thrown when entity id is not available</exception>
        public HierarchyRelationshipCollection GetHierarchyRelationships(Int64 entityId, Boolean includeCategories)
        {
            SqlDataReader reader = null;
            HierarchyRelationshipCollection hierarchyRelationshipCollection = null;

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            try
            {
                connectionString = AppConfigurationHelper.ConnectionString;

                SqlParametersGenerator generator = new SqlParametersGenerator("EntityManager_SqlParameters");

                parameters = generator.GetParameters("EntityManager_EntityHierarchyRelationship_Get_ParametersArray");

                parameters[0].Value = entityId;
                parameters[1].Value = includeCategories;

                storedProcedureName = "usp_EntityManager_EntityHierarchyRelationship_Get";

                //if (!String.IsNullOrWhiteSpace(AppConfigurationHelper.StoredProcedureSuffix))
                //    storedProcedureName = String.Format("{0}_{1}", storedProcedureName, AppConfigurationHelper.StoredProcedureSuffix);

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    hierarchyRelationshipCollection = PopulateHierarchyRelationshipCollection(reader);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return hierarchyRelationshipCollection;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private HierarchyRelationshipCollection PopulateHierarchyRelationshipCollection(SqlDataReader reader)
        {
            HierarchyRelationship hierarchyRelationship = null;
            HierarchyRelationshipCollection hierarchyRelationshipCollection = new HierarchyRelationshipCollection();

            while (reader.Read())
            {
                Int32 id = 0;
                String type = String.Empty;
                Int32 relatedEntityId = 0;
                RelationshipDirection relationshipDirection = RelationshipDirection.None;
                String path = String.Empty;
                Int32 baseEntityId = 0;
                String relatedEntityName = String.Empty;

                if (reader["Id"] != null)
                    Int32.TryParse(reader["Id"].ToString(), out id);
                if (reader["Type"] != null)
                    type = reader["Type"].ToString();
                if (reader["RelatedEntityId"] != null)
                    Int32.TryParse(reader["RelatedEntityId"].ToString(), out relatedEntityId);
                if (reader["RelatedEntityName"] != null)
                    relatedEntityName = reader["RelatedEntityName"].ToString();
                if (reader["ParentEntityId"] != null)
                    Int32.TryParse(reader["ParentEntityId"].ToString(), out baseEntityId);
                if (reader["Direction"] != null)
                {
                    String direction = reader["Direction"].ToString();
                    Enum.TryParse<RelationshipDirection>(direction, true, out relationshipDirection);
                }

                if (reader["Path"] != null)
                    path = reader["Path"].ToString();

                hierarchyRelationship = new HierarchyRelationship(id, type, relatedEntityId, relationshipDirection, path);
                hierarchyRelationship.Name = relatedEntityName;

                //Check whether this relationship is the child of already added relationship. This check has to happen recursively.
                HierarchyRelationship parentHierarchyRelationship = hierarchyRelationshipCollection.FindHierarchyRelationshipRecursive(baseEntityId);

                if (parentHierarchyRelationship != null)
                    parentHierarchyRelationship.RelationshipCollection.Add(hierarchyRelationship);
                else
                    hierarchyRelationshipCollection.Add(hierarchyRelationship);
            }

            return hierarchyRelationshipCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operationResult"></param>
        /// <param name="reader"></param>
        private void UpdateOperationResult(OperationResult operationResult, SqlDataReader reader)
        {

            try
            {
                Boolean areAllSuccess = true;
                Boolean areAllFailed = true;
                Table processStatusTable = new Table();

                Column relatedEntityIdColumn = new Column(1, "RelatedEntityId", "EntityId", null);
                processStatusTable.Columns.Add(relatedEntityIdColumn);

                Column relatedEntityLongNameColumn = new Column(2, "RelatedEntityLongName", "RelatedEntityLongName", null);
                processStatusTable.Columns.Add(relatedEntityLongNameColumn);

                Column processStatusColumn = new Column(3, "ProcessStatus", "ProcessStatus", null);
                processStatusTable.Columns.Add(processStatusColumn);

                while (reader.Read())
                {
                    Int32 relatedEntityId = 0;
                    String relatedEntityLongName = String.Empty;
                    Boolean processStatus = false;

                    if (reader["RelatedEntityId"] != null)
                        Int32.TryParse(reader["RelatedEntityId"].ToString(), out relatedEntityId);
                    if (reader["RelatedEntityLongName"] != null)
                        relatedEntityLongName = reader["RelatedEntityLongName"].ToString();
                    if (reader["ProcessStatus"] != null)
                        Boolean.TryParse(reader["ProcessStatus"].ToString(), out processStatus);

                    if (processStatus)
                        areAllFailed = false;
                    else
                        areAllSuccess = false;

                    Row processStatusRow = processStatusTable.NewRow(relatedEntityId);
                    processStatusRow.SetValue(relatedEntityIdColumn, relatedEntityId);
                    processStatusRow.SetValue(relatedEntityLongNameColumn, relatedEntityLongName);
                    processStatusRow.SetValue(processStatusColumn, processStatus);
                }

                operationResult.ReturnValues.Add(processStatusTable);

                if (areAllSuccess)
                    operationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                else if (areAllFailed)
                    operationResult.OperationResultStatus = OperationResultStatusEnum.Failed;
                else
                    operationResult.OperationResultStatus = OperationResultStatusEnum.CompletedWithErrors;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        #endregion

        #endregion
    }
}