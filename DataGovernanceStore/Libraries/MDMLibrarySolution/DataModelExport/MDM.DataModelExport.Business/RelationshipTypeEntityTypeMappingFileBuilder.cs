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
    /// Class provides functionality to export RelationshipTypeEntityTypeMappingData as an excel file.
    /// </summary>
    internal class RelationshipTypeEntityTypeMappingFileBuilder : DataModelFileBuilderBase
    {
        protected override IDataModelObjectCollection GetDataModelObjectCollection(BO.CallerContext callerContext)
        {
            RelationshipTypeEntityTypeMappingBL relationshipTypeEntityTypeMappingBL = new RelationshipTypeEntityTypeMappingBL();
            BO.RelationshipTypeEntityTypeMappingCollection relationshipTypeEntityTypeMappingCollection = relationshipTypeEntityTypeMappingBL.GetAll(callerContext);
            return relationshipTypeEntityTypeMappingCollection;
        }

        protected override IDataModelObjectCollection GetDataModelObjectCollection(DataModelExportContext dataModelExportContext, BO.CallerContext callerContext)
        {
            RelationshipTypeEntityTypeMappingBL relationshipTypeEntityTypeMappingBL = new RelationshipTypeEntityTypeMappingBL();
            BO.RelationshipTypeEntityTypeMappingCollection relationshipTypeEntityTypeMappings = new BO.RelationshipTypeEntityTypeMappingCollection();

            if (dataModelExportContext.EntityTypeIdList != null && dataModelExportContext.EntityTypeIdList.Count > 0)
            {
                foreach (Int32 entityTypeId in dataModelExportContext.EntityTypeIdList)
                {
                    BO.RelationshipTypeEntityTypeMappingCollection entityTypeBasedRelationshipTypeEntityTypeMappings =
                                                relationshipTypeEntityTypeMappingBL.GetMappingsByEntityTypeId(entityTypeId, callerContext);
                    relationshipTypeEntityTypeMappings.AddRange(entityTypeBasedRelationshipTypeEntityTypeMappings);
                }
            }
            else
            {
                return GetDataModelObjectCollection(callerContext);
            }

            return relationshipTypeEntityTypeMappings;
        }

        protected override void PopulateDataModelObjectInDataRow(IDataModelObject dataModelObject, List<String> headerList, Row dataRow)
        {
            UInt32 columnIndex = 1;

            PopulateDataModelBasePropertiesInDataRow(dataModelObject, headerList, dataRow, ref columnIndex);

            BO.RelationshipTypeEntityTypeMapping mappingObject = (BO.RelationshipTypeEntityTypeMapping)dataModelObject;

            if (headerList.Contains(DataModelDictionary.RelationshipTypeEntityTypeMappingDictionary[DataModelRelationshipTypeEntityTypeMapping.RelationshipTypeName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, mappingObject.RelationshipTypeName);
            }

            if (headerList.Contains(DataModelDictionary.RelationshipTypeEntityTypeMappingDictionary[DataModelRelationshipTypeEntityTypeMapping.EntityTypeName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, mappingObject.EntityTypeName);
            }

            if (headerList.Contains(DataModelDictionary.RelationshipTypeEntityTypeMappingDictionary[DataModelRelationshipTypeEntityTypeMapping.DrillDown]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ConvertBooleanValuesToString(mappingObject.DrillDown));
            }

            if (headerList.Contains(DataModelDictionary.RelationshipTypeEntityTypeMappingDictionary[DataModelRelationshipTypeEntityTypeMapping.ReadOnly]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ConvertBooleanValuesToString(mappingObject.ReadOnly));
            }

            if (headerList.Contains(DataModelDictionary.RelationshipTypeEntityTypeMappingDictionary[DataModelRelationshipTypeEntityTypeMapping.IsDefaultRelation]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ConvertBooleanValuesToString(mappingObject.IsDefaultRelation));
            }

            if (headerList.Contains(DataModelDictionary.RelationshipTypeEntityTypeMappingDictionary[DataModelRelationshipTypeEntityTypeMapping.Excludable]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ConvertBooleanValuesToString(mappingObject.Excludable));
            }
        }

        protected override ObjectType ObjectType
        {
            get { return ObjectType.RelationshipTypeEntityTypeMapping; }
        }
    }
}
