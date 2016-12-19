using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using Microsoft.SqlServer.Server;

namespace MDM.AttributeModelManager.Data
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects;

    /// <summary>
    /// 
    /// </summary>
    public class AttributeGroupDA : SqlClientDataAccessBase
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// GetRelationshipAttributeGroups
        /// </summary>
        /// <param name="catalogID">Indicates the catalogID of an Attribute Group</param>
        /// <param name="nodeTypeID">Indicates the nodeTypeID of an Attribute Group</param>
        /// <returns></returns>
        public Collection<AttributeGroup> GetRelationshipAttributeGroups(Int32 catalogID, Int32 nodeTypeID)
        {
            Collection<AttributeGroup> data = null;

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("AttributeModelManager_SqlParameters");

                parameters = generator.GetParameters("AttributeModelManager_AttributeGroup_GetRelationshipAttributeGroups_ParametersArray");

                parameters[0].Value = catalogID;
                parameters[1].Value = nodeTypeID;

                storedProcedureName = "usp_RelationshipManager_RelationshipType_Get";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                data = new Collection<AttributeGroup>();

                while (reader.Read())
                {
                    String longName = String.Empty;
                    Int32 iRelationShipTypeId = 0;

                    if (reader["LongName"] != null)
                        longName = reader["LongName"].ToString();

                    if (reader["PK_RelationshipType"] != null)
                        Int32.TryParse(reader["PK_RelationshipType"].ToString(), out iRelationShipTypeId);

                    AttributeGroup attributeGroup = new AttributeGroup(iRelationShipTypeId, longName);
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
        /// Get AttributeType
        /// </summary>
        /// <param name="common">Indicates the common of an Attribute Group</param>
        /// <param name="technical">Indicates the technical of an Attribute Group</param>
        /// <param name="relationship">Indicates the relationship of an Attribute Group</param>
        /// <param name="locales">Indicates collection of locales.</param>
        /// <param name="systemLocale">Indicates system locale</param>
        /// <returns></returns>
        public Collection<AttributeGroup> GetByAttributeType(Int32 common, Int32 technical, Int32 relationship,Collection<LocaleEnum> locales,LocaleEnum systemLocale)
        {
            Collection<AttributeGroup> data = null; ;

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("AttributeModelManager_SqlParameters");

                parameters = generator.GetParameters("AttributeModelManager_AttributeGroup_GetByAttributeType_ParametersArray");

                #region Populate table value parameters and also populate return result collection

                List<SqlDataRecord> localeList = new List<SqlDataRecord>();

                SqlMetaData[] sqllocales = generator.GetTableValueMetadata("AttributeModelManager_AttributeGroup_GetByAttributeType_ParametersArray", parameters[3].ParameterName);

                foreach (LocaleEnum locale in locales)
                {
                    SqlDataRecord localeRecord = new SqlDataRecord(sqllocales);
                    localeRecord.SetValue(0, (Int32)locale);
                    localeRecord.SetValue(1, locale.ToString());
                    if ((Int32)systemLocale == (Int32)locale)
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

                parameters[0].Value = common;
                parameters[1].Value = technical;
                parameters[2].Value = relationship;
                parameters[3].Value = localeList;

                storedProcedureName = "usp_Attribute_GetAttributeGroupsByType";

                data = new Collection<AttributeGroup>();
                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    Int32 attributeId = 0;
                    String shortName = String.Empty;
                    String longName = String.Empty;

                    if (reader["PK_Attribute"] != null)
                        Int32.TryParse(reader["PK_Attribute"].ToString(), out attributeId);
                    if (reader["ShortName"] != null)
                        shortName = reader["ShortName"].ToString();
                    if (reader["LongName"] != null)
                        longName = reader["LongName"].ToString();

                    AttributeGroup attributeGroup = new AttributeGroup(attributeId, shortName, longName);
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
        /// Get ChildAttributeGroups
        /// </summary>
        /// <param name="attributeParentID">Indicates the attributeParentID of an Attribute Group</param>
        /// <returns></returns>
        public String GetChildAttributeGroups(Int32 attributeParentID)
        {
            StringBuilder childAttributeGroupDataXML = new StringBuilder();

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("AttributeModelManager_SqlParameters");

                parameters = generator.GetParameters("AttributeModelManager_AttributeGroup_GetChildAttributeGroups_ParametersArray");

                parameters[0].Value = attributeParentID;

                storedProcedureName = "usp_N_getAttrGroups_XML";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        childAttributeGroupDataXML.Append(reader[0]);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return childAttributeGroupDataXML.ToString();
        }

        #endregion
    }
}
