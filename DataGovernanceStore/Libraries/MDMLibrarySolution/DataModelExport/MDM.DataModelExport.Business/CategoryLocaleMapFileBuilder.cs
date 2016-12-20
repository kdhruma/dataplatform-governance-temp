using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using DocumentFormat.OpenXml.Spreadsheet;

namespace MDM.DataModelExport.Business
{
    using MDM.Core;
    using MDM.Core.DataModel;
    using MDM.ExcelUtility;
    using MDM.Interfaces;
    using MDM.AdminManager.Business;
    using BO = MDM.BusinessObjects;
    using MDM.HierarchyManager.Business;
    using MDM.CategoryManager.Business;
    using MDM.KnowledgeManager.Business;
    using MDM.BusinessObjects.Exports;

    /// <summary>
    /// Class provides functionality to export category localization data as an excel file.
    /// </summary>
    internal class CategoryLocaleMapFileBuilder : DataModelFileBuilderBase
    {
        #region Overridden Methods

        protected override IDataModelObjectCollection GetDataModelObjectCollection(BO.CallerContext callerContext)
        {
            BO.HierarchyCollection hierarchies = null;
            HierarchyBL hierarchyBL = new HierarchyBL();
            LocaleBL localeBL = new LocaleBL();
            CategoryBL categoryBL = new CategoryBL();
            BO.CategoryCollection categories = new BO.CategoryCollection();
            BO.CategoryCollection allCategories = new BO.CategoryCollection();

            hierarchies = hierarchyBL.GetAll(callerContext);
            Collection<LocaleEnum> dataLocales = localeBL.GetAvailableLocaleValues();
            dataLocales.Remove(systemDataLocale);

            foreach (BO.Hierarchy hierarchy in hierarchies)
            {
                foreach (LocaleEnum dataLocale in dataLocales)
                {
                    categories = categoryBL.GetAllCategories(hierarchy.Id, callerContext, dataLocale);
                    allCategories.AddRange(categories);
                }
            }

            return allCategories;
        }

        protected override IDataModelObjectCollection GetDataModelObjectCollection(DataModelExportContext dataModelExportContext, BO.CallerContext callerContext)
        {
            BO.CategoryCollection allCategories = new BO.CategoryCollection();
            Collection<LocaleEnum> dataLocales = dataModelExportContext.Locales;

            if (dataLocales.Contains(systemDataLocale))
            {
                dataLocales.Remove(systemDataLocale);
            }

            if (dataLocales.Count > 0)
            {
                CategoryBL categoryBL = new CategoryBL();
                BO.CategoryCollection categories = new BO.CategoryCollection();

                if (dataModelExportContext.HierarchyIdList != null && dataModelExportContext.HierarchyIdList.Count > 0)
                {
                    foreach (Int32 hierarchyId in dataModelExportContext.HierarchyIdList)
                    {
                        foreach (LocaleEnum dataLocale in dataLocales)
                        {
                            categories = categoryBL.GetAllCategories(hierarchyId, callerContext, dataLocale);
                            allCategories.AddRange(categories, dataModelExportContext.ExcludeNonTranslatedModelData);
                        }
                    }
                }
                else
                {
                    BO.HierarchyCollection hierarchies = null;
                    HierarchyBL hierarchyBL = new HierarchyBL();
                    hierarchies = hierarchyBL.GetAll(callerContext);

                    foreach (BO.Hierarchy hierarchy in hierarchies)
                    {
                        foreach (LocaleEnum dataLocale in dataLocales)
                        {
                            categories = categoryBL.GetAllCategories(hierarchy.Id, callerContext, dataLocale);
                            allCategories.AddRange(categories, dataModelExportContext.ExcludeNonTranslatedModelData);
                        }
                    }
                }
            }

            return allCategories;
        }

        protected override void PopulateDataModelObjectInDataRow(IDataModelObject dataModelObject, List<String> headerList, Row dataRow)
        {
            UInt32 columnIndex = 1;

            BO.Category category = (BO.Category)dataModelObject;

            //Category Id is overridden Property in Category class
            if (headerList.Contains(DataModelDictionary.CommonHeadersDictionary[DataModelCommonHeaders.Id]))
            {
                OpenSpreadsheetUtility.AppendRowWithNumberCell(dataRow, columnIndex++, category.Id);
            }

            //Category Action is overridden Property in Category class
            //Action should be empty in the exported file
            if (headerList.Contains(DataModelDictionary.CommonHeadersDictionary[DataModelCommonHeaders.Action]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, String.Empty);
            }

            if (headerList.Contains(DataModelDictionary.CommonHeadersDictionary[DataModelCommonHeaders.UniqueIdentifier]))
            {
                //TODO : As of now UniqueIdentifier is set as empty.
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, String.Empty);
            }

            if (headerList.Contains(DataModelDictionary.CategoryLocalizationDictionary[DataModelCategoryLocalization.CategoryName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, category.Name);
            }

            if (headerList.Contains(DataModelDictionary.CategoryLocalizationDictionary[DataModelCategoryLocalization.ParentCategoryPath]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, GetParentCategoryPath(category.Path));
            }

            if (headerList.Contains(DataModelDictionary.CategoryLocalizationDictionary[DataModelCategoryLocalization.HierarchyName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, category.HierarchyName);
            }

            if (headerList.Contains(DataModelDictionary.CategoryLocalizationDictionary[DataModelCategoryLocalization.Locale]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, category.Locale);
            }

            if (headerList.Contains(DataModelDictionary.CategoryLocalizationDictionary[DataModelCategoryLocalization.CategoryLongName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, (category.HasLocaleProperties) ? category.LongName : RSExcelConstants.BlankText);
            }
        }

        protected override ObjectType ObjectType
        {
            get { return ObjectType.CategoryLocalization; }
        }

        #endregion
    }
}
