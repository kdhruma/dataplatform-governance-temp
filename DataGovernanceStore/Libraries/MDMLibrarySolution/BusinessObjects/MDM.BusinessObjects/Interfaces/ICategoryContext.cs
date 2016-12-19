using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get the category context.
    /// </summary>
    public interface ICategoryContext
    {
        #region Properties

        /// <summary>
        /// The hierarchy Id
        /// </summary>
        Int32 HierarchyId { get; set; }

        /// <summary>
        /// Indicates container identifier of category context
        /// </summary>
        Int32 ContainerId { get; set; }

        /// <summary>
        /// Container Name in which the category belongs
        /// </summary>
        String ContainerName { get; set; }

        /// <summary>
        /// Flag for loading child categories or not
        /// </summary>
        Boolean LoadRecursive { get; set; }

        /// <summary>
        /// SearchRules for Category
        /// </summary>
        CategorySearchRuleCollection CategorySearchRules { get; set; }

        /// <summary>
        /// Field on which result should be filtered
        /// </summary>
        CategoryField OrderByField { get; set; }

        /// <summary>
        /// Maximum records to return in search result
        /// </summary>
        Int32 MaxRecordsToReturn { get; set; }

        /// <summary>
        /// Index denoting from where to return result
        /// </summary>
        Int32 StartIndex { get; set; }

        /// <summary>
        /// Index denoting till what to return in result
        /// </summary>
        Int64 EndIndex { get; set; }

        /// <summary>
        /// DataLocales in which result has to be returned
        /// </summary>
        Collection<LocaleEnum> Locales { get; set; }

        /// <summary>
        /// Flag denoting whether to apply security on search result or not
        /// </summary>
        Boolean ApplySecurity { get; set; }

        /// <summary>
        /// Field denoting whether to load parent categories recursive or not
        /// </summary>
        Boolean LoadParentRecursive { get; set; }

        /// <summary>
        /// Field denoting whether to load leaf level categories
        /// </summary>
        Boolean LoadOnlyLeafCategories { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of CategoryContext
        /// </summary>
        /// <returns>Xml representation of CategoryContext</returns>
        String ToXml();

        #endregion
    }
}
