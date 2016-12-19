using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;
    
    public interface ICachedDataModel
    {
       // ICachedDataModel Load();

        List<Organization> GetOrganizations();

        List<Container> GetContainers();

        List<EntityType> GetEntityTypes();

        List<RelationshipType> GetRelationshipTypes();

        Dictionary<Int32, CategoryCollection> GetCategories();

        CategoryCollection GetCategories(Int32 hierarchyId);

        Category GetCategory(Int32 hierarchyId, String categoryPath);

        Category GetCategory(String hierarchyname, String categoryPath);

        Category GetCategory(Int32 hierarchyId, Int64 categoryId);

        ConcurrentDictionary<AttributeModelContext, AttributeModelCollection> GetAllContextualAttributeModels();

        AttributeModelCollection GetContextualAttributeModels(AttributeModelContext attributeModelContext);

        AttributeModel GetContextualAttributeModel(AttributeModelContext attributeModelContext, Attribute attribute);

        AttributeModelCollection GetContextualAttributeModels(AttributeModelContext attributeModelContext, Collection<Int32> attributeIds);

        AttributeModelCollection GetAllBaseAttributeModels();

        AttributeModel GetBaseAttributeModel(Int32 attributeId, LocaleEnum locale);

        void LoadLookupTableCollection();

        Lookup GetLookupTable(Int32 attributeId, LocaleEnum locale);
        

        /// <summary>
        /// Get the lookup based on requested lookup name and locale
        /// </summary>
        /// <param name="lookupName">Indicates the lookup Name</param>
        /// <param name="locale">Indicates the locale</param>
        /// <returns>Returns the lookup object</returns>
		Lookup GetLookup(String lookupName, LocaleEnum locale);

        /// <summary>
        /// Get the lookups based on requested lookup names and the locales.
        /// </summary>
        /// <param name="lookupNames">Indicates list of lookup names</param>
        /// <param name="locales">Indicates list of locale enums</param>
        /// <returns>Returns the collection of lookups</returns>
        LookupCollection GetLookups(Collection<String> lookupNames, Collection<LocaleEnum> locales);

        /// <summary>
        /// Get all the locales list available in the system
        /// </summary>
        /// <returns>Return the list of locales</returns>
        Collection<LocaleEnum> GetAvailableLocaleValues();

        UOMCollection LoadUOMCollection();

        //Boolean RefreshCache(String cacheKey);
    }
}
