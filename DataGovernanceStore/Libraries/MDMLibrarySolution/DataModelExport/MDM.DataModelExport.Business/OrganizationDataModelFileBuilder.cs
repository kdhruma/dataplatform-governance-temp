using System;
using System.Collections.Generic;

using DocumentFormat.OpenXml.Spreadsheet;

namespace MDM.DataModelExport.Business
{
    using MDM.Core;
    using MDM.Core.DataModel;
    using MDM.ExcelUtility;
    using MDM.Interfaces;
    using MDM.OrganizationManager.Business;
    using BO = MDM.BusinessObjects;
    using MDM.BusinessObjects.Exports;

    /// <summary>
    /// Class provides functionality to export Organization data as an excel file.
    /// </summary>
    internal class OrganizationDataModelFileBuilder : DataModelFileBuilderBase
    {
        #region Overridden Methods

        protected override IDataModelObjectCollection GetDataModelObjectCollection(BO.CallerContext callerContext)
        {
            OrganizationBL organizationBL = new OrganizationBL();
            return organizationBL.GetAll(new BO.OrganizationContext(), callerContext);
        }

        protected override IDataModelObjectCollection GetDataModelObjectCollection(DataModelExportContext dataModelExportContext, BO.CallerContext callerContext)
        {
            OrganizationBL organizationBL = new OrganizationBL();
            BO.OrganizationCollection organizations = new BO.OrganizationCollection();

            if (dataModelExportContext.OrganizationIdList != null && dataModelExportContext.OrganizationIdList.Count > 0)
            {
                organizations = organizationBL.GetOrganizationsByIds(dataModelExportContext.OrganizationIdList, new BO.OrganizationContext(), callerContext);
            }
            else
            {
                return GetDataModelObjectCollection(callerContext);
            }

            return organizations;
        }

        protected override void PopulateDataModelObjectInDataRow(IDataModelObject dataModelObject, List<String> headerList, Row dataRow)
        {
            UInt32 columnIndex = 1;

            PopulateDataModelBasePropertiesInDataRow(dataModelObject, headerList, dataRow, ref columnIndex);

            BO.Organization organization = (BO.Organization)dataModelObject;            

            if (headerList.Contains(DataModelDictionary.OrganizationDictionary[DataModelOrganization.OrganizationName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, organization.Name);
            }

            if (headerList.Contains(DataModelDictionary.OrganizationDictionary[DataModelOrganization.OrganizationLongName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, organization.LongName);
            }

            if (headerList.Contains(DataModelDictionary.OrganizationDictionary[DataModelOrganization.OrganizationParentName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex, organization.ParentOrganizationName);
            }
        }

        protected override ObjectType ObjectType
        {
            get { return ObjectType.Organization; }
        }

        #endregion
    }
}