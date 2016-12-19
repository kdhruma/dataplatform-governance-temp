using System;
using System.Data.SqlClient;

namespace MDM.IntegrationManager.Data
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Integration;
    using MDM.Core;
    using MDM.Utility;

    /// <summary>
    /// Specifies IntegrationItemDimensionType data access
    /// </summary>
    public class IntegrationItemDimensionTypeDA : SqlClientDataAccessBase
    {
        #region Fields

        private const String _getMethodName = "MDM.IntegrationManager.Data.IntegrationItemDimensionTypeDA.Get";

        #endregion Fields

        /// <summary>
        /// Get IntegrationItemDimensionTypes
        /// </summary>
        /// <param name="command">Connection related properties</param>
        /// <returns>IntegrationItemDimensionTypes</returns>
        public IntegrationItemDimensionTypeCollection Get(Int32 integrationItemDimensionTypeId, String integrationItemDimensionTypeName, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_getMethodName, MDMTraceSource.Integration, false);
            }

            SqlDataReader reader = null;
            SqlParameter[] parameters;
            IntegrationItemDimensionTypeCollection integrationItemDimensionTypes = new IntegrationItemDimensionTypeCollection();
            String storedProcedureName = "usp_IntegrationManager_IntegrationItemDimensionType_Get";

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("IntegrationManager_SqlParameters");

                parameters = generator.GetParameters("IntegrationManager_IntegrationItemDimensionType_Get_ParametersArray");

                parameters[0].Value = integrationItemDimensionTypeId;
                parameters[1].Value = integrationItemDimensionTypeName;

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                integrationItemDimensionTypes = ReadIntegrationItemDimensionTypes(reader);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity(_getMethodName, MDMTraceSource.Integration);
                }
            }
            return integrationItemDimensionTypes;
        }

        #region Private method

        private IntegrationItemDimensionTypeCollection ReadIntegrationItemDimensionTypes(SqlDataReader reader)
        {
            IntegrationItemDimensionTypeCollection integrationItemDimensionTypeCollection = new IntegrationItemDimensionTypeCollection();

            if (reader != null)
            {
                while (reader.Read())
                {
                    #region Declaration

                    Int32 id = -1;
                    String name = String.Empty;
                    String longName = String.Empty;
                    Int16 connectorId = -1;

                    #endregion Declaration

                    #region Read properties

                    if (reader["PK_Integration_ItemDimensionType"] != null)
                        id = ValueTypeHelper.Int32TryParse(reader["PK_Integration_ItemDimensionType"].ToString(), -1);

                    if (reader["ShortName"] != null)
                        name = reader["ShortName"].ToString();

                    if (reader["LongName"] != null)
                        longName = reader["LongName"].ToString();

                    if (reader["FK_Connector"] != null)
                        connectorId = ValueTypeHelper.Int16TryParse(reader["FK_Connector"].ToString(), -1);

                    #endregion Read properties

                    #region Initialize object

                    IntegrationItemDimensionType integrationItemDimensionType = new IntegrationItemDimensionType();
                    integrationItemDimensionType.Id = id;
                    integrationItemDimensionType.Name = name;
                    integrationItemDimensionType.LongName = longName;
                    integrationItemDimensionType.ConnectorId = connectorId;

                    #endregion Initialize object

                    integrationItemDimensionTypeCollection.Add(integrationItemDimensionType);
                }
            }
            return integrationItemDimensionTypeCollection;
        }

        #endregion Private method

    }
}
