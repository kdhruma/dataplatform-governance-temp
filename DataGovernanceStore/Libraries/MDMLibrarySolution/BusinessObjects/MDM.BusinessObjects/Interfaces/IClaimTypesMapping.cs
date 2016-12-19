using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties used for providing claim types mapping related information.
    /// </summary>
    public interface IClaimTypesMapping : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Field specifying the login name of claim types mapping
        /// </summary>
        String LoginName { get; set; }

        /// <summary>
        /// Field specifying the email address of claim types mapping
        /// </summary>
        String EmailAddress { get; set; }

        /// <summary>
        /// Field specifying the external group name of claim types mapping
        /// </summary>
        String ExternalGroupName { get; set; }

        /// <summary>
        /// Field specifying the display name of claim types mapping
        /// </summary>
        String DisplayName { get; set; }
        
        #endregion

        #region Methods

        #region ToXml methods

        /// <summary>
        /// Get XML representation of Claim types Mapping
        /// </summary>
        /// <returns>XML representation of Claim types Mapping</returns>
        String ToXml();

        #endregion ToXml methods

        #endregion Methods
    }
}
