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
    /// Class provides functionality to export RelationshipTypeEntityTypeCardinalityFileBuilder data as an excel file.
    /// </summary>
    internal class RelationshipTypeEntityTypeCardinalityFileBuilder : DataModelFileBuilderBase
    {
        #region Overridden Methods

        protected override IDataModelObjectCollection GetDataModelObjectCollection(BO.CallerContext callerContext)
        {
            BO.RelationshipTypeEntityTypeMappingCardinalityCollection cardinalityCollection = new RelationshipTypeEntityTypeMappingCardinalityBL().GetAll(callerContext);
            return cardinalityCollection;
        }

        protected override IDataModelObjectCollection GetDataModelObjectCollection(DataModelExportContext dataModelExportContext, BO.CallerContext callerContext)
        {
            RelationshipTypeEntityTypeMappingCardinalityBL relationshipTypeEntityTypeMappingCardinalityBL = new RelationshipTypeEntityTypeMappingCardinalityBL();
            BO.RelationshipTypeEntityTypeMappingCardinalityCollection cardinalityCollection = new BO.RelationshipTypeEntityTypeMappingCardinalityCollection();

            if (dataModelExportContext.EntityTypeIdList != null && dataModelExportContext.EntityTypeIdList.Count > 0)
            {
                foreach (Int32 entityTypeId in dataModelExportContext.EntityTypeIdList)
                {
                    BO.RelationshipTypeEntityTypeMappingCardinalityCollection cardinalitiesBasedOnEntityType =
                                    relationshipTypeEntityTypeMappingCardinalityBL.GetByFromEntityTypeId(entityTypeId, callerContext);
                    cardinalityCollection.AddRange(cardinalitiesBasedOnEntityType);
                }
            }
            else
            {
                return GetDataModelObjectCollection(callerContext);
            }

            return cardinalityCollection;
        }

        protected override void PopulateDataModelObjectInDataRow(IDataModelObject dataModelObject, List<String> headerList, Row dataRow)
        {
            UInt32 columnIndex = 1;

            PopulateDataModelBasePropertiesInDataRow(dataModelObject, headerList, dataRow, ref columnIndex);

            BO.RelationshipTypeEntityTypeMappingCardinality mappingObject = (BO.RelationshipTypeEntityTypeMappingCardinality)dataModelObject;

            if (headerList.Contains(DataModelDictionary.RelationshipTypeEntityTypeMappingCardinalityDictionary[DataModelRelationshipTypeEntityTypeMappingCardinality.RelationshipTypeName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, mappingObject.RelationshipTypeName);
            }

            if (headerList.Contains(DataModelDictionary.RelationshipTypeEntityTypeMappingCardinalityDictionary[DataModelRelationshipTypeEntityTypeMappingCardinality.EntityTypeName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, mappingObject.EntityTypeName);
            }

            if (headerList.Contains(DataModelDictionary.RelationshipTypeEntityTypeMappingCardinalityDictionary[DataModelRelationshipTypeEntityTypeMappingCardinality.ToEntityTypeName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, mappingObject.ToEntityTypeName);
            }

            if (headerList.Contains(DataModelDictionary.RelationshipTypeEntityTypeMappingCardinalityDictionary[DataModelRelationshipTypeEntityTypeMappingCardinality.MinRequired]))
            {
                OpenSpreadsheetUtility.AppendRowWithNumberCell(dataRow, columnIndex++, mappingObject.MinRelationships);
            }

            if (headerList.Contains(DataModelDictionary.RelationshipTypeEntityTypeMappingCardinalityDictionary[DataModelRelationshipTypeEntityTypeMappingCardinality.MaxAllowed]))
            {
                OpenSpreadsheetUtility.AppendRowWithNumberCell(dataRow, columnIndex++, mappingObject.MaxRelationships);
            }
        }

        protected override ObjectType ObjectType
        {
            get { return ObjectType.RelationshipTypeEntityTypeMappingCardinality; }
        }

        #endregion
    }
}
