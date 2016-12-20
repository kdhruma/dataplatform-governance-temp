using System;
using System.Collections.Generic;

using DocumentFormat.OpenXml.Spreadsheet;

namespace MDM.DataModelExport.Business
{
    using MDM.Core;
    using MDM.Core.DataModel;
    using MDM.ExcelUtility;
    using MDM.Interfaces;
    using MDM.ContainerManager.Business;
    using BO = MDM.BusinessObjects;
    using MDM.BusinessObjects.Exports;

    /// <summary>
    /// Class provides functionality to export Container data model as an excel file.
    /// </summary>
    internal class ContainerDataModelFileBuilder : DataModelFileBuilderBase
    {
        protected override IDataModelObjectCollection GetDataModelObjectCollection(BO.CallerContext callerContext)
        {
            ContainerBL containerBL = new ContainerBL();
            return containerBL.GetAll(callerContext);
        }

        protected override IDataModelObjectCollection GetDataModelObjectCollection(DataModelExportContext dataModelExportContext, BO.CallerContext callerContext)
        {
            ContainerBL containerBL = new ContainerBL();
            BO.ContainerCollection containers = new BO.ContainerCollection();

            if (dataModelExportContext.ContainerIdList != null && dataModelExportContext.ContainerIdList.Count > 0)
            {
                containers = containerBL.GetByIds(dataModelExportContext.ContainerIdList, callerContext);
            }
            else
            {
                return GetDataModelObjectCollection(callerContext);
            }

            return containers;
        }

        protected override void PopulateDataModelObjectInDataRow(IDataModelObject dataModelObject, List<String> headerList, Row dataRow)
        {
            UInt32 columnIndex = 1;

            PopulateDataModelBasePropertiesInDataRow(dataModelObject, headerList, dataRow, ref columnIndex);

            BO.Container container = (BO.Container)dataModelObject;            

            if (headerList.Contains(DataModelDictionary.ContainerDictionary[DataModelContainer.ContainerName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, container.Name);
            }

            if (headerList.Contains(DataModelDictionary.ContainerDictionary[DataModelContainer.ContainerLongName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, container.LongName);
            }

            if (headerList.Contains(DataModelDictionary.ContainerDictionary[DataModelContainer.OrganizationName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, container.OrganizationShortName);
            }

            if (headerList.Contains(DataModelDictionary.ContainerDictionary[DataModelContainer.HierarchyName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, container.HierarchyShortName);
            }

            if (headerList.Contains(DataModelDictionary.ContainerDictionary[DataModelContainer.ContainerType]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, container.ContainerType);
            }

            if (headerList.Contains(DataModelDictionary.ContainerDictionary[DataModelContainer.ContainerQualifier]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, container.ContainerQualifierName);
            }

            if (headerList.Contains(DataModelDictionary.ContainerDictionary[DataModelContainer.ContainerSecondaryQualifiers]))
            {
                String containerSecondaryQualifier = ValueTypeHelper.JoinCollection(container.ContainerSecondaryQualifiers, Constants.STRING_PATH_SEPARATOR);
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, containerSecondaryQualifier);
            }

            if (headerList.Contains(DataModelDictionary.ContainerDictionary[DataModelContainer.ParentContainerName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, container.ParentContainerName);
            }

            if (headerList.Contains(DataModelDictionary.ContainerDictionary[DataModelContainer.NeedsApprovedCopy]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ConvertBooleanValuesToString(container.NeedsApprovedCopy));
            }

            if (headerList.Contains(DataModelDictionary.ContainerDictionary[DataModelContainer.WorkflowType]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, container.WorkflowType);
            }

            if (headerList.Contains(DataModelDictionary.ContainerDictionary[DataModelContainer.AutoExtensionEnabled]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ConvertBooleanValuesToString(container.AutoExtensionEnabled));
            }
        }

        protected override ObjectType ObjectType
        {
            get { return ObjectType.Catalog; }
        }
    }
}