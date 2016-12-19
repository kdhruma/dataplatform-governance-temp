using System;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;

    /// <summary>
    /// Specifies interface providing Lookup Model Manager
    /// </summary>
    
    public interface ILookupManager
    {
        /// <summary>
        /// Gets lookup data for the requested attribute and locale
        /// </summary>
        /// <param name="attributeId">Id of the attribute for which data needs to be get</param>
        /// <param name="locale">Locale for which data needs to be fetched</param>
        /// <param name="maxRecordsToReturn">Max number of lookup records to return. Setting '-1' returns all record</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Lookup data</returns>
        Lookup Get(Int32 attributeId, LocaleEnum locale, Int32 maxRecordsToReturn, CallerContext callerContext);

        /// <summary>
        /// Gets model of the requested lookup table 
        /// </summary>
        /// <param name="lookupTableName">Name of the lookup table</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Returns lookup model</returns>
        Lookup GetModel(String lookupTableName, CallerContext callerContext);

        /// <summary>
        /// Gets lookup data for the requested lookup table and locale
        /// </summary>
        /// <param name="lookupTableName">Name of the lookup table</param>
        /// <param name="locale">Locale for which data needs to be fetched</param>
        /// <param name="maxRecordsToReturn">Max number of lookup records to return. Setting '-1' returns all record</param>
        /// <param name="getLatest">Boolean flag which says whether to get from DB or cache. True means always get from DB</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Lookup data</returns>
        /// <exception cref="MDMOperationException">Thrown when lookupTableName or locale is not available</exception>
        Lookup Get(String lookupTableName, LocaleEnum locale, Int32 maxRecordsToReturn, Boolean getLatest, CallerContext callerContext);
    }
}
