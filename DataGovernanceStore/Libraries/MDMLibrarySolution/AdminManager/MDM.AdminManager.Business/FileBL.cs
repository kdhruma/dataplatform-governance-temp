using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Transactions;
using IO = System.IO;

namespace MDM.AdminManager.Business
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects;

    public class FileBL : BusinessLogicBase
    {
        #region Fields
         
        private FileDA _fileDA = new FileDA();
        private String _programName = " MDM.ApplicationServiceManager.Business.FileBL";
        private SecurityPrincipal _securityPrincipal = null;

        #endregion

        #region Constructors

        public FileBL()
        {
            GetSecurityPrincipal();
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Release File Reference
        /// </summary>
        /// <param name="fileId">Indicates the File Id</param>
        public Int32 ReleaseReference(Int32 fileId)
        {
            return _fileDA.ReleaseFileReference(fileId);
        }
        
        /// <summary>
        /// Processing the file
        /// </summary>
        /// <param name="file">Indicates the collections of File details</param>
        /// <param name="callerContext">Indicates the context of caller making call to this API</param>
        /// <returns></returns>
        public Int32 Process(File file, CallerContext callerContext)
        {
            Int32 returnValue = 0;
            String userName = "cfadmin";

            if (String.IsNullOrWhiteSpace(callerContext.ProgramName))
            {
                callerContext.ProgramName = this._programName;
            }
            
            if (_securityPrincipal != null)
            {
                userName = _securityPrincipal.CurrentUserName;
            }

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
            {
                returnValue = _fileDA.Process(file, userName, callerContext.ProgramName);
                transactionScope.Complete();
            }
            return returnValue;
        }

        public File GetFile(Int32 fileId, Boolean getOnlyFileDetails)
        {
            FileDA fileDA = new FileDA();
            
            File file = new File();
            file = fileDA.GetFile(fileId, getOnlyFileDetails);
            
            return file;
        }

        /// <summary>
        /// Returns files using specified filter <paramref name="fileIdsFilter"/>.
        /// </summary>
        /// <param name="fileIdsFilter">Indicates ids of files. All files will be returned if filter is empty.</param>
        /// <param name="getOnlyFileDetails">Indicates file content requesting status. Please set to True if you want only files metadata information except file content.</param>
        /// <returns>Returns collection of files</returns>
        public FileCollection GetFiles(Collection<Int32> fileIdsFilter, Boolean getOnlyFileDetails)
        {
            FileDA fileDA = new FileDA();
            return fileDA.GetFiles(fileIdsFilter, getOnlyFileDetails);
        }

        /// <summary>
        /// Uploads a given file in to the file table and gets an file id back
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public Int32 UploadFile(String filePath, CallerContext callerContext)
        {
            Int32 fileId = -1;
            String message = String.Empty;

            System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);

            if (!fileInfo.Exists)
            {
                message = String.Format("The specified input file {0} is not found and will not be processed", filePath);
                MDMTraceHelper.EmitTraceEvent(global::System.Diagnostics.TraceEventType.Error, message);
                return fileId;
            }

            FileBL fileBL = new FileBL();
            MDM.BusinessObjects.File file = new MDM.BusinessObjects.File();

            byte[] binaryData = IO.File.ReadAllBytes(filePath);

            String fileName = fileInfo.Name;

            file.Name = fileName;
            file.FileData = binaryData;
            file.IsArchive = false;
            file.Action = ObjectAction.Create;

            try
            {
                fileId = Process(file, callerContext);
            }
            catch (Exception ex)
            {
                message = String.Format("Uploading the processing file {0} to the database failed with the exception {1}. This file will not be processed", filePath, ex.Message);
                MDMTraceHelper.EmitTraceEvent(global::System.Diagnostics.TraceEventType.Error, message);
                fileId = 0;
            }

            return fileId;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Get security principal
        /// </summary>
        private void GetSecurityPrincipal()
        {
            if (_securityPrincipal == null)
            {
                _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            }
        }

        #endregion Private methods
    }
}
