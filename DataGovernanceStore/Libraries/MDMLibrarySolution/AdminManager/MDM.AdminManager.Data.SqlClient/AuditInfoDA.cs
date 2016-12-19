using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Xml;

using MDM.Core;
using MDM.BusinessObjects;
using MDM.Utility;
using Microsoft.SqlServer.Server;

namespace MDM.AdminManager.Data
{
    public class AuditInfoDA : SqlClientDataAccessBase
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        public Int64 Create(String userLoginName, String programName, bool isCreateOperation)
        {
            Int64 returnValue = -1;

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            connectionString = AppConfigurationHelper.ConnectionString;

            SqlParametersGenerator generator = new SqlParametersGenerator("AdminManager_SqlParameters");

            parameters = generator.GetParameters("usp_Audit_Ref_SET_ParametersArray");

            parameters[0].Value = userLoginName;
            parameters[1].Value = programName;
            parameters[2].Value = isCreateOperation;
            parameters[3].Value = returnValue;

            storedProcedureName = "usp_Audit_Ref_SET";

            ExecuteProcedureNonQuery(connectionString, parameters, storedProcedureName);

            returnValue = Convert.ToInt64(parameters[3].Value);

            return returnValue;
        }

        public EntityAuditInfoCollection Get(Collection<Int64> entityIds, Collection<Int32> attributeIds, LocaleEnum locale, Decimal sequence, Boolean returnEntityAudit, Boolean returnAttirbuteAudit, Boolean returnOnlyLatestAudit)
        {
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            EntityAuditInfoCollection entityAuditInfoCollection = new EntityAuditInfoCollection();

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("AdminManager_SqlParameters");

                parameters = generator.GetParameters("AdminManager_AuditInfo_EntityAuditRef_Get_ParametersArray");

                #region Populate Entity table value parameters

                List<SqlDataRecord> entityList = null;

                if (entityIds != null && entityIds.Count > 0)
                {
                    entityList = new List<SqlDataRecord>();
                    SqlMetaData[] entityMetadata = generator.GetTableValueMetadata("AdminManager_AuditInfo_EntityAuditRef_Get_ParametersArray", parameters[0].ParameterName);

                    SqlDataRecord entityRecord = null;

                    foreach (Int64 entityId in entityIds)
                    {
                        entityRecord = new SqlDataRecord(entityMetadata);
                        entityRecord.SetValues(entityId);
                        entityList.Add(entityRecord);
                    }
                }

                #endregion

                #region Populate Attribute table value parameters

                List<SqlDataRecord> attributeList = null;

                if (attributeIds != null && attributeIds.Count > 0)
                {
                    attributeList = new List<SqlDataRecord>();
                    SqlMetaData[] attributeMetadata = generator.GetTableValueMetadata("AdminManager_AuditInfo_EntityAuditRef_Get_ParametersArray", parameters[1].ParameterName);

                    SqlDataRecord attrRecord = null;

                    foreach (Int32 attrId in attributeIds)
                    {
                        attrRecord = new SqlDataRecord(attributeMetadata);
                        attrRecord.SetValues(attrId);
                        attributeList.Add(attrRecord);
                    }
                }

                #endregion

                #region Populate Locale table value parameters

                Collection<LocaleEnum> locales = new Collection<LocaleEnum>();
                locales.Add(locale);

                SqlMetaData[] sqlLocalesMetadata = generator.GetTableValueMetadata("AdminManager_AuditInfo_EntityAuditRef_Get_ParametersArray", parameters[2].ParameterName);
                List<SqlDataRecord> localeList = EntityDataReaderUtility.CreateLocaleTable(locales, (Int32)GlobalizationHelper.GetSystemDataLocale(), sqlLocalesMetadata);

                #endregion Populate table value parameters for Locale

                parameters[0].Value = entityList;
                parameters[1].Value = attributeList;
                parameters[2].Value = localeList;
                parameters[3].Value = sequence;
                parameters[4].Value = returnEntityAudit;
                parameters[5].Value = returnAttirbuteAudit;
                parameters[6].Value = returnOnlyLatestAudit;

                storedProcedureName = "usp_AuditRef_EntityAuditRef_Get";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        EntityAuditInfo entityAuditInfo = new EntityAuditInfo();

                        if (reader["Id"] != null)
                            entityAuditInfo.Id = ValueTypeHelper.Int64TryParse(reader["Id"].ToString(), -1);

                        if (reader["ProgramName"] != null)
                            entityAuditInfo.ProgramName = reader["ProgramName"].ToString();

                        if (reader["UserName"] != null)
                            entityAuditInfo.UserLogin = reader["UserName"].ToString();

                        if (reader["ChangeDateTime"] != null)
                            entityAuditInfo.ChangeDateTime = ValueTypeHelper.ConvertToDateTime(reader["ChangeDateTime"].ToString());

                        if (reader["EntityId"] != null)
                            entityAuditInfo.EntityId = ValueTypeHelper.Int64TryParse(reader["EntityId"].ToString(), -1);

                        if (reader["AttributeId"] != null)
                            entityAuditInfo.AttributeId = ValueTypeHelper.Int32TryParse(reader["AttributeId"].ToString(), -1);

                        if (reader["Action"] != null)
                        {
                            entityAuditInfo.Action = ValueTypeHelper.GetAction(reader["Action"].ToString());
                        }

                        if (reader["LocaleId"] != null)
                        {
                            LocaleEnum auditInfoLocale = LocaleEnum.UnKnown;
                            Enum.TryParse(reader["LocaleId"].ToString(), out auditInfoLocale);
                            entityAuditInfo.Locale = auditInfoLocale;
                        }

                        entityAuditInfoCollection.Add(entityAuditInfo);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return entityAuditInfoCollection;
        }

        public AuditInfoCollection Get(Collection<Int64> auditInfoIds)
        {
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            AuditInfoCollection auditInfoCollection = new AuditInfoCollection();

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
               
                SqlParametersGenerator generator = new SqlParametersGenerator("AdminManager_SqlParameters");

                parameters = generator.GetParameters("AdminManager_AuditInfo_Get_ParametersArray");

                #region Populate AuditInfo Ids
                List<SqlDataRecord> auditInfoIdList = null;
                if (auditInfoIds != null && auditInfoIds.Count > 0)
                {
                    auditInfoIdList = new List<SqlDataRecord>();
                    SqlMetaData[] attributeMetadata = generator.GetTableValueMetadata("AdminManager_AuditInfo_Get_ParametersArray", parameters[0].ParameterName);

                    SqlDataRecord auditRecord = null;

                    foreach (Int64 attrId in auditInfoIds)
                    {
                        auditRecord = new SqlDataRecord(attributeMetadata);
                        auditRecord.SetValues(attrId);
                        auditInfoIdList.Add(auditRecord);
                    }
                }
                parameters[0].Value = auditInfoIdList;
                #endregion

                storedProcedureName = "usp_AuditRef_Get";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        AuditInfo auditInfo = new AuditInfo();
                        if (reader["Id"] != null)
                            auditInfo.Id = ValueTypeHelper.Int64TryParse(reader["Id"].ToString(), -1);

                        if (reader["ProgramName"] != null)
                            auditInfo.ProgramName = reader["ProgramName"].ToString();

                        if (reader["UserName"] != null)
                            auditInfo.UserLogin = reader["UserName"].ToString();

                        if (reader["ChangeDateTime"] != null)
                            auditInfo.ChangeDateTime = ValueTypeHelper.ConvertToDateTime(reader["ChangeDateTime"].ToString());

                        if (reader["Action"] != null)
                        {
                            auditInfo.Action = ValueTypeHelper.GetAction(reader["Action"].ToString());
                        }

                        auditInfoCollection.Add(auditInfo);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return auditInfoCollection;
        }

        #endregion
    }
}
