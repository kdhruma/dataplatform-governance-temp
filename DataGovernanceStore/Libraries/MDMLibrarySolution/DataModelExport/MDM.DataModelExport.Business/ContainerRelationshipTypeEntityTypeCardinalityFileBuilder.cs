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
    /// Class provides functionality to export ContainerRelationshipTypeEntityTypeCardinality data as an excel file.
    /// </summary>
    internal class ContainerRelationshipTypeEntityTypeCardinalityFileBuilder : DataModelFileBuilderBase
    {
        #region Overridden Methods

        protected override IDataModelObjectCollection GetDataModelObjectCollection(BO.CallerContext callerContext)
        {
            BO.ContainerRelationshipTypeEntityTypeMappingCardinalityCollection mappingCollection = new ContainerRelationshipTypeEntityTypeMappingCardinalityBL().GetAll(callerContext);
            BO.ContainerRelationshipTypeEntityTypeMappingCardinalityCollection filteredMappings = new BO.ContainerRelationshipTypeEntityTypeMappingCardinalityCollection();

            if (mappingCollection != null && mappingCollection.Count > 0)
            {
                foreach (BO.ContainerRelationshipTypeEntityTypeMappingCardinality mapping in mappingCollection)
                {
                    if (mapping.IsSpecialized)
                    {
                        filteredMappings.Add(mapping);
                    }
                }
            }

            return filteredMappings;
        }

        protected override IDataModelObjectCollection GetDataModelObjectCollection(DataModelExportContext dataModelExportContext, BO.CallerContext callerContext)
        {
            BO.ContainerRelationshipTypeEntityTypeMappingCardinalityCollection mappingCollection = new BO.ContainerRelationshipTypeEntityTypeMappingCardinalityCollection();
            BO.ContainerRelationshipTypeEntityTypeMappingCardinalityCollection filteredMappings = new BO.ContainerRelationshipTypeEntityTypeMappingCardinalityCollection();

            if (dataModelExportContext.ContainerIdList != null && dataModelExportContext.ContainerIdList.Count > 0)
            {
                foreach (Int32 containerId in dataModelExportContext.ContainerIdList)
                {
                    BO.ContainerRelationshipTypeEntityTypeMappingCardinalityCollection mappingsByContainerId =
                             new ContainerRelationshipTypeEntityTypeMappingCardinalityBL().Get(containerId, -1, -1, callerContext);
                    mappingCollection.AddRange(mappingsByContainerId);
                }

                if (mappingCollection != null && mappingCollection.Count > 0)
                {
                    foreach (BO.ContainerRelationshipTypeEntityTypeMappingCardinality mapping in mappingCollection)
                    {
                        if (mapping.IsSpecialized)
                        {
                            filteredMappings.Add(mapping);
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

            BO.ContainerRelationshipTypeEntityTypeMappingCardinality mappingObject = (BO.ContainerRelationshipTypeEntityTypeMappingCardinality)dataModelObject;

            if (headerList.Contains(DataModelDictionary.ContainerRelationshipTypeEntityTypeMappingCardinalityDictionary[DataModelContainerRelationshipTypeEntityTypeMappingCardinality.OrganizationName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, mappingObject.OrganizationName);
            }

            if (headerList.Contains(DataModelDictionary.ContainerRelationshipTypeEntityTypeMappingCardinalityDictionary[DataModelContainerRelationshipTypeEntityTypeMappingCardinality.ContainerName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, mappingObject.ContainerName);
            }

            if (headerList.Contains(DataModelDictionary.ContainerRelationshipTypeEntityTypeMappingCardinalityDictionary[DataModelContainerRelationshipTypeEntityTypeMappingCardinality.RelationshipTypeName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, mappingObject.RelationshipTypeName);
            }

            if (headerList.Contains(DataModelDictionary.ContainerRelationshipTypeEntityTypeMappingCardinalityDictionary[DataModelContainerRelationshipTypeEntityTypeMappingCardinality.EntityTypeName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, mappingObject.EntityTypeName);
            }

            if (headerList.Contains(DataModelDictionary.ContainerRelationshipTypeEntityTypeMappingCardinalityDictionary[DataModelContainerRelationshipTypeEntityTypeMappingCardinality.ToEntityTypeName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, mappingObject.ToEntityTypeName);
            }

            if (headerList.Contains(DataModelDictionary.ContainerRelationshipTypeEntityTypeMappingCardinalityDictionary[DataModelContainerRelationshipTypeEntityTypeMappingCardinality.MinRequired]))
            {
                OpenSpreadsheetUtility.AppendRowWithNumberCell(dataRow, columnIndex++, mappingObject.MinRelationships);
            }

            if (headerList.Contains(DataModelDictionary.ContainerRelationshipTypeEntityTypeMappingCardinalityDictionary[DataModelContainerRelationshipTypeEntityTypeMappingCardinality.MaxAllowed]))
            {
                OpenSpreadsheetUtility.AppendRowWithNumberCell(dataRow, columnIndex++, mappingObject.MaxRelationships);
            }
        }

        protected override ObjectType ObjectType
        {
            get { return ObjectType.ContainerRelationshipTypeEntityTypeMappingCardinality; }
        }

        #endregion
    }
}