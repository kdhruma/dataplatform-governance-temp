using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Linq;

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
    /// Class provides functionality to export Container locale map data as an excel file.
    /// </summary>
    internal class ContainerLocaleMapFileBuilder : DataModelFileBuilderBase
    {
        #region Overridden Methods

        protected override IDataModelObjectCollection GetDataModelObjectCollection(BO.CallerContext callerContext)
        {
            ContainerBL containerBL = new ContainerBL();
            BO.ContainerCollection allContainers = new BO.ContainerCollection();
            BO.ContainerCollection containers = containerBL.GetAll(new BO.ContainerContext(), callerContext);

            if (containers != null && containers.Count > 0)
            {
                foreach (BO.Container container in containers)
                {
                    foreach (BO.Locale localeEnum in container.SupportedLocales)
                    {
                        BO.Container clonedContainer = (BO.Container)container.Clone();

                        clonedContainer.SupportedLocales = new BO.LocaleCollection() { localeEnum };
                        allContainers.Add(clonedContainer, true);
                    }
                }
            }

            return allContainers;
        }

        protected override IDataModelObjectCollection GetDataModelObjectCollection(DataModelExportContext dataModelExportContext, BO.CallerContext callerContext)
        {
            ContainerBL containerBL = new ContainerBL();
            BO.ContainerCollection allContainers = new BO.ContainerCollection();
            BO.ContainerCollection containers = new BO.ContainerCollection();

            if (dataModelExportContext.Locales != null && dataModelExportContext.Locales.Count > 0)
            {
                if (dataModelExportContext.ContainerIdList != null && dataModelExportContext.ContainerIdList.Count > 0)
                {
                    containers = containerBL.GetByIds(dataModelExportContext.ContainerIdList, callerContext);
                }
                else
                {
                    containers = containerBL.GetAll(new BO.ContainerContext(), callerContext);
                }

                if (containers != null && containers.Count > 0)
                {
                    foreach (BO.Container container in containers)
                    {
                        foreach (BO.Locale locale in container.SupportedLocales)
                        {
                            BO.Container clonedContainer = (BO.Container)container.Clone();

                            clonedContainer.SupportedLocales = new BO.LocaleCollection() { locale };
                            allContainers.Add(clonedContainer, true);
                        }
                    }
                }
            }

            return allContainers;
        }

        protected override void PopulateDataModelObjectInDataRow(IDataModelObject dataModelObject, List<String> headerList, Row dataRow)
        {
            UInt32 columnIndex = 1;

            PopulateDataModelBasePropertiesInDataRow(dataModelObject, headerList, dataRow, ref columnIndex);

            BO.Container container = (BO.Container)dataModelObject;           

            if (headerList.Contains(DataModelDictionary.ContainerLocalizationDictionary[DataModelContainerLocalization.ContainerName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, container.Name);
            }

            if (headerList.Contains(DataModelDictionary.ContainerLocalizationDictionary[DataModelContainerLocalization.Locale]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, container.SupportedLocales.FirstOrDefault().Locale);
            }
        }

        protected override ObjectType ObjectType
        {
            get { return ObjectType.ContainerLocalization; }
        }

        #endregion
    }
}