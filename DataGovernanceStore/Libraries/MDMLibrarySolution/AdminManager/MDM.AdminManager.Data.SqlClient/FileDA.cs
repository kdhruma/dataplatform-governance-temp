using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using Microsoft.SqlServer.Server;

namespace MDM.AdminManager.Business
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects;

    /// <summary>
    /// This is File Data Manager
    /// </summary>
    public class FileDA : SqlClientDataAccessBase
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods
        /// <summary>
        /// Release File Reference
        /// </summary>
        /// <param name="fileId">Id of the file to remove the reference of it</param>
        public Int32 ReleaseFileReference(Int32 fileId)
        {
            SqlDataReader reader = null;
            Int32 returnValue = 0;

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("ApplicationServiceManager_SqlParameters");

                parameters = generator.GetParameters("ApplicationServiceManager_File_ReleaseFileReference_ParametersArray");

                parameters[0].Value = fileId;

                storedProcedureName = "usp_ApplicationServiceManager_File_ReleaseFileReference";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                        Int32.TryParse(reader[0].ToString(), out returnValue);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return returnValue;
        }

        /// <summary>
        /// Processing the file
        /// </summary>
        /// <param name="file">Indicates the collections of File details</param>
        /// <param name="userId">Indicates the Login user Name</param>
        /// <param name="programName">Indicates the ProgramName</param>
        /// <returns></returns>
        public Int32 Process(File file, String userId, String programName)
        {
            SqlDataReader reader = null;
            Int32 returnValue = 0;

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("ApplicationServiceManager_SqlParameters");

                parameters = generator.GetParameters("ApplicationServiceManager_File_Process_ParametersArray");

                parameters[0].Value = file.Name;
                parameters[1].Value = file.FileType;
                parameters[2].Value = file.FileData;
                parameters[3].Value = file.IsArchive;
                parameters[4].Value = userId;
                parameters[5].Value = programName;

                storedProcedureName = "usp_ApplicationServiceManager_File_Process";

                if (!String.IsNullOrWhiteSpace(AppConfigurationHelper.StoredProcedureSuffix))
                    storedProcedureName = String.Format("{0}_{1}", storedProcedureName, AppConfigurationHelper.StoredProcedureSuffix);

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                        Int32.TryParse(reader[0].ToString(), out returnValue);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return returnValue;
        }

        /// <summary>
        /// Returns file using specified id (<paramref name="fileId"/>).
        /// </summary>
        /// <param name="fileId">Indicates id of file</param>
        /// <param name="getOnlyFileDetails">Indicates file content requesting status. Please set to True if you want only files metadata information except file content.</param>
        /// <returns></returns>
        public File GetFile(Int32 fileId, Boolean getOnlyFileDetails)
        {
            FileCollection items = GetFiles(new Collection<Int32>() {fileId}, getOnlyFileDetails);
            return (items.Count == 0) ? null : items[0];
        }

        /// <summary>
        /// Returns files using specified filter <paramref name="fileIdsFilter"/>.
        /// </summary>
        /// <param name="fileIdsFilter">Indicates ids of files. All files will be returned if filter is empty.</param>
        /// <param name="getOnlyFileMetadataExceptFileContent">Indicates file content requesting status. Please set to True if you want only files metadata information except file content.</param>
        /// <returns></returns>
        public FileCollection GetFiles(Collection<Int32> fileIdsFilter, Boolean getOnlyFileMetadataExceptFileContent)
        {
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            FileCollection result = new FileCollection();

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("ApplicationServiceManager_SqlParameters");

                parameters = generator.GetParameters("ApplicationServiceManager_File_GetFiles_ParametersArray");

                SqlMetaData[] templateMetadata = generator.GetTableValueMetadata("ApplicationServiceManager_File_GetFiles_ParametersArray", parameters[0].ParameterName);
                List<SqlDataRecord> idFilterPreparedRecords = new List<SqlDataRecord>();
                foreach (Int32 itemId in fileIdsFilter)
                {
                    SqlDataRecord preparedItem = new SqlDataRecord(templateMetadata);

                    preparedItem.SetValue(0, itemId);

                    idFilterPreparedRecords.Add(preparedItem);
                }

                if (idFilterPreparedRecords.Count != 0)
                {
                    parameters[0].Value = idFilterPreparedRecords;
                }
                parameters[1].Value = !getOnlyFileMetadataExceptFileContent;

                storedProcedureName = "usp_ApplicationServiceManager_File_Get";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    Int32 fileId;
                    String fileName;
                    String fileType;
                    Boolean isArchive;
                    Byte[] fileData;

                    while (reader.Read())
                    {
                        fileId = 0;
                        fileName = String.Empty;
                        fileType = String.Empty;
                        isArchive = false;
                        fileData = null;

                        if (reader["PK_File"] != null)
                            Int32.TryParse(reader["PK_File"].ToString(), out fileId);
                        if (reader["FileName"] != null)
                            fileName = reader["FileName"].ToString();
                        if (reader["FileType"] != null)
                            fileType = reader["FileType"].ToString();
                        if (reader["Archive"] != null)
                            Boolean.TryParse(reader["Archive"].ToString(), out isArchive);
                        if (!getOnlyFileMetadataExceptFileContent && reader["FileData"] != System.DBNull.Value && reader["FileData"] != null)
                            fileData = (Byte[])reader["FileData"];

                        result.Add(new File(fileId, fileName, fileType, isArchive, fileData));
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return result;
        }

        #endregion
    }
}