using System;
using System.Collections.ObjectModel;


namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get search criteria for integration item status. 
    /// </summary>
    public interface IIntegrationItemStatusSearchCriteria
    {
        #region Properties

        /// <summary>
        /// Indicates search status for which connector?
        /// </summary>
        Int16 ConnectorId { get; set; }

        /// <summary>
        /// Indicates the values of status types which needs to be searched.
        /// </summary>
        Collection<OperationResultType> StatusTypes { get; set; }


        /// <summary>
        /// Indicates which view will be used
        /// </summary>
        Boolean IncludeHistoryData { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Add MDMObjectTypeId and MDMObjectId for search criteria
        /// </summary>
        /// <param name="mdmObjectTypeId">MDMObjectTypeId to search</param>
        /// <param name="commaSeparatedMDMObjectIds">Comma separated list of MDMObjectIds to search</param>
        void AddMDMObjectTypeIdAndValues(Int16 mdmObjectTypeId, String commaSeparatedMDMObjectIds);

        /// <summary>
        /// Add ExternalObjectTypeId and ExternalId for search criteria
        /// </summary>
        /// <param name="externalObjectTypeId">ExternalObjectTypeId to search</param>
        /// <param name="commaSeparatedExternalMDMObjectIds">Comma separated list of ExternalIds to search</param>
        void AddExternalObjectTypeIdAndValues(Int16 externalObjectTypeId, String commaSeparatedExternalMDMObjectIds);

        /// <summary>
        /// Add DimensionTypeId and DimensionValueId for search criteria
        /// </summary>
        /// <param name="dimensionValueId">Indicates dimension value identifier to search</param>
        /// <param name="commaSeparatedStatusToSearch">Indicates comma separated list of SeparatedStatusIds to search</param>
        void AddDimensionValuesAndStatus(Int32 dimensionValueId, String commaSeparatedStatusToSearch);

        /// <summary>
        /// Add comments of ItemStatus for search criteria
        /// </summary>
        /// <param name="comments">ItemStatus comments to search</param>
        /// <param name="searchOperator">Operator for search for comments</param>
        void AddItemStatusComments(String comments, SearchOperator searchOperator);

        /// <summary>
        /// Add StatusValues for search criteria
        /// </summary>
        /// <param name="commaSeparatedStatusValuesToSearch">Comma separated status values to search</param>
        void AddStatusValue(String commaSeparatedStatusValuesToSearch);


        /// <summary>
        /// Get Xml representation of IntegrationItemStatusSearchCriteria
        /// </summary>
        /// <returns>Xml representation of object</returns>
        String ToXml();

        #endregion Methods
    }
}
