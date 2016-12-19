using System;
using System.Collections.Generic;

using DocumentFormat.OpenXml.Spreadsheet;

namespace MDM.DataModelExport.Business
{
    using MDM.Core;
    using MDM.Core.DataModel;
    using MDM.ExcelUtility;
    using MDM.Interfaces;
    using MDM.RelationshipManager.Business;
    using BO = MDM.BusinessObjects;
    using MDM.BusinessObjects.Exports;

    /// <summary>
    /// Class provides functionality to export RelationshipType data as an excel file.
    /// </summary>
    internal class RelationshipTypeDataModelFileBuilder : DataModelFileBuilderBase
    {
        #region Overridden Methods

        protected override IDataModelObjectCollection GetDataModelObjectCollection(BO.CallerContext callerContext)
        {
            RelationshipTypeBL relationshipTypeBL = new RelationshipTypeBL();
            BO.RelationshipTypeCollection relationshipTypes = relationshipTypeBL.GetAll(callerContext);
            return relationshipTypes;
        }

        protected override IDataModelObjectCollection GetDataModelObjectCollection(DataModelExportContext dataModelExportContext, BO.CallerContext callerContext)
        {
            RelationshipTypeBL relationshipTypeBL = new RelationshipTypeBL();
            BO.RelationshipTypeCollection relationshipTypes = new BO.RelationshipTypeCollection();

            if (dataModelExportContext.RelationshipTypeIdList != null && dataModelExportContext.RelationshipTypeIdList.Count > 0)
            {
                relationshipTypes = relationshipTypeBL.GetByIds(dataModelExportContext.RelationshipTypeIdList, callerContext);
            }
            else
            {
                return GetDataModelObjectCollection(callerContext);
            }

            return relationshipTypes;
        }

        protected override void PopulateDataModelObjectInDataRow(IDataModelObject dataModelObject, List<String> headerList, Row dataRow)
        {
            UInt32 columnIndex = 1;

            PopulateDataModelBasePropertiesInDataRow(dataModelObject, headerList, dataRow, ref columnIndex);

            BO.RelationshipType relationshipType = (BO.RelationshipType)dataModelObject;            

            if (headerList.Contains(DataModelDictionary.RelationshipTypeDictionary[DataModelRelationshipType.RelationshipTypeName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, relationshipType.Name);
            }

            if (headerList.Contains(DataModelDictionary.RelationshipTypeDictionary[DataModelRelationshipType.RelationshipTypeLongName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, relationshipType.LongName);
            }

            if (headerList.Contains(DataModelDictionary.RelationshipTypeDictionary[DataModelRelationshipType.EnforceRelatedEntityStateOnSourceEntity]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ConvertBooleanValuesToString(relationshipType.EnforceRelatedEntityStateOnSourceEntity));
            }

            if (headerList.Contains(DataModelDictionary.RelationshipTypeDictionary[DataModelRelationshipType.CheckRelatedEntityPromoteStatusOnPromote]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ConvertBooleanValuesToString(relationshipType.CheckRelatedEntityPromoteStatusOnPromote));
            }
        }

        protected override ObjectType ObjectType
        {
            get { return ObjectType.RelationshipType; }
        }

        #endregion
    }
}
