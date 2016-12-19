
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.AdminManager.Business
{
    using MDM.AdminManager.Data;
    using Interfaces;
    /// <summary>
    /// FileBLHelper
    /// </summary>
    public class FileBLHelper
    {
        #region Fileds

        Boolean _mock = false;
        FileBL _fileBL = new FileBL();
        IFile _file = null;

        #endregion

        #region Constructors 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mock"></param>
        public FileBLHelper(Boolean mock = false)
        {
            _mock = mock;
        }

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        public void InjectFileObject(IFile file)
        {
            _file = file;
        }
        #endregion
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public IFile GetFile(Int32 fileId)
        {
            if (!_mock)
            {
                return _fileBL.GetFile(fileId, getOnlyFileDetails: false);
            }

            return _file;
        }

        #endregion
    }
}
