
using System;
namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get collection of integration item status search criteria.
    /// </summary>
    public interface IIntegrationItemStatusSearchParameterCollection
    {
        #region Public Methods

        /// <summary>
        /// Get Xml representation of IntegrationItemStatusSearchParameter object
        /// </summary>
        /// <returns>Xml String representing the IntegrationItemStatusSearchParameterCollection</returns>
        String ToXml();

        /// <summary>
        /// Clone IntegrationItemStatusSearchParameter collection.
        /// </summary>
        /// <returns>cloned IntegrationItemStatusSearchParameter collection object.</returns>
        IIntegrationItemStatusSearchParameterCollection Clone();

        #endregion Public Methods
    }
}
