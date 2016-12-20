using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get security role.
    /// </summary>
    public interface ISecurityRole : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property denoting whether current SecurityRole is PrivateRole 
        /// </summary>
        Boolean IsPrivateRole { get; set; }

        /// <summary>
        /// Property defining the type of the MDM object
        /// </summary>
        String ObjectType { get; }

        /// <summary>
        /// Property denoting the PermissionSet of SecurityRole
        /// </summary>
        Boolean PermissionSet { get; set; }

        /// <summary>
        /// Property denoting the SecurityUserTypeID of SecurityRole
        /// </summary>
        MDM.Core.SecurityUserType SecurityUserType { get; set; }

        /// <summary>
        /// Property denoting whether current SecurityRole is SystemRole 
        /// </summary>
        Boolean IsSystemRole { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Xml representation of Role object
        /// </summary>
        /// <returns>Xml format of role</returns>
        String ToXml();

        /// <summary>
        /// Xml representation of Role object
        /// </summary>
        /// <param name="serialization">Type of serialization</param>
        /// <returns>Xml format of role</returns>
        String ToXml(MDM.Core.ObjectSerialization serialization);

        /// <summary>
        /// Get users for current role
        /// </summary>
        /// <returns>User collection for current role</returns>
        ISecurityUserCollection GetUsers();

        /// <summary>
        /// Clone security role object
        /// </summary>
        /// <returns>cloned copy of security role object.</returns>
        ISecurityRole Clone();

        /// <summary>
        /// Delta Merge of security role
        /// </summary>
        /// <param name="deltaSecurityRole">Security role that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="returnClonedObject">true means return new merged cloned object;otherwise, same object.</param>
        /// <returns>Merged security role instance</returns>
        ISecurityRole MergeDelta(ISecurityRole deltaSecurityRole, ICallerContext iCallerContext, Boolean returnClonedObject = true);

        #endregion Methods
    }
}