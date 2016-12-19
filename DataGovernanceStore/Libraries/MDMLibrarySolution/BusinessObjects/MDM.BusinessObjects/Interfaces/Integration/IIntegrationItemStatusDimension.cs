using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get dimension values for an item's status.
    /// </summary>
    public interface IIntegrationItemStatusDimension
    {
        #region Properties

        /// <summary>
        /// Gets or sets the integration item dimension type identifier.
        /// </summary>
        /// <value>
        /// The integration item dimension type identifier.
        /// </value>
        String IntegrationItemDimensionTypeName { get; set; }

        /// <summary>
        /// Gets or sets the integration item dimension value.
        /// </summary>
        /// <value>
        /// The integration item dimension value.
        /// </value>
        String IntegrationItemDimensionValue { get; set; }

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
        IIntegrationItemStatusDimension Clone();

        #endregion Public methods
    }
}
