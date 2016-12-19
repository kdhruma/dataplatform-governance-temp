using System;
using System.Data.SqlClient;

namespace MDM.DataModelManager.Data
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Utility;

    /// <summary>
    /// Specifies the data access operations for data model exclusion context
    /// </summary>
    public class DataModelExclusionContextDA : SqlClientDataAccessBase
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets All Data Model exclusion contexts
        /// </summary>
        /// <param name="command">Specifies DB Connection information</param>
        /// <returns>Data Model exclusion contexts</returns>
        public DataModelExclusionContextCollection Get(DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("DataModelExclusionContextDA.Get", MDMTraceSource.DataModel, false);
            }

            SqlDataReader reader = null;
            String storedProcedureName = String.Empty;
            DataModelExclusionContextCollection attributeModelExtendedProperties = null;

            try
            {
                storedProcedureName = "usp_DataModelManager_ServiceExclusionContext_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, null, storedProcedureName);

                if (reader != null)
                {
                    attributeModelExtendedProperties = ReadDataModelExclusionContexts(reader);
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("DataModelExclusionContextDA.Get", MDMTraceSource.DataModel);
                }
            }

            return attributeModelExtendedProperties;
        }

        #endregion

        #region Private Methods

        private DataModelExclusionContextCollection ReadDataModelExclusionContexts(SqlDataReader reader)
        {
            DataModelExclusionContextCollection attributeModelExtendedProperties = new DataModelExclusionContextCollection();

            while (reader.Read())
            {
                DataModelExclusionContext dataModelExclusionContext = new DataModelExclusionContext();

                if (reader["ServiceExclusionContextId"] != null)
                {
                    dataModelExclusionContext.Id = ValueTypeHelper.Int32TryParse(reader["ServiceExclusionContextId"].ToString(), dataModelExclusionContext.Id);
                }

                if (reader["ServiceType"] != null)
                {
                    MDMServiceType denormServiceType = MDMServiceType.UnKnown;
                    Enum.TryParse(reader["ServiceType"].ToString(), out denormServiceType);
                    dataModelExclusionContext.ServiceType = denormServiceType;
                }

                if (reader["OrgId"] != null)
                {
                    dataModelExclusionContext.OrganizationId = ValueTypeHelper.Int32TryParse(reader["OrgId"].ToString(), dataModelExclusionContext.OrganizationId);
                }

                if (reader["ContainerId"] != null)
                {
                    dataModelExclusionContext.ContainerId = ValueTypeHelper.Int32TryParse(reader["ContainerId"].ToString(), dataModelExclusionContext.ContainerId);
                }

                if (reader["EntityTypeId"] != null)
                {
                    dataModelExclusionContext.EntityTypeId = ValueTypeHelper.Int32TryParse(reader["EntityTypeId"].ToString(), dataModelExclusionContext.EntityTypeId);
                }

                if (reader["AttributeId"] != null)
                {
                    dataModelExclusionContext.AttributeId = ValueTypeHelper.Int32TryParse(reader["AttributeId"].ToString(), dataModelExclusionContext.AttributeId);
                }

                if (reader["LocaleId"] != null)
                {
                    Int32 localeId = ValueTypeHelper.Int32TryParse(reader["LocaleId"].ToString(), 0);
                    dataModelExclusionContext.Locale = (LocaleEnum)localeId;
                }

                attributeModelExtendedProperties.Add(dataModelExclusionContext);
            }

            return attributeModelExtendedProperties;
        }

        #endregion
    }
}
