using System;

namespace MDM.Imports.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.BusinessObjects.Jobs;
    using MDM.BusinessObjects.Imports;
    using MDM.Interfaces;

    /// <summary>
    /// This interface will be used by the import process to get data from a source.
    /// </summary>
    public interface ILookupImportSourceData
    {
        #region Lookup Data

        /// <summary>
        /// Provide an opportunity for the source data to initializes itself with some configuration from the job.
        /// </summary>
        /// <param name="job"></param>
        /// <param name="lookupImportProfile"></param>
        /// <returns></returns>
        Boolean Initialize(Job job, LookupImportProfile lookupImportProfile);

        /// <summary>
        /// Total number of entities available for processing.
        /// </summary>
        /// <returns></returns>
        Int64 GetLookupTableCount(MDMCenterApplication application, MDMCenterModules module);

        /// <summary>
        /// Indicates the batching mode the provider supports.
        /// </summary>
        /// <returns></returns>
        ImportProviderBatchingType GetBatchingType();

        /// <summary>
        /// Gets all the Lookup data available to process.
        /// </summary>
        /// <returns></returns>
        LookupCollection GetAllLookupTables(MDMCenterApplication application, MDMCenterModules module);

        /// <summary>
        /// Get data for a lookup table
        /// </summary>
        /// <param name="lookupTableName"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        Lookup GetSingleLookupTable(String lookupTableName, MDMCenterApplication application, MDMCenterModules module);

        /// <summary>
        /// Get data for next lookup table
        /// </summary>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        Lookup GetNextLookupTable(MDMCenterApplication application, MDMCenterModules module);

        /// <summary>
        /// Get the operation result if there are any error occur during Source data reader operation
        /// </summary>
        /// <returns>Returns the operation result interface</returns>
        IOperationResult GetOperationResult();

        #endregion

    }
}
