using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get role mapping related information.
    /// </summary>
    public interface IRoleMapping : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Field specifying the external role of role mapping
        /// </summary>
        String ExternalRole { get; set; }

        /// <summary>
        /// Field specifying the mdm roles of role mapping
        /// </summary>
        Collection<String> MDMRoles { get; set; }

        #endregion

        #region Methods

        #region ToXml methods

        /// <summary>
        /// Get XML representation of Role Mapping
        /// </summary>
        /// <returns>XML representation of Role Mapping</returns>
        String ToXml();

        #endregion ToXml methods

        #endregion Methods
    }
}
