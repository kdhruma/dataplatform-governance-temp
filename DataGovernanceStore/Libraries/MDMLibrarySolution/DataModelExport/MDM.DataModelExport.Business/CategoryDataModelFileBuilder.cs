using System;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Spreadsheet;

namespace MDM.DataModelExport.Business
{
    using MDM.Core;
    using MDM.Core.DataModel;
    using MDM.ExcelUtility;
    using MDM.Interfaces;
    using BO = MDM.BusinessObjects;
    using MDM.HierarchyManager.Business;
    using MDM.CategoryManager.Business;
    using MDM.BusinessObjects.Exports;

    /// <summary>
    /// Class provides functionality to export category data as an excel file.
    /// </summary>
    internal class CategoryDataModelFileBuilder : DataModelFileBuilderBase
    {
        #region Overridden Methods

        protected override IDataModelObjectCollection GetDataModelObjectCollection(BO.CallerContext callerContext)
        {
            BO.HierarchyCollection hierarchies = null;
            CategoryBL categoryBL = new CategoryBL();
            BO.CategoryCollection categories = new BO.CategoryCollection();
            BO.CategoryCollection allCategories = new BO.CategoryCollection();

            HierarchyBL hierarchyBL = new HierarchyBL();
            hierarchies = hierarchyBL.GetAll(callerContext);

            foreach (BO.Hierarchy hierarchy in hierarchies)
            {
                categories = categoryBL.GetAllCategories(hierarchy.Id, callerContext);
                allCategories.AddRange(categories);
            }

            return allCategories;
        }

        protected override IDataModelObjectCollection GetDataModelObjectCollection(DataModelExportContext dataModelExportContext, BO.CallerContext callerContext)
        {
            BO.CategoryCollection allCategories = new BO.CategoryCollection();

            if (dataModelExportContext.HierarchyIdList != null && dataModelExportContext.HierarchyIdList.Count > 0)
            {
                CategoryBL categoryBL = new CategoryBL();
                BO.CategoryCollection categories = new BO.CategoryCollection();

                foreach (Int32 hierarchyId in dataModelExportContext.HierarchyIdList)
                {
                    categories = categoryBL.GetAllCategories(hierarchyId, callerContext);
                    allCategories.AddRange(categories);
                }
            }
            else
            {
                return GetDataModelObjectCollection(callerContext);
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
            //Action should not get displayed in the exported file.
            if (headerList.Contains(DataModelDictionary.CommonHeadersDictionary[DataModelCommonHeaders.Action]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, String.Empty);
            }

            if (headerList.Contains(DataModelDictionary.CommonHeadersDictionary[DataModelCommonHeaders.UniqueIdentifier]))
            {
                //TODO : As of now UniqueIdentifier is set as empty.
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, String.Empty);
            }

            if (headerList.Contains(DataModelDictionary.CategoryDictionary[DataModelCategory.CategoryName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, category.Name);
            }

            if (headerList.Contains(DataModelDictionary.CategoryDictionary[DataModelCategory.CategoryLongName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, category.LongName);
            }

            if (headerList.Contains(DataModelDictionary.CategoryDictionary[DataModelCategory.ParentCategoryPath]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, GetParentCategoryPath(category.Path));
            }

            if (headerList.Contains(DataModelDictionary.CategoryDictionary[DataModelCategory.HierarchyName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, category.HierarchyName);
            }
        }

        protected override ObjectType ObjectType
        {
            get { return ObjectType.Category; }
        }

        #endregion
    }
}
