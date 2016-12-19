using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.BusinessObjects.Jobs
{
    /// <summary>
    /// job execution status
    /// </summary>
    public class JobOperationStatus
    {
        /// <summary>
        /// Operation status
        /// </summary>
        public String Status { get; set; }

        /// <summary>
        /// Total progress
        /// </summary>
        public Int32 ProgressValue { get; set; }
    }
}
