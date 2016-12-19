using System;
using System.Collections.Generic;
using MDM.BusinessObjects;
using MDM.Core;

namespace MDM.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICategoryDataProvider
    {
        /// <summary>
        /// Based on the given context this method will indicate to the core if the custom data provider can handle this situation or not. This method will be 
        /// called for each category get. For a custom provider, this method has to return true to handle the custom category get. If this method returns false, 
        /// the default core behavior will continue.
        /// </summary>
        /// <param name="hierarchy"></param>
        /// <param name="categoryContext"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        Boolean CanHandleContext(Hierarchy hierarchy, CategoryContext categoryContext, CallerContext callerContext);

        /// <summary>
        /// Get ALL Base Properties for categories under requested hierarchy Id
        /// </summary>
        /// <param name="hierarchy">Hierarchy object in which category is requested</param>
        /// <param name="command">Command having all DB connection information</param>
        /// <param name="categoryContext"></param>
        /// <param name="callerContext"></param>
        /// <returns>All Base Properties</returns>
        Dictionary<Int64, CategoryBaseProperties> GetAllBaseCategories(Hierarchy hierarchy, DBCommandProperties command, CategoryContext categoryContext, CallerContext callerContext);

        /// <summary>
        /// Get ALL Locale Properties for categories under requested hierarchy Id and Locale
        /// </summary>
        /// <param name="locale">Locale in which category properties are needed</param>
        /// <param name="hierarchyId">Hierarchy Id in which category is requested</param>
        /// <param name="command">Command having all DB connection information</param>
        /// <param name="categoryContext"></param>
        /// <param name="callerContext"></param>
        /// <returns>All Locale properties</returns>
        Dictionary<String, CategoryLocaleProperties> GetCategoryLocaleProperties(LocaleEnum locale, Int32 hierarchyId, DBCommandProperties command, CategoryContext categoryContext, CallerContext callerContext);
    }
}