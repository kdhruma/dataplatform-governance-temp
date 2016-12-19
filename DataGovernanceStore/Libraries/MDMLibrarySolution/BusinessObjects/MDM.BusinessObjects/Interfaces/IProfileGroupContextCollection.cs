using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects.DQM;

    /// <summary>
    /// Exposes methods or properties to set or get the context for collection of matching profile group. 
    /// </summary>
    public interface IProfileGroupContextCollection : IEnumerable<ProfileGroupContext>, ICollection<ProfileGroupContext>
    {
        #region Properties
        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of MatchingProfileGroup Collection
        /// </summary>
        /// <returns>Xml representation of MatchingProfileGroup Collection</returns>
        String ToXml();

        #endregion
    }
}
