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
    /// Class provides functionality to export ContainerEntityTypeMapping data as an excel file.
    /// </summary>
    internal class ContainerEntityTypeMappingFileBuilder : DataModelFileBuilderBase
    {
        protected override IDataModelObjectCollection GetDataModelObjectCollection(BO.CallerContext callerContext)
        {
            ContainerEntityTypeMappingBL containerEntityTypeMappingBL = new ContainerEntityTypeMappingBL();
            BO.ContainerEntityTypeMappingCollection masterContainerEntityTypeMappingCollection = new BO.ContainerEntityTypeMappingCollection();
            BO.ContainerCollection allContainers = null;
            HashSet<KeyValuePair<Int32, Int32>> containerEntityType = new HashSet<KeyValuePair<Int32, Int32>>();

            BO.ContainerEntityTypeMappingCollection containerEntityTypeMappingCollection = containerEntityTypeMappingBL.GetAll(callerContext);

            if (containerEntityTypeMappingCollection != null)
            {
                foreach (BO.ContainerEntityTypeMapping containerEntityTypeMapping in containerEntityTypeMappingCollection)
                {
                    if (containerEntityTypeMapping.ContainerId == 0 && containerEntityTypeMapping.ContainerName == ALL || containerEntityTypeMapping.OrganizationId == 0 && containerEntityTypeMapping.OrganizationName == ALL)
                    {
                        //If EntityType is mapped with AllOrganizations and AllContainers, Get Mappings for each container and entity type.
                        if (allContainers == null)
                        {
                            ContainerBL containerManager = new ContainerBL();
                            allContainers = containerManager.GetAll(callerContext);
                        }

                        if (allContainers != null)
                        {
                            foreach (BO.Container container in allContainers)
                            {
                                if (!containerEntityType.Contains(new KeyValuePair<Int32, Int32>(container.Id, containerEntityTypeMapping.EntityTypeId)))
                                {
                                    BO.ContainerEntityTypeMapping newContainerEntityTypeMapping = new BO.ContainerEntityTypeMapping();

                                    newContainerEntityTypeMapping.Id = containerEntityTypeMapping.Id;
                                    newContainerEntityTypeMapping.Action = containerEntityTypeMapping.Action;
                                    newContainerEntityTypeMapping.ContainerName = container.Name;
                                    newContainerEntityTypeMapping.OrganizationName = container.OrganizationShortName;
                                    newContainerEntityTypeMapping.EntityTypeName = containerEntityTypeMapping.EntityTypeName;

                                    masterContainerEntityTypeMappingCollection.Add(newContainerEntityTypeMapping);
                                    containerEntityType.Add(new KeyValuePair<Int32, Int32>(container.Id, containerEntityTypeMapping.EntityTypeId));
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!containerEntityType.Contains(new KeyValuePair<Int32, Int32>(containerEntityTypeMapping.ContainerId, containerEntityTypeMapping.EntityTypeId)))
                        {
                            masterContainerEntityTypeMappingCollection.Add(containerEntityTypeMapping);
                            containerEntityType.Add(new KeyValuePair<Int32, Int32>(containerEntityTypeMapping.ContainerId, containerEntityTypeMapping.EntityTypeId));
                        }
                    }
                }
            }

            return masterContainerEntityTypeMappingCollection;
        }

        protected override IDataModelObjectCollection GetDataModelObjectCollection(DataModelExportContext dataModelExportContext, BO.CallerContext callerContext)
        {
            ContainerEntityTypeMappingBL containerEntityTypeMappingBL = new ContainerEntityTypeMappingBL();
            BO.ContainerEntityTypeMappingCollection masterContainerEntityTypeMappingCollection = new BO.ContainerEntityTypeMappingCollection();
            BO.ContainerCollection allContainers = null;
            HashSet<KeyValuePair<Int32, Int32>> containerEntityType = new HashSet<KeyValuePair<Int32, Int32>>();
            BO.ContainerEntityTypeMappingCollection containerEntityTypeMappingCollection = new BO.ContainerEntityTypeMappingCollection();
            BO.ContainerEntityTypeMappingCollection exportedContainerEntityTypeMappings = new BO.ContainerEntityTypeMappingCollection();

            if (dataModelExportContext.ContainerIdList != null && dataModelExportContext.ContainerIdList.Count > 0)
            {
                foreach (Int32 containerId in dataModelExportContext.ContainerIdList)
                {
                    containerEntityTypeMappingCollection = containerEntityTypeMappingBL.GetMappingsByContainerId(containerId, callerContext);

                    if (containerEntityTypeMappingCollection != null)
                    {
                        exportedContainerEntityTypeMappings.AddRange(containerEntityTypeMappingCollection);
                    }
                }

                if (exportedContainerEntityTypeMappings != null)
                {
                    foreach (BO.ContainerEntityTypeMapping containerEntityTypeMapping in exportedContainerEntityTypeMappings)
                    {
                        if (containerEntityTypeMapping.ContainerId == 0 && containerEntityTypeMapping.ContainerName == ALL || containerEntityTypeMapping.OrganizationId == 0 && containerEntityTypeMapping.OrganizationName == ALL)
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
                                    if (!containerEntityType.Contains(new KeyValuePair<Int32, Int32>(container.Id, containerEntityTypeMapping.EntityTypeId)))
                                    {
                                        BO.ContainerEntityTypeMapping newContainerEntityTypeMapping = new BO.ContainerEntityTypeMapping();

                                        newContainerEntityTypeMapping.Id = containerEntityTypeMapping.Id;
                                        newContainerEntityTypeMapping.Action = containerEntityTypeMapping.Action;
                                        newContainerEntityTypeMapping.ContainerName = container.Name;
                                        newContainerEntityTypeMapping.OrganizationName = container.OrganizationShortName;
                                        newContainerEntityTypeMapping.EntityTypeName = containerEntityTypeMapping.EntityTypeName;

                                        masterContainerEntityTypeMappingCollection.Add(newContainerEntityTypeMapping);
                                        containerEntityType.Add(new KeyValuePair<Int32, Int32>(container.Id, containerEntityTypeMapping.EntityTypeId));
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!containerEntityType.Contains(new KeyValuePair<Int32, Int32>(containerEntityTypeMapping.ContainerId, containerEntityTypeMapping.EntityTypeId)))
                            {
                                masterContainerEntityTypeMappingCollection.Add(containerEntityTypeMapping);
                                containerEntityType.Add(new KeyValuePair<Int32, Int32>(containerEntityTypeMapping.ContainerId, containerEntityTypeMapping.EntityTypeId));
                            }
                        }
                    }
                }
            }
            else
            {
                return GetDataModelObjectCollection(callerContext);
            }

            return masterContainerEntityTypeMappingCollection;
        }

        protected override void PopulateDataModelObjectInDataRow(IDataModelObject dataModelObject, List<String> headerList, Row dataRow)
        {
            UInt32 columnIndex = 1;

            PopulateDataModelBasePropertiesInDataRow(dataModelObject, headerList, dataRow, ref columnIndex);

            BO.ContainerEntityTypeMapping mappingObject = (BO.ContainerEntityTypeMapping)dataModelObject;

            if (headerList.Contains(DataModelDictionary.ContainerEntityTypeMappingDictionary[DataModelContainerEntityTypeMapping.OrganizationName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, mappingObject.OrganizationName);
            }

            if (headerList.Contains(DataModelDictionary.ContainerEntityTypeMappingDictionary[DataModelContainerEntityTypeMapping.ContainerName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, mappingObject.ContainerName);
            }

            if (headerList.Contains(DataModelDictionary.ContainerEntityTypeMappingDictionary[DataModelContainerEntityTypeMapping.EntityTypeName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, mappingObject.EntityTypeName);
            }

            if (headerList.Contains(DataModelDictionary.ContainerEntityTypeMappingDictionary[DataModelContainerEntityTypeMapping.ShowAtCreation]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ConvertBooleanValuesToString(mappingObject.ShowAtCreation));
            }

            if (headerList.Contains(DataModelDictionary.ContainerEntityTypeMappingDictionary[DataModelContainerEntityTypeMapping.MinimumExtensions]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, mappingObject.MinimumExtensions);
            }

            if (headerList.Contains(DataModelDictionary.ContainerEntityTypeMappingDictionary[DataModelContainerEntityTypeMapping.MaximumExtensions]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, mappingObject.MaximumExtensions);
            }
        }

        protected override ObjectType ObjectType
        {
            get { return ObjectType.ContainerEntityTypeMapping; }
        }
    }
}
