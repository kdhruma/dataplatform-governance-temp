using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get the file type, file data, and file is archive or not information.
    /// </summary>
    public interface IFile : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property denoting file type
        /// </summary>
        String FileType { get; set; }

        /// <summary>
        /// Property denoting file data
        /// </summary>
        Byte[] FileData { get; set; }

        /// <summary>
        /// Property denoting file is archive or not
        /// </summary>
        Boolean IsArchive { get; set; }

        #endregion

    }
}
