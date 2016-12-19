using System;
using System.Data.SqlClient;
using System.Diagnostics;

namespace MDM.AdminManager.Data
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Utility;
    using Microsoft.SqlServer.Server;
    using System.Collections.Generic;

    /// <summary>
    /// Specifies the data access operations for attribute model
    /// </summary>
    public class BuildInfoDA : SqlClientDataAccessBase
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
        /// Gets the latest build details
        /// </summary>
        /// <returns>BuildDetail object</returns>
        public BuildDetail GetLatestBuildDetail()
        {
            SqlDataReader reader = null;

            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            BuildDetail buildDetail = null;
            try
            {
                connectionString = AppConfigurationHelper.ConnectionString;

                storedProcedureName = "usp_AdminManager_BuildDetails_Get";

                if (!String.IsNullOrWhiteSpace(AppConfigurationHelper.StoredProcedureSuffix))
                    storedProcedureName = String.Format("{0}_{1}", storedProcedureName, AppConfigurationHelper.StoredProcedureSuffix);

                reader = ExecuteProcedureReader(connectionString, null, storedProcedureName);

                if (reader != null)
                {
                    buildDetail = new BuildDetail();
                    while (reader.Read())
                    {
                        if (reader["Id"] != null)
                            buildDetail.Id = ValueTypeHelper.ConvertToInt32(reader["Id"]);
                        if (reader["BuildServer"] != null)
                            buildDetail.BuildServer = reader["BuildServer"].ToString();
                        if (reader["BuildType"] != null)
                            buildDetail.BuildType = reader["BuildType"].ToString();
                        if (reader["BuildVersion"] != null)
                            buildDetail.BuildVersion = reader["BuildVersion"].ToString();
                        if (reader["BuildUser"] != null)
                            buildDetail.BuildUser = reader["BuildUser"].ToString();

                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return buildDetail;
        }

        /// <summary>
        /// Gets the corresponding build feature Id
        /// </summary>
        /// <param name="buildDetailId">Indicates the  buildDetailId</param>
        /// <param name="featureName">Indicates the featureName</param>
        /// <returns>Int</returns>
        public Int32 GetBuildFeatureId(Int32 buildDetailId, String featureName)
        {
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            int buildFeatureId = 0;

            SqlParameter[] parameters = null;
            SqlParametersGenerator generator = new SqlParametersGenerator("AdminManager_SqlParameters");

            parameters = generator.GetParameters("AdminManager_BuildFeature_Get_ParametersArray");

            parameters[0].Value = buildDetailId;
            parameters[1].Value = featureName;
            connectionString = AppConfigurationHelper.ConnectionString;
            storedProcedureName = "usp_AdminManager_BuildFeatures_Get";
            if (!String.IsNullOrWhiteSpace(AppConfigurationHelper.StoredProcedureSuffix))
                storedProcedureName = String.Format("{0}_{1}", storedProcedureName, AppConfigurationHelper.StoredProcedureSuffix);

            Object objectvalue = ExecuteProcedureScalar(connectionString, parameters, storedProcedureName);

            buildFeatureId = ValueTypeHelper.ConvertToInt32(objectvalue);

            return buildFeatureId;
        }

        /// <summary>
        /// Add/Delete records in tb_FileCheckSum
        /// </summary>
        /// <param name="version">Indicates the BuildContext</param>
        /// <returns></returns>

        public OperationResult ProcessFileCheckSum(BuildDetailContext buildDetailContext)
        {
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            OperationResult processOperationResult = new OperationResult();

            try
            {
                SqlParameter[] parameters = null;
                SqlParametersGenerator generator = new SqlParametersGenerator("AdminManager_SqlParameters");

                parameters = generator.GetParameters("AdminManager_FileCheckSum_Process_ParametersArray");

                #region Populate profile table value parameters

                List<SqlDataRecord> fileHashTableList = new List<SqlDataRecord>();

                SqlMetaData[] hashMetadata = generator.GetTableValueMetadata("AdminManager_FileCheckSum_Process_ParametersArray", parameters[1].ParameterName);
                SqlDataRecord fileHashResultRecord = null;

                if (buildDetailContext.Action == ObjectAction.Create)
                {
                    foreach (KeyValuePair<String, String> fileHashDetails in buildDetailContext.FileHashDetails)
                    {
                        fileHashResultRecord = new SqlDataRecord(hashMetadata);
                        fileHashResultRecord.SetValue(0, fileHashDetails.Key);
                        fileHashResultRecord.SetValue(1, fileHashDetails.Value);
                        fileHashTableList.Add(fileHashResultRecord);
                    }
                }

                if (fileHashTableList.Count < 1)
                {
                    fileHashTableList = null;
                }
                #endregion Populate profile table value parameters

                parameters[0].Value = buildDetailContext.BuildFeatureId;
                parameters[1].Value = fileHashTableList;
                parameters[2].Value = buildDetailContext.FilePath;
                parameters[3].Value = buildDetailContext.BuildServer;
                parameters[4].Value = buildDetailContext.FeatureDescription;
                parameters[5].Value = Convert.ToString(buildDetailContext.Action);
                connectionString = AppConfigurationHelper.ConnectionString;

                storedProcedureName = "usp_AdminManager_FileChecksum_Process";

                if (!String.IsNullOrWhiteSpace(AppConfigurationHelper.StoredProcedureSuffix))
                {
                    storedProcedureName = String.Format("{0}_{1}", storedProcedureName, AppConfigurationHelper.StoredProcedureSuffix);
                }

                ExecuteProcedureNonQuery(connectionString, parameters, storedProcedureName);
                processOperationResult.AddOperationResult(String.Empty, String.Empty, OperationResultType.Information);
            }
            catch (Exception exception)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "ProcessFileCheckSum failed." + exception.Message);
                processOperationResult.AddOperationResult(String.Empty, exception.Message, OperationResultType.Error);
            }
            
            return processOperationResult;
        }

        /// <summary>
        /// Update Build Status 
        /// </summary>
        /// <param name="version">Indicates the BuildContext</param>
        /// <returns></returns>

        public OperationResult UpdateBuildStatus(BuildDetailContext buildDetailContext)
        {

            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            OperationResult processOperationResult = new OperationResult();
            try
            {
                SqlParameter[] parameters = null;
                SqlParametersGenerator generator = new SqlParametersGenerator("AdminManager_SqlParameters");

                parameters = generator.GetParameters("AdminManager_BuildStatus_Process_ParametersArray");

                parameters[0].Value = buildDetailContext.Feature;
                parameters[1].Value = buildDetailContext.FeatureDescription;
                parameters[2].Value = buildDetailContext.BuildType;
                parameters[3].Value = buildDetailContext.BuildServer;
                parameters[4].Value = Convert.ToString(buildDetailContext.Action);
                connectionString = AppConfigurationHelper.ConnectionString;

                storedProcedureName = "usp_AdminManager_BuildDetails_Process";

                if (!String.IsNullOrWhiteSpace(AppConfigurationHelper.StoredProcedureSuffix))
                    storedProcedureName = String.Format("{0}_{1}", storedProcedureName, AppConfigurationHelper.StoredProcedureSuffix);

                ExecuteProcedureNonQuery(connectionString, parameters, storedProcedureName);
                processOperationResult.AddOperationResult(String.Empty, String.Empty, OperationResultType.Information);
            }
            catch (Exception exception)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "UpdateBuildStatus Failed." + exception.Message);
                processOperationResult.AddOperationResult(String.Empty, exception.Message, OperationResultType.Error);

            }
            return processOperationResult;
        }

        /// <summary>
        /// SaveBuildDetails
        /// </summary>
        /// <param name="buildDetailContext">Indicates the BuildContext</param>
        /// <returns>Results of the operation having errors and information if any</returns>
        public OperationResult SaveBuildDetails(BuildDetailContext buildDetailContext)
        {
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            OperationResult processOperationResult = new OperationResult();
            try
            {
                SqlParameter[] parameters = null;
                SqlParametersGenerator generator = new SqlParametersGenerator("AdminManager_SqlParameters");

                parameters = generator.GetParameters("AdminManager_BuildDetails_Save_ParametersArray");

                parameters[0].Value = buildDetailContext.Version;
                parameters[1].Value = buildDetailContext.Feature;
                parameters[2].Value = buildDetailContext.FeatureDescription;
                parameters[3].Value = buildDetailContext.CoreError;
                parameters[4].Value = buildDetailContext.MdmCenterLog;
                parameters[5].Value = buildDetailContext.WorkFlowError;
                parameters[6].Value = buildDetailContext.WorkFlowErrorLog;
                parameters[7].Value = buildDetailContext.VpError;
                parameters[8].Value = buildDetailContext.VpErrorLog;
                parameters[9].Value = buildDetailContext.BuildType;
                parameters[10].Value = buildDetailContext.BuildUser;
                parameters[11].Value = buildDetailContext.BuildServer;
                connectionString = AppConfigurationHelper.ConnectionString;

                storedProcedureName = "usp_Store_Build_Details";

                if (!String.IsNullOrWhiteSpace(AppConfigurationHelper.StoredProcedureSuffix))
                    storedProcedureName = String.Format("{0}_{1}", storedProcedureName, AppConfigurationHelper.StoredProcedureSuffix);

                ExecuteProcedureNonQuery(connectionString, parameters, storedProcedureName);
                processOperationResult.AddOperationResult(String.Empty, String.Empty, OperationResultType.Information);

            }
            catch (Exception exception)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "Save Build Details Failed." + exception.Message);
                processOperationResult.AddOperationResult(String.Empty, exception.Message, OperationResultType.Error);

            }
            return processOperationResult;


        }

        public RoleType GetUserRoleType(string userName)
        {
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            var roleType = RoleType.Unknown;
            try
            {
               
                SqlParameter[] parameters = null;
                SqlParametersGenerator generator = new SqlParametersGenerator("AdminManager_SqlParameters");

                parameters = generator.GetParameters("AdminManager_BuildDetails_Get_UserPermissionForDB");

                parameters[0].Value = userName;
                connectionString = AppConfigurationHelper.ConnectionString;

                storedProcedureName = "usp_AdminManager_BuildUserRole_Get ";

                if (!String.IsNullOrWhiteSpace(AppConfigurationHelper.StoredProcedureSuffix))
                    storedProcedureName = String.Format("{0}_{1}", storedProcedureName, AppConfigurationHelper.StoredProcedureSuffix);

                Object objectvalue = ExecuteProcedureScalar(connectionString, parameters, storedProcedureName);

                Enum.TryParse<RoleType>(Convert.ToString(objectvalue), out roleType);
            }
            catch (Exception ex)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "GetUserRoleType Failed." + ex.Message);
            }
            return roleType;
        }

        #endregion

        #region Private Methods





        #endregion

        #endregion
    }
}
