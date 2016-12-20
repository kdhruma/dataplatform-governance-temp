using System;
using System.Diagnostics;

namespace MDM.ProfileManager.Business
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Imports;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.Utility;

    /// <summary>
    /// Represents the DataModel import profile business logic layer.
    /// It used to get the dataModel import profile details and performing dataModel import profile process operations.
    /// </summary>
    public class DataModelImportProfileBL : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// Field denoting the security principal
        /// </summary>
        private SecurityPrincipal _securityPrincipal = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public DataModelImportProfileBL()
        {
            _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
        }

        #endregion Constructors

        #region Get

        /// <summary>
        /// Get the specified DataModel profile by id.
        /// </summary>
        /// <param name="profileId">The profile id.</param>
        /// <param name="callerContext">The caller context.</param>
        /// <returns>Return the DataModel Import profile object</returns>
        public DataModelImportProfile GetProfileById(Int32 profileId, CallerContext callerContext)
        {
            #region Validate input parameters

            if (profileId < 1)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "profileId parameter is not valid.", MDMTraceSource.DataModelImport);
                return null;
            }

            if (callerContext == null)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "callerContext parameter can not be null or empty.", MDMTraceSource.DataModelImport);
                return null;
            }

            #endregion

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("MDM.ProfileManager.Business.DataModelImportProfileBL.Get-ById", MDMTraceSource.DataModelImport, false);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "Start : MDM.ProfileManager.Business.ImportProfileBL.Get-ById", MDMTraceSource.DataModelImport);
            }

            #region Validate User Permission

            if (!ProfileUtility.ValidateUserPermission(_securityPrincipal.CurrentUserId))
            {
                throw new MDMOperationException("101146", "You do not have sufficient permission to perform this operation", "ImportProfileManager.GetAllDataModelImportProfiles", String.Empty, "Get");
            }

            #endregion Validate User Permission

            DataModelImportProfile lkpImportProfile = null;
            JobProfile jobProfile = new JobProfileBL().Get(profileId, callerContext.Application, MDMCenterModules.Import);

            if (jobProfile != null)
            {
                lkpImportProfile = new DataModelImportProfile(jobProfile.ProfileDataXml); 

                lkpImportProfile.Id = jobProfile.Id;
                lkpImportProfile.Name = jobProfile.Name;
            }

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "End : MDM.ProfileManager.Business.DataModelImportProfileBL.Get-ById", MDMTraceSource.DataModelImport);
                MDMTraceHelper.StopTraceActivity("MDM.ProfileManager.Business.ImportProfileBL.Get-ById", MDMTraceSource.DataModelImport);
            }

            return lkpImportProfile;
        }

        /// <summary>
        /// Gets the specified dataModel profile by name.
        /// </summary>
        /// <param name="profileName">Name of the profile.</param>
        /// <param name="callerContext">The caller context.</param>
        /// <returns>Return the DataModel Import profile</returns>
        public DataModelImportProfile GetProfileByName(String profileName, CallerContext callerContext)
        {
            #region Validate input parameters

            if (String.IsNullOrEmpty(profileName))
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "callerContext parameter can not be null or empty.", MDMTraceSource.DataModelImport);
                return null;
            }

            if (callerContext == null)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "callerContext parameter can not be null or empty.", MDMTraceSource.DataModelImport);
                return null;
            }

            #endregion

            #region Validate User Permission

            if (!ProfileUtility.ValidateUserPermission(_securityPrincipal.CurrentUserId))
            {
                throw new MDMOperationException("101146", "You do not have sufficient permission to perform this operation", "ImportProfileManager.GetAllDataModelImportProfiles", String.Empty, "Get");
            }

            #endregion Validate User Permission

            MDMTraceHelper.StartTraceActivity("MDM.ProfileManager.Business.DataModelImportProfileBL.Get-ByName", MDMTraceSource.DataModelImport, false);
            MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "Start : MDM.ProfileManager.Business.DataModelImportProfileBL.Get-ByName", MDMTraceSource.DataModelImport);

            DataModelImportProfile lkpImportProfile = null;
            JobProfile jobProfile = new JobProfileBL().Get(profileName, callerContext.Application, MDMCenterModules.Import);

            if (jobProfile != null)
            {
                lkpImportProfile = new DataModelImportProfile(jobProfile.ProfileDataXml); 

                lkpImportProfile.Id = jobProfile.Id;
                lkpImportProfile.Name = jobProfile.Name;
            }

            MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "End : MDM.ProfileManager.Business.DataModelImportProfileBL.Get-ByName", MDMTraceSource.DataModelImport);
            MDMTraceHelper.StopTraceActivity("MDM.ProfileManager.Business.DataModelImportProfileBL.Get-ByName", MDMTraceSource.DataModelImport);

            return lkpImportProfile;
        }

        /// <summary>
        /// Gets all DataModel import profiles.
        /// </summary>
        /// <param name="callerContext">Indicates the caller context.</param>
        /// <returns>DataModel Import profile collection</returns>
        public DataModelImportProfileCollection GetAllDataModelImportProfiles(CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("MDM.ProfileManager.Business.DataModelImportProfileBL.GetAllDataModelImportProfiles", MDMTraceSource.JobService, false);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "Start : MDM.ProfileManager.Business.DataModelImportProfileBL.GetAllDataModelImportProfiles", MDMTraceSource.JobService);
            }

            #region Validate User Permission

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Validating user permission started...", MDMTraceSource.Imports);
            }

            if (!ProfileUtility.ValidateUserPermission(_securityPrincipal.CurrentUserId))
            {
                throw new MDMOperationException("101146", "You do not have sufficient permission to perform this operation", "ImportProfileManager.GetAllDataModelImportProfiles", String.Empty, "Get");
            }

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Validating user permission completed...", MDMTraceSource.JobService);
            }

            #endregion Validate User Permission

            DataModelImportProfileCollection dataModelImportProfileCollection = new DataModelImportProfileCollection();
            JobProfileCollection jobProfileCollection = new JobProfileBL().GetAll(callerContext.Application, MDMCenterModules.Import, JobType.DataModelImport);

            if (jobProfileCollection != null)
            {
                foreach (JobProfile jobProfile in jobProfileCollection)
                {
                    DataModelImportProfile dataModelImportProfile = new DataModelImportProfile(jobProfile.ProfileDataXml);

                    dataModelImportProfile.Id = jobProfile.Id;
                    dataModelImportProfile.Name = jobProfile.Name;
                    dataModelImportProfile.ProfileDomain = jobProfile.ProfileDomain;
                    dataModelImportProfile.FileType = jobProfile.FileType;
                    dataModelImportProfile.IsSystemProfile = jobProfile.IsSystemProfile;

                    dataModelImportProfileCollection.Add(dataModelImportProfile);
                }
            }

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "End : MDM.ProfileManager.Business.DataModelImportProfileBL.GetAllDataModelImportProfiles", MDMTraceSource.JobService);
                MDMTraceHelper.StopTraceActivity("MDM.ProfileManager.Business.DataModelImportProfileBL.GetAllDataModelImportProfiles", MDMTraceSource.JobService);
            }

            return dataModelImportProfileCollection;
        }

      
        #endregion Get

        #region Private Methods

        #endregion 
    }
}
