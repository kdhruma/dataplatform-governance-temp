using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace MDM.CategoryManager.Data
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.Utility;

    /// <summary>
    /// Specifies the data access operations for category
    /// </summary>
    public class CategoryDA : SqlClientDataAccessBase, ICategoryDataProvider
    {
        #region Fields

        /// <summary>
        /// Field denoting Category path separator.
        /// </summary>
        private String _categoryPathSeparator = AppConfigurationHelper.GetAppConfig<String>("Catalog.Category.PathSeparator", " >> ");

        #endregion

        #region Constructors
        #endregion

        #region Properties
        #endregion

        #region Public Methods

        /// <summary>
        /// Core provider has to return false for this 
        /// </summary>
        /// <param name="hierarchy"></param>
        /// <param name="categoryContext"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public Boolean CanHandleContext(Hierarchy hierarchy, CategoryContext categoryContext, CallerContext callerContext)
        {
            // core does not need to handle custom context. Core can only do a get all. This method is for customization team to indicate if they need an override.
            return false;
        }

        /// <summary>
        /// Get ALL Base Properties for categories under requested hierarchy Id
        /// </summary>
        /// <param name="hierarchy">Hierarchy in which category is requested</param>
        /// <param name="command">Command having all DB connection information</param>
        /// <param name="categoryContext"></param>
        /// <param name="callerContext"></param>
        /// <returns>All Base Properties</returns>
        public Dictionary<Int64, CategoryBaseProperties> GetAllBaseCategories(Hierarchy hierarchy, DBCommandProperties command, CategoryContext categoryContext, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("CategoryDA.GetAllBaseCategories", MDMTraceSource.CategoryGet, false);

            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            DurationHelper durationHelper = new DurationHelper(DateTime.Now);
            Dictionary<Int64, CategoryBaseProperties> categories = new Dictionary<Int64, CategoryBaseProperties>();

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("CategoryManager_SqlParameters");

                parameters = generator.GetParameters("CategoryManager_Category_Get_ParametersArray");
                parameters[0].Value = hierarchy.Id;

                storedProcedureName = "usp_EntityManager_Category_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                categories = PopulateCategoryBaseProperties(reader, hierarchy);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.PERFORMANCE_TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Overall time to load Base Properties for {1} Categories from DB", durationHelper.GetDurationInMilliseconds(DateTime.Now), categories.Count), MDMTraceSource.CategoryGet);

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("CategoryDA.GetAllBaseCategories", MDMTraceSource.CategoryGet);
            }

            return categories;
        }

        /// <summary>
        /// Get ALL Locale Properties for categories under requested hierarchy Id and Locale
        /// </summary>
        /// <param name="locale">Locale in which category properties are needed</param>
        /// <param name="hierarchyId">Hierarchy Id in which category is requested</param>
        /// <param name="command">Command having all DB connection information</param>
        /// <param name="categoryContext"></param>
        /// <param name="callerContext"></param>
        /// <returns>All Locale properties</returns>
        public Dictionary<String, CategoryLocaleProperties> GetCategoryLocaleProperties(LocaleEnum locale, Int32 hierarchyId, DBCommandProperties command, CategoryContext categoryContext, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("CategoryDA.GetCategoryLocaleProperties", MDMTraceSource.CategoryGet, false);

            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;
            SqlParameter[] parameters;
            DurationHelper durationHelper = new DurationHelper(DateTime.Now);
            Dictionary<String, CategoryLocaleProperties> categories = new Dictionary<String, CategoryLocaleProperties>();

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("CategoryManager_SqlParameters");

                parameters = generator.GetParameters("CategoryManager_CategoryLocale_Get_ParametersArray");

                parameters[0].Value = hierarchyId;
                parameters[1].Value = (Int32)locale;

                storedProcedureName = "usp_EntityManager_CategoryLocale_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                categories = PopulateCategoryLocaleProperties(reader, locale);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.PERFORMANCE_TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Overall time to load Locale Properties for {1} Categories from DB", durationHelper.GetDurationInMilliseconds(DateTime.Now), categories.Count), MDMTraceSource.CategoryGet);

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("CategoryDA.GetCategoryLocaleProperties", MDMTraceSource.CategoryGet);
            }

            return categories;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populates category base properties
        /// </summary>
        /// <param name="reader">Indicates sql reader which contains category base properties data</param>
        /// <param name="hierarchy">Indicates hierarchy</param>
        /// <returns>Returns dictionary containing mapping between category base properties identifier and category base properties</returns>
        private Dictionary<Int64, CategoryBaseProperties> PopulateCategoryBaseProperties(SqlDataReader reader, Hierarchy hierarchy)
        {
            Dictionary<Int64, CategoryBaseProperties> categories = new Dictionary<Int64, CategoryBaseProperties>();
            
            if (reader != null)
            {
                while (reader.Read())
                {
                    CategoryBaseProperties categoryBaseProperties = new CategoryBaseProperties();

                    #region Reading Base Properties

                    if (reader["ID"] != null)
                    {
                        categoryBaseProperties.Id = ValueTypeHelper.Int64TryParse(reader["ID"].ToString(), -1);
                    }

                    if (reader["ParentId"] != null)
                    {
                        categoryBaseProperties.ParentCategoryId = ValueTypeHelper.Int64TryParse(reader["ParentId"].ToString(), -1);
                    }

                    if (reader["ShortName"] != null)
                    {
                        categoryBaseProperties.Name = reader["ShortName"].ToString();
                    }

                    if (reader["LongName"] != null)
                    {
                        categoryBaseProperties.LongName = reader["LongName"].ToString();
                    }

                    if (reader["Path"] != null)
                    {
                        categoryBaseProperties.Path = reader["Path"].ToString().Replace(Constants.STRING_PATH_SEPARATOR, _categoryPathSeparator);
                    }

                    if (reader["Level"] != null)
                    {
                        categoryBaseProperties.Level = ValueTypeHelper.Int32TryParse(reader["Level"].ToString(), categoryBaseProperties.Level);
                    }

                    if (reader["TaxonomyId"] != null)
                    {
                        categoryBaseProperties.HierarchyId = ValueTypeHelper.Int32TryParse(reader["TaxonomyId"].ToString(), categoryBaseProperties.HierarchyId);
                    }

                    if (reader["IsLeaf"] != null)
                    {
                        categoryBaseProperties.IsLeaf = ValueTypeHelper.BooleanTryParse(reader["IsLeaf"].ToString(), categoryBaseProperties.IsLeaf);
                    }

                    if (reader["IdPath"] != null)
                    {
                        categoryBaseProperties.IdPath = reader["IdPath"].ToString();
                    }

                    #endregion

                    if (hierarchy != null)
                    {
                        categoryBaseProperties.HierarchyName = hierarchy.Name;
                        categoryBaseProperties.HierarchyLongName = hierarchy.LongName;
                    }

                    if (!categories.ContainsKey(categoryBaseProperties.Id))
                    {
                        categories.Add(categoryBaseProperties.Id, categoryBaseProperties);
                    }
                    else
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Multiple category found for Category id: {0}", categoryBaseProperties.Id), MDMTraceSource.CategoryGet);
                    }
                }
            }

            return categories;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        private Dictionary<String, CategoryLocaleProperties> PopulateCategoryLocaleProperties(SqlDataReader reader, LocaleEnum locale)
        {
            Dictionary<String, CategoryLocaleProperties> categories = new Dictionary<String, CategoryLocaleProperties>();
            
            if (reader != null)
            {
                while (reader.Read())
                {
                    CategoryLocaleProperties categoryLocaleProperties = new CategoryLocaleProperties();
                    Int64 categoryId = -1;

                    #region Reading Locale Properties

                    if (reader["Id"] != null)
                    {
                        categoryId = ValueTypeHelper.Int64TryParse(reader["Id"].ToString(), -1);
                    }

                    if (reader["LongName"] != null)
                    {
                        categoryLocaleProperties.LongName = reader["LongName"].ToString();
                    }

                    if (reader["LongNamePath"] != null)
                    {
                        categoryLocaleProperties.LongNamePath = reader["LongNamePath"].ToString().Replace(Constants.STRING_PATH_SEPARATOR, _categoryPathSeparator); 
                    }

                    if (reader["HasLocaleProperties"] != null)
                    {
                        categoryLocaleProperties.HasLocaleProperties = ValueTypeHelper.BooleanTryParse(reader["HasLocaleProperties"].ToString().ToLowerInvariant(), false); 
                    }

                    if (reader["ParentCategoryName"] != null)
                    {
                        categoryLocaleProperties.ParentCategoryName = reader["ParentCategoryName"].ToString();
                    }

                    categoryLocaleProperties.Locale = locale;

                    #endregion

                    String Key = GetKey(categoryId, locale);

                    if (!categories.ContainsKey(Key))
                    {
                        categories.Add(Key, categoryLocaleProperties);
                    }
                    else
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Multiple category found for Category id: {0}", categoryId), MDMTraceSource.CategoryGet);
                    }
                }
            }

            return categories;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        private String GetKey(Int64 categoryId, LocaleEnum locale)
        {
            return String.Concat(categoryId, "_", (Int32)locale);
        }

        #region Old CategoryDA Private Methods

        private String ExtractNullableString(SqlDataReader reader, String fieldName)
        {
            var item = reader[fieldName];
            return (item == DBNull.Value) ? null : item as String;
        }

        #endregion Old CategoryDA Private Methods

        #endregion
    }
}
