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
    /// Class provides functionality to export ContainerRelationshipTypeEntityTypeMapping data as an excel file.
    /// </summary>
    internal class ContainerRelationshipTypeEntityTypeMappingFileBuilder : DataModelFileBuilderBase
    {
        protected override IDataModelObjectCollection GetDataModelObjectCollection(BO.CallerContext callerContext)
        {
            ContainerRelationshipTypeEntityTypeMappingBL containerRelationshipTypeEntityTypeMappingBL = new ContainerRelationshipTypeEntityTypeMappingBL();
            BO.ContainerRelationshipTypeEntityTypeMappingCollection filteredMappings = new BO.ContainerRelationshipTypeEntityTypeMappingCollection();

            BO.ContainerRelationshipTypeEntityTypeMappingCollection containerRelationshipTypeEntityTypeMappings = containerRelationshipTypeEntityTypeMappingBL.GetAll(callerContext);

            if (containerRelationshipTypeEntityTypeMappings != null && containerRelationshipTypeEntityTypeMappings.Count > 0)
            {
                foreach (BO.ContainerRelationshipTypeEntityTypeMapping containerRelationshipTypeEntityTypeMapping in containerRelationshipTypeEntityTypeMappings)
                {
                    if (containerRelationshipTypeEntityTypeMapping.IsSpecialized)
                    {
                        filteredMappings.Add(containerRelationshipTypeEntityTypeMapping);
                    }
                }
            }

            return filteredMappings;
        }

        protected override IDataModelObjectCollection GetDataModelObjectCollection(DataModelExportContext dataModelExportContext, BO.CallerContext callerContext)
        {
            ContainerRelationshipTypeEntityTypeMappingBL containerRelationshipTypeEntityTypeMappingBL = new ContainerRelationshipTypeEntityTypeMappingBL();
            BO.ContainerRelationshipTypeEntityTypeMappingCollection filteredMappings = new BO.ContainerRelationshipTypeEntityTypeMappingCollection();
            BO.ContainerRelationshipTypeEntityTypeMappingCollection containerRelationshipTypeEntityTypeMappings = new BO.ContainerRelationshipTypeEntityTypeMappingCollection();

            if (dataModelExportContext.ContainerIdList != null && dataModelExportContext.ContainerIdList.Count > 0)
            {
                BO.EntityModelContext entityModelContext = new BO.EntityModelContext();
                foreach (Int32 containerId in dataModelExportContext.ContainerIdList)
                {
                    entityModelContext.ContainerId = containerId;
                    BO.ContainerRelationshipTypeEntityTypeMappingCollection mappingsByContainer =
                                containerRelationshipTypeEntityTypeMappingBL.GetMappingsByContext(entityModelContext, callerContext);
                    containerRelationshipTypeEntityTypeMappings.AddRange(mappingsByContainer);
                }

                if (containerRelationshipTypeEntityTypeMappings != null && containerRelationshipTypeEntityTypeMappings.Count > 0)
                {
                    foreach (BO.ContainerRelationshipTypeEntityTypeMapping containerRelationshipTypeEntityTypeMapping in containerRelationshipTypeEntityTypeMappings)
                    {
                        if (containerRelationshipTypeEntityTypeMapping.IsSpecialized)
                        {
                            filteredMappings.Add(containerRelationshipTypeEntityTypeMapping);
                        }
                    }
                }
            }
            else
            {
                return GetDataModelObjectCollection(callerContext);
            }

            return filteredMappings;
        }

        protected override void PopulateDataModelObjectInDataRow(IDataModelObject dataModelObject, List<String> headerList, Row dataRow)
        {
            UInt32 columnIndex = 1;

            PopulateDataModelBasePropertiesInDataRow(dataModelObject, headerList, dataRow, ref columnIndex);

            BO.ContainerRelationshipTypeEntityTypeMapping mappingObject = (BO.ContainerRelationshipTypeEntityTypeMapping)dataModelObject;

            if (headerList.Contains(DataModelDictionary.ContainerRelationshipTypeEntityTypeMappingDictionary[DataModelContainerRelationshipTypeEntityTypeMapping.OrganizationName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, mappingObject.OrganizationName);
            }

            if (headerList.Contains(DataModelDictionary.ContainerRelationshipTypeEntityTypeMappingDictionary[DataModelContainerRelationshipTypeEntityTypeMapping.ContainerName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, mappingObject.ContainerName);
            }

            if (headerList.Contains(DataModelDictionary.ContainerRelationshipTypeEntityTypeMappingDictionary[DataModelContainerRelationshipTypeEntityTypeMapping.RelationshipTypeName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, mappingObject.RelationshipTypeName);
            }

            if (headerList.Contains(DataModelDictionary.ContainerRelationshipTypeEntityTypeMappingDictionary[DataModelContainerRelationshipTypeEntityTypeMapping.EntityTypeName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, mappingObject.EntityTypeName);
            }

            if (headerList.Contains(DataModelDictionary.ContainerRelationshipTypeEntityTypeMappingDictionary[DataModelContainerRelationshipTypeEntityTypeMapping.DrillDown]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ConvertBooleanValuesToString(mappingObject.DrillDown));
            }

            if (headerList.Contains(DataModelDictionary.ContainerRelationshipTypeEntityTypeMappingDictionary[DataModelContainerRelationshipTypeEntityTypeMapping.ReadOnly]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ConvertBooleanValuesToString(mappingObject.ReadOnly));
            }

            if (headerList.Contains(DataModelDictionary.ContainerRelationshipTypeEntityTypeMappingDictionary[DataModelContainerRelationshipTypeEntityTypeMapping.IsDefaultRelation]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ConvertBooleanValuesToString(mappingObject.IsDefaultRelation));
            }

            if (headerList.Contains(DataModelDictionary.ContainerRelationshipTypeEntityTypeMappingDictionary[DataModelContainerRelationshipTypeEntityTypeMapping.Excludable]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ConvertBooleanValuesToString(mappingObject.Excludable));
            }
        }

        protected override ObjectType ObjectType
        {
            get { return ObjectType.ContainerRelationshipTypeEntityTypeMapping; }
        }
    }
}
