using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;

    public interface ICategoryManager
    {
        /// <summary>
        /// Search Categories for requested context
        /// </summary>
        /// <param name="categoryContext">Search Context containing hierarchy id, locale and other criteria. </param>
        /// <param name="callerContext">Context indicating the caller of the API</param>
        /// <returns>Collection of Category</returns>
        CategoryCollection Search(CategoryContext categoryContext, CallerContext callerContext);

        /// <summary>
        /// Get Categories details By requested Hierarchy and category mapping
        /// </summary>
        /// <param name="mappingCollection">Property denotes which categories for which hierarchy ids need to use</param>
        /// <param name="callerContext">Context indicating the caller of the API</param>
        /// <returns>Collection of Categories</returns>
        CategoryCollection GetByIds(HierachyCategoryMappingCollection mappingCollection, CallerContext callerContext);

        /// <summary>
        /// Get Category details By requested HierarchyId and CategoryId
        /// </summary>
        /// <param name="hierarchyId">Hierarchy Id in which category is requested</param>
        /// <param name="categoryId">Id of an Category</param>
        /// <param name="callerContext">Context indicating the caller of the API</param>
        /// <returns>Category Information</returns>
        Category GetById(Int32 hierarchyId, Int64 categoryId, CallerContext callerContext);

        /// <summary>
        /// Get Category details By requested HierarchyId and CategoryId
        /// </summary>
        /// <param name="hierarchyId">Hierarchy Id in which category is requested</param>
        /// <param name="categoryId">Id of an Category</param>
        /// <param name="locale">DataLocale</param>
        /// <param name="callerContext">Context indicating the caller of the API</param>
        /// <param name="applySecurity">Context indicating the caller of the API</param>
        /// <returns>Category Information</returns>
        Category GetById(Int32 hierarchyId, Int64 categoryId, LocaleEnum locale, CallerContext callerContext, Boolean applySecurity = false);

        /// <summary>
        /// Get Category details By requested CategoryName
        /// </summary>
        /// <param name="hierarchyId">Hierarchy Id in which category is requested</param>
        /// <param name="categoryName">Name of the category</param>
        /// <param name="callerContext">Context indicating the caller of the API</param>
        /// <returns>Category Information</returns>
        Category GetByName(Int32 hierarchyId, String categoryName, CallerContext callerContext);

        /// <summary>
        /// Get Category details By requested category path
        /// </summary>
        /// <param name="hierarchyId">Hierarchy Id in which category is requested</param>
        /// <param name="path">Path of the category</param>
        /// <param name="callerContext">Context indicating the caller of the API</param>
        /// <returns>Category Information</returns>
        Category GetByPath(Int32 hierarchyId, String path, CallerContext callerContext);

        /// <summary>
        /// Get All Categories under requested hierarchy Id
        /// </summary>
        /// <param name="hierarchyId">Hierarchy Id in which category is requested</param>
        /// <param name="callerContext">Context indicating the caller of the API</param>
        /// <param name="locale">Locale in which Category is needed</param>
        /// <returns>Collection of Category</returns>
        CategoryCollection GetAllCategories(Int32 hierarchyId, CallerContext callerContext, LocaleEnum locale = LocaleEnum.UnKnown);
    }
}
