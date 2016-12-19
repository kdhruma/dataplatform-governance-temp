using System;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;
using Microsoft.SqlServer.Server;

namespace MDM.AttributeModelManager.Data
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects;

    public class AttributeModelOperationsDA : SqlClientDataAccessBase
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        ///Get Attribute Type
        /// </summary>
        /// <param name="attributeGroupId">Indicates the attributeParentID of an Attribute MetaModel</param>
        /// <param name="locales">collection of locales</param>
        /// <param name="systemDataLocale">This parameter is specifying system data locale.</param>
        /// <returns>collection of attribute model</returns>
        public Collection<AttributeModel> GetByAttributeType(int attributeGroupId, Collection<LocaleEnum> locales, LocaleEnum systemDataLocale)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("AttributeModelManager.AttributeModelOperationDA.GetByAttributeGroup", false);

            Collection<AttributeModel> data = null;

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("Attributemanager_SqlParameters");

                parameters = generator.GetParameters("AttributeManager_AttributeMetaModel_GetByAttributeType_ParametersArray");

                #region Populate table value parameters and also populate return result collection

                List<SqlDataRecord> localeList = null;

                SqlMetaData[] sqllocales = generator.GetTableValueMetadata("AttributeManager_AttributeMetaModel_GetByAttributeType_ParametersArray", parameters[1].ParameterName);

                if (locales != null && locales.Count > 0)
                {
                    SqlDataRecord localeRecord = new SqlDataRecord(sqllocales);
                    localeList = new List<SqlDataRecord>();

                    foreach (LocaleEnum locale in locales)
                    {
                        localeRecord = new SqlDataRecord(sqllocales);
                        localeRecord.SetValue(0, (Int32)locale);
                        localeRecord.SetValue(1, locale.ToString());
                        if ((Int32)systemDataLocale == (Int32)locale)
                        {
                            localeRecord.SetValue(2, true);
                        }
                        else
                        {
                            localeRecord.SetValue(2, false);
                        }

                        localeList.Add(localeRecord);
                    }

                }
                #endregion

                parameters[0].Value = attributeGroupId;
                parameters[1].Value = localeList;

                storedProcedureName = "usp_Attribute_GetByAttributeGroup";

                data = new Collection<AttributeModel>();
                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    Int32 iAttributeId = 0;
                    String shortName = String.Empty;
                    String longName = String.Empty;
                    String attributeParentName = String.Empty;
                    Boolean showAtCreation = false;
                    Boolean isRequired = false;
                    Boolean isReadonly = false;
                    Int32 sortOrder = 0;

                    if (reader["PK_Attribute"] != null)
                        Int32.TryParse(reader["PK_Attribute"].ToString(), out iAttributeId);
                    if (reader["ShortName"] != null)
                        shortName = reader["ShortName"].ToString();
                    if (reader["LongName"] != null)
                        longName = reader["LongName"].ToString();
                    if (reader["AttributeParentName"] != null)
                        attributeParentName = reader["AttributeParentName"].ToString();
                    if (reader["ShowAtCreation"] != null)
                        Boolean.TryParse(reader["ShowAtCreation"].ToString(), out showAtCreation);
                    if (reader["Required"] != null)
                        Boolean.TryParse(reader["Required"].ToString(), out isRequired);
                    if (reader["ReadOnly"] != null)
                        Boolean.TryParse(reader["ReadOnly"].ToString(), out isReadonly);
                    if (reader["SortOrder"] != null)
                        ValueTypeHelper.Int32TryParse(reader["SortOrder"].ToString(), sortOrder);

                    AttributeModel attributeGroup = new AttributeModel(iAttributeId, shortName, longName, attributeParentName,
                                                        showAtCreation, isRequired, isReadonly, sortOrder);
                    data.Add(attributeGroup);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("AttributeModelManager.AttributeModelOperationDA.GetByAttributeGroup");
            }

            return data;
        }

        /// <summary>
        /// Get all Attributes
        /// </summary>
        /// <param name="ParentId"> Indicates the ParentId of an Attribute MetaModel</param>
        /// <param name="CountFrom"> Indicates the CountFrom of an Attribute MetaModel</param>
        /// <param name="CountTo"> Indicates the CountTo of an Attribute MetaModel</param>
        /// <param name="SearchParameter"> Indicates the SearchParameter of an Attribute MetaModel</param>
        /// <param name="SearchColumn"> Indicates the SearchColumn of an Attribute MetaModel</param>
        /// <param name="SortColumn"> Indicates the SortColumn of an Attribute MetaModel</param>
        /// <param name="LocaleId"> Indicates the LocaleId of an Attribute MetaModel</param>
        /// <param name="UnusedOnly"> Indicates the UnusedOnly of an Attribute MetaModel</param>
        /// <param name="UserLogin"> Indicates the UserLogin of an Attribute MetaModel</param>
        /// <returns>String</returns>
        public String GetAllAttributes(Int32 ParentId, Int32 CountFrom, Int32 CountTo, String SearchParameter, String SearchColumn, String SortColumn, Int32 LocaleId, Boolean UnusedOnly, String UserLogin)
        {
            StringBuilder commonAttributeData = new StringBuilder();

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("Attributemanager_SqlParameters");

                parameters = generator.GetParameters("AttributeManager_AttributeMetaModel_GetAllAttributes_ParametersArray");

                parameters[0].Value = ParentId;
                parameters[1].Value = CountFrom;
                parameters[2].Value = CountTo;
                parameters[3].Value = SearchParameter;
                parameters[4].Value = SearchColumn;
                parameters[5].Value = SortColumn;
                parameters[6].Value = LocaleId;
                parameters[7].Value = UnusedOnly;
                parameters[8].Value = UserLogin;

                storedProcedureName = "usp_Sec_Attr_getAttributes";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        commonAttributeData.Append(reader[0]);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return commonAttributeData.ToString();
        }

        /// <summary>
        /// Get technical attributes based on the taxonomy and Category
        /// </summary>
        /// <param name="categoryID"> Indicates the categoryID of an Attribute MetaModel</param>
        /// <param name="taxonomyID"> Indicates the taxonomyID of an Attribute MetaModel</param>
        /// <param name="localeID"> Indicates the localeID of an Attribute MetaModel</param>
        /// <returns></returns>
        public Collection<AttributeModel> GetTechAttributesByTaxonomyAndCategory(Int32 categoryID, Int32 taxonomyID, Int32 localeID)
        {
            Collection<AttributeModel> data = null;

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("Attributemanager_SqlParameters");

                parameters = generator.GetParameters("AttributeManager_AttributeMetaModel_GetTechAttributesByTaxonomyAndCategory_ParametersArray");

                parameters[0].Value = categoryID;
                parameters[1].Value = taxonomyID;
                parameters[2].Value = localeID;

                storedProcedureName = "usp_Sec_Catalog_getCategoryAttributeMap";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                data = new Collection<AttributeModel>();

                while (reader.Read())
                {
                    Int32 iAttributeId = 0;
                    String attributeName = reader["AttributeName"].ToString();
                    String attributeParent = reader["AttributeParent"].ToString();
                    String attributeDataTypeName = reader["AttributeDataTypeName"].ToString();
                    String attributeDisplayTypeName = reader["AttributeDisplayTypeName"].ToString();

                    if (reader["AttributeID"] != null)
                        Int32.TryParse(reader["AttributeID"].ToString(), out iAttributeId);
                    if (reader["AttributeName"] != null)
                        attributeName = reader["AttributeName"].ToString();
                    if (reader["AttributeParent"] != null)
                        attributeParent = reader["AttributeParent"].ToString();
                    if (reader["AttributeDataTypeName"] != null)
                        attributeDataTypeName = reader["AttributeDataTypeName"].ToString();
                    if (reader["AttributeDisplayTypeName"] != null)
                        attributeDisplayTypeName = reader["AttributeDisplayTypeName"].ToString();

                    AttributeModel attributeGroup = new AttributeModel(iAttributeId, attributeName, attributeParent, attributeDataTypeName, attributeDisplayTypeName);
                    data.Add(attributeGroup);
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
        /// Get Common Attributes based on Entity Type Id
        /// </summary>
        /// <param name="catalogId">Indicates the catalogID of an Attribute MetaModel</param>
        /// <param name="entityTypeId">Indicates the entityTypeID of an Attribute MetaModel</param>
        /// <param name="localeId">Indicates the data localeId</param>
        /// <returns></returns>
        public Collection<AttributeModel> GetCommonAttributesByContainerAndEntityType(Int32 catalogId, Int32 entityTypeId, Int32 localeId)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("AttributeModelManager.AttributeModelOperationDA.GetCommonAttributesByContainerAndEntityType", false);

            Collection<AttributeModel> data = null;

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("Attributemanager_SqlParameters");

                parameters = generator.GetParameters("AttributeManager_AttributeMetaModel_GetCommonAttributesByContainerAndEntityType_ParametersArray");

                parameters[0].Value = catalogId;
                parameters[1].Value = entityTypeId;
                parameters[2].Value = localeId;

                storedProcedureName = "usp_Attr_CatalogNodeTypeAttr_GetDT";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                data = new Collection<AttributeModel>();

                while (reader.Read())
                {
                    Int32 iAttributeId = 0;
                    String attributeName = String.Empty;
                    String attributeParent = String.Empty;
                    String attributeDataTypeName = String.Empty;
                    String attributeDisplayTypeName = String.Empty;

                    if (reader["AttrID"] != null)
                        Int32.TryParse(reader["AttrID"].ToString(), out iAttributeId);
                    if (reader["AttributeLongName"] != null)
                        attributeName = reader["AttributeLongName"].ToString();
                    if (reader["AttributeParentLongName"] != null)
                        attributeParent = reader["AttributeParentLongName"].ToString();
                    if (reader["DataTypeName"] != null)
                        attributeDataTypeName = reader["DataTypeName"].ToString();
                    if (reader["DisplayTypeName"] != null)
                        attributeDisplayTypeName = reader["DisplayTypeName"].ToString();

                    AttributeModel attributeGroup = new AttributeModel(iAttributeId, attributeName, attributeParent, attributeDataTypeName, attributeDisplayTypeName);
                    data.Add(attributeGroup);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("AttributeModelManager.AttributeModelOperationDA.GetCommonAttributesByContainerAndEntityType");
            }

            return data;
        }

        /// <summary>
        ///  Get Attribute Xml
        /// </summary>
        /// <param name="organizationId">Indicates the organization Id</param>
        /// <param name="catalogId">Indicates the catalogId Id</param>
        /// <param name="nodeType">Indicates the nodeType</param>
        /// <param name="branchLevel">Indicates the</param>
        /// <param name="includeComplexAttrChildren">Indicates the includeComplexAttrChildren</param>
        /// <param name="excludeableSearchable">Indicates the excludeableSearchable</param>
        /// <param name="locales">Indicates the collection locales</param>
        /// <param name="systemDataLocale">Indicates the systemDataLocale</param>
        /// <returns></returns>
        public String GetCatalogNodeTypeAttrbiuteAsXml(Int32 organizationId, Int32 catalogId, String nodeType, Int32 branchLevel, Boolean includeComplexAttrChildren, Boolean excludeableSearchable, Collection<LocaleEnum> locales, LocaleEnum systemDataLocale)
        {
            StringBuilder attributeXml = new StringBuilder();

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("Attributemanager_SqlParameters");

                parameters = generator.GetParameters("AttributeManager_AttributeModelOpeationDA_GetCatalogNodeTypeAttrbiuteAsXml_ParametersArray");

                #region Populate table value parameters and also populate return result collection

                List<SqlDataRecord> localeList = new List<SqlDataRecord>();

                SqlMetaData[] sqllocales = generator.GetTableValueMetadata("AttributeManager_AttributeModelOpeationDA_GetCatalogNodeTypeAttrbiuteAsXml_ParametersArray", parameters[6].ParameterName);

                foreach (LocaleEnum locale in locales)
                {
                    SqlDataRecord localeRecord = new SqlDataRecord(sqllocales);
                    localeRecord.SetValue(0, (Int32)locale);
                    localeRecord.SetValue(1, locale.ToString());
                    if ((Int32)systemDataLocale == (Int32)locale)
                    {
                        localeRecord.SetValue(2, true);
                    }
                    else
                    {
                        localeRecord.SetValue(2, false);
                    }
                    localeList.Add(localeRecord);
                }

                #endregion

                parameters[0].Value = organizationId;
                parameters[1].Value = catalogId;
                parameters[2].Value = nodeType;
                parameters[3].Value = branchLevel;
                parameters[4].Value = includeComplexAttrChildren;
                parameters[5].Value = excludeableSearchable;
                parameters[6].Value = localeList;

                storedProcedureName = "Usp_Attr_CatalogNodeTypeAttr_GetXML";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        attributeXml.Append(reader[0]);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return attributeXml.ToString();
        }
        #endregion
    }
}
