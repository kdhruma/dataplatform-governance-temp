using System;

namespace MDM.Interfaces
{

    /// <summary>
    /// Exposes methods or properties to set or get dimension values for an item's internal status.
    /// </summary>
    public interface IIntegrationItemStatusDimensionInternal
    {
        #region Properties

        /// <summary>
        /// Indicates Id of IntegrationItemDimensionStatus
        /// </summary>
        Int64 Id { get; set; }

        /// <summary>
        /// Indicates Id of dimension type.
        /// </summary>
        Int32 IntegrationItemDimensionTypeId { get; set; }

        #endregion Properties

        #region Public methods

        /// <summary>
        /// Represent object in xml format
        /// </summary>
        /// <returns>object with values in xml format</returns>
        String ToXml();

        /// <summary>
        /// Return new object with the values same as current one
        /// </summary>
        /// <returns>New object with same values as current one</returns>
        IIntegrationItemStatusDimensionInternal Clone();

        #endregion Public methods
    }
}
