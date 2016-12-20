using System;
using System.IO;

namespace MDM.DiagnosticManager.TraceListeners
{
    /// <summary>
    /// The Stream class which would be used to write to the log file
    /// </summary>
    /// <remarks>This class should not throw any exceptions</remarks>
    public class RollingStream : Stream
    {
        #region Fields

        private FileStream _fStream = null;        
        private long _dataWritten = 0;
        private long _fileQuota = 0;
        private int _rollingSequence = 1;
        private String _filePath = String.Empty;
        private String _fileBase = String.Empty;
        private String _fileExt = String.Empty;

        #endregion

        #region Constructors
        /// <summary>
        /// The constructor which the Service Model automatically calls with the file
        /// specified in the initializeData 
        /// </summary>
        /// <param name="FileName"></param>
        public RollingStream(string FileName)
        {
            try
            {
                String rollingFilePath = String.Empty;
                String fileBaseRolling = String.Empty;

                _filePath = Path.GetDirectoryName(FileName);
                _fileBase = Path.GetFileNameWithoutExtension(FileName);
                _fileExt = Path.GetExtension(FileName);
                
                //This logic is only when a file name is give as opposed to a full path
                //This gives without the bin in web application and with the bin in windows applications
                if (string.IsNullOrEmpty(_filePath))
                {
                    _filePath = AppDomain.CurrentDomain.BaseDirectory;
                }

                rollingFilePath = GetRollingFilePath();

                _fStream = new FileStream(rollingFilePath, FileMode.Create);
            }
            catch
            {
            }

        }


        #endregion

        #region Properties
        /// <summary>
        /// The maximum file size the stream can use to write the data
        /// </summary>
        public long MaxQuotaSize
        {
            get
            {
                return _fileQuota;
            }
            set
            {
                _fileQuota = value;
            }
        }

        /// <summary>
        /// Check if the Quota is reached
        /// </summary>
        public bool IsOverQuota
        {
            get
            {
                return (_dataWritten >= _fileQuota);
            }

        }

        #region Stream Properties that needs to be overridden
        /// <summary>
        /// Refer to the Stream documentation
        /// </summary>
        public override bool CanRead
        {
            get
            {
                try
                {
                    return _fStream.CanRead;
                }
                catch
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Refer to the Stream documentation
        /// </summary>
        public override bool CanSeek
        {
            get
            {
                try
                {
                    return _fStream.CanSeek;
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Refer to the Stream documentation
        /// </summary>
        public override long Length
        {
            get
            {
                try
                {
                    return _fStream.Length;
                }
                catch
                {
                    return -1;
                }
            }
        }

        /// <summary>
        /// Refer to the Stream documentation
        /// </summary>
        public override long Position
        {
            get
            {
                try
                {
                    return _fStream.Position;
                }
                catch
                {
                    return -1;
                }
            }
            set
            {
                try
                {
                    _fStream.Position = Position;
                }
                catch
                { 
                }
            }
        }

        /// <summary>
        /// Refer to the Stream documentation
        /// </summary>
        public override bool CanWrite
        {
            get
            {
                try
                {
                    return _fStream.CanWrite;
                }
                catch
                {
                    return true;
                }
            }
        }

        #endregion

        #endregion

        #region Methods
        /// <summary>
        /// Once the Quota is done, this method is used to roll over the log
        /// </summary>
        public void NextFile()
        {
            try
            {
                String rollingFilePath = String.Empty;
                //Close current file, open next file (deleting its contents)

                _dataWritten = 0;
                _fStream.Close();

                rollingFilePath = GetRollingFilePath();

                _fStream = new FileStream(rollingFilePath, FileMode.Create);
            }
            catch
            { 
            }
        }
        /// <summary>
        /// This provides the filename along with the full file path for the rolling log
        /// </summary>
        /// <returns>The configured file name appended by the current date in YYYY-MM-DD format followed by _ and the rolling number starting with 1</returns>
        /// <example>c:\logs\MDMCenterService_2012-11-03-12-56-10_1.svclog</example>
        private String GetRollingFilePath()
        {
            String fileBaseRolling = String.Empty;
            String rollingFilePath = String.Empty;

            fileBaseRolling = String.Concat(_fileBase, "_", DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss"), "_", _rollingSequence);

            _rollingSequence++;

            rollingFilePath = Path.Combine(_filePath, fileBaseRolling + _fileExt);

            return rollingFilePath;
        }

        #region Stream Methods that needs to be overridden
        /// <summary>
        /// Refer to the Stream documentation
        /// </summary>
        public override void Flush()
        {
            try
            {
                _fStream.Flush();
            }
            catch
            { 
            }
        }

        /// <summary>
        /// Refer to the Stream documentation
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            try
            {
                return _fStream.Seek(offset, origin);
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// Refer to the Stream documentation
        /// </summary>
        /// <param name="value"></param>
        public override void SetLength(long value)
        {
            try
            {
                _fStream.SetLength(value);
            }
            catch
            { 
            }
        }

        /// <summary>
        /// Refer to the stream documentation
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            try
            {
                
                //Write to the file but at the same time make use of the byte count property
                //to calculate the size so that we can check if the quota is reached than having
                //to depend on OS to provide the file info details
                _fStream.Write(buffer, offset, count);
                _dataWritten += count;

            }
            catch
            {
            }
        }

        /// <summary>
        /// Refer to the stream documentation
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            try
            {
                return _fStream.Read(buffer, offset, count);
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// Refer to the stream documentation
        /// </summary>
        public override void Close()
        {
            try
            {
                _fStream.Close();
            }
            catch
            {
            }
        }

        #endregion


        #endregion
    }
}
