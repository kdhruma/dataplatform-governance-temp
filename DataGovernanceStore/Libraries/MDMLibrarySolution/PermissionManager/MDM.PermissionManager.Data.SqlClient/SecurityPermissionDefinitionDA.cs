using System;
using System.Data.SqlClient;
using System.Diagnostics;

namespace MDM.PermissionManager.Data
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects;

    /// <summary>
    /// Data Access for Security Permission Definition
    /// </summary>
    public class SecurityPermissionDefinitionDA : SqlClientDataAccessBase
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Get Security Permission Definition Collection from Database based on role Id
        /// </summary>
        /// <param name="roleId">Indicates the role identifier</param>
        /// <param name="command">>Object having command properties</param>
        /// <returns>Returns security permission definitions</returns>
        public SecurityPermissionDefinitionCollection Get(Int32 roleId, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("PermissionManager.Get", false);

            SecurityPermissionDefinitionCollection securityPermisionDefinitionCollection = null;

            try
            {
                SqlDataReader reader = null;
                SqlParameter[] parameters;
                String storedProcedureName = String.Empty;

                SqlParametersGenerator generator = new SqlParametersGenerator("PermissionManager_SqlParameters");

                parameters = generator.GetParameters("PermissionManager_SecurityPermissionDefinition_Get_ParametersArray");

                parameters[0].Value = roleId;

                storedProcedureName = "usp_SecurityManager_Permission_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Starting PopulateSecurityPermissionDefinition...");

                securityPermisionDefinitionCollection = PopulateSecurityPermissionDefinition(reader);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("PermissionManager.Get");
            }

            return securityPermisionDefinitionCollection;
        }

        #region Private Method

        /// <summary>
        /// Populate Security Permission Definition object from reader
        /// </summary>
        /// <param name="reader">Indicates the reader</param>
        /// <returns>Returns Security Permission Definition Collection</returns>
        private SecurityPermissionDefinitionCollection PopulateSecurityPermissionDefinition(SqlDataReader reader)
        {
            SecurityPermissionDefinition securityPermissionDefinition = null;
            SecurityPermissionDefinitionCollection securityPermisionDefinitionCollection = new SecurityPermissionDefinitionCollection();

            try
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        securityPermissionDefinition = new SecurityPermissionDefinition();

                        if (reader["ApplicationContextId"] != null)
                            securityPermissionDefinition.ApplicationContextId = ValueTypeHelper.Int32TryParse(reader["ApplicationContextId"].ToString(), 0);
                        if (reader["OrganizationId"] != null)
                            securityPermissionDefinition.ApplicationContext.OrganizationId = ValueTypeHelper.Int32TryParse(reader["OrganizationId"].ToString(), 0);
                        if (reader["OrganizationName"] != null)
                            securityPermissionDefinition.ApplicationContext.OrganizationName = reader["OrganizationName"].ToString();
                        if (reader["ContainerId"] != null)
                            securityPermissionDefinition.ApplicationContext.ContainerId = ValueTypeHelper.Int32TryParse(reader["ContainerId"].ToString(), 0);
                        if (reader["ContainerName"] != null)
                            securityPermissionDefinition.ApplicationContext.ContainerName = reader["ContainerName"].ToString();
                        if (reader["RoleId"] != null)
                            securityPermissionDefinition.ApplicationContext.RoleId = ValueTypeHelper.Int32TryParse(reader["RoleId"].ToString(), 0);
                        if (reader["RoleName"] != null)
                            securityPermissionDefinition.ApplicationContext.RoleName = reader["RoleName"].ToString();
                        if (reader["AttributeId"] != null)
                            securityPermissionDefinition.ApplicationContext.AttributeId = ValueTypeHelper.Int32TryParse(reader["AttributeId"].ToString(), 0);
                        if (reader["AttributeName"] != null)
                            securityPermissionDefinition.ApplicationContext.AttributeName = reader["AttributeName"].ToString();
                        if (reader["ContextWeightage"] != null)
                            securityPermissionDefinition.ContextWeightage = ValueTypeHelper.Int32TryParse(reader["ContextWeightage"].ToString(), 0);
                        if (reader["Name"] != null)
                            securityPermissionDefinition.Name = reader["Name"].ToString();
                        if (reader["LongName"] != null)
                            securityPermissionDefinition.LongName = reader["LongName"].ToString();
                        if (reader["ActionList"] != null)
                        {
                            String actionList = reader["ActionList"].ToString();
                            UserAction userAction = UserAction.Unknown;

                            String[] values = actionList.Split(',');

                            foreach (String value in values)
                            {
                                if (!String.IsNullOrWhiteSpace(value) && Enum.TryParse<UserAction>(value, out userAction))
                                {
                                    securityPermissionDefinition.PermissionSet.Add(userAction);
                                }
                            }
                        }

                        if (reader["ValueList"] != null)
                        {
                            String permissionValueList = reader["ValueList"].ToString();

                            String[] values = permissionValueList.Split(new String[] { "@#@" }, StringSplitOptions.None);

                            foreach (String value in values)
                            {
                                securityPermissionDefinition.PermissionValues.Add(value.Trim().ToLowerInvariant());
                            }
                        }

                        securityPermisionDefinitionCollection.Add(securityPermissionDefinition);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return securityPermisionDefinitionCollection;
        }

        #endregion

        #endregion Methods
    }
}