using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get security user information.
    /// </summary>
    public interface ISecurityUser : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property denoting Security_UserType of current user
        /// </summary>
        Int32 SecurityUserTypeID { get; set; }

        /// <summary>
        /// Property denoting Login of current user
        /// </summary>
        String SecurityUserLogin { get; set; }

        /// <summary>
        /// Property denoting security role id
        /// </summary>
        Int32 SecurityRoleId { get; set; }

        /// <summary>
        /// Property denoting if current user is a SystemUser
        /// </summary>
        Boolean SystemUser { get; set; }

        /// <summary>
        /// Property denoting SMTP address
        /// </summary>
        String Smtp { get; set; }

        /// <summary>
        /// Property denoting Password of current user
        /// </summary>
        String Password { get; set; }

        /// <summary>
        /// Property denoting manager Id of current user
        /// </summary>
        Int32 ManagerId { get; set; }

        /// <summary>
        /// Property denoting current user's manager's login
        /// </summary>
        String ManagerLogin { get; set; }

        /// <summary>
        /// Property denoting First Name of current user
        /// </summary>
        String FirstName { get; set; }

        /// <summary>
        /// Property denoting Last Name of current user
        /// </summary>
        String LastName { get; set; }

        /// <summary>
        /// Property denoting Initials of current user
        /// </summary>
        String Initials { get; set; }

        /// <summary>
        /// Property denoting whether current user is Disabled
        /// </summary>
        Boolean Disabled { get; set; }

        /// <summary>
        /// Property denoting Authentication of User whether it is WindowsAuthentication or not
        /// </summary>
        Boolean IsWindowsAuthentication { get; set; }

        /// <summary>
        /// Represents the type of authentication for the current user
        /// </summary>
        AuthenticationType AuthenticationType { get; set; }

        /// <summary>
        /// Represents whether the user is created by system or not.
        /// </summary>
        Boolean IsSystemCreatedUser { get; set; }

        /// <summary>
        /// Represents the the SAML authentication token.
        /// </summary>
        String AuthenticationToken { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Xml representation of Security User object
        /// </summary>
        /// <returns>Xml representation of security user object</returns>
        String ToXml();

        /// <summary>
        /// Xml representation of Security User object
        /// </summary>
        /// <param name="serialization">Type of serialization</param>
        /// <returns>Xml represetation of security user object</returns>
        String ToXml(ObjectSerialization serialization);

        /// <summary>
        /// Get the mapped roles for the securityUser
        /// </summary>
        /// <returns>Return the collection of security role interface object which are mapped to the security user</returns>
        ISecurityRoleCollection GetRoles();

        /// <summary>
        /// Set the mapped roles to the security user.
        /// </summary>
        /// <param name="roles">Indicates the security role collection.</param>
        void SetRoles(ISecurityRoleCollection roles);

        /// <summary>
        /// Clone security user object
        /// </summary>
        /// <returns>cloned copy of security user object.</returns>
        ISecurityUser Clone();

        /// <summary>
        /// Delta Merge of security user
        /// </summary>
        /// <param name="deltaSecurityUser">Security user that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="returnClonedObject">true means return new merged cloned object;otherwise, same object.</param>
        /// <returns>Merged security user instance</returns>
        ISecurityUser MergeDelta(ISecurityUser deltaSecurityUser, ICallerContext iCallerContext, Boolean returnClonedObject = true);

        #endregion
    }
}
