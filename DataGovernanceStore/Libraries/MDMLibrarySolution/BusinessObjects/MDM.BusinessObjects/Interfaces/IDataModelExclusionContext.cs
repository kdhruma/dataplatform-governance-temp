using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Interface for Data Model Exclusion context object
    /// </summary>
    public interface IDataModelExclusionContext : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Specifies service type 
        /// </summary>
        MDMServiceType ServiceType { get; set; }

        /// <summary>
        /// Specifies Id of an organization
        /// </summary>
        Int32 OrganizationId { get; set; }

        /// <summary>
        /// Specifies Id of an container
        /// </summary>
        Int32 ContainerId { get; set; }

        /// <summary>
        /// Specifies Id of an entity type
        /// </summary>
        Int32 EntityTypeId { get; set; }

        /// <summary>
        /// Specifies Id of an attribute (0: All, -1: common, -2:Technical)
        /// </summary>
        Int32 AttributeId { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Gives XMl representation of an instance
        /// </summary>
        /// <returns>String xml representation</returns>
        String ToXml();

        #endregion
    }
}
