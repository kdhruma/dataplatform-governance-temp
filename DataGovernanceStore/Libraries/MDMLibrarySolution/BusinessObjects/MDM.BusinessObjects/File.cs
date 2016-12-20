using System;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;
    
    /// <summary>
    /// Specifies a File
    /// </summary>
    [DataContract]
    public class File: MDMObject , IFile
    {

        #region Fields

        /// <summary>
        /// Field denoting Type of the File
        /// </summary>
        private String _type = String.Empty;

        /// <summary>
        /// Field denoting File Data
        /// </summary>
        private Byte[] _data = null;

        /// <summary>
        /// Field denoting IsArchive. If file is archived
        /// </summary>
        private Boolean _isArchive = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public File()
            :base()
        {
        }

        /// <summary>
        /// Creates <see cref="MDM.BusinessObjects.File"/> instance using specified data
        /// </summary>
        /// <param name="fileName">Contains Name of File</param>
        /// <param name="fileType">Indicates Mime Type of file</param>
        /// <param name="isArchive">Indicates archive flag of the file</param>
        /// <param name="fileData">Contains content of file in byte array</param>
        public File(String fileName, String fileType, Boolean isArchive, byte[] fileData)
            :base(0, fileName, fileName)
        {
            this.IsArchive = isArchive;
            this.FileType = fileType;
            this.FileData = fileData;
        }

        /// <summary>
        /// Creates <see cref="MDM.BusinessObjects.File"/> instance using specified data
        /// </summary>
        /// <param name="fileId">Indicates file record primary key</param>
        /// <param name="fileName">Contains Name of File</param>
        /// <param name="fileType">Indicates Mime Type of file</param>
        /// <param name="isArchive">Indicates archive flag of the file</param>
        /// <param name="fileData">Contains content of file in byte array</param>
        public File(Int32 fileId, String fileName, String fileType, Boolean isArchive, byte[] fileData)
            : base(fileId, fileName, fileName)
        {
            this.IsArchive = isArchive;
            this.FileType = fileType;
            this.FileData = fileData;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting the File Type
        /// </summary>
        [DataMember]
        public String FileType
        {
            get { return this._type; }
            set { this._type = value; }
        }

        /// <summary>
        /// Property denoting the Data Content of the File
        /// </summary>
        [DataMember]
        public Byte[] FileData
        {
            get { return this._data; }
            set { this._data = value; }
        }

        /// <summary>
        /// Property denoting the IsArchive of the File. If file is archived
        /// </summary>
        [DataMember]
        public Boolean IsArchive
        {
            get { return this._isArchive; }
            set { this._isArchive = value; }
        }


        #endregion
    }
}
