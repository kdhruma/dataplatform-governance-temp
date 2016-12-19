using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Provides the properties and methods relating to export queue
    /// </summary>
    public interface IExportQueue : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property denoting the entity identifier.
        /// </summary>
        Int64 EntityId { get; set; }

        /// <summary>
        /// Property denoting the entity global family identifier.
        /// </summary>
        Int64 EntityGlobalFamilyId { get; set; }

        /// <summary>
        /// Property denoting the entity family identifier.
        /// </summary>
        Int64 EntityFamilyId { get; set; }

        /// <summary>
        /// Property denoting the hierarchy level
        /// </summary>
        Int16 HierarchyLevel { get; set; }

        /// <summary>
        /// Property denoting the container identifier
        /// </summary>
        Int32 ContainerId { get; set; }

        /// <summary>
        /// Property denoting the export profile identifier.
        /// </summary>
        Int32 ExportProfileId { get; set; }

        /// <summary>
        /// Property denoting the entity type idenifier.
        /// </summary>
        Int32 EntityTypeId { get; set; }

        /// <summary>
        /// Property denoting a value indicating whether export of the this instance is in progress.
        /// </summary>
        Boolean IsExportInProgress { get; set; }

        /// <summary>
        /// Property denoting a value indicating whether this instance is exported.
        /// </summary>
        Boolean IsExported { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets XMl representation of the entity hierarchy context
        /// </summary>
        /// <returns>XMl string representation of entity hierarchy context</returns>
        String ToXml();

        #endregion Methods

    }
}
