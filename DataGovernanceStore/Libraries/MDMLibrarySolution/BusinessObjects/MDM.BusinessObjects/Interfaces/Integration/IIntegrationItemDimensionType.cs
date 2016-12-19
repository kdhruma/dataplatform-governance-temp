using System;

namespace MDM.Interfaces
{
    using MDM.Core;
    /// <summary>
    /// Exposes methods or properties used for indicating a status of an item for a connector.
    /// </summary>
    public interface IIntegrationItemDimensionType : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Indicates Id of IntegrationItemDimensionType
        /// </summary>
        new Int32 Id { get; set; }

        /// <summary>
        /// Indicates id of connector for the status
        /// </summary>
        Int16 ConnectorId { get; set; }

        /// <summary>
        /// Indicates name of connector for the status
        /// </summary>
        String ConnectorName { get; set; }

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Represent IntegrationItemDimensionType in Xml format
        /// </summary>
        /// <returns>IntegrationItemDimensionType object in Xml format</returns>
        String ToXml();

        /// <summary>
        /// Create new instance of IIntegrationItemDimensionType with same values as current one
        /// </summary>
        /// <returns></returns>
        IIntegrationItemDimensionType Clone();

        #endregion Public methods
    }
}