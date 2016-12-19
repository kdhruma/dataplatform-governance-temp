using System;
using System.Collections.Generic;

using DocumentFormat.OpenXml.Spreadsheet;

namespace MDM.DataModelExport.Business
{
    using MDM.Core;
    using MDM.Core.DataModel;
    using MDM.ExcelUtility;
    using MDM.Interfaces;
    using MDM.DataModelManager.Business;
    using MDM.HierarchyManager.Business;
    using BO = MDM.BusinessObjects;
    using MDM.BusinessObjects.Exports;

    /// <summary>
    /// Class provides functionality to export CategoryAttributeMapping data as an excel file.
    /// </summary>
    internal class CategoryAttributeMappingFileBuilder : DataModelFileBuilderBase
    {
        protected override IDataModelObjectCollection GetDataModelObjectCollection(BO.CallerContext callerContext)
        {
            BO.CategoryAttributeMappingCollection masterCollection = new BO.CategoryAttributeMappingCollection();

            BO.HierarchyCollection hierarchyCollection = new HierarchyBL().GetAll(callerContext);
            CategoryAttributeMappingBL mappingBL = new CategoryAttributeMappingBL();

            foreach (BO.Hierarchy hierarchy in hierarchyCollection)
            {
                BO.CategoryAttributeMappingCollection categoryAttributeMappings = mappingBL.GetByHierarchyId(hierarchy.Id, callerContext);

                if (categoryAttributeMappings != null && categoryAttributeMappings.Count > 0)
                {
                    foreach (BO.CategoryAttributeMapping categoryAttributeMapping in categoryAttributeMappings)
                    {
                        if (categoryAttributeMapping.SourceFlag.Equals("O"))
                        {
                            masterCollection.Add(categoryAttributeMapping);
                        }
                    }
                }
            }

            return masterCollection;
        }

        protected override IDataModelObjectCollection GetDataModelObjectCollection(DataModelExportContext dataModelExportContext, BO.CallerContext callerContext)
        {
            BO.CategoryAttributeMappingCollection masterCollection = new BO.CategoryAttributeMappingCollection();

            if (dataModelExportContext.HierarchyIdList != null && dataModelExportContext.HierarchyIdList.Count > 0)
            {
                BO.HierarchyCollection hierarchyCollection = new HierarchyBL().GetByIds(dataModelExportContext.HierarchyIdList, callerContext);
                CategoryAttributeMappingBL mappingBL = new CategoryAttributeMappingBL();

                foreach (BO.Hierarchy hierarchy in hierarchyCollection)
                {
                    BO.CategoryAttributeMappingCollection categoryAttributeMappings = mappingBL.GetByHierarchyId(hierarchy.Id, callerContext);

                    if (categoryAttributeMappings != null && categoryAttributeMappings.Count > 0)
                    {
                        foreach (BO.CategoryAttributeMapping categoryAttributeMapping in categoryAttributeMappings)
                        {
                            if (categoryAttributeMapping.SourceFlag.Equals("O"))
                            {
                                masterCollection.Add(categoryAttributeMapping);
                            }
                        }
                    }
                }
            }
            else
            {
                return GetDataModelObjectCollection(callerContext);
            }

            return masterCollection;
        }

        protected override void PopulateDataModelObjectInDataRow(IDataModelObject dataModelObject, List<String> headerList, Row dataRow)
        {
            UInt32 columnIndex = 1;

            PopulateDataModelBasePropertiesInDataRow(dataModelObject, headerList, dataRow, ref columnIndex);
            
            BO.CategoryAttributeMapping categoryAttributeMapping = (BO.CategoryAttributeMapping)dataModelObject;

            if (headerList.Contains(DataModelDictionary.CategoryAttributeMappingDictionary[DataModelCategoryAttributeMapping.HierarchyName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, categoryAttributeMapping.HierarchyName);
            }

            if (headerList.Contains(DataModelDictionary.CategoryAttributeMappingDictionary[DataModelCategoryAttributeMapping.CategoryName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, categoryAttributeMapping.CategoryName);
            }

            if (headerList.Contains(DataModelDictionary.CategoryAttributeMappingDictionary[DataModelCategoryAttributeMapping.ParentCategoryPath]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, GetParentCategoryPath(categoryAttributeMapping.Path));
            }

            if (headerList.Contains(DataModelDictionary.CategoryAttributeMappingDictionary[DataModelCategoryAttributeMapping.AttributePath]))
            {
                String attributePath = GetAttributePath(categoryAttributeMapping.AttributeParentName, categoryAttributeMapping.AttributeName);
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, attributePath);
            }

            if (headerList.Contains(DataModelDictionary.CategoryAttributeMappingDictionary[DataModelCategoryAttributeMapping.IsRequired]))
            {
                String required = (categoryAttributeMapping.Required != null) ? ConvertBooleanValuesToString((Boolean)categoryAttributeMapping.Required) : String.Empty;
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, required);
            }

            if (headerList.Contains(DataModelDictionary.CategoryAttributeMappingDictionary[DataModelCategoryAttributeMapping.IsReadOnly]))
            {
                String readOnly = (categoryAttributeMapping.ReadOnly != null) ? ConvertBooleanValuesToString((Boolean)categoryAttributeMapping.ReadOnly) : String.Empty;
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, readOnly);
            }

            if (headerList.Contains(DataModelDictionary.CategoryAttributeMappingDictionary[DataModelCategoryAttributeMapping.DefaultValue]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, categoryAttributeMapping.DefaultValue);
            }

            if (headerList.Contains(DataModelDictionary.CategoryAttributeMappingDictionary[DataModelCategoryAttributeMapping.MinimumLength]))
            {
                OpenSpreadsheetUtility.AppendRowWithNumberCell(dataRow, columnIndex++, categoryAttributeMapping.MinLength);
            }

            if (headerList.Contains(DataModelDictionary.CategoryAttributeMappingDictionary[DataModelCategoryAttributeMapping.MaximumLength]))
            {
                OpenSpreadsheetUtility.AppendRowWithNumberCell(dataRow, columnIndex++, categoryAttributeMapping.MaxLength);
            }

            if (headerList.Contains(DataModelDictionary.CategoryAttributeMappingDictionary[DataModelCategoryAttributeMapping.RangeFrom]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, categoryAttributeMapping.RangeFrom);
            }

            if (headerList.Contains(DataModelDictionary.CategoryAttributeMappingDictionary[DataModelCategoryAttributeMapping.IsRangeFromInclusive]))
            {
                String isRangeFromInclusive = String.Empty;

                if (!String.IsNullOrWhiteSpace(categoryAttributeMapping.RangeFrom))
                {
                    isRangeFromInclusive = String.IsNullOrWhiteSpace(categoryAttributeMapping.MinInclusive) == true ? ConvertBooleanValuesToString(false) : ConvertBooleanValuesToString(true);
                }

                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, isRangeFromInclusive);
            }

            if (headerList.Contains(DataModelDictionary.CategoryAttributeMappingDictionary[DataModelCategoryAttributeMapping.RangeTo]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, categoryAttributeMapping.RangeTo);
            }

            if (headerList.Contains(DataModelDictionary.CategoryAttributeMappingDictionary[DataModelCategoryAttributeMapping.IsRangeToInclusive]))
            {
                String isRangeToInclusive = String.Empty;

                if (!String.IsNullOrWhiteSpace(categoryAttributeMapping.RangeTo))
                {
                    isRangeToInclusive = String.IsNullOrWhiteSpace(categoryAttributeMapping.MaxInclusive) == true ? ConvertBooleanValuesToString(false) : ConvertBooleanValuesToString(true);
                }

                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, isRangeToInclusive);
            }

            if (headerList.Contains(DataModelDictionary.CategoryAttributeMappingDictionary[DataModelCategoryAttributeMapping.Precision]))
            {
                OpenSpreadsheetUtility.AppendRowWithNumberCell(dataRow, columnIndex++, categoryAttributeMapping.Precision);
            }

            if (headerList.Contains(DataModelDictionary.CategoryAttributeMappingDictionary[DataModelCategoryAttributeMapping.AllowedUOMs]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, categoryAttributeMapping.AllowableUOM);
            }

            if (headerList.Contains(DataModelDictionary.CategoryAttributeMappingDictionary[DataModelCategoryAttributeMapping.DefaultUOM]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, categoryAttributeMapping.DefaultUOM);
            }

            if (headerList.Contains(DataModelDictionary.CategoryAttributeMappingDictionary[DataModelCategoryAttributeMapping.AllowableValues]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, categoryAttributeMapping.AllowableValues);
            }

            if (headerList.Contains(DataModelDictionary.CategoryAttributeMappingDictionary[DataModelCategoryAttributeMapping.SortOrder]))
            {
                OpenSpreadsheetUtility.AppendRowWithNumberCell(dataRow, columnIndex++, categoryAttributeMapping.SortOrder);
            }

            if (headerList.Contains(DataModelDictionary.CategoryAttributeMappingDictionary[DataModelCategoryAttributeMapping.Definition]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, categoryAttributeMapping.Definition);
            }

            if (headerList.Contains(DataModelDictionary.CategoryAttributeMappingDictionary[DataModelCategoryAttributeMapping.Example]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, categoryAttributeMapping.AttributeExample);
            }

            if (headerList.Contains(DataModelDictionary.CategoryAttributeMappingDictionary[DataModelCategoryAttributeMapping.BusinessRule]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, categoryAttributeMapping.BusinessRule);
            }

            if (headerList.Contains(DataModelDictionary.CategoryAttributeMappingDictionary[DataModelCategoryAttributeMapping.InheritableOnly]))
            {
                String inheritableOnly = (categoryAttributeMapping.InheritableOnly != null) ? ConvertBooleanValuesToString((Boolean)categoryAttributeMapping.InheritableOnly) : String.Empty;
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, inheritableOnly);
            }

            if (headerList.Contains(DataModelDictionary.CategoryAttributeMappingDictionary[DataModelCategoryAttributeMapping.AutoPromotable]))
            {
                String autoPromotable = (categoryAttributeMapping.AutoPromotable != null) ? ConvertBooleanValuesToString((Boolean)categoryAttributeMapping.AutoPromotable) : String.Empty;
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, autoPromotable);
            }
        }

        protected override ObjectType ObjectType
        {
            get { return ObjectType.CategoryAttributeMapping; }
        }
    }
}