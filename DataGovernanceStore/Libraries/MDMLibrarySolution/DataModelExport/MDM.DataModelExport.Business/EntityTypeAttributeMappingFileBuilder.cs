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
    using BO = MDM.BusinessObjects;
    using MDM.BusinessObjects.Exports;

    /// <summary>
    /// Class provides functionality to export EntityTypeAttributeMapping data as an excel file.
    /// </summary>
    internal class EntityTypeAttributeMappingFileBuilder : DataModelFileBuilderBase
    {
        protected override IDataModelObjectCollection GetDataModelObjectCollection(BO.CallerContext callerContext)
        {
            EntityTypeAttributeMappingBL entityTypeAttributeMappingBL = new EntityTypeAttributeMappingBL();
            BO.EntityTypeAttributeMappingCollection entityTypeAttributeMappingCollection = entityTypeAttributeMappingBL.GetAll(callerContext);
            return entityTypeAttributeMappingCollection;
        }

        protected override IDataModelObjectCollection GetDataModelObjectCollection(DataModelExportContext dataModelExportContext, BO.CallerContext callerContext)
        {
            BO.EntityTypeAttributeMappingCollection allEntityTypeAttributeMappingCollection = new BO.EntityTypeAttributeMappingCollection();

            if (dataModelExportContext.EntityTypeIdList != null && dataModelExportContext.EntityTypeIdList.Count > 0)
            {
                EntityTypeAttributeMappingBL entityTypeAttributeMappingBL = new EntityTypeAttributeMappingBL();

                foreach (Int32 entityTypeId in dataModelExportContext.EntityTypeIdList)
                {
                    BO.EntityTypeAttributeMappingCollection entityTypeAttributeMappingCollection = entityTypeAttributeMappingBL.GetMappingsByEntityTypeId(entityTypeId, callerContext);
                    allEntityTypeAttributeMappingCollection.AddRange(entityTypeAttributeMappingCollection);
                }
            }
            else
            {
                return GetDataModelObjectCollection(callerContext);
            }

            return allEntityTypeAttributeMappingCollection;
        }

        protected override void PopulateDataModelObjectInDataRow(IDataModelObject dataModelObject, List<String> headerList, Row dataRow)
        {
            UInt32 columnIndex = 1;

            PopulateDataModelBasePropertiesInDataRow(dataModelObject, headerList, dataRow, ref columnIndex);

            BO.EntityTypeAttributeMapping mappingObject = (BO.EntityTypeAttributeMapping)dataModelObject;

            if (headerList.Contains(DataModelDictionary.EntityTypeAttributeMappingDictionary[DataModelEntityTypeAttributeMapping.EntityTypeName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, mappingObject.EntityTypeName);
            }

            if (headerList.Contains(DataModelDictionary.EntityTypeAttributeMappingDictionary[DataModelEntityTypeAttributeMapping.AttributePath]))
            {
                String attributePath = GetAttributePath(mappingObject.AttributeParentName, mappingObject.AttributeName);
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, attributePath);
            }

            if (headerList.Contains(DataModelDictionary.EntityTypeAttributeMappingDictionary[DataModelEntityTypeAttributeMapping.Required]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ConvertBooleanValuesToString(mappingObject.Required));
            }

            if (headerList.Contains(DataModelDictionary.EntityTypeAttributeMappingDictionary[DataModelEntityTypeAttributeMapping.ReadOnly]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ConvertBooleanValuesToString(mappingObject.ReadOnly));
            }

            if (headerList.Contains(DataModelDictionary.EntityTypeAttributeMappingDictionary[DataModelEntityTypeAttributeMapping.Extension]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, mappingObject.Extension);
            }

            if (headerList.Contains(DataModelDictionary.EntityTypeAttributeMappingDictionary[DataModelEntityTypeAttributeMapping.ShowAtCreation]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ConvertBooleanValuesToString(mappingObject.ShowAtCreation));
            }

            if (headerList.Contains(DataModelDictionary.EntityTypeAttributeMappingDictionary[DataModelEntityTypeAttributeMapping.SortOrder]))
            {
                OpenSpreadsheetUtility.AppendRowWithNumberCell(dataRow, columnIndex++, mappingObject.SortOrder);
            }
        }

        protected override ObjectType ObjectType
        {
            get { return ObjectType.EntityTypeAttributeMapping; }
        }
    }
}
