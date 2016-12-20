using System;
using System.Xml;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get source description and check whether the source IsSystem source or not.
    /// </summary>
    public interface ISource : IMDMObject, ICloneable
    {
        /// <summary>
        /// Indicates the Description
        /// </summary>
        String Description { get; set; }

        /// <summary>
        /// Indicates the IsSystem
        /// </summary>
        Boolean IsInternal { get; set; }

        #region Methods

        /// <summary>
        /// Set source id attribute to target node
        /// </summary>
        /// <param name="xmlWriter">Xml writer responsible for serializing target object</param>
        void SetSourceIdAttribute(XmlWriter xmlWriter);

        #endregion Methods
    }
}