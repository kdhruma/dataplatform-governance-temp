using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.BusinessObjects.MergeCopy
{
    using MDM.BusinessObjects;
    using MDM.Interfaces;
    using MDM.Core;
    
    /// <summary>
    /// Indicates enumeration for extension provider type
    /// </summary>
    public enum ExtensionProviderType 
    { 
        /// <summary>
        /// Indicates MDL extension provider
        /// </summary>
        MDLExtensionProvider = 1, 

        /// <summary>
        /// Indicates attribute extension provider
        /// </summary>
        AttributeExtensionProvider, 

        /// <summary>
        /// Indicates user defined extension provider
        /// </summary>
        UserDefinedExtensionProvider 
    } ;

    /// <summary>
    /// Entity Extension Simulator Interface
    /// Any external Entity-Entity extension map needs to implement these methods which are used
    ///      by the MergeCopy engine to find out which entities are connected through extension
    /// There are two native implementations : one by parentExtensionId and one by entity attribute
    /// </summary>
    public interface IMergeCopyEntityExtensionProvider
    {
        /// <summary>
        /// Get extension related entity id based on target entity, target container id, and target category identifier
        /// </summary>
        /// <param name="sourceEntity">Indicates the source entity</param>
        /// <param name="targetEntity">indicates the target entity</param>
        /// <param name="targetCatalogId">Indicates the target container identifier</param>
        /// <param name="targetCategoryId">Indicates the target category identifier</param>
        /// <param name="isCheckout">Indicates a boolean value indicating if entities to be promoted or demoted</param>
        /// <returns>Returns extension related entity id based on target entity, target container id, and target category id</returns>
        long GetExtensionRelatedEntityId(Entity sourceEntity, Entity targetEntity, long targetCatalogId, long targetCategoryId, bool isCheckout);

        /// <summary>
        /// Get extension related master entity identifier
        /// </summary>
        /// <param name="stagingEntity">Indicates the staging entity</param>
        /// <returns>Returns extension related master entity identifier</returns>
        long GetExtensionRelatedMasterEntityId(Entity stagingEntity);

        /// <summary>
        /// Get extension children entity identfiers of master entity based on container id and target category id
        /// </summary>
        /// <param name="masterEntity">Indicates the master entity</param>
        /// <param name="targetCatalogId">Indicates the target container id</param>
        /// <param name="targetCategoryId">Indicates the target category id</param>
        /// <returns>Returns extension children entity identfiers of master entity based on container id and target category id</returns>
        List<long> GetExtensionChildrenEntityIdsOfMaster(Entity masterEntity, long targetCatalogId, long targetCategoryId);

        /// <summary>
        ///  Set the extension based on staging and master entity
        /// </summary>
        /// <param name="stagingEntity">Indicates the staging entity</param>
        /// <param name="masterEntity">Indicates the master entity</param>
        /// <param name="isCheckout">Indicates a boolean value indicating if entities to be promoted or demoted</param>
        void SetExtension(Entity stagingEntity, Entity masterEntity, bool isCheckout);

        /// <summary>
        /// Get a boolean value indicating whether staging entity is extended
        /// </summary>
        /// <param name="stagingEntity">Indicates staging entity</param>
        /// <returns>Returns a boolean value indicating whether staging entity is extended</returns>
        bool IsStagingEntityExtended(Entity stagingEntity);

        /// <summary>
        /// Get the extension provider type
        /// </summary>
        /// <returns>Returns the extension provider type</returns>
        ExtensionProviderType GetExtensionProviderType() ;
    }

}
