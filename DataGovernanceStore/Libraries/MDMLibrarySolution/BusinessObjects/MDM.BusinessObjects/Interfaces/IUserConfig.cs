using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace MDM.Interfaces
{
    using MDM.Core;
    /// <summary>
    /// Exposes methods or properties to set or get the user configuration.
    /// </summary>
    public interface IUserConfig : IMDMObject
    {
        /// <summary>
        /// Property denoting the UserConfigTypeId of the User
        /// </summary>
        Int32 UserConfigTypeId { get; set; }
       
        /// <summary>
        /// Property denoting the SecurityId of User
        /// </summary>
        Int32 SecurityUserId { get; set; }
       
        /// <summary>
        /// Property denoting the Organization Id of the User
        /// </summary>
        Int32 OrgId { get; set; }
       
        /// <summary>
        /// Property denoting the configXml of the User
        /// </summary>
        String ConfigXml { get; set; }
      
       
    }
}
