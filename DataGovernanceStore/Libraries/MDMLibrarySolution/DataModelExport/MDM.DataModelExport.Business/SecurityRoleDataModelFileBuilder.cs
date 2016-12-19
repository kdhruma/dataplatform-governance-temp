using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

using DocumentFormat.OpenXml.Spreadsheet;

namespace MDM.DataModelExport.Business
{
    using MDM.Core;
    using MDM.Core.DataModel;
    using MDM.ExcelUtility;
    using MDM.Interfaces;
    using MDM.AdminManager.Business;
    using BO = MDM.BusinessObjects;
    using MDM.BusinessObjects.Exports;

    /// <summary>
    /// Class provides functionality to export Security Role data as an excel file.
    /// </summary>
    internal class SecurityRoleDataModelFileBuilder : DataModelFileBuilderBase
    {
        private BO.CallerContext  roleCallerContext = null;

        #region Overridden Methods

        protected override IDataModelObjectCollection GetDataModelObjectCollection(BO.CallerContext callerContext)
        {
            roleCallerContext = callerContext;
            SecurityRoleBL securityRoleBL = new SecurityRoleBL();
            BO.SecurityRoleCollection securityRoles = securityRoleBL.GetAll(callerContext);
            Collection<String> internalSecurityRoleNames = MDM.Core.DataModel.InternalObjectCollection.SecurityRoleNames;
            
            BO.SecurityRoleCollection filteredSecurityRoles = new BO.SecurityRoleCollection();

            foreach(BO.SecurityRole securityRole in securityRoles)
            {
                if(!internalSecurityRoleNames.Contains(securityRole.Name.ToLowerInvariant()))
                {
                    filteredSecurityRoles.Add(securityRole);
                }
            }

            return filteredSecurityRoles;
        }

        protected override IDataModelObjectCollection GetDataModelObjectCollection(DataModelExportContext dataModelExportContext, BO.CallerContext callerContext)
        {
            return GetDataModelObjectCollection(callerContext);
        }

        protected override void PopulateDataModelObjectInDataRow(IDataModelObject dataModelObject, List<String> headerList, Row dataRow)
        {
            UInt32 columnIndex = 1;
            
            PopulateDataModelBasePropertiesInDataRow(dataModelObject, headerList, dataRow, ref columnIndex);

            BO.SecurityRole securityRole = (BO.SecurityRole)dataModelObject;
            
            if (headerList.Contains(DataModelDictionary.SecurityRoleDictionary[DataModelSecurityRole.ShortName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, securityRole.Name);
            }

            if (headerList.Contains(DataModelDictionary.SecurityRoleDictionary[DataModelSecurityRole.LongName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, securityRole.LongName);
            }

            if (headerList.Contains(DataModelDictionary.SecurityRoleDictionary[DataModelSecurityRole.IsPrivateRole]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ConvertBooleanValuesToString(securityRole.IsPrivateRole));
            }
        }

        protected override ObjectType ObjectType
        {
            get { return ObjectType.Role; }
        }

        #endregion
    }
}
