using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods used for setting configuration options for qualifying queue.
    /// </summary>
    public interface IQualificationConfiguration
    {
        #region Methods

        /// <summary>
        /// Represents QualificationConfiguration in Xml format
        /// </summary>
        String ToXml();

        /// <summary>
        /// Clone the QualificationConfiguration object and return new instance with the same values.
        /// </summary>
        /// <returns>New cloned IQualificationConfiguration</returns>
        IQualificationConfiguration Clone();

        /// <summary>
        /// Get schedule criteria for qualification
        /// </summary>
        /// <returns></returns>
        IScheduleCriteria GetScheduleCriteria();

        #endregion Methods
    }
}
