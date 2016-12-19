using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get the collection of security permission definition.
    /// </summary>
    public interface ISecurityPermissionDefinitionCollection : IEnumerable<SecurityPermissionDefinition>
    {
        #region Properties

        #endregion

        #region ToXml Methods

        /// <summary>
        /// Get Xml representation of SecurityPermissionDefinitionCollection object
        /// </summary>
        /// <returns>Xml string representing the SecurityPermissionDefinitionCollection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of SecurityPermissionDefinitionCollection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml string representing the SecurityPermissionDefinitionCollection</returns>
        String ToXml(ObjectSerialization serialization);

        /// <summary>
        /// Add SecurityPermissionDefinition object in collection
        /// </summary>
        /// <param name="item">ISecurityPermissionDefinition to add in collection</param>
        void Add(ISecurityPermissionDefinition item);

        #endregion
    }
}