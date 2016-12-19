using System;
using System.Collections.Generic;
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
    /// Class provides functionality to export Security User data as an excel file.
    /// </summary>
    internal class SecurityUserDataModelFileBuilder : DataModelFileBuilderBase
    {
        #region Fields

        /// <summary>
        /// Indicates dictionary to store value as user preference object with key as security user Id
        /// </summary>
        private Dictionary<Int32, BO.UserPreferences> _idBasedUserPreferenceDictionary = new Dictionary<Int32, BO.UserPreferences>();

        /// <summary>
        /// Indicates the instance of security role collection class
        /// </summary>
        private BO.SecurityRoleCollection securityRoles = new BO.SecurityRoleCollection();

        #endregion

        #region Overridden Methods

        protected override IDataModelObjectCollection GetDataModelObjectCollection(BO.CallerContext callerContext)
        {
            SecurityUserBL securityUserBL = new SecurityUserBL();
            UserPreferencesBL userPreferencesBL = new UserPreferencesBL();
            BO.SecurityUserCollection filteredUsers = new BO.SecurityUserCollection();
            Collection<String> internalSecurityUserNames = InternalObjectCollection.SecurityUserNames;

            BO.SecurityUserCollection users = securityUserBL.GetAll(callerContext);

            if (users != null && users.Count > 0)
            {
                foreach (BO.SecurityUser user in users)
                {
                    // If user is disabled or belongs to internal collection then don't export. This fix is applicable only for FP5 release.
                    if (user.Disabled || internalSecurityUserNames.Contains(user.SecurityUserLogin.ToLowerInvariant()))
                    {
                        continue;
                    }

                    BO.UserPreferences userPreferences = userPreferencesBL.GetUserPreferences(user.SecurityUserLogin);

                    if (!_idBasedUserPreferenceDictionary.ContainsKey(user.Id) && userPreferences != null)
                    {
                        _idBasedUserPreferenceDictionary.Add(user.Id, userPreferences);
                    }

                    filteredUsers.Add(user);
                }
            }

            return filteredUsers;
        }

        protected override IDataModelObjectCollection GetDataModelObjectCollection(DataModelExportContext dataModelExportContext, BO.CallerContext callerContext)
        {
            return GetDataModelObjectCollection(callerContext);
        }

        protected override void PopulateDataModelObjectInDataRow(IDataModelObject dataModelObject, List<String> headerList, Row dataRow)
        {
            UInt32 columnIndex = 1;

            PopulateDataModelBasePropertiesInDataRow(dataModelObject, headerList, dataRow, ref columnIndex);

            BO.SecurityUser securityUser = (BO.SecurityUser)dataModelObject;
            Collection<String> filteredRolesName = new Collection<String>();
            String defaultRole = String.Empty;

            if (headerList.Contains(DataModelDictionary.SecurityUserDictionary[DataModelSecurityUser.Login]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, securityUser.SecurityUserLogin);
            }

            if (headerList.Contains(DataModelDictionary.SecurityUserDictionary[DataModelSecurityUser.Password]))
            {
                //From 7.6.7 version, data model export will not populate password column for security users.
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, String.Empty);
            }

            if (headerList.Contains(DataModelDictionary.SecurityUserDictionary[DataModelSecurityUser.AuthenticationType]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, securityUser.AuthenticationType.ToString());
            }

            if (headerList.Contains(DataModelDictionary.SecurityUserDictionary[DataModelSecurityUser.Roles]))
            {
                if (securityUser != null && securityUser.Roles != null && securityUser.Roles.Count > 0)
                {
                    securityRoles = securityUser.Roles;
                }

                filteredRolesName = RemoveInternalSecurityRole(securityRoles);

                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ValueTypeHelper.JoinCollection(filteredRolesName, ","));
            }

            if (headerList.Contains(DataModelDictionary.SecurityUserDictionary[DataModelSecurityUser.ManagerName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, securityUser.ManagerLogin);
            }

            if (headerList.Contains(DataModelDictionary.SecurityUserDictionary[DataModelSecurityUser.Initials]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, securityUser.Initials);
            }

            if (headerList.Contains(DataModelDictionary.SecurityUserDictionary[DataModelSecurityUser.FirstName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, securityUser.FirstName);
            }

            if (headerList.Contains(DataModelDictionary.SecurityUserDictionary[DataModelSecurityUser.LastName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, securityUser.LastName);
            }

            if (headerList.Contains(DataModelDictionary.SecurityUserDictionary[DataModelSecurityUser.Email]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, securityUser.Smtp);
            }

            BO.UserPreferences userPreference = null;

            _idBasedUserPreferenceDictionary.TryGetValue(securityUser.Id, out userPreference);

            if (userPreference != null)
            {
                if (!userPreference.DefaultRoleName.Equals("systemadmin", StringComparison.OrdinalIgnoreCase))
                {
                    defaultRole = userPreference.DefaultRoleName;
                }

                if (headerList.Contains(DataModelDictionary.SecurityUserDictionary[DataModelSecurityUser.DefaultUILocale]))
                {
                    OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, userPreference.UILocale.ToString());
                }

                if (headerList.Contains(DataModelDictionary.SecurityUserDictionary[DataModelSecurityUser.DefaultDataLocale]))
                {
                    OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, userPreference.DataLocale.ToString());
                }

                if (headerList.Contains(DataModelDictionary.SecurityUserDictionary[DataModelSecurityUser.DefaultOrganization]))
                {
                    OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, userPreference.DefaultOrgName);
                }

                if (headerList.Contains(DataModelDictionary.SecurityUserDictionary[DataModelSecurityUser.DefaultHierarchy]))
                {
                    OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, userPreference.DefaultHierarchyName);
                }

                if (headerList.Contains(DataModelDictionary.SecurityUserDictionary[DataModelSecurityUser.DefaultContainer]))
                {
                    OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, userPreference.DefaultContainerName);
                }

                if (headerList.Contains(DataModelDictionary.SecurityUserDictionary[DataModelSecurityUser.DefaultRole]))
                {
                    OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, defaultRole);
                }

                if (headerList.Contains(DataModelDictionary.SecurityUserDictionary[DataModelSecurityUser.DefaultTimeZone]))
                {
                    OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, userPreference.DefaultTimeZoneShortName);
                }

                if (headerList.Contains(DataModelDictionary.SecurityUserDictionary[DataModelSecurityUser.NoOfRecordsToShowPerPageInDisplayTable]))
                {
                    OpenSpreadsheetUtility.AppendRowWithNumberCell(dataRow, columnIndex++, userPreference.MaxTableRows);
                }

                if (headerList.Contains(DataModelDictionary.SecurityUserDictionary[DataModelSecurityUser.NoOfPagesToShowForDisplayTable]))
                {
                    OpenSpreadsheetUtility.AppendRowWithNumberCell(dataRow, columnIndex++, userPreference.MaxTablePages);
                }
            }
        }

        protected override ObjectType ObjectType
        {
            get { return ObjectType.User; }
        }

        #endregion

        #region Private Methods

        private Collection<String> RemoveInternalSecurityRole(BO.SecurityRoleCollection securityRoles)
        {
            Collection<String> filteredRolesName = new Collection<String>();

            BO.SecurityRoleCollection filteredSecurityRoles = new BO.SecurityRoleCollection();

            foreach (BO.SecurityRole securityRole in securityRoles)
            {
                if (!InternalObjectCollection.SecurityRoleNames.Contains(securityRole.Name.ToLowerInvariant()) && !securityRole.IsPrivateRole)
                {
                    filteredRolesName.Add(securityRole.Name);
                }
            }

            return filteredRolesName;
        }

        #endregion
    }
}