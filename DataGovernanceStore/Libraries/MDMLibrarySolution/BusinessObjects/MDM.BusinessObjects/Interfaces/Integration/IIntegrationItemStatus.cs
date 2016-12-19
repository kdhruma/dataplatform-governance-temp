using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get status for an integration item.
    /// </summary>
    public interface IIntegrationItemStatus
    {
        #region Properties

        /// <summary>
        /// Indicates Id of MDMobject participating as IntegrationItem
        /// </summary>
        Int64 MDMObjectId { get; set; }

        /// <summary>
        /// Indicates id of Object type
        /// </summary>
        String MDMObjectTypeName { get; set; }

        /// <summary>
        /// Indicates Id of entity/message in external system.
        /// </summary>
        String ExternalId { get; set; }

        /// <summary>
        /// Indicates  name of object type for external id.
        /// </summary>
        String ExternalObjectTypeName { get; set; }

        /// <summary>
        /// Indicates name of connector for which status is being recorded.
        /// </summary>
        String ConnectorName { get; set; }

        /// <summary>
        /// Indicates status value.
        /// </summary>
        String Status { get; set; }

        /// <summary>
        /// Indicates some additional comments for the dimension value
        /// </summary>
        String Comments { get; set; }

        /// <summary>
        /// Indicates type of status. Is it error/info/warning
        /// </summary>
        OperationResultType StatusType { get; set; }

        /// <summary>
        /// Indicates if indicating status is for message in external system or it is indicating status for message in our system. 
        /// </summary>
        Boolean IsExternalStatus { get; set; }

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get Xml string  representing the current object.
        /// </summary>
        /// <returns>Object in xml format</returns>
        String ToXml();

        /// <summary>
        /// Return new object with the values same as current one
        /// </summary>
        /// <returns>New object with same values as current one</returns>
        IIntegrationItemStatus Clone();

        /// <summary>
        /// Gets the attributes belonging to the Entity
        /// </summary>
        /// <returns>Attribute Collection Interface</returns>
        /// <exception cref="NullReferenceException">Attributes for entity is null. There are no attributes to search in</exception>
        IIntegrationItemStatusDimensionCollection GetStatusDimensions();

        /// <summary>
        /// Sets the integration item status dimension for item status
        /// </summary>
        /// <param name="iIntegrationItemStatusDimensions">IntegrationItemStatusDimensions Interface</param>
        void SetStatusDimensions(IIntegrationItemStatusDimensionCollection iIntegrationItemStatusDimensions);

        #endregion

        #endregion
    }
}