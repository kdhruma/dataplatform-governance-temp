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
    using MDM.RelationshipManager.Business;
    using BO = MDM.BusinessObjects;
    using MDM.BusinessObjects.Exports;

    /// <summary>
    /// Class provides functionality to export RelationshipTypeAttributeMapping data as an excel file.
    /// </summary>
    internal class RelationshipTypeAttributeMappingFileBuilder : DataModelFileBuilderBase
    {
        #region Overridden Methods

        protected override IDataModelObjectCollection GetDataModelObjectCollection(BO.CallerContext callerContext)
        {
            BO.RelationshipTypeAttributeMappingCollection mappingCollection = new RelationshipTypeAttributeMappingBL().Get(-1, -1, callerContext);
            return mappingCollection;            
        }

        protected override IDataModelObjectCollection GetDataModelObjectCollection(DataModelExportContext dataModelExportContext, BO.CallerContext callerContext)
        {
            BO.RelationshipTypeAttributeMappingCollection mappingCollection = new BO.RelationshipTypeAttributeMappingCollection();

            if (dataModelExportContext.RelationshipTypeIdList != null && dataModelExportContext.RelationshipTypeIdList.Count > 0)
            {
                BO.RelationshipTypeAttributeMappingCollection mappingByRelationshipType = new BO.RelationshipTypeAttributeMappingCollection();

                foreach (Int32 relationshipTypeId in dataModelExportContext.RelationshipTypeIdList)
                {
                    mappingByRelationshipType = new RelationshipTypeAttributeMappingBL().Get(relationshipTypeId, -1, callerContext);
                    mappingCollection.AddRange(mappingByRelationshipType);
                }
            }
            else
            {
                return GetDataModelObjectCollection(callerContext);
            }

            return mappingCollection;
        }

        protected override void PopulateDataModelObjectInDataRow(IDataModelObject dataModelObject, List<String> headerList, Row dataRow)
        {
            UInt32 columnIndex = 1;

            PopulateDataModelBasePropertiesInDataRow(dataModelObject, headerList, dataRow, ref columnIndex);

            BO.RelationshipTypeAttributeMapping mappingObject = (BO.RelationshipTypeAttributeMapping)dataModelObject;

            if (headerList.Contains(DataModelDictionary.RelationshipTypeAttributeMappingDictionary[DataModelRelationshipTypeAttributeMapping.RelationshipTypeName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, mappingObject.RelationshipTypeName);
            }

            if (headerList.Contains(DataModelDictionary.RelationshipTypeAttributeMappingDictionary[DataModelRelationshipTypeAttributeMapping.AttributePath]))
            {
                String attributePath = GetAttributePath(mappingObject.AttributeParentName, mappingObject.AttributeName);
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, attributePath);
            }

            if (headerList.Contains(DataModelDictionary.RelationshipTypeAttributeMappingDictionary[DataModelRelationshipTypeAttributeMapping.Required]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ConvertBooleanValuesToString(mappingObject.Required));
            }

            if (headerList.Contains(DataModelDictionary.RelationshipTypeAttributeMappingDictionary[DataModelRelationshipTypeAttributeMapping.ReadOnly]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ConvertBooleanValuesToString(mappingObject.ReadOnly));
            }

            if (headerList.Contains(DataModelDictionary.RelationshipTypeAttributeMappingDictionary[DataModelRelationshipTypeAttributeMapping.ShowInline]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ConvertBooleanValuesToString(mappingObject.ShowInline));
            }

            if (headerList.Contains(DataModelDictionary.RelationshipTypeAttributeMappingDictionary[DataModelRelationshipTypeAttributeMapping.SortOrder]))
            {
                OpenSpreadsheetUtility.AppendRowWithNumberCell(dataRow, columnIndex++, mappingObject.SortOrder);
            }
        }

        protected override ObjectType ObjectType
        {
            get { return ObjectType.RelationshipTypeAttributeMapping; }
        }

        #endregion
    }
}
