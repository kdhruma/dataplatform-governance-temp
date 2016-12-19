using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace MDM.ImportSources.StagingDB.Data
{
    using System.Collections;
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.Utility;

    public class DataBaseSourceDataDA : SqlClientDataAccessBase
    {
        Boolean serverSplitEnabled = false;

        ICachedDataModel cachedDataModel = null;

        public DataBaseSourceDataDA()
        {
            serverSplitEnabled = IsServerSplitEnabled();

            cachedDataModel = CachedDataModelManager.CachedDataModel.GetSingleton();
        }

        #region Public Methods

        /// <summary>
        /// Gets the entity information ( seed, endpoint, count) from the staging database.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="seed"></param>
        /// <param name="endpoint"></param>
        /// <param name="entitycount"></param>
        /// <returns></returns>
        public void GetEntityInformation(DBCommandProperties command, out Int64 seed, out Int64 endpoint, out Int64 entitycount, out Int32 batchsize)
        {
            SqlDataReader reader = null;
            seed = endpoint = entitycount = -1;
            batchsize = 100;
            try
            {
                string storedProcedureName = "usp_DataLoadManager_Staging_Entity_GetInformation";
                reader = ExecuteProcedureReader(GetConnectionString(command), null, storedProcedureName);

                while (reader.Read())
                {
                    seed = Convert.ToInt32(reader["Entity_Seed"]);
                    endpoint = Convert.ToInt32(reader["Entity_Endpoint"]);
                    entitycount = Convert.ToInt32(reader["Entity_Count"]);
                    batchsize = Convert.ToInt32(reader["Entity_Batchsize"]);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        /// <summary>
        /// Gets the entity data from the staging database for a given batch.
        /// </summary>
        /// <param name="startPK"></param>
        /// <param name="endPK"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public EntityCollection GetEntityDataBatch(Int64 startPK, Int64 endPK, DBCommandProperties command, String containerName, String organizationName, String entityTypeName)
        {
            EntityCollection entityCollection = new EntityCollection();
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("ImportManager_Parameters");

                parameters = generator.GetParameters("usp_DataLoadManager_Staging_Entity_Get_ParametersArray");

                parameters[0].Value = startPK;
                parameters[1].Value = endPK;

                if (String.IsNullOrEmpty(containerName) == false)
                {
                    parameters[2].Value = containerName;
                }

                if (String.IsNullOrEmpty(organizationName) == false)
                {
                    parameters[3].Value = organizationName;
                }

                if (String.IsNullOrEmpty(entityTypeName) == false)
                {
                    parameters[4].Value = entityTypeName;
                }

                storedProcedureName = "usp_DataLoadManager_Staging_Entity_Get";
                reader = ExecuteProcedureReader(GetConnectionString(command), parameters, storedProcedureName);

                while (reader.Read())
                {
                    Entity staging = new Entity()
                    {
                        ContainerName = reader["CatalogName"].ToString(),
                        ParentExternalId = reader["ParentExternalId"].ToString(),
                        CategoryPath = reader["CategoryPath"].ToString(),
                        ExternalId = reader["ExternalId"].ToString(),
                        Name = reader["ShortName"].ToString(),
                        LongName = reader["LongName"].ToString(),
                        EntityTypeName = reader["EntityTypeName"].ToString(),
                        ReferenceId = Convert.ToInt64(reader["ReferenceId"]),
                        OrganizationName = reader["OrgName"].ToString()
                    };

                    string strAction = reader["Action"].ToString();
                    MDM.Core.ObjectAction action = ObjectAction.Unknown;
                    Enum.TryParse<MDM.Core.ObjectAction>(strAction, out action);
                    staging.Action = action;

                    if (reader["TargetCategoryPath"] != null)
                    {
                        staging.EntityMoveContext.TargetCategoryPath = reader["TargetCategoryPath"].ToString();
                    }

                    entityCollection.Add(staging);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return entityCollection;
        }

        /// <summary>
        /// Gets the specfic attribute staging data from the staging database.
        /// </summary>
        /// <param name="attributeType"></param>
        /// <param name="entityList"></param>
        /// <param name="entityCollection"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public AttributeCollection GetAttributeDataforEntities(AttributeModelType attributeType, string entityList, EntityCollection entityCollection, DBCommandProperties command)
        {
            AttributeCollection attribCollection = new AttributeCollection();
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;

            try
            {
                string attrType = string.Empty;
                switch (attributeType)
                {
                    case MDM.Core.AttributeModelType.Common:
                        attrType = "C";
                        break;
                    case MDM.Core.AttributeModelType.Category:
                        attrType = "T";
                        break;
                    case MDM.Core.AttributeModelType.Relationship:
                        attrType = "R";
                        break;
                }

                SqlParametersGenerator generator = new SqlParametersGenerator("ImportManager_Parameters");

                parameters = generator.GetParameters("usp_DataLoadManager_Staging_Attributes_Get_ParametersArray");

                parameters[0].Value = entityList;
                parameters[1].Value = attrType;

                storedProcedureName = "usp_DataLoadManager_Staging_Attributes_Get";

                reader = ExecuteProcedureReader(GetConnectionString(command), parameters, storedProcedureName);

                while (reader.Read())
                {
                    String attrName = reader["Attributename"].ToString();
                    String attrParentName = reader["AttributeParentName"].ToString();
                    String referenceNumber = reader["ExternalId"].ToString();
                    Int64 referenceId = Convert.ToInt64(reader["ReferenceId"]);
                    Boolean isComplex = Convert.ToBoolean(reader["IsComplex"]);

                    if (String.IsNullOrEmpty(attrName) || String.IsNullOrEmpty(attrParentName))
                    {
                        // Mandatory columns are available..log and continue
                        continue;
                    }
                    // Get the container name
                    string containerName = reader["CatalogName"].ToString();
                    String strlocale = reader["Locale"].ToString();

                    LocaleEnum locale = LocaleEnum.UnKnown;
                    //try to parse only when this value is valid, otherwise TryParse will assign default value
                    if (Enum.IsDefined(typeof(LocaleEnum), strlocale))
                    {
                        Enum.TryParse<LocaleEnum>(strlocale, out locale);
                    }

                    Attribute attrib = null;
                    long attributeId = 0;

                    if (attributeType != MDM.Core.AttributeModelType.Relationship)
                    {
                        Entity entity = null;

                        entity = entityCollection.GetEntityByReferenceId(referenceId);

                        // did we find one?
                        if (entity == null)
                        {
                            // Mandatory columns are available..log and continue
                            continue;
                        }

                        attributeId = entity.Id;
                    }
                    else
                    {
                        Relationship relationship = entityCollection.GetRelationshipByExternalId(referenceNumber);

                        if (relationship == null)
                        {
                            // Mandatory columns are available..log and continue
                            continue;
                        }

                        attributeId = relationship.RelatedEntityId;
                    }

                    // see if we have this combination already
                    var filteredAttributes = from attr in attribCollection
                                             where
                                                attr.Name.Equals(attrName) && attr.AttributeParentName.Equals(attrParentName) && attr.EntityId.Equals(attributeId) && attr.Locale.Equals(locale)
                                             select attr;

                    // did we have a match?
                    if (filteredAttributes.Any())
                        attrib = filteredAttributes.First();

                    if (attrib == null)
                    {
                        // Create new attribute instance..
                        if (isComplex)
                        {
                            AttributeModel complexModel = cachedDataModel.GetAllBaseAttributeModels().Where(a => a.Name.Equals(attrName)
                                                            && a.AttributeParentName.Equals(attrParentName)
                                                            && a.IsComplex)
                                .FirstOrDefault();
                            if (complexModel == null)
                            {
                                continue;//if cant find model, leave this record
                            }
                            attrib = new Attribute(complexModel);
                        }
                        else
                        {
                            attrib = new MDM.BusinessObjects.Attribute()
                            {
                                Name = attrName,
                                LongName = attrName,
                                AttributeParentName = attrParentName,
                                AttributeModelType = attributeType
                            };
                        }

                        string strAction = reader["Action"].ToString();
                        MDM.Core.ObjectAction action = ObjectAction.Unknown;
                        Enum.TryParse<MDM.Core.ObjectAction>(strAction, out action);
                        attrib.Action = action;

                        attrib.Locale = locale;

                        attribCollection.Add(attrib);
                    }

                    attrib.EntityId = attributeId;

                    attrib.InstanceRefId = ( Int32 )attributeId;

                    #region Add value in attribute object

                    if (isComplex == false)
                    {
                        //Add value
                        Value value = new Value(reader["AttrVal"]);

                        /// The PK of the staging Attribute key is passed here...
                        value.Id = Convert.ToInt32(reader["PK_Initialload_Staging"]);

                        // if the staging has the lookup refernce populated use it..
                        Int32 lookupReferenceId = 0;
                        lookupReferenceId = ValueTypeHelper.Int32TryParse(reader["WSID"].ToString(), 0);

                        if (lookupReferenceId > 0)
                        {
                            // if the WSID is given, use it for attrval also..
                            value.ValueRefId = lookupReferenceId;
                            value.AttrVal = lookupReferenceId;
                            value.InvariantVal = lookupReferenceId;
                        }
                        // by default set to -1 for all attributes. For collection we can override later.
                        value.Sequence = -1;

                        value.Uom = reader["UOM"].ToString();

                        attrib.AppendValue(value);
                    }
                    #endregion
                }

                #region Read complex attribute values

                GetComplexAttributeDataforEntities(reader, attribCollection);

                #endregion Read complex attribute values
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return attribCollection;
        }

        private bool GetComplexAttributeDataforEntities(SqlDataReader reader, AttributeCollection attributes)
        {
            ArrayList complexColumnNames = new ArrayList();

            try
            {
                while (reader.NextResult())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        String columnName = reader.GetName(i);
                        complexColumnNames.Add(columnName);
                    }

                    while (reader.Read())
                    {
                        Int64 referenceId = Convert.ToInt64(reader["ReferenceId"]);
                        String attributeName = Convert.ToString(reader["AttributeName"]);
                        String attributeParentName = Convert.ToString(reader["AttributeParentName"]);
                        if (referenceId <= 0)
                        {
                            continue;
                        }

                        Attribute complexAttribute = attributes.FirstOrDefault(a => a.Name.ToLower().Equals(attributeName.ToLower()) && a.AttributeParentName.ToLower().Equals(attributeParentName.ToLower()));
                        if (complexAttribute == null)
                        {
                            continue;//should not happen
                        }

                        IAttributeCollection newComplexRecord = complexAttribute.NewComplexChildRecord();
                        foreach (IAttribute childAttribute in newComplexRecord)
                        {
                            String childAttributeColumnName = childAttribute.Name.Replace(" ", "_");
                            if (!complexColumnNames.Contains(childAttributeColumnName))
                            {
                                continue;//TODO: no column for this child attribute... this should be a warning.
                            }
                            Value value = new Value();
                            String attrVal = reader[childAttributeColumnName].ToString();
                            value.AttrVal = attrVal;

                            childAttribute.SetValue(( IValue )value);
                        }
                        complexAttribute.AddComplexChildRecord(newComplexRecord);
                        complexAttribute.Action = ObjectAction.Update;
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return true;
        }

        public bool UpdateErrorEntities(EntityOperationResultCollection errorEntities, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DataBaseSourceDataDA.UpdateErrorEntities", MDMTraceSource.Imports, false);

            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;

            bool returnStatus = true;

            if (errorEntities == null || errorEntities.Count <= 0)
            {
                // nothing to process..
                return returnStatus;
            }

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("ImportManager_Parameters");

                parameters = generator.GetParameters("usp_DataLoadManager_Staging_Entity_Error_Update_ParametersArray");

                parameters[0].Value = errorEntities.ToXml();

                storedProcedureName = "usp_DataLoadManager_Staging_Entity_Error_Update";

                reader = ExecuteProcedureReader(GetConnectionString(command), parameters, storedProcedureName);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DataBaseSourceDataDA.UpdateErrorEntities", MDMTraceSource.Imports);

            }

            return returnStatus;
        }

        public bool UpdateErrorRelationships(RelationshipOperationResultCollection errorRelationships, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DataBaseSourceDataDA.UpdateErrorRelationships", MDMTraceSource.Imports, false);

            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;
            bool returnStatus = true;

            if (errorRelationships == null || errorRelationships.Count <= 0)
            {
                // nothing to process..
                return returnStatus;
            }

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("ImportManager_Parameters");

                parameters = generator.GetParameters("usp_DataLoadManager_Staging_Relationship_Error_Update_ParametersArray");

                parameters[0].Value = errorRelationships.ToXml();

                storedProcedureName = "usp_DataLoadManager_Staging_Relationship_Error_Update";

                reader = ExecuteProcedureReader(GetConnectionString(command), parameters, storedProcedureName);
            }

            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DataBaseSourceDataDA.UpdateErrorRelationships", MDMTraceSource.Imports);

            }
            return returnStatus;
        }

        public bool UpdateErrorAttributes(AttributeOperationResultCollection errorAttributes, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DataBaseSourceDataDA.UpdateErrorAttributes", MDMTraceSource.Imports, false);

            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;

            bool returnStatus = true;
            if (errorAttributes == null || errorAttributes.Count <= 0)
            {
                // nothing to process..
                return returnStatus;
            }

            // call a single SP and update one shot
            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("ImportManager_Parameters");

                parameters = generator.GetParameters("usp_DataLoadManager_Staging_Attributes_Error_Update_ParametersArray");

                parameters[0].Value = errorAttributes.ToXml();

                storedProcedureName = "usp_DataLoadManager_Staging_Attributes_Error_Update";

                reader = ExecuteProcedureReader(GetConnectionString(command), parameters, storedProcedureName);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DataBaseSourceDataDA.UpdateErrorAttributes", MDMTraceSource.Imports);
            }

            return returnStatus;
        }

        public bool UpdateErrorAttributes(AttributeModelType attributeType, string entityList, string errorMessage, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DataBaseSourceDataDA.UpdateErrorAttributes", MDMTraceSource.Imports, false);

            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;
            bool returnStatus = true;

            if (string.IsNullOrEmpty(entityList))
            {
                // nothing to process..
                return returnStatus;
            }

            if (string.IsNullOrEmpty(errorMessage))
            {
                // nothing to process..
                return returnStatus;
            }

            string attrType = string.Empty;
            switch (attributeType)
            {
                case MDM.Core.AttributeModelType.Common:
                    attrType = "C";
                    break;
                case MDM.Core.AttributeModelType.Category:
                    attrType = "T";
                    break;
                case MDM.Core.AttributeModelType.Relationship:
                    attrType = "R";
                    break;
            }

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("ImportManager_Parameters");

                parameters = generator.GetParameters("usp_DataLoadManager_Staging_Attributes_EntityError_Update_ParametersArray");

                parameters[0].Value = entityList;
                parameters[1].Value = attrType;
                parameters[2].Value = errorMessage;

                storedProcedureName = "usp_DataLoadManager_Staging_Attributes_EntityError_Update";

                reader = ExecuteProcedureReader(GetConnectionString(command), parameters, storedProcedureName);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DataBaseSourceDataDA.UpdateErrorAttributes", MDMTraceSource.Imports);

            }

            return returnStatus;
        }

        public bool UpdateSuccessEntities(EntityCollection entities, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DataBaseSourceDataDA.UpdateSuccessEntities", MDMTraceSource.Imports, false);

            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;
            bool returnStatus = true;
            string entityList = string.Empty;

            if (entities == null || entities.Count() == 0)
            {
                // nothing to process..
                return returnStatus;
            }

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Operation result node start
            xmlWriter.WriteStartElement("InitialLoad");

            // Note that for initial load, entity load is exclusive to relationship load
            // When loading entities, 
            foreach (Entity item in entities)
            {
                if (item.Relationships.Count < 1)
                {
                    //Information node start
                    xmlWriter.WriteStartElement("Entity");
                    xmlWriter.WriteAttributeString("Id", item.Id.ToString());
                    xmlWriter.WriteAttributeString("ReferenceId", item.ReferenceId.ToString());
                    xmlWriter.WriteEndElement();
                }
                else
                {
                    foreach (Relationship relationship in item.Relationships)
                    {
                        xmlWriter.WriteStartElement("Relationship");
                        xmlWriter.WriteAttributeString("RelationshipExternalId", relationship.RelationshipExternalId);
                        xmlWriter.WriteAttributeString("FromId", item.Id.ToString());
                        xmlWriter.WriteAttributeString("ToId", relationship.RelatedEntityId.ToString());
                        xmlWriter.WriteEndElement();
                    }
                }
            }

            xmlWriter.WriteEndElement();
            xmlWriter.Flush();

            //Get the actual XML
            entityList = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("ImportManager_Parameters");

                parameters = generator.GetParameters("usp_DataLoadManager_Staging_Entity_Status_Update_ParametersArray");

                parameters[0].Value = entityList;

                storedProcedureName = "usp_DataLoadManager_Staging_Entity_Status_Update";

                reader = ExecuteProcedureReader(GetConnectionString(command), parameters, storedProcedureName);
            }

            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DataBaseSourceDataDA.UpdateSuccessEntities", MDMTraceSource.Imports);
            }
            return returnStatus;
        }

        public bool UpdateSuccessAttributes(AttributeModelType attributeType, EntityCollection entities, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DataBaseSourceDataDA.UpdateSuccessAttributes", MDMTraceSource.Imports, false);

            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;
            bool returnStatus = true;
            if (entities == null || entities.Count() == 0)
            {
                // nothing to process..
                return returnStatus;
            }

            // Get the entity id list
            string entityList = string.Empty;

            string attrType = string.Empty;
            switch (attributeType)
            {
                case MDM.Core.AttributeModelType.Common:
                    attrType = "C";
                    break;
                case MDM.Core.AttributeModelType.Category:
                    attrType = "T";
                    break;
                case MDM.Core.AttributeModelType.Relationship:
                    attrType = "R";
                    break;
            }

            if (attrType != "R")
            {
                foreach (Entity item in entities)
                {
                    if (string.IsNullOrEmpty(entityList))
                        entityList = string.Format("'{0}'", item.ReferenceId.ToString());
                    else
                        entityList = string.Format("{0}, '{1}'", entityList, item.ReferenceId);
                }
            }
            else
            {
                foreach (Entity item in entities)
                {
                    foreach (Relationship relationship in item.Relationships)
                    {
                        if (string.IsNullOrEmpty(entityList))
                            entityList = string.Format("'{0}'", relationship.RelationshipExternalId);
                        else
                            entityList = string.Format("{0}, '{1}'", entityList, relationship.RelationshipExternalId);
                    }
                }
            }

            // if there are no entities..dont make db call..
            if (String.IsNullOrEmpty(entityList))
            {
                return returnStatus;
            }

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("ImportManager_Parameters");

                parameters = generator.GetParameters("usp_DataLoadManager_Staging_Attribute_Status_Update_ParametersArray");

                parameters[0].Value = entityList;
                parameters[1].Value = attrType;

                storedProcedureName = "usp_DataLoadManager_Staging_Attribute_Status_Update";

                reader = ExecuteProcedureReader(GetConnectionString(command), parameters, storedProcedureName);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DataBaseSourceDataDA.UpdateSuccessAttributes", MDMTraceSource.Imports);

            }
            return returnStatus;
        }

        #region Relationship DA methods

        /// <summary>
        /// Gets the relationship data from the staging database for a given batch.
        /// </summary>
        /// <param name="startPK"></param>
        /// <param name="endPK"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public EntityCollection GetRelationshipDataBatch(Int64 startPK, Int64 endPK, DBCommandProperties command)
        {
            EntityCollection entityCollection = new EntityCollection();
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;
            Dictionary<string, Entity> batchedEntityMap = new Dictionary<string, Entity>();

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("ImportManager_Parameters");

                parameters = generator.GetParameters("usp_DataLoadManager_Staging_Relationship_Get_ParametersArray");

                parameters[0].Value = startPK;
                parameters[1].Value = endPK;

                storedProcedureName = "usp_DataLoadManager_Staging_Relationship_Get";
                reader = ExecuteProcedureReader(GetConnectionString(command), parameters, storedProcedureName);

                while (reader.Read())
                {
                    Entity entity = null;
                    String strAction = reader["Action"].ToString();

                    ObjectAction action = ObjectAction.Unknown;

                    switch (strAction)
                    {
                        case "Create":
                            action = ObjectAction.Create;
                            break;
                        case "Update":
                            action = ObjectAction.Update;
                            break;
                        case "Delete":
                            action = ObjectAction.Delete;
                            break;
                        default:
                            action = ObjectAction.Unknown;
                            break;
                    }

                    string externalId = reader["ExternalId_From"].ToString();

                    if (batchedEntityMap.ContainsKey(externalId))
                    {
                        entity = batchedEntityMap[externalId];
                    }

                    if (entity == null)
                    {
                        entity = new Entity()
                        {
                            ExternalId = externalId,
                            ContainerName = reader["CatalogName_From"].ToString(),
                            CategoryPath = reader["CategoryPath_From"].ToString(),
                            EntityTypeName = reader["NodeTypeName_From"].ToString(),
                            LongName = reader["LongName_From"].ToString(),
                        };

                        if (reader["ParentExternalId_From"] != null)
                        {
                            entity.ParentExternalId = reader["ParentExternalId_From"].ToString();
                        }

                        if (reader["OrgName_From"] != null)
                        {
                            entity.OrganizationName = reader["OrgName_From"].ToString();
                        }

                        batchedEntityMap.Add(externalId, entity);
                        entityCollection.Add(entity);
                    }

                    Relationship relationship = new Relationship()
                    {
                        RelationshipExternalId = reader["RelationshipExternalId"].ToString(),
                        ToExternalId = reader["ExternalId_To"].ToString(),
                        ToContainerName = reader["CatalogName_To"].ToString(),
                        ToCategoryPath = reader["CategoryPath_To"].ToString(),
                        ToEntityTypeName = reader["NodeTypeName_To"].ToString(),
                        RelationshipTypeName = reader["RelationshipTypeName"].ToString(),
                        RelationshipSourceEntityName = entity.ExternalId,
                        Action = action
                    };

                    entity.Relationships.Add(relationship);
                    entity.Action = action;
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return entityCollection;
        }

        #endregion

        #region Extension Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startPK"></param>
        /// <param name="endPK"></param>
        /// <param name="command"></param>
        /// <param name="containerName"></param>
        /// <returns></returns>
        public ExtensionRelationship GetParentExtensionRelationShip(Entity entity, DBCommandProperties command)
        {
            ExtensionRelationship extensionRelationship = new ExtensionRelationship();
            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("ImportManager_Parameters");

                parameters = generator.GetParameters("usp_DataLoadManager_Staging_Entity_ExtensionParent_Get_ParametersArray");

                parameters[0].Value = entity.ReferenceId;

                storedProcedureName = "usp_DataLoadManager_Staging_Entity_ExtensionParent_Get";
                reader = ExecuteProcedureReader(GetConnectionString(command), parameters, storedProcedureName);

                while (reader.Read())
                {
                    extensionRelationship.ContainerName = reader["CatalogName_MDL"].ToString();
                    extensionRelationship.ExternalId = reader["ExternalId_MDL"].ToString();
                    extensionRelationship.CategoryPath = reader["CategoryPath_MDL"].ToString();
                    // this is the parent. The direction has to be up.
                    extensionRelationship.Direction = RelationshipDirection.Up;
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            // if there is no external id..do not process..
            if (String.IsNullOrEmpty(extensionRelationship.ExternalId))
            {
                extensionRelationship = null;
            }

            return extensionRelationship;
        }
        #endregion
        #endregion

        #region Private Methods
        /// <summary>
        /// If the server split is enabled, get the connection string from configuration file
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private String GetConnectionString(DBCommandProperties command)
        {
            String connectionString = String.Empty;

            if (serverSplitEnabled)
            {
                connectionString = command.ConnectionString;
            }
            else
            {
                connectionString = ConfigurationManager.AppSettings["StagingConnectionString"];
            }

            return connectionString;
        }

        /// <summary>
        /// Method to check if the server split is enabled or not.
        /// </summary>
        /// <returns></returns>
        private Boolean IsServerSplitEnabled()
        {
            String serverSplitEnabledStr = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.PhysicalServerSplit.Enabled");
            serverSplitEnabled = ValueTypeHelper.ConvertToBoolean(serverSplitEnabledStr);
            return serverSplitEnabled;
        }
        #endregion
    }
}
