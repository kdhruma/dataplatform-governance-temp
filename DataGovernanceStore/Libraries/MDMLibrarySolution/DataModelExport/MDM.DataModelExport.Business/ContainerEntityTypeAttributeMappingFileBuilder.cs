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
    using MDM.ContainerManager.Business;
    using MDM.BusinessObjects.Exports;

    /// <summary>
    /// Class provides functionality to export ContainerEntityTypeAttributeMapping data as an excel file.
    /// </summary>
    internal class ContainerEntityTypeAttributeMappingFileBuilder : DataModelFileBuilderBase
    {
        protected override IDataModelObjectCollection GetDataModelObjectCollection(BO.CallerContext callerContext)
        {
            // Get ContainerEntityTypeMappingCollection
            BO.ContainerEntityTypeMappingCollection containerEntityTypeMappings = new ContainerEntityTypeMappingBL().GetAll(callerContext);
            BO.ContainerEntityTypeAttributeMappingCollection allMappings = new BO.ContainerEntityTypeAttributeMappingCollection();
            BO.ContainerCollection allContainers = null;
            HashSet<KeyValuePair<Int32, Int32>> containerEntityType = new HashSet<KeyValuePair<Int32, Int32>>();

            foreach (BO.ContainerEntityTypeMapping containerEntityTypeMapping in containerEntityTypeMappings)
            {
                if (containerEntityTypeMapping.ContainerId == 0 && containerEntityTypeMapping.ContainerName == ALL && containerEntityTypeMapping.OrganizationId == 0 && containerEntityTypeMapping.OrganizationName == ALL)
                {
                    //If EntityType is mapped with AllOrganizations and AllContainers, Get Mappings for each container and entity type.
                    if (allContainers == null)
                    {
                        ContainerBL conatinerManager = new ContainerBL();
                        allContainers = conatinerManager.GetAll(callerContext);
                    }

                    if (allContainers != null)
                    {
                        foreach (BO.Container container in allContainers)
                        {
                            containerEntityType.Add(new KeyValuePair<Int32, Int32>(container.Id, containerEntityTypeMapping.EntityTypeId));
                        }
                    }
                }
                else
                {
                    containerEntityType.Add(new KeyValuePair<Int32, Int32>(containerEntityTypeMapping.ContainerId, containerEntityTypeMapping.EntityTypeId));
                }
            }

            allMappings = GetContainerEntityTypeAttributeMappings(containerEntityType, callerContext);

            return allMappings;
        }

        protected override IDataModelObjectCollection GetDataModelObjectCollection(DataModelExportContext dataModelExportContext, BO.CallerContext callerContext)
        {
            BO.ContainerEntityTypeAttributeMappingCollection allMappings = new BO.ContainerEntityTypeAttributeMappingCollection();

            // Get ContainerEntityTypeMappingCollection
            if (dataModelExportContext.ContainerIdList != null && dataModelExportContext.ContainerIdList.Count > 0)
            {
                BO.ContainerEntityTypeMappingCollection containerEntityTypeMappings = new BO.ContainerEntityTypeMappingCollection();
                BO.ContainerEntityTypeMappingCollection containerBasedcontainerEntityTypeMappings = new BO.ContainerEntityTypeMappingCollection();

                foreach (Int32 containerId in dataModelExportContext.ContainerIdList)
                {
                    containerBasedcontainerEntityTypeMappings = new ContainerEntityTypeMappingBL().GetMappingsByContainerId(containerId, callerContext);
                    containerEntityTypeMappings.AddRange(containerBasedcontainerEntityTypeMappings);
                }

                BO.ContainerCollection allContainers = null;
                HashSet<KeyValuePair<Int32, Int32>> containerEntityType = new HashSet<KeyValuePair<Int32, Int32>>();

                foreach (BO.ContainerEntityTypeMapping containerEntityTypeMapping in containerEntityTypeMappings)
                {
                    if (containerEntityTypeMapping.ContainerId == 0 && containerEntityTypeMapping.ContainerName == ALL && containerEntityTypeMapping.OrganizationId == 0 && containerEntityTypeMapping.OrganizationName == ALL)
                    {
                        //If EntityType is mapped with AllOrganizations and AllContainers, Get Mappings for each container and entity type.
                        if (allContainers == null)
                        {
                            ContainerBL conatinerManager = new ContainerBL();
                            allContainers = conatinerManager.GetByIds(dataModelExportContext.ContainerIdList, callerContext);
                        }

                        if (allContainers != null)
                        {
                            foreach (BO.Container container in allContainers)
                            {
                                containerEntityType.Add(new KeyValuePair<Int32, Int32>(container.Id, containerEntityTypeMapping.EntityTypeId));
                            }
                        }
                    }
                    else
                    {
                        containerEntityType.Add(new KeyValuePair<Int32, Int32>(containerEntityTypeMapping.ContainerId, containerEntityTypeMapping.EntityTypeId));
                    }
                }

                allMappings = GetContainerEntityTypeAttributeMappings(containerEntityType, callerContext);
            }
            else
            {
                return GetDataModelObjectCollection(callerContext);
            }

            return allMappings;
        }

        protected override void PopulateDataModelObjectInDataRow(IDataModelObject dataModelObject, List<String> headerList, Row dataRow)
        {
            UInt32 columnIndex = 1;

            PopulateDataModelBasePropertiesInDataRow(dataModelObject, headerList, dataRow, ref columnIndex);

            BO.ContainerEntityTypeAttributeMapping mappingObject = (BO.ContainerEntityTypeAttributeMapping)dataModelObject;

            if (headerList.Contains(DataModelDictionary.ContainerEntityTypeAttributeMappingDictionary[DataModelContainerEntityTypeAttributeMapping.OrganizationName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, mappingObject.OrganizationName);
            }
            if (headerList.Contains(DataModelDictionary.ContainerEntityTypeAttributeMappingDictionary[DataModelContainerEntityTypeAttributeMapping.ContainerName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, mappingObject.ContainerName);
            }

            if (headerList.Contains(DataModelDictionary.ContainerEntityTypeAttributeMappingDictionary[DataModelContainerEntityTypeAttributeMapping.EntityTypeName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, mappingObject.EntityTypeName);
            }

            if (headerList.Contains(DataModelDictionary.ContainerEntityTypeAttributeMappingDictionary[DataModelContainerEntityTypeAttributeMapping.AttributePath]))
            {
                String attributePath = GetAttributePath(mappingObject.AttributeParentName, mappingObject.AttributeName);
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, attributePath);
            }

            if (headerList.Contains(DataModelDictionary.ContainerEntityTypeAttributeMappingDictionary[DataModelContainerEntityTypeAttributeMapping.Required]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ConvertBooleanValuesToString(mappingObject.Required));
            }

            if (headerList.Contains(DataModelDictionary.ContainerEntityTypeAttributeMappingDictionary[DataModelContainerEntityTypeAttributeMapping.ReadOnly]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ConvertBooleanValuesToString(mappingObject.ReadOnly));
            }

            if (headerList.Contains(DataModelDictionary.ContainerEntityTypeAttributeMappingDictionary[DataModelContainerEntityTypeAttributeMapping.Extension]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, mappingObject.Extension);
            }

            if (headerList.Contains(DataModelDictionary.ContainerEntityTypeAttributeMappingDictionary[DataModelContainerEntityTypeAttributeMapping.ShowAtCreation]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ConvertBooleanValuesToString(mappingObject.ShowAtCreation));
            }

            if (headerList.Contains(DataModelDictionary.ContainerEntityTypeAttributeMappingDictionary[DataModelContainerEntityTypeAttributeMapping.SortOrder]))
            {
                OpenSpreadsheetUtility.AppendRowWithNumberCell(dataRow, columnIndex++, mappingObject.SortOrder);
            }

            if (headerList.Contains(DataModelDictionary.ContainerEntityTypeAttributeMappingDictionary[DataModelContainerEntityTypeAttributeMapping.InheritableOnly]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ConvertBooleanValuesToString(mappingObject.InheritableOnly));
            }

            if (headerList.Contains(DataModelDictionary.ContainerEntityTypeAttributeMappingDictionary[DataModelContainerEntityTypeAttributeMapping.AutoPromotable]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ConvertBooleanValuesToString(mappingObject.AutoPromotable));
            }
        }

        protected override ObjectType ObjectType
        {
            get { return ObjectType.ContainerEntityTypeAttributeMapping; }
        }

        private BO.ContainerEntityTypeAttributeMappingCollection GetContainerEntityTypeAttributeMappings(HashSet<KeyValuePair<Int32, Int32>> containerEntityTypes, BO.CallerContext callerContext)
        {
            ContainerEntityTypeAttributeMappingBL containerEntityTypeAttributeMappingBL = new ContainerEntityTypeAttributeMappingBL();
            BO.ContainerEntityTypeAttributeMappingCollection allMappings = new BO.ContainerEntityTypeAttributeMappingCollection();

            foreach (KeyValuePair<Int32,Int32> mapping in containerEntityTypes)
            {
                BO.ContainerEntityTypeAttributeMappingCollection containerEntityTypeAttributeMappings = containerEntityTypeAttributeMappingBL.Get(mapping.Key, mapping.Value, -1, -1, callerContext);

                if (containerEntityTypeAttributeMappings != null && containerEntityTypeAttributeMappings.Count > 0)
                {
                    foreach (BO.ContainerEntityTypeAttributeMapping containerEntityTypeAttributeMapping in containerEntityTypeAttributeMappings)
                    {
                        if (containerEntityTypeAttributeMapping.IsSpecialized)
                        {
                            allMappings.Add(containerEntityTypeAttributeMapping);
                        }
                    }
                }
            }

            return allMappings;
        }
    }
}
