using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Transactions;
using System.Collections.ObjectModel;

namespace MDM.AdminManager.Data
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.Utility;

    /// <summary>
    /// Specifies the data access operations for attribute model
    /// </summary>
    public class DataSecurityDA : SqlClientDataAccessBase
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
        /// Gets the permissions for the requested MDM object if provided else gets all the permissions in the requested context
        /// </summary>
        /// <param name="mdmObjectId">Id of the MDM object</param>
        /// <param name="mdmObjectType">Type of the MDM object</param>
        /// <param name="context">Context under which permissions needs to be get</param>
        /// <returns>Collection of permissions</returns>
        public PermissionCollection GetPermissions(Int32 mdmObjectId, String mdmObjectType, PermissionContext context)
        {
            SqlDataReader reader = null;
            Permission permission = null;
            PermissionCollection permissionCollection = new PermissionCollection();

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            try
            {
                connectionString = AppConfigurationHelper.ConnectionString;

                SqlParametersGenerator generator = new SqlParametersGenerator("AdminManager_SqlParameters");

                parameters = generator.GetParameters("AdminManager_DataSecurity_Permissions_Get_ParametersArray");

                if (mdmObjectId > 0 && !String.IsNullOrEmpty(mdmObjectType))
                    SetContextWithMDMObjectId(mdmObjectId, mdmObjectType, context);

                parameters[0].Value = mdmObjectType;
                parameters[1].Value = mdmObjectId;
                parameters[2].Value = context.RoleId;
                parameters[3].Value = context.UserId;
                parameters[4].Value = 0;
                parameters[5].Value = 0;
                parameters[6].Value = 0;
                parameters[7].Value = 0;
                parameters[8].Value = 0;
                parameters[9].Value = 0;
                parameters[10].Value = 0;
                parameters[11].Value = 0;

                storedProcedureName = "usp_AdminManager_DataSecurity_Permissions_Get";

                if (!String.IsNullOrWhiteSpace(AppConfigurationHelper.StoredProcedureSuffix))
                    storedProcedureName = String.Format("{0}_{1}", storedProcedureName, AppConfigurationHelper.StoredProcedureSuffix);

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        permission = PopulatePermissionObject(reader);
                        permissionCollection.Add(permission);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return permissionCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vchrTargetUserLogin"></param>
        /// <param name="vchrUserLogin"></param>
        /// <param name="FK_Org"></param>
        /// <param name="FK_Locale"></param>
        /// <param name="intCountFrom"></param>
        /// <param name="intCountTo"></param>
        /// <param name="vchrSortColumn"></param>
        /// <param name="vchrSearchColumn"></param>
        /// <param name="vchrSearchParameter"></param>
        /// <param name="PK_Catalog"></param>
        /// <param name="IncludeTaxonomy"></param>
        /// <param name="IncludeDynamicTaxonomy"></param>
        /// <param name="IncludeCatalog"></param>
        /// <param name="IncludeView"></param>
        /// <param name="IncludeProduction"></param>
        /// <param name="IncludeDraft"></param>
        /// <returns></returns>
        public String GetDataSecurityGetCatalogPermissionsByOrg(String vchrTargetUserLogin, String vchrUserLogin, Int32 FK_Org, Int32 FK_Locale, Int32 intCountFrom, Int32 intCountTo, String vchrSortColumn, String vchrSearchColumn, String vchrSearchParameter, Int32 PK_Catalog, Boolean IncludeTaxonomy, Boolean IncludeDynamicTaxonomy, Boolean IncludeCatalog, Boolean IncludeView, Boolean IncludeProduction, Boolean IncludeDraft)
        {
            StringBuilder securityDataXML = new StringBuilder();

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("AdminManager_SqlParameters");

                parameters = generator.GetParameters("AdminManager_DataSecurity_GetDataSecurityGetCatalogPermissionsByOrg_ParametersArray");

                parameters[0].Value = vchrTargetUserLogin;
                parameters[1].Value = vchrUserLogin;
                parameters[2].Value = FK_Org;
                parameters[3].Value = FK_Locale;
                parameters[4].Value = intCountFrom;
                parameters[5].Value = intCountTo;
                parameters[6].Value = vchrSortColumn;
                parameters[7].Value = vchrSearchColumn;
                parameters[8].Value = vchrSearchParameter;
                parameters[9].Value = PK_Catalog;
                parameters[10].Value = IncludeTaxonomy;
                parameters[11].Value = IncludeDynamicTaxonomy;
                parameters[12].Value = IncludeCatalog;
                parameters[13].Value = IncludeView;
                parameters[14].Value = IncludeProduction;
                parameters[15].Value = IncludeDraft;

                storedProcedureName = "usp_Sec_Catalog_getCatalogPermissionsByOrg_XML";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        securityDataXML.Append(reader[0]);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return securityDataXML.ToString();
        }

        #endregion

        #region Private Methods

        private Permission PopulatePermissionObject(SqlDataReader reader)
        {
            Permission permission = null;

            Int32 objectId = 0;
            Int32 objectTypeId = 0;
            String objectType = String.Empty;
            Int32 roleId = 0;
            Int32 userId = 0;
            Int32 orgId = 0;
            Int32 containerId = 0;
            Int64 categoryId = 0;
            Int32 entityTypeId = 0;
            Int64 entityId = 0;
            Int32 relationshipTypeId = 0;
            Int32 attributeId = 0;
            Int32 localeId = 0;
            Int32 sequence = 0;
            Collection<UserAction> permissionSet = new Collection<UserAction>();

            if (reader["ObjectId"] != null)
                Int32.TryParse(reader["ObjectId"].ToString(), out objectId);
            if (reader["ObjectTypeId"] != null)
                Int32.TryParse(reader["ObjectTypeId"].ToString(), out objectTypeId);
            if (reader["ObjectType"] != null)
                objectType = reader["ObjectType"].ToString();
            if (reader["CanAdd"] != null)
            {
                Int32 canAdd = 0;
                Int32.TryParse(reader["CanAdd"].ToString(), out canAdd);

                if (canAdd == 1)
                    permissionSet.Add(UserAction.Add);
            }
            if (reader["CanDelete"] != null)
            {
                Int32 canDelete = 0;
                Int32.TryParse(reader["CanDelete"].ToString(), out canDelete);

                if (canDelete == 1)
                    permissionSet.Add(UserAction.Delete);
            }
            if (reader["CanView"] != null)
            {
                Int32 canView = 0;
                Int32.TryParse(reader["CanView"].ToString(), out canView);

                if (canView == 1)
                    permissionSet.Add(UserAction.View);
            }
            if (reader["CanUpdate"] != null)
            {
                Int32 canUpdate = 0;
                Int32.TryParse(reader["CanUpdate"].ToString(), out canUpdate);

                if (canUpdate == 1)
                    permissionSet.Add(UserAction.Update);
            }
            if (reader["CanExport"] != null)
            {
                Int32 canExport = 0;
                Int32.TryParse(reader["CanExport"].ToString(), out canExport);

                if (canExport == 1)
                    permissionSet.Add(UserAction.Export);
            }
            if (reader["CanExecute"] != null)
            {
                Int32 canExecute = 0;
                Int32.TryParse(reader["CanExecute"].ToString(), out canExecute);

                if (canExecute == 1)
                    permissionSet.Add(UserAction.Execute);
            }
            if (reader["CanSubscribe"] != null)
            {
                Int32 canSubscribe = 0;
                Int32.TryParse(reader["CanSubscribe"].ToString(), out canSubscribe);

                if (canSubscribe == 1)
                    permissionSet.Add(UserAction.Subscribe);
            }

            if (reader["CanSchedule"] != null)
            {
                Int32 canSchedule = 0;
                Int32.TryParse(reader["CanSchedule"].ToString(), out canSchedule);

                if (canSchedule == 1)
                    permissionSet.Add(UserAction.Schedule);
            }

            if (reader["CanImport"] != null)
            {
                Int32 canImport = 0;
                Int32.TryParse(reader["CanImport"].ToString(), out canImport);

                if (canImport == 1)
                    permissionSet.Add(UserAction.Import);
            }

            if (reader["RoleId"] != null)
                Int32.TryParse(reader["RoleId"].ToString(), out roleId);
            if (reader["OrgId"] != null)
                Int32.TryParse(reader["OrgId"].ToString(), out orgId);
            if (reader["ContainerId"] != null)
                Int32.TryParse(reader["ContainerId"].ToString(), out containerId);
            if (reader["CategoryId"] != null)
                Int64.TryParse(reader["CategoryId"].ToString(), out categoryId);
            if (reader["EntityTypeId"] != null)
                Int32.TryParse(reader["EntityTypeId"].ToString(), out entityTypeId);
            if (reader["EntityId"] != null)
                Int64.TryParse(reader["EntityId"].ToString(), out entityId);
            if (reader["AttributeId"] != null)
                Int32.TryParse(reader["AttributeId"].ToString(), out attributeId);
            if (reader["RelationshipTypeId"] != null)
                Int32.TryParse(reader["RelationshipTypeId"].ToString(), out relationshipTypeId);
            if (reader["LocaleId"] != null)
                Int32.TryParse(reader["LocaleId"].ToString(), out localeId);
            if (reader["Sequence"] != null)
                Int32.TryParse(reader["Sequence"].ToString(), out sequence);

            PermissionContext perContext = new PermissionContext(orgId, containerId, categoryId, entityTypeId, relationshipTypeId, entityId, attributeId, roleId, userId, localeId);

            permission = new Permission(objectId, objectTypeId, objectType, permissionSet, perContext, sequence);

            return permission;
        }

        private void SetContextWithMDMObjectId(Int32 mdmObjectId, String mdmObjectType, PermissionContext context)
        {
            switch (mdmObjectType)
            {
                case "Organization":
                    context.OrgId = mdmObjectId;
                    break;
                case "Container":
                    context.ContainerId = mdmObjectId;
                    break;
                case "Category":
                    context.CategoryId = mdmObjectId;
                    break;
                case "EntityType":
                    context.EntityTypeId = mdmObjectId;
                    break;
                case "RelationshipTypeId":
                    context.RelationshipTypeId = mdmObjectId;
                    break;
                case "Entity":
                    context.EntityId = mdmObjectId;
                    break;
                case "Attribute":
                    context.AttributeId = mdmObjectId;
                    break;
                case "Role":
                    context.RoleId = mdmObjectId;
                    break;
                case "User":
                    context.UserId = mdmObjectId;
                    break;
                case "Locale":
                    context.LocaleId = mdmObjectId;
                    break;
            }
        }

        #endregion

        #endregion
    }
}
