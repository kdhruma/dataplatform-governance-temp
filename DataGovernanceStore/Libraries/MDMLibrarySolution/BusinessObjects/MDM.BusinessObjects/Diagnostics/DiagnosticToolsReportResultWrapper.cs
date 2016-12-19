using System;
using System.Runtime.Serialization;


namespace MDM.BusinessObjects.Diagnostics
{

    /// <summary>
    /// wrapper class to wrap returned excel file and operation result logging purpose
    /// </summary>
    [DataContract]
     public class DiagnosticToolsReportResultWrapper
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private File _resultExcelFile = new File();

        /// <summary>
        /// 
        /// </summary>
        private OperationResult _operationResult = new OperationResult();

        /// <summary>
        /// 
        /// </summary>
        private Int32 _fileId = -1;

        
        #endregion Fields


        #region Properties

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public File File
        {
            get { return _resultExcelFile ;  }
            set { _resultExcelFile = value ; }
        }


        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public OperationResult OperationResult
        {
            get { return _operationResult; }
            set { _operationResult = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Int32 Fileid
        {
            get { return _fileId; }
            set { _fileId = value; }
        }

        #endregion Properties


        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public DiagnosticToolsReportResultWrapper()
        {
            

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resultFile"></param>
        /// <param name="apiOperationResult"></param>
        /// <param name="fileId"></param>
        public DiagnosticToolsReportResultWrapper(File resultFile, OperationResult apiOperationResult, Int32 fileId)
        {
            _operationResult = apiOperationResult;
            _resultExcelFile = resultFile;
            _fileId = fileId;

        }

        #endregion Constructors


        #region Public Methods


        #endregion Public Methods


        #region Private Methods



        #endregion Private Methods


    }
}
