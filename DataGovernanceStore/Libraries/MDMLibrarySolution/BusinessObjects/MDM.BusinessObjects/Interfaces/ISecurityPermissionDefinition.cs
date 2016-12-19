using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get security permission definition.
    /// </summary>
    public interface ISecurityPermissionDefinition : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property denoting Application Context id
        /// </summary>
        Int32 ApplicationContextId { get; set; }

        /// <summary>
        /// Property denoting Application Context instance
        /// </summary>
        ApplicationContext ApplicationContext { get; set; }

        /// <summary>
        /// Property denoting attribute values used for permissions.
        /// </summary>
        Collection<String> PermissionValues { get; set; }

        /// <summary>
        /// Property denoting permission action.
        /// </summary>
        Collection<UserAction> PermissionSet { get; set; }

        /// <summary>
        /// Property denoting context weightage
        /// </summary>
        Int32 ContextWeightage { get; set; }

        #endregion

        #region Method

        /// <summary>
        /// Get Xml representation of SecurityPermissionDefinition object
        /// </summary>
        /// <returns>Xml representation of object</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of SecurityPermissionDefinition object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization serialization);

        #endregion
    }
}