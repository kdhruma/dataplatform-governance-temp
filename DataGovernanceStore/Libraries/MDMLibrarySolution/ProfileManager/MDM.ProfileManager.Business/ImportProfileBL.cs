using System;
using System.Diagnostics;
using System.Linq;

namespace MDM.ProfileManager.Business
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Imports;
    using MDM.Utility;
    using MDM.Core.Exceptions;
    using MDM.AdminManager.Business;
    using MDM.MessageManager.Business;
    using MDM.BusinessObjects.Diagnostics;

    /// <summary>
    /// Specifies the business logic class for Import Profile
    /// </summary>
    public class ImportProfileBL : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// Field denoting the security principal
        /// </summary>
        private SecurityPrincipal _securityPrincipal = null;


        /// <summary>
        /// Field denoting key value to decide whether to enable or disable the security permissions for import profile.
        /// </summary>
        private Boolean _isImportProfileManagerPermissionsEnabled = true;

        /// <summary>
        /// Indicates current trace settings
        /// </summary>
        private TraceSettings _traceSettings = new TraceSettings();

        #endregion

        #region Constructors

        public ImportProfileBL()
        {
            GetSecurityPrincipal();
            _isImportProfileManagerPermissionsEnabled = AppConfigurationHelper.GetAppConfig<bool>("MDMCenter.ImportProfileManager.Permissions.Enabled", true);

            _traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
        }

        #endregion Constructors

        #region CUD

        /// <summary>
        /// Creates the specified import profile.
        /// </summary>
        /// <param name="importProfile">Indicates the import profile.</param>
        /// <param name="callerContext">Indicates the caller context specifying the application and modules.</param>
        /// <returns>Returns operation result collection when trying to create the given import profile collection</returns>
        public OperationResult Create(ImportProfile importProfile, CallerContext callerContext)
        {
            ValidateInputParameters(importProfile, callerContext);

            importProfile.Action = ObjectAction.Create;
            return Process(importProfile, callerContext);
        }

        /// <summary>
        /// Updates the specified import profile.
        /// </summary>
        /// <param name="importProfile">Indicates the import profile.</param>
        /// <param name="callerContext">Indicates the caller context specifying the application and modules</param>
        /// <returns></returns>
        public OperationResult Update(ImportProfile importProfile, CallerContext callerContext)
        {
            ValidateInputParameters(importProfile, callerContext);

            importProfile.Action = ObjectAction.Update;
            return Process(importProfile, callerContext);
        }

        /// <summary>
        /// Deletes the specified import profile.
        /// </summary>
        /// <param name="importProfile">Indicates the import profile.</param>
        /// <param name="callerContext">Indicates the caller context specifying the application and modules</param>
        /// <returns>Returns operation result collection when trying to delete the given import profile collection.</returns>
        public OperationResult Delete(ImportProfile importProfile, CallerContext callerContext)
        {
            ValidateInputParameters(importProfile, callerContext);

            importProfile.Action = ObjectAction.Delete;
            return Process(importProfile, callerContext);
        }

        /// <summary>
        /// Deletes the specified import profile.
        /// </summary>
        /// <param name="importProfiles">Indicates the collection of import profile.</param>
        /// <param name="callerContext">Indicates the caller context specifying the application and modules</param>
        /// <returns>Returns operation result collection after processing the given import profile collection.</returns>
        public OperationResultCollection Process(ImportProfileCollection importProfiles, CallerContext callerContext)
        {
            #region Validation

            if (importProfiles == null)
            {
                const String errorMessage = "ImportProfiles cannot be null";
                throw new MDMOperationException("112456", errorMessage, "ImportProfileBL.Process", String.Empty, "Process");
            }

            ValidateCallerContext(callerContext);

            #endregion Validation

            OperationResultCollection operationResults = new OperationResultCollection();

            foreach (ImportProfile profile in importProfiles)
            {
                operationResults.Add(this.Process(profile, callerContext));
            }

            return operationResults;
        }

        #endregion CUD

        #region Get

        /// <summary>
        /// Gets the specified profile by id.
        /// </summary>
        /// <param name="profileId">Indicates the profile id.</param>
        /// <param name="callerContext">Indicates the caller context specifying the application and modules.</param>
        /// <returns>Returns the import profile based on profile Id</returns>
        public ImportProfile Get(Int32 profileId, CallerContext callerContext)
        {
      
            #region Check input parameters

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

            MDMTraceHelper.StartTraceActivity("MDM.ProfileManager.Business.ImportProfileBL.Get-ById", MDMTraceSource.JobService, false);
            MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "Start : MDM.ProfileManager.Business.ImportProfileBL.Get-ById", MDMTraceSource.JobService);

            ImportProfile importProfile = null;
            JobProfile jobProfile = new JobProfileBL().Get(profileId, callerContext.Application, MDMCenterModules.Import);

            if (jobProfile != null)
            {
                importProfile = new ImportProfile(jobProfile.ProfileDataXml);

                importProfile.Id = jobProfile.Id;
                importProfile.Name = jobProfile.Name;
            }

            MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "End : MDM.ProfileManager.Business.ImportProfileBL.Get", MDMTraceSource.JobService);
            MDMTraceHelper.StopTraceActivity("MDM.ProfileManager.Business.ImportProfileBL.Get", MDMTraceSource.JobService);

            return importProfile;
        }

        /// <summary>
        /// Gets the specified profile by name.
        /// </summary>
        /// <param name="profileName">Indicates the name of the profile.</param>
        /// <param name="callerContext">Indicates the caller context specifying the application and modules.</param>
        /// <returns>Returns the import profile by profile Name</returns>
        public ImportProfile Get(String profileName, CallerContext callerContext)
        {
            #region Check input parameters

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

            MDMTraceHelper.StartTraceActivity("MDM.ProfileManager.Business.ImportProfileBL.Get-ByName", MDMTraceSource.JobService, false);
            MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "Start : MDM.ProfileManager.Business.ImportProfileBL.Get-ByName", MDMTraceSource.JobService);

            ImportProfile importProfile = null;
            JobProfile jobProfile = new JobProfileBL().Get(profileName, callerContext.Application, MDMCenterModules.Import);

            if (jobProfile != null)
            {
                importProfile = new ImportProfile(jobProfile.ProfileDataXml); //TODO: Need a way to create job profile..
                //importProfile = (ImportProfile)jobProfile; //TODO: Need a way to create job profile..

                importProfile.Id = jobProfile.Id;
                importProfile.Name = jobProfile.Name;
            }

            MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "End : MDM.ProfileManager.Business.ImportProfileBL.Get-ByName", MDMTraceSource.JobService);
            MDMTraceHelper.StopTraceActivity("MDM.ProfileManager.Business.ImportProfileBL.Get-ByName", MDMTraceSource.JobService);

            return importProfile;
        }

        /// <summary>
        /// Gets all import profiles.
        /// </summary>
        /// <param name="callerContext">Indicates the caller context specifying the application and modules.</param>
        /// <returns></returns>
        public ImportProfileCollection GetAll(CallerContext callerContext)
        {
            DiagnosticActivity activity = new DiagnosticActivity();

            if (_traceSettings.IsBasicTracingEnabled)
            {   
                activity.Start(new ExecutionContext(callerContext, new CallDataContext(), new SecurityContext(), string.Empty));
            }

            #region Check input parameters

            ValidateCallerContext(callerContext);

            #endregion

            ImportProfileCollection importProfileCollection = new ImportProfileCollection();
            JobProfileCollection jobProfileCollection = new JobProfileBL().GetAll(callerContext.Application, MDMCenterModules.Import);

            if (jobProfileCollection != null)
            {
                foreach (JobProfile jobProfile in jobProfileCollection)
                {
                    ImportProfile importProfile = new ImportProfile(jobProfile.ProfileDataXml); //TODO: Need a way to create job profile..

                    importProfile.Id = jobProfile.Id;
                    importProfile.Name = jobProfile.Name;
                    importProfile.ProfileDomain = jobProfile.ProfileDomain;
                    importProfile.FileType = jobProfile.FileType;
                    importProfile.IsSystemProfile = jobProfile.IsSystemProfile;
                    importProfile.JobType = jobProfile.JobType;
                    importProfile.LastModified = jobProfile.LastModified;

                    importProfileCollection.Add(importProfile);
                }
            }

            if (_traceSettings.IsBasicTracingEnabled) activity.Stop();

            return importProfileCollection;
        }

        /// <summary>
        /// Get Profile type by id
        /// </summary>
        /// <param name="profileId">Indicates the identifier of profile</param>
        /// <param name="callerContext">Indicates the caller context specifying the application and modules.</param>
        /// <param name="applySecurity">Indicates whether security should be applied or not</param>
        /// <returns>Returns the type of profile based on profile Id.</returns>
        public String GetProfileType(Int32 profileId, CallerContext callerContext, Boolean applySecurity = true)
        {
            #region Check input parameters

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

            MDMTraceHelper.StartTraceActivity("MDM.ProfileManager.Business.ImportProfileBL.GetProfileType", MDMTraceSource.JobService, false);
            MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "Start : MDM.ProfileManager.Business.ImportProfileBL.GetProfileType", MDMTraceSource.JobService);

            #region Populate and Validate User Permission
            
             //If _isImportProfileManagerPermissionsEnabled key value is true, then it follows the security permissions, otherwise bypass the permissions.
             //Added this key to bypass the security permissions for VP for import profile.
             if (_isImportProfileManagerPermissionsEnabled && applySecurity && !ValidateUserPermission())
             {
                throw new MDMOperationException("101146", "You do not have sufficient permission to perform this operation", "ImportProfileManager.GetAll", String.Empty, "Get");
             }
           
            #endregion Validate User Permission

            JobProfile jobProfile = new JobProfileBL().Get(profileId, callerContext.Application, MDMCenterModules.Import);

            MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "End : MDM.ProfileManager.Business.ImportProfileBL.GetProfileType", MDMTraceSource.JobService);
            MDMTraceHelper.StopTraceActivity("MDM.ProfileManager.Business.ImportProfileBL.GetProfileType", MDMTraceSource.JobService);

            return jobProfile == null ? String.Empty : jobProfile.FileType;
        }

        #endregion Get

        #region Private Methods

        private OperationResult Process(ImportProfile profile, CallerContext callerContext)
        {
            #region Validate User Permission

            //If _isImportProfileManagerPermissionsEnabled key value is true, then it follows the security permissions, otherwise bypass the permissions.
            //Added this key to bypass the security permissions for VP for import profile.
            if (_isImportProfileManagerPermissionsEnabled && !ValidateUserPermission())
            {
                OperationResult permissionOperationResult = new OperationResult();
                PopulateOperationResult(profile, permissionOperationResult, callerContext, true);
                return permissionOperationResult;
            }
            
            #endregion Validate User Permission

            OperationResultCollection operationResults = new OperationResultCollection();

            if (String.IsNullOrWhiteSpace(callerContext.ProgramName))
            {
                callerContext.ProgramName = "MDM.ProfileManager.Business.ImportProfileBL.Process";
            }
            if (callerContext.Module == MDMCenterModules.Unknown)
            {
                callerContext.Module = MDMCenterModules.Import;
            }

            JobProfileBL jobProfileBL = new JobProfileBL();
            profile.ProfileDataXml = profile.ToXml();

            operationResults = jobProfileBL.Process(new JobProfileCollection() { (JobProfile)profile }, callerContext);
            //Here expecting that jobProfileBL.Process() would have done the errorCode to errorMessage conversion so not doing here.
            return operationResults.FirstOrDefault();
        }

        private void ValidateInputParameters(ImportProfile importProfile, CallerContext callerContext)
        {
            if (importProfile == null)
            {
                const String errorMessage = "ImportProfile cannot be null";
                throw new MDMOperationException("112455", errorMessage, "ImportProfileBL.Process", String.Empty, "Process");
            }

            ValidateCallerContext(callerContext);
        }

        private void ValidateCallerContext(CallerContext callerContext)
        {
            if (callerContext == null)
            {
                const String errorMessage = "CallerContext cannot be null";
                throw new MDMOperationException("111846", errorMessage, "ImportProfileBL.Process", String.Empty, "Process");
            }
        }

        private void GetSecurityPrincipal()
        {
            if (_securityPrincipal == null)
            {
                _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            }
        }

        private static void PopulateOperationResult(ImportProfile importProfile, OperationResult importProfileProcessOperationResult, CallerContext callerContext, Boolean populatePermissionError = false)
        {
            if (populatePermissionError)
            {
                
                LocaleMessage localeMessage = null;
                LocaleMessageBL localeMessageBL = new LocaleMessageBL();
                String profileReference =  String.IsNullOrEmpty(importProfile.ReferenceId)
                ? importProfile.Name
                : importProfile.ReferenceId;
                Object[] param = new Object[] { importProfile.Action.ToString(), profileReference};
                
                localeMessage = localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "112291", param, false, callerContext);

                String errorMessage = localeMessage.Message;
                importProfileProcessOperationResult.AddOperationResult("112291", errorMessage, OperationResultType.Error);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, errorMessage, MDMTraceSource.JobService);
            }
            else
            {
                if (importProfile.Action == ObjectAction.Create)
                {
                    if (importProfileProcessOperationResult.ReturnValues.Any())
                    {
                        importProfileProcessOperationResult.Id =
                            Convert.ToInt32(importProfileProcessOperationResult.ReturnValues[0]);
                    }
                }
                else
                {
                    importProfileProcessOperationResult.Id = importProfile.Id;
                }
            }

            importProfileProcessOperationResult.ReferenceId = String.IsNullOrEmpty(importProfile.ReferenceId)
                ? importProfile.Name
                : importProfile.ReferenceId;

        }

        private Boolean ValidateUserPermission()
        {
            Permission permission = null;
            DataSecurityBL dataSecurityManager = new DataSecurityBL();
            Int32 objectTypeId = (Int32)ObjectType.Catalog;

            PermissionContext permissionContext = new PermissionContext(0, 0, 0, 0, 0, 0, 0, 0, _securityPrincipal.CurrentUserId, 0);
            permission = dataSecurityManager.GetMDMObjectPermission(0, objectTypeId, ObjectType.Catalog.ToString(), permissionContext);

            return (permission == null ? false : permission.PermissionSet.Contains(UserAction.Import));
        }

        #endregion
    }
}
