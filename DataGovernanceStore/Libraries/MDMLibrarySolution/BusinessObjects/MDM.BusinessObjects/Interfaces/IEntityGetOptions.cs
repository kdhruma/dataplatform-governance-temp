using System;
namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get the options available while retrieving entity data. 
    /// </summary>
    public interface IEntityGetOptions
    {
        /// <summary>
        /// Property denoting if attribute value security should be applied or not
        /// </summary>
        Boolean ApplyAVS { get; set; }

        /// <summary>
        /// Property denoting while loading entities whether to apply user permissions or not 
        /// </summary>
        Boolean ApplySecurity { get; set; }
        
        /// <summary>
        /// Property denoting no. of batches for bulk entity get
        /// </summary>
        Int32 BulkGetBatchSize { get; set; }
        
        /// <summary>
        /// property denoting entity filling options
        /// </summary>
        EntityGetOptions.EntityFillOptions FillOptions { get; set; }
        
        /// <summary>
        /// Property denoting if entity should be loaded from database
        /// </summary>
        Boolean LoadLatestFromDB { get; set; }
        
        /// <summary>
        /// Property denoting if events to be published as part of entity processing or not
        /// </summary>
        Boolean PublishEvents { get; set; }
        
        /// <summary>
        /// Property denoting if cache should be updated or not
        /// </summary>
        Boolean UpdateCache { get; set; }
        
        /// <summary>
        /// Property denoting if cache status needs to be updated in database or not
        /// </summary>
        Boolean UpdateCacheStatusInDB { get; set; }
    }
}
