using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods used for setting configuration options for processing (outbound) queue.
    /// </summary>
    public interface IProcessingConfiguration
    {
        #region Methods

        /// <summary>
        /// Represents ProcessingConfiguration in Xml format
        /// </summary>
        String ToXml();

        /// <summary>
        /// Clone the ProcessingConfiguration object and return new instance with the same values.
        /// </summary>
        /// <returns>New cloned IProcessingConfiguration</returns>
        IProcessingConfiguration Clone();

        /// <summary>
        /// Get schedule criteria for qualification
        /// </summary>
        /// <returns></returns>
        IScheduleCriteria GetScheduleCriteria();

        #endregion Methods
    }
}
