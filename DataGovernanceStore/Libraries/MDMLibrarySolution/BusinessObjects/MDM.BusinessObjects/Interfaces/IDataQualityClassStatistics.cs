using System;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get data quality class statistics.
    /// </summary>
    public interface IDataQualityClassStatistics
    {
        /// <summary>
        /// Property for the Data quality class Id of DataQualityClassStatistics
        /// </summary>
        Int16 DataQualityClassId { get; set; }

        /// <summary>
        /// Property for the Count of entities in the data quality class of DataQualityClassStatistics
        /// </summary>
        Int64? EntitiesCount { get; set; }        
    }
}
