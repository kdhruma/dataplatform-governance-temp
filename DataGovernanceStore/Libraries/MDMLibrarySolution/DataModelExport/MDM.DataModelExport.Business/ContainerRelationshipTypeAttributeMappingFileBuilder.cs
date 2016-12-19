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
    using MDM.RelationshipManager.Business;

    /// <summary>
    /// Class provides functionality to export ContainerRelationshipTypeAttributeMapping data as an excel file.
    /// </summary>
    internal class ContainerRelationshipTypeAttributeMappingFileBuilder : DataModelFileBuilderBase
    {
        #region Overridden Methods

        protected override IDataModelObjectCollection GetDataModelObjectCollection(BO.CallerContext callerContext)
        {
            BO.ContainerRelationshipTypeAttributeMappingCollection masterCollection = new BO.ContainerRelationshipTypeAttributeMappingCollection();
            ContainerRelationshipTypeAttributeMappingBL mappingBL = new ContainerRelationshipTypeAttributeMappingBL();
            BO.ContainerRelationshipTypeAttributeMappingCollection containerRelationshipTypeAttributeMappings = mappingBL.GetAll(callerContext);

            foreach (BO.ContainerRelationshipTypeAttributeMapping containerRelationshipTypeAttributeMapping in containerRelationshipTypeAttributeMappings)
            {
                if (containerRelationshipTypeAttributeMapping.IsSpecialized)
                {
                    masterCollection.Add(containerRelationshipTypeAttributeMapping);
                }
            }

            return masterCollection;
        }

        protected override IDataModelObjectCollection GetDataModelObjectCollection(DataModelExportContext dataModelExportContext, BO.CallerContext callerContext)
        {
            BO.ContainerRelationshipTypeAttributeMappingCollection masterCollection = new BO.ContainerRelationshipTypeAttributeMappingCollection();
            ContainerRelationshipTypeAttributeMappingBL mappingBL = new ContainerRelationshipTypeAttributeMappingBL();
            BO.ContainerRelationshipTypeAttributeMappingCollection containerRelationshipTypeAttributeMappings = new BO.ContainerRelationshipTypeAttributeMappingCollection();
            BO.RelationshipTypeCollection relTypes = new RelationshipTypeBL().GetAll(callerContext);

            if (dataModelExportContext.ContainerIdList != null && dataModelExportContext.ContainerIdList.Count > 0 && relTypes != null && relTypes.Count > 0)
            {
                foreach (Int32 containerId in dataModelExportContext.ContainerIdList)
                {
                    foreach (BO.RelationshipType relationshipType in relTypes)
                    {
                        BO.ContainerRelationshipTypeAttributeMappingCollection mappingsByContainer = mappingBL.Get(containerId, relationshipType.Id, -1, callerContext);
                        containerRelationshipTypeAttributeMappings.AddRange(mappingsByContainer);
                    }
                }

                foreach (BO.ContainerRelationshipTypeAttributeMapping containerRelationshipTypeAttributeMapping in containerRelationshipTypeAttributeMappings)
                {
                    if (containerRelationshipTypeAttributeMapping.IsSpecialized && !masterCollection.Contains(containerRelationshipTypeAttributeMapping))
                    {
                        masterCollection.Add(containerRelationshipTypeAttributeMapping);
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
 
            BO.ContainerRelationshipTypeAttributeMapping mappingObject = (BO.ContainerRelationshipTypeAttributeMapping)dataModelObject;

            if (headerList.Contains(DataModelDictionary.ContainerRelationshipTypeAttributeMappingDictionary[DataModelContainerRelationshipTypeAttributeMapping.OrganizationName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, mappingObject.OrganizationName);
            }

            if (headerList.Contains(DataModelDictionary.ContainerRelationshipTypeAttributeMappingDictionary[DataModelContainerRelationshipTypeAttributeMapping.ContainerName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, mappingObject.ContainerName);
            }

            if (headerList.Contains(DataModelDictionary.ContainerRelationshipTypeAttributeMappingDictionary[DataModelContainerRelationshipTypeAttributeMapping.RelationshipTypeName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, mappingObject.RelationshipTypeName);
            }

            if (headerList.Contains(DataModelDictionary.ContainerRelationshipTypeAttributeMappingDictionary[DataModelContainerRelationshipTypeAttributeMapping.AttributePath]))
            {
                String attributePath = GetAttributePath(mappingObject.AttributeParentName, mappingObject.AttributeName);
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, attributePath);
            }

            if (headerList.Contains(DataModelDictionary.ContainerRelationshipTypeAttributeMappingDictionary[DataModelContainerRelationshipTypeAttributeMapping.Required]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ConvertBooleanValuesToString(mappingObject.Required));
            }

            if (headerList.Contains(DataModelDictionary.ContainerRelationshipTypeAttributeMappingDictionary[DataModelContainerRelationshipTypeAttributeMapping.ReadOnly]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ConvertBooleanValuesToString(mappingObject.ReadOnly));
            }

            if (headerList.Contains(DataModelDictionary.ContainerRelationshipTypeAttributeMappingDictionary[DataModelContainerRelationshipTypeAttributeMapping.ShowInline]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ConvertBooleanValuesToString(mappingObject.ShowInline));
            }

            if (headerList.Contains(DataModelDictionary.ContainerRelationshipTypeAttributeMappingDictionary[DataModelContainerRelationshipTypeAttributeMapping.SortOrder]))
            {
                OpenSpreadsheetUtility.AppendRowWithNumberCell(dataRow, columnIndex++, mappingObject.SortOrder);
            }

            if (headerList.Contains(DataModelDictionary.ContainerRelationshipTypeAttributeMappingDictionary[DataModelContainerRelationshipTypeAttributeMapping.AutoPromotable]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ConvertBooleanValuesToString(mappingObject.AutoPromotable));
            }
        }

        protected override ObjectType ObjectType
        {
            get { return ObjectType.ContainerRelationshipTypeAttributeMapping; }
        }

        #endregion
    }
}
