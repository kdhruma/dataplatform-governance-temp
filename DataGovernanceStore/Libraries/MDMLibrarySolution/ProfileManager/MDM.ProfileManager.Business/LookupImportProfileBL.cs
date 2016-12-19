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
    /// Represents the Lookup import profile business logic layer.
    /// It used to get the lookup import profile details and performing lookup import profile process operations.
    /// </summary>
    public class LookupImportProfileBL : BusinessLogicBase
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
        public LookupImportProfileBL()
        {
            _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
        }

        #endregion Constructors

        #region Get

        /// <summary>
        /// Get the specified Lookup profile by id.
        /// </summary>
        /// <param name="profileId">The profile id.</param>
        /// <param name="callerContext">The caller context.</param>
        /// <returns>Return the Lookup Import profile object</returns>
        public LookupImportProfile GetProfileById(Int32 profileId, CallerContext callerContext)
        {
            #region Validate input parameters

            if (profileId < 1)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "profileId parameter is not valid.", MDMTraceSource.JobService);
                return null;
            }

            if (callerContext == null)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "callerContext parameter can not be null or empty.", MDMTraceSource.JobService);
                return null;
            }

            #endregion

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("MDM.ProfileManager.Business.LookupImportProfileBL.Get-ById", MDMTraceSource.JobService, false);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "Start : MDM.ProfileManager.Business.ImportProfileBL.Get-ById", MDMTraceSource.JobService);
            }

            #region Validate User Permission

            if (!ProfileUtility.ValidateUserPermission(_securityPrincipal.CurrentUserId))
            {
                throw new MDMOperationException("101146", "You do not have sufficient permission to perform this operation", "ImportProfileManager.GetAllLookupImportProfiles", String.Empty, "Get");
            }

            #endregion Validate User Permission

            LookupImportProfile lkpImportProfile = null;
            JobProfile jobProfile = new JobProfileBL().Get(profileId, callerContext.Application, MDMCenterModules.Import);

            if (jobProfile != null)
            {
                lkpImportProfile = new LookupImportProfile(jobProfile.ProfileDataXml); 

                lkpImportProfile.Id = jobProfile.Id;
                lkpImportProfile.Name = jobProfile.Name;
            }

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "End : MDM.ProfileManager.Business.LookupImportProfileBL.Get-ById", MDMTraceSource.JobService);
                MDMTraceHelper.StopTraceActivity("MDM.ProfileManager.Business.ImportProfileBL.Get-ById", MDMTraceSource.JobService);
            }

            return lkpImportProfile;
        }

        /// <summary>
        /// Gets the specified lookup profile by name.
        /// </summary>
        /// <param name="profileName">Name of the profile.</param>
        /// <param name="callerContext">The caller context.</param>
        /// <returns>Return the Lookup Import profile</returns>
        public LookupImportProfile GetProfileByName(String profileName, CallerContext callerContext)
        {
            #region Validate input parameters

            if (String.IsNullOrEmpty(profileName))
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "callerContext parameter can not be null or empty.", MDMTraceSource.JobService);
                return null;
            }

            if (callerContext == null)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "callerContext parameter can not be null or empty.", MDMTraceSource.JobService);
                return null;
            }

            #endregion

            #region Validate User Permission

            if (!ProfileUtility.ValidateUserPermission(_securityPrincipal.CurrentUserId))
            {
                throw new MDMOperationException("101146", "You do not have sufficient permission to perform this operation", "ImportProfileManager.GetAllLookupImportProfiles", String.Empty, "Get");
            }

            #endregion Validate User Permission

            MDMTraceHelper.StartTraceActivity("MDM.ProfileManager.Business.LookupImportProfileBL.Get-ByName", MDMTraceSource.JobService, false);
            MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "Start : MDM.ProfileManager.Business.LookupImportProfileBL.Get-ByName", MDMTraceSource.JobService);

            LookupImportProfile lkpImportProfile = null;
            JobProfile jobProfile = new JobProfileBL().Get(profileName, callerContext.Application, MDMCenterModules.Import);

            if (jobProfile != null)
            {
                lkpImportProfile = new LookupImportProfile(jobProfile.ProfileDataXml); 

                lkpImportProfile.Id = jobProfile.Id;
                lkpImportProfile.Name = jobProfile.Name;
            }

            MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "End : MDM.ProfileManager.Business.LookupImportProfileBL.Get-ByName", MDMTraceSource.JobService);
            MDMTraceHelper.StopTraceActivity("MDM.ProfileManager.Business.LookupImportProfileBL.Get-ByName", MDMTraceSource.JobService);

            return lkpImportProfile;
        }

        /// <summary>
        /// Gets all Lookup import profiles.
        /// </summary>
        /// <param name="callerContext">Indicates the caller context.</param>
        /// <returns>Lookup Import profile collection</returns>
        public LookupImportProfileCollection GetAllLookupImportProfiles(CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("MDM.ProfileManager.Business.LookupImportProfileBL.GetAllLookupImportProfiles", MDMTraceSource.JobService, false);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "Start : MDM.ProfileManager.Business.LookupImportProfileBL.GetAllLookupImportProfiles", MDMTraceSource.JobService);
            }

            #region Validate User Permission

            if(Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Validating user permission started...", MDMTraceSource.Imports);
            }

            if (!ProfileUtility.ValidateUserPermission(_securityPrincipal.CurrentUserId))
            {
                throw new MDMOperationException("101146", "You do not have sufficient permission to perform this operation", "ImportProfileManager.GetAllLookupImportProfiles", String.Empty, "Get");
            }

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Validating user permission completed...", MDMTraceSource.JobService);
            }

            #endregion Validate User Permission

            LookupImportProfileCollection lkpImportProfileCollection = new LookupImportProfileCollection();
            JobProfileCollection jobProfileCollection = new JobProfileBL().GetAll(callerContext.Application, MDMCenterModules.Import, JobType.LookupImport);

            if (jobProfileCollection != null)
            {
                foreach (JobProfile jobProfile in jobProfileCollection)
                {
                    LookupImportProfile lkpImportProfile = new LookupImportProfile(jobProfile.ProfileDataXml); 

                    lkpImportProfile.Id = jobProfile.Id;
                    lkpImportProfile.Name = jobProfile.Name;
                    lkpImportProfile.ProfileDomain = jobProfile.ProfileDomain;
                    lkpImportProfile.FileType = jobProfile.FileType;
                    lkpImportProfile.IsSystemProfile = jobProfile.IsSystemProfile;

                    lkpImportProfileCollection.Add(lkpImportProfile);
                }
            }

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "End : MDM.ProfileManager.Business.LookupImportProfileBL.GetAllLookupImportProfiles", MDMTraceSource.JobService);
                MDMTraceHelper.StopTraceActivity("MDM.ProfileManager.Business.LookupImportProfileBL.GetAllLookupImportProfiles", MDMTraceSource.JobService);
            }

            return lkpImportProfileCollection;
        }

        #endregion Get

        #region Private Methods

        #endregion 
    }
}
