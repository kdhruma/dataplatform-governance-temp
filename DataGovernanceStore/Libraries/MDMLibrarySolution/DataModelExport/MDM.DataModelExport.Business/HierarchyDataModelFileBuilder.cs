using System;
using System.Collections.Generic;

using DocumentFormat.OpenXml.Spreadsheet;

namespace MDM.DataModelExport.Business
{
    using MDM.Core;
    using MDM.Core.DataModel;
    using MDM.ExcelUtility;
    using MDM.Interfaces;
    using MDM.HierarchyManager.Business;
    using BO = MDM.BusinessObjects;
    using MDM.BusinessObjects.Exports;

    /// <summary>
    /// Class provides functionality to export Hierarchy data model as an excel file.
    /// </summary>
    internal class HierarchyDataModelFileBuilder : DataModelFileBuilderBase
    {
        protected override IDataModelObjectCollection GetDataModelObjectCollection(BO.CallerContext callerContext)
        {
            HierarchyBL hierarchyBL = new HierarchyBL();
            return hierarchyBL.GetAll(callerContext);
        }

        protected override IDataModelObjectCollection GetDataModelObjectCollection(DataModelExportContext dataModelExportContext, BO.CallerContext callerContext)
        {
            HierarchyBL hierarchyBL = new HierarchyBL();
            BO.HierarchyCollection hierarchies = new BO.HierarchyCollection();

            if (dataModelExportContext.HierarchyIdList != null && dataModelExportContext.HierarchyIdList.Count > 0)
            {
                hierarchies = hierarchyBL.GetByIds(dataModelExportContext.HierarchyIdList, callerContext);
            }
            else
            {
                return GetDataModelObjectCollection(callerContext);
            }

            return hierarchies;
        }

        protected override void PopulateDataModelObjectInDataRow(IDataModelObject dataModelObject, List<String> headerList, Row dataRow)
        {
            UInt32 columnIndex = 1;

            PopulateDataModelBasePropertiesInDataRow(dataModelObject, headerList, dataRow, ref columnIndex);

            BO.Hierarchy hierarchy = (BO.Hierarchy)dataModelObject;            

            if (headerList.Contains(DataModelDictionary.HierarchyDictionary[DataModelHierarchy.HierarchyName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, hierarchy.Name);
            }

            if (headerList.Contains(DataModelDictionary.HierarchyDictionary[DataModelHierarchy.HierarchyLongName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, hierarchy.LongName);
            }

            if (headerList.Contains(DataModelDictionary.HierarchyDictionary[DataModelHierarchy.LeafNodeOnly]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ConvertBooleanValuesToString(hierarchy.LeafNodeOnly));
            }
        }

        protected override ObjectType ObjectType
        {
            get { return ObjectType.Taxonomy; }
        }
    }
}