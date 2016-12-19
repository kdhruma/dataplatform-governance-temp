using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties used for providing attributeinfo related information.
    /// </summary>
    public interface IAttributeInfo : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Fields denoting the attribute parent id
        /// </summary>
        Int32 AttributeParentId { get; set; }

        /// <summary>
        /// Field denoting the attribute parent name
        /// </summary>
        String AttributeParentName { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets XML representation of attribute change context object
        /// </summary>
        /// <returns>XML representation of attribute change context object</returns>
        String ToXml();

        #endregion Methods
    }
}
