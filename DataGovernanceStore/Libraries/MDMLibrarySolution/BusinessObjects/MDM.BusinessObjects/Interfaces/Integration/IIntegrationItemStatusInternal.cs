using System;

namespace MDM.Interfaces
{

    /// <summary>
    /// Exposes methods or properties to set or get integration item internal status.
    /// </summary>
    public interface IIntegrationItemStatusInternal
    {
        #region Properties

        /// <summary>
        /// Indicates Id of IntegrationItemStatus
        /// </summary>
        Int64 Id { get; set; }

        /// <summary>
        /// Indicates Id of MDMobject participating as IntegrationItem
        /// </summary>
        Int64 MDMObjectId { get; set; }

        /// <summary>
        /// Indicates id of Object type
        /// </summary>
        Int16 MDMObjectTypeId { get; set; }

        /// <summary>
        /// Indicates Id of entity/message in external system.
        /// </summary>
        String ExternalId { get; set; }

        /// <summary>
        /// Indicates Id of connector for which status is being recorded.
        /// </summary>
        Int16 ConnectorId { get; set; }

        /// <summary>
        /// Indicates long name of connector for which status is being recorded.
        /// </summary>
        String ConnectorLongName { get; set; }

        /// <summary>
        /// Indicates time stamp indicating when status was updated.
        /// </summary>
        DateTime AuditTimeStamp { get; }

        #endregion Properties

        #region Public methods

        /// <summary>
        /// Represents IntegrationItemStatusInternalCollection in Xml format
        /// </summary>
        /// <returns></returns>
        String ToXml();

        /// <summary>
        /// Return new object with the values same as current one
        /// </summary>
        /// <returns>New object with same values as current one</returns>
        IIntegrationItemStatusInternal Clone();

        #endregion Public methods
    }
}
